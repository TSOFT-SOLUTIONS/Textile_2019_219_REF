Public Class PayRoll_Category_Creation
    Implements Interface_MDIActions

    Private con As New SqlClient.SqlConnection(Common_Procedures.Connection_String)
    Private New_Entry As Boolean = False
    Private Prec_ActCtrl As New Control
    Private vcbo_KeyDwnVal As Double

    Private WithEvents dgtxt_SchemesalaryDetails As New DataGridViewTextBoxEditingControl

    Private Sub clear()

        New_Entry = False

        pnl_Back.Enabled = True


        lbl_IdNo.Text = ""
        lbl_IdNo.ForeColor = Color.Black

        txt_Name.Text = ""
        msk_InTimeshift1.Text = ""
        msk_InTimeShift2.Text = ""
        msk_inTimeShift3.Text = ""
        txt_lunchMiniutes.Text = ""
        cbo_WeekOff.Text = "FIXED"
        chk_ot.Checked = False
        chk_TimeDelay.Checked = False
        cbo_AttendanceLeave.Text = "ATTENDANCE"
        chk_Attendance_Ot.Checked = False
        chk_Attendance_Incentive.Checked = False
        msk_OutTime_Shift1.Text = ""
        msk_OutTime_Shift2.Text = ""
        msk_OutTime_Shift3.Text = ""
        cbo_Monthly_Shift.Text = "SHIFT"
        txt_OtAllowed_Minute.Text = ""
        txt_MinimumDelay.Text = ""
        Chk_FestivalHolidays.Checked = False
        txt_Incentive_Amount.Text = ""

        msk_Working_Hours_Shift1.Text = ""
        msk_Working_Hours_Shift2.Text = ""
        msk_Working_Hours_Shift3.Text = ""

        txt_NoofDaye_Monthly.Text = ""


        chk_WeekOffCredit.Checked = False
        txt_LessMinuteDelay.Text = ""

        chk_Festival_Holiday_OtSalary.Checked = False
        chk_Production.Checked = False
        txt_Incentive_Amount_Days.Text = ""

        chk_LeaveSalaryLess.Checked = True

        txt_AttnIncenRange1_FromDays.Text = ""
        txt_AttnIncenRange1_ToDays.Text = ""
        txt_AttnIncenRange2_FromDays.Text = ""
        txt_AttnIncenRange2_ToDays.Text = ""

        chk_CL.Checked = False
        chk_SL.Checked = False
        cbo_CLArrear.Text = "SALARY"
        cbo_SLArrear.Text = "SALARY"

        If Trim(Common_Procedures.settings.CustomerCode) = "1087" Then
            txt_NoofDaye_Monthly.Enabled = True
        Else
            txt_NoofDaye_Monthly.Enabled = False
        End If

        cbo_AttendanceLeave.Enabled = False
        chk_LeaveSalaryLess.Enabled = False


    End Sub

    Private Sub ControlGotFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim txtbx As TextBox
        Dim combobx As ComboBox
        Dim Msktxbx As MaskedTextBox

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
            Msktxbx = Me.ActiveControl
            Msktxbx.SelectAll()
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
        'dgv_SchemeSalarydetails.CurrentCell.Selected = False
    End Sub

    Public Sub move_record(ByVal idno As Integer)
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt2 As New DataTable
        Dim SNo As Integer = 0
        Dim i As Integer = 0, n As Integer = 0

        If Val(idno) = 0 Then Exit Sub

        clear()

        Try
            da = New SqlClient.SqlDataAdapter("select a.* from PayRoll_Category_Head a  where a.Category_IdNo = " & Str(Val(idno)), con)
            dt = New DataTable
            da.Fill(dt)
            If dt.Rows.Count > 0 Then
                lbl_IdNo.Text = dt.Rows(0).Item("Category_IdNo").ToString
                txt_Name.Text = dt.Rows(0).Item("Category_Name").ToString
                msk_InTimeshift1.Text = dt.Rows(0).Item("Shift1_In_Time").ToString
                msk_InTimeShift2.Text = dt.Rows(0).Item("Shift2_In_Time").ToString
                msk_inTimeShift3.Text = dt.Rows(0).Item("Shift3_In_Time").ToString
                txt_lunchMiniutes.Text = Val(dt.Rows(0).Item("Lunch_Minutes").ToString)
                cbo_WeekOff.Text = dt.Rows(0).Item("Fixed_Rotation").ToString
                If Val(dt.Rows(0).Item("OT_Allowed").ToString) = 1 Then
                    chk_ot.Checked = True
                End If
                If Val(dt.Rows(0).Item("Time_Delay").ToString) = 1 Then
                    chk_TimeDelay.Checked = True
                End If
                cbo_AttendanceLeave.Text = dt.Rows(0).Item("Attendance_Leave").ToString
                If Val(dt.Rows(0).Item("Week_Attendance_Ot").ToString) = 1 Then
                    chk_Attendance_Ot.Checked = True
                End If
                If Val(dt.Rows(0).Item("Attendance_Incentive").ToString) = 1 Then
                    chk_Attendance_Incentive.Checked = True
                End If
                msk_OutTime_Shift1.Text = dt.Rows(0).Item("Shift1_Out_Time").ToString
                msk_OutTime_Shift2.Text = dt.Rows(0).Item("Shift2_Out_Time").ToString
                msk_OutTime_Shift3.Text = dt.Rows(0).Item("Shift3_Out_Time").ToString
                cbo_Monthly_Shift.Text = dt.Rows(0).Item("Monthly_Shift").ToString
                txt_OtAllowed_Minute.Text = Val(dt.Rows(0).Item("OT_Allowed_After_Minutes").ToString)
                txt_MinimumDelay.Text = Val(dt.Rows(0).Item("Minimum_Delay").ToString)
                If Val(dt.Rows(0).Item("Festival_Holidays").ToString) = 1 Then
                    Chk_FestivalHolidays.Checked = True
                End If
                txt_Incentive_Amount.Text = Format(Val(dt.Rows(0).Item("Incentive_Amount").ToString), "########0.00")
                msk_Working_Hours_Shift1.Text = Format(Val(dt.Rows(0).Item("Shift1_Working_Hours").ToString), "#######00.00")
                msk_Working_Hours_Shift2.Text = Format(Val(dt.Rows(0).Item("Shift2_Working_Hours").ToString), "#######00.00")
                msk_Working_Hours_Shift3.Text = Format(Val(dt.Rows(0).Item("Shift3_Working_Hours").ToString), "#######00.00")
                txt_NoofDaye_Monthly.Text = Val(dt.Rows(0).Item("No_Days_Month_Wages").ToString)

                txt_AttnIncenRange1_FromDays.Text = Val(dt.Rows(0).Item("Att_Incentive_FromDays_Range1").ToString)
                txt_AttnIncenRange1_ToDays.Text = Val(dt.Rows(0).Item("Att_Incentive_ToDays_Range1").ToString)
                txt_AttnIncenRange2_FromDays.Text = Val(dt.Rows(0).Item("Att_Incentive_FromDays_Range2").ToString)
                txt_AttnIncenRange2_ToDays.Text = Val(dt.Rows(0).Item("Att_Incentive_ToDays_Range2").ToString)


                If Val(dt.Rows(0).Item("Week_Off_Credit").ToString) = 1 Then
                    chk_WeekOffCredit.Checked = True
                End If
                txt_LessMinuteDelay.Text = Val(dt.Rows(0).Item("Less_Minute_Delay").ToString)

                If Val(dt.Rows(0).Item("Leave_Salary_Less").ToString) = 0 Then
                    chk_LeaveSalaryLess.Checked = False
                End If
                If Val(dt.Rows(0).Item("CL_Leave").ToString) = 1 Then
                    chk_CL.Checked = True
                End If
                If Val(dt.Rows(0).Item("SL_Leave").ToString) = 1 Then
                    chk_SL.Checked = True
                End If
                cbo_CLArrear.Text = Trim(dt.Rows(0).Item("CL_Arrear_Type").ToString)
                cbo_SLArrear.Text = Trim(dt.Rows(0).Item("SL_Arrear_Type").ToString)

                If Val(dt.Rows(0).Item("Festival_Holidays_OT_Salary").ToString) = 1 Then
                    chk_Festival_Holiday_OtSalary.Checked = True
                End If
                If Val(dt.Rows(0).Item("Production_Incentive").ToString) = 1 Then
                    chk_Production.Checked = True
                End If

                txt_Incentive_Amount_Days.Text = Format(Val(dt.Rows(0).Item("Incentive_Amount_Days").ToString), "########0.00")

            Else
                new_record()

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT MOVE RECORDS...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally

            dt.Dispose()
            da.Dispose()

            dt2.Dispose()
            da2.Dispose()

            Grid_Cell_DeSelect()
            If txt_Name.Enabled And txt_Name.Visible Then txt_Name.Focus()

        End Try

    End Sub

    Private Sub PayRoll_Category_Creation_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Me.Load
        ' Me.Height = 296 ' 197
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        con.Open()

        cbo_WeekOff.Items.Clear()
        cbo_WeekOff.Items.Add(" ")
        cbo_WeekOff.Items.Add("FIXED")
        cbo_WeekOff.Items.Add("ROTATION")

        cbo_AttendanceLeave.Items.Clear()
        cbo_AttendanceLeave.Items.Add(" ")
        cbo_AttendanceLeave.Items.Add("ATTENDANCE")
        cbo_AttendanceLeave.Items.Add("LEAVE")

        cbo_Monthly_Shift.Items.Clear()
        cbo_Monthly_Shift.Items.Add(" ")
        cbo_Monthly_Shift.Items.Add("MONTH")
        cbo_Monthly_Shift.Items.Add("SHIFT")


        cbo_CLArrear.Items.Clear()
        cbo_CLArrear.Items.Add(" ")
        cbo_CLArrear.Items.Add("SALARY")
        cbo_CLArrear.Items.Add("ELIMINATE")
        cbo_CLArrear.Items.Add("CARRY ON")

        cbo_SLArrear.Items.Clear()
        cbo_SLArrear.Items.Add(" ")
        cbo_SLArrear.Items.Add("SALARY")
        cbo_SLArrear.Items.Add("ELIMINATE")
        cbo_SLArrear.Items.Add("CARRY ON")

        If Trim(Common_Procedures.settings.CustomerCode) = "1087" Then 'Kalaimagal Tex palladam payRoll
            txt_NoofDaye_Monthly.Enabled = True
        Else
            txt_NoofDaye_Monthly.Enabled = False
        End If

        AddHandler txt_Name.GotFocus, AddressOf ControlGotFocus
        AddHandler msk_InTimeshift1.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_AttendanceLeave.GotFocus, AddressOf ControlGotFocus


        AddHandler msk_InTimeShift2.GotFocus, AddressOf ControlGotFocus
        AddHandler msk_inTimeShift3.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_WeekOff.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_lunchMiniutes.GotFocus, AddressOf ControlGotFocus
        AddHandler msk_OutTime_Shift2.GotFocus, AddressOf ControlGotFocus
        AddHandler msk_Working_Hours_Shift2.GotFocus, AddressOf ControlGotFocus
        AddHandler chk_ot.GotFocus, AddressOf ControlGotFocus
        AddHandler chk_TimeDelay.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_AttendanceLeave.GotFocus, AddressOf ControlGotFocus
        AddHandler chk_Attendance_Ot.GotFocus, AddressOf ControlGotFocus
        AddHandler chk_Attendance_Incentive.GotFocus, AddressOf ControlGotFocus
        AddHandler msk_OutTime_Shift1.GotFocus, AddressOf ControlGotFocus
        AddHandler msk_OutTime_Shift2.GotFocus, AddressOf ControlGotFocus
        AddHandler msk_OutTime_Shift3.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Monthly_Shift.GotFocus, AddressOf ControlGotFocus

        AddHandler txt_OtAllowed_Minute.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_MinimumDelay.GotFocus, AddressOf ControlGotFocus
        AddHandler Chk_FestivalHolidays.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Incentive_Amount.GotFocus, AddressOf ControlGotFocus
        AddHandler msk_Working_Hours_Shift1.GotFocus, AddressOf ControlGotFocus
        AddHandler msk_Working_Hours_Shift2.GotFocus, AddressOf ControlGotFocus
        AddHandler msk_Working_Hours_Shift3.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_NoofDaye_Monthly.GotFocus, AddressOf ControlGotFocus
        AddHandler chk_WeekOffCredit.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_LessMinuteDelay.GotFocus, AddressOf ControlGotFocus
        AddHandler chk_Production.GotFocus, AddressOf ControlGotFocus
        AddHandler chk_Festival_Holiday_OtSalary.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Incentive_Amount_Days.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_AttnIncenRange1_FromDays.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_AttnIncenRange2_FromDays.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_AttnIncenRange1_ToDays.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_AttnIncenRange2_ToDays.GotFocus, AddressOf ControlGotFocus
        AddHandler chk_CL.GotFocus, AddressOf ControlGotFocus
        AddHandler chk_SL.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_CLArrear.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_SLArrear.GotFocus, AddressOf ControlGotFocus


        AddHandler txt_Name.LostFocus, AddressOf ControlLostFocus
        AddHandler msk_InTimeshift1.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_AttendanceLeave.LostFocus, AddressOf ControlLostFocus
        AddHandler msk_InTimeShift2.LostFocus, AddressOf ControlLostFocus
        AddHandler msk_inTimeShift3.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_WeekOff.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_lunchMiniutes.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_AttendanceLeave.LostFocus, AddressOf ControlLostFocus
        AddHandler chk_ot.LostFocus, AddressOf ControlLostFocus
        AddHandler chk_TimeDelay.LostFocus, AddressOf ControlLostFocus
        AddHandler chk_Attendance_Ot.LostFocus, AddressOf ControlLostFocus
        AddHandler chk_Attendance_Incentive.LostFocus, AddressOf ControlLostFocus
        AddHandler msk_OutTime_Shift1.LostFocus, AddressOf ControlLostFocus
        AddHandler msk_OutTime_Shift2.LostFocus, AddressOf ControlLostFocus
        AddHandler msk_OutTime_Shift3.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Monthly_Shift.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_OtAllowed_Minute.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_MinimumDelay.LostFocus, AddressOf ControlLostFocus
        AddHandler Chk_FestivalHolidays.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Incentive_Amount.LostFocus, AddressOf ControlLostFocus
        AddHandler msk_Working_Hours_Shift1.LostFocus, AddressOf ControlLostFocus
        AddHandler msk_Working_Hours_Shift2.LostFocus, AddressOf ControlLostFocus
        AddHandler msk_Working_Hours_Shift3.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_NoofDaye_Monthly.LostFocus, AddressOf ControlLostFocus
        AddHandler chk_WeekOffCredit.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_LessMinuteDelay.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_MinimumDelay.LostFocus, AddressOf ControlLostFocus
        AddHandler chk_Production.LostFocus, AddressOf ControlLostFocus
        AddHandler chk_Festival_Holiday_OtSalary.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Incentive_Amount_Days.LostFocus, AddressOf ControlLostFocus
        ' AddHandler txt_BankAcNo.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_AttnIncenRange1_FromDays.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_AttnIncenRange2_FromDays.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_AttnIncenRange1_ToDays.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_AttnIncenRange2_ToDays.LostFocus, AddressOf ControlLostFocus
        AddHandler chk_CL.LostFocus, AddressOf ControlLostFocus
        AddHandler chk_SL.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_CLArrear.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_SLArrear.LostFocus, AddressOf ControlLostFocus




        AddHandler txt_Name.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler msk_InTimeshift1.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler msk_InTimeShift2.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler msk_inTimeShift3.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_lunchMiniutes.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler chk_ot.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler chk_TimeDelay.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler chk_Attendance_Ot.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler chk_Attendance_Incentive.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler msk_OutTime_Shift1.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler msk_OutTime_Shift2.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler msk_OutTime_Shift3.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_OtAllowed_Minute.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_MinimumDelay.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler Chk_FestivalHolidays.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Incentive_Amount.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler msk_Working_Hours_Shift1.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler msk_Working_Hours_Shift2.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler msk_Working_Hours_Shift3.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_NoofDaye_Monthly.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_LessMinuteDelay.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler chk_WeekOffCredit.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler chk_Production.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler chk_Festival_Holiday_OtSalary.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler chk_LeaveSalaryLess.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Incentive_Amount_Days.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_AttnIncenRange1_FromDays.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_AttnIncenRange2_FromDays.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_AttnIncenRange1_ToDays.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_AttnIncenRange2_ToDays.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler chk_CL.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler chk_SL.KeyDown, AddressOf TextBoxControlKeyDown


        AddHandler txt_Name.KeyPress, AddressOf TextBoxControlKeyPress

        AddHandler msk_InTimeshift1.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler msk_InTimeShift2.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler msk_inTimeShift3.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_lunchMiniutes.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler chk_ot.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler chk_TimeDelay.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler chk_Attendance_Ot.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler chk_Attendance_Incentive.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler msk_OutTime_Shift1.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler msk_OutTime_Shift2.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler msk_OutTime_Shift3.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler Chk_FestivalHolidays.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler chk_Festival_Holiday_OtSalary.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler chk_WeekOffCredit.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler chk_Production.KeyPress, AddressOf TextBoxControlKeyPress

        AddHandler txt_OtAllowed_Minute.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_MinimumDelay.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Incentive_Amount.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler msk_Working_Hours_Shift1.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler msk_Working_Hours_Shift2.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler msk_Working_Hours_Shift3.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_NoofDaye_Monthly.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_LessMinuteDelay.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler chk_LeaveSalaryLess.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_AttnIncenRange1_FromDays.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_AttnIncenRange2_FromDays.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_AttnIncenRange1_ToDays.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_AttnIncenRange2_ToDays.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler chk_CL.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler chk_SL.KeyPress, AddressOf TextBoxControlKeyPress

        new_record()

    End Sub

    Private Sub PayRoll_Category_Creation_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If Asc(e.KeyChar) = 27 Then
            'If grp_Find.Visible Then
            '    btnClose_Click(sender, e)
            'ElseIf grp_Filter.Visible Then
            '    btn_CloseFilter_Click(sender, e)
            'Else
            Me.Close()
        End If
        'End If
    End Sub

    Private Sub PayRoll_Category_Creation_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        con.Close()
        con.Dispose()
    End Sub

    Private Sub btn_Close_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Close.Click
        Me.Close()
        Me.Dispose()
    End Sub

    Public Sub delete_record() Implements Interface_MDIActions.delete_record
        Dim cmd As New SqlClient.SqlCommand


        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.DeletingEntry, Common_Procedures.UR.PAYROLL_ENTRY_CATEGORY_CREATION, New_Entry, Me) = False Then Exit Sub

        ' If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.Employee_Creation, "~L~") = 0 And InStr(Common_Procedures.UR.Employee_Creation, "~D~") = 0 Then MessageBox.Show("You have No Rights to Delete", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error) : Exit Sub

        If pnl_Back.Enabled = False Then
            MessageBox.Show("Close other window", "DOES NOT SAVE", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If
        If New_Entry = True Then
            MessageBox.Show("This is new entry", "DOES NOT DELETION....", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If
        'If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.Master_Employee_Creation, "~L~") = 0 And InStr(Common_Procedures.UR.Master_Employee_Creation, "~D~") = 0 Then MessageBox.Show("You have No Rights to Delete", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error) : Exit Sub

        If MessageBox.Show("Do you want to Delete ?", "FOR DELETION....", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) <> Windows.Forms.DialogResult.Yes Then
            Exit Sub
        End If

        Try

            cmd.Connection = con
            cmd.CommandText = "delete from PayRoll_Category_Head where Category_IdNo = " & Str(Val(lbl_IdNo.Text))




            cmd.ExecuteNonQuery()

            cmd.Dispose()

            new_record()

            MessageBox.Show("Deleted Sucessfully!!!", "FOR DELETION...", MessageBoxButtons.OK, MessageBoxIcon.Information)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally
            If txt_Name.Enabled And txt_Name.Visible Then txt_Name.Focus()

        End Try

    End Sub

    Public Sub filter_record() Implements Interface_MDIActions.filter_record
        'Dim da As New SqlClient.SqlDataAdapter("select Category_IdNo, Bag_Type_Name,Weight_Bag from PayRoll_Category_Head where Category_IdNo <> 0 order by Category_IdNo", con)
        'Dim dt As New DataTable

        'da.Fill(dt)

        'With dgv_Filter

        '    .Columns.Clear()
        '    .DataSource = dt

        '    .RowHeadersVisible = False

        '    .AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells
        '    .AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill

        '    .Columns(0).HeaderText = "IDNO"
        '    .Columns(1).HeaderText = "BAGTYPE NAME"
        '    .Columns(2).HeaderText = "WEIGHT BAG"

        '    .Columns(0).FillWeight = 40
        '    .Columns(1).FillWeight = 160
        '    .Columns(2).FillWeight = 80

        'End With

        'new_record()

        'grp_Filter.Visible = True
        'grp_Filter.Left = grp_Find.Left
        'grp_Filter.Top = grp_Find.Top

        'pnl_Back.Enabled = False

        'If dgv_Filter.Enabled And dgv_Filter.Visible Then dgv_Filter.Focus()

        'Me.Height = 595 ' 400

        'dt.Dispose()
        'da.Dispose()

    End Sub

    Public Sub movefirst_record() Implements Interface_MDIActions.movefirst_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim movid As Integer = 0

        Try
            da = New SqlClient.SqlDataAdapter("select min(Category_IdNo) from PayRoll_Category_Head Where Category_IdNo <> 0", con)
            da.Fill(dt)

            movid = 0
            If dt.Rows.Count > 0 Then
                If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                    movid = Val(dt.Rows(0)(0).ToString)
                End If
            End If

            If Val(movid) <> 0 Then move_record(movid)

            dt.Dispose()
            da.Dispose()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT MOVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Public Sub movelast_record() Implements Interface_MDIActions.movelast_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim movid As Integer = 0

        Try
            da = New SqlClient.SqlDataAdapter("select max(Category_IdNo) from PayRoll_Category_Head Where Category_IdNo <> 0", con)
            da.Fill(dt)

            movid = 0
            If dt.Rows.Count > 0 Then
                If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                    movid = Val(dt.Rows(0)(0).ToString)
                End If
            End If

            If Val(movid) <> 0 Then move_record(movid)

            dt.Dispose()
            da.Dispose()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT MOVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub

    Public Sub movenext_record() Implements Interface_MDIActions.movenext_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim movid As Integer

        Try
            da = New SqlClient.SqlDataAdapter("select min(Category_IdNo) from PayRoll_Category_Head Where Category_IdNo > " & Str(Val(lbl_IdNo.Text)) & " and Category_IdNo <> 0", con)
            da.Fill(dt)


            movid = 0
            If dt.Rows.Count > 0 Then
                If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                    movid = Val(dt.Rows(0)(0).ToString)
                End If
            End If

            If Val(movid) <> 0 Then move_record(movid)

            dt.Dispose()
            da.Dispose()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT MOVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub

        End Try

    End Sub

    Public Sub moveprevious_record() Implements Interface_MDIActions.moveprevious_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim movid As Integer

        Try
            da = New SqlClient.SqlDataAdapter("select max(Category_IdNo) from PayRoll_Category_Head where Category_IdNo < " & Str(Val(lbl_IdNo.Text)) & " and Category_IdNo <> 0 ", con)
            da.Fill(dt)

            movid = 0
            If dt.Rows.Count > 0 Then
                If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                    movid = Val(dt.Rows(0)(0).ToString)
                End If
            End If

            dt.Dispose()
            da.Dispose()

            If movid <> 0 Then move_record(movid)

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT MOVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub

        End Try

    End Sub

    Public Sub new_record() Implements Interface_MDIActions.new_record

        clear()

        New_Entry = True
        lbl_IdNo.ForeColor = Color.Red

        lbl_IdNo.Text = Common_Procedures.get_MaxIdNo(con, "PayRoll_Category_Head", "Category_IdNo", "")



        If txt_Name.Enabled And txt_Name.Visible Then txt_Name.Focus()

    End Sub

    Public Sub open_record() Implements Interface_MDIActions.open_record
        'Dim da As New SqlClient.SqlDataAdapter("select Bag_Type_Name from PayRoll_Category_Head order by Bag_Type_Name", con)
        'Dim dt As New DataTable

        'da.Fill(dt)

        'cbo_Find.DataSource = dt
        'cbo_Find.DisplayMember = "Bag_Type_Name"

        'new_record()

        'grp_Find.Visible = True
        'pnl_Back.Enabled = False

        'If cbo_Find.Enabled And cbo_Find.Visible Then cbo_Find.Focus()
        'Me.Height = 521 ' 355

    End Sub

    Public Sub insert_record() Implements Interface_MDIActions.insert_record
        'MessageBox.Show("insert record")
    End Sub

    Public Sub print_record() Implements Interface_MDIActions.print_record
        '--- No Printing
    End Sub

    Public Sub save_record() Implements Interface_MDIActions.save_record
        Dim cmd As New SqlClient.SqlCommand
        Dim trans As SqlClient.SqlTransaction
        Dim Sur As String
        Dim WrkTy_id As Integer = 0
        Dim SNo As Integer = 0
        Dim vTmDey As Integer = 0, vFhDa As Integer = 0
        Dim vOT As Integer = 0, vAttOT As Integer = 0
        Dim vAttIC As Integer = 0, vFhld As Integer = 0
        Dim vWekCd As Integer = 0, vProd As Integer = 0
        Dim vLeaSal As Integer = 1
        Dim vCL As Integer = 0, vSL As Integer = 0

        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.SavingEntry, Common_Procedures.UR.PAYROLL_ENTRY_CATEGORY_CREATION, New_Entry, Me) = False Then Exit Sub

        ' If Common_Procedures.UserRight_Check(Common_Procedures.UR.Employee_Creation, New_Entry) = False Then Exit Sub

        If pnl_Back.Enabled = False Then
            MessageBox.Show("Close other window", "DOES NOT SAVE", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        'If Common_Procedures.UserRight_Check(Common_Procedures.UR.Master_Employee_Creation, New_Entry) = False Then Exit Sub

        If Trim(txt_Name.Text) = "" Then
            MessageBox.Show("Invalid Name", "DOES NOT SAVE", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If txt_Name.Enabled And txt_Name.Visible Then txt_Name.Focus()
            Exit Sub
        End If
        If Val(Common_Procedures.settings.PAYROLLENTRY_Attendance_In_Hours_Status) = 1 Then
            If Val(msk_Working_Hours_Shift1.Text) = 0 Then
                MessageBox.Show("Invalid Shift Time", "DOES NOT SAVE", MessageBoxButtons.OK, MessageBoxIcon.Error)
                If msk_InTimeshift1.Enabled And msk_InTimeshift1.Visible Then msk_InTimeshift1.Focus()
                Exit Sub
            End If
        End If
        

        Sur = Common_Procedures.Remove_NonCharacters(Trim(txt_Name.Text))

        vFhld = 0
        If Chk_FestivalHolidays.Checked = True Then vFhld = 1

        vOT = 0
        If chk_ot.Checked = True Then vOT = 1

        vAttIC = 0
        If chk_Attendance_Incentive.Checked = True Then vAttIC = 1

        vWekCd = 0
        If chk_WeekOffCredit.Checked = True Then vWekCd = 1

        vProd = 0
        If chk_Production.Checked = True Then vProd = 1

        vFhDa = 0
        If chk_Festival_Holiday_OtSalary.Checked = True Then vFhDa = 1



        vAttOT = 0
        If chk_Attendance_Ot.Checked = True Then vAttOT = 1

        vTmDey = 0
        If chk_TimeDelay.Checked = True Then vTmDey = 1


        vLeaSal = 1
        If chk_LeaveSalaryLess.Checked = False Then vLeaSal = 0

        vCL = 0
        If chk_CL.Checked = True Then vCL = 1
        vSL = 0
        If chk_SL.Checked = True Then vSL = 1


        trans = con.BeginTransaction

        Try

            cmd.Connection = con
            cmd.Transaction = trans

            If New_Entry = True Then

                lbl_IdNo.Text = Common_Procedures.get_MaxIdNo(con, "PayRoll_Category_Head", "Category_IdNo", "", trans)

                cmd.CommandText = "Insert into PayRoll_Category_Head ( Category_IdNo, Category_Name, sur_name, Shift1_In_Time, Shift2_In_Time, Shift3_In_Time, Lunch_Minutes, Fixed_Rotation, OT_Allowed, Time_Delay, Attendance_Leave, Week_Attendance_OT, Attendance_Incentive, Shift1_Out_Time, Shift2_Out_Time, Shift3_Out_Time, Monthly_Shift, OT_Allowed_After_Minutes, Minimum_Delay, Festival_Holidays, Incentive_Amount, Shift1_Working_Hours, Shift2_Working_Hours, Shift3_Working_Hours, No_Days_Month_Wages, Week_Off_Credit, Less_Minute_Delay, Production_Incentive, Festival_Holidays_Ot_Salary, Incentive_Amount_Days, Leave_Salary_Less, Att_Incentive_FromDays_Range1, Att_Incentive_ToDays_Range1, Att_Incentive_FromDays_Range2, Att_Incentive_ToDays_Range2, CL_Leave, SL_Leave, CL_Arrear_Type, SL_Arrear_Type ) Values (" & Str(Val(lbl_IdNo.Text)) & ", '" & Trim(txt_Name.Text) & "', '" & Trim(Sur) & "', '" & Trim(msk_InTimeshift1.Text) & "', '" & Trim(msk_InTimeShift2.Text) & "', '" & Trim(msk_inTimeShift3.Text) & "', " & Val(txt_lunchMiniutes.Text) & ", '" & Trim(cbo_WeekOff.Text) & "', " & Val(vOT) & ", " & Val(vTmDey) & ", '" & Trim(cbo_AttendanceLeave.Text) & "', " & Val(vAttOT) & ", " & Val(vAttIC) & ", '" & Trim(msk_OutTime_Shift1.Text) & "', '" & Trim(msk_OutTime_Shift2.Text) & "', '" & Trim(msk_OutTime_Shift3.Text) & "', '" & Trim(cbo_Monthly_Shift.Text) & "', " & Val(txt_OtAllowed_Minute.Text) & ", " & Val(txt_MinimumDelay.Text) & ", " & Val(vFhld) & ", " & Val(txt_Incentive_Amount.Text) & ", '" & Trim(msk_Working_Hours_Shift1.Text) & "', '" & Trim(msk_Working_Hours_Shift2.Text) & "', '" & Trim(msk_Working_Hours_Shift3.Text) & "', " & Val(txt_NoofDaye_Monthly.Text) & ", " & Val(vWekCd) & ", " & Val(txt_LessMinuteDelay.Text) & ", " & Val(vProd) & ", " & Val(vFhDa) & ", " & Val(txt_Incentive_Amount_Days.Text) & "," & Val(vLeaSal) & ", " & Val(txt_AttnIncenRange1_FromDays.Text) & ", " & Val(txt_AttnIncenRange1_ToDays.Text) & ", " & Val(txt_AttnIncenRange2_FromDays.Text) & ", " & Val(txt_AttnIncenRange2_ToDays.Text) & ", " & Val(vCL) & ", " & Val(vSL) & ", '" & Trim(cbo_CLArrear.Text) & "', '" & Trim(cbo_SLArrear.Text) & "')"
                cmd.ExecuteNonQuery()

            Else
                cmd.CommandText = "update PayRoll_Category_Head set Category_Name = '" & Trim(txt_Name.Text) & "', sur_name = '" & Trim(Sur) & "',Shift1_In_Time = '" & Trim(msk_InTimeshift1.Text) & "' , Shift2_In_Time = '" & Trim(msk_InTimeShift2.Text) & "' ,Shift3_In_Time = '" & Trim(msk_inTimeShift3.Text) & "' ,Lunch_Minutes = " & Val(txt_lunchMiniutes.Text) & " , Fixed_Rotation = '" & Trim(cbo_WeekOff.Text) & "' ,OT_Allowed = " & Val(vOT) & " ,Time_Delay = " & Val(vTmDey) & " ,Attendance_Leave = '" & Trim(cbo_AttendanceLeave.Text) & "' ,Week_Attendance_OT = " & Val(vAttOT) & " ,Attendance_Incentive = " & Val(vAttIC) & " ,Shift1_Out_Time = '" & Trim(msk_OutTime_Shift1.Text) & "' ,Shift2_Out_Time = '" & Trim(msk_OutTime_Shift2.Text) & "' ,Shift3_Out_Time = '" & Trim(msk_OutTime_Shift3.Text) & "' , Monthly_Shift = '" & Trim(cbo_Monthly_Shift.Text) & "' ,OT_Allowed_After_Minutes = " & Val(txt_OtAllowed_Minute.Text) & " ,Minimum_Delay = " & Val(txt_MinimumDelay.Text) & " ,Festival_Holidays = " & Val(vFhld) & ",Incentive_Amount = " & Val(txt_Incentive_Amount.Text) & " ,Shift1_Working_Hours = '" & Trim(msk_Working_Hours_Shift1.Text) & "' ,Shift2_Working_Hours = '" & Trim(msk_Working_Hours_Shift2.Text) & "' ,Shift3_Working_Hours = '" & Trim(msk_Working_Hours_Shift3.Text) & "' ,No_Days_Month_Wages = " & Val(txt_NoofDaye_Monthly.Text) & " ,Week_Off_Credit = " & Val(vWekCd) & ",Less_Minute_Delay = " & Val(txt_LessMinuteDelay.Text) & " ,Production_Incentive = " & Val(vProd) & " ,Festival_Holidays_Ot_Salary = " & Val(vFhDa) & " ,Incentive_Amount_Days = " & Val(txt_Incentive_Amount_Days.Text) & " , Leave_Salary_Less = " & Val(vLeaSal) & " ,Att_Incentive_FromDays_Range1 = " & Val(txt_AttnIncenRange1_FromDays.Text) & " ,Att_Incentive_ToDays_Range1 = " & Val(txt_AttnIncenRange1_ToDays.Text) & " ,Att_Incentive_FromDays_Range2 =  " & Val(txt_AttnIncenRange2_FromDays.Text) & "   ,Att_Incentive_ToDays_Range2 = " & Val(txt_AttnIncenRange2_ToDays.Text) & ", CL_Leave =" & Val(vCL) & ",SL_Leave =" & Val(vSL) & " ,CL_Arrear_Type ='" & Trim(cbo_CLArrear.Text) & "' ,SL_Arrear_Type ='" & Trim(cbo_SLArrear.Text) & "' where Category_IdNo = " & Str(Val(lbl_IdNo.Text))
                cmd.ExecuteNonQuery()

            End If


            trans.Commit()

            Common_Procedures.Master_Return.Return_Value = Trim(txt_Name.Text)
            Common_Procedures.Master_Return.Master_Type = "CATEGORY"

            If New_Entry = True Then new_record()

            MessageBox.Show("Saved Sucessfully!!!", "FOR SAVING....", MessageBoxButtons.OK, MessageBoxIcon.Information)

            If Val(Common_Procedures.settings.OnSave_MoveTo_NewEntry_Status) = 1 Then
                If New_Entry = True Then
                    new_record()
                Else
                    move_record(lbl_IdNo.Text)
                End If
            Else
                move_record(lbl_IdNo.Text)
            End If

        Catch ex As Exception
            trans.Rollback()

            If InStr(1, Trim(LCase(ex.Message)), "ix_payRoll_category_head") > 0 Then
                MessageBox.Show("Duplicate Category Name", "DOES NOT SAVE", MessageBoxButtons.OK, MessageBoxIcon.Error)

            Else
                MessageBox.Show(ex.Message, "DOES NOT SAVE", MessageBoxButtons.OK, MessageBoxIcon.Error)

            End If

        Finally
            If txt_Name.Enabled And txt_Name.Visible Then txt_Name.Focus()

        End Try

    End Sub

    Private Sub btn_Save_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        save_record()
    End Sub

    'Private Sub btn_Find_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Find.Click
    '    Dim da As New SqlClient.SqlDataAdapter
    '    Dim dt As New DataTable
    '    Dim movid As Integer

    '    da = New SqlClient.SqlDataAdapter("select Category_IdNo from PayRoll_Category_Head where Bag_Type_Name = '" & Trim(cbo_Find.Text) & "'", con)
    '    da.Fill(dt)

    '    movid = 0
    '    If dt.Rows.Count > 0 Then
    '        If IsDBNull(dt.Rows(0)(0).ToString) = False Then
    '            movid = Val(dt.Rows(0)(0).ToString)
    '        End If
    '    End If

    '    dt.Dispose()
    '    da.Dispose()

    '    If movid <> 0 Then
    '        move_record(movid)
    '    Else
    '        new_record()
    '    End If

    '    btnClose_Click(sender, e)

    'End Sub

    'Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
    '    Me.Height = 296 ' 197
    '    pnl_Back.Enabled = True
    '    grp_Find.Visible = False
    '    If txt_Name.Enabled And txt_Name.Visible Then txt_Name.Focus()
    'End Sub

    'Private Sub cbo_Find_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Find.GotFocus
    '    'cbo_Find.DroppedDown = True
    'End Sub

    'Private Sub cbo_Find_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Find.KeyDown
    '    Try
    '        With cbo_Find
    '            If e.KeyValue = 38 And .DroppedDown = False Then
    '                e.Handled = True
    '                'SendKeys.Send("+{TAB}")
    '            ElseIf e.KeyValue = 40 And .DroppedDown = False Then
    '                e.Handled = True
    '                'SendKeys.Send("{TAB}")
    '            ElseIf e.KeyValue <> 13 And e.KeyValue <> 17 And e.KeyValue <> 27 And .DroppedDown = False Then
    '                .DroppedDown = True
    '            End If
    '        End With

    '    Catch ex As Exception
    '        MessageBox.Show(ex.Message, "DOES NOT SELECT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

    '    End Try
    'End Sub


    'Private Sub btn_CloseFilter_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_CloseFilter.Click

    '    pnl_Back.Enabled = True
    '    grp_Filter.Visible = False
    '    If txt_Name.Enabled And txt_Name.Visible Then txt_Name.Focus()

    '    Me.Height = 296 '197

    'End Sub

    'Private Sub btn_Open_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Open.Click
    '    Dim movid As Integer

    '    movid = 0
    '    If IsDBNull((dgv_Filter.CurrentRow.Cells(0).Value)) = False Then
    '        movid = Val(dgv_Filter.CurrentRow.Cells(0).Value)
    '    End If

    '    If Val(movid) <> 0 Then
    '        move_record(movid)
    '        btn_CloseFilter_Click(sender, e)
    '    End If

    'End Sub

    'Private Sub dgv_Filter_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgv_Filter.DoubleClick
    '    btn_Open_Click(sender, e)
    'End Sub

    'Private Sub dgv_Filter_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_Filter.KeyDown
    '    If e.KeyCode = Keys.Enter Then
    '        btn_Open_Click(sender, e)
    '    End If
    'End Sub


    Private Sub txt_Name_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
        If Asc(e.KeyChar) = 34 Or Asc(e.KeyChar) = 39 Then
            e.Handled = True
        End If


    End Sub






    Private Sub cbo_Weekoff_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_WeekOff.KeyDown

        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_WeekOff, txt_lunchMiniutes, chk_ot, "", "", "", "")


    End Sub

    Private Sub cbo_WeekOff_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_WeekOff.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_WeekOff, chk_ot, "", "", "", "")

    End Sub

    Private Sub msk_InTimeshift1_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles msk_InTimeshift1.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
        msk_Working_Hours_Shift1.Text = getHourFromMinitues(msk_InTimeshift1.Text, msk_OutTime_Shift1.Text)
    End Sub

    Private Sub msk_InTimeShift2_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles msk_InTimeShift2.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
        msk_Working_Hours_Shift2.Text = getHourFromMinitues(msk_InTimeShift2.Text, msk_OutTime_Shift2.Text)
    End Sub

    Private Sub msk_inTimeShift3_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles msk_inTimeShift3.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
        msk_Working_Hours_Shift3.Text = getHourFromMinitues(msk_inTimeShift3.Text, msk_OutTime_Shift3.Text)
    End Sub

    Private Sub txt_lunchMinutes_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_lunchMiniutes.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub msk_OutTime_Shift1_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles msk_OutTime_Shift1.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
        msk_Working_Hours_Shift1.Text = getHourFromMinitues(msk_InTimeshift1.Text, msk_OutTime_Shift1.Text)
    End Sub



    Private Sub msk_OutTime_Shift2_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles msk_OutTime_Shift2.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
        msk_Working_Hours_Shift2.Text = getHourFromMinitues(msk_InTimeShift2.Text, msk_OutTime_Shift2.Text)
    End Sub

    Private Sub txt_MinimumDelay_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_MinimumDelay.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True

    End Sub

    Private Sub msk_OutTime_Shift3_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles msk_OutTime_Shift3.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
        msk_Working_Hours_Shift3.Text = getHourFromMinitues(msk_inTimeShift3.Text, msk_OutTime_Shift3.Text)
    End Sub

    Private Sub txt_LessMiniuteDelay_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_LessMinuteDelay.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True

    End Sub

    Private Sub txt_Incentive_Amount_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Incentive_Amount.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True

    End Sub

    Private Sub txt_Incentive_Amount_Days_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Incentive_Amount_Days.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
        If Asc(e.KeyChar) = 13 Then
            If MessageBox.Show("Do you want to save ?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                save_record()
            Else
                txt_Name.Focus()
            End If
        End If
    End Sub

    Private Sub txt_OtAllowed_After_Miniute_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_OtAllowed_Minute.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True

    End Sub

    Private Sub txt_opAmt_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_NoofDaye_Monthly.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True

    End Sub

    Private Sub cbo_Monthly_Shift_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Monthly_Shift.GotFocus
        cbo_Monthly_Shift.Tag = cbo_Monthly_Shift.Text
    End Sub

    Private Sub cbo_Monthly_Shift_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Monthly_Shift.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Monthly_Shift, msk_OutTime_Shift3, txt_NoofDaye_Monthly, "", "", "", "")
    End Sub

    Private Sub cbo_Monthly_Shift_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Monthly_Shift.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Monthly_Shift, txt_NoofDaye_Monthly, "", "", "", "")
        If Asc(e.KeyChar) = 13 Then
            If Trim(UCase(cbo_Monthly_Shift.Tag)) <> Trim(UCase(cbo_Monthly_Shift.Text)) Then
                If Trim(UCase(cbo_Monthly_Shift.Text)) = "SHIFT" Then
                    txt_NoofDaye_Monthly.Text = ""
                    cbo_AttendanceLeave.Text = "ATTENDANCE"
                    chk_LeaveSalaryLess.Checked = True

                    txt_NoofDaye_Monthly.Enabled = False
                    cbo_AttendanceLeave.Enabled = False
                    chk_LeaveSalaryLess.Enabled = False

                Else
                    If Val(txt_NoofDaye_Monthly.Text) = 0 Then
                        txt_NoofDaye_Monthly.Text = "26"
                        cbo_AttendanceLeave.Text = "LEAVE"
                    End If

                    txt_NoofDaye_Monthly.Enabled = True
                    cbo_AttendanceLeave.Enabled = True
                    chk_LeaveSalaryLess.Enabled = True

                End If
            End If
        End If
    End Sub

    Private Sub cbo_AttendanceLeave_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_AttendanceLeave.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_AttendanceLeave, txt_NoofDaye_Monthly, chk_LeaveSalaryLess, "", "", "", "")
    End Sub

    Private Sub cbo_AttendanceLeave_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_AttendanceLeave.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_AttendanceLeave, chk_LeaveSalaryLess, "", "", "", "")
    End Sub
   

    Private Sub cbo_CLArrear_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_CLArrear.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_CLArrear, chk_SL, cbo_SLArrear, "", "", "", "")
    End Sub

    Private Sub cbo_CLArrear_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_CLArrear.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_CLArrear, cbo_SLArrear, "", "", "", "")
    End Sub

    Private Sub cbo_SLArrear_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_SLArrear.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_SLArrear, cbo_CLArrear, Chk_FestivalHolidays, "", "", "", "")
    End Sub

    Private Sub cbo_SLArrear_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_SLArrear.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_SLArrear, Chk_FestivalHolidays, "", "", "", "")
    End Sub
    Function getHourFromMinitues(ByVal inTime As String, ByVal outTime As String)

        Dim Dt1 As Date, Dt2 As Date
        Dim TotMins As Double
        Dim H As Double, m As Double, Hrs As Double

        If Val(Microsoft.VisualBasic.Left(inTime, 2)) >= 24 Then MessageBox.Show("Time should not greater than 24", "", MessageBoxButtons.OK, MessageBoxIcon.Error)

        If Val(Microsoft.VisualBasic.Right(inTime, 2)) >= 60 Then MessageBox.Show("Minutes should not greater than 60", "", MessageBoxButtons.OK, MessageBoxIcon.Error)

        If Val(Microsoft.VisualBasic.Left(outTime, 2)) >= 24 Then MessageBox.Show("Time should not greater than 24", "", MessageBoxButtons.OK, MessageBoxIcon.Error)

        If Val(Microsoft.VisualBasic.Right(outTime, 2)) >= 60 Then MessageBox.Show("Minutes should not greater than 60", "", MessageBoxButtons.OK, MessageBoxIcon.Error)

        If Trim(inTime) <> "" And Trim(outTime) <> "" Then
            If IsDate(inTime) And IsDate(outTime) Then
                If IsDate(Convert.ToDateTime(inTime)) And IsDate(Convert.ToDateTime(outTime)) Then

                    Dt1 = Convert.ToDateTime(inTime)
                    Dt2 = Convert.ToDateTime(outTime)

                    If Convert.ToDateTime(outTime) > Convert.ToDateTime(inTime) Then
                        TotMins = DateDiff("n", Dt1, Dt2)
                    Else

                        Dt2 = CDate(DateAdd("d", 1, Dt2))
                        TotMins = DateDiff("n", Dt1, Dt2)
                    End If

                    H = TotMins \ 60
                    m = TotMins - (H * 60)
                    Hrs = H & "." & Format(m, "00")
                End If
            End If
        End If

        Return Hrs
    End Function

    
    Private Sub chk_CL_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chk_CL.CheckedChanged
        If chk_CL.Checked = True Then
            cbo_CLArrear.Enabled = True
        Else
            cbo_CLArrear.Text = ""
            cbo_CLArrear.Enabled = False
        End If

    End Sub

    Private Sub chk_SL_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chk_SL.CheckedChanged
        If chk_SL.Checked = True Then
            cbo_SLArrear.Enabled = True
        Else
            cbo_SLArrear.Text = ""
            cbo_SLArrear.Enabled = False
        End If

    End Sub

    Private Sub btn_Save_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Save.Click
        save_record()
    End Sub

End Class