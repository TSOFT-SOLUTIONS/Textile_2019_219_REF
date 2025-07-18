Public Class MarkSheet_Entry
    Implements Interface_MDIActions

    Private con As New SqlClient.SqlConnection(Common_Procedures.Connection_String)
    Private FrmLdSTS As Boolean = False
    Private New_Entry As Boolean = False
    Private Insert_Entry As Boolean = False
    Private Filter_Status As Boolean = False
    Private Pk_Condition As String = "SMARK-"
    Private Prec_ActCtrl As New Control

    Private Sub clear()
        pnl_Back.Enabled = True
        pnl_Filter.Visible = False

        New_Entry = False

        lbl_Code.Text = ""
        lbl_Code.ForeColor = Color.Black
        cbo_Class.Text = ""
        cbo_Student.Text = ""
        cbo_Term.Text = ""

        dgv_Details.Rows.Clear()

        dgv_Details_Total.Rows.Clear()
        dgv_Details_Total.Rows.Add()

        cbo_Grid_Subject.Visible = False

    End Sub

    Private Sub ControlGotFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim txtbx As TextBox
        Dim combobx As ComboBox

        On Error Resume Next

        Me.ActiveControl.BackColor = Color.PaleGreen
        Me.ActiveControl.ForeColor = Color.Blue

        If TypeOf Me.ActiveControl Is TextBox Then
            txtbx = Me.ActiveControl
            txtbx.SelectAll()
        ElseIf TypeOf Me.ActiveControl Is ComboBox Then
            combobx = Me.ActiveControl
            combobx.SelectAll()
        End If

        If Me.ActiveControl.Name <> cbo_Grid_Subject.Name Then
            cbo_Grid_Subject.Visible = False
        End If
        If Me.ActiveControl.Name <> dgv_Details.Name Then
            Grid_DeSelect()
        End If

        Prec_ActCtrl = Me.ActiveControl

    End Sub

    Private Sub ControlLostFocus(ByVal sender As Object, ByVal e As System.EventArgs)

        On Error Resume Next

        If IsNothing(Prec_ActCtrl) = False Then
            Prec_ActCtrl.BackColor = Color.White
            Prec_ActCtrl.ForeColor = Color.Black
        End If

    End Sub

    Private Sub Grid_DeSelect()
        On Error Resume Next
        dgv_Details.CurrentCell.Selected = False
        dgv_Details_Total.CurrentCell.Selected = False
    End Sub



    Private Sub move_record(ByVal no As String)
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim NewCode As String
        Dim n As Integer
        Dim SNo As Integer

        If Val(no) = 0 Then Exit Sub

        clear()

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(no) & "/" & Trim(Common_Procedures.FnYearCode)

        Try

            da1 = New SqlClient.SqlDataAdapter("select a.*, b.class_Name, c.student_Name from Mark_Head a INNER JOIN Class_Head b ON a.Class_IdNo = b.class_IdNo INNER JOIN Student_Head c ON a.Student_IdNo = c.Student_IdNo Where a.Mark_Code = '" & Trim(NewCode) & "'", con)
            da1.Fill(dt1)

            If dt1.Rows.Count > 0 Then
                lbl_Code.Text = dt1.Rows(0).Item("Mark_No").ToString

                cbo_Class.Text = dt1.Rows(0).Item("Class_Name").ToString
                cbo_Student.Text = dt1.Rows(0).Item("student_name").ToString
                cbo_Term.Text = dt1.Rows(0).Item("Term_Name").ToString

                da2 = New SqlClient.SqlDataAdapter("select a.*, b.subject_name from Mark_Details a INNER JOIN Subject_Head b ON a.subject_idno = b.subject_idno where a.Mark_Code = '" & Trim(NewCode) & "' Order by a.Sl_No", con)
                dt2 = New DataTable
                da2.Fill(dt2)

                dgv_Details.Rows.Clear()
                SNo = 0

                If dt2.Rows.Count > 0 Then

                    For i = 0 To dt2.Rows.Count - 1

                        n = dgv_Details.Rows.Add()

                        SNo = SNo + 1
                        dgv_Details.Rows(n).Cells(0).Value = Val(SNo)
                        dgv_Details.Rows(n).Cells(1).Value = dt2.Rows(i).Item("subject_name").ToString
                        dgv_Details.Rows(n).Cells(2).Value = Format(Val(dt2.Rows(i).Item("marks").ToString), "########0.00")

                    Next i

                End If

                With dgv_Details_Total
                    .Rows.Clear()
                    .Rows.Add()
                    .Rows(0).Cells(2).Value = Format(Val(dt1.Rows(0).Item("Total_Marks").ToString), "########0.00")
                End With

                dt2.Dispose()
                da2.Dispose()

            End If

            dt1.Dispose()
            da1.Dispose()

            Grid_DeSelect()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT MOVE RECORDS...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally
            If cbo_Class.Visible And cbo_Class.Enabled Then cbo_Class.Focus()

        End Try

    End Sub

    Private Sub JobWork_PavuYarn_Receipt_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim NoofComps As Integer = 0
        Dim CompCondt As String = ""

        Try

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Class.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "CLASS" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Class.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Student.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "STUDENT" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Student.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Grid_Subject.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "SUBJECT" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Grid_Subject.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            Common_Procedures.Master_Return.Form_Name = ""
            Common_Procedures.Master_Return.Control_Name = ""
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""


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

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "DOES NOT SHOW...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        FrmLdSTS = False

    End Sub

    Private Sub MarkSheet_Entry_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim dt3 As New DataTable
        Dim dt4 As New DataTable
        Dim dt5 As New DataTable
        Dim dt6 As New DataTable

        Me.Text = ""

        con.Open()

        da = New SqlClient.SqlDataAdapter("select class_Name from Class_Head order by class_Name", con)
        da.Fill(dt1)
        cbo_Class.DataSource = dt1
        cbo_Class.DisplayMember = "class_Name"

        da = New SqlClient.SqlDataAdapter("select student_Name from student_Head order by student_Name", con)
        da.Fill(dt2)
        cbo_Student.DataSource = dt2
        cbo_Student.DisplayMember = "student_Name"

        da = New SqlClient.SqlDataAdapter("select Subject_Name from Subject_Head order by Subject_Name", con)
        da.Fill(dt3)
        cbo_Grid_Subject.DataSource = dt3
        cbo_Grid_Subject.DisplayMember = "Subject_Name"

        cbo_Term.Items.Clear()
        cbo_Term.Items.Add("TERM-1")
        cbo_Term.Items.Add("TERM-2")
        cbo_Term.Items.Add("TERM-3")

        pnl_Filter.Visible = False
        pnl_Filter.Left = (Me.Width - pnl_Filter.Width) \ 2
        pnl_Filter.Top = (Me.Height - pnl_Filter.Height) \ 2


        AddHandler cbo_Class.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Student.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Term.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Grid_Subject.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Filter_Class.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Filter_Student.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Filter_Term.GotFocus, AddressOf ControlGotFocus

        AddHandler cbo_Class.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Student.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Term.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Grid_Subject.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Filter_Class.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Filter_Student.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Filter_Term.LostFocus, AddressOf ControlLostFocus

        lbl_Company.Text = ""
        lbl_Company.Tag = 0
        lbl_Company.Visible = False
        Common_Procedures.CompIdNo = 0

        Filter_Status = False
        FrmLdSTS = True
        new_record()

    End Sub

    Private Sub MarkSheet_Entry_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        con.Close()
        con.Dispose()
    End Sub

    Private Sub MarkSheet_Entry_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        Try
            If Asc(e.KeyChar) = 27 Then

                If pnl_Filter.Visible = True Then
                    btn_Filter_Close_Click(sender, e)
                    Exit Sub

                Else
                    Close_Form()

                End If

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT CLOSE...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub Close_Form()
        Dim da As SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim NoofComps As Integer
        Dim CompCondt As String

        Try

            lbl_Company.Tag = 0
            lbl_Company.Text = ""
            Me.Text = ""
            Common_Procedures.CompIdNo = 0

            CompCondt = ""
            If Trim(UCase(Common_Procedures.User.Type)) = "ACCOUNT" Then
                CompCondt = "Company_Type = 'ACCOUNT'"
            End If

            da = New SqlClient.SqlDataAdapter("select count(*) from Company_Head where " & CompCondt & IIf(Trim(CompCondt) <> "", " and ", "") & " Company_IdNo <> 0", con)
            dt1 = New DataTable
            da.Fill(dt1)

            NoofComps = 0
            If dt1.Rows.Count > 0 Then
                If IsDBNull(dt1.Rows(0)(0).ToString) = False Then
                    NoofComps = Val(dt1.Rows(0)(0).ToString)
                End If
            End If
            dt1.Clear()

            If Val(NoofComps) > 1 Then

                Dim f As New Company_Selection
                f.ShowDialog()

                If Val(Common_Procedures.CompIdNo) <> 0 Then

                    da = New SqlClient.SqlDataAdapter("select Company_IdNo, Company_Name from Company_Head where Company_IdNo = " & Str(Val(Common_Procedures.CompIdNo)), con)
                    dt1 = New DataTable
                    da.Fill(dt1)

                    If dt1.Rows.Count > 0 Then
                        If IsDBNull(dt1.Rows(0)(0).ToString) = False Then
                            lbl_Company.Tag = Val(dt1.Rows(0)(0).ToString)
                            lbl_Company.Text = Trim(dt1.Rows(0)(1).ToString)
                            Me.Text = Trim(dt1.Rows(0)(1).ToString)
                        End If
                    End If
                    dt1.Clear()
                    dt1.Dispose()
                    da.Dispose()

                    new_record()

                Else
                    Me.Close()

                End If

            Else

                Me.Close()

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT CLOSE...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Protected Overrides Function ProcessCmdKey(ByRef msg As System.Windows.Forms.Message, ByVal keyData As System.Windows.Forms.Keys) As Boolean
        Dim dgv1 As New DataGridView

        If keyData = Keys.Enter Then

            On Error Resume Next

            If ActiveControl.Name = dgv_Details.Name Or TypeOf ActiveControl Is DataGridViewTextBoxEditingControl Then

                dgv1 = dgv_Details

                With dgv1

                    If .CurrentCell.ColumnIndex = .ColumnCount - 1 Then

                        If .CurrentCell.RowIndex = .RowCount - 1 Then

                            If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                                save_record()

                            Else
                                cbo_Class.Focus()
                                Return True
                                Exit Function

                            End If

                        Else
                            .CurrentCell = .Rows(.CurrentCell.RowIndex + 1).Cells(1)

                        End If

                    Else

                        If .CurrentCell.RowIndex = .RowCount - 1 And .CurrentCell.ColumnIndex >= 1 And (Trim(.CurrentRow.Cells(1).Value) = "" And Val(.CurrentRow.Cells(2).Value) = 0) Then
                            If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                                save_record()

                            Else
                                cbo_Class.Focus()
                                Return True
                                Exit Function
                            End If

                        Else
                            .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 1)

                        End If


                    End If

                End With

            Else

                Return MyBase.ProcessCmdKey(msg, keyData)
                'SendKeys.Send("{TAB}")

            End If

            Return True

        Else

            Return MyBase.ProcessCmdKey(msg, keyData)

        End If


    End Function

    Public Sub delete_record() Implements Interface_MDIActions.delete_record
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim cmd As New SqlClient.SqlCommand
        Dim trans As SqlClient.SqlTransaction
        Dim NewCode As String = ""

        'If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.Pavu_Delivery_Entry, "~L~") = 0 And InStr(Common_Procedures.UR.Pavu_Delivery_Entry, "~D~") = 0 Then MessageBox.Show("You have No Rights to Delete", "DOES NOT INSERT...", MessageBoxButtons.OK, MessageBoxIcon.Error) : Exit Sub

        If pnl_Back.Enabled = False Then
            MessageBox.Show("Close other windows", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        If MessageBox.Show("Do you want to Delete?", "FOR DELETION...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) <> DialogResult.Yes Then
            Exit Sub
        End If

        If New_Entry = True Then
            MessageBox.Show("This is New Entry", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        trans = con.BeginTransaction

        Try

            NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_Code.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            cmd.Connection = con
            cmd.Transaction = trans

            cmd.CommandText = "Delete from Mark_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Mark_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "delete from Mark_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Mark_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            trans.Commit()

            Dt1.Dispose()
            Da.Dispose()
            trans.Dispose()
            cmd.Dispose()

            new_record()

            MessageBox.Show("Deleted Sucessfully!!!", "FOR DELETION...", MessageBoxButtons.OK, MessageBoxIcon.Information)

        Catch ex As Exception
            trans.Rollback()
            MessageBox.Show(ex.Message, "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally
            If cbo_Class.Enabled = True And cbo_Class.Visible = True Then cbo_Class.Focus()

        End Try

    End Sub

    Public Sub filter_record() Implements Interface_MDIActions.filter_record

        If Filter_Status = False Then

            Dim da As New SqlClient.SqlDataAdapter
            Dim dt1 As New DataTable
            Dim dt2 As New DataTable
            Dim dt3 As New DataTable

            da = New SqlClient.SqlDataAdapter("select class_Name from Class_Head order by class_Name", con)
            da.Fill(dt1)
            cbo_Filter_Class.DataSource = dt1
            cbo_Filter_Class.DisplayMember = "class_Name"

            da = New SqlClient.SqlDataAdapter("select student_Name from student_Head order by student_Name", con)
            da.Fill(dt2)
            cbo_Filter_Student.DataSource = dt2
            cbo_Filter_Student.DisplayMember = "student_Name"

            cbo_Filter_Term.Items.Clear()
            cbo_Filter_Term.Items.Add("TERM-1")
            cbo_Filter_Term.Items.Add("TERM-2")
            cbo_Filter_Term.Items.Add("TERM-3")

            cbo_Filter_Class.Text = ""
            cbo_Filter_Student.Text = ""
            cbo_Filter_Term.Text = ""
            cbo_Filter_Class.SelectedIndex = -1
            cbo_Filter_Student.SelectedIndex = -1
            cbo_Filter_Term.SelectedIndex = -1
            dgv_Filter_Details.Rows.Clear()

        End If

        pnl_Filter.Visible = True
        pnl_Back.Enabled = False
        If cbo_Filter_Class.Enabled And cbo_Filter_Class.Visible Then cbo_Filter_Class.Focus()

    End Sub

    Public Sub insert_record() Implements Interface_MDIActions.insert_record
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim movno As String, inpno As String
        Dim RefCode As String

        'If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.Pavu_Delivery_Entry, "~L~") = 0 And InStr(Common_Procedures.UR.Pavu_Delivery_Entry, "~I~") = 0 Then MessageBox.Show("You have No Rights to Insert", "DOES NOT INSERT...", MessageBoxButtons.OK, MessageBoxIcon.Error) : Exit Sub

        Try

            inpno = InputBox("Enter New Ref.No.", "FOR NEW NO INSERTION...")

            RefCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

            Da = New SqlClient.SqlDataAdapter("select Mark_No from Mark_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Mark_Code = '" & Trim(RefCode) & "'", con)
            Da.Fill(Dt)

            movno = ""
            If Dt.Rows.Count > 0 Then
                If IsDBNull(Dt.Rows(0)(0).ToString) = False Then
                    movno = Trim(Dt.Rows(0)(0).ToString)
                End If
            End If

            Dt.Clear()
            Dt.Dispose()
            Da.Dispose()

            If Val(movno) <> 0 Then
                move_record(movno)

            Else
                If Val(inpno) = 0 Then
                    MessageBox.Show("Invalid Ref No", "DOES NOT INSERT NEW NO...", MessageBoxButtons.OK, MessageBoxIcon.Error)

                Else
                    new_record()
                    Insert_Entry = True
                    lbl_Code.Text = Trim(UCase(inpno))

                End If

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT INSERT NEW RECEIPT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Public Sub movefirst_record() Implements Interface_MDIActions.movefirst_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim movno As String

        Try

            da = New SqlClient.SqlDataAdapter("select top 1 Mark_No from Mark_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Mark_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby, Mark_No", con)
            da.Fill(dt)

            movno = ""
            If dt.Rows.Count > 0 Then
                If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                    movno = Trim(dt.Rows(0)(0).ToString)
                End If
            End If

            dt.Clear()
            dt.Dispose()
            da.Dispose()

            If Val(movno) <> 0 Then move_record(movno)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR  MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Public Sub movenext_record() Implements Interface_MDIActions.movenext_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim movno As String = ""
        Dim OrdByNo As Single = 0

        Try

            OrdByNo = Common_Procedures.OrderBy_CodeToValue(Trim(lbl_Code.Text))

            da = New SqlClient.SqlDataAdapter("select top 1 Mark_No from Mark_Head where for_orderby > " & Str(Val(OrdByNo)) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and Mark_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby, Mark_No", con)
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

        End Try

    End Sub

    Public Sub moveprevious_record() Implements Interface_MDIActions.moveprevious_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim movno As String = ""
        Dim OrdByNo As Single = 0

        Try

            OrdByNo = Common_Procedures.OrderBy_CodeToValue(Trim(lbl_Code.Text))

            da = New SqlClient.SqlDataAdapter("select top 1 Mark_No from Mark_Head where for_orderby < " & Str(Val(OrdByNo)) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and Mark_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, Mark_No desc", con)
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

        End Try

    End Sub

    Public Sub movelast_record() Implements Interface_MDIActions.movelast_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim movno As String = ""

        Try
            da = New SqlClient.SqlDataAdapter("select top 1 Mark_No from Mark_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Mark_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, Mark_No desc", con)
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

        End Try

    End Sub

    Public Sub new_record() Implements Interface_MDIActions.new_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim NewID As Integer = 0

        Try
            clear()

            New_Entry = True

            lbl_Code.Text = Common_Procedures.get_MaxCode(con, "Mark_Head", "Mark_Code", "For_OrderBy", "", Val(lbl_Company.Tag), Common_Procedures.FnYearCode)

            lbl_Code.ForeColor = Color.Red

            If cbo_Class.Enabled And cbo_Class.Visible Then cbo_Class.Focus()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR NEW RECORD...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        dt.Dispose()
        da.Dispose()

    End Sub

    Public Sub open_record() Implements Interface_MDIActions.open_record
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim movno As String, inpno As String
        Dim RefCode As String

        Try

            inpno = InputBox("Enter Ref.No.", "FOR FINDING...")

            RefCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

            Da = New SqlClient.SqlDataAdapter("select Mark_No from Mark_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Mark_Code = '" & Trim(RefCode) & "'", con)
            Da.Fill(Dt)

            movno = ""
            If Dt.Rows.Count > 0 Then
                If IsDBNull(Dt.Rows(0)(0).ToString) = False Then
                    movno = Trim(Dt.Rows(0)(0).ToString)
                End If
            End If

            Dt.Clear()
            Dt.Dispose()
            Da.Dispose()

            If Val(movno) <> 0 Then
                move_record(movno)

            Else
                MessageBox.Show("Ref No. does not exists", "DOES NOT FIND...", MessageBoxButtons.OK, MessageBoxIcon.Error)

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT FIND...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Public Sub print_record() Implements Interface_MDIActions.print_record
        '--------
    End Sub

    Public Sub save_record() Implements Interface_MDIActions.save_record
        Dim cmd As New SqlClient.SqlCommand
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim tr As SqlClient.SqlTransaction
        Dim NewCode As String = ""
        Dim Cls_ID As Integer = 0
        Dim Sub_ID As Integer = 0
        Dim Stud_ID As Integer = 0
        Dim Sno As Integer = 0
        Dim vTotMarks As Single = 0

        If Val(lbl_Company.Tag) = 0 Then
            MessageBox.Show("Invalid Company Selection", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        'If Common_Procedures.UserRight_Check(Common_Procedures.UR.Pavu_Delivery_Entry, New_Entry) = False Then Exit Sub

        If pnl_Back.Enabled = False Then
            MessageBox.Show("Close Other Windows", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        Cls_ID = Common_Procedures.Class_NameToIdNo(con, cbo_Class.Text)
        If Cls_ID = 0 Then
            MessageBox.Show("Invalid Class Name", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If cbo_Class.Enabled And cbo_Class.Visible Then cbo_Class.Focus()
            Exit Sub
        End If

        Stud_ID = Common_Procedures.Student_NameToIdNo(con, cbo_Student.Text)
        If Stud_ID = 0 Then
            MessageBox.Show("Invalid Student Name", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If cbo_Student.Enabled And cbo_Student.Visible Then cbo_Student.Focus()
            Exit Sub
        End If

        For i = 0 To dgv_Details.RowCount - 1

            If Val(dgv_Details.Rows(i).Cells(2).Value) <> 0 Then

                Sub_ID = Common_Procedures.Subject_NameToIdNo(con, dgv_Details.Rows(i).Cells(1).Value)
                If Sub_ID = 0 Then
                    MessageBox.Show("Invalid Subject Name", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    If dgv_Details.Enabled And dgv_Details.Visible Then
                        dgv_Details.Focus()
                        dgv_Details.CurrentCell = dgv_Details.Rows(i).Cells(1)
                    End If
                    Exit Sub
                End If

            End If

        Next

        vTotMarks = 0
        If dgv_Details_Total.RowCount > 0 Then
            vTotMarks = Val(dgv_Details_Total.Rows(0).Cells(2).Value())
        End If

        tr = con.BeginTransaction

        Try

            If Insert_Entry = True Or New_Entry = False Then
                NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_Code.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            Else

                lbl_Code.Text = Common_Procedures.get_MaxCode(con, "Mark_Head", "Mark_Code", "For_OrderBy", "", Val(lbl_Company.Tag), Common_Procedures.FnYearCode, tr)

                NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_Code.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            End If

            cmd.Connection = con
            cmd.Transaction = tr

            If New_Entry = True Then
                cmd.CommandText = "Insert into mark_Head(Mark_Code, Company_IdNo, Mark_No, for_OrderBy, Class_IdNo, Student_IdNo, Term_Name, Total_Marks) Values ('" & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_Code.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_Code.Text))) & ", " & Str(Val(Cls_ID)) & ", " & Str(Val(Stud_ID)) & ", '" & Trim(cbo_Term.Text) & "', " & Str(Val(vTotMarks)) & " )"
                cmd.ExecuteNonQuery()

            Else
                cmd.CommandText = "Update Mark_Head set Class_IdNo = " & Str(Val(Cls_ID)) & ", Term_Name = '" & Trim(cbo_Term.Text) & "', Student_IdNo = " & Str(Val(Stud_ID)) & ", Total_Marks = " & Str(Val(vTotMarks)) & " Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Mark_Code = '" & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()

            End If


            cmd.CommandText = "Delete from Mark_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Mark_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            With dgv_Details
                Sno = 0
                For i = 0 To dgv_Details.RowCount - 1

                    If Val(dgv_Details.Rows(i).Cells(2).Value) <> 0 Then

                        Sno = Sno + 1

                        Sub_ID = Common_Procedures.Subject_NameToIdNo(con, dgv_Details.Rows(i).Cells(1).Value, tr)

                        cmd.CommandText = "Insert into Mark_Details ( Mark_Code, Company_IdNo, Mark_No, for_OrderBy, Sl_No, Subject_IdNo, Marks) Values ('" & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_Code.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_Code.Text))) & ", " & Str(Val(Sno)) & ", " & Val(Sub_ID) & ", " & Str(Val(.Rows(i).Cells(2).Value)) & " )"
                        cmd.ExecuteNonQuery()

                    End If

                Next

            End With


            tr.Commit()

            Dt1.Dispose()
            Da.Dispose()

            move_record(lbl_Code.Text)

            MessageBox.Show("Saved Sucessfully!!!", "FOR SAVING...", MessageBoxButtons.OK, MessageBoxIcon.Information)

        Catch ex As Exception
            tr.Rollback()
            MessageBox.Show(ex.Message, "FOR SAVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally
            If cbo_Class.Enabled And cbo_Class.Visible Then cbo_Class.Focus()

        End Try

    End Sub

    Private Sub btn_Save_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Save.Click
        save_record()
    End Sub

    Private Sub btn_Cancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Cancel.Click
        Me.Close()
    End Sub

    Private Sub cbo_Class_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Class.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Class, Nothing, cbo_Student, "CLASS_HEAD", "class_name", "", "(class_idno = 0)")
        If e.KeyValue = 38 And cbo_Class.DroppedDown = False Then
            dgv_Details.Focus()
            dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(1)
        End If
    End Sub

    Private Sub cbo_Class_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Class.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Class, cbo_Student, "CLASS_HEAD", "class_name", "", "(class_idno = 0)")
    End Sub

    Private Sub cbo_Class_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Class.KeyUp
        If e.Control = False And e.KeyValue = 17 Then
            Dim f As New class_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Class.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If
    End Sub

    Private Sub cbo_Student_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Student.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Student, cbo_Class, cbo_Term, "student_HEAD", "student_name", "", "(student_idno = 0)")
    End Sub

    Private Sub cbo_Student_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Student.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Student, cbo_Term, "student_HEAD", "student_name", "", "(student_idno = 0)")
    End Sub

    Private Sub cbo_Student_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Student.KeyUp
        If e.Control = False And e.KeyValue = 17 Then
            Dim f As New Student_creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Student.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If
    End Sub

    Private Sub cbo_Term_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Term.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Term, cbo_Student, Nothing, "", "", "", "")
        If e.KeyValue = 40 And cbo_Term.DroppedDown = False Then
            dgv_Details.Focus()
            dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(1)
        End If
    End Sub

    Private Sub cbo_Term_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Term.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Term, Nothing, "", "", "", "")
        If Asc(e.KeyChar) = 13 Then
            dgv_Details.Focus()
            dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(1)
        End If
    End Sub

    Private Sub dgv_Details_CellEndEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellEndEdit
        With dgv_Details
            If .CurrentCell.ColumnIndex = 2 Then
                If Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value) <> 0 Then
                    .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = Format(Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value), "#########0.00")
                End If
            End If
        End With
    End Sub

    Private Sub dgv_Details_CellEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellEnter
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim Dt2 As New DataTable
        Dim Dt3 As New DataTable
        Dim rect As Rectangle

        With dgv_Details

            If Val(.CurrentRow.Cells(0).Value) = 0 Then
                .CurrentRow.Cells(0).Value = .CurrentRow.Index + 1
            End If

            If e.ColumnIndex = 1 Then

                If cbo_Grid_Subject.Visible = False Or Val(cbo_Grid_Subject.Tag) <> e.RowIndex Then

                    cbo_Grid_Subject.Tag = -1
                    Da = New SqlClient.SqlDataAdapter("select Subject_Name from Subject_Head order by Subject_Name", con)
                    Dt1 = New DataTable
                    Da.Fill(Dt1)
                    cbo_Grid_Subject.DataSource = Dt1
                    cbo_Grid_Subject.DisplayMember = "Subject_Name"

                    rect = .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False)

                    cbo_Grid_Subject.Left = .Left + rect.Left  '  .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Left
                    cbo_Grid_Subject.Top = .Top + rect.Top  ' .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Top

                    cbo_Grid_Subject.Width = rect.Width  ' .CurrentCell.Size.Width
                    cbo_Grid_Subject.Height = rect.Height  ' rect.Height
                    cbo_Grid_Subject.Text = .CurrentCell.Value  '  Trim(.CurrentRow.Cells(e.ColumnIndex).Value)

                    cbo_Grid_Subject.Tag = Val(e.RowIndex)
                    cbo_Grid_Subject.Visible = True

                    cbo_Grid_Subject.BringToFront()
                    cbo_Grid_Subject.Focus()

                    'cbo_Grid_MillName.Visible = False
                    'cbo_Grid_YarnType.Visible = False

                End If


            Else
                cbo_Grid_Subject.Visible = False

            End If

        End With

    End Sub

    Private Sub dgv_Details_CellLeave(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellLeave
        With dgv_Details
            If .CurrentCell.ColumnIndex = 2 Then
                If Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value) <> 0 Then
                    .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = Format(Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value), "#########0.00")
                End If
            End If
        End With
    End Sub

    Private Sub dgv_Details_CellValueChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellValueChanged
        On Error Resume Next
        With dgv_Details
            If .Visible Then
                If .CurrentCell.ColumnIndex = 2 Then
                    TotalMarks_Calculation()
                End If
            End If
        End With
    End Sub

    Private Sub dgv_Details_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_Details.KeyDown

        'On Error Resume Next

        With dgv_Details

            'MsgBox("dgv_YarnDetails_KeyDown : " & .CurrentCell.ColumnIndex())

            If e.KeyCode = Keys.Up Then
                If .CurrentCell.RowIndex = 0 Then
                    cbo_Term.Focus()
                End If
            End If
            If e.KeyCode = Keys.Left Then
                If .CurrentCell.RowIndex = 0 And .CurrentCell.ColumnIndex <= 0 Then
                    cbo_Term.Focus()
                End If
            End If

        End With
    End Sub

    Private Sub dgv_Details_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_Details.KeyUp
        Dim i As Integer
        Dim n As Integer

        If e.Control = True And UCase(Chr(e.KeyCode)) = "D" Then

            With dgv_Details

                n = .CurrentRow.Index

                If .Rows.Count = 1 Then
                    For i = 0 To .Rows.Count - 1
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

    Private Sub dgv_Details_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgv_Details.LostFocus
        On Error Resume Next
        dgv_Details.CurrentCell.Selected = False
    End Sub

    Private Sub dgv_Details_RowsAdded(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewRowsAddedEventArgs) Handles dgv_Details.RowsAdded
        Dim n As Integer

        With dgv_Details
            n = .RowCount
            .Rows(n - 1).Cells(0).Value = Val(n)
        End With
    End Sub

    
    Private Sub cbo_Grid_Subject_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Grid_Subject.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Grid_Subject, Nothing, Nothing, "Subject_Head", "subject_name", "", "(subject_idno = 0)")
        With dgv_Details

            If e.KeyValue = 38 And cbo_Grid_Subject.DroppedDown = False Then

                If Trim(.Rows(.CurrentRow.Index).Cells(1).Value) = "" And .CurrentRow.Index = .Rows.Count - 1 Then
                    If MessageBox.Show("Do you want to save", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = DialogResult.Yes Then
                        save_record()
                    Else
                        cbo_Class.Focus()
                    End If

                Else
                    dgv_Details.Focus()
                    dgv_Details.CurrentCell = dgv_Details.Rows(dgv_Details.CurrentRow.Index).Cells(2)

                End If

            End If

            If e.KeyValue = 40 And cbo_Grid_Subject.DroppedDown = False Then

                If Trim(.Rows(.CurrentRow.Index).Cells(1).Value) = "" And .CurrentRow.Index = .Rows.Count - 1 Then
                    btn_Save.Focus()
                    'If MessageBox.Show("Do you want to save", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = DialogResult.Yes Then
                    '    save_record()
                    'Else
                    '    cbo_Class.Focus()
                    'End If

                Else
                    dgv_Details.Focus()
                    dgv_Details.CurrentCell = dgv_Details.Rows(dgv_Details.CurrentRow.Index).Cells(2)

                End If

            End If

        End With
    End Sub

    Private Sub cbo_Grid_Subject_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Grid_Subject.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Grid_Subject, Nothing, "Subject_Head", "subject_name", "", "(subject_idno = 0)")
        If Asc(e.KeyChar) = 13 Then
            With dgv_Details
                If Trim(.Rows(.CurrentRow.Index).Cells(1).Value) = "" And .CurrentRow.Index = .Rows.Count - 1 Then
                    If MessageBox.Show("Do you want to save", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = DialogResult.Yes Then
                        save_record()
                    Else
                        cbo_Class.Focus()
                    End If

                Else
                    dgv_Details.Focus()
                    dgv_Details.CurrentCell = dgv_Details.Rows(dgv_Details.CurrentRow.Index).Cells(2)

                End If
            End With
        End If
    End Sub

    Private Sub cbo_Grid_Subject_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Grid_Subject.KeyUp
        If e.Control = False And e.KeyValue = 17 Then
            Dim f As New Subject_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Grid_Subject.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If
    End Sub

    Private Sub cbo_Grid_Subject_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Grid_Subject.TextChanged
        Try
            If cbo_Grid_Subject.Visible Then
                With dgv_Details
                    If Val(cbo_Grid_Subject.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = 1 Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(1).Value = Trim(cbo_Grid_Subject.Text)
                    End If
                End With
            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub


    Private Sub TotalMarks_Calculation()
        Dim Sno As Integer
        Dim TotMrks As Single

        Sno = 0
        TotMrks = 0
        With dgv_Details
            For i = 0 To .RowCount - 1
                Sno = Sno + 1
                .Rows(i).Cells(0).Value = Sno
                If Val(.Rows(i).Cells(2).Value) <> 0 Then
                    TotMrks = TotMrks + Val(.Rows(i).Cells(2).Value)
                End If
            Next
        End With

        With dgv_Details_Total
            If .RowCount = 0 Then .Rows.Add()
            .Rows(0).Cells(2).Value = Format(Val(TotMrks), "########0.00")
        End With

    End Sub

    Private Sub cbo_Filter_Class_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Filter_Class.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_Class, Nothing, cbo_Filter_Student, "class_head", "class_name", "", "(class_idno = 0)")
    End Sub

    Private Sub cbo_Filter_Class_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Filter_Class.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Filter_Class, cbo_Filter_Student, "class_head", "class_name", "", "(class_idno = 0)")
    End Sub

    Private Sub cbo_Filter_Student_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Filter_Student.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_Student, cbo_Filter_Class, cbo_Filter_Term, "student_head", "student_name", "", "(student_idno = 0)")
    End Sub

    Private Sub cbo_Filter_Student_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Filter_Student.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Filter_Student, cbo_Filter_Term, "student_head", "student_name", "", "(student_idno = 0)")
    End Sub

    Private Sub cbo_Filter_Term_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Filter_Term.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_Term, cbo_Filter_Student, btn_Filter_Show, "", "", "", "")
    End Sub

    Private Sub cbo_Filter_Term_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Filter_Term.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Filter_Term, Nothing, "", "", "", "")
        If Asc(e.KeyChar) = 13 Then
            btn_Filter_Show_Click(sender, e)
        End If
    End Sub

    Private Sub btn_Filter_Show_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Filter_Show.Click
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim n As Integer, i As Integer
        Dim Cls_IdNo As Integer, Stud_IdNo As Integer
        Dim Condt As String = ""

        Try

            Condt = ""
            Cls_IdNo = 0
            Stud_IdNo = 0

            If Trim(cbo_Filter_Class.Text) <> "" Then
                Cls_IdNo = Common_Procedures.Class_NameToIdNo(con, cbo_Filter_Class.Text)
            End If

            If Trim(cbo_Filter_Student.Text) <> "" Then
                Stud_IdNo = Common_Procedures.Student_NameToIdNo(con, cbo_Filter_Student.Text)
            End If

            If Val(Cls_IdNo) <> 0 Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " a.Class_IdNo = " & Str(Val(Cls_IdNo))
            End If

            If Val(Stud_IdNo) <> 0 Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " a.Student_IdNo = " & Str(Val(Stud_IdNo))
            End If

            If Trim(cbo_Filter_Term.Text) <> "" Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " a.Term_Name = '" & Trim(cbo_Filter_Term.Text) & "'"
            End If

            da = New SqlClient.SqlDataAdapter("select a.*, b.class_Name, c.student_Name from Mark_Head a INNER JOIN Class_Head b ON a.Class_IdNo = b.class_IdNo INNER JOIN Student_Head c ON a.Student_IdNo = c.Student_IdNo Where a.company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.mark_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "' " & IIf(Trim(Condt) <> "", " and ", "") & Condt & " Order by a.for_orderby, a.Mark_No", con)
            da.Fill(dt2)

            dgv_Filter_Details.Rows.Clear()

            If dt2.Rows.Count > 0 Then

                For i = 0 To dt2.Rows.Count - 1

                    n = dgv_Filter_Details.Rows.Add()

                    dgv_Filter_Details.Rows(n).Cells(0).Value = i + 1
                    dgv_Filter_Details.Rows(n).Cells(1).Value = dt2.Rows(i).Item("Mark_No").ToString
                    dgv_Filter_Details.Rows(n).Cells(2).Value = dt2.Rows(i).Item("Class_Name").ToString
                    dgv_Filter_Details.Rows(n).Cells(3).Value = dt2.Rows(i).Item("Student_Name").ToString
                    dgv_Filter_Details.Rows(n).Cells(4).Value = dt2.Rows(i).Item("Term_name").ToString
                    dgv_Filter_Details.Rows(n).Cells(5).Value = Format(Val(dt2.Rows(i).Item("Total_Marks").ToString), "########0.00")

                Next i

            End If

            dt2.Clear()
            dt2.Dispose()
            da.Dispose()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT FILTER...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        If dgv_Filter_Details.Visible And dgv_Filter_Details.Enabled Then dgv_Filter_Details.Focus()

    End Sub

    Private Sub btn_Filter_Close_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Filter_Close.Click
        pnl_Back.Enabled = True
        pnl_Filter.Visible = False
        Filter_Status = False
    End Sub

    Private Sub Open_FilterEntry()
        Dim movno As String

        On Error Resume Next

        movno = Trim(dgv_Filter_Details.CurrentRow.Cells(1).Value)

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

    Private Sub dgv_Filter_Details_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_Filter_Details.KeyDown
        If e.KeyCode = 13 Then
            Open_FilterEntry()
        End If
    End Sub

End Class