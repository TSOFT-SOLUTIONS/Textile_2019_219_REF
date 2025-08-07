Public Class Count_Creation
    Implements Interface_MDIActions

    Private con As New SqlClient.SqlConnection(Common_Procedures.Connection_String)
    Private New_Entry As Boolean
    Private FrmLdSTS As Boolean = False
    Private WithEvents dgtxt_Details As New DataGridViewTextBoxEditingControl
    Private WithEvents dgtxt_CountRate_Details As New DataGridViewTextBoxEditingControl
    Private dgv_ActiveCtrl_Name As String
    Private vcbo_KeyDwnVal As Double
    Private Prec_ActCtrl As New Control
    Private TrnTo_DbName As String = ""
    Private SizTo_DbName As String = ""
    Private Const vFRM_HEIGHT As Integer = 430
    Private Const vFRM_WIDTH As Integer = 540

    Public Sub New()
        FrmLdSTS = True
        ' This call is required by the designer.
        InitializeComponent()
        ' Add any initialization after the InitializeComponent() call.
    End Sub

    Private Sub clear()
        Me.Height = vFRM_HEIGHT ' 390 ' 335  ' 327
        pnl_back.Enabled = True
        grp_find.Visible = False
        grp_Filter.Visible = False
        lbl_IdNo.Text = ""
        lbl_IdNo.ForeColor = Color.Black
        txt_Name.Text = ""
        txt_description.Text = ""
        cbo_stockunder.Text = ""
        txt_resultantcount.Text = ""
        txt_RateKg.Text = ""
        cbo_Cotton_Polyester_Jari.Text = "COTTON"
        cbo_Transfer.Text = ""
        cbo_Sizing_CountName.Text = ""
        cbo_GridCount.Visible = False
        cbo_GridCount.Tag = -1
        cbo_GridCount.Text = ""
        dgv_Details.Rows.Clear()
        pnl_JariConsumption_Details.Visible = False

        cbo_ItemGroup.Text = ""

        pnl_RateDetails.Visible = False
        dgv_CountRate_Details.Rows.Clear()

        New_Entry = False

    End Sub

    Private Sub ControlGotFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim txtbx As TextBox
        Dim combobx As ComboBox

        On Error Resume Next

        If FrmLdSTS = True Then Exit Sub

        If TypeOf Me.ActiveControl Is TextBox Or TypeOf Me.ActiveControl Is ComboBox Then
            Me.ActiveControl.BackColor = Color.Lime ' Color.MistyRose ' Color.lime
            Me.ActiveControl.ForeColor = Color.Blue
        End If

        If TypeOf Me.ActiveControl Is TextBox Then
            txtbx = Me.ActiveControl
            txtbx.SelectAll()

        ElseIf TypeOf Me.ActiveControl Is ComboBox Then
            combobx = Me.ActiveControl
            combobx.SelectAll()

        End If

        If Me.ActiveControl.Name <> cbo_GridCount.Name Then
            cbo_GridCount.Visible = False
        End If

        Prec_ActCtrl = Me.ActiveControl

    End Sub

    Private Sub ControlLostFocus(ByVal sender As Object, ByVal e As System.EventArgs)

        On Error Resume Next

        If FrmLdSTS = True Then Exit Sub

        If IsNothing(Prec_ActCtrl) = False Then
            If TypeOf Prec_ActCtrl Is TextBox Or TypeOf Prec_ActCtrl Is ComboBox Then
                Prec_ActCtrl.BackColor = Color.White
                Prec_ActCtrl.ForeColor = Color.Black
            End If
        End If

    End Sub

    Private Sub ControlLostFocus1(ByVal sender As Object, ByVal e As System.EventArgs)

        On Error Resume Next

        If FrmLdSTS = True Then Exit Sub

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

    Private Sub Grid_DeSelect()
        On Error Resume Next

        If FrmLdSTS = True Then Exit Sub

        If Not IsNothing(dgv_Details.CurrentCell) Then dgv_Details.CurrentCell.Selected = False
        If Not IsNothing(dgv_CountRate_Details.CurrentCell) Then dgv_CountRate_Details.CurrentCell.Selected = False

    End Sub

    Public Sub delete_record() Implements Interface_MDIActions.delete_record
        Dim cmd As New SqlClient.SqlCommand
        Dim da As SqlClient.SqlDataAdapter
        Dim dt As DataTable

        '  If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.Count_Creation, "~L~") = 0 And InStr(Common_Procedures.UR.Count_Creation, "~D~") = 0 Then MessageBox.Show("You have No Rights to Delete", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error) : Exit Sub

        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.DeletingEntry, Common_Procedures.UR.Count_Creation, New_Entry, Me) = False Then Exit Sub


        If MessageBox.Show("Do you want to Delete ?", "FOR DELETION....", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.No Then
            Exit Sub
        End If

        If New_Entry = True Then
            MessageBox.Show("This is new entry", "DOES NOT DELETION....", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        Try

            da = New SqlClient.SqlDataAdapter("select count(*) from Cloth_Head where Cloth_WarpCount_IdNo = " & Str(Val(lbl_IdNo.Text)) & " OR Cloth_WeftCount_IdNo = " & Str(Val(lbl_IdNo.Text)), con)
            dt = New DataTable
            da.Fill(dt)
            If dt.Rows.Count > 0 Then
                If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                    If Val(dt.Rows(0)(0).ToString) > 0 Then
                        MessageBox.Show("Already used this Count", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        Exit Sub
                    End If
                End If
            End If

            da = New SqlClient.SqlDataAdapter("select count(*) from EndsCount_Head where Count_IdNo = " & Str(Val(lbl_IdNo.Text)), con)
            dt = New DataTable
            da.Fill(dt)
            If dt.Rows.Count > 0 Then
                If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                    If Val(dt.Rows(0)(0).ToString) > 0 Then
                        MessageBox.Show("Already used this Count", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        Exit Sub
                    End If
                End If
            End If


            da = New SqlClient.SqlDataAdapter("select count(*) from Mill_Count_Details where Count_IdNo = " & Str(Val(lbl_IdNo.Text)), con)
            dt = New DataTable
            da.Fill(dt)
            If dt.Rows.Count > 0 Then
                If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                    If Val(dt.Rows(0)(0).ToString) > 0 Then
                        MessageBox.Show("Already used this Count", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        Exit Sub
                    End If
                End If
            End If

            da = New SqlClient.SqlDataAdapter("select count(*) from Stock_Yarn_Processing_Details where Count_Idno = " & Str(Val(lbl_IdNo.Text)), con)
            dt = New DataTable
            da.Fill(dt)
            If dt.Rows.Count > 0 Then
                If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                    If Val(dt.Rows(0)(0).ToString) > 0 Then
                        MessageBox.Show("Already used this Count", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        Exit Sub
                    End If
                End If
            End If
            dt.Clear()

            cmd.Connection = con

            cmd.CommandText = "delete from Count_Master_Rate_Details where Count_Idno = " & Str(Val(lbl_IdNo.Text) & "")
            cmd.ExecuteNonQuery()

            cmd.CommandText = "delete from Count_Jari_Consumption_Details where Count_Idno = " & Str(Val(lbl_IdNo.Text) & "")
            cmd.ExecuteNonQuery()

            cmd.CommandText = "delete from Count_Head where Count_IdNo = " & Str(Val(lbl_IdNo.Text))
            cmd.ExecuteNonQuery()

            cmd.Dispose()

            new_record()

            MessageBox.Show("Deleted Sucessfully!!!", "FOR DELETION...", MessageBoxButtons.OK, MessageBoxIcon.Information)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally
            If txt_Name.Enabled And txt_Name.Visible Then txt_Name.Focus()

        End Try
    End Sub

    Public Sub filter_record() Implements Interface_MDIActions.filter_record
        Dim da As New SqlClient.SqlDataAdapter("select count_IdNo, Count_Name,Count_Description from Count_Head where Count_IdNo <> 0 order by Count_IdNo", con)
        Dim dt As New DataTable

        da.Fill(dt)

        With dgv_Filter

            .Columns.Clear()
            .DataSource = dt

            .RowHeadersVisible = False

            .AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells
            .AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill

            .Columns(0).HeaderText = "IDNO"
            .Columns(1).HeaderText = "NAME"
            .Columns(2).HeaderText = "DESCRIPTION"


            .Columns(0).FillWeight = 60
            .Columns(1).FillWeight = 160
            .Columns(2).FillWeight = 300


        End With

        new_record()

        grp_Filter.Visible = True

        pnl_back.Enabled = False

        If dgv_Filter.Enabled And dgv_Filter.Visible Then dgv_Filter.Focus()

        Me.Height = vFRM_HEIGHT '  520   '    514

        da.Dispose()

    End Sub

    Public Sub insert_record() Implements Interface_MDIActions.insert_record
        '-----
    End Sub

    Public Sub move_record(ByVal idno As Integer)
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim dt2 As New DataTable
        Dim dt3 As New DataTable
        Dim slno As Integer = 0
        Dim n As Integer = 0


        If Val(idno) = 0 Then Exit Sub

        clear()

        da = New SqlClient.SqlDataAdapter("select a.*, b.count_name as stock_undername ,IG.ItemGroup_Name from Count_head a LEFT OUTER JOIN count_head b ON a.Count_StockUnder_IdNo = b.count_idno LEFT OUTER JOIN ItemGroup_Head IG ON a.ItemGroup_IdNo = IG.ItemGroup_IdNo  where a.Count_idno = " & Str(Val(idno)), con)
        'da = New SqlClient.SqlDataAdapter("select a. from Count_head where Count_idno = " & Str(Val(idno)), con)
        dt = New DataTable
        da.Fill(dt)

        If dt.Rows.Count > 0 Then
            lbl_IdNo.Text = dt.Rows(0).Item("Count_IdNo").ToString
            txt_Name.Text = dt.Rows(0).Item("Count_Name").ToString
            txt_description.Text = dt.Rows(0).Item("Count_Description").ToString
            If Val(dt.Rows(0).Item("Count_Stockunder_IdNo").ToString) <> Val(dt.Rows(0).Item("Count_IdNo").ToString) Then
                cbo_stockunder.Text = dt.Rows(0).Item("stock_undername").ToString
            End If
            cbo_Cotton_Polyester_Jari.Text = dt.Rows(0).Item("Cotton_Polyester_Jari").ToString

            cbo_Transfer.Text = Common_Procedures.Count_IdNoToName(con, Val(dt.Rows(0).Item("Transfer_To_CountIdNo").ToString), , TrnTo_DbName)

            txt_resultantcount.Text = dt.Rows(0).Item("Resultant_Count").ToString
            txt_RateKg.Text = Format(Val(dt.Rows(0).Item("Rate_Kg").ToString), "###########0.00")

            cbo_ItemGroup.Text = dt.Rows(0).Item("ItemGroup_Name").ToString

            cbo_Sizing_CountName.Text = Common_Procedures.Count_IdNoToName(con, Val(dt.Rows(0).Item("Sizing_To_CountIdNo").ToString), , SizTo_DbName)

            da = New SqlClient.SqlDataAdapter("select a.*, b.Count_Name from Count_Jari_Consumption_Details a INNER JOIN Count_Head b ON a.JariCount_IdNo = b.Count_IdNo where a.Count_Idno = " & Str(Val(idno)) & " Order by a.sl_no", con)
            dt2 = New DataTable
            da.Fill(dt2)

            dgv_Details.Rows.Clear()
            slno = 0

            If dt2.Rows.Count > 0 Then

                For i = 0 To dt2.Rows.Count - 1

                    n = dgv_Details.Rows.Add()

                    dgv_Details.Rows(n).Cells(0).Value = dt2.Rows(i).Item("Count_Name").ToString
                    dgv_Details.Rows(n).Cells(1).Value = Format(Val(dt2.Rows(i).Item("Jari_Consumption").ToString), "#######0.0000")

                Next i

            End If
            dt2.Clear()
            dt2.Dispose()

        End If

        dt.Dispose()
        da.Dispose()


        da = New SqlClient.SqlDataAdapter("select a.* from Count_Master_Rate_Details a where a.Count_Idno = " & Str(Val(idno)) & " Order by FromDate_DateTime, ToDate_DateTime, Sl_No", con)
        dt3 = New DataTable
        da.Fill(dt3)

        dgv_CountRate_Details.Rows.Clear()
        slno = 0

        If dt3.Rows.Count > 0 Then

            For i = 0 To dt3.Rows.Count - 1

                n = dgv_CountRate_Details.Rows.Add()

                slno = slno + 1

                dgv_CountRate_Details.Rows(n).Cells(0).Value = Val(slno)
                dgv_CountRate_Details.Rows(n).Cells(1).Value = dt3.Rows(i).Item("FromDate_Text").ToString
                dgv_CountRate_Details.Rows(n).Cells(2).Value = dt3.Rows(i).Item("ToDate_Text").ToString
                dgv_CountRate_Details.Rows(n).Cells(3).Value = Format(Val(dt3.Rows(i).Item("Rate").ToString), "#########0.00")

            Next i

        End If
        dt3.Clear()
        dt3.Dispose()



        Grid_DeSelect()

        If txt_Name.Enabled And txt_Name.Visible Then txt_Name.Focus()

    End Sub

    Public Sub movefirst_record() Implements Interface_MDIActions.movefirst_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim movid As Integer = 0

        Try
            da = New SqlClient.SqlDataAdapter("select min(count_idno) from Count_head Where count_idno <> 0", con)
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

        'Try
        da = New SqlClient.SqlDataAdapter("select max(Count_idno) from Count_head Where Count_idno <> 0", con)
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

        'Catch ex As Exception
        'MessageBox.Show(ex.Message, "DOES NOT MOVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        'End Try
    End Sub

    Public Sub movenext_record() Implements Interface_MDIActions.movenext_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim movid As Integer = 0

        Try
            da = New SqlClient.SqlDataAdapter("select min(Count_idno) from count_head Where Count_idno > " & Str(Val(lbl_IdNo.Text)) & " and Count_idno <> 0", con)
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
            da = New SqlClient.SqlDataAdapter("select max(Count_idno) from Count_head Where count_idno < " & Str(Val(lbl_IdNo.Text)) & " and count_idno <> 0", con)
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
        clear()

        New_Entry = True
        lbl_IdNo.ForeColor = Color.Red

        lbl_IdNo.Text = Common_Procedures.get_MaxIdNo(con, "Count_Head", "Count_IdNo", "")

        If txt_Name.Enabled And txt_Name.Visible Then txt_Name.Focus()
    End Sub

    Public Sub open_record() Implements Interface_MDIActions.open_record
        Dim da As New SqlClient.SqlDataAdapter("select Count_Name from Count_Head order by Count_Name", con)
        Dim dt As New DataTable

        da.Fill(dt)

        cbo_Find.DataSource = dt
        cbo_Find.DisplayMember = "Count_Name"

        new_record()

        Me.Height = vFRM_HEIGHT '  520   ' 513
        grp_find.Visible = True
        pnl_back.Enabled = False
        If cbo_Find.Enabled And cbo_Find.Visible Then cbo_Find.Focus()
    End Sub

    Public Sub print_record() Implements Interface_MDIActions.print_record
        '-------
    End Sub

    Public Sub save_record() Implements Interface_MDIActions.save_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim dt2 As New DataTable
        Dim trans As SqlClient.SqlTransaction
        Dim cmd As New SqlClient.SqlCommand
        Dim Sur As String
        Dim stk_id As Integer = 0
        Dim GdCnt_ID As Integer = 0
        Dim SNo As Integer = 0
        Dim Transtk_id As Integer = 0
        Dim ItemGrp_IDno As Integer = 0
        Dim Sizstk_id As Integer = 0
        Dim vSTS As Boolean = False
        Dim vToDate1STS As Boolean = False
        Dim vToDate2STS As Boolean = False
        Dim vFrmDate1 As Date
        Dim vToDate1 As Date
        Dim vFrmDate2 As Date
        Dim vToDate2 As Date
        Dim vBlank_ToDate_Count As Integer = 0
        Dim vHSNCode As String = 0
        Dim vGstPerc As String = 0
        Dim Nr = 0L

        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.SavingEntry, Common_Procedures.UR.Count_Creation, New_Entry, Me) = False Then Exit Sub

        If pnl_back.Enabled = False Then
            MessageBox.Show("Close Other Windows", "DOES NOT SAVE", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        If Trim(txt_Name.Text) = "" Then
            MessageBox.Show("Invalid CountName", "DOES NOT SAVE", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If txt_Name.Enabled Then txt_Name.Focus()
            Exit Sub
        End If
        If Val(txt_resultantcount.Text) = 0 Then
            txt_resultantcount.Text = Val(txt_Name.Text)
        End If

        Sur = Common_Procedures.Remove_NonCharacters(Trim(txt_Name.Text))
        da = New SqlClient.SqlDataAdapter("select * from  Count_Head where Count_IdNo <> " & Str(Val(lbl_IdNo.Text)) & " and Sur_Name = '" & Trim(Sur) & "' Order by Count_IdNo", con)
        dt = New DataTable
        da.Fill(dt)
        If dt.Rows.Count > 0 Then
            MessageBox.Show("The count name is duplicated, and it has already been created with an ID of " & dt.Rows(0).Item("Count_IdNo").ToString, "DOES NOT SAVE....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
        End If
        dt.Clear()

        stk_id = Common_Procedures.Count_NameToIdNo(con, cbo_stockunder.Text)
        If Val(stk_id) = 0 Then
            stk_id = Val(lbl_IdNo.Text)
        End If

        ItemGrp_IDno = Common_Procedures.ItemGroup_NameToIdNo(con, cbo_ItemGroup.Text)
        If Val(ItemGrp_IDno) = 0 Then
            MessageBox.Show("Invalid Item Group Name (HSN CODE)", "DOES NOT SAVE", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If cbo_ItemGroup.Enabled Then cbo_ItemGroup.Focus()
            Exit Sub
        End If

        Transtk_id = Common_Procedures.Count_NameToIdNo(con, cbo_Transfer.Text, , TrnTo_DbName)
        If cbo_Transfer.Visible Then
            If Trim(cbo_Transfer.Text) <> "" Then
                If Val(Transtk_id) = 0 Then
                    MessageBox.Show("Invalid Transfer Stock To", "DOES NOT SAVE", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    If cbo_Transfer.Enabled Then cbo_Transfer.Focus()
                    Exit Sub
                End If
            End If
        End If

        Sizstk_id = Common_Procedures.Count_NameToIdNo(con, cbo_Sizing_CountName.Text, , SizTo_DbName)
        If cbo_Sizing_CountName.Visible Then
            If Trim(cbo_Sizing_CountName.Text) <> "" Then
                If Val(Sizstk_id) = 0 Then
                    MessageBox.Show("Invalid Sizing Count Name", "DOES NOT SAVE", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    If cbo_Sizing_CountName.Enabled Then cbo_Sizing_CountName.Focus()
                    Exit Sub
                End If
            End If
        End If

        With dgv_CountRate_Details

            For i = 0 To .RowCount - 1

                If Val(.Rows(i).Cells(3).Value) <> 0 Then

                    If Trim(.Rows(i).Cells(1).Value) = "" Then
                        MessageBox.Show("Invalid From Date", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                        .Focus()
                        .CurrentCell = .Rows(i).Cells(1)
                        Exit Sub
                    End If

                    If IsDate(.Rows(i).Cells(1).Value) = False Then
                        MessageBox.Show("Invalid From Date", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                        .Focus()
                        .CurrentCell = .Rows(i).Cells(1)
                        Exit Sub
                    End If

                End If

            Next

        End With

        With dgv_CountRate_Details

            For i = 0 To .RowCount - 1

                vFrmDate1 = #12:00:00 PM#
                vToDate1 = #12:00:00 PM#

                vToDate1STS = False

                vSTS = False
                If Trim(.Rows(i).Cells(1).Value) <> "" Then
                    If IsDate(.Rows(i).Cells(1).Value) = True Then
                        vSTS = True
                        vFrmDate1 = CDate(.Rows(i).Cells(1).Value)
                    End If
                End If

                If vSTS = True And Val(.Rows(i).Cells(3).Value) <> 0 Then

                    vToDate1STS = False

                    If Trim(.Rows(i).Cells(2).Value) <> "" Then
                        If IsDate(.Rows(i).Cells(2).Value) = True Then
                            vToDate1STS = True
                            vToDate1 = CDate(.Rows(i).Cells(2).Value)
                        End If
                    End If

                    If vToDate1STS = False Then
                        vBlank_ToDate_Count = vBlank_ToDate_Count + 1
                        'MessageBox.Show("Invalid To Date in Rate Details", "DOES NOT SAVE", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                        'pnl_Back.Enabled = False
                        'pnl_RateDetails.Visible = True
                        'If dgv_CountRate_Details.Enabled And dgv_CountRate_Details.Visible Then
                        '    dgv_CountRate_Details.Focus()
                        '    dgv_CountRate_Details.CurrentCell = dgv_CountRate_Details.Rows(i).Cells(1)
                        'End If
                        'Exit Sub

                    Else

                        If DateDiff(DateInterval.Day, vToDate1, vFrmDate1) > 0 Then


                            MessageBox.Show("Invalid Date in Rate Details" & Chr(13) & "To Date lesser than from date", "DOES NOT SAVE", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

                            pnl_back.Enabled = False
                            pnl_RateDetails.Visible = True
                            If dgv_CountRate_Details.Enabled And dgv_CountRate_Details.Visible Then
                                dgv_CountRate_Details.Focus()
                                dgv_CountRate_Details.CurrentCell = dgv_CountRate_Details.Rows(i).Cells(1)
                            End If

                            Exit Sub

                        End If

                    End If

                    For j = i + 1 To .RowCount - 1

                        vFrmDate2 = #12:00:00 PM#
                        vToDate2 = #12:00:00 PM#

                        vSTS = False
                        If Trim(.Rows(j).Cells(1).Value) <> "" Then
                            If IsDate(.Rows(j).Cells(1).Value) = True Then
                                vSTS = True
                                vFrmDate2 = CDate(.Rows(j).Cells(1).Value)
                            End If
                        End If


                        If vSTS = True And Val(.Rows(j).Cells(3).Value) <> 0 Then

                            If DateDiff(DateInterval.Day, vFrmDate2, vFrmDate1) > 0 Then

                                MessageBox.Show("Invalid Date in Rate Details - from date should be grater than previous date ", "DOES NOT SAVE", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                                pnl_back.Enabled = False
                                pnl_RateDetails.Visible = True
                                If dgv_CountRate_Details.Enabled And dgv_CountRate_Details.Visible Then
                                    dgv_CountRate_Details.Focus()
                                    dgv_CountRate_Details.CurrentCell = dgv_CountRate_Details.Rows(j).Cells(1)
                                End If
                                Exit Sub

                            End If

                        End If

                    Next j

                End If

            Next i

            If vBlank_ToDate_Count > 1 Then

                MessageBox.Show("Invalid To-Date in Rate Details", "DOES NOT SAVE", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                pnl_back.Enabled = False
                pnl_RateDetails.Visible = True
                If dgv_CountRate_Details.Enabled And dgv_CountRate_Details.Visible Then
                    dgv_CountRate_Details.Focus()
                    dgv_CountRate_Details.CurrentCell = dgv_CountRate_Details.Rows(0).Cells(1)
                End If
                Exit Sub
            End If

        End With



        vHSNCode = 0
        vGstPerc = 0

        da = New SqlClient.SqlDataAdapter("select a.* from itemgroup_head a  where a.itemgroup_idno = " & Str(Val(ItemGrp_IDno)), con)
        dt2 = New DataTable
        da.Fill(dt2)
        If dt2.Rows.Count > 0 Then
            vHSNCode = dt2.Rows(0)("Item_HSN_Code").ToString
            vGstPerc = Val(dt2.Rows(0)("Item_GST_Percentage").ToString)
        End If
        dt2.Clear()


        trans = con.BeginTransaction

        Try

            cmd.Connection = con
            cmd.Transaction = trans

            If New_Entry = True Then

                lbl_IdNo.Text = Common_Procedures.get_MaxIdNo(con, "Count_Head", "Count_IdNo", "", trans)

                cmd.CommandText = "Insert into Count_Head ( Count_IdNo, Count_Name, Sur_Name,Count_Description, Count_StockUnder_IdNo,Resultant_Count,Rate_Kg , Cotton_Polyester_Jari,Transfer_To_CountIdNo,ItemGroup_Idno,Sizing_To_CountIdNo, HSN_Code, GST_Percentege) values (" & Str(Val(lbl_IdNo.Text)) & ", '" & Trim(txt_Name.Text) & "', '" & Trim(Sur) & "','" & Trim(txt_description.Text) & "'," & Val(stk_id) & "," & Val(txt_resultantcount.Text) & "," & Val(txt_RateKg.Text) & ",'" & Trim(cbo_Cotton_Polyester_Jari.Text) & "'," & Val(Transtk_id) & "," & Val(ItemGrp_IDno) & "," & Val(Sizstk_id) & ", '" & Trim(vHSNCode) & "', " & Str(Val(vGstPerc)) & ")"
                cmd.ExecuteNonQuery()

            Else
                cmd.CommandText = "update Count_Head set Count_Name = '" & Trim(txt_Name.Text) & "', Sur_Name = '" & Trim(Sur) & "',Count_Description='" & Trim(txt_description.Text) & "', Count_StockUnder_IdNo=" & Val(stk_id) & ",Resultant_Count=" & Val(txt_resultantcount.Text) & ", Rate_Kg =" & Val(txt_RateKg.Text) & ",Cotton_Polyester_Jari = '" & Trim(cbo_Cotton_Polyester_Jari.Text) & "',Transfer_To_CountIdNo = " & Val(Transtk_id) & ",ItemGroup_IdNo = " & Str(Val(ItemGrp_IDno)) & ",Sizing_To_CountIdNo = " & Val(Sizstk_id) & " , HSN_Code =  '" & Trim(vHSNCode) & "', GST_Percentege = " & Str(Val(vGstPerc)) & " Where Count_IdNo = " & Str(Val(lbl_IdNo.Text))
                NR = cmd.ExecuteNonQuery()

            End If

            cmd.CommandText = "delete from Count_Jari_Consumption_Details where Count_Idno = " & Str(Val(lbl_IdNo.Text) & "")
            cmd.ExecuteNonQuery()

            With dgv_Details
                SNo = 0
                For i = 0 To .RowCount - 1

                    GdCnt_ID = Common_Procedures.Count_NameToIdNo(con, .Rows(i).Cells(0).Value, trans)

                    If Val(GdCnt_ID) <> 0 Then

                        SNo = SNo + 1

                        cmd.CommandText = "Insert into Count_Jari_Consumption_Details(Count_Idno, sl_No, JariCount_IdNo, Jari_Consumption) Values (" & Str(Val(lbl_IdNo.Text)) & ", " & Str(Val(SNo)) & ", " & Val(GdCnt_ID) & ", " & Val(.Rows(i).Cells(1).Value) & " )"
                        cmd.ExecuteNonQuery()

                    End If
                Next

            End With

            cmd.CommandText = "delete from Count_Master_Rate_Details where Count_Idno = " & Str(Val(lbl_IdNo.Text) & "")
            cmd.ExecuteNonQuery()

            With dgv_CountRate_Details
                SNo = 0
                For i = 0 To .RowCount - 1

                    vSTS = False

                    cmd.Parameters.Clear()

                    If Trim(.Rows(i).Cells(1).Value) <> "" Then
                        If IsDate(.Rows(i).Cells(1).Value) = True Then
                            cmd.Parameters.AddWithValue("@FromDate", CDate(.Rows(i).Cells(1).Value))
                            vSTS = True
                        End If
                    End If

                    If vSTS = True And Val(.Rows(i).Cells(3).Value) <> 0 Then

                        SNo = SNo + 1

                        If Trim(.Rows(i).Cells(2).Value) <> "" Then
                            If IsDate(.Rows(i).Cells(2).Value) = True Then
                                cmd.Parameters.AddWithValue("@ToDate", CDate(.Rows(i).Cells(2).Value))
                            End If
                        End If

                        cmd.CommandText = "Insert into Count_Master_Rate_Details (             Count_Idno        ,            sl_No     , FromDate_DateTime ,                    FromDate_Text        ,                                             ToDate_DateTime            ,                    ToDate_Text          ,                      Rate                 ) " &
                                            " Values                             (" & Str(Val(lbl_IdNo.Text)) & ", " & Str(Val(SNo)) & ",    @FromDate      , '" & Trim(.Rows(i).Cells(1).Value) & "' , " & IIf(IsDate(.Rows(i).Cells(2).Value) = True, "@ToDate", "Null") & " , '" & Trim(.Rows(i).Cells(2).Value) & "' , " & Str(Val(.Rows(i).Cells(3).Value)) & " ) "
                        cmd.ExecuteNonQuery()


                        If btn_RateDetails.Visible = True Then
                            cmd.CommandText = "Update Count_Head set Rate_Kg = " & Val(.Rows(i).Cells(3).Value) & " Where Count_Idno = " & Str(Val(lbl_IdNo.Text))
                            cmd.ExecuteNonQuery()
                        End If





                    End If

                Next

            End With

            trans.Commit()

            Common_Procedures.Master_Return.Return_Value = Trim(txt_Name.Text)
            Common_Procedures.Master_Return.Master_Type = "Count"

            MessageBox.Show("Saved Sucessfully!!!", "FOR SAVING....", MessageBoxButtons.OK, MessageBoxIcon.Information)

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

            If InStr(1, Trim(LCase(ex.Message)), "ix_count_head") > 0 Then
                MessageBox.Show("Duplicate Count Name", "DOES NOT SAVE", MessageBoxButtons.OK, MessageBoxIcon.Error)

            Else
                MessageBox.Show(ex.Message, "DOES NOT SAVE", MessageBoxButtons.OK, MessageBoxIcon.Error)

            End If

        Finally
            If txt_Name.Enabled And txt_Name.Visible Then txt_Name.Focus()


        End Try
    End Sub

    Private Sub Count_Creation_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim dt3 As New DataTable
        Dim dt4 As New DataTable

        If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_ItemGroup.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "ITEMGROUP" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
            da = New SqlClient.SqlDataAdapter("select ItemGroup_Name from ItemGroup_Head order by ItemGroup_Name", con)
            da.Fill(dt1)
            cbo_ItemGroup.DataSource = dt1
            cbo_ItemGroup.DisplayMember = "ItemGroup_Name"

            cbo_ItemGroup.Text = Trim(Common_Procedures.Master_Return.Return_Value)
        End If

        Common_Procedures.Master_Return.Return_Value = ""
        Common_Procedures.Master_Return.Master_Type = ""
        FrmLdSTS = False
    End Sub

    Private Sub LotNo_creation_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        On Error Resume Next
        con.Close()
        con.Dispose()
        Common_Procedures.Last_Closed_FormName = Me.Name
    End Sub

    Private Sub LotNo_creation_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If Asc(e.KeyChar) = 27 Then
            If grp_Filter.Visible Then
                btn_FilterClose_Click(sender, e)
            ElseIf grp_find.Visible Then
                btn_FindClose_Click(sender, e)
            ElseIf pnl_RateDetails.Visible = True Then
                btn_Close_rate_Click(sender, e)
            ElseIf pnl_JariConsumption_Details.Visible = True Then
                btn_Close_JariConsumption_Details_Click(sender, e)
            Else
                If MessageBox.Show("Do you want to Close?", "FOR CLOSING ENTRY...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) <> Windows.Forms.DialogResult.Yes Then
                    Exit Sub

                Else
                    Me.Close()

                End If

            End If

        End If
    End Sub


    Private Sub LotNo_creation_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim dt1 As New DataTable
        Dim TrnTo_CmpGrpIdNo As Integer = 0
        Dim SizTo_CmpGrpIdNo As Integer = 0

        FrmLdSTS = True

        grp_find.Left = 8  ' 12
        grp_find.Top = (Me.Height - grp_find.Height) - 20  ' 310  '292
        grp_find.Visible = False
        grp_Filter.BringToFront()

        grp_Filter.Left = 8  ' 12
        grp_Filter.Top = (Me.Height - grp_Filter.Height) - 20  '  310  '292
        grp_Filter.Visible = False
        grp_Filter.BringToFront()

        pnl_JariConsumption_Details.Visible = False
        pnl_JariConsumption_Details.Left = (Me.Width - pnl_JariConsumption_Details.Width) \ 2
        pnl_JariConsumption_Details.Top = (Me.Height - pnl_JariConsumption_Details.Height) \ 2
        pnl_JariConsumption_Details.BringToFront()

        btn_show_JariConsumption_Details.Visible = False
        If Common_Procedures.settings.Bobin_Zari_Kuri_Entries_Status = 1 Or Common_Procedures.settings.Bobin_Production_Entries_Status = 1 Or Common_Procedures.settings.Weaver_Zari_Kuri_Entries_Status = 1 Then
            btn_show_JariConsumption_Details.Visible = True
        End If

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1135" Then '---- Maria Fab (Karumathampatti)
            dgv_Details.Visible = False
            Me.Width = vFRM_WIDTH ' 840 ' 845 ' 535 ' 544
            Me.Height = vFRM_HEIGHT '  390 ' 335
        Else
            Me.Width = vFRM_WIDTH '840 ' 845 ' 535 ' 544
            Me.Height = vFRM_HEIGHT '  390 ' 335
        End If

        con.Open()
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1333" Then 'NT Tex 
            txt_description.MaxLength = 50
        End If

        cbo_Transfer.Visible = False
        lbl_Transfer.Visible = False
        TrnTo_CmpGrpIdNo = Val(Common_Procedures.get_FieldValue(con, Trim(Common_Procedures.CompanyDetailsDataBaseName) & "..CompanyGroup_Head", "Transfer_To_CompanyGroupIdNo", "(CompanyGroup_IdNo = " & Str(Val(Common_Procedures.CompGroupIdNo)) & ")"))
        If Val(TrnTo_CmpGrpIdNo) <> 0 Then
            TrnTo_DbName = Common_Procedures.get_Company_DataBaseName(Trim(Val(TrnTo_CmpGrpIdNo)))
            cbo_Transfer.Visible = True
            lbl_Transfer.Visible = True
        Else
            TrnTo_DbName = Common_Procedures.get_Company_DataBaseName(Trim(Val(Common_Procedures.CompGroupIdNo)))
        End If

        cbo_Sizing_CountName.Visible = False
        lbl_Sizing.Visible = False

        If Common_Procedures.settings.Combine_Textile_SizingSOftware = 1 Then
            SizTo_DbName = Common_Procedures.get_Company_SizingDataBaseName(Trim(Val(Common_Procedures.CompGroupIdNo)))
            cbo_Sizing_CountName.Visible = True
            lbl_Sizing.Visible = True
        Else
            SizTo_DbName = Common_Procedures.get_Company_DataBaseName(Trim(Val(Common_Procedures.CompGroupIdNo)))
        End If


        da = New SqlClient.SqlDataAdapter("select Count_Name from Count_Head order by Count_Name", con)
        da.Fill(dt)
        cbo_stockunder.DataSource = dt
        cbo_stockunder.DisplayMember = "Count_Name"


        da = New SqlClient.SqlDataAdapter("select ItemGroup_Name from ItemGroup_Head order by ItemGroup_Name", con)
        da.Fill(dt1)
        cbo_ItemGroup.DataSource = dt1
        cbo_ItemGroup.DisplayMember = "ItemGroup_Name"

        txt_RateKg.Visible = True
        btn_RateDetails.Visible = False
        If Common_Procedures.settings.CustomerCode = "1155" Or Common_Procedures.settings.CustomerCode = "1267" Then
            txt_RateKg.Visible = False
            btn_RateDetails.Visible = True
        End If

        pnl_RateDetails.Visible = False
        pnl_RateDetails.Left = (Me.Width - pnl_RateDetails.Width) \ 2
        pnl_RateDetails.Top = (Me.Height - pnl_RateDetails.Height) \ 2
        pnl_RateDetails.BringToFront()

        cbo_Cotton_Polyester_Jari.Items.Clear()
        cbo_Cotton_Polyester_Jari.Items.Add("COTTON")
        cbo_Cotton_Polyester_Jari.Items.Add("POLYESTER")
        cbo_Cotton_Polyester_Jari.Items.Add("JARI")

        AddHandler txt_Name.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_description.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Cotton_Polyester_Jari.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Transfer.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Sizing_CountName.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_resultantcount.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_RateKg.GotFocus, AddressOf ControlGotFocus

        AddHandler cbo_GridCount.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Find.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_ItemGroup.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_stockunder.GotFocus, AddressOf ControlGotFocus


        AddHandler txt_Name.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_description.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Cotton_Polyester_Jari.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Sizing_CountName.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_resultantcount.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Find.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Transfer.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_GridCount.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_RateKg.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_ItemGroup.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_stockunder.LostFocus, AddressOf ControlLostFocus


        AddHandler txt_resultantcount.KeyDown, AddressOf TextBoxControlKeyDown

        AddHandler txt_resultantcount.KeyPress, AddressOf TextBoxControlKeyPress





        'Me.Top = Me.Top - 75

        new_record()

    End Sub


    Private Sub btn_Save_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Save.Click
        save_record()
    End Sub

    Private Sub btn_FilterClose_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_FilterClose.Click
        Me.Height = vFRM_HEIGHT '  390 ' 335  ' 327
        pnl_back.Enabled = True
        grp_Filter.Visible = False
    End Sub

    Private Sub btn_FindOpen_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_FindOpen.Click
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim movid As Integer

        If Trim(cbo_Find.Text) = "" Then
            MessageBox.Show("Invalid CountName", "DOES NOT FIND...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If cbo_Find.Visible And cbo_Find.Enabled Then cbo_Find.Focus()
            Exit Sub
        End If

        da = New SqlClient.SqlDataAdapter("select Count_IdNo from Count_Head where Count_Name= '" & Trim(cbo_Find.Text) & "'", con)
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
        Me.Height = vFRM_HEIGHT '  390 ' 335 ' 327
    End Sub

    Private Sub cbo_Find_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Find.KeyDown
        Try
            With cbo_Find
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

    Private Sub cbo_Find_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Find.KeyPress
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim Condt As String
        Dim FindStr As String

        Try

            With cbo_Find

                If Asc(e.KeyChar) <> 27 Then

                    If Asc(e.KeyChar) = 13 Then

                        If Trim(.Text) <> "" Then
                            If .DroppedDown = True Then
                                If Trim(.SelectedText) <> "" Then
                                    .Text = .SelectedText
                                Else
                                    If .Items.Count > 0 Then
                                        .SelectedIndex = 0
                                        .SelectedItem = .Items(0)
                                        .Text = .GetItemText(.SelectedItem)
                                    End If
                                End If
                            End If
                        End If

                        Call btn_FindOpen_Click(sender, e)

                    Else

                        Condt = ""
                        FindStr = ""

                        If Asc(e.KeyChar) = 8 Then
                            If .SelectionStart <= 1 Then
                                .Text = ""
                            End If

                            If Trim(.Text) <> "" Then
                                If .SelectionLength = 0 Then
                                    FindStr = .Text.Substring(0, .Text.Length - 1)
                                Else
                                    FindStr = .Text.Substring(0, .SelectionStart - 1)
                                End If
                            End If

                        Else
                            If .SelectionLength = 0 Then
                                FindStr = .Text & e.KeyChar
                            Else
                                FindStr = .Text.Substring(0, .SelectionStart) & e.KeyChar
                            End If

                        End If

                        FindStr = LTrim(FindStr)

                        If Trim(FindStr) <> "" Then
                            Condt = " Where Count_Name like '" & Trim(FindStr) & "%' or Count_Name like '% " & Trim(FindStr) & "%' "
                        End If

                        da = New SqlClient.SqlDataAdapter("select Count_Name from Count_Head " & Condt & " order by Count_Name", con)
                        da.Fill(dt)

                        .DataSource = dt
                        .DisplayMember = "Count_Name"

                        .Text = FindStr

                        .SelectionStart = FindStr.Length

                        e.Handled = True

                    End If

                End If

            End With

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT SELECT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        da.Dispose()
    End Sub

    Private Sub dgv_Filter_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgv_Filter.DoubleClick
        Call btn_Filteropen_Click(sender, e)
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

    Private Sub dgv_Filter_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_Filter.KeyDown
        If e.KeyValue = 13 Then
            Call btn_Filteropen_Click(sender, e)
        End If
    End Sub

    Private Sub btn_Close_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Close.Click
        Me.Close()
    End Sub

    Private Sub txt_Description_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_description.KeyDown
        If e.KeyValue = 40 Then SendKeys.Send("{TAB}")
        If e.KeyValue = 38 Then SendKeys.Send("+{TAB}")
    End Sub

    Private Sub txt_Name_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_Name.KeyDown
        If e.KeyValue = 40 Then SendKeys.Send("{TAB}")
        If e.KeyValue = 38 Then SendKeys.Send("+{TAB}")
    End Sub

    Private Sub txt_Name_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Name.KeyPress
        If Asc(e.KeyChar) = 34 Or Asc(e.KeyChar) = 39 Then   '-- Single Quotes and double quotes blocked
            e.Handled = True
        End If
        If Asc(e.KeyChar) = 13 Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub cbo_ItemGroup_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_ItemGroup.GotFocus

        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "ItemGroup_Head", "ItemGroup_Name", "", "(Cloth_IdNo = 0)")

    End Sub

    Private Sub cbo_ItemGroup_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_ItemGroup.KeyDown

        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_ItemGroup, txt_description, cbo_stockunder, "ItemGroup_Head", "ItemGroup_Name", "", "(ItemGroup_IdNo = 0)")
        'If (e.KeyValue = 38 And cbo_ItemGroup.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
        '    cbo_Cotton_Polyester_Jari.Focus()
        'End If

    End Sub

    Private Sub cbo_ItemGroup_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_ItemGroup.KeyPress

        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_ItemGroup, cbo_stockunder, "ItemGroup_Head", "ItemGroup_Name", "", "(ItemGroup_IdNo = 0)")

    End Sub
    Private Sub cbo_ItemGroup_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_ItemGroup.KeyUp
        If e.Control = False And e.KeyValue = 17 Then
            Dim f As New ItemGroup_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_ItemGroup.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If
    End Sub
    Private Sub cbo_stock_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_stockunder.KeyDown
        Try
            With cbo_stockunder
                If e.KeyValue = 38 And .DroppedDown = False Then
                    e.Handled = True
                    cbo_ItemGroup.Focus()
                    'SendKeys.Send("+{TAB}")
                ElseIf e.KeyValue = 40 And .DroppedDown = False Then
                    e.Handled = True
                    cbo_Cotton_Polyester_Jari.Focus()
                    'SendKeys.Send("{TAB}")
                ElseIf e.KeyValue <> 13 And e.KeyValue <> 17 And e.KeyValue <> 27 And .DroppedDown = False Then
                    .DroppedDown = True
                End If
            End With

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT SELECT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub cbo_stock_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_stockunder.KeyPress
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim Condt As String
        Dim FindStr As String

        Try

            With cbo_stockunder

                If Asc(e.KeyChar) <> 27 Then

                    If Asc(e.KeyChar) = 13 Then

                        If Trim(.Text) <> "" Then
                            If .DroppedDown = True Then
                                If Trim(.SelectedText) <> "" Then
                                    .Text = .SelectedText
                                Else
                                    If .Items.Count > 0 Then
                                        .SelectedIndex = 0
                                        .SelectedItem = .Items(0)
                                        .Text = .GetItemText(.SelectedItem)
                                    End If
                                End If
                            End If
                        End If

                        cbo_Cotton_Polyester_Jari.Focus()

                    Else

                        Condt = ""
                        FindStr = ""

                        If Asc(e.KeyChar) = 8 Then
                            If .SelectionStart <= 1 Then
                                .Text = ""
                            End If

                            If Trim(.Text) <> "" Then
                                If .SelectionLength = 0 Then
                                    FindStr = .Text.Substring(0, .Text.Length - 1)
                                Else
                                    FindStr = .Text.Substring(0, .SelectionStart - 1)
                                End If
                            End If

                        Else
                            If .SelectionLength = 0 Then
                                FindStr = .Text & e.KeyChar
                            Else
                                FindStr = .Text.Substring(0, .SelectionStart) & e.KeyChar
                            End If

                        End If

                        FindStr = LTrim(FindStr)

                        If Trim(FindStr) <> "" Then
                            Condt = " Where Count_Name like '" & Trim(FindStr) & "%' or Count_Name like '% " & Trim(FindStr) & "%' "
                        End If

                        da = New SqlClient.SqlDataAdapter("select Count_Name from Count_Head " & Condt & " order by Count_Name", con)
                        da.Fill(dt)

                        .DataSource = dt
                        .DisplayMember = "Count_Name"

                        .Text = FindStr

                        .SelectionStart = FindStr.Length

                        e.Handled = True

                    End If

                End If

            End With

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT SELECT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        da.Dispose()
    End Sub

    Private Sub cbo_Cotton_Polyester_Jari_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Cotton_Polyester_Jari.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Cotton_Polyester_Jari, cbo_stockunder, txt_resultantcount, "", "", "", "")
    End Sub

    Private Sub cbo_Cotton_Polyester_Jari_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Cotton_Polyester_Jari.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Cotton_Polyester_Jari, txt_resultantcount, "", "", "", "")
    End Sub

    'Private Sub txt_count_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_resultantcount.KeyDown
    '    If e.KeyValue = 40 Then SendKeys.Send("{TAB}")
    '    If e.KeyValue = 38 Then SendKeys.Send("+{TAB}")
    'End Sub

    Private Sub txt_count_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_resultantcount.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
        'If Asc(e.KeyChar) = 13 Then
        '    If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
        '        save_record()
        '    End If
        'End If
    End Sub

    Private Sub txt_description_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_description.KeyPress
        If Asc(e.KeyChar) = 13 Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub txt_rateKg_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_RateKg.KeyDown

        If e.KeyValue = 40 Then
            If cbo_Transfer.Visible = True Then
                cbo_Transfer.Focus()
            ElseIf cbo_Sizing_CountName.Visible = True Then
                cbo_Sizing_CountName.Focus()
            Else
                If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                    save_record()
                Else
                    txt_Name.Focus()
                End If

                'If dgv_Details.Rows.Count <= 0 Then dgv_Details.Rows.Add()
                'dgv_Details.Focus()
                'dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(0)
            End If
        End If
        If e.KeyValue = 38 Then SendKeys.Send("+{TAB}")
    End Sub

    Private Sub txt_ratekg_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_RateKg.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
        If Asc(e.KeyChar) = 13 Then
            If cbo_Transfer.Visible = True Then
                cbo_Transfer.Focus()
            ElseIf cbo_Sizing_CountName.Visible = True Then
                cbo_Sizing_CountName.Focus()
            Else
                If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                    save_record()
                Else
                    txt_Name.Focus()
                End If

                'If dgv_Details.Rows.Count <= 0 Then dgv_Details.Rows.Add()
                'dgv_Details.Focus()
                'dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(0)
            End If
        End If


    End Sub

    Private Sub dgv_Details_CellEndEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellEndEdit
        dgv_Details_CellLeave(sender, e)
    End Sub

    Private Sub dgv_Details_CellEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellEnter
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim Dt2 As New DataTable
        Dim Dt3 As New DataTable
        Dim rect As Rectangle

        With dgv_Details

            'dgv_ActiveCtrl_Name = .Name

            If e.ColumnIndex = 0 Then

                If cbo_GridCount.Visible = False Or Val(cbo_GridCount.Tag) <> e.RowIndex Then

                    cbo_GridCount.Tag = -1
                    Da = New SqlClient.SqlDataAdapter("select Count_Name from Count_Head order by Count_Name", con)
                    Dt1 = New DataTable
                    Da.Fill(Dt1)
                    cbo_GridCount.DataSource = Dt1
                    cbo_GridCount.DisplayMember = "Count_Name"

                    rect = .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False)

                    cbo_GridCount.Left = .Left + rect.Left  '  .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Left
                    cbo_GridCount.Top = .Top + rect.Top  ' .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Top

                    cbo_GridCount.Width = rect.Width  ' .CurrentCell.Size.Width
                    cbo_GridCount.Height = rect.Height  ' rect.Height
                    cbo_GridCount.Text = .CurrentCell.Value  '  Trim(.CurrentRow.Cells(e.ColumnIndex).Value)

                    cbo_GridCount.Tag = Val(e.RowIndex)
                    cbo_GridCount.Visible = True

                    cbo_GridCount.BringToFront()
                    cbo_GridCount.Focus()


                End If


            Else

                cbo_GridCount.Visible = False

            End If
        End With
    End Sub

    Private Sub dgv_Details_CellLeave(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellLeave
        With dgv_Details
            If .CurrentCell.ColumnIndex = 1 Then
                If Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value) <> 0 Then
                    .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = Format(Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value), "#########0.0000")
                End If
            End If
        End With
    End Sub

    Private Sub dgv_KuriDetails_EditingControlShowing(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewEditingControlShowingEventArgs) Handles dgv_Details.EditingControlShowing
        dgtxt_Details = CType(dgv_Details.EditingControl, DataGridViewTextBoxEditingControl)
    End Sub

    Private Sub dgv_Details_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_Details.KeyUp
        Dim i As Integer
        Dim n As Integer

        If e.Control = True And UCase(Chr(e.KeyCode)) = "D" Then

            With dgv_Details

                n = .CurrentRow.Index

                If .CurrentCell.RowIndex = .Rows.Count - 1 Then
                    For i = 0 To .ColumnCount - 1
                        .Rows(n).Cells(i).Value = ""
                    Next

                Else
                    .Rows.RemoveAt(n)

                End If

            End With

        End If
    End Sub

    Private Sub dgv_Details_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgv_Details.LostFocus
        On Error Resume Next
        dgv_Details.CurrentCell.Selected = False
    End Sub
    Private Sub dgtxt_Details_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgtxt_Details.Enter
        ' dgv_ActiveCtrl_Name = dgv_KuriDetails.Name
        dgv_Details.EditingControl.BackColor = Color.Lime
        dgv_Details.EditingControl.ForeColor = Color.Blue
    End Sub

    Private Sub dgtxt_Details_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dgtxt_Details.KeyPress
        With dgv_Details
            If .Visible Then
                If .CurrentCell.ColumnIndex = 1 Then

                    If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
                        e.Handled = True
                    End If

                End If
            End If
        End With
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

            ElseIf pnl_back.Enabled = True Then
                dgv1 = dgv_Details

            ElseIf ActiveControl.Name = dgv_CountRate_Details.Name Then
                dgv1 = dgv_CountRate_Details

            ElseIf dgv_CountRate_Details.IsCurrentRowDirty = True Then
                dgv1 = dgv_CountRate_Details

            ElseIf dgv_ActiveCtrl_Name = dgv_CountRate_Details.Name Then
                dgv1 = dgv_CountRate_Details

            End If

            If IsNothing(dgv1) = True Then
                Return MyBase.ProcessCmdKey(msg, keyData)
                Exit Function
            End If


            With dgv1

                If dgv1.Name = dgv_Details.Name Then

                    If keyData = Keys.Enter Or keyData = Keys.Down Then
                        If .CurrentCell.ColumnIndex >= .ColumnCount - 1 Then
                            If .CurrentCell.RowIndex = .RowCount - 1 Then

                                If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                                    save_record()
                                Else
                                    txt_Name.Focus()
                                End If

                            Else
                                .CurrentCell = .Rows(.CurrentCell.RowIndex + 1).Cells(0)

                            End If

                        Else
                            .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 1)

                        End If

                        Return True

                    ElseIf keyData = Keys.Up Then

                        If .CurrentCell.ColumnIndex <= 0 Then
                            If .CurrentCell.RowIndex = 0 Then
                                txt_RateKg.Focus()

                            Else
                                .CurrentCell = .Rows(.CurrentCell.RowIndex - 1).Cells(.ColumnCount - 1)

                            End If

                        Else
                            .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex - 1)

                        End If

                        Return True

                    Else
                        Return MyBase.ProcessCmdKey(msg, keyData)

                    End If

                ElseIf dgv1.Name = dgv_CountRate_Details.Name Then

                    If keyData = Keys.Enter Or keyData = Keys.Down Then
                        If .CurrentCell.ColumnIndex >= .ColumnCount - 1 Then
                            If .CurrentCell.RowIndex = .RowCount - 1 Then

                                Close_Count_Master_Rate_Details()

                            Else
                                .Focus()
                                .CurrentCell = .Rows(.CurrentCell.RowIndex + 1).Cells(1)


                            End If

                        Else

                            If .CurrentCell.RowIndex = .RowCount - 1 And .CurrentCell.ColumnIndex >= 1 And ((.CurrentCell.ColumnIndex <> 1 And Val(.CurrentRow.Cells(1).Value) = 0) Or (.CurrentCell.ColumnIndex = 1 And Val(dgtxt_CountRate_Details.Text) = 0)) Then
                                For i = 0 To .Columns.Count - 1
                                    .Rows(.CurrentCell.RowIndex).Cells(i).Value = ""
                                Next

                                Close_Count_Master_Rate_Details()

                            ElseIf .CurrentCell.ColumnIndex = 1 Then
                                .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 2)

                            Else
                                .Focus()
                                .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 1)

                            End If


                        End If

                        Return True

                    ElseIf keyData = Keys.Up Then

                        If .CurrentCell.ColumnIndex <= 1 Then
                            If .CurrentCell.RowIndex = 0 Then

                                Close_Count_Master_Rate_Details()

                            Else
                                .CurrentCell = .Rows(.CurrentCell.RowIndex - 1).Cells(.ColumnCount - 1)

                            End If

                        ElseIf .CurrentCell.ColumnIndex = 3 Then
                            .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex - 2)

                        Else
                            .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex - 1)

                        End If

                        Return True

                    Else
                        Return MyBase.ProcessCmdKey(msg, keyData)

                    End If


                End If

            End With

        Else

            Return MyBase.ProcessCmdKey(msg, keyData)

        End If

    End Function

    Private Sub cbo_GridCount_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_GridCount.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_GridCount, Nothing, Nothing, "Count_Head", "Count_Name", "", "(Count_Idno = 0)")
        With dgv_Details


            If (e.KeyValue = 38 And cbo_GridCount.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
                If cbo_Transfer.Visible = True Then
                    cbo_Transfer.Focus()
                ElseIf .CurrentCell.RowIndex = 0 Then
                    If cbo_Sizing_CountName.Visible = True Then
                        cbo_Sizing_CountName.Focus()
                    Else
                        txt_RateKg.Focus()
                    End If
                Else
                    .Focus()
                    .CurrentCell = .Rows(.CurrentRow.Index - 1).Cells(.ColumnCount - 1)
                End If
            End If
            If (e.KeyValue = 40 And cbo_GridCount.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
                .Focus()
                .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 1)
            End If

        End With
    End Sub

    Private Sub cbo_GridCount_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_GridCount.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_GridCount, Nothing, "Count_Head", "Count_Name", "", "(Count_IdNo = 0)")
        If Asc(e.KeyChar) = 13 Then

            With dgv_Details
                If .CurrentCell.RowIndex = .Rows.Count - 1 And Trim(.Rows(.CurrentCell.RowIndex).Cells.Item(0).Value) = "" Then
                    If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                        save_record()
                    Else
                        txt_Name.Focus()
                    End If

                Else
                    .Focus()
                    .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)
                End If
            End With
        End If
    End Sub

    Private Sub cbo_GridCount_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_GridCount.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Dim f As New Count_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_GridCount.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()
        End If

    End Sub

    Private Sub cbo_GridCount_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_GridCount.TextChanged
        Try
            If cbo_GridCount.Visible Then
                If IsNothing(dgv_Details.CurrentCell) Then Exit Sub
                With dgv_Details
                    If Val(cbo_GridCount.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = 0 Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(0).Value = Trim(cbo_GridCount.Text)
                    End If
                End With
            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub cbo_Transfer_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Transfer.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, TrnTo_DbName & "..Count_Head", "Count_Name", "", "(Count_idno = 0)")
    End Sub

    Private Sub cbo_Transfer_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Transfer.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Transfer, txt_RateKg, Nothing, TrnTo_DbName & "..Count_Head", "Count_Name", "", "(Count_idno = 0)")
        If (e.KeyValue = 40 And cbo_Transfer.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
            If cbo_Sizing_CountName.Visible = True Then
                cbo_Sizing_CountName.Focus()
            Else
                If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                    save_record()
                Else
                    txt_Name.Focus()
                End If

                'If dgv_Details.Rows.Count <= 0 Then dgv_Details.Rows.Add()
                'dgv_Details.Focus()
                'dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(0)
            End If
        End If
    End Sub

    Private Sub cbo_Transfer_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Transfer.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Transfer, Nothing, TrnTo_DbName & "..Count_Head", "Count_Name", "", "(Count_idno = 0)")
        If Asc(e.KeyChar) = 13 Then
            If cbo_Sizing_CountName.Visible = True Then
                cbo_Sizing_CountName.Focus()
            Else
                If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                    save_record()
                Else
                    txt_Name.Focus()
                End If

                'If dgv_Details.Rows.Count <= 0 Then dgv_Details.Rows.Add()
                'dgv_Details.Focus()
                'dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(0)
            End If
        End If
    End Sub

    Private Sub cbo_Sizing_CountName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Sizing_CountName.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, SizTo_DbName & "..Count_Head", "Count_Name", "", "(Count_idno = 0)")
    End Sub

    Private Sub cbo_Sizing_CountName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Sizing_CountName.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Sizing_CountName, Nothing, Nothing, SizTo_DbName & "..Count_Head", "Count_Name", "", "(Count_idno = 0)")
        If (e.KeyValue = 38 And cbo_Sizing_CountName.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
            If cbo_Transfer.Visible = True Then
                cbo_Transfer.Focus()
            Else
                txt_RateKg.Focus()
            End If
        End If
        If (e.KeyValue = 40 And cbo_Sizing_CountName.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
            If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                save_record()
            Else
                txt_Name.Focus()
            End If

            'If dgv_Details.Rows.Count <= 0 Then dgv_Details.Rows.Add()
            'dgv_Details.Focus()
            'dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(0)
        End If
    End Sub

    Private Sub cbo_Sizing_CountName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Sizing_CountName.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Sizing_CountName, Nothing, SizTo_DbName & "..Count_Head", "Count_Name", "", "(Count_idno = 0)")
        If Asc(e.KeyChar) = 13 Then
            If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                save_record()
            Else
                txt_Name.Focus()
            End If

            'If dgv_Details.Rows.Count <= 0 Then dgv_Details.Rows.Add()
            'dgv_Details.Focus()
            'dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(0)
        End If
    End Sub

    Private Sub btn_RateDetails_Click(sender As System.Object, e As System.EventArgs) Handles btn_RateDetails.Click
        pnl_back.Enabled = False
        pnl_RateDetails.Visible = True
        If dgv_CountRate_Details.Enabled And dgv_CountRate_Details.Visible Then
            dgv_CountRate_Details.Focus()
            dgv_CountRate_Details.CurrentCell = dgv_CountRate_Details.Rows(0).Cells(1)
        End If
    End Sub

    Private Sub btn_Close_rate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Close_rate.Click
        Close_Count_Master_Rate_Details()
    End Sub

    Private Sub Close_Count_Master_Rate_Details()
        pnl_back.Enabled = True
        pnl_RateDetails.Visible = False
        cbo_Sizing_CountName.Focus()
    End Sub

    Private Sub dgv_CountRate_Details_CellEnter(sender As Object, e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_CountRate_Details.CellEnter
        Dim CmpGrp_Fromdate As Date


        If FrmLdSTS = True Then Exit Sub

        With dgv_CountRate_Details

            If Val(.CurrentRow.Cells(0).Value) = 0 Then
                .CurrentRow.Cells(0).Value = .CurrentRow.Index + 1
            End If
            dgv_ActiveCtrl_Name = dgv_CountRate_Details.Name

            CmpGrp_Fromdate = New DateTime(Val(Microsoft.VisualBasic.Left(Common_Procedures.FnRange, 4)), 4, 1)
            .Rows(0).Cells(1).Value = Format(DateAdd(DateInterval.Year, -1, CmpGrp_Fromdate), "dd-MM-yyyy")

        End With
    End Sub

    Private Sub dgv_CountRate_Details_CellLeave(sender As Object, e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_CountRate_Details.CellLeave
        With dgv_CountRate_Details

            If e.ColumnIndex = 1 Or e.ColumnIndex = 2 Then

                If Trim(.Rows(e.RowIndex).Cells(1).Value) <> "" Then
                    If IsDate(.Rows(e.RowIndex).Cells(1).Value) = False Then
                        .Rows(e.RowIndex).Cells(1).Value = ""
                    End If
                End If

                If Trim(.Rows(e.RowIndex).Cells(2).Value) <> "" Then
                    If IsDate(.Rows(e.RowIndex).Cells(2).Value) = False Then
                        .Rows(e.RowIndex).Cells(2).Value = ""
                    End If
                End If

            End If
        End With
    End Sub

    Private Sub dgv_CountRate_Details_EditingControlShowing(sender As Object, e As System.Windows.Forms.DataGridViewEditingControlShowingEventArgs) Handles dgv_CountRate_Details.EditingControlShowing
        dgtxt_CountRate_Details = CType(dgv_CountRate_Details.EditingControl, DataGridViewTextBoxEditingControl)
    End Sub

    Private Sub dgv_CountRate_Details_KeyUp(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles dgv_CountRate_Details.KeyUp
        Dim i As Integer
        Dim n As Integer

        If e.Control = True And UCase(Chr(e.KeyCode)) = "D" Then

            With dgv_CountRate_Details

                n = .CurrentRow.Index

                If .CurrentCell.RowIndex = .Rows.Count - 1 Then
                    For i = 0 To .ColumnCount - 1
                        .Rows(n).Cells(i).Value = ""
                    Next

                Else
                    .Rows.RemoveAt(n)

                End If

            End With

        End If
    End Sub

    Private Sub dgv_CountRate_Details_RowsAdded(sender As Object, e As System.Windows.Forms.DataGridViewRowsAddedEventArgs) Handles dgv_CountRate_Details.RowsAdded
        Dim n As Integer
        If FrmLdSTS = True Then Exit Sub

        If IsNothing(dgv_CountRate_Details.CurrentCell) Then Exit Sub
        With dgv_CountRate_Details
            n = .RowCount
            .Rows(n - 1).Cells(0).Value = Val(n)
        End With
    End Sub

    Private Sub dgv_CountRate_Details_LostFocus(sender As Object, e As System.EventArgs) Handles dgv_CountRate_Details.LostFocus
        On Error Resume Next
        If Not IsNothing(dgv_CountRate_Details.CurrentCell) Then dgv_CountRate_Details.CurrentCell.Selected = False
    End Sub

    Private Sub dgtxt_CountRate_Details_Enter(sender As Object, e As System.EventArgs) Handles dgtxt_CountRate_Details.Enter
        If FrmLdSTS = True Then Exit Sub
        dgv_ActiveCtrl_Name = dgv_CountRate_Details.Name
        dgv_CountRate_Details.EditingControl.BackColor = Color.Lime
        dgv_CountRate_Details.EditingControl.ForeColor = Color.Blue
    End Sub

    Private Sub dgtxt_CountRate_Details_KeyDown(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles dgtxt_CountRate_Details.KeyDown
        Try

            With dgv_CountRate_Details

                vcbo_KeyDwnVal = e.KeyValue

                If .Visible Then
                    If e.KeyValue <> 27 Then

                        If .CurrentCell.RowIndex = 0 And .CurrentCell.ColumnIndex = 1 Then

                            e.Handled = True
                            e.SuppressKeyPress = True

                        End If

                    End If


                End If

            End With

        Catch ex As Exception
            '---

        End Try
    End Sub

    Private Sub dgtxt_CountRate_Details_KeyPress(sender As Object, e As System.Windows.Forms.KeyPressEventArgs) Handles dgtxt_CountRate_Details.KeyPress
        With dgv_CountRate_Details
            If .Visible Then
                If .CurrentCell.ColumnIndex = 1 Then
                    If Common_Procedures.Accept_NegativeNumbers(Asc(e.KeyChar)) = 0 Then
                        e.Handled = True
                    End If
                ElseIf .CurrentCell.ColumnIndex = 2 Or .CurrentCell.ColumnIndex = 3 Then
                    If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
                        e.Handled = True
                    End If
                End If
            End If

        End With
    End Sub


    Private Sub dgv_CountRate_Details_CellValueChanged(sender As Object, e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_CountRate_Details.CellValueChanged
        Dim vDat1 As Date
        If FrmLdSTS = True Then Exit Sub

        If IsNothing(dgv_CountRate_Details.CurrentCell) Then Exit Sub
        With dgv_CountRate_Details

            If e.ColumnIndex = 1 And e.RowIndex > 0 Then

                If Trim(.Rows(e.RowIndex).Cells(1).Value) <> "" Then
                    If IsDate(.Rows(e.RowIndex).Cells(1).Value) = True Then
                        vDat1 = CDate(.Rows(e.RowIndex).Cells(1).Value)
                        .Rows(e.RowIndex - 1).Cells(2).Value = Format(DateAdd(DateInterval.Day, -1, vDat1), "dd-MM-yyyy")
                    End If
                End If

            End If

        End With
    End Sub

    Private Sub dgv_CountRate_Details_KeyDown(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles dgv_CountRate_Details.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
    End Sub

    Private Sub dgtxt_CountRate_Details_KeyUp(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles dgtxt_CountRate_Details.KeyUp
        Try
            If e.Control = True And UCase(Chr(e.KeyCode)) = "D" Then
                dgv_CountRate_Details_KeyUp(sender, e)
            End If
        Catch ex As Exception
            '---
        End Try
    End Sub

    Private Sub dgtxt_CountRate_Details_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgtxt_CountRate_Details.TextChanged
        Try
            With dgv_CountRate_Details

                If .Visible Then
                    If .Rows.Count > 0 Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(dgtxt_CountRate_Details.Text)
                    End If
                End If
            End With

        Catch ex As NullReferenceException
            '---MessageBox.Show(ex.Message, "ERROR WHILE DETAILS TEXTBOX TEXTCHANGE....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Catch ex As ObjectDisposedException
            '---MessageBox.Show(ex.Message, "ERROR WHILE DETAILS TEXTBOX TEXTCHANGE....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "ERROR WHILE DETAILS TEXTBOX TEXTCHANGE....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub


    Private Sub btn_Close_JariConsumption_Details_Click(sender As Object, e As EventArgs) Handles btn_Close_JariConsumption_Details.Click
        pnl_back.Enabled = True
        pnl_JariConsumption_Details.Visible = False
    End Sub

    Private Sub btn_show_JariConsumption_Details_Click(sender As Object, e As EventArgs) Handles btn_show_JariConsumption_Details.Click
        pnl_JariConsumption_Details.Visible = True
        pnl_back.Enabled = False
        If dgv_Details.Rows.Count <= 0 Then dgv_Details.Rows.Add()
        dgv_Details.Focus()
        dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(0)
    End Sub

    Private Sub cbo_Sizing_CountName_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbo_Sizing_CountName.SelectedIndexChanged

    End Sub

    Private Sub cbo_stockunder_GotFocus(sender As Object, e As EventArgs) Handles cbo_stockunder.GotFocus

    End Sub

    Private Sub txt_RateKg_TextChanged(sender As Object, e As EventArgs) Handles txt_RateKg.TextChanged

    End Sub
End Class