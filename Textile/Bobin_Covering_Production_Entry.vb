Public Class Bobin_Covering_Production_Entry
    Implements Interface_MDIActions

    Private con As New SqlClient.SqlConnection(Common_Procedures.Connection_String)
    Private FrmLdSTS As Boolean = False
    Private New_Entry As Boolean = False

    Private Pk_Condition As String = "CPE-"

    Private Insert_Entry As Boolean = False
    Private Filter_Status As Boolean = False

    Private Prec_ActCtrl As New Control
    Public vmskOldText As String = ""
    Public vmskSelStrt As Integer = -1

    Private dgv_ActiveCtrl_Name As String

    Private WithEvents dgtxt_Details As New DataGridViewTextBoxEditingControl

    Private vcbo_KeyDwnVal As Double
    Private vCbo_ItmNm As String


    Public Sub New()
        ' This call is required by the designer.
        FrmLdSTS = True
        InitializeComponent()
        ' Add any initialization after the InitializeComponent() call.
        clear()
    End Sub

    Public Sub clear()
        New_Entry = False
        Insert_Entry = False

        lbl_Refno.Text = ""
        msk_date.Text = ""
        dtp_date.Text = ""

        vmskOldText = ""
        vmskSelStrt = -1
        lbl_Refno.Text = ""
        lbl_Refno.ForeColor = Color.Black

        pnl_back.Enabled = True
        pnl_Filter.Visible = False

        dgv_details.Rows.Clear()

        dgv_details_total.Rows.Clear()
        dgv_details_total.Rows.Add()

        If Filter_Status = False Then
            dtp_Filter_Fromdate.Text = Common_Procedures.Company_FromDate
            dtp_Filter_ToDate.Text = Common_Procedures.Company_ToDate
            cbo_Filter_EmployeeName.Text = ""
            cbo_Filter_EmployeeName.SelectedIndex = -1
            dgv_Filter_Details.Rows.Clear()
        End If
        Grid_Cell_DeSelect()
        dgv_ActiveCtrl_Name = ""
    End Sub
    Private Sub ControlGotFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim txtbx As TextBox
        Dim combobx As ComboBox
        Dim msktxbx As MaskedTextBox
        On Error Resume Next

        If TypeOf Me.ActiveControl Is TextBox Or TypeOf Me.ActiveControl Is ComboBox Or TypeOf Me.ActiveControl Is Button Or TypeOf Me.ActiveControl Is MaskedTextBox Then
            Me.ActiveControl.BackColor = Color.Lime
            Me.ActiveControl.ForeColor = Color.Blue
        End If

        If TypeOf Me.ActiveControl Is TextBox Then
            txtbx = Me.ActiveControl
            txtbx.SelectAll()
        ElseIf TypeOf Me.ActiveControl Is MaskedTextBox Then
            msktxbx = Me.ActiveControl
            msktxbx.SelectionStart = 0
        ElseIf TypeOf Me.ActiveControl Is ComboBox Then
            combobx = Me.ActiveControl
            combobx.SelectAll()
        End If



        Grid_Cell_DeSelect()

        Prec_ActCtrl = Me.ActiveControl

    End Sub
    Private Sub ControlLostFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        On Error Resume Next
        If IsNothing(Prec_ActCtrl) = False Then
            If TypeOf Prec_ActCtrl Is TextBox Or TypeOf Prec_ActCtrl Is ComboBox Or TypeOf Prec_ActCtrl Is MaskedTextBox Then
                Prec_ActCtrl.BackColor = Color.White
                Prec_ActCtrl.ForeColor = Color.Black
            ElseIf TypeOf Prec_ActCtrl Is Button Then
                Prec_ActCtrl.BackColor = Color.DeepPink
                Prec_ActCtrl.ForeColor = Color.White
            End If
        End If
    End Sub


    Private Sub Grid_Cell_DeSelect()
        On Error Resume Next
        dgv_Details.CurrentCell.Selected = False
        dgv_details_total.CurrentCell.Selected = False
        dgv_Filter_Details.CurrentCell.Selected = False

    End Sub
    Private Sub Covering_Production_Entry_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated

        If FrmLdSTS = True Then

            lbl_Company.Text = ""
            lbl_Company.Tag = 0
            Common_Procedures.CompIdNo = 0

            Me.Text = ""

            lbl_Company.Text = Common_Procedures.get_Company_From_CompanySelection(con)
            lbl_Company.Tag = Val(Common_Procedures.CompIdNo)

            Me.Text = lbl_Company.Text

            new_record()

        End If


        FrmLdSTS = False

    End Sub



    Private Sub Covering_Production_Entry_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Me.Text = ""

        con.Open()

        pnl_Filter.Visible = False
        pnl_Filter.Left = (Me.Width - pnl_Filter.Width) \ 2
        pnl_Filter.Top = (Me.Height - pnl_Filter.Height) \ 3
        pnl_Filter.BringToFront()



        AddHandler dtp_date.GotFocus, AddressOf ControlGotFocus
        AddHandler msk_date.GotFocus, AddressOf ControlGotFocus
        AddHandler Cbo_grid_EmpName.GotFocus, AddressOf ControlGotFocus

        AddHandler dtp_Filter_Fromdate.GotFocus, AddressOf ControlGotFocus
        AddHandler dtp_Filter_ToDate.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Filter_EmployeeName.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_save.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_close.GotFocus, AddressOf ControlGotFocus


        AddHandler msk_date.LostFocus, AddressOf ControlLostFocus
        AddHandler dtp_date.LostFocus, AddressOf ControlLostFocus
        AddHandler Cbo_grid_EmpName.LostFocus, AddressOf ControlLostFocus
        AddHandler dtp_Filter_Fromdate.LostFocus, AddressOf ControlLostFocus
        AddHandler dtp_Filter_ToDate.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Filter_EmployeeName.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_save.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_close.LostFocus, AddressOf ControlLostFocus


        lbl_Company.Text = ""
        lbl_Company.Tag = 0
        lbl_Company.Visible = False
        Common_Procedures.CompIdNo = 0

        Filter_Status = False
        FrmLdSTS = True
        new_record()
    End Sub
    Private Sub TextBoxControlKeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs)
        On Error Resume Next
        If e.KeyValue = 38 Then e.Handled = True : SendKeys.Send("+{TAB}")
        If e.KeyValue = 40 Then e.Handled = True : SendKeys.Send("{TAB}")
    End Sub

    Private Sub TextBoxControlKeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
        On Error Resume Next
        If Asc(e.KeyChar) = 13 Then e.Handled = True : SendKeys.Send("{TAB}")
    End Sub
    Private Sub Close_Form()

        Try

            lbl_Company.Tag = 0
            lbl_Company.Text = ""
            Me.Text = ""
            Common_Procedures.CompIdNo = 0

            lbl_Company.Text = Common_Procedures.Show_CompanySelection_On_FormClose(con)
            lbl_Company.Tag = Val(Common_Procedures.CompIdNo)
            Me.Text = lbl_Company.Text
            If Val(Common_Procedures.CompIdNo) = 0 Then

                Me.Close()

            Else

                new_record()

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT CLOSE...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub get_EmployeeList()
        Dim Cmd As New SqlClient.SqlCommand
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim n As Integer
        Dim SNo As Integer



        Cmd.Connection = con

        Cmd.Parameters.Clear()
        Cmd.Parameters.AddWithValue("@AttDate", dtp_date.Value.Date)

        Cmd.CommandText = "select a.*, b.* from PayRoll_Employee_Head a LEFT OUTER JOIN PayRoll_Category_Head b ON a.Category_IdNo = b.Category_IdNo where a.Join_DateTime <= @AttDate and (a.Date_Status = 0 or (a.Date_Status = 1 and a.Releave_DateTime >= @AttDate ) ) "
        da1 = New SqlClient.SqlDataAdapter(Cmd)
        dt1 = New DataTable
        da1.Fill(dt1)

        With dgv_details

            .Rows.Clear()
            SNo = 0

            If dt1.Rows.Count > 0 Then

                For i = 0 To dt1.Rows.Count - 1

                    n = .Rows.Add()

                    SNo = SNo + 1

                    .Rows(n).Cells(0).Value = Val(SNo)
                    .Rows(n).Cells(1).Value = dt1.Rows(i).Item("Employee_Name").ToString
                    .Rows(n).Cells(2).Value = "0"

                Next i

            End If

            Grid_Cell_DeSelect()

        End With
    End Sub
    Private Sub move_record(ByVal no As String)
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt2 As New DataTable
        Dim NewCode As String
        Dim n As Integer
        Dim SNo As Integer
        Dim LockSTS As Boolean = False


        If Val(no) = 0 Then Exit Sub


        clear()

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(no) & "/" & Trim(Common_Procedures.FnYearCode)

        Try

            da1 = New SqlClient.SqlDataAdapter("select a.* from Covering_Production_Entry_Head a Where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Covering_Production_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' ", con)
            dt1 = New DataTable
            da1.Fill(dt1)

            If dt1.Rows.Count > 0 Then

                lbl_Refno.Text = dt1.Rows(0).Item("Covering_Production_No").ToString
                dtp_date.Text = dt1.Rows(0).Item("Covering_Production_Date").ToString
                msk_date.Text = dtp_date.Text



                da2 = New SqlClient.SqlDataAdapter("Select a.*  from covering_production_details a Where a.Covering_Production_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' Order by a.sl_no", con)
                dt2 = New DataTable
                da2.Fill(dt2)



                With dgv_details

                    .Rows.Clear()
                    SNo = 0

                    If dt2.Rows.Count > 0 Then

                        For i = 0 To dt2.Rows.Count - 1

                            n = .Rows.Add()

                            SNo = SNo + 1

                            .Rows(n).Cells(0).Value = Val(SNo)
                            .Rows(n).Cells(1).Value = Common_Procedures.Employee_IdNoToName(con, Val(dt2.Rows(i).Item("Employee_idno").ToString))
                            ' .Rows(n).Cells(2).Value = Format(Val(dt2.Rows(i).Item("No_of_Spindles").ToString), "#########0.0")
                            ' If Val(dt2.Rows(i).Item("No_of_Spindles").ToString) <> 0 Then
                            .Rows(n).Cells(2).Value = (Val(dt2.Rows(i).Item("No_of_Spindles").ToString))
                          
                            ' End If

                        Next i

                    End If

                    If .RowCount = 0 Then .Rows.Add()

                End With

                With dgv_details_total
                    If .RowCount = 0 Then .Rows.Add()

                    .Rows(0).Cells(2).Value = Format(Val(dt1.Rows(0).Item("Total_Spindles").ToString), "########0.0")
                End With


                If LockSTS = True Then
                    Cbo_grid_EmpName.Enabled = False
                    Cbo_grid_EmpName.BackColor = Color.LightGray
                End If

                dgv_ActiveCtrl_Name = ""
                Grid_Cell_DeSelect()

            End If


        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT MOVE RECORDS...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally
            dt1.Dispose()
            da1.Dispose()

            dt2.Dispose()
            da2.Dispose()

            If msk_date.Visible And msk_date.Enabled Then msk_date.Focus()
        End Try

    End Sub

    Public Sub delete_record() Implements Interface_MDIActions.delete_record
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim cmd As New SqlClient.SqlCommand
        Dim trans As SqlClient.SqlTransaction
        Dim NewCode As String = ""


        If Val(lbl_Company.Tag) = 0 Then
            MessageBox.Show("Invalid Company Selection", "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
        End If

        If pnl_back.Enabled = False Then
            MessageBox.Show("Close Other Windows", "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
        End If

        If MessageBox.Show("Do you want to Delete?", "FOR DELETION...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) <> Windows.Forms.DialogResult.Yes Then
            Exit Sub
        End If

        If New_Entry = True Then
            MessageBox.Show("This is New Entry", "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
        End If


        trans = con.BeginTransaction

        Try

            NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_Refno.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            cmd.Connection = con
            cmd.Transaction = trans


            cmd.CommandText = "delete from covering_production_details where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Covering_Production_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()
            cmd.CommandText = "delete from Covering_Production_Entry_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Covering_Production_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()



            trans.Commit()

            new_record()

            MessageBox.Show("Deleted Sucessfully!!!", "FOR DELETION...", MessageBoxButtons.OKCancel, MessageBoxIcon.Information)

        Catch ex As Exception
            trans.Rollback()
            MessageBox.Show(ex.Message, "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Finally

            Dt1.Dispose()
            Da.Dispose()
            trans.Dispose()
            cmd.Dispose()

            If msk_date.Visible And msk_date.Enabled Then msk_date.Focus()

        End Try
    End Sub

    Public Sub filter_record() Implements Interface_MDIActions.filter_record

        If Filter_Status = False Then

            Dim da As New SqlClient.SqlDataAdapter
            Dim dt1 As New DataTable
            Dim dt2 As New DataTable


            dtp_Filter_Fromdate.Text = Common_Procedures.Company_FromDate
            dtp_Filter_ToDate.Text = Common_Procedures.Company_ToDate
            cbo_Filter_EmployeeName.Text = ""



            cbo_Filter_EmployeeName.SelectedIndex = -1
            dgv_Filter_Details.Rows.Clear()

        End If

        pnl_Filter.Visible = True
        pnl_Filter.Enabled = True
        pnl_back.Enabled = False
        If dtp_Filter_Fromdate.Enabled And dtp_Filter_Fromdate.Visible Then dtp_Filter_Fromdate.Focus()

    End Sub

    Public Sub insert_record() Implements Interface_MDIActions.insert_record
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim movno As String, inpno As String
        Dim InvCode As String

        Try

            inpno = InputBox("Enter New Ref No.", "FOR NEW Ref NO. INSERTION...")

            InvCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

            Da = New SqlClient.SqlDataAdapter("select Covering_Production_No from Covering_Production_Entry_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Covering_Production_Code like '" & Trim(Pk_Condition) & "%'  and Covering_Production_Code = '" & Trim(InvCode) & "'", con)
            Da.Fill(Dt)

            movno = ""
            If Dt.Rows.Count > 0 Then
                If IsDBNull(Dt.Rows(0)(0).ToString) = False Then
                    movno = Trim(Dt.Rows(0)(0).ToString)
                End If
            End If

            Dt.Clear()

            If Val(movno) <> 0 Then
                move_record(movno)

            Else
                If Val(inpno) = 0 Then
                    MessageBox.Show("Invalid Lot No.", "DOES NOT INSERT NEW Lot...", MessageBoxButtons.OK, MessageBoxIcon.Error)

                Else
                    new_record()
                    Insert_Entry = True
                    lbl_Refno.Text = Trim(UCase(inpno))

                End If

            End If


        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT INSERT NEW BILL...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally
            Dt.Dispose()
            Da.Dispose()
        End Try
    End Sub

    Public Sub movefirst_record() Implements Interface_MDIActions.movefirst_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim movno As String

        Try

            da = New SqlClient.SqlDataAdapter("select top 1 Covering_Production_No from Covering_Production_Entry_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Covering_Production_Code like '" & Trim(Pk_Condition) & "%'  and Covering_Production_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby, Covering_Production_No", con)
            da.Fill(dt)

            movno = ""
            If dt.Rows.Count > 0 Then
                If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                    movno = Trim(dt.Rows(0)(0).ToString)
                End If
            End If

            dt.Clear()

            If Val(movno) <> 0 Then move_record(movno)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR  MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally
            dt.Dispose()
            da.Dispose()

        End Try

    End Sub

    Public Sub movelast_record() Implements Interface_MDIActions.movelast_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim movno As String = ""

        Try
            da = New SqlClient.SqlDataAdapter("select top 1 Covering_Production_No from Covering_Production_Entry_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Covering_Production_Code like '" & Trim(Pk_Condition) & "%'  and Covering_Production_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, Covering_Production_No desc", con)
            da.Fill(dt)

            movno = ""
            If dt.Rows.Count > 0 Then
                If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                    movno = dt.Rows(0)(0).ToString
                End If
            End If

            If Val(movno) <> 0 Then move_record(movno)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR  MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally
            dt.Dispose()
            da.Dispose()

        End Try
    End Sub

    Public Sub movenext_record() Implements Interface_MDIActions.movenext_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim movno As String = ""
        Dim OrdByNo As Single = 0

        Try

            OrdByNo = Common_Procedures.OrderBy_CodeToValue(Trim(lbl_Refno.Text))

            da = New SqlClient.SqlDataAdapter("select top 1 Covering_Production_No from Covering_Production_Entry_Head where for_orderby > " & Str(Val(OrdByNo)) & " and Covering_Production_Code like '" & Trim(Pk_Condition) & "%'  and company_idno = " & Str(Val(lbl_Company.Tag)) & " and Covering_Production_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby, Covering_Production_No", con)
            da.Fill(dt)

            movno = ""
            If dt.Rows.Count > 0 Then
                If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                    movno = dt.Rows(0)(0).ToString
                End If
            End If

            If Val(movno) <> 0 Then move_record(movno)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR  MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally
            dt.Dispose()
            da.Dispose()

        End Try
    End Sub

    Public Sub moveprevious_record() Implements Interface_MDIActions.moveprevious_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim movno As String = ""
        Dim OrdByNo As Single = 0

        Try

            OrdByNo = Common_Procedures.OrderBy_CodeToValue(Trim(lbl_Refno.Text))

            da = New SqlClient.SqlDataAdapter("select top 1 Covering_Production_No from Covering_Production_Entry_Head where for_orderby < " & Str(Val(OrdByNo)) & " and Covering_Production_Code like '" & Trim(Pk_Condition) & "%'  and company_idno = " & Str(Val(lbl_Company.Tag)) & " and Covering_Production_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, Covering_Production_No desc", con)
            da.Fill(dt)

            movno = ""
            If dt.Rows.Count > 0 Then
                If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                    movno = dt.Rows(0)(0).ToString
                End If
            End If

            If Val(movno) <> 0 Then move_record(movno)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR  MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally
            dt.Dispose()
            da.Dispose()

        End Try

    End Sub

    Public Sub new_record() Implements Interface_MDIActions.new_record
        Dim Da1 As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim Dt2 As New DataTable

        Try
            clear()

            New_Entry = True


            lbl_Refno.Text = Common_Procedures.get_MaxCode(con, "Covering_Production_Entry_Head", "Covering_Production_Code", "For_OrderBy", " Covering_Production_Code like '" & Trim(Pk_Condition) & "%' ", Val(lbl_Company.Tag), Common_Procedures.FnYearCode)
            lbl_Refno.ForeColor = Color.Red
            msk_date.Text = Date.Today.ToShortDateString

            get_EmployeeList()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR NEW RECORD...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally

            Dt1.Dispose()
            Dt2.Dispose()
            Da1.Dispose()

            If msk_date.Enabled And msk_date.Visible Then
                msk_date.Focus()
                msk_date.SelectionStart = 0
            End If

        End Try

    End Sub

    Public Sub open_record() Implements Interface_MDIActions.open_record
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim movno As String, inpno As String
        Dim RefCode As String

        Try

            inpno = InputBox("Enter Ref No.", "FOR FINDING...")

            RefCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

            Da = New SqlClient.SqlDataAdapter("select Covering_Production_No from Covering_Production_Entry_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Covering_Production_Code like '" & Trim(Pk_Condition) & "%' and  Covering_Production_Code = '" & Trim(RefCode) & "'", con)
            Da.Fill(Dt)

            movno = ""
            If Dt.Rows.Count > 0 Then
                If IsDBNull(Dt.Rows(0)(0).ToString) = False Then
                    movno = Trim(Dt.Rows(0)(0).ToString)
                End If
            End If

            Dt.Clear()

            If Val(movno) <> 0 Then
                move_record(movno)

            Else
                MessageBox.Show("Ref No. does not exists", "DOES NOT FIND...", MessageBoxButtons.OK, MessageBoxIcon.Error)

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT FIND...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally
            Dt.Dispose()
            Da.Dispose()

        End Try
    End Sub

    Public Sub print_record() Implements Interface_MDIActions.print_record

    End Sub
    Private Sub Open_FilterEntry()
        Dim movno As String

        On Error Resume Next

        movno = Trim(dgv_Filter_Details.CurrentRow.Cells(0).Value)

        If Val(movno) <> 0 Then
            Filter_Status = True
            move_record(movno)
            pnl_Back.Enabled = True
            pnl_Filter.Visible = False
        End If

    End Sub
    Private Sub dgv_Filter_Details_CellDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Filter_Details.CellDoubleClick
        Open_FilterEntry()
    End Sub

    Private Sub dgv_Filter_Details_CellValueChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Filter_Details.CellValueChanged
        Try

            With dgv_details
                If .Visible Then
                    If .Rows.Count > 0 Then
                        If .CurrentCell.ColumnIndex = 2 Or .CurrentCell.ColumnIndex = 5 Or .CurrentCell.ColumnIndex = 5 Then
                            Total_Calculation()
                        End If
                    End If
                End If
            End With

        Catch ex As Exception
            '---
        End Try
    End Sub


    Private Sub dgv_Filter_Details_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_Filter_Details.KeyDown
        If e.KeyCode = 13 Then
            Open_FilterEntry()
        End If
    End Sub
    Public Sub save_record() Implements Interface_MDIActions.save_record
        Dim cmd As New SqlClient.SqlCommand
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim tr As SqlClient.SqlTransaction
        Dim NewCode As String = ""
        Dim Employee_Id As Integer
        Dim vTotspin As Single
        Dim Sno As Integer = 0
        Dim spin As Integer = 0

        If Val(lbl_Company.Tag) = 0 Then
            MessageBox.Show("Invalid Company Selection", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        If pnl_back.Enabled = False Then
            MessageBox.Show("Close Other Windows", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        If IsDate(msk_date.Text) = False Then
            MessageBox.Show("Invalid Date", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If msk_date.Enabled Then msk_date.Focus()
            Exit Sub
        End If


        If Not (Convert.ToDateTime(msk_date.Text) >= Common_Procedures.Company_FromDate And Convert.ToDateTime(msk_date.Text) <= Common_Procedures.Company_ToDate) Then
            MessageBox.Show("Date is out of financial range", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If msk_date.Enabled Then msk_date.Focus()
            Exit Sub
        End If


        'With dgv_details

        '    For i = 0 To .RowCount - 1

        '        If Val(.Rows(i).Cells(2).Value) <> 0 Then

        '            Employee_Id = Common_Procedures.Employee_NameToIdNo(con, dgv_details.Rows(i).Cells(1).Value)

        '            'If Val(Employee_Id) = 0 Then
        '            '    MessageBox.Show("Invalid Employee Name", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
        '            '    If .Enabled And .Visible Then
        '            '        .Focus()
        '            '        .CurrentCell = .Rows(i).Cells(1)
        '            '    End If
        '            '    Exit Sub
        '            'End If

        '            'spin = Val(.Rows(i).Cells(2).Value)

        '            'If Val(.Rows(i).Cells(2).Value) <> 0 Then
        '            '    MessageBox.Show("Invalid Spindles", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
        '            '    If .Enabled And .Visible Then
        '            '        .Focus()
        '            '        .CurrentCell = .Rows(i).Cells(2)
        '            '    End If
        '            '    Exit Sub
        '            'End If

        '        End If

        '    Next

        'End With
        vTotspin = 0


        If dgv_details_total.RowCount > 0 Then
            vTotspin = Val(dgv_details_total.Rows(0).Cells(2).Value())
        
        End If


        tr = con.BeginTransaction

        Try

            If Insert_Entry = True Or New_Entry = False Then
                NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_Refno.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            Else

                lbl_Refno.Text = Common_Procedures.get_MaxCode(con, "covering_production_entry_head", "Covering_Production_Code", "For_OrderBy", "Covering_Production_Code like '" & Trim(Pk_Condition) & "%' ", Val(lbl_Company.Tag), Common_Procedures.FnYearCode, tr)

                NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_Refno.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            End If


            cmd.Connection = con
            cmd.Transaction = tr

            cmd.Parameters.Clear()
            cmd.Parameters.AddWithValue("@refDate", dtp_date.Value.Date)

            If New_Entry = True Then
                cmd.CommandText = "Insert into Covering_Production_Entry_Head (       Covering_Production_Code  ,               Company_IdNo       ,           Covering_Production_No  ,                               for_OrderBy                          , Covering_Production_Date    ,       Total_spindles             ) " & _
                                    "     Values                  (   '" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_Refno.Text) & "'    , " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_Refno.Text))) & ",           @RefDate             ,  " & Str(Val(vTotspin)) & "      ) "
                cmd.ExecuteNonQuery()

            Else
                cmd.CommandText = "Update Covering_Production_Entry_Head set Covering_Production_Date = @refDate,  Total_spindles = " & Str(Val(vTotspin)) & " Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Covering_Production_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()


            End If

            cmd.CommandText = "Delete from covering_production_details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Covering_Production_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()


            With dgv_details
                Sno = 0
                For i = 0 To .RowCount - 1

                    If Val(.Rows(i).Cells(2).Value) >= 0 Then

                        Sno = Sno + 1
                        Employee_Id = Common_Procedures.Employee_NameToIdNo(con, .Rows(i).Cells(1).Value, tr)



                        cmd.CommandText = "Insert into covering_production_details ( Covering_Production_Code          ,   Company_IdNo                   ,   Covering_Production_No  ,                                             for_OrderBy  ,                                                                 Covering_Production_Date,           Sl_No       ,                        Employee_IdNo         ,                 No_of_spindles    ) " & _
                                          "     Values                 (   '" & Trim(Pk_Condition) & Trim(NewCode) & "'      , " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_Refno.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_Refno.Text))) & ",       @refDate                    ,  " & Str(Val(Sno)) & ", " & Str(Val(Employee_Id)) & ",  " & Str(Val(.Rows(i).Cells(2).Value)) & "  ) "
                        cmd.ExecuteNonQuery()
                    End If
                Next

            End With

            tr.Commit()


            MessageBox.Show("Saved Sucessfully!!!", "FOR SAVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Information)

            If Val(Common_Procedures.settings.OnSave_MoveTo_NewEntry_Status) = 1 Then
                If New_Entry = True Then
                    new_record()
                Else
                    move_record(lbl_Refno.Text)
                End If
            Else
                move_record(lbl_Refno.Text)
            End If


        Catch ex As Exception
            If InStr(1, ex.Message, "IX_covering_production_entry_head") > 0 Then
                MessageBox.Show("Dupliacate Date", "FOR SAVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            ElseIf InStr(1, ex.Message, "IX_covering_production_details") > 0 Then
                MessageBox.Show("Dupliacate Date", "FOR SAVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Else
                MessageBox.Show(ex.Message, "FOR SAVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If

        Finally
            Dt1.Dispose()
            Da.Dispose()
            cmd.Dispose()
            tr.Dispose()
            Dt1.Clear()

            If msk_date.Visible And msk_date.Enabled Then msk_date.Focus()
        End Try

    End Sub

    Private Sub Covering_Production_Entry_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        con.Close()
        con.Dispose()
    End Sub


    'Private Sub Cbo_grid_EmpName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles Cbo_grid_EmpName.GotFocus
    '    Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "PayRoll_Employee_Head", "Employee_Name", "", "(Employee_IdNo = 0)")
    'End Sub

    'Private Sub Cbo_grid_EmpName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Cbo_grid_EmpName.KeyDown
    '    vcbo_KeyDwnVal = e.KeyValue
    '    Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, Cbo_grid_EmpName, Nothing, Nothing, "PayRoll_Employee_Head", "Employee_Name", "", "(Employee_IdNo = 0)")
    '    With dgv_details
    '        If (e.KeyValue = 38 And Cbo_grid_EmpName.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then

    '            If Val(.CurrentCell.RowIndex) <= 0 Then
    '                msk_date.Focus()

    '            Else
    '                .Focus()
    '                .CurrentCell = .Rows(.CurrentCell.RowIndex - 1).Cells(.ColumnCount - 1)
    '                .CurrentCell.Selected = True

    '            End If 
    '        End If

    '        If (e.KeyValue = 40 And Cbo_grid_EmpName.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then

    '            If .CurrentCell.RowIndex = .Rows.Count - 1 And Trim(.Rows(.CurrentCell.RowIndex).Cells.Item(1).Value) = "" Then

    '                If MessageBox.Show("Do you want to save", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = DialogResult.Yes Then
    '                    save_record()
    '                End If

    '            Else
    '                .Focus()
    '                .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)

    '            End If

    '        End If
    '    End With
    'End Sub

    'Private Sub Cbo_grid_EmpName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Cbo_grid_EmpName.KeyPress
    '    Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, Cbo_grid_EmpName, Nothing, "PayRoll_Employee_Head", "Employee_Name", "", "(Employee_IdNo = 0)")
    '    If Asc(e.KeyChar) = 13 Then
    '        With dgv_details

    '            .Rows(.CurrentCell.RowIndex).Cells.Item(1).Value = Trim(Cbo_grid_EmpName.Text)
    '            If .CurrentCell.RowIndex = .RowCount - 1 And .CurrentCell.ColumnIndex >= 1 And Trim(.CurrentRow.Cells(1).Value) = "" Then
    '                If MessageBox.Show("Do you want to save", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = DialogResult.Yes Then
    '                    save_record()
    '                End If
    '            Else
    '                .Focus()
    '                .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)
    '            End If
    '        End With
    '    End If
    'End Sub

    'Private Sub Cbo_grid_EmpName_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Cbo_grid_EmpName.KeyUp
    '    If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then


    '        Dim f As New EmployeeCreation_Simple

    '        Common_Procedures.Master_Return.Form_Name = Me.Name
    '        Common_Procedures.Master_Return.Control_Name = Cbo_grid_EmpName.Name
    '        Common_Procedures.Master_Return.Return_Value = ""
    '        Common_Procedures.Master_Return.Master_Type = ""

    '        f.MdiParent = MDIParent1
    '        f.Show()
    '    End If
    'End Sub

    'Private Sub Cbo_grid_EmpName_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles Cbo_grid_EmpName.TextChanged
    '    Try
    '        If Cbo_grid_EmpName.Visible Then
    '            With dgv_details
    '                If .Rows.Count > 0 Then
    '                    If Val(Cbo_grid_EmpName.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = 1 Then
    '                        .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(Cbo_grid_EmpName.Text)
    '                    End If
    '                End If
    '            End With
    '        End If

    '    Catch ex As Exception
    '        'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

    '    End Try
    'End Sub

    Protected Overrides Function ProcessCmdKey(ByRef msg As System.Windows.Forms.Message, ByVal keyData As System.Windows.Forms.Keys) As Boolean
        Dim dgv1 As New DataGridView

        'On Error Resume Next


        If ActiveControl.Name = dgv_details.Name Or TypeOf ActiveControl Is DataGridViewTextBoxEditingControl Then

            dgv1 = Nothing

            If ActiveControl.Name = dgv_details.Name Then
                dgv1 = dgv_details

            ElseIf dgv_details.IsCurrentRowDirty = True Then
                dgv1 = dgv_details

            Else
                dgv1 = dgv_details

            End If

            With dgv1
                If keyData = Keys.Enter Or keyData = Keys.Down Then

                    If .CurrentCell.ColumnIndex >= .ColumnCount - 1 Then
                        If .CurrentCell.RowIndex = .RowCount - 1 Then
                            If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                                save_record()
                            Else
                                msk_date.Focus()
                            End If

                        Else

                            .CurrentCell = .Rows(.CurrentCell.RowIndex + 1).Cells(2)

                        End If

                    Else

                        If .CurrentCell.RowIndex = .RowCount - 1 And .CurrentCell.ColumnIndex >= 1 And Trim(.CurrentRow.Cells(1).Value) = "" Then
                            If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                                save_record()
                            Else
                                dtp_date.Focus()
                            End If

                        ElseIf .CurrentCell.ColumnIndex = 3 Then
                            .CurrentCell = .Rows(.CurrentRow.Index).Cells(5)

                        Else
                            .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 1)

                        End If

                    End If

                    Return True

                ElseIf keyData = Keys.Up Then

                    If .CurrentCell.ColumnIndex <= 2 Then
                        If .CurrentCell.RowIndex = 0 Then
                            msk_date.Focus()

                        Else
                            .CurrentCell = .Rows(.CurrentCell.RowIndex - 1).Cells(2)

                        End If

                    ElseIf .CurrentCell.ColumnIndex = 5 Then
                        .CurrentCell = .Rows(.CurrentRow.Index).Cells(3)

                    Else
                        .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex - 1)

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

    Private Sub dgv_details_CellEndEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_details.CellEndEdit
        dgv_details_CellLeave(sender, e)
    End Sub

    Private Sub dgv_details_CellEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_details.CellEnter
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim Dt2 As New DataTable
        '  Dim Rect As Rectangle

        With dgv_details
            dgv_ActiveCtrl_Name = .Name

            If Val(.CurrentRow.Cells(0).Value) = 0 Then
                .CurrentRow.Cells(0).Value = .CurrentRow.Index + 1
            End If


        End With
    End Sub

    Private Sub dgv_details_CellLeave(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_details.CellLeave
        With dgv_details


            If .CurrentCell.ColumnIndex = 2 Then
                If Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value) <> 0 Then
                    .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = (Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value))
                Else
                    .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = "0"
                End If
            End If
        End With
    End Sub

    Private Sub dgv_details_CellValueChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_details.CellValueChanged
        On Error Resume Next
        With dgv_details
            If .Visible Then
                If e.ColumnIndex = 2 Then
                    'If Val(.Rows(e.RowIndex).Cells(5).Value) <> 0 Then
                    '    .Rows(e.RowIndex).Cells(7).Value = Format(Val(.Rows(e.RowIndex).Cells(5).Value) - Format(Val(.Rows(e.RowIndex).Cells(6).Value), "##########0.00"))
                    'End If
                    Total_Calculation()
                End If
            End If
        End With
    End Sub

    Private Sub Total_Calculation()

        Dim Totspin As Single
        Dim Sno As Integer
        Dim Rate As Double
        Sno = 0 : Totspin = 0 : Rate = 0


        With dgv_details
            For i = 0 To .RowCount - 1
                Sno = Sno + 1
                .Rows(i).Cells(0).Value = Sno
                If Trim(.Rows(i).Cells(1).Value) <> "" And Val(.Rows(i).Cells(2).Value) <> 0 Then
                    Totspin = Totspin + Format(Val(.Rows(i).Cells(2).Value), "########0.00")
                    ' Rate = Totspin * Format(Val(.Rows(i).Cells(3).Value), "########0.00")
                End If

            Next

        End With

        With dgv_details_total
            If .RowCount = 0 Then .Rows.Add()
            .Rows(0).Cells(2).Value = Format(Val(Totspin), "########0.00")
        End With

    End Sub

    Private Sub dgv_details_EditingControlShowing(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewEditingControlShowingEventArgs) Handles dgv_details.EditingControlShowing
        dgtxt_Details = CType(dgv_details.EditingControl, DataGridViewTextBoxEditingControl)
    End Sub

    Private Sub dgv_details_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_details.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Try
            With dgv_details
                vcbo_KeyDwnVal = e.KeyValue
                If e.KeyValue = Keys.Delete Then
                    If Val(dgv_details.Rows(dgv_details.CurrentCell.RowIndex).Cells(9).Value) <> 0 Then
                        e.Handled = True
                    End If
                End If
            End With

        Catch ex As Exception
            '----

        End Try

    End Sub

    Private Sub dgv_details_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_details.KeyUp
        Dim i As Integer
        Dim n As Integer

        If e.Control = True And UCase(Chr(e.KeyCode)) = "D" Then

            With dgv_details

                n = .CurrentRow.Index

                If .Rows.Count = 1 Then
                    For i = 0 To .Columns.Count - 1
                        .Rows(n).Cells(i).Value = ""
                    Next

                Else

                    .Rows.RemoveAt(n)

                End If

                For i = 0 To .Rows.Count - 1
                    .Rows(i).Cells(0).Value = i + 1
                Next

            End With
        End If
    End Sub


    Private Sub dgv_details_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgv_details.LostFocus
        On Error Resume Next
        dgv_details.CurrentCell.Selected = False
    End Sub


    Private Sub dgv_details_RowsAdded(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewRowsAddedEventArgs) Handles dgv_details.RowsAdded
        Dim n As Integer = 0

        With dgv_details

            n = .RowCount
            .Rows(n - 1).Cells(0).Value = Val(n)
            If Val(.Rows(e.RowIndex).Cells(2).Value) = 0 Then

            End If
        End With
    End Sub

    Private Sub dgtxt_Details_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgtxt_Details.Enter
        dgv_ActiveCtrl_Name = dgv_details.Name
        dgv_details.EditingControl.BackColor = Color.Lime
        dgv_details.EditingControl.ForeColor = Color.Blue
        dgtxt_Details.SelectAll()
    End Sub

    Private Sub dgtxt_Details_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgtxt_Details.KeyDown
        Try
            vcbo_KeyDwnVal = e.KeyValue

        Catch ex As Exception
            MessageBox.Show(ex.Message, "ERROR WHILE DETAILS TEXT KEYDOWN....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub dgtxt_Details_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dgtxt_Details.KeyPress
        Try
            With dgv_details
                If .Visible Then
                    'If Val(dgv_Details.Rows(dgv_Details.CurrentCell.RowIndex).Cells(9).Value) <> 0 Then
                    '    e.Handled = True
                    'End If
                    If .CurrentCell.ColumnIndex = 2 Then
                        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
                            e.Handled = True
                        End If
                    End If

                End If

            End With


        Catch ex As Exception
            '----

        End Try

    End Sub

    Private Sub dgtxt_Details_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgtxt_Details.KeyUp
        Try
            With dgv_details
                If .Rows.Count > 0 Then
                    If e.Control = True And UCase(Chr(e.KeyCode)) = "D" Then
                        dgv_details_KeyUp(sender, e)
                    End If

                    If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
                        dgv_details_KeyUp(sender, e)
                    End If
                End If
            End With

        Catch ex As Exception
            '----

        End Try
    End Sub

    Private Sub dgtxt_Details_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgtxt_Details.TextChanged
        Try
            With dgv_details
                If .Rows.Count <> 0 Then
                    .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(dgtxt_Details.Text)
                End If
            End With

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub cbo_Filter_EmployeeName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Filter_EmployeeName.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "PayRoll_Employee_Head", "Employee_Name", "", "(Employee_IdNo = 0)")
    End Sub

    Private Sub cbo_Filter_EmployeeName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Filter_EmployeeName.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_EmployeeName, dtp_Filter_ToDate, dgv_Filter_Details, "PayRoll_Employee_Head", "Employee_Name", "", "(Employee_IdNo = 0)")
    End Sub

    Private Sub cbo_Filter_EmployeeName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Filter_EmployeeName.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Filter_EmployeeName, dgv_Filter_Details, "PayRoll_Employee_Head", "Employee_Name", "", "(Employee_IdNo = 0)")
    End Sub
    Private Sub btn_Filter_Show_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Filter_Show.Click

        Dim da As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim n As Integer
        Dim Employee_id As Integer
        Dim Condt As String = ""

        Try

            Condt = ""
            Employee_id = 0


            If IsDate(dtp_Filter_Fromdate.Value) = True And IsDate(dtp_Filter_ToDate.Value) = True Then
                Condt = "a.Covering_Production_Date between '" & Trim(Format(dtp_Filter_Fromdate.Value, "MM/dd/yyyy")) & "' and '" & Trim(Format(dtp_Filter_ToDate.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_Filter_Fromdate.Value) = True Then
                Condt = "a.Covering_Production_Date = '" & Trim(Format(dtp_Filter_Fromdate.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_Filter_ToDate.Value) = True Then
                Condt = "a.Covering_Production_Date = '" & Trim(Format(dtp_Filter_ToDate.Value, "MM/dd/yyyy")) & "' "
            End If

            If Trim(cbo_Filter_EmployeeName.Text) <> "" Then
                Employee_id = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Filter_EmployeeName.Text)
            End If


            If Val(Employee_id) <> 0 Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " a.Employee_IdNo = " & Str(Val(Employee_id)) & " "
            End If

            da = New SqlClient.SqlDataAdapter("select a.*,b.*,c.Employee_Name from Covering_Production_Entry_Head a INNER JOIN covering_production_details b ON a.Covering_Production_Code = b.Covering_Production_Code inner JOIN PayRoll_Employee_Head c ON b.Employee_IdNo = c.Employee_IdNo    where a.company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Covering_Production_Code like '" & Trim(Pk_Condition) & "%'  and a.Covering_Production_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "' " & IIf(Trim(Condt) <> "", " and ", "") & Condt & " Order by a.for_orderby, a.Covering_Production_No", con)
            dt2 = New DataTable
            da.Fill(dt2)

            dgv_Filter_Details.Rows.Clear()

            If dt2.Rows.Count > 0 Then

                For i = 0 To dt2.Rows.Count - 1

                    n = dgv_Filter_Details.Rows.Add()
                    dgv_Filter_Details.Rows(n).Cells(0).Value = dt2.Rows(i).Item("Covering_Production_No").ToString
                    dgv_Filter_Details.Rows(n).Cells(1).Value = Format(Convert.ToDateTime(dt2.Rows(i).Item("Covering_Production_Date").ToString), "dd-MM-yyyy")
                    dgv_Filter_Details.Rows(n).Cells(2).Value = dt2.Rows(i).Item("Employee_name").ToString
                    dgv_Filter_Details.Rows(n).Cells(3).Value = Format(Val(dt2.Rows(i).Item("No_of_spindles").ToString), "########0.00")

                Next i

            End If

            dt2.Clear()
        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT FILTER...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally
            dt2.Dispose()
            da.Dispose()

            If dgv_Filter_Details.Visible And dgv_Filter_Details.Enabled Then dgv_Filter_Details.Focus()

        End Try

    End Sub

    Private Sub btn_Filter_Close_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Filter_Close.Click
        pnl_Back.Enabled = True
        pnl_Filter.Visible = False
        Filter_Status = False
    End Sub

    Private Sub msk_date_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles msk_date.GotFocus
        msk_date.Tag = msk_date.Text
    End Sub
    Private Sub msk_Date_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles msk_date.KeyDown


        Try
            If e.KeyValue = 40 Then
                If dgv_details.Rows.Count > 0 Then
                    dgv_details.Focus()
                    dgv_details.CurrentCell = dgv_details.Rows(0).Cells(2)
                    dgv_details.CurrentCell.Selected = True
                Else
                    If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                        save_record()
                    Else
                        dtp_date.Focus()
                    End If

                End If

            End If

        Catch ex As Exception
            '----

        End Try
    End Sub

    Private Sub msk_Date_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles msk_date.KeyPress
        Dim Cmd As New SqlClient.SqlCommand
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim movno As String = ""
        Dim NewCode As String = ""

        Try
            If Asc(e.KeyChar) = 13 Then
                Cmd.Connection = con

                Cmd.Parameters.Clear()
                Cmd.Parameters.AddWithValue("@EntryDate", dtp_date.Value.Date)

                NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_Refno.Text) & "/" & Trim(Common_Procedures.FnYearCode)

                If Trim(UCase(msk_date.Tag)) <> Trim(UCase(msk_date.Text)) Then


                    Cmd.CommandText = "select Covering_Production_No from covering_production_entry_head Where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Covering_Production_date = @EntryDate"
                    Da = New SqlClient.SqlDataAdapter(Cmd)
                    Dt = New DataTable
                    Da.Fill(Dt)

                    movno = ""
                    If Dt.Rows.Count > 0 Then
                        If IsDBNull(Dt.Rows(0)(0).ToString) = False Then
                            movno = Trim(Dt.Rows(0)(0).ToString)
                        End If
                    End If
                    Dt.Clear()

                    If Val(movno) <> 0 Then
                        move_record(movno)
                    Else
                        get_EmployeeList()
                    End If

                Else
                    Cmd.CommandText = "select Covering_Production_No from covering_production_entry_head Where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Covering_Production_date = @EntryDate and Covering_Production_Code <> '" & Trim(NewCode) & "'"
                    Da = New SqlClient.SqlDataAdapter(Cmd)
                    Dt = New DataTable
                    Da.Fill(Dt)

                    movno = ""
                    If Dt.Rows.Count > 0 Then
                        If IsDBNull(Dt.Rows(0)(0).ToString) = False Then
                            movno = Trim(Dt.Rows(0)(0).ToString)
                        End If
                    End If
                    Dt.Clear()

                    If Val(movno) <> 0 Then
                        move_record(movno)
                    Else
                        get_EmployeeList()
                    End If

                End If


                If dgv_details.Rows.Count > 0 Then
                    dgv_details.Focus()
                    dgv_details.CurrentCell = dgv_details.Rows(0).Cells(2)
                    dgv_details.CurrentCell.Selected = True

                Else

                    If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                        save_record()
                    Else
                        dtp_date.Focus()
                    End If

                End If
            End If
        Catch ex As Exception
            '-------
        End Try
    End Sub


    Private Sub msk_Date_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles msk_date.KeyUp
        Dim vmRetTxt As String = ""
        Dim vmRetSelStrt As Integer = -1

        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            msk_Date.Text = Date.Today
        End If
        If e.KeyCode = 107 Then
            msk_Date.Text = DateAdd("D", 1, Convert.ToDateTime(msk_Date.Text))
        ElseIf e.KeyCode = 109 Then
            msk_Date.Text = DateAdd("D", -1, Convert.ToDateTime(msk_Date.Text))
        End If

        If e.KeyCode = 46 Or e.KeyCode = 8 Then

            Common_Procedures.maskEdit_Date_ON_DelBackSpace(sender, e, vmskOldText, vmskSelStrt)

        End If

    End Sub

    Private Sub dtp_Date_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dtp_date.TextChanged

        If IsDate(dtp_Date.Text) = True Then

            msk_Date.Text = dtp_Date.Text
            msk_Date.SelectionStart = 0
        End If
    End Sub

    Private Sub msk_Date_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles msk_date.LostFocus

        If IsDate(msk_Date.Text) = True Then
            If Microsoft.VisualBasic.DateAndTime.Day(Convert.ToDateTime(msk_Date.Text)) <= 31 Or Microsoft.VisualBasic.DateAndTime.Month(Convert.ToDateTime(msk_Date.Text)) <= 31 Then
                If Microsoft.VisualBasic.DateAndTime.Year(Convert.ToDateTime(msk_Date.Text)) <= 2050 And Microsoft.VisualBasic.DateAndTime.Year(Convert.ToDateTime(msk_Date.Text)) >= 2000 Then
                    dtp_Date.Value = Convert.ToDateTime(msk_Date.Text)
                End If
            End If

        End If
    End Sub

    Private Sub dtp_Date_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dtp_date.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            msk_date.Text = Date.Today
        End If
    End Sub



    Private Sub btn_close_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_close.Click
        Me.Close()
    End Sub

    Private Sub btn_save_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_save.Click
        save_record()
    End Sub

    Private Sub Covering_Production_Entry_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        Try
            If Asc(e.KeyChar) = 27 Then
                Close_Form()
            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT CLOSE...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub


    Private Sub dtp_date_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles dtp_date.ValueChanged

    End Sub

    Private Sub msk_date_MaskInputRejected(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MaskInputRejectedEventArgs) Handles msk_date.MaskInputRejected

    End Sub
End Class