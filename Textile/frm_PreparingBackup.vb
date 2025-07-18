Public Class frm_PreparingBackup
    Private Sub Preparing_Backup_Load(sender As Object, e As EventArgs) Handles Me.Load
        lbl_Wait.Visible = True
        bgw_Process.RunWorkerAsync()
        ProgressBar1.Visible = True
    End Sub

    Private Sub bgw_Process_DoWork(ByVal sender As System.Object, ByVal e As System.ComponentModel.DoWorkEventArgs) Handles bgw_Process.DoWork
        Dim Nr As Long

        Try

            Common_Procedures.Sql_AutoBackUP(Common_Procedures.DataBaseName, False)

            System.Threading.Thread.Sleep(100)

            Dim cn1 As New SqlClient.SqlConnection(Common_Procedures.Connection_String)
            Dim cmd As New SqlClient.SqlCommand

            cn1.Open()

            cmd.Connection = cn1

            cmd.Parameters.Clear()
            cmd.Parameters.AddWithValue("@BackupDate", Now)

            Nr = 0
            cmd.CommandText = "update settings_head set AutoBackUp_Date = @BackupDate"
            Nr = cmd.ExecuteNonQuery()
            If Nr = 0 Then
                cmd.CommandText = "insert into settings_head (AutoBackUp_Date) values (@BackupDate)"
                cmd.ExecuteNonQuery()
            End If

            cmd.Dispose()

            cn1.Close()
            cn1.Dispose()

            Common_Procedures.Sql_AutoBackUP(Common_Procedures.CompanyDetailsDataBaseName, True)

        Catch ex As Exception

            MessageBox.Show(ex.Message, "SOFTWARE AUTOBACKUP FAILED...", MessageBoxButtons.OKCancel, MessageBoxIcon.Information)

            If Common_Procedures.vShowEntrance_Status_FromMDI = False And Common_Procedures.vShowEntrance_Status_FromCCupdate = False Then
                Application.Exit()
                End
            End If

        End Try

    End Sub
    Private Sub bgw_Process_RunWorkerCompleted(ByVal sender As Object, ByVal e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles bgw_Process.RunWorkerCompleted

        Try

            Me.Refresh()

            lbl_Wait.Visible = False
            ProgressBar1.Visible = False

            Me.Close()

            If Common_Procedures.vShowEntrance_Status_FromMDI = False And Common_Procedures.vShowEntrance_Status_FromCCupdate = False Then
                Application.Exit()
                End
            End If

        Catch ex As Exception
            '------

        End Try

    End Sub

End Class