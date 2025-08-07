Public Class Bobin_Covering_Production_Wages_Master
    Implements Interface_MDIActions


    Private con As New SqlClient.SqlConnection(Common_Procedures.Connection_String)
    Private New_Entry As Boolean = False
    Private Prec_ActCtrl As New Control

    Public Sub clear()

        New_Entry = False
        txt_rate.Text = ""
        'lbl_idno.Text = ""
        'lbl_idno.ForeColor = Color.Black

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
            If TypeOf Prec_ActCtrl Is TextBox Or TypeOf Prec_ActCtrl Is ComboBox Then
                Prec_ActCtrl.BackColor = Color.White
                Prec_ActCtrl.ForeColor = Color.Black
            End If
        End If

    End Sub

    Private Sub Covering_Production_Rate_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown

    End Sub





    Private Sub Covering_Production_Rate_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        con.Open()

        AddHandler txt_rate.LostFocus, AddressOf ControlLostFocus


        AddHandler txt_rate.GotFocus, AddressOf ControlGotFocus


        move_record()
    End Sub

    Public Sub move_record()
        Dim cmd As New SqlClient.SqlCommand

        Dim da1 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable

        clear()

        Try

            cmd.Connection = con

            da1 = New SqlClient.SqlDataAdapter("select * from Covering_Production_Head", con)
            dt1 = New DataTable
            da1.Fill(dt1)


            If dt1.Rows.Count > 0 Then

                If IsDBNull(dt1.Rows(0).Item("Rate").ToString) = False Then
                    txt_rate.Text = dt1.Rows(0).Item("Rate").ToString()
                End If

            End If

            dt1.Dispose()
            da1.Dispose()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT MOVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally

            cmd.Dispose()

            '  If lbl_idno.Enabled And lbl_idno.Visible Then lbl_idno.Focus()

        End Try


    End Sub

    Public Sub delete_record() Implements Interface_MDIActions.delete_record
        Dim cmd As New SqlClient.SqlCommand

        If pnl_back.Enabled = False Then
            MessageBox.Show("Close other window", "DOES NOT SAVE", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        If MessageBox.Show("Do you want to Delete ?", "FOR DELETION....", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) <> Windows.Forms.DialogResult.Yes Then
            Exit Sub
        End If


        Try
            cmd.Connection = con
            cmd.CommandText = "delete from Covering_Production_Head "

            cmd.ExecuteNonQuery()

            new_record()

            MessageBox.Show("Deleted Sucessfully!!!", "FOR DELETION...", MessageBoxButtons.OK, MessageBoxIcon.Information)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally
            If txt_rate.Enabled And txt_rate.Visible Then txt_rate.Focus()

        End Try

    End Sub

    Public Sub filter_record() Implements Interface_MDIActions.filter_record

    End Sub

    Public Sub insert_record() Implements Interface_MDIActions.insert_record

    End Sub

    Public Sub movefirst_record() Implements Interface_MDIActions.movefirst_record
        '---
    End Sub

    Public Sub movelast_record() Implements Interface_MDIActions.movelast_record
        '--
    End Sub

    Public Sub movenext_record() Implements Interface_MDIActions.movenext_record
        '-----

    End Sub

    Public Sub moveprevious_record() Implements Interface_MDIActions.moveprevious_record
        '----------
    End Sub

    Public Sub new_record() Implements Interface_MDIActions.new_record

        'clear()

        'New_Entry = True
        'lbl_idno.ForeColor = Color.Red

        'lbl_idno.Text = Common_Procedures.get_MaxIdNo(con, "Covering_Production_Head", "Production_IdNo", "")

        'If txt_rate.Enabled And txt_rate.Visible Then txt_rate.Focus()
    End Sub

    Public Sub open_record() Implements Interface_MDIActions.open_record

    End Sub

    Public Sub print_record() Implements Interface_MDIActions.print_record

    End Sub

    Public Sub save_record() Implements Interface_MDIActions.save_record
        Dim cmd As New SqlClient.SqlCommand
        Dim trans As SqlClient.SqlTransaction

        If pnl_back.Enabled = False Then
            MessageBox.Show("Close other window", "DOES NOT SAVE", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If


        trans = con.BeginTransaction

        Try
            cmd.Connection = con
            cmd.Transaction = trans


            cmd.CommandText = "delete from Covering_Production_Head "
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Insert into Covering_Production_Head(Production_IdNo, Rate) values (1, '" & Trim(txt_rate.Text) & "')"
            cmd.ExecuteNonQuery()




            trans.Commit()

            MessageBox.Show("Saved Sucessfully!!!", "FOR SAVING....", MessageBoxButtons.OK, MessageBoxIcon.Information)

            If Val(Common_Procedures.settings.OnSave_MoveTo_NewEntry_Status) = 1 Then
                If New_Entry = True Then
                    new_record()
                Else
                    move_record()
                End If
            Else
                move_record()
            End If


        Catch ex As Exception
            trans.Rollback()

            If InStr(1, Trim(LCase(ex.Message)), "ix_Covering_Production_Head") > 0 Then
                MessageBox.Show("Duplicate Area Name", "DOES NOT SAVE", MessageBoxButtons.OK, MessageBoxIcon.Error)

            Else
                MessageBox.Show(ex.Message, "DOES NOT SAVE", MessageBoxButtons.OK, MessageBoxIcon.Error)

            End If
        Finally
            If txt_rate.Enabled And txt_rate.Visible Then txt_rate.Focus()


        End Try

    End Sub

    Private Sub btn_save_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_save.Click
        save_record()
    End Sub

    Private Sub btn_close_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_close.Click
        Me.Close()
    End Sub

    Private Sub Covering_Production_Rate_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        On Error Resume Next
        con.Close()
        con.Dispose()
        Common_Procedures.Last_Closed_FormName = Me.Name
    End Sub

    Private Sub txt_rate_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_rate.KeyDown
        On Error Resume Next
        If e.KeyValue = 38 Then e.Handled = True : SendKeys.Send("+{TAB}")
        If e.KeyValue = 40 Then e.Handled = True : SendKeys.Send("{TAB}")
    End Sub


    Private Sub txt_rate_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_rate.KeyPress
        On Error Resume Next
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
            e.Handled = True
        End If
        If Asc(e.KeyChar) = 13 Then e.Handled = True : SendKeys.Send("{TAB}")
    End Sub

    Private Sub Covering_Production_Rate_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If Asc(e.KeyChar) = 27 Then
            Me.Close()
        End If
    End Sub
End Class