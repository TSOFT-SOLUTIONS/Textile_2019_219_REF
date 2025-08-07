Imports System.ComponentModel
Imports System.Data.SqlClient
Imports System.Windows.Forms.VisualStyles.VisualStyleElement.Status
Imports System.Windows.Forms.VisualStyles.VisualStyleElement.TrackBar

Public Class fabric_lotno_creation
    Implements Interface_MDIActions


    Private con As New SqlClient.SqlConnection(Common_Procedures.Connection_String)
    Private New_Entry As Boolean = False

    Private vcbo_KeyDwnVal As Double
    Public dte1 As String = ""
    Public dte2 As String
    Private actcotrl1 As New Control

    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

    End Sub

    Private Sub fabric_lotno_creation_Activated(sender As Object, e As EventArgs) Handles Me.Activated


        Try

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(Cbo_ClothName.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "CLOTH" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                Cbo_ClothName.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_warp_count.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "Count" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_warp_count.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_warp_millname.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "Mill" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_warp_millname.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_weft_count.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "Count" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_weft_count.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If


            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_weft_millname.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "Mill" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_weft_millname.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

        Catch ex As Exception

        End Try


    End Sub

    Private Sub fabric_lotno_creation_Load(sender As Object, e As EventArgs) Handles Me.Load

        con.Open()



        AddHandler msk_date.GotFocus, AddressOf controlgotfocus
        AddHandler txt_warp_lotno.GotFocus, AddressOf controlgotfocus
        AddHandler txt_fabric_lotno.GotFocus, AddressOf controlgotfocus
        AddHandler txt_Weft_LotNo.GotFocus, AddressOf controlgotfocus


        AddHandler Cbo_ClothName.GotFocus, AddressOf controlgotfocus
        AddHandler cbo_warp_count.GotFocus, AddressOf controlgotfocus
        AddHandler cbo_warp_millname.GotFocus, AddressOf controlgotfocus
        AddHandler cbo_weft_count.GotFocus, AddressOf controlgotfocus
        AddHandler cbo_weft_millname.GotFocus, AddressOf controlgotfocus
        AddHandler cbo_Find.GotFocus, AddressOf controlgotfocus



        AddHandler msk_date.LostFocus, AddressOf controllostfocus
        AddHandler txt_warp_lotno.LostFocus, AddressOf controllostfocus
        AddHandler txt_fabric_lotno.LostFocus, AddressOf controllostfocus
        AddHandler txt_Weft_LotNo.LostFocus, AddressOf controllostfocus


        AddHandler Cbo_ClothName.LostFocus, AddressOf controllostfocus
        AddHandler cbo_warp_count.LostFocus, AddressOf controllostfocus
        AddHandler cbo_warp_millname.LostFocus, AddressOf controllostfocus
        AddHandler cbo_weft_count.LostFocus, AddressOf controllostfocus
        AddHandler cbo_weft_millname.LostFocus, AddressOf controllostfocus
        AddHandler cbo_Find.LostFocus, AddressOf controllostfocus


        ''''''''''''''''''''
        Me.Left = 400
        Me.Top = 25
        lbl_RefNo.ForeColor = Color.Red
        lbl_RefNo.BackColor = Color.White

        '''''''''''''''''''''

        grp_Find.Visible = False
        grp_Filter.Visible = False

        msk_date.Text = Date.Now.ToShortDateString

        ''''''''''''''''


        new_record()

        If msk_date.Visible And msk_date.Enabled Then msk_date.Focus()

    End Sub
    Private Sub clear()

        New_Entry = True
        lbl_RefNo.ForeColor = Color.Black

        Cbo_ClothName.Text = ""
        cbo_warp_count.Text = ""
        cbo_warp_millname.Text = ""
        txt_warp_lotno.Text = ""
        cbo_weft_count.Text = ""
        cbo_weft_millname.Text = ""
        txt_fabric_lotno.Text = ""
        txt_Weft_LotNo.Text = ""

    End Sub
    Private Sub controlgotfocus(ByVal sender As Object, ByVal e As EventArgs)
        Dim txt As TextBox
        Dim cbo As ComboBox
        Dim msktxbx As MaskedTextBox

        If TypeOf Me.ActiveControl Is TextBox Or TypeOf Me.ActiveControl Is ComboBox Or TypeOf Me.ActiveControl Is MaskedTextBox Then
            Me.ActiveControl.ForeColor = Color.Black
            Me.ActiveControl.BackColor = Color.Lime
        End If
        If TypeOf Me.ActiveControl Is TextBox Then
            txt = Me.ActiveControl
            txt.SelectAll()

        ElseIf TypeOf Me.ActiveControl Is ComboBox Then
            cbo = Me.ActiveControl
            cbo.SelectAll()
        ElseIf TypeOf Me.ActiveControl Is MaskedTextBox Then
            msktxbx = Me.ActiveControl
            msktxbx.SelectionStart = 0

        End If

        actcotrl1 = Me.ActiveControl

    End Sub
    Private Sub controllostfocus(ByVal sender As Object, ByVal e As EventArgs)
        On Error Resume Next
        If IsNothing(actcotrl1) = False Then
            If TypeOf actcotrl1 Is TextBox Or TypeOf actcotrl1 Is ComboBox Or TypeOf actcotrl1 Is MaskedTextBox Then
                actcotrl1.ForeColor = Color.Black
                actcotrl1.BackColor = Color.White
            End If
        End If
    End Sub


    Private Sub move_record(ByVal num As Integer)

        Dim da As SqlClient.SqlDataAdapter
        Dim dt As DataTable

        Dim VCONDTION As String

        New_Entry = False

        lbl_RefNo.ForeColor = Color.Black

        If Val(num) = 0 Then Exit Sub

        VCONDTION = num


        Try


            da = New SqlClient.SqlDataAdapter("select * from Fabric_LotNo_Head where ref_no='" & Trim(VCONDTION) & "'", con)
            dt = New DataTable
            da.Fill(dt)


            If dt.Rows.Count > 0 Then

                lbl_RefNo.Text = (dt.Rows(0).Item("Ref_No").ToString)

                dtp_Date.Text = dt.Rows(0).Item("Ref_Date")

                msk_date.Text = dtp_Date.Text


                Cbo_ClothName.Text = Common_Procedures.Cloth_IdNoToName(con, Val(dt.Rows(0).Item("Cloth_IdNo")).ToString)

                cbo_warp_count.Text = Common_Procedures.Count_IdNoToName(con, Val(dt.Rows(0).Item("Warp_Count_IdNo")).ToString)

                cbo_warp_millname.Text = Common_Procedures.Mill_IdNoToName(con, Val(dt.Rows(0).Item("Warp_Mill_IdNo")).ToString)

                txt_warp_lotno.Text = Trim(dt.Rows(0).Item("Warp_LotNo").ToString)

                cbo_weft_count.Text = Common_Procedures.Count_IdNoToName(con, Val(dt.Rows(0).Item("Weft_Count_IdNo")).ToString)

                cbo_weft_millname.Text = Common_Procedures.Mill_IdNoToName(con, Val(dt.Rows(0).Item("Weft_Mill_IdNo")).ToString)

                txt_fabric_lotno.Text = Trim(dt.Rows(0).Item("Fabric_LotNo").ToString)

                txt_Weft_LotNo.Text = Trim(dt.Rows(0).Item("Weft_LotNo").ToString)


            End If



            dt.Clear()



            If msk_date.Enabled And msk_date.Visible Then msk_date.Focus()

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT MOVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub
    Public Sub new_record() Implements Interface_MDIActions.new_record


        clear()

        New_Entry = True
        lbl_RefNo.ForeColor = Color.Red
        lbl_RefNo.Text = Common_Procedures.get_MaxIdNo(con, "Fabric_LotNo_Head", "ref_no", "")

        msk_date.Focus()


    End Sub

    Public Sub open_record() Implements Interface_MDIActions.open_record
        '  Throw New NotImplementedException()

        Dim da As SqlDataAdapter
        Dim dt As DataTable


        da = New SqlClient.SqlDataAdapter("select fabric_lotno from Fabric_LotNo_Head order by fabric_lotno", con)
        dt = New DataTable

        da.Fill(dt)

        cbo_Find.DataSource = dt
        cbo_Find.DisplayMember = "fabric_lotno"
        cbo_Find.SelectedIndex = -1

        new_record()

        grp_Find.Visible = True
        pnl_Back.Enabled = False

        If cbo_Find.Enabled And cbo_Find.Visible Then cbo_Find.Focus()


        'grp_Find.Left = (Me.Width + grp_Find.Width + 200) \ 2
        'grp_Find.Top = (Me.Top + grp_Find.Top) \ 2


        grp_Find.Left = 60 / 4
        grp_Find.Top = 300

    End Sub

    Public Sub insert_record() Implements Interface_MDIActions.insert_record
        'MessageBox.Show("insert record")
    End Sub

    Public Sub print_record() Implements Interface_MDIActions.print_record
        '--- No Printing
    End Sub

    Public Sub save_record() Implements Interface_MDIActions.save_record

        Dim Da As New SqlClient.SqlDataAdapter
        Dim cmd As New SqlClient.SqlCommand
        Dim tr As SqlClient.SqlTransaction
        Dim surname As String
        Dim cloth_id As Integer = 0
        Dim warp_id As Integer = 0
        Dim warp_mill_id As Integer = 0
        Dim weft_id As Integer = 0
        Dim weft_mill_id As Integer = 0

        If pnl_Back.Enabled = False Then
            MessageBox.Show("Close other window", "DOES NOT SAVE", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If


        If IsDate(msk_date.Text) = False Then
            MessageBox.Show("Invalid Date", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If msk_date.Enabled Then msk_date.Focus()
            Exit Sub
        End If


        'If Trim(Cbo_ClothName.Text) = "" Then
        '    MessageBox.Show("Invalid Sort Name", "DOES NOT SAVE", MessageBoxButtons.OK, MessageBoxIcon.Error)
        '    Cbo_ClothName.Focus()
        '    Exit Sub
        'End If


        If Trim(txt_fabric_lotno.Text) = "" Then
            MessageBox.Show("Invalid FabricLot_No", "DOES NOT SAVE", MessageBoxButtons.OK, MessageBoxIcon.Error)
            txt_fabric_lotno.Focus()

            Exit Sub
        End If

        cloth_id = Common_Procedures.Cloth_NameToIdNo(con, Cbo_ClothName.Text)

        warp_id = Common_Procedures.Count_NameToIdNo(con, cbo_warp_count.Text)

        warp_mill_id = Common_Procedures.Mill_NameToIdNo(con, cbo_warp_millname.Text)

        weft_id = Common_Procedures.Count_NameToIdNo(con, cbo_weft_count.Text)

        weft_mill_id = Common_Procedures.Mill_NameToIdNo(con, cbo_weft_millname.Text)

        surname = Common_Procedures.Remove_NonCharacters(Trim(txt_fabric_lotno.Text))


        tr = con.BeginTransaction

        Try

            cmd.Connection = con
            cmd.Transaction = tr


            cmd.Parameters.Clear()
            cmd.Parameters.AddWithValue("@datetime", Convert.ToDateTime(msk_date.Text))

            If New_Entry = True Then


                lbl_RefNo.Text = Common_Procedures.get_MaxIdNo(con, "Fabric_LotNo_Head", "ref_no", "", tr)

                cmd.CommandText = "Insert into  Fabric_LotNo_Head     (ref_no ,                                        Ref_Date,                               Fabric_LotNo,                                                 Sur_Name,                                Cloth_IdNo,                            Warp_Count_IdNo,                                 Warp_Mill_IdNo,                             Warp_LotNo,                                Weft_Count_IdNo,                        Weft_Mill_IdNo,                         Weft_LotNo)" &
                                                            " values  (" & Str(Val(lbl_RefNo.Text)) & "  ,           @datetime  ,                   '" & Trim(txt_fabric_lotno.Text) & "',                       '" & Trim(surname) & "' ,                  " & Val(cloth_id) & " ,                 " & Str(Val(warp_id)) & ",                     " & Str(Val(warp_mill_id)) & ",      '" & Trim(txt_warp_lotno.Text) & "',              " & Str(Val(weft_id)) & " ,       " & Str(Val(weft_mill_id)) & " ,  '" & Trim(txt_Weft_LotNo.Text) & "' )"
                cmd.ExecuteNonQuery()

                lbl_RefNo.ForeColor = Color.Black


            Else
                cmd.CommandText = "update fabric_lotno_head set Ref_Date=@datetime,  Sur_Name = '" & Trim(txt_fabric_lotno.Text) & "',  Cloth_IdNo=" & Str(Val(cloth_id)) & ",  Warp_Count_IdNo=" & Str(Val(warp_id)) & ",  Warp_Mill_IdNo=" & Str(Val(warp_mill_id)) & " , Warp_LotNo='" & Trim(txt_warp_lotno.Text) & "',Weft_Count_IdNo=" & Str(Val(weft_id)) & ",  Weft_Mill_IdNo=" & Str(Val(weft_mill_id)) & "  , Weft_LotNo='" & Trim(txt_Weft_LotNo.Text) & "' ,  Fabric_LotNo = '" & Trim(txt_fabric_lotno.Text) & "' where ref_no = " & Str(Val(lbl_RefNo.Text))
                cmd.ExecuteNonQuery()

            End If


            tr.Commit()

            MessageBox.Show("Saved Successfully!!!", "FOR SAVING....", MessageBoxButtons.OK, MessageBoxIcon.Information)


            If New_Entry = True Then new_record()


            If msk_date.Enabled And msk_date.Visible Then msk_date.Focus()

        Catch ex As Exception

            MessageBox.Show(ex.Message, "SAVING", MessageBoxButtons.OK, MessageBoxIcon.Error)

            tr.Rollback()

        End Try

    End Sub



    Public Sub filter_record() Implements Interface_MDIActions.filter_record

        ' Throw New NotImplementedException()

        Dim da As New SqlClient.SqlDataAdapter("select Ref_No, Fabric_LotNo from Fabric_LotNo_Head where Ref_No <> 0 order by Ref_No", con)
        Dim dt As New DataTable

        da.Fill(dt)

        With dgv_Filter
            .Columns.Clear()
            .DataSource = dt

            .RowHeadersVisible = False

            .AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells
            .AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill

            .Columns(0).HeaderText = "REF NO"          '''''Ref_No
            .Columns(1).HeaderText = "FABRIC LOT NO"    ''''Fabric_LotNo


            .Columns(0).FillWeight = 60
            .Columns(1).FillWeight = 160



        End With

        new_record()

        grp_Filter.Visible = True
        grp_Filter.Left = grp_Find.Left
        grp_Filter.Top = grp_Find.Top

        pnl_Back.Enabled = False

        If dgv_Filter.Enabled And dgv_Filter.Visible Then dgv_Filter.Focus()

        grp_Filter.Left = 60 / 4
        grp_Filter.Top = 260

        dt.Dispose()
        da.Dispose()

    End Sub

    Private Sub delete_record() Implements Interface_MDIActions.delete_record
        'Throw New NotImplementedException()
        Dim cmd As New SqlClient.SqlCommand
        Dim da As SqlClient.SqlDataAdapter
        Dim Dt As DataTable

        If pnl_Back.Enabled = False Then
            MessageBox.Show("Close other window", "DOES NOT SAVE", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If


        If New_Entry = True Then
            MessageBox.Show("This is New Entry", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If


        If MessageBox.Show("Do you want to Delete ?", "FOR DELETION....", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) <> Windows.Forms.DialogResult.Yes Then
            Exit Sub
        End If

        Try
            cmd.Connection = con

            cmd.CommandText = "delete from Fabric_LotNo_Head where ref_no = '" & Trim(lbl_RefNo.Text) & "' "
            cmd.ExecuteNonQuery()


            MessageBox.Show("Deleted Successfully!!!", "FOR DELETION...", MessageBoxButtons.OK, MessageBoxIcon.Information)



            new_record()

            msk_date.Focus()




        Catch ex As Exception

            MessageBox.Show(ex.Message, "DELETE", MessageBoxButtons.OK, MessageBoxIcon.Error)



        End Try
    End Sub

    Public Sub movefirst_record() Implements Interface_MDIActions.movefirst_record
        'Throw New NotImplementedException()
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim movid As Integer = 0

        Try
            da = New SqlClient.SqlDataAdapter("select min(ref_no) from Fabric_LotNo_Head Where ref_no <> 0", con)
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

    Private Sub movenext_record() Implements Interface_MDIActions.movenext_record
        '   Throw New NotImplementedException()

        Dim da As SqlClient.SqlDataAdapter
        Dim dt As DataTable
        Dim moveno As String


        New_Entry = False



        Try


            da = New SqlClient.SqlDataAdapter("select top 1 * from Fabric_LotNo_Head where ref_no >'" & Trim(lbl_RefNo.Text) & "' order by ref_no asc", con)
            dt = New DataTable
            da.Fill(dt)


            If dt.Rows.Count > 0 Then

                moveno = Val(dt.Rows(0)(0).ToString)
            End If
            move_record(moveno)



        Catch ex As Exception

        End Try
    End Sub

    Private Sub moveprevious_record() Implements Interface_MDIActions.moveprevious_record
        '  Throw New NotImplementedException()

        Dim da As SqlClient.SqlDataAdapter
        Dim dt As DataTable
        Dim moveno As String


        New_Entry = False

        Try

            da = New SqlClient.SqlDataAdapter("select top 1 * from Fabric_LotNo_Head where ref_no < '" & Trim(lbl_RefNo.Text) & "' order by ref_no desc", con)
            dt = New DataTable
            da.Fill(dt)


            If dt.Rows.Count > 0 Then

                moveno = Val(dt.Rows(0)(0).ToString)


            End If

            move_record(moveno)

        Catch ex As Exception

        End Try
    End Sub
    Public Sub movelast_record() Implements Interface_MDIActions.movelast_record
        '   Throw New NotImplementedException()
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim movid As Integer = 0

        Try
            da = New SqlClient.SqlDataAdapter("select MAX(ref_no) from Fabric_LotNo_Head Where ref_no <> 0", con)
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
    Private Sub btn_Save_Click(sender As Object, e As EventArgs) Handles btn_Save.Click
        save_record()
    End Sub

    Private Sub btn_Close_Click(sender As Object, e As EventArgs) Handles btn_Close.Click
        con.Close()
        Me.Close()
        Me.Dispose()
    End Sub
    Private Sub btn_Find_Click(sender As Object, e As EventArgs) Handles btn_Find.Click
        Dim da As SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim moveno As Integer

        da = New SqlClient.SqlDataAdapter("Select * from Fabric_LotNo_Head where Fabric_LotNo ='" & Trim(cbo_Find.Text) & "'", con)
        dt = New DataTable
        da.Fill(dt)

        If dt.Rows.Count > 0 Then
            moveno = Val(dt.Rows(0)(0).ToString)

        End If
        If Val(moveno) <> 0 Then

            move_record(moveno)
        Else
            new_record()

        End If

        grp_Find.Visible = False
        grp_Filter.Visible = False
        pnl_Back.Enabled = True



        If msk_date.Enabled And msk_date.Visible Then msk_date.Focus()

    End Sub

    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click


        grp_Find.Visible = False
        grp_Filter.Visible = False
        pnl_Back.Enabled = True


        If msk_date.Enabled And msk_date.Visible Then msk_date.Focus()


    End Sub

    Private Sub btn_Open_Click(sender As Object, e As EventArgs) Handles btn_Open.Click

        Dim moveno As Integer

        moveno = 0

        If IsDBNull((dgv_Filter.CurrentRow.Cells(0).Value)) = False Then
            moveno = Val(dgv_Filter.CurrentRow.Cells(0).Value)

        End If
        If Val(moveno) <> 0 Then
            move_record(moveno)
            btn_CloseFilter_Click(sender, e)
        End If

    End Sub

    Private Sub btn_CloseFilter_Click(sender As Object, e As EventArgs) Handles btn_CloseFilter.Click

        btnClose_Click(sender, e)

    End Sub
    Private Sub dgv_Filter_DoubleClick(sender As Object, e As EventArgs) Handles dgv_Filter.DoubleClick

        btn_Open_Click(sender, e)

    End Sub
    Private Sub dgv_Filter_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_Filter.KeyDown
        If e.KeyCode = Keys.Enter Then
            btn_Open_Click(sender, e)
        End If
    End Sub

    Private Sub fabric_lotno_creation_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If Asc(e.KeyChar) = 27 Then
            If grp_Find.Visible Then
                btnClose_Click(sender, e)
            ElseIf grp_Filter.Visible Then
                btn_CloseFilter_Click(sender, e)
            Else
                Me.Close()
            End If
        End If
    End Sub
    Private Sub msk_date_KeyUp(sender As Object, e As KeyEventArgs) Handles msk_date.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            msk_date.Text = Date.Today
        End If

        If IsDate(msk_date.Text) = True Then
            If e.KeyCode = 107 Then
                msk_date.Text = DateAdd("D", 1, Convert.ToDateTime(msk_date.Text))
            ElseIf e.KeyCode = 109 Then
                msk_date.Text = DateAdd("D", -1, Convert.ToDateTime(msk_date.Text))
            End If
        End If
    End Sub

    Private Sub msk_date_LostFocus(sender As Object, e As EventArgs) Handles msk_date.LostFocus

        If IsDate(msk_date.Text) = True Then

            If Microsoft.VisualBasic.DateAndTime.Day(Convert.ToDateTime(msk_date.Text)) <= 31 Or Microsoft.VisualBasic.DateAndTime.Month(Convert.ToDateTime(msk_date.Text)) <= 31 Then
                If Microsoft.VisualBasic.DateAndTime.Year(Convert.ToDateTime(msk_date.Text)) <= 2000 And Microsoft.VisualBasic.DateAndTime.Year(Convert.ToDateTime(msk_date.Text)) >= 2060 Then
                    dtp_Date.Value = Convert.ToDateTime(msk_date.Text)
                End If
            End If
        End If
    End Sub
    Private Sub dtp_Date_TextChanged(sender As Object, e As EventArgs) Handles dtp_Date.TextChanged

        Try

            If Me.ActiveControl.Name <> msk_date.Name Then
                If IsDate(dtp_Date.Text) = True Then
                    msk_date.Text = dtp_Date.Text
                    msk_date.SelectionStart = 0
                End If
            End If

        Catch ex As Exception
            '---

        End Try
    End Sub

    Private Sub msk_date_KeyPress(sender As Object, e As KeyPressEventArgs) Handles msk_date.KeyPress
        If Asc(e.KeyChar) = 13 Then
            e.Handled = True
            cbo_warp_count.Focus()
        End If
    End Sub
    Private Sub msk_date_KeyDown(sender As Object, e As KeyEventArgs) Handles msk_date.KeyDown

        vcbo_KeyDwnVal = e.KeyValue
        dte1 = ""
        dte2 = -1

        If e.KeyCode = 46 Or e.KeyCode = 8 Then
            dte1 = msk_date.Text
            dte2 = msk_date.SelectionStart

        ElseIf e.KeyCode = 40 Then
            e.Handled = True
            e.SuppressKeyPress = True
            cbo_warp_count.Focus()

        ElseIf e.KeyCode = 38 Then
            e.Handled = True
            e.SuppressKeyPress = True
            txt_fabric_lotno.Focus()

        End If
    End Sub

    Private Sub Cbo_ClothName_KeyPress(sender As Object, e As KeyPressEventArgs) Handles Cbo_ClothName.KeyPress

        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, Cbo_ClothName, cbo_warp_count, "cloth_head", "Cloth_Name", "", "(Cloth_Idno = 0)")

    End Sub



    Private Sub Cbo_ClothName_KeyDown(sender As Object, e As KeyEventArgs) Handles Cbo_ClothName.KeyDown
        vcbo_KeyDwnVal = e.KeyValue

        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, Cbo_ClothName, msk_date, cbo_warp_count, "cloth_head", "Cloth_Name", "", "(Cloth_Idno = 0)")

    End Sub
    Private Sub Cbo_ClothName_KeyUp(sender As Object, e As KeyEventArgs) Handles Cbo_ClothName.KeyUp

        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then

            Dim f As New Cloth_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = Cbo_ClothName.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If
    End Sub

    Private Sub Cbo_ClothName_GotFocus(sender As Object, e As EventArgs) Handles Cbo_ClothName.GotFocus

        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "cloth_head", "Cloth_Name", "", "(Cloth_Idno = 0)")

    End Sub

    Private Sub cbo_warp_count_KeyPress(sender As Object, e As KeyPressEventArgs) Handles cbo_warp_count.KeyPress


        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_warp_count, cbo_warp_millname, "Count_Head", "Count_Name", "", "(Count_Idno = 0)")

    End Sub



    Private Sub cbo_warp_count_KeyDown(sender As Object, e As KeyEventArgs) Handles cbo_warp_count.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_warp_count, msk_date, cbo_warp_millname, "Count_Head", "Count_Name", "", "(Count_Idno = 0)")
    End Sub
    Private Sub cbo_warp_count_KeyUp(sender As Object, e As KeyEventArgs) Handles cbo_warp_count.KeyUp

        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then

            Dim f As New Count_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_warp_count.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.show()


        End If

    End Sub
    Private Sub cbo_warp_count_GotFocus(sender As Object, e As EventArgs) Handles cbo_warp_count.GotFocus

        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Count_Head", "Count_Name", "", "(Count_Idno = 0)")

    End Sub
    Private Sub cbo_warp_millname_KeyPress(sender As Object, e As KeyPressEventArgs) Handles cbo_warp_millname.KeyPress


        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_warp_millname, txt_warp_lotno, "Mill_Head", "Mill_Name", "", "(Mill_IdNo = 0)")

    End Sub
    Private Sub cbo_warp_millname_KeyDown(sender As Object, e As KeyEventArgs) Handles cbo_warp_millname.KeyDown

        vcbo_KeyDwnVal = e.KeyValue


        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_warp_millname, cbo_warp_count, txt_warp_lotno, "Mill_Head", "Mill_Name", "", "(Mill_IdNo = 0)")

    End Sub
    Private Sub cbo_warp_millname_KeyUp(sender As Object, e As KeyEventArgs) Handles cbo_warp_millname.KeyUp

        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then

            Dim f As New Mill_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_warp_millname.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.mdiparent = MDIParent1
            f.show()

        End If
    End Sub
    Private Sub cbo_warp_millname_GotFocus(sender As Object, e As EventArgs) Handles cbo_warp_millname.GotFocus

        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Mill_Head", "Mill_Name", "", "(Mill_IdNo = 0)")

    End Sub
    Private Sub txt_warp_lotno_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txt_warp_lotno.KeyPress


        If Asc(e.KeyChar) = 13 Then
            cbo_weft_count.Focus()

        End If

    End Sub
    Private Sub txt_warp_lotno_KeyDown(sender As Object, e As KeyEventArgs) Handles txt_warp_lotno.KeyDown

        vcbo_KeyDwnVal = e.KeyValue

        If e.KeyValue = 38 Then
            cbo_warp_millname.Focus()

        ElseIf e.KeyValue = 40 Then
            cbo_weft_count.Focus()

        End If
    End Sub


    Private Sub cbo_weft_count_KeyPress(sender As Object, e As KeyPressEventArgs) Handles cbo_weft_count.KeyPress

        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_weft_count, cbo_weft_millname, "Count_Head", "Count_Name", "", "(Count_Idno = 0)")

    End Sub

    Private Sub cbo_weft_count_KeyDown(sender As Object, e As KeyEventArgs) Handles cbo_weft_count.KeyDown

        vcbo_KeyDwnVal = e.KeyValue

        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_weft_count, txt_warp_lotno, cbo_weft_millname, "Count_Head", "Count_Name", "", "(Count_Idno = 0)")

    End Sub
    Private Sub cbo_weft_count_KeyUp(sender As Object, e As KeyEventArgs) Handles cbo_weft_count.KeyUp

        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then

            Dim f As New Count_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_weft_count.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.mdiparent = MDIParent1
            f.show()

        End If
    End Sub
    Private Sub cbo_weft_count_GotFocus(sender As Object, e As EventArgs) Handles cbo_weft_count.GotFocus

        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Count_Head", "Count_Name", "", "(Count_Idno = 0)")

    End Sub

    Private Sub cbo_weft_millname_KeyPress(sender As Object, e As KeyPressEventArgs) Handles cbo_weft_millname.KeyPress

        'item_select_keypress(sender, e, cbo_weft_millname, Nothing, con, txt_fabric_lotno)
        'txt_caps(sender, e)

        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_weft_millname, txt_Weft_LotNo, "Mill_Head", "Mill_Name", "", "(Mill_IdNo = 0)")

    End Sub



    Private Sub cbo_weft_millname_KeyDown(sender As Object, e As KeyEventArgs) Handles cbo_weft_millname.KeyDown


        vcbo_KeyDwnVal = e.KeyValue

        '   item_selection_keydown(sender, e, cbo_weft_millname, Nothing, con, txt_fabric_lotno, cbo_weft_count)

        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_weft_millname, cbo_weft_count, txt_Weft_LotNo, "Mill_Head", "Mill_Name", "", "(Mill_IdNo = 0)")

    End Sub

    Private Sub cbo_weft_millname_KeyUp(sender As Object, e As KeyEventArgs) Handles cbo_weft_millname.KeyUp


        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then

            Dim f As New Mill_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_weft_millname.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If
    End Sub
    Private Sub cbo_weft_millname_GotFocus(sender As Object, e As EventArgs) Handles cbo_weft_millname.GotFocus

        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Mill_Head", "Mill_Name", "", "(Mill_IdNo = 0)")

    End Sub
    Private Sub txt_Weft_LotNo_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txt_Weft_LotNo.KeyPress

        If Asc(e.KeyChar) = 13 Then
            e.Handled = True
            txt_fabric_lotno.Focus()
        End If
    End Sub

    Private Sub txt_Weft_LotNo_KeyDown(sender As Object, e As KeyEventArgs) Handles txt_Weft_LotNo.KeyDown

        If e.KeyValue = 38 Then
            cbo_weft_millname.Focus()

        ElseIf e.KeyValue = 40 Then
            txt_fabric_lotno.Focus()

        End If
    End Sub
    Private Sub txt_fabric_lotno_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txt_fabric_lotno.KeyPress

        If Asc(e.KeyChar) = 13 Then
            e.Handled = True
            If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                save_record()
            Else

                msk_date.Focus()
            End If


        End If

    End Sub
    Private Sub txt_fabric_lotno_KeyDown(sender As Object, e As KeyEventArgs) Handles txt_fabric_lotno.KeyDown

        vcbo_KeyDwnVal = e.KeyValue

        If e.KeyValue = 38 Then
            e.Handled = True
            txt_Weft_LotNo.Focus()

        ElseIf e.KeyValue = 40 Then
            e.Handled = True
            btn_Save.Focus()

        End If

    End Sub

    Private Sub cbo_Find_GotFocus(sender As Object, e As EventArgs) Handles cbo_Find.GotFocus

        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Fabric_LotNo_Head", "Fabric_LotNo", "", "")

    End Sub
    Private Sub cbo_Find_KeyDown(sender As Object, e As KeyEventArgs) Handles cbo_Find.KeyDown

        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Find, Nothing, Nothing, "Fabric_LotNo_Head", "Fabric_LotNo", "", "")

    End Sub

    Private Sub cbo_Find_KeyPress(sender As Object, e As KeyPressEventArgs) Handles cbo_Find.KeyPress

        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Find, Nothing, "Fabric_LotNo_Head", "Fabric_LotNo", "", "")

        If Asc(e.KeyChar) = 13 Then btn_Find_Click(sender, e)

    End Sub

End Class