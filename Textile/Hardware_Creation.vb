Public Class Hardware_Creation
    Implements Interface_MDIActions

    Private con As New SqlClient.SqlConnection(Common_Procedures.Connection_String)
    Private New_Entry As Boolean = False

    Private Sub clear()
        New_Entry = False

        txt_IdNo.ForeColor = Color.Red
        txt_IdNo.Text = ""

        txt_Name.Text = ""
        txt_Code.Text = ""
        cbo_ItemGroup.Text = ""
        cbo_Unit.Text = ""
        txt_MinimumStock.Text = ""
        txt_TaxPerc.Text = ""
        txt_CostRate.Text = ""
        txt_Rate.Text = ""
        txt_TaxRate.Text = ""

        pnl_Back.Enabled = True
        grp_Filter.Visible = False
        grp_Open.Visible = False

    End Sub

    Private Sub move_record(ByVal idno As Integer)
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable

        If Val(idno) = 0 Then Exit Sub

        clear()

        Try

            da = New SqlClient.SqlDataAdapter("select a.Hardware_IdNo, a.Hardware_Name, a.Item_Code, b.ItemGroup_Name, c.Unit_Name, a.Minimum_Stock, a.Tax_Percentage, a.cost_rate, a.Sale_TaxRate, a.Sales_Rate from Hardware_Head a LEFT OUTER JOIN ItemGroup_Head b ON a.ItemGroup_IdNo = b.ItemGroup_IdNo LEFT OUTER JOIN Unit_Head c ON a.Unit_IdNo = c.Unit_IdNo where a.Hardware_IdNo = " & Str(Val(idno)), con)
            da.Fill(dt)

            If dt.Rows.Count > 0 Then
                If IsDBNull(dt.Rows(0).Item("Hardware_IdNo").ToString) = False Then
                    txt_IdNo.Text = dt.Rows(0).Item("Hardware_IdNo").ToString
                    txt_Name.Text = dt.Rows(0).Item("Hardware_Name").ToString
                    txt_Code.Text = dt.Rows(0).Item("Item_Code").ToString
                    cbo_ItemGroup.Text = dt.Rows(0).Item("ItemGroup_Name").ToString
                    cbo_Unit.Text = dt.Rows(0).Item("Unit_Name").ToString
                    txt_MinimumStock.Text = dt.Rows(0).Item("Minimum_Stock").ToString
                    txt_TaxPerc.Text = dt.Rows(0).Item("Tax_Percentage").ToString
                    txt_CostRate.Text = dt.Rows(0).Item("Cost_Rate").ToString
                    txt_Rate.Text = dt.Rows(0).Item("Sales_Rate").ToString
                    txt_TaxRate.Text = dt.Rows(0).Item("Sale_TaxRate").ToString
                End If
            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        If txt_Name.Visible And txt_Name.Enabled Then txt_Name.Focus()

    End Sub

    Public Sub delete_record() Implements Interface_MDIActions.delete_record
        Dim cmd As New SqlClient.SqlCommand

        ' If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.Item_Creation, "~L~") = 0 And InStr(Common_Procedures.UR.Item_Creation, "~D~") = 0 Then MessageBox.Show("You have No Rights to Delete", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error) : Exit Sub

        If MessageBox.Show("Do you want to Delete?", "FOR DELETION...", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.No Then
            Exit Sub
        End If
        If New_Entry = True Then
            MessageBox.Show("This is new entry", "DOES NOT DELETION....", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If
        Try
            cmd.Connection = con
            cmd.CommandText = "delete from Hardware_Head where Hardware_IdNo = " & Str(Val(txt_IdNo.Text))
            cmd.ExecuteNonQuery()

            new_record()

            MessageBox.Show("Deleted Sucessfully!!!", "FOR DELETION...", MessageBoxButtons.OK, MessageBoxIcon.Information)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR DELETION...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        If txt_Name.Enabled = True And txt_Name.Visible = True Then txt_Name.Focus()

    End Sub

    Public Sub filter_record() Implements Interface_MDIActions.filter_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable

        da = New SqlClient.SqlDataAdapter("select a.Hardware_IdNo, a.Hardware_Name, b.unit_name, a.Sale_TaxRate from Hardware_Head a, unit_head b where a.Hardware_IdNo <> 0 and a.unit_idno = b.unit_idno Order by a.Hardware_IdNo", con)
        da.Fill(dt)

        dgv_Filter.Columns.Clear()
        dgv_Filter.DataSource = dt

        dgv_Filter.RowHeadersVisible = False

        dgv_Filter.Columns(0).HeaderText = "IDNO"
        dgv_Filter.Columns(1).HeaderText = "ITEM NAME"
        dgv_Filter.Columns(2).HeaderText = "UNIT"
        dgv_Filter.Columns(2).HeaderText = "Sales_Rate"

        dgv_Filter.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill

        dgv_Filter.Columns(0).FillWeight = 40
        dgv_Filter.Columns(1).FillWeight = 240
        dgv_Filter.Columns(2).FillWeight = 60
        dgv_Filter.Columns(3).FillWeight = 60

        pnl_Back.Enabled = False
        grp_Filter.Visible = True

        dgv_Filter.BringToFront()
        dgv_Filter.Focus()

    End Sub

    Public Sub movefirst_record() Implements Interface_MDIActions.movefirst_record
        Dim cmd As New SqlClient.SqlCommand
        Dim dr As SqlClient.SqlDataReader
        Dim movid As Integer = 0

        Try
            cmd.Connection = con
            cmd.CommandText = "select min(Hardware_IdNo) from Hardware_Head where Hardware_IdNo <> 0"
            dr = cmd.ExecuteReader

            If dr.HasRows Then
                If dr.Read Then
                    If IsDBNull(dr(0).ToString) = False Then
                        movid = Val(dr(0).ToString)
                    End If
                End If
            End If

            dr.Close()

            If movid <> 0 Then move_record(movid)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR  MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Public Sub movelast_record() Implements Interface_MDIActions.movelast_record
        Dim da As New SqlClient.SqlDataAdapter("select max(Hardware_IdNo) from Hardware_Head where Hardware_IdNo <> 0", con)
        Dim dt As New DataTable
        Dim movid As Integer = 0

        Try
            da.Fill(dt)
            If dt.Rows.Count > 0 Then
                If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                    movid = Val(dt.Rows(0)(0).ToString)
                End If
            End If

            If movid <> 0 Then move_record(movid)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR  MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try

    End Sub

    Public Sub movenext_record() Implements Interface_MDIActions.movenext_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim movid As Integer = 0

        Try
            da = New SqlClient.SqlDataAdapter("select min(Hardware_IdNo) from Hardware_Head where Hardware_IdNo > " & Str(Val(txt_IdNo.Text)) & " and Hardware_IdNo <> 0", con)
            da.Fill(dt)

            If dt.Rows.Count > 0 Then
                If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                    movid = Val(dt.Rows(0)(0).ToString)
                End If
            End If

            If movid <> 0 Then move_record(movid)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR  MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Public Sub moveprevious_record() Implements Interface_MDIActions.moveprevious_record
        Dim cmd As New SqlClient.SqlCommand
        Dim dr As SqlClient.SqlDataReader
        Dim movid As Integer = 0

        Try
            cmd.Connection = con
            cmd.CommandText = "select max(Hardware_IdNo) from Hardware_Head where Hardware_IdNo < " & Str(Val(txt_IdNo.Text)) & " and Hardware_IdNo <> 0"

            dr = cmd.ExecuteReader

            If dr.HasRows Then
                If dr.Read Then
                    If IsDBNull(dr(0).ToString) = False Then
                        movid = Val(dr(0).ToString)
                    End If
                End If
            End If
            dr.Close()
            If Val(movid) <> 0 Then move_record(movid)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR  MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try


    End Sub

    Public Sub new_record() Implements Interface_MDIActions.new_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim newid As Integer = 0

        clear()
        New_Entry = True

        da = New SqlClient.SqlDataAdapter("select max(Hardware_IdNo) from Hardware_Head", con)
        da.Fill(dt)

        If dt.Rows.Count > 0 Then
            If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                newid = Val(dt.Rows(0)(0).ToString)
            End If
        End If

        newid = newid + 1

        txt_IdNo.Text = newid
        txt_IdNo.ForeColor = Color.Red

        If txt_Name.Enabled And txt_Name.Visible Then txt_Name.Focus()

    End Sub

    Public Sub open_record() Implements Interface_MDIActions.open_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable

        da = New SqlClient.SqlDataAdapter("select Hardware_Name from Hardware_Head order by Hardware_Name", con)
        da.Fill(dt)

        'cbo_Open.Items.Clear()

        cbo_Open.DataSource = dt
        cbo_Open.DisplayMember = "Hardware_Name"

        grp_Open.Visible = True
        pnl_Back.Enabled = False
        If cbo_Open.Enabled And cbo_Open.Visible Then cbo_Open.Focus()

    End Sub

    Public Sub insert_record() Implements Interface_MDIActions.insert_record
        '-----
    End Sub

    Public Sub print_record() Implements Interface_MDIActions.print_record
        '-----
    End Sub

    Public Sub save_record() Implements Interface_MDIActions.save_record
        Dim cmd As New SqlClient.SqlCommand
        Dim tr As SqlClient.SqlTransaction
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim dt2 As New DataTable
        Dim new_entry As Boolean = False
        Dim nr As Long = 0
        Dim itmgrp_id As Integer = 0
        Dim unt_id As Integer = 0

        ' If Common_Procedures.UserRight_Check(Common_Procedures.UserRightsCheckFor.SavingEntry, Common_Procedures.UR.Item_Creation, new_entry) = False Then Exit Sub

        If Trim(txt_Name.Text) = "" Then
            MessageBox.Show("Invalid Item Name", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If txt_Name.Enabled Then txt_Name.Focus()
            Exit Sub
        End If

        da = New SqlClient.SqlDataAdapter("select itemgroup_idno from itemgroup_head where itemgroup_name = '" & Trim(cbo_ItemGroup.Text) & "'", con)
        da.Fill(dt)

        If dt.Rows.Count > 0 Then
            If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                itmgrp_id = Val(dt.Rows(0)(0).ToString)
            End If
        End If

        dt.Clear()

        da = New SqlClient.SqlDataAdapter("select unit_idno from unit_head where unit_name = '" & Trim(cbo_Unit.Text) & "'", con)
        da.Fill(dt2)

        unt_id = 0
        If dt2.Rows.Count > 0 Then
            If IsDBNull(dt2.Rows(0)(0).ToString) = False Then
                unt_id = Val(dt2.Rows(0)(0).ToString)
            End If
        End If

        If Val(unt_id) = 0 Then
            MessageBox.Show("Invalid Unit", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If cbo_Unit.Enabled Then cbo_Unit.Focus()
            Exit Sub
        End If

        tr = con.BeginTransaction

        Try
            cmd.Connection = con
            cmd.Transaction = tr
            cmd.CommandText = "update Hardware_Head set Hardware_Name = '" & Trim(txt_Name.Text) & "', Sur_Name = '" & Trim(txt_Name.Text) & "', Item_Code = '" & Trim(txt_Code.Text) & "', ItemGroup_IdNo = " & Str(Val(itmgrp_id)) & ", Unit_IdNo = " & Str(Val(unt_id)) & ", Minimum_Stock = " & Str(Val(txt_MinimumStock.Text)) & ", Tax_Percentage = " & Str(Val(txt_TaxPerc.Text)) & ", Sale_TaxRate = " & Str(Val(txt_TaxRate.Text)) & ", Sales_Rate = " & Str(Val(txt_Rate.Text)) & ", Cost_Rate = " & Str(Val(txt_CostRate.Text)) & " where Hardware_IdNo = " & Str(Val(txt_IdNo.Text))

            nr = cmd.ExecuteNonQuery

            If nr = 0 Then
                cmd.CommandText = "Insert into Hardware_Head(Hardware_IdNo, Hardware_Name, Sur_Name, Item_Code, ItemGroup_IdNo, Unit_IdNo, Minimum_Stock, Tax_Percentage, Sale_TaxRate, Sales_Rate, Cost_Rate) values (" & Str(Val(txt_IdNo.Text)) & ", '" & Trim(txt_Name.Text) & "', '" & Trim(txt_Name.Text) & "', '" & Trim(txt_Code.Text) & "', " & Str(Val(itmgrp_id)) & ", " & Str(Val(unt_id)) & ", " & Str(Val(txt_MinimumStock.Text)) & ", " & Str(Val(txt_TaxPerc.Text)) & ", " & Str(Val(txt_TaxRate.Text)) & ", " & Str(Val(txt_Rate.Text)) & ", " & Str(Val(txt_CostRate.Text)) & ")"
                cmd.ExecuteNonQuery()
                new_entry = True
            End If

            tr.Commit()

            Common_Procedures.Master_Return.Return_Value = Trim(txt_Name.Text)
            Common_Procedures.Master_Return.Master_Type = "HARDWARE"



            MessageBox.Show("Saved Sucessfully!!!", "FOR SAVING...", MessageBoxButtons.OK, MessageBoxIcon.Information)

            If Val(Common_Procedures.settings.OnSave_MoveTo_NewEntry_Status) = 1 Then
                If new_entry = True Then
                    new_record()
                Else
                    move_record(txt_IdNo.Text)
                End If
            Else
                move_record(txt_IdNo.Text)
            End If

        Catch ex As Exception
            tr.Rollback()
            MessageBox.Show(ex.Message, "FOR SAVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        If txt_Name.Enabled And txt_Name.Visible Then txt_Name.Focus()

    End Sub

    Private Sub Hardware_Creation_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        On Error Resume Next
        con.Close()
        con.Dispose()
        Common_Procedures.Last_Closed_FormName = Me.Name
    End Sub

    Private Sub Hardware_Creation_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If Asc(e.KeyChar) = 27 Then
            If grp_Filter.Visible Then
                Call btn_CloseFilter_Click(sender, e)
                Exit Sub
            End If
            If grp_Open.Visible Then
                Call btnClose_Click(sender, e)
                Exit Sub
            End If
            Me.Close()
        End If
    End Sub

    Private Sub Hardware_Creation_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable



        con.Open()

        da = New SqlClient.SqlDataAdapter("select itemgroup_name from itemgroup_head order by itemgroup_name", con)
        da.Fill(dt1)

        cbo_ItemGroup.Items.Clear()

        cbo_ItemGroup.DataSource = dt1
        cbo_ItemGroup.DisplayMember = "itemgroup_name"
        'cbo_ItemGroup.ValueMember = "itemgroup_idno"

        da = New SqlClient.SqlDataAdapter("select unit_name from unit_head order by unit_name", con)

        da.Fill(dt2)

        cbo_Unit.DataSource = dt2
        cbo_Unit.DisplayMember = "unit_name"
        'cbo_Unit.ValueMember = "unit_idno"

        grp_Open.Visible = False
        grp_Open.Left = (Me.Width - grp_Open.Width) - 50
        grp_Open.Top = (Me.Height - grp_Open.Height) - 50
        grp_Open.BringToFront()

        grp_Filter.Visible = False
        grp_Filter.Left = (Me.Width - grp_Filter.Width) - 25
        grp_Filter.Top = (Me.Height - grp_Filter.Height) - 55
        grp_Filter.BringToFront()

        new_record()

    End Sub

    Private Sub cbo_Open_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Open.GotFocus
        cbo_Open.BackColor = Color.Lime
        cbo_Open.ForeColor = Color.Blue
        'cbo_Open.DroppedDown = True
    End Sub

    Private Sub cbo_Open_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Open.KeyDown
        Try
            With cbo_Open
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
            'MessageBox.Show(ex.Message, "DOES NOT SELECT ITEM...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub cbo_Open_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Open.KeyPress
        Dim Indx As Integer
        Dim FindStr As String

        On Error Resume Next

        If Asc(e.KeyChar) = 13 Then
            btn_Find_Click(sender, e)
        End If

        If Asc(e.KeyChar) = 8 Then
            If cbo_Open.SelectionStart <= 1 Then
                cbo_Open.Text = ""
                Exit Sub
            End If

            If cbo_Open.SelectionLength = 0 Then
                FindStr = cbo_Open.Text.Substring(0, cbo_Open.Text.Length - 1)
            Else
                FindStr = cbo_Open.Text.Substring(0, cbo_Open.SelectionStart - 1)
            End If

        Else

            If cbo_Open.SelectionLength = 0 Then
                FindStr = cbo_Open.Text & e.KeyChar
            Else
                FindStr = cbo_Open.Text.Substring(0, cbo_Open.SelectionStart) & e.KeyChar
            End If

        End If

        Indx = cbo_Open.FindString(FindStr)

        If Indx <> -1 Then
            cbo_Open.SelectedText = ""
            cbo_Open.SelectedIndex = Indx
            cbo_Open.SelectionStart = FindStr.Length
            cbo_Open.SelectionLength = cbo_Open.Text.Length
            e.Handled = True

        Else
            If Asc(e.KeyChar) <> 8 Then e.Handled = True

        End If



    End Sub

    Private Sub txt_Name_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_Name.GotFocus
        txt_Name.BackColor = Color.Lime
        txt_Name.ForeColor = Color.Blue
        SendKeys.Send("{HOME}+{END}")
    End Sub

    Private Sub txt_Name_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_Name.KeyDown
        If e.KeyValue = 40 Then
            SendKeys.Send("{TAB}")
        End If
        If e.KeyValue = 38 Then
            SendKeys.Send("+{TAB}")
        End If
    End Sub

    Private Sub txt_Name_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Name.KeyPress
        If Asc(e.KeyChar) = 13 Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub btn_Find_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Find.Click
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim movid As Integer = 0

        Try
            da = New SqlClient.SqlDataAdapter("select Hardware_IdNo from Hardware_Head where Hardware_Name = '" & Trim(cbo_Open.Text) & "'", con)
            da.Fill(dt)

            If dt.Rows.Count > 0 Then
                If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                    movid = Val(dt.Rows(0)(0).ToString)
                End If
            End If

            If movid <> 0 Then
                move_record(movid)
                btnClose_Click(sender, e)
            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR FINDING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        'Me.Height = 400

    End Sub

    Private Sub btnClose_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClose.Click
        pnl_Back.Enabled = True
        grp_Open.Visible = False
    End Sub

    Private Sub cbo_ItemGroup_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_ItemGroup.GotFocus
        cbo_ItemGroup.BackColor = Color.Lime
        cbo_ItemGroup.ForeColor = Color.Blue
        cbo_ItemGroup.SelectionStart = 0
        cbo_ItemGroup.SelectionLength = cbo_ItemGroup.Text.Length
    End Sub

    Private Sub cbo_ItemGroup_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_ItemGroup.KeyDown
        If e.KeyValue = 38 And cbo_ItemGroup.DroppedDown = False Then
            e.Handled = True
            txt_Code.Focus()
            'SendKeys.Send("+{TAB}")
        ElseIf e.KeyValue = 40 And cbo_ItemGroup.DroppedDown = False Then
            e.Handled = True
            cbo_Unit.Focus()
            'SendKeys.Send("{TAB}")
        ElseIf e.KeyValue <> 13 And cbo_ItemGroup.DroppedDown = False Then
            cbo_ItemGroup.DroppedDown = True
        End If
    End Sub

    Private Sub cbo_ItemGroup_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_ItemGroup.KeyPress
        Dim Indx As Integer = -1
        Dim strFindStr As String = ""

        Try
            If Asc(e.KeyChar) = 8 Then
                If cbo_ItemGroup.SelectionStart <= 1 Then
                    cbo_ItemGroup.Text = ""
                    Exit Sub
                End If
                If cbo_ItemGroup.SelectionLength = 0 Then
                    strFindStr = cbo_ItemGroup.Text.Substring(0, cbo_ItemGroup.Text.Length - 1)
                Else
                    strFindStr = cbo_ItemGroup.Text.Substring(0, cbo_ItemGroup.SelectionStart - 1)
                End If

            Else

                If cbo_ItemGroup.SelectionLength = 0 Then
                    strFindStr = cbo_ItemGroup.Text & e.KeyChar
                Else
                    strFindStr = cbo_ItemGroup.Text.Substring(0, cbo_ItemGroup.SelectionStart) & e.KeyChar
                End If

            End If

            Indx = cbo_ItemGroup.FindString(strFindStr)

            If Indx <> -1 Then
                cbo_ItemGroup.SelectedText = ""
                cbo_ItemGroup.SelectedIndex = Indx
                cbo_ItemGroup.SelectionStart = strFindStr.Length
                cbo_ItemGroup.SelectionLength = cbo_ItemGroup.Text.Length
                e.Handled = True
            Else
                If Asc(e.KeyChar) <> 8 Then e.Handled = True

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT SELECT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        If Asc(e.KeyChar) = 13 Then
            SendKeys.Send("{TAB}")
        End If

    End Sub

    Private Sub cbo_Unit_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Unit.GotFocus
        cbo_Unit.BackColor = Color.Lime
        cbo_Unit.ForeColor = Color.Blue
        cbo_Unit.SelectionStart = 0
        cbo_Unit.SelectionLength = cbo_Unit.Text.Length
    End Sub

    Private Sub cbo_Unit_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Unit.KeyDown
        If e.KeyValue = 38 And cbo_Unit.DroppedDown = False Then
            e.Handled = True
            cbo_ItemGroup.Focus()
            'SendKeys.Send("+{TAB}")
        ElseIf e.KeyValue = 40 And cbo_Unit.DroppedDown = False Then
            e.Handled = True
            txt_MinimumStock.Focus()
            'SendKeys.Send("{TAB}")
        ElseIf e.KeyValue <> 13 And cbo_Unit.DroppedDown = False Then
            cbo_Unit.DroppedDown = True
        End If
    End Sub

    Private Sub txt_TaxPerc_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_TaxPerc.GotFocus
        txt_TaxPerc.BackColor = Color.Lime
        txt_TaxPerc.ForeColor = Color.Blue
        SendKeys.Send("{HOME}+{END}")
    End Sub

    Private Sub txt_TaxPerc_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_TaxPerc.KeyDown
        If e.KeyCode = 40 Then SendKeys.Send("{TAB}")
        If e.KeyCode = 38 Then SendKeys.Send("+{TAB}")
    End Sub

    Private Sub txt_TaxPerc_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_TaxPerc.KeyPress
        If Not ((Asc(e.KeyChar) >= 48 And Asc(e.KeyChar) <= 57) Or Asc(e.KeyChar) = 46 Or Asc(e.KeyChar) = 8 Or Asc(e.KeyChar) = 13) Then
            e.Handled = True
        End If

        If Asc(e.KeyChar) = 13 Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub btn_OpenFilter_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_OpenFilter.Click
        Dim movid As Integer = 0

        Try
            movid = Val(dgv_Filter.CurrentRow.Cells(0).Value)

            If Val(movid) <> 0 Then
                move_record(movid)
                pnl_Back.Enabled = True
                grp_Filter.Visible = False
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message)

        End Try
    End Sub

    Private Sub btn_CloseFilter_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_CloseFilter.Click
        pnl_Back.Enabled = True
        grp_Filter.Visible = False
    End Sub

    Private Sub dgv_Filter_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_Filter.KeyDown
        If e.KeyValue = 13 Then
            Call btn_OpenFilter_Click(sender, e)
        End If
    End Sub

    Private Sub txt_Rate_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_Rate.GotFocus
        txt_Rate.BackColor = Color.Lime
        txt_Rate.ForeColor = Color.Blue
        SendKeys.Send("{HOME}+{END}")
    End Sub

    Private Sub txt_Rate_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_Rate.KeyDown
        If e.KeyCode = 40 Then SendKeys.Send("{TAB}")
        If e.KeyCode = 38 Then SendKeys.Send("+{TAB}")
    End Sub

    Private Sub txt_Rate_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_Rate.KeyUp
        txt_TaxRate.Text = Format(Val(txt_Rate.Text) * ((100 + Val(txt_TaxPerc.Text)) / 100), "##########0.00")
    End Sub

    Private Sub txt_TaxRate_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_TaxRate.GotFocus
        txt_TaxRate.BackColor = Color.Lime
        txt_TaxRate.ForeColor = Color.Blue
        SendKeys.Send("{HOME}+{END}")
    End Sub

    Private Sub txt_TaxRate_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_TaxRate.KeyUp
        txt_Rate.Text = Format(Val(txt_TaxRate.Text) * (100 / (100 + Val(txt_TaxPerc.Text))), "#########0.00")
    End Sub

    Private Sub txt_TaxPerc_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_TaxPerc.KeyUp
        txt_TaxRate.Text = Format(Val(txt_Rate.Text) * ((100 + Val(txt_TaxPerc.Text)) / 100), "########0.00")
    End Sub

    Private Sub txt_TaxRate_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_TaxRate.KeyPress
        If Not ((Asc(e.KeyChar) >= 48 And Asc(e.KeyChar) <= 57) Or Asc(e.KeyChar) = 46 Or Asc(e.KeyChar) = 8 Or Asc(e.KeyChar) = 13) Then
            e.Handled = True
        End If

        If Asc(e.KeyChar) = 13 Then
            If MessageBox.Show("Do you want to save ?", "FOR SAVING...", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) Then
                save_record()
            End If
        End If
    End Sub

    Private Sub txt_Code_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_Code.GotFocus
        txt_Code.BackColor = Color.Lime
        txt_Code.ForeColor = Color.Blue
        SendKeys.Send("{HOME}+{END}")
    End Sub

    Private Sub txt_Code_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_Code.KeyDown
        If e.KeyCode = 40 Then SendKeys.Send("{TAB}")
        If e.KeyCode = 38 Then SendKeys.Send("+{TAB}")
    End Sub

    Private Sub txt_Code_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Code.KeyPress
        If Asc(e.KeyChar) = 13 Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub txt_CostRate_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_CostRate.GotFocus
        txt_CostRate.BackColor = Color.Lime
        txt_CostRate.ForeColor = Color.Blue
        SendKeys.Send("{HOME}+{END}")
    End Sub

    Private Sub txt_CostRate_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_CostRate.KeyDown
        If e.KeyCode = 40 Then SendKeys.Send("{TAB}")
        If e.KeyCode = 38 Then SendKeys.Send("+{TAB}")
    End Sub

    Private Sub txt_CostRate_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_CostRate.KeyPress
        If Not ((Asc(e.KeyChar) >= 48 And Asc(e.KeyChar) <= 57) Or Asc(e.KeyChar) = 46 Or Asc(e.KeyChar) = 8 Or Asc(e.KeyChar) = 13) Then
            e.Handled = True
        End If

        If Asc(e.KeyChar) = 13 Then SendKeys.Send("{TAB}")
    End Sub

    Private Sub cbo_Unit_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Unit.KeyPress
        Dim FindStr As String = ""
        Dim Indx As Integer = -1

        If Asc(e.KeyChar) = 13 Then
            SendKeys.Send("{TAB}")
        End If

        If Asc(e.KeyChar) = 8 Then
            If cbo_Unit.SelectionStart <= 1 Then
                cbo_Unit.Text = ""
                Exit Sub
            End If

            If cbo_Unit.SelectionLength = 0 Then
                FindStr = cbo_Unit.Text.Substring(0, cbo_Unit.Text.Length - 1)
            Else
                FindStr = cbo_Unit.Text.Substring(0, cbo_Unit.SelectionStart - 1)
            End If

        Else
            If cbo_Unit.SelectionLength = 0 Then
                FindStr = cbo_Unit.Text & e.KeyChar
            Else
                FindStr = cbo_Unit.Text.Substring(0, cbo_Unit.SelectionStart) & e.KeyChar
            End If

        End If

        Indx = cbo_Unit.FindString(FindStr)

        If Indx <> -1 Then
            cbo_Unit.SelectedText = ""
            cbo_Unit.SelectedIndex = Indx
            cbo_Unit.SelectionStart = FindStr.Length
            cbo_Unit.SelectionLength = cbo_Unit.Text.Length
        End If
        e.Handled = True

    End Sub

    Private Sub txt_TaxRate_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_TaxRate.KeyDown
        If e.KeyCode = 40 Then
            If MessageBox.Show("Do you want to save ?", "FOR SAVING...", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) Then
                save_record()
            End If
        End If
        If e.KeyCode = 38 Then SendKeys.Send("+{TAB}")
    End Sub

    Private Sub txt_Rate_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Rate.KeyPress
        If Not ((Asc(e.KeyChar) >= 48 And Asc(e.KeyChar) <= 57) Or Asc(e.KeyChar) = 46 Or Asc(e.KeyChar) = 8 Or Asc(e.KeyChar) = 13) Then
            e.Handled = True
        End If

        If Asc(e.KeyChar) = 13 Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub txt_Name_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_Name.LostFocus
        txt_Name.BackColor = Color.White
        txt_Name.ForeColor = Color.Black
    End Sub

    Private Sub cbo_ItemGroup_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_ItemGroup.LostFocus
        cbo_ItemGroup.BackColor = Color.White
        cbo_ItemGroup.ForeColor = Color.Black
    End Sub

    Private Sub cbo_Open_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Open.LostFocus
        cbo_Open.BackColor = Color.White
        cbo_Open.ForeColor = Color.Black
    End Sub

    Private Sub cbo_Unit_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Unit.LostFocus
        cbo_Unit.BackColor = Color.White
        cbo_Unit.ForeColor = Color.Black
    End Sub

    Private Sub txt_Code_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_Code.LostFocus
        txt_Code.BackColor = Color.White
        txt_Code.ForeColor = Color.Black
    End Sub

    Private Sub txt_CostRate_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_CostRate.LostFocus
        txt_CostRate.BackColor = Color.White
        txt_CostRate.ForeColor = Color.Black
    End Sub

    Private Sub txt_Rate_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_Rate.LostFocus
        txt_Rate.BackColor = Color.White
        txt_Rate.ForeColor = Color.Black
    End Sub

    Private Sub txt_TaxPerc_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_TaxPerc.LostFocus
        txt_TaxPerc.BackColor = Color.White
        txt_TaxPerc.ForeColor = Color.Black
    End Sub

    Private Sub txt_TaxRate_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_TaxRate.LostFocus
        txt_TaxRate.BackColor = Color.White
        txt_TaxRate.ForeColor = Color.Black
    End Sub

    Private Sub txt_MinimumStock_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_MinimumStock.GotFocus
        txt_MinimumStock.BackColor = Color.Lime
        txt_MinimumStock.ForeColor = Color.Black
    End Sub

    Private Sub txt_MinimumStock_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_MinimumStock.KeyDown
        If e.KeyCode = 40 Then SendKeys.Send("{TAB}")
        If e.KeyCode = 38 Then SendKeys.Send("+{TAB}")
    End Sub

    Private Sub txt_MinimumStock_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_MinimumStock.KeyPress
        If Not ((Asc(e.KeyChar) >= 48 And Asc(e.KeyChar) <= 57) Or Asc(e.KeyChar) = 46 Or Asc(e.KeyChar) = 8 Or Asc(e.KeyChar) = 13) Then
            e.Handled = True
        End If
        If Asc(e.KeyChar) = 13 Then SendKeys.Send("{TAB}")
    End Sub

    Private Sub btn_Close_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Close.Click
        Me.Close()
    End Sub

    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        save_record()
    End Sub

    Private Sub txt_MinimumStock_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_MinimumStock.LostFocus
        txt_MinimumStock.BackColor = Color.White
        txt_MinimumStock.ForeColor = Color.Black
    End Sub
End Class