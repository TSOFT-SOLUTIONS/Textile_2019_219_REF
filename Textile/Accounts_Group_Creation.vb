Public Class Accounts_Group_Creation
    Implements Interface_MDIActions

    Private con As New SqlClient.SqlConnection(Common_Procedures.Connection_String)
    Private New_Entry As Boolean = False

    Private Sub clear()

        New_Entry = False

        pnl_Back.Enabled = True
        grp_Find.Visible = False
        grp_Filter.Visible = False

        Me.Height = 303  ' 284

        lbl_IdNo.Text = ""
        lbl_IdNo.ForeColor = Color.Black

        txt_Name.Text = ""
        Cbo_Group.Text = ""
        cbo_Find.Text = ""
        'dgv_Filter.Rows.Clear()

    End Sub

    Public Sub move_record(ByVal idno As Integer)
        Dim cmd As New SqlClient.SqlCommand
        Dim dr As SqlClient.SqlDataReader

        If Val(idno) = 0 Then Exit Sub

        clear()

        Try
            cmd.Connection = con
            cmd.CommandText = "select * from AccountsGroup_Head where AccountsGroup_IdNo = " & Str(Val(idno))

            dr = cmd.ExecuteReader

            If dr.HasRows Then
                If dr.Read() Then
                    lbl_IdNo.Text = dr("AccountsGroup_IdNo").ToString()
                    txt_Name.Text = dr("AccountsGroup_Name").ToString()
                    Cbo_Group.Text = dr("Parent_Name").ToString()
                    'txt_ProdMtrs_Day.Text = dr("Lomm_Production_Capacity_Day").ToString()
                End If
            End If

            dr.Close()

            cmd.Dispose()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT MOVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally

            cmd.Dispose()

            If txt_Name.Enabled And txt_Name.Visible Then txt_Name.Focus()

        End Try

    End Sub

    Private Sub Accounts_Group_Creation_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        On Error Resume Next
        con.Close()
        con.Dispose()
        Common_Procedures.Last_Closed_FormName = Me.Name
    End Sub

    Private Sub Accounts_Group_Creation_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If Asc(e.KeyChar) = 27 Then
            If grp_Find.Visible Then
                btnClose_Click(sender, e)
            ElseIf grp_Filter.Visible Then
                btn_CloseFilter_Click(sender, e)
            Else
                If MessageBox.Show("Do you want to Close?", "FOR CLOSING ENTRY...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) <> Windows.Forms.DialogResult.Yes Then
                    Exit Sub

                Else
                    Me.Close()

                End If

            End If
        End If
    End Sub

    Private Sub Accounts_Group_Creation_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Me.Text = ""
        con.Open()

        da = New SqlClient.SqlDataAdapter("select AccountsGroup_Name from AccountsGroup_Head  order by AccountsGroup_Name", con)

        'da = New SqlClient.SqlDataAdapter("select AccountsGroup_Name from AccountsGroup_Head Where (AccountsGroup_IdNo < 100) order by AccountsGroup_Name", con)
        da.Fill(dt1)
        Cbo_Group.DataSource = dt1
        Cbo_Group.DisplayMember = "AccountsGroup_Name"

        Me.Height = 303  ' 284 ' 197

        new_record()
    End Sub

    Private Sub btn_Close_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Close.Click
        Me.Close()
        Me.Dispose()
    End Sub

    Public Sub delete_record() Implements Interface_MDIActions.delete_record
        Dim cmd As New SqlClient.SqlCommand
        Dim da As SqlClient.SqlDataAdapter
        Dim dt As DataTable


        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.DeletingEntry, Common_Procedures.UR.Account_Group_creation, New_Entry, Me) = False Then Exit Sub


        If MessageBox.Show("Do you want to Delete ?", "FOR DELETION....", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.No Then
            Exit Sub
        End If

        If Val(lbl_IdNo.Text) <= 32 Then
            MessageBox.Show("Cannot delete this default group", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If


        ''------- check whether it is lready used


        da = New SqlClient.SqlDataAdapter("select count(*) from LEDGER_HEAD where ACCOUNTSGROUP_Idno = " & Str(Val(lbl_IdNo.Text)), con)
        dt = New DataTable
        da.Fill(dt)
        If dt.Rows.Count > 0 Then
            If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                If Val(dt.Rows(0)(0).ToString) > 0 Then
                    MessageBox.Show("Already used this Accounts Group", "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                    Exit Sub
                End If
            End If
        End If
        dt.Clear()

        Try


            cmd.Connection = con
            cmd.CommandText = "delete from AccountsGroup_Head where AccountsGroup_IdNo = " & Str(Val(lbl_IdNo.Text))

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
        Dim da As New SqlClient.SqlDataAdapter("select AccountsGroup_IdNo, AccountsGroup_Name from AccountsGroup_Head where AccountsGroup_IdNo > 100 order by AccountsGroup_IdNo", con)
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

            .Columns(0).FillWeight = 40
            .Columns(1).FillWeight = 160

        End With

        new_record()

        grp_Filter.Visible = True
        grp_Filter.Left = grp_Find.Left
        grp_Filter.Top = grp_Find.Top

        pnl_Back.Enabled = False

        If dgv_Filter.Enabled And dgv_Filter.Visible Then dgv_Filter.Focus()

        Me.Height = 520 ' 400

        dt.Dispose()
        da.Dispose()

    End Sub

    Public Sub movefirst_record() Implements Interface_MDIActions.movefirst_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim movid As Integer = 0

        Try
            'da = New SqlClient.SqlDataAdapter("select min(AccountsGroup_IdNo) from AccountsGroup_Head Where AccountsGroup_IdNo > 100", con)

            da = New SqlClient.SqlDataAdapter("select min(AccountsGroup_IdNo) from AccountsGroup_Head ", con)
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
            'cmd.CommandText = "select max(AccountsGroup_IdNo) from AccountsGroup_Head WHERE AccountsGroup_IdNo > 100"
            cmd.CommandText = "select max(AccountsGroup_IdNo) from AccountsGroup_Head "

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
            cmd.CommandText = "select min(AccountsGroup_IdNo) from AccountsGroup_Head where AccountsGroup_IdNo > " & Str(Val(lbl_IdNo.Text))

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
            'da = New SqlClient.SqlDataAdapter("select max(AccountsGroup_IdNo) from AccountsGroup_Head where AccountsGroup_IdNo < " & Str(Val(lbl_IdNo.Text)) & " and AccountsGroup_IdNo > 100 ", con)

            da = New SqlClient.SqlDataAdapter("select max(AccountsGroup_IdNo) from AccountsGroup_Head where AccountsGroup_IdNo < " & Str(Val(lbl_IdNo.Text)) & " ", con)
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

        lbl_IdNo.Text = Common_Procedures.get_MaxIdNo(con, "AccountsGroup_Head", "AccountsGroup_IdNo", "")

        'If Val(lbl_IdNo.Text) < 101 Then lbl_IdNo.Text = 101

        If txt_Name.Enabled And txt_Name.Visible Then txt_Name.Focus()

    End Sub

    Public Sub open_record() Implements Interface_MDIActions.open_record
        Dim da As New SqlClient.SqlDataAdapter("select AccountsGroup_Name from AccountsGroup_Head WHERE AccountsGroup_IdNo > 100 order by AccountsGroup_Name", con)
        Dim dt As New DataTable

        da.Fill(dt)

        cbo_Find.DataSource = dt
        cbo_Find.DisplayMember = "AccountsGroup_Name"

        new_record()

        grp_Find.Visible = True
        pnl_Back.Enabled = False

        If cbo_Find.Enabled And cbo_Find.Visible Then cbo_Find.Focus()
        Me.Height = 500  ' 480 ' 355

    End Sub

    Public Sub insert_record() Implements Interface_MDIActions.insert_record
        'MessageBox.Show("insert record")
    End Sub

    Public Sub print_record() Implements Interface_MDIActions.print_record
        '--- No Printing
    End Sub

    Public Sub save_record() Implements Interface_MDIActions.save_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim cmd As New SqlClient.SqlCommand
        Dim trans As SqlClient.SqlTransaction
        Dim Sur As String
        Dim Prn_id As String = ""
        Dim Ac_id As Integer = 0
        Dim undgrp_idno As Integer = 0
        Dim undgrp_ParntCD As String = ""
        Dim taly_nme As String = ""
        Dim taly_subnme As String = ""
        Dim carr_bal As Integer = 0
        Dim ord_po As Single = 0
        Dim indi As Integer = 0

        '  If Common_Procedures.UserRight_Check(Common_Procedures.UR.Masters_AccountsGroup_Creations, New_Entry) = False Then Exit Sub

        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.SavingEntry, Common_Procedures.UR.Account_Group_creation, New_Entry, Me) = False Then Exit Sub

        If Trim(txt_Name.Text) = "" Then
            MessageBox.Show("Invalid Name", "DOES NOT SAVE", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub

        End If

        If Trim(Cbo_Group.Text) = "" Then
            MessageBox.Show("Invalid Group Name", "DOES NOT SAVE", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        undgrp_idno = Common_Procedures.AccountsGroup_NameToIdNo(con, Trim(Cbo_Group.Text))

        undgrp_ParntCD = Common_Procedures.AccountsGroup_IdNoToCode(con, undgrp_idno)

        Prn_id = "~" & Trim(Val(lbl_IdNo.Text)) & Trim(undgrp_ParntCD)

        Sur = Common_Procedures.Remove_NonCharacters(Trim(txt_Name.Text))

        da = New SqlClient.SqlDataAdapter("select a.* from AccountsGroup_Head a Where a.Parent_Idno = '" & Trim(undgrp_ParntCD) & "'", con)
        dt = New DataTable
        da.Fill(dt)

        If dt.Rows.Count > 0 Then
            If IsDBNull(dt.Rows(0)(0).ToString) = False Then

                taly_nme = Trim(dt.Rows(0).Item("TallyName").ToString)
                taly_subnme = Trim(dt.Rows(0).Item("TallySubName").ToString)
                ord_po = Format(Val(dt.Rows(0).Item("Order_Position").ToString), "#######0.00")
                carr_bal = Val(dt.Rows(0).Item("Carried_Balance").ToString)
                indi = Val(dt.Rows(0).Item("Indicate").ToString)

            End If
        End If

        dt.Dispose()
        da.Dispose()

        trans = con.BeginTransaction

        Try
            cmd.Connection = con
            cmd.Transaction = trans

            If New_Entry = True Then

                lbl_IdNo.Text = Common_Procedures.get_MaxIdNo(con, "AccountsGroup_Head", "AccountsGroup_IdNo", "", trans)
                'If Val(lbl_IdNo.Text) <= 100 Then lbl_IdNo.Text = 101

                cmd.CommandText = "Insert into AccountsGroup_Head( AccountsGroup_IdNo , AccountsGroup_Name, sur_name, Parent_Name , Parent_Idno , Carried_Balance , Order_Position , TallyName , TallySubName ,Indicate ) values (" & Str(Val(lbl_IdNo.Text)) & ", '" & Trim(txt_Name.Text) & "', '" & Trim(Sur) & "', '" & Trim(Cbo_Group.Text) & "' , '" & Trim(Prn_id) & "' , " & Str(Val(carr_bal)) & "," & Str(Val(ord_po)) & ", '" & Trim(taly_nme) & "','" & Trim(taly_subnme) & "'," & Str(Val(indi)) & " )"
                cmd.ExecuteNonQuery()

            Else

                cmd.CommandText = "update AccountsGroup_Head set AccountsGroup_Name = '" & Trim(txt_Name.Text) & "', sur_name = '" & Trim(Sur) & "', Parent_Name = '" & Trim(Cbo_Group.Text) & "' , Parent_Idno = '" & Trim(Prn_id) & "' , Carried_Balance =  " & Str(Val(carr_bal)) & " , Order_Position =  " & Str(Val(ord_po)) & " , TallyName = '" & Trim(taly_nme) & "'  , TallySubName = '" & Trim(taly_subnme) & "'  ,Indicate = " & Str(Val(indi)) & "  where AccountsGroup_IdNo = " & Str(Val(lbl_IdNo.Text))
                cmd.ExecuteNonQuery()

            End If

            trans.Commit()

            Common_Procedures.Master_Return.Return_Value = Trim(txt_Name.Text)
            Common_Procedures.Master_Return.Master_Type = "LOOMNO"



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

            If InStr(1, Trim(LCase(ex.Message)), "ix_AccountsGroup_Head") > 0 Then
                MessageBox.Show("Duplicate Account Name", "DOES NOT SAVE", MessageBoxButtons.OK, MessageBoxIcon.Error)

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

        da = New SqlClient.SqlDataAdapter("select AccountsGroup_IdNo from AccountsGroup_Head where AccountsGroup_Name = '" & Trim(cbo_Find.Text) & "'", con)
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
        Me.Height = 303  ' 284 ' 197
        pnl_Back.Enabled = True
        grp_Find.Visible = False
        If txt_Name.Enabled And txt_Name.Visible Then txt_Name.Focus()
    End Sub

    Private Sub cbo_Find_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Find.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "AccountsGroup_Head", "AccountsGroup_Name", "", "(AccountsGroup_IdNo = 0)")
    End Sub

    Private Sub cbo_Find_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Find.KeyDown
        Try
            Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Find, Nothing, Nothing, "AccountsGroup_Head", "AccountsGroup_Name", "", "(AccountsGroup_IdNo = 0)")

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "DOES NOT SELECT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub cbo_Find_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Find.KeyPress

        Try

            With cbo_Find

                If Asc(e.KeyChar) <> 27 Then

                    Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Find, Nothing, "AccountsGroup_Head", "AccountsGroup_Name", "", "(AccountsGroup_IdNo = 0)")

                    If Asc(e.KeyChar) = 13 Then

                        btn_Find_Click(sender, e)

                    End If

                End If

            End With

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT SELECT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub btn_CloseFilter_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_CloseFilter.Click

        pnl_Back.Enabled = True
        grp_Filter.Visible = False
        If txt_Name.Enabled And txt_Name.Visible Then txt_Name.Focus()

        Me.Height = 303  ' 284 '197

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

    Private Sub txt_Name_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_Name.GotFocus
        With txt_Name
            .BackColor = Color.lime
            .ForeColor = Color.Blue
            .SelectAll()
        End With
    End Sub

    Private Sub txt_Name_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_Name.KeyDown
        If e.KeyCode = 40 Then SendKeys.Send("{TAB}")
        'If e.KeyCode = 38 Then cbo.Focus() ' SendKeys.Send("+{TAB}")
    End Sub

    Private Sub txt_Name_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Name.KeyPress
        If Asc(e.KeyChar) = 13 Then SendKeys.Send("{TAB}")
    End Sub

    Private Sub txt_Name_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_Name.LostFocus
        With txt_Name
            .BackColor = Color.White
            .ForeColor = Color.Black
        End With
    End Sub

    Private Sub Cbo_Group_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles Cbo_Group.GotFocus
        With Cbo_Group
            .BackColor = Color.Lime
            .ForeColor = Color.Blue
            .SelectionStart = 0
            .SelectionLength = .Text.Length
        End With
    End Sub

    Private Sub Cbo_Group_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Cbo_Group.KeyDown

        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, Cbo_Group, Nothing, Nothing, "AccountsGroup_Head", "AccountsGroup_Name", "(AccountsGroup_IdNo < 100)", "")

        Try
            With Cbo_Group
                If e.KeyValue = 38 And .DroppedDown = False Then
                    e.Handled = True
                    txt_Name.Focus()
                    'SendKeys.Send("+{TAB}")
                ElseIf e.KeyValue = 40 And .DroppedDown = False Then
                    e.Handled = True
                    If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                        save_record()
                    Else
                        txt_Name.Focus()
                    End If
                ElseIf e.KeyValue <> 13 And e.KeyValue <> 17 And e.KeyValue <> 27 And .DroppedDown = False Then
                    .DroppedDown = True
                End If
            End With

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT SELECT ITEM...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub Cbo_Group_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Cbo_Group.KeyPress

        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, Cbo_Group, Nothing, "AccountsGroup_Head", "AccountsGroup_Name", "(AccountsGroup_IdNo < 100)", "")
        If Asc(e.KeyChar) = 13 And Cbo_Group.DroppedDown = False Then
            If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                save_record()
            Else
                txt_Name.Focus()
            End If
        End If

    End Sub

    Private Sub Cbo_Group_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles Cbo_Group.LostFocus
        Cbo_Group.BackColor = Color.White
        Cbo_Group.ForeColor = Color.Black
    End Sub

    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

    End Sub
End Class