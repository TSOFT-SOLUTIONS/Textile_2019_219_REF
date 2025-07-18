'Imports RestSharp
Imports System.Net
Imports System.IO
Imports System.Text
Imports System.Timers
Imports System.Net.Http
Imports System.Net.HttpRequestHeader

Imports System.Data.SqlClient
Imports System.Net.Security
Imports System.Net.Mail
Imports System.IO.Ports

Imports System.Xml
Imports System.Net.Mail.SmtpClient
Imports System.Data
Imports System.Web
Imports System.Collections.Specialized
Imports Newtonsoft.Json
Imports System.String
Imports System
Imports System.Collections.Generic
Imports System.ComponentModel

Imports System.Drawing
Imports System.Drawing.Design
Imports System.Linq

Imports System.Threading.Tasks
Imports System.Diagnostics
Imports System.Web.Script.Serialization

Imports MessagingToolkit.QRCode

Public Class GSTIN_Search

    Public Shared Sub SEARCHGSTIN(GSTIN As String, ByRef v_Name As String, ByRef v_LegalName_Business As String, ByRef v_Address1 As String, ByRef v_Address2 As String, ByRef v_Address3 As String, ByRef v_Address4 As String, ByRef v_city As String, ByRef v_StateName As String, ByRef v_pincode As String, ByRef v_ERRMSG_SHOWN_STS As Boolean, Optional ByVal vSAVING_STS As Boolean = False)

        v_ERRMSG_SHOWN_STS = False

        If My.Computer.Network.IsAvailable = True Then
            If My.Computer.Network.Ping("www.Google.com") = False Then
                If vSAVING_STS = False Then
                    MessageBox.Show("Invalid Internet Connection", "GSTIN VERIFICATION FAILED...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                    v_ERRMSG_SHOWN_STS = True
                End If
                Exit Sub
            End If

        Else
            If vSAVING_STS = False Then
                MessageBox.Show("Invalid Internet Connection", "GSTIN VERIFICATION FAILED...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                v_ERRMSG_SHOWN_STS = True
            End If
            Exit Sub

        End If

        ServicePointManager.Expect100Continue = True
        ServicePointManager.SecurityProtocol = CType(3072, SecurityProtocolType)

        GSTIN = Microsoft.VisualBasic.Trim(GSTIN)
        GSTIN = Microsoft.VisualBasic.Replace(GSTIN, "  ", "")
        GSTIN = Microsoft.VisualBasic.Replace(GSTIN, " ", "")

        v_ERRMSG_SHOWN_STS = False

        If Len(GSTIN) <> 15 Then
            MessageBox.Show("Invalid GSTIN-Invalid Length", "GSTIN VERIFICATION FAILED...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            v_ERRMSG_SHOWN_STS = True
            Exit Sub
        End If

        If Not IsNumeric(Microsoft.VisualBasic.Left(GSTIN, 2)) Then
            MessageBox.Show("Invalid GSTIN-Invalid State Code", "GSTIN VERIFICATION FAILED...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            v_ERRMSG_SHOWN_STS = True
            Exit Sub
        End If

        For i = 3 To 7
            If IsNumeric(Microsoft.VisualBasic.Mid(GSTIN, i, 1)) Then
                MessageBox.Show("Invalid GSTIN-Invalid PAN Format(1)", "GSTIN VERIFICATION FAILED...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                v_ERRMSG_SHOWN_STS = True
                Exit Sub
            End If
        Next

        For i = 8 To 11
            If Not IsNumeric(Microsoft.VisualBasic.Mid(GSTIN, i, 1)) Then
                MessageBox.Show("Invalid GSTIN-Invalid PAN Format(2)", "GSTIN VERIFICATION FAILED...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                v_ERRMSG_SHOWN_STS = True
                Exit Sub
            End If
        Next

        If IsNumeric(Microsoft.VisualBasic.Mid(GSTIN, 12, 1)) Then
            MessageBox.Show("Invalid GSTIN-Invalid PAN Format(3)", "GSTIN VERIFICATION FAILED...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            v_ERRMSG_SHOWN_STS = True
            Exit Sub
        End If


        Dim REQSTR As String

        Try

        Catch ex As Exception

        End Try

        REQSTR = "https://deprecatedgstapi.charteredinfo.com/commonapi/v1.1/search?aspid=1612689919&password=Ruth@2009&Action=TP&Gstin=" & GSTIN.Trim
        'REQSTR = "https://gstapi.charteredinfo.com/commonapi/v1.1/search?aspid=1612689919&password=Ruth@2009&Action=TP&Gstin=" & GSTIN.Trim

        Try

            Dim request1 As WebRequest = WebRequest.Create(REQSTR)

            request1.Credentials = CredentialCache.DefaultCredentials

            Dim response1 As WebResponse = request1.GetResponse()

            Dim dataStream1 As Stream = response1.GetResponseStream()

            Dim reader1 As New StreamReader(dataStream1)

            Dim responseFromServer1 As String = reader1.ReadToEnd()

            Dim jResults1 As Newtonsoft.Json.Linq.JObject = Newtonsoft.Json.Linq.JObject.Parse(responseFromServer1)

            If jResults1.ContainsKey("error") Then

                MsgBox(jResults1("error")("message").ToString & "  Error Code : " & jResults1("error")("error_cd").ToString)

            Else



                v_Name = jResults1("tradeNam").ToString
                v_LegalName_Business = jResults1("lgnm").ToString
                'Nature = jResults1("ntr")
                'ConstitutionOfBusiness = jResults1("ctb")
                'RegDate = jResults1("rgdt")

                'Dim praddr As New GSTINADDRESS

                v_Address1 = jResults1("pradr")("addr")("flno").ToString
                v_Address1 = v_Address1.ToString & IIf(v_Address1.ToString.Trim <> "" And Microsoft.VisualBasic.Right(v_Address1.Trim, 1) <> ",", ", ", " ") & jResults1("pradr")("addr")("bno").ToString
                v_Address1 = v_Address1.ToString & IIf(v_Address1.ToString.Trim <> "" And Microsoft.VisualBasic.Right(v_Address1.Trim, 1) <> ",", ", ", " ") & jResults1("pradr")("addr")("bnm").ToString

                v_Address2 = jResults1("pradr")("addr")("st").ToString

                'If jResults1("pradr")("addr")("city").ToString.Trim <> "" And jResults1("pradr")("addr")("loc").ToString.Trim.ToLower <> jResults1("pradr")("addr")("city").ToString.Trim.ToLower Then

                'v_Address2 = v_Address2 & IIf(v_Address2.ToString.Trim <> "" And Microsoft.VisualBasic.Right(v_Address2.Trim, 1) <> ",", ", ", " ") & jResults1("pradr")("addr")("loc").ToString
                'v_Address3 = jResults1("pradr")("addr")("city").ToString

                'Else
                v_Address3 = jResults1("pradr")("addr")("loc").ToString

                'End If

                If jResults1("pradr")("addr")("loc").ToString.Trim.ToLower <> jResults1("pradr")("addr")("dst").ToString.Trim.ToLower Then
                    'If jResults1("pradr")("addr")("city").ToString.Trim.ToLower <> jResults1("pradr")("addr")("dst").ToString.Trim.ToLower Then
                    v_Address4 = jResults1("pradr")("addr")("dst").ToString
                    'End If
                End If



                v_city = "" ' jResults1("pradr")("addr")("city").ToString
                If v_city.Trim = "" Then
                    v_city = jResults1("pradr")("addr")("loc").ToString
                End If
                If v_city.Trim = "" Then
                    v_city = jResults1("pradr")("addr")("dst").ToString
                End If

                v_StateName = jResults1("pradr")("addr")("stcd").ToString

                v_pincode = jResults1("pradr")("addr")("pncd").ToString

                'ActiveStatus = jResults1("sts")

                'For Each ADD1 In jResults1("adadr")
                '    Dim addadd As New GSTINADDRESS
                '    addadd.BuildigName = ADD1("bnm")
                '    addadd.Street = ADD1("st")
                '    addadd.Locality = ADD1("loc")
                '    addadd.BuildigNo = ADD1("bno")
                '    addadd.State = ADD1("stcd")
                '    addadd.District = ADD1("dst")
                '    addadd.City = ADD1("city")
                '    addadd.PinCode = ADD1("pncd")
                '    addaddr.Add(addadd)
                'Next

            End If

            reader1.Close()
            response1.Close()

        Catch EX As Exception

            If InStr(1, Microsoft.VisualBasic.Trim(LCase(EX.Message)), Microsoft.VisualBasic.Trim(LCase("(400)"))) > 0 Then
                MessageBox.Show("Invalid GSTIN - GST No. does not exists", "GSTIN VERIFICATION FAILED...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Else
                MessageBox.Show("Invalid Response" & Chr(13) & EX.Message, "GSTIN VERIFICATION FAILED...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            End If
            v_ERRMSG_SHOWN_STS = True

        End Try

    End Sub



    'Public Function SEARCHGSTIN111111(GSTIN As String) As String

    '    ServicePointManager.Expect100Continue = True
    '    ServicePointManager.SecurityProtocol = CType(3072, SecurityProtocolType)

    '    If Len(GSTIN) <> 15 Then
    '        MsgBox("Invalid GSTIN-Invalid Length")

    '    End If

    '    If Not IsNumeric(Microsoft.VisualBasic.Left(GSTIN, 2)) Then
    '        MsgBox("Invalid GSTIN-Invalid State Code")
    '        Return RESP
    '    End If



    '    For i = 3 To 7
    '        If IsNumeric(Microsoft.VisualBasic.Mid(GSTIN, i, 1)) Then
    '            MsgBox("Invalid GSTIN-Invalid PAN Format")
    '            Return RESP
    '        End If
    '    Next

    '    For i = 8 To 11
    '        If Not IsNumeric(Microsoft.VisualBasic.Mid(GSTIN, i, 1)) Then
    '            MsgBox("Invalid GSTIN-Invalid PAN Format")
    '            Return RESP
    '        End If
    '    Next

    '    If IsNumeric(Microsoft.VisualBasic.Mid(GSTIN, 12, 1)) Then
    '        MsgBox("Invalid GSTIN@#$Invalid PAN Format")
    '        Return RESP
    '    End If



    '    Dim REQSTR As String

    '    Try

    '    Catch ex As Exception

    '    End Try
    '    REQSTR = "https://gstapi.charteredinfo.com/commonapi/v1.1/search?aspid=" & aspid & "&password=" & asppwd & "&Action=TP&Gstin=" & GSTIN


    '    Dim request1 As WebRequest = WebRequest.Create(REQSTR)

    '    request1.Credentials = CredentialCache.DefaultCredentials

    '    Dim response1 As WebResponse = request1.GetResponse()

    '    Dim dataStream1 As Stream = response1.GetResponseStream()

    '    Dim reader1 As New StreamReader(dataStream1)

    '    Dim responseFromServer1 As String = reader1.ReadToEnd()
    '    '  
    '    Dim jResults1 As Newtonsoft.Json.Linq.JObject = Newtonsoft.Json.Linq.JObject.Parse(responseFromServer1)

    '    If jResults1.ContainsKey("error") Then

    '        MsgBox(jResults1("error")("message").ToString & "  Error Code : " & jResults1("error")("error_cd").ToString

    '        Else

    '        TradeName = jResults1("tradeNam")
    '        LegalName = jResults1("lgnm")
    '        Nature = jResults1("ntr")
    '        ConstitutionOfBusiness = jResults1("ctb")
    '        RegDate = jResults1("rgdt")

    '        Dim praddr As New GSTINADDRESS

    '        praddr.BuildigName = jResults1("pradr")("addr")("bnm")
    '        praddr.Street = jResults1("pradr")("addr")("st")
    '        praddr.Locality = jResults1("pradr")("addr")("loc")
    '        praddr.BuildigNo = jResults1("pradr")("addr")("bno")
    '        praddr.State = jResults1("pradr")("addr")("stcd")
    '        praddr.District = jResults1("pradr")("addr")("dst")
    '        praddr.City = jResults1("pradr")("addr")("city")
    '        praddr.PinCode = jResults1("pradr")("addr")("pncd")

    '        ActiveStatus = jResults1("sts")

    '        For Each ADD1 In jResults1("adadr")
    '            Dim addadd As New GSTINADDRESS
    '            addadd.BuildigName = ADD1("bnm")
    '            addadd.Street = ADD1("st")
    '            addadd.Locality = ADD1("loc")
    '            addadd.BuildigNo = ADD1("bno")
    '            addadd.State = ADD1("stcd")
    '            addadd.District = ADD1("dst")
    '            addadd.City = ADD1("city")
    '            addadd.PinCode = ADD1("pncd")
    '            addaddr.Add(addadd)
    '        Next

    '    End If

    '    reader1.Close()
    '    response1.Close()

    '    Catch EX As Exception

    '    MsgBox(EX.Message & " Invalid Response "


    '    End Try

    'End Function


End Class
