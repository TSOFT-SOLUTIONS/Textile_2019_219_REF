Public Class CC_Update
    Private con As New SqlClient.SqlConnection(Common_Procedures.ConnectionString_CompanyGroupdetails)

    Private Sub CC_Update_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        On Error Resume Next
        con.Close()
        con.Dispose()

    End Sub

    Private Sub CC_Update_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        con.Open()

        lbl_CurrentCC.Text = ""
        txt_NewCC.Text = ""

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

                If IsDBNull(dt1.Rows(0).Item("Cc_No").ToString) = False Then
                    lbl_CurrentCC.Text = dt1.Rows(0).Item("Cc_No").ToString()
                End If

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
        Dim Nr As Long = 0

        If Trim(txt_NewCC.Text) = "" Then
            MessageBox.Show("Invalid CC_No", "DOES NOT SAVE", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        CC_Leng = Len(Trim(txt_NewCC.Text))

        If Val(CC_Leng) < 4 Then
            MessageBox.Show("Invalid CC_No", "DOES NOT SAVE", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        trans = con.BeginTransaction

        Try
            cmd.Connection = con
            cmd.Transaction = trans

            cmd.CommandText = "update Settings_Head set Cc_No = '" & Trim(txt_NewCC.Text) & "'"
            Nr = cmd.ExecuteNonQuery()
            If Nr = 0 Then
                cmd.CommandText = "Insert into Settings_Head ( Cc_No ) values ('" & Trim(txt_NewCC.Text) & "')"
                Nr = cmd.ExecuteNonQuery()
            End If


            If Common_Procedures.is_OfficeSystem = True Then

                If MessageBox.Show("Do you want to update CC No. into this CompanyGroup ?", "FOR COMPANY GROUP CC UPDATION...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = Windows.Forms.DialogResult.Yes Then

                    cmd.CommandText = "Update CompanyGroup_Head set Cc_No = '" & Trim(txt_NewCC.Text) & "' Where CompanyGroup_IdNo = " & Str(Val(Common_Procedures.CompGroupIdNo))
                    cmd.ExecuteNonQuery()

                End If

            End If


            trans.Commit()

        Catch ex As Exception
            trans.Rollback()

            MessageBox.Show(ex.Message, "DOES NOT Update", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally
            Me.Close()
            Common_Procedures.vShowEntrance_Status_FromCCupdate = True
            MDIParent1.Close()
            Entrance.Show()
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

    Private Sub txt_NewCC_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_NewCC.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
            e.Handled = True
        End If
        If Asc(e.KeyChar) = 13 Then
            btn_UPDATE.Focus()
        End If
    End Sub
End Class