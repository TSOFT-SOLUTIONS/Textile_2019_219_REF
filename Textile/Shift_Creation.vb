Public Class Shift_Creation
    Implements Interface_MDIActions

    Private con As New SqlClient.SqlConnection(Common_Procedures.Connection_String)
    Private New_Entry As Boolean = False

    Private Sub clear()

        New_Entry = False

        pnl_Back.Enabled = True
        grp_Find.Visible = False
        grp_Filter.Visible = False

        'Me.Height = 284

        txt_IdNo.Text = ""
        txt_IdNo.ForeColor = Color.Black

        txt_Name.Text = ""
        cbo_Find.Text = ""
        'dgv_Filter.Rows.Clear()

        msk_InTimeshift.Text = ""
        msk_OutTimeshift.Text = ""

        lbl_TotHours.Text = ""
        lbl_Minutes.Text = ""

    End Sub

    Public Sub move_record(ByVal idno As Integer)
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable

        If Val(idno) = 0 Then Exit Sub

        clear()

        da = New SqlClient.SqlDataAdapter("select * from Shift_Head a where Shift_IdNo = " & Str(Val(idno)), con)
        da.Fill(dt)

        If dt.Rows.Count > 0 Then

            txt_IdNo.Text = dt.Rows(0).Item("Shift_IdNo").ToString
            txt_Name.Text = dt.Rows(0).Item("Shift_Name").ToString
            msk_InTimeshift.Text = dt.Rows(0).Item("In_Time_Shift").ToString
            msk_OutTimeshift.Text = dt.Rows(0).Item("Out_Time_Shift").ToString
            lbl_TotHours.Text = Format(Val(dt.Rows(0).Item("Total_Hours").ToString), "##########0.00")
            lbl_Minutes.Text = Format(Val(dt.Rows(0).Item("Total_Minutes").ToString), "##########0.00")

        End If

        dt.Dispose()
        da.Dispose()

        If txt_Name.Enabled And txt_Name.Visible Then txt_Name.Focus()
    End Sub

    Private Sub Area_Creation_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Me.Height = 284 ' 197

        con.Open()

        new_record()

    End Sub

    Private Sub Area_Creation_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If Asc(e.KeyChar) = 27 Then
            If grp_Find.Visible Then
                btnClose_Click(sender, e)
            ElseIf grp_Filter.Visible Then
                btn_CloseFilter_Click(sender, e)
            Else
                Me.Close()
            End If
        End If
    End Sub

    Private Sub Area_Creation_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        con.Close()
        con.Dispose()
    End Sub

    Private Sub btn_Close_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Close.Click
        Me.Close()
        Me.Dispose()
    End Sub

    Public Sub delete_record() Implements Interface_MDIActions.delete_record
        Dim cmd As New SqlClient.SqlCommand

        If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.Area_Creation, "~L~") = 0 And InStr(Common_Procedures.UR.Area_Creation, "~D~") = 0 Then MessageBox.Show("You have No Rights to Delete", "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error) : Exit Sub

        If MessageBox.Show("Do you want to Delete ?", "FOR DELETION....", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.No Then
            Exit Sub
        End If

        Try

            'da = New SqlClient.SqlDataAdapter("select count(*) from item_head where Shift_IdNo = " & Str(Val(txt_IdNo.Text)), con)
            'dt = New DataTable
            'da.Fill(dt)
            'If dt.Rows.Count > 0 Then
            '    If IsDBNull(dt.Rows(0)(0).ToString) = False Then
            '        If Val(dt.Rows(0)(0).ToString) > 0 Then
            '            MessageBox.Show("Already used this ItemGroup", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            '            Exit Sub
            '        End If
            '    End If
            'End If

            'da = New SqlClient.SqlDataAdapter("select count(*) from Sales_Details where Shift_IdNo = " & Str(Val(txt_IdNo.Text)), con)
            'dt = New DataTable
            'da.Fill(dt)
            'If dt.Rows.Count > 0 Then
            '    If IsDBNull(dt.Rows(0)(0).ToString) = False Then
            '        If Val(dt.Rows(0)(0).ToString) > 0 Then
            '            MessageBox.Show("Already used this ItemGroup", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            '            Exit Sub
            '        End If
            '    End If
            'End If

            cmd.Connection = con
            cmd.CommandText = "delete from Shift_Head where Shift_IdNo = " & Str(Val(txt_IdNo.Text))

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
        Dim da As New SqlClient.SqlDataAdapter("select Shift_IdNo, Shift_Name from Shift_Head where Shift_IdNo <> 0 order by Shift_IdNo", con)
        Dim dt As New DataTable

        da.Fill(dt)

        With dgv_Filter

            .Columns.Clear()
            .DataSource = dt

            .RowHeadersVisible = False

            .AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells
            .AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill

            .Columns(0).HeaderText = "IDNO"
            .Columns(1).HeaderText = "SHIFT NAME"

            .Columns(0).FillWeight = 40
            .Columns(1).FillWeight = 160

        End With

        new_record()

        grp_Filter.Visible = True
        grp_Filter.Left = grp_Find.Left
        grp_Filter.Top = grp_Find.Top

        pnl_Back.Enabled = False

        If dgv_Filter.Enabled And dgv_Filter.Visible Then dgv_Filter.Focus()

        Me.Height = 600 ' 400

        dt.Dispose()
        da.Dispose()

    End Sub

    Public Sub movefirst_record() Implements Interface_MDIActions.movefirst_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim movid As Integer = 0

        Try
            da = New SqlClient.SqlDataAdapter("select min(Shift_IdNo) from Shift_Head Where Shift_IdNo <> 0", con)
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
        Dim cmd As New SqlClient.SqlCommand
        Dim dr As SqlClient.SqlDataReader
        Dim movid As Integer

        Try
            cmd.Connection = con
            cmd.CommandText = "select max(Shift_IdNo) from Shift_Head WHERE Shift_IdNo <> 0"

            dr = cmd.ExecuteReader

            movid = 0
            If dr.HasRows Then
                If dr.Read() Then
                    If IsDBNull(dr(0).ToString) = False Then
                        movid = Val(dr(0).ToString)
                    End If
                End If
            End If

            dr.Close()
            cmd.Dispose()

            If movid <> 0 Then move_record(movid)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT MOVE....", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub

        End Try

    End Sub

    Public Sub movenext_record() Implements Interface_MDIActions.movenext_record
        Dim cmd As New SqlClient.SqlCommand
        Dim dr As SqlClient.SqlDataReader
        Dim movid As Integer

        Try
            cmd.Connection = con
            cmd.CommandText = "select min(Shift_IdNo) from Shift_Head where Shift_IdNo > " & Str(Val(txt_IdNo.Text))

            dr = cmd.ExecuteReader()

            movid = 0
            If dr.HasRows Then
                If dr.Read() Then
                    If IsDBNull(dr(0).ToString) = False Then
                        movid = Val(dr(0).ToString)
                    End If
                End If
            End If

            dr.Close()
            cmd.Dispose()

            If movid <> 0 Then move_record(movid)

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
            da = New SqlClient.SqlDataAdapter("select max(Shift_IdNo) from Shift_Head where Shift_IdNo < " & Str(Val(txt_IdNo.Text)) & " and Shift_IdNo <> 0 ", con)
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
        'Dim cmd As New SqlClient.SqlCommand
        'Dim dr As SqlClient.SqlDataReader
        'Dim newno As Integer

        clear()

        New_Entry = True
        txt_IdNo.ForeColor = Color.Red

        txt_IdNo.Text = Common_Procedures.get_MaxIdNo(con, "Shift_Head", "Shift_IdNo", "")

        'cmd.Connection = con
        'cmd.CommandText = "select max(Shift_IdNo) from Shift_Head"

        'dr = cmd.ExecuteReader

        'newno = 0
        'If dr.HasRows Then
        '    If dr.Read() Then
        '        If IsDBNull(dr(0).ToString) = False Then
        '            newno = Val(dr(0).ToString)
        '        End If
        '    End If
        'End If

        'dr.Close()
        'cmd.Dispose()

        'txt_IdNo.Text = Val(newno) + 1

        If txt_Name.Enabled And txt_Name.Visible Then txt_Name.Focus()

    End Sub

    Public Sub open_record() Implements Interface_MDIActions.open_record
        Dim da As New SqlClient.SqlDataAdapter("select Shift_Name from Shift_Head order by Shift_Name", con)
        Dim dt As New DataTable

        da.Fill(dt)

        cbo_Find.DataSource = dt
        cbo_Find.DisplayMember = "Shift_Name"

        new_record()

        grp_Find.Visible = True
        pnl_Back.Enabled = False

        If cbo_Find.Enabled And cbo_Find.Visible Then cbo_Find.Focus()
        Me.Height = 550 ' 355

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

        Dim Shft_Hours As Double = 0
        Dim Hr As Long = 0, Mins As Long = 0
        Dim Shft_Mins As Double = 0


        ' If Common_Procedures.UserRight_Check(Common_Procedures.UR.Area_Creation, New_Entry) = False Then Exit Sub

        If Trim(txt_Name.Text) = "" Then
            MessageBox.Show("Invalid Name", "DOES NOT SAVE", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        Sur = Common_Procedures.Remove_NonCharacters(Trim(txt_Name.Text))

        trans = con.BeginTransaction

        Try
            cmd.Connection = con
            cmd.Transaction = trans


        
            Shft_Hours = Val(lbl_TotHours.Text)

            Hr = Int(Shft_Hours)
            Mins = (Shft_Hours - Hr) * 100
            Shft_Mins = (Hr * 60) + Mins


            If New_Entry = True Then

                txt_IdNo.Text = Common_Procedures.get_MaxIdNo(con, "Shift_Head", "Shift_IdNo", "", trans)

                cmd.CommandText = "Insert into Shift_Head(Shift_IdNo, Shift_Name, In_Time_Shift ,Out_Time_Shift , Total_Hours , Total_Minutes) values (" & Str(Val(txt_IdNo.Text)) & ", '" & Trim(txt_Name.Text) & "',  '" & Trim(msk_InTimeshift.Text) & "' ,'" & Trim(msk_OutTimeshift.Text) & "' , '" & Trim(lbl_TotHours.Text) & "', " & Str(Val(Shft_Mins)) & ")"
                cmd.ExecuteNonQuery()

            Else
                cmd.CommandText = "update Shift_Head set Shift_Name = '" & Trim(txt_Name.Text) & "',  In_Time_Shift = '" & Trim(msk_InTimeshift.Text) & "' , Out_Time_Shift = '" & Trim(msk_OutTimeshift.Text) & "' , Total_Hours = '" & Trim(lbl_TotHours.Text) & "' ,Total_Minutes = " & Str(Val(Shft_Mins)) & "  where Shift_IdNo = " & Str(Val(txt_IdNo.Text))
                cmd.ExecuteNonQuery()

            End If

            trans.Commit()

            Common_Procedures.Master_Return.Return_Value = Trim(txt_Name.Text)
            Common_Procedures.Master_Return.Master_Type = "SHIFT"

            If New_Entry = True Then new_record()

            MessageBox.Show("Saved Sucessfully!!!", "FOR SAVING....", MessageBoxButtons.OK, MessageBoxIcon.Information)

        Catch ex As Exception
            trans.Rollback()

            If InStr(1, Trim(LCase(ex.Message)), "ix_Shift_Head") > 0 Then
                MessageBox.Show("Duplicate Shift Name", "DOES NOT SAVE", MessageBoxButtons.OK, MessageBoxIcon.Error)

            Else
                MessageBox.Show(ex.Message, "DOES NOT SAVE", MessageBoxButtons.OK, MessageBoxIcon.Error)

            End If

        Finally
            If txt_Name.Enabled And txt_Name.Visible Then txt_Name.Focus()

        End Try

    End Sub

    Private Sub btn_Save_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Save.Click
        save_record()
    End Sub

    Private Sub btn_Find_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Find.Click
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim movid As Integer

        da = New SqlClient.SqlDataAdapter("select Shift_IdNo from Shift_Head where Shift_Name = '" & Trim(cbo_Find.Text) & "'", con)
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

        btnClose_Click(sender, e)

    End Sub

    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        Me.Height = 350 ' 197
        pnl_Back.Enabled = True
        grp_Find.Visible = False
        If txt_Name.Enabled And txt_Name.Visible Then txt_Name.Focus()
    End Sub

    Private Sub cbo_Find_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Find.GotFocus
        'cbo_Find.DroppedDown = True
    End Sub

    Private Sub cbo_Find_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Find.KeyDown
        Try
            With cbo_Find
                If e.KeyValue = 38 And .DroppedDown = False Then
                    e.Handled = True
                    'SendKeys.Send("+{TAB}")
                ElseIf e.KeyValue = 40 And .DroppedDown = False Then
                    e.Handled = True
                    'SendKeys.Send("{TAB}")
                ElseIf e.KeyValue <> 13 And e.KeyValue <> 17 And e.KeyValue <> 27 And .DroppedDown = False Then
                    .DroppedDown = True
                End If
            End With

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT SELECT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub cbo_Find_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Find.KeyPress
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim Condt As String
        Dim FindStr As String

        Try

            With cbo_Find

                If Asc(e.KeyChar) <> 27 Then

                    If Asc(e.KeyChar) = 13 Then

                        If Trim(.Text) <> "" Then
                            If .DroppedDown = True Then
                                If Trim(.SelectedText) <> "" Then
                                    .Text = .SelectedText
                                Else
                                    If .Items.Count > 0 Then
                                        .SelectedIndex = 0
                                        .SelectedItem = .Items(0)
                                        .Text = .GetItemText(.SelectedItem)
                                    End If
                                End If
                            End If
                        End If

                        btn_Find_Click(sender, e)

                    Else

                        Condt = ""
                        FindStr = ""

                        If Asc(e.KeyChar) = 8 Then
                            If .SelectionStart <= 1 Then
                                .Text = ""
                            End If

                            If Trim(.Text) <> "" Then
                                If .SelectionLength = 0 Then
                                    FindStr = .Text.Substring(0, .Text.Length - 1)
                                Else
                                    FindStr = .Text.Substring(0, .SelectionStart - 1)
                                End If
                            End If

                        Else
                            If .SelectionLength = 0 Then
                                FindStr = .Text & e.KeyChar
                            Else
                                FindStr = .Text.Substring(0, .SelectionStart) & e.KeyChar
                            End If

                        End If

                        FindStr = LTrim(FindStr)

                        If Trim(FindStr) <> "" Then
                            Condt = " Where Shift_Name like '" & Trim(FindStr) & "%' or Shift_Name like '% " & Trim(FindStr) & "%' "
                        End If

                        da = New SqlClient.SqlDataAdapter("select Shift_Name from Shift_Head " & Condt & " order by Shift_Name", con)
                        da.Fill(dt)

                        .DataSource = dt
                        .DisplayMember = "Shift_Name"

                        .Text = FindStr

                        .SelectionStart = FindStr.Length

                        e.Handled = True

                    End If

                End If

            End With

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT SELECT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        'If Asc(e.KeyChar) = 13 Then
        '    btn_Find_Click(sender, e)
        'End If

    End Sub

    Private Sub btn_CloseFilter_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_CloseFilter.Click

        pnl_Back.Enabled = True
        grp_Filter.Visible = False
        If txt_Name.Enabled And txt_Name.Visible Then txt_Name.Focus()

        Me.Height = 380 '197

    End Sub

    Private Sub btn_Open_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Open.Click
        Dim movid As Integer

        movid = 0
        If IsDBNull((dgv_Filter.CurrentRow.Cells(0).Value)) = False Then
            movid = Val(dgv_Filter.CurrentRow.Cells(0).Value)
        End If

        If Val(movid) <> 0 Then
            move_record(movid)
            btn_CloseFilter_Click(sender, e)
        End If

    End Sub

    Private Sub dgv_Filter_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgv_Filter.DoubleClick
        btn_Open_Click(sender, e)
    End Sub

    Private Sub dgv_Filter_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_Filter.KeyDown
        If e.KeyCode = Keys.Enter Then
            btn_Open_Click(sender, e)
        End If
    End Sub


    Private Sub txt_Name_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Name.KeyPress
        'Dim K As Integer

        'If Asc(e.KeyChar) >= 97 And Asc(e.KeyChar) <= 122 Then
        '    K = Asc(e.KeyChar)
        '    K = K - 32
        '    e.KeyChar = Chr(K)
        'End If

        If Asc(e.KeyChar) = 13 Then
            'If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
            '    save_record()
            'End If
            msk_InTimeshift.Focus()
        End If
    End Sub

    Private Sub msk_InTimeshift_KeyDown(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles msk_InTimeshift.KeyDown
        If (e.KeyValue = 38) Then
            txt_Name.Focus()
        End If
        If (e.KeyValue = 40) Then
            msk_OutTimeshift.Focus()
        End If

        lbl_TotHours.Text = getHourFromMinitues(msk_InTimeshift.Text, msk_OutTimeshift.Text)
    End Sub

    Private Sub msk_InTimeshift_KeyPress(sender As Object, e As System.Windows.Forms.KeyPressEventArgs) Handles msk_InTimeshift.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
        If Asc(e.KeyChar) = 13 Then
            msk_OutTimeshift.Focus()
        End If
        lbl_TotHours.Text = getHourFromMinitues(msk_InTimeshift.Text, msk_OutTimeshift.Text)

    End Sub
    Private Sub msk_InTimeshift_TextChanged(sender As Object, e As System.EventArgs) Handles msk_InTimeshift.TextChanged
        lbl_TotHours.Text = getHourFromMinitues(msk_InTimeshift.Text, msk_OutTimeshift.Text)
    End Sub
    Private Sub msk_OutTimeshift_KeyDown(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles msk_OutTimeshift.KeyDown
        If (e.KeyValue = 38) Then
            msk_InTimeshift.Focus()
        End If

        lbl_TotHours.Text = getHourFromMinitues(msk_InTimeshift.Text, msk_OutTimeshift.Text)

        If (e.KeyValue = 40) Then
            If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                save_record()
            End If
        End If

    End Sub

    Private Sub msk_OutTimeshift_KeyPress(sender As Object, e As System.Windows.Forms.KeyPressEventArgs) Handles msk_OutTimeshift.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True

        lbl_TotHours.Text = getHourFromMinitues(msk_InTimeshift.Text, msk_OutTimeshift.Text)

        If Asc(e.KeyChar) = 13 Then
            If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                save_record()
            End If
        End If

    End Sub
    Private Sub msk_OutTimeshift_TextChanged(sender As Object, e As System.EventArgs) Handles msk_OutTimeshift.TextChanged
        lbl_TotHours.Text = getHourFromMinitues(msk_InTimeshift.Text, msk_OutTimeshift.Text)
    End Sub
    Function getHourFromMinitues(ByVal inTime As String, ByVal outTime As String)

        Dim Dt1 As Date, Dt2 As Date
        Dim TotMins As Double
        Dim H As Double, m As Double, Hrs As Double

        If Val(Microsoft.VisualBasic.Left(inTime, 2)) >= 24 Then MessageBox.Show("Time should not greater than 24", "", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        If Val(Microsoft.VisualBasic.Right(inTime, 2)) >= 60 Then MessageBox.Show("Minutes should not greater than 60", "", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        If Val(Microsoft.VisualBasic.Left(outTime, 2)) >= 24 Then MessageBox.Show("Time should not greater than 24", "", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        If Val(Microsoft.VisualBasic.Right(outTime, 2)) >= 60 Then MessageBox.Show("Minutes should not greater than 60", "", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        If Trim(inTime) <> "" And Trim(outTime) <> "" Then
            'If Microsoft.VisualBasic.Len(Trim(outTime)) = 4 Then
            '    outTime = Trim(outTime) & Microsoft.VisualBasic.Right(Trim(inTime), 1)
            'End If
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

   
   
End Class