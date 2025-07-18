Public Class Checking_table_creation
    Implements Interface_MDIActions

    Dim con As New SqlClient.SqlConnection(Common_Procedures.Connection_String)
    Dim New_Entry As Boolean = False
    Private Prec_ActCtrl As New Control

    Private Sub Checking_table_creation_load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Me.Width = 475
        'Me.Height = 284 ' 197
        grp_Find.Left = 6
        grp_Find.Top = 250
        grp_Find.Visible = False

        grp_Filter.Left = 6
        grp_Filter.Top = 250
        grp_Filter.Visible = False
        con.Open()
        new_record()


        AddHandler Txt_Checking_table_No.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Find.GotFocus, AddressOf ControlGotFocus

        AddHandler Txt_Checking_table_No.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Find.LostFocus, AddressOf ControlLostFocus


    End Sub
    Private Sub ControlGotFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim txtbx As TextBox
        Dim combobx As ComboBox

        On Error Resume Next

        If TypeOf Me.ActiveControl Is TextBox Or TypeOf Me.ActiveControl Is ComboBox Then
            Me.ActiveControl.BackColor = Color.Lime
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

    Private Sub Checking_table_creation_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        On Error Resume Next
        con.Close()
        con.Dispose()
        Common_Procedures.Last_Closed_FormName = Me.Name
    End Sub

    Private Sub btn_Find_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_FindOpen.Click
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim movid As Integer

        da = New SqlClient.SqlDataAdapter("select Checking_Table_IdNo from Checking_TableNo_Head where Checking_Table_No = '" & Trim(cbo_Find.Text) & "'", con)
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

        btn_FindClose_Click(sender, e)

    End Sub

    Private Sub Checking_table_creation_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If Asc(e.KeyChar) = 27 Then
            If grp_Find.Visible Then
                btn_FindClose_Click(sender, e)
            ElseIf grp_Filter.Visible Then
                btn_FilterClose_Click(sender, e)
            Else
                If MessageBox.Show("Do you want to Close?", "FOR CLOSING ENTRY...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) <> Windows.Forms.DialogResult.Yes Then
                    Exit Sub

                Else
                    Me.Close()
                End If
            End If

        End If
    End Sub


    Private Sub clear()
        Me.Height = 275
        pnl_back.Enabled = True
        grp_Find.Visible = True
        grp_Filter.Visible = False
        Txt_Checking_table_lbl_IdNo.Text = ""
        Txt_Checking_table_lbl_IdNo.ForeColor = Color.Black
        Txt_Checking_table_No.Text = ""

        New_Entry = False
    End Sub


    Public Sub delete_record() Implements Interface_MDIActions.delete_record
        Dim cmd As New SqlClient.SqlCommand

        'If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.Size_Creation, "~L~") = 0 And InStr(Common_Procedures.UR.Size_Creation, "~D~") = 0 Then MessageBox.Show("You have No Rights to Delete", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error) : Exit Sub

        If MessageBox.Show("Do you want to Delete ?", "FOR DELETION....", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.No Then
            Exit Sub
        End If

        If New_Entry = True Then
            MessageBox.Show("This is new entry", "DOES NOT DELETION....", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        Try

            cmd.Connection = con
            cmd.CommandText = "delete from Checking_TableNo_Head where Checking_Table_IdNo = " & Str(Val(Txt_Checking_table_lbl_IdNo.Text))

            cmd.ExecuteNonQuery()

            cmd.Dispose()

            new_record()

            MessageBox.Show("Deleted Sucessfully!!!", "FOR DELETION...", MessageBoxButtons.OK, MessageBoxIcon.Information)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally
            If Txt_Checking_table_No.Enabled And Txt_Checking_table_No.Visible Then Txt_Checking_table_No.Focus()

        End Try

    End Sub
    Public Sub print_record() Implements Interface_MDIActions.print_record
        '--
    End Sub
    Public Sub insert_record() Implements Interface_MDIActions.insert_record
        '--
    End Sub

    Public Sub filter_record() Implements Interface_MDIActions.filter_record
        Dim da As New SqlClient.SqlDataAdapter("select Checking_Table_IdNo ,Checking_Table_No from Checking_TableNo_Head where Checking_Table_IdNo <> 0 order by Checking_Table_IdNo", con)
        Dim dt As New DataTable

        da.Fill(dt)

        With dgv_Filter

            .Columns.Clear()
            .DataSource = dt

            .RowHeadersVisible = False

            .AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells
            .AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill

            .Columns(0).HeaderText = "IDNO"
            .Columns(1).HeaderText = "Checking No"

            .Columns(0).FillWeight = 40
            .Columns(1).FillWeight = 160

        End With

        new_record()

        'grp_Filter.Visible = True
        'grp_Filter.Left = grp_Filter.Left
        'grp_Filter.Top = grp_Filter.Top

        grp_Filter.Visible = True
        grp_Filter.BringToFront()
        pnl_back.Enabled = False

        If dgv_Filter.Enabled And dgv_Filter.Visible Then dgv_Filter.Focus()

        Me.Height = 510

        da.Dispose()
    End Sub

    Public Sub move_record(ByVal idno As Integer)
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable

        If Val(idno) = 0 Then Exit Sub

        clear()

        da = New SqlClient.SqlDataAdapter("select * from Checking_TableNo_Head a where Checking_Table_IdNo = " & Str(Val(idno)), con)
        da.Fill(dt)

        If dt.Rows.Count > 0 Then
            Txt_Checking_table_lbl_IdNo.Text = dt.Rows(0).Item("Checking_Table_IdNo").ToString
            Txt_Checking_table_No.Text = dt.Rows(0).Item("Checking_Table_No").ToString

            'If Val(txt_Sqft.Text) = 0 Then
            '    txt_Sqft.Text = ""
            'End If
        End If

        dt.Dispose()
        da.Dispose()

        If Txt_Checking_table_No.Enabled And Txt_Checking_table_No.Visible Then Txt_Checking_table_No.Focus()
    End Sub

    Public Sub movefirst_record() Implements Interface_MDIActions.movefirst_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim movid As Integer = 0

        Try
            da = New SqlClient.SqlDataAdapter("select min(Checking_Table_IdNo) from Checking_TableNo_Head Where Checking_Table_IdNo <> 0", con)
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
            da = New SqlClient.SqlDataAdapter("select max(Checking_Table_IdNo) from Checking_TableNo_Head Where Checking_Table_IdNo <> 0", con)
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
        Dim movid As Integer = 0

        Try
            da = New SqlClient.SqlDataAdapter("select min(Checking_Table_IdNo) from Checking_TableNo_Head Where Checking_Table_IdNo > " & Str(Val(Txt_Checking_table_lbl_IdNo.Text)) & " and Checking_Table_IdNo <> 0", con)
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
            da = New SqlClient.SqlDataAdapter("select max(Checking_Table_IdNo) from Checking_TableNo_Head Where Checking_Table_IdNo < " & Str(Val(Txt_Checking_table_lbl_IdNo.Text)) & " and Checking_Table_IdNo <> 0", con)
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
        Txt_Checking_table_lbl_IdNo.ForeColor = Color.Red

        Txt_Checking_table_lbl_IdNo.Text = Common_Procedures.get_MaxIdNo(con, "Checking_TableNo_Head", "Checking_Table_IdNo", "")

        If Txt_Checking_table_No.Enabled And Txt_Checking_table_No.Visible Then Txt_Checking_table_No.Focus()

    End Sub

    Public Sub open_record() Implements Interface_MDIActions.open_record
        Dim da As New SqlClient.SqlDataAdapter("select Checking_Table_No from Checking_TableNo_Head order by Checking_Table_No", con)
        Dim dt As New DataTable

        da.Fill(dt)

        cbo_Find.DataSource = dt
        cbo_Find.DisplayMember = "Checking_Table_No"

        new_record()

        Me.Height = 510
        grp_Find.Visible = True
        pnl_back.Enabled = False
        If cbo_Find.Enabled And cbo_Find.Visible Then cbo_Find.Focus()
    End Sub


    Public Sub save_record() Implements Interface_MDIActions.save_record
        Dim trans As SqlClient.SqlTransaction
        Dim cmd As New SqlClient.SqlCommand
        Dim Sur As String

        '  If Common_Procedures.UserRight_Check(Common_Procedures.UR.Size_Creation, New_Entry) = False Then Exit Sub

        If pnl_back.Enabled = False Then
            MessageBox.Show("Close Other Windows", "DOES NOT SAVE", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        If Trim(Txt_Checking_table_No.Text) = "" Then
            MessageBox.Show("Invalid Checking No", "DOES NOT SAVE", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        Sur = Common_Procedures.Remove_NonCharacters(Trim(Txt_Checking_table_No.Text))

        trans = con.BeginTransaction
        Try

            cmd.Connection = con
            cmd.Transaction = trans

            If New_Entry = True Then

                Txt_Checking_table_lbl_IdNo.Text = Common_Procedures.get_MaxIdNo(con, "Checking_TableNo_Head", "Checking_Table_IdNo", "", trans)

                cmd.CommandText = "Insert into Checking_TableNo_Head(Checking_Table_IdNo, Checking_Table_No, Sur_Name) values (" & Str(Val(Txt_Checking_table_lbl_IdNo.Text)) & ", '" & Trim(Txt_Checking_table_No.Text) & "',  '" & Trim(Sur) & "')"
                cmd.ExecuteNonQuery()

            Else
                cmd.CommandText = "update Checking_TableNo_Head set Checking_Table_No = '" & Trim(Txt_Checking_table_No.Text) & "', Sur_Name = '" & Trim(Sur) & "' where Checking_Table_IdNo = " & Str(Val(Txt_Checking_table_lbl_IdNo.Text))
                cmd.ExecuteNonQuery()

            End If

            trans.Commit()

            Common_Procedures.Master_Return.Return_Value = Trim(Txt_Checking_table_No.Text)
            Common_Procedures.Master_Return.Master_Type = "TABLENO"

            If New_Entry = True Then new_record()

            MessageBox.Show("Saved Sucessfully!!!", "FOR SAVING....", MessageBoxButtons.OK, MessageBoxIcon.Information)

        Catch ex As Exception
            trans.Rollback()
            If InStr(1, Trim(LCase(ex.Message)), "ix_Checking_TableNo_Head") > 0 Then
                MessageBox.Show("Duplicate Brand Name", "DOES NOT SAVE", MessageBoxButtons.OK, MessageBoxIcon.Error)

            Else
                MessageBox.Show(ex.Message, "DOES NOT SAVE", MessageBoxButtons.OK, MessageBoxIcon.Error)

            End If

        Finally
            If Txt_Checking_table_No.Enabled And Txt_Checking_table_No.Visible Then Txt_Checking_table_No.Focus()


        End Try
    End Sub

    Private Sub btn_Save_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Save.Click
        save_record()
    End Sub

    Private Sub btnClose_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Close.Click
        Me.Height = 284 ' 197
        pnl_back.Enabled = True
        grp_Find.Visible = False
        If Txt_Checking_table_No.Enabled And Txt_Checking_table_No.Visible Then Txt_Checking_table_No.Focus()
    End Sub

    Private Sub btn_Close_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Close.Click
        Me.Close()
        Me.Dispose()
    End Sub
    Private Sub btn_FindOpen_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_FindOpen.Click
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim movid As Integer

        da = New SqlClient.SqlDataAdapter("select Checking_Table_IdNo from Checking_TableNo_Head where Checking_Table_No = '" & Trim(cbo_Find.Text) & "'", con)
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

        btn_FindClose_Click(sender, e)
    End Sub

    Private Sub btn_FindClose_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_FindClose.Click

        Me.Height = 275
        pnl_back.Enabled = True
        grp_Find.Visible = False
    End Sub

    Private Sub btn_FilterOpen_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_FilterOpen.Click
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

    Private Sub btn_FilterClose_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_FilterClose.Click
        Me.Height = 275
        pnl_back.Enabled = True
        grp_Filter.Visible = False
        If Txt_Checking_table_No.Enabled And Txt_Checking_table_No.Visible Then Txt_Checking_table_No.Focus()

        'Me.Height = 284 '197
    End Sub

    Private Sub Checking_table_No_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles Txt_Checking_table_No.GotFocus
        Txt_Checking_table_No.BackColor = Color.PaleGreen
        Txt_Checking_table_No.ForeColor = Color.Blue
    End Sub

    Private Sub Checking_table_No_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Txt_Checking_table_No.KeyDown
        If (e.KeyValue = 38) Then
            btn_Close.Focus()
        End If
        'If (e.KeyValue = 40) Then
        '    txt_Sqft.Focus()
        'End If
    End Sub

    Private Sub Checking_table_No_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Txt_Checking_table_No.KeyPress
        Dim K As Integer
        If Asc(e.KeyChar) >= 97 And Asc(e.KeyChar) <= 122 Then
            K = Asc(e.KeyChar)
            K = K - 32
            e.KeyChar = Chr(K)
        End If

        If Asc(e.KeyChar) = 13 Then
            If MessageBox.Show("Do you want to save", "FOR SAVING...", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then
                save_record()
            Else
            End If
        End If
    End Sub

    Private Sub cbo_Find_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Find.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Find, Nothing, Nothing, "Checking_TableNo_Head", "Checking_Table_No", "", "")
    End Sub


    Private Sub cbo_Find_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Find.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Find, Nothing, "Checking_TableNo_Head", "Checking_Table_No", "", "")
        If Asc(e.KeyChar) = 13 Then
            btn_FindOpen_Click(sender, e)
        End If
    End Sub

    Private Sub dgv_Filter_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgv_Filter.DoubleClick
        Call btn_FilterOpen_Click(sender, e)
    End Sub

    Private Sub dgv_Filter_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_Filter.KeyDown
        If e.KeyCode = Keys.Enter Then
            Call btn_FilterOpen_Click(sender, e)
        End If
    End Sub
    Private Sub Checking_table_No_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles Txt_Checking_table_No.LostFocus
        Txt_Checking_table_No.BackColor = Color.White
        Txt_Checking_table_No.ForeColor = Color.Black
    End Sub

    Private Sub txt_Sqft_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        Txt_Checking_table_No.BackColor = Color.PaleGreen
        Txt_Checking_table_No.ForeColor = Color.Blue
    End Sub

    Private Sub txt_Sqft_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs)
        If (e.KeyValue = 38) Then
            Txt_Checking_table_No.Focus()
        End If
        If (e.KeyValue = 40) Then
            If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                save_record()
            End If
        End If
    End Sub

    Private Sub txt_Sqft_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
        If Asc(e.KeyChar) = 13 Then
            If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                save_record()
            End If
        End If
    End Sub

    Private Sub txt_Sqft_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        Txt_Checking_table_No.BackColor = Color.White
        Txt_Checking_table_No.ForeColor = Color.Black
    End Sub
End Class