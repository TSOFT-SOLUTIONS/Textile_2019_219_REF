Public Class Sort_Change_Entry
    Implements Interface_MDIActions

    Private con As New SqlClient.SqlConnection(Common_Procedures.Connection_String)
    Private con1 As New SqlClient.SqlConnection(Common_Procedures.ConnectionString_CompanyGroupdetails)
    Private FrmLdSTS As Boolean = False
    Private New_Entry As Boolean = False
    Private Insert_Entry As Boolean = False
    Private Filter_Status As Boolean = False
    Private Pk_Condition As String = "SORCH-"
    Private Prec_ActCtrl As New Control
    Private vcbo_KeyDwnVal As Double
    Public vmskOldText As String = ""
    Public vmskSelStrt As Integer = -1

    Public Sub New()
        ' This call is required by the designer.
        InitializeComponent()
        ' Add any initialization after the InitializeComponent() call.
    End Sub

    Private Sub clear()
        New_Entry = False
        Insert_Entry = False

        pnl_back.Enabled = True
        pnl_filter.Visible = False
        pnl_Selection.Visible = False

        lbl_RefNo.Text = ""
        lbl_RefNo.ForeColor = Color.Black
        msk_Date.Text = ""
        dtp_Date.Text = ""
        cbo_Shift.Text = Common_Procedures.Shift_IdNoToName(con, 1)
        lbl_PartyName.Text = ""
        lbl_PartyName.Tag = ""
        cbo_ClothName1.Text = ""
        cbo_ClothName1.Tag = ""
        cbo_ClothName2.Text = ""
        cbo_ClothName2.Tag = ""
        cbo_ClothName3.Text = ""
        cbo_ClothName3.Tag = ""
        cbo_ClothName4.Text = ""
        cbo_ClothName4.Tag = ""

        lbl_Cloth_Name1.Text = ""
        lbl_Cloth_Name2.Text = ""
        lbl_Cloth_Name3.Text = ""
        lbl_Cloth_Name4.Text = ""

        lbl_WidthType.Text = ""

        lbl_KnotCode.Text = ""
        lbl_KnotNo.Text = ""
        lbl_LastSortCode.Text = ""
        lbl_LastSortNo.Text = ""

        lbl_UserName.Text = "USER : " & Trim(UCase(Common_Procedures.User_IdNoToName(con1, Common_Procedures.User.IdNo)))
        cbo_KnotterName.Text = ""
        txt_Wages.Text = ""

        lbl_EndsCount.Text = ""

        cbo_KnotterName.Text = ""
        cbo_WidthType.Text = ""

        lbl_SetNo1.Text = ""
        lbl_Meters1.Text = ""
        lbl_Meters2.Text = ""
        lbl_SetNo2.Text = ""
        cbo_LoomNo.Text = ""
        cbo_LoomNo.Tag = ""
        lbl_BeamNo1.Text = ""
        lbl_BeamNo2.Text = ""



        cbo_ClothName1.Enabled = True
        cbo_ClothName1.BackColor = Color.White

        cbo_ClothName2.Enabled = True
        cbo_ClothName2.BackColor = Color.White

        cbo_ClothName3.Enabled = True
        cbo_ClothName3.BackColor = Color.White

        cbo_ClothName4.Enabled = True
        cbo_ClothName4.BackColor = Color.White


        cbo_LoomNo.Enabled = True
        cbo_LoomNo.BackColor = Color.White

        btn_Selection.Enabled = True

    End Sub

    Private Sub ControlGotFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim txtbx As TextBox
        Dim combobx As ComboBox
        Dim msktxbx As MaskedTextBox
        On Error Resume Next

        If TypeOf Me.ActiveControl Is TextBox Or TypeOf Me.ActiveControl Is ComboBox Or TypeOf Me.ActiveControl Is Button Or TypeOf Me.ActiveControl Is MaskedTextBox Then
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
        dgv_filter.CurrentCell.Selected = False
    End Sub

    Private Sub move_record(ByVal no As String)

        Dim da1 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim NewCode As String
        Dim LockSTS As Boolean = False

        If Val(no) = 0 Then Exit Sub

        clear()

        Try

            NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(no) & "/" & Trim(Common_Procedures.FnYearCode)

            da1 = New SqlClient.SqlDataAdapter("select a.*, b.Ledger_Name, c.Cloth_Name, e.Cloth_Name as Cloth_Name2, f.Cloth_Name as Cloth_Name3, g.Cloth_Name as Cloth_Name4, d.Loom_Name from Sort_Change_Head a INNER JOIN Ledger_Head b ON a.Ledger_IdNo = b.Ledger_IdNo INNER JOIN Cloth_Head c ON a.Display_Cloth_Idno1 = c.Cloth_IdNo LEFT OUTER JOIN Cloth_Head e ON a.Display_Cloth_Idno2 = e.Cloth_IdNo LEFT OUTER JOIN Cloth_Head f ON a.Display_Cloth_Idno3 = f.Cloth_IdNo LEFT OUTER JOIN Cloth_Head g ON a.Display_Cloth_Idno4 = g.Cloth_IdNo LEFT OUTER JOIN Loom_Head d ON a.Loom_IdNo = d.Loom_IdNo Where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Sort_Change_Code = '" & Trim(NewCode) & "'", con)
            dt1 = New DataTable
            da1.Fill(dt1)

            If dt1.Rows.Count > 0 Then

                lbl_RefNo.Text = dt1.Rows(0).Item("Sort_Change_No").ToString
                dtp_Date.Text = dt1.Rows(0).Item("Sort_Change_Date").ToString
                msk_Date.Text = dtp_Date.Text
                cbo_Shift.Text = dt1.Rows(0).Item("Shift").ToString
                lbl_PartyName.Text = dt1.Rows(0).Item("Ledger_Name").ToString
                cbo_ClothName1.Text = dt1.Rows(0).Item("Cloth_Name").ToString
                If dt1.Rows(0).Item("Cloth_Name2").ToString <> "" Then
                    cbo_ClothName2.Text = dt1.Rows(0).Item("Cloth_Name2").ToString
                End If
                If dt1.Rows(0).Item("Cloth_Name3").ToString <> "" Then
                    cbo_ClothName3.Text = dt1.Rows(0).Item("Cloth_Name3").ToString
                End If
                If dt1.Rows(0).Item("Cloth_Name4").ToString <> "" Then
                    cbo_ClothName4.Text = dt1.Rows(0).Item("Cloth_Name4").ToString
                End If

                lbl_Cloth_Name1.Text = Common_Procedures.Cloth_IdNoToName(con, Val(dt1.Rows(0).Item("Cloth_Idno1").ToString))
                lbl_Cloth_Name2.Text = Common_Procedures.Cloth_IdNoToName(con, Val(dt1.Rows(0).Item("Cloth_Idno2").ToString))
                lbl_Cloth_Name3.Text = Common_Procedures.Cloth_IdNoToName(con, Val(dt1.Rows(0).Item("Cloth_Idno3").ToString))
                lbl_Cloth_Name4.Text = Common_Procedures.Cloth_IdNoToName(con, Val(dt1.Rows(0).Item("Cloth_Idno4").ToString))

                lbl_EndsCount.Text = Common_Procedures.EndsCount_IdNoToName(con, dt1.Rows(0).Item("EndsCount_IdNo").ToString)
                cbo_LoomNo.Text = dt1.Rows(0).Item("Loom_Name").ToString

                cbo_WidthType.Text = dt1.Rows(0).Item("Display_Width_Type").ToString

                lbl_WidthType.Text = dt1.Rows(0).Item("Width_Type").ToString

                lbl_SetCode1.Text = dt1.Rows(0).Item("Set_Code1").ToString
                lbl_SetCode2.Text = dt1.Rows(0).Item("Set_Code2").ToString
                lbl_SetNo1.Text = dt1.Rows(0).Item("Set_No1").ToString
                lbl_SetNo2.Text = dt1.Rows(0).Item("Set_No2").ToString
                lbl_BeamNo1.Text = dt1.Rows(0).Item("Beam_No1").ToString
                lbl_BeamNo2.Text = dt1.Rows(0).Item("Beam_No2").ToString
                lbl_Meters1.Text = dt1.Rows(0).Item("Beam_Meters1").ToString
                cbo_KnotterName.Text = Common_Procedures.Employee_IdNoToName(con, (dt1.Rows(0).Item("Employee_IdNo").ToString))
                txt_Wages.Text = dt1.Rows(0).Item("Wages_Amount").ToString
                lbl_UserName.Text = "USER : " & Trim(UCase(Common_Procedures.User_IdNoToName(con1, Val(dt1.Rows(0).Item("User_IdNo").ToString))))
                If Val(lbl_Meters1.Text) = 0 Then
                    lbl_Meters1.Text = ""
                End If
                lbl_Meters2.Text = dt1.Rows(0).Item("Beam_Meters2").ToString
                If Val(lbl_Meters2.Text) = 0 Then
                    lbl_Meters2.Text = ""
                End If

                lbl_KnotCode.Text = dt1.Rows(0).Item("Beam_Knotting_Code").ToString
                lbl_KnotNo.Text = dt1.Rows(0).Item("Beam_Knotting_No").ToString
                lbl_LastSortCode.Text = dt1.Rows(0).Item("Last_Sort_Change_Code").ToString
                lbl_LastSortNo.Text = dt1.Rows(0).Item("Last_Sort_Change_No").ToString

                'LockSTS = False
                'If IsDBNull(dt1.Rows(0).Item("Production_Meters").ToString) = False Then
                '    If Val(dt1.Rows(0).Item("Production_Meters").ToString) <> 0 Then
                '        LockSTS = True
                '    End If
                'End If

                'If IsDBNull(dt1.Rows(0).Item("Beam_RunOut_Code").ToString) = False Then
                '    If Trim(dt1.Rows(0).Item("Beam_RunOut_Code").ToString) <> "" Then
                '        LockSTS = True
                '    End If
                'End If

                If LockSTS = True Then

                    cbo_LoomNo.Enabled = False
                    cbo_LoomNo.BackColor = Color.LightGray

                    cbo_ClothName1.Enabled = False
                    cbo_ClothName1.BackColor = Color.LightGray

                    cbo_ClothName2.Enabled = False
                    cbo_ClothName2.BackColor = Color.LightGray

                    cbo_ClothName3.Enabled = False
                    cbo_ClothName3.BackColor = Color.LightGray

                    cbo_ClothName4.Enabled = False
                    cbo_ClothName4.BackColor = Color.LightGray

                    btn_Selection.Enabled = False

                End If

            End If

            dt1.Dispose()
            da1.Dispose()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally
            da1.Dispose()
            dt1.Dispose()

            If msk_Date.Visible And msk_Date.Enabled Then msk_Date.Focus()

        End Try

    End Sub

    Private Sub Empty_BeamBagCone_Delivery_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated

        Try


            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_ClothName1.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "CLOTH" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_ClothName1.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If
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

    Private Sub Sort_Change_Entry_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim Dt2 As New DataTable
        Dim dt3 As New DataTable
        Dim dt4 As New DataTable
        Dim dt5 As New DataTable
        Dim dt6 As New DataTable
        Me.Text = ""

        con.Open()

        Da = New SqlClient.SqlDataAdapter("select Loom_Name from Loom_head order by Loom_Name", con)
        Da.Fill(Dt2)
        cbo_LoomNo.DataSource = Dt2
        cbo_LoomNo.DisplayMember = "Loom_Name"

        'Da = New SqlClient.SqlDataAdapter("select Cloth_Name from Cloth_Head order by Cloth_Name", con)
        'Da.Fill(dt3)
        'cbo_ClothName1.DataSource = dt3
        'cbo_ClothName1.DisplayMember = "Cloth_Name"

        'Da = New SqlClient.SqlDataAdapter("select distinct(Knotter_Name) from Sort_Change_Head order by Knotter_Name", con)
        'Da.Fill(dt5)
        'cbo_KnotterName.DataSource = dt5
        'cbo_KnotterName.DisplayMember = "Knotter_Name"

        Da = New SqlClient.SqlDataAdapter("select Shift_Name from Shift_Head order by Shift_Name", con)
        Da.Fill(dt6)
        cbo_Shift.DataSource = dt6
        cbo_Shift.DisplayMember = "Shift_Name"

        'cbo_Shift.Items.Clear()
        'cbo_Shift.Items.Add("1 - 1st SHIFT")
        'cbo_Shift.Items.Add("2 - 2nd SHIFT")
        'cbo_Shift.Items.Add("3 - 3rd SHIFT")


        cbo_WidthType.Items.Clear()
        cbo_WidthType.Items.Add("SINGLE")
        cbo_WidthType.Items.Add("DOUBLE")
        cbo_WidthType.Items.Add("TRIPLE")
        cbo_WidthType.Items.Add("FOURTH")

        pnl_filter.Visible = False
        pnl_filter.Left = (Me.Width - pnl_filter.Width) \ 2
        pnl_filter.Top = ((Me.Height - pnl_filter.Height) \ 2) + 20
        btn_UserModification.Visible = False
        If Common_Procedures.settings.User_Modifications_Show_Status = 1 Then
            If Val(Common_Procedures.User.IdNo) = 1 Or Common_Procedures.User.Show_UserModification_Status = 1 Then
                btn_UserModification.Visible = True
            End If
        End If

        AddHandler msk_Date.GotFocus, AddressOf ControlGotFocus
        AddHandler dtp_date.GotFocus, AddressOf ControlGotFocus
        AddHandler lbl_PartyName.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Shift.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_ClothName1.GotFocus, AddressOf ControlGotFocus

        AddHandler cbo_ClothName2.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_ClothName3.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_ClothName4.GotFocus, AddressOf ControlGotFocus
        AddHandler lbl_EndsCount.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_LoomNo.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_WidthType.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_KnotterName.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Filter_ClothName.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Filter_PartyName.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Filter_LoomNo.GotFocus, AddressOf ControlGotFocus

        AddHandler cbo_KnotterName.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Wages.GotFocus, AddressOf ControlGotFocus

        AddHandler msk_Date.LostFocus, AddressOf ControlLostFocus
        AddHandler dtp_date.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Shift.LostFocus, AddressOf ControlLostFocus
        AddHandler lbl_PartyName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_ClothName1.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_ClothName2.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_ClothName3.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_ClothName4.LostFocus, AddressOf ControlLostFocus

        AddHandler lbl_EndsCount.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_LoomNo.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_WidthType.LostFocus, AddressOf ControlLostFocus
        ' AddHandler cbo_KnotterName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Filter_ClothName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Filter_PartyName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Filter_LoomNo.LostFocus, AddressOf ControlLostFocus

        AddHandler cbo_KnotterName.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Wages.LostFocus, AddressOf ControlLostFocus

        AddHandler dtp_date.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler dtp_FilterFrom_date.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler dtp_FilterTo_date.KeyDown, AddressOf TextBoxControlKeyDown
        ' AddHandler txt_filterpono.KeyDown, AddressOf TextBoxControlKeyDown


        AddHandler dtp_date.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler dtp_FilterFrom_date.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler dtp_FilterTo_date.KeyPress, AddressOf TextBoxControlKeyPress
        'AddHandler txt_filterpono.KeyPress, AddressOf TextBoxControlKeyPress


        pnl_Selection.Visible = False
        pnl_Selection.Left = (Me.Width - pnl_Selection.Width) \ 2
        pnl_Selection.Top = (Me.Height - pnl_Selection.Height) \ 2


        lbl_Company.Text = ""
        lbl_Company.Tag = 0
        lbl_Company.Visible = False
        Common_Procedures.CompIdNo = 0

        dgv_filter.RowTemplate.Height = 27

        Filter_Status = False
        FrmLdSTS = True
        new_record()

    End Sub


    Private Sub Sort_Change_Entry_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        On Error Resume Next
        con.Close()
        con.Dispose()
        Common_Procedures.Last_Closed_FormName = Me.Name
        con.Dispose()
    End Sub

    Private Sub Sort_Change_Entry_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress

        Try

            If Asc(e.KeyChar) = 27 Then

                If pnl_filter.Visible = True Then
                    btn_closefilter_Click(sender, e)
                    Exit Sub

                ElseIf pnl_Selection.Visible = True Then
                    btn_Close_Selection_Click(sender, e)
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
            MessageBox.Show(ex.Message, "DOES NOT CLOSE...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Public Sub delete_record() Implements Interface_MDIActions.delete_record
        Dim cmd As New SqlClient.SqlCommand
        Dim tr As SqlClient.SqlTransaction
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim Dt2 As New DataTable
        Dim NewCode As String = ""
        Dim DelvSts As Integer = 0
        Dim Nr As Integer = 0
        Dim NoofKnotBmsInCD As Integer = 0
        Dim NoofKnotBmsInLom As Integer = 0
        Dim Lm_ID As Integer = 0
        Dim vOrdByNo As String = ""

        vOrdByNo = Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text)

        If MessageBox.Show("Do you want to Delete?", "FOR DELETION...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) <> Windows.Forms.DialogResult.Yes Then
            Exit Sub
        End If

        ' If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.Inhouse_Sort_Change_Entry, "~L~") = 0 And InStr(Common_Procedures.UR.Inhouse_Sort_Change_Entry, "~D~") = 0 Then MessageBox.Show("You have No Rights to Delete", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error) : Exit Sub
        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)




        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.DeletingEntry, Common_Procedures.UR.Inhouse_Sort_Change_Entry, New_Entry, Me, con, "Sort_Change_Head", "Sort_Change_Code", NewCode, "Sort_Change_Date", "(Sort_Change_Code = '" & Trim(NewCode) & "')") = False Then Exit Sub







       
        If New_Entry = True Then
            MessageBox.Show("This is New Entry", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If
        cmd.Connection = con

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Da = New SqlClient.SqlDataAdapter("select * from Sort_Change_Head Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Sort_Change_Code = '" & Trim(NewCode) & "'", con)
        Dt1 = New DataTable
        Da.Fill(Dt1)

        If Dt1.Rows.Count > 0 Then
            If IsDBNull(Dt1.Rows(0).Item("Last_Sort_Change_Code").ToString) = False Then
                If Trim(Dt1.Rows(0).Item("Last_Sort_Change_Code").ToString) <> "" Then
                    MessageBox.Show("Invalid : Already this knotting, was Sort Changed", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Exit Sub
                End If
            End If
        End If
        Dt1.Clear()

        Da = New SqlClient.SqlDataAdapter("select b.* from Sort_Change_Head a LEFT OUTER JOIN Beam_Knotting_Head b ON a.Beam_Knotting_Code = b.Beam_Knotting_Code  where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Sort_Change_Code = '" & Trim(NewCode) & "'", con)
        Dt2 = New DataTable
        Da.Fill(Dt2)
        If Dt2.Rows.Count > 0 Then
            If Dt2.Rows(0).Item("Beam_RunOut_Code").ToString <> "" Then
                MessageBox.Show("Invalid Editing : These Beams already runnot", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Exit Sub
            End If
        End If
        Dt2.Clear()

        tr = con.BeginTransaction

        Try


            cmd.Transaction = tr
            Call Common_Procedures.User_Modification_Updation(con, "HEAD", Me.Name, "DELETE", "Sort_Change_Head", "Sort_Change_Code", Val(lbl_Company.Tag), NewCode, lbl_RefNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, True, "", "", "Sort_Change_Code, Company_IdNo, for_OrderBy", tr)

            Nr = 0
            cmd.CommandText = "Update Beam_Knotting_Head Set Sort_Change_No = '' , Sort_Change_Code = '' , Width_Type = b.Width_Type ,  Cloth_Idno1 = b.Cloth_Idno1 , Cloth_Idno2 = b.Cloth_Idno2 ,Cloth_Idno3 = b.Cloth_Idno3 , Cloth_Idno4 = b.Cloth_Idno4 from Beam_Knotting_Head a LEFT OUTER JOIN Sort_Change_Head B ON a.Sort_Change_Code = b.Sort_Change_Code Where a.Sort_Change_Code = '" & Trim(NewCode) & "' and a.Beam_RunOut_Code = ''"
            Nr = cmd.ExecuteNonQuery()
            If Nr > 1 Then
                Throw New ApplicationException("Error on Beam Knotting Updation")
            End If

            Nr = 0
            cmd.CommandText = "Update Beam_Knotting_Head Set Width_Type = b.Display_Width_Type ,  Cloth_Idno1 = b.Display_Cloth_Idno1 , Cloth_Idno2 = b.Display_Cloth_Idno2 , Cloth_Idno3 = b.Display_Cloth_Idno3 , Cloth_Idno4 = b.Display_Cloth_Idno4 from Beam_Knotting_Head a LEFT OUTER JOIN Sort_Change_Head b ON a.Beam_Knotting_Code = b.Beam_Knotting_Code Where b.Last_Sort_Change_Code = '" & Trim(NewCode) & "' and a.Beam_RunOut_Code = ''"
            Nr = cmd.ExecuteNonQuery()
            If Nr > 1 Then
                Throw New ApplicationException("Error on Beam Knotting Updation")
            End If

            Nr = 0
            cmd.CommandText = "Update Sort_Change_Head Set Last_Sort_Change_No = '' , Last_Sort_Change_Code = '' from Sort_Change_Head a Where a.Last_Sort_Change_Code = '" & Trim(NewCode) & "'"
            Nr = cmd.ExecuteNonQuery()
            If Nr > 1 Then
                Throw New ApplicationException("Error on Beam Knotting Updation")
            End If


            If Common_Procedures.VoucherBill_Deletion(con, Trim(Pk_Condition) & Trim(NewCode), tr) = False Then
                Throw New ApplicationException("Error on Voucher Bill Deletion")
            End If
            Common_Procedures.Voucher_Deletion(con, Val(lbl_Company.Tag), Trim(Pk_Condition) & Trim(NewCode), tr)

            cmd.CommandText = "delete from Sort_Change_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Sort_Change_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()


            tr.Commit()

            new_record()

            MessageBox.Show("Deleted Sucessfully!!!", "FOR DELETION...", MessageBoxButtons.OK, MessageBoxIcon.Information)

        Catch ex As Exception

            tr.Rollback()
            MessageBox.Show(ex.Message, "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        If msk_Date.Visible And msk_Date.Enabled Then msk_Date.Focus()

    End Sub

    Public Sub filter_record() Implements Interface_MDIActions.filter_record
        If Filter_Status = False Then
            Dim da As New SqlClient.SqlDataAdapter
            Dim dt1 As New DataTable
            Dim dt2 As New DataTable
            Dim dt3 As New DataTable

            da = New SqlClient.SqlDataAdapter("select Ledger_DisplayName from Ledger_AlaisHead where (Ledger_IdNo = 0 or Ledger_Type = 'JOBWORKER') order by Ledger_DisplayName", con)
            da.Fill(dt1)
            cbo_Filter_PartyName.DataSource = dt1
            cbo_Filter_PartyName.DisplayMember = "Ledger_DisplayName"

            da = New SqlClient.SqlDataAdapter("select Loom_Name from Loom_head order by Loom_Name", con)
            da.Fill(dt2)
            cbo_Filter_LoomNo.DataSource = dt2
            cbo_Filter_LoomNo.DisplayMember = "Loom_Name"

            da = New SqlClient.SqlDataAdapter("select Cloth_Name from Cloth_Head order by Cloth_Name", con)
            da.Fill(dt3)
            cbo_Filter_ClothName.DataSource = dt3
            cbo_Filter_ClothName.DisplayMember = "Cloth_Name"

            dtp_FilterFrom_date.Text = Common_Procedures.Company_FromDate
            dtp_FilterTo_date.Text = Common_Procedures.Company_ToDate
            pnl_filter.Text = ""
            cbo_Filter_PartyName.SelectedIndex = -1
            cbo_Filter_ClothName.SelectedIndex = -1
            cbo_Filter_LoomNo.SelectedIndex = -1
            dgv_filter.Rows.Clear()

            da.Dispose()

        End If

        pnl_filter.Visible = True
        pnl_filter.Enabled = True
        pnl_filter.BringToFront()
        pnl_back.Enabled = False
        If dtp_FilterFrom_date.Enabled And dtp_FilterFrom_date.Visible Then dtp_FilterFrom_date.Focus()

    End Sub

    Public Sub insert_record() Implements Interface_MDIActions.insert_record
        Dim cmd As New SqlClient.SqlCommand
        Dim dr As SqlClient.SqlDataReader
        Dim movno As String, inpno As String
        Dim NewCode As String

        '  If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.Inhouse_Sort_Change_Entry, "~L~") = 0 And InStr(Common_Procedures.UR.Inhouse_Sort_Change_Entry, "~I~") = 0 Then MessageBox.Show("You have No Rights to Insert", "DOES NOT INSERT...", MessageBoxButtons.OK, MessageBoxIcon.Error) : Exit Sub

        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.InsertingEntry, Common_Procedures.UR.Inhouse_Sort_Change_Entry, New_Entry, Me) = False Then Exit Sub



        Try

            inpno = InputBox("Enter New Ref.No.", "FOR INSERTION...")

            NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

            cmd.Connection = con
            cmd.CommandText = "select Sort_Change_No from Sort_Change_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Sort_Change_Code = '" & Trim(NewCode) & "'"
            dr = cmd.ExecuteReader

            movno = ""
            If dr.HasRows Then
                If dr.Read Then
                    If IsDBNull(dr(0).ToString) = False Then
                        movno = dr(0).ToString
                    End If
                End If
            End If

            dr.Close()
            cmd.Dispose()

            If Val(movno) <> 0 Then
                move_record(movno)

            Else
                If Val(inpno) = 0 Then
                    MessageBox.Show("Invalid Ref.No", "DOES NOT INSERT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

                Else
                    new_record()
                    Insert_Entry = True
                    lbl_RefNo.Text = Trim(UCase(inpno))

                End If

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT FIND...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Public Sub movefirst_record() Implements Interface_MDIActions.movefirst_record
        Dim cmd As New SqlClient.SqlCommand
        Dim dr As SqlClient.SqlDataReader
        Dim movno As String

        Try

            cmd.Connection = con
            cmd.CommandText = "select top 1 Sort_Change_No from Sort_Change_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Sort_Change_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby, Sort_Change_No"
            dr = cmd.ExecuteReader

            movno = ""
            If dr.HasRows Then
                If dr.Read Then
                    If IsDBNull(dr(0).ToString) = False Then
                        movno = dr(0).ToString
                    End If
                End If
            End If

            dr.Close()

            If Val(movno) <> 0 Then move_record(movno)

        Catch ex As Exception

            MessageBox.Show(ex.Message, "FOR  MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Public Sub movelast_record() Implements Interface_MDIActions.movelast_record
        Dim cmd As New SqlClient.SqlCommand
        Dim dr As SqlClient.SqlDataReader
        Dim movno As String

        Try
            cmd.Connection = con
            cmd.CommandText = "select top 1 Sort_Change_No from Sort_Change_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Sort_Change_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, Sort_Change_No desc"
            dr = cmd.ExecuteReader

            movno = ""
            If dr.HasRows Then
                If dr.Read Then
                    If IsDBNull(dr(0).ToString) = False Then
                        movno = dr(0).ToString
                    End If
                End If
            End If

            dr.Close()

            If Val(movno) <> 0 Then move_record(movno)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR  MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Public Sub movenext_record() Implements Interface_MDIActions.movenext_record
        Dim cmd As New SqlClient.SqlCommand
        Dim dr As SqlClient.SqlDataReader
        Dim movno As String = ""
        Dim OrdByNo As Single = 0

        Try

            OrdByNo = Common_Procedures.OrderBy_CodeToValue(Trim(lbl_RefNo.Text))

            cmd.Connection = con
            cmd.CommandText = "select top 1 Sort_Change_No from Sort_Change_Head where for_orderby > " & Str(OrdByNo) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and Sort_Change_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby, Sort_Change_No"
            dr = cmd.ExecuteReader

            movno = ""
            If dr.HasRows Then
                If dr.Read Then
                    If IsDBNull(dr(0).ToString) = False Then
                        movno = dr(0).ToString
                    End If
                End If
            End If

            dr.Close()

            If Val(movno) <> 0 Then move_record(movno)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR  MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub

    Public Sub moveprevious_record() Implements Interface_MDIActions.moveprevious_record
        Dim cmd As New SqlClient.SqlCommand
        Dim dr As SqlClient.SqlDataReader
        Dim movno As String = ""
        Dim OrdByNo As Single = 0

        Try

            OrdByNo = Common_Procedures.OrderBy_CodeToValue(Trim(lbl_RefNo.Text))

            cmd.Connection = con
            cmd.CommandText = "select top 1 Sort_Change_No from Sort_Change_Head where for_orderby < " & Str(OrdByNo) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and  Sort_Change_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, Sort_Change_No desc"
            dr = cmd.ExecuteReader

            movno = ""
            If dr.HasRows Then
                If dr.Read Then
                    If IsDBNull(dr(0).ToString) = False Then
                        movno = dr(0).ToString
                    End If
                End If
            End If

            dr.Close()

            If Val(movno) <> 0 Then move_record(movno)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR  MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub

    Public Sub new_record() Implements Interface_MDIActions.new_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim dt1 As New DataTable
        Dim NewID As Integer = 0

        Try
            clear()

            New_Entry = True

            da = New SqlClient.SqlDataAdapter("select max(for_orderby) from Sort_Change_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Sort_Change_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' ", con)
            da.Fill(dt)

            NewID = 0
            If dt.Rows.Count > 0 Then
                If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                    NewID = Val(dt.Rows(0)(0).ToString)
                End If
            End If

            NewID = NewID + 1

            lbl_RefNo.Text = NewID
            lbl_RefNo.ForeColor = Color.Red



            msk_Date.Text = Date.Today.ToShortDateString
            da = New SqlClient.SqlDataAdapter("select top 1 * from Sort_Change_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Sort_Change_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, Sort_Change_No desc", con)
            dt1 = New DataTable
            da.Fill(dt1)
            If dt1.Rows.Count > 0 Then
                If Val(Common_Procedures.settings.PreviousEntryDate_ByDefault) = 1 Then '---- M.S Textiles (Tirupur)
                    If dt1.Rows(0).Item("Sort_Change_Date").ToString <> "" Then msk_Date.Text = dt1.Rows(0).Item("Sort_Change_Date").ToString
                End If
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
        Dim cmd As New SqlClient.SqlCommand
        Dim dr As SqlClient.SqlDataReader
        Dim movno As String, inpno As String
        Dim NewCode As String

        Try

            inpno = InputBox("Enter Ref.No", "FOR FINDING...")

            NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

            cmd.Connection = con
            cmd.CommandText = "select Sort_Change_No from Sort_Change_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Sort_Change_Code = '" & Trim(NewCode) & "'"
            dr = cmd.ExecuteReader

            movno = ""
            If dr.HasRows Then
                If dr.Read Then
                    If IsDBNull(dr(0).ToString) = False Then
                        movno = dr(0).ToString
                    End If
                End If
            End If

            dr.Close()
            cmd.Dispose()

            If Val(movno) <> 0 Then
                move_record(movno)

            Else
                MessageBox.Show("Ref.No. Does not exists", "DOES NOT FIND...", MessageBoxButtons.OK, MessageBoxIcon.Error)

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT FIND...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Public Sub save_record() Implements Interface_MDIActions.save_record
        Dim cmd As New SqlClient.SqlCommand
        Dim tr As SqlClient.SqlTransaction
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim NewCode As String = ""
        Dim nr As Long = 0
        Dim led_id As Integer = 0
        Dim Clo_ID As Integer = 0
        Dim Clo_ID2 As Integer = 0
        Dim Clo_ID3 As Integer = 0
        Dim Clo_ID4 As Integer = 0

        Dim Disp_Clo_ID As Integer = 0
        Dim Disp_Clo_ID2 As Integer = 0
        Dim Disp_Clo_ID3 As Integer = 0
        Dim Disp_Clo_ID4 As Integer = 0

        Dim EdsCnt_ID As Integer = 0
        Dim Cnt_ID As Integer = 0
        Dim Lm_ID As Integer = 0
        Dim NoofInpBmsInLom As Integer = 0
        Dim NoofKnotBmsInCD As Integer = 0
        Dim NoofKnotBmsInLom As Integer = 0
        Dim Emp_id As Integer = 0
        Dim CR_id As Integer = 0
        Dim DR_id As Integer = 0
        Dim vOrdByNo As String = ""

        vOrdByNo = Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text)

        If pnl_back.Enabled = False Then
            MessageBox.Show("Close all other windows", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If
        '  If Common_Procedures.UserRight_Check(Common_Procedures.UR.Inhouse_Sort_Change_Entry, New_Entry) = False Then Exit Sub

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)


        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.SavingEntry, Common_Procedures.UR.Inhouse_Sort_Change_Entry, New_Entry, Me, con, "Sort_Change_Head", "Sort_Change_Code", NewCode, "Sort_Change_Date", "(Sort_Change_Code = '" & Trim(NewCode) & "')", "(Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Sort_Change_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "')", "for_Orderby desc, Sort_Change_No desc", dtp_date.Value.Date) = False Then Exit Sub


      
        If Val(lbl_Company.Tag) = 0 Then
            MessageBox.Show("Invalid Company Selection", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        da = New SqlClient.SqlDataAdapter("select * from Sort_Change_Head Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Sort_Change_Code = '" & Trim(NewCode) & "'", con)
        dt1 = New DataTable
        da.Fill(dt1)

        If dt1.Rows.Count > 0 Then
            If IsDBNull(dt1.Rows(0).Item("Last_Sort_Change_Code").ToString) = False Then
                If Trim(dt1.Rows(0).Item("Last_Sort_Change_Code").ToString) <> "" Then
                    MessageBox.Show("Invalid : Already this Sort Change, was Sort Changed again", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                    Exit Sub
                End If
            End If
        End If
        dt1.Clear()

        If IsDate(msk_Date.Text) = False Then
            MessageBox.Show("Invalid Date", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If msk_Date.Enabled Then msk_Date.Focus()
            Exit Sub
        End If

        If Not (Convert.ToDateTime(msk_Date.Text) >= Common_Procedures.Company_FromDate And Convert.ToDateTime(msk_Date.Text) <= Common_Procedures.Company_ToDate) Then
            'If Not (dtp_date.Value.Date >= Common_Procedures.Company_FromDate And dtp_date.Value.Date <= Common_Procedures.Company_ToDate) Then
            MessageBox.Show("Date is out of financial range", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If dtp_date.Enabled Then dtp_date.Focus()
            Exit Sub
        End If

        If Trim(cbo_Shift.Text) = "" Then
            MessageBox.Show("Invalid Shift", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If cbo_Shift.Enabled Then cbo_Shift.Focus()
            Exit Sub
        End If

        led_id = Common_Procedures.Ledger_AlaisNameToIdNo(con, lbl_PartyName.Text)
        If led_id = 0 Then
            MessageBox.Show("Invalid Ledger Name", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If lbl_PartyName.Enabled Then lbl_PartyName.Focus()
            Exit Sub
        End If

        lbl_UserName.Text = Common_Procedures.User.IdNo

        Disp_Clo_ID = Common_Procedures.Cloth_NameToIdNo(con, cbo_ClothName1.Text)
        If Disp_Clo_ID = 0 Then
            MessageBox.Show("Invalid Cloth Name", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If cbo_ClothName1.Enabled Then cbo_ClothName1.Focus()
            Exit Sub
        End If

        Disp_Clo_ID2 = 0
        If Trim(UCase(cbo_WidthType.Text)) = "DOUBLE" Or Trim(UCase(cbo_WidthType.Text)) = "TRIPLE" Or Trim(UCase(cbo_WidthType.Text)) = "FOURTH" Then
            Disp_Clo_ID2 = Common_Procedures.Cloth_NameToIdNo(con, cbo_ClothName2.Text)
            If Disp_Clo_ID2 = 0 Then
                MessageBox.Show("Invalid Cloth Name", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                If cbo_ClothName2.Enabled Then cbo_ClothName2.Focus()
                Exit Sub
            End If
        End If

        Disp_Clo_ID3 = 0
        If Trim(UCase(cbo_WidthType.Text)) = "TRIPLE" Or Trim(UCase(cbo_WidthType.Text)) = "FOURTH" Then
            Disp_Clo_ID3 = Common_Procedures.Cloth_NameToIdNo(con, cbo_ClothName3.Text)
            If Disp_Clo_ID3 = 0 Then
                MessageBox.Show("Invalid Cloth Name", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                If cbo_ClothName3.Enabled Then cbo_ClothName3.Focus()
                Exit Sub
            End If
        End If

        Disp_Clo_ID4 = 0
        If Trim(UCase(cbo_WidthType.Text)) = "FOURTH" Then
            Disp_Clo_ID4 = Common_Procedures.Cloth_NameToIdNo(con, cbo_ClothName4.Text)
            If Disp_Clo_ID4 = 0 Then
                MessageBox.Show("Invalid Cloth Name", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                If cbo_ClothName4.Enabled Then cbo_ClothName4.Focus()
                Exit Sub
            End If
        End If

        Clo_ID = Common_Procedures.Cloth_NameToIdNo(con, lbl_Cloth_Name1.Text)

        Clo_ID2 = Common_Procedures.Cloth_NameToIdNo(con, lbl_Cloth_Name2.Text)

        Clo_ID3 = Common_Procedures.Cloth_NameToIdNo(con, lbl_Cloth_Name3.Text)

        Clo_ID4 = Common_Procedures.Cloth_NameToIdNo(con, lbl_Cloth_Name4.Text)


        EdsCnt_ID = Common_Procedures.EndsCount_NameToIdNo(con, lbl_EndsCount.Text)
        If EdsCnt_ID = 0 Then
            MessageBox.Show("Invalid Ends/Count", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If lbl_EndsCount.Enabled Then lbl_EndsCount.Focus()
            Exit Sub
        End If

        Lm_ID = Common_Procedures.Loom_NameToIdNo(con, cbo_LoomNo.Text)
        If Lm_ID = 0 Then
            MessageBox.Show("Invalid Loom No", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If cbo_LoomNo.Enabled Then cbo_LoomNo.Focus()
            Exit Sub
        End If

        If Trim(cbo_WidthType.Text) = "" Then
            MessageBox.Show("Invalid Width Type", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If cbo_WidthType.Enabled Then cbo_WidthType.Focus()
            Exit Sub
        End If

        Emp_id = Common_Procedures.Employee_NameToIdNo(con, cbo_KnotterName.Text)


        NoofInpBmsInLom = Common_Procedures.get_FieldValue(con, "Loom_Head", "Noof_Input_Beams", "(Loom_IdNo = " & Str(Val(Lm_ID)) & ")")
        If Val(NoofInpBmsInLom) = 0 Then NoofInpBmsInLom = 1

        If NoofInpBmsInLom = 1 Then
            If Trim(lbl_BeamNo1.Text) = "" Then
                MessageBox.Show("Invalid Beams", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                If cbo_LoomNo.Enabled Then cbo_LoomNo.Focus()
                Exit Sub
            End If

            If Val(lbl_Meters1.Text) = 0 Then
                MessageBox.Show("Invalid Beam Meters", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                If cbo_LoomNo.Enabled Then cbo_LoomNo.Focus()
                Exit Sub
            End If

            If Trim(lbl_BeamNo2.Text) <> "" Then
                MessageBox.Show("Invalid Beams, Select Only One Beam", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                If cbo_LoomNo.Enabled Then cbo_LoomNo.Focus()
                Exit Sub
            End If

            If Val(lbl_Meters2.Text) <> 0 Then
                MessageBox.Show("Invalid Beam Meters, Select Only One Beam", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                If cbo_LoomNo.Enabled Then cbo_LoomNo.Focus()
                Exit Sub
            End If

        Else

            If Trim(lbl_BeamNo1.Text) = "" Or Trim(lbl_BeamNo2.Text) = "" Then
                MessageBox.Show("Invalid Beams", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                If cbo_LoomNo.Enabled Then cbo_LoomNo.Focus()
                Exit Sub
            End If

            If Val(lbl_Meters1.Text) = 0 Or Val(lbl_Meters2.Text) = 0 Then
                MessageBox.Show("Invalid Beam Meters", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                If cbo_LoomNo.Enabled Then cbo_LoomNo.Focus()
                Exit Sub
            End If

        End If



        tr = con.BeginTransaction

        Try

            If Insert_Entry = True Or New_Entry = False Then
                NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            Else

                lbl_RefNo.Text = Common_Procedures.get_MaxCode(con, "Sort_Change_Head", "Sort_Change_Code", "for_OrderBy", "", Val(lbl_Company.Tag), Common_Procedures.FnYearCode, tr)

                NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            End If

            cmd.Connection = con
            cmd.Transaction = tr

            cmd.Parameters.Clear()
            cmd.Parameters.AddWithValue("@EntryDate", dtp_date.Value.Date)


            If New_Entry = True Then

                cmd.CommandText = "Insert into Sort_Change_Head   (     Sort_Change_Code,           Company_IdNo      ,        Sort_Change_No      ,                                for_OrderBy                             , Sort_Change_Date  ,               Shift           ,    Ledger_IdNo     ,       Cloth_Idno1  ,       Cloth_Idno2  ,      Cloth_Idno3    ,      Cloth_Idno4    ,    Display_Cloth_Idno1 ,     Display_Cloth_Idno2 ,      Display_Cloth_Idno3 ,      Display_Cloth_Idno4 ,            EndsCount_IdNo  ,       Loom_IdNo   ,              Width_Type           ,          Display_Width_Type        ,               Knotter_Name          ,            Set_Code1             ,             Set_No1            ,             Beam_No1            ,          Beam_Meters1        ,             Set_Code2            ,             Set_No2            ,           Beam_No2              ,           Beam_Meters2        ,     Employee_IdNo  ,       Wages_Amount         ,             User_idNo          ,           Beam_Knotting_No      ,          Beam_Knotting_Code       , Last_Sort_Change_No, Last_Sort_Change_Code ) " & _
                                        "      Values             ('" & Trim(NewCode) & "', " & Val(lbl_Company.Tag) & ", '" & Trim(lbl_RefNo.Text) & "', " & Val(Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text)) & ",     @EntryDate    , '" & Trim(cbo_Shift.Text) & "', " & Val(led_id) & ", " & Val(Clo_ID) & "," & Val(Clo_ID2) & ", " & Val(Clo_ID3) & ", " & Val(Clo_ID4) & "," & Val(Disp_Clo_ID) & "," & Val(Disp_Clo_ID2) & ", " & Val(Disp_Clo_ID3) & ", " & Val(Disp_Clo_ID4) & ", " & Str(Val(EdsCnt_ID)) & ", " & Val(Lm_ID) & ", '" & Trim(lbl_WidthType.Text) & "', '" & Trim(cbo_WidthType.Text) & "' , '" & Trim(cbo_KnotterName.Text) & "', '" & Trim(lbl_SetCode1.Text) & "', '" & Trim(lbl_SetNo1.Text) & "', '" & Trim(lbl_BeamNo1.Text) & "', " & Val(lbl_Meters1.Text) & ", '" & Trim(lbl_SetCode2.Text) & "', '" & Trim(lbl_SetNo2.Text) & "', '" & Trim(lbl_BeamNo2.Text) & "',  " & Val(lbl_Meters2.Text) & ", " & Val(Emp_id) & ", " & Val(txt_Wages.Text) & ", " & Val(lbl_UserName.Text) & " , '" & Trim(lbl_KnotNo.Text) & "' , '" & Trim(lbl_KnotCode.Text) & "' ,        ''          ,      ''               ) "
                cmd.ExecuteNonQuery()

            Else
                Call Common_Procedures.User_Modification_Updation(con, "HEAD", Me.Name, "OLD", "Sort_Change_Head", "Sort_Change_Code", Val(lbl_Company.Tag), NewCode, lbl_RefNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, False, "", "", "Sort_Change_Code, Company_IdNo, for_OrderBy", tr)

                nr = 0
                cmd.CommandText = "Update Beam_Knotting_Head Set Sort_Change_No = '' , Sort_Change_Code = '' , Width_Type = b.Width_Type  , Cloth_Idno1 = b.Cloth_Idno1 , Cloth_Idno2 = b.Cloth_Idno2 , Cloth_Idno3 = b.Cloth_Idno3 , Cloth_Idno4 = b.Cloth_Idno4 from Beam_Knotting_Head a INNER JOIN Sort_Change_Head b ON a.Beam_Knotting_Code = b.Beam_Knotting_Code and a.Sort_Change_Code = b.Sort_Change_Code Where a.Sort_Change_Code = '" & Trim(NewCode) & "' and a.Beam_RunOut_Code = ''"
                nr = cmd.ExecuteNonQuery()
                If nr > 1 Then
                    Throw New ApplicationException("Editing : Error on Beam Knotting Updation")
                    Exit Sub
                End If

                nr = 0
                cmd.CommandText = "Update Beam_Knotting_Head Set Width_Type = b.Display_Width_Type ,  Cloth_Idno1 = b.Display_Cloth_Idno1 , Cloth_Idno2 = b.Display_Cloth_Idno2 , Cloth_Idno3 = b.Display_Cloth_Idno3 , Cloth_Idno4 = b.Display_Cloth_Idno4 from Beam_Knotting_Head a LEFT OUTER JOIN Sort_Change_Head b ON a.Beam_Knotting_Code = b.Beam_Knotting_Code Where b.Last_Sort_Change_Code = '" & Trim(NewCode) & "' and a.Beam_RunOut_Code = ''"
                'cmd.CommandText = "Update Beam_Knotting_Head Set Width_Type = b.Display_Width_Type ,  Cloth_Idno1 = b.Display_Cloth_Idno1 , Cloth_Idno2 = b.Display_Cloth_Idno2 , Cloth_Idno3 = b.Display_Cloth_Idno3 , Cloth_Idno4 = b.Display_Cloth_Idno4 from Beam_Knotting_Head a LEFT OUTER JOIN Sort_Change_Head B ON b.Last_Sort_Change_Code = '" & Trim(NewCode) & "' Where b.Last_Sort_Change_Code = '" & Trim(NewCode) & "' and a.Beam_RunOut_Code = ''"
                nr = cmd.ExecuteNonQuery()
                If nr > 1 Then
                    Throw New ApplicationException("Editing : Error on Beam Knotting Updation")
                    Exit Sub
                End If

                nr = 0
                cmd.CommandText = "Update Sort_Change_Head Set Last_Sort_Change_No = '' , Last_Sort_Change_Code = '' from Sort_Change_Head a Where a.Last_Sort_Change_Code = '" & Trim(NewCode) & "'"
                nr = cmd.ExecuteNonQuery()
                If nr > 1 Then
                    Throw New ApplicationException("Editing : Error on Sort Change Updation")
                    Exit Sub
                End If

                cmd.CommandText = "Update Sort_Change_Head set Sort_Change_Date = @EntryDate, Shift = '" & Trim(cbo_Shift.Text) & "', Ledger_IdNo = " & Str(Val(led_id)) & ", Cloth_Idno1 = " & Str(Val(Clo_ID)) & ",  Cloth_Idno2 = " & Str(Val(Clo_ID2)) & ",  Cloth_Idno3 = " & Str(Val(Clo_ID3)) & ", Cloth_Idno4 = " & Str(Val(Clo_ID4)) & ", Display_Cloth_Idno1 = " & Str(Val(Disp_Clo_ID)) & ",  Display_Cloth_Idno2 = " & Str(Val(Disp_Clo_ID2)) & ",  Display_Cloth_Idno3 = " & Str(Val(Disp_Clo_ID3)) & ", Display_Cloth_Idno4 = " & Str(Val(Disp_Clo_ID4)) & ",  EndsCount_IdNo = " & Str(Val(EdsCnt_ID)) & ", Employee_IdNo = " & Str(Val(Emp_id)) & " , Wages_Amount = " & Str(Val(txt_Wages.Text)) & " ,  Loom_IdNo = " & Str(Val(Lm_ID)) & ", Display_Width_Type = '" & Trim(cbo_WidthType.Text) & "' , Width_Type = '" & Trim(lbl_WidthType.Text) & "', Knotter_Name = '" & Trim(cbo_KnotterName.Text) & "',  set_Code1 = '" & Trim(lbl_SetCode1.Text) & "', set_no1 = '" & Trim(lbl_SetNo1.Text) & "', Beam_No1 = '" & Trim(lbl_BeamNo1.Text) & "',  Beam_Knotting_No = '" & Trim(lbl_KnotNo.Text) & "' ,Beam_Knotting_Code = '" & Trim(lbl_KnotCode.Text) & "',  Beam_Meters1 = " & Str(Val(lbl_Meters1.Text)) & ", set_Code2 = '" & Trim(lbl_SetCode2.Text) & "', set_no2 = '" & Trim(lbl_SetNo2.Text) & "', Beam_No2 = '" & Trim(lbl_BeamNo2.Text) & "', Beam_Meters2 = " & Str(Val(lbl_Meters2.Text)) & ", User_idNo = " & Val(lbl_UserName.Text) & "  Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Sort_Change_Code = '" & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()

            End If
            Call Common_Procedures.User_Modification_Updation(con, "HEAD", Me.Name, "NEW", "Sort_Change_Head", "Sort_Change_Code", Val(lbl_Company.Tag), NewCode, lbl_RefNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, False, "", "", "Sort_Change_Code, Company_IdNo, for_OrderBy", tr)

            If Trim(lbl_LastSortCode.Text) = "" Then

                nr = 0
                cmd.CommandText = "Update Beam_Knotting_Head Set Sort_Change_No = '" & Trim(lbl_RefNo.Text) & "' , Sort_Change_Code = '" & Trim(NewCode) & "' , Width_Type = '" & Trim(cbo_WidthType.Text) & "' ,  Cloth_Idno1 = " & Str(Val(Disp_Clo_ID)) & ",  Cloth_Idno2 = " & Str(Val(Disp_Clo_ID2)) & ",  Cloth_Idno3 = " & Str(Val(Disp_Clo_ID3)) & ", Cloth_Idno4 = " & Str(Val(Disp_Clo_ID4)) & " Where Loom_Idno = " & Str(Lm_ID) & " and Beam_Knotting_Code = '" & Trim(lbl_KnotCode.Text) & "' and Beam_RunOut_Code = ''"
                nr = cmd.ExecuteNonQuery()
                If nr = 0 Then
                    Throw New ApplicationException("Saving : These Beams already runnot")
                    Exit Sub
                End If
                If nr > 1 Then
                    Throw New ApplicationException("Saving : Error on Beam Knotting Updation")
                    Exit Sub
                End If

            Else

                nr = 0
                cmd.CommandText = "Update Beam_Knotting_Head Set Width_Type = '" & Trim(cbo_WidthType.Text) & "' , Cloth_Idno1 = " & Str(Val(Disp_Clo_ID)) & ",  Cloth_Idno2 = " & Str(Val(Disp_Clo_ID2)) & ",  Cloth_Idno3 = " & Str(Val(Disp_Clo_ID3)) & ", Cloth_Idno4 = " & Str(Val(Disp_Clo_ID4)) & " Where Loom_Idno = " & Str(Lm_ID) & " and Beam_Knotting_Code = '" & Trim(lbl_KnotCode.Text) & "' and Beam_RunOut_Code = '' and Sort_Change_Code <> '' and Sort_Change_Code <> '" & Trim(NewCode) & "'"
                nr = cmd.ExecuteNonQuery()
                If nr = 0 Then
                    Throw New ApplicationException("Saving : These Beams already runnot")
                    Exit Sub
                End If
                If nr > 1 Then
                    Throw New ApplicationException("Saving : Error on Beam Knotting Updation")
                    Exit Sub
                End If

                nr = 0
                cmd.CommandText = "Update Sort_Change_Head Set Last_Sort_Change_No = '" & Trim(lbl_RefNo.Text) & "' , Last_Sort_Change_Code = '" & Trim(NewCode) & "' Where Loom_Idno = " & Str(Lm_ID) & " and Sort_Change_Code = '" & Trim(lbl_LastSortCode.Text) & "' and Beam_Knotting_Code = '" & Trim(lbl_KnotCode.Text) & "'"
                nr = cmd.ExecuteNonQuery()
                If nr = 0 Then
                    Throw New ApplicationException("Saving : These Beams already runnot")
                    Exit Sub
                End If
                If nr > 1 Then
                    Throw New ApplicationException("Saving : Error on Sort Change Updation")
                    Exit Sub
                End If

            End If

            tr.Commit()

          
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
            dt1.Dispose()
            da.Dispose()
            cmd.Dispose()
            tr.Dispose()

            If msk_Date.Enabled And msk_Date.Visible Then msk_Date.Focus()

        End Try

    End Sub

    Public Sub print_record() Implements Interface_MDIActions.print_record
        '------ No Print
    End Sub

    Private Sub cbo_ClothName1_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_ClothName1.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Cloth_Head", "Cloth_Name", "", "(Cloth_idno = 0)")
    End Sub

    Private Sub cbo_ClothName1_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_ClothName1.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_ClothName1, cbo_WidthType, Nothing, "Cloth_Head", "Cloth_Name", "", "(Cloth_idno = 0)")
        If (e.KeyValue = 40 And cbo_ClothName1.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
            If cbo_ClothName2.Enabled = True Then
                cbo_ClothName2.Focus()
            Else
                If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                    save_record()
                Else
                    dtp_date.Focus()
                End If
            End If
        End If
    End Sub

    Private Sub cbo_ClothName1_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_ClothName1.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_ClothName1, cbo_ClothName2, "Cloth_Head", "Cloth_Name", "", "(Cloth_idno = 0)")
    End Sub

    Private Sub cbo_ClothName2_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_ClothName2.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Cloth_Head", "Cloth_Name", "", "(Cloth_idno = 0)")
    End Sub

    Private Sub cbo_ClothName2_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_ClothName2.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_ClothName2, cbo_ClothName1, Nothing, "Cloth_Head", "Cloth_Name", "", "(Cloth_idno = 0)")
        If (e.KeyValue = 40 And cbo_ClothName2.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
            If cbo_ClothName3.Enabled = True Then
                cbo_ClothName3.Focus()
            Else
                If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                    save_record()
                Else
                    dtp_date.Focus()
                End If
            End If
        End If
    End Sub

    Private Sub cbo_ClothName2_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_ClothName2.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_ClothName2, Nothing, "Cloth_Head", "Cloth_Name", "", "(Cloth_idno = 0)")
        If Asc(e.KeyChar) = 13 Then
            If cbo_ClothName3.Enabled = True Then
                cbo_ClothName3.Focus()

            Else
                If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                    save_record()
                Else
                    dtp_date.Focus()
                End If

            End If
        End If
    End Sub

    Private Sub cbo_ClothName3_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_ClothName3.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Cloth_Head", "Cloth_Name", "", "(Cloth_idno = 0)")
    End Sub

    Private Sub cbo_ClothName3_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_ClothName3.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_ClothName3, cbo_ClothName2, Nothing, "Cloth_Head", "Cloth_Name", "", "(Cloth_idno = 0)")
        If (e.KeyValue = 40 And cbo_ClothName3.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
            If cbo_ClothName4.Enabled = True Then
                cbo_ClothName4.Focus()

            Else
                If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                    save_record()
                Else
                    dtp_date.Focus()
                End If

            End If
        End If
    End Sub

    Private Sub cbo_ClothName3_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_ClothName3.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_ClothName3, Nothing, "Cloth_Head", "Cloth_Name", "", "(Cloth_idno = 0)")
        If Asc(e.KeyChar) = 13 Then
            If cbo_ClothName4.Enabled = True Then
                cbo_ClothName4.Focus()

            Else
                If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                    save_record()
                Else
                    dtp_date.Focus()
                End If

            End If
        End If
    End Sub

    Private Sub cbo_ClothName4_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_ClothName4.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Cloth_Head", "Cloth_Name", "", "(Cloth_idno = 0)")
    End Sub

    Private Sub cbo_ClothName4_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_ClothName4.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_ClothName4, cbo_ClothName3, Nothing, "Cloth_Head", "Cloth_Name", "", "(Cloth_idno = 0)")
        If (e.KeyValue = 40 And cbo_ClothName4.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
            btn_save.Focus()
            If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                save_record()
            Else
                dtp_date.Focus()
            End If
        End If
    End Sub

    Private Sub cbo_ClothName4_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_ClothName4.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_ClothName4, Nothing, "Cloth_Head", "Cloth_Name", "", "(Cloth_idno = 0)")
        If Asc(e.KeyChar) = 13 Then
            btn_save.Focus()
            If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                save_record()
            Else
                dtp_date.Focus()
            End If
        End If
    End Sub

    Private Sub cbo_WidthType_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_WidthType.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_WidthType, cbo_LoomNo, cbo_ClothName1, "", "", "", "")
    End Sub

    Private Sub cbo_widthType_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_WidthType.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_WidthType, cbo_ClothName1, "", "", "", "")
        If Asc(e.KeyChar) = 13 Then
            cbo_WidthType_TextChanged(sender, e)
        End If
    End Sub

    Private Sub cbo_Shift_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Shift.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Shift_Head", "Shift_Name", "", "(Shift_IdNo)")
    End Sub

    Private Sub cbo_Shift_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Shift.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Shift, msk_Date, cbo_LoomNo, "Shift_Head", "Shift_Name", "", "(Shift_IdNo = 0)")
    End Sub

    Private Sub cbo_Shift_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Shift.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Shift, cbo_LoomNo, "Shift_Head", "Shift_Name", "", "(Shift_IdNo)")
    End Sub

    Private Sub cbo_KnotterName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_KnotterName.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "PayRoll_Employee_Head", "Employee_Name", "", "(Employee_IdNo = 0)")
    End Sub

    Private Sub cbo_KnotterName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_KnotterName.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_KnotterName, dtp_date, txt_Wages, "PayRoll_Employee_Head", "Employee_Name", "", "(Employee_IdNo = 0)")
        If (e.KeyValue = 38 And cbo_KnotterName.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
            If cbo_ClothName4.Enabled = True Then
                cbo_ClothName4.Focus()
            ElseIf cbo_ClothName3.Enabled = True Then
                cbo_ClothName3.Focus()
            ElseIf cbo_ClothName2.Enabled = True Then
                cbo_ClothName2.Focus()
            Else
                cbo_ClothName1.Focus()
            End If
        End If

    End Sub

    Private Sub cbo_KnotterName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_KnotterName.KeyPress
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim New_Rate As Double = 0
        Dim Emp_idno As String

        Emp_idno = Common_Procedures.Employee_NameToIdNo(con, Trim(cbo_KnotterName.Text))

        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_KnotterName, txt_Wages, "PayRoll_Employee_Head", "Employee_Name", "", "(Employee_IdNo = 0)")
        If Asc(e.KeyChar) = 13 Then
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

        End If
    End Sub

    Private Sub cbo_Filter_PartyName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Filter_PartyName.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_PartyName, cbo_Filter_ClothName, cbo_Filter_LoomNo, "Ledger_AlaisHead", "Ledger_DisplayName", " (  Ledger_Type = 'WEAVER' OR Ledger_Type = 'JOBWORKER')", "(Ledger_idno = 0)")
    End Sub

    Private Sub cbo_Filter_PartyName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Filter_PartyName.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Filter_PartyName, cbo_Filter_LoomNo, "Ledger_AlaisHead", "Ledger_DisplayName", " (  Ledger_Type = 'WEAVER' OR Ledger_Type = 'JOBWORKER')", "(Ledger_idno = 0)")
    End Sub


    Private Sub btn_filtershow_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_filtershow.Click
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim n As Integer
        Dim Led_IdNo As Integer, Clt_IdNo As Integer, Lom_IdNo As Integer
        Dim Condt As String = ""

        Try

            Condt = ""
            Led_IdNo = 0
            Clt_IdNo = 0
            Lom_IdNo = 0

            If IsDate(dtp_FilterFrom_date.Value) = True And IsDate(dtp_FilterTo_date.Value) = True Then
                Condt = "a.Sort_Change_Date between '" & Trim(Format(dtp_FilterFrom_date.Value, "MM/dd/yyyy")) & "' and '" & Trim(Format(dtp_FilterTo_date.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_FilterFrom_date.Value) = True Then
                Condt = "a.Sort_Change_Date = '" & Trim(Format(dtp_FilterFrom_date.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_FilterTo_date.Value) = True Then
                Condt = "a. Sort_Change_Date= '" & Trim(Format(dtp_FilterTo_date.Value, "MM/dd/yyyy")) & "' "
            End If

            If Trim(cbo_Filter_PartyName.Text) <> "" Then
                Led_IdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Filter_PartyName.Text)
            End If

            If Val(Led_IdNo) <> 0 Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " (a.Ledger_Idno = " & Str(Val(Led_IdNo)) & ")"
            End If

            If Trim(cbo_Filter_ClothName.Text) <> "" Then
                Clt_IdNo = Common_Procedures.Cloth_NameToIdNo(con, cbo_Filter_ClothName.Text)
            End If
            If Val(Clt_IdNo) <> 0 Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " (a.Cloth_Idno1 = " & Str(Val(Clt_IdNo)) & " or a.Cloth_Idno2 = " & Str(Val(Clt_IdNo)) & " or a.Cloth_Idno3 = " & Str(Val(Clt_IdNo)) & " or a.Cloth_Idno4 = " & Str(Val(Clt_IdNo)) & ")"
            End If

            If Trim(cbo_Filter_LoomNo.Text) <> "" Then
                Lom_IdNo = Common_Procedures.Loom_NameToIdNo(con, cbo_Filter_LoomNo.Text)
            End If
            If Val(Lom_IdNo) <> 0 Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " (a.Loom_Idno = " & Str(Val(Lom_IdNo)) & ")"
            End If

            da = New SqlClient.SqlDataAdapter("select a.*, b.Ledger_Name , c.Cloth_Name,  d.Loom_Name from Sort_Change_Head a INNER JOIN Ledger_Head b ON a.Ledger_IdNo = b.Ledger_IdNo INNER JOIN Cloth_Head c ON a.Cloth_IdNo1 = c.Cloth_IdNo LEFT OUTER JOIN Loom_Head d ON a.Loom_IdNo = d.Loom_IdNo where a.company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Sort_Change_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "' " & IIf(Trim(Condt) <> "", " and ", "") & Condt & " Order by a.for_orderby, a.Sort_Change_No", con)
            dt2 = New DataTable
            da.Fill(dt2)

            dgv_filter.Rows.Clear()

            If dt2.Rows.Count > 0 Then

                For i = 0 To dt2.Rows.Count - 1

                    n = dgv_filter.Rows.Add()

                    dgv_filter.Rows(n).Cells(0).Value = dt2.Rows(i).Item("Sort_Change_No").ToString
                    dgv_filter.Rows(n).Cells(1).Value = Format(Convert.ToDateTime(dt2.Rows(i).Item("Sort_Change_Date").ToString), "dd-MM-yyyy")
                    dgv_filter.Rows(n).Cells(2).Value = dt2.Rows(i).Item("Ledger_Name").ToString
                    dgv_filter.Rows(n).Cells(3).Value = dt2.Rows(i).Item("Cloth_Name").ToString
                    dgv_filter.Rows(n).Cells(4).Value = dt2.Rows(i).Item("Beam_No1").ToString
                    dgv_filter.Rows(n).Cells(5).Value = dt2.Rows(i).Item("Beam_No2").ToString



                Next i

            End If

            dt2.Clear()
            dt2.Dispose()
            da.Dispose()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT FILTER...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        If dgv_filter.Visible And dgv_filter.Enabled Then dgv_filter.Focus()

    End Sub
    Private Sub cbo_Filter_ClothName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Filter_ClothName.KeyDown

        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_ClothName, dtp_FilterTo_date, cbo_Filter_PartyName, "Cloth_Head", "Cloth_Name", "", "(Cloth_idno = 0)")
    End Sub

    Private Sub cbo_Filter_ClothName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Filter_ClothName.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Filter_ClothName, cbo_Filter_PartyName, "Cloth_Head", "Cloth_Name", "", "(Cloth_idno = 0)")
    End Sub
    Private Sub cbo_Filter_LoomNo_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Filter_LoomNo.KeyDown

        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_LoomNo, cbo_Filter_PartyName, btn_filtershow, "Loom_Head", "Loom_Name", "", "(Loom_IdNo = 0)")

    End Sub

    Private Sub cbo_Filter_LoomNo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Filter_LoomNo.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Filter_LoomNo, btn_filtershow, "Loom_Head", "Loom_Name", "", "(Loom_IdNo = 0 )")
        'If Asc(e.KeyChar) = 13 Then
        '    If Trim(UCase(cbo_LoomNo.Text)) = "" Then
        '        If MessageBox.Show("Do you want to select  :", "FOR  SELECTION...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = DialogResult.Yes Then
        '            btn_Selection_Click(sender, e)
        '        Else
        '            cbo_WidthType.Focus()
        '        End If

        '    Else
        '        cbo_WidthType.Focus()

        '    End If

        'End If

    End Sub


    Private Sub dgv_filter_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgv_filter.DoubleClick
        Open_FilterEntry()
    End Sub


    Private Sub btn_filtershow_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles btn_filtershow.KeyPress
        If Asc(e.KeyChar) = 13 Then
            cbo_Filter_PartyName.Focus()
        End If
    End Sub


    Private Sub btn_closefilter_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        pnl_back.Enabled = True
        pnl_filter.Visible = False
        Filter_Status = False
    End Sub

    Private Sub dgv_filter_CellDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_filter.CellDoubleClick
        Open_FilterEntry()
    End Sub

    Private Sub dgv_filter_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_filter.KeyDown
        If e.KeyCode = 13 Then
            Open_FilterEntry()
        End If
    End Sub

    Private Sub Open_FilterEntry()
        Dim movno As String

        movno = Trim(dgv_filter.CurrentRow.Cells(0).Value)

        If Val(movno) <> 0 Then
            Filter_Status = True
            move_record(movno)
            pnl_back.Enabled = True
            pnl_filter.Visible = False
        End If

    End Sub

    Private Sub cbo_ClothName_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_ClothName1.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Dim f As New Cloth_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_ClothName1.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If

    End Sub


    Private Sub cbo_LoomNo_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_LoomNo.GotFocus
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1155" Then
            Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Loom_Head", "Loom_Name", "( Beam_Knotting_Code <> '' and  Loom_CompanyIdno = " & Val(lbl_Company.Tag) & " ) ", "(Loom_IdNo = 0)")
        Else
            Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Loom_Head", "Loom_Name", "( Beam_Knotting_Code <> '')", "(Loom_IdNo = 0)")
        End If
    End Sub

    Private Sub cbo_LoomNo_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_LoomNo.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1155" Then
            Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_LoomNo, lbl_EndsCount, cbo_WidthType, "Loom_Head", "Loom_Name", "( Beam_Knotting_Code <> '' and  Loom_CompanyIdno = " & Val(lbl_Company.Tag) & " )", "(Loom_IdNo = 0)")
        Else
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_LoomNo, lbl_EndsCount, cbo_WidthType, "Loom_Head", "Loom_Name", "( Beam_Knotting_Code <> '')", "(Loom_IdNo = 0)")
        End If
    End Sub

    Private Sub cbo_LoomNo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_LoomNo.KeyPress
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1155" Then
            Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_LoomNo, Nothing, "Loom_Head", "Loom_Name", "( Beam_Knotting_Code <> '' and  Loom_CompanyIdno = " & Val(lbl_Company.Tag) & " )", "(Loom_IdNo = 0 )")
        Else
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_LoomNo, Nothing, "Loom_Head", "Loom_Name", "( Beam_Knotting_Code <>'')", "(Loom_IdNo = 0 )")
        End If
        If Asc(e.KeyChar) = 13 Then
            If MessageBox.Show("Do you want to select Loom :", "FOR LOOM SELECTION...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = DialogResult.Yes Then
                btn_Selection_Click(sender, e)
            Else
                cbo_WidthType.Focus()
            End If
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

    Private Sub cbo_LoomNo_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_LoomNo.LostFocus
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim Lm_ID As Integer

        With cbo_LoomNo

            If Trim(UCase(.Tag)) <> Trim(UCase(.Text)) Then

                Lm_ID = Common_Procedures.Loom_NameToIdNo(con, .Text)

                Da = New SqlClient.SqlDataAdapter("select top 1 Width_Type from Sort_Change_Head where loom_idno = " & Str(Val(Lm_ID)) & " Order by Sort_Change_Date desc, For_OrderBy desc, Sort_Change_No desc", con)
                Dt1 = New DataTable
                Da.Fill(Dt1)

                If Dt1.Rows.Count > 0 Then
                    If IsDBNull(Dt1.Rows(0).Item("Width_Type").ToString) = False Then
                        If Dt1.Rows(0).Item("Width_Type").ToString <> "" Then
                            cbo_WidthType.Text = Dt1.Rows(0).Item("Width_Type").ToString
                        End If
                    End If
                End If

            End If

        End With
    End Sub

    Private Sub btn_save_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_save.Click
        save_record()
    End Sub

    Private Sub btn_close_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_close.Click
        Me.Close()
    End Sub

    Private Sub img_Selection_Click()
        '    Dim a() As String
        '    Dim RS As ADODB.Recordset
        '    Dim New_Code As String
        '    Dim i As Integer, Led_Idno As Integer, Loom_Idno As Integer
        '    Dim Nr As Long

        'If Trim(sdt_DcDate.GetDate) = "" Then MgB.Message [Error On Save], "Invalid Date": Exit Sub
        'If IsDate(sdt_DcDate.GetDate) = False Then MgB.Message [Error On Save], "Invalid Date": Exit Sub
        'If Not (CmpDet.FromDate <= Trim(sdt_DcDate.GetDate) And CmpDet.ToDate >= Trim(sdt_DcDate.GetDate)) Then MgB.Message [Error On Save], "Date is Out of Financial Year": Exit Sub

        '    Led_Idno = Val(Cmpr.Ledger_NameToIdno(con, cmb_Ledger.Text))
        'If Led_Idno = 0 Then MgB.Message Others, "Invalid Godown", "Does Not Select", , , [Error Icon]: cmb_Ledger.SetFocus: Exit Sub
        '    Loom_Idno = Val(Loom_NameToIdno(cmb_LoomNo.Text))
        'If Loom_Idno = 0 Then MgB.Message Others, "Invalid LoomNo", "Does Not Select", , , [Error Icon]: cmb_LoomNo.SetFocus: Exit Sub

        '    New_Code = Trim(Pk_Control.Caption) & "/" & Trim(CmpDet.FnYear)

        '    grd_Selection.Rows = 1

        '    con.Execute("truncate table Entry_Temp")
        '    If New_Entry = False Then
        '        con.Execute("insert into Entry_Temp(smallint_1, text_4, text_1, text_2, meters_1, meters_2, text_3,text_5) select 1, a.Set_No, a.Set_No, a.beam_no, a.meters, (a.Meters - a.Production_Meters), '1', a.Ends_Count from Stock_SizedPavu_Processing_Details a, " & Table_Name & " b Where b." & Pk_Field & " = '" & Trim(New_Code) & "' and a.Set_No = b.Set_Code1 and a.Beam_No = b.Beam_No1 and a.Ends_Count = '" & Trim(Cmb_Ends.Text) & "'")
        '        con.Execute("insert into Entry_Temp(smallint_1, text_4, text_1, text_2, meters_1, meters_2, text_3,text_5) select 1, a.Set_No, a.Set_No, a.beam_no, a.meters, (a.Meters - a.Production_Meters), '1', a.Ends_Count from Stock_SizedPavu_Processing_Details a, " & Table_Name & " b Where b." & Pk_Field & " = '" & Trim(New_Code) & "' and a.Set_No = b.Set_Code2 and a.Beam_No = b.Beam_No2 and a.Ends_Count = '" & Trim(Cmb_Ends.Text) & "'")
        '    End If
        '    con.Execute("insert into Entry_Temp(smallint_1, text_4, text_1, text_2, meters_1, meters_2, text_3) select 2, set_no, set_no, beam_no, meters, (a.Meters - a.Production_Meters), '' from Stock_SizedPavu_Processing_Details a Where Holding_Idno = " & Str(Led_Idno) & " and a.Ends_Count = '" & Trim(Cmb_Ends.Text) & "' and Sort_Change_Code = '' and Loom_IdNo = 0 and Production_Status <> 2 and Meters > Production_Meters and a.beam_no NOT IN (select Text_2 from Entry_Temp c where c.Text_4 = a.set_no)", Nr)
        '    Debug.Print(Nr)

        '    RS = New ADODB.Recordset
        '    RS.Open("select * from Entry_Temp where smallint_1 > 0 order by smallint_1, text_2", con, adOpenStatic, adLockReadOnly)
        '    If Not (RS.BOF And RS.EOF) Then
        '        RS.MoveFirst()
        '        Do While Not RS.EOF
        '            grd_Selection.AddItem(grd_Selection.Rows & vbTab & RS!text_1 & vbTab & RS!text_2 & vbTab & RS!Meters_1 & vbTab & RS!Meters_2 & vbTab & RS!text_3 & vbTab & RS!text_4)
        '            If Val(RS!text_3) = 1 Then
        '                Call Cmpr.Grids_CellForeColor(grd_Selection, grd_Selection.Rows - 1, 1)
        '            End If
        '            RS.MoveNext()
        '        Loop
        '    End If
        '    RS.Close()
        '    RS = Nothing

        '    fra_Back.Enabled = False
        '    fra_Selection.Visible = True
        '    grd_Selection.Row = 0 : grd_Selection.Col = 0 : grd_Selection.SetFocus()
        '    Call Smart_SendKeys("{DOWN}")      'WshShl.SendKeys "{DOWN}"

    End Sub

    Private Sub grd_Selection_Click()
        'Dim i As Integer, BmCnt As Integer

        'With grd_Selection
        '    If .Row > 0 Then
        '        .TextMatrix(.Row, 5) = (Val(.TextMatrix(.Row, 5)) + 1) Mod 2
        '        Call Cmpr.Grids_CellForeColor(grd_Selection, .Row, Val(.TextMatrix(.Row, 5)))
        '        If Val(.TextMatrix(.Row, 5)) = 0 Then .TextMatrix(.Row, 5) = ""
        '        Call Smart_SendKeys("{RIGHT}")      'WshShl.SendKeys "{RIGHT}"
        '    End If
        '    BmCnt = 0
        '    For i = 1 To .Rows - 1
        '        If Val(.TextMatrix(i, 5)) = 1 Then
        '            BmCnt = BmCnt + 1
        '        End If
        '    Next
        '    If BmCnt = 2 Then Call lbl_CloseSelection_Click()
        'End With
    End Sub

    Private Sub grd_Selection_KeyPress(ByVal KeyAscii As Integer)
        'If KeyAscii = 32 Or KeyAscii = 13 Then Call grd_Selection_Click()
        'If KeyAscii = 27 Then Call lbl_CloseSelection_Click()
    End Sub

    Private Sub cmb_BeamNo1_KeyPress(ByVal KeyAscii As Integer)
        'Dim Rs1 As ADODB.Recordset

        's2d_Meters1.SetValue(0)
        'Rs1 = New ADODB.Recordset
        'Rs1.Open("Select * from Stock_SizedPavu_Processing_Details where Set_No = '" & Trim(cmb_SetNo1.Text) & "' and Beam_No = '" & Trim(cmb_BeamNo1.Text) & "' and Production_Status <> 2", con, adOpenStatic, adLockReadOnly)
        'If Not (Rs1.BOF And Rs1.EOF) Then
        '    Rs1.MoveFirst()
        '    s2d_Meters1.SetValue(Val(Rs1!meters - Rs1!Production_Meters))
        'End If
        'Rs1.Close()
        'Rs1 = Nothing
    End Sub

    Private Sub cmb_SetNo1_GotFocus()
        'Dim God_ID As Integer
        'God_ID = Val(Cmpr.Ledger_NameToIdno(con, cmb_Ledger.Text))
        'cmb_SetNo1.Condition = "(Holding_IdNo = " & Str(God_ID) & " and Production_Status <> 2)"
    End Sub

    Private Sub btn_Selection_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Selection.Click
        Dim Cmd As New SqlClient.SqlCommand
        Dim Da1 As New SqlClient.SqlDataAdapter
        Dim Da2 As New SqlClient.SqlDataAdapter
        Dim Da3 As New SqlClient.SqlDataAdapter
        Dim Da4 As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim Dt2 As New DataTable
        Dim Dt3 As New DataTable
        Dim Dt4 As New DataTable
        Dim i As Integer, j As Integer, n As Integer, SNo As Integer
        Dim Lm_ID As Integer
        Dim NewCode As String

        If IsDate(dtp_date.Text) = False Then
            MessageBox.Show("Invalid Date", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If dtp_date.Enabled Then dtp_date.Focus()
            Exit Sub
        End If

        If Not (dtp_date.Value.Date >= Common_Procedures.Company_FromDate And dtp_date.Value.Date <= Common_Procedures.Company_ToDate) Then
            MessageBox.Show("Date is out of financial range", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If dtp_date.Enabled Then dtp_date.Focus()
            Exit Sub
        End If

        Lm_ID = Common_Procedures.Loom_NameToIdNo(con, cbo_LoomNo.Text)
        If Lm_ID = 0 Then
            MessageBox.Show("Invalid Loom No", "DOES NOT SELECT PAVU...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If cbo_LoomNo.Enabled Then cbo_LoomNo.Focus()
            Exit Sub
        End If

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Cmd.Connection = con

        With dgv_Selection

            .Rows.Clear()
            SNo = 0

            Da1 = New SqlClient.SqlDataAdapter("Select a.* from Sort_Change_Head a Where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Loom_IdNo = " & Str(Val(Lm_ID)) & " and a.Beam_RunOut_Code = '' and a.Sort_Change_Code = '" & Trim(NewCode) & "'  order by a.Sort_Change_Date desc, a.for_orderby desc, a.Beam_Knotting_No desc", con)
            Dt1 = New DataTable
            Da1.Fill(Dt1)

            If Dt1.Rows.Count > 0 Then
                For i = 0 To Dt1.Rows.Count - 1

                    n = .Rows.Add()

                    SNo = SNo + 1
                    .Rows(n).Cells(0).Value = Val(SNo)

                    .Rows(n).Cells(1).Value = Dt1.Rows(i).Item("Beam_Knotting_No").ToString
                    .Rows(n).Cells(2).Value = Format(Convert.ToDateTime(Dt1.Rows(i).Item("Sort_Change_Date").ToString), "dd-MM-yyyy")
                    .Rows(n).Cells(3).Value = Common_Procedures.Ledger_IdNoToName(con, Val(Dt1.Rows(i).Item("Ledger_IdNo").ToString))
                    .Rows(n).Cells(4).Value = Common_Procedures.EndsCount_IdNoToName(con, Val(Dt1.Rows(i).Item("EndsCount_IdNo").ToString))
                    .Rows(n).Cells(5).Value = Dt1.Rows(i).Item("Width_Type").ToString
                    .Rows(n).Cells(6).Value = Common_Procedures.Cloth_IdNoToName(con, Val(Dt1.Rows(i).Item("Cloth_Idno1").ToString))

                    .Rows(n).Cells(7).Value = "1"

                    .Rows(n).Cells(8).Value = Common_Procedures.Cloth_IdNoToName(con, Val(Dt1.Rows(i).Item("Cloth_Idno2").ToString))
                    .Rows(n).Cells(9).Value = Common_Procedures.Cloth_IdNoToName(con, Val(Dt1.Rows(i).Item("Cloth_Idno3").ToString))
                    .Rows(n).Cells(10).Value = Common_Procedures.Cloth_IdNoToName(con, Val(Dt1.Rows(i).Item("Cloth_Idno4").ToString))

                    .Rows(n).Cells(11).Value = Dt1.Rows(i).Item("Beam_Knotting_Code").ToString
                    .Rows(n).Cells(12).Value = ""
                    .Rows(n).Cells(13).Value = ""

                    .Rows(n).Cells(14).Value = Dt1.Rows(i).Item("Set_No1").ToString
                    .Rows(n).Cells(15).Value = Dt1.Rows(i).Item("Set_No2").ToString
                    .Rows(n).Cells(16).Value = Dt1.Rows(i).Item("Beam_No1").ToString
                    .Rows(n).Cells(17).Value = Dt1.Rows(i).Item("Beam_No2").ToString
                    .Rows(n).Cells(18).Value = Dt1.Rows(i).Item("Beam_Meters1").ToString
                    .Rows(n).Cells(19).Value = Dt1.Rows(i).Item("Beam_Meters2").ToString
                    ' .Rows(n).Cells(20).Value = Dt1.Rows(i).Item("Production_Meters").ToString
                    .Rows(n).Cells(21).Value = ""


                    For j = 0 To .ColumnCount - 1
                        .Rows(i).Cells(j).Style.ForeColor = Color.Red
                    Next

                Next

            End If
            Dt1.Clear()

            Da1 = New SqlClient.SqlDataAdapter("Select a.* from Beam_Knotting_Head a Where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Loom_IdNo = " & Str(Val(Lm_ID)) & " and a.Beam_RunOut_Code = '' and a.Sort_Change_Code = '' Order by a.Beam_Knotting_Date desc, a.for_orderby desc, a.Beam_Knotting_No desc", con)
            Dt1 = New DataTable
            Da1.Fill(Dt1)

            If Dt1.Rows.Count > 0 Then
                For i = 0 To Dt1.Rows.Count - 1

                    n = .Rows.Add()

                    SNo = SNo + 1
                    .Rows(n).Cells(0).Value = Val(SNo)

                    .Rows(n).Cells(1).Value = Dt1.Rows(i).Item("Beam_Knotting_No").ToString
                    .Rows(n).Cells(2).Value = Format(Convert.ToDateTime(Dt1.Rows(i).Item("Beam_Knotting_Date").ToString), "dd-MM-yyyy")
                    .Rows(n).Cells(3).Value = Common_Procedures.Ledger_IdNoToName(con, Val(Dt1.Rows(i).Item("Ledger_IdNo").ToString))
                    .Rows(n).Cells(4).Value = Common_Procedures.EndsCount_IdNoToName(con, Val(Dt1.Rows(i).Item("EndsCount_IdNo").ToString))
                    .Rows(n).Cells(5).Value = Dt1.Rows(i).Item("Width_Type").ToString
                    .Rows(n).Cells(6).Value = Common_Procedures.Cloth_IdNoToName(con, Val(Dt1.Rows(i).Item("Cloth_Idno1").ToString))

                    .Rows(n).Cells(7).Value = ""

                    .Rows(n).Cells(8).Value = Common_Procedures.Cloth_IdNoToName(con, Val(Dt1.Rows(i).Item("Cloth_Idno2").ToString))
                    .Rows(n).Cells(9).Value = Common_Procedures.Cloth_IdNoToName(con, Val(Dt1.Rows(i).Item("Cloth_Idno3").ToString))
                    .Rows(n).Cells(10).Value = Common_Procedures.Cloth_IdNoToName(con, Val(Dt1.Rows(i).Item("Cloth_Idno4").ToString))

                    .Rows(n).Cells(11).Value = Dt1.Rows(i).Item("Beam_Knotting_Code").ToString
                    .Rows(n).Cells(12).Value = ""
                    .Rows(n).Cells(13).Value = ""

                    .Rows(n).Cells(14).Value = Dt1.Rows(i).Item("Set_No1").ToString
                    .Rows(n).Cells(15).Value = Dt1.Rows(i).Item("Set_No2").ToString
                    .Rows(n).Cells(16).Value = Dt1.Rows(i).Item("Beam_No1").ToString
                    .Rows(n).Cells(17).Value = Dt1.Rows(i).Item("Beam_No2").ToString
                    .Rows(n).Cells(18).Value = Dt1.Rows(i).Item("Beam_Meters1").ToString
                    .Rows(n).Cells(19).Value = Dt1.Rows(i).Item("Beam_Meters2").ToString
                    .Rows(n).Cells(20).Value = Dt1.Rows(i).Item("Production_Meters").ToString
                    .Rows(n).Cells(21).Value = ""


                    For j = 0 To .ColumnCount - 1
                        .Rows(i).Cells(j).Style.ForeColor = Color.Black
                    Next

                Next

            End If


            If dgv_Selection.Rows.Count = 0 Then

                Da1 = New SqlClient.SqlDataAdapter("Select a.* from Sort_Change_Head a Where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Loom_IdNo = " & Str(Val(Lm_ID)) & " and a.Beam_Knotting_Code <> '' and a.Last_Sort_Change_Code = '" & Trim(NewCode) & "'  order by a.Sort_Change_Date desc, a.for_orderby desc, a.Beam_Knotting_No desc", con)
                Dt1 = New DataTable
                Da1.Fill(Dt1)

                If Dt1.Rows.Count > 0 Then
                    For i = 0 To Dt1.Rows.Count - 1

                        n = .Rows.Add()

                        SNo = SNo + 1
                        .Rows(n).Cells(0).Value = Val(SNo)

                        .Rows(n).Cells(1).Value = Dt1.Rows(i).Item("Beam_Knotting_No").ToString
                        .Rows(n).Cells(2).Value = Format(Convert.ToDateTime(Dt1.Rows(i).Item("Sort_Change_Date").ToString), "dd-MM-yyyy")
                        .Rows(n).Cells(3).Value = Common_Procedures.Ledger_IdNoToName(con, Val(Dt1.Rows(i).Item("Ledger_IdNo").ToString))
                        .Rows(n).Cells(4).Value = Common_Procedures.EndsCount_IdNoToName(con, Val(Dt1.Rows(i).Item("EndsCount_IdNo").ToString))
                        .Rows(n).Cells(5).Value = Dt1.Rows(i).Item("Display_Width_Type").ToString
                        .Rows(n).Cells(6).Value = Common_Procedures.Cloth_IdNoToName(con, Val(Dt1.Rows(i).Item("Display_Cloth_Idno1").ToString))

                        .Rows(n).Cells(7).Value = "1"

                        .Rows(n).Cells(8).Value = Common_Procedures.Cloth_IdNoToName(con, Val(Dt1.Rows(i).Item("Display_Cloth_Idno2").ToString))
                        .Rows(n).Cells(9).Value = Common_Procedures.Cloth_IdNoToName(con, Val(Dt1.Rows(i).Item("Display_Cloth_Idno3").ToString))
                        .Rows(n).Cells(10).Value = Common_Procedures.Cloth_IdNoToName(con, Val(Dt1.Rows(i).Item("Display_Cloth_Idno4").ToString))

                        .Rows(n).Cells(11).Value = Dt1.Rows(i).Item("Beam_Knotting_Code").ToString
                        .Rows(n).Cells(12).Value = Dt1.Rows(i).Item("Sort_Change_No").ToString
                        .Rows(n).Cells(13).Value = Dt1.Rows(i).Item("Sort_Change_Code").ToString

                        .Rows(n).Cells(14).Value = Dt1.Rows(i).Item("Set_No1").ToString
                        .Rows(n).Cells(15).Value = Dt1.Rows(i).Item("Set_No2").ToString
                        .Rows(n).Cells(16).Value = Dt1.Rows(i).Item("Beam_No1").ToString
                        .Rows(n).Cells(17).Value = Dt1.Rows(i).Item("Beam_No2").ToString
                        .Rows(n).Cells(18).Value = Dt1.Rows(i).Item("Beam_Meters1").ToString
                        .Rows(n).Cells(19).Value = Dt1.Rows(i).Item("Beam_Meters2").ToString
                        .Rows(n).Cells(20).Value = Dt1.Rows(i).Item("Balance_Meters1").ToString
                        .Rows(n).Cells(21).Value = Dt1.Rows(i).Item("Balance_Meters2").ToString


                        For j = 0 To .ColumnCount - 1
                            .Rows(i).Cells(j).Style.ForeColor = Color.Red
                        Next

                    Next

                End If
                Dt1.Clear()

                Da1 = New SqlClient.SqlDataAdapter("Select a.* from Sort_Change_Head a Where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Loom_IdNo = " & Str(Val(Lm_ID)) & " and a.Beam_Knotting_Code <> '' and a.Last_Sort_Change_Code = '' order by a.Sort_Change_Date desc, a.for_orderby desc, a.Beam_Knotting_No desc", con)
                Dt1 = New DataTable
                Da1.Fill(Dt1)

                If Dt1.Rows.Count > 0 Then
                    For i = 0 To Dt1.Rows.Count - 1

                        n = .Rows.Add()

                        SNo = SNo + 1
                        .Rows(n).Cells(0).Value = Val(SNo)

                        .Rows(n).Cells(1).Value = Dt1.Rows(i).Item("Beam_Knotting_No").ToString
                        .Rows(n).Cells(2).Value = Format(Convert.ToDateTime(Dt1.Rows(i).Item("Sort_Change_Date").ToString), "dd-MM-yyyy")
                        .Rows(n).Cells(3).Value = Common_Procedures.Ledger_IdNoToName(con, Val(Dt1.Rows(i).Item("Ledger_IdNo").ToString))
                        .Rows(n).Cells(4).Value = Common_Procedures.EndsCount_IdNoToName(con, Val(Dt1.Rows(i).Item("EndsCount_IdNo").ToString))
                        .Rows(n).Cells(5).Value = Dt1.Rows(i).Item("Display_Width_Type").ToString
                        .Rows(n).Cells(6).Value = Common_Procedures.Cloth_IdNoToName(con, Val(Dt1.Rows(i).Item("Display_Cloth_Idno1").ToString))

                        .Rows(n).Cells(7).Value = ""

                        .Rows(n).Cells(8).Value = Common_Procedures.Cloth_IdNoToName(con, Val(Dt1.Rows(i).Item("Display_Cloth_Idno2").ToString))
                        .Rows(n).Cells(9).Value = Common_Procedures.Cloth_IdNoToName(con, Val(Dt1.Rows(i).Item("Display_Cloth_Idno3").ToString))
                        .Rows(n).Cells(10).Value = Common_Procedures.Cloth_IdNoToName(con, Val(Dt1.Rows(i).Item("Display_Cloth_Idno4").ToString))

                        .Rows(n).Cells(11).Value = Dt1.Rows(i).Item("Beam_Knotting_Code").ToString
                        .Rows(n).Cells(12).Value = Dt1.Rows(i).Item("Sort_Change_No").ToString
                        .Rows(n).Cells(13).Value = Dt1.Rows(i).Item("Sort_Change_Code").ToString

                        .Rows(n).Cells(14).Value = Dt1.Rows(i).Item("Set_No1").ToString
                        .Rows(n).Cells(15).Value = Dt1.Rows(i).Item("Set_No2").ToString
                        .Rows(n).Cells(16).Value = Dt1.Rows(i).Item("Beam_No1").ToString
                        .Rows(n).Cells(17).Value = Dt1.Rows(i).Item("Beam_No2").ToString
                        .Rows(n).Cells(18).Value = Dt1.Rows(i).Item("Beam_Meters1").ToString
                        .Rows(n).Cells(19).Value = Dt1.Rows(i).Item("Beam_Meters2").ToString
                        .Rows(n).Cells(20).Value = Dt1.Rows(i).Item("Balance_Meters1").ToString
                        .Rows(n).Cells(21).Value = Dt1.Rows(i).Item("Balance_Meters2").ToString

                        For j = 0 To .ColumnCount - 1
                            .Rows(i).Cells(j).Style.ForeColor = Color.Black
                        Next

                    Next

                End If
                Dt1.Clear()
            End If

        End With

        Dt3.Clear()
        Dt1.Dispose()
        Da1.Dispose()
        Cmd.Dispose()

        pnl_Selection.Visible = True
        pnl_back.Enabled = False
        dgv_Selection.Focus()
        If dgv_Selection.Rows.Count > 0 Then
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

    Private Sub dgv_Selection_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_Selection.KeyDown
        'Try
        '    If e.KeyCode = Keys.Enter Or e.KeyCode = Keys.Space Then
        '        If dgv_Selection.CurrentCell.RowIndex >= 0 Then
        '            Select_Pavu(dgv_Selection.CurrentCell.RowIndex)
        '            e.Handled = True
        '        End If
        '    End If
        'Catch ex As Exception
        '    '-----
        'End Try
    End Sub

    Private Sub btn_Close_Selection_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Close_Selection.Click
        Close_Selection()
    End Sub

    Private Sub Close_Selection()
        Dim i As Integer

        lbl_PartyName.Text = ""
        lbl_EndsCount.Text = ""

        lbl_SetCode1.Text = ""
        lbl_SetNo1.Text = ""
        lbl_BeamNo1.Text = ""
        lbl_Meters1.Text = ""
        lbl_SetCode2.Text = ""
        lbl_SetNo2.Text = ""
        lbl_BeamNo2.Text = ""
        lbl_Meters2.Text = ""

        lbl_KnotNo.Text = ""
        lbl_KnotCode.Text = ""

        lbl_LastSortNo.Text = ""
        lbl_LastSortCode.Text = ""

        lbl_BalanceMeters1.Text = ""
        lbl_BalanceMeters2.Text = ""

        cbo_ClothName1.Text = ""
        cbo_ClothName2.Text = ""
        cbo_ClothName3.Text = ""
        cbo_ClothName4.Text = ""
        lbl_Cloth_Name1.Text = ""
        lbl_Cloth_Name2.Text = ""
        lbl_Cloth_Name3.Text = ""
        lbl_Cloth_Name4.Text = ""


        With dgv_Selection
            For i = 0 To .Rows.Count - 1
                If Val(.Rows(i).Cells(7).Value) = 1 Then

                    lbl_PartyName.Text = .Rows(i).Cells(3).Value
                    cbo_ClothName1.Text = .Rows(i).Cells(6).Value
                    cbo_ClothName2.Text = .Rows(i).Cells(8).Value
                    cbo_ClothName3.Text = .Rows(i).Cells(9).Value
                    cbo_ClothName4.Text = .Rows(i).Cells(10).Value

                    lbl_Cloth_Name1.Text = .Rows(i).Cells(6).Value
                    lbl_Cloth_Name2.Text = .Rows(i).Cells(8).Value
                    lbl_Cloth_Name3.Text = .Rows(i).Cells(9).Value
                    lbl_Cloth_Name4.Text = .Rows(i).Cells(10).Value

                    lbl_EndsCount.Text = .Rows(i).Cells(4).Value
                    cbo_WidthType.Text = .Rows(i).Cells(5).Value
                    lbl_WidthType.Text = .Rows(i).Cells(5).Value

                    lbl_SetNo1.Text = .Rows(i).Cells(14).Value
                    lbl_SetNo2.Text = .Rows(i).Cells(15).Value
                    lbl_BeamNo1.Text = .Rows(i).Cells(16).Value
                    lbl_BeamNo2.Text = .Rows(i).Cells(17).Value

                    lbl_Meters1.Text = Format(Val(.Rows(i).Cells(18).Value), "#########0.00")
                    lbl_Meters2.Text = Format(Val(.Rows(i).Cells(19).Value), "#########0.00")

                    lbl_KnotNo.Text = .Rows(i).Cells(1).Value
                    lbl_KnotCode.Text = .Rows(i).Cells(11).Value

                    lbl_LastSortNo.Text = .Rows(i).Cells(12).Value
                    lbl_LastSortCode.Text = .Rows(i).Cells(13).Value

                    lbl_BalanceMeters1.Text = Format(Val(.Rows(i).Cells(18).Value), "#########0.00")
                    lbl_BalanceMeters2.Text = Format(Val(.Rows(i).Cells(19).Value), "#########0.00")

                End If
            Next
        End With

        pnl_back.Enabled = True
        pnl_Selection.Visible = False
        If cbo_WidthType.Enabled And cbo_WidthType.Visible Then cbo_WidthType.Focus()

    End Sub

    Private Sub cbo_WidthType_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_WidthType.TextChanged
        If Trim(UCase(cbo_WidthType.Text)) = "FOURTH" Then
            cbo_ClothName1.Enabled = True
            cbo_ClothName2.Enabled = True
            cbo_ClothName3.Enabled = True
            cbo_ClothName4.Enabled = True

        ElseIf Trim(UCase(cbo_WidthType.Text)) = "TRIPLE" Then
            cbo_ClothName1.Enabled = True
            cbo_ClothName2.Enabled = True
            cbo_ClothName3.Enabled = True

            cbo_ClothName4.Text = ""
            cbo_ClothName4.Enabled = False

        ElseIf Trim(UCase(cbo_WidthType.Text)) = "DOUBLE" Then
            cbo_ClothName3.Text = ""
            cbo_ClothName4.Text = ""
            cbo_ClothName2.Enabled = True
            cbo_ClothName3.Enabled = False
            cbo_ClothName4.Enabled = False

        Else

            cbo_ClothName2.Text = ""
            cbo_ClothName3.Text = ""
            cbo_ClothName4.Text = ""
            cbo_ClothName2.Enabled = False
            cbo_ClothName3.Enabled = False
            cbo_ClothName4.Enabled = False

        End If
    End Sub

    Private Sub Grid_Selection(ByVal RwIndx As Integer)
        'Dim i As Integer

        'With dgv_Selection

        '    If .RowCount > 0 And RwIndx >= 0 Then

        '        .Rows(RwIndx).Cells(5).Value = (Val(.Rows(RwIndx).Cells(5).Value) + 1) Mod 2

        '        If Val(.Rows(RwIndx).Cells(5).Value) = 0 Then

        '            .Rows(RwIndx).Cells(5).Value = ""
        '            .CurrentCell = .Rows(RwIndx).Cells(0)
        '            If RwIndx >= 10 Then .FirstDisplayedScrollingRowIndex = RwIndx - 9

        '            For i = 0 To .ColumnCount - 1
        '                .Rows(RwIndx).Cells(i).Style.ForeColor = Color.Blue
        '            Next

        '        Else
        '            For i = 0 To .ColumnCount - 1
        '                .Rows(RwIndx).Cells(i).Style.ForeColor = Color.Red
        '            Next

        '        End If

        '    End If
        '    If txt_SetNoSelection.Enabled = True Then txt_SetNoSelection.Focus()

        'End With

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
                dtp_date.Focus()
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
                dtp_date.Focus()
            End If
        End If
    End Sub

    Private Sub cbo_LoomNo_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbo_LoomNo.SelectedIndexChanged

    End Sub

    Private Sub btn_UserModification_Click(sender As Object, e As System.EventArgs) Handles btn_UserModification.Click
        If Val(Common_Procedures.User.IdNo) = 1 Then
            Dim f1 As New User_Modifications
            f1.Entry_Name = Me.Name
            f1.Entry_PkValue = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)
            f1.ShowDialog()
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
            cbo_Shift.Focus()
        End If

    End Sub

    Private Sub msk_Date_KeyPress(sender As Object, e As KeyPressEventArgs) Handles msk_Date.KeyPress
        If Trim(UCase(e.KeyChar)) = "D" Then
            msk_Date.Text = Date.Today
            msk_Date.SelectionStart = 0
        End If


        If Asc(e.KeyChar) = 13 Then
            cbo_Shift.Focus()
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

    Private Sub msk_Date_MaskInputRejected(sender As Object, e As MaskInputRejectedEventArgs) Handles msk_Date.MaskInputRejected

    End Sub

    Private Sub dtp_Date_ValueChanged(sender As Object, e As EventArgs) Handles dtp_Date.ValueChanged

    End Sub

    Private Sub cbo_Shift_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbo_Shift.SelectedIndexChanged

    End Sub
End Class