Imports System.Security.Cryptography.X509Certificates
Imports System.Net
Imports System.Net.Security
Imports System.Net.Mail
Imports System.IO
Imports System.ComponentModel

Public Class New_EMail_Entry

    Implements Interface_MDIActions

    Private con As New SqlClient.SqlConnection(Common_Procedures.Connection_String)
    Private con1 As New SqlClient.SqlConnection(Common_Procedures.ConnectionString_CompanyGroupdetails)
    Private New_Entry As Boolean = False
    Private Mov_Status As Boolean = False
    Private Filter_Status As Boolean = False
    Private Insert_Entry As Boolean = False
    Private SaveAll_STS As Boolean = False

    Public Shared vMailID As String
    Public Shared vSubJect As String
    Public Shared vAttchFilepath As String
    Public Shared vMessage As String
    Private FrmLdSTS As Boolean = False
    Private NoCalc_Status As Boolean = False
    Private Prec_ActCtrl As New Control
    Private vcbo_KeyDwnVal As Double
    Private Other_Condition As String = ""
    Private Pk_Condition As String = ""
    Private PkCondition_Entry As String = ""


    Private Sub clear()
        pnl_Select_Party_Name_Details.Visible = True
        lbl_MailNo.Text = ""
        lbl_MailNo.ForeColor = Color.Black
        msk_Date.Text = ""
        msk_Date.Enabled = True
        msk_Date.BackColor = Color.White
        txt_Msg.Text = ""
        txt_PhnNo.Text = ""
        txt_Attachment.Text = ""
        txt_SubJect.Text = ""

        dgv_Party_Name_Details.Rows.Clear()
        New_Entry = True

    End Sub

    Private Sub EMAIL_Entry_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'Me.Left = Screen.PrimaryScreen.WorkingArea.Width - 30 - Me.Width
        'Me.Top = Screen.PrimaryScreen.WorkingArea.Height - 140 - Me.Height

        'lbl_Company.Text = ""
        'lbl_Company.Tag = 0
        'lbl_Company.Visible = False



        AddHandler cbo_PartyName.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_PhnNo.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_SubJect.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Attachment.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Msg.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_save.Enter, AddressOf ControlGotFocus

        AddHandler msk_Date.Enter, AddressOf ControlGotFocus

        AddHandler cbo_PartyName.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_PhnNo.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Attachment.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_SubJect.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Msg.LostFocus, AddressOf ControlLostFocus
        AddHandler msk_Date.Leave, AddressOf ControlLostFocus
        AddHandler btn_save.Leave, AddressOf ControlLostFocus

        lbl_Company.Text = ""
        lbl_Company.Tag = 0
        lbl_Company.Visible = False
        'Common_Procedures.CompIdNo = 0

        Filter_Status = False
        FrmLdSTS = True


        txt_PhnNo.Text = ""
        txt_SubJect.Text = ""
        txt_Msg.Text = ""

        LIST_ALL_LEDGERNAMES()

        new_record()




    End Sub
    Private Sub ControlGotFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim txtbx As TextBox
        Dim combobx As ComboBox

        On Error Resume Next

        If FrmLdSTS = True Or NoCalc_Status = True Then Exit Sub

        If TypeOf Me.ActiveControl Is TextBox Or TypeOf Me.ActiveControl Is ComboBox Or TypeOf Me.ActiveControl Is Button Then
            Me.ActiveControl.BackColor = Color.Lime
            Me.ActiveControl.ForeColor = Color.Blue
        End If

        If TypeOf Me.ActiveControl Is TextBox Then
            txtbx = Me.ActiveControl
            txtbx.SelectAll()
        ElseIf TypeOf Me.ActiveControl Is ComboBox Then
            combobx = Me.ActiveControl
            combobx.SelectAll()
        End If

        Grid_Cell_DeSelect()

        Prec_ActCtrl = Me.ActiveControl

    End Sub

    Private Sub ControlLostFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        On Error Resume Next
        If FrmLdSTS = True Or NoCalc_Status = True Then Exit Sub
        If IsNothing(Prec_ActCtrl) = False Then
            If TypeOf Prec_ActCtrl Is TextBox Or TypeOf Prec_ActCtrl Is ComboBox Then
                Prec_ActCtrl.BackColor = Color.White
                Prec_ActCtrl.ForeColor = Color.Black
            ElseIf TypeOf Prec_ActCtrl Is Button Then
                Prec_ActCtrl.BackColor = Color.FromArgb(44, 61, 90)
                Prec_ActCtrl.ForeColor = Color.White
            End If
        End If
    End Sub
    Private Sub move_record(ByVal no As String)
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim da3 As New SqlClient.SqlDataAdapter
        Dim dt3 As New DataTable
        Dim da4 As New SqlClient.SqlDataAdapter
        Dim dt4 As New DataTable
        Dim NewCode As String
        Dim n As Integer
        Dim SNo As Integer


        If Val(no) = 0 Then Exit Sub

        clear()

        Mov_Status = True

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(no) & "/" & Trim(Common_Procedures.FnYearCode)


        Try

            con = New SqlClient.SqlConnection(Common_Procedures.Connection_String)
            con.Open()

            da1 = New SqlClient.SqlDataAdapter("select a.* from Marketing_EMail_Head a Where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Email_Code = '" & Trim(NewCode) & "' ", con)
            dt1 = New DataTable
            da1.Fill(dt1)

            If dt1.Rows.Count > 0 Then
                lbl_MailNo.Text = dt1.Rows(0).Item("Email_No").ToString
                dtp_Date.Text = dt1.Rows(0).Item("Party_Mail_Date").ToString
                msk_Date.Text = dtp_Date.Text

                txt_PhnNo.Text = dt1.Rows(0).Item("To_Address").ToString
                txt_Msg.Text = dt1.Rows(0).Item("Message").ToString
                txt_Attachment.Text = dt1.Rows(0).Item("Attachment").ToString
                txt_SubJect.Text = dt1.Rows(0).Item("Subject").ToString
                New_Entry = False

            End If


            da2 = New SqlClient.SqlDataAdapter("select a.* , b.ledger_name,d.Contact_Designation_Name from Marketing_Party_EMail_Details a  INNER JOIN Ledger_Head b ON a.ledger_idno <> 0 and a.ledger_idno = b.ledger_idno  LEFT OUTER JOIN Contact_Designation_Head D ON a.Contact_Designation_IdNo= d.Contact_Designation_IdNo where a.Email_Code = '" & Trim(NewCode) & "' Order by a.sl_no", con)
            dt2 = New DataTable
            da2.Fill(dt2)
            With dgv_Party_Name_Details

                dgv_Party_Name_Details.Rows.Clear()
                SNo = 0

                If dt2.Rows.Count > 0 Then

                    For i = 0 To dt2.Rows.Count - 1

                        n = dgv_Party_Name_Details.Rows.Add()

                        SNo = SNo + 1
                        dgv_Party_Name_Details.Rows(n).Cells(0).Value = Val(SNo)
                        dgv_Party_Name_Details.Rows(n).Cells(1).Value = dt2.Rows(i).Item("ledger_name").ToString
                        dgv_Party_Name_Details.Rows(n).Cells(2).Value = Trim(dt2.Rows(i).Item("Contact_Person"))
                        dgv_Party_Name_Details.Rows(n).Cells(3).Value = Trim(dt2.Rows(i).Item("Contact_Designation_Name"))
                        dgv_Party_Name_Details.Rows(n).Cells(4).Value = Trim(Val(dt2.Rows(i).Item("Ledger_sts")))
                        dgv_Party_Name_Details.Rows(n).Cells(5).Value = Trim(Val(dt2.Rows(i).Item("ledger_idno")))
                        dgv_Party_Name_Details.Rows(n).Cells(6).Value = Trim(dt2.Rows(i).Item("Ledger_Emailid"))
                    Next i

                End If



            End With


            dt2.Clear()
            dt2.Dispose()
            da2.Dispose()



            dt1.Dispose()
            da1.Dispose()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT MOVE RECORDS...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub
    Private Sub TextBoxControlKeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs)
        On Error Resume Next
        If e.KeyValue = 38 Then e.Handled = True : SendKeys.Send("+{TAB}")
        If e.KeyValue = 40 Then e.Handled = True : SendKeys.Send("{TAB}")
    End Sub

    Private Sub TextBoxControlKeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
        On Error Resume Next
        If Asc(e.KeyChar) = 13 Then e.Handled = True : SendKeys.Send("{TAB}")
    End Sub
    Private Sub Grid_Cell_DeSelect()
        On Error Resume Next
        'If Not IsNothing(dgv_Details.CurrentCell) Then dgv_Details.CurrentCell.Selected = False
        If Not IsNothing(dgv_Party_Name_Details.CurrentCell) Then dgv_Party_Name_Details.CurrentCell.Selected = False
        'If Not IsNothing(dgv_Filter_Details.CurrentCell) Then dgv_Filter_Details.CurrentCell.Selected = False
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
        Dim MailID As String
        Dim MsgTxt As String
        Dim SubTxt As String
        Dim Atch_FlName As String
        Dim Led_IdNo As Integer
        Dim MailTxt As String
        Dim vPARTYMSELCMAILIDS As String
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim Dt2 As New DataTable
        Dim i As Integer, j As Integer, n As Integer, SNo As Integer


        Try

            Atch_FlName = Trim(txt_Attachment.Text)

            If Trim(Atch_FlName) <> "" Then
                If File.Exists(Atch_FlName) = False Then
                    MessageBox.Show("Invalid Attachment File, File does not exists", "DOES NOT SEND MAIL...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Exit Sub
                End If
            End If

            If Trim(txt_PhnNo.Text) <> "" Then


                vPARTYMSELCMAILIDS = Trim(txt_PhnNo.Text)

                MailID = vPARTYMSELCMAILIDS
                SubTxt = Trim(txt_SubJect.Text)
                MsgTxt = Trim(txt_Msg.Text)



                SmtpServer.Port = Val(Common_Procedures.settings.Email_Port)  ' 587
                SmtpServer.Host = Trim(Common_Procedures.settings.Email_Host)  ' "smtp.gmail.com"
                SmtpServer.UseDefaultCredentials = False
                SmtpServer.EnableSsl = True

                SmtpServer.Credentials = New Net.NetworkCredential(Trim(Common_Procedures.settings.Email_Address), Trim(Common_Procedures.settings.Email_Password))


                mail = New MailMessage()
                mail.From = New MailAddress(Trim(Common_Procedures.settings.Email_Address))
                mail.To.Add(Trim(MailID))
                mail.Subject = Trim(SubTxt)
                mail.Body = Trim(MsgTxt)

                If Trim(Atch_FlName) <> "" Then
                    Dim attachment As System.Net.Mail.Attachment
                    attachment = New System.Net.Mail.Attachment(Trim(Atch_FlName))
                    mail.Attachments.Add(attachment)
                End If

                SmtpServer.Send(mail)

            End If


            For i = 0 To dgv_Party_Name_Details.Rows.Count - 1
                If Val(dgv_Party_Name_Details.Rows(i).Cells(4).Value) = 1 Then
                    If Trim(dgv_Party_Name_Details.Rows(i).Cells(6).Value) <> "" Then

                        vPARTYMSELCMAILIDS = Trim(dgv_Party_Name_Details.Rows(i).Cells(6).Value)

                        'vPARTYMSELCMAILIDS = Trim(vPARTYMSELCMAILIDS) & IIf(Trim(vPARTYMSELCMAILIDS) <> "", ", ", "") & Trim(dgv_Party_Name_Details.Rows(i).Cells(6).Value)

                        MailID = vPARTYMSELCMAILIDS
                        SubTxt = Trim(txt_SubJect.Text)
                        MsgTxt = Trim(txt_Msg.Text)



                        SmtpServer.Port = Val(Common_Procedures.settings.Email_Port)  ' 587
                        SmtpServer.Host = Trim(Common_Procedures.settings.Email_Host)  ' "smtp.gmail.com"
                        SmtpServer.UseDefaultCredentials = False
                        SmtpServer.EnableSsl = True

                        SmtpServer.Credentials = New Net.NetworkCredential(Trim(Common_Procedures.settings.Email_Address), Trim(Common_Procedures.settings.Email_Password))
                        'SmtpServer.Credentials = New Net.NetworkCredential("tsoft.tirupur@gmail.com", "GOLD@tn39av7417")
                        'SmtpServer.Credentials = New Net.NetworkCredential("varalakshmithanges@gmail.com", "GOLD@tn39av7417")
                        'SmtpServer.Credentials = New Net.NetworkCredential("varalakshmithanges@gmail.com", "gold&VL@19=rj")
                        'SmtpServer.Credentials = New Net.NetworkCredential("t.thanges@gmail.com", "gold@tn39av7417")
                        'SmtpServer.Credentials = New Net.NetworkCredential("t.thanges@gmail.com", "cikysrpmkzbwliuc")
                        'SmtpServer.Credentials = New Net.NetworkCredential("varalakshmithanges@gmail.com", "thanges19")
                        'SmtpServer.Credentials = New Net.NetworkCredential("tsoft.tirupur@gmail.com", "8508403221")


                        mail = New MailMessage()
                        mail.From = New MailAddress(Trim(Common_Procedures.settings.Email_Address))
                        'mail.From = New MailAddress("varalakshmithanges@gmail.com")
                        'mail.From = New MailAddress("tsoft.tirupur@gmail.com")
                        'mail.From = New MailAddress("t.thanges@gmail.com")
                        'srirajatex@gmail.com
                        mail.To.Add(Trim(MailID))
                        mail.Subject = Trim(SubTxt)
                        mail.Body = Trim(MsgTxt)

                        If Trim(Atch_FlName) <> "" Then
                            Dim attachment As System.Net.Mail.Attachment
                            attachment = New System.Net.Mail.Attachment(Trim(Atch_FlName))
                            mail.Attachments.Add(attachment)
                        End If

                        SmtpServer.Send(mail)

                    End If
                End If
            Next




            MessageBox.Show("Mail send Sucessfully", "FOR MAILING...", MessageBoxButtons.OK, MessageBoxIcon.Information)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT SEND MAIL...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally
            mail.Dispose()
            SmtpServer.Dispose()

        End Try

    End Sub


    Private Sub btnSendMail_Click_1111(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim SmtpServer As New SmtpClient()
        Dim mail As New MailMessage()
        Dim MailID As String
        Dim MsgTxt As String
        Dim SubTxt As String
        Dim Atch_FlName As String
        Dim Led_IdNo As Integer
        Dim MailTxt As String
        Dim vPARTYMSELCMAILIDS As String
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim Dt2 As New DataTable
        Dim i As Integer, j As Integer, n As Integer, SNo As Integer


        Try

            Atch_FlName = Trim(txt_Attachment.Text)

            If Trim(Atch_FlName) <> "" Then
                If File.Exists(Atch_FlName) = False Then
                    MessageBox.Show("Invalid Attachment File, File does not exists", "DOES NOT SEND MAIL...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Exit Sub
                End If
            End If



            'With dgv_Party_Name_Details

            '    .Rows.Clear()
            '    SNo = 0

            '    Da = New SqlClient.SqlDataAdapter("select a.*, b.*  from PartyList_Mail_Details a INNER JOIN Ledger_Head b ON a.ledger_idno = b.ledger_idno   Order by a.Sl_No", con)
            '    Dt1 = New DataTable
            '    Da.Fill(Dt1)


            '    If Dt1.Rows.Count > 0 Then

            '        For i = 0 To Dt1.Rows.Count - 1

            '            n = .Rows.Add()
            '            SNo = SNo + 1
            '            .Rows(n).Cells(0).Value = Val(SNo)

            '            .Rows(n).Cells(1).Value = Dt1.Rows(i).Item("Ledger_Name").ToString
            '            '.Rows(n).Cells(2).Value = Dt1.Rows(i).Item("Party_OrderNo").ToString
            '            .Rows(n).Cells(3).Value = Dt1.Rows(i).Item("ledger_idno").ToString
            '            .Rows(n).Cells(4).Value = Dt1.Rows(i).Item("Ledger_Mail").ToString
            '        Next
            '    End If
            '    Dt1.Clear()
            'End With






            ' Led_IdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_PartyName.Text)

            'MailTxt = "INVOICE " & vbCrLf & vbCrLf
            'MailTxt = MailTxt & "Invoice No.-" & Trim(lbl_InvoiceNo.Text) & vbCrLf & "Date-" & Trim(msk_date.Text)
            'MailTxt = MailTxt & vbCrLf & "Lr No.-" & Trim(txt_LrNo.Text) & IIf(Trim(msk_Lr_Date.Text) <> "", " Dt.", "") & Trim(msk_Lr_Date.Text)
            'MailTxt = MailTxt & vbCrLf & "Value-" & Trim(lbl_NetAmount.Text)

            'New_EMail_Entry.vMailID = Common_Procedures.get_FieldValue(con, "Ledger_Head", "Ledger_Mail", "(Ledger_IdNo = " & Str(Val(Led_IdNo)) & ")")
            'New_EMail_Entry.vSubJect = "Invocie : " & Trim(lbl_InvoiceNo.Text)
            ' New_EMail_Entry.vMessage = Trim(MailTxt)


            vPARTYMSELCMAILIDS = Trim(txt_PhnNo.Text)
            For i = 0 To dgv_Party_Name_Details.Rows.Count - 1
                If Val(dgv_Party_Name_Details.Rows(i).Cells(4).Value) = 1 Then
                    If Trim(dgv_Party_Name_Details.Rows(i).Cells(6).Value) <> "" Then
                        vPARTYMSELCMAILIDS = Trim(vPARTYMSELCMAILIDS) & IIf(Trim(vPARTYMSELCMAILIDS) <> "", ", ", "") & Trim(dgv_Party_Name_Details.Rows(i).Cells(6).Value)
                    End If
                End If
            Next




            'Dim f1 As New EMAIL_Entry
            'f1.MdiParent = MDIParent1
            'f1.Show()

            MailID = vPARTYMSELCMAILIDS
            SubTxt = Trim(txt_SubJect.Text)
            MsgTxt = Trim(txt_Msg.Text)



            SmtpServer.Port = Val(Common_Procedures.settings.Email_Port)  ' 587
            SmtpServer.Host = Trim(Common_Procedures.settings.Email_Host)  ' "smtp.gmail.com"
            SmtpServer.UseDefaultCredentials = False
            SmtpServer.EnableSsl = True

            SmtpServer.Credentials = New Net.NetworkCredential(Trim(Common_Procedures.settings.Email_Address), Trim(Common_Procedures.settings.Email_Password))
            'SmtpServer.Credentials = New Net.NetworkCredential("tsoft.tirupur@gmail.com", "GOLD@tn39av7417")
            'SmtpServer.Credentials = New Net.NetworkCredential("varalakshmithanges@gmail.com", "GOLD@tn39av7417")
            'SmtpServer.Credentials = New Net.NetworkCredential("varalakshmithanges@gmail.com", "gold&VL@19=rj")
            'SmtpServer.Credentials = New Net.NetworkCredential("t.thanges@gmail.com", "gold@tn39av7417")
            'SmtpServer.Credentials = New Net.NetworkCredential("t.thanges@gmail.com", "cikysrpmkzbwliuc")
            'SmtpServer.Credentials = New Net.NetworkCredential("varalakshmithanges@gmail.com", "thanges19")
            'SmtpServer.Credentials = New Net.NetworkCredential("tsoft.tirupur@gmail.com", "8508403221")


            mail = New MailMessage()
            mail.From = New MailAddress(Trim(Common_Procedures.settings.Email_Address))
            'mail.From = New MailAddress("varalakshmithanges@gmail.com")
            'mail.From = New MailAddress("tsoft.tirupur@gmail.com")
            'mail.From = New MailAddress("t.thanges@gmail.com")
            'srirajatex@gmail.com
            mail.To.Add(Trim(MailID))
            mail.Subject = Trim(SubTxt)
            mail.Body = Trim(MsgTxt)

            If Trim(Atch_FlName) <> "" Then
                Dim attachment As System.Net.Mail.Attachment
                attachment = New System.Net.Mail.Attachment(Trim(Atch_FlName))
                mail.Attachments.Add(attachment)
            End If

            SmtpServer.Send(mail)

            MessageBox.Show("Mail send Sucessfully", "FOR MAILING...", MessageBoxButtons.OK, MessageBoxIcon.Information)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT SEND MAIL...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally
            mail.Dispose()
            SmtpServer.Dispose()

        End Try

    End Sub
    Private Sub SelectLedgerName_Selection()
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim n As Integer = 0
        Dim SNo As Integer = 0
        Dim Led_idno As Integer = 0
        Dim i As Integer, j As Integer
        Dim Mail_Idno As Integer = 0

        Da = New SqlClient.SqlDataAdapter("select * from Ledger_head where ledger_idno = " & Str(Val(Led_idno)), con)
        Dt1 = New DataTable
        Da.Fill(Dt1)

        Led_idno = Trim(Val(Dt1.Rows(i).Item("ledger_idno")))

        Mail_Idno = Trim(Val(Dt1.Rows(i).Item("Ledger_Mail")))

        dgv_Party_Name_Details.Rows.Clear()
        For i = 0 To Dt1.Rows.Count - 1

            n = dgv_Party_Name_Details.Rows.Add()
            SNo = SNo + 1
            dgv_Party_Name_Details.Rows(n).Cells(0).Value = Val(SNo)
            'dgv_Print_Details.Rows(n).Cells(1).Value = cbo_PartyName(i).Cells(1).Value
            Led_idno = Common_Procedures.Ledger_NameToIdNo(con, dgv_Party_Name_Details.Rows(i).Cells(1).Value)
            dgv_Party_Name_Details.Rows(n).Cells(4).Value = "1"


            ' empidno = 0 ' Common_Procedures.Employee_NameToIdNo(con, dgv_Details.Rows(i).Cells(1).Value)
            dgv_Party_Name_Details.Rows(n).Cells(5).Value = Led_idno

            dgv_Party_Name_Details.Rows(n).Cells(6).Value = Mail_Idno
        Next
    End Sub
    Public Sub delete_record() Implements Interface_MDIActions.delete_record
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim cmd As New SqlClient.SqlCommand
        Dim trans As SqlClient.SqlTransaction
        Dim NewCode As String = ""
        Dim PkCode As String = ""

        Dim vOrdByNo As String = ""

        ' vOrdByNo = Common_Procedures.OrderBy_CodeToValue(lbl_MailNo.Text)




        If Val(lbl_Company.Tag) = 0 Then
            MessageBox.Show("Invalid Company Selection", "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
        End If

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_MailNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)
        If pnl_Back.Enabled = False Then
            MessageBox.Show("Close Other Windows", "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
        End If

        If MessageBox.Show("Do you want to Delete?", "FOR DELETION...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) <> Windows.Forms.DialogResult.Yes Then
            Exit Sub
        End If

        'If New_Entry = True Then
        '    MessageBox.Show("This is New Entry", "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
        '    Exit Sub
        'End If
        ' PkCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_MailNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)
        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_MailNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)


        trans = con.BeginTransaction

        Try

            cmd.Connection = con
            cmd.Transaction = trans
            cmd.CommandText = "delete from Marketing_Party_EMail_Details where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Email_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()
            cmd.CommandText = "delete from Marketing_EMail_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Email_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            trans.Commit()

            new_record()

            MessageBox.Show("Deleted Sucessfully!!!", "FOR DELETION...", MessageBoxButtons.OKCancel, MessageBoxIcon.Information)

        Catch ex As Exception
            trans.Rollback()
            MessageBox.Show(ex.Message, "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Finally

            Dt1.Dispose()
            Da.Dispose()
            trans.Dispose()
            cmd.Dispose()

            If msk_Date.Enabled = True And msk_Date.Visible = True Then msk_Date.Focus()

        End Try


    End Sub
    Public Sub save_record() Implements Interface_MDIActions.save_record
        Dim cmd As New SqlClient.SqlCommand
        Dim trans As SqlClient.SqlTransaction
        Dim Sur As String
        Dim vOrdByNo As String = ""
        Dim NewCode As String = ""
        Dim vEmailNo As String = ""
        Dim Sno As Integer = 0
        Dim Party_Idno As Integer = 0
        Dim tr As SqlClient.SqlTransaction
        Dim Nr As Integer = 0
        Dim Desg_Id As Integer = 0
        Dim Vdesignation_Id As Integer = 0

        con = New SqlClient.SqlConnection(Common_Procedures.Connection_String)
        con.Open()




        If Val(lbl_Company.Tag) = 0 Then
            MessageBox.Show("Invalid Company Selection", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
        End If

        NewCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_MailNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)
        If pnl_Back.Enabled = False Then
            MessageBox.Show("Close other window", "DOES NOT SAVE", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        'If Trim(txt_PhnNo.Text) = "" Then
        '    MessageBox.Show("Invalid TO Address", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
        '    If txt_PhnNo.Enabled And txt_PhnNo.Visible Then txt_PhnNo.Focus()
        '    Exit Sub
        'End If
        'With dgv_Party_Name_Details
        '    If Party_Idno = 0 Then
        '        MessageBox.Show("Invalid Count Name", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
        '        If .Enabled And .Visible Then
        '            .Focus()
        '            .CurrentCell = .Rows(0).Cells(1)
        '        End If
        '        Exit Sub
        '    End If
        'End With

        'NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_MailNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)
        'cmd.Connection = con
        'cmd.Parameters.Clear()

        'cmd.Parameters.AddWithValue("@EntryDate", dtp_Date.Value)
        vEmailNo = Trim(lbl_MailNo.Text)


        tr = con.BeginTransaction

        Try


            If Insert_Entry = True Or New_Entry = False Then
                NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_MailNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            Else

                lbl_MailNo.Text = Common_Procedures.get_MaxCode(con, "Marketing_EMail_Head", "Email_Code", "For_OrderBy", "", Val(lbl_Company.Tag), Common_Procedures.FnYearCode, tr)

                NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_MailNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            End If

            cmd.Connection = con
            cmd.Transaction = tr

            cmd.Parameters.Clear()
            cmd.Parameters.AddWithValue("@EntryDate", dtp_Date.Value)

            vOrdByNo = Common_Procedures.OrderBy_CodeToValue(lbl_MailNo.Text)



            If New_Entry = True Then

                cmd.CommandText = "Insert into Marketing_EMail_Head(Email_Code ,        Company_IdNo      ,                       for_OrderBy     ,              Email_No ,        Party_Mail_Date,         To_Address,                         Attachment,                        Subject,                         Message       ) values 
                                                       ('" & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", " & Str(Val(vOrdByNo)) & ",   '" & Trim(vEmailNo) & "',       @EntryDate   ,'" & Trim(txt_PhnNo.Text) & "', '" & Trim(txt_Attachment.Text) & "',   '" & Trim(txt_SubJect.Text) & "' ,'" & Trim(txt_Msg.Text) & "'  )"
                cmd.ExecuteNonQuery()

            Else
                cmd.CommandText = "update Marketing_EMail_Head set Party_Mail_Date = @EntryDate,   Email_No= '" & Trim(vEmailNo) & "', To_Address = '" & Trim(txt_PhnNo.Text) & "', Attachment ='" & Trim(txt_Attachment.Text) & "', Subject =  '" & Trim(txt_SubJect.Text) & "' ,Message ='" & Trim(txt_Msg.Text) & "'  where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " And Email_Code = '" & Trim(NewCode) & "'"

                Nr = cmd.ExecuteNonQuery()

            End If

            cmd.CommandText = "Delete from Marketing_Party_EMail_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Email_Code = '" & Trim(NewCode) & "'"
            Nr = cmd.ExecuteNonQuery()

            With dgv_Party_Name_Details

                Sno = 0

                For i = 0 To .RowCount - 1

                    Sno = Sno + 1




                    If Trim(.Rows(i).Cells(5).Value) <> "" And Val(.Rows(i).Cells(4).Value) = 1 Then

                        Vdesignation_Id = Common_Procedures.Contact_Designation_NameToIdNo(con, .Rows(i).Cells(3).Value, tr)

                        cmd.CommandText = "Insert into Marketing_Party_EMail_Details (   Email_Code   ,              Company_IdNo        ,            Email_No     ,             for_OrderBy     ,    Party_Mail_Date,        Sl_No      ,          Contact_Person   ,                      Contact_Designation_IdNo,                            Ledger_sts,                         ledger_idno  ,                                Ledger_Emailid                  ) " &
                                               "          Values               ('" & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(vEmailNo) & "', " & Str(Val(vOrdByNo)) & ",    @EntryDate      , " & Str(Val(Sno)) & ",   '" & Trim(.Rows(i).Cells(2).Value) & "' ,  " & Str(Val(Vdesignation_Id)) & ", " & Str(Val(.Rows(i).Cells(4).Value)) & ", " & Str(Val(.Rows(i).Cells(5).Value)) & " , '" & Trim(.Rows(i).Cells(6).Value) & "' ) "
                        cmd.ExecuteNonQuery()

                    End If
                Next
            End With

            tr.Commit()
            If SaveAll_STS <> True Then
                MessageBox.Show("Saved Sucessfully!!!", "FOR SAVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Information)
            End If

            If Val(Common_Procedures.settings.OnSave_MoveTo_NewEntry_Status) = 1 Then
                If New_Entry = True Then
                    new_record()
                Else
                    move_record(lbl_MailNo.Text)
                End If
            Else
                move_record(lbl_MailNo.Text)
            End If


        Catch ex As Exception
            tr.Rollback()
            MessageBox.Show(ex.Message, "FOR SAVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)


        Finally

            cmd.Dispose()

        End Try
    End Sub

    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub

    Private Sub cbo_PartyName_GotFocus(sender As Object, e As EventArgs) Handles cbo_PartyName.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_Head", "Ledger_Name", "(Ledger_Mail <> '' and Close_Status=0)", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_PartyName_KeyDown(sender As Object, e As KeyEventArgs) Handles cbo_PartyName.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_PartyName, Nothing, Nothing, "Ledger_Head", "Ledger_Name", "(Ledger_Mail <> '' and Close_Status=0)", "(Ledger_IdNo = 0)")

    End Sub

    Private Sub cbo_PartyName_KeyPress(sender As Object, e As KeyPressEventArgs) Handles cbo_PartyName.KeyPress
        Dim Party_Id As Integer = 0
        Dim Party_Id1 As Integer = 0
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_PartyName, Nothing, "Ledger_Head", "Ledger_Name", "(Ledger_Mail <> '' and Close_Status=0)", "(Ledger_IdNo = 0)")
        If Asc(e.KeyChar) = 13 Then

            Party_Id = Common_Procedures.Ledger_NameToIdNo(con, cbo_PartyName.Text)

            With dgv_Party_Name_Details

                For i = 0 To .RowCount - 1

                    If Trim(.Rows(i).Cells(1).Value) <> "" Then

                        Party_Id1 = Val(.Rows(i).Cells(3).Value)

                        If Val(Party_Id) = Val(Party_Id1) Then
                            If .Enabled And .Visible Then
                                .Focus()
                                .CurrentCell = .Rows(i).Cells(0)
                            End If
                            Exit Sub
                        End If

                    End If
                Next

            End With

        End If
    End Sub

    Private Sub btn__Select_Click(sender As Object, e As EventArgs) Handles btn__Select.Click
        If dgv_Party_Name_Details.Rows.Count > 0 Then
            For i = 0 To dgv_Party_Name_Details.Rows.Count - 1
                dgv_Party_Name_Details.Rows(i).Cells(4).Value = 1

            Next
            txt_PhnNo.Focus()
        End If

    End Sub

    Private Sub btn__Deselect_Click(sender As Object, e As EventArgs) Handles btn__Deselect.Click
        If dgv_Party_Name_Details.Rows.Count > 0 Then
            For i = 0 To dgv_Party_Name_Details.Rows.Count - 1
                dgv_Party_Name_Details.Rows(i).Cells(4).Value = ""
            Next
            txt_PhnNo.Focus()
        End If
    End Sub

    Private Sub dgv_Party_Name_Details_CellClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Party_Name_Details.CellClick
        Select_PartyName(e.RowIndex)
    End Sub


    Private Sub Select_PartyName(ByVal RwIndx As Integer)
        Dim i As Integer

        With dgv_Party_Name_Details

            If .RowCount > 0 And RwIndx >= 0 Then

                .Rows(RwIndx).Cells(4).Value = (Val(.Rows(RwIndx).Cells(4).Value) + 1) Mod 2
                If Val(.Rows(RwIndx).Cells(4).Value) = 1 Then

                    For i = 0 To .ColumnCount - 1
                        .Rows(RwIndx).Cells(i).Style.ForeColor = Color.Blue
                    Next


                Else
                    .Rows(RwIndx).Cells(4).Value = ""

                    For i = 0 To .ColumnCount - 1
                        .Rows(RwIndx).Cells(i).Style.ForeColor = Color.Black
                    Next

                End If


            End If

        End With

    End Sub
    Private Sub dgv_Party_Name_Details_KeyDown(sender As Object, e As KeyEventArgs) Handles dgv_Party_Name_Details.KeyDown
        Dim n As Integer

        On Error Resume Next

        If e.KeyCode = Keys.Enter Or e.KeyCode = Keys.Space Then
            If dgv_Party_Name_Details.CurrentCell.RowIndex >= 0 Then

                n = dgv_Party_Name_Details.CurrentCell.RowIndex

                Select_PartyName(n)

                e.Handled = True

            End If
        End If
    End Sub

    Private Sub cbo_PartyName_KeyUp(sender As Object, e As KeyEventArgs) Handles cbo_PartyName.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then

            Dim f As New Ledger_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_PartyName.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If
    End Sub

    'Private Sub dgv_Party_Name_Details_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgv_Party_Name_Details.CellClick
    '    SelectLedgerName_Selection()
    'End Sub

    Private Sub cbo_mail_id_GotFocus(sender As Object, e As EventArgs)
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_Head", "Ledger_Mail", "(date_status<>1)", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_mail_id_KeyDown(sender As Object, e As KeyEventArgs)
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_mail_id, Nothing, Nothing, "Ledger_Head", "Ledger_Mail", "(date_status<>1)", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_mail_id_KeyPress(sender As Object, e As KeyPressEventArgs)
        Dim Party_Mail_Id As Integer = 0
        Dim Party_Mail_Id1 As Integer = 0
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_mail_id, Nothing, "Ledger_Head", "Ledger_Mail", "(date_status<>1)", "(Ledger_IdNo = 0)")
        If Asc(e.KeyChar) = 13 Then
            With dgv_Party_Name_Details

                For i = 0 To .RowCount - 1
                    Party_Mail_Id = Common_Procedures.Ledger_NameToIdNo(con, cbo_mail_id.Text)
                    If Trim(.Rows(i).Cells(1).Value) <> "" Then

                        Party_Mail_Id1 = Common_Procedures.Ledger_NameToIdNo(con, .Rows(i).Cells(4).Value)
                        If Val(Party_Mail_Id) = Val(Party_Mail_Id1) Then

                            If .Enabled And .Visible Then
                                .Focus()
                                .CurrentCell = .Rows(i).Cells(0)
                            End If
                            Exit Sub
                        End If

                    End If
                Next

            End With

        End If
    End Sub

    Private Sub LIST_ALL_LEDGERNAMES()

        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim n As Integer = 0
        Dim SNo As Integer = 0
        Dim Led_idno As Integer = 0
        Dim i As Integer, j As Integer
        Dim Mail_Idno As Integer = 0

        dgv_Party_Name_Details.Rows.Clear()

        Da = New SqlClient.SqlDataAdapter("select a.Ledger_Name, a.Ledger_idno , b.*, c.Contact_Designation_Name from Ledger_head a INNER JOIN Ledger_ContactName_Details b ON  b.Ledger_Emailid <> '' and a.Ledger_Idno = b.Ledger_Idno LEFT OUTER JOIN Contact_Designation_Head C ON b.Contact_Designation_IdNo= c.Contact_Designation_IdNo  where b.Ledger_Emailid <> '' and Close_Status=0 Order by ledger_name", con)
        Dt1 = New DataTable
        Da.Fill(Dt1)

        For i = 0 To Dt1.Rows.Count - 1

            n = dgv_Party_Name_Details.Rows.Add()

            SNo = SNo + 1
            dgv_Party_Name_Details.Rows(n).Cells(0).Value = Val(SNo)
            dgv_Party_Name_Details.Rows(n).Cells(1).Value = Trim(Dt1.Rows(i).Item("Ledger_Name"))
            dgv_Party_Name_Details.Rows(n).Cells(2).Value = Trim(Dt1.Rows(i).Item("Contact_Person"))
            dgv_Party_Name_Details.Rows(n).Cells(3).Value = Trim(Dt1.Rows(i).Item("Contact_Designation_Name"))
            dgv_Party_Name_Details.Rows(n).Cells(4).Value = ""
            dgv_Party_Name_Details.Rows(n).Cells(5).Value = Trim(Val(Dt1.Rows(i).Item("ledger_idno")))
            dgv_Party_Name_Details.Rows(n).Cells(6).Value = Trim(Dt1.Rows(i).Item("Ledger_Emailid"))

        Next
        Dt1.Clear()
        Dt1.Dispose()
        Da.Dispose()


    End Sub

    Public Sub new_record() Implements Interface_MDIActions.new_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim NewID As Integer = 0

        Try
            clear()

            New_Entry = True

            lbl_MailNo.Text = Common_Procedures.get_MaxCode(con, "Marketing_EMail_Head", "Email_Code", "For_OrderBy", "", Val(lbl_Company.Tag), Common_Procedures.FnYearCode)
            lbl_MailNo.ForeColor = Color.Red

            msk_Date.Text = Date.Today.ToShortDateString
            'da = New SqlClient.SqlDataAdapter("select top 1 * from Marketing_EMail_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Email_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' and " & Other_Condition & "   Email_No ", con)
            'dt1 = New DataTable
            'da.Fill(dt1)
            'If dt1.Rows.Count > 0 Then
            '    If Val(Common_Procedures.settings.PreviousEntryDate_ByDefault) = 1 Then '---- M.S Textiles (Tirupur)
            '        If dt1.Rows(0).Item("Party_Mail_Date").ToString <> "" Then msk_Date.Text = dt1.Rows(0).Item("Party_Mail_Date").ToString
            '    End If
            'End If
            dt1.Clear()


        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR NEW RECORD...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Finally
            '    If msk_Date.Enabled And msk_Date.Visible Then
            '        msk_Date.Focus()
            '        msk_Date.SelectionStart = 0
            '    End If
            txt_PhnNo.Focus()

        End Try

        dt1.Dispose()
        da.Dispose()

        LIST_ALL_LEDGERNAMES()

    End Sub

    Public Sub open_record() Implements Interface_MDIActions.open_record
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim movno As String, inpno As String
        Dim RecCode As String

        Try

            inpno = InputBox("Enter Mail.No", "FOR FINDING...")

            RecCode = Trim(PkCondition_Entry) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)
            RecCode = Replace(RecCode, "'", "''")

            Da = New SqlClient.SqlDataAdapter("select Email_No from Marketing_EMail_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Email_Code = '" & Trim(RecCode) & "'", con)
            Dt = New DataTable
            Da.Fill(Dt)

            movno = ""
            If Dt.Rows.Count > 0 Then
                If IsDBNull(Dt.Rows(0)(0).ToString) = False Then
                    movno = Trim(Dt.Rows(0)(0).ToString)
                End If
            End If

            Dt.Clear()
            Dt.Dispose()
            Da.Dispose()

            If Val(movno) <> 0 Then
                move_record(movno)

            Else
                MessageBox.Show("MailRef No. does not exists", "DOES NOT FIND...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT FIND...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Public Sub insert_record() Implements Interface_MDIActions.insert_record
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim movno As String, inpno As String
        Dim InvCode As String

        'If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.Cotton_Waste_Sales_Entry, "~L~") = 0 And InStr(Common_Procedures.UR.Cotton_Waste_Sales_Entry, "~I~") = 0 Then MessageBox.Show("You have No Rights to Insert", "DOES NOT INSERT...", MessageBoxButtons.OK, MessageBoxIcon.Error) : Exit Sub

        Try

            inpno = InputBox("Enter New Mail No.", "FOR NEW MAIL NO. INSERTION...")

            InvCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

            Da = New SqlClient.SqlDataAdapter("select Email_No from Marketing_EMail_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Email_Code = '" & Trim(InvCode) & "'", con)
            Da.Fill(Dt)

            movno = ""
            If Dt.Rows.Count > 0 Then
                If IsDBNull(Dt.Rows(0)(0).ToString) = False Then
                    movno = Trim(Dt.Rows(0)(0).ToString)
                End If
            End If

            Dt.Clear()

            If Val(movno) <> 0 Then
                move_record(movno)

            Else
                If Val(inpno) = 0 Then
                    MessageBox.Show("Invalid Ref No.", "DOES NOT INSERT NEW Ref...", MessageBoxButtons.OK, MessageBoxIcon.Error)

                Else
                    new_record()
                    Insert_Entry = True
                    lbl_MailNo.Text = Trim(UCase(inpno))

                End If

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT INSERT NEW BILL...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally
            Dt.Dispose()
            Da.Dispose()

        End Try
    End Sub

    Public Sub filter_record() Implements Interface_MDIActions.filter_record
        Throw New NotImplementedException()
    End Sub


    Public Sub movefirst_record() Implements Interface_MDIActions.movefirst_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim movno As String

        Try

            da = New SqlClient.SqlDataAdapter("select top 1 Email_No from Marketing_EMail_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Email_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby, Email_No", con)
            dt = New DataTable
            da.Fill(dt)

            movno = ""
            If dt.Rows.Count > 0 Then
                If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                    movno = Trim(dt.Rows(0)(0).ToString)
                End If
            End If

            dt.Clear()
            dt.Dispose()
            da.Dispose()

            If Val(movno) <> 0 Then move_record(movno)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR  MOVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try
    End Sub

    Public Sub movenext_record() Implements Interface_MDIActions.movenext_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim movno As String = ""
        Dim OrdByNo As Single = 0

        Try

            OrdByNo = Common_Procedures.OrderBy_CodeToValue(Trim(lbl_MailNo.Text))

            con = New SqlClient.SqlConnection(Common_Procedures.Connection_String)
            con.Open()

            da = New SqlClient.SqlDataAdapter("select top 1 Email_No from Marketing_EMail_Head where for_orderby > " & Str(OrdByNo) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and Email_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' and Email_Code LIKE '" & Trim(Pk_Condition) & "%' Order by for_Orderby, Email_No", con)
            dt = New DataTable
            da.Fill(dt)

            movno = ""
            If dt.Rows.Count > 0 Then
                If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                    movno = dt.Rows(0)(0).ToString
                End If
            End If

            If Val(movno) <> 0 Then move_record(movno)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR  MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally
            dt.Dispose()
            da.Dispose()

        End Try

    End Sub

    Public Sub moveprevious_record() Implements Interface_MDIActions.moveprevious_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim movno As String = ""
        Dim OrdByNo As Single = 0

        Try

            OrdByNo = Common_Procedures.OrderBy_CodeToValue(Trim(lbl_MailNo.Text))

            con = New SqlClient.SqlConnection(Common_Procedures.Connection_String)
            con.Open()

            da = New SqlClient.SqlDataAdapter("select top 1 Email_No from Marketing_EMail_Head where for_orderby < " & Str(Val(OrdByNo)) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and Email_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "'   Order by for_Orderby desc, Email_No desc", con)
            dt = New DataTable
            da.Fill(dt)

            movno = ""
            If dt.Rows.Count > 0 Then
                If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                    movno = dt.Rows(0)(0).ToString
                End If
            End If

            If Val(movno) <> 0 Then move_record(movno)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR  MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally
            dt.Dispose()
            da.Dispose()

        End Try
    End Sub

    Public Sub movelast_record() Implements Interface_MDIActions.movelast_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim movno As String = ""

        Try

            con = New SqlClient.SqlConnection(Common_Procedures.Connection_String)
            con.Open()

            da = New SqlClient.SqlDataAdapter("select top 1 Email_No from Marketing_EMail_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Email_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "'   Order by for_Orderby desc, Email_No desc", con)
            dt = New DataTable
            da.Fill(dt)

            movno = ""
            If dt.Rows.Count > 0 Then
                If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                    movno = dt.Rows(0)(0).ToString
                End If
            End If

            If Val(movno) <> 0 Then move_record(movno)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR  MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally
            dt.Dispose()
            da.Dispose()

        End Try

    End Sub

    Public Sub print_record() Implements Interface_MDIActions.print_record
        Throw New NotImplementedException()
    End Sub

    Private Sub New_EMail_Entry_Activated(sender As Object, e As EventArgs) Handles Me.Activated
        If FrmLdSTS = True Then
            lbl_Company.Text = ""
            lbl_Company.Tag = 0
            lbl_Company.Text = Common_Procedures.get_Company_From_CompanySelection(con)
            lbl_Company.Tag = Val(Common_Procedures.CompIdNo)


            Me.Text = lbl_Company.Text
            new_record()
        End If
    End Sub

    Private Sub btn_save_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_save.Click
        save_record()
    End Sub
End Class