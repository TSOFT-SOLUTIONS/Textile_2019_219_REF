Public Class OE_Count_Creation
    Implements Interface_MDIActions

    Dim con As New SqlClient.SqlConnection(Common_Procedures.Connection_String)
    Dim New_Entry As Boolean

    Private Sub clear()
        Me.Height = 330  ' 327
        pnl_back.Enabled = True
        grp_find.Visible = False
        grp_Filter.Visible = False
        lbl_IdNo.Text = ""
        lbl_IdNo.ForeColor = Color.Black
        txt_Name.Text = ""
        txt_description.Text = ""
        txt_resultantcount.Text = ""
        txt_HSNCode.Text = ""
        txt_GSTPerc.Text = ""

        New_Entry = False
    End Sub

    Public Sub delete_record() Implements Interface_MDIActions.delete_record
        Dim cmd As New SqlClient.SqlCommand
        Dim da As SqlClient.SqlDataAdapter
        Dim dt As DataTable

        ' If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.Count_Creation, "~L~") = 0 And InStr(Common_Procedures.UR.Count_Creation, "~D~") = 0 Then MessageBox.Show("You have No Rights to Delete", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error) : Exit Sub

        If MessageBox.Show("Do you want to Delete ?", "FOR DELETION....", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.No Then
            Exit Sub
        End If

        If New_Entry = True Then
            MessageBox.Show("This is new entry", "DOES NOT DELETION....", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        Try

            da = New SqlClient.SqlDataAdapter("select count(*) from Mill_Count_Details where Count_IdNo = " & Str(Val(lbl_IdNo.Text)), con)
            dt = New DataTable
            da.Fill(dt)
            If dt.Rows.Count > 0 Then
                If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                    If Val(dt.Rows(0)(0).ToString) > 0 Then
                        MessageBox.Show("Already used this Count", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        Exit Sub
                    End If
                End If
            End If

            da = New SqlClient.SqlDataAdapter("select count(*) from Stock_Yarn_Processing_Details where Count_Idno = " & Str(Val(lbl_IdNo.Text)), con)
            dt = New DataTable
            da.Fill(dt)
            If dt.Rows.Count > 0 Then
                If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                    If Val(dt.Rows(0)(0).ToString) > 0 Then
                        MessageBox.Show("Already used this Count", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        Exit Sub
                    End If
                End If
            End If
            dt.Clear()

            cmd.Connection = con
            cmd.CommandText = "delete from Count_Head where Count_IdNo = " & Str(Val(lbl_IdNo.Text))

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
        Dim da As New SqlClient.SqlDataAdapter("select count_IdNo, Count_Name,Count_Description from Count_Head where Count_IdNo <> 0 order by Count_IdNo", con)
        Dim dt As New DataTable

        da.Fill(dt)

        With dgv_Filter

            .Columns.Clear()
            .DataSource = dt

            .RowHeadersVisible = False

            .AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells
            .AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill

            .Columns(0).HeaderText = "IDNO"
            .Columns(1).HeaderText = "NAME"
            .Columns(2).HeaderText = "DESCRIPTION"


            .Columns(0).FillWeight = 60
            .Columns(1).FillWeight = 160
            .Columns(2).FillWeight = 300


        End With

        new_record()

        grp_Filter.Visible = True

        pnl_back.Enabled = False

        If dgv_Filter.Enabled And dgv_Filter.Visible Then dgv_Filter.Focus()

        Me.Height = 520   '    514

        da.Dispose()
    End Sub

    Public Sub insert_record() Implements Interface_MDIActions.insert_record
        '-----
    End Sub

    Public Sub move_record(ByVal idno As Integer)
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable


        If Val(idno) = 0 Then Exit Sub

        clear()

        da = New SqlClient.SqlDataAdapter("select a.*, b.count_name as stock_undername from Count_head a LEFT OUTER JOIN count_head b ON a.Count_StockUnder_IdNo = b.count_idno where a.Count_idno = " & Str(Val(idno)), con)
        'da = New SqlClient.SqlDataAdapter("select a. from Count_head where Count_idno = " & Str(Val(idno)), con)
        da.Fill(dt)

        If dt.Rows.Count > 0 Then
            lbl_IdNo.Text = dt.Rows(0).Item("Count_IdNo").ToString
            txt_Name.Text = dt.Rows(0).Item("Count_Name").ToString
            txt_description.Text = dt.Rows(0).Item("Count_Description").ToString

            'cbo_stock.Text = Common_Procedures.Count_IdNoToName(con, dt.Rows(0).Item("Count_Stockunder_IdNo").ToString)
            'cbo_stock.Text = dt.Rows(0).Item("Count_Stockunder_IdNo").ToString
            txt_resultantcount.Text = dt.Rows(0).Item("Resultant_Count").ToString

            txt_HSNCode.Text = dt.Rows(0).Item("HSN_Code").ToString
            txt_GSTPerc.Text = dt.Rows(0).Item("GST_Percentege").ToString



        End If

        dt.Dispose()
        da.Dispose()

        If txt_Name.Enabled And txt_Name.Visible Then txt_Name.Focus()

    End Sub

    Public Sub movefirst_record() Implements Interface_MDIActions.movefirst_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim movid As Integer = 0

        Try
            da = New SqlClient.SqlDataAdapter("select min(count_idno) from Count_head Where count_idno <> 0", con)
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

        'Try
        da = New SqlClient.SqlDataAdapter("select max(Count_idno) from Count_head Where Count_idno <> 0", con)
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

        'Catch ex As Exception
        'MessageBox.Show(ex.Message, "DOES NOT MOVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        'End Try
    End Sub

    Public Sub movenext_record() Implements Interface_MDIActions.movenext_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim movid As Integer = 0

        Try
            da = New SqlClient.SqlDataAdapter("select min(Count_idno) from count_head Where Count_idno > " & Str(Val(lbl_IdNo.Text)) & " and Count_idno <> 0", con)
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

    Public Sub moveprevious_record() Implements Interface_MDIActions.moveprevious_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim movid As Integer = 0

        Try
            da = New SqlClient.SqlDataAdapter("select max(Count_idno) from Count_head Where count_idno < " & Str(Val(lbl_IdNo.Text)) & " and count_idno <> 0", con)
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

    Public Sub new_record() Implements Interface_MDIActions.new_record
        clear()

        New_Entry = True
        lbl_IdNo.ForeColor = Color.Red

        lbl_IdNo.Text = Common_Procedures.get_MaxIdNo(con, "Count_Head", "Count_IdNo", "")

        If txt_Name.Enabled And txt_Name.Visible Then txt_Name.Focus()
    End Sub

    Public Sub open_record() Implements Interface_MDIActions.open_record
        Dim da As New SqlClient.SqlDataAdapter("select Count_Name from Count_Head order by Count_Name", con)
        Dim dt As New DataTable

        da.Fill(dt)

        cbo_Find.DataSource = dt
        cbo_Find.DisplayMember = "Count_Name"

        new_record()

        Me.Height = 520   ' 513
        grp_find.Visible = True
        pnl_back.Enabled = False
        If cbo_Find.Enabled And cbo_Find.Visible Then cbo_Find.Focus()
    End Sub

    Public Sub print_record() Implements Interface_MDIActions.print_record
        '-------
    End Sub

    Public Sub save_record() Implements Interface_MDIActions.save_record
        Dim trans As SqlClient.SqlTransaction
        Dim cmd As New SqlClient.SqlCommand
        Dim Sur As String


        '  If Common_Procedures.UserRight_Check(Common_Procedures.UR.Count_Creation, New_Entry) = False Then Exit Sub

        If pnl_back.Enabled = False Then
            MessageBox.Show("Close Other Windows", "DOES NOT SAVE", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        If Trim(txt_Name.Text) = "" Then
            MessageBox.Show("Invalid CountName", "DOES NOT SAVE", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If txt_Name.Enabled Then txt_Name.Focus()
            Exit Sub
        End If
        If Val(txt_resultantcount.Text) = 0 Then
            txt_resultantcount.Text = Val(txt_Name.Text)
        End If

        Sur = Common_Procedures.Remove_NonCharacters(Trim(txt_Name.Text))


        trans = con.BeginTransaction
        Try

            cmd.Connection = con
            cmd.Transaction = trans

            If New_Entry = True Then

                lbl_IdNo.Text = Common_Procedures.get_MaxIdNo(con, "Count_Head", "Count_IdNo", "", trans)

                cmd.CommandText = "Insert into Count_Head(Count_IdNo, Count_Name, Sur_Name, Count_Description,Resultant_Count,HSN_Code,GST_Percentege) values (" & Str(Val(lbl_IdNo.Text)) & ", '" & Trim(txt_Name.Text) & "', '" & Trim(Sur) & "','" & Trim(txt_description.Text) & "'," & Val(txt_resultantcount.Text) & ",'" & Trim(txt_HSNCode.Text) & "'," & Val(txt_GSTPerc.Text) & " ) "
                cmd.ExecuteNonQuery()

            Else
                cmd.CommandText = "update Count_Head set Count_Name = '" & Trim(txt_Name.Text) & "', Sur_Name = '" & Trim(Sur) & "',Count_Description='" & Trim(txt_description.Text) & "',Resultant_Count=" & Val(txt_resultantcount.Text) & " ,HSN_Code='" & Trim(txt_HSNCode.Text) & "',GST_Percentege=" & Val(txt_GSTPerc.Text) & " where Count_IdNo = " & Str(Val(lbl_IdNo.Text)) & " "
                cmd.ExecuteNonQuery()

            End If

            trans.Commit()

            Common_Procedures.Master_Return.Return_Value = Trim(txt_Name.Text)
            Common_Procedures.Master_Return.Master_Type = "Count"


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
            If InStr(1, Trim(LCase(ex.Message)), "ix_count_head") > 0 Then
                MessageBox.Show("Duplicate Count Name", "DOES NOT SAVE", MessageBoxButtons.OK, MessageBoxIcon.Error)

            Else
                MessageBox.Show(ex.Message, "DOES NOT SAVE", MessageBoxButtons.OK, MessageBoxIcon.Error)

            End If

        Finally
            If txt_Name.Enabled And txt_Name.Visible Then txt_Name.Focus()


        End Try
    End Sub

    Private Sub LotNo_creation_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        On Error Resume Next
        con.Close()
        con.Dispose()
        Common_Procedures.Last_Closed_FormName = Me.Name
    End Sub

    Private Sub LotNo_creation_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If Asc(e.KeyChar) = 27 Then
            If grp_Filter.Visible Then
                btn_FilterClose_Click(sender, e)
            ElseIf grp_find.Visible Then
                btn_FindClose_Click(sender, e)
            Else
                Me.Close()
            End If

        End If
    End Sub


    Private Sub LotNo_creation_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable

        Me.Width = 535 ' 544
        Me.Height = 310

        grp_find.Left = 8  ' 12
        grp_find.Top = 310  '292
        grp_find.Visible = False

        grp_Filter.Left = 8  ' 12
        grp_Filter.Top = 310  '292
        grp_Filter.Visible = False

        con.Open()



        Me.Top = Me.Top - 75

        new_record()

    End Sub


    Private Sub btn_Save_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Save.Click
        save_record()
    End Sub

    Private Sub btn_FilterClose_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_FilterClose.Click
        Me.Height = 310  ' 327
        pnl_back.Enabled = True
        grp_Filter.Visible = False
    End Sub

    Private Sub btn_FindOpen_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_FindOpen.Click
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim movid As Integer

        If Trim(cbo_Find.Text) = "" Then
            MessageBox.Show("Invalid CountName", "DOES NOT FIND...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If cbo_Find.Visible And cbo_Find.Enabled Then cbo_Find.Focus()
            Exit Sub
        End If

        da = New SqlClient.SqlDataAdapter("select Count_IdNo from Count_Head where Count_Name= '" & Trim(cbo_Find.Text) & "'", con)
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

        btn_FilterClose_Click(sender, e)
    End Sub


    Private Sub btn_FindClose_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_FindClose.Click

        pnl_back.Enabled = True
        grp_find.Visible = False
        Me.Height = 310 ' 327
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

                        Call btn_FindOpen_Click(sender, e)

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
                            Condt = " Where Count_Name like '" & Trim(FindStr) & "%' or Count_Name like '% " & Trim(FindStr) & "%' "
                        End If

                        da = New SqlClient.SqlDataAdapter("select Count_Name from Count_Head " & Condt & " order by Count_Name", con)
                        da.Fill(dt)

                        .DataSource = dt
                        .DisplayMember = "Count_Name"

                        .Text = FindStr

                        .SelectionStart = FindStr.Length

                        e.Handled = True

                    End If

                End If

            End With

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT SELECT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        da.Dispose()
    End Sub

    Private Sub dgv_Filter_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgv_Filter.DoubleClick
        Call btn_Filteropen_Click(sender, e)
    End Sub

    Private Sub btn_Filteropen_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Filteropen.Click
        Dim movid As Integer

        movid = 0
        If IsDBNull((dgv_Filter.CurrentRow.Cells(0).Value)) = False Then
            movid = Val(dgv_Filter.CurrentRow.Cells(0).Value)
        End If

        If Val(movid) <> 0 Then
            move_record(movid)
            btn_FilterClose_Click(sender, e)
        End If
    End Sub

    Private Sub dgv_Filter_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_Filter.KeyDown
        If e.KeyValue = 13 Then
            Call btn_Filteropen_Click(sender, e)
        End If
    End Sub

    Private Sub btn_Close_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Close.Click
        Me.Close()
    End Sub

    Private Sub txt_Description_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_description.KeyDown
        If e.KeyValue = 40 Then SendKeys.Send("{TAB}")
        If e.KeyValue = 38 Then SendKeys.Send("+{TAB}")
    End Sub

    Private Sub txt_Name_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_Name.KeyDown
        If e.KeyValue = 40 Then SendKeys.Send("{TAB}")
        If e.KeyValue = 38 Then SendKeys.Send("+{TAB}")
    End Sub

    Private Sub txt_Name_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Name.KeyPress
        If Asc(e.KeyChar) = 34 Or Asc(e.KeyChar) = 39 Then
            e.Handled = True
        End If
        If Asc(e.KeyChar) = 13 Then
            SendKeys.Send("{TAB}")
        End If
    End Sub


    Private Sub txt_count_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_resultantcount.KeyDown
        If e.KeyValue = 40 Then SendKeys.Send("{TAB}")
        If e.KeyValue = 38 Then SendKeys.Send("+{TAB}")
    End Sub

    Private Sub txt_count_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_resultantcount.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
        If Asc(e.KeyChar) = 13 Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub txt_description_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_description.KeyPress
        If Asc(e.KeyChar) = 13 Then
            SendKeys.Send("{TAB}")
        End If
    End Sub
    Private Sub txt_GSTPerc_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_GSTPerc.KeyDown
        If e.KeyValue = 38 Then SendKeys.Send("+{TAB}")
        If e.KeyValue = 40 Then
            If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                save_record()
            Else
                txt_Name.Focus()
            End If
        End If

    End Sub

    Private Sub txt_GSTPerc_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_GSTPerc.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
        If Asc(e.KeyChar) = 13 Then
            If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                save_record()
            Else
                txt_Name.Focus()
            End If
        End If
    End Sub

    Private Sub txt_HSNCode_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_HSNCode.KeyDown
        If e.KeyValue = 40 Then SendKeys.Send("{TAB}")
        If e.KeyValue = 38 Then SendKeys.Send("+{TAB}")
    End Sub

    Private Sub txt_HSNCode_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_HSNCode.KeyPress
        If Asc(e.KeyChar) = 13 Then
            SendKeys.Send("{TAB}")
        End If
    End Sub
End Class
