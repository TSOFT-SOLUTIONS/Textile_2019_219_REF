Public Class Empolyee_Attendance

        Implements Interface_MDIActions

        Private con As New SqlClient.SqlConnection(Common_Procedures.Connection_String)
        Private FrmLdSTS As Boolean = False
        Private New_Entry As Boolean = False
        Private Insert_Entry As Boolean = False
        Private Filter_Status As Boolean = False

        Private NoCalc_Status As Boolean = False
        Private Prec_ActCtrl As New Control
        Private vCbo_ItmNm As String
        Private vcbo_KeyDwnVal As Double

        Private WithEvents dgtxt_Details As New DataGridViewTextBoxEditingControl

        Private prn_HdDt As New DataTable
        Private prn_DetDt As New DataTable
        Private prn_PageNo As Integer
        Private prn_DetIndx As Integer
        Private prn_DetAr(50, 10) As String
        Private prn_DetMxIndx As Integer
        Private prn_NoofBmDets As Integer
    Private prn_DetSNo As Integer
    Public vmskOldText As String = ""
    Public vmskSelStrt As Integer = -1

        Public Sub New()
            ' This call is required by the designer.
            InitializeComponent()
            ' Add any initialization after the InitializeComponent() call.
            clear()
        End Sub

    Private Property cbo_VatAc2 As Object

    Private Property dgv_Details_Total As Object

        Private Sub clear()

            NoCalc_Status = True

            New_Entry = False
            Insert_Entry = False

            pnl_Back.Enabled = True

        vmskOldText = ""
        vmskSelStrt = -1
            lbl_RefNo.Text = ""
            lbl_RefNo.ForeColor = Color.Black
        msk_Date.Text = ""
            dtp_Date.Text = ""


           
        lbl_day.Text = ""




        dgv_Details.Rows.Clear()

       

        Grid_Cell_DeSelect()

        Cbo_EmployeeName.Visible = False


        NoCalc_Status = False

    End Sub

    Private Sub ControlGotFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim txtbx As TextBox
        Dim combobx As ComboBox
        Dim msktxbx As MaskedTextBox
        On Error Resume Next

        If TypeOf Me.ActiveControl Is TextBox Or TypeOf Me.ActiveControl Is ComboBox Or TypeOf Me.ActiveControl Is MaskedTextBox Then
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

        If Me.ActiveControl.Name <> Cbo_EmployeeName.Name Then
            Cbo_EmployeeName.Visible = False
        End If


        Grid_Cell_DeSelect()

        Prec_ActCtrl = Me.ActiveControl

    End Sub

    Private Sub ControlLostFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        On Error Resume Next
        If IsNothing(Prec_ActCtrl) = False Then
            Prec_ActCtrl.BackColor = Color.White
            Prec_ActCtrl.ForeColor = Color.Black
        End If
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

    Private Sub Grid_Cell_DeSelect()
        On Error Resume Next
        dgv_Details.CurrentCell.Selected = False

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

        NoCalc_Status = True

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(no) & "/" & Trim(Common_Procedures.FnYearCode)

        Try

            da1 = New SqlClient.SqlDataAdapter("select a.* from Employee_Attendance_Head a Where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Employee_Attendance_Code = '" & Trim(NewCode) & "'", con)
            dt1 = New DataTable
            da1.Fill(dt1)

            If dt1.Rows.Count > 0 Then

                lbl_RefNo.Text = dt1.Rows(0).Item("Employee_Attendance_No").ToString
                dtp_Date.Text = dt1.Rows(0).Item("Employee_Attendance_Date").ToString
                msk_Date.Text = dtp_Date.Text
                lbl_Day.Text = dt1.Rows(0).Item("Day").ToString




                da2 = New SqlClient.SqlDataAdapter("Select a.*, b.Employee_Name from Employee_Attendance_Details a INNER JOIN Employee_Head b ON a.Employee_IdNo = b.Employee_IdNo  Where a.Employee_Attendance_Code = '" & Trim(NewCode) & "' Order by a.sl_no", con)
                dt2 = New DataTable
                da2.Fill(dt2)

                With dgv_Details

                    .Rows.Clear()
                    SNo = 0

                    If dt2.Rows.Count > 0 Then

                        For i = 0 To dt2.Rows.Count - 1

                            n = .Rows.Add()

                            SNo = SNo + 1

                            .Rows(n).Cells(0).Value = Val(SNo)
                            .Rows(n).Cells(1).Value = dt2.Rows(i).Item("Employee_Name").ToString
                            .Rows(n).Cells(2).Value = dt2.Rows(i).Item("Working_Type_Name").ToString
                            .Rows(n).Cells(3).Value = Val(dt2.Rows(i).Item("Day_Shift").ToString)
                            .Rows(n).Cells(4).Value = Val(dt2.Rows(i).Item("Night_Shift").ToString)
                            .Rows(n).Cells(5).Value = Format(Val(dt2.Rows(i).Item("Bonus_Shift").ToString), "########0.00")
                            .Rows(n).Cells(6).Value = Format(Val(dt2.Rows(i).Item("Wages_Shift").ToString), "########0.00")
                            .Rows(n).Cells(7).Value = Format(Val(dt2.Rows(i).Item("Tiffen").ToString), "########0.00")
                            .Rows(n).Cells(8).Value = Format(Val(dt2.Rows(i).Item("Extra_Wages").ToString), "########0.00")
                            .Rows(n).Cells(9).Value = Format(Val(dt2.Rows(i).Item("Total_Wages").ToString), "########0.00")

                        Next i

                    End If



                End With




            End If

            Grid_Cell_DeSelect()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT MOVE RECORDS...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally

            dt1.Dispose()
            da1.Dispose()

            dt2.Dispose()
            da2.Dispose()

            If msk_Date.Visible And msk_Date.Enabled Then msk_Date.Focus()

        End Try

        NoCalc_Status = False



    End Sub

    Private Sub Employee_Attendance_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated

        Try


            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(Cbo_EmployeeName.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "EMPLOYEE" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                Cbo_EmployeeName.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If



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

    Private Sub Employee_Attendance_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable


        Me.Text = ""

        con.Open()

        'Common_Procedures.get_VehicleNo_From_All_Entries(con)

        da = New SqlClient.SqlDataAdapter("select Employee_Name from Employee_Head order by Employee_Name", con)
        da.Fill(dt1)
        Cbo_EmployeeName.DataSource = dt1
        Cbo_EmployeeName.DisplayMember = "Employee_Name"





       

        AddHandler dtp_Date.GotFocus, AddressOf ControlGotFocus
        AddHandler msk_Date.GotFocus, AddressOf ControlGotFocus
        AddHandler Cbo_EmployeeName.GotFocus, AddressOf ControlGotFocus

       

        AddHandler btn_save.GotFocus, AddressOf ControlGotFocus

        AddHandler btn_close.GotFocus, AddressOf ControlGotFocus


        AddHandler msk_Date.LostFocus, AddressOf ControlLostFocus
        AddHandler dtp_Date.LostFocus, AddressOf ControlLostFocus
        AddHandler Cbo_EmployeeName.LostFocus, AddressOf ControlLostFocus


       
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

    Private Sub Employee_Attendance_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        On Error Resume Next
        con.Close()
        con.Dispose()
        Common_Procedures.Last_Closed_FormName = Me.Name
    End Sub

    Private Sub Employee_Attendance_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress

        Try
            If Asc(e.KeyChar) = 27 Then

                'If pnl_Filter.Visible = True Then
                '    btn_Filter_Close_Click(sender, e)
                '    Exit Sub
                'Else
                If MessageBox.Show("Do you want to Close?", "FOR CLOSING ENTRY...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) <> Windows.Forms.DialogResult.Yes Then
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

                    If .CurrentCell.ColumnIndex >= .ColumnCount - 2 Then
                        If .CurrentCell.RowIndex = .RowCount - 1 Then
                            If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                                save_record()
                            Else
                                msk_Date.Focus()
                            End If

                        Else
                            .CurrentCell = .Rows(.CurrentCell.RowIndex + 1).Cells(1)

                        End If

                    Else

                        If .CurrentCell.RowIndex = .RowCount - 1 And .CurrentCell.ColumnIndex >= 1 And Trim(.CurrentRow.Cells(1).Value) = "" Then
                            If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                                save_record()
                            Else
                                msk_Date.Focus()
                            End If

                        Else
                            .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 1)

                        End If

                    End If

                    Return True

                ElseIf keyData = Keys.Up Then
                    If .CurrentCell.ColumnIndex <= 1 Then
                        If .CurrentCell.RowIndex = 0 Then
                            dtp_Date.Focus()

                        Else
                            .CurrentCell = .Rows(.CurrentCell.RowIndex - 1).Cells(9)

                        End If

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

    Public Sub delete_record() Implements Interface_MDIActions.delete_record
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim cmd As New SqlClient.SqlCommand
        Dim trans As SqlClient.SqlTransaction
        Dim NewCode As String = ""

        'If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.Pavu_Delivery_Entry, "~L~") = 0 And InStr(Common_Procedures.UR.Pavu_Delivery_Entry, "~D~") = 0 Then MessageBox.Show("You have No Rights to Insert", "DOES NOT INSERT...", MessageBoxButtons.OK, MessageBoxIcon.Error) : Exit Sub

        If Val(lbl_Company.Tag) = 0 Then
            MessageBox.Show("Invalid Company Selection", "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
        End If

        If pnl_Back.Enabled = False Then
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

            NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            cmd.Connection = con
            cmd.Transaction = trans





            cmd.CommandText = "delete from Employee_Attendance_Details where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Employee_Attendance_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()
            cmd.CommandText = "delete from Employee_Attendance_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Employee_Attendance_Code = '" & Trim(NewCode) & "'"
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

            If msk_Date.Visible And msk_Date.Enabled Then msk_Date.Focus()

        End Try

    End Sub

    Public Sub filter_record() Implements Interface_MDIActions.filter_record

       
    End Sub

    Public Sub movefirst_record() Implements Interface_MDIActions.movefirst_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim movno As String

        Try

            da = New SqlClient.SqlDataAdapter("select top 1 Employee_Attendance_No from Employee_Attendance_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Employee_Attendance_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby, Employee_Attendance_No", con)
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

    Public Sub movenext_record() Implements Interface_MDIActions.movenext_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim movno As String = ""
        Dim OrdByNo As Single = 0

        Try

            OrdByNo = Common_Procedures.OrderBy_CodeToValue(Trim(lbl_RefNo.Text))

            da = New SqlClient.SqlDataAdapter("select top 1 Employee_Attendance_No from Employee_Attendance_Head where for_orderby > " & Str(OrdByNo) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and Employee_Attendance_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby, Employee_Attendance_No", con)
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

            OrdByNo = Common_Procedures.OrderBy_CodeToValue(Trim(lbl_RefNo.Text))

            da = New SqlClient.SqlDataAdapter("select top 1 Employee_Attendance_No from Employee_Attendance_Head where for_orderby < " & Str(OrdByNo) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and Employee_Attendance_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, Employee_Attendance_No desc", con)
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

    Public Sub movelast_record() Implements Interface_MDIActions.movelast_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim movno As String = ""

        Try
            da = New SqlClient.SqlDataAdapter("select top 1 Employee_Attendance_No from Employee_Attendance_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Employee_Attendance_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, Employee_Attendance_No desc", con)
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
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim Dt2 As New DataTable

        Try
            clear()

            New_Entry = True

            lbl_RefNo.Text = Common_Procedures.get_MaxCode(con, "Employee_Attendance_Head", "Employee_Attendance_Code", "For_OrderBy", "", Val(lbl_Company.Tag), Common_Procedures.FnYearCode)
            lbl_RefNo.ForeColor = Color.Red

            msk_Date.Text = Date.Today.ToShortDateString


            ' dtp_date.Text = Date.Today.ToShortDateString
            Da = New SqlClient.SqlDataAdapter("select top 1 * from Employee_Attendance_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Employee_Attendance_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, Employee_Attendance_No desc", con)
            Dt1 = New DataTable
            da.Fill(Dt1)
            If Dt1.Rows.Count > 0 Then
                If Val(Common_Procedures.settings.PreviousEntryDate_ByDefault) = 1 Then '---- M.S Textiles (Tirupur)

                    If Dt1.Rows(0).Item("Employee_Attendance_Date").ToString <> "" Then msk_Date.Text = Dt1.Rows(0).Item("Employee_Attendance_Date").ToString
                End If
            End If
            Dt1.Clear()
            If msk_Date.Enabled And msk_Date.Visible Then
                msk_Date.Focus()
                msk_Date.SelectionStart = 0
            End If





        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR NEW RECORD...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally

            Dt1.Dispose()
            Dt2.Dispose()
            Da.Dispose()

            If dtp_Date.Enabled And dtp_Date.Visible Then dtp_Date.Focus()

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

            Da = New SqlClient.SqlDataAdapter("select Employee_Attendance_No from Employee_Attendance_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Employee_Attendance_Code = '" & Trim(RefCode) & "'", con)
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

    Public Sub insert_record() Implements Interface_MDIActions.insert_record
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim movno As String, inpno As String
        Dim InvCode As String

        'If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.Pavu_Delivery_Entry, "~L~") = 0 And InStr(Common_Procedures.UR.Pavu_Delivery_Entry, "~I~") = 0 Then MessageBox.Show("You have No Rights to Insert", "DOES NOT INSERT...", MessageBoxButtons.OK, MessageBoxIcon.Error) : Exit Sub

        Try

            inpno = InputBox("Enter New Ref No.", "FOR NEW Ref NO. INSERTION...")

            InvCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

            Da = New SqlClient.SqlDataAdapter("select Employee_Attendance_No from Employee_Attendance_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Employee_Attendance_Code = '" & Trim(InvCode) & "'", con)
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
                    MessageBox.Show("Invalid Ref No.", "DOES NOT INSERT NEW Ref...", MessageBoxButtons.OK, MessageBoxIcon.Error)

                Else
                    new_record()
                    Insert_Entry = True
                    lbl_RefNo.Text = Trim(UCase(inpno))

                End If

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT INSERT NEW BILL...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally
            Dt.Dispose()
            Da.Dispose()

        End Try

    End Sub

    Public Sub save_record() Implements Interface_MDIActions.save_record
        Dim cmd As New SqlClient.SqlCommand
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim tr As SqlClient.SqlTransaction
        Dim NewCode As String = ""
        Dim PurAc_ID As Integer = 0
        Dim Rck_IdNo As Integer = 0
        Dim Fp_Id As Integer = 0
        Dim Led_ID As Integer = 0
        Dim Empe_ID As Integer = 0
        Dim WrkTy_ID As Integer = 0
        Dim Sno As Integer = 0
        Dim EntID As String = ""
        Dim PBlNo As String = ""
        Dim Partcls As String = ""



        Dim VouBil As String = ""
        Dim YrnClthNm As String = ""

        If Val(lbl_Company.Tag) = 0 Then
            MessageBox.Show("Invalid Company Selection", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        'If Common_Procedures.UserRight_Check(Common_Procedures.UR.Pavu_Delivery_Entry, New_Entry) = False Then Exit Sub

        If pnl_Back.Enabled = False Then
            MessageBox.Show("Close Other Windows", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        If IsDate(msk_Date.Text) = False Then
            MessageBox.Show("Invalid Date", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If msk_Date.Enabled Then msk_Date.Focus()
            Exit Sub
        End If


        If Not (Convert.ToDateTime(msk_Date.Text) >= Common_Procedures.Company_FromDate And Convert.ToDateTime(msk_Date.Text) <= Common_Procedures.Company_ToDate) Then
            MessageBox.Show("Date is out of financial range", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If msk_Date.Enabled Then msk_Date.Focus()
            Exit Sub
        End If





        With dgv_Details

            For i = 0 To .RowCount - 1

                If Val(.Rows(i).Cells(3).Value) <> 0 Or Val(.Rows(i).Cells(6).Value) <> 0 Then

                    Empe_ID = Common_Procedures.Employee_NameToIdNo(con, .Rows(i).Cells(1).Value)
                    If Empe_ID = 0 Then
                        MessageBox.Show("Invalid Item Name", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        If .Enabled And .Visible Then
                            .Focus()
                            .CurrentCell = .Rows(i).Cells(1)
                        End If
                        Exit Sub
                    End If

                    If Val(.Rows(i).Cells(6).Value) = 0 Then
                        MessageBox.Show("Invalid Tiffen", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        If .Enabled And .Visible Then
                            .Focus()
                            .CurrentCell = .Rows(i).Cells(6)
                        End If
                        Exit Sub
                    End If

                End If

            Next

        End With


        NoCalc_Status = False


        tr = con.BeginTransaction

        Try

            If Insert_Entry = True Or New_Entry = False Then
                NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            Else

                lbl_RefNo.Text = Common_Procedures.get_MaxCode(con, "Employee_Attendance_Head", "Employee_Attendance_Code", "For_OrderBy", "", Val(lbl_Company.Tag), Common_Procedures.FnYearCode, tr)

                NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            End If

            cmd.Connection = con
            cmd.Transaction = tr

            cmd.Parameters.Clear()
            cmd.Parameters.AddWithValue("@AttenDate", dtp_Date.Value.Date)

            If New_Entry = True Then
                cmd.CommandText = "Insert into Employee_Attendance_Head (       Employee_Attendance_Code ,               Company_IdNo       ,           Employee_Attendance_No    ,                               for_OrderBy           , Employee_Attendance_Date   ,  Day                  ) " & _
                                    "     Values                  (   '" & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_RefNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text))) & ",      @AttenDate    , '" & Trim(lbl_Day.Text) & "' ) "
                cmd.ExecuteNonQuery()
            Else

                cmd.CommandText = "Update Employee_Attendance_Head set Employee_Attendance_Date = @AttenDate,  Day = '" & Trim(lbl_Day.Text) & "' Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Employee_Attendance_Code = '" & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()
            End If

            'EntID = Trim(Pk_Condition) & Trim(lbl_RefNo.Text)
            ''PBlNo = Trim(txt_BillNo.Text)
            Partcls = "Purc : Ref No. " & Trim(lbl_RefNo.Text)

            cmd.CommandText = "Delete from Employee_Attendance_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Employee_Attendance_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()


            With dgv_Details

                Sno = 0
                For i = 0 To .RowCount - 1

                    If Val(.Rows(i).Cells(5).Value) <> 0 Then

                        Sno = Sno + 1

                        Empe_ID = Common_Procedures.Employee_NameToIdNo(con, .Rows(i).Cells(1).Value, tr)
                        '  WrkTy_ID = Common_Procedures.WorkType_NameToIdNo(con, .Rows(i).Cells(2).Value, tr)

                        cmd.CommandText = "Insert into Employee_Attendance_Details ( Employee_Attendance_Code ,               Company_IdNo       ,   Employee_Attendance_No    ,                     for_OrderBy                                            ,   Employee_Attendance_Date  ,             Sl_No     ,  Employee_IdNo    ,     Working_Type_Name   ,                  Day_Shift           ,                Night_Shift             ,                        Bonus_Shift         ,                 Wages_Shift         ,                  Tiffen                               ,       Extra_Wages               ,                          Total_Wages  ) " & _
                                            "     Values                 (   '" & Trim(NewCode) & "'      , " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_RefNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text))) & ",       @AttenDate            ,  " & Str(Val(Sno)) & ", " & Str(Val(Empe_ID)) & ",  '" & Trim(.Rows(i).Cells(2).Value) & "', " & Str(Val(.Rows(i).Cells(3).Value)) & ", " & Str(Val(.Rows(i).Cells(4).Value)) & ", " & Str(Val(.Rows(i).Cells(5).Value)) & ",  " & Str(Val(.Rows(i).Cells(6).Value)) & ", " & Str(Val(.Rows(i).Cells(7).Value)) & " , " & Str(Val(.Rows(i).Cells(8).Value)) & " ," & Str(Val(.Rows(i).Cells(9).Value)) & ") "
                        cmd.ExecuteNonQuery()




                    End If

                Next

            End With





            tr.Commit()
            move_record(lbl_RefNo.Text)

            MessageBox.Show("Saved Sucessfully!!!", "FOR SAVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Information)

        Catch ex As Exception
            tr.Rollback()
            MessageBox.Show(ex.Message, "FOR SAVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally

            Dt1.Dispose()
            Da.Dispose()
            cmd.Dispose()
            tr.Dispose()
            Dt1.Clear()

            If msk_Date.Visible And msk_Date.Enabled Then msk_Date.Focus()


        End Try

    End Sub





   
    Private Sub dgv_Details_CellEndEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellEndEdit
        dgv_Details_CellLeave(sender, e)
        'If dgv_Details.CurrentCell.ColumnIndex = 2 Then
        '    get_Employee_Head()
        'End If
    End Sub

    Private Sub dgv_Details_CellEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellEnter
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim Dt2 As New DataTable
        Dim rect As Rectangle

        With dgv_Details

            If Val(.CurrentRow.Cells(0).Value) = 0 Then
                .CurrentRow.Cells(0).Value = .CurrentRow.Index + 1
            End If

            If e.ColumnIndex = 1 Then

                If Cbo_EmployeeName.Visible = False Or Val(Cbo_EmployeeName.Tag) <> e.RowIndex Then

                    Cbo_EmployeeName.Tag = -1
                    Da = New SqlClient.SqlDataAdapter("select Employee_Name from Employee_Head order by Employee_Name", con)
                    Dt1 = New DataTable
                    Da.Fill(Dt1)
                    Cbo_EmployeeName.DataSource = Dt1
                    Cbo_EmployeeName.DisplayMember = "Employee_Name"

                    rect = .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False)

                    Cbo_EmployeeName.Left = .Left + rect.Left
                    Cbo_EmployeeName.Top = .Top + rect.Top

                    Cbo_EmployeeName.Width = rect.Width
                    Cbo_EmployeeName.Height = rect.Height
                    Cbo_EmployeeName.Text = .CurrentCell.Value

                    Cbo_EmployeeName.Tag = Val(e.RowIndex)
                    Cbo_EmployeeName.Visible = True

                    Cbo_EmployeeName.BringToFront()
                    Cbo_EmployeeName.Focus()

                End If

            Else
                Cbo_EmployeeName.Visible = False

            End If

           


        End With

    End Sub

    Private Sub dgv_Details_CellLeave(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellLeave
        With dgv_Details
            If .CurrentCell.ColumnIndex = 5 Or .CurrentCell.ColumnIndex = 7 Or .CurrentCell.ColumnIndex = 9 Then
                If Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value) <> 0 Then
                    .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = Format(Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value), "#########0.00")
                Else
                    .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = ""
                End If
            End If

          
        End With
    End Sub

    

    Private Sub dgv_Details_CellValueChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellValueChanged
        On Error Resume Next

        With dgv_Details
            If .Visible Then
                If Val(.CurrentCell.ColumnIndex) = 3 Or Val(.CurrentCell.ColumnIndex) = 8 Then
                    .Rows(.CurrentCell.RowIndex).Cells(9).Value = ((Val(dgv_Details.Rows(.CurrentCell.RowIndex).Cells(3).Value) + Val(dgv_Details.Rows(.CurrentCell.RowIndex).Cells(4).Value) + Val(dgv_Details.Rows(.CurrentCell.RowIndex).Cells(5).Value)) * Val(dgv_Details.Rows(.CurrentCell.RowIndex).Cells(6).Value)) + Val(dgv_Details.Rows(.CurrentCell.RowIndex).Cells(8).Value)
                End If
            End If
        End With

    End Sub

    Private Sub dgv_Details_EditingControlShowing(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewEditingControlShowingEventArgs) Handles dgv_Details.EditingControlShowing
        dgtxt_Details = CType(dgv_Details.EditingControl, DataGridViewTextBoxEditingControl)
    End Sub


    Private Sub dgtxt_Details_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgtxt_Details.Enter
        dgv_Details.EditingControl.BackColor = Color.Lime
        dgv_Details.EditingControl.ForeColor = Color.Blue
        dgtxt_Details.SelectAll()
    End Sub
    Private Sub dgtxt_Details_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dgtxt_Details.KeyPress

        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
            e.Handled = True
        End If

    End Sub

    'Private Sub dgv_Details_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_Details.KeyDown
    '    vcbo_KeyDwnVal = e.KeyValue

    'End Sub

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

   
    Private Sub btn_save_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_save.Click
        save_record()
    End Sub

    Private Sub btn_close_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_close.Click
        Me.Close()
    End Sub

    

    Private Sub Amount_Calculation(ByVal CurRow As Integer, ByVal CurCol As Integer)
        On Error Resume Next

        With dgv_Details
            If .Visible Then
                If CurCol = 5 Or CurCol = 6 Then

                    .Rows(CurRow).Cells(9).Value = Format((Val(.Rows(CurRow).Cells(3).Value) + Val(.Rows(CurRow).Cells(4).Value) + Val(.Rows(CurRow).Cells(5).Value)) * Val(.Rows(CurRow).Cells(6).Value), "#########0.00")

                End If



            End If

        End With

    End Sub

    

    Public Sub print_record() Implements Interface_MDIActions.print_record


    End Sub


  
    Private Sub cbo_EmployeeName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Cbo_EmployeeName.KeyDown
        Dim dep_idno As Integer = 0

        vcbo_KeyDwnVal = e.KeyValue

        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, Cbo_EmployeeName, Nothing, Nothing, "Employee_Head", "Employee_Name", "", "(Employee_IdNo = 0)")

        With dgv_Details

            If (e.KeyValue = 38 And Cbo_EmployeeName.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then

                If Val(.CurrentCell.RowIndex) <= 0 Then
                    msk_Date.Focus()
                    
                Else
                    .Focus()
                    .CurrentCell = .Rows(.CurrentCell.RowIndex - 1).Cells(9)
                    .CurrentCell.Selected = True

                End If
            End If

            If (e.KeyValue = 40 And Cbo_EmployeeName.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then

                If .CurrentCell.RowIndex = .Rows.Count - 1 And Trim(.Rows(.CurrentCell.RowIndex).Cells.Item(1).Value) = "" Then

                    If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                        save_record()
                    Else
                        msk_Date.Focus()
                    End If
                Else
                    .Focus()
                    .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(3)

                End If

            End If

        End With

    End Sub

    Private Sub cbo_EmployeeName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Cbo_EmployeeName.KeyPress
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim WrkTy_Nm As String
        Dim Empe_idno As Integer = 0

        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, Cbo_EmployeeName, Nothing, "Employee_Head", "Employee_Name", "", "(Employee_IdNo = 0)")

        If Asc(e.KeyChar) = 13 Then

            With dgv_Details
                .Rows(.CurrentRow.Index).Cells(2).Value = ""


                Empe_idno = Common_Procedures.Employee_NameToIdNo(con, Trim(.Rows(.CurrentRow.Index).Cells(1).Value))

                da = New SqlClient.SqlDataAdapter("select a.*,b.Working_Type_Name from Employee_Head a INNER JOIN Working_Type_Head b ON a.Working_Type_IdNo = b.Working_Type_IdNo where Employee_Idno = " & Str(Val(Empe_idno)), con)
                dt = New DataTable
                da.Fill(dt)

                WrkTy_Nm = ""
                If dt.Rows.Count > 0 Then
                    If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                        WrkTy_Nm = Trim(dt.Rows(0).Item("Working_Type_Name").ToString)
                    End If
                End If
                dt.Clear()

                .Rows(.CurrentRow.Index).Cells(2).Value = Trim(WrkTy_Nm)

                dt.Dispose()
                da.Dispose()

                .Focus()
                .Rows(.CurrentCell.RowIndex).Cells.Item(1).Value = Trim(Cbo_EmployeeName.Text)
                If .CurrentCell.RowIndex = .RowCount - 1 And .CurrentCell.ColumnIndex >= 1 And Trim(.CurrentRow.Cells(1).Value) = "" Then

                    If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                        save_record()
                    Else
                        msk_Date.Focus()
                    End If

                Else
                    .Focus()
                    .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(3)
                End If

            End With

        End If

    End Sub

    Private Sub cbo_EmployeeName_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Cbo_EmployeeName.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            'Dim f As New Employee_Creation

            'Common_Procedures.Master_Return.Form_Name = Me.Name
            'Common_Procedures.Master_Return.Control_Name = Cbo_EmployeeName.Name
            'Common_Procedures.Master_Return.Return_Value = ""
            'Common_Procedures.Master_Return.Master_Type = ""

            'f.MdiParent = MDIParent1
            'f.Show()
        End If
    End Sub

    Private Sub cbo_Grid_ItemName_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles Cbo_EmployeeName.TextChanged
        Try
            If Cbo_EmployeeName.Visible Then
                With dgv_Details
                    If Val(Cbo_EmployeeName.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = 1 Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(Cbo_EmployeeName.Text)
                    End If
                End With
            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub

    
    Private Sub dgtxt_Details_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgtxt_Details.KeyUp
        If e.Control = True And UCase(Chr(e.KeyCode)) = "D" Then
            dgv_Details_KeyUp(sender, e)
        End If
    End Sub




    
    Private Sub dtp_Date_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dtp_Date.KeyDown

        If e.KeyValue = 40 Then
            dgv_Details.Focus()
            dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(1)
            dgv_Details.CurrentCell.Selected = True
        End If
    End Sub

    Private Sub dtp_Date_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dtp_Date.KeyPress
        If Asc(e.KeyChar) = 13 Then
            dgv_Details.Focus()
            dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(1)
            dgv_Details.CurrentCell.Selected = True
        End If
    End Sub

   
    Private Sub dtp_Date_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dtp_Date.TextChanged
        lbl_Day.Text = ""
        If IsDate(dtp_Date.Text) = True Then
            lbl_Day.Text = Format(Convert.ToDateTime(dtp_Date.Text), "dddd").ToString
            msk_Date.Text = dtp_Date.Text
            msk_Date.SelectionStart = 0
        End If
    End Sub

    Private Sub msk_Date_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles msk_Date.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        If e.KeyValue = 40 Then
            dgv_Details.Focus()
            dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(1)

        End If

        vmskOldText = ""
        vmskSelStrt = -1
        If e.KeyCode = 46 Or e.KeyCode = 8 Then
            vmskOldText = msk_Date.Text
            vmskSelStrt = msk_Date.SelectionStart
        End If

    End Sub

    Private Sub msk_Date_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles msk_Date.KeyPress
        If Asc(e.KeyChar) = 13 Then
            dgv_Details.Focus()
            dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(1)

        End If
        If Trim(UCase(e.KeyChar)) = "D" Then
            msk_Date.Text = Date.Today
            msk_Date.SelectionStart = 0
        End If
    End Sub


    Private Sub msk_Date_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles msk_Date.KeyUp
        Dim vmRetTxt As String = ""
        Dim vmRetSelStrt As Integer = -1

        'If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
        '    msk_Date.Text = Date.Today
        'End If
        If e.KeyCode = 107 Then
            msk_Date.Text = DateAdd("D", 1, Convert.ToDateTime(msk_Date.Text))
        ElseIf e.KeyCode = 109 Then
            msk_Date.Text = DateAdd("D", -1, Convert.ToDateTime(msk_Date.Text))
        End If

        If e.KeyCode = 46 Or e.KeyCode = 8 Then

            Common_Procedures.maskEdit_Date_ON_DelBackSpace(sender, e, vmskOldText, vmskSelStrt)

        End If

    End Sub

   

    Private Sub msk_Date_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles msk_Date.LostFocus

        If IsDate(msk_Date.Text) = True Then
            If Microsoft.VisualBasic.DateAndTime.Day(Convert.ToDateTime(msk_Date.Text)) <= 31 Or Microsoft.VisualBasic.DateAndTime.Month(Convert.ToDateTime(msk_Date.Text)) <= 31 Then
                If Microsoft.VisualBasic.DateAndTime.Year(Convert.ToDateTime(msk_Date.Text)) <= 2050 And Microsoft.VisualBasic.DateAndTime.Year(Convert.ToDateTime(msk_Date.Text)) >= 2000 Then
                    dtp_Date.Value = Convert.ToDateTime(msk_Date.Text)
                End If
            End If

        End If
    End Sub
    Private Sub msk_Date_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles msk_Date.TextChanged
        lbl_Day.Text = ""
        If IsDate(msk_Date.Text) = True Then
            lbl_Day.Text = Format(Convert.ToDateTime(msk_Date.Text), "dddd").ToString
        End If
    End Sub
    Private Sub dtp_Date_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dtp_Date.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            dtp_Date.Text = Date.Today
        End If
    End Sub

End Class