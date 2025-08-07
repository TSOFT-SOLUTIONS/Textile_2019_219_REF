
Public Class Mill_Creation
    Implements Interface_MDIActions

    Dim con As New SqlClient.SqlConnection(Common_Procedures.Connection_String)
    Dim New_Entry As Boolean
    Private Prec_ActCtrl As New Control
    Private WithEvents dgtxt_Details As New DataGridViewTextBoxEditingControl
    Private SizTo_DbName As String = ""
    Private vcbo_KeyDwnVal As Double

    Private Sub clear()

        pnl1_back.Enabled = True
        grp_Open.Visible = False
        ''grp_Filter.Visible = False
        cbo_count.Visible = False
        lbl_IdNo.Text = ""
        lbl_IdNo.ForeColor = Color.Black
        txt_MillName.Text = ""
        txt_weightemptybag.Text = ""
        txt_weightemptycone.Text = ""
        cbo_Sizing_MillName.Text = ""
        dgv_countdetails.Rows.Clear()
        New_Entry = False
        txt_TamilName.Text = ""
    End Sub

    Public Sub delete_record() Implements Interface_MDIActions.delete_record
        Dim cmd As New SqlClient.SqlCommand
        Dim da As SqlClient.SqlDataAdapter
        Dim dt As DataTable

        '    If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.Mill_Creation, "~L~") = 0 And InStr(Common_Procedures.UR.Mill_Creation, "~D~") = 0 Then MessageBox.Show("You have No Rights to Delete", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error) : Exit Sub

        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.DeletingEntry, Common_Procedures.UR.Mill_Creation, New_Entry, Me) = False Then Exit Sub


        If MessageBox.Show("Do you want to Delete ?", "FOR DELETION....", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.No Then
            Exit Sub
        End If

        If New_Entry = True Then
            MessageBox.Show("This is new entry", "DOES NOT DELETION....", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        Try

            da = New SqlClient.SqlDataAdapter("select count(*) from Stock_Yarn_Processing_Details where mill_idno = " & Str(Val(lbl_IdNo.Text)), con)
            dt = New DataTable
            da.Fill(dt)
            If dt.Rows.Count > 0 Then
                If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                    If Val(dt.Rows(0)(0).ToString) > 0 Then
                        MessageBox.Show("Already used this Mill", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        Exit Sub
                    End If
                End If
            End If
            dt.Clear()

            da = New SqlClient.SqlDataAdapter("select count(*) from Yarn_Sales_Details where mill_idno = " & Str(Val(lbl_IdNo.Text)), con)
            dt = New DataTable
            da.Fill(dt)
            If dt.Rows.Count > 0 Then
                If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                    If Val(dt.Rows(0)(0).ToString) > 0 Then
                        MessageBox.Show("Already used this Mill", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        Exit Sub
                    End If
                End If
            End If
            dt.Clear()

            cmd.Connection = con
            cmd.CommandText = "delete from Mill_Count_Details where mill_idno = " & Str(Val(lbl_IdNo.Text))
            cmd.ExecuteNonQuery()

            cmd.Connection = con
            cmd.CommandText = "delete from Mill_Head where Mill_IdNo = " & Str(Val(lbl_IdNo.Text))

            cmd.ExecuteNonQuery()

            cmd.Dispose()

            new_record()

            MessageBox.Show("Deleted Sucessfully!!!", "FOR DELETION...", MessageBoxButtons.OK, MessageBoxIcon.Information)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally
            If txt_MillName.Enabled And txt_MillName.Visible Then txt_MillName.Focus()

        End Try
    End Sub

    'Public Sub filter_record() Implements Interface_MDIActions.filter_record
    '    Dim da As New SqlClient.SqlDataAdapter("select count_IdNo, Count_Name,Count_Description from Count_Head where Count_IdNo <> 0 order by Count_IdNo", con)
    '    Dim dt As New DataTable

    '    da.Fill(dt)

    '    With dgv_Filter

    '        .Columns.Clear()
    '        .DataSource = dt

    '        .RowHeadersVisible = False

    '        .AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells
    '        .AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill

    '        .Columns(0).HeaderText = "IDNO"
    '        .Columns(1).HeaderText = "NAME"
    '        .Columns(2).HeaderText = "DESCRIPTION"


    '        .Columns(0).FillWeight = 60
    '        .Columns(1).FillWeight = 160
    '        .Columns(2).FillWeight = 300


    '    End With

    '    new_record()

    '    grp_Filter.Visible = True

    '    pnl_back.Enabled = False

    '    If dgv_Filter.Enabled And dgv_Filter.Visible Then dgv_Filter.Focus()

    '    Me.Height = 514

    '    da.Dispose()
    'End Sub

    Public Sub insert_record() Implements Interface_MDIActions.insert_record
        '-----
    End Sub

    Public Sub move_record(ByVal idno As Integer)
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim dt2 As New DataTable
        Dim sno As Integer, n As Integer


        If Val(idno) = 0 Then Exit Sub

        clear()

        ''da = New SqlClient.SqlDataAdapter("select a.*, b.count_name as stock_undername from Count_head a LEFT OUTER JOIN count_head b ON a.Count_StockUnder_IdNo = b.count_idno where a.Count_idno = " & Str(Val(idno)), con)
        da = New SqlClient.SqlDataAdapter("select * from mill_head where Mill_idno = " & Str(Val(idno)), con)
        da.Fill(dt)

        If dt.Rows.Count > 0 Then
            lbl_IdNo.Text = dt.Rows(0).Item("Mill_IdNo").ToString
            txt_MillName.Text = dt.Rows(0).Item("Mill_Name").ToString
            txt_weightemptybag.Text = dt.Rows(0).Item("Weight_EmptyBag").ToString
            txt_weightemptycone.Text = dt.Rows(0).Item("Weight_EmptyCone").ToString
            txt_TamilName.Text = dt.Rows(0)("Tamil_Name").ToString
            cbo_Sizing_MillName.Text = Common_Procedures.Mill_IdNoToName(con, Val(dt.Rows(0).Item("Sizing_To_MillIdNo").ToString), , SizTo_DbName)
            da = New SqlClient.SqlDataAdapter("select a.*, b.count_Name  from Mill_Count_Details a, count_Head b where a.mill_idno = " & Str(Val(idno)) & " and a.count_idno = b.count_idno Order by a.sl_no", con)
            da.Fill(dt2)

            dgv_countdetails.Rows.Clear()
            sno = 0

            If dt2.Rows.Count > 0 Then

                For i = 0 To dt2.Rows.Count - 1

                    n = dgv_countdetails.Rows.Add()

                    sno = sno + 1
                    dgv_countdetails.Rows(n).Cells(0).Value = Val(sno)
                    dgv_countdetails.Rows(n).Cells(1).Value = dt2.Rows(i).Item("count_Name").ToString
                    dgv_countdetails.Rows(n).Cells(2).Value = Format(Val(dt2.Rows(i).Item("Weight_Bag")), "#########0.000")
                    dgv_countdetails.Rows(n).Cells(3).Value = Val(dt2.Rows(i).Item("Cones_Bag"))
                    dgv_countdetails.Rows(n).Cells(4).Value = Format(Val(dt2.Rows(i).Item("Weight_Cone")), "#########0.000")
                    dgv_countdetails.Rows(n).Cells(5).Value = Format(Val(dt2.Rows(i).Item("Rate_Kg")), "#########0.00")
                    dgv_countdetails.Rows(n).Cells(6).Value = Format(Val(dt2.Rows(i).Item("Rate_Thiri")), "#########0.00")

                Next i

                For i = 0 To dgv_countdetails.RowCount - 1
                    dgv_countdetails.Rows(i).Cells(0).Value = Val(i) + 1


                Next

            End If


        End If

        dt.Dispose()
        da.Dispose()

        If txt_MillName.Enabled And txt_MillName.Visible Then txt_MillName.Focus()
    End Sub
    Public Sub movefirst_record() Implements Interface_MDIActions.movefirst_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim movid As Integer = 0

        Try
            da = New SqlClient.SqlDataAdapter("select min(Mill_idno) from Mill_head Where Mill_idno <> 0", con)
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
            da = New SqlClient.SqlDataAdapter("select max(mill_idno) from Mill_head Where Mill_idno <> 0", con)
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
            da = New SqlClient.SqlDataAdapter("select min(mill_idno) from mill_head Where Mill_idno > " & Str(Val(lbl_IdNo.Text)) & " and Mill_idno <> 0", con)
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
            da = New SqlClient.SqlDataAdapter("select max(Mill_idno) from Mill_head Where Mill_idno < " & Str(Val(lbl_IdNo.Text)) & " and Mill_idno <> 0", con)
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

        lbl_IdNo.Text = Common_Procedures.get_MaxIdNo(con, "Mill_Head", "Mill_IdNo", "")

        If txt_MillName.Enabled And txt_MillName.Visible Then txt_MillName.Focus()
    End Sub

    'Public Sub open_record() Implements Interface_MDIActions.open_record
    '    Dim da As New SqlClient.SqlDataAdapter("select Count_Name from Count_Head order by Count_Name", con)
    '    Dim dt As New DataTable

    '    da.Fill(dt)

    '    cbo_Find.DataSource = dt
    '    cbo_Find.DisplayMember = "Count_Name"

    '    new_record()

    '    Me.Height = 513
    '    grp_find.Visible = True
    '    pnl_back.Enabled = False
    '    If cbo_Find.Enabled And cbo_Find.Visible Then cbo_Find.Focus()
    'End Sub

    Public Sub print_record() Implements Interface_MDIActions.print_record

    End Sub

    Public Sub save_record() Implements Interface_MDIActions.save_record
        Dim trans As SqlClient.SqlTransaction
        Dim cmd As New SqlClient.SqlCommand
        Dim Sur As String
        Dim cnt_id As Integer
        Dim SNo As Integer
        Dim Sizstk_id As Integer = 0
        '  If Common_Procedures.UserRight_Check(Common_Procedures.UR.Mill_Creation, New_Entry) = False Then Exit Sub

        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.SavingEntry, Common_Procedures.UR.Mill_Creation, New_Entry, Me) = False Then Exit Sub


        If pnl_back.Enabled = False Then
            MessageBox.Show("Close Other Windows", "DOES NOT SAVE", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        If Trim(txt_MillName.Text) = "" Then
            MessageBox.Show("Invalid MillName", "DOES NOT SAVE", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        Sur = Common_Procedures.Remove_NonCharacters(Trim(txt_MillName.Text))

        Sizstk_id = Common_Procedures.Mill_NameToIdNo(con, cbo_Sizing_MillName.Text, , SizTo_DbName)
        If cbo_Sizing_MillName.Visible Then
            If Trim(cbo_Sizing_MillName.Text) <> "" Then
                If Val(Sizstk_id) = 0 Then
                    MessageBox.Show("Invalid Sizing Mill Name", "DOES NOT SAVE", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    If cbo_Sizing_MillName.Enabled Then cbo_Sizing_MillName.Focus()
                    Exit Sub
                End If
            End If
        End If
        trans = con.BeginTransaction
        Try

            cmd.Connection = con
            cmd.Transaction = trans

            If New_Entry = True Then

                lbl_IdNo.Text = Common_Procedures.get_MaxIdNo(con, "Mill_Head", "Mill_IdNo", "", trans)

                cmd.CommandText = "Insert into Mill_Head(Mill_IdNo, Mill_Name, Sur_Name,Weight_EmptyBag,Weight_EmptyCone,Tamil_Name,Sizing_To_MillIdNo) values (" & Str(Val(lbl_IdNo.Text)) & ", '" & Trim(txt_MillName.Text) & "', '" & Trim(Sur) & "', " & Val(txt_weightemptybag.Text) & ", " & Val(txt_weightemptycone.Text) & ",'" & Trim(txt_TamilName.Text) & "'," & Val(Sizstk_id) & ")"
                cmd.ExecuteNonQuery()

            Else
                cmd.CommandText = "update Mill_Head set Mill_Name = '" & Trim(txt_MillName.Text) & "', Sur_Name = '" & Trim(Sur) & "', Weight_EmptyBag = " & Val(txt_weightemptybag.Text) & ", Weight_EmptyCone = " & Val(txt_weightemptycone.Text) & ",Tamil_Name ='" & Trim(txt_TamilName.Text) & "',Sizing_To_MillIdNo = " & Val(Sizstk_id) & " Where Mill_IdNo = " & Str(Val(lbl_IdNo.Text))
                cmd.ExecuteNonQuery()

            End If

            cmd.CommandText = "delete from Mill_Count_Details where mill_idno = " & Str(Val(lbl_IdNo.Text))
            cmd.ExecuteNonQuery()

            With dgv_countdetails
                SNo = 0
                For i = 0 To .RowCount - 1
                    cnt_id = Common_Procedures.Count_NameToIdNo(con, .Rows(i).Cells(1).Value, trans)
                    If Val(cnt_id) <> 0 Then
                        SNo = SNo + 1
                        cmd.CommandText = "Insert into Mill_Count_Details(Mill_IdNo, sl_no, Count_IdNo, Weight_Bag, Cones_Bag, Weight_Cone , Rate_Kg , Rate_Thiri) values (" & Str(Val(lbl_IdNo.Text)) & ", " & Str(Val(SNo)) & ", " & Str(Val(cnt_id)) & ", " & Str(Val(.Rows(i).Cells(2).Value)) & ", " & Str(Val(.Rows(i).Cells(3).Value)) & ", " & Str(Val(.Rows(i).Cells(4).Value)) & " ,  " & Str(Val(.Rows(i).Cells(5).Value)) & ", " & Str(Val(.Rows(i).Cells(6).Value)) & " )"
                        cmd.ExecuteNonQuery()
                    End If
                Next

            End With

            trans.Commit()

            Common_Procedures.Master_Return.Return_Value = Trim(txt_MillName.Text)
            Common_Procedures.Master_Return.Master_Type = "Mill"



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
            If InStr(1, Trim(LCase(ex.Message)), "ix_mill_head") > 0 Then
                MessageBox.Show("Duplicate Mill Name", "DOES NOT SAVE", MessageBoxButtons.OK, MessageBoxIcon.Error)

            Else
                MessageBox.Show(ex.Message, "DOES NOT SAVE", MessageBoxButtons.OK, MessageBoxIcon.Error)

            End If

        Finally
            If txt_MillName.Enabled And txt_MillName.Visible Then txt_MillName.Focus()
        End Try
    End Sub

    Private Sub btn_Close_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_close.Click
        Me.Close()

    End Sub

    Public Sub filter_record() Implements Interface_MDIActions.filter_record
        '-----
    End Sub

    Public Sub open_record() Implements Interface_MDIActions.open_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable

        da = New SqlClient.SqlDataAdapter("select Mill_Name from Mill_Head order by mill_Name", con)
        da.Fill(dt)

        cbo_Open.DataSource = dt
        cbo_Open.DisplayMember = "mill_Name"

        da.Dispose()

        grp_Open.Visible = True
        grp_Open.BringToFront()
        If cbo_Open.Enabled And cbo_Open.Visible Then cbo_Open.Focus()
        pnl1_back.Enabled = False

    End Sub

    Private Sub Mill_Entry_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        con.Close()
        con.Dispose()
    End Sub

    Private Sub Mill_Entry_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If Asc(e.KeyChar) = 27 Then
            '  If grp_filter.Visible Then
            'btn_FilterClose_Click(sender, e)
            If grp_Open.Visible Then
                btn_CloseOpen_Click(sender, e)

            Else
                Me.Close()
            End If

        End If


    End Sub
    Private Sub ControlGotFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim txtbx As TextBox
        Dim combobx As ComboBox
        On Error Resume Next

        If TypeOf Me.ActiveControl Is TextBox Or TypeOf Me.ActiveControl Is ComboBox Or TypeOf Me.ActiveControl Is Button Or TypeOf Me.ActiveControl Is CheckBox Then
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

        If Me.ActiveControl.Name <> dgv_countdetails.Name Then
            Grid_DeSelect()
        End If

        Grid_DeSelect()
        Prec_ActCtrl = Me.ActiveControl

    End Sub
    Private Sub Grid_DeSelect()
        On Error Resume Next
        If Not IsNothing(dgv_countdetails.CurrentCell) Then dgv_countdetails.CurrentCell.Selected = False

    End Sub
    Private Sub ControlLostFocus(ByVal sender As Object, ByVal e As System.EventArgs)

        On Error Resume Next

        If IsNothing(Prec_ActCtrl) = False Then
            If TypeOf Prec_ActCtrl Is TextBox Or TypeOf Prec_ActCtrl Is ComboBox Then
                Prec_ActCtrl.BackColor = Color.White
                Prec_ActCtrl.ForeColor = Color.Black
            ElseIf TypeOf Prec_ActCtrl Is Button Then
                Prec_ActCtrl.BackColor = Color.FromArgb(41, 57, 85)
                Prec_ActCtrl.ForeColor = Color.White
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
    Private Sub Mill_Entry_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        con.Open()

        cbo_Sizing_MillName.Visible = False
        lbl_Sizing.Visible = False

        If Common_Procedures.settings.Combine_Textile_SizingSOftware = 1 Then
            SizTo_DbName = Common_Procedures.get_Company_SizingDataBaseName(Trim(Val(Common_Procedures.CompGroupIdNo)))
            cbo_Sizing_MillName.Visible = True
            lbl_Sizing.Visible = True
          
            SizTo_DbName = Common_Procedures.get_Company_DataBaseName(Trim(Val(Common_Procedures.CompGroupIdNo)))
        End If

        da = New SqlClient.SqlDataAdapter("select count_Name from count_Head order by count_Name", con)
        da.Fill(dt)
        cbo_count.DataSource = dt
        cbo_count.DisplayMember = "count_Name"
        grp_Open.Visible = False
        grp_Open.Left = (Me.Width - grp_Open.Width)
        grp_Open.Top = (Me.Height - grp_Open.Height)

        AddHandler txt_MillName.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_weightemptybag.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_weightemptycone.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_count.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Open.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Sizing_MillName.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_TamilName.GotFocus, AddressOf ControlGotFocus

        AddHandler txt_MillName.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_weightemptybag.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_weightemptycone.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Open.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_count.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Sizing_MillName.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_TamilName.LostFocus, AddressOf ControlLostFocus

        AddHandler txt_MillName.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_weightemptybag.KeyDown, AddressOf TextBoxControlKeyDown
       
        AddHandler txt_MillName.KeyPress, AddressOf TextBoxControlKeyPress
         AddHandler txt_weightemptybag.KeyPress, AddressOf TextBoxControlKeyPress

        txt_TamilName.Visible = False
        lbl_tamilname.Visible = False
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1114" Then '---- Sundara Mills
            txt_TamilName.Visible = True
            lbl_tamilname.Visible = True
        End If

        new_record()
    End Sub

    Private Sub btn_save_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_save.Click
        save_record()
    End Sub

    Private Sub dgv_countdetails_CellEndEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_countdetails.CellEndEdit
        dgv_countdetails_CellLeave(sender, e)
    End Sub

    Private Sub dgv_countdetails_CellEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_countdetails.CellEnter
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim Dt2 As New DataTable
        Dim Dt3 As New DataTable
        Dim rect As Rectangle
        Dim n As Integer

        With dgv_countdetails

            n = .RowCount
            .Rows(n - 1).Cells(0).Value = Val(n)

            If e.ColumnIndex = 1 Then

                If cbo_count.Visible = False Or Val(cbo_count.Tag) <> e.RowIndex Then

                    cbo_count.Tag = -1
                    Da = New SqlClient.SqlDataAdapter("select Count_Name from Count_Head order by Count_Name", con)
                    Dt1 = New DataTable
                    Da.Fill(Dt1)
                    cbo_count.DataSource = Dt1
                    cbo_count.DisplayMember = "Count_Name"

                    rect = .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False)

                    cbo_count.Left = .Left + rect.Left  '  .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Left
                    cbo_count.Top = .Top + rect.Top  ' .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Top

                    cbo_count.Width = rect.Width  ' .CurrentCell.Size.Width
                    cbo_count.Height = rect.Height  ' rect.Height
                    cbo_count.Text = .CurrentCell.Value  '  Trim(.CurrentRow.Cells(e.ColumnIndex).Value)

                    cbo_count.Tag = Val(e.RowIndex)
                    cbo_count.Visible = True

                    cbo_count.BringToFront()
                    cbo_count.Focus()

                    'cbo_Grid_MillName.Visible = False
                    'cbo_Grid_YarnType.Visible = False

                End If


            Else

                cbo_count.Visible = False
                'cbo_Grid_CountName.Tag = -1
                'cbo_Grid_CountName.Text = ""

            End If
        End With
    End Sub

    Private Sub dgv_countdetails_CellLeave(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_countdetails.CellLeave
        With dgv_countdetails
            If .CurrentCell.ColumnIndex = 2 Then
                .CurrentRow.Cells(2).Value = Format(Val(.CurrentRow.Cells(2).Value), "#########0.000")
            End If
            If .CurrentCell.ColumnIndex = 5 Or .CurrentCell.ColumnIndex = 6 Then
                .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = Format(Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value), "#########0.00")
            End If
        End With
    End Sub

    Private Sub dgv_countdetails_CellValueChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_countdetails.CellValueChanged
        On Error Resume Next

        If IsNothing(dgv_countdetails.CurrentCell) Then Exit Sub
        With dgv_countdetails
            If e.ColumnIndex = 2 Or e.ColumnIndex = 3 Then
                .CurrentRow.Cells(4).Value = 0
                If Val(.CurrentRow.Cells(3).Value) <> 0 Then
                    .CurrentRow.Cells(4).Value = Format(Val(.CurrentRow.Cells(2).Value) / Val(.CurrentRow.Cells(3).Value), "#########0.000")
                End If
            End If
        End With
    End Sub

    Private Sub dgv_countdetails_EditingControlShowing(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewEditingControlShowingEventArgs) Handles dgv_countdetails.EditingControlShowing
        dgtxt_Details = CType(dgv_countdetails.EditingControl, DataGridViewTextBoxEditingControl)
    End Sub

    Private Sub dgv_countdetails_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_countdetails.KeyDown
        On Error Resume Next

        With dgv_countdetails
            If e.KeyCode = Keys.Up Then
                If .CurrentRow.Index = 0 Then
                    txt_weightemptycone.Focus()
                End If
            End If

            If e.KeyCode = Keys.Left Then
                If .CurrentCell.RowIndex = 0 And .CurrentCell.ColumnIndex <= 1 Then
                    txt_weightemptycone.Focus()
                End If
            End If

            If e.KeyCode = Keys.Enter Then

                If .CurrentRow.Index = .RowCount - 1 And .CurrentCell.ColumnIndex >= 1 And Trim(.CurrentRow.Cells(1).Value) = "" Then
                    If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                        save_record()
                    End If

                Else
                    e.SuppressKeyPress = True
                    e.Handled = True
                    SendKeys.Send("{Tab}")

                End If

            End If

        End With

    End Sub

    Private Sub dgv_countdetails_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dgv_countdetails.KeyPress
        If dgv_countdetails.CurrentCell.ColumnIndex = 2 Or dgv_countdetails.CurrentCell.ColumnIndex = 3 Or dgv_countdetails.CurrentCell.ColumnIndex = 5 Or dgv_countdetails.CurrentCell.ColumnIndex = 6 Then
            If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
        End If
    End Sub

    Private Sub dgv_countdetails_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_countdetails.KeyUp
        Dim i As Integer

        If e.Control = True And UCase(Chr(e.KeyCode)) = "D" Then

            With dgv_countdetails

                .Rows.RemoveAt(.CurrentRow.Index)

                For i = 0 To .Rows.Count - 1
                    .Rows(i).Cells(0).Value = i + 1
                Next

            End With
        End If
    End Sub

    Private Sub dgv_countdetails_RowsAdded(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewRowsAddedEventArgs) Handles dgv_countdetails.RowsAdded

        If IsNothing(dgv_countdetails.CurrentCell) Then Exit Sub
        With dgv_countdetails
            Dim n As Integer
            n = .RowCount
            .Rows(n - 1).Cells(0).Value = Val(n)
        End With
    End Sub

    Private Sub cbo_count_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_count.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Count_Head", "Count_Name", "", "(Count_IdNo = 0)")

    End Sub

    Private Sub cbo_count_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_count.KeyDown

        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_count, Nothing, Nothing, "Count_Head", "Count_Name", "", "(Count_IdNo = 0)")

        Try
            With cbo_count
                If e.KeyValue = 38 And .DroppedDown = False Then
                    e.Handled = True

                    If Val(dgv_countdetails.CurrentCell.RowIndex) <= 0 Then
                        txt_weightemptycone.Focus()

                    Else
                        dgv_countdetails.CurrentCell = dgv_countdetails.Rows(dgv_countdetails.CurrentCell.RowIndex - 1).Cells(2)
                        dgv_countdetails.CurrentCell.Selected = True
                        dgv_countdetails.Focus()
                        cbo_count.Visible = False

                    End If

                ElseIf e.KeyValue = 40 And .DroppedDown = False Then
                    e.Handled = True

                    dgv_countdetails.CurrentCell = dgv_countdetails.Rows(dgv_countdetails.CurrentCell.RowIndex).Cells(dgv_countdetails.CurrentCell.ColumnIndex + 1)
                    dgv_countdetails.CurrentCell.Selected = True
                    dgv_countdetails.Focus()
                    cbo_count.Visible = False

                ElseIf e.KeyValue <> 13 And e.KeyValue <> 17 And e.KeyValue <> 27 And .DroppedDown = False Then
                    .DroppedDown = True

                End If
            End With

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT SELECT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub


    Private Sub cbo_count_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_count.KeyPress

        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_count, Nothing, "Count_Head", "Count_Name", "", "(Count_IdNo = 0)")

        If Asc(e.KeyChar) = 13 Then

            If dgv_countdetails.CurrentRow.Index = dgv_countdetails.RowCount - 1 And dgv_countdetails.CurrentCell.ColumnIndex >= 1 And Trim(dgv_countdetails.CurrentRow.Cells(1).Value) = "" Then
                If txt_TamilName.Visible = True Then
                    txt_TamilName.Focus()
                ElseIf cbo_Sizing_MillName.Visible = True Then
                    cbo_Sizing_MillName.Focus()
                Else
                    If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                        save_record()
                    Else

                        txt_MillName.Focus()
                    End If

                End If
                
            Else
                dgv_countdetails.CurrentCell = dgv_countdetails.Rows(dgv_countdetails.CurrentCell.RowIndex).Cells(dgv_countdetails.CurrentCell.ColumnIndex + 1)
                dgv_countdetails.CurrentCell.Selected = True
                dgv_countdetails.Focus()
                cbo_count.Visible = False
            End If
        End If

    End Sub

    Private Sub cbo_count_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_count.KeyUp
        If e.Control = False And e.KeyValue = 17 Then
            Dim f As New Count_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_count.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If
    End Sub

    Private Sub cbo_count_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_count.TextChanged
        Try
            If cbo_count.Visible Then

                If IsNothing(dgv_countdetails.CurrentCell) Then Exit Sub
                With dgv_countdetails
                    If Val(cbo_count.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = 1 Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_count.Text)
                    End If
                End With
            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub btn_CloseOpen_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_CloseOpen.Click
        pnl1_back.Enabled = True
        grp_Open.Visible = False
    End Sub

    Private Sub btn_Find_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Find.Click
        Dim movid As Integer

        If Trim(cbo_Open.Text) = "" Then
            MessageBox.Show("Invalid Mill Name", "DOES NOT FIND...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If cbo_Open.Enabled Then cbo_Open.Focus()
            Exit Sub
        End If

        movid = Common_Procedures.Mill_NameToIdNo(con, cbo_Open.Text)
        If movid <> 0 Then move_record(movid)

        pnl1_back.Enabled = True
        grp_Open.Visible = False

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
            MessageBox.Show(ex.Message, "DOES NOT SELECT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub cbo_Open_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Open.KeyPress
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim Condt As String
        Dim FindStr As String

        Try

            With cbo_Open

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

                        Call btn_Find_Click(sender, e)

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
                            Condt = " Where Mill_Name like '" & Trim(FindStr) & "%' or Mill_Name like '% " & Trim(FindStr) & "%' "
                        End If

                        da = New SqlClient.SqlDataAdapter("select Mill_Name from Mill_Head " & Condt & " order by Mill_Name", con)
                        da.Fill(dt)

                        .DataSource = dt
                        .DisplayMember = "Mill_Name"

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

    Protected Overrides Function ProcessCmdKey(ByRef msg As System.Windows.Forms.Message, ByVal keyData As System.Windows.Forms.Keys) As Boolean
        Dim dgv1 As New DataGridView

        On Error Resume Next

        If ActiveControl.Name = dgv_countdetails.Name Or TypeOf ActiveControl Is DataGridViewTextBoxEditingControl Then

            dgv1 = Nothing

            If ActiveControl.Name = dgv_countdetails.Name Then
                dgv1 = dgv_countdetails

            ElseIf dgv_countdetails.IsCurrentRowDirty = True Then
                dgv1 = dgv_countdetails

            End If

            If IsNothing(dgv1) = False Then

                With dgv1


                    If keyData = Keys.Enter Or keyData = Keys.Down Then
                        If .CurrentCell.ColumnIndex >= .ColumnCount - 1 Then
                            If .CurrentCell.RowIndex = .RowCount - 1 Then
                                btn_save.Focus()

                            Else
                                .CurrentCell = .Rows(.CurrentCell.RowIndex + 1).Cells(1)

                            End If

                        Else
                            .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 1)

                        End If

                        Return True

                    ElseIf keyData = Keys.Up Then

                        If .CurrentCell.ColumnIndex <= 1 Then
                            If .CurrentCell.RowIndex = 0 Then
                                txt_weightemptycone.Focus()

                            Else
                                .CurrentCell = .Rows(.CurrentCell.RowIndex - 1).Cells(.ColumnCount - 1)

                            End If

                        Else
                            .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex - 1)

                        End If

                        Return True

                    Else
                        Return MyBase.ProcessCmdKey(msg, keyData)

                    End If

                End With

            Else

                Return MyBase.ProcessCmdKey(msg, keyData)

            End If

        Else

            Return MyBase.ProcessCmdKey(msg, keyData)

        End If



    End Function
    Private Sub dgtxt_Details_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgtxt_Details.Enter
        dgv_countdetails.EditingControl.BackColor = Color.Lime
        dgv_countdetails.EditingControl.ForeColor = Color.Blue
        dgtxt_Details.SelectAll()
    End Sub

    Private Sub txt_weightemptycone_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_weightemptycone.KeyDown
        If e.KeyValue = 38 Then SendKeys.Send("+{TAB}")
        If (e.KeyValue = 40) Then
            If dgv_countdetails.Rows.Count > 0 Then
                dgv_countdetails.Focus()
                dgv_countdetails.CurrentCell = dgv_countdetails.Rows(0).Cells(1)

            Else
                btn_save.Focus()

            End If
        End If
    End Sub

    Private Sub txt_weightemptycone_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_weightemptycone.KeyPress

        If Asc(e.KeyChar) = 13 Then
            If dgv_countdetails.Rows.Count > 0 Then
                dgv_countdetails.Focus()
                dgv_countdetails.CurrentCell = dgv_countdetails.Rows(0).Cells(1)

            Else
                btn_save.Focus()

            End If
        End If
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
            e.Handled = True
        End If
    End Sub

    Private Sub cbo_Sizing_MillName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Sizing_MillName.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, SizTo_DbName & "..Mill_Head", "Mill_Name", "", "(Mill_idno = 0)")
    End Sub

    Private Sub cbo_Sizing_MillName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Sizing_MillName.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Sizing_MillName, txt_TamilName, Nothing, SizTo_DbName & "..Mill_Head", "Mill_Name", "", "(Mill_idno = 0)")
        
        If (e.KeyValue = 40 And cbo_Sizing_MillName.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
            If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                save_record()
            Else
                txt_MillName.Focus()
            End If
        End If
    End Sub

    Private Sub cbo_Sizing_MillName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Sizing_MillName.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Sizing_MillName, Nothing, SizTo_DbName & "..Mill_Head", "Mill_Name", "", "(Mill_idno = 0)")
        If Asc(e.KeyChar) = 13 Then
            If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                save_record()
            Else
                txt_MillName.Focus()
            End If
        End If
    End Sub

    Private Sub txt_TamilName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_TamilName.KeyDown
        If (e.KeyValue = 38) Then
            If dgv_countdetails.Rows.Count > 0 Then
                dgv_countdetails.Focus()
                dgv_countdetails.CurrentCell = dgv_countdetails.Rows(0).Cells(1)
            Else
                txt_weightemptycone.Focus()
            End If
        End If
        If (e.KeyValue = 40) Then
            If cbo_Sizing_MillName.Visible = True Then
                cbo_Sizing_MillName.Focus()
            Else
                If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                    save_record()
                Else
                    txt_MillName.Focus()
                End If
            End If
        End If

    End Sub

    Private Sub txt_TamilName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_TamilName.KeyPress
        If Asc(e.KeyChar) = 13 Then
            If cbo_Sizing_MillName.Visible = True Then
                cbo_Sizing_MillName.Focus()
            Else
                If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                    save_record()
                Else
                    txt_MillName.Focus()
                End If
            End If
        End If
    End Sub


    Private Sub txt_MillName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_MillName.KeyPress
        If Asc(e.KeyChar) = 34 Or Asc(e.KeyChar) = 39 Then   '-- Single Quotes and double quotes blocked
            e.Handled = True
        End If
    End Sub

    Private Sub dgv_countdetails_LostFocus(sender As Object, e As EventArgs) Handles dgv_countdetails.LostFocus
        On Error Resume Next
        If IsNothing(dgv_countdetails.CurrentCell) Then Exit Sub
        dgv_countdetails.CurrentCell.Selected = False
    End Sub

    Private Sub txt_weightemptybag_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txt_weightemptybag.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
            e.Handled = True
        End If
    End Sub

    Private Sub dgtxt_Details_KeyPress(sender As Object, e As KeyPressEventArgs) Handles dgtxt_Details.KeyPress
        If dgv_countdetails.CurrentCell.ColumnIndex = 2 Or dgv_countdetails.CurrentCell.ColumnIndex = 3 Or dgv_countdetails.CurrentCell.ColumnIndex = 4 Or dgv_countdetails.CurrentCell.ColumnIndex = 5 Or dgv_countdetails.CurrentCell.ColumnIndex = 6 Then
            If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
                e.Handled = True
            End If
        End If
    End Sub

End Class

