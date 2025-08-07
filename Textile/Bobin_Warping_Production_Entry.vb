Imports System.Data.SqlClient

Public Class Bobin_Warping_Production_Entry
    Implements Interface_MDIActions


    Private con As New SqlClient.SqlConnection(Common_Procedures.Connection_String)
    Private FrmLdSTS As Boolean = False
    Private New_Entry As Boolean = False

    Private Pk_Condition As String = "WBE-"

    Private Prec_ActCtrl As New Control

    Private Insert_Entry As Boolean = False
    Private Filter_Status As Boolean = False
    Public vmskOldText As String = ""
    Public vmskSelStrt As Integer = -1
    Private vcbo_KeyDwnVal As Double
    Private vCbo_ItmNm As String
    Public Sub New()
        FrmLdSTS = True
        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

    End Sub

    Public Sub clear()


        New_Entry = False
        Insert_Entry = False

        lbl_RefNo.Text = ""
        lbl_RefNo.ForeColor = Color.Black

        msk_date.Text = ""
        dtp_date.Text = ""

        vmskOldText = ""
        vmskSelStrt = -1

        cbo_empName.Text = ""
        txt_ends.Text = ""
        txt_Bobin.Text = ""
        txt_meters.Text = ""
        lbl_Reel.Text = ""


        If Filter_Status = False Then

            dtp_Filter_Fromdate.Text = Common_Procedures.Company_FromDate
            dtp_Filter_Todate.Text = Common_Procedures.Company_ToDate


            dgv_Filter_Details.Rows.Clear()

        End If


    End Sub
    Private Sub ControlGotFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim txtbx As TextBox
        Dim combobx As ComboBox
        Dim msktxbx As MaskedTextBox
        On Error Resume Next

        If TypeOf Me.ActiveControl Is TextBox Or TypeOf Me.ActiveControl Is ComboBox Or TypeOf Me.ActiveControl Is Button Or TypeOf Me.ActiveControl Is MaskedTextBox Then
            Me.ActiveControl.BackColor = Color.Lime
            Me.ActiveControl.ForeColor = Color.Blue
        End If

        If TypeOf Me.ActiveControl Is TextBox Then
            txtbx = Me.ActiveControl
            txtbx.SelectAll()
        ElseIf TypeOf Me.ActiveControl Is MaskedTextBox Then
            msktxbx = Me.ActiveControl
            msktxbx.SelectionStart = 0
        ElseIf TypeOf Me.ActiveControl Is ComboBox Then
            combobx = Me.ActiveControl
            combobx.SelectAll()
        End If

        Prec_ActCtrl = Me.ActiveControl

    End Sub

    Private Sub ControlLostFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        On Error Resume Next
        If IsNothing(Prec_ActCtrl) = False Then
            If TypeOf Prec_ActCtrl Is TextBox Or TypeOf Prec_ActCtrl Is ComboBox Or TypeOf Prec_ActCtrl Is MaskedTextBox Then
                Prec_ActCtrl.BackColor = Color.White
                Prec_ActCtrl.ForeColor = Color.Black
            ElseIf TypeOf Prec_ActCtrl Is Button Then
                Prec_ActCtrl.BackColor = Color.DeepPink
                Prec_ActCtrl.ForeColor = Color.White
            End If
        End If
    End Sub

    Private Sub Warping_Bobin_Entry_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated

        If FrmLdSTS = True Then
            lbl_company.Text = ""

            lbl_company.Tag = 0
            Common_Procedures.CompIdNo = 0

            Me.Text = ""

            lbl_company.Text = Common_Procedures.get_Company_From_CompanySelection(con)
            lbl_company.Tag = Val(Common_Procedures.CompIdNo)

            Me.Text = lbl_company.Text

            new_record()

        End If

        FrmLdSTS = False
    End Sub
    Private Sub Grid_Cell_DeSelect()
        On Error Resume Next

        dgv_Filter_Details.CurrentCell.Selected = False
    End Sub


    Private Sub Warping_Bobin_Entry_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Me.Text = ""

        con.Open()

        pnl_Filter.Visible = False
        pnl_Filter.Left = (Me.Width - pnl_Filter.Width) \ 4
        pnl_Filter.Top = (Me.Height - pnl_Filter.Height) \ 2
        pnl_Filter.BringToFront()

        AddHandler dtp_date.GotFocus, AddressOf ControlGotFocus
        AddHandler msk_date.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_empName.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_ends.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Bobin.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_meters.GotFocus, AddressOf ControlGotFocus



        AddHandler dtp_date.LostFocus, AddressOf ControlLostFocus
        AddHandler msk_date.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_empName.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_ends.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Bobin.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_meters.LostFocus, AddressOf ControlLostFocus


        AddHandler txt_ends.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Bobin.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_meters.KeyDown, AddressOf TextBoxControlKeyDown


        AddHandler txt_ends.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Bobin.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_meters.KeyPress, AddressOf TextBoxControlKeyPress


        lbl_company.Text = ""
        lbl_company.Tag = 0
        lbl_company.Visible = False
        Common_Procedures.CompIdNo = 0


        FrmLdSTS = True
        new_record()
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


    Private Sub move_record(ByVal No As String)
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As DataTable = New DataTable
        Dim Newcode As String = ""


        If Val(No) = 0 Then Exit Sub

        Newcode = Trim(Val(lbl_company.Tag)) & "-" & Trim(No) & "/" & Trim(Common_Procedures.FnYearCode)

        clear()
        da = New SqlClient.SqlDataAdapter("Select a.* from Warping_Bobin_Entry_Head a where a.Warping_Bobin_Code = '" & Trim(Newcode) & "' ", con)
        da.Fill(dt)

        If dt.Rows.Count > 0 Then
            lbl_RefNo.Text = dt.Rows(0).Item("Warping_Bobin_No").ToString
            dtp_date.Text = dt.Rows(0).Item("Warping_Bobin_Date").ToString
            msk_date.Text = dtp_date.Text
            cbo_empName.Text = Common_Procedures.Employee_IdNoToName(con, Val(dt.Rows(0).Item("Employee_IdNo").ToString))
            txt_ends.Text = dt.Rows(0).Item("ends").ToString
            txt_Bobin.Text = dt.Rows(0).Item("No_of_Bobins").ToString
            txt_meters.Text = dt.Rows(0).Item("Meters").ToString
            lbl_Reel.Text = dt.Rows(0).Item("Reel").ToString


        End If


        dt.Clear()
        dt.Dispose()
        da.Dispose()


        If (msk_date.Enabled And msk_date.Visible) Then msk_date.Focus()

    End Sub

    Public Sub delete_record() Implements Interface_MDIActions.delete_record
        Dim trans As SqlTransaction
        Dim cmd As New SqlClient.SqlCommand

        If MessageBox.Show("Do you want to delete", "FOR DELETION...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) <> Windows.Forms.DialogResult.Yes Then
            Exit Sub
        End If

        trans = con.BeginTransaction

        Try
            cmd.Connection = con
            cmd.Transaction = trans

            cmd.CommandText = "delete from Warping_Bobin_Entry_Head where Warping_Bobin_No= " & Str(Val(lbl_RefNo.Text)) & ""
            cmd.ExecuteNonQuery()

            trans.Commit()
            new_record()

        Catch ex As Exception

            trans.Rollback()
            MessageBox.Show(ex.Message, "Does not Delete", MessageBoxButtons.OKCancel, MessageBoxIcon.Information)

        End Try

    End Sub

    Public Sub filter_record() Implements Interface_MDIActions.filter_record

        If Filter_Status = False Then

            dtp_Filter_Fromdate.Text = ""
            dtp_Filter_Todate.Text = ""

            dgv_Filter_Details.Rows.Clear()

        End If

        pnl_Filter.Visible = True
        pnl_Filter.Enabled = True
        pnl_Filter.BringToFront()
        Pnl_back.Enabled = False
        If dtp_Filter_Fromdate.Enabled And dtp_Filter_Fromdate.Visible Then dtp_Filter_Fromdate.Focus()


    End Sub

    Public Sub insert_record() Implements Interface_MDIActions.insert_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim movno As String, inpno As String

        Try
            inpno = InputBox("Enter New Ref No.", "For New Reference No. INSERTION..")

            movno = ""

            If dt.Rows.Count > 0 Then
                If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                    movno = Trim(dt.Rows(0)(0).ToString)
                End If
            End If
            dt.Clear()

            If Val(movno) <> 0 Then
                move_record(movno)

            Else
                If Val(inpno) = 0 Then
                    MessageBox.Show("Invalid Ref No.", "DOES NOT INSERT..", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Else
                    new_record()
                    Insert_Entry = True
                    lbl_RefNo.Text = Trim(UCase(inpno))
                End If
            End If


        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT INSERT", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            dt.Dispose()
            da.Dispose()
        End Try
    End Sub

    Public Sub movefirst_record() Implements Interface_MDIActions.movefirst_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim movid As String = ""

        Try
            da = New SqlClient.SqlDataAdapter("select top 1 Warping_Bobin_No from Warping_Bobin_Entry_Head where company_idno = " & Str(Val(lbl_company.Tag)) & " and Warping_Bobin_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "%' Order by for_orderby , Warping_Bobin_No ", con)
            dt = New DataTable
            da.Fill(dt)
            movid = ""

            If (dt.Rows.Count > 0) Then
                If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                    movid = dt.Rows(0)(0).ToString

                End If
            End If
            If Val(movid) <> 0 Then move_record(movid)

            dt.Dispose()
            da.Dispose()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DO NOT SAVE......", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try
    End Sub

    Public Sub movelast_record() Implements Interface_MDIActions.movelast_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim movid As String = ""

        Try
            da = New SqlClient.SqlDataAdapter("select top 1 Warping_Bobin_No from Warping_Bobin_Entry_Head where company_idno = " & Str(Val(lbl_company.Tag)) & " and Warping_Bobin_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "%' Order by for_orderby desc, Warping_Bobin_No desc", con)
            dt = New DataTable
            da.Fill(dt)
            movid = ""

            If (dt.Rows.Count > 0) Then
                If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                    movid = dt.Rows(0)(0).ToString


                End If
            End If
            If Val(movid) <> 0 Then move_record(movid)

            dt.Dispose()
            da.Dispose()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DO NOT SAVE......", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Public Sub movenext_record() Implements Interface_MDIActions.movenext_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim movid As String = ""

        Dim OrdByNo As String = ""
        Try
            OrdByNo = Common_Procedures.OrderBy_CodeToValue(Trim(lbl_RefNo.Text))

            da = New SqlClient.SqlDataAdapter("select top 1 Warping_Bobin_No from Warping_Bobin_Entry_Head where for_orderby > " & Str(Val(OrdByNo)) & " and Company_idno = " & Str(Val(lbl_company.Tag)) & " and Warping_Bobin_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "%' Order by for_orderby , Warping_Bobin_No ", con)

            dt = New DataTable
            da.Fill(dt)

            movid = ""
            If dt.Rows.Count > 0 Then
                If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                    movid = dt.Rows(0)(0).ToString
                End If
            End If

            If Val(movid) <> 0 Then move_record(movid)

            dt.Dispose()
            da.Dispose()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT MOVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try
    End Sub

    Public Sub moveprevious_record() Implements Interface_MDIActions.moveprevious_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim movid As String = ""
        Dim OrdByNo As String = ""
        Try
            OrdByNo = Common_Procedures.OrderBy_CodeToValue(Trim(lbl_RefNo.Text))

            da = New SqlClient.SqlDataAdapter("select top 1 Warping_Bobin_No from Warping_Bobin_Entry_Head where for_orderby < " & Str(Val(OrdByNo)) & " and Company_idno = " & Str(Val(lbl_company.Tag)) & " and Warping_Bobin_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "%' Order by for_orderby desc, Warping_Bobin_No desc", con)

            dt = New DataTable
            da.Fill(dt)



            movid = ""
            If dt.Rows.Count > 0 Then
                If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                    movid = (dt.Rows(0)(0).ToString)
                End If
            End If

            If Val(movid) <> 0 Then move_record(movid)

            dt.Dispose()
            da.Dispose()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT MOVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Public Sub new_record() Implements Interface_MDIActions.new_record
        clear()

        New_Entry = True
        lbl_RefNo.ForeColor = Color.Red

        lbl_RefNo.Text = Common_Procedures.get_MaxCode(con, "Warping_Bobin_Entry_Head", "Warping_Bobin_Code", "for_orderby", "", Val(lbl_company.Tag), Common_Procedures.FnYearCode)

    End Sub

    Public Sub open_record() Implements Interface_MDIActions.open_record
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim movno As String, inpno As String

        Try

            inpno = InputBox("Enter Ref No.", "FOR FINDING...")

            movno = inpno

            If Val(movno) <> 0 Then
                move_record(movno)
            Else
                MessageBox.Show("Ref No. does Not exists", "DOES Not FIND...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES Not FIND...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Finally
            Dt.Dispose()
            Da.Dispose()

            If dtp_date.Enabled And dtp_date.Visible Then dtp_date.Focus()

        End Try

    End Sub

    Public Sub print_record() Implements Interface_MDIActions.print_record
        ''-------------
    End Sub

    Private Sub Reel_Calculation()
        Dim Ends As Single = 0
        Dim Meters As Single = 0
        Dim Bobin As Single
        Dim Reel As Single

        Ends = Val(txt_ends.Text)
        Meters = Val(txt_meters.Text)
        Bobin = Val(txt_Bobin.Text)
        Reel = Val(lbl_Reel.Text)

        Reel = ((Ends * Meters * Bobin) / 3150) * 31
        lbl_Reel.Text = Val(Reel)

    End Sub

    Public Sub save_record() Implements Interface_MDIActions.save_record
        Dim trans As SqlClient.SqlTransaction
        Dim cmd As New SqlClient.SqlCommand
        Dim newcode As String = ""
        Dim vforOrdby As String = ""
        Dim vEmp_IdNo As String = ""

        If Val(lbl_company.Tag) = 0 Then
            MessageBox.Show("Invalid Company Selection", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If
        If Pnl_back.Enabled = False Then
            MessageBox.Show("Close Other Windows", "DOES Not SAVE", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
        End If

        vEmp_IdNo = Common_Procedures.Employee_NameToIdNo(con, cbo_empName.Text)
        If vEmp_IdNo = 0 Then
            MessageBox.Show("Invalid  Employee name", "Do Not SAVE", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            cbo_empName.Focus()
            Exit Sub
        End If


        trans = con.BeginTransaction
        Try
            cmd.Connection = con
            cmd.Transaction = trans

            cmd.Parameters.Clear()
            cmd.Parameters.AddWithValue("@Refdate", Convert.ToDateTime(msk_date.Text))


            If Insert_Entry = True Or New_Entry = False Then
                newcode = Trim(Val(lbl_company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            Else
                lbl_RefNo.Text = Common_Procedures.get_MaxCode(con, "Warping_Bobin_Entry_Head", "Warping_Bobin_Code", "for_orderby", "", Val(lbl_company.Tag), Common_Procedures.FnYearCode, trans)
                newcode = Trim(Val(lbl_company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            End If

            vforOrdby = Val(Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text))

            If New_Entry = True Then
                cmd.CommandText = "INsert into Warping_Bobin_Entry_Head   ( Warping_Bobin_Code    ,                 Company_IdNo     ,                Warping_Bobin_No          ,            for_OrderBy     , Warping_Bobin_Date,                   Employee_IdNo    ,                          Ends          ,                    No_Of_Bobins     ,        Meters     ,                    Reel       ) " &
                                        " Values                       ('" & Trim(newcode) & "', " & Str(Val(lbl_company.Tag)) & ",  '" & Trim(lbl_RefNo.Text) & "', " & Str(Val(vforOrdby)) & ",             @RefDate ,                 " & Val(vEmp_IdNo) & ",      " & Str(txt_ends.Text) & ",              '" & Trim(txt_Bobin.Text) & "' ,      '" & Trim(txt_meters.Text) & "'  ,      '" & Trim(lbl_Reel.Text) & "')"
                cmd.ExecuteNonQuery()

            Else
                cmd.CommandText = ("update Warping_Bobin_Entry_Head set Warping_Bobin_Date = @RefDate, Employee_IdNo = " & Val(vEmp_IdNo) & ",Ends=" & Str(Val(txt_ends.Text)) & ", No_Of_Bobins ='" & Trim(txt_Bobin.Text) & "' ,  Meters =  '" & Trim(txt_meters.Text) & "' ,     Reel ='" & Trim(lbl_Reel.Text) & "'   where Warping_Bobin_Code = '" & Trim(newcode) & "'")
                cmd.ExecuteNonQuery()
            End If


            trans.Commit()
            MessageBox.Show("Saved Successfully", "FOR SAVING", MessageBoxButtons.OK, MessageBoxIcon.Information)


            If (New_Entry = True) Then
                new_record()
            Else
                move_record(lbl_RefNo.Text)
            End If

        Catch ex As Exception
            trans.Rollback()
            If InStr(1, Trim(LCase(ex.Message)), Trim(LCase("IX_Warping_Bobin_Entry_Head"))) > 0 Then
                MessageBox.Show("Duplicate Entry", "Do Not Save", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Else

                MessageBox.Show(ex.Message, "DOES NOT SAVE", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

            End If

        End Try

    End Sub


    Private Sub Warping_Bobin_Entry_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        On Error Resume Next
        con.Close()
        con.Dispose()
        Common_Procedures.Last_Closed_FormName = Me.Name
    End Sub


    Private Sub btn_close_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_close.Click
        Me.Close()

    End Sub

    Private Sub btn_Save_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Save.Click
        save_record()

    End Sub

    Private Sub cbo_empName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_empName.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "PayRoll_Employee_Head", "Employee_Name", "", "(Employee_Name_idno = 0)")
    End Sub

    Private Sub cbo_empName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_empName.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_empName, msk_date, txt_ends, "PayRoll_Employee_Head", "Employee_Name", "", "(Employee_Name_idno = 0)")
    End Sub

    Private Sub cbo_empName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_empName.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_empName, txt_ends, "PayRoll_Employee_Head", "Employee_Name", "", "(Employee_Name_idno = 0)")
    End Sub

    Private Sub cbo_empName_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_empName.KeyUp
        If e.Control = False And e.KeyValue = 17 Then

            Dim f As New Payroll_Employee_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_empName.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()
        End If
    End Sub

    Private Sub msk_Date_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles msk_date.KeyDown

        vcbo_KeyDwnVal = e.KeyValue
        vmskOldText = ""
        vmskSelStrt = -1
        If e.KeyCode = 46 Or e.KeyCode = 8 Then
            vmskOldText = msk_date.Text
            vmskSelStrt = msk_date.SelectionStart
        End If

        If e.KeyCode = 40 Then
            cbo_empName.Focus()
        End If

    End Sub

    Private Sub msk_date_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles msk_date.KeyPress
        If Trim(UCase(e.KeyChar)) = "D" Then
            msk_date.Text = Date.Today
            msk_date.SelectionStart = 0
        End If
        If Asc(e.KeyChar) = 13 Then
            e.Handled = True
            cbo_empName.Focus()
        End If
    End Sub

    Private Sub msk_date_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles msk_date.KeyUp
        Dim vmRetTxt As String = ""
        Dim vmRetSelStrt As Integer = -1

        If e.KeyCode = 107 Then
            msk_date.Text = DateAdd("D", 1, Convert.ToDateTime(msk_date.Text))
            msk_date.SelectionStart = 0
        ElseIf e.KeyCode = 109 Then
            msk_date.Text = DateAdd("D", -1, Convert.ToDateTime(msk_date.Text))
            msk_date.SelectionStart = 0
        End If

        If e.KeyCode = 46 Or e.KeyCode = 8 Then
            Common_Procedures.maskEdit_Date_ON_DelBackSpace(sender, e, vmskOldText, vmskSelStrt)
        End If
    End Sub


    Private Sub msk_Date_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles msk_date.LostFocus

        If IsDate(msk_date.Text) = True Then
            If Microsoft.VisualBasic.DateAndTime.Day(Convert.ToDateTime(msk_date.Text)) <= 31 Or Microsoft.VisualBasic.DateAndTime.Month(Convert.ToDateTime(msk_date.Text)) <= 31 Then
                If Microsoft.VisualBasic.DateAndTime.Year(Convert.ToDateTime(msk_date.Text)) <= 2050 And Microsoft.VisualBasic.DateAndTime.Year(Convert.ToDateTime(msk_date.Text)) >= 2000 Then
                    dtp_date.Value = Convert.ToDateTime(msk_date.Text)
                End If
            End If

        End If
    End Sub

    Private Sub dtp_Date_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dtp_date.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            dtp_date.Text = Date.Today
        End If
    End Sub
    Private Sub dtp_Date_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dtp_date.TextChanged

        If IsDate(dtp_date.Text) = True Then

            msk_date.Text = dtp_date.Text
            msk_date.SelectionStart = 0
        End If
    End Sub

    Private Sub Warping_Bobin_Entry_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress

        Try
            If Asc(e.KeyChar) = 27 Then

                'If pnl_Filter.Visible = True Then
                '    btn_filter_close_Click_1(sender, e)
                '    Exit Sub

                'Else
                '    Close_Form()
                Me.Close()

            End If

            ' End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT CLOSE...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub Close_Form()
        Try

            lbl_company.Tag = 0
            lbl_company.Text = ""
            Me.Text = ""
            Common_Procedures.CompIdNo = 0

            lbl_company.Text = Common_Procedures.Show_CompanySelection_On_FormClose(con)
            lbl_company.Tag = Val(Common_Procedures.CompIdNo)
            Me.Text = lbl_company.Text
            If Val(Common_Procedures.CompIdNo) = 0 Then

                Me.Close()

            Else

                new_record()

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT CLOSE...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub


    Private Sub Warping_Bobin_Entry_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = 115 Then
            If e.Shift = True Then filter_record() Else open_record()
        End If

        If e.Shift = True And e.KeyCode = 115 Then filter_record()
    End Sub
    Public Sub open_filterEntry()
        Dim movno As String

        On Error Resume Next

        movno = Trim(dgv_Filter_Details.CurrentRow.Cells(0).Value)

        If Val(movno) <> 0 Then
            Filter_Status = True
            move_record(movno)
            Pnl_back.Enabled = True
            pnl_Filter.Visible = False
        End If
    End Sub


    Private Sub dgv_Filter_Details_CellDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Filter_Details.CellDoubleClick
        open_filterEntry()
    End Sub

    Private Sub dgv_Filter_Details_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_Filter_Details.KeyDown
        If e.KeyCode = 13 Then
            open_filterEntry()
        End If
    End Sub
    Private Sub cbo_Filter_EmpName_GotFocus(ByVal sender As Object, ByVal e As EventArgs) Handles cbo_Filter_EmpName.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "( Ledger_Type = 'Employee' )", "(Ledger_idno = 0)")

    End Sub

    Private Sub cbo_Filter_EmpName_KeyDown(ByVal sender As Object, ByVal e As KeyEventArgs) Handles cbo_Filter_EmpName.KeyDown
        'vcbo_KeyDwnVal = e.KeyValue

        'Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_EmpName, dtp_Filter_Todate, dtp_date, "PayRoll_Employee_Head", "Employee_Name", "", "(Employee_idno = 0)")

        'If (e.KeyValue = 38 And cbo_empName.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
        '    dtp_Filter_Todate.Focus()
        'End If

        'If (e.KeyValue = 40 And cbo_empName.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
        '    btn_Filter_Show.Focus()
        'End If
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_EmpName, dtp_Filter_Todate, "", "Ledger_AlaisHead", "Ledger_DisplayName", "( Ledger_Type = 'Employee' )", "(Ledger_idno = 0)")
    End Sub


    Private Sub cbo_Filter_EmpName_KeyPress(ByVal sender As Object, ByVal e As KeyPressEventArgs) Handles cbo_Filter_EmpName.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Filter_EmpName, dgv_Filter_Details, "Ledger_AlaisHead", "Ledger_DisplayName", "( Ledger_Type = 'Employee' )", "(Ledger_idno = 0)")

    End Sub
    Private Sub btn_Filter_Show_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btn_Filter_Show.Click

        Dim da As New SqlClient.SqlDataAdapter

        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim Emp_IdNo As Integer

        Dim n As Integer
        Dim condt As String = ""

        Try
            condt = ""
            Emp_IdNo = 0

            If IsDate(dtp_Filter_Fromdate.Value) = True And IsDate(dtp_Filter_Todate.Value) = True Then
                condt = "a.Warping_Bobin_Date between '" & Trim(Format(dtp_Filter_Fromdate.Value, "MM/dd/yyyy")) & "' and '" & Trim(Format(dtp_Filter_Todate.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_Filter_Fromdate.Value) = True Then
                condt = "a.Warping_Bobin_Date = '" & Trim(Format(dtp_Filter_Fromdate.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_Filter_Todate.Value) = True Then
                condt = "a.Warping_Bobin_Date = '" & Trim(Format(dtp_Filter_Todate.Value, "MM/dd/yyyy")) & "' "
            End If

            If Trim(cbo_Filter_EmpName.Text) <> "" Then
                Emp_IdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Filter_EmpName.Text)
                'Emp_IdNo = Common_Procedures.Employee_IdNoToName(con, cbo_empName.Text)

            End If

            If Val(Emp_IdNo) <> 0 Then
                condt = condt & IIf(Trim(condt) <> "", " and ", "") & " (b.Employee_IdNo = " & Str(Val(Emp_IdNo)) & "  )"   ' or a.Party_Idno = " & Str(Val(Led_IdNo)) & ")"
            End If

            da = New SqlClient.SqlDataAdapter("select a.*, b.Employee_Name from Warping_Bobin_Entry_Head a  inner join PayRoll_Employee_Head b on a.Employee_idNo =b.Employee_idno where a.company_IdNo = " & Str(Val(lbl_company.Tag)) & " and  a.Warping_Bobin_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "' " & IIf(Trim(condt) <> "", " and ", "") & condt & " Order by a.for_orderby, a.Warping_Bobin_No", con)
            da.Fill(dt2)

            dgv_Filter_Details.Rows.Clear()

            If dt2.Rows.Count > 0 Then

                For i = 0 To dt2.Rows.Count - 1

                    n = dgv_Filter_Details.Rows.Add()

                    dgv_Filter_Details.Rows(n).Cells(0).Value = i + 1
                    dgv_Filter_Details.Rows(n).Cells(1).Value = dt2.Rows(i).Item("Employee_Name").ToString
                    dgv_Filter_Details.Rows(n).Cells(2).Value = dt2.Rows(i).Item("Ends").ToString
                    dgv_Filter_Details.Rows(n).Cells(3).Value = dt2.Rows(i).Item("No_of_Bobins").ToString
                    dgv_Filter_Details.Rows(n).Cells(4).Value = dt2.Rows(i).Item("Meters").ToString
                    dgv_Filter_Details.Rows(n).Cells(5).Value = dt2.Rows(i).Item("Reel").ToString

                Next i

            End If

            dt2.Clear()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT FILTER.....", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            dt1.Dispose()
            dt2.Dispose()
            da.Dispose()

        End Try

        If dgv_Filter_Details.Rows.Count > 0 Then
            If dgv_Filter_Details.Visible And dgv_Filter_Details.Enabled Then dgv_Filter_Details.Focus()
        Else
            dtp_Filter_Fromdate.Focus()
        End If

    End Sub

    Private Sub btn_filter_close_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_filter_close.Click
        Pnl_back.Enabled = True
        pnl_Filter.Visible = False
        Filter_Status = False
    End Sub

    Private Sub txt_ends_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_ends.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
            e.Handled = True
        End If
    End Sub

    Private Sub txt_ends_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txt_ends.TextChanged
        Reel_Calculation()
    End Sub

    Private Sub txt_Bobin_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Bobin.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
            e.Handled = True
        End If
    End Sub

    Private Sub txt_Bobin_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txt_Bobin.TextChanged
        Reel_Calculation()
    End Sub

    Private Sub txt_meters_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_meters.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
            e.Handled = True
        End If
    End Sub

    Private Sub txt_meters_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txt_meters.TextChanged
        Reel_Calculation()
    End Sub

    Private Sub lbl_Reel_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbl_Reel.TextChanged
        Reel_Calculation()
    End Sub
End Class