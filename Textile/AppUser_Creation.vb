Public Class AppUser_Creation
    Implements Interface_MDIActions

    Private con As New SqlClient.SqlConnection(Common_Procedures.Connection_String)
    Private New_Entry As Boolean = False
    Private FrmLdSTS As Boolean = False
    Private vcbo_KeyDwnVal As Double
    Private WithEvents dgtxt_CheckingTableno_Details As New DataGridViewTextBoxEditingControl
    Private dgv_ActiveCtrl_Name As String
    Private Prec_ActCtrl As New Control

    Public Sub New()
        FrmLdSTS = True
        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
    End Sub

    Private Sub clear()

        pnl_Back.Enabled = True
        grp_Find.Visible = False
        grp_Filter.Visible = False

        'Me.Height = 270

        lbl_IdNo.Text = ""
        lbl_IdNo.ForeColor = Color.Black
        cbo_Checking_tableno.Tag = -1
        cbo_Checking_tableno.Text = ""
        txt_User_Name.Text = ""
        txt_user_password.Text = ""
        cbo_Find.Text = ""

        cbo_Checking_tableno.Visible = False
        cbo_Checking_tableno.Tag = -1
        cbo_Checking_tableno.Text = ""
        dgv_ActiveCtrl_Name = ""

        dgv_CheckingTableno_Details.Rows.Clear()
        New_Entry = False

    End Sub

    Private Sub ControlGotFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim txtbx As TextBox
        Dim combobx As ComboBox
        If FrmLdSTS = True Then Exit Sub
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

        Grid_DeSelect()


        Prec_ActCtrl = Me.ActiveControl

    End Sub

    Private Sub ControlLostFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        If FrmLdSTS = True Then Exit Sub

        On Error Resume Next

        If IsNothing(Prec_ActCtrl) = False Then
            If TypeOf Prec_ActCtrl Is TextBox Or TypeOf Prec_ActCtrl Is ComboBox Then
                Prec_ActCtrl.BackColor = Color.White
                Prec_ActCtrl.ForeColor = Color.Black
            End If
        End If
        Grid_DeSelect()
    End Sub
    Private Sub ControlLostFocus1(ByVal sender As Object, ByVal e As System.EventArgs)
        If FrmLdSTS = True Then Exit Sub

        On Error Resume Next

        If IsNothing(Prec_ActCtrl) = False Then
            If TypeOf Prec_ActCtrl Is TextBox Or TypeOf Prec_ActCtrl Is ComboBox Then
                Prec_ActCtrl.BackColor = Color.LightBlue
                Prec_ActCtrl.ForeColor = Color.Blue
            End If
        End If
        Grid_DeSelect()
    End Sub

    Public Sub move_record(ByVal idno As Integer)

        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim dt2 As New DataTable
        Dim slno, n As New Integer

        If Val(idno) = 0 Then Exit Sub

        clear()
        Try
            da = New SqlClient.SqlDataAdapter("select a.* from appuser_head a  where a.user_idno = " & Str(Val(idno)), con)
            dt = New DataTable
            da.Fill(dt)

            If dt.Rows.Count > 0 Then
                lbl_IdNo.Text = dt.Rows(0)("user_idno").ToString
                txt_User_Name.Text = dt.Rows(0)("user_name").ToString
                txt_user_password.Text = dt.Rows(0)("user_password").ToString

                da = New SqlClient.SqlDataAdapter(" select b.Checking_Table_No from AppUser_Checking_Tableno_Details a INNER JOIN Checking_TableNo_Head b ON a.Checking_Table_IdNo = b.Checking_Table_IdNo where a.user_idno = " & Str(Val(idno)) & " Order by a.Sl_No", con)
                da.Fill(dt2)
                dgv_CheckingTableno_Details.Rows.Clear()

                slno = 0
                If dt2.Rows.Count > 0 Then

                    For i = 0 To dt2.Rows.Count - 1

                        n = dgv_CheckingTableno_Details.Rows.Add()

                        slno = slno + 1
                        dgv_CheckingTableno_Details.Rows(n).Cells(0).Value = Val(slno)
                        dgv_CheckingTableno_Details.Rows(n).Cells(1).Value = dt2.Rows(i).Item("Checking_Table_No").ToString


                    Next i

                    For i = 0 To dgv_CheckingTableno_Details.RowCount - 1
                        dgv_CheckingTableno_Details.Rows(i).Cells(0).Value = Val(i) + 1
                    Next

                End If

                dt2.Clear()
                dt2.Dispose()

            Else
                new_record()

            End If
            dt.Clear()

            dt.Dispose()
            da.Dispose()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
        cbo_Checking_tableno.Tag = -1
        cbo_Checking_tableno.Text = ""
        cbo_Checking_tableno.Visible = False
        Grid_DeSelect()

    End Sub

    Private Sub AppUser_Creation_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
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
    Private Sub Grid_DeSelect()
        If FrmLdSTS = True Then Exit Sub
        On Error Resume Next
        If Not IsNothing(dgv_CheckingTableno_Details.CurrentCell) Then dgv_CheckingTableno_Details.CurrentCell.Selected = False
    End Sub
    Private Sub AppUser_Creation_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Me.Height = 270 ' 197


        grp_Find.Left = 6
        grp_Find.Top = 250
        grp_Find.Visible = False

        grp_Filter.Left = 6
        grp_Filter.Top = 250
        grp_Filter.Visible = False
        con.Open()

        grp_Find.Visible = False
        grp_Find.Left = (Me.Width - grp_Find.Width) \ 2
        grp_Find.Top = (Me.Height - grp_Find.Height) \ 2




        AddHandler txt_User_Name.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_user_password.GotFocus, AddressOf ControlGotFocus

        AddHandler cbo_Checking_tableno.GotFocus, AddressOf ControlGotFocus


        AddHandler txt_User_Name.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_user_password.LostFocus, AddressOf ControlLostFocus

        AddHandler cbo_Checking_tableno.LostFocus, AddressOf ControlLostFocus


        dgv_CheckingTableno_Details.Visible = True
        new_record()
    End Sub

    Private Sub AppUser_Creation_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        On Error Resume Next
        con.Close()
        con.Dispose()
        Common_Procedures.Last_Closed_FormName = Me.Name
    End Sub

    Private Sub btn_Close_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Close.Click
        Me.Close()
        Me.Dispose()
    End Sub

    Public Sub delete_record() Implements Interface_MDIActions.delete_record
        Dim cmd As New SqlClient.SqlCommand
        Dim da As SqlClient.SqlDataAdapter
        Dim dt As DataTable

        ' If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.ItemGroup_Creation, "~L~") = 0 And InStr(Common_Procedures.UR.ItemGroup_Creation, "~D~") = 0 Then MessageBox.Show("You have No Rights to Delete", "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error) : Exit Sub

        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.DeletingEntry, Common_Procedures.UR.ItemGroup_Creation, New_Entry, Me) = False Then Exit Sub


        If MessageBox.Show("Do you want to Delete ?", "FOR DELETION....", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.No Then
            Exit Sub
        End If


        If New_Entry = True Then
            MessageBox.Show("This is new entry", "DOES NOT DELETION....", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        Try

            da = New SqlClient.SqlDataAdapter("select count(*) from Weaver_ClothReceipt_App_PieceReceipt_Details where user_idno = " & Str(Val(lbl_IdNo.Text)), con)
            dt = New DataTable
            Dim unused = da.Fill(dt)
            If dt.Rows.Count > 0 Then
                If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                    If Val(dt.Rows(0)(0).ToString) > 0 Then
                        MessageBox.Show("Already used this Username", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        Exit Sub
                    End If
                End If
            End If

            da = New SqlClient.SqlDataAdapter("select count(*) from Weaver_ClothReceipt_Piece_Details where user_idno = " & Str(Val(lbl_IdNo.Text)), con)
            dt = New DataTable
            da.Fill(dt)
            If dt.Rows.Count > 0 Then
                If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                    If Val(dt.Rows(0)(0).ToString) > 0 Then
                        MessageBox.Show("Already used this Username", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        Exit Sub
                    End If
                End If
            End If


            da = New SqlClient.SqlDataAdapter("select count(*) from Weaver_ClothReceipt_App_Piece_Defect_Details where user_idno = " & Str(Val(lbl_IdNo.Text)), con)
            dt = New DataTable
            da.Fill(dt)
            If dt.Rows.Count > 0 Then
                If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                    If Val(dt.Rows(0)(0).ToString) > 0 Then
                        MessageBox.Show("Already used this Username", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        Exit Sub
                    End If
                End If
            End If

            cmd.Connection = con
            cmd.CommandText = "delete from appuser_head where user_idno = " & Str(Val(lbl_IdNo.Text))

            cmd.ExecuteNonQuery()

            dt.Dispose()
            da.Dispose()
            cmd.Dispose()

            new_record()

            MessageBox.Show("Deleted Sucessfully!!!", "FOR DELETION...", MessageBoxButtons.OK, MessageBoxIcon.Information)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally
            If txt_User_Name.Enabled And txt_User_Name.Visible Then txt_User_Name.Focus()

        End Try

    End Sub

    Public Sub filter_record() Implements Interface_MDIActions.filter_record
        Dim da As New SqlClient.SqlDataAdapter("select user_idno, user_name from appuser_head where user_idno <> 0 order by user_idno", con)
        Dim dt As New DataTable

        da.Fill(dt)

        With dgv_Filter

            .Columns.Clear()
            .DataSource = dt

            .RowHeadersVisible = False

            .AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells
            .AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill

            .Columns(0).HeaderText = "IDNO"
            .Columns(1).HeaderText = "USER NAME"

            .Columns(0).FillWeight = 40
            .Columns(1).FillWeight = 160

        End With

        new_record()

        grp_Filter.Visible = True
        grp_Filter.Left = grp_Find.Left
        grp_Filter.Top = grp_Find.Top

        pnl_Back.Enabled = False

        If dgv_Filter.Enabled And dgv_Filter.Visible Then dgv_Filter.Focus()

        'Me.Height = 580 ' 400

        dt.Dispose()
        da.Dispose()

    End Sub

    Public Sub movefirst_record() Implements Interface_MDIActions.movefirst_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim movid As Integer = 0

        Try
            da = New SqlClient.SqlDataAdapter("select min(user_idno) from appuser_head Where user_idno <> 0", con)
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
            cmd.CommandText = "select max(user_idno) from appuser_head WHERE user_idno <> 0"

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
            cmd.CommandText = "select min(user_idno) from appuser_head where user_idno > " & Str(Val(lbl_IdNo.Text))

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
            da = New SqlClient.SqlDataAdapter("select max(user_idno) from appuser_head where user_idno < " & Str(Val(lbl_IdNo.Text)) & " and user_idno <> 0 ", con)
            da.Fill(dt)

            movid = 0
            If dt.Rows.Count > 0 Then
                If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                    movid = Val(dt.Rows(0)(0).ToString)
                End If
            End If

            If movid <> 0 Then move_record(movid)
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

        lbl_IdNo.Text = Common_Procedures.get_MaxIdNo(con, "appuser_head", "user_idno", "")
        If Val(lbl_IdNo.Text) < 100 Then lbl_IdNo.Text = 101



        If txt_User_Name.Enabled And txt_User_Name.Visible Then txt_User_Name.Focus()

    End Sub

    Public Sub open_record() Implements Interface_MDIActions.open_record
        Dim da As New SqlClient.SqlDataAdapter("select user_name from appuser_head order by user_name", con)
        Dim dt As New DataTable

        da.Fill(dt)

        cbo_Find.DataSource = dt
        cbo_Find.DisplayMember = "user_name"

        new_record()

        grp_Find.Visible = True
        grp_Find.BringToFront()
        pnl_Back.Enabled = False

        If cbo_Find.Enabled And cbo_Find.Visible Then cbo_Find.Focus()
        'Me.Height = 485 ' 355

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

        Dim vCHKTBL_ID As Integer
        Dim appuser_Id As Integer = 0
        Dim Sno As Integer = 0
        Dim ct_id As Integer = 0


        '  If Common_Procedures.UserRight_Check(Common_Procedures.UR.ItemGroup_Creation, New_Entry) = False Then Exit Sub

        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.SavingEntry, Common_Procedures.UR.ItemGroup_Creation, New_Entry, Me) = False Then Exit Sub

        If pnl_Back.Enabled = False Then
            MessageBox.Show("Close Other Windows", "DOES NOT SAVE", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        If Trim(txt_User_Name.Text) = "" Then
            MessageBox.Show("Invalid UserName", "DOES NOT SAVE", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            txt_User_Name.Focus()
            Exit Sub
        End If

        If Trim(txt_user_password.Text) = "" Then
            MessageBox.Show("Invalid Userpassword", "DOES NOT SAVE", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            txt_user_password.Focus()
            Exit Sub
        End If

        Sur = Common_Procedures.Remove_NonCharacters(Trim(txt_User_Name.Text))

        'With dgv_CheckingTableno_Details
        '    For i = 0 To .RowCount - 1
        '        If Val(.Rows(i).Cells(1).Value) <> 0 Then

        '            If Trim(.Rows(i).Cells(1).Value) = "" Then
        '                MessageBox.Show("Invalid Checking Tableno", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
        '                If dgv_CheckingTableno_Details.Enabled Then dgv_CheckingTableno_Details.Focus()
        '                dgv_CheckingTableno_Details.CurrentCell = dgv_CheckingTableno_Details.Rows(i).Cells(0)
        '                Exit Sub
        '            End If

        '        End If
        '    Next
        'End With

        trans = con.BeginTransaction

        Try
            cmd.Connection = con
            cmd.Transaction = trans

            If New_Entry = True Then

                lbl_IdNo.Text = Common_Procedures.get_MaxIdNo(con, "appuser_head", "user_idno", "", trans)
                If Val(lbl_IdNo.Text) < 100 Then lbl_IdNo.Text = 101

                cmd.CommandText = "Insert into appuser_head(  user_idno            ,                      user_name      ,       user_password               ,                 sur_name       ) " &
                                                    "values (" & Str(Val(lbl_IdNo.Text)) & ", '" & Trim(txt_User_Name.Text) & "','" & Trim(txt_user_password.Text) & "', '" & Trim(Sur) & "' )"
                cmd.ExecuteNonQuery()

            Else
                cmd.CommandText = "update appuser_head set user_name = '" & Trim(txt_User_Name.Text) & "', user_password = '" & Trim(txt_user_password.Text) & "', sur_name = '" & Trim(Sur) & "'  where user_idno = " & Str(Val(lbl_IdNo.Text))
                cmd.ExecuteNonQuery()

            End If




            cmd.CommandText = "delete from AppUser_Checking_Tableno_Details where user_idno = " & Str(Val(lbl_IdNo.Text) & "")
            cmd.ExecuteNonQuery()

            With dgv_CheckingTableno_Details
                Sno = 0
                For i = 0 To .RowCount - 1

                    vCHKTBL_ID = Common_Procedures.Checking_TableNo_NameToIdNo(con, .Rows(i).Cells(1).Value, trans)

                    If Val(vCHKTBL_ID) <> 0 Then

                        Sno = Sno + 1

                        cmd.CommandText = "Insert into AppUser_Checking_Tableno_Details(user_idno, sl_No, Checking_Table_IdNo) Values (" & Str(Val(lbl_IdNo.Text)) & ", " & Str(Val(Sno)) & ", " & Val(vCHKTBL_ID) & "  )"
                        cmd.ExecuteNonQuery()

                    End If

                Next

            End With

            trans.Commit()

            Common_Procedures.Master_Return.Return_Value = Trim(txt_User_Name.Text)
            Common_Procedures.Master_Return.Master_Type = "APPUSER"

            MessageBox.Show("Saved Sucessfully!!!", "FOR SAVING....", MessageBoxButtons.OKCancel, MessageBoxIcon.Information)

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

            If InStr(1, Trim(LCase(ex.Message)), Trim(LCase("ix_Cloth_Sales_Rate_Head"))) > 0 Then
                MessageBox.Show("Duplicate User Name", "DOES NOT SAVE", MessageBoxButtons.OK, MessageBoxIcon.Error)

            Else
                MessageBox.Show(ex.Message, "DOES NOT SAVE", MessageBoxButtons.OK, MessageBoxIcon.Error)

            End If

        Finally
            If txt_User_Name.Enabled And txt_User_Name.Visible Then txt_User_Name.Focus()

        End Try
    End Sub
    Protected Overrides Function ProcessCmdKey(ByRef msg As System.Windows.Forms.Message, ByVal keyData As System.Windows.Forms.Keys) As Boolean
        Dim dgv1 As New DataGridView

        If ActiveControl.Name = dgv_CheckingTableno_Details.Name Or TypeOf ActiveControl Is DataGridViewTextBoxEditingControl Then


            On Error Resume Next

            dgv1 = Nothing

            If ActiveControl.Name = dgv_CheckingTableno_Details.Name Then
                dgv1 = dgv_CheckingTableno_Details

            ElseIf dgv_CheckingTableno_Details.IsCurrentRowDirty = True Then
                dgv1 = dgv_CheckingTableno_Details

            ElseIf dgv_ActiveCtrl_Name = dgv_CheckingTableno_Details.Name Then
                dgv1 = dgv_CheckingTableno_Details
            End If

            If IsNothing(dgv1) = True Then
                Return MyBase.ProcessCmdKey(msg, keyData)
                Exit Function
            End If

            With dgv1


                If dgv1.Name = dgv_CheckingTableno_Details.Name Then

                    If keyData = Keys.Enter Or keyData = Keys.Down Then
                        If .CurrentCell.ColumnIndex >= .ColumnCount - 1 Then
                            If .CurrentCell.RowIndex = .RowCount - 1 Then
                                save_record()

                            Else
                                .Focus()
                                .CurrentCell = .Rows(.CurrentCell.RowIndex + 1).Cells(1)

                            End If

                        Else
                            .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 1)

                        End If

                        Return True

                    ElseIf keyData = Keys.Up Then

                        If .CurrentCell.ColumnIndex <= 0 Then
                            If .CurrentCell.RowIndex = 0 Then
                                txt_user_password.Focus()

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
                End If
            End With

        Else

            Return MyBase.ProcessCmdKey(msg, keyData)

        End If

    End Function



    Private Sub dgv_CheckingTableno_Details_CellEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_CheckingTableno_Details.CellEnter

        dgv_ActiveCtrl_Name = dgv_CheckingTableno_Details.Name
        dgv_CheckingTableno_Details_GridCombo_Design()
    End Sub


    Private Sub dgv_CheckingTableno_Details_GridCombo_Design()
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable

        With dgv_CheckingTableno_Details
            If .CurrentCell.ColumnIndex = 1 Then

                If cbo_Checking_tableno.Visible = False Or Val(cbo_Checking_tableno.Tag) <> .CurrentCell.RowIndex Then

                    cbo_Checking_tableno.Tag = -1
                    Da = New SqlClient.SqlDataAdapter("select Checking_Table_No from Checking_TableNo_Head order by Checking_Table_No", con)
                    Da.Fill(Dt1)
                    cbo_Checking_tableno.DataSource = Dt1
                    cbo_Checking_tableno.DisplayMember = "Checking_Table_No"

                    cbo_Checking_tableno.Left = .Left + .GetCellDisplayRectangle(.CurrentCell.ColumnIndex, .CurrentCell.RowIndex, False).Left
                    cbo_Checking_tableno.Top = .Top + .GetCellDisplayRectangle(.CurrentCell.ColumnIndex, .CurrentCell.RowIndex, False).Top
                    cbo_Checking_tableno.Width = .CurrentCell.Size.Width
                    cbo_Checking_tableno.Text = Trim(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value)

                    cbo_Checking_tableno.Tag = Val(.CurrentCell.RowIndex)
                    cbo_Checking_tableno.Visible = True

                    cbo_Checking_tableno.BringToFront()
                    cbo_Checking_tableno.Focus()

                Else
                    cbo_Checking_tableno.Visible = False
                    cbo_Checking_tableno.Tag = -1
                    cbo_Checking_tableno.Text = ""

                End If

            Else

                cbo_Checking_tableno.Visible = False
                cbo_Checking_tableno.Tag = -1
                cbo_Checking_tableno.Text = ""

            End If

        End With
    End Sub

    Private Sub dgv_CheckingTableno_Details_EditingControlShowing(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewEditingControlShowingEventArgs) Handles dgv_CheckingTableno_Details.EditingControlShowing

        dgtxt_CheckingTableno_Details = CType(dgv_CheckingTableno_Details.EditingControl, DataGridViewTextBoxEditingControl)

    End Sub



    Private Sub btn_Save_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Save.Click
        save_record()
    End Sub

    Private Sub btn_Find_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Find.Click
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim movid As Integer

        da = New SqlClient.SqlDataAdapter("select user_idno from appuser_head where user_name = '" & Trim(cbo_Find.Text) & "'", con)
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
        'Me.Height = 350 ' 197
        pnl_Back.Enabled = True
        grp_Find.Visible = False
        If txt_User_Name.Enabled And txt_User_Name.Visible Then txt_User_Name.Focus()
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
                            Condt = " Where user_name like '" & Trim(FindStr) & "%' or user_name like '% " & Trim(FindStr) & "%' "
                        End If

                        da = New SqlClient.SqlDataAdapter("select user_name from appuser_head " & Condt & " order by user_name", con)
                        da.Fill(dt)

                        .DataSource = dt
                        .DisplayMember = "user_name"

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
        If txt_User_Name.Enabled And txt_User_Name.Visible Then txt_User_Name.Focus()

        'Me.Height = 270 '197

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

    Private Sub txt_Name_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_User_Name.KeyDown
        If e.KeyValue = 40 Then e.Handled = True : e.SuppressKeyPress = True : SendKeys.Send("{TAB}")

        If e.KeyValue = 38 Then

            If dgv_CheckingTableno_Details.Rows.Count > 0 Then
                dgv_CheckingTableno_Details.Focus()
                dgv_CheckingTableno_Details.CurrentCell = dgv_CheckingTableno_Details.Rows(0).Cells(1)

            Else
                btn_Save.Focus()

            End If
        End If
    End Sub

    Private Sub txt_Name_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_User_Name.KeyPress
        If Asc(e.KeyChar) = 13 Then
            e.Handled = True
            txt_user_password.Focus()

        End If
    End Sub

    Private Sub txt_user_password_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_user_password.KeyDown
        If e.KeyValue = 40 Then

            If dgv_CheckingTableno_Details.Rows.Count > 0 Then
                dgv_CheckingTableno_Details.Focus()
                dgv_CheckingTableno_Details.CurrentCell = dgv_CheckingTableno_Details.Rows(0).Cells(1)

            Else
                btn_Save.Focus()

            End If
        End If


        If e.KeyValue = 38 Then e.Handled = True : e.SuppressKeyPress = True : SendKeys.Send("+{TAB}")
    End Sub

    Private Sub txt_user_password_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_user_password.KeyPress

        If Asc(e.KeyChar) = 13 Then


            If dgv_CheckingTableno_Details.Rows.Count > 0 Then
                dgv_CheckingTableno_Details.Focus()
                dgv_CheckingTableno_Details.CurrentCell = dgv_CheckingTableno_Details.Rows(0).Cells(1)

            Else
                btn_Save.Focus()

            End If
        End If


    End Sub


    Private Sub txt_HSN_Percentage_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs)
        If e.KeyValue = 40 Then
            e.Handled = True : e.SuppressKeyPress = True
            If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                save_record()
            Else
                txt_User_Name.Focus()
            End If
        End If
        If e.KeyValue = 38 Then e.Handled = True : e.SuppressKeyPress = True : SendKeys.Send("+{TAB}")
    End Sub

    Private Sub txt_HSN_Percentage_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
        If Asc(e.KeyChar) = 13 Then
            e.Handled = True
            If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                save_record()
            Else
                txt_User_Name.Focus()
            End If
        End If

    End Sub

    Private Sub cbo_Checking_tableno_GotFocus(sender As Object, e As EventArgs) Handles cbo_Checking_tableno.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Checking_TableNo_Head", "Checking_Table_No", "", "(Checking_Table_IdNo = 0)")
    End Sub

    Private Sub cbo_Checking_tableno_KeyDown(sender As Object, e As KeyEventArgs) Handles cbo_Checking_tableno.KeyDown

        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Checking_tableno, Nothing, Nothing, "Checking_TableNo_Head", "Checking_Table_No", "", "(Checking_Table_IdNo = 0)")

        Try
            With dgv_CheckingTableno_Details
                If e.KeyValue = 38 And cbo_Checking_tableno.DroppedDown = False Then
                    e.Handled = True

                    If Val(dgv_CheckingTableno_Details.CurrentCell.RowIndex) <= 0 Then
                        txt_user_password.Focus()

                    Else
                        'dgv_CheckingTableno_Details.CurrentCell = dgv_CheckingTableno_Details.Rows(dgv_CheckingTableno_Details.CurrentCell.RowIndex - 1).Cells(1)
                        .Focus()
                        .CurrentCell = .Rows(.CurrentCell.RowIndex - 1).Cells(1)
                        .CurrentCell.Selected = True
                        dgv_CheckingTableno_Details.Focus()
                        cbo_Checking_tableno.Visible = False

                    End If
                End If

                If e.KeyValue = 40 And cbo_Checking_tableno.DroppedDown = False Then
                    e.Handled = True
                    If dgv_CheckingTableno_Details.CurrentRow.Index = dgv_CheckingTableno_Details.RowCount - 1 Then
                        save_record()

                    Else
                        dgv_CheckingTableno_Details.CurrentCell = dgv_CheckingTableno_Details.Rows(dgv_CheckingTableno_Details.CurrentCell.RowIndex + 1).Cells(dgv_CheckingTableno_Details.CurrentCell.ColumnIndex)
                        dgv_CheckingTableno_Details.CurrentCell.Selected = True
                        dgv_CheckingTableno_Details.Focus()
                        cbo_Checking_tableno.Visible = False



                    End If
                End If
            End With

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT SELECT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub cbo_Checking_tableno_KeyPress(sender As Object, e As KeyPressEventArgs) Handles cbo_Checking_tableno.KeyPress
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable


        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Checking_tableno, Nothing, "Checking_TableNo_Head", "Checking_Table_No", "", "(Checking_Table_IdNo = 0)")

        If Asc(e.KeyChar) = 13 Then

            Me.dgv_CheckingTableno_Details.Rows(Me.dgv_CheckingTableno_Details.CurrentCell.RowIndex).Cells.Item(1).Value = Trim(cbo_Checking_tableno.Text)

            If dgv_CheckingTableno_Details.CurrentRow.Index >= dgv_CheckingTableno_Details.RowCount - 1 Then

                'If Trim(cbo_Checking_tableno.Text) = "" Then
                If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                    save_record()
                Else

                    txt_User_Name.Focus()
                End If


                'Else
                '    Dim n As Integer
                '    n = dgv_CheckingTableno_Details.Rows.Add()
                '    dgv_CheckingTableno_Details.CurrentCell = dgv_CheckingTableno_Details.Rows(n).Cells(1)
                '    dgv_CheckingTableno_Details.CurrentCell.Selected = True
                '    dgv_CheckingTableno_Details.Focus()
                '    cbo_Checking_tableno.Visible = False

                'End If


            Else
                dgv_CheckingTableno_Details.CurrentCell = dgv_CheckingTableno_Details.Rows(dgv_CheckingTableno_Details.CurrentCell.RowIndex + 1).Cells(1)
                dgv_CheckingTableno_Details.CurrentCell.Selected = True
                dgv_CheckingTableno_Details.Focus()
                cbo_Checking_tableno.Visible = False

            End If

        End If


    End Sub

    Private Sub cbo_Checking_tableno_TextChanged(sender As Object, e As EventArgs) Handles cbo_Checking_tableno.TextChanged
        Try
            If cbo_Checking_tableno.Visible Then
                If Val(cbo_Checking_tableno.Tag) = Val(dgv_CheckingTableno_Details.CurrentCell.RowIndex) And dgv_CheckingTableno_Details.CurrentCell.ColumnIndex = 1 Then
                    dgv_CheckingTableno_Details.Rows(dgv_CheckingTableno_Details.CurrentCell.RowIndex).Cells.Item(1).Value = Trim(cbo_Checking_tableno.Text)
                End If
            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub dgv_CheckingTableno_Details_KeyUp(sender As Object, e As KeyEventArgs) Handles dgv_CheckingTableno_Details.KeyUp
        Dim n As Integer
        Dim i As Integer

        Try

            If e.Control = True And UCase(Chr(e.KeyCode)) = "D" Then

                With dgv_CheckingTableno_Details
                    If .CurrentRow.Index = .RowCount - 1 Then
                        For i = 1 To .ColumnCount - 1
                            .Rows(.CurrentRow.Index).Cells(i).Value = ""

                        Next

                    Else

                        n = .CurrentRow.Index
                        .Rows.RemoveAt(n)
                    End If

                    For i = 0 To .Rows.Count - 1
                        .Rows(i).Cells(0).Value = i + 1

                    Next

                End With
            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub dgv_CheckingTableno_Details_RowsAdded(sender As Object, e As DataGridViewRowsAddedEventArgs) Handles dgv_CheckingTableno_Details.RowsAdded
        With dgv_CheckingTableno_Details
            If Val(.Rows(.RowCount - 1).Cells(0).Value) = 0 Then
                .Rows(.RowCount - 1).Cells(0).Value = .RowCount
            End If
        End With
    End Sub

    Private Sub dgv_CheckingTableno_Details_KeyPress(sender As Object, e As KeyPressEventArgs) Handles dgv_CheckingTableno_Details.KeyPress
        If Asc(e.KeyChar) = 13 Then
            If MessageBox.Show("Do you want to save", "FOR SAVING...", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then
                save_record()
            End If
        End If
    End Sub

    Private Sub dgtxt_CheckingTableno_Details_Enter(sender As Object, e As EventArgs) Handles dgtxt_CheckingTableno_Details.Enter

        'dgv_ActiveCtrl_Name = dgv_CheckingTableno_Details.Name
        dgv_CheckingTableno_Details.EditingControl.BackColor = Color.Lime
        dgv_CheckingTableno_Details.EditingControl.ForeColor = Color.Blue
        dgv_CheckingTableno_Details.SelectAll()

    End Sub

    Private Sub AppUser_Creation_Activated(sender As Object, e As EventArgs) Handles Me.Activated
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim dt3 As New DataTable
        Dim dt4 As New DataTable

        Try
            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Checking_tableno.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "ENDSCOUNT" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then


                cbo_Checking_tableno.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If
        Catch ex As Exception

        End Try

        FrmLdSTS = False


    End Sub

    Private Sub cbo_Checking_tableno_KeyUp(sender As Object, e As KeyEventArgs) Handles cbo_Checking_tableno.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Dim f As New Checking_table_creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Checking_tableno.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()
        End If
    End Sub

    Private Sub txt_User_Name_GotFocus(sender As Object, e As EventArgs) Handles txt_User_Name.GotFocus
        txt_User_Name.BackColor = Color.Lime
        txt_User_Name.ForeColor = Color.Blue
    End Sub

    Private Sub txt_User_Name_LostFocus(sender As Object, e As EventArgs) Handles txt_User_Name.LostFocus
        txt_User_Name.BackColor = Color.White
        txt_User_Name.ForeColor = Color.Black
    End Sub

End Class