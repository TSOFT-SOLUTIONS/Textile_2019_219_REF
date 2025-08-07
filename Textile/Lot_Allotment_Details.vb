Public Class Lot_Allotment_Details
    Implements Interface_MDIActions

    Private con As New SqlClient.SqlConnection(Common_Procedures.Connection_String)
    Private con1 As New SqlClient.SqlConnection(Common_Procedures.ConnectionString_CompanyGroupdetails)
    Private FrmLdSTS As Boolean = False
    Private New_Entry As Boolean = False
    Private Insert_Entry As Boolean = False
    Private Filter_Status As Boolean = False
    Private Pk_Condition As String = "LTALT-"
    Private NoCalc_Status As Boolean = False
    Private Prec_ActCtrl As New Control
    Private vcbo_KeyDwnVal As Double
    Private vCbo_ItmNm As String
    Private EntFnYrCode As String = ""
    Private WithEvents dgtxt_Details As New DataGridViewTextBoxEditingControl

    Private prn_HdDt As New DataTable
    Private prn_DetDt As New DataTable
    Private prn_PageNo As Integer
    Private prn_DetIndx As Integer
    Private prn_DetAr(50, 10) As String
    Private prn_DetMxIndx As Integer
    Private prn_NoofBmDets As Integer
    Private prn_DetSNo As Integer

    Public vmskOldText As String = ""
    Public vmskSelStrt As Integer = -1

    Private Sub clear()

        NoCalc_Status = True

        New_Entry = False
        Insert_Entry = False


        lbl_RefNo.Text = ""
        lbl_RefNo.ForeColor = Color.Black

        Pnl_Back.Enabled = True
        pnl_Filter.Visible = False
        pnl_Selection.Visible = False
        txt_lot_no.Text = ""
        txt_lot_code.Text = ""
        'lbl_DcNo.Text = ""
        'lbl_DcNo.ForeColor = Color.Black

        msk_date.Text = ""
        dtp_Date.Text = ""
        msk_AllottedDate.Text = ""
        dtp_AllottedDate.Text = ""

        lbl_WeaverName.Text = ""
        lbl_ClothName.Text = ""
        'cbo_Rec_Ledger.Text = ""
        cbo_Grid_TableNo.Text = ""
        cbo_Grid_checker.Text = ""
        cbo_grid_folder.Text = ""
        cbo_Grid_Supervisor.Text = ""
        cbo_Lotcode_Selection.Text = ""
        cbo_checker.Text = ""
        cbo_folder.Text = ""

        cbo_Filter_CountName.Text = ""
        cbo_Filter_MillName.Text = ""
        cbo_Filter_PartyName.Text = ""

        txt_receipt_mtrs.Text = ""
        'cbo_Vechile.Text = ""
        txt_total_pcs.Text = ""
        'txt_Freight.Text = ""
        'cbo_TransportName.Text = ""
        'cbo_Type.Text = "SELECTION"
        lbl_UserName.Text = "USER : " & Trim(UCase(Common_Procedures.User_IdNoToName(con1, Common_Procedures.User.IdNo)))

        cbo_Checking_Section.Text = ""

        cbo_Checking_Section.Enabled = True
        cbo_Checking_Section.BackColor = Color.White


        'cbo_clothname.Enabled = True
        'cbo_clothname.BackColor = Color.White

        'cbo_Rec_Ledger.Enabled = True
        'cbo_Rec_Ledger.BackColor = Color.White

        'cbo_TransportName.Enabled = True
        'cbo_TransportName.BackColor = Color.White

        'cbo_Vechile.Enabled = True
        'cbo_Vechile.BackColor = Color.White

        'txt_total_pcs.Enabled = True
        'txt_total_pcs.BackColor = Color.White

        'txt_Freight.Enabled = True
        'txt_Freight.BackColor = Color.White


        cbo_Grid_TableNo.Enabled = True
        cbo_Grid_TableNo.BackColor = Color.White

        cbo_Grid_checker.Enabled = True
        cbo_Grid_checker.BackColor = Color.White

        cbo_grid_folder.Enabled = True
        cbo_grid_folder.BackColor = Color.White

        dgv_details.Rows.Clear()
        dgv_YarnDetails_Total.Rows.Clear()
        dgv_YarnDetails_Total.Rows.Add()

        If Filter_Status = False Then
            dtp_Filter_Fromdate.Text = ""
            dtp_Filter_ToDate.Text = ""
            cbo_Filter_PartyName.Text = ""
            cbo_Filter_CountName.Text = ""
            cbo_Filter_PartyName.SelectedIndex = -1
            cbo_Filter_CountName.SelectedIndex = -1
            dgv_Filter_Details.Rows.Clear()
        End If

        Grid_Cell_DeSelect()

        cbo_Grid_TableNo.Visible = False
        cbo_Grid_checker.Visible = False
        cbo_Grid_Supervisor.Visible = False
        cbo_grid_folder.Visible = False

        NoCalc_Status = False
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
            Msktxbx.SelectAll()
        End If

        If Me.ActiveControl.Name <> cbo_Grid_TableNo.Name Then
            cbo_Grid_TableNo.Visible = False
        End If
        If Me.ActiveControl.Name <> cbo_Grid_checker.Name Then
            cbo_Grid_checker.Visible = False
        End If
        If Me.ActiveControl.Name <> cbo_Grid_Supervisor.Name Then
            cbo_Grid_Supervisor.Visible = False
        End If

        If Me.ActiveControl.Name <> cbo_grid_folder.Name Then
            cbo_grid_folder.Visible = False
        End If
        If Me.ActiveControl.Name <> dgv_YarnDetails_Total.Name Then
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
        If Not IsNothing(dgv_details.CurrentCell) Then dgv_details.CurrentCell.Selected = False
        If Not IsNothing(dgv_YarnDetails_Total.CurrentCell) Then dgv_YarnDetails_Total.CurrentCell.Selected = False
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
        If Not IsNothing(dgv_details.CurrentCell) Then dgv_details.CurrentCell.Selected = False
        If Not IsNothing(dgv_YarnDetails_Total.CurrentCell) Then dgv_YarnDetails_Total.CurrentCell.Selected = False
        If Not IsNothing(dgv_Filter_Details.CurrentCell) Then dgv_Filter_Details.CurrentCell.Selected = False
    End Sub

    Private Sub Lot_Allotment_Details_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Activated

        Try


            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Grid_Supervisor.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "EMPLOYEE" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Grid_Supervisor.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Grid_TableNo.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "TABLENO" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Grid_TableNo.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Grid_checker.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "EMPLOYEE" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Grid_checker.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_grid_folder.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "EMPLOYEE" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_grid_folder.Text = Trim(Common_Procedures.Master_Return.Return_Value)
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
            'MessageBox.Show(ex.Message, "DOES NOT SHOW...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        FrmLdSTS = False
    End Sub

    Private Sub Lot_Allotment_Details_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim dt3 As New DataTable
        Dim dt4 As New DataTable
        Dim dt5 As New DataTable
        Dim dt6 As New DataTable
        Dim dt7 As New DataTable
        Dim dt8 As New DataTable

        Me.Text = ""

        con.Open()



        'da = New SqlClient.SqlDataAdapter("select Ledger_DisplayName from Ledger_AlaisHead where (Ledger_IdNo = 0 or Ledger_Type = 'REWINDING' ) order by Ledger_DisplayName", con)
        'da.Fill(dt1)
        'cbo_clothname.DataSource = dt1
        'cbo_clothname.DisplayMember = "Ledger_DisplayName"



        'da = New SqlClient.SqlDataAdapter("select Ledger_DisplayName from Ledger_AlaisHead where (Ledger_IdNo = 0 or  Ledger_Type = 'GODOWN' or  Ledger_Type = 'SIZING'  or  Ledger_Type = 'WEAVER' or  Ledger_Type = 'JOBWORKER') order by Ledger_DisplayName", con)
        'da.Fill(dt2)
        ''cbo_Rec_Ledger.DataSource = dt2
        ''cbo_Rec_Ledger.DisplayMember = "Ledger_DisplayName"


        'da = New SqlClient.SqlDataAdapter("select Ledger_DisplayName from Ledger_AlaisHead where (Ledger_IdNo = 0 or Ledger_Type = 'TRANSPORT') order by Ledger_DisplayName", con)
        'da.Fill(dt3)
        ''cbo_TransportName.DataSource = dt3
        ''cbo_TransportName.DisplayMember = "Ledger_DisplayName"


        'da = New SqlClient.SqlDataAdapter("select distinct(Vechile_No) from Lot_Allotment_Head order by Vechile_No", con)
        'da.Fill(dt7)
        ''cbo_Vechile.DataSource = dt7
        ''cbo_Vechile.DisplayMember = "Vechile_No"


        'da = New SqlClient.SqlDataAdapter("select mill_name from Mill_Head order by mill_name", con)
        'da.Fill(dt4)
        'cbo_Grid_checker.DataSource = dt4
        'cbo_Grid_checker.DisplayMember = "mill_name"

        'da = New SqlClient.SqlDataAdapter("select count_name from Count_Head order by count_name", con)
        'da.Fill(dt5)
        'cbo_Grid_TableNo.DataSource = dt5
        'cbo_Grid_TableNo.DisplayMember = "count_name"

        'da = New SqlClient.SqlDataAdapter("select Lot_Checker from YarnType_Head order by Lot_Checker", con)
        'da.Fill(dt6)
        'cbo_Grid_Supervisor.DataSource = dt6
        'cbo_Grid_Supervisor.DisplayMember = "Lot_Checker"

        'cbo_Type.Items.Add("")
        'cbo_Type.Items.Add("DIRECT")
        'cbo_Type.Items.Add("SELECTION")

        cbo_Grid_TableNo.Visible = False
        cbo_Grid_checker.Visible = False
        cbo_grid_folder.Visible = False
        cbo_Grid_Supervisor.Visible = False

        pnl_Selection.Visible = False
        pnl_Selection.Left = (Me.Width - pnl_Selection.Width) \ 2
        pnl_Selection.Top = (Me.Height - pnl_Selection.Height) \ 2
        pnl_Selection.BringToFront()

        'dtp_Date.Text = ""
        'msk_date.Text = ""

        pnl_Filter.Visible = False
        pnl_Filter.Left = (Me.Width - pnl_Filter.Width) \ 2
        pnl_Filter.Top = (Me.Height - pnl_Filter.Height) \ 2

        'btn_UserModification.Visible = False
        'If Common_Procedures.settings.User_Modifications_Show_Status = 1 Then
        '    If Val(Common_Procedures.User.IdNo) = 1 Or Common_Procedures.User.Show_UserModification_Status = 1 Then
        '        btn_UserModification.Visible = True
        '    End If
        'End If

        AddHandler msk_date.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Grid_TableNo.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Grid_checker.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_grid_folder.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Grid_Supervisor.GotFocus, AddressOf ControlGotFocus
        'AddHandler cbo_TransportName.GotFocus, AddressOf ControlGotFocus
        'AddHandler txt_Freight.GotFocus, AddressOf ControlGotFocus
        'AddHandler cbo_Vechile.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Lotcode_Selection.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_total_pcs.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_checker.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_folder.GotFocus, AddressOf ControlGotFocus
        AddHandler msk_AllottedDate.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Checking_Section.GotFocus, AddressOf ControlGotFocus

        AddHandler cbo_Filter_CountName.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Filter_MillName.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Filter_PartyName.GotFocus, AddressOf ControlGotFocus

        AddHandler msk_date.LostFocus, AddressOf ControlLostFocus
        AddHandler msk_AllottedDate.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Lotcode_Selection.LostFocus, AddressOf ControlLostFocus
        'AddHandler cbo_clothname.LostFocus, AddressOf ControlLostFocus
        'AddHandler cbo_Rec_Ledger.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Grid_TableNo.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Grid_checker.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_grid_folder.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Grid_Supervisor.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_checker.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_folder.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Checking_Section.LostFocus, AddressOf ControlLostFocus
        'AddHandler txt_total_pcs.LostFocus, AddressOf ControlLostFocus

        'AddHandler cbo_TransportName.LostFocus, AddressOf ControlLostFocus
        'AddHandler cbo_Vechile.LostFocus, AddressOf ControlLostFocus
        'AddHandler cbo_Type.LostFocus, AddressOf ControlLostFocus
        'AddHandler txt_Freight.LostFocus, AddressOf ControlLostFocus

        AddHandler cbo_Filter_CountName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Filter_MillName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Filter_PartyName.LostFocus, AddressOf ControlLostFocus

        'AddHandler msk_date.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_total_pcs.KeyDown, AddressOf TextBoxControlKeyDown
        'AddHandler dtp_Date.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_total_pcs.KeyPress, AddressOf TextBoxControlKeyPress

        AddHandler dtp_Filter_Fromdate.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler dtp_Filter_ToDate.KeyDown, AddressOf TextBoxControlKeyDown

        'AddHandler msk_date.KeyPress, AddressOf TextBoxControlKeyPress



        lbl_Company.Text = ""
        lbl_Company.Tag = 0
        lbl_Company.Visible = False
        Common_Procedures.CompIdNo = 0

        Filter_Status = False
        FrmLdSTS = True
        new_record()
    End Sub

    Private Sub Lot_Allotment_Details_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles MyBase.KeyPress

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
            MessageBox.Show(ex.Message, "DOES NOT CLOSE...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub Lot_Allotment_Details_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles MyBase.FormClosed
        con.Close()
        con.Dispose()
    End Sub

    Protected Overrides Function ProcessCmdKey(ByRef msg As System.Windows.Forms.Message, ByVal keyData As System.Windows.Forms.Keys) As Boolean
        Dim dgv1 As New DataGridView

        On Error Resume Next

        If ActiveControl.Name = dgv_details.Name Or TypeOf ActiveControl Is DataGridViewTextBoxEditingControl Then

            dgv1 = Nothing

            If ActiveControl.Name = dgv_details.Name Then
                dgv1 = dgv_details

            ElseIf dgv_details.IsCurrentRowDirty = True Then
                dgv1 = dgv_details
            ElseIf Pnl_Back.Enabled = True Then
                dgv1 = dgv_details
            End If

            If IsNothing(dgv1) = False Then

                With dgv1


                    If keyData = Keys.Enter Or keyData = Keys.Down Then
                        If .CurrentCell.ColumnIndex >= .ColumnCount - 4 Then
                            If .CurrentCell.RowIndex = .RowCount - 1 Then

                                If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                                    save_record()
                                Else
                                    cbo_Lotcode_Selection.Focus()
                                End If


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
                                'txt_Freight.Focus()

                            Else
                                .CurrentCell = .Rows(.CurrentCell.RowIndex - 1).Cells(.ColumnCount - 5)

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
            MessageBox.Show(ex.Message, "DOES NOT CLOSE...", MessageBoxButtons.OK, MessageBoxIcon.Error)

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
        Dim dt4 As New DataTable

        If Val(no) = 0 Then Exit Sub

        clear()

        'NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(no) & "/" & Trim(Common_Procedures.FnYearCode)

        NewCode = Trim(no) & "/" & Trim(Common_Procedures.FnYearCode) & "/" & Trim(Val(lbl_Company.Tag))

        'Try
        '    'da1 = New SqlClient.SqlDataAdapter("select a.*, b.Cloth_name from lot_Approved_Head a INNER JOIN Cloth_Head b ON a.Cloth_idno = b.Cloth_idno  where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.lot_no = '" & Trim(cbo_lotno.Text) & "' ", con)

        '    'da1.Fill(dt1)

        '    'If dt1.Rows.Count > 0 Then

        '    cbo_checker.Text = Common_Procedures.Employee_Simple_IdNoToName(con, Val(dt1.Rows(0).Item("Checker_Idno_IRwages").ToString))
        '    cbo_folder.Text = Common_Procedures.Employee_Simple_IdNoToName(con, Val(dt1.Rows(0).Item("Folder_Idno_IRwages").ToString))


        '    lbl_ClothName.Text = Common_Procedures.Company_IdNoToName(con, Val(dt1.Rows(0).Item("Cloth_IdNo").ToString))
        '    'cbo_Rec_Ledger.Text = Common_Procedures.Ledger_IdNoToName(con, Val(dt1.Rows(0).Item("ReceivedFrom_IdNo").ToString))
        '    txt_total_pcs.Text = dt1.Rows(0).Item("Party_DcNo").ToString


        '    'cbo_Vechile.Text = dt1.Rows(0).Item("Vechile_No").ToString
        '    'cbo_Type.Text = dt1.Rows(0).Item("Selection_Type").ToString

        '    If Val(dt1.Rows(0).Item("Freight").ToString) <> 0 Then
        '        'txt_Freight.Text = Val(dt1.Rows(0).Item("Freight").ToString)
        '    End If
        '    lbl_UserName.Text = "USER : " & Trim(UCase(Common_Procedures.User_IdNoToName(con1, Val(dt1.Rows(0).Item("User_IdNo").ToString))))

        '    dt2.Clear()

        '    da2 = New SqlClient.SqlDataAdapter("select a.* from  lot_allotment_details a where a.Lotcode_ForSelection='" & Trim(cbo_Lotcode_Selection.Text) & "' ", con)
        '    dt2 = New DataTable
        '    da2.Fill(dt2)

        '    dgv_details.Rows.Clear()
        '    SNo = 0
        '    With dgv_details

        '        .Rows.Clear()
        '        SNo = 0

        '        If dt4.Rows.Count > 0 Then

        '            cbo_checker.Text = Common_Procedures.Employee_Simple_IdNoToName(con, Val(dt4.Rows(0).Item("Checker_Idno_IRwages").ToString))
        '            cbo_folder.Text = Common_Procedures.Employee_Simple_IdNoToName(con, Val(dt4.Rows(0).Item("Folder_Idno_IRwages").ToString))

        '            For i = 0 To dt4.Rows.Count - 1

        '                n = .Rows.Add()

        '                SNo = SNo + 1

        '                .Rows(n).Cells(0).Value = Val(SNo)
        '                .Rows(n).Cells(1).Value = Common_Procedures.Checking_TableNo_IdNoToName(con, dt4.Rows(i).Item("Checking_Table_IdNo").ToString)
        '                .Rows(n).Cells(2).Value = dt4.Rows(i).Item("No_of_pcs").ToString

        '                .Rows(n).Cells(3).Value = Common_Procedures.Employee_Simple_IdNoToName(con, dt4.Rows(i).Item("Lot_Supervisor_idno").ToString)
        '                'dt4.Rows(i).Item("Lot_Supervisor_idno").ToString
        '                .Rows(n).Cells(4).Value = Common_Procedures.Employee_Simple_IdNoToName(con, dt4.Rows(i).Item("Lot_Checker_idno").ToString)
        '                'dt4.Rows(i).Item("Lot_Checker_idno").ToString
        '                .Rows(n).Cells(8).Value = Common_Procedures.Employee_Simple_IdNoToName(con, dt4.Rows(i).Item("Lot_Folder_Idno").ToString)


        '            Next i

        '            Total_Calculation()

        '        End If

        '    End With



        '    dt2.Clear()

        '    dt2.Dispose()
        '    da2.Dispose()

        '    'End If

        '    dt1.Clear()
        '    dt1.Dispose()
        '    da1.Dispose()

        '    Grid_Cell_DeSelect()

        '    If LockSTS = True Then

        '        lbl_ClothName.Enabled = False
        '        lbl_ClothName.BackColor = Color.LightGray

        '        'cbo_Rec_Ledger.Enabled = False
        '        'cbo_Rec_Ledger.BackColor = Color.LightGray

        '        'cbo_TransportName.Enabled = False
        '        'cbo_TransportName.BackColor = Color.LightGray

        '        'cbo_Vechile.Enabled = False
        '        'cbo_Vechile.BackColor = Color.LightGray

        '        txt_total_pcs.Enabled = False
        '        txt_total_pcs.BackColor = Color.LightGray

        '        'txt_Freight.Enabled = False
        '        'txt_Freight.BackColor = Color.LightGray

        '        cbo_Grid_TableNo.Enabled = False
        '        cbo_Grid_TableNo.BackColor = Color.LightGray

        '        cbo_Grid_checker.Enabled = False
        '        cbo_Grid_checker.BackColor = Color.LightGray



        '        cbo_grid_folder.Enabled = False
        '        cbo_grid_folder.BackColor = Color.LightGray


        '    End If
        'Catch ex As Exception
        '    MessageBox.Show(ex.Message, "DOES NOT MOVE RECORDS...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        'End Try

        'If cbo_Lotcode_Selection.Visible And cbo_Lotcode_Selection.Enabled Then cbo_Lotcode_Selection.Focus()


        Try


            da1 = New SqlClient.SqlDataAdapter("Select a.* , b.Ledger_Name as WeaverName , c.Checking_Section_Name  from LotAllotment_Head a inner join Ledger_Head b ON a.Ledger_IdNo = b.Ledger_IdNo INNER JOIN Checking_Section_Head c ON a.Checking_Section_IdNo = c.Checking_Section_IdNo Where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.LotAllot_RefNo = '" & Trim(no) & "'  and a.Lotcode_forselection like '%/" & Trim(Common_Procedures.FnYearCode) & "/%" & "' ", con)
            'da1 = New SqlClient.SqlDataAdapter("Select a.* , b.Ledger_Name as WeaverName from LotAllotment_Head a inner join Ledger_Head b ON a.Ledger_IdNo = b.Ledger_IdNo Where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Lotcode_ForSelection = '" & Trim(NewCode) & "' ", con)
            dt1 = New DataTable
            da1.Fill(dt1)

            If dt1.Rows.Count > 0 Then

                lbl_RefNo.Text = dt1.Rows(0).Item("LotAllot_RefNo").ToString

                lbl_ClothName.Text = Common_Procedures.Cloth_IdNoToName(con, Val(dt1.Rows(0).Item("Cloth_IdNo").ToString))
                lbl_WeaverName.Text = dt1.Rows(0).Item("WeaverName").ToString

                dtp_Date.Text = dt1.Rows(0).Item("LotAllot_Date")
                msk_date.Text = dtp_Date.Text

                dtp_AllottedDate.Text = dt1.Rows(0).Item("Alloted_Date")
                msk_AllottedDate.Text = dtp_AllottedDate.Text

                cbo_Lotcode_Selection.Text = dt1.Rows(0).Item("Lotcode_forselection").ToString

                txt_total_pcs.Text = Val(dt1.Rows(0).Item("Total_Pcs").ToString)

                txt_receipt_mtrs.Text = Format(Val(dt1.Rows(0).Item("Receipt_Meters").ToString), "########0.00")

                cbo_checker.Text = Common_Procedures.Employee_Simple_IdNoToName(con, dt1.Rows(0).Item("Checker_IdNo").ToString)
                cbo_folder.Text = Common_Procedures.Employee_Simple_IdNoToName(con, dt1.Rows(0).Item("Folder_IdNo").ToString)

                cbo_Checking_Section.Text = dt1.Rows(0).Item("Checking_Section_Name").ToString

            End If



            da2 = New SqlClient.SqlDataAdapter("select a.* from  lot_allotment_details a where a.Lotcode_ForSelection = '" & Trim(cbo_Lotcode_Selection.Text) & "' ", con)
            dt2 = New DataTable
            da2.Fill(dt2)

            dgv_details.Rows.Clear()
            SNo = 0
            With dgv_details

                .Rows.Clear()
                SNo = 0

                If dt2.Rows.Count > 0 Then

                    cbo_checker.Text = Common_Procedures.Employee_Simple_IdNoToName(con, Val(dt2.Rows(0).Item("Checker_Idno_IRwages").ToString))
                    cbo_folder.Text = Common_Procedures.Employee_Simple_IdNoToName(con, Val(dt2.Rows(0).Item("Folder_Idno_IRwages").ToString))
                    For i = 0 To dt2.Rows.Count - 1

                        n = .Rows.Add()

                        SNo = SNo + 1

                        .Rows(n).Cells(0).Value = Val(SNo)
                        .Rows(n).Cells(1).Value = Common_Procedures.Checking_TableNo_IdNoToName(con, dt2.Rows(i).Item("Checking_Table_IdNo").ToString)
                        .Rows(n).Cells(2).Value = dt2.Rows(i).Item("No_of_pcs").ToString

                        .Rows(n).Cells(3).Value = Common_Procedures.Employee_Simple_IdNoToName(con, dt2.Rows(i).Item("Lot_Supervisor_idno").ToString)
                        'dt4.Rows(i).Item("Lot_Supervisor_idno").ToString
                        .Rows(n).Cells(4).Value = Common_Procedures.Employee_Simple_IdNoToName(con, dt2.Rows(i).Item("Lot_Checker_idno").ToString)
                        'dt4.Rows(i).Item("Lot_Checker_idno").ToString
                        .Rows(n).Cells(8).Value = Common_Procedures.Employee_Simple_IdNoToName(con, dt2.Rows(i).Item("Lot_Folder_Idno").ToString)

                    Next i

                    Total_Calculation()

                End If

            End With

            dt2.Clear()

            dt2.Dispose()
            da2.Dispose()

            dt1.Clear()
            dt1.Dispose()
            da1.Dispose()

            Grid_Cell_DeSelect()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT MOVE RECORDS...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try


    End Sub

    Public Sub delete_record() Implements Interface_MDIActions.delete_record
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim cmd As New SqlClient.SqlCommand
        Dim trans As SqlClient.SqlTransaction
        Dim NewCode As String = ""
        Dim vOrdByNo As String = ""

        'vOrdByNo = Common_Procedures.OrderBy_CodeToValue(lbl_DcNo.Text)

        'NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        'If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.Lot_Allotment_Details_Entry, "~L~") = 0 And InStr(Common_Procedures.UR.Lot_Allotment_Details_Entry, "~D~") = 0 Then MessageBox.Show("You have No Rights to Delete", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error) : Exit Sub
        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.DeletingEntry, Common_Procedures.UR.Lot_Allotment_Details, New_Entry, Me) = False Then Exit Sub

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

        'If New_Entry = True Then
        '    MessageBox.Show("This is New Entry", "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
        '    Exit Sub
        'End If


        'NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)





        trans = con.BeginTransaction

        Try

            'NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            cmd.Connection = con
            cmd.Transaction = trans

            cmd.CommandText = "delete from LotAllotment_Head where Lotcode_ForSelection='" & Trim(cbo_Lotcode_Selection.Text) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "delete from Lot_Allotment_Details where Lotcode_ForSelection='" & Trim(cbo_Lotcode_Selection.Text) & "'"
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

            'If msk_date.Enabled = True And msk_date.Visible = True Then msk_date.Focus()

        End Try

    End Sub

    Public Sub filter_record() Implements Interface_MDIActions.filter_record

        'If Filter_Status = False Then

        '    Dim da As New SqlClient.SqlDataAdapter
        '    Dim dt1 As New DataTable
        '    Dim dt2 As New DataTable
        '    Dim dt3 As New DataTable

        '    da = New SqlClient.SqlDataAdapter("select Ledger_DisplayName from Ledger_AlaisHead where (Ledger_IdNo = 0 or Ledger_Type = 'SIZING') order by Ledger_DisplayName", con)
        '    da.Fill(dt1)
        '    cbo_Filter_PartyName.DataSource = dt1
        '    cbo_Filter_PartyName.DisplayMember = "Ledger_DisplayName"

        '    da = New SqlClient.SqlDataAdapter("select count_name from count_head order by count_name", con)
        '    da.Fill(dt2)
        '    cbo_Filter_CountName.DataSource = dt2
        '    cbo_Filter_CountName.DisplayMember = "count_name"

        '    da = New SqlClient.SqlDataAdapter("select Mill_name from Mill_head order by Mill_name", con)
        '    da.Fill(dt2)
        '    cbo_Filter_MillName.DataSource = dt2
        '    cbo_Filter_MillName.DisplayMember = "Mill_name"


        '    dtp_Filter_Fromdate.Text = ""
        '    dtp_Filter_ToDate.Text = ""
        '    cbo_Filter_PartyName.Text = ""
        '    cbo_Filter_CountName.Text = ""
        '    cbo_Filter_MillName.Text = ""

        '    cbo_Filter_PartyName.SelectedIndex = -1
        '    cbo_Filter_CountName.SelectedIndex = -1
        '    cbo_Filter_MillName.SelectedIndex = -1
        '    dgv_Filter_Details.Rows.Clear()

        'End If

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

            da = New SqlClient.SqlDataAdapter("select top 1 LotAllot_RefNo from LotAllotment_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Lotcode_forselection like '%/" & Trim(Common_Procedures.FnYearCode) & "/%" & "' Order by for_Orderby, LotAllot_RefNo", con)
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

            da = New SqlClient.SqlDataAdapter("select top 1 LotAllot_RefNo from LotAllotment_Head where for_orderby > " & Str(Val(OrdByNo)) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and Lotcode_forselection like '%/" & Trim(Common_Procedures.FnYearCode) & "/%" & "' Order by for_Orderby, LotAllot_RefNo ", con)
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

            da = New SqlClient.SqlDataAdapter("select top 1 LotAllot_RefNo from LotAllotment_Head where for_orderby < " & Str(Val(OrdByNo)) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and Lotcode_forselection like '%/" & Trim(Common_Procedures.FnYearCode) & "/%" & "' Order by for_Orderby desc, LotAllot_RefNo desc", con)
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
            MessageBox.Show(ex.Message, "FOR  MOVING....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Public Sub movelast_record() Implements Interface_MDIActions.movelast_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim movno As String = ""

        Try

            da = New SqlClient.SqlDataAdapter("select top 1 LotAllot_RefNo from LotAllotment_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Lotcode_forselection like '%/" & Trim(Common_Procedures.FnYearCode) & "/%" & "' Order by for_Orderby desc, LotAllot_RefNo desc", con)
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

        'Try
        'clear()

        '    New_Entry = True

        '    'lbl_DcNo.Text = Common_Procedures.get_MaxCode(con, "Lot_Allotment_Head", "Lot_Allotment_Details_Code", "For_OrderBy", "", Val(lbl_Company.Tag), Common_Procedures.FnYearCode)

        '    'lbl_DcNo.ForeColor = Color.Red
        '    'msk_date.Text = Date.Today.ToShortDateString

        '    da = New SqlClient.SqlDataAdapter("select top 1 * from Lot_Allotment_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Lot_Allotment_Details_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, Lot_Allotment_Details_No desc", con)
        '    dt1 = New DataTable
        '    da.Fill(dt1)
        '    If dt1.Rows.Count > 0 Then
        '        If Val(Common_Procedures.settings.PreviousEntryDate_ByDefault) = 1 Then '---- M.S Textiles (Tirupur)
        '            'If dt1.Rows(0).Item("Lot_Allotment_Details_Date").ToString <> "" Then msk_date.Text = dt1.Rows(0).Item("Lot_Allotment_Details_Date").ToString
        '        End If
        '    End If
        '    dt1.Clear()


        'If cbo_Lotcode_Selection.Enabled And cbo_Lotcode_Selection.Visible Then cbo_Lotcode_Selection.Focus()

        'Catch ex As Exception
        '    MessageBox.Show(ex.Message, "FOR NEW RECORD...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        'End Try

        'dt1.Dispose()
        'da.Dispose()

        '-------------------

        Try
            clear()

            New_Entry = True


            EntFnYrCode = Trim(Common_Procedures.FnYearCode) & "%"

            lbl_RefNo.Text = Common_Procedures.get_MaxCode(con, "LotAllotment_Head", "Lotcode_forselection", "For_OrderBy", "", Val(lbl_Company.Tag), EntFnYrCode)

            lbl_RefNo.ForeColor = Color.Red

            'da = New SqlClient.SqlDataAdapter("select top 1 * from LotAllotment_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Lotcode_forselection like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, LotAllot_RefNo desc", con)
            'dt1 = New DataTable
            'da.Fill(dt1)
            'If dt1.Rows.Count > 0 Then

            'End If


            If msk_date.Enabled And msk_date.Visible Then msk_date.Focus()

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

        'Try

        '    inpno = InputBox("Enter Dc.No.", "FOR FINDING...")

        '    RecCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

        '    Da = New SqlClient.SqlDataAdapter("select Lot_Allotment_Details_No from Lot_Allotment_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Lot_Allotment_Details_Code = '" & Trim(RecCode) & "'", con)
        '    Da.Fill(Dt)

        '    movno = ""
        '    If Dt.Rows.Count > 0 Then
        '        If IsDBNull(Dt.Rows(0)(0).ToString) = False Then
        '            movno = Trim(Dt.Rows(0)(0).ToString)
        '        End If
        '    End If

        '    Dt.Clear()
        '    Dt.Dispose()
        '    Da.Dispose()

        '    If Val(movno) <> 0 Then
        '        move_record(movno)

        '    Else
        '        MessageBox.Show("Dc No. does not exists", "DOES NOT FIND...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        '    End If

        'Catch ex As Exception
        '    MessageBox.Show(ex.Message, "DOES NOT FIND...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        'End Try

    End Sub

    Public Sub insert_record() Implements Interface_MDIActions.insert_record
        'Dim Da As New SqlClient.SqlDataAdapter
        'Dim Dt As New DataTable
        'Dim movno As String, inpno As String
        'Dim RecCode As String

        ''  If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.Lot_Allotment_Details_Entry, "~L~") = 0 And InStr(Common_Procedures.UR.Lot_Allotment_Details_Entry, "~I~") = 0 Then MessageBox.Show("You have No Rights to Insert", "DOES NOT INSERT...", MessageBoxButtons.OK, MessageBoxIcon.Error) : Exit Sub
        'If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.InsertingEntry, Common_Procedures.UR.Lot_Allotment_Details, New_Entry, Me) = False Then Exit Sub

        'Try

        '    inpno = InputBox("Enter New Dc No.", "FOR NEW RECEIPT INSERTION...")

        '    RecCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

        '    Da = New SqlClient.SqlDataAdapter("select Lot_Allotment_Details_No from Lot_Allotment_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Lot_Allotment_Details_Code = '" & Trim(RecCode) & "'", con)
        '    Da.Fill(Dt)

        '    movno = ""
        '    If Dt.Rows.Count > 0 Then
        '        If IsDBNull(Dt.Rows(0)(0).ToString) = False Then
        '            movno = Trim(Dt.Rows(0)(0).ToString)
        '        End If
        '    End If

        '    Dt.Clear()
        '    Dt.Dispose()
        '    Da.Dispose()

        '    If Val(movno) <> 0 Then
        '        move_record(movno)

        '    Else
        '        If Val(inpno) = 0 Then
        '            MessageBox.Show("Invalid Dc No", "DOES NOT INSERT NEW RECEIPT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        '        Else
        '            new_record()
        '            Insert_Entry = True
        '            'lbl_DcNo.Text = Trim(UCase(inpno))

        '        End If

        '    End If

        'Catch ex As Exception
        '    MessageBox.Show(ex.Message, "DOES NOT INSERT NEW RECEIPT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        'End Try

    End Sub

    Public Sub save_record() Implements Interface_MDIActions.save_record
        Dim cmd As New SqlClient.SqlCommand
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim tr As SqlClient.SqlTransaction
        Dim NewCode As String = ""
        Dim Delv_ID As Integer = 0
        Dim Sur As String
        Dim Rec_ID As Integer = 0
        Dim Trans_ID As Integer = 0
        Dim Clo_ID As Integer = 0
        Dim EdsCnt_ID As Integer = 0
        Dim Sno As Integer = 0
        Dim Partcls As String = ""
        Dim PBlNo As String = ""
        Dim vCHKTAB_IDNO As Integer = 0
        Dim vCHKR_IDNO As Integer = 0
        Dim vFOLDR_IDNO As Integer = 0
        Dim vSUPVSR_IDNO As Integer = 0
        Dim Receipt_pKCOndtion As String = "" ', vTotYrnCones As Single, vTotYrnWeight As Single
        Dim EntID As String = ""
        Dim Nr As Integer = 0
        Dim Usr_ID As Integer = 0
        Dim ByCn_RefCd As String = ""
        Dim vOrdByNo As String = ""
        Dim vFolderwg_id As Integer = 0
        Dim vCloth_ID As Integer
        Dim vCLORECCODE As String = ""
        Dim vCheckwg_id As Integer = 0
        Dim vChkSec_IdNo As Integer = 0
        Dim EntFnYrCode As String
        'vOrdByNo = Common_Procedures.OrderBy_CodeToValue(lbl_DcNo.Text)

        'NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Dim vLed_id As Integer = 0

        If Val(lbl_Company.Tag) = 0 Then
            MessageBox.Show("Invalid Company Selection", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        'If Common_Procedures.UserRight_Check(Common_Procedures.UR.Lot_Allotment_Details_Entry, New_Entry) = False Then Exit Sub

        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.SavingEntry, Common_Procedures.UR.Lot_Allotment_Details, New_Entry, Me, con, "Lot_Approved_Head", "Lot_Approved_Code", NewCode, "Lot_Approved_Date", "(Lot_Approved_Code = '" & Trim(NewCode) & "')", "(Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Lot_Approved_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "')", "for_Orderby desc, Lot_Approved_no desc") = False Then Exit Sub


        If Pnl_Back.Enabled = False Then
            MessageBox.Show("Close Other Windows", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        If IsDate(msk_date.Text) = False Then
            MessageBox.Show("Invalid Date", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If msk_date.Enabled And msk_date.Visible Then msk_date.Focus()
            Exit Sub
        End If


        If IsDate(msk_AllottedDate.Text) = False Then
            MessageBox.Show("Invalid Date", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If msk_AllottedDate.Enabled And msk_AllottedDate.Visible Then msk_AllottedDate.Focus()
            Exit Sub
        End If

        'If Not (Convert.ToDateTime(msk_date.Text) >= Common_Procedures.Company_FromDate And Convert.ToDateTime(msk_date.Text) <= Common_Procedures.Company_ToDate) Then
        '    MessageBox.Show("Date is out of financial range", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
        '    If msk_date.Enabled And msk_date.Visible Then msk_date.Focus()
        '    Exit Sub
        'End If

        vCloth_ID = Common_Procedures.Cloth_NameToIdNo(con, lbl_ClothName.Text)

        vLed_id = Common_Procedures.Ledger_AlaisNameToIdNo(con, lbl_WeaverName.Text)

        vChkSec_IdNo = Common_Procedures.CheckingSection_NameToIdNo(con, cbo_Checking_Section.Text)

        'If vCloth_ID = 0 Then
        '    MessageBox.Show("Invalid Cloth Name", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
        '    If cbo_clothname.Enabled And cbo_clothname.Visible Then cbo_clothname.Focus()
        '    Exit Sub
        'End If
        'Rec_ID = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Rec_Ledger.Text)

        'Trans_ID = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_TransportName.Text)

        Dim vChekr_id As Integer = 0
        Dim vFolder_id As Integer = 0

        vChekr_id = Common_Procedures.Employee_Simple_NameToIdNo(con, cbo_checker.Text)
        vFolder_id = Common_Procedures.Employee_Simple_NameToIdNo(con, cbo_folder.Text)

        If Val(txt_total_pcs.Text) <> Val(dgv_YarnDetails_Total.Rows(0).Cells(2).Value) Then

            MessageBox.Show("Mismatch Total Pcs...", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

            With dgv_details

                .Focus()
                dgv_details.CurrentCell = dgv_details.Rows(0).Cells(2)

            End With

            Exit Sub

        End If

        tr = con.BeginTransaction

        Try


            cmd.Connection = con
            cmd.Transaction = tr

            New_Entry = True
            Da = New SqlClient.SqlDataAdapter("select count(*) from LotAllotment_Head where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Lotcode_ForSelection = '" & Trim(cbo_Lotcode_Selection.Text) & "'", con)
            Da.SelectCommand.Transaction = tr
            Dt1 = New DataTable
            Da.Fill(Dt1)
            If Dt1.Rows.Count > 0 Then
                If IsDBNull(Dt1.Rows(0)(0).ToString) = False Then
                    If Val(Dt1.Rows(0)(0).ToString) > 0 Then
                        New_Entry = False
                    End If
                End If
            End If
            Dt1.Clear()

            If New_Entry = True Then
                EntFnYrCode = Trim(Common_Procedures.FnYearCode) & "%"
                lbl_RefNo.Text = Common_Procedures.get_MaxCode(con, "LotAllotment_Head", "Lotcode_forselection", "For_OrderBy", "", Val(lbl_Company.Tag), EntFnYrCode, tr)
            End If


            cmd.Parameters.Clear()
            cmd.Parameters.AddWithValue("@RefDate", dtp_Date.Value)
            cmd.Parameters.AddWithValue("@AllotedDate", dtp_AllottedDate.Value)

            If New_Entry = True Then

                'cmd.CommandText = "Insert into Lot_Allotment_Head(Lot_Allotment_Details_Code, Company_IdNo, Lot_Allotment_Details_No, for_OrderBy, Lot_Allotment_Details_Date, DeliveryTo_IdNo,Selection_Type, ReceivedFrom_IdNo, Total_Bags, Total_Cones, Total_Weight ,  Party_DcNo , Transport_IdNo ,Vechile_No ,Freight,  user_Idno ) Values ('" & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ",  , @DcDate, " & Str(Val(Delv_ID)) & ",  " & Str(Val(Rec_ID)) & "  , " & Str(Val(vTotYrnBags)) & ", " & Str(Val(vTotYrnCones)) & ", " & Str(Val(vTotYrnWeight)) & "  , '" & Trim(txt_total_pcs.Text) & "' , " & Str(Val(Trans_ID)) & ", " & Val(Common_Procedures.User.IdNo) & " )"
                'cmd.ExecuteNonQuery()

                cmd.CommandText = "Insert into LotAllotment_Head(   Lotcode_ForSelection,                             Company_IdNo,                    LotAllot_RefNo,                                     for_OrderBy,                                           LotAllot_Date,          Alloted_Date,                   Ledger_IdNo,                Cloth_IdNo,                 Checker_IdNo,                      Folder_IdNo ,                           Total_Pcs ,                           Receipt_Meters ,                             user_Idno                     , Checking_Section_IdNo) " &
                                                    "Values  ('" & Trim(cbo_Lotcode_Selection.Text) & "', " & Str(Val(lbl_Company.Tag)) & ",  '" & Trim(lbl_RefNo.Text) & "',      " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text))) & " ,       @RefDate        ,       @AllotedDate   ,     " & Str(Val(vLed_id)) & " ,     " & Val(vCloth_ID) & "  ,    " & Str(Val(vChekr_id)) & "  ,      " & Str(Val(vFolder_id)) & "  ,       " & Val(txt_total_pcs.Text) & " ,      " & Val(txt_receipt_mtrs.Text) & "   , " & Val(Common_Procedures.User.IdNo) & " , " & Str(Val(vChkSec_IdNo)) & ")"
                cmd.ExecuteNonQuery()


            Else


                'Call Common_Procedures.User_Modification_Updation(con, "HEAD", Me.Name, "OLD", " Lot_Allotment_Head", " Lot_Allotment_Details_Code", Val(lbl_Company.Tag), NewCode, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, False, "", "", " Lot_Allotment_Details_Code, Company_IdNo, for_OrderBy")
                'Call Common_Procedures.User_Modification_Updation(con, "DETAILS", Me.Name, "OLD", " Lot_Allotment_Details", " Lot_Allotment_Details_Code", Val(lbl_Company.Tag), NewCode, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, False, "Lot_Checker, Table_No,count_idno, Bags, Cones, Weight , Lot_Supervisor,Reference_Code", "Sl_No", " Lot_Allotment_Details_Code, For_OrderBy, Company_IdNo,  Lot_Allotment_Details_No,  Lot_Allotment_Details_Date, Ledger_Idno")

                'cmd.CommandText = "Update Lot_Allotment_Head set Lot_Allotment_Details_Date = @DcDate, DeliveryTo_IdNo = " & Str(Val(Delv_ID)) & ", ReceivedFrom_IdNo = " & Str(Val(Rec_ID)) & "  ,  Total_Bags = " & Str(Val(vTotYrnBags)) & ", Total_Cones = " & Str(Val(vTotYrnCones)) & ", Total_Weight = " & Str(Val(vTotYrnWeight)) & "  , Party_DcNo =  '" & Trim(txt_total_pcs.Text) & "'  ,    Transport_IdNo = " & Str(Val(Trans_ID)) & ", User_IdNo = " & Val(Common_Procedures.User.IdNo) & " Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Lot_Allotment_Details_Code = '" & Trim(NewCode) & "' "
                'cmd.ExecuteNonQuery()

                'cmd.CommandText = "Update Stock_BabyCone_Processing_Details set Delivered_Bags = a.Delivered_Bags - b.Bags, Delivered_Cones = a.Delivered_Cones - b.Cones, Delivered_Weight = a.Delivered_Weight - b.Weight from Stock_BabyCone_Processing_Details a, Lot_Allotment_Details b Where b.Lot_Allotment_Details_Code = '" & Trim(NewCode) & "' and a.Reference_Code = b.Reference_Code"
                'cmd.ExecuteNonQuery()


                cmd.CommandText = "Update LotAllotment_Head set  LotAllot_Date = @RefDate,  Alloted_Date =  @AllotedDate  ,  Ledger_IdNo =  " & Str(Val(vLed_id)) & "  ,  Cloth_IdNo = " & Val(vCloth_ID) & "  ,  Checker_IdNo = " & Str(Val(vChekr_id)) & "  ,  Folder_IdNo = " & Str(Val(vFolder_id)) & " ,  Total_Pcs = " & Val(txt_total_pcs.Text) & " ,  Receipt_Meters =   " & Val(txt_receipt_mtrs.Text) & " ,   User_IdNo = " & Val(Common_Procedures.User.IdNo) & " , Checking_Section_IdNo = " & Str(Val(vChkSec_IdNo)) & " Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Lotcode_ForSelection = '" & Trim(cbo_Lotcode_Selection.Text) & "'  "
                cmd.ExecuteNonQuery()


            End If

            'Call Common_Procedures.User_Modification_Updation(con, "HEAD", Me.Name, "NEW", " Lot_Allotment_Head", " Lot_Allotment_Details_Code", Val(lbl_Company.Tag), NewCode, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, False, "", "", " Lot_Allotment_Details_Code, Company_IdNo, for_OrderBy")




            cmd.CommandText = "Delete from Lot_Allotment_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Lotcode_ForSelection = '" & Trim(cbo_Lotcode_Selection.Text) & "'"
            cmd.ExecuteNonQuery()


            Receipt_pKCOndtion = "WCLRC-"

            vCLORECCODE = Trim(Receipt_pKCOndtion) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(txt_lot_no.Text) & "/" & Trim(Common_Procedures.FnYearCode)


            With dgv_details
                Sno = 0

                For i = 0 To .RowCount - 1

                    If Val(.Rows(i).Cells(2).Value) <> 0 Then

                        Sno = Sno + 1

                        vCHKTAB_IDNO = Common_Procedures.Checking_TableNo_NameToIdNo(con, .Rows(i).Cells(1).Value, tr)

                        vSUPVSR_IDNO = Common_Procedures.Employee_Simple_NameToIdNo(con, .Rows(i).Cells(3).Value, tr)
                        vCHKR_IDNO = Common_Procedures.Employee_Simple_NameToIdNo(con, .Rows(i).Cells(4).Value, tr)
                        vFOLDR_IDNO = Common_Procedures.Employee_Simple_NameToIdNo(con, .Rows(i).Cells(8).Value, tr)


                        vCheckwg_id = Common_Procedures.Employee_Simple_NameToIdNo(con, cbo_checker.Text, tr)
                        vFolderwg_id = Common_Procedures.Employee_Simple_NameToIdNo(con, cbo_folder.Text, tr)


                        'Nr = 0
                        'cmd.CommandText = "Update Lot_Allotment_Details set   Checking_Table_IdNo=" & Val(vCHKTAB_IDNO) & ", No_Of_Pcs=" & Str(Val(.Rows(i).Cells(2).Value)) & ",Lot_Supervisor_idno=" & Str(Val(vCHKR_IDNO)) & ", Lot_Checker_idno=" & Str(Val(vSUPVSR_IDNO)) & ",user_idno =" & Str(Val(Common_Procedures.User.IdNo)) & ",Year_Code='" & Trim(Common_Procedures.FnYearCode) & "',Receipt_PkCondition='" & Trim(Receipt_pKCOndtion) & "',Lot_No='" & Trim(txt_lot_no.Text) & "',Lot_code='" & Trim(txt_lot_code.Text) & "',Weaver_ClothReceipt_Code='" & Trim(vCLORECCODE) & "'where Lotcode_ForSelection = '" & Trim(cbo_lotno.Text) & "' and sl_no=" & Str(Val(Sno)) & "and company_idno =" & Str(Val(lbl_Company.Tag)) & ""
                        'Nr = cmd.ExecuteNonQuery()

                        'If Nr = 0 Then
                        cmd.CommandText = "Insert into Lot_Allotment_Details (Company_idno,sl_no, Checking_Table_IdNo,No_Of_Pcs,Lot_Checker_idno, Lot_Supervisor_idno ,Lotcode_ForSelection,User_idno,Year_Code,Receipt_PkCondition,Lot_No,Lot_Code,Weaver_ClothReceipt_Code,Checker_Idno_IRwages,Folder_Idno_IRwages,Lot_Folder_Idno) Values (" & Val(lbl_Company.Tag) & "," & Str(Val(Sno)) & "," & Val(vCHKTAB_IDNO) & ", " & Str(Val(.Rows(i).Cells(2).Value)) & ", " & Str(Val(vCHKR_IDNO)) & ", " & Str(Val(vSUPVSR_IDNO)) & " , '" & Trim(cbo_Lotcode_Selection.Text) & "' ," & Val(Common_Procedures.User.IdNo) & ",'" & Trim(Common_Procedures.FnYearCode) & "','" & Trim(Receipt_pKCOndtion) & "','" & Trim(txt_lot_no.Text) & "','" & Trim(txt_lot_code.Text) & "','" & Trim(vCLORECCODE) & "', " & Str(Val(vCheckwg_id)) & ", " & Str(Val(vFolderwg_id)) & "," & Str(Val(vFOLDR_IDNO)) & ")"
                        cmd.ExecuteNonQuery()
                        'End If



                    End If

                Next

                'Call Common_Procedures.User_Modification_Updation(con, "DETAILS", Me.Name, "NEW", " Lot_Allotment_Details", " Lotcode_ForSelection", Val(lbl_Company.Tag), NewCode, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, False, "Lot_Checker_idno, Table_idNo,Company_idno,User_idno,Total_pcs,Lot_Supervisor_idno", "Sl_No", " ")
                'Call Common_Procedures.User_Modification_Updation(con, "DETAILS", Me.Name, "NEW", " Lot_Allotment_Details", " Lot_Allotment_Details_Code", Val(lbl_Company.Tag), NewCode, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, False, "Lot_Checker, Table_No,count_idno, Bags, Cones, Weight , Lot_Supervisor,Reference_Code", "Sl_No", " Lot_Allotment_Details_Code, For_OrderBy, Company_IdNo,  Lot_Allotment_Details_No,  Lot_Allotment_Details_Date, Ledger_Idno")

            End With


            tr.Commit()

            Dt1.Dispose()
            Da.Dispose()

            MessageBox.Show("Saved Sucessfully!!!", "FOR SAVING...", MessageBoxButtons.OK, MessageBoxIcon.Information)

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
            'If msk_date.Enabled And msk_date.Visible Then msk_date.Focus()

        End Try

    End Sub


    Private Sub txt_total_pcs_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs)
        If e.KeyValue = 38 Then SendKeys.Send("+{TAB}")
        If (e.KeyValue = 40) Then
            'If dgv_details.Rows.Count > 0 Then
            '    dgv_details.Focus()
            '    dgv_details.CurrentCell = dgv_details.Rows(0).Cells(1)

            'Else
            '    btn_save.Focus()

            'End If
        End If
    End Sub

    Private Sub txt_total_pcs_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
        If Asc(e.KeyChar) = 13 Then
            'If dgv_details.Rows.Count > 0 Then
            '    dgv_details.Focus()
            '    dgv_details.CurrentCell = dgv_details.Rows(0).Cells(1)

            'Else
            '    btn_save.Focus()

            'End If
        End If
    End Sub

    Private Sub dgv_YarnDetails_CellEndEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_details.CellEndEdit
        Total_Calculation()
        If dgv_details.CurrentRow.Cells(2).Value = "MILL" Then
            If dgv_details.CurrentCell.ColumnIndex = 4 Or dgv_details.CurrentCell.ColumnIndex = 5 Then
                get_MillCount_Details()
            End If
        End If
    End Sub

    Private Sub dgv_YarnDetails_CellEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_details.CellEnter
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim Dt2 As New DataTable
        Dim Dt3 As New DataTable
        Dim rect As Rectangle

        With dgv_details

            'If Val(.Rows(e.RowIndex).Cells(8).Value) = 0 Then
            '    Set_Max_DetailsSlNo(e.RowIndex, 8)
            '    'If e.RowIndex = 0 Then
            '    '    .Rows(e.RowIndex).Cells(15).Value = 1
            '    'Else
            '    '    .Rows(e.RowIndex).Cells(15).Value = Val(.Rows(e.RowIndex - 1).Cells(15).Value) + 1
            '    'End If
            'End If

            If Val(.CurrentRow.Cells(0).Value) = 0 Then
                .CurrentRow.Cells(0).Value = .CurrentRow.Index + 1
            End If



            If e.ColumnIndex = 1 Then

                If cbo_Grid_TableNo.Visible = False Or Val(cbo_Grid_TableNo.Tag) <> e.RowIndex Then

                    cbo_Grid_TableNo.Tag = -1
                    Da = New SqlClient.SqlDataAdapter("select Checking_Table_No from Checking_TableNo_Head order by Checking_Table_No", con)
                    Dt1 = New DataTable
                    Da.Fill(Dt1)
                    cbo_Grid_TableNo.DataSource = Dt1
                    cbo_Grid_TableNo.DisplayMember = "Checking_Table_No"

                    rect = .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False)

                    cbo_Grid_TableNo.Left = .Left + rect.Left  '  .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Left
                    cbo_Grid_TableNo.Top = .Top + rect.Top  ' .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Top

                    cbo_Grid_TableNo.Width = rect.Width  ' .CurrentCell.Size.Width
                    cbo_Grid_TableNo.Height = rect.Height  ' rect.Height
                    cbo_Grid_TableNo.Text = .CurrentCell.Value  '  Trim(.CurrentRow.Cells(e.ColumnIndex).Value)

                    cbo_Grid_TableNo.Tag = Val(e.RowIndex)
                    cbo_Grid_TableNo.Visible = True

                    cbo_Grid_TableNo.BringToFront()
                    cbo_Grid_TableNo.Focus()


                End If


            Else

                cbo_Grid_TableNo.Visible = False

            End If

            If e.ColumnIndex = 3 Then
                If cbo_Grid_Supervisor.Visible = False Or Val(cbo_Grid_Supervisor.Tag) <> e.RowIndex Then

                    cbo_Grid_Supervisor.Tag = -1
                    Da = New SqlClient.SqlDataAdapter("select Employee_Name from Employee_Head order by Employee_Name", con)
                    Dt2 = New DataTable
                    Da.Fill(Dt2)
                    cbo_Grid_Supervisor.DataSource = Dt2
                    cbo_Grid_Supervisor.DisplayMember = "Employee_Name"

                    rect = .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False)

                    cbo_Grid_Supervisor.Left = .Left + rect.Left  '  .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Left
                    cbo_Grid_Supervisor.Top = .Top + rect.Top  ' .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Top
                    cbo_Grid_Supervisor.Width = rect.Width  ' .CurrentCell.Size.Width
                    cbo_Grid_Supervisor.Height = rect.Height  ' rect.Height

                    cbo_Grid_Supervisor.Text = .CurrentCell.Value  '  Trim(.CurrentRow.Cells(e.ColumnIndex).Value)

                    cbo_Grid_Supervisor.Tag = Val(e.RowIndex)
                    cbo_Grid_Supervisor.Visible = True

                    cbo_Grid_Supervisor.BringToFront()
                    cbo_Grid_Supervisor.Focus()

                End If

            Else

                cbo_Grid_Supervisor.Visible = False

            End If

            If e.ColumnIndex = 4 Then
                If cbo_Grid_checker.Visible = False Or Val(cbo_Grid_checker.Tag) <> e.RowIndex Then

                    cbo_Grid_checker.Tag = -1
                    Da = New SqlClient.SqlDataAdapter("select Employee_Name from Employee_Head order by Employee_Name", con)
                    Dt3 = New DataTable
                    Da.Fill(Dt3)
                    cbo_Grid_checker.DataSource = Dt3
                    cbo_Grid_checker.DisplayMember = "Employee_Name"

                    rect = .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False)

                    cbo_Grid_checker.Left = .Left + rect.Left  '  .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Left
                    cbo_Grid_checker.Top = .Top + rect.Top  ' .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Top
                    cbo_Grid_checker.Width = rect.Width  ' .CurrentCell.Size.Width
                    cbo_Grid_checker.Height = rect.Height  ' rect.Height

                    cbo_Grid_checker.Text = .CurrentCell.Value  '  Trim(.CurrentRow.Cells(e.ColumnIndex).Value)

                    cbo_Grid_checker.Tag = Val(e.RowIndex)
                    cbo_Grid_checker.Visible = True

                    cbo_Grid_checker.BringToFront()
                    cbo_Grid_checker.Focus()

                End If

            Else

                cbo_Grid_checker.Visible = False

            End If



            If e.ColumnIndex = 8 Then
                If cbo_grid_folder.Visible = False Or Val(cbo_grid_folder.Tag) <> e.RowIndex Then

                    cbo_grid_folder.Tag = -1
                    Da = New SqlClient.SqlDataAdapter("select Employee_Name from Employee_Head order by Employee_Name", con)
                    Dt3 = New DataTable
                    Da.Fill(Dt3)
                    cbo_grid_folder.DataSource = Dt3
                    cbo_grid_folder.DisplayMember = "Employee_Name"

                    rect = .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False)

                    cbo_grid_folder.Left = .Left + rect.Left  '  .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Left
                    cbo_grid_folder.Top = .Top + rect.Top  ' .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Top
                    cbo_grid_folder.Width = rect.Width  ' .CurrentCell.Size.Width
                    cbo_grid_folder.Height = rect.Height  ' rect.Height

                    cbo_grid_folder.Text = .CurrentCell.Value  '  Trim(.CurrentRow.Cells(e.ColumnIndex).Value)

                    cbo_grid_folder.Tag = Val(e.RowIndex)
                    cbo_grid_folder.Visible = True

                    cbo_grid_folder.BringToFront()
                    cbo_grid_folder.Focus()

                End If

            Else

                cbo_grid_folder.Visible = False

            End If

        End With
    End Sub

    Private Sub dgv_YarnDetails_CellLeave(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_details.CellLeave
        With dgv_details

            If .CurrentCell.ColumnIndex = 2 Then
                If Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value) <> 0 Then
                    .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = Format(Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value), "#########0.000")
                End If
            End If
        End With
    End Sub

    Private Sub dgv_YarnDetails_CellValueChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_details.CellValueChanged
        On Error Resume Next
        If IsNothing(dgv_details.CurrentCell) Then Exit Sub
        With dgv_details
            If .Visible Then
                If .CurrentCell.ColumnIndex = 2 Then
                    Total_Calculation()
                End If
            End If
        End With
    End Sub

    Private Sub dgv_YarnDetails_EditingControlShowing(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewEditingControlShowingEventArgs) Handles dgv_details.EditingControlShowing
        dgtxt_Details = CType(dgv_details.EditingControl, DataGridViewTextBoxEditingControl)
    End Sub

    Private Sub dgv_YarnDetails_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_details.KeyDown

        With dgv_details

            If e.KeyCode = Keys.Up Then
                If .CurrentCell.RowIndex = 1 Then
                    .CurrentCell.Selected = False
                    txt_total_pcs.Focus()
                End If
            End If
            If e.KeyCode = Keys.Left Then
                If .CurrentCell.RowIndex = 0 And .CurrentCell.ColumnIndex <= 1 Then
                    .CurrentCell.Selected = False
                    txt_total_pcs.Focus()
                    'SendKeys.Send("{RIGHT}")
                End If
            End If

            If e.KeyCode = Keys.Enter Then
                e.SuppressKeyPress = True
                e.Handled = True

                If .CurrentCell.RowIndex = .RowCount - 1 And .CurrentCell.ColumnIndex >= 1 And Trim(.CurrentRow.Cells(1).Value) = "" Then

                    If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                        save_record()
                    End If

                Else
                    SendKeys.Send("{Tab}")

                End If


            End If

        End With


    End Sub

    Private Sub dgv_YarnDetails_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_details.KeyUp




        Dim i As Integer

        If e.Control = True And UCase(Chr(e.KeyCode)) = "D" Then

            With dgv_details

                .Rows.RemoveAt(.CurrentRow.Index)

                For i = 0 To .Rows.Count - 1
                    .Rows(i).Cells(0).Value = i + 1
                Next

            End With
        End If

    End Sub

    Private Sub dgv_YarnDetails_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgv_details.LostFocus
        On Error Resume Next
        If Not IsNothing(dgv_details.CurrentCell) Then dgv_details.CurrentCell.Selected = False
    End Sub

    Private Sub dgv_YarnDetails_RowsAdded(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewRowsAddedEventArgs) Handles dgv_details.RowsAdded
        Dim n As Integer
        If IsNothing(dgv_details.CurrentCell) Then Exit Sub
        With dgv_details
            n = .RowCount
            .Rows(n - 1).Cells(0).Value = Val(n)

            'If Val(.Rows(e.RowIndex).Cells(8).Value) = 0 Then
            '    Set_Max_DetailsSlNo(e.RowIndex, 8)
            'End If
        End With
    End Sub

    Private Sub get_MillCount_Details()
        Dim q As Single = 0
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim Cn_bag As Single
        Dim Wgt_Bag As Single
        Dim Wgt_Cn As Single
        Dim CntID As Integer
        Dim MilID As Integer

        CntID = Common_Procedures.Count_NameToIdNo(con, dgv_details.Rows(dgv_details.CurrentRow.Index).Cells(1).Value)
        MilID = Common_Procedures.Mill_NameToIdNo(con, dgv_details.Rows(dgv_details.CurrentRow.Index).Cells(3).Value)

        Wgt_Bag = 0 : Wgt_Cn = 0 : Cn_bag = 0

        If CntID <> 0 And MilID <> 0 Then

            Da = New SqlClient.SqlDataAdapter("select * from Mill_Count_Details where Table_No = " & Str(Val(MilID)) & " and count_idno = " & Str(Val(CntID)), con)
            Da.Fill(Dt)
            With dgv_details

                If Dt.Rows.Count > 0 Then
                    If IsDBNull(Dt.Rows(0)(0).ToString) = False Then
                        Wgt_Bag = Dt.Rows(0).Item("Weight_Bag").ToString
                        Wgt_Cn = Dt.Rows(0).Item("Weight_Cone").ToString
                        Cn_bag = Dt.Rows(0).Item("Cones_Bag").ToString
                    End If
                End If

                Dt.Clear()
                Dt.Dispose()
                Da.Dispose()

                If .CurrentCell.ColumnIndex = 4 Or .CurrentCell.ColumnIndex = 5 Then
                    If .CurrentCell.ColumnIndex = 4 Then
                        If Val(Cn_bag) <> 0 Then
                            .Rows(.CurrentRow.Index).Cells(5).Value = .Rows(.CurrentRow.Index).Cells(4).Value * Val(Cn_bag)
                        End If

                        If Val(Wgt_Bag) <> 0 Then
                            .Rows(.CurrentRow.Index).Cells(6).Value = Format(Val(.Rows(.CurrentRow.Index).Cells(4).Value) * Val(Wgt_Bag), "#########0.000")
                        End If

                    End If

                    If .CurrentCell.ColumnIndex = 5 Then
                        If Val(Wgt_Cn) <> 0 Then
                            .Rows(.CurrentRow.Index).Cells(6).Value = Format(.Rows(.CurrentRow.Index).Cells(5).Value * Val(Wgt_Cn), "##########0.000")
                        End If

                    End If

                End If

            End With

        End If

    End Sub

    Private Sub Total_Calculation()
        Dim Sno As Integer
        Dim Totpcs As Single

        Sno = 0
        Totpcs = 0


        With dgv_details
            For i = 0 To .RowCount - 1
                Sno = Sno + 1
                .Rows(i).Cells(0).Value = Sno
                If Val(.Rows(i).Cells(2).Value) <> 0 Then
                    Totpcs = Totpcs + Val(.Rows(i).Cells(2).Value)

                End If
            Next
        End With

        With dgv_YarnDetails_Total
            If .RowCount = 0 Then .Rows.Add()
            .Rows(0).Cells(2).Value = Val(Totpcs)

        End With

    End Sub

    Private Sub cbo_Grid_YarnType_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Grid_Supervisor.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Employee_Head", "Employee_name", "", "(Employee_IdNo = 0)")

    End Sub
    Private Sub cbo_Grid_YarnType_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Grid_Supervisor.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Grid_Supervisor, cbo_Grid_TableNo, cbo_Grid_checker, "Employee_Head", "Employee_name", "", "(Employee_IdNo = 0)")

        With dgv_details

            If (e.KeyValue = 38 And cbo_Grid_Supervisor.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
                .Focus()
                .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex - 1)
            End If

            If (e.KeyValue = 40 And cbo_Grid_Supervisor.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
                .Focus()
                .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 1)
            End If

        End With


    End Sub

    Private Sub cbo_Grid_Supervisor_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Grid_Supervisor.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Grid_Supervisor, cbo_Grid_checker, "Employee_Head", "Employee_name", "", "(Employee_IdNo = 0)")
        If Asc(e.KeyChar) = 13 Then

            With dgv_details

                .Focus()
                .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)

            End With

        End If
    End Sub
    Private Sub cbo_Grid_Supervisor_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Grid_Supervisor.TextChanged
        Try
            If cbo_Grid_Supervisor.Visible Then
                With dgv_details
                    If Val(cbo_Grid_Supervisor.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = 3 Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(3).Value = Trim(cbo_Grid_Supervisor.Text)
                    End If
                End With
            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub cbo_Grid_checker_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Grid_checker.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Employee_Head", "Employee_name", "", "(Employee_IdNo = 0)")

    End Sub

    Private Sub cbo_Grid_checker_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Grid_checker.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Grid_checker, cbo_Grid_Supervisor, cbo_grid_folder, "Employee_Head", "Employee_name", "", "(Employee_IdNo = 0)")

        With dgv_details

            If (e.KeyValue = 38 And cbo_Grid_checker.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
                .Focus()
                .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex - 1)
            End If

            If (e.KeyValue = 40 And cbo_Grid_checker.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then

                .Focus()
                .CurrentCell = .Rows(.CurrentRow.Index).Cells(8)
            End If

        End With



    End Sub

    Private Sub cbo_Grid_checker_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Grid_checker.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Grid_checker, cbo_grid_folder, "Employee_Head", "Employee_name", "", "(Employee_IdNo = 0)")
        If Asc(e.KeyChar) = 13 Then
            With dgv_details
                .Focus()
                .CurrentCell = .Rows(.CurrentRow.Index).Cells(8)
            End With
        End If

        '    With dgv_details

        '        If .CurrentCell.RowIndex = .Rows.Count - 1 Then
        '            cbo_grid_folder.Focus()

        '        ElseIf MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
        '            save_record()
        '        Else
        '            cbo_Lotcode_Selection.Focus()
        '        End If
        '        Else
        '        .Focus()
        '        .CurrentCell = .Rows(.CurrentCell.RowIndex + 1).Cells(1)
        '        End If

        'End With


    End Sub

    Private Sub cbo_Grid_checkere_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Grid_checker.KeyUp
        If e.Control = False And e.KeyValue = 17 Then
            Dim f As New EmployeeCreation_Simple

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Grid_checker.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If
    End Sub
    Private Sub cbo_Grid_checker_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Grid_checker.TextChanged
        Try
            If cbo_Grid_checker.Visible Then
                If IsNothing(dgv_details.CurrentCell) Then Exit Sub
                With dgv_details
                    If Val(cbo_Grid_checker.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = 4 Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(4).Value = Trim(cbo_Grid_checker.Text)
                    End If
                End With
            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub cbo_Grid_CountName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Grid_TableNo.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Checking_TableNo_Head", "Checking_Table_no", "", "(Checking_Table_idNo = 0)")

    End Sub
    Private Sub cbo_Grid_CountName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Grid_TableNo.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Grid_TableNo, Nothing, cbo_Grid_Supervisor, "Checking_TableNo_Head", "Checking_Table_no", "", "(Checking_Table_IdNo = 0)")
        With dgv_details
            With dgv_details

                If (e.KeyValue = 38 And cbo_Grid_TableNo.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
                    If .CurrentCell.RowIndex = 0 Then
                        cbo_Lotcode_Selection.Focus()
                    Else
                        .Focus()
                        .CurrentCell = .Rows(.CurrentRow.Index - 1).Cells(8)
                    End If
                End If
                If (e.KeyValue = 40 And cbo_Grid_TableNo.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
                    .Focus()
                    .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 1)
                End If

            End With

        End With
    End Sub

    Private Sub cbo_Grid_CountName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Grid_TableNo.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Grid_TableNo, Nothing, "Checking_TableNo_Head", "Checking_Table_no", "", "(Checking_Table_IdNo = 0)")
        If Asc(e.KeyChar) = 13 Then

            With dgv_details

                If .CurrentCell.RowIndex = .Rows.Count - 1 And Trim(cbo_Grid_TableNo.Text) = "" Then
                    If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                        save_record()
                    Else
                        cbo_Lotcode_Selection.Focus()
                    End If
                Else
                    .Focus()
                    .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)
                End If

            End With
        End If

    End Sub

    Private Sub cbo_Grid_CountName_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Grid_TableNo.KeyUp
        If e.Control = False And e.KeyValue = 17 Then
            Dim f As New Checking_table_creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Grid_TableNo.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If
    End Sub

    Private Sub cbo_Grid_CountName_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Grid_TableNo.TextChanged
        Try
            If cbo_Grid_TableNo.Visible Then
                If IsNothing(dgv_details.CurrentCell) Then Exit Sub
                With dgv_details
                    If Val(cbo_Grid_TableNo.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = 1 Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(1).Value = Trim(cbo_Grid_TableNo.Text)
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
    Private Sub dgtxt_Details_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgtxt_Details.Enter
        dgv_details.EditingControl.BackColor = Color.Lime
        dgv_details.EditingControl.ForeColor = Color.Blue
        dgtxt_Details.SelectAll()
    End Sub
    Private Sub dgtxt_Details_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgtxt_Details.KeyDown
        With dgv_details
            If e.KeyValue = Keys.Delete Then
                'If Val(.Rows(.CurrentCell.RowIndex).Cells(9).Value) <> 0 Then
                '    e.Handled = True
                'End If
            End If
        End With
    End Sub


    Private Sub dgtxt_Details_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dgtxt_Details.KeyPress
        With dgv_details
            If .Visible Then



                If .CurrentCell.ColumnIndex = 2 Then

                    If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
                        e.Handled = True
                    End If

                End If
            End If
        End With
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
        Dim Led_IdNo As Integer, Del_IdNo As Integer, Cnt_IdNo As Integer, Mil_IdNo As Integer
        Dim Condt As String = ""

        Try

            Condt = ""
            Del_IdNo = 0
            Cnt_IdNo = 0

            If IsDate(dtp_Filter_Fromdate.Value) = True And IsDate(dtp_Filter_ToDate.Value) = True Then
                Condt = "a.Lot_Allotment_Details_Date between '" & Trim(Format(dtp_Filter_Fromdate.Value, "MM/dd/yyyy")) & "' and '" & Trim(Format(dtp_Filter_ToDate.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_Filter_Fromdate.Value) = True Then
                Condt = "a.Lot_Allotment_Details_Date = '" & Trim(Format(dtp_Filter_Fromdate.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_Filter_ToDate.Value) = True Then
                Condt = "a.Lot_Allotment_Details_Date = '" & Trim(Format(dtp_Filter_ToDate.Value, "MM/dd/yyyy")) & "' "
            End If

            If Trim(cbo_Filter_PartyName.Text) <> "" Then
                Led_IdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Filter_PartyName.Text)
            End If
            If Trim(cbo_Filter_CountName.Text) <> "" Then
                Cnt_IdNo = Common_Procedures.Count_NameToIdNo(con, cbo_Filter_CountName.Text)
            End If

            If Trim(cbo_Filter_MillName.Text) <> "" Then
                Mil_IdNo = Common_Procedures.Mill_NameToIdNo(con, cbo_Filter_MillName.Text)
            End If

            If Val(Led_IdNo) <> 0 Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " (a.DeliveryTo_IdNo = " & Str(Val(Led_IdNo)) & ")"
            End If
            If Val(Cnt_IdNo) <> 0 Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " a.Lot_Allotment_Details_Code IN ( select z1.Lot_Allotment_Details_Code from Lot_Allotment_Details z1 where z1.Count_IdNo = " & Str(Val(Cnt_IdNo)) & " )"
                'Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " b.Count_IdNo = " & Str(Val(Cnt_IdNo)) & ""
            End If

            If Val(Mil_IdNo) <> 0 Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " a.Lot_Allotment_Details_Code IN ( select z2.Lot_Allotment_Details_Code from Lot_Allotment_Details z2 where z2.Table_No = " & Str(Val(Mil_IdNo)) & " )"
                'Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " b.Table_No = " & Str(Val(Mil_IdNo))
            End If

            da = New SqlClient.SqlDataAdapter("select a.*, e.Ledger_Name from LotAllotment_Head a inner join Ledger_head e on a.DeliveryTo_IdNo = e.Ledger_idno where a.company_idno =" & Str(Val(lbl_Company.Tag)) & "and a.Lot_Allotment_Details_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' " & IIf(Trim(Condt) <> "", " and ", "") & Condt & " Order by a.Lot_Allotment_Details_Date, a.for_orderby, a.Lot_Allotment_Details_No", con)
            'da = New SqlClient.SqlDataAdapter("select a.*, e.Ledger_Name from Lot_Allotment_Head a left outer join Lot_Allotment_Details b on a.Lot_Allotment_Details_Code = b.Lot_Allotment_Details_Code left outer join Ledger_head e on a.Ledger_idno = e.Ledger_idno where a.company_idno =" & Str(Val(lbl_Company.Tag)) & "and a.Lot_Allotment_Details_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' " & IIf(Trim(Condt) <> "", " and ", "") & Condt & " Order by a.Lot_Allotment_Details_Date, a.for_orderby, a.Lot_Allotment_Details_No", con)
            da.Fill(dt2)

            dgv_Filter_Details.Rows.Clear()

            If dt2.Rows.Count > 0 Then

                For i = 0 To dt2.Rows.Count - 1

                    n = dgv_Filter_Details.Rows.Add()

                    dgv_Filter_Details.Rows(n).Cells(0).Value = i + 1
                    dgv_Filter_Details.Rows(n).Cells(1).Value = dt2.Rows(i).Item("Lot_Allotment_Details_No").ToString
                    dgv_Filter_Details.Rows(n).Cells(2).Value = Format(Convert.ToDateTime(dt2.Rows(i).Item("Lot_Allotment_Details_Date").ToString), "dd-MM-yyyy")
                    dgv_Filter_Details.Rows(n).Cells(3).Value = dt2.Rows(i).Item("Ledger_Name").ToString
                    dgv_Filter_Details.Rows(n).Cells(4).Value = Val(dt2.Rows(i).Item("Total_Bags").ToString)
                    dgv_Filter_Details.Rows(n).Cells(5).Value = Val(dt2.Rows(i).Item("Total_Cones").ToString)
                    dgv_Filter_Details.Rows(n).Cells(6).Value = Format(Val(dt2.Rows(i).Item("Total_Weight").ToString), "########0.000")

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
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "( (Ledger_Type = 'REWINDING' or (Ledger_Type = '' and Stock_Maintenance_Status = 1) or Show_In_All_Entry = 1 )  )", "(Ledger_idno = 0)")
    End Sub

    Private Sub cbo_Filter_PartyName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Filter_PartyName.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_PartyName, dtp_Filter_ToDate, cbo_Filter_CountName, "Ledger_AlaisHead", "Ledger_DisplayName", "( (Ledger_Type = 'REWINDING' or (Ledger_Type = '' and Stock_Maintenance_Status = 1) or Show_In_All_Entry = 1 ) )", "(Ledger_idno = 0)")
    End Sub

    Private Sub cbo_Filter_PartyName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Filter_PartyName.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Filter_PartyName, cbo_Filter_CountName, "Ledger_AlaisHead", "Ledger_DisplayName", "( (Ledger_Type = 'REWINDING' or (Ledger_Type = '' and Stock_Maintenance_Status = 1) or Show_In_All_Entry = 1 ) )", "(Ledger_idno = 0)")
    End Sub

    Private Sub cbo_Filter_CountName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Filter_CountName.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Count_Head", "Count_Name", "", "(Count_IdNo = 0)")

    End Sub


    Private Sub cbo_Filter_CountName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Filter_CountName.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_CountName, cbo_Filter_PartyName, cbo_Filter_MillName, "Count_Head", "Count_Name", "", "(Count_IdNo = 0)")

    End Sub

    Private Sub cbo_Filter_CountName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Filter_CountName.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Filter_CountName, cbo_Filter_MillName, "Count_Head", "Count_Name", "", "(Count_IdNo = 0)")

    End Sub

    Private Sub Open_FilterEntry()
        Dim movno As String

        On Error Resume Next

        movno = Trim(dgv_Filter_Details.CurrentRow.Cells(1).Value)

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

    Private Sub cbo_Filter_MillName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Filter_MillName.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Mill_Head", "Mill_Name", "", "(Table_No = 0)")

    End Sub

    Private Sub cbo_Filter_MillName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Filter_MillName.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_MillName, cbo_Filter_CountName, btn_Filter_Show, "Mill_Head", "Mill_Name", "", "(Table_No = 0)")

    End Sub

    Private Sub cbo_Filter_MillName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Filter_MillName.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Filter_MillName, btn_Filter_Show, "Mill_Head", "Mill_Name", "", "(Table_No = 0)")
    End Sub

    Private Sub btn_Print_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Print.Click
        Common_Procedures.Print_OR_Preview_Status = 1
        print_record()
    End Sub

    Public Sub print_record() Implements Interface_MDIActions.print_record
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim NewCode As String

        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.PrintEntry, Common_Procedures.UR.Lot_Allotment_Details, New_Entry) = False Then Exit Sub


        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(Common_Procedures.FnYearCode)

        Try

            da1 = New SqlClient.SqlDataAdapter("select * from LotAllotment_Head Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Lot_Allotment_Details_Code = '" & Trim(NewCode) & "'", con)
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

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(Common_Procedures.FnYearCode)

        prn_HdDt.Clear()
        prn_DetDt.Clear()
        prn_DetIndx = 0
        prn_DetSNo = 0
        prn_PageNo = 0

        Try

            da1 = New SqlClient.SqlDataAdapter("select a.*, b.*, c.*, d.Ledger_Name as Ledger1_Name , e.Transport_Name  from LotAllotment_Head a INNER JOIN Company_Head b ON a.Company_IdNo = b.Company_IdNo INNER JOIN Ledger_Head c ON a.DeliveryTo_IdNo = c.Ledger_IdNo Left Outer JOIN Ledger_Head d ON a.ReceivedFrom_IdNo = d.Ledger_IdNo Left Outer JOIN Transport_Head e ON a.Transport_IdNo = e.Transport_IdNo where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Lot_Allotment_Details_Code = '" & Trim(NewCode) & "'", con)
            prn_HdDt = New DataTable
            da1.Fill(prn_HdDt)

            If prn_HdDt.Rows.Count > 0 Then

                da2 = New SqlClient.SqlDataAdapter("select a.*, b.Mill_name, d.Count_name  from Lot_Allotment_Details a INNER JOIN Mill_Head b ON a.Table_No = b.Table_No LEFT OUTER JOIN Count_Head d ON a.Count_idno = d.Count_idno where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Lot_Allotment_Details_Code = '" & Trim(NewCode) & "' Order by a.Sl_No", con)
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

        Printing_Format1(e)

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
        Dim LnAr(15) As Single, ClAr(15) As Single
        Dim EntryCode As String
        Dim CurY As Single
        Dim ItmNm1 As String, ItmNm2 As String

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
        If PrintDocument1.DefaultPageSettings.Landscape = True Then
            With PrintDocument1.DefaultPageSettings.PaperSize
                PrintWidth = .Height - TMargin - BMargin
                PrintHeight = .Width - RMargin - LMargin
                PageWidth = .Height - TMargin
                PageHeight = .Width - RMargin
            End With
        End If

        NoofItems_PerPage = 6

        Erase LnAr
        Erase ClAr

        LnAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        ClAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}

        ClAr(1) = Val(45) : ClAr(2) = 120 : ClAr(3) = 250 : ClAr(4) = 80 : ClAr(5) = 75 : ClAr(6) = 90
        ClAr(7) = PageWidth - (LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6))

        TxtHgt = 18.5

        EntryCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(Common_Procedures.FnYearCode)

        Try
            If prn_HdDt.Rows.Count > 0 Then

                Printing_Format1_PageHeader(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, prn_PageNo, NoofItems_PerPage, CurY, LnAr, ClAr)

                NoofDets = 0

                CurY = CurY - 10

                If prn_DetDt.Rows.Count > 0 Then

                    Do While prn_DetIndx <= prn_DetDt.Rows.Count - 1

                        If NoofDets >= NoofItems_PerPage Then

                            CurY = CurY + TxtHgt

                            Common_Procedures.Print_To_PrintDocument(e, "Continued...", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + 10, CurY, 0, 0, pFont)

                            NoofDets = NoofDets + 1

                            Printing_Format1_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClAr, NoofDets, False)

                            e.HasMorePages = True
                            Return

                        End If

                        ItmNm1 = Trim(prn_DetDt.Rows(prn_DetIndx).Item("Mill_Name").ToString)
                        ItmNm2 = ""
                        If Len(ItmNm1) > 35 Then
                            For I = 15 To 1 Step -1
                                If Mid$(Trim(ItmNm1), I, 1) = " " Or Mid$(Trim(ItmNm1), I, 1) = "," Or Mid$(Trim(ItmNm1), I, 1) = "." Or Mid$(Trim(ItmNm1), I, 1) = "-" Or Mid$(Trim(ItmNm1), I, 1) = "/" Or Mid$(Trim(ItmNm1), I, 1) = "_" Or Mid$(Trim(ItmNm1), I, 1) = "(" Or Mid$(Trim(ItmNm1), I, 1) = ")" Or Mid$(Trim(ItmNm1), I, 1) = "\" Or Mid$(Trim(ItmNm1), I, 1) = "[" Or Mid$(Trim(ItmNm1), I, 1) = "]" Or Mid$(Trim(ItmNm1), I, 1) = "{" Or Mid$(Trim(ItmNm1), I, 1) = "}" Then Exit For
                            Next I
                            If I = 0 Then I = 35
                            ItmNm2 = Microsoft.VisualBasic.Right(Trim(ItmNm1), Len(ItmNm1) - I)
                            ItmNm1 = Microsoft.VisualBasic.Left(Trim(ItmNm1), I - 1)
                        End If

                        CurY = CurY + TxtHgt
                        Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetDt.Rows(prn_DetIndx).Item("Sl_No").ToString), LMargin + 15, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetDt.Rows(prn_DetIndx).Item("Lot_Checker").ToString), LMargin + ClAr(1) + 10, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm1), LMargin + ClAr(1) + ClAr(2) + 10, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Count_Name").ToString, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + 10, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(prn_DetIndx).Item("Bags").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 1, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(prn_DetIndx).Item("Cones").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY, 1, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Weight").ToString), " #######0.000"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, pFont)

                        NoofDets = NoofDets + 1

                        If Trim(ItmNm2) <> "" Then
                            CurY = CurY + TxtHgt - 5
                            Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm2), LMargin + ClAr(1) + 10, CurY, 0, 0, pFont)
                            NoofDets = NoofDets + 1
                        End If

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
        Dim W1 As Single
        Dim S1 As Single

        PageNo = PageNo + 1

        CurY = TMargin

        da2 = New SqlClient.SqlDataAdapter("select a.*, b.Mill_name, d.Count_name  from Lot_Allotment_Details a INNER JOIN Mill_Head b ON a.Table_No = b.Table_No LEFT OUTER JOIN Count_Head d ON a.Count_idno = d.Count_idno where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Lot_Allotment_Details_Code = '" & Trim(EntryCode) & "' Order by a.Sl_No", con)
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

        CurY = CurY + TxtHgt - 5
        p1Font = New Font("Calibri", 16, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "YARN DELIVERY", LMargin, CurY, 2, PrintWidth, p1Font)
        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height

        CurY = CurY + strHeight + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(2) = CurY

        C1 = ClAr(1) + ClAr(2) + ClAr(3)
        W1 = e.Graphics.MeasureString("D.C DATE  : ", pFont).Width
        S1 = e.Graphics.MeasureString("TO  :  ", pFont).Width


        CurY = CurY + TxtHgt - 10
        p1Font = New Font("Calibri", 12, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "TO  :  " & "M/s." & prn_HdDt.Rows(0).Item("Ledger_MainName").ToString, LMargin + 10, CurY, 0, 0, p1Font)
        Common_Procedures.Print_To_PrintDocument(e, "DC.NO", LMargin + C1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Lot_Allotment_Details_No").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, p1Font)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address1").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
        p1Font = New Font("Calibri", 14, FontStyle.Bold)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address2").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, "DATE", LMargin + C1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Lot_Allotment_Details_Date").ToString), "dd-MM-yyyy").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, pFont)

        CurY = CurY + TxtHgt + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(3) = CurY

        e.Graphics.DrawLine(Pens.Black, LMargin + C1, CurY, LMargin + C1, LnAr(2))

        CurY = CurY + TxtHgt - 10
        Common_Procedures.Print_To_PrintDocument(e, "SNO", LMargin, CurY, 2, ClAr(1), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "YARN TYPE", LMargin + ClAr(1), CurY, 2, ClAr(2), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "MILL NAME", LMargin + ClAr(1) + ClAr(2), CurY, 2, ClAr(3), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "COUNT", LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, 2, ClAr(4), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "BAG", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 2, ClAr(5), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "CONE", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, 2, ClAr(6), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "WEIGHT", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, 2, ClAr(7), pFont)

        CurY = CurY + TxtHgt + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(4) = CurY


    End Sub

    Private Sub Printing_Format1_PageFooter(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single, ByVal NoofDets As Integer, ByVal is_LastPage As Boolean)
        Dim i As Integer
        Dim Cmp_Name As String
        Dim p1Font As Font
        Dim W1 As Single

        For i = NoofDets + 1 To NoofItems_PerPage
            CurY = CurY + TxtHgt
        Next

        W1 = e.Graphics.MeasureString(" Vehicle No :", pFont).Width

        CurY = CurY + TxtHgt
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(5) = CurY

        CurY = CurY + TxtHgt - 10
        If is_LastPage = True Then
            Common_Procedures.Print_To_PrintDocument(e, " TOTAL", LMargin + ClAr(1) + 30, CurY, 2, ClAr(4), pFont)

            Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(0).Item("Total_Bags").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 1, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(0).Item("Total_Cones").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY, 1, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Total_Weight").ToString), " #######0.000"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, pFont)

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

        CurY = CurY + 10

        Common_Procedures.Print_To_PrintDocument(e, " Vehicle No : ", LMargin + 20, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Vechile_No").ToString, LMargin + W1 + 30, CurY, 0, 0, pFont)

        CurY = CurY + TxtHgt + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

        CurY = CurY + TxtHgt
        CurY = CurY + TxtHgt

        Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString
        p1Font = New Font("Calibri", 12, FontStyle.Bold)

        Common_Procedures.Print_To_PrintDocument(e, "Receiver's Signature", LMargin + 20, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, "For " & Cmp_Name, PageWidth - 15, CurY, 1, 0, p1Font)

        CurY = CurY + TxtHgt + 10

        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

        e.Graphics.DrawLine(Pens.Black, LMargin, LnAr(1), LMargin, CurY)
        e.Graphics.DrawLine(Pens.Black, PageWidth, LnAr(1), PageWidth, CurY)

    End Sub

    Private Sub cbo_Vechile_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Weaver_Yarn_Delivery_Head", "Vechile_No", "", "")

    End Sub

    'Private Sub cbo_Vechile_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Vechile.KeyDown
    '    Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Vechile, txt_total_pcs, cbo_TransportName, "Weaver_Yarn_Delivery_Head", "Vechile_No", "", "")

    'End Sub

    'Private Sub cbo_Vechile_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Vechile.KeyPress
    '    Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Vechile, cbo_TransportName, "Weaver_Yarn_Delivery_Head", "Vechile_No", "", "", False)

    'End Sub

    Private Sub txt_Freight_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs)
        If e.KeyValue = 38 Then SendKeys.Send("+{TAB}")
        If (e.KeyValue = 40) Then
            If dgv_details.Rows.Count > 0 Then
                dgv_details.Focus()
                dgv_details.CurrentCell = dgv_details.Rows(0).Cells(1)

            Else
                btn_save.Focus()

            End If
        End If

    End Sub

    Private Sub txt_Freight_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
            e.Handled = True
        End If
        If Asc(e.KeyChar) = 13 Then
            If dgv_details.Rows.Count > 0 Then
                dgv_details.Focus()
                dgv_details.CurrentCell = dgv_details.Rows(0).Cells(1)

            Else
                btn_save.Focus()

            End If
        End If
    End Sub

    Private Sub cbo_TransportName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'TRANSPORT')", "(Ledger_IdNo = 0)")

    End Sub

    'Private Sub cbo_Transportname_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs)
    '    Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_TransportName, cbo_Vechile, txt_Freight, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'TRANSPORT')", "(Ledger_IdNo = 0)")
    'End Sub

    'Private Sub cbo_Transportname_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
    '    Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_TransportName, txt_Freight, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'TRANSPORT')", "(Ledger_IdNo = 0)")
    'End Sub

    Private Sub cbo_Transport_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs)
        If e.Control = False And e.KeyValue = 17 Then
            Common_Procedures.UR.Ledr_Wea_Siz_Rw_Trans_JbWrk_Creation = Common_Procedures.UR.Ledger_Creation
            Common_Procedures.MDI_LedType = "TRANSPORT"
            Dim f As New Ledger_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            'Common_Procedures.Master_Return.Control_Name = cbo_TransportName.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()
        End If
    End Sub
    Private Sub dgtxt_Details_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgtxt_Details.KeyUp
        If e.Control = True And UCase(Chr(e.KeyCode)) = "D" Then
            dgv_YarnDetails_KeyUp(sender, e)
        End If
    End Sub

    'Private Sub cbo_Type_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs)
    '    vcbo_KeyDwnVal = e.KeyValue
    '    Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Type, msk_date, cbo_clothname, "", "", "", "")
    'End Sub

    'Private Sub cbo_Type_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
    '    Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Type, cbo_clothname, "", "", "", "")


    'End Sub

    Private Sub btn_Selection_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim i As Integer, j As Integer, n As Integer, SNo As Integer
        Dim LedNo As Integer
        Dim NewCode As String
        Dim CompIDCondt As String
        Dim Ent_Wgt As Single = 0
        Dim Ent_Bg As Integer = 0
        Dim Ent_cne As Integer = 0
        Dim Ent_DcDetSlNo As Long

        'If Trim(cbo_Type.Text) <> "SELECTION" Then
        '    MessageBox.Show("Invalid Entry Type", "DOES NOT SELECT BABY CONE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
        '    If cbo_Type.Enabled And cbo_Type.Visible Then cbo_Type.Focus()
        '    Exit Sub
        'End If

        'LedNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Rec_Ledger.Text)

        If LedNo = 0 Then
            MessageBox.Show("Invalid Party Name", "DOES NOT SELECT BABY CONE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            'If cbo_Rec_Ledger.Enabled And cbo_Rec_Ledger.Visible Then cbo_Rec_Ledger.Focus()
            Exit Sub
        End If

        CompIDCondt = "(a.company_idno = " & Str(Val(lbl_Company.Tag)) & ")"
        If Trim(UCase(Common_Procedures.settings.CompanyName)) = "-----~~~" Then
            CompIDCondt = ""
        End If
        'NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        With dgv_Selection

            .Rows.Clear()
            SNo = 0

            Da = New SqlClient.SqlDataAdapter("select a.*, b.Weight as Ent_DcWeight,  b.Cones as Ent_DcCones ,  b.Bags as Ent_DcBags , b.Lot_Allotment_Details_SlNo as Ent_Lot_Allotment_Details_SlNo , c.Count_Name, d.Mill_Name from Stock_BabyCone_Processing_Details a LEFT OUTER JOIN Lot_Allotment_Details b ON b.Lot_Allotment_Details_Code = '" & Trim(NewCode) & "' and a.Reference_Code = b.Reference_Code LEFT OUTER JOIN Count_Head c ON a.Count_IdNo = c.Count_IdNo LEFT OUTER JOIN Mill_Head d ON a.Table_No = d.Table_No where  " & CompIDCondt & IIf(Trim(CompIDCondt) <> "", " and ", "") & "  a.Lot_Checker = 'BABY' and a.DeliveryTo_IdNo  = " & Str(Val(LedNo)) & " and ((a.Baby_Weight - a.Delivered_Weight) > 0 or b.Weight > 0 )  order by a.for_orderby, a.Set_Code", con)
            Dt1 = New DataTable
            Da.Fill(Dt1)

            If Dt1.Rows.Count > 0 Then

                For i = 0 To Dt1.Rows.Count - 1

                    n = .Rows.Add()
                    Ent_Wgt = 0
                    Ent_Bg = 0
                    Ent_cne = 0

                    Ent_DcDetSlNo = 0

                    If IsDBNull(Dt1.Rows(i).Item("Ent_DcWeight").ToString) = False Then
                        Ent_Wgt = Val(Dt1.Rows(i).Item("Ent_DcWeight").ToString)
                    End If
                    If IsDBNull(Dt1.Rows(i).Item("Ent_DcCones").ToString) = False Then
                        Ent_cne = Val(Dt1.Rows(i).Item("Ent_DcCones").ToString)
                    End If
                    If IsDBNull(Dt1.Rows(i).Item("Ent_DcBags").ToString) = False Then
                        Ent_Bg = Val(Dt1.Rows(i).Item("Ent_DcBags").ToString)
                    End If
                    If IsDBNull(Dt1.Rows(i).Item("Ent_Lot_Allotment_Details_SlNo").ToString) = False Then
                        Ent_DcDetSlNo = Val(Dt1.Rows(i).Item("Ent_Lot_Allotment_Details_SlNo").ToString)
                    End If

                    SNo = SNo + 1
                    .Rows(n).Cells(0).Value = Val(SNo)
                    .Rows(n).Cells(1).Value = Dt1.Rows(i).Item("Lot_Supervisor").ToString
                    .Rows(n).Cells(2).Value = Dt1.Rows(i).Item("Count_Name").ToString
                    .Rows(n).Cells(3).Value = Dt1.Rows(i).Item("Lot_Checker").ToString
                    .Rows(n).Cells(4).Value = Dt1.Rows(i).Item("Mill_Name").ToString
                    .Rows(n).Cells(5).Value = Val(Dt1.Rows(i).Item("Baby_Bags").ToString) - Val(Dt1.Rows(i).Item("Delivered_Bags").ToString) + Val(Ent_Bg)
                    .Rows(n).Cells(6).Value = Val(Dt1.Rows(i).Item("Baby_Cones").ToString) - Val(Dt1.Rows(i).Item("Delivered_Cones").ToString) + Val(Ent_cne)
                    .Rows(n).Cells(7).Value = Format(Val(Dt1.Rows(i).Item("Baby_Weight").ToString) - Val(Dt1.Rows(i).Item("Delivered_Weight").ToString) + Val(Ent_Wgt), "#########0.00")

                    If Ent_Wgt > 0 Then
                        .Rows(n).Cells(8).Value = "1"
                        For j = 0 To .ColumnCount - 1
                            .Rows(n).Cells(j).Style.ForeColor = Color.Red
                        Next

                    Else

                        .Rows(n).Cells(8).Value = ""

                    End If

                    .Rows(n).Cells(9).Value = Dt1.Rows(i).Item("Reference_Code").ToString
                    .Rows(n).Cells(10).Value = Ent_DcDetSlNo
                    .Rows(n).Cells(11).Value = Ent_Wgt
                    .Rows(n).Cells(12).Value = Ent_cne
                    .Rows(n).Cells(13).Value = Ent_Bg

                Next

            End If
            Dt1.Clear()

        End With

        pnl_Selection.Visible = True
        Pnl_Back.Enabled = False
        If dgv_Selection.Rows.Count > 0 Then
            dgv_Selection.Focus()
            dgv_Selection.CurrentCell = dgv_Selection.Rows(0).Cells(0)
        End If

    End Sub

    Private Sub dgv_Selection_CellClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Selection.CellClick
        Select_Baby(e.RowIndex)
    End Sub

    Private Sub Select_Baby(ByVal RwIndx As Integer)
        Dim i As Integer

        With dgv_Selection

            If .RowCount > 0 And RwIndx >= 0 Then

                'If Val(.Rows(RwIndx).Cells(8).Value) > 0 And Val(.Rows(RwIndx).Cells(8).Value) <> Val(.Rows(RwIndx).Cells(10).Value) Then
                '    MessageBox.Show("Cannot deselect" & Chr(13) & "Already this pavu delivered to others")
                '    Exit Sub
                'End If

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

        If e.KeyCode = Keys.Enter Or e.KeyCode = Keys.Space Then
            If dgv_Selection.CurrentCell.RowIndex >= 0 Then
                Select_Baby(dgv_Selection.CurrentCell.RowIndex)
                e.Handled = True
            End If
        End If
    End Sub

    Private Sub btn_Close_Selection_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Close_Selection.Click
        BabyCone_Selection()
    End Sub
    Private Sub BabyCone_Selection()
        Dim Da1 As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim n As Integer = 0
        Dim sno As Integer = 0
        Dim i As Integer = 0
        Dim j As Integer = 0

        dgv_details.Rows.Clear()

        For i = 0 To dgv_Selection.RowCount - 1

            If Val(dgv_Selection.Rows(i).Cells(8).Value) = 1 Then

                n = dgv_details.Rows.Add()
                sno = sno + 1
                dgv_details.Rows(n).Cells(0).Value = Val(sno)
                dgv_details.Rows(n).Cells(1).Value = dgv_Selection.Rows(i).Cells(2).Value
                dgv_details.Rows(n).Cells(2).Value = dgv_Selection.Rows(i).Cells(3).Value
                dgv_details.Rows(n).Cells(3).Value = dgv_Selection.Rows(i).Cells(4).Value
                'dgv_YarnDetails.Rows(n).Cells(4).Value = dgv_Selection.Rows(i).Cells(5).Value
                'dgv_YarnDetails.Rows(n).Cells(5).Value = dgv_Selection.Rows(i).Cells(6).Value
                ''dgv_YarnDetails.Rows(n).Cells(6).Value = dgv_Selection.Rows(i).Cells(7).Value
                dgv_details.Rows(n).Cells(7).Value = dgv_Selection.Rows(i).Cells(1).Value
                dgv_details.Rows(n).Cells(8).Value = dgv_Selection.Rows(i).Cells(10).Value
                dgv_details.Rows(n).Cells(9).Value = dgv_Selection.Rows(i).Cells(11).Value
                dgv_details.Rows(n).Cells(10).Value = dgv_Selection.Rows(i).Cells(9).Value



                If Val(dgv_Selection.Rows(i).Cells(11).Value) <> 0 Then
                    dgv_details.Rows(n).Cells(6).Value = dgv_Selection.Rows(i).Cells(11).Value
                Else
                    dgv_details.Rows(n).Cells(6).Value = dgv_Selection.Rows(i).Cells(7).Value
                End If

                If Val(dgv_Selection.Rows(i).Cells(12).Value) <> 0 Then
                    dgv_details.Rows(n).Cells(5).Value = dgv_Selection.Rows(i).Cells(12).Value
                Else
                    dgv_details.Rows(n).Cells(5).Value = dgv_Selection.Rows(i).Cells(6).Value
                End If

                If Val(dgv_Selection.Rows(i).Cells(13).Value) <> 0 Then
                    dgv_details.Rows(n).Cells(4).Value = dgv_Selection.Rows(i).Cells(13).Value
                Else
                    dgv_details.Rows(n).Cells(4).Value = dgv_Selection.Rows(i).Cells(5).Value
                End If

            End If
        Next
        For i = 0 To dgv_details.Rows.Count - 1
            If Val(dgv_details.Rows(i).Cells(8).Value) = 0 Then
                Set_Max_DetailsSlNo(i, 8)
            End If
        Next
        Total_Calculation()


        Pnl_Back.Enabled = True
        pnl_Selection.Visible = False
        If txt_total_pcs.Enabled And txt_total_pcs.Visible Then txt_total_pcs.Focus()

    End Sub

    Private Sub Set_Max_DetailsSlNo(ByVal RowNo As Integer, ByVal DetSlNo_ColNo As Integer)
        Dim MaxSlNo As Integer = 0
        Dim i As Integer

        With dgv_details
            For i = 0 To .Rows.Count - 1
                If Val(.Rows(i).Cells(DetSlNo_ColNo).Value) > Val(MaxSlNo) Then
                    MaxSlNo = Val(.Rows(i).Cells(DetSlNo_ColNo).Value)
                End If
            Next
            .Rows(RowNo).Cells(DetSlNo_ColNo).Value = Val(MaxSlNo) + 1
        End With

    End Sub


    Private Sub btn_UserModification_Click(sender As Object, e As System.EventArgs) Handles btn_UserModification.Click
        If Val(Common_Procedures.User.IdNo) = 1 Then
            Dim f1 As New User_Modifications
            f1.Entry_Name = Me.Name
            f1.Entry_PkValue = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(Common_Procedures.FnYearCode)
            f1.ShowDialog()
        End If
    End Sub


    Private Sub cbo_Grid_Supervisor_KeyUp(sender As Object, e As KeyEventArgs) Handles cbo_Grid_Supervisor.KeyUp
        If e.Control = False And e.KeyValue = 17 Then
            Dim f As New EmployeeCreation_Simple

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Grid_Supervisor.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If
    End Sub
    Private Sub cbo_lotno_GotFocus(sender As Object, e As EventArgs) Handles cbo_Lotcode_Selection.GotFocus
        Dim vCurYr As String = ""
        Dim vPreYr As String = ""
        Dim vAllotedDate As String = ""
        Dim vChkSec As String = ""
        vCurYr = Trim(Common_Procedures.FnYearCode)
        vPreYr = Microsoft.VisualBasic.Left(Trim(Common_Procedures.FnYearCode), 2)
        vPreYr = Trim(Format(Val(vPreYr) - 1, "00")) & "-" & Trim(Format(Val(vPreYr), "00"))

        vAllotedDate = " '" & Trim(Format(dtp_AllottedDate.Value, "yyy/MM/dd")) & "' "
        vChkSec = Common_Procedures.CheckingSection_NameToIdNo(con, Trim(cbo_Checking_Section.Text))


        'Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "lot_Approved_Head", "lotcode_forSelection", "( Approved_sts<>0 and (  lotcode_forSelection LIKE '%/" & Trim(vCurYr) & "%'  or lotcode_forSelection LIKE '%/" & Trim(vPreYr) & "%'  )  )", "(lotcode_forSelection = '')")

        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "lot_Approved_Head", "lotcode_forSelection", "( Approved_sts<>0 and lotcode_forSelection IN (Select sq1.lotcode_forSelection from Lot_Checking_Plan_Details sq1 Where Sq1.Checking_Section_Idno = " & Str(Val(vChkSec)) & " and sq1.Allotment_Date = " & vAllotedDate & "  ) )", "(lotcode_forSelection = '')")

    End Sub


    Private Sub cbo_lotno_KeyDown(sender As Object, e As KeyEventArgs) Handles cbo_Lotcode_Selection.KeyDown
        Dim vCurYr As String = ""
        Dim vPreYr As String = ""
        Dim vAllotedDate As String = ""
        Dim vChkSec As String = ""
        vCurYr = Trim(Common_Procedures.FnYearCode)
        vPreYr = Microsoft.VisualBasic.Left(Trim(Common_Procedures.FnYearCode), 2)
        vPreYr = Trim(Format(Val(vPreYr) - 1, "00")) & "-" & Trim(Format(Val(vPreYr), "00"))

        vAllotedDate = "'" & Trim(Format(dtp_AllottedDate.Value, "yyy/MM/dd")) & "' "
        vChkSec = Common_Procedures.CheckingSection_NameToIdNo(con, Trim(cbo_Checking_Section.Text))


        'Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Lotcode_Selection, Nothing, cbo_Checking_Section, "lot_Approved_Head", "lotcode_forSelection", "(  Approved_sts<>0     and (  lotcode_forSelection LIKE '%/" & Trim(vCurYr) & "%'  or lotcode_forSelection LIKE '%/" & Trim(vPreYr) & "%'  )  )", "(lotcode_forSelection = '')")

        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Lotcode_Selection, cbo_Checking_Section, Nothing, "lot_Approved_Head", "lotcode_forSelection", "(  Approved_sts<>0   and   lotcode_forSelection IN (Select sq1.lotcode_forSelection from Lot_Checking_Plan_Details sq1 Where sq1.Allotment_Date = " & (vAllotedDate) & " and Sq1.Checking_Section_Idno = '" & Trim(vChkSec) & "' ) )", "(lotcode_forSelection = '')")

    End Sub
    Private Sub cbo_lotno_KeyPress(sender As Object, e As KeyPressEventArgs) Handles cbo_Lotcode_Selection.KeyPress
        Dim vAllotedDate As String = ""
        Dim vCurYr As String = ""
        Dim vPreYr As String = ""
        Dim vChkSec As String = ""
        vCurYr = Trim(Common_Procedures.FnYearCode)
        vPreYr = Microsoft.VisualBasic.Left(Trim(Common_Procedures.FnYearCode), 2)
        vPreYr = Trim(Format(Val(vPreYr) - 1, "00")) & "-" & Trim(Format(Val(vPreYr), "00"))

        vAllotedDate = "'" & Trim(Format(dtp_AllottedDate.Value, "yyy/MM/dd")) & "' "
        vChkSec = Common_Procedures.CheckingSection_NameToIdNo(con, Trim(cbo_Checking_Section.Text))

        'Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Lotcode_Selection, Nothing, "lot_Approved_Head", "lotcode_forSelection", "(   Approved_sts<>0    and (  lotcode_forSelection LIKE '%/" & Trim(vCurYr) & "%'  or lotcode_forSelection LIKE '%/" & Trim(vPreYr) & "%'  ) )", "(lotcode_forSelection = '')")

        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Lotcode_Selection, Nothing, "lot_Approved_Head", "lotcode_forSelection", "(   Approved_sts<>0    and lotcode_forSelection IN (Select sq1.lotcode_forSelection from Lot_Checking_Plan_Details sq1 Where sq1.Allotment_Date = " & (vAllotedDate) & " and Sq1.Checking_Section_Idno = '" & Trim(vChkSec) & "' ) )", "(lotcode_forSelection = '')")

        If Asc(e.KeyChar) = 13 Then

            'cbo_Checking_Section.Focus()

            btn_get_LotDetails_Click(sender, e)

        End If

    End Sub

    Private Sub cbo_checker_KeyDown(sender As Object, e As KeyEventArgs) Handles cbo_checker.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_checker, cbo_Lotcode_Selection, cbo_folder, "Employee_Head", "Employee_Name", "", "(Employee_idno = 0)")
        'Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_checker, cbo_Lotcode_Selection, cbo_folder, "Employee_Head", "Employee_Name", "", "(Employee_idno = 0)")
    End Sub

    Private Sub cbo_checker_GotFocus(sender As Object, e As EventArgs) Handles cbo_checker.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Employee_Head", "Employee_Name", "", "(Employee_idno = 0)")
    End Sub

    Private Sub cbo_checker_KeyPress(sender As Object, e As KeyPressEventArgs) Handles cbo_checker.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_checker, cbo_folder, "Employee_Head", "Employee_Name", "", "(Employee_idno = 0)")

    End Sub

    Private Sub cbo_checker_KeyUp(sender As Object, e As KeyEventArgs) Handles cbo_checker.KeyUp
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


    Private Sub cbo_folder_GotFocus(sender As Object, e As EventArgs) Handles cbo_folder.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Employee_Head", "Employee_Name", "", "(Employee_idno = 0)")
    End Sub

    Private Sub cbo_folder_KeyDown(sender As Object, e As KeyEventArgs) Handles cbo_folder.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_folder, cbo_checker, Nothing, "Employee_Head", "Employee_Name", "", "(Employee_idno = 0)")
        If (e.KeyValue = 40 And cbo_folder.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
            If dgv_details.Rows.Count > 0 Then
                dgv_details.Focus()
                dgv_details.CurrentCell = dgv_details.Rows(0).Cells(1)

            Else
                btn_save.Focus()

            End If
        End If
    End Sub

    Private Sub cbo_folder_KeyPress(sender As Object, e As KeyPressEventArgs) Handles cbo_folder.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_folder, Nothing, "Employee_Head", "Employee_Name", "", "(Employee_idno = 0)")
        If Asc(e.KeyChar) = 13 Then
            If dgv_details.Rows.Count > 0 Then
                dgv_details.Focus()
                dgv_details.CurrentCell = dgv_details.Rows(0).Cells(1)

            Else
                btn_save.Focus()

            End If
        End If
    End Sub

    Private Sub cbo_folder_KeyUp(sender As Object, e As KeyEventArgs) Handles cbo_folder.KeyUp
        If e.Control = False And e.KeyValue = 17 Then
            Dim f As New EmployeeCreation_Simple

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_folder.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If
    End Sub

    Private Sub cbo_grid_folder_GotFocus(sender As Object, e As EventArgs) Handles cbo_grid_folder.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Employee_Head", "Employee_name", "", "(Employee_IdNo = 0)")
    End Sub

    Private Sub cbo_grid_folder_KeyDown(sender As Object, e As KeyEventArgs) Handles cbo_grid_folder.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_grid_folder, cbo_Grid_checker, Nothing, "Employee_Head", "Employee_name", "", "(Employee_IdNo = 0)")

        With dgv_details

            If (e.KeyValue = 38 And cbo_grid_folder.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
                .Focus()
                .CurrentCell = .Rows(.CurrentRow.Index).Cells(4)
            End If

            If (e.KeyValue = 40 And cbo_grid_folder.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
                .Focus()
                .CurrentCell = .Rows(.CurrentRow.Index + 1).Cells(1)
            End If

        End With
    End Sub

    Private Sub cbo_grid_folder_KeyPress(sender As Object, e As KeyPressEventArgs) Handles cbo_grid_folder.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_grid_folder, Nothing, "Employee_Head", "Employee_name", "", "(Employee_IdNo = 0)")
        If Asc(e.KeyChar) = 13 Then

            With dgv_details

                If .CurrentCell.RowIndex = .Rows.Count - 1 Then
                    If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                        save_record()
                    Else
                        cbo_Lotcode_Selection.Focus()
                    End If
                Else
                    .Focus()
                    .CurrentCell = .Rows(.CurrentCell.RowIndex + 1).Cells(1)
                End If

            End With

        End If
    End Sub

    Private Sub cbo_grid_folder_KeyUp(sender As Object, e As KeyEventArgs) Handles cbo_grid_folder.KeyUp
        If e.Control = False And e.KeyValue = 17 Then
            Dim f As New EmployeeCreation_Simple

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_grid_folder.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()
        End If
    End Sub

    Private Sub cbo_grid_folder_TextChanged(sender As Object, e As EventArgs) Handles cbo_grid_folder.TextChanged
        Try
            If cbo_grid_folder.Visible Then
                If IsNothing(dgv_details.CurrentCell) Then Exit Sub
                With dgv_details
                    If Val(cbo_grid_folder.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = 8 Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(8).Value = Trim(cbo_grid_folder.Text)
                    End If
                End With
            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub


    Private Sub btn_get_LotDetails_Click(sender As Object, e As EventArgs) Handles btn_get_LotDetails.Click
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim NewCode As String = ""
        Dim n As Integer
        Dim sno As Integer
        Dim vLOTNO As String = ""
        Dim vEntry_Date As String = ""
        Dim vAllotment_Date As String = ""
        Dim vChk_Sec As String = ""
        Dim vRef_Sec As String = ""

        If Trim(cbo_Lotcode_Selection.Text) <> "" Then

            vLOTNO = Trim(cbo_Lotcode_Selection.Text)
            vEntry_Date = msk_date.Text
            vAllotment_Date = msk_AllottedDate.Text
            vChk_Sec = Trim(cbo_Checking_Section.Text)
            vRef_Sec = Trim(lbl_RefNo.Text)

            clear()

            msk_date.Text = Trim(vEntry_Date)
            msk_AllottedDate.Text = Trim(vAllotment_Date)
            cbo_Checking_Section.Text = Trim(vChk_Sec)
            lbl_RefNo.Text = Val(vRef_Sec)

            da2 = New SqlClient.SqlDataAdapter("Select a.*, b.Cloth_name from lot_Approved_Head a INNER JOIN Cloth_Head b ON a.Cloth_idno = b.Cloth_idno where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Lotcode_ForSelection = '" & Trim(vLOTNO) & "'", con)
            dt1 = New DataTable
            da2.Fill(dt1)
            If dt1.Rows.Count > 0 Then

                cbo_Lotcode_Selection.Text = Trim(vLOTNO)
                lbl_ClothName.Text = dt1.Rows(0).Item("Cloth_name").ToString
                txt_total_pcs.Text = dt1.Rows(0).Item("noof_pcs").ToString
                txt_receipt_mtrs.Text = dt1.Rows(0).Item("Receipt_Meters").ToString


                da2 = New SqlClient.SqlDataAdapter("Select a.Weaver_ClothReceipt_No, a.Weaver_ClothReceipt_Code , b.Cloth_name, c.ledger_name as weavername from Weaver_Cloth_Receipt_head a INNER JOIN Cloth_Head b ON a.Cloth_idno = b.Cloth_idno   INNER JOIN ledger_Head c ON a.ledger_idno = c.ledger_idno  where a.lotcode_forSelection = '" & Trim(cbo_Lotcode_Selection.Text) & "'", con)
                dt2 = New DataTable
                da2.Fill(dt2)
                If dt2.Rows.Count > 0 Then
                    lbl_WeaverName.Text = dt2.Rows(0).Item("weavername").ToString
                    txt_lot_no.Text = dt2.Rows(0).Item("Weaver_ClothReceipt_No").ToString
                    txt_lot_code.Text = dt2.Rows(0).Item("Weaver_ClothReceipt_Code").ToString
                Else
                    lbl_WeaverName.Text = ""
                    txt_lot_no.Text = ""
                    txt_lot_code.Text = ""
                End If
                dt2.Clear()

                da2 = New SqlClient.SqlDataAdapter("Select a.*  from LotAllotment_Head a Where a.Lotcode_ForSelection = '" & Trim(cbo_Lotcode_Selection.Text) & "' ", con)
                dt2 = New DataTable
                da2.Fill(dt2)
                If dt2.Rows.Count > 0 Then
                    lbl_RefNo.Text = dt2.Rows(0).Item("LotAllot_RefNo").ToString
                End If
                dt2.Clear()



            Else

                lbl_ClothName.Text = ""
                txt_total_pcs.Text = ""
                txt_receipt_mtrs.Text = ""
                lbl_WeaverName.Text = ""
                txt_lot_no.Text = ""
                txt_lot_code.Text = ""

            End If
            dt1.Clear()

            da2 = New SqlClient.SqlDataAdapter("select a.* from  lot_allotment_details a where a.Lotcode_ForSelection = '" & Trim(cbo_Lotcode_Selection.Text) & "' ", con)
            dt2 = New DataTable
            da2.Fill(dt2)

            dgv_details.Rows.Clear()
            sno = 0
            With dgv_details

                .Rows.Clear()
                sno = 0

                If dt2.Rows.Count > 0 Then

                    cbo_checker.Text = Common_Procedures.Employee_Simple_IdNoToName(con, Val(dt2.Rows(0).Item("Checker_Idno_IRwages").ToString))
                    cbo_folder.Text = Common_Procedures.Employee_Simple_IdNoToName(con, Val(dt2.Rows(0).Item("Folder_Idno_IRwages").ToString))
                    For i = 0 To dt2.Rows.Count - 1

                        n = .Rows.Add()

                        sno = sno + 1

                        .Rows(n).Cells(0).Value = Val(sno)
                        .Rows(n).Cells(1).Value = Common_Procedures.Checking_TableNo_IdNoToName(con, dt2.Rows(i).Item("Checking_Table_IdNo").ToString)
                        .Rows(n).Cells(2).Value = dt2.Rows(i).Item("No_of_pcs").ToString

                        .Rows(n).Cells(3).Value = Common_Procedures.Employee_Simple_IdNoToName(con, dt2.Rows(i).Item("Lot_Supervisor_idno").ToString)
                        'dt4.Rows(i).Item("Lot_Supervisor_idno").ToString
                        .Rows(n).Cells(4).Value = Common_Procedures.Employee_Simple_IdNoToName(con, dt2.Rows(i).Item("Lot_Checker_idno").ToString)
                        'dt4.Rows(i).Item("Lot_Checker_idno").ToString
                        .Rows(n).Cells(8).Value = Common_Procedures.Employee_Simple_IdNoToName(con, dt2.Rows(i).Item("Lot_Folder_Idno").ToString)

                    Next i

                    Total_Calculation()
                End If

            End With

            dt2.Clear()

            dt2.Dispose()
            da2.Dispose()

        End If


        cbo_checker.Focus()

    End Sub

    Private Sub dtp_Date_KeyDown(sender As Object, e As KeyEventArgs) Handles dtp_Date.KeyDown

        If e.KeyCode = 40 Then
            e.Handled = True : e.SuppressKeyPress = True
            msk_date.Focus()
        End If

        If e.KeyCode = 38 Then
            e.Handled = True : e.SuppressKeyPress = True
            msk_date.Focus()
        End If
    End Sub

    Private Sub dtp_Date_KeyPress(sender As Object, e As KeyPressEventArgs) Handles dtp_Date.KeyPress
        If Asc(e.KeyChar) = 13 Then
            e.Handled = True
            msk_date.Focus()
        End If
    End Sub

    Private Sub dtp_Date_KeyUp(sender As Object, e As KeyEventArgs) Handles dtp_Date.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            dtp_Date.Text = Date.Today
        End If
    End Sub

    Private Sub dtp_Date_TextChanged(sender As Object, e As EventArgs) Handles dtp_Date.TextChanged
        If IsDate(dtp_Date.Text) = True Then
            msk_date.Text = dtp_Date.Text
            msk_date.SelectionStart = 0
        End If

    End Sub

    Private Sub msk_date_KeyDown(sender As Object, e As KeyEventArgs) Handles msk_date.KeyDown
        vcbo_KeyDwnVal = e.KeyValue

        If e.KeyCode = 40 Then
            e.Handled = True : e.SuppressKeyPress = True
            msk_AllottedDate.Focus()
        End If

        If e.KeyCode = 38 Then
            e.Handled = True : e.SuppressKeyPress = True

            cbo_folder.Focus()

        End If

        vmskOldText = ""
        vmskSelStrt = -1
        If e.KeyCode = 46 Or e.KeyCode = 8 Then
            vmskOldText = msk_date.Text
            vmskSelStrt = msk_date.SelectionStart
        End If

    End Sub

    Private Sub msk_date_KeyPress(sender As Object, e As KeyPressEventArgs) Handles msk_date.KeyPress
        If Trim(UCase(e.KeyChar)) = "D" Then
            msk_date.Text = Date.Today
            msk_date.SelectionStart = 0
        End If
        If Asc(e.KeyChar) = 13 Then
            e.Handled = True

            msk_AllottedDate.Focus()

        End If
    End Sub

    Private Sub msk_date_KeyUp(sender As Object, e As KeyEventArgs) Handles msk_date.KeyUp
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

    Private Sub msk_date_LostFocus(sender As Object, e As EventArgs) Handles msk_date.LostFocus

        If IsDate(msk_date.Text) = True Then
            If Microsoft.VisualBasic.DateAndTime.Day(Convert.ToDateTime(msk_date.Text)) <= 31 Or Microsoft.VisualBasic.DateAndTime.Month(Convert.ToDateTime(msk_date.Text)) <= 31 Then
                If Microsoft.VisualBasic.DateAndTime.Year(Convert.ToDateTime(msk_date.Text)) <= 2050 And Microsoft.VisualBasic.DateAndTime.Year(Convert.ToDateTime(msk_date.Text)) >= 2000 Then
                    dtp_Date.Value = Convert.ToDateTime(msk_date.Text)
                End If
            End If

        End If
    End Sub

    Private Sub msk_AllottedDate_KeyDown(sender As Object, e As KeyEventArgs) Handles msk_AllottedDate.KeyDown
        vcbo_KeyDwnVal = e.KeyValue

        If e.KeyCode = 40 Then
            e.Handled = True : e.SuppressKeyPress = True
            'cbo_Lotcode_Selection.Focus()

            cbo_Checking_Section.Focus()

        End If

        If e.KeyCode = 38 Then
            e.Handled = True : e.SuppressKeyPress = True

            msk_date.Focus()

        End If

        vmskOldText = ""
        vmskSelStrt = -1
        If e.KeyCode = 46 Or e.KeyCode = 8 Then
            vmskOldText = msk_AllottedDate.Text
            vmskSelStrt = msk_AllottedDate.SelectionStart
        End If
    End Sub

    Private Sub msk_AllottedDate_KeyPress(sender As Object, e As KeyPressEventArgs) Handles msk_AllottedDate.KeyPress
        If Trim(UCase(e.KeyChar)) = "D" Then
            msk_AllottedDate.Text = Date.Today
            msk_AllottedDate.SelectionStart = 0
        End If
        If Asc(e.KeyChar) = 13 Then
            e.Handled = True
            cbo_Checking_Section.Focus()
            'cbo_Lotcode_Selection.Focus()

        End If
    End Sub

    Private Sub msk_AllottedDate_KeyUp(sender As Object, e As KeyEventArgs) Handles msk_AllottedDate.KeyUp
        Dim vmRetTxt As String = ""
        Dim vmRetSelStrt As Integer = -1

        If e.KeyCode = 107 Then
            msk_AllottedDate.Text = DateAdd("D", 1, Convert.ToDateTime(msk_AllottedDate.Text))
            msk_AllottedDate.SelectionStart = 0
        ElseIf e.KeyCode = 109 Then
            msk_AllottedDate.Text = DateAdd("D", -1, Convert.ToDateTime(msk_AllottedDate.Text))
            msk_AllottedDate.SelectionStart = 0
        End If

        If e.KeyCode = 46 Or e.KeyCode = 8 Then
            Common_Procedures.maskEdit_Date_ON_DelBackSpace(sender, e, vmskOldText, vmskSelStrt)
        End If
    End Sub

    Private Sub msk_AllottedDate_LostFocus(sender As Object, e As EventArgs) Handles msk_AllottedDate.LostFocus

        If IsDate(msk_AllottedDate.Text) = True Then
            If Microsoft.VisualBasic.DateAndTime.Day(Convert.ToDateTime(msk_AllottedDate.Text)) <= 31 Or Microsoft.VisualBasic.DateAndTime.Month(Convert.ToDateTime(msk_AllottedDate.Text)) <= 31 Then
                If Microsoft.VisualBasic.DateAndTime.Year(Convert.ToDateTime(msk_AllottedDate.Text)) <= 2050 And Microsoft.VisualBasic.DateAndTime.Year(Convert.ToDateTime(msk_AllottedDate.Text)) >= 2000 Then
                    dtp_AllottedDate.Value = Convert.ToDateTime(msk_AllottedDate.Text)
                End If
            End If

        End If
    End Sub

    Private Sub dtp_AllottedDate_KeyDown(sender As Object, e As KeyEventArgs) Handles dtp_AllottedDate.KeyDown
        If e.KeyCode = 40 Then
            e.Handled = True : e.SuppressKeyPress = True
            cbo_Lotcode_Selection.Focus()
        End If

        If e.KeyCode = 38 Then
            e.Handled = True : e.SuppressKeyPress = True
            msk_AllottedDate.Focus()
        End If
    End Sub

    Private Sub dtp_AllottedDate_KeyPress(sender As Object, e As KeyPressEventArgs) Handles dtp_AllottedDate.KeyPress
        If Asc(e.KeyChar) = 13 Then
            e.Handled = True
            cbo_Lotcode_Selection.Focus()
        End If
    End Sub

    Private Sub dtp_AllottedDate_KeyUp(sender As Object, e As KeyEventArgs) Handles dtp_AllottedDate.KeyUp

        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            dtp_AllottedDate.Text = Date.Today
        End If

    End Sub

    Private Sub dtp_AllottedDate_TextChanged(sender As Object, e As EventArgs) Handles dtp_AllottedDate.TextChanged
        If IsDate(dtp_AllottedDate.Text) = True Then
            msk_AllottedDate.Text = dtp_AllottedDate.Text
            msk_AllottedDate.SelectionStart = 0
        End If

    End Sub


    Private Sub cbo_Checking_Section_KeyPress(sender As Object, e As KeyPressEventArgs) Handles cbo_Checking_Section.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Checking_Section, Nothing, "Checking_Section_Head", "Checking_Section_Name", "", "Checking_Section_Name")


        If Asc(e.KeyChar) = 13 Then

            cbo_Lotcode_Selection.Focus()

            'btn_get_LotDetails_Click(sender, e)

        End If



    End Sub

    Private Sub cbo_Checking_Section_KeyDown(sender As Object, e As KeyEventArgs) Handles cbo_Checking_Section.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Checking_Section, cbo_Lotcode_Selection, Nothing, "Checking_Section_Head", "Checking_Section_Name", "", "Checking_Section_Name")

        If (e.KeyValue = 38 And cbo_Checking_Section.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
            msk_AllottedDate.Focus()
        End If

        If (e.KeyValue = 40 And cbo_Checking_Section.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then

            cbo_Lotcode_Selection.Focus()

        End If

    End Sub

    Private Sub cbo_Checking_Section_GotFocus(sender As Object, e As EventArgs) Handles cbo_Checking_Section.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Checking_Section_Head", "Checking_Section_Name", "", "Checking_Section_Name")
    End Sub

    Private Sub cbo_Checking_Section_KeyUp(sender As Object, e As KeyEventArgs) Handles cbo_Checking_Section.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then

            Dim f As New Checking_Section_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Checking_Section.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If
    End Sub

    Private Sub cbo_checker_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbo_checker.SelectedIndexChanged

    End Sub

    Private Sub cbo_Checking_Section_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbo_Checking_Section.SelectedIndexChanged

    End Sub

    Private Sub cbo_Lotcode_Selection_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbo_Lotcode_Selection.SelectedIndexChanged

    End Sub

End Class