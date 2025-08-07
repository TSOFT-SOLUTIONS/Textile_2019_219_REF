Public Class Delivery_Party_Creation

    Implements Interface_MDIActions

    Private con As New SqlClient.SqlConnection(Common_Procedures.Connection_String)
    Private New_Entry As Boolean = False
    Private vLedType As String
    Private Prec_ActCtrl As New Control
    Private vcbo_KeyDwnVal As Double
    Private Show_STS As Integer = 0
    Private Close_STS As Integer = 0

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
        chk_Close_Status.Checked = False
        New_Entry = False

        'cbo_BillType.Text = "BALANCE ONLY"
        'cbo_BillType.Enabled = False

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

        If Not IsNothing(dgv_Filter.CurrentCell) Then dgv_Filter.CurrentCell.Selected = False
    End Sub

    Public Sub move_record(ByVal idno As Integer)
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable

        If Val(idno) = 0 Then Exit Sub

        clear()

        da = New SqlClient.SqlDataAdapter("select a.*, c.Area_Name from  Sales_DeliveryAddress_Head a LEFT OUTER JOIN Area_Head c ON a.Area_IdNo = c.Area_IdNo   where a.Party_IdNo = " & Str(Val(idno)) & " ", con)
        da.Fill(dt)

        If dt.Rows.Count > 0 Then
            lbl_IdNo.Text = dt.Rows(0).Item("Party_IdNo").ToString
            txt_Name.Text = dt.Rows(0).Item("Party_Name").ToString
            txt_AlaisName.Text = dt.Rows(0).Item("Party_AlaisName").ToString
            cbo_Area.Text = dt.Rows(0)("Area_Name").ToString
            txt_Address1.Text = dt.Rows(0)("Address1").ToString
            txt_Address2.Text = dt.Rows(0)("Address2").ToString
            txt_Address3.Text = dt.Rows(0)("Address3").ToString
            txt_Address4.Text = dt.Rows(0)("Address4").ToString
            txt_PhoneNo.Text = dt.Rows(0)("Phone_No").ToString
            txt_TinNo.Text = dt.Rows(0)("Tin_No").ToString
            txt_CstNo.Text = dt.Rows(0)("Cst_No").ToString
            txt_Mail.Text = dt.Rows(0)("Mail_Name").ToString
            txt_MobileSms.Text = dt.Rows(0)("Mobile_No").ToString
            
            '-----------------GST ALTER------------------------------------
            txt_GSTIN_No.Text = dt.Rows(0)("Gstin_No").ToString
            cbo_State.Text = Common_Procedures.State_IdNoToName(con, Val(dt.Rows(0)("State_IdNo").ToString))
            '---------------------------------------------------------------

        End If

        dt.Dispose()
        da.Dispose()

        If txt_Name.Enabled And txt_Name.Visible Then txt_Name.Focus()

    End Sub

    Public Sub new_record() Implements Interface_MDIActions.new_record

        Call clear()

        lbl_IdNo.ForeColor = Color.Red
        New_Entry = True

        lbl_IdNo.Text = Common_Procedures.get_MaxIdNo(con, " Sales_DeliveryAddress_Head", "Party_IdNo", "")

        lbl_IdNo.Text = Common_Procedures.get_MaxIdNo(con, "Sales_DeliveryAddress_Head", "Party_IdNo", "")

        If txt_Name.Enabled And txt_Name.Visible Then txt_Name.Focus()

    End Sub

    Public Sub delete_record() Implements Interface_MDIActions.delete_record
        Dim cmd As New SqlClient.SqlCommand

        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.DeletingEntry, Common_Procedures.UR.Sizing_Delivery_Party_Creation, New_Entry, Me) = False Then Exit Sub

        If MessageBox.Show("Do you want to delete", "FOR DELETION...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) <> Windows.Forms.DialogResult.Yes Then
            Exit Sub
        End If

        Try

            cmd.Connection = con

            cmd.CommandText = "delete from  Sales_DeliveryAddress_Head where Party_IdNo = " & Str(Val(lbl_IdNo.Text))
            cmd.ExecuteNonQuery()

            cmd.Dispose()

            new_record()

            MessageBox.Show("Deleted Sucessfully!!", "FOR DELETION...", MessageBoxButtons.OK, MessageBoxIcon.Information)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT DELETE..", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If txt_Name.Enabled And txt_Name.Visible Then txt_Name.Focus()
            Exit Sub

        End Try

    End Sub

    Public Sub filter_record() Implements Interface_MDIActions.filter_record
        Dim da As New SqlClient.SqlDataAdapter("select Party_IdNo, Party_name from  Sales_DeliveryAddress_Head where Party_IdNo <> 0  order by Party_IdNo", con)
        Dim dt As New DataTable

        da.Fill(dt)

        dgv_Filter.Columns.Clear()
        dgv_Filter.DataSource = dt
        dgv_Filter.RowHeadersVisible = False

        dgv_Filter.Columns(0).HeaderText = "IDNO"
        dgv_Filter.Columns(1).HeaderText = "DELIVERY NAME"

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
            cmd.CommandText = "select min(Party_IdNo) from  Sales_DeliveryAddress_Head Where Party_IdNo <> 0"

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
            cmd.CommandText = "select max(Party_IdNo) from  Sales_DeliveryAddress_Head where Party_IdNo <> 0"

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
        Dim cmd As New SqlClient.SqlCommand("select min(Party_IdNo) from  Sales_DeliveryAddress_Head where Party_IdNo <> 0 and Party_IdNo > " & Str(Val(lbl_IdNo.Text)) & "", con)
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
            cmd.CommandText = "select max(Party_IdNo ) from  Sales_DeliveryAddress_Head where Party_IdNo <> 0 and Party_IdNo < " & Str((lbl_IdNo.Text)) & " "

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

        da = New SqlClient.SqlDataAdapter("select Party_Name from Sales_DeliveryAddress_Head where (Party_IdNo = 0 ) order by Party_Name", con)
        da.Fill(dt)

        cbo_Open.DataSource = dt
        cbo_Open.DisplayMember = "Party_Name"

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
        Dim sTATE_iD As Integer
        Dim LedArName As String = ""
        Dim LedPhNo As String = ""
        'Dim PhAr() As String
        Dim Sno As Integer = 0
        Dim undgrp_ParntCD As String = ""
        Dim LedAls_AcGrp_idno As Integer = 0


        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.SavingEntry, Common_Procedures.UR.Sizing_Delivery_Party_Creation, New_Entry, Me) = False Then Exit Sub

        If grp_Back.Enabled = False Then
            MessageBox.Show("Close other window", "DOES NOT SAVE", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        If Trim(txt_Name.Text) = "" Then
            MessageBox.Show("Invalid Delivery Name", "DOES NOT SAVE", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If txt_Name.Enabled Then txt_Name.Focus()
            Exit Sub
        End If



        Parnt_CD = Common_Procedures.AccountsGroup_IdNoToCode(con, acgrp_idno)

        'If acgrp_idno <> 10 And acgrp_idno <> 14 Then
        '    cbo_BillType.Text = "BALANCE ONLY"
        'End If



        ar_idno = Common_Procedures.Area_NameToIdNo(con, cbo_Area.Text)

        LedName = Trim(txt_Name.Text)
        If Val(ar_idno) <> 0 Then
            LedName = Trim(txt_Name.Text) & " (" & Trim(cbo_Area.Text) & ")"
        End If

        Show_STS = 0
        If chk_Show_In_AllEntry.Checked = True Then Show_STS = 1

        Close_STS = 0
        If chk_Close_Status.Checked = True Then Close_STS = 1


        SurName = Common_Procedures.Remove_NonCharacters(LedName)


        sTATE_iD = Common_Procedures.State_NameToIdNo(con, cbo_State.Text)

        trans = con.BeginTransaction

        Try

            cmd.Transaction = trans

            cmd.Connection = con

            If New_Entry = True Then
                lbl_IdNo.Text = Common_Procedures.get_MaxIdNo(con, " Sales_DeliveryAddress_Head", "Party_IdNo", "", trans)


                cmd.CommandText = "Insert into  Sales_DeliveryAddress_Head(Party_IdNo   ,    Sur_Name          ,    Party_Name                     , Party_AlaisName                  , Area_IdNo                 ,  Address1                         , Address2                            , Address3                       , Address4                   ,                Phone_No                    ,       Tin_No        ,          Cst_No                    ,                 Mobile_No         ,    Mail_Name             ,       Close_Status            ,   Gstin_No             ,   State_IdNo ) " & _
                            "       Values               (" & Str(Val(lbl_IdNo.Text)) & ",  '" & Trim(SurName) & "', '" & Trim(txt_Name.Text) & "', '" & Trim(txt_AlaisName.Text) & "', " & Str(Val(ar_idno)) & ",   '" & Trim(txt_Address1.Text) & "', '" & Trim(txt_Address2.Text) & "', '" & Trim(txt_Address3.Text) & "', '" & Trim(txt_Address4.Text) & "', '" & Trim(txt_PhoneNo.Text) & "', '" & Trim(txt_TinNo.Text) & "', '" & Trim(txt_CstNo.Text) & "', '" & Trim(txt_MobileSms.Text) & "' , '" & Trim(txt_Mail.Text) & "'," & Str(Val(Close_STS)) & " ,'" & Trim(txt_GSTIN_No.Text) & "'," & Str(sTATE_iD) & ")"
                cmd.ExecuteNonQuery()

            Else
                cmd.CommandText = "Update  Sales_DeliveryAddress_Head set  Sur_Name = '" & Trim(SurName) & "', Party_Name = '" & Trim(txt_Name.Text) & "', Party_AlaisName = '" & Trim(txt_AlaisName.Text) & "', Area_IdNo = " & Str(Val(ar_idno)) & ",  Address1 = '" & Trim(txt_Address1.Text) & "', Address2 = '" & Trim(txt_Address2.Text) & "', Address3 = '" & Trim(txt_Address3.Text) & "', Address4 = '" & Trim(txt_Address4.Text) & "', Phone_No = '" & Trim(txt_PhoneNo.Text) & "', Tin_No = '" & Trim(txt_TinNo.Text) & "', Cst_No = '" & Trim(txt_CstNo.Text) & "' , Mobile_No = '" & Trim(txt_MobileSms.Text) & "' ,  Mail_Name =  '" & Trim(txt_Mail.Text) & "' ,Close_Status = " & Str(Val(Close_STS)) & ",Gstin_No='" & Trim(txt_GSTIN_No.Text) & "',State_IdNo=" & Str(sTATE_iD) & "   where Party_IdNo = " & Str(Val(lbl_IdNo.Text))
                cmd.ExecuteNonQuery()

            End If

           

            trans.Commit()

            trans.Dispose()
            dt.Dispose()

            Common_Procedures.Master_Return.Return_Value = Trim(txt_Name.Text)
            Common_Procedures.Master_Return.Master_Type = "DELIVERY ADDRESS"

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

            If InStr(1, Trim(LCase(ex.Message)), "ix_ sales_deliveryaddress_head") > 0 Then
                MessageBox.Show("Duplicate Delivery Name", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            
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
        If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Area.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "AREA" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
            cbo_Area.Text = Trim(Common_Procedures.Master_Return.Return_Value)
        End If
        Common_Procedures.Master_Return.Master_Type = ""
        Common_Procedures.Master_Return.Return_Value = ""
    End Sub

    Private Sub Ledger_Creation_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim dt3 As New DataTable


        cbo_State.Items.Clear()
        cbo_State.DataSource = dt3
        cbo_State.DisplayMember = "State_Name"



        con.Open()



        da = New SqlClient.SqlDataAdapter("select Area_Name from Area_Head Order by Area_Name", con)
        da.Fill(dt2)
        cbo_Area.Items.Clear()
        cbo_Area.DataSource = dt2
        cbo_Area.DisplayMember = "Area_Name"

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
        AddHandler txt_Address1.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Address2.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Address3.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Address4.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_GSTIN_No.GotFocus, AddressOf ControlGotFocus

        AddHandler txt_Mail.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_PhoneNo.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_TinNo.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_CstNo.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_MobileSms.GotFocus, AddressOf ControlGotFocus
        AddHandler btnSave.GotFocus, AddressOf ControlGotFocus
        AddHandler btnClose.GotFocus, AddressOf ControlGotFocus

        AddHandler cbo_State.GotFocus, AddressOf ControlGotFocus

        AddHandler txt_Name.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_AlaisName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Area.LostFocus, AddressOf ControlLostFocus
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
        AddHandler txt_GSTIN_No.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_State.LostFocus, AddressOf ControlLostFocus

        AddHandler txt_Name.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_AlaisName.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Address1.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Address2.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Address3.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Address4.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_PhoneNo.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_TinNo.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_MobileSms.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_CstNo.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_GSTIN_No.KeyDown, AddressOf TextBoxControlKeyDown


        AddHandler txt_Name.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_AlaisName.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Address1.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Address2.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Address3.KeyPress, AddressOf TextBoxControlKeyPress

        AddHandler txt_CstNo.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Address4.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_PhoneNo.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_TinNo.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_MobileSms.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_GSTIN_No.KeyPress, AddressOf TextBoxControlKeyPress

        new_record()

    End Sub

    Private Sub Ledger_Creation_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed

        On Error Resume Next
        con.Close()
        con.Dispose()
        Common_Procedures.MDI_LedType = vLedType
        Common_Procedures.Last_Closed_FormName = Me.Name
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
        grp_Back.Enabled = True
        grp_Open.Visible = False
    End Sub

    Private Sub btn_Find_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Find.Click
        Dim movid As Integer


        movid = Common_Procedures.Despatch_NameToIdNo(con, cbo_Open.Text)

        If movid <> 0 Then move_record(movid)

        grp_Back.Enabled = True
        grp_Open.Visible = False

    End Sub

    Private Sub cbo_Open_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Open.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Open, Nothing, Nothing, "Sales_DeliveryAddress_Head", "Party_Name", "", "(Party_IdNo = 0)")
    End Sub

    Private Sub cbo_Open_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Open.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Open, Nothing, "Sales_DeliveryAddress_Head", "Party_Name", "", "(Party_IdNo = 0)")

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

   

    Private Sub txt_AlaisName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_AlaisName.KeyPress
        If Asc(e.KeyChar) = 34 Or Asc(e.KeyChar) = 39 Then
            e.Handled = True
        End If
    End Sub

    Private Sub cbo_Area_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Area.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Area, txt_AlaisName, txt_Address1, "area_head", "area_name", "", "(area_idno = 0)")
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

    Private Sub cbo_State_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_State.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "State_Head", "State_Name", "", "(State_Idno = 0)")

    End Sub

    Private Sub cbo_State_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_State.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_State, txt_Address4, txt_PhoneNo, "State_Head", "State_Name", "", "(State_Idno = 0)")
    End Sub

    Private Sub cbo_State_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_State.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_State, txt_PhoneNo, "State_Head", "State_Name", "", "(State_Idno = 0)")
    End Sub

    Private Sub txt_Mail_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_Mail.KeyDown
        On Error Resume Next
        If e.KeyValue = 38 Then SendKeys.Send("+{TAB}")
        If e.KeyValue = 40 Then

            If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                save_record()
            Else
                txt_Name.Focus()
            End If

        End If

    End Sub

    Private Sub txt_Mail_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Mail.KeyPress
        If Asc(e.KeyChar) = 13 Then
            If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                save_record()
            Else
                txt_Name.Focus()
            End If
        End If
    End Sub


End Class
