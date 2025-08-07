Imports System.IO
Public Class Tsoft_Register_Encryption_DeCrption_Form

    Private Sub btn_Register_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Register.Click
        '---
    End Sub

    Private Sub btn_Show_LicenseCode_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Show_LicenseCode.Click

        If Trim(lbl_SystemNo.Text) = "" Then
            MessageBox.Show("Invalid System No", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If txt_LicenseCode.Enabled Then txt_LicenseCode.Focus()
            Exit Sub
        End If

        txt_LicenseCode.Text = Common_Procedures.Encrypt(Trim(lbl_SystemNo.Text), Trim(Common_Procedures.SoftWareRegister.passPhrase), Trim(Common_Procedures.SoftWareRegister.passPhrase))

    End Sub

    Private Sub Tsoft_Register_Encryption_DeCrption_Form_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim pathnameRoot As String = ""


        If InStr(1, Trim(LCase(Application.StartupPath)), "\bin\debug") > 0 Then
            Common_Procedures.AppPath = Replace(Trim(LCase(Application.StartupPath)), "\bin\debug", "")
        Else
            Common_Procedures.AppPath = Application.StartupPath
        End If


        pathnameRoot = Path.GetPathRoot(Common_Procedures.AppPath)
        'pth = Trim(Common_Procedures.AppPath) & "\license.txt"

        lbl_SystemNo.Text = Common_Procedures.GetDriveSerialNumber(Microsoft.VisualBasic.Left(pathnameRoot, 2))

    End Sub

    Private Sub btn_Close_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Close.Click
        Me.Close()
        Me.Dispose()
    End Sub

End Class