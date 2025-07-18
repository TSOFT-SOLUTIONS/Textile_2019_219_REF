Imports System.Security.Cryptography.X509Certificates
Imports System.Net
Imports System.Net.Security
Imports System.Net.Mail
Imports System.IO

Public Class EMAIL_Entry

    Public Shared vMailID As String
    Public Shared vSubJect As String
    Public Shared vAttchFilepath As String
    Public Shared vMessage As String

    Private Sub EMAIL_Entry_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Me.Left = Screen.PrimaryScreen.WorkingArea.Width - 30 - Me.Width
        Me.Top = Screen.PrimaryScreen.WorkingArea.Height - 140 - Me.Height

        txt_PhnNo.Text = Trim(vMailID)
        txt_SubJect.Text = Trim(vSubJect)
        txt_Msg.Text = Trim(vMessage)
        txt_Attachment.Text = Trim(vAttchFilepath)

    End Sub

    Private Sub EMAIL_Entry_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If Asc(e.KeyChar) = 27 Then
            Me.Close()
        End If
    End Sub

    Private Sub btn_AttachmentSelection_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_AttachmentSelection.Click
        Dim Atch_FlName As String

        OpenFileDialog1.ShowDialog()
        Atch_FlName = OpenFileDialog1.FileName

        If Trim(Atch_FlName) <> "" Then
            txt_Attachment.Text = Trim(Atch_FlName)
            Exit Sub
        End If

    End Sub

    Private Sub btnSendMail_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSendMail.Click
        Dim SmtpServer As New SmtpClient()
        Dim mail As New MailMessage()
        Dim vFROM_MailID As String
        Dim vFROM_MailPWD As String
        Dim vTO_MailID As String
        Dim MsgTxt As String
        Dim SubTxt As String
        Dim Atch_FlName As String

        Try

            Atch_FlName = Trim(txt_Attachment.Text)

            If Trim(Atch_FlName) <> "" Then
                If File.Exists(Atch_FlName) = False Then
                    MessageBox.Show("Invalid Attachment File, File does not exists", "DOES NOT SEND MAIL...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Exit Sub
                End If
            End If

            vTO_MailID = Trim(txt_PhnNo.Text)
            SubTxt = Trim(txt_SubJect.Text)
            MsgTxt = Trim(txt_Msg.Text)

            If Val(Common_Procedures.settings.Email_Port) <> 0 Then
                SmtpServer.Port = Val(Common_Procedures.settings.Email_Port)  ' 587
            Else
                SmtpServer.Port = 587
            End If

            If Trim(Common_Procedures.settings.Email_Host) <> "" Then
                SmtpServer.Host = Trim(Common_Procedures.settings.Email_Host)  ' "smtp.gmail.com"
            Else
                SmtpServer.Host = "smtp.gmail.com"
            End If

            SmtpServer.UseDefaultCredentials = False
            SmtpServer.EnableSsl = True

            vFROM_MailID = Trim(Common_Procedures.settings.Email_Address)
            vFROM_MailPWD = Trim(Common_Procedures.settings.Email_Password)
            If Trim(vFROM_MailID) = "" Then

                vFROM_MailID = "tsoftsolutions.info@gmail.com"  '"tsoftsolutions.mail@gmail.com"
                vFROM_MailPWD = "vshc vavn bfys sifo"     '"paktvggtqgdtrtxh"     '--App Password

                'vFROM_MailPWD = "pdgomvvgcqoumauj"       '--App Password
                'Common_Procedures.settings.Email_Password = "8508403222"  '---Login Password
                SmtpServer.Host = "smtp.gmail.com"
                SmtpServer.Port = 587
            End If

            SmtpServer.Credentials = New Net.NetworkCredential(Trim(vFROM_MailID), Trim(vFROM_MailPWD))
            'SmtpServer.Credentials = New Net.NetworkCredential(Trim(Common_Procedures.settings.Email_Address), Trim(Common_Procedures.settings.Email_Password))
            ''''SmtpServer.Credentials = New Net.NetworkCredential("tsoft.tirupur@gmail.com", "GOLD@tn39av7417")
            ''''SmtpServer.Credentials = New Net.NetworkCredential("varalakshmithanges@gmail.com", "GOLD@tn39av7417")
            ''''SmtpServer.Credentials = New Net.NetworkCredential("varalakshmithanges@gmail.com", "gold&VL@19=rj")
            ''''SmtpServer.Credentials = New Net.NetworkCredential("t.thanges@gmail.com", "gold@tn39av7417")
            ''''SmtpServer.Credentials = New Net.NetworkCredential("t.thanges@gmail.com", "cikysrpmkzbwliuc")
            ''''SmtpServer.Credentials = New Net.NetworkCredential("varalakshmithanges@gmail.com", "thanges19")
            ''''SmtpServer.Credentials = New Net.NetworkCredential("tsoft.tirupur@gmail.com", "8508403221")
            ''''SmtpServer.Credentials = New Net.NetworkCredential("t.thanges@gmail.com", "rj17052012")

            mail = New MailMessage()
            mail.From = New MailAddress(Trim(vFROM_MailID))
            'mail.From = New MailAddress("varalakshmithanges@gmail.com")
            'mail.From = New MailAddress("tsoft.tirupur@gmail.com")
            'mail.From = New MailAddress("t.thanges@gmail.com")
            'srirajatex@gmail.com
            mail.To.Add(Trim(vTO_MailID))
            mail.Subject = Trim(SubTxt)
            Mail.Body = Trim(MsgTxt)

            If Trim(Atch_FlName) <> "" Then
                Dim attachment As System.Net.Mail.Attachment
                attachment = New System.Net.Mail.Attachment(Trim(Atch_FlName))
                Mail.Attachments.Add(attachment)
            End If

            SmtpServer.Send(Mail)

            MessageBox.Show("Mail send Sucessfully", "FOR MAILING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Information)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT SEND MAIL...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Finally
            mail.Dispose()
            SmtpServer.Dispose()

        End Try

    End Sub

    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub

    Private Sub pnl_Back_Paint(sender As Object, e As PaintEventArgs) Handles pnl_Back.Paint

    End Sub
End Class