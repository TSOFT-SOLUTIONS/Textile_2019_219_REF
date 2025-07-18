Public Class LoomNo_Production_Entry

    Implements Interface_MDIActions

    Private con As New SqlClient.SqlConnection(Common_Procedures.Connection_String)
    Private FrmLdSTS As Boolean = False
    Private New_Entry As Boolean = False
    Private Insert_Entry As Boolean = False
    Private Filter_Status As Boolean = False
    Private Pk_Condition As String = "LOMPR-"
    Private vcbo_KeyDwnVal As Double
    Private Prec_ActCtrl As New Control
    Private dgv_DrawNo As String = ""
    Private vCbo_ItmNm As String = ""
    Private vCloPic_STS As Boolean = False

    Private WithEvents dgtxt_Details As New DataGridViewTextBoxEditingControl

    Private prn_HdDt As New DataTable
    Private prn_DetDt As New DataTable
    Private prn_PageNo As Integer
    Private prn_DetIndx As Integer
    Private prn_DetSNo As Integer
    Public vmskOldText As String = ""
    Public vmskSelStrt As Integer = -1
    Private NoCalc_Status As Boolean = False
    Private Mov_Status As Boolean = False

    Private dgvDet_CboBx_ColNos_Arr As Integer() = {1, 6, 10, 12, 14, 16}
    Private Enum DgvCol_Details As Integer

        SL_NO                               '0
        LOOM_NO                             '1
        CLOTH_NAME                          '2 
        RPM                                 '3
        SHIFT_1_PICK_EFFICIENCY             '4
        SHIFT_1                             '5
        SHIFT_1_WARP                        '6
        SHIFT_1_WEFT                        '7
        SHIFT_1_EMPLOYEE                    '8

        SHIFT_2_PICK_EFFICIENCY             '9
        SHIFT_2                             '10
        SHIFT_2_WARP                        '11
        SHIFT_2_WEFT                        '12           
        SHIFT_2_EMPLOYEE                    '13   
        SHIFT_3                             '14            

        TOTAL_PICK_EFFICIENCY               '15
        TOTAL_METERS                        '16    
        REMARKS                             '17

        TOTAL_WARP                          '18
        TOTAL_WEFT                          '19    

        DOFF_1_SHIFT                        '20
        DOFF_1_METERS                       '21      
        DOFF_2_SHIFT                        '22  
        DOFF_2_METERS                       '23   
        DOFF_3_SHIFT                        '24  
        DOFF_3_METERS                       '25   
        DOFF_4_SHIFT                        '26           
        DOFF_4_METERS                       '27

        PARTY_NAME                          '28
        RATE_METER                          '29    
        AMOUNT                              '30





        'SL_NO                       '0
        'LOOM_NO                     '1     
        'TOTAL_METERS                      '2
        'WARP                        '3
        'WEFT                       '4
        'PICK_EFFICIENCY                  '5
        'EMPLOYEE                  '6
        'SHIFT_1                  '7
        'SHIFT_2                  '8
        'SHIFT_3                  '9
        'DOFF_1_SHIFT                  '10
        'DOFF_1_METERS      '11
        'DOFF_2_SHIFT    '12
        'DOFF_2_METERS             '13
        'DOFF_3_SHIFT                      '14
        'DOFF_3_METERS              '15
        'DOFF_4_SHIFT        '16
        'DOFF_4_METERS        '17


    End Enum

    Private Sub clear()
        pnl_Back.Enabled = True
        pnl_Filter.Visible = False

        NoCalc_Status = True
        Mov_Status = False

        New_Entry = False
        vmskOldText = ""
        vmskSelStrt = -1

        lbl_RefNo.Text = ""
        lbl_RefNo.ForeColor = Color.Black
        cbo_Shift.Text = ""
        msk_Date.Text = ""
        dtp_Date.Text = ""
        msk_Date.Enabled = False
        dtp_Date.Enabled = False
        msk_Date.Tag = msk_Date.Text

        txt_EB_Units.Text = ""
        txt_EB_Amount.Text = ""
        txt_Employee_Salary.Text = ""

        dgv_Details.Rows.Clear()

        dgv_Details_Total.Rows.Clear()
        dgv_Details_Total.Rows.Add()

        cbo_Grid_Loom.Visible = False
        'cbo_Grid_Shift_1_Employee.Visible = False

        Cbo_Grid_Doff1.Visible = False
        Cbo_Grid_Doff2.Visible = False
        Cbo_Grid_Doff3.Visible = False
        Cbo_Grid_Doff4.Visible = False

        cbo_Shift.Visible = False
        cbo_Shift.Enabled = False
        NoCalc_Status = False

    End Sub

    Private Sub ControlGotFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim txtbx As TextBox
        Dim combobx As ComboBox
        Dim msktxbx As MaskedTextBox
        On Error Resume Next

        If TypeOf Me.ActiveControl Is TextBox Or TypeOf Me.ActiveControl Is ComboBox Or TypeOf Me.ActiveControl Is MaskedTextBox Then
            Me.ActiveControl.BackColor = Color.Lime
            Me.ActiveControl.ForeColor = Color.Blue
        End If

        'If TypeOf Me.ActiveControl Is TextBox Then
        '    txtbx = Me.ActiveControl
        '    txtbx.SelectAll()
        'ElseIf TypeOf Me.ActiveControl Is ComboBox Then
        '    combobx = Me.ActiveControl
        '    combobx.SelectAll()
        'ElseIf TypeOf Me.ActiveControl Is MaskedTextBox Then
        '    msktxbx = Me.ActiveControl
        '    msktxbx.SelectionStart = 0
        'End If

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

        If Me.ActiveControl.Name <> cbo_Grid_Loom.Name Then
            cbo_Grid_Loom.Visible = False
        End If

        If Me.ActiveControl.Name <> cbo_Grid_Shift_1_Employee.Name Then
            cbo_Grid_Shift_1_Employee.Visible = False
        End If

        If Me.ActiveControl.Name <> Cbo_Grid_Doff1.Name Then
            Cbo_Grid_Doff1.Visible = False
        End If

        If Me.ActiveControl.Name <> Cbo_Grid_Doff2.Name Then
            Cbo_Grid_Doff2.Visible = False
        End If

        If Me.ActiveControl.Name <> Cbo_Grid_Doff3.Name Then
            Cbo_Grid_Doff3.Visible = False
        End If

        If Me.ActiveControl.Name <> Cbo_Grid_Doff4.Name Then
            Cbo_Grid_Doff4.Visible = False
        End If
        Grid_Cell_DeSelect()

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
    Private Sub TextBoxControlKeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs)
        On Error Resume Next
        If e.KeyValue = 38 Then SendKeys.Send("+{TAB}")
        If e.KeyValue = 40 Then SendKeys.Send("{TAB}")
    End Sub

    Private Sub TextBoxControlKeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
        On Error Resume Next
        If Asc(e.KeyChar) = 13 Then SendKeys.Send("{TAB}")
    End Sub
    Private Sub Grid_DeSelect()
        On Error Resume Next
        If Not IsNothing(dgv_Details.CurrentCell) Then dgv_Details.CurrentCell.Selected = False
        If Not IsNothing(dgv_Details_Total.CurrentCell) Then dgv_Details_Total.CurrentCell.Selected = False
    End Sub

    Private Sub Grid_Cell_DeSelect()
        On Error Resume Next
        If Not IsNothing(dgv_Details.CurrentCell) Then dgv_Details.CurrentCell.Selected = False
        If Not IsNothing(dgv_Details_Total.CurrentCell) Then dgv_Details_Total.CurrentCell.Selected = False
    End Sub

    Private Sub LoomNo_Production_Entry_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated


        Try

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Grid_Loom.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "LOOMNO" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Grid_Loom.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Grid_Shift_1_Employee.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "EMPLOYEE" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Grid_Shift_1_Employee.Text = Trim(Common_Procedures.Master_Return.Return_Value)
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

    Private Sub LoomNo_Production_Entry_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Me.Load


        Me.Text = ""

        con.Open()


        pnl_Filter.Visible = False
        pnl_Filter.Left = (Me.Width - pnl_Filter.Width) \ 2
        pnl_Filter.Top = (Me.Height - pnl_Filter.Height) \ 2
        pnl_Filter.BringToFront()


        cbo_Shift.Visible = False
        cbo_Shift.Enabled = False


        dgv_Details.Columns(DgvCol_Details.PARTY_NAME).Visible = False
        dgv_Details.Columns(DgvCol_Details.RATE_METER).Visible = False
        dgv_Details.Columns(DgvCol_Details.AMOUNT).Visible = False

        dgv_Details_Total.Columns(DgvCol_Details.PARTY_NAME).Visible = False
        dgv_Details_Total.Columns(DgvCol_Details.RATE_METER).Visible = False
        dgv_Details_Total.Columns(DgvCol_Details.AMOUNT).Visible = False

        lbl_Eb_Unit.Visible = False
        txt_EB_Units.Visible = False
        lbl_Eb_Amount.Visible = False
        txt_EB_Amount.Visible = False
        lbl_Employee_Salary.Visible = False
        txt_Employee_Salary.Visible = False



        btn_UserModification.Visible = False
        If Common_Procedures.settings.User_Modifications_Show_Status = 1 Then
            If Val(Common_Procedures.User.IdNo) = 1 Or Common_Procedures.User.Show_UserModification_Status = 1 Then
                btn_UserModification.Visible = True
            End If
        End If

        'dgv_Details.Columns(DgvCol_Details.SHIFT_1_PICK_EFFICIENCY).HeaderText = "               PICK EFF"
        'dgv_Details.Columns(DgvCol_Details.SHIFT_1_WARP).HeaderText = "WARP"
        'dgv_Details.Columns(DgvCol_Details.SHIFT_1_WEFT).HeaderText = "WEFT"
        'dgv_Details.Columns(DgvCol_Details.SHIFT_1_EMPLOYEE).HeaderText = "EMPLOYEE"

        'dgv_Details.Columns(DgvCol_Details.SHIFT_2_PICK_EFFICIENCY).HeaderText = "               PICK EFF"
        'dgv_Details.Columns(DgvCol_Details.SHIFT_2_WARP).HeaderText = "WARP"
        'dgv_Details.Columns(DgvCol_Details.SHIFT_2_WEFT).HeaderText = "WEFT"
        'dgv_Details.Columns(DgvCol_Details.SHIFT_2_EMPLOYEE).HeaderText = "EMPLOYEE"

        'dgv_Details.Columns(DgvCol_Details.TOTAL_PICK_EFFICIENCY).HeaderText = "PICK EFFICIENCY"

        'dgv_Details.Columns(DgvCol_Details.TOTAL_WARP).Visible = False
        'dgv_Details.Columns(DgvCol_Details.TOTAL_WEFT).Visible = False
        'dgv_Details.Columns(DgvCol_Details.REMARKS).Visible = False


        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1464" Then ' --- MOF

            lbl_Shift_1.Visible = False
            lbl_Shift_2.Visible = False
            Lbl_Total.Visible = False


            dgv_Details.Columns(DgvCol_Details.CLOTH_NAME).Visible = False
            dgv_Details.Columns(DgvCol_Details.RPM).Visible = False
            dgv_Details.Columns(DgvCol_Details.SHIFT_1_PICK_EFFICIENCY).Visible = False
            dgv_Details.Columns(DgvCol_Details.SHIFT_1_WARP).Visible = False
            dgv_Details.Columns(DgvCol_Details.SHIFT_1_WEFT).Visible = False
            dgv_Details.Columns(DgvCol_Details.SHIFT_1_EMPLOYEE).Visible = False
            dgv_Details.Columns(DgvCol_Details.SHIFT_2_PICK_EFFICIENCY).Visible = False
            dgv_Details.Columns(DgvCol_Details.SHIFT_2_WARP).Visible = False
            dgv_Details.Columns(DgvCol_Details.SHIFT_2_WEFT).Visible = False
            dgv_Details.Columns(DgvCol_Details.SHIFT_2_EMPLOYEE).Visible = False
            dgv_Details.Columns(DgvCol_Details.TOTAL_PICK_EFFICIENCY).Visible = False
            dgv_Details.Columns(DgvCol_Details.TOTAL_WARP).Visible = False
            dgv_Details.Columns(DgvCol_Details.TOTAL_WEFT).Visible = False
            dgv_Details.Columns(DgvCol_Details.REMARKS).Visible = False

            dgv_Details.Columns(DgvCol_Details.PARTY_NAME).Visible = False
            dgv_Details.Columns(DgvCol_Details.RATE_METER).Visible = False
            dgv_Details.Columns(DgvCol_Details.AMOUNT).Visible = False

            dgv_Details_Total.Columns(DgvCol_Details.CLOTH_NAME).Visible = False
            dgv_Details_Total.Columns(DgvCol_Details.RPM).Visible = False
            dgv_Details_Total.Columns(DgvCol_Details.SHIFT_1_PICK_EFFICIENCY).Visible = False
            dgv_Details_Total.Columns(DgvCol_Details.SHIFT_1_WARP).Visible = False
            dgv_Details_Total.Columns(DgvCol_Details.SHIFT_1_WEFT).Visible = False
            dgv_Details_Total.Columns(DgvCol_Details.SHIFT_1_EMPLOYEE).Visible = False
            dgv_Details_Total.Columns(DgvCol_Details.SHIFT_2_PICK_EFFICIENCY).Visible = False
            dgv_Details_Total.Columns(DgvCol_Details.SHIFT_2_WARP).Visible = False
            dgv_Details_Total.Columns(DgvCol_Details.SHIFT_2_WEFT).Visible = False
            dgv_Details_Total.Columns(DgvCol_Details.SHIFT_2_EMPLOYEE).Visible = False
            dgv_Details_Total.Columns(DgvCol_Details.TOTAL_PICK_EFFICIENCY).Visible = False
            dgv_Details_Total.Columns(DgvCol_Details.TOTAL_WARP).Visible = False
            dgv_Details_Total.Columns(DgvCol_Details.TOTAL_WEFT).Visible = False
            dgv_Details_Total.Columns(DgvCol_Details.REMARKS).Visible = False

            dgv_Details_Total.Columns(DgvCol_Details.PARTY_NAME).Visible = False
            dgv_Details_Total.Columns(DgvCol_Details.RATE_METER).Visible = False
            dgv_Details_Total.Columns(DgvCol_Details.AMOUNT).Visible = False



            Me.Size = New Size(877, 551)
            Me.StartPosition = FormStartPosition.CenterScreen
            dgv_Details.Size = New Size(830, 319)
            dgv_Details_Total.Size = New Size(830, 26)
            dgv_Details.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            btn_Print.Visible = False


        Else

            lbl_Shift_1.Visible = True
            lbl_Shift_2.Visible = True
            Lbl_Total.Visible = True


            dgv_Details.Columns(DgvCol_Details.SHIFT_1).HeaderText = "METER"
            dgv_Details.Columns(DgvCol_Details.SHIFT_2).HeaderText = "METER"


        End If



        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1520" Then ' --- RAINBOW COTTON FABBRIC

            dgv_Details_Total.Columns(DgvCol_Details.REMARKS).Visible = False
            dgv_Details_Total.Columns(DgvCol_Details.PARTY_NAME).Visible = False
            dgv_Details_Total.Columns(DgvCol_Details.RATE_METER).Visible = False

            dgv_Details_Total.Columns(DgvCol_Details.AMOUNT).Visible = True

            dgv_Details.Columns(DgvCol_Details.PARTY_NAME).Visible = True
            dgv_Details.Columns(DgvCol_Details.RATE_METER).Visible = True
            dgv_Details.Columns(DgvCol_Details.AMOUNT).Visible = True

            dgv_Details.Columns(DgvCol_Details.REMARKS).Width = 90
            dgv_Details.Columns(DgvCol_Details.REMARKS).Width = 90
            dgv_Details.Columns(DgvCol_Details.REMARKS).Width = 90
            dgv_Details.Columns(DgvCol_Details.REMARKS).Width = 90





            dgv_Details.ScrollBars = ScrollBars.Both
            '  dgv_Details.FirstDisplayedScrollingColumnIndex = DgvCol_Details.SHIFT_2_EMPLOYEE

            lbl_Eb_Unit.Visible = True
            txt_EB_Units.Visible = True
            lbl_Eb_Amount.Visible = True
            txt_EB_Amount.Visible = True
            lbl_Employee_Salary.Visible = True
            txt_Employee_Salary.Visible = True

        End If


        AddHandler msk_Date.GotFocus, AddressOf ControlGotFocus
        AddHandler dtp_Date.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Grid_Loom.GotFocus, AddressOf ControlGotFocus
        AddHandler dtp_Filter_Fromdate.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Shift.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Filter_Loom.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Filter_Shift.GotFocus, AddressOf ControlGotFocus
        AddHandler dtp_Filter_ToDate.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Grid_Shift_1_Employee.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_Save.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_Cancel.GotFocus, AddressOf ControlGotFocus
        AddHandler Cbo_Grid_Doff1.GotFocus, AddressOf ControlGotFocus
        AddHandler Cbo_Grid_Doff2.GotFocus, AddressOf ControlGotFocus
        AddHandler Cbo_Grid_Doff3.GotFocus, AddressOf ControlGotFocus
        AddHandler Cbo_Grid_Doff4.GotFocus, AddressOf ControlGotFocus


        AddHandler dtp_Date.LostFocus, AddressOf ControlLostFocus
        AddHandler msk_Date.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Grid_Loom.LostFocus, AddressOf ControlLostFocus
        AddHandler dtp_Filter_Fromdate.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Shift.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Filter_Shift.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Filter_Loom.LostFocus, AddressOf ControlLostFocus
        AddHandler dtp_Filter_ToDate.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Grid_Shift_1_Employee.LostFocus, AddressOf ControlLostFocus
        AddHandler Cbo_Grid_Doff1.LostFocus, AddressOf ControlLostFocus
        AddHandler Cbo_Grid_Doff2.LostFocus, AddressOf ControlLostFocus
        AddHandler Cbo_Grid_Doff3.LostFocus, AddressOf ControlLostFocus
        AddHandler Cbo_Grid_Doff4.LostFocus, AddressOf ControlLostFocus

        AddHandler dtp_Date.KeyDown, AddressOf TextBoxControlKeyDown
        ' AddHandler msk_Date.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler dtp_Filter_Fromdate.KeyDown, AddressOf TextBoxControlKeyDown
        ' AddHandler dtp_Filter_ToDate.KeyDown, AddressOf TextBoxControlKeyDown

        AddHandler dtp_Date.KeyPress, AddressOf TextBoxControlKeyPress
        ' AddHandler msk_Date.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler dtp_Filter_Fromdate.KeyPress, AddressOf TextBoxControlKeyPress
        ' AddHandler dtp_Filter_ToDate.KeyPress, AddressOf TextBoxControlKeyPress

        AddHandler Cbo_Grid_Cloth_Name.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Grid_Shift_2_Employee.GotFocus, AddressOf ControlGotFocus

        AddHandler Cbo_Grid_Cloth_Name.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Grid_Shift_2_Employee.LostFocus, AddressOf ControlLostFocus


        AddHandler Cbo_Grid_Ledger_Name.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_EB_Units.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_EB_Amount.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Employee_Salary.GotFocus, AddressOf ControlGotFocus

        AddHandler Cbo_Grid_Ledger_Name.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_EB_Units.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_EB_Amount.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Employee_Salary.LostFocus, AddressOf ControlLostFocus

        lbl_Company.Text = ""
        lbl_Company.Tag = 0
        lbl_Company.Visible = False
        Common_Procedures.CompIdNo = 0

        Filter_Status = False
        FrmLdSTS = True
        new_record()

    End Sub

    Private Sub LoomNo_Production_Entry_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        On Error Resume Next
        con.Close()
        con.Dispose()
        Common_Procedures.Last_Closed_FormName = Me.Name
    End Sub

    Private Sub LoomNo_Production_Entry_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        Try
            If Asc(e.KeyChar) = 27 Then

                If pnl_Filter.Visible = True Then
                    btn_Filter_Close_Click(sender, e)
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
        Dim vColmnCount_No As Integer = 0
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


                    If .Columns(DgvCol_Details.AMOUNT).Visible Then
                        vColmnCount_No = DgvCol_Details.AMOUNT
                    Else
                        vColmnCount_No = DgvCol_Details.REMARKS
                    End If

                    If keyData = Keys.Enter Or keyData = Keys.Down Then

                        '        If .CurrentCell.ColumnIndex >= DgvCol_Details.SHIFT_2 Then
                        If .CurrentCell.ColumnIndex >= vColmnCount_No Then

                            'If .CurrentCell.ColumnIndex >= .ColumnCount - 3 Then

                            If .CurrentCell.RowIndex = .RowCount - 1 Then
                                If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                                    save_record()
                                Else
                                    msk_Date.Focus()
                                    Return True
                                    Exit Function
                                End If

                            Else
                                If dgv_Details.Rows(dgv_Details.CurrentCell.RowIndex).Cells(DgvCol_Details.CLOTH_NAME).Visible = True Then
                                    .CurrentCell = .Rows(.CurrentRow.Index + 1).Cells(DgvCol_Details.CLOTH_NAME)
                                Else
                                    .CurrentCell = .Rows(.CurrentRow.Index + 1).Cells(DgvCol_Details.SHIFT_1)
                                End If
                                '.CurrentCell = .Rows(.CurrentRow.Index + 1).Cells(DgvCol_Details.SHIFT_1)
                            End If

                            'ElseIf .CurrentCell.ColumnIndex <= DgvCol_Details.SHIFT_1_EMPLOYEE Then
                            '    .CurrentCell = .Rows(.CurrentRow.Index).Cells(DgvCol_Details.SHIFT_1)

                            'ElseIf .CurrentCell.ColumnIndex = DgvCol_Details.SHIFT_2 Then
                            '    .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 2)
                        ElseIf .CurrentCell.ColumnIndex = DgvCol_Details.SHIFT_1 Then
                            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1464" Then '--mani omega
                                .CurrentCell = .Rows(.CurrentRow.Index).Cells(DgvCol_Details.SHIFT_2)
                            Else
                                .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)
                            End If

                        ElseIf .CurrentCell.ColumnIndex = DgvCol_Details.SHIFT_1_WEFT Then

                            .CurrentCell = .Rows(.CurrentRow.Index).Cells(DgvCol_Details.SHIFT_1_EMPLOYEE)

                        ElseIf .CurrentCell.ColumnIndex = DgvCol_Details.SHIFT_2 Then

                            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1464" Then '--mani omega

                                If .CurrentCell.RowIndex = .RowCount - 1 Then
                                    If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                                        save_record()
                                    Else
                                        msk_Date.Focus()
                                    End If
                                Else
                                    .CurrentCell = .Rows(.CurrentRow.Index + 1).Cells(DgvCol_Details.SHIFT_1)
                                End If

                            Else
                                .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)
                            End If

                        ElseIf .CurrentCell.ColumnIndex = DgvCol_Details.REMARKS Then

                            If .Columns(DgvCol_Details.PARTY_NAME).Visible Then
                                .CurrentCell = .Rows(.CurrentRow.Index).Cells(DgvCol_Details.PARTY_NAME)
                            Else
                                .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)

                            End If
                        Else
                            .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)
                                '.CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 1)
                            End If
                            'Else
                            '    .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)
                            'End If

                            Return True

                        ElseIf keyData = Keys.Up Then

                            If .CurrentCell.ColumnIndex <= DgvCol_Details.LOOM_NO Then

                                If .CurrentCell.ColumnIndex = DgvCol_Details.LOOM_NO And .CurrentCell.RowIndex = 0 Then
                                    msk_Date.Focus()
                                    'cbo_Shift.Focus()
                                Else
                                If .Columns(DgvCol_Details.AMOUNT).Visible Then
                                    .CurrentCell = .Rows(.CurrentCell.RowIndex - 1).Cells(DgvCol_Details.AMOUNT)
                                Else
                                    .CurrentCell = .Rows(.CurrentCell.RowIndex - 1).Cells(DgvCol_Details.REMARKS)
                                End If

                                'If .Columns(.ColumnCount - 1).Visible = True And Val(.Rows(.CurrentCell.RowIndex - 1).Cells(.ColumnCount - 1).Value) <> 0 Then
                                '    .CurrentCell = .Rows(.CurrentCell.RowIndex - 1).Cells(.ColumnCount - 1)
                                'ElseIf .Columns(.ColumnCount - 2).Visible = True And Trim(.Rows(.CurrentCell.RowIndex - 1).Cells(.ColumnCount - 2).Value) <> "" Then
                                '    .CurrentCell = .Rows(.CurrentCell.RowIndex - 1).Cells(.ColumnCount - 2)
                                'ElseIf .Columns(.ColumnCount - 3).Visible = True And Val(.Rows(.CurrentCell.RowIndex - 1).Cells(.ColumnCount - 3).Value) <> 0 Then
                                '    .CurrentCell = .Rows(.CurrentCell.RowIndex - 1).Cells(.ColumnCount - 1)
                                'ElseIf .Columns(.ColumnCount - 4).Visible = True And Trim(.Rows(.CurrentCell.RowIndex - 1).Cells(.ColumnCount - 4).Value) <> "" Then
                                '    .CurrentCell = .Rows(.CurrentCell.RowIndex - 1).Cells(.ColumnCount - 4)

                                'ElseIf .Columns(.ColumnCount - 5).Visible = True And Val(.Rows(.CurrentCell.RowIndex - 1).Cells(.ColumnCount - 5).Value) <> 0 Then
                                '    .CurrentCell = .Rows(.CurrentCell.RowIndex - 1).Cells(.ColumnCount - 5)
                                'ElseIf .Columns(.ColumnCount - 6).Visible = True And Trim(.Rows(.CurrentCell.RowIndex - 1).Cells(.ColumnCount - 6).Value) <> "" Then
                                '    .CurrentCell = .Rows(.CurrentCell.RowIndex - 1).Cells(.ColumnCount - 6)

                                'ElseIf .Columns(.ColumnCount - 7).Visible = True And Val(.Rows(.CurrentCell.RowIndex - 1).Cells(.ColumnCount - 7).Value) <> 0 Then
                                '    .CurrentCell = .Rows(.CurrentCell.RowIndex - 1).Cells(.ColumnCount - 7)
                                'ElseIf .Columns(.ColumnCount - 8).Visible = True And Trim(.Rows(.CurrentCell.RowIndex - 1).Cells(.ColumnCount - 8).Value) <> "" Then
                                '    .CurrentCell = .Rows(.CurrentCell.RowIndex - 1).Cells(.ColumnCount - 8)

                                'ElseIf .Columns(.ColumnCount - 9).Visible = True And Val(.Rows(.CurrentCell.RowIndex - 1).Cells(.ColumnCount - 9).Value) <> 0 Then
                                '    .CurrentCell = .Rows(.CurrentCell.RowIndex - 1).Cells(.ColumnCount - 9)

                                'Else
                                '    .CurrentCell = .Rows(.CurrentCell.RowIndex - 1).Cells(.ColumnCount - 10)
                                'End If

                            End If

                                'ElseIf .CurrentCell.ColumnIndex = DgvCol_Details.SHIFT_2 Then
                                '    .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex - 2)
                                ElseIf .CurrentCell.ColumnIndex = DgvCol_Details.SHIFT_1 Then
                                If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1464" Then '--mani omega
                                    If .CurrentCell.ColumnIndex = DgvCol_Details.SHIFT_1 And .CurrentCell.RowIndex = 0 Then
                                        msk_Date.Focus()
                                    Else
                                        .CurrentCell = .Rows(.CurrentCell.RowIndex - 1).Cells(DgvCol_Details.SHIFT_2)
                                    End If
                                Else
                                    .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex - 1)
                                End If

                            ElseIf .CurrentCell.ColumnIndex = DgvCol_Details.SHIFT_2 Then

                                If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1464" Then '--mani omega
                                    .CurrentCell = .Rows(.CurrentRow.Index).Cells(DgvCol_Details.SHIFT_1)
                                Else
                                    .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex - 1)
                                End If
                            ElseIf .CurrentCell.ColumnIndex = DgvCol_Details.REMARKS Then
                                .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(DgvCol_Details.SHIFT_2_EMPLOYEE)
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

    Private Sub move_record(ByVal no As String, SELEC As Boolean)
        Dim cmd As New SqlClient.SqlCommand
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
        Mov_Status = True

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(no) & "/" & Trim(Common_Procedures.FnYearCode)


        Try


            da1 = New SqlClient.SqlDataAdapter("select a.* from LoomNo_Production_Head a  Where a.LoomNo_Production_Code = '" & Trim(NewCode) & "'", con)
            dt1 = New DataTable
            da1.Fill(dt1)

            If dt1.Rows.Count > 0 Then

                lbl_RefNo.Text = dt1.Rows(0).Item("LoomNo_Production_No").ToString
                dtp_Date.Text = dt1.Rows(0).Item("LoomNo_Production_Date").ToString
                msk_Date.Text = dtp_Date.Text
                msk_Date.Tag = msk_Date.Text
                cbo_Shift.Text = Common_Procedures.Shift_IdNoToName(con, dt1.Rows(0).Item("Shift_Idno").ToString)

                txt_EB_Units.Text = dt1.Rows(0).Item("EB_Units_Consumed").ToString
                txt_EB_Amount.Text = dt1.Rows(0).Item("EB_Amount").ToString
                txt_Employee_Salary.Text = dt1.Rows(0).Item("Employee_Salary").ToString



                cmd.Connection = con

                '---------------
                cmd.CommandText = "truncate table " & Trim(Common_Procedures.ReportTempTable) & " "
                cmd.ExecuteNonQuery()


                If SELEC = True Then
                    cmd.CommandText = "insert into " & Trim(Common_Procedures.ReportTempTable) & " (Currency1, name1 ) select LmNo_OrderBy, Loom_Name from Loom_Head Where loom_idno <> 0"
                    cmd.ExecuteNonQuery()
                End If

                cmd.CommandText = "insert into " & Trim(Common_Procedures.ReportTempTable) & "  ( Currency1     , name1     , Meters1   , Meters2       , Meters3     ,           Meters4,       Meters6,    Meters7,        Meters8,                             Name2,              Name3,                                      Name4,                                  Name5      ,   weight1    ,    weight2  ,    weight3  , weight4 ,         Name6     ,     Name7       , Meters9 ,         Meters10        ,            int2         ,             int3    ,            Meters11        ,            int4         ,             int5      ,       Name8       ,   Name9      ,   Name10         ,currency2       ,   currency3 )     " &
                                                                                     "select     b.LmNo_OrderBy, b.Loom_Name, a.Meters  , a.Warp_Meters , a.Weft_Meters, a.Pick_Efficiency, a.Shift1_Mtrs, a.Shift2_Mtrs, a.Shift3_Mtrs, s1.Shift_Name as Doff1_Shift_Name,  s2.Shift_Name as Doff2_Shift_Name,  s3.Shift_Name as Doff3_Shift_Name,  s4.Shift_Name as Doff4_Shift_Name, a.Doff1_Mtrs, a.Doff2_Mtrs, a.Doff3_Mtrs, a.Doff4_Mtrs ,CH.Cloth_Name ,EM.Employee_Name , a.RPM   ,a.Shift_1_Pick_Efficiency,a.Shift_1_Warp_Breakage ,a.Shift_1_Weft_Breakage ,a.Shift_2_Pick_Efficiency  ,a.Shift_2_Warp_Breakage   , Shift_2_Weft_Breakage , EM2.Employee_Name ,a.Remarks  ,LH.Ledger_Name  , a.Rate_Meter      , a.Amount      " &
                                                                                     "from LoomNo_Production_Details a INNER JOIN Loom_Head b ON a.Loom_idno = b.Loom_idno LEFT OUTER JOIN Shift_Head s1 ON a.Doff1_Shift_IdNo = s1.Shift_IdNo  LEFT OUTER JOIN Shift_Head s2 ON a.Doff2_Shift_IdNo = s2.Shift_IdNo  LEFT OUTER JOIN Shift_Head s3 ON a.Doff3_Shift_IdNo = s3.Shift_IdNo LEFT OUTER JOIN Shift_Head s4 ON a.Doff4_Shift_IdNo = s4.Shift_IdNo   LEFT OUTER JOIN Employee_Head EM  ON A.Employee_IdNo_Shift_1=EM.Employee_IdNo  LEFT OUTER JOIN Employee_Head EM2 on A.Employee_IdNo_Shift_2=EM2.Employee_IdNo  LEFT OUTER JOIN  Cloth_Head CH on a.Cloth_idno=CH.Cloth_Idno LEFT  OUTER JOIN ledger_head LH  on a.Ledger_Idno = LH.Ledger_Idno where a.LoomNo_Production_Code = '" & Trim(NewCode) & "' Order by a.Sl_No"
                cmd.ExecuteNonQuery()

                da2 = New SqlClient.SqlDataAdapter("select Currency1 as Loom_Orderby, name1 as Loom_Name, sum(Meters1) as Total_Meters, sum(Meters2) as Warp_Meters , sum(Meters3) as Weft_Meters, sum(Meters4) as Pick_Efficiency, sum(Meters6) as Shift1_Mtrs, sum(Meters7) as Shift2_Mtrs, sum(Meters8) as Shift3_Mtrs, Name2 as Doff1_Shift_Name,  Name3 as Doff2_Shift_Name,  Name4 as Doff3_Shift_Name,  Name5 as Doff4_Shift_Name, sum(weight1) as Doff1_Mtrs, sum(weight2) as Doff2_Mtrs, sum(weight3) as Doff3_Mtrs, sum(weight4) as Doff4_Mtrs ,Name6 as Cloth_Name,Name7 as Shift_1_Employee_Name  ,sum(Meters9) as RPM ,sum(Meters10) AS Shift_1_Pick_Efficiency ,sum(int2) as Shift_1_Warp_Breakage ,sum(int3) as Shift_1_Weft_Breakage ,sum(Meters11) as Shift_2_Pick_Efficiency ,sum(int4) as Shift_2_Warp_Breakage, sum(int5) as Shift_2_Weft_Breakage ,Name8 AS Shift_2_Employee_Name, Name9 as remarks  ,   Name10  as Ledger_Name  , currency2  as Rate_Meter ,  sum(currency3)  as Amount " &
                                                   "from " & Trim(Common_Procedures.ReportTempTable) & "  GROUP BY Currency1, name1, Name2,  Name3,  Name4, Name5 ,Name6,name7,name8 ,NAME9  , Name10 , Currency2  Order by Currency1, Name1", con)
                dt2 = New DataTable
                da2.Fill(dt2)

                '---------------

                'cmd.CommandText = "truncate table entrytemp"
                'cmd.ExecuteNonQuery()


                'If SELEC = True Then
                '    cmd.CommandText = "insert into entrytemp(Currency1, name1 ) select LmNo_OrderBy, Loom_Name from Loom_Head Where loom_idno <> 0"
                '    cmd.ExecuteNonQuery()
                'End If

                'cmd.CommandText = "insert into entrytemp ( Currency1, name1, Meters1, Meters2, Meters3, Meters4, Meters6, Meters7, Meters8, Name2,  Name3,  Name4,  Name5, weight1, weight2, weight3, weight4 ,Name7 ,Name8) select b.LmNo_OrderBy, b.Loom_Name, a.Meters, a.Warp_Meters, a.Weft_Meters, a.Pick_Efficiency, a.Shift1_Mtrs, a.Shift2_Mtrs, a.Shift3_Mtrs, s1.Shift_Name as Doff1_Shift_Name,  s2.Shift_Name as Doff2_Shift_Name,  s3.Shift_Name as Doff3_Shift_Name,  s4.Shift_Name as Doff4_Shift_Name, a.Doff1_Mtrs, a.Doff2_Mtrs, a.Doff3_Mtrs, a.Doff4_Mtrs ,EM.Employee_Name ,CH.Cloth_Name  from LoomNo_Production_Details a INNER JOIN Loom_Head b ON a.Loom_idno = b.Loom_idno LEFT OUTER JOIN Shift_Head s1 ON a.Doff1_Shift_IdNo = s1.Shift_IdNo  LEFT OUTER JOIN Shift_Head s2 ON a.Doff2_Shift_IdNo = s2.Shift_IdNo  LEFT OUTER JOIN Shift_Head s3 ON a.Doff3_Shift_IdNo = s3.Shift_IdNo LEFT OUTER JOIN Shift_Head s4 ON a.Doff4_Shift_IdNo = s4.Shift_IdNo   LEFT OUTER JOIN Employee_Head EM  ON   (A.Employee_IdNo_Shift_1=EM.Employee_IdNo)  or  (A.Employee_IdNo_Shift_2=EM.Employee_IdNo)   LEFT OUTER JOIN  Cloth_Head CH on a.Cloth_idno=CH.Cloth_Idno where a.LoomNo_Production_Code = '" & Trim(NewCode) & "' Order by a.Sl_No"
                'cmd.ExecuteNonQuery()

                'da2 = New SqlClient.SqlDataAdapter("select Currency1 as Loom_Orderby, name1 as Loom_Name, sum(Meters1) as totalMeters, sum(Meters2) as Warp_Meters , sum(Meters3) as Weft_Meters, sum(Meters4) as Pick_Efficiency, sum(Meters6) as Shift1_Mtrs, sum(Meters7) as Shift2_Mtrs, sum(Meters8) as Shift3_Mtrs, Name2 as Doff1_Shift_Name,  Name3 as Doff2_Shift_Name,  Name4 as Doff3_Shift_Name,  Name5 as Doff4_Shift_Name, sum(weight1) as Doff1_Mtrs, sum(weight2) as Doff2_Mtrs, sum(weight3) as Doff3_Mtrs, sum(weight4) as Doff4_Mtrs ,Name7 as Employee_Name ,Name8 as Cloth_Name from entrytemp GROUP BY Currency1, name1, Name2,  Name3,  Name4, Name5 ,Name7,name8  Order by Currency1, Name1", con)
                ''da2 = New SqlClient.SqlDataAdapter("select a.*, b.Loom_Name from LoomNo_Production_Details a INNER JOIN Loom_Head b ON a.Loom_idno = b.Loom_idno  where a.LoomNo_Production_Code = '" & Trim(NewCode) & "' Order by a.Sl_No", con)
                ''da2 = New SqlClient.SqlDataAdapter("select a.*, b.Loom_Name, c.Employee_Name from LoomNo_Production_Details a INNER JOIN Loom_Head b ON a.Loom_idno = b.Loom_idno INNER JOIN PayRoll_Employee_Head c ON a.Employee_idno = c.Employee_idno  where a.LoomNo_Production_Code = '" & Trim(NewCode) & "' Order by a.Sl_No", con)
                'dt2 = New DataTable
                'da2.Fill(dt2)

                dgv_Details.Rows.Clear()
                SNo = 0

                If dt2.Rows.Count > 0 Then

                    For i = 0 To dt2.Rows.Count - 1

                        n = dgv_Details.Rows.Add()

                        SNo = SNo + 1

                        dgv_Details.Rows(n).Cells(DgvCol_Details.SL_NO).Value = Val(SNo)

                        dgv_Details.Rows(n).Cells(DgvCol_Details.LOOM_NO).Value = dt2.Rows(i).Item("Loom_Name").ToString
                        dgv_Details.Rows(n).Cells(DgvCol_Details.CLOTH_NAME).Value = dt2.Rows(i).Item("Cloth_Name").ToString
                        'dgv_Details.Rows(n).Cells(DgvCol_Details.CLOTH_NAME).Value = Common_Procedures.Cloth_IdNoToName(con, Val(dt2.Rows(i).Item("Cloth_IdNo").ToString))

                        dgv_Details.Rows(n).Cells(DgvCol_Details.RPM).Value = Format(Val(dt2.Rows(i).Item("RPM").ToString), "########0.00")

                        dgv_Details.Rows(n).Cells(DgvCol_Details.SHIFT_1_PICK_EFFICIENCY).Value = Format(Val(dt2.Rows(i).Item("Shift_1_Pick_Efficiency").ToString), "########0.00")
                        dgv_Details.Rows(n).Cells(DgvCol_Details.SHIFT_1).Value = Format(Val(dt2.Rows(i).Item("Shift1_Mtrs").ToString), "########0.00")
                        dgv_Details.Rows(n).Cells(DgvCol_Details.SHIFT_1_WARP).Value = dt2.Rows(i).Item("Shift_1_Warp_Breakage").ToString
                        dgv_Details.Rows(n).Cells(DgvCol_Details.SHIFT_1_WEFT).Value = dt2.Rows(i).Item("Shift_1_Weft_Breakage").ToString
                        dgv_Details.Rows(n).Cells(DgvCol_Details.SHIFT_1_EMPLOYEE).Value = dt2.Rows(i).Item("Shift_1_Employee_Name").ToString
                        'dgv_Details.Rows(n).Cells(DgvCol_Details.SHIFT_1_EMPLOYEE).Value = Common_Procedures.Employee_Simple_IdNoToName(con, Val(dt2.Rows(i).Item("Employee_IdNo_Shift_1").ToString))

                        dgv_Details.Rows(n).Cells(DgvCol_Details.SHIFT_2_PICK_EFFICIENCY).Value = Format(Val(dt2.Rows(i).Item("Shift_2_Pick_Efficiency").ToString), "########0.00")
                        dgv_Details.Rows(n).Cells(DgvCol_Details.SHIFT_2).Value = Format(Val(dt2.Rows(i).Item("Shift2_Mtrs").ToString), "########0.00")
                        dgv_Details.Rows(n).Cells(DgvCol_Details.SHIFT_2_WARP).Value = dt2.Rows(i).Item("Shift_2_Warp_Breakage").ToString
                        dgv_Details.Rows(n).Cells(DgvCol_Details.SHIFT_2_WEFT).Value = dt2.Rows(i).Item("Shift_2_Weft_Breakage").ToString
                        dgv_Details.Rows(n).Cells(DgvCol_Details.SHIFT_2_EMPLOYEE).Value = dt2.Rows(i).Item("Shift_2_Employee_Name").ToString
                        'dgv_Details.Rows(n).Cells(DgvCol_Details.SHIFT_2_EMPLOYEE).Value = Common_Procedures.Employee_Simple_IdNoToName(con, Val(dt2.Rows(i).Item("Employee_IdNo_Shift_2").ToString))

                        dgv_Details.Rows(n).Cells(DgvCol_Details.TOTAL_PICK_EFFICIENCY).Value = Format(Val(dt2.Rows(i).Item("Pick_Efficiency").ToString), "########0.00")
                        dgv_Details.Rows(n).Cells(DgvCol_Details.TOTAL_METERS).Value = Format(Val(dt2.Rows(i).Item("Total_Meters").ToString), "########0.00")
                        dgv_Details.Rows(n).Cells(DgvCol_Details.REMARKS).Value = dt2.Rows(i).Item("REMARKS").ToString
                        dgv_Details.Rows(n).Cells(DgvCol_Details.TOTAL_WARP).Value = Format(Val(dt2.Rows(i).Item("Warp_Meters").ToString), "########0.00")
                        dgv_Details.Rows(n).Cells(DgvCol_Details.TOTAL_WEFT).Value = Format(Val(dt2.Rows(i).Item("Weft_Meters").ToString), "########0.00")


                        dgv_Details.Rows(n).Cells(DgvCol_Details.DOFF_1_SHIFT).Value = dt2.Rows(i).Item("Doff1_Shift_Name").ToString
                        dgv_Details.Rows(n).Cells(DgvCol_Details.DOFF_2_SHIFT).Value = dt2.Rows(i).Item("Doff2_Shift_Name").ToString
                        dgv_Details.Rows(n).Cells(DgvCol_Details.DOFF_3_SHIFT).Value = dt2.Rows(i).Item("Doff3_Shift_Name").ToString
                        dgv_Details.Rows(n).Cells(DgvCol_Details.DOFF_4_SHIFT).Value = dt2.Rows(i).Item("Doff4_Shift_Name").ToString

                        ''dgv_Details.Rows(n).Cells(10).Value = Common_Procedures.Shift_IdNoToName(con, dt2.Rows(i).Item("Doff1_Shift_IdNo").ToString)
                        ''dgv_Details.Rows(n).Cells(12).Value = Common_Procedures.Shift_IdNoToName(con, dt2.Rows(i).Item("Doff2_Shift_IdNo").ToString)
                        ''dgv_Details.Rows(n).Cells(14).Value = Common_Procedures.Shift_IdNoToName(con, dt2.Rows(i).Item("Doff3_Shift_IdNo").ToString)
                        ''dgv_Details.Rows(n).Cells(16).Value = Common_Procedures.Shift_IdNoToName(con, dt2.Rows(i).Item("Doff4_Shift_IdNo").ToString)

                        dgv_Details.Rows(n).Cells(DgvCol_Details.DOFF_1_METERS).Value = Format(Val(dt2.Rows(i).Item("Doff1_Mtrs").ToString), "########0.00")
                        dgv_Details.Rows(n).Cells(DgvCol_Details.DOFF_2_METERS).Value = Format(Val(dt2.Rows(i).Item("Doff2_Mtrs").ToString), "########0.00")
                        dgv_Details.Rows(n).Cells(DgvCol_Details.DOFF_3_METERS).Value = Format(Val(dt2.Rows(i).Item("Doff3_Mtrs").ToString), "########0.00")
                        dgv_Details.Rows(n).Cells(DgvCol_Details.DOFF_4_METERS).Value = Format(Val(dt2.Rows(i).Item("Doff4_Mtrs").ToString), "########0.00")

                        dgv_Details.Rows(n).Cells(DgvCol_Details.PARTY_NAME).Value = dt2.Rows(i).Item("Ledger_Name").ToString
                        dgv_Details.Rows(n).Cells(DgvCol_Details.RATE_METER).Value = Format(Val(dt2.Rows(i).Item("Rate_Meter").ToString), "########0.00")
                        dgv_Details.Rows(n).Cells(DgvCol_Details.AMOUNT).Value = Format(Val(dt2.Rows(i).Item("Amount").ToString), "########0.00")


                    Next i

                End If

                If dgv_Details.RowCount = 0 Then
                    dgv_Details.Rows.Add()
                End If
                With dgv_Details_Total

                    If dgv_Details_Total.RowCount = 0 Then dgv_Details_Total.Rows.Add()

                    .Rows(0).Cells(DgvCol_Details.RPM).Value = Format(Val(dt1.Rows(0).Item("Total_Avg_RPM").ToString), "########0.00")
                    .Rows(0).Cells(DgvCol_Details.SHIFT_1_PICK_EFFICIENCY).Value = Format(Val(dt1.Rows(0).Item("Total_Avg_Shift_1_Pick_Efficiency").ToString), "########0.00")
                    .Rows(0).Cells(DgvCol_Details.SHIFT_1).Value = Format(Val(dt1.Rows(0).Item("total_shift1_mtrs").ToString), "########0.00")
                    .Rows(0).Cells(DgvCol_Details.SHIFT_1_WARP).Value = dt1.Rows(0).Item("Total_Shift_1_Warp_Breakage").ToString
                    .Rows(0).Cells(DgvCol_Details.SHIFT_1_WARP).Value = dt1.Rows(0).Item("Total_Shift_1_Weft_Breakage").ToString

                    .Rows(0).Cells(DgvCol_Details.SHIFT_2_PICK_EFFICIENCY).Value = Format(Val(dt1.Rows(0).Item("Total_Avg_Shift_2_Pick_Efficiency").ToString), "########0.00")
                    .Rows(0).Cells(DgvCol_Details.SHIFT_2).Value = Format(Val(dt1.Rows(0).Item("total_shift2_mtrs").ToString), "########0.00")
                    .Rows(0).Cells(DgvCol_Details.SHIFT_2_WARP).Value = dt1.Rows(0).Item("Total_Shift_2_Warp_Breakage").ToString
                    .Rows(0).Cells(DgvCol_Details.SHIFT_2_WARP).Value = dt1.Rows(0).Item("Total_Shift_2_Weft_Breakage").ToString


                    .Rows(0).Cells(DgvCol_Details.TOTAL_PICK_EFFICIENCY).Value = Format(Val(dt1.Rows(0).Item("Total_PickEfficiency").ToString), "########0.00")
                    .Rows(0).Cells(DgvCol_Details.TOTAL_METERS).Value = Format(Val(dt1.Rows(0).Item("Total_Meters").ToString), "########0.00")
                    .Rows(0).Cells(DgvCol_Details.TOTAL_WARP).Value = Format(Val(dt1.Rows(0).Item("Total_WarpMeters").ToString), "########0.00")
                    .Rows(0).Cells(DgvCol_Details.TOTAL_WEFT).Value = Format(Val(dt1.Rows(0).Item("Total_WeftMeters").ToString), "########0.00")

                    .Rows(0).Cells(DgvCol_Details.AMOUNT).Value = Format(Val(dt1.Rows(0).Item("ToTal_Amount").ToString), "########0.00")



                    '.Rows(0).Cells(DgvCol_Details.TOTAL_METERS).Value = Format(Val(dt1.Rows(0).Item("Total_Meters").ToString), "########0.00")
                    '.Rows(0).Cells(DgvCol_Details.TOTAL_WARP).Value = Format(Val(dt1.Rows(0).Item("Total_WarpMeters").ToString), "########0.00")
                    '.Rows(0).Cells(DgvCol_Details.TOTAL_WEFT).Value = Format(Val(dt1.Rows(0).Item("Total_WeftMeters").ToString), "########0.00")
                    '.Rows(0).Cells(DgvCol_Details.TOTAL_PICK_EFFICIENCY).Value = Format(Val(dt1.Rows(0).Item("Total_PickEfficiency").ToString), "########0.000")

                    '.Rows(0).Cells(DgvCol_Details.SHIFT_1).Value = Format(Val(dt1.Rows(0).Item("Total_Shift1_Mtrs").ToString), "########0.000")
                    '.Rows(0).Cells(DgvCol_Details.SHIFT_2).Value = Format(Val(dt1.Rows(0).Item("Total_Shift2_Mtrs").ToString), "########0.000")
                    '.Rows(0).Cells(DgvCol_Details.SHIFT_3).Value = Format(Val(dt1.Rows(0).Item("Total_Shift3_Mtrs").ToString), "########0.000")

                    ''.Rows(0).Cells(11).Value = Format(Val(dt1.Rows(0).Item("Total_Doff1_Mtrs").ToString), "########0.000")
                    ''.Rows(0).Cells(13).Value = Format(Val(dt1.Rows(0).Item("Total_Doff2_Mtrs").ToString), "########0.000")
                    ''.Rows(0).Cells(15).Value = Format(Val(dt1.Rows(0).Item("Total_Doff3_Mtrs").ToString), "########0.000")
                    ''.Rows(0).Cells(17).Value = Format(Val(dt1.Rows(0).Item("Total_Doff4_Mtrs").ToString), "########0.000")


                End With
                dt2.Dispose()
                da2.Dispose()

            End If

            dt1.Dispose()
            da1.Dispose()

            Grid_DeSelect()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT MOVE RECORDS...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Finally
            If msk_Date.Visible And msk_Date.Enabled Then msk_Date.Focus()

        End Try
        NoCalc_Status = False
        Mov_Status = False
    End Sub

    Public Sub delete_record() Implements Interface_MDIActions.delete_record
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim cmd As New SqlClient.SqlCommand
        Dim trans As SqlClient.SqlTransaction
        Dim NewCode As String = ""
        Dim vOrdByNo As String = ""

        vOrdByNo = Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text)

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.DeletingEntry, Common_Procedures.UR.Inhouse_Loom_Production_Entry, New_Entry, Me, con, "LoomNo_Production_Head", "LoomNo_Production_Code", NewCode, "LoomNo_Production_Date", "(LoomNo_Production_Code = '" & Trim(NewCode) & "')") = False Then Exit Sub


        If pnl_Back.Enabled = False Then
            MessageBox.Show("Close other windows", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        If MessageBox.Show("Do you want to Delete?", "FOR DELETION...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) <> DialogResult.Yes Then
            Exit Sub
        End If

        If New_Entry = True Then
            MessageBox.Show("This is New Entry", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        trans = con.BeginTransaction

        Try

            NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            cmd.Connection = con
            cmd.Transaction = trans

            Call Common_Procedures.User_Modification_Updation(con, "HEAD", Me.Name, "DELETE", "LoomNo_Production_Head", "LoomNo_Production_Code", Val(lbl_Company.Tag), NewCode, lbl_RefNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, True, "", "", "LoomNo_Production_Code, Company_IdNo, for_OrderBy", trans)

            Call Common_Procedures.User_Modification_Updation(con, "DETAILS", Me.Name, "DELETE", "LoomNo_Production_Details", "LoomNo_Production_Code", Val(lbl_Company.Tag), NewCode, lbl_RefNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, True, " Shift_IdNo   , Loom_IdNo, Meters   ,  Warp_Meters  ,   Weft_Meters  ,  Pick_Efficiency , Employee_IdNo", "Sl_No", "LoomNo_Production_Code, For_OrderBy, Company_IdNo, LoomNo_Production_No, LoomNo_Production_Date, Ledger_Idno", trans)

            cmd.CommandText = "Delete from LoomNo_Production_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and LoomNo_Production_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "delete from LoomNo_Production_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and LoomNo_Production_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            trans.Commit()

            Dt1.Dispose()
            Da.Dispose()
            trans.Dispose()
            cmd.Dispose()

            new_record()

            MessageBox.Show("Deleted Sucessfully!!!", "FOR DELETION...", MessageBoxButtons.OK, MessageBoxIcon.Information)

        Catch ex As Exception
            trans.Rollback()
            MessageBox.Show(ex.Message, "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally

            If msk_Date.Visible And msk_Date.Enabled Then msk_Date.Focus()

        End Try
    End Sub

    Public Sub filter_record() Implements Interface_MDIActions.filter_record

        Exit Sub

        If Filter_Status = False Then



            cbo_Filter_Loom.Text = ""
            cbo_Filter_Loom.SelectedIndex = -1
            cbo_Filter_Shift.Text = ""
            cbo_Filter_Shift.SelectedIndex = -1
            dgv_Filter_Details.Rows.Clear()

        End If

        pnl_Filter.Visible = True
        pnl_Back.Enabled = False
        If dtp_Filter_Fromdate.Enabled And dtp_Filter_Fromdate.Visible Then dtp_Filter_Fromdate.Focus()

    End Sub

    Public Sub insert_record() Implements Interface_MDIActions.insert_record
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim movno As String, inpno As String
        Dim RefCode As String


        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.InsertingEntry, Common_Procedures.UR.Inhouse_Loom_Production_Entry, New_Entry, Me) = False Then Exit Sub



        Try

            inpno = InputBox("Enter New LOOM.No.", "FOR NEW LOOM NO INSERTION...")

            RefCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

            Da = New SqlClient.SqlDataAdapter("select LoomNo_Production_No from LoomNo_Production_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and LoomNo_Production_Code = '" & Trim(RefCode) & "'", con)
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
                move_record(movno, False)

            Else
                If Val(inpno) = 0 Then
                    MessageBox.Show("Invalid Loom No", "DOES NOT INSERT NEW LOOM NO...", MessageBoxButtons.OK, MessageBoxIcon.Error)

                Else
                    new_record()
                    Insert_Entry = True
                    lbl_RefNo.Text = Trim(UCase(inpno))

                End If

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT INSERT NEW LOOM...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Public Sub movefirst_record() Implements Interface_MDIActions.movefirst_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim movno As String

        Try

            da = New SqlClient.SqlDataAdapter("select top 1 LoomNo_Production_No from LoomNo_Production_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and LoomNo_Production_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby, LoomNo_Production_No", con)
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

            If Val(movno) <> 0 Then move_record(movno, False)

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

            da = New SqlClient.SqlDataAdapter("select top 1 LoomNo_Production_No from LoomNo_Production_Head where for_orderby > " & Str(Val(OrdByNo)) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and LoomNo_Production_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby, LoomNo_Production_No", con)
            da.Fill(dt)

            movno = ""
            If dt.Rows.Count > 0 Then
                If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                    movno = dt.Rows(0)(0).ToString
                End If
            End If

            If Val(movno) <> 0 Then move_record(movno, False)

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

            da = New SqlClient.SqlDataAdapter("select top 1 LoomNo_Production_No from LoomNo_Production_Head where for_orderby < " & Str(Val(OrdByNo)) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and LoomNo_Production_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, LoomNo_Production_No desc", con)
            da.Fill(dt)

            movno = ""
            If dt.Rows.Count > 0 Then
                If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                    movno = dt.Rows(0)(0).ToString
                End If
            End If

            If Val(movno) <> 0 Then move_record(movno, False)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR  MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Public Sub movelast_record() Implements Interface_MDIActions.movelast_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim movno As String = ""

        Try
            da = New SqlClient.SqlDataAdapter("select top 1 LoomNo_Production_No from LoomNo_Production_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and LoomNo_Production_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, LoomNo_Production_No desc", con)
            da.Fill(dt)

            movno = ""
            If dt.Rows.Count > 0 Then
                If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                    movno = dt.Rows(0)(0).ToString
                End If
            End If

            If Val(movno) <> 0 Then move_record(movno, False)

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

            msk_Date.Enabled = True
            dtp_Date.Enabled = True

            New_Entry = True

            lbl_RefNo.Text = Common_Procedures.get_MaxCode(con, "LoomNo_Production_Head", "LoomNo_Production_Code", "For_OrderBy", "", Val(lbl_Company.Tag), Common_Procedures.FnYearCode)

            lbl_RefNo.ForeColor = Color.Red

            msk_Date.Text = Date.Today.ToShortDateString
            da = New SqlClient.SqlDataAdapter("select top 1 * from LoomNo_Production_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and LoomNo_Production_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, LoomNo_Production_No desc", con)
            dt1 = New DataTable
            da.Fill(dt1)
            If dt1.Rows.Count > 0 Then
                If Val(Common_Procedures.settings.PreviousEntryDate_ByDefault) = 1 Then '---- M.S Textiles (Tirupur)
                    If dt1.Rows(0).Item("LoomNo_Production_Date").ToString <> "" Then msk_Date.Text = dt1.Rows(0).Item("LoomNo_Production_Date").ToString
                End If
            End If
            dt1.Clear()

            If msk_Date.Enabled And msk_Date.Visible Then
                msk_Date.Focus()
                msk_Date.SelectionStart = 0
            End If

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
        Dim RefCode As String

        Try

            inpno = InputBox("Enter Ref.No.", "FOR FINDING...")

            RefCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

            Da = New SqlClient.SqlDataAdapter("select LoomNo_Production_No from LoomNo_Production_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and LoomNo_Production_Code = '" & Trim(RefCode) & "'", con)
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
                move_record(movno, False)

            Else
                MessageBox.Show("Loom No. does not exists", "DOES NOT FIND...", MessageBoxButtons.OK, MessageBoxIcon.Error)

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT FIND...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Public Sub save_record() Implements Interface_MDIActions.save_record
        Dim cmd As New SqlClient.SqlCommand
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim tr As SqlClient.SqlTransaction
        Dim NewCode As String = ""
        Dim Lom_Id As Integer = 0
        Dim Item_ID As Integer = 0
        Dim Emp_Id As Integer = 0
        Dim Sht_ID As Integer = 0
        Dim Brand_ID As Integer = 0
        Dim Sno As Integer = 0
        Dim vTotMtr As Single = 0, vTotWrp As Single = 0, vTotWeft As Single = 0, vTotEff As Single = 0
        Dim EntID As String = ""
        Dim PBlNo As String = ""
        Dim Partcls As String = ""
        Dim vOrdByNo As String = ""
        Dim vShift1 As String = 0
        Dim vShift2 As String = 0
        Dim vShift3 As String = 0
        Dim vDoff1_Mtrs As String = 0
        Dim vDoff2_Mtrs As String = 0
        Dim vDoff3_Mtrs As String = 0
        Dim vDoff4_Mtrs As String = 0
        Dim vLASTDOFSTS As Integer = 0
        Dim vONLOOMFABMTRS As String = 0
        Dim vSHFTPRODMTRS As String = 0
        Dim DofShit1_Id As Integer = 0
        Dim DofShit2_Id As Integer = 0
        Dim DofShit3_Id As Integer = 0
        Dim DofShit4_Id As Integer = 0
        Dim Shift_Id As Integer

        Dim vTot_Avg_Rpm As Single = 0
        Dim vTot_Avg_Shift1_PICKEFF As Single = 0
        Dim vTot_Shift1_Warp As Single = 0
        Dim vTot_Shift1_Weft As Single = 0

        Dim vTot_Avg_Shift2_PICKEFF As Single = 0
        Dim vTot_Shift2_Warp As Single = 0
        Dim vTot_Shift2_Weft As Single = 0

        Dim Emp_Id_Shift_1 As Integer = 0
        Dim Emp_Id_Shift_2 As Integer = 0
        Dim cloth_Idno As Integer = 0


        Dim vTot_Amount As String = 0
        Dim Led_ID As String = 0

        vOrdByNo = Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text)
        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.SavingEntry, Common_Procedures.UR.Inhouse_Loom_Production_Entry, New_Entry, Me, con, "LoomNo_Production_Head", "LoomNo_Production_Code", NewCode, "LoomNo_Production_Date", "(LoomNo_Production_Code = '" & Trim(NewCode) & "')", "(Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and LoomNo_Production_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "')", "for_Orderby desc, LoomNo_Production_No desc", dtp_Date.Value.Date) = False Then Exit Sub

        If Val(lbl_Company.Tag) = 0 Then
            MessageBox.Show("Invalid Company Selection", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        If pnl_Back.Enabled = False Then
            MessageBox.Show("Close Other Windows", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        If IsDate(msk_Date.Text) = False Then
            MessageBox.Show("Invalid Date", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If msk_Date.Enabled Then msk_Date.Focus()
            Exit Sub
        End If


        If Not (Convert.ToDateTime(msk_Date.Text) >= Common_Procedures.Company_FromDate And Convert.ToDateTime(msk_Date.Text) <= Common_Procedures.Company_ToDate) Then
            MessageBox.Show("Date is out of financial range", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If msk_Date.Enabled Then msk_Date.Focus()
            Exit Sub
        End If


        'Sht_ID = Common_Procedures.Shift_NameToIdNo(con, cbo_Shift.Text)
        'If Sht_ID = 0 Then
        '    MessageBox.Show("Invalid Shift Name", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
        '    If cbo_Shift.Enabled And cbo_Shift.Visible Then cbo_Shift.Focus()
        '    Exit Sub
        'End If

        For i = 0 To dgv_Details.RowCount - 1

            If Trim(dgv_Details.Rows(i).Cells(DgvCol_Details.LOOM_NO).Value) <> "" Or Trim(dgv_Details.Rows(i).Cells(DgvCol_Details.SHIFT_1_EMPLOYEE).Value) <> "" Or Trim(dgv_Details.Rows(i).Cells(DgvCol_Details.SHIFT_2_EMPLOYEE).Value) <> "" Or Trim(dgv_Details.Rows(i).Cells(DgvCol_Details.DOFF_1_SHIFT).Value) <> "" Then

                Lom_Id = Common_Procedures.Loom_NameToIdNo(con, dgv_Details.Rows(i).Cells(DgvCol_Details.LOOM_NO).Value)
                If Lom_Id = 0 Then
                    MessageBox.Show("Invalid Loom Name", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                    If dgv_Details.Enabled And dgv_Details.Visible Then
                        dgv_Details.Focus()
                        dgv_Details.CurrentCell = dgv_Details.Rows(i).Cells(DgvCol_Details.LOOM_NO)
                    End If
                    Exit Sub
                End If

                'If Val(dgv_Details.Rows(i).Cells(2).Value) = 0 Then
                '    MessageBox.Show("Invalid Meters", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                '    If dgv_Details.Enabled And dgv_Details.Visible Then
                '        dgv_Details.Focus()
                '        dgv_Details.CurrentCell = dgv_Details.Rows(i).Cells(2)
                '    End If
                '    Exit Sub
                'End If



                'Emp_Id = Common_Procedures.Employee_NameToIdNo(con, dgv_Details.Rows(i).Cells(6).Value)
                'If Emp_Id = 0 Then
                '    MessageBox.Show("Invalid Employee Name", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                '    If dgv_Details.Enabled And dgv_Details.Visible Then
                '        dgv_Details.Focus()
                '        dgv_Details.CurrentCell = dgv_Details.Rows(i).Cells(6)
                '    End If
                '    Exit Sub
                'End If

                If Val(dgv_Details.Rows(i).Cells(DgvCol_Details.SHIFT_1).Value) < 0 Then
                    MessageBox.Show("Invalid Shift-1 Production Meters", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                    If dgv_Details.Enabled And dgv_Details.Visible Then
                        dgv_Details.Focus()
                        dgv_Details.CurrentCell = dgv_Details.Rows(i).Cells(DgvCol_Details.SHIFT_1)
                    End If
                    Exit Sub
                End If

                If Val(dgv_Details.Rows(i).Cells(DgvCol_Details.SHIFT_2).Value) < 0 Then
                    MessageBox.Show("Invalid Shift-2 Production Meters", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                    If dgv_Details.Enabled And dgv_Details.Visible Then
                        dgv_Details.Focus()
                        dgv_Details.CurrentCell = dgv_Details.Rows(i).Cells(DgvCol_Details.SHIFT_2)
                    End If
                    Exit Sub
                End If

                If Val(dgv_Details.Rows(i).Cells(DgvCol_Details.SHIFT_3).Value) < 0 Then
                    MessageBox.Show("Invalid Shift-3 Production Meters", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                    If dgv_Details.Enabled And dgv_Details.Visible Then
                        dgv_Details.Focus()
                        dgv_Details.CurrentCell = dgv_Details.Rows(i).Cells(DgvCol_Details.SHIFT_3)
                    End If
                    Exit Sub
                End If


                If Val(dgv_Details.Rows(i).Cells(DgvCol_Details.TOTAL_METERS).Value) < 0 Then
                    MessageBox.Show("Invalid Production Meters", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                    If dgv_Details.Enabled And dgv_Details.Visible Then
                        dgv_Details.Focus()
                        dgv_Details.CurrentCell = dgv_Details.Rows(i).Cells(DgvCol_Details.TOTAL_METERS)
                    End If
                    Exit Sub
                End If


                If Val(dgv_Details.Rows(i).Cells(DgvCol_Details.DOFF_1_METERS).Value) > 0 Then

                    Shift_Id = Common_Procedures.Shift_NameToIdNo(con, dgv_Details.Rows(i).Cells(DgvCol_Details.DOFF_1_SHIFT).Value)
                    If Shift_Id = 0 Then
                        MessageBox.Show("Invalid Doff-1 Shift", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                        If dgv_Details.Enabled And dgv_Details.Visible Then
                            dgv_Details.Focus()
                            dgv_Details.CurrentCell = dgv_Details.Rows(i).Cells(DgvCol_Details.DOFF_1_SHIFT)
                        End If
                        Exit Sub
                    End If

                    If Shift_Id = 7 Then
                        vSHFTPRODMTRS = Val(dgv_Details.Rows(i).Cells(DgvCol_Details.SHIFT_2).Value)
                    Else
                        vSHFTPRODMTRS = Val(dgv_Details.Rows(i).Cells(DgvCol_Details.SHIFT_1).Value)
                    End If

                    vONLOOMFABMTRS = Val(vSHFTPRODMTRS) - Val(dgv_Details.Rows(i).Cells(DgvCol_Details.DOFF_1_METERS).Value)

                    If Val(vONLOOMFABMTRS) < 0 Then
                        MessageBox.Show("Invalid Doff-1 Meters", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                        If dgv_Details.Enabled And dgv_Details.Visible Then
                            dgv_Details.Focus()
                            dgv_Details.CurrentCell = dgv_Details.Rows(i).Cells(DgvCol_Details.DOFF_1_METERS)
                        End If
                        Exit Sub
                    End If

                End If


                If Val(dgv_Details.Rows(i).Cells(DgvCol_Details.DOFF_2_METERS).Value) > 0 Then

                    Shift_Id = Common_Procedures.Shift_NameToIdNo(con, dgv_Details.Rows(i).Cells(DgvCol_Details.DOFF_2_SHIFT).Value)
                    If Shift_Id = 0 Then
                        MessageBox.Show("Invalid Doff-2 Shift", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                        If dgv_Details.Enabled And dgv_Details.Visible Then
                            dgv_Details.Focus()
                            dgv_Details.CurrentCell = dgv_Details.Rows(i).Cells(DgvCol_Details.DOFF_2_SHIFT)
                        End If
                        Exit Sub
                    End If

                    If Shift_Id = 7 Then
                        vSHFTPRODMTRS = Val(dgv_Details.Rows(i).Cells(DgvCol_Details.SHIFT_2).Value)
                    Else
                        vSHFTPRODMTRS = Val(dgv_Details.Rows(i).Cells(DgvCol_Details.SHIFT_1).Value)
                    End If

                    vONLOOMFABMTRS = Val(vSHFTPRODMTRS) - Val(dgv_Details.Rows(i).Cells(DgvCol_Details.DOFF_2_METERS).Value)

                    If Val(vONLOOMFABMTRS) < 0 Then
                        MessageBox.Show("Invalid Doff-2 Meters", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                        If dgv_Details.Enabled And dgv_Details.Visible Then
                            dgv_Details.Focus()
                            dgv_Details.CurrentCell = dgv_Details.Rows(i).Cells(DgvCol_Details.DOFF_2_METERS)
                        End If
                        Exit Sub
                    End If

                End If


                If Val(dgv_Details.Rows(i).Cells(DgvCol_Details.DOFF_3_METERS).Value) > 0 Then

                    Shift_Id = Common_Procedures.Shift_NameToIdNo(con, dgv_Details.Rows(i).Cells(DgvCol_Details.DOFF_3_SHIFT).Value)
                    If Shift_Id = 0 Then
                        MessageBox.Show("Invalid Doff-3 Shift", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                        If dgv_Details.Enabled And dgv_Details.Visible Then
                            dgv_Details.Focus()
                            dgv_Details.CurrentCell = dgv_Details.Rows(i).Cells(DgvCol_Details.DOFF_3_SHIFT)
                        End If
                        Exit Sub
                    End If

                    If Shift_Id = 7 Then
                        vSHFTPRODMTRS = Val(dgv_Details.Rows(i).Cells(DgvCol_Details.SHIFT_2).Value)
                    Else
                        vSHFTPRODMTRS = Val(dgv_Details.Rows(i).Cells(DgvCol_Details.SHIFT_1).Value)
                    End If

                    vONLOOMFABMTRS = Val(vSHFTPRODMTRS) - Val(dgv_Details.Rows(i).Cells(DgvCol_Details.DOFF_3_METERS).Value)

                    If Val(vONLOOMFABMTRS) < 0 Then
                        MessageBox.Show("Invalid Doff-3 Meters", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                        If dgv_Details.Enabled And dgv_Details.Visible Then
                            dgv_Details.Focus()
                            dgv_Details.CurrentCell = dgv_Details.Rows(i).Cells(DgvCol_Details.DOFF_3_METERS)
                        End If
                        Exit Sub
                    End If

                End If


                If Val(dgv_Details.Rows(i).Cells(DgvCol_Details.DOFF_4_METERS).Value) > 0 Then

                    Shift_Id = Common_Procedures.Shift_NameToIdNo(con, dgv_Details.Rows(i).Cells(DgvCol_Details.DOFF_4_SHIFT).Value)
                    If Shift_Id = 0 Then
                        MessageBox.Show("Invalid Doff-4 Shift", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                        If dgv_Details.Enabled And dgv_Details.Visible Then
                            dgv_Details.Focus()
                            dgv_Details.CurrentCell = dgv_Details.Rows(i).Cells(DgvCol_Details.DOFF_4_SHIFT)
                        End If
                        Exit Sub
                    End If

                    If Shift_Id = 7 Then
                        vSHFTPRODMTRS = Val(dgv_Details.Rows(i).Cells(DgvCol_Details.SHIFT_2).Value)
                    Else
                        vSHFTPRODMTRS = Val(dgv_Details.Rows(i).Cells(DgvCol_Details.SHIFT_1).Value)
                    End If

                    vONLOOMFABMTRS = Val(vSHFTPRODMTRS) - Val(dgv_Details.Rows(i).Cells(DgvCol_Details.DOFF_4_METERS).Value)

                    If Val(vONLOOMFABMTRS) < 0 Then
                        MessageBox.Show("Invalid Doff-4 Meters", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                        If dgv_Details.Enabled And dgv_Details.Visible Then
                            dgv_Details.Focus()
                            dgv_Details.CurrentCell = dgv_Details.Rows(i).Cells(DgvCol_Details.DOFF_4_METERS)
                        End If
                        Exit Sub
                    End If

                End If

            End If

        Next


        vTotMtr = 0 : vTotWrp = 0 : vTotWeft = 0 : vTotEff = 0
        vTot_Avg_Rpm = 0 : vTot_Avg_Shift1_PICKEFF = 0 : vTot_Shift1_Warp = 0 : vTot_Shift1_Weft = 0
        vTot_Avg_Shift2_PICKEFF = 0 : vTot_Shift2_Warp = 0 : vTot_Shift2_Weft = 0

        If dgv_Details_Total.RowCount > 0 Then

            vTot_Avg_Rpm = Val(dgv_Details_Total.Rows(0).Cells(DgvCol_Details.RPM).Value())
            vTot_Avg_Shift1_PICKEFF = Val(dgv_Details_Total.Rows(0).Cells(DgvCol_Details.SHIFT_1_PICK_EFFICIENCY).Value())
            vShift1 = Val(dgv_Details_Total.Rows(0).Cells(DgvCol_Details.SHIFT_1).Value())
            vTot_Shift1_Warp = Val(dgv_Details_Total.Rows(0).Cells(DgvCol_Details.SHIFT_1_WARP).Value())
            vTot_Shift1_Weft = Val(dgv_Details_Total.Rows(0).Cells(DgvCol_Details.SHIFT_1_WEFT).Value())

            vTot_Avg_Shift2_PICKEFF = Val(dgv_Details_Total.Rows(0).Cells(DgvCol_Details.SHIFT_2_PICK_EFFICIENCY).Value())
            vShift2 = Val(dgv_Details_Total.Rows(0).Cells(DgvCol_Details.SHIFT_2).Value())
            vTot_Shift2_Warp = Val(dgv_Details_Total.Rows(0).Cells(DgvCol_Details.SHIFT_2_WARP).Value())
            vTot_Shift2_Weft = Val(dgv_Details_Total.Rows(0).Cells(DgvCol_Details.SHIFT_2_WEFT).Value())

            vShift3 = Val(dgv_Details_Total.Rows(0).Cells(DgvCol_Details.SHIFT_3).Value())

            vTotEff = Val(dgv_Details_Total.Rows(0).Cells(DgvCol_Details.TOTAL_PICK_EFFICIENCY).Value())
            vTotMtr = Val(dgv_Details_Total.Rows(0).Cells(DgvCol_Details.TOTAL_METERS).Value())
            vTotWrp = Val(dgv_Details_Total.Rows(0).Cells(DgvCol_Details.TOTAL_WARP).Value())
            vTotWeft = Val(dgv_Details_Total.Rows(0).Cells(DgvCol_Details.TOTAL_WEFT).Value())


            vTot_Amount = Val(dgv_Details_Total.Rows(0).Cells(DgvCol_Details.AMOUNT).Value())


            'vDoff1_Mtrs = Val(dgv_Details_Total.Rows(0).Cells(DgvCol_Details.DOFF_1_METERS).Value())
            'vDoff2_Mtrs = Val(dgv_Details_Total.Rows(0).Cells(DgvCol_Details.DOFF_2_METERS).Value())
            'vDoff3_Mtrs = Val(dgv_Details_Total.Rows(0).Cells(DgvCol_Details.DOFF_3_METERS).Value())
            'vDoff4_Mtrs = Val(dgv_Details_Total.Rows(0).Cells(DgvCol_Details.DOFF_4_METERS).Value())



            ''vTotMtr = Val(dgv_Details_Total.Rows(0).Cells(DgvCol_Details.TOTAL_METERS).Value())
            ''vTotWrp = Val(dgv_Details_Total.Rows(0).Cells(DgvCol_Details.TOTAL_WARP).Value())
            ''vTotWeft = Val(dgv_Details_Total.Rows(0).Cells(DgvCol_Details.TOTAL_WEFT).Value())
            ''vTotEff = Val(dgv_Details_Total.Rows(0).Cells(DgvCol_Details.TOTAL_PICK_EFFICIENCY).Value())

            ''vShift1 = Val(dgv_Details_Total.Rows(0).Cells(DgvCol_Details.SHIFT_1).Value())
            ''vShift2 = Val(dgv_Details_Total.Rows(0).Cells(DgvCol_Details.SHIFT_2).Value())
            ''vShift3 = Val(dgv_Details_Total.Rows(0).Cells(DgvCol_Details.SHIFT_3).Value())

            ''vDoff1_Mtrs = Val(dgv_Details_Total.Rows(0).Cells(DgvCol_Details.DOFF_1_METERS).Value())
            ''vDoff2_Mtrs = Val(dgv_Details_Total.Rows(0).Cells(DgvCol_Details.DOFF_2_METERS).Value())
            ''vDoff3_Mtrs = Val(dgv_Details_Total.Rows(0).Cells(DgvCol_Details.DOFF_3_METERS).Value())
            ''vDoff4_Mtrs = Val(dgv_Details_Total.Rows(0).Cells(DgvCol_Details.DOFF_4_METERS).Value())

        End If

        tr = con.BeginTransaction

        Try

            If Insert_Entry = True Or New_Entry = False Then
                NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            Else

                lbl_RefNo.Text = Common_Procedures.get_MaxCode(con, "LoomNo_Production_Head", "LoomNo_Production_Code", "For_OrderBy", "", Val(lbl_Company.Tag), Common_Procedures.FnYearCode, tr)

                NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            End If

            cmd.Connection = con
            cmd.Transaction = tr
            cmd.Parameters.Clear()
            cmd.Parameters.AddWithValue("@LoomDate", Convert.ToDateTime(msk_Date.Text))

            If New_Entry = True Then


                cmd.CommandText = "Insert into LoomNo_Production_Head(      LoomNo_Production_Code,         Company_IdNo,                   LoomNo_Production_No,                           for_OrderBy,                                         LoomNo_Production_Date,         Shift_IdNo,             Total_Meters,                  Total_WarpMeters   ,        Total_WeftMeters  ,             Total_PickEfficiency        ,       Total_Shift1_Mtrs    ,       Total_Shift2_Mtrs ,                Total_Shift3_Mtrs   ,   Total_Doff1_Mtrs    ,        Total_Doff2_Mtrs  ,                     Total_Doff3_Mtrs ,          Total_Doff4_Mtrs        ,          Total_Avg_RPM       ,   Total_Avg_Shift_1_Pick_Efficiency   ,     Total_Shift_1_Warp_Breakage   ,    Total_Shift_1_Weft_Breakage    ,     Total_Avg_Shift_2_Pick_Efficiency   ,   Total_Shift_2_Warp_Breakage     ,     Total_Shift_2_Weft_Breakage     ,           Total_Amount        ,    EB_Units_Consumed                ,                      EB_Amount            ,   Employee_Salary  ) " &
                                                            "   Values (    '" & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_RefNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text))) & ",       @LoomDate,             " & Val(Sht_ID) & ",     " & Str(Val(vTotMtr)) & "," & Str(Val(vTotWrp)) & ",       " & Str(Val(vTotWeft)) & " ,    " & Str(Val(vTotEff)) & "   ,   " & Str(Val(vShift1)) & "   ,  " & Str(Val(vShift2)) & "    , " & Str(Val(vShift3)) & " , " & Str(Val(vDoff1_Mtrs)) & ", " & Str(Val(vDoff2_Mtrs)) & ", " & Str(Val(vDoff3_Mtrs)) & "   ," & Str(Val(vDoff4_Mtrs)) & " ," & Str(Val(vTot_Avg_Rpm)) & "," & Str(Val(vTot_Avg_Shift1_PICKEFF)) & "," & Str(Val(vTot_Shift1_Warp)) & "," & Str(Val(vTot_Shift1_Weft)) & "," & Str(Val(vTot_Avg_Shift2_PICKEFF)) & "," & Str(Val(vTot_Shift2_Warp)) & " ," & Str(Val(vTot_Shift2_Weft)) & "    ," & Str(Val(vTot_Amount)) & " ," & Str(Val(txt_EB_Units.Text)) & "  ," & Str(Val(txt_EB_Amount.Text)) & " ," & Str(Val(txt_Employee_Salary.Text)) & " ) "
                cmd.ExecuteNonQuery()

            Else
                Call Common_Procedures.User_Modification_Updation(con, "HEAD", Me.Name, "OLD", "LoomNo_Production_Head", "LoomNo_Production_Code", Val(lbl_Company.Tag), NewCode, lbl_RefNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, False, "", "", "LoomNo_Production_Code, Company_IdNo, for_OrderBy", tr)

                Call Common_Procedures.User_Modification_Updation(con, "DETAILS", Me.Name, "OLD", "LoomNo_Production_Details", "LoomNo_Production_Code", Val(lbl_Company.Tag), NewCode, lbl_RefNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, False, " Shift_IdNo   , Loom_IdNo, Meters   ,  Warp_Meters  ,   Weft_Meters  ,  Pick_Efficiency , Employee_IdNo", "Sl_No", "LoomNo_Production_Code, For_OrderBy, Company_IdNo, LoomNo_Production_No, LoomNo_Production_Date, Ledger_Idno", tr)

                cmd.CommandText = "Update LoomNo_Production_Head set Shift_IdNo = " & Val(Sht_ID) & ", LoomNo_Production_Date= @LoomDate ,  Total_Meters = " & Str(Val(vTotMtr)) & ",Total_WarpMeters = " & Str(Val(vTotWrp)) & " , Total_WeftMeters = " & Str(Val(vTotWeft)) & ", Total_PickEfficiency = " & Str(Val(vTotEff)) & " , Total_Shift1_Mtrs =  " & Str(Val(vShift1)) & "  ,   Total_Shift2_Mtrs  = " & Str(Val(vShift2)) & "  ,   Total_Shift3_Mtrs = " & Str(Val(vShift3)) & "  , Total_Doff1_Mtrs   = " & Str(Val(vDoff1_Mtrs)) & "  ,   Total_Doff2_Mtrs = " & Str(Val(vDoff2_Mtrs)) & "  ,  Total_Doff3_Mtrs = " & Str(Val(vDoff3_Mtrs)) & "  ,  Total_Doff4_Mtrs =  " & Str(Val(vDoff4_Mtrs)) & ",Total_Avg_RPM  =" & Str(Val(vTot_Avg_Rpm)) & ",Total_Avg_Shift_1_Pick_Efficiency =" & Str(Val(vTot_Avg_Shift1_PICKEFF)) & " , Total_Shift_1_Warp_Breakage=" & Str(Val(vTot_Shift1_Warp)) & " , Total_Shift_1_Weft_Breakage =" & Str(Val(vTot_Shift1_Weft)) & "  ,Total_Avg_Shift_2_Pick_Efficiency=" & Str(Val(vTot_Avg_Shift2_PICKEFF)) & "   ,Total_Shift_2_Warp_Breakage =" & Str(Val(vTot_Shift2_Warp)) & " ,Total_Shift_2_Weft_Breakage =" & Str(Val(vTot_Shift2_Weft)) & " ,  Total_Amount   =" & Str(Val(vTot_Amount)) & "  , EB_Units_Consumed  = " & Str(Val(txt_EB_Units.Text)) & " , EB_Amount  =  " & Str(Val(txt_EB_Amount.Text)) & " , Employee_Salary = " & Str(Val(txt_Employee_Salary.Text)) & "    Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and LoomNo_Production_Code = '" & Trim(NewCode) & "' "
                cmd.ExecuteNonQuery()

            End If
            Call Common_Procedures.User_Modification_Updation(con, "HEAD", Me.Name, "NEW", "LoomNo_Production_Head", "LoomNo_Production_Code", Val(lbl_Company.Tag), NewCode, lbl_RefNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, False, "", "", "LoomNo_Production_Code, Company_IdNo, for_OrderBy", tr)


            EntID = Trim(Pk_Condition) & Trim(lbl_RefNo.Text)
            PBlNo = Trim(lbl_RefNo.Text)
            Partcls = "Loom : Loom.No. " & Trim(lbl_RefNo.Text)

            cmd.CommandText = "Delete from LoomNo_Production_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and LoomNo_Production_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()



            With dgv_Details
                Sno = 0
                For i = 0 To dgv_Details.RowCount - 1

                    If Trim(dgv_Details.Rows(i).Cells(DgvCol_Details.LOOM_NO).Value) <> "" Or Val(dgv_Details.Rows(i).Cells(DgvCol_Details.TOTAL_PICK_EFFICIENCY).Value) <> 0 Then

                        Sno = Sno + 1

                        Lom_Id = Common_Procedures.Loom_NameToIdNo(con, dgv_Details.Rows(i).Cells(DgvCol_Details.LOOM_NO).Value, tr)
                        cloth_Idno = Common_Procedures.Cloth_NameToIdNo(con, dgv_Details.Rows(i).Cells(DgvCol_Details.CLOTH_NAME).Value, tr)
                        Emp_Id_Shift_1 = Common_Procedures.Employee_Simple_NameToIdNo(con, dgv_Details.Rows(i).Cells(DgvCol_Details.SHIFT_1_EMPLOYEE).Value, tr)
                        Emp_Id_Shift_2 = Common_Procedures.Employee_Simple_NameToIdNo(con, dgv_Details.Rows(i).Cells(DgvCol_Details.SHIFT_2_EMPLOYEE).Value, tr)



                        DofShit1_Id = Common_Procedures.Shift_NameToIdNo(con, dgv_Details.Rows(i).Cells(DgvCol_Details.DOFF_1_SHIFT).Value, tr)
                        DofShit2_Id = Common_Procedures.Shift_NameToIdNo(con, dgv_Details.Rows(i).Cells(DgvCol_Details.DOFF_2_SHIFT).Value, tr)
                        DofShit3_Id = Common_Procedures.Shift_NameToIdNo(con, dgv_Details.Rows(i).Cells(DgvCol_Details.DOFF_3_SHIFT).Value, tr)
                        DofShit4_Id = Common_Procedures.Shift_NameToIdNo(con, dgv_Details.Rows(i).Cells(DgvCol_Details.DOFF_4_SHIFT).Value, tr)

                        Led_ID = Common_Procedures.Ledger_AlaisNameToIdNo(con, dgv_Details.Rows(i).Cells(DgvCol_Details.PARTY_NAME).Value, tr)

                        vLASTDOFSTS = 0
                        vONLOOMFABMTRS = 0
                        vSHFTPRODMTRS = 0

                        If DofShit4_Id <> 0 And Val(.Rows(i).Cells(DgvCol_Details.DOFF_4_METERS).Value) > 0 Then

                            vLASTDOFSTS = 1

                            If DofShit4_Id = 7 Then
                                vSHFTPRODMTRS = Val(.Rows(i).Cells(DgvCol_Details.SHIFT_2).Value)
                            Else
                                vSHFTPRODMTRS = Val(.Rows(i).Cells(DgvCol_Details.SHIFT_1).Value)
                            End If

                            vONLOOMFABMTRS = Val(vSHFTPRODMTRS) - Val(.Rows(i).Cells(DgvCol_Details.DOFF_4_METERS).Value)

                        ElseIf DofShit3_Id <> 0 And Val(.Rows(i).Cells(DgvCol_Details.DOFF_3_METERS).Value) > 0 Then

                            vLASTDOFSTS = 1

                            If DofShit3_Id = 7 Then
                                vSHFTPRODMTRS = Val(.Rows(i).Cells(DgvCol_Details.SHIFT_2).Value)
                            Else
                                vSHFTPRODMTRS = Val(.Rows(i).Cells(DgvCol_Details.SHIFT_1).Value)
                            End If

                            vONLOOMFABMTRS = Val(vSHFTPRODMTRS) - Val(.Rows(i).Cells(DgvCol_Details.DOFF_3_METERS).Value)

                        ElseIf DofShit2_Id <> 0 And Val(.Rows(i).Cells(DgvCol_Details.DOFF_2_METERS).Value) > 0 Then

                            vLASTDOFSTS = 1

                            If DofShit2_Id = 7 Then
                                vSHFTPRODMTRS = Val(.Rows(i).Cells(DgvCol_Details.SHIFT_2).Value)
                            Else
                                vSHFTPRODMTRS = Val(.Rows(i).Cells(DgvCol_Details.SHIFT_1).Value)
                            End If

                            vONLOOMFABMTRS = Val(vSHFTPRODMTRS) - Val(.Rows(i).Cells(DgvCol_Details.DOFF_2_METERS).Value)

                        ElseIf DofShit1_Id <> 0 And Val(.Rows(i).Cells(DgvCol_Details.DOFF_1_METERS).Value) > 0 Then

                            vLASTDOFSTS = 1

                            If DofShit1_Id = 7 Then
                                vSHFTPRODMTRS = Val(.Rows(i).Cells(DgvCol_Details.SHIFT_2).Value)
                            Else
                                vSHFTPRODMTRS = Val(.Rows(i).Cells(DgvCol_Details.SHIFT_1).Value)
                            End If

                            vONLOOMFABMTRS = Val(vSHFTPRODMTRS) - Val(.Rows(i).Cells(DgvCol_Details.DOFF_1_METERS).Value)

                        Else

                            vONLOOMFABMTRS = Val(.Rows(i).Cells(DgvCol_Details.SHIFT_1).Value) + Val(.Rows(i).Cells(DgvCol_Details.SHIFT_2).Value)

                        End If

                        If Val(vONLOOMFABMTRS) < 0 Then
                            vONLOOMFABMTRS = 0
                        End If

                        cmd.CommandText = "Insert into LoomNo_Production_Details ( LoomNo_Production_Code    ,                              Company_IdNo                   ,                                        LoomNo_Production_No                   ,                        for_OrderBy                                     ,                             LoomNo_Production_Date                 ,                     Sl_No       ,                                     Shift_IdNo                                ,                            Loom_IdNo                              ,                                    Meters                            ,                         Warp_Meters                               ,                          Weft_Meters                            ,                             Pick_Efficiency                                 ,                             Employee_IdNo                           ,                            Shift1_Mtrs                          ,                               Shift2_Mtrs                           ,                                            Shift3_Mtrs           ,        Doff1_Shift_IdNo        ,                         Doff1_Mtrs                                    ,                     Doff2_Shift_IdNo    ,                Doff2_Mtrs                                             ,         Doff3_Shift_IdNo          ,                      Doff3_Mtrs                                           ,              Doff4_Shift_IdNo        ,                    Doff4_Mtrs                                         ,         Last_Doff_Status       ,         OnLoom_Fabric_Meters     , " &
                                                                          "          Cloth_IdNo              ,                                   RPM                       ,                                     Shift_1_Pick_Efficiency                   ,                       Shift_1_Warp_Breakage                            ,                               Shift_1_Weft_Breakage                ,        Employee_IdNo_Shift_1    ,                               Shift_2_Pick_Efficiency                         ,                        Shift_2_Warp_Breakage                      ,                           Shift_2_Weft_Breakage                      ,                        Employee_IdNo_Shift_2                      ,                          Remarks                                ,                             Ledger_Idno                                     ,                             Rate_Meter                              ,                            Amount     ) " &
                                                                          " Values ('" & Trim(NewCode) & "'  ,                        " & Str(Val(lbl_Company.Tag)) & "    ,                                 '" & Trim(lbl_RefNo.Text) & "'                , " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text))) & ",                                @LoomDate                           ,       " & Str(Val(Sno)) & "     ,                               " & Val(Sht_ID) & "                             ,                       " & Str(Val(Lom_Id)) & "                    ,  " & Str(Val(.Rows(i).Cells(DgvCol_Details.TOTAL_METERS).Value)) & " , " & Str(Val(.Rows(i).Cells(DgvCol_Details.TOTAL_WARP).Value)) & " ," & Str(Val(.Rows(i).Cells(DgvCol_Details.TOTAL_WEFT).Value)) & "," & Str(Val(.Rows(i).Cells(DgvCol_Details.TOTAL_PICK_EFFICIENCY).Value)) & " ,                           " & Val(Emp_Id) & "                       ,  " & Str(Val(.Rows(i).Cells(DgvCol_Details.SHIFT_1).Value)) & " ,   " & Str(Val(.Rows(i).Cells(DgvCol_Details.SHIFT_2).Value)) & "   ,   " & Str(Val(.Rows(i).Cells(DgvCol_Details.SHIFT_3).Value)) & " ,   " & Str(Val(DofShit1_Id)) & ",    " & Str(Val(.Rows(i).Cells(DgvCol_Details.DOFF_1_METERS).Value)) & ",           " & Str(Val(DofShit2_Id)) & " , " & Str(Val(.Rows(i).Cells(DgvCol_Details.DOFF_2_METERS).Value)) & " ,     " & Str(Val(DofShit3_Id)) & " ,     " & Str(Val(.Rows(i).Cells(DgvCol_Details.DOFF_3_METERS).Value)) & "    ,      " & Str(Val(DofShit4_Id)) & " , " & Str(Val(.Rows(i).Cells(DgvCol_Details.DOFF_4_METERS).Value)) & ",   " & Str(Val(vLASTDOFSTS)) & " , " & Str(Val(vONLOOMFABMTRS)) & "  , " &
                                                                          "     " & Str(Val(cloth_Idno)) & " ,  " & Str(Val(.Rows(i).Cells(DgvCol_Details.RPM).Value)) & " , " & Str(Val(.Rows(i).Cells(DgvCol_Details.SHIFT_1_PICK_EFFICIENCY).Value)) & ", " & Str(Val(.Rows(i).Cells(DgvCol_Details.SHIFT_1_WARP).Value)) & "    , " & Str(Val(.Rows(i).Cells(DgvCol_Details.SHIFT_1_WEFT).Value)) & "," & Str(Val(Emp_Id_Shift_1)) & " , " & Str(Val(.Rows(i).Cells(DgvCol_Details.SHIFT_2_PICK_EFFICIENCY).Value)) & "," & Str(Val(.Rows(i).Cells(DgvCol_Details.SHIFT_2_WARP).Value)) & "," & Str(Val(.Rows(i).Cells(DgvCol_Details.SHIFT_2_WEFT).Value)) & "   ,               " & Str(Val(Emp_Id_Shift_2)) & "                    ,'" & Trim(.Rows(i).Cells(DgvCol_Details.REMARKS).Value) & "'     ,                                            " & Str(Val(Led_ID)) & "         ,  " & Str(Val(.Rows(i).Cells(DgvCol_Details.RATE_METER).Value)) & "  ,  " & Str(Val(.Rows(i).Cells(DgvCol_Details.AMOUNT).Value)) & "  )"
                        cmd.ExecuteNonQuery()


                        'cmd.CommandText = "Insert into LoomNo_Production_Details ( LoomNo_Production_Code,   C      ompany_IdNo,                      LoomNo_Production_No,                        for_OrderBy,                                        LoomNo_Production_Date,    Sl_No,                  Shift_IdNo   ,          Loom_IdNo,                                             Meters   ,                                                 Warp_Meters  ,                                   Weft_Meters  ,                          Pick_Efficiency ,              Employee_IdNo       ,             Shift1_Mtrs     ,                               Shift2_Mtrs,                                            Shift3_Mtrs           ,   Doff1_Shift_IdNo    ,                         Doff1_Mtrs  ,                     Doff2_Shift_IdNo    ,                Doff2_Mtrs  ,                                   Doff3_Shift_IdNo    ,                      Doff3_Mtrs  ,                                Doff4_Shift_IdNo    ,                                Doff4_Mtrs  ,         Last_Doff_Status       ,         OnLoom_Fabric_Meters     ) " &
                        '                                                " Values ('" & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_RefNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text))) & ", @LoomDate,            " & Str(Val(Sno)) & ",     " & Val(Sht_ID) & " ," & Str(Val(Lom_Id)) & ",  " & Str(Val(.Rows(i).Cells(DgvCol_Details.TOTAL_METERS).Value)) & ", " & Str(Val(.Rows(i).Cells(DgvCol_Details.TOTAL_WARP).Value)) & "," & Str(Val(.Rows(i).Cells(DgvCol_Details.TOTAL_WEFT).Value)) & "," & Str(Val(.Rows(i).Cells(DgvCol_Details.TOTAL_PICK_EFFICIENCY).Value)) & ",   " & Val(Emp_Id) & " ,  " & Str(Val(.Rows(i).Cells(DgvCol_Details.SHIFT_1).Value)) & " ,   " & Str(Val(.Rows(i).Cells(DgvCol_Details.SHIFT_2).Value)) & "   ,   " & Str(Val(.Rows(i).Cells(DgvCol_Details.SHIFT_3).Value)) & " ,   " & Str(Val(DofShit1_Id)) & ",    " & Str(Val(.Rows(i).Cells(DgvCol_Details.DOFF_1_METERS).Value)) & ",  " & Str(Val(DofShit2_Id)) & " , " & Str(Val(.Rows(i).Cells(DgvCol_Details.DOFF_2_METERS).Value)) & " ,     " & Str(Val(DofShit3_Id)) & " ,     " & Str(Val(.Rows(i).Cells(DgvCol_Details.DOFF_3_METERS).Value)) & "    ,      " & Str(Val(DofShit4_Id)) & " , " & Str(Val(.Rows(i).Cells(DgvCol_Details.DOFF_4_METERS).Value)) & ", " & Str(Val(vLASTDOFSTS)) & " , " & Str(Val(vONLOOMFABMTRS)) & " ) "
                        'cmd.ExecuteNonQuery()

                    End If

                Next
                Call Common_Procedures.User_Modification_Updation(con, "DETAILS", Me.Name, "NEW", "LoomNo_Production_Details", "LoomNo_Production_Code", Val(lbl_Company.Tag), NewCode, lbl_RefNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, False, " Shift_IdNo   , Loom_IdNo, Meters   ,  Warp_Meters  ,   Weft_Meters  ,  Pick_Efficiency , Employee_IdNo", "Sl_No", "LoomNo_Production_Code, For_OrderBy, Company_IdNo, LoomNo_Production_No, LoomNo_Production_Date, Ledger_Idno", tr)

            End With

            tr.Commit()

            move_record(lbl_RefNo.Text, False)

            MessageBox.Show("Saved Sucessfully!!!", "FOR SAVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Information)

        Catch ex As Exception
            tr.Rollback()
            If InStr(1, Trim(LCase(ex.Message)), Trim(LCase("IX_LoomNo_Production_Head_1"))) > 0 Then
                MessageBox.Show("Duplicate Production Date", "FOR SAVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            ElseIf InStr(1, Trim(LCase(ex.Message)), Trim(LCase("IX_LoomNo_Production_Details_1"))) > 0 Then
                MessageBox.Show("Duplicate Loom No", "FOR SAVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            ElseIf InStr(1, Trim(LCase(ex.Message)), Trim(LCase("IX_LoomNo_Production_Details_2"))) > 0 Then
                MessageBox.Show("Duplicate Loom No", "FOR SAVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Else
                MessageBox.Show(ex.Message, "FOR SAVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            End If

        Finally

            Dt1.Dispose()
            Da.Dispose()
            tr.Dispose()

            If msk_Date.Enabled And msk_Date.Visible Then msk_Date.Focus()

        End Try

    End Sub

    Private Sub cbo_Grid_Loom_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Grid_Loom.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Loom_HEAD", "Loom_name", "", "(Loom_idno = 0)")
    End Sub

    Private Sub cbo_Grid_Loom_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Grid_Loom.KeyDown

        vcbo_KeyDwnVal = e.KeyValue

        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Grid_Loom, Nothing, Nothing, "Loom_HEAD", "Loom_name", "", "(Loom_idno = 0)")

        With dgv_Details

            If (e.KeyValue = 38 And cbo_Grid_Loom.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
                e.Handled = True
                If .CurrentRow.Index <= 0 Then
                    'cbo_Shift.Focus()
                    msk_Date.Focus()

                Else
                    .Focus()
                    .CurrentCell = dgv_Details.Rows(dgv_Details.CurrentRow.Index - 1).Cells(DgvCol_Details.DOFF_3_METERS)

                End If

            End If


            If (e.KeyValue = 40 And cbo_Grid_Loom.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
                e.Handled = True
                If Trim(.Rows(.CurrentRow.Index).Cells(DgvCol_Details.LOOM_NO).Value) = "" And .CurrentRow.Index = .Rows.Count - 1 Then
                    If MessageBox.Show("Do you want to save", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = DialogResult.Yes Then
                        save_record()
                    Else
                        msk_Date.Focus()
                    End If

                Else
                    ' .CurrentCell = .Rows(.CurrentRow.Index).Cells(7)
                    .Focus()
                    .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 6)

                End If
            End If

        End With
    End Sub

    Private Sub cbo_Grid_Loom_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Grid_Loom.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Grid_Loom, Nothing, "Loom_HEAD", "Loom_name", "", "(Loom_idno = 0)")

        'If Asc(e.KeyChar) = 13 Then

        '    e.Handled = True

        '    With dgv_Details
        '        If Trim(.Rows(.CurrentRow.Index).Cells(1).Value) = "" And .CurrentRow.Index = .Rows.Count - 1 Then
        '            If MessageBox.Show("Do you want to save", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = DialogResult.Yes Then
        '                save_record()
        '            Else
        '                msk_Date.Focus()
        '            End If

        '        Else
        '            .CurrentCell = .Rows(.CurrentRow.Index).Cells(7)

        '        End If
        '    End With
        'End If

        If Asc(e.KeyChar) = 13 Then

            With dgv_Details

                If Trim(.Rows(.CurrentRow.Index).Cells(DgvCol_Details.LOOM_NO).Value) = "" And .CurrentRow.Index = .Rows.Count - 1 Then
                    If MessageBox.Show("Do you want to save", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = DialogResult.Yes Then
                        save_record()
                    Else
                        msk_Date.Focus()
                    End If

                Else
                    '  .CurrentCell = .Rows(.CurrentRow.Index).Cells(7)
                    .Focus()
                    .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 6)

                End If

            End With

        End If

    End Sub


    Private Sub dgv_Details_CellEndEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs)
        Try
            With dgv_Details
                If .Visible = True Then
                    If .Rows.Count > 0 Then
                        dgv_Details_CellLeave(sender, e)

                    End If
                End If
            End With

        Catch ex As Exception
            MessageBox.Show(ex.Message, "ERROR WHILE DETAILS CELL END EDIT....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try


    End Sub

    Private Sub dgv_Details_CellEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellEnter
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim Dt2 As New DataTable
        Dim Dt3 As New DataTable
        Dim Dt4 As New DataTable
        Dim Dt5 As New DataTable
        Dim Dt6 As New DataTable
        Dim Dt7 As New DataTable
        Dim rect As Rectangle


        With dgv_Details

            If Val(.CurrentRow.Cells(DgvCol_Details.SL_NO).Value) = 0 Then
                .CurrentRow.Cells(DgvCol_Details.SL_NO).Value = .CurrentRow.Index + 1
            End If




            If e.ColumnIndex = 111 Then

                If cbo_Grid_Loom.Visible = False Or Val(cbo_Grid_Loom.Tag) <> e.RowIndex Then

                    cbo_Grid_Loom.Tag = -1
                    Da = New SqlClient.SqlDataAdapter("select Loom_Name from Loom_Head order by Loom_Name", con)
                    Dt1 = New DataTable
                    Da.Fill(Dt1)
                    cbo_Grid_Loom.DataSource = Dt1
                    cbo_Grid_Loom.DisplayMember = "Loom_Name"

                    rect = .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False)

                    cbo_Grid_Loom.Left = .Left + rect.Left
                    cbo_Grid_Loom.Top = .Top + rect.Top

                    cbo_Grid_Loom.Width = rect.Width
                    cbo_Grid_Loom.Height = rect.Height
                    cbo_Grid_Loom.Text = .CurrentCell.Value

                    cbo_Grid_Loom.Tag = Val(e.RowIndex)
                    cbo_Grid_Loom.Visible = True

                    cbo_Grid_Loom.BringToFront()
                    cbo_Grid_Loom.Focus()

                End If

            Else
                cbo_Grid_Loom.Visible = False

            End If


            If e.ColumnIndex = DgvCol_Details.CLOTH_NAME Then

                If Cbo_Grid_Cloth_Name.Visible = False Or Val(Cbo_Grid_Cloth_Name.Tag) <> e.RowIndex Then

                    Cbo_Grid_Cloth_Name.Tag = -1
                    Da = New SqlClient.SqlDataAdapter("select cloth_Name from Cloth_Head order by Cloth_Name", con)
                    Dt1 = New DataTable
                    Da.Fill(Dt2)

                    Cbo_Grid_Cloth_Name.DataSource = Dt2
                    Cbo_Grid_Cloth_Name.DisplayMember = "cloth_Name"

                    rect = .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False)

                    Cbo_Grid_Cloth_Name.Left = .Left + rect.Left
                    Cbo_Grid_Cloth_Name.Top = .Top + rect.Top

                    Cbo_Grid_Cloth_Name.Width = rect.Width
                    Cbo_Grid_Cloth_Name.Height = rect.Height
                    Cbo_Grid_Cloth_Name.Text = .CurrentCell.Value

                    Cbo_Grid_Cloth_Name.Tag = Val(e.RowIndex)
                    Cbo_Grid_Cloth_Name.Visible = True

                    Cbo_Grid_Cloth_Name.BringToFront()
                    Cbo_Grid_Cloth_Name.Focus()


                End If


            Else
                Cbo_Grid_Cloth_Name.Visible = False

            End If



            If e.ColumnIndex = DgvCol_Details.SHIFT_1_EMPLOYEE Then

                If cbo_Grid_Shift_1_Employee.Visible = False Or Val(cbo_Grid_Shift_1_Employee.Tag) <> e.RowIndex Then

                    cbo_Grid_Shift_1_Employee.Tag = -1
                    Da = New SqlClient.SqlDataAdapter("select Employee_Name from Employee_Head order by Employee_Name", con)
                    '     Da = New SqlClient.SqlDataAdapter("select Employee_Name from PayRoll_Employee_Head order by Employee_Name", con)
                    Dt1 = New DataTable
                    Da.Fill(Dt3)
                    cbo_Grid_Shift_1_Employee.DataSource = Dt3
                    cbo_Grid_Shift_1_Employee.DisplayMember = "Employee_Name"

                    rect = .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False)

                    cbo_Grid_Shift_1_Employee.Left = .Left + rect.Left
                    cbo_Grid_Shift_1_Employee.Top = .Top + rect.Top

                    cbo_Grid_Shift_1_Employee.Width = rect.Width
                    cbo_Grid_Shift_1_Employee.Height = rect.Height
                    cbo_Grid_Shift_1_Employee.Text = .CurrentCell.Value

                    cbo_Grid_Shift_1_Employee.Tag = Val(e.RowIndex)
                    cbo_Grid_Shift_1_Employee.Visible = True

                    cbo_Grid_Shift_1_Employee.BringToFront()
                    cbo_Grid_Shift_1_Employee.Focus()


                End If


            Else
                cbo_Grid_Shift_1_Employee.Visible = False

            End If



            If e.ColumnIndex = DgvCol_Details.SHIFT_2_EMPLOYEE Then

                If cbo_Grid_Shift_2_Employee.Visible = False Or Val(cbo_Grid_Shift_2_Employee.Tag) <> e.RowIndex Then

                    cbo_Grid_Shift_2_Employee.Tag = -1
                    Da = New SqlClient.SqlDataAdapter("select Employee_Name from Employee_Head order by Employee_Name", con)
                    '     Da = New SqlClient.SqlDataAdapter("select Employee_Name from PayRoll_Employee_Head order by Employee_Name", con)
                    Dt1 = New DataTable
                    Da.Fill(Dt4)
                    cbo_Grid_Shift_2_Employee.DataSource = Dt4
                    cbo_Grid_Shift_2_Employee.DisplayMember = "Employee_Name"

                    rect = .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False)

                    cbo_Grid_Shift_2_Employee.Left = .Left + rect.Left
                    cbo_Grid_Shift_2_Employee.Top = .Top + rect.Top

                    cbo_Grid_Shift_2_Employee.Width = rect.Width
                    cbo_Grid_Shift_2_Employee.Height = rect.Height
                    cbo_Grid_Shift_2_Employee.Text = .CurrentCell.Value

                    cbo_Grid_Shift_2_Employee.Tag = Val(e.RowIndex)
                    cbo_Grid_Shift_2_Employee.Visible = True

                    cbo_Grid_Shift_2_Employee.BringToFront()
                    cbo_Grid_Shift_2_Employee.Focus()


                End If


            Else
                cbo_Grid_Shift_2_Employee.Visible = False

            End If



            '--------


            If e.ColumnIndex = DgvCol_Details.DOFF_1_SHIFT Then

                If Cbo_Grid_Doff1.Visible = False Or Val(Cbo_Grid_Doff1.Tag) <> e.RowIndex Then

                    Cbo_Grid_Doff1.Tag = -1
                    Da = New SqlClient.SqlDataAdapter("select Shift_Name from Shift_Head order by Shift_Name", con)
                    Dt1 = New DataTable
                    Da.Fill(Dt1)
                    Cbo_Grid_Doff1.DataSource = Dt1
                    Cbo_Grid_Doff1.DisplayMember = "Loom_Name"

                    rect = .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False)

                    Cbo_Grid_Doff1.Left = .Left + rect.Left
                    Cbo_Grid_Doff1.Top = .Top + rect.Top

                    Cbo_Grid_Doff1.Width = rect.Width
                    Cbo_Grid_Doff1.Height = rect.Height
                    Cbo_Grid_Doff1.Text = .CurrentCell.Value

                    Cbo_Grid_Doff1.Tag = Val(e.RowIndex)
                    Cbo_Grid_Doff1.Visible = True

                    Cbo_Grid_Doff1.BringToFront()
                    Cbo_Grid_Doff1.Focus()

                End If

            Else
                Cbo_Grid_Doff1.Visible = False

            End If


            If e.ColumnIndex = DgvCol_Details.DOFF_2_SHIFT Then

                If Cbo_Grid_Doff2.Visible = False Or Val(Cbo_Grid_Doff2.Tag) <> e.RowIndex Then

                    Cbo_Grid_Doff2.Tag = -1
                    Da = New SqlClient.SqlDataAdapter("select Shift_Name from Shift_Head order by Shift_Name", con)
                    Dt1 = New DataTable
                    Da.Fill(Dt1)
                    Cbo_Grid_Doff2.DataSource = Dt1
                    Cbo_Grid_Doff2.DisplayMember = "Loom_Name"

                    rect = .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False)

                    Cbo_Grid_Doff2.Left = .Left + rect.Left
                    Cbo_Grid_Doff2.Top = .Top + rect.Top

                    Cbo_Grid_Doff2.Width = rect.Width
                    Cbo_Grid_Doff2.Height = rect.Height
                    Cbo_Grid_Doff2.Text = .CurrentCell.Value

                    Cbo_Grid_Doff2.Tag = Val(e.RowIndex)
                    Cbo_Grid_Doff2.Visible = True

                    Cbo_Grid_Doff2.BringToFront()
                    Cbo_Grid_Doff2.Focus()

                End If

            Else
                Cbo_Grid_Doff2.Visible = False

            End If


            If e.ColumnIndex = DgvCol_Details.DOFF_3_SHIFT Then

                If Cbo_Grid_Doff3.Visible = False Or Val(Cbo_Grid_Doff3.Tag) <> e.RowIndex Then

                    Cbo_Grid_Doff3.Tag = -1
                    Da = New SqlClient.SqlDataAdapter("select Shift_Name from Shift_Head order by Shift_Name", con)
                    Dt1 = New DataTable
                    Da.Fill(Dt1)
                    Cbo_Grid_Doff3.DataSource = Dt1
                    Cbo_Grid_Doff3.DisplayMember = "Loom_Name"

                    rect = .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False)

                    Cbo_Grid_Doff3.Left = .Left + rect.Left
                    Cbo_Grid_Doff3.Top = .Top + rect.Top

                    Cbo_Grid_Doff3.Width = rect.Width
                    Cbo_Grid_Doff3.Height = rect.Height
                    Cbo_Grid_Doff3.Text = .CurrentCell.Value

                    Cbo_Grid_Doff3.Tag = Val(e.RowIndex)
                    Cbo_Grid_Doff3.Visible = True

                    Cbo_Grid_Doff3.BringToFront()
                    Cbo_Grid_Doff3.Focus()

                End If

            Else
                Cbo_Grid_Doff3.Visible = False

            End If


            If e.ColumnIndex = DgvCol_Details.DOFF_4_SHIFT Then

                If Cbo_Grid_Doff4.Visible = False Or Val(Cbo_Grid_Doff4.Tag) <> e.RowIndex Then

                    Cbo_Grid_Doff4.Tag = -1
                    Da = New SqlClient.SqlDataAdapter("select Shift_Name from Shift_Head order by Shift_Name", con)
                    Dt1 = New DataTable
                    Da.Fill(Dt1)
                    Cbo_Grid_Doff4.DataSource = Dt1
                    Cbo_Grid_Doff4.DisplayMember = "Loom_Name"

                    rect = .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False)

                    Cbo_Grid_Doff4.Left = .Left + rect.Left
                    Cbo_Grid_Doff4.Top = .Top + rect.Top

                    Cbo_Grid_Doff4.Width = rect.Width
                    Cbo_Grid_Doff4.Height = rect.Height
                    Cbo_Grid_Doff4.Text = .CurrentCell.Value

                    Cbo_Grid_Doff4.Tag = Val(e.RowIndex)
                    Cbo_Grid_Doff4.Visible = True

                    Cbo_Grid_Doff4.BringToFront()
                    Cbo_Grid_Doff4.Focus()

                End If

            Else
                Cbo_Grid_Doff4.Visible = False

            End If


            If e.ColumnIndex = DgvCol_Details.PARTY_NAME Then

                If Cbo_Grid_Ledger_Name.Visible = False Or Val(Cbo_Grid_Ledger_Name.Tag) <> e.RowIndex Then

                    Cbo_Grid_Ledger_Name.Tag = -1
                    Da = New SqlClient.SqlDataAdapter("select Ledger_Name from Ledger_Head order by Ledger_Name ", con)
                    Dim Dt8 = New DataTable
                    Da.Fill(Dt6)

                    Cbo_Grid_Ledger_Name.DataSource = Dt8
                    Cbo_Grid_Ledger_Name.DisplayMember = "Ledger_Name"

                    rect = .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False)

                    Cbo_Grid_Ledger_Name.Left = .Left + rect.Left
                    Cbo_Grid_Ledger_Name.Top = .Top + rect.Top

                    Cbo_Grid_Ledger_Name.Width = rect.Width
                    Cbo_Grid_Ledger_Name.Height = rect.Height
                    Cbo_Grid_Ledger_Name.Text = .CurrentCell.Value

                    Cbo_Grid_Ledger_Name.Tag = Val(e.RowIndex)
                    Cbo_Grid_Ledger_Name.Visible = True

                    Cbo_Grid_Ledger_Name.BringToFront()
                    Cbo_Grid_Ledger_Name.Focus()


                End If


            Else
                Cbo_Grid_Ledger_Name.Visible = False

            End If


        End With
    End Sub
    Private Sub dgv_Details_CellLeave(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellLeave
        Try
            With dgv_Details
                If .Visible = True Then
                    If .Rows.Count > 0 Then

                        If .CurrentCell.ColumnIndex <> DgvCol_Details.LOOM_NO And .CurrentCell.ColumnIndex <> DgvCol_Details.CLOTH_NAME And .CurrentCell.ColumnIndex <> DgvCol_Details.RPM And .CurrentCell.ColumnIndex <> DgvCol_Details.SHIFT_1_EMPLOYEE And .CurrentCell.ColumnIndex <> DgvCol_Details.SHIFT_2_EMPLOYEE And .CurrentCell.ColumnIndex <> DgvCol_Details.REMARKS And .CurrentCell.ColumnIndex <> DgvCol_Details.PARTY_NAME Then

                            If Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value) <> 0 Then
                                .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = Format(Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value), "#########0.00")
                                'Else
                                '    .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = ""
                            End If
                        End If
                    End If
                End If

            End With

        Catch ex As Exception
            MessageBox.Show(ex.Message, "ERROR WHILE DETAILS CELL LEAVE....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub
    Private Sub dgv_Details_CellValueChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellValueChanged
        On Error Resume Next

        If FrmLdSTS = True Or NoCalc_Status = True Or Mov_Status = True Then Exit Sub

        If IsNothing(dgv_Details.CurrentCell) Then Exit Sub

        With dgv_Details
            If .Visible Then
                If .CurrentCell.ColumnIndex <> DgvCol_Details.LOOM_NO And .CurrentCell.ColumnIndex <> DgvCol_Details.CLOTH_NAME And .CurrentCell.ColumnIndex <> DgvCol_Details.SHIFT_1_EMPLOYEE And .CurrentCell.ColumnIndex <> DgvCol_Details.SHIFT_2_EMPLOYEE And .CurrentCell.ColumnIndex <> DgvCol_Details.PARTY_NAME Then

                    .Rows(e.RowIndex).Cells(DgvCol_Details.TOTAL_METERS).Value = Format(Val(.Rows(e.RowIndex).Cells(DgvCol_Details.SHIFT_1).Value) + Val(.Rows(e.RowIndex).Cells(DgvCol_Details.SHIFT_2).Value) + Val(.Rows(e.RowIndex).Cells(DgvCol_Details.SHIFT_3).Value), "###########0.00")
                    .Rows(e.RowIndex).Cells(DgvCol_Details.TOTAL_PICK_EFFICIENCY).Value = Format(Val(Val(.Rows(e.RowIndex).Cells(DgvCol_Details.SHIFT_1_PICK_EFFICIENCY).Value) + Val(.Rows(e.RowIndex).Cells(DgvCol_Details.SHIFT_2_PICK_EFFICIENCY).Value)) / 2, "###########0.00")
                    .Rows(e.RowIndex).Cells(DgvCol_Details.TOTAL_WARP).Value = Format(Val(.Rows(e.RowIndex).Cells(DgvCol_Details.SHIFT_1_WARP).Value) + Val(.Rows(e.RowIndex).Cells(DgvCol_Details.SHIFT_2_WARP).Value), "###########0.00")
                    .Rows(e.RowIndex).Cells(DgvCol_Details.TOTAL_WEFT).Value = Format(Val(.Rows(e.RowIndex).Cells(DgvCol_Details.SHIFT_1_WEFT).Value) + Val(.Rows(e.RowIndex).Cells(DgvCol_Details.SHIFT_2_WEFT).Value), "###########0.00")

                    If Val(.Rows(e.RowIndex).Cells(DgvCol_Details.RATE_METER).Value) <> 0 Or Val(.Rows(e.RowIndex).Cells(DgvCol_Details.TOTAL_METERS).Value) <> 0 Then

                        .Rows(e.RowIndex).Cells(DgvCol_Details.AMOUNT).Value = Format(Val(.Rows(e.RowIndex).Cells(DgvCol_Details.TOTAL_METERS).Value) * Val(.Rows(e.RowIndex).Cells(DgvCol_Details.RATE_METER).Value), "##########0.00")

                    End If

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
    End Sub
    Private Sub dgtxt_Details_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dgtxt_Details.KeyPress
        With dgv_Details
            If .Visible Then
                ' If .CurrentCell.ColumnIndex = DgvCol_Details.WARP Or .CurrentCell.ColumnIndex = DgvCol_Details.WEFT Or .CurrentCell.ColumnIndex = DgvCol_Details.PICK_EFFICIENCY Or .CurrentCell.ColumnIndex = DgvCol_Details.SHIFT_1 Or .CurrentCell.ColumnIndex = DgvCol_Details.SHIFT_2 Or .CurrentCell.ColumnIndex = DgvCol_Details.SHIFT_3 Or .CurrentCell.ColumnIndex = DgvCol_Details.DOFF_1_METERS Or .CurrentCell.ColumnIndex = DgvCol_Details.DOFF_2_METERS Or .CurrentCell.ColumnIndex = DgvCol_Details.DOFF_3_METERS Or .CurrentCell.ColumnIndex = DgvCol_Details.DOFF_4_METERS Then
                If .CurrentCell.ColumnIndex <> DgvCol_Details.LOOM_NO And .CurrentCell.ColumnIndex <> DgvCol_Details.CLOTH_NAME And .CurrentCell.ColumnIndex <> DgvCol_Details.SHIFT_1_EMPLOYEE And .CurrentCell.ColumnIndex <> DgvCol_Details.SHIFT_2_EMPLOYEE And .CurrentCell.ColumnIndex <> DgvCol_Details.REMARKS And .CurrentCell.ColumnIndex <> DgvCol_Details.PARTY_NAME Then

                    If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
                        e.Handled = True
                    End If

                End If
            End If
        End With

    End Sub



    Private Sub dgv_Details_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_Details.KeyUp
        Dim n As Integer
        Dim nrw As Integer
        Dim LMNO As String
        Dim I As Integer

        If e.Control = True And (UCase(Chr(e.KeyCode)) = "A" Or UCase(Chr(e.KeyCode)) = "I" Or e.KeyCode = Keys.Insert) Then

            With dgv_Details

                n = .CurrentRow.Index

                LMNO = Trim(UCase(.Rows(n).Cells(DgvCol_Details.LOOM_NO).Value))

                nrw = n + 1

                dgv_Details.Rows.Insert(nrw, 1)

                dgv_Details.Rows(nrw).Cells(DgvCol_Details.LOOM_NO).Value = Trim(UCase(LMNO))

                For I = 0 To dgv_Details.Rows.Count - 1
                    dgv_Details.Rows(I).Cells(DgvCol_Details.SL_NO).Value = I + 1
                Next I

            End With

        ElseIf e.Control = True And UCase(Chr(e.KeyCode)) = "D" Then

            With dgv_Details

                n = .CurrentRow.Index

                If .Rows.Count = 1 Then
                    For i = 0 To .Columns.Count - 1
                        .Rows(n).Cells(i).Value = ""
                    Next

                Else

                    .Rows.RemoveAt(n)

                End If

                For i = 0 To .Rows.Count - 1
                    .Rows(i).Cells(DgvCol_Details.SL_NO).Value = i + 1
                Next

            End With

            Total_Calculation()

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
            .Rows(n - 1).Cells(DgvCol_Details.SL_NO).Value = Val(n)
        End With
    End Sub

    Private Sub cbo_Shift_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Shift.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Shift_Head", "Shift_Name", "", "(Shift_idno = 0)")
    End Sub

    Private Sub cbo_Shift_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Shift.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Shift, msk_Date, Nothing, "Shift_Head", "Shift_Name", "", "(Shift_idno = 0)")
        If (e.KeyValue = 40 And cbo_Shift.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
            If dgv_Details.Rows.Count > 0 Then


                dgv_Details.Focus()
                dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(DgvCol_Details.LOOM_NO)
            Else
                If MessageBox.Show("Do you want to save ?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = DialogResult.Yes Then
                    save_record()
                Else

                    dtp_Date.Focus()
                End If
            End If
        End If
    End Sub

    Private Sub cbo_Shift_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Shift.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Shift, Nothing, "Shift_Head", "Shift_Name", "", "(Shift_idno = 0)")
        If Asc(e.KeyChar) = 13 Then
            If dgv_Details.Rows.Count > 0 Then
                dgv_Details.Focus()
                dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(DgvCol_Details.LOOM_NO)
            Else

                If MessageBox.Show("Do you want to save ?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = DialogResult.Yes Then
                    save_record()
                Else

                    dtp_Date.Focus()
                End If
            End If
        End If
    End Sub



    Private Sub cbo_Grid_Shift_1_Employee_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Grid_Shift_1_Employee.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Employee_Head", "Employee_name", "", "(Employee_idno = 0)")
    End Sub




    Private Sub cbo_Grid_Shift_1_Employee_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Grid_Shift_1_Employee.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Grid_Shift_1_Employee, Nothing, Nothing, "Employee_Head", "Employee_name", "", "(Employee_idno = 0)")
        With dgv_Details

            If (e.KeyValue = 38 And cbo_Grid_Shift_1_Employee.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then

                dgv_Details.Focus()
                dgv_Details.CurrentCell = dgv_Details.Rows(dgv_Details.CurrentRow.Index).Cells(DgvCol_Details.SHIFT_1_WEFT)

            End If

            If (e.KeyValue = 40 And cbo_Grid_Shift_1_Employee.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then

                'If .CurrentRow.Index = .Rows.Count - 1 Then

                '    If MessageBox.Show("Do you want to save", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = DialogResult.Yes Then
                '        save_record()
                '    Else
                '        dtp_Date.Focus()
                '    End If

                'Else
                dgv_Details.Focus()

                dgv_Details.CurrentCell = dgv_Details.Rows(dgv_Details.CurrentRow.Index).Cells(DgvCol_Details.SHIFT_2_PICK_EFFICIENCY)

                '   End If

            End If

        End With
    End Sub

    Private Sub cbo_Grid_Shift_1_Employee_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Grid_Shift_1_Employee.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Grid_Shift_1_Employee, Nothing, "Employee_Head", "Employee_name", "", "(Employee_idno = 0)")
        If Asc(e.KeyChar) = 13 Then
            With dgv_Details
                'If Trim(.Rows(.CurrentRow.Index).Cells(DgvCol_Details.SHIFT_1_EMPLOYEE).Value) = "" And .CurrentRow.Index = .Rows.Count - 1 Then
                '    If MessageBox.Show("Do you want to save", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = DialogResult.Yes Then
                '        save_record()
                '    Else
                '        dtp_Date.Focus()
                '    End If

                'Else
                dgv_Details.Focus()
                dgv_Details.CurrentCell = dgv_Details.Rows(dgv_Details.CurrentRow.Index).Cells(DgvCol_Details.SHIFT_2_PICK_EFFICIENCY)
                'dgv_Details.CurrentCell = dgv_Details.Rows(dgv_Details.CurrentRow.Index + 1).Cells(DgvCol_Details.LOOM_NO)

                '  End If
            End With
        End If
    End Sub

    Private Sub cbo_Grid_Shift_1_Employee_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Grid_Shift_1_Employee.KeyUp
        'If e.Control = True And UCase(Chr(e.KeyCode)) = "D" Then
        '    dgv_Details_KeyUp(sender, e)
        'End If
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Dim f As New EmployeeCreation_Simple
            ' Dim f As New Payroll_Employee_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Grid_Shift_1_Employee.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()
        End If
    End Sub


    Private Sub cbo_Grid_Shift_1_Employee_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Grid_Shift_1_Employee.TextChanged
        Try
            If cbo_Grid_Shift_1_Employee.Visible Then

                If IsNothing(dgv_Details.CurrentCell) Then Exit Sub
                With dgv_Details
                    If Val(cbo_Grid_Shift_1_Employee.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = DgvCol_Details.SHIFT_1_EMPLOYEE Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_Grid_Shift_1_Employee.Text)
                    End If
                End With
            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub btn_Filter_Close_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Filter_Close.Click
        pnl_Back.Enabled = True
        pnl_Filter.Visible = False
        Filter_Status = False
    End Sub

    Private Sub btn_Filter_Show_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Filter_Show.Click
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim n As Integer, i As Integer
        Dim Lom_IdNo As Integer, Sht_IdNo As Integer
        Dim Condt As String = ""

        Try

            Condt = ""
            Lom_IdNo = 0
            Sht_IdNo = 0

            If IsDate(dtp_Filter_Fromdate.Value) = True And IsDate(dtp_Filter_ToDate.Value) = True Then
                Condt = "a.LoomNo_Production_Date between '" & Trim(Format(dtp_Filter_Fromdate.Value, "MM/dd/yyyy")) & "' and '" & Trim(Format(dtp_Filter_ToDate.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_Filter_Fromdate.Value) = True Then
                Condt = "a.LoomNo_Production_Date = '" & Trim(Format(dtp_Filter_Fromdate.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_Filter_ToDate.Value) = True Then
                Condt = "a.LoomNo_Production_Date = '" & Trim(Format(dtp_Filter_ToDate.Value, "MM/dd/yyyy")) & "' "
            End If

            If Trim(cbo_Filter_Loom.Text) <> "" Then
                Lom_IdNo = Common_Procedures.Loom_NameToIdNo(con, cbo_Filter_Loom.Text)
            End If

            If Val(Lom_IdNo) <> 0 Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " b.Loom_IdNo = " & Str(Val(Lom_IdNo))
            End If

            If Trim(cbo_Filter_Shift.Text) <> "" Then
                Sht_IdNo = Common_Procedures.Shift_NameToIdNo(con, cbo_Filter_Shift.Text)
            End If

            If Val(Sht_IdNo) <> 0 Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " a.Shift_IdNo = " & Str(Val(Sht_IdNo))
            End If


            da = New SqlClient.SqlDataAdapter("select a.*, b.*, c.Loom_name, d.Employee_name from LoomNo_Production_Head a INNER join LoomNo_Production_Details b on a.LoomNo_Production_Code = b.LoomNo_Production_Code left outer join Loom_head c on b.Loom_idno = c.Loom_idno left outer join PayRoll_Employee_head d on b.Employee_idno = d.Employee_idno  where a.company_idno =" & Str(Val(lbl_Company.Tag)) & "and a.LoomNo_Production_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' " & IIf(Trim(Condt) <> "", " and ", "") & Condt & " Order by a.LoomNo_Production_Date, a.for_orderby, a.LoomNo_Production_No", con)
            da.Fill(dt2)

            dgv_Filter_Details.Rows.Clear()

            If dt2.Rows.Count > 0 Then

                For i = 0 To dt2.Rows.Count - 1

                    n = dgv_Filter_Details.Rows.Add()


                    dgv_Filter_Details.Rows(n).Cells(0).Value = dt2.Rows(i).Item("LoomNo_Production_No").ToString
                    dgv_Filter_Details.Rows(n).Cells(1).Value = dt2.Rows(i).Item("LoomNo_Production_Date").ToString
                    dgv_Filter_Details.Rows(n).Cells(2).Value = Common_Procedures.Shift_IdNoToName(con, dt2.Rows(i).Item("Shift_Idno").ToString)
                    dgv_Filter_Details.Rows(n).Cells(3).Value = dt2.Rows(i).Item("Loom_Name").ToString
                    dgv_Filter_Details.Rows(n).Cells(4).Value = Format(Val(dt2.Rows(i).Item("Meters").ToString), "########0.00")
                    'dgv_Filter_Details.Rows(n).Cells(5).Value = Format(Val(dt2.Rows(i).Item("Warp_Meters").ToString), "########0.00")
                    'dgv_Filter_Details.Rows(n).Cells(6).Value = Format(Val(dt2.Rows(i).Item("Weft_Meters").ToString), "########0.00")
                    'dgv_Filter_Details.Rows(n).Cells(7).Value = Format(Val(dt2.Rows(i).Item("Pick_Efficiency").ToString), "########0.00")
                    'dgv_Filter_Details.Rows(n).Cells(8).Value = dt2.Rows(i).Item("Employee_Name").ToString

                Next i

            End If

            dt2.Clear()
            dt2.Dispose()
            da.Dispose()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT FILTER...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        If dgv_Filter_Details.Visible And dgv_Filter_Details.Enabled Then dgv_Filter_Details.Focus()
    End Sub

    Private Sub cbo_Filter_Item_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Filter_Loom.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Loom_Head", "Loom_Name", "", "(Loom_idno = 0)")
    End Sub

    Private Sub cbo_Filter_Item_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Filter_Loom.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_Loom, dtp_Filter_ToDate, btn_Filter_Show, "Loom_Head", "Loom_Name", "", "(Loom_idno = 0)")
    End Sub


    Private Sub cbo_Filter_Item_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Filter_Loom.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Filter_Loom, btn_Filter_Show, "Loom_Head", "Loom_Name", "", "(Loom_idno = 0)")
    End Sub



    Private Sub Open_FilterEntry()
        Dim movno As String

        On Error Resume Next

        movno = Trim(dgv_Filter_Details.CurrentRow.Cells(0).Value)

        If Val(movno) <> 0 Then
            Filter_Status = True
            move_record(movno, False)
            pnl_Back.Enabled = True
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

    Private Sub cbo_Grid_Loom_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Grid_Loom.KeyUp
        'If e.Control = True And UCase(Chr(e.KeyCode)) = "D" Then
        '    dgv_Details_KeyUp(sender, e)
        'End If
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Dim f As New LoomNo_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Grid_Loom.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()
        End If
    End Sub



    Private Sub cbo_Grid_Loom_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Grid_Loom.TextChanged
        Try
            If cbo_Grid_Loom.Visible Then

                If IsNothing(dgv_Details.CurrentCell) Then Exit Sub
                With dgv_Details
                    If Val(cbo_Grid_Loom.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = DgvCol_Details.LOOM_NO Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_Grid_Loom.Text)
                    End If
                End With
            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub



    Private Sub Total_Calculation()
        Dim Sno As Integer
        Dim TotMtr As String
        Dim TotWrp As String
        Dim TotWft As String
        Dim TotEff As String

        Dim vShift1 As String = 0
        Dim vShift2 As String = 0
        Dim vShift3 As String = 0

        Dim vDoff1_Mtrs As String = 0
        Dim vDoff2_Mtrs As String = 0
        Dim vDoff3_Mtrs As String = 0
        Dim vDoff4_Mtrs As String = 0


        Dim vRPM As String = 0
        Dim VSHFT_1_PICK_EFF As String = 0
        Dim VSHFT_1_METERS As String = 0
        Dim VSHFT_1_WARP As String = 0
        Dim VSHFT_1_WEFT As String = 0
        Dim VSHFT_2_PICK_EFF As String = 0
        Dim VSHFT_2_METERS As String = 0
        Dim VSHFT_2_WARP As String = 0
        Dim VSHFT_2_WEFT As String = 0
        Dim TOTAL_PICK_EFF As String = 0
        Dim TOTAL_METERS As String = 0
        Dim TOTAL_WARP As String = 0
        Dim TOTAL_WEFT As String = 0
        Dim TOTAL_AMOUNT As String = 0


        Dim vAVG_RPM_Cul As String = 0
        Dim VAVG_SHFT_1_EFF_Cul As String = 0
        Dim VAVG_SHFT_2_EFF_Cul As String = 0
        Dim VAVG_TOTAL_EFF_Cul As String = 0

        Sno = 0
        TotMtr = 0 : TotWrp = 0 : TotWft = 0 : TotEff = 0 : TOTAL_AMOUNT = 0

        If FrmLdSTS = True Or NoCalc_Status = True Or Mov_Status = True Then Exit Sub

        With dgv_Details
            For i = 0 To .RowCount - 1
                Sno = Sno + 1
                .Rows(i).Cells(DgvCol_Details.SL_NO).Value = Sno

                If Val(.Rows(i).Cells(DgvCol_Details.TOTAL_METERS).Value) <> 0 Or Val(.Rows(i).Cells(DgvCol_Details.RPM).Value) <> 0 Or Val(.Rows(i).Cells(DgvCol_Details.SHIFT_1_PICK_EFFICIENCY).Value) <> 0 Or Val(.Rows(i).Cells(DgvCol_Details.SHIFT_1).Value) <> 0 Or Val(.Rows(i).Cells(DgvCol_Details.SHIFT_2).Value) <> 0 Or Val(.Rows(i).Cells(DgvCol_Details.SHIFT_2_PICK_EFFICIENCY).Value) <> 0 Or Val(.Rows(i).Cells(DgvCol_Details.SHIFT_3).Value) <> 0 Or Val(.Rows(i).Cells(DgvCol_Details.DOFF_1_METERS).Value) <> 0 Or Val(.Rows(i).Cells(DgvCol_Details.DOFF_2_METERS).Value) <> 0 Or Val(.Rows(i).Cells(DgvCol_Details.DOFF_3_METERS).Value) <> 0 Or Val(.Rows(i).Cells(DgvCol_Details.DOFF_4_METERS).Value) <> 0 Then


                    If Val(.Rows(i).Cells(DgvCol_Details.RPM).Value) <> 0 Then
                        vRPM = vRPM + Val(.Rows(i).Cells(DgvCol_Details.RPM).Value)
                        vAVG_RPM_Cul = vAVG_RPM_Cul + 1
                    End If

                    If Val(.Rows(i).Cells(DgvCol_Details.SHIFT_1_PICK_EFFICIENCY).Value) <> 0 Then
                        VSHFT_1_PICK_EFF = VSHFT_1_PICK_EFF + Val(.Rows(i).Cells(DgvCol_Details.SHIFT_1_PICK_EFFICIENCY).Value)
                        VAVG_SHFT_1_EFF_Cul = VAVG_SHFT_1_EFF_Cul + 1
                    End If

                    VSHFT_1_METERS = VSHFT_1_METERS + Val(.Rows(i).Cells(DgvCol_Details.SHIFT_1).Value)
                    VSHFT_1_WARP = VSHFT_1_WARP + Val(.Rows(i).Cells(DgvCol_Details.SHIFT_1_WARP).Value)
                    VSHFT_1_WEFT = VSHFT_1_WEFT + Val(.Rows(i).Cells(DgvCol_Details.SHIFT_1_WEFT).Value)

                    If Val(.Rows(i).Cells(DgvCol_Details.SHIFT_2_PICK_EFFICIENCY).Value) <> 0 Then
                        VSHFT_2_PICK_EFF = VSHFT_2_PICK_EFF + Val(.Rows(i).Cells(DgvCol_Details.SHIFT_2_PICK_EFFICIENCY).Value)
                        VAVG_SHFT_2_EFF_Cul = VAVG_SHFT_2_EFF_Cul + 1
                    End If

                    VSHFT_2_METERS = VSHFT_2_METERS + Val(.Rows(i).Cells(DgvCol_Details.SHIFT_2).Value)
                    VSHFT_2_WARP = VSHFT_2_WARP + Val(.Rows(i).Cells(DgvCol_Details.SHIFT_2_WARP).Value)
                    VSHFT_2_WEFT = VSHFT_2_WEFT + Val(.Rows(i).Cells(DgvCol_Details.SHIFT_2_WEFT).Value)

                    vShift3 = vShift3 + Val(.Rows(i).Cells(DgvCol_Details.SHIFT_3).Value)

                    If Val(.Rows(i).Cells(DgvCol_Details.TOTAL_PICK_EFFICIENCY).Value) <> 0 Then
                        TOTAL_PICK_EFF = TOTAL_PICK_EFF + Val(.Rows(i).Cells(DgvCol_Details.TOTAL_PICK_EFFICIENCY).Value)
                        VAVG_TOTAL_EFF_Cul = VAVG_TOTAL_EFF_Cul + 1
                    End If

                    TOTAL_METERS = TOTAL_METERS + Val(.Rows(i).Cells(DgvCol_Details.TOTAL_METERS).Value)
                    TOTAL_WARP = TOTAL_WARP + Val(.Rows(i).Cells(DgvCol_Details.TOTAL_WARP).Value)
                    TOTAL_WEFT = TOTAL_WEFT + Val(.Rows(i).Cells(DgvCol_Details.TOTAL_WEFT).Value)



                    vDoff1_Mtrs = vDoff1_Mtrs + Val(.Rows(i).Cells(DgvCol_Details.DOFF_1_METERS).Value)
                    vDoff2_Mtrs = vDoff2_Mtrs + Val(.Rows(i).Cells(DgvCol_Details.DOFF_2_METERS).Value)
                    vDoff3_Mtrs = vDoff3_Mtrs + Val(.Rows(i).Cells(DgvCol_Details.DOFF_3_METERS).Value)
                    vDoff4_Mtrs = vDoff4_Mtrs + Val(.Rows(i).Cells(DgvCol_Details.DOFF_4_METERS).Value)

                    TOTAL_AMOUNT = TOTAL_AMOUNT + Val(.Rows(i).Cells(DgvCol_Details.AMOUNT).Value)



                    'TotMtr = TotMtr + Val(.Rows(i).Cells(DgvCol_Details.TOTAL_METERS).Value)
                    'TotWrp = TotWrp + Val(.Rows(i).Cells(DgvCol_Details.WARP).Value)
                    'TotWft = TotWft + Val(.Rows(i).Cells(DgvCol_Details.WEFT).Value)
                    'TotEff = TotEff + Val(.Rows(i).Cells(DgvCol_Details.PICK_EFFICIENCY).Value)

                    'vShift1 = vShift1 + Val(.Rows(i).Cells(DgvCol_Details.SHIFT_1).Value)
                    'vShift2 = vShift2 + Val(.Rows(i).Cells(DgvCol_Details.SHIFT_2).Value)
                    'vShift3 = vShift3 + Val(.Rows(i).Cells(DgvCol_Details.SHIFT_3).Value)

                    'vDoff1_Mtrs = vDoff1_Mtrs + Val(.Rows(i).Cells(DgvCol_Details.DOFF_1_METERS).Value)
                    'vDoff2_Mtrs = vDoff2_Mtrs + Val(.Rows(i).Cells(DgvCol_Details.DOFF_2_METERS).Value)
                    'vDoff3_Mtrs = vDoff3_Mtrs + Val(.Rows(i).Cells(DgvCol_Details.DOFF_3_METERS).Value)
                    'vDoff4_Mtrs = vDoff4_Mtrs + Val(.Rows(i).Cells(DgvCol_Details.DOFF_4_METERS).Value)



                End If
            Next
        End With

        With dgv_Details_Total

            If .RowCount = 0 Then .Rows.Add()

            .Rows(0).Cells(DgvCol_Details.RPM).Value = Format(Val(vRPM) / Val(vAVG_RPM_Cul), "########0.00")
            .Rows(0).Cells(DgvCol_Details.SHIFT_1_PICK_EFFICIENCY).Value = Format(Val(VSHFT_1_PICK_EFF) / Val(VAVG_SHFT_1_EFF_Cul), "########0.00")
            .Rows(0).Cells(DgvCol_Details.SHIFT_1).Value = Format(Val(VSHFT_1_METERS), "########0.00")
            .Rows(0).Cells(DgvCol_Details.SHIFT_1_WARP).Value = Format(Val(VSHFT_1_WARP), "########0.00")
            .Rows(0).Cells(DgvCol_Details.SHIFT_1_WEFT).Value = Format(Val(VSHFT_1_WEFT), "########0.00")

            .Rows(0).Cells(DgvCol_Details.SHIFT_2_PICK_EFFICIENCY).Value = Format(Val(VSHFT_2_PICK_EFF) / Val(VAVG_SHFT_2_EFF_Cul), "########0.00")
            .Rows(0).Cells(DgvCol_Details.SHIFT_2).Value = Format(Val(VSHFT_2_METERS), "########0.00")
            .Rows(0).Cells(DgvCol_Details.SHIFT_2_WARP).Value = Format(Val(VSHFT_2_WARP), "########0.00")
            .Rows(0).Cells(DgvCol_Details.SHIFT_2_WEFT).Value = Format(Val(VSHFT_2_WEFT), "########0.00")

            .Rows(0).Cells(DgvCol_Details.SHIFT_3).Value = Format(Val(vShift3), "########0.00")

            .Rows(0).Cells(DgvCol_Details.TOTAL_PICK_EFFICIENCY).Value = Format(Val(TOTAL_PICK_EFF) / Val(VAVG_TOTAL_EFF_Cul), "########0.00")
            .Rows(0).Cells(DgvCol_Details.TOTAL_METERS).Value = Format(Val(TOTAL_METERS), "########0.00")
            .Rows(0).Cells(DgvCol_Details.TOTAL_WARP).Value = Format(Val(TOTAL_WARP), "########0.00")
            .Rows(0).Cells(DgvCol_Details.TOTAL_WEFT).Value = Format(Val(TOTAL_WEFT), "########0.00")

            .Rows(0).Cells(DgvCol_Details.AMOUNT).Value = Format(Val(TOTAL_AMOUNT), "########0.00")

            '.Rows(0).Cells(DgvCol_Details.TOTAL_METERS).Value = Format(Val(TotMtr), "########0.00")
            '.Rows(0).Cells(DgvCol_Details.TOTAL_WARP).Value = Format(Val(TotWrp), "########0.00")
            '.Rows(0).Cells(DgvCol_Details.TOTAL_WEFT).Value = Format(Val(TotWft), "########0.00")
            '.Rows(0).Cells(DgvCol_Details.TOTAL_PICK_EFFICIENCY).Value = Format(Val(TotEff), "########0.00")

            '.Rows(0).Cells(DgvCol_Details.SHIFT_1).Value = Format(Val(vShift1), "########0.00")
            '.Rows(0).Cells(DgvCol_Details.SHIFT_2).Value = Format(Val(vShift2), "########0.00")
            '.Rows(0).Cells(DgvCol_Details.SHIFT_3).Value = Format(Val(vShift3), "########0.00")

            ''    .Rows(0).Cells(11).Value = Format(Val(vDoff1_Mtrs), "########0.00")
            ''   .Rows(0).Cells(13).Value = Format(Val(vDoff2_Mtrs), "########0.00")
            ''  .Rows(0).Cells(15).Value = Format(Val(vDoff3_Mtrs), "########0.00")
            '' .Rows(0).Cells(17).Value = Format(Val(vDoff4_Mtrs), "########0.00")


        End With

    End Sub

    Private Sub btn_Save_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Save.Click
        save_record()
    End Sub

    Private Sub btn_Cancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Cancel.Click
        Me.Close()
    End Sub

    Public Sub print_record() Implements Interface_MDIActions.print_record
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim NewCode As String

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)
        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.PrintEntry, Common_Procedures.UR.Inhouse_Loom_Production_Entry, New_Entry) = False Then Exit Sub

        Try

            da1 = New SqlClient.SqlDataAdapter("select * from LoomNo_Production_Head Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and LoomNo_Production_Code = '" & Trim(NewCode) & "'", con)
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

        If Val(Common_Procedures.Print_OR_Preview_Status) = 1 Then

            Try
                PrintDialog1.PrinterSettings = PrintDocument1.PrinterSettings
                If PrintDialog1.ShowDialog() = Windows.Forms.DialogResult.OK Then
                    PrintDocument1.PrinterSettings = PrintDialog1.PrinterSettings
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

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        prn_HdDt.Clear()
        prn_DetDt.Clear()
        prn_DetIndx = 0
        prn_DetSNo = 0
        prn_PageNo = 0

        Try

            da1 = New SqlClient.SqlDataAdapter("select a.*, b.* from LoomNo_Production_Head a INNER JOIN Company_Head b ON a.Company_IdNo = b.Company_IdNo where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.LoomNo_Production_Code = '" & Trim(NewCode) & "'", con)
            prn_HdDt = New DataTable
            da1.Fill(prn_HdDt)

            If prn_HdDt.Rows.Count > 0 Then


                da2 = New SqlClient.SqlDataAdapter("select a.*,Lh.Loom_Name,sh.Shift_Name,ch.Cloth_Name,Em.Employee_nAME AS Employee_Shift_1,em2.Employee_nAME AS Employee_Shift_2 from LoomNo_Production_Details a  LEFT OUTER JOIN Loom_Head Lh On a.Loom_IdNo = Lh.Loom_IdNo LEFT OUTER JOIN Shift_Head sh on a.Shift_IdNo = sh.Shift_IdNo LEFT OUTER JOIN Employee_Head EM  ON A.Employee_IdNo_Shift_1=EM.Employee_IdNo  LEFT OUTER JOIN Employee_Head EM2 on A.Employee_IdNo_Shift_2=EM2.Employee_IdNo  LEFT OUTER JOIN  Cloth_Head CH on a.Cloth_idno=CH.Cloth_Idno where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.LoomNo_Production_Code = '" & Trim(NewCode) & "' Order by a.Sl_No", con)
                ' da2 = New SqlClient.SqlDataAdapter("select a.*,eh.Employee_Name,Lh.Loom_Name,sh.Shift_Name from LoomNo_Production_Details a LEFT OUTER JOIN PayRoll_Employee_Head eh ON a.Employee_Idno = eh.Employee_IdNo LEFT OUTER JOIN Loom_Head Lh On a.Loom_IdNo = Lh.Loom_IdNo LEFT OUTER JOIN Shift_Head sh on a.Shift_IdNo = sh.Shift_IdNo where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.LoomNo_Production_Code = '" & Trim(NewCode) & "' Order by a.Sl_No", con)
                'da2 = New SqlClient.SqlDataAdapter("select a.*, b.Item_name, d.Unit_name, e.Machine_name, f.Brand_Name from LoomNo_Production_Details a INNER JOIN Item_Head b ON a.Item_idno = b.Item_idno LEFT OUTER JOIN Unit_Head d ON a.Unit_idno = d.Unit_idno LEFT OUTER JOIN Machine_Head e ON a.Machine_idno = e.Machine_idno LEFT OUTER JOIN Brand_Head f ON a.Brand_idno = f.Brand_idno where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.LoomNo_Production_Code = '" & Trim(NewCode) & "' Order by a.Sl_No", con)
                prn_DetDt = New DataTable
                da2.Fill(prn_DetDt)

            Else
                MessageBox.Show("This is New Entry", "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

            End If

            da1.Dispose()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub PrintDocument1_PrintPage(ByVal sender As Object, ByVal e As System.Drawing.Printing.PrintPageEventArgs) Handles PrintDocument1.PrintPage

        If prn_HdDt.Rows.Count <= 0 Then Exit Sub
        If Common_Procedures.settings.CustomerCode = "1520" Then ' --- RAINBOW COTTON FABRIC 
            Printing_Format2_1520(e)
        Else
            Printing_Format1(e)
        End If


    End Sub

    Private Sub Printing_Format1(ByRef e As System.Drawing.Printing.PrintPageEventArgs)
        Dim pFont As Font
        Dim ps As Printing.PaperSize
        Dim LMargin As Single, RMargin As Single, TMargin As Single, BMargin As Single
        Dim PrintWidth As Single, PrintHeight As Single
        Dim PageWidth As Single, PageHeight As Single
        Dim I As Integer
        Dim NoofItems_PerPage As Integer, NoofDets As Integer
        Dim TxtHgt As Single
        Dim PpSzSTS As Boolean = False
        Dim LnAr(15) As Single, ClAr(20) As Single
        Dim EntryCode As String
        Dim CurY As Single
        Dim ItmNm1 As String, ItmNm2 As String
        Dim ClthNm1 As String, ClthNm2 As String
        Dim EmpNm1 As String, EmpNm2 As String
        Dim EmpNm4 As String, EmpNm3 As String
        Dim Remark1 As String, Remark2 As String
        Dim itemNmStr1(20) As String
        Dim ClthNmStr1(20) As String
        Dim EmpNmStr1(20) As String
        Dim EmpNmStr2(20) As String
        Dim RemarksStr1(20) As String

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
            .Right = 30
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
        'If PrintDocument1.DefaultPageSettings.Landscape = False Then
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
        ClAr = New Single(20) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}

        ClAr(1) = Val(30) : ClAr(2) = 55 : ClAr(3) = 95 : ClAr(4) = 35 : ClAr(5) = 35 : ClAr(6) = 45 : ClAr(7) = 35 : ClAr(8) = 35 : ClAr(9) = 65 : ClAr(10) = 35 : ClAr(11) = 40 : ClAr(12) = 35 : ClAr(13) = 35 : ClAr(14) = 65 : ClAr(15) = 35 : ClAr(16) = 50
        ClAr(17) = PageWidth - (LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12) + ClAr(13) + ClAr(14) + ClAr(15) + ClAr(16))

        TxtHgt = 19
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1520" Then
            NoofItems_PerPage = 35
        Else
            NoofItems_PerPage = 40
        End If


        EntryCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Try
            If prn_HdDt.Rows.Count > 0 Then

                Printing_Format1_PageHeader(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, prn_PageNo, NoofItems_PerPage, CurY, LnAr, ClAr)

                NoofDets = 0

                CurY = CurY - 10

                If prn_DetDt.Rows.Count > 0 Then

                    Do While prn_DetIndx <= prn_DetDt.Rows.Count - 1

                        If NoofDets >= NoofItems_PerPage Then

                            CurY = CurY + TxtHgt

                            Common_Procedures.Print_To_PrintDocument(e, "Continued...", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + 10, CurY, 0, 0, pFont)

                            NoofDets = NoofDets + 1

                            Printing_Format1_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClAr, NoofDets, False)

                            e.HasMorePages = True
                            Return

                        End If

                        Erase itemNmStr1
                        itemNmStr1 = New String(15) {}

                        ItmNm1 = Trim(prn_DetDt.Rows(prn_DetIndx).Item("Loom_Name").ToString)
                        ItmNm2 = ""
                        If Len(ItmNm1) > 15 Then
                            For I = 15 To 1 Step -1
                                If Mid$(Trim(ItmNm1), I, 1) = " " Or Mid$(Trim(ItmNm1), I, 1) = "," Or Mid$(Trim(ItmNm1), I, 1) = "." Or Mid$(Trim(ItmNm1), I, 1) = "-" Or Mid$(Trim(ItmNm1), I, 1) = "/" Or Mid$(Trim(ItmNm1), I, 1) = "_" Or Mid$(Trim(ItmNm1), I, 1) = "(" Or Mid$(Trim(ItmNm1), I, 1) = ")" Or Mid$(Trim(ItmNm1), I, 1) = "\" Or Mid$(Trim(ItmNm1), I, 1) = "[" Or Mid$(Trim(ItmNm1), I, 1) = "]" Or Mid$(Trim(ItmNm1), I, 1) = "{" Or Mid$(Trim(ItmNm1), I, 1) = "}" Then Exit For
                            Next I
                            If I = 0 Then I = 15
                            ItmNm2 = Microsoft.VisualBasic.Right(Trim(ItmNm1), Len(ItmNm1) - I)
                            ItmNm1 = Microsoft.VisualBasic.Left(Trim(ItmNm1), I - 1)
                        End If

                        If Trim(ItmNm2) <> "" Then
                            For k = 0 To 9
                                If Len(ItmNm2) > 15 Then

                                    For I = 15 To 1 Step -1
                                        If Mid$(Trim(ItmNm2), I, 1) = " " Or Mid$(Trim(ItmNm2), I, 1) = "," Or Mid$(Trim(ItmNm2), I, 1) = "." Or Mid$(Trim(ItmNm2), I, 1) = "-" Or Mid$(Trim(ItmNm2), I, 1) = "/" Or Mid$(Trim(ItmNm2), I, 1) = "_" Or Mid$(Trim(ItmNm2), I, 1) = "(" Or Mid$(Trim(ItmNm2), I, 1) = ")" Or Mid$(Trim(ItmNm2), I, 1) = "\" Or Mid$(Trim(ItmNm2), I, 1) = "[" Or Mid$(Trim(ItmNm2), I, 1) = "]" Or Mid$(Trim(ItmNm2), I, 1) = "{" Or Mid$(Trim(ItmNm2), I, 1) = "}" Then Exit For
                                    Next I
                                    If I = 0 Then I = 15

                                    'ClthNmStr1(k) = Microsoft.VisualBasic.Right(Trim(ItmNm2), Len(ItmNm2) - I)
                                    'ItmNm2 = Microsoft.VisualBasic.Left(Trim(ItmNm2), I - 1)

                                    itemNmStr1(k) = Microsoft.VisualBasic.Left(Trim(ItmNm2), I)
                                    ItmNm2 = Microsoft.VisualBasic.Right(Trim(ItmNm2), Len(ItmNm2) - I)

                                    If Len(ItmNm2) > 15 Then
                                        ItmNm2 = ItmNm2

                                    Else
                                        k = k + 1
                                        itemNmStr1(k) = ItmNm2
                                        Exit For

                                    End If


                                Else
                                    k = k + 1
                                    itemNmStr1(k) = ItmNm2
                                    Exit For

                                End If
                            Next
                        End If



                        '-------------------

                        Erase ClthNmStr1
                        ClthNmStr1 = New String(15) {}

                        ClthNm1 = Trim(prn_DetDt.Rows(prn_DetIndx).Item("Cloth_Name").ToString)
                        ClthNm2 = ""
                        If Len(ClthNm1) > 20 Then
                            For I = 20 To 1 Step -1
                                If Mid$(Trim(ClthNm1), I, 1) = " " Or Mid$(Trim(ClthNm1), I, 1) = "," Or Mid$(Trim(ClthNm1), I, 1) = "." Or Mid$(Trim(ClthNm1), I, 1) = "-" Or Mid$(Trim(ClthNm1), I, 1) = "/" Or Mid$(Trim(ClthNm1), I, 1) = "_" Or Mid$(Trim(ClthNm1), I, 1) = "(" Or Mid$(Trim(ClthNm1), I, 1) = ")" Or Mid$(Trim(ClthNm1), I, 1) = "\" Or Mid$(Trim(ClthNm1), I, 1) = "[" Or Mid$(Trim(ClthNm1), I, 1) = "]" Or Mid$(Trim(ClthNm1), I, 1) = "{" Or Mid$(Trim(ClthNm1), I, 1) = "}" Then Exit For
                            Next I
                            If I = 0 Then I = 20
                            ClthNm2 = Microsoft.VisualBasic.Right(Trim(ClthNm1), Len(ClthNm1) - I)
                            ClthNm1 = Microsoft.VisualBasic.Left(Trim(ClthNm1), I - 1)
                        End If


                        If Trim(ClthNm2) <> "" Then
                            For k = 0 To 9
                                If Len(ClthNm2) > 20 Then

                                    For I = 20 To 1 Step -1
                                        If Mid$(Trim(ClthNm2), I, 1) = " " Or Mid$(Trim(ClthNm2), I, 1) = "," Or Mid$(Trim(ClthNm2), I, 1) = "." Or Mid$(Trim(ClthNm2), I, 1) = "-" Or Mid$(Trim(ClthNm2), I, 1) = "/" Or Mid$(Trim(ClthNm2), I, 1) = "_" Or Mid$(Trim(ClthNm2), I, 1) = "(" Or Mid$(Trim(ClthNm2), I, 1) = ")" Or Mid$(Trim(ClthNm2), I, 1) = "\" Or Mid$(Trim(ClthNm2), I, 1) = "[" Or Mid$(Trim(ClthNm2), I, 1) = "]" Or Mid$(Trim(ClthNm2), I, 1) = "{" Or Mid$(Trim(ClthNm2), I, 1) = "}" Then Exit For
                                    Next I
                                    If I = 0 Then I = 20

                                    'ClthNmStr1(k) = Microsoft.VisualBasic.Right(Trim(ClthNm2), Len(ClthNm2) - I)
                                    'ClthNm2 = Microsoft.VisualBasic.Left(Trim(ClthNm2), I - 1)

                                    ClthNmStr1(k) = Microsoft.VisualBasic.Left(Trim(ClthNm2), I)
                                    ClthNm2 = Microsoft.VisualBasic.Right(Trim(ClthNm2), Len(ClthNm2) - I)

                                    If Len(ClthNm2) > 15 Then
                                        ClthNm2 = ClthNm2

                                    Else
                                        k = k + 1
                                        ClthNmStr1(k) = ClthNm2
                                        Exit For

                                    End If


                                Else
                                    k = k + 1
                                    ClthNmStr1(k) = ClthNm2
                                    Exit For

                                End If
                            Next
                        End If

                        '-----------------

                        Erase EmpNmStr1
                        EmpNmStr1 = New String(10) {}

                        EmpNm1 = Trim(prn_DetDt.Rows(prn_DetIndx).Item("Employee_shift_1").ToString)
                        EmpNm2 = ""

                        If Len(EmpNm1) > 10 Then
                            For I = 10 To 1 Step -1
                                If Mid$(Trim(EmpNm1), I, 1) = " " Or Mid$(Trim(EmpNm1), I, 1) = "," Or Mid$(Trim(EmpNm1), I, 1) = "." Or Mid$(Trim(EmpNm1), I, 1) = "-" Or Mid$(Trim(EmpNm1), I, 1) = "/" Or Mid$(Trim(EmpNm1), I, 1) = "_" Or Mid$(Trim(EmpNm1), I, 1) = "(" Or Mid$(Trim(EmpNm1), I, 1) = ")" Or Mid$(Trim(EmpNm1), I, 1) = "\" Or Mid$(Trim(EmpNm1), I, 1) = "[" Or Mid$(Trim(EmpNm1), I, 1) = "]" Or Mid$(Trim(EmpNm1), I, 1) = "{" Or Mid$(Trim(EmpNm1), I, 1) = "}" Then Exit For
                            Next I
                            If I = 0 Then I = 10
                            EmpNm2 = Microsoft.VisualBasic.Right(Trim(EmpNm1), Len(EmpNm1) - I)
                            EmpNm1 = Microsoft.VisualBasic.Left(Trim(EmpNm1), I - 1)
                        End If


                        If Trim(EmpNm2) <> "" Then
                            For k = 0 To 9
                                If Len(EmpNm2) > 9 Then

                                    For I = 9 To 1 Step -1
                                        If Mid$(Trim(EmpNm2), I, 1) = " " Or Mid$(Trim(EmpNm2), I, 1) = "," Or Mid$(Trim(EmpNm2), I, 1) = "." Or Mid$(Trim(EmpNm2), I, 1) = "-" Or Mid$(Trim(EmpNm2), I, 1) = "/" Or Mid$(Trim(EmpNm2), I, 1) = "_" Or Mid$(Trim(EmpNm2), I, 1) = "(" Or Mid$(Trim(EmpNm2), I, 1) = ")" Or Mid$(Trim(EmpNm2), I, 1) = "\" Or Mid$(Trim(EmpNm2), I, 1) = "[" Or Mid$(Trim(EmpNm2), I, 1) = "]" Or Mid$(Trim(EmpNm2), I, 1) = "{" Or Mid$(Trim(EmpNm2), I, 1) = "}" Then Exit For
                                    Next I
                                    If I = 0 Then I = 9



                                    EmpNmStr1(k) = Microsoft.VisualBasic.Left(Trim(EmpNm2), I)
                                    EmpNm2 = Microsoft.VisualBasic.Right(Trim(EmpNm2), Len(EmpNm2) - I)

                                    If Len(EmpNm2) > 9 Then
                                        EmpNm2 = EmpNm2

                                    Else
                                        k = k + 1
                                        EmpNmStr1(k) = EmpNm2
                                        Exit For

                                    End If


                                Else
                                    k = k + 1
                                    EmpNmStr1(k) = EmpNm2
                                    Exit For

                                End If
                            Next
                        End If

                        '---------------------

                        '-----------------

                        Erase EmpNmStr2
                        EmpNmStr2 = New String(10) {}

                        EmpNm3 = Trim(prn_DetDt.Rows(prn_DetIndx).Item("Employee_shift_2").ToString)
                        EmpNm4 = ""

                        If Len(EmpNm3) > 10 Then
                            For I = 10 To 1 Step -1
                                If Mid$(Trim(EmpNm3), I, 1) = " " Or Mid$(Trim(EmpNm3), I, 1) = "," Or Mid$(Trim(EmpNm3), I, 1) = "." Or Mid$(Trim(EmpNm3), I, 1) = "-" Or Mid$(Trim(EmpNm3), I, 1) = "/" Or Mid$(Trim(EmpNm3), I, 1) = "_" Or Mid$(Trim(EmpNm3), I, 1) = "(" Or Mid$(Trim(EmpNm3), I, 1) = ")" Or Mid$(Trim(EmpNm3), I, 1) = "\" Or Mid$(Trim(EmpNm3), I, 1) = "[" Or Mid$(Trim(EmpNm3), I, 1) = "]" Or Mid$(Trim(EmpNm3), I, 1) = "{" Or Mid$(Trim(EmpNm3), I, 1) = "}" Then Exit For
                            Next I
                            If I = 0 Then I = 10
                            EmpNm4 = Microsoft.VisualBasic.Right(Trim(EmpNm3), Len(EmpNm3) - I)
                            EmpNm3 = Microsoft.VisualBasic.Left(Trim(EmpNm3), I - 1)
                        End If


                        If Trim(EmpNm4) <> "" Then
                            For k = 0 To 9
                                If Len(EmpNm4) > 10 Then

                                    For I = 9 To 1 Step -1
                                        If Mid$(Trim(EmpNm4), I, 1) = " " Or Mid$(Trim(EmpNm4), I, 1) = "," Or Mid$(Trim(EmpNm4), I, 1) = "." Or Mid$(Trim(EmpNm4), I, 1) = "-" Or Mid$(Trim(EmpNm4), I, 1) = "/" Or Mid$(Trim(EmpNm4), I, 1) = "_" Or Mid$(Trim(EmpNm4), I, 1) = "(" Or Mid$(Trim(EmpNm4), I, 1) = ")" Or Mid$(Trim(EmpNm4), I, 1) = "\" Or Mid$(Trim(EmpNm4), I, 1) = "[" Or Mid$(Trim(EmpNm4), I, 1) = "]" Or Mid$(Trim(EmpNm4), I, 1) = "{" Or Mid$(Trim(EmpNm4), I, 1) = "}" Then Exit For
                                    Next I
                                    If I = 0 Then I = 9

                                    'ClthNmStr1(k) = Microsoft.VisualBasic.Right(Trim(EMPNM4), Len(EMPNM4) - I)
                                    'EMPNM4 = Microsoft.VisualBasic.Left(Trim(EMPNM4), I - 1)

                                    EmpNmStr2(k) = Microsoft.VisualBasic.Left(Trim(EmpNm4), I)
                                    EmpNm4 = Microsoft.VisualBasic.Right(Trim(EmpNm4), Len(EmpNm4) - I)

                                    If Len(EmpNm4) > 10 Then
                                        EmpNm4 = EmpNm4

                                    Else
                                        k = k + 1
                                        EmpNmStr2(k) = EmpNm4
                                        Exit For

                                    End If


                                Else
                                    k = k + 1
                                    EmpNmStr2(k) = EmpNm4
                                    Exit For

                                End If
                            Next
                        End If

                        '---------------------


                        Erase RemarksStr1
                        RemarksStr1 = New String(10) {}

                        Remark1 = Trim(prn_DetDt.Rows(prn_DetIndx).Item("Remarks").ToString)
                        Remark2 = ""

                        If Len(Remark1) > 15 Then
                            For I = 15 To 1 Step -1
                                If Mid$(Trim(Remark1), I, 1) = " " Or Mid$(Trim(Remark1), I, 1) = "," Or Mid$(Trim(Remark1), I, 1) = "." Or Mid$(Trim(Remark1), I, 1) = "-" Or Mid$(Trim(Remark1), I, 1) = "/" Or Mid$(Trim(Remark1), I, 1) = "_" Or Mid$(Trim(Remark1), I, 1) = "(" Or Mid$(Trim(Remark1), I, 1) = ")" Or Mid$(Trim(Remark1), I, 1) = "\" Or Mid$(Trim(Remark1), I, 1) = "[" Or Mid$(Trim(Remark1), I, 1) = "]" Or Mid$(Trim(Remark1), I, 1) = "{" Or Mid$(Trim(Remark1), I, 1) = "}" Then Exit For
                            Next I
                            If I = 0 Then I = 15
                            Remark2 = Microsoft.VisualBasic.Right(Trim(Remark1), Len(Remark1) - I)
                            Remark1 = Microsoft.VisualBasic.Left(Trim(Remark1), I - 1)
                        End If


                        If Trim(Remark2) <> "" Then
                            For k = 0 To 9
                                If Len(Remark2) > 15 Then

                                    For I = 10 To 1 Step -1
                                        If Mid$(Trim(Remark2), I, 1) = " " Or Mid$(Trim(Remark2), I, 1) = "," Or Mid$(Trim(Remark2), I, 1) = "." Or Mid$(Trim(Remark2), I, 1) = "-" Or Mid$(Trim(Remark2), I, 1) = "/" Or Mid$(Trim(Remark2), I, 1) = "_" Or Mid$(Trim(Remark2), I, 1) = "(" Or Mid$(Trim(Remark2), I, 1) = ")" Or Mid$(Trim(Remark2), I, 1) = "\" Or Mid$(Trim(Remark2), I, 1) = "[" Or Mid$(Trim(Remark2), I, 1) = "]" Or Mid$(Trim(Remark2), I, 1) = "{" Or Mid$(Trim(Remark2), I, 1) = "}" Then Exit For
                                    Next I
                                    If I = 0 Then I = 10

                                    RemarksStr1(k) = Microsoft.VisualBasic.Left(Trim(Remark2), I)
                                    Remark2 = Microsoft.VisualBasic.Right(Trim(Remark2), Len(Remark2) - I)

                                    If Len(Remark2) > 10 Then
                                        Remark2 = Remark2

                                    Else
                                        k = k + 1
                                        RemarksStr1(k) = Remark2
                                        Exit For

                                    End If
                                Else
                                    k = k + 1
                                    RemarksStr1(k) = Remark2
                                    Exit For

                                End If
                            Next
                        End If

                        '---------------------



                        pFont = New Font("Calibri", 8, FontStyle.Regular)
                        CurY = CurY + TxtHgt
                        Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetDt.Rows(prn_DetIndx).Item("Sl_No").ToString), LMargin + 10, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm1), LMargin + ClAr(1) + 10, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Trim(ClthNm1), LMargin + ClAr(1) + ClAr(2), CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(prn_DetIndx).Item("RPM").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) - 3, CurY, 1, 0, pFont)

                        Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(prn_DetIndx).Item("Shift_1_Pick_Efficiency").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 3, CurY, 1, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(prn_DetIndx).Item("Shift1_Mtrs").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 3, CurY, 1, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(prn_DetIndx).Item("Shift_1_Warp_Breakage").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 3, CurY, 1, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(prn_DetIndx).Item("Shift_1_weft_Breakage").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 3, CurY, 1, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Trim(EmpNm1), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, 0, 0, pFont)

                        Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(prn_DetIndx).Item("Shift_2_Pick_Efficiency").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) - 3, CurY, 1, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(prn_DetIndx).Item("Shift2_Mtrs").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) - 3, CurY, 1, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(prn_DetIndx).Item("Shift_2_Warp_Breakage").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12) - 3, CurY, 1, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(prn_DetIndx).Item("Shift_2_Warp_Breakage").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12) + ClAr(13) - 3, CurY, 1, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Trim(EmpNm3), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12) + ClAr(13), CurY, 0, 0, pFont)

                        Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(prn_DetIndx).Item("Pick_Efficiency").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12) + ClAr(13) + ClAr(14) + ClAr(15) - 3, CurY, 1, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(prn_DetIndx).Item("Meters").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12) + ClAr(13) + ClAr(14) + ClAr(15) + ClAr(16) - 3, CurY, 1, 0, pFont)

                        If Trim(Remark1) <> "" Then
                            Common_Procedures.Print_To_PrintDocument(e, Trim(Remark1), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12) + ClAr(13) + ClAr(14) + ClAr(15) + ClAr(16), CurY, 0, 0, pFont)
                        End If

                        NoofDets = NoofDets + 1


                        'If Trim(ItmNm2) <> "" Then
                        '    '     CurY = CurY + TxtHgt
                        '    Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm2), LMargin + ClAr(1) + 10, CurY, 0, 0, pFont)

                        'End If
                        For I = 0 To 9
                            If Trim(itemNmStr1(I)) <> "" Or Trim(ClthNmStr1(I)) <> "" Or Trim(EmpNmStr1(I)) <> "" Or Trim(EmpNmStr2(I)) <> "" Or Trim(RemarksStr1(I)) <> "" Then
                                CurY = CurY + TxtHgt
                                NoofDets = NoofDets + 1
                                Common_Procedures.Print_To_PrintDocument(e, Trim(itemNmStr1(I)), LMargin + ClAr(1) + 10, CurY, 0, 0, pFont)
                                Common_Procedures.Print_To_PrintDocument(e, Trim(ClthNmStr1(I)), LMargin + ClAr(1) + ClAr(2), CurY, 0, PageWidth, pFont)
                                Common_Procedures.Print_To_PrintDocument(e, Trim(EmpNmStr1(I)), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, 0, 0, pFont)
                                Common_Procedures.Print_To_PrintDocument(e, Trim(EmpNmStr2(I)), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12) + ClAr(13), CurY, 0, 0, pFont)
                                Common_Procedures.Print_To_PrintDocument(e, Trim(RemarksStr1(I)), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12) + ClAr(13) + ClAr(14) + ClAr(15) + ClAr(16), CurY, 0, 0, pFont)

                            End If
                            'If Trim(EmpNmStr1(I)) <> "" Then
                            '    CurY = CurY + TxtHgt
                            '    NoofDets = NoofDets + 1
                            '    Common_Procedures.Print_To_PrintDocument(e, Trim(EmpNmStr1(I)), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, 0, 0, pFont)
                            'End If

                            'If Trim(EmpNmStr2(I)) <> "" Then
                            '    CurY = CurY + TxtHgt
                            '    NoofDets = NoofDets + 1
                            '    Common_Procedures.Print_To_PrintDocument(e, Trim(EmpNmStr2(I)), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12) + ClAr(13), CurY, 0, 0, pFont)
                            'End If
                        Next

                        prn_DetIndx = prn_DetIndx + 1

                    Loop

                End If

                Printing_Format1_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClAr, NoofDets, True)

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        e.HasMorePages = False

    End Sub

    Private Sub Printing_Format1_PageHeader(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByRef PageNo As Integer, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single)
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt2 As New DataTable
        Dim p1Font As Font
        Dim Cmp_Name As String, Cmp_Add1 As String, Cmp_Add2 As String
        Dim Cmp_PhNo As String, Cmp_TinNo As String, Cmp_CstNo As String
        Dim strHeight As Single
        Dim C1 As Single
        Dim C2 As Single
        Dim W1 As Single
        Dim S1 As Single

        PageNo = PageNo + 1

        CurY = TMargin

        da2 = New SqlClient.SqlDataAdapter("select a.*,eh.Employee_Name,Lh.Loom_Name,sh.Shift_Name from LoomNo_Production_Details a LEFT OUTER JOIN PayRoll_Employee_Head eh ON a.Employee_Idno = eh.Employee_IdNo LEFT OUTER JOIN Loom_Head Lh On a.Loom_IdNo = Lh.Loom_IdNo LEFT OUTER JOIN Shift_Head sh on a.Shift_IdNo = sh.Shift_IdNo where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.LoomNo_Production_Code = '" & Trim(EntryCode) & "' Order by a.Sl_No", con)
        da2.Fill(dt2)

        If dt2.Rows.Count > NoofItems_PerPage Then
            Common_Procedures.Print_To_PrintDocument(e, "Page : " & Trim(Val(PageNo)), PageWidth - 10, CurY - TxtHgt, 1, 0, pFont)
        End If
        dt2.Clear()

        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(1) = CurY

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

        If Trim(Common_Procedures.settings.CustomerCode) <> "1234" Then '-----ARULJOTHI EXPORTS PVT LTD

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

        End If


        'CurY = CurY + TxtHgt - 5
        'p1Font = New Font("Calibri", 16, FontStyle.Bold)
        'Common_Procedures.Print_To_PrintDocument(e, "DELIVERY", LMargin, CurY, 2, PrintWidth, p1Font)
        'strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height

        CurY = CurY + strHeight
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(2) = CurY
        C1 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4)
        C2 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9)
        W1 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12) + ClAr(13) + ClAr(14)

        Common_Procedures.Print_To_PrintDocument(e, "DATE :  " & Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("LoomNo_Production_Date").ToString), "dd-MM-yyyy").ToString, LMargin + ClAr(1), CurY, 2, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, "SHIFT A", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, 2, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, "SHIFT B", LMargin + C2 + 50, CurY, 2, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, "TOTAL", LMargin + W1 + 50, CurY, 2, 0, pFont)

        CurY = CurY + TxtHgt
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(3) = CurY


        CurY = CurY + TxtHgt
        e.Graphics.DrawLine(Pens.Black, LMargin + C1, CurY, LMargin + C1, LnAr(2))
        e.Graphics.DrawLine(Pens.Black, LMargin + C2, CurY, LMargin + C2, LnAr(2))
        e.Graphics.DrawLine(Pens.Black, LMargin + W1, CurY, LMargin + W1, LnAr(2))

        pFont = New Font("Calibri", 8, FontStyle.Bold)
        CurY = CurY + TxtHgt - 20
        Common_Procedures.Print_To_PrintDocument(e, "SL", LMargin, CurY, 2, ClAr(1), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "NO", LMargin, CurY + 15, 2, ClAr(1), pFont)

        Common_Procedures.Print_To_PrintDocument(e, "LOOM", LMargin + ClAr(1), CurY, 2, ClAr(2), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "NO", LMargin + ClAr(1), CurY + 15, 2, ClAr(2), pFont)

        Common_Procedures.Print_To_PrintDocument(e, "CLOTH", LMargin + ClAr(1) + ClAr(2), CurY, 2, ClAr(3), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "NAME", LMargin + ClAr(1) + ClAr(2), CurY + 15, 2, ClAr(3), pFont)

        Common_Procedures.Print_To_PrintDocument(e, "RPM", LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, 2, ClAr(4), pFont)

        Common_Procedures.Print_To_PrintDocument(e, "PICK", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 2, ClAr(5), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "EFF", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY + 15, 2, ClAr(5), pFont)

        Common_Procedures.Print_To_PrintDocument(e, "METER", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, 2, ClAr(6), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "WARP", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, 2, ClAr(7), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "WEFT", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, 2, ClAr(8), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "EMPLOYEE", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, 2, ClAr(9), pFont)

        Common_Procedures.Print_To_PrintDocument(e, "PICK", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9), CurY, 2, ClAr(10), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "EFF", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9), CurY + 15, 2, ClAr(10), pFont)

        Common_Procedures.Print_To_PrintDocument(e, "METER", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10), CurY, 2, ClAr(11), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "WARP", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11), CurY, 2, ClAr(12), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "WEFT", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12), CurY, 2, ClAr(13), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "EMPLOYEE", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12) + ClAr(13), CurY, 2, ClAr(14), pFont)

        Common_Procedures.Print_To_PrintDocument(e, "TOTAL", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12) + ClAr(13) + ClAr(14), CurY, 2, ClAr(15), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "EFF", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12) + ClAr(13) + ClAr(14), CurY + 15, 2, ClAr(15), pFont)

        Common_Procedures.Print_To_PrintDocument(e, "TOTAL", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12) + ClAr(13) + ClAr(14) + ClAr(15), CurY, 2, ClAr(16), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "METERS", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12) + ClAr(13) + ClAr(14) + ClAr(15), CurY + 15, 2, ClAr(16), pFont)

        Common_Procedures.Print_To_PrintDocument(e, "REMARKS", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12) + ClAr(13) + ClAr(14) + ClAr(15) + ClAr(16), CurY, 2, ClAr(17), pFont)

        CurY = CurY + TxtHgt + 15
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(4) = CurY


    End Sub

    Private Sub Printing_Format1_PageFooter(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single, ByVal NoofDets As Integer, ByVal is_LastPage As Boolean)
        Dim i As Integer
        Dim Cmp_Name As String
        Dim p1Font As Font


        For i = NoofDets + 1 To NoofItems_PerPage
            CurY = CurY + TxtHgt
        Next

        CurY = CurY + TxtHgt
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(5) = CurY
        pFont = New Font("Calibri", 8, FontStyle.Regular)
        CurY = CurY + TxtHgt - 10
        If is_LastPage = True Then
            Common_Procedures.Print_To_PrintDocument(e, " TOTAL", LMargin + ClAr(1) + ClAr(2) + 20, CurY, 2, ClAr(4), pFont)
            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Total_Avg_RPM").ToString), "########0"), LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, 2, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Total_Avg_Shift_1_Pick_Efficiency").ToString), "#########0"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 2, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Total_Shift1_Mtrs").ToString), "########0"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, 2, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Total_Avg_Shift_2_Pick_Efficiency").ToString), "#########0"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9), CurY, 2, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Total_Shift2_Mtrs").ToString), "########0"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10), CurY, 2, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Total_PickEfficiency").ToString), "########0"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12) + ClAr(13) + ClAr(14), CurY, 2, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Total_Meters").ToString), "#########0"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12) + ClAr(13) + ClAr(14) + ClAr(15), CurY, 2, 0, pFont)
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
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11), LnAr(3))

        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12) + ClAr(13), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12) + ClAr(13), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12) + ClAr(13) + ClAr(14), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12) + ClAr(13) + ClAr(14), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12) + ClAr(13) + ClAr(14) + ClAr(15), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12) + ClAr(13) + ClAr(14) + ClAr(15), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12) + ClAr(13) + ClAr(14) + ClAr(15) + ClAr(16), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12) + ClAr(13) + ClAr(14) + ClAr(15) + ClAr(16), LnAr(3))



        If is_LastPage = True Then
            If Val(prn_HdDt.Rows(0).Item("EB_Units_Consumed").ToString) <> 0 Or Val(prn_HdDt.Rows(0).Item("EB_Amount").ToString) <> 0 Or Val(prn_HdDt.Rows(0).Item("Employee_Salary").ToString) <> 0 Then
                Dim vStringWidth As Single = 0

                CurY = CurY + TxtHgt - 15

                Common_Procedures.Print_To_PrintDocument(e, "EB Units :  " & Format(Val(prn_HdDt.Rows(0).Item("EB_Units_Consumed").ToString), "##########0.00"), LMargin + 10, CurY, 0, 0, pFont)

                Common_Procedures.Print_To_PrintDocument(e, "EB Amount :  " & Format(Val(prn_HdDt.Rows(0).Item("EB_Amount").ToString), "##########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + 10, CurY, 0, 0, pFont)

                Common_Procedures.Print_To_PrintDocument(e, "Employee Salary :  " & Format(Val(prn_HdDt.Rows(0).Item("Employee_Salary").ToString), "##########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + 10, CurY, 0, 0, pFont)

                CurY = CurY + TxtHgt
                e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

            End If
        End If



        Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString
        p1Font = New Font("Calibri", 12, FontStyle.Bold)

        If is_LastPage = True Then
            CurY = CurY + TxtHgt - 5
            Common_Procedures.Print_To_PrintDocument(e, "For " & Cmp_Name, PageWidth - 15, CurY, 1, 0, p1Font)

            CurY = CurY + TxtHgt + 10
            CurY = CurY + TxtHgt
            CurY = CurY + TxtHgt


            Common_Procedures.Print_To_PrintDocument(e, "Prepared by", LMargin + ClAr(1) + ClAr(2) + 20, CurY, 1, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "Checked by", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, 1, 0, pFont)
            'End If
            CurY = CurY + TxtHgt + 10

            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3), LnAr(6))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), LnAr(6))
        End If
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

        e.Graphics.DrawLine(Pens.Black, LMargin, LnAr(1), LMargin, CurY)
        e.Graphics.DrawLine(Pens.Black, PageWidth, LnAr(1), PageWidth, CurY)

    End Sub



    Private Sub cbo_Filter_Shift_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Filter_Shift.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Shift_Head", "Shift_Name", "", "(Shift_idno = 0)")
    End Sub

    Private Sub cbo_Filter_Shift_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Filter_Shift.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_Shift, dtp_Filter_ToDate, cbo_Filter_Loom, "Shift_Head", "Shift_Name", "", "(Shift_idno = 0)")

    End Sub

    Private Sub cbo_Filter_Shift_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Filter_Shift.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Filter_Shift, cbo_Filter_Loom, "Shift_Head", "Shift_Name", "", "(Shift_idno = 0)")

    End Sub

    Private Sub msk_Date_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles msk_Date.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        vmskOldText = ""
        vmskSelStrt = -1
        If e.KeyCode = 46 Or e.KeyCode = 8 Then
            vmskOldText = msk_Date.Text
            vmskSelStrt = msk_Date.SelectionStart
        End If

        If (e.KeyValue = 40) Then
            dgv_Details.Focus()
            dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(DgvCol_Details.LOOM_NO)
        End If

    End Sub

    Private Sub msk_Date_KeyPress(sender As Object, e As System.Windows.Forms.KeyPressEventArgs) Handles msk_Date.KeyPress
        Dim vTotMtr As String

        If Trim(UCase(e.KeyChar)) = "D" Then
            msk_Date.Text = Date.Today
            msk_Date.SelectionStart = 0
        End If

        Try

            If Asc(e.KeyChar) = 13 Then


                vTotMtr = 0
                If dgv_Details_Total.RowCount > 0 Then
                    vTotMtr = Val(dgv_Details_Total.Rows(0).Cells(DgvCol_Details.TOTAL_METERS).Value())
                End If

                If Trim(UCase(msk_Date.Text)) <> Trim(UCase(msk_Date.Tag)) Or dgv_Details.Rows.Count = 0 Or Val(vTotMtr) = 0 Then
                    Check_and_Get_LoomNo_List(sender)
                End If

                If dgv_Details.Rows.Count > 0 Then
                    dgv_Details.Focus()
                    If dgv_Details.Rows(0).Cells(DgvCol_Details.CLOTH_NAME).Visible = True Then
                        dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(DgvCol_Details.CLOTH_NAME)
                    ElseIf dgv_Details.Rows(0).Cells(DgvCol_Details.SHIFT_1).Visible = True Then
                        dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(DgvCol_Details.SHIFT_1)
                    Else
                        dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(DgvCol_Details.LOOM_NO)
                    End If
                    dgv_Details.CurrentCell.Selected = True

                Else

                    If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                        save_record()
                    Else
                        msk_Date.Focus()
                    End If

                End If

            End If

        Catch ex As Exception
            '----

        End Try

        'If Asc(e.KeyChar) = 13 Then
        '    dgv_Details.Focus()
        '    dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(1)
        'End If
    End Sub


    Private Sub msk_Date_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles msk_Date.KeyUp
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

    Private Sub dtp_Date_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dtp_Date.TextChanged

        If IsDate(dtp_Date.Text) = True Then

            msk_Date.Text = dtp_Date.Text
            msk_Date.SelectionStart = 0
        End If
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

    Private Sub dtp_Date_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dtp_Date.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            dtp_Date.Text = Date.Today
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

    Private Sub Cbo_Grid_Doff1_GotFocus(sender As Object, e As EventArgs) Handles Cbo_Grid_Doff1.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Shift_Head", "Shift_Name", "", "(Shift_idno = 0)")
    End Sub

    Private Sub Cbo_Grid_Doff1_KeyDown(sender As Object, e As KeyEventArgs) Handles Cbo_Grid_Doff1.KeyDown

        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, Cbo_Grid_Doff1, Nothing, Nothing, "Shift_Head", "Shift_Name", "", "(Shift_idno = 0)")

        With dgv_Details

            If (e.KeyValue = 38 And Cbo_Grid_Doff1.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then

                dgv_Details.Focus()
                dgv_Details.CurrentCell = dgv_Details.Rows(dgv_Details.CurrentRow.Index).Cells(DgvCol_Details.SHIFT_2)

            End If

            If (e.KeyValue = 40 And Cbo_Grid_Doff1.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then

                'dgv_Details.CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 1)
                .Focus()
                .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)


            End If

        End With
    End Sub

    Private Sub Cbo_Grid_Doff1_KeyPress(sender As Object, e As KeyPressEventArgs) Handles Cbo_Grid_Doff1.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, Cbo_Grid_Doff1, Nothing, "Shift_Head", "Shift_Name", "", "(Shift_idno = 0)")
        If Asc(e.KeyChar) = 13 Then
            With dgv_Details

                If Trim(Cbo_Grid_Doff1.Text) = "" Then

                    If Val(.Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1).Value) <> 0 Then
                        .Focus()
                        .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)

                    ElseIf Trim(.Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 2).Value) <> "" And Val(.Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 3).Value) <> 0 Then
                        .Focus()
                        .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 2)

                    ElseIf .CurrentCell.RowIndex = .RowCount - 1 Then

                        If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                            save_record()
                        Else
                            msk_Date.Focus()
                            Exit Sub
                        End If

                    Else
                        .Focus()
                        .CurrentCell = .Rows(.CurrentRow.Index + 1).Cells(DgvCol_Details.SHIFT_1)

                    End If


                Else
                    .Focus()
                    .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)

                End If

            End With
        End If
    End Sub

    Private Sub Cbo_Grid_Doff1_TextChanged(sender As Object, e As EventArgs) Handles Cbo_Grid_Doff1.TextChanged
        Try
            If Cbo_Grid_Doff1.Visible Then

                If IsNothing(dgv_Details.CurrentCell) Then Exit Sub
                With dgv_Details
                    If Val(Cbo_Grid_Doff1.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = DgvCol_Details.DOFF_1_SHIFT Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(Cbo_Grid_Doff1.Text)
                    End If
                End With
            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub Cbo_Grid_Doff2_GotFocus(sender As Object, e As EventArgs) Handles Cbo_Grid_Doff2.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Shift_Head", "Shift_Name", "", "(Shift_idno = 0)")
    End Sub

    Private Sub Cbo_Grid_Doff2_KeyDown(sender As Object, e As KeyEventArgs) Handles Cbo_Grid_Doff2.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, Cbo_Grid_Doff2, Nothing, Nothing, "Shift_Head", "Shift_Name", "", "(Shift_idno = 0)")

        With dgv_Details

            If (e.KeyValue = 38 And Cbo_Grid_Doff2.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then

                dgv_Details.Focus()
                dgv_Details.CurrentCell = dgv_Details.Rows(dgv_Details.CurrentRow.Index).Cells(DgvCol_Details.DOFF_1_METERS)

            End If

            If (e.KeyValue = 40 And Cbo_Grid_Doff2.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then


                '  dgv_Details.CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 1)
                .Focus()
                .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)


            End If

        End With

    End Sub
    Private Sub Cbo_Grid_Doff2_KeyPress(sender As Object, e As KeyPressEventArgs) Handles Cbo_Grid_Doff2.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, Cbo_Grid_Doff2, Nothing, "Shift_Head", "Shift_Name", "", "(Shift_idno = 0)")

        If Asc(e.KeyChar) = 13 Then

            With dgv_Details

                If Trim(Cbo_Grid_Doff2.Text) = "" Then

                    If Val(.Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1).Value) <> 0 Then
                        .Focus()
                        .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)

                    ElseIf Trim(.Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 2).Value) <> "" And Val(.Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 3).Value) <> 0 Then
                        .Focus()
                        .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 2)

                    ElseIf .CurrentCell.RowIndex = .RowCount - 1 Then

                        If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                            save_record()
                        Else
                            msk_Date.Focus()
                            Exit Sub
                        End If

                    Else
                        .Focus()
                        .CurrentCell = .Rows(.CurrentRow.Index + 1).Cells(DgvCol_Details.SHIFT_1)

                    End If


                Else
                    .Focus()
                    .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)

                End If

            End With

        End If

    End Sub

    Private Sub Cbo_Grid_Doff2_TextChanged(sender As Object, e As EventArgs) Handles Cbo_Grid_Doff2.TextChanged
        Try
            If Cbo_Grid_Doff2.Visible Then

                If IsNothing(dgv_Details.CurrentCell) Then Exit Sub
                With dgv_Details
                    If Val(Cbo_Grid_Doff2.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = DgvCol_Details.DOFF_2_SHIFT Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(Cbo_Grid_Doff2.Text)
                    End If
                End With
            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub
    Private Sub Cbo_Grid_Doff3_GotFocus(sender As Object, e As EventArgs) Handles Cbo_Grid_Doff3.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Shift_Head", "Shift_Name", "", "(Shift_idno = 0)")
    End Sub

    Private Sub Cbo_Grid_Doff3_KeyDown(sender As Object, e As KeyEventArgs) Handles Cbo_Grid_Doff3.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, Cbo_Grid_Doff3, Nothing, Nothing, "Shift_Head", "Shift_Name", "", "(Shift_idno = 0)")

        With dgv_Details

            If (e.KeyValue = 38 And Cbo_Grid_Doff3.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then

                dgv_Details.Focus()
                dgv_Details.CurrentCell = dgv_Details.Rows(dgv_Details.CurrentRow.Index).Cells(DgvCol_Details.DOFF_2_METERS)

            End If

            If (e.KeyValue = 40 And Cbo_Grid_Doff3.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then


                '   dgv_Details.CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 1)
                .Focus()
                .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)



            End If

        End With

    End Sub

    Private Sub Cbo_Grid_Doff3_KeyPress(sender As Object, e As KeyPressEventArgs) Handles Cbo_Grid_Doff3.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, Cbo_Grid_Doff3, Nothing, "Shift_Head", "Shift_Name", "", "(Shift_idno = 0)")

        If Asc(e.KeyChar) = 13 Then

            With dgv_Details

                If Trim(Cbo_Grid_Doff3.Text) = "" Then

                    If Val(.Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1).Value) <> 0 Then
                        .Focus()
                        .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)

                    ElseIf .CurrentCell.RowIndex = .RowCount - 1 Then

                        If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                            save_record()
                        Else
                            msk_Date.Focus()
                            Exit Sub
                        End If

                    Else
                        .Focus()
                        .CurrentCell = .Rows(.CurrentRow.Index + 1).Cells(DgvCol_Details.SHIFT_1)

                    End If


                Else
                    .Focus()
                    .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)

                End If

            End With

        End If

    End Sub
    Private Sub Cbo_Grid_Doff3_TextChanged(sender As Object, e As EventArgs) Handles Cbo_Grid_Doff3.TextChanged
        Try
            If Cbo_Grid_Doff3.Visible Then

                If IsNothing(dgv_Details.CurrentCell) Then Exit Sub
                With dgv_Details
                    If Val(Cbo_Grid_Doff3.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = DgvCol_Details.DOFF_3_SHIFT Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(Cbo_Grid_Doff3.Text)
                    End If
                End With
            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub Cbo_Grid_Doff4_GotFocus(sender As Object, e As EventArgs) Handles Cbo_Grid_Doff4.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Shift_Head", "Shift_Name", "", "(Shift_idno = 0)")
    End Sub

    Private Sub Cbo_Grid_Doff4_KeyDown(sender As Object, e As KeyEventArgs) Handles Cbo_Grid_Doff4.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, Cbo_Grid_Doff4, Nothing, Nothing, "Shift_Head", "Shift_Name", "", "(Shift_idno = 0)")

        With dgv_Details

            If (e.KeyValue = 38 And Cbo_Grid_Doff4.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then

                dgv_Details.Focus()
                dgv_Details.CurrentCell = dgv_Details.Rows(dgv_Details.CurrentRow.Index).Cells(DgvCol_Details.DOFF_3_METERS)

            End If

            If (e.KeyValue = 40 And Cbo_Grid_Doff4.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then

                If .CurrentRow.Index = .Rows.Count - 1 Then

                    If MessageBox.Show("Do you want to save", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = DialogResult.Yes Then
                        save_record()
                    Else
                        dtp_Date.Focus()
                    End If

                Else
                    ' dgv_Details.CurrentCell = dgv_Details.Rows(dgv_Details.CurrentRow.Index + 1).Cells(1)
                    .Focus()
                    .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)

                End If

            End If

        End With

    End Sub

    Private Sub Cbo_Grid_Doff4_KeyPress(sender As Object, e As KeyPressEventArgs) Handles Cbo_Grid_Doff4.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, Cbo_Grid_Doff4, Nothing, "Shift_Head", "Shift_Name", "", "(Shift_idno = 0)")

        If Asc(e.KeyChar) = 13 Then
            With dgv_Details
                If Trim(.Rows(.CurrentRow.Index).Cells(DgvCol_Details.SHIFT_1_EMPLOYEE).Value) = "" And .CurrentRow.Index = .Rows.Count - 1 Then
                    If MessageBox.Show("Do you want to save", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = DialogResult.Yes Then
                        save_record()
                    Else
                        dtp_Date.Focus()
                    End If

                Else
                    ' dgv_Details.CurrentCell = dgv_Details.Rows(dgv_Details.CurrentRow.Index + 1).Cells(1)
                    .Focus()
                    .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)

                End If
            End With
        End If

    End Sub

    Private Sub Cbo_Grid_Doff4_TextChanged(sender As Object, e As EventArgs) Handles Cbo_Grid_Doff4.TextChanged

        Try
            If Cbo_Grid_Doff4.Visible Then

                If IsNothing(dgv_Details.CurrentCell) Then Exit Sub
                With dgv_Details
                    If Val(Cbo_Grid_Doff4.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = DgvCol_Details.DOFF_4_SHIFT Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(Cbo_Grid_Doff4.Text)
                    End If
                End With
            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub dgtxt_Details_KeyUp(sender As Object, e As KeyEventArgs) Handles dgtxt_Details.KeyUp
        Try
            With dgv_Details
                If .Visible = True Then
                    If .Rows.Count > 0 Then
                        If e.Control = True And UCase(Chr(e.KeyCode)) = "D" Then
                            dgv_Details_KeyUp(sender, e)
                        End If
                    End If
                End If
            End With

        Catch ex As Exception
            MessageBox.Show(ex.Message, "ERROR WHILE DETAILS TEXT KEYUP....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub dgtxt_Details_TextChanged(sender As Object, e As EventArgs) Handles dgtxt_Details.TextChanged
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

    Private Sub dtp_Filter_ToDate_KeyDown(sender As Object, e As KeyEventArgs) Handles dtp_Filter_ToDate.KeyDown

        If (e.KeyValue = 38) Then
            dtp_Filter_Fromdate.Focus()
        End If


        If (e.KeyValue = 40) Then
            cbo_Filter_Loom.Focus()
        End If

    End Sub

    Private Sub dtp_Filter_ToDate_KeyPress(sender As Object, e As KeyPressEventArgs) Handles dtp_Filter_ToDate.KeyPress
        If Asc(e.KeyChar) = 13 Then
            cbo_Filter_Loom.Focus()
        End If
    End Sub

    Private Sub btn_Selection_Click(sender As Object, e As EventArgs) Handles btn_List_LoomDetails.Click
        Check_and_Get_LoomNo_List(sender)
    End Sub

    Private Sub Check_and_Get_LoomNo_List(sender As System.Object)
        Dim Cmd As New SqlClient.SqlCommand
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim movno As String = ""
        Dim NewCode As String = ""
        Dim Cat_ID As Integer = 0

        Try

            If IsDate(msk_Date.Text) = False Then
                MessageBox.Show("Invalid Date", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                If msk_Date.Enabled And msk_Date.Visible Then msk_Date.Focus()
                Exit Sub
            End If


            If Not (Convert.ToDateTime(msk_Date.Text) >= Common_Procedures.Company_FromDate And Convert.ToDateTime(msk_Date.Text) <= Common_Procedures.Company_ToDate) Then
                MessageBox.Show("Date is out of financial range", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                If msk_Date.Enabled And msk_Date.Visible Then msk_Date.Focus()
                Exit Sub
            End If


            Cmd.Connection = con

            Cmd.Parameters.Clear()
            Cmd.Parameters.AddWithValue("@EntryDate", Convert.ToDateTime(msk_Date.Text))

            NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            Cmd.CommandText = "Select LoomNo_Production_No from LoomNo_Production_Head Where company_idno = " & Str(Val(lbl_Company.Tag)) & " and LoomNo_Production_Date = @EntryDate and LoomNo_Production_Code <> '" & Trim(NewCode) & "' Order by LoomNo_Production_Date, for_orderby, LoomNo_Production_No"
            Da = New SqlClient.SqlDataAdapter(Cmd)
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

                If Trim(UCase(movno)) <> Trim(UCase(lbl_RefNo.Text)) Then
                    move_record(movno, True)

                Else

                    If sender.name.ToString.ToLower = btn_List_LoomDetails.Name.ToString.ToLower Then
                        move_record(movno, True)
                    End If

                End If

            Else

                get_LoomList()

            End If

            msk_Date.Tag = msk_Date.Text

        Catch ex As Exception
            '----

        End Try
    End Sub

    Private Sub get_LoomList()
        Dim Cmd As New SqlClient.SqlCommand
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim n As Integer
        Dim SNo As Integer

        Cmd.Connection = con

        Cmd.CommandText = "select a.* from Loom_Head a Where Loom_Name <> '' Order by a.LmNo_OrderBy, a.Loom_Name"
        da1 = New SqlClient.SqlDataAdapter(Cmd)
        dt1 = New DataTable
        da1.Fill(dt1)

        With dgv_Details

            .Rows.Clear()
            SNo = 0

            If dt1.Rows.Count > 0 Then

                For i = 0 To dt1.Rows.Count - 1

                    n = .Rows.Add()

                    SNo = SNo + 1

                    .Rows(n).Cells(DgvCol_Details.SL_NO).Value = Val(SNo)
                    .Rows(n).Cells(DgvCol_Details.LOOM_NO).Value = dt1.Rows(i).Item("Loom_Name").ToString

                Next i

            End If

            msk_Date.Tag = msk_Date.Text

            Grid_Cell_DeSelect()

        End With
    End Sub

    Private Sub msk_Date_GotFocus(sender As Object, e As EventArgs) Handles msk_Date.GotFocus
        msk_Date.Tag = msk_Date.Text
    End Sub
    Private Sub Cbo_Grid_Cloth_Name_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles Cbo_Grid_Cloth_Name.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Cloth_Head", "Cloth_Name", "", "(Cloth_idno = 0)")
    End Sub
    Private Sub Cbo_Grid_Cloth_Name_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Cbo_Grid_Cloth_Name.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, Cbo_Grid_Cloth_Name, Nothing, Nothing, "Cloth_Head", "Cloth_Name", "", "(Cloth_idno = 0)")

        With dgv_Details

            If (e.KeyValue = 38 And Cbo_Grid_Cloth_Name.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then


                dgv_Details.Focus()
                dgv_Details.CurrentCell = dgv_Details.Rows(dgv_Details.CurrentRow.Index).Cells(DgvCol_Details.LOOM_NO)
                'dgv_Details.CurrentCell = dgv_Details.Rows(dgv_Details.CurrentRow.Index - 1).Cells(DgvCol_Details.LOOM_NO)

            End If
            If (e.KeyValue = 40 And Cbo_Grid_Cloth_Name.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then

                If Trim(.Rows(.CurrentRow.Index).Cells(DgvCol_Details.CLOTH_NAME).Value) = "" And .CurrentRow.Index = .Rows.Count - 1 Then
                    If txt_EB_Units.Visible Then
                        txt_EB_Units.Focus()
                    Else
                        If MessageBox.Show("Do you want to save", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = DialogResult.Yes Then
                            save_record()
                        Else
                            dtp_Date.Focus()
                        End If
                    End If
                Else
                    dgv_Details.Focus()
                    dgv_Details.CurrentCell = dgv_Details.Rows(dgv_Details.CurrentRow.Index).Cells(DgvCol_Details.RPM)

                End If
            End If
        End With
    End Sub
    Private Sub Cbo_Grid_Cloth_Name_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Cbo_Grid_Cloth_Name.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, Cbo_Grid_Cloth_Name, Nothing, "Cloth_Head", "Cloth_Name", "", "(Cloth_idno = 0)")
        With dgv_Details

            If Asc(e.KeyChar) = 13 Then

                If Trim(.Rows(.CurrentRow.Index).Cells(DgvCol_Details.CLOTH_NAME).Value) = "" And .CurrentRow.Index = .Rows.Count - 1 Then

                    If txt_EB_Units.Visible Then
                        txt_EB_Units.Focus()
                    Else
                        If MessageBox.Show("Do you want to save", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = DialogResult.Yes Then
                            save_record()
                        Else
                            dtp_Date.Focus()
                        End If
                    End If
                Else
                    dgv_Details.Focus()
                    dgv_Details.CurrentCell = dgv_Details.Rows(dgv_Details.CurrentRow.Index).Cells(DgvCol_Details.RPM)
                End If
            End If
        End With
    End Sub
    Private Sub Cbo_Grid_Cloth_Name_KeyUp(sender As Object, e As KeyEventArgs) Handles Cbo_Grid_Cloth_Name.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Dim f As New Cloth_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = Cbo_Grid_Cloth_Name.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()
        End If
    End Sub

    Private Sub cbo_Grid_Shift_2_Employee_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Grid_Shift_2_Employee.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Employee_Head", "Employee_name", "", "(Employee_idno = 0)")
    End Sub
    Private Sub cbo_Grid_Shift_2_Employee_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Grid_Shift_2_Employee.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Grid_Shift_2_Employee, Nothing, Nothing, "Employee_Head", "Employee_name", "", "(Employee_idno = 0)")
        With dgv_Details

            If (e.KeyValue = 38 And cbo_Grid_Shift_2_Employee.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then

                dgv_Details.Focus()
                dgv_Details.CurrentCell = dgv_Details.Rows(dgv_Details.CurrentRow.Index).Cells(DgvCol_Details.SHIFT_2_WEFT)

            End If

            If (e.KeyValue = 40 And cbo_Grid_Shift_2_Employee.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then

                'If .CurrentRow.Index = .Rows.Count - 1 Then

                '    If MessageBox.Show("Do you want to save", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = DialogResult.Yes Then
                '        save_record()
                '    Else
                '        dtp_Date.Focus()
                '    End If

                'Else
                dgv_Details.Focus()
                dgv_Details.CurrentCell = dgv_Details.Rows(dgv_Details.CurrentRow.Index).Cells(DgvCol_Details.REMARKS)


                ' End If

            End If

        End With
    End Sub

    Private Sub cbo_Grid_Shift_2_Employee_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Grid_Shift_2_Employee.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Grid_Shift_2_Employee, Nothing, "Employee_Head", "Employee_name", "", "(Employee_idno = 0)")
        If Asc(e.KeyChar) = 13 Then
            With dgv_Details
                'If Trim(.Rows(.CurrentRow.Index).Cells(DgvCol_Details.SHIFT_2_EMPLOYEE).Value) = "" And .CurrentRow.Index = .Rows.Count - 1 Then
                '    If MessageBox.Show("Do you want to save", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = DialogResult.Yes Then
                '        save_record()
                '    Else
                '        dtp_Date.Focus()
                '    End If

                'Else
                dgv_Details.Focus()
                dgv_Details.CurrentCell = dgv_Details.Rows(dgv_Details.CurrentRow.Index).Cells(DgvCol_Details.REMARKS)

                '  End If
            End With
        End If
    End Sub

    Private Sub cbo_Grid_Shift_2_Employee_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Grid_Shift_2_Employee.KeyUp
        'If e.Control = True And UCase(Chr(e.KeyCode)) = "D" Then
        '    dgv_Details_KeyUp(sender, e)
        'End If
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Dim f As New EmployeeCreation_Simple
            ' Dim f As New Payroll_Employee_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Grid_Shift_2_Employee.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()
        End If
    End Sub


    Private Sub cbo_Grid_Shift_2_Employee_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Grid_Shift_2_Employee.TextChanged
        Try
            If cbo_Grid_Shift_2_Employee.Visible Then

                If IsNothing(dgv_Details.CurrentCell) Then Exit Sub
                With dgv_Details
                    If Val(cbo_Grid_Shift_2_Employee.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = DgvCol_Details.SHIFT_2_EMPLOYEE Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_Grid_Shift_2_Employee.Text)
                    End If
                End With
            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub Cbo_Grid_Cloth_Name_TextChanged(sender As Object, e As EventArgs) Handles Cbo_Grid_Cloth_Name.TextChanged
        Try
            If Cbo_Grid_Cloth_Name.Visible Then

                If IsNothing(dgv_Details.CurrentCell) Then Exit Sub
                With dgv_Details
                    If IsNothing(dgv_Details.CurrentCell) Then Exit Sub
                    If Val(Cbo_Grid_Cloth_Name.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = DgvCol_Details.CLOTH_NAME Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(Cbo_Grid_Cloth_Name.Text)
                    End If
                End With
            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub btn_Print_Click(sender As Object, e As EventArgs) Handles btn_Print.Click
        Common_Procedures.Print_OR_Preview_Status = 1
        print_record()
    End Sub

    Private Sub Cbo_Grid_Ledger_Name_TextChanged(sender As Object, e As EventArgs) Handles Cbo_Grid_Ledger_Name.TextChanged
        Try
            If Cbo_Grid_Ledger_Name.Visible Then

                If IsNothing(dgv_Details.CurrentCell) Then Exit Sub
                With dgv_Details
                    If IsNothing(dgv_Details.CurrentCell) Then Exit Sub
                    If Val(Cbo_Grid_Ledger_Name.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = DgvCol_Details.PARTY_NAME Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(Cbo_Grid_Ledger_Name.Text)
                    End If
                End With
            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub
    Private Sub Cbo_Grid_Ledger_Name_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles Cbo_Grid_Ledger_Name.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "( ( (Ledger_Type = 'WEAVER' and Own_Loom_Status = 1) or Ledger_Type = 'GODOWN' or Ledger_Type = 'JOBWORKER' or Show_In_All_Entry = 1 ) and Close_status = 0 )", "(Ledger_idno = 0)")
    End Sub
    Private Sub Cbo_Grid_Ledger_Name_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Cbo_Grid_Ledger_Name.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, Cbo_Grid_Ledger_Name, Nothing, Nothing, "Ledger_AlaisHead", "Ledger_DisplayName", "( ( (Ledger_Type = 'WEAVER' and Own_Loom_Status = 1) or Ledger_Type = 'GODOWN' or Ledger_Type = 'JOBWORKER' or Show_In_All_Entry = 1 ) and Close_status = 0 )", "(Ledger_idno = 0)")

        With dgv_Details

            If (e.KeyValue = 38 And Cbo_Grid_Ledger_Name.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then


                dgv_Details.Focus()
                dgv_Details.CurrentCell = dgv_Details.Rows(dgv_Details.CurrentRow.Index).Cells(DgvCol_Details.REMARKS)


            End If
            If (e.KeyValue = 40 And Cbo_Grid_Ledger_Name.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then


                dgv_Details.Focus()
                dgv_Details.CurrentCell = dgv_Details.Rows(dgv_Details.CurrentRow.Index).Cells(DgvCol_Details.RATE_METER)


            End If
        End With
    End Sub
    Private Sub Cbo_Grid_Ledger_Name_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Cbo_Grid_Ledger_Name.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, Cbo_Grid_Ledger_Name, Nothing, "Ledger_AlaisHead", "Ledger_DisplayName", "( ( (Ledger_Type = 'WEAVER' and Own_Loom_Status = 1) or Ledger_Type = 'GODOWN' or Ledger_Type = 'JOBWORKER' or Show_In_All_Entry = 1 ) and Close_status = 0 )", "(Ledger_idno = 0)")
        With dgv_Details

            If Asc(e.KeyChar) = 13 Then
                dgv_Details.Focus()
                dgv_Details.CurrentCell = dgv_Details.Rows(dgv_Details.CurrentRow.Index).Cells(DgvCol_Details.RATE_METER)

            End If
        End With
    End Sub
    Private Sub Cbo_Grid_Ledger_Name_KeyUp(sender As Object, e As KeyEventArgs) Handles Cbo_Grid_Ledger_Name.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Common_Procedures.MDI_LedType = "JOBWORKER"
            Dim f As New Ledger_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = sender.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1

            f.Show()

        End If
    End Sub
    Private Sub txt_EB_Units_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txt_EB_Units.KeyPress
        If Asc(e.KeyChar) = 13 Then
            txt_EB_Amount.Focus()

        End If
    End Sub

    Private Sub txt_EB_Units_KeyDown(sender As Object, e As KeyEventArgs) Handles txt_EB_Units.KeyDown

        If e.KeyCode = 38 Then
            dgv_Details.Focus()
            dgv_Details.CurrentCell = dgv_Details.Rows(dgv_Details.CurrentRow.Index).Cells(DgvCol_Details.CLOTH_NAME)
            dgv_Details.CurrentCell.Selected = True
        ElseIf e.KeyCode = 40 Then
            txt_EB_Amount.Focus()

        End If
    End Sub

    Private Sub txt_EB_Amount_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txt_EB_Amount.KeyPress
        If Asc(e.KeyChar) = 13 Then
            txt_Employee_Salary.Focus()

        End If
    End Sub

    Private Sub txt_EB_Amount_KeyDown(sender As Object, e As KeyEventArgs) Handles txt_EB_Amount.KeyDown
        If e.KeyCode = 38 Then
            txt_EB_Units.Focus()

        ElseIf e.KeyCode = 40 Then
            txt_Employee_Salary.Focus()

        End If
    End Sub

    Private Sub txt_Employee_Salary_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txt_Employee_Salary.KeyPress
        If Asc(e.KeyChar) = 13 Then
            If MessageBox.Show("Do you want to save", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = DialogResult.Yes Then
                save_record()
            Else
                dtp_Date.Focus()
            End If

        End If
    End Sub

    Private Sub txt_Employee_Salary_KeyDown(sender As Object, e As KeyEventArgs) Handles txt_Employee_Salary.KeyDown
        If e.KeyCode = 38 Then
            txt_EB_Amount.Focus()

        ElseIf e.KeyCode = 40 Then
            If MessageBox.Show("Do you want to save", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = DialogResult.Yes Then
                save_record()
            Else
                dtp_Date.Focus()
            End If

        End If
    End Sub

    Private Sub Cbo_Grid_Ledger_Name_SelectedIndexChanged(sender As Object, e As EventArgs) Handles Cbo_Grid_Ledger_Name.SelectedIndexChanged
        Try
            If Cbo_Grid_Ledger_Name.Visible Then

                If IsNothing(dgv_Details.CurrentCell) Then Exit Sub
                With dgv_Details
                    If IsNothing(dgv_Details.CurrentCell) Then Exit Sub
                    If Val(Cbo_Grid_Ledger_Name.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = DgvCol_Details.PARTY_NAME Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(Cbo_Grid_Ledger_Name.Text)
                    End If
                End With
            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub Printing_Format2_1520(ByRef e As System.Drawing.Printing.PrintPageEventArgs)
        Dim pFont As Font
        Dim ps As Printing.PaperSize
        Dim LMargin As Single, RMargin As Single, TMargin As Single, BMargin As Single
        Dim PrintWidth As Single, PrintHeight As Single
        Dim PageWidth As Single, PageHeight As Single
        Dim I As Integer
        Dim NoofItems_PerPage As Integer, NoofDets As Integer
        Dim TxtHgt As Single
        Dim PpSzSTS As Boolean = False
        Dim LnAr(15) As Single, ClAr(20) As Single
        Dim EntryCode As String
        Dim CurY As Single
        Dim ItmNm1 As String, ItmNm2 As String
        Dim ClthNm1 As String, ClthNm2 As String
        Dim EmpNm1 As String, EmpNm2 As String
        Dim EmpNm4 As String, EmpNm3 As String
        Dim Remark1 As String, Remark2 As String
        Dim itemNmStr1(20) As String
        Dim ClthNmStr1(20) As String
        Dim EmpNmStr1(20) As String
        Dim EmpNmStr2(20) As String
        Dim RemarksStr1(20) As String
        Dim PartyNameStr1(20) As String
        Dim PartNm1 As String, PartNm2 As String

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
            .Right = 30
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
        'If PrintDocument1.DefaultPageSettings.Landscape = False Then
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
        ClAr = New Single(20) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}

        ClAr(1) = Val(25) : ClAr(2) = 40 : ClAr(3) = 95 : ClAr(4) = 30 : ClAr(5) = 30 : ClAr(6) = 35 : ClAr(7) = 25 : ClAr(8) = 25 : ClAr(9) = 50 : ClAr(10) = 30 : ClAr(11) = 35 : ClAr(12) = 25 : ClAr(13) = 25 : ClAr(14) = 50 : ClAr(15) = 30 : ClAr(16) = 40 : ClAr(17) = 45 : ClAr(18) = 45 : ClAr(19) = 30
        ClAr(20) = PageWidth - (LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12) + ClAr(13) + ClAr(14) + ClAr(15) + ClAr(16) + ClAr(17) + ClAr(18) + ClAr(19))

        TxtHgt = 19
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1520" Then
            NoofItems_PerPage = 35
        Else
            NoofItems_PerPage = 40
        End If


        EntryCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        ' Try
        If prn_HdDt.Rows.Count > 0 Then

                Printing_Format2_1520_PageHeader(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, prn_PageNo, NoofItems_PerPage, CurY, LnAr, ClAr)

                NoofDets = 0

                CurY = CurY - 10

                If prn_DetDt.Rows.Count > 0 Then

                    Do While prn_DetIndx <= prn_DetDt.Rows.Count - 1

                        If NoofDets >= NoofItems_PerPage Then

                            CurY = CurY + TxtHgt

                            Common_Procedures.Print_To_PrintDocument(e, "Continued...", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + 10, CurY, 0, 0, pFont)

                            NoofDets = NoofDets + 1

                            Printing_Format2_1520_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClAr, NoofDets, False)

                            e.HasMorePages = True
                            Return

                        End If

                        Erase itemNmStr1
                        itemNmStr1 = New String(15) {}

                        ItmNm1 = Trim(prn_DetDt.Rows(prn_DetIndx).Item("Loom_Name").ToString)
                        ItmNm2 = ""
                        If Len(ItmNm1) > 5 Then
                            For I = 5 To 1 Step -1
                                If Mid$(Trim(ItmNm1), I, 1) = " " Or Mid$(Trim(ItmNm1), I, 1) = "," Or Mid$(Trim(ItmNm1), I, 1) = "." Or Mid$(Trim(ItmNm1), I, 1) = "-" Or Mid$(Trim(ItmNm1), I, 1) = "/" Or Mid$(Trim(ItmNm1), I, 1) = "_" Or Mid$(Trim(ItmNm1), I, 1) = "(" Or Mid$(Trim(ItmNm1), I, 1) = ")" Or Mid$(Trim(ItmNm1), I, 1) = "\" Or Mid$(Trim(ItmNm1), I, 1) = "[" Or Mid$(Trim(ItmNm1), I, 1) = "]" Or Mid$(Trim(ItmNm1), I, 1) = "{" Or Mid$(Trim(ItmNm1), I, 1) = "}" Then Exit For
                            Next I
                            If I = 0 Then I = 5
                            ItmNm2 = Microsoft.VisualBasic.Right(Trim(ItmNm1), Len(ItmNm1) - I)
                            ItmNm1 = Microsoft.VisualBasic.Left(Trim(ItmNm1), I - 1)
                        End If

                        If Trim(ItmNm2) <> "" Then
                        For k = 0 To 8
                            If Len(ItmNm2) > 5 Then

                                For I = 5 To 1 Step -1
                                    If Mid$(Trim(ItmNm2), I, 1) = " " Or Mid$(Trim(ItmNm2), I, 1) = "," Or Mid$(Trim(ItmNm2), I, 1) = "." Or Mid$(Trim(ItmNm2), I, 1) = "-" Or Mid$(Trim(ItmNm2), I, 1) = "/" Or Mid$(Trim(ItmNm2), I, 1) = "_" Or Mid$(Trim(ItmNm2), I, 1) = "(" Or Mid$(Trim(ItmNm2), I, 1) = ")" Or Mid$(Trim(ItmNm2), I, 1) = "\" Or Mid$(Trim(ItmNm2), I, 1) = "[" Or Mid$(Trim(ItmNm2), I, 1) = "]" Or Mid$(Trim(ItmNm2), I, 1) = "{" Or Mid$(Trim(ItmNm2), I, 1) = "}" Then Exit For
                                Next I
                                If I = 0 Then I = 5

                                'ClthNmStr1(k) = Microsoft.VisualBasic.Right(Trim(ItmNm2), Len(ItmNm2) - I)
                                'ItmNm2 = Microsoft.VisualBasic.Left(Trim(ItmNm2), I - 1)

                                itemNmStr1(k) = Microsoft.VisualBasic.Left(Trim(ItmNm2), I)
                                ItmNm2 = Microsoft.VisualBasic.Right(Trim(ItmNm2), Len(ItmNm2) - I)

                                If Len(ItmNm2) > 5 Then
                                    ItmNm2 = ItmNm2

                                Else
                                    k = k + 1
                                    itemNmStr1(k) = ItmNm2
                                    Exit For

                                End If


                            Else
                                'k = k + 1
                                itemNmStr1(k) = ItmNm2
                                Exit For

                            End If
                        Next
                    End If



                        '-------------------

                        Erase ClthNmStr1
                        ClthNmStr1 = New String(15) {}

                        ClthNm1 = Trim(prn_DetDt.Rows(prn_DetIndx).Item("Cloth_Name").ToString)
                        ClthNm2 = ""
                        If Len(ClthNm1) > 20 Then
                            For I = 20 To 1 Step -1
                                If Mid$(Trim(ClthNm1), I, 1) = " " Or Mid$(Trim(ClthNm1), I, 1) = "," Or Mid$(Trim(ClthNm1), I, 1) = "." Or Mid$(Trim(ClthNm1), I, 1) = "-" Or Mid$(Trim(ClthNm1), I, 1) = "/" Or Mid$(Trim(ClthNm1), I, 1) = "_" Or Mid$(Trim(ClthNm1), I, 1) = "(" Or Mid$(Trim(ClthNm1), I, 1) = ")" Or Mid$(Trim(ClthNm1), I, 1) = "\" Or Mid$(Trim(ClthNm1), I, 1) = "[" Or Mid$(Trim(ClthNm1), I, 1) = "]" Or Mid$(Trim(ClthNm1), I, 1) = "{" Or Mid$(Trim(ClthNm1), I, 1) = "}" Then Exit For
                            Next I
                            If I = 0 Then I = 20
                            ClthNm2 = Microsoft.VisualBasic.Right(Trim(ClthNm1), Len(ClthNm1) - I)
                            ClthNm1 = Microsoft.VisualBasic.Left(Trim(ClthNm1), I - 1)
                        End If


                        If Trim(ClthNm2) <> "" Then
                        For k = 0 To 8
                            If Len(ClthNm2) > 20 Then

                                For I = 20 To 1 Step -1
                                    If Mid$(Trim(ClthNm2), I, 1) = " " Or Mid$(Trim(ClthNm2), I, 1) = "," Or Mid$(Trim(ClthNm2), I, 1) = "." Or Mid$(Trim(ClthNm2), I, 1) = "-" Or Mid$(Trim(ClthNm2), I, 1) = "/" Or Mid$(Trim(ClthNm2), I, 1) = "_" Or Mid$(Trim(ClthNm2), I, 1) = "(" Or Mid$(Trim(ClthNm2), I, 1) = ")" Or Mid$(Trim(ClthNm2), I, 1) = "\" Or Mid$(Trim(ClthNm2), I, 1) = "[" Or Mid$(Trim(ClthNm2), I, 1) = "]" Or Mid$(Trim(ClthNm2), I, 1) = "{" Or Mid$(Trim(ClthNm2), I, 1) = "}" Then Exit For
                                Next I
                                If I = 0 Then I = 20

                                'ClthNmStr1(k) = Microsoft.VisualBasic.Right(Trim(ClthNm2), Len(ClthNm2) - I)
                                'ClthNm2 = Microsoft.VisualBasic.Left(Trim(ClthNm2), I - 1)

                                ClthNmStr1(k) = Microsoft.VisualBasic.Left(Trim(ClthNm2), I)
                                ClthNm2 = Microsoft.VisualBasic.Right(Trim(ClthNm2), Len(ClthNm2) - I)

                                If Len(ClthNm2) > 20 Then
                                    ClthNmStr1(k) = ClthNm2

                                    'ClthNm2 = ClthNm2

                                Else
                                    k = k + 1
                                    ClthNmStr1(k) = ClthNm2
                                    Exit For

                                End If


                            Else
                                ' k = k + 1
                                ClthNmStr1(k) = ClthNm2
                                Exit For

                            End If
                        Next
                    End If

                        '-----------------

                        Erase EmpNmStr1
                        EmpNmStr1 = New String(10) {}

                        EmpNm1 = Trim(prn_DetDt.Rows(prn_DetIndx).Item("Employee_shift_1").ToString)
                        EmpNm2 = ""

                        If Len(EmpNm1) > 8 Then
                            For I = 8 To 1 Step -1
                                If Mid$(Trim(EmpNm1), I, 1) = " " Or Mid$(Trim(EmpNm1), I, 1) = "," Or Mid$(Trim(EmpNm1), I, 1) = "." Or Mid$(Trim(EmpNm1), I, 1) = "-" Or Mid$(Trim(EmpNm1), I, 1) = "/" Or Mid$(Trim(EmpNm1), I, 1) = "_" Or Mid$(Trim(EmpNm1), I, 1) = "(" Or Mid$(Trim(EmpNm1), I, 1) = ")" Or Mid$(Trim(EmpNm1), I, 1) = "\" Or Mid$(Trim(EmpNm1), I, 1) = "[" Or Mid$(Trim(EmpNm1), I, 1) = "]" Or Mid$(Trim(EmpNm1), I, 1) = "{" Or Mid$(Trim(EmpNm1), I, 1) = "}" Then Exit For
                            Next I
                            If I = 0 Then I = 8
                            EmpNm2 = Microsoft.VisualBasic.Right(Trim(EmpNm1), Len(EmpNm1) - I)
                            EmpNm1 = Microsoft.VisualBasic.Left(Trim(EmpNm1), I - 1)
                        End If


                        If Trim(EmpNm2) <> "" Then
                            For k = 0 To 8
                                If Len(EmpNm2) > 8 Then

                                    For I = 8 To 1 Step -1
                                        If Mid$(Trim(EmpNm2), I, 1) = " " Or Mid$(Trim(EmpNm2), I, 1) = "," Or Mid$(Trim(EmpNm2), I, 1) = "." Or Mid$(Trim(EmpNm2), I, 1) = "-" Or Mid$(Trim(EmpNm2), I, 1) = "/" Or Mid$(Trim(EmpNm2), I, 1) = "_" Or Mid$(Trim(EmpNm2), I, 1) = "(" Or Mid$(Trim(EmpNm2), I, 1) = ")" Or Mid$(Trim(EmpNm2), I, 1) = "\" Or Mid$(Trim(EmpNm2), I, 1) = "[" Or Mid$(Trim(EmpNm2), I, 1) = "]" Or Mid$(Trim(EmpNm2), I, 1) = "{" Or Mid$(Trim(EmpNm2), I, 1) = "}" Then Exit For
                                    Next I
                                    If I = 0 Then I = 8



                                    EmpNmStr1(k) = Microsoft.VisualBasic.Left(Trim(EmpNm2), I)
                                    EmpNm2 = Microsoft.VisualBasic.Right(Trim(EmpNm2), Len(EmpNm2) - I)

                                    If Len(EmpNm2) > 8 Then
                                        EmpNm2 = EmpNm2

                                    Else
                                        k = k + 1
                                        EmpNmStr1(k) = EmpNm2
                                        Exit For

                                    End If


                                Else
                                'k = k + 1
                                EmpNmStr1(k) = EmpNm2
                                    Exit For

                                End If
                            Next
                        End If

                        '---------------------

                        '-----------------

                        Erase EmpNmStr2
                        EmpNmStr2 = New String(10) {}

                        EmpNm3 = Trim(prn_DetDt.Rows(prn_DetIndx).Item("Employee_shift_2").ToString)
                        EmpNm4 = ""

                        If Len(EmpNm3) > 8 Then
                            For I = 8 To 1 Step -1
                                If Mid$(Trim(EmpNm3), I, 1) = " " Or Mid$(Trim(EmpNm3), I, 1) = "," Or Mid$(Trim(EmpNm3), I, 1) = "." Or Mid$(Trim(EmpNm3), I, 1) = "-" Or Mid$(Trim(EmpNm3), I, 1) = "/" Or Mid$(Trim(EmpNm3), I, 1) = "_" Or Mid$(Trim(EmpNm3), I, 1) = "(" Or Mid$(Trim(EmpNm3), I, 1) = ")" Or Mid$(Trim(EmpNm3), I, 1) = "\" Or Mid$(Trim(EmpNm3), I, 1) = "[" Or Mid$(Trim(EmpNm3), I, 1) = "]" Or Mid$(Trim(EmpNm3), I, 1) = "{" Or Mid$(Trim(EmpNm3), I, 1) = "}" Then Exit For
                            Next I
                            If I = 0 Then I = 8
                            EmpNm4 = Microsoft.VisualBasic.Right(Trim(EmpNm3), Len(EmpNm3) - I)
                            EmpNm3 = Microsoft.VisualBasic.Left(Trim(EmpNm3), I - 1)
                        End If


                        If Trim(EmpNm4) <> "" Then
                            For k = 0 To 8
                                If Len(EmpNm4) > 8 Then

                                    For I = 8 To 1 Step -1
                                        If Mid$(Trim(EmpNm4), I, 1) = " " Or Mid$(Trim(EmpNm4), I, 1) = "," Or Mid$(Trim(EmpNm4), I, 1) = "." Or Mid$(Trim(EmpNm4), I, 1) = "-" Or Mid$(Trim(EmpNm4), I, 1) = "/" Or Mid$(Trim(EmpNm4), I, 1) = "_" Or Mid$(Trim(EmpNm4), I, 1) = "(" Or Mid$(Trim(EmpNm4), I, 1) = ")" Or Mid$(Trim(EmpNm4), I, 1) = "\" Or Mid$(Trim(EmpNm4), I, 1) = "[" Or Mid$(Trim(EmpNm4), I, 1) = "]" Or Mid$(Trim(EmpNm4), I, 1) = "{" Or Mid$(Trim(EmpNm4), I, 1) = "}" Then Exit For
                                    Next I
                                    If I = 0 Then I = 8

                                    'ClthNmStr1(k) = Microsoft.VisualBasic.Right(Trim(EMPNM4), Len(EMPNM4) - I)
                                    'EMPNM4 = Microsoft.VisualBasic.Left(Trim(EMPNM4), I - 1)

                                    EmpNmStr2(k) = Microsoft.VisualBasic.Left(Trim(EmpNm4), I)
                                    EmpNm4 = Microsoft.VisualBasic.Right(Trim(EmpNm4), Len(EmpNm4) - I)

                                    If Len(EmpNm4) > 8 Then
                                        EmpNm4 = EmpNm4

                                    Else
                                        k = k + 1
                                        EmpNmStr2(k) = EmpNm4
                                        Exit For

                                    End If


                                Else
                                ' k = k + 1
                                EmpNmStr2(k) = EmpNm4
                                    Exit For

                                End If
                            Next
                        End If

                        '---------------------


                        Erase RemarksStr1
                        RemarksStr1 = New String(10) {}

                        Remark1 = Trim(prn_DetDt.Rows(prn_DetIndx).Item("Remarks").ToString)
                        Remark2 = ""

                        If Len(Remark1) > 8 Then
                            For I = 8 To 1 Step -1
                                If Mid$(Trim(Remark1), I, 1) = " " Or Mid$(Trim(Remark1), I, 1) = "," Or Mid$(Trim(Remark1), I, 1) = "." Or Mid$(Trim(Remark1), I, 1) = "-" Or Mid$(Trim(Remark1), I, 1) = "/" Or Mid$(Trim(Remark1), I, 1) = "_" Or Mid$(Trim(Remark1), I, 1) = "(" Or Mid$(Trim(Remark1), I, 1) = ")" Or Mid$(Trim(Remark1), I, 1) = "\" Or Mid$(Trim(Remark1), I, 1) = "[" Or Mid$(Trim(Remark1), I, 1) = "]" Or Mid$(Trim(Remark1), I, 1) = "{" Or Mid$(Trim(Remark1), I, 1) = "}" Then Exit For
                            Next I
                            If I = 0 Then I = 8
                            Remark2 = Microsoft.VisualBasic.Right(Trim(Remark1), Len(Remark1) - I)
                            Remark1 = Microsoft.VisualBasic.Left(Trim(Remark1), I - 1)
                        End If


                        If Trim(Remark2) <> "" Then
                            For k = 0 To 8
                                If Len(Remark2) > 8 Then

                                    For I = 8 To 1 Step -1
                                        If Mid$(Trim(Remark2), I, 1) = " " Or Mid$(Trim(Remark2), I, 1) = "," Or Mid$(Trim(Remark2), I, 1) = "." Or Mid$(Trim(Remark2), I, 1) = "-" Or Mid$(Trim(Remark2), I, 1) = "/" Or Mid$(Trim(Remark2), I, 1) = "_" Or Mid$(Trim(Remark2), I, 1) = "(" Or Mid$(Trim(Remark2), I, 1) = ")" Or Mid$(Trim(Remark2), I, 1) = "\" Or Mid$(Trim(Remark2), I, 1) = "[" Or Mid$(Trim(Remark2), I, 1) = "]" Or Mid$(Trim(Remark2), I, 1) = "{" Or Mid$(Trim(Remark2), I, 1) = "}" Then Exit For
                                    Next I
                                    If I = 0 Then I = 8

                                    RemarksStr1(k) = Microsoft.VisualBasic.Left(Trim(Remark2), I)
                                    Remark2 = Microsoft.VisualBasic.Right(Trim(Remark2), Len(Remark2) - I)

                                    If Len(Remark2) > 8 Then
                                        Remark2 = Remark2

                                    Else
                                        k = k + 1
                                        RemarksStr1(k) = Remark2
                                        Exit For

                                    End If
                                Else
                                ' k = k + 1
                                RemarksStr1(k) = Remark2
                                    Exit For

                                End If
                            Next
                        End If

                        '---------------------   

                        '---------------------


                        Erase PartyNameStr1
                        PartyNameStr1 = New String(10) {}

                        PartNm1 = Common_Procedures.Ledger_IdNoToName(con, prn_DetDt.Rows(prn_DetIndx).Item("Ledger_Idno").ToString)
                        PartNm2 = ""

                    If Len(PartNm1) > 9 Then
                        For I = 10 To 1 Step -1
                            If Mid$(Trim(PartNm1), I, 1) = " " Or Mid$(Trim(PartNm1), I, 1) = "," Or Mid$(Trim(PartNm1), I, 1) = "." Or Mid$(Trim(PartNm1), I, 1) = "-" Or Mid$(Trim(PartNm1), I, 1) = "/" Or Mid$(Trim(PartNm1), I, 1) = "_" Or Mid$(Trim(PartNm1), I, 1) = "(" Or Mid$(Trim(PartNm1), I, 1) = ")" Or Mid$(Trim(PartNm1), I, 1) = "\" Or Mid$(Trim(PartNm1), I, 1) = "[" Or Mid$(Trim(PartNm1), I, 1) = "]" Or Mid$(Trim(PartNm1), I, 1) = "{" Or Mid$(Trim(PartNm1), I, 1) = "}" Then Exit For
                        Next I
                        If I = 0 Then I = 7
                        PartNm2 = Microsoft.VisualBasic.Right(Trim(PartNm1), Len(PartNm1) - I)
                        PartNm1 = Microsoft.VisualBasic.Left(Trim(PartNm1), I - 1)
                    End If


                    If Trim(PartNm2) <> "" Then
                            For k = 0 To 8
                                If Len(PartNm2) > 8 Then

                                    For I = 8 To 1 Step -1
                                        If Mid$(Trim(PartNm2), I, 1) = " " Or Mid$(Trim(PartNm2), I, 1) = "," Or Mid$(Trim(PartNm2), I, 1) = "." Or Mid$(Trim(PartNm2), I, 1) = "-" Or Mid$(Trim(PartNm2), I, 1) = "/" Or Mid$(Trim(PartNm2), I, 1) = "_" Or Mid$(Trim(PartNm2), I, 1) = "(" Or Mid$(Trim(PartNm2), I, 1) = ")" Or Mid$(Trim(PartNm2), I, 1) = "\" Or Mid$(Trim(PartNm2), I, 1) = "[" Or Mid$(Trim(PartNm2), I, 1) = "]" Or Mid$(Trim(PartNm2), I, 1) = "{" Or Mid$(Trim(PartNm2), I, 1) = "}" Then Exit For
                                    Next I
                                    If I = 0 Then I = 8

                                PartyNameStr1(k) = Microsoft.VisualBasic.Left(Trim(PartNm2), I)
                                PartNm2 = Microsoft.VisualBasic.Right(Trim(PartNm2), Len(PartNm2) - I)

                                If Len(PartNm2) > 8 Then
                                        PartNm2 = PartNm2

                                    Else
                                        k = k + 1
                                        PartyNameStr1(k) = PartNm2
                                        Exit For

                                    End If
                                Else
                                ' k = k + 1
                                PartyNameStr1(k) = PartNm2
                                    Exit For

                                End If
                            Next
                        End If

                    '---------------------



                    pFont = New Font("Calibri", 7, FontStyle.Regular)
                    CurY = CurY + TxtHgt
                        Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetDt.Rows(prn_DetIndx).Item("Sl_No").ToString), LMargin + 10, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm1), LMargin + ClAr(1) + 10, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Trim(ClthNm1), LMargin + ClAr(1) + ClAr(2), CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(prn_DetIndx).Item("RPM").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) - 3, CurY, 1, 0, pFont)

                        Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(prn_DetIndx).Item("Shift_1_Pick_Efficiency").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 3, CurY, 1, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(prn_DetIndx).Item("Shift1_Mtrs").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 3, CurY, 1, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(prn_DetIndx).Item("Shift_1_Warp_Breakage").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 3, CurY, 1, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(prn_DetIndx).Item("Shift_1_weft_Breakage").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 3, CurY, 1, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Trim(EmpNm1), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, 0, 0, pFont)

                        Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(prn_DetIndx).Item("Shift_2_Pick_Efficiency").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) - 3, CurY, 1, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(prn_DetIndx).Item("Shift2_Mtrs").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) - 3, CurY, 1, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(prn_DetIndx).Item("Shift_2_Warp_Breakage").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12) - 3, CurY, 1, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(prn_DetIndx).Item("Shift_2_Warp_Breakage").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12) + ClAr(13) - 3, CurY, 1, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Trim(EmpNm3), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12) + ClAr(13), CurY, 0, 0, pFont)

                        Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(prn_DetIndx).Item("Pick_Efficiency").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12) + ClAr(13) + ClAr(14) + ClAr(15) - 3, CurY, 1, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(prn_DetIndx).Item("Meters").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12) + ClAr(13) + ClAr(14) + ClAr(15) + ClAr(16) - 3, CurY, 1, 0, pFont)

                        If Trim(Remark1) <> "" Then
                            Common_Procedures.Print_To_PrintDocument(e, Trim(Remark1), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12) + ClAr(13) + ClAr(14) + ClAr(15) + ClAr(16), CurY, 0, 0, pFont)
                        End If

                        Common_Procedures.Print_To_PrintDocument(e, Trim(PartNm1), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12) + ClAr(13) + ClAr(14) + ClAr(15) + ClAr(16) + ClAr(17), CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(prn_DetIndx).Item("Rate_Meter").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12) + ClAr(13) + ClAr(14) + ClAr(15) + ClAr(16) + ClAr(17) + ClAr(18) + ClAr(19) - 3, CurY, 1, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Amount").ToString, PageWidth - 10, CurY, 1, 0, pFont)
                    'Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(prn_DetIndx).Item("Amount").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12) + ClAr(13) + ClAr(14) + ClAr(15) + ClAr(16) + ClAr(17) + ClAr(18) + ClAr(19), CurY, 2, 0, pFont)



                    NoofDets = NoofDets + 1


                    'If Trim(ItmNm2) <> "" Then
                    '    '     CurY = CurY + TxtHgt
                    '    Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm2), LMargin + ClAr(1) + 10, CurY, 0, 0, pFont)

                    'End If
                    For I = 0 To 9
                        If Trim(itemNmStr1(I)) <> "" Or Trim(ClthNmStr1(I)) <> "" Or Trim(EmpNmStr1(I)) <> "" Or Trim(EmpNmStr2(I)) <> "" Or Trim(RemarksStr1(I)) <> "" Or Trim(PartyNameStr1(I)) <> "" Then
                            CurY = CurY + TxtHgt
                            NoofDets = NoofDets + 1
                            Common_Procedures.Print_To_PrintDocument(e, Trim(itemNmStr1(I)), LMargin + ClAr(1) + 10, CurY, 0, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, Trim(ClthNmStr1(I)), LMargin + ClAr(1) + ClAr(2), CurY, 0, PageWidth, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, Trim(EmpNmStr1(I)), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, 0, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, Trim(EmpNmStr2(I)), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12) + ClAr(13), CurY, 0, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, Trim(RemarksStr1(I)), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12) + ClAr(13) + ClAr(14) + ClAr(15) + ClAr(16), CurY, 0, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, Trim(PartyNameStr1(I)), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12) + ClAr(13) + ClAr(14) + ClAr(15) + ClAr(16) + ClAr(17), CurY, 0, 0, pFont)

                        End If
                    Next

                    prn_DetIndx = prn_DetIndx + 1

                    Loop

                End If

                Printing_Format2_1520_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClAr, NoofDets, True)

            End If

        'Catch ex As Exception
        '    MessageBox.Show(ex.Message, "DOES Not PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        'End Try

        e.HasMorePages = False

    End Sub

    Private Sub Printing_Format2_1520_PageHeader(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByRef PageNo As Integer, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single)
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt2 As New DataTable
        Dim p1Font As Font
        Dim Cmp_Name As String, Cmp_Add1 As String, Cmp_Add2 As String
        Dim Cmp_PhNo As String, Cmp_TinNo As String, Cmp_CstNo As String
        Dim strHeight As Single
        Dim C1 As Single
        Dim C2 As Single
        Dim W1 As Single
        Dim S1 As Single

        PageNo = PageNo + 1

        CurY = TMargin

        da2 = New SqlClient.SqlDataAdapter("Select a.*,eh.Employee_Name,Lh.Loom_Name,sh.Shift_Name from LoomNo_Production_Details a LEFT OUTER JOIN PayRoll_Employee_Head eh On a.Employee_Idno = eh.Employee_IdNo LEFT OUTER JOIN Loom_Head Lh On a.Loom_IdNo = Lh.Loom_IdNo LEFT OUTER JOIN Shift_Head sh On a.Shift_IdNo = sh.Shift_IdNo where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " And a.LoomNo_Production_Code = '" & Trim(EntryCode) & "' Order by a.Sl_No", con)
        da2.Fill(dt2)

        If dt2.Rows.Count > NoofItems_PerPage Then
            Common_Procedures.Print_To_PrintDocument(e, "Page : " & Trim(Val(PageNo)), PageWidth - 10, CurY - TxtHgt, 1, 0, pFont)
        End If
        dt2.Clear()

        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(1) = CurY

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

        If Trim(Common_Procedures.settings.CustomerCode) <> "1234" Then '-----ARULJOTHI EXPORTS PVT LTD

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

        End If


        'CurY = CurY + TxtHgt - 5
        'p1Font = New Font("Calibri", 16, FontStyle.Bold)
        'Common_Procedures.Print_To_PrintDocument(e, "DELIVERY", LMargin, CurY, 2, PrintWidth, p1Font)
        'strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height
        p1Font = New Font("Calibri", 15, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "PRODUCTION REPORT", LMargin, CurY, 2, PageWidth, p1Font)


        CurY = CurY + strHeight
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(2) = CurY
        C1 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4)
        C2 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9)
        W1 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12) + ClAr(13) + ClAr(14)

        Common_Procedures.Print_To_PrintDocument(e, "DATE :  " & Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("LoomNo_Production_Date").ToString), "dd-MM-yyyy").ToString, LMargin + ClAr(1), CurY, 2, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, "SHIFT A", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, 2, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, "SHIFT B", LMargin + C2 + 50, CurY, 2, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, "TOTAL", LMargin + W1 + ClAr(15) + ClAr(16), CurY, 2, 0, pFont)

        CurY = CurY + TxtHgt
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(3) = CurY


        CurY = CurY + TxtHgt
        e.Graphics.DrawLine(Pens.Black, LMargin + C1, CurY, LMargin + C1, LnAr(2))
        e.Graphics.DrawLine(Pens.Black, LMargin + C2, CurY, LMargin + C2, LnAr(2))
        e.Graphics.DrawLine(Pens.Black, LMargin + W1, CurY, LMargin + W1, LnAr(2))

        pFont = New Font("Calibri", 8, FontStyle.Bold)
        CurY = CurY + TxtHgt - 20
        Common_Procedures.Print_To_PrintDocument(e, "SL", LMargin, CurY, 2, ClAr(1), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "NO", LMargin, CurY + 15, 2, ClAr(1), pFont)

        Common_Procedures.Print_To_PrintDocument(e, "LOOM", LMargin + ClAr(1), CurY, 2, ClAr(2), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "NO", LMargin + ClAr(1), CurY + 15, 2, ClAr(2), pFont)

        Common_Procedures.Print_To_PrintDocument(e, "CLOTH", LMargin + ClAr(1) + ClAr(2), CurY, 2, ClAr(3), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "NAME", LMargin + ClAr(1) + ClAr(2), CurY + 15, 2, ClAr(3), pFont)

        Common_Procedures.Print_To_PrintDocument(e, "RPM", LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, 2, ClAr(4), pFont)

        Common_Procedures.Print_To_PrintDocument(e, "PICK", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 2, ClAr(5), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "EFF", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY + 15, 2, ClAr(5), pFont)

        Common_Procedures.Print_To_PrintDocument(e, "MTR", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, 2, ClAr(6), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "WA", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, 2, ClAr(7), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "-RP", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY + 15, 2, ClAr(7), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "WE", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, 2, ClAr(8), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "FT", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY + 15, 2, ClAr(8), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "EMP", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, 2, ClAr(9), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "NAME", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY + 15, 2, ClAr(9), pFont)

        Common_Procedures.Print_To_PrintDocument(e, "PICK", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9), CurY, 2, ClAr(10), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "EFF", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9), CurY + 15, 2, ClAr(10), pFont)

        Common_Procedures.Print_To_PrintDocument(e, "MTR", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10), CurY, 2, ClAr(11), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "WA", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11), CurY, 2, ClAr(12), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "-RP", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11), CurY + 15, 2, ClAr(12), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "WE", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12), CurY, 2, ClAr(13), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "-FT", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12), CurY + 15, 2, ClAr(13), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "EMP", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12) + ClAr(13), CurY, 2, ClAr(14), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "NAME", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12) + ClAr(13), CurY + 15, 2, ClAr(14), pFont)

        Common_Procedures.Print_To_PrintDocument(e, "TOT", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12) + ClAr(13) + ClAr(14), CurY, 2, ClAr(15), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "EFF", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12) + ClAr(13) + ClAr(14), CurY + 15, 2, ClAr(15), pFont)

        Common_Procedures.Print_To_PrintDocument(e, "TOT", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12) + ClAr(13) + ClAr(14) + ClAr(15), CurY, 2, ClAr(16), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "MTR", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12) + ClAr(13) + ClAr(14) + ClAr(15), CurY + 15, 2, ClAr(16), pFont)

        Common_Procedures.Print_To_PrintDocument(e, "REMA", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12) + ClAr(13) + ClAr(14) + ClAr(15) + ClAr(16), CurY, 2, ClAr(17), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "-RKS", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12) + ClAr(13) + ClAr(14) + ClAr(15) + ClAr(16), CurY + 15, 2, ClAr(17), pFont)

        Common_Procedures.Print_To_PrintDocument(e, "PARTY", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12) + ClAr(13) + ClAr(14) + ClAr(15) + ClAr(16) + ClAr(17), CurY, 2, ClAr(18), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "NAME", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12) + ClAr(13) + ClAr(14) + ClAr(15) + ClAr(16) + ClAr(17), CurY + 15, 2, ClAr(18), pFont)

        Common_Procedures.Print_To_PrintDocument(e, "RATE", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12) + ClAr(13) + ClAr(14) + ClAr(15) + ClAr(16) + ClAr(17) + ClAr(18), CurY, 2, ClAr(19), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "AMOUNT", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12) + ClAr(13) + ClAr(14) + ClAr(15) + ClAr(16) + ClAr(17) + ClAr(18) + ClAr(19), CurY, 2, ClAr(20), pFont)

        CurY = CurY + TxtHgt + 15
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(4) = CurY


    End Sub

    Private Sub Printing_Format2_1520_PageFooter(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single, ByVal NoofDets As Integer, ByVal is_LastPage As Boolean)
        Dim i As Integer
        Dim Cmp_Name As String
        Dim p1Font As Font


        For i = NoofDets + 1 To NoofItems_PerPage
            CurY = CurY + TxtHgt
        Next

        CurY = CurY + TxtHgt
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(5) = CurY
        pFont = New Font("Calibri", 8, FontStyle.Regular)
        CurY = CurY + TxtHgt - 10
        If is_LastPage = True Then
            Common_Procedures.Print_To_PrintDocument(e, " TOTAL", LMargin + ClAr(1) + ClAr(2) + 20, CurY, 2, ClAr(4), pFont)
            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Total_Avg_RPM").ToString), "########0"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 1, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Total_Avg_Shift_1_Pick_Efficiency").ToString), "#########0"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, 1, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Total_Shift1_Mtrs").ToString), "########0"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, 1, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Total_Avg_Shift_2_Pick_Efficiency").ToString), "#########0"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10), CurY, 1, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Total_Shift2_Mtrs").ToString), "########0"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11), CurY, 1, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Total_PickEfficiency").ToString), "########0"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12) + ClAr(13) + ClAr(14) + ClAr(15), CurY, 1, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Total_Meters").ToString), "#########0"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12) + ClAr(13) + ClAr(14) + ClAr(15) + ClAr(16), CurY, 1, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Total_aMOUNT").ToString), "#########0.00"), PageWidth - 5, CurY, 1, 0, pFont)
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
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11), LnAr(3))

        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12) + ClAr(13), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12) + ClAr(13), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12) + ClAr(13) + ClAr(14), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12) + ClAr(13) + ClAr(14), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12) + ClAr(13) + ClAr(14) + ClAr(15), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12) + ClAr(13) + ClAr(14) + ClAr(15), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12) + ClAr(13) + ClAr(14) + ClAr(15) + ClAr(16), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12) + ClAr(13) + ClAr(14) + ClAr(15) + ClAr(16), LnAr(3))

        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12) + ClAr(13) + ClAr(14) + ClAr(15) + ClAr(16) + ClAr(17), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12) + ClAr(13) + ClAr(14) + ClAr(15) + ClAr(16) + ClAr(17), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12) + ClAr(13) + ClAr(14) + ClAr(15) + ClAr(16) + ClAr(17) + ClAr(18), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12) + ClAr(13) + ClAr(14) + ClAr(15) + ClAr(16) + ClAr(17) + ClAr(18), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12) + ClAr(13) + ClAr(14) + ClAr(15) + ClAr(16) + ClAr(17) + ClAr(18) + ClAr(19), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12) + ClAr(13) + ClAr(14) + ClAr(15) + ClAr(16) + ClAr(17) + ClAr(18) + ClAr(19), LnAr(3))



        If is_LastPage = True Then

            If Val(prn_HdDt.Rows(0).Item("EB_Units_Consumed").ToString) <> 0 Or Val(prn_HdDt.Rows(0).Item("EB_Amount").ToString) <> 0 Or Val(prn_HdDt.Rows(0).Item("Employee_Salary").ToString) <> 0 Then
                Dim vStringWidth As Single = 0

                CurY = CurY + TxtHgt - 15

                Common_Procedures.Print_To_PrintDocument(e, "EB Units :  " & Format(Val(prn_HdDt.Rows(0).Item("EB_Units_Consumed").ToString), "##########0.00"), LMargin + 10, CurY, 0, 0, pFont)

                Common_Procedures.Print_To_PrintDocument(e, "EB Amount :  " & Format(Val(prn_HdDt.Rows(0).Item("EB_Amount").ToString), "##########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + 10, CurY, 0, 0, pFont)

                Common_Procedures.Print_To_PrintDocument(e, "Employee Salary :  " & Format(Val(prn_HdDt.Rows(0).Item("Employee_Salary").ToString), "##########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + 10, CurY, 0, 0, pFont)

                CurY = CurY + TxtHgt
                e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

            End If
        End If



        Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString
        p1Font = New Font("Calibri", 12, FontStyle.Bold)

        If is_LastPage = True Then
            CurY = CurY + TxtHgt - 5
            Common_Procedures.Print_To_PrintDocument(e, "For " & Cmp_Name, PageWidth - 15, CurY, 1, 0, p1Font)

            CurY = CurY + TxtHgt + 10
            CurY = CurY + TxtHgt
            CurY = CurY + TxtHgt


            Common_Procedures.Print_To_PrintDocument(e, "Prepared by", LMargin + ClAr(1) + ClAr(2) + 20, CurY, 1, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "Checked by", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, 1, 0, pFont)
            'End If
            CurY = CurY + TxtHgt + 10

            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3), LnAr(6))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), LnAr(6))
        End If
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

        e.Graphics.DrawLine(Pens.Black, LMargin, LnAr(1), LMargin, CurY)
        e.Graphics.DrawLine(Pens.Black, PageWidth, LnAr(1), PageWidth, CurY)

    End Sub


End Class