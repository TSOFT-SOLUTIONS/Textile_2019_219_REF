Public Class Mail_Settings
    Implements Interface_MDIActions

    Private con As New SqlClient.SqlConnection(Common_Procedures.Connection_String)
    Private New_Entry As Boolean = False
    Private Prev_ActCtrl As New Control


    Private Sub clear()
        New_Entry = True

        txt_MailID.Text = ""
        txt_Mail_Pwd.Text = ""
        cbo_Mail_Host.Text = "smtp.gmail.com"
        txt_Mail_Port.Text = "587"
    End Sub


    Private Sub ControlGotFocus(ByVal sender As Object, ByVal e As EventArgs)
        Dim txtbox As TextBox
        Dim cbox As ComboBox
        On Error Resume Next

        Me.ActiveControl.BackColor = Color.Lime
        Me.ActiveControl.ForeColor = Color.Blue

        If TypeOf Me.ActiveControl Is TextBox Then
            txtbox = Me.ActiveControl
            txtbox.SelectAll()
        ElseIf TypeOf Me.ActiveControl Is ComboBox Then
            cbox = Me.ActiveControl
            cbox.SelectAll()
        End If


        Prev_ActCtrl = Me.ActiveControl

    End Sub

    Private Sub ControlLostFocus(ByVal sender As Object, ByVal e As EventArgs)
        On Error Resume Next

        If IsDBNull(Prev_ActCtrl) = False Then
            If TypeOf Prev_ActCtrl Is TextBox Or TypeOf Prev_ActCtrl Is ComboBox Then
                Prev_ActCtrl.BackColor = Color.White
                Prev_ActCtrl.ForeColor = Color.Black
            End If
        End If

    End Sub

    Private Sub TextboxControlKeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs)
        On Error Resume Next
        If e.KeyValue = 40 Then SendKeys.Send("{TAB}")
        If e.KeyValue = 38 Then SendKeys.Send("+{TAB}")
    End Sub

    Private Sub TextControlKeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
        If Asc(e.KeyChar) = 13 Then SendKeys.Send("{TAB}")
    End Sub

    Private Sub move_record()
        Dim da As SqlClient.SqlDataAdapter
        Dim dt As DataTable

        clear()

        New_Entry = False

        Try

            da = New SqlClient.SqlDataAdapter("SELECT top 1 * FROM Mail_Settings_Head Order by Setting_Idno", con)
            dt = New DataTable
            da.Fill(dt)

            If dt.Rows.Count > 0 Then
                txt_MailID.Text = dt.Rows(0).Item("EMail_Id").ToString
                txt_Mail_Pwd.Text = dt.Rows(0).Item("EMail_pwd").ToString
                cbo_Mail_Host.Text = dt.Rows(0).Item("EMail_Host").ToString
                txt_Mail_Port.Text = dt.Rows(0).Item("EMail_Port").ToString
            End If

        Catch ex As Exception
            '-----

        End Try

        If txt_MailID.Enabled And txt_MailID.Visible Then txt_MailID.Focus()

    End Sub

    Public Sub delete_record() Implements Interface_MDIActions.delete_record
        Dim CMD As New SqlClient.SqlCommand

        If txt_MailID.Text = "" Then
            MessageBox.Show("Could not delete this entry ?", "DOES NOT DELETE", MessageBoxButtons.OK, MessageBoxIcon.Error)
            txt_MailID.SelectAll()
            Exit Sub
        End If
        If cbo_Mail_Host.Text = "" Then
            MessageBox.Show("Could not delete this entry ?", "DOES NOT DELETE", MessageBoxButtons.OK, MessageBoxIcon.Error)
            cbo_Mail_Host.SelectAll()
            Exit Sub
        End If

        If MessageBox.Show("Do you want to Delete?", "FOR DELETE", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.No Then
            Exit Sub
        End If
        CMD.Connection = con

        CMD.CommandText = "DELETE FROM Mail_Settings_Head "
        CMD.ExecuteNonQuery()

        MessageBox.Show("Deleted Successfully", "FOR DELETE", MessageBoxButtons.OK, MessageBoxIcon.Information)

        clear()
        If txt_MailID.Enabled And txt_MailID.Visible Then txt_MailID.SelectAll()


    End Sub

    Public Sub filter_record() Implements Interface_MDIActions.filter_record
        '................
    End Sub

    Public Sub insert_record() Implements Interface_MDIActions.insert_record
        '................
    End Sub

    Public Sub movefirst_record() Implements Interface_MDIActions.movefirst_record
        move_record()
    End Sub

    Public Sub movelast_record() Implements Interface_MDIActions.movelast_record
        move_record()
    End Sub

    Public Sub movenext_record() Implements Interface_MDIActions.movenext_record
        move_record()
    End Sub

    Public Sub moveprevious_record() Implements Interface_MDIActions.moveprevious_record
        move_record()
    End Sub

    Public Sub new_record() Implements Interface_MDIActions.new_record
        '----
        txt_MailID.Focus()
    End Sub

    Public Sub open_record() Implements Interface_MDIActions.open_record
        '................
    End Sub

    Public Sub print_record() Implements Interface_MDIActions.print_record
        '................
    End Sub


    Public Sub save_record() Implements Interface_MDIActions.save_record
        Dim tr As SqlClient.SqlTransaction
        Dim cmd As New SqlClient.SqlCommand


        If Trim(txt_MailID.Text) = "" Then
            MessageBox.Show("Invalid Mail-ID ?", "DOES NOT SAVE", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            txt_MailID.SelectAll()
            txt_MailID.Focus()
            Exit Sub
        End If
        If Trim(txt_Mail_Pwd.Text) = "" Then
            MessageBox.Show("Invalid Mail Password ?", "DOES NOT SAVE", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            txt_Mail_Pwd.SelectAll()
            txt_Mail_Pwd.Focus()
            Exit Sub
        End If
        If Trim(cbo_Mail_Host.Text) = "" Then
            MessageBox.Show("Invalid Mail Host ?", "DOES NOT SAVE", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            cbo_Mail_Host.SelectAll()
            cbo_Mail_Host.Focus()
            Exit Sub
        End If
        If Trim(txt_Mail_Port.Text) = "" Then
            MessageBox.Show("Invalid Mail Port ?", "DOES NOT SAVE", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            txt_Mail_Port.SelectAll()
            txt_Mail_Port.Focus()
            Exit Sub
        End If



        tr = con.BeginTransaction
        Try
            cmd.Connection = con
            cmd.Transaction = tr

            cmd.CommandText = "Delete from Mail_Settings_Head "
            cmd.ExecuteNonQuery()

            cmd.CommandText = "INSERT INTO Mail_Settings_Head (               EMail_Id         ,              EMail_pwd           ,              EMail_Host           ,              EMail_Port            ) " & _
                                                       " VALUES  ( '" & Trim(txt_MailID.Text) & "', '" & Trim(txt_Mail_Pwd.Text) & "', '" & Trim(cbo_Mail_Host.Text) & "', " & Str(Val(txt_Mail_Port.Text)) & " ) "
            cmd.ExecuteNonQuery()

            MessageBox.Show("Saved Successfully", "FOR SAVE", MessageBoxButtons.OK, MessageBoxIcon.Information)

            Common_Procedures.settings.Email_Address = Trim(txt_MailID.Text)  ' "tsoft.tirupur@gmail.com"
            Common_Procedures.settings.Email_Password = Trim(txt_Mail_Pwd.Text) ' "GOLD@tn39av7417"
            Common_Procedures.settings.Email_Host = Trim(cbo_Mail_Host.Text)
            Common_Procedures.settings.Email_Port = Val(txt_Mail_Port.Text)

            tr.Commit()

        Catch ex As Exception
            tr.Rollback()

        End Try

        If txt_MailID.Enabled And txt_MailID.Visible Then txt_MailID.SelectAll()

    End Sub

    Private Sub Mail_and_SMS_Settings_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        con.Dispose()
        con.Close()
    End Sub

    Private Sub Mail_and_SMS_Settings_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If Asc(e.KeyChar) = 27 Then
            If MessageBox.Show("Do you want to Close ?", "FOR CLOSE", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                Me.Close()
            Else
                txt_MailID.Focus()
            End If
        End If
    End Sub

    Private Sub Mail_and_SMS_Settings_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        con.Open()

        cbo_Mail_Host.Items.Clear()
        cbo_Mail_Host.Items.Add("")
        cbo_Mail_Host.Items.Add("smtp.gmail.com")
        cbo_Mail_Host.Items.Add("smtp.mail.yahoo.com")
        cbo_Mail_Host.Items.Add("Smtp.live.com")
        cbo_Mail_Host.Items.Add("smtpout.secureserver.net")

        AddHandler cbo_Mail_Host.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Mail_Pwd.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_MailID.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Mail_Port.GotFocus, AddressOf ControlGotFocus

        AddHandler cbo_Mail_Host.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Mail_Pwd.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_MailID.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Mail_Port.LostFocus, AddressOf ControlLostFocus

        AddHandler txt_Mail_Pwd.KeyDown, AddressOf TextboxControlKeyDown
        AddHandler txt_MailID.KeyDown, AddressOf TextboxControlKeyDown
        AddHandler txt_Mail_Port.KeyDown, AddressOf TextboxControlKeyDown

        AddHandler txt_Mail_Pwd.KeyPress, AddressOf TextControlKeyPress
        AddHandler txt_MailID.KeyPress, AddressOf TextControlKeyPress
        AddHandler txt_Mail_Port.KeyPress, AddressOf TextControlKeyPress


        move_record()


    End Sub

    Private Sub txt_SenderID_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)

    End Sub

    Private Sub btn_Mail_Save_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Mail_Save.Click
        save_record()
    End Sub

    Private Sub btn_Mail_Close_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Mail_Close.Click
        Me.Close()
    End Sub

    Private Sub txt_Mail_Port_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Mail_Port.KeyPress
        If Asc(e.KeyChar) = 13 Then
            If MessageBox.Show("Do you want to Save?", "FOR SAVE", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Information) = Windows.Forms.DialogResult.Yes Then
                save_record()
            Else
                txt_MailID.Focus()
            End If
        End If
    End Sub

    Private Sub cbo_Mail_Host_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Mail_Host.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "", "", "", "")
    End Sub

    Private Sub cbo_Mail_Host_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Mail_Host.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Mail_Host, txt_Mail_Pwd, txt_Mail_Port, "", "", "", "")
    End Sub

    Private Sub cbo_Mail_Host_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Mail_Host.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Mail_Host, txt_Mail_Port, "", "", "", "", False)
    End Sub


End Class