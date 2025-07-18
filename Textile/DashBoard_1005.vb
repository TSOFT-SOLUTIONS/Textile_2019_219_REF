Imports System.IO
Imports System.Security.Cryptography.X509Certificates
Imports System.Net
Imports System.Net.Security
Imports System.Net.Mail
Imports System.IO.Ports
'Imports System.Windows.Forms.DataVisualization.Charting

Public Class DashBoard_1005
    Implements Interface_MDIActions

    Private con As New SqlClient.SqlConnection(Common_Procedures.Connection_String)
    Private Current_Date As Date = Now
    Private CurX As Integer = 0


    Private Sub lbl_Close_Left_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Me.Close()
    End Sub

    Private Sub DashBoard_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        con.Close()
    End Sub



    Private Sub DashBoard_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        con.Open()


        Me.Top = 0
        Me.Left = 0
        Me.Width = Screen.PrimaryScreen.WorkingArea.Width - 9 ' 15
        Me.Height = Screen.PrimaryScreen.WorkingArea.Height - 90 ' 100


        pnl_Back.Top = 0
        pnl_Back.Left = 0
        pnl_Back.Width = Me.Width
        pnl_Back.Height = Me.Height


        btn_Close.Left = Me.Width - 40
        btn_Close.Top = Me.Top + 10
        CurX = 10


        dgv_OverDueInvoices.DefaultCellStyle.ForeColor = Color.Blue
        dgv_OverDueBills.DefaultCellStyle.ForeColor = Color.Blue

        '    pnl_OverDue.Top = Panel1.Top

        Display()


    End Sub

    Private Sub Display()


        OverDue_Purchase()
        OverDue_Sales()
        Aged_PurchaseBills()
        Aged_SalesBills()
        Net_Income()
        'Chart_IncomeAndExpense()
        '   Pie_Chart()
        ' Active_Orders()

    End Sub
    Private Sub OverDue_Purchase()
        Dim cmd As New SqlClient.SqlCommand
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim Dt1 As New DataTable
        Dim n As Integer = 0
        Dim ParentCode As String = "~14~11~"   '-Sundry Creditors



        cmd.Connection = con

        cmd.Parameters.Clear()
        cmd.Parameters.AddWithValue("@uptodate", Current_Date)
        cmd.Parameters.AddWithValue("@companyfromdate", Common_Procedures.Company_FromDate)



        cmd.CommandText = "truncate table " & Trim(Common_Procedures.ReportTempSubTable) & ""
        cmd.ExecuteNonQuery()

        '======Bill to Bill
        'cmd.CommandText = "insert into " & Trim(Common_Procedures.ReportTempSubTable) & " ( int1  ,name1                , int2          , currency1 ) " & _
        '                           "Select tZ.company_idno   ,a.Voucher_Bill_Code  , tP.ledger_idno, sum(a.Debit_Amount)  from voucher_bill_head a INNER JOIN company_head tz  ON a.company_idno <> 0 and a.company_idno = tZ.company_idno INNER JOIN Ledger_Head tP ON a.Ledger_IdNo <> 0 and a.Ledger_IdNo = tP.Ledger_IdNo where a.voucher_bill_date <= @uptodate and  tP.Parent_Code = '" & Trim(ParentCode) & "' group by tZ.company_idno, tP.ledger_idno,a.Voucher_Bill_Code"
        'cmd.ExecuteNonQuery()

        'cmd.CommandText = "insert into " & Trim(Common_Procedures.ReportTempSubTable) & " ( int1  ,Name1               , int2          , currency2 ) " & _
        '                         "Select   tZ.company_idno   ,a.Voucher_Bill_Code , tP.ledger_idno, sum(a.Credit_Amount)  from voucher_bill_Head a INNER JOIN company_head tZ ON a.company_idno <> 0 and a.company_idno = tZ.company_idno INNER JOIN Ledger_Head tP ON a.Ledger_IdNo <> 0 and a.Ledger_IdNo = tP.Ledger_IdNo Where  a.voucher_bill_date <= @uptodate and  tP.Parent_Code = '" & Trim(ParentCode) & "' group by tZ.company_idno, tP.ledger_idno ,a.Voucher_Bill_Code"
        'cmd.ExecuteNonQuery()

        cmd.CommandText = "Insert into " & Trim(Common_Procedures.ReportTempSubTable) & " ( int1           ,Name1           , Int2           , Name2           , currency1 )  " & _
                                             " select tZ.company_idno , a.voucher_Code ,tL.ledger_Idno  ,  tL.ledger_Name , sum(a.voucher_amount) from voucher_details a, ledger_head tL, company_head tZ Where  a.voucher_date >= @companyfromdate and voucher_date <= @uptodate and a.company_idno = tz.company_idno and a.ledger_idno = tL.ledger_idno and tL.Parent_Code = '" & Trim(ParentCode) & "' group by tZ.company_idno ,a.voucher_Code ,tl.ledger_idno,tL.ledger_Name having sum(a.voucher_amount) <> 0"
        cmd.ExecuteNonQuery()


        cmd.CommandText = "truncate table " & Trim(Common_Procedures.ReportTempTable) & ""
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Insert into " & Trim(Common_Procedures.ReportTempTable) & " ( int1      , int2 ,name1 ,Name2 , currency1         ,Currency2) " & _
                                             " Select int1   , int2 ,name1 ,Name2 ,  sum(currency1)   , sum(currency2) from " & Trim(Common_Procedures.ReportTempSubTable) & "  group by  int1, int2,name1,Name2 having sum(currency1) <> 0 "
        cmd.ExecuteNonQuery()

        'cmd.CommandText = "truncate table " & Trim(Common_Procedures.ReportTempTable) & ""
        'cmd.ExecuteNonQuery()

        'cmd.CommandText = "insert into " & Trim(Common_Procedures.ReportTempTable) & " ( name1  ,int1          , currency1              ) " & _
        '                         " Select  tP.Ledger_Name  ,tP.Ledger_Idno, currency2 - currency1  from voucher_bill_head a, " & Trim(Common_Procedures.EntryTempTable) & " b, company_head tz, ledger_head tp Where a.voucher_bill_date <= @uptodate and (a.bill_amount- (case when b.currency1 is null then 0 else b.currency1 end)) <> 0 and a.voucher_bill_code = b.name1 and a.company_idno = b.int1 and a.ledger_idno = tP.ledger_idno and a.company_idno = tZ.company_idno  "
        'cmd.ExecuteNonQuery()

     


        Da = New SqlClient.SqlDataAdapter("select name2 as LedgerName,int1 as LedgerIdno ,sum(currency1) as Amount from " & Trim(Common_Procedures.ReportTempTable) & " group by name2 ,int1 Order by name2", con)
        Dt = New DataTable
        Da.Fill(Dt)

        If Dt.Rows.Count > 0 Then

            dgv_OverDueInvoices.Rows.Clear()
            dgv_OverDueInvoices.Height = 0

            For i = 0 To Dt.Rows.Count - 1

                If Val(Dt.Rows(i).Item("Amount").ToString) <> 0 Then
                    With dgv_OverDueInvoices

                        n = .Rows.Add()

                        .Rows(n).Cells(0).Value = "*"
                        .Rows(n).Cells(1).Value = Trim(Dt.Rows(i).Item("LedgerName").ToString)
                        .Rows(n).Cells(2).Value = Trim(Dt.Rows(i).Item("Amount").ToString)
                        .Rows(n).Cells(3).Value = "Remind"
                        .Rows(n).Cells(4).Value = Val(Dt.Rows(i).Item("LedgerIdno").ToString)

                        If .Height < 400 Then

                            CurX = CurX + 25

                            .Height = .Height + 25
                            ' pnl_OverDue.Height = pnl_OverDue.Height + 25
                        End If
                        '    Me.Height = Me.Height + 25
                    End With


                End If
            Next
        End If

        Dt.Clear()
        Da.Dispose()


    End Sub
    Private Sub OverDue_Sales()
        Dim cmd As New SqlClient.SqlCommand
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim Dt1 As New DataTable
        Dim n As Integer = 0
        Dim ParentCode As String = "~10~4~"   '-Sundry Debtors



        cmd.Connection = con

        cmd.Parameters.Clear()
        cmd.Parameters.AddWithValue("@uptodate", Current_Date)
        cmd.Parameters.AddWithValue("@companyfromdate", Common_Procedures.Company_FromDate)



        cmd.CommandText = "truncate table " & Trim(Common_Procedures.ReportTempSubTable) & ""
        cmd.ExecuteNonQuery()

        'cmd.CommandText = "insert into " & Trim(Common_Procedures.ReportTempSubTable) & " ( int1,name1              , int2          , currency1 ) " & _
        '                           "Select tZ.company_idno ,a.Voucher_Bill_Code, tP.ledger_idno, sum(a.Debit_Amount)  from voucher_bill_head a INNER JOIN company_head tz  ON a.company_idno <> 0 and a.company_idno = tZ.company_idno INNER JOIN Ledger_Head tP ON a.Ledger_IdNo <> 0 and a.Ledger_IdNo = tP.Ledger_IdNo where a.voucher_bill_date <= @uptodate and  tP.Parent_Code = '" & Trim(ParentCode) & "' group by tZ.company_idno, tP.ledger_idno,a.Voucher_Bill_Code"
        'cmd.ExecuteNonQuery()

        'cmd.CommandText = "insert into " & Trim(Common_Procedures.ReportTempSubTable) & " ( int1,Name1               , int2          , currency2 ) " & _
        '                         "Select   tZ.company_idno ,a.Voucher_Bill_Code , tP.ledger_idno, sum(a.Credit_Amount)  from voucher_bill_Head a INNER JOIN company_head tZ ON a.company_idno <> 0 and a.company_idno = tZ.company_idno INNER JOIN Ledger_Head tP ON a.Ledger_IdNo <> 0 and a.Ledger_IdNo = tP.Ledger_IdNo Where  a.voucher_bill_date <= @uptodate and  tP.Parent_Code = '" & Trim(ParentCode) & "' group by tZ.company_idno, tP.ledger_idno ,a.Voucher_Bill_Code"
        'cmd.ExecuteNonQuery()


        cmd.CommandText = "Insert into " & Trim(Common_Procedures.ReportTempSubTable) & " ( int1            , Int2           , Name2           , currency1 )  " & _
                                             " select tZ.company_idno  ,tL.ledger_Idno  ,  tL.ledger_Name ,  - 1 * sum(a.voucher_amount) from voucher_details a, ledger_head tL, company_head tZ Where  a.voucher_date >= @companyfromdate and voucher_date <= @uptodate and a.company_idno = tz.company_idno and a.ledger_idno = tL.ledger_idno and tL.Parent_Code = '" & Trim(ParentCode) & "' group by tZ.company_idno  ,tl.ledger_idno,tL.ledger_Name having sum(a.voucher_amount) <> 0"
        cmd.ExecuteNonQuery()





        cmd.CommandText = "truncate table " & Trim(Common_Procedures.ReportTempTable) & ""
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Insert into " & Trim(Common_Procedures.ReportTempTable) & " ( int1      , int2 ,name1 ,Name2 , currency1      ,Currency2) " & _
                                             " Select int1   , int2  ,name1 ,Name2 ,  sum(currency1) , sum(currency2) from " & Trim(Common_Procedures.ReportTempSubTable) & "  group by  int1, int2,name1 ,Name2"
        cmd.ExecuteNonQuery()

        'cmd.CommandText = "truncate table " & Trim(Common_Procedures.ReportTempTable) & ""
        'cmd.ExecuteNonQuery()

        'cmd.CommandText = "insert into " & Trim(Common_Procedures.ReportTempTable) & " ( name1 ,Int1           , currency1              ) " & _
        '                         " Select  tP.Ledger_Name ,tP.Ledger_Idno, currency1 - currency2  from voucher_bill_head a, " & Trim(Common_Procedures.EntryTempTable) & " b, company_head tz, ledger_head tp Where a.voucher_bill_date <= @uptodate and (a.bill_amount- (case when b.currency2 is null then 0 else b.currency2 end)) <> 0 and a.voucher_bill_code = b.name1 and a.company_idno = b.int1 and a.ledger_idno = tP.ledger_idno and a.company_idno = tZ.company_idno  "
        'cmd.ExecuteNonQuery()

        Da = New SqlClient.SqlDataAdapter("select name2 as LedgerName , Int2 as LedgerIdno, sum(currency1) as Amount from " & Trim(Common_Procedures.ReportTempTable) & " group by name1 ,name2,Int2 Order by name2", con)
        Dt = New DataTable
        Da.Fill(Dt)

        If Dt.Rows.Count > 0 Then

            dgv_OverDueBills.Rows.Clear()
            dgv_OverDueBills.Height = 0

            For i = 0 To Dt.Rows.Count - 1

                If Val(Dt.Rows(i).Item("Amount").ToString) <> 0 Then
                    With dgv_OverDueBills
                        n = .Rows.Add()

                        .Rows(n).Cells(0).Value = "*"
                        .Rows(n).Cells(1).Value = Trim(Dt.Rows(i).Item("LedgerName").ToString)
                        .Rows(n).Cells(2).Value = Trim(Dt.Rows(i).Item("Amount").ToString)
                        .Rows(n).Cells(3).Value = "Remind"
                        .Rows(n).Cells(4).Value = Val(Dt.Rows(i).Item("LedgerIdno").ToString)

                        If .Height < 400 Then
                            .Height = .Height + 25
                            'pnl_OverDue.Height = pnl_OverDue.Height + 25

                        End If
                        'Me.Height = Me.Height + 25
                    End With


                End If
            Next
        End If

        Dt.Clear()
        Da.Dispose()


    End Sub

    Private Sub dgv_OverDueBills_CellClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_OverDueBills.CellClick
        Dim vPhnNo As String = ""
        Dim Smstxt As String = ""
        Dim vLedId As Integer = 0

        With dgv_OverDueBills
            If e.ColumnIndex = 3 Then
                If Val(.Rows(e.RowIndex).Cells(2).Value) <> 0 Then
                    If MessageBox.Show("Do you want to send Reminder Sms to  " & Trim(.Rows(e.RowIndex).Cells(1).Value) & " ?", "FOR REMINDER SMS...", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = vbYes Then
                        vPhnNo = Common_Procedures.get_FieldValue(con, "Ledger_Head", "Ledger_PhoneNo", "Ledger_IdNo = " & Val(.Rows(e.RowIndex).Cells(4).Value))
                        If vPhnNo <> "" Then
                            Smstxt = "Mr. " & Trim(.Rows(e.RowIndex).Cells(1).Value) & " , Your Balance Bill Amount Rs." & Val(.Rows(e.RowIndex).Cells(2).Value) & "/- , Pay Immediatly.."

                            If REMINDER_SMS(vPhnNo, Smstxt, 1) = True Then
                                MessageBox.Show("Sms Send Successfully...", "SENDED..", MessageBoxButtons.OK, MessageBoxIcon.Information)
                            End If
                        Else
                            If MessageBox.Show("Phone No. not found.." & vbCrLf & "Do you want to add Phone No.", "NOT SEND..", MessageBoxButtons.YesNo, MessageBoxIcon.Information) = vbYes Then
                                If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1005" Then
                                    Dim F2 As New LedgerCreation_Processing
                                    F2.MdiParent = MDIParent1
                                    F2.Show()
                                    F2.move_record(Val(.Rows(e.RowIndex).Cells(4).Value))
                                Else
                                    Dim F2 As New Ledger_Creation
                                    F2.MdiParent = MDIParent1
                                    F2.Show()
                                    F2.move_record(Val(.Rows(e.RowIndex).Cells(4).Value))
                                End If
                             

                            End If
                        End If


                    End If
                End If

            End If


        End With

    End Sub
    Private Function REMINDER_SMS(ByVal vPhNo As String, ByVal smstext As String, Optional ByVal Gateway As Integer = 1) As Boolean
        Dim request As HttpWebRequest
        Dim response As HttpWebResponse = Nothing
        Dim url As String
        Dim timeout As Integer = 50000

        REMINDER_SMS = False

        Try
            url = ""
            If Gateway = 1 Then

                url = "http://sms1.shamsoft.in/api/mt/SendSMS?APIKey=" & Trim(Common_Procedures.settings.SMS_Provider_Key) & "&senderid=" & Trim(Common_Procedures.settings.SMS_Provider_SenderID) & "&channel=2&DCS=0&flashsms=0&number=" & Trim(vPhNo) & "&text=" & Trim(smstext) & "&route=" & Trim(Common_Procedures.settings.SMS_Provider_RouteID)

            ElseIf Gateway = 2 Then

                'url = "http://sms.shamsoft.in/app/smsapi/index.php?key=" & Trim(Common_Procedures.settings.SMS_Provider_Key_1) & "&routeid=" & Trim(Common_Procedures.settings.SMS_Provider_RouteID_1) & "&type=" & Trim(Common_Procedures.settings.SMS_Provider_Type_1) & "&contacts=" & Trim(vPhNo) & "&senderid=" & Trim(Common_Procedures.settings.SMS_Provider_SenderID_1) & "&msg=" & Trim(smstext)
            End If

            request = DirectCast(WebRequest.Create(url), HttpWebRequest)
            request.KeepAlive = True

            request.Timeout = timeout

            response = DirectCast(request.GetResponse(), HttpWebResponse)

            'If Trim(UCase(response.StatusDescription)) = "OK" Then
            '    MessageBox.Show("Sucessfully Sent...", "FOR SENDING SMS...", MessageBoxButtons.OK, MessageBoxIcon.Information)
            'Else
            '    MessageBox.Show("Failed to sent SMS...", "FOR SENDING SMS...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            'End If

            REMINDER_SMS = True

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT SEND OTP...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Finally
            response.Close()

            response = Nothing
            request = Nothing

        End Try

    End Function


    Private Sub Aged_PurchaseBills()
        Dim Cmd As New SqlClient.SqlCommand
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim RptCondt As String = ""
        Dim CompCondt As String = ""
        ' Dim Comp_IdNo As Integer
        Dim b() As String
        Dim i As Integer
        Dim S As String
        Dim oldvl As String
        Dim RepPeriods As String = ""
        Dim Nr As Integer = 0
        Dim ParentCode As String = "~14~11~"   '-Sundry Creditors

        Cmd.Connection = con

        Cmd.Parameters.Clear()
        Cmd.Parameters.AddWithValue("@uptodate", Current_Date)

        CompCondt = ""
        If Trim(UCase(Common_Procedures.User.Type)) <> "UNACCOUNT" Then
            CompCondt = "(Company_Type <> 'UNACCOUNT')"
        End If

        RptCondt = CompCondt

        ' RptCondt = " a.Company_IdNo = " & Str(Val(Comp_IdNo))

        If Trim(RepPeriods) = "" Then
            RepPeriods = "30,60,90,120"
        End If


        b = Split(RepPeriods, ",")

        Cmd.Connection = con

        Cmd.CommandText = "truncate table " & Trim(Common_Procedures.ReportTempSubTable) & ""
        Cmd.ExecuteNonQuery()

        oldvl = "0"

        'For i = 0 To UBound(b)

        '    S = Trim(Val(oldvl)) & " TO " & Trim(Val(b(i)))

        '    Cmd.CommandText = "Insert into " & Trim(Common_Procedures.ReportTempSubTable) & " (  Int1                               ,  currency1) " & _
        '                                " Select  datediff(dd, a.voucher_bill_date, @uptodate) ,      0    from voucher_bill_head a INNER JOIN company_head tz ON a.company_idno <> 0 and a.company_idno = tz.company_idno INNER JOIN Ledger_Head tP ON a.Ledger_IdNo <> 0 and a.Ledger_idno = tP.Ledger_IdNo Where " & Trim(RptCondt) & IIf(RptCondt <> "", " and ", "") & " a.voucher_bill_date <= @uptodate and datediff(dd, a.voucher_bill_date, @uptodate) between " & Str(Val(oldvl)) & " and " & Str(Val(b(i))) & " and  a.credit_amount > a.debit_amount group by a.voucher_bill_date"
        '    Cmd.ExecuteNonQuery()

        '    Cmd.CommandText = "Insert into " & Trim(Common_Procedures.ReportTempSubTable) & " ( Int1                                  ,  currency1) " & _
        '                                " Select    datediff(dd, a.voucher_bill_date, @uptodate) ,  sum(a.credit_amount - a.debit_amount) from voucher_bill_head a INNER JOIN company_head tz ON a.company_idno <> 0 and a.company_idno = tz.company_idno INNER JOIN Ledger_Head tP ON a.Ledger_IdNo <> 0 and a.Ledger_idno = tP.Ledger_IdNo Where " & Trim(RptCondt) & IIf(RptCondt <> "", " and ", "") & " a.voucher_bill_date <= @uptodate and datediff(dd, a.voucher_bill_date, @uptodate) between " & Str(Val(oldvl)) & " and " & Str(Val(b(i))) & " and a.credit_amount > a.debit_amount  group by a.voucher_bill_date"
        '    Nr = Cmd.ExecuteNonQuery()

        '    oldvl = Val(b(i)) + 1

        'Next i


        'If Val(oldvl) <> 0 Then

        '    S = "ABV " & Trim(Val(oldvl) - 1)

        '    Cmd.CommandText = "Insert into " & Trim(Common_Procedures.ReportTempSubTable) & " ( Int1                               , currency1) " & _
        '                                " Select datediff(dd, a.voucher_bill_date, @uptodate) ,      0    from voucher_bill_head a INNER JOIN company_head tz ON a.company_idno <> 0 and a.company_idno = tz.company_idno INNER JOIN Ledger_Head tP ON a.Ledger_IdNo <> 0 and a.Ledger_idno = tP.Ledger_IdNo Where " & Trim(RptCondt) & IIf(RptCondt <> "", " and ", "") & " a.voucher_bill_date <= @uptodate and datediff(dd, a.voucher_bill_date, @uptodate) >= " & Str(Val(oldvl)) & " and a.credit_amount > a.debit_amount  group by a.voucher_bill_date"
        '    Cmd.ExecuteNonQuery()

        '    Cmd.CommandText = "Insert into " & Trim(Common_Procedures.ReportTempSubTable) & " ( Int1                                     , currency1) " & _
        '                                " Select      datediff(dd, a.voucher_bill_date, @uptodate)  ,  sum(a.credit_amount - a.debit_amount) from voucher_bill_head a INNER JOIN company_head tz ON a.company_idno <> 0 and a.company_idno = tz.company_idno INNER JOIN Ledger_Head tP ON a.Ledger_IdNo <> 0 and a.Ledger_idno = tP.Ledger_IdNo Where " & Trim(RptCondt) & IIf(RptCondt <> "", " and ", "") & " a.voucher_bill_date <= @uptodate and datediff(dd, a.voucher_bill_date, @uptodate)  >= " & Str(Val(oldvl)) & " and a.credit_amount > a.debit_amount  group by a.voucher_bill_date"
        '    Cmd.ExecuteNonQuery()

        'End If


        For i = 0 To UBound(b)

            S = Trim(Val(oldvl)) & " TO " & Trim(Val(b(i)))

            Cmd.CommandText = "Insert into " & Trim(Common_Procedures.ReportTempSubTable) & " (  Int1                          ,  currency1) " & _
                                        " Select  datediff(dd, a.voucher_date, @uptodate) ,      0    from voucher_details a INNER JOIN company_head tz ON a.company_idno <> 0 and a.company_idno = tz.company_idno INNER JOIN Ledger_Head tP ON a.Ledger_IdNo <> 0 and a.Ledger_idno = tP.Ledger_IdNo and tP.Bill_Type = 'BILL TO BILL'  and tP.Parent_Code = '" & Trim(ParentCode) & "' Where " & Trim(RptCondt) & IIf(RptCondt <> "", " and ", "") & " a.voucher_date <= @uptodate and datediff(dd, a.voucher_date, @uptodate) between " & Str(Val(oldvl)) & " and " & Str(Val(b(i))) & " and  a.voucher_amount > 0   group by a.voucher_date"
            Cmd.ExecuteNonQuery()

            Cmd.CommandText = "Insert into " & Trim(Common_Procedures.ReportTempSubTable) & " ( Int1                                  ,  currency1) " & _
                                        " Select    datediff(dd, a.voucher_date, @uptodate) , sum(a.voucher_Amount) from voucher_details a INNER JOIN company_head tz ON a.company_idno <> 0 and a.company_idno = tz.company_idno INNER JOIN Ledger_Head tP ON a.Ledger_IdNo <> 0 and a.Ledger_idno = tP.Ledger_IdNo  and tP.Bill_Type = 'BILL TO BILL' and  tP.Parent_Code = '" & Trim(ParentCode) & "' Where " & Trim(RptCondt) & IIf(RptCondt <> "", " and ", "") & " a.voucher_date <= @uptodate and datediff(dd, a.voucher_date, @uptodate) between " & Str(Val(oldvl)) & " and " & Str(Val(b(i))) & " and  a.voucher_amount > 0  group by a.voucher_date"
            Cmd.ExecuteNonQuery()

            oldvl = Val(b(i)) + 1

        Next i



        If Val(oldvl) <> 0 Then

            S = "ABV " & Trim(Val(oldvl) - 1)

            Cmd.CommandText = "Insert into " & Trim(Common_Procedures.ReportTempSubTable) & " ( Int1                          , currency1) " & _
                                        " Select datediff(dd, a.voucher_date, @uptodate) ,      0    from voucher_details a INNER JOIN company_head tz ON a.company_idno <> 0 and a.company_idno = tz.company_idno INNER JOIN Ledger_Head tP ON a.Ledger_IdNo <> 0 and a.Ledger_idno = tP.Ledger_IdNo and tP.Bill_Type = 'BILL TO BILL'   and tP.Parent_Code = '" & Trim(ParentCode) & "' Where " & Trim(RptCondt) & IIf(RptCondt <> "", " and ", "") & " a.voucher_date <= @uptodate and datediff(dd, a.voucher_date, @uptodate) >= " & Str(Val(oldvl)) & " and  a.voucher_amount > 0  group by a.voucher_date"
            Cmd.ExecuteNonQuery()

            Cmd.CommandText = "Insert into " & Trim(Common_Procedures.ReportTempSubTable) & " ( Int1                                , currency1) " & _
                                        " Select      datediff(dd, a.voucher_date, @uptodate)  ,  sum(a.voucher_Amount) from voucher_details a INNER JOIN company_head tz ON a.company_idno <> 0 and a.company_idno = tz.company_idno INNER JOIN Ledger_Head tP ON a.Ledger_IdNo <> 0 and a.Ledger_idno = tP.Ledger_IdNo and tP.Bill_Type = 'BILL TO BILL'  and tP.Parent_Code = '" & Trim(ParentCode) & "'  Where " & Trim(RptCondt) & IIf(RptCondt <> "", " and ", "") & " a.voucher_date <= @uptodate and datediff(dd, a.voucher_date, @uptodate)  >= " & Str(Val(oldvl)) & " and  a.voucher_amount > 0  group by a.voucher_date"
            Cmd.ExecuteNonQuery()

        End If


        Cmd.CommandText = "truncate table " & Trim(Common_Procedures.ReportTempTable) & ""
        Cmd.ExecuteNonQuery()

        Cmd.CommandText = "insert into " & Trim(Common_Procedures.ReportTempTable) & " ( Int1 , currency1 ) Select Int1 , sum(currency1) from " & Trim(Common_Procedures.ReportTempSubTable) & " group by Int1 having sum(currency1) <> 0 "
        Cmd.ExecuteNonQuery()


        Da = New SqlClient.SqlDataAdapter("select Int1, currency1 from " & Trim(Common_Procedures.ReportTempTable) & "", con)
        Dt = New DataTable
        Da.Fill(Dt)

        If Dt.Rows.Count > 0 Then
            For i = 0 To Dt.Rows.Count - 1

                If Val(Dt.Rows(i).Item("Int1").ToString) <= 30 Then

                    lbl_Inv_1to30.Text = Format(Val(lbl_Inv_1to30.Text) + Val(Dt.Rows(i).Item("currency1").ToString), "##########0.00")

                ElseIf Val(Dt.Rows(i).Item("Int1").ToString) <= 60 Then

                    lbl_Inv_31to60.Text = Format(Val(lbl_Inv_31to60.Text) + Val(Dt.Rows(i).Item("currency1").ToString), "##########0.00")

                ElseIf Val(Dt.Rows(i).Item("Int1").ToString) <= 90 Then

                    lbl_Inv_61to90.Text = Format(Val(lbl_Inv_61to90.Text) + Val(Dt.Rows(i).Item("currency1").ToString), "##########0.00")

                ElseIf Val(Dt.Rows(i).Item("Int1").ToString) <= 120 Then

                    lbl_Inv_91to120.Text = Format(Val(lbl_Inv_91to120.Text) + Val(Dt.Rows(i).Item("currency1").ToString), "##########0.00")

                ElseIf Val(Dt.Rows(i).Item("Int1").ToString) > 120 Then

                    lbl_Inv_Above120.Text = Format(Val(lbl_Inv_Above120.Text) + Val(Dt.Rows(i).Item("currency1").ToString), "##########0.00")
                End If

            Next
        End If

        Dt.Clear()
        Da.Dispose()

        If Val(lbl_Inv_1to30.Text) <> 0 Then
            lbl_Inv_1to30.Text = "₹ " & Trim(lbl_Inv_1to30.Text)
        End If

        If Val(lbl_Inv_31to60.Text) <> 0 Then
            lbl_Inv_31to60.Text = "₹ " & Trim(lbl_Inv_31to60.Text)
        End If

        If Val(lbl_Inv_61to90.Text) <> 0 Then
            lbl_Inv_61to90.Text = "₹ " & Trim(lbl_Inv_61to90.Text)
        End If

        If Val(lbl_Inv_91to120.Text) <> 0 Then
            lbl_Inv_91to120.Text = "₹ " & Trim(lbl_Inv_91to120.Text)
        End If

        If Val(lbl_Inv_Above120.Text) <> 0 Then
            lbl_Inv_Above120.Text = "₹ " & Trim(lbl_Inv_Above120.Text)
        End If

    End Sub
    Private Sub Aged_SalesBills()
        Dim Cmd As New SqlClient.SqlCommand
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim RptCondt As String = ""
        Dim CompCondt As String = ""
        '  Dim Comp_IdNo As Integer
        Dim b() As String
        Dim i As Integer
        Dim S As String
        Dim oldvl As String
        Dim RepPeriods As String = ""
        Dim ParentCode As String = "~10~4~"   '-Sundry Debtors

        Cmd.Connection = con

        Cmd.Parameters.Clear()
        Cmd.Parameters.AddWithValue("@uptodate", Current_Date)

        CompCondt = ""
        If Trim(UCase(Common_Procedures.User.Type)) <> "UNACCOUNT" Then
            CompCondt = "(Company_Type <> 'UNACCOUNT')"
        End If

        RptCondt = CompCondt

        ' RptCondt = " a.Company_IdNo = " & Str(Val(Comp_IdNo))

        If Trim(RepPeriods) = "" Then
            RepPeriods = "30,60,90,120"
        End If


        b = Split(RepPeriods, ",")

        Cmd.Connection = con

        Cmd.CommandText = "truncate table " & Trim(Common_Procedures.ReportTempSubTable) & ""
        Cmd.ExecuteNonQuery()

        oldvl = "0"

        'For i = 0 To UBound(b)

        '    S = Trim(Val(oldvl)) & " TO " & Trim(Val(b(i)))

        '    Cmd.CommandText = "Insert into " & Trim(Common_Procedures.ReportTempSubTable) & " (  Int1                               ,  currency1) " & _
        '                                " Select  datediff(dd, a.voucher_bill_date, @uptodate) ,      0    from voucher_bill_head a INNER JOIN company_head tz ON a.company_idno <> 0 and a.company_idno = tz.company_idno INNER JOIN Ledger_Head tP ON a.Ledger_IdNo <> 0 and a.Ledger_idno = tP.Ledger_IdNo Where " & Trim(RptCondt) & IIf(RptCondt <> "", " and ", "") & " a.voucher_bill_date <= @uptodate and datediff(dd, a.voucher_bill_date, @uptodate) between " & Str(Val(oldvl)) & " and " & Str(Val(b(i))) & " and  a.debit_amount > a.credit_amount  group by a.voucher_bill_date"
        '    Cmd.ExecuteNonQuery()

        '    Cmd.CommandText = "Insert into " & Trim(Common_Procedures.ReportTempSubTable) & " ( Int1                                  ,  currency1) " & _
        '                                " Select    datediff(dd, a.voucher_bill_date, @uptodate) ,  sum(a.debit_amount - a.credit_amount) from voucher_bill_head a INNER JOIN company_head tz ON a.company_idno <> 0 and a.company_idno = tz.company_idno INNER JOIN Ledger_Head tP ON a.Ledger_IdNo <> 0 and a.Ledger_idno = tP.Ledger_IdNo Where " & Trim(RptCondt) & IIf(RptCondt <> "", " and ", "") & " a.voucher_bill_date <= @uptodate and datediff(dd, a.voucher_bill_date, @uptodate) between " & Str(Val(oldvl)) & " and " & Str(Val(b(i))) & " and  a.debit_amount > a.credit_amount group by a.voucher_bill_date"
        '    Cmd.ExecuteNonQuery()

        '    oldvl = Val(b(i)) + 1

        'Next i

        'If Val(oldvl) <> 0 Then

        '    S = "ABV " & Trim(Val(oldvl) - 1)

        '    Cmd.CommandText = "Insert into " & Trim(Common_Procedures.ReportTempSubTable) & " ( Int1                               , currency1) " & _
        '                                " Select datediff(dd, a.voucher_bill_date, @uptodate) ,      0    from voucher_bill_head a INNER JOIN company_head tz ON a.company_idno <> 0 and a.company_idno = tz.company_idno INNER JOIN Ledger_Head tP ON a.Ledger_IdNo <> 0 and a.Ledger_idno = tP.Ledger_IdNo Where " & Trim(RptCondt) & IIf(RptCondt <> "", " and ", "") & " a.voucher_bill_date <= @uptodate and datediff(dd, a.voucher_bill_date, @uptodate) >= " & Str(Val(oldvl)) & " and  a.debit_amount > a.credit_amount group by a.voucher_bill_date"
        '    Cmd.ExecuteNonQuery()

        '    Cmd.CommandText = "Insert into " & Trim(Common_Procedures.ReportTempSubTable) & " ( Int1                                     , currency1) " & _
        '                                " Select      datediff(dd, a.voucher_bill_date, @uptodate)  , sum(a.debit_amount - a.credit_amount) from voucher_bill_head a INNER JOIN company_head tz ON a.company_idno <> 0 and a.company_idno = tz.company_idno INNER JOIN Ledger_Head tP ON a.Ledger_IdNo <> 0 and a.Ledger_idno = tP.Ledger_IdNo Where " & Trim(RptCondt) & IIf(RptCondt <> "", " and ", "") & " a.voucher_bill_date <= @uptodate and datediff(dd, a.voucher_bill_date, @uptodate)  >= " & Str(Val(oldvl)) & " and  a.debit_amount > a.credit_amount group by a.voucher_bill_date"
        '    Cmd.ExecuteNonQuery()

        'End If



        For i = 0 To UBound(b)

            S = Trim(Val(oldvl)) & " TO " & Trim(Val(b(i)))

            Cmd.CommandText = "Insert into " & Trim(Common_Procedures.ReportTempSubTable) & " (  Int1                          ,  currency1) " & _
                                        " Select  datediff(dd, a.voucher_date, @uptodate) ,      0    from voucher_details a INNER JOIN company_head tz ON a.company_idno <> 0 and a.company_idno = tz.company_idno INNER JOIN Ledger_Head tP ON a.Ledger_IdNo <> 0 and a.Ledger_idno = tP.Ledger_IdNo and  tP.Bill_Type = 'BILL TO BILL'  and tP.Parent_Code = '" & Trim(ParentCode) & "' Where " & Trim(RptCondt) & IIf(RptCondt <> "", " and ", "") & " a.voucher_date <= @uptodate and datediff(dd, a.voucher_date, @uptodate) between " & Str(Val(oldvl)) & " and " & Str(Val(b(i))) & " and  a.voucher_amount < 0   group by A.ledger_idno , a.voucher_date"
            Cmd.ExecuteNonQuery()

            Cmd.CommandText = "Insert into " & Trim(Common_Procedures.ReportTempSubTable) & " ( Int1                                  ,  currency1) " & _
                                        " Select    datediff(dd, a.voucher_date, @uptodate) , -1* sum(a.voucher_Amount) from voucher_details a INNER JOIN company_head tz ON a.company_idno <> 0 and a.company_idno = tz.company_idno INNER JOIN Ledger_Head tP ON a.Ledger_IdNo <> 0 and a.Ledger_idno = tP.Ledger_IdNo and  tP.Bill_Type = 'BILL TO BILL'  and  tP.Parent_Code = '" & Trim(ParentCode) & "' Where " & Trim(RptCondt) & IIf(RptCondt <> "", " and ", "") & " a.voucher_date <= @uptodate and datediff(dd, a.voucher_date, @uptodate) between " & Str(Val(oldvl)) & " and " & Str(Val(b(i))) & " and  a.voucher_amount < 0  group by A.ledger_idno , a.voucher_date"
            Cmd.ExecuteNonQuery()

            oldvl = Val(b(i)) + 1

        Next i



        If Val(oldvl) <> 0 Then

            S = "ABV " & Trim(Val(oldvl) - 1)

            Cmd.CommandText = "Insert into " & Trim(Common_Procedures.ReportTempSubTable) & " ( Int1                          , currency1) " & _
                                        " Select datediff(dd, a.voucher_date, @uptodate) ,      0    from voucher_details a INNER JOIN company_head tz ON a.company_idno <> 0 and a.company_idno = tz.company_idno INNER JOIN Ledger_Head tP ON a.Ledger_IdNo <> 0 and a.Ledger_idno = tP.Ledger_IdNo and  tP.Bill_Type = 'BILL TO BILL' and  tP.Parent_Code = '" & Trim(ParentCode) & "'  Where " & Trim(RptCondt) & IIf(RptCondt <> "", " and ", "") & " a.voucher_date <= @uptodate and datediff(dd, a.voucher_date, @uptodate) >= " & Str(Val(oldvl)) & " and  a.voucher_amount < 0  group by A.ledger_idno , a.voucher_date"
            Cmd.ExecuteNonQuery()

            Cmd.CommandText = "Insert into " & Trim(Common_Procedures.ReportTempSubTable) & " ( Int1                                , currency1) " & _
                                        " Select      datediff(dd, a.voucher_date, @uptodate)  , -1 * sum(a.voucher_Amount) from voucher_details a INNER JOIN company_head tz ON a.company_idno <> 0 and a.company_idno = tz.company_idno INNER JOIN Ledger_Head tP ON a.Ledger_IdNo <> 0 and a.Ledger_idno = tP.Ledger_IdNo and  tP.Bill_Type = 'BILL TO BILL'  and  tP.Parent_Code = '" & Trim(ParentCode) & "' Where " & Trim(RptCondt) & IIf(RptCondt <> "", " and ", "") & " a.voucher_date <= @uptodate and datediff(dd, a.voucher_date, @uptodate)  >= " & Str(Val(oldvl)) & " and  a.voucher_amount < 0  group by A.ledger_idno , a.voucher_date"
            Cmd.ExecuteNonQuery()

        End If

        Cmd.CommandText = "truncate table " & Trim(Common_Procedures.ReportTempTable) & ""
        Cmd.ExecuteNonQuery()

        Cmd.CommandText = "insert into " & Trim(Common_Procedures.ReportTempTable) & " ( Int1 , currency1 ) Select Int1 , sum(currency1) from " & Trim(Common_Procedures.ReportTempSubTable) & " group by Int1 having sum(currency1) <> 0 "
        Cmd.ExecuteNonQuery()


        Da = New SqlClient.SqlDataAdapter("select Int1, currency1 from " & Trim(Common_Procedures.ReportTempTable) & "", con)
        Dt = New DataTable
        Da.Fill(Dt)

        If Dt.Rows.Count > 0 Then
            For i = 0 To Dt.Rows.Count - 1

                If Val(Dt.Rows(i).Item("Int1").ToString) <= 30 Then

                    lbl_Bill_1to30.Text = Format(Val(lbl_Bill_1to30.Text) + Val(Dt.Rows(i).Item("currency1").ToString), "##########0.00")

                ElseIf Val(Dt.Rows(i).Item("Int1").ToString) <= 60 Then

                    lbl_Bill_31to60.Text = Format(Val(lbl_Bill_31to60.Text) + Val(Dt.Rows(i).Item("currency1").ToString), "##########0.00")

                ElseIf Val(Dt.Rows(i).Item("Int1").ToString) <= 90 Then

                    lbl_Bill_61to90.Text = Format(Val(lbl_Bill_61to90.Text) + Val(Dt.Rows(i).Item("currency1").ToString), "##########0.00")

                ElseIf Val(Dt.Rows(i).Item("Int1").ToString) <= 120 Then

                    lbl_Bill_91to120.Text = Format(Val(lbl_Bill_91to120.Text) + Val(Dt.Rows(i).Item("currency1").ToString), "##########0.00")

                ElseIf Val(Dt.Rows(i).Item("Int1").ToString) > 120 Then

                    lbl_Bill_Above120.Text = Format(Val(lbl_Bill_Above120.Text) + Val(Dt.Rows(i).Item("currency1").ToString), "##########0.00")
                End If

            Next
        End If

        Dt.Clear()
        Da.Dispose()

        If Val(lbl_Bill_1to30.Text) <> 0 Then
            lbl_Bill_1to30.Text = "₹ " & Trim(lbl_Bill_1to30.Text)
        End If

        If Val(lbl_Bill_31to60.Text) <> 0 Then
            lbl_Bill_31to60.Text = "₹ " & Trim(lbl_Bill_31to60.Text)
        End If

        If Val(lbl_Bill_61to90.Text) <> 0 Then
            lbl_Bill_61to90.Text = "₹ " & Trim(lbl_Bill_61to90.Text)
        End If

        If Val(lbl_Bill_91to120.Text) <> 0 Then
            lbl_Bill_91to120.Text = "₹ " & Trim(lbl_Bill_91to120.Text)
        End If

        If Val(lbl_Bill_Above120.Text) <> 0 Then
            lbl_Bill_Above120.Text = "₹ " & Trim(lbl_Bill_Above120.Text)
        End If

    End Sub



    Private Sub btn_Close_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Close.Click
        '  MDIParent1.mnu_Tools_Dashboard.Text = "Show DashBoard"
        Me.Close()
    End Sub
    Private Sub Net_Income()
        Dim cmd As New SqlClient.SqlCommand
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim Dt1 As New DataTable
        Dim n As Integer = 0
        Dim Curyear As String = ""
        Dim Prevyear As String = ""
        ' Dim ParentCode As String = "~10~4~"   '-Sundry Debtors


        Curyear = Current_Date.Year
        Prevyear = Current_Date.Year - 1

        cmd.Connection = con

        cmd.Parameters.Clear()
        cmd.Parameters.AddWithValue("@uptodate", Current_Date)
        cmd.Parameters.AddWithValue("@companyfromdate", Common_Procedures.Company_FromDate)




        cmd.CommandText = "truncate table " & Trim(Common_Procedures.ReportTempSubTable) & ""
        cmd.ExecuteNonQuery()

        cmd.CommandText = "insert into " & Trim(Common_Procedures.ReportTempSubTable) & " ( int1,Name1                     ,  currency1       ) " & _
                                   "Select tZ.company_idno ,year(a.Voucher_Date) ,     sum(a.Voucher_Amount)  from voucher_Details a INNER JOIN company_head tz  ON a.company_idno <> 0 and a.company_idno = tZ.company_idno  LEFT OUTER JOIN Ledger_Head Lh ON a.Ledger_Idno = lh.ledger_Idno  where (LH.Parent_Code = '~19~18~' ) and a.Voucher_Amount <> 0  and  a.voucher_date <= @uptodate  group by tZ.company_idno, a.Voucher_Date"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "insert into " & Trim(Common_Procedures.ReportTempSubTable) & " ( int1 , Name1                    ,currency2 ) " & _
                                 "Select   tZ.company_idno , year(a.Voucher_Date) , -1 *  sum(a.Voucher_Amount)  from voucher_Details a INNER JOIN company_head tZ ON a.company_idno <> 0 and a.company_idno = tZ.company_idno LEFT OUTER JOIN Ledger_Head Lh ON a.Ledger_Idno = lh.ledger_Idno  Where (LH.Parent_Code = '~15~18~' or LH.Parent_Code = '~16~18~') and a.Voucher_Amount <> 0  and  a.voucher_date <= @uptodate  group by tZ.company_idno, a.Voucher_Date"
        cmd.ExecuteNonQuery()


        cmd.CommandText = "truncate table " & Trim(Common_Procedures.ReportTempTable) & ""
        cmd.ExecuteNonQuery()


        cmd.CommandText = "Insert into " & Trim(Common_Procedures.ReportTempTable) & " ( int1    ,Name1  , currency1      ,Currency2) " & _
                                             " Select int1  ,Name1  , sum(currency1) , sum(currency2) from " & Trim(Common_Procedures.ReportTempSubTable) & "  group by  int1,name1"
        cmd.ExecuteNonQuery()


        Da = New SqlClient.SqlDataAdapter("select Name1, sum(currency1) as income, sum(currency2) as expense from " & Trim(Common_Procedures.ReportTempTable) & " group by Name1  Order by name1", con)
        Dt = New DataTable
        Da.Fill(Dt)

        'If Dt.Rows.Count > 0 Then



        '    For i = 0 To Dt.Rows.Count - 1

        '        If Trim(Dt.Rows(i).Item("Name1").ToString) = Curyear Then

        '            lbl_CurYear.Text = Trim(Dt.Rows(i).Item("Name1").ToString)
        '            lbl_CurIncome.Text = Format(IIf(Val(Dt.Rows(i).Item("income").ToString) < 0, -1 * Val(Dt.Rows(i).Item("income").ToString), Val(Dt.Rows(i).Item("income").ToString)), "#########0.00")
        '            lbl_CurExpenses.Text = Format(Val(Dt.Rows(i).Item("expense").ToString), "#########0.00")
        '            lbl_CurNetIncome.Text = Format(Val(lbl_CurIncome.Text) - Val(lbl_CurExpenses.Text), "############0.00")

        '        ElseIf Trim(Dt.Rows(i).Item("Name1").ToString) = Prevyear Then


        '            lbl_PrevYear.Text = Trim(Dt.Rows(i).Item("Name1").ToString)
        '            lbl_PrevIncome.Text = Format(Val(Dt.Rows(i).Item("income").ToString), "##########0.00")
        '            lbl_PrevExpenses.Text = Format(Val(Dt.Rows(i).Item("expense").ToString), "##########0.00")
        '            lbl_PrevNetIncome.Text = Format(Val(lbl_PrevIncome.Text) - Val(lbl_PrevExpenses.Text), "##########0.00")
        '        End If
        '    Next
        'End If

        Dt.Clear()
        Da.Dispose()


    End Sub
   


    Public Sub delete_record() Implements Interface_MDIActions.delete_record

    End Sub

    Public Sub filter_record() Implements Interface_MDIActions.filter_record

    End Sub

    Public Sub insert_record() Implements Interface_MDIActions.insert_record

    End Sub

    Public Sub movefirst_record() Implements Interface_MDIActions.movefirst_record

    End Sub

    Public Sub movelast_record() Implements Interface_MDIActions.movelast_record

    End Sub

    Public Sub movenext_record() Implements Interface_MDIActions.movenext_record

    End Sub

    Public Sub moveprevious_record() Implements Interface_MDIActions.moveprevious_record

    End Sub

    Public Sub new_record() Implements Interface_MDIActions.new_record

    End Sub

    Public Sub open_record() Implements Interface_MDIActions.open_record

    End Sub

    Public Sub print_record() Implements Interface_MDIActions.print_record

    End Sub

    Public Sub save_record() Implements Interface_MDIActions.save_record

    End Sub
  
End Class