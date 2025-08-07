Public Class OE_Count_Details

    Implements Interface_MDIActions

    Dim con As New SqlClient.SqlConnection(Common_Procedures.Connection_String)
    Dim New_Entry As Boolean
    Private Prec_ActCtrl As New Control
    Private vcbo_KeyDwnVal As Double
    Private FrmLdSTS As Boolean = False
    Private Insert_Entry As Boolean = False
    Private Filter_Status As Boolean = False

    Private Dep_Id As Integer
    Private Sub clear()

        pnl_Back.Enabled = True
        grp_find.Visible = False
        grp_Filter.Visible = False

        lbl_IdNo.Text = ""
        lbl_IdNo.ForeColor = Color.Black
        cbo_Count.Text = ""
        txt_Description.Text = ""
        txt_Count_Hank.Text = ""

        cbo_Find.Text = ""
        New_Entry = False
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

    Private Sub TextBoxControlKeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs)
        On Error Resume Next
        If e.KeyValue = 32 Then SendKeys.Send("+{TAB}")
        If e.KeyValue = 40 Then SendKeys.Send("{TAB}")
    End Sub

    Private Sub TextBoxControlKeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
        On Error Resume Next
        If Asc(e.KeyChar) = 13 Then SendKeys.Send("{TAB}")
    End Sub
    Public Sub delete_record() Implements Interface_MDIActions.delete_record
        Dim cmd As New SqlClient.SqlCommand


        ' If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.Count_Creation, "~L~") = 0 And InStr(Common_Procedures.UR.Count_Creation, "~D~") = 0 Then MessageBox.Show("You have No Rights to Delete", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error) : Exit Sub

        If MessageBox.Show("Do you want to Delete ?", "FOR DELETION....", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.No Then
            Exit Sub
        End If

        If New_Entry = True Then
            MessageBox.Show("This is new entry", "DOES NOT DELETION....", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        Try



            cmd.Connection = con
            cmd.CommandText = "delete from OE_Count_Details where Count_Details_IdNo = " & Str(Val(lbl_IdNo.Text))

            cmd.ExecuteNonQuery()

            cmd.Dispose()

            new_record()

            MessageBox.Show("Deleted Sucessfully!!!", "FOR DELETION...", MessageBoxButtons.OK, MessageBoxIcon.Information)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally
            If cbo_Count.Enabled And cbo_Count.Visible Then cbo_Count.Focus()

        End Try
    End Sub

    Public Sub filter_record() Implements Interface_MDIActions.filter_record
        Dim da As New SqlClient.SqlDataAdapter("select a.Count_Details_IdNo,b.Count_Name ,a.cOUNT_Hank,a.Description from OE_Count_details a INNER JOIN Count_Head b ON b.Count_IdNo = a.Count_Idno  where a.Department_IdNo = " & Val(Dep_Id) & " and a.Count_Details_IdNo <> 0 order by a.Count_Details_IdNo", con)
        Dim dt As New DataTable

        da.Fill(dt)

        With dgv_Filter

            .Columns.Clear()
            .DataSource = dt

            .RowHeadersVisible = False

            .AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells
            .AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill

            .Columns(0).HeaderText = "IDNO"
            .Columns(1).HeaderText = "COUNT NAME"
            .Columns(2).HeaderText = "COUNT HANK"
            .Columns(3).HeaderText = "DESCRIPTION"


            .Columns(0).FillWeight = 50
            .Columns(1).FillWeight = 90
            .Columns(2).FillWeight = 50
            .Columns(3).FillWeight = 80

        End With

        new_record()


        pnl_Back.Enabled = False
        grp_Filter.Visible = True

        If dgv_Filter.Enabled And dgv_Filter.Visible Then dgv_Filter.Focus()



        da.Dispose()

    End Sub

    Public Sub insert_record() Implements Interface_MDIActions.insert_record

    End Sub

    Public Sub move_record(ByVal no As Integer)
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim dt2 As New DataTable
        Dim da2 As New SqlClient.SqlDataAdapter


        If Val(no) = 0 Then Exit Sub

        clear()
        Try

            da = New SqlClient.SqlDataAdapter("select a.* from OE_Count_Details a  where a.Count_Details_IdNo = " & Val(no) & " and a.Department_IdNo = " & Str(Val(Dep_Id)) & " ", con)

            da.Fill(dt)

            If dt.Rows.Count > 0 Then
                lbl_IdNo.Text = dt.Rows(0).Item("Count_Details_IdNo").ToString
                cbo_Count.Text = Common_Procedures.Count_IdNoToName(con, dt.Rows(0).Item("Count_IdNo").ToString)
                txt_Description.Text = (dt.Rows(0).Item("Description").ToString)
                txt_Count_Hank.Text = Val(dt.Rows(0).Item("count_Hank").ToString)
            End If




        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally
            da.Dispose()
            dt.Dispose()


            If cbo_Count.Enabled And cbo_Count.Visible Then cbo_Count.Focus()
        End Try







    End Sub

    Public Sub movefirst_record() Implements Interface_MDIActions.movefirst_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim movid As Integer = 0

        Try
            da = New SqlClient.SqlDataAdapter("select min(Count_Details_IdNo) from OE_Count_Details Where Department_IdNo = " & Str(Val(Dep_Id)) & " AND Count_Details_IdNo <> 0", con)
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
            da = New SqlClient.SqlDataAdapter("select max(Count_Details_IdNo) from OE_Count_Details Where Department_IdNo = " & Str(Val(Dep_Id)) & " AND Count_Details_IdNo <> 0", con)
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
            da = New SqlClient.SqlDataAdapter("select min(Count_Details_IdNo) from OE_Count_Details Where Department_IdNo = " & Str(Val(Dep_Id)) & " AND Count_Details_IdNo > " & Str(Val(lbl_IdNo.Text)), con)
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
            da = New SqlClient.SqlDataAdapter("select max(Count_Details_IdNo) from OE_Count_Details Where Department_IdNo = " & Str(Val(Dep_Id)) & " AND Count_Details_IdNo < " & Str(Val(lbl_IdNo.Text)) & " and Count_Details_IdNo <> 0", con)
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
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim NewID As Integer = 0

        Try
            clear()
            New_Entry = True
            lbl_IdNo.ForeColor = Color.Red
            lbl_IdNo.Text = Common_Procedures.get_MaxIdNo(con, "OE_Count_Details", "Count_Details_IdNo", "")


            If cbo_Count.Enabled And cbo_Count.Visible Then cbo_Count.Focus()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR NEW RECORD...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub

    Public Sub open_record() Implements Interface_MDIActions.open_record
        Dim da As New SqlClient.SqlDataAdapter("select b.Count_Name from OE_Count_details a INNER JOIN Count_Head B ON B.Count_IdNo = a.Count_Idno WHERE a.Department_IdNo = " & Str(Val(Dep_Id)) & " order by A.Count_details_IdNo", con)
        Dim dt As New DataTable

        da.Fill(dt)

        cbo_Find.DataSource = dt
        cbo_Find.DisplayMember = "Count_name"

        new_record()

        grp_find.Visible = True
        pnl_Back.Enabled = False
    End Sub



    Public Sub print_record() Implements Interface_MDIActions.print_record
        '-------
    End Sub

    Public Sub save_record() Implements Interface_MDIActions.save_record
        Dim trans As SqlClient.SqlTransaction
        Dim cmd As New SqlClient.SqlCommand
        Dim Cnt_id As Integer
        Dim NewCode As String = ""
        Dim vOrdByNo As Single = 0
        'If Common_Procedures.UserRight_Check(Common_Procedures.UR.Count_Creation, New_Entry) = False Then Exit Sub

        If pnl_Back.Enabled = False Then
            MessageBox.Show("Close Other Windows", "DOES NOT SAVE", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If


        Cnt_id = Common_Procedures.Count_NameToIdNo(con, cbo_Count.Text)
        If Cnt_id = 0 Then
            MessageBox.Show("Invalid Count Name", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If cbo_Count.Enabled And cbo_Count.Visible Then cbo_Count.Focus()
            Exit Sub
        End If


        trans = con.BeginTransaction
        Try


            cmd.Connection = con
            cmd.Transaction = trans

            If New_Entry = True Then

                lbl_IdNo.Text = Common_Procedures.get_MaxIdNo(con, "OE_Count_Details", "Count_Details_IdNo", "", trans)

                cmd.CommandText = "Insert into OE_Count_Details(  Count_Details_IdNo  ,     Count_IdNo      ,            Description              ,           Count_Hank            ,       Department_IdNo) " &
                                        "           values ( " & Val(lbl_IdNo.Text) & ", " & Val(Cnt_id) & ", '" & Trim(txt_Description.Text) & "'," & Str(Val(txt_Count_Hank.Text)) & ",       " & Val(Dep_Id) & " )"
                cmd.ExecuteNonQuery()

            Else
                cmd.CommandText = "update OE_Count_Details Set Count_IdNo = " & Val(Cnt_id) & ",Description = '" & Trim(txt_Description.Text) & "', Count_Hank=" & Str(Val(txt_Count_Hank.Text)) & " ,Department_IdNo = " & Val(Dep_Id) & "   where Count_Details_IdNo =  " & Val(lbl_IdNo.Text) & ""
                cmd.ExecuteNonQuery()

            End If

            trans.Commit()
            move_record(lbl_IdNo.Text)

            MessageBox.Show("Saved Sucessfully!!!", "FOR SAVING...", MessageBoxButtons.OK, MessageBoxIcon.Information)

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
            MessageBox.Show(ex.Message, "FOR SAVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally

            cmd.Dispose()
            trans.Dispose()
            If cbo_Count.Enabled And cbo_Count.Visible Then cbo_Count.Focus()


        End Try

    End Sub

    Private Sub Count_Details_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated
        Try

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Count.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "COUNT" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Count.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If



            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""



        Catch ex As Exception
            'MessageBox.Show(ex.Message, "DOES NOT SHOW...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        FrmLdSTS = False
    End Sub


    Private Sub Count_Details_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        con.Close()
        con.Dispose()
    End Sub

    Private Sub Count_Details_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If Asc(e.KeyChar) = 27 Then
            If grp_Filter.Visible Then
                btn_FilterClose_Click(sender, e)
            ElseIf grp_find.Visible Then
                btn_FindClose_Click(sender, e)


            Else

                If MessageBox.Show("Do you want to Close?", "FOR CLOSING ENTRY...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) <> Windows.Forms.DialogResult.Yes Then
                    Exit Sub

                Else
                    Me.Close()



                End If


            End If

        End If

        ' End If
    End Sub
    Private Sub Count_Details_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Me.Load

        con.Open()

        Dep_Id = Val(Common_Procedures.OE_Department_IdNo)

        If Val(Dep_Id) = 1 Then
            Me.BackColor = Color.LightSkyBlue
            lbl_Heading.Text = "CARDING"
            Me.Text = "(COUNT DETAILS)CARDING"
        End If

        If Val(Dep_Id) = 2 Then
            Me.BackColor = Color.LightSkyBlue
            lbl_Heading.Text = "DRAWING"
            Me.Text = "(COUNT DETAILS)DRAWING"
        End If

        If Val(Dep_Id) = 3 Then
            Me.BackColor = Color.LightSkyBlue
            lbl_Heading.Text = "VORTEX"
            Me.Text = "(COUNT DETAILS)VORTEX"
        End If

        grp_Filter.Visible = False
        grp_Filter.Left = (Me.Width - grp_Filter.Width) \ 2
        grp_Filter.Top = ((Me.Height - grp_Filter.Height) \ 2) + 20

        grp_find.Visible = False
        grp_find.Left = (Me.Width - grp_find.Width) \ 2
        grp_find.Top = ((Me.Height - grp_find.Height) \ 2) + 20


        AddHandler cbo_Count.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Find.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Description.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Count_Hank.GotFocus, AddressOf ControlGotFocus

        AddHandler btn_save.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_close.GotFocus, AddressOf ControlGotFocus

        AddHandler cbo_Count.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Find.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Description.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Count_Hank.LostFocus, AddressOf ControlLostFocus

        AddHandler btn_save.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_close.GotFocus, AddressOf ControlGotFocus




        Me.Top = Me.Top - 85
        FrmLdSTS = True
        new_record()

    End Sub


    Private Sub btn_Save_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_save.Click
        save_record()
    End Sub

    Private Sub btn_FilterClose_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_FilterClose.Click

        pnl_Back.Enabled = True
        grp_Filter.Visible = False
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

    Private Sub btn_Close_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_close.Click
        Me.Close()
    End Sub

    Private Sub btn_FindOpen_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_FindOpen.Click
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim movid As Integer

        If Trim(cbo_Find.Text) = "" Then
            MessageBox.Show("Invalid Count Name", "DOES NOT FIND...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If cbo_Find.Visible And cbo_Find.Enabled Then cbo_Find.Focus()
            Exit Sub
        End If

        da = New SqlClient.SqlDataAdapter("select a.Count_Details_IdNo,b.Count_Name from OE_Count_Details a INNER JOIN Count_Head b ON b.Count_IdNo = a.Count_Idno  where b.Count_Name = '" & Trim(cbo_Find.Text) & "'", con)
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

        pnl_Back.Enabled = True
        grp_find.Visible = False

    End Sub
    Private Sub cbo_Find_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Find.GotFocus

        'Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "OE_Count_Details", "count_idno", "Department_IdNo = " & Val(Dep_Id) & "", "(Count_Details_IdNo = 0)")
        '   Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Count_Head", "Count_Name", "", "(Count_IdNo = 0)")
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim SqlCondt As String, Condt2 As String
        Dim FindStr As String
        Dim indx As Integer
        Dim SelStrt As Integer

        da = New SqlClient.SqlDataAdapter("SELECT B.COUNT_NAME FROM OE_COUNT_DETAILS A LEFT OUTER JOIN COUNT_HEAD B ON A.COUNT_iDNO = B.COUNT_iDNO WHERE A.COUNT_iDNO <>0 AND A.Department_IdNo = " & Val(Dep_Id) & " ", con)
        dt = New DataTable
        da.Fill(dt)

        cbo_Find.DataSource = dt
            cbo_Find.DisplayMember = "COUNT_NAME"
        cbo_Find.SelectedIndex = -1



    End Sub

    Private Sub cbo_Find_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Find.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Find, Nothing, Nothing, "Count_Head", "Count_Name", "", "(Count_Idno=0)")
    End Sub
    Private Sub cbo_Find_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Find.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Find, Nothing, "Count_Head", "Count_Name", "", "(Count_Idno=0)")
        If Asc(e.KeyChar) = 13 Then
            btn_FindOpen_Click(sender, e)
        End If
    End Sub
    Private Sub cbo_Count_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Count.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Count_Head", "Count_Name", "", "(Count_Idno=0)")
    End Sub
    Private Sub cbo_Count_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Count.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Count, Nothing, txt_Count_Hank, "Count_Head", "Count_Name", "", "(Count_Idno=0)")
    End Sub
    Private Sub cbo_Count_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Count.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Count, txt_Count_Hank, "Count_Head", "Count_Name", "", "(Count_Idno=0)")
    End Sub
    Private Sub cbo_Count_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Count.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Dim f As New Count_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Count.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()
        End If
    End Sub
    Private Sub txt_CountHank_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Count_Hank.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
        If Asc(e.KeyChar) = 13 Then
            txt_Description.Focus()
        End If
    End Sub
    Private Sub txt_Description_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txt_Description.KeyPress
        If Asc(e.KeyChar) = 13 Then
            If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                save_record()
            Else
                cbo_Count.Focus()
            End If
        End If
    End Sub

    Private Sub txt_Description_KeyDown(sender As Object, e As KeyEventArgs) Handles txt_Description.KeyDown
        If e.KeyCode = 38 Then
            txt_Count_Hank.Focus()
        Else

            If e.KeyCode = 40 Then
                If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                    save_record()
                Else
                    cbo_Count.Focus()
                End If
            End If
        End If
    End Sub

    Private Sub txt_CountHank_KeyDown(sender As Object, e As KeyEventArgs) Handles txt_Count_Hank.KeyDown
        If e.KeyCode = 38 Then
            cbo_Count.Focus()
        Else
            If e.KeyCode = 40 Then
                txt_Description.Focus()

            End If
        End If
    End Sub
End Class