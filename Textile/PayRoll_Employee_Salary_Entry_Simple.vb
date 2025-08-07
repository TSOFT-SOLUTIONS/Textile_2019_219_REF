Public Class PayRoll_Employee_Salary_Entry_Simple
    Implements Interface_MDIActions

    Private con As New SqlClient.SqlConnection(Common_Procedures.Connection_String)
    Private FrmLdSTS As Boolean = False
    Private New_Entry As Boolean = False
    Private Insert_Entry As Boolean = False
    Private Filter_Status As Boolean = False
    Private Pk_Condition As String = "EMSAL-"
    Private Pk_Condition2 As String = "ADVLS-"
    Private NoCalc_Status As Boolean = False
    Private Prec_ActCtrl As New Control
    Private vCbo_ItmNm As String
    Private vcbo_KeyDwnVal As Double

    Private WithEvents dgtxt_Details As New DataGridViewTextBoxEditingControl

    Private prn_HdDt As New DataTable
    Private prn_DetDt As New DataTable
    Private prn_PageNo As Integer
    Private prn_DetIndx As Integer
    Private prn_DetAr(50, 10) As String
    Private prn_DetMxIndx As Integer
    Private prn_NoofBmDets As Integer
    Private prn_DetSNo As Integer

    Public Enum dgvcol_SalDetails As Integer

        SlNo    '0
        Employee_Name   '1
        Sal_day     '2
        Att_day         '3
        Basic_Salary    '4
        Ot_Hrs          '5
        Ot_SalHr        '6  
        OT_Sal          '7
        ESI      '8
        PF       '9
        Incen           '10
        Tot_sal           '11
        Mess            '12
        Net_Sal         '13
        Earnings        '14
        Tot_Adv         '15
        Less_adv        '16
        BalAdv          '17
        Salary_Adv      '18
        Salary_pen      '19
        Net_pay         '20
        Ot_Minutes      '21


    End Enum

    Public Sub New()
        FrmLdSTS = True
        ' This call is required by the designer.
        InitializeComponent()
        ' Add any initialization after the InitializeComponent() call.
        clear()
    End Sub

    Private Sub clear()

        NoCalc_Status = True

        New_Entry = False
        Insert_Entry = False

        pnl_Back.Enabled = True
        pnl_Filter.Visible = False

        lbl_RefNo.Text = ""
        lbl_RefNo.ForeColor = Color.Black

        dtp_Date.Text = ""
        cbo_PaymentType.Text = Common_Procedures.Salary_PaymentType_IdNoToName(con, 1)
        Dtp_FromDate.Text = ""
        cbo_Month.Text = ""
        Dtp_ToDate.Text = ""
        txt_FestivalDays.Text = ""
        txt_TotalDays.Text = ""

        cbo_Category.Text = ""

        dtp_Advance_FromDate.Text = ""
        dtp_Advance_ToDate.Text = ""

        dgv_Details.Rows.Clear()

        If Filter_Status = False Then
            dtp_Filter_Fromdate.Text = Common_Procedures.Company_FromDate
            dtp_Filter_ToDate.Text = Common_Procedures.Company_ToDate
            cbo_Filter_PartyName.Text = ""

            cbo_Filter_PartyName.SelectedIndex = -1
            dgv_Filter_Details.Rows.Clear()
        End If

        Grid_Cell_DeSelect()

        NoCalc_Status = False

    End Sub

    Private Sub ControlGotFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim txtbx As TextBox
        Dim combobx As ComboBox
        Dim mskdat As MaskedTextBox

        On Error Resume Next

        If TypeOf Me.ActiveControl Is TextBox Or TypeOf Me.ActiveControl Is ComboBox Or TypeOf Me.ActiveControl Is Button Or TypeOf Me.ActiveControl Is MaskedTextBox Then
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
            mskdat = Me.ActiveControl
            mskdat.SelectAll()
        End If

        Grid_Cell_DeSelect()

        Prec_ActCtrl = Me.ActiveControl

    End Sub

    Private Sub ControlLostFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        On Error Resume Next
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
        If Not IsNothing(dgv_Details.CurrentCell) Then dgv_Details.CurrentCell.Selected = False
        If Not IsNothing(dgv_Filter_Details.CurrentCell) Then dgv_Filter_Details.CurrentCell.Selected = False
    End Sub

    Private Sub move_record(ByVal no As String)
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim NewCode As String
        Dim n As Integer
        Dim SNo As Integer

        If Val(no) = 0 Then Exit Sub

        clear()

        NoCalc_Status = True

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(no) & "/" & Trim(Common_Procedures.FnYearCode)

        Try

            da1 = New SqlClient.SqlDataAdapter("select a.* from PayRoll_Salary_Head a Where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Salary_Code = '" & Trim(NewCode) & "'", con)
            dt1 = New DataTable
            da1.Fill(dt1)

            If dt1.Rows.Count > 0 Then

                lbl_RefNo.Text = dt1.Rows(0).Item("Salary_No").ToString
                dtp_Date.Text = dt1.Rows(0).Item("Salary_Date").ToString
                cbo_PaymentType.Text = Common_Procedures.Salary_PaymentType_IdNoToName(con, Val(dt1.Rows(0).Item("Salary_Payment_Type_IdNo").ToString))
                cbo_Month.Text = Common_Procedures.Month_IdNoToName(con, Val(dt1.Rows(0).Item("Month_IdNo").ToString))
                Dtp_FromDate.Text = dt1.Rows(0).Item("From_Date").ToString
                dtp_ToDate.Text = dt1.Rows(0).Item("To_Date").ToString
                dtp_Advance_UpToDate.Text = dt1.Rows(0).Item("Advance_UptoDate").ToString
                txt_TotalDays.Text = Val(dt1.Rows(0).Item("Total_Days").ToString)
                txt_FestivalDays.Text = Val(dt1.Rows(0).Item("Festival_Days").ToString)
                cbo_Category.Text = Common_Procedures.Category_IdNoToName(con, Val(dt1.Rows(0).Item("Category_IdNo").ToString))

                dtp_Advance_FromDate.Text = dt1.Rows(0).Item("Advance_FromDate").ToString
                dtp_Advance_ToDate.Text = dt1.Rows(0).Item("Advance_ToDate").ToString

                da2 = New SqlClient.SqlDataAdapter("Select a.*, b.Employee_Name from PayRoll_Salary_Details a INNER JOIN PayRoll_Employee_Head b ON a.Employee_IdNo = b.Employee_IdNo  Where a.Salary_Code = '" & Trim(NewCode) & "' Order by a.sl_no", con)
                dt2 = New DataTable
                da2.Fill(dt2)

                With dgv_Details

                    .Rows.Clear()
                    SNo = 0

                    If dt2.Rows.Count > 0 Then

                        For i = 0 To dt2.Rows.Count - 1

                            n = .Rows.Add()

                            SNo = SNo + 1

                            .Rows(n).Cells(dgvcol_SalDetails.SlNo).Value = Val(SNo)
                            .Rows(n).Cells(dgvcol_SalDetails.Employee_Name).Value = dt2.Rows(i).Item("Employee_Name").ToString

                            .Rows(n).Cells(dgvcol_SalDetails.Sal_day).Value = Val(dt2.Rows(i).Item("Salary_Shift").ToString)
                            If Val(.Rows(n).Cells(dgvcol_SalDetails.Sal_day).Value) = 0 Then .Rows(n).Cells(dgvcol_SalDetails.Sal_day).Value = ""

                            .Rows(n).Cells(dgvcol_SalDetails.Att_day).Value = Val(dt2.Rows(i).Item("No_Of_Attendance_Days").ToString)
                            If Val(.Rows(n).Cells(dgvcol_SalDetails.Att_day).Value) = 0 Then .Rows(n).Cells(dgvcol_SalDetails.Att_day).Value = ""

                            .Rows(n).Cells(dgvcol_SalDetails.Basic_Salary).Value = Val(dt2.Rows(i).Item("Basic_Salary").ToString)
                            If Val(.Rows(n).Cells(dgvcol_SalDetails.Basic_Salary).Value) = 0 Then .Rows(n).Cells(dgvcol_SalDetails.Basic_Salary).Value = ""

                            .Rows(n).Cells(dgvcol_SalDetails.Ot_Hrs).Value = Val(dt2.Rows(i).Item("Ot_Hours").ToString)
                            If Val(.Rows(n).Cells(dgvcol_SalDetails.Ot_Hrs).Value) = 0 Then .Rows(n).Cells(dgvcol_SalDetails.Ot_Hrs).Value = ""

                            .Rows(n).Cells(dgvcol_SalDetails.Ot_SalHr).Value = Val(dt2.Rows(i).Item("Ot_Pay_Hours").ToString)
                            If Val(.Rows(n).Cells(dgvcol_SalDetails.Ot_SalHr).Value) = 0 Then .Rows(n).Cells(dgvcol_SalDetails.Ot_SalHr).Value = ""

                            .Rows(n).Cells(dgvcol_SalDetails.OT_Sal).Value = Val(dt2.Rows(i).Item("Ot_Salary").ToString)
                            If Val(.Rows(n).Cells(dgvcol_SalDetails.OT_Sal).Value) = 0 Then .Rows(n).Cells(dgvcol_SalDetails.OT_Sal).Value = ""

                            .Rows(n).Cells(dgvcol_SalDetails.Incen).Value = Val(dt2.Rows(i).Item("Incentive_Amount").ToString)
                            If Val(.Rows(n).Cells(dgvcol_SalDetails.Incen).Value) = 0 Then .Rows(n).Cells(dgvcol_SalDetails.Incen).Value = ""

                            .Rows(n).Cells(dgvcol_SalDetails.Tot_sal).Value = Val(dt2.Rows(i).Item("Total_Salary").ToString)
                            If Val(.Rows(n).Cells(dgvcol_SalDetails.Tot_sal).Value) = 0 Then .Rows(n).Cells(dgvcol_SalDetails.Tot_sal).Value = ""

                            .Rows(n).Cells(dgvcol_SalDetails.Mess).Value = Val(dt2.Rows(i).Item("Mess").ToString)
                            If Val(.Rows(n).Cells(dgvcol_SalDetails.Mess).Value) = 0 Then .Rows(n).Cells(dgvcol_SalDetails.Mess).Value = ""

                            .Rows(n).Cells(dgvcol_SalDetails.Net_Sal).Value = Format(Val(dt2.Rows(i).Item("Net_Salary").ToString), "#########0.00")
                            If Val(.Rows(n).Cells(dgvcol_SalDetails.Net_Sal).Value) = 0 Then
                                If Val(.Rows(n).Cells(dgvcol_SalDetails.Mess).Value) = 0 Then
                                    .Rows(n).Cells(dgvcol_SalDetails.Net_Sal).Value = ""
                                End If
                            End If

                            .Rows(n).Cells(dgvcol_SalDetails.Tot_Adv).Value = Val(dt2.Rows(i).Item("Advance").ToString)
                            If Val(.Rows(n).Cells(dgvcol_SalDetails.Tot_Adv).Value) = 0 Then .Rows(n).Cells(dgvcol_SalDetails.Tot_Adv).Value = ""


                            .Rows(n).Cells(dgvcol_SalDetails.Less_adv).Value = Format(Val(dt2.Rows(i).Item("Minus_MainAdvance").ToString), "#########0.00")
                            If Val(.Rows(n).Cells(dgvcol_SalDetails.Less_adv).Value) = 0 Then .Rows(n).Cells(dgvcol_SalDetails.Less_adv).Value = ""


                            .Rows(n).Cells(dgvcol_SalDetails.BalAdv).Value = Val(dt2.Rows(i).Item("Balance_Advance").ToString)
                            If Val(.Rows(n).Cells(dgvcol_SalDetails.BalAdv).Value) = 0 Then
                                If Val(.Rows(n).Cells(dgvcol_SalDetails.Tot_Adv).Value) = 0 Then
                                    .Rows(n).Cells(dgvcol_SalDetails.BalAdv).Value = ""
                                End If
                            End If
                            If Val(.Rows(n).Cells(dgvcol_SalDetails.BalAdv).Value) = 0 Then .Rows(n).Cells(dgvcol_SalDetails.BalAdv).Value = ""

                            .Rows(n).Cells(dgvcol_SalDetails.Salary_Adv).Value = Val(dt2.Rows(i).Item("Minus_Advance").ToString)
                            If Val(.Rows(n).Cells(dgvcol_SalDetails.Salary_Adv).Value) = 0 Then .Rows(n).Cells(dgvcol_SalDetails.Salary_Adv).Value = ""

                            .Rows(n).Cells(dgvcol_SalDetails.Salary_pen).Value = Val(dt2.Rows(i).Item("Salary_Pending").ToString)
                            If Val(.Rows(n).Cells(dgvcol_SalDetails.Salary_pen).Value) = 0 Then .Rows(n).Cells(dgvcol_SalDetails.Salary_pen).Value = ""

                            .Rows(n).Cells(dgvcol_SalDetails.Net_pay).Value = Format(Val(dt2.Rows(i).Item("Net_Pay_Amount").ToString), "#########0.00")
                            If Val(.Rows(n).Cells(dgvcol_SalDetails.Net_pay).Value) = 0 Then
                                If Val(.Rows(n).Cells(dgvcol_SalDetails.Salary_Adv).Value) = 0 Then
                                    .Rows(n).Cells(dgvcol_SalDetails.Net_pay).Value = ""
                                End If
                            End If

                            .Rows(n).Cells(dgvcol_SalDetails.Ot_Minutes).Value = Val(dt2.Rows(i).Item("OT_Minutes").ToString)
                            If Val(.Rows(n).Cells(dgvcol_SalDetails.Ot_Minutes).Value) = 0 Then .Rows(n).Cells(dgvcol_SalDetails.Ot_Minutes).Value = ""


                            .Rows(n).Cells(dgvcol_SalDetails.Earnings).Value = Format(Val(dt2.Rows(i).Item("Earning").ToString), "##########0.00")
                            If Val(.Rows(n).Cells(dgvcol_SalDetails.Earnings).Value) = 0 Then .Rows(n).Cells(dgvcol_SalDetails.Earnings).Value = ""

                            .Rows(n).Cells(dgvcol_SalDetails.PF).Value = Format(Val(dt2.Rows(i).Item("P_F").ToString), "##########0.00")
                            If Val(.Rows(n).Cells(dgvcol_SalDetails.PF).Value) = 0 Then .Rows(n).Cells(dgvcol_SalDetails.PF).Value = ""


                            .Rows(n).Cells(dgvcol_SalDetails.ESI).Value = Format(Val(dt2.Rows(i).Item("ESI").ToString), "##########0.00")
                            If Val(.Rows(n).Cells(dgvcol_SalDetails.ESI).Value) = 0 Then .Rows(n).Cells(dgvcol_SalDetails.ESI).Value = ""



                        Next i

                    End If

                    If .RowCount = 0 Then .Rows.Add()

                End With

            End If

            Grid_Cell_DeSelect()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT MOVE RECORDS...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Finally

            dt1.Dispose()
            da1.Dispose()

            dt2.Dispose()
            da2.Dispose()

            If dtp_Date.Visible And dtp_Date.Enabled Then dtp_Date.Focus()

        End Try

        NoCalc_Status = False
    End Sub

    Private Sub get_PayRoll_Salary_Details()
        Dim cmd As New SqlClient.SqlCommand
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim da3 As New SqlClient.SqlDataAdapter
        Dim da4 As New SqlClient.SqlDataAdapter
        Dim da5 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim dt3 As New DataTable
        Dim dt4 As New DataTable
        Dim dt5 As New DataTable

        Dim n As Integer = 0
        Dim Mess_wrk_dys As Integer = 0
        Dim wrk_dys As Single = 0
        Dim OT_wrk_dys As Double = 0
        Dim Incen As String = 0
        Dim Salary As Double = 0
        Dim Sal_Shft As Double = 0
        Dim Bas_Sal As Double = 0
        Dim OT_Sal_Shft As Double = 0
        Dim OT_Salary As Double = 0
        Dim Amt_OpBal As String = 0
        Dim Cmp_Cond As String = ""
        Dim mins_Adv As Double = 0
        Dim mess_Ded As Double = 0
        Dim OT_Mins As Integer = 0
        Dim Ot_Dbl As Double = 0
        Dim Ot_Int As Integer = 0
        Dim Ot_minVal As Integer = 0
        Dim Net_Salary As Double = 0
        Dim Net_Pay As Double = 0
        Dim Salary_Pending As Double = 0
        Dim SNo As Integer = 0
        Dim SalPymtTyp_IdNo As Integer = 0
        Dim PrevEnt_RefNo As String = ""
        Dim EntOrdBy As Single = 0, PrevEnt_OrdBy As Single = 0
        Dim AdvDtTm As Date
        Dim NewCode As String = ""
        Dim Mess_From_Dedution_Entry As Single = 0

        Dim vPFSTS_Sal As Integer = 0
        Dim vESISTS_Sal As Integer = 0
        Dim vCatgry_IdNo As Integer = 0
        Dim vSQLCondt As String = ""

        If FrmLdSTS = True Then Exit Sub

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        cmd.Connection = con



        SalPymtTyp_IdNo = Common_Procedures.Salary_PaymentType_NameToIdNo(con, cbo_PaymentType.Text)

        vCatgry_IdNo = Common_Procedures.Category_NameToIdNo(con, cbo_Category.Text)

        If Val(vCatgry_IdNo) = 0 Then
            MessageBox.Show("Invalid Category", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If cbo_Category.Enabled And cbo_Category.Visible Then cbo_Category.Focus()
            Exit Sub
        End If

        EntOrdBy = Val(Common_Procedures.OrderBy_CodeToValue(Trim(lbl_RefNo.Text)))

        vSQLCondt = ""
        vSQLCondt = "(a.company_idno = 0 or a.company_idno = " & Str(Val(lbl_Company.Tag)) & ")"

        If Val(vCatgry_IdNo) <> 0 Then
            vSQLCondt = Trim(vSQLCondt) & IIf(Trim(vSQLCondt) <> "", " and ", "") & " a.Category_IdNo = " & Str(Val(vCatgry_IdNo))
        End If


        cmd.Parameters.Clear()
        cmd.Parameters.AddWithValue("@CompFromDate", Common_Procedures.Company_FromDate)
        cmd.Parameters.AddWithValue("@SalaryDate", dtp_Date.Value.Date)
        cmd.Parameters.AddWithValue("@FromDate", dtp_FromDate.Value.Date)
        cmd.Parameters.AddWithValue("@ToDate", dtp_ToDate.Value.Date)




        cmd.CommandText = "select a.Employee_Name, a.Employee_IdNo, a.Shift_Day_Month, b.No_Days_Month_Wages , a.Pf_Salary , a.Esi_Salary from PayRoll_Employee_Head a LEFT OUTER JOIN PayRoll_Category_Head b ON a.Category_IdNo <> 0 and a.Category_IdNo = b.Category_IdNo  Where " & vSQLCondt & IIf(vSQLCondt <> "", " and ", "") & " a.Salary_Payment_Type_IdNo = " & Str(Val(SalPymtTyp_IdNo)) & " and a.Join_DateTime <= @ToDate and (a.Date_Status = 0 or (a.Date_Status = 1 and a.Releave_DateTime >= @FromDate ) ) order by a.Employee_Name"
        da1 = New SqlClient.SqlDataAdapter(cmd)
        dt1 = New DataTable
        da1.Fill(dt1)

        With dgv_Details

            .Rows.Clear()
            SNo = 0

            If dt1.Rows.Count > 0 Then

                For i = 0 To dt1.Rows.Count - 1

                    cmd.Parameters.Clear()
                    cmd.Parameters.AddWithValue("@CompFromDate", Common_Procedures.Company_FromDate)
                    cmd.Parameters.AddWithValue("@SalaryDate", dtp_Date.Value.Date)
                    cmd.Parameters.AddWithValue("@FromDate", dtp_FromDate.Value.Date)
                    cmd.Parameters.AddWithValue("@ToDate", dtp_ToDate.Value.Date)
                    If dtp_Advance_UpToDate.Visible = True Then
                        cmd.Parameters.AddWithValue("@AdvanceUpToDate", dtp_Advance_UpToDate.Value.Date)
                    Else
                        cmd.Parameters.AddWithValue("@AdvanceUpToDate", dtp_ToDate.Value.Date)
                    End If

                    If dtp_Advance_ToDate.Visible = True And dtp_Advance_FromDate.Visible = True Then
                        cmd.Parameters.AddWithValue("@AdvanceFromDate", dtp_Advance_FromDate.Value.Date)
                        cmd.Parameters.AddWithValue("@AdvanceToDate", dtp_Advance_ToDate.Value.Date)
                    Else
                        cmd.Parameters.AddWithValue("@AdvanceFromDate", dtp_FromDate.Value.Date)
                        cmd.Parameters.AddWithValue("@AdvanceToDate", dtp_ToDate.Value.Date)
                    End If

                    n = dgv_Details.Rows.Add()

                    SNo = SNo + 1

                    Amt_OpBal = 0 : mins_Adv = 0

                    'Old
                    'cmd.CommandText = "select sum(a.voucher_amount) from voucher_details a, ledger_head b, Company_Head tZ Where a.Ledger_IdNo = " & Str(Val(dt1.Rows(i).Item("Employee_IdNo").ToString)) & " and a.voucher_date <= @AdvanceUpToDate and a.Entry_Identification <> '" & Trim(Pk_Condition) & Trim(Val(dt1.Rows(i).Item("Employee_IdNo").ToString)) & "/" & Trim(NewCode) & "' and a.Entry_Identification <> '" & Trim(Pk_Condition2) & Trim(Val(dt1.Rows(i).Item("Employee_IdNo").ToString)) & "/" & Trim(NewCode) & "' and a.ledger_idno = b.ledger_idno and b.parent_code NOT LIKE '%~18~' and a.company_idno = tZ.company_idno and (a.Voucher_Code LIKE 'ADVOP-%' or a.Voucher_Code LIKE 'EADPY-%' or a.Voucher_Code LIKE 'ADVLS-%') "

                    If Trim(Common_Procedures.settings.CustomerCode) = "1087" Then

                        cmd.CommandText = "select abs(sum(a.AMount)) as VouAMt from PayRoll_Employee_Payment_Head a Inner join  Company_Head tZ ON a.company_idno = tZ.company_idno  Where a.Employee_Idno =  " & Str(Val(dt1.Rows(i).Item("Employee_IdNo").ToString)) & " and a.Employee_Payment_Date Between @AdvanceFromDate and @AdvanceToDate and  Advance_Salary = 'ADVANCE' "
                        Da = New SqlClient.SqlDataAdapter(cmd)
                        dt4 = New DataTable
                        Da.Fill(dt4)

                    Else
                        cmd.CommandText = "select abs(sum(a.voucher_amount)) as VouAMt from voucher_details a Inner join  Company_Head tZ ON a.company_idno = tZ.company_idno Inner Join Ledger_Head b On a.ledger_idno = b.ledger_idno Where a.Ledger_IdNo =  " & Str(Val(dt1.Rows(i).Item("Employee_IdNo").ToString)) & " and a.voucher_date <= @AdvanceUpToDate and (a.Voucher_Code LIKE 'EADPY-%' or a.Voucher_Code LIKE 'ESAPY-%' or a.Voucher_Code LIKE 'ESLPY-%')"
                        Da = New SqlClient.SqlDataAdapter(cmd)
                        dt4 = New DataTable
                        Da.Fill(dt4)
                    End If


                    If dt4.Rows.Count > 0 Then
                        'If IsDBNull(dt4.Rows(0)(0).ToString) = False Then
                        '    If Val(dt4.Rows(0)(0).ToString) < 0 Then
                        '        Amt_OpBal = Format(-1 * Val(dt4.Rows(0)(0).ToString), "##########0.00")
                        '    End If
                        'End If
                        Amt_OpBal = dt4.Rows(0).Item("VouAMt").ToString()
                    End If
                    dt4.Clear()

                    If Val(dt1.Rows(i).Item("Employee_IdNo").ToString) = 556 Then
                        Debug.Print(dt1.Rows(i).Item("Employee_Name").ToString)
                    End If

                    AdvDtTm = #1/1/1990#
                    cmd.CommandText = "Select b.Advance_UptoDate from PayRoll_Salary_Details a INNER JOIN PayRoll_Salary_Head b ON a.Salary_Code = b.Salary_Code Where a.Employee_IdNo = " & Str(Val(dt1.Rows(i).Item("Employee_IdNo").ToString)) & " and (a.Salary_Date < @SalaryDate or (a.Salary_Date = @SalaryDate and a.for_OrderBy < " & Str(Val(EntOrdBy)) & ") ) Order by a.Salary_Date desc, a.for_OrderBy desc, b.Advance_UptoDate desc "
                    Da = New SqlClient.SqlDataAdapter(cmd)
                    dt4 = New DataTable
                    Da.Fill(dt4)
                    If dt4.Rows.Count > 0 Then
                        If IsDBNull(dt4.Rows(0)(0).ToString) = False Then
                            If IsDate(dt4.Rows(0)(0).ToString) = True Then
                                AdvDtTm = dt4.Rows(0)(0)
                            End If
                        End If
                    End If
                    dt4.Clear()

                    AdvDtTm = DateAdd(DateInterval.Day, 1, AdvDtTm)
                    cmd.Parameters.AddWithValue("@PreviousAdvanceDate", AdvDtTm)

                    cmd.CommandText = "Select sum(a.Voucher_Amount) as Sal_Advance from Voucher_Details a INNER JOIN Company_Head tZ ON a.Company_IdNo = tZ.Company_IdNo Where a.Ledger_IdNo = " & Str(Val(dt1.Rows(i).Item("Employee_IdNo").ToString)) & " and a.Voucher_Date between @PreviousAdvanceDate and @AdvanceUpToDate and a.Voucher_Amount < 0 and a.Entry_Identification LIKE 'ESAPY-%'"
                    Da = New SqlClient.SqlDataAdapter(cmd)
                    dt4 = New DataTable
                    Da.Fill(dt4)
                    If dt4.Rows.Count > 0 Then
                        If IsDBNull(dt4.Rows(0)(0).ToString) = False Then
                            If Val(dt4.Rows(0).Item("Sal_Advance").ToString) < 0 Then
                                mins_Adv = Format(Math.Abs(Val(dt4.Rows(0).Item("Sal_Advance").ToString)), "##########0.00")
                            End If
                        End If
                    End If
                    dt4.Clear()

                    Salary_Pending = 0

                    cmd.CommandText = "Select sum(a.Voucher_Amount) as Sal_Pending from Voucher_Details a INNER JOIN Company_Head tZ ON a.Company_IdNo <>0 and a.Company_IdNo = tZ.Company_IdNo LEFT OUTER JOIN Ledger_Head tP ON a.Ledger_IdNo <> 0 and a.Ledger_IdNo = tP.Ledger_IdNo Where a.Ledger_IdNo = " & Str(Val(dt1.Rows(i).Item("Employee_IdNo").ToString)) & " and a.Voucher_Date < @PreviousAdvanceDate and  (a.Voucher_Code NOT LIKE 'ADVOP-%' and a.Voucher_Code NOT LIKE 'EADPY-%' and a.Voucher_Code NOT LIKE 'ADVLS-%' ) "
                    Da = New SqlClient.SqlDataAdapter(cmd)
                    dt4 = New DataTable
                    Da.Fill(dt4)
                    If dt4.Rows.Count > 0 Then
                        If IsDBNull(dt4.Rows(0)(0).ToString) = False Then
                            'If Val(dt4.Rows(0).Item("Sal_Pending").ToString) > 0 Then
                            Salary_Pending = Format(Val(dt4.Rows(0)(0).ToString), "##########0.00")
                            'End If
                        End If
                    End If
                    dt4.Clear()

                    cmd.CommandText = "Select sum(a.Voucher_Amount) as Sal_Paid_Amt from Voucher_Details a INNER JOIN Company_Head tZ ON a.Company_IdNo = tZ.Company_IdNo LEFT OUTER JOIN Ledger_Head tP ON a.Ledger_IdNo = tP.Ledger_IdNo Where a.Ledger_IdNo = " & Str(Val(dt1.Rows(i).Item("Employee_IdNo").ToString)) & " and a.Voucher_Date between @PreviousAdvanceDate and @ToDate and a.Entry_Identification LIKE 'ESLPY-%'"
                    Da = New SqlClient.SqlDataAdapter(cmd)
                    dt4 = New DataTable
                    Da.Fill(dt4)
                    If dt4.Rows.Count > 0 Then
                        If IsDBNull(dt4.Rows(0)(0).ToString) = False Then
                            'If Val(dt4.Rows(0).Item("cr_Amt").ToString) > 0 Then
                            Salary_Pending = Salary_Pending + Format(Val(dt4.Rows(0)(0).ToString), "##########0.00")
                            'End If
                        End If
                    End If
                    dt4.Clear()

                    wrk_dys = 0
                    Incen = 0
                    OT_Mins = 0
                    Mess_wrk_dys = 0


                    cmd.CommandText = "select sum(a.No_Of_Shift) as WRKING_DAYS , sum(a.Mess_Attendance) as MESS_DAYS , Sum(a.Incentive_Amount) as Incen ,  Sum(A.OT_Minutes) as Ot_Mins from PayRoll_Employee_Attendance_Details a INNER JOIN PayRoll_Employee_Head b ON a.Employee_IdNo <> 0 and a.Employee_IdNo = B.Employee_IdNo where a.Employee_IdNo = " & Str(Val(dt1.Rows(i).Item("Employee_IdNo").ToString)) & " and Employee_Attendance_Date between @fromdate and @toDate "
                    da2 = New SqlClient.SqlDataAdapter(cmd)
                    dt2 = New DataTable
                    da2.Fill(dt2)
                    If dt2.Rows.Count > 0 Then
                        If IsDBNull(dt2.Rows(0).Item("WRKING_DAYS").ToString) = False Then
                            wrk_dys = Val(dt2.Rows(0).Item("WRKING_DAYS").ToString)
                        End If
                        If IsDBNull(dt2.Rows(0).Item("Incen").ToString) = False Then
                            Incen = Format(Val(dt2.Rows(0).Item("Incen").ToString), "########0.00")
                        End If
                        If IsDBNull(dt2.Rows(0).Item("Ot_Mins").ToString) = False Then
                            OT_Mins = Val(dt2.Rows(0).Item("Ot_Mins").ToString)
                        End If
                        If IsDBNull(dt2.Rows(0).Item("MESS_DAYS").ToString) = False Then
                            Mess_wrk_dys = Val(dt2.Rows(0).Item("MESS_DAYS").ToString)
                        End If
                    End If
                    dt2.Clear()

                    '---Mess Amount From Deduction entry
                    cmd.CommandText = "select sum(a.Mess_Amount) as Mess from PayRoll_Employee_Deduction_Head a INNER JOIN PayRoll_Employee_Head b ON a.Employee_IdNo <> 0 and a.Employee_IdNo = B.Employee_IdNo where a.Employee_IdNo = " & Str(Val(dt1.Rows(i).Item("Employee_IdNo").ToString)) & " and Employee_Deduction_Date between @fromdate and @toDate "
                    da2 = New SqlClient.SqlDataAdapter(cmd)
                    dt2 = New DataTable
                    da2.Fill(dt2)
                    If dt2.Rows.Count > 0 Then
                        If IsDBNull(dt2.Rows(0).Item("Mess").ToString) = False Then
                            Mess_From_Dedution_Entry = Val(dt2.Rows(0).Item("Mess").ToString)
                        End If

                    End If
                    dt2.Clear()

                    cmd.CommandText = "SELECT TOP 1 * from PayRoll_Employee_Salary_Details a Where a.employee_idno = " & Str(Val(dt1.Rows(i).Item("Employee_IdNo").ToString)) & " and ( ( @FromDate < (select min(y.From_DateTime) from PayRoll_Employee_Salary_Details y where y.employee_idno = a.employee_idno )) or (@FromDate BETWEEN a.From_DateTime and a.To_DateTime) or ( @FromDate >= (select max(z.From_DateTime) from PayRoll_Employee_Salary_Details z where z.employee_idno = a.employee_idno ))) order by a.From_DateTime desc"
                    da3 = New SqlClient.SqlDataAdapter(cmd)
                    dt3 = New DataTable
                    da3.Fill(dt3)

                    Salary = 0
                    OT_Sal_Shft = 0
                    OT_Salary = 0
                    mess_Ded = 0

                    If dt3.Rows.Count > 0 Then
                        If IsDBNull(dt3.Rows(0).Item("For_Salary").ToString) = False Then
                            Salary = Format(Val(dt3.Rows(0).Item("For_Salary").ToString), "########0.00")
                        End If
                        If IsDBNull(dt3.Rows(0).Item("O_T").ToString) = False Then
                            OT_Sal_Shft = Format(Val(dt3.Rows(0).Item("O_T").ToString), "########0.00")
                        End If
                        If IsDBNull(dt3.Rows(0).Item("MessDeduction").ToString) = False Then
                            mess_Ded = Format(Val(dt3.Rows(0).Item("MessDeduction").ToString), "########0.00")
                        End If
                    End If
                    dt3.Clear()


                    Sal_Shft = 0
                    If Trim(UCase(dt1.Rows(i).Item("Shift_Day_Month").ToString)) = "MONTH" Then
                        If Val(dt1.Rows(i).Item("No_Days_Month_Wages").ToString) <> 0 Then
                            Sal_Shft = Format(Salary / Val(dt1.Rows(i).Item("No_Days_Month_Wages").ToString), "########0.00")
                        Else
                            Sal_Shft = Format(Salary / 26, "########0.00")
                        End If

                    Else
                        Sal_Shft = Salary

                    End If

                    '-----------------------------------------

                    vPFSTS_Sal = 0
                    vESISTS_Sal = 0

                    vPFSTS_Sal = Val(dt1.Rows(i).Item("Pf_Salary").ToString)
                    vESISTS_Sal = Val(dt1.Rows(i).Item("Esi_Salary").ToString)




                    .Rows(n).Cells(dgvcol_SalDetails.SlNo).Value = Val(SNo)

                    .Rows(n).Cells(dgvcol_SalDetails.Employee_Name).Value = dt1.Rows(i).Item("Employee_Name").ToString

                    .Rows(n).Cells(dgvcol_SalDetails.Sal_day).Value = Val(Sal_Shft)
                    If Val(.Rows(n).Cells(dgvcol_SalDetails.Sal_day).Value) = 0 Then .Rows(n).Cells(dgvcol_SalDetails.Sal_day).Value = ""

                    .Rows(n).Cells(dgvcol_SalDetails.Att_day).Value = Val(wrk_dys)
                    If Val(.Rows(n).Cells(dgvcol_SalDetails.Att_day).Value) = 0 Then .Rows(n).Cells(dgvcol_SalDetails.Att_day).Value = ""

                    Bas_Sal = Format(wrk_dys * Sal_Shft, "#########0.00")
                    .Rows(n).Cells(dgvcol_SalDetails.Basic_Salary).Value = Val(Bas_Sal)
                    If Val(.Rows(n).Cells(dgvcol_SalDetails.Basic_Salary).Value) = 0 Then .Rows(n).Cells(dgvcol_SalDetails.Basic_Salary).Value = ""

                    Ot_Int = Int(OT_Mins / 60)
                    Ot_minVal = Ot_Int * 60
                    Ot_Dbl = (OT_Mins - Ot_minVal) / 100

                    .Rows(n).Cells(dgvcol_SalDetails.Ot_Hrs).Value = Format(Ot_Dbl + Ot_Int, "#########0.00")
                    If Val(.Rows(n).Cells(dgvcol_SalDetails.Ot_Hrs).Value) = 0 Then .Rows(n).Cells(dgvcol_SalDetails.Ot_Hrs).Value = ""

                    .Rows(n).Cells(dgvcol_SalDetails.Ot_SalHr).Value = Val(OT_Sal_Shft)
                    If Val(.Rows(n).Cells(dgvcol_SalDetails.Ot_SalHr).Value) = 0 Then .Rows(n).Cells(dgvcol_SalDetails.Ot_SalHr).Value = ""

                    OT_Salary = Format(OT_Mins * (OT_Sal_Shft / 480), "##########0.00")
                    .Rows(n).Cells(dgvcol_SalDetails.OT_Sal).Value = Val(OT_Salary)
                    If Val(.Rows(n).Cells(dgvcol_SalDetails.OT_Sal).Value) = 0 Then .Rows(n).Cells(dgvcol_SalDetails.OT_Sal).Value = ""

                    .Rows(n).Cells(dgvcol_SalDetails.Incen).Value = Val(Incen)
                    If Val(.Rows(n).Cells(dgvcol_SalDetails.Incen).Value) = 0 Then .Rows(n).Cells(dgvcol_SalDetails.Incen).Value = ""


                    '------------


                    .Rows(n).Cells(dgvcol_SalDetails.Earnings).Value = Format(Val(.Rows(n).Cells(dgvcol_SalDetails.Basic_Salary).Value) + Val(.Rows(n).Cells(dgvcol_SalDetails.OT_Sal).Value), "#######0.00")
                    If Val(.Rows(n).Cells(dgvcol_SalDetails.Earnings).Value) = 0 Then .Rows(n).Cells(dgvcol_SalDetails.Earnings).Value = ""


                    Dim cEPF_MAX_BASICPAY As String = 0
                    Dim cPF_PERC As String = 0

                    If vPFSTS_Sal = 1 Then

                        cEPF_MAX_BASICPAY = 15000
                        cPF_PERC = 12

                        If Val(.Rows(n).Cells(dgvcol_SalDetails.Earnings).Value) >= Val(cEPF_MAX_BASICPAY) And Val(cEPF_MAX_BASICPAY) > 0 Then

                            .Rows(n).Cells(dgvcol_SalDetails.PF).Value = Format(Math.Ceiling(Val(cEPF_MAX_BASICPAY) * Val(cPF_PERC) / 100), "#########0.00")

                        Else

                            .Rows(n).Cells(dgvcol_SalDetails.PF).Value = Format(Math.Ceiling(Val(.Rows(n).Cells(dgvcol_SalDetails.Earnings).Value) * Val(cPF_PERC) / 100), "#########0.00")

                        End If


                    End If

                    Dim cESI_MAX_BASICPAY_LIMIT As String = 0
                    Dim cESi_PERC As String = 0

                    If vESISTS_Sal = 1 Then

                        cESI_MAX_BASICPAY_LIMIT = 15000
                        cESi_PERC = 8.33

                        If Val(.Rows(n).Cells(dgvcol_SalDetails.Earnings).Value) > Val(cESI_MAX_BASICPAY_LIMIT) Then

                            .Rows(n).Cells(dgvcol_SalDetails.ESI).Value = Format(Val(cESI_MAX_BASICPAY_LIMIT) * Val(cESi_PERC) / 100, "#########0")

                        Else
                            .Rows(n).Cells(dgvcol_SalDetails.ESI).Value = Format(Val(.Rows(n).Cells(dgvcol_SalDetails.Earnings).Value) * Val(cESi_PERC) / 100, "#########0")

                        End If


                    End If
                    '-------------

                    .Rows(n).Cells(dgvcol_SalDetails.Tot_sal).Value = Val(Val(.Rows(n).Cells(dgvcol_SalDetails.Basic_Salary).Value) + Val(.Rows(n).Cells(dgvcol_SalDetails.OT_Sal).Value) + Val(.Rows(n).Cells(dgvcol_SalDetails.Incen).Value) - Val(.Rows(n).Cells(dgvcol_SalDetails.ESI).Value) - Val(.Rows(n).Cells(dgvcol_SalDetails.PF).Value))
                    If Val(.Rows(n).Cells(dgvcol_SalDetails.Tot_sal).Value) = 0 Then .Rows(n).Cells(dgvcol_SalDetails.Tot_sal).Value = ""


                    If Val(Mess_From_Dedution_Entry) <> 0 Then
                        .Rows(n).Cells(dgvcol_SalDetails.Mess).Value = Val(Mess_From_Dedution_Entry)
                    Else
                        .Rows(n).Cells(dgvcol_SalDetails.Mess).Value = Val(mess_Ded) * Val(Mess_wrk_dys)
                    End If
                    If Val(.Rows(n).Cells(dgvcol_SalDetails.Mess).Value) = 0 Then .Rows(n).Cells(dgvcol_SalDetails.Mess).Value = ""

                    Net_Salary = Format(Val(.Rows(n).Cells(dgvcol_SalDetails.Tot_sal).Value) - Val(.Rows(n).Cells(dgvcol_SalDetails.Mess).Value), "##########0")

                    .Rows(n).Cells(dgvcol_SalDetails.Net_Sal).Value = Format(Val(Net_Salary), "##########0.00")
                    If Val(.Rows(n).Cells(dgvcol_SalDetails.Net_Sal).Value) = 0 Then
                        If Val(.Rows(n).Cells(dgvcol_SalDetails.Mess).Value) = 0 Then
                            .Rows(n).Cells(dgvcol_SalDetails.Net_Sal).Value = ""
                        End If
                    End If

                    .Rows(n).Cells(dgvcol_SalDetails.Tot_Adv).Value = Val(Amt_OpBal)
                    If Val(.Rows(n).Cells(dgvcol_SalDetails.Tot_Adv).Value) = 0 Then .Rows(n).Cells(dgvcol_SalDetails.Tot_Adv).Value = ""

                    '.Rows(n).Cells(13).Value = Val(mins_Adv)
                    'If Val(.Rows(n).Cells(13).Value) = 0 Then .Rows(n).Cells(13).Value = ""

                    .Rows(n).Cells(dgvcol_SalDetails.BalAdv).Value = Val(Amt_OpBal)
                    If Val(.Rows(n).Cells(dgvcol_SalDetails.BalAdv).Value) = 0 Then
                        If Val(.Rows(n).Cells(dgvcol_SalDetails.Tot_Adv).Value) = 0 Then
                            .Rows(n).Cells(dgvcol_SalDetails.BalAdv).Value = ""
                        End If
                    End If
                    If Val(.Rows(n).Cells(dgvcol_SalDetails.BalAdv).Value) = 0 Then .Rows(n).Cells(dgvcol_SalDetails.BalAdv).Value = ""


                    .Rows(n).Cells(dgvcol_SalDetails.Salary_Adv).Value = Val(mins_Adv)
                    If Val(.Rows(n).Cells(dgvcol_SalDetails.Salary_Adv).Value) = 0 Then .Rows(n).Cells(dgvcol_SalDetails.Salary_Adv).Value = ""

                    .Rows(n).Cells(dgvcol_SalDetails.Salary_pen).Value = Val(Salary_Pending)
                    If Val(.Rows(n).Cells(dgvcol_SalDetails.Salary_pen).Value) = 0 Then .Rows(n).Cells(dgvcol_SalDetails.Salary_pen).Value = ""

                    'Old
                    ' Net_Pay = Format((Val(.Rows(n).Cells(dgvcol_SalDetails.Net_Sal).Value) - Val(.Rows(n).Cells(dgvcol_SalDetails.Tot_Adv).Value) - Val(.Rows(n).Cells(dgvcol_SalDetails.Salary_Adv).Value)) + Val(.Rows(n).Cells(dgvcol_SalDetails.Salary_pen).Value), "##########0")

                    '--------------Ref

                    Dim A1 As String = 0
                    Dim A2 As String = 0
                    Dim A3 As String = 0
                    Dim A4 As String = 0

                    A1 = Val(.Rows(n).Cells(dgvcol_SalDetails.Net_Sal).Value)
                    A2 = Val(.Rows(n).Cells(dgvcol_SalDetails.Less_adv).Value)
                    A3 = Val(.Rows(n).Cells(dgvcol_SalDetails.Salary_Adv).Value)
                    A4 = Val(.Rows(n).Cells(dgvcol_SalDetails.Salary_pen).Value)
                    '--------------Ref

                    'Old
                    'Net_Pay = Format((Val(.Rows(n).Cells(dgvcol_SalDetails.Net_Sal).Value) - Val(.Rows(n).Cells(dgvcol_SalDetails.Less_adv).Value) - Val(.Rows(n).Cells(dgvcol_SalDetails.Salary_Adv).Value)) + Val(.Rows(n).Cells(dgvcol_SalDetails.Salary_pen).Value), "##########0")

                    If Common_Procedures.settings.CustomerCode = "1087" Then
                        Net_Pay = Format((Val(.Rows(n).Cells(dgvcol_SalDetails.Net_Sal).Value) - Val(.Rows(n).Cells(dgvcol_SalDetails.Less_adv).Value)), "##########0")
                    Else
                        Net_Pay = Format((Val(.Rows(n).Cells(dgvcol_SalDetails.Net_Sal).Value) - Val(.Rows(n).Cells(dgvcol_SalDetails.Less_adv).Value) - Val(.Rows(n).Cells(dgvcol_SalDetails.Salary_Adv).Value)) + Val(.Rows(n).Cells(dgvcol_SalDetails.Salary_pen).Value), "##########0")
                    End If


                    .Rows(n).Cells(dgvcol_SalDetails.Net_pay).Value = Format(Net_Pay, "#########0.00")
                    If Val(.Rows(n).Cells(dgvcol_SalDetails.Net_pay).Value) = 0 Then
                        If Val(.Rows(n).Cells(dgvcol_SalDetails.Salary_Adv).Value) = 0 Then
                            .Rows(n).Cells(dgvcol_SalDetails.Net_pay).Value = ""
                        End If
                    End If

                    .Rows(n).Cells(dgvcol_SalDetails.Ot_Minutes).Value = OT_Mins
                    If Val(.Rows(n).Cells(dgvcol_SalDetails.Ot_Minutes).Value) = 0 Then .Rows(n).Cells(dgvcol_SalDetails.Ot_Minutes).Value = ""


                    '.Rows(n).Cells(dgvcol_SalDetails.Earnings).Value = Format(Val(.Rows(n).Cells(dgvcol_SalDetails.Basic_Salary).Value) + Val(.Rows(n).Cells(dgvcol_SalDetails.OT_Sal).Value), "#######0.00")
                    'If Val(.Rows(n).Cells(dgvcol_SalDetails.Earnings).Value) = 0 Then .Rows(n).Cells(dgvcol_SalDetails.Earnings).Value = ""


                    'Dim cEPF_MAX_BASICPAY As String = 0
                    'Dim cPF_PERC As String = 0

                    'If vPFSTS_Sal = 1 Then

                    '    cEPF_MAX_BASICPAY = 15000
                    '    cPF_PERC = 12

                    '    If Val(.Rows(n).Cells(dgvcol_SalDetails.Earnings).Value) >= Val(cEPF_MAX_BASICPAY) And Val(cEPF_MAX_BASICPAY) > 0 Then

                    '        .Rows(n).Cells(dgvcol_SalDetails.PF).Value = Format(Math.Ceiling(Val(cEPF_MAX_BASICPAY) * Val(cPF_PERC) / 100), "#########0.00")

                    '    Else

                    '        .Rows(n).Cells(dgvcol_SalDetails.PF).Value = Format(Math.Ceiling(Val(.Rows(n).Cells(dgvcol_SalDetails.Earnings).Value) * Val(cPF_PERC) / 100), "#########0.00")

                    '    End If


                    'End If

                    'Dim cESI_MAX_BASICPAY_LIMIT As String = 0
                    'Dim cESi_PERC As String = 0

                    'If vESISTS_Sal = 1 Then

                    '    cESI_MAX_BASICPAY_LIMIT = 15000
                    '    cESi_PERC = 8.33

                    '    If Val(.Rows(n).Cells(dgvcol_SalDetails.Earnings).Value) > Val(cESI_MAX_BASICPAY_LIMIT) Then

                    '        .Rows(n).Cells(dgvcol_SalDetails.ESI).Value = Format(Val(cESI_MAX_BASICPAY_LIMIT) * Val(cESi_PERC) / 100, "#########0")

                    '    Else
                    '        .Rows(n).Cells(dgvcol_SalDetails.ESI).Value = Format(Val(.Rows(n).Cells(dgvcol_SalDetails.Earnings).Value) * Val(cESi_PERC) / 100, "#########0")

                    '    End If


                    'End If


                Next i

            End If

            dt1.Dispose()
            da1.Dispose()

            dt2.Dispose()
            da2.Dispose()

        End With

        Grid_Cell_DeSelect()

    End Sub

    Private Sub Salary_Payment_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated

        Try

            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            If FrmLdSTS = True Then

                lbl_Company.Text = ""
                lbl_Company.Tag = 0
                Common_Procedures.CompIdNo = 0

                Me.Text = ""

                lbl_Company.Text = Common_Procedures.get_Company_From_CompanySelection(con)
                lbl_Company.Tag = Val(Common_Procedures.CompIdNo)

                Me.Text = lbl_Company.Text

                new_record()

            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "DOES NOT SHOW...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

        FrmLdSTS = False

    End Sub

    Private Sub Salary_Payment_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Me.Text = ""

        con.Open()

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1018" Then
            dgv_Details.Columns(6).Visible = False
        End If

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1087" Then 'Kalaimagal PayRoll - Palladam
            dgv_Details.Columns(18).Visible = False
            Label3.Text = "Advance Date"
            dtp_Advance_UpToDate.Visible = False
            dtp_Advance_FromDate.Visible = True
            dtp_Advance_ToDate.Visible = True
            Label7.Visible = True
        End If


        pnl_Filter.Visible = False
        pnl_Filter.Left = (Me.Width - pnl_Filter.Width) \ 2
        pnl_Filter.Top = (Me.Height - pnl_Filter.Height) \ 2
        pnl_Filter.BringToFront()

        AddHandler dtp_Date.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_PaymentType.GotFocus, AddressOf ControlGotFocus
        AddHandler Dtp_FromDate.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Month.GotFocus, AddressOf ControlGotFocus
        AddHandler Dtp_ToDate.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_TotalDays.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_FestivalDays.GotFocus, AddressOf ControlGotFocus
        AddHandler dtp_Filter_Fromdate.GotFocus, AddressOf ControlGotFocus
        AddHandler dtp_Filter_ToDate.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Filter_PartyName.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_FilterBillNo.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_save.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_close.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Category.GotFocus, AddressOf ControlGotFocus
        AddHandler dtp_Advance_FromDate.GotFocus, AddressOf ControlGotFocus
        AddHandler dtp_Advance_ToDate.GotFocus, AddressOf ControlGotFocus

        AddHandler dtp_Date.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_PaymentType.LostFocus, AddressOf ControlLostFocus
        AddHandler Dtp_FromDate.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Month.LostFocus, AddressOf ControlLostFocus
        AddHandler Dtp_ToDate.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_TotalDays.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_FestivalDays.LostFocus, AddressOf ControlLostFocus
        AddHandler dtp_Filter_Fromdate.LostFocus, AddressOf ControlLostFocus
        AddHandler dtp_Filter_ToDate.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Filter_PartyName.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_FilterBillNo.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_save.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_close.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Category.LostFocus, AddressOf ControlLostFocus
        AddHandler dtp_Advance_FromDate.LostFocus, AddressOf ControlLostFocus
        AddHandler dtp_Advance_ToDate.LostFocus, AddressOf ControlLostFocus

        AddHandler dtp_Date.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler Dtp_FromDate.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler Dtp_ToDate.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_FilterBillNo.KeyDown, AddressOf TextBoxControlKeyDown
        ' AddHandler txt_Month.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_TotalDays.KeyDown, AddressOf TextBoxControlKeyDown
        'AddHandler txt_FestivalDays.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler dtp_Filter_Fromdate.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler dtp_Filter_ToDate.KeyDown, AddressOf TextBoxControlKeyDown

        AddHandler dtp_Date.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler Dtp_FromDate.KeyPress, AddressOf TextBoxControlKeyPress
        ' AddHandler txt_Month.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_FilterBillNo.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_TotalDays.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler Dtp_ToDate.KeyPress, AddressOf TextBoxControlKeyPress
        'AddHandler txt_FestivalDays.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler dtp_Filter_Fromdate.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler dtp_Filter_ToDate.KeyPress, AddressOf TextBoxControlKeyPress

        lbl_Company.Text = ""
        lbl_Company.Tag = 0
        lbl_Company.Visible = False
        Common_Procedures.CompIdNo = 0

        Filter_Status = False
        FrmLdSTS = True
        new_record()

    End Sub

    Private Sub Salary_Payment_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        On Error Resume Next
        con.Close()
        con.Dispose()
        Common_Procedures.Last_Closed_FormName = Me.Name
    End Sub

    Private Sub Salary_Payment_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress

        Try
            If Asc(e.KeyChar) = 27 Then

                If pnl_Filter.Visible = True Then
                    btn_Filter_Close_Click(sender, e)
                    Exit Sub

                Else
                    Close_Form()

                End If

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT CLOSE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub Close_Form()

        Try

            lbl_Company.Tag = 0
            lbl_Company.Text = ""
            Me.Text = ""
            Common_Procedures.CompIdNo = 0

            lbl_Company.Text = Common_Procedures.Show_CompanySelection_On_FormClose(con)
            lbl_Company.Tag = Val(Common_Procedures.CompIdNo)
            Me.Text = lbl_Company.Text
            If Val(Common_Procedures.CompIdNo) = 0 Then

                Me.Close()

            Else

                new_record()

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT CLOSE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Public Sub delete_record() Implements Interface_MDIActions.delete_record
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim cmd As New SqlClient.SqlCommand
        Dim trans As SqlClient.SqlTransaction
        Dim NewCode As String = ""

        'If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.Pavu_Delivery_Entry, "~L~") = 0 And InStr(Common_Procedures.UR.Pavu_Delivery_Entry, "~D~") = 0 Then MessageBox.Show("You have No Rights to Insert", "DOES NOT INSERT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error) : Exit Sub

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)
        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.DeletingEntry, Common_Procedures.UR.PAYROLL_ENTRY_EMPLOYEE_SALARY, New_Entry, Me, con, "PayRoll_Salary_Head", "Salary_Code", NewCode, "Salary_Date", "(Salary_Code = '" & Trim(NewCode) & "')") = False Then Exit Sub


        If Val(lbl_Company.Tag) = 0 Then
            MessageBox.Show("Invalid Company Selection", "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
        End If

        If pnl_Back.Enabled = False Then
            MessageBox.Show("Close Other Windows", "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
        End If

        If MessageBox.Show("Do you want to Delete?", "FOR DELETION...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) <> Windows.Forms.DialogResult.Yes Then
            Exit Sub
        End If

        If New_Entry = True Then
            MessageBox.Show("This is New Entry", "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
        End If

        trans = con.BeginTransaction

        Try

            NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            cmd.Connection = con
            cmd.Transaction = trans

            cmd.CommandText = "Delete from Voucher_Head Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Voucher_Code LIKE '" & Trim(Pk_Condition) & "%/" & Trim(NewCode) & "' and Entry_Identification LIKE '" & Trim(Pk_Condition) & "%/" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()
            cmd.CommandText = "Delete from Voucher_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Voucher_Code LIKE '" & Trim(Pk_Condition) & "%/" & Trim(NewCode) & "' and Entry_Identification LIKE '" & Trim(Pk_Condition) & "%/" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Voucher_Head Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Voucher_Code LIKE '" & Trim(Pk_Condition2) & "%/" & Trim(NewCode) & "' and Entry_Identification LIKE '" & Trim(Pk_Condition2) & "%/" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()
            cmd.CommandText = "Delete from Voucher_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Voucher_Code LIKE '" & Trim(Pk_Condition2) & "%/" & Trim(NewCode) & "' and Entry_Identification LIKE '" & Trim(Pk_Condition2) & "%/" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "delete from PayRoll_Salary_Details where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Salary_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()
            cmd.CommandText = "delete from PayRoll_Salary_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Salary_Code = '" & Trim(NewCode) & "'"
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

            If dtp_Date.Enabled = True And dtp_Date.Visible = True Then dtp_Date.Focus()

        End Try

    End Sub

    Public Sub filter_record() Implements Interface_MDIActions.filter_record

        If Filter_Status = False Then

            Dim da As New SqlClient.SqlDataAdapter
            Dim dt1 As New DataTable
            Dim dt2 As New DataTable
            da = New SqlClient.SqlDataAdapter("select Ledger_DisplayName from Ledger_AlaisHead where (Ledger_IdNo = 0 or (Ledger_Type = '' and (AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14) ) ) order by Ledger_DisplayName", con)
            da.Fill(dt1)
            cbo_Filter_PartyName.DataSource = dt1
            cbo_Filter_PartyName.DisplayMember = "Ledger_DisplayName"

            dtp_Filter_Fromdate.Text = Common_Procedures.Company_FromDate
            dtp_Filter_ToDate.Text = Common_Procedures.Company_ToDate
            cbo_Filter_PartyName.Text = ""

            txt_FilterBillNo.Text = ""

            cbo_Filter_PartyName.SelectedIndex = -1
            dgv_Filter_Details.Rows.Clear()

        End If

        pnl_Filter.Visible = True
        pnl_Filter.Enabled = True
        pnl_Back.Enabled = False
        If dtp_Filter_Fromdate.Enabled And dtp_Filter_Fromdate.Visible Then dtp_Filter_Fromdate.Focus()

    End Sub

    Public Sub movefirst_record() Implements Interface_MDIActions.movefirst_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim movno As String

        Try

            da = New SqlClient.SqlDataAdapter("select top 1 Salary_No from PayRoll_Salary_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Salary_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby, Salary_No", con)
            dt = New DataTable
            da.Fill(dt)

            movno = ""
            If dt.Rows.Count > 0 Then
                If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                    movno = Trim(dt.Rows(0)(0).ToString)
                End If
            End If

            dt.Clear()

            If Val(movno) <> 0 Then move_record(movno)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR  MOVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Finally
            dt.Dispose()
            da.Dispose()

        End Try

    End Sub

    Public Sub movenext_record() Implements Interface_MDIActions.movenext_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim movno As String = ""
        Dim OrdByNo As Single = 0

        Try

            OrdByNo = Common_Procedures.OrderBy_CodeToValue(Trim(lbl_RefNo.Text))

            da = New SqlClient.SqlDataAdapter("select top 1 Salary_No from PayRoll_Salary_Head where for_orderby > " & Str(Format(Val(OrdByNo), "########.00")) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and Salary_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby, Salary_No", con)
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
            MessageBox.Show(ex.Message, "FOR  MOVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

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

            OrdByNo = Common_Procedures.OrderBy_CodeToValue(Trim(lbl_RefNo.Text))

            da = New SqlClient.SqlDataAdapter("select top 1 Salary_No from PayRoll_Salary_Head where for_orderby < " & Str(Format(Val(OrdByNo), "########0.00")) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and Salary_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, Salary_No desc", con)
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
            MessageBox.Show(ex.Message, "FOR  MOVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

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
            da = New SqlClient.SqlDataAdapter("select top 1 Salary_No from PayRoll_Salary_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Salary_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, Salary_No desc", con)
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
            MessageBox.Show(ex.Message, "FOR  MOVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Finally
            dt.Dispose()
            da.Dispose()

        End Try

    End Sub

    Public Sub new_record() Implements Interface_MDIActions.new_record

        Try
            clear()

            New_Entry = True

            lbl_RefNo.Text = Common_Procedures.get_MaxCode(con, "PayRoll_Salary_Head", "Salary_Code", "For_OrderBy", "", Val(lbl_Company.Tag), Common_Procedures.FnYearCode)
            lbl_RefNo.ForeColor = Color.Red

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR NEW RECORD...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Finally

            If dtp_Date.Enabled And dtp_Date.Visible Then dtp_Date.Focus()

        End Try

    End Sub

    Public Sub open_record() Implements Interface_MDIActions.open_record
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim movno As String, inpno As String
        Dim RefCode As String

        Try

            inpno = InputBox("Enter Ref No.", "FOR FINDING...")

            RefCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

            Da = New SqlClient.SqlDataAdapter("select Salary_No from PayRoll_Salary_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Salary_Code = '" & Trim(RefCode) & "'", con)
            Dt = New DataTable
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
                MessageBox.Show("Ref No. does not exists", "DOES NOT FIND...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT FIND...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Finally
            Dt.Dispose()
            Da.Dispose()

        End Try

    End Sub

    Public Sub insert_record() Implements Interface_MDIActions.insert_record
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim movno As String, inpno As String
        Dim InvCode As String

        'If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.Pavu_Delivery_Entry, "~L~") = 0 And InStr(Common_Procedures.UR.Pavu_Delivery_Entry, "~I~") = 0 Then MessageBox.Show("You have No Rights to Insert", "DOES NOT INSERT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error) : Exit Sub
        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.InsertingEntry, Common_Procedures.UR.PAYROLL_ENTRY_EMPLOYEE_SALARY, New_Entry, Me) = False Then Exit Sub

        Try

            inpno = InputBox("Enter New Ref No.", "FOR NEW Ref NO. INSERTION...")

            InvCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

            Da = New SqlClient.SqlDataAdapter("select Salary_No from PayRoll_Salary_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Salary_Code = '" & Trim(InvCode) & "'", con)
            Dt = New DataTable
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
                    MessageBox.Show("Invalid Ref No.", "DOES NOT INSERT NEW Ref...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

                Else
                    new_record()
                    Insert_Entry = True
                    lbl_RefNo.Text = Trim(UCase(inpno))

                End If

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT INSERT NEW BILL...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Finally
            Dt.Dispose()
            Da.Dispose()

        End Try

    End Sub

    Public Sub save_record() Implements Interface_MDIActions.save_record
        Dim cmd As New SqlClient.SqlCommand
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim tr As SqlClient.SqlTransaction
        Dim NewCode As String = ""
        Dim Emp_ID As Integer = 0
        Dim Mth_IDNo As Integer = 0
        Dim Sno As Integer = 0
        Dim EntID As String = ""
        Dim PBlNo As String = ""
        Dim Partcls As String = ""
        Dim VouBil As String = ""
        Dim YrnClthNm As String = ""
        Dim SalPymtTyp_IdNo As Integer = 0
        Dim Mon_Wek As String = "", VouNarr As String = ""
        Dim vLed_IdNos As String = "", vVou_Amts As String = "", ErrMsg As String = ""
        Dim Sal_Amt As Single = 0
        Dim CategoryId As Integer = 0


        If Val(lbl_Company.Tag) = 0 Then
            MessageBox.Show("Invalid Company Selection", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
        End If

        'If Common_Procedures.UserRight_Check(Common_Procedures.UR.Pavu_Delivery_Entry, New_Entry) = False Then Exit Sub

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)
        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.SavingEntry, Common_Procedures.UR.PAYROLL_ENTRY_EMPLOYEE_SALARY, New_Entry, Me, con, "PayRoll_Salary_Head", "Salary_Code", NewCode, "Salary_Date", "(Salary_Code = '" & Trim(NewCode) & "')", "(Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Salary_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "')", "for_Orderby desc, Salary_No desc", dtp_Date.Value.Date) = False Then Exit Sub


        If pnl_Back.Enabled = False Then
            MessageBox.Show("Close Other Windows", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
        End If

        If IsDate(dtp_Date.Text) = False Then
            MessageBox.Show("Invalid Date", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If dtp_Date.Enabled And dtp_Date.Visible Then dtp_Date.Focus()
            Exit Sub
        End If

        If Not (dtp_Date.Value.Date >= Common_Procedures.Company_FromDate And dtp_Date.Value.Date <= Common_Procedures.Company_ToDate) Then
            MessageBox.Show("Date is out of financial range", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If dtp_Date.Enabled And dtp_Date.Visible Then dtp_Date.Focus()
            Exit Sub
        End If

        SalPymtTyp_IdNo = Common_Procedures.Salary_PaymentType_NameToIdNo(con, cbo_PaymentType.Text)
        If Val(SalPymtTyp_IdNo) = 0 Then
            MessageBox.Show("Invalid Payment Type", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If cbo_PaymentType.Enabled And cbo_PaymentType.Visible Then cbo_PaymentType.Focus()
            Exit Sub
        End If

        Mon_Wek = Common_Procedures.get_FieldValue(con, "PayRoll_Salary_Payment_Type_Head", "Monthly_Weekly", "(Salary_Payment_Type_IdNo = " & Str(Val(SalPymtTyp_IdNo)) & ")")

        Mth_IDNo = 0
        If Trim(UCase(Mon_Wek)) <> "WEEKLY" Then
            Mth_IDNo = Common_Procedures.Month_NameToIdNo(con, cbo_Month.Text)
            If Val(Mth_IDNo) = 0 Then
                MessageBox.Show("Invalid Month", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                If cbo_Month.Enabled And cbo_Month.Visible Then cbo_Month.Focus()
                Exit Sub
            End If
        End If

        If IsDate(dtp_FromDate.Text) = False Then
            MessageBox.Show("Invalid From Date", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If dtp_FromDate.Enabled And dtp_FromDate.Visible Then dtp_FromDate.Focus()
            Exit Sub
        End If

        If IsDate(dtp_ToDate.Text) = False Then
            MessageBox.Show("Invalid To Date", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If dtp_ToDate.Enabled And dtp_ToDate.Visible Then dtp_ToDate.Focus()
            Exit Sub
        End If

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        CategoryId = Common_Procedures.Category_NameToIdNo(con, cbo_Category.Text)

        If Val(CategoryId) = 0 Then
            MessageBox.Show("Invalid Category", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If cbo_Category.Enabled And cbo_Category.Visible Then cbo_Category.Focus()
            Exit Sub
        End If
        cmd.Connection = con

        cmd.Parameters.Clear()

        cmd.Parameters.AddWithValue("@SalaryFromDate", dtp_FromDate.Value.Date)

        cmd.Parameters.AddWithValue("@SalaryToDate", dtp_ToDate.Value.Date)

        cmd.CommandText = "select * from PayRoll_Salary_Head where Salary_Payment_Type_IdNo = " & Str(Val(SalPymtTyp_IdNo)) & " and Salary_Code <> '" & Trim(NewCode) & "' and CateGory_Idno = " & Str(Val(CategoryId)) & " and ( (@SalaryFromDate Between From_Date and To_Date) or (@SalaryToDate Between From_Date and To_Date) )"
        Da = New SqlClient.SqlDataAdapter(cmd)
        Dt1 = New DataTable
        Da.Fill(Dt1)
        If Dt1.Rows.Count > 0 Then
            MessageBox.Show("Invalid From (or) To date " & Chr(13) & "Already Salary Entry prepared for this Date", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If dtp_ToDate.Enabled And dtp_ToDate.Visible Then dtp_ToDate.Focus()
            Exit Sub
        End If
        Dt1.Clear()

        With dgv_Details

            For i = 0 To .RowCount - 1

                If Val(.Rows(i).Cells(dgvcol_SalDetails.Att_day).Value) <> 0 Or Val(.Rows(i).Cells(dgvcol_SalDetails.Ot_Hrs).Value) <> 0 Or Val(.Rows(i).Cells(dgvcol_SalDetails.Incen).Value) <> 0 Or Val(.Rows(i).Cells(dgvcol_SalDetails.Mess).Value) <> 0 Or Val(.Rows(i).Cells(dgvcol_SalDetails.Salary_Adv).Value) <> 0 Then

                    Emp_ID = Common_Procedures.Employee_NameToIdNo(con, .Rows(i).Cells(dgvcol_SalDetails.Employee_Name).Value)
                    If Emp_ID = 0 Then
                        MessageBox.Show("Invalid Employee Name", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                        If .Enabled And .Visible Then
                            .Focus()
                            .CurrentCell = .Rows(i).Cells(dgvcol_SalDetails.Employee_Name)
                        End If
                        Exit Sub
                    End If

                End If

            Next

        End With

        tr = con.BeginTransaction

        'Try

        If Insert_Entry = True Or New_Entry = False Then
                NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            Else

                lbl_RefNo.Text = Common_Procedures.get_MaxCode(con, "PayRoll_Salary_Head", "Salary_Code", "For_OrderBy", "", Val(lbl_Company.Tag), Common_Procedures.FnYearCode, tr)

                NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            End If

            cmd.Connection = con
            cmd.Transaction = tr

            cmd.Parameters.Clear()
            cmd.Parameters.AddWithValue("@SalaryDate", dtp_Date.Value.Date)

            cmd.Parameters.AddWithValue("@FromDate", dtp_FromDate.Value.Date)

            cmd.Parameters.AddWithValue("@ToDate", dtp_ToDate.Value.Date)

        If dtp_Advance_UpToDate.Visible = True Then
            cmd.Parameters.AddWithValue("@AdvanceUpToDate", dtp_Advance_UpToDate.Value.Date)
        Else
            cmd.Parameters.AddWithValue("@AdvanceUpToDate", dtp_ToDate.Value.Date)
        End If

        If dtp_Advance_ToDate.Visible = True And dtp_Advance_FromDate.Visible = True Then
            cmd.Parameters.AddWithValue("@AdvanceFromDate", dtp_Advance_FromDate.Value.Date)
            cmd.Parameters.AddWithValue("@AdvanceToDate", dtp_Advance_ToDate.Value.Date)
        Else
            cmd.Parameters.AddWithValue("@AdvanceFromDate", dtp_FromDate.Value.Date)
            cmd.Parameters.AddWithValue("@AdvanceToDate", dtp_ToDate.Value.Date)
        End If


        If New_Entry = True Then
            cmd.CommandText = "Insert into PayRoll_Salary_Head (     Salary_Code        ,               Company_IdNo       ,           Salary_No           ,                               for_OrderBy                              ,   Salary_Date ,       Salary_Payment_Type_IdNo   ,          Month_IdNo  ,  From_Date,  To_Date,  Advance_UptoDate,                 Total_Days          ,                  Festival_Days          ,         Category_IdNo  ,   Advance_FromDate    ,   Advance_ToDate ) " &
                                    "          Values              ( '" & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_RefNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text))) & ",  @SalaryDate  , " & Str(Val(SalPymtTyp_IdNo)) & ", " & Val(Mth_IDNo) & ", @FromDate , @ToDate , @AdvanceUpToDate , " & Str(Val(txt_TotalDays.Text)) & ",  " & Str(Val(txt_FestivalDays.Text)) & " ,   " & CategoryId & " , @AdvanceFromDate  ,    @AdvanceToDate) "
            cmd.ExecuteNonQuery()

            Else

            cmd.CommandText = "Update PayRoll_Salary_Head set Salary_Date = @SalaryDate, Salary_Payment_Type_IdNo = " & Str(Val(SalPymtTyp_IdNo)) & ",  Month_IdNo = " & Val(Mth_IDNo) & ", From_Date = @FromDate, To_Date =  @ToDate, Advance_UptoDate =  @AdvanceUpToDate, Total_Days = " & Str(Val(txt_TotalDays.Text)) & ", Festival_Days = " & Str(Val(txt_FestivalDays.Text)) & " , Category_IdNo = " & CategoryId & " , Advance_FromDate =  @AdvanceFromDate , Advance_ToDate = @AdvanceToDate  Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Salary_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            End If

            cmd.CommandText = "Delete from PayRoll_Salary_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Salary_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Voucher_Head Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Voucher_Code LIKE '" & Trim(Pk_Condition) & "%/" & Trim(NewCode) & "' and Entry_Identification LIKE '" & Trim(Pk_Condition) & "%/" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()
            cmd.CommandText = "Delete from Voucher_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Voucher_Code LIKE '" & Trim(Pk_Condition) & "%/" & Trim(NewCode) & "' and Entry_Identification LIKE '" & Trim(Pk_Condition) & "%/" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Voucher_Head Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Voucher_Code LIKE '" & Trim(Pk_Condition2) & "%/" & Trim(NewCode) & "' and Entry_Identification LIKE '" & Trim(Pk_Condition2) & "%/" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()
            cmd.CommandText = "Delete from Voucher_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Voucher_Code LIKE '" & Trim(Pk_Condition2) & "%/" & Trim(NewCode) & "' and Entry_Identification LIKE '" & Trim(Pk_Condition2) & "%/" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            VouNarr = ""


            With dgv_Details

                Sno = 0
                For i = 0 To .RowCount - 1

                    Emp_ID = Common_Procedures.Employee_NameToIdNo(con, .Rows(i).Cells(dgvcol_SalDetails.Employee_Name).Value, tr)

                    If Val(Emp_ID) <> 0 Then

                        Sno = Sno + 1

                        cmd.CommandText = "Insert into PayRoll_Salary_Details (        Salary_Code     ,               Company_IdNo       ,            Salary_No          ,                               for_OrderBy                              ,   Salary_Date,            Sl_No     ,        Employee_IdNo    ,           Salary_Shift                   ,                                   No_Of_Attendance_Days       ,                                             Basic_Salary        ,                                                      Ot_Hours            ,                                          Ot_Pay_Hours        ,                                                Ot_Salary           ,                                        Incentive_Amount    ,                                                Total_Salary        ,                                                 Mess                 ,                                                       Net_Salary           ,                                  Advance              ,                                           Minus_Advance        ,                                                    Minus_MainAdvance                       ,                      Balance_Advance      ,                                                         Salary_Pending   ,                                         Net_Pay_Amount       ,                                                OT_Minutes                                          ,                             Earning        ,                                             P_F                                  ,                   ESI             ) " &
                                          "            Values                 ( '" & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_RefNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text))) & ",  @SalaryDate , " & Str(Val(Sno)) & ", " & Str(Val(Emp_ID)) & ", " & Str(Val(.Rows(i).Cells(dgvcol_SalDetails.Sal_day).Value)) & ", " & Str(Val(.Rows(i).Cells(dgvcol_SalDetails.Att_day).Value)) & ", " & Str(Val(.Rows(i).Cells(dgvcol_SalDetails.Basic_Salary).Value)) & ", " & Str(Val(.Rows(i).Cells(dgvcol_SalDetails.Ot_Hrs).Value)) & ",  " & Str(Val(.Rows(i).Cells(dgvcol_SalDetails.Ot_SalHr).Value)) & ", " & Str(Val(.Rows(i).Cells(dgvcol_SalDetails.OT_Sal).Value)) & ", " & Str(Val(.Rows(i).Cells(dgvcol_SalDetails.Incen).Value)) & ", " & Str(Val(.Rows(i).Cells(dgvcol_SalDetails.Tot_sal).Value)) & ",  " & Str(Val(.Rows(i).Cells(dgvcol_SalDetails.Mess).Value)) & ", " & Str(Val(.Rows(i).Cells(dgvcol_SalDetails.Net_Sal).Value)) & ", " & Str(Val(.Rows(i).Cells(dgvcol_SalDetails.Tot_Adv).Value)) & ", " & Str(Val(.Rows(i).Cells(dgvcol_SalDetails.Salary_Adv).Value)) & ", " & Str(Val(.Rows(i).Cells(dgvcol_SalDetails.Less_adv).Value)) & " , " & Str(Val(.Rows(i).Cells(dgvcol_SalDetails.BalAdv).Value)) & ", " & Str(Val(.Rows(i).Cells(dgvcol_SalDetails.Salary_pen).Value)) & ",  " & Str(Val(.Rows(i).Cells(dgvcol_SalDetails.Net_pay).Value)) & ", " & Str(Val(.Rows(i).Cells(dgvcol_SalDetails.Ot_Minutes).Value)) & "  ,  " & Str(Val(.Rows(i).Cells(dgvcol_SalDetails.Earnings).Value)) & " , " & Str(Val(.Rows(i).Cells(dgvcol_SalDetails.PF).Value)) & ", " & Str(Val(.Rows(i).Cells(dgvcol_SalDetails.ESI).Value)) & "   ) "
                        cmd.ExecuteNonQuery()



                        Sal_Amt = Val(.Rows(i).Cells(dgvcol_SalDetails.Net_Sal).Value) - Val(.Rows(i).Cells(dgvcol_SalDetails.Less_adv).Value)

                        If Val(Sal_Amt) <> 0 Then

                            If Val(Sal_Amt) < 0 Then
                                vLed_IdNos = Common_Procedures.CommonLedger.Salary_Ac & "|" & Emp_ID

                            Else
                                vLed_IdNos = Emp_ID & "|" & Common_Procedures.CommonLedger.Salary_Ac

                            End If

                            vVou_Amts = Format(Math.Abs(Val(Sal_Amt)), "#########0.00") & "|" & Format(-1 * Math.Abs(Val(Sal_Amt)), "#########0.00")

                            If Trim(UCase(Mon_Wek)) = "WEEKLY" Then
                                VouNarr = "Salary for Week " & dtp_FromDate.Text & " to " & dtp_ToDate.Text

                            Else
                                VouNarr = "Salary for Month " & cbo_Month.Text

                            End If

                            If Common_Procedures.Voucher_Updation(con, "Emp.Sal", Val(lbl_Company.Tag), Trim(Pk_Condition) & Trim(Val(Emp_ID)) & "/" & Trim(NewCode), Trim(lbl_RefNo.Text), dtp_Date.Value.Date, Trim(VouNarr), vLed_IdNos, vVou_Amts, ErrMsg, tr) = False Then
                                Throw New ApplicationException(ErrMsg)
                                Exit Sub
                            End If

                        End If

                        If Val(.Rows(i).Cells(dgvcol_SalDetails.Less_adv).Value) = 0 Then
                            .Rows(i).Cells(dgvcol_SalDetails.Less_adv).Value = 0
                        End If

                        If Val(.Rows(i).Cells(dgvcol_SalDetails.Less_adv).Value) <> 0 Then

                            If Trim(UCase(Mon_Wek)) = "WEEKLY" Then
                                VouNarr = "Less Advance for Week " & dtp_FromDate.Text & " to " & dtp_ToDate.Text

                            Else
                                VouNarr = "Less Advance for Month " & cbo_Month.Text

                            End If

                            If (Val(.Rows(i).Cells(dgvcol_SalDetails.Less_adv).Value)) < 0 Then
                                vLed_IdNos = Common_Procedures.CommonLedger.Salary_Ac & "|" & Emp_ID

                            Else
                                vLed_IdNos = Emp_ID & "|" & Common_Procedures.CommonLedger.Salary_Ac

                            End If

                            vVou_Amts = Math.Abs(Val(.Rows(i).Cells(dgvcol_SalDetails.Less_adv).Value)) & "|" & -1 * Math.Abs(Val(.Rows(i).Cells(dgvcol_SalDetails.Less_adv).Value))

                            If Common_Procedures.Voucher_Updation(con, "ADV.Less", Val(lbl_Company.Tag), Trim(Pk_Condition2) & Trim(Val(Emp_ID)) & "/" & Trim(NewCode), Trim(lbl_RefNo.Text), dtp_Date.Value.Date, Trim(VouNarr), vLed_IdNos, vVou_Amts, ErrMsg, tr) = False Then
                                Throw New ApplicationException(ErrMsg)
                                Exit Sub
                            End If

                        End If




                        'If Val(.Rows(i).Cells(13).Value) > 0 Then
                        '    vLed_IdNos = Emp_ID & "|" & Common_Procedures.CommonLedger.Salary_Ac

                        'Else
                        '    vLed_IdNos = Common_Procedures.CommonLedger.Salary_Ac & "|" & Emp_ID

                        'End If

                        'vVou_Amts = Math.Abs(Val(.Rows(i).Cells(13).Value)) & "|" & -1 * Math.Abs(Val(.Rows(i).Cells(13).Value))

                        'If Common_Procedures.Voucher_Updation(con, "Adv.Less", Val(lbl_Company.Tag), Trim(Pk_Condition2) & Trim(Val(Emp_ID)) & "/" & Trim(NewCode), Trim(lbl_RefNo.Text), dtp_Advance_UpToDate.Value.Date, Trim(VouNarr), vLed_IdNos, vVou_Amts, ErrMsg, tr) = False Then
                        '    Throw New ApplicationException(ErrMsg)
                        '    Exit Sub
                        'End If

                        '  End If


                    End If

                Next

            End With

            tr.Commit()


            MessageBox.Show("Saved Sucessfully!!!", "FOR SAVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Information)

            If Val(Common_Procedures.settings.OnSave_MoveTo_NewEntry_Status) = 1 Then
                If New_Entry = True Then
                    new_record()
                Else
                    move_record(lbl_RefNo.Text)
                End If
            Else
                move_record(lbl_RefNo.Text)
            End If


        'Catch ex As Exception
        '    tr.Rollback()
        '    MessageBox.Show(ex.Message, "FOR SAVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        'Finally

        '    Dt1.Dispose()
        '    Da.Dispose()
        '    cmd.Dispose()
        '    tr.Dispose()
        '    Dt1.Clear()

        '    If dtp_Date.Enabled And dtp_Date.Visible Then dtp_Date.Focus()

        'End Try

    End Sub

    Private Sub cbo_PaymentType_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_PaymentType.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "PayRoll_Salary_Payment_Type_Head", "Salary_Payment_Type_Name", "", "(Salary_Payment_Type_IdNo = 0)")
        cbo_PaymentType.Tag = cbo_PaymentType.Text
    End Sub

    Private Sub cbo_PaymentType_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_PaymentType.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_PaymentType, dtp_Date, cbo_Category, "PayRoll_Salary_Payment_Type_Head", "Salary_Payment_Type_Name", "", "(Salary_Payment_Type_IdNo = 0)")
    End Sub

    Private Sub cbo_PaymentType_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_PaymentType.KeyPress
        Dim Mon_Wek As String = ""
        Dim SalPymtTyp_IdNo As Integer = 0

        Try
            Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_PaymentType, Nothing, "PayRoll_Salary_Payment_Type_Head", "Salary_Payment_Type_Name", "", "(Salary_Payment_Type_IdNo = 0)")

            If Asc(e.KeyChar) = 13 Then

                If Trim(cbo_PaymentType.Text) <> "" Then

                    SalPymtTyp_IdNo = Common_Procedures.Salary_PaymentType_NameToIdNo(con, cbo_PaymentType.Text)

                    Mon_Wek = Common_Procedures.get_FieldValue(con, "PayRoll_Salary_Payment_Type_Head", "Monthly_Weekly", "(Salary_Payment_Type_IdNo = " & Str(Val(SalPymtTyp_IdNo)) & ")")

                    If Trim(UCase(cbo_PaymentType.Text)) <> Trim(UCase(cbo_PaymentType.Tag)) Then

                        If Trim(UCase(Mon_Wek)) = "WEEKLY" Then
                            dtp_FromDate.Enabled = True
                            dtp_ToDate.Enabled = True

                            cbo_Month.Text = ""

                            dtp_FromDate.Focus()

                            cbo_Month.Enabled = False

                        Else

                            If cbo_Category.Enabled = True Then
                                cbo_Category.Focus()
                            Else
                                cbo_Month.Focus()
                            End If
                            dtp_FromDate.Enabled = False
                            dtp_ToDate.Enabled = False

                            dtp_FromDate.Text = ""
                            dtp_ToDate.Text = ""



                        End If

                    Else

                        If Trim(UCase(Mon_Wek)) = "WEEKLY" Then
                            dtp_FromDate.Enabled = True
                            dtp_ToDate.Enabled = True

                            cbo_Category.Focus()

                            cbo_Month.Enabled = False

                        Else
                            cbo_Month.Enabled = True
                            dtp_FromDate.Enabled = False
                            dtp_ToDate.Enabled = False

                            cbo_Category.Focus()

                        End If

                    End If
                Else
                    cbo_Category.Focus()
                End If

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "ERROR WHILE PAYMENTTYPE KEYPRESS....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub cbo_Month_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Month.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Month_Head", "Month_Name", "", "(Month_IdNo = 0)")
        cbo_Month.Tag = cbo_Month.Text
    End Sub

    Private Sub cbo_Month_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Month.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Month, cbo_PaymentType, Dtp_FromDate, "Month_Head", "Month_Name", "", "(Month_IdNo = 0)")
    End Sub

    Private Sub cbo_Month_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Month.KeyPress
        Dim dttm As Date
        Dim Mth_ID As Integer = 0

        Try

            Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Month, Nothing, "Month_Head", "Month_Name", "", "(Month_IdNo = 0)")

            If Asc(e.KeyChar) = 13 And Trim(cbo_Month.Text) <> "" Then

                If Trim(UCase(cbo_Month.Tag)) <> Trim(UCase(cbo_Month.Text)) Then

                    Mth_ID = Common_Procedures.Month_NameToIdNo(con, cbo_Month.Text)

                    dttm = New DateTime(IIf(Mth_ID >= 4, Year(Common_Procedures.Company_FromDate), Year(Common_Procedures.Company_ToDate)), Mth_ID, 1)

                    dtp_FromDate.Text = dttm

                    dttm = DateAdd("M", 1, dttm)
                    dttm = DateAdd("d", -1, dttm)

                    dtp_ToDate.Text = dttm

                    get_PayRoll_Salary_Details()


                End If

                If dtp_Advance_UpToDate.Visible And dtp_Advance_UpToDate.Enabled Then
                    dtp_Advance_UpToDate.Focus()

                Else
                    If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                        save_record()
                    Else
                        dtp_Date.Focus()
                    End If

                End If

            End If

        Catch ex As Exception

            MessageBox.Show(ex.Message, "ERROR WHILE MONTH KEYPRESS....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub btn_Filter_Close_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Filter_Close.Click
        pnl_Back.Enabled = True
        pnl_Filter.Visible = False
        Filter_Status = False
    End Sub

    Private Sub btn_Filter_Show_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Filter_Show.Click
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim n As Integer
        Dim Led_IdNo As Integer, Del_IdNo As Integer, Cnt_IdNo As Integer
        Dim Condt As String = ""

        Try

            Condt = ""
            Del_IdNo = 0
            Cnt_IdNo = 0

            If IsDate(dtp_Filter_Fromdate.Value) = True And IsDate(dtp_Filter_ToDate.Value) = True Then
                Condt = "a.Salary_Date between '" & Trim(Format(dtp_Filter_Fromdate.Value, "MM/dd/yyyy")) & "' and '" & Trim(Format(dtp_Filter_ToDate.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_Filter_Fromdate.Value) = True Then
                Condt = "a.Salary_Date = '" & Trim(Format(dtp_Filter_Fromdate.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_Filter_ToDate.Value) = True Then
                Condt = "a.Salary_Date = '" & Trim(Format(dtp_Filter_ToDate.Value, "MM/dd/yyyy")) & "' "
            End If

            If Trim(cbo_Filter_PartyName.Text) <> "" Then
                Led_IdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Filter_PartyName.Text)
            End If


            If Val(Led_IdNo) <> 0 Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " a.Ledger_IdNo = " & Str(Val(Led_IdNo)) & " "
            End If


            If Trim(txt_FilterBillNo.Text) <> "" Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " a.Bill_No = '" & Trim(txt_FilterBillNo.Text) & "' "
            End If

            da = New SqlClient.SqlDataAdapter("select a.*,  c.Ledger_Name as PartyName from PayRoll_Salary_Head a  INNER JOIN Ledger_Head c ON a.Ledger_IdNo = c.Ledger_IdNo  where a.company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Salary_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "' " & IIf(Trim(Condt) <> "", " and ", "") & Condt & " Order by a.for_orderby, a.Salary_No", con)
            'da = New SqlClient.SqlDataAdapter("select a.*, b.*, c.Ledger_Name as Delv_Name from PayRoll_Salary_Head a INNER JOIN PayRoll_Salary_Details b ON a.Salary_Code = b.Salary_Code LEFT OUTER JOIN Ledger_Head c ON a.DeliveryTo_Idno = c.Ledger_IdNo where a.company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Salary_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "' " & IIf(Trim(Condt) <> "", " and ", "") & Condt & " Order by a.for_orderby, a.Salary_No", con)
            dt2 = New DataTable
            da.Fill(dt2)

            dgv_Filter_Details.Rows.Clear()

            If dt2.Rows.Count > 0 Then

                For i = 0 To dt2.Rows.Count - 1

                    n = dgv_Filter_Details.Rows.Add()

                    'dgv_Filter_Details.Rows(n).Cells(0).Value = i + 1
                    dgv_Filter_Details.Rows(n).Cells(0).Value = dt2.Rows(i).Item("Salary_No").ToString
                    dgv_Filter_Details.Rows(n).Cells(1).Value = Format(Convert.ToDateTime(dt2.Rows(i).Item("Salary_Date").ToString), "dd-MM-yyyy")
                    dgv_Filter_Details.Rows(n).Cells(2).Value = dt2.Rows(i).Item("PartyName").ToString
                    dgv_Filter_Details.Rows(n).Cells(3).Value = dt2.Rows(i).Item("Bill_No").ToString
                    dgv_Filter_Details.Rows(n).Cells(4).Value = Format(Val(dt2.Rows(i).Item("Net_Amount").ToString), "########0.00")

                Next i

            End If

            dt2.Clear()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT FILTER...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Finally
            dt2.Dispose()
            da.Dispose()

            If dgv_Filter_Details.Visible And dgv_Filter_Details.Enabled Then dgv_Filter_Details.Focus()

        End Try

    End Sub

    Private Sub cbo_Filter_PartyName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Filter_PartyName.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_PartyName, dtp_Filter_ToDate, btn_Filter_Show, "Ledger_AlaisHead", "Ledger_DisplayName", "(AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14 )", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_Filter_PartyName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Filter_PartyName.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Filter_PartyName, btn_Filter_Show, "Ledger_AlaisHead", "Ledger_DisplayName", "(AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14 )", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub Open_FilterEntry()
        Dim movno As String = ""

        Try
            movno = Trim(dgv_Filter_Details.CurrentRow.Cells(0).Value)

            If Val(movno) <> 0 Then
                Filter_Status = True
                move_record(movno)
                pnl_Back.Enabled = True
                pnl_Filter.Visible = False
            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "ERROR WHILE OPEN FILTER....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try


    End Sub

    Private Sub dgv_Filter_Details_CellDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Filter_Details.CellDoubleClick
        Open_FilterEntry()
    End Sub

    Private Sub dgv_Filter_Details_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_Filter_Details.KeyDown
        If e.KeyCode = 13 Then
            Open_FilterEntry()
        End If
    End Sub

    Private Sub dgv_Details_CellLeave(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellLeave
        Try
            With dgv_Details
                If .Visible Then
                    If .Rows.Count > 0 Then
                        If .CurrentCell.ColumnIndex = dgvcol_SalDetails.Less_adv Then
                            If Val(.CurrentRow.Cells(dgvcol_SalDetails.Less_adv).Value) <> 0 Then

                                .CurrentRow.Cells(dgvcol_SalDetails.Net_pay).Value = Format((Val(.CurrentRow.Cells(dgvcol_SalDetails.Net_Sal).Value) - Val(.CurrentRow.Cells(dgvcol_SalDetails.Salary_Adv).Value)) + Val(.CurrentRow.Cells(dgvcol_SalDetails.Salary_pen).Value) - Val(.CurrentRow.Cells(dgvcol_SalDetails.Less_adv).Value), "##########0.00")
                                .CurrentRow.Cells(dgvcol_SalDetails.BalAdv).Value = Format(Val(.CurrentRow.Cells(dgvcol_SalDetails.Tot_Adv).Value) - Val(.CurrentRow.Cells(dgvcol_SalDetails.Less_adv).Value), "#########0.00")

                            End If

                        End If

                    End If

                End If

            End With

        Catch ex As NullReferenceException
            '---MessageBox.Show(ex.Message, "ERROR WHILE DETAILS CELL CHANGE....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Catch ex As ObjectDisposedException
            '---MessageBox.Show(ex.Message, "ERROR WHILE DETAILS CELL CHANGE....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "ERROR WHILE DETAILS CELL CHANGE....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub dgv_Details_CellValueChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellValueChanged
        Try

            If IsNothing(dgv_Details.CurrentCell) Then Exit Sub

            With dgv_Details
                If .Visible Then
                    If .Rows.Count > 0 Then
                        If .CurrentCell.ColumnIndex = dgvcol_SalDetails.Less_adv Then
                            If Val(.CurrentRow.Cells(dgvcol_SalDetails.Less_adv).Value) <> 0 Then

                                If Common_Procedures.settings.CustomerCode = "1087" Then
                                    .CurrentRow.Cells(dgvcol_SalDetails.Net_pay).Value = Format((Val(.CurrentRow.Cells(dgvcol_SalDetails.Net_Sal).Value) - Val(.CurrentRow.Cells(dgvcol_SalDetails.Salary_Adv).Value)) - Val(.CurrentRow.Cells(dgvcol_SalDetails.Less_adv).Value), "##########0.00")
                                Else
                                    .CurrentRow.Cells(dgvcol_SalDetails.Net_pay).Value = Format((Val(.CurrentRow.Cells(dgvcol_SalDetails.Net_Sal).Value) - Val(.CurrentRow.Cells(dgvcol_SalDetails.Salary_Adv).Value)) + Val(.CurrentRow.Cells(dgvcol_SalDetails.Salary_pen).Value) - Val(.CurrentRow.Cells(dgvcol_SalDetails.Less_adv).Value), "##########0.00")
                                End If

                                .CurrentRow.Cells(dgvcol_SalDetails.BalAdv).Value = Format(Val(.CurrentRow.Cells(dgvcol_SalDetails.Tot_Adv).Value) - Val(.CurrentRow.Cells(dgvcol_SalDetails.Less_adv).Value), "#########0.00")

                            End If

                        End If

                    End If

                End If

            End With

        Catch ex As NullReferenceException
            '---MessageBox.Show(ex.Message, "ERROR WHILE DETAILS CELL CHANGE....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Catch ex As ObjectDisposedException
            '---MessageBox.Show(ex.Message, "ERROR WHILE DETAILS CELL CHANGE....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "ERROR WHILE DETAILS CELL CHANGE....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub dgv_Details_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_Details.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
    End Sub

    Private Sub dgv_Details_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_Details.KeyUp
        Dim i As Integer
        Dim n As Integer

        Try
            If e.Control = True And UCase(Chr(e.KeyCode)) = "D" Then

                With dgv_Details

                    n = .CurrentRow.Index

                    If .CurrentCell.RowIndex = .Rows.Count - 1 Then
                        For i = 0 To .ColumnCount - 1
                            .Rows(n).Cells(i).Value = ""
                        Next

                    Else
                        .Rows.RemoveAt(n)

                    End If

                    For i = 0 To .Rows.Count - 1
                        .Rows(i).Cells(dgvcol_SalDetails.SlNo).Value = i + 1
                    Next

                End With

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "ERROR WHILE DETAILS KEYPRESS....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub dgv_Details_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgv_Details.LostFocus
        On Error Resume Next
        If Not IsNothing(dgv_Details.CurrentCell) Then dgv_Details.CurrentCell.Selected = False
    End Sub

    Private Sub dgv_Details_RowsAdded(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewRowsAddedEventArgs) Handles dgv_Details.RowsAdded
        Dim n As Integer

        If IsNothing(dgv_Details.CurrentCell) Then Exit Sub

        With dgv_Details
            n = .RowCount
            .Rows(n - 1).Cells(dgvcol_SalDetails.SlNo).Value = Val(n)
        End With
    End Sub

    Private Sub btn_save_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_save.Click
        save_record()
    End Sub

    Private Sub btn_close_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_close.Click
        Me.Close()
    End Sub

    Private Sub txt_FestivalDays_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_FestivalDays.KeyDown
        If e.KeyValue = 40 Then
            If dgv_Details.Rows.Count > 0 Then
                dgv_Details.Focus()
                dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(dgvcol_SalDetails.Employee_Name)
                dgv_Details.CurrentCell.Selected = True

            Else
                btn_save.Focus()

            End If
        End If

        If e.KeyValue = 38 Then txt_TotalDays.Focus()

    End Sub

    Private Sub txt_TotalDays_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_TotalDays.LostFocus
        txt_TotalDays.Text = Format(Val(txt_TotalDays.Text), "#########0.00")
    End Sub

    Private Sub txt_FestivalDays_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_FestivalDays.LostFocus
        txt_FestivalDays.Text = Format(Val(txt_FestivalDays.Text), "#########0.00")
    End Sub

    Private Sub txt_FestivalDays_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_FestivalDays.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
        If Asc(e.KeyChar) = 13 Then
            If dgv_Details.RowCount > 0 Then

                dgv_Details.Focus()
                dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(dgvcol_SalDetails.Employee_Name)
                dgv_Details.CurrentCell.Selected = True
            End If
        End If
    End Sub

    Private Sub txt_VatAmount_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_TotalDays.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub btn_Print_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Common_Procedures.Print_OR_Preview_Status = 1
        print_record()
    End Sub

    Private Sub dtp_ToDate_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles dtp_ToDate.GotFocus
        dtp_FromDate.Tag = dtp_FromDate.Text
        dtp_ToDate.Tag = dtp_ToDate.Text
        dtp_Advance_UpToDate.Tag = dtp_Advance_UpToDate.Text
    End Sub

    Private Sub dtp_ToDate_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dtp_ToDate.KeyPress
        'Dim DtTm As Date
        Dim Mon_Wek As String = ""
        Dim SalPymtTyp_IdNo As Integer = 0

        Try
            If Asc(e.KeyChar) = 13 Then

                If Trim(UCase(dtp_FromDate.Tag)) <> Trim(UCase(dtp_FromDate.Text)) Or Trim(UCase(dtp_ToDate.Tag)) <> Trim(UCase(dtp_ToDate.Text)) Or Trim(UCase(dtp_Advance_UpToDate.Tag)) <> Trim(UCase(dtp_Advance_UpToDate.Text)) Then

                    'SalPymtTyp_IdNo = Common_Procedures.Salary_PaymentType_NameToIdNo(con, cbo_PaymentType.Text)

                    'Mon_Wek = Common_Procedures.get_FieldValue(con, "PayRoll_Salary_Payment_Type_Head", "Monthly_Weekly", "(Salary_Payment_Type_IdNo = " & Str(Val(SalPymtTyp_IdNo)) & ")")

                    'If Trim(UCase(Mon_Wek)) = "WEEKLY" Then

                    '    DtTm = dtp_FromDate.Value.Date

                    '    DtTm = DateAdd("d", 6, DtTm)

                    '    dtp_ToDate.Text = DtTm

                    'End If

                    get_PayRoll_Salary_Details()

                End If

                If dtp_Advance_UpToDate.Visible And dtp_Advance_UpToDate.Enabled Then
                    dtp_Advance_UpToDate.Focus()

                Else
                    If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                        save_record()
                    Else
                        dtp_Date.Focus()
                    End If

                End If

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "ERROR WHILE TODATE KEYPRESS....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub dtp_FromDate_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles dtp_FromDate.GotFocus
        dtp_FromDate.Tag = dtp_FromDate.Text
        dtp_ToDate.Tag = dtp_ToDate.Text
        dtp_Advance_UpToDate.Tag = dtp_Advance_UpToDate.Text
    End Sub

    Private Sub dtp_FromDate_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dtp_FromDate.KeyPress
        Dim DtTm As Date
        Dim Mon_Wek As String = ""
        Dim SalPymtTyp_IdNo As Integer = 0

        Try

            If Asc(e.KeyChar) = 13 Then

                If Trim(UCase(dtp_FromDate.Tag)) <> Trim(UCase(dtp_FromDate.Text)) Or Trim(UCase(dtp_ToDate.Tag)) <> Trim(UCase(dtp_ToDate.Text)) Or Trim(UCase(dtp_Advance_UpToDate.Tag)) <> Trim(UCase(dtp_Advance_UpToDate.Text)) Then

                    SalPymtTyp_IdNo = Common_Procedures.Salary_PaymentType_NameToIdNo(con, cbo_PaymentType.Text)

                    Mon_Wek = Common_Procedures.get_FieldValue(con, "PayRoll_Salary_Payment_Type_Head", "Monthly_Weekly", "(Salary_Payment_Type_IdNo = " & Str(Val(SalPymtTyp_IdNo)) & ")")

                    If Trim(UCase(Mon_Wek)) = "WEEKLY" Then

                        DtTm = dtp_FromDate.Value.Date

                        DtTm = DateAdd("d", 6, DtTm)

                        dtp_ToDate.Text = DtTm

                    End If

                    get_PayRoll_Salary_Details()

                End If

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "ERROR WHILE FROMDATE KEYPRESS....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Public Sub print_record() Implements Interface_MDIActions.print_record
        '------
    End Sub

    Private Sub btn_Calculation_Salary_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Calculation_Salary.Click
        get_PayRoll_Salary_Details()
    End Sub

    Private Sub dtp_Advance_UpToDate_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles dtp_Advance_UpToDate.GotFocus
        dtp_FromDate.Tag = dtp_FromDate.Text
        dtp_ToDate.Tag = dtp_ToDate.Text
        dtp_Advance_UpToDate.Tag = dtp_Advance_UpToDate.Text
    End Sub

    Private Sub dtp_Advance_UpToDate_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dtp_Advance_UpToDate.KeyPress
        Dim Mon_Wek As String = ""
        Dim SalPymtTyp_IdNo As Integer = 0

        Try
            If Asc(e.KeyChar) = 13 Then

                If Trim(UCase(dtp_FromDate.Tag)) <> Trim(UCase(dtp_FromDate.Text)) Or Trim(UCase(dtp_ToDate.Tag)) <> Trim(UCase(dtp_ToDate.Text)) Or Trim(UCase(dtp_Advance_UpToDate.Tag)) <> Trim(UCase(dtp_Advance_UpToDate.Text)) Then

                    get_PayRoll_Salary_Details()

                End If

                If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                    save_record()
                Else
                    dtp_Date.Focus()
                End If

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "ERROR WHILE ADVANCE UPTO DATE KEYPRESS....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub dgv_Details_EditingControlShowing(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewEditingControlShowingEventArgs) Handles dgv_Details.EditingControlShowing
        Try
            With dgv_Details
                If .Rows.Count > 0 Then
                    dgtxt_Details = CType(dgv_Details.EditingControl, DataGridViewTextBoxEditingControl)
                End If
            End With

        Catch ex As Exception
            MessageBox.Show(ex.Message, "ERROR WHILE DETAILS EDITING SHOWING....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub dgtxt_Details_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgtxt_Details.Enter
        Try
            dgv_Details.EditingControl.BackColor = Color.Lime
            dgv_Details.EditingControl.ForeColor = Color.Blue
            dgtxt_Details.SelectAll()

        Catch ex As Exception
            '---MessageBox.Show(ex.Message, "ERROR WHILE DETAILS TEXT ENTER....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub dgtxt_Details_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dgtxt_Details.KeyPress
        Try
            With dgv_Details
                If .Visible Then

                    If .Rows.Count > 0 Then

                        If .CurrentCell.ColumnIndex = dgvcol_SalDetails.Less_adv Or .CurrentCell.ColumnIndex = dgvcol_SalDetails.BalAdv Then

                            If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
                                e.Handled = True
                            End If

                        End If

                    End If

                End If
            End With

        Catch ex As Exception
            '---MessageBox.Show(ex.Message, "ERROR WHILE DETAILS TEXT KEYPRESS....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub btn_SalaryList_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_SalaryList.Click
        Dim f As New Report_Details
        Common_Procedures.RptInputDet.ReportGroupName = "Register"
        Common_Procedures.RptInputDet.ReportName = "Payroll Salary Register2"
        Common_Procedures.RptInputDet.ReportHeading = "Salary Register"
        Common_Procedures.RptInputDet.ReportInputs = "2DT,CAT,MON"
        f.MdiParent = MDIParent1
        f.Show()
        f.dtp_FromDate.Text = dtp_FromDate.Text 'dtp_Date.Text
        f.dtp_ToDate.Text = dtp_ToDate.Text 'dtp_Date.Text
        f.lbl_Inputs1.Text = ""
        f.cbo_Inputs2.Text = cbo_Month.Text
        f.Show_Report()
    End Sub


    Private Sub cbo_Category_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Category.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "PayRoll_Category_Head", "Category_Name", "", "(Category_IdNo = 0)")
        cbo_Category.Tag = cbo_Category.Text
    End Sub

    Private Sub cbo_Category_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Category.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Category, cbo_PaymentType, cbo_Month, "PayRoll_Category_Head", "Category_Name", "", "(Category_IdNo = 0)")
    End Sub

    Private Sub cbo_Category_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Category.KeyPress
        Try
            Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Category, cbo_Month, "PayRoll_Category_Head", "Category_Name", "", "(Category_IdNo = 0)")
        Catch ex As Exception
            MessageBox.Show(ex.Message, "ERROR WHILE KEYPRESS....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub dgtxt_Details_TextChanged(sender As Object, e As EventArgs) Handles dgtxt_Details.TextChanged
        Try
            With dgv_Details
                If .Rows.Count > 0 Then
                    .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(dgtxt_Details.Text)
                End If

            End With

        Catch ex As Exception
            '---

        End Try
    End Sub

    Private Sub cbo_PaymentType_TextChanged(sender As Object, e As EventArgs) Handles cbo_PaymentType.TextChanged


        'If Trim(cbo_PaymentType.Text) = "WEEKLY" Then
        '    dtp_FromDate.Enabled = True
        '    dtp_ToDate.Enabled = True

        '    cbo_Month.Text = ""

        '    dtp_FromDate.Focus()

        '    cbo_Month.Enabled = False

        'Else

        '    If cbo_Category.Enabled = True Then
        '        cbo_Category.Focus()
        '    Else
        '        cbo_Month.Focus()
        '    End If
        '    dtp_FromDate.Enabled = False
        '    dtp_ToDate.Enabled = False
        '    cbo_Month.Enabled = True
        '    dtp_FromDate.Text = ""
        '    dtp_ToDate.Text = ""



        'End If

    End Sub

    Private Sub dtp_Advance_FromDate_KeyDown(sender As Object, e As KeyEventArgs) Handles dtp_Advance_FromDate.KeyDown
        If e.KeyValue = 38 Then
            cbo_Month.Focus()
        End If
        If e.KeyValue = 40 Then
            dtp_Advance_ToDate.Focus()
        End If
    End Sub

    Private Sub dtp_Advance_FromDate_KeyPress(sender As Object, e As KeyPressEventArgs) Handles dtp_Advance_FromDate.KeyPress
        If Asc(e.KeyChar) = 13 Then
            dtp_Advance_ToDate.Focus()
        End If
    End Sub

    Private Sub dtp_Advance_ToDate_KeyPress(sender As Object, e As KeyPressEventArgs) Handles dtp_Advance_ToDate.KeyPress
        If Asc(e.KeyChar) = 13 Then
            btn_Calculation_Salary_Click(sender, e)
        End If
    End Sub

    Private Sub dtp_Advance_ToDate_KeyDown(sender As Object, e As KeyEventArgs) Handles dtp_Advance_ToDate.KeyDown
        If e.KeyValue = 38 Then
            dtp_Advance_FromDate.Focus()
        End If
        If e.KeyValue = 40 Then
            btn_Calculation_Salary_Click(sender, e)
        End If
    End Sub
End Class