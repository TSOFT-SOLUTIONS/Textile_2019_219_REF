Public Class GST_Account_Settings
    Implements Interface_MDIActions

    Private con As New SqlClient.SqlConnection(Common_Procedures.Connection_String)
    Private con1 As New SqlClient.SqlConnection(Common_Procedures.ConnectionString_CompanyGroupdetails)
    Private FrmLdSTS As Boolean = False
    Private New_Entry As Boolean = False
    Private Insert_Entry As Boolean = False
    Private Filter_Status As Boolean = False
    Private Prec_ActCtrl As New Control
    Private vcbo_KeyDwnVal As Double

    Private Sub clear()

        New_Entry = False

        pnl_Back.Enabled = True

        lbl_RefNO.Text = ""
        lbl_RefNO.ForeColor = Color.Black
        txt_GST_Percentage.Text = ""

        cbo_Input_CGST_Acc.Text = ""
        cbo_Input_SGST_Acc.Text = ""
        cbo_Input_IGST_Acc.Text = ""

        cbo_output_CGST_Acc.Text = ""
        cbo_Output_SGST_Acc.Text = ""
        cbo_Output_IGST_Acc.Text = ""

        cbo_Reverse_CGST_Acc.Text = ""
        cbo_Reverse_SGST_Acc.Text = ""
        cbo_Reverse_IGST_Acc.Text = ""

        cbo_Payable_CGST_Acc.Text = ""
        cbo_Payable_SGST_Acc.Text = ""
        cbo_Payable_IGST_Acc.Text = ""

        cbo_Find.Text = ""


    End Sub

    Private Sub ControlGotFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim txtbx As TextBox
        Dim combobx As ComboBox
        Dim msktxbx As MaskedTextBox
        On Error Resume Next

        If TypeOf Me.ActiveControl Is TextBox Or TypeOf Me.ActiveControl Is ComboBox Or TypeOf Me.ActiveControl Is MaskedTextBox Then
            Me.ActiveControl.BackColor = Color.PaleGreen
            Me.ActiveControl.ForeColor = Color.Blue
        End If

        If TypeOf Me.ActiveControl Is TextBox Then
            txtbx = Me.ActiveControl
            txtbx.SelectAll()
        ElseIf TypeOf Me.ActiveControl Is ComboBox Then
            combobx = Me.ActiveControl
            combobx.SelectAll()
        ElseIf TypeOf Me.ActiveControl Is MaskedTextBox Then
            msktxbx = Me.ActiveControl
            msktxbx.SelectionStart = 0
        End If

        Prec_ActCtrl = Me.ActiveControl

    End Sub

    Private Sub ControlLostFocus(ByVal sender As Object, ByVal e As System.EventArgs)

        On Error Resume Next

        If IsNothing(Prec_ActCtrl) = False Then
            If TypeOf Prec_ActCtrl Is TextBox Or TypeOf Prec_ActCtrl Is ComboBox Or TypeOf Prec_ActCtrl Is MaskedTextBox Then
                Prec_ActCtrl.BackColor = Color.White
                Prec_ActCtrl.ForeColor = Color.Black
            End If
        End If

    End Sub

    Private Sub TextBox_ControlKeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs)
        If e.keycode = 38 Then e.Handled = True : SendKeys.Send("+{TAB}")
        If e.keycode = 40 Then e.Handled = True : SendKeys.Send("{TAB}")
    End Sub

    Private Sub TextBox_ControlKeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
        If Asc(e.KeyChar) = 13 Then e.Handled = True : SendKeys.Send("{TAB}")
    End Sub

    Private Sub GST_Account_Settings_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        con.Dispose()
        con.Close()
    End Sub

    Private Sub GST_Account_Settings_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If Asc(e.KeyChar) = 27 Then
            e.Handled = True
            If pnl_Open.Visible = True Then
                btn_OpenClose_Click(sender, e)
                Exit Sub
            ElseIf MessageBox.Show("Do you want to Close?..", "FOR CLOSE", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                Me.Close()
            Else
                Exit Sub
            End If
        End If
    End Sub

    Private Sub GST_Account_Settings_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        con.Open()

        pnl_Open.Visible = False
        pnl_Open.Top = (pnl_Back.Height - pnl_Open.Height) \ 2
        pnl_Open.Left = (pnl_Back.Width - pnl_Open.Width) \ 2
        pnl_Open.BringToFront()


        AddHandler txt_GST_Percentage.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Input_CGST_Acc.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Input_SGST_Acc.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Input_IGST_Acc.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_output_CGST_Acc.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Output_SGST_Acc.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Output_IGST_Acc.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Reverse_CGST_Acc.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Reverse_SGST_Acc.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Reverse_IGST_Acc.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Payable_CGST_Acc.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Payable_SGST_Acc.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Payable_IGST_Acc.GotFocus, AddressOf ControlGotFocus

        AddHandler cbo_Find.GotFocus, AddressOf ControlGotFocus

        AddHandler txt_GST_Percentage.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Input_CGST_Acc.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Input_SGST_Acc.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Input_IGST_Acc.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_output_CGST_Acc.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Output_SGST_Acc.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Output_IGST_Acc.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Reverse_CGST_Acc.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Reverse_SGST_Acc.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Reverse_IGST_Acc.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Payable_CGST_Acc.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Payable_SGST_Acc.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Payable_IGST_Acc.LostFocus, AddressOf ControlLostFocus

        AddHandler cbo_Find.LostFocus, AddressOf ControlLostFocus

        AddHandler txt_GST_Percentage.KeyDown, AddressOf TextBox_ControlKeyDown
        'AddHandler cbo_Payable_IGST_Acc.KeyDown, AddressOf TextBox_ControlKeyDown
        AddHandler txt_GST_Percentage.KeyPress, AddressOf TextBox_ControlKeyPress

        FrmLdSTS = True
        new_record()
    End Sub

    Public Sub delete_record() Implements Interface_MDIActions.delete_record
        Dim cmd As New SqlClient.SqlCommand
        Dim tr As SqlClient.SqlTransaction


        If MessageBox.Show("Do you want to Delete?...", "FOR DELETE", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) <> Windows.Forms.DialogResult.Yes Then
            Exit Sub
            cbo_Input_CGST_Acc.Focus()
        End If
        If pnl_Back.Enabled = False Then
            MessageBox.Show("Close Other Windows!.....", "COULD NOT DELETE", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
        End If
        If New_Entry = True Then
            MessageBox.Show("This is New Entry", "COULD NOT DELETE", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
            cbo_Input_CGST_Acc.Focus()
        End If

        tr = con.BeginTransaction

        Try

            cmd.Connection = con
            cmd.Transaction = tr

            If New_Entry = False Then

                cmd.CommandText = "DELETE FROM GST_AccountSettings_Head WHERE GST_Settings_IdNo = " & Val(lbl_RefNO.Text) & ""
                cmd.ExecuteNonQuery()

            End If
            tr.Commit()

            MessageBox.Show("Deleted Successfully", "FOR DELETE", MessageBoxButtons.OKCancel, MessageBoxIcon.Information)

            new_record()

        Catch ex As Exception
            tr.Rollback()
            MessageBox.Show(ex.Message, "DOES NOT DELETE..", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If cbo_Input_CGST_Acc.Enabled And cbo_Input_CGST_Acc.Visible Then cbo_Input_CGST_Acc.Focus()
            Exit Sub
        End Try
    End Sub

    Public Sub filter_record() Implements Interface_MDIActions.filter_record
        '------
    End Sub

    Public Sub insert_record() Implements Interface_MDIActions.insert_record
        '---------
    End Sub

    Private Sub move_record(ByVal IdNo As String)
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable

        clear()

        If Val(IdNo) = 0 Then Exit Sub

        Try
            da = New SqlClient.SqlDataAdapter("SELECT * FROM GST_AccountSettings_Head where GST_Settings_IdNo =" & Val(IdNo) & "", con)
            dt = New DataTable
            da.Fill(dt)

            If dt.Rows.Count > 0 Then

                lbl_RefNO.Text = dt.Rows(0).Item("GST_Settings_IdNo").ToString
                txt_GST_Percentage.Text = dt.Rows(0).Item("GST_Percentage").ToString

                cbo_Input_CGST_Acc.Text = Common_Procedures.Ledger_IdNoToName(con, dt.Rows(0).Item("IP_CGST_Ac_IdNo").ToString)
                cbo_Input_SGST_Acc.Text = Common_Procedures.Ledger_IdNoToName(con, dt.Rows(0).Item("IP_SGST_Ac_IdNo").ToString)
                cbo_Input_IGST_Acc.Text = Common_Procedures.Ledger_IdNoToName(con, dt.Rows(0).Item("IP_IGST_Ac_IdNo").ToString)
                cbo_output_CGST_Acc.Text = Common_Procedures.Ledger_IdNoToName(con, dt.Rows(0).Item("OP_CGST_Ac_IdNo").ToString)
                cbo_Output_SGST_Acc.Text = Common_Procedures.Ledger_IdNoToName(con, dt.Rows(0).Item("OP_SGST_Ac_IdNo").ToString)
                cbo_Output_IGST_Acc.Text = Common_Procedures.Ledger_IdNoToName(con, dt.Rows(0).Item("OP_IGST_Ac_IdNo").ToString)
                cbo_Reverse_CGST_Acc.Text = Common_Procedures.Ledger_IdNoToName(con, dt.Rows(0).Item("RC_CGST_Ac_IdNo").ToString)
                cbo_Reverse_SGST_Acc.Text = Common_Procedures.Ledger_IdNoToName(con, dt.Rows(0).Item("RC_SGST_Ac_IdNo").ToString)
                cbo_Reverse_IGST_Acc.Text = Common_Procedures.Ledger_IdNoToName(con, dt.Rows(0).Item("RC_IGST_Ac_IdNo").ToString)
                cbo_Payable_CGST_Acc.Text = Common_Procedures.Ledger_IdNoToName(con, dt.Rows(0).Item("PA_CGST_Ac_IdNo").ToString)
                cbo_Payable_SGST_Acc.Text = Common_Procedures.Ledger_IdNoToName(con, dt.Rows(0).Item("PA_SGST_Ac_IdNo").ToString)
                cbo_Payable_IGST_Acc.Text = Common_Procedures.Ledger_IdNoToName(con, dt.Rows(0).Item("PA_IGST_Ac_IdNo").ToString)

            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT MOVE,....", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Error)
            Exit Sub
        End Try

        If cbo_Input_CGST_Acc.Enabled And cbo_Input_CGST_Acc.Visible Then cbo_Input_CGST_Acc.Focus()

    End Sub

    Public Sub movefirst_record() Implements Interface_MDIActions.movefirst_record
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim move As String = ""


        Try
            Da = New SqlClient.SqlDataAdapter("SELECT TOP 1(GST_Settings_IdNo) FROM GST_AccountSettings_Head WHERE GST_Settings_IdNo <> 0", con)
            Dt = New DataTable
            Da.Fill(Dt)

            If Dt.Rows.Count > 0 Then
                If IsDBNull(Dt.Rows(0)(0).ToString) = False Then
                    move = Val(Dt.Rows(0)(0).ToString)
                End If
            End If

            If Trim(move) <> "" Then
                move_record(move)
            Else
                new_record()
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT MOVE", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
        End Try
    End Sub

    Public Sub movelast_record() Implements Interface_MDIActions.movelast_record
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim move As String = ""


        Try
            Da = New SqlClient.SqlDataAdapter("SELECT TOP 1(GST_Settings_IdNo) FROM GST_AccountSettings_Head WHERE GST_Settings_IdNo <> 0 ORDER BY GST_Settings_IdNo DESC", con)
            Dt = New DataTable
            Da.Fill(Dt)

            If Dt.Rows.Count > 0 Then
                If IsDBNull(Dt.Rows(0)(0).ToString) = False Then
                    move = Val(Dt.Rows(0)(0).ToString)
                End If
            End If

            If Trim(move) <> "" Then
                move_record(move)
            Else
                new_record()
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT MOVE", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
        End Try
    End Sub

    Public Sub movenext_record() Implements Interface_MDIActions.movenext_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim move As String = ""

        Try
            da = New SqlClient.SqlDataAdapter("SELECT TOP 1(GST_Settings_IdNo) FROM GST_AccountSettings_Head WHERE GST_Settings_IdNo > " & Val(lbl_RefNO.Text) & " AND GST_Settings_IdNo <> 0 ORDER BY GST_Settings_IdNo", con)
            dt = New DataTable
            da.Fill(dt)

            If dt.Rows.Count > 0 Then
                If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                    move = Trim(dt.Rows(0)(0).ToString)
                End If
            End If

            If Trim(move) <> "" Then move_record(move)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOESN'T MOVE", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
        End Try
    End Sub

    Public Sub moveprevious_record() Implements Interface_MDIActions.moveprevious_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim move As String = ""

        Try
            da = New SqlClient.SqlDataAdapter("SELECT TOP 1(GST_Settings_IdNo) FROM GST_AccountSettings_Head WHERE GST_Settings_IdNo < " & Val(lbl_RefNO.Text) & " AND GST_Settings_IdNo <> 0 ORDER BY GST_Settings_IdNo DESC", con)
            dt = New DataTable
            da.Fill(dt)

            If dt.Rows.Count > 0 Then
                If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                    move = Trim(dt.Rows(0)(0).ToString)
                End If
            End If

            If Trim(move) <> "" Then move_record(move)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOESN'T MOVE", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
        End Try
    End Sub

    Public Sub new_record() Implements Interface_MDIActions.new_record
        clear()
        New_Entry = True

        lbl_RefNO.Text = Common_Procedures.get_MaxIdNo(con, "GST_AccountSettings_Head", "GST_Settings_IdNo", "")
        If Val(lbl_RefNO.Text) <= 100 Then lbl_RefNO.Text = 101
        lbl_RefNO.ForeColor = Color.Red

        txt_GST_Percentage.Focus()
    End Sub

    Public Sub open_record() Implements Interface_MDIActions.open_record
        pnl_Back.Enabled = False
        pnl_Open.Visible = True
        cbo_Find.Focus()
    End Sub

    Public Sub print_record() Implements Interface_MDIActions.print_record
        '--------
    End Sub

    Public Sub save_record() Implements Interface_MDIActions.save_record


        Dim cmd As New SqlClient.SqlCommand
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim auto_slno As Integer = 0
        Dim tr As SqlClient.SqlTransaction


        Dim CGST_Input_LedID As Integer = 0
        Dim SGST_Input_LedID As Integer = 0
        Dim IGST_Input_LedID As Integer = 0

        Dim CGST_Output_LedID As Integer = 0
        Dim SGST_Output_LedID As Integer = 0
        Dim IGST_Output_LedID As Integer = 0

        Dim CGST_Reverse_LedID As Integer = 0
        Dim SGST_Reverse_LedID As Integer = 0
        Dim IGST_Reverse_LedID As Integer = 0

        Dim CGST_Payable_LedID As Integer = 0
        Dim SGST_Payable_LedID As Integer = 0
        Dim IGST_Payable_LedID As Integer = 0

        Dim Nr As Integer = 0


        If pnl_Back.Enabled = False Then
            MessageBox.Show("Close Other Windows!..", "DOES NOT SAVE", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
        End If
        If Val(lbl_RefNO.Text) = 0 Then
            MessageBox.Show("Invalid Ref No..", "DOES NOT SAVE", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
        End If

        CGST_Input_LedID = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Input_CGST_Acc.Text)
        If CGST_Input_LedID = 0 Then
            MessageBox.Show("Invalid CGST Input A/c", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If cbo_Input_CGST_Acc.Enabled And cbo_Input_CGST_Acc.Visible Then cbo_Input_CGST_Acc.Focus()
            Exit Sub
        End If

        SGST_Input_LedID = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Input_SGST_Acc.Text)
        If SGST_Input_LedID = 0 Then
            MessageBox.Show("Invalid SGST Input A/c", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If cbo_Input_SGST_Acc.Enabled And cbo_Input_SGST_Acc.Visible Then cbo_Input_SGST_Acc.Focus()
            Exit Sub
        End If

        IGST_Input_LedID = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Input_IGST_Acc.Text)
        If IGST_Input_LedID = 0 Then
            MessageBox.Show("Invalid IGST Input A/c", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If cbo_Input_IGST_Acc.Enabled And cbo_Input_IGST_Acc.Visible Then cbo_Input_IGST_Acc.Focus()
            Exit Sub
        End If


        CGST_Output_LedID = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_output_CGST_Acc.Text)
        If CGST_Output_LedID = 0 Then
            MessageBox.Show("Invalid CGST Output A/c", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If cbo_output_CGST_Acc.Enabled And cbo_output_CGST_Acc.Visible Then cbo_output_CGST_Acc.Focus()
            Exit Sub
        End If

        SGST_Output_LedID = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Output_SGST_Acc.Text)
        If SGST_Output_LedID = 0 Then
            MessageBox.Show("Invalid SGST Output A/c", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If cbo_Output_SGST_Acc.Enabled And cbo_Output_SGST_Acc.Visible Then cbo_Output_SGST_Acc.Focus()
            Exit Sub
        End If

        IGST_Output_LedID = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Output_IGST_Acc.Text)
        If IGST_Output_LedID = 0 Then
            MessageBox.Show("Invalid IGST Output A/c", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If cbo_Output_IGST_Acc.Enabled And cbo_Output_IGST_Acc.Visible Then cbo_Output_IGST_Acc.Focus()
            Exit Sub
        End If

        CGST_Reverse_LedID = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Reverse_CGST_Acc.Text)
        SGST_Reverse_LedID = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Reverse_SGST_Acc.Text)
        IGST_Reverse_LedID = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Reverse_IGST_Acc.Text)

        CGST_Payable_LedID = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Payable_CGST_Acc.Text)
        SGST_Payable_LedID = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Payable_SGST_Acc.Text)
        IGST_Payable_LedID = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Payable_IGST_Acc.Text)


        tr = con.BeginTransaction

        Try

            cmd.Connection = con
            cmd.Transaction = tr


            If New_Entry = True Then

                If Val(lbl_RefNO.Text) <= 100 Then lbl_RefNO.Text = 101

                cmd.CommandText = "INSERT INTO GST_AccountSettings_Head (      GST_Settings_IdNo    ,             GST_Percentage          ,              IP_CGST_Ac_IdNo     ,         IP_SGST_Ac_IdNo          ,         IP_IGST_Ac_IdNo          ,         OP_CGST_Ac_IdNo           ,         OP_SGST_Ac_IdNo           ,         OP_IGST_Ac_IdNo           ,         RC_CGST_Ac_IdNo            ,         RC_SGST_Ac_IdNo            ,         RC_IGST_Ac_IdNo            ,         PA_CGST_Ac_IdNo            ,         PA_SGST_Ac_IdNo            ,         PA_IGST_Ac_IdNo            )" & _
                                  "VALUES                               (" & Val(lbl_RefNO.Text) & "," & Trim(txt_GST_Percentage.Text) & "," & Str(Val(CGST_Input_LedID)) & "," & Str(Val(SGST_Input_LedID)) & "," & Str(Val(IGST_Input_LedID)) & "," & Str(Val(CGST_Output_LedID)) & "," & Str(Val(SGST_Output_LedID)) & "," & Str(Val(IGST_Output_LedID)) & "," & Str(Val(CGST_Reverse_LedID)) & "," & Str(Val(SGST_Reverse_LedID)) & "," & Str(Val(IGST_Reverse_LedID)) & "," & Str(Val(CGST_Payable_LedID)) & "," & Str(Val(SGST_Payable_LedID)) & "," & Str(Val(IGST_Payable_LedID)) & ")  "
                cmd.ExecuteNonQuery()

            Else

                cmd.CommandText = "UPDATE GST_AccountSettings_Head SET GST_Percentage = " & Trim(txt_GST_Percentage.Text) & ", IP_CGST_Ac_IdNo = " & Str(Val(CGST_Input_LedID)) & ", IP_SGST_Ac_IdNo = " & Str(Val(SGST_Input_LedID)) & ", IP_IGST_Ac_IdNo = " & Str(Val(IGST_Input_LedID)) & ", OP_CGST_Ac_IdNo = " & Str(Val(CGST_Output_LedID)) & ", OP_SGST_Ac_IdNo = " & Str(Val(SGST_Output_LedID)) & ", OP_IGST_Ac_IdNo = " & Str(Val(IGST_Output_LedID)) & ", RC_CGST_Ac_IdNo = " & Str(Val(CGST_Reverse_LedID)) & ", RC_SGST_Ac_IdNo = " & Str(Val(SGST_Reverse_LedID)) & ", RC_IGST_Ac_IdNo = " & Str(Val(IGST_Reverse_LedID)) & ", PA_CGST_Ac_IdNo = " & Str(Val(CGST_Payable_LedID)) & ", PA_SGST_Ac_IdNo = " & Str(Val(SGST_Payable_LedID)) & ", PA_IGST_Ac_IdNo = " & Str(Val(IGST_Payable_LedID)) & " WHERE GST_Settings_IdNo = " & Val(lbl_RefNO.Text) & ""
                cmd.ExecuteNonQuery()

            End If
            tr.Commit()

            MessageBox.Show("Saved Sucessfully!!", "FOR SAVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Information)

            If New_Entry = False Then
                move_record(lbl_RefNO.Text)
            Else
                new_record()
            End If

        Catch ex As Exception
            tr.Rollback()
            If InStr(1, Trim(LCase(ex.Message)), Trim(LCase("IX_GST_Settings_Head"))) > 0 Then
                MessageBox.Show("Duplicate GST %", "DOES NOT SAVE..", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Else
                MessageBox.Show(ex.Message, "DOES NOT SAVE..", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            End If

            Exit Sub
        End Try

        If cbo_Input_CGST_Acc.Enabled And cbo_Input_CGST_Acc.Visible Then cbo_Input_CGST_Acc.Focus()

    End Sub

    Private Sub cbo_Input_CGST_Acc_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Input_CGST_Acc.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "( AccountsGroup_IdNo = 12 )", "(Ledger_idno = 0)")
    End Sub

    Private Sub cbo_Input_CGST_Acc_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Input_CGST_Acc.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Input_CGST_Acc, txt_GST_Percentage, cbo_Input_SGST_Acc, "Ledger_AlaisHead", "Ledger_DisplayName", "( AccountsGroup_IdNo = 12 )", "(Ledger_idno = 0)")
    End Sub

    Private Sub cbo_Input_CGST_Acc_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Input_CGST_Acc.KeyUp
        Try
            If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then

                Common_Procedures.MDI_LedType = ""
                Dim f As New Ledger_Creation

                Common_Procedures.Master_Return.Form_Name = Me.Name
                Common_Procedures.Master_Return.Control_Name = cbo_Input_CGST_Acc.Name
                Common_Procedures.Master_Return.Return_Value = ""
                Common_Procedures.Master_Return.Master_Type = ""

                f.MdiParent = MDIParent1
                f.Show()
            End If

        Catch ex As Exception
            '--------
        End Try

    End Sub

    Private Sub cbo_Input_CGST_Acc_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Input_CGST_Acc.KeyPress
        Try
            Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Input_CGST_Acc, cbo_Input_SGST_Acc, "Ledger_AlaisHead", "Ledger_DisplayName", "( AccountsGroup_IdNo = 12 )", "(Ledger_idno = 0)")
        Catch ex As Exception
            '----
        End Try
    End Sub

    Private Sub cbo_Input_SGST_Acc_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Input_SGST_Acc.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "( AccountsGroup_IdNo = 12 )", "(Ledger_idno = 0)")
    End Sub

    Private Sub cbo_Input_SGST_Acc_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Input_SGST_Acc.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Input_SGST_Acc, cbo_Input_CGST_Acc, cbo_Input_IGST_Acc, "Ledger_AlaisHead", "Ledger_DisplayName", "( AccountsGroup_IdNo = 12 )", "(Ledger_idno = 0)")
    End Sub

    Private Sub cbo_Input_SGST_Acc_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Input_SGST_Acc.KeyPress
        Try
            Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Input_SGST_Acc, cbo_Input_IGST_Acc, "Ledger_AlaisHead", "Ledger_DisplayName", "( AccountsGroup_IdNo = 12 )", "(Ledger_idno = 0)")
        Catch ex As Exception
            '----
        End Try

    End Sub

    Private Sub cbo_Input_IGST_Acc_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Input_IGST_Acc.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "( AccountsGroup_IdNo = 12 )", "(Ledger_idno = 0)")
    End Sub

    Private Sub cbo_Input_IGST_Acc_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Input_IGST_Acc.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Input_IGST_Acc, cbo_Input_SGST_Acc, cbo_output_CGST_Acc, "Ledger_AlaisHead", "Ledger_DisplayName", "( AccountsGroup_IdNo = 12 )", "(Ledger_idno = 0)")
    End Sub

    Private Sub cbo_Input_IGST_Acc_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Input_IGST_Acc.KeyPress
        Try
            Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Input_IGST_Acc, cbo_output_CGST_Acc, "Ledger_AlaisHead", "Ledger_DisplayName", "( AccountsGroup_IdNo = 12 )", "(Ledger_idno = 0)")
        Catch ex As Exception
            '----
        End Try
    End Sub

    Private Sub cbo_output_CGST_Acc_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_output_CGST_Acc.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "( AccountsGroup_IdNo = 12 )", "(Ledger_idno = 0)")
    End Sub

    Private Sub cbo_output_CGST_Acc_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_output_CGST_Acc.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_output_CGST_Acc, cbo_Input_IGST_Acc, cbo_Output_SGST_Acc, "Ledger_AlaisHead", "Ledger_DisplayName", "( AccountsGroup_IdNo = 12 )", "(Ledger_idno = 0)")
    End Sub

    Private Sub cbo_output_CGST_Acc_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_output_CGST_Acc.KeyPress
        Try
            Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_output_CGST_Acc, cbo_Output_SGST_Acc, "Ledger_AlaisHead", "Ledger_DisplayName", "( AccountsGroup_IdNo = 12 )", "(Ledger_idno = 0)")
        Catch ex As Exception
            '----
        End Try

    End Sub

    Private Sub cbo_Output_SGST_Acc_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Output_SGST_Acc.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "( AccountsGroup_IdNo = 12 )", "(Ledger_idno = 0)")
    End Sub

    Private Sub cbo_Output_SGST_Acc_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Output_SGST_Acc.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Output_SGST_Acc, cbo_output_CGST_Acc, cbo_Output_IGST_Acc, "Ledger_AlaisHead", "Ledger_DisplayName", "( AccountsGroup_IdNo = 12 )", "(Ledger_idno = 0)")
    End Sub

    Private Sub cbo_Output_SGST_Acc_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Output_SGST_Acc.KeyPress
        Try
            Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Output_SGST_Acc, cbo_Output_IGST_Acc, "Ledger_AlaisHead", "Ledger_DisplayName", "( AccountsGroup_IdNo = 12 )", "(Ledger_idno = 0)")
        Catch ex As Exception
            '----
        End Try

    End Sub

    Private Sub cbo_Output_IGST_Acc_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Output_IGST_Acc.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "( AccountsGroup_IdNo = 12 )", "(Ledger_idno = 0)")
    End Sub

    Private Sub cbo_Output_IGST_Acc_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Output_IGST_Acc.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Output_IGST_Acc, cbo_Output_SGST_Acc, cbo_Reverse_CGST_Acc, "Ledger_AlaisHead", "Ledger_DisplayName", "( AccountsGroup_IdNo = 12 )", "(Ledger_idno = 0)")
    End Sub

    Private Sub cbo_Output_IGST_Acc_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Output_IGST_Acc.KeyPress
        Try
            Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Output_IGST_Acc, cbo_Reverse_CGST_Acc, "Ledger_AlaisHead", "Ledger_DisplayName", "( AccountsGroup_IdNo = 12 )", "(Ledger_idno = 0)")
        Catch ex As Exception
            '----
        End Try

    End Sub

    Private Sub cbo_Reverse_CGST_Acc_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Reverse_CGST_Acc.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "( AccountsGroup_IdNo = 12 )", "(Ledger_idno = 0)")
    End Sub

    Private Sub cbo_Reverse_CGST_Acc_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Reverse_CGST_Acc.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Reverse_CGST_Acc, cbo_Output_IGST_Acc, cbo_Reverse_SGST_Acc, "Ledger_AlaisHead", "Ledger_DisplayName", "( AccountsGroup_IdNo = 12 )", "(Ledger_idno = 0)")
    End Sub

    Private Sub cbo_Reverse_CGST_Acc_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Reverse_CGST_Acc.KeyPress
        Try
            Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Reverse_CGST_Acc, cbo_Reverse_SGST_Acc, "Ledger_AlaisHead", "Ledger_DisplayName", "( AccountsGroup_IdNo = 12 )", "(Ledger_idno = 0)")
        Catch ex As Exception
            '----
        End Try

    End Sub

    Private Sub cbo_Reverse_SGST_Acc_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Reverse_SGST_Acc.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "( AccountsGroup_IdNo = 12 )", "(Ledger_idno = 0)")
    End Sub

    Private Sub cbo_Reverse_SGST_Acc_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Reverse_SGST_Acc.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Reverse_SGST_Acc, cbo_Reverse_CGST_Acc, cbo_Reverse_IGST_Acc, "Ledger_AlaisHead", "Ledger_DisplayName", "( AccountsGroup_IdNo = 12 )", "(Ledger_idno = 0)")
    End Sub

    Private Sub cbo_Reverse_SGST_Acc_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Reverse_SGST_Acc.KeyPress
        Try
            Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Reverse_SGST_Acc, cbo_Reverse_IGST_Acc, "Ledger_AlaisHead", "Ledger_DisplayName", "( AccountsGroup_IdNo = 12 )", "(Ledger_idno = 0)")
        Catch ex As Exception
            '----
        End Try

    End Sub

    Private Sub cbo_Reverse_IGST_Acc_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Reverse_IGST_Acc.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "( AccountsGroup_IdNo = 12 )", "(Ledger_idno = 0)")
    End Sub

    Private Sub cbo_Reverse_IGST_Acc_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Reverse_IGST_Acc.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Reverse_IGST_Acc, cbo_Reverse_SGST_Acc, cbo_Payable_CGST_Acc, "Ledger_AlaisHead", "Ledger_DisplayName", "( AccountsGroup_IdNo = 12 )", "(Ledger_idno = 0)")
    End Sub

    Private Sub cbo_Reverse_IGST_Acc_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Reverse_IGST_Acc.KeyPress
        Try
            Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Reverse_IGST_Acc, cbo_Payable_CGST_Acc, "Ledger_AlaisHead", "Ledger_DisplayName", "( AccountsGroup_IdNo = 12 )", "(Ledger_idno = 0)")
        Catch ex As Exception
            '----
        End Try

    End Sub

    Private Sub cbo_Input_SGST_Acc_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Input_SGST_Acc.KeyUp
        Try
            If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then

                Common_Procedures.MDI_LedType = ""
                Dim f As New Ledger_Creation

                Common_Procedures.Master_Return.Form_Name = Me.Name
                Common_Procedures.Master_Return.Control_Name = cbo_Input_CGST_Acc.Name
                Common_Procedures.Master_Return.Return_Value = ""
                Common_Procedures.Master_Return.Master_Type = ""

                f.MdiParent = MDIParent1
                f.Show()
            End If

        Catch ex As Exception
            '--------
        End Try
    End Sub

    Private Sub cbo_Input_IGST_Acc_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Input_IGST_Acc.KeyUp
        Try
            If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then

                Common_Procedures.MDI_LedType = ""
                Dim f As New Ledger_Creation

                Common_Procedures.Master_Return.Form_Name = Me.Name
                Common_Procedures.Master_Return.Control_Name = cbo_Input_CGST_Acc.Name
                Common_Procedures.Master_Return.Return_Value = ""
                Common_Procedures.Master_Return.Master_Type = ""

                f.MdiParent = MDIParent1
                f.Show()
            End If

        Catch ex As Exception
            '--------
        End Try
    End Sub

    Private Sub cbo_Output_IGST_Acc_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Output_IGST_Acc.KeyUp
        Try
            If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then

                Common_Procedures.MDI_LedType = ""
                Dim f As New Ledger_Creation

                Common_Procedures.Master_Return.Form_Name = Me.Name
                Common_Procedures.Master_Return.Control_Name = cbo_Input_CGST_Acc.Name
                Common_Procedures.Master_Return.Return_Value = ""
                Common_Procedures.Master_Return.Master_Type = ""

                f.MdiParent = MDIParent1
                f.Show()
            End If

        Catch ex As Exception
            '--------
        End Try
    End Sub

    Private Sub cbo_output_CGST_Acc_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_output_CGST_Acc.KeyUp
        Try
            If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then

                Common_Procedures.MDI_LedType = ""
                Dim f As New Ledger_Creation

                Common_Procedures.Master_Return.Form_Name = Me.Name
                Common_Procedures.Master_Return.Control_Name = cbo_Input_CGST_Acc.Name
                Common_Procedures.Master_Return.Return_Value = ""
                Common_Procedures.Master_Return.Master_Type = ""

                f.MdiParent = MDIParent1
                f.Show()
            End If

        Catch ex As Exception
            '--------
        End Try
    End Sub

    Private Sub cbo_Output_SGST_Acc_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Output_SGST_Acc.KeyUp
        Try
            If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then

                Common_Procedures.MDI_LedType = ""
                Dim f As New Ledger_Creation

                Common_Procedures.Master_Return.Form_Name = Me.Name
                Common_Procedures.Master_Return.Control_Name = cbo_Input_CGST_Acc.Name
                Common_Procedures.Master_Return.Return_Value = ""
                Common_Procedures.Master_Return.Master_Type = ""

                f.MdiParent = MDIParent1
                f.Show()
            End If

        Catch ex As Exception
            '--------
        End Try
    End Sub

    Private Sub cbo_Reverse_CGST_Acc_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Reverse_CGST_Acc.KeyUp
        Try
            If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then

                Common_Procedures.MDI_LedType = ""
                Dim f As New Ledger_Creation

                Common_Procedures.Master_Return.Form_Name = Me.Name
                Common_Procedures.Master_Return.Control_Name = cbo_Input_CGST_Acc.Name
                Common_Procedures.Master_Return.Return_Value = ""
                Common_Procedures.Master_Return.Master_Type = ""

                f.MdiParent = MDIParent1
                f.Show()
            End If

        Catch ex As Exception
            '--------
        End Try
    End Sub

    Private Sub cbo_Reverse_IGST_Acc_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Reverse_IGST_Acc.KeyUp
        Try
            If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then

                Common_Procedures.MDI_LedType = ""
                Dim f As New Ledger_Creation

                Common_Procedures.Master_Return.Form_Name = Me.Name
                Common_Procedures.Master_Return.Control_Name = cbo_Input_CGST_Acc.Name
                Common_Procedures.Master_Return.Return_Value = ""
                Common_Procedures.Master_Return.Master_Type = ""

                f.MdiParent = MDIParent1
                f.Show()
            End If

        Catch ex As Exception
            '--------
        End Try
    End Sub

    Private Sub cbo_Reverse_SGST_Acc_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Reverse_SGST_Acc.KeyUp
        Try
            If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then

                Common_Procedures.MDI_LedType = ""
                Dim f As New Ledger_Creation

                Common_Procedures.Master_Return.Form_Name = Me.Name
                Common_Procedures.Master_Return.Control_Name = cbo_Input_CGST_Acc.Name
                Common_Procedures.Master_Return.Return_Value = ""
                Common_Procedures.Master_Return.Master_Type = ""

                f.MdiParent = MDIParent1
                f.Show()
            End If

        Catch ex As Exception
            '--------
        End Try
    End Sub

    Private Sub cbo_Payable_CGST_Acc_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Payable_CGST_Acc.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "( AccountsGroup_IdNo = 12 )", "(Ledger_idno = 0)")
    End Sub

    Private Sub cbo_Payable_CGST_Acc_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Payable_CGST_Acc.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Payable_CGST_Acc, cbo_Reverse_IGST_Acc, cbo_Payable_SGST_Acc, "Ledger_AlaisHead", "Ledger_DisplayName", "( AccountsGroup_IdNo = 12 )", "(Ledger_idno = 0)")
    End Sub

    Private Sub cbo_Payable_CGST_Acc_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Payable_CGST_Acc.KeyPress
        Try
            Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Payable_CGST_Acc, cbo_Payable_SGST_Acc, "Ledger_AlaisHead", "Ledger_DisplayName", "( AccountsGroup_IdNo = 12 )", "(Ledger_idno = 0)")
        Catch ex As Exception
            '----
        End Try
    End Sub

    Private Sub cbo_Payable_CGST_Acc_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Payable_CGST_Acc.KeyUp
        Try
            If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then

                Common_Procedures.MDI_LedType = ""
                Dim f As New Ledger_Creation

                Common_Procedures.Master_Return.Form_Name = Me.Name
                Common_Procedures.Master_Return.Control_Name = cbo_Payable_CGST_Acc.Name
                Common_Procedures.Master_Return.Return_Value = ""
                Common_Procedures.Master_Return.Master_Type = ""

                f.MdiParent = MDIParent1
                f.Show()
            End If

        Catch ex As Exception
            '--------
        End Try
    End Sub

    Private Sub cbo_Payable_SGST_Acc_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Payable_SGST_Acc.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "( AccountsGroup_IdNo = 12 )", "(Ledger_idno = 0)")
    End Sub

    Private Sub cbo_Payable_SGST_Acc_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Payable_SGST_Acc.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Payable_SGST_Acc, cbo_Payable_CGST_Acc, cbo_Payable_IGST_Acc, "Ledger_AlaisHead", "Ledger_DisplayName", "( AccountsGroup_IdNo = 12 )", "(Ledger_idno = 0)")
    End Sub

    Private Sub cbo_Payable_SGST_Acc_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Payable_SGST_Acc.KeyPress
        Try
            Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Payable_SGST_Acc, cbo_Payable_IGST_Acc, "Ledger_AlaisHead", "Ledger_DisplayName", "( AccountsGroup_IdNo = 12 )", "(Ledger_idno = 0)")
        Catch ex As Exception
            '----
        End Try
    End Sub

    Private Sub cbo_Payable_SGST_Acc_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Payable_SGST_Acc.KeyUp
        Try
            If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then

                Common_Procedures.MDI_LedType = ""
                Dim f As New Ledger_Creation

                Common_Procedures.Master_Return.Form_Name = Me.Name
                Common_Procedures.Master_Return.Control_Name = cbo_Payable_SGST_Acc.Name
                Common_Procedures.Master_Return.Return_Value = ""
                Common_Procedures.Master_Return.Master_Type = ""

                f.MdiParent = MDIParent1
                f.Show()
            End If

        Catch ex As Exception
            '--------
        End Try
    End Sub

    Private Sub cbo_Payable_IGST_Acc_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Payable_IGST_Acc.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "( AccountsGroup_IdNo = 12 )", "(Ledger_idno = 0)")
    End Sub

    Private Sub cbo_Payable_IGST_Acc_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Payable_IGST_Acc.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Payable_IGST_Acc, cbo_Payable_SGST_Acc, Nothing, "Ledger_AlaisHead", "Ledger_DisplayName", "( AccountsGroup_IdNo = 12 )", "(Ledger_idno = 0)")
        If e.KeyCode = 40 And cbo_Payable_IGST_Acc.DroppedDown = False Or e.Control = True And e.KeyCode = 40 Then
            If MessageBox.Show("Do you want to Save?..", "FOR SAVE", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Information) = Windows.Forms.DialogResult.Yes Then
                save_record()
            Else
                cbo_Input_CGST_Acc.Focus()
            End If
        End If
    End Sub

    Private Sub cbo_Payable_IGST_Acc_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Payable_IGST_Acc.KeyPress
        Try
            Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Payable_IGST_Acc, Nothing, "Ledger_AlaisHead", "Ledger_DisplayName", "( AccountsGroup_IdNo = 12 )", "(Ledger_idno = 0)")
            If Asc(e.KeyChar) = 13 Then
                If MessageBox.Show("Do you want to Save?..", "FOR SAVE", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Information) = Windows.Forms.DialogResult.Yes Then
                    save_record()
                Else
                    cbo_Input_CGST_Acc.Focus()
                End If
            End If
        Catch ex As Exception
            '----
        End Try
    End Sub

    Private Sub cbo_Payable_IGST_Acc_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Payable_IGST_Acc.KeyUp
        Try
            If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then

                Common_Procedures.MDI_LedType = ""
                Dim f As New Ledger_Creation

                Common_Procedures.Master_Return.Form_Name = Me.Name
                Common_Procedures.Master_Return.Control_Name = cbo_Payable_IGST_Acc.Name
                Common_Procedures.Master_Return.Return_Value = ""
                Common_Procedures.Master_Return.Master_Type = ""

                f.MdiParent = MDIParent1
                f.Show()
            End If

        Catch ex As Exception
            '--------
        End Try
    End Sub

    Private Sub btn_close_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_close.Click
        Me.Close()
    End Sub

    Private Sub btn_save_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_save.Click
        save_record()
    End Sub

    Private Sub btn_OpenClose_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_OpenClose.Click
        pnl_Back.Enabled = True
        pnl_Open.Visible = False
        cbo_Input_CGST_Acc.Focus()
    End Sub

    Private Sub cbo_Find_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Find.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "GST_AccountSettings_Head", "GST_Percentage", "", "(GST_Settings_IdNo = 0)")
    End Sub

    Private Sub cbo_Find_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Find.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Find, Nothing, Nothing, "GST_AccountSettings_Head", "GST_Percentage", "", "(GST_Settings_IdNo = 0)")
        If e.KeyCode = 40 And cbo_Find.DroppedDown = False Or e.Control = True And e.KeyCode = 40 Then
            btn_Open_Click(sender, e)
        End If
    End Sub

    Private Sub cbo_Find_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Find.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Find, Nothing, "GST_AccountSettings_Head", "GST_Percentage", "", "(GST_Settings_IdNo = 0)")
        If Asc(e.KeyChar) = 13 Then
            e.Handled = True
            btn_Open_Click(sender, e)
        End If
    End Sub

    Private Sub btn_Open_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Open.Click
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim move As String = ""

        Try
            da = New SqlClient.SqlDataAdapter("SELECT GST_Settings_IdNo FROM GST_AccountSettings_Head WHERE GST_Percentage =" & Str(Val(cbo_Find.Text)) & " ", con)
            dt = New DataTable
            da.Fill(dt)

            If dt.Rows.Count > 0 Then
                If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                    move = Trim(dt.Rows(0)(0).ToString)
                End If
            End If

            If Trim(move) <> "" Then move_record(move)
        Catch ex As Exception
            MessageBox.Show("Invalid Ref No", "DOES NOT OPEN", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
        End Try
        btn_OpenClose_Click(sender, e)
    End Sub
End Class