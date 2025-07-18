Public Class Software_Module_Selection

    Private con As New SqlClient.SqlConnection(Common_Procedures.Connection_String)

    Private Sub Company_Selection_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Me.BackColor = Color.FromArgb(41, 57, 85) ' Color.White  ' Color.FromArgb(41, 57, 85)
        pnl_Back.BackColor = Color.FromArgb(41, 57, 85)  'Me.BackColor ' Color.FromArgb(41, 57, 85)

        con.Open()

        Common_Procedures.SoftwareModuleType_SelectedIdNo = 0

    End Sub

    Private Sub Company_Selection_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If Asc(e.KeyChar) = 27 Then
            Common_Procedures.SoftwareModuleType_SelectedIdNo = 0
            Me.Close()
        End If
    End Sub

    Private Sub Company_Selection_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        con.Close()
    End Sub

    Private Sub btn_Cancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Cancel.Click
        Common_Procedures.CompIdNo = 0
        Me.Close()
    End Sub

    Private Sub btn_OK_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_OK.Click
        Dim da As SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim CompID As Integer

        da = New SqlClient.SqlDataAdapter("select Software_Modules_IdNo from Software_Modules_Head where Software_Modules_Name = '" & Trim(cbo_Company.Text) & "'", con)
        dt1 = New DataTable
        da.Fill(dt1)

        CompID = 0
        If dt1.Rows.Count > 0 Then
            If IsDBNull(dt1.Rows(0)(0).ToString) = False Then
                CompID = Val(dt1.Rows(0)(0).ToString)
            End If
        End If

        If CompID = 0 Then
            MessageBox.Show("Invalid Software Module Selection", "DOES NOT SELECT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If cbo_Company.Enabled Then cbo_Company.Focus()
            Exit Sub
        End If

        Common_Procedures.SoftwareModuleType_SelectedIdNo = Val(CompID)

        Me.Close()

    End Sub


    Private Sub cbo_Company_GotFocus(sender As Object, e As EventArgs) Handles cbo_Company.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Software_Modules_Head", "Software_Modules_Name", "", "(Software_Modules_IdNo = 0)")
    End Sub

    Private Sub cbo_Company_KeyDown(sender As Object, e As KeyEventArgs) Handles cbo_Company.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, sender, Nothing, Nothing, "Software_Modules_Head", "Software_Modules_Name", "", "(Software_Modules_IdNo = 0)")
    End Sub

    Private Sub cbo_Company_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Company.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, sender, Nothing, "Software_Modules_Head", "Software_Modules_Name", "", "(Software_Modules_IdNo = 0)")

        If Asc(e.KeyChar) = 13 Then
            e.Handled = True
            btn_OK_Click(sender, e)
        End If

    End Sub

End Class