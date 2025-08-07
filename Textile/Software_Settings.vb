Public Class Software_Settings
    Implements Interface_MDIActions

    Dim cn1 As New SqlClient.SqlConnection(Common_Procedures.Connection_String)
    Dim cn2 As New SqlClient.SqlConnection(Common_Procedures.ConnectionString_CompanyGroupdetails)
    Dim New_Entry As Boolean
    Private Prec_ActCtrl As New Control

    Private Sub clear()
        'Me.Height = 335  ' 327
        pnl_back.Enabled = True

        txt_CompanyName.Text = ""
        txt_CustomerCode.Text = ""
        cbo_Company_Software.Text = ""
        cbo_CompanyGroup_Software.Text = ""
        New_Entry = False
    End Sub

    Private Sub ControlGotFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim txtbx As TextBox
        Dim combobx As ComboBox

        On Error Resume Next

        If TypeOf Me.ActiveControl Is TextBox Or TypeOf Me.ActiveControl Is ComboBox Then
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
            Prec_ActCtrl.BackColor = Color.White
            Prec_ActCtrl.ForeColor = Color.Black
        End If
    End Sub

    Private Sub TextBoxControlKeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs)
        On Error Resume Next
        If e.KeyValue = 38 Then e.Handled = True : SendKeys.Send("+{TAB}")
        If e.KeyValue = 40 Then e.Handled = True : SendKeys.Send("{TAB}")
    End Sub

    Private Sub TextBoxControlKeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
        On Error Resume Next
        If Asc(e.KeyChar) = 13 Then e.Handled = True : SendKeys.Send("{TAB}")
    End Sub

    Public Sub delete_record() Implements Interface_MDIActions.delete_record
        '----------------
    End Sub

    Public Sub filter_record() Implements Interface_MDIActions.filter_record
        '--------------
    End Sub

    Public Sub insert_record() Implements Interface_MDIActions.insert_record
        '-----
    End Sub

    Public Sub move_record()
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim dt1 As New DataTable

        clear()

        da = New SqlClient.SqlDataAdapter("select * from settings_head ", cn1)
        dt = New DataTable
        da.Fill(dt)
        If dt.Rows.Count > 0 Then
            If IsDBNull(dt.Rows(0).Item("C_Name").ToString) = False Then
                txt_CustomerCode.Text = dt.Rows(0).Item("C_Name").ToString
            End If
            If IsDBNull(dt.Rows(0).Item("S_Name").ToString) = False Then
                cbo_CompanyGroup_Software.Text = dt.Rows(0).Item("S_Name").ToString
            End If
        End If
        dt.Clear()

        da = New SqlClient.SqlDataAdapter("select * from settings_head ", cn2)
        dt1 = New DataTable
        da.Fill(dt1)
        If dt1.Rows.Count > 0 Then
            If IsDBNull(dt1.Rows(0).Item("Cc_No").ToString) = False Then
                txt_CompanyName.Text = dt1.Rows(0).Item("Cc_No").ToString
            End If
            If IsDBNull(dt1.Rows(0).Item("s_name").ToString) = False Then
                cbo_Company_Software.Text = dt1.Rows(0).Item("s_name").ToString
            End If
        End If
        dt1.Clear()

        dt.Dispose()
        dt1.Dispose()
        da.Dispose()

        If txt_CustomerCode.Enabled And txt_CustomerCode.Visible Then txt_CustomerCode.Focus()

    End Sub

    Public Sub movefirst_record() Implements Interface_MDIActions.movefirst_record
        '--------------
    End Sub

    Public Sub movelast_record() Implements Interface_MDIActions.movelast_record
        '--------------
    End Sub

    Public Sub moveprevious_record() Implements Interface_MDIActions.moveprevious_record
        '---------------
    End Sub

    Public Sub movenext_record() Implements Interface_MDIActions.movenext_record
        '---------------
    End Sub

    Public Sub new_record() Implements Interface_MDIActions.new_record
        move_record()
        If txt_CustomerCode.Enabled And txt_CustomerCode.Visible Then txt_CustomerCode.Focus()
    End Sub

    Public Sub open_record() Implements Interface_MDIActions.open_record
        '-------
    End Sub

    Public Sub print_record() Implements Interface_MDIActions.print_record
        '-------
    End Sub

    Public Sub save_record() Implements Interface_MDIActions.save_record
        Dim trans As SqlClient.SqlTransaction
        Dim trans1 As SqlClient.SqlTransaction
        Dim cmd As New SqlClient.SqlCommand
        Dim cmd1 As New SqlClient.SqlCommand

        If pnl_back.Enabled = False Then
            MessageBox.Show("Close Other Windows", "DOES NOT SAVE", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        trans = cn1.BeginTransaction
        Try

            cmd.Connection = cn1
            cmd.Transaction = trans

            If New_Entry = True Then
                cmd.CommandText = "Insert into Settings_Head( C_Name, S_Name) values( '" & Trim(txt_CustomerCode.Text) & "', '" & Trim(cbo_CompanyGroup_Software.Text) & "')"
                cmd.ExecuteNonQuery()

            Else
                cmd.CommandText = "update Settings_Head set  S_Name='" & Trim(cbo_CompanyGroup_Software.Text) & "' where C_Name = '" & Trim(txt_CustomerCode.Text) & "'"
                cmd.ExecuteNonQuery()

            End If

            trans.Commit()


        Catch ex As Exception
            trans.Rollback()
            MessageBox.Show(ex.Message, "DOES NOT SAVE", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub

        End Try

        trans1 = cn1.BeginTransaction
        Try

            cmd.Connection = cn2
            cmd.Transaction = trans1

            cmd.CommandText = "Insert into Settings_Head( Cc_No, S_Name) values( '" & Trim(txt_CompanyName.Text) & "', '" & Trim(cbo_Company_Software.Text) & "')"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "update Settings_Head set  S_Name='" & Trim(cbo_Company_Software.Text) & "' where Cc_No = '" & Trim(txt_CompanyName.Text) & "'"
            cmd.ExecuteNonQuery()

            trans1.Commit()

            move_record()

            MessageBox.Show("Saved Sucessfully!!!", "FOR SAVING....", MessageBoxButtons.OK, MessageBoxIcon.Information)

        Catch ex As Exception
            trans1.Rollback()
            MessageBox.Show(ex.Message, "DOES NOT SAVE", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally

            trans.Dispose()
            trans1.Dispose()
            cmd.Dispose()

            If txt_CustomerCode.Enabled And txt_CustomerCode.Visible Then txt_CustomerCode.Focus()

        End Try

    End Sub

    Private Sub Software_Settings_creation_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim dt1 As New DataTable

        cn1.Open()
        cn2.Open()

        cbo_Company_Software.Items.Clear()
        cbo_Company_Software.Items.Add("")
        cbo_Company_Software.Items.Add("TEXTILE")
        cbo_Company_Software.Items.Add("FP")
        cbo_Company_Software.Items.Add("JOBWORK")
        cbo_Company_Software.Items.Add("TEXTILE & JOBWORK")

        cbo_CompanyGroup_Software.Items.Clear()
        cbo_CompanyGroup_Software.Items.Add("")
        cbo_CompanyGroup_Software.Items.Add("TEXTILE")
        cbo_CompanyGroup_Software.Items.Add("FP")
        cbo_CompanyGroup_Software.Items.Add("JOBWORK")
        cbo_CompanyGroup_Software.Items.Add("TEXTILE & JOBWORK")

        AddHandler cbo_Company_Software.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_CompanyGroup_Software.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_CompanyName.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_CustomerCode.GotFocus, AddressOf ControlGotFocus

        AddHandler txt_CompanyName.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_CustomerCode.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Company_Software.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_CompanyGroup_Software.LostFocus, AddressOf ControlLostFocus

        AddHandler txt_CompanyName.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_CustomerCode.KeyDown, AddressOf TextBoxControlKeyDown

        AddHandler txt_CompanyName.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_CustomerCode.KeyPress, AddressOf TextBoxControlKeyPress

        new_record()

    End Sub

    Private Sub Software_Settings_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        cn1.Close()
        cn1.Dispose()
        cn2.Close()
        cn2.Dispose()
    End Sub

    Private Sub Software_Settings_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If Asc(e.KeyChar) = 27 Then
            Me.Close()
        End If
    End Sub

    Private Sub btn_Save_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_save.Click
        save_record()
    End Sub

    Private Sub btn_Close_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_close.Click
        Me.Close()
    End Sub

    Private Sub cbo_CompanyGroup_Software_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_CompanyGroup_Software.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, cn1, cbo_CompanyGroup_Software, txt_CustomerCode, txt_CompanyName, "", "", "", "")
    End Sub

    Private Sub cbo_CompanyGroup_Software_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_CompanyGroup_Software.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, cn1, cbo_CompanyGroup_Software, txt_CompanyName, "", "", "", "")
    End Sub

    Private Sub cbo_Company_Software_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Company_Software.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, cn2, cbo_Company_Software, txt_CompanyName, Nothing, "", "", "", "")
        If (e.KeyValue = 40 And cbo_Company_Software.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
            If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                save_record()
            Else
                txt_CustomerCode.Focus()
            End If
        End If
    End Sub

    Private Sub cbo_Company_Software_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Company_Software.KeyPress

        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, cn2, cbo_Company_Software, Nothing, "", "", "", "")


        If Asc(e.KeyChar) = 13 Then
            If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                save_record()
            Else
                txt_CustomerCode.Focus()
            End If
        End If
    End Sub

End Class