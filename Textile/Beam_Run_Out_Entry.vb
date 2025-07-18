Public Class Beam_Run_Out_Entry
    Implements Interface_MDIActions

    Private con As New SqlClient.SqlConnection(Common_Procedures.Connection_String)
    Private con1 As New SqlClient.SqlConnection(Common_Procedures.ConnectionString_CompanyGroupdetails)
    Private FrmLdSTS As Boolean = False
    Private New_Entry As Boolean = False
    Private Insert_Entry As Boolean = False
    Private Filter_Status As Boolean = False
    Private Pk_Condition As String = "RNOUT-"
    Private Prec_ActCtrl As New Control
    Private vcbo_KeyDwnVal As Double
    Private SaveAll_STS As Boolean = False
    Private LastNo As String = ""
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
        lbl_NewSTS.Visible = False
        vmskOldText = ""
        vmskSelStrt = -1
        lbl_RefNo.Text = ""
        lbl_RefNo.ForeColor = Color.Black
        dtp_Date.Text = ""
        msk_Date.Text = ""
        cbo_Shift.Text = Common_Procedures.Shift_IdNoToName(con, 1)
        lbl_KnotCode.Text = ""
        lbl_KnotNo.Text = ""

        lbl_SetCode1.Text = ""
        lbl_SetNo1.Text = ""
        lbl_TotMtrs1.Text = ""
        lbl_TotMtrs2.Text = ""
        lbl_BalMtrs1.Text = ""
        lbl_BalMtrs2.Text = ""
        lbl_SetCode2.Text = ""
        lbl_SetNo2.Text = ""
        cbo_LoomNo.Text = ""
        cbo_LoomNo.Tag = ""
        lbl_BeamNo1.Text = ""
        lbl_BeamNo2.Text = ""
        cbo_Empolyee.Text = ""
        Chk_BeamClose.Checked = True
        Chk_Beam2Close.Checked = True
        lbl_CrimpPerc1.Text = ""
        lbl_CrimpPerc2.Text = ""
        lbl_ProdMtrs1.Text = ""
        lbl_ProdMtrs2.Text = ""
        cbo_LoomNo.Enabled = True
        cbo_LoomNo.BackColor = Color.White
        lbl_UserName.Text = "USER : " & Trim(UCase(Common_Procedures.User_IdNoToName(con1, Common_Procedures.User.IdNo)))
    End Sub

    Private Sub ControlGotFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim txtbx As TextBox
        Dim combobx As ComboBox
        Dim msktxbx As MaskedTextBox
        On Error Resume Next

        If TypeOf Me.ActiveControl Is TextBox Or TypeOf Me.ActiveControl Is ComboBox Or TypeOf Me.ActiveControl Is Button Or TypeOf Me.ActiveControl Is CheckBox Or TypeOf Me.ActiveControl Is MaskedTextBox Then
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
            msktxbx = Me.ActiveControl
            msktxbx.SelectionStart = 0
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
        If Not IsNothing(dgv_filter.CurrentCell) Then dgv_filter.CurrentCell.Selected = False
    End Sub

    Private Sub move_record(ByVal no As String)
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim da4 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim dt4 As New DataTable
        Dim NewCode As String
        Dim LockSTS As Boolean = False

        If Val(no) = 0 Then Exit Sub

        clear()

        Try

            NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(no) & "/" & Trim(Common_Procedures.FnYearCode)

            da1 = New SqlClient.SqlDataAdapter("select a.*, d.Loom_Name from Beam_RunOut_Head a INNER JOIN Loom_Head d ON a.Loom_IdNo = d.Loom_IdNo Where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Beam_RunOut_Code = '" & Trim(NewCode) & "'", con)
            da1.Fill(dt1)

            If dt1.Rows.Count > 0 Then
                lbl_RefNo.Text = dt1.Rows(0).Item("Beam_RunOut_No").ToString
                dtp_Date.Text = dt1.Rows(0).Item("Beam_RunOut_Date").ToString
                msk_Date.Text = dtp_Date.Text
                cbo_Shift.Text = dt1.Rows(0).Item("Shift").ToString
                cbo_LoomNo.Text = dt1.Rows(0).Item("Loom_Name").ToString
                lbl_KnotCode.Text = dt1.Rows(0).Item("Beam_Knotting_Code").ToString
                lbl_KnotNo.Text = dt1.Rows(0).Item("Beam_Knotting_No").ToString
                lbl_BalMtrs1.Text = dt1.Rows(0).Item("Balance_Meters1").ToString
                lbl_BalMtrs2.Text = dt1.Rows(0).Item("Balance_Meters2").ToString
                lbl_BeamNo1.Text = dt1.Rows(0).Item("Beam_No1").ToString
                lbl_BeamNo2.Text = dt1.Rows(0).Item("Beam_No2").ToString
                lbl_SetCode1.Text = dt1.Rows(0).Item("Set_Code1").ToString
                lbl_SetCode2.Text = dt1.Rows(0).Item("Set_Code2").ToString
                lbl_SetNo1.Text = dt1.Rows(0).Item("Set_No1").ToString
                lbl_SetNo2.Text = dt1.Rows(0).Item("Set_No2").ToString
                cbo_Empolyee.Text = dt1.Rows(0).Item("Employee_Name").ToString
                Chk_BeamClose.Checked = False
                If IsDBNull(dt1.Rows(0).Item("Close_Status").ToString) = False Then
                    If Val(dt1.Rows(0).Item("Close_Status").ToString) = 1 Then
                        Chk_BeamClose.Checked = True
                    End If
                End If
                Chk_Beam2Close.Checked = False
                If IsDBNull(dt1.Rows(0).Item("Beam2_CloseStatus").ToString) = False Then
                    If Val(dt1.Rows(0).Item("Beam2_CloseStatus").ToString) = 1 Then
                        Chk_Beam2Close.Checked = True
                    End If
                End If
                lbl_CrimpPerc1.Text = dt1.Rows(0).Item("Crimp_Percentage1").ToString
                lbl_CrimpPerc2.Text = dt1.Rows(0).Item("Crimp_Percentage2").ToString

                lbl_ProdMtrs1.Text = dt1.Rows(0).Item("Production_Meters1").ToString
                lbl_ProdMtrs2.Text = dt1.Rows(0).Item("Production_Meters2").ToString
                lbl_UserName.Text = "USER : " & Trim(UCase(Common_Procedures.User_IdNoToName(con1, Val(dt1.Rows(0).Item("User_IdNo").ToString))))
                lbl_TotMtrs1.Text = ""
                Da4 = New SqlClient.SqlDataAdapter("Select * from Stock_SizedPavu_Processing_Details Where Set_Code = '" & Trim(lbl_SetCode1.Text) & "' and Beam_No = '" & Trim(lbl_BeamNo1.Text) & "'", con)
                Dt4 = New DataTable
                Da4.Fill(Dt4)
                If Dt4.Rows.Count > 0 Then
                    lbl_TotMtrs1.Text = Format(Val(Dt4.Rows(0).Item("Meters").ToString), "#########0.00")
                End If
                Dt4.Clear()

                lbl_TotMtrs2.Text = ""
                da4 = New SqlClient.SqlDataAdapter("Select * from Stock_SizedPavu_Processing_Details Where Set_Code = '" & Trim(lbl_SetCode2.Text) & "' and Beam_No = '" & Trim(lbl_BeamNo2.Text) & "'", con)
                Dt4 = New DataTable
                Da4.Fill(Dt4)
                If Dt4.Rows.Count > 0 Then
                    lbl_TotMtrs2.Text = Format(Val(Dt4.Rows(0).Item("Meters").ToString), "#########0.00")
                End If
                Dt4.Clear()

                txt_Stock_CrimpPerc.Text = dt1.Rows(0).Item("Crimp_Perc_ForStock").ToString
                lbl_Stock_CrimpMeters.Text = dt1.Rows(0).Item("Crimp_Meters_ForStock").ToString

                LockSTS = False
                'da1 = New SqlClient.SqlDataAdapter("select * from Loom_Head where Loom_IdNo = " & Str(Val(dt1.Rows(0).Item("Loom_IdNo").ToString)), con)
                'Dt2 = New DataTable
                'da1.Fill(dt2)
                'If dt2.Rows.Count > 0 Then
                '    If Dt2.Rows(0).Item("Beam_Knotting_Code").ToString <> "" Then
                '        LockSTS = True
                '    End If
                'End If
                'dt2.Clear()

            End If
            dt1.Clear()

            Selection_Knotting_Details()

            If LockSTS = True Then
                cbo_LoomNo.Enabled = False
                cbo_LoomNo.BackColor = Color.LightGray
            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally
            dt1.Dispose()
            dt4.Dispose()
            da1.Dispose()
            da4.Dispose()
            If msk_Date.Visible And msk_Date.Enabled Then msk_Date.Focus()

        End Try

    End Sub

    Private Sub Empty_BeamBagCone_Delivery_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated

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

    Private Sub Beam_Knotting_Entry_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim dt3 As New DataTable

        Me.Text = ""

        con.Open()

        Da = New SqlClient.SqlDataAdapter("select Loom_Name from Loom_head order by Loom_Name", con)
        Da.Fill(Dt1)
        cbo_LoomNo.DataSource = Dt1
        cbo_LoomNo.DisplayMember = "Loom_Name"

        Da = New SqlClient.SqlDataAdapter("select Shift_Name from Shift_Head order by Shift_Name", con)
        Da.Fill(Dt2)
        cbo_Shift.DataSource = Dt2
        cbo_Shift.DisplayMember = "Shift_Name"

        Da = New SqlClient.SqlDataAdapter("select Employee_Name from Beam_RunOut_Head order by Employee_Name", con)
        Da.Fill(dt3)
        cbo_Shift.DataSource = dt3
        cbo_Shift.DisplayMember = "Employee_Name"

        pnl_filter.Visible = False
        pnl_filter.Left = (Me.Width - pnl_filter.Width) \ 2
        pnl_filter.Top = ((Me.Height - pnl_filter.Height) \ 2) + 20

        pnl_Stock_CrimpDetails.Visible = False
        If Common_Procedures.settings.AutoLoom_Pavu_CrimpMeters_Consumption_Stock_Posting_In_SeparateEntry = 1 Then
            pnl_Stock_CrimpDetails.Visible = True
        End If


        btn_UserModification.Visible = False
        If Common_Procedures.settings.User_Modifications_Show_Status = 1 Then
            If Val(Common_Procedures.User.IdNo) = 1 Or Common_Procedures.User.Show_UserModification_Status = 1 Then
                btn_UserModification.Visible = True
            End If
        End If


        AddHandler msk_Date.GotFocus, AddressOf ControlGotFocus
        AddHandler dtp_Date.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Shift.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_LoomNo.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Empolyee.GotFocus, AddressOf ControlGotFocus
        AddHandler Chk_BeamClose.GotFocus, AddressOf ControlGotFocus
        AddHandler Chk_Beam2Close.GotFocus, AddressOf ControlGotFocus

        AddHandler txt_Filter_knotting_no.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Filter_LoomNo.GotFocus, AddressOf ControlGotFocus
        AddHandler msk_Date.LostFocus, AddressOf ControlLostFocus
        AddHandler dtp_Date.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Shift.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_LoomNo.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Empolyee.LostFocus, AddressOf ControlLostFocus
        AddHandler Chk_BeamClose.LostFocus, AddressOf ControlLostFocus
        AddHandler Chk_Beam2Close.LostFocus, AddressOf ControlLostFocus

        AddHandler cbo_Filter_LoomNo.LostFocus, AddressOf ControlLostFocus

        AddHandler txt_Filter_knotting_no.LostFocus, AddressOf ControlLostFocus
        'AddHandler msk_Date.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Filter_knotting_no.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler dtp_FilterFrom_date.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler dtp_FilterTo_date.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler Chk_BeamClose.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler Chk_Beam2Close.KeyDown, AddressOf TextBoxControlKeyDown

        'AddHandler msk_Date.KeyPress, AddressOf TextBoxControlKeyPress

        AddHandler txt_Filter_knotting_no.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler dtp_FilterFrom_date.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler dtp_FilterTo_date.KeyPress, AddressOf TextBoxControlKeyPress

        lbl_Company.Text = ""
        lbl_Company.Tag = 0
        lbl_Company.Visible = False
        Common_Procedures.CompIdNo = 0

        dgv_filter.RowTemplate.Height = 27

        Filter_Status = False
        FrmLdSTS = True
        new_record()

    End Sub


    Private Sub Beam_Knotting_Entry_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        On Error Resume Next
        con.Close()
        con.Dispose()
        Common_Procedures.Last_Closed_FormName = Me.Name
    End Sub

    Private Sub Beam_Knotting_Entry_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress

        Try

            If Asc(e.KeyChar) = 27 Then

                If pnl_filter.Visible = True Then
                    btn_closefilter_Click(sender, e)
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
        Dim Nr As Long = 0
        Dim vOrdByNo As String = ""

        vOrdByNo = Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text)
        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.DeletingEntry, Common_Procedures.UR.Inhouse_Beam_RunOut_Entry, New_Entry, Me, con, "Beam_RunOut_Head", "Beam_RunOut_Code", NewCode, "Beam_RunOut_Date", "(Beam_RunOut_Code = '" & Trim(NewCode) & "')") = False Then Exit Sub


        If MessageBox.Show("Do you want to Delete?", "FOR DELETION...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) <> Windows.Forms.DialogResult.Yes Then
            Exit Sub
        End If

        If New_Entry = True Then
            MessageBox.Show("This is New Entry", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        cmd.Connection = con

        Da = New SqlClient.SqlDataAdapter("select * from Beam_RunOut_Head Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Beam_RunOut_Code = '" & Trim(NewCode) & "'", con)
        Dt1 = New DataTable
        Da.Fill(Dt1)
        If Dt1.Rows.Count > 0 Then

            Da = New SqlClient.SqlDataAdapter("select * from Loom_Head where Loom_IdNo = " & Str(Val(Dt1.Rows(0).Item("Loom_IdNo").ToString)), con)
            Dt2 = New DataTable
            Da.Fill(dt2)
            If dt2.Rows.Count > 0 Then
                If Dt2.Rows(0).Item("Beam_Knotting_Code").ToString <> "" Then
                    MessageBox.Show("Already this loom is knotted with another set of beams", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Exit Sub
                End If
            End If
            dt2.Clear()

            Da = New SqlClient.SqlDataAdapter("select * from Stock_SizedPavu_Processing_Details where set_code = '" & Trim(Dt1.Rows(0).Item("Set_Code1").ToString) & "' and Beam_No = '" & Trim(Dt1.Rows(0).Item("Beam_No1").ToString) & "'", con)
            Dt2 = New DataTable
            Da.Fill(dt2)
            If Dt2.Rows.Count > 0 Then
                If Dt2.Rows(0).Item("Beam_Knotting_Code").ToString <> "" Then
                    MessageBox.Show("Already this beam is running", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Exit Sub
                End If
            End If
            Dt2.Clear()

            Da = New SqlClient.SqlDataAdapter("select * from Stock_SizedPavu_Processing_Details where set_code = '" & Trim(Dt1.Rows(0).Item("Set_Code2").ToString) & "' and Beam_No = '" & Trim(Dt1.Rows(0).Item("Beam_No2").ToString) & "'", con)
            Dt2 = New DataTable
            Da.Fill(Dt2)
            If Dt2.Rows.Count > 0 Then
                If Dt2.Rows(0).Item("Beam_Knotting_Code").ToString <> "" Then
                    MessageBox.Show("Already this beam is running", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Exit Sub
                End If
            End If
            Dt2.Clear()

        End If
        Dt1.Clear()

        tr = con.BeginTransaction

        Try


            cmd.Transaction = tr
            Call Common_Procedures.User_Modification_Updation(con, "HEAD", Me.Name, "DELETE", "Beam_RunOut_Head", "Beam_RunOut_Code", Val(lbl_Company.Tag), NewCode, lbl_RefNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, True, "", "", "Beam_RunOut_Code, Company_IdNo, for_OrderBy", tr)

            Da = New SqlClient.SqlDataAdapter("select * from Beam_RunOut_Head Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Beam_RunOut_Code = '" & Trim(NewCode) & "'", con)
            Da.SelectCommand.Transaction = tr
            Dt1 = New DataTable
            Da.Fill(Dt1)
            If Dt1.Rows.Count > 0 Then

                Nr = 0
                cmd.CommandText = "Update Beam_Knotting_Head Set Beam_RunOut_Code = '' Where Loom_Idno = " & Str(Val(Dt1.Rows(0).Item("Loom_Idno").ToString)) & " and Beam_Knotting_Code = '" & Trim(Dt1.Rows(0).Item("Beam_Knotting_Code").ToString) & "' and Beam_RunOut_Code = '" & Trim(NewCode) & "'"
                Nr = cmd.ExecuteNonQuery()
                If Nr = 0 Then
                    Throw New ApplicationException("Invalid Editing : These Beams already runnot")
                    Exit Sub
                End If

                If Trim(Dt1.Rows(0).Item("Set_Code1").ToString) <> "" And Trim(Dt1.Rows(0).Item("Beam_No1").ToString) <> "" Then
                    If Val(Dt1.Rows(0).Item("Close_Status").ToString) = 1 Then
                        cmd.CommandText = "Update Stock_SizedPavu_Processing_Details Set Close_Status = 0 Where set_code = '" & Trim(Dt1.Rows(0).Item("Set_Code1").ToString) & "' and Beam_No = '" & Trim(Dt1.Rows(0).Item("Beam_No1").ToString) & "'"
                        cmd.ExecuteNonQuery()
                    End If
                End If

                If Trim(Dt1.Rows(0).Item("Set_Code2").ToString) <> "" And Trim(Dt1.Rows(0).Item("Beam_No2").ToString) <> "" Then
                    If Val(Dt1.Rows(0).Item("Close_Status").ToString) = 1 Then
                        cmd.CommandText = "Update Stock_SizedPavu_Processing_Details Set Close_Status = 0 Where set_code = '" & Trim(Dt1.Rows(0).Item("Set_Code2").ToString) & "' and Beam_No = '" & Trim(Dt1.Rows(0).Item("Beam_No2").ToString) & "'"
                        cmd.ExecuteNonQuery()
                    End If
                End If

                If Trim(Dt1.Rows(0).Item("Set_Code1").ToString) <> "" And Trim(Dt1.Rows(0).Item("Beam_No1").ToString) <> "" Then

                    Nr = 0
                    cmd.CommandText = "Update Stock_SizedPavu_Processing_Details Set Beam_Knotting_Code = '" & Trim(Dt1.Rows(0).Item("Beam_Knotting_Code").ToString) & "', Loom_Idno = " & Str(Val(Dt1.Rows(0).Item("Loom_Idno").ToString)) & " Where set_code = '" & Trim(Dt1.Rows(0).Item("Set_Code1").ToString) & "' and Beam_No = '" & Trim(Dt1.Rows(0).Item("Beam_No1").ToString) & "' and Beam_Knotting_Code = '' and Loom_Idno = 0"
                    Nr = cmd.ExecuteNonQuery()
                    If Nr = 0 Then
                        Throw New ApplicationException("Invalid Editing : Already this beam is running")
                        Exit Sub
                    End If

                End If

                If Trim(Dt1.Rows(0).Item("Set_Code2").ToString) <> "" And Trim(Dt1.Rows(0).Item("Beam_No2").ToString) <> "" Then

                    Nr = 0
                    cmd.CommandText = "Update Stock_SizedPavu_Processing_Details Set Beam_Knotting_Code = '" & Trim(Dt1.Rows(0).Item("Beam_Knotting_Code").ToString) & "', Loom_Idno = " & Str(Val(Dt1.Rows(0).Item("Loom_Idno").ToString)) & " Where set_code = '" & Trim(Dt1.Rows(0).Item("Set_Code2").ToString) & "' and Beam_No = '" & Trim(Dt1.Rows(0).Item("Beam_No2").ToString) & "' and Beam_Knotting_Code = '' and Loom_Idno = 0"
                    Nr = cmd.ExecuteNonQuery()
                    If Nr = 0 Then
                        Throw New ApplicationException("Invalid Editing : Already this beam is running")
                        Exit Sub
                    End If

                End If

                Nr = 0
                cmd.CommandText = "Update Loom_Head Set Beam_Knotting_Code = '" & Trim(Dt1.Rows(0).Item("Beam_Knotting_Code").ToString) & "' Where Loom_Idno = " & Str(Val(Dt1.Rows(0).Item("Loom_Idno").ToString)) & " and Beam_Knotting_Code = ''"
                Nr = cmd.ExecuteNonQuery()
                If Nr = 0 Then
                    Throw New ApplicationException("Invalid Editing : Already this Loom was knotted with other beams")
                    Exit Sub
                End If

            End If
            Dt1.Clear()

            cmd.CommandText = "Delete from Stock_Pavu_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "delete from Beam_RunOut_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Beam_RunOut_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            tr.Commit()

            new_record()

            MessageBox.Show("Deleted Sucessfully!!!", "FOR DELETION...", MessageBoxButtons.OK, MessageBoxIcon.Information)

        Catch ex As Exception

            tr.Rollback()
            MessageBox.Show(ex.Message, "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally
            cmd.Dispose()
            tr.Dispose()
            Da.Dispose()
            Dt1.Dispose()
            Dt2.Dispose()

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

            da = New SqlClient.SqlDataAdapter("select Loom_Name from Loom_head order by Loom_Name", con)
            da.Fill(dt2)
            cbo_Filter_LoomNo.DataSource = dt2
            cbo_Filter_LoomNo.DisplayMember = "Loom_Name"

            da = New SqlClient.SqlDataAdapter("select BeamNo_SetCode_forSelection from Stock_SizedPavu_Processing_Details order by BeamNo_SetCode_forSelection", con)
            dt3 = New DataTable
            da.Fill(dt3)
            cbo_Filter_BeamNo.DataSource = dt3
            cbo_Filter_BeamNo.DisplayMember = "BeamNo_SetCode_forSelection"

            Cmd.Connection = con

            Cmd.CommandText = "Update Stock_SizedPavu_Processing_Details set BeamNo_SetCode_forSelection = Beam_No + ' | ' + setcode_forSelection Where Beam_No <> ''"
            Cmd.ExecuteNonQuery()

            dtp_FilterFrom_date.Text = Common_Procedures.Company_FromDate
            dtp_FilterTo_date.Text = Common_Procedures.Company_ToDate
            pnl_filter.Text = ""

            cbo_Filter_LoomNo.SelectedIndex = -1
            cbo_Filter_BeamNo.SelectedIndex = -1
            dgv_filter.Rows.Clear()

            da.Dispose()

        End If
        '****************************************************************
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


        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.InsertingEntry, Common_Procedures.UR.Inhouse_Beam_RunOut_Entry, New_Entry, Me) = False Then Exit Sub




    
        Try

            inpno = InputBox("Enter New Ref.No.", "FOR INSERTION...")

            NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

            cmd.Connection = con
            cmd.CommandText = "select Beam_RunOut_No from Beam_RunOut_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Beam_RunOut_Code = '" & Trim(NewCode) & "'"
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
            cmd.CommandText = "select top 1 Beam_RunOut_No from Beam_RunOut_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Beam_RunOut_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby, Beam_RunOut_No"
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
            cmd.CommandText = "select top 1 Beam_RunOut_No from Beam_RunOut_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Beam_RunOut_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, Beam_RunOut_No desc"
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
            cmd.CommandText = "select top 1 Beam_RunOut_No from Beam_RunOut_Head where for_orderby > " & Str(OrdByNo) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and Beam_RunOut_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby, Beam_RunOut_No"
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
            cmd.CommandText = "select top 1 Beam_RunOut_No from Beam_RunOut_Head where for_orderby < " & Str(OrdByNo) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and  Beam_RunOut_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, Beam_RunOut_No desc"
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
            lbl_NewSTS.Visible = True

            da = New SqlClient.SqlDataAdapter("select max(for_orderby) from Beam_RunOut_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Beam_RunOut_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' ", con)
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
            da = New SqlClient.SqlDataAdapter("select top 1 * from Beam_RunOut_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Beam_RunOut_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, Beam_RunOut_No desc", con)
            dt1 = New DataTable
            da.Fill(dt1)
            If dt1.Rows.Count > 0 Then
                If Val(Common_Procedures.settings.PreviousEntryDate_ByDefault) = 1 Then '---- M.S Textiles (Tirupur)
                    If dt1.Rows(0).Item("Beam_RunOut_Date").ToString <> "" Then msk_Date.Text = dt1.Rows(0).Item("Beam_RunOut_Date").ToString
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
        Dim cmd As New SqlClient.SqlCommand
        Dim dr As SqlClient.SqlDataReader
        Dim movno As String, inpno As String
        Dim NewCode As String

        Try

            inpno = InputBox("Enter Ref.No", "FOR FINDING...")

            NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

            cmd.Connection = con
            cmd.CommandText = "select Beam_RunOut_No from Beam_RunOut_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Beam_RunOut_Code = '" & Trim(NewCode) & "'"
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

    Public Sub print_record() Implements Interface_MDIActions.print_record
        '------ No Print
    End Sub

    Public Sub save_record() Implements Interface_MDIActions.save_record
        Dim cmd As New SqlClient.SqlCommand
        Dim tr As SqlClient.SqlTransaction
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim NewCode As String = ""
        Dim OrdByNo As Single = 0
        Dim Nr As Long = 0
        Dim Lm_ID As Integer = 0
        Dim vBM1_CloSTS As Integer = 0, vBM2_CloSTS As Integer = 0
        Dim Led_Type As String = ""
        Dim Led_ID As Integer = 0, Delv_ID As Integer = 0, Rec_ID As Integer = 0
        Dim Partcls As String, PBlNo As String, EntID As String
        Dim Clo_ID As Integer = 0
        Dim EdsCnt_ID As Integer = 0
        Dim vOrdByNo As String = ""

        vOrdByNo = Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text)


        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        ' If Common_Procedures.UserRight_Check(Common_Procedures.UR.Inhouse_Beam_Runout_Entry, New_Entry) = False Then Exit Sub
        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.SavingEntry, Common_Procedures.UR.Inhouse_Beam_RunOut_Entry, New_Entry, Me, con, "Beam_RunOut_Head", "Beam_RunOut_Code", NewCode, "Beam_RunOut_Date", "(Beam_RunOut_Code = '" & Trim(NewCode) & "')", "(Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Beam_RunOut_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "')", "for_Orderby desc, Beam_RunOut_No desc", dtp_Date.Value.Date) = False Then Exit Sub





        If pnl_back.Enabled = False Then
            MessageBox.Show("Close all other windows", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        If Val(lbl_Company.Tag) = 0 Then
            MessageBox.Show("Invalid Company Selection", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        If IsDate(msk_Date.Text) = False Then
            MessageBox.Show("Invalid Date", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If msk_Date.Enabled Then msk_Date.Focus()
            Exit Sub
        End If

        If Not (Convert.ToDateTime(msk_Date.Text) >= Common_Procedures.Company_FromDate And Convert.ToDateTime(msk_Date.Text) <= Common_Procedures.Company_ToDate) Then
            MessageBox.Show("Date is out of financial range", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If msk_Date.Enabled Then msk_Date.Focus()
            Exit Sub
        End If

        If Trim(cbo_Shift.Text) = "" Then
            MessageBox.Show("Invalid Shift", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If cbo_Shift.Enabled Then cbo_Shift.Focus()
            Exit Sub
        End If

        Lm_ID = Common_Procedures.Loom_NameToIdNo(con, cbo_LoomNo.Text)
        If Lm_ID = 0 Then
            MessageBox.Show("Invalid Loom Name", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If cbo_LoomNo.Enabled Then cbo_LoomNo.Focus()
            Exit Sub
        End If

        vBM1_CloSTS = 0
        If Chk_BeamClose.Checked = True Then vBM1_CloSTS = 1

        vBM2_CloSTS = 0
        If Chk_Beam2Close.Checked = True Then vBM2_CloSTS = 1

        tr = con.BeginTransaction

        Try

            If Insert_Entry = True Or New_Entry = False Then
                NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            Else

                lbl_RefNo.Text = Common_Procedures.get_MaxCode(con, "Beam_RunOut_Head", "Beam_RunOut_Code", "for_OrderBy", "", Val(lbl_Company.Tag), Common_Procedures.FnYearCode, tr)

                NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            End If

            OrdByNo = Val(Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text))

            cmd.Connection = con
            cmd.Transaction = tr

            cmd.Parameters.Clear()
            cmd.Parameters.AddWithValue("@EntryDate", Convert.ToDateTime(msk_Date.Text))

            If New_Entry = True Then

                cmd.CommandText = "Insert into Beam_RunOut_Head (     Beam_RunOut_Code  ,           Company_IdNo      ,        Beam_RunOut_No         ,     for_OrderBy          , Beam_RunOut_Date,            Shift              ,     Loom_IdNo     ,        Beam_Knotting_Code        ,            Beam_Knotting_No    ,            Set_Code1             ,              Set_No1           ,               Beam_No1          ,          Balance_Meters1      ,               Set_Code2          ,               Set_No2          ,               Beam_No2            ,         Balance_Meters2       ,              Crimp_Percentage1  ,         Crimp_Percentage2       ,            Production_Meters1  ,        Production_Meters2      ,              Employee_Name       ,       Close_Status      ,        Crimp_Perc_ForStock           ,        Crimp_Meters_ForStock           ,                   user_idno              ,      Beam2_CloseStatus   ) " &
                                        "      Values           ('" & Trim(NewCode) & "', " & Val(lbl_Company.Tag) & ", '" & Trim(lbl_RefNo.Text) & "', " & Str(Val(OrdByNo)) & ",    @EntryDate   , '" & Trim(cbo_Shift.Text) & "', " & Val(Lm_ID) & ", '" & Trim(lbl_KnotCode.Text) & "', '" & Trim(lbl_KnotNo.Text) & "', '" & Trim(lbl_SetCode1.Text) & "', '" & Trim(lbl_SetNo1.Text) & "', '" & Trim(lbl_BeamNo1.Text) & "', " & Val(lbl_BalMtrs1.Text) & ", '" & Trim(lbl_SetCode2.Text) & "', '" & Trim(lbl_SetNo2.Text) & "', '" & Trim(lbl_BeamNo2.Text) & "'  , " & Val(lbl_BalMtrs2.Text) & ", " & Val(lbl_CrimpPerc1.Text) & ", " & Val(lbl_CrimpPerc2.Text) & ", " & Val(lbl_ProdMtrs1.Text) & ", " & Val(lbl_ProdMtrs2.Text) & ", '" & Trim(cbo_Empolyee.Text) & "', " & Val(vBM1_CloSTS) & ", " & Val(txt_Stock_CrimpPerc.Text) & ", " & Val(lbl_Stock_CrimpMeters.Text) & ", " & Val(Common_Procedures.User.IdNo) & " , " & Val(vBM2_CloSTS) & " ) "
                cmd.ExecuteNonQuery()

            Else
                Call Common_Procedures.User_Modification_Updation(con, "HEAD", Me.Name, "OLD", "Beam_RunOut_Head", "Beam_RunOut_Code", Val(lbl_Company.Tag), NewCode, lbl_RefNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, False, "", "", "Beam_RunOut_Code, Company_IdNo, for_OrderBy", tr)

                da = New SqlClient.SqlDataAdapter("select * from Beam_RunOut_Head Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Beam_RunOut_Code = '" & Trim(NewCode) & "'", con)
                da.SelectCommand.Transaction = tr
                dt1 = New DataTable
                da.Fill(dt1)
                If dt1.Rows.Count > 0 Then

                    'cmd.Parameters.AddWithValue("@EntryOldDate", dt1.Rows(0).Item("Beam_RunOut_Date"))

                    'cmd.CommandText = "select * from Beam_RunOut_Head where Loom_IdNo = " & Str(Val(dt1.Rows(0).Item("Loom_IdNo").ToString)) & " and (Beam_RunOut_Date > @EntryOldDate or (Beam_RunOut_Date = @EntryOldDate and for_orderby > " & Str(Val(OrdByNo)) & ") )"
                    'da = New SqlClient.SqlDataAdapter(cmd)
                    'dt2 = New DataTable
                    'da.Fill(dt2)
                    'If dt2.Rows.Count > 0 Then
                    '    Throw New ApplicationException("Already another runout is there next to these entry for this loom.")
                    '    Exit Sub
                    'End If
                    'dt2.Clear()

                    'da = New SqlClient.SqlDataAdapter("select * from Loom_Head where Loom_IdNo = " & Str(Val(dt1.Rows(0).Item("Loom_IdNo").ToString)), con)
                    'da.SelectCommand.Transaction = tr
                    'dt2 = New DataTable
                    'da.Fill(dt2)
                    'If dt2.Rows.Count > 0 Then
                    '    If dt2.Rows(0).Item("Beam_Knotting_Code").ToString <> "" Then
                    '        Throw New ApplicationException("Already this loom is knotted with another set of beams")
                    '        Exit Sub
                    '    End If
                    'End If
                    'dt2.Clear()


                    If Trim(dt1.Rows(0).Item("Set_Code1").ToString) <> "" And Trim(dt1.Rows(0).Item("Beam_No1").ToString) <> "" Then

                        If Val(dt1.Rows(0).Item("Close_Status").ToString) = 1 Then
                            Nr = 0
                            cmd.CommandText = "Update Stock_SizedPavu_Processing_Details Set Close_Status = 0 Where set_code = '" & Trim(dt1.Rows(0).Item("Set_Code1").ToString) & "' and Beam_No = '" & Trim(dt1.Rows(0).Item("Beam_No1").ToString) & "'"
                            cmd.ExecuteNonQuery()
                        End If

                    End If


                    If Trim(dt1.Rows(0).Item("Set_Code2").ToString) <> "" And Trim(dt1.Rows(0).Item("Beam_No2").ToString) <> "" Then

                        If Val(dt1.Rows(0).Item("Close_Status").ToString) = 1 Then
                            cmd.CommandText = "Update Stock_SizedPavu_Processing_Details Set Close_Status = 0 Where set_code = '" & Trim(dt1.Rows(0).Item("Set_Code2").ToString) & "' and Beam_No = '" & Trim(dt1.Rows(0).Item("Beam_No2").ToString) & "'"
                            cmd.ExecuteNonQuery()
                        End If

                    End If

                    'da = New SqlClient.SqlDataAdapter("select * from Stock_SizedPavu_Processing_Details where set_code = '" & Trim(dt1.Rows(0).Item("Set_Code1").ToString) & "' and Beam_No = '" & Trim(dt1.Rows(0).Item("Beam_No1").ToString) & "'", con)
                    'da.SelectCommand.Transaction = tr
                    'dt2 = New DataTable
                    'da.Fill(dt2)
                    'If dt2.Rows.Count > 0 Then
                    '    If IsDBNull(dt2.Rows(0).Item("Close_Status").ToString) = False Then
                    '        If Val(dt2.Rows(0).Item("Close_Status").ToString) = 1 Then
                    '            Throw New ApplicationException("Already this beam is Closed")
                    '            Exit Sub
                    '        End If
                    '    End If

                    '    If IsDBNull(dt2.Rows(0).Item("Beam_Knotting_Code").ToString) = False Then
                    '        If dt2.Rows(0).Item("Beam_Knotting_Code").ToString <> "" Then
                    '            Throw New ApplicationException("Already this beam is running")
                    '            Exit Sub
                    '        End If
                    '    End If

                    'End If
                    'dt2.Clear()

                    'da = New SqlClient.SqlDataAdapter("select * from Stock_SizedPavu_Processing_Details where set_code = '" & Trim(dt1.Rows(0).Item("Set_Code2").ToString) & "' and Beam_No = '" & Trim(dt1.Rows(0).Item("Beam_No2").ToString) & "'", con)
                    'da.SelectCommand.Transaction = tr
                    'dt2 = New DataTable
                    'da.Fill(dt2)
                    'If dt2.Rows.Count > 0 Then
                    '    If IsDBNull(dt2.Rows(0).Item("Close_Status").ToString) = False Then
                    '        If Val(dt2.Rows(0).Item("Close_Status").ToString) = 1 Then
                    '            Throw New ApplicationException("Already this beam is Closed")
                    '            Exit Sub
                    '        End If
                    '    End If
                    '    If IsDBNull(dt2.Rows(0).Item("Beam_Knotting_Code").ToString) = False Then
                    '        If dt2.Rows(0).Item("Beam_Knotting_Code").ToString <> "" Then
                    '            Throw New ApplicationException("Already this beam is running")
                    '            Exit Sub
                    '        End If
                    '    End If

                    'End If
                    'dt2.Clear()

                    Nr = 0
                    cmd.CommandText = "Update Beam_Knotting_Head Set Beam_RunOut_Code = '' Where Loom_Idno = " & Str(Val(dt1.Rows(0).Item("Loom_Idno").ToString)) & " and Beam_Knotting_Code = '" & Trim(dt1.Rows(0).Item("Beam_Knotting_Code").ToString) & "' and Beam_RunOut_Code = '" & Trim(NewCode) & "'"
                    Nr = cmd.ExecuteNonQuery()
                    'If Nr = 0 Then
                    '    Throw New ApplicationException("Invalid Editing : These Beams already runnot")
                    '    Exit Sub
                    'End If


                    If Trim(dt1.Rows(0).Item("Set_Code1").ToString) <> "" And Trim(dt1.Rows(0).Item("Beam_No1").ToString) <> "" Then

                        Nr = 0
                        cmd.CommandText = "Update Stock_SizedPavu_Processing_Details Set Beam_Knotting_Code = '" & Trim(dt1.Rows(0).Item("Beam_Knotting_Code").ToString) & "', Loom_Idno = " & Str(Val(dt1.Rows(0).Item("Loom_Idno").ToString)) & " Where set_code = '" & Trim(dt1.Rows(0).Item("Set_Code1").ToString) & "' and Beam_No = '" & Trim(dt1.Rows(0).Item("Beam_No1").ToString) & "' and Beam_Knotting_Code = '' and Loom_Idno = 0"
                        'cmd.CommandText = "Update Stock_SizedPavu_Processing_Details Set Beam_Knotting_Code = '" & Trim(dt1.Rows(0).Item("Beam_Knotting_Code").ToString) & "', Loom_Idno = " & Str(Val(dt1.Rows(0).Item("Loom_Idno").ToString)) & ", Close_Status = 0 Where set_code = '" & Trim(dt1.Rows(0).Item("Set_Code1").ToString) & "' and Beam_No = '" & Trim(dt1.Rows(0).Item("Beam_No1").ToString) & "' and Beam_Knotting_Code = '' and Loom_Idno = 0"
                        Nr = cmd.ExecuteNonQuery()
                        'If Nr = 0 Then
                        '    Throw New ApplicationException("Invalid Editing : Already this beam is running")
                        '    Exit Sub
                        'End If

                    End If

                    If Trim(dt1.Rows(0).Item("Set_Code2").ToString) <> "" And Trim(dt1.Rows(0).Item("Beam_No2").ToString) <> "" Then

                        Nr = 0
                        cmd.CommandText = "Update Stock_SizedPavu_Processing_Details Set Beam_Knotting_Code = '" & Trim(dt1.Rows(0).Item("Beam_Knotting_Code").ToString) & "', Loom_Idno = " & Str(Val(dt1.Rows(0).Item("Loom_Idno").ToString)) & " Where set_code = '" & Trim(dt1.Rows(0).Item("Set_Code2").ToString) & "' and Beam_No = '" & Trim(dt1.Rows(0).Item("Beam_No2").ToString) & "' and Beam_Knotting_Code = '' and Loom_Idno = 0"
                        'cmd.CommandText = "Update Stock_SizedPavu_Processing_Details Set Beam_Knotting_Code = '" & Trim(dt1.Rows(0).Item("Beam_Knotting_Code").ToString) & "', Loom_Idno = " & Str(Val(dt1.Rows(0).Item("Loom_Idno").ToString)) & ", Close_Status = 0 Where set_code = '" & Trim(dt1.Rows(0).Item("Set_Code2").ToString) & "' and Beam_No = '" & Trim(dt1.Rows(0).Item("Beam_No2").ToString) & "' and Beam_Knotting_Code = '' and Loom_Idno = 0"
                        Nr = cmd.ExecuteNonQuery()
                        'If Nr = 0 Then
                        '    Throw New ApplicationException("Invalid Editing : Already this beam is running")
                        '    Exit Sub
                        'End If

                    End If

                    Nr = 0
                    cmd.CommandText = "Update Loom_Head Set Beam_Knotting_Code = '" & Trim(dt1.Rows(0).Item("Beam_Knotting_Code").ToString) & "' Where Loom_Idno = " & Str(Val(dt1.Rows(0).Item("Loom_Idno").ToString)) & " and Beam_Knotting_Code = ''"
                    Nr = cmd.ExecuteNonQuery()
                    'If Nr = 0 Then
                    '    Throw New ApplicationException("Invalid Editing : Already this Loom was knotted with other beams")
                    '    Exit Sub
                    'End If


                End If
                dt1.Clear()

                cmd.CommandText = "Update Beam_RunOut_Head set Beam_RunOut_Date = @EntryDate, Shift = '" & Trim(cbo_Shift.Text) & "', Loom_IdNo = " & Str(Val(Lm_ID)) & ", Beam_Knotting_Code = '" & Trim(lbl_KnotCode.Text) & "', Beam_Knotting_No = '" & Trim(lbl_KnotNo.Text) & "', set_Code1 = '" & Trim(lbl_SetCode1.Text) & "', set_no1 = '" & Trim(lbl_SetNo1.Text) & "', Beam_No1 = '" & Trim(lbl_BeamNo1.Text) & "', Balance_Meters1 = " & Str(Val(lbl_BalMtrs1.Text)) & ", set_Code2 = '" & Trim(lbl_SetCode2.Text) & "', set_no2 = '" & Trim(lbl_SetNo2.Text) & "', Beam_No2 = '" & Trim(lbl_BeamNo2.Text) & "', Balance_Meters2 = " & Str(Val(lbl_BalMtrs2.Text)) & ", Crimp_Percentage1 = " & Str(Val(lbl_CrimpPerc1.Text)) & ", Crimp_Percentage2 = " & Str(Val(lbl_CrimpPerc2.Text)) & ", Production_Meters1 = " & Str(Val(lbl_ProdMtrs1.Text)) & ", Production_Meters2 = " & Str(Val(lbl_ProdMtrs2.Text)) & ", Employee_Name = '" & Trim(cbo_Empolyee.Text) & "' , Close_Status = " & Str(Val(vBM1_CloSTS)) & ", Crimp_Perc_ForStock = " & Val(txt_Stock_CrimpPerc.Text) & ",  Crimp_Meters_ForStock  = " & Val(lbl_Stock_CrimpMeters.Text) & " , Beam2_CloseStatus = " & Val(vBM2_CloSTS) & " Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Beam_RunOut_Code = '" & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()

            End If
            Call Common_Procedures.User_Modification_Updation(con, "HEAD", Me.Name, "NEW", "Beam_RunOut_Head", "Beam_RunOut_Code", Val(lbl_Company.Tag), NewCode, lbl_RefNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, False, "", "", "Beam_RunOut_Code, Company_IdNo, for_OrderBy", tr)

            Nr = 0
            cmd.CommandText = "Update Beam_Knotting_Head Set Beam_RunOut_Code = '" & Trim(NewCode) & "' Where Loom_Idno = " & Str(Lm_ID) & " and Beam_Knotting_Code = '" & Trim(lbl_KnotCode.Text) & "' and Beam_RunOut_Code = ''"
            Nr = cmd.ExecuteNonQuery()
            If Nr = 0 Then
                Throw New ApplicationException("These Beams already runnot")
                Exit Sub
            End If

            Nr = 0
            cmd.CommandText = "Update Loom_Head Set Beam_Knotting_Code = '' Where Beam_Knotting_Code = '" & Trim(lbl_KnotCode.Text) & "'"
            Nr = cmd.ExecuteNonQuery()
            Nr = 0
            cmd.CommandText = "Update Loom_Head Set Beam_Knotting_Code = '' Where Loom_Idno = " & Str(Lm_ID)
            Nr = cmd.ExecuteNonQuery()

            'Nr = 0
            'cmd.CommandText = "Update Loom_Head Set Beam_Knotting_Code = '' Where Loom_Idno = " & Str(Lm_ID) & " and Beam_Knotting_Code = '" & Trim(lbl_KnotCode.Text) & "'"
            'Nr = cmd.ExecuteNonQuery()
            ''If Nr = 0 Then
            ''    Throw New ApplicationException("This Loom is Knotted Again")
            ''    Exit Sub
            ''End If

            If Trim(lbl_SetCode1.Text) <> "" And Trim(lbl_BeamNo1.Text) <> "" Then
                Nr = 0
                cmd.CommandText = "Update Stock_SizedPavu_Processing_Details Set Beam_Knotting_Code = '', Loom_Idno = 0, Close_Status = " & Str(Val(vBM1_CloSTS)) & " Where set_code = '" & Trim(lbl_SetCode1.Text) & "' and Beam_No = '" & Trim(lbl_BeamNo1.Text) & "'"
                Nr = cmd.ExecuteNonQuery()
                'If Nr = 0 Then
                '    Throw New ApplicationException("This Beam is currently not running")
                '    Exit Sub
                'End If
            End If

            If Trim(lbl_SetCode2.Text) <> "" And Trim(lbl_BeamNo2.Text) <> "" Then
                Nr = 0
                cmd.CommandText = "Update Stock_SizedPavu_Processing_Details Set Beam_Knotting_Code = '', Loom_Idno = 0, Close_Status = " & Str(Val(vBM2_CloSTS)) & " Where set_code = '" & Trim(lbl_SetCode2.Text) & "' and Beam_No = '" & Trim(lbl_BeamNo2.Text) & "'"
                Nr = cmd.ExecuteNonQuery()
                'If Nr = 0 Then
                '    Throw New ApplicationException("This Beam is currently not running")
                '    Exit Sub
                'End If
            End If

            'da = New SqlClient.SqlDataAdapter("Select count(*) from Stock_SizedPavu_Processing_Details where Loom_Idno = " & Str(Val(Lm_ID)), con)
            'da.SelectCommand.Transaction = tr
            'dt1 = New DataTable
            'da.Fill(dt1)
            'If dt1.Rows.Count > 0 Then
            '    If IsDBNull(dt1.Rows(0)(0).ToString) = False Then
            '        If Val(dt1.Rows(0)(0).ToString) <> 0 Then
            '            Throw New ApplicationException("Invalid RunOut for this Loom - Some other beams also knotted in this loom")
            '            Exit Sub
            '        End If
            '    End If
            'End If

            cmd.CommandText = "Delete from Stock_Pavu_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()


            If Common_Procedures.settings.AutoLoom_Pavu_CrimpMeters_Consumption_Stock_Posting_In_SeparateEntry = 1 Then

                If Val(lbl_Stock_CrimpMeters.Text) <> 0 Then

                    Led_ID = 0
                    Clo_ID = 0
                    EdsCnt_ID = 0
                    da = New SqlClient.SqlDataAdapter("Select * from Beam_Knotting_Head where Beam_Knotting_Code = '" & Trim(lbl_KnotCode.Text) & "'", con)
                    da.SelectCommand.Transaction = tr
                    dt1 = New DataTable
                    da.Fill(dt1)
                    If dt1.Rows.Count > 0 Then
                        Led_ID = Val(dt1.Rows(0).Item("Ledger_IdNo").ToString)
                        Clo_ID = Val(dt1.Rows(0).Item("Cloth_Idno1").ToString)
                        EdsCnt_ID = Val(dt1.Rows(0).Item("EndsCount_IdNo").ToString)
                    End If
                    dt1.Clear()

                    Led_Type = Common_Procedures.get_FieldValue(con, "Ledger_Head", "Ledger_Type", "(Ledger_IdNo = " & Str(Val(Led_ID)) & ")", , tr)

                    Delv_ID = 0 : Rec_ID = 0
                    If Trim(UCase(Led_Type)) = "JOBWORKER" Then
                        Delv_ID = Led_ID
                        Rec_ID = 0
                    Else
                        Delv_ID = 0
                        Rec_ID = Led_ID
                    End If

                    EntID = Trim(Pk_Condition) & Trim(lbl_RefNo.Text)
                    If Trim(UCase(lbl_SetNo1.Text)) <> Trim(UCase(lbl_SetNo2.Text)) Then
                        Partcls = "Crimp Meters : Beam No1. " & Trim(lbl_BeamNo1.Text) & ", Set No. " & Trim(lbl_SetNo1.Text) & " and Beam No2. " & Trim(lbl_BeamNo1.Text) & ", Set No. " & Trim(lbl_SetNo1.Text)
                    Else
                        Partcls = "Crimp Meters : Beam No. " & Trim(lbl_BeamNo1.Text) & IIf(Trim(lbl_BeamNo2.Text) <> "", " & ", "") & Trim(lbl_BeamNo2.Text) & ", Set No. " & Trim(lbl_SetNo1.Text)
                    End If
                    PBlNo = Trim(lbl_RefNo.Text)


                    cmd.CommandText = "Insert into Stock_Pavu_Processing_Details (           Reference_Code                   ,                 Company_IdNo     ,            Reference_No        ,          for_OrderBy    , Reference_Date,        DeliveryTo_Idno   ,      ReceivedFrom_Idno  ,          Cloth_Idno     ,           Entry_ID   ,        Party_Bill_No ,          Particulars   , Sl_No,          EndsCount_IdNo    , Sized_Beam,                 Meters                       ) " & _
                                        "          Values                        ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_RefNo.Text) & "', " & Str(Val(OrdByNo)) & ",   @EntryDate  , " & Str(Val(Delv_ID)) & ", " & Str(Val(Rec_ID)) & ", " & Str(Val(Clo_ID)) & ", '" & Trim(EntID) & "', '" & Trim(PBlNo) & "', '" & Trim(Partcls) & "',   1  , " & Str(Val(EdsCnt_ID)) & ",      0    , " & Str(Val(lbl_Stock_CrimpMeters.Text)) & " )"
                    cmd.ExecuteNonQuery()

                End If

            End If

            tr.Commit()

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
            cmd.Dispose()
            tr.Dispose()
            da.Dispose()
            dt1.Dispose()
            dt2.Dispose()

            If msk_Date.Enabled And msk_Date.Visible Then msk_Date.Focus()

        End Try

    End Sub

    Private Sub cbo_Shift_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Shift.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Shift_Head", "Shift_Name", "", "(Shift_IdNo = 0)")
    End Sub

    Private Sub cbo_Shift_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Shift.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Shift, msk_Date, cbo_LoomNo, "Shift_Head", "Shift_Name", "", "(Shift_IdNo = 0)")
    End Sub

    Private Sub cbo_Shift_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Shift.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Shift, cbo_LoomNo, "Shift_Head", "Shift_Name", "", "(Shift_IdNo = 0)")
    End Sub

    Private Sub btn_filtershow_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_filtershow.Click
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim n As Integer
        Dim Led_IdNo As Integer, Clt_IdNo As Integer, Lom_IdNo As Integer
        Dim Condt As String = ""
        Dim StCode As String = "", BmNo As String = ""

        Try

            Condt = ""
            Led_IdNo = 0
            Clt_IdNo = 0
            Lom_IdNo = 0

            If IsDate(dtp_FilterFrom_date.Value) = True And IsDate(dtp_FilterTo_date.Value) = True Then
                Condt = "a.Beam_RunOut_Date between '" & Trim(Format(dtp_FilterFrom_date.Value, "MM/dd/yyyy")) & "' and '" & Trim(Format(dtp_FilterTo_date.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_FilterFrom_date.Value) = True Then
                Condt = "a.Beam_RunOut_Date = '" & Trim(Format(dtp_FilterFrom_date.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_FilterTo_date.Value) = True Then
                Condt = "a. Beam_RunOut_Date= '" & Trim(Format(dtp_FilterTo_date.Value, "MM/dd/yyyy")) & "' "
            End If


            Lom_IdNo = 0
            If Trim(cbo_Filter_LoomNo.Text) <> "" Then
                Lom_IdNo = Common_Procedures.Loom_NameToIdNo(con, cbo_Filter_LoomNo.Text)
            End If
            If Val(Lom_IdNo) <> 0 Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " (a.Loom_Idno = " & Str(Val(Lom_IdNo)) & ")"
            End If
            If Val(txt_Filter_knotting_no.Text) <> 0 Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " (a.Beam_Knotting_No = " & Str(Val(txt_Filter_knotting_no.Text)) & ")"
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
                dt2.Clear()

                If Trim(StCode) <> "" And Trim(BmNo) <> "" Then
                    Condt = Trim(Condt) & IIf(Trim(Condt) <> "", " and ", "") & "  ( (a.Set_Code1 = '" & Trim(StCode) & "' and a.Beam_No1 = '" & Trim(BmNo) & "') or (a.Set_Code2 = '" & Trim(StCode) & "' and a.Beam_No2 = '" & Trim(BmNo) & "') ) "

                End If

            End If

            da = New SqlClient.SqlDataAdapter("select a.*,   d.Loom_Name from Beam_RunOut_Head a INNER JOIN Loom_Head d ON a.Loom_IdNo = d.Loom_IdNo where a.company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Beam_RunOut_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "' " & IIf(Trim(Condt) <> "", " and ", "") & Condt & " Order by a.for_orderby, a.Beam_RunOut_No", con)
            dt2 = New DataTable
            da.Fill(dt2)

            dgv_filter.Rows.Clear()

            If dt2.Rows.Count > 0 Then

                For i = 0 To dt2.Rows.Count - 1

                    n = dgv_filter.Rows.Add()

                    dgv_filter.Rows(n).Cells(0).Value = dt2.Rows(i).Item("Beam_RunOut_No").ToString
                    dgv_filter.Rows(n).Cells(1).Value = Format(Convert.ToDateTime(dt2.Rows(i).Item("Beam_RunOut_Date").ToString), "dd-MM-yyyy")
                    dgv_filter.Rows(n).Cells(2).Value = dt2.Rows(i).Item("Loom_Name").ToString
                    dgv_filter.Rows(n).Cells(3).Value = dt2.Rows(i).Item("Set_Code1").ToString
                    dgv_filter.Rows(n).Cells(4).Value = dt2.Rows(i).Item("Set_Code2").ToString
                    dgv_filter.Rows(n).Cells(5).Value = dt2.Rows(i).Item("Beam_No1").ToString
                    dgv_filter.Rows(n).Cells(6).Value = dt2.Rows(i).Item("Beam_No2").ToString



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

    Private Sub cbo_Filter_LoomNo_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Filter_LoomNo.KeyDown

        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_LoomNo, txt_Filter_knotting_no, cbo_Filter_BeamNo, "Loom_Head", "Loom_Name", "", "(Loom_IdNo = 0)")

    End Sub

    Private Sub cbo_Filter_LoomNo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Filter_LoomNo.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Filter_LoomNo, cbo_Filter_BeamNo, "Loom_Head", "Loom_Name", "", "(Loom_IdNo = 0 )")
        'If Asc(e.KeyChar) = 13 Then
        '    If Trim(UCase(cbo_LoomNo.Text)) = "" Then
        '        If MessageBox.Show("Do you want to select  :", "FOR  SELECTION...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = DialogResult.Yes Then
        '            btn_Selection_Click(sender, e)
        '        Else
        '            lbl_KnotNo.Focus()
        '        End If

        '    Else
        '        lbl_KnotNo.Focus()

        '    End If

        'End If

    End Sub


    Private Sub dgv_filter_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgv_filter.DoubleClick
        Open_FilterEntry()
    End Sub


    Private Sub btn_filtershow_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles btn_filtershow.KeyPress
        If Asc(e.KeyChar) = 13 Then
            cbo_Filter_LoomNo.Focus()
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

    Private Sub cbo_LoomNo_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_LoomNo.GotFocus
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1155" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1608" Then
            Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Loom_Head", "Loom_Name", " ( Loom_CompanyIdno = " & Val(lbl_Company.Tag) & " ) ", "(Loom_IdNo = 0)")
        Else
            Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Loom_Head", "Loom_Name", "", "(Loom_IdNo = 0)")
        End If
    End Sub

    Private Sub cbo_LoomNo_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_LoomNo.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1155" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1608" Then
            Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_LoomNo, cbo_Shift, cbo_Empolyee, "Loom_Head", "Loom_Name", " ( Loom_CompanyIdno = " & Val(lbl_Company.Tag) & " )  ", "(Loom_IdNo = 0)")
        Else
            Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_LoomNo, cbo_Shift, cbo_Empolyee, "Loom_Head", "Loom_Name", "", "(Loom_IdNo = 0)")
        End If
    End Sub

    Private Sub cbo_LoomNo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_LoomNo.KeyPress
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1155" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1608" Then
            Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_LoomNo, Nothing, "Loom_Head", "Loom_Name", " ( Loom_CompanyIdno = " & Val(lbl_Company.Tag) & " )  ", "(Loom_IdNo = 0 )")
        Else
            Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_LoomNo, Nothing, "Loom_Head", "Loom_Name", "", "(Loom_IdNo = 0 )")
        End If

        If Asc(e.KeyChar) = 13 Then
            If Trim(cbo_LoomNo.Text) <> "" And (Trim(UCase(cbo_LoomNo.Text)) <> Trim(UCase(cbo_LoomNo.Tag)) Or Trim(lbl_KnotCode.Text) = "") Then
                btn_Selection_Click(sender, e)
            End If
            cbo_Empolyee.Focus()
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


    Private Sub btn_save_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_save.Click
        save_record()
    End Sub

    Private Sub btn_close_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_close.Click
        Me.Close()
    End Sub

    Private Sub txt_Empolyee_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
        '    If Asc(e.KeyChar) = 13 Then
        '        If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
        '            save_record()
        '        Else
        '            dtp_date.Focus()
        '        End If
        '    End If
    End Sub

    Private Sub Chk_BeamClose_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Chk_BeamClose.Click
        Calculate_Crimp_Percentage()
    End Sub

    Private Sub Chk_BeamClose_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Chk_BeamClose.KeyPress
        If Asc(e.KeyChar) = 13 Then
            If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                save_record()
            Else
                dtp_date.Focus()
            End If
        End If
    End Sub

    Private Sub cbo_Empolyee_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Empolyee.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Beam_RunOut_Head", "Employee_Name", "", "")
    End Sub

    Private Sub cbo_Empolyee_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Empolyee.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Empolyee, cbo_LoomNo, Chk_BeamClose, "Beam_RunOut_Head", "Employee_Name", "", "")
    End Sub

    Private Sub cbo_Empolyee_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Empolyee.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Empolyee, Chk_BeamClose, "Beam_RunOut_Head", "Employee_Name", "", "", False)
    End Sub

    Private Sub btn_Selection_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Selection.Click
        Selection_Knotting_Details()
    End Sub

    Private Sub Selection_Knotting_Details()
        Dim Da1 As New SqlClient.SqlDataAdapter
        Dim Da2 As New SqlClient.SqlDataAdapter
        Dim Da3 As New SqlClient.SqlDataAdapter
        Dim Da4 As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim Dt2 As New DataTable
        Dim Dt3 As New DataTable
        Dim Dt4 As New DataTable
        Dim Lm_ID As Integer
        Dim NewCode As String = ""
        Dim ProdMtrs As Double = 0

        Lm_ID = Common_Procedures.Loom_NameToIdNo(con, cbo_LoomNo.Text)
        If Val(Lm_ID) = 0 Then
            MessageBox.Show("Invalid Loom NO", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If cbo_LoomNo.Enabled Then cbo_LoomNo.Focus()
            Exit Sub
        End If

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Da1 = New SqlClient.SqlDataAdapter("Select a.* from Beam_RunOut_Head a Where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Loom_IdNo = " & Str(Val(Lm_ID)) & " and a.Beam_RunOut_Code = '" & Trim(NewCode) & "'", con)
        Dt1 = New DataTable
        Da1.Fill(Dt1)

        If Dt1.Rows.Count > 0 Then

            lbl_KnotCode.Text = Dt1.Rows(0).Item("Beam_Knotting_Code").ToString
            lbl_KnotNo.Text = Dt1.Rows(0).Item("Beam_Knotting_No").ToString

            lbl_SetCode1.Text = Dt1.Rows(0).Item("Set_Code1").ToString
            lbl_SetNo1.Text = Dt1.Rows(0).Item("Set_No1").ToString
            lbl_BeamNo1.Text = Dt1.Rows(0).Item("Beam_No1").ToString
            lbl_TotMtrs1.Text = ""
            lbl_BalMtrs1.Text = ""
            Da2 = New SqlClient.SqlDataAdapter("Select * from Stock_SizedPavu_Processing_Details Where Set_Code = '" & Trim(lbl_SetCode1.Text) & "' and Beam_No = '" & Trim(lbl_BeamNo1.Text) & "'", con)
            Dt2 = New DataTable
            Da2.Fill(Dt2)
            If Dt2.Rows.Count > 0 Then
                lbl_TotMtrs1.Text = Format(Val(Dt2.Rows(0).Item("Meters").ToString), "#########0.00")
                lbl_BalMtrs1.Text = Format(Val(Dt2.Rows(0).Item("Meters").ToString) - Val(Dt2.Rows(0).Item("Production_Meters").ToString), "#########0.00")
            End If
            Dt2.Clear()

            lbl_SetCode2.Text = Dt1.Rows(0).Item("Set_Code2").ToString
            lbl_SetNo2.Text = Dt1.Rows(0).Item("Set_No2").ToString
            lbl_BeamNo2.Text = Dt1.Rows(0).Item("Beam_No2").ToString
            lbl_TotMtrs2.Text = ""
            lbl_BalMtrs2.Text = ""
            Da2 = New SqlClient.SqlDataAdapter("Select * from Stock_SizedPavu_Processing_Details Where Set_Code = '" & Trim(lbl_SetCode2.Text) & "' and Beam_No = '" & Trim(lbl_BeamNo2.Text) & "'", con)
            Dt2 = New DataTable
            Da2.Fill(Dt2)
            If Dt2.Rows.Count > 0 Then
                lbl_TotMtrs2.Text = Format(Val(Dt2.Rows(0).Item("Meters").ToString), "#########0.00")
                lbl_BalMtrs2.Text = Format(Val(Dt2.Rows(0).Item("Meters").ToString) - Val(Dt2.Rows(0).Item("Production_Meters").ToString), "#########0.00")
            End If
            Dt2.Clear()

        Else

            Da3 = New SqlClient.SqlDataAdapter("select a.* from Beam_Knotting_Head a Where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Loom_IdNo = " & Str(Val(Lm_ID)) & " and a.Beam_RunOut_Code = '' Order by a.Beam_Knotting_Date, a.for_OrderBy, a.Beam_Knotting_Code", con)
            Dt3 = New DataTable
            Da3.Fill(Dt3)
            If Dt3.Rows.Count > 0 Then

                lbl_KnotCode.Text = Dt3.Rows(0).Item("Beam_Knotting_Code").ToString
                lbl_KnotNo.Text = Dt3.Rows(0).Item("Beam_Knotting_No").ToString

                lbl_SetCode1.Text = Dt3.Rows(0).Item("Set_Code1").ToString
                lbl_SetNo1.Text = Dt3.Rows(0).Item("Set_No1").ToString
                lbl_BeamNo1.Text = Dt3.Rows(0).Item("Beam_No1").ToString

                lbl_TotMtrs1.Text = ""
                lbl_BalMtrs1.Text = ""

                Da4 = New SqlClient.SqlDataAdapter("Select * from Stock_SizedPavu_Processing_Details Where Set_Code = '" & Trim(lbl_SetCode1.Text) & "' and Beam_No = '" & Trim(lbl_BeamNo1.Text) & "'", con)
                Dt4 = New DataTable
                Da4.Fill(Dt4)
                If Dt4.Rows.Count > 0 Then
                    lbl_TotMtrs1.Text = Format(Val(Dt4.Rows(0).Item("Meters").ToString), "#########0.00")
                    lbl_BalMtrs1.Text = Format(Val(Dt4.Rows(0).Item("Meters").ToString) - Val(Dt4.Rows(0).Item("Production_Meters").ToString), "#########0.00")
                End If
                Dt4.Clear()

                lbl_SetCode2.Text = Dt3.Rows(0).Item("Set_Code2").ToString
                lbl_SetNo2.Text = Dt3.Rows(0).Item("Set_No2").ToString
                lbl_BeamNo2.Text = Dt3.Rows(0).Item("Beam_No2").ToString
                lbl_TotMtrs2.Text = ""
                lbl_BalMtrs2.Text = ""

                Da4 = New SqlClient.SqlDataAdapter("Select * from Stock_SizedPavu_Processing_Details Where Set_Code = '" & Trim(lbl_SetCode2.Text) & "' and Beam_No = '" & Trim(lbl_BeamNo2.Text) & "'", con)
                Dt4 = New DataTable
                Da4.Fill(Dt4)
                If Dt4.Rows.Count > 0 Then
                    lbl_TotMtrs2.Text = Format(Val(Dt4.Rows(0).Item("Meters").ToString), "#########0.00")
                    lbl_BalMtrs2.Text = Format(Val(Dt4.Rows(0).Item("Meters").ToString) - Val(Dt4.Rows(0).Item("Production_Meters").ToString), "#########0.00")
                End If
                Dt4.Clear()

            End If
            Dt3.Clear()

        End If

        lbl_CrimpPerc1.Text = ""
        lbl_ProdMtrs1.Text = ""
        lbl_CrimpPerc2.Text = ""
        lbl_ProdMtrs2.Text = ""
        lbl_BalMtrs1.Text = ""
        lbl_BalMtrs2.Text = ""


        If Trim(lbl_SetCode1.Text) <> "" And Trim(lbl_BeamNo1.Text) <> "" Then
            lbl_CrimpPerc1.Text = Common_Procedures.Calculation_CrimpPercentage_On_BEAMRUNOUT(con, lbl_SetCode1.Text, lbl_BeamNo1.Text, Val(lbl_TotMtrs1.Text), ProdMtrs)
            lbl_ProdMtrs1.Text = Format(Val(ProdMtrs), "#########0.00")
            lbl_BalMtrs1.Text = Format(Val(lbl_TotMtrs1.Text) - Val(ProdMtrs), "#########0.00")
        End If


        If Trim(lbl_SetCode2.Text) <> "" And Trim(lbl_BeamNo2.Text) <> "" Then
            lbl_CrimpPerc2.Text = Common_Procedures.Calculation_CrimpPercentage_On_BEAMRUNOUT(con, lbl_SetCode2.Text, lbl_BeamNo2.Text, Val(lbl_TotMtrs2.Text), ProdMtrs)
            lbl_ProdMtrs2.Text = Format(Val(ProdMtrs), "#########0.00")
            lbl_BalMtrs2.Text = Format(Val(lbl_TotMtrs2.Text) - Val(ProdMtrs), "#########0.00")
        End If

        If Chk_BeamClose.Checked = False Then
            lbl_CrimpPerc1.Text = ""
            lbl_CrimpPerc2.Text = ""
        End If

        txt_Stock_CrimpPerc.Text = Format(Val(lbl_CrimpPerc1.Text), "#########0.00")
        lbl_Stock_CrimpMeters.Text = Format((Val(lbl_TotMtrs1.Text) + Val(lbl_TotMtrs2.Text)) * Val(txt_Stock_CrimpPerc.Text) / 100, "#########0.00")

        cbo_LoomNo.Tag = cbo_LoomNo.Text

        Dt1.Dispose()
        Da1.Dispose()

        Dt2.Dispose()
        Da2.Dispose()

        Dt3.Dispose()
        Da3.Dispose()

        Dt4.Dispose()
        Da4.Dispose()

        If cbo_Empolyee.Enabled And cbo_Empolyee.Visible Then cbo_Empolyee.Focus()

    End Sub

    Private Function Calculation_CrimpPercentage1111(ByVal SetCd As String, ByVal BmNo As String, ByRef ProdMtrs As Double) As Double
        Dim Da1 As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim cmd As New SqlClient.SqlCommand
        Dim WidTyp As Single = 0
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim PavuConsMtrs As Double = 0
        Dim NoofBeams As Integer = 0
        Dim vClo_Mtrs As Double = 0
        Dim vLm_IdNo As Integer = 0
        Dim vWidth_Type As String = ""
        Dim CrmpPerc As Double = 0
        Dim Nr As Long = 0
        Dim SQL1 As String

        cmd.Connection = con

        cmd.CommandText = "Truncate table " & Trim(Common_Procedures.EntryTempTable) & ""
        cmd.ExecuteNonQuery()

        'If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1018" Then '---- M.K Textiles (Palladam)

        SQL1 = "Insert into " & Trim(Common_Procedures.EntryTempTable) & " ( Name1, Meters1 ) Select a.Width_Type,  (CASE WHEN (a.Type1_Meters+a.Type2_Meters+a.Type3_Meters+a.Type4_Meters+a.Type5_Meters) <> 0 THEN (a.Type1_Meters+a.Type2_Meters+a.Type3_Meters+a.Type4_Meters+a.Type5_Meters) ELSE a.Receipt_Meters END) from Weaver_ClothReceipt_Piece_Details a Where a.Set_Code1 = '" & Trim(SetCd) & "' and a.Beam_No1 = '" & Trim(BmNo) & "'"
        cmd.CommandText = "EXEC [SP_ExecuteQuery] '" & Replace(Trim(SQL1), "'", "''") & "'"
        Nr = cmd.ExecuteNonQuery()
        'cmd.CommandText = "Insert into " & Trim(Common_Procedures.EntryTempTable) & " ( Name1, Meters1 ) Select a.Width_Type,  (CASE WHEN (a.Type1_Meters+a.Type2_Meters+a.Type3_Meters+a.Type4_Meters+a.Type5_Meters) <> 0 THEN (a.Type1_Meters+a.Type2_Meters+a.Type3_Meters+a.Type4_Meters+a.Type5_Meters) ELSE a.Receipt_Meters END) from Weaver_ClothReceipt_Piece_Details a Where a.Set_Code1 = '" & Trim(SetCd) & "' and a.Beam_No1 = '" & Trim(BmNo) & "'"
        'Nr = cmd.ExecuteNonQuery()


        SQL1 = "Insert into " & Trim(Common_Procedures.EntryTempTable) & " ( Name1, Meters1 ) Select a.Width_Type, (CASE WHEN (a.Type1_Meters+a.Type2_Meters+a.Type3_Meters+a.Type4_Meters+a.Type5_Meters) <> 0 THEN (a.Type1_Meters+a.Type2_Meters+a.Type3_Meters+a.Type4_Meters+a.Type5_Meters) ELSE a.Receipt_Meters END) from Weaver_ClothReceipt_Piece_Details a Where a.Set_Code2 = '" & Trim(SetCd) & "' and a.Beam_No2 = '" & Trim(BmNo) & "'"
        cmd.CommandText = "EXEC [SP_ExecuteQuery] '" & Replace(Trim(SQL1), "'", "''") & "'"
        Nr = cmd.ExecuteNonQuery()
        'cmd.CommandText = "Insert into " & Trim(Common_Procedures.EntryTempTable) & " ( Name1, Meters1 ) Select a.Width_Type, (CASE WHEN (a.Type1_Meters+a.Type2_Meters+a.Type3_Meters+a.Type4_Meters+a.Type5_Meters) <> 0 THEN (a.Type1_Meters+a.Type2_Meters+a.Type3_Meters+a.Type4_Meters+a.Type5_Meters) ELSE a.Receipt_Meters END) from Weaver_ClothReceipt_Piece_Details a Where a.Set_Code2 = '" & Trim(SetCd) & "' and a.Beam_No2 = '" & Trim(BmNo) & "'"
        'Nr = cmd.ExecuteNonQuery()


        'Else
        SQL1 = "Insert into " & Trim(Common_Procedures.EntryTempTable) & " ( Name1, Meters1 ) Select a.Width_Type, (CASE WHEN (a.Type1_Checking_Meters+a.Type2_Checking_Meters+a.Type3_Checking_Meters+a.Type4_Checking_Meters+a.Type5_Checking_Meters) <> 0 THEN (a.Type1_Checking_Meters+a.Type2_Checking_Meters+a.Type3_Checking_Meters+a.Type4_Checking_Meters+a.Type5_Checking_Meters) ELSE a.Receipt_Meters END) from Weaver_Cloth_Receipt_Head a Where a.Set_Code1 = '" & Trim(SetCd) & "' and a.Beam_No1 = '" & Trim(BmNo) & "'"
        cmd.CommandText = "EXEC [SP_ExecuteQuery] '" & Replace(Trim(SQL1), "'", "''") & "'"
        Nr = cmd.ExecuteNonQuery()
        'cmd.CommandText = "Insert into " & Trim(Common_Procedures.EntryTempTable) & " ( Name1, Meters1 ) Select a.Width_Type, (CASE WHEN (a.Type1_Checking_Meters+a.Type2_Checking_Meters+a.Type3_Checking_Meters+a.Type4_Checking_Meters+a.Type5_Checking_Meters) <> 0 THEN (a.Type1_Checking_Meters+a.Type2_Checking_Meters+a.Type3_Checking_Meters+a.Type4_Checking_Meters+a.Type5_Checking_Meters) ELSE a.Receipt_Meters END) from Weaver_Cloth_Receipt_Head a Where a.Set_Code1 = '" & Trim(SetCd) & "' and a.Beam_No1 = '" & Trim(BmNo) & "'"
        'Nr = cmd.ExecuteNonQuery()


        SQL1 = "Insert into " & Trim(Common_Procedures.EntryTempTable) & " ( Name1, Meters1 ) Select a.Width_Type, (CASE WHEN (a.Type1_Checking_Meters+a.Type2_Checking_Meters+a.Type3_Checking_Meters+a.Type4_Checking_Meters+a.Type5_Checking_Meters) <> 0 THEN (a.Type1_Checking_Meters+a.Type2_Checking_Meters+a.Type3_Checking_Meters+a.Type4_Checking_Meters+a.Type5_Checking_Meters) ELSE a.Receipt_Meters END) from Weaver_Cloth_Receipt_Head a Where a.Set_Code2 = '" & Trim(SetCd) & "' and a.Beam_No2 = '" & Trim(BmNo) & "'"
        cmd.CommandText = "EXEC [SP_ExecuteQuery] '" & Replace(Trim(SQL1), "'", "''") & "'"
        Nr = cmd.ExecuteNonQuery()
        'cmd.CommandText = "Insert into " & Trim(Common_Procedures.EntryTempTable) & " ( Name1, Meters1 ) Select a.Width_Type, (CASE WHEN (a.Type1_Checking_Meters+a.Type2_Checking_Meters+a.Type3_Checking_Meters+a.Type4_Checking_Meters+a.Type5_Checking_Meters) <> 0 THEN (a.Type1_Checking_Meters+a.Type2_Checking_Meters+a.Type3_Checking_Meters+a.Type4_Checking_Meters+a.Type5_Checking_Meters) ELSE a.Receipt_Meters END) from Weaver_Cloth_Receipt_Head a Where a.Set_Code2 = '" & Trim(SetCd) & "' and a.Beam_No2 = '" & Trim(BmNo) & "'"
        'Nr = cmd.ExecuteNonQuery()


        'End If

        'vLm_IdNo = Common_Procedures.Loom_NameToIdNo(con, cbo_LoomNo.Text)

        'NoofBeams = Val(Common_Procedures.get_FieldValue(con, "Loom_Head", "Noof_Input_Beams", "(Loom_IdNo = " & Str(Val(vLm_IdNo)) & ")"))
        'If Val(NoofBeams) = 0 Then NoofBeams = 1

        Da1 = New SqlClient.SqlDataAdapter("Select Name1, sum(Meters1) as ProdMeters from " & Trim(Common_Procedures.EntryTempTable) & " group by Name1 Having sum(Meters1) <> 0", con)
        Dt1 = New DataTable
        Da1.Fill(Dt1)

        PavuConsMtrs = 0
        If Dt1.Rows.Count > 0 Then
            For i = 0 To Dt1.Rows.Count - 1

                vWidth_Type = Dt1.Rows(i).Item("Name1").ToString
                vClo_Mtrs = Val(Dt1.Rows(i).Item("ProdMeters").ToString)

                WidTyp = 0
                If Trim(UCase(vWidth_Type)) = "FOURTH" Then
                    WidTyp = 4
                ElseIf Trim(UCase(vWidth_Type)) = "TRIPLE" Then
                    WidTyp = 3
                ElseIf Trim(UCase(vWidth_Type)) = "DOUBLE" Then
                    WidTyp = 2
                Else
                    WidTyp = 1
                End If

                PavuConsMtrs = PavuConsMtrs + (Val(vClo_Mtrs) / Val(WidTyp))
                'PavuConsMtrs = PavuConsMtrs + ((Val(vClo_Mtrs) / Val(WidTyp)) * Val(NoofBeams))

            Next

        End If
        Dt1.Clear()

        CrmpPerc = 0
        If Val(lbl_TotMtrs1.Text) <> 0 Then
            CrmpPerc = Format((Val(lbl_TotMtrs1.Text) - Val(PavuConsMtrs)) / Val(lbl_TotMtrs1.Text) * 100, "#########0.00")
        End If

        ProdMtrs = Format(Val(PavuConsMtrs), "#########0.00")
        Calculation_CrimpPercentage1111 = Format(Val(CrmpPerc), "#########0.00")

    End Function

    Private Sub btn_SaveAll_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_SaveAll.Click
        Dim ProdMtrs As Double = 0

        SaveAll_STS = True

        LastNo = ""
        movelast_record()

        LastNo = lbl_RefNo.Text

        movefirst_record()

        Calculate_Crimp_Percentage()

        Timer1.Enabled = True

    End Sub

    Private Sub Timer1_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer1.Tick
        Dim ProdMtrs As Double = 0

        save_record()

        If Trim(UCase(LastNo)) = Trim(UCase(lbl_RefNo.Text)) Then
            Timer1.Enabled = False
            SaveAll_STS = False
            MessageBox.Show("All entries saved sucessfully", "FOR SAVING...", MessageBoxButtons.OK, MessageBoxIcon.Information)

        Else
            movenext_record()

            Calculate_Crimp_Percentage()

        End If
    End Sub

    Private Sub btn_Filter_Close_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Filter_Close.Click
        pnl_back.Enabled = True
        pnl_filter.Visible = False
        Filter_Status = False
    End Sub

    Private Sub cbo_Filter_BeamNo_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Filter_BeamNo.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Stock_SizedPavu_Processing_Details", "BeamNo_SetCode_forSelection", "", "(BeamNo_SetCode_forSelection = '')")
    End Sub

    Private Sub cbo_Filter_BeamNo_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Filter_BeamNo.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_BeamNo, cbo_Filter_LoomNo, btn_filtershow, "Stock_SizedPavu_Processing_Details", "BeamNo_SetCode_forSelection", "", "(BeamNo_SetCode_forSelection = '')")
    End Sub

    Private Sub cbo_Filter_BeamNo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Filter_BeamNo.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Filter_BeamNo, btn_filtershow, "Stock_SizedPavu_Processing_Details", "BeamNo_SetCode_forSelection", "", "(BeamNo_SetCode_forSelection = '')")
    End Sub
    Private Sub msk_Date_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles msk_Date.KeyDown
        vcbo_KeyDwnVal = e.KeyValue

        If e.KeyCode = 40 Then
            e.Handled = True : e.SuppressKeyPress = True
            cbo_Shift.Focus()
        End If

        If e.KeyCode = 38 Then
            e.Handled = True
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
            msk_date.Text = Date.Today
            msk_date.SelectionStart = 0
        End If
        If Asc(e.KeyChar) = 13 Then
            cbo_Shift.Focus()
            e.Handled = True
        End If
    End Sub

   
    Private Sub msk_Date_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles msk_Date.KeyUp
        Dim vmRetTxt As String = ""
        Dim vmRetSelStrt As Integer = -1

        If e.KeyCode = 107 Then
            msk_Date.Text = DateAdd("D", 1, Convert.ToDateTime(msk_Date.Text))
            msk_Date.SelectionStart = 0
        ElseIf e.KeyCode = 109 Then
            msk_Date.Text = DateAdd("D", -1, Convert.ToDateTime(msk_Date.Text))
            msk_Date.SelectionStart = 0
        End If

        If e.KeyCode = 46 Or e.KeyCode = 8 Then
            Common_Procedures.maskEdit_Date_ON_DelBackSpace(sender, e, vmskOldText, vmskSelStrt)
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

    Private Sub dtp_Date_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dtp_Date.TextChanged
        If IsDate(dtp_Date.Text) = True Then
            msk_Date.Text = dtp_Date.Text
            msk_Date.SelectionStart = 0
        End If
    End Sub
    Private Sub dtp_Date_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dtp_Date.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            dtp_Date.Text = Date.Today
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

    Private Sub dtp_Date_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dtp_Date.KeyPress
        If Asc(e.KeyChar) = 13 Then
            e.Handled = True
            msk_date.Focus()
        End If
    End Sub

    Private Sub txt_Stock_CrimpPerc_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txt_Stock_CrimpPerc.TextChanged
        lbl_Stock_CrimpMeters.Text = Format((Val(lbl_TotMtrs1.Text) + Val(lbl_TotMtrs2.Text)) * Val(txt_Stock_CrimpPerc.Text) / 100, "#########0.00")
    End Sub

    Private Sub btn_UserModification_Click(sender As Object, e As System.EventArgs) Handles btn_UserModification.Click
        If Val(Common_Procedures.User.IdNo) = 1 Then
            Dim f1 As New User_Modifications
            f1.Entry_Name = Me.Name
            f1.Entry_PkValue = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)
            f1.ShowDialog()
        End If
    End Sub

    Private Sub Chk_Beam2Close_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Chk_Beam2Close.KeyPress
        If Asc(e.KeyChar) = 13 Then
            If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                save_record()
            Else
                dtp_Date.Focus()
            End If
        End If
    End Sub

    Private Sub Chk_Beam2Close_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Chk_Beam2Close.Click
        Calculate_Crimp_Percentage()
    End Sub

    Private Sub Calculate_Crimp_Percentage()
        Dim ProdMtrs As Double = 0

        lbl_CrimpPerc1.Text = ""
        lbl_ProdMtrs1.Text = ""
        lbl_CrimpPerc2.Text = ""
        lbl_ProdMtrs2.Text = ""

        If Chk_BeamClose.Checked = True Then
            If Trim(lbl_SetCode1.Text) <> "" And Trim(lbl_BeamNo1.Text) <> "" Then
                lbl_CrimpPerc1.Text = Common_Procedures.Calculation_CrimpPercentage_On_BEAMRUNOUT(con, lbl_SetCode1.Text, lbl_BeamNo1.Text, Val(lbl_TotMtrs1.Text), ProdMtrs)
                lbl_ProdMtrs1.Text = Format(Val(ProdMtrs), "#########0.00")
            End If
        End If
        If Chk_Beam2Close.Checked = True Then
            If Trim(lbl_SetCode2.Text) <> "" And Trim(lbl_BeamNo2.Text) <> "" Then
                lbl_CrimpPerc2.Text = Common_Procedures.Calculation_CrimpPercentage_On_BEAMRUNOUT(con, lbl_SetCode2.Text, lbl_BeamNo2.Text, Val(lbl_TotMtrs2.Text), ProdMtrs)
                lbl_ProdMtrs2.Text = Format(Val(ProdMtrs), "#########0.00")
            End If

        End If

    End Sub

    Private Sub cbo_LoomNo_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbo_LoomNo.SelectedIndexChanged

    End Sub

    Private Sub lbl_CrimpPerc1_Click(sender As Object, e As EventArgs) Handles lbl_CrimpPerc1.Click

    End Sub
End Class