Imports System.Windows.Forms.DataVisualization.Charting

Public Class DashBoard_New
    Implements Interface_MDIActions

    Private con As New SqlClient.SqlConnection(Common_Procedures.Connection_String)
    Private Current_Date As Date = Now
    Private CurX As Integer = 0

    Private Sub DashBoard_New_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.Left = 0
        Me.Top = 0
        Me.Width = Screen.PrimaryScreen.WorkingArea.Width - 12 '10
        Me.Height = Screen.PrimaryScreen.WorkingArea.Height - 130 ' (MDIParent1.MenuStrip.Height - MDIParent1.Panel1.Height - 30) ' 110 ' 90 ' 100

        lbl_UserName.Text = Common_Procedures.User.Name
        lbl_CompanyName.Text = Common_Procedures.CompGroupName

        pnl_Entry_Menu_Details.Visible = False
        pnl_DashBoard_Details.Dock = DockStyle.Fill
        pnl_DashBoard_Details.Visible = True

        'dgv_OverDueBills.DefaultCellStyle.ForeColor = Color.Black


        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1365" Then
            If InStr(1, Trim(LCase(Common_Procedures.CompGroupName)), Trim(LCase("TRADING"))) > 0 Then

                lbl_CompanyName.BackColor = Color.LightCyan
                lbl_CompanyName.ForeColor = Color.Maroon

                pnl_DashBoard_MainMenu_Details.BackColor = Color.LightGreen

                pnl_Report_Menu_Details.BackgroundImage = Textile.My.Resources.Resources.BackGround1
                pnl_Entry_Menu_Details.BackgroundImage = Textile.My.Resources.Resources.BackGround1
                pnl_DashBoard_Details.BackgroundImage = Textile.My.Resources.Resources.BackGround1

                Lbl_Dashboard.ForeColor = Color.Black
                lbl_Entry.ForeColor = Color.Black
                lbl_Reports.ForeColor = Color.Black
                lbl_About.ForeColor = Color.Black
                lbl_Logout.ForeColor = Color.Black

                lbl_Close_DashBoard.BackColor = Color.Maroon
                lbl_Close_DashBoard.ForeColor = Color.WhiteSmoke

            End If

        End If

        Display_DashBoard_Datas()

    End Sub

    Private Sub Display_DashBoard_Datas()



        con.Open()

        Sales_Amount_Details()
        Purchase_Amount_Details()
        Receivable_Payable_Amount_Details()
        NOStock_Minimum_Stock_Items()

        OverDue_Purchase_Sales()

        BarChart_IncomeAndExpense()
        PieChart_Expense()


        'OverDue_Purchase()
        'OverDue_Sales()
        'Aged_PurchaseBills()
        'Aged_SalesBills()
        'Net_Income()
        'Active_Orders()

        con.Close()

        con.Dispose()

        pnl_Entry_Menu_Details.Visible = False
        pnl_Report_Menu_Details.Visible = False

        pnl_DashBoard_Details.Dock = DockStyle.Fill
        pnl_DashBoard_Details.Visible = True

    End Sub


    Private Sub RectangleShape1_Click(sender As System.Object, e As System.EventArgs) Handles RectangleShape1.Click, Lbl_Dashboard.Click, PictureBox2.Click
        pnl_Entry_Menu_Details.Visible = False
        pnl_Report_Menu_Details.Visible = False

        pnl_DashBoard_Details.Dock = DockStyle.Fill
        pnl_DashBoard_Details.Visible = True
    End Sub

    Private Sub RectangleShape2_Click(sender As System.Object, e As System.EventArgs) Handles RectangleShape2.Click, lbl_Entry.Click, PictureBox3.Click, RectangleShape5.Click
        pnl_DashBoard_Details.Visible = False
        pnl_Report_Menu_Details.Visible = False

        pnl_Entry_Menu_Details.Dock = DockStyle.Fill
        pnl_Entry_Menu_Details.Visible = True
    End Sub

    Private Sub RectangleShape3_Click(sender As System.Object, e As System.EventArgs) Handles RectangleShape3.Click, lbl_Reports.Click, PictureBox4.Click
        pnl_DashBoard_Details.Visible = False
        pnl_Entry_Menu_Details.Visible = False

        pnl_Report_Menu_Details.Dock = DockStyle.Fill
        pnl_Report_Menu_Details.Visible = True
    End Sub

    Private Sub RectangleShape4_Click(sender As System.Object, e As System.EventArgs) Handles RectangleShape4.Click, lbl_Logout.Click, PictureBox6.Click
        Me.Close()
        MDIParent1.Close()
        Application.Exit()
    End Sub

    Public Sub New()
        ' This call is required by the designer.
        InitializeComponent()
        ' Add any initialization after the InitializeComponent() call.
    End Sub

    Private Sub lbl_Close_DashBoard_Click(sender As System.Object, e As System.EventArgs) Handles lbl_Close_DashBoard.Click
        Me.Close()
    End Sub

    Public Sub delete_record() Implements Interface_MDIActions.delete_record
        '------
    End Sub

    Public Sub filter_record() Implements Interface_MDIActions.filter_record
        '------
    End Sub

    Public Sub insert_record() Implements Interface_MDIActions.insert_record
        '------
    End Sub

    Public Sub movefirst_record() Implements Interface_MDIActions.movefirst_record
        '------
    End Sub

    Public Sub movelast_record() Implements Interface_MDIActions.movelast_record
        '------
    End Sub

    Public Sub movenext_record() Implements Interface_MDIActions.movenext_record
        '------
    End Sub

    Public Sub moveprevious_record() Implements Interface_MDIActions.moveprevious_record
        '------
    End Sub

    Public Sub new_record() Implements Interface_MDIActions.new_record
        '------
    End Sub

    Public Sub open_record() Implements Interface_MDIActions.open_record
        '------
    End Sub

    Public Sub print_record() Implements Interface_MDIActions.print_record
        '------
    End Sub

    Public Sub save_record() Implements Interface_MDIActions.save_record
        '------
    End Sub


    Private Sub Sales_Amount_Details()
        Dim cmd As New SqlClient.SqlCommand
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim vMon_ID As Integer = 0
        Dim CompIDCondt As String = ""

        cmd.Connection = con

        cmd.Parameters.Clear()
        cmd.Parameters.AddWithValue("@entrydate", Current_Date.Date)

        vMon_ID = Month(Current_Date)

        CompIDCondt = ""
        If Trim(UCase(Common_Procedures.User.Type)) <> "UNACCOUNT" Then
            CompIDCondt = "(tZ.Company_Type <> 'UNACCOUNT')"
        End If

        lbl_SalesAmount_ThisMonth.Text = ""
        Da = New SqlClient.SqlDataAdapter("Select sum(a.Net_Amount) as Amount from Sales_Head a, Company_Head tZ Where " & CompIDCondt & IIf(Trim(CompIDCondt) <> "", " and ", "") & " month(a.Sales_Date) = " & Str(Val(vMon_ID)) & " and a.Sales_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "' and tZ.company_IdNo <> 0 and a.company_idno = tZ.company_IdNo", con)
        Dt = New DataTable
        Da.Fill(Dt)
        If Dt.Rows.Count > 0 Then
            If IsDBNull(Dt.Rows(0)(0).ToString) = False Then
                lbl_SalesAmount_ThisMonth.Text = Common_Procedures.Currency_Format(Val(Dt.Rows(0)(0).ToString))
            End If
        End If
        Dt.Clear()

        lbl_SalesAmount_Today.Text = ""
        cmd.CommandText = "Select sum(a.Net_Amount) as Amount from Sales_Head a, Company_Head tZ Where " & CompIDCondt & IIf(Trim(CompIDCondt) <> "", " and ", "") & " a.Sales_Date = @entrydate and a.Sales_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "' and tZ.company_IdNo <> 0 and a.company_idno = tZ.company_IdNo"
        Da = New SqlClient.SqlDataAdapter(cmd)
        Dt = New DataTable
        Da.Fill(Dt)
        If Dt.Rows.Count > 0 Then
            If IsDBNull(Dt.Rows(0)(0).ToString) = False Then
                lbl_SalesAmount_Today.Text = Common_Procedures.Currency_Format(Val(Dt.Rows(0)(0).ToString))
            End If
        End If
        Dt.Clear()

        Dt.Dispose()
        Da.Dispose()
        cmd.Dispose()


    End Sub


    Private Sub Purchase_Amount_Details()
        Dim cmd As New SqlClient.SqlCommand
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim vMon_ID As Integer = 0
        Dim CompIDCondt As String = ""

        cmd.Connection = con

        cmd.Parameters.Clear()
        cmd.Parameters.AddWithValue("@entrydate", Current_Date.Date)

        vMon_ID = Month(Current_Date)

        CompIDCondt = ""
        If Trim(UCase(Common_Procedures.User.Type)) <> "UNACCOUNT" Then
            CompIDCondt = "(tZ.Company_Type <> 'UNACCOUNT')"
        End If

        lbl_PurchaseAmount_ThisMonth.Text = ""
        Da = New SqlClient.SqlDataAdapter("Select sum(a.Net_Amount) as Amount from Purchase_Head a, Company_Head tZ Where " & CompIDCondt & IIf(Trim(CompIDCondt) <> "", " and ", "") & " month(a.Purchase_Date) = " & Str(Val(vMon_ID)) & " and a.Purchase_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "' and tZ.company_IdNo <> 0 and a.company_idno = tZ.company_IdNo", con)
        Dt = New DataTable
        Da.Fill(Dt)
        If Dt.Rows.Count > 0 Then
            If IsDBNull(Dt.Rows(0)(0).ToString) = False Then
                lbl_PurchaseAmount_ThisMonth.Text = Common_Procedures.Currency_Format(Val(Dt.Rows(0)(0).ToString))
            End If
        End If
        Dt.Clear()

        lbl_PurchaseAmount_Today.Text = ""
        cmd.CommandText = "Select sum(a.Net_Amount) as Amount from Purchase_Head a, Company_Head tZ Where " & CompIDCondt & IIf(Trim(CompIDCondt) <> "", " and ", "") & " a.Purchase_Date = @entrydate and a.Purchase_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "' and tZ.company_IdNo <> 0 and a.company_idno = tZ.company_IdNo"
        Da = New SqlClient.SqlDataAdapter(cmd)
        Dt = New DataTable
        Da.Fill(Dt)
        If Dt.Rows.Count > 0 Then
            If IsDBNull(Dt.Rows(0)(0).ToString) = False Then
                lbl_PurchaseAmount_Today.Text = Common_Procedures.Currency_Format(Val(Dt.Rows(0)(0).ToString))
            End If
        End If
        Dt.Clear()

        Dt.Dispose()
        Da.Dispose()
        cmd.Dispose()


    End Sub


    Private Sub Receivable_Payable_Amount_Details()
        Dim cmd As New SqlClient.SqlCommand
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim CompIDCondt As String = ""

        cmd.Connection = con

        cmd.Parameters.Clear()
        cmd.Parameters.AddWithValue("@entrydate", Current_Date.Date)

        CompIDCondt = ""
        If Trim(UCase(Common_Procedures.User.Type)) <> "UNACCOUNT" Then
            CompIDCondt = "(tZ.Company_Type <> 'UNACCOUNT')"
        End If


        lbl_Total_Amount_Receivable.Text = ""
        cmd.CommandText = "Select abs(sum(a.voucher_amount)) from voucher_details a, ledger_head b, AccountsGroup_Head tG, company_head tz where " & CompIDCondt & IIf(CompIDCondt <> "", " and ", "") & " a.voucher_date <= @entrydate and tG.parent_idno LIKE '%~10~4~' and a.ledger_idno = b.ledger_idno and b.AccountsGroup_IdNo = tG.AccountsGroup_IdNo and a.company_idno <> 0 and a.company_idno = tz.company_idno having sum(a.voucher_amount) <> 0"
        Da = New SqlClient.SqlDataAdapter(cmd)
        Dt = New DataTable
        Da.Fill(Dt)
        If Dt.Rows.Count > 0 Then
            If IsDBNull(Dt.Rows(0)(0).ToString) = False Then
                lbl_Total_Amount_Receivable.Text = Common_Procedures.Currency_Format(Val(Dt.Rows(0)(0).ToString))
            End If
        End If
        Dt.Clear()

        lbl_Total_Amount_Payable.Text = ""
        cmd.CommandText = "Select abs(sum(a.voucher_amount)) from voucher_details a, ledger_head b, AccountsGroup_Head tG, company_head tz where " & CompIDCondt & IIf(CompIDCondt <> "", " and ", "") & " a.voucher_date <= @entrydate and tG.parent_idno LIKE '%~14~11~' and a.ledger_idno = b.ledger_idno and b.AccountsGroup_IdNo = tG.AccountsGroup_IdNo and a.company_idno <> 0 and a.company_idno = tz.company_idno having sum(a.voucher_amount) <> 0"
        Da = New SqlClient.SqlDataAdapter(cmd)
        Dt = New DataTable
        Da.Fill(Dt)
        If Dt.Rows.Count > 0 Then
            If IsDBNull(Dt.Rows(0)(0).ToString) = False Then
                lbl_Total_Amount_Payable.Text = Common_Procedures.Currency_Format(Val(Dt.Rows(0)(0).ToString))
            End If
        End If
        Dt.Clear()

        Dt.Dispose()
        Da.Dispose()
        cmd.Dispose()

    End Sub


    Private Sub NOStock_Minimum_Stock_Items()
        Dim cmd As New SqlClient.SqlCommand
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim CompIDCondt As String = ""

        cmd.Connection = con

        cmd.Parameters.Clear()
        cmd.Parameters.AddWithValue("@entrydate", Current_Date.Date)

        CompIDCondt = ""
        If Trim(UCase(Common_Procedures.User.Type)) <> "UNACCOUNT" Then
            CompIDCondt = "(tZ.Company_Type <> 'UNACCOUNT')"
        End If




        cmd.CommandText = "Truncate table ReportTemp"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Insert into ReportTemp (name1       ,       weight1  ,    Weight2      )   " & _
                            " Select               b.item_name , sum(a.Quantity),  b.Minimum_Stock  from Item_Processing_Details a INNER JOIN Company_Head tZ ON a.Company_IdNo <> 0 and a.Company_IdNo = tZ.Company_IdNo INNER JOIN Item_Head b ON a.item_idno <> 0 and a.item_idno = b.item_idno Where " & Trim(CompIDCondt) & IIf(Trim(CompIDCondt) <> "", " and ", "") & " a.Reference_Date <= @entrydate group by b.item_name,  b.Minimum_Stock  "
        cmd.ExecuteNonQuery()


        lbl_NoStock_Items.Text = ""
        cmd.CommandText = "Select count(a.name1) from ReportTemp a where a.name1 <> '' and a.weight1 <= 0"
        Da = New SqlClient.SqlDataAdapter(cmd)
        Dt = New DataTable
        Da.Fill(Dt)
        If Dt.Rows.Count > 0 Then
            If IsDBNull(Dt.Rows(0)(0).ToString) = False Then
                lbl_NoStock_Items.Text = Format(Val(Dt.Rows(0)(0).ToString), "##,##,##,##,##0")
            End If
        End If
        Dt.Clear()

        lbl_MinimumStock_Items.Text = ""
        cmd.CommandText = "Select count(a.name1) from ReportTemp a where a.name1 <> '' and a.weight1 > 0 and a.weight1 <= a.weight2"
        Da = New SqlClient.SqlDataAdapter(cmd)
        Dt = New DataTable
        Da.Fill(Dt)
        If Dt.Rows.Count > 0 Then
            If IsDBNull(Dt.Rows(0)(0).ToString) = False Then
                lbl_MinimumStock_Items.Text = Format(Val(Dt.Rows(0)(0).ToString), "##,##,##,##,##0")
            End If
        End If
        Dt.Clear()

        lbl_Total_Amount_Payable.Text = ""
        cmd.CommandText = "Select abs(sum(a.voucher_amount)) from voucher_details a, ledger_head b, AccountsGroup_Head tG, company_head tz where " & CompIDCondt & IIf(CompIDCondt <> "", " and ", "") & " a.voucher_date <= @entrydate and tG.parent_idno LIKE '%~14~11~' and a.ledger_idno = b.ledger_idno and b.AccountsGroup_IdNo = tG.AccountsGroup_IdNo and a.company_idno <> 0 and a.company_idno = tz.company_idno having sum(a.voucher_amount) <> 0"
        Da = New SqlClient.SqlDataAdapter(cmd)
        Dt = New DataTable
        Da.Fill(Dt)
        If Dt.Rows.Count > 0 Then
            If IsDBNull(Dt.Rows(0)(0).ToString) = False Then
                lbl_Total_Amount_Payable.Text = Common_Procedures.Currency_Format(Val(Dt.Rows(0)(0).ToString))
            End If
        End If
        Dt.Clear()

        Dt.Dispose()
        Da.Dispose()
        cmd.Dispose()

    End Sub

    Private Sub OverDue_Purchase_Sales()
        Dim cmd As New SqlClient.SqlCommand
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim Dt1 As New DataTable
        Dim n As Integer = 0
        Dim CompIDCondt As String


        'lbl_OverDue_Bills.Top = CurX + 30

        'dgv_OverDueBills.Top = CurX + lbl_OverDue_Bills.Height + 30

        cmd.Connection = con

        cmd.Parameters.Clear()
        cmd.Parameters.AddWithValue("@entrydate", Current_Date.Date)

        CompIDCondt = ""
        If Trim(UCase(Common_Procedures.User.Type)) <> "UNACCOUNT" Then
            CompIDCondt = "(tZ.Company_Type <> 'UNACCOUNT')"
        End If

        cmd.CommandText = "truncate table ReportTemp"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Insert into ReportTemp ( int1, name2, currency1) Select b.ledger_idno, b.ledger_name, sum(a.voucher_amount) from voucher_details a, ledger_head b, AccountsGroup_Head tG, company_head tz where (tG.parent_idno LIKE '%~10~4~' or tG.parent_idno LIKE '%~14~11~') and a.ledger_idno = b.ledger_idno and b.AccountsGroup_IdNo = tG.AccountsGroup_IdNo and a.company_idno <> 0 and a.company_idno = tz.company_idno group by b.ledger_idno, b.ledger_name having sum(a.voucher_amount) <> 0"
        cmd.ExecuteNonQuery()

        Da = New SqlClient.SqlDataAdapter("select name2 as LedgerName, Int1 as LedgerIdno, sum(currency1) as Amount from ReportTemp group by name1 ,name2,int1 having sum(currency1) <> 0 Order by name2", con)
        Dt = New DataTable
        Da.Fill(Dt)

        If Dt.Rows.Count > 0 Then

            dgv_OverDueBills.Rows.Clear()
            'dgv_OverDueBills.Height = 0

            For i = 0 To Dt.Rows.Count - 1

                If Val(Dt.Rows(i).Item("Amount").ToString) <> 0 Then
                    With dgv_OverDueBills
                        n = .Rows.Add()

                        .Rows(n).Cells(0).Value = "*"
                        .Rows(n).Cells(1).Value = Trim(Dt.Rows(i).Item("LedgerName").ToString)
                        .Rows(n).Cells(2).Value = Common_Procedures.Currency_Format(Math.Abs(Val(Dt.Rows(i).Item("Amount").ToString)))
                        .Rows(n).Cells(3).Value = IIf(Val(Dt.Rows(i).Item("Amount").ToString) > 0, "Cr", "Dr")
                        .Rows(n).Cells(4).Value = Val(Dt.Rows(i).Item("LedgerIdno").ToString)

                        If .Height < 200 Then
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

        On Error Resume Next
        If Not IsNothing(dgv_OverDueBills.CurrentCell) Then dgv_OverDueBills.CurrentCell.Selected = False

    End Sub

    Private Sub BarChart_IncomeAndExpense()
        Dim cmd As New SqlClient.SqlCommand
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim Dt1 As New DataTable
        Dim n As Integer = 0
        Dim RptCondt As String = ""
        Dim CompCondt As String = ""
        Dim Nr As Integer = 0


        cmd.Parameters.Clear()
        cmd.Parameters.AddWithValue("@uptodate", Current_Date)
        cmd.Parameters.AddWithValue("@companyfromdate", Common_Procedures.Company_FromDate)

        CompCondt = ""
        If Trim(UCase(Common_Procedures.User.Type)) <> "UNACCOUNT" Then
            CompCondt = "(Company_Type <> 'UNACCOUNT')"
        End If

        RptCondt = CompCondt

        cmd.Connection = con

        cmd.CommandText = "truncate table reporttempsub"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "truncate table ReportTemp"
        cmd.ExecuteNonQuery()


        For i = 1 To 12
            cmd.CommandText = "Insert into ReportTemp ( Int1   ,  NAME1          ,  currency1 , Currency2) " & _
                                " Select     MH.Idno   ,  MH.Month_ShortName  ,    0       ,   0     from  Month_Head MH where MH.Month_IdNo =" & i & " "
            cmd.ExecuteNonQuery()
        Next


        cmd.CommandText = "Insert into reporttempsub ( Int1   ,  NAME1               ,  currency1) " & _
                                    " Select     MH.Idno,  MH.Month_ShortName  ,     abs(sum( a.Voucher_Amount))   from voucher_Details a INNER JOIN company_head tz ON a.company_idno <> 0 and a.company_idno = tz.company_idno INNER JOIN Ledger_Head tP ON a.Ledger_IdNo <> 0 and a.Ledger_idno = tP.Ledger_IdNo  INNER JOIN AccountsGroup_Head tG ON tP.AccountsGroup_IdNo = tG.AccountsGroup_IdNo LEFT OUTER JOIN Month_Head MH ON MH.Month_IdNo = MONTH(a.voucher_date)   Where " & Trim(RptCondt) & IIf(RptCondt <> "", " and ", "") & " ( tG.parent_idno LIKE '%~28~18~') AND a.voucher_date BETWEEN @companyfromdate AND  @uptodate group by MH.Month_ShortName , MH.Idno "
        Nr = cmd.ExecuteNonQuery()


        cmd.CommandText = "Insert into reporttempsub ( Int1 ,  NAME1                ,  currency2) " & _
                                    " Select  MH.Idno , MH.Month_ShortName    ,   abs(sum(a.Voucher_Amount))  from voucher_Details a INNER JOIN company_head tz ON a.company_idno <> 0 and a.company_idno = tz.company_idno INNER JOIN Ledger_Head tP ON a.Ledger_IdNo <> 0 and a.Ledger_idno = tP.Ledger_IdNo  INNER JOIN AccountsGroup_Head tG ON tP.AccountsGroup_IdNo = tG.AccountsGroup_IdNo LEFT OUTER JOIN Month_Head MH ON MH.Month_IdNo = MONTH(a.voucher_date)  Where " & Trim(RptCondt) & IIf(RptCondt <> "", " and ", "") & " ( tG.parent_idno LIKE '%~15~18~' or tG.parent_idno LIKE '~16~18~' or tG.parent_idno LIKE '~27~18~') and a.voucher_date BETWEEN @companyfromdate AND @uptodate   group by MH.Month_ShortName ,MH.Idno"
        Nr = cmd.ExecuteNonQuery()



        cmd.CommandText = "Insert into ReportTemp ( int1 , NAME1   ,  currency1       ,  currency2   ) " & _
                                   " Select         int1 , NAME1   , sum(currency1)   , sum(currency2) from reporttempsub GROUP BY NAME1,int1 order by int1 asc"
        cmd.ExecuteNonQuery()

        Da = New SqlClient.SqlDataAdapter("select NAME1, sum(currency1) AS Sales ,SUM(currency2) as Expense from ReportTemp group by NAME1, int1 Having sum(currency1) <> 0 or SUM(currency2) <> 0 order by int1 asc", con)
        Dt = New DataTable
        Da.Fill(Dt)

        If Dt.Rows.Count > 0 Then
            For i = 0 To Dt.Rows.Count - 1
                Chart1.Series("Expenses").Points.AddXY((Trim(Dt.Rows(i).Item("NAME1").ToString)), Dt.Rows(i).Item("Expense").ToString)
                Chart1.Series("Sales").Points.AddXY((Trim(Dt.Rows(i).Item("NAME1").ToString)), Dt.Rows(i).Item("Sales").ToString)
            Next
        End If

        If Nr = 0 Then Chart1.Visible = False


    End Sub

    Private Sub PieChart_Expense()
        Dim cmd As New SqlClient.SqlCommand
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim Dt1 As New DataTable
        Dim n As Integer = 0
        Dim RptCondt As String = ""
        Dim CompCondt As String = ""
        Dim Nr As Integer = 0
        Dim Total_Value As Double = 0
        Dim Percen As Double = 0

        With Me.Chart2
            .Legends.Clear()
            .Series.Clear()
            .ChartAreas.Clear()
        End With

        Dim areas1 As ChartArea = Me.Chart2.ChartAreas.Add("Areas1")

        With areas1
        End With

        Dim series1 As Series = Me.Chart2.Series.Add("Series1")
        series1.ChartArea = areas1.Name
        series1.ChartType = SeriesChartType.Pie
        series1("PieLabelStyle") = "Disabled"


        cmd.Parameters.Clear()

        cmd.Parameters.AddWithValue("@uptodate", Current_Date)
        cmd.Parameters.AddWithValue("@companyfromdate", Common_Procedures.Company_FromDate)

        CompCondt = ""
        If Trim(UCase(Common_Procedures.User.Type)) <> "UNACCOUNT" Then
            CompCondt = "(Company_Type <> 'UNACCOUNT')"
        End If

        RptCondt = CompCondt

        cmd.Connection = con

        cmd.CommandText = "truncate table reporttempsub"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Insert into reporttempsub (   NAME1               ,  currency1) " & _
                                    " Select            tP.Ledger_Name       ,   abs(sum( a.Voucher_Amount))     from voucher_Details a INNER JOIN company_head tz ON a.company_idno <> 0 and a.company_idno = tz.company_idno INNER JOIN Ledger_Head tP ON a.Ledger_IdNo <> 0 and a.Ledger_idno = tP.Ledger_IdNo  INNER JOIN AccountsGroup_Head tG ON tP.AccountsGroup_IdNo = tG.AccountsGroup_IdNo    Where " & Trim(RptCondt) & IIf(RptCondt <> "", " and ", "") & " ( tG.parent_idno LIKE '%~15~18~' or tG.parent_idno LIKE '~16~18~' or tG.parent_idno LIKE '~27~18~') and a.voucher_date BETWEEN @companyfromdate AND  @uptodate  group by  tP.Ledger_Name"
        cmd.ExecuteNonQuery()


        cmd.CommandText = "truncate table ReportTemp"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Insert into ReportTemp (  NAME1   ,  currency1        ) " & _
                                   " Select          NAME1   , sum(currency1)    from reporttempsub GROUP BY NAME1 having sum(currency1)  <> 0 order by NAME1 asc"
        cmd.ExecuteNonQuery()

        Total_Value = 0
        Da = New SqlClient.SqlDataAdapter("select sum(currency1) AS Expenses  from ReportTemp having sum(currency1)  <> 0", con)
        Dt = New DataTable
        Da.Fill(Dt)
        If Dt.Rows.Count > 0 Then
            For i = 0 To Dt.Rows.Count - 1
                With series1

                    Total_Value = Val(Dt.Rows(i).Item("Expenses").ToString)

                End With
            Next

        Else

            Chart2.Visible = False

        End If

        If Total_Value <> 0 Then

            Da = New SqlClient.SqlDataAdapter("select name1, sum(currency1) AS Expenses from ReportTemp group by name1 having sum(currency1) <> 0 order by name1 ", con)
            Dt = New DataTable
            Da.Fill(Dt)
            If Dt.Rows.Count > 0 Then
                For i = 0 To Dt.Rows.Count - 1
                    With series1

                        If Val(Dt.Rows(i).Item("Expenses").ToString) <> 0 Then
                            Percen = (Val(Dt.Rows(i).Item("Expenses").ToString) / Total_Value) * 100
                        End If
                        .Points.AddXY(Trim(Dt.Rows(i).Item("NAME1").ToString), Percen)

                    End With
                Next
            End If

        End If

        Dim legends1 As Legend = Me.Chart2.Legends.Add("Legends1")

    End Sub

    Private Sub cbo_Ledger_GotFocus(sender As Object, e As System.EventArgs) Handles cbo_Ledger.GotFocus
        con = New SqlClient.SqlConnection(Common_Procedures.Connection_String)
        con.Open()

        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "(AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14 or Show_In_All_Entry = 1)", "(Ledger_IdNo = 0)")
        cbo_Ledger.Tag = cbo_Ledger.Text
    End Sub

    Private Sub cbo_Ledger_KeyDown(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles cbo_Ledger.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Ledger, Nothing, Nothing, "Ledger_AlaisHead", "Ledger_DisplayName", "(AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14 or Show_In_All_Entry = 1)", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_Ledger_KeyPress(sender As Object, e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Ledger.KeyPress
        Try

            Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Ledger, Nothing, "Ledger_AlaisHead", "Ledger_DisplayName", "(AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14 or Show_In_All_Entry = 1)", "(Ledger_IdNo = 0)")

            If Asc(e.KeyChar) = 13 Then

                If Trim(UCase(cbo_Ledger.Tag)) <> Trim(UCase(cbo_Ledger.Text)) Then

                    btn_Select_Ledger_Click(sender, e)

                End If

            End If

        Catch ex As Exception
            '---
        End Try

    End Sub

    Private Sub dgv_OverDueBills_LostFocus(sender As Object, e As System.EventArgs) Handles dgv_OverDueBills.LostFocus
        On Error Resume Next
        dgv_OverDueBills.CurrentCell.Selected = False
    End Sub

    Private Sub btn_Select_Ledger_Click(sender As System.Object, e As System.EventArgs) Handles btn_Select_Ledger.Click
        Try

            cbo_Ledger.Tag = cbo_Ledger.Text

            For i = 0 To dgv_OverDueBills.Rows.Count - 1

                If Trim(UCase(cbo_Ledger.Text)) = Trim(UCase(dgv_OverDueBills.Rows(i).Cells(1).Value)) Then
                    dgv_OverDueBills.Focus()
                    dgv_OverDueBills.CurrentCell = dgv_OverDueBills.Rows(i).Cells(1)
                    Exit For
                End If

            Next i

        Catch ex As Exception
            '---

        End Try

    End Sub

    Private Sub pic_LedgerCreation_Click(sender As System.Object, e As System.EventArgs) Handles pic_LedgerCreation.Click, lbl_Pic_LedgerCreation.Click
        'MDIParent1.mnu_Master_LedgerCreation_Click(sender, e)
    End Sub

    Private Sub pic_AreaCreation_Click(sender As System.Object, e As System.EventArgs) Handles pic_AreaCreation.Click, lbl_Pic_AreaCreation.Click
        'MDIParent1.mnu_Master_AreaCreation_Click(sender, e)
    End Sub

    Private Sub pic_AccountsGroupCreation_Click(sender As System.Object, e As System.EventArgs) Handles pic_AccountsGroupCreation.Click, lbl_Pic_AccountsGroupCreation.Click
        'MDIParent1.mnu_Entry_AccoundsGroup_Creations_Click(sender, e)
    End Sub

    Private Sub pic_Item_Creation_Click(sender As System.Object, e As System.EventArgs) Handles pic_Item_Creation.Click, lbl_Pic_Item_Creation.Click
        'MDIParent1.mnu_Master_ItemCreation_Click(sender, e)
    End Sub

    Private Sub pic_ItemGroup_Creation_Click(sender As System.Object, e As System.EventArgs) Handles pic_ItemGroup_Creation.Click, lbl_Pic_ItemGroup_Creation.Click
        'MDIParent1.mnu_Master_ItemGroupCreation_Click(sender, e)
    End Sub

    Private Sub pic_Category_Creation_Click(sender As System.Object, e As System.EventArgs) Handles pic_Category_Creation.Click, lbl_Pic_Category_Creation.Click
        'MDIParent1.mnu_Master_CategoryCreation_Click(sender, e)
    End Sub

    Private Sub pic_Units_Creation_Click(sender As System.Object, e As System.EventArgs) Handles pic_Units_Creation.Click, lbl_Pic_Units_Creation.Click
        'MDIParent1.mnu_Master_CategoryCreation_Click(sender, e)
    End Sub

    Private Sub pic_User_Creation_Click(sender As System.Object, e As System.EventArgs) Handles pic_User_Creation.Click, lbl_Pic_User_Creation.Click
        'MDIParent1.mnu_Master_UserCreation_Click(sender, e)
    End Sub

    Private Sub ovlshp_PurchaseRegister_Click(sender As System.Object, e As System.EventArgs) Handles ovlshp_PurchaseRegister.Click, lbl_ovlshp_PurchaseRegister.Click
        'MDIParent1.mnu_Reports_PurchaseRegister_Click(sender, e)
    End Sub

    Private Sub ovlshp_PurchaseDetails_Click(sender As System.Object, e As System.EventArgs) Handles ovlshp_PurchaseDetails.Click, lbl_ovlshp_PurchaseDetails.Click
        MDIParent1.mnu_Reports_PurchaseDetails_Click(sender, e)
    End Sub

    Private Sub ovlshp_PurchaseSummary_ItemWise_Click(sender As System.Object, e As System.EventArgs) Handles ovlshp_PurchaseSummary_ItemWise.Click, lbl_ovlshp_PurchaseSummary_ItemWise.Click
        MDIParent1.mnu_Report_Purchase_Summary_ItemWise_Click(sender, e)
    End Sub

    Private Sub ovlshp_SalesRegister_Click(sender As System.Object, e As System.EventArgs) Handles ovlshp_SalesRegister.Click, lbl_ovlshp_SalesRegister.Click
        'MDIParent1.mnu_Reports_SalesRegister_Click(sender, e)
    End Sub

    Private Sub ovlshp_SalesDetails_Click(sender As System.Object, e As System.EventArgs) Handles ovlshp_SalesDetails.Click, lbl_ovlshp_SalesDetails.Click
        'MDIParent1.mnu_Reports_SalesDetails_Click(sender, e)
    End Sub

    Private Sub ovlshp_SalesDetails_MonthWise_Click(sender As System.Object, e As System.EventArgs) Handles ovlshp_SalesDetails_MonthWise.Click, lbl_ovlshp_SalesDetails_MonthWise.Click
        MDIParent1.mnu_Report_Month_wise_sales_Click(sender, e)
        'MDIParent1.mnu_Report_SalesDetails_PartyWise_Click(sender, e)
    End Sub

    Private Sub ovlshp_SalesSummary_PartyWise_Click(sender As System.Object, e As System.EventArgs) Handles ovlshp_SalesSummary_PartyWise.Click, lbl_ovlshp_SalesSummary_PartyWise.Click
        MDIParent1.mnu_Reports_Sales_Summary_PartyWise_Click(sender, e)
    End Sub

    Private Sub ovlshp_SalesSummary_ItemWise_Click(sender As System.Object, e As System.EventArgs) Handles ovlshp_SalesSummary_ItemWise.Click, lbl_ovlshp_SalesSummary_ItemWise.Click
        MDIParent1.mnu_Reports_Sales_Summary_ItemWise_Click(sender, e)
    End Sub

    Private Sub ovlshp_StockDetails_Click(sender As System.Object, e As System.EventArgs) Handles ovlshp_StockDetails.Click, lbl_ovlshp_StockDetails.Click
        'MDIParent1.mnu_Reports_Stock_Details_Click(sender, e)
    End Sub

    Private Sub ovlshp_StockSummary_Click(sender As System.Object, e As System.EventArgs) Handles ovlshp_StockSummary.Click, lbl_ovlshp_StockSummary.Click
        'MDIParent1.mnu_Reports_StockSummary_Click(sender, e)
    End Sub

    Private Sub ovlshp_GSTR1_Click(sender As System.Object, e As System.EventArgs) Handles ovlshp_GSTR1.Click, lbl_ovlshp_GSTR1.Click
        'MDIParent1.mnu_Report_GSTR1_WithPartyName_Click(sender, e)
    End Sub

    Private Sub ovlshp_GSTR2_Click(sender As System.Object, e As System.EventArgs) Handles ovlshp_GSTR2.Click, lbl_ovlshp_GSTR2.Click
        'MDIParent1.mnu_Report_GSTR2_WithPartyName_Click(sender, e)
    End Sub

    Private Sub ovlshp_GSTR_CrDr_Note_Click(sender As System.Object, e As System.EventArgs) Handles ovlshp_GSTR_CrDr_Note.Click, lbl_ovlshp_GSTR_CrDr_Note.Click
        'MDIParent1.mnu_reports_CreditOrDebit_Note_Click(sender, e)
    End Sub

    Private Sub ovlshp_Ac_PartyLedger_Click(sender As System.Object, e As System.EventArgs) Handles ovlshp_Ac_PartyLedger.Click, lbl_ovlshp_Ac_PartyLedger.Click
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1365" Then  ' NACHIYAR TRADINGS
            'MDIParent1.mnu_Accounts_SingleLedger_With_DueDays_Click(sender, e)
        Else
            'MDIParent1.mnu_Accounts_SingleLedger_DateWise_Click(sender, e)
        End If
    End Sub

    Private Sub ovlshp_Ac_SingleLedger_Click(sender As System.Object, e As System.EventArgs) Handles ovlshp_Ac_SingleLedger.Click, lbl_ovlshp_Ac_SingleLedger.Click
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1365" Then  ' NACHIYAR TRADINGS
            'MDIParent1.mnu_Accounts_SingleLedgerDateWise_Grid_With_DueDays_Click(sender, e)
        Else
            'MDIParent1.mnu_Accounts_SingleLedgerDateWise_Grid_Click(sender, e)
        End If
    End Sub

    Private Sub ovlshp_Ac_GroupLedger_Click(sender As System.Object, e As System.EventArgs) Handles ovlshp_Ac_GroupLedger.Click, lbl_ovlshp_Ac_GroupLedger.Click
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1365" Then  ' NACHIYAR TRADINGS
            'MDIParent1.mnu_Accounts_GroupLedger_Grid_Click(sender, e)
        Else
            'MDIParent1.mnu_Accounts_GroupLedger_Click(sender, e)
        End If
    End Sub

    Private Sub ovlshp_Ac_DayBook_Click(sender As System.Object, e As System.EventArgs) Handles ovlshp_Ac_DayBook.Click, lbl_ovlshp_Ac_DayBook.Click
        'MDIParent1.mnu_Accounts_DayBook_Click(sender, e)
    End Sub

    Private Sub ovlshp_Ac_TrialBalance_Click(sender As System.Object, e As System.EventArgs) Handles ovlshp_Ac_TrialBalance.Click, lbl_ovlshp_Ac_TrialBalance.Click
        'MDIParent1.mnu_Accounts_GeneralTB_Click(sender, e)
    End Sub

    Private Sub ovlshp_Ac_SalesParty_OutStanding_BillWise_Click(sender As System.Object, e As System.EventArgs) Handles ovlshp_Ac_SalesParty_OutStanding_BillWise.Click, lbl_ovlshp_Ac_SalesParty_OutStanding_BillWise.Click
        'MDIParent1.mnu_Account_Party_Outstanding_Simple_Click(sender, e)
    End Sub

    Private Sub ovlshp_Ac_SalesParty_OutStanding_DayWise_Click(sender As System.Object, e As System.EventArgs) Handles ovlshp_Ac_SalesParty_OutStanding_DayWise.Click, lbl_ovlshp_Ac_SalesParty_OutStanding_DayWise.Click
        'MDIParent1.mnu_Accounts_PartyBalanceDayWise_Click(sender, e)
    End Sub

    Private Sub ovlshp_Ac_Receipt_Voucher_Report_Click(sender As System.Object, e As System.EventArgs) Handles ovlshp_Ac_Receipt_Voucher_Report.Click, lbl_ovlshp_Ac_Receipt_Voucher_Report.Click
        'MDIParent1.mnu_Accounts_VoucherRegisters_ReceiptRegister_Click(sender, e)
    End Sub

    Private Sub ovlshp_Ac_Payment_Voucher_Report_Click(sender As System.Object, e As System.EventArgs) Handles ovlshp_Ac_Payment_Voucher_Report.Click, lbl_ovlshp_Ac_Payment_Voucher_Report.Click
        'MDIParent1.mnu_Accounts_VoucherRegisters_PaymentRegister_Click(sender, e)
    End Sub

    Private Sub pic_Purchase_Entry_Click(sender As System.Object, e As System.EventArgs) Handles pic_Purchase_Entry.Click, lbl_pic_Purchase_Entry.Click
        'MDIParent1.mnu_Entry_PurchaseEntry_GST_Click(sender, e)
    End Sub

    Private Sub pic_Purchase_Return_Entry_Click(sender As System.Object, e As System.EventArgs) Handles pic_Purchase_Return_Entry.Click, lbl_pic_Purchase_Return_Entry.Click
        'MDIParent1.mnu_Entry_PurchaseReturn_GST_Click(sender, e)
    End Sub

    Private Sub pic_Sales_Entry_Click(sender As System.Object, e As System.EventArgs) Handles pic_Sales_Entry.Click, lbl_pic_Sales_Entry.Click
        'MDIParent1.mnu_Entry_SalesEntry_GST_Click(sender, e)
    End Sub

    Private Sub pic_Sales_Return_Entry_Click(sender As System.Object, e As System.EventArgs) Handles pic_Sales_Return_Entry.Click, lbl_pic_Sales_Return_Entry.Click
        'MDIParent1.mnu_Entry_SalesReturn_GST_Click(sender, e)
    End Sub

    Private Sub pic_Receipt_Entry_Click(sender As System.Object, e As System.EventArgs) Handles pic_Receipt_Entry.Click, lbl_pic_Receipt_Entry.Click
        'MDIParent1.mnu_Entry_Item_Inward_Entry_Click(sender, e)
    End Sub

    Private Sub pic_Delivery_Entry_Click(sender As System.Object, e As System.EventArgs) Handles pic_Delivery_Entry.Click, lbl_pic_Delivery_Entry.Click
        'MDIParent1.mnu_DemoEntry_Delivery_entry_Click(sender, e)
    End Sub

    Private Sub pic_Excess_Short_Entry_Click(sender As System.Object, e As System.EventArgs) Handles pic_Excess_Short_Entry.Click, lbl_pic_Excess_Short_Entry.Click
        'MDIParent1.mnu_Entry_ItemExcessShort_Click(sender, e)
    End Sub

    Private Sub pic_Voucher_Payment_Entry_Click(sender As System.Object, e As System.EventArgs) Handles pic_Voucher_Payment_Entry.Click, lbl_pic_Voucher_Payment_Entry.Click
        'MDIParent1.mnu_voucher_Payment_entry_Click(sender, e)
    End Sub

    Private Sub pic_Voucher_Receipt_Entry_Click(sender As System.Object, e As System.EventArgs) Handles pic_Voucher_Receipt_Entry.Click, lbl_pic_Voucher_Receipt_Entry.Click
        'MDIParent1.mnu_voucher_Receipt_entry_Click(sender, e)
    End Sub

    Private Sub pic_Voucher_Journel_Click(sender As System.Object, e As System.EventArgs) Handles pic_Voucher_Journel.Click, lbl_pic_Voucher_Journel.Click
        'MDIParent1.mnu_Voucher_Journal_Click(sender, e)
    End Sub

    Private Sub pic_Voucher_Petticash_Click(sender As System.Object, e As System.EventArgs) Handles pic_Voucher_Petticash.Click, lbl_pic_Voucher_Petticash.Click
        'MDIParent1.mnu_Voucher_PettiCash_Click(sender, e)
    End Sub

    Private Sub pic_General_Purchase_Entry_Click(sender As System.Object, e As System.EventArgs) Handles pic_General_Purchase_Entry.Click, lbl_pic_General_Purchase_Entry.Click
        'MDIParent1.mnu_Entry_Textile_Purchase_GST_Click(sender, e)
    End Sub

    Private Sub pic_General_Sales_Entry_Click(sender As System.Object, e As System.EventArgs) Handles pic_General_Sales_Entry.Click, lbl_pic_General_Sales_Entry.Click
        'MDIParent1.mnu_Entry_Textile_Sales_GST_Click(sender, e)
    End Sub

    Private Sub pic_General_CreditNote_Entry_Click(sender As System.Object, e As System.EventArgs) Handles pic_General_CreditNote_Entry.Click, lbl_pic_General_CreditNote_Entry.Click
        'MDIParent1.mnu_Entry_Textile_CreditNote_GST_Click(sender, e)
    End Sub

    Private Sub pic_General_DebitNote_Entry_Click(sender As System.Object, e As System.EventArgs) Handles pic_General_DebitNote_Entry.Click, lbl_pic_General_DebitNote_Entry.Click
        'MDIParent1.mnu_Entry_Textile_DebitNote_GST_Click(sender, e)
    End Sub

    Private Sub pic_Other_Purchase_Entry_CLick(sender As System.Object, e As System.EventArgs) Handles pic_Other_Purchase_Entry.Click, lbl_pic_Other_Purchase_Entry.Click
        'MDIParent1.Mnu_Entry_OtherPurchase_Entry_Click(sender, e)
    End Sub


End Class

