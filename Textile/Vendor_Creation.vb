Public Class Vendor_Creation
    Implements Interface_MDIActions

    Private con As New SqlClient.SqlConnection(Common_Procedures.Connection_String)
    Private New_Entry As Boolean = False
    Private vVedType As String
    Private Prec_ActCtrl As New Control
    Private vcbo_KeyDwnVal As Double
    Private Show_STS As Integer = 0
    Private TrnTo_DbName As String = ""
    Private vMovIdNo_FromEntry As Integer = 0

    Public Sub New(Optional ByVal MovIdNo As Integer = 0)
        ' This call is required by the designer.
        InitializeComponent()
        ' Add any initialization after the InitializeComponent() call.
        vMovIdNo_FromEntry = MovIdNo
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

        txt_MobileSms.Text = ""
        chk_Show_In_AllEntry.Checked = False
        cbo_Textile_WeaverName.Text = ""
        New_Entry = False

        txt_GSTNo.Text = ""
        cbo_State.Text = "TAMILNADU"

        cbo_AcGroup.Text = Common_Procedures.get_FieldValue(con, "AccountsGroup_Head", "AccountsGroup_Name", "(AccountsGroup_IdNo = 14)")

        cbo_BillType.Text = "BALANCE ONLY"
        cbo_BillType.Enabled = False

        lbl_IdNo.ForeColor = Color.Black

        grp_Back.Enabled = True
        grp_Open.Visible = False
        grp_Filter.Visible = False

    End Sub

    Private Sub ControlGotFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim txtbx As TextBox
        Dim combobx As ComboBox

        On Error Resume Next

        If TypeOf Me.ActiveControl Is TextBox Or TypeOf Me.ActiveControl Is ComboBox Then
            Me.ActiveControl.BackColor = Color.SpringGreen ' Color.MistyRose ' Color.PaleGreen
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
        If IsNothing(dgv_Filter.CurrentCell) Then Exit Sub
        If Not IsNothing(dgv_Filter.CurrentCell) Then dgv_Filter.CurrentCell.Selected = False
    End Sub

    Public Sub move_record(ByVal idno As Integer)
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable

        If Val(idno) = 0 Then Exit Sub

        clear()

        da = New SqlClient.SqlDataAdapter("select a.*, b.AccountsGroup_Name, c.Area_Name from Vendor_head a LEFT OUTER JOIN Area_Head c ON a.Area_IdNo = c.Area_IdNo  LEFT OUTER JOIN AccountsGroup_Head b ON a.AccountsGroup_IdNo = b.AccountsGroup_IdNo where a.Vendor_idno = " & Str(Val(idno)) & " and a.Vendor_type = '" & Trim(vVedType) & "'", con)
        da.Fill(dt)

        If dt.Rows.Count > 0 Then
            lbl_IdNo.Text = dt.Rows(0).Item("Vendor_IdNo").ToString
            txt_Name.Text = dt.Rows(0).Item("Vendor_MainName").ToString
            txt_AlaisName.Text = dt.Rows(0).Item("Vendor_AlaisName").ToString
            cbo_Area.Text = dt.Rows(0)("Area_Name").ToString
            cbo_AcGroup.Text = dt.Rows(0)("AccountsGroup_Name").ToString
            cbo_BillType.Text = dt.Rows(0)("Bill_Type").ToString
            txt_Address1.Text = dt.Rows(0)("Vendor_Address1").ToString
            txt_Address2.Text = dt.Rows(0)("Vendor_Address2").ToString
            txt_Address3.Text = dt.Rows(0)("Vendor_Address3").ToString
            txt_Address4.Text = dt.Rows(0)("Vendor_Address4").ToString
            txt_PhoneNo.Text = dt.Rows(0)("Vendor_PhoneNo").ToString
            txt_TinNo.Text = dt.Rows(0)("Vendor_TinNo").ToString
            txt_CstNo.Text = dt.Rows(0)("Vendor_CstNo").ToString
            txt_Mail.Text = dt.Rows(0)("Vendor_Mail").ToString
            txt_MobileSms.Text = dt.Rows(0)("Vendor_MobileNo").ToString
            If Val(dt.Rows(0).Item("Show_In_All_Entry").ToString) = 1 Then chk_Show_In_AllEntry.Checked = True
            cbo_Textile_WeaverName.Text = Common_Procedures.Ledger_IdNoToName(con, Val(dt.Rows(0).Item("Textile_To_WeaverIdNo").ToString), , TrnTo_DbName)
            cbo_State.Text = Common_Procedures.State_IdNoToName(con, Val(dt.Rows(0).Item("State").ToString))
            txt_GSTNo.Text = dt.Rows(0).Item("GST_No").ToString

        End If

        dt.Dispose()
        da.Dispose()

        If txt_Name.Enabled And txt_Name.Visible Then txt_Name.Focus()

    End Sub

    Public Sub new_record() Implements Interface_MDIActions.new_record

        Call clear()

        lbl_IdNo.ForeColor = Color.Red
        New_Entry = True

        lbl_IdNo.Text = Common_Procedures.get_MaxIdNo(con, "Vendor_head", "Vendor_idno", "")

        If Val(lbl_IdNo.Text) < 101 Then lbl_IdNo.Text = 101

        If txt_Name.Enabled And txt_Name.Visible Then txt_Name.Focus()

    End Sub

    Public Sub delete_record() Implements Interface_MDIActions.delete_record
        Dim cmd As New SqlClient.SqlCommand
        Dim da As SqlClient.SqlDataAdapter
        Dim dt As DataTable

        If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.Vendor_Creation, "~L~") = 0 And InStr(Common_Procedures.UR.Vendor_Creation, "~D~") = 0 Then MessageBox.Show("You have No Rights to Delete", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error) : Exit Sub

        If MessageBox.Show("Do you want to delete", "FOR DELETION...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) <> Windows.Forms.DialogResult.Yes Then
            Exit Sub
        End If

        If Val(lbl_IdNo.Text) < 101 Then
            MessageBox.Show("Cannot delete this default Vendor", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        Try


            da = New SqlClient.SqlDataAdapter("select count(*) from PAVU_DELIVERY_HEAD where Vendor_IdNo = " & Str(Val(lbl_IdNo.Text)), con)
            dt = New DataTable
            da.Fill(dt)
            If dt.Rows.Count > 0 Then
                If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                    If Val(dt.Rows(0)(0).ToString) > 0 Then
                        MessageBox.Show("Already used this Vendor", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        Exit Sub
                    End If
                End If
            End If

            da = New SqlClient.SqlDataAdapter("select count(*) from YArn_delivery_head where Vendor_IdNo = " & Str(Val(lbl_IdNo.Text)), con)
            dt = New DataTable
            da.Fill(dt)
            If dt.Rows.Count > 0 Then
                If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                    If Val(dt.Rows(0)(0).ToString) > 0 Then
                        MessageBox.Show("Already used this Vendor", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        Exit Sub
                    End If
                End If
            End If

            cmd.Connection = con

            cmd.CommandText = "delete from Vendor_AlaisHead where Vendor_IdNo = " & Str(Val(lbl_IdNo.Text))
            cmd.ExecuteNonQuery()

            cmd.CommandText = "delete from Vendor_head where Vendor_idno = " & Str(Val(lbl_IdNo.Text))
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
        Dim da As New SqlClient.SqlDataAdapter("select Vendor_idno, Vendor_name from Vendor_head where Vendor_idno <> 0 and Vendor_type = '" & Trim(vVedType) & "' order by Vendor_idno", con)
        Dim dt As New DataTable

        da.Fill(dt)

        dgv_Filter.Columns.Clear()
        dgv_Filter.DataSource = dt
        dgv_Filter.RowHeadersVisible = False

        dgv_Filter.Columns(0).HeaderText = "IDNO"
        dgv_Filter.Columns(1).HeaderText = "Vendor NAME"

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
            cmd.CommandText = "select min(Vendor_idno) from Vendor_head Where Vendor_idno <> 0 and Vendor_type = '" & Trim(vVedType) & "'"

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
            cmd.CommandText = "select max(Vendor_idno) from Vendor_head where Vendor_idno <> 0 and Vendor_type = '" & Trim(vVedType) & "'"

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
        Dim cmd As New SqlClient.SqlCommand("select min(Vendor_idno) from Vendor_head where Vendor_idno <> 0 and Vendor_idno > " & Str(Val(lbl_IdNo.Text)) & " and Vendor_type = '" & Trim(vVedType) & "'", con)
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
            cmd.CommandText = "select max(Vendor_idno ) from Vendor_head where Vendor_idno <> 0 and Vendor_idno < " & Str((lbl_IdNo.Text)) & " and Vendor_type = '" & Trim(vVedType) & "'"

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

        da = New SqlClient.SqlDataAdapter("select Vendor_DisplayName from Vendor_AlaisHead where (Vendor_IdNo = 0 or Vendor_type = '" & Trim(vVedType) & "') order by Vendor_DisplayName", con)
        da.Fill(dt)

        cbo_Open.DataSource = dt
        cbo_Open.DisplayMember = "Vendor_DisplayName"

        da.Dispose()

        grp_Open.Visible = True
        grp_Open.BringToFront()
        If cbo_Open.Enabled And cbo_Open.Visible Then cbo_Open.Focus()
        grp_Back.Enabled = False

    End Sub

    Public Sub insert_record() Implements Interface_MDIActions.insert_record
        'MessageBox.Show("insert record")
    End Sub

    Public Sub print_record() Implements Interface_MDIActions.print_record
        'MessageBox.Show("Vendor creation  -  print")
    End Sub

    Public Sub save_record() Implements Interface_MDIActions.save_record
        Dim cmd As New SqlClient.SqlCommand
        Dim trans As SqlClient.SqlTransaction
        Dim dt As New DataTable
        Dim nr As Long = 0
        Dim acgrp_idno As Integer = 0
        Dim TexStk_idno As Integer = 0
        Dim ar_idno As Integer = 0
        Dim Parnt_CD As String = ""
        Dim LedName As String = ""
        Dim SurName As String = ""
        Dim LedArName As String = ""
        Dim LedPhNo As String = ""
        'Dim PhAr() As String
        Dim Sno As Integer = 0
        Dim undgrp_ParntCD As String = ""
        Dim LedAls_AcGrp_idno As Integer = 0
        Dim sTATE_iD As Integer = 0


        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.SavingEntry, Common_Procedures.UR.Vendor_Creation, New_Entry, Me) = False Then Exit Sub

        If grp_Back.Enabled = False Then
            MessageBox.Show("Close other window", "DOES NOT SAVE", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        If Trim(txt_Name.Text) = "" Then
            MessageBox.Show("Invalid Legder Name", "DOES NOT SAVE", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If txt_Name.Enabled Then txt_Name.Focus()
            Exit Sub
        End If

        acgrp_idno = Common_Procedures.AccountsGroup_NameToIdNo(con, cbo_AcGroup.Text)
        If acgrp_idno = 0 Then
            MessageBox.Show("Invalid Accounts Group", "DOES NOT SAVE", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If cbo_AcGroup.Enabled Then cbo_AcGroup.Focus()
            Exit Sub
        End If

        Parnt_CD = Common_Procedures.AccountsGroup_IdNoToCode(con, acgrp_idno)

        If acgrp_idno <> 10 And acgrp_idno <> 14 Then
            cbo_BillType.Text = "BALANCE ONLY"
        End If
        If Trim(cbo_BillType.Text) = "" Then
            cbo_BillType.Text = "BALANCE ONLY"
            'MessageBox.Show("Invalid Bill Type", "DOES NOT SAVE", MessageBoxButtons.OK, MessageBoxIcon.Error)
            'If cbo_BillType.Enabled And cbo_BillType.Visible Then cbo_BillType.Focus()
            'Exit Sub
        End If

        ar_idno = Common_Procedures.Area_NameToIdNo(con, cbo_Area.Text)

        LedName = Trim(txt_Name.Text)
        If Val(ar_idno) <> 0 Then
            LedName = Trim(txt_Name.Text) & " (" & Trim(cbo_Area.Text) & ")"
        End If

        Show_STS = 0
        If chk_Show_In_AllEntry.Checked = True Then Show_STS = 1

        sTATE_iD = Common_Procedures.State_NameToIdNo(con, cbo_State.Text)

        TexStk_idno = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Textile_WeaverName.Text, , TrnTo_DbName)
        If cbo_Textile_WeaverName.Visible Then
            If Trim(cbo_Textile_WeaverName.Text) <> "" Then
                If Val(TexStk_idno) = 0 Then
                    MessageBox.Show("Invalid Textile Weaver Name", "DOES NOT SAVE", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    If cbo_Textile_WeaverName.Enabled Then cbo_Textile_WeaverName.Focus()
                    Exit Sub
                End If
            End If
        End If


        SurName = Common_Procedures.Remove_NonCharacters(LedName)

        trans = con.BeginTransaction

        Try

            cmd.Transaction = trans

            cmd.Connection = con

            If New_Entry = True Then
                lbl_IdNo.Text = Common_Procedures.get_MaxIdNo(con, "Vendor_head", "Vendor_idno", "", trans)
                If Val(lbl_IdNo.Text) < 101 Then lbl_IdNo.Text = 101

                cmd.CommandText = "Insert into Vendor_head(Vendor_IdNo      , Vendor_Name            , Sur_Name               , Vendor_MainName              , Vendor_AlaisName                  , Area_IdNo                , AccountsGroup_IdNo          , Parent_Code             , Bill_Type                        , Vendor_Address1                  , Vendor_Address2                  , Vendor_Address3                  , Vendor_Address4                  , Vendor_PhoneNo                  , Vendor_TinNo                  , Vendor_CstNo                  , Vendor_Type              , Vendor_MobileNo                    ,Show_In_All_Entry           , Vendor_Mail                   ,Textile_To_WeaverIdNo    , GST_No                       ,            State) " & _
                                    "Values (" & Str(Val(lbl_IdNo.Text)) & ", '" & Trim(LedName) & "', '" & Trim(SurName) & "', '" & Trim(txt_Name.Text) & "', '" & Trim(txt_AlaisName.Text) & "', " & Str(Val(ar_idno)) & ", " & Str(Val(acgrp_idno)) & ", '" & Trim(Parnt_CD) & "', '" & Trim(cbo_BillType.Text) & "', '" & Trim(txt_Address1.Text) & "', '" & Trim(txt_Address2.Text) & "', '" & Trim(txt_Address3.Text) & "', '" & Trim(txt_Address4.Text) & "', '" & Trim(txt_PhoneNo.Text) & "', '" & Trim(txt_TinNo.Text) & "', '" & Trim(txt_CstNo.Text) & "', '" & Trim(vVedType) & "' , '" & Trim(txt_MobileSms.Text) & "' , " & Str(Val(Show_STS)) & " , '" & Trim(txt_Mail.Text) & "' ," & Val(TexStk_idno) & " ,'" & Trim(txt_GSTNo.Text) & "'," & Str(Val(sTATE_iD)) & ")"
                cmd.ExecuteNonQuery()

            Else
                cmd.CommandText = "Update Vendor_head set Vendor_Name = '" & Trim(LedName) & "', Sur_Name = '" & Trim(SurName) & "', Vendor_MainName = '" & Trim(txt_Name.Text) & "', Vendor_AlaisName = '" & Trim(txt_AlaisName.Text) & "', Area_IdNo = " & Str(Val(ar_idno)) & ", AccountsGroup_IdNo = " & Str(Val(acgrp_idno)) & ", Parent_Code = '" & Trim(Parnt_CD) & "', Bill_Type = '" & Trim(cbo_BillType.Text) & "', Vendor_Address1 = '" & Trim(txt_Address1.Text) & "', Vendor_Address2 = '" & Trim(txt_Address2.Text) & "', Vendor_Address3 = '" & Trim(txt_Address3.Text) & "', Vendor_Address4 = '" & Trim(txt_Address4.Text) & "', Vendor_PhoneNo = '" & Trim(txt_PhoneNo.Text) & "', Vendor_TinNo = '" & Trim(txt_TinNo.Text) & "', Vendor_CstNo = '" & Trim(txt_CstNo.Text) & "' , Vendor_MobileNo = '" & Trim(txt_MobileSms.Text) & "' , Show_In_All_Entry = " & Str(Val(Show_STS)) & " , Vendor_Mail =  '" & Trim(txt_Mail.Text) & "' ,Textile_To_WeaverIdNo = " & Val(TexStk_idno) & ",GST_No='" & Trim(txt_GSTNo.Text) & "',State=" & Str(Val(sTATE_iD)) & " where Vendor_IdNo = " & Str(Val(lbl_IdNo.Text)) & ""
                cmd.ExecuteNonQuery()

            End If

            If acgrp_idno > 30 Then
                undgrp_ParntCD = Replace(Parnt_CD, "~" & Trim(Val(acgrp_idno)) & "~", "")

                undgrp_ParntCD = "~" & Trim(undgrp_ParntCD)

                LedAls_AcGrp_idno = Val(Common_Procedures.get_FieldValue(con, "AccountsGroup_Head", "AccountsGroup_IdNo", "(Parent_Idno = '" & Trim(undgrp_ParntCD) & "')", , trans))

            Else
                LedAls_AcGrp_idno = acgrp_idno

            End If


            cmd.CommandText = "delete from Vendor_AlaisHead where Vendor_IdNo = " & Str(Val(lbl_IdNo.Text))
            cmd.ExecuteNonQuery()

            LedArName = Trim(txt_Name.Text)
            If Val(ar_idno) <> 0 Then
                LedArName = Trim(txt_Name.Text) & " (" & Trim(cbo_Area.Text) & ")"
            End If

            cmd.CommandText = "Insert into Vendor_AlaisHead(Vendor_IdNo, Sl_No, Vendor_DisplayName, AccountsGroup_IdNo, Vendor_Type ) Values (" & Str(Val(lbl_IdNo.Text)) & ", 1, '" & Trim(LedArName) & "', " & Str(Val(LedAls_AcGrp_idno)) & ", '" & Trim(vVedType) & "')"
            cmd.ExecuteNonQuery()

            If Trim(txt_AlaisName.Text) <> "" Then
                LedArName = Trim(txt_AlaisName.Text)
                If Val(ar_idno) <> 0 Then
                    LedArName = Trim(txt_AlaisName.Text) & " (" & Trim(cbo_Area.Text) & ")"
                End If

                cmd.CommandText = "Insert into Vendor_AlaisHead(Vendor_IdNo, Sl_No, Vendor_DisplayName, AccountsGroup_IdNo, Vendor_Type ) Values (" & Str(Val(lbl_IdNo.Text)) & ", 2, '" & Trim(LedArName) & "', " & Str(Val(LedAls_AcGrp_idno)) & ", '" & Trim(vVedType) & "')"
                cmd.ExecuteNonQuery()

            End If

            trans.Commit()

            trans.Dispose()
            dt.Dispose()

            Common_Procedures.Master_Return.Return_Value = Trim(txt_Name.Text)
            Common_Procedures.Master_Return.Master_Type = "VENDOR"

            MessageBox.Show("Sucessfully Saved", "FOR SAVING...", MessageBoxButtons.OK, MessageBoxIcon.Information)

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

            If InStr(1, Trim(LCase(ex.Message)), Trim(LCase("ix_vendor_head"))) > 0 Then
                MessageBox.Show("Duplicate Vendor Name", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            ElseIf InStr(1, Trim(LCase(ex.Message)), Trim(LCase("ix_vendor_alaishead"))) > 0 Then
                MessageBox.Show("Duplicate Vendor Alais Name", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
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

    Private Sub Vendor_Creation_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated
        If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Area.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "AREA" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
            cbo_Area.Text = Trim(Common_Procedures.Master_Return.Return_Value)
        End If
        Common_Procedures.Master_Return.Master_Type = ""
        Common_Procedures.Master_Return.Return_Value = ""
        If vMovIdNo_FromEntry <> 0 Then
            If cbo_Textile_WeaverName.Enabled And cbo_Textile_WeaverName.Visible Then
                cbo_Textile_WeaverName.Focus()
            Else
                If txt_Name.Enabled And txt_Name.Visible Then txt_Name.Focus()
            End If
        End If
    End Sub

    Private Sub Vendor_Creation_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim dt3 As New DataTable

        cbo_Textile_WeaverName.Visible = False
        lbl_Textile.Visible = False
        ' TrnTo_CmpGrpIdNo = Val(Common_Procedures.get_FieldValue(con, Trim(Common_Procedures.CompanyDetailsDataBaseName) & "..CompanyGroup_Head", "CompanyGroup_IdNo", "(CompanyGroup_IdNo = " & Str(Val(Common_Procedures.CompGroupIdNo)) & ")"))
        If Common_Procedures.settings.Combine_Textile_Sizing_Software_Status = 1 Then
            TrnTo_DbName = Common_Procedures.get_Company_TextileDataBaseName(Trim(Val(Common_Procedures.CompGroupIdNo)))
            cbo_Textile_WeaverName.Visible = True
            lbl_Textile.Visible = True
        Else
            TrnTo_DbName = Common_Procedures.get_Company_DataBaseName(Trim(Val(Common_Procedures.CompGroupIdNo)))
        End If
        vVedType = Trim(Common_Procedures.MDI_VedType)

        cbo_BillType.Enabled = False

        If Trim(UCase(vVedType)) = "TRANSPORT" Then
            Me.BackColor = Color.LightCyan   'Color.LightSteelBlue   'Color.LightSeaGreen
            lbl_Heading.Text = "TRANSPORT CREATION"
            Me.Text = "TRANSPORT CREATION"

        ElseIf Trim(UCase(vVedType)) = "SIZING" Then
            Me.BackColor = Color.LightSkyBlue
            lbl_Heading.Text = "SIZING CREATION"
            Me.Text = "SIZING CREATION"

        ElseIf Trim(UCase(vVedType)) = "WEAVER" Then
            Me.BackColor = Color.LemonChiffon
            lbl_Heading.Text = "WEAVER CREATION"
            Me.Text = "WEAVER CREATION"

        ElseIf Trim(UCase(vVedType)) = "JOBWORKER" Then
            Me.BackColor = Color.LightGreen
            lbl_Heading.Text = "JOBWORKER CREATION"
            Me.Text = "JOBWORKER CREATION"

        ElseIf Trim(UCase(vVedType)) = "REWINDING" Then
            Me.BackColor = Color.Khaki
            lbl_Heading.Text = "REWINDING CREATION"
            Me.Text = "REWINDING CREATION"

        ElseIf Trim(UCase(vVedType)) = "SPINNING" Then
            Me.BackColor = Color.LightGray
            lbl_Heading.Text = "SPINNING CREATION"
            Me.Text = "SPINNING CREATION"

        ElseIf Trim(UCase(vVedType)) = "SALESPARTY" Then
            Me.BackColor = Color.LightSalmon
            lbl_Heading.Text = "SALESPARTY CREATION"
            Me.Text = "SALESPARTY CREATION"

        Else
            Me.BackColor = Color.LightSkyBlue
            lbl_Heading.Text = "VENDOR CREATION"
            Me.Text = "VENDOR CREATION"

            cbo_BillType.Enabled = True

        End If

        con.Open()

        da = New SqlClient.SqlDataAdapter("select AccountsGroup_Name from AccountsGroup_Head Order by AccountsGroup_Name", con)
        da.Fill(dt1)
        cbo_AcGroup.Items.Clear()
        cbo_AcGroup.DataSource = dt1
        cbo_AcGroup.DisplayMember = "AccountsGroup_Name"

        cbo_BillType.Items.Clear()
        cbo_BillType.Items.Add("BALANCE ONLY")
        cbo_BillType.Items.Add("BILL TO BILL")

        da = New SqlClient.SqlDataAdapter("select Area_Name from Area_Head Order by Area_Name", con)
        da.Fill(dt2)
        cbo_Area.Items.Clear()
        cbo_Area.DataSource = dt2
        cbo_Area.DisplayMember = "Area_Name"

        da = New SqlClient.SqlDataAdapter("SELECT State_Name FROM State_Head ORDER BY State_Name", con)
        da.Fill(dt3)
        cbo_State.Items.Clear()
        cbo_State.DataSource = dt3
        cbo_State.DisplayMember = "State_Name"

        da.Dispose()

        grp_Open.Visible = False
        grp_Open.Left = (Me.Width - grp_Open.Width) - 100
        grp_Open.Top = (Me.Height - grp_Open.Height) - 100

        grp_Filter.Visible = False
        grp_Filter.Left = (Me.Width - grp_Filter.Width) - 100
        grp_Filter.Top = (Me.Height - grp_Filter.Height) - 100

        AddHandler txt_Name.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_AlaisName.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Area.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Textile_WeaverName.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_AcGroup.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_BillType.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Address1.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Address2.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Address3.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Address4.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_State.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_GSTNo.GotFocus, AddressOf ControlGotFocus

        AddHandler txt_Mail.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_PhoneNo.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_TinNo.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_CstNo.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_MobileSms.GotFocus, AddressOf ControlGotFocus
        AddHandler btnSave.GotFocus, AddressOf ControlGotFocus
        AddHandler btnClose.GotFocus, AddressOf ControlGotFocus

        AddHandler txt_Name.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_AlaisName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Textile_WeaverName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Area.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_AcGroup.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_BillType.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Mail.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Address1.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Address2.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Address3.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Address4.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_PhoneNo.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_TinNo.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_MobileSms.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_CstNo.LostFocus, AddressOf ControlLostFocus
        AddHandler btnSave.LostFocus, AddressOf ControlLostFocus
        AddHandler btnClose.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_State.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_GSTNo.LostFocus, AddressOf ControlLostFocus

        AddHandler txt_Name.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_AlaisName.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Address1.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Address2.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Address3.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Address4.KeyDown, AddressOf TextBoxControlKeyDown
        'AddHandler txt_PhoneNo.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_TinNo.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_MobileSms.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Mail.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_GSTNo.KeyDown, AddressOf TextBoxControlKeyDown


        AddHandler txt_Name.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_AlaisName.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Address1.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Address2.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Address3.KeyPress, AddressOf TextBoxControlKeyPress

        AddHandler txt_Mail.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Address4.KeyPress, AddressOf TextBoxControlKeyPress
        'AddHandler txt_PhoneNo.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_TinNo.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_MobileSms.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_GSTNo.KeyPress, AddressOf TextBoxControlKeyPress

        If Val(vMovIdNo_FromEntry) <> 0 Then
            move_record(vMovIdNo_FromEntry)
        Else
            new_record()
        End If

    End Sub

    Private Sub Vendor_Creation_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed

        On Error Resume Next
        con.Close()
        con.Dispose()
        Common_Procedures.Last_Closed_FormName = Me.Name
    End Sub

    Private Sub Vendor_Creation_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
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
        grp_Back.Enabled = True
        grp_Open.Visible = False
    End Sub

    Private Sub btn_Find_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Find.Click
        Dim movid As Integer


        movid = Common_Procedures.Vendor_AlaisNameToIdNo(con, cbo_Open.Text)

        If movid <> 0 Then move_record(movid)

        grp_Back.Enabled = True
        grp_Open.Visible = False

    End Sub

    Private Sub cbo_Open_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Open.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Open, Nothing, Nothing, "Vendor_AlaisHead", "Vendor_DisplayName", "(Vendor_Type = '" & Trim(vVedType) & "')", "(Vendor_idno = 0)")
    End Sub

    Private Sub cbo_Open_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Open.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Open, Nothing, "Vendor_AlaisHead", "Vendor_DisplayName", "(Vendor_Type = '" & Trim(vVedType) & "')", "(Vendor_idno = 0)")

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
        If Asc(e.KeyChar) = 34 Or Asc(e.KeyChar) = 39 Then
            e.Handled = True
        End If
    End Sub

    Private Sub cbo_AcGroup_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_AcGroup.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_AcGroup, cbo_Area, Nothing, "", "", "", "")

        If (e.KeyValue = 40 And cbo_AcGroup.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
            If cbo_BillType.Enabled And cbo_BillType.Visible Then
                cbo_BillType.Focus()
            Else
                txt_Address1.Focus()
            End If
        End If

    End Sub

    Private Sub cbo_AcGroup_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_AcGroup.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_AcGroup, Nothing, "AccountsGroup_Head", "AccountsGroup_Name", "", "(AccountsGroup_IdNo = 0)")
        If Asc(e.KeyChar) = 13 Then
            If cbo_BillType.Enabled And cbo_BillType.Visible Then
                cbo_BillType.Focus()
            Else
                txt_Address1.Focus()
            End If
        End If
    End Sub

    Private Sub cbo_BillType_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_BillType.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_BillType, cbo_AcGroup, txt_Address1, "", "", "", "")
    End Sub

    Private Sub cbo_BillType_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_BillType.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_BillType, txt_Address1, "", "", "", "")
    End Sub

    Private Sub txt_AlaisName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_AlaisName.KeyPress
        If Asc(e.KeyChar) = 34 Or Asc(e.KeyChar) = 39 Then
            e.Handled = True
        End If
    End Sub

    Private Sub cbo_Area_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Area.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Area, txt_Name, txt_Address1, "area_head", "area_name", "", "(area_idno = 0)")
    End Sub

    Private Sub cbo_Area_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Area.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Area, txt_Address1, "area_head", "area_name", "", "(area_idno = 0)")

    End Sub

    Private Sub cbo_Area_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Area.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then

            Dim f As New Area_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Area.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If

    End Sub

    Private Sub txt_CstNo_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_CstNo.KeyDown
        If e.KeyValue = 38 Then SendKeys.Send("+{TAB}")
        If e.KeyValue = 40 Then btnSave.Focus()
    End Sub

    Private Sub txt_CstNo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_CstNo.KeyPress
        If Asc(e.KeyChar) = 13 Then
            If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                save_record()
            Else
                txt_Name.Focus()
            End If
        End If
    End Sub

    Private Sub txt_PhoneNo_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_PhoneNo.KeyDown
        If e.KeyValue = 38 Then SendKeys.Send("+{TAB}")
        If e.KeyValue = 40 Then btnSave.Focus()
    End Sub

    Private Sub txt_PhoneNo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_PhoneNo.KeyPress
        If Asc(e.KeyChar) = 13 Then
            If cbo_Textile_WeaverName.Visible = True Then
                cbo_Textile_WeaverName.Focus()
            Else
                If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                    save_record()
                Else
                    txt_Name.Focus()
                End If
            End If
        End If
    End Sub
 
    Private Sub cbo_Textile_WeaverName_Gotfocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Textile_WeaverName.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, TrnTo_DbName & "..Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_IdNo = 0 or Ledger_Type = 'WEAVER') and Close_status = 0", "(Ledger_idno = 0)")
    End Sub

    Private Sub cbo_Textile_WeaverName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Textile_WeaverName.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Textile_WeaverName, txt_PhoneNo, Nothing, TrnTo_DbName & "..Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_IdNo = 0 or Ledger_Type = 'WEAVER') and Close_status = 0", "(Ledger_idno = 0)")
        If (e.KeyValue = 40 And cbo_Textile_WeaverName.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
            If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                save_record()
            Else
                txt_Name.Focus()
            End If
        End If
    End Sub

    Private Sub cbo_Textile_WeaverName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Textile_WeaverName.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Textile_WeaverName, Nothing, TrnTo_DbName & "..Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_IdNo = 0 or Ledger_Type = 'WEAVER') and Close_status = 0", "(Ledger_idno = 0)")
        If Asc(e.KeyChar) = 13 Then
            If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                save_record()
            Else
                txt_Name.Focus()
            End If
        End If
    End Sub

    Private Sub cbo_State_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_State.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "State_Head", "State_Name", "", "(State_IdNo = 0)")
    End Sub

    Private Sub cbo_State_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_State.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_State, txt_Address4, txt_GSTNo, "State_Head", "State_Name", "", "(State_IdNo = 0)")
    End Sub

    Private Sub cbo_State_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_State.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_State, txt_GSTNo, "State_Head", "State_Name", "", "(State_IdNo = 0)")
    End Sub

End Class
