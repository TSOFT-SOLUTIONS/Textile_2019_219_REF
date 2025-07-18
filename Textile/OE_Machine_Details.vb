Public Class OE_Machine_Details

    Implements Interface_MDIActions

    Private con As New SqlClient.SqlConnection(Common_Procedures.Connection_String)
    Private FrmLdSTS As Boolean = False
    Private New_Entry As Boolean = False
    Private Insert_Entry As Boolean = False
    Private Filter_Status As Boolean = False
    Private Pk_Condition As String = "MACDT-"
    Private Prec_ActCtrl As New Control
    Private Dep_Id As Integer
    Private vcbo_KeyDwnVal As Double


    Public Sub New()
        ' This call is required by the designer.
        InitializeComponent()
        ' Add any initialization after the InitializeComponent() call.
    End Sub

    Private Sub clear()
        New_Entry = False
        Insert_Entry = False

        pnl_back.Enabled = True
        grp_Filter.Visible = False
        grp_find.Visible = False

        lbl_IdNo.Text = ""
        lbl_IdNo.ForeColor = Color.Black
        cbo_Count.Text = ""
        cbo_MachineName.Text = ""
        txt_MachineNo.Text = ""

        cbo_Find.Text = ""
        txt_Speed.Text = ""

        txt_Efficiency.Text = ""
        cbo_Manufacturer.Text = ""
        txt_CountHank.Text = ""

    End Sub

    Private Sub ControlGotFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim txtbx As TextBox
        Dim combobx As ComboBox
        On Error Resume Next

        If TypeOf Me.ActiveControl Is TextBox Or TypeOf Me.ActiveControl Is ComboBox Or TypeOf Me.ActiveControl Is Button Then
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
    Private Sub move_record(ByVal no As String)
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt2 As New DataTable
        Dim LockSTS As Boolean = False
        Dim Sno As Integer = 0
        Dim n As Integer = 0

        If Val(no) = 0 Then Exit Sub

        clear()

        Try



            da1 = New SqlClient.SqlDataAdapter("select a.* from OE_Machine_Details a  Where a.Machine_Details_IdNo = " & Val(no) & " and a.Department_IdNo = " & Str(Val(Dep_Id)) & "", con)
            dt1 = New DataTable
            da1.Fill(dt1)

            If dt1.Rows.Count > 0 Then

                lbl_IdNo.Text = dt1.Rows(0).Item("Machine_Details_IdNo").ToString
                txt_MachineNo.Text = dt1.Rows(0).Item("Machine_Details_No").ToString
                cbo_Count.Text = Common_Procedures.Count_IdNoToName(con, dt1.Rows(0).Item("Count_IdNo").ToString)
                cbo_MachineName.Text = Common_Procedures.OE_Machine_IdNoToName(con, dt1.Rows(0).Item("Machine_IdNo").ToString)
                cbo_Manufacturer.Text = Common_Procedures.OE_Manufacture_IdNoToName(con, dt1.Rows(0).Item("Manufacture_Idno").ToString)
                txt_CountHank.Text = Val(dt1.Rows(0).Item("Count_Hank").ToString)
                txt_Speed.Text = Val(dt1.Rows(0).Item("Speed").ToString)
                txt_Efficiency.Text = Val(dt1.Rows(0).Item("Efficiency_Percentage").ToString)

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally
            da1.Dispose()
            dt1.Dispose()

            If txt_MachineNo.Visible And txt_MachineNo.Enabled Then txt_MachineNo.Focus()

        End Try

    End Sub

    Private Sub Machine_Details_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated

        Try

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_MachineName.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "MACHINE" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_MachineName.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If
            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Count.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "COLOUR" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Count.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If
            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Manufacturer.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "MANUFACTURE" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Manufacturer.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If


            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""


        Catch ex As Exception
            'MessageBox.Show(ex.Message, "DOES NOT SHOW...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        FrmLdSTS = False

    End Sub

    Private Sub Machine_Details_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Me.Text = ""

        con.Open()
        Dep_Id = Trim(Common_Procedures.OE_Department_IdNo)

        If Val(Dep_Id) = 1 Then
            Me.BackColor = Color.LightSkyBlue
            lbl_Heading.Text = "CARDING"
            Me.Text = "(MACHINE DETAILS)CARDING"
            lbl_Speed.Text = "Speed"

        End If

        If Val(Dep_Id) = 2 Then
            Me.BackColor = Color.LightSkyBlue
            lbl_Heading.Text = "DRAWING"
            Me.Text = "(MACHINE DETAILS)DRAWING"
            lbl_Speed.Text = "Speed"
        End If

        If Val(Dep_Id) = 3 Then
            Me.BackColor = Color.LightSkyBlue
            lbl_Heading.Text = "VORTEX"
            Me.Text = "(MACHINE DETAILS)VORTEX"
            lbl_Speed.Text = "Speed"

        End If

        grp_Filter.Visible = False
        grp_Filter.Left = (Me.Width - grp_Filter.Width) \ 2
        grp_Filter.Top = ((Me.Height - grp_Filter.Height) \ 2) + 20

        grp_find.Visible = False
        grp_find.Left = (Me.Width - grp_find.Width) \ 2
        grp_find.Top = ((Me.Height - grp_find.Height) \ 2) + 20

        AddHandler cbo_MachineName.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Count.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Find.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_MachineNo.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Efficiency.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Manufacturer.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_CountHank.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Speed.GotFocus, AddressOf ControlGotFocus

        AddHandler cbo_Count.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_MachineName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Find.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_MachineNo.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Efficiency.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Speed.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Manufacturer.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_CountHank.LostFocus, AddressOf ControlLostFocus

        ' AddHandler txt_MachineNo.KeyDown, AddressOf TextBoxControlKeyDown
        'AddHandler txt_FrontRollDia.KeyDown, AddressOf TextBoxControlKeyDown
        '    AddHandler txt_Efficiency.KeyDown, AddressOf TextBoxControlKeyDown
        ' AddHandler txt_Speed.KeyDown, AddressOf TextBoxControlKeyDown

        'AddHandler txt_Efficiency.KeyPress, AddressOf TextBoxControlKeyPress
        ' AddHandler txt_MachineNo.KeyPress, AddressOf TextBoxControlKeyPress
        ' AddHandler txt_Speed.KeyPress, AddressOf TextBoxControlKeyPress




        dgv_Filter.RowTemplate.Height = 27

        Filter_Status = False
        FrmLdSTS = True
        new_record()

    End Sub

    Private Sub Machine_Details_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        On Error Resume Next
        con.Close()
        con.Dispose()
        Common_Procedures.Last_Closed_FormName = Me.Name
    End Sub

    Private Sub Machine_Details_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress

        Try

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

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT CLOSE...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub



    Public Sub delete_record() Implements Interface_MDIActions.delete_record
        Dim cmd As New SqlClient.SqlCommand
        Dim tr As SqlClient.SqlTransaction
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim Dt2 As New DataTable
        Dim NewCode As String = ""
        Dim DelvSts As Integer = 0
        Dim Nr As Integer = 0
        Dim NoofKnotBmsInCD As Integer = 0
        Dim NoofKnotBmsInLom As Integer = 0
        Dim Lm_ID As Integer = 0

        If MessageBox.Show("Do you want to Delete?", "FOR DELETION...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) <> Windows.Forms.DialogResult.Yes Then
            Exit Sub
        End If

        'If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.Inhouse_Machine_Details, "~L~") = 0 And InStr(Common_Procedures.UR.Inhouse_Machine_Details, "~D~") = 0 Then MessageBox.Show("You have No Rights to Delete", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error) : Exit Sub

        If New_Entry = True Then
            MessageBox.Show("This is New Entry", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If



        tr = con.BeginTransaction

        Try

            cmd.Connection = con
            cmd.Transaction = tr



            cmd.CommandText = "delete from OE_Machine_Details where Machine_details_idno = " & Str(Val(lbl_IdNo.Text)) & " "
            cmd.ExecuteNonQuery()



            tr.Commit()

            new_record()

            MessageBox.Show("Deleted Sucessfully!!!", "FOR DELETION...", MessageBoxButtons.OK, MessageBoxIcon.Information)

        Catch ex As Exception

            tr.Rollback()
            MessageBox.Show(ex.Message, "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        If cbo_MachineName.Enabled = True And cbo_MachineName.Visible = True Then cbo_MachineName.Focus()

    End Sub

    Public Sub filter_record() Implements Interface_MDIActions.filter_record
        Dim da As New SqlClient.SqlDataAdapter("select a.Machine_Details_IdNo,a.Machine_Details_No,a.Manufacturer,A.Efficiency_Perc,b.Count_Name,c.Machine_name from OE_Machine_details a INNER JOIN Count_Head b ON b.Count_IdNo = a.Count_Idno INNER JOIN OE_Machine_Head c ON c.machine_IdNo = a.Machine_Idno where a.Department_IdNo = " & Val(Dep_Id) & " and a.Machine_Details_IdNo <> 0 order by a.Machine_Details_IdNo", con)
        Dim dt As New DataTable

        da.Fill(dt)

        With dgv_Filter

            .Columns.Clear()
            .DataSource = dt

            .RowHeadersVisible = False

            .AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells
            .AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill

            .Columns(0).HeaderText = "IDNO"
            .Columns(1).HeaderText = "MACHINE NO"
            .Columns(2).HeaderText = "MACHINE NAME"
            .Columns(3).HeaderText = "MANUFACTURE"
            .Columns(4).HeaderText = "COUNT NAME"
            .Columns(5).HeaderText = "EFF%"


            .Columns(0).FillWeight = 60
            .Columns(1).FillWeight = 100
            .Columns(2).FillWeight = 150
            .Columns(3).FillWeight = 100
            .Columns(4).FillWeight = 80
            .Columns(5).FillWeight = 80


        End With

        new_record()


        pnl_back.Enabled = False
        grp_Filter.Visible = True

        If dgv_Filter.Enabled And dgv_Filter.Visible Then dgv_Filter.Focus()



        da.Dispose()



    End Sub

    Public Sub insert_record() Implements Interface_MDIActions.insert_record

    End Sub

    Public Sub movefirst_record() Implements Interface_MDIActions.movefirst_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim movid As Integer = 0

        Try
            da = New SqlClient.SqlDataAdapter("select min(Machine_Details_idno) from OE_Machine_Details Where Department_IdNo = " & Str(Val(Dep_Id)) & " AND Machine_Details_idno <> 0", con)
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
            da = New SqlClient.SqlDataAdapter("select max(Machine_Details_IdNo) from OE_Machine_Details Where Department_IdNo = " & Str(Val(Dep_Id)) & " AND Machine_Details_IdNo <> 0", con)
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
            da = New SqlClient.SqlDataAdapter("select min(Machine_Details_IdNo) from OE_Machine_Details Where Department_IdNo = " & Str(Val(Dep_Id)) & " AND Machine_Details_IdNo > " & Str(Val(lbl_IdNo.Text)) & " and Machine_Details_IdNo <> 0", con)
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
            da = New SqlClient.SqlDataAdapter("select max(Machine_Details_IdNo) from OE_Machine_Details Where Department_IdNo = " & Str(Val(Dep_Id)) & " AND Machine_Details_IdNo < " & Str(Val(lbl_IdNo.Text)) & " and Machine_Details_IdNo <> 0", con)
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
            lbl_IdNo.Text = Common_Procedures.get_MaxIdNo(con, "OE_Machine_Details", "Machine_Details_IdNo", "")


            If txt_MachineNo.Enabled And txt_MachineNo.Visible Then txt_MachineNo.Focus()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR NEW RECORD...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub


    Public Sub open_record() Implements Interface_MDIActions.open_record
        Dim da As New SqlClient.SqlDataAdapter("select Machine_Details_No from OE_Machine_details order by Machine_details_No", con)
        Dim dt As New DataTable

        da.Fill(dt)

        cbo_Find.DataSource = dt
        cbo_Find.DisplayMember = "Machine_details_No"

        new_record()

        grp_find.Visible = True
        pnl_back.Enabled = False
        If cbo_Find.Enabled And cbo_Find.Visible Then cbo_Find.Focus()
    End Sub

    Public Sub save_record() Implements Interface_MDIActions.save_record
        Dim cmd As New SqlClient.SqlCommand
        Dim tr As SqlClient.SqlTransaction
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim NewCode As String = ""
        Dim nr As Long = 0
        Dim Lot_STS As Integer = 0
        Dim led_id As Integer = 0
        Dim PkTy_ID As Integer = 0
        Dim Cnt_ID As Integer = 0
        Dim Mac_ID As Integer = 0
        Dim Partcls As String = ""
        Dim EntID As String = ""
        Dim PBlNo As String = ""
        Dim NoofInpBmsInLom As Integer = 0
        Dim NoofKnotBmsInCD As Integer = 0
        Dim NoofKnotBmsInLom As Integer = 0
        Dim decnt_ID As Integer = 0
        Dim declr_ID As Integer = 0
        Dim Sno As Integer = 0
        Dim vOrdByNo As Single = 0
        Dim VManufact_Idno As Integer = 0

        If pnl_back.Enabled = False Then
            MessageBox.Show("Close all other windows", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        ' If Common_Procedures.UserRight_Check(Common_Procedures.UR.Inhouse_Machine_Details, New_Entry) = False Then Exit Sub



        Cnt_ID = Common_Procedures.Count_NameToIdNo(con, cbo_Count.Text)
        If Cnt_ID = 0 Then
            MessageBox.Show("Invalid Count Name", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If cbo_Count.Enabled Then cbo_Count.Focus()
            Exit Sub
        End If

        Mac_ID = Common_Procedures.OE_Machine_NameToIdNo(con, cbo_MachineName.Text)
        If Mac_ID = 0 Then
            MessageBox.Show("Invalid Machine Name", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If cbo_MachineName.Enabled Then cbo_MachineName.Focus()
            Exit Sub
        End If

        If Trim(cbo_Count.Text) <> "" Then
            If Val(txt_CountHank.Text) <> 0 Then
                Dim VChk_Cnt_Hank
                da = New SqlClient.SqlDataAdapter("Select Count_Hank from OE_Count_details where Count_Idno = " & Val(Cnt_ID) & " and Department_IdNo =" & Val(Dep_Id) & " ", con)
                Dim dt = New DataTable
                da.Fill(dt)

                If dt.Rows.Count > 0 Then
                    VChk_Cnt_Hank = dt.Rows(0)(0).ToString
                End If

                If Val(VChk_Cnt_Hank) <> Val(txt_CountHank.Text) Then
                    MessageBox.Show("Invalid Count Hank For This Count", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    If cbo_MachineName.Enabled Then cbo_MachineName.Focus()
                    Exit Sub
                End If

            End If
        End If

        VManufact_Idno = Common_Procedures.OE_Manufacture_NameToIdNo(con, cbo_Manufacturer.Text)

        tr = con.BeginTransaction

        Try

            cmd.Connection = con
            cmd.Transaction = tr


            If New_Entry = True Then

                lbl_IdNo.Text = Common_Procedures.get_MaxIdNo(con, "OE_Machine_Details", "Machine_Details_IdNo", "", tr)

                cmd.CommandText = "Insert into OE_Machine_Details (             Machine_Details_IdNo ,    Machine_Details_No          ,  Machine_IdNo     ,        Count_IdNo      ,       Manufacture_IdNo        ,    Count_Hank       ,                      Speed                 ,        Efficiency_Percentage   ,     Department_Idno       ) " &
                                        "      Values             ('" & Trim(lbl_IdNo.Text) & "',      '" & Trim(txt_MachineNo.Text) & "',  " & Val(Mac_ID) & ", " & Val(Cnt_ID) & ", " & Val(VManufact_Idno) & "," & Str(Val(txt_CountHank.Text)) & ",  " & Val(txt_Speed.Text) & "  ," & Val(txt_Efficiency.Text) & "," & Val(Dep_Id) & "    ) "
                cmd.ExecuteNonQuery()

            Else

                cmd.CommandText = "Update OE_Machine_Details set  Machine_Details_No = '" & Trim(txt_MachineNo.Text) & "',Count_IdNo = " & Str(Val(Cnt_ID)) & ", Machine_IdNo = " & Str(Val(Mac_ID)) & ",Manufacture_IdNo = " & Val(VManufact_Idno) & ", Count_Hank = " & Str(Val(txt_CountHank.Text)) & " ,Speed = " & Val(txt_Speed.Text) & "  , Efficiency_Percentage = " & Val(txt_Efficiency.Text) & "  ,Department_Idno=" & Val(Dep_Id) & "    Where Machine_Details_IdNo = " & Str(Val(lbl_IdNo.Text)) & " "
                cmd.ExecuteNonQuery()

            End If


            tr.Commit()

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
            tr.Rollback()
            MessageBox.Show(ex.Message, "FOR SAVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally
            dt1.Dispose()
            da.Dispose()
            cmd.Dispose()
            tr.Dispose()

            If txt_MachineNo.Enabled And txt_MachineNo.Visible Then txt_MachineNo.Focus()

        End Try

    End Sub

    Public Sub print_record() Implements Interface_MDIActions.print_record
        '------ No Print
    End Sub

    Private Sub cbo_MachineName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_MachineName.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "OE_Machine_Head", "Machine_Name", "", "(Machine_idno = 0)")
    End Sub

    Private Sub cbo_MachineName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_MachineName.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_MachineName, txt_MachineNo, cbo_Manufacturer, "OE_Machine_Head", "Machine_Name", "", "(Machine_idno = 0)")
    End Sub

    Private Sub cbo_MachineName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_MachineName.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_MachineName, cbo_Manufacturer, "OE_Machine_Head", "Machine_Name", "", "(Machine_idno = 0)")
    End Sub

    Private Sub cbo_Machinename_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_MachineName.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then

            Dim f As New OE_Machine_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_MachineName.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1

            f.Show()

        End If
    End Sub


    Private Sub cbo_Count_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Count.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Count_hEAD", "Count_Name", "", "(Count_IdNo = 0)")

    End Sub
    Private Sub cbo_Count_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Count.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Count, cbo_Manufacturer, txt_CountHank, "Count_head", "Count_Name", "", "(Count_IdNo = 0)")
    End Sub

    Private Sub cbo_Count_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Count.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Count, txt_CountHank, "Count_Head", "Count_Name", "", "(Count_IdNo)")
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable

        da = New SqlClient.SqlDataAdapter("select Count_Hank from OE_Count_Details Where Count_Idno = '" & Trim(Common_Procedures.Count_NameToIdNo(con, cbo_Count.Text)) & "'and Department_IdNo = " & Str(Val(Dep_Id)), con)
        da.Fill(dt)

        If dt.Rows.Count > 0 Then
            If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                txt_CountHank.Text = dt.Rows(0)(0).ToString
            End If
        End If

    End Sub
    Private Sub btn_save_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_save.Click
        save_record()
    End Sub

    Private Sub btn_close_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_close.Click
        Me.Close()
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
    Private Sub txt_ShellRodSpeedMpm_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Speed.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
        If Asc(e.KeyChar) = 13 Then
            txt_Efficiency.Focus()
        End If
    End Sub
    Private Sub txt_CountHank_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_CountHank.KeyDown
        If e.KeyValue = 38 Then SendKeys.Send("+{TAB}")
        If e.KeyValue = 40 Then

            SendKeys.Send("{TAB}")

        End If
    End Sub
    Private Sub txt_CountHank_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_CountHank.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
        If Asc(e.KeyChar) = 13 Then

            SendKeys.Send("{TAB}")

        End If

    End Sub
    Private Sub txt_Efficiency_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_Efficiency.KeyDown
        If e.KeyValue = 38 Then 'SendKeys.Send("+{TAB}")
            txt_Speed.Focus()

        End If
        If e.KeyValue = 40 Then

            If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                    save_record()
                Else
                    txt_MachineNo.Focus()
                End If

        End If

    End Sub


    Private Sub txt_Efficiency_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Efficiency.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
        If Asc(e.KeyChar) = 13 Then

            If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                    save_record()
                Else
                    txt_MachineNo.Focus()
                End If

        End If

    End Sub

    Private Sub btn_FilterClose_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_FilterClose.Click

        pnl_back.Enabled = True
        grp_Filter.Visible = False
    End Sub

    Private Sub btn_FindOpen_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_FindOpen.Click
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim movid As Integer

        If Trim(cbo_Find.Text) = "" Then
            MessageBox.Show("Invalid Machine No", "DOES NOT FIND...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If cbo_Find.Visible And cbo_Find.Enabled Then cbo_Find.Focus()
            Exit Sub
        End If

        da = New SqlClient.SqlDataAdapter("select Machine_Details_IdNo from OE_Machine_Details where Machine_Details_No = '" & Trim(cbo_Find.Text) & "'", con)
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

        pnl_back.Enabled = True
        grp_find.Visible = False

    End Sub
    Private Sub cbo_Find_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Find.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "OE_Machine_details", "Machine_Details_No", "Department_IdNo = " & Val(Dep_Id) & "", "(Machine_Details_IdNo = 0)")
    End Sub

    Private Sub cbo_Find_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Find.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Find, Nothing, Nothing, "OE_Machine_details", "Machine_details_No", "Department_IdNo = " & Val(Dep_Id) & "", "(Machine_Details_IdNo = 0)")
    End Sub
    Private Sub cbo_Find_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Find.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Find, Nothing, "OE_Machine_details", "Machine_Details_No", "Department_IdNo = " & Val(Dep_Id) & "", "(Machine_Details_idNo=0)")
        If Asc(e.KeyChar) = 13 Then
            btn_FindOpen_Click(sender, e)
        End If
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

    Private Sub dgv_filter_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgv_Filter.DoubleClick
        Call btn_Filteropen_Click(sender, e)
    End Sub

    Private Sub dgv_filter_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_Filter.KeyDown
        If e.KeyValue = 13 Then
            Call btn_Filteropen_Click(sender, e)
        End If
    End Sub
    Private Sub cbo_Manufacturer_GotFocus(sender As Object, e As EventArgs) Handles cbo_Manufacturer.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "OE_Manufacture_Head", "Manufacture_Name", "", "(Manufacture_IdNo = 0)")

    End Sub
    Private Sub cbo_Manufacturer_KeyPress(sender As Object, e As KeyPressEventArgs) Handles cbo_Manufacturer.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Manufacturer, cbo_Count, "OE_Manufacture_Head", "Manufacture_Name", "", "(Manufacture_IdNo = 0)")
    End Sub
    Private Sub cbo_Manufacturer_KeyDown(sender As Object, e As KeyEventArgs) Handles cbo_Manufacturer.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Manufacturer, cbo_MachineName, cbo_Count, "OE_Manufacture_Head", "Manufacture_Name", "", "(Manufacture_IdNo = 0)")
    End Sub
    Private Sub cbo_Manufacturer_KeyUp(sender As Object, e As KeyEventArgs) Handles cbo_Manufacturer.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then

            Dim f As New OE_Manufacture_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Manufacturer.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If
    End Sub
    Private Sub cbo_Count_Leave(sender As Object, e As EventArgs) Handles cbo_Count.Leave
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable

        da = New SqlClient.SqlDataAdapter("select Count_Hank from OE_Count_Details Where Count_Idno = '" & Trim(Common_Procedures.Count_NameToIdNo(con, cbo_Count.Text)) & "'and Department_IdNo = " & Str(Val(Dep_Id)), con)
        da.Fill(dt)

        If dt.Rows.Count > 0 Then
            If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                txt_CountHank.Text = dt.Rows(0)(0).ToString
            End If
        End If
    End Sub

    Private Sub txt_Speed_KeyDown(sender As Object, e As KeyEventArgs) Handles txt_Speed.KeyDown
        If e.KeyCode = 38 Then

            txt_CountHank.Focus()

        ElseIf e.KeyCode = 40 Then

            txt_Efficiency.Focus()
        End If
    End Sub

    Private Sub txt_MachineNo_KeyDown(sender As Object, e As KeyEventArgs) Handles txt_MachineNo.KeyDown

        If e.KeyCode = 40 Then

            cbo_MachineName.Focus()

        End If
    End Sub

    Private Sub txt_MachineNo_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txt_MachineNo.KeyPress
        If Asc(e.KeyChar) = 13 Then
            cbo_MachineName.Focus()

        End If
    End Sub
End Class