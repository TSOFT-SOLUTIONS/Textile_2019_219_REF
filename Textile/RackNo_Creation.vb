Public Class RackNo_Creation
    Implements Interface_MDIActions

    Dim con As New SqlClient.SqlConnection(Common_Procedures.Connection_String)
    Dim New_Entry As Boolean

    Private Sub clear()
        Me.Height = 258
        pnl_back.Enabled = True
        grp_Find.Visible = False
        grp_Filter.Visible = False
        lbl_IdNo.Text = ""
        lbl_IdNo.ForeColor = Color.Black
        txt_RackNo.Text = ""
        chk_Close.Checked = False
        New_Entry = False
    End Sub

    Public Sub delete_record() Implements Interface_MDIActions.delete_record
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim cmd As New SqlClient.SqlCommand

        ' If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.RackNo_Creation, "~L~") = 0 And InStr(Common_Procedures.UR.RackNo_Creation, "~D~") = 0 Then MessageBox.Show("You have No Rights to Delete", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error) : Exit Sub

        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.DeletingEntry, Common_Procedures.UR.FP_RackNo_Creation, New_Entry, Me) = False Then Exit Sub


        If MessageBox.Show("Do you want to Delete ?", "FOR DELETION....", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.No Then
            Exit Sub
        End If

        If New_Entry = True Then
            MessageBox.Show("This is new entry", "DOES NOT DELETION....", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        Try

            Da = New SqlClient.SqlDataAdapter("select count(*) from Stock_Item_Processing_Details where Rack_IdNo = " & Str(Val(lbl_IdNo.Text)), con)
            dt = New DataTable
            da.Fill(dt)
            If dt.Rows.Count > 0 Then
                If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                    If Val(dt.Rows(0)(0).ToString) > 0 Then
                        MessageBox.Show("Already used this Rack", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        Exit Sub
                    End If
                End If
            End If

            cmd.Connection = con
            cmd.CommandText = "delete from Rack_Head where Rack_IdNo = " & Str(Val(lbl_IdNo.Text))

            cmd.ExecuteNonQuery()

            cmd.Dispose()

            new_record()

            MessageBox.Show("Deleted Sucessfully!!!", "FOR DELETION...", MessageBoxButtons.OK, MessageBoxIcon.Information)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally
            If txt_RackNo.Enabled And txt_RackNo.Visible Then txt_RackNo.Focus()

        End Try

    End Sub

    Public Sub filter_record() Implements Interface_MDIActions.filter_record
        Dim da As New SqlClient.SqlDataAdapter("select Rack_IdNo, Rack_No from Rack_Head where Rack_IdNo <> 0 order by Rack_IdNo", con)
        Dim dt As New DataTable

        da.Fill(dt)

        With dgv_Filter

            .Columns.Clear()
            .DataSource = dt

            .RowHeadersVisible = False

            .AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells
            .AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill

            .Columns(0).HeaderText = "IDNO"
            .Columns(1).HeaderText = "RaCK NO"

            .Columns(0).FillWeight = 40
            .Columns(1).FillWeight = 160

        End With

        new_record()

        grp_Filter.Visible = True

        pnl_back.Enabled = False

        If dgv_Filter.Enabled And dgv_Filter.Visible Then dgv_Filter.Focus()

        Me.Height = 427

        da.Dispose()
    End Sub

    Public Sub insert_record() Implements Interface_MDIActions.insert_record
        '---
    End Sub

    Public Sub move_record(ByVal idno As Integer)
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable

        If Val(idno) = 0 Then Exit Sub

        clear()

        da = New SqlClient.SqlDataAdapter("select * from Rack_head a where Rack_idno = " & Str(Val(idno)), con)
        da.Fill(dt)

        If dt.Rows.Count > 0 Then
            lbl_IdNo.Text = dt.Rows(0).Item("Rack_IdNo").ToString
            txt_RackNo.Text = dt.Rows(0).Item("Rack_No").ToString
            If IsDBNull(dt.Rows(0).Item("Close_Status").ToString) = False Then
                If Val(dt.Rows(0).Item("Close_Status").ToString) = 1 Then
                    chk_Close.Checked = True
                End If
            End If
        End If

        dt.Dispose()
        da.Dispose()

        If txt_RackNo.Enabled And txt_RackNo.Visible Then txt_RackNo.Focus()
    End Sub
    Public Sub movefirst_record() Implements Interface_MDIActions.movefirst_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim movid As Integer = 0

        Try
            da = New SqlClient.SqlDataAdapter("select min(Rack_idno) from Rack_head Where Rack_idno <> 0", con)
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
            da = New SqlClient.SqlDataAdapter("select max(Rack_idno) from Rack_head Where Rack_idno <> 0", con)
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
            da = New SqlClient.SqlDataAdapter("select min(Rack_idno) from Rack_head Where Rack_idno > " & Str(Val(lbl_IdNo.Text)) & " and Rack_idno <> 0", con)
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
            da = New SqlClient.SqlDataAdapter("select max(Rack_idno) from Rack_head Where Rack_idno < " & Str(Val(lbl_IdNo.Text)) & " and Rack_idno <> 0", con)
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
        Try
            New_Entry = True
            lbl_IdNo.ForeColor = Color.Red

            lbl_IdNo.Text = Common_Procedures.get_MaxIdNo(con, "Rack_Head", "Rack_IdNo", "")

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR NEW RECORD...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally
            If txt_RackNo.Enabled And txt_RackNo.Visible Then txt_RackNo.Focus()

        End Try
    End Sub

    Public Sub open_record() Implements Interface_MDIActions.open_record
        Dim da As New SqlClient.SqlDataAdapter("select Rack_No from Rack_Head order by Rack_No", con)
        Dim dt As New DataTable

        da.Fill(dt)

        cbo_Find.DataSource = dt
        cbo_Find.DisplayMember = "Rack_No"

        new_record()

        Me.Height = 411
        grp_Find.Visible = True
        pnl_back.Enabled = False
        If cbo_Find.Enabled And cbo_Find.Visible Then cbo_Find.Focus()
    End Sub

    Public Sub print_record() Implements Interface_MDIActions.print_record
        '------------
    End Sub

    Public Sub save_record() Implements Interface_MDIActions.save_record
        Dim trans As SqlClient.SqlTransaction
        Dim cmd As New SqlClient.SqlCommand
        Dim Sur As String = ""
        Dim Clo_STS As Integer = 0

        'If Common_Procedures.UserRight_Check(Common_Procedures.UR.RackNo_Creation, New_Entry) = False Then Exit Sub
        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.SavingEntry, Common_Procedures.UR.FP_RackNo_Creation, New_Entry, Me) = False Then Exit Sub

        If pnl_back.Enabled = False Then
            MessageBox.Show("Close Other Windows", "DOES NOT SAVE", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        If Trim(txt_RackNo.Text) = "" Then
            MessageBox.Show("Invalid Name", "DOES NOT SAVE", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If txt_RackNo.Enabled Then txt_RackNo.Focus()
            Exit Sub
        End If

        Sur = Common_Procedures.Remove_NonCharacters(Trim(txt_RackNo.Text))


        Clo_STS = 0
        If chk_Close.Checked = True Then
            Clo_STS = 1
        End If

        trans = con.BeginTransaction
        Try

            cmd.Connection = con
            cmd.Transaction = trans

            If New_Entry = True Then

                lbl_IdNo.Text = Common_Procedures.get_MaxIdNo(con, "Rack_Head", "Rack_IdNo", "", trans)

                cmd.CommandText = "Insert into Rack_Head(Rack_IdNo, Rack_No, Sur_Name, Close_Status) values (" & Str(Val(lbl_IdNo.Text)) & ", '" & Trim(txt_RackNo.Text) & "', '" & Trim(Sur) & "', " & Str(Val(Clo_STS)) & ")"
                cmd.ExecuteNonQuery()

            Else
                cmd.CommandText = "update Rack_Head set Rack_No = '" & Trim(txt_RackNo.Text) & "', Sur_Name = '" & Trim(Sur) & "', Close_Status = " & Str(Val(Clo_STS)) & " Where Rack_IdNo = " & Str(Val(lbl_IdNo.Text))
                cmd.ExecuteNonQuery()

            End If

            trans.Commit()
            If Clo_STS = 0 Then
                Common_Procedures.Master_Return.Return_Value = Trim(txt_RackNo.Text)
            Else
                Common_Procedures.Master_Return.Return_Value = ""
            End If

            Common_Procedures.Master_Return.Master_Type = "RACK"



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
            If InStr(1, Trim(LCase(ex.Message)), "ix_rack_head") > 0 Then
                MessageBox.Show("Duplicate Rack Name", "DOES NOT SAVE", MessageBoxButtons.OK, MessageBoxIcon.Error)

            Else
                MessageBox.Show(ex.Message, "DOES NOT SAVE", MessageBoxButtons.OK, MessageBoxIcon.Error)

            End If

        Finally
            If txt_RackNo.Enabled And txt_RackNo.Visible Then txt_RackNo.Focus()


        End Try
    End Sub
    Private Sub RackNo_Creation_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        con.Close()
        con.Dispose()
    End Sub

    Private Sub RackNo_Creation_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If Asc(e.KeyChar) = 27 Then
            If grp_Filter.Visible Then
                btn_FilterClose_Click(sender, e)
            ElseIf grp_Find.Visible Then
                btn_FindOpen_Click(sender, e)
            Else
                Me.Close()
            End If

        End If
    End Sub

    Private Sub btn_FilterClose_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_FilterClose.Click
        Me.Height = 258
        pnl_back.Enabled = True
        grp_Filter.Visible = False
    End Sub

    Private Sub btn_FindOpen_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_FindOpen.Click
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim movid As Integer

        da = New SqlClient.SqlDataAdapter("select Rack_IdNo from Rack_Head where Rack_No = '" & Trim(cbo_Find.Text) & "'", con)
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

    Private Sub RackNo_Creation_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.Width = 370
        grp_Find.Left = 12
        grp_Find.Top = 237
        grp_Find.Visible = False

        grp_Filter.Left = 12
        grp_Filter.Top = 237
        grp_Filter.Visible = False
        con.Open()
        new_record()
    End Sub

    Private Sub btn_Save_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Save.Click
        save_record()
    End Sub

    Private Sub btn_Close_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Close.Click
        Me.Close()
    End Sub

    Private Sub txt_RackNo_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_RackNo.GotFocus
        With txt_RackNo
            .BackColor = Color.Lime
            .ForeColor = Color.Blue
            .SelectAll()
        End With
    End Sub

    Private Sub txt_RackNo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_RackNo.KeyPress
        If Asc(e.KeyChar) = 13 Then
            If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                save_record()
            End If
        End If
    End Sub

    Private Sub btn_FindClose_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_FindClose.Click
        Me.Height = 258
        pnl_back.Enabled = True
        grp_Find.Visible = False
    End Sub

    Private Sub cbo_Find_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Find.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Rack_Head", "Rack_No", "", "")

    End Sub

    Private Sub cbo_Find_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Find.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Find, Nothing, Nothing, "Rack_Head", "Rack_No", "", "")

    End Sub

    Private Sub cbo_Find_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Find.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Find, btn_FindOpen, "Rack_Head", "Rack_No", "", "")

    End Sub

    Private Sub dgv_Filter_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgv_Filter.DoubleClick
        Call btn_FilterOpen_Click(sender, e)
    End Sub

   
    Private Sub dgv_Filter_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_Filter.KeyDown
        If e.KeyValue = 13 Then
            Call btn_FilterOpen_Click(sender, e)
        End If
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

    Private Sub txt_RackNo_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_RackNo.LostFocus
        With txt_RackNo
            .BackColor = Color.White
            .ForeColor = Color.Black
            .SelectAll()
        End With
    End Sub
End Class