Public Class Beam_Close
    Implements Interface_MDIActions

    Private con As New SqlClient.SqlConnection(Common_Procedures.Connection_String)
    Private con1 As New SqlClient.SqlConnection(Common_Procedures.ConnectionString_CompanyGroupdetails)
    Private FrmLdSTS As Boolean = False
    Private New_Entry As Boolean = False
    Private Insert_Entry As Boolean = False
    Private Filter_Status As Boolean = False
    Private Prec_ActCtrl As New Control

    Public Sub New()
        ' This call is required by the designer.
        InitializeComponent()
        ' Add any initialization after the InitializeComponent() call.
    End Sub

    Private Sub clear()
        New_Entry = False
        Insert_Entry = False
        pnl_back.Enabled = True
      
        cbo_SetNo.Text = ""
        cbo_SetNo.Tag = ""
        lbl_TotMtrs.Text = ""
        cbo_BeamNo.Text = ""
        cbo_BeamNo.Tag = ""
        lbl_PartyName.Text = ""
        lbl_BalMtrs.Text = ""
        chk_BeamClose.Checked = False
        lbl_UserName.Text = "USER : " & Trim(UCase(Common_Procedures.User_IdNoToName(con1, Common_Procedures.User.IdNo)))
    End Sub

    Private Sub ControlGotFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim txtbx As TextBox
        Dim combobx As ComboBox
        On Error Resume Next

        If TypeOf Me.ActiveControl Is TextBox Or TypeOf Me.ActiveControl Is ComboBox Or TypeOf Me.ActiveControl Is Button Or TypeOf Me.ActiveControl Is CheckBox Then
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

        Prec_ActCtrl = Me.ActiveControl

    End Sub

    Private Sub ControlLostFocus(ByVal sender As Object, ByVal e As System.EventArgs)

        On Error Resume Next

        If IsNothing(Prec_ActCtrl) = False Then
            If TypeOf Prec_ActCtrl Is TextBox Or TypeOf Prec_ActCtrl Is ComboBox Or TypeOf Prec_ActCtrl Is CheckBox Then
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
    
    Private Sub Beam_Close_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated

        Try

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

    Private Sub Beam_Close_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt2 As New DataTable
        Dim dt3 As New DataTable

        Me.Text = ""

        con.Open()

        Da = New SqlClient.SqlDataAdapter("select Beam_No from Stock_SizedPavu_Processing_Details order by ForOrderBy_BeamNo, Beam_No", con)
        Da.Fill(Dt2)
        cbo_BeamNo.DataSource = Dt2
        cbo_BeamNo.DisplayMember = "Beam_No"

        Da = New SqlClient.SqlDataAdapter("select setcode_forSelection from Stock_SizedPavu_Processing_Details order by setcode_forSelection", con)
        Da.Fill(dt3)
        cbo_SetNo.DataSource = dt3
        cbo_SetNo.DisplayMember = "setcode_forSelection"


        AddHandler cbo_SetNo.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_BeamNo.GotFocus, AddressOf ControlGotFocus
        AddHandler chk_BeamClose.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_save.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_close.GotFocus, AddressOf ControlGotFocus

        AddHandler cbo_SetNo.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_BeamNo.LostFocus, AddressOf ControlLostFocus
        AddHandler chk_BeamClose.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_save.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_close.LostFocus, AddressOf ControlLostFocus

        lbl_Company.Text = ""
        lbl_Company.Tag = 0
        lbl_Company.Visible = False
        Common_Procedures.CompIdNo = 0

        Filter_Status = False
        FrmLdSTS = True
        new_record()

    End Sub


    Private Sub Beam_Close_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        On Error Resume Next
        con.Close()
        con.Dispose()
        Common_Procedures.Last_Closed_FormName = Me.Name
    End Sub

    Private Sub Beam_Close_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress

        Try

            If Asc(e.KeyChar) = 27 Then
                If MessageBox.Show("Do you want to Close?", "FOR CLOSING ENTRY...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) <> Windows.Forms.DialogResult.Yes Then
                    Exit Sub

                Else
                    Close_Form()


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
       
        'If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.Inhouse_Beam_Close_Entry, "~L~") = 0 And InStr(Common_Procedures.UR.Inhouse_Beam_Close_Entry, "~D~") = 0 Then MessageBox.Show("You have No Rights to Delete", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error) : Exit Sub

        If MessageBox.Show("Do you want to Delete?", "FOR DELETION...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) <> Windows.Forms.DialogResult.Yes Then
            Exit Sub
        End If

        If New_Entry = True Then
            MessageBox.Show("This is New Entry", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        tr = con.BeginTransaction

        Try

            cmd.Connection = con
            cmd.Transaction = tr

            'cmd.CommandText = "delete from Stock_SizedPavu_Processing_Details where company_idno = " & Str(Val(lbl_Company.Tag)) & " and setcode_forSelection = '" & Trim(cbo_SetNo.Text) & "' AND Beam_No = '" & Trim(cbo_BeamNo.Text) & "'"
            cmd.ExecuteNonQuery()

            tr.Commit()

            new_record()

            MessageBox.Show("Deleted Sucessfully!!!", "FOR DELETION...", MessageBoxButtons.OK, MessageBoxIcon.Information)

        Catch ex As Exception

            tr.Rollback()
            MessageBox.Show(ex.Message, "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        If cbo_SetNo.Enabled = True And cbo_SetNo.Visible = True Then cbo_SetNo.Focus()

    End Sub

    Public Sub filter_record() Implements Interface_MDIActions.filter_record
        '-----
    End Sub

    Public Sub insert_record() Implements Interface_MDIActions.insert_record
        '-----
    End Sub

    Public Sub movefirst_record() Implements Interface_MDIActions.movefirst_record
        '-----
    End Sub

    Public Sub movelast_record() Implements Interface_MDIActions.movelast_record
        '-----
    End Sub

    Public Sub movenext_record() Implements Interface_MDIActions.movenext_record
        '-----
    End Sub

    Public Sub moveprevious_record() Implements Interface_MDIActions.moveprevious_record
        '-----
    End Sub

    Public Sub new_record() Implements Interface_MDIActions.new_record
        clear()
        If cbo_SetNo.Enabled And cbo_SetNo.Visible Then cbo_SetNo.Focus()
    End Sub

    Public Sub open_record() Implements Interface_MDIActions.open_record
        '------ 
    End Sub

    Public Sub print_record() Implements Interface_MDIActions.print_record
        '------ No Print
    End Sub

    Public Sub save_record() Implements Interface_MDIActions.save_record
        Dim cmd As New SqlClient.SqlCommand
        Dim tr As SqlClient.SqlTransaction
        Dim Nr As Long = 0
        Dim Clo_sts As Integer

        ' If Common_Procedures.UserRight_Check(Common_Procedures.UR.Inhouse_Beam_Close_Entry, New_Entry) = False Then Exit Sub
    

        If pnl_back.Enabled = False Then
            MessageBox.Show("Close all other windows", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        If Val(lbl_Company.Tag) = 0 Then
            MessageBox.Show("Invalid Company Selection", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        If Trim(cbo_SetNo.Text) = "" Then
            MessageBox.Show("Invalid Set No", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If cbo_SetNo.Enabled Then cbo_SetNo.Focus()
            Exit Sub
        End If


        If Trim(cbo_BeamNo.Text) = "" Then
            MessageBox.Show("Invalid Beam No", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If cbo_BeamNo.Enabled Then cbo_BeamNo.Focus()
            Exit Sub
        End If

        lbl_UserName.Text = Common_Procedures.User.IdNo
        Clo_STS = 0
        If chk_BeamClose.Checked = True Then Clo_sts = 1
       
        tr = con.BeginTransaction

        Try
            
            cmd.Connection = con
            cmd.Transaction = tr

            Nr = 0
            cmd.CommandText = "Update Stock_SizedPavu_Processing_Details set Close_Status = " & Str(Clo_sts) & " Where setcode_forSelection = '" & Trim(cbo_SetNo.Text) & "' and Beam_No = '" & Trim(cbo_BeamNo.Text) & "' and Beam_Knotting_Code = '' and Loom_Idno = 0"
            Nr = cmd.ExecuteNonQuery()
            If Nr = 0 Then
                Throw New ApplicationException("this beam is knotted, so u cannot close/un close this beam")
                Exit Sub
            End If

            tr.Commit()

            If New_Entry = True Then
                'move_record(lbl_RollNo.Text)
            End If

            MessageBox.Show("Saved Sucessfully!!!", "FOR SAVING...", MessageBoxButtons.OK, MessageBoxIcon.Information)

        Catch ex As Exception
            tr.Rollback()
            MessageBox.Show(ex.Message, "FOR SAVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally

            cmd.Dispose()
            tr.Dispose()

            If cbo_SetNo.Enabled And cbo_SetNo.Visible Then cbo_SetNo.Focus()


        End Try

    End Sub

    Private Sub cbo_BeamNo_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_BeamNo.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Stock_SizedPavu_Processing_Details", "Beam_No", "(setcode_forSelection = '" & Trim(cbo_SetNo.Text) & "')", "(Beam_No = '')")
    End Sub

    Private Sub cbo_BeamNo_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_BeamNo.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_BeamNo, cbo_SetNo, chk_BeamClose, "Stock_SizedPavu_Processing_Details", "Beam_No", "(setcode_forSelection = '" & Trim(cbo_SetNo.Text) & "')", "(Beam_No = '')")
    End Sub

    Private Sub cbo_BeamNo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_BeamNo.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_BeamNo, Nothing, "Stock_SizedPavu_Processing_Details", "Beam_No", "(setcode_forSelection = '" & Trim(cbo_SetNo.Text) & "')", "(Beam_No = '')")
        If Asc(e.KeyChar) = 13 Then
            btn_Selection_Click(sender, e)
            chk_BeamClose.Focus()
        End If
    End Sub

    Private Sub cbo_SetNo_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_SetNo.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Stock_SizedPavu_Processing_Details", "setcode_forSelection", "(company_idno = " & Str(Val(lbl_Company.Tag)) & ")", "(setcode_forSelection = '')")
    End Sub

    Private Sub cbo_SetNo_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_SetNo.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_SetNo, Nothing, cbo_BeamNo, "Stock_SizedPavu_Processing_Details", "setcode_forSelection", "(company_idno = " & Str(Val(lbl_Company.Tag)) & ")", "(setcode_forSelection = '')")
    End Sub

    Private Sub cbo_SetNo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_SetNo.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_SetNo, cbo_BeamNo, "Stock_SizedPavu_Processing_Details", "setcode_forSelection", "(company_idno = " & Str(Val(lbl_Company.Tag)) & ")", "(setcode_forSelection = '')")
    End Sub

    Private Sub chk_BeamClose_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles chk_BeamClose.KeyDown
        If e.KeyValue = 38 Then
            cbo_BeamNo.Focus() ' SendKeys.Send("+{TAB}")
        End If
        If e.KeyValue = 40 Then btn_close.Focus() ' SendKeys.Send("{TAB}")
    End Sub

    Private Sub chk_BeamClose_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles chk_BeamClose.KeyPress
        If Asc(e.KeyChar) = 13 Then
            If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                save_record()
            Else
                cbo_SetNo.Focus()
            End If
        End If
    End Sub

    Private Sub btn_save_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_save.Click
        save_record()
    End Sub

    Private Sub btn_close_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_close.Click
        Me.Close()
    End Sub

    Private Sub btn_Selection_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Selection.Click
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable

        If Trim(cbo_SetNo.Text) = "" Then
            MessageBox.Show("Invalid Set No", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If cbo_SetNo.Enabled Then cbo_SetNo.Focus()
            Exit Sub
        End If

        If Trim(cbo_BeamNo.Text) = "" Then
            MessageBox.Show("Invalid Beam No", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If cbo_BeamNo.Enabled Then cbo_BeamNo.Focus()
            Exit Sub
        End If

        If Trim(cbo_BeamNo.Text) <> "" And Trim(cbo_SetNo.Text) <> "" Then

            If Trim(UCase(cbo_BeamNo.Text)) <> Trim(UCase(cbo_BeamNo.Tag)) Or Trim(UCase(cbo_SetNo.Text)) <> Trim(UCase(cbo_SetNo.Tag)) Then

                Da = New SqlClient.SqlDataAdapter("select a.*, b.Ledger_Name from Stock_SizedPavu_Processing_Details a LEFT OUTER JOIN Ledger_Head b ON a.StockAt_IdNo = b.Ledger_IdNo where a.Beam_no = '" & Trim(cbo_BeamNo.Text) & "' and a.setcode_forSelection = '" & Trim(cbo_SetNo.Text) & "'", con)
                Dt = New DataTable
                Da.Fill(Dt)
                If Dt.Rows.Count > 0 Then
                    If IsDBNull(Dt.Rows(0).Item("Ledger_Name").ToString) = False Then
                        lbl_PartyName.Text = Dt.Rows(0).Item("Ledger_Name").ToString
                    End If
                    lbl_TotMtrs.Text = Dt.Rows(0).Item("Meters").ToString
                    lbl_BalMtrs.Text = Format(Val(Dt.Rows(0).Item("Meters").ToString) - Val(Dt.Rows(0).Item("Production_Meters").ToString), "#########0.00")
                    chk_BeamClose.Checked = False
                    If IsDBNull(Dt.Rows(0).Item("Close_Status").ToString) = False Then
                        If Val(Dt.Rows(0).Item("Close_Status").ToString) = 1 Then
                            chk_BeamClose.Checked = True
                        End If
                    End If
                End If

                Dt.Clear()
                Dt.Dispose()
                Da.Dispose()

                cbo_SetNo.Tag = cbo_SetNo.Text
                cbo_BeamNo.Tag = cbo_BeamNo.Text

                If chk_BeamClose.Enabled And chk_BeamClose.Visible Then chk_BeamClose.Focus()

            End If

        End If

    End Sub

End Class