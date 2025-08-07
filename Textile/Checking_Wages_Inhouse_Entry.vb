Public Class Checking_Wages_Inhouse_Entry
    Implements Interface_MDIActions

    Private con As New SqlClient.SqlConnection(Common_Procedures.Connection_String)
    Private FrmLdSTS As Boolean = False
    Private New_Entry As Boolean = False
    Private SaveAll_STS As Boolean = False
    Private Insert_Entry As Boolean = False
    Private Filter_Status As Boolean = False

    Private Pk_Condition As String = "ICHWA-"
    Private Pk_Condition2 As String = "PCDOF-"
    Private NoCalc_Status As Boolean = False
    Private Prec_ActCtrl As New Control
    Private vcbo_KeyDwnVal As Double
    Private vCbo_ItmNm As String
    Private LastNo As String = ""

    Private dgvDet_CboBx_ColNos_Arr As Integer() = {-100}

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


    Private Sub clear()

        NoCalc_Status = True

        New_Entry = False
        Insert_Entry = False

        Pnl_Back.Enabled = True
        pnl_Filter.Visible = False

        lbl_RefNo.Text = ""
        lbl_RefNo.ForeColor = Color.Black

        dtp_Date.Text = ""
        cbo_LoomNo.Text = ""
        cbo_Filter_PartyName.Text = ""
        cbo_grid_employee.Text = ""

        'txt_RollNo.Text = ""
        'txt_RollNo.Tag = ""

        lbl_RecCode.Text = ""


        dgv_Checking_Wages_Details.Rows.Clear()
        dgv_Checking_wages_Total.Rows.Clear()
        dgv_Checking_wages_Total.Rows.Add()


        If Filter_Status = False Then
            dtp_Filter_Fromdate.Text = ""
            dtp_Filter_ToDate.Text = ""
            cbo_Filter_PartyName.Text = ""

            cbo_Filter_PartyName.SelectedIndex = -1

            dgv_Filter_Details.Rows.Clear()

        End If

        Grid_Cell_DeSelect()



        cbo_grid_employee.Visible = False

        NoCalc_Status = False

    End Sub

    Private Sub ControlGotFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim txtbx As TextBox
        Dim combobx As ComboBox
        On Error Resume Next

        If TypeOf Me.ActiveControl Is TextBox Or TypeOf Me.ActiveControl Is ComboBox Or TypeOf Prec_ActCtrl Is Button Then
            Me.ActiveControl.BackColor = Color.Lime
            Me.ActiveControl.ForeColor = Color.Blue
        End If

        If TypeOf Me.ActiveControl Is TextBox Then
            txtbx = Me.ActiveControl
            txtbx.SelectAll()
        ElseIf TypeOf Me.ActiveControl Is ComboBox Then
            combobx = Me.ActiveControl
            combobx.SelectAll()
        End If



        If Me.ActiveControl.Name <> cbo_grid_employee.Name Then
            cbo_grid_employee.Visible = False
        End If


        If Me.ActiveControl.Name <> dgv_Checking_Wages_Details.Name Then
            Grid_DeSelect()
        End If

        Prec_ActCtrl = Me.ActiveControl

    End Sub

    Private Sub ControlLostFocus(ByVal sender As Object, ByVal e As System.EventArgs)

        On Error Resume Next

        If IsNothing(Prec_ActCtrl) = False Then
            If TypeOf Prec_ActCtrl Is TextBox Or TypeOf Prec_ActCtrl Is ComboBox Then
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

        If IsNothing(dgv_Checking_Wages_Details.CurrentCell) Then Exit Sub
        If IsNothing(dgv_Checking_wages_Total.CurrentCell) Then Exit Sub

        dgv_Checking_Wages_Details.CurrentCell.Selected = False
            dgv_Checking_wages_Total.CurrentCell.Selected = False

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

        If Not IsNothing(dgv_Filter_Details.CurrentCell) Then dgv_Filter_Details.CurrentCell.Selected = False

        If Not IsNothing(dgv_Checking_Wages_Details.CurrentCell) Then dgv_Checking_Wages_Details.CurrentCell.Selected = False
        If Not IsNothing(dgv_Checking_wages_Total.CurrentCell) Then dgv_Checking_wages_Total.CurrentCell.Selected = False
    End Sub

    Private Sub Piece_Checking_InHouse_Format1_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated

        Try

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_LoomNo.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "LOOMNO" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_LoomNo.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If


            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_grid_employee.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "EMPLOYEE" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_grid_employee.Text = Trim(Common_Procedures.Master_Return.Return_Value)
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

        ' lbl_RollNo_Heading.Text = StrConv(Common_Procedures.settings.ClothReceipt_LotNo_OR_RollNo_Text, vbProperCase)

        'If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1037" Or Common_Procedures.settings.ClothReceipt_PieceNo_Concept = "CONTINUOUS NO" Then
        '    dgv_Details.Columns(0).ReadOnly = False
        '    dgv_Details.Columns(0).DefaultCellStyle.Alignment = 0
        'Else
        '    dgv_Details.Columns(0).ReadOnly = True
        '    dgv_Details.Columns(0).DefaultCellStyle.Alignment = 2
        'End If



        con.Open()



        cbo_grid_employee.Visible = False


        dtp_Date.Text = ""

        pnl_Filter.Visible = False
        pnl_Filter.Left = (Me.Width - pnl_Filter.Width) \ 2
        pnl_Filter.Top = (Me.Height - pnl_Filter.Height) \ 2

        pnl_Print.Visible = False
        pnl_Print.Left = (Me.Width - pnl_Print.Width) \ 2
        pnl_Print.Top = (Me.Height - pnl_Print.Height) \ 2
        pnl_Print.BringToFront()


        btn_UserModification.Visible = False
        If Common_Procedures.settings.User_Modifications_Show_Status = 1 Then
            If Val(Common_Procedures.User.IdNo) = 1 Or Common_Procedures.User.Show_UserModification_Status = 1 Then
                btn_UserModification.Visible = True
            End If
        End If


        AddHandler dtp_Date.GotFocus, AddressOf ControlGotFocus

        ' AddHandler txt_RollNo.GotFocus, AddressOf ControlGotFocus

        AddHandler cbo_LoomNo.GotFocus, AddressOf ControlGotFocus

        AddHandler cbo_Filter_LoomNo.GotFocus, AddressOf ControlGotFocus

        AddHandler cbo_Filter_PartyName.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_PrintFrom.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_PrintTo.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_grid_employee.GotFocus, AddressOf ControlGotFocus


        AddHandler dtp_Date.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_LoomNo.LostFocus, AddressOf ControlLostFocus
        ' AddHandler txt_RollNo.LostFocus, AddressOf ControlLostFocus



        AddHandler cbo_Filter_PartyName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Filter_LoomNo.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_PrintFrom.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_PrintTo.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_grid_employee.LostFocus, AddressOf ControlLostFocus

        AddHandler dtp_Date.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler dtp_Filter_Fromdate.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler dtp_Filter_ToDate.KeyDown, AddressOf TextBoxControlKeyDown
        '  AddHandler txt_RollNo.KeyDown, AddressOf TextBoxControlKeyDown

        AddHandler txt_PrintFrom.KeyDown, AddressOf TextBoxControlKeyDown

        AddHandler dtp_Date.KeyPress, AddressOf TextBoxControlKeyPress
        ' AddHandler txt_PrintFrom.KeyPress, AddressOf TextBoxControlKeyPress

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
        'Dim dgv1 As New DataGridView
        'Dim FCol As Integer = 0

        'On Error Resume Next

        Dim I As Integer = 0
        Dim dgv1 As New DataGridView

        If ActiveControl.Name = dgv_Checking_Wages_Details.Name Or ActiveControl.Name = dgv_Checking_Wages_Details.Name Or TypeOf ActiveControl Is DataGridViewTextBoxEditingControl Then

            'On Error Resume Next

            dgv1 = Nothing

            If ActiveControl.Name = dgv_Checking_Wages_Details.Name Then
                dgv1 = dgv_Checking_Wages_Details


            ElseIf dgv_Checking_Wages_Details.IsCurrentRowDirty = True Then
                dgv1 = dgv_Checking_Wages_Details



            ElseIf Pnl_Back.Enabled = True Then
                dgv1 = dgv_Checking_Wages_Details

            End If

            If IsNothing(dgv1) = False Then

                With dgv1

                    If keyData = Keys.Enter Or keyData = Keys.Down Or keyData = Keys.Up Then

                        Common_Procedures.DGVGrid_Cursor_Focus(dgv1, keyData, dtp_Date, btn_save, dgvDet_CboBx_ColNos_Arr, dgtxt_Details, dtp_Date)

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


        'If ActiveControl.Name = dgv_Details.Name Or ActiveControl.Name = dgv_Production_Wages_Details.Name Or TypeOf ActiveControl Is DataGridViewTextBoxEditingControl Then

        '    dgv1 = Nothing

        '    If ActiveControl.Name = dgv_Details.Name Then
        '        dgv1 = dgv_Details

        '    ElseIf dgv_Details.IsCurrentRowDirty = True Then
        '        dgv1 = dgv_Details

        '    ElseIf ActiveControl.Name = dgv_Checking_Wages_Details.Name Then
        '        dgv1 = dgv_Checking_Wages_Details

        '    ElseIf dgv_Checking_Wages_Details.IsCurrentRowDirty = True Then
        '        dgv1 = dgv_Checking_Wages_Details

        '    ElseIf Pnl_Back.Enabled = True Then
        '        dgv1 = dgv_Details

        '    End If

        '    If IsNothing(dgv1) = False Then



        '        With dgv1

        '            If .Columns(0).ReadOnly = True Then
        '                FCol = 1
        '            Else
        '                FCol = 0
        '            End If
        '            If dgv1.Name = dgv_Details.Name Then


        '                If keyData = Keys.Enter Or keyData = Keys.Down Then

        '                    If .CurrentCell.ColumnIndex >= .ColumnCount - 3 Then

        '                        If .CurrentCell.RowIndex = .RowCount - 1 Then

        '                            'If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = Windows.Forms.DialogResult.Yes Then
        '                            '    save_record()
        '                            'Else
        '                            '    dtp_Date.Focus()
        '                            'End If
        '                            dgv_Checking_Wages_Details.Focus()
        '                            dgv_Checking_Wages_Details.CurrentCell = dgv_Checking_Wages_Details.Rows(0).Cells(1)

        '                        Else
        '                            .CurrentCell = .Rows(.CurrentCell.RowIndex + 1).Cells(FCol)

        '                        End If

        '                    Else

        '                        If .CurrentCell.RowIndex = .RowCount - 1 And .CurrentCell.ColumnIndex >= 1 And Trim(.CurrentRow.Cells(1).Value) = "" Then

        '                            'If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = Windows.Forms.DialogResult.Yes Then
        '                            '    save_record()
        '                            'Else
        '                            '    dtp_Date.Focus()
        '                            'End If
        '                            dgv_Checking_Wages_Details.Focus()
        '                            dgv_Checking_Wages_Details.CurrentCell = dgv_Checking_Wages_Details.Rows(0).Cells(1)

        '                        Else
        '                            .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 1)

        '                        End If

        '                    End If

        '                    Return True

        '                ElseIf keyData = Keys.Up Then

        '                    If .CurrentCell.ColumnIndex <= FCol Then
        '                        If .CurrentCell.RowIndex = 0 Then
        '                            If txt_Folding.Enabled Then txt_Folding.Focus() Else dtp_Date.Focus()

        '                        Else
        '                            .CurrentCell = .Rows(.CurrentCell.RowIndex - 1).Cells(.ColumnCount - 3)

        '                        End If

        '                    Else
        '                        .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex - 1)

        '                    End If

        '                    Return True

        '                Else
        '                    Return MyBase.ProcessCmdKey(msg, keyData)

        '                End If

        '            ElseIf dgv1.Name = dgv_Checking_Wages_Details.Name Then

        '                If keyData = Keys.Enter Or keyData = Keys.Down Then

        '                    If .CurrentCell.ColumnIndex >= .ColumnCount - 2 Then

        '                        If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = Windows.Forms.DialogResult.Yes Then
        '                            save_record()
        '                        Else
        '                            dtp_Date.Focus()
        '                        End If
        '                    Else
        '                        .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 1)
        '                    End If

        '                    Return True

        '                ElseIf keyData = Keys.Up Then

        '                    If .CurrentCell.ColumnIndex <= 1 Then

        '                        If dgv_Details.Rows.Count > 0 Then
        '                            dgv_Details.Focus()
        '                            dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(1)
        '                        Else
        '                            txt_Folding.Focus()
        '                        End If

        '                    Else
        '                        .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex - 1)
        '                    End If

        '                    Return True

        '                Else
        '                    Return MyBase.ProcessCmdKey(msg, keyData)

        '                End If


        '            End If


        '        End With



        '    Else

        '        Return MyBase.ProcessCmdKey(msg, keyData)

        '    End If

        'Else

        '    Return MyBase.ProcessCmdKey(msg, keyData)

        'End If



    End Function

    Private Sub move_record(ByVal no As String)
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim da3 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim dt3 As New DataTable
        Dim NewCode As String
        Dim n As Integer, i As Integer, j As Integer
        Dim SNo As Integer = 0
        Dim Lm_ID As Integer = 0
        Dim LockSTS As Boolean = False


        If Val(no) = 0 Then Exit Sub

        clear()

        NewCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(no) & "/" & Trim(Common_Procedures.FnYearCode)

        Try
            da1 = New SqlClient.SqlDataAdapter("select a.*, b.Loom_Name  from  Checking_Wages_Head a LEFT OUTER JOIN Loom_Head b ON a.Loom_IdNo = b.Loom_IdNo Where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Checking_Wages_Code = '" & Trim(NewCode) & "' ", con)
            dt1 = New DataTable
            da1.Fill(dt1)

            If dt1.Rows.Count > 0 Then

                lbl_RefNo.Text = dt1.Rows(0).Item("Checking_Wages_No").ToString
                dtp_Date.Text = dt1.Rows(0).Item("Checking_Wages_Date").ToString
                'txt_RollNo.Text = dt1.Rows(0).Item("Piece_Receipt_No").ToString
                'txt_RollNo.Tag = txt_RollNo.Text
                '  cbo_LotNo.Text = dt1.Rows(0).Item("Piece_Receipt_No").ToString
                ' lbl_RecCode.Text = dt1.Rows(0).Item("Piece_Receipt_Code").ToString
                cbo_LoomNo.Text = dt1.Rows(0).Item("Loom_Name").ToString
                lbl_UserName.Text = "USER : " & Trim(UCase(Common_Procedures.User_IdNoToName(con, Val(dt1.Rows(0).Item("User_IdNo").ToString))))

                'Lm_ID = 0
                'da1 = New SqlClient.SqlDataAdapter("select Loom_IdNo from Weaver_Cloth_Receipt_Head Where Weaver_ClothReceipt_Code = '" & Trim(lbl_RecCode.Text) & "'", con)
                'dt2 = New DataTable
                'dt2 = New DataTable
                'da1.Fill(dt2)
                'If dt2.Rows.Count > 0 Then
                '    Lm_ID = Val(dt2.Rows(0).Item("Loom_IdNo").ToString)
                'End If
                'dt2.Clear()

                'cbo_LoomNo.Text = Common_Procedures.Loom_IdNoToName(con, Val(Lm_ID))

                '  LockSTS = False

                'da2 = New SqlClient.SqlDataAdapter("Select a.*, b.Loom_Name from Weaver_ClothReceipt_Piece_Details a LEFT OUTER JOIN Loom_Head b ON a.Loom_IdNo = b.Loom_IdNo where a.Checking_Wages_Code = '" & Trim(NewCode) & "' Order by a.Piece_No", con)
                'dt2 = New DataTable
                'da2.Fill(dt2)

                'With dgv_Details

                '    .Rows.Clear()
                '    SNo = 0

                '    If dt2.Rows.Count > 0 Then

                '        For i = 0 To dt2.Rows.Count - 1

                '            n = .Rows.Add()

                '            .Rows(n).Cells(0).Value = dt2.Rows(i).Item("Piece_No").ToString
                '            .Rows(n).Cells(5).Value = ""
                '            .Rows(n).Cells(6).Value = ""

                '            If Val(dt2.Rows(i).Item("Type1_Meters").ToString) <> 0 Then
                '                .Rows(n).Cells(1).Value = Common_Procedures.ClothType.Type1
                '                .Rows(n).Cells(2).Value = Format(Val(dt2.Rows(i).Item("Type1_Meters").ToString), "########0.00")
                '                .Rows(n).Cells(6).Value = dt2.Rows(i).Item("PackingSlip_Code_Type1").ToString
                '                If Trim(.Rows(n).Cells(6).Value) = "" Then .Rows(n).Cells(6).Value = dt2.Rows(i).Item("BuyerOffer_Code_Type1").ToString
                '                If Trim(.Rows(n).Cells(6).Value) <> "" Then
                '                    .Rows(n).Cells(5).Value = "1"

                '                    LockSTS = True
                '                    For j = 0 To .ColumnCount - 1
                '                        .Rows(n).Cells(j).Style.BackColor = Color.Gainsboro
                '                        .Rows(n).Cells(j).Style.ForeColor = Color.Red
                '                    Next
                '                End If

                '            ElseIf Val(dt2.Rows(i).Item("Type2_Meters").ToString) <> 0 Then
                '                .Rows(n).Cells(1).Value = Common_Procedures.ClothType.Type2
                '                .Rows(n).Cells(2).Value = Format(Val(dt2.Rows(i).Item("Type2_Meters").ToString), "########0.00")
                '                .Rows(n).Cells(6).Value = dt2.Rows(i).Item("PackingSlip_Code_Type2").ToString
                '                If Trim(.Rows(n).Cells(6).Value) = "" Then .Rows(n).Cells(6).Value = dt2.Rows(i).Item("BuyerOffer_Code_Type2").ToString
                '                If Trim(.Rows(n).Cells(6).Value) <> "" Then
                '                    .Rows(n).Cells(5).Value = "1"
                '                    .Rows(n).Cells(1).Style.ForeColor = Color.Red
                '                    LockSTS = True
                '                    For j = 0 To .ColumnCount - 1
                '                        .Rows(n).Cells(j).Style.BackColor = Color.Gainsboro
                '                        .Rows(n).Cells(j).Style.ForeColor = Color.Red
                '                    Next
                '                End If

                '            ElseIf Val(dt2.Rows(i).Item("Type3_Meters").ToString) <> 0 Then
                '                .Rows(n).Cells(1).Value = Common_Procedures.ClothType.Type3
                '                .Rows(n).Cells(2).Value = Format(Val(dt2.Rows(i).Item("Type3_Meters").ToString), "########0.00")
                '                .Rows(n).Cells(6).Value = dt2.Rows(i).Item("PackingSlip_Code_Type3").ToString
                '                If Trim(.Rows(n).Cells(6).Value) = "" Then .Rows(n).Cells(6).Value = dt2.Rows(i).Item("BuyerOffer_Code_Type3").ToString
                '                If Trim(.Rows(n).Cells(6).Value) <> "" Then
                '                    .Rows(n).Cells(5).Value = "1"
                '                    .Rows(n).Cells(1).Style.ForeColor = Color.Red
                '                    LockSTS = True
                '                    For j = 0 To .ColumnCount - 1
                '                        .Rows(n).Cells(j).Style.BackColor = Color.Gainsboro
                '                        .Rows(n).Cells(j).Style.ForeColor = Color.Red
                '                    Next
                '                End If

                '            ElseIf Val(dt2.Rows(i).Item("Type4_Meters").ToString) <> 0 Then
                '                .Rows(n).Cells(1).Value = Common_Procedures.ClothType.Type4
                '                .Rows(n).Cells(2).Value = Format(Val(dt2.Rows(i).Item("Type4_Meters").ToString), "########0.00")
                '                .Rows(n).Cells(6).Value = dt2.Rows(i).Item("PackingSlip_Code_Type4").ToString
                '                If Trim(.Rows(n).Cells(6).Value) = "" Then .Rows(n).Cells(6).Value = dt2.Rows(i).Item("BuyerOffer_Code_Type4").ToString
                '                If Trim(.Rows(n).Cells(6).Value) <> "" Then
                '                    .Rows(n).Cells(5).Value = "1"
                '                    .Rows(n).Cells(1).Style.ForeColor = Color.Red
                '                    LockSTS = True
                '                    For j = 0 To .ColumnCount - 1
                '                        .Rows(n).Cells(j).Style.BackColor = Color.Gainsboro
                '                        .Rows(n).Cells(j).Style.ForeColor = Color.Red
                '                    Next
                '                End If

                '            ElseIf Val(dt2.Rows(i).Item("Type5_Meters").ToString) <> 0 Then
                '                .Rows(n).Cells(1).Value = Common_Procedures.ClothType.Type5
                '                .Rows(n).Cells(2).Value = Format(Val(dt2.Rows(i).Item("Type5_Meters").ToString), "########0.00")
                '                .Rows(n).Cells(6).Value = dt2.Rows(i).Item("PackingSlip_Code_Type5").ToString
                '                If Trim(.Rows(n).Cells(6).Value) = "" Then .Rows(n).Cells(6).Value = dt2.Rows(i).Item("BuyerOffer_Code_Type5").ToString
                '                If Trim(.Rows(n).Cells(6).Value) <> "" Then
                '                    .Rows(n).Cells(5).Value = "1"
                '                    .Rows(n).Cells(1).Style.ForeColor = Color.Red
                '                    LockSTS = True
                '                    For j = 0 To .ColumnCount - 1
                '                        .Rows(n).Cells(j).Style.BackColor = Color.Gainsboro
                '                        .Rows(n).Cells(j).Style.ForeColor = Color.Red
                '                    Next
                '                End If

                '            End If

                '            .Rows(n).Cells(3).Value = Val(dt2.Rows(i).Item("Weight").ToString)
                '            .Rows(n).Cells(4).Value = Format(Val(dt2.Rows(i).Item("Weight_Meter").ToString), "########0.000")

                '        Next i

                '    End If
                '    n = .Rows.Count - 1
                '    If (Trim(.Rows(n).Cells(1).Value) = "" And Val(.Rows(n).Cells(2).Value) <> 0) Or (.Rows(n).Cells(1).Value = Nothing And .Rows(n).Cells(2).Value = Nothing) Then
                '        .Rows(n).Cells(0).Value = ""
                '    End If

                'End With

                da3 = New SqlClient.SqlDataAdapter("Select a.*, b.Employee_Name from Checking_Wages_Details a inner join PayRoll_Employee_Head b On a.Employee_idno = b.Employee_idno Where Checking_Wages_Code = '" & Trim(NewCode) & "'", con)
                dt3 = New DataTable
                da3.Fill(dt3)
                With dgv_Checking_Wages_Details
                    .Rows.Clear()
                    SNo = 0

                    If dt3.Rows.Count > 0 Then

                        For i = 0 To dt3.Rows.Count - 1

                            n = .Rows.Add()

                            SNo = SNo + 1

                            .Rows(n).Cells(0).Value = SNo
                            .Rows(n).Cells(1).Value = dt3.Rows(i).Item("Employee_Name").ToString
                            .Rows(n).Cells(2).Value = Format(Val(dt3.Rows(i).Item("Wages_Meters").ToString), "########0.00")
                            .Rows(n).Cells(3).Value = Format(Val(dt3.Rows(i).Item("Wages_Rate").ToString), "########0.00")
                            .Rows(n).Cells(4).Value = Format(Val(dt3.Rows(i).Item("Wages_Amount").ToString), "########0.00")

                        Next i

                    End If
                End With

                With dgv_Checking_wages_Total
                    If .RowCount = 0 Then .Rows.Add()
                    .Rows(0).Cells(2).Value = Format(Val(dt1.Rows(0).Item("Total_Wages_Meters").ToString), "########0.00")
                    .Rows(0).Cells(4).Value = Format(Val(dt1.Rows(0).Item("Total_Wages_Amount").ToString), "########0.00")
                End With

                dt3.Clear()

                'dt2.Clear()

                'da2 = New SqlClient.SqlDataAdapter("select Weaver_Wages_Code from Weaver_Cloth_Receipt_Head Where Checking_Wages_Code = '" & Trim(NewCode) & "'", con)
                'dt2 = New DataTable
                'da2.Fill(dt2)

                'If dt2.Rows.Count > 0 Then
                '    If IsDBNull(dt2.Rows(0).Item("Weaver_Wages_Code").ToString) = False Then
                '        If Trim(dt2.Rows(0).Item("Weaver_Wages_Code").ToString) <> "" Then
                '            LockSTS = True
                '        End If
                '    End If
                'End If
                dt1.Clear()

                If LockSTS = True Then

                    'cbo_LotNo.Enabled = False
                    'cbo_LotNo.BackColor = Color.Gainsboro



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

        If dtp_Date.Visible And dtp_Date.Enabled Then dtp_Date.Focus()

    End Sub

    Public Sub delete_record() Implements Interface_MDIActions.delete_record
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim cmd As New SqlClient.SqlCommand
        Dim trans As SqlClient.SqlTransaction
        Dim NewCode As String = ""
        Dim vOrdByNo As String = ""

        vOrdByNo = Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text)

        NewCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)


        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.DeletingEntry, Common_Procedures.UR.Inhouse_Piece_Checking_Entry, New_Entry, Me, con, "Checking_Wages_Head", "Checking_Wages_Code", NewCode, "Checking_Wages_Date", "(Checking_Wages_Code = '" & Trim(NewCode) & "')") = False Then Exit Sub








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

        NewCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        'Da = New SqlClient.SqlDataAdapter("select COUNT(*) from Weaver_ClothReceipt_Piece_Details Where Checking_Wages_Code = '" & Trim(NewCode) & "' and (PackingSlip_Code_Type1 <> '' or PackingSlip_Code_Type2 <> '' or PackingSlip_Code_Type3 <> '' or PackingSlip_Code_Type4 <> '' or PackingSlip_Code_Type5 <> '' or BuyerOffer_Code_Type1 <> '' or BuyerOffer_Code_Type2 <> '' or BuyerOffer_Code_Type3 <> '' or BuyerOffer_Code_Type4 <> '' or BuyerOffer_Code_Type5 <> '')", con)
        'Dt1 = New DataTable
        'Da.Fill(Dt1)

        'If Dt1.Rows.Count > 0 Then
        '    If IsDBNull(Dt1.Rows(0)(0).ToString) = False Then
        '        If Val(Dt1.Rows(0)(0).ToString) <> 0 Then
        '            MessageBox.Show("Packing Slip prepared", "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
        '            Exit Sub
        '        End If
        '    End If
        'End If
        'Dt1.Clear()

        'Da = New SqlClient.SqlDataAdapter("select Weaver_Wages_Code from Weaver_Cloth_Receipt_Head Where Checking_Wages_Code = '" & Trim(NewCode) & "'", con)
        'Dt1 = New DataTable
        'Da.Fill(Dt1)

        'If Dt1.Rows.Count > 0 Then
        '    If IsDBNull(Dt1.Rows(0).Item("Weaver_Wages_Code").ToString) = False Then
        '        If Trim(Dt1.Rows(0).Item("Weaver_Wages_Code").ToString) <> "" Then
        '            MessageBox.Show("Weaver Wages prepared", "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
        '            Exit Sub

        '        End If
        '    End If
        'End If
        'Dt1.Clear()

        trans = con.BeginTransaction

        Try

            cmd.Connection = con
            cmd.Transaction = trans
            '   Call Common_Procedures.User_Modification_Updation(con, "HEAD", Me.Name, "DELETE", "Checking_Wages_Head", "Checking_Wages_Code", Val(lbl_Company.Tag), NewCode, lbl_RefNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, True, "", "", "Checking_Wages_Code, Company_IdNo, for_OrderBy", trans)

            '   Call Common_Procedures.User_Modification_Updation(con, "DETAILS", Me.Name, "DELETE", "Weaver_ClothReceipt_Piece_Details", "Checking_Wages_Code", Val(lbl_Company.Tag), NewCode, lbl_RefNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, True, "Piece_No,PieceNo_OrderBy,ReceiptMeters_Checking", "Sl_No", "Checking_Wages_Code, For_OrderBy, Company_IdNo, Checking_Wages_No, Checking_Wages_Date, Ledger_Idno", trans)

            '----- Less Checking Meters (Consumption)
            'cmd.CommandText = "Update Stock_SizedPavu_Processing_Details set Production_Meters = a.Production_Meters - b.BeamConsumption_Checking from Stock_SizedPavu_Processing_Details a, Weaver_Cloth_Receipt_Head b where b.Checking_Wages_Code = '" & Trim(NewCode) & "' and  b.set_code1 <> '' and b.Beam_No1 <> '' and a.set_code = b.Set_code1 and a.Beam_No = b.Beam_No1"
            'cmd.ExecuteNonQuery()
            'cmd.CommandText = "Update Stock_SizedPavu_Processing_Details set Production_Meters = a.Production_Meters - b.BeamConsumption_Checking from Stock_SizedPavu_Processing_Details a, Weaver_Cloth_Receipt_Head b where b.Checking_Wages_Code = '" & Trim(NewCode) & "' and  b.set_code2 <> '' and b.Beam_No2 <> '' and a.set_code = b.Set_code2 and a.Beam_No = b.Beam_No2"
            'cmd.ExecuteNonQuery()
            ''----- Add Doffing Meters (Consumption)
            'cmd.CommandText = "Update Stock_SizedPavu_Processing_Details set Production_Meters = a.Production_Meters + b.BeamConsumption_Receipt from Stock_SizedPavu_Processing_Details a, Weaver_Cloth_Receipt_Head b where b.Checking_Wages_Code = '" & Trim(NewCode) & "' and  b.set_code1 <> '' and b.Beam_No1 <> '' and a.set_code = b.Set_code1 and a.Beam_No = b.Beam_No1"
            'cmd.ExecuteNonQuery()
            'cmd.CommandText = "Update Stock_SizedPavu_Processing_Details set Production_Meters = a.Production_Meters + b.BeamConsumption_Receipt from Stock_SizedPavu_Processing_Details a, Weaver_Cloth_Receipt_Head b where b.Checking_Wages_Code = '" & Trim(NewCode) & "' and  b.set_code2 <> '' and b.Beam_No2 <> '' and a.set_code = b.Set_code2 and a.Beam_No = b.Beam_No2"
            'cmd.ExecuteNonQuery()

            'cmd.CommandText = "Update Stock_Yarn_Processing_Details set Particulars = 'Doff : Roll.No. ' + b.Weaver_ClothReceipt_No +  ', Cloth : ' + c.Cloth_Name + ', Meters : ' +  cast(ROUND(b.ReceiptMeters_Receipt,2) as varchar), Weight = b.ConsumedYarn_Receipt from Stock_Yarn_Processing_Details a, Weaver_Cloth_Receipt_Head b, Cloth_Head c Where b.Checking_Wages_Code = '" & Trim(NewCode) & "' and a.Reference_Code = '" & Trim(Pk_Condition2) & "' + b.Weaver_ClothReceipt_Code and b.Weaver_Wages_Code = '' and b.cloth_idno = c.cloth_idno"
            'cmd.ExecuteNonQuery()

            'cmd.CommandText = "Update Stock_Pavu_Processing_Details set Meters = b.ConsumedPavu_Receipt from Stock_Pavu_Processing_Details a, Weaver_Cloth_Receipt_Head b Where b.Checking_Wages_Code = '" & Trim(NewCode) & "' and a.Reference_Code = '" & Trim(Pk_Condition2) & "' + b.Weaver_ClothReceipt_Code and b.Weaver_Wages_Code = ''"
            'cmd.ExecuteNonQuery()

            'cmd.CommandText = "Update Stock_Cloth_Processing_Details set reference_date = (case when b.Weaver_Wages_Code <> '' then b.Weaver_Wages_Date else b.Weaver_ClothReceipt_Date end), Folding = b.Folding_Receipt, UnChecked_Meters = (case when b.Weaver_Wages_Code = '' then b.ReceiptMeters_Receipt else 0 end), Meters_Type1 = (case when b.Weaver_Wages_Code <> '' then b.Type1_Wages_Meters else 0 end), Meters_Type2 = (case when b.Weaver_Wages_Code <> '' then b.Type2_Wages_Meters else 0 end), Meters_Type3 = (case when b.Weaver_Wages_Code <> '' then b.Type3_Wages_Meters else 0 end), Meters_Type4 = (case when b.Weaver_Wages_Code <> '' then b.Type4_Wages_Meters else 0 end), Meters_Type5 = (case when b.Weaver_Wages_Code <> '' then b.Type5_Wages_Meters else 0 end) from Stock_Cloth_Processing_Details a, Weaver_Cloth_Receipt_Head b Where b.Checking_Wages_Code = '" & Trim(NewCode) & "' and a.Reference_Code = '" & Trim(Pk_Condition2) & "' + b.Weaver_ClothReceipt_Code"
            'cmd.ExecuteNonQuery()

            'cmd.CommandText = "Update Weaver_Cloth_Receipt_Head set Checking_Wages_Code = '', Weaver_Piece_Checking_Increment = Weaver_Piece_Checking_Increment - 1, Checking_Wages_Date = Null, Receipt_Meters = ReceiptMeters_Receipt, Folding = Folding_Receipt, Consumed_Yarn = ConsumedYarn_Receipt, Consumed_Pavu = ConsumedPavu_Receipt, BeamConsumption_Meters = BeamConsumption_Receipt, Folding_Checking = 0, ReceiptMeters_Checking = 0, ConsumedYarn_Checking = 0, ConsumedPavu_Checking = 0, BeamConsumption_Checking = 0, Type1_Checking_Meters = 0, Type2_Checking_Meters = 0, Type3_Checking_Meters = 0, Type4_Checking_Meters = 0, Type5_Checking_Meters = 0, Total_Checking_Meters = 0 Where Checking_Wages_Code = '" & Trim(NewCode) & "'"
            'cmd.ExecuteNonQuery()


            'cmd.CommandText = "delete from Weaver_ClothReceipt_Piece_Details where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Checking_Wages_Code = '" & Trim(NewCode) & "'"
            'cmd.ExecuteNonQuery()
            cmd.CommandText = "delete from Checking_Wages_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Checking_Wages_Code = '" & Trim(NewCode) & "'"
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

            If dtp_Date.Enabled = True And dtp_Date.Visible = True Then dtp_Date.Focus()

        End Try

    End Sub

    Public Sub filter_record() Implements Interface_MDIActions.filter_record

        If Filter_Status = False Then
            Dim Cmd As New SqlClient.SqlCommand
            Dim da As New SqlClient.SqlDataAdapter
            Dim dt1 As New DataTable
            Dim dt2 As New DataTable
            Dim dt3 As New DataTable

            da = New SqlClient.SqlDataAdapter("select Employee_Name from PayRoll_Employee_Head  order by Employee_Name", con)
            dt1 = New DataTable
            da.Fill(dt1)
            cbo_Filter_PartyName.DataSource = dt1
            cbo_Filter_PartyName.DisplayMember = "Employee_Name"

            da = New SqlClient.SqlDataAdapter("select Loom_Name from Loom_head order by Loom_Name", con)
            dt2 = New DataTable
            da.Fill(dt2)
            cbo_Filter_LoomNo.DataSource = dt2
            cbo_Filter_LoomNo.DisplayMember = "Loom_Name"

            'da = New SqlClient.SqlDataAdapter("select BeamNo_SetCode_forSelection from Stock_SizedPavu_Processing_Details order by BeamNo_SetCode_forSelection", con)
            'dt3 = New DataTable
            'da.Fill(dt3)
            'cbo_Filter_BeamNo.DataSource = dt3
            'cbo_Filter_BeamNo.DisplayMember = "BeamNo_SetCode_forSelection"

            Cmd.Connection = con

            Cmd.CommandText = "Update Stock_SizedPavu_Processing_Details set BeamNo_SetCode_forSelection = Beam_No + ' | ' + setcode_forSelection Where Beam_No <> ''"
            Cmd.ExecuteNonQuery()

            dtp_Filter_Fromdate.Text = Common_Procedures.Company_FromDate
            dtp_Filter_ToDate.Text = Common_Procedures.Company_ToDate
            cbo_Filter_PartyName.Text = ""

            cbo_Filter_PartyName.SelectedIndex = -1
            cbo_Filter_LoomNo.SelectedIndex = -1
            '  cbo_Filter_BeamNo.SelectedIndex = -1

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

            da = New SqlClient.SqlDataAdapter("select top 1 Checking_Wages_No from Checking_Wages_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Checking_Wages_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "'  Order by for_Orderby, Checking_Wages_No", con)
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

            OrdByNo = Common_Procedures.OrderBy_CodeToValue(Trim(lbl_RefNo.Text))

            da = New SqlClient.SqlDataAdapter("select top 1 Checking_Wages_No from Checking_Wages_Head where for_orderby > " & Str(Val(OrdByNo)) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and Checking_Wages_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby, Checking_Wages_No", con)
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

            OrdByNo = Common_Procedures.OrderBy_CodeToValue(Trim(lbl_RefNo.Text))

            da = New SqlClient.SqlDataAdapter("select top 1 Checking_Wages_No from Checking_Wages_Head where for_orderby < " & Str(Val(OrdByNo)) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and Checking_Wages_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "'  Order by for_Orderby desc, Checking_Wages_No desc", con)
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
            da = New SqlClient.SqlDataAdapter("select top 1 Checking_Wages_No from Checking_Wages_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Checking_Wages_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "'  Order by for_Orderby desc, Checking_Wages_No desc", con)
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

            lbl_RefNo.Text = Common_Procedures.get_MaxCode(con, "Checking_Wages_Head", "Checking_Wages_Code", "For_OrderBy", "", Val(lbl_Company.Tag), Common_Procedures.FnYearCode)

            lbl_RefNo.ForeColor = Color.Red


            dtp_Date.Text = Date.Today.ToShortDateString
            da = New SqlClient.SqlDataAdapter("select top 1 * from Checking_Wages_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Checking_Wages_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, Checking_Wages_No desc", con)
            dt1 = New DataTable
            da.Fill(dt1)
            If dt1.Rows.Count > 0 Then
                If Val(Common_Procedures.settings.PreviousEntryDate_ByDefault) = 1 Then '---- M.S Textiles (Tirupur)
                    If dt1.Rows(0).Item("Checking_Wages_Date").ToString <> "" Then dtp_Date.Text = dt1.Rows(0).Item("Checking_Wages_Date").ToString
                End If
                'If dt1.Rows(0).Item("Folding").ToString <> "" Then txt_Folding.Text = dt1.Rows(0).Item("Folding").ToString
            End If
            dt1.Clear()


            If dtp_Date.Enabled And dtp_Date.Visible Then dtp_Date.Focus()

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

            inpno = InputBox("Enter Ref No.", "FOR FINDING...")

            RecCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

            Da = New SqlClient.SqlDataAdapter("select Checking_Wages_No from Checking_Wages_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Checking_Wages_Code = '" & Trim(RecCode) & "' ", con)
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
                MessageBox.Show("Reference No. does not exists", "DOES NOT FIND...", MessageBoxButtons.OK, MessageBoxIcon.Error)

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

            inpno = InputBox("Enter New REF No.", "FOR NEW REFERENCE_NO INSERTION...")

            RecCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

            Da = New SqlClient.SqlDataAdapter("select Checking_Wages_No from Checking_Wages_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Checking_Wages_Code = '" & Trim(RecCode) & "'", con)
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
                    MessageBox.Show("Invalid REF No", "DOES NOT INSERT NEW ReferenceNo...", MessageBoxButtons.OK, MessageBoxIcon.Error)

                Else
                    new_record()
                    Insert_Entry = True
                    lbl_RefNo.Text = Trim(UCase(inpno))

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

        Dim vOrdByNo As String = ""
        Dim Em_id As Integer = 0

        vOrdByNo = Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text)
        NewCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)


        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.SavingEntry, Common_Procedures.UR.Inhouse_Piece_Checking_Entry, New_Entry, Me, con, "Checking_Wages_Head", "Checking_Wages_Code", NewCode, "Checking_Wages_Date", "(Checking_Wages_Code = '" & Trim(NewCode) & "')", "(Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Checking_Wages_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "')", "for_Orderby desc, Checking_Wages_No desc", dtp_Date.Value.Date) = False Then Exit Sub



        If Val(lbl_Company.Tag) = 0 Then
            MessageBox.Show("Invalid Company Selection", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        'If Common_Procedures.UserRight_Check(Common_Procedures.UR.Inhouse_Piece_Checking_Entry, New_Entry) = False Then Exit Sub

        If Pnl_Back.Enabled = False Then
            MessageBox.Show("Close Other Windows", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        If IsDate(dtp_Date.Text) = False Then
            MessageBox.Show("Invalid Date", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If dtp_Date.Enabled And dtp_Date.Visible Then dtp_Date.Focus()
            Exit Sub
        End If

        If Not (dtp_Date.Value.Date >= Common_Procedures.Company_FromDate And dtp_Date.Value.Date <= Common_Procedures.Company_ToDate) Then
            MessageBox.Show("Date is out of financial range", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If dtp_Date.Enabled And dtp_Date.Visible Then dtp_Date.Focus()
            Exit Sub
        End If

        'Led_ID = Common_Procedures.Ledger_AlaisNameToIdNo(con, lbl_PartyName.Text)
        'If Led_ID = 0 Then
        '    MessageBox.Show("Invalid Party Name", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
        '    If cbo_LotNo.Enabled And cbo_LotNo.Visible Then cbo_LotNo.Focus()
        '    Exit Sub
        'End If

        'Clo_ID = Common_Procedures.Cloth_NameToIdNo(con, lbl_ClothName.Text)
        'If Clo_ID = 0 Then
        '    MessageBox.Show("Invalid Cloth Name", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
        '    If cbo_LotNo.Enabled And cbo_LotNo.Visible Then cbo_LotNo.Focus()
        '    Exit Sub
        'End If

        Lm_ID = Common_Procedures.Loom_NameToIdNo(con, cbo_LoomNo.Text)
        If Val(Lm_ID) = 0 Then
            MessageBox.Show("Invalid Loom No", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If cbo_LoomNo.Enabled And cbo_LoomNo.Visible Then cbo_LoomNo.Focus()
            Exit Sub
        End If

        lbl_UserName.Text = Common_Procedures.User.IdNo


        With dgv_Checking_Wages_Details
            For i = 0 To .RowCount - 1

                If Val(.Rows(i).Cells(2).Value) <> 0 Then

                    Em_id = Common_Procedures.Employee_NameToIdNo(con, .Rows(i).Cells(1).Value)
                    If Em_id = 0 Then
                        MessageBox.Show("Invalid Employee Name", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        .Focus()
                        .CurrentCell = .Rows(0).Cells(1)
                        Exit Sub
                    End If

                End If

            Next

        End With

        Total_Calculation()

        vTot_Typ1Mtrs = 0 : vTot_Typ2Mtrs = 0 : vTot_Typ3Mtrs = 0 : vTot_Typ4Mtrs = 0 : vTot_Typ5Mtrs = 0
        'With dgv_Details
        '    For i = 0 To .RowCount - 1

        '        If Val(.Rows(i).Cells(2).Value) <> 0 Then

        '            CloTyp_ID = Common_Procedures.ClothType_NameToIdNo(con, .Rows(i).Cells(1).Value)
        '            If CloTyp_ID <> 0 Then

        '                If CloTyp_ID = 1 Then
        '                    vTot_Typ1Mtrs = vTot_Typ1Mtrs + Val(.Rows(i).Cells(2).Value)

        '                ElseIf CloTyp_ID = 2 Then
        '                    vTot_Typ2Mtrs = vTot_Typ2Mtrs + Val(.Rows(i).Cells(2).Value)

        '                ElseIf CloTyp_ID = 3 Then
        '                    vTot_Typ3Mtrs = vTot_Typ3Mtrs + Val(.Rows(i).Cells(2).Value)

        '                ElseIf CloTyp_ID = 4 Then
        '                    vTot_Typ4Mtrs = vTot_Typ4Mtrs + Val(.Rows(i).Cells(2).Value)

        '                ElseIf CloTyp_ID = 5 Then
        '                    vTot_Typ5Mtrs = vTot_Typ5Mtrs + Val(.Rows(i).Cells(2).Value)

        '                End If

        '            End If

        '        End If

        '    Next

        'End With

        vTot_ChkMtrs = 0 : vTot_Wgt = 0
        'If dgv_Details_Total.RowCount > 0 Then
        '    vTot_ChkMtrs = Val(dgv_Details_Total.Rows(0).Cells(2).Value())
        '    vTot_Wgt = Val(dgv_Details_Total.Rows(0).Cells(3).Value())
        'End If

        Dim vTot_wagMtr As Single
        Dim vTot_WagAmt As Single
        vTot_wagMtr = 0 : vTot_WagAmt = 0

        If dgv_Checking_wages_Total.RowCount > 0 Then
            vTot_wagMtr = Val(dgv_Checking_wages_Total.Rows(0).Cells(2).Value())
            vTot_WagAmt = Val(dgv_Checking_wages_Total.Rows(0).Cells(4).Value())
        End If

        'If Val(vTot_ChkMtrs) = 0 Then
        '    MessageBox.Show("Invalid Checking Meters", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
        '    If dgv_Details.Rows.Count > 0 Then
        '        dgv_Details.Focus()
        '        dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(1)
        '    Else
        '        txt_Folding.Focus()
        '    End If

        '    Exit Sub

        'End If

        'vTot_100Fld_Typ1Mtrs = Format(Val(vTot_Typ1Mtrs) * Val(txt_Folding.Text) / 100, "########0.00")
        'vTot_100Fld_Typ2Mtrs = Format(Val(vTot_Typ2Mtrs) * Val(txt_Folding.Text) / 100, "########0.00")
        'vTot_100Fld_Typ3Mtrs = Format(Val(vTot_Typ3Mtrs) * Val(txt_Folding.Text) / 100, "########0.00")
        'vTot_100Fld_Typ4Mtrs = Format(Val(vTot_Typ4Mtrs) * Val(txt_Folding.Text) / 100, "########0.00")
        'vTot_100Fld_Typ5Mtrs = Format(Val(vTot_Typ5Mtrs) * Val(txt_Folding.Text) / 100, "########0.00")
        'vTot_100Fld_ChkMtr = Format(Val(vTot_ChkMtrs) * Val(txt_Folding.Text) / 100, "########0.00")

        tr = con.BeginTransaction

        Try

            If Insert_Entry = True Or New_Entry = False Then
                NewCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            Else

                lbl_RefNo.Text = Common_Procedures.get_MaxCode(con, "Checking_Wages_Head", "Checking_Wages_Code", "For_OrderBy", "", Val(lbl_Company.Tag), Common_Procedures.FnYearCode, tr)

                NewCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            End If


            WagesCode = ""
            Wdth_Typ = ""
            vSetCD1 = ""
            vBmNo1 = ""
            vSetCD2 = ""
            vBmNo2 = ""


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

            End If
            Dt1.Clear()

            cmd.Connection = con
            cmd.Transaction = tr

            cmd.Parameters.Clear()
            cmd.Parameters.AddWithValue("@ChkDate", dtp_Date.Value.Date)
            '   cmd.Parameters.AddWithValue("@RecDate", CDate(lbl_RecDate.Text))

            If New_Entry = True Then
                cmd.CommandText = "Insert into Checking_Wages_Head (  Checking_Wages_Code,             Company_IdNo         ,      Checking_Wages_No ,                               for_OrderBy                              ,                   Checking_Wages_Date,       Loom_IdNo        ,  Total_Wages_Meters   ,          Total_Wages_Amount  , User_idno ) " & _
                                    "          Values   (    '" & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_RefNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text))) & ",   @chkDate,     " & Val(Lm_ID) & ",  " & Str(Val(vTot_wagMtr)) & " , " & Str(Val(vTot_WagAmt)) & "  ," & Val(lbl_UserName.Text) & ") "
                cmd.ExecuteNonQuery()


            Else
                'Call Common_Procedures.User_Modification_Updation(con, "HEAD", Me.Name, "OLD", "Checking_Wages_Head", "Checking_Wages_Code", Val(lbl_Company.Tag), NewCode, lbl_RefNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, False, "", "", "Checking_Wages_Code, Company_IdNo, for_OrderBy", tr)

                'Call Common_Procedures.User_Modification_Updation(con, "DETAILS", Me.Name, "OLD", "Weaver_ClothReceipt_Piece_Details", "Checking_Wages_Code", Val(lbl_Company.Tag), NewCode, lbl_RefNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, False, "Piece_No,PieceNo_OrderBy,ReceiptMeters_Checking", "Sl_No", "Checking_Wages_Code, For_OrderBy, Company_IdNo, Checking_Wages_No, Checking_Wages_Date, Ledger_Idno", tr)


                cmd.CommandText = "Update Checking_Wages_Head set Checking_Wages_Date = @ChkDate, Checking_Wages_No ='" & Trim(lbl_RefNo.Text) & "',  Loom_IdNo = " & Val(Lm_ID) & ", Total_Wages_Meters = " & Str(Val(vTot_wagMtr)) & ", Total_Wages_Amount = " & Str(Val(vTot_WagAmt)) & " , User_idno = " & Val(lbl_UserName.Text) & "  Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Checking_Wages_Code = '" & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()


                'cmd.CommandText = "Update Stock_Yarn_Processing_Details set Particulars = 'Doff : Roll.No. ' + b.Weaver_ClothReceipt_No +  ', Cloth : ' + c.Cloth_Name + ', Meters : ' + cast(ROUND(b.ReceiptMeters_Receipt,2) as varchar), Weight = b.ConsumedYarn_Receipt from Stock_Yarn_Processing_Details a, Weaver_Cloth_Receipt_Head b, Cloth_Head c Where b.Checking_Wages_Code = '" & Trim(NewCode) & "' and a.Reference_Code = '" & Trim(Pk_Condition2) & "' + b.Weaver_ClothReceipt_Code and b.Weaver_Wages_Code = '' and b.cloth_idno = c.cloth_idno"
                'cmd.ExecuteNonQuery()

                'cmd.CommandText = "Update Stock_Pavu_Processing_Details set Meters = b.ConsumedPavu_Receipt from Stock_Pavu_Processing_Details a, Weaver_Cloth_Receipt_Head b Where b.Checking_Wages_Code = '" & Trim(NewCode) & "' and a.Reference_Code = '" & Trim(Pk_Condition2) & "' + b.Weaver_ClothReceipt_Code and b.Weaver_Wages_Code = ''"
                'cmd.ExecuteNonQuery()

                'cmd.CommandText = "Update Stock_Cloth_Processing_Details set reference_date = (case when b.Weaver_Wages_Code <> '' then b.Weaver_Wages_Date else b.Weaver_ClothReceipt_Date end), Folding = b.Folding_Receipt, UnChecked_Meters = (case when b.Weaver_Wages_Code = '' then b.ReceiptMeters_Receipt else 0 end), Meters_Type1 = (case when b.Weaver_Wages_Code <> '' then b.Type1_Wages_Meters else 0 end), Meters_Type2 = (case when b.Weaver_Wages_Code <> '' then b.Type2_Wages_Meters else 0 end), Meters_Type3 = (case when b.Weaver_Wages_Code <> '' then b.Type3_Wages_Meters else 0 end), Meters_Type4 = (case when b.Weaver_Wages_Code <> '' then b.Type4_Wages_Meters else 0 end), Meters_Type5 = (case when b.Weaver_Wages_Code <> '' then b.Type5_Wages_Meters else 0 end) from Stock_Cloth_Processing_Details a, Weaver_Cloth_Receipt_Head b Where b.Checking_Wages_Code = '" & Trim(NewCode) & "' and a.Reference_Code = '" & Trim(Pk_Condition2) & "' + b.Weaver_ClothReceipt_Code"
                'cmd.ExecuteNonQuery()

                ''----- Less Checking Meters (Consumption)
                'cmd.CommandText = "Update Stock_SizedPavu_Processing_Details set Production_Meters = a.Production_Meters - b.BeamConsumption_Checking from Stock_SizedPavu_Processing_Details a, Weaver_Cloth_Receipt_Head b where b.Checking_Wages_Code = '" & Trim(NewCode) & "' and  b.set_code1 <> '' and b.Beam_No1 <> '' and a.set_code = b.Set_code1 and a.Beam_No = b.Beam_No1"
                'cmd.ExecuteNonQuery()
                'cmd.CommandText = "Update Stock_SizedPavu_Processing_Details set Production_Meters = a.Production_Meters - b.BeamConsumption_Checking from Stock_SizedPavu_Processing_Details a, Weaver_Cloth_Receipt_Head b where b.Checking_Wages_Code = '" & Trim(NewCode) & "' and  b.set_code2 <> '' and b.Beam_No2 <> '' and a.set_code = b.Set_code2 and a.Beam_No = b.Beam_No2"
                'cmd.ExecuteNonQuery()
                ''----- Add Doffing Meters (Consumption)
                'cmd.CommandText = "Update Stock_SizedPavu_Processing_Details set Production_Meters = a.Production_Meters + b.BeamConsumption_Receipt from Stock_SizedPavu_Processing_Details a, Weaver_Cloth_Receipt_Head b where b.Checking_Wages_Code = '" & Trim(NewCode) & "' and  b.set_code1 <> '' and b.Beam_No1 <> '' and a.set_code = b.Set_code1 and a.Beam_No = b.Beam_No1"
                'cmd.ExecuteNonQuery()
                'cmd.CommandText = "Update Stock_SizedPavu_Processing_Details set Production_Meters = a.Production_Meters + b.BeamConsumption_Receipt from Stock_SizedPavu_Processing_Details a, Weaver_Cloth_Receipt_Head b where b.Checking_Wages_Code = '" & Trim(NewCode) & "' and  b.set_code2 <> '' and b.Beam_No2 <> '' and a.set_code = b.Set_code2 and a.Beam_No = b.Beam_No2"
                'cmd.ExecuteNonQuery()

                'cmd.CommandText = "Update Weaver_Cloth_Receipt_Head set Checking_Wages_Code = '', Weaver_Piece_Checking_Increment = Weaver_Piece_Checking_Increment - 1, Checking_Wages_Date = Null, Receipt_Meters = ReceiptMeters_Receipt, Folding = Folding_Receipt, Consumed_Yarn = ConsumedYarn_Receipt, Consumed_Pavu = ConsumedPavu_Receipt, BeamConsumption_Meters = BeamConsumption_Receipt, Folding_Checking = 0, ReceiptMeters_Checking = 0, ConsumedYarn_Checking = 0, ConsumedPavu_Checking = 0, BeamConsumption_Checking = 0, Type1_Checking_Meters = 0, Type2_Checking_Meters = 0, Type3_Checking_Meters = 0, Type4_Checking_Meters = 0, Type5_Checking_Meters = 0, Total_Checking_Meters = 0 Where Checking_Wages_Code = '" & Trim(NewCode) & "'"
                'cmd.ExecuteNonQuery()

            End If
            '   Call Common_Procedures.User_Modification_Updation(con, "HEAD", Me.Name, "NEW", "Checking_Wages_Head", "Checking_Wages_Code", Val(lbl_Company.Tag), NewCode, lbl_RefNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, False, "", "", "Checking_Wages_Code, Company_IdNo, for_OrderBy", tr)


            'EntID = Trim(Pk_Condition2) & Trim(cbo_LotNo.Text)
            'Partcls = "Doff : Roll.No. " & Trim(cbo_LotNo.Text)
            'PBlNo = Trim(cbo_LotNo.Text)

            ConsYarn = Val(Common_Procedures.get_Weft_ConsumedYarn(con, Clo_ID, Val(vTot_ChkMtrs), tr))
            'ConsYarn = Val(Common_Procedures.get_Weft_ConsumedYarn(con, Clo_ID, Val(lbl_RecMtrs.Text), tr))

            ConsPavu = 0
            BeamConPavu = 0
            ConsumedPavu_Calculation(Clo_ID, Lm_ID, Val(vTot_ChkMtrs), Trim(Wdth_Typ), ConsPavu, BeamConPavu, tr)
            'ConsPavu = Val(Common_Procedures.get_Pavu_Consumption(con, Clo_ID, Lm_ID, Val(vTot_ChkMtrs), Trim(Wdth_Typ), tr))
            'ConsPavu = Val(Common_Procedures.get_Pavu_Consumption(con, Clo_ID, Lm_ID, Val(lbl_RecMtrs.Text), Trim(Wdth_Typ), tr))

            'If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1370" Then '---- AKIL IMPEX (ANNUR)
            '    ConsPavu = ConsPavu * Val(txt_Folding.Text) / 100
            'End If

            WftCnt_FldNmVal = ""
            'If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1019" Then '---- Subham Textiles
            '    WftCnt_IDNo = Common_Procedures.get_FieldValue(con, "Cloth_Head", "Cloth_WeftCount_IdNo", "(Cloth_IdNo = " & Str(Val(Clo_ID)) & ")", , tr)
            '    WftCnt_FldNmVal = ", Count_IdNo = " & Str(Val(WftCnt_IDNo))
            'End If

            Nr = 0
            '  cmd.CommandText = "Update Weaver_Cloth_Receipt_Head set Checking_Wages_Code = '" & Trim(NewCode) & "', Weaver_Piece_Checking_Increment = Weaver_Piece_Checking_Increment + 1, Checking_Wages_Date = @ChkDate,  ConsumedYarn_Checking = " & Str(Val(ConsYarn)) & ", Consumed_Yarn = " & Str(Val(ConsYarn)) & ", ConsumedPavu_Checking = " & Str(Val(ConsPavu)) & ", Consumed_Pavu = " & Str(Val(ConsPavu)) & ", BeamConsumption_Checking = " & Str(Val(BeamConPavu)) & ", BeamConsumption_Meters = " & Str(Val(BeamConPavu)) & ", Type1_Checking_Meters = " & Str(Val(vTot_Typ1Mtrs)) & ", Type2_Checking_Meters = " & Str(Val(vTot_Typ2Mtrs)) & ", Type3_Checking_Meters = " & Str(Val(vTot_Typ3Mtrs)) & ", Type4_Checking_Meters = " & Str(Val(vTot_Typ4Mtrs)) & ", Type5_Checking_Meters = " & Str(Val(vTot_Typ5Mtrs)) & ", Total_Checking_Meters = " & Str(Val(vTot_ChkMtrs)) & " " & WftCnt_FldNmVal & "  Where Weaver_ClothReceipt_Code = '" & Trim(lbl_RecCode.Text) & "' and Loom_IdNo = " & Str(Val(Lm_ID))
            '   Nr = cmd.ExecuteNonQuery()
            'If Nr = 0 Then
            '    Throw New ApplicationException("invalid LotNo, Mismatch of LoomNo and LotNo")
            '    Exit Sub
            'End If


            '----- Less Doffing Meters (Consumption)
            cmd.CommandText = "Update Stock_SizedPavu_Processing_Details set Production_Meters = a.Production_Meters - b.BeamConsumption_Receipt from Stock_SizedPavu_Processing_Details a, Weaver_Cloth_Receipt_Head b where b.Weaver_ClothReceipt_Code = '" & Trim(lbl_RecCode.Text) & "' and b.set_code1 <> '' and b.Beam_No1 <> '' and a.set_code = b.Set_code1 and a.Beam_No = b.Beam_No1"
            cmd.ExecuteNonQuery()
            cmd.CommandText = "Update Stock_SizedPavu_Processing_Details set Production_Meters = a.Production_Meters - b.BeamConsumption_Receipt from Stock_SizedPavu_Processing_Details a, Weaver_Cloth_Receipt_Head b where b.Weaver_ClothReceipt_Code = '" & Trim(lbl_RecCode.Text) & "' and b.set_code2 <> '' and b.Beam_No2 <> '' and a.set_code = b.Set_code2 and a.Beam_No = b.Beam_No2"
            cmd.ExecuteNonQuery()

            '----- Less Doffing Meters (Consumption)
            cmd.CommandText = "Update Stock_SizedPavu_Processing_Details set Production_Meters = a.Production_Meters + b.BeamConsumption_Checking from Stock_SizedPavu_Processing_Details a, Weaver_Cloth_Receipt_Head b where b.Weaver_ClothReceipt_Code = '" & Trim(lbl_RecCode.Text) & "' and b.set_code1 <> '' and b.Beam_No1 <> '' and a.set_code = b.Set_code1 and a.Beam_No = b.Beam_No1"
            cmd.ExecuteNonQuery()
            cmd.CommandText = "Update Stock_SizedPavu_Processing_Details set Production_Meters = a.Production_Meters + b.BeamConsumption_Checking from Stock_SizedPavu_Processing_Details a, Weaver_Cloth_Receipt_Head b where b.Weaver_ClothReceipt_Code = '" & Trim(lbl_RecCode.Text) & "' and b.set_code2 <> '' and b.Beam_No2 <> '' and a.set_code = b.Set_code2 and a.Beam_No = b.Beam_No2"
            cmd.ExecuteNonQuery()

            'cmd.CommandText = "Delete from Weaver_ClothReceipt_Piece_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Checking_Wages_Code = '" & Trim(NewCode) & "' and Lot_Code = '" & Trim(lbl_RecCode.Text) & "' and PackingSlip_Code_Type1 = '' and PackingSlip_Code_Type2 = ''  and PackingSlip_Code_Type3 = ''  and PackingSlip_Code_Type4 = ''  and PackingSlip_Code_Type5 = '' and BuyerOffer_Code_Type1 = '' and BuyerOffer_Code_Type2 = '' and BuyerOffer_Code_Type3 = '' and BuyerOffer_Code_Type4 = '' and BuyerOffer_Code_Type5 = ''"
            'Nr = cmd.ExecuteNonQuery()

            Led_type = Common_Procedures.get_FieldValue(con, "Ledger_Head", "Ledger_Type", "(Ledger_IdNo = " & Str(Val(Led_ID)) & ")", , tr)

            StkOf_IdNo = 0
            If Trim(UCase(Led_type)) = "JOBWORKER" Then
                StkOf_IdNo = Led_ID
            Else
                StkOf_IdNo = Val(Common_Procedures.CommonLedger.Godown_Ac)
            End If

            'With dgv_Details
            '    Sno = 0

            '    For i = 0 To .RowCount - 1

            '        If Val(.Rows(i).Cells(2).Value) <> 0 Then

            '            Sno = Sno + 1

            '            CloTyp_ID = Common_Procedures.ClothType_NameToIdNo(con, .Rows(i).Cells(1).Value, tr)

            '            vBrCode_Typ1 = ""
            '            vBrCode_Typ2 = ""
            '            vBrCode_Typ3 = ""
            '            vBrCode_Typ4 = ""
            '            vBrCode_Typ5 = ""



            '            vYrCd = Microsoft.VisualBasic.Right(Trim(lbl_RecCode.Text), 5)

            '            If CloTyp_ID = 1 Then
            '                vBrCode_Typ1 = Microsoft.VisualBasic.Left(vYrCd, 2) & Trim(Val(lbl_Company.Tag)) & Trim(UCase(cbo_LotNo.Text)) & Trim(UCase((.Rows(i).Cells(0).Value))) & "1"
            '            ElseIf CloTyp_ID = 2 Then
            '                vBrCode_Typ2 = Microsoft.VisualBasic.Left(vYrCd, 2) & Trim(Val(lbl_Company.Tag)) & Trim(UCase(cbo_LotNo.Text)) & Trim(UCase((.Rows(i).Cells(0).Value))) & "2"
            '            ElseIf CloTyp_ID = 3 Then
            '                vBrCode_Typ3 = Microsoft.VisualBasic.Left(vYrCd, 2) & Trim(Val(lbl_Company.Tag)) & Trim(UCase(cbo_LotNo.Text)) & Trim(UCase((.Rows(i).Cells(0).Value))) & "3"
            '            ElseIf CloTyp_ID = 4 Then
            '                vBrCode_Typ4 = Microsoft.VisualBasic.Left(vYrCd, 2) & Trim(Val(lbl_Company.Tag)) & Trim(UCase(cbo_LotNo.Text)) & Trim(UCase((.Rows(i).Cells(0).Value))) & "4"
            '            ElseIf CloTyp_ID = 5 Then
            '                vBrCode_Typ5 = Microsoft.VisualBasic.Left(vYrCd, 2) & Trim(Val(lbl_Company.Tag)) & Trim(UCase(cbo_LotNo.Text)) & Trim(UCase((.Rows(i).Cells(0).Value))) & "5"
            '            End If

            '            Nr = 0
            '            cmd.CommandText = "Update Weaver_ClothReceipt_Piece_Details set Checking_Wages_Code = '" & Trim(NewCode) & "',  Checking_Wages_No = '" & Trim(lbl_RefNo.Text) & "', Checking_Wages_Date = @ChkDate, StockOff_IdNo = " & Str(Val(StkOf_IdNo)) & ", Ledger_IdNo = " & Str(Val(Led_ID)) & ", Loom_IdNo = " & Str(Val(Lm_ID)) & ", Folding_Receipt = " & Str(Val(txt_Folding.Text)) & ", Folding_Checking = " & Str(Val(txt_Folding.Text)) & ", Folding = " & Str(Val(txt_Folding.Text)) & ", Sl_No = " & Str(Val(Sno)) & ", PieceNo_OrderBy = " & Str(Val(Common_Procedures.OrderBy_CodeToValue(Val(.Rows(i).Cells(0).Value)))) & ", ReceiptMeters_Checking = " & Str(Val(lbl_RecMtrs.Text)) & ", Receipt_Meters = " & Str(Val(lbl_RecMtrs.Text)) & ", Type" & Trim(Val(CloTyp_ID)) & "_Meters = " & Str(Val(.Rows(i).Cells(2).Value)) & ", Total_Checking_Meters = " & Str(Val(.Rows(i).Cells(2).Value)) & ", Weight = " & Str(Val(.Rows(i).Cells(3).Value)) & ", Weight_Meter = " & Str(Val(.Rows(i).Cells(4).Value)) & " , Checked_Pcs_Barcode_Type1 = '" & Trim(vBrCode_Typ1) & "', Checked_Pcs_Barcode_Type2 = '" & Trim(vBrCode_Typ2) & "', Checked_Pcs_Barcode_Type3 = '" & Trim(vBrCode_Typ3) & "', Checked_Pcs_Barcode_Type4 = '" & Trim(vBrCode_Typ4) & "', Checked_Pcs_Barcode_Type5 = '" & Trim(vBrCode_Typ5) & "' Where Weaver_ClothReceipt_Code = '" & Trim(Pk_Condition2) & Trim(lbl_RecCode.Text) & "' and Lot_Code = '" & Trim(lbl_RecCode.Text) & "' and Piece_No = '" & Trim(.Rows(i).Cells(0).Value) & "'"
            '            Nr = cmd.ExecuteNonQuery()

            '            If Nr = 0 Then
            '                cmd.CommandText = "Insert into Weaver_ClothReceipt_Piece_Details ( Checking_Wages_Code ,             Company_IdNo         ,     Checking_Wages_No   ,  Checking_Wages_Date,               Weaver_ClothReceipt_Code                ,    Weaver_ClothReceipt_No      ,                               for_orderby                               , Weaver_ClothReceipt_Date,           Lot_Code              ,               Lot_No           ,           StockOff_IdNo     ,         Ledger_IdNo     ,           Cloth_IdNo    ,            Loom_IdNo   ,            Folding_Receipt        ,             Folding_Checking       ,             Folding               ,           Sl_No      ,                 Piece_No               ,                                PieceNo_OrderBy                                         ,            ReceiptMeters_Checking  ,                Receipt_Meters      ,   Type" & Trim(Val(CloTyp_ID)) & "_Meters,                   Total_Checking_Meters  ,                     Weight                ,                   Weight_Meter           ,   Checked_Pcs_Barcode_Type1 ,   Checked_Pcs_Barcode_Type2 ,   Checked_Pcs_Barcode_Type3 ,   Checked_Pcs_Barcode_Type4 ,   Checked_Pcs_Barcode_Type5              ) " & _
            '                                    "     Values                                 (    '" & Trim(NewCode) & "' , " & Str(Val(lbl_Company.Tag)) & ",  '" & Trim(lbl_RefNo.Text) & "',            @ChkDate        , '" & Trim(Pk_Condition2) & Trim(lbl_RecCode.Text) & "', '" & Trim(cbo_LotNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(cbo_LotNo.Text))) & ",      @RecDate           , '" & Trim(lbl_RecCode.Text) & "', '" & Trim(cbo_LotNo.Text) & "', " & Str(Val(StkOf_IdNo)) & ", " & Str(Val(Led_ID)) & ", " & Str(Val(Clo_ID)) & ", " & Str(Val(Lm_ID)) & ", " & Str(Val(txt_Folding.Text)) & ", " & Str(Val(txt_Folding.Text)) & ", " & Str(Val(txt_Folding.Text)) & ", " & Str(Val(Sno)) & ", '" & Trim(.Rows(i).Cells(0).Value) & "',  " & Str(Val(Common_Procedures.OrderBy_CodeToValue(Trim(.Rows(i).Cells(0).Value)))) & ",  " & Str(Val(lbl_RecMtrs.Text)) & ",  " & Str(Val(lbl_RecMtrs.Text)) & ", " & Str(Val(.Rows(i).Cells(2).Value)) & ", " & Str(Val(.Rows(i).Cells(2).Value)) & ", " & Str(Val(.Rows(i).Cells(3).Value)) & " , " & Str(Val(.Rows(i).Cells(4).Value)) & " , '" & Trim(vBrCode_Typ1) & "', '" & Trim(vBrCode_Typ2) & "', '" & Trim(vBrCode_Typ3) & "', '" & Trim(vBrCode_Typ4) & "', '" & Trim(vBrCode_Typ5) & "' ) "
            '                cmd.ExecuteNonQuery()
            '            End If

            '        End If

            '    Next
            '    Call Common_Procedures.User_Modification_Updation(con, "DETAILS", Me.Name, "NEW", "Weaver_ClothReceipt_Piece_Details", "Checking_Wages_Code", Val(lbl_Company.Tag), NewCode, lbl_RefNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, False, "Piece_No,PieceNo_OrderBy,ReceiptMeters_Checking", "Sl_No", "Checking_Wages_Code, For_OrderBy, Company_IdNo, Checking_Wages_No, Checking_Wages_Date, Ledger_Idno", tr)

            'End With

            cmd.CommandText = "Delete from Checking_Wages_Details where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Checking_Wages_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            With dgv_Checking_Wages_Details
                Sno = 0

                For i = 0 To .RowCount - 1
                    If Val(.Rows(i).Cells(2).Value) <> 0 Then
                        Sno = Sno + 1

                        Emp_id = Common_Procedures.Employee_NameToIdNo(con, .Rows(i).Cells(1).Value, tr)

                        If Nr = 0 Then
                            cmd.CommandText = "Insert into Checking_Wages_Details ( Checking_Wages_Code ,             Company_IdNo         ,     Checking_Wages_No   ,  Checking_Wages_Date,                                         for_orderby                               ,                               Employee_IdNo   ,  Sl_No      ,                 Wages_Meters               ,                            Wages_Rate        ,   Wages_Amount  ) " & _
                                             "     Values                                 (    '" & Trim(NewCode) & "' , " & Str(Val(lbl_Company.Tag)) & ",  '" & Trim(lbl_RefNo.Text) & "',            @ChkDate        , " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text))) & ",     " & Str(Val(Emp_id)) & ",  " & Str(Val(Sno)) & ", " & Str(Val(.Rows(i).Cells(2).Value)) & ",   " & Str(Val(.Rows(i).Cells(3).Value)) & " ,  " & Str(Val(.Rows(i).Cells(4).Value)) & ") "
                            cmd.ExecuteNonQuery()
                        End If
                    End If
                Next i

            End With

            '   YrnPartcls = Partcls & ", Cloth : " & Trim(lbl_ClothName.Text) & ", Meters : " & Str(Val(vTot_ChkMtrs))

            'If Trim(WagesCode) = "" Then

            '    WftCnt_FldNmVal = ""
            '    If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1019" Then '---- Subham Textiles
            '        WftCnt_IDNo = Common_Procedures.get_FieldValue(con, "Cloth_Head", "Cloth_WeftCount_IdNo", "(Cloth_IdNo = " & Str(Val(Clo_ID)) & ")", , tr)
            '        WftCnt_FldNmVal = ", Count_IdNo = " & Str(Val(WftCnt_IDNo))
            '    End If

            '    If Trim(UCase(Led_type)) <> "JOBWORKER" Or (Common_Procedures.settings.JobWorker_Pavu_Yarn_Stock_Posting_IN_Production = 1 And Trim(UCase(Led_type)) = "JOBWORKER") Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1192" Then

            '        Nr = 0
            '        cmd.CommandText = "Update Stock_Yarn_Processing_Details set Particulars = '" & Trim(YrnPartcls) & "', Weight = " & Str(Val(ConsYarn)) & " " & WftCnt_FldNmVal & " Where Reference_Code = '" & Trim(Pk_Condition2) & Trim(lbl_RecCode.Text) & "'"
            '        Nr = cmd.ExecuteNonQuery()
            '        If Nr = 0 Then

            '            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1192" Then
            '                Led_type = Common_Procedures.get_FieldValue(con, "Ledger_Head", "Ledger_Type", "(Ledger_IdNo = " & Str(Val(Led_ID)) & ")", , tr)

            '                Delv_ID = 0 : Rec_ID = 0
            '                If Trim(UCase(Led_type)) = "JOBWORKER" Then
            '                    Delv_ID = Led_ID
            '                    Rec_ID = 0
            '                Else
            '                    Delv_ID = 0
            '                    Rec_ID = Led_ID
            '                End If

            '                WftCnt_IDNo = Common_Procedures.get_FieldValue(con, "Weaver_Cloth_Receipt_Head", "Count_IdNo", "(Weaver_ClothReceipt_Code = '" & Trim(lbl_RecCode.Text) & "' and Loom_IdNo = " & Str(Val(Lm_ID)) & ")", , tr)

            '                cmd.CommandText = "Insert into Stock_Yarn_Processing_Details (           Reference_Code                             ,                 Company_IdNo     ,            Reference_No        ,                               for_OrderBy                               , Reference_Date,        DeliveryTo_Idno   ,    ReceivedFrom_Idno    ,        Entry_ID      ,            Particulars    ,       Party_Bill_No  , Sl_No,          Count_IdNo          , Yarn_Type, Mill_IdNo, Bags, Cones,              Weight        ) " & _
            '                                    "          Values                        ('" & Trim(Pk_Condition2) & Trim(lbl_RecCode.Text) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(cbo_LotNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(cbo_LotNo.Text))) & ",     @ChkDate  , " & Str(Val(Delv_ID)) & ", " & Str(Val(Rec_ID)) & ", '" & Trim(EntID) & "', '" & Trim(YrnPartcls) & "', '" & Trim(PBlNo) & "',    1 , " & Str(Val(WftCnt_IDNo)) & ",    'MILL',     0    ,    0 ,   0 , " & Str(Val(ConsYarn)) & " ) "
            '                cmd.ExecuteNonQuery()

            '            End If

            '        End If

            '        Nr = 0
            '        cmd.CommandText = "Update Stock_Pavu_Processing_Details set Meters = " & Str(Val(ConsPavu)) & " Where Reference_Code = '" & Trim(Pk_Condition2) & Trim(lbl_RecCode.Text) & "'"
            '        Nr = cmd.ExecuteNonQuery()
            '        If Nr = 0 Then

            '            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1192" Then

            '                Led_type = Common_Procedures.get_FieldValue(con, "Ledger_Head", "Ledger_Type", "(Ledger_IdNo = " & Str(Val(Led_ID)) & ")", , tr)

            '                Delv_ID = 0 : Rec_ID = 0
            '                If Trim(UCase(Led_type)) = "JOBWORKER" Then
            '                    Delv_ID = Led_ID
            '                    Rec_ID = 0
            '                Else
            '                    Delv_ID = 0
            '                    Rec_ID = Led_ID
            '                End If

            '                EdsCnt_IDNo = Common_Procedures.get_FieldValue(con, "Weaver_Cloth_Receipt_Head", "EndsCount_Idno", "(Weaver_ClothReceipt_Code = '" & Trim(lbl_RecCode.Text) & "' and Loom_IdNo = " & Str(Val(Lm_ID)) & ")", , tr)

            '                cmd.CommandText = "Insert into Stock_Pavu_Processing_Details (                Reference_Code              ,                 Company_IdNo     ,              Reference_No      ,                               for_OrderBy                               , Reference_Date,        DeliveryTo_Idno    ,    ReceivedFrom_Idno   ,      Cloth_Idno         ,         Entry_ID     ,     Party_Bill_No    ,       Particulars      ,  Sl_No,        EndsCount_IdNo        , Sized_Beam,          Meters            ) " & _
            '                                    "          Values                        ('" & Trim(Pk_Condition2) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(cbo_LotNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(cbo_LotNo.Text))) & ",   @ChkDate    , " & Str(Val(Delv_ID)) & ", " & Str(Val(Rec_ID)) & ", " & Str(Val(Clo_ID)) & ", '" & Trim(EntID) & "', '" & Trim(PBlNo) & "', '" & Trim(Partcls) & "',    1  , " & Str(Val(EdsCnt_IDNo)) & ",      0    , " & Str(Val(ConsPavu)) & " )"
            '                cmd.ExecuteNonQuery()

            '                'cmd.CommandText = "Insert into Stock_Yarn_Processing_Details (           Reference_Code                             ,                 Company_IdNo     ,            Reference_No        ,                               for_OrderBy                               , Reference_Date,        DeliveryTo_Idno   ,    ReceivedFrom_Idno    ,        Entry_ID      ,            Particulars    ,       Party_Bill_No  , Sl_No,          Count_IdNo          , Yarn_Type, Mill_IdNo, Bags, Cones,              Weight        ) " & _
            '                '                    "          Values                        ('" & Trim(Pk_Condition2) & Trim(lbl_RecCode.Text) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(txt_RollNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(txt_RollNo.Text))) & ",     @ChkDate  , " & Str(Val(Delv_ID)) & ", " & Str(Val(Rec_ID)) & ", '" & Trim(EntID) & "', '" & Trim(YrnPartcls) & "', '" & Trim(PBlNo) & "',    1 , " & Str(Val(WftCnt_IDNo)) & ",    'MILL',     0    ,    0 ,   0 , " & Str(Val(ConsYarn)) & " ) "
            '                'cmd.ExecuteNonQuery()

            '            End If

            '        End If

            '    End If

            'End If

            If Val(vTot_Typ1Mtrs) <> 0 Or Val(vTot_Typ2Mtrs) <> 0 Or Val(vTot_Typ3Mtrs) <> 0 Or Val(vTot_Typ4Mtrs) <> 0 Or Val(vTot_Typ5Mtrs) <> 0 Then
                cmd.CommandText = "Update Stock_Cloth_Processing_Details set reference_date = @ChkDate,  StockOff_IdNo = " & Str(Val(StkOf_IdNo)) & ",  UnChecked_Meters = 0, Meters_Type1 = " & Str(Val(vTot_Typ1Mtrs)) & ", Meters_Type2 = " & Str(Val(vTot_Typ2Mtrs)) & ", Meters_Type3 = " & Str(Val(vTot_Typ3Mtrs)) & ", Meters_Type4 = " & Str(Val(vTot_Typ4Mtrs)) & ", Meters_Type5 = " & Str(Val(vTot_Typ5Mtrs)) & " Where Reference_Code = '" & Trim(Pk_Condition2) & Trim(lbl_RecCode.Text) & "'"
                cmd.ExecuteNonQuery()
            End If

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

            tr.Commit()

            Dt1.Dispose()
            Da.Dispose()

            If SaveAll_STS <> True Then
                MessageBox.Show("Saved Sucessfully!!!", "FOR SAVING...", MessageBoxButtons.OK, MessageBoxIcon.Information)
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
            MessageBox.Show(ex.Message, "FOR SAVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally
            If dtp_Date.Enabled And dtp_Date.Visible Then dtp_Date.Focus()

        End Try

    End Sub



    Private Sub Total_Calculation()
        Dim Sno As Integer
        Dim TotMtrs As String = 0, TotWgt As String = 0, TtMtrs_100Fld As String = 0
        Dim FldPerc As String = 0

        Sno = -1
        TotMtrs = 0
        TotWgt = 0

        'With dgv_Details
        '    For i = 0 To .RowCount - 1
        '        Sno = Sno + 1

        '        '.Rows(i).Cells(0).Value = Chr(65 + Sno)
        '        '.Rows(i).Cells(0).Value = Sno

        '        If Trim(.Rows(i).Cells(1).Value) <> "" Or Val(.Rows(i).Cells(2).Value) <> 0 Then
        '            TotMtrs = Val(TotMtrs) + Val(.Rows(i).Cells(2).Value)
        '            TotWgt = Val(TotWgt) + Val(.Rows(i).Cells(3).Value)
        '        End If
        '    Next
        'End With

        'With dgv_Details_Total
        '    If .RowCount = 0 Then .Rows.Add()
        '    .Rows(0).Cells(2).Value = Format(Val(TotMtrs), "#########0.00")
        '    .Rows(0).Cells(3).Value = Format(Val(TotWgt), "#########0.000")
        'End With

        'If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1370" Then '---- AKIL IMPEX (ANNUR)
        '    FldPerc = Val(txt_Folding.Text)
        '    If Val(FldPerc) = 0 Then FldPerc = 100

        '    TtMtrs_100Fld = Format(Val(TotMtrs) * Val(FldPerc) / 100, "#########0.00")

        '    lbl_ExcSht.Text = Format(Val(TotMtrs) - Val(TtMtrs_100Fld), "#########0.00")


        'Else

        '    lbl_ExcSht.Text = Format(Val(TotMtrs) - Val(lbl_RecMtrs.Text), "#########0.00")

        'End If




        wages_calculation()

    End Sub

    Private Sub btn_close_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_close.Click
        Me.Close()
    End Sub

    Private Sub btn_save_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_save.Click
        save_record()
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
        Dim Led_IdNo As Integer, Del_IdNo As Integer, Cnt_IdNo As Integer
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
                Condt = "a.Checking_Wages_Date between '" & Trim(Format(dtp_Filter_Fromdate.Value, "MM/dd/yyyy")) & "' and '" & Trim(Format(dtp_Filter_ToDate.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_Filter_Fromdate.Value) = True Then
                Condt = "a.Checking_Wages_Date = '" & Trim(Format(dtp_Filter_Fromdate.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_Filter_ToDate.Value) = True Then
                Condt = "a.Checking_Wages_Date = '" & Trim(Format(dtp_Filter_ToDate.Value, "MM/dd/yyyy")) & "' "
            End If

            If Trim(cbo_Filter_PartyName.Text) <> "" Then
                Led_IdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Filter_PartyName.Text)
            End If

            If Val(Led_IdNo) <> 0 Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " (a.DeliveryTo_IdNo = " & Str(Val(Led_IdNo)) & ")"
            End If

            Lom_IdNo = 0
            If Trim(cbo_Filter_LoomNo.Text) <> "" Then
                Lom_IdNo = Common_Procedures.Loom_NameToIdNo(con, cbo_Filter_LoomNo.Text)
            End If
            If Val(Lom_IdNo) <> 0 Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " (b.Loom_Idno = " & Str(Val(Lom_IdNo)) & ")"
            End If

            StCode = "" : BmNo = ""
            'If Trim(cbo_Filter_BeamNo.Text) <> "" Then
            '    da = New SqlClient.SqlDataAdapter("select * from Stock_SizedPavu_Processing_Details where BeamNo_SetCode_forSelection = '" & Trim(cbo_Filter_BeamNo.Text) & "'", con)
            '    dt2 = New DataTable
            '    da.Fill(dt2)
            '    If dt2.Rows.Count > 0 Then
            '        StCode = dt2.Rows(0).Item("set_code").ToString
            '        BmNo = dt2.Rows(0).Item("beam_no").ToString
            '    End If

            '    If Trim(StCode) <> "" And Trim(BmNo) <> "" Then
            '        Condt = Trim(Condt) & IIf(Trim(Condt) <> "", " and ", "") & "  ( (b.Set_Code1 = '" & Trim(StCode) & "' and b.Beam_No1 = '" & Trim(BmNo) & "') or (b.Set_Code2 = '" & Trim(StCode) & "' and b.Beam_No2 = '" & Trim(BmNo) & "') ) "
            '    End If

            'End If

            'Join1 = ""
            'If Trim(cbo_Filter_BeamNo.Text) <> "" Then
            '    Condt = Trim(Condt) & IIf(Trim(Condt) <> "", " and ", "") & " tSPP.BeamNo_SetCode_forSelection = '" & Trim(cbo_Filter_BeamNo.Text) & "'"
            '    Join1 = " LEFT OUTER JOIN Stock_SizedPavu_Processing_Details tSPP ON tSPP.BeamNo_SetCode_forSelection = '" & Trim(cbo_Filter_BeamNo.Text) & "' and ( (tSPP.Set_Code = B.Set_Code1 and tSPP.Beam_No = B.Beam_No1) or (tSPP.Set_Code = B.Set_Code2 and tSPP.Beam_No = B.Beam_No2) ) "
            'End If

            da = New SqlClient.SqlDataAdapter("select a.*, b.Loom_Name  from Checking_Wages_Head a  LEFT OUTER JOIN Loom_Head b ON a.Loom_IdNo = b.Loom_IdNo Where a.company_idno =" & Str(Val(lbl_Company.Tag)) & "and a.Checking_Wages_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' " & IIf(Trim(Condt) <> "", " and ", "") & Condt & " Order by a.Checking_Wages_Date, a.for_orderby, a.Checking_Wages_No", con)
            dt2 = New DataTable
            da.Fill(dt2)

            dgv_Filter_Details.Rows.Clear()

            If dt2.Rows.Count > 0 Then

                For i = 0 To dt2.Rows.Count - 1

                    n = dgv_Filter_Details.Rows.Add()

                    dgv_Filter_Details.Rows(n).Cells(0).Value = dt2.Rows(i).Item("Checking_Wages_No").ToString
                    dgv_Filter_Details.Rows(n).Cells(1).Value = Format(Convert.ToDateTime(dt2.Rows(i).Item("Checking_Wages_Date").ToString), "dd-MM-yyyy")
                    dgv_Filter_Details.Rows(n).Cells(2).Value = dt2.Rows(i).Item("Loom_Name").ToString
                    dgv_Filter_Details.Rows(n).Cells(3).Value = Format(Val(dt2.Rows(i).Item("Total_Wages_Meters").ToString), "########0.00")

                    dgv_Filter_Details.Rows(n).Cells(4).Value = Format(Val(dt2.Rows(i).Item("Total_Wages_Amount").ToString), "########0.00")

                    cHK_Mtr = cHK_Mtr + Format(Val(dt2.Rows(i).Item("Total_Wages_Meters").ToString), "########0.00")
                    cHK_wGT = cHK_wGT + Val(dt2.Rows(i).Item("Total_Wages_Amount").ToString)

                Next i

            End If

            dt2.Clear()


            dgv_fILTER_Total.Rows.Add()
            dgv_fILTER_Total.Rows(0).Cells(2).Value = "TOTAL"
            dgv_fILTER_Total.Rows(0).Cells(3).Value = Format(Val(cHK_Mtr), "########0.00")
            dgv_fILTER_Total.Rows(0).Cells(4).Value = Format(Val(cHK_wGT), "########0.00")


        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT FILTER...", MessageBoxButtons.OK, MessageBoxIcon.Error)

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
    'Private Sub cbo_Filter_BeamNo_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs)
    '    Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Stock_SizedPavu_Processing_Details", "BeamNo_SetCode_forSelection", "", "(BeamNo_SetCode_forSelection = '')")
    'End Sub

    'Private Sub cbo_Filter_BeamNo_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs)
    '    Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_BeamNo, cbo_Filter_LoomNo, btn_Filter_Show, "Stock_SizedPavu_Processing_Details", "BeamNo_SetCode_forSelection", "", "(BeamNo_SetCode_forSelection = '')")
    'End Sub

    'Private Sub cbo_Filter_BeamNo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
    '    Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Filter_BeamNo, btn_Filter_Show, "Stock_SizedPavu_Processing_Details", "BeamNo_SetCode_forSelection", "", "(BeamNo_SetCode_forSelection = '')")
    'End Sub
    Private Sub cbo_Filter_PartyName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Filter_PartyName.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_PartyName, dtp_Filter_ToDate, cbo_Filter_LoomNo, "PayRoll_Employee_Head", "Employee_Name", "", "(Employee_idno = 0)")
    End Sub

    Private Sub cbo_Filter_PartyName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Filter_PartyName.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Filter_PartyName, cbo_Filter_LoomNo, "PayRoll_Employee_Head", "Employee_Name", " ", "(Employee_idno = 0)")
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
            '    Common_Procedures.Master_Return.Control_Name = lbl_ClothName.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If
    End Sub

    'Public Sub Get_LotDetails(ByVal LtNo As String)
    '    Dim Da As New SqlClient.SqlDataAdapter
    '    Dim Dt1 As New DataTable
    '    Dim Dt2 As New DataTable
    '    Dim LtCd As String = ""
    '    Dim ChkNo As String = ""
    '    Dim n As Integer = 0
    '    Dim ChkDate As Date
    '    Dim InsEntry As Boolean = False
    '    Dim LmID As Integer = 0

    '    If Trim(LtNo) = "" Then
    '        MessageBox.Show("Invalid Lot No", "DOES NOT SHOW LOT DETAILS...", MessageBoxButtons.OK, MessageBoxIcon.Error)
    '        If cbo_LotNo.Enabled And cbo_LotNo.Visible Then cbo_LotNo.Focus()
    '        Exit Sub
    '    End If

    '    LtCd = LtNo
    '    If Not (Trim(LtNo) Like "*/??-??") Then LtCd = LtCd & "/" & Trim(Common_Procedures.FnYearCode)
    '    LtCd = Trim(Val(lbl_Company.Tag)) & "-" & Trim(LtCd)

    '    Da = New SqlClient.SqlDataAdapter("Select Checking_Wages_No from Checking_Wages_Head where Receipt_PkCondition = '" & Trim(Pk_Condition2) & "' and Piece_Receipt_Code = '" & Trim(LtCd) & "' and Receipt_Type = 'L'", con)
    '    Dt1 = New DataTable
    '    Da.Fill(Dt1)

    '    If Dt1.Rows.Count > 0 Then
    '        Call move_record(Dt1.Rows(0).Item("Checking_Wages_No").ToString)

    '    Else

    '        LmID = Common_Procedures.Loom_NameToIdNo(con, cbo_LoomNo.Text)
    '        If Val(LmID) = 0 Then
    '            MessageBox.Show("Invalid LoomNo", "DOES NOT SHOW LOT DETAILS...", MessageBoxButtons.OK, MessageBoxIcon.Error)
    '            If cbo_LoomNo.Enabled And cbo_LoomNo.Visible Then cbo_LoomNo.Focus()
    '            Exit Sub
    '        End If

    '        InsEntry = Insert_Entry
    '        ChkDate = dtp_Date.Value
    '        ChkNo = Trim(lbl_RefNo.Text)

    '        new_record()

    '        Insert_Entry = InsEntry
    '        dtp_Date.Text = ChkDate
    '        cbo_LotNo.Text = Trim(LtNo)
    '        cbo_LoomNo.Text = Common_Procedures.Loom_IdNoToName(con, LmID)
    '        lbl_RefNo.Text = ChkNo

    '        Da = New SqlClient.SqlDataAdapter("select a.*, b.ledger_name, c.cloth_name, d.Loom_Name from Weaver_Cloth_Receipt_Head a, ledger_head b, cloth_head c, Loom_Head d where a.Weaver_ClothReceipt_Code = '" & Trim(LtCd) & "' and a.Loom_Idno = " & Str(Val(LmID)) & " and a.Receipt_Type = 'L' and a.ledger_idno = b.ledger_idno and a.cloth_idno = c.cloth_idno and a.Loom_IdNo = d.Loom_IdNo", con)
    '        Dt2 = New DataTable
    '        Da.Fill(Dt2)
    '        If Dt2.Rows.Count > 0 Then
    '            If IsDBNull(Dt2.Rows(0).Item("Checking_Wages_Date").ToString) = False Then
    '                If IsDate(Dt2.Rows(0).Item("Checking_Wages_Date").ToString) = True Then
    '                    dtp_Date.Text = Dt2.Rows(0).Item("Checking_Wages_Date").ToString
    '                End If
    '            End If

    '            lbl_RecCode.Text = Dt2.Rows(0).Item("Weaver_ClothReceipt_Code").ToString
    '            lbl_RecDate.Text = Dt2.Rows(0).Item("Weaver_ClothReceipt_Date").ToString
    '            lbl_PartyName.Text = Dt2.Rows(0).Item("ledger_name").ToString
    '            lbl_ClothName.Text = Dt2.Rows(0).Item("cloth_name").ToString
    '            cbo_LoomNo.Text = Dt2.Rows(0).Item("loom_name").ToString
    '            lbl_RecMtrs.Text = Dt2.Rows(0).Item("Receipt_Meters").ToString
    '            txt_Crimp.Text = Val(Dt2.Rows(0).Item("Crimp_Percentage").ToString)
    '            txt_Folding.Text = Val(Dt2.Rows(0).Item("Folding").ToString)
    '            txt_BarCode.Text = Val(Dt2.Rows(0).Item("Bar_Code").ToString)

    '            With dgv_Details

    '                .Rows.Clear()

    '                n = .Rows.Add()

    '                If Common_Procedures.settings.ClothReceipt_PieceNo_Concept = "1,2,3" Or Trim(UCase(Common_Procedures.settings.ClothReceipt_PieceNo_Concept)) = "CONTINUOUS NO" Then
    '                    .Rows(n).Cells(0).Value = "1"
    '                Else
    '                    .Rows(n).Cells(0).Value = "A"
    '                End If

    '                .Rows(n).Cells(1).Value = Common_Procedures.ClothType.Type1
    '                .Rows(n).Cells(2).Value = Format(Val(Dt2.Rows(0).Item("Receipt_Meters").ToString), "########0.00")
    '                .Rows(n).Cells(3).Value = ""
    '                .Rows(n).Cells(4).Value = ""
    '                .Rows(n).Cells(5).Value = ""
    '                .Rows(n).Cells(6).Value = ""


    '                n = .Rows.Count - 1
    '                If (Trim(.Rows(n).Cells(1).Value) = "" And Val(.Rows(n).Cells(2).Value) <> 0) Or (.Rows(n).Cells(1).Value = Nothing And .Rows(n).Cells(2).Value = Nothing) Then
    '                    .Rows(n).Cells(0).Value = ""
    '                End If

    '            End With

    '        Else
    '            MessageBox.Show("LotNo does not exists (or) LoomNo/LotNo does not Match", "DOES NOT SHOW LOT DETAILS...", MessageBoxButtons.OK, MessageBoxIcon.Error)
    '            If cbo_LotNo.Enabled And cbo_LotNo.Visible Then cbo_LotNo.Focus()
    '            Exit Sub

    '        End If
    '        Dt2.Clear()

    '    End If
    '    Dt1.Clear()

    '    Dt1.Dispose()
    '    Dt2.Dispose()
    '    Da.Dispose()

    'End Sub

    'Private Sub txt_RollNo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
    '    If Asc(e.KeyChar) = 13 Then
    '        If Trim(txt_RollNo.Text) <> "" Then
    '            If Trim(txt_RollNo.Text) <> Trim(txt_RollNo.Tag) Then
    '                Get_LotDetails(txt_RollNo.Text)
    '                txt_RollNo.Tag = txt_RollNo.Text
    '            End If
    '        End If
    '        If Trim(Common_Procedures.settings.CustomerCode) = "1234" Then
    '            txt_Folding.Focus()
    '        Else
    '            If txt_BarCode.Visible = True Then
    '                txt_BarCode.Focus()
    '            Else
    '                txt_Folding.Focus()
    '            End If
    '        End If
    '    End If
    'End Sub



    Private Sub ConsumedPavu_Calculation(ByVal Clo_ID As Integer, ByVal Lm_ID As Integer, ByVal CloChkMtrs As Single, ByVal Wdth_Typ As String, ByRef ConsPavu As Single, ByRef BeamConPavu As Single, ByVal tr As SqlClient.SqlTransaction)
        Dim Da1 As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim NoofBeams As Integer = 0
        Dim BmNo1 As String = ""
        Dim BmNo2 As String = ""

        ConsPavu = Val(Common_Procedures.get_Pavu_Consumption(con, Clo_ID, Lm_ID, Val(CloChkMtrs), Trim(Wdth_Typ), tr))
        ConsPavu = Format(Val(ConsPavu), "#########0.00")

        BmNo1 = ""
        BmNo2 = ""
        Da1 = New SqlClient.SqlDataAdapter("Select * from Weaver_Cloth_Receipt_Head Where Weaver_ClothReceipt_Code = '" & Trim(lbl_RecCode.Text) & "'", con)
        If IsNothing(tr) = False Then
            Da1.SelectCommand.Transaction = tr
        End If
        Dt1 = New DataTable
        Da1.Fill(Dt1)
        If Dt1.Rows.Count > 0 Then
            BmNo1 = Dt1.Rows(0).Item("Beam_No1").ToString
            BmNo2 = Dt1.Rows(0).Item("Beam_No2").ToString
        End If
        Dt1.Clear()

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
        If Trim(UCase(LastNo)) = Trim(UCase(lbl_RefNo.Text)) Then
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

        LastNo = lbl_RefNo.Text

        movefirst_record()
        Timer1.Enabled = True

    End Sub

    Private Sub cbo_LoomNo_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_LoomNo.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Loom_Head", "Loom_Name", "", "(Loom_IdNo = 0)")
    End Sub

    Private Sub cbo_LoomNo_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_LoomNo.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_LoomNo, dtp_Date, dgv_Checking_Wages_Details, "Loom_Head", "Loom_Name", "", "(Loom_IdNo = 0)")
    End Sub

    Private Sub cbo_LoomNo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_LoomNo.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_LoomNo, dgv_Checking_Wages_Details, "Loom_Head", "Loom_Name", "", "(Loom_IdNo = 0 )")
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

    Private Sub wages_calculation()
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim dt1 As New DataTable
        Dim New_Rate As Double = 0
        Dim clth_pick As Double = 0
        Dim chk_meter As Double = 0
        Dim Emp_idno As Integer = 0
        Dim Clth_idno As Integer = 0

        'Clth_idno = Common_Procedures.Cloth_NameToIdNo(con, Trim(lbl_ClothName.Text))
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

        '  chk_meter = Val(dgv_Details_Total.Rows(0).Cells(2).Value)

        '  lbl_Wages_Amount.Text = Val(clth_pick * Val(txt_Wages.Text) * chk_meter)

    End Sub

    Private Sub cbo_Filter_LoomNo_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Filter_LoomNo.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Loom_Head", "Loom_Name", "", "(Loom_IdNo = 0 )")
    End Sub

    Private Sub cbo_Filter_LoomNo_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Filter_LoomNo.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_LoomNo, cbo_Filter_PartyName, btn_Filter_Show, "Loom_Head", "Loom_Name", "", "(Loom_IdNo = 0)")
    End Sub

    Private Sub cbo_Filter_LoomNo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Filter_LoomNo.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Filter_LoomNo, btn_Filter_Show, "Loom_Head", "Loom_Name", "", "(Loom_IdNo = 0 )")
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
        Printing_BarCode_Sticker(_NewCode)
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
        txt_PrintFrom.Text = lbl_RefNo.Text
        txt_PrintTo.Text = lbl_RefNo.Text
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

            da1 = New SqlClient.SqlDataAdapter("select * from Checking_Wages_Head Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Checking_Wages_Code = '" & Trim(NewCode) & "'", con)
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



            'SQL = "select a.*, b.* ,c.* , d.Cloth_Name from Checking_Wages_Head a " & _
            '    " INNER JOIN Company_Head b ON a.Company_IdNo = b.Company_IdNo  " & _
            '    " LEFT OUTER JOIN Ledger_Head c ON c.Ledger_IdNo = a.Ledger_Idno  " & _
            '    " LEFT OUTER JOIN Cloth_Head d ON d.Cloth_IdNo = a.Cloth_Idno  " & _
            '    " where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & _
            '     " and a.Checking_Wages_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "'" & _
            '    IIf(Trim(Condt) <> "", " and ", "") & Condt & _
            '    " ORDER BY Checking_Wages_No ASC "


            'da1 = New SqlClient.SqlDataAdapter(SQL, con)
            'prn_HdDt = New DataTable
            'da1.Fill(prn_HdDt)

            SQL = "Select a.*, b.Piece_Receipt_No, d.Cloth_Name,d.CLoth_Description from Weaver_ClothReceipt_Piece_Details a " & _
                " INNER JOIN Checking_Wages_Head b on  a.Checking_Wages_Code= b.Checking_Wages_Code " & _
                 " LEFT OUTER JOIN Cloth_Head d ON d.Cloth_IdNo = a.Cloth_Idno " & _
                " where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & _
                " and a.Checking_Wages_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "'" & _
                IIf(Trim(Condt) <> "", " and ", "") & Condt & _
                " ORDER BY Checking_Wages_No ASC "


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
            f1.Entry_PkValue = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)
            f1.ShowDialog()
        End If
    End Sub

    Private Sub dgv_Production_Wages_CellEndEdit(sender As Object, e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Checking_Wages_Details.CellEndEdit
        dgv_Production_Wages_CellLeave(sender, e)
    End Sub

    Private Sub dgv_Production_Wages_Details_RowsAdded(sender As Object, e As System.Windows.Forms.DataGridViewRowsAddedEventArgs) Handles dgv_Checking_Wages_Details.RowsAdded
        Dim n As Integer

        If IsNothing(dgv_Checking_Wages_Details.CurrentCell) Then Exit Sub
        With dgv_Checking_Wages_Details
            n = .RowCount
            .Rows(n - 1).Cells(0).Value = Val(n)
        End With
    End Sub

    Private Sub dgv_Production_Wages_CellEnter(sender As Object, e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Checking_Wages_Details.CellEnter
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim Dt2 As New DataTable
        Dim Dt3 As New DataTable
        Dim rect As Rectangle
        Dim n As Integer = 0

        With dgv_Checking_Wages_Details

            If e.ColumnIndex = 1 Then

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

    Private Sub dgv_Production_Wages_CellLeave(sender As Object, e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Checking_Wages_Details.CellLeave
        With dgv_Checking_Wages_Details

            If .CurrentCell.ColumnIndex = 2 Or .CurrentCell.ColumnIndex = 3 Or .CurrentCell.ColumnIndex = 4 Then
                If Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value) <> 0 Then
                    .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = Format(Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value), "#########0.00")
                End If
            End If
        End With

    End Sub

    Private Sub dgv_Production_Wages_CellValueChanged(sender As Object, e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Checking_Wages_Details.CellValueChanged
        Try

            If IsNothing(dgv_Checking_Wages_Details.CurrentCell) Then Exit Sub
            With dgv_Checking_Wages_Details
                If .Visible Then
                    If .Rows.Count > 0 Then
                        If e.ColumnIndex = 2 Or e.ColumnIndex = 3 Then

                            If Val(.CurrentRow.Cells(2).Value) <> 0 Then
                                .CurrentRow.Cells(4).Value = Format(Val(.CurrentRow.Cells(2).Value) * Val(.CurrentRow.Cells(3).Value), "#########0.00")
                                '     Total_ProductionWages_Calculation()

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

    Private Sub dgv_Production_Wages_EditingControlShowing(sender As Object, e As System.Windows.Forms.DataGridViewEditingControlShowingEventArgs) Handles dgv_Checking_Wages_Details.EditingControlShowing
        dgtxt_WagesDetails = CType(dgv_Checking_Wages_Details.EditingControl, DataGridViewTextBoxEditingControl)
        dgtxt_WagesDetails.CharacterCasing = CharacterCasing.Upper
    End Sub

    Private Sub dgv_Production_Wages_KeyUp(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles dgv_Checking_Wages_Details.KeyUp
        Dim i As Integer
        Dim n As Integer
        Try
            If e.Control = True And UCase(Chr(e.KeyCode)) = "D" Then

                With dgv_Checking_Wages_Details
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

    Private Sub dgv_Production_Wages_LostFocus(sender As Object, e As System.EventArgs) Handles dgv_Checking_Wages_Details.LostFocus
        On Error Resume Next
        If Not IsNothing(dgv_Checking_Wages_Details.CurrentCell) Then dgv_Checking_Wages_Details.CurrentCell.Selected = False
    End Sub

    Private Sub Total_ProductionWages_Calculation()
        Dim Sno As Integer
        Dim Tot_WagAmt As Single
        Dim Tot_WagMtrs As Single


        With dgv_Checking_Wages_Details
            For i = 0 To .RowCount - 1

                Sno = Sno + 1
                .Rows(i).Cells(0).Value = Sno

                If Val(.Rows(i).Cells(2).Value) <> 0 Or Val(.Rows(i).Cells(4).Value) <> 0 Then
                    Tot_WagMtrs = Tot_WagMtrs + Val(.Rows(i).Cells(2).Value)
                    Tot_WagAmt = Tot_WagAmt + Val(.Rows(i).Cells(4).Value)
                End If

            Next
        End With

        With dgv_Checking_wages_Total
            If .RowCount = 0 Then .Rows.Add()
            .Rows(0).Cells(2).Value = Format(Val(Tot_WagMtrs), "#########0.00")
            .Rows(0).Cells(4).Value = Format(Val(Tot_WagAmt), "#########0.00")
        End With

    End Sub

    Private Sub dgtxt_WagesDetails_Enter(sender As Object, e As System.EventArgs) Handles dgtxt_WagesDetails.Enter
        Try
            dgv_Checking_Wages_Details.EditingControl.BackColor = Color.Lime
            dgv_Checking_Wages_Details.EditingControl.ForeColor = Color.Blue
            dgtxt_WagesDetails.SelectAll()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "ERROR WHILE DETAILS TEXT ENTER....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub dgtxt_WagesDetails_KeyPress(sender As Object, e As System.Windows.Forms.KeyPressEventArgs) Handles dgtxt_WagesDetails.KeyPress
        Try
            With dgv_Checking_Wages_Details
                If .Visible Then

                    If .Rows.Count > 0 Then

                        If .CurrentCell.ColumnIndex = 2 Or .CurrentCell.ColumnIndex = 3 Or .CurrentCell.ColumnIndex = 4 Then

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
            With dgv_Checking_Wages_Details
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
            With dgv_Checking_Wages_Details

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

        With dgv_Checking_Wages_Details

            If (e.KeyValue = 38 And cbo_grid_employee.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
                .Focus()

                cbo_LoomNo.Focus()

            End If
            If (e.KeyValue = 40 And cbo_grid_employee.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
                .Focus()
                .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 1)


                If .CurrentCell.RowIndex = .Rows.Count - 1 And Trim(.Rows(.CurrentCell.RowIndex).Cells.Item(1).Value) = "" Then
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


            With dgv_Checking_Wages_Details
                e.Handled = True
                If .CurrentCell.RowIndex = .RowCount - 1 And .CurrentCell.ColumnIndex >= 1 And Trim(.CurrentRow.Cells(1).Value) = "" Then
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


            Led_id = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_grid_employee.Text)

            da = New SqlClient.SqlDataAdapter("select  a.Wages_Amount from PayRoll_Employee_Head a  where a.Employee_IdNo = " & Str(Val(led_id)), con)
            dt = New DataTable
            da.Fill(dt)
            If dt.Rows.Count > 0 Then
                With dgv_Checking_Wages_Details
                    If .Rows.Count > 0 Then
                        .CurrentRow.Cells(3).Value = dt.Rows(0)("Wages_Amount").ToString
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
            If cbo_grid_employee.Visible Then

                If IsNothing(dgv_Checking_Wages_Details.CurrentCell) Then Exit Sub
                With dgv_Checking_Wages_Details
                    If Val(cbo_grid_employee.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = 1 Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_grid_employee.Text)
                    End If
                End With
            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub

End Class