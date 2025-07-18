Public Class Bank_Creation

    Implements Interface_MDIActions

    Private con As New SqlClient.SqlConnection(Common_Procedures.Connection_String)
    Private New_Entry As Boolean = False
    Private Prec_ActCtrl As New Control

    Private Sub clear()
        Dim obj As Object
        Dim ctrl As Object
        Dim grpbx As GroupBox

        For Each obj In Me.Controls
            If TypeOf obj Is TextBox Then
                obj.text = ""
            ElseIf TypeOf obj Is ComboBox Then
                obj.text = ""
            ElseIf TypeOf obj Is GroupBox Then
                grpbx = obj
                For Each ctrl In grpbx.Controls
                    If TypeOf ctrl Is TextBox Then
                        ctrl.text = ""
                    ElseIf TypeOf ctrl Is ComboBox Then
                        ctrl.text = ""
                    End If

                Next

            End If
        Next

        New_Entry = False



        lbl_IdNo.ForeColor = Color.Black
        Me.Height = 245

        grp_Back.Enabled = True
        grp_Open.Visible = False
        grp_Filter.Visible = False

    End Sub

    Private Sub ControlGotFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim txtbx As TextBox
        Dim combobx As ComboBox

        On Error Resume Next

        Me.ActiveControl.BackColor = Color.Lime
        Me.ActiveControl.ForeColor = Color.Blue

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
            Prec_ActCtrl.BackColor = Color.White
            Prec_ActCtrl.ForeColor = Color.Black
        End If

    End Sub

    Public Sub move_record(ByVal idno As Integer)
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable

        If Val(idno) = 0 Then Exit Sub

        clear()

        da = New SqlClient.SqlDataAdapter("select a.Ledger_IdNo, a.Ledger_Name , a.Ledger_MainName from ledger_head a  where A.Ledger_Type = 'BANK' and   a.ledger_idno = " & Str(Val(idno)) & "   ", con)
        dt = New DataTable
        da.Fill(dt)

        If dt.Rows.Count > 0 Then
            lbl_IdNo.Text = dt.Rows(0).Item("Ledger_IdNo").ToString
            txt_Name.Text = dt.Rows(0).Item("Ledger_MainName").ToString
           
        End If

        dt.Dispose()
        da.Dispose()

        If txt_Name.Enabled And txt_Name.Visible Then txt_Name.Focus()

    End Sub

    Public Sub new_record() Implements Interface_MDIActions.new_record

        Call clear()

        lbl_IdNo.ForeColor = Color.Red
        New_Entry = True

        lbl_IdNo.Text = Common_Procedures.get_MaxIdNo(con, "ledger_head", "ledger_idno", "")

        If Val(lbl_IdNo.Text) < 101 Then lbl_IdNo.Text = 101

        If txt_Name.Enabled And txt_Name.Visible Then txt_Name.Focus()

    End Sub

    Public Sub delete_record() Implements Interface_MDIActions.delete_record
        Dim cmd As New SqlClient.SqlCommand
        Dim da As SqlClient.SqlDataAdapter
        Dim dt As DataTable

        If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.Ledger_Creation, "~L~") = 0 And InStr(Common_Procedures.UR.Ledger_Creation, "~D~") = 0 Then MessageBox.Show("You have No Rights to Delete", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error) : Exit Sub

        If MessageBox.Show("Do you want to delete", "FOR DELETION...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) <> Windows.Forms.DialogResult.Yes Then
            Exit Sub
        End If

        Try

            da = New SqlClient.SqlDataAdapter("select count(*) from Cheque_Print_Positioning_Head where Ledger_IdNo = " & Str(Val(lbl_IdNo.Text)), con)
            dt = New DataTable
            da.Fill(dt)
            If dt.Rows.Count > 0 Then
                If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                    If Val(dt.Rows(0)(0).ToString) > 0 Then
                        MessageBox.Show("Already used this Ledger", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        Exit Sub
                    End If
                End If
            End If

            da = New SqlClient.SqlDataAdapter("select count(*) from Voucher_Details where Ledger_IdNo = " & Str(Val(lbl_IdNo.Text)), con)
            dt = New DataTable
            da.Fill(dt)
            If dt.Rows.Count > 0 Then
                If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                    If Val(dt.Rows(0)(0).ToString) > 0 Then
                        MessageBox.Show("Already used this Ledger", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        Exit Sub
                    End If
                End If
            End If

            da = New SqlClient.SqlDataAdapter("select count(*) from Voucher_Bill_Head where Ledger_IdNo = " & Str(Val(lbl_IdNo.Text)), con)
            dt = New DataTable
            da.Fill(dt)
            If dt.Rows.Count > 0 Then
                If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                    If Val(dt.Rows(0)(0).ToString) > 0 Then
                        MessageBox.Show("Already used this Ledger", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        Exit Sub
                    End If
                End If
            End If

            cmd.Connection = con

            cmd.CommandText = "delete from Ledger_AlaisHead where Ledger_IdNo = " & Str(Val(lbl_IdNo.Text))
            cmd.ExecuteNonQuery()

            cmd.CommandText = "delete from ledger_head where ledger_idno = " & Str(Val(lbl_IdNo.Text))
            cmd.ExecuteNonQuery()

            cmd.Dispose()
            dt.Dispose()
            da.Dispose()

            new_record()

            MessageBox.Show("Deleted Sucessfully!!", "FOR DELETION...", MessageBoxButtons.OK, MessageBoxIcon.Information)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT DELETE..", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If txt_Name.Enabled And txt_Name.Visible Then txt_Name.Focus()
            Exit Sub

        End Try

    End Sub

    Public Sub filter_record() Implements Interface_MDIActions.filter_record
        Dim da As New SqlClient.SqlDataAdapter("select ledger_idno, ledger_name from ledger_head where ledger_idno <> 0  and Ledger_Type = 'BANK' order by ledger_idno", con)
        Dim dt As New DataTable

        da.Fill(dt)

        dgv_Filter.Columns.Clear()
        dgv_Filter.DataSource = dt
        dgv_Filter.RowHeadersVisible = False

        dgv_Filter.Columns(0).HeaderText = "IDNO"
        dgv_Filter.Columns(1).HeaderText = "LEDGER NAME"

        dgv_Filter.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells
        dgv_Filter.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill

        dgv_Filter.Columns(0).FillWeight = 35
        dgv_Filter.Columns(1).FillWeight = 165

        grp_Filter.Visible = True
        grp_Filter.BringToFront()
        dgv_Filter.Focus()

        grp_Back.Enabled = False
        Me.Height = 540

        da.Dispose()

    End Sub

    Public Sub movefirst_record() Implements Interface_MDIActions.movefirst_record
        Dim cmd As New SqlClient.SqlCommand
        Dim movid As Integer

        Try
            cmd.Connection = con
            cmd.CommandText = "select min(ledger_idno) from ledger_head Where ledger_idno <> 0 and Ledger_Type = 'BANK' "

            movid = 0
            If IsDBNull(cmd.ExecuteScalar()) = False Then
                movid = cmd.ExecuteScalar
            End If

            cmd.Dispose()

            If movid <> 0 Then Call move_record(movid)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR MOVING....", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Public Sub movelast_record() Implements Interface_MDIActions.movelast_record
        Dim cmd As New SqlClient.SqlCommand
        Dim movid As Integer

        Try
            cmd.Connection = con
            cmd.CommandText = "select max(ledger_idno) from ledger_head where ledger_idno <> 0 and Ledger_Type = 'BANK' "

            movid = 0
            If IsDBNull(cmd.ExecuteScalar()) = False Then
                movid = cmd.ExecuteScalar
            End If

            cmd.Dispose()

            If movid <> 0 Then move_record(movid)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR MOVING....", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Public Sub movenext_record() Implements Interface_MDIActions.movenext_record
        Dim cmd As New SqlClient.SqlCommand("select min(ledger_idno) from ledger_head where ledger_idno <> 0 and  Ledger_Type = 'BANK' and ledger_idno > " & Str(Val(lbl_IdNo.Text)), con)
        Dim movid As Integer = 0

        Try
            movid = 0
            If IsDBNull(cmd.ExecuteScalar()) = False Then
                movid = cmd.ExecuteScalar
            End If

            cmd.Dispose()

            If movid <> 0 Then move_record(movid)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Public Sub moveprevious_record() Implements Interface_MDIActions.moveprevious_record
        Dim cmd As New SqlClient.SqlCommand
        Dim movid As Integer = 0

        Try
            cmd.Connection = con
            cmd.CommandText = "select max(ledger_idno ) from ledger_head where ledger_idno <> 0 and  Ledger_Type = 'BANK' and ledger_idno < " & Str((lbl_IdNo.Text))

            movid = 0
            If IsDBNull(cmd.ExecuteScalar()) = False Then
                movid = cmd.ExecuteScalar()
            End If

            cmd.Dispose()

            If movid <> 0 Then move_record(movid)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR MOVING....", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Public Sub open_record() Implements Interface_MDIActions.open_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable

        da = New SqlClient.SqlDataAdapter("select Ledger_DisplayName from Ledger_AlaisHead where (Ledger_IdNo = 0 or Ledger_type = 'BANK') order by Ledger_DisplayName", con)
        da.Fill(dt)

        cbo_Open.DataSource = dt
        cbo_Open.DisplayMember = "Ledger_DisplayName"

        da.Dispose()
        Me.Height = 455

        grp_Open.Visible = True
        grp_Open.BringToFront()
        If cbo_Open.Enabled And cbo_Open.Visible Then cbo_Open.Focus()
        grp_Back.Enabled = False
        
    End Sub

    Public Sub insert_record() Implements Interface_MDIActions.insert_record
        'MessageBox.Show("insert record")
    End Sub

    Public Sub print_record() Implements Interface_MDIActions.print_record
        'MessageBox.Show("Ledger creation  -  print")
    End Sub

    Public Sub save_record() Implements Interface_MDIActions.save_record
        Dim cmd As New SqlClient.SqlCommand
        Dim trans As SqlClient.SqlTransaction
        Dim dt As New DataTable
        Dim nr As Long = 0
        Dim acgrp_idno As Integer = 0
        Dim ar_idno As Integer = 0
        Dim Parnt_CD As String = ""
        Dim LedName As String = ""
        Dim SurName As String = ""
        Dim LedArName As String = ""
        Dim LedPhNo As String = ""
        'Dim PhAr() As String
        Dim Sno As Integer = 0

        If Common_Procedures.UserRight_Check(Common_Procedures.UR.Ledger_Creation, New_Entry) = False Then Exit Sub

        If grp_Back.Enabled = False Then
            MessageBox.Show("Close other window", "DOES NOT SAVE", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        If Trim(txt_Name.Text) = "" Then
            MessageBox.Show("Invalid Legder Name", "DOES NOT SAVE", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If txt_Name.Enabled Then txt_Name.Focus()
            Exit Sub
        End If


        If acgrp_idno = 0 Then
            acgrp_idno = 5

        End If

        Parnt_CD = Common_Procedures.AccountsGroup_IdNoToCode(con, acgrp_idno)
        LedName = Trim(txt_Name.Text)
      
        SurName = Common_Procedures.Remove_NonCharacters(LedName)

        trans = con.BeginTransaction

        Try

            cmd.Transaction = trans

            cmd.Connection = con

            If New_Entry = True Then
                lbl_IdNo.Text = Common_Procedures.get_MaxIdNo(con, "ledger_head", "ledger_idno", "", trans)
                If Val(lbl_IdNo.Text) < 101 Then lbl_IdNo.Text = 101

                cmd.CommandText = "Insert into ledger_head(Ledger_IdNo, Ledger_Name, Sur_Name, Ledger_MainName,  AccountsGroup_IdNo ,  Parent_Code,  Ledger_Type) Values (" & Str(Val(lbl_IdNo.Text)) & ", '" & Trim(LedName) & "', '" & Trim(SurName) & "', '" & Trim(txt_Name.Text) & "', " & Str(Val(acgrp_idno)) & ", '" & Trim(Parnt_CD) & "',  'BANK')"

            Else
                cmd.CommandText = "Update ledger_head set Ledger_Name = '" & Trim(LedName) & "', Sur_Name = '" & Trim(SurName) & "', Ledger_MainName = '" & Trim(txt_Name.Text) & "', AccountsGroup_IdNo = " & Str(Val(acgrp_idno)) & ", Parent_Code = '" & Trim(Parnt_CD) & "' where Ledger_IdNo = " & Str(Val(lbl_IdNo.Text))

            End If

            cmd.ExecuteNonQuery()

            cmd.CommandText = "delete from Ledger_AlaisHead where Ledger_IdNo = " & Str(Val(lbl_IdNo.Text))
            cmd.ExecuteNonQuery()

          

            cmd.CommandText = "Insert into Ledger_AlaisHead(Ledger_IdNo, Sl_No, Ledger_DisplayName, Ledger_Type, AccountsGroup_IdNo ) Values (" & Str(Val(lbl_IdNo.Text)) & ", 1, '" & Trim(LedName) & "', 'BANK', " & Str(Val(acgrp_idno)) & " )"
            cmd.ExecuteNonQuery()

            'If Trim(txt_AlaisName.Text) <> "" Then
            '    LedArName = Trim(txt_AlaisName.Text)
            '    If Val(ar_idno) <> 0 Then
            '        LedArName = Trim(txt_AlaisName.Text) & " (" & Trim(cbo_Area.Text) & ")"
            '    End If

            '    cmd.CommandText = "Insert into Ledger_AlaisHead(Ledger_IdNo, Sl_No, Ledger_DisplayName, Ledger_Type, AccountsGroup_IdNo  ) Values (" & Str(Val(lbl_IdNo.Text)) & ", 2, '" & Trim(LedName) & "', '', " & Str(Val(acgrp_idno)) & " )"
            '    cmd.ExecuteNonQuery()

            'End If

            trans.Commit()

            trans.Dispose()
            dt.Dispose()

            Common_Procedures.Master_Return.Return_Value = Trim(txt_Name.Text)
            Common_Procedures.Master_Return.Master_Type = "LEDGER"

            If New_Entry = True Then new_record()

            MessageBox.Show("Sucessfully Saved", "FOR SAVING...", MessageBoxButtons.OK, MessageBoxIcon.Information)

        Catch ex As Exception
            trans.Rollback()

            If InStr(1, Trim(LCase(ex.Message)), "ix_ledger_head") > 0 Then
                MessageBox.Show("Duplicate Ledger Name", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            ElseIf InStr(1, Trim(LCase(ex.Message)), "ix_ledger_alaishead") > 0 Then
                MessageBox.Show("Duplicate Ledger Alais Name", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Else
                MessageBox.Show(ex.Message, "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If

            Exit Sub

        End Try

    End Sub

    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        save_record()
    End Sub

    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub

    Private Sub Ledger_Creation_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated
       
    End Sub

    Private Sub Ledger_Creation_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable

        con.Open()

       

        da.Dispose()

        grp_Open.Left = 6
        grp_Open.Top = 220
        grp_Open.Visible = False

        grp_Filter.Left = 6
        grp_Filter.Top = 220
        grp_Filter.Visible = False

        AddHandler txt_Name.GotFocus, AddressOf ControlGotFocus
       

        AddHandler txt_Name.LostFocus, AddressOf ControlLostFocus
       

        new_record()

    End Sub

    Private Sub Ledger_Creation_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        con.Close()
        con.Dispose()
    End Sub

    Private Sub Ledger_Creation_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If Asc(e.KeyChar) = 27 Then
            If grp_Open.Visible Then
                btn_CloseOpen_Click(sender, e)
            ElseIf grp_Filter.Visible Then
                btn_CloseFilter_Click(sender, e)
            Else
                Me.Close()
            End If

        End If
    End Sub

    Private Sub btn_CloseOpen_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_CloseOpen.Click
        Me.Height = 245

        grp_Back.Enabled = True
        grp_Open.Visible = False
    End Sub

    Private Sub btn_Find_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Find.Click
        'Dim cmd As New SqlClient.SqlCommand
        'Dim dr As SqlClient.SqlDataReader
        Dim movid As Integer

        If Trim(cbo_Open.Text) = "" Then
            MessageBox.Show("Invalid Ledger Name", "DOES NOT FIND...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If cbo_Open.Enabled Then cbo_Open.Focus()
            Exit Sub
        End If

        movid = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Open.Text)

        'cmd.CommandText = "select ledger_idno from ledger_head where ledger_name = '" & Trim(cbo_Open.Text) & "'"
        'cmd.Connection = con

        'movid = 0

        'dr = cmd.ExecuteReader()
        'If dr.HasRows Then
        '    If dr.Read Then
        '        If IsDBNull(dr(0).ToString) = False Then
        '            movid = Val((dr(0).ToString))
        '        End If
        '    End If
        'End If
        'dr.Close()
        'cmd.Dispose()

        If movid <> 0 Then move_record(movid)

        grp_Back.Enabled = True
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
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Open, Nothing, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'BANK')", "(Ledger_IdNo = 0)")
        If Asc(e.KeyChar) = 13 Then
            Call btn_Find_Click(sender, e)
        End If
    End Sub

    Private Sub btn_CloseFilter_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_CloseFilter.Click
        Me.Height = 245
        grp_Back.Enabled = True
        grp_Filter.Visible = False
    End Sub

    Private Sub btn_Filter_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Filter.Click
        Dim idno As Integer

        idno = Val(dgv_Filter.CurrentRow.Cells(0).Value)

        If Val(idno) <> 0 Then
            move_record(idno)
            grp_Back.Enabled = True
            grp_Filter.Visible = False
        End If


    End Sub

    Private Sub dgv_Filter_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_Filter.KeyDown
        If e.KeyValue = 13 Then
            Call btn_Filter_Click(sender, e)
        End If
    End Sub

    Private Sub txt_Name_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Name.KeyPress
        Dim K As Integer

        If Asc(e.KeyChar) >= 97 And Asc(e.KeyChar) <= 122 Then
            K = Asc(e.KeyChar)
            K = K - 32
            e.KeyChar = Chr(K)
        End If

        If Asc(e.KeyChar) = 13 Then
            If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                save_record()
            End If
        End If
    End Sub

    Private Sub txt_Name_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_Name.KeyDown
        If e.KeyValue = 40 Then
            If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                save_record()
            End If
        End If


    End Sub

    

   
End Class
