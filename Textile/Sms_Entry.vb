Imports System.Security.Cryptography.X509Certificates
Imports System.Net
Imports System.Net.Security
Imports System.Net.Mail
Imports System.Collections.Specialized
Imports System.Text
Imports RestSharp

Public Class Sms_Entry

    Public Shared vSmsPhoneNo As String
    Public Shared vSmsMessage As String
    Public Shared SMSProvider_SenderID As String
    Public Shared SMSProvider_Key As String
    Public Shared SMSProvider_RouteID As String
    Public Shared SMSProvider_Type As String
    Public Shared SMS_TempleteID As String
    Public Shared vAttchFilepath As String

    Private Sub Sms_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Me.Left = Screen.PrimaryScreen.WorkingArea.Width - 30 - Me.Width - 15
        Me.Top = Screen.PrimaryScreen.WorkingArea.Height - 132 - Me.Height - 15
        txt_PhnNo.Text = Trim(vSmsPhoneNo)
        txt_Msg.Text = Trim(vSmsMessage)
        txt_Attachment.Text = Trim(vAttchFilepath)
    End Sub

    Private Sub Sms_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If Asc(e.KeyChar) = 27 Then
            Me.Close()
        End If
    End Sub

    Private Sub btnSendSMS_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSendSMS.Click
        Dim request As HttpWebRequest
        Dim response As HttpWebResponse = Nothing
        Dim url As String
        Dim i As Integer = 0
        Dim smstxt As String = ""
        Dim PhNo As String = ""
        Dim timeout As Integer = 50000

        Try

            PhNo = Trim(txt_PhnNo.Text)

            smstxt = Trim(txt_Msg.Text)

            smstxt = Replace(smstxt, " & ", " and ")
            smstxt = Replace(smstxt, "&", " and ")

            SMSProvider_SenderID = Common_Procedures.settings.SMS_Provider_SenderID
            SMSProvider_Key = Common_Procedures.settings.SMS_Provider_Key
            SMSProvider_RouteID = Common_Procedures.settings.SMS_Provider_RouteID
            SMSProvider_Type = Common_Procedures.settings.SMS_Provider_Type


            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1078" Then '---- Kalaimagal Sizing (Palladam)
                url = "http://api.lionsms.com/api/?username=kalaimagalweaving&password=Newlife@5&cmd=sendSMS&to=" & Trim(PhNo) & "&sender=KMWEAV&message=" & Trim(smstxt)
                'url = "http://api.lionsms.com/api/?username=kalaimagalweaving&password=Newlife@5&cmd=sendSMS&to=8508403222&sender=KMWEAV&message=TestmESSAGE BY KALAIMAGAL SIZING"

            Else

                url = "http://198.24.149.4/API/pushsms.aspx?loginID=" & Trim(Common_Procedures.settings.SMS_Provider_LoginID) & "&password=" & Trim(Common_Procedures.settings.SMS_Provider_LoginPassword) & "&mobile=" & Trim(PhNo) & "&text=" & Trim(smstxt) & "&senderid=" & Trim(Common_Procedures.settings.SMS_Provider_SenderID) & "&route_id=2&Unicode=0&Template_id=" & Trim(SMS_TempleteID)
                'url = "http://198.24.149.4/API/pushsms.aspx?loginID=tsoft&password=amutha&mobile=" & Trim(PhNo) & " &text=" & Trim(smstxt) & "&senderid=" & Trim(SMSProvider_SenderID) & "&route_id=2&Unicode=0"
                'url = "http://sms.shamsoft.in/app/smsapi/index.php?key=" & Trim(SMSProvider_Key) & "&routeid=" & Trim(SMSProvider_RouteID) & "&type=" & Trim(SMSProvider_Type) & "&contacts=" & Trim(PhNo) & "&senderid=" & Trim(SMSProvider_SenderID) & "&msg=" & Trim(smstxt)

            End If

            'If Trim(UCase(Common_Procedures.settings.SMS_Provider_LoginID)) <> "" And Trim(UCase(Common_Procedures.settings.SMS_Provider_LoginPassword)) <> "" And Trim(UCase(Common_Procedures.settings.SMS_Provider_SenderID)) <> "" And Trim(UCase(Common_Procedures.settings.SMS_Provider_Key)) = "" Then '---- CheapSMS.com
            '    url = "http://198.24.149.4/API/pushsms.aspx?loginID=" & Trim(Common_Procedures.settings.SMS_Provider_LoginID) & "&password=" & Trim(Common_Procedures.settings.SMS_Provider_LoginPassword) & "&mobile=" & Trim(PhNo) & "&text=" & Trim(smstxt) & "&senderid=" & Trim(Common_Procedures.settings.SMS_Provider_SenderID) & "&route_id=2&Unicode=0&Template_id=" & Trim(Common_Procedures.settings.SMS_TemplateID)
            '    'url = "http://198.24.149.4/API/pushsms.aspx?loginID=" & Trim(Common_Procedures.settings.SMS_Provider_LoginID) & "&password=" & Trim(Common_Procedures.settings.SMS_Provider_LoginPassword) & "&mobile=" & Trim(PhNo) & "&text=" & Trim(smstxt) & "&senderid=" & Trim(Common_Procedures.settings.SMS_Provider_SenderID) & "&route_id=2&Unicode=0"

            '    'ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1102" Then '---- Ganesh karthik Sizing (Somanur)
            '    '    url = "http://198.24.149.4/API/pushsms.aspx?loginID=" & Trim(SMSProvider_SenderID) & "&password=tsoft123&mobile=" & Trim(PhNo) & "&text=" & Trim(smstxt) & "&senderid=" & Trim(SMSProvider_SenderID) & "&route_id=2&Unicode=0"
            '    '    '  url = "http://sms1.shamsoft.in/api/mt/SendSMS?APIKey=" & Trim(SMSProvider_Key) & "&senderid=" & Trim(SMSProvider_SenderID) & "&channel=2&DCS=0&flashsms=0&number=" & Trim(PhNo) & "&text=" & Trim(smstxt) & "&route=" & Trim(SMSProvider_RouteID)

            '    'ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1163" Then
            '    '    url = "http://198.24.149.4/API/pushsms.aspx?loginID=gurusizing&password=tsoft123&mobile=" & Trim(PhNo) & "&text=" & Trim(smstxt) & "&senderid=GURUSZ&route_id=2&Unicode=0"

            '    '    'ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1144" Then
            '    '    'url = "http://198.24.149.4/API/pushsms.aspx?loginID=ADAVAN&password=1234&mobile=" & Trim(PhNo) & "&text=" & Trim(smstxt) & "&senderid=ADAVAN&route_id=2&Unicode=0"

            '    'ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1037" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1038" Then '---- Prakash Sizing (Somanur)
            '    '    url = "http://198.24.149.4/API/pushsms.aspx?loginID=" & Trim(SMSProvider_SenderID) & "&password=tsoft123&mobile=" & Trim(PhNo) & " &text=" & Trim(smstxt) & "&senderid=" & Trim(SMSProvider_SenderID) & "&route_id=2&Unicode=0"

            'ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1078" Then '---- Kalaimagal Sizing (Palladam)
            '    url = "http://api.lionsms.com/api/?username=kalaimagalweaving&password=Newlife@5&cmd=sendSMS&to=" & Trim(PhNo) & "&sender=KMWEAV&message=" & Trim(smstxt)
            '    'url = "http://api.lionsms.com/api/?username=kalaimagalweaving&password=Newlife@5&cmd=sendSMS&to=8508403222&sender=KMWEAV&message=TestmESSAGE BY KALAIMAGAL SIZING"


            'Else
            '    url = "http://198.24.149.4/API/pushsms.aspx?loginID=" & Trim(Common_Procedures.settings.SMS_Provider_LoginID) & "&password=" & Trim(Common_Procedures.settings.SMS_Provider_LoginPassword) & "&mobile=" & Trim(PhNo) & "&text=" & Trim(smstxt) & "&senderid=" & Trim(Common_Procedures.settings.SMS_Provider_SenderID) & "&route_id=2&Unicode=0&Template_id=" & Trim(Common_Procedures.settings.SMS_TemplateID)
            '    'url = "http://198.24.149.4/API/pushsms.aspx?loginID=tsoft&password=amutha&mobile=" & Trim(PhNo) & " &text=" & Trim(smstxt) & "&senderid=" & Trim(SMSProvider_SenderID) & "&route_id=2&Unicode=0"
            '    'url = "http://sms.shamsoft.in/app/smsapi/index.php?key=" & Trim(SMSProvider_Key) & "&routeid=" & Trim(SMSProvider_RouteID) & "&type=" & Trim(SMSProvider_Type) & "&contacts=" & Trim(PhNo) & "&senderid=" & Trim(SMSProvider_SenderID) & "&msg=" & Trim(smstxt)

            'End If

            ''url = "http://sms.shamsoft.in/app/smsapi/index.php?key=" & Trim(Common_Procedures.settings.SMS_Provider_Key) & "&routeid=" & Trim(Common_Procedures.settings.SMS_Provider_RouteID) & "&type=" & Trim(Common_Procedures.settings.SMS_Provider_Type) & "&contacts=" & Trim(PhNo) & "&senderid=" & Trim(Common_Procedures.settings.SMS_Provider_SenderID) & "&msg=" & Trim(smstxt)

            ''url = "http://sms.shamsoft.in/app/smsapi/index.php?key=355C7A0B5595B2&routeid=134&type=text&contacts=" & Trim(PhNo) & "&senderid=WEBSMS&msg=" & Trim(smstxt)

            ''--url = "http://sms.shamsoft.in/app/smsapi/index.php?key=355C7A0B5595B2&routeid=14&type=text&contacts=" & Trim(PhNo) & "&senderid=WEBSMS&msg=" & Trim(smstxt)

            ''--(jenilla)
            ''url = "http://103.16.101.52:8080/bulksms/bulksms?username=nila-jenial&password=nila123&type=0&dlr=1&destination=" & Trim(PhNo) & "&source=JENIAL&message=" & Trim(smstxt)

            ''--THIS IS Working (jenilla)
            ''url = "http://103.16.101.52:8080/bulksms/bulksms?username=nila-jenial&password=nila123&type=0&dlr=1&destination=918508403222&source=JENIAL&message=" & Trim(smstxt)

            ''THIS IS OK
            ''url = "http://sms.shamsoft.in/app/smsapi/index.php?key=355C7A0B5595B2&routeid=73&type=text&contacts=8508403222&senderid=WEBSMS&msg=Hello+People%2C+have+a+great+day"

            ''url = "http://sms.shamsoft.in/app/smsapi/index.php?key=355C7A0B5595B2&routeid=14&type=text&contacts=97656XXXXX,98012XXXXX&senderid=DEMO&msg=Hello+People%2C+have+a+great+day"

            ''url = "http://103.16.101.52:8080/bulksms/bulksms?username=nila-jenial&password=nila123&type=0&dlr=1&destination=918508403222&source=JENIAL&message=testmsg"

            ''url = "http://103.16.101.52:8080/bulksms/bulksms?username=nila-jenial&password=nila123&type=0&dlr=1&destination=918508403222&source=JENIAL&message=testmsg"

            request = DirectCast(WebRequest.Create(url), HttpWebRequest)
            request.KeepAlive = True

            request.Timeout = timeout

            response = DirectCast(request.GetResponse(), HttpWebResponse)

            If Trim(UCase(response.StatusDescription)) = "OK" Then
                MessageBox.Show("Sucessfully Sent...", "FOR SENDING SMS...", MessageBoxButtons.OKCancel, MessageBoxIcon.Information)
                Me.Close()
                Me.Dispose()
            Else
                MessageBox.Show("Failed to sent SMS...", "FOR SENDING SMS...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            End If

        Catch ex As Exception
            If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1078" Then '---- Kalaimagal Sizing (Palladam)
                MessageBox.Show(ex.Message, "DOES NOT SEND SMS...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            End If

        Finally
            Try
                response.Close()

                response = Nothing
                request = Nothing

            Catch ex As Exception
                '---
            End Try


        End Try

    End Sub

    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub

    Private Sub btn_AttachmentSelection_Click(sender As Object, e As EventArgs) Handles btn_AttachmentSelection.Click
        Dim Atch_FlName As String

        OpenFileDialog1.ShowDialog()
        Atch_FlName = OpenFileDialog1.FileName

        If Trim(Atch_FlName) <> "" Then
            txt_Attachment.Text = Trim(Atch_FlName)
            Exit Sub
        End If
    End Sub

    Private Sub btnSend_WpSMS_Click(sender As Object, e As EventArgs) Handles btnSend_WpSMS.Click
        Dim I As Integer = 0
        Dim N As Integer = 0
        Dim WhatsAppkey As String = ""      'WhatsAppPubKey ' your api key
        Dim vSENTSTS As Boolean = False

        Try

            txt_PhnNo.Text = Replace(Trim(txt_PhnNo.Text), "-", "")
            txt_PhnNo.Text = Replace(Trim(txt_PhnNo.Text), "  ", "")
            txt_PhnNo.Text = Replace(Trim(txt_PhnNo.Text), " ", "")

            If Trim(txt_PhnNo.Text) = "" Or Microsoft.VisualBasic.Len(Trim(txt_PhnNo.Text)) < 10 Then
                MessageBox.Show("Invalid Phone No.", "FOR SENDING WHATSAPP MESSAGE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                Exit Sub
            End If

            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1123" Then  '---- Shanthi Sizing(Somanur)  or  SRI NIKITHA SIZING MILLS (SOMANUR)
                WhatsAppkey = "cbLUyDJQyNYUlWXEdC"  ' 9942912399
            ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1027" Then
                WhatsAppkey = "cbEJHKEuGeoITrfkjD"  ' 919489688111   pwd - 323075
            ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1155" Then
                WhatsAppkey = "cbmCTteFFoYFlNitcJ"  ' 917418918377   pwd - 807930
            ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1031" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1172" Then '---- SRI RAM WEAVING MILL (PALLADAM)   -- OR --  SRI RAM SIZING (PALLADAAM)
                WhatsAppkey = "cbCFZYANuBbVgCwzYB"  ' 919367334455   pwd - 979849
            Else
                WhatsAppkey = "cbxSAlkkySfKWquZSM" '  8508403221-OLD
                'WhatsAppkey = "cbMIIPDyfehzxdqCjl" ' 8508403222
            End If

            System.Net.ServicePointManager.Expect100Continue = False
            ServicePointManager.SecurityProtocol = CType(768, SecurityProtocolType) Or CType(3072, SecurityProtocolType)
            ServicePointManager.SecurityProtocol = DirectCast(3072, SecurityProtocolType)

            N = 1
            If Trim(txt_Attachment.Text) <> "" And Microsoft.VisualBasic.Len(Trim(txt_Attachment.Text)) > 5 And Trim(UCase(Microsoft.VisualBasic.Right(Trim(txt_Attachment.Text), 4))) = Trim(UCase(".PDF")) Then
                N = 2
            End If

            vSENTSTS = False
            For I = 1 To N

                If I = 1 And Trim(txt_Attachment.Text) <> "" And Microsoft.VisualBasic.Len(Trim(txt_Attachment.Text)) > 5 Then

                    Dim client As New RestClient("https://api.whatsdesk.in/v4/filefromdisk.php")
                    client.Timeout = -1
                    Dim request = New RestRequest(Method.POST)
                    request.AddFile("data", Trim(txt_Attachment.Text))
                    request.AddParameter("key", Trim(WhatsAppkey))
                    request.AddParameter("number", "91" & Trim(txt_PhnNo.Text))
                    request.AddParameter("caption", Trim(txt_Msg.Text))
                    request.AddParameter("message", Trim(txt_Msg.Text))
                    Dim response As IRestResponse = client.Execute(request)
                    txt_response.Text = response.Content

                    vSENTSTS = True

                ElseIf Trim(txt_Msg.Text) <> "" Then

                    Dim wb = New WebClient()
                    Dim data = New NameValueCollection()
                    data("message") = Trim(txt_Msg.Text)
                    data("key") = Trim(WhatsAppkey)
                    data("number") = "91" & Trim(txt_PhnNo.Text)

                    Dim response = wb.UploadValues("https://api.whatsdesk.in/v4/text.php", "POST", data)

                    Dim responseInString As String = Encoding.UTF8.GetString(response)

                    txt_response.Text = responseInString


                    vSENTSTS = True

                End If

            Next I


            If vSENTSTS = True Then
                MessageBox.Show("Sent Sucessfully...", "WHATSAPP MESSAGE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Information)
            End If


        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error Occured", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

End Class