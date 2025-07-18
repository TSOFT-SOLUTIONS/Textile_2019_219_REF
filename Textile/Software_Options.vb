Public Class Software_Options
    Private con As New SqlClient.SqlConnection(Common_Procedures.ConnectionString_CompanyGroupdetails)

    Private Sub Software_Options_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        On Error Resume Next
        con.Close()
        con.Dispose()

    End Sub

    Private Sub Software_Options_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        con.Open()


        txt_AutoBakServer.Text = ""
        txt_AutoBakClient1.Text = ""
        txt_AutoBakClient2.Text = ""

        move_record()

    End Sub

    Public Sub move_record()

        Dim da1 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable

        Try
            da1 = New SqlClient.SqlDataAdapter("select * from Settings_Head", con)
            dt1 = New DataTable
            da1.Fill(dt1)

            If dt1.Rows.Count > 0 Then

                txt_AutoBakServer.Text = dt1.Rows(0).Item("Autobackup_Path_Server").ToString()
                txt_AutoBakClient1.Text = dt1.Rows(0).Item("Autobackup_Path_Client1").ToString()
                txt_AutoBakClient2.Text = dt1.Rows(0).Item("Autobackup_Path_Client2").ToString()

            End If
            dt1.Dispose()
            da1.Dispose()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT MOVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally

        End Try

    End Sub

    Public Sub save_record()

        Dim cmd As New SqlClient.SqlCommand
        Dim trans As SqlClient.SqlTransaction
        Dim CC_Leng As Integer = 0

        'If Trim(txt_AutoBakServer.Text) = "" Then
        '    MessageBox.Show("Invalid CC_No", "DOES NOT SAVE", MessageBoxButtons.OK, MessageBoxIcon.Error)
        '    Exit Sub
        'End If

        'CC_Leng = Len(Trim(txt_AutoBakServer.Text))

        'If Val(CC_Leng) <> 4 Then
        '    MessageBox.Show("Invalid CC_No", "DOES NOT SAVE", MessageBoxButtons.OK, MessageBoxIcon.Error)
        '    Exit Sub
        'End If

        trans = con.BeginTransaction

        Try
            cmd.Connection = con
            cmd.Transaction = trans

            cmd.CommandText = "update Settings_Head set Autobackup_Path_Server = '" & Trim(txt_AutoBakServer.Text) & "', Autobackup_Path_Client1 = '" & Trim(txt_AutoBakClient1.Text) & "',Autobackup_Path_Client2 = '" & Trim(txt_AutoBakClient2.Text) & "'"
            cmd.ExecuteNonQuery()

            trans.Commit()

        Catch ex As Exception
            trans.Rollback()

            MessageBox.Show(ex.Message, "DOES NOT Update", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally
            Me.Close()
            'Common_Procedures.vShowEntrance_Status_FromCCupdate = True
            'MDIParent1.Close()
            'Entrance.Show()
        End Try

    End Sub

    Private Sub btn_UPDATE_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_UPDATE.Click
        save_record()
    End Sub

    Private Sub btn_Close_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Close.Click
        On Error Resume Next
        Me.Close()
        Me.Dispose()
    End Sub

    Private Sub txt_AutoBakServer_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_AutoBakServer.KeyDown
        If e.KeyValue = 38 Then SendKeys.Send("+{TAB}")
        If e.KeyValue = 40 Then
            txt_AutoBakClient1.Focus()
        End If
    End Sub

    Private Sub txt_NewCC_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_AutoBakServer.KeyPress
        If Asc(e.KeyChar) = 13 Then
            txt_AutoBakClient1.Focus()
        End If
    End Sub

    Private Sub txt_AutoBakClient1_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_AutoBakClient1.KeyDown
        If e.KeyValue = 38 Then SendKeys.Send("+{TAB}")
        If e.KeyValue = 40 Then
            txt_AutoBakClient2.Focus()
        End If
    End Sub

    Private Sub txt_AutoBakClient1_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_AutoBakClient1.KeyPress
        If Asc(e.KeyChar) = 13 Then
            txt_AutoBakClient2.Focus()
        End If
    End Sub

    Private Sub txt_AutoBakClient2_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_AutoBakClient2.KeyDown
        If e.KeyValue = 38 Then SendKeys.Send("+{TAB}")
        If e.KeyValue = 40 Then
            btn_UPDATE.Focus()
        End If
    End Sub

    Private Sub txt_AutoBakClient2_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_AutoBakClient2.KeyPress
        If Asc(e.KeyChar) = 13 Then
            btn_UPDATE.Focus()
        End If
    End Sub

End Class