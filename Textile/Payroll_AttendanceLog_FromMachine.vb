Public Class Payroll_AttendanceLog_FromMachine
    Implements Interface_MDIActions

    Public axCZKEM1 As New zkemkeeper.CZKEM
    Private bIsConnected = False 'the boolean value identifies whether the device is connected
    Private iMachineNumber As Integer 'the serial number of the device.After connecting the device ,this value will be changed.

    Private con As New SqlClient.SqlConnection(Common_Procedures.Connection_String)
    Private FrmLdSTS As Boolean = False
    Private New_Entry As Boolean = False
    Private Insert_Entry As Boolean = False
    Private Filter_Status As Boolean = False
    Private Pk_Condition As String = "ATTFM-"
    Private NoCalc_Status As Boolean = False
    Private Prec_ActCtrl As New Control
    Private vcbo_KeyDwnVal As Double
    Private vCbo_ItmNm As String

    Private prn_HdDt As New DataTable
    Private prn_DetDt As New DataTable
    Private prn_PageNo As Integer
    Private prn_DetIndx As Integer
    Private prn_DetAr(50, 10) As String
    Private prn_DetMxIndx As Integer
    Private prn_NoofBmDets As Integer
    Private prn_DetSNo As Integer
    Public vmskOldText As String = ""
    Public vmskSelStrt As Integer = -1

    Private Sub clear()

        NoCalc_Status = True

        New_Entry = False
        Insert_Entry = False

        Pnl_Back.Enabled = True
        vmskOldText = ""
        vmskSelStrt = -1

        lbl_RefNo.Text = ""
        lbl_RefNo.ForeColor = Color.Black

        lvLogs.Items.Clear()

        dtp_Date.Text = ""
        msk_Date.Text = ""
        NoCalc_Status = False
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
            Msktxbx.SelectionStart = 0
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
        If e.KeyValue = 38 Then SendKeys.Send("+{TAB}")
        If e.KeyValue = 40 Then SendKeys.Send("{TAB}")
    End Sub

    Private Sub TextBoxControlKeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
        On Error Resume Next
        If Asc(e.KeyChar) = 13 Then SendKeys.Send("{TAB}")
    End Sub

    Private Sub AttendanceLog_FromMachine_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated
        Dim da As SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim NoofComps As Integer
        Dim CompCondt As String

        Try

            If FrmLdSTS = True Then

                Common_Procedures.CompIdNo = 0

                Me.Text = ""

                CompCondt = ""
                If Trim(UCase(Common_Procedures.User.Type)) = "ACCOUNT" Then
                    CompCondt = "Company_Type = 'ACCOUNT'"
                End If

                da = New SqlClient.SqlDataAdapter("select count(*) from Company_Head Where " & CompCondt & IIf(Trim(CompCondt) <> "", " and ", "") & " Company_IdNo <> 0", con)
                dt1 = New DataTable
                da.Fill(dt1)

                NoofComps = 0
                If dt1.Rows.Count > 0 Then
                    If IsDBNull(dt1.Rows(0)(0).ToString) = False Then
                        NoofComps = Val(dt1.Rows(0)(0).ToString)
                    End If
                End If
                dt1.Clear()

                If Val(NoofComps) = 1 Then

                    da = New SqlClient.SqlDataAdapter("select Company_IdNo, Company_Name from Company_Head Where " & CompCondt & IIf(Trim(CompCondt) <> "", " and ", "") & " Company_IdNo <> 0 Order by Company_IdNo", con)
                    dt1 = New DataTable
                    da.Fill(dt1)

                    If dt1.Rows.Count > 0 Then
                        If IsDBNull(dt1.Rows(0)(0).ToString) = False Then
                            Common_Procedures.CompIdNo = Val(dt1.Rows(0)(0).ToString)
                        End If

                    End If
                    dt1.Clear()

                Else

                    Dim f As New Company_Selection
                    f.ShowDialog()

                End If

                If Val(Common_Procedures.CompIdNo) <> 0 Then

                    da = New SqlClient.SqlDataAdapter("select Company_IdNo, Company_Name from Company_Head where Company_IdNo = " & Str(Val(Common_Procedures.CompIdNo)), con)
                    dt1 = New DataTable
                    da.Fill(dt1)

                    If dt1.Rows.Count > 0 Then
                        If IsDBNull(dt1.Rows(0)(0).ToString) = False Then
                            Me.Text = Trim(dt1.Rows(0)(1).ToString)
                        End If
                    End If
                    dt1.Clear()

                    new_record()

                Else
                    MessageBox.Show("Invalid Company Selection", "DOES NOT SELECT...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    'Me.Close()
                    Exit Sub

                End If

            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "DOES NOT SHOW...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        FrmLdSTS = False
    End Sub

    Private Sub AttendanceLog_FromMachine_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        con.Close()
        con.Dispose()
    End Sub

    Private Sub AttendanceLog_FromMachine_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        Try

            If Asc(e.KeyChar) = 27 Then
                Close_Form()
            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT CLOSE...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub AttendanceLog_FromMachine_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim dt3 As New DataTable
        Dim dt4 As New DataTable
        Dim dt5 As New DataTable
        Dim dt6 As New DataTable
        Dim dt7 As New DataTable
        Dim dt8 As New DataTable

        Me.Text = ""

        con.Open()

        dtp_Date.Text = ""
        msk_Date.Text = ""

        AddHandler dtp_Date.GotFocus, AddressOf ControlGotFocus
        AddHandler msk_Date.GotFocus, AddressOf ControlGotFocus

        AddHandler dtp_Date.LostFocus, AddressOf ControlLostFocus
        AddHandler msk_Date.LostFocus, AddressOf ControlLostFocus

        AddHandler dtp_Date.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler msk_Date.KeyDown, AddressOf TextBoxControlKeyDown

        Common_Procedures.CompIdNo = 0

        Filter_Status = False
        FrmLdSTS = True
        new_record()
    End Sub

    Private Sub Close_Form()
        Dim da As SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim NoofComps As Integer
        Dim CompCondt As String

        Try

            Me.Text = ""
            Common_Procedures.CompIdNo = 0

            CompCondt = ""
            If Trim(UCase(Common_Procedures.User.Type)) = "ACCOUNT" Then
                CompCondt = "Company_Type = 'ACCOUNT'"
            End If

            da = New SqlClient.SqlDataAdapter("select count(*) from Company_Head where " & CompCondt & IIf(Trim(CompCondt) <> "", " and ", "") & " Company_IdNo <> 0", con)
            dt1 = New DataTable
            da.Fill(dt1)

            NoofComps = 0
            If dt1.Rows.Count > 0 Then
                If IsDBNull(dt1.Rows(0)(0).ToString) = False Then
                    NoofComps = Val(dt1.Rows(0)(0).ToString)
                End If
            End If
            dt1.Clear()

            If Val(NoofComps) > 1 Then

                Dim f As New Company_Selection
                f.ShowDialog()

                If Val(Common_Procedures.CompIdNo) <> 0 Then

                    da = New SqlClient.SqlDataAdapter("select Company_IdNo, Company_Name from Company_Head where Company_IdNo = " & Str(Val(Common_Procedures.CompIdNo)), con)
                    dt1 = New DataTable
                    da.Fill(dt1)

                    If dt1.Rows.Count > 0 Then
                        If IsDBNull(dt1.Rows(0)(0).ToString) = False Then
                            Me.Text = Trim(dt1.Rows(0)(1).ToString)
                        End If
                    End If
                    dt1.Clear()
                    dt1.Dispose()
                    da.Dispose()

                    new_record()

                Else
                    Me.Close()

                End If

            Else

                Me.Close()

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT CLOSE...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub move_record(ByVal no As String)
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim NewCode As String
        Dim sno As Integer = 0
        Dim n As Integer = 0
        Dim iGLCount = 0
        Dim lvItem As New ListViewItem("Items", 0)

      
        If Val(no) = 0 Then Exit Sub

        clear()

        NewCode = Trim(no) & "/" & Trim(Common_Procedures.FnYearCode)

        Try

            da1 = New SqlClient.SqlDataAdapter("select a.* from Payroll_AttendanceLog_FromMachine_Head a Where a.AttendanceLog_FromMachine_Code = '" & Trim(NewCode) & "'", con)
            dt1 = New DataTable
            da1.Fill(dt1)

            If dt1.Rows.Count > 0 Then

                lbl_RefNo.Text = dt1.Rows(0).Item("AttendanceLog_FromMachine_No").ToString
                dtp_Date.Text = dt1.Rows(0).Item("AttendanceLog_FromMachine_Date").ToString
                msk_Date.Text = dtp_Date.Text
            End If

            lvLogs.Items.Clear()

            da2 = New SqlClient.SqlDataAdapter("Select a.* from Payroll_AttendanceLog_FromMachine_Details a Where a.AttendanceLog_FromMachine_Code = '" & Trim(NewCode) & "' Order by a.sl_no", con)
            dt2 = New DataTable
            da2.Fill(dt2)

            With lvLogs

                sno = 0
                n = 0
                If dt2.Rows.Count > 0 Then

                    For i = 0 To dt2.Rows.Count - 1

                        n = n + 1

                        lvItem = lvLogs.Items.Add(n.ToString())
                        lvItem.SubItems.Add(dt2.Rows(i).Item("Employee_CardNo").ToString)
                        lvItem.SubItems.Add("")  '--idwVerifyMode.ToString() 
                        lvItem.SubItems.Add(Val(dt2.Rows(i).Item("IN_Out").ToString))
                        lvItem.SubItems.Add(dt2.Rows(i).Item("INOut_DateTime_Text").ToString)
                        lvItem.SubItems.Add(Year(dt2.Rows(i).Item("INOut_DateTime")) & "~" & Month(dt2.Rows(i).Item("INOut_DateTime")) & "~" & Microsoft.VisualBasic.DateAndTime.Day(dt2.Rows(i).Item("INOut_DateTime")) & "~" & Hour(dt2.Rows(i).Item("INOut_DateTime")) & "~" & Minute(dt2.Rows(i).Item("INOut_DateTime")) & "~" & Second(dt2.Rows(i).Item("INOut_DateTime")))
                        lvItem.SubItems.Add("")

                    Next i

                End If
            End With

            dt1.Clear()
            dt1.Dispose()
            da1.Dispose()


        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT MOVE RECORDS...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        If msk_Date.Visible And msk_Date.Enabled Then msk_Date.Focus()

    End Sub

    Public Sub delete_record() Implements Interface_MDIActions.delete_record
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim cmd As New SqlClient.SqlCommand
        Dim trans As SqlClient.SqlTransaction
        Dim NewCode As String = ""

        If Pnl_Back.Enabled = False Then
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

            NewCode = Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            cmd.Connection = con
            cmd.Transaction = trans

            cmd.CommandText = "Delete from Payroll_AttendanceLog_FromMachine_Details Where AttendanceLog_FromMachine_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "delete from Payroll_AttendanceLog_FromMachine_Head where AttendanceLog_FromMachine_Code = '" & Trim(NewCode) & "'"
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

    Public Sub filter_record() Implements Interface_MDIActions.filter_record
        '-----
    End Sub

    Public Sub movefirst_record() Implements Interface_MDIActions.movefirst_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim movno As String

        Try

            da = New SqlClient.SqlDataAdapter("select top 1 AttendanceLog_FromMachine_No from Payroll_AttendanceLog_FromMachine_Head where AttendanceLog_FromMachine_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby, AttendanceLog_FromMachine_No", con)
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
            MessageBox.Show(ex.Message, "FOR  MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Public Sub movenext_record() Implements Interface_MDIActions.movenext_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim movno As String = ""
        Dim OrdByNo As Single = 0

        Try

            OrdByNo = Common_Procedures.OrderBy_CodeToValue(Trim(lbl_RefNo.Text))

            da = New SqlClient.SqlDataAdapter("select top 1 AttendanceLog_FromMachine_No from Payroll_AttendanceLog_FromMachine_Head where for_orderby > " & Str(Val(OrdByNo)) & " and AttendanceLog_FromMachine_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby, AttendanceLog_FromMachine_No", con)
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

        End Try

    End Sub

    Public Sub moveprevious_record() Implements Interface_MDIActions.moveprevious_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim movno As String = ""
        Dim OrdByNo As Single = 0

        Try

            OrdByNo = Common_Procedures.OrderBy_CodeToValue(Trim(lbl_RefNo.Text))

            da = New SqlClient.SqlDataAdapter("select top 1 AttendanceLog_FromMachine_No from Payroll_AttendanceLog_FromMachine_Head where for_orderby < " & Str(Val(OrdByNo)) & " and AttendanceLog_FromMachine_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, AttendanceLog_FromMachine_No desc", con)
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

    Public Sub movelast_record() Implements Interface_MDIActions.movelast_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim movno As String = ""

        Try
            da = New SqlClient.SqlDataAdapter("select top 1 AttendanceLog_FromMachine_No from Payroll_AttendanceLog_FromMachine_Head where AttendanceLog_FromMachine_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, AttendanceLog_FromMachine_No desc", con)
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

        End Try

    End Sub

    Public Sub new_record() Implements Interface_MDIActions.new_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim NewID As Integer = 0

        Try
            clear()

            New_Entry = True

            lbl_RefNo.Text = Common_Procedures.get_MaxCode(con, "Payroll_AttendanceLog_FromMachine_Head", "AttendanceLog_FromMachine_Code", "For_OrderBy", "", 0, Common_Procedures.FnYearCode)

            lbl_RefNo.ForeColor = Color.Red

            msk_Date.Text = Date.Today.ToShortDateString
            da = New SqlClient.SqlDataAdapter("select top 1 * from Payroll_AttendanceLog_FromMachine_Head where AttendanceLog_FromMachine_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, AttendanceLog_FromMachine_No desc", con)
            dt1 = New DataTable
            da.Fill(dt1)
            If dt1.Rows.Count > 0 Then
                If Val(Common_Procedures.settings.PreviousEntryDate_ByDefault) = 1 Then '---- M.S Textiles (Tirupur)
                    If dt1.Rows(0).Item("AttendanceLog_FromMachine_Date").ToString <> "" Then msk_Date.Text = dt1.Rows(0).Item("AttendanceLog_FromMachine_Date").ToString
                End If
            End If
            dt1.Clear()

            If msk_Date.Enabled And msk_Date.Visible Then msk_Date.Focus() : msk_Date.SelectionStart = 0

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR NEW RECORD...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        dt1.Dispose()
        da.Dispose()

    End Sub

    Public Sub open_record() Implements Interface_MDIActions.open_record
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim movno As String, inpno As String
        Dim RecCode As String

        Try

            inpno = InputBox("Enter Ref.No.", "FOR FINDING...")

            RecCode = Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

            Da = New SqlClient.SqlDataAdapter("select AttendanceLog_FromMachine_No from Payroll_AttendanceLog_FromMachine_Head where AttendanceLog_FromMachine_Code = '" & Trim(RecCode) & "'", con)
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
                MessageBox.Show("Ref No. does not exists", "DOES NOT FIND...", MessageBoxButtons.OK, MessageBoxIcon.Error)

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT FIND...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Public Sub insert_record() Implements Interface_MDIActions.insert_record
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim movno As String, inpno As String
        Dim RecCode As String

        '   If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.AttendanceLog_FromMachine_Entry, "~L~") = 0 And InStr(Common_Procedures.UR.AttendanceLog_FromMachine_Entry, "~I~") = 0 Then MessageBox.Show("You have No Rights to Insert", "DOES NOT INSERT...", MessageBoxButtons.OK, MessageBoxIcon.Error) : Exit Sub

        Try

            inpno = InputBox("Enter New Ref No.", "FOR NEW RECEIPT INSERTION...")

            RecCode = Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

            Da = New SqlClient.SqlDataAdapter("select AttendanceLog_FromMachine_No from Payroll_AttendanceLog_FromMachine_Head where AttendanceLog_FromMachine_Code = '" & Trim(RecCode) & "'", con)
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
                If Val(inpno) = 0 Then
                    MessageBox.Show("Invalid Ref No", "DOES NOT INSERT NEW RECEIPT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

                Else
                    new_record()
                    Insert_Entry = True
                    lbl_RefNo.Text = Trim(UCase(inpno))

                End If

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT INSERT NEW RECEIPT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Public Sub print_record() Implements Interface_MDIActions.print_record
        '----
    End Sub

    Public Sub save_record() Implements Interface_MDIActions.save_record
        Dim cmd As New SqlClient.SqlCommand
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim tr As SqlClient.SqlTransaction
        Dim NewCode As String = ""
        Dim Led_ID As Integer = 0
        Dim Led1_ID As Integer = 0
        Dim Trans_ID As Integer = 0
        Dim Clo_ID As Integer = 0
        Dim EdsCnt_ID As Integer = 0
        Dim Sno As Integer = 0
        Dim Partcls As String = ""
        Dim PBlNo As String = ""
        Dim YCnt_ID As Integer = 0
        Dim YMil_ID As Integer = 0
        Dim EntID As String = ""
        Dim Delv_ID As Integer = 0, Rec_ID As Integer = 0
        Dim Led_type As String = ""
        Dim VouBil As String = ""
        Dim IO_DtTm_Arr() As String
        Dim InOut_Date As DateTime

        'If Common_Procedures.UserRight_Check(Common_Procedures.UR.AttendanceLog_FromMachine_Entry, New_Entry) = False Then Exit Sub

        If pnl_Back.Enabled = False Then
            MessageBox.Show("Close Other Windows", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        If IsDate(msk_Date.Text) = False Then
            MessageBox.Show("Invalid Date", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If msk_Date.Enabled And msk_Date.Visible Then msk_Date.Focus()
            Exit Sub
        End If

        If Not (Convert.ToDateTime(msk_Date.Text) >= Common_Procedures.Company_FromDate And Convert.ToDateTime(msk_Date.Text) <= Common_Procedures.Company_ToDate) Then
            MessageBox.Show("Date is out of financial range", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If msk_Date.Enabled And msk_Date.Visible Then msk_Date.Focus()
            Exit Sub
        End If

        tr = con.BeginTransaction

        Try

            If Insert_Entry = True Or New_Entry = False Then
                NewCode = Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            Else

                lbl_RefNo.Text = Common_Procedures.get_MaxCode(con, "Payroll_AttendanceLog_FromMachine_Head", "AttendanceLog_FromMachine_Code", "For_OrderBy", "", 0, Common_Procedures.FnYearCode, tr)

                NewCode = Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            End If

            cmd.Connection = con
            cmd.Transaction = tr

            cmd.Parameters.Clear()
            cmd.Parameters.AddWithValue("@EntryDate", Convert.ToDateTime(msk_Date.Text))

            If New_Entry = True Then
                cmd.CommandText = "Insert into Payroll_AttendanceLog_FromMachine_Head(AttendanceLog_FromMachine_Code, AttendanceLog_FromMachine_No, for_OrderBy, AttendanceLog_FromMachine_Date ) Values ('" & Trim(NewCode) & "', '" & Trim(lbl_RefNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text))) & ", @EntryDate )"
                cmd.ExecuteNonQuery()

            Else

                cmd.CommandText = "Update Payroll_AttendanceLog_FromMachine_Head set AttendanceLog_FromMachine_Date = @EntryDate  Where AttendanceLog_FromMachine_Code = '" & Trim(NewCode) & "' "
                cmd.ExecuteNonQuery()

            End If

            cmd.CommandText = "Delete from Payroll_AttendanceLog_FromMachine_Details Where AttendanceLog_FromMachine_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            Sno = 0
            For i = 0 To lvLogs.Items.Count - 1

                If Val(lvLogs.Items(i).SubItems(1).Text) <> 0 Then

                    IO_DtTm_Arr = Split(Trim(lvLogs.Items(i).SubItems(5).Text), "~")

                    InOut_Date = New DateTime(Val(IO_DtTm_Arr(0)), Val(IO_DtTm_Arr(1)), Val(IO_DtTm_Arr(2)), Val(IO_DtTm_Arr(3)), Val(IO_DtTm_Arr(4)), Val(IO_DtTm_Arr(5)))

                    cmd.Parameters.Clear()
                    cmd.Parameters.AddWithValue("@EntryDate", Convert.ToDateTime(msk_Date.Text))
                    cmd.Parameters.AddWithValue("@AttenDateTime", InOut_Date)

                    Sno = Sno + 1
                    cmd.CommandText = "Insert into Payroll_AttendanceLog_FromMachine_Details ( AttendanceLog_FromMachine_Code , AttendanceLog_FromMachine_No  ,                               for_OrderBy                              ,   AttendanceLog_FromMachine_Date ,          Sl_No        ,                  Employee_CardNo                ,                  IN_Out                          ,                    INOut_DateTime_Text            , INOut_DateTime   ) " & _
                                        "          Values                                    (   '" & Trim(NewCode) & "'      , '" & Trim(lbl_RefNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text))) & ",               @EntryDate         ,  " & Str(Val(Sno)) & ", '" & Trim(lvLogs.Items(i).SubItems(1).Text) & "',  '" & Trim(lvLogs.Items(i).SubItems(3).Text) & "',   '" & Trim(lvLogs.Items(i).SubItems(4).Text) & "',  @AttenDateTime  ) "
                    cmd.ExecuteNonQuery()

                End If

            Next

            tr.Commit()

            Dt1.Dispose()
            Da.Dispose()

            MessageBox.Show("Saved Sucessfully!!!", "FOR SAVING...", MessageBoxButtons.OK, MessageBoxIcon.Information)

            If Val(Common_Procedures.settings.OnSave_MoveTo_NewEntry_Status) = 1 Then
                If New_Entry = True Then
                    new_record()
                Else
                    move_record(lbl_RefNo.Text)
                End If

            Else
                move_record(lbl_RefNo.Text)

            End If

        Catch ex As Exception
            tr.Rollback()
            MessageBox.Show(ex.Message, "FOR SAVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally
            If msk_Date.Enabled And msk_Date.Visible Then msk_Date.Focus()

        End Try

    End Sub

    Private Sub btn_close_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_close.Click
        Me.Close()
    End Sub

    Private Sub btn_save_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_save.Click
        save_record()
    End Sub

    Private Sub btnConnect_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnConnect.Click
        If txtIP.Text.Trim() = "" Or txtPort.Text.Trim() = "" Then
            MsgBox("IP and Port cannot be null", MsgBoxStyle.Exclamation, "Error")
            Return
        End If
        Dim idwErrorCode As Integer
        Cursor = Cursors.WaitCursor
        If btnConnect.Text = "Disconnect" Then
            axCZKEM1.Disconnect()
            bIsConnected = False
            btnConnect.Text = "Connect"
            lblState.Text = "Current State:Disconnected"
            Cursor = Cursors.Default
            Return
        End If

        bIsConnected = axCZKEM1.Connect_Net(txtIP.Text.Trim(), Convert.ToInt32(txtPort.Text.Trim()))
        If bIsConnected = True Then
            btnConnect.Text = "Disconnect"
            btnConnect.Refresh()
            lblState.Text = "Current State:Connected"
            iMachineNumber = 1 'In fact,when you are using the tcp/ip communication,this parameter will be ignored,that is any integer will all right.Here we use 1.
            axCZKEM1.RegEvent(iMachineNumber, 65535) 'Here you can register the realtime events that you want to be triggered(the parameters 65535 means registering all)
        Else
            axCZKEM1.GetLastError(idwErrorCode)
            MsgBox("Unable to connect the device,ErrorCode=" & idwErrorCode, MsgBoxStyle.Exclamation, "Error")
        End If
        Cursor = Cursors.Default

    End Sub

    Private Sub btnRsConnect_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRsConnect.Click
        If cbPort.Text.Trim() = "" Or cbBaudRate.Text.Trim() = "" Or txtMachineSN.Text.Trim() = "" Then
            MsgBox("Port,BaudRate and MachineSN cannot be null", MsgBoxStyle.Exclamation, "Error")
            Return
        End If
        Dim idwErrorCode As Integer

        'accept serialport number from string like "COMi"
        Dim iPort As Integer
        'Dim sPort = cbPort.Text.Trim()
        Dim sPort As String = cbPort.Text.Trim()
        For iPort = 1 To 9
            If sPort.IndexOf(iPort.ToString()) > -1 Then
                Exit For
            End If
        Next

        Cursor = Cursors.WaitCursor
        If btnRsConnect.Text = "Disconnect" Then
            AxCZKEM1.Disconnect()
            bIsConnected = False
            btnRsConnect.Text = "Connect"
            lblState.Text = "Current State:Disconnected"
            Cursor = Cursors.Default
            Return
        End If

        iMachineNumber = Convert.ToInt32(txtMachineSN.Text.Trim()) '//when you are using the serial port communication,you can distinguish different devices by their serial port number.
        bIsConnected = AxCZKEM1.Connect_Com(iPort, iMachineNumber, Convert.ToInt32(cbBaudRate.Text.Trim()))

        If bIsConnected = True Then
            btnRsConnect.Text = "Disconnect"
            btnRsConnect.Refresh()
            lblState.Text = "Current State:Connected"
            AxCZKEM1.RegEvent(iMachineNumber, 65535) 'Here you can register the realtime events that you want to be triggered(the parameters 65535 means registering all)
        Else
            AxCZKEM1.GetLastError(idwErrorCode)
            MsgBox("Unable to connect the device,ErrorCode=" & idwErrorCode, MsgBoxStyle.Exclamation, "Error")
        End If
        Cursor = Cursors.Default
    End Sub

    'If your device supports the USBCLient, you can refer to this.
    'Not all series devices can support this kind of connection.Please make sure your device supports USBClient.
    'Connect the device via the virtual serial port created by USBClient
    Private Sub btnUSBConnect_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnUSBConnect.Click
        'Dim idwErrorCode As Integer

        'Cursor = Cursors.WaitCursor
        'If btnUSBConnect.Text = "Disconnect" Then
        '    AxCZKEM1.Disconnect()
        '    bIsConnected = False
        '    btnUSBConnect.Text = "Connect"
        '    lblState.Text = "Current State:Disconnected"
        '    Cursor = Cursors.Default
        '    Return
        'End If

        'Dim sCom As String = ""
        'Dim bSearch As Boolean
        'Dim usbcom As New SearchforUSBCom 'new
        'bSearch = usbcom.SearchforCom(sCom)

        'If bSearch = False Then
        '    MsgBox("Can not find the virtual serial port that can be used", MsgBoxStyle.Exclamation, "Error")
        '    Cursor = Cursors.Default
        '    Return
        'End If

        'Dim iPort As Integer
        'For iPort = 1 To 9
        '    If sCom.IndexOf(iPort.ToString()) > -1 Then
        '        Exit For
        '    End If
        'Next

        'iMachineNumber = Convert.ToInt32(txtMachineSN2.Text.Trim())
        'If iMachineNumber = 0 Or iMachineNumber > 255 Then
        '    MsgBox("The Machine Number is invalid!", MsgBoxStyle.Exclamation, "Error")
        '    Cursor = Cursors.Default
        '    Return
        'End If

        'Dim iBaudRate = 115200 '115200 is one possible baudrate value(its value cannot be 0)
        'bIsConnected = AxCZKEM1.Connect_Com(iPort, iMachineNumber, iBaudRate)

        'If bIsConnected = True Then
        '    btnUSBConnect.Text = "Disconnect"
        '    btnUSBConnect.Refresh()
        '    lblState.Text = "Current State:Connected"
        '    AxCZKEM1.RegEvent(iMachineNumber, 65535) 'Here you can register the realtime events that you want 
        'Else
        '    AxCZKEM1.GetLastError(idwErrorCode)
        '    MsgBox("Unable to connect the device,ErrorCode=" & idwErrorCode, MsgBoxStyle.Exclamation, "Error")
        'End If
        'Cursor = Cursors.Default
    End Sub

    Private Sub btnGetGeneralLogData_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnGetGeneralLogData.Click

        If bIsConnected = False Then
            MsgBox("Please connect the device first", MsgBoxStyle.Exclamation, "Error")
            Return
        End If

        Dim sdwEnrollNumber As String = ""
        Dim idwVerifyMode As Integer
        Dim idwInOutMode As Integer
        Dim idwYear As Integer
        Dim idwMonth As Integer
        Dim idwDay As Integer
        Dim idwHour As Integer
        Dim idwMinute As Integer
        Dim idwSecond As Integer
        Dim idwWorkcode As Integer

        Dim idwErrorCode As Integer
        Dim iGLCount = 0
        Dim lvItem As New ListViewItem("Items", 0)
        Dim InOut_Date As String = ""
        Dim InOut_DateTime As Date
        'Dim dttm As DateTime

        Cursor = Cursors.WaitCursor
        lvLogs.Items.Clear()
        AxCZKEM1.EnableDevice(iMachineNumber, False) 'disable the device
        If axCZKEM1.ReadGeneralLogData(iMachineNumber) Then 'read all the attendance records to the memory
            'get records from the memory
            While axCZKEM1.SSR_GetGeneralLogData(iMachineNumber, sdwEnrollNumber, idwVerifyMode, idwInOutMode, idwYear, idwMonth, idwDay, idwHour, idwMinute, idwSecond, idwWorkcode)

                InOut_DateTime = New DateTime(Val(idwYear.ToString()), Val(idwMonth.ToString()), Val(idwDay.ToString()))
                'InOut_Date = idwYear.ToString() & "-" + idwMonth.ToString() & "-" & idwDay.ToString() ' & " " & idwHour.ToString() & ":" & idwMinute.ToString() & ":" & idwSecond.ToString()
                'InOut_DateTime = CDate(InOut_Date)

                If DateDiff(DateInterval.Day, Convert.ToDateTime(msk_Date.Text), InOut_DateTime) = 0 Then
                    iGLCount += 1
                    lvItem = lvLogs.Items.Add(iGLCount.ToString())
                    lvItem.SubItems.Add(sdwEnrollNumber)
                    lvItem.SubItems.Add(idwVerifyMode.ToString())
                    lvItem.SubItems.Add(idwInOutMode.ToString())
                    lvItem.SubItems.Add(idwDay.ToString() & "-" + idwMonth.ToString() & "-" & idwYear.ToString() & " " & idwHour.ToString() & ":" & idwMinute.ToString() & ":" & idwSecond.ToString())
                    lvItem.SubItems.Add(idwYear.ToString() & "~" + idwMonth.ToString() & "~" & idwDay.ToString() & "~" & idwHour.ToString() & "~" & idwMinute.ToString() & "~" & idwSecond.ToString())
                    'lvItem.SubItems.Add(idwWorkcode.ToString())
                End If

            End While

            Cursor = Cursors.Default

            MsgBox("Attendance Log Completed", MsgBoxStyle.OkCancel, "SUCESSFULLY COMPLETED...")

        Else

            Cursor = Cursors.Default
            axCZKEM1.GetLastError(idwErrorCode)
            If idwErrorCode <> 0 Then
                MsgBox("Reading data from terminal failed,ErrorCode: " & idwErrorCode, MsgBoxStyle.Exclamation, "Error")
            Else
                MsgBox("No data from terminal returns!", MsgBoxStyle.Exclamation, "Error")
            End If

        End If

        AxCZKEM1.EnableDevice(iMachineNumber, True) 'enable the device
        Cursor = Cursors.Default
    End Sub

    'Get the count of attendance records in from ternimal.
    Private Sub btnGetDeviceStatus_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnGetDeviceStatus.Click
        If bIsConnected = False Then
            MsgBox("Please connect the device first", MsgBoxStyle.Exclamation, "Error")
            Return
        End If
        Dim idwErrorCode As Integer
        Dim iValue = 0

        AxCZKEM1.EnableDevice(iMachineNumber, False) 'disable the device
        If AxCZKEM1.GetDeviceStatus(iMachineNumber, 6, iValue) = True Then 'Here we use the function "GetDeviceStatus" to get the record's count.The parameter "Status" is 6.
            MsgBox("The count of the AttLogs in the device is " + iValue.ToString(), MsgBoxStyle.Information, "Success")
        Else
            AxCZKEM1.GetLastError(idwErrorCode)
            MsgBox("Operation failed,ErrorCode=" & idwErrorCode, MsgBoxStyle.Exclamation, "Error")
        End If

        AxCZKEM1.EnableDevice(iMachineNumber, True) 'enable the device
    End Sub

    'Clear all attendance records from terminal.
    Private Sub btnClearGLog_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClearGLog.Click

        Dim g As New Password
        g.ShowDialog()

        If Trim(UCase(Common_Procedures.Password_Input)) <> "CL#3222" Then
            MessageBox.Show("Invalid Password", "CLEAR LOG FAILED...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
        End If

        If bIsConnected = False Then
            MsgBox("Please connect the device first", MsgBoxStyle.Exclamation, "Error")
            Return
        End If
        Dim idwErrorCode As Integer

        lvLogs.Items.Clear()
        axCZKEM1.EnableDevice(iMachineNumber, False) 'disable the device
        If axCZKEM1.ClearGLog(iMachineNumber) = True Then
            axCZKEM1.RefreshData(iMachineNumber) 'the data in the device should be refreshed
            MsgBox("All att Logs have been cleared from teiminal!", MsgBoxStyle.Information, "Success")
        Else
            axCZKEM1.GetLastError(idwErrorCode)
            MsgBox("Operation failed,ErrorCode=" & idwErrorCode, MsgBoxStyle.Exclamation, "Error")
        End If

        axCZKEM1.EnableDevice(iMachineNumber, True) 'enable the device
    End Sub
    Private Sub msk_Date_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles msk_Date.KeyDown
        vcbo_KeyDwnVal = e.KeyValue


        vmskOldText = ""
        vmskSelStrt = -1
        If e.KeyCode = 46 Or e.KeyCode = 8 Then
            vmskOldText = msk_Date.Text
            vmskSelStrt = msk_Date.SelectionStart
        End If

    End Sub


    Private Sub msk_Date_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles msk_Date.KeyUp
        Dim vmRetTxt As String = ""
        Dim vmRetSelStrt As Integer = -1

        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            msk_Date.Text = Date.Today
        End If
        If e.KeyCode = 107 Then
            msk_Date.Text = DateAdd("D", 1, Convert.ToDateTime(msk_Date.Text))
        ElseIf e.KeyCode = 109 Then
            msk_Date.Text = DateAdd("D", -1, Convert.ToDateTime(msk_Date.Text))
        End If

        If e.KeyCode = 46 Or e.KeyCode = 8 Then

            Common_Procedures.maskEdit_Date_ON_DelBackSpace(sender, e, vmskOldText, vmskSelStrt)

        End If

    End Sub

    Private Sub dtp_Date_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dtp_Date.TextChanged

        If IsDate(dtp_Date.Text) = True Then

            msk_Date.Text = dtp_Date.Text
            msk_Date.SelectionStart = 0
        End If
    End Sub

    Private Sub msk_Date_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles msk_Date.LostFocus

        If IsDate(msk_Date.Text) = True Then
            If Microsoft.VisualBasic.DateAndTime.Day(Convert.ToDateTime(msk_Date.Text)) <= 31 Or Microsoft.VisualBasic.DateAndTime.Month(Convert.ToDateTime(msk_Date.Text)) <= 31 Then
                If Microsoft.VisualBasic.DateAndTime.Year(Convert.ToDateTime(msk_Date.Text)) <= 2050 And Microsoft.VisualBasic.DateAndTime.Year(Convert.ToDateTime(msk_Date.Text)) >= 2000 Then
                    dtp_Date.Value = Convert.ToDateTime(msk_Date.Text)
                End If
            End If

        End If
    End Sub

    Private Sub dtp_Date_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dtp_Date.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            dtp_Date.Text = Date.Today
        End If
    End Sub

    
End Class