Public Class Ledger_Creation_AdditionalCharges
    Implements Interface_MDIActions

    Private Const vLEDTYPE As String = "ADDITINAL-CHARGES"
    Private con As New SqlClient.SqlConnection(Common_Procedures.Connection_String)
    Private New_Entry As Boolean = False
    Private Prec_ActCtrl As New Control
    Private vcbo_KeyDwnVal As Double
    Private OwmLoom_STS As Integer = 0
    Private Close_STS As Integer = 0
    Private Show_STS As Integer = 0
    Private Verified_STS As Integer = 0
    Private Stock_STS As Integer = 0
    Private TrnTo_DbName As String = ""
    Private SaveAll_STS As Boolean = False
    Private LastNo As String = ""
    Private FrmLdSTS As Boolean = False
    Private dgv_ActiveCtrl_Name As String = ""
    Private WithEvents dgtxt_KnittingDetails As New DataGridViewTextBoxEditingControl
    Private WithEvents dgtxt_FreihtChargeDetails As New DataGridViewTextBoxEditingControl
    Private WithEvents dgtxt_WeaverWagesDetails As New DataGridViewTextBoxEditingControl
    Private prn_HdDt As New DataTable
    Private WithEvents dgtxt_LoomDetails As New DataGridViewTextBoxEditingControl
    Private SizTo_DbName As String = ""

    Private prn_DetDt As New DataTable
    Private prn_DetIndx As Integer
    Private prn_DetSNo As Integer
    Private prn_PageNo As Integer
    Private prn_count As Integer = 0

    Public Sub New()
        FrmLdSTS = True
        ' This call is required by the designer.
        InitializeComponent()
        ' Add any initialization after the InitializeComponent() call.
    End Sub

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

        cbo_AcGroup.Text = Common_Procedures.AccountsGroup_IdNoToName(con, 19)

        lbl_IdNo.ForeColor = Color.Black
        grp_Back.Enabled = True
        grp_Open.Visible = False
        grp_Filter.Visible = False
        dgv_ActiveCtrl_Name = ""

    End Sub

    Private Sub ControlGotFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim txtbx As TextBox
        Dim combobx As ComboBox

        On Error Resume Next

        If TypeOf Me.ActiveControl Is TextBox Or TypeOf Me.ActiveControl Is ComboBox Then
            Me.ActiveControl.BackColor = Color.SpringGreen ' Color.MistyRose ' Color.lime
            Me.ActiveControl.ForeColor = Color.Blue
        End If

        If TypeOf Me.ActiveControl Is TextBox Then
            txtbx = Me.ActiveControl
            txtbx.SelectAll()

        ElseIf TypeOf Me.ActiveControl Is ComboBox Then
            combobx = Me.ActiveControl
            combobx.SelectAll()

        End If

        If Me.ActiveControl.Name <> dgv_Filter.Name Then
            Grid_Cell_DeSelect()
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

    Private Sub ControlLostFocus1(ByVal sender As Object, ByVal e As System.EventArgs)

        On Error Resume Next

        If IsNothing(Prec_ActCtrl) = False Then
            If TypeOf Prec_ActCtrl Is TextBox Or TypeOf Prec_ActCtrl Is ComboBox Then
                Prec_ActCtrl.BackColor = Color.LightBlue
                Prec_ActCtrl.ForeColor = Color.Blue
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

    Private Sub Grid_Cell_DeSelect()
        On Error Resume Next
        If Not IsNothing(dgv_Filter.CurrentCell) Then dgv_Filter.CurrentCell.Selected = False
    End Sub

    Public Sub move_record(ByVal idno As Integer)
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim da3 As New SqlClient.SqlDataAdapter
        Dim dt3 As New DataTable
        Dim da4 As New SqlClient.SqlDataAdapter
        Dim dt4 As New DataTable
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt2 As New DataTable
        Dim n As Integer = 0
        Dim SNo As Integer = 0

        If Val(idno) = 0 Then Exit Sub


        clear()


        da = New SqlClient.SqlDataAdapter("select a.*, b.AccountsGroup_Name, c.Area_Name   from Ledger_Head a  LEFT OUTER JOIN Area_Head c ON a.Area_IdNo = c.Area_IdNo , AccountsGroup_Head b where a.ledger_idno = " & Str(Val(idno)) & " and a.ledger_type = '" & Trim(vLedType) & "' and a.AccountsGroup_IdNo = b.AccountsGroup_IdNo ", con)
        da.Fill(dt)

        If dt.Rows.Count > 0 Then
            lbl_IdNo.Text = dt.Rows(0).Item("Ledger_IdNo").ToString
            txt_Name.Text = dt.Rows(0).Item("Ledger_MainName").ToString
            cbo_AcGroup.Text = dt.Rows(0)("AccountsGroup_Name").ToString
            txt_HSNSAC_Code.Text = dt.Rows(0).Item("HSN_SAC_Code").ToString
            txt_GSTPerc.Text = dt.Rows(0).Item("GST_Percentage").ToString

        End If
        dt.Clear()

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

        ' If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.Ledr_Wea_Siz_Rw_Trans_JbWrk_Creation, "~L~") = 0 And InStr(Common_Procedures.UR.Ledr_Wea_Siz_Rw_Trans_JbWrk_Creation, "~D~") = 0 Then MessageBox.Show("You have No Rights to Delete", "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error) : Exit Sub

        If MessageBox.Show("Do you want to delete", "FOR DELETION...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) <> Windows.Forms.DialogResult.Yes Then
            Exit Sub
        End If

        Try

            da = New SqlClient.SqlDataAdapter("select count(*) from Chit_Auction_Head where member_idno = " & Str(Val(lbl_IdNo.Text)), con)
            dt = New DataTable
            da.Fill(dt)
            If dt.Rows.Count > 0 Then
                If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                    If Val(dt.Rows(0)(0).ToString) > 0 Then
                        MessageBox.Show("Already used this Ledger", "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                        Exit Sub
                    End If
                End If
            End If

            da = New SqlClient.SqlDataAdapter("select count(*) from Chit_Auction_Details where member_idno = " & Str(Val(lbl_IdNo.Text)), con)
            dt = New DataTable
            da.Fill(dt)
            If dt.Rows.Count > 0 Then
                If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                    If Val(dt.Rows(0)(0).ToString) > 0 Then
                        MessageBox.Show("Already used this Ledger", "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                        Exit Sub
                    End If
                End If
            End If

            da = New SqlClient.SqlDataAdapter("select count(*) from Chit_Group_Details where member_idno = " & Str(Val(lbl_IdNo.Text)), con)
            dt = New DataTable
            da.Fill(dt)
            If dt.Rows.Count > 0 Then
                If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                    If Val(dt.Rows(0)(0).ToString) > 0 Then
                        MessageBox.Show("Already used this Ledger", "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                        Exit Sub
                    End If
                End If
            End If

            da = New SqlClient.SqlDataAdapter("select count(*) from Voucher_Details where Ledger_Idno = " & Str(Val(lbl_IdNo.Text)), con)
            dt = New DataTable
            da.Fill(dt)
            If dt.Rows.Count > 0 Then
                If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                    If Val(dt.Rows(0)(0).ToString) > 0 Then
                        MessageBox.Show("Already used this Ledger", "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
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


            new_record()

            MessageBox.Show("Deleted Sucessfully!!", "FOR DELETION...", MessageBoxButtons.OKCancel, MessageBoxIcon.Information)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT DELETE..", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If txt_Name.Enabled And txt_Name.Visible Then txt_Name.Focus()
            Exit Sub

        End Try

    End Sub

    Public Sub filter_record() Implements Interface_MDIActions.filter_record
        Dim da As New SqlClient.SqlDataAdapter("select ledger_idno, ledger_name from ledger_head where ledger_idno <> 0 and ledger_type = '" & Trim(vLedType) & "' order by ledger_idno", con)
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

        da.Dispose()

    End Sub

    Public Sub movefirst_record() Implements Interface_MDIActions.movefirst_record
        Dim cmd As New SqlClient.SqlCommand
        Dim movid As Integer

        Try
            cmd.Connection = con
            cmd.CommandText = "select min(ledger_idno) from ledger_head Where ledger_idno <> 0 and ledger_type = '" & Trim(vLedType) & "'"

            movid = 0
            If IsDBNull(cmd.ExecuteScalar()) = False Then
                movid = cmd.ExecuteScalar
            End If

            cmd.Dispose()

            If movid <> 0 Then Call move_record(movid)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR MOVING....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Public Sub movelast_record() Implements Interface_MDIActions.movelast_record
        Dim cmd As New SqlClient.SqlCommand
        Dim movid As Integer

        Try
            cmd.Connection = con
            cmd.CommandText = "select max(ledger_idno) from ledger_head where ledger_idno <> 0 and ledger_type = '" & Trim(vLedType) & "'"

            movid = 0
            If IsDBNull(cmd.ExecuteScalar()) = False Then
                movid = cmd.ExecuteScalar
            End If

            cmd.Dispose()

            If movid <> 0 Then move_record(movid)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR MOVING....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Public Sub movenext_record() Implements Interface_MDIActions.movenext_record
        Dim cmd As New SqlClient.SqlCommand("select min(ledger_idno) from ledger_head where ledger_idno <> 0 and ledger_idno > " & Str(Val(lbl_IdNo.Text)) & " and ledger_type = '" & Trim(vLedType) & "'", con)
        Dim movid As Integer = 0

        Try
            movid = 0
            If IsDBNull(cmd.ExecuteScalar()) = False Then
                movid = cmd.ExecuteScalar
            End If

            cmd.Dispose()

            If movid <> 0 Then move_record(movid)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Public Sub moveprevious_record() Implements Interface_MDIActions.moveprevious_record
        Dim cmd As New SqlClient.SqlCommand
        Dim movid As Integer = 0

        Try
            cmd.Connection = con
            cmd.CommandText = "select max(ledger_idno ) from ledger_head where ledger_idno <> 0 and ledger_idno < " & Str((lbl_IdNo.Text)) & " and ledger_type = '" & Trim(vLedType) & "'"

            movid = 0
            If IsDBNull(cmd.ExecuteScalar()) = False Then
                movid = cmd.ExecuteScalar()
            End If

            cmd.Dispose()

            If movid <> 0 Then move_record(movid)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR MOVING....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Public Sub open_record() Implements Interface_MDIActions.open_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable

        da = New SqlClient.SqlDataAdapter("select Ledger_DisplayName from Ledger_AlaisHead where (Ledger_IdNo = 0 or Ledger_type = '" & Trim(vLedType) & "') order by Ledger_DisplayName", con)
        da.Fill(dt)

        cbo_Open.DataSource = dt
        cbo_Open.DisplayMember = "Ledger_DisplayName"

        da.Dispose()

        grp_Open.Visible = True
        grp_Open.BringToFront()
        If cbo_Open.Enabled And cbo_Open.Visible Then cbo_Open.Focus()
        grp_Back.Enabled = False

    End Sub

    Public Sub insert_record() Implements Interface_MDIActions.insert_record
        'MessageBox.Show("insert record")
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
        Dim clth_ID As Integer = 0
        Dim LedArName As String = ""
        Dim LedPhNo As String = ""
        Dim Transtk_Id As Integer = 0
        Dim Grp_idno As Integer = 0
        Dim Sno As Integer = 0
        Dim undgrp_ParntCD As String = ""
        Dim LedAls_AcGrp_idno As Integer = 0
        Dim vState_ID As Integer = 0
        Dim Color_Id As Integer = 0
        Dim Slno As Integer = 0
        Dim SizCmpstk_Id As Integer = 0
        Dim SizVndrstk_Id As Integer = 0

        Dim FrmAddschk_Sts As Integer = 0
        Dim Cmp_Name As String = ""


        '  If Common_Procedures.UserRight_Check(Common_Procedures.UR.Ledr_Wea_Siz_Rw_Trans_JbWrk_Creation, New_Entry) = False Then Exit Sub


        If grp_Back.Enabled = False Then
            MessageBox.Show("Close other window", "DOES NOT SAVE", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
        End If

        If Trim(txt_Name.Text) = "" Then
            MessageBox.Show("Invalid Legder Name", "DOES NOT SAVE", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If txt_Name.Enabled Then txt_Name.Focus()
            Exit Sub
        End If

        acgrp_idno = Common_Procedures.AccountsGroup_NameToIdNo(con, cbo_AcGroup.Text)
        If acgrp_idno = 0 Then
            MessageBox.Show("Invalid Accounts Group", "DOES NOT SAVE", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If cbo_AcGroup.Enabled Then cbo_AcGroup.Focus()
            Exit Sub
        End If

        Parnt_CD = Common_Procedures.AccountsGroup_IdNoToCode(con, acgrp_idno)

        If Val(Grp_idno) = 0 Then
            Grp_idno = Val(lbl_IdNo.Text)
        End If


        LedName = Trim(txt_Name.Text)

        FrmAddschk_Sts = 0

        SurName = Common_Procedures.Remove_NonCharacters(LedName)
        trans = con.BeginTransaction

        Try

            cmd.Transaction = trans

            cmd.Connection = con

            If New_Entry = True Then
                lbl_IdNo.Text = Common_Procedures.get_MaxIdNo(con, "ledger_head", "ledger_idno", "", trans)
                If Val(lbl_IdNo.Text) < 101 Then lbl_IdNo.Text = 101

                cmd.CommandText = "Insert into ledger_head(        Ledger_Type     ,        Ledger_IdNo           ,       Ledger_Name      ,           Sur_Name     ,    Ledger_MainName           ,     AccountsGroup_IdNo        ,            Parent_Code  ,             HSN_SAC_Code             ,            GST_Percentage         )  " &
                                  "Values                 ('" & Trim(vLEDTYPE) & "', " & Str(Val(lbl_IdNo.Text)) & ", '" & Trim(LedName) & "', '" & Trim(SurName) & "', '" & Trim(txt_Name.Text) & "',   " & Str(Val(acgrp_idno)) & ", '" & Trim(Parnt_CD) & "', '" & Trim(txt_HSNSAC_Code.Text) & "', " & Str(Val(txt_GSTPerc.Text)) & " ) "
                cmd.ExecuteNonQuery()

            Else

                cmd.CommandText = "Update ledger_head set Ledger_Type = '" & Trim(vLEDTYPE) & "', Ledger_Name = '" & Trim(LedName) & "', Sur_Name = '" & Trim(SurName) & "', Ledger_MainName = '" & Trim(txt_Name.Text) & "', AccountsGroup_IdNo = " & Str(Val(acgrp_idno)) & " , Parent_Code = '" & Trim(Parnt_CD) & "' , HSN_SAC_Code = '" & Trim(txt_HSNSAC_Code.Text) & "', GST_Percentage = " & Str(Val(txt_GSTPerc.Text)) & "  Where Ledger_IdNo = " & Str(Val(lbl_IdNo.Text))
                cmd.ExecuteNonQuery()

            End If


            LedAls_AcGrp_idno = acgrp_idno
            undgrp_ParntCD = Trim(Parnt_CD)

            While LedAls_AcGrp_idno > 32
                undgrp_ParntCD = Replace(undgrp_ParntCD, "~" & Trim(Val(LedAls_AcGrp_idno)) & "~", "")

                undgrp_ParntCD = "~" & Trim(undgrp_ParntCD)

                LedAls_AcGrp_idno = Val(Common_Procedures.get_FieldValue(con, "AccountsGroup_Head", "AccountsGroup_IdNo", "(Parent_Idno = '" & Trim(undgrp_ParntCD) & "')", , trans))
            End While

            If acgrp_idno > 30 Then
                undgrp_ParntCD = Replace(Parnt_CD, "~" & Trim(Val(acgrp_idno)) & "~", "")

                undgrp_ParntCD = "~" & Trim(undgrp_ParntCD)

                LedAls_AcGrp_idno = Val(Common_Procedures.get_FieldValue(con, "AccountsGroup_Head", "AccountsGroup_IdNo", "(Parent_Idno = '" & Trim(undgrp_ParntCD) & "')", , trans))

            Else
                LedAls_AcGrp_idno = acgrp_idno

            End If

            cmd.CommandText = "delete from Ledger_AlaisHead where Ledger_IdNo = " & Str(Val(lbl_IdNo.Text))
            cmd.ExecuteNonQuery()


            LedArName = Trim(txt_Name.Text)


            cmd.CommandText = "Insert into Ledger_AlaisHead(Ledger_IdNo, Sl_No, Ledger_DisplayName, AccountsGroup_IdNo, Ledger_Type) Values (" & Str(Val(lbl_IdNo.Text)) & ", 1, '" & Trim(LedArName) & "', " & Str(Val(LedAls_AcGrp_idno)) & ", '" & Trim(vLEDTYPE) & "')"
            cmd.ExecuteNonQuery()


            trans.Commit()
            dt.Dispose()
            trans.Dispose()


            Common_Procedures.Master_Return.Return_Value = Trim(LedName)
            Common_Procedures.Master_Return.Master_Type = "LEDGER"


            If SaveAll_STS <> True Then
                MessageBox.Show("Saved Sucessfully!!!", "FOR SAVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Information)
            End If


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

            If InStr(1, Trim(LCase(ex.Message)), Trim(LCase("ix_ledger_head"))) > 0 Then
                MessageBox.Show("Duplicate Ledger Name", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Else
                MessageBox.Show(ex.Message, "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
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

        Common_Procedures.Master_Return.Master_Type = ""
        Common_Procedures.Master_Return.Return_Value = ""

        FrmLdSTS = False
    End Sub

    Private Sub Ledger_Creation_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        con.Open()

        FrmLdSTS = True

        grp_Open.Visible = False
        grp_Open.Left = (Me.Width - grp_Open.Width) \ 2
        grp_Open.Top = (Me.Height - grp_Open.Height) \ 2

        grp_Filter.Visible = False
        grp_Filter.Left = (Me.Width - grp_Filter.Width) \ 2
        grp_Filter.Top = (Me.Height - grp_Filter.Height) \ 2


        AddHandler txt_Name.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_AcGroup.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Open.GotFocus, AddressOf ControlGotFocus


        AddHandler btnSave.GotFocus, AddressOf ControlGotFocus
        AddHandler btnClose.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Open.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_HSNSAC_Code.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_GSTPerc.GotFocus, AddressOf ControlGotFocus

        AddHandler txt_Name.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_AcGroup.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_HSNSAC_Code.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_GSTPerc.LostFocus, AddressOf ControlLostFocus
        AddHandler btnSave.LostFocus, AddressOf ControlLostFocus
        AddHandler btnClose.LostFocus, AddressOf ControlLostFocus

        AddHandler txt_Name.KeyDown, AddressOf TextBoxControlKeyDown

        AddHandler txt_Name.KeyPress, AddressOf TextBoxControlKeyPress

        FrmLdSTS = True
        new_record()

    End Sub

    Private Sub Ledger_Creation_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        On Error Resume Next
        con.Close()
        con.Dispose()
        Common_Procedures.MDI_LedType = Me.Name
    End Sub

    Private Sub Ledger_Creation_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If Asc(e.KeyChar) = 27 Then
            If grp_Open.Visible Then
                btn_CloseOpen_Click(sender, e)

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

    Private Sub btn_CloseOpen_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_CloseOpen.Click
        grp_Back.Enabled = True
        grp_Open.Visible = False
        If txt_Name.Enabled And txt_Name.Visible Then txt_Name.Focus()
    End Sub

    Private Sub btn_Find_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Find.Click
        Dim movid As Integer

        movid = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Open.Text)

        If movid <> 0 Then move_record(movid)

        grp_Back.Enabled = True
        grp_Open.Visible = False

    End Sub

    Private Sub cbo_Open_GotFocus(sender As Object, e As EventArgs) Handles cbo_Open.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_Head", "Ledger_Name", "(Ledger_Type = '" & Trim(vLedType) & "')", "(ledger_idno = 0)")
    End Sub

    Private Sub cbo_Open_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Open.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Open, Nothing, Nothing, "Ledger_Head", "Ledger_Name", "(Ledger_Type = '" & Trim(vLedType) & "')", "(ledger_idno = 0)")
    End Sub

    Private Sub cbo_Open_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Open.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Open, Nothing, "Ledger_Head", "Ledger_Name", "(Ledger_Type = '" & Trim(vLedType) & "')", "(ledger_idno = 0)")

        If Asc(e.KeyChar) = 13 Then
            Call btn_Find_Click(sender, e)
        End If

    End Sub

    Private Sub btn_CloseFilter_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_CloseFilter.Click
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
        If Asc(e.KeyChar) = 39 Then
            e.Handled = True
        End If
    End Sub

    Private Sub cbo_AcGroup_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_AcGroup.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "AccountsGroup_Head", "AccountsGroup_Name", "", "(AccountsGroup_IdNo = 0)")
    End Sub

    Private Sub cbo_AcGroup_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_AcGroup.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_AcGroup, txt_Name, txt_HSNSAC_Code, "AccountsGroup_Head", "AccountsGroup_Name", "", "(AccountsGroup_IdNo = 0)")

    End Sub

    Private Sub cbo_AcGroup_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_AcGroup.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_AcGroup, txt_HSNSAC_Code, "AccountsGroup_Head", "AccountsGroup_Name", "", "(AccountsGroup_IdNo = 0)")
        'If Asc(e.KeyChar) = 13 Then
        '    If MessageBox.Show("Do you want to Save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
        '        save_record()

        '    Else
        '        txt_Name.Focus()

        '    End If

        'End If

    End Sub


    Public Sub print_record() Implements Interface_MDIActions.print_record
        'Printing_LedgerAddress_Print()
    End Sub


    Private Sub txt_HSNSAC_Code_KeyDown(sender As Object, e As KeyEventArgs) Handles txt_HSNSAC_Code.KeyDown
        On Error Resume Next
        If e.KeyValue = 38 Then e.Handled = True : e.SuppressKeyPress = True : SendKeys.Send("+{TAB}")
        If e.KeyValue = 40 Then e.Handled = True : e.SuppressKeyPress = True : SendKeys.Send("{TAB}")
    End Sub

    Private Sub txt_HSNSAC_Code_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txt_HSNSAC_Code.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
            e.Handled = True
        End If
        If Asc(e.KeyChar) = 13 Then
            e.Handled = True
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub txt_GSTPerc_KeyDown(sender As Object, e As KeyEventArgs) Handles txt_GSTPerc.KeyDown
        On Error Resume Next
        If e.KeyValue = 38 Then e.Handled = True : e.SuppressKeyPress = True : SendKeys.Send("+{TAB}")
        If e.KeyValue = 40 Then e.Handled = True : e.SuppressKeyPress = True : SendKeys.Send("{TAB}")
    End Sub

    Private Sub txt_GSTPerc_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txt_GSTPerc.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
            e.Handled = True
        End If
        If Asc(e.KeyChar) = 13 Then
            e.Handled = True
            If MessageBox.Show("Do you want to Save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                save_record()

            Else
                txt_Name.Focus()

            End If

        End If
    End Sub


End Class
