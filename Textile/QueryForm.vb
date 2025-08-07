Public Class QueryForm

    Private Sub btn_Close_Click(sender As Object, e As EventArgs) Handles btn_Close.Click
        Me.Close()
    End Sub

    Private Sub btn_NewQuery_Click(sender As Object, e As EventArgs) Handles btn_NewQuery.Click

        dgv_Result.DataSource = Nothing
        txt_Query.Text = ""
        txt_Result.Text = ""
        txt_Query.Focus()

    End Sub

    Private Sub btn_ExecuteQuery_Click(sender As Object, e As EventArgs) Handles btn_ExecuteQuery.Click

        dgv_Result.DataSource = Nothing
        txt_Result.Text = ""

        Dim con As New SqlClient.SqlConnection(txtConnectionString.Text)
        con.Open()

        Dim cmd As New SqlClient.SqlCommand

        cmd.Connection = con

        Dim tr As SqlClient.SqlTransaction

        tr = con.BeginTransaction

        Try

            cmd.Transaction = tr
            cmd.CommandText = txt_Query.Text

            If LCase(Microsoft.VisualBasic.Left(Trim(txt_Query.Text), 6)) = "select" Then
                Dim da As New SqlClient.SqlDataAdapter
                da.SelectCommand = cmd
                Dim dt As New DataTable
                da.Fill(dt)

                tr.Commit()
                    dgv_Result.DataSource = dt
                    dgv_Result.Refresh()
                If dt.Rows.Count = 0 Then
                    txt_Result.Text = "No rows retrieved."
                End If

            Else
                Dim n As Integer = cmd.ExecuteNonQuery()
                tr.Commit()
                txt_Result.Text = n.ToString & " Rows effeted. Query executed succesfully . "
            End If

        Catch ex As Exception

            tr.Rollback()
            txt_Result.Text = ex.Message & ". Query Failed to Execute ! "

        End Try

    End Sub

    Private Sub btn_Clear_Click(sender As Object, e As EventArgs) Handles btn_Clear.Click

        dgv_Result.DataSource = Nothing
        txt_Result.Text = ""
        dgv_Result.Refresh()

    End Sub

    Private Sub QueryForm_Load(sender As Object, e As EventArgs) Handles Me.Load
        txtConnectionString.Text = Common_Procedures.Connection_String
    End Sub

    Private Sub btn_DefaultConnection_Click(sender As Object, e As EventArgs) Handles btn_DefaultConnection.Click

        dgv_Result.DataSource = Nothing
        txt_Result.Text = ""
        txtConnectionString.Text = Common_Procedures.Connection_String

    End Sub

End Class