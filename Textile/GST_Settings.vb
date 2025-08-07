Public Class GST_Settings
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

    End Sub
    Public Sub delete_record() Implements Interface_MDIActions.delete_record
        Dim cmd As New SqlClient.SqlCommand
        Dim da As SqlClient.SqlDataAdapter
        Dim dt As DataTable
        Dim auto_slno As Integer = 0

        Try
            cmd.Connection = con

            da = New SqlClient.SqlDataAdapter("select top 1 Auto_SlNo from GST_Settings_Head", con)
            dt = New DataTable
            da.Fill(dt)

            If dt.Rows.Count > 0 Then
                auto_slno = Val(dt.Rows(0).Item("Auto_SlNo"))
                cmd.CommandText = "delete from GST_Settings_Head where Auto_SlNo = " & Str(Val(auto_slno))
                cmd.ExecuteNonQuery()
            End If

            MessageBox.Show("Deleted Sucessfully!!", "FOR DELETION...", MessageBoxButtons.OK, MessageBoxIcon.Information)
        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT DELETE..", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If cbo_Input_CGST_Acc.Enabled And cbo_Input_CGST_Acc.Visible Then cbo_Input_CGST_Acc.Focus()
            Exit Sub

        End Try
    End Sub

    Public Sub filter_record() Implements Interface_MDIActions.filter_record

    End Sub

    Public Sub insert_record() Implements Interface_MDIActions.insert_record

    End Sub
    Private Sub move_record(ByVal no As String)

    End Sub
    Public Sub movefirst_record() Implements Interface_MDIActions.movefirst_record

    End Sub

    Public Sub movelast_record() Implements Interface_MDIActions.movelast_record

    End Sub

    Public Sub movenext_record() Implements Interface_MDIActions.movenext_record

    End Sub

    Public Sub moveprevious_record() Implements Interface_MDIActions.moveprevious_record

    End Sub

    Public Sub new_record() Implements Interface_MDIActions.new_record

    End Sub

    Public Sub open_record() Implements Interface_MDIActions.open_record

    End Sub

    Public Sub print_record() Implements Interface_MDIActions.print_record

    End Sub

    Public Sub save_record() Implements Interface_MDIActions.save_record
        Dim cmd As New SqlClient.SqlCommand
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim auto_slno As Integer = 0

        Try

     
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
        CGST_Input_LedID = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Input_CGST_Acc.Text)
        If CGST_Input_LedID = 0 Then
            MessageBox.Show("Invalid Party Name", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If cbo_Input_CGST_Acc.Enabled And cbo_Input_CGST_Acc.Visible Then cbo_Input_CGST_Acc.Focus()
            Exit Sub
        End If
        SGST_Input_LedID = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Input_SGST_Acc.Text)
        IGST_Input_LedID = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Input_IGST_Acc.Text)

        CGST_Output_LedID = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_output_CGST_Acc.Text)
        SGST_Output_LedID = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Output_SGST_Acc.Text)
        IGST_Output_LedID = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Output_IGST_Acc.Text)

        CGST_Reverse_LedID = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Reverse_CGST_Acc.Text)
        SGST_Reverse_LedID = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Reverse_SGST_Acc.Text)
        IGST_Reverse_LedID = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Reverse_IGST_Acc.Text)

        CGST_Payable_LedID = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Payable_CGST_Acc.Text)
        SGST_Payable_LedID = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Payable_SGST_Acc.Text)
        IGST_Payable_LedID = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Payable_IGST_Acc.Text)


        cmd.Connection = con

        da = New SqlClient.SqlDataAdapter("select top 1 Auto_SlNo from GST_Settings_Head", con)
        da.Fill(dt)

        If dt.Rows.Count > 0 Then
            auto_slno = Val(dt.Rows(0).Item("Auto_SlNo"))
            cmd.CommandText = "Update GST_Settings_Head set CGST_Input_IdNo=" & Str(Val(CGST_Input_LedID)) & ",SGST_Input_IdNo=" & Str(Val(SGST_Input_LedID)) & ",IGST_Input_IdNo=" & Str(Val(IGST_Input_LedID)) & ",CGST_Output_IdNo=" & Str(Val(CGST_Output_LedID)) & ",SGST_Output_IdNo=" & Str(Val(SGST_Output_LedID)) & ",IGST_Output_IdNo=" & Str(Val(IGST_Output_LedID)) & ",CGST_Reverse_IdNo=" & Str(Val(CGST_Reverse_LedID)) & ",SGST_Reverse_IdNo=" & Str(Val(SGST_Reverse_LedID)) & ",IGST_Reverse_IdNo=" & Str(Val(IGST_Reverse_LedID)) & ",CGST_Payable_IdNo=" & Str(Val(CGST_Payable_LedID)) & ",SGST_Payable_IdNo=" & Str(Val(SGST_Payable_LedID)) & ",IGST_Payable_IdNo=" & Str(Val(IGST_Payable_LedID)) & "  where Auto_SlNo =" & auto_slno
            cmd.ExecuteNonQuery()
        Else
            cmd.CommandText = "Insert into GST_Settings_Head (      CGST_Input_IdNo         ,       SGST_Input_IdNo             ,       IGST_Input_IdNo             ,       CGST_Output_IdNo            ,       SGST_Output_IdNo            ,       IGST_Output_IdNo            ,       CGST_Reverse_IdNo           ,       SGST_Reverse_IdNo           ,       IGST_Reverse_IdNo           ,           CGST_Payable_IdNo           ,           SGST_Payable_IdNo       ,       IGST_Payable_IdNo           ) " & _
                                             " values (" & Str(Val(CGST_Input_LedID)) & "," & Str(Val(SGST_Input_LedID)) & "," & Str(Val(IGST_Input_LedID)) & "," & Str(Val(CGST_Output_LedID)) & "," & Str(Val(SGST_Output_LedID)) & "," & Str(Val(IGST_Output_LedID)) & "," & Str(Val(CGST_Reverse_LedID)) & "," & Str(Val(SGST_Reverse_LedID)) & "," & Str(Val(IGST_Reverse_LedID)) & "," & Str(Val(CGST_Payable_LedID)) & "," & Str(Val(SGST_Payable_LedID)) & "," & Str(Val(IGST_Payable_LedID)) & ") "
            cmd.ExecuteNonQuery()
        End If

        'Nr = 0
        'cmd.CommandText = "Update GST_Settings_Head set CGST_Input_IdNo=" & Str(Val(CGST_Input_LedID)) & ",SGST_Input_IdNo=" & Str(Val(SGST_Input_LedID)) & ",IGST_Input_IdNo=" & Str(Val(IGST_Input_LedID)) & ",CGST_Output_IdNo=" & Str(Val(CGST_Output_LedID)) & ",SGST_Output_IdNo=" & Str(Val(SGST_Output_LedID)) & ",IGST_Output_IdNo=" & Str(Val(IGST_Output_LedID)) & ",CGST_Reverse_IdNo=" & Str(Val(CGST_Reverse_LedID)) & ",SGST_Reverse_IdNo=" & Str(Val(SGST_Reverse_LedID)) & ",IGST_Reverse_IdNo=" & Str(Val(IGST_Reverse_LedID)) & ",CGST_Payable_IdNo=" & Str(Val(CGST_Payable_LedID)) & ",SGST_Payable_IdNo=" & Str(Val(SGST_Payable_LedID)) & ",IGST_Payable_IdNo=" & Str(Val(IGST_Payable_LedID)) & "   "
        'Nr = cmd.ExecuteNonQuery()

        'If Nr = 0 Then
        '    cmd.CommandText = "Insert into GST_Settings_Head (      CGST_Input_IdNo         ,       SGST_Input_IdNo             ,       IGST_Input_IdNo             ,       CGST_Output_IdNo            ,       SGST_Output_IdNo            ,       IGST_Output_IdNo            ,       CGST_Reverse_IdNo           ,       SGST_Reverse_IdNo           ,       IGST_Reverse_IdNo           ,           CGST_Payable_IdNo           ,           SGST_Payable_IdNo       ,       IGST_Payable_IdNo           ) " & _
        '                                         " values (" & Str(Val(CGST_Input_LedID)) & "," & Str(Val(SGST_Input_LedID)) & "," & Str(Val(IGST_Input_LedID)) & "," & Str(Val(CGST_Output_LedID)) & "," & Str(Val(SGST_Output_LedID)) & "," & Str(Val(IGST_Output_LedID)) & "," & Str(Val(CGST_Reverse_LedID)) & "," & Str(Val(SGST_Reverse_LedID)) & "," & Str(Val(IGST_Reverse_LedID)) & "," & Str(Val(CGST_Payable_LedID)) & "," & Str(Val(SGST_Payable_LedID)) & "," & Str(Val(IGST_Payable_LedID)) & ") "
        '    cmd.ExecuteNonQuery()
            'End If

            MessageBox.Show("Saved Sucessfully!!", "FOR DELETION...", MessageBoxButtons.OK, MessageBoxIcon.Information)
        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT DELETE..", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If cbo_Input_CGST_Acc.Enabled And cbo_Input_CGST_Acc.Visible Then cbo_Input_CGST_Acc.Focus()
            Exit Sub

        End Try
    End Sub
    Private Sub ControlGotFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim txtbx As TextBox
        Dim combobx As ComboBox
        Dim msktxbx As MaskedTextBox
        On Error Resume Next

        If TypeOf Me.ActiveControl Is TextBox Or TypeOf Me.ActiveControl Is ComboBox Or TypeOf Me.ActiveControl Is MaskedTextBox Then
            Me.ActiveControl.BackColor = Color.Lime
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

    Private Sub GST_Settings_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        con.Dispose()
        con.Close()
    End Sub

    Private Sub GST_Settings_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
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

        FrmLdSTS = True

        con.Open()

    End Sub

    Private Sub cbo_Input_CGST_Acc_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Input_CGST_Acc.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "( AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14 )", "(Ledger_idno = 0)")
    End Sub

    Private Sub cbo_Input_CGST_Acc_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Input_CGST_Acc.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Input_CGST_Acc, Nothing, cbo_Input_SGST_Acc, "Ledger_AlaisHead", "Ledger_DisplayName", "( AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14 )", "(Ledger_idno = 0)")
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
            Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Input_CGST_Acc, cbo_Input_SGST_Acc, "Ledger_AlaisHead", "Ledger_DisplayName", "( AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14 )", "(Ledger_idno = 0)")
        Catch ex As Exception
            '----
        End Try
    End Sub

    Private Sub cbo_Input_SGST_Acc_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Input_SGST_Acc.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "( AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14 )", "(Ledger_idno = 0)")
    End Sub

    Private Sub cbo_Input_SGST_Acc_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Input_SGST_Acc.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Input_SGST_Acc, cbo_Input_CGST_Acc, cbo_Input_IGST_Acc, "Ledger_AlaisHead", "Ledger_DisplayName", "( AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14 )", "(Ledger_idno = 0)")
    End Sub

    Private Sub cbo_Input_SGST_Acc_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Input_SGST_Acc.KeyPress
        Try
            Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Input_SGST_Acc, cbo_Input_IGST_Acc, "Ledger_AlaisHead", "Ledger_DisplayName", "( AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14 )", "(Ledger_idno = 0)")
        Catch ex As Exception
            '----
        End Try

    End Sub
    Private Sub cbo_Input_IGST_Acc_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Input_IGST_Acc.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "( AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14 )", "(Ledger_idno = 0)")
    End Sub

    Private Sub cbo_Input_IGST_Acc_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Input_IGST_Acc.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Input_IGST_Acc, cbo_Input_SGST_Acc, cbo_output_CGST_Acc, "Ledger_AlaisHead", "Ledger_DisplayName", "( AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14 )", "(Ledger_idno = 0)")
    End Sub

    Private Sub cbo_Input_IGST_Acc_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Input_IGST_Acc.KeyPress
        Try
            Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Input_IGST_Acc, cbo_output_CGST_Acc, "Ledger_AlaisHead", "Ledger_DisplayName", "( AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14 )", "(Ledger_idno = 0)")
        Catch ex As Exception
            '----
        End Try
    End Sub
    Private Sub cbo_output_CGST_Acc_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_output_CGST_Acc.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "( AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14 )", "(Ledger_idno = 0)")
    End Sub

    Private Sub cbo_output_CGST_Acc_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_output_CGST_Acc.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_output_CGST_Acc, cbo_Input_SGST_Acc, cbo_Output_SGST_Acc, "Ledger_AlaisHead", "Ledger_DisplayName", "( AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14 )", "(Ledger_idno = 0)")
    End Sub

    Private Sub cbo_output_CGST_Acc_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_output_CGST_Acc.KeyPress
        Try
            Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_output_CGST_Acc, cbo_Output_SGST_Acc, "Ledger_AlaisHead", "Ledger_DisplayName", "( AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14 )", "(Ledger_idno = 0)")
        Catch ex As Exception
            '----
        End Try

    End Sub

 
    Private Sub cbo_Output_SGST_Acc_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Output_SGST_Acc.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "( AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14 )", "(Ledger_idno = 0)")
    End Sub

    Private Sub cbo_Output_SGST_Acc_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Output_SGST_Acc.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Output_SGST_Acc, cbo_output_CGST_Acc, cbo_Output_IGST_Acc, "Ledger_AlaisHead", "Ledger_DisplayName", "( AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14 )", "(Ledger_idno = 0)")
    End Sub

    Private Sub cbo_Output_SGST_Acc_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Output_SGST_Acc.KeyPress
        Try
            Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Output_SGST_Acc, cbo_Output_IGST_Acc, "Ledger_AlaisHead", "Ledger_DisplayName", "( AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14 )", "(Ledger_idno = 0)")
        Catch ex As Exception
            '----
        End Try

    End Sub

    Private Sub cbo_Output_IGST_Acc_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Output_IGST_Acc.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "( AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14 )", "(Ledger_idno = 0)")
    End Sub

    Private Sub cbo_Output_IGST_Acc_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Output_IGST_Acc.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Output_IGST_Acc, cbo_Output_SGST_Acc, cbo_Reverse_CGST_Acc, "Ledger_AlaisHead", "Ledger_DisplayName", "( AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14 )", "(Ledger_idno = 0)")
    End Sub

    Private Sub cbo_Output_IGST_Acc_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Output_IGST_Acc.KeyPress
        Try
            Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Output_IGST_Acc, cbo_Reverse_CGST_Acc, "Ledger_AlaisHead", "Ledger_DisplayName", "( AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14 )", "(Ledger_idno = 0)")
        Catch ex As Exception
            '----
        End Try

    End Sub

    Private Sub cbo_Reverse_CGST_Acc_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Reverse_CGST_Acc.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "( AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14 )", "(Ledger_idno = 0)")
    End Sub

    Private Sub cbo_Reverse_CGST_Acc_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Reverse_CGST_Acc.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Reverse_CGST_Acc, cbo_Output_IGST_Acc, cbo_Reverse_SGST_Acc, "Ledger_AlaisHead", "Ledger_DisplayName", "( AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14 )", "(Ledger_idno = 0)")
    End Sub

    Private Sub cbo_Reverse_CGST_Acc_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Reverse_CGST_Acc.KeyPress
        Try
            Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Reverse_CGST_Acc, cbo_Reverse_SGST_Acc, "Ledger_AlaisHead", "Ledger_DisplayName", "( AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14 )", "(Ledger_idno = 0)")
        Catch ex As Exception
            '----
        End Try

    End Sub

    Private Sub cbo_Reverse_SGST_Acc_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Reverse_SGST_Acc.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "( AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14 )", "(Ledger_idno = 0)")
    End Sub

    Private Sub cbo_Reverse_SGST_Acc_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Reverse_SGST_Acc.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Reverse_SGST_Acc, cbo_Reverse_CGST_Acc, cbo_Reverse_IGST_Acc, "Ledger_AlaisHead", "Ledger_DisplayName", "( AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14 )", "(Ledger_idno = 0)")
    End Sub

    Private Sub cbo_Reverse_SGST_Acc_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Reverse_SGST_Acc.KeyPress
        Try
            Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Reverse_SGST_Acc, cbo_Reverse_IGST_Acc, "Ledger_AlaisHead", "Ledger_DisplayName", "( AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14 )", "(Ledger_idno = 0)")
        Catch ex As Exception
            '----
        End Try

    End Sub
    Private Sub cbo_Reverse_IGST_Acc_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Reverse_IGST_Acc.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "( AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14 )", "(Ledger_idno = 0)")
    End Sub

    Private Sub cbo_Reverse_IGST_Acc_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Reverse_IGST_Acc.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Reverse_IGST_Acc, cbo_Reverse_SGST_Acc, cbo_Payable_CGST_Acc, "Ledger_AlaisHead", "Ledger_DisplayName", "( AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14 )", "(Ledger_idno = 0)")
    End Sub

    Private Sub cbo_Reverse_IGST_Acc_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Reverse_IGST_Acc.KeyPress
        Try
            Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Reverse_IGST_Acc, cbo_Payable_CGST_Acc, "Ledger_AlaisHead", "Ledger_DisplayName", "( AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14 )", "(Ledger_idno = 0)")
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
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "( AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14 )", "(Ledger_idno = 0)")
    End Sub

    Private Sub cbo_Payable_CGST_Acc_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Payable_CGST_Acc.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Payable_CGST_Acc, cbo_Reverse_IGST_Acc, cbo_Payable_SGST_Acc, "Ledger_AlaisHead", "Ledger_DisplayName", "( AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14 )", "(Ledger_idno = 0)")
    End Sub

    Private Sub cbo_Payable_CGST_Acc_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Payable_CGST_Acc.KeyPress
        Try
            Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Reverse_IGST_Acc, cbo_Payable_CGST_Acc, "Ledger_AlaisHead", "Ledger_DisplayName", "( AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14 )", "(Ledger_idno = 0)")
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
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "( AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14 )", "(Ledger_idno = 0)")
    End Sub

    Private Sub cbo_Payable_SGST_Acc_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Payable_SGST_Acc.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Payable_SGST_Acc, cbo_Payable_CGST_Acc, cbo_Payable_IGST_Acc, "Ledger_AlaisHead", "Ledger_DisplayName", "( AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14 )", "(Ledger_idno = 0)")
    End Sub

    Private Sub cbo_Payable_SGST_Acc_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Payable_SGST_Acc.KeyPress
        Try
            Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Payable_SGST_Acc, cbo_Payable_IGST_Acc, "Ledger_AlaisHead", "Ledger_DisplayName", "( AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14 )", "(Ledger_idno = 0)")
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
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "( AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14 )", "(Ledger_idno = 0)")
    End Sub

    Private Sub cbo_Payable_IGST_Acc_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Payable_IGST_Acc.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Payable_IGST_Acc, cbo_Payable_SGST_Acc, Nothing, "Ledger_AlaisHead", "Ledger_DisplayName", "( AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14 )", "(Ledger_idno = 0)")
    End Sub

    Private Sub cbo_Payable_IGST_Acc_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Payable_IGST_Acc.KeyPress
        Try
            Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Payable_IGST_Acc, Nothing, "Ledger_AlaisHead", "Ledger_DisplayName", "( AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14 )", "(Ledger_idno = 0)")
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
End Class