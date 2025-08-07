Public Class Bale_Transfer
    Implements Interface_MDIActions

    Private con As New SqlClient.SqlConnection(Common_Procedures.Connection_String)
    Private con1 As New SqlClient.SqlConnection(Common_Procedures.ConnectionString_CompanyGroupdetails)
    Private FrmLdSTS As Boolean = False
    Private New_Entry As Boolean = False
    Private Insert_Entry As Boolean = False
    Private Filter_Status As Boolean = False
    Private Filter_RowNo As Integer = -1
    Private Pk_Condition As String = "BLTRF-"
    Private NoCalc_Status As Boolean = False
    Private Prec_ActCtrl As New Control
    Private vcbo_KeyDwnVal As Double
    Private vCbo_ItmNm As String
    Private WithEvents dgtxt_Details As New DataGridViewTextBoxEditingControl

    Private prn_HdDt As New DataTable
    Private prn_DetDt As New DataTable
    Private prn_PageNo As Integer
    Private prn_DetIndx As Integer
    Private prn_DetAr(500, 500, 10) As String
    Private prn_DetMxIndx As Integer
    Private prn_NoofBmDets As Integer
    Private prn_DetSNo As Integer
    Private prn_HdIndx As Integer
    Private prn_HdIndx1 As Integer
    Private prn_HdMxIndx As Integer
    Private prn_Count As Integer
    Private prn_HdAr(100, 10) As String
    Private prn_HeadIndx As Integer
    Private prn_Prev_HeadIndx As Integer
    Private prn_InpOpts As String = ""
    Private prn_OriDupTri As String = ""
    Private prn_TotCopies As Integer = 0
    Private prn_DetAr1(500, 10) As String
    Private prn_DetDt1 As New DataTable
    Private prn_BLDetAr(1000, 10) As String
    Private prn_TotBlMtr As Single = 0
    Private prn_TotBlWgt As Single = 0
    Private prn_TotBls As Integer = 0
    Private prn_NoofBaleDets As Integer
    Private prn_BaleCode1 As String = ""
    Private prn_BaleCode2 As String = ""
    Private prn_TotalBales As Integer = 0
    Private prn_TotalPcs As String = ""
    Private prn_TotalMtrs As String = ""
    Private prn_TotalWgt As String = ""

    Public Shared EntFnYrCode As String = ""

    Public vmskOldText As String = ""
    Public vmskSelStrt As Integer = -1
    Public vmskLrText As String = ""
    Public vmskLrStrt As Integer = -1


    Private Sub clear()

        NoCalc_Status = True

        New_Entry = False
        Insert_Entry = False

        pnl_Back.Enabled = True
        pnl_Filter.Visible = False
        pnl_Selection.Visible = False
        pnl_BaleSelection.Visible = False
        pnl_BaleSelection_ToolTip.Visible = False
        pnl_Print.Visible = False

        vmskOldText = ""
        vmskSelStrt = -1

        vmskLrText = ""
        vmskLrStrt = -1


        lbl_RefNo.Text = ""
        lbl_RefNo.ForeColor = Color.Black

        msk_date.Text = ""
        dtp_Date.Text = ""
        cbo_TransferGodownTo.Text = ""
        cbo_Transport.Text = ""
        cbo_DespTo.Text = ""
        cbo_Grid_ClothName.Text = ""
        cbo_Grid_Clothtype.Text = ""
        cbo_RollBundle.Text = "BALE"
        cbo_Vechile.Text = ""
        cbo_TransferGodownFrom.Text = "GODOWN"

        txt_Freight.Text = ""
        txt_DelvAdd1.Text = ""
        txt_DelvAdd2.Text = ""
        txt_LrNo.Text = ""

        dtp_LrDate.Text = ""
        msk_LrDate.Text = ""
        txt_JJFormNo.Text = ""
        lbl_UserName.Text = "USER : " & Trim(UCase(Common_Procedures.User_IdNoToName(con1, Common_Procedures.User.IdNo)))

        txt_Note.Text = ""

        cbo_TransferGodownTo.Enabled = True
        cbo_TransferGodownTo.BackColor = Color.White

        cbo_DespTo.Enabled = True
        cbo_DespTo.BackColor = Color.White

        cbo_Transport.Enabled = True
        cbo_Transport.BackColor = Color.White

        txt_DelvAdd1.Enabled = True
        txt_DelvAdd1.BackColor = Color.White

        txt_DelvAdd2.Enabled = True
        txt_DelvAdd2.BackColor = Color.White

        msk_date.Enabled = True
        msk_date.BackColor = Color.White

        txt_LrNo.Enabled = True
        txt_LrNo.BackColor = Color.White

        cbo_Grid_ClothName.Enabled = True
        cbo_Grid_ClothName.BackColor = Color.White

        cbo_Grid_Clothtype.Enabled = True
        cbo_Grid_Clothtype.BackColor = Color.White

        dgv_Details.Rows.Clear()
        dgv_Details_Total.Rows.Clear()
        dgv_Details_Total.Rows.Add()

        dgv_BaleDetails.Rows.Clear()

        If Filter_Status = False Then
            dtp_Filter_Fromdate.Text = Common_Procedures.Company_FromDate
            dtp_Filter_ToDate.Text = Common_Procedures.Company_ToDate
            cbo_Filter_PartyName.Text = ""
            cbo_Filter_PartyName.SelectedIndex = -1
            cbo_Filter_ClothName.Text = ""
            cbo_Filter_ClothName.SelectedIndex = -1
            dgv_Filter_Details.Rows.Clear()
        End If

        Grid_Cell_DeSelect()

        cbo_Grid_ClothName.Visible = False
        cbo_Grid_Clothtype.Visible = False

        NoCalc_Status = False

    End Sub

    Private Sub ControlGotFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim txtbx As TextBox
        Dim combobx As ComboBox
        Dim Msktxbx As MaskedTextBox

        On Error Resume Next

        If TypeOf Me.ActiveControl Is TextBox Or TypeOf Me.ActiveControl Is ComboBox Or TypeOf Me.ActiveControl Is MaskedTextBox Or TypeOf Me.ActiveControl Is Button Then
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

        If Me.ActiveControl.Name <> cbo_Grid_ClothName.Name Then
            cbo_Grid_ClothName.Visible = False
        End If
        If Me.ActiveControl.Name <> cbo_Grid_Clothtype.Name Then
            cbo_Grid_Clothtype.Visible = False
        End If
        If Me.ActiveControl.Name <> dgv_Details.Name And Not (TypeOf ActiveControl Is DataGridViewTextBoxEditingControl) Then
            pnl_BaleSelection_ToolTip.Visible = False
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
                Prec_ActCtrl.BackColor = Color.FromArgb(41, 57, 85)
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

    Private Sub Bale_Transfer_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable

        Try

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_TransferGodownTo.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "LEDGER" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_TransferGodownTo.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Transport.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "LEDGER" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Transport.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If
            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Grid_ClothName.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "CLOTH NAME" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Grid_ClothName.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If
            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Grid_Clothtype.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "CLOTH TYPE" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Grid_Clothtype.Text = Trim(Common_Procedures.Master_Return.Return_Value)
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
            'MessageBox.Show(ex.Message, "DOES NOT SHOW...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

        FrmLdSTS = False

    End Sub

    Private Sub Bale_Transfer_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        On Error Resume Next
        con.Close()
        con.Dispose()
        Common_Procedures.Last_Closed_FormName = Me.Name
    End Sub

    Private Sub Bale_Transfer_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        Try

            If Asc(e.KeyChar) = 27 Then

                'If MessageBox.Show("Do you want to Close?", "FOR CLOSING ENTRY...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) <> Windows.Forms.DialogResult.Yes Then
                '    Exit Sub
                'End If

                If pnl_Filter.Visible = True Then
                    btn_Filter_Close_Click(sender, e)
                    Exit Sub

                ElseIf pnl_Selection.Visible = True Then
                    btn_Close_Selection_Click(sender, e)
                    Exit Sub

                ElseIf pnl_BaleSelection.Visible = True Then
                    btn_Close_BaleSelection_Click(sender, e)
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

    Private Sub Bale_Transfer_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim dt3 As New DataTable
        Dim dt4 As New DataTable
        Dim dt5 As New DataTable
        Dim dt6 As New DataTable
        Dim dt7 As New DataTable
        Dim dt8 As New DataTable
        Dim OpYrCode As String = ""
        Me.Text = ""

        con.Open()

        If Trim(UCase(Common_Procedures.ClothDelivery_Opening_OR_Entry)) = "OPENING" Then
            OpYrCode = Microsoft.VisualBasic.Left(Trim(Common_Procedures.FnRange), 4)
            OpYrCode = Trim(Mid(Val(OpYrCode) - 1, 3, 2)) & "-" & Trim(Microsoft.VisualBasic.Right(OpYrCode, 2))

            EntFnYrCode = OpYrCode
            btn_BaleSelection.Visible = False


        Else

            EntFnYrCode = Common_Procedures.FnYearCode
            btn_BaleSelection.Visible = True

        End If

        'If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1267" Then '||BRT SPINNERS PRIVATE LIMITED (FABRIC DIVISION)  
        '    txt_DelvAdd1.Height = 23
        '    txt_DelvAdd2.Height = 23
        '    txt_DelvAdd1.Width = 170
        '    txt_DelvAdd2.Width = 170
        '    txt_DelvAdd1.Left = 73
        '    txt_DelvAdd2.Left = 259
        '    lbl_Godown.Visible = True
        '    cbo_Godown.Visible = True
        'End If


        da = New SqlClient.SqlDataAdapter("select Ledger_DisplayName from Ledger_AlaisHead Where ( Ledger_IdNo = 0 or (Ledger_Type = '' and ( AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14) ) ) order by Ledger_DisplayName", con)
        da.Fill(dt1)
        cbo_TransferGodownTo.DataSource = dt1
        cbo_TransferGodownTo.DisplayMember = "Ledger_DisplayName"

        da = New SqlClient.SqlDataAdapter("select Ledger_DisplayName from Ledger_AlaisHead where (Ledger_IdNo = 0 or Ledger_Type = 'TRANSPORT') order by Ledger_DisplayName", con)
        da.Fill(dt2)
        cbo_Transport.DataSource = dt2
        cbo_Transport.DisplayMember = "Ledger_DisplayName"

        da = New SqlClient.SqlDataAdapter("select Cloth_Name from Cloth_Head order by Cloth_Name", con)
        da.Fill(dt4)
        cbo_Grid_ClothName.DataSource = dt4
        cbo_Grid_ClothName.DisplayMember = "Cloth_Name"

        da = New SqlClient.SqlDataAdapter("select ClothType_Name from ClothType_Head order by ClothType_Name", con)
        da.Fill(dt5)
        cbo_Grid_Clothtype.DataSource = dt5
        cbo_Grid_Clothtype.DisplayMember = "ClothType_Name"

        da = New SqlClient.SqlDataAdapter("select distinct(Despatch_To) from Bale_Transfer_Head order by Despatch_To", con)
        da.Fill(dt6)
        cbo_DespTo.DataSource = dt6
        cbo_DespTo.DisplayMember = "Despatch_To"


        cbo_RollBundle.Items.Clear()
        cbo_RollBundle.Items.Add(" ")
        cbo_RollBundle.Items.Add("ROLL")
        cbo_RollBundle.Items.Add("BUNDLE")
        cbo_RollBundle.Items.Add("BALE")

        pnl_Filter.Visible = False
        pnl_Filter.Left = (Me.Width - pnl_Filter.Width) \ 2
        pnl_Filter.Top = (Me.Height - pnl_Filter.Height) \ 2
        pnl_Filter.BringToFront()

        pnl_Selection.Visible = False
        pnl_Selection.Left = (Me.Width - pnl_Selection.Width) \ 2
        pnl_Selection.Top = (Me.Height - pnl_Selection.Height) \ 2
        pnl_Selection.BringToFront()

        pnl_BaleSelection.Visible = False
        pnl_BaleSelection.Left = (Me.Width - pnl_BaleSelection.Width) \ 2
        pnl_BaleSelection.Top = (Me.Height - pnl_BaleSelection.Height) \ 2
        pnl_BaleSelection.BringToFront()

        pnl_Print.Visible = False
        pnl_Print.Left = (Me.Width - pnl_Print.Width) \ 2
        pnl_Print.Top = (Me.Height - pnl_Print.Height) \ 2
        pnl_Print.BringToFront()

        dgv_BaleDetails.Visible = False

        pnl_BaleSelection_ToolTip.Visible = False


        btn_UserModification.Visible = False
        If Common_Procedures.settings.User_Modifications_Show_Status = 1 Then
            If Val(Common_Procedures.User.IdNo) = 1 Or Common_Procedures.User.Show_UserModification_Status = 1 Then
                btn_UserModification.Visible = True
            End If
        End If

        AddHandler cbo_TransferGodownTo.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_DespTo.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Grid_ClothName.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Grid_Clothtype.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Transport.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Freight.GotFocus, AddressOf ControlGotFocus
        AddHandler msk_date.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_LrNo.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_DelvAdd1.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_DelvAdd2.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Note.GotFocus, AddressOf ControlGotFocus
        AddHandler dtp_Filter_Fromdate.GotFocus, AddressOf ControlGotFocus
        AddHandler dtp_Filter_ToDate.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Filter_PartyName.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Filter_ClothName.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_save.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_Print.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_close.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_Print_Bale.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_Print_Bundle.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_Print_Cancel.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_Print_Delivery.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_Print_FormJJ.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_JJFormNo.GotFocus, AddressOf ControlGotFocus
        AddHandler msk_LrDate.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_RollBundle.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Vechile.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_TransferGodownFrom.GotFocus, AddressOf ControlGotFocus


        AddHandler cbo_TransferGodownTo.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Transport.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Grid_ClothName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_DespTo.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Grid_Clothtype.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Freight.LostFocus, AddressOf ControlLostFocus
        AddHandler msk_date.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_LrNo.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_DelvAdd1.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_DelvAdd2.LostFocus, AddressOf ControlLostFocus
        AddHandler dtp_Filter_Fromdate.LostFocus, AddressOf ControlLostFocus
        AddHandler dtp_Filter_ToDate.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Filter_PartyName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Filter_ClothName.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_save.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_Print.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_close.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_Print_Bale.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_Print_Bundle.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_Print_Cancel.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_Print_Delivery.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_Print_FormJJ.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_JJFormNo.LostFocus, AddressOf ControlLostFocus
        AddHandler msk_LrDate.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_RollBundle.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Vechile.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_TransferGodownFrom.LostFocus, AddressOf ControlLostFocus

        'AddHandler msk_date.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Freight.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_DelvAdd1.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_LrNo.KeyDown, AddressOf TextBoxControlKeyDown
        ' AddHandler msk_date.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler dtp_Filter_Fromdate.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler dtp_Filter_ToDate.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_JJFormNo.KeyDown, AddressOf TextBoxControlKeyDown
        'AddHandler msk_LrDate.KeyDown, AddressOf TextBoxControlKeyDown

        AddHandler msk_date.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Freight.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_DelvAdd1.KeyPress, AddressOf TextBoxControlKeyPress
        ' AddHandler msk_date.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_LrNo.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_JJFormNo.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler dtp_Filter_Fromdate.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler dtp_Filter_ToDate.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler msk_LrDate.KeyPress, AddressOf TextBoxControlKeyPress

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
            MessageBox.Show(ex.Message, "DOES NOT CLOSE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Protected Overrides Function ProcessCmdKey(ByRef msg As System.Windows.Forms.Message, ByVal keyData As System.Windows.Forms.Keys) As Boolean
        Dim dgv1 As New DataGridView

        On Error Resume Next

        If ActiveControl.Name = dgv_Details.Name Or TypeOf ActiveControl Is DataGridViewTextBoxEditingControl Then

            dgv1 = Nothing

            If ActiveControl.Name = dgv_Details.Name Then
                dgv1 = dgv_Details

            ElseIf dgv_Details.IsCurrentRowDirty = True Then
                dgv1 = dgv_Details

            ElseIf pnl_Back.Enabled = True Then
                dgv1 = dgv_Details

            End If

            If IsNothing(dgv1) = False Then

                With dgv1


                    If keyData = Keys.Enter Or keyData = Keys.Down Then

                        If .CurrentCell.ColumnIndex > 7 Then

                            If .CurrentCell.RowIndex = .RowCount - 1 Then
                                cbo_Transport.Focus()

                            Else

                                .CurrentCell = .Rows(.CurrentCell.RowIndex + 1).Cells(1)

                            End If
                        ElseIf .CurrentCell.ColumnIndex = 7 Then

                            .CurrentCell = .Rows(.CurrentRow.Index).Cells(13)
                        Else
                            .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 1)

                        End If

                        Return True

                    ElseIf keyData = Keys.Up Then

                        If .CurrentCell.ColumnIndex <= 1 Then
                            If .CurrentCell.RowIndex = 0 Then
                                If cbo_TransferGodownFrom.Visible = True Then
                                    cbo_TransferGodownFrom.Focus()
                                Else
                                    txt_DelvAdd2.Focus()
                                End If

                            Else
                                .CurrentCell = .Rows(.CurrentCell.RowIndex - 1).Cells(7)

                            End If

                        ElseIf .CurrentCell.ColumnIndex = 17 Then

                            .CurrentCell = .Rows(.CurrentRow.Index).Cells(8)

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
        Dim LockSTS As Boolean = False
        Dim n As Integer
        Dim SNo As Integer

        If Val(no) = 0 Then Exit Sub

        clear()

        NoCalc_Status = True

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(no) & "/" & Trim(EntFnYrCode)

        Try

            da1 = New SqlClient.SqlDataAdapter("select a.* from Bale_Transfer_Head a Where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Bale_Transfer_Code = '" & Trim(NewCode) & "'", con)
            dt1 = New DataTable
            da1.Fill(dt1)

            If dt1.Rows.Count > 0 Then

                lbl_RefNo.Text = dt1.Rows(0).Item("Bale_Transfer_No").ToString
                dtp_Date.Text = dt1.Rows(0).Item("Bale_Transfer_Date").ToString
                msk_date.Text = dtp_Date.Text

                cbo_TransferGodownTo.Text = Common_Procedures.Ledger_IdNoToName(con, Val(dt1.Rows(0).Item("Ledger_IdNo").ToString))
                cbo_Transport.Text = Common_Procedures.Ledger_IdNoToName(con, Val(dt1.Rows(0).Item("Transport_IdNo").ToString))

                cbo_DespTo.Text = Trim(UCase(dt1.Rows(0).Item("Despatch_To").ToString))

                txt_LrNo.Text = dt1.Rows(0).Item("Lr_No").ToString
                msk_LrDate.Text = dt1.Rows(0).Item("Lr_Date").ToString

                txt_DelvAdd1.Text = dt1.Rows(0).Item("Delivery_Address1").ToString
                txt_DelvAdd2.Text = dt1.Rows(0).Item("Delivery_Address2").ToString
                txt_Freight.Text = dt1.Rows(0).Item("Freight_Amount").ToString
                txt_Note.Text = dt1.Rows(0).Item("Note").ToString
                txt_JJFormNo.Text = dt1.Rows(0).Item("JJ_FormNo").ToString

                cbo_RollBundle.Text = dt1.Rows(0).Item("Packing_Type").ToString
                cbo_Vechile.Text = dt1.Rows(0).Item("Vechile_No").ToString
                cbo_TransferGodownFrom.Text = Common_Procedures.Ledger_IdNoToName(con, Val(dt1.Rows(0).Item("WareHouse_IdNo").ToString))

                lbl_UserName.Text = "USER : " & Trim(UCase(Common_Procedures.User_IdNoToName(con1, Val(dt1.Rows(0).Item("User_IdNo").ToString))))

                da2 = New SqlClient.SqlDataAdapter("Select a.*, b.Cloth_Name, c.ClothType_Name from Bale_Transfer_Details a LEFT OUTER JOIN Cloth_Head b ON a.Cloth_IdNo = b.Cloth_IdNo LEFT OUTER JOIN ClothType_Head c ON a.ClothType_IdNo = c.ClothType_IdNo Where a.Bale_Transfer_Code = '" & Trim(NewCode) & "' Order by a.sl_no", con)
                dt2 = New DataTable
                da2.Fill(dt2)

                With dgv_Details

                    .Rows.Clear()
                    SNo = 0

                    If dt2.Rows.Count > 0 Then

                        For i = 0 To dt2.Rows.Count - 1

                            n = .Rows.Add()

                            SNo = SNo + 1

                            .Rows(n).Cells(0).Value = Val(SNo)
                            .Rows(n).Cells(1).Value = dt2.Rows(i).Item("Cloth_Name").ToString
                            .Rows(n).Cells(2).Value = dt2.Rows(i).Item("ClothType_Name").ToString
                            If Val(dt2.Rows(i).Item("Fold_Perc").ToString) <> 0 Then
                                .Rows(n).Cells(3).Value = Val(dt2.Rows(i).Item("Fold_Perc").ToString)
                            End If
                            If Val(dt2.Rows(i).Item("Bales").ToString) <> 0 Then
                                .Rows(n).Cells(4).Value = Val(dt2.Rows(i).Item("Bales").ToString)
                            End If
                            .Rows(n).Cells(5).Value = dt2.Rows(i).Item("Bales_Nos").ToString
                            If Val(dt2.Rows(i).Item("Pcs").ToString) <> 0 Then
                                .Rows(n).Cells(6).Value = Val(dt2.Rows(i).Item("Pcs").ToString)
                            End If
                            .Rows(n).Cells(7).Value = Format(Val(dt2.Rows(i).Item("Meters").ToString), "########0.00")
                            .Rows(n).Cells(8).Value = dt2.Rows(i).Item("ClothSales_Order_Code").ToString
                            .Rows(n).Cells(9).Value = dt2.Rows(i).Item("ClothSales_Order_SlNo").ToString
                            .Rows(n).Cells(10).Value = dt2.Rows(i).Item("Bale_Transfer_SlNo").ToString
                            .Rows(n).Cells(11).Value = Val(dt2.Rows(i).Item("Invoice_Meters").ToString) + Val(dt2.Rows(i).Item("Return_Meters").ToString)
                            .Rows(n).Cells(12).Value = dt2.Rows(i).Item("PackingSlip_Codes").ToString
                            .Rows(n).Cells(13).Value = Format(Val(dt2.Rows(i).Item("Rate").ToString), "#######0.00")

                            If Val(.Rows(n).Cells(11).Value) <> 0 Then
                                For j = 0 To .ColumnCount - 1
                                    .Rows(n).Cells(j).Style.BackColor = Color.LightGray
                                Next j
                                LockSTS = True
                            End If

                        Next i

                    End If

                    If .Rows.Count = 0 Then
                        .Rows.Add()

                    Else

                        n = .Rows.Count - 1
                        If Trim(.Rows(n).Cells(1).Value) = "" And Val(.Rows(n).Cells(7).Value) = 0 Then
                            .Rows(n).Cells(10).Value = ""
                            If Val(.Rows(n).Cells(10).Value) = 0 Then
                                If n = 0 Then
                                    .Rows(n).Cells(10).Value = 1
                                Else
                                    .Rows(n).Cells(10).Value = Val(.Rows(n - 1).Cells(10).Value) + 1
                                End If
                            End If
                        End If

                    End If

                End With

                With dgv_Details_Total
                    If .RowCount = 0 Then .Rows.Add()
                    .Rows(0).Cells(4).Value = Val(dt1.Rows(0).Item("Total_Bales").ToString)
                    .Rows(0).Cells(6).Value = Val(dt1.Rows(0).Item("Total_Pcs").ToString)
                    .Rows(0).Cells(7).Value = Format(Val(dt1.Rows(0).Item("Total_Meters").ToString), "########0.00")
                End With


                da2 = New SqlClient.SqlDataAdapter("Select a.* from Packing_Slip_Head a Where a.Delivery_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' Order by a.Delivery_DetailsSlNo, a.Delivery_No, a.Packing_Slip_Date, a.for_OrderBy, a.Packing_Slip_No, a.Packing_Slip_Code", con)
                dt2 = New DataTable
                da2.Fill(dt2)

                With dgv_BaleDetails

                    .Rows.Clear()
                    SNo = 0

                    If dt2.Rows.Count > 0 Then

                        For i = 0 To dt2.Rows.Count - 1

                            n = .Rows.Add()

                            SNo = SNo + 1

                            .Rows(n).Cells(0).Value = Val(dt2.Rows(i).Item("Delivery_DetailsSlNo").ToString)
                            .Rows(n).Cells(1).Value = dt2.Rows(i).Item("Packing_Slip_No").ToString
                            .Rows(n).Cells(2).Value = Val(dt2.Rows(i).Item("Total_Pcs").ToString)
                            .Rows(n).Cells(3).Value = Val(dt2.Rows(i).Item("Total_Meters").ToString)
                            .Rows(n).Cells(4).Value = Val(dt2.Rows(i).Item("Total_Weight").ToString)
                            .Rows(n).Cells(5).Value = dt2.Rows(i).Item("Packing_Slip_Code").ToString
                            .Rows(n).Cells(6).Value = dt2.Rows(i).Item("Bale_Bundle").ToString

                        Next i

                    End If

                End With


            End If

            Grid_Cell_DeSelect()

            If LockSTS = True Then
                cbo_TransferGodownTo.Enabled = False
                cbo_TransferGodownTo.BackColor = Color.LightGray

                cbo_DespTo.Enabled = False
                cbo_DespTo.BackColor = Color.LightGray

                cbo_Transport.Enabled = False
                cbo_Transport.BackColor = Color.LightGray

                txt_DelvAdd1.Enabled = False
                txt_DelvAdd1.BackColor = Color.LightGray

                txt_DelvAdd2.Enabled = False
                txt_DelvAdd2.BackColor = Color.LightGray

                'msk_date.Enabled = False
                'msk_date.BackColor = Color.LightGray

                txt_LrNo.Enabled = False
                txt_LrNo.BackColor = Color.LightGray

                cbo_Grid_ClothName.Enabled = False
                cbo_Grid_ClothName.BackColor = Color.LightGray

                cbo_Grid_Clothtype.Enabled = False
                cbo_Grid_Clothtype.BackColor = Color.LightGray

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT MOVE RECORDS...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Finally

            dt1.Dispose()
            da1.Dispose()

            dt2.Dispose()
            da2.Dispose()

            If msk_date.Visible And msk_date.Enabled Then msk_date.Focus()

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
        vOrdByNo = Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text)


        '  If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.ClothSales_Cloth_Delivery_Entry, "~L~") = 0 And InStr(Common_Procedures.UR.ClothSales_Cloth_Delivery_Entry, "~D~") = 0 Then MessageBox.Show("You have No Rights to Delete", "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error) : Exit Sub

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)
        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.DeletingEntry, Common_Procedures.UR.Bale_Transfer_Entry, New_Entry, Me, con, "Bale_Transfer_Head", "Bale_Transfer_Code", NewCode, "Bale_Transfer_Date", "(Bale_Transfer_Code = '" & Trim(NewCode) & "')") = False Then Exit Sub


        If Val(lbl_Company.Tag) = 0 Then
            MessageBox.Show("Select Company Selection", "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
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

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(EntFnYrCode)

        Da = New SqlClient.SqlDataAdapter("select sum(Invoice_Meters) from Bale_Transfer_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Bale_Transfer_Code = '" & Trim(NewCode) & "'", con)
        Dt1 = New DataTable
        Da.Fill(Dt1)
        If Dt1.Rows.Count > 0 Then
            If IsDBNull(Dt1.Rows(0)(0).ToString) = False Then
                If Val(Dt1.Rows(0)(0).ToString) > 0 Then
                    MessageBox.Show("Already some pieces invoiced for this dc", "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                    Exit Sub
                End If
            End If
        End If

        Da = New SqlClient.SqlDataAdapter("select sum(Return_Meters) from Bale_Transfer_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Bale_Transfer_Code = '" & Trim(NewCode) & "'", con)
        Dt1 = New DataTable
        Da.Fill(Dt1)
        If Dt1.Rows.Count > 0 Then
            If IsDBNull(Dt1.Rows(0)(0).ToString) = False Then
                If Val(Dt1.Rows(0)(0).ToString) > 0 Then
                    MessageBox.Show("Already some pieces Returned for this dc", "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                    Exit Sub
                End If
            End If
        End If
        Dt1.Clear()

        trans = con.BeginTransaction

        Try

            NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(EntFnYrCode)

            cmd.Connection = con
            cmd.Transaction = trans


            Call Common_Procedures.User_Modification_Updation(con, "HEAD", Me.Name, "DELETE", "Bale_Transfer_Head", "Bale_Transfer_Code", Val(lbl_Company.Tag), NewCode, lbl_RefNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, True, "", "", "Bale_Transfer_Code, Company_IdNo, for_OrderBy", trans)

            Call Common_Procedures.User_Modification_Updation(con, "DETAILS", Me.Name, "DELETE", "Bale_Transfer_Details", "Bale_Transfer_Code", Val(lbl_Company.Tag), NewCode, lbl_RefNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, True, "Cloth_IdNo ,ClothType_IdNo , Fold_Perc   ,  Bales ,  Bales_Nos ,  Pcs ,  Meters   ,  ClothSales_Order_code ,   ClothSales_Order_SlNo  ,  Bale_Transfer_SlNo ,  PackingSlip_Codes  , Rate ", "Sl_No", " Bale_Transfer_Code, For_OrderBy, Company_IdNo, Bale_Transfer_No, Bale_Transfer_Date, Ledger_Idno ", trans)



            cmd.CommandText = "Update ClothSales_order_Details set Delivery_Meters = a.Delivery_Meters - b.Meters from ClothSales_order_Details a, Bale_Transfer_Details b Where b.Bale_Transfer_Code = '" & Trim(NewCode) & "' and a.ClothSales_Order_code = b.ClothSales_Order_code and a.ClothSales_Order_SlNo = b.ClothSales_Order_SlNo"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Update Packing_Slip_Head set Delivery_Code = '', Delivery_No = '', Delivery_DetailsSlNo = 0, Delivery_Increment = Delivery_Increment - 1, Delivery_Date = Null Where Delivery_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Packing_Slip_Head Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Bale_Transfer_Code = '" & Trim(NewCode) & "' and Delivery_Code = ''"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Stock_Cloth_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "delete from Bale_Transfer_Details where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Bale_Transfer_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()
            cmd.CommandText = "delete from Bale_Transfer_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Bale_Transfer_Code = '" & Trim(NewCode) & "'"
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

            da = New SqlClient.SqlDataAdapter("select top 1 Bale_Transfer_No from Bale_Transfer_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Bale_Transfer_Code like '%/" & Trim(EntFnYrCode) & "' Order by for_Orderby, Bale_Transfer_No", con)
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
        Dim OrdByNo As Single = 0

        Try

            OrdByNo = Common_Procedures.OrderBy_CodeToValue(Trim(lbl_RefNo.Text))

            da = New SqlClient.SqlDataAdapter("select top 1 Bale_Transfer_No from Bale_Transfer_Head where for_orderby > " & Str(Val(OrdByNo)) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and Bale_Transfer_Code like '%/" & Trim(EntFnYrCode) & "' Order by for_Orderby, Bale_Transfer_No", con)
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
        Dim OrdByNo As Single = 0

        Try

            OrdByNo = Common_Procedures.OrderBy_CodeToValue(Trim(lbl_RefNo.Text))

            da = New SqlClient.SqlDataAdapter("select top 1 Bale_Transfer_No from Bale_Transfer_Head where for_orderby < " & Str(Val(OrdByNo)) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and Bale_Transfer_Code like '%/" & Trim(EntFnYrCode) & "' Order by for_Orderby desc, Bale_Transfer_No desc", con)
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
            da = New SqlClient.SqlDataAdapter("select top 1 Bale_Transfer_No from Bale_Transfer_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Bale_Transfer_Code like '%/" & Trim(EntFnYrCode) & "' Order by for_Orderby desc, Bale_Transfer_No desc", con)
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

            lbl_RefNo.Text = Common_Procedures.get_MaxCode(con, "Bale_Transfer_Head", "Bale_Transfer_Code", "For_OrderBy", "", Val(lbl_Company.Tag), EntFnYrCode)
            lbl_RefNo.ForeColor = Color.Red

            msk_date.Text = Date.Today.ToShortDateString
            Da = New SqlClient.SqlDataAdapter("select top 1 * from Bale_Transfer_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Bale_Transfer_Code like '%/" & Trim(EntFnYrCode) & "' Order by for_Orderby desc, Bale_Transfer_No desc", con)
            Dt1 = New DataTable
            Da.Fill(Dt1)
            If Dt1.Rows.Count > 0 Then
                If Val(Common_Procedures.settings.PreviousEntryDate_ByDefault) = 1 Then '---- M.S Textiles (Tirupur)
                    If Dt1.Rows(0).Item("Bale_Transfer_Date").ToString <> "" Then msk_date.Text = Dt1.Rows(0).Item("Bale_Transfer_Date").ToString
                End If
            End If
            Dt1.Clear()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR NEW RECORD...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Finally

            Dt1.Dispose()
            Da.Dispose()

            If msk_date.Enabled And msk_date.Visible = True Then msk_date.Focus()
            msk_date.SelectionStart = 0

        End Try
    End Sub

    Public Sub open_record() Implements Interface_MDIActions.open_record
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim movno As String, inpno As String
        Dim InvCode As String

        Try

            inpno = InputBox("Enter Dc No.", "FOR FINDING...")

            InvCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(EntFnYrCode)

            Da = New SqlClient.SqlDataAdapter("select Bale_Transfer_No from Bale_Transfer_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Bale_Transfer_Code = '" & Trim(InvCode) & "'", con)
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
                MessageBox.Show("Dc No. does not exists", "DOES NOT FIND...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT FIND...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

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

        'If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.ClothSales_Cloth_Delivery_Entry, "~L~") = 0 And InStr(Common_Procedures.UR.ClothSales_Cloth_Delivery_Entry, "~I~") = 0 Then MessageBox.Show("You have No Rights to DELETE", "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error) : Exit Sub
        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.InsertingEntry, Common_Procedures.UR.Bale_Transfer_Entry, New_Entry, Me) = False Then Exit Sub

        Try

            inpno = InputBox("Enter New Dc No.", "FOR NEW Dc NO. INSERTION...")

            InvCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(EntFnYrCode)

            Da = New SqlClient.SqlDataAdapter("select Bale_Transfer_No from Bale_Transfer_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Bale_Transfer_Code = '" & Trim(InvCode) & "'", con)
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
                    MessageBox.Show("Select Dc No.", "DOES NOT INSERT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

                Else
                    new_record()
                    Insert_Entry = True
                    lbl_RefNo.Text = Trim(UCase(inpno))

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
        Dim tr As SqlClient.SqlTransaction
        Dim NewCode As String = ""
        Dim I As Integer = 0, J As Integer = 0
        Dim clth_ID As Integer = 0
        Dim FP_ID As Integer = 0
        Dim clthtyp_ID As Integer = 0
        Dim Trans_ID As Integer
        Dim vGodnTo_IdNo As Integer = 0
        Dim Sno As Integer = 0
        Dim EntID As String = ""
        Dim PBlNo As String = ""
        Dim Partcls As String = ""
        Dim vTotPcs As Single, vTotMtrs As Single, vTotBals As Single
        Dim Cr_ID As Integer = 0
        Dim Dr_ID As Integer = 0
        Dim Nr As Integer = 0
        Dim DcCd As String = ""
        Dim DcSlNo As Long = 0
        Dim OpYrCode As String = ""
        Dim Usr_ID As Integer = 0
        Dim vGodnFrm_IdNo As Integer = 0
        Dim OrdCd As String = ""
        Dim OrdSlNo As Long = 0
        Dim vNewRollCode As String = ""

        Dim vOrdByNo As String = ""
        vOrdByNo = Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text)


        If Val(lbl_Company.Tag) = 0 Then
            MessageBox.Show("Select Company Selection", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
        End If

        '  If Common_Procedures.UserRight_Check(Common_Procedures.UR.ClothSales_Cloth_Delivery_Entry, New_Entry) = False Then Exit Sub


        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)
        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.SavingEntry, Common_Procedures.UR.Bale_Transfer_Entry, New_Entry, Me, con, "Bale_Transfer_Head", "Bale_Transfer_Code", NewCode, "Bale_Transfer_Date", "(Bale_Transfer_Code = '" & Trim(NewCode) & "')", "(Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Bale_Transfer_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "')", "for_Orderby desc, Bale_Transfer_No desc", dtp_Date.Value.Date) = False Then Exit Sub


        If pnl_Back.Enabled = False Then
            MessageBox.Show("Close Other Windows", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
        End If

        If IsDate(msk_date.Text) = False Then
            MessageBox.Show("Enter Valid Date", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If msk_date.Enabled And msk_date.Visible Then msk_date.Focus()
            Exit Sub
        End If

        If EntFnYrCode = Common_Procedures.FnYearCode Then
            If Not (Convert.ToDateTime(msk_date.Text) >= Common_Procedures.Company_FromDate And Convert.ToDateTime(msk_date.Text) <= Common_Procedures.Company_ToDate) Then
                MessageBox.Show("Date is out of financial range", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                If msk_date.Enabled And msk_date.Visible Then msk_date.Focus()
                Exit Sub
            End If

        End If

        vGodnTo_IdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_TransferGodownTo.Text)
        If vGodnTo_IdNo = 0 Then
            MessageBox.Show("Select Transfer To Godown Name", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If cbo_TransferGodownTo.Enabled Then cbo_TransferGodownTo.Focus()
            Exit Sub
        End If


        vGodnFrm_IdNo = Common_Procedures.Ledger_NameToIdNo(con, cbo_TransferGodownFrom.Text)
        If cbo_TransferGodownFrom.Visible = True Then
            If vGodnFrm_IdNo = 0 Then
                MessageBox.Show("Invalid Transfer From Godown Name", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                If cbo_TransferGodownFrom.Enabled And cbo_TransferGodownFrom.Visible Then cbo_TransferGodownFrom.Focus()
                Exit Sub
            End If
        End If
        If vGodnFrm_IdNo = 0 Then vGodnFrm_IdNo = Common_Procedures.CommonLedger.Godown_Ac

        If vGodnTo_IdNo = vGodnFrm_IdNo Then
            MessageBox.Show("Invalid Godown Name's, Both Godown names should not be equal", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If cbo_TransferGodownTo.Enabled Then cbo_TransferGodownTo.Focus()
            Exit Sub
        End If


        Trans_ID = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Transport.Text)

        For I = 0 To dgv_Details.RowCount - 1

            If Val(dgv_Details.Rows(I).Cells(6).Value) <> 0 Or Val(dgv_Details.Rows(I).Cells(7).Value) <> 0 Then

                clth_ID = Common_Procedures.Cloth_NameToIdNo(con, dgv_Details.Rows(I).Cells(1).Value)
                If clth_ID = 0 Then
                    MessageBox.Show("Select Cloth Name", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                    If dgv_Details.Enabled And dgv_Details.Visible Then
                        dgv_Details.Focus()
                        dgv_Details.CurrentCell = dgv_Details.Rows(I).Cells(1)
                    End If
                    Exit Sub
                End If

                clthtyp_ID = Common_Procedures.ClothType_NameToIdNo(con, dgv_Details.Rows(I).Cells(2).Value)
                If clthtyp_ID = 0 Then
                    MessageBox.Show("Select Cloth Type Name", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                    If dgv_Details.Enabled And dgv_Details.Visible Then
                        dgv_Details.Focus()
                        dgv_Details.CurrentCell = dgv_Details.Rows(I).Cells(2)
                    End If
                    Exit Sub
                End If

                If Val(dgv_Details.Rows(I).Cells(3).Value) = 0 Then
                    dgv_Details.Rows(I).Cells(3).Value = 100
                    'MessageBox.Show("Select Folding", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                    'If dgv_Details.Enabled And dgv_Details.Visible Then
                    '    dgv_Details.Focus()
                    '    dgv_Details.CurrentCell = dgv_Details.Rows(i).Cells(3)
                    'End If
                    'Exit Sub
                End If

                If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1116" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1380" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1446" Then '----Lourdu Matha
                    If Val(dgv_Details.Rows(I).Cells(6).Value) = 0 Then
                        MessageBox.Show("Select Pcs", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                        If dgv_Details.Enabled And dgv_Details.Visible Then
                            dgv_Details.Focus()
                            dgv_Details.CurrentCell = dgv_Details.Rows(I).Cells(6)
                        End If
                        Exit Sub
                    End If

                Else

                    If Val(dgv_Details.Rows(I).Cells(7).Value) = 0 Then
                        MessageBox.Show("Select Meters", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                        If dgv_Details.Enabled And dgv_Details.Visible Then
                            dgv_Details.Focus()
                            dgv_Details.CurrentCell = dgv_Details.Rows(I).Cells(7)
                        End If
                        Exit Sub
                    End If

                End If

            End If

        Next

        NoCalc_Status = False
        Total_Calculation()

        vTotPcs = 0 : vTotMtrs = 0 : vTotBals = 0

        If dgv_Details_Total.RowCount > 0 Then
            vTotBals = Val(dgv_Details_Total.Rows(0).Cells(4).Value())
            vTotPcs = Val(dgv_Details_Total.Rows(0).Cells(6).Value())
            vTotMtrs = Val(dgv_Details_Total.Rows(0).Cells(7).Value())
        End If
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1116" And Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1380" And Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1446" Then '----Lourdu Matha
            If vTotMtrs = 0 Then
                MessageBox.Show("Select METERS", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                If dgv_Details.Enabled And dgv_Details.Visible Then
                    dgv_Details.Focus()
                    dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(7)
                End If
                Exit Sub
            End If
        End If


        If Trim(txt_JJFormNo.Text) <> "" Then
            NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(EntFnYrCode)
            Da = New SqlClient.SqlDataAdapter("select * from Bale_Transfer_Head where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Bale_Transfer_Code <> '" & Trim(NewCode) & "' and Bale_Transfer_Code LIKE '%/" & Trim(EntFnYrCode) & "' and JJ_FormNo = '" & Trim(txt_JJFormNo.Text) & "'", con)
            Dt1 = New DataTable
            Da.Fill(Dt1)
            If Dt1.Rows.Count > 0 Then
                MessageBox.Show("Duplicate JJ Form No", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                If txt_JJFormNo.Enabled And txt_JJFormNo.Visible Then txt_JJFormNo.Focus()
                Exit Sub
            End If
            Dt1.Clear()
        End If

        tr = con.BeginTransaction

        Try

            If Insert_Entry = True Or New_Entry = False Then
                NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(EntFnYrCode)

            Else

                lbl_RefNo.Text = Common_Procedures.get_MaxCode(con, "Bale_Transfer_Head", "Bale_Transfer_Code", "For_OrderBy", "", Val(lbl_Company.Tag), EntFnYrCode, tr)

                NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(EntFnYrCode)

            End If

            cmd.Connection = con
            cmd.Transaction = tr

            cmd.Parameters.Clear()
            cmd.Parameters.AddWithValue("@EntryDate", Convert.ToDateTime(msk_date.Text))

            If New_Entry = True Then

                cmd.CommandText = "Insert into Bale_Transfer_Head (  Bale_Transfer_Code    ,               Company_IdNo       ,     Bale_Transfer_No           ,                        for_OrderBy                                      , Bale_Transfer_Date ,        Ledger_IdNo       ,                   Lr_No       ,          Lr_Date                ,               Despatch_To       ,               Delivery_Address1   ,     Delivery_Address2             ,       Transport_IdNo       ,                   Freight_Amount      ,               Note             ,        Total_Bales         ,          Total_Pcs       ,          Total_Meters     ,                                                    JJ_Form_OrderByNo     ,               JJ_FormNo           ,                                 user_idNo      ,               Packing_Type          ,               Vechile_No         ,         WareHouse_IdNo    ) " & _
                                    "     Values                  ('" & Trim(NewCode) & "' , " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_RefNo.Text) & "' , " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text))) & " ,      @EntryDate       , " & Str(Val(vGodnTo_IdNo)) & " ,  '" & Trim(txt_LrNo.Text) & "', '" & Trim(msk_LrDate.Text) & "' , '" & Trim(cbo_DespTo.Text) & "' , '" & Trim(txt_DelvAdd1.Text) & "' , '" & Trim(txt_DelvAdd2.Text) & "' , " & Str(Val(Trans_ID)) & " ,   " & Str(Val(txt_Freight.Text)) & "  , '" & Trim(txt_Note.Text) & "' , " & Str(Val(vTotBals)) & " , " & Str(Val(vTotPcs)) & ", " & Str(Val(vTotMtrs)) & "," & Str(Val(Common_Procedures.OrderBy_CodeToValue(txt_JJFormNo.Text))) & ", '" & Trim(txt_JJFormNo.Text) & "' , " & Val(Common_Procedures.User.IdNo) & " , '" & Trim(cbo_RollBundle.Text) & "' , '" & Trim(cbo_Vechile.Text) & "' , " & Val(vGodnFrm_IdNo) & "   ) "
                cmd.ExecuteNonQuery()

            Else


                Call Common_Procedures.User_Modification_Updation(con, "HEAD", Me.Name, "OLD", "Bale_Transfer_Head", "Bale_Transfer_Code", Val(lbl_Company.Tag), NewCode, lbl_RefNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, False, "", "", "Bale_Transfer_Code, Company_IdNo, for_OrderBy", tr)

                Call Common_Procedures.User_Modification_Updation(con, "DETAILS", Me.Name, "OLD", "Bale_Transfer_Details", "Bale_Transfer_Code", Val(lbl_Company.Tag), NewCode, lbl_RefNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, False, "Cloth_IdNo ,ClothType_IdNo , Fold_Perc   ,  Bales ,  Bales_Nos ,  Pcs ,  Meters   ,  ClothSales_Order_code ,   ClothSales_Order_SlNo  ,  Bale_Transfer_SlNo ,  PackingSlip_Codes  , Rate ", "Sl_No", " Bale_Transfer_Code, For_OrderBy, Company_IdNo, Bale_Transfer_No, Bale_Transfer_Date, Ledger_Idno ", tr)




                cmd.CommandText = "Update Bale_Transfer_Head set Bale_Transfer_Date = @EntryDate, Ledger_IdNo =  " & Str(Val(vGodnTo_IdNo)) & " , Lr_No    = '" & Trim(txt_LrNo.Text) & "' ,   Lr_Date = '" & Trim(msk_LrDate.Text) & "'  ,   Despatch_To = '" & Trim(cbo_DespTo.Text) & "' ,     Transport_IdNo = " & Str(Val(Trans_ID)) & "       ,  Delivery_Address1 = '" & Trim(txt_DelvAdd1.Text) & "', Delivery_Address2 = '" & Trim(txt_DelvAdd2.Text) & "'  ,   Freight_Amount =  " & Str(Val(txt_Freight.Text)) & "  ,  Note = '" & Trim(txt_Note.Text) & "' , Total_Bales  = " & Str(Val(vTotBals)) & ", Total_Pcs =  " & Str(Val(vTotPcs)) & ", Total_Meters = " & Str(Val(vTotMtrs)) & ", JJ_Form_OrderByNo = " & Str(Val(Common_Procedures.OrderBy_CodeToValue(txt_JJFormNo.Text))) & ", JJ_FormNo = '" & Trim(txt_JJFormNo.Text) & "',  Packing_Type = '" & Trim(cbo_RollBundle.Text) & "'  ,  Vechile_No = '" & Trim(cbo_Vechile.Text) & "'  , USER_IDNO = " & Val(Common_Procedures.User.IdNo) & " , WareHouse_IdNo = " & Val(vGodnFrm_IdNo) & " Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Bale_Transfer_Code = '" & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()

                cmd.CommandText = "Update ClothSales_order_Details set Delivery_Meters = a.Delivery_Meters - b.Meters from ClothSales_order_Details a, Bale_Transfer_Details b Where b.Bale_Transfer_Code = '" & Trim(NewCode) & "' and a.ClothSales_Order_code = b.ClothSales_Order_code and a.ClothSales_Order_SlNo = b.ClothSales_Order_SlNo"
                cmd.ExecuteNonQuery()

                cmd.CommandText = "Update Packing_Slip_Head set Delivery_Code = '', Delivery_No = '', Delivery_DetailsSlNo = 0, Delivery_Increment = Delivery_Increment - 1, Delivery_Date = Null Where Delivery_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()

                cmd.CommandText = "Delete from Packing_Slip_Head Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Bale_Transfer_Code = '" & Trim(NewCode) & "' and Delivery_Code = ''"
                cmd.ExecuteNonQuery()

            End If

            Call Common_Procedures.User_Modification_Updation(con, "HEAD", Me.Name, "NEW", "Bale_Transfer_Head", "Bale_Transfer_Code", Val(lbl_Company.Tag), NewCode, lbl_RefNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, False, "", "", "Bale_Transfer_Code, Company_IdNo, for_OrderBy", tr)


            EntID = Trim(Pk_Condition) & Trim(lbl_RefNo.Text)
            PBlNo = Trim(lbl_RefNo.Text)
            Partcls = "Bale Transfer : Dc.No. " & Trim(lbl_RefNo.Text)

            cmd.CommandText = "Delete from Bale_Transfer_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Bale_Transfer_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Stock_Cloth_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            With dgv_Details

                Sno = 0
                For I = 0 To .RowCount - 1

                    If Trim(.Rows(I).Cells(1).Value) <> "" And (Val(.Rows(I).Cells(6).Value) <> 0 Or Val(.Rows(I).Cells(7).Value) <> 0) Then

                        Sno = Sno + 1

                        clth_ID = Common_Procedures.Cloth_NameToIdNo(con, .Rows(I).Cells(1).Value, tr)

                        clthtyp_ID = Common_Procedures.ClothType_NameToIdNo(con, .Rows(I).Cells(2).Value, tr)

                        OrdCd = ""
                        OrdSlNo = 0
                        'If Trim(UCase(cbo_Type.Text)) = "ORDER" Then
                        'OrdCd = Trim(.Rows(i).Cells(8).Value)
                        'OrdSlNo = Val(.Rows(i).Cells(9).Value)
                        'End If

                        cmd.CommandText = "Insert into Bale_Transfer_Details (      Bale_Transfer_Code     ,               Company_IdNo       ,          Bale_Transfer_No     ,                                 for_OrderBy                             , Bale_Transfer_Date      ,            Ledger_IdNo        ,         Sl_No       ,          Cloth_IdNo          ,      ClothType_IdNo         ,                   Fold_Perc              ,                 Bales                    ,                   Bales_Nos           ,                       Pcs                 ,                      Meters              ,  ClothSales_Order_code ,   ClothSales_Order_SlNo  ,              Bale_Transfer_SlNo            ,              PackingSlip_Codes          ,                    Rate                    ) " & _
                                              "Values                            (   '" & Trim(NewCode) & "'   , " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_RefNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text))) & " ,       @EntryDate           ,      " & Str(Val(vGodnTo_IdNo)) & " , " & Str(Val(Sno)) & ", " & Str(Val(clth_ID)) & "   , " & Str(Val(clthtyp_ID)) & ", " & Str(Val(.Rows(I).Cells(3).Value)) & ", " & Str(Val(.Rows(I).Cells(4).Value)) & ",'" & Trim(.Rows(I).Cells(5).Value) & "',  " & Str(Val(.Rows(I).Cells(6).Value)) & ", " & Str(Val(.Rows(I).Cells(7).Value)) & ", '" & Trim(OrdCd) & "'  , " & Str(Val(OrdSlNo)) & ", " & Str(Val(.Rows(I).Cells(10).Value)) & " , '" & Trim(.Rows(I).Cells(12).Value) & "', " & Str(Val(.Rows(I).Cells(13).Value)) & " ) "
                        cmd.ExecuteNonQuery()


                        With dgv_BaleDetails

                            Sno = 0
                            For J = 0 To .RowCount - 1

                                If Val(dgv_BaleDetails.Rows(J).Cells(3).Value) <> 0 And Trim(dgv_BaleDetails.Rows(J).Cells(5).Value) <> "" Then

                                    If Val(dgv_BaleDetails.Rows(J).Cells(0).Value) = Val(dgv_Details.Rows(I).Cells(10).Value) Then

                                        cmd.CommandText = "Update Packing_Slip_Head set Delivery_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "', Delivery_No = '" & Trim(lbl_RefNo.Text) & "', Delivery_DetailsSlNo = " & Str(Val(dgv_BaleDetails.Rows(J).Cells(0).Value)) & ", Delivery_Increment = Delivery_Increment + 1, Delivery_Date = @EntryDate Where Packing_Slip_Code = '" & Trim(dgv_BaleDetails.Rows(J).Cells(5).Value) & "'"
                                        cmd.ExecuteNonQuery()

                                        vNewRollCode = Trim(Pk_Condition) & Trim(NewCode) & "\\" & Trim(dgv_BaleDetails.Rows(J).Cells(5).Value) & "//" & Trim(Common_Procedures.FnYearCode)

                                        Nr = 0
                                        cmd.CommandText = "Update Packing_Slip_Head set Packing_Slip_Date = @EntryDate, Note = '" & Trim(txt_Note.Text) & "' , User_IdNo = " & Val(Common_Procedures.User.IdNo) & " Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Bale_Transfer_Code = '" & Trim(NewCode) & "'  and Packing_Slip_Code = '" & Trim(vNewRollCode) & "'"
                                        Nr = cmd.ExecuteNonQuery()
                                        If Nr = 0 Then
                                            cmd.CommandText = "Insert into Packing_Slip_Head ( Bale_Transfer_Code    ,       Packing_Slip_Code     ,               Company_IdNo       ,                           Packing_Slip_No             ,                               for_OrderBy                                                      , Packing_Slip_Date,                                Ledger_IdNo                  ,    Cloth_IdNo            ,            ClothType_IdNo   ,                               Bale_Bundle             ,                                  Folding             ,                                     Total_Pcs             ,                                     Total_Meters         ,                                     Total_Weight         ,               Note            ,                                 User_IdNo      ,      WareHouse_IdNo ) " & _
                                                                "          Values            ('" & Trim(NewCode) & "', '" & Trim(vNewRollCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(dgv_BaleDetails.Rows(J).Cells(1).Value) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(dgv_BaleDetails.Rows(J).Cells(1).Value))) & ",      @EntryDate    ,  " & Str(Val(Common_Procedures.CommonLedger.OwnSort_Ac)) & ", " & Str(Val(clth_ID)) & ", " & Str(Val(clthtyp_ID)) & ", '" & Trim(dgv_BaleDetails.Rows(J).Cells(6).Value) & "',  " & Str(Val(dgv_Details.Rows(I).Cells(3).Value)) & ",  " & Str(Val(dgv_BaleDetails.Rows(J).Cells(2).Value)) & " , " & Str(Val(dgv_BaleDetails.Rows(J).Cells(3).Value)) & " , " & Str(Val(dgv_BaleDetails.Rows(J).Cells(4).Value)) & " ,  '" & Trim(txt_Note.Text) & "', " & Val(Common_Procedures.User.IdNo) & " ," & Val(vGodnTo_IdNo) & " ) "
                                            cmd.ExecuteNonQuery()
                                        End If

                                    End If


                                End If

                            Next J

                        End With


                    End If

                Next I
                Call Common_Procedures.User_Modification_Updation(con, "DETAILS", Me.Name, "OLD", "Bale_Transfer_Details", "Bale_Transfer_Code", Val(lbl_Company.Tag), NewCode, lbl_RefNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, False, "Cloth_IdNo ,ClothType_IdNo , Fold_Perc   ,  Bales ,  Bales_Nos ,  Pcs ,  Meters   ,  ClothSales_Order_code ,   ClothSales_Order_SlNo  ,  Bale_Transfer_SlNo ,  PackingSlip_Codes  , Rate ", "Sl_No", "  Bale_Transfer_Code, For_OrderBy, Company_IdNo, Bale_Transfer_No, Bale_Transfer_Date, Ledger_Idno  ", tr)


            End With



            OpYrCode = Microsoft.VisualBasic.Left(Trim(Common_Procedures.FnRange), 4)
            OpYrCode = Trim(Mid(Val(OpYrCode) - 1, 3, 2)) & "-" & Trim(Microsoft.VisualBasic.Right(OpYrCode, 2))

            cmd.CommandText = "Truncate Table " & Trim(Common_Procedures.EntryTempSubTable) & ""
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Insert into " & Trim(Common_Procedures.EntryTempSubTable) & "(Name1, Int1, Int2, Weight1, Meters1, Meters2, Meters3, Meters4, Meters5) select a.Bale_Transfer_Code, a.Bale_Transfer_No, a.Cloth_IdNo, a.Fold_Perc, (CASE WHEN a.ClothType_IdNo = 1 THEN a.Meters ELSE 0 END), (CASE WHEN a.ClothType_IdNo = 2 THEN a.Meters ELSE 0 END), (CASE WHEN a.ClothType_IdNo = 3 THEN a.Meters ELSE 0 END), (CASE WHEN a.ClothType_IdNo = 4 THEN a.Meters ELSE 0 END), (CASE WHEN a.ClothType_IdNo = 5 THEN a.Meters ELSE 0 END) from Bale_Transfer_Details a where a.Bale_Transfer_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Insert into " & Trim(Common_Procedures.EntryTempSubTable) & "(Name1, Int1, Int2, Weight1, Meters1, Meters2, Meters3, Meters4, Meters5) select tC.Bale_Transfer_Code, tC.Bale_Transfer_No, tC.Cloth_IdNo, tC.Fold_Perc, -1*(CASE WHEN tC.ClothType_IdNo = 1 THEN a.Total_Meters ELSE 0 END), -1*(CASE WHEN tC.ClothType_IdNo = 2 THEN a.Total_Meters ELSE 0 END), -1*(CASE WHEN tC.ClothType_IdNo = 3 THEN a.Total_Meters ELSE 0 END), -1*(CASE WHEN tC.ClothType_IdNo = 4 THEN a.Total_Meters ELSE 0 END), -1*(CASE WHEN tC.ClothType_IdNo = 5 THEN a.Total_Meters ELSE 0 END) from Packing_Slip_Head a, Bale_Transfer_Details tC where tC.Bale_Transfer_Code = '" & Trim(NewCode) & "' and a.Packing_Slip_Code LIKE '%/" & Trim(OpYrCode) & "' and a.Delivery_Code = '" & Trim(Pk_Condition) & "' + tC.Bale_Transfer_Code and a.Delivery_DetailsSlNo = tC.Bale_Transfer_SlNo"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Insert into " & Trim(Common_Procedures.EntryTempSubTable) & "(Name1, Int1, Int2, Weight1, Meters1, Meters2, Meters3, Meters4, Meters5) select tC.Bale_Transfer_Code, tC.Bale_Transfer_No, tC.Cloth_IdNo, tC.Fold_Perc, -1*(CASE WHEN tC.ClothType_IdNo = 1 THEN b.Meters ELSE 0 END), -1*(CASE WHEN tC.ClothType_IdNo = 2 THEN b.Meters ELSE 0 END), -1*(CASE WHEN tC.ClothType_IdNo = 3 THEN b.Meters ELSE 0 END), -1*(CASE WHEN tC.ClothType_IdNo = 4 THEN b.Meters ELSE 0 END), -1*(CASE WHEN tC.ClothType_IdNo = 5 THEN b.Meters ELSE 0 END) from Packing_Slip_Head a, Packing_Slip_Details b, Bale_Transfer_Details tC where tC.Bale_Transfer_Code = '" & Trim(NewCode) & "' and a.Packing_Slip_Code NOT LIKE '%/" & Trim(OpYrCode) & "' and a.Company_IdNo = b.Company_IdNo and a.Packing_Slip_Code = b.Packing_Slip_Code and a.Delivery_Code = '" & Trim(Pk_Condition) & "' + tC.Bale_Transfer_Code and a.Delivery_DetailsSlNo = tC.Bale_Transfer_SlNo"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Truncate Table " & Trim(Common_Procedures.EntryTempTable) & ""
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Insert into " & Trim(Common_Procedures.EntryTempTable) & "(Int1, Weight1, Meters1, Meters2, Meters3, Meters4, Meters5) select Int2, Weight1, sum(Meters1), sum(Meters2), sum(Meters3), sum(Meters4), sum(Meters5) from " & Trim(Common_Procedures.EntryTempSubTable) & " group by Int2, Weight1 "
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Insert into " & Trim(Common_Procedures.EntryTempTable) & "(Int1, Weight1, Meters1, Meters2, Meters3, Meters4, Meters5) select a.Cloth_IdNo, a.Folding, (CASE WHEN a.ClothType_IdNo = 1 THEN a.Total_Meters ELSE 0 END), (CASE WHEN a.ClothType_IdNo = 2 THEN a.Total_Meters ELSE 0 END), (CASE WHEN a.ClothType_IdNo = 3 THEN a.Total_Meters ELSE 0 END), (CASE WHEN a.ClothType_IdNo = 4 THEN a.Total_Meters ELSE 0 END), (CASE WHEN a.ClothType_IdNo = 5 THEN a.Total_Meters ELSE 0 END) from Packing_Slip_Head a where a.Delivery_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' and a.Packing_Slip_Code LIKE '%/" & Trim(OpYrCode) & "'"
            cmd.ExecuteNonQuery()
            cmd.CommandText = "Insert into " & Trim(Common_Procedures.EntryTempTable) & "(Int1, Weight1, Meters1, Meters2, Meters3, Meters4, Meters5) select b.Cloth_IdNo, b.Folding, (CASE WHEN b.ClothType_IdNo = 1 THEN b.Meters ELSE 0 END), (CASE WHEN b.ClothType_IdNo = 2 THEN b.Meters ELSE 0 END), (CASE WHEN b.ClothType_IdNo = 3 THEN b.Meters ELSE 0 END), (CASE WHEN b.ClothType_IdNo = 4 THEN b.Meters ELSE 0 END), (CASE WHEN b.ClothType_IdNo = 5 THEN b.Meters ELSE 0 END) from Packing_Slip_Head a, Packing_Slip_Details b where a.Delivery_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' and a.Packing_Slip_Code NOT LIKE '%/" & Trim(OpYrCode) & "' and a.Company_IdNo = b.Company_IdNo and a.Packing_Slip_Code = b.Packing_Slip_Code"
            cmd.ExecuteNonQuery()

            Da = New SqlClient.SqlDataAdapter("select Int1 as Clo_IdNo, Weight1 as FoldPerc, sum(Meters1 ) as Type1_Mtrs, sum(Meters2) as Type2_Mtrs, sum(Meters3) as Type3_Mtrs, sum(Meters4) as Type4_Mtrs, sum(Meters5) as Type5_Mtrs from " & Trim(Common_Procedures.EntryTempTable) & " group by Int1, Weight1 having sum(Meters1) <> 0 or sum(Meters2) <> 0 or sum(Meters3) <> 0 or sum(Meters4) <> 0 or sum(Meters5) <> 0 ", con)
            Da.SelectCommand.Transaction = tr
            Dt1 = New DataTable
            Da.Fill(Dt1)

            Sno = 0
            If Dt1.Rows.Count > 0 Then
                For I = 0 To Dt1.Rows.Count - 1
                    Sno = Sno + 1

                    cmd.CommandText = "Insert into Stock_Cloth_Processing_Details ( Reference_Code        ,             Company_IdNo         ,           Reference_No        ,                                for_OrderBy                             , Reference_Date ,                                            StockOff_IdNo  ,      DeliveryTo_Idno    ,      ReceivedFrom_Idno   ,         Entry_ID     ,       Party_Bill_No  ,       Particulars      ,           Sl_No      ,                         Cloth_Idno                     ,                         Folding                        ,                         Meters_Type1                     ,                         Meters_Type2                     ,                         Meters_Type3                     ,                         Meters_Type4                     ,                         Meters_Type5                      ) " & _
                                                    " Values ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_RefNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text))) & ",    @EntryDate    , " & Str(Val(Common_Procedures.CommonLedger.Godown_Ac)) & ", " & Str(Val(vGodnTo_IdNo)) & ", " & Str(Val(vGodnFrm_IdNo)) & ", '" & Trim(EntID) & "', '" & Trim(PBlNo) & "', '" & Trim(Partcls) & "', " & Str(Val(Sno)) & ", " & Str(Val(Dt1.Rows(I).Item("Clo_IdNo").ToString)) & ", " & Str(Val(Dt1.Rows(I).Item("FoldPerc").ToString)) & ", " & Str(Val(Dt1.Rows(I).Item("Type1_Mtrs").ToString)) & ", " & Str(Val(Dt1.Rows(I).Item("Type2_Mtrs").ToString)) & ", " & Str(Val(Dt1.Rows(I).Item("Type3_Mtrs").ToString)) & ", " & Str(Val(Dt1.Rows(I).Item("Type4_Mtrs").ToString)) & ", " & Str(Val(Dt1.Rows(I).Item("Type5_Mtrs").ToString)) & " ) "
                    cmd.ExecuteNonQuery()

                Next
            End If

            If Common_Procedures.settings.CustomerCode = "1267" Then

                '----- Saving Cross Checking

                cmd.CommandText = "Truncate Table " & Trim(Common_Procedures.EntryTempTable) & ""
                cmd.ExecuteNonQuery()

                cmd.CommandText = "Insert Into " & Trim(Common_Procedures.EntryTempTable) & "(Meters1) select Total_Meters from Packing_Slip_Head where Delivery_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()
                cmd.CommandText = "Insert Into " & Trim(Common_Procedures.EntryTempTable) & "(Meters1) select -1*Total_Meters from Bale_Transfer_Head where  Bale_Transfer_Code = '" & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()

                Da = New SqlClient.SqlDataAdapter("select sum(Meters1) as ProdMtrs from " & Trim(Common_Procedures.EntryTempTable) & " having sum(Meters1) <> 0", con)
                Da.SelectCommand.Transaction = tr
                dt2 = New DataTable
                Da.Fill(dt2)
                If dt2.Rows.Count > 0 Then
                    If IsDBNull(dt2.Rows(0)(0).ToString) = False Then
                        If Val(dt2.Rows(0)(0).ToString) <> 0 Then
                            Throw New ApplicationException("Invalid Bale Selection : Mismatch of Dc && Bale Meters")
                            Exit Sub
                        End If
                    End If
                End If
                Dt2.Clear()


                cmd.CommandText = "Truncate Table " & Trim(Common_Procedures.EntryTempTable) & ""
                cmd.ExecuteNonQuery()

                cmd.CommandText = "Insert Into " & Trim(Common_Procedures.EntryTempTable) & "(Meters1) select Total_Meters from Packing_Slip_Head where Delivery_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' and Packing_Slip_Date <= @EntryDate"
                cmd.ExecuteNonQuery()
                cmd.CommandText = "Insert Into " & Trim(Common_Procedures.EntryTempTable) & "(Meters1) select -1*Total_Meters from Bale_Transfer_Head where Bale_Transfer_Code = '" & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()

                Da = New SqlClient.SqlDataAdapter("select sum(Meters1) as ProdMtrs from " & Trim(Common_Procedures.EntryTempTable) & " having sum(Meters1) <> 0", con)
                Da.SelectCommand.Transaction = tr
                Dt2 = New DataTable
                Da.Fill(Dt2)
                If Dt2.Rows.Count > 0 Then
                    If IsDBNull(Dt2.Rows(0)(0).ToString) = False Then
                        If Val(Dt2.Rows(0)(0).ToString) <> 0 Then
                            Throw New ApplicationException("Invalid Bale Selection : Delivery Date should be greater than Packing Slip Date")
                            Exit Sub
                        End If
                    End If
                End If
                Dt2.Clear()

            End If

            tr.Commit()

            MessageBox.Show("Saved Sucessfully!!!", "FOR SAVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Information)

            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1267" Then '---- BRT TEXTTILES (SOMANUR)
                If New_Entry = True Then
                    Send_SMS()
                End If
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
            MessageBox.Show(ex.Message, "FOR SAVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Finally

            Dt1.Dispose()
            Da.Dispose()
            cmd.Dispose()
            tr.Dispose()

            If msk_date.Enabled And msk_date.Visible Then msk_date.Focus()

        End Try

    End Sub

    Private Sub Total_Calculation()
        Dim Sno As Integer
        Dim TotPcs As Single
        Dim TotBals As Single
        Dim TotMtrs As Single

        If NoCalc_Status = True Then Exit Sub

        Sno = 0
        TotPcs = 0 : TotBals = 0 : TotMtrs = 0

        With dgv_Details
            For i = 0 To .RowCount - 1
                Sno = Sno + 1
                .Rows(i).Cells(0).Value = Sno
                If Val(.Rows(i).Cells(7).Value) <> 0 Then

                    TotBals = TotBals + Val(.Rows(i).Cells(4).Value())
                    TotPcs = TotPcs + Val(.Rows(i).Cells(6).Value())
                    TotMtrs = TotMtrs + Val(.Rows(i).Cells(7).Value())

                End If

            Next i

        End With


        With dgv_Details_Total
            If .RowCount = 0 Then .Rows.Add()
            .Rows(0).Cells(4).Value = Val(TotBals)
            .Rows(0).Cells(6).Value = Val(TotPcs)
            .Rows(0).Cells(7).Value = Format(Val(TotMtrs), "########0.00")
        End With

    End Sub

    Private Sub cbo_TransferGodownTo_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_TransferGodownTo.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "( Ledger_Type = 'GODOWN' or Ledger_Type = 'JOBWORKER' or Show_In_All_Entry = 1 )", "(Ledger_idno = 0)")
    End Sub

    Private Sub cbo_TransferGodownTo_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_TransferGodownTo.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_TransferGodownTo, msk_date, cbo_TransferGodownFrom, "Ledger_AlaisHead", "Ledger_DisplayName", " ( Ledger_Type = 'GODOWN' or Ledger_Type = 'JOBWORKER' or Show_In_All_Entry = 1) ", "(Ledger_idno = 0)")
    End Sub

    Private Sub cbo_TransferGodownTo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_TransferGodownTo.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_TransferGodownTo, cbo_TransferGodownFrom, "Ledger_AlaisHead", "Ledger_DisplayName", " ( Ledger_Type = 'GODOWN' or Ledger_Type = 'JOBWORKER' or Show_In_All_Entry = 1) ", "(Ledger_idno = 0)")
    End Sub

    Private Sub cbo_TransferGodownTo_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_TransferGodownTo.KeyUp
        If e.Control = False And e.KeyValue = 17 Then
            Common_Procedures.MDI_LedType = "GODOWN"
            Dim f As New Ledger_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_TransferGodownTo.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If
    End Sub

    Private Sub cbo_DespTo_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_DespTo.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Bale_Transfer_Head", "Despatch_To", "", "")
    End Sub

    Private Sub cbo_DespTo_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_DespTo.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_DespTo, msk_LrDate, txt_JJFormNo, "Bale_Transfer_Head", "Despatch_To", "", "")
    End Sub

    Private Sub cbo_DespTo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_DespTo.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_DespTo, txt_JJFormNo, "Bale_Transfer_Head", "Despatch_To", "", "", False)
    End Sub

    Private Sub cbo_Transport_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Transport.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'TRANSPORT')", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_Transport_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Transport.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Transport, Nothing, txt_Freight, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'TRANSPORT')", "(Ledger_IdNo = 0)")
        If e.KeyValue = 38 Then
            If dgv_Details.Rows.Count > 0 Then
                dgv_Details.Focus()
                dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(1)
                dgv_Details.CurrentCell.Selected = True
            Else
                txt_DelvAdd2.Focus()

            End If
        End If

    End Sub

    Private Sub cbo_Transport_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Transport.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Transport, txt_Freight, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'TRANSPORT')", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_Transport_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Transport.KeyUp
        If e.Control = False And e.KeyValue = 17 Then
            Common_Procedures.UR.Ledr_Wea_Siz_Rw_Trans_JbWrk_Creation = Common_Procedures.UR.Ledger_Creation
            Common_Procedures.MDI_LedType = "TRANSPORT"
            Dim f As New Ledger_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Transport.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()
        End If
    End Sub

    Private Sub cbo_Grid_ClothName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Grid_ClothName.GotFocus
        vCbo_ItmNm = Trim(cbo_Grid_ClothName.Text)

        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Cloth_Head", "Cloth_Name", "", "(Cloth_IdNo = 0)")

    End Sub

    Private Sub cbo_Grid_ClothName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Grid_ClothName.KeyDown

        Dim dep_idno As Integer = 0

        vcbo_KeyDwnVal = e.KeyValue

        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Grid_ClothName, txt_DelvAdd2, cbo_Grid_Clothtype, "Cloth_Head", "Cloth_Name", "", "(Cloth_IdNo = 0)")

        With dgv_Details

            If (e.KeyValue = 38 And cbo_Grid_ClothName.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
                txt_DelvAdd2.Focus()
            End If

            If (e.KeyValue = 40 And cbo_Grid_ClothName.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then

                If .CurrentCell.RowIndex = .Rows.Count - 1 And Trim(.Rows(.CurrentCell.RowIndex).Cells.Item(1).Value) = "" Then
                    cbo_Transport.Focus()

                Else
                    .Focus()
                    .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)

                End If

            End If

        End With
    End Sub

    Private Sub cbo_Grid_ClothName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Grid_ClothName.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Grid_ClothName, cbo_Grid_Clothtype, "Cloth_Head", "Cloth_Name", "", "(Cloth_IdNo = 0)")

        If Asc(e.KeyChar) = 13 Then

            e.Handled = True

            With dgv_Details
                If Trim(.Rows(.CurrentRow.Index).Cells(1).Value) = "" And .CurrentRow.Index = .Rows.Count - 1 Then
                    cbo_Transport.Focus()
                Else
                    .CurrentCell = .Rows(.CurrentRow.Index).Cells(2)

                End If
            End With

        End If
    End Sub

    Private Sub cbo_Grid_ClothName_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Grid_ClothName.KeyUp
        If e.Control = False And e.KeyValue = 17 Then
            Dim f As New Cloth_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Grid_ClothName.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()
        End If
    End Sub

    Private Sub cbo_Grid_Clothtype_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Grid_Clothtype.GotFocus
        vCbo_ItmNm = Trim(cbo_Grid_Clothtype.Text)
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "ClothType_Head", "ClothType_Name", "", "(ClothType_IdNo = 0)")

    End Sub

    Private Sub cbo_Grid_Clothtype_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Grid_Clothtype.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Grid_Clothtype, Nothing, Nothing, "ClothType_Head", "ClothType_Name", "", "(ClothType_IdNo = 0)")
        vcbo_KeyDwnVal = e.KeyValue

        With dgv_Details

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
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Grid_Clothtype, Nothing, "ClothType_Head", "ClothType_Name", "", "(ClothType_IdNo = 0)")

        If Asc(e.KeyChar) = 13 Then

            With dgv_Details

                .Focus()
                .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)

            End With

        End If

    End Sub


    Private Sub dgv_Details_CellEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellEnter
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim Dt2 As New DataTable
        Dim rect As Rectangle

        With dgv_Details

            If Val(.CurrentRow.Cells(0).Value) = 0 Then
                .CurrentRow.Cells(0).Value = .CurrentRow.Index + 1
            End If

            If Val(.Rows(e.RowIndex).Cells(10).Value) = 0 Then
                If e.RowIndex = 0 Then
                    .Rows(e.RowIndex).Cells(10).Value = 1
                Else
                    .Rows(e.RowIndex).Cells(10).Value = Val(.Rows(e.RowIndex - 1).Cells(10).Value) + 1
                End If
            End If

            If Trim(.CurrentRow.Cells(2).Value) = "" Then
                .CurrentRow.Cells(2).Value = Common_Procedures.ClothType_IdNoToName(con, 1)
            End If

            If Val(.CurrentRow.Cells(3).Value) = 0 Then
                .CurrentRow.Cells(3).Value = "100"
            End If

            If e.ColumnIndex = 1 And Val(.Rows(.CurrentCell.RowIndex).Cells(11).Value) = 0 Then

                If cbo_Grid_ClothName.Visible = False Or Val(cbo_Grid_ClothName.Tag) <> e.RowIndex Then

                    cbo_Grid_ClothName.Tag = -1
                    Da = New SqlClient.SqlDataAdapter("select Cloth_Name from Cloth_Head order by Cloth_Name", con)
                    Dt1 = New DataTable
                    Da.Fill(Dt1)
                    cbo_Grid_ClothName.DataSource = Dt1
                    cbo_Grid_ClothName.DisplayMember = "Cloth_Name"

                    rect = .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False)

                    cbo_Grid_ClothName.Left = .Left + rect.Left
                    cbo_Grid_ClothName.Top = .Top + rect.Top

                    cbo_Grid_ClothName.Width = rect.Width
                    cbo_Grid_ClothName.Height = rect.Height
                    cbo_Grid_ClothName.Text = .CurrentCell.Value

                    cbo_Grid_ClothName.Tag = Val(e.RowIndex)
                    cbo_Grid_ClothName.Visible = True

                    cbo_Grid_ClothName.BringToFront()
                    cbo_Grid_ClothName.Focus()

                End If

            Else
                cbo_Grid_ClothName.Visible = False

            End If

            If e.ColumnIndex = 2 And Val(.Rows(.CurrentCell.RowIndex).Cells(11).Value) = 0 Then

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

            If e.ColumnIndex = 4 And Trim(UCase(Common_Procedures.ClothDelivery_Opening_OR_Entry)) <> "OPENING" Or e.ColumnIndex = 5 And Trim(UCase(Common_Procedures.ClothDelivery_Opening_OR_Entry)) <> "OPENING" Then

                rect = .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False)

                pnl_BaleSelection_ToolTip.Left = .Left + rect.Left
                pnl_BaleSelection_ToolTip.Top = .Top + rect.Top + rect.Height + 3

                pnl_BaleSelection_ToolTip.Visible = True

            Else
                pnl_BaleSelection_ToolTip.Visible = False

            End If



        End With

    End Sub

    Private Sub dgv_Details_CellLeave(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellLeave
        With dgv_Details
            If .CurrentCell.ColumnIndex = 6 Or .CurrentCell.ColumnIndex = 7 Or .CurrentCell.ColumnIndex = 8 Or .CurrentCell.ColumnIndex = 9 Then
                If Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value) <> 0 Then
                    .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = Format(Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value), "#########0.00")
                Else
                    .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = ""
                End If
            End If
        End With
    End Sub

    Private Sub dgv_Details_CellValueChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellValueChanged
        On Error Resume Next

        If IsNothing(dgv_Details.CurrentCell) Then Exit Sub
        With dgv_Details
            If .Visible Then
                If .CurrentCell.ColumnIndex = 1 Or .CurrentCell.ColumnIndex = 4 Or .CurrentCell.ColumnIndex = 6 Or .CurrentCell.ColumnIndex = 7 Then
                    Total_Calculation()

                End If
                If .CurrentCell.ColumnIndex = 1 Or .CurrentCell.ColumnIndex = 6 Then
                    Amount_Calculation(.CurrentCell.RowIndex, .CurrentCell.ColumnIndex)
                End If
            End If
        End With
    End Sub

    Private Sub dgv_Details_EditingControlShowing(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewEditingControlShowingEventArgs) Handles dgv_Details.EditingControlShowing
        dgtxt_Details = CType(dgv_Details.EditingControl, DataGridViewTextBoxEditingControl)
    End Sub

    Private Sub dgtxt_Details_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgtxt_Details.Enter
        dgv_Details.EditingControl.BackColor = Color.SpringGreen
    End Sub

    Private Sub dgtxt_Details_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgtxt_Details.KeyDown
        With dgv_Details
            vcbo_KeyDwnVal = e.KeyValue
            If .Visible Then
                If e.KeyValue = Keys.Delete Then
                    If Val(.Rows(.CurrentCell.RowIndex).Cells(11).Value) <> 0 Or Trim(.Rows(.CurrentCell.RowIndex).Cells(12).Value) <> "" Then
                        e.Handled = True
                    End If

                    'If .CurrentCell.ColumnIndex = 1 Or .CurrentCell.ColumnIndex = 2 Or .CurrentCell.ColumnIndex = 3 Then
                    '    If Trim(UCase(cbo_Type.Text)) = "ORDER" Then
                    '        e.Handled = True
                    '        e.SuppressKeyPress = True
                    '    End If
                    'End If
                End If
            End If
        End With

    End Sub

    Private Sub dgtxt_Details_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dgtxt_Details.KeyPress
        On Error Resume Next
        Dim i As Integer
        Dim n As Integer = -1

        With dgv_Details
            If .Visible Then

                If Val(.Rows(.CurrentCell.RowIndex).Cells(11).Value) <> 0 Then
                    e.Handled = True

                    Add_NewRow_ToGrid()

                End If

                'If .CurrentCell.ColumnIndex = 1 Or .CurrentCell.ColumnIndex = 2 Or .CurrentCell.ColumnIndex = 3 Then
                '    If Trim(UCase(cbo_Type.Text)) = "ORDER" Then
                '        e.Handled = True
                '    End If
                'End If

                If .CurrentCell.ColumnIndex = 3 Or .CurrentCell.ColumnIndex = 4 Or .CurrentCell.ColumnIndex = 6 Or .CurrentCell.ColumnIndex = 7 Or .CurrentCell.ColumnIndex = 13 Then

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

        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            btn_BaleSelection_Click(sender, e)
        End If

    End Sub

    Private Sub dgv_Details_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_Details.KeyDown
        With dgv_Details

            'vcbo_KeyDwnVal = e.KeyValue

            'If e.KeyCode = Keys.Left Then
            '    If .CurrentCell.ColumnIndex <= 1 Then
            '        If .CurrentCell.RowIndex = 0 Then
            '            txt_DelvAdd2.Focus()
            '        Else
            '            .CurrentCell = .Rows(.CurrentCell.RowIndex - 1).Cells(.ColumnCount - 1)
            '        End If
            '    End If
            'End If

            'If e.KeyCode = Keys.Right Then
            '    If .CurrentCell.ColumnIndex >= .ColumnCount - 1 Then
            '        If .CurrentCell.RowIndex >= .Rows.Count - 1 Then
            '            cbo_Transport.Focus()
            '        Else
            '            .CurrentCell = .Rows(.CurrentCell.RowIndex + 1).Cells(1)
            '        End If
            '    End If
            'End If

        End With

    End Sub

    Private Sub dgv_Details_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_Details.KeyUp
        Dim i As Integer
        Dim n As Integer

        If e.Control = True And UCase(Chr(e.KeyCode)) = "D" Then

            With dgv_Details

                If Val(.Rows(.CurrentCell.RowIndex).Cells(11).Value) = 0 Then

                    n = .CurrentRow.Index

                    If n = .Rows.Count - 1 Then
                        For i = 0 To .ColumnCount - 1
                            .Rows(n).Cells(i).Value = ""
                        Next

                    Else
                        .Rows.RemoveAt(n)

                    End If

                    Total_Calculation()

                End If

            End With

        End If

        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            btn_BaleSelection_Click(sender, e)
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

            If Val(.Rows(e.RowIndex).Cells(10).Value) = 0 Then
                If e.RowIndex = 0 Then
                    .Rows(e.RowIndex).Cells(10).Value = 1
                Else
                    .Rows(e.RowIndex).Cells(10).Value = Val(.Rows(e.RowIndex - 1).Cells(10).Value) + 1
                End If
            End If

        End With

    End Sub

    Private Sub txt_DelvAdd2_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_DelvAdd2.KeyDown
        If e.KeyValue = 38 Then SendKeys.Send("+{TAB}")
        If (e.KeyValue = 40) Then
            If dgv_Details.Rows.Count > 0 Then
                dgv_Details.Focus()
                dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(1)
            Else
                cbo_Transport.Focus()
            End If
        End If
    End Sub

    Private Sub txt_DelvAdd2_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_DelvAdd2.KeyPress
        If Asc(e.KeyChar) = 13 Then
            If dgv_Details.Rows.Count > 0 Then
                dgv_Details.Focus()
                dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(1)
            Else
                cbo_Transport.Focus()
            End If
        End If
    End Sub

    Private Sub txt_Note_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_Note.KeyDown
        If e.KeyValue = 38 Then
            txt_Freight.Focus()
        End If

        If e.KeyValue = 40 Then
            If MessageBox.Show("Do you want to save", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = DialogResult.Yes Then
                save_record()
            Else
                msk_date.Focus()
            End If
        End If
    End Sub

    Private Sub cbo_Grid_ClothName_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Grid_ClothName.TextChanged
        Try
            If cbo_Grid_ClothName.Visible Then
                If IsNothing(dgv_Details.CurrentCell) Then Exit Sub
                With dgv_Details
                    If Val(cbo_Grid_ClothName.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = 1 Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_Grid_ClothName.Text)
                    End If
                End With
            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub cbo_Grid_Clothtype_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Grid_Clothtype.TextChanged
        Try
            If cbo_Grid_Clothtype.Visible Then
                If IsNothing(dgv_Details.CurrentCell) Then Exit Sub
                With dgv_Details
                    If Val(cbo_Grid_Clothtype.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = 2 Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_Grid_Clothtype.Text)
                    End If
                End With
            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub btn_save_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_save.Click
        save_record()
    End Sub

    Private Sub btn_close_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_close.Click
        Me.Close()
    End Sub

    Private Sub btn_Print_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Print.Click
        Common_Procedures.Print_OR_Preview_Status = 1
        print_record()
    End Sub

    Private Sub txt_Note_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Note.KeyPress
        If Asc(e.KeyChar) = 13 Then

            If MessageBox.Show("Do you want to save", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = DialogResult.Yes Then
                save_record()
            Else
                msk_date.Focus()
            End If
        End If
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
                Condt = "a.Bale_Transfer_Date between '" & Trim(Format(dtp_Filter_Fromdate.Value, "MM/dd/yyyy")) & "' and '" & Trim(Format(dtp_Filter_ToDate.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_Filter_Fromdate.Value) = True Then
                Condt = "a.Bale_Transfer_Date = '" & Trim(Format(dtp_Filter_Fromdate.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_Filter_ToDate.Value) = True Then
                Condt = "a.Bale_Transfer_Date = '" & Trim(Format(dtp_Filter_ToDate.Value, "MM/dd/yyyy")) & "' "
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
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " b.Cloth_IdNo = " & Str(Val(Clth_IdNo))
            End If

            da = New SqlClient.SqlDataAdapter("select a.*, c.Cloth_Name, d.ClothType_name, e.Ledger_Name from Bale_Transfer_Head a left outer join Bale_Transfer_Details b on a.Bale_Transfer_Code = b.Bale_Transfer_Code left outer join Cloth_head c on b.Cloth_idno = c.Cloth_idno left outer join ClothType_head d on b.ClothType_idno = d.ClothType_idno left outer join Ledger_head e on a.Ledger_idno = e.Ledger_idno where a.company_idno =" & Str(Val(lbl_Company.Tag)) & "and a.Bale_Transfer_Code like '%/" & Trim(EntFnYrCode) & "' " & IIf(Trim(Condt) <> "", " and ", "") & Condt & " Order by Bale_Transfer_Date, for_orderby, Bale_Transfer_No", con)
            da.Fill(dt2)

            dgv_Filter_Details.Rows.Clear()

            If dt2.Rows.Count > 0 Then

                For i = 0 To dt2.Rows.Count - 1

                    n = dgv_Filter_Details.Rows.Add()

                    dgv_Filter_Details.Rows(n).Cells(0).Value = i + 1
                    dgv_Filter_Details.Rows(n).Cells(1).Value = dt2.Rows(i).Item("Bale_Transfer_No").ToString
                    dgv_Filter_Details.Rows(n).Cells(2).Value = Format(Convert.ToDateTime(dt2.Rows(i).Item("Bale_Transfer_Date").ToString), "dd-MM-yyyy")
                    dgv_Filter_Details.Rows(n).Cells(3).Value = dt2.Rows(i).Item("Ledger_Name").ToString
                    dgv_Filter_Details.Rows(n).Cells(4).Value = "" ' dt2.Rows(i).Item("Party_OrderNo").ToString
                    dgv_Filter_Details.Rows(n).Cells(5).Value = dt2.Rows(i).Item("Cloth_Name").ToString
                    dgv_Filter_Details.Rows(n).Cells(6).Value = dt2.Rows(i).Item("ClothType_Name").ToString
                    dgv_Filter_Details.Rows(n).Cells(7).Value = Format(Val(dt2.Rows(i).Item("Total_Meters").ToString), "########0.00")

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

        movno = Trim(dgv_Filter_Details.CurrentRow.Cells(1).Value)

        If Val(movno) <> 0 Then
            Filter_Status = True
            Filter_RowNo = dgv_Filter_Details.CurrentRow.Index
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

    Private Sub dgv_Details_CellEndEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellEndEdit
        dgv_Details_CellLeave(sender, e)
    End Sub

    Private Sub cbo_Filter_ClothName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Filter_ClothName.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Cloth_Head", "Cloth_Name", "", "(Cloth_idno = 0)")

    End Sub

    Private Sub cbo_Filter_ClothName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Filter_ClothName.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_ClothName, cbo_Filter_PartyName, btn_Filter_Show, "Cloth_Head", "Cloth_Name", "", "(Cloth_idno = 0)")

    End Sub

    Private Sub cbo_Filter_ClothName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Filter_ClothName.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Filter_ClothName, btn_Filter_Show, "Cloth_Head", "Cloth_Name", "", "(Cloth_idno = 0)")
    End Sub

    Private Sub cbo_Filter_PartyName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Filter_PartyName.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "( (Ledger_Type = '' and ( AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14) ) or Show_In_All_Entry = 1)", "(Ledger_idno = 0)")
    End Sub

    Private Sub cbo_Filter_PartyName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Filter_PartyName.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_PartyName, dtp_Filter_ToDate, cbo_Filter_ClothName, "Ledger_AlaisHead", "Ledger_DisplayName", "( (Ledger_Type = '' and ( AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14) ) or Show_In_All_Entry = 1)", "(Ledger_idno = 0)")
    End Sub

    Private Sub cbo_Filter_PartyName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Filter_PartyName.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Filter_PartyName, cbo_Filter_ClothName, "Ledger_AlaisHead", "Ledger_DisplayName", "( (Ledger_Type = '' and ( AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14) ) or Show_In_All_Entry = 1)", "(Ledger_idno = 0)")
    End Sub

    Public Sub print_record() Implements Interface_MDIActions.print_record

        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.PrintEntry, Common_Procedures.UR.Bale_Transfer_Entry, New_Entry) = False Then Exit Sub

        pnl_Print.Visible = True
        pnl_Back.Enabled = False
        If btn_Print_Delivery.Enabled And btn_Print_Delivery.Visible Then
            btn_Print_Delivery.Focus()
        End If
    End Sub

    Private Sub btn_Print_Delivery_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Print_Delivery.Click
        Printing_Delivery()
        btn_print_Close_Click(sender, e)
    End Sub

    Private Sub btn_Print_Bale_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Print_Bale.Click
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1090" Then
            Printing_Bale_Delivery()

        Else
            Printing_Bale()

        End If
        btn_print_Close_Click(sender, e)

    End Sub

    Private Sub btn_Print_Cancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Print_Cancel.Click
        btn_print_Close_Click(sender, e)
    End Sub

    Private Sub btn_print_Close_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Close_Print.Click
        pnl_Back.Enabled = True
        pnl_Print.Visible = False
    End Sub

    Public Sub Printing_Delivery()
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim NewCode As String
        'Dim ps As Printing.PaperSize
        Dim PpSzSTS As Boolean = False

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(EntFnYrCode)

        Try

            da1 = New SqlClient.SqlDataAdapter("select * from Bale_Transfer_Head Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Bale_Transfer_Code = '" & Trim(NewCode) & "'", con)
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
                MessageBox.Show("The printing operation failed" & vbCrLf & ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

            End Try


        Else
            Try

                Dim ppd As New PrintPreviewDialog

                set_PaperSize_For_PrintDocument1()
                'For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
                '    ps = PrintDocument1.PrinterSettings.PaperSizes(I)
                '    If ps.Width = 800 And ps.Height = 600 Then
                '        PrintDocument1.DefaultPageSettings.PaperSize = ps
                '        PpSzSTS = True
                '        Exit For
                '    End If
                'Next

                'If PpSzSTS = False Then
                '    Dim pkCustomSize1 As New System.Drawing.Printing.PaperSize("PAPER 8X6", 800, 600)
                '    PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = pkCustomSize1
                '    PrintDocument1.DefaultPageSettings.PaperSize = pkCustomSize1
                'End If

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

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(EntFnYrCode)

        prn_HdDt.Clear()
        prn_DetDt.Clear()
        prn_DetIndx = 0
        prn_DetSNo = 0
        prn_PageNo = 0

        Try

            da1 = New SqlClient.SqlDataAdapter("select a.*, b.*, c.*, d.Ledger_Name as TransportName , Csh.State_Name as Company_State_Name, Csh.State_Code as Company_State_Code, Lsh.State_Name as Ledger_State_Name, Lsh.State_Code as Ledger_State_Code from Bale_Transfer_Head a INNER JOIN Company_Head b ON a.Company_IdNo = b.Company_IdNo LEFT OUTER JOIN State_Head Csh ON b.Company_State_IdNo = Csh.State_IdNo INNER JOIN Ledger_Head c ON a.Ledger_IdNo = c.Ledger_IdNo LEFT OUTER JOIN State_Head Lsh ON c.Ledger_State_IdNo = Lsh.State_IdNo Left outer JOIN Ledger_Head d ON a.Transport_IdNo = d.Ledger_IdNo where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Bale_Transfer_Code = '" & Trim(NewCode) & "'", con)
            prn_HdDt = New DataTable
            da1.Fill(prn_HdDt)

            If prn_HdDt.Rows.Count > 0 Then

                da2 = New SqlClient.SqlDataAdapter("select a.*, b.Cloth_name, d.ClothType_name ,e.* from Bale_Transfer_Details a INNER JOIN Cloth_Head b ON a.Cloth_idno = b.Cloth_idno INNER JOIN ItemGroup_Head e ON e.ItemGroup_IdNo = b.ItemGroup_IdNo LEFT OUTER JOIN ClothType_Head d ON a.ClothType_idno = d.ClothType_idno where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Bale_Transfer_Code = '" & Trim(NewCode) & "' Order by a.Sl_No", con)
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

        Printing_Delivery_Format1(e)

    End Sub

    Private Sub Printing_Delivery_Format1(ByRef e As System.Drawing.Printing.PrintPageEventArgs)
        Dim pFont As Font
        'Dim ps As Printing.PaperSize
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
        Dim ItmNm1 As String, ItmNm2 As String


        set_PaperSize_For_PrintDocument1()

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

        '    Dim pkCustomSize1 As New System.Drawing.Printing.PaperSize("PAPER 8X6", 800, 600)
        '    PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = pkCustomSize1
        '    PrintDocument1.DefaultPageSettings.PaperSize = pkCustomSize1

        '    'For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
        '    '    If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.GermanStandardFanfold Then
        '    '        ps = PrintDocument1.PrinterSettings.PaperSizes(I)
        '    '        PrintDocument1.DefaultPageSettings.PaperSize = ps
        '    '        e.PageSettings.PaperSize = ps
        '    '        PpSzSTS = True
        '    '        Exit For
        '    '    End If
        '    'Next

        '    'If PpSzSTS = False Then
        '    '    For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
        '    '        If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
        '    '            ps = PrintDocument1.PrinterSettings.PaperSizes(I)
        '    '            PrintDocument1.DefaultPageSettings.PaperSize = ps
        '    '            e.PageSettings.PaperSize = ps
        '    '            Exit For
        '    '        End If
        '    '    Next
        '    'End If

        'End If

        With PrintDocument1.DefaultPageSettings.Margins
            .Left = 30
            .Right = 45
            .Top = 8 ' 15 ' 30
            .Bottom = 30
            LMargin = .Left
            RMargin = .Right
            TMargin = .Top
            BMargin = .Bottom
        End With

        pFont = New Font("Calibri", 10, FontStyle.Regular)
        'pFont = New Font("Calibri", 11, FontStyle.Regular)

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

        NoofItems_PerPage = 3
        If Trim(prn_HdDt.Rows(0).Item("Note").ToString) <> "" Then
            NoofItems_PerPage = NoofItems_PerPage - 1
        End If
        'If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1168" Then
        '    NoofItems_PerPage = 4

        'Else
        '    NoofItems_PerPage = 5

        'End If

        Erase LnAr
        Erase ClAr

        LnAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        ClAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}

        ClAr(1) = Val(35) : ClAr(2) = 290 : ClAr(3) = 105 : ClAr(4) = 80 : ClAr(5) = 110
        ClAr(6) = PageWidth - (LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5))

        TxtHgt = 16 ' 17.5 '18

        EntryCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(EntFnYrCode)

        Try
            If prn_HdDt.Rows.Count > 0 Then

                Printing_Delivery_Format1_PageHeader(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, prn_PageNo, NoofItems_PerPage, CurY, LnAr, ClAr)

                NoofDets = 0

                CurY = CurY - 10

                If prn_DetDt.Rows.Count > 0 Then

                    Do While prn_DetIndx <= prn_DetDt.Rows.Count - 1

                        If NoofDets >= NoofItems_PerPage Then

                            CurY = CurY + TxtHgt

                            Common_Procedures.Print_To_PrintDocument(e, "Continued...", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + 10, CurY, 0, 0, pFont)

                            NoofDets = NoofDets + 1

                            Printing_Delivery_Format1_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClAr, NoofDets, False)

                            e.HasMorePages = True
                            Return

                        End If

                        ItmNm1 = Trim(prn_DetDt.Rows(prn_DetIndx).Item("Cloth_Name").ToString)
                        ItmNm2 = ""
                        If Len(ItmNm1) > 40 Then
                            For I = 40 To 1 Step -1
                                If Mid$(Trim(ItmNm1), I, 1) = " " Or Mid$(Trim(ItmNm1), I, 1) = "," Or Mid$(Trim(ItmNm1), I, 1) = "." Or Mid$(Trim(ItmNm1), I, 1) = "-" Or Mid$(Trim(ItmNm1), I, 1) = "/" Or Mid$(Trim(ItmNm1), I, 1) = "_" Or Mid$(Trim(ItmNm1), I, 1) = "(" Or Mid$(Trim(ItmNm1), I, 1) = ")" Or Mid$(Trim(ItmNm1), I, 1) = "\" Or Mid$(Trim(ItmNm1), I, 1) = "[" Or Mid$(Trim(ItmNm1), I, 1) = "]" Or Mid$(Trim(ItmNm1), I, 1) = "{" Or Mid$(Trim(ItmNm1), I, 1) = "}" Then Exit For
                            Next I
                            If I = 0 Then I = 40
                            ItmNm2 = Microsoft.VisualBasic.Right(Trim(ItmNm1), Len(ItmNm1) - I)
                            ItmNm1 = Microsoft.VisualBasic.Left(Trim(ItmNm1), I - 1)
                        End If

                        CurY = CurY + TxtHgt
                        Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetDt.Rows(prn_DetIndx).Item("Sl_No").ToString), LMargin + 15, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm1), LMargin + ClAr(1) + 10, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Item_HSN_Code").ToString, LMargin + ClAr(1) + ClAr(2) + 10, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(prn_DetIndx).Item("Bales").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) - 10, CurY, 1, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(prn_DetIndx).Item("Pcs").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 1, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Meters").ToString, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY, 1, 0, pFont)

                        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1168" Then
                            If Val(prn_DetDt.Rows(prn_DetIndx).Item("Fold_Perc").ToString) <> 0 And Val(prn_DetDt.Rows(prn_DetIndx).Item("Fold_Perc").ToString) <> 100 Then
                                CurY = CurY + TxtHgt
                                Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(prn_DetIndx).Item("Fold_Perc").ToString) & " CM  Folding ", LMargin + ClAr(1) + 10, CurY, 0, 0, pFont)
                                NoofDets = NoofDets + 1
                            End If
                        End If

                        NoofDets = NoofDets + 1

                        If Trim(ItmNm2) <> "" Then
                            CurY = CurY + TxtHgt - 5
                            Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm2), LMargin + ClAr(1) + 10, CurY, 0, 0, pFont)
                            NoofDets = NoofDets + 1
                        End If

                        prn_DetIndx = prn_DetIndx + 1

                    Loop

                End If

                Printing_Delivery_Format1_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClAr, NoofDets, True)

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

        e.HasMorePages = False

    End Sub

    Private Sub Printing_Delivery_Format1_PageHeader(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByRef PageNo As Integer, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single)
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt2 As New DataTable
        Dim p1Font As Font
        Dim Cmp_Name As String, Cmp_Add1 As String, Cmp_Add2 As String
        Dim Cmp_PhNo As String, Cmp_TinNo As String, Cmp_CstNo As String, Cmp_Email As String
        Dim Cmp_StateCap As String, Cmp_StateNm As String, Cmp_StateCode As String, Cmp_GSTIN_Cap As String, Cmp_GSTIN_No As String
        Dim strHeight As Single, strWidth As Single = 0
        Dim C1 As Single
        Dim W1, w2 As Single
        Dim S1, S2 As Single
        Dim vprn_BlNos As String = ""
        Dim Gst_dt As Date
        Dim Entry_dt As Date
        Dim CurX As Single = 0
        Dim vPackType As String = ""

        PageNo = PageNo + 1

        CurY = TMargin

        da2 = New SqlClient.SqlDataAdapter("select a.*, b.Cloth_name, d.ClothType_name from Bale_Transfer_Details a INNER JOIN Cloth_Head b ON a.Cloth_idno = b.Cloth_idno LEFT OUTER JOIN ClothType_Head d ON a.ClothType_idno = d.ClothType_idno where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Bale_Transfer_Code = '" & Trim(EntryCode) & "' Order by a.Sl_No", con)
        da2.Fill(dt2)

        If dt2.Rows.Count > NoofItems_PerPage Then
            Common_Procedures.Print_To_PrintDocument(e, "Page : " & Trim(Val(PageNo)), PageWidth - 10, CurY - TxtHgt, 1, 0, pFont)
        End If
        dt2.Clear()

        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(1) = CurY



        Cmp_Name = "" : Cmp_Add1 = "" : Cmp_Add2 = ""
        Cmp_PhNo = "" : Cmp_TinNo = "" : Cmp_CstNo = "" : Cmp_Email = ""
        Cmp_StateCap = "" : Cmp_StateNm = "" : Cmp_StateCode = "" : Cmp_GSTIN_Cap = "" : Cmp_GSTIN_No = ""
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
        If Trim(prn_HdDt.Rows(0).Item("Company_EMail").ToString) <> "" Then
            Cmp_Email = "E-mail : " & Trim(prn_HdDt.Rows(0).Item("Company_EMail").ToString)
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


        CurY = CurY + TxtHgt - 10
            p1Font = New Font("Calibri", 18, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, Cmp_Name, LMargin, CurY, 2, PrintWidth, p1Font)
            strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height

            CurY = CurY + strHeight - 1
            Common_Procedures.Print_To_PrintDocument(e, Cmp_Add1, LMargin, CurY, 2, PrintWidth, pFont)

            CurY = CurY + TxtHgt - 1
            Common_Procedures.Print_To_PrintDocument(e, Cmp_Add2, LMargin, CurY, 2, PrintWidth, pFont)

            Gst_dt = #7/1/2017#
            Entry_dt = dtp_Date.Value

        If DateDiff("d", Gst_dt.ToShortDateString, Entry_dt.ToShortDateString) < 0 Then
            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, Cmp_PhNo, LMargin, CurY, 2, PrintWidth, pFont)
            CurY = CurY + TxtHgt
            CurY = CurY + TxtHgt - 1
            Common_Procedures.Print_To_PrintDocument(e, Cmp_TinNo, LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Cmp_CstNo, PageWidth - 10, CurY, 1, 0, pFont)

            CurY = CurY + TxtHgt - 13  ' 10

        Else

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
            Common_Procedures.Print_To_PrintDocument(e, Trim(Cmp_PhNo & "   " & Cmp_Email), LMargin, CurY, 2, PrintWidth, pFont)

            CurY = CurY + TxtHgt - 1

        End If


        'CurY = CurY + TxtHgt - 5
        p1Font = New Font("Calibri", 16, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "FABRIC DELIVERY CHALLAN", LMargin, CurY, 2, PrintWidth, p1Font)
        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height

        CurY = CurY + strHeight + 5
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

        LnAr(2) = CurY

        C1 = ClAr(1) + ClAr(2) + ClAr(3)
        W1 = e.Graphics.MeasureString("ORDER NO : ", pFont).Width
        w2 = e.Graphics.MeasureString("DESP.TO : ", pFont).Width
        S1 = e.Graphics.MeasureString("TO  :  ", pFont).Width
        s2 = e.Graphics.MeasureString("TRANSPORT :  ", pFont).Width

        CurY = CurY + TxtHgt - 10
        p1Font = New Font("Calibri", 12, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "TO  :  " & "M/s." & prn_HdDt.Rows(0).Item("Ledger_MAInName").ToString, LMargin + 10, CurY, 0, 0, p1Font)
        Common_Procedures.Print_To_PrintDocument(e, "DC.NO", LMargin + C1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Bale_Transfer_No").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, p1Font)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address1").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
        p1Font = New Font("Calibri", 14, FontStyle.Bold)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address2").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, "DATE", LMargin + C1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Bale_Transfer_Date").ToString), "dd-MM-yyyy").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, pFont)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address3").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
        'If prn_HdDt.Rows(0).Item("Party_OrderNo").ToString <> "" Then
        '    Common_Procedures.Print_To_PrintDocument(e, "ORDER NO", LMargin + C1 + 10, CurY, 0, 0, pFont)
        '    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
        '    Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Party_OrderNo").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, p1Font)
        'End If

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address4").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)

        CurY = CurY + TxtHgt
        If DateDiff("d", Gst_dt.ToShortDateString, Entry_dt.ToShortDateString) < 0 Then
            If Trim(prn_HdDt.Rows(0).Item("Ledger_TinNo").ToString) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, " TIN NO : " & Trim(prn_HdDt.Rows(0).Item("Ledger_TinNo").ToString), LMargin + S1 + 10, CurY, 0, 0, pFont)
            End If

        Else
            If Trim(prn_HdDt.Rows(0).Item("ledger_GSTinNo").ToString) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, " GSTIN NO : " & Trim(prn_HdDt.Rows(0).Item("ledger_GSTinNo").ToString), LMargin + S1 + 10, CurY, 0, 0, pFont)
            End If

        End If

        CurY = CurY + TxtHgt + 5
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(3) = CurY

        e.Graphics.DrawLine(Pens.Black, LMargin + C1, CurY, LMargin + C1, LnAr(2))
        CurY = CurY + 5

        Common_Procedures.Print_To_PrintDocument(e, "VEHICLE NO  ", LMargin + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, " :  ", LMargin + s2 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Vechile_No").ToString, LMargin + S2 + 30, CurY, 0, 0, pFont)




        Common_Procedures.Print_To_PrintDocument(e, "DESP.TO", LMargin + C1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + w2 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Despatch_To").ToString, LMargin + C1 + w2 + 30, CurY, 0, 0, pFont)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Delivery_Address1").ToString, LMargin + C1 + w2 + 30, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, "LR.NO  ", LMargin + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, " :  ", LMargin + s2 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Lr_No").ToString, LMargin + s2 + 30, CurY, 0, 0, pFont)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, "TRANSPORT ", LMargin + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, " :  ", LMargin + s2 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("TransportName").ToString, LMargin + s2 + 30, CurY, 0, 0, pFont)

        Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Delivery_Address2").ToString, LMargin + C1 + w2 + 30, CurY, 0, 0, pFont)

        'CurY = CurY + TxtHgt
        'Common_Procedures.Print_To_PrintDocument(e, "AGENT ", LMargin + 10, CurY, 0, 0, pFont)
        'Common_Procedures.Print_To_PrintDocument(e, " :  ", LMargin + s2 + 10, CurY, 0, 0, pFont)
        'Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Agent_Name").ToString, LMargin + s2 + 30, CurY, 0, 0, pFont)

        'vprn_BlNos = ""
        'For i = 0 To prn_DetDt.Rows.Count - 1
        '    If Trim(prn_DetDt.Rows(i).Item("Bales_Nos").ToString) <> "" Then
        '        vprn_BlNos = Trim(vprn_BlNos) & IIf(Trim(vprn_BlNos) <> "", ", ", "") & prn_DetDt.Rows(i).Item("Bales_Nos").ToString
        '    End If
        'Next

        vPackType = Trim(UCase(prn_HdDt.Rows(0).Item("Packing_Type").ToString))
        If Trim(vPackType) = "" Then vPackType = "BALE"
        'Common_Procedures.Print_To_PrintDocument(e, Trim(vPackType) & " NOS : " & vprn_BlNos, LMargin + C1 + 10, CurY, 0, 0, pFont)

        CurY = CurY + TxtHgt + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(3) = CurY

        e.Graphics.DrawLine(Pens.Black, LMargin + C1, CurY, LMargin + C1, LnAr(2))

        CurY = CurY + TxtHgt - 10
        Common_Procedures.Print_To_PrintDocument(e, "SNO", LMargin, CurY, 2, ClAr(1), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "CLOTH NAME", LMargin + ClAr(1), CurY, 2, ClAr(2), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "HSN CODE", LMargin + ClAr(1) + ClAr(2), CurY, 2, ClAr(3), pFont)
        Common_Procedures.Print_To_PrintDocument(e, Trim(vPackType) & "S", LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, 2, ClAr(4), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "PCS", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 2, ClAr(5), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "METERS", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, 2, ClAr(6), pFont)


        CurY = CurY + TxtHgt + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(4) = CurY


    End Sub

    Private Sub Printing_Delivery_Format1_PageFooter(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single, ByVal NoofDets As Integer, ByVal is_LastPage As Boolean)
        Dim i As Integer
        Dim Cmp_Name As String
        Dim p1Font As Font
        Dim W1 As Single = 0
        Dim vprn_BlNos As String = ""
        Dim vPackType As String = ""
        Dim BLNo1 As String = ""
        Dim BLNo2 As String = ""


        For i = NoofDets + 1 To NoofItems_PerPage
            CurY = CurY + TxtHgt
        Next

        CurY = CurY + TxtHgt
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(5) = CurY

        CurY = CurY + TxtHgt - 10
        If is_LastPage = True Then
            Common_Procedures.Print_To_PrintDocument(e, " TOTAL", LMargin + ClAr(1) + 30, CurY, 2, ClAr(4), pFont)
            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Total_Meters").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY, 1, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(0).Item("Total_Bales").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) - 10, CurY, 1, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Total_Pcs").ToString), "#########0"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 1, 0, pFont)
        End If

        CurY = CurY + TxtHgt + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(6) = CurY

        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1), CurY, LMargin + ClAr(1), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2), CurY, LMargin + ClAr(1) + ClAr(2), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(3))
        CurY = CurY + 10


        vprn_BlNos = ""
        For i = 0 To prn_DetDt.Rows.Count - 1
            If Trim(prn_DetDt.Rows(i).Item("Bales_Nos").ToString) <> "" Then
                vprn_BlNos = Trim(vprn_BlNos) & IIf(Trim(vprn_BlNos) <> "", ", ", "") & prn_DetDt.Rows(i).Item("Bales_Nos").ToString
            End If
        Next

        BLNo1 = Trim(vprn_BlNos)
        BLNo2 = ""

        If Len(BLNo1) > 75 Then
            For i = 75 To 1 Step -1
                If Mid$(Trim(BLNo1), i, 1) = " " Or Mid$(Trim(BLNo1), i, 1) = "," Or Mid$(Trim(BLNo1), i, 1) = "." Or Mid$(Trim(BLNo1), i, 1) = "-" Or Mid$(Trim(BLNo1), i, 1) = "/" Or Mid$(Trim(BLNo1), i, 1) = "_" Or Mid$(Trim(BLNo1), i, 1) = "(" Or Mid$(Trim(BLNo1), i, 1) = ")" Or Mid$(Trim(BLNo1), i, 1) = "\" Or Mid$(Trim(BLNo1), i, 1) = "[" Or Mid$(Trim(BLNo1), i, 1) = "]" Or Mid$(Trim(BLNo1), i, 1) = "{" Or Mid$(Trim(BLNo1), i, 1) = "}" Then Exit For
            Next i
            If i = 0 Then i = 75
            BLNo2 = Microsoft.VisualBasic.Right(Trim(BLNo1), Len(BLNo1) - i)
            BLNo1 = Microsoft.VisualBasic.Left(Trim(BLNo1), i)
        End If


        vPackType = Trim(UCase(prn_HdDt.Rows(0).Item("Packing_Type").ToString))
        If Trim(vPackType) = "" Then vPackType = "BALE"
        Common_Procedures.Print_To_PrintDocument(e, Trim(vPackType) & " NOS : " & BLNo1, LMargin + 10, CurY, 0, 0, pFont)
        CurY = CurY + TxtHgt

        Common_Procedures.Print_To_PrintDocument(e, "        " & BLNo2, LMargin + 10, CurY, 0, 0, pFont)
        CurY = CurY + TxtHgt

        'If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1168" Then
        If Trim(prn_HdDt.Rows(0).Item("Note").ToString) <> "" Then
            Common_Procedures.Print_To_PrintDocument(e, "NOTE : " & prn_HdDt.Rows(0).Item("Note").ToString, LMargin + 10, CurY, 0, 0, pFont)
            CurY = CurY + TxtHgt
        End If
        'End If

        If Common_Procedures.User.IdNo <> 1 Then
            Common_Procedures.Print_To_PrintDocument(e, "(" & Trim(Common_Procedures.User.Name) & ")", LMargin + 20, CurY, 0, 0, pFont)
        End If

        CurY = CurY + TxtHgt

        Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString
        p1Font = New Font("Calibri", 12, FontStyle.Bold)


        Common_Procedures.Print_To_PrintDocument(e, "Prepared By", LMargin + 20, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, "Checked By ", LMargin + 250, CurY, 0, 0, pFont)

        Common_Procedures.Print_To_PrintDocument(e, "For " & Cmp_Name, PageWidth - 15, CurY, 1, 0, p1Font)

        CurY = CurY + TxtHgt + 10

        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

        e.Graphics.DrawLine(Pens.Black, LMargin, LnAr(1), LMargin, CurY)
        e.Graphics.DrawLine(Pens.Black, PageWidth, LnAr(1), PageWidth, CurY)

    End Sub

    Private Sub dgv_Filter_Details_CellDoubleClick1(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Filter_Details.CellDoubleClick
        Open_FilterEntry()
    End Sub

    Private Sub dgv_Filter_Details_KeyDown1(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_Filter_Details.KeyDown
        If e.KeyCode = 13 Then
            Open_FilterEntry()
        End If
    End Sub

    Private Sub txt_Freight_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Freight.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
            e.Handled = True
        End If
    End Sub

    Private Sub btn_Selection_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Selection.Click
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim i As Integer, j As Integer, n As Integer, SNo As Integer
        Dim LedIdNo As Integer
        Dim NewCode As String
        Dim CompIDCondt As String
        Dim Ent_Bls As Single = 0
        Dim Ent_BlNos As String = ""
        Dim Ent_Pcs As Single = 0
        Dim Ent_Mtrs As Single = 0
        Dim Ent_Rate As Single = 0

        LedIdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_TransferGodownTo.Text)

        If LedIdNo = 0 Then
            MessageBox.Show("Select Party Name", "DOES NOT SELECT ORDER...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If cbo_TransferGodownTo.Enabled And cbo_TransferGodownTo.Visible Then cbo_TransferGodownTo.Focus()
            Exit Sub
        End If

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(EntFnYrCode)

        CompIDCondt = "(a.company_idno = " & Str(Val(lbl_Company.Tag)) & ")"
        If Trim(UCase(Common_Procedures.settings.CompanyName)) = "-----~~~" Then
            CompIDCondt = ""
        End If


        With dgv_Selection

            .Rows.Clear()
            SNo = 0

            Da = New SqlClient.SqlDataAdapter("select a.*, b.*, c.Cloth_Name, d.Ledger_Name as agentname, e.Ledger_Name as Transportname,  g.ClothType_name, h.Bales as Ent_Bales, h.Bales_Nos as Ent_Bales_Nos, h.Pcs as Ent_Pcs, h.Meters as Ent_DcMeters,h.Rate as Ent_Rate from ClothSales_Order_Head a INNER JOIN Clothsales_Order_details b ON a.ClothSales_Order_Code = b.ClothSales_Order_Code INNER JOIN Cloth_Head c ON b.Cloth_IdNo = c.Cloth_IdNo INNER JOIN ClothType_Head g ON b.ClothType_IdNo = g.ClothType_IdNo LEFT OUTER JOIN Ledger_Head d ON a.Agent_IdNo = d.Ledger_IdNo LEFT OUTER JOIN Ledger_Head e ON a.Transport_IdNo = e.Ledger_IdNo LEFT OUTER JOIN Bale_Transfer_Details h ON h.Bale_Transfer_Code = '" & Trim(NewCode) & "' and b.ClothSales_Order_Code = h.ClothSales_Order_Code and b.ClothSales_Order_SlNo = h.ClothSales_Order_SlNo Where " & CompIDCondt & IIf(Trim(CompIDCondt) <> "", " and ", "") & " a.ledger_Idno = " & Str(Val(LedIdNo)) & " and ((b.Order_Meters - b.Order_Cancel_Meters - b.Delivery_Meters) > 0 or h.Meters > 0 ) order by a.ClothSales_Order_Date, a.for_orderby, a.ClothSales_Order_No", con)
            Dt1 = New DataTable
            Da.Fill(Dt1)

            If Dt1.Rows.Count > 0 Then

                For i = 0 To Dt1.Rows.Count - 1

                    n = .Rows.Add()



                    Ent_Bls = 0
                    Ent_BlNos = ""
                    Ent_Pcs = 0
                    Ent_Mtrs = 0
                    Ent_Rate = 0
                    If IsDBNull(Dt1.Rows(i).Item("Ent_Bales").ToString) = False Then
                        Ent_Bls = Val(Dt1.Rows(i).Item("Ent_Bales").ToString)
                    End If
                    If IsDBNull(Dt1.Rows(i).Item("Ent_Bales_Nos").ToString) = False Then
                        Ent_BlNos = Dt1.Rows(i).Item("Ent_Bales_Nos").ToString
                    End If
                    If IsDBNull(Dt1.Rows(i).Item("Ent_Pcs").ToString) = False Then
                        Ent_Pcs = Val(Dt1.Rows(i).Item("Ent_Pcs").ToString)
                    End If
                    If IsDBNull(Dt1.Rows(i).Item("Ent_DcMeters").ToString) = False Then
                        Ent_Mtrs = Val(Dt1.Rows(i).Item("Ent_DcMeters").ToString)
                    End If

                    If IsDBNull(Dt1.Rows(i).Item("Ent_Rate").ToString) = False Then
                        Ent_Rate = Val(Dt1.Rows(i).Item("Ent_Rate").ToString)
                    End If

                    SNo = SNo + 1
                    .Rows(n).Cells(0).Value = Val(SNo)
                    .Rows(n).Cells(1).Value = Dt1.Rows(i).Item("Cloth_Name").ToString
                    .Rows(n).Cells(2).Value = Dt1.Rows(i).Item("ClothType_Name").ToString
                    .Rows(n).Cells(3).Value = Val(Dt1.Rows(i).Item("Fold_Perc").ToString)
                    .Rows(n).Cells(4).Value = Val(Dt1.Rows(i).Item("Order_Pcs").ToString)
                    .Rows(n).Cells(5).Value = Format(Val(Dt1.Rows(i).Item("Order_Meters").ToString) - Val(Dt1.Rows(i).Item("Order_Cancel_Meters").ToString) - Val(Dt1.Rows(i).Item("Delivery_Meters").ToString) - Val(Dt1.Rows(i).Item("Invoice_Meters").ToString) + Val(Ent_Mtrs), "#########0.00")
                    '.Rows(n).Cells(6).Value =
                    '.Rows(n).Cells(7).Value = 

                    If Ent_Mtrs > 0 Then
                        .Rows(n).Cells(8).Value = "1"
                        For j = 0 To .ColumnCount - 1
                            .Rows(n).Cells(j).Style.ForeColor = Color.Red
                        Next

                    Else
                        .Rows(n).Cells(8).Value = ""

                    End If

                    .Rows(n).Cells(9).Value = Dt1.Rows(i).Item("agentname").ToString
                    .Rows(n).Cells(10).Value = Dt1.Rows(i).Item("Transportname").ToString
                    .Rows(n).Cells(11).Value = Dt1.Rows(i).Item("Through_Name").ToString
                    .Rows(n).Cells(12).Value = Dt1.Rows(i).Item("Despatch_To").ToString
                    .Rows(n).Cells(13).Value = Dt1.Rows(i).Item("Delivery_Address1").ToString
                    .Rows(n).Cells(14).Value = Dt1.Rows(i).Item("Delivery_Address2").ToString
                    .Rows(n).Cells(15).Value = Dt1.Rows(i).Item("Clothsales_Order_Code").ToString
                    .Rows(n).Cells(16).Value = Dt1.Rows(i).Item("Clothsales_Order_SlNo").ToString

                    .Rows(n).Cells(17).Value = Val(Ent_Bls)
                    .Rows(n).Cells(18).Value = Ent_BlNos
                    .Rows(n).Cells(19).Value = Ent_Pcs
                    .Rows(n).Cells(20).Value = Ent_Mtrs
                    .Rows(n).Cells(21).Value = Val(Dt1.Rows(i).Item("Rate").ToString)
                    .Rows(n).Cells(22).Value = Ent_Rate
                Next

            End If
            Dt1.Clear()

        End With

        pnl_Selection.Visible = True
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

    Private Sub btn_Close_Selection_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Close_Selection.Click
        Cloth_Invoice_Selection()
    End Sub

    Private Sub Cloth_Invoice_Selection()
        Dim Da1 As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim n As Integer = 0
        Dim sno As Integer = 0
        Dim i As Integer = 0
        Dim j As Integer = 0

        dgv_Details.Rows.Clear()

        For i = 0 To dgv_Selection.RowCount - 1

            If Val(dgv_Selection.Rows(i).Cells(8).Value) = 1 Then

                cbo_DespTo.Text = dgv_Selection.Rows(i).Cells(12).Value
                cbo_Transport.Text = dgv_Selection.Rows(i).Cells(10).Value

                If txt_DelvAdd1.Text = "" Then
                    If (dgv_Selection.Rows(i).Cells(13).Value) <> "" Then
                        txt_DelvAdd1.Text = dgv_Selection.Rows(i).Cells(13).Value
                    End If
                End If

                If txt_DelvAdd2.Text = "" Then
                    If (dgv_Selection.Rows(i).Cells(14).Value) <> "" Then
                        txt_DelvAdd2.Text = dgv_Selection.Rows(i).Cells(14).Value
                    End If
                End If

                n = dgv_Details.Rows.Add()
                sno = sno + 1
                dgv_Details.Rows(n).Cells(0).Value = Val(sno)
                dgv_Details.Rows(n).Cells(1).Value = dgv_Selection.Rows(i).Cells(3).Value
                dgv_Details.Rows(n).Cells(2).Value = dgv_Selection.Rows(i).Cells(4).Value
                dgv_Details.Rows(n).Cells(3).Value = dgv_Selection.Rows(i).Cells(5).Value

                If Val(dgv_Selection.Rows(i).Cells(17).Value) <> 0 Then
                    dgv_Details.Rows(n).Cells(4).Value = dgv_Selection.Rows(i).Cells(17).Value
                End If
                dgv_Details.Rows(n).Cells(5).Value = dgv_Selection.Rows(i).Cells(18).Value

                If Val(dgv_Selection.Rows(i).Cells(19).Value) <> 0 Then
                    dgv_Details.Rows(n).Cells(6).Value = dgv_Selection.Rows(i).Cells(19).Value
                Else
                    dgv_Details.Rows(n).Cells(6).Value = dgv_Selection.Rows(i).Cells(6).Value
                End If

                If Val(dgv_Selection.Rows(i).Cells(20).Value) <> 0 Then
                    dgv_Details.Rows(n).Cells(7).Value = dgv_Selection.Rows(i).Cells(20).Value
                Else
                    dgv_Details.Rows(n).Cells(7).Value = dgv_Selection.Rows(i).Cells(7).Value
                End If

                If Val(dgv_Selection.Rows(i).Cells(22).Value) <> 0 Then
                    dgv_Details.Rows(n).Cells(13).Value = dgv_Selection.Rows(i).Cells(22).Value
                Else
                    dgv_Details.Rows(n).Cells(13).Value = dgv_Selection.Rows(i).Cells(13).Value
                End If

                dgv_Details.Rows(n).Cells(8).Value = dgv_Selection.Rows(i).Cells(15).Value
                dgv_Details.Rows(n).Cells(9).Value = dgv_Selection.Rows(i).Cells(16).Value
                dgv_Details.Rows(n).Cells(13).Value = dgv_Selection.Rows(i).Cells(21).Value


            End If

        Next

        Total_Calculation()

        pnl_Back.Enabled = True
        pnl_Selection.Visible = False
        If txt_LrNo.Enabled And txt_LrNo.Visible Then txt_LrNo.Focus()

    End Sub

    Private Sub btn_BaleSelection_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_BaleSelection.Click
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim i As Integer, j As Integer, n As Integer, SNo As Integer
        Dim Clo_ID As Integer, CloType_ID As Integer
        Dim NewCode As String = ""
        Dim Fd_Perc As String = 0
        Dim CompIDCondt As String
        Dim dgvDet_CurRow As Integer
        Dim dgv_DetSlNo As Long
        Dim vGod_ID As Integer = 0

        Try

            If dgv_Details.CurrentCell.RowIndex < 0 Then
                MessageBox.Show("Select Cloth Name & Type Selection", "DOES NOT SELECT BALE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                If dgv_Details.Enabled And dgv_Details.Visible Then
                    If dgv_Details.Rows.Count > 0 Then
                        dgv_Details.Focus()
                        dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(4)
                        dgv_Details.CurrentCell.Selected = True
                    End If
                End If
                Exit Sub
            End If

            vGod_ID = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_TransferGodownFrom.Text)
            If cbo_TransferGodownFrom.Visible = True Then
                If vGod_ID = 0 Then
                    MessageBox.Show("Invalid Fabric Godown Name", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                    If cbo_TransferGodownFrom.Enabled And cbo_TransferGodownFrom.Visible Then cbo_TransferGodownFrom.Focus()
                    Exit Sub
                End If
            End If
            If vGod_ID = 0 Then vGod_ID = Common_Procedures.CommonLedger.Godown_Ac


            Clo_ID = Common_Procedures.Cloth_NameToIdNo(con, dgv_Details.Rows(dgv_Details.CurrentCell.RowIndex).Cells(1).Value)
            If Clo_ID = 0 Then
                MessageBox.Show("Select Cloth Name", "DOES NOT SELECT BALE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                If dgv_Details.Enabled And dgv_Details.Visible Then
                    If dgv_Details.Rows.Count > 0 Then
                        dgv_Details.Focus()
                        dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(1)
                        If cbo_Grid_ClothName.Visible And cbo_Grid_ClothName.Enabled Then cbo_Grid_ClothName.Focus()
                        'dgv_Details.CurrentCell.Selected = True
                        Exit Sub
                    End If
                End If
                Exit Sub
            End If

            CloType_ID = Common_Procedures.ClothType_NameToIdNo(con, dgv_Details.Rows(dgv_Details.CurrentCell.RowIndex).Cells(2).Value)
            If CloType_ID = 0 Then
                MessageBox.Show("Select Cloth Type ", "DOES NOT SELECT BALE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                If dgv_Details.Enabled And dgv_Details.Visible Then
                    If dgv_Details.Rows.Count > 0 Then
                        dgv_Details.Focus()
                        dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(2)
                        If cbo_Grid_Clothtype.Visible And cbo_Grid_Clothtype.Enabled Then cbo_Grid_Clothtype.Focus()
                        Exit Sub
                    End If
                End If
                Exit Sub
            End If

            Fd_Perc = dgv_Details.Rows(dgv_Details.CurrentCell.RowIndex).Cells(3).Value
            If Val(Fd_Perc) = 0 Then Fd_Perc = 100
            If Val(Fd_Perc) = 0 Then
                MessageBox.Show("Select Folding", "DOES NOT SELECT BALE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                If dgv_Details.Enabled And dgv_Details.Visible Then
                    If dgv_Details.Rows.Count > 0 Then
                        dgv_Details.Focus()
                        dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(3)
                        dgv_Details.CurrentCell.Selected = True
                    End If
                End If
                Exit Sub
            End If

            CompIDCondt = "(a.company_idno = " & Str(Val(lbl_Company.Tag)) & ")"
            If Trim(UCase(Common_Procedures.settings.CompanyName)) = "-----~~~" Then
                CompIDCondt = ""
            End If

            CompIDCondt = Trim(CompIDCondt) & IIf(Trim(CompIDCondt) <> "", " and ", "") & "(a.ledger_idno = " & Str(Common_Procedures.CommonLedger.OwnSort_Ac) & " or a.ledger_idno = " & Str(Common_Procedures.CommonLedger.Godown_Ac) & ")"

            If cbo_TransferGodownFrom.Visible = True Then
                CompIDCondt = Trim(CompIDCondt) & IIf(Trim(CompIDCondt) <> "", " and ", "") & "(a.WareHouse_idno = " & Str(vGod_ID) & ")"
            End If

            NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(EntFnYrCode)

            dgvDet_CurRow = dgv_Details.CurrentCell.RowIndex
            dgv_DetSlNo = Val(dgv_Details.Rows(dgvDet_CurRow).Cells(10).Value)

            With dgv_BaleSelection

                .Rows.Clear()
                SNo = 0

                Da = New SqlClient.SqlDataAdapter("Select a.* from Packing_Slip_Head a Where " & CompIDCondt & IIf(Trim(CompIDCondt) <> "", " and ", "") & " a.Delivery_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' and a.Delivery_DetailsSlNo = " & Str(Val(dgv_DetSlNo)) & " and a.Cloth_IdNo = " & Str(Val(Clo_ID)) & "  and a.ClothType_IdNo = " & Str(Val(CloType_ID)) & "  and a.Folding = " & Str(Val(Fd_Perc)) & " order by a.Packing_Slip_Date, a.for_orderby, a.Packing_Slip_No, a.Packing_Slip_Code", con)
                Dt1 = New DataTable
                Da.Fill(Dt1)

                If Dt1.Rows.Count > 0 Then

                    For i = 0 To Dt1.Rows.Count - 1

                        n = .Rows.Add()

                        SNo = SNo + 1
                        .Rows(n).Cells(0).Value = Val(SNo)
                        .Rows(n).Cells(1).Value = Dt1.Rows(i).Item("Packing_Slip_No").ToString
                        If Val(Dt1.Rows(i).Item("Total_Pcs").ToString) <> 0 Then
                            .Rows(n).Cells(2).Value = Val(Dt1.Rows(i).Item("Total_Pcs").ToString)
                        End If
                        If Val(Dt1.Rows(i).Item("Total_Meters").ToString) <> 0 Then
                            .Rows(n).Cells(3).Value = Format(Val(Dt1.Rows(i).Item("Total_Meters").ToString), "#########0.00")
                        End If
                        If Val(Dt1.Rows(i).Item("Total_Weight").ToString) <> 0 Then
                            .Rows(n).Cells(4).Value = Format(Val(Dt1.Rows(i).Item("Total_Weight").ToString), "#########0.000")
                        End If
                        .Rows(n).Cells(5).Value = "1"
                        .Rows(n).Cells(6).Value = Dt1.Rows(i).Item("Packing_Slip_Code").ToString
                        .Rows(n).Cells(7).Value = Dt1.Rows(i).Item("Bale_Bundle").ToString

                        For j = 0 To .ColumnCount - 1
                            .Rows(i).Cells(j).Style.ForeColor = Color.Red
                        Next

                    Next

                End If
                Dt1.Clear()

                Da = New SqlClient.SqlDataAdapter("select a.* from Packing_Slip_Head a where " & CompIDCondt & IIf(Trim(CompIDCondt) <> "", " and ", "") & " a.Delivery_Code = '' and a.Cloth_IdNo = " & Str(Val(Clo_ID)) & "  and a.ClothType_IdNo = " & Str(Val(CloType_ID)) & "  and a.Folding = " & Str(Val(Fd_Perc)) & " order by a.Packing_Slip_Date, a.for_orderby, a.Packing_Slip_No, a.Packing_Slip_Code", con)
                Dt1 = New DataTable
                Da.Fill(Dt1)

                If Dt1.Rows.Count > 0 Then

                    For i = 0 To Dt1.Rows.Count - 1

                        n = .Rows.Add()

                        SNo = SNo + 1
                        .Rows(n).Cells(0).Value = Val(SNo)
                        .Rows(n).Cells(1).Value = Dt1.Rows(i).Item("Packing_Slip_No").ToString
                        If Val(Dt1.Rows(i).Item("Total_Pcs").ToString) <> 0 Then
                            .Rows(n).Cells(2).Value = Val(Dt1.Rows(i).Item("Total_Pcs").ToString)
                        End If
                        If Val(Dt1.Rows(i).Item("Total_Meters").ToString) <> 0 Then
                            .Rows(n).Cells(3).Value = Format(Val(Dt1.Rows(i).Item("Total_Meters").ToString), "#########0.00")
                        End If
                        If Val(Dt1.Rows(i).Item("Total_Weight").ToString) <> 0 Then
                            .Rows(n).Cells(4).Value = Format(Val(Dt1.Rows(i).Item("Total_Weight").ToString), "#########0.000")
                        End If
                        .Rows(n).Cells(5).Value = ""
                        .Rows(n).Cells(6).Value = Dt1.Rows(i).Item("Packing_Slip_Code").ToString
                        .Rows(n).Cells(7).Value = Dt1.Rows(i).Item("Bale_Bundle").ToString

                    Next

                End If
                Dt1.Clear()


            End With

            pnl_BaleSelection.Visible = True
            pnl_Back.Enabled = False
            dgv_BaleSelection.Focus()
            If dgv_BaleSelection.Rows.Count > 0 Then
                dgv_BaleSelection.CurrentCell = dgv_BaleSelection.Rows(0).Cells(0)
                dgv_BaleSelection.CurrentCell.Selected = True
            End If

        Catch ex As NullReferenceException
            MessageBox.Show("Select the ClothName for Bale Selection", "DOES NOT SELECT BALE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT SELECT BALE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try



    End Sub

    Private Sub dgv_BaleSelection_CellClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_BaleSelection.CellClick
        Select_Bale(e.RowIndex)
    End Sub

    Private Sub Select_Bale(ByVal RwIndx As Integer)
        Dim i As Integer

        With dgv_BaleSelection

            If .RowCount > 0 And RwIndx >= 0 Then

                .Rows(RwIndx).Cells(5).Value = (Val(.Rows(RwIndx).Cells(5).Value) + 1) Mod 2

                If Val(.Rows(RwIndx).Cells(5).Value) = 0 Then .Rows(RwIndx).Cells(5).Value = ""

                For i = 0 To .ColumnCount - 1
                    .Rows(RwIndx).Cells(i).Style.ForeColor = Color.Red
                Next

            End If

        End With

    End Sub

    Private Sub dgv_BaleSelection_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_BaleSelection.KeyDown
        On Error Resume Next

        If e.KeyCode = Keys.Enter Or e.KeyCode = Keys.Space Then
            If dgv_BaleSelection.CurrentCell.RowIndex >= 0 Then
                Select_Bale(dgv_BaleSelection.CurrentCell.RowIndex)
                e.Handled = True
            End If
        End If

        If e.KeyCode = Keys.Delete Or e.KeyCode = Keys.Back Then
            If dgv_BaleSelection.CurrentCell.RowIndex >= 0 Then
                If Val(dgv_BaleSelection.Rows(dgv_BaleSelection.CurrentCell.RowIndex).Cells(5).Value) = 1 Then
                    e.Handled = True
                    Select_Bale(dgv_BaleSelection.CurrentCell.RowIndex)
                End If
            End If
        End If

    End Sub

    Private Sub btn_Close_BaleSelection_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Close_BaleSelection.Click
        Dim Cmd As New SqlClient.SqlCommand
        Dim Da1 As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim I As Integer, J As Integer
        Dim n As Integer
        Dim sno As Integer
        Dim dgvDet_CurRow As Integer = 0
        Dim dgv_DetSlNo As Integer = 0
        Dim NoofBls As Integer
        Dim FsNo As Single, LsNo As Single
        Dim FsBaleNo As String, LsBaleNo As String
        Dim BlNo As String, PackSlpCodes As String
        Dim Tot_Pcs As Single, Tot_Mtrs As Single


        Cmd.Connection = con

        dgvDet_CurRow = dgv_Details.CurrentCell.RowIndex
        dgv_DetSlNo = Val(dgv_Details.Rows(dgvDet_CurRow).Cells(10).Value)

        With dgv_BaleDetails

LOOP1:
            For I = 0 To .RowCount - 1

                If Val(.Rows(I).Cells(0).Value) = Val(dgv_DetSlNo) Then

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

            Cmd.CommandText = "truncate table " & Trim(Common_Procedures.EntryTempTable) & ""
            Cmd.ExecuteNonQuery()

            NoofBls = 0 : Tot_Pcs = 0 : Tot_Mtrs = 0 : BlNo = "" : PackSlpCodes = ""

            For I = 0 To dgv_BaleSelection.RowCount - 1

                If Val(dgv_BaleSelection.Rows(I).Cells(5).Value) = 1 Then

                    n = .Rows.Add()

                    sno = sno + 1
                    .Rows(n).Cells(0).Value = Val(dgv_DetSlNo)
                    .Rows(n).Cells(1).Value = dgv_BaleSelection.Rows(I).Cells(1).Value
                    .Rows(n).Cells(2).Value = Val(dgv_BaleSelection.Rows(I).Cells(2).Value)
                    .Rows(n).Cells(3).Value = Format(Val(dgv_BaleSelection.Rows(I).Cells(3).Value), "#########0.00")
                    .Rows(n).Cells(4).Value = Format(Val(dgv_BaleSelection.Rows(I).Cells(4).Value), "#########0.000")
                    .Rows(n).Cells(5).Value = dgv_BaleSelection.Rows(I).Cells(6).Value
                    .Rows(n).Cells(6).Value = dgv_BaleSelection.Rows(I).Cells(7).Value

                    Cmd.CommandText = "Insert into " & Trim(Common_Procedures.EntryTempTable) & "(Name1, Name2, Meters1) values ('" & Trim(dgv_BaleSelection.Rows(I).Cells(6).Value) & "', '" & Trim(dgv_BaleSelection.Rows(I).Cells(1).Value) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(dgv_BaleSelection.Rows(I).Cells(1).Value))) & " ) "
                    Cmd.ExecuteNonQuery()

                    NoofBls = NoofBls + 1
                    Tot_Pcs = Val(Tot_Pcs) + Val(dgv_BaleSelection.Rows(I).Cells(2).Value)
                    Tot_Mtrs = Val(Tot_Mtrs) + Val(dgv_BaleSelection.Rows(I).Cells(3).Value)
                    PackSlpCodes = Trim(PackSlpCodes) & IIf(Trim(PackSlpCodes) = "", "~", "") & Trim(dgv_BaleSelection.Rows(I).Cells(6).Value) & "~"

                End If

            Next

            BlNo = ""
            FsNo = 0 : LsNo = 0
            FsBaleNo = "" : LsBaleNo = ""

            Da1 = New SqlClient.SqlDataAdapter("Select Name1 as Bale_Code, Name2 as Bale_No, Meters1 as fororderby_baleno from " & Trim(Common_Procedures.EntryTempTable) & " where Name1 <> '' order by Meters1, Name2, Name1", con)
            Dt1 = New DataTable
            Da1.Fill(Dt1)

            If Dt1.Rows.Count > 0 Then

                FsNo = Val(Dt1.Rows(0).Item("fororderby_baleno").ToString)
                LsNo = Val(Dt1.Rows(0).Item("fororderby_baleno").ToString)

                FsBaleNo = Trim(UCase(Dt1.Rows(0).Item("Bale_No").ToString))
                LsBaleNo = Trim(UCase(Dt1.Rows(0).Item("Bale_No").ToString))

                For I = 1 To Dt1.Rows.Count - 1
                    If LsNo + 1 = Val(Dt1.Rows(I).Item("fororderby_baleno").ToString) Then
                        LsNo = Val(Dt1.Rows(I).Item("fororderby_baleno").ToString)
                        LsBaleNo = Trim(UCase(Dt1.Rows(I).Item("Bale_No").ToString))

                    Else
                        If FsNo = LsNo Then
                            BlNo = BlNo & Trim(FsBaleNo) & ","
                        Else
                            BlNo = BlNo & Trim(FsBaleNo) & "-" & Trim(LsBaleNo) & ","
                        End If
                        FsNo = Dt1.Rows(I).Item("fororderby_baleno").ToString
                        LsNo = Dt1.Rows(I).Item("fororderby_baleno").ToString

                        FsBaleNo = Trim(UCase(Dt1.Rows(I).Item("Bale_No").ToString))
                        LsBaleNo = Trim(UCase(Dt1.Rows(I).Item("Bale_No").ToString))

                    End If

                Next

                If FsNo = LsNo Then BlNo = BlNo & Trim(FsBaleNo) Else BlNo = BlNo & Trim(FsBaleNo) & "-" & Trim(LsBaleNo)

            End If
            Dt1.Clear()

            If Trim(dgv_Details.Rows(dgvDet_CurRow).Cells(12).Value) <> "" Then
                dgv_Details.Rows(dgvDet_CurRow).Cells(4).Value = ""
                dgv_Details.Rows(dgvDet_CurRow).Cells(5).Value = ""
                dgv_Details.Rows(dgvDet_CurRow).Cells(6).Value = ""
                dgv_Details.Rows(dgvDet_CurRow).Cells(7).Value = ""
                dgv_Details.Rows(dgvDet_CurRow).Cells(12).Value = ""
            End If
            If Val(NoofBls) <> 0 And Val(Tot_Mtrs) <> 0 Then
                dgv_Details.Rows(dgvDet_CurRow).Cells(4).Value = NoofBls
                dgv_Details.Rows(dgvDet_CurRow).Cells(5).Value = BlNo
                If Val(Tot_Pcs) <> 0 Then
                    dgv_Details.Rows(dgvDet_CurRow).Cells(6).Value = Val(Tot_Pcs)
                End If
                dgv_Details.Rows(dgvDet_CurRow).Cells(7).Value = Format(Val(Tot_Mtrs), "#########0.00")
                dgv_Details.Rows(dgvDet_CurRow).Cells(12).Value = PackSlpCodes
            End If

            Add_NewRow_ToGrid()

            Total_Calculation()

        End With

        pnl_Back.Enabled = True
        pnl_BaleSelection.Visible = False
        If dgv_Details.Enabled And dgv_Details.Visible Then
            If dgv_Details.Rows.Count > 0 Then
                dgv_Details.Focus()
                If dgv_Details.CurrentCell.RowIndex >= 0 Then
                    dgv_Details.CurrentCell = dgv_Details.Rows(dgv_Details.CurrentCell.RowIndex).Cells(6)
                    dgv_Details.CurrentCell.Selected = True
                End If
            End If
        End If

    End Sub

    Private Sub Add_NewRow_ToGrid()
        'On Error Resume Next

        'Dim i As Integer
        'Dim n As Integer = -1

        'With dgv_Details
        '    If .Visible Then

        '        If .CurrentCell.RowIndex = .Rows.Count - 1 Then
        '            If Trim(UCase(cbo_Type.Text)) <> "ORDER" Then
        '                n = .Rows.Add()
        '                'MessageBox.Show("New Added Row = " & n & "  -  Current Row = " & .CurrentCell.RowIndex)

        '                For i = 0 To .Columns.Count - 1
        '                    .Rows(n).Cells(i).Value = .Rows(.CurrentCell.RowIndex).Cells(i).Value
        '                    .Rows(.CurrentCell.RowIndex).Cells(i).Value = ""
        '                Next

        '                For i = 0 To .Rows.Count - 1
        '                    .Rows(i).Cells(0).Value = i + 1
        '                Next

        '                .CurrentCell = .Rows(n).Cells(.CurrentCell.ColumnIndex)
        '                .CurrentCell.Selected = True

        '            End If
        '        End If

        '    End If

        'End With

    End Sub

    Public Sub Printing_Bale()
        Dim prtFrm As Single = 0
        Dim prtTo As Single = 0
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim Condt As String = ""
        Dim PpSzSTS As Boolean = False
        Dim ps As Printing.PaperSize
        Dim NewCode As String

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(EntFnYrCode)

        Try

            da1 = New SqlClient.SqlDataAdapter("select a.*, b.* from Packing_Slip_Head a INNER JOIN Cloth_Head b ON a.Cloth_IdNo = b.Cloth_IdNo Where a.Delivery_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'", con)
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

        If Val(Common_Procedures.Print_OR_Preview_Status) = 1 Then
            Try

                For I = 0 To PrintDocument2.PrinterSettings.PaperSizes.Count - 1
                    If PrintDocument2.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.GermanStandardFanfold Then
                        ps = PrintDocument2.PrinterSettings.PaperSizes(I)
                        PrintDocument2.DefaultPageSettings.PaperSize = ps
                        PpSzSTS = True
                        Exit For
                    End If
                Next

                If PpSzSTS = False Then
                    For I = 0 To PrintDocument2.PrinterSettings.PaperSizes.Count - 1
                        If PrintDocument2.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
                            ps = PrintDocument2.PrinterSettings.PaperSizes(I)
                            PrintDocument2.DefaultPageSettings.PaperSize = ps
                            Exit For
                        End If
                    Next
                End If

                PrintDialog1.PrinterSettings = PrintDocument2.PrinterSettings
                If PrintDialog1.ShowDialog() = Windows.Forms.DialogResult.OK Then
                    PrintDocument2.PrinterSettings = PrintDialog1.PrinterSettings
                    PrintDocument2.Print()
                End If

            Catch ex As Exception
                MessageBox.Show("The printing operation failed" & vbCrLf & ex.Message, "DOES NOT SHOW PRINT PREVIEW...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

            End Try

        Else

            Try

                Dim ppd As New PrintPreviewDialog

                For I = 0 To PrintDocument2.PrinterSettings.PaperSizes.Count - 1
                    If PrintDocument2.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.GermanStandardFanfold Then
                        ps = PrintDocument2.PrinterSettings.PaperSizes(I)
                        PrintDocument2.DefaultPageSettings.PaperSize = ps
                        PpSzSTS = True
                        Exit For
                    End If
                Next

                If PpSzSTS = False Then
                    For I = 0 To PrintDocument2.PrinterSettings.PaperSizes.Count - 1
                        If PrintDocument2.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
                            ps = PrintDocument2.PrinterSettings.PaperSizes(I)
                            PrintDocument2.DefaultPageSettings.PaperSize = ps
                            Exit For
                        End If
                    Next
                End If

                ppd.Document = PrintDocument2

                ppd.WindowState = FormWindowState.Normal
                ppd.StartPosition = FormStartPosition.CenterScreen
                ppd.ClientSize = New Size(600, 600)

                ppd.ShowDialog()
                'If PageSetupDialog1.ShowDialog() = Windows.Forms.DialogResult.OK Then
                '    PrintDocument2.DefaultPageSettings = PageSetupDialog1.PageSettings
                '    ppd.ShowDialog()
                'End If

            Catch ex As Exception
                MsgBox("The printing operation failed" & vbCrLf & ex.Message, MsgBoxStyle.Critical, "DOES NOT SHOW PRINT PREVIEW...")

            End Try

        End If

        pnl_Back.Enabled = True
        pnl_Print.Visible = False

    End Sub

    Private Sub PrintDocument2_BeginPrint(ByVal sender As Object, ByVal e As System.Drawing.Printing.PrintEventArgs) Handles PrintDocument2.BeginPrint
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim NewCode As String = ""
        Dim Total_mtrs As Single = 0
        prn_HdDt.Clear()
        prn_DetDt.Clear()

        prn_PageNo = 0
        prn_HdIndx = 1
        prn_DetIndx = 0
        prn_HdMxIndx = 0
        prn_DetMxIndx = 0
        prn_Count = 1
        Erase prn_DetAr
        Erase prn_HdAr

        prn_HdAr = New String(100, 10) {}

        prn_DetAr = New String(100, 50, 10) {}

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(EntFnYrCode)

        Try

            da1 = New SqlClient.SqlDataAdapter("select a.Packing_Slip_No,a.cloth_idno,a.Total_Pcs as Pak_Pcs,a.Packing_Slip_Code , a.Total_Weight ,a.Total_Meters as Pak_Mtrs, tZ.*, c.*,d.*,E.*  from Packing_Slip_Head a INNER JOIN Company_head tZ ON a.company_idno = tZ.Company_Idno INNER JOIN Cloth_Head c ON a.Cloth_IdNo = c.Cloth_IdNo INNER JOIN Ledger_Head d ON a.Ledger_IdNo = d.Ledger_IdNo  INNER JOIN Bale_Transfer_Head e ON e.Bale_Transfer_Code =  '" & Trim(NewCode) & "'  Where a.Delivery_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' Order by a.Packing_Slip_Date, a.for_OrderBy, a.Packing_Slip_No, a.Packing_Slip_Code", con)
            prn_HdDt = New DataTable
            da1.Fill(prn_HdDt)

            If prn_HdDt.Rows.Count > 0 Then
                For i = 0 To prn_HdDt.Rows.Count - 1

                    prn_HdMxIndx = prn_HdMxIndx + 1

                    prn_HdAr(prn_HdMxIndx, 1) = Trim(prn_HdDt.Rows(i).Item("Packing_Slip_No").ToString)
                    prn_HdAr(prn_HdMxIndx, 2) = Trim(prn_HdDt.Rows(i).Item("Cloth_Name").ToString)
                    prn_HdAr(prn_HdMxIndx, 3) = Val(prn_HdDt.Rows(i).Item("Pak_Pcs").ToString)
                    prn_HdAr(prn_HdMxIndx, 4) = Format(Val(prn_HdDt.Rows(i).Item("Pak_Mtrs").ToString), "#########0.00")

                    prn_DetMxIndx = 0

                    da2 = New SqlClient.SqlDataAdapter("select a.* from Packing_Slip_Details a where a.Packing_Slip_Code = '" & Trim(prn_HdDt.Rows(i).Item("Packing_Slip_Code").ToString) & "' order by a.Sl_No", con)
                    prn_DetDt = New DataTable
                    da2.Fill(prn_DetDt)

                    'If prn_DetDt.Rows.Count > 0 Then
                    '    For j = 0 To prn_DetDt.Rows.Count - 1
                    '        If Val(prn_DetDt.Rows(j).Item("Meters").ToString) <> 0 Then
                    '            prn_DetMxIndx = prn_DetMxIndx + 1
                    '            prn_DetAr(prn_HdMxIndx, prn_DetMxIndx, 5) = Trim(prn_HdDt.Rows(i).Item("Packing_Slip_No").ToString)
                    '            prn_DetAr(prn_HdMxIndx, prn_DetMxIndx, 0) = Trim(prn_DetDt.Rows(j).Item("Sl_No").ToString)
                    '            prn_DetAr(prn_HdMxIndx, prn_DetMxIndx, 1) = Trim(prn_DetDt.Rows(j).Item("Lot_No").ToString)
                    '            prn_DetAr(prn_HdMxIndx, prn_DetMxIndx, 2) = Trim(prn_DetDt.Rows(j).Item("Pcs_No").ToString)
                    '            prn_DetAr(prn_HdMxIndx, prn_DetMxIndx, 3) = Format(Val(prn_DetDt.Rows(j).Item("Meters").ToString), "#########0.00")

                    '            '  Total_mtrs = Total_mtrs + Format(Val(prn_DetDt.Rows(j).Item("Meters").ToString), "#########0.00")
                    '            prn_DetAr(prn_HdMxIndx, prn_DetMxIndx, 4) = Format(Val(prn_DetDt.Rows(j).Item("Weight").ToString), "#########0.00")
                    '        End If
                    '    Next j
                    'End If

                    If prn_DetDt.Rows.Count > 0 Then
                        For j = 0 To prn_DetDt.Rows.Count - 1
                            If Val(prn_DetDt.Rows(j).Item("Meters").ToString) <> 0 Then
                                prn_DetMxIndx = prn_DetMxIndx + 1
                                prn_DetAr(prn_HdMxIndx, prn_DetMxIndx, 5) = Trim(prn_HdDt.Rows(i).Item("Packing_Slip_No").ToString)
                                prn_DetAr(prn_HdMxIndx, prn_DetMxIndx, 0) = Trim(prn_DetDt.Rows(j).Item("Sl_No").ToString)
                                prn_DetAr(prn_HdMxIndx, prn_DetMxIndx, 1) = Trim(prn_DetDt.Rows(j).Item("Lot_No").ToString)
                                prn_DetAr(prn_HdMxIndx, prn_DetMxIndx, 2) = Trim(prn_DetDt.Rows(j).Item("Pcs_No").ToString)
                                prn_DetAr(prn_HdMxIndx, prn_DetMxIndx, 3) = Format(Val(prn_DetDt.Rows(j).Item("Meters").ToString), "#########0.00")

                                Total_mtrs = Total_mtrs + Format(Val(prn_DetDt.Rows(j).Item("Meters").ToString), "#########0.00")
                                prn_DetAr(prn_HdMxIndx, prn_DetMxIndx, 4) = Format(Val(prn_DetDt.Rows(j).Item("Weight").ToString), "#########0.00")
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

    Private Sub PrintDocument2_PrintPage(ByVal sender As System.Object, ByVal e As System.Drawing.Printing.PrintPageEventArgs) Handles PrintDocument2.PrintPage
        If prn_HdDt.Rows.Count <= 0 Then Exit Sub
        Common_Procedures.Printing_PackingSlip_Format1(PrintDocument2, e, prn_HdDt, prn_HdMxIndx, prn_DetMxIndx, prn_HdAr, prn_DetAr, prn_PageNo, prn_Count, prn_HdIndx, prn_DetIndx)
    End Sub

    Private Sub msk_Date_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles msk_date.KeyUp
        If Trim(UCase(e.KeyCode)) = "D" And vcbo_KeyDwnVal = e.KeyValue Then
            msk_date.Text = Date.Today
        End If
        If IsDate(msk_date.Text) = True Then
            If e.KeyCode = 107 Then
                msk_date.Text = DateAdd("D", 1, Convert.ToDateTime(msk_date.Text))
            ElseIf e.KeyCode = 109 Then
                msk_date.Text = DateAdd("D", -1, Convert.ToDateTime(msk_date.Text))
            End If
        End If
        If e.KeyCode = 46 Or e.KeyCode = 8 Then

            Common_Procedures.maskEdit_Date_ON_DelBackSpace(sender, e, vmskOldText, vmskSelStrt)
        End If
    End Sub

    Private Sub msk_Date_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles msk_date.LostFocus

        If IsDate(msk_Date.Text) = True Then
            If Microsoft.VisualBasic.DateAndTime.Day(Convert.ToDateTime(msk_Date.Text)) <= 31 Or Microsoft.VisualBasic.DateAndTime.Month(Convert.ToDateTime(msk_Date.Text)) <= 31 Then
                If Microsoft.VisualBasic.DateAndTime.Year(Convert.ToDateTime(msk_Date.Text)) <= 2050 And Microsoft.VisualBasic.DateAndTime.Year(Convert.ToDateTime(msk_Date.Text)) >= 2000 Then
                    dtp_Date.Value = Convert.ToDateTime(msk_Date.Text)
                End If
            End If

        End If
    End Sub

    Private Sub dtp_Date_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dtp_Date.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            dtp_Date.Text = Date.Today
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
            cbo_TransferGodownTo.Focus()
        End If

        If e.KeyCode = 38 Then
            e.Handled = True : e.SuppressKeyPress = True
            txt_Note.Focus()
        End If


        vmskOldText = ""
        vmskSelStrt = -1
        If e.KeyCode = 46 Or e.KeyCode = 8 Then
            vmskOldText = msk_date.Text
            vmskSelStrt = msk_date.SelectionStart
        End If

    End Sub

    Private Sub btn_Print_FormJJ_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Print_FormJJ.Click
        Print_FormJJ()
        btn_print_Close_Click(sender, e)
    End Sub

    Private Sub Print_FormJJ()
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim NewCode As String
        Dim ps As Printing.PaperSize
        Dim OrdBy_FrmNo As Single = 0, OrdByToNo As Single = 0
        Dim PpSzSTS As Boolean = False

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(EntFnYrCode)

        Try

            da1 = New SqlClient.SqlDataAdapter("select * from Bale_Transfer_Head Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Bale_Transfer_Code = '" & Trim(NewCode) & "'", con)
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

        prn_InpOpts = InputBox("Select    -   0. None" & Chr(13) & "                  1. Original" & Chr(13) & "                  2. Duplicate" & Chr(13) & "                  3. Triplicate" & Chr(13) & "                  4. All", "FOR FORMJJ PRINTING...", "4")
        prn_InpOpts = Replace(Trim(prn_InpOpts), "4", "123")

        For I = 0 To PrintDocument3.PrinterSettings.PaperSizes.Count - 1
            If PrintDocument3.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
                ps = PrintDocument3.PrinterSettings.PaperSizes(I)
                PrintDocument3.DefaultPageSettings.PaperSize = ps
                Exit For
            End If
        Next


        If Val(Common_Procedures.Print_OR_Preview_Status) = 1 Then
            Try
                If Val(Common_Procedures.settings.Printing_Show_PrintDialogue) = 1 Then
                    PrintDialog1.PrinterSettings = PrintDocument3.PrinterSettings
                    If PrintDialog1.ShowDialog() = Windows.Forms.DialogResult.OK Then
                        PrintDocument3.PrinterSettings = PrintDialog1.PrinterSettings
                        PrintDocument3.Print()
                    End If

                Else
                    PrintDocument3.Print()

                End If

            Catch ex As Exception
                MessageBox.Show("The printing operation failed" & vbCrLf & ex.Message, "DOES NOT SHOW PRINT PREVIEW...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

            End Try


        Else
            Try

                Dim ppd As New PrintPreviewDialog

                ppd.Document = PrintDocument3


                ppd.WindowState = FormWindowState.Normal
                ppd.StartPosition = FormStartPosition.CenterScreen
                ppd.ClientSize = New Size(600, 600)

                ppd.ShowDialog()
                'If PageSetupDialog1.ShowDialog() = Windows.Forms.DialogResult.OK Then
                '    PrintDocument2.DefaultPageSettings = PageSetupDialog1.PageSettings
                '    ppd.ShowDialog()
                'End If

            Catch ex As Exception
                MsgBox("The printing operation failed" & vbCrLf & ex.Message, MsgBoxStyle.Critical, "DOES NOT SHOW PRINT PREVIEW...")

            End Try

        End If
    End Sub

    Private Sub PrintDocument3_BeginPrint(ByVal sender As Object, ByVal e As System.Drawing.Printing.PrintEventArgs) Handles PrintDocument3.BeginPrint
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim cmd As New SqlClient.SqlCommand
        Dim NewCode As String
        Dim i As Integer, k As Integer
        Dim W1 As Single = 0

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(EntFnYrCode)

        prn_HdDt.Clear()
        prn_DetDt.Clear()
        prn_DetIndx = 0
        prn_DetSNo = 0
        prn_PageNo = 0
        prn_DetMxIndx = 0
        prn_NoofBmDets = 0
        prn_Count = 0

        Erase prn_HdAr
        Erase prn_DetAr

        prn_HdAr = New String(200, 10) {}
        prn_DetAr1 = New String(200, 10) {}

        Try

            da1 = New SqlClient.SqlDataAdapter("select a.*, b.*, c.Ledger_TinNo , c.Ledger_CstNo, c.Ledger_Name , c.Ledger_Address1, c.Ledger_Address2, c.Ledger_Address3, c.Ledger_Address4, d.Ledger_Name as Transport_Name, e.Area_Name from Bale_Transfer_Head a INNER JOIN Company_Head b ON a.Company_IdNo = b.Company_IdNo INNER JOIN Ledger_Head c ON a.Ledger_Idno = c.Ledger_IdNo LEFT OUTER JOIN Ledger_Head d ON a.Transport_Idno = d.Ledger_IdNo  LEFT OUTER JOIN Area_Head e ON b.Area_Idno = e.Area_Idno   where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Bale_Transfer_Code = '" & Trim(NewCode) & "'", con)
            prn_HdDt = New DataTable
            da1.Fill(prn_HdDt)

            If prn_HdDt.Rows.Count > 0 Then

                da2 = New SqlClient.SqlDataAdapter("select a.*, b.ClothType_Name, c.Cloth_Name from Bale_Transfer_Details a LEFT OUTER JOIN ClothType_Head b on a.ClothType_IdNo = b.ClothType_IdNo INNER JOIN Cloth_Head c on a.Cloth_IdNo = c.Cloth_IdNo where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Bale_Transfer_Code = '" & Trim(NewCode) & "' Order by a.sl_no", con)
                prn_DetDt = New DataTable
                da2.Fill(prn_DetDt)

                da1 = New SqlClient.SqlDataAdapter("select top 1  c.Cloth_Name, d.ClothType_Name, b.Total_Bales, b.Total_Pcs, b.Total_Meters from Bale_Transfer_Details a INNER JOIN Bale_Transfer_Head b ON a.Bale_Transfer_Code = b.Bale_Transfer_Code INNER JOIN Cloth_Head c on a.Cloth_IdNo = c.Cloth_IdNo LEFT OUTER JOIN ClothType_Head d on a.ClothType_IdNo = d.ClothType_IdNo where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Bale_Transfer_Code = '" & Trim(NewCode) & "' Order by a.sl_no", con)
                prn_DetDt1 = New DataTable
                da1.Fill(prn_DetDt1)

                k = 0
                If prn_DetDt1.Rows.Count > 0 Then

                    For i = 0 To prn_DetDt1.Rows.Count - 1

                        If Val(prn_DetDt1.Rows(i).Item("Total_Meters").ToString) <> 0 Then
                            k = k + 1
                            prn_DetAr1(k + 100, 1) = Trim(prn_DetDt1.Rows(i).Item("Cloth_Name").ToString)
                            k = k + 1
                            prn_DetAr1(k + 100, 1) = Trim(prn_DetDt1.Rows(i).Item("ClothType_Name").ToString)
                            k = k + 1
                            prn_DetAr1(k + 100, 1) = Val(prn_DetDt1.Rows(i).Item("Total_Bales").ToString)
                            k = k + 1
                            prn_DetAr1(k + 100, 1) = Val(prn_DetDt1.Rows(i).Item("Total_Pcs").ToString)
                            k = k + 1
                            prn_DetAr1(k + 100, 1) = Format(Val(prn_DetDt1.Rows(i).Item("Total_Meters").ToString), "#########0.000")

                        End If

                    Next i

                End If
                Dt1.Clear()

                If k > prn_DetMxIndx Then prn_DetMxIndx = k

            Else
                MessageBox.Show("This is New Entry", "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

            End If

            da1.Dispose()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub PrintDocument3_PrintPage(ByVal sender As System.Object, ByVal e As System.Drawing.Printing.PrintPageEventArgs) Handles PrintDocument3.PrintPage
        If prn_HdDt.Rows.Count <= 0 Then Exit Sub
        Printing_FormJJ(e)
    End Sub

    Private Sub Printing_FormJJ(ByRef e As System.Drawing.Printing.PrintPageEventArgs)
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
        Dim ps As Printing.PaperSize
        Dim strHeight As Single = 0
        Dim PpSzSTS As Boolean = False
        Dim W1 As Single = 0
        Dim SNo As Integer = 0
        Dim flperc As Single = 0
        Dim flmtr As Single = 0
        Dim fmtr As Single = 0
        Dim LnAr(15) As Single, ClArr(15) As Single
        Dim ItmNm1 As String, ItmNm2 As String


        For I = 0 To PrintDocument3.PrinterSettings.PaperSizes.Count - 1
            If PrintDocument3.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
                ps = PrintDocument3.PrinterSettings.PaperSizes(I)
                PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = ps
                PrintDocument1.DefaultPageSettings.PaperSize = ps
                e.PageSettings.PaperSize = ps
                Exit For
            End If
        Next

        With PrintDocument2.DefaultPageSettings.Margins
            .Left = 20
            .Right = 65
            .Top = 50 ' 60
            .Bottom = 40
            LMargin = .Left
            RMargin = .Right
            TMargin = .Top
            BMargin = .Bottom
        End With


        pFont = New Font("Calibri", 10, FontStyle.Regular)

        e.Graphics.TextRenderingHint = Drawing.Text.TextRenderingHint.AntiAlias

        With PrintDocument3.DefaultPageSettings.PaperSize
            PrintWidth = .Width - RMargin - LMargin
            PrintHeight = .Height - TMargin - BMargin
            PageWidth = .Width - RMargin
            PageHeight = .Height - BMargin
        End With
        If PrintDocument3.DefaultPageSettings.Landscape = True Then
            With PrintDocument3.DefaultPageSettings.PaperSize
                PrintWidth = .Height - TMargin - BMargin
                PrintHeight = .Width - RMargin - LMargin
                PageWidth = .Height - TMargin
                PageHeight = .Width - RMargin
            End With
        End If

        NoofItems_PerPage = 5 ' 6

        Erase LnAr
        Erase ClArr

        LnAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        ClArr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}

        ClArr(1) = Val(35) : ClArr(2) = 300 : ClArr(3) = 100 : ClArr(4) = 95 : ClArr(5) = 110 : ClArr(6) = 80
        ClArr(8) = PageWidth - (LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7))

        TxtHgt = 19  ' e.Graphics.MeasureString("A", pFont).Height  ' 20

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

        EntryCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(EntFnYrCode)

        Try
            If prn_HdDt.Rows.Count > 0 Then

                Printing_FormJJ_PageHeader(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, prn_PageNo, NoofItems_PerPage, CurY, LnAr, ClArr)

                NoofDets = 0

                CurY = CurY - 10
                W1 = e.Graphics.MeasureString("No.of Beams  : ", pFont).Width

                NoofDets = 0

                CurY = CurY - 10

                If prn_DetDt.Rows.Count > 0 Then

                    Do While prn_DetIndx <= prn_DetDt.Rows.Count - 1

                        If NoofDets >= NoofItems_PerPage Then
                            CurY = CurY + TxtHgt

                            Common_Procedures.Print_To_PrintDocument(e, "Continued....", PageWidth - 10, CurY, 1, 0, pFont)

                            NoofDets = NoofDets + 1

                            Printing_FormJJ_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, NoofDets, False)

                            e.HasMorePages = True
                            Return

                        End If

                        prn_DetSNo = prn_DetSNo + 1

                        ItmNm1 = Trim(prn_DetDt.Rows(prn_DetIndx).Item("Cloth_Name").ToString)
                        ItmNm2 = ""
                        If Len(ItmNm1) > 30 Then
                            For I = 15 To 1 Step -1
                                If Mid$(Trim(ItmNm1), I, 1) = " " Or Mid$(Trim(ItmNm1), I, 1) = "," Or Mid$(Trim(ItmNm1), I, 1) = "." Or Mid$(Trim(ItmNm1), I, 1) = "-" Or Mid$(Trim(ItmNm1), I, 1) = "/" Or Mid$(Trim(ItmNm1), I, 1) = "_" Or Mid$(Trim(ItmNm1), I, 1) = "(" Or Mid$(Trim(ItmNm1), I, 1) = ")" Or Mid$(Trim(ItmNm1), I, 1) = "\" Or Mid$(Trim(ItmNm1), I, 1) = "[" Or Mid$(Trim(ItmNm1), I, 1) = "]" Or Mid$(Trim(ItmNm1), I, 1) = "{" Or Mid$(Trim(ItmNm1), I, 1) = "}" Then Exit For
                            Next I
                            If I = 0 Then I = 30
                            ItmNm2 = Microsoft.VisualBasic.Right(Trim(ItmNm1), Len(ItmNm1) - I)
                            ItmNm1 = Microsoft.VisualBasic.Left(Trim(ItmNm1), I - 1)
                        End If

                        CurY = CurY + TxtHgt + 5

                        Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetDt.Rows(prn_DetIndx).Item("Sl_No").ToString), LMargin + 15, CurY, 0, 0, pFont)

                        Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm1), LMargin + ClArr(1) + 10, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("ClothType_Name").ToString, LMargin + ClArr(1) + ClArr(2) + 10, CurY, 0, 0, pFont)
                        If Val(prn_DetDt.Rows(prn_DetIndx).Item("Bales").ToString) <> 0 Then
                            Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(prn_DetIndx).Item("Bales").ToString), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) - 10, CurY, 1, 0, pFont)
                        End If
                        If Val(prn_DetDt.Rows(prn_DetIndx).Item("Pcs").ToString) <> 0 Then
                            Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(prn_DetIndx).Item("Pcs").ToString), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) - 10, CurY, 1, 0, pFont)
                        End If
                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Meters").ToString), "########0.00"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) - 10, CurY, 1, 0, pFont)



                        NoofDets = NoofDets + 1

                        If Trim(ItmNm2) <> "" Then
                            CurY = CurY + TxtHgt - 5
                            Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm2), LMargin + ClArr(1) + 10, CurY, 0, 0, pFont)
                            NoofDets = NoofDets + 1
                        End If

                        prn_DetIndx = prn_DetIndx + 1

                    Loop

                End If

                Printing_FormJJ_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, NoofDets, True)

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


        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

        e.HasMorePages = False

    End Sub

    Private Sub Printing_FormJJ_PageHeader(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByRef PageNo As Integer, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClArr() As Single)
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt2 As New DataTable
        Dim da3 As New SqlClient.SqlDataAdapter
        Dim dt3 As New DataTable
        Dim p1Font As Font
        Dim strHeight As Double = 0
        Dim C1 As Single, W1, W2 As Single, S1, S2 As Single
        Dim Cmp_Name, Desc As String, Cmp_Add1 As String, Cmp_Add2, Cmp_Add4 As String
        Dim Cmp_PhNo As String, Cmp_TinNo As String, Cmp_CstNo As String
        Dim S As String = ""
        Dim goods_value As Double = 0
        Dim Fab_Value As Double = 0
        Dim pavu_value As Double = 0
        Dim NewCode As String = ""
        Dim To_Add As String = ""

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(EntFnYrCode)

        PageNo = PageNo + 1

        CurY = TMargin

        da2 = New SqlClient.SqlDataAdapter("select a.*, b.* from Bale_Transfer_Details a INNER JOIN Cloth_Head b ON a.Cloth_IdNo = b.Cloth_IdNo  where a.Bale_Transfer_Code = '" & Trim(NewCode) & "'", con)
        dt2 = New DataTable
        da2.Fill(dt2)

        Fab_Value = 0
        If dt2.Rows.Count > 0 Then
            For i = 0 To dt2.Rows.Count - 1
                If Val(dt2.Rows(prn_DetIndx).Item("ClothType_IdNo").ToString) = 1 Then
                    Fab_Value = Fab_Value + Format(Val(dt2.Rows(0).Item("Meters").ToString) * Val(dt2.Rows(0).Item("Sound_Rate").ToString), "#######0.00")
                ElseIf Val(dt2.Rows(prn_DetIndx).Item("ClothType_IdNo").ToString) = 2 Then
                    Fab_Value = Fab_Value + Format(Val(dt2.Rows(0).Item("Meters").ToString) * Val(dt2.Rows(0).Item("Seconds_Rate").ToString), "#######0.00")
                ElseIf Val(dt2.Rows(prn_DetIndx).Item("ClothType_IdNo").ToString) = 3 Then
                    Fab_Value = Fab_Value + Format(Val(dt2.Rows(0).Item("Meters").ToString) * Val(dt2.Rows(0).Item("Bits_Rate").ToString), "#######0.00")
                ElseIf Val(dt2.Rows(prn_DetIndx).Item("ClothType_IdNo").ToString) = 4 Then
                    Fab_Value = Fab_Value + Format(Val(dt2.Rows(0).Item("Meters").ToString) * Val(dt2.Rows(0).Item("Other_Rate").ToString), "#######0.00")
                ElseIf Val(dt2.Rows(prn_DetIndx).Item("ClothType_IdNo").ToString) = 5 Then
                    Fab_Value = Fab_Value + Format(Val(dt2.Rows(0).Item("Meters").ToString) * Val(dt2.Rows(0).Item("Reject_Rate").ToString), "#######0.00")
                End If
            Next i
        End If
        dt2.Clear()

        'da2 = New SqlClient.SqlDataAdapter("select Sum(a.Meters * b.Sound_Rate) as Value_Of_Sound ,sum(a.Meters * b.Seconds_Rate) as Value_Of_Sound, Sum(a.Meters * b.Bits_Rate) as Value_Of_Bits,Sum(a.Meters * b.Other_Rate) as Value_Of_Other,Sum(a.Meters * b.Reject_Rate) as Value_Of_Reject from Bale_Transfer_Details a INNER JOIN Cloth_Head b ON a.Cloth_IdNo = b.Cloth_IdNo  where a.Bale_Transfer_Code = '" & Trim(NewCode) & "'", con)
        'dt2 = New DataTable
        'da2.Fill(dt2)

        'Fab_Value = 0
        'If dt2.Rows.Count > 0 Then
        '    If Val(prn_DetDt.Rows(prn_DetIndx).Item("ClothType_IdNo").ToString) <> 0 Then
        '        Fab_Value = Format(Val(dt2.Rows(0).Item("Value_Of_Sound").ToString), "#######0.00")
        '    ElseIf Fab_Value = Format(Val(dt2.Rows(0).Item("Value_Of_Second").ToString), "#######0.00") Then
        '    ElseIf Fab_Value = Format(Val(dt2.Rows(0).Item("Value_Of_Bits").ToString), "#######0.00") Then
        '    ElseIf Fab_Value = Format(Val(dt2.Rows(0).Item("Value_Of_Others").ToString), "#######0.00") Then
        '    ElseIf Fab_Value = Format(Val(dt2.Rows(0).Item("Value_Of_Rejects").ToString), "#######0.00") Then
        '    End If
        'End If
        'dt2.Clear()

        goods_value = Format(Val(Fab_Value), "#########0.00")

        dt2.Clear()
        dt3.Clear()

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
                Else
                    If Trim(prn_InpOpts) <> "0" And Val(prn_InpOpts) = "0" And Len(Trim(prn_InpOpts)) > 2 Then
                        prn_OriDupTri = Trim(prn_InpOpts)
                    End If
                End If
            End If
        End If

        p1Font = New Font("Calibri", 20, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "FORM JJ", LMargin + 10, CurY - TxtHgt - 10, 0, 0, p1Font)

        If Trim(prn_OriDupTri) <> "" Then
            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_OriDupTri), PageWidth - 10, CurY - TxtHgt, 1, 0, pFont)
        End If

        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(1) = CurY

        Desc = ""
        Cmp_Name = "" : Cmp_Add1 = "" : Cmp_Add2 = ""
        Cmp_PhNo = "" : Cmp_TinNo = "" : Cmp_CstNo = ""

        Desc = prn_HdDt.Rows(0).Item("Company_Description").ToString
        Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString
        Cmp_Add1 = prn_HdDt.Rows(0).Item("Company_Address1").ToString & " " & prn_HdDt.Rows(0).Item("Company_Address2").ToString
        Cmp_Add2 = prn_HdDt.Rows(0).Item("Company_Address3").ToString & " " & prn_HdDt.Rows(0).Item("Company_Address4").ToString
        Cmp_Add4 = prn_HdDt.Rows(0).Item("Company_Address4").ToString

        If Trim(prn_HdDt.Rows(0).Item("Company_TinNo").ToString) <> "" Then
            Cmp_TinNo = "TIN NO.: " & prn_HdDt.Rows(0).Item("Company_TinNo").ToString
        End If
        If Trim(prn_HdDt.Rows(0).Item("Company_CstNo").ToString) <> "" Then
            Cmp_CstNo = "CST NO.: " & prn_HdDt.Rows(0).Item("Company_CstNo").ToString
        End If

        p1Font = New Font("Calibri", 15, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "DELIVERY NOTE", LMargin, CurY, 2, PrintWidth, p1Font)
        CurY = CurY + TxtHgt + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(2) = CurY

        CurY = CurY + TxtHgt - 10
        Common_Procedures.Print_To_PrintDocument(e, "[See rule 15(3), 15(18), 15(19), 15(20), 15(21)]", LMargin, CurY, 2, PrintWidth, pFont)
        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, "(for sales / stock transfer / works contract / labour)", LMargin, CurY, 2, PrintWidth, pFont)
        CurY = CurY + TxtHgt

        Common_Procedures.Print_To_PrintDocument(e, "Asst Year : " & Trim(EntFnYrCode), LMargin, CurY, 2, PrintWidth, pFont)
        'Common_Procedures.Print_To_PrintDocument(e, "Asst Year : 15-16", LMargin, CurY, 2, PrintWidth, pFont)
        Common_Procedures.Print_To_PrintDocument(e, Cmp_TinNo, LMargin + 10, CurY, 0, 0, pFont)
        If prn_HdDt.Rows(0).Item("JJ_FormNo").ToString <> "" Then
            Common_Procedures.Print_To_PrintDocument(e, "NO  :  " & prn_HdDt.Rows(0).Item("JJ_FormNo").ToString, PageWidth - 10, CurY, 1, 0, pFont)
        Else
            Common_Procedures.Print_To_PrintDocument(e, "NO  :  " & prn_HdDt.Rows(0).Item("Bale_Transfer_No").ToString, PageWidth - 10, CurY, 1, 0, pFont)
        End If

        CurY = CurY + TxtHgt
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(3) = CurY

        Try
            C1 = ClArr(1) + ClArr(2) + ClArr(3)

            W1 = e.Graphics.MeasureString("INVOICE DATE  : ", pFont).Width
            S1 = e.Graphics.MeasureString("TO     :    ", pFont).Width
            W2 = e.Graphics.MeasureString("Despatch To   : ", pFont).Width
            S2 = e.Graphics.MeasureString("Sent Through  : ", pFont).Width

            CurY = CurY + TxtHgt - 10
            Common_Procedures.Print_To_PrintDocument(e, "1.(a) Name and address of the", LMargin + 10, CurY, 0, 0, pFont)
            p1Font = New Font("Calibri", 12, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, Cmp_Name, LMargin + C1 + 10, CurY, 0, 0, p1Font)
            strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "Consigner", LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Cmp_Add1, LMargin + C1 + 10, CurY, 0, 0, p1Font)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, Cmp_Add2, LMargin + C1 + 10, CurY, 0, 0, p1Font)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "(b).TIN", LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Cmp_TinNo, LMargin + C1 + 10, CurY, 0, 0, p1Font)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "(c).CST Registration No", LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Cmp_CstNo, LMargin + C1 + 10, CurY, 0, 0, p1Font)

            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(4) = CurY

            CurY = CurY + TxtHgt - 10
            Common_Procedures.Print_To_PrintDocument(e, "2.(a) Name and address of the", LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Ledger_Name").ToString, LMargin + C1 + 10, CurY, 0, 0, p1Font)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "consignee / branch / agent", LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Ledger_Address1").ToString, LMargin + C1 + 10, CurY, 0, 0, p1Font)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Ledger_Address2").ToString, LMargin + C1 + 10, CurY, 0, 0, p1Font)
            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Ledger_Address4").ToString, LMargin + C1 + 10, CurY, 0, 0, p1Font)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "(b).TIN", LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Ledger_TinNo").ToString, LMargin + C1 + 10, CurY, 0, 0, p1Font)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "(c).CST Registration No.", LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Ledger_CstNo").ToString, LMargin + C1 + 10, CurY, 0, 0, p1Font)

            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(5) = CurY

            CurY = CurY + TxtHgt - 10
            Common_Procedures.Print_To_PrintDocument(e, "3 Address", LMargin + 10, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "(i) from which goods are consigned.", LMargin + 10, CurY, 0, 0, pFont)

            If Trim(prn_HdDt.Rows(0).Item("Area_Name").ToString) <> "" Then
                Cmp_Add4 = Trim(prn_HdDt.Rows(0).Item("Area_Name").ToString)

            ElseIf Trim(prn_HdDt.Rows(0).Item("Company_Address4").ToString) <> "" Then
                Cmp_Add4 = Trim(prn_HdDt.Rows(0).Item("Company_Address4").ToString)

            ElseIf Trim(prn_HdDt.Rows(0).Item("Company_Address3").ToString) <> "" Then
                Cmp_Add4 = Trim(prn_HdDt.Rows(0).Item("Company_Address3").ToString)

            ElseIf Trim(prn_HdDt.Rows(0).Item("Company_Address2").ToString) <> "" Then
                Cmp_Add4 = Trim(prn_HdDt.Rows(0).Item("Company_Address2").ToString)

            End If

            Common_Procedures.Print_To_PrintDocument(e, Cmp_Add4, LMargin + C1 + 10, CurY, 0, 0, p1Font)

            If Trim(prn_HdDt.Rows(0).Item("Ledger_Address4").ToString) <> "" Then
                To_Add = Trim(prn_HdDt.Rows(0).Item("Ledger_Address4").ToString)

            ElseIf Trim(prn_HdDt.Rows(0).Item("Ledger_Address3").ToString) <> "" Then
                To_Add = Trim(prn_HdDt.Rows(0).Item("Ledger_Address3").ToString)

            ElseIf Trim(prn_HdDt.Rows(0).Item("Ledger_Address2").ToString) <> "" Then
                To_Add = Trim(prn_HdDt.Rows(0).Item("Ledger_Address2").ToString)

            End If

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "(ii) to which goods are consigned.", LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, To_Add, LMargin + C1 + 10, CurY, 0, 0, p1Font)

            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(6) = CurY

            CurY = CurY + TxtHgt - 10
            Common_Procedures.Print_To_PrintDocument(e, "4.Description of goods consigned", LMargin + 10, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "(a) Name of the goods", LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "Fabric", LMargin + C1 + 10, CurY, 0, 0, p1Font)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "(b) Quantity Or Weight", LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "" & prn_HdDt.Rows(0).Item("Total_Meters").ToString & " Mtrs", LMargin + C1 + 10, CurY, 0, 0, p1Font)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "(c) Value of the goods", LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "Rs. " & Common_Procedures.Currency_Format(Val(goods_value)), LMargin + C1 + 10, CurY, 0, 0, p1Font)

            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(7) = CurY

            CurY = CurY + TxtHgt - 10
            Common_Procedures.Print_To_PrintDocument(e, "5.Purpose of transport", LMargin + 10, CurY, 0, 0, pFont)
            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "(a) for sale / purchase", LMargin + 10, CurY, 0, 0, pFont)
            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "(b) for shipment", LMargin + 10, CurY, 0, 0, pFont)
            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "(c) transfer to branch/head office", LMargin + 10, CurY, 0, 0, pFont)
            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "/Consignment agent", LMargin + 10, CurY, 0, 0, pFont)
            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "(d) for executionof works contract", LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "(e) FOR LABOUR WORK / PROCESSING", LMargin + C1 + 10, CurY, 0, 0, p1Font)
            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "(e) for labour work / processing", LMargin + 10, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(8) = CurY

            CurY = CurY + TxtHgt - 10
            Common_Procedures.Print_To_PrintDocument(e, "6.To Whom delivered for transport", LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Transport_Name").ToString, LMargin + C1 + 10, CurY, 0, 0, p1Font)

            'CurY = CurY + TxtHgt
            'Common_Procedures.Print_To_PrintDocument(e, "and vehicle no, if any", LMargin + 10, CurY, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Vechile_No").ToString, LMargin + C1 + 10, CurY, 0, 0, p1Font)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "7.Remarks, if any", LMargin + 10, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(9) = CurY

            CurY = CurY + TxtHgt - 10
            Common_Procedures.Print_To_PrintDocument(e, "SNO", LMargin, CurY, 2, ClArr(1), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "CLOTHNAME", LMargin + ClArr(1), CurY, 2, ClArr(2), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "TYPE", LMargin + ClArr(1) + ClArr(2), CurY, 2, ClArr(3), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "BALES", LMargin + ClArr(1) + ClArr(2) + ClArr(3), CurY, 2, ClArr(4), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "PCS", LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4), CurY, 2, ClArr(5), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "METERS", LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5), CurY, 2, ClArr(6), pFont)

            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(10) = CurY


        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub Printing_FormJJ_PageFooter(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClArr() As Single, ByVal NoofDets As Integer, ByVal is_LastPage As Boolean)
        Dim p1Font As Font
        Dim I As Integer
        Dim W1 As Single = 0
        Dim vprn_BlNos As String = ""
        Dim BankNm1 As String = ""
        Dim BankNm2 As String = ""
        Dim BankNm3 As String = ""
        Dim BankNm4 As String = ""
        Dim Cmp_Name As String

        Try
            W1 = e.Graphics.MeasureString("No.of Beams  : ", pFont).Width

            For I = NoofDets + 1 To NoofItems_PerPage

                CurY = CurY + TxtHgt
            Next

            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(5) = CurY

            CurY = CurY + TxtHgt - 10
            If is_LastPage = True Then
                Common_Procedures.Print_To_PrintDocument(e, " TOTAL", LMargin + ClArr(1) + ClArr(2) + 30, CurY, 2, ClArr(4), pFont)
            End If

            If Val(prn_HdDt.Rows(0).Item("Total_Bales").ToString) <> 0 Then
                If is_LastPage = True Then
                    Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(0).Item("Total_Bales").ToString), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) - 10, CurY, 1, 0, pFont)
                End If
            End If
            If Val(prn_HdDt.Rows(0).Item("Total_Pcs").ToString) <> 0 Then
                If is_LastPage = True Then
                    Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(0).Item("Total_Pcs").ToString), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) - 10, CurY, 1, 0, pFont)
                End If
            End If
            If Val(prn_HdDt.Rows(0).Item("Total_Meters").ToString) <> 0 Then
                If is_LastPage = True Then
                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Total_Meters").ToString), "#########0.00"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) - 10, CurY, 1, 0, pFont)
                End If
            End If

            CurY = CurY + TxtHgt - 15

            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(6) = CurY

            e.Graphics.DrawLine(Pens.Black, LMargin + ClArr(1), CurY, LMargin + ClArr(1), LnAr(9))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClArr(1) + ClArr(2) + ClArr(3), CurY, LMargin + ClArr(1) + ClArr(2) + ClArr(3), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClArr(1) + ClArr(2), CurY, LMargin + ClArr(1) + ClArr(2), LnAr(9))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4), CurY, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4), LnAr(9))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5), CurY, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5), LnAr(9))
            'e.Graphics.DrawLine(Pens.Black, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6), CurY, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6), LnAr(9))
            'e.Graphics.DrawLine(Pens.Black, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7), CurY, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7), LnAr(9))

            CurY = CurY + TxtHgt - 10
            Common_Procedures.Print_To_PrintDocument(e, "We certify that to the best of my/our knowledge the particulare are true, correct and complete.", LMargin + 10, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt + 10
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(12) = CurY

            CurY = CurY + TxtHgt + 5

            p1Font = New Font("Calibri", 12, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "Signature :", LMargin + 10, CurY, 0, 0, p1Font)

            p1Font = New Font("Calibri", 12, FontStyle.Bold)
            Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString

            Common_Procedures.Print_To_PrintDocument(e, "For " & Cmp_Name, PageWidth - 15, CurY, 1, 0, p1Font)

            'Common_Procedures.Print_To_PrintDocument(e, "Signature :", LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + 10, CurY, 0, 0, p1Font)

            CurY = CurY + TxtHgt + 10
            CurY = CurY + TxtHgt

            Common_Procedures.Print_To_PrintDocument(e, "Name :", LMargin + 10, CurY, 0, 0, p1Font)

            Common_Procedures.Print_To_PrintDocument(e, "AUTHORISED SIGNATORY ", PageWidth - 5, CurY, 1, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, "Name :", LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + 10, CurY, 0, 0, p1Font)

            CurY = CurY + TxtHgt + 10
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(13) = CurY

            CurY = CurY + TxtHgt - 10
            Common_Procedures.Print_To_PrintDocument(e, "Name and signature of the person to whom the goods were", LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "Name and signature of the consigner /", LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) - 20, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "delivered for transporting with status of person signing", LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "his employee / his representative", LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) - 20, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            CurY = CurY + TxtHgt

            p1Font = New Font("Calibri", 12, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "Place : ", LMargin + 10, CurY, 0, 0, p1Font)
            Common_Procedures.Print_To_PrintDocument(e, "Date : " & Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Bale_Transfer_Date").ToString), "dd-MM-yyyy"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) - 20, CurY, 0, 0, p1Font)

            CurY = CurY + TxtHgt + 10

            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            e.Graphics.DrawLine(Pens.Black, LMargin, LnAr(1), LMargin, CurY)
            e.Graphics.DrawLine(Pens.Black, PageWidth, LnAr(1), PageWidth, CurY)

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub
    Private Sub msk_LrDate_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles msk_LrDate.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            msk_LrDate.Text = Date.Today
        End If
        If IsDate(msk_LrDate.Text) = True Then
            If e.KeyCode = 107 Then
                msk_LrDate.Text = DateAdd("D", 1, Convert.ToDateTime(msk_LrDate.Text))
            ElseIf e.KeyCode = 109 Then
                msk_LrDate.Text = DateAdd("D", -1, Convert.ToDateTime(msk_LrDate.Text))
            End If
        End If
        If e.KeyCode = 46 Or e.KeyCode = 8 Then

            Common_Procedures.maskEdit_Date_ON_DelBackSpace(sender, e, vmskLrText, vmskLrStrt)
        End If
    End Sub
    Private Sub msk_LrDate_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles msk_LrDate.LostFocus

        If IsDate(msk_LrDate.Text) = True Then
            If Microsoft.VisualBasic.DateAndTime.Day(Convert.ToDateTime(msk_LrDate.Text)) <= 31 Or Microsoft.VisualBasic.DateAndTime.Month(Convert.ToDateTime(msk_LrDate.Text)) <= 31 Then
                If Microsoft.VisualBasic.DateAndTime.Year(Convert.ToDateTime(msk_LrDate.Text)) <= 2050 And Microsoft.VisualBasic.DateAndTime.Year(Convert.ToDateTime(msk_LrDate.Text)) >= 2000 Then
                    dtp_LrDate.Value = Convert.ToDateTime(msk_LrDate.Text)
                End If
            End If

        End If
    End Sub

    Private Sub dtp_LrDate_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dtp_LrDate.KeyPress
        If Asc(e.KeyChar) = 13 Then
            e.Handled = True
            dtp_LrDate.Text = msk_LrDate.Text
            cbo_DespTo.Focus()
        End If
    End Sub

    Private Sub dtp_LrDate_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dtp_LrDate.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            dtp_LrDate.Text = Date.Today
        End If
    End Sub

    Private Sub dtp_LrDate_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dtp_LrDate.TextChanged
        If IsDate(dtp_LrDate.Text) = True Then
            msk_LrDate.Text = dtp_LrDate.Text
            msk_LrDate.SelectionStart = 0
        End If
    End Sub

    Private Sub msk_LrDate_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles msk_LrDate.KeyDown
        vcbo_KeyDwnVal = e.KeyCode
        If e.KeyCode = 38 Then
            e.Handled = True : e.SuppressKeyPress = True
            txt_LrNo.Focus()
        End If
        If e.KeyCode = 40 Then
            e.Handled = True : e.SuppressKeyPress = True
            cbo_DespTo.Focus()
        End If

        vcbo_KeyDwnVal = e.KeyValue
        vmskLrText = ""
        vmskLrStrt = -1
        If e.KeyCode = 46 Or e.KeyCode = 8 Then
            vmskLrText = msk_LrDate.Text
            vmskLrStrt = msk_LrDate.SelectionStart
        End If

    End Sub

    Private Sub Amount_Calculation(ByVal CurRow As Integer, ByVal CurCol As Integer)
        Dim fldmtr As Double = 0
        Dim fmt As Double = 0
        Dim CloID As Integer
        Dim ConsYarn As Single
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt2 As New DataTable
        Dim StkIn_For As String = ""
        Dim mtr_pcs As Single = 0
        On Error Resume Next

        With dgv_Details
            If .Visible Then

                If CurCol = 3 Or CurCol = 6 Or CurCol = 7 Then

                    If CurCol = 3 Or CurCol = 6 Or CurCol = 7 Then

                        If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1044" Then

                            If Val(.Rows(CurRow).Cells(3).Value) = 0 Or Val(.Rows(CurRow).Cells(3).Value) = 100 Then

                                If Common_Procedures.settings.Weaver_Zari_Kuri_Entries_Status = 1 Then

                                    CloID = Common_Procedures.Cloth_NameToIdNo(con, .Rows(CurRow).Cells(1).Value)

                                    StkIn_For = ""
                                    mtr_pcs = 0

                                    Da = New SqlClient.SqlDataAdapter("Select Stock_In , Meters_Pcs from Cloth_Head Where Cloth_IdNo = " & Str(Val(CloID)), con)
                                    Dt2 = New DataTable
                                    Da.Fill(Dt2)
                                    If Dt2.Rows.Count > 0 Then
                                        StkIn_For = Dt2.Rows(0)("Stock_In").ToString
                                        mtr_pcs = Val(Dt2.Rows(0)("Meters_Pcs").ToString)
                                    End If
                                    Dt2.Clear()

                                    If Trim(StkIn_For) = "PCS" Then
                                        .Rows(CurRow).Cells(7).Value = Format(Val(.Rows(CurRow).Cells(6).Value) * Val(mtr_pcs), "#########0.00")
                                    End If

                                End If

                            Else

                                fmt = ((100 - Val(.Rows(CurRow).Cells(3).Value)) / 100) * Val(.Rows(CurRow).Cells(7).Value)


                                If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1035" Then
                                    fmt = Format(Math.Abs(Val(fmt)), "######0.00")
                                    fmt = Common_Procedures.Meter_RoundOff(fmt)

                                End If

                                If (100 - Val(.Rows(CurRow).Cells(3).Value)) > 0 Then
                                    fldmtr = Format(Val(.Rows(CurRow).Cells(7).Value) - Val(fmt), "#########0.00")
                                Else
                                    fldmtr = Format(Val(.Rows(CurRow).Cells(7).Value) + Val(fmt), "#########0.00")
                                End If



                            End If


                        End If

                    End If

                    Total_Calculation()

                End If

            End If
        End With
    End Sub

    Private Sub Printing_Bale_Delivery()
        Dim prtFrm As Single = 0
        Dim prtTo As Single = 0
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim Condt As String = ""
        Dim PpSzSTS As Boolean = False
        'Dim ps As Printing.PaperSize
        Dim NewCode As String = ""

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(EntFnYrCode)

        Try

            da1 = New SqlClient.SqlDataAdapter("select a.*, b.* from Packing_Slip_Head a INNER JOIN Cloth_Head b ON a.Cloth_IdNo <> 0 and a.Cloth_IdNo = b.Cloth_IdNo Where a.Delivery_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'", con)
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

        Dim pkCustomSize1 As New System.Drawing.Printing.PaperSize("PAPER 9X12", 900, 1200)
        PrintDocument4.PrinterSettings.DefaultPageSettings.PaperSize = pkCustomSize1
        PrintDocument4.DefaultPageSettings.PaperSize = pkCustomSize1

        If Val(Common_Procedures.Print_OR_Preview_Status) = 1 Then

            Try

                PrintDialog1.PrinterSettings = PrintDocument4.PrinterSettings
                If PrintDialog1.ShowDialog() = Windows.Forms.DialogResult.OK Then
                    PrintDocument4.PrinterSettings = PrintDialog1.PrinterSettings
                    PrintDocument4.Print()
                End If

            Catch ex As Exception
                MessageBox.Show("The printing operation failed" & vbCrLf & ex.Message, "DOES NOT SHOW PRINT PREVIEW...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

            End Try

        Else

            Try

                Dim ppd As New PrintPreviewDialog

                ppd.Document = PrintDocument4

                ppd.WindowState = FormWindowState.Normal
                ppd.StartPosition = FormStartPosition.CenterScreen
                ppd.ClientSize = New Size(600, 600)

                ppd.ShowDialog()
                'If PageSetupDialog1.ShowDialog() = Windows.Forms.DialogResult.OK Then
                '    PrintDocument4.DefaultPageSettings = PageSetupDialog1.PageSettings
                '    ppd.ShowDialog()
                'End If

            Catch ex As Exception
                MsgBox("The printing operation failed" & vbCrLf & ex.Message, MsgBoxStyle.Critical, "DOES NOT SHOW PRINT PREVIEW...")

            End Try

        End If

        pnl_Back.Enabled = True
        pnl_Print.Visible = False
    End Sub

    Private Sub PrintDocument4_BeginPrint(ByVal sender As Object, ByVal e As System.Drawing.Printing.PrintEventArgs) Handles PrintDocument4.BeginPrint
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim cmd As New SqlClient.SqlCommand
        Dim NewCode As String
        Dim i As Integer
        Dim OrdBy_FrmNo As Single = 0, OrdByToNo As Single = 0
        Dim LotSTS As Boolean = False
        Dim PrevBaleCd As String = ""
        Dim vSlNo As Integer = 0
        Dim vTotMtrs As Single = 0
        Dim vTotWgt As Single = 0

        prn_HdDt.Clear()
        prn_DetDt.Clear()
        prn_Prev_HeadIndx = -100
        prn_HeadIndx = 0
        prn_HdIndx = 0
        prn_HdMxIndx = 0
        prn_DetIndx = 0
        prn_DetSNo = 0
        prn_PageNo = 0
        prn_DetMxIndx = 0
        prn_NoofBaleDets = 0
        prn_Count = 0
        prn_TotBlWgt = 0
        prn_TotBlMtr = 0
        prn_TotBls = 0
        prn_BaleCode1 = ""
        prn_BaleCode2 = ""

        Erase prn_BLDetAr
        prn_BLDetAr = New String(1000, 10) {}

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(EntFnYrCode)

        Try

            da1 = New SqlClient.SqlDataAdapter("select a.*, b.*, c.*, d.Ledger_Name as TransportName, e.Ledger_Name as Agent_Name from Bale_Transfer_Head a INNER JOIN Company_Head b ON a.Company_IdNo <> 0 and a.Company_IdNo = b.Company_IdNo INNER JOIN Ledger_Head c ON a.Ledger_IdNo <> 0 and a.Ledger_IdNo = c.Ledger_IdNo LEFT OUTER JOIN Ledger_Head d ON a.Transport_IdNo = d.Ledger_IdNo LEFT OUTER JOIN Ledger_Head e ON a.Agent_IdNo = e.Ledger_IdNo Where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Bale_Transfer_Code = '" & Trim(NewCode) & "'", con)
            prn_HdDt = New DataTable
            da1.Fill(prn_HdDt)

            If prn_HdDt.Rows.Count > 0 Then

                da2 = New SqlClient.SqlDataAdapter("select a.*, b.*, tC.* from Packing_Slip_Head a LEFT OUTER JOIN Packing_Slip_Details b ON a.Company_IdNo = b.Company_IdNo and a.Packing_Slip_Code = b.Packing_Slip_Code INNER JOIN Cloth_Head tC ON tC.Cloth_IdNo <> 0 and a.Cloth_IdNo = tC.Cloth_IdNo Where a.Delivery_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' Order by a.Packing_Slip_Date, a.for_OrderBy, a.Packing_Slip_No, a.Packing_Slip_Code, b.Sl_No", con)
                'da2 = New SqlClient.SqlDataAdapter("select a.*, b.*, c.*, tZ.*, tC.*, tP.* from Packing_Slip_Head a LEFT OUTER JOIN Packing_Slip_Details b ON a.Company_IdNo = b.Company_IdNo and a.Packing_Slip_Code = b.Packing_Slip_Code INNER JOIN Bale_Transfer_Head c ON c.Bale_Transfer_Code =  '" & Trim(NewCode) & "' and a.Delivery_Code = '" & Trim(Pk_Condition) & "' + c.Bale_Transfer_Code INNER JOIN Company_head tZ ON tZ.company_idno <> 0 and c.company_idno = tZ.Company_Idno INNER JOIN Cloth_Head tC ON tC.Cloth_IdNo <> 0 and a.Cloth_IdNo = tC.Cloth_IdNo LEFT OUTER JOIN Ledger_Head tP ON tP.Ledger_IdNo <> 0 and c.Ledger_IdNo = tP.Ledger_IdNo Where a.Delivery_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' Order by a.Packing_Slip_Date, a.for_OrderBy, a.Packing_Slip_No, a.Packing_Slip_Code, b.Sl_No", con)
                prn_DetDt = New DataTable
                da2.Fill(prn_DetDt)

                If prn_DetDt.Rows.Count > 0 Then

                    PrevBaleCd = ""
                    vSlNo = 0
                    vTotMtrs = 0
                    vTotWgt = 0

                    For i = 0 To prn_DetDt.Rows.Count - 1

                        LotSTS = False
                        If IsDBNull(prn_DetDt.Rows(i).Item("Lot_No").ToString) = False Then
                            If Trim(prn_DetDt.Rows(i).Item("Lot_No").ToString) <> "" Then
                                LotSTS = True
                            End If
                        End If

                        prn_DetMxIndx = prn_DetMxIndx + 1
                        If Trim(UCase(PrevBaleCd)) <> Trim(UCase(prn_DetDt.Rows(i).Item("Packing_Slip_Code").ToString)) Then

                            If Trim(UCase(PrevBaleCd)) <> "" Then
                                prn_BLDetAr(prn_DetMxIndx, 1) = ""
                                prn_BLDetAr(prn_DetMxIndx, 2) = ""
                                prn_BLDetAr(prn_DetMxIndx, 3) = ""
                                prn_BLDetAr(prn_DetMxIndx, 4) = ""
                                prn_BLDetAr(prn_DetMxIndx, 5) = "BALETOTALLINE"

                                prn_DetMxIndx = prn_DetMxIndx + 1
                                prn_BLDetAr(prn_DetMxIndx, 1) = ""
                                prn_BLDetAr(prn_DetMxIndx, 2) = ""
                                prn_BLDetAr(prn_DetMxIndx, 3) = Format(Val(vTotMtrs), "#########0.00")
                                prn_BLDetAr(prn_DetMxIndx, 4) = Format(Val(vTotWgt), "#########0.000")
                                prn_BLDetAr(prn_DetMxIndx, 5) = "BALETOTAL"

                                prn_DetMxIndx = prn_DetMxIndx + 1
                                prn_BLDetAr(prn_DetMxIndx, 1) = ""
                                prn_BLDetAr(prn_DetMxIndx, 2) = ""
                                prn_BLDetAr(prn_DetMxIndx, 3) = ""
                                prn_BLDetAr(prn_DetMxIndx, 4) = ""
                                prn_BLDetAr(prn_DetMxIndx, 5) = "BALETOTALLINE"

                                'prn_BLDetAr(prn_DetMxIndx, 1) = ""
                                'prn_BLDetAr(prn_DetMxIndx, 2) = ""
                                'prn_BLDetAr(prn_DetMxIndx, 3) = ""
                                'prn_BLDetAr(prn_DetMxIndx, 4) = ""
                                'prn_BLDetAr(prn_DetMxIndx, 5) = "BLANKROW"
                            End If

                            prn_DetMxIndx = prn_DetMxIndx + 1
                            prn_BLDetAr(prn_DetMxIndx, 1) = Trim(prn_DetDt.Rows(i).Item("Packing_Slip_No").ToString)

                        End If

                        vSlNo = vSlNo + 1
                        prn_BLDetAr(prn_DetMxIndx, 0) = Trim(vSlNo)

                        If LotSTS = True Then
                            prn_BLDetAr(prn_DetMxIndx, 2) = Trim(prn_DetDt.Rows(i).Item("Lot_No").ToString) & "/" & Trim(prn_DetDt.Rows(i).Item("Pcs_No").ToString)
                            prn_BLDetAr(prn_DetMxIndx, 3) = Format(Val(prn_DetDt.Rows(i).Item("Meters").ToString), "#########0.00")
                            prn_BLDetAr(prn_DetMxIndx, 4) = Format(Val(prn_DetDt.Rows(i).Item("Weight").ToString), "#########0.000")
                        Else
                            prn_BLDetAr(prn_DetMxIndx, 2) = ""
                            prn_BLDetAr(prn_DetMxIndx, 3) = Format(Val(prn_DetDt.Rows(i).Item("Total_Meters").ToString), "#########0.00")
                            prn_BLDetAr(prn_DetMxIndx, 4) = Format(Val(prn_DetDt.Rows(i).Item("Total_Weight").ToString), "#########0.000")
                        End If

                        vTotMtrs = vTotMtrs + Val(prn_DetDt.Rows(i).Item("Total_Meters").ToString)
                        vTotWgt = vTotWgt + Val(prn_DetDt.Rows(i).Item("Total_Weight").ToString)

                        If Val(prn_BLDetAr(prn_DetMxIndx, 4)) = 0 Then prn_BLDetAr(prn_DetMxIndx, 4) = ""
                        prn_BLDetAr(prn_DetMxIndx, 5) = ""

                        PrevBaleCd = prn_DetDt.Rows(i).Item("Packing_Slip_Code").ToString

                    Next i

                    prn_DetMxIndx = prn_DetMxIndx + 1
                    prn_BLDetAr(prn_DetMxIndx, 1) = ""
                    prn_BLDetAr(prn_DetMxIndx, 2) = ""
                    prn_BLDetAr(prn_DetMxIndx, 3) = ""
                    prn_BLDetAr(prn_DetMxIndx, 4) = ""
                    prn_BLDetAr(prn_DetMxIndx, 5) = "BALETOTALLINE"

                    prn_DetMxIndx = prn_DetMxIndx + 1
                    prn_BLDetAr(prn_DetMxIndx, 1) = ""
                    prn_BLDetAr(prn_DetMxIndx, 2) = ""
                    prn_BLDetAr(prn_DetMxIndx, 3) = Format(Val(vTotMtrs), "#########0.00")
                    prn_BLDetAr(prn_DetMxIndx, 4) = Format(Val(vTotWgt), "#########0.000")
                    prn_BLDetAr(prn_DetMxIndx, 5) = "BALETOTAL"

                    prn_DetMxIndx = prn_DetMxIndx + 1
                    prn_BLDetAr(prn_DetMxIndx, 1) = ""
                    prn_BLDetAr(prn_DetMxIndx, 2) = ""
                    prn_BLDetAr(prn_DetMxIndx, 3) = ""
                    prn_BLDetAr(prn_DetMxIndx, 4) = ""
                    prn_BLDetAr(prn_DetMxIndx, 5) = "BALETOTALLINE"

                End If

            Else

                MessageBox.Show("This is New Entry", "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

            End If

            da1.Dispose()
            da2.Dispose()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub PrintDocument4_PrintPage(ByVal sender As System.Object, ByVal e As System.Drawing.Printing.PrintPageEventArgs) Handles PrintDocument4.PrintPage
        If prn_HdDt.Rows.Count <= 0 Then Exit Sub
        Printing_Bale_Format2_PrePrint_Anuman(e)
    End Sub

    Private Sub Printing_Bale_Format2_PrePrint_Anuman(ByRef e As System.Drawing.Printing.PrintPageEventArgs)
        Dim cmd As New SqlClient.SqlCommand
        Dim Da1 As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim EntryCode As String
        Dim I As Integer = 0
        Dim NoofDets As Integer, NoofItems_PerPage As Integer
        Dim pFont As Font
        Dim LMargin As Single, RMargin As Single, TMargin As Single, BMargin As Single
        Dim PrintWidth As Single, PrintHeight As Single
        Dim PageWidth As Single, PageHeight As Single
        Dim CurY As Single, TxtHgt As Single
        Dim LnAr(15) As Single, ClArr(15) As Single
        Dim MilNm1 As String = "", MilNm2 As String = ""
        'Dim ps As Printing.PaperSize
        Dim strHeight As Single = 0
        Dim PpSzSTS As Boolean = False
        Dim W1 As Single = 0
        Dim TotMtr1 As Single = 0, TotMtr2 As Single = 0
        Dim TotWgt1 As Single = 0, TotWgt2 As Single = 0
        Dim IncY As Single = 0



        Dim pkCustomSize1 As New System.Drawing.Printing.PaperSize("PAPER 9X12", 900, 1200)
        PrintDocument4.PrinterSettings.DefaultPageSettings.PaperSize = pkCustomSize1
        PrintDocument4.DefaultPageSettings.PaperSize = pkCustomSize1
        e.PageSettings.PaperSize = pkCustomSize1
        PrintDocument4.DefaultPageSettings.Landscape = False

        With PrintDocument4.DefaultPageSettings.Margins
            .Left = 0
            .Right = 0
            .Top = 0
            .Bottom = 0
            LMargin = .Left
            RMargin = .Right
            TMargin = .Top
            BMargin = .Bottom

        End With

        pFont = New Font("Arial", 12, FontStyle.Bold)
        'pFont = New Font("Calibri", 11, FontStyle.Bold)

        With PrintDocument4.DefaultPageSettings.PaperSize
            PrintWidth = .Width - RMargin - LMargin
            PrintHeight = .Height - TMargin - BMargin
            PageWidth = .Width - RMargin
            PageHeight = .Height - BMargin
        End With

        NoofItems_PerPage = 30

        Erase LnAr
        Erase ClArr
        LnAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        ClArr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}

        'TxtHgt = e.Graphics.MeasureString("A", pFont).Height  '18.69
        TxtHgt = 19 ' e.Graphics.MeasureString("A", pFont).Height  '18.69

        EntryCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(EntFnYrCode)

        Try

            If prn_HdDt.Rows.Count > 0 Then

                If prn_HeadIndx <= prn_HdDt.Rows.Count - 1 Then

                    Printing_Bale_Format2_PrePrint_Anuman_PageHeader(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, prn_PageNo, NoofItems_PerPage, CurY, LnAr, ClArr)

                    NoofDets = 0
                    TotMtr1 = 0 : TotMtr2 = 0
                    TotWgt1 = 0 : TotWgt2 = 0

                    If prn_DetMxIndx > 0 Then

                        CurY = TMargin + 380

                        Do While prn_NoofBaleDets < prn_DetMxIndx

                            If NoofDets >= NoofItems_PerPage Then

                                CurY = CurY + TxtHgt

                                Common_Procedures.Print_To_PrintDocument(e, "Continued....", PageWidth - 10, CurY, 1, 0, pFont)

                                NoofDets = NoofDets + 1

                                CurY = TMargin + 990
                                Common_Procedures.Print_To_PrintDocument(e, Format(Val(TotMtr1), "#########0.00"), LMargin + 270, CurY, 1, 0, pFont)
                                Common_Procedures.Print_To_PrintDocument(e, Format(Val(TotWgt1), "#########0.000"), LMargin + 370, CurY, 1, 0, pFont)
                                If Val(TotMtr2) <> 0 Then
                                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(TotMtr2), "#########0.00"), LMargin + 680, CurY, 1, 0, pFont)
                                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(TotWgt2), "#########0.000"), LMargin + 770, CurY, 1, 0, pFont)
                                End If

                                Printing_Bale_Format2_PrePrint_Anuman_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, NoofDets, False)

                                prn_DetIndx = prn_DetIndx + NoofItems_PerPage
                                prn_DetSNo = prn_DetSNo + NoofItems_PerPage

                                e.HasMorePages = True

                                Return

                            End If


                            prn_DetIndx = prn_DetIndx + 1

                            CurY = CurY + TxtHgt

                            If Val(prn_BLDetAr(prn_DetIndx, 3)) <> 0 Or Val(prn_BLDetAr(prn_DetIndx + NoofItems_PerPage, 3)) <> 0 Then

                                prn_DetSNo = prn_DetSNo + 1

                                If Val(prn_BLDetAr(prn_DetIndx, 3)) <> 0 Or Trim(UCase(prn_BLDetAr(prn_DetIndx, 5))) = "BALETOTAL" Or Trim(UCase(prn_BLDetAr(prn_DetIndx, 5))) = "BLANKROW" Then

                                    IncY = 0
                                    If Trim(UCase(prn_BLDetAr(prn_DetIndx, 5))) = "BALETOTAL" Then
                                        IncY = 10
                                    End If

                                    Common_Procedures.Print_To_PrintDocument(e, Trim(prn_BLDetAr(prn_DetIndx, 0)), LMargin + 20, CurY - IncY, 0, 0, pFont)
                                    'Common_Procedures.Print_To_PrintDocument(e, Trim(Val(prn_DetSNo)), LMargin + 20, CurY-IncY, 0, 0, pFont)
                                    Common_Procedures.Print_To_PrintDocument(e, Trim(prn_BLDetAr(prn_DetIndx, 1)), LMargin + 60, CurY - IncY, 0, 0, pFont)
                                    Common_Procedures.Print_To_PrintDocument(e, Trim(prn_BLDetAr(prn_DetIndx, 2)), LMargin + 140, CurY - IncY, 0, 0, pFont)
                                    Common_Procedures.Print_To_PrintDocument(e, Trim(prn_BLDetAr(prn_DetIndx, 3)), LMargin + 270, CurY - IncY, 1, 0, pFont)
                                    Common_Procedures.Print_To_PrintDocument(e, Trim(prn_BLDetAr(prn_DetIndx, 4)), LMargin + 370, CurY - IncY, 1, 0, pFont)

                                    TotMtr1 = TotMtr1 + Val(prn_BLDetAr(prn_DetIndx, 3))
                                    TotWgt1 = TotWgt1 + Val(prn_BLDetAr(prn_DetIndx, 4))

                                    prn_TotBls = prn_TotBls + 1
                                    prn_TotBlMtr = prn_TotBlMtr + Val(prn_BLDetAr(prn_DetIndx, 3))
                                    prn_TotBlWgt = prn_TotBlWgt + Val(prn_BLDetAr(prn_DetIndx, 4))

                                    prn_NoofBaleDets = prn_NoofBaleDets + 1

                                    prn_BaleCode1 = Trim(prn_BLDetAr(prn_DetIndx, 1))

                                Else

                                    If Trim(UCase(prn_BLDetAr(prn_DetIndx, 5))) = "BALETOTALLINE" Then
                                        'CurY = CurY + TxtHgt
                                        e.Graphics.DrawLine(Pens.Black, LMargin + 5, CurY, LMargin + 380, CurY)
                                        prn_NoofBaleDets = prn_NoofBaleDets + 1
                                    End If

                                End If


                                If Val(prn_BLDetAr(prn_DetIndx + NoofItems_PerPage, 3)) <> 0 Or Trim(UCase(prn_BLDetAr(prn_DetIndx + NoofItems_PerPage, 5))) = "BALETOTAL" Or Trim(UCase(prn_BLDetAr(prn_DetIndx + NoofItems_PerPage, 5))) = "BLANKROW" Then

                                    IncY = 0
                                    If Trim(UCase(prn_BLDetAr(prn_DetIndx + NoofItems_PerPage, 5))) = "BALETOTAL" Then
                                        IncY = 10
                                    End If

                                    Common_Procedures.Print_To_PrintDocument(e, Trim(prn_BLDetAr(prn_DetIndx + NoofItems_PerPage, 0)), LMargin + 410, CurY - IncY, 0, 0, pFont)
                                    'Common_Procedures.Print_To_PrintDocument(e, Trim(Val(prn_DetSNo + NoofItems_PerPage)), LMargin + 410, CurY-IncY, 0, 0, pFont)
                                    Common_Procedures.Print_To_PrintDocument(e, Trim(prn_BLDetAr(prn_DetIndx + NoofItems_PerPage, 1)), LMargin + 460, CurY - IncY, 0, 0, pFont)
                                    Common_Procedures.Print_To_PrintDocument(e, Trim(prn_BLDetAr(prn_DetIndx + NoofItems_PerPage, 2)), LMargin + 540, CurY - IncY, 0, 0, pFont)
                                    Common_Procedures.Print_To_PrintDocument(e, Trim(prn_BLDetAr(prn_DetIndx + NoofItems_PerPage, 3)), LMargin + 680, CurY - IncY, 1, 0, pFont)
                                    Common_Procedures.Print_To_PrintDocument(e, Trim(prn_BLDetAr(prn_DetIndx + NoofItems_PerPage, 4)), LMargin + 770, CurY - IncY, 1, 0, pFont)

                                    TotMtr2 = TotMtr2 + Val(prn_BLDetAr(prn_DetIndx + NoofItems_PerPage, 3))
                                    TotWgt2 = TotWgt2 + Val(prn_BLDetAr(prn_DetIndx + NoofItems_PerPage, 4))

                                    prn_TotBls = prn_TotBls + 1
                                    prn_TotBlMtr = prn_TotBlMtr + Val(prn_BLDetAr(prn_DetIndx + NoofItems_PerPage, 3))
                                    prn_TotBlWgt = prn_TotBlWgt + Val(prn_BLDetAr(prn_DetIndx + NoofItems_PerPage, 4))

                                    prn_NoofBaleDets = prn_NoofBaleDets + 1

                                    prn_BaleCode2 = Trim(prn_BLDetAr(prn_DetIndx + NoofItems_PerPage, 1))

                                Else

                                    If Trim(UCase(prn_BLDetAr(prn_DetIndx + NoofItems_PerPage, 5))) = "BALETOTALLINE" Then
                                        'CurY = CurY + TxtHgt
                                        e.Graphics.DrawLine(Pens.Black, LMargin + 400, CurY, LMargin + 775, CurY)
                                        prn_NoofBaleDets = prn_NoofBaleDets + 1
                                    End If

                                End If


                            Else

                                If Trim(UCase(prn_BLDetAr(prn_DetIndx, 5))) = "BALETOTALLINE" Then
                                    e.Graphics.DrawLine(Pens.Black, LMargin + 5, CurY, LMargin + 380, CurY)
                                    prn_NoofBaleDets = prn_NoofBaleDets + 1
                                End If
                                If Trim(UCase(prn_BLDetAr(prn_DetIndx + NoofItems_PerPage, 5))) = "BALETOTALLINE" Then
                                    e.Graphics.DrawLine(Pens.Black, LMargin + 400, CurY, LMargin + 775, CurY)
                                    prn_NoofBaleDets = prn_NoofBaleDets + 1
                                End If

                                If Trim(UCase(prn_BLDetAr(prn_DetIndx, 5))) = "BLANKROW" Then
                                    prn_NoofBaleDets = prn_NoofBaleDets + 1
                                End If
                                If Trim(UCase(prn_BLDetAr(prn_DetIndx + NoofItems_PerPage, 5))) = "BLANKROW" Then
                                    prn_NoofBaleDets = prn_NoofBaleDets + 1
                                End If
                                If Trim(UCase(prn_BLDetAr(prn_DetIndx, 5))) <> "BLANKROW" And Trim(UCase(prn_BLDetAr(prn_DetIndx + NoofItems_PerPage, 5))) <> "BLANKROW" And Trim(UCase(prn_BLDetAr(prn_DetIndx, 5))) <> "BALETOTALLINE" And Trim(UCase(prn_BLDetAr(prn_DetIndx + NoofItems_PerPage, 5))) <> "BALETOTALLINE" Then
                                    prn_NoofBaleDets = prn_NoofBaleDets + 1
                                End If

                            End If

                            NoofDets = NoofDets + 1

                        Loop

                    End If

                    CurY = TMargin + 990
                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(TotMtr1), "#########0.00"), LMargin + 270, CurY, 1, 0, pFont)
                    If Val(TotWgt1) <> 0 Then
                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(TotWgt1), "#########0.000"), LMargin + 370, CurY, 1, 0, pFont)
                    End If
                    If Val(TotMtr2) <> 0 Then
                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(TotMtr2), "#########0.00"), LMargin + 680, CurY, 1, 0, pFont)
                        If Val(TotWgt2) <> 0 Then
                            Common_Procedures.Print_To_PrintDocument(e, Format(Val(TotWgt2), "#########0.000"), LMargin + 770, CurY, 1, 0, pFont)
                        End If
                    End If

                    Printing_Bale_Format2_PrePrint_Anuman_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, NoofDets, True)

                End If

            End If

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

        prn_HeadIndx = prn_HeadIndx + 1

        'If prn_HeadIndx <= prn_HdDt.Rows.Count - 1 Then
        '    e.HasMorePages = True
        'Else
        e.HasMorePages = False
        'End If

    End Sub

    Private Sub Printing_Bale_Format2_PrePrint_Anuman_PageHeader(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByRef PageNo As Integer, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single)
        Dim p1Font As Font
        Dim CurX As Single = 0


        PageNo = PageNo + 1

        'da2 = New SqlClient.SqlDataAdapter("select a.*, b.*, c.*, tZ.*, tC.*, tP.* from Packing_Slip_Head a LEFT OUTER JOIN Packing_Slip_Details b ON a.Company_IdNo = b.Company_IdNo and a.Packing_Slip_Code = b.Packing_Slip_Code INNER JOIN Bale_Transfer_Head c ON c.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and c.Bale_Transfer_Code =  '" & Trim(EntryCode) & "' and a.Delivery_Code = '" & Trim(Pk_Condition) & "' + c.Bale_Transfer_Code INNER JOIN Company_head tZ ON tZ.company_idno <> 0 and c.company_idno = tZ.Company_Idno INNER JOIN Cloth_Head tC ON tC.Cloth_IdNo <> 0 and a.Cloth_IdNo = tC.Cloth_IdNo LEFT OUTER JOIN Ledger_Head tP ON tP.Ledger_IdNo <> 0 and c.Ledger_IdNo = tP.Ledger_IdNo Where a.Delivery_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' Order by a.Packing_Slip_Date, a.for_OrderBy, a.Packing_Slip_No, a.Packing_Slip_Code, b.Sl_No", con)
        'dt2 = New DataTable
        'da2.Fill(dt2)
        'If dt2.Rows.Count > NoofItems_PerPage Then
        '    Common_Procedures.Print_To_PrintDocument(e, "Page : " & Trim(Val(PageNo)), PageWidth - 10, CurY - TxtHgt, 1, 0, pFont)
        'End If
        'dt2.Clear()

        Try

            CurX = LMargin + 60 ' 40  '150
            CurY = TMargin + 140 ' 122 ' 100

            p1Font = New Font("Arial", 13, FontStyle.Bold)
            'p1Font = New Font("Calibri", 11, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "M/s." & prn_HdDt.Rows(prn_HeadIndx).Item("Ledger_MainName").ToString, CurX, CurY, 0, 0, p1Font)
            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(prn_HeadIndx).Item("Ledger_Address1").ToString, CurX, CurY, 0, 0, pFont)
            If Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Ledger_Address2").ToString) <> "" Then
                CurY = CurY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(prn_HeadIndx).Item("Ledger_Address2").ToString, CurX, CurY, 0, 0, pFont)
            End If
            If Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Ledger_Address3").ToString) <> "" Then
                CurY = CurY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(prn_HeadIndx).Item("Ledger_Address3").ToString, CurX, CurY, 0, 0, pFont)
            End If
            If Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Ledger_Address4").ToString) <> "" Then
                CurY = CurY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(prn_HeadIndx).Item("Ledger_Address4").ToString, CurX, CurY, 0, 0, pFont)
            End If
            If Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Ledger_TinNo").ToString) <> "" Then
                CurY = CurY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, " Tin No : " & prn_HdDt.Rows(prn_HeadIndx).Item("Ledger_TinNo").ToString, CurX, CurY, 0, 0, pFont)
            End If


            CurX = LMargin + 580
            CurY = TMargin + 140
            p1Font = New Font("Arial", 14, FontStyle.Bold)
            'p1Font = New Font("Calibri", 14, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(prn_HeadIndx).Item("Bale_Transfer_No").ToString, CurX, CurY, 0, 0, pFont)

            CurY = TMargin + 170
            Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(prn_HeadIndx).Item("Bale_Transfer_Date").ToString), "dd-MM-yyyy"), CurX, CurY, 0, 0, pFont)

            CurY = TMargin + 215
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(prn_HeadIndx).Item("Party_OrderNo").ToString, CurX, CurY, 0, 0, pFont)


            If prn_DetDt.Rows.Count > 0 Then
                CurX = LMargin + 60
                CurY = TMargin + 260
                Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(0).Item("Cloth_Name").ToString, CurX, CurY, 0, 0, pFont)


                CurX = LMargin + 540
                CurY = TMargin + 260
                Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(0).Item("Weave").ToString, CurX, CurY, 0, 0, pFont)

            End If

            CurX = LMargin + 120
            CurY = TMargin + 300
            If Common_Procedures.Ledger_IdNoToName(con, Val(prn_HdDt.Rows(prn_HeadIndx).Item("Agent_IdNo").ToString)) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, Common_Procedures.Ledger_IdNoToName(con, Val(prn_HdDt.Rows(prn_HeadIndx).Item("Agent_IdNo").ToString)), CurX, CurY, 0, 0, pFont)
            Else
                Common_Procedures.Print_To_PrintDocument(e, " DIRECT", CurX, CurY, 0, 0, pFont)
            End If

            CurY = TMargin + 330
            If Val(prn_HdDt.Rows(prn_HeadIndx).Item("Transport_idNo").ToString) <> 0 Then
                Common_Procedures.Print_To_PrintDocument(e, Common_Procedures.Ledger_IdNoToName(con, Val(prn_HdDt.Rows(prn_HeadIndx).Item("Transport_idNo").ToString)), CurX, CurY, 0, 0, pFont)
            End If


            CurX = LMargin + 540
            CurY = TMargin + 300
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(prn_HeadIndx).Item("Despatch_To").ToString, CurX, CurY, 0, 0, pFont)
            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(prn_HeadIndx).Item("Delivery_Address1").ToString, CurX, CurY, 0, 0, pFont)
            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(prn_HeadIndx).Item("Delivery_Address2").ToString, CurX, CurY, 0, 0, pFont)


        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub Printing_Bale_Format2_PrePrint_Anuman_PageFooter(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single, ByVal NoofDets As Integer, ByVal is_LastPage As Boolean)
        Dim CurX As Single = 0

        Try

            If is_LastPage = True Then

                CurX = LMargin + 120
                CurY = TMargin + 1050
                Common_Procedures.Print_To_PrintDocument(e, Val(prn_TotBls), CurX, CurY, 0, 0, pFont)
                'Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(prn_HeadIndx).Item("Total_Bales").ToString, CurX, CurY, 0, 0, pFont)

                CurY = TMargin + 1080
                Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_TotBlMtr), "###########0.00"), CurX, CurY, 0, 0, pFont)
                'Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(prn_HeadIndx).Item("Total_Meters").ToString, CurX, CurY, 0, 0, pFont)

                CurY = TMargin + 1120
                If Val(prn_TotBlWgt) <> 0 Then
                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_TotBlWgt), "###########0.00"), CurX, CurY, 0, 0, pFont)
                End If

            End If

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub set_PaperSize_For_PrintDocument1()
        Dim I As Integer = 0
        Dim PpSzSTS As Boolean = False
        Dim ps As Printing.PaperSize


        If Val(Common_Procedures.settings.Printing_For_HalfSheet_Set_Custom8X6_As_Default_PaperSize) = 1 Then
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

    Private Sub cbo_RollBundle_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_RollBundle.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_RollBundle, txt_JJFormNo, txt_DelvAdd1, "", "", "", "")
    End Sub

    Private Sub cbo_RollBundle_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_RollBundle.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_RollBundle, txt_DelvAdd1, "", "", "", "")
    End Sub

    Private Sub cbo_Vechile_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Vechile.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Bale_Transfer_Head", "Vechile_No", "", "")
    End Sub

    Private Sub cbo_Vechile_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Vechile.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Vechile, txt_Freight, txt_Note, "Bale_Transfer_Head", "Vechile_No", "", "")
    End Sub

    Private Sub cbo_Vechile_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Vechile.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Vechile, txt_Note, "Bale_Transfer_Head", "Vechile_No", "", "", False)
    End Sub

    Private Sub btn_Print_Bundle_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Print_Bundle.Click
        Printing_Bundle()
        btn_print_Close_Click(sender, e)
    End Sub

    Public Sub Printing_Bundle()
        Dim prtFrm As Single = 0
        Dim prtTo As Single = 0
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim Condt As String = ""
        Dim PpSzSTS As Boolean = False
        Dim ps As Printing.PaperSize
        Dim NewCode As String

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(EntFnYrCode)

        Try

            da1 = New SqlClient.SqlDataAdapter("select a.*, b.* from Packing_Slip_Head a INNER JOIN Cloth_Head b ON a.Cloth_IdNo <> 0 and a.Cloth_IdNo = b.Cloth_IdNo Where a.Delivery_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'", con)
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

        For I = 0 To PrintDocument5.PrinterSettings.PaperSizes.Count - 1
            If PrintDocument5.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
                ps = PrintDocument5.PrinterSettings.PaperSizes(I)
                PrintDocument5.DefaultPageSettings.PaperSize = ps
                Exit For
            End If
        Next

        If Val(Common_Procedures.Print_OR_Preview_Status) = 1 Then
            Try

                If Val(Common_Procedures.settings.Printing_Show_PrintDialogue) = 1 Then
                    PrintDialog1.PrinterSettings = PrintDocument5.PrinterSettings
                    If PrintDialog1.ShowDialog() = Windows.Forms.DialogResult.OK Then
                        PrintDocument5.PrinterSettings = PrintDialog1.PrinterSettings
                        PrintDocument5.Print()
                    End If

                Else
                    PrintDocument5.Print()

                End If

            Catch ex As Exception
                MessageBox.Show("The printing operation failed" & vbCrLf & ex.Message, "DOES NOT SHOW PRINT PREVIEW...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

            End Try

        Else

            Try

                Dim ppd As New PrintPreviewDialog

                ppd.Document = PrintDocument5

                ppd.WindowState = FormWindowState.Normal
                ppd.StartPosition = FormStartPosition.CenterScreen
                ppd.ClientSize = New Size(600, 600)

                ppd.ShowDialog()

            Catch ex As Exception
                MsgBox("The printing operation failed" & vbCrLf & ex.Message, MsgBoxStyle.Critical, "DOES NOT SHOW PRINT PREVIEW...")

            End Try

        End If

        pnl_Back.Enabled = True
        pnl_Print.Visible = False

    End Sub

    Private Sub PrintDocument5_BeginPrint(ByVal sender As Object, ByVal e As System.Drawing.Printing.PrintEventArgs) Handles PrintDocument5.BeginPrint
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim NewCode As String = ""


        prn_HdDt.Clear()
        prn_DetDt.Clear()

        prn_PageNo = 0
        prn_HdIndx = 0
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

        prn_HdAr = New String(100, 10) {}

        prn_DetAr = New String(100, 50, 10) {}

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(EntFnYrCode)

        Try

            da1 = New SqlClient.SqlDataAdapter("select a.Packing_Slip_No, a.cloth_idno, a.Total_Pcs as Pak_Pcs, a.Packing_Slip_Code, a.Total_Weight, a.Total_Meters as Pak_Mtrs, tZ.*, tC.*, tL.*, tE.*  from Packing_Slip_Head a INNER JOIN Company_head tZ ON a.company_idno = tZ.Company_Idno INNER JOIN Bale_Transfer_Head tE ON tE.Bale_Transfer_Code = '" & Trim(NewCode) & "'  INNER JOIN Ledger_Head tL ON tE.Ledger_IdNo = tL.Ledger_IdNo INNER JOIN Cloth_Head tC ON a.Cloth_IdNo = tC.Cloth_IdNo Where a.Delivery_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' Order by a.Packing_Slip_Date, a.for_OrderBy, a.Packing_Slip_No, a.Packing_Slip_Code", con)
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

    End Sub

    Private Sub PrintDocument5_PrintPage(ByVal sender As System.Object, ByVal e As System.Drawing.Printing.PrintPageEventArgs) Handles PrintDocument5.PrintPage
        If prn_HdDt.Rows.Count <= 0 Then Exit Sub
        Printing_BundlePrint_Format_1(PrintDocument5, e)
    End Sub

    Private Sub Printing_BundlePrint_Format_1(ByRef PrintDocument1 As Printing.PrintDocument, ByRef e As System.Drawing.Printing.PrintPageEventArgs)
        Dim NoofDets As Integer, NoofItems_PerPage As Integer
        Dim pFont As Font, P1fONT As Font
        Dim LMargin As Single, RMargin As Single, TMargin As Single, BMargin As Single
        Dim PrintWidth As Single, PrintHeight As Single
        Dim PageWidth As Single, PageHeight As Single
        Dim CurY As Single, TxtHgt As Single
        Dim LnAr(15) As Single, ClArr(15) As Single
        Dim ps As Printing.PaperSize
        Dim strHeight As Single = 0
        Dim PpSzSTS As Boolean = False
        Dim LM As Single = 0, TM As Single = 0
        Dim PgWt As Single = 0, PrWt As Single = 0
        Dim PgHt As Single = 0, PrHt As Single = 0

        For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
            If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
                ps = PrintDocument1.PrinterSettings.PaperSizes(I)
                PrintDocument1.DefaultPageSettings.PaperSize = ps
                e.PageSettings.PaperSize = ps
                Exit For
            End If
        Next

        With PrintDocument1.DefaultPageSettings.Margins
            .Left = 15
            .Right = 40
            .Top = 35
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
        'With PrintDocument1.DefaultPageSettings.PaperSize
        '    PrintWidth = (.Width / 2) - RMargin - LMargin
        '    PrintHeight = (.Height / 2) - TMargin - BMargin
        '    PageWidth = (.Width / 2) - RMargin
        '    PageHeight = (.Height / 2) - BMargin
        'End With
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

        NoofItems_PerPage = 28

        Erase ClArr
        Erase LnAr
        ClArr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        LnAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}

        ClArr(1) = 65 : ClArr(2) = 70 : ClArr(3) = 70 : ClArr(4) = 70 : ClArr(5) = 70 : ClArr(6) = 70 : ClArr(7) = 69 : ClArr(8) = 68 : ClArr(9) = 67 : ClArr(10) = 65
        ClArr(11) = PageWidth - (LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + ClArr(9) + ClArr(10))

        TxtHgt = 19 ' e.Graphics.MeasureString("A", pFont).Height  ' 20

        Try

            If prn_HdDt.Rows.Count > 0 Then

                If prn_HdMxIndx > 0 Then

                    Erase LnAr
                    LnAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}

                    Printing_BundlePrint_Format_1_PageHeader(PrintDocument1, e, prn_HdDt, prn_HdAr, TxtHgt, pFont, LMargin, RMargin, TM, BMargin, PageWidth, PrintWidth, prn_PageNo, NoofItems_PerPage, CurY, LnAr, ClArr, prn_HdIndx)
                    CurY = CurY - 10

                    NoofDets = 0
                    Do While prn_HdIndx < prn_HdMxIndx

                        If NoofDets >= NoofItems_PerPage Then

                            CurY = CurY + TxtHgt
                            Common_Procedures.Print_To_PrintDocument(e, "Continued....", LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + ClArr(9) + ClArr(10) + ClArr(11) - 10, CurY, 1, 0, pFont)
                            NoofDets = NoofDets + 1

                            Printing_BundlePrint_Format_1_PageFooter(e, prn_HdAr, TxtHgt, pFont, LMargin, RMargin, TM, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, NoofDets, prn_HdIndx, False)

                            e.HasMorePages = True

                            NoofDets = 0
                            prn_Count = prn_Count + 1

                            Return

                        End If

                        prn_HdIndx = prn_HdIndx + 1

                        If Val(prn_HdAr(prn_HdIndx, 4)) <> 0 Then

                            CurY = CurY + TxtHgt

                            P1fONT = New Font("Calibri", 10, FontStyle.Regular)

                            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdAr(prn_HdIndx, 1)), LMargin + 15, CurY, 0, 0, P1fONT)
                            If Val(prn_DetAr(prn_HdIndx, 1, 3)) <> 0 Then

                                Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr(prn_HdIndx, 1, 3)), LMargin + ClArr(1) + ClArr(2) - 2, CurY, 1, 0, P1fONT)

                            End If
                            If Val(prn_DetAr(prn_HdIndx, 2, 3)) <> 0 Then
                                Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr(prn_HdIndx, 2, 3)), LMargin + ClArr(1) + ClArr(2) + ClArr(3) - 2, CurY, 1, 0, P1fONT)

                            End If
                            If Val(prn_DetAr(prn_HdIndx, 3, 3)) <> 0 Then
                                Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr(prn_HdIndx, 3, 3)), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) - 2, CurY, 1, 0, P1fONT)

                            End If

                            If Val(prn_DetAr(prn_HdIndx, 4, 3)) <> 0 Then
                                Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr(prn_HdIndx, 4, 3)), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) - 2, CurY, 1, 0, P1fONT)

                            End If
                            If Val(prn_DetAr(prn_HdIndx, 5, 3)) <> 0 Then
                                Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr(prn_HdIndx, 5, 3)), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) - 2, CurY, 1, 0, P1fONT)

                            End If
                            If Val(prn_DetAr(prn_HdIndx, 6, 3)) <> 0 Then
                                Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr(prn_HdIndx, 6, 3)), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) - 2, CurY, 1, 0, P1fONT)

                            End If
                            If Val(prn_DetAr(prn_HdIndx, 7, 3)) <> 0 Then
                                Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr(prn_HdIndx, 7, 3)), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) - 2, CurY, 1, 0, P1fONT)

                            End If

                            If Val(prn_DetAr(prn_HdIndx, 8, 3)) <> 0 Then
                                Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr(prn_HdIndx, 8, 3)), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + ClArr(9) - 2, CurY, 1, 0, P1fONT)
                            End If

                            If Val(prn_HdAr(prn_HdIndx, 4)) <> 0 Then
                                Common_Procedures.Print_To_PrintDocument(e, Trim(Val(prn_HdAr(prn_HdIndx, 3))), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + ClArr(9) + ClArr(10) - 2, CurY, 1, 0, P1fONT)
                            End If

                            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdAr(prn_HdIndx, 4)), "#########0.00"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + ClArr(9) + ClArr(10) + ClArr(11) - 2, CurY, 1, 0, pFont)

                            NoofDets = NoofDets + 1

                        End If

                    Loop

                    Printing_BundlePrint_Format_1_PageFooter(e, prn_HdAr, TxtHgt, pFont, LMargin, RMargin, TM, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, NoofDets, prn_HdIndx, True)

                End If

            End If

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        e.HasMorePages = False

    End Sub


    Private Sub Printing_BundlePrint_Format_1_PageHeader(ByRef PrintDocument1 As Printing.PrintDocument, ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByRef prn_HdDt As DataTable, ByRef prn_HdAr(,) As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByRef PageNo As Integer, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single, ByVal prn_HdIndx As Integer)
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt2 As New DataTable
        Dim dt3 As New DataTable
        Dim dt4 As New DataTable
        Dim p1Font As Font
        Dim strHeight As Single
        Dim Cmp_Add As String = ""
        Dim C1 As Single, W1, W2 As Single, S1, S2 As Single
        Dim Cmp_Name, Desc As String, Cmp_Add1 As String, Cmp_Add2 As String
        Dim Cmp_PhNo As String, Cmp_TinNo As String, Cmp_CstNo As String, Cmp_EMail As String

        PageNo = PageNo + 1

        CurY = TMargin + 30

        'da2 = New SqlClient.SqlDataAdapter("select a.*, b.Ledger_Name, c.Ledger_Name, d.Company_Description as Transport_Name from ClothSales_Invoice_Head a  INNER JOIN Ledger_Head b ON b.Ledger_IdNo = a.Ledger_Idno LEFT OUTER JOIN Ledger_Head c ON c.Ledger_IdNo = a.Transport_IdNo LEFT OUTER JOIN Company_Head d ON d.Company_IdNo = a.Company_IdNo where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.ClothSales_Invoice_Code = '" & Trim(EntryCode) & "' Order by a.For_OrderBy", con)
        'da2.Fill(dt2)
        'If dt2.Rows.Count > NoofItems_PerPage Then
        '    Common_Procedures.Print_To_PrintDocument(e, "Page : " & Trim(Val(PageNo)), PageWidth - 10, CurY - TxtHgt, 1, 0, pFont)
        'End If
        'dt2.Clear()

        prn_Count = prn_Count + 1

        p1Font = New Font("Calibri", 15, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "BALE PACKING DETAILS", LMargin, CurY - TxtHgt - 5, 2, PrintWidth, p1Font)

        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(1) = CurY
        Desc = ""
        Cmp_Name = "" : Cmp_Add1 = "" : Cmp_Add2 = ""
        Cmp_PhNo = "" : Cmp_TinNo = "" : Cmp_CstNo = "" : Cmp_EMail = ""

        Desc = prn_HdDt.Rows(0).Item("Company_Description").ToString
        Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString
        Cmp_Add1 = prn_HdDt.Rows(0).Item("Company_Address1").ToString & " " & prn_HdDt.Rows(0).Item("Company_Address2").ToString
        Cmp_Add2 = prn_HdDt.Rows(0).Item("Company_Address3").ToString & " " & prn_HdDt.Rows(0).Item("Company_Address4").ToString

        If Trim(prn_HdDt.Rows(0).Item("Company_PhoneNo").ToString) <> "" Then
            Cmp_PhNo = "PHONE : " & prn_HdDt.Rows(0).Item("Company_PhoneNo").ToString
        End If
        If Trim(prn_HdDt.Rows(0).Item("Company_GSTinNo").ToString) <> "" Then
            Cmp_TinNo = "GST NO.: " & prn_HdDt.Rows(0).Item("Company_GSTinNo").ToString
        End If
        If Trim(prn_HdDt.Rows(0).Item("Company_CstNo").ToString) <> "" Then
            Cmp_CstNo = "CST NO.: " & prn_HdDt.Rows(0).Item("Company_CstNo").ToString
        End If
        If Trim(prn_HdDt.Rows(0).Item("Company_EMail").ToString) <> "" Then
            Cmp_EMail = "MAIL ID : " & prn_HdDt.Rows(0).Item("Company_EMail").ToString
        End If

        CurY = CurY + TxtHgt
        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1249" Then
            e.Graphics.DrawImage(DirectCast(Global.Textile.My.Resources.Resources.Company_Logo_Vaipav, Drawing.Image), LMargin + 20, CurY - 5, 100, 90)
        End If

        p1Font = New Font("Calibri", 18, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Name, LMargin, CurY, 2, PrintWidth, p1Font)
        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height


        CurY = CurY + strHeight - 1
        Common_Procedures.Print_To_PrintDocument(e, Desc, LMargin, CurY, 2, PrintWidth, pFont)

        CurY = CurY + TxtHgt - 1
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Add1, LMargin, CurY, 2, PrintWidth, pFont)

        CurY = CurY + TxtHgt - 1
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Add2, LMargin, CurY, 2, PrintWidth, pFont)
        CurY = CurY + TxtHgt - 1
        Common_Procedures.Print_To_PrintDocument(e, Cmp_PhNo, LMargin, CurY, 2, PrintWidth, pFont)
        CurY = CurY + TxtHgt - 1
        Common_Procedures.Print_To_PrintDocument(e, Cmp_TinNo, LMargin + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, Cmp_EMail, LMargin, CurY, 2, PrintWidth, pFont)
        Common_Procedures.Print_To_PrintDocument(e, Cmp_CstNo, PageWidth - 10, CurY, 1, 0, pFont)

        CurY = CurY + TxtHgt + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(2) = CurY

        C1 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6)
        W1 = e.Graphics.MeasureString("INVOICE DATE  : ", pFont).Width
        S1 = e.Graphics.MeasureString("TO     :    ", pFont).Width
        W2 = e.Graphics.MeasureString("Despatch To   : ", pFont).Width
        S2 = e.Graphics.MeasureString("Sent Through  : ", pFont).Width


        CurY = CurY + 10
        p1Font = New Font("Calibri", 12, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "TO  :  " & "M/s." & prn_HdDt.Rows(0).Item("Ledger_Name").ToString, LMargin + 10, CurY, 0, 0, p1Font)
        Common_Procedures.Print_To_PrintDocument(e, "DC NO", LMargin + C1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Bale_Transfer_No").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, p1Font)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address1").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
        p1Font = New Font("Calibri", 14, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "DC DATE", LMargin + C1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Bale_Transfer_Date").ToString), "dd-MM-yyyy").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, pFont)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address2").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, "ORDER NO", LMargin + C1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Party_OrderNo").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, pFont)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address3").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)


        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address4").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
        If Trim(prn_HdDt.Rows(0).Item("Party_OrderDate").ToString) <> "" Then
            Common_Procedures.Print_To_PrintDocument(e, "ORDER DATE", LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Party_OrderDate").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, pFont)
        End If

        CurY = CurY + TxtHgt
        If Trim(prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString) <> "" Then
            Common_Procedures.Print_To_PrintDocument(e, " GST : " & prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
        End If


        CurY = CurY + TxtHgt
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        e.Graphics.DrawLine(Pens.Black, LMargin + C1, CurY, LMargin + C1, LnAr(2))

        Try

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "QUALITY", LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdAr(prn_HdMxIndx, 2), LMargin + W1 + 25, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt + 10
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(2) = CurY

            CurY = CurY + TxtHgt - 10
            Common_Procedures.Print_To_PrintDocument(e, "BALE NO", LMargin, CurY, 2, ClAr(1), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "PCS-1", LMargin + ClAr(1), CurY, 2, ClAr(2), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "PCS-2", LMargin + ClAr(1) + ClAr(2), CurY, 2, ClAr(3), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "PCS-3", LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, 2, ClAr(4), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "PCS-4", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 2, ClAr(5), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "PCS-5", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, 2, ClAr(6), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "PCS-6", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, 2, ClAr(7), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "PCS-7", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, 2, ClAr(8), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "PCS-8", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, 2, ClAr(9), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "TOT PCS", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9), CurY, 2, ClAr(10), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "METERS", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10), CurY, 2, ClAr(11), pFont)

            CurY = CurY + TxtHgt + 10
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(3) = CurY

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub Printing_BundlePrint_Format_1_PageFooter(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByRef prn_HdAr(,) As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single, ByVal NoofDets As Integer, ByVal prn_HdIndx As Integer, ByVal is_LastPage As Boolean)
        Dim I As Integer
        Dim p1Font As Font

        Try

            For I = NoofDets + 1 To NoofItems_PerPage
                CurY = CurY + TxtHgt
            Next

            CurY = CurY + TxtHgt + 5
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(5) = CurY

            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1), CurY, LMargin + ClAr(1), LnAr(2))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2), CurY, LMargin + ClAr(1) + ClAr(2), LnAr(2))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3), LnAr(2))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), LnAr(2))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(2))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), LnAr(2))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), LnAr(2))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), LnAr(2))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9), LnAr(2))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10), LnAr(2))
            e.Graphics.DrawLine(Pens.Black, PageWidth, CurY, PageWidth, LnAr(2))


            If is_LastPage = True Then

                CurY = CurY + TxtHgt - 10

                Common_Procedures.Print_To_PrintDocument(e, "TOTAL BALES", LMargin + ClAr(1) + ClAr(2) - 15, CurY, 1, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, Val(prn_TotalBales), LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, 1, 0, pFont)

                Common_Procedures.Print_To_PrintDocument(e, "TOTAL PIECES", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 15, CurY, 1, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, Val(prn_TotalPcs), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, 1, 0, pFont)

                Common_Procedures.Print_To_PrintDocument(e, "TOTAL METERS", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) - 15, CurY, 1, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_TotalMtrs), "#########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) - 2, CurY, 1, 0, pFont)


                e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2), CurY + 24, LMargin + ClAr(1) + ClAr(2), LnAr(5))
                e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY + 24, LMargin + ClAr(1) + ClAr(2) + ClAr(3), LnAr(5))
                e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY + 24, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(5))
                e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY + 24, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), LnAr(5))
                e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9), CurY + 24, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9), LnAr(5))

                CurY = CurY + TxtHgt + 5
                e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
                LnAr(6) = CurY
            End If
            CurY = CurY + TxtHgt - 10

            p1Font = New Font("Calibri", 12, FontStyle.Bold)

            Common_Procedures.Print_To_PrintDocument(e, "For " & Trim(prn_HdDt.Rows(0).Item("Company_Name").ToString), PageWidth - 15, CurY, 1, 0, p1Font)
            CurY = CurY + TxtHgt
            CurY = CurY + TxtHgt
            CurY = CurY + TxtHgt

            p1Font = New Font("Calibri", 12, FontStyle.Bold)

            Common_Procedures.Print_To_PrintDocument(e, "AUTHORISED SIGNATORY ", PageWidth - 5, CurY, 1, 0, pFont)
            CurY = CurY + TxtHgt + 10

            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            e.Graphics.DrawLine(Pens.Black, LMargin, LnAr(1), LMargin, CurY)
            e.Graphics.DrawLine(Pens.Black, PageWidth, LnAr(1), PageWidth, CurY)


        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub Send_SMS()
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

            Led_IdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_TransferGodownTo.Text)

            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1267" Then '---- BRT TEXTTILES (SOMANUR)
                PhNo = "9366635141,7373532551,9344415141,9366655141"

            Else
                PhNo = Common_Procedures.get_FieldValue(con, "Ledger_Head", "MobileNo_Frsms", "(Ledger_IdNo = " & Str(Val(Led_IdNo)) & ")")

                If Trim(AgPNo) <> "" Then
                    PhNo = Trim(PhNo) & IIf(Trim(PhNo) <> "", ",", "") & Trim(AgPNo)
                End If

            End If

            smstxt = "Dc No : " & Trim(lbl_RefNo.Text) & Chr(13)
            smstxt = smstxt & " Date : " & Trim(msk_date.Text) & Chr(13)
            smstxt = smstxt & " Party : " & Trim(cbo_TransferGodownTo.Text) & Chr(13)

            If dgv_Details.RowCount > 0 Then
                smstxt = smstxt & " Quality : " & Trim(dgv_Details.Rows(0).Cells(1).Value) & Chr(13)
            End If
            If dgv_Details_Total.RowCount > 0 Then
                smstxt = smstxt & " No.Of Bales : " & Val((dgv_Details_Total.Rows(0).Cells(4).Value)) & Chr(13)
                BlNos = ""
                For i = 0 To dgv_Details.Rows.Count - 1
                    If Val(dgv_Details.Rows(i).Cells(7).Value()) <> 0 Then
                        BlNos = BlNos & IIf(Trim(BlNos) <> "", ", ", "") & Trim(dgv_Details.Rows(i).Cells(4).Value)
                    End If
                Next
                smstxt = smstxt & " Bales No.s : " & Trim(BlNos) & Chr(13)
                smstxt = smstxt & " Meters : " & Val(dgv_Details_Total.Rows(0).Cells(7).Value()) & Chr(13)
            End If


            smstxt = smstxt & " " & Chr(13)
            smstxt = smstxt & " Thanks! " & Chr(13)
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

    Private Sub btn_SMS_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_SMS.Click
        Send_SMS()
    End Sub

    Private Sub cbo_TransferGodownFrom_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_TransferGodownFrom.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_Head", "Ledger_Name", "(Ledger_Type = 'GODOWN' or Ledger_Type = 'JOBWORKER' or Show_In_All_Entry = 1)", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_TransferGodownFrom_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_TransferGodownFrom.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_TransferGodownFrom, cbo_TransferGodownTo, txt_LrNo, "Ledger_Head", "Ledger_Name", "(Ledger_Type = 'GODOWN' or Ledger_Type = 'JOBWORKER' or Show_In_All_Entry = 1)", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_TransferGodownFrom_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_TransferGodownFrom.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_TransferGodownFrom, txt_LrNo, "Ledger_Head", "Ledger_Name", "(Ledger_Type = 'GODOWN' or Ledger_Type = 'JOBWORKER' or Show_In_All_Entry = 1)", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_TransferGodownFrom_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_TransferGodownFrom.KeyUp
        If e.Control = False And e.KeyCode = 17 Then
            Common_Procedures.MDI_LedType = "GODOWN"
            Dim f As New Ledger_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_TransferGodownFrom.Name
            Common_Procedures.Master_Return.Master_Type = ""
            Common_Procedures.Master_Return.Return_Value = ""

            f.MdiParent = MDIParent1
            f.Show()
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
End Class
