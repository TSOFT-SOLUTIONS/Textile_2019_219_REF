Public Class Payroll_Deduction_On_Salary_CategoryWise
    Implements Interface_MDIActions

    Private con As New SqlClient.SqlConnection(Common_Procedures.Connection_String)
    Private FrmLdSTS As Boolean = False
    Private New_Entry As Boolean = False
    Private Insert_Entry As Boolean = False
    Private Filter_Status As Boolean = False
    Private Pk_Condition As String = "EMPSA-"
    Private Pk_Condition2 As String = "ADVSD-"
    Private Pk_Condition3 As String = "MESSD-"
    Private Pk_Condition4 As String = "DEPAC-"
    Private NoCalc_Status As Boolean = False
    Private Prec_ActCtrl As New Control
    Private vCbo_ItmNm As String
    Private vcbo_KeyDwnVal As Double
    Private dgvDet_CboBx_ColNos_Arr As Integer() = {}
    Private WithEvents dgtxt_Details As New DataGridViewTextBoxEditingControl

    Private prn_HdDt As New DataTable
    Private prn_DetDt As New DataTable
    Private prn_PageNo As Integer
    Private prn_DetIndx As Integer
    Private prn_DetAr(50, 10) As String
    Private prn_DetMxIndx As Integer
    Private prn_NoofBmDets As Integer
    Private prn_DetSNo As Integer

    Private Enum dgvCol_Details As Integer

        SlNO            '0
        EMPLOYEE_NAME   '1
        CODE_DEST       '2
        NO_Of_DAYS      '3
        NO_OF_SHIFT     '4
        NO_Of_LEAVE     '5
        BASIC_SALARY    '6
        GROSS_SALARY    '7
        OT_SALARY       '8
        OT_HOURS        '9
        OT_AMOUNT       '10
        PRODUCT_INC_AMT '11
        INCENTIVE_AMT   '12
        ADVANCE_BALANCE '13
        LESS_ADVANCE    '14
        LESS_MESS       '15

        ESI             '16
        PF              '17

        LESS_DEPOSIT    '18
        LESS_OTHER      '19
        NET_SALARY      '20
        SIGN            '21
        EMP_CODE        '22


    End Enum

    Public Sub New()
        ' This call is required by the designer.
        InitializeComponent()
        ' Add any initialization after the InitializeComponent() call.
        clear()
    End Sub

    Private Sub clear()

        NoCalc_Status = True

        New_Entry = False
        Insert_Entry = False

        pnl_Back.Enabled = True

        lbl_RefNo.Text = ""
        lbl_RefNo.ForeColor = Color.Black

        lbl_Day.Text = ""
        dtp_date.Text = ""
        dtp_date.Tag = dtp_date.Text

        lbl_Day.Text = Trim(Format(dtp_date.Value, "dddddd"))

        cbo_Category.Text = ""
        cbo_Category.Tag = cbo_Category.Text

        cbo_Month.Text = ""
        cbo_Month.Tag = ""

        lbl_Adv_Deduction.Text = ""
        lbl_Mess_Deduction.Text = ""
        lbl_Other_Deduction.Text = ""
        lbl_OT_Salary.Text = ""
        lbl_Tot_Incentive.Text = ""
        lbl_Deposit_Deduction.Text = ""
        lbl_Tot_Salary.Text = ""
        lbl_GrossSalary.Text = ""

        dtp_FromDate.Enabled = True
        dtp_ToDate.Enabled = True
        cbo_Category.Enabled = True
        cbo_Month.Enabled = True

        dgv_Details.Rows.Clear()
        dgv_Details.Rows.Add()


        dgv_Details_Total.Rows.Clear()
        dgv_Details_Total.Rows.Add()

        Grid_Cell_DeSelect()

        'dtp_date.Enabled = False
        'cbo_Month.Enabled = False
        'cbo_Category.Enabled = False
        btn_List_EmployeeDetails.Enabled = True

        NoCalc_Status = False

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



        Grid_Cell_DeSelect()

        Prec_ActCtrl = Me.ActiveControl

    End Sub

    Private Sub ControlLostFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        On Error Resume Next
        If IsNothing(Prec_ActCtrl) = False Then
            If TypeOf Prec_ActCtrl Is Button Then
                Prec_ActCtrl.BackColor = Color.FromArgb(44, 61, 90)
                Prec_ActCtrl.ForeColor = Color.White

            Else
                Prec_ActCtrl.BackColor = Color.White
                Prec_ActCtrl.ForeColor = Color.Black

            End If
        End If
    End Sub

    Private Sub TextBoxControlKeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs)
        On Error Resume Next
        If e.KeyValue = 38 Then e.Handled = True : e.SuppressKeyPress = True : SendKeys.Send("+{TAB}")
        If e.KeyValue = 40 Then e.Handled = True : e.SuppressKeyPress = True : SendKeys.Send("{TAB}")
    End Sub

    Private Sub TextBoxControlKeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
        On Error Resume Next
        If Asc(e.KeyChar) = 13 Then e.Handled = True : SendKeys.Send("{TAB}")
    End Sub

    Private Sub Grid_Cell_DeSelect()
        On Error Resume Next
        If IsNothing(dgv_Details.CurrentCell) Then Exit Sub
        If Not IsNothing(dgv_Details_Total.CurrentCell) Then dgv_Details_Total.CurrentCell.Selected = False
        dgv_Details.CurrentCell.Selected = False

    End Sub

    Private Sub move_record(ByVal no As String)
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim cmd As New SqlClient.SqlCommand
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim NewCode As String
        Dim n As Integer
        Dim SNo As Integer
        Dim dttm1 As DateTime, dttm2 As DateTime
        Dim vCatID As Integer = 0
        Dim vMon_ID As Integer = 0


        If Val(no) = 0 Then Exit Sub

        clear()

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(no) & "/" & Trim(Common_Procedures.FnYearCode)

        Try
            da1 = New SqlClient.SqlDataAdapter("select a.* from PayRoll_Salary_Head a Where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Salary_Code = '" & Trim(NewCode) & "'", con)
            dt1 = New DataTable
            da1.Fill(dt1)

            If dt1.Rows.Count > 0 Then

                lbl_RefNo.Text = dt1.Rows(0).Item("Salary_No").ToString
                dtp_date.Text = dt1.Rows(0).Item("Salary_Date").ToString
                dtp_date.Tag = dtp_date.Text

                cbo_Category.Text = Common_Procedures.Category_IdNoToName(con, dt1.Rows(0).Item("Category_IdNo").ToString)
                cbo_Category.Tag = cbo_Category.Text

                cbo_Month.Text = Common_Procedures.Month_IdNoToName(con, dt1.Rows(0).Item("Month_idno").ToString)
                cbo_Month.Tag = cbo_Month.Text

                dtp_FromDate.Text = dt1.Rows(0).Item("From_Date").ToString
                dtp_ToDate.Text = dt1.Rows(0).Item("To_Date").ToString

                lbl_Adv_Deduction.Text = Format(Val(dt1.Rows(0).Item("Total_Advance_Deduction").ToString), "#########0.00")
                lbl_Mess_Deduction.Text = Format(Val(dt1.Rows(0).Item("Total_Mess_Deduction").ToString), "#########0.00")
                lbl_Other_Deduction.Text = Format(Val(dt1.Rows(0).Item("Total_Other_Deduction").ToString), "#########0.00")
                lbl_OT_Salary.Text = Format(Val(dt1.Rows(0).Item("OT_salary").ToString), "#########0.00")
                lbl_Tot_Incentive.Text = Format(Val(dt1.Rows(0).Item("Total_Incentive_Amt").ToString), "#########0.00")
                lbl_Deposit_Deduction.Text = Format(Val(dt1.Rows(0).Item("Total_Deposit_Deduction").ToString), "#########0.00")

                lbl_GrossSalary.Text = Format(Val(dt1.Rows(0).Item("Gross_salary").ToString), "#########0.00")
                txt_Mess_Amount.Text = Format(Val(dt1.Rows(0).Item("Mess_Amount").ToString), "#########0.00")
                txt_Hdfc_Deposit.Text = Format(Val(dt1.Rows(0).Item("Hdfc_Deposit_Amount").ToString), "#########0.00")
                lbl_Tot_Salary.Text = Format(Val(dt1.Rows(0).Item("Total_salary").ToString), "#########0.00")



                lbl_Day.Text = Trim(Format(dtp_date.Value, "dddddd"))

                dtp_date.Enabled = False
                cbo_Category.Enabled = False
                cbo_Month.Enabled = False
                btn_List_EmployeeDetails.Enabled = True




                da2 = New SqlClient.SqlDataAdapter("Select a.*, b.Employee_Name , b.* from PayRoll_Salary_Details a INNER JOIN PayRoll_Employee_Head b ON a.Employee_IdNo = b.Employee_IdNo  Where a.Salary_Code = '" & Trim(NewCode) & "' Order by a.sl_no", con)
                dt2 = New DataTable
                da2.Fill(dt2)

                With dgv_Details

                    .Rows.Clear()

                    SNo = 0

                    If dt2.Rows.Count > 0 Then

                        For i = 0 To dt2.Rows.Count - 1

                            n = .Rows.Add()

                            SNo = SNo + 1
                            .Rows(n).Cells(dgvCol_Details.SlNO).Value = Val(SNo)
                            .Rows(n).Cells(dgvCol_Details.EMPLOYEE_NAME).Value = dt2.Rows(i).Item("Employee_Name").ToString
                            .Rows(n).Cells(dgvCol_Details.CODE_DEST).Value = dt2.Rows(i).Item("Destination").ToString

                            .Rows(n).Cells(dgvCol_Details.NO_Of_DAYS).Value = Val(dt2.Rows(i).Item("Total_Days").ToString)
                            If Val(.Rows(n).Cells(dgvCol_Details.NO_Of_DAYS).Value) = 0 Then .Rows(n).Cells(dgvCol_Details.NO_Of_DAYS).Value = ""

                            .Rows(n).Cells(dgvCol_Details.NO_OF_SHIFT).Value = dt2.Rows(i).Item("Noof_WorkedShifts_From_Attendance").ToString
                            If Val(.Rows(n).Cells(dgvCol_Details.NO_OF_SHIFT).Value) = 0 Then .Rows(n).Cells(dgvCol_Details.NO_OF_SHIFT).Value = ""

                            .Rows(n).Cells(dgvCol_Details.NO_Of_LEAVE).Value = Format(Val(dt2.Rows(i).Item("No_Of_Leave").ToString), "########0.00")
                            If Val(.Rows(n).Cells(dgvCol_Details.NO_Of_LEAVE).Value) = 0 Then .Rows(n).Cells(dgvCol_Details.NO_Of_LEAVE).Value = ""

                            .Rows(n).Cells(dgvCol_Details.BASIC_SALARY).Value = Format(Val(dt2.Rows(i).Item("Basic_Salary").ToString), "########0.00")
                            If Val(.Rows(n).Cells(dgvCol_Details.BASIC_SALARY).Value) = 0 Then .Rows(n).Cells(dgvCol_Details.BASIC_SALARY).Value = ""

                            .Rows(n).Cells(dgvCol_Details.GROSS_SALARY).Value = Format(Val(dt2.Rows(i).Item("Basic_Pay").ToString), "########0.00")
                            If Val(.Rows(n).Cells(dgvCol_Details.GROSS_SALARY).Value) = 0 Then .Rows(n).Cells(dgvCol_Details.GROSS_SALARY).Value = ""

                            .Rows(n).Cells(dgvCol_Details.OT_SALARY).Value = Format(Val(dt2.Rows(i).Item("Ot_Pay_Hours").ToString), "########0.00")
                            If Val(.Rows(n).Cells(dgvCol_Details.OT_SALARY).Value) = 0 Then .Rows(n).Cells(dgvCol_Details.OT_SALARY).Value = ""

                            .Rows(n).Cells(dgvCol_Details.OT_HOURS).Value = Val(dt2.Rows(i).Item("OT_Hours").ToString)
                            If Val(.Rows(n).Cells(dgvCol_Details.OT_HOURS).Value) = 0 Then .Rows(n).Cells(dgvCol_Details.OT_HOURS).Value = ""

                            .Rows(n).Cells(dgvCol_Details.OT_AMOUNT).Value = Format(Val(dt2.Rows(i).Item("OT_Salary").ToString), "#########0.00")
                            If Val(.Rows(n).Cells(dgvCol_Details.OT_AMOUNT).Value) = 0 Then .Rows(n).Cells(dgvCol_Details.OT_AMOUNT).Value = ""

                            .Rows(n).Cells(dgvCol_Details.PRODUCT_INC_AMT).Value = Format(Val(dt2.Rows(i).Item("Product_Inc_Amount").ToString), "###########0.00")
                            If Val(.Rows(n).Cells(dgvCol_Details.PRODUCT_INC_AMT).Value) = 0 Then .Rows(n).Cells(dgvCol_Details.PRODUCT_INC_AMT).Value = ""

                            .Rows(n).Cells(dgvCol_Details.INCENTIVE_AMT).Value = Format(Val(dt2.Rows(i).Item("Incentive_Amount").ToString), "###########0.00")
                            If Val(.Rows(n).Cells(dgvCol_Details.INCENTIVE_AMT).Value) = 0 Then .Rows(n).Cells(dgvCol_Details.INCENTIVE_AMT).Value = ""

                            .Rows(n).Cells(dgvCol_Details.ADVANCE_BALANCE).Value = Format(Val(dt2.Rows(i).Item("Total_Advance").ToString), "#######0.00")
                            If Val(.Rows(n).Cells(dgvCol_Details.ADVANCE_BALANCE).Value) = 0 Then .Rows(n).Cells(dgvCol_Details.ADVANCE_BALANCE).Value = ""

                            .Rows(n).Cells(dgvCol_Details.LESS_ADVANCE).Value = Format(Val(dt2.Rows(i).Item("Minus_Advance").ToString), "#########0.00")
                            If Val(.Rows(n).Cells(dgvCol_Details.LESS_ADVANCE).Value) = 0 Then .Rows(n).Cells(dgvCol_Details.LESS_ADVANCE).Value = ""

                            .Rows(n).Cells(dgvCol_Details.LESS_MESS).Value = Format(Val(dt2.Rows(i).Item("Mess").ToString), "########0.00")
                            If Val(.Rows(n).Cells(dgvCol_Details.LESS_MESS).Value) = 0 Then .Rows(n).Cells(dgvCol_Details.LESS_MESS).Value = ""

                            .Rows(n).Cells(dgvCol_Details.ESI).Value = Format(Val(dt2.Rows(i).Item("ESI").ToString), "########0.00")
                            If Val(.Rows(n).Cells(dgvCol_Details.ESI).Value) = 0 Then .Rows(n).Cells(dgvCol_Details.ESI).Value = ""

                            .Rows(n).Cells(dgvCol_Details.PF).Value = Format(Val(dt2.Rows(i).Item("P_F").ToString), "########0.00")
                            If Val(.Rows(n).Cells(dgvCol_Details.PF).Value) = 0 Then .Rows(n).Cells(dgvCol_Details.PF).Value = ""

                            .Rows(n).Cells(dgvCol_Details.LESS_DEPOSIT).Value = Format(Val(dt2.Rows(i).Item("Less_Deposit_Amount").ToString), "########0.00")
                            If Val(.Rows(n).Cells(dgvCol_Details.LESS_DEPOSIT).Value) = 0 Then .Rows(n).Cells(dgvCol_Details.LESS_DEPOSIT).Value = ""

                            .Rows(n).Cells(dgvCol_Details.LESS_OTHER).Value = Format(Val(dt2.Rows(i).Item("Other_Deduction").ToString), "#########0.00")
                            If Val(.Rows(n).Cells(dgvCol_Details.LESS_OTHER).Value) = 0 Then .Rows(n).Cells(dgvCol_Details.LESS_OTHER).Value = ""

                            .Rows(n).Cells(dgvCol_Details.NET_SALARY).Value = Format(Val(dt2.Rows(i).Item("Net_Salary").ToString), "#########0.00")
                            If Val(.Rows(n).Cells(dgvCol_Details.NET_SALARY).Value) = 0 Then .Rows(n).Cells(dgvCol_Details.NET_SALARY).Value = ""

                            .Rows(n).Cells(dgvCol_Details.EMP_CODE).Value = dt2.Rows(i).Item("Emp_Code").ToString


                        Next i

                    End If

                End With



                With dgv_Details_Total
                    If .RowCount = 0 Then .Rows.Add()
                    .Rows(0).Cells(4).Value = Val(dt1.Rows(0).Item("Total_Shift").ToString)
                    .Rows(0).Cells(6).Value = Format(Val(dt1.Rows(0).Item("Total_BasicSalary").ToString), "########0.00")
                    .Rows(0).Cells(7).Value = Format(Val(dt1.Rows(0).Item("Gross_salary").ToString), "########0.00")
                    .Rows(0).Cells(9).Value = Format(Val(dt1.Rows(0).Item("OT_salary").ToString), "########0.00")

                    .Rows(0).Cells(11).Value = Format(Val(dt1.Rows(0).Item("Total_Incentive_Amt").ToString), "########0.00")
                    .Rows(0).Cells(12).Value = Format(Val(dt1.Rows(0).Item("Total_Advance_Deduction").ToString), "########0.00")
                    .Rows(0).Cells(14).Value = Format(Val(dt1.Rows(0).Item("Total_Mess_Deduction").ToString), "########0.00")
                    .Rows(0).Cells(15).Value = Format(Val(dt1.Rows(0).Item("Total_Deposit_Deduction").ToString), "########0.00")
                    .Rows(0).Cells(16).Value = Format(Val(dt1.Rows(0).Item("Total_Other_Deduction").ToString), "########0.00")
                    .Rows(0).Cells(17).Value = Format(Val(dt1.Rows(0).Item("Total_Salary").ToString), "########0.00")

                End With

            End If

            Grid_Cell_DeSelect()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT MOVE RECORDS...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
        Finally

            'dt1.Clear()
            dt1.Dispose()
            da1.Dispose()

            'dt2.Clear()

            dt2.Dispose()
            da2.Dispose()

            dtp_FromDate.Enabled = False
            dtp_ToDate.Enabled = False

            If dtp_date.Visible And dtp_date.Enabled Then
                dtp_date.Focus()
            ElseIf dgv_Details.Rows.Count > 0 Then
                dgv_Details.Focus()
                If dgv_Details.Columns(3).Visible = True Then
                    dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(3)
                Else
                    dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(4)
                End If

            Else
                btn_save.Focus()

            End If

        End Try


    End Sub

    Private Sub Employee_Attendance_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated

        Try


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
            'MessageBox.Show(ex.Message, "DOES NOT SHOW...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

        FrmLdSTS = False

    End Sub

    Private Sub Employee_Attendance_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable

        Me.Text = ""

        con.Open()



        AddHandler dtp_date.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_save.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_save.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_close.GotFocus, AddressOf ControlGotFocus

        AddHandler txt_Mess_Amount.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Hdfc_Deposit.GotFocus, AddressOf ControlGotFocus

        AddHandler dtp_date.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_save.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_close.LostFocus, AddressOf ControlLostFocus

        AddHandler txt_Mess_Amount.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Hdfc_Deposit.LostFocus, AddressOf ControlLostFocus

        AddHandler cbo_Category.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Category.LostFocus, AddressOf ControlLostFocus

        AddHandler cbo_Month.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Month.LostFocus, AddressOf ControlLostFocus


        'If Common_Procedures.settings.CustomerCode = "1271" Then
        '    dgv_Details.Columns(3).Visible = False
        '    dgv_Details.Columns(9).Visible = False

        '    dgv_Details.Columns(0).ReadOnly = True
        '    dgv_Details.Columns(1).ReadOnly = True
        '    dgv_Details.Columns(2).ReadOnly = True

        '    dgv_Details.Columns(4).HeaderText = "SNACK"
        '    dgv_Details.Columns(5).HeaderText = "INSURANCE"
        '    dgv_Details.Columns(6).HeaderText = "MISC"

        '    dgv_Details.Columns(5).Visible = True

        '    dgv_Details.Columns(0).ReadOnly = True
        '    dgv_Details.Columns(1).ReadOnly = True
        '    dgv_Details.Columns(2).ReadOnly = True

        '    dgv_Details.Columns(1).Width = dgv_Details.Columns(1).Width + 25


        'End If

        'If Common_Procedures.settings.CustomerCode = "1347" Then
        '    dgv_Details.Columns(8).HeaderText = "EB"
        '    dgv_Details.Columns(3).Visible = False
        '    dgv_Details.Columns(5).Visible = False
        '    dgv_Details.Columns(6).Visible = False
        '    dgv_Details.Columns(9).Visible = False

        'End If

        'If Common_Procedures.settings.CustomerCode = "1234" Then

        '    dgv_Details.Columns(3).Visible = False
        '    dgv_Details.Columns(5).Visible = False
        '    dgv_Details.Columns(8).Visible = False
        '    dgv_Details.Columns(9).Visible = False

        'End If

        'If Common_Procedures.settings.CustomerCode = "1186" Then
        '    dgv_Details.Columns(5).ReadOnly = True
        'End If


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
                Close_Form()

            End If



        Catch ex As Exception
            'MessageBox.Show(ex.Message, "DOES NOT CLOSE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

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
            MessageBox.Show(ex.Message, "DOES NOT CLOSE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Protected Overrides Function ProcessCmdKey(ByRef msg As System.Windows.Forms.Message, ByVal keyData As System.Windows.Forms.Keys) As Boolean


        Dim I As Integer = 0
        Dim dgv1 As New DataGridView

        If ActiveControl.Name = dgv_Details.Name Or ActiveControl.Name = dgv_Details.Name Or TypeOf ActiveControl Is DataGridViewTextBoxEditingControl Then

            'On Error Resume Next

            dgv1 = Nothing

            If ActiveControl.Name = dgv_Details.Name Then
                dgv1 = dgv_Details


            ElseIf dgv_Details.IsCurrentRowDirty = True Then
                dgv1 = dgv_Details



            ElseIf Pnl_Back.Enabled = True Then
                dgv1 = dgv_Details

            End If

            If IsNothing(dgv1) = False Then

                With dgv1

                    If keyData = Keys.Enter Or keyData = Keys.Down Or keyData = Keys.Up Then

                        Common_Procedures.DGVGrid_Cursor_Focus(dgv1, keyData, dtp_ToDate, txt_Mess_Amount, dgvDet_CboBx_ColNos_Arr, dgtxt_Details, dtp_date)

                        Return True

                    Else
                        Return MyBase.ProcessCmdKey(msg, keyData)

                    End If


                End With

            Else

                Return MyBase.ProcessCmdKey(msg, keyData)

            End If

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

        'If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.Employee_Attendance_Missing_Time_Addition, "~L~") = 0 And InStr(Common_Procedures.UR.Employee_Attendance_Missing_Time_Addition, "~D~") = 0 Then MessageBox.Show("You have No Rights to Delete", "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error) : Exit Sub

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)
        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.DeletingEntry, Common_Procedures.UR.PAYROLL_ENTRY_EMPLOYEE_SALARY, New_Entry, Me, con, "PayRoll_Salary_Head", "Salary_Code", NewCode, "Salary_Date", "(Salary_Code = '" & Trim(NewCode) & "')") = False Then Exit Sub


        If Val(lbl_Company.Tag) = 0 Then
            MessageBox.Show("Invalid Company Selection", "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
        End If

        If Pnl_Back.Enabled = False Then
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
            'NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(Common_Procedures.FnYearCode)

            cmd.Connection = con
            cmd.Transaction = trans


            'Common_Procedures.Voucher_Deletion(con, Val(lbl_Company.Tag), Trim(NewCode), trans)

            cmd.CommandText = "Delete from Voucher_Head Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Voucher_Code LIKE '" & Trim(Pk_Condition) & "%/" & Trim(NewCode) & "' and Entry_Identification LIKE '" & Trim(Pk_Condition) & "%/" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()
            cmd.CommandText = "Delete from Voucher_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Voucher_Code LIKE '" & Trim(Pk_Condition) & "%/" & Trim(NewCode) & "' and Entry_Identification LIKE '" & Trim(Pk_Condition) & "%/" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Voucher_Head Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Voucher_Code LIKE '" & Trim(Pk_Condition2) & "%/" & Trim(NewCode) & "' and Entry_Identification LIKE '" & Trim(Pk_Condition2) & "%/" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()
            cmd.CommandText = "Delete from Voucher_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Voucher_Code LIKE '" & Trim(Pk_Condition2) & "%/" & Trim(NewCode) & "' and Entry_Identification LIKE '" & Trim(Pk_Condition2) & "%/" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Voucher_Head Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Voucher_Code LIKE '" & Trim(Pk_Condition3) & "%/" & Trim(NewCode) & "' and Entry_Identification LIKE '" & Trim(Pk_Condition3) & "%/" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()
            cmd.CommandText = "Delete from Voucher_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Voucher_Code LIKE '" & Trim(Pk_Condition3) & "%/" & Trim(NewCode) & "' and Entry_Identification LIKE '" & Trim(Pk_Condition3) & "%/" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Voucher_Head Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Voucher_Code LIKE '" & Trim(Pk_Condition4) & "%/" & Trim(NewCode) & "' and Entry_Identification LIKE '" & Trim(Pk_Condition4) & "%/" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()
            cmd.CommandText = "Delete from Voucher_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Voucher_Code LIKE '" & Trim(Pk_Condition4) & "%/" & Trim(NewCode) & "' and Entry_Identification LIKE '" & Trim(Pk_Condition4) & "%/" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()



            cmd.CommandText = "delete from PayRoll_Salary_Details where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Salary_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()
            cmd.CommandText = "delete from PayRoll_Salary_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Salary_Code = '" & Trim(NewCode) & "'"
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

            If dtp_date.Enabled = True And dtp_date.Visible = True Then dtp_date.Focus()

        End Try

    End Sub

    Public Sub filter_record() Implements Interface_MDIActions.filter_record


    End Sub

    Public Sub movefirst_record() Implements Interface_MDIActions.movefirst_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim movno As String

        Try

            da = New SqlClient.SqlDataAdapter("select top 1 Salary_No from PayRoll_Salary_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Salary_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby, Salary_No", con)
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
            MessageBox.Show(ex.Message, "FOR  MOVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Public Sub movenext_record() Implements Interface_MDIActions.movenext_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim movno As String = ""
        Dim OrdByNo As Single = 0

        Try

            OrdByNo = Common_Procedures.OrderBy_CodeToValue(Trim(lbl_RefNo.Text))

            da = New SqlClient.SqlDataAdapter("select top 1 Salary_No from PayRoll_Salary_Head where for_orderby > " & Str(Val(OrdByNo)) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and Salary_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby, Salary_No", con)
            da.Fill(dt)

            movno = ""
            If dt.Rows.Count > 0 Then
                If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                    movno = dt.Rows(0)(0).ToString
                End If
            End If

            If Val(movno) <> 0 Then move_record(movno)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR  MOVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Public Sub moveprevious_record() Implements Interface_MDIActions.moveprevious_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim movno As String = ""
        Dim OrdByNo As Single = 0

        Try

            OrdByNo = Common_Procedures.OrderBy_CodeToValue(Trim(lbl_RefNo.Text))

            da = New SqlClient.SqlDataAdapter("select top 1 Salary_No from PayRoll_Salary_Head where for_orderby < " & Str(Val(OrdByNo)) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and Salary_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, Salary_No desc", con)
            da.Fill(dt)

            movno = ""
            If dt.Rows.Count > 0 Then
                If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                    movno = dt.Rows(0)(0).ToString
                End If
            End If

            If Val(movno) <> 0 Then move_record(movno)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR  MOVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try
    End Sub

    Public Sub movelast_record() Implements Interface_MDIActions.movelast_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim movno As String = ""

        Try
            da = New SqlClient.SqlDataAdapter("select top 1 Salary_No from PayRoll_Salary_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Salary_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, Salary_No desc", con)
            da.Fill(dt)

            movno = ""
            If dt.Rows.Count > 0 Then
                If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                    movno = dt.Rows(0)(0).ToString
                End If
            End If

            If Val(movno) <> 0 Then move_record(movno)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR  MOVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Public Sub new_record() Implements Interface_MDIActions.new_record
        Dim Da1 As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim Dt2 As New DataTable

        Try

            clear()

            New_Entry = True

            lbl_RefNo.Text = Common_Procedures.get_MaxCode(con, "PayRoll_Salary_Head", "Salary_Code", "For_OrderBy", "", Val(lbl_Company.Tag), Common_Procedures.FnYearCode)
            lbl_RefNo.ForeColor = Color.Red


        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR NEW RECORD...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Finally

            Dt1.Dispose()
            Dt2.Dispose()
            Da1.Dispose()

            If dtp_date.Enabled And dtp_date.Visible Then
                dtp_date.Focus()
            Else
                cbo_Month.Focus()
            End If

        End Try

    End Sub

    Public Sub open_record() Implements Interface_MDIActions.open_record
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim movno As String, inpno As String
        Dim RecCode As String

        Try

            inpno = InputBox("Enter Ref.No.", "FOR FINDING...")

            RecCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

            Da = New SqlClient.SqlDataAdapter("select Salary_No from PayRoll_Salary_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Salary_Code = '" & Trim(RecCode) & "'", con)
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
                MessageBox.Show("Ref No. does not exists", "DOES NOT FIND...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT FIND...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try
    End Sub

    Public Sub insert_record() Implements Interface_MDIActions.insert_record
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim movno As String, inpno As String
        Dim InvCode As String

        'If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.Pavu_Delivery_Entry, "~L~") = 0 And InStr(Common_Procedures.UR.Pavu_Delivery_Entry, "~I~") = 0 Then MessageBox.Show("You have No Rights to Insert", "DOES NOT INSERT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error) : Exit Sub
        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.InsertingEntry, Common_Procedures.UR.PAYROLL_ENTRY_EMPLOYEE_SALARY, New_Entry, Me) = False Then Exit Sub

        Try

            inpno = InputBox("Enter New Ref No.", "FOR NEW Ref NO. INSERTION...")

            InvCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

            Da = New SqlClient.SqlDataAdapter("select Timing_Addition_No from Payroll_Timing_Addition_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Timing_Addition_Code = '" & Trim(InvCode) & "'", con)
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
                    MessageBox.Show("Invalid Ref No.", "DOES NOT INSERT NEW Ref...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

                Else
                    new_record()
                    Insert_Entry = True
                    lbl_RefNo.Text = Trim(UCase(inpno))

                End If

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT INSERT NEW BILL...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

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
        Dim Delv_ID As Integer = 0
        Dim Rec_ID As Integer = 0
        Dim Trans_ID As Integer = 0
        Dim Clo_ID As Integer = 0
        Dim EdsCnt_ID As Integer = 0
        Dim Sno As Integer = 0
        Dim Partcls As String = ""
        Dim PBlNo As String = ""
        Dim Emp_id As Integer = 0
        Dim Cat_id As Integer = 0
        Dim EntID As String = ""
        Dim Usr_ID As Integer = 0
        Dim OurOrd_No As String = ""
        Dim dttm1 As DateTime, dttm2 As DateTime
        Dim vMonth_Id As Integer = 0
        Dim vMovNo As String = ""
        Dim vOrdBy As String = 0

        If Val(lbl_Company.Tag) = 0 Then
            MessageBox.Show("Invalid Company Selection", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
        End If

        ' If Common_Procedures.UserRight_Check(Common_Procedures.UR.Deduction_On_Salary_Entry, New_Entry) = False Then Exit Sub
        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)
        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.SavingEntry, Common_Procedures.UR.PAYROLL_ENTRY_EMPLOYEE_SALARY, New_Entry, Me, con, "PayRoll_Salary_Head", "Salary_Code", NewCode, "Salary_Date", "(Salary_Code = '" & Trim(NewCode) & "')", "(Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Salary_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "')", "for_Orderby desc, Salary_No desc", dtp_date.Value.Date) = False Then Exit Sub


        If Pnl_Back.Enabled = False Then
            MessageBox.Show("Close Other Windows", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
        End If

        vMonth_Id = Common_Procedures.Month_NameToIdNo(con, cbo_Month.Text)
        If Common_Procedures.settings.CustomerCode = "1271" Then

            If Val(vMonth_Id) = 0 Then
                MessageBox.Show("Invalid Month Name", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                cbo_Month.Focus()
                Exit Sub
            End If

            dttm1 = New DateTime(Year(dtp_date.Value.Date), vMonth_Id, 1)
            dttm2 = DateAdd(DateInterval.Month, 1, dttm1)
            dttm2 = DateAdd(DateInterval.Day, -1, dttm1)
            dttm2 = DateAdd(DateInterval.Month, 1, dttm2)

            dtp_date.Text = dttm2.Date

        End If


        If IsDate(dtp_date.Text) = False Then
            MessageBox.Show("Invalid Date", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If dtp_date.Enabled And dtp_date.Visible Then dtp_date.Focus()
            Exit Sub
        End If

        If Not (dtp_date.Value.Date >= Common_Procedures.Company_FromDate And dtp_date.Value.Date <= Common_Procedures.Company_ToDate) Then
            MessageBox.Show("Date is out of financial range", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If dtp_date.Enabled And dtp_date.Visible Then dtp_date.Focus()
            Exit Sub
        End If

        Cat_id = Common_Procedures.Category_NameToIdNo(con, cbo_Category.Text)
        If Common_Procedures.settings.CustomerCode = "1271" Then
            If Val(Cat_id) = 0 Then
                MessageBox.Show("Invalid Category Name", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                cbo_Category.Focus()
                Exit Sub
            End If
        End If

        With dgv_Details
            For i = 0 To .RowCount - 1
                If Val(.Rows(i).Cells(3).Value) <> 0 Or Val(.Rows(i).Cells(4).Value) <> 0 Or Val(.Rows(i).Cells(5).Value) <> 0 Or Val(.Rows(i).Cells(6).Value) <> 0 Or Val(.Rows(i).Cells(7).Value) <> 0 Or Val(.Rows(i).Cells(8).Value) <> 0 Or Val(.Rows(i).Cells(9).Value) <> 0 Then
                    Emp_id = Common_Procedures.Employee_NameToIdNo(con, .Rows(i).Cells(1).Value)
                    If Emp_id = 0 Then
                        MessageBox.Show("Invalid Employee Name", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                        If .Enabled And .Visible Then
                            .Focus()
                            .CurrentCell = .Rows(i).Cells(1)
                        End If
                        Exit Sub
                    End If
                End If
            Next
        End With


        If Val(vMonth_Id) <> 0 Then

            NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            Da = New SqlClient.SqlDataAdapter("Select Salary_No from PayRoll_Salary_Head Where Company_idno = " & Str(Val(lbl_Company.Tag)) & " and Month_Idno = " & Str(Val(vMonth_Id)) & " and Category_IdNo = " & Str(Val(Cat_id)) & " and Salary_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "' and Salary_Code <> '" & Trim(NewCode) & "'", con)
            Dt1 = New DataTable
            Da.Fill(Dt1)
            vMovNo = ""
            If Dt1.Rows.Count > 0 Then
                If IsDBNull(Dt1.Rows(0)(0).ToString) = False Then
                    vMovNo = Trim(Dt1.Rows(0)(0).ToString)
                End If
            End If
            Dt1.Clear()

            If Val(vMovNo) <> 0 Then
                MessageBox.Show("Duplicate entry for this category in this month - Ref.No : " & vMovNo, "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                cbo_Month.Focus()
                Exit Sub
            End If

        End If

        Dim vTotshift As String

        Dim vBasicSal As String
        Dim TotGrsAmt As String, OtAmt As String, IncAmt As String, Totsal As String

        vTotshift = 0 : TotGrsAmt = 0 : OtAmt = 0 : IncAmt = 0 : Totsal = 0 : vBasicSal = 0
        If dgv_Details_Total.RowCount > 0 Then
            vTotshift = Val(dgv_Details_Total.Rows(0).Cells(4).Value())
            TotGrsAmt = Val(dgv_Details_Total.Rows(0).Cells(7).Value())
            OtAmt = Val(dgv_Details_Total.Rows(0).Cells(10).Value())
            IncAmt = Val(dgv_Details_Total.Rows(0).Cells(12).Value())
            Totsal = Val(dgv_Details_Total.Rows(0).Cells(18).Value())
            vBasicSal = Val(dgv_Details_Total.Rows(0).Cells(6).Value())
        End If

        tr = con.BeginTransaction

        Try

            If Insert_Entry = True Or New_Entry = False Then
                NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            Else

                lbl_RefNo.Text = Common_Procedures.get_MaxCode(con, "PayRoll_Salary_Head", "Salary_Code", "For_OrderBy", "", Val(lbl_Company.Tag), Common_Procedures.FnYearCode, tr)

                NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            End If

            cmd.Connection = con
            cmd.Transaction = tr



            cmd.Parameters.Clear()
            cmd.Parameters.AddWithValue("@EntryDate", Convert.ToDateTime(dtp_date.Text))

            cmd.Parameters.AddWithValue("@FromDate", dtp_FromDate.Value.Date)

            cmd.Parameters.AddWithValue("@ToDate", dtp_ToDate.Value.Date)

            vOrdBy = Val(Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text))

            If New_Entry = True Then

                cmd.CommandText = "Insert into PayRoll_Salary_Head (     Salary_Code        ,               Company_IdNo       ,           Salary_No           ,                               for_OrderBy                              ,   Salary_Date ,                 Category_IdNo     ,         Month_IdNo   ,  From_Date,  To_Date   ,     Total_Advance_Deduction ,               Total_Mess_Deduction ,                 Total_Other_Deduction ,                    Gross_salary ,                         OT_salary  ,                  Total_Incentive_Amt ,             Total_Deposit_Deduction ,                  Mess_Amount  ,                          Hdfc_Deposit_Amount ,                   Total_Salary         ,      Total_Shift               ,   Total_BasicSalary) " &
                                    "          Values              ( '" & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_RefNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text))) & ",  @EntryDate  ,     " & Str(Val(Cat_id)) & ", " & Val(vMonth_Id) & ",        @FromDate , @ToDate  ,   " & Val(lbl_Adv_Deduction.Text) & ",   " & Val(lbl_Mess_Deduction.Text) & ",  " & Val(lbl_Other_Deduction.Text) & ",  " & Val(lbl_GrossSalary.Text) & ",  " & Val(lbl_OT_Salary.Text) & ",  " & Val(lbl_Tot_Incentive.Text) & ",   " & Val(lbl_Deposit_Deduction.Text) & ",   " & Val(txt_Mess_Amount.Text) & ",  " & Val(txt_Hdfc_Deposit.Text) & ", " & Val(lbl_Tot_Salary.Text) & ", " & Str(Val(vTotshift)) & " ,   " & Str(Val(vBasicSal)) & " ) "
                cmd.ExecuteNonQuery()

            Else

                cmd.CommandText = "Update PayRoll_Salary_Head set Salary_Date = @EntryDate, Category_IdNo = " & Str(Val(Cat_id)) & ", Month_IdNo = " & Val(vMonth_Id) & ", From_Date = @FromDate, To_Date =  @ToDate , Total_Advance_Deduction =  " & Val(lbl_Adv_Deduction.Text) & " , Total_Mess_Deduction = " & Val(lbl_Mess_Deduction.Text) & " ,  Total_Other_Deduction = " & Val(lbl_Other_Deduction.Text) & ",    Gross_salary = " & Val(lbl_GrossSalary.Text) & ",    OT_salary = " & Val(lbl_OT_Salary.Text) & ", Total_Incentive_Amt = " & Val(lbl_Tot_Incentive.Text) & ",  Total_Deposit_Deduction =  " & Val(lbl_Deposit_Deduction.Text) & " ,  Mess_Amount = " & Val(txt_Mess_Amount.Text) & ",   Hdfc_Deposit_Amount =  " & Val(txt_Hdfc_Deposit.Text) & "  ,  Total_Salary = " & Val(lbl_Tot_Salary.Text) & " ,  Total_Shift = " & Str(Val(vTotshift)) & " , Total_BasicSalary = " & Str(Val(vBasicSal)) & "  Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Salary_Code = '" & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()

            End If

            cmd.CommandText = "Delete from Voucher_Head Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Voucher_Code LIKE '" & Trim(Pk_Condition) & "%/" & Trim(NewCode) & "' and Entry_Identification LIKE '" & Trim(Pk_Condition) & "%/" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()
            cmd.CommandText = "Delete from Voucher_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Voucher_Code LIKE '" & Trim(Pk_Condition) & "%/" & Trim(NewCode) & "' and Entry_Identification LIKE '" & Trim(Pk_Condition) & "%/" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Voucher_Head Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Voucher_Code LIKE '" & Trim(Pk_Condition2) & "%/" & Trim(NewCode) & "' and Entry_Identification LIKE '" & Trim(Pk_Condition2) & "%/" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()
            cmd.CommandText = "Delete from Voucher_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Voucher_Code LIKE '" & Trim(Pk_Condition2) & "%/" & Trim(NewCode) & "' and Entry_Identification LIKE '" & Trim(Pk_Condition2) & "%/" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Voucher_Head Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Voucher_Code LIKE '" & Trim(Pk_Condition3) & "%/" & Trim(NewCode) & "' and Entry_Identification LIKE '" & Trim(Pk_Condition3) & "%/" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()
            cmd.CommandText = "Delete from Voucher_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Voucher_Code LIKE '" & Trim(Pk_Condition3) & "%/" & Trim(NewCode) & "' and Entry_Identification LIKE '" & Trim(Pk_Condition3) & "%/" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Voucher_Head Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Voucher_Code LIKE '" & Trim(Pk_Condition4) & "%/" & Trim(NewCode) & "' and Entry_Identification LIKE '" & Trim(Pk_Condition4) & "%/" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()
            cmd.CommandText = "Delete from Voucher_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Voucher_Code LIKE '" & Trim(Pk_Condition4) & "%/" & Trim(NewCode) & "' and Entry_Identification LIKE '" & Trim(Pk_Condition4) & "%/" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from PayRoll_Salary_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Salary_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            With dgv_Details

                Sno = 0

                For i = 0 To .RowCount - 1

                    Emp_id = Common_Procedures.Employee_NameToIdNo(con, .Rows(i).Cells(1).Value, tr)

                    If Val(Emp_id) <> 0 Then


                        'cmd.Parameters.Clear()
                        'cmd.Parameters.AddWithValue("@EntryDate", dtp_date.Value.Date)


                        Sno = Sno + 1

                        cmd.CommandText = "Insert into PayRoll_Salary_Details (        Salary_Code     ,               Company_IdNo       ,            Salary_No          ,                               for_OrderBy                              ,   Salary_Date,            Sl_No     ,        Employee_IdNo    ,                      Destination                                 ,                        Total_Days ,                                                    Noof_WorkedShifts_From_Attendance           ,                         No_Of_Leave                      ,                    Basic_Salary                                    ,                         Basic_Pay                                        ,                                      Ot_Pay_Hours             ,                                            OT_Hours                   ,                             OT_Salary                         ,                                    Product_Inc_Amount                 ,                           Incentive_Amount                    ,                                           Total_Advance               ,                                     Minus_Advance                             ,                                 Mess                          ,                       ESI                                     ,                                                     P_F                        ,                             Less_Deposit_Amount                        ,                             Other_Deduction                           ,                           Net_Salary                      ,                                                                                  Emp_Code ) " &
                                          "            Values                 ( '" & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_RefNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text))) & ",  @EntryDate , " & Str(Val(Sno)) & ", " & Str(Val(Emp_id)) & ",    '" & Trim(.Rows(i).Cells(dgvCol_Details.CODE_DEST).Value) & "', " & Str(Val(.Rows(i).Cells(dgvCol_Details.NO_Of_DAYS).Value)) & ",  " & Str(Val(.Rows(i).Cells(dgvCol_Details.NO_OF_SHIFT).Value)) & ",  " & Str(Val(.Rows(i).Cells(dgvCol_Details.NO_Of_LEAVE).Value)) & "  ,  " & Str(Val(.Rows(i).Cells(dgvCol_Details.BASIC_SALARY).Value)) & ", " & Str(Val(.Rows(i).Cells(dgvCol_Details.GROSS_SALARY).Value)) & ",  " & Str(Val(.Rows(i).Cells(dgvCol_Details.OT_SALARY).Value)) & ",     " & Str(Val(.Rows(i).Cells(dgvCol_Details.OT_HOURS).Value)) & ",   " & Str(Val(.Rows(i).Cells(dgvCol_Details.OT_AMOUNT).Value)) & ", " & Str(Val(.Rows(i).Cells(dgvCol_Details.PRODUCT_INC_AMT).Value)) & ",  " & Str(Val(.Rows(i).Cells(dgvCol_Details.INCENTIVE_AMT).Value)) & ",  " & Str(Val(.Rows(i).Cells(dgvCol_Details.ADVANCE_BALANCE).Value)) & ",  " & Str(Val(.Rows(i).Cells(dgvCol_Details.LESS_ADVANCE).Value)) & ", " & Str(Val(.Rows(i).Cells(dgvCol_Details.LESS_MESS).Value)) & "  ,    " & Str(Val(.Rows(i).Cells(dgvCol_Details.ESI).Value)) & "     ,         " & Str(Val(.Rows(i).Cells(dgvCol_Details.PF).Value)) & " ,        " & Str(Val(.Rows(i).Cells(dgvCol_Details.LESS_DEPOSIT).Value)) & "  ,   " & Str(Val(.Rows(i).Cells(dgvCol_Details.LESS_OTHER).Value)) & ",          " & Str(Val(.Rows(i).Cells(dgvCol_Details.NET_SALARY).Value)) & "     ,            '" & Trim(.Rows(i).Cells(dgvCol_Details.EMP_CODE).Value) & "' ) "
                        cmd.ExecuteNonQuery()


                    End If

                    '----------------A/c Posting
                    Dim Sal_Amt As String = ""
                    Dim Mess_Deduction_Amt As String = ""
                    Dim Advance_Deduction_amt As String = ""
                    Dim Deposit_deduction_amt As String = ""

                    Dim led_Id As Integer = 0


                    'Sal_Amt = 0
                    'Mess_Deduction_Amt = 0
                    'Advance_Deduction_amt = 0
                    'Deposit_deduction_amt = 0

                    Sal_Amt = Format(Val(.Rows(i).Cells(dgvCol_Details.NET_SALARY).Value), "##########0")

                    Mess_Deduction_Amt = Format(Val(.Rows(i).Cells(dgvCol_Details.LESS_MESS).Value), "##########0")

                    Advance_Deduction_amt = Format(Val(.Rows(i).Cells(dgvCol_Details.LESS_ADVANCE).Value), "##########0")

                    Deposit_deduction_amt = Format((Val(.Rows(i).Cells(dgvCol_Details.LESS_DEPOSIT).Value) / 2) + Val(.Rows(i).Cells(dgvCol_Details.LESS_DEPOSIT).Value), "##########0")


                    Dim vLed_IdNos As String = "", vVou_Amts As String = "", ErrMsg As String = ""
                    Dim VouNarr As String = ""

                    vLed_IdNos = Emp_id & "|" & Common_Procedures.CommonLedger.Salary_Ac


                    VouNarr = " Ref no : " & Trim(lbl_RefNo.Text) & " Month : " & cbo_Month.Text

                    vVou_Amts = Format(Math.Abs(Val(Sal_Amt)), "#########0.00") & "|" & Format(-1 * Math.Abs(Val(Sal_Amt)), "#########0.00")

                    '  If Common_Procedures.Voucher_Updation(con, "Emp.Sal", Val(lbl_Company.Tag), Trim(NewCode), Trim(lbl_RefNo.Text), dtp_date.Value.Date, "Ref no : " & Trim(lbl_RefNo.Text), vLed_IdNos, vVou_Amts, ErrMsg, tr) = False Then
                    If Common_Procedures.Voucher_Updation(con, "Emp.Sal", Val(lbl_Company.Tag), Trim(Pk_Condition) & Trim(Val(Emp_id)) & "/" & Trim(NewCode), Trim(lbl_RefNo.Text), dtp_date.Value.Date, VouNarr, vLed_IdNos, vVou_Amts, ErrMsg, tr) = False Then
                        Throw New ApplicationException(ErrMsg)
                        Exit Sub
                    End If


                    'advance

                    'vLed_IdNos = Emp_id & "|" & Common_Procedures.CommonLedger.Advance_Deduction

                    'vVou_Amts = Format(-1 * Math.Abs(Val(Advance_Deduction_amt)), "#########0.00") & "|" & Format(Math.Abs(Val(Advance_Deduction_amt)), "#########0.00")

                    'If Common_Procedures.Voucher_Updation(con, "Adv.Dedctn", Val(lbl_Company.Tag), Trim(Pk_Condition2) & Trim(Val(Emp_id)) & "/" & Trim(NewCode), Trim(lbl_RefNo.Text), dtp_date.Value.Date, VouNarr, vLed_IdNos, vVou_Amts, ErrMsg, tr) = False Then
                    '    Throw New ApplicationException(ErrMsg)
                    '    Exit Sub
                    'End If


                    vLed_IdNos = Emp_id & "|" & Common_Procedures.CommonLedger.Advance_Deduction

                    vVou_Amts = Format(Math.Abs(Val(Advance_Deduction_amt)), "#########0.00") & "|" & Format(-1 * Math.Abs(Val(Advance_Deduction_amt)), "#########0.00")

                    If Common_Procedures.Voucher_Updation(con, "Adv.Dedctn", Val(lbl_Company.Tag), Trim(Pk_Condition2) & Trim(Val(Emp_id)) & "/" & Trim(NewCode), Trim(lbl_RefNo.Text), dtp_date.Value.Date, VouNarr, vLed_IdNos, vVou_Amts, ErrMsg, tr) = False Then
                        Throw New ApplicationException(ErrMsg)
                        Exit Sub
                    End If


                    'mess

                    vLed_IdNos = Emp_id & "|" & Common_Procedures.CommonLedger.Canteen

                    vVou_Amts = Format(-1 * Math.Abs(Val(Mess_Deduction_Amt)), "#########0.00") & "|" & Format(Math.Abs(Val(Mess_Deduction_Amt)), "#########0.00")

                    If Common_Procedures.Voucher_Updation(con, "Mess.Dedctn", Val(lbl_Company.Tag), Trim(Pk_Condition3) & Trim(Val(Emp_id)) & "/" & Trim(NewCode), Trim(lbl_RefNo.Text), dtp_date.Value.Date, VouNarr, vLed_IdNos, vVou_Amts, ErrMsg, tr) = False Then
                        Throw New ApplicationException(ErrMsg)
                        Exit Sub
                    End If


                    'Deposit

                    vLed_IdNos = Emp_id & "|" & Common_Procedures.CommonLedger.Deposit_ac

                    ' vVou_Amts = Format(-1 * Math.Abs(Val(Deposit_deduction_amt)), "#########0.00") & "|" & Format(Math.Abs(Val(Deposit_deduction_amt)), "#########0.00")
                    vVou_Amts = Format(Math.Abs(Val(Deposit_deduction_amt)), "#########0.00") & "|" & Format(-1 * Math.Abs(Val(Deposit_deduction_amt)), "#########0.00")


                    If Common_Procedures.Voucher_Updation(con, "Deposit Ac", Val(lbl_Company.Tag), Trim(Pk_Condition4) & Trim(Val(Emp_id)) & "/" & Trim(NewCode), Trim(lbl_RefNo.Text), dtp_date.Value.Date, VouNarr, vLed_IdNos, vVou_Amts, ErrMsg, tr) = False Then
                        ' If Common_Procedures.Voucher_Updation(con, "Deposit Ac", Val(lbl_Company.Tag), Trim(Pk_Condition4) & Trim(Val(Emp_id)) & "/" & Trim(NewCode), Trim(lbl_RefNo.Text), dtp_date.Value.Date, "Ref no : " & Trim(lbl_RefNo.Text) & Trim(cbo_Month.Text), vLed_IdNos, vVou_Amts, ErrMsg, tr) = False Then
                        Throw New ApplicationException(ErrMsg)
                        Exit Sub
                    End If


                Next

            End With





            tr.Commit()

            MessageBox.Show("Saved Sucessfully!!!", "FOR SAVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Information)

            If Val(Common_Procedures.settings.OnSave_MoveTo_NewEntry_Status) = 1 Then
                If New_Entry = True Then
                    new_record()
                Else
                    move_record(lbl_RefNo.Text)
                End If

            Else
                move_record(lbl_RefNo.Text)

            End If


        Catch ex As Exception
            tr.Rollback()
            MessageBox.Show(ex.Message, "FOR SAVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Finally
            If dtp_date.Enabled And dtp_date.Visible Then dtp_date.Focus()

        End Try

    End Sub

    Private Sub dgv_Details_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_Details.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
    End Sub

    Private Sub dgv_Details_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgv_Details.LostFocus
        On Error Resume Next
        dgv_Details.CurrentCell.Selected = False
    End Sub

    Private Sub dgv_Details_RowsAdded(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewRowsAddedEventArgs) Handles dgv_Details.RowsAdded
        Dim n As Integer = 0
        Try
            With dgv_Details
                n = .RowCount
                .Rows(n - 1).Cells(0).Value = Val(n)
            End With

        Catch ex As Exception
            '----

        End Try
    End Sub

    Private Sub btn_save_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_save.Click
        save_record()
    End Sub

    Private Sub btn_close_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_close.Click
        Me.Close()
    End Sub

    Public Sub print_record() Implements Interface_MDIActions.print_record
        '-------
    End Sub

    Public Sub get_DateDetails()
        Dim Cmd As New SqlClient.SqlCommand
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim movno As String = ""

        Try

            Cmd.Connection = con

            Cmd.Parameters.Clear()
            Cmd.Parameters.AddWithValue("@EntryDate", dtp_date.Value.Date)

            Cmd.CommandText = "select Timing_Addition_No from Payroll_Timing_Addition_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Timing_Addition_Date = @EntryDate"
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
                new_record()
            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT INSERT NEW DATE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Finally
            Dt.Dispose()
            Da.Dispose()

        End Try

    End Sub

    Private Sub dtp_Date_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles dtp_date.GotFocus
        dtp_date.Tag = dtp_date.Text
    End Sub

    Private Sub dtp_Date_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dtp_date.KeyDown
        Try

            If e.KeyValue = 40 Then


                If cbo_Month.Visible And cbo_Month.Enabled Then
                    cbo_Month.Focus()

                ElseIf cbo_Category.Visible And cbo_Category.Enabled = True Then
                    cbo_Category.Focus()

                ElseIf dgv_Details.Rows.Count > 0 Then

                    dgv_Details.Focus()
                    If dgv_Details.Columns(3).Visible = True Then
                        dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(3)
                    Else
                        dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(4)
                    End If

                Else

                    btn_save.Focus()
                    'If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                    '    save_record()
                    'Else
                    '    dtp_date.Focus()
                    'End If

                End If


            ElseIf e.KeyValue = 38 Then

                If dgv_Details.Rows.Count > 0 Then

                    dgv_Details.Focus()
                    If dgv_Details.Columns(3).Visible = True Then
                        dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(3)
                    Else
                        dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(4)
                    End If

                ElseIf cbo_Category.Visible And cbo_Category.Enabled = True Then
                    cbo_Category.Focus()


                Else

                    btn_save.Focus()

                End If

            End If

        Catch ex As Exception

            '-------

        End Try

    End Sub

    Private Sub dtp_Date_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dtp_date.KeyPress
        Dim Cmd As New SqlClient.SqlCommand
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim movno As String = ""
        Dim NewCode As String = ""

        Try

            If Asc(e.KeyChar) = 13 Then

                If Trim(UCase(cbo_Category.Text)) <> "" Then
                    If Trim(UCase(cbo_Category.Text)) <> Trim(UCase(cbo_Category.Tag)) Or Trim(UCase(dtp_date.Text)) <> Trim(UCase(dtp_date.Tag)) Then
                        Check_and_Get_EmployeeList(sender)
                    End If
                End If

                If cbo_Month.Visible And cbo_Month.Enabled Then
                    cbo_Month.Focus()

                ElseIf cbo_Category.Visible And cbo_Category.Enabled Then
                    cbo_Category.Focus()

                ElseIf dgv_Details.Rows.Count > 0 Then
                    dgv_Details.Focus()
                    If dgv_Details.Columns(3).Visible = True Then
                        dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(3)
                    Else
                        dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(4)
                    End If
                    dgv_Details.CurrentCell.Selected = True

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

    Private Sub dtp_Date_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles dtp_date.LostFocus

        Try

            lbl_Day.Text = Trim(Format(dtp_date.Value, "dddddd"))

            If Trim(UCase(cbo_Category.Text)) <> "" Then
                If Trim(UCase(cbo_Category.Text)) <> Trim(UCase(cbo_Category.Tag)) Or Trim(UCase(dtp_date.Text)) <> Trim(UCase(dtp_date.Tag)) Then
                    Check_and_Get_EmployeeList(sender)
                End If
            End If

        Catch ex As Exception
            '----

        End Try

    End Sub

    Private Sub dtp_date_ValueChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dtp_date.ValueChanged
        Try

            lbl_Day.Text = Trim(Format(dtp_date.Value, "dddddd"))

        Catch ex As Exception
            '----

        End Try

    End Sub

    Private Sub get_EmployeeList()
        Dim Cmd As New SqlClient.SqlCommand
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim n As Integer
        Dim SNo As Integer
        Dim Sht_Mins As Single = 0
        Dim catId As Integer = 0
        Dim vMon_id As Integer = 0
        Dim vOrdBy As String = ""
        Dim dttm1 As DateTime, dttm2 As DateTime
        Dim Salary As Double = 0
        Dim OT_Salary As Double = 0

        Cmd.Connection = con

        dttm1 = New DateTime(Year(dtp_date.Value.Date), Month(dtp_date.Value.Date), 1)
        dttm2 = DateAdd(DateInterval.Month, 1, dttm1)
        dttm2 = DateAdd(DateInterval.Day, -1, dttm1)
        dttm2 = DateAdd(DateInterval.Month, 1, dttm2)

        catId = Common_Procedures.Category_NameToIdNo(con, cbo_Category.Text)

        vMon_id = 0
        If Trim(cbo_Month.Text) <> "" Then

            vMon_id = Common_Procedures.Month_NameToIdNo(con, cbo_Month.Text)

            ' If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1271" Then

            If vMon_id <= 3 Then
                    dttm1 = New DateTime(Year(Common_Procedures.Company_ToDate), vMon_id, 1)
                Else
                    dttm1 = New DateTime(Year(Common_Procedures.Company_FromDate), vMon_id, 1)
                End If

                dttm2 = DateAdd(DateInterval.Month, 1, dttm1)
                dttm2 = DateAdd(DateInterval.Day, -1, dttm2)

                dtp_date.Text = dttm2.Date

            'End If

            'dttm1 = New DateTime(IIf(vMon_id >= 4, Year(Common_Procedures.Company_FromDate), Year(Common_Procedures.Company_ToDate)), vMon_id, 1)
            'dtp_FromDate.Text = dttm1

            'dttm2 = DateAdd("M", 1, dttm1)
            'dttm2 = DateAdd("d", -1, dttm2)
            'dtp_ToDate.Text = dttm2



        End If
        If vMon_id = 0 Then
            vMon_id = Month(dtp_date.Value.Date)
        End If

        vOrdBy = ""
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1271" Then
            vOrdBy = "tRECSLNO.Category_Serial_Number, a.Card_No, a.Employee_Name"
        Else
            vOrdBy = "a.Employee_Name"
        End If


        Cmd.Parameters.Clear()
        Cmd.Parameters.AddWithValue("@AttDate", dtp_date.Value.Date)

        Cmd.Parameters.AddWithValue("@FromDate", dtp_FromDate.Value.Date)
        Cmd.Parameters.AddWithValue("@ToDate", dtp_ToDate.Value.Date)


        Cmd.CommandText = "Select a.*, b.* , Dh.Department_Name from PayRoll_Employee_Head a INNER JOIN PayRoll_Category_Head b ON a.Category_IdNo = b.Category_IdNo  Left Outer Join Department_Head Dh On Dh.Department_IdNo = a.Department_IdNo  " &
             " Where a.category_IdNo = " & Str(Val(catId)) & " and a.Join_DateTime <= @AttDate and ( a.Date_Status = 0 or ( a.Date_Status = 1 and a.Releave_DateTime >= @AttDate ) )   Order by " & vOrdBy & ""
        da1 = New SqlClient.SqlDataAdapter(Cmd)
            dt1 = New DataTable
            da1.Fill(dt1)


        With dgv_Details

            .Rows.Clear()
            SNo = 0

            If dt1.Rows.Count > 0 Then

                For i = 0 To dt1.Rows.Count - 1

                    n = .Rows.Add()

                    SNo = SNo + 1



                    Cmd.CommandText = "SELECT TOP 1 * from PayRoll_Employee_Salary_Details a Where a.employee_idno = " & Str(Val(dt1.Rows(i).Item("Employee_IdNo").ToString)) & " and ( ( @FromDate < (select min(y.From_DateTime) from PayRoll_Employee_Salary_Details y where y.employee_idno = a.employee_idno )) or (@FromDate BETWEEN a.From_DateTime and a.To_DateTime) or ( @FromDate >= (select max(z.From_DateTime) from PayRoll_Employee_Salary_Details z where z.employee_idno = a.employee_idno ))) order by a.From_DateTime desc"
                    da2 = New SqlClient.SqlDataAdapter(Cmd)
                    dt2 = New DataTable
                    da2.Fill(dt2)

                    Salary = 0
                    OT_Salary = 0

                    If dt2.Rows.Count > 0 Then
                        If IsDBNull(dt2.Rows(0).Item("For_Salary").ToString) = False Then
                            Salary = Format(Val(dt2.Rows(0).Item("For_Salary").ToString), "########0.00")
                        End If
                        If IsDBNull(dt2.Rows(0).Item("O_T").ToString) = False Then
                            OT_Salary = Format(Val(dt2.Rows(0).Item("O_T").ToString), "########0.00")
                        End If

                    End If
                    dt2.Clear()



                    .Rows(n).Cells(dgvCol_Details.SlNO).Value = Val(SNo)  ' dt1.Rows(i).Item("Category_Serial_Number").ToString 
                    .Rows(n).Cells(dgvCol_Details.EMPLOYEE_NAME).Value = dt1.Rows(i).Item("Employee_Name").ToString
                    '.Rows(n).Cells(2).Value = dt1.Rows(i).Item("Category_Name").ToString
                    .Rows(n).Cells(dgvCol_Details.BASIC_SALARY).Value = Val(Salary)
                    .Rows(n).Cells(dgvCol_Details.CODE_DEST).Value = dt1.Rows(i).Item("Department_Name").ToString
                    .Rows(n).Cells(dgvCol_Details.OT_SALARY).Value = Val(OT_Salary)
                    '.Rows(n).Cells(3).Value = ""

                    '.Rows(n).Cells(10).Value = dt1.Rows(i).Item("Category_IdNo").ToString


                Next i

            End If

            dtp_date.Tag = dtp_date.Text
            cbo_Month.Tag = cbo_Month.Text
            cbo_Category.Tag = cbo_Category.Text

            Grid_Cell_DeSelect()

        End With






    End Sub

    Private Sub dgv_Details_EditingControlShowing(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewEditingControlShowingEventArgs) Handles dgv_Details.EditingControlShowing
        Try
            dgtxt_Details = CType(dgv_Details.EditingControl, DataGridViewTextBoxEditingControl)
        Catch ex As Exception
            '----
        End Try
    End Sub

    Private Sub dgtxt_Details_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgtxt_Details.Enter
        Try
            With dgv_Details
                If .Rows.Count > 0 Then
                    .EditingControl.BackColor = Color.Lime
                    .EditingControl.ForeColor = Color.Blue
                    dgtxt_Details.SelectAll()
                End If
            End With

        Catch ex As Exception
            '-----
        End Try
    End Sub

    Private Sub dgtxt_Details_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dgtxt_Details.KeyPress

        Try
            If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
                e.Handled = True
            End If

        Catch ex As Exception
            '-----
        End Try

    End Sub

    Private Sub dgtxt_Details_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgtxt_Details.TextChanged
        Try
            With dgv_Details
                If .Rows.Count > 0 Then
                    .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(dgtxt_Details.Text)
                End If

                'If .Rows(.CurrentCell.RowIndex).Cells(dgvCol_Details.NO_Of_LEAVE).Value = "OFFICE STAFF" Then
                '    dgv_Details.Columns(5).ReadOnly = False
                'Else
                '    dgv_Details.Columns(5).ReadOnly = True
                'End If

            End With

        Catch ex As Exception
            '---

        End Try
    End Sub


    Private Sub cbo_Category_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Category.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "PayRoll_Category_Head", "Category_Name", "", "(Category_IdNo = 0)")
        cbo_Category.Tag = cbo_Category.Text
    End Sub

    Private Sub cbo_Category_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Category.KeyDown

        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Category, dtp_date, Nothing, "PayRoll_Category_Head", "Category_Name", "", "(Category_IdNo = 0)")

        If (e.KeyValue = 38 And sender.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then

            If cbo_Month.Visible And cbo_Month.Enabled Then
                cbo_Month.Focus()

            ElseIf dtp_date.Visible And dtp_date.Enabled Then
                dtp_date.Focus()

            End If



        ElseIf (e.KeyValue = 40 And sender.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then

            If Trim(UCase(cbo_Category.Text)) <> "" Then
                If Trim(UCase(cbo_Category.Text)) <> Trim(UCase(cbo_Category.Tag)) Or Trim(UCase(cbo_Month.Text)) <> Trim(UCase(cbo_Month.Tag)) Or Trim(UCase(dtp_date.Text)) <> Trim(UCase(dtp_date.Tag)) Then
                    Check_and_Get_EmployeeList(sender)
                End If
            End If

            If dgv_Details.Rows.Count > 0 Then

                dgv_Details.Focus()
                If dgv_Details.Columns(3).Visible = True Then
                    dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(3)
                Else
                    dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(4)
                End If

            Else

                If MessageBox.Show("Do you want to Save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                    save_record()
                Else
                    If cbo_Month.Visible And cbo_Month.Enabled Then
                        cbo_Month.Focus()
                    ElseIf dtp_date.Visible And dtp_date.Enabled Then
                        dtp_date.Focus()
                    End If
                End If

            End If

        End If

    End Sub

    Private Sub cbo_Category_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Category.KeyPress
        Dim Sal_type As String = ""
        Dim cat_IDNo As Integer = 0
        Dim Emp_id As Integer = 0
        Dim Sno As Integer = 0
        Dim n As Integer
        Dim Cmd As New SqlClient.SqlCommand
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable

        Dim Cmd1 As New SqlClient.SqlCommand
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt2 As New DataTable

        Dim da3 As New SqlClient.SqlDataAdapter
        Dim dt3 As New DataTable

        Dim dttm2 As Date
        Dim dttm1 As Date
        Dim Mth_Id As Integer = 0

        Dim VouAmt As String = ""
        Dim vDeduAmt As String = ""
        Dim TotDedu_Amt As String = ""

        VouAmt = 0 : vDeduAmt = 0 : TotDedu_Amt = 0

        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Category, Nothing, "PayRoll_Category_Head", "Category_Name", "", "(Category_IdNo = 0)")
        If Asc(e.KeyChar) = 13 Then

            If Trim(UCase(cbo_Category.Text)) <> "" Then
                If Trim(UCase(cbo_Category.Text)) <> Trim(UCase(cbo_Category.Tag)) Or Trim(UCase(cbo_Month.Text)) <> Trim(UCase(cbo_Month.Tag)) Or Trim(UCase(dtp_date.Text)) <> Trim(UCase(dtp_date.Tag)) Then
                    Check_and_Get_EmployeeList(sender)
                End If
            End If

            '------------------
            Cmd.Connection = con

            With dgv_Details

                For i = 0 To .RowCount - 1

                    Emp_id = Common_Procedures.Employee_NameToIdNo(con, .Rows(i).Cells(1).Value)

                    Cmd.Parameters.Clear()
                    Cmd.Parameters.AddWithValue("@FromDate", dtp_FromDate.Value.Date)
                    Cmd.Parameters.AddWithValue("@ToDate", dtp_ToDate.Value.Date)

                    TotDedu_Amt = 0
                    VouAmt = 0
                    vDeduAmt = 0

                    Cmd.CommandText = " select abs(sum(Voucher_Amount)) as VouAmount from Voucher_Details where voucher_code like 'ADVSD-%' and Ledger_idno =  " & Str(Val(Emp_id)) & " and Voucher_Date < = @ToDate "
                    ' Cmd.CommandText = "select top 1 abs(Voucher_Amount) as VouAmount  from Voucher_Details where voucher_code like 'ADVSD-%' and Ledger_idno =  " & Str(Val(Emp_id)) & " order by Voucher_No desc "
                    da2 = New SqlClient.SqlDataAdapter(Cmd)
                    dt2 = New DataTable
                    da2.Fill(dt2)

                    If dt2.Rows.Count > 0 Then
                        VouAmt = dt2.Rows(0).Item("VouAmount").ToString()
                    End If

                    dt2.Clear()

                    Cmd.CommandText = "Select Sum(a.AMount) as Deduction_AMount from Employee_Payment_Head a Where a.Ledger_Idno =  " & Str(Val(Emp_id)) & " and a.Employee_Payment_Date <= @ToDate  and (a.Payment_idno = 8 ) "
                    da3 = New SqlClient.SqlDataAdapter(Cmd)
                    dt3 = New DataTable
                    da3.Fill(dt3)
                    If dt3.Rows.Count > 0 Then
                        vDeduAmt = dt3.Rows(0).Item("Deduction_AMount").ToString()
                    End If

                    dt3.Clear()

                    'Cmd.CommandText = "Select Sum(a.AMount) as Amount from Employee_Payment_Head a Where a.Ledger_Idno =  " & Str(Val(Emp_id)) & " and a.Employee_Payment_Date Between @FromDate and @ToDate  and (a.Payment_idno = 7 or a.Payment_Idno = 6 ) "
                    Cmd.CommandText = "Select Sum(a.AMount) as Amount from Employee_Payment_Head a Where a.Ledger_Idno =  " & Str(Val(Emp_id)) & " and a.Employee_Payment_Date <= @ToDate  and (a.Payment_idno = 7 or a.Payment_Idno = 6 ) "
                    da1 = New SqlClient.SqlDataAdapter(Cmd)
                    dt1 = New DataTable
                    da1.Fill(dt1)

                    If dt1.Rows.Count > 0 Then

                        For j = 0 To dt1.Rows.Count - 1

                            TotDedu_Amt = Format(Val(VouAmt) + Val(vDeduAmt), "########0.00")

                            .Rows(i).Cells(dgvCol_Details.ADVANCE_BALANCE).Value = Format(Math.Abs(Val(dt1.Rows(j).Item("AMount").ToString) - (Val(TotDedu_Amt))), "########0.00")


                        Next

                    End If

                    dt1.Clear()


                Next

            End With



            If dgv_Details.Rows.Count > 0 Then

                dgv_Details.Focus()
                If dgv_Details.Columns(3).Visible = True Then
                    dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(3)
                Else
                    dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(4)
                End If

            Else

                If MessageBox.Show("Do you want to Save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                    save_record()
                Else
                    If cbo_Month.Visible And cbo_Month.Enabled Then
                        cbo_Month.Focus()
                    ElseIf dtp_date.Visible And dtp_date.Enabled Then
                        dtp_date.Focus()
                    End If
                End If

            End If

        End If

    End Sub

    Private Sub cbo_Category_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Category.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Dim f As New PayRoll_Category_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Category.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If
    End Sub

    Private Sub btn_List_EmployeeDetails_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_List_EmployeeDetails.Click
        Check_and_Get_EmployeeList(sender)
    End Sub

    Private Sub Check_and_Get_EmployeeList(ByVal sender As System.Object)
        Dim Cmd As New SqlClient.SqlCommand
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim movno As String = ""
        Dim NewCode As String = ""
        Dim Cat_ID As Integer = 0
        Dim month_Id As Integer = 0
        Dim vMonth_ID As Integer = 0
        Dim vSQLCondt As String = ""

        Try

            Cmd.Connection = con


            Cmd.Parameters.Clear()
            'Cmd.Parameters.AddWithValue("@EntryDate", dttm2)
            Cmd.Parameters.AddWithValue("@EntryDate", dtp_date.Value.Date)


            NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            Cat_ID = 0
            If cbo_Category.Visible = True Then
                Cat_ID = Common_Procedures.Category_NameToIdNo(con, cbo_Category.Text)
            End If

            vMonth_ID = Common_Procedures.Month_NameToIdNo(con, cbo_Month.Text)

            vSQLCondt = ""

            If vMonth_ID <> 0 Then
                vSQLCondt = " Month_IdNo = " & Str(Val(vMonth_ID)) & "  "

            Else
                vSQLCondt = " Salary_Date = @EntryDate "

            End If


            Cmd.CommandText = "Select Salary_No from PayRoll_Salary_Head " &
                                " Where company_idno = " & Str(Val(lbl_Company.Tag)) & " and " & vSQLCondt & " and " &
                                " category_IdNo = " & Str(Val(Cat_ID)) & " and " &
                                " Salary_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "'"
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
                If Trim(UCase(movno)) <> Trim(UCase(lbl_RefNo.Text)) Then
                    move_record(movno)

                Else

                    If sender.name.ToString.ToLower = btn_List_EmployeeDetails.Name.ToString.ToLower Then
                        move_record(movno)
                    End If

                End If

            Else
                get_EmployeeList()

            End If

            dtp_date.Tag = dtp_date.Text
            cbo_Month.Tag = cbo_Month.Text
            cbo_Category.Tag = cbo_Category.Text

            Total_Calculation()

        Catch ex As Exception
            MessageBox.Show(ex.Message)


        End Try

    End Sub


    Private Sub cbo_Month_GotFocus(sender As Object, e As System.EventArgs) Handles cbo_Month.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Month_Head", "Month_Name", "", "(Month_IdNo = 0)")
        cbo_Month.Tag = cbo_Month.Text
    End Sub

    Private Sub cbo_Month_KeyDown(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles cbo_Month.KeyDown
        Dim Mth_ID As Integer
        Dim dttm1 As Date
        Dim dttm2 As Date

        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Month, Nothing, Nothing, "Month_Head", "Month_Name", "", "(Month_IdNo = 0)")
        If (e.KeyValue = 38 And cbo_Month.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then

            If dtp_Date.Enabled And dtp_Date.Visible Then
                dtp_Date.Focus()

            ElseIf dgv_Details.Rows.Count > 0 Then
                dgv_Details.Focus()
                dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(1)

            Else

                btn_save.Focus()

            End If

        End If

        If (e.KeyValue = 40 And cbo_Month.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then

            cbo_Category.Focus()

            Mth_ID = Common_Procedures.Month_NameToIdNo(con, cbo_Month.Text)

            dttm1 = New DateTime(IIf(Mth_ID >= 4, Year(Common_Procedures.Company_FromDate), Year(Common_Procedures.Company_ToDate)), Mth_ID, 1)
            dtp_FromDate.Text = dttm1

            dttm2 = DateAdd("M", 1, dttm1)
            dttm2 = DateAdd("d", -1, dttm2)
            dtp_ToDate.Text = dttm2

            dtp_FromDate.Enabled = False
            dtp_ToDate.Enabled = False
        End If

    End Sub

    Private Sub cbo_Month_KeyPress(sender As Object, e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Month.KeyPress
        Dim Mth_ID As Integer
        Dim dttm1 As Date
        Dim dttm2 As Date

        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Month, Nothing, "Month_Head", "Month_Name", "", "(Month_IdNo = 0)")

        If Asc(e.KeyChar) = 13 Then
            cbo_Category.Focus()

            Mth_ID = Common_Procedures.Month_NameToIdNo(con, cbo_Month.Text)

            dttm1 = New DateTime(IIf(Mth_ID >= 4, Year(Common_Procedures.Company_FromDate), Year(Common_Procedures.Company_ToDate)), Mth_ID, 1)
            dtp_FromDate.Text = dttm1

            dttm2 = DateAdd("M", 1, dttm1)
            dttm2 = DateAdd("d", -1, dttm2)
            dtp_ToDate.Text = dttm2


            dtp_FromDate.Enabled = False
            dtp_ToDate.Enabled = False

        End If

    End Sub

    Private Sub dgv_Details_KeyUp(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles dgv_Details.KeyUp
        Dim i As Integer
        Dim n As Integer

        Try

            If e.Control = True And UCase(Chr(e.KeyCode)) = "D" Then

                With dgv_Details

                    If .Rows.Count >= 0 Then

                        n = .CurrentRow.Index

                        If .CurrentCell.RowIndex = .Rows.Count - 1 Then
                            For i = 0 To .ColumnCount - 1
                                .Rows(n).Cells(i).Value = ""
                            Next

                        Else
                            .Rows.RemoveAt(n)

                        End If

                    End If

                End With

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "ERROR WHILE DETAILS KEYUP....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub dgv_Details_CellEnter(sender As Object, e As DataGridViewCellEventArgs) Handles dgv_Details.CellEnter

        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt2 As New DataTable
        Dim rect As Rectangle

        With dgv_Details

            If e.ColumnIndex = 5 Then

                If Trim(.CurrentRow.Cells(2).Value) = "OFFICE STAFF" And Trim(.CurrentRow.Cells(2).Value) <> "" Then
                    .CurrentRow.Cells(5).ReadOnly = False
                Else
                    .CurrentRow.Cells(5).ReadOnly = True

                End If

            ElseIf e.ColumnIndex = 4 Then

                If Trim(.CurrentRow.Cells(2).Value) <> "OFFICE STAFF" And Trim(.CurrentRow.Cells(2).Value) <> "" Then
                    .CurrentRow.Cells(4).ReadOnly = False
                Else
                    .CurrentRow.Cells(4).ReadOnly = True

                End If

            End If


        End With


    End Sub

    Private Sub dgv_Details_CellValueChanged(sender As Object, e As DataGridViewCellEventArgs) Handles dgv_Details.CellValueChanged

        If FrmLdSTS = True Or NoCalc_Status = True Then Exit Sub

        Dim cmd As New SqlClient.SqlCommand
        Dim Da1 As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable

        Dim vSal_day As String
        Dim vLeave_Sal As String

        Dim vPFSTS As Integer
        Dim vESISTS As Integer

        Dim Emp_id As Integer

        Dim vTotAdd As String = 0

        Try

            If FrmLdSTS = True Or NoCalc_Status = True Then Exit Sub

            cmd.Connection = con

            With dgv_Details
                If .Visible Then

                    If .Rows.Count > 0 Then

                        'If e.ColumnIndex = dgvCol_Details.NO_OF_SHIFT Then

                        '    cmd.Parameters.Clear()
                        '    cmd.Parameters.AddWithValue("@FromDate", dtp_FromDate.Value.Date)
                        '    cmd.Parameters.AddWithValue("@ToDate", dtp_ToDate.Value.Date)
                        '    '---------
                        '    Emp_id = Common_Procedures.Employee_NameToIdNo(con, .Rows(e.RowIndex).Cells(dgvCol_Details.EMPLOYEE_NAME).Value)

                        '    If Val(Emp_id) <> 0 Then

                        '        cmd.CommandText = "select a.*, b.* from PayRoll_Employee_Head a INNER JOIN PayRoll_Category_Head b ON a.Category_IdNo <> 0 and a.Category_IdNo = b.Category_IdNo Where a.Employee_IdNo = " & Str(Val(Emp_id)) & " and a.Join_DateTime <= @ToDate and (a.Date_Status = 0 or (a.Date_Status = 1 and a.Releave_DateTime >= @FromDate ) ) Order by a.Employee_Name"
                        '        Da1 = New SqlClient.SqlDataAdapter(cmd)
                        '        Dt1 = New DataTable
                        '        Da1.Fill(Dt1)

                        '        If Dt1.Rows.Count > 0 Then
                        '            vPFSTS = Val(Dt1.Rows(0).Item("Pf_Salary").ToString)
                        '            vESISTS = Val(Dt1.Rows(0).Item("Esi_Salary").ToString)
                        '        End If

                        '    End If

                        '    '----------
                        '    If Val(vPFSTS) = 1 Then
                        '        .Rows(e.RowIndex).Cells(dgvCol_Details.PF).Value = Format((Val(.Rows(e.RowIndex).Cells(dgvCol_Details.GROSS_SALARY).Value) * 12 / 100), "###########0.00")
                        '    End If

                        '    If Val(vESISTS) = 1 Then
                        '        .Rows(e.RowIndex).Cells(dgvCol_Details.ESI).Value = Format((Val(.Rows(e.RowIndex).Cells(dgvCol_Details.GROSS_SALARY).Value) * 0.75 / 100), "###########0.00")
                        '    End If

                        'End If

                        If e.ColumnIndex = dgvCol_Details.OT_HOURS Or e.ColumnIndex = dgvCol_Details.OT_SALARY Then

                            .Rows(e.RowIndex).Cells(dgvCol_Details.OT_AMOUNT).Value = Format(Val(.Rows(e.RowIndex).Cells(dgvCol_Details.OT_SALARY).Value) * Val(.Rows(e.RowIndex).Cells(dgvCol_Details.OT_HOURS).Value), "###########0.00")

                        ElseIf e.ColumnIndex = dgvCol_Details.NO_OF_SHIFT Or e.ColumnIndex = dgvCol_Details.NO_Of_LEAVE Or e.ColumnIndex = dgvCol_Details.BASIC_SALARY Or e.ColumnIndex = dgvCol_Details.GROSS_SALARY Or e.ColumnIndex = dgvCol_Details.OT_AMOUNT Or e.ColumnIndex = dgvCol_Details.PRODUCT_INC_AMT Or e.ColumnIndex = dgvCol_Details.INCENTIVE_AMT Or e.ColumnIndex = dgvCol_Details.ADVANCE_BALANCE Or e.ColumnIndex = dgvCol_Details.LESS_ADVANCE Or e.ColumnIndex = dgvCol_Details.LESS_MESS Or e.ColumnIndex = dgvCol_Details.LESS_DEPOSIT Or e.ColumnIndex = dgvCol_Details.LESS_OTHER Then


                            '-------------ESI PF CALC

                            cmd.Parameters.Clear()
                            cmd.Parameters.AddWithValue("@FromDate", dtp_FromDate.Value.Date)
                            cmd.Parameters.AddWithValue("@ToDate", dtp_ToDate.Value.Date)
                            '---------
                            Emp_id = Common_Procedures.Employee_NameToIdNo(con, .Rows(e.RowIndex).Cells(dgvCol_Details.EMPLOYEE_NAME).Value)

                            If Val(Emp_id) <> 0 Then

                                cmd.CommandText = "select a.*, b.* from PayRoll_Employee_Head a INNER JOIN PayRoll_Category_Head b ON a.Category_IdNo <> 0 and a.Category_IdNo = b.Category_IdNo Where a.Employee_IdNo = " & Str(Val(Emp_id)) & " and a.Join_DateTime <= @ToDate and (a.Date_Status = 0 or (a.Date_Status = 1 and a.Releave_DateTime >= @FromDate ) ) Order by a.Employee_Name"
                                Da1 = New SqlClient.SqlDataAdapter(cmd)
                                Dt1 = New DataTable
                                Da1.Fill(Dt1)

                                If Dt1.Rows.Count > 0 Then
                                    vPFSTS = Val(Dt1.Rows(0).Item("Pf_Salary").ToString)
                                    vESISTS = Val(Dt1.Rows(0).Item("Esi_Salary").ToString)
                                End If

                            End If

                            '----------
                            If Val(vPFSTS) = 1 Then
                                .Rows(e.RowIndex).Cells(dgvCol_Details.PF).Value = Format((Val(.Rows(e.RowIndex).Cells(dgvCol_Details.GROSS_SALARY).Value) * 12 / 100), "###########0")
                            End If

                            If Val(vESISTS) = 1 Then
                                .Rows(e.RowIndex).Cells(dgvCol_Details.ESI).Value = Format((Val(.Rows(e.RowIndex).Cells(dgvCol_Details.GROSS_SALARY).Value) * 0.75 / 100), "###########0")
                            End If

                            '-------------

                            vTotAdd = Format(Val(.Rows(e.RowIndex).Cells(dgvCol_Details.OT_AMOUNT).Value) + Val(.Rows(e.RowIndex).Cells(dgvCol_Details.INCENTIVE_AMT).Value) + Val(.Rows(e.RowIndex).Cells(dgvCol_Details.PRODUCT_INC_AMT).Value), "###########0.00")

                            If Trim(UCase(.CurrentRow.Cells(dgvCol_Details.CODE_DEST).Value)) = Trim(UCase("OFFICE STAFF")) Then

                                vSal_day = Format(Val(.Rows(e.RowIndex).Cells(dgvCol_Details.BASIC_SALARY).Value) / (Val(.Rows(e.RowIndex).Cells(dgvCol_Details.NO_Of_DAYS).Value)), "###########0.00")

                                vLeave_Sal = Format(vSal_day * Val(.Rows(e.RowIndex).Cells(dgvCol_Details.NO_Of_LEAVE).Value), "#########0.00")

                                .Rows(e.RowIndex).Cells(dgvCol_Details.GROSS_SALARY).Value = Format(Val(.Rows(e.RowIndex).Cells(dgvCol_Details.BASIC_SALARY).Value) - vLeave_Sal + vTotAdd, "#########0")

                            Else
                                .Rows(e.RowIndex).Cells(dgvCol_Details.GROSS_SALARY).Value = Format(Val(.Rows(e.RowIndex).Cells(dgvCol_Details.NO_OF_SHIFT).Value) * Val(.Rows(e.RowIndex).Cells(dgvCol_Details.BASIC_SALARY).Value) + vTotAdd, "###########0.00")

                            End If

                            '.Rows(e.RowIndex).Cells(dgvCol_Details.GROSS_SALARY).Value = Format(Val(.Rows(e.RowIndex).Cells(dgvCol_Details.NO_OF_SHIFT).Value) * Val(.Rows(e.RowIndex).Cells(dgvCol_Details.BASIC_SALARY).Value), "###########0.00")

                            .Rows(e.RowIndex).Cells(dgvCol_Details.NET_SALARY).Value = Format(Val(.Rows(e.RowIndex).Cells(dgvCol_Details.GROSS_SALARY).Value) - Val(.Rows(e.RowIndex).Cells(dgvCol_Details.LESS_ADVANCE).Value) - Val(.Rows(e.RowIndex).Cells(dgvCol_Details.LESS_MESS).Value) - Val(.Rows(e.RowIndex).Cells(dgvCol_Details.LESS_DEPOSIT).Value) - Val(.Rows(e.RowIndex).Cells(dgvCol_Details.LESS_OTHER).Value) - Val(.Rows(e.RowIndex).Cells(dgvCol_Details.ESI).Value) - Val(.Rows(e.RowIndex).Cells(dgvCol_Details.PF).Value), "###########0")
                            '.Rows(e.RowIndex).Cells(dgvCol_Details.NET_SALARY).Value = Format(Val(.Rows(e.RowIndex).Cells(dgvCol_Details.GROSS_SALARY).Value) + Val(.Rows(e.RowIndex).Cells(dgvCol_Details.OT_AMOUNT).Value) + Val(.Rows(e.RowIndex).Cells(dgvCol_Details.PRODUCT_INC_AMT).Value) + Val(.Rows(e.RowIndex).Cells(dgvCol_Details.INCENTIVE_AMT).Value) - Val(.Rows(e.RowIndex).Cells(dgvCol_Details.LESS_ADVANCE).Value) - Val(.Rows(e.RowIndex).Cells(dgvCol_Details.LESS_MESS).Value) - Val(.Rows(e.RowIndex).Cells(dgvCol_Details.LESS_DEPOSIT).Value) - Val(.Rows(e.RowIndex).Cells(dgvCol_Details.LESS_OTHER).Value) - Val(.Rows(e.RowIndex).Cells(dgvCol_Details.ESI).Value) - Val(.Rows(e.RowIndex).Cells(dgvCol_Details.PF).Value), "###########0")

                        End If


                        Total_Calculation()

                    End If

                End If

            End With

        Catch ex As Exception

        End Try
    End Sub


    Private Sub Total_Calculation()
        Dim Sno As Integer
        Dim TotShift As String
        Dim OtAmt As String
        Dim IncAmt As String
        Dim TotGrsAmt As String
        Dim AdvBal As String
        Dim vMessDed As String
        Dim vOtrDed As String
        Dim vDepDed As String
        Dim Totsal As String
        Dim vBasSal As String

        If FrmLdSTS = True Or NoCalc_Status = True Then Exit Sub

        Sno = 0
        TotShift = 0 : OtAmt = 0 : IncAmt = 0 : TotGrsAmt = 0 : AdvBal = 0 : vMessDed = 0 : vOtrDed = 0 : vDepDed = 0 : Totsal = 0 : vBasSal = 0

        With dgv_Details
            For i = 0 To .RowCount - 1
                Sno = Sno + 1
                .Rows(i).Cells(0).Value = Sno
                If Trim(.Rows(i).Cells(dgvCol_Details.EMPLOYEE_NAME).Value) <> "" Or (Val(.Rows(i).Cells(dgvCol_Details.NO_Of_DAYS).Value) <> 0 Or Val(.Rows(i).Cells(dgvCol_Details.NO_OF_SHIFT).Value) <> 0) Then

                    TotShift = TotShift + Val(.Rows(i).Cells(dgvCol_Details.NO_OF_SHIFT).Value)
                    OtAmt = OtAmt + Val(.Rows(i).Cells(dgvCol_Details.OT_AMOUNT).Value)
                    IncAmt = IncAmt + Val(.Rows(i).Cells(dgvCol_Details.INCENTIVE_AMT).Value)
                    AdvBal = AdvBal + Val(.Rows(i).Cells(dgvCol_Details.ADVANCE_BALANCE).Value)
                    vMessDed = vMessDed + Val(.Rows(i).Cells(dgvCol_Details.LESS_MESS).Value)
                    vOtrDed = vOtrDed + Val(.Rows(i).Cells(dgvCol_Details.LESS_OTHER).Value)
                    vDepDed = vDepDed + Val(.Rows(i).Cells(dgvCol_Details.LESS_DEPOSIT).Value)
                    TotGrsAmt = Format(Val(TotGrsAmt) + Val(.Rows(i).Cells(dgvCol_Details.GROSS_SALARY).Value), "##########0.00")
                    Totsal = Totsal + Val(.Rows(i).Cells(dgvCol_Details.NET_SALARY).Value)
                    vBasSal = vBasSal + Val(.Rows(i).Cells(dgvCol_Details.BASIC_SALARY).Value)
                End If

            Next

        End With

        lbl_GrossSalary.Text = Format(Val(TotGrsAmt), "###########0.00")

        With dgv_Details_Total
            If .RowCount = 0 Then .Rows.Add()
            .Rows(0).Cells(4).Value = Val(TotShift)
            .Rows(0).Cells(6).Value = Format(Val(vBasSal), "########0.00")
            .Rows(0).Cells(7).Value = Format(Val(TotGrsAmt), "########0.00")
            .Rows(0).Cells(10).Value = Format(Val(OtAmt), "########0.00")
            .Rows(0).Cells(12).Value = Format(Val(IncAmt), "########0.00")
            .Rows(0).Cells(13).Value = Format(Val(AdvBal), "########0.00")
            .Rows(0).Cells(15).Value = Format(Val(vMessDed), "########0.00")
            .Rows(0).Cells(16).Value = Format(Val(vDepDed), "########0.00")
            .Rows(0).Cells(17).Value = Format(Val(vOtrDed), "########0.00")
            .Rows(0).Cells(18).Value = Format(Val(Totsal), "########0.00")

        End With

        lbl_Adv_Deduction.Text = Format(Val(AdvBal), "########0.00")
        lbl_Mess_Deduction.Text = Format(Val(vMessDed), "########0.00")
        lbl_Other_Deduction.Text = Format(Val(vOtrDed), "########0.00")
        lbl_OT_Salary.Text = Format(Val(OtAmt), "########0.00")
        lbl_Tot_Incentive.Text = Format(Val(IncAmt), "########0.00")
        lbl_Deposit_Deduction.Text = Format(Val(vDepDed), "########0.00")
        lbl_Tot_Salary.Text = Format(Val(Totsal), "########0.00")

    End Sub
    Private Sub txt_Mess_Amount_KeyDown(sender As Object, e As KeyEventArgs) Handles txt_Mess_Amount.KeyDown
        If e.KeyValue = 38 Then
            If dgv_Details.Enabled And dgv_Details.Visible Then
                dgv_Details.Focus()
                dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(dgvCol_Details.EMPLOYEE_NAME)
            End If
        End If

        If e.KeyValue = 40 Then
            txt_Hdfc_Deposit.Focus()
        End If
    End Sub

    Private Sub txt_Mess_Amount_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txt_Mess_Amount.KeyPress

        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
            e.Handled = True
        End If
        If Asc(e.KeyChar) = 13 Then
            txt_Hdfc_Deposit.Focus()
        End If
    End Sub
    Private Sub txt_Hdfc_Deposit_KeyDown(sender As Object, e As KeyEventArgs) Handles txt_Hdfc_Deposit.KeyDown
        If e.KeyValue = 38 Then
            txt_Mess_Amount.Focus()
        End If

        If e.KeyValue = 40 Then
            If MessageBox.Show("Do you want to save", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = DialogResult.Yes Then
                save_record()
            Else
                dtp_date.Focus()
            End If
        End If
    End Sub

    Private Sub txt_Hdfc_Deposit_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txt_Hdfc_Deposit.KeyPress

        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
            e.Handled = True
        End If

        If Asc(e.KeyChar) = 13 Then
            If MessageBox.Show("Do you want to save", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = DialogResult.Yes Then
                save_record()
            Else
                dtp_date.Focus()
            End If
        End If
    End Sub

    Private Sub dgv_Details_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgv_Details.CellContentClick

    End Sub

    Private Sub dgv_Details_Scroll(sender As Object, e As ScrollEventArgs) Handles dgv_Details.Scroll
        On Error Resume Next
        dgv_Details_Total.FirstDisplayedScrollingColumnIndex = dgv_Details.FirstDisplayedScrollingColumnIndex
    End Sub

    'Private Sub Esi_Pf_Calculation(ByVal vPFSTS As Integer, ByVal vESISTS As Integer)


    '    Dim cmd As New SqlClient.SqlCommand
    '    Dim Da As New SqlClient.SqlDataAdapter
    '    Dim Dt As New DataTable

    '    Dim SNo As Long = 0
    '    Dim vPF As Integer, vEsi As Integer
    '    Dim vTotErngs As String = 0
    '    Dim Emp_id As Integer = 0

    '    Dim vEsi_Value As String, vPf_Value As String

    '    vPF = 0
    '    vEsi = 0


    '    With dgv_Details
    '        SNo = 0

    '        For i = 0 To .RowCount - 1
    '            Emp_id = Common_Procedures.Employee_NameToIdNo(con, .Rows(i).Cells(1).Value)

    '            If Val(Emp_id) <> 0 Then

    '                vPFSTS = 0
    '                vESISTS = 0

    '                If Val(Emp_id) = 746 Then
    '                    vEsi_Value = 0
    '                End If

    '                cmd.CommandText = "select a.*, b.* from PayRoll_Employee_Head a INNER JOIN PayRoll_Category_Head b ON a.Category_IdNo <> 0 and a.Category_IdNo = b.Category_IdNo Where a.Employee_IdNo = " & Str(Val(Emp_id)) & " and a.Join_DateTime <= @ToDate and (a.Date_Status = 0 or (a.Date_Status = 1 and a.Releave_DateTime >= @FromDate ) ) Order by a.Employee_Name"
    '                Da = New SqlClient.SqlDataAdapter(cmd)
    '                Dt = New DataTable
    '                Da.Fill(Dt)

    '                If Dt.Rows.Count > 0 Then
    '                    vPFSTS = Val(Dt.Rows(0).Item("Pf_Salary").ToString)
    '                    vESISTS = Val(Dt.Rows(0).Item("Esi_Salary").ToString)
    '                End If

    '            End If

    '        Next

    '    End With

    '    vPF = vPFSTS
    '    vEsi = vESISTS
    'End Sub




End Class
