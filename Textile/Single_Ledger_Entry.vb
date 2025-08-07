Public Class Single_Ledger_Entry
    Implements Interface_MDIActions

    Private con As New SqlClient.SqlConnection(Common_Procedures.Connection_String)
    Private New_Entry As Boolean = False
    Private Prec_ActCtrl As New Control
    Private vcbo_KeyDwnVal As Double
    Private WithEvents dgtxt_Details As New DataGridViewTextBoxEditingControl


    Private Print_PDF_Status As Boolean = False

    Private prn_HdDt As New DataTable
    Private prn_DetDt As New DataTable
    Private prn_DetDt_sub As New DataTable
    Private prn_PageNo As Integer
    Private prn_Status As Integer = 0
    Private prn_DetIndx As Integer
    Private prn_DetMxIndx As Integer
    Private prn_NoofBmDets As Integer
    Private prn_DetSNo As Integer
    Private NoFo_STS As Integer = 0
    Private prn_HdIndx As Integer
    Private prn_HdMxIndx As Integer
    Private prn_Count As Integer
    Private prn_HdAr(1000, 10) As String
    Private prn_DetAr(1000, 50, 10) As String
    Private prn_InpOpts As String = ""
    Private prn_OriDupTri As String = ""
    Private Total_mtrs As Single = 0
    Private Print_Format As String = ""
    Private Printing_Btn_Status As Boolean = False



    Private Sub clear()

        New_Entry = False
        pnl_Back.Enabled = True

        cbo_AccountName.Text = ""
      

        dgv_Details.Rows.Clear()
        dgv_Details_Total.Rows.Clear()

    End Sub
    Private Sub ControlGotFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim txtbx As TextBox
        Dim combobx As ComboBox
        Dim mskbx As MaskedTextBox

        On Error Resume Next

        If TypeOf Me.ActiveControl Is TextBox Or TypeOf Me.ActiveControl Is ComboBox Or TypeOf Me.ActiveControl Is MaskedTextBox Then
            Me.ActiveControl.BackColor = Color.Lime
            Me.ActiveControl.ForeColor = Color.Blue
        End If

        If TypeOf Me.ActiveControl Is TextBox Then
            txtbx = Me.ActiveControl
            txtbx.SelectAll()
        ElseIf TypeOf Me.ActiveControl Is ComboBox Then
            combobx = Me.ActiveControl
            combobx.SelectAll()
        ElseIf TypeOf Me.ActiveControl Is MaskedTextBox Then
            mskbx = Me.ActiveControl
            mskbx.SelectAll()
        End If

        Prec_ActCtrl = Me.ActiveControl

    End Sub

    Private Sub ControlLostFocus(ByVal sender As Object, ByVal e As System.EventArgs)

        On Error Resume Next

        If IsNothing(Prec_ActCtrl) = False Then
            If TypeOf Prec_ActCtrl Is TextBox Or TypeOf Prec_ActCtrl Is ComboBox Or TypeOf Prec_ActCtrl Is MaskedTextBox Then
                Prec_ActCtrl.BackColor = Color.White
                Prec_ActCtrl.ForeColor = Color.Black
            End If
        End If

    End Sub

    Public Sub move_record(ByVal idno As Integer)
        '  If idno <= 100 Then Exit Sub

        Get_VoucherDetails(idno)
        cbo_AccountName.Text = Common_Procedures.Ledger_IdNoToName(con, idno)

    End Sub

    Private Sub Single_Ledger_Entry_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated

        lbl_Company.Text = ""
        lbl_Company.Tag = 0
        Common_Procedures.CompIdNo = 0

        Me.Text = ""

        lbl_Company.Text = Common_Procedures.get_Company_From_CompanySelection(con)
        lbl_Company.Tag = Val(Common_Procedures.CompIdNo)


        dtp_FromDate.Text = Common_Procedures.Company_FromDate
        dtp_ToDate.Text = Now

        msk_Fromdate.Text = dtp_FromDate.Text
        msk_Todate.Text = dtp_ToDate.Text

        Me.Text = lbl_Company.Text
    End Sub

    Private Sub Single_Ledger_Entry_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Me.Height = 284 ' 197

        con.Open()


        pnl_Print.Visible = False
        pnl_Print.Left = (Me.Width - pnl_Print.Width) \ 2
        pnl_Print.Top = (Me.Height - pnl_Print.Height) \ 2
        pnl_Print.BringToFront()

        AddHandler cbo_AccountName.GotFocus, AddressOf ControlGotFocus
        AddHandler msk_Fromdate.GotFocus, AddressOf ControlGotFocus
        AddHandler msk_Todate.GotFocus, AddressOf ControlGotFocus

        AddHandler cbo_AccountName.LostFocus, AddressOf ControlLostFocus
        AddHandler msk_Fromdate.LostFocus, AddressOf ControlLostFocus
        AddHandler msk_Todate.LostFocus, AddressOf ControlLostFocus



        new_record()

    End Sub

    Private Sub Single_Ledger_Entry_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If Asc(e.KeyChar) = 27 Then

            If pnl_Print.Visible = True Then
                btn_Print_Cancel_Click(sender, e)
                Exit Sub

            End If
        End If
    End Sub

    Private Sub Single_Ledger_Entry_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        On Error Resume Next
        con.Close()
        con.Dispose()
        Common_Procedures.Last_Closed_FormName = Me.Name
    End Sub

    Private Sub btn_Close_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Me.Close()
        Me.Dispose()
    End Sub

    Public Sub delete_record() Implements Interface_MDIActions.delete_record
        

    End Sub

    Public Sub filter_record() Implements Interface_MDIActions.filter_record
        
    End Sub

    Public Sub movefirst_record() Implements Interface_MDIActions.movefirst_record
       Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim movno As String = ""


        Try

            da = New SqlClient.SqlDataAdapter("select top 1 Ledger_IdNo from Single_Ledger_Print_Details where Ledger_IdNo <> 0 Order by Ledger_IdNo", con)
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

            If Val(movno) <> 0 Then

                If movno <= 100 Then move_record(movno + 100)
            End If


        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR  MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        If Val(movno) <> 0 Then move_record(movno)

       
    End Sub

    Public Sub movelast_record() Implements Interface_MDIActions.movelast_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim movno As String = ""

        Try
            da = New SqlClient.SqlDataAdapter("select top 1 Ledger_IdNo from Single_Ledger_Print_Details where ledger_idno <> 0 Order by Ledger_IdNo desc", con)
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

        End Try

    End Sub

    Public Sub movenext_record() Implements Interface_MDIActions.movenext_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim movno As String = ""
        Dim OrdByNo As Single = 0

        Try


            OrdByNo = Common_Procedures.OrderBy_CodeToValue(Common_Procedures.Ledger_NameToIdNo(con, Trim(cbo_AccountName.Text)))

            da = New SqlClient.SqlDataAdapter("select top 1 Ledger_IdNo from Single_Ledger_Print_Details where Ledger_IdNo > " & Str(OrdByNo) & " Order by Ledger_IdNo", con)
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

        End Try

    End Sub

    Public Sub moveprevious_record() Implements Interface_MDIActions.moveprevious_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim movno As String = ""
        Dim OrdByNo As Single = 0

        Try


            OrdByNo = Common_Procedures.OrderBy_CodeToValue(Common_Procedures.Ledger_NameToIdNo(con, Trim(cbo_AccountName.Text)))

            da = New SqlClient.SqlDataAdapter("select top 1 Ledger_IdNo from Single_Ledger_Print_Details where ledger_idno < " & Str(Val(OrdByNo)) & " Order by Ledger_IdNo desc", con)
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

        End Try


    End Sub

    Public Sub new_record() Implements Interface_MDIActions.new_record

        clear()
    End Sub

    Public Sub open_record() Implements Interface_MDIActions.open_record
       
    End Sub

    Public Sub insert_record() Implements Interface_MDIActions.insert_record
        'MessageBox.Show("insert record")
    End Sub

    Public Sub print_record() Implements Interface_MDIActions.print_record

      
        If Common_Procedures.Print_OR_Preview_Status = 1 Then
            If MessageBox.Show("Check and Set Paper Position Correctly...", "READY TO PRINT", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) = vbOK Then
                pnl_Back.Enabled = False
                pnl_Print.Visible = True
                lbl_PrintPanel_Caption.Text = "PRINT OPTION"
            Else
                Move_Next()
                Exit Sub
            End If
        Else
            If MessageBox.Show("Check and Set Paper Position Correctly...", "READY TO PRINT", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) = vbOK Then
                lbl_PrintPanel_Caption.Text = "PREVIEW OPTION"
                pnl_Back.Enabled = False
                pnl_Print.Visible = True
            Else
                Move_Next()
                Exit Sub
            End If
          
        End If
        


    End Sub
    Private Sub Printing_Invoice()
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim PpSzSTS As Boolean = False
        Dim I As Integer = 0
        Dim ps As Printing.PaperSize


        Try


        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        If PpSzSTS = False Then
            For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
                If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
                    ps = PrintDocument1.PrinterSettings.PaperSizes(I)
                    PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = ps
                    PrintDocument1.DefaultPageSettings.PaperSize = ps
                    Exit For
                End If
            Next
        End If



        If Val(Common_Procedures.Print_OR_Preview_Status) = 1 Then

            Try
                If Val(Common_Procedures.settings.Printing_Show_PrintDialogue) = 1 Then
                    PrintDialog1.PrinterSettings = PrintDocument1.PrinterSettings
                    If PrintDialog1.ShowDialog() = Windows.Forms.DialogResult.OK Then
                        PrintDocument1.PrinterSettings = PrintDialog1.PrinterSettings



                        If PpSzSTS = False Then


                            If PpSzSTS = False Then
                                For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
                                    If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
                                        ps = PrintDocument1.PrinterSettings.PaperSizes(I)
                                        PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = ps
                                        PrintDocument1.DefaultPageSettings.PaperSize = ps
                                        Exit For
                                    End If
                                Next
                            End If

                        End If

                        PrintDocument1.Print()
                    End If

                Else

                    PrintDocument1.Print()

                End If

            Catch ex As Exception
                MessageBox.Show("The printing operation failed" & vbCrLf & ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

            End Try


        Else
            Try

                Dim ppd As New PrintPreviewDialog

                ppd.Document = PrintDocument1

                ppd.WindowState = FormWindowState.Normal
                ppd.StartPosition = FormStartPosition.CenterScreen
                ppd.ClientSize = New Size(600, 600)

                ppd.ShowDialog()

            Catch ex As Exception
                MsgBox("The printing operation failed" & vbCrLf & ex.Message, MsgBoxStyle.Critical, "DOES NOT SHOW PRINT PREVIEW...")

            End Try

        End If

    End Sub

    Public Sub save_record() Implements Interface_MDIActions.save_record
        Dim cmd As New SqlClient.SqlCommand
        Dim trans As SqlClient.SqlTransaction
        Dim Led_Id As Integer = 0
        Dim Nr As Integer = 0

        If pnl_Back.Enabled = False Then
            MessageBox.Show("Close other window", "DOES NOT SAVE", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        'If Common_Procedures.UserRight_Check(Common_Procedures.UR.Master_Area_Creation, New_Entry) = False Then Exit Sub

        Led_Id = Val(Common_Procedures.Ledger_NameToIdNo(con, Trim(cbo_AccountName.Text)))

        If Led_Id = 0 Then
            MessageBox.Show("Invalid Account Name", "DOES NOT SAVE", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If cbo_AccountName.Visible And cbo_AccountName.Enabled Then cbo_AccountName.Focus()
            Exit Sub
        End If


        trans = con.BeginTransaction

        Try
            cmd.Connection = con
            cmd.Transaction = trans

            If dgv_Details.Rows.Count > 0 Then
                For i = 0 To dgv_Details.Rows.Count - 1

                    If dgv_Details.Rows(i).Cells(5).Value <> "" Then

                        cmd.CommandText = "Update Single_Ledger_Print_Details SET Print_Status = 1 ,  Page_No =" & Val(dgv_Details.Rows(i).Cells(6).Value) & " ,Balance_Amount = '" & Trim(dgv_Details.Rows(i).Cells(4).Value) & "'   Where Voucher_Code = '" & Trim(dgv_Details.Rows(i).Cells(7).Value) & "' and  Ledger_IdNo =" & Val(Led_Id) & "   and Company_Idno =  " & Val(lbl_Company.Tag) & ""
                        cmd.ExecuteNonQuery()

                        For cc = 0 To 6
                            dgv_Details.Rows(i).Cells(cc).Style.BackColor = Color.Gainsboro
                        Next
                    Else

                        cmd.CommandText = "Update Single_Ledger_Print_Details SET   Print_Status = 0 , Page_No =" & Val(dgv_Details.Rows(i).Cells(6).Value) & " , Balance_Amount = '" & Trim(dgv_Details.Rows(i).Cells(4).Value) & "'   Where Voucher_Code = '" & Trim(dgv_Details.Rows(i).Cells(7).Value) & "' and  Ledger_IdNo =" & Val(Led_Id) & "   and Company_Idno =  " & Val(lbl_Company.Tag) & ""
                        cmd.ExecuteNonQuery()

                        For cc = 0 To 6
                            dgv_Details.Rows(i).Cells(cc).Style.BackColor = Color.White
                        Next
                    End If

                Next
            End If
         

            trans.Commit()


            MessageBox.Show("Saved Sucessfully!!!", "FOR SAVING....", MessageBoxButtons.OK, MessageBoxIcon.Information)



        Catch ex As Exception
            trans.Rollback()

       
                MessageBox.Show(ex.Message, "DOES NOT SAVE", MessageBoxButtons.OK, MessageBoxIcon.Error)



        Finally
          
        End Try

    End Sub

    Private Sub btn_Save_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_save.Click
        save_record()
    End Sub

  
    Private Sub Get_VoucherDetails(ByVal Led_IdNo As Integer)
        Dim cmd As New SqlClient.SqlCommand
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dtbl1 As New DataTable
        Dim Dtbl2 As New DataTable
        Dim Dt As New DataTable
        Dim Amt As Double = 0, BillPend As Double = 0
        Dim RptCondt As String
        Dim Tot_CR As Decimal = 0, Tot_DB As Decimal = 0
        Dim Bal As Decimal
        Dim Nr As Integer = 0
        Dim n As Integer
        Dim SNo As Integer
        Dim Page_Count As Integer = 0, Row_Count As Integer = 0
        Dim Print_Sts As Integer = 0
        Dim Row_No As Integer = 0, PageNo As Integer

        If IsDate(msk_Fromdate.Text) = False Then
            MessageBox.Show("Invalid Date", "DOES NOT SHOW REPORT...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If msk_Fromdate.Visible = True And msk_Fromdate.Enabled = True Then msk_Fromdate.Focus()
            Exit Sub
        End If

        If IsDate(msk_Todate.Text) = False Then
            MessageBox.Show("Invalid Date", "DOES NOT SHOW REPORT...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If msk_Todate.Visible = True And msk_Todate.Enabled = True Then msk_Todate.Focus()
            Exit Sub
        End If


        ' Led_IdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, Trim(cbo_AccountName.Text))

        If Led_IdNo = 0 Then
            MessageBox.Show("Invalid Account Name", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If cbo_AccountName.Enabled And cbo_AccountName.Visible Then cbo_AccountName.Focus()
            Exit Sub
        End If

        cmd.Connection = con

        cmd.Parameters.Clear()
        cmd.Parameters.AddWithValue("@fromdate", dtp_FromDate.Value.Date)
        cmd.Parameters.AddWithValue("@todate", dtp_ToDate.Value.Date)
        cmd.Parameters.AddWithValue("@companyfromdate", Common_Procedures.Company_FromDate)

        RptCondt = ""
        RptCondt = " a.Company_IdNo = " & Str(Val(lbl_Company.Tag))

        If cbo_AccountName.Visible = True And Val(Led_IdNo) <> 0 Then
            RptCondt = Trim(RptCondt) & IIf(Trim(RptCondt) <> "", " and ", "") & " a.Ledger_IdNo = " & Str(Val(Led_IdNo))
        End If


        Amt = 0
        cmd.CommandText = "select sum(a.voucher_amount) from voucher_details a, ledger_head b, Company_Head tZ where " & RptCondt & IIf(RptCondt <> "", " and ", "") & " a.voucher_date < @companyfromdate and a.ledger_idno = b.ledger_idno and b.parent_code NOT LIKE '%~18~' and a.company_idno = tZ.company_idno"
        Da = New SqlClient.SqlDataAdapter(cmd)
        Dt = New DataTable
        Da.Fill(Dt)

        If Dt.Rows.Count > 0 Then
            If IsDBNull(Dt.Rows(0)(0).ToString) = False Then
                Amt = Val(Dt.Rows(0)(0).ToString)
            End If
        End If
        Dt.Clear()

        cmd.CommandText = "Truncate table " & Trim(Common_Procedures.ReportTempTable) & ""
        cmd.ExecuteNonQuery()

        '-------Opening
        If Amt <> 0 Then
            cmd.CommandText = "Insert into " & Trim(Common_Procedures.ReportTempTable) & "(Int5  ,Date1             ,int6                          ,Int7                              , Meters1 , Name1    , Name2, Name3    , Name4, Name5 , Currency1                             , Currency2                   , Meters6 ) " &
                                             "values (0 , @companyfromdate , " & Str(Val(Led_IdNo)) & "   , " & Str(Val(lbl_Company.Tag)) & "    ,0       , 'OPENING' , ''   , 'OPENING', ''   , ''    , " & IIf(Amt < 0, Math.Abs(Amt), 0) & ", " & IIf(Amt > 0, Amt, 0) & ", " & Str(Val(BillPend)) & " ) "
            cmd.ExecuteNonQuery()

        End If


        '---From Voucher Debit Details
        cmd.CommandText = "Insert into " & Trim(Common_Procedures.ReportTempTable) & "(Meters5, Int5, Date1         , Meters1      , Name1         , Name2       , Name3        , Currency1            , Currency2,    Name4   ,   Name5       ,   Name7               ,    Name8            ,     Name9         ,int6            ,Int7) " &
                                              "select 0  ,  1  , a.Voucher_Date, b.For_OrderBy, b.Voucher_Code, b.Voucher_No, a.Narration  , Abs(a.voucher_amount),    0     , a.narration, a.Voucher_Type, a.Entry_Identification, tZ.Company_ShortName, c.Parent_Code     ,b.debtor_idno ,a.Company_Idno from voucher_details a INNER JOIN Company_Head tZ ON a.Company_IdNo = tZ.Company_IdNo LEFT OUTER JOIN voucher_head b ON a.Voucher_Code = b.Voucher_Code and a.Company_Idno = b.Company_Idno LEFT OUTER JOIN ledger_head c ON b.creditor_idno = c.ledger_idno where " & RptCondt & IIf(RptCondt <> "", " and ", "") & " a.voucher_date between @companyfromdate and @todate and a.voucher_amount < 0"
        cmd.ExecuteNonQuery()

        '---From Voucher Credit Details
        cmd.CommandText = "Insert into " & Trim(Common_Procedures.ReportTempTable) & "(Meters5, Int5, Date1         , Meters1      , Name1         , Name2       , Name3                , Currency1, Currency2       , Name4      , Name5         , Name7                 , Name8               , Name9         ,int6          , Int7) " &
                                            "select 0    ,  2  , a.Voucher_Date, b.For_OrderBy, b.Voucher_Code, b.Voucher_No,  a.Narration ,   0      , a.Voucher_Amount, a.narration, a.Voucher_Type, a.Entry_Identification, tZ.Company_ShortName, c.Parent_Code ,b.creditor_idno ,a.Company_Idno from voucher_details a INNER JOIN Company_Head tZ ON a.Company_IdNo = tZ.Company_IdNo LEFT OUTER JOIN voucher_head b ON a.Voucher_Code = b.Voucher_Code and a.Company_Idno = b.Company_Idno LEFT OUTER JOIN ledger_head c ON b.debtor_idno = c.ledger_idno where " & RptCondt & IIf(RptCondt <> "", " and ", "") & " a.voucher_date between @companyfromdate and @todate and a.voucher_amount > 0"
        cmd.ExecuteNonQuery()


        cmd.CommandText = "Update " & Trim(Common_Procedures.ReportTempTable) & " SET Meters5 = b.LedgerOrder_Position from " & Trim(Common_Procedures.ReportTempTable) & " a, AccountsGroup_Head b Where a.Name9 COLLATE Latin1_General_CI_AI = b.Parent_Idno"
        cmd.ExecuteNonQuery()

        '--------Deleted Vouchers Checking-------

        Check_Delete_Status(Led_IdNo)


        '--- Insert or update To print table
        Da = New SqlClient.SqlDataAdapter("select int6 as LedgerId, Int7 as Comp_Id,  Date1 as VouDate, Name5 as VouType, Name8 as Company_ShortName, Name2 as VouNo, Name3 as Particulars, Currency1 as Debit, Currency2 as Credit, Name6 as Balance, Name4 as Narration, Name7 as VoucherCode from " & Trim(Common_Procedures.ReportTempTable) & " Order by Date1, Meters5, Int5, meters1, name2, name1", con)
        Dtbl1 = New DataTable
        Da.Fill(Dtbl1)
        If Dtbl1.Rows.Count > 0 Then

            For i = 0 To Dtbl1.Rows.Count - 1

                cmd.Parameters.Clear()
                cmd.Parameters.AddWithValue("@voudate", Convert.ToDateTime(Dtbl1.Rows(i).Item("VouDate").ToString))

                Print_Sts = 0
                Print_Sts = Val(Common_Procedures.get_FieldValue(con, "Single_Ledger_Print_Details", "Print_Status", " Voucher_Code = '" & Trim(Dtbl1.Rows(i).Item("VoucherCode").ToString) & "' and  Ledger_IdNo =" & Val(Dtbl1.Rows(i).Item("LedgerId").ToString) & "   and Company_Idno =  " & Val(Dtbl1.Rows(i).Item("Comp_Id").ToString) & " and Voucher_Type = '" & Trim(Dtbl1.Rows(i).Item("VouType").ToString) & "' ", Val(Dtbl1.Rows(i).Item("Comp_Id").ToString)))

                If Print_Sts = 0 Then

                    Nr = 0
                    cmd.CommandText = "Update Single_Ledger_Print_Details SET Particulars = '" & Trim(Dtbl1.Rows(i).Item("Particulars").ToString) & "' ,Debit_Amount  =" & Val(Dtbl1.Rows(i).Item("Debit").ToString) & " ,Credit_Amount  =" & Val(Dtbl1.Rows(i).Item("Credit").ToString) & ",  Balance_Amount ='" & Trim(Dtbl1.Rows(i).Item("Balance").ToString) & "'  Where Voucher_Code = '" & Trim(Dtbl1.Rows(i).Item("VoucherCode").ToString) & "' and  Ledger_IdNo =" & Val(Dtbl1.Rows(i).Item("LedgerId").ToString) & "   and Company_Idno =  " & Val(Dtbl1.Rows(i).Item("Comp_Id").ToString) & " and Voucher_Type = '" & Trim(Dtbl1.Rows(i).Item("VouType").ToString) & "' "
                    Nr = cmd.ExecuteNonQuery()

                    If Nr = 0 Then
                        '---Getting Last Row no from Table
                        Row_No = Get_Record_Count(Led_IdNo) + 1

                        PageNo = Row_No / 28
                        If PageNo < (Row_No / 28) Then
                            PageNo = PageNo + 1
                        End If
                        cmd.CommandText = "Insert into Single_Ledger_Print_Details(Ledger_IdNo                                           , Company_Idno                                           , Sl_No                                   ,Voucher_Date   ,Particulars                                                 ,Debit_Amount                                         ,Credit_Amount                                        ,         Balance_Amount                                   , Print_Status     ,Page_No            , Row_No                   ,Voucher_Code                                             ,Voucher_No                                              ,Voucher_Type) " &
                                                                        "values(  " & Val(Dtbl1.Rows(i).Item("LedgerId").ToString) & "   ,   " & Val(Dtbl1.Rows(i).Item("Comp_Id").ToString) & "  ,  " & Get_Record_Count(Led_IdNo) + 1 & " ,   @voudate    , '" & Trim(Dtbl1.Rows(i).Item("Particulars").ToString) & "' ,  " & Val(Dtbl1.Rows(i).Item("Debit").ToString) & "  , " & Val(Dtbl1.Rows(i).Item("Credit").ToString) & "  , '" & Trim(Dtbl1.Rows(i).Item("Balance").ToString) & "'   ,          0       , " & PageNo & "    ,    " & Val(Row_No) & "   , '" & Trim(Dtbl1.Rows(i).Item("VoucherCode").ToString) & "' , '" & Trim(Dtbl1.Rows(i).Item("VouNo").ToString) & "', '" & Trim(Dtbl1.Rows(i).Item("VouType").ToString) & "' )"
                        Nr = cmd.ExecuteNonQuery()

                    End If

                End If

            Next i

        End If



        cmd.Parameters.Clear()
        cmd.Parameters.AddWithValue("@fromdate", dtp_FromDate.Value.Date)
        cmd.Parameters.AddWithValue("@todate", dtp_ToDate.Value.Date)
        cmd.Parameters.AddWithValue("@companyfromdate", Common_Procedures.Company_FromDate)


        Tot_CR = 0 : Tot_DB = 0 : Bal = 0 : Page_Count = 1 : Row_Count = 0

        '---Fill the values to dgv
        cmd.CommandText = "select * from Single_Ledger_Print_Details where Voucher_Date BETWEEN  @fromdate and @todate and Ledger_IdNo = " & Val(Led_IdNo) & "  order by Row_No asc  "
        Da = New SqlClient.SqlDataAdapter(cmd)
        Dt = New DataTable
        Da.Fill(Dt)
        If Dt.Rows.Count > 0 Then
            dgv_Details.Rows.Clear()
            SNo = 0

            For i = 0 To Dt.Rows.Count - 1
                n = dgv_Details.Rows.Add()

                dgv_Details.Rows(n).Cells(0).Value = Format((Dt.Rows(i).Item("Voucher_Date")), "dd/MM/yyyy")
                dgv_Details.Rows(n).Cells(1).Value = Trim(Dt.Rows(i).Item("Particulars").ToString)
                dgv_Details.Rows(n).Cells(2).Value = IIf(Val(Dt.Rows(i).Item("Debit_Amount").ToString) <> 0, Val(Dt.Rows(i).Item("Debit_Amount").ToString), "")
                dgv_Details.Rows(n).Cells(3).Value = IIf(Val(Dt.Rows(i).Item("Credit_Amount").ToString) <> 0, Val(Dt.Rows(i).Item("Credit_Amount").ToString), "")
                dgv_Details.Rows(n).Cells(5).Value = IIf(Val((Dt.Rows(i).Item("Print_Status").ToString)) > 0, "Y", "")
                dgv_Details.Rows(n).Cells(6).Value = Val(Dt.Rows(i).Item("Page_No").ToString)
                dgv_Details.Rows(n).Cells(7).Value = (Dt.Rows(i).Item("Voucher_Code").ToString)
                dgv_Details.Rows(n).Cells(8).Value = (Dt.Rows(i).Item("Voucher_Type").ToString)
                dgv_Details.Rows(n).Cells(9).Value = Val(Dt.Rows(i).Item("Row_No").ToString)


                Tot_DB = Tot_DB + Val(Dt.Rows(i).Item("Debit_Amount").ToString)
                Tot_CR = Tot_CR + Val(Dt.Rows(i).Item("Credit_Amount").ToString)
                Bal = Val(Bal) + Val(Dt.Rows(i).Item("Debit_Amount").ToString) - Val(Dt.Rows(i).Item("Credit_Amount").ToString)

                dgv_Details.Rows(n).Cells(4).Value = Trim(Format(Math.Abs(Val(Bal)), "#########0")) & IIf(Val(Bal) >= 0, " Dr", " Cr")

                '---Update Balance Amount row by row

                cmd.CommandText = "Update Single_Ledger_Print_Details SET Balance_Amount = '" & Trim(dgv_Details.Rows(n).Cells(4).Value) & "'   Where Voucher_Code = '" & Trim(Dt.Rows(i).Item("Voucher_Code").ToString) & "' and  Ledger_IdNo =" & Val(Dt.Rows(i).Item("Ledger_IdNo").ToString) & "   and Company_Idno =  " & Val(Dt.Rows(i).Item("Company_Idno").ToString) & " and Voucher_Type = '" & Trim(Dt.Rows(i).Item("Voucher_Type").ToString) & "' "
                cmd.ExecuteNonQuery()

                If Val((Dt.Rows(i).Item("Print_Status").ToString)) > 0 Then
                    For cc = 0 To 6
                        dgv_Details.Rows(n).Cells(cc).Style.BackColor = Color.Gainsboro
                    Next
                End If
            Next


        End If
        Dt.Clear()
        Dt.Dispose()


    End Sub

    Private Sub btn_Show_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Show.Click
        Dim Led_Id As Integer = 0

        Led_Id = Val(Common_Procedures.Ledger_AlaisNameToIdNo(con, Trim(cbo_AccountName.Text)))

        clear()

        Get_VoucherDetails(Led_Id)

        cbo_AccountName.Text = Trim(Common_Procedures.Ledger_IdNoToName(con, Val(Led_Id)))

        Total_Calculation()

    End Sub

    Private Sub cbo_AccountName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_AccountName.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "AccountsGroup_IdNo =10 and Close_status = 0", "(Ledger_idno = 0)")


    End Sub

    Private Sub cbo_AccountName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_AccountName.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_AccountName, Nothing, msk_Fromdate, "Ledger_AlaisHead", "Ledger_DisplayName", "AccountsGroup_IdNo =10 and Close_status = 0", "(Ledger_idno = 0)")




        If (e.KeyValue = 38 And cbo_AccountName.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then

            msk_Fromdate.Focus()

        End If
        If (e.KeyValue = 40 And cbo_AccountName.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then

            msk_Fromdate.Focus()
        End If


    End Sub

    Private Sub cbo_AccountName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_AccountName.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_AccountName, msk_Fromdate, "Ledger_AlaisHead", "Ledger_DisplayName", "AccountsGroup_IdNo =10 and Close_status = 0", "(Ledger_idno = 0)")

        If Asc(e.KeyChar) = 13 Then
            msk_Fromdate.Focus()

        End If
    End Sub

    Private Sub cbo_AccountName_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_AccountName.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Common_Procedures.MDI_LedType = ""
            Dim f As New Ledger_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_AccountName.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If
    End Sub
    Private Sub msk_Fromdate_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles msk_Fromdate.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        If e.KeyValue = 38 Then
            cbo_AccountName.Focus()
        End If
        If e.KeyValue = 40 Then
            msk_Todate.Focus()
        End If
    End Sub

    Private Sub msk_Fromdate_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles msk_Fromdate.KeyPress
        If Asc(e.KeyChar) = 13 Then
            msk_Todate.Focus()
        End If
        If Trim(UCase(e.KeyChar)) = "D" Then
            msk_Fromdate.Text = Date.Today
            msk_Fromdate.SelectionStart = 0
        End If
    End Sub
    Private Sub msk_Fromdate_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles msk_Fromdate.KeyUp
        'If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
        '    msk_Fromdate.Text = Date.Today
        'End If
        If IsDate(msk_Fromdate.Text) = True Then
            If e.KeyCode = 107 Then
                msk_Fromdate.Text = DateAdd("D", 1, Convert.ToDateTime(msk_Fromdate.Text))
            ElseIf e.KeyCode = 109 Then
                msk_Fromdate.Text = DateAdd("D", -1, Convert.ToDateTime(msk_Fromdate.Text))
            End If
        End If
    End Sub
    Private Sub msk_Fromdate_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles msk_Fromdate.LostFocus

        If IsDate(msk_Fromdate.Text) = True Then
            If Microsoft.VisualBasic.DateAndTime.Day(Convert.ToDateTime(msk_Fromdate.Text)) <= 31 Or Microsoft.VisualBasic.DateAndTime.Month(Convert.ToDateTime(msk_Fromdate.Text)) <= 31 Then
                If Microsoft.VisualBasic.DateAndTime.Year(Convert.ToDateTime(msk_Fromdate.Text)) <= 2050 And Microsoft.VisualBasic.DateAndTime.Year(Convert.ToDateTime(msk_Fromdate.Text)) >= 2000 Then
                    dtp_FromDate.Value = Convert.ToDateTime(msk_Fromdate.Text)
                End If
            End If

        End If
    End Sub
    Private Sub dtp_FromDate_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dtp_FromDate.TextChanged
        If IsDate(dtp_FromDate.Text) = True Then
            msk_Fromdate.Text = dtp_FromDate.Text
        End If
    End Sub


    Private Sub msk_Todate_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles msk_Todate.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        If e.KeyValue = 38 Then
            msk_Fromdate.Focus()
        End If
        If e.KeyValue = 40 Then
            btn_Show.Focus()
        End If
    End Sub

    Private Sub msk_Todate_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles msk_Todate.KeyPress
        If Asc(e.KeyChar) = 13 Then
            Get_VoucherDetails(Common_Procedures.Ledger_AlaisNameToIdNo(con, Trim(cbo_AccountName.Text)))
        End If
        If Trim(UCase(e.KeyChar)) = "D" Then
            msk_Todate.Text = Date.Today
            msk_Todate.SelectionStart = 0
        End If
    End Sub
    Private Sub msk_Todate_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles msk_Todate.KeyUp
        'If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
        '    msk_Todate.Text = Date.Today
        'End If
        If IsDate(msk_Todate.Text) = True Then
            If e.KeyCode = 107 Then
                msk_Todate.Text = DateAdd("D", 1, Convert.ToDateTime(msk_Todate.Text))
            ElseIf e.KeyCode = 109 Then
                msk_Todate.Text = DateAdd("D", -1, Convert.ToDateTime(msk_Todate.Text))
            End If
        End If
    End Sub
    Private Sub msk_Todate_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles msk_Todate.LostFocus

        If IsDate(msk_Todate.Text) = True Then
            If Microsoft.VisualBasic.DateAndTime.Day(Convert.ToDateTime(msk_Todate.Text)) <= 31 Or Microsoft.VisualBasic.DateAndTime.Month(Convert.ToDateTime(msk_Todate.Text)) <= 31 Then
                If Microsoft.VisualBasic.DateAndTime.Year(Convert.ToDateTime(msk_Todate.Text)) <= 2050 And Microsoft.VisualBasic.DateAndTime.Year(Convert.ToDateTime(msk_Todate.Text)) >= 2000 Then
                    dtp_ToDate.Value = Convert.ToDateTime(msk_Todate.Text)
                End If
            End If

        End If
    End Sub
    Private Sub dtp_ToDate_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dtp_ToDate.TextChanged
        If IsDate(dtp_ToDate.Text) = True Then
            msk_Todate.Text = dtp_ToDate.Text
        End If
    End Sub

    Private Sub dgv_Report_EditingControlShowing(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewEditingControlShowingEventArgs) Handles dgv_Details.EditingControlShowing
        dgtxt_Details = CType(dgv_Details.EditingControl, DataGridViewTextBoxEditingControl)
    End Sub
    Private Sub dgtxt_Details_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgtxt_Details.Enter
        dgv_Details.EditingControl.BackColor = Color.Pink
        dgv_Details.EditingControl.ForeColor = Color.Black
        dgtxt_Details.SelectAll()
    End Sub



    Private Sub dgtxt_Details_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgtxt_Details.KeyDown
        Try
            vcbo_KeyDwnVal = e.KeyValue

        Catch ex As Exception
            MessageBox.Show(ex.Message, "ERROR WHILE DETAILS TEXT KEYDOWN....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub dgtxt_Details_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dgtxt_Details.KeyPress
        With dgv_Details
            If .Visible Then
                If .CurrentCell.ColumnIndex = 5 Then

                    'If (Asc(e.KeyChar)) = 89 Then
                    '    e.Handled = True
                    'End If

                End If
            End If
        End With
    End Sub
    Private Sub Total_Calculation()
        Dim TotDr As Single, TotCr As Single, TotBal As Single


        TotDr = 0 : TotCr = 0 : TotBal = 0

        With dgv_Details

            For i = 0 To .Rows.Count - 1
                If Val(.Rows(i).Cells(4).Value) <> 0 Then
                    TotDr = TotDr + Val(.Rows(i).Cells(2).Value)
                    TotCr = TotCr + Val(.Rows(i).Cells(3).Value)
                    ' TotBal = TotBal + Val(.Rows(i).Cells(4).Value)
                End If
            Next

        End With

        With dgv_Details_Total
            If .RowCount = 0 Then .Rows.Add()
            .Rows(0).Cells(2).Value = Format(Val(TotDr), "##########0.00")
            .Rows(0).Cells(3).Value = Format(Val(TotCr), "########0.00")
            .Rows(0).Cells(4).Value = Format(Val(TotDr - TotCr), "########0.00")


        End With

    End Sub
    Private Sub Check_Delete_Status(ByVal Led_id As Integer)
        Dim cmd As New SqlClient.SqlCommand
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim Nr As Integer = 0
        Dim Temp As String = ""


        Da = New SqlClient.SqlDataAdapter("select A.Voucher_Code ,A.Print_Status from Single_Ledger_Print_Details a WHERE A.Voucher_Code NOT IN (SELECT B.Name7 FROM  " & Trim(Common_Procedures.ReportTempTable) & " B  where B.Name7 <> '' AND B.INT6 =" & Val(Led_id) & "  ) AND A.Ledger_IdNo = " & Val(Led_id) & "  ", con)
        Dt = New DataTable
        Da.Fill(Dt)

        If Dt.Rows.Count > 0 Then

            cmd.Connection = con

            For i = 0 To Dt.Rows.Count - 1

                If Val(Dt.Rows(i).Item("Print_Status")) <> 0 Then
                    cmd.CommandText = "Update Single_Ledger_Print_Details SET  Voucher_Code = '' ,Particulars = 'no record found' ,Debit_Amount  = 0 ,Credit_Amount  = 0     ,  Balance_Amount =''  Where Voucher_Code = '" & Trim(Dt.Rows(i).Item("Voucher_Code").ToString) & "' and  Ledger_IdNo =" & Val(Led_id)
                    cmd.ExecuteNonQuery()
                Else
                    cmd.CommandText = "Delete from Single_Ledger_Print_Details WHERE Voucher_Code = '" & Trim(Dt.Rows(i).Item("Voucher_Code").ToString) & "' "
                    cmd.ExecuteNonQuery()

                End If

            Next


        End If





    End Sub
    Private Function Get_Record_Count(ByVal Led_id As Integer) As Integer
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim Count As Integer = 0


        Da = New SqlClient.SqlDataAdapter("select * from Single_Ledger_Print_Details a WHERE  A.Ledger_IdNo = " & Val(Led_id) & "  ", con)
        Dt = New DataTable
        Da.Fill(Dt)
        If Dt.Rows.Count > 0 Then
            Count = Dt.Rows.Count
        End If
        Dt.Dispose()
        Da.Dispose()

        Get_Record_Count = Count


    End Function
    Private Function Get_Page_No(ByVal Led_Name As String) As Integer
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim PageNo As Integer = 0
        Dim Led_ID As Integer = Val(Common_Procedures.Ledger_NameToIdNo(con, Trim(Led_Name)))

        Da = New SqlClient.SqlDataAdapter("select top 1 Page_No  from Single_Ledger_Print_Details a WHERE Print_Status = 0 and  A.Ledger_IdNo = " & Val(Led_ID) & "  order by a.Row_No  asc", con)
        Dt = New DataTable
        Da.Fill(Dt)

        If Dt.Rows.Count > 0 Then

            PageNo = Val(Dt.Rows(0).Item("Page_No").ToString)

        End If

        Dt.Clear()
        Dt.Dispose()
        Da.Dispose()

        Get_Page_No = PageNo


    End Function

    Private Sub Printing_Format1(ByRef e As System.Drawing.Printing.PrintPageEventArgs)
        Dim cmd As New SqlClient.SqlCommand
        Dim Da1 As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim I As Integer
        Dim pFont As Font
        Dim LMargin As Single, RMargin As Single, TMargin As Single, BMargin As Single
        Dim PrintWidth As Single, PrintHeight As Single
        Dim PageWidth As Single, PageHeight As Single
        Dim CurY As Single, TxtHgt As Single
        Dim LnAr(15) As Single, ClAr(15) As Single
        Dim ps As Printing.PaperSize
        Dim Half_Width As Single = 0
        Dim Half_Height As Single = 0
        Dim p1Font As Font
        Dim blackPen As New Pen(Color.Black, 2)

        For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
            If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
                ps = PrintDocument1.PrinterSettings.PaperSizes(I)
                PrintDocument1.DefaultPageSettings.PaperSize = ps
                PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = ps
                e.PageSettings.PaperSize = ps
                Exit For
            End If
        Next

        Half_Width = 825
        Half_Height = 1167

        'With PrintDocument1.DefaultPageSettings.Margins
        '    .Left = 20 ' 30 
        '    .Right = 40
        '    .Top = 30 ' 50 
        '    .Bottom = 40
        '    LMargin = .Left
        '    RMargin = .Right
        '    TMargin = .Top
        '    BMargin = .Bottom
        'End With

        With PrintDocument1.DefaultPageSettings.Margins
            .Left = (Half_Width - 600) / 2
            .Right = ((Half_Width - 600) / 2) + 50
            .Top = 5
            .Bottom = Half_Height - Half_Width
            LMargin = .Left
            RMargin = .Right
            TMargin = .Top
            BMargin = .Bottom
        End With


        pFont = New Font("Calibri", 8, FontStyle.Regular)

        e.Graphics.TextRenderingHint = Drawing.Text.TextRenderingHint.AntiAlias

        With PrintDocument1.DefaultPageSettings.PaperSize

            PrintWidth = .Width - RMargin - LMargin
            PrintHeight = .Height - TMargin - BMargin
            PageWidth = .Width - RMargin
            PageHeight = .Height - BMargin

        End With



        Erase LnAr
        Erase ClAr

        LnAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        ClAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}

        ClAr(1) = 80 : ClAr(2) = 220 : ClAr(3) = 70 : ClAr(4) = 70
        ClAr(5) = PageWidth - (LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4))

        TxtHgt = 19  

        Try

            CurY = CurY + TxtHgt

            e.Graphics.DrawLine(blackPen, LMargin, CurY, PageWidth, CurY)
            LnAr(1) = CurY

            CurY = CurY + TxtHgt - 15
            p1Font = New Font("Calibri", 10, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "DATE", LMargin + 15, CurY, 0, 0, p1Font)
            Common_Procedures.Print_To_PrintDocument(e, "PARTICULARS", LMargin + ClAr(1) + 15, CurY, 0, 0, p1Font)
            Common_Procedures.Print_To_PrintDocument(e, "CREDIT", LMargin + ClAr(1) + ClAr(2) + 15, CurY, 0, 0, p1Font)
            Common_Procedures.Print_To_PrintDocument(e, "DEBIT", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + 15, CurY, 0, 0, p1Font)
            Common_Procedures.Print_To_PrintDocument(e, "BALANCE", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + 15, CurY, 0, 0, p1Font)

            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(blackPen, LMargin, CurY, PageWidth, CurY)
            LnAr(2) = CurY

            prn_DetIndx = 1

            Do While prn_DetIndx <= 28

                CurY = CurY + TxtHgt + 5
                e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

                prn_DetIndx = prn_DetIndx + 1

            Loop

            e.Graphics.DrawLine(blackPen, LMargin, CurY, PageWidth, CurY)
            LnAr(3) = CurY

            e.Graphics.DrawLine(blackPen, LMargin + ClAr(1), LnAr(1), LMargin + ClAr(1), CurY)
            e.Graphics.DrawLine(blackPen, LMargin + ClAr(1) + ClAr(2), LnAr(1), LMargin + ClAr(1) + ClAr(2), CurY)
            e.Graphics.DrawLine(blackPen, LMargin + ClAr(1) + ClAr(2) + ClAr(3), LnAr(1), LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY)

            CurY = CurY + TxtHgt - 10
            Common_Procedures.Print_To_PrintDocument(e, "PARTY NAME : " & Trim(cbo_AccountName.Text), LMargin + 15, CurY, 0, 0, p1Font)
            Common_Procedures.Print_To_PrintDocument(e, "PAGE NO : " & Get_Page_No(Trim(cbo_AccountName.Text)), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + 10, CurY, 0, 0, p1Font)

            CurY = CurY + TxtHgt + 5
            e.Graphics.DrawLine(blackPen, LMargin, CurY, PageWidth, CurY)
            LnAr(4) = CurY

            e.Graphics.DrawLine(blackPen, LMargin, LnAr(1), LMargin, CurY)
            e.Graphics.DrawLine(blackPen, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), LnAr(1), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY)
            e.Graphics.DrawLine(blackPen, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + +ClAr(4) + ClAr(5), LnAr(1), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + +ClAr(5), CurY)
            e.Graphics.DrawLine(blackPen, PageWidth, LnAr(1), PageWidth, CurY)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        e.HasMorePages = False

    End Sub
    Private Sub Printing_Format2(ByRef e As System.Drawing.Printing.PrintPageEventArgs)
        Dim cmd As New SqlClient.SqlCommand
        Dim Da1 As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim I As Integer
        Dim pFont As Font
        Dim LMargin As Single, RMargin As Single, TMargin As Single, BMargin As Single
        Dim PrintWidth As Single, PrintHeight As Single
        Dim PageWidth As Single, PageHeight As Single
        Dim CurY As Single, TxtHgt As Single
        Dim LnAr(15) As Single, ClAr(15) As Single
        Dim ps As Printing.PaperSize
        Dim strHeight As Single = 0
        Dim PpSzSTS As Boolean = False
        Dim Half_Width As Single = 0
        Dim Half_Height As Single = 0
        Dim Cur_Row As Integer = 0

        For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
            If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
                ps = PrintDocument1.PrinterSettings.PaperSizes(I)
                PrintDocument1.DefaultPageSettings.PaperSize = ps
                PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = ps
                e.PageSettings.PaperSize = ps
                Exit For
            End If
        Next

        Half_Width = 825
        Half_Height = 1167

        'With PrintDocument1.DefaultPageSettings.Margins
        '    .Left = 20 ' 30 
        '    .Right = 40
        '    .Top = 30 ' 50 
        '    .Bottom = 40
        '    LMargin = .Left
        '    RMargin = .Right
        '    TMargin = .Top
        '    BMargin = .Bottom
        'End With

        With PrintDocument1.DefaultPageSettings.Margins
            .Left = (Half_Width - 600) / 2
            .Right = ((Half_Width - 600) / 2) + 50
            .Top = 5
            .Bottom = Half_Height - Half_Width
            LMargin = .Left
            RMargin = .Right
            TMargin = .Top
            BMargin = .Bottom
        End With


        pFont = New Font("Calibri", 10, FontStyle.Regular)

        e.Graphics.TextRenderingHint = Drawing.Text.TextRenderingHint.AntiAlias

        With PrintDocument1.DefaultPageSettings.PaperSize

            PrintWidth = .Width - RMargin - LMargin
            PrintHeight = .Height - TMargin - BMargin
            PageWidth = .Width - RMargin
            PageHeight = .Height - BMargin

        End With



        Erase LnAr
        Erase ClAr

        LnAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        ClAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}

        ClAr(1) = 80 : ClAr(2) = 220 : ClAr(3) = 70 : ClAr(4) = 70
        ClAr(5) = PageWidth - (LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4))

        TxtHgt = 19

        Try

            If prn_DetDt.Rows.Count > 0 Then

                CurY = CurY + TxtHgt
                CurY = CurY + TxtHgt - 15
                CurY = CurY + TxtHgt

                prn_DetIndx = 0

                Cur_Row = Get_Current_RowNo(Val(prn_DetDt.Rows(0).Item("Row_No").ToString), 28)

                For I = 1 To Cur_Row - 1
                    CurY = CurY + TxtHgt + 6
                Next

                Do While prn_DetIndx <= prn_DetDt.Rows.Count - 1

                    If Cur_Row > 28 Then
                        Exit Do
                    Else
                        Cur_Row = Cur_Row + 1
                    End If

                    Common_Procedures.Print_To_PrintDocument(e, Format(prn_DetDt.Rows(prn_DetIndx).Item("Voucher_Date"), "dd/MM/yyyy"), LMargin + 10, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Particulars").ToString, LMargin + ClAr(1) + 10, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, IIf(Val(prn_DetDt.Rows(prn_DetIndx).Item("Credit_Amount").ToString) <> 0, Val(prn_DetDt.Rows(prn_DetIndx).Item("Credit_Amount").ToString), ""), LMargin + ClAr(1) + ClAr(2) + ClAr(3) - 5, CurY, 1, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, IIf(Val(prn_DetDt.Rows(prn_DetIndx).Item("Debit_Amount").ToString) <> 0, Val(prn_DetDt.Rows(prn_DetIndx).Item("Debit_Amount").ToString), ""), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) - 5, CurY, 1, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Balance_Amount").ToString, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 5, CurY, 1, 0, pFont)

                    If Common_Procedures.Print_OR_Preview_Status = 1 Then

                        '----Update Print Status
                        Save_Print_Status(Trim(prn_DetDt.Rows(prn_DetIndx).Item("Voucher_Code").ToString), Val(prn_DetDt.Rows(prn_DetIndx).Item("Ledger_IdNo").ToString), Val(prn_DetDt.Rows(prn_DetIndx).Item("Company_Idno").ToString))
                    End If

                    CurY = CurY + TxtHgt + 5

                    prn_DetIndx = prn_DetIndx + 1

                Loop

            End If

            '---Move Next Sales Party
            Move_Next()




        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        e.HasMorePages = False

    End Sub

  

    

    Private Sub PrintDocument1_BeginPrint(ByVal sender As Object, ByVal e As System.Drawing.Printing.PrintEventArgs) Handles PrintDocument1.BeginPrint
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim cmd As New SqlClient.SqlCommand
        Dim Ledger_ID As Integer = 0
        Dim W1 As Single = 0

        Ledger_ID = Val(Common_Procedures.Ledger_NameToIdNo(con, Trim(cbo_AccountName.Text)))

        prn_HdDt.Clear()
        prn_DetDt.Clear()
        prn_DetDt_sub.Clear()
        prn_DetIndx = 0
        prn_DetSNo = 0
        prn_PageNo = 0
        prn_Count = 0

     

        Try
           

            If Ledger_ID <> 0 Then
                da2 = New SqlClient.SqlDataAdapter("select  a.*  from Single_Ledger_Print_Details a where  a.Ledger_IdNo = " & Val(Ledger_ID) & " and  a.Company_Idno = " & Val(lbl_Company.Tag) & " and a.Print_Status = 0   order by a.Row_No asc     ", con)
                prn_DetDt = New DataTable
                da2.Fill(prn_DetDt)
            Else
                MessageBox.Show("Invalid Account Name", "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Exit Sub 
            End If

            da1.Dispose()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub PrintDocument1_PrintPage(ByVal sender As System.Object, ByVal e As System.Drawing.Printing.PrintPageEventArgs) Handles PrintDocument1.PrintPage

        'If prn_HdDt.Rows.Count <= 0 Then Exit Sub

        If Print_Format = "FORMAT-1" Then
            Printing_Format1(e)
        Else
            Printing_Format2(e)
        End If

    End Sub

   
    Private Sub btn_EmptyPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        pnl_Back.Enabled = False
        pnl_Print.Visible = True
        Common_Procedures.Print_OR_Preview_Status = 1
    End Sub

    Private Sub btn_Print_Bale_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Print_Empty.Click
        Printing_Btn_Status = True
        Print_Format = "FORMAT-1"
        Print_PDF_Status = False
        'print_record()
        Printing_Invoice()
        btn_Close_Print_Click(sender, e)

    End Sub

    Private Sub btn_Print_Invoice_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Print_Entry.Click

        Printing_Btn_Status = True
        Print_Format = "FORMAT-2"
        Print_PDF_Status = False
        '    print_record()
        Printing_Invoice()
        btn_Close_Print_Click(sender, e)

    End Sub

    Private Sub btn_Close_Print_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Close_Print.Click
        pnl_Print.Visible = False
        pnl_Back.Enabled = True
    End Sub

    Private Sub btn_Print_Cancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Print_Cancel.Click
        pnl_Print.Visible = False
        pnl_Back.Enabled = True
    End Sub

    Private Function Get_Current_RowNo(ByVal Row_No As Integer, ByVal Row_Count As Integer) As Integer
        Dim Current_Row As Integer = 0

        If Row_No <= Row_Count Then
            Get_Current_RowNo = Row_No
            Exit Function
        End If

        Current_Row = Row_No / Row_Count

        Current_Row = Current_Row * Row_Count

        Current_Row = Row_No - Current_Row

        Get_Current_RowNo = Current_Row

    End Function


    Private Sub btn_Print_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Print.Click

        If MessageBox.Show("Check and Set Paper Position Correctly...", "READY TO PRINT", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) = vbOK Then
            pnl_Back.Enabled = False
            pnl_Print.Visible = True
            Common_Procedures.Print_OR_Preview_Status = 1
            lbl_PrintPanel_Caption.Text = "PRINT OPTION"
        Else
            Move_Next()
            Exit Sub
        End If


    End Sub

    Private Sub btn_Close_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Close.Click
        Me.Close()
    End Sub

    Private Sub Save_Print_Status(ByVal Vou_Code As String, ByVal Led_Idno As Integer, ByVal Comp_Idno As Integer)
        Dim cmd As New SqlClient.SqlCommand
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim Nr As Integer = 0
      
        cmd.Connection = con

        cmd.CommandText = "Update Single_Ledger_Print_Details SET Print_Status = 1  Where Voucher_Code = '" & Trim(Vou_Code) & "' and  Ledger_IdNo =" & Val(Led_Idno) & "   and Company_Idno =  " & Val(Comp_Idno) & ""
        Nr = cmd.ExecuteNonQuery()

        cmd.Dispose()


    End Sub

    Private Sub Move_Next()
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim Led_ID As Integer = Val(Common_Procedures.Ledger_NameToIdNo(con, Trim(cbo_AccountName.Text)))

        Da = New SqlClient.SqlDataAdapter("select top 1 A.Ledger_IdNo ,B.Ledger_Name from Single_Ledger_Print_Details a LEFT OUTER JOIN Ledger_Head B ON A.Ledger_IdNo = B.Ledger_IdNo WHERE  A.Ledger_IdNo <> " & Val(Led_ID) & " and A.Ledger_IdNo > " & Val(Led_ID) & " order by a.Ledger_IdNo  asc", con)
        Dt = New DataTable
        Da.Fill(Dt)

        If Dt.Rows.Count > 0 Then
            cbo_AccountName.Text = Trim(Dt.Rows(0).Item("Ledger_Name").ToString)
            Get_VoucherDetails(Val(Dt.Rows(0).Item("Ledger_IdNo").ToString))

        Else
            Get_VoucherDetails(Led_ID)
        End If

        Dt.Clear()
        Dt.Dispose()
        Da.Dispose()


    End Sub

End Class