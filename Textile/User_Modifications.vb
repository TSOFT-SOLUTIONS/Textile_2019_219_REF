Public Class User_Modifications

    Private Con As New SqlClient.SqlConnection(Common_Procedures.Connection_String)

    Private FrmLdSTS As Boolean = False
    Public Entry_Name As String = ""
    Public Entry_PkValue As String = ""
    Private Prec_ActCtrl As New Control

    Public Sub New()
        FrmLdSTS = True
        ' This call is required by the designer.
        InitializeComponent()
        ' Add any initialization after the InitializeComponent() call.
    End Sub

    Private Sub clear()
        pnl_Back.Enabled = True
        dgv_Details.Rows.Clear()
        dtp_FilterFrom_date.Text = Common_Procedures.Company_FromDate
        dtp_FilterTo_date.Text = Common_Procedures.Company_ToDate
       
        cbo_UserName.Text = ""
        cbo_ColumnName.Text = ""
        cbo_Slno.Text = ""
        cbo_Add.Text = ""
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

    Private Sub ControlGotFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim txtbx As TextBox
        Dim combobx As ComboBox
        Dim chkbx As ComboBox
        On Error Resume Next

        If TypeOf Me.ActiveControl Is TextBox Or TypeOf Me.ActiveControl Is ComboBox Or TypeOf Me.ActiveControl Is CheckBox Then
            Me.ActiveControl.BackColor = Color.Lime ' Color.MistyRose ' Color.PaleGreen
            Me.ActiveControl.ForeColor = Color.Blue
        End If

        If TypeOf Me.ActiveControl Is TextBox Then
            txtbx = Me.ActiveControl
            txtbx.SelectAll()

        ElseIf TypeOf Me.ActiveControl Is ComboBox Then
            combobx = Me.ActiveControl
            combobx.SelectAll()

        ElseIf TypeOf Me.ActiveControl Is CheckBox Then
            chkbx = Me.ActiveControl
            chkbx.SelectAll()

        End If


        Prec_ActCtrl = Me.ActiveControl

    End Sub
   
    Private Sub ControlLostFocus(ByVal sender As Object, ByVal e As System.EventArgs)

        On Error Resume Next

        If IsNothing(Prec_ActCtrl) = False Then
            If TypeOf Prec_ActCtrl Is TextBox Or TypeOf Prec_ActCtrl Is ComboBox Then
                Prec_ActCtrl.BackColor = Color.White
                Prec_ActCtrl.ForeColor = Color.Black
            ElseIf TypeOf Prec_ActCtrl Is CheckBox Then
                Prec_ActCtrl.BackColor = Color.LightSkyBlue
                Prec_ActCtrl.ForeColor = Color.Black

            End If
        End If

    End Sub

    Private Sub User_Modifications_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Me.Text = Trim(UCase(Entry_Name)) & "  USER MODOFICATION DETAILS     -   ENTRY NO : " & Trim(UCase(Entry_PkValue))


        If UCase(Trim(Entry_Name)) = "USER_CREATION" Then
            Con = New SqlClient.SqlConnection(Common_Procedures.ConnectionString_CompanyGroupdetails)
        End If

        Con.Open()

        FrmLdSTS = True
        clear()

        'MessageBox.Show(dgv_Details.Columns.Count())

        Display_Details(1, "")

        If Entry_Name = "Weight_Bridge_Entry" Then
            opt_FirstWeight.Visible = True
            opt_SecondWeight.Visible = True
            Panel2.Width = 370
        Else
            opt_FirstWeight.Visible = False
            opt_SecondWeight.Visible = False
            Panel2.Width = 150
        End If

        cbo_Add.Items.Clear()
        cbo_Add.Items.Add("")
        cbo_Add.Items.Add("ADD")
        cbo_Add.Items.Add("DELETE")
        cbo_Add.Items.Add("EDIT")

        AddHandler cbo_ColumnName.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_UserName.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Add.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Slno.GotFocus, AddressOf ControlGotFocus
        AddHandler dtp_FilterFrom_date.GotFocus, AddressOf ControlGotFocus
        AddHandler dtp_FilterTo_date.GotFocus, AddressOf ControlGotFocus

        AddHandler cbo_ColumnName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_UserName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Add.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Slno.LostFocus, AddressOf ControlLostFocus
        AddHandler dtp_FilterFrom_date.LostFocus, AddressOf ControlLostFocus
        AddHandler dtp_FilterTo_date.LostFocus, AddressOf ControlLostFocus

        AddHandler dtp_FilterFrom_date.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler dtp_FilterTo_date.KeyDown, AddressOf TextBoxControlKeyDown

        AddHandler dtp_FilterFrom_date.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler dtp_FilterTo_date.KeyPress, AddressOf TextBoxControlKeyPress
    End Sub

    Private Sub User_Modifications_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated

        If FrmLdSTS = True Then
            With dgv_Details
                If .Visible = True And .Enabled = True Then
                    If .Rows.Count = 0 Then .Rows.Add()
                    .Focus()
                    .CurrentCell = .Rows(0).Cells(5)
                End If
                If .Rows.Count > 0 Then
                    .Focus()
                    .CurrentCell = .Rows(0).Cells(5)
                End If
            End With
        End If
        FrmLdSTS = False
    End Sub

    Private Sub User_Modifications_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress

        Try
            If Asc(e.KeyChar) = 27 Then
                btn_close_Click(sender, e)
            End If

        Catch ex As Exception
            '---MessageBox.Show(ex.Message, "DOES NOT CLOSE...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Protected Overrides Function ProcessCmdKey(ByRef msg As System.Windows.Forms.Message, ByVal keyData As System.Windows.Forms.Keys) As Boolean
        Dim dgv1 As New DataGridView

        On Error Resume Next

        If ActiveControl.Name = dgv_Details.Name Or TypeOf ActiveControl Is DataGridViewTextBoxEditingControl Then

            dgv1 = Nothing

            If ActiveControl.Name = dgv_Details.Name Then
                dgv1 = dgv_Details

            ElseIf dgv_Details.IsCurrentRowDirty = True Then
                dgv1 = dgv_Details

            Else
                dgv1 = dgv_Details

            End If

            With dgv1
                If keyData = Keys.Enter Or keyData = Keys.Down Then

                    If .CurrentCell.RowIndex = .RowCount - 1 Then
                        '----()

                    Else
                        .CurrentCell = .Rows(.CurrentCell.RowIndex + 1).Cells(0)

                    End If

                    Return True

                ElseIf keyData = Keys.Up Then

                    If .CurrentCell.RowIndex = 0 Then
                        '---()

                    Else
                        .CurrentCell = .Rows(.CurrentCell.RowIndex - 1).Cells(0)

                    End If

                    Return True

                Else
                    Return MyBase.ProcessCmdKey(msg, keyData)

                End If

            End With

        Else

            Return MyBase.ProcessCmdKey(msg, keyData)

        End If

    End Function

    Public Sub Display_Details(ByVal Pending_sts As Integer, ByVal Modify_Sts As String)
        Dim Da1 As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim Dt2 As New DataTable
        Dim I As Integer = 0, J As Integer = 0
        Dim n As Integer = 0
        Dim SNo As Integer = 0
        Dim Us_id As Integer = 0
        Dim MDt As Date
        Dim UsrNm As String = ""
        Dim str1 As String = ""
        Dim Condt As String = ""
        Dim vUsrLog_DBName As String
        Dim vENTRYCODE_Condt As String


        vUsrLog_DBName = Common_Procedures.get_UserModificationDetails_DataBaseName(Val(Common_Procedures.CompGroupIdNo))

        Condt = ""
        vENTRYCODE_Condt = " a.Entry_Code = '" & Trim(Entry_PkValue) & "' and "

        If IsDate(dtp_FilterFrom_date.Value) = True And IsDate(dtp_FilterTo_date.Value) = True Then
            Condt = "a.Modification_DateTime between '" & Trim(Format(dtp_FilterFrom_date.Value, "MM/dd/yyyy")) & "' and '" & Trim(Format(dtp_FilterTo_date.Value, "MM/dd/yyyy")) & "' "
        ElseIf IsDate(dtp_FilterFrom_date.Value) = True Then
            Condt = "a.Modification_DateTime = '" & Trim(Format(dtp_FilterFrom_date.Value, "MM/dd/yyyy")) & "' "
        ElseIf IsDate(dtp_FilterTo_date.Value) = True Then
            Condt = "a. Modification_DateTime= '" & Trim(Format(dtp_FilterTo_date.Value, "MM/dd/yyyy")) & "' "
        End If

        If Trim(cbo_UserName.Text) <> "" Then
            Us_id = Common_Procedures.User_NameToIdNo(Con, cbo_UserName.Text)
        End If
        If Val(Us_id) <> 0 Then
            Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " (a.User_Idno = " & Str(Val(Us_id)) & ")"
        End If

        If Val(cbo_Slno.Text) <> 0 Then
            Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " (a.Details_SlNo = " & Str(Val(cbo_Slno.Text)) & ")"
        End If
        If Trim(cbo_Add.Text) <> "" Then
            Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " (a.Entry_Status = '" & Trim(cbo_Add.Text) & "')"

            If Trim(UCase(cbo_Add.Text)) = Trim(UCase("DELETE")) Then
                vENTRYCODE_Condt = "" ' " a.Entry_Code LIKE '" & Trim(Microsoft.VisualBasic.Left(Entry_PkValue, 6)) & "%' and "
            End If

        End If
        If Trim(cbo_ColumnName.Text) <> "" Then
            Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " (a.Field_Name = '" & Trim(cbo_ColumnName.Text) & "')"
        End If

        If UCase(Trim(Entry_Name)) <> "USER_CREATION" Then

            Da1 = New SqlClient.SqlDataAdapter("Select a.*, b.User_Name from " & Trim(vUsrLog_DBName) & "..User_Modification_Details a, " & Trim(Common_Procedures.CompanyDetailsDataBaseName) & "..User_Head b where a.Entry_Name = '" & Trim(Entry_Name) & "' and " & vENTRYCODE_Condt & " a.User_IdNo = b.User_IdNo  " & IIf(Trim(Modify_Sts) <> "", " AND a.Modification_Status = '" & Modify_Sts & "'", " ") & IIf(Pending_sts = 1, "  ", " and Verified_Status = 0") & " and (Show_Status = 0 or Show_Status = 1)  " & IIf(Trim(Condt) <> "", " and ", "") & Condt & " Order By a.Modification_DateTime desc, b.User_Name, a.Details_SlNo, a.Auto_SlNo, a.Entry_Status", Con)
            'Da1 = New SqlClient.SqlDataAdapter("Select a.*, b.User_Name from " & Trim(vUsrLog_DBName) & "..User_Modification_Details a, " & Trim(Common_Procedures.CompanyDetailsDataBaseName) & "..User_Head b where a.Entry_Name = '" & Trim(Entry_Name) & "' and a.Entry_Code = '" & Trim(Entry_PkValue) & "' and a.User_IdNo = b.User_IdNo  " & IIf(Trim(Modify_Sts) <> "", " AND a.Modification_Status = '" & Modify_Sts & "'", " ") & IIf(Pending_sts = 1, "  ", " and Verified_Status = 0") & " and (Show_Status = 0 or Show_Status = 1)  " & IIf(Trim(Condt) <> "", " and ", "") & Condt & " Order By a.Modification_DateTime desc, b.User_Name, a.Details_SlNo, a.Auto_SlNo, a.Entry_Status", Con)
            Dt1 = New DataTable
            Da1.Fill(Dt1)

            With dgv_Details

                .Rows.Clear()

                If Dt1.Rows.Count > 0 Then

                    For I = 0 To Dt1.Rows.Count - 1

                        If .Rows.Count > 0 Then
                            If Trim(MDt) <> Trim(Dt1.Rows(I).Item("Modification_DateTime")) Or Trim(UCase(UsrNm)) <> Trim(UCase(Dt1.Rows(I).Item("User_Name").ToString)) Then
                                n = .Rows.Add()
                                For J = 0 To .ColumnCount - 1
                                    .Rows(n).Cells(J).Style.BackColor = Color.FromArgb(240, 240, 240)
                                Next
                            End If
                        End If
                        'MessageBox.Show(.Columns.Count())
                        n = .Rows.Add()

                        SNo = SNo + 1

                        .Rows(n).Cells(0).Value = Val(SNo)
                        .Rows(n).Cells(1).Value = Format(Dt1.Rows(I).Item("Modification_DateTime"), "dd-MM-yyyy hh:mm tt")
                        .Rows(n).Cells(2).Value = Dt1.Rows(I).Item("User_Name").ToString
                        .Rows(n).Cells(3).Value = Dt1.Rows(I).Item("Entry_Status").ToString
                        .Rows(n).Cells(4).Value = Dt1.Rows(I).Item("Details_SlNo").ToString
                        If Val(.Rows(n).Cells(4).Value) = 0 Then .Rows(n).Cells(4).Value = ""
                        .Rows(n).Cells(5).Value = Dt1.Rows(I).Item("Field_Name").ToString
                        .Rows(n).Cells(6).Value = Dt1.Rows(I).Item("Field_OldValue").ToString
                        .Rows(n).Cells(7).Value = Dt1.Rows(I).Item("Field_NewValue").ToString
                        .Rows(n).Cells(8).Value = Dt1.Rows(I).Item("Modification_Details").ToString
                        If Val(Dt1.Rows(I).Item("Verified_Status").ToString) = 1 Then
                            .Rows(n).Cells(9).Value = True
                        Else
                            .Rows(n).Cells(9).Value = False
                        End If
                        .Rows(n).Cells(10).Value = Dt1.Rows(I).Item("Auto_SlNo").ToString
                        MDt = Dt1.Rows(I).Item("Modification_DateTime")
                        UsrNm = Dt1.Rows(I).Item("User_Name").ToString

                    Next

                End If

                If .Rows.Count = 0 Then .Rows.Add()

                .Focus()
                .CurrentCell = .Rows(0).Cells(0)
                .CurrentCell.Selected = True

            End With
            Dt1.Clear()

            Dt1.Dispose()
            Da1.Dispose()


        Else


            Da1 = New SqlClient.SqlDataAdapter("Select a.*, b.User_Name from  " & Trim(vUsrLog_DBName) & "..User_Modification_Details a, " & Trim(Common_Procedures.CompanyDetailsDataBaseName) & "..User_Head b where a.Entry_Name = '" & Trim(Entry_Name) & "' and " & Trim(vENTRYCODE_Condt) & " a.User_IdNo = b.User_IdNo  " & IIf(Trim(Modify_Sts) <> "", "AND a.Modification_Status = '" & Modify_Sts & "'", " ") & IIf(Pending_sts = 1, "  ", " and Verified_Status = 0") & " and Show_Status = 1  " & IIf(Trim(Condt) <> "", " and ", "") & Condt & " Order By a.Modification_DateTime desc, b.User_Name, a.Details_SlNo, a.Auto_SlNo, a.Entry_Status", Con)
            'Da1 = New SqlClient.SqlDataAdapter("Select a.*, b.User_Name from  " & Trim(vUsrLog_DBName) & "..User_Modification_Details a, " & Trim(Common_Procedures.CompanyDetailsDataBaseName) & "..User_Head b where a.Entry_Name = '" & Trim(Entry_Name) & "' and a.Entry_Code = '" & Trim(Entry_PkValue) & "' and a.User_IdNo = b.User_IdNo  " & IIf(Trim(Modify_Sts) <> "", "AND a.Modification_Status = '" & Modify_Sts & "'", " ") & IIf(Pending_sts = 1, "  ", " and Verified_Status = 0") & " and Show_Status = 1  " & IIf(Trim(Condt) <> "", " and ", "") & Condt & " Order By a.Modification_DateTime desc, b.User_Name, a.Details_SlNo, a.Auto_SlNo, a.Entry_Status", Con)
            Dt1 = New DataTable
            Da1.Fill(Dt1)
            Da1.Fill(Dt2)

            With dgv_Details

                .Rows.Clear()

                If Dt1.Rows.Count > 0 Then

                    For I = 0 To Dt1.Rows.Count - 1

                        If .Rows.Count > 0 Then
                            If Trim(MDt) <> Trim(Dt1.Rows(I).Item("Modification_DateTime")) Or Trim(UCase(UsrNm)) <> Trim(UCase(Dt1.Rows(I).Item("User_Name").ToString)) Then
                                n = .Rows.Add()
                                For J = 0 To .ColumnCount - 1
                                    .Rows(n).Cells(J).Style.BackColor = Color.FromArgb(240, 240, 240)
                                Next
                            End If
                        End If

                        'MessageBox.Show(.Columns.Count())

                        n = .Rows.Add()

                        SNo = SNo + 1

                        .Rows(n).Cells(0).Value = Val(SNo)
                        .Rows(n).Cells(1).Value = Format(Dt1.Rows(I).Item("Modification_DateTime"), "dd-MM-yyyy hh:mm tt")
                        .Rows(n).Cells(2).Value = Dt1.Rows(I).Item("User_Name").ToString
                        .Rows(n).Cells(3).Value = Dt1.Rows(I).Item("Entry_Status").ToString
                        .Rows(n).Cells(4).Value = Dt1.Rows(I).Item("Details_SlNo").ToString
                        If Val(.Rows(n).Cells(4).Value) = 0 Then .Rows(n).Cells(4).Value = ""
                        .Rows(n).Cells(5).Value = Dt1.Rows(I).Item("Field_Name").ToString

                        str1 = ""
                        str1 = Replace(Dt1.Rows(I).Item("Field_OldValue").ToString, "~L", "ALL")
                        str1 = Replace(str1, "~A", "ADD-")
                        str1 = Replace(str1, "~E", "EDIT-")
                        str1 = Replace(str1, "~D", "DELETE-")
                        str1 = Replace(str1, "~V", "VIEW-")
                        str1 = Replace(str1, "~I", "INSERT-")
                        str1 = Replace(str1, "~F", "FILTER-")
                        .Rows(n).Cells(6).Value = Replace(str1, "~", ", ")

                        str1 = ""
                        str1 = Replace(Dt1.Rows(I).Item("Field_NewValue").ToString, "~L", "ALL")
                        str1 = Replace(str1, "~A", "ADD-")
                        str1 = Replace(str1, "~E", "EDIT-")
                        str1 = Replace(str1, "~D", "DELETE-")
                        str1 = Replace(str1, "~V", "VIEW-")
                        str1 = Replace(str1, "~I", "INSERT-")
                        str1 = Replace(str1, "~F", "FILTER-")
                        .Rows(n).Cells(7).Value = Replace(str1, "~", ", ")

                        .Rows(n).Cells(8).Value = Dt1.Rows(I).Item("Modification_Details").ToString
                        If Val(Dt1.Rows(I).Item("Verified_Status").ToString) = 1 Then
                            .Rows(n).Cells(9).Value = True
                        Else
                            .Rows(n).Cells(9).Value = False
                        End If
                        .Rows(n).Cells(10).Value = Dt1.Rows(I).Item("Auto_SlNo").ToString
                        MDt = Dt1.Rows(I).Item("Modification_DateTime")
                        UsrNm = Dt1.Rows(I).Item("User_Name").ToString

                    Next

                End If

                If .Rows.Count = 0 Then .Rows.Add()

                .Focus()
                .CurrentCell = .Rows(0).Cells(0)
                .CurrentCell.Selected = True

            End With
            Dt1.Clear()

            Dt1.Dispose()
            Da1.Dispose()

        End If

    End Sub

    Private Sub btn_close_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_close.Click
        Me.Close()
    End Sub

    Private Sub dgv_Details_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgv_Details.Click

        With dgv_Details
            If .CurrentCell.ColumnIndex = 9 Then
                If .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = False Then
                    .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = True

                Else
                    .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = False
                End If
            End If
            save_Satus()
        End With

    End Sub

    Private Sub dgv_Details_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgv_Details.DoubleClick
        Dim MsgTxt As String = ""

        With dgv_Details
            MsgTxt = "USER NAME : " & .Rows(.CurrentRow.Index).Cells(2).Value
            MsgTxt = MsgTxt & Chr(13) & "DATE & TIME : " & .Rows(.CurrentRow.Index).Cells(1).Value
            MsgTxt = MsgTxt & Chr(13) & "STATUS : " & .Rows(.CurrentRow.Index).Cells(3).Value
            If Val(.Rows(.CurrentRow.Index).Cells(4).Value) <> 0 Then MsgTxt = MsgTxt & Chr(13) & "SL.NO : " & .Rows(.CurrentRow.Index).Cells(4).Value
            MsgTxt = MsgTxt & Chr(13) & "COLUMN NAME : " & .Rows(.CurrentRow.Index).Cells(5).Value
            MsgTxt = MsgTxt & Chr(13) & "OLD VALUE : " & .Rows(.CurrentRow.Index).Cells(6).Value
            MsgTxt = MsgTxt & Chr(13) & "NEW VALUE : " & .Rows(.CurrentRow.Index).Cells(7).Value

            MessageBox.Show(MsgTxt, "USER MODIFICATIONS...", MessageBoxButtons.OKCancel, MessageBoxIcon.Information)

        End With


    End Sub

    Public Sub save_Satus()
        Dim trans As SqlClient.SqlTransaction
        Dim cmd As New SqlClient.SqlCommand
        Dim vUsrLog_DBName As String = ""

        With dgv_Details

            If .CurrentCell.ColumnIndex <> 9 Then
                Exit Sub
            End If

            vUsrLog_DBName = Common_Procedures.get_UserModificationDetails_DataBaseName(Val(Common_Procedures.CompGroupIdNo))

            trans = Con.BeginTransaction

            Try

                cmd.Connection = Con
                cmd.Transaction = trans

                If .Rows(.CurrentCell.RowIndex).Cells.Item(9).Value = True Then

                    cmd.CommandText = "Update " & Trim(vUsrLog_DBName) & "..User_Modification_Details set Verified_Status = 1 where Auto_SlNo =" & Str(Val(.Rows(.CurrentCell.RowIndex).Cells.Item(10).Value))
                    cmd.ExecuteNonQuery()

                Else

                    cmd.CommandText = "Update  " & Trim(vUsrLog_DBName) & "..User_Modification_Details set Verified_Status = 0 where Auto_SlNo =" & Str(Val(.Rows(.CurrentCell.RowIndex).Cells.Item(10).Value))
                    cmd.ExecuteNonQuery()

                End If

                trans.Commit()


            Catch ex As Exception
                trans.Rollback()


            Finally

            End Try
        End With
    End Sub

    Private Sub btn_VerifyAll_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_VerifyAll.Click
        Dim trans As SqlClient.SqlTransaction
        Dim cmd As New SqlClient.SqlCommand
        Dim Nr As Integer = 0
        Dim vUsrLog_DBName As String = ""

        vUsrLog_DBName = Common_Procedures.get_UserModificationDetails_DataBaseName(Val(Common_Procedures.CompGroupIdNo))

        trans = Con.BeginTransaction

        Try
            cmd.Connection = Con
            cmd.Transaction = trans
            With dgv_Details
                For i = 0 To .Rows.Count - 1
                    cmd.CommandText = "update  " & Trim(vUsrLog_DBName) & "..User_Modification_Details set Verified_Status = 1 where Auto_SlNo =" & Str(Val(.Rows(i).Cells.Item(10).Value))
                    Nr = cmd.ExecuteNonQuery()
                    If Nr = 1 Then
                        .Rows(i).Cells.Item(9).Value = True
                    Else
                        .Rows(i).Cells.Item(9).Value = False
                    End If
                Next
            End With

            trans.Commit()

        Catch ex As Exception
            trans.Rollback()


        Finally

        End Try
    End Sub

    Private Sub btn_RemoveAll_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_RemoveAll.Click
        Dim trans As SqlClient.SqlTransaction
        Dim cmd As New SqlClient.SqlCommand
        Dim Nr As Integer = 0
        Dim vUsrLog_DBName As String

        vUsrLog_DBName = Common_Procedures.get_UserModificationDetails_DataBaseName(Val(Common_Procedures.CompGroupIdNo))

        trans = Con.BeginTransaction

        Try
            cmd.Connection = Con
            cmd.Transaction = trans
            With dgv_Details
                For i = 0 To .Rows.Count - 1
                    cmd.CommandText = "update " & Trim(vUsrLog_DBName) & "..User_Modification_Details set Verified_Status = 0 where Auto_SlNo =" & Str(Val(.Rows(i).Cells.Item(10).Value))
                    Nr = cmd.ExecuteNonQuery()
                    If Nr = 1 Then
                        .Rows(i).Cells.Item(9).Value = False
                    End If
                Next
            End With

            trans.Commit()

        Catch ex As Exception
            trans.Rollback()


        Finally
            '---

        End Try
    End Sub

    Private Sub opt_All_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles opt_All.CheckedChanged
        If FrmLdSTS = True Then Exit Sub
        get_Conditions()
    End Sub
    Private Sub opt_VerifyPending_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles opt_VerifyPending.CheckedChanged
        If FrmLdSTS = True Then Exit Sub
        get_Conditions()
    End Sub
    Private Sub opt_FirstWeight_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles opt_FirstWeight.CheckedChanged
        If FrmLdSTS = True Then Exit Sub
        get_Conditions()
    End Sub
    Private Sub opt_SecondWeight_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles opt_SecondWeight.CheckedChanged
        If FrmLdSTS = True Then Exit Sub
        get_Conditions()
    End Sub

    Private Sub opt_Editing_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles opt_Editing.CheckedChanged
        If FrmLdSTS = True Then Exit Sub
        get_Conditions()
    End Sub
    Private Sub opt_All_Modify_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles opt_All_Modify.CheckedChanged
        If FrmLdSTS = True Then Exit Sub
        get_Conditions()
    End Sub

    Private Sub get_Conditions()
        Dim All As Integer = 0
        Dim Vrfy_Pend As Integer = 0
        Dim Add As Integer = 0
        Dim Add2 As Integer = 0
        Dim Edit As Integer = 0
        Dim All_Modify As Integer = 0

        If FrmLdSTS = True Then Exit Sub

        All = IIf(opt_All.Checked = True, 1, 0)
        Vrfy_Pend = IIf(opt_VerifyPending.Checked = True, 1, 0)
        Add = IIf(opt_FirstWeight.Checked = True, 1, 0)
        Add2 = IIf(opt_SecondWeight.Checked = True, 1, 0)
        Edit = IIf(opt_Editing.Checked = True, 1, 0)
        All_Modify = IIf(opt_All_Modify.Checked = True, 1, 0)


        If All = 1 And Vrfy_Pend = 0 And Add = 1 And Add2 = 0 And Edit = 0 And All_Modify = 0 Then   '---ALL and First Weight
            Display_Details(1, "ADD")

        ElseIf All = 1 And Vrfy_Pend = 0 And Add = 0 And Add2 = 1 And Edit = 0 And All_Modify = 0 Then  '----ALL and Second Weight
            Display_Details(1, "ADD2")

        ElseIf All = 1 And Vrfy_Pend = 0 And Add = 0 And Add2 = 0 And Edit = 1 And All_Modify = 0 Then  '-----All and Editing
            Display_Details(1, "EDIT")

        ElseIf All = 1 And Vrfy_Pend = 0 And Add = 0 And Add2 = 0 And Edit = 0 And All_Modify = 1 Then  '-----All and All Modification
            Display_Details(1, "")

        ElseIf All = 0 And Vrfy_Pend = 1 And Add = 1 And Add2 = 0 And Edit = 0 And All_Modify = 0 Then  '---Verify Pendig and First Weight
            Display_Details(0, "ADD")

        ElseIf All = 0 And Vrfy_Pend = 1 And Add = 0 And Add2 = 1 And Edit = 0 And All_Modify = 0 Then   '---Verify Pendig and Second Weight
            Display_Details(0, "ADD2")

        ElseIf All = 0 And Vrfy_Pend = 1 And Add = 0 And Add2 = 0 And Edit = 1 And All_Modify = 0 Then   '---Verify Pendig and Editing
            Display_Details(0, "EDIT")

        ElseIf All = 0 And Vrfy_Pend = 1 And Add = 0 And Add2 = 0 And Edit = 0 And All_Modify = 1 Then   '---Verify Pendig and All Modification
            Display_Details(0, "")

        End If
    End Sub

    Private Sub btn_filtershow_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_filtershow.Click
        Display_Details(1, "")
    End Sub
    Private Sub cbo_UserName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_UserName.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, Con, "User_head", "User_name", "", "(User_idno = 0)")
    End Sub

    Private Sub cbo_UserName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_UserName.KeyDown
        '  vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, Con, cbo_UserName, dtp_FilterTo_date, cbo_Slno, "User_head", "User_name", "", "(User_idno = 0)")
    End Sub

    Private Sub cbo_UserName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_UserName.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, Con, cbo_UserName, cbo_Slno, "User_head", "User_name", "", "(User_idno = 0)")
    End Sub

    Private Sub cbo_ColumnName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_ColumnName.GotFocus
        Dim vUsrLog_DBName As String

        vUsrLog_DBName = Common_Procedures.get_UserModificationDetails_DataBaseName(Val(Common_Procedures.CompGroupIdNo))
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, Con, Trim(vUsrLog_DBName) & "..User_Modification_Details", "Field_Name", "", "")
    End Sub

    Private Sub cbo_ColumnName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_ColumnName.KeyDown
        Dim vUsrLog_DBName As String

        vUsrLog_DBName = Common_Procedures.get_UserModificationDetails_DataBaseName(Val(Common_Procedures.CompGroupIdNo))
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, Con, cbo_ColumnName, cbo_Add, btn_filtershow, Trim(vUsrLog_DBName) & "..User_Modification_Details", "Field_Name", "", "")
    End Sub

    Private Sub cbo_ColumnName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_ColumnName.KeyPress
        Dim vUsrLog_DBName As String

        vUsrLog_DBName = Common_Procedures.get_UserModificationDetails_DataBaseName(Val(Common_Procedures.CompGroupIdNo))
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, Con, cbo_ColumnName, btn_filtershow, Trim(vUsrLog_DBName) & "..User_Modification_Details", "Field_Name", "", "", True)
    End Sub

    Private Sub cbo_Slno_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Slno.GotFocus
        Dim vUsrLog_DBName As String

        vUsrLog_DBName = Common_Procedures.get_UserModificationDetails_DataBaseName(Val(Common_Procedures.CompGroupIdNo))
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, Con, Trim(vUsrLog_DBName) & "..User_Modification_Details", "Details_SlNo", "", "")
    End Sub

    Private Sub cbo_Slno_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Slno.KeyDown
        Dim vUsrLog_DBName As String

        vUsrLog_DBName = Common_Procedures.get_UserModificationDetails_DataBaseName(Val(Common_Procedures.CompGroupIdNo))
        '  vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, Con, cbo_Slno, cbo_UserName, cbo_Add, Trim(vUsrLog_DBName) & "..User_Modification_Details", "Details_SlNo", "", "")
    End Sub

    Private Sub cbo_Slno_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Slno.KeyPress
        Dim vUsrLog_DBName As String

        vUsrLog_DBName = Common_Procedures.get_UserModificationDetails_DataBaseName(Val(Common_Procedures.CompGroupIdNo))
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, Con, cbo_Slno, cbo_Add, Trim(vUsrLog_DBName) & "..User_Modification_Details", "Details_SlNo", "", "", True)
    End Sub
    Private Sub cbo_Add_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Add.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, Con, "", "", "", "")
    End Sub

    Private Sub cbo_Add_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Add.KeyDown
        '  vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, Con, cbo_Add, cbo_Slno, cbo_ColumnName, "", "", "", "")
    End Sub

    Private Sub cbo_Add_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Add.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, Con, cbo_Add, cbo_ColumnName, "", "", "", "")
    End Sub
End Class