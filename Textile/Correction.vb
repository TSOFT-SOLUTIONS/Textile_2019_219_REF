Imports Excel = Microsoft.Office.Interop.Excel
Public Class Correction
    Implements Interface_MDIActions

    Dim con As New SqlClient.SqlConnection(Common_Procedures.Connection_String)
    Dim New_Entry As Boolean
    Private Prec_ActCtrl As New Control
    Private vcbo_KeyDwnVal As Double
    Private Filter_RowNo As Integer = -1

    Private Sub CLEAR()
        Me.Height = 400
        pnl_Back.Enabled = True
        lbl_RefNo.Text = ""

        lbl_RefNo.ForeColor = Color.Black
        dtp_Time.Text = ""
        dtp_CompletedTime.Text = ""
        cbo_AttendedBy.Text = ""
        Cbo_InformedBy.Text = ""
        Rtxt_CorrectionDetails.Text = ""
        cbo_EntryOrReport.Text = ""
        cbo_Type.Text = "CORRECTION"
        chk_CompletedStatus.Checked = False
        dtp_ComplededDate.Text = ""
        dtp_VerifiedDate.Text = ""
        dtp_VerifiedTime.Text = ""
        chk_VerifiedStatus.Checked = False

        New_Entry = False

    End Sub

    Private Sub ControlGotFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim txtbx As TextBox
        Dim combobx As ComboBox


        On Error Resume Next

        If TypeOf Me.ActiveControl Is TextBox Or TypeOf Me.ActiveControl Is ComboBox Then
            Me.ActiveControl.BackColor = Color.MistyRose ' Color.PaleGreen
            Me.ActiveControl.ForeColor = Color.Blue
        End If

        If TypeOf Me.ActiveControl Is TextBox Then
            txtbx = Me.ActiveControl
            txtbx.SelectAll()
        ElseIf TypeOf Me.ActiveControl Is ComboBox Then
            combobx = Me.ActiveControl
            combobx.SelectAll()
        End If

        Prec_ActCtrl = Me.ActiveControl

    End Sub

    Private Sub ControlLostFocus(ByVal sender As Object, ByVal e As System.EventArgs)

        On Error Resume Next

        If IsNothing(Prec_ActCtrl) = False Then
            If TypeOf Prec_ActCtrl Is TextBox Or TypeOf Prec_ActCtrl Is ComboBox Then
                Prec_ActCtrl.BackColor = Color.White
                Prec_ActCtrl.ForeColor = Color.Black
            End If
        End If

    End Sub

    Private Sub ControlLostFocus1(ByVal sender As Object, ByVal e As System.EventArgs)

        On Error Resume Next

        If IsNothing(Prec_ActCtrl) = False Then
            If TypeOf Prec_ActCtrl Is TextBox Or TypeOf Prec_ActCtrl Is ComboBox Then
                Prec_ActCtrl.BackColor = Color.LightBlue
                Prec_ActCtrl.ForeColor = Color.Blue
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


    Public Sub move_record(ByVal idno As Integer)
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim dt2 As New DataTable

        If Val(idno) = 0 Then Exit Sub

        CLEAR()

        da = New SqlClient.SqlDataAdapter("select a.* from Corrections_Head a where a.Correction_IdNo = " & Str(Val(idno)), con)
        dt = New DataTable
        da.Fill(dt)

        If dt.Rows.Count > 0 Then
            lbl_RefNo.Text = dt.Rows(0).Item("Correction_IdNo").ToString
            dtp_Date.Text = dt.Rows(0).Item("Correction_Date").ToString
            dtp_Time.Text = (dt.Rows(0).Item("Correction_DateTime").ToString)

            cbo_Type.Text = (dt.Rows(0).Item("Correction_type").ToString)
            cbo_EntryOrReport.Text = (dt.Rows(0).Item("Entry_Name").ToString)
            Rtxt_CorrectionDetails.Text = (dt.Rows(0).Item("Correction_Details").ToString)

            Cbo_InformedBy.Text = (dt.Rows(0).Item("Informed_By").ToString)
            cbo_AttendedBy.Text = (dt.Rows(0).Item("Attended_By").ToString)

            If Val(dt.Rows(0).Item("Completed_Status").ToString) = 1 Then
                chk_CompletedStatus.Checked = True
            Else
                chk_CompletedStatus.Checked = False
            End If

            dtp_ComplededDate.Text = dt.Rows(0).Item("Completed_Date").ToString
            If Trim(dt.Rows(0).Item("Completed_Date").ToString) = "" Then
                dtp_ComplededDate.Text = ""
            End If
            dtp_CompletedTime.Text = (dt.Rows(0).Item("Completed_DateTime").ToString)
            If Trim(dt.Rows(0).Item("Completed_DateTime").ToString) = "" Then
                dtp_CompletedTime.Text = ""
            End If

            If Val(dt.Rows(0).Item("Verified_Status").ToString) = 1 Then
                chk_VerifiedStatus.Checked = True
            Else
                chk_VerifiedStatus.Checked = False
            End If
            dtp_VerifiedDate.Text = dt.Rows(0).Item("Verified_Date").ToString
            If Trim(dt.Rows(0).Item("Verified_Date").ToString) = "" Then
                dtp_VerifiedDate.Text = ""
            End If
            dtp_VerifiedTime.Text = (dt.Rows(0).Item("Verified_DateTime").ToString)

        End If

        dt.Dispose()
        da.Dispose()


        If dtp_Date.Enabled And dtp_Date.Visible Then dtp_Date.Focus()

    End Sub


    Public Sub delete_record() Implements Interface_MDIActions.delete_record
        Dim cmd As New SqlClient.SqlCommand
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim Alas_IdNo1 As Integer = 0
        Dim Alas_IdNo2 As Integer = 0


        '   If Common_Procedures.UserRight_Check(Common_Procedures.UR.Accounts_Name_Creation, False, False, True, False) = False Then Exit Sub

        If MessageBox.Show("Do you want to Delete ?", "FOR DELETION....", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.No Then
            Exit Sub
        End If

        If New_Entry = True Then
            MessageBox.Show("This is new entry", "DOES NOT DELETION....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
        End If

        Try


            cmd.Connection = con
            cmd.CommandText = "delete from Corrections_Head where Correction_IdNo = " & Str(Val(lbl_RefNo.Text))
            cmd.ExecuteNonQuery()


            cmd.Dispose()

            new_record()

            MessageBox.Show("Deleted Sucessfully!!!", "FOR DELETION...", MessageBoxButtons.OKCancel, MessageBoxIcon.Information)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Finally
            If dtp_Date.Enabled And dtp_Date.Visible Then dtp_Date.Focus()

        End Try

    End Sub

    Public Sub filter_record() Implements Interface_MDIActions.filter_record
        Dim da As New SqlClient.SqlDataAdapter("select Correction_IdNo,Entry_Name, Correction_Details ,(CASE WHEN Completed_Status = 1 THEN 'COMPLETED' ELSE '' END),(CASE WHEN Verified_Status = 1 THEN 'VERIFIED' ELSE '' END) from Corrections_Head where Correction_IdNo <> 0 order by Correction_IdNo", con)
        Dim dt As New DataTable

        da.Fill(dt)

        With dgv_Filter

            .Columns.Clear()
            .DataSource = dt

            .RowHeadersVisible = False

            .AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells
            .AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill

            .Columns(0).HeaderText = "IDNO"
            .Columns(1).HeaderText = "ENTRY NAME"
            .Columns(2).HeaderText = "CORRECTION DETAILS"
            .Columns(3).HeaderText = "COMP.STATUS"
            .Columns(4).HeaderText = "VERIFY.STATUS"

            .Columns(0).FillWeight = 20
            .Columns(1).FillWeight = 80
            .Columns(2).FillWeight = 200
            .Columns(3).FillWeight = 40
            .Columns(4).FillWeight = 40

        End With

        new_record()

        grp_Filter.Visible = True

        pnl_Back.Enabled = False


        If dgv_Filter.Rows.Count > 0 And Filter_RowNo >= 0 Then
            dgv_Filter.Focus()
            dgv_Filter.CurrentCell = dgv_Filter.Rows(Filter_RowNo).Cells(0)
            dgv_Filter.CurrentCell.Selected = True
        Else
            dgv_Filter.CurrentCell = dgv_Filter.Rows(0).Cells(0)

        End If


        If dgv_Filter.Enabled And dgv_Filter.Visible Then dgv_Filter.Focus()

        Me.Height = 500

        da.Dispose()
    End Sub

    Public Sub insert_record() Implements Interface_MDIActions.insert_record
        '-----  
    End Sub

    Public Sub movefirst_record() Implements Interface_MDIActions.movefirst_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim movid As Integer = 0

        Try
            da = New SqlClient.SqlDataAdapter("select min(Correction_IdNo) from Corrections_Head Where Correction_IdNo <> 0 ", con)
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
            MessageBox.Show(ex.Message, "DOES NOT MOVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Public Sub movelast_record() Implements Interface_MDIActions.movelast_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim movid As Integer = 0

        Try
            da = New SqlClient.SqlDataAdapter("select max(Correction_IdNo) from Corrections_Head Where Correction_IdNo <> 0 ", con)
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
            MessageBox.Show(ex.Message, "DOES NOT MOVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try
    End Sub
    Public Sub movenext_record() Implements Interface_MDIActions.movenext_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim movid As Integer = 0

        Try
            da = New SqlClient.SqlDataAdapter("select min(Correction_IdNo) from Corrections_Head Where Correction_IdNo > " & Str(Val(lbl_RefNo.Text)) & " and Correction_IdNo <> 0 ", con)
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
            MessageBox.Show(ex.Message, "DOES NOT MOVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Public Sub moveprevious_record() Implements Interface_MDIActions.moveprevious_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim movid As Integer = 0

        Try
            da = New SqlClient.SqlDataAdapter("select max(Correction_IdNo) from Corrections_Head Where Correction_IdNo < " & Str(Val(lbl_RefNo.Text)) & " and Correction_IdNo <> 0 ", con)
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
            MessageBox.Show(ex.Message, "DOES NOT MOVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Public Sub new_record() Implements Interface_MDIActions.new_record

        CLEAR()

        New_Entry = True
        lbl_RefNo.ForeColor = Color.Red

        lbl_RefNo.Text = Common_Procedures.get_MaxIdNo(con, "Corrections_Head", "Correction_IdNo", "")

        If dtp_Date.Enabled And dtp_Date.Visible Then dtp_Date.Focus()

    End Sub

    Public Sub open_record() Implements Interface_MDIActions.open_record
        'Dim da As New SqlClient.SqlDataAdapter("select Entry_Name from Corrections_Head order by Entry_Name", con)
        'Dim dt As New DataTable

        'da.Fill(dt)

        'cbo_Open.DataSource = dt
        'cbo_Open.DisplayMember = "Entry_Name"

        'new_record()

        'Me.Height = 500
        'grp_Open.Visible = True
        'pnl_Back.Enabled = False
        'If cbo_Open.Enabled And cbo_Open.Visible Then cbo_Open.Focus()


        Dim cmd As New SqlClient.SqlCommand
        Dim dr As SqlClient.SqlDataReader
        Dim movno As String, inpno As String


        Try

            inpno = InputBox("Enter Ref.No", "FOR FINDING...")


            cmd.Connection = con
            cmd.CommandText = "select Correction_IdNo from Corrections_Head where  Correction_IdNo = " & Val(inpno)
            dr = cmd.ExecuteReader

            movno = ""
            If dr.HasRows Then
                If dr.Read Then
                    If IsDBNull(dr(0).ToString) = False Then
                        movno = dr(0).ToString
                    End If
                End If
            End If

            dr.Close()
            cmd.Dispose()

            If Val(movno) <> 0 Then
                move_record(movno)

            Else
                MessageBox.Show("Ref.No. Does not exists", "DOES NOT FIND...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT FIND...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub
    Private Sub btn_Open_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Open.Click
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim movid As Integer

        da = New SqlClient.SqlDataAdapter("select Correction_IdNo from Corrections_Head where Entry_Name = '" & Trim(cbo_Open.Text) & "'", con)
        da.Fill(dt)

        movid = 0
        If dt.Rows.Count > 0 Then
            If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                movid = Val(dt.Rows(0)(0).ToString)
            End If
        End If

        dt.Dispose()
        da.Dispose()

        If movid <> 0 Then
            move_record(movid)
        Else
            new_record()
        End If

        btn_CloseOpen_Click(sender, e)

    End Sub
    Private Sub dgv_Filter_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgv_Filter.DoubleClick
        Call btn_Filter_Click(sender, e)
    End Sub

    Private Sub dgv_Filter_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_Filter.KeyDown
        If e.KeyValue = 13 Then
            Call btn_Filter_Click(sender, e)
        End If
    End Sub

    Public Sub print_record() Implements Interface_MDIActions.print_record
        '----
    End Sub


    Public Sub save_record() Implements Interface_MDIActions.save_record
        Dim trans As SqlClient.SqlTransaction
        Dim cmd As New SqlClient.SqlCommand
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim V_Sts As Integer = 0
        Dim CmpltdSts As Integer = 0
        Dim VrfdSts As Integer = 0
        Dim CompltnTimetxt As String = ""
        Dim VerfdTimetxt As String = ""


        'If Common_Procedures.UserRight_Check(Common_Procedures.UR.Ledger_Creation, True,New_Entry, False, False) = False Then Exit Sub

        If pnl_Back.Enabled = False Then
            MessageBox.Show("Close Other Windows", "DOES NOT SAVE", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
        End If

        If Trim(cbo_EntryOrReport.Text) = "" Then
            MessageBox.Show("Invalid Name", "DOES NOT SAVE", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If cbo_EntryOrReport.Enabled Then cbo_EntryOrReport.Focus()
            Exit Sub
        End If
        If Trim(cbo_Type.Text) = "" Then
            MessageBox.Show("Invalid Type", "DOES NOT SAVE", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If cbo_Type.Enabled Then cbo_Type.Focus()
            Exit Sub
        End If

        If chk_CompletedStatus.Checked = False And chk_VerifiedStatus.Checked = True Then
            MessageBox.Show("Not Completed the correction", "DOES NOT SAVE", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            chk_VerifiedStatus.Checked = False
            If chk_CompletedStatus.Enabled And chk_CompletedStatus.Visible Then chk_CompletedStatus.Focus()
            Exit Sub
        End If

        CmpltdSts = 0
        If chk_CompletedStatus.Visible = True And chk_CompletedStatus.Enabled = True Then
            If chk_CompletedStatus.Checked = True Then
                CmpltdSts = 1
            End If
        End If

        VrfdSts = 0
        If chk_VerifiedStatus.Visible = True And chk_VerifiedStatus.Enabled = True Then
            If chk_VerifiedStatus.Checked = True Then
                VrfdSts = 1
            End If
        End If

        trans = con.BeginTransaction

        Try

            cmd.Connection = con
            cmd.Transaction = trans

            cmd.Parameters.Clear()
            cmd.Parameters.AddWithValue("@EntryDate", dtp_Date.Value.Date)
            cmd.Parameters.AddWithValue("@EntryDateTime", dtp_Time.Value)


            CompltnTimetxt = ""
            If CmpltdSts = 1 Then
                cmd.Parameters.AddWithValue("@CompletedDate", dtp_ComplededDate.Value.Date)
                cmd.Parameters.AddWithValue("@CompletedDateTime", dtp_CompletedTime.Value)
                CompltnTimetxt = dtp_CompletedTime.Text
            End If

            VerfdTimetxt = ""
            If VrfdSts = 1 Then
                cmd.Parameters.AddWithValue("@VerifiedDate", dtp_VerifiedDate.Value.Date)
                cmd.Parameters.AddWithValue("@VerifiedDateTime", dtp_VerifiedTime.Value)
                VerfdTimetxt = dtp_VerifiedTime.Text
            End If


            If New_Entry = True Then

                lbl_RefNo.Text = Common_Procedures.get_MaxIdNo(con, "Corrections_Head", "Correction_IdNo", "", trans)

                cmd.CommandText = "Insert into Corrections_Head (          Correction_IdNo        ,    Correction_Date     , Correction_DateTime,          Correction_type     ,               Entry_Name              ,           Correction_Details               ,             Informed_By            ,             Attended_By           ,   Completed_Status    ,                         Completed_Date              ,                         Completed_DateTime              ,       Completed_Time_Text     ,    Verified_Status  ,                      Verified_Date               ,                       Verified_DateTime              ,        Verified_Time_Text    ) " & _
                                    "             Values        (" & Str(Val(lbl_RefNo.Text)) & " ,     @EntryDate         ,  @EntryDateTime    , '" & Trim(cbo_Type.Text) & "', '" & Trim(cbo_EntryOrReport.Text) & "', '" & Trim(Rtxt_CorrectionDetails.Text) & "', '" & Trim(Cbo_InformedBy.Text) & "', '" & Trim(cbo_AttendedBy.Text) & "', " & Val(CmpltdSts) & ", " & IIf(CmpltdSts = 1, "@CompletedDate", "Null") & ", " & IIf(CmpltdSts = 1, "@CompletedDateTime", "Null") & ", '" & Trim(CompltnTimetxt) & "', " & Val(VrfdSts) & ", " & IIf(VrfdSts = 1, "@VerifiedDate", "Null") & ", " & IIf(VrfdSts = 1, "@VerifiedDateTime", "Null") & ", '" & Trim(VerfdTimetxt) & "' ) "
                cmd.ExecuteNonQuery()

            Else
                cmd.CommandText = "update Corrections_Head set Correction_Date  = @EntryDate   , Correction_DateTime = @EntryDateTime , Correction_type =  '" & Trim(cbo_Type.Text) & "', Entry_Name   ='" & Trim(cbo_EntryOrReport.Text) & "'  ,   Correction_Details ='" & Trim(Rtxt_CorrectionDetails.Text) & "' ,   Informed_By  ='" & Trim(Cbo_InformedBy.Text) & "' , Attended_By ='" & Trim(cbo_AttendedBy.Text) & "' , Completed_Status =" & Val(CmpltdSts) & ", Completed_Date = " & IIf(CmpltdSts = 1, "@CompletedDate", "Null") & ", Completed_DateTime = " & IIf(CmpltdSts = 1, "@CompletedDateTime", "Null") & ", Completed_Time_Text = '" & Trim(CompltnTimetxt) & "', Verified_Status = " & Val(VrfdSts) & ", Verified_Date = " & IIf(VrfdSts = 1, "@VerifiedDate", "Null") & ", Verified_DateTime = " & IIf(VrfdSts = 1, "@VerifiedDateTime", "Null") & ", Verified_Time_Text = '" & Trim(VerfdTimetxt) & "' where Correction_IdNo = " & Str(Val(lbl_RefNo.Text))
                cmd.ExecuteNonQuery()

            End If

            trans.Commit()

            MessageBox.Show("Saved Sucessfully!!!", "FOR SAVING....", MessageBoxButtons.OKCancel, MessageBoxIcon.Information)

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
            trans.Rollback()
            If InStr(1, Trim(LCase(ex.Message)), "pk_corrections_head") > 0 Then
                MessageBox.Show("Duplicate Entry", "DOES NOT SAVE", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

            Else
                MessageBox.Show(ex.Message, "DOES NOT SAVE", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

            End If

        Finally
            If dtp_Date.Enabled And dtp_Date.Visible Then dtp_Date.Focus()


        End Try

    End Sub



    Private Sub Correction_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        On Error Resume Next
        con.Close()
        con.Dispose()
        Common_Procedures.Last_Closed_FormName = Me.Name
    End Sub

    Private Sub Correction_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If Asc(e.KeyChar) = 27 Then
            If grp_Filter.Visible Then
                btn_CloseFilter_Click(sender, e)

            ElseIf grp_Open.Visible Then
                btn_CloseOpen_Click(sender, e)
            Else
                'If MessageBox.Show("Do you want to Close?", "FOR CLOSING ENTRY...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) <> Windows.Forms.DialogResult.Yes Then
                '    Exit Sub
                'Else
                Me.Close()
                'End If
            End If

        End If

    End Sub

    Private Sub Correction_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        con.Open()
        Me.Top = 50
        'Me.Top = Me.Top - 100

        grp_Open.Left = 6
        grp_Open.Top = 200
        grp_Open.BringToFront()
        grp_Open.Visible = False

        grp_Filter.Left = 10
        grp_Filter.Top = 50
        grp_Filter.Visible = False

        cbo_Type.Items.Clear()
        cbo_Type.Items.Add("NEW")
        cbo_Type.Items.Add("CORRECTION")

        AddHandler dtp_Date.GotFocus, AddressOf ControlGotFocus
        AddHandler dtp_Time.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_EntryOrReport.GotFocus, AddressOf ControlGotFocus
        AddHandler Rtxt_CorrectionDetails.GotFocus, AddressOf ControlGotFocus
        AddHandler Cbo_InformedBy.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_AttendedBy.GotFocus, AddressOf ControlGotFocus
        AddHandler chk_CompletedStatus.GotFocus, AddressOf ControlGotFocus
        AddHandler dtp_ComplededDate.GotFocus, AddressOf ControlGotFocus
        AddHandler dtp_CompletedTime.GotFocus, AddressOf ControlGotFocus
        AddHandler chk_VerifiedStatus.GotFocus, AddressOf ControlGotFocus
        AddHandler dtp_VerifiedDate.GotFocus, AddressOf ControlGotFocus
        AddHandler dtp_VerifiedTime.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Type.GotFocus, AddressOf ControlGotFocus

        AddHandler dtp_Date.LostFocus, AddressOf ControlLostFocus
        AddHandler dtp_Time.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_EntryOrReport.LostFocus, AddressOf ControlLostFocus
        AddHandler Rtxt_CorrectionDetails.LostFocus, AddressOf ControlLostFocus
        AddHandler Cbo_InformedBy.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_AttendedBy.LostFocus, AddressOf ControlLostFocus
        AddHandler chk_CompletedStatus.LostFocus, AddressOf ControlLostFocus
        AddHandler dtp_ComplededDate.LostFocus, AddressOf ControlLostFocus
        AddHandler dtp_CompletedTime.LostFocus, AddressOf ControlLostFocus
        AddHandler chk_VerifiedStatus.LostFocus, AddressOf ControlLostFocus
        AddHandler dtp_VerifiedDate.LostFocus, AddressOf ControlLostFocus
        AddHandler dtp_VerifiedDate.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Type.LostFocus, AddressOf ControlLostFocus

        AddHandler dtp_Date.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler dtp_Time.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler Rtxt_CorrectionDetails.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler chk_CompletedStatus.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler dtp_ComplededDate.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler chk_VerifiedStatus.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler dtp_VerifiedDate.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler dtp_CompletedTime.KeyDown, AddressOf TextBoxControlKeyDown


        AddHandler dtp_Date.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler dtp_Time.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler Rtxt_CorrectionDetails.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler chk_CompletedStatus.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler dtp_ComplededDate.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler chk_VerifiedStatus.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler dtp_VerifiedDate.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler dtp_CompletedTime.KeyPress, AddressOf TextBoxControlKeyPress

        new_record()

    End Sub

    Private Sub cbo_EntryOrReport_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_EntryOrReport.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Corrections_Head", "Entry_Name", "", "(Correction_IdNo = 0)")
    End Sub

    Private Sub cbo_EntryOrReport_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_EntryOrReport.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_EntryOrReport, Nothing, Nothing, "Corrections_Head", "Entry_Name", "", "(Correction_IdNo = 0)")
        If e.KeyCode = 38 And cbo_EntryOrReport.DroppedDown = False Then
            cbo_Type.Focus()
        End If
        If e.KeyCode = 40 And cbo_EntryOrReport.DroppedDown = False Then
            Rtxt_CorrectionDetails.Focus()
        End If
    End Sub

    Private Sub cbo_EntryOrReport_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_EntryOrReport.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_EntryOrReport, Nothing, "Corrections_Head", "Entry_Name", "", "(Correction_IdNo = 0)", False)
        If Asc(e.KeyChar) = 13 Then
            Rtxt_CorrectionDetails.Focus()
        End If

        'Dim selectionStart As Integer = Me.cbo_EntryOrReport.SelectionStart
        'Me.cbo_EntryOrReport.Text = Me.cbo_EntryOrReport.Text.ToUpper()
        'Me.cbo_EntryOrReport.SelectionStart = selectionStart
    End Sub

    Private Sub cbo_Type_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Type.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "", "", "", "")
    End Sub

    Private Sub cbo_Type_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Type.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Type, Nothing, Nothing, "", "", "", "")

        If e.KeyCode = 38 And cbo_Type.DroppedDown = False Then
            dtp_Time.Focus()
        End If
        If e.KeyCode = 40 And cbo_Type.DroppedDown = False Then
            cbo_EntryOrReport.Focus()
        End If
    End Sub

    Private Sub cbo_Type_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Type.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Type, Nothing, "", "", "", "", False)
        If Asc(e.KeyChar) = 13 Then
            cbo_EntryOrReport.Focus()
        End If
    End Sub

    Private Sub dtp_VerifiedTime_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dtp_VerifiedTime.KeyDown
        If e.KeyCode = 38 Then
            dtp_VerifiedDate.Focus()
        End If
        If e.KeyCode = 40 Then
            btn_Save.Focus()
        End If
    End Sub

    Private Sub dtp_VerifiedTime_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dtp_VerifiedTime.KeyPress
        If Asc(e.KeyChar) = 13 Then
            If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                save_record()
            Else
                dtp_Date.Focus()
            End If
        End If
    End Sub
    Private Sub btn_CloseFilter_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_CloseFilter.Click
        Me.Height = 400
        pnl_Back.Enabled = True
        grp_Filter.Visible = False
    End Sub

    Private Sub btn_Filter_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Filter.Click
        Dim idno As Integer

        idno = Val(dgv_Filter.CurrentRow.Cells(0).Value)

        If Val(idno) <> 0 Then
            move_record(idno)
            pnl_Back.Enabled = True
            Filter_RowNo = dgv_Filter.CurrentRow.Index
            grp_Filter.Visible = False
        End If
    End Sub
    Private Sub btn_CloseOpen_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_CloseOpen.Click
        Me.Height = 400
        pnl_Back.Enabled = True
        grp_Open.Visible = False
    End Sub
    Private Sub btn_Save_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Save.Click
        save_record()
    End Sub

    Private Sub btn_Close_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Close.Click
        Me.Close()
    End Sub

    Private Sub chk_CompletedStatus_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chk_CompletedStatus.CheckedChanged
        If chk_CompletedStatus.Checked = True Then
            dtp_ComplededDate.Visible = True
            dtp_CompletedTime.Visible = True
            lbl_Completeddate.Visible = True
            lbl_CompletedTime.Visible = True
            chk_VerifiedStatus.Enabled = True
        Else
            dtp_ComplededDate.Visible = False
            dtp_CompletedTime.Visible = False
            lbl_Completeddate.Visible = False
            lbl_CompletedTime.Visible = False
            chk_VerifiedStatus.Enabled = False
            chk_VerifiedStatus.Checked = False
        End If
    End Sub
    Private Sub chk_VerifiedStatus_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chk_VerifiedStatus.CheckedChanged
        If chk_VerifiedStatus.Checked = True Then
            dtp_VerifiedDate.Visible = True
            dtp_VerifiedTime.Visible = True
            lbl_VerifiedDate.Visible = True
            lbl_VerifiedTime.Visible = True
            chk_CompletedStatus.Enabled = False
        Else
            dtp_VerifiedDate.Visible = False
            dtp_VerifiedTime.Visible = False
            lbl_VerifiedDate.Visible = False
            lbl_VerifiedTime.Visible = False
            chk_CompletedStatus.Enabled = True
        End If
    End Sub

    Private Sub Cbo_InformedBy_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles Cbo_InformedBy.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Corrections_Head", "Informed_By", "", "(Correction_IdNo = 0)")
    End Sub

    Private Sub Cbo_InformedBy_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Cbo_InformedBy.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, Cbo_InformedBy, Nothing, Nothing, "Corrections_Head", "Informed_By", "", "(Correction_IdNo = 0)")
        If e.KeyCode = 38 And Cbo_InformedBy.DroppedDown = False Then
            Rtxt_CorrectionDetails.Focus()
        End If
        If e.KeyCode = 40 And Cbo_InformedBy.DroppedDown = False Then
            cbo_AttendedBy.Focus()
        End If
    End Sub

    Private Sub Cbo_InformedBy_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Cbo_InformedBy.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, Cbo_InformedBy, Nothing, "Corrections_Head", "Informed_By", "", "(Correction_IdNo = 0)", False)
        If Asc(e.KeyChar) = 13 Then
            cbo_AttendedBy.Focus()
        End If
        'Dim selectionStart As Integer = Me.Cbo_InformedBy.SelectionStart
        'Me.Cbo_InformedBy.Text = Me.Cbo_InformedBy.Text.ToUpper()
        'Me.Cbo_InformedBy.SelectionStart = selectionStart
    End Sub
    Private Sub cbo_AttendedBy_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_AttendedBy.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Corrections_Head", "Attended_By", "", "(Correction_IdNo = 0)")
    End Sub

    Private Sub cbo_AttendedBy_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_AttendedBy.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_AttendedBy, Nothing, Nothing, "Corrections_Head", "Attended_By", "", "(Correction_IdNo = 0)")
        If e.KeyCode = 38 And cbo_AttendedBy.DroppedDown = False Then
            Cbo_InformedBy.Focus()
        End If
        If e.KeyCode = 40 And cbo_AttendedBy.DroppedDown = False Then
            chk_CompletedStatus.Focus()
        End If
    End Sub

    Private Sub cbo_AttendedBy_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_AttendedBy.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_AttendedBy, Nothing, "Corrections_Head", "Attended_By", "", "(Correction_IdNo = 0)", False)
        If Asc(e.KeyChar) = 13 Then
            chk_CompletedStatus.Focus()
        End If
        'Dim selectionStart As Integer = Me.cbo_AttendedBy.SelectionStart
        'Me.cbo_AttendedBy.Text = Me.cbo_AttendedBy.Text.ToUpper()
        'Me.cbo_AttendedBy.SelectionStart = selectionStart
    End Sub

    Private Sub chk_VerifiedStatus_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles chk_VerifiedStatus.KeyPress
        If Asc(e.KeyChar) = 13 And chk_VerifiedStatus.Checked = False Then
            If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                save_record()
            Else
                dtp_Date.Focus()
            End If
        End If
    End Sub

    Private Sub btn_Import_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Import.Click
        getExcelData_OneTime()
    End Sub
    Private Sub getExcelData()
        Dim cmd As New SqlClient.SqlCommand

        Dim da As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        'Dim xlApp As Excel.Application
        'Dim xlWorkBook As Excel.Workbook
        'Dim xlWorkSheet As Excel.Worksheet
        Dim RowCnt As Long = 0
        Dim FileName As String = ""
        Dim NewCode As String = ""
        Dim ShfId As Integer = 0
        Dim ItmId As Integer = 0



        Try
            OpenFileDialog1.ShowDialog()
            FileName = OpenFileDialog1.FileName

            If Not IO.File.Exists(FileName) Then
                MessageBox.Show(FileName & " File not found", "File not found...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                Exit Sub
            End If

            'xlApp = New Excel.Application
            'xlWorkBook = xlApp.Workbooks.Open(FileName)
            'xlWorkSheet = xlWorkBook.Worksheets("sheet1")

            'If xlApp Is Nothing Then
            '    MessageBox.Show("Excel is not properly installed!!")
            '    Return
            'End If

            'With xlWorkSheet
            '    'RowCnt = .Range("A" & .Rows.Count).End(Excel.XlDirection.xlUp).Row
            'End With

            ''RowCnt = xlWorkSheet.UsedRange.Rows.Count

            If RowCnt < 1 Then
                MessageBox.Show("Data not found", "Data not found...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                Exit Sub
            End If
            For i = 2 To RowCnt

                cmd.Connection = con

                '    If Val(xlWorkSheet.Cells(i, 12).value) <> 0 Then
                '        cmd.Parameters.Clear()
                '        cmd.Parameters.AddWithValue("@CompletedDate", Convert.ToDateTime(xlWorkSheet.Cells(i, 10).value))
                '        cmd.Parameters.AddWithValue("@CompletedDateTime", Convert.ToDateTime(Trim(xlWorkSheet.Cells(i, 11).value)))

                '        If Val(xlWorkSheet.Cells(i, 16).value) <> 0 Then
                '            cmd.Parameters.AddWithValue("@VerifiedDate", Convert.ToDateTime(xlWorkSheet.Cells(i, 14).value))
                '            cmd.Parameters.AddWithValue("@VerifiedDateTime", Convert.ToDateTime(Trim(xlWorkSheet.Cells(i, 15).value)))

                '            cmd.CommandText = "update Corrections_Head set Verified_Date =@VerifiedDate    ,Verified_DateTime = @VerifiedDateTime  ,Verified_Status =" & Val(xlWorkSheet.Cells(i, 16).value) & "  where Correction_IdNo =" & Trim(xlWorkSheet.Cells(i, 3).value) & ""
                '            cmd.ExecuteNonQuery()
                '        End If

                '        cmd.CommandText = "update Corrections_Head set Completed_Date = @CompletedDate , Completed_DateTime =  @CompletedDateTime ,Completed_Status =" & Val(xlWorkSheet.Cells(i, 12).value) & "  where Correction_IdNo =" & Trim(xlWorkSheet.Cells(i, 3).value) & ""
                '        cmd.ExecuteNonQuery()

                '    End If

            Next i



            'xlWorkBook.Close(False, FileName)
            'xlApp.Quit()

            'xlApp = Nothing

            'System.Runtime.InteropServices.Marshal.ReleaseComObject(xlWorkSheet)
            'System.Runtime.InteropServices.Marshal.ReleaseComObject(xlWorkBook)
            'System.Runtime.InteropServices.Marshal.ReleaseComObject(xlApp)


            MessageBox.Show("Imported Sucessfully!!!", "FOR IMPORTING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Information)
            movelast_record()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT IMPORT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try


    End Sub

    Private Sub Put_DataTo_Excel()
        'Dim da As New SqlClient.SqlDataAdapter
        'Dim dt As New DataTable
        'Dim RwCnt As Integer
        'Dim excel_app As New Excel.Application
        'Dim workbook As Excel.Workbook
        'Dim WorkSheet As Excel.Worksheet
        'Dim sheet As Excel.Worksheet
        'Dim FileName As String


        'If excel_app Is Nothing Then
        '    MessageBox.Show("Excel is not properly installed!!")
        '    Return
        'End If

        'FileName = "Correction_List"
        'workbook = excel_app.Workbooks.Add()
        'workbook = excel_app.Workbooks.Open(FileName:=Common_Procedures.AppPath & "\" & FileName)

        'excel_app.Visible = True

        'sheet = workbook.Sheets("sheet1")


        'Dim sheet_name As String = DateTime.Now.ToString("dd-MM-yyyy")
        'Dim sheet_name As String = DateTime.Now.ToString("Sheet1")


        'If (sheet Is Nothing) Then
        '    sheet = DirectCast(workbook.Sheets.Add(After:=workbook.Sheets(workbook.Sheets.Count), Count:=1, Type:=Excel.XlSheetType.xlWorksheet), Excel.Worksheet)
        '    sheet.Name = sheet_name
        'End If
        'sheet.Cells(1, 1) = "DATE"
        'sheet.Cells(1, 2) = "TIME"
        'sheet.Cells(1, 3) = "REF.NO"
        'sheet.Cells(1, 4) = "CORRECTION TYPE"
        'sheet.Cells(1, 5) = "ENTRY NAME"
        'sheet.Cells(1, 6) = "CORRECTION DETAILS"
        'sheet.Cells(1, 7) = "INFORMED BY"
        'sheet.Cells(1, 8) = "ATTENTED BY"
        'sheet.Cells(1, 9) = "COMPLETED STATUS"
        'sheet.Cells(1, 10) = "COMPLETED DATE"
        'sheet.Cells(1, 11) = "COMPLETED TIME"
        'sheet.Cells(1, 12) = "STS"
        'sheet.Cells(1, 13) = "VERIFIED STATUS"
        'sheet.Cells(1, 14) = "VERIFIED DATE"
        'sheet.Cells(1, 15) = "VERIFIED TIME"
        'sheet.Cells(1, 16) = "STS"

        'da = New SqlClient.SqlDataAdapter("select * from Corrections_Head ", con)
        'da.Fill(dt)
        'RwCnt = dt.Rows.Count - 1
        'If RwCnt > 0 Then
        '    For n = 1 To RwCnt
        '        If IsDBNull(dt.Rows(n)(0).ToString) = False Then

        '            sheet.Cells.Item(n + 1, 1) = Convert.ToDateTime(dt.Rows(n).Item("Correction_Date").ToString)
        '            sheet.Cells.Item(n + 1, 2) = Convert.ToDateTime(dt.Rows(n).Item("Correction_DateTime").ToString)
        '            sheet.Cells.Item(n + 1, 3) = dt.Rows(n).Item("Correction_IdNo").ToString
        '            sheet.Cells.Item(n + 1, 4) = dt.Rows(n).Item("Correction_type").ToString
        '            sheet.Cells.Item(n + 1, 5) = dt.Rows(n).Item("Entry_Name").ToString
        '            sheet.Cells.Item(n + 1, 6) = dt.Rows(n).Item("Correction_Details").ToString
        '            sheet.Cells.Item(n + 1, 7) = dt.Rows(n).Item("Informed_By").ToString
        '            sheet.Cells.Item(n + 1, 8) = dt.Rows(n).Item("Attended_By").ToString

        '            If Val(dt.Rows(n).Item("Completed_Status").ToString) = 1 Then
        '                sheet.Cells.Item(n + 1, 9) = "COMPLETED"
        '                sheet.Cells.Item(n + 1, 10) = Convert.ToDateTime(dt.Rows(n).Item("Completed_Date").ToString)
        '                sheet.Cells.Item(n + 1, 11) = Convert.ToDateTime(dt.Rows(n).Item("Completed_DateTime").ToString)
        '                sheet.Cells.Item(n + 1, 12) = Val(dt.Rows(n).Item("Completed_Status").ToString)
        '            Else
        '                sheet.Cells.Item(n + 1, 9) = ""
        '                sheet.Cells.Item(n + 1, 10) = ""
        '                sheet.Cells.Item(n + 1, 11) = ""
        '                sheet.Cells.Item(n + 1, 12) = 0
        '            End If

        '            If Val(dt.Rows(n).Item("Verified_Status").ToString) = 1 Then
        '                sheet.Cells.Item(n + 1, 13) = "VERIFIED"
        '                sheet.Cells.Item(n + 1, 14) = Convert.ToDateTime(dt.Rows(n).Item("Verified_Date").ToString)
        '                sheet.Cells.Item(n + 1, 15) = Convert.ToDateTime(dt.Rows(n).Item("Verified_DateTime").ToString)
        '                sheet.Cells.Item(n + 1, 16) = Val(dt.Rows(n).Item("Verified_Status").ToString)

        '            Else
        '                sheet.Cells.Item(n + 1, 13) = ""
        '                sheet.Cells.Item(n + 1, 14) = ""
        '                sheet.Cells.Item(n + 1, 15) = ""
        '                sheet.Cells.Item(n + 1, 16) = 0

        '            End If

        '        End If
        '    Next
        'End If

        'dt.Dispose()
        'da.Dispose()



        'Dim header_range As Excel.Range = sheet.Range("A1", "P1")

        'header_range.Font.Bold = True
        'header_range.Font.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Blue)
        'header_range.Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Yellow)


        'Dim values(,) As Integer = {{2, 4, 6}, {3, 6, 9}, {4, 8, 12}, {5, 10, 15}}
        'Dim value_range As Excel.Range = sheet.Range("A2", "C5")
        'value_range.Value2 = values

        'workbook.SaveAs(Common_Procedures.AppPath & "\Correction_List_" & DateTime.Now.ToString("dd-MM-yyyy_(hh-mm)") & ".xlsx")
        'workbook.Close(SaveChanges:=True)

        'excel_app.Quit()

        'excel_app.SaveAs("c:\Correction_List.xls")
        'excel_app.Close()

        'excel_app = Nothing

        'MessageBox.Show("Exported Successfully..")
    End Sub


    Private Sub btn_Export_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Export.Click
        Put_DataTo_Excel()
    End Sub
    Private Sub getExcelData_OneTime()
        Dim cmd As New SqlClient.SqlCommand

        Dim da As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        'Dim xlApp As Excel.Application
        'Dim xlWorkBook As Excel.Workbook
        'Dim xlWorkSheet As Excel.Worksheet
        Dim RowCnt As Long = 0
        Dim FileName As String = ""
        Dim NewCode As String = ""
        Dim ShfId As Integer = 0
        Dim ItmId As Integer = 0



        Try
            OpenFileDialog1.ShowDialog()
            FileName = OpenFileDialog1.FileName

            If Not IO.File.Exists(FileName) Then
                MessageBox.Show(FileName & " File not found", "File not found...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                Exit Sub
            End If

            'xlApp = New Excel.Application
            'xlWorkBook = xlApp.Workbooks.Open(FileName)
            'xlWorkSheet = xlWorkBook.Worksheets("sheet1")

            'If xlApp Is Nothing Then
            '    MessageBox.Show("Excel is not properly installed!!")
            '    Return
            'End If

            'With xlWorkSheet
            '    'RowCnt = .Range("A" & .Rows.Count).End(Excel.XlDirection.xlUp).Row
            'End With

            'RowCnt = xlWorkSheet.UsedRange.Rows.Count

            If RowCnt < 1 Then
                MessageBox.Show("Data not found", "Data not found...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                Exit Sub
            End If
            For i = 2 To RowCnt

                cmd.Connection = con

                'If Val(xlWorkSheet.Cells(i, 11).value) <> 0 Then
                '    cmd.Parameters.Clear()
                '    cmd.Parameters.AddWithValue("@EntryDate", Convert.ToDateTime(xlWorkSheet.Cells(i, 1).value))
                '    cmd.Parameters.AddWithValue("@EntryDateTime", Convert.ToDateTime(Trim(xlWorkSheet.Cells(i, 2).value)))

                '    cmd.Parameters.AddWithValue("@CompletedDate", Convert.ToDateTime(xlWorkSheet.Cells(i, 10).value))
                '    cmd.Parameters.AddWithValue("@CompletedDateTime", Convert.ToDateTime(Trim(xlWorkSheet.Cells(i, 11).value)))


                '    cmd.CommandText = "Insert into Corrections_Head (          Correction_IdNo                      ,    Correction_Date     ,Correction_DateTime    ,Correction_type                               ,    Entry_Name                                ,   Correction_Details                         ,   Informed_By                                , Attended_By                                 ,Completed_Status                           ,   Completed_Date   , Completed_DateTime  , Verified_Status  ) " & _
                '                     "             Values        (" & Str(Val(xlWorkSheet.Cells(i, 3).Value)) & "    ,     @EntryDate         , @EntryDateTime       , '" & Trim(xlWorkSheet.Cells(i, 4).Value) & "', '" & Trim(xlWorkSheet.Cells(i, 5).Value) & "', '" & Trim(xlWorkSheet.Cells(i, 6).Value) & "', '" & Trim(xlWorkSheet.Cells(i, 7).Value) & "','" & Trim(xlWorkSheet.Cells(i, 8).Value) & "'," & Val(xlWorkSheet.Cells(i, 12).Value) & ",  @CompletedDate    ,@CompletedDateTime   ,0) "
                '    cmd.ExecuteNonQuery()


                'End If

            Next i



            'xlWorkBook.Close(False, FileName)
            'xlApp.Quit()

            ''  xlApp = Nothing

            'System.Runtime.InteropServices.Marshal.ReleaseComObject(xlWorkSheet)
            'System.Runtime.InteropServices.Marshal.ReleaseComObject(xlWorkBook)
            'System.Runtime.InteropServices.Marshal.ReleaseComObject(xlApp)


            MessageBox.Show("Imported Sucessfully!!!", "FOR IMPORTING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Information)
            movelast_record()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT IMPORT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try


    End Sub

    Private Sub Rtxt_CorrectionDetails_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles Rtxt_CorrectionDetails.GotFocus
        Rtxt_CorrectionDetails.BackColor = Color.SpringGreen
        Rtxt_CorrectionDetails.ForeColor = Color.Blue
    End Sub

    Private Sub Rtxt_CorrectionDetails_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles Rtxt_CorrectionDetails.LostFocus
        Rtxt_CorrectionDetails.BackColor = Color.White
        Rtxt_CorrectionDetails.ForeColor = Color.Black
    End Sub

    Private Sub cbo_Type_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbo_Type.SelectedIndexChanged

    End Sub

    Private Sub chk_CompletedStatus_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles chk_CompletedStatus.KeyPress
        If Asc(e.KeyChar) = 13 And chk_VerifiedStatus.Enabled = False Then
            If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                save_record()
            Else
                dtp_Date.Focus()
            End If
        End If
    End Sub

End Class