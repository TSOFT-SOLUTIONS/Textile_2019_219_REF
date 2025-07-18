Public Class Market_Status_Creation
    Implements Interface_MDIActions

    Private Con As New SqlClient.SqlConnection(Common_Procedures.Connection_String)
    Private New_Entry As Boolean = False
    Private Prec_ActCtrl As New Control

    Private Sub Clear()
        Me.Height = 240
        New_Entry = False
        pnl_Back.Enabled = True
        lbl_IdNo.Text = ""
        lbl_IdNo.ForeColor = Color.Black
        txt_Name.Text = ""
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
            If TypeOf Prec_ActCtrl Is Button Then
                Prec_ActCtrl.BackColor = Color.FromArgb(44, 61, 90)
                Prec_ActCtrl.ForeColor = Color.White
            Else
                Prec_ActCtrl.BackColor = Color.White
                Prec_ActCtrl.ForeColor = Color.Black
            End If
        End If

    End Sub


    Public Sub move_record(ByVal idno As Integer)
        Dim da As SqlClient.SqlDataAdapter
        Dim dt As DataTable

        Try

            If Val(idno) = 0 Then Exit Sub

            Clear()

            da = New SqlClient.SqlDataAdapter("select * from MarketStatus_Head where MarketStatus_IdNo = " & Str(Val(idno)), Con)
            dt = New DataTable
            da.Fill(dt)

            If dt.Rows.Count > 0 Then
                lbl_IdNo.Text = dt.Rows(0).Item("MarketStatus_IdNo").ToString
                txt_Name.Text = dt.Rows(0).Item("MarketStatus_Name").ToString

            Else
                new_record()

            End If

            dt.Dispose()
            da.Dispose()

            If txt_Name.Enabled And txt_Name.Visible Then txt_Name.Focus()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT MOVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try



    End Sub

    Public Sub delete_record() Implements Interface_MDIActions.delete_record
        Dim tr As SqlClient.SqlTransaction
        Dim cmd As New SqlClient.SqlCommand

        ' If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.Masters_Market_Status_Creation, "~L~") = 0 And InStr(Common_Procedures.UR.Masters_Market_Status_Creation, "~D~") = 0 Then MessageBox.Show("You have No Rights to Delete", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error) : Exit Sub

        If MessageBox.Show("Do you want to Delete ?", "FOR DELETION....", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) <> Windows.Forms.DialogResult.Yes Then
            Exit Sub
        End If

        If New_Entry = True Then
            MessageBox.Show("This is new entry", "DOES NOT DELETION....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
        End If

        If Val(lbl_IdNo.Text) < 101 Then
            MessageBox.Show("Cannot delete this default MarketStatus", "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
        End If

        tr = Con.BeginTransaction

        Try

            cmd.Connection = Con
            cmd.Transaction = tr

            cmd.CommandText = "delete from MarketStatus_Head where MarketStatus_IdNo = " & Str(Val(lbl_IdNo.Text))
            cmd.ExecuteNonQuery()

            tr.Commit()

            new_record()

            MessageBox.Show("Deleted Sucessfully!!!", "FOR DELETION...", MessageBoxButtons.OKCancel, MessageBoxIcon.Information)

        Catch ex As Exception
            tr.Rollback()
            MessageBox.Show(ex.Message, "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Finally
            cmd.Dispose()
            tr.Dispose()
            If txt_Name.Enabled And txt_Name.Visible Then txt_Name.Focus()

        End Try

    End Sub

    Public Sub filter_record() Implements Interface_MDIActions.filter_record
        '-----
    End Sub

    Public Sub insert_record() Implements Interface_MDIActions.insert_record
        '------
    End Sub

    Public Sub movefirst_record() Implements Interface_MDIActions.movefirst_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim movid As Integer = 0

        Try

            da = New SqlClient.SqlDataAdapter("select min(MarketStatus_IdNo) from MarketStatus_Head Where MarketStatus_IdNo <> 0", Con)
            dt = New DataTable
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
            MessageBox.Show(ex.Message, "DOES NOT MOVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Public Sub movelast_record() Implements Interface_MDIActions.movelast_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim movid As Integer = 0

        Try
            da = New SqlClient.SqlDataAdapter("select max(MarketStatus_IdNo) from MarketStatus_Head Where MarketStatus_IdNo <> 0", Con)
            dt = New DataTable
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
            MessageBox.Show(ex.Message, "DOES NOT MOVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Public Sub movenext_record() Implements Interface_MDIActions.movenext_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim movid As Integer = 0

        Try

            da = New SqlClient.SqlDataAdapter("select min(MarketStatus_IdNo) from MarketStatus_Head Where MarketStatus_IdNo > " & Str(Val(lbl_IdNo.Text)) & " and MarketStatus_IdNo <> 0", Con)
            dt = New DataTable
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
            da = New SqlClient.SqlDataAdapter("select max(MarketStatus_IdNo) from MarketStatus_Head Where MarketStatus_IdNo < " & Str(Val(lbl_IdNo.Text)) & " and MarketStatus_IdNo <> 0", con)
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
        Clear()
        lbl_IdNo.Text = Common_Procedures.get_MaxIdNo(Con, "MarketStatus_Head", "MarketStatus_IdNo", "")
        If Val(lbl_IdNo.Text) < 101 Then lbl_IdNo.Text = 101
        lbl_IdNo.ForeColor = Color.Red
        New_Entry = True
        If txt_Name.Enabled = True And txt_Name.Visible = True Then txt_Name.Focus()
    End Sub

    Public Sub open_record() Implements Interface_MDIActions.open_record
        Me.Height = 440
        grp_Open.Visible = True
        pnl_Back.Enabled = False
        cbo_Open.Focus()
    End Sub

    Public Sub print_record() Implements Interface_MDIActions.print_record
        '----
    End Sub

    Public Sub save_record() Implements Interface_MDIActions.save_record
        Dim cmd As New SqlClient.SqlCommand
        Dim tr As SqlClient.SqlTransaction
        Dim vSurNm As String = ""

        '   If Common_Procedures.UserRight_Check(Common_Procedures.UR.Masters_Market_Status_Creation, New_Entry) = False Then Exit Sub

        If pnl_Back.Enabled = False Then
            MessageBox.Show("Close other window", "DOES NOT SAVE", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
        End If

        If Trim(txt_Name.Text) = "" Then
            MessageBox.Show("Invalid Market Status Name", "DOES NOT SAVE", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If txt_Name.Enabled Then txt_Name.Focus()
            Exit Sub
        End If


        vSurNm = Common_Procedures.Remove_NonCharacters(txt_Name.Text)

        tr = Con.BeginTransaction
        Try


            cmd.Connection = Con
            cmd.Transaction = tr

            If New_Entry = True Then

                lbl_IdNo.Text = Common_Procedures.get_MaxIdNo(Con, "MarketStatus_Head", "MarketStatus_IdNo", "", tr)
                If Val(lbl_IdNo.Text) < 101 Then lbl_IdNo.Text = 101

                cmd.CommandText = "Insert into MarketStatus_Head(MarketStatus_IdNo, MarketStatus_Name, Sur_Name ) Values (" & Str(Val(lbl_IdNo.Text)) & ", '" & Trim(txt_Name.Text) & "', '" & Trim(vSurNm) & "' )"
                cmd.ExecuteNonQuery()

            Else
                cmd.CommandText = "Update MarketStatus_Head set MarketStatus_Name = '" & Trim(txt_Name.Text) & "', Sur_Name = '" & Trim(vSurNm) & "' Where MarketStatus_IdNo = " & Str(Val(lbl_IdNo.Text))
                cmd.ExecuteNonQuery()

            End If

            tr.Commit()

            Common_Procedures.Master_Return.Return_Value = Trim(txt_Name.Text)
            Common_Procedures.Master_Return.Master_Type = "MARKETSTATUS"

            If New_Entry = True Then new_record()

            MessageBox.Show("Sucessfully Saved", "FOR SAVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Information)

        Catch ex As Exception
            tr.Rollback()
            If InStr(1, LCase(ex.Message), LCase("Duplicate_MarketStatusHead_Name")) > 0 Then
                MessageBox.Show("Duplicate Market Status Name", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Else
                MessageBox.Show(ex.Message, "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            End If


        End Try


    End Sub

    Private Sub btn_Save_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Save.Click
        save_record()
    End Sub

    Private Sub btn_Close_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Close.Click
        Me.Close()
    End Sub

    Private Sub Market_Status_Creation_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Con.Open()

        Me.Height = 240

        grp_Open.Visible = False
        grp_Open.Left = pnl_Back.Left
        grp_Open.Top = 205

        AddHandler txt_Name.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_Save.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_Close.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Open.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_Open.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_CloseOpen.GotFocus, AddressOf ControlGotFocus

        AddHandler txt_Name.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_Save.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_Save.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Open.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_Open.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_CloseOpen.LostFocus, AddressOf ControlLostFocus

        new_record()
    End Sub

    Private Sub Market_Status_Creation_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        con.Close()
        con.Dispose()
    End Sub

    Private Sub Market_Status_Creation_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If Asc(e.KeyChar) = 27 Then
            If grp_Open.Visible Then
                btn_CloseOpen_Click(sender, e)
            Else
                Me.Close()
            End If
        End If
    End Sub

    Private Sub cbo_Open_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Open.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, Con, "MarketStatus_Head", "MarketStatus_Name", "", "(MarketStatus_IdNo = 0)")
    End Sub

    Private Sub cbo_Open_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Open.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Open, Nothing, Nothing, "MarketStatus_Head", "MarketStatus_Name", "", "(MarketStatus_IdNo = 0)")
    End Sub

    Private Sub cbo_Open_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Open.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Open, Nothing, "MarketStatus_Head", "MarketStatus_Name", "", "(MarketStatus_IdNo = 0)")

        If Asc(e.KeyChar) = 13 Then
            Call btn_Open_Click(sender, e)
        End If

    End Sub

    Private Sub btn_Open_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Open.Click
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim movid As Integer = 0

        da = New SqlClient.SqlDataAdapter("select MarketStatus_IdNo from MarketStatus_Head where MarketStatus_Name = '" & Trim(cbo_Open.Text) & "'", Con)
        dt = New DataTable
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

    Private Sub btn_CloseOpen_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_CloseOpen.Click
        Me.Height = 240
        pnl_Back.Enabled = True
        grp_Open.Visible = False
    End Sub

    Private Sub txt_Name_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Name.KeyPress
        If Asc(e.KeyChar) = 39 Then
            e.Handled = True
        End If
        If Asc(e.KeyChar) = 13 Then
            If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                save_record()
            Else
                txt_Name.Focus()
            End If
        End If
    End Sub

End Class