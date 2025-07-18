Public Class Process_Creation
    Implements Interface_MDIActions

    Dim con As New SqlClient.SqlConnection(Common_Procedures.Connection_String)
    Dim New_Entry As Boolean

    Private Sub CLEAR()

        Me.Height = 290
        pnl_Back.Enabled = True
        grp_Open.Visible = False
        grp_Filter.Visible = False

        lbl_IdNo.Text = ""
        lbl_IdNo.ForeColor = Color.Black
        txt_Name.Text = ""

        If Common_Procedures.settings.CustomerCode = "1516" Then

            lbl_Del_Type_Caption.Visible = True
            chk_Delivery_Cloth.Visible = True
            chk_Delivery_FinishedProduct.Visible = True

            lbl_Return_Type_Caption.Visible = True
            chk_Return_Cloth.Visible = True
            chk_Retuen_FinishedProduct.Visible = True

            chk_Delivery_Cloth.Checked = False
            chk_Delivery_FinishedProduct.Checked = False

            chk_Return_Cloth.Checked = False
            chk_Retuen_FinishedProduct.Checked = False

        Else

            lbl_Del_Type_Caption.Visible = False
            chk_Delivery_Cloth.Visible = False
            chk_Delivery_FinishedProduct.Visible = False

            lbl_Return_Type_Caption.Visible = False
            chk_Return_Cloth.Visible = False
            chk_Retuen_FinishedProduct.Visible = False

            chk_Delivery_Cloth.Checked = True
            chk_Delivery_FinishedProduct.Checked = False

            chk_Return_Cloth.Checked = True
            chk_Retuen_FinishedProduct.Checked = False

        End If

        New_Entry = False

    End Sub

    Public Sub move_record(ByVal idno As Integer)
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable

        If Val(idno) = 0 Then Exit Sub

        clear()

        da = New SqlClient.SqlDataAdapter("select a.* from process_head a where a.process_idno = " & Str(Val(idno)), con)
        da.Fill(dt)

        If dt.Rows.Count > 0 Then

            lbl_IdNo.Text = dt.Rows(0).Item("Process_IdNo").ToString
            txt_Name.Text = dt.Rows(0).Item("Process_Name").ToString

            'Cloth_Delivered  ,FP_Delivered ,  Cloth_Returned  ,FP_Returned

            If Not IsDBNull(dt.Rows(0).Item("Cloth_Delivered")) Then
                chk_Delivery_Cloth.Checked = dt.Rows(0).Item("Cloth_Delivered")
            End If

            If Not IsDBNull(dt.Rows(0).Item("FP_Delivered")) Then
                chk_Delivery_FinishedProduct.Checked = dt.Rows(0).Item("FP_Delivered")
            End If

            If Not IsDBNull(dt.Rows(0).Item("Cloth_Returned")) Then
                chk_Return_Cloth.Checked = dt.Rows(0).Item("Cloth_Returned")
            End If

            If Not IsDBNull(dt.Rows(0).Item("FP_Returned")) Then
                chk_Retuen_FinishedProduct.Checked = dt.Rows(0).Item("FP_Returned")
            End If

        End If

        dt.Dispose()
        da.Dispose()

        If txt_Name.Enabled And txt_Name.Visible Then txt_Name.Focus()

    End Sub

    Private Sub Process_Creation_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        grp_Open.Left = 6
        grp_Open.Top = 280
        grp_Open.Visible = False

        grp_Filter.Left = 6
        grp_Filter.Top = 280
        grp_Filter.Visible = False

        con.Open()

        Me.Top = Me.Top - 100

        If Common_Procedures.settings.CustomerCode = "1516" Then

            lbl_Del_Type_Caption.Visible = True
            chk_Delivery_Cloth.Visible = True
            chk_Delivery_FinishedProduct.Visible = True

            lbl_Return_Type_Caption.Visible = True
            chk_Return_Cloth.Visible = True
            chk_Retuen_FinishedProduct.Visible = True

        Else

            lbl_Del_Type_Caption.Visible = False
            chk_Delivery_Cloth.Visible = False
            chk_Delivery_FinishedProduct.Visible = False

            lbl_Return_Type_Caption.Visible = False
            chk_Return_Cloth.Visible = False
            chk_Retuen_FinishedProduct.Visible = False

        End If

        new_record()

    End Sub

    Private Sub Process_Creation_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        con.Close()
        con.Dispose()
    End Sub

    Private Sub Process_Creation_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If Asc(e.KeyChar) = 27 Then
            If grp_Filter.Visible Then
                btn_CloseFilter_Click(sender, e)
            ElseIf grp_Open.Visible Then
                btn_CloseOpen_Click(sender, e)
            Else
                Me.Close()
            End If

        End If

    End Sub

    Public Sub delete_record() Implements Interface_MDIActions.delete_record
        Dim cmd As New SqlClient.SqlCommand

        '  If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.Process_Creation, "~L~") = 0 And InStr(Common_Procedures.UR.Process_Creation, "~D~") = 0 Then MessageBox.Show("You have No Rights to Delete", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error) : Exit Sub

        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.DeletingEntry, Common_Procedures.UR.FP_Process_Creation, New_Entry, Me) = False Then Exit Sub

        If MessageBox.Show("Do you want to Delete ?", "FOR DELETION....", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.No Then
            Exit Sub
        End If

        If New_Entry = True Then
            MessageBox.Show("This is new entry", "DOES NOT DELETION....", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        Try

            'da = New SqlClient.SqlDataAdapter("select count(*) from item_head where Area_IdNo = " & Str(Val(txt_IdNo.Text)), con)
            'dt = New DataTable
            'da.Fill(dt)
            'If dt.Rows.Count > 0 Then
            '    If IsDBNull(dt.Rows(0)(0).ToString) = False Then
            '        If Val(dt.Rows(0)(0).ToString) > 0 Then
            '            MessageBox.Show("Already used this Process", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            '            Exit Sub
            '        End If
            '    End If
            'End If

            cmd.Connection = con
            cmd.CommandText = "delete from Process_Head where Process_IdNo = " & Str(Val(lbl_IdNo.Text))

            cmd.ExecuteNonQuery()

            cmd.Dispose()

            new_record()

            MessageBox.Show("Deleted Successfully!!!", "FOR DELETION...", MessageBoxButtons.OK, MessageBoxIcon.Information)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally
            If txt_Name.Enabled And txt_Name.Visible Then txt_Name.Focus()

        End Try

    End Sub

    Public Sub filter_record() Implements Interface_MDIActions.filter_record
        Dim da As New SqlClient.SqlDataAdapter("select Process_IdNo, Process_Name from Process_Head where Process_IdNo <> 0 order by Process_IdNo", con)
        Dim dt As New DataTable

        da.Fill(dt)

        With dgv_Filter

            .Columns.Clear()
            .DataSource = dt

            .RowHeadersVisible = False

            .AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells
            .AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill

            .Columns(0).HeaderText = "IDNO"
            .Columns(1).HeaderText = "PROCESS NAME"

            .Columns(0).FillWeight = 40
            .Columns(1).FillWeight = 160

        End With

        new_record()

        grp_Filter.Visible = True

        pnl_Back.Enabled = False

        If dgv_Filter.Enabled And dgv_Filter.Visible Then dgv_Filter.Focus()

        Me.Height = 540

        da.Dispose()
    End Sub

    Public Sub insert_record() Implements Interface_MDIActions.insert_record
        '----
    End Sub

    Public Sub movefirst_record() Implements Interface_MDIActions.movefirst_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim movid As Integer = 0

        Try
            da = New SqlClient.SqlDataAdapter("select min(process_idno) from process_head Where process_idno <> 0", con)
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
            da = New SqlClient.SqlDataAdapter("select max(process_idno) from process_head Where process_idno <> 0", con)
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
            da = New SqlClient.SqlDataAdapter("select min(process_idno) from process_head Where process_idno > " & Str(Val(lbl_IdNo.Text)) & " and process_idno <> 0", con)
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
            da = New SqlClient.SqlDataAdapter("select max(process_idno) from process_head Where process_idno < " & Str(Val(lbl_IdNo.Text)) & " and process_idno <> 0", con)
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

        CLEAR()

        New_Entry = True
        lbl_IdNo.ForeColor = Color.Red

        lbl_IdNo.Text = Common_Procedures.get_MaxIdNo(con, "Process_Head", "Process_IdNo", "")

        If txt_Name.Enabled And txt_Name.Visible Then txt_Name.Focus()

    End Sub

    Public Sub open_record() Implements Interface_MDIActions.open_record
        Dim da As New SqlClient.SqlDataAdapter("select process_Name from process_Head order by process_Name", con)
        Dim dt As New DataTable

        da.Fill(dt)

        cbo_Open.DataSource = dt
        cbo_Open.DisplayMember = "process_Name"

        new_record()

        Me.Height = 515
        grp_Open.Visible = True
        pnl_Back.Enabled = False
        If cbo_Open.Enabled And cbo_Open.Visible Then cbo_Open.Focus()

    End Sub

    Public Sub print_record() Implements Interface_MDIActions.print_record
        '----
    End Sub

    Public Sub save_record() Implements Interface_MDIActions.save_record

        Dim trans As SqlClient.SqlTransaction
        Dim cmd As New SqlClient.SqlCommand
        Dim Sur As String

        Dim Del_Clo As String = "0"
        Dim Del_FP As String = "0"

        Dim Rec_Clo As String = "0"
        Dim Rec_FP As String = "0"

        '  If Common_Procedures.UserRight_Check(Common_Procedures.UR.Process_Creation, New_Entry) = False Then Exit Sub

        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.SavingEntry, Common_Procedures.UR.FP_Process_Creation, New_Entry, Me) = False Then Exit Sub

        If pnl_Back.Enabled = False Then
            MessageBox.Show("Close Other Windows", "DOES NOT SAVE", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        If Trim(txt_Name.Text) = "" Then
            MessageBox.Show("Invalid Name", "DOES NOT SAVE", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If txt_Name.Enabled Then txt_Name.Focus()
            Exit Sub
        End If

        Sur = Common_Procedures.Remove_NonCharacters(Trim(txt_Name.Text))

        If chk_Delivery_Cloth.Checked Then Del_Clo = "1"
        If chk_Delivery_FinishedProduct.Checked Then Del_FP = "1"
        If chk_Return_Cloth.Checked Then Rec_Clo = "1"
        If chk_Retuen_FinishedProduct.Checked Then Rec_FP = "1"

        trans = con.BeginTransaction

        Try

            cmd.Connection = con
            cmd.Transaction = trans

            If New_Entry = True Then

                lbl_IdNo.Text = Common_Procedures.get_MaxIdNo(con, "Process_Head", "Process_IdNo", "", trans)

                cmd.CommandText = "Insert into Process_Head(Process_IdNo, Process_Name, sur_name,Cloth_Delivered  ,FP_Delivered ,  Cloth_Returned  ,FP_Returned) values (" & Str(Val(lbl_IdNo.Text)) & ", '" & Trim(txt_Name.Text) & "', '" & Trim(Sur) & "'," & Del_Clo.ToString & "," & Del_FP.ToString & "," & Rec_Clo.ToString & "," & Rec_FP.ToString & ")"
                cmd.ExecuteNonQuery()

            Else

                cmd.CommandText = "update Process_Head set Process_Name = '" & Trim(txt_Name.Text) & "', sur_name = '" & Trim(Sur) & "',Cloth_Delivered = " & Del_Clo.ToString & " ,FP_Delivered = " & Del_FP.ToString & ",  Cloth_Returned = " & Rec_Clo.ToString & " ,FP_Returned = " & Rec_FP.ToString & " where Process_IdNo = " & Str(Val(lbl_IdNo.Text)) & ""
                cmd.ExecuteNonQuery()

            End If

            trans.Commit()

            Common_Procedures.Master_Return.Return_Value = Trim(txt_Name.Text)
            Common_Procedures.Master_Return.Master_Type = "PROCESS"



            MessageBox.Show("Saved Successfully!!!", "FOR SAVING....", MessageBoxButtons.OK, MessageBoxIcon.Information)

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
            If InStr(1, Trim(LCase(ex.Message)), "ix_process_head") > 0 Then
                MessageBox.Show("Duplicate Process Name", "DOES NOT SAVE", MessageBoxButtons.OK, MessageBoxIcon.Error)

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

    Private Sub btn_Close_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Close.Click
        Me.Close()
    End Sub

    Private Sub txt_Name_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Name.KeyPress

        If Asc(e.KeyChar) = 13 Then

            If chk_Delivery_Cloth.Visible Then
                SendKeys.Send("{Tab}    ")
            Else
                If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                    save_record()
                End If
            End If

        End If

    End Sub

    Private Sub btn_CloseOpen_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_CloseOpen.Click
        Me.Height = 290
        pnl_Back.Enabled = True
        grp_Open.Visible = False
        If txt_Name.Enabled And txt_Name.Visible Then txt_Name.Focus()
    End Sub

    Private Sub btn_Open_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Open.Click
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim movid As Integer

        da = New SqlClient.SqlDataAdapter("select Process_IdNo from process_Head where process_Name = '" & Trim(cbo_Open.Text) & "'", con)
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

    Private Sub cbo_Open_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Open.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Process_Head", "Process_Name", "", "")

    End Sub

    Private Sub cbo_Open_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Open.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Open, Nothing, Nothing, "Process_Head", "Process_Name", "", "")
    End Sub

    Private Sub cbo_Open_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Open.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Open, Nothing, "Process_Head", "Process_Name", "", "")

    End Sub

    Private Sub btn_CloseFilter_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_CloseFilter.Click
        Me.Height = 290
        pnl_Back.Enabled = True
        grp_Filter.Visible = False
    End Sub

    Private Sub btn_Filter_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Filter.Click
        Dim idno As Integer

        idno = Val(dgv_Filter.CurrentRow.Cells(0).Value)

        If Val(idno) <> 0 Then
            move_record(idno)
            pnl_Back.Enabled = True
            grp_Filter.Visible = False
        End If
    End Sub

    Private Sub dgv_Filter_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgv_Filter.DoubleClick
        Call btn_Filter_Click(sender, e)
    End Sub

    Private Sub dgv_Filter_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_Filter.KeyDown
        If e.KeyValue = 13 Then
            Call btn_Filter_Click(sender, e)
        End If
    End Sub

    Private Sub Label3_Click(sender As Object, e As EventArgs) Handles lbl_Return_Type_Caption.Click

    End Sub

    Private Sub txt_Name_TextChanged(sender As Object, e As EventArgs) Handles txt_Name.TextChanged

    End Sub

    Private Sub txt_Name_KeyDown(sender As Object, e As KeyEventArgs) Handles txt_Name.KeyDown

        If e.KeyCode = 40 Then

            If chk_Delivery_Cloth.Visible Then
                SendKeys.Send("{Tab")
            Else
                If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                    save_record()
                End If
            End If

        End If



    End Sub



    Private Sub chk_Delivery_Cloth_KeyPress(sender As Object, e As KeyPressEventArgs) Handles chk_Delivery_Cloth.KeyPress

        If Asc(e.KeyChar) = 13 Then
            SendKeys.Send("{Tab}")
        End If

    End Sub

    Private Sub chk_Delivery_Cloth_KeyDown(sender As Object, e As KeyEventArgs) Handles chk_Delivery_Cloth.KeyDown
        If e.KeyCode = 40 Then
            SendKeys.Send("{Tab}")
        End If
        If e.KeyCode = 38 Then
            SendKeys.Send("{+Tab}")
        End If
    End Sub

    Private Sub chk_Delivery_FinishedProduct_CheckedChanged(sender As Object, e As EventArgs) Handles chk_Delivery_FinishedProduct.CheckedChanged

    End Sub

    Private Sub chk_Delivery_FinishedProduct_KeyPress(sender As Object, e As KeyPressEventArgs) Handles chk_Delivery_FinishedProduct.KeyPress
        If Asc(e.KeyChar) = 13 Then
            SendKeys.Send("{Tab}")
        End If
    End Sub

    Private Sub chk_Return_Cloth_CheckedChanged(sender As Object, e As EventArgs) Handles chk_Return_Cloth.CheckedChanged

    End Sub

    Private Sub chk_Return_Cloth_KeyPress(sender As Object, e As KeyPressEventArgs) Handles chk_Return_Cloth.KeyPress
        If Asc(e.KeyChar) = 13 Then
            SendKeys.Send("{Tab}")
        End If
    End Sub

    Private Sub chk_Delivery_Cloth_CheckedChanged(sender As Object, e As EventArgs) Handles chk_Delivery_Cloth.CheckedChanged

    End Sub

    Private Sub chk_Delivery_FinishedProduct_KeyDown(sender As Object, e As KeyEventArgs) Handles chk_Delivery_FinishedProduct.KeyDown
        If e.KeyCode = 40 Then
            SendKeys.Send("{Tab}")
        End If
        If e.KeyCode = 38 Then
            SendKeys.Send("{+Tab}")
        End If
    End Sub

    Private Sub chk_Return_Cloth_KeyDown(sender As Object, e As KeyEventArgs) Handles chk_Return_Cloth.KeyDown
        If e.KeyCode = 40 Then
            SendKeys.Send("{Tab}")
        End If
        If e.KeyCode = 38 Then
            SendKeys.Send("{+Tab}")
        End If
    End Sub


    Private Sub chk_Retuen_FinishedProduct_KeyDown(sender As Object, e As KeyEventArgs) Handles chk_Retuen_FinishedProduct.KeyDown


        If e.KeyCode = 38 Then
            SendKeys.Send("{+Tab}")
        End If

        If e.KeyCode = 40 Then


            If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                save_record()
            Else
                txt_Name.Focus()
            End If

        End If

    End Sub

    Private Sub chk_Retuen_FinishedProduct_KeyPress(sender As Object, e As KeyPressEventArgs) Handles chk_Retuen_FinishedProduct.KeyPress
        If Asc(e.KeyChar) = 13 Then
            If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                save_record()
            Else
                txt_Name.Focus()
            End If
        End If
    End Sub

End Class