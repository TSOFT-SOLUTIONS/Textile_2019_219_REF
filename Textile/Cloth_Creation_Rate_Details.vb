Public Class Cloth_Creation_Rate_Details
    Implements Interface_MDIActions

    Private con As New SqlClient.SqlConnection(Common_Procedures.Connection_String)
    Private New_Entry As Boolean = False
    Private Prec_ActCtrl As New Control
    Private vcbo_KeyDwnVal As Double
    Private FrmLdSTS As Boolean = False
    Private WithEvents dgtxt_SalesRate_Details As New DataGridViewTextBoxEditingControl
    Private dgv_ActiveCtrl_Name As String

    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

    End Sub

    Private Sub clear()

        New_Entry = False

        pnl_Back.Enabled = True
        grp_Find.Visible = False
        grp_Filter.Visible = False

        'Me.Height = 274

        lbl_IdNo.Text = ""
        lbl_IdNo.ForeColor = Color.Black

        cbo_ClothName.Text = ""
        cbo_Find.Text = ""

        dgv_SalesRate_Details.Rows.Clear()
        'dgv_Filter.Rows.Clear()

    End Sub
    Private Sub ControlGotFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim txtbx As TextBox
        Dim combobx As ComboBox

        On Error Resume Next

        If TypeOf Me.ActiveControl Is TextBox Or TypeOf Me.ActiveControl Is ComboBox Then
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
            End If
        End If

    End Sub

    Private Sub Grid_DeSelect()
        If FrmLdSTS = True Then Exit Sub
        On Error Resume Next
        If Not IsNothing(dgv_SalesRate_Details.CurrentCell) Then dgv_SalesRate_Details.CurrentCell.Selected = False
    End Sub
    Public Sub move_record(ByVal idno As Integer)
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim dt4 As New DataTable
        Dim slno, n, Sno As Integer

        If Val(idno) = 0 Then Exit Sub

        clear()


        da = New SqlClient.SqlDataAdapter("select a.* from Cloth_Head a Where a.Cloth_IdNo = " & Str(Val(idno)), con)
        dt = New DataTable
        da.Fill(dt)

        If dt.Rows.Count > 0 Then
            lbl_IdNo.Text = dt.Rows(0)("Cloth_IdNo").ToString
            cbo_ClothName.Text = Common_Procedures.Cloth_IdNoToName(con, Val(dt.Rows(0).Item("Cloth_IdNo").ToString))


            da = New SqlClient.SqlDataAdapter("Select a.* from Cloth_Master_Sales_Rate_Details a Where a.Cloth_IdNo = " & Str(Val(idno)) & " Order by a.FromDate_DateTime, a.ToDate_DateTime, a.sl_no", con)
            dt4 = New DataTable
            da.Fill(dt4)

            With dgv_SalesRate_Details

                .Rows.Clear()
                Sno = 0

                If dt4.Rows.Count > 0 Then

                    For i = 0 To dt4.Rows.Count - 1

                        n = .Rows.Add()

                        Sno = Sno + 1

                        .Rows(n).Cells(0).Value = Val(Sno)
                        .Rows(n).Cells(1).Value = dt4.Rows(i).Item("FromDate_Text").ToString
                        .Rows(n).Cells(2).Value = dt4.Rows(i).Item("ToDate_Text").ToString
                        .Rows(n).Cells(3).Value = Format(Val(dt4.Rows(i).Item("Type1_Sales_Rate").ToString), "########0.00")
                        .Rows(n).Cells(4).Value = Format(Val(dt4.Rows(i).Item("Type2_Sales_Rate").ToString), "########0.00")
                        .Rows(n).Cells(5).Value = Format(Val(dt4.Rows(i).Item("Type3_Sales_Rate").ToString), "########0.00")
                        .Rows(n).Cells(6).Value = Format(Val(dt4.Rows(i).Item("Type4_Sales_Rate").ToString), "########0.00")
                        .Rows(n).Cells(7).Value = Format(Val(dt4.Rows(i).Item("Type5_Sales_Rate").ToString), "########0.00")

                    Next i

                End If

            End With
            dt4.Clear()

        Else
            new_record()

        End If




        dt.Clear()
        dt.Dispose()
        da.Dispose()




    End Sub

    Private Sub Cloth_Creaion_Rate_Details_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Me.Height = 284 ' 197

        con.Open()
        dgv_SalesRate_Details.Columns(3).HeaderText = Trim(UCase(Common_Procedures.ClothType.Type1))
        dgv_SalesRate_Details.Columns(4).HeaderText = Trim(UCase(Common_Procedures.ClothType.Type2))
        dgv_SalesRate_Details.Columns(5).HeaderText = Trim(UCase(Common_Procedures.ClothType.Type3))
        dgv_SalesRate_Details.Columns(6).HeaderText = Trim(UCase(Common_Procedures.ClothType.Type4))
        dgv_SalesRate_Details.Columns(7).HeaderText = Trim(UCase(Common_Procedures.ClothType.Type5))



        grp_Find.Visible = False
        grp_Find.Left = (Me.Width - grp_Find.Width) \ 2
        grp_Find.Top = (Me.Height - grp_Find.Height) \ 2

        AddHandler cbo_ClothName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Find.LostFocus, AddressOf ControlLostFocus

        AddHandler cbo_ClothName.GotFocus, AddressOf ControlGotFocus

        dgv_SalesRate_Details.Visible = True

        new_record()

    End Sub

    Private Sub Cloth_Creation_Rate_Details_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles MyBase.KeyPress
        If Asc(e.KeyChar) = 27 Then
            If grp_Find.Visible Then
                btnClose_Click(sender, e)
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

    Private Sub Cloth_Creation_Rate_Details_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles MyBase.FormClosed
        On Error Resume Next
        con.Close()
        con.Dispose()
        Common_Procedures.Last_Closed_FormName = Me.Name
    End Sub

    Private Sub btn_Close_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Me.Close()
        Me.Dispose()
    End Sub

    Public Sub delete_record() Implements Interface_MDIActions.delete_record
        Dim cmd As New SqlClient.SqlCommand
        Dim da As SqlClient.SqlDataAdapter
        Dim Dt As DataTable
        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.DeletingEntry, Common_Procedures.UR.cloth_Creation, New_Entry, Me) = False Then Exit Sub

        If pnl_Back.Enabled = False Then
            MessageBox.Show("Close other window", "DOES NOT SAVE", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If


        'If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.Master_Area_Creation, "~L~") = 0 And InStr(Common_Procedures.UR.Master_Area_Creation, "~D~") = 0 Then MessageBox.Show("You have No Rights to Delete", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error) : Exit Sub
        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.DeletingEntry, Common_Procedures.UR.cloth_Creation, New_Entry, Me) = False Then Exit Sub

        If MessageBox.Show("Do you want to Delete ?", "FOR DELETION....", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) <> Windows.Forms.DialogResult.Yes Then
            Exit Sub
        End If

        Try


            cmd.Connection = con
            cmd.CommandText = "delete from Cloth_Master_Sales_Rate_Details where Cloth_IdNo = " & Str(Val(lbl_IdNo.Text))

            cmd.ExecuteNonQuery()

            cmd.Dispose()

            new_record()

            MessageBox.Show("Deleted Sucessfully!!!", "FOR DELETION...", MessageBoxButtons.OK, MessageBoxIcon.Information)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally
            If cbo_ClothName.Enabled And cbo_ClothName.Visible Then cbo_ClothName.Focus()

        End Try

    End Sub

    Public Sub filter_record() Implements Interface_MDIActions.filter_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        da = New SqlClient.SqlDataAdapter("select Cloth_IdNo, Cloth_Name from Cloth_Head where Cloth_IdNo <> 0 order by Cloth_IdNo", con)


        da.Fill(dt)

        With dgv_Filter

            .Columns.Clear()
            .DataSource = dt

            .RowHeadersVisible = False

            .AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells
            .AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill

            .Columns(0).HeaderText = "IDNO"
            .Columns(1).HeaderText = "CLOTH NAME"

            .Columns(0).FillWeight = 40
            .Columns(1).FillWeight = 160

        End With

        new_record()

        grp_Filter.Visible = True
        grp_Filter.Left = grp_Find.Left
        grp_Filter.Top = grp_Find.Top

        pnl_Back.Enabled = False

        If dgv_Filter.Enabled And dgv_Filter.Visible Then dgv_Filter.Focus()

        'Me.Height = 520 ' 400

        dt.Dispose()
        da.Dispose()

    End Sub

    Public Sub movefirst_record() Implements Interface_MDIActions.movefirst_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim movid As Integer = 0

        Try
            da = New SqlClient.SqlDataAdapter("select min(Cloth_IdNo) from Cloth_Head Where Cloth_IdNo <> 0", con)
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
        Dim cmd As New SqlClient.SqlCommand
        Dim dr As SqlClient.SqlDataReader
        Dim movid As Integer

        Try
            cmd.Connection = con
            cmd.CommandText = "select max(Cloth_IdNo) from Cloth_Head WHERE Cloth_IdNo <> 0"

            dr = cmd.ExecuteReader

            movid = 0
            If dr.HasRows Then
                If dr.Read() Then
                    If IsDBNull(dr(0).ToString) = False Then
                        movid = Val(dr(0).ToString)
                    End If
                End If
            End If

            dr.Close()
            cmd.Dispose()

            If movid <> 0 Then move_record(movid)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT MOVE....", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub

        End Try

    End Sub

    Public Sub movenext_record() Implements Interface_MDIActions.movenext_record
        Dim cmd As New SqlClient.SqlCommand
        Dim dr As SqlClient.SqlDataReader
        Dim movid As Integer

        Try
            cmd.Connection = con
            cmd.CommandText = "select min(Cloth_IdNo) from Cloth_Head where Cloth_IdNo > " & Str(Val(lbl_IdNo.Text))

            dr = cmd.ExecuteReader()

            movid = 0
            If dr.HasRows Then
                If dr.Read() Then
                    If IsDBNull(dr(0).ToString) = False Then
                        movid = Val(dr(0).ToString)
                    End If
                End If
            End If

            dr.Close()
            cmd.Dispose()

            If movid <> 0 Then move_record(movid)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT MOVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub

        End Try

    End Sub

    Public Sub moveprevious_record() Implements Interface_MDIActions.moveprevious_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim movid As Integer

        Try
            da = New SqlClient.SqlDataAdapter("select max(Cloth_IdNo) from Cloth_Head where Cloth_IdNo < " & Str(Val(lbl_IdNo.Text)) & " and Cloth_IdNo <> 0 ", con)
            da.Fill(dt)

            movid = 0
            If dt.Rows.Count > 0 Then
                If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                    movid = Val(dt.Rows(0)(0).ToString)
                End If
            End If

            dt.Dispose()
            da.Dispose()

            If movid <> 0 Then move_record(movid)

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT MOVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub

        End Try

    End Sub

    Public Sub new_record() Implements Interface_MDIActions.new_record
        'Dim cmd As New SqlClient.SqlCommand
        'Dim dr As SqlClient.SqlDataReader
        'Dim newno As Integer

        clear()

        New_Entry = True
        lbl_IdNo.ForeColor = Color.Red

        lbl_IdNo.Text = Common_Procedures.get_MaxIdNo(con, "Cloth_Head", "Cloth_IdNo", "")

        If cbo_ClothName.Enabled And cbo_ClothName.Visible Then cbo_ClothName.Focus()

    End Sub

    Public Sub open_record() Implements Interface_MDIActions.open_record
        Dim da As New SqlClient.SqlDataAdapter("select Cloth_Name from Cloth_Head order by Cloth_Name", con)
        Dim dt As New DataTable

        da.Fill(dt)

        cbo_Find.DataSource = dt
        cbo_Find.DisplayMember = "Cloth_Name"

        new_record()

        grp_Find.Visible = True
        pnl_Back.Enabled = False

        If cbo_Find.Enabled And cbo_Find.Visible Then cbo_Find.Focus()
        'Me.Height = 480 ' 355

    End Sub

    Public Sub insert_record() Implements Interface_MDIActions.insert_record
        'MessageBox.Show("insert record")
    End Sub

    Public Sub print_record() Implements Interface_MDIActions.print_record
        '--- No Printing
    End Sub

    Public Sub save_record() Implements Interface_MDIActions.save_record
        Dim cmd As New SqlClient.SqlCommand
        Dim trans As SqlClient.SqlTransaction
        Dim Sur As String
        Dim vBlank_ToDate_Count As Integer = 0
        Dim vFrmDate1 As Date
        Dim vToDate1 As Date
        Dim vToDate1STS As Boolean = False
        Dim vToDate2STS As Boolean = False
        Dim vSTS As Boolean = False
        Dim vFrmDate2 As Date
        Dim vToDate2 As Date
        Dim vCloth_ID As Integer
        Dim Sno As Integer = 0

        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.SavingEntry, Common_Procedures.UR.Area_Creation, New_Entry, Me) = False Then Exit Sub

        vCloth_ID = Common_Procedures.Cloth_NameToIdNo(con, cbo_ClothName.Text)

        If Val(vCloth_ID) = 0 Then
            MessageBox.Show("Invalid Cloth Name", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If cbo_ClothName.Enabled Then cbo_ClothName.Focus()
            Exit Sub
        End If

        If pnl_Back.Enabled = False Then
            MessageBox.Show("Close other window", "DOES NOT SAVE", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.SavingEntry, Common_Procedures.UR.Area_Creation, New_Entry, Me) = False Then Exit Sub

        If Trim(cbo_ClothName.Text) = "" Then
            MessageBox.Show("Invalid Name", "DOES NOT SAVE", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
        End If


        With dgv_SalesRate_Details

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

                If vSTS = True And (Val(.Rows(i).Cells(3).Value) <> 0 Or Val(.Rows(i).Cells(4).Value) <> 0 Or Val(.Rows(i).Cells(5).Value) <> 0 Or Val(.Rows(i).Cells(6).Value) <> 0 Or Val(.Rows(i).Cells(7).Value) <> 0) Then

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
                        'pnl_SalesRate_Details.Visible = True
                        'If dgv_SalesRate_Details.Enabled And dgv_SalesRate_Details.Visible Then
                        '    dgv_SalesRate_Details.Focus()
                        '    dgv_SalesRate_Details.CurrentCell = dgv_SalesRate_Details.Rows(i).Cells(1)
                        'End If
                        'Exit Sub

                    Else

                        If DateDiff(DateInterval.Day, vToDate1, vFrmDate1) > 0 Then


                            MessageBox.Show("Invalid Date in Rate Details" & Chr(13) & "To Date lesser than from date", "DOES NOT SAVE", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                            pnl_Back.Enabled = False
                            pnl_SalesRate_Details.Visible = True
                            If dgv_SalesRate_Details.Enabled And dgv_SalesRate_Details.Visible Then
                                dgv_SalesRate_Details.Focus()
                                dgv_SalesRate_Details.CurrentCell = dgv_SalesRate_Details.Rows(i).Cells(1)
                            End If
                            Exit Sub

                        End If

                    End If

                    For j = i + 1 To .RowCount - 1

                        If j <> i Then

                            vFrmDate2 = #12:00:00 PM#
                            vToDate2 = #12:00:00 PM#

                            vToDate2STS = False

                            vSTS = False
                            If Trim(.Rows(j).Cells(1).Value) <> "" Then
                                If IsDate(.Rows(j).Cells(1).Value) = True Then
                                    vSTS = True
                                    vFrmDate2 = CDate(.Rows(j).Cells(1).Value)
                                End If
                            End If


                            If vSTS = True And (Val(.Rows(j).Cells(3).Value) <> 0 Or Val(.Rows(j).Cells(4).Value) <> 0 Or Val(.Rows(j).Cells(5).Value) <> 0 Or Val(.Rows(j).Cells(6).Value) <> 0 Or Val(.Rows(j).Cells(7).Value) <> 0) Then

                                If DateDiff(DateInterval.Day, vFrmDate2, vFrmDate1) > 0 Then

                                    MessageBox.Show("Invalid Date in Rate Details - from date should be grater than previous date ", "DOES NOT SAVE", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                                    pnl_SalesRate_Details.Visible = True
                                    If dgv_SalesRate_Details.Enabled And dgv_SalesRate_Details.Visible Then
                                        dgv_SalesRate_Details.Focus()
                                        dgv_SalesRate_Details.CurrentCell = dgv_SalesRate_Details.Rows(j).Cells(1)
                                    End If
                                    Exit Sub

                                End If

                            End If

                        End If

                    Next j

                End If

            Next i

            If vBlank_ToDate_Count > 1 Then

                MessageBox.Show("Invalid To Date in Rate Details", "DOES NOT SAVE", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                pnl_Back.Enabled = False
                pnl_SalesRate_Details.Visible = True
                If dgv_SalesRate_Details.Enabled And dgv_SalesRate_Details.Visible Then
                    dgv_SalesRate_Details.Focus()
                    dgv_SalesRate_Details.CurrentCell = dgv_SalesRate_Details.Rows(0).Cells(1)
                End If
                Exit Sub
            End If

        End With

        Sur = Common_Procedures.Remove_NonCharacters(Trim(cbo_ClothName.Text))

        trans = con.BeginTransaction

        Try
            cmd.Connection = con
            cmd.Transaction = trans

            cmd.CommandText = "delete from Cloth_Master_Sales_Rate_Details where Cloth_IdNo = " & Str(Val(lbl_IdNo.Text))
            cmd.ExecuteNonQuery()

            Sno = 0

            With dgv_SalesRate_Details


                For i = 0 To .RowCount - 1

                    vSTS = False

                    cmd.Parameters.Clear()

                    If Trim(.Rows(i).Cells(1).Value) <> "" Then
                        If IsDate(.Rows(i).Cells(1).Value) = True Then
                            cmd.Parameters.AddWithValue("@FromDate", CDate(.Rows(i).Cells(1).Value))
                            vSTS = True
                        End If
                    End If

                    If vSTS = True And (Val(.Rows(i).Cells(3).Value) <> 0 Or Val(.Rows(i).Cells(4).Value) <> 0 Or Val(.Rows(i).Cells(5).Value) <> 0 Or Val(.Rows(i).Cells(6).Value) <> 0 Or Val(.Rows(i).Cells(7).Value) <> 0) Then

                        If Trim(.Rows(i).Cells(2).Value) <> "" Then
                            If IsDate(.Rows(i).Cells(2).Value) = True Then
                                cmd.Parameters.AddWithValue("@toDate", CDate(.Rows(i).Cells(2).Value))
                            End If
                        End If

                        Sno = Sno + 1

                        cmd.CommandText = "Insert into Cloth_Master_Sales_Rate_Details (              Cloth_IdNo        ,           Sl_No      ,                     FromDate_Text       ,                                              FromDate_DateTime           ,                    ToDate_Text          ,                                            ToDate_DateTime             ,                      Type1_Sales_Rate     ,          Type2_Sales_Rate                  ,                       Type3_Sales_Rate    ,                       Type4_Sales_Rate    ,                      Type5_Sales_Rate     )  " &
                                                " Values                                   ( " & Str(Val(lbl_IdNo.Text)) & ", " & Str(Val(Sno)) & ", '" & Trim(.Rows(i).Cells(1).Value) & "' , " & IIf(IsDate(.Rows(i).Cells(1).Value) = True, "@fromDate", "Null") & " , '" & Trim(.Rows(i).Cells(2).Value) & "' , " & IIf(IsDate(.Rows(i).Cells(2).Value) = True, "@toDate", "Null") & " , " & Str(Val(.Rows(i).Cells(3).Value)) & " ,   " & Str(Val(.Rows(i).Cells(4).Value)) & ",  " & Str(Val(.Rows(i).Cells(5).Value)) & ",  " & Str(Val(.Rows(i).Cells(6).Value)) & ", " & Str(Val(.Rows(i).Cells(7).Value)) & " ) "
                        cmd.ExecuteNonQuery()

                        cmd.CommandText = "Update Cloth_Head set Sound_Rate = " & Val(.Rows(i).Cells(3).Value) & ", Seconds_Rate = " & Val(.Rows(i).Cells(4).Value) & " , Bits_Rate = " & Val(.Rows(i).Cells(5).Value) & " , Reject_Rate =" & Val(.Rows(i).Cells(6).Value) & " , Other_Rate = " & Val(.Rows(i).Cells(7).Value) & " Where Cloth_Idno = " & Str(Val(lbl_IdNo.Text))
                        cmd.ExecuteNonQuery()

                    End If

                Next

            End With

            trans.Commit()

            Common_Procedures.Master_Return.Return_Value = Trim(cbo_ClothName.Text)
            Common_Procedures.Master_Return.Master_Type = "CLOTH"


            MessageBox.Show("Saved Sucessfully!!!", "FOR SAVING....", MessageBoxButtons.OKCancel, MessageBoxIcon.Information)

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

            If InStr(1, Trim(LCase(ex.Message)), Trim(LCase("ix_Cloth_Sales_Rate_Head"))) > 0 Then
                MessageBox.Show("Duplicate Cloth Name", "DOES NOT SAVE", MessageBoxButtons.OK, MessageBoxIcon.Error)

            Else
                MessageBox.Show(ex.Message, "DOES NOT SAVE", MessageBoxButtons.OK, MessageBoxIcon.Error)

            End If

        Finally
            If cbo_ClothName.Enabled And cbo_ClothName.Visible Then cbo_ClothName.Focus()

        End Try

    End Sub
    Protected Overrides Function ProcessCmdKey(ByRef msg As System.Windows.Forms.Message, ByVal keyData As System.Windows.Forms.Keys) As Boolean
        Dim dgv1 As New DataGridView

        If ActiveControl.Name = dgv_SalesRate_Details.Name Or TypeOf ActiveControl Is DataGridViewTextBoxEditingControl Then

            On Error Resume Next

            dgv1 = Nothing
            If ActiveControl.Name = dgv_SalesRate_Details.Name Then
                dgv1 = dgv_SalesRate_Details

            ElseIf dgv_SalesRate_Details.IsCurrentRowDirty = True Then
                dgv1 = dgv_SalesRate_Details


            ElseIf dgv_ActiveCtrl_Name = dgv_SalesRate_Details.Name Then
                dgv1 = dgv_SalesRate_Details

            End If


            If IsNothing(dgv1) = True Then
                Return MyBase.ProcessCmdKey(msg, keyData)
                Exit Function
            End If

            With dgv1
                If dgv1.Name = dgv_SalesRate_Details.Name Then

                    If keyData = Keys.Enter Or keyData = Keys.Down Then
                        If .CurrentCell.ColumnIndex >= .ColumnCount - 1 Then
                            If .CurrentCell.RowIndex = .RowCount - 1 Then
                                'btn_Save.Focus()
                                save_record()


                            Else
                                .Focus()
                                .CurrentCell = .Rows(.CurrentCell.RowIndex + 1).Cells(1)


                            End If

                        Else

                            If .CurrentCell.RowIndex = .RowCount - 1 And .CurrentCell.ColumnIndex >= 1 And ((.CurrentCell.ColumnIndex <> 1 And Val(.CurrentRow.Cells(1).Value) = 0) Or (.CurrentCell.ColumnIndex = 1 And Val(dgtxt_SalesRate_Details.Text) = 0)) Then
                                For i = 0 To .Columns.Count - 1
                                    .Rows(.CurrentCell.RowIndex).Cells(i).Value = ""
                                Next

                                save_record()
                                'Close_SalesRate_Details()

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
                                cbo_ClothName.Focus()

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

    Private Sub btn_Save_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        save_record()
    End Sub

    Private Sub txt_Name_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
        Dim K As Integer

        If Asc(e.KeyChar) >= 97 And Asc(e.KeyChar) <= 122 Then
            K = Asc(e.KeyChar)
            K = K - 32
            e.KeyChar = Chr(K)
        End If

        If Asc(e.KeyChar) = 13 Then
            If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                save_record()
            End If
        End If
    End Sub

    Private Sub dgv_SalesRate_Details_CellEnter(sender As Object, e As DataGridViewCellEventArgs) Handles dgv_SalesRate_Details.CellEnter
        Dim CmpGrp_Fromdate As Date


        If FrmLdSTS = True Then Exit Sub
        With dgv_SalesRate_Details

            If Val(.CurrentRow.Cells(0).Value) = 0 Then
                .CurrentRow.Cells(0).Value = .CurrentRow.Index + 1
            End If
            dgv_ActiveCtrl_Name = dgv_SalesRate_Details.Name

            CmpGrp_Fromdate = New DateTime(Val(Microsoft.VisualBasic.Left(Common_Procedures.FnRange, 4)), 4, 1)
            .Rows(0).Cells(1).Value = Format(DateAdd(DateInterval.Year, -1, CmpGrp_Fromdate), "dd-MM-yyyy")

        End With
    End Sub
    Private Sub dgv_SalesRate_Details_CellLeave(sender As Object, e As DataGridViewCellEventArgs) Handles dgv_SalesRate_Details.CellLeave
        If FrmLdSTS = True Then Exit Sub

        With dgv_SalesRate_Details

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
    Private Sub dgv_SalesRate_Details_CellValueChanged(sender As Object, e As DataGridViewCellEventArgs) Handles dgv_SalesRate_Details.CellValueChanged
        Dim vDat1 As Date
        If FrmLdSTS = True Then Exit Sub

        If IsNothing(dgv_SalesRate_Details.CurrentCell) Then Exit Sub
        With dgv_SalesRate_Details

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


    Private Sub dgv_SalesRate_Details_EditingControlShowing(sender As Object, e As DataGridViewEditingControlShowingEventArgs) Handles dgv_SalesRate_Details.EditingControlShowing
        dgtxt_SalesRate_Details = CType(dgv_SalesRate_Details.EditingControl, DataGridViewTextBoxEditingControl)
    End Sub
    Private Sub dgtxt_SalesRate_Details_Enter(sender As Object, e As EventArgs) Handles dgtxt_SalesRate_Details.Enter
        Try
            dgv_ActiveCtrl_Name = dgv_SalesRate_Details.Name

            dgv_SalesRate_Details.EditingControl.BackColor = Color.Lime
            dgv_SalesRate_Details.EditingControl.ForeColor = Color.Blue
            dgv_SalesRate_Details.SelectAll()
        Catch ex As Exception
            '--
        End Try
    End Sub
    Private Sub dgtxt_SalesRate_Details_KeyDown(sender As Object, e As KeyEventArgs) Handles dgtxt_SalesRate_Details.KeyDown
        Try

            With dgv_SalesRate_Details

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

    Private Sub dgtxt_SalesRate_Details_KeyPress(sender As Object, e As KeyPressEventArgs) Handles dgtxt_SalesRate_Details.KeyPress
        Try
            With dgv_SalesRate_Details
                If .Visible Then

                    If .CurrentCell.RowIndex = 0 And .CurrentCell.ColumnIndex = 1 Then
                        e.Handled = True

                    Else

                        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
                            e.Handled = True
                        End If

                    End If

                End If
            End With
        Catch ex As Exception
            '---
        End Try
    End Sub

    Private Sub dgtxt_SalesRate_Details_KeyUp(sender As Object, e As KeyEventArgs) Handles dgtxt_SalesRate_Details.KeyUp
        Try
            If e.Control = True And UCase(Chr(e.KeyCode)) = "D" Then
                dgv_SalesRate_Details_KeyUp(sender, e)
            End If
        Catch ex As Exception
            '---
        End Try
    End Sub
    Private Sub dgv_SalesRate_Details_KeyDown(sender As Object, e As KeyEventArgs) Handles dgv_SalesRate_Details.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
    End Sub

    Private Sub dgv_SalesRate_Details_KeyUp(sender As Object, e As KeyEventArgs) Handles dgv_SalesRate_Details.KeyUp
        Dim n As Integer

        Try
            If e.Control = True And UCase(Chr(e.KeyCode)) = "D" Then
                With dgv_SalesRate_Details

                    n = .CurrentRow.Index

                    If n = .Rows.Count - 1 Then
                        For i = 0 To .ColumnCount - 1
                            .Rows(n).Cells(i).Value = ""
                        Next

                    Else
                        .Rows.RemoveAt(n)

                    End If

                End With

            End If

        Catch ex As Exception
            '---
        End Try
    End Sub



    Private Sub dgv_SalesRate_Details_LostFocus(sender As Object, e As EventArgs) Handles dgv_SalesRate_Details.LostFocus
        On Error Resume Next
        If Not IsNothing(dgv_SalesRate_Details.CurrentCell) Then dgv_SalesRate_Details.CurrentCell.Selected = False
    End Sub

    Private Sub dgv_SalesRate_Details_RowsAdded(sender As Object, e As DataGridViewRowsAddedEventArgs) Handles dgv_SalesRate_Details.RowsAdded
        If FrmLdSTS = True Then Exit Sub
        Dim n As Integer = 0
        Try

            If IsNothing(dgv_SalesRate_Details.CurrentCell) Then Exit Sub
            With dgv_SalesRate_Details
                n = .RowCount
                .Rows(n - 1).Cells(0).Value = Val(n)
            End With
        Catch ex As Exception
            '---
        End Try

    End Sub
    Private Sub dgtxt_SalesRate_Details_TextChanged(sender As Object, e As EventArgs) Handles dgtxt_SalesRate_Details.TextChanged
        Try
            With dgv_SalesRate_Details

                If .Visible Then
                    If .Rows.Count > 0 Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(dgtxt_SalesRate_Details.Text)
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

    Private Sub btn_Save_Click_1(sender As Object, e As EventArgs) Handles btn_Save.Click
        save_record()
    End Sub

    Private Sub btn_Close_Click_1(sender As Object, e As EventArgs) Handles btn_Close.Click
        Me.Close()
        Me.Dispose()
    End Sub
    Private Sub cbo_ClothName_GotFocus(sender As Object, e As EventArgs) Handles cbo_ClothName.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Cloth_Head", "Cloth_Name", "", "(Cloth_Idno = 0)")
    End Sub

    Private Sub cbo_ClothName_KeyDown(sender As Object, e As KeyEventArgs) Handles cbo_ClothName.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_ClothName, Nothing, dgv_SalesRate_Details, "Cloth_Head", "Cloth_Name", "", "(Cloth_Idno=0)")

    End Sub

    Private Sub cbo_ClothName_KeyPress(sender As Object, e As KeyPressEventArgs) Handles cbo_ClothName.KeyPress
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim movid As Integer = 0
        Dim vCLONM As String = ""

        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_ClothName, Nothing, "Cloth_Head", "Cloth_Name", "", "(Cloth_Idno = 0)")

        Try

            If Asc(e.KeyChar) = 13 Then

                vCLONM = Trim(cbo_ClothName.Text)

                da = New SqlClient.SqlDataAdapter("select Cloth_Idno from Cloth_Head where Cloth_Name = '" & Trim(vCLONM) & "'", con)
                dt = New DataTable
                da.Fill(dt)
                movid = 0
                If dt.Rows.Count > 0 Then
                    If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                        movid = Val(dt.Rows(0)(0).ToString)
                    End If
                End If

                If movid <> 0 Then
                    move_record(movid)
                    If dgv_SalesRate_Details.Rows.Count > 0 Then
                        dgv_SalesRate_Details.Focus()
                        dgv_SalesRate_Details.CurrentCell = dgv_SalesRate_Details.Rows(0).Cells(1)
                    Else
                        cbo_ClothName.Focus()
                    End If


                Else

                    new_record()
                    cbo_ClothName.Text = vCLONM
                    cbo_ClothName.Focus()

                End If

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR FINDING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub cbo_ClothName_KeyUp(sender As Object, e As KeyEventArgs) Handles cbo_ClothName.KeyUp
        If e.Control = False And e.KeyValue = 17 Then
            Dim f As New Cloth_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_ClothName.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()
        End If
    End Sub

    Private Sub btn_CloseFilter_Click(sender As Object, e As EventArgs) Handles btn_CloseFilter.Click
        pnl_Back.Enabled = True
        grp_Filter.Visible = False
        If cbo_ClothName.Enabled And cbo_ClothName.Visible Then cbo_ClothName.Focus()

        'Me.Height = 284 '197
    End Sub

    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click

        pnl_Back.Enabled = True
        grp_Find.Visible = False
        If cbo_ClothName.Enabled And cbo_ClothName.Visible Then cbo_ClothName.Focus()
        'Me.Height = 284 ' 197
    End Sub

    Private Sub btn_Find_Click(sender As Object, e As EventArgs) Handles btn_Find.Click
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim movid As Integer

        da = New SqlClient.SqlDataAdapter("select Cloth_IdNo from Cloth_Head where Cloth_Name = '" & Trim(cbo_Find.Text) & "'", con)
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

        btnClose_Click(sender, e)

    End Sub

    Private Sub btn_Open_Click(sender As Object, e As EventArgs) Handles btn_Open.Click
        Dim movid As Integer

        movid = 0
        If IsDBNull((dgv_Filter.CurrentRow.Cells(0).Value)) = False Then
            movid = Val(dgv_Filter.CurrentRow.Cells(0).Value)
        End If

        If Val(movid) <> 0 Then
            move_record(movid)
            'btn_CloseFilter_Click(sender, e)
        End If

    End Sub

    Private Sub dgv_Filter_KeyDown(sender As Object, e As KeyEventArgs) Handles dgv_Filter.KeyDown
        If e.KeyCode = Keys.Enter Then
            btn_Open_Click(sender, e)
        End If
    End Sub

    Private Sub dgv_Filter_DoubleClick(sender As Object, e As EventArgs) Handles dgv_Filter.DoubleClick
        btn_Open_Click(sender, e)
    End Sub

    Private Sub cbo_Find_GotFocus(sender As Object, e As EventArgs) Handles cbo_Find.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Cloth_Head", "Cloth_Name", "", "")
    End Sub
    Private Sub cbo_Find_KeyDown(sender As Object, e As KeyEventArgs) Handles cbo_Find.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Find, Nothing, Nothing, "Cloth_Head", "Cloth_Name", "", "")
    End Sub

    Private Sub cbo_Find_KeyPress(sender As Object, e As KeyPressEventArgs) Handles cbo_Find.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Find, Nothing, "Cloth_Head", "Cloth_Name", "", "")

        If Asc(e.KeyChar) = 13 Then
            btn_Find_Click(sender, e)

        End If
    End Sub
End Class