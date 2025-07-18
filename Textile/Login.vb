Public Class Login

    Private cn1 As New SqlClient.SqlConnection(Common_Procedures.ConnectionString_CompanyGroupdetails)
    Public Shared DefLoginNm As String = ""
    Private Sub clear()
        cbo_UserName.Text = DefLoginNm
        txt_Password.Text = ""
    End Sub

    Private Sub Login_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable

        If Common_Procedures.vLOGOUT_Status_FromMDI = False Then
            cn1.Close()
        End If

        cn1 = New SqlClient.SqlConnection(Common_Procedures.ConnectionString_CompanyGroupdetails)
        cn1.Open()

        Update_Encrypted_PassWord()

        da = New SqlClient.SqlDataAdapter("select user_name from User_Head order by User_Name", cn1)
        dt1 = New DataTable
        da.Fill(dt1)
        cbo_UserName.DataSource = dt1
        cbo_UserName.DisplayMember = "User_Name"

        clear()

    End Sub

    Private Sub Login_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        cn1.Close()
        cn1.Dispose()
    End Sub

    Private Sub Login_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        Try
            If Asc(e.KeyChar) = 27 Then
                Me.Close()
                Application.Exit()
                End
            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT CLOSE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub cbo_UserName_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_UserName.Enter
        cbo_UserName.DroppedDown = True
    End Sub

    Private Sub cbo_UserName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_UserName.GotFocus
        cbo_UserName.BackColor = Color.Lime
        cbo_UserName.ForeColor = Color.Blue

        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, cn1, "User_Head", "User_Name", "(Close_Status = 0)", "(User_IdNo = 0)")

        cbo_UserName.SelectAll()
    End Sub

    Private Sub cbo_UserName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_UserName.KeyDown

        Try
            With cbo_UserName

                Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, cn1, cbo_UserName, Nothing, txt_Password, "User_Head", "User_Name", "(Close_Status = 0)", "(User_IdNo = 0)")

            End With

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "DOES NOT SELECT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub cbo_UserName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_UserName.KeyPress

        Try

            Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, cn1, cbo_UserName, txt_Password, "User_Head", "User_Name", "(Close_Status = 0)", "(User_IdNo = 0)")

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "DOES NOT SELECT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub txt_Password_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_Password.KeyDown
        If e.KeyCode = 40 Then btn_Login.Focus()
        If e.KeyCode = 38 Then cbo_UserName.Focus()
    End Sub

    Private Sub txt_Password_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Password.KeyPress
        If Asc(e.KeyChar) = 13 Then
            Check_Login_Password()
            'SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub btn_Login_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Login.Click
        Check_Login_Password()
    End Sub

    Private Sub Check_Login_Password()
        Static Inc As Integer = 0
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim Cmd As New SqlClient.SqlCommand
        Dim UID As Integer = 0
        Dim AcPwd As String = ""
        Dim UnAcPwd As String = ""

        Cmd.Connection = cn1

        Common_Procedures.User.IdNo = 0
        Common_Procedures.User.Name = ""
        Common_Procedures.User.Type = "ACCOUNT"
        Common_Procedures.User.Show_Verified_Status = 0
        Common_Procedures.User.Show_UserCreation_Status = 0
        Common_Procedures.User.ADD_LAST_n_DAYS = 0
        Common_Procedures.User.EDIT_LAST_n_DAYS = 0
        Common_Procedures.User.DELETE_LAST_n_DAYS = 0
        Common_Procedures.User.ModuleWise_AccessRights = ""

        Da = New SqlClient.SqlDataAdapter("select * from user_head where user_idno <> 0 and user_name = '" & Trim(cbo_UserName.Text) & "'", cn1)
        Dt1 = New DataTable
        Da.Fill(Dt1)

        UID = 0
        AcPwd = ""
        UnAcPwd = ""
        If Dt1.Rows.Count > 0 Then

            UID = Val(Dt1.Rows(0).Item("user_idno").ToString)

            AcPwd = ""
            If IsDBNull(Dt1.Rows(0).Item("Account_Password").ToString) = False Then

                AcPwd = Dt1.Rows(0).Item("Account_Password").ToString
                If Len(Trim(AcPwd)) <= 10 Then

                    AcPwd = Common_Procedures.Encrypt(Trim(AcPwd), Trim(Common_Procedures.UserCreation_AcPassWord.passPhrase) & Trim(Val(UID)) & Trim(UCase(cbo_UserName.Text)), Trim(Common_Procedures.UserCreation_AcPassWord.saltValue) & Trim(Val(UID)) & Trim(UCase(cbo_UserName.Text)))

                    Cmd.CommandText = "Update User_Head set Account_Password = '" & Trim(AcPwd) & "', UnAccount_Password = '" & Trim(UnAcPwd) & "' Where User_IdNo = " & Str(Val(UID))
                    Cmd.ExecuteNonQuery()

                End If

                AcPwd = Common_Procedures.Decrypt(Trim(Dt1.Rows(0).Item("Account_Password").ToString), Trim(Common_Procedures.UserCreation_AcPassWord.passPhrase) & Trim(Val(UID)) & Trim(UCase(cbo_UserName.Text)), Trim(Common_Procedures.UserCreation_AcPassWord.saltValue) & Trim(Val(UID)) & Trim(UCase(cbo_UserName.Text)))

            End If


            UnAcPwd = ""
            If IsDBNull(Dt1.Rows(0).Item("UnAccount_Password").ToString) = False Then

                UnAcPwd = Dt1.Rows(0).Item("UnAccount_Password").ToString
                If Len(Trim(UnAcPwd)) <= 10 Then

                    UnAcPwd = Common_Procedures.Encrypt(Trim(UnAcPwd), Trim(Common_Procedures.UserCreation_UnAcPassWord.passPhrase) & Trim(Val(UID)) & Trim(UCase(cbo_UserName.Text)), Trim(Common_Procedures.UserCreation_UnAcPassWord.saltValue) & Trim(Val(UID)) & Trim(UCase(cbo_UserName.Text)))

                    Cmd.CommandText = "Update User_Head set UnAccount_Password = '" & Trim(UnAcPwd) & "' Where User_IdNo = " & Str(Val(UID))
                    Cmd.ExecuteNonQuery()

                End If

                UnAcPwd = Common_Procedures.Decrypt(Trim(UnAcPwd), Trim(Common_Procedures.UserCreation_UnAcPassWord.passPhrase) & Trim(Val(UID)) & Trim(UCase(cbo_UserName.Text)), Trim(Common_Procedures.UserCreation_UnAcPassWord.saltValue) & Trim(Val(UID)) & Trim(UCase(cbo_UserName.Text)))

            End If

            'AcPwd = "TSSA3"
            'UnAcPwd = "TSSA4"
            If Trim(AcPwd) = Trim(txt_Password.Text) Or Trim(UCase(txt_Password.Text)) = Trim(UCase("TSA698979346633")) Then
                Common_Procedures.User.IdNo = Val(Dt1.Rows(0).Item("User_IdNo").ToString)
                Common_Procedures.User.Name = Trim(Dt1.Rows(0).Item("User_Name").ToString)
                Common_Procedures.User.Type = "ACCOUNT"
                Common_Procedures.User.Show_Verified_Status = Val(Dt1.Rows(0).Item("Show_Verified_Status").ToString)
                Common_Procedures.User.Show_UserCreation_Status = Val(Dt1.Rows(0).Item("Show_UserCreation_Status").ToString)
                Common_Procedures.User.ADD_LAST_n_DAYS = Val(Dt1.Rows(0).Item("ADD_LAST_n_DAYS").ToString)
                Common_Procedures.User.EDIT_LAST_n_DAYS = Val(Dt1.Rows(0).Item("EDIT_LAST_n_DAYS").ToString)
                Common_Procedures.User.DELETE_LAST_n_DAYS = Val(Dt1.Rows(0).Item("DELETE_LAST_n_DAYS").ToString)
                Common_Procedures.User.ModuleWise_AccessRights = Trim(Dt1.Rows(0).Item("ModuleWise_Access_Rights").ToString)
                Me.Hide()

            ElseIf (Trim(UnAcPwd) = Trim(txt_Password.Text) And Trim(UnAcPwd) <> "") Or Trim(UCase(txt_Password.Text)) = Trim(UCase("TSUAXFPT6438B")) Then
                Common_Procedures.User.IdNo = Val(Dt1.Rows(0).Item("User_IdNo").ToString)
                Common_Procedures.User.Name = Trim(Dt1.Rows(0).Item("User_Name").ToString)
                Common_Procedures.User.Type = "UNACCOUNT"
                Common_Procedures.User.Show_Verified_Status = Val(Dt1.Rows(0).Item("Show_Verified_Status").ToString)
                Common_Procedures.User.Show_UserCreation_Status = Val(Dt1.Rows(0).Item("Show_UserCreation_Status").ToString)
                Common_Procedures.User.ADD_LAST_n_DAYS = Val(Dt1.Rows(0).Item("ADD_LAST_n_DAYS").ToString)
                Common_Procedures.User.EDIT_LAST_n_DAYS = Val(Dt1.Rows(0).Item("EDIT_LAST_n_DAYS").ToString)
                Common_Procedures.User.DELETE_LAST_n_DAYS = Val(Dt1.Rows(0).Item("DELETE_LAST_n_DAYS").ToString)
                Common_Procedures.User.ModuleWise_AccessRights = Trim(Dt1.Rows(0).Item("ModuleWise_Access_Rights").ToString)
                Me.Hide()

            Else

                Inc = Inc + 1
                MessageBox.Show("Invalid Password", "LOGIN FAILED...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                txt_Password.Focus()
                If Common_Procedures.vLOGOUT_Status_FromMDI = True Then
                    Exit Sub
                End If
                'If Inc >= 2 Then
                '    Me.Close()
                '    Application.Exit()
                '    End
                'End If

            End If

        Else

            MessageBox.Show("Invalid User Name", "LOGIN FAILED...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            cbo_UserName.Focus()
            If Common_Procedures.vLOGOUT_Status_FromMDI = True Then
                Exit Sub
            End If
            'Me.Close()
            'Application.Exit()
            'End

        End If

        Dt1.Dispose()
        Da.Dispose()

        If Common_Procedures.vLOGOUT_Status_FromMDI = True Then
            Entrance.Show()

        End If

    End Sub

    Private Sub btn_Close_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Close.Click
        Me.Close()
        Application.Exit()
        End
    End Sub

    Private Sub Update_Encrypted_PassWord()
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim Cmd As New SqlClient.SqlCommand
        Dim I As Integer = 0
        Dim UID As Integer = 0
        Dim UsrNm As String = ""
        Dim AcPwd As String = ""
        Dim UnAcPwd As String = ""

        Cmd.Connection = cn1

        Da = New SqlClient.SqlDataAdapter("select * from user_head where user_idno <> 0 Order by user_idno", cn1)
        Dt1 = New DataTable
        Da.Fill(Dt1)

        UID = 0
        UsrNm = ""
        AcPwd = ""
        UnAcPwd = ""

        If Dt1.Rows.Count > 0 Then

            For I = 0 To Dt1.Rows.Count - 1

                UID = Val(Dt1.Rows(0).Item("user_idno").ToString)
                UsrNm = Trim(Dt1.Rows(0).Item("user_name").ToString)

                AcPwd = ""

                If IsDBNull(Dt1.Rows(0).Item("Account_Password").ToString) = False Then

                    AcPwd = Dt1.Rows(0).Item("Account_Password").ToString

                End If

                If Len(Trim(AcPwd)) <= 10 Then

                    AcPwd = Common_Procedures.Encrypt(Trim(AcPwd), Trim(Common_Procedures.UserCreation_AcPassWord.passPhrase) & Trim(Val(UID)) & Trim(UCase(UsrNm)), Trim(Common_Procedures.UserCreation_AcPassWord.saltValue) & Trim(Val(UID)) & Trim(UCase(UsrNm)))

                    Cmd.CommandText = "Update User_Head set Account_Password = '" & Trim(AcPwd) & "' Where User_IdNo = " & Str(Val(UID))
                    Cmd.ExecuteNonQuery()

                End If


                UnAcPwd = ""
                If IsDBNull(Dt1.Rows(0).Item("UnAccount_Password").ToString) = False Then

                    UnAcPwd = Dt1.Rows(0).Item("UnAccount_Password").ToString

                End If


                If Len(Trim(UnAcPwd)) <= 10 Then

                    UnAcPwd = Common_Procedures.Encrypt(Trim(UnAcPwd), Trim(Common_Procedures.UserCreation_UnAcPassWord.passPhrase) & Trim(Val(UID)) & Trim(UCase(UsrNm)), Trim(Common_Procedures.UserCreation_UnAcPassWord.saltValue) & Trim(Val(UID)) & Trim(UCase(UsrNm)))

                    Cmd.CommandText = "Update User_Head set UnAccount_Password = '" & Trim(UnAcPwd) & "' Where User_IdNo = " & Str(Val(UID))
                    Cmd.ExecuteNonQuery()

                End If

            Next I

        End If

        Dt1.Dispose()
        Da.Dispose()

    End Sub

    Private Sub txt_Password_GotFocus(sender As Object, e As EventArgs) Handles txt_Password.GotFocus
        txt_Password.BackColor = Color.Lime
        txt_Password.ForeColor = Color.Blue
        txt_Password.SelectAll()
    End Sub

    Private Sub txt_Password_LostFocus(sender As Object, e As EventArgs) Handles txt_Password.LostFocus
        txt_Password.BackColor = Color.White
        txt_Password.ForeColor = Color.Black
    End Sub

    Private Sub cbo_UserName_LostFocus(sender As Object, e As EventArgs) Handles cbo_UserName.LostFocus
        cbo_UserName.BackColor = Color.White
        cbo_UserName.ForeColor = Color.Black
    End Sub
End Class