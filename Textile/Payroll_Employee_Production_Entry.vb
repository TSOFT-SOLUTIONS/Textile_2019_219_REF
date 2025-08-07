
Public Class Payroll_Employee_Production_Entry
    Implements Interface_MDIActions


    Private con As New SqlClient.SqlConnection(Common_Procedures.Connection_String)
    Private con1 As New SqlClient.SqlConnection(Common_Procedures.ConnectionString_CompanyGroupdetails)
    Private FrmLdSTS As Boolean = False
    Private New_Entry As Boolean = False
    Private Insert_Entry As Boolean = False
    Private Filter_Status As Boolean = False

    Private WithEvents dgtxt_Details As New DataGridViewTextBoxEditingControl
    Private dgvDet_CboBx_ColNos_Arr As Integer() = {1, 1}

    Private Prec_ActCtrl As New Control
    Private vCbo_ItmNm As String
    Private vcbo_KeyDwnVal As Double
    Private prn_HdDt As New DataTable
    Private prn_DetDt As New DataTable
    Private prn_PageNo As Integer
    Private cnt As Integer = 0
    Private prn_DetIndx As Integer
    Private prn_DetDt1 As New DataTable
    Private prn_PageNo1 As Integer
    Private prn_DetIndx1 As Integer
    Private prn_DetAr(50, 10) As String
    Private prn_DetMxIndx As Integer
    Private prn_NoofBmDets As Integer
    Private prn_NoofBmDets1 As Integer
    Private prn_Status As Integer
    Private Prev_kyData As Keys
    Private prn_TotCopies As Integer = 0
    Private prn_Count As Integer = 0
    Private PrntCnt2ndPageSTS As Boolean = False
    Private TrnTo_DbName As String = ""
    Private Pk_Condition As String = "TPEMP-"


    Private pth As String
    Private pth2 As String
    Private PrnTxt As String = ""
    Private a() As String

    Private prn_DetSNo As Integer
    Private prn_DetSNo1 As Integer
    Private Hz1 As Integer, Hz2 As Integer, Vz1 As Integer, Vz2 As Integer
    Private Corn1 As Integer, Corn2 As Integer, Corn3 As Integer, Corn4 As Integer
    Private LfCon As Integer, RgtCon As Integer
    Private LnCnt As Integer = 0, CenCon As Integer
    Private CenDwn As Integer, CenUp As Integer
    Private NoCalc_Status As Boolean = False
    Private Mov_Status As Boolean = False
    Private loomno_tag As String = ""
    Private Enum dgvCol_Details As Integer

        SLNO
        LOOM_NO
        CLOTH_NAME
        PRODUCTION_METER
        DAMAGE_PCS
        DAMAGE_METER
        SOUND_METER
        COOLIE_METER
        AMOUNT

    End Enum
    Private Sub clear()

        NoCalc_Status = True

        Mov_Status = False

        New_Entry = False
        Insert_Entry = False

        pnl_Back.Enabled = True
        pnl_Filter.Visible = False


        lbl_RefNo.Text = ""
        lbl_RefNo.ForeColor = Color.Black

        msk_date.Text = (dtp_Date.Text)
        msk_date.Tag = msk_date.Text

        cbo_Shift.Text = ""
        cbo_Shift.Tag = cbo_Shift.Text = ""

        cbo_Employee.Text = ""
        cbo_Employee.Tag = cbo_Employee.Text = ""


        cbo_Grid_Loom_No.Text = ""
        'cbo_Grid_Loom_No.Tag = cbo_Grid_Loom_No.Text
        Cbo_Grid_Cloth_Name.Text = ""
        loomno_tag = ""

        If Filter_Status = False Then
            dtp_Filter_Fromdate.Text = Common_Procedures.Company_FromDate   '.Date.ToShortDateString
            dtp_Filter_ToDate.Text = Common_Procedures.Company_ToDate   '.Date.ToShortDateString
            cbo_Filter_EmployeeName.Text = ""
            cbo_Filter_Quality.Text = ""
            Cbo_Filter_Loomno.Text = ""
            cbo_Filter_EmployeeName.SelectedIndex = -1
            cbo_Filter_Quality.SelectedIndex = -1
            Cbo_Filter_Loomno.SelectedIndex = -1
            dgv_Filter_Details.Rows.Clear()
        End If



        dgv_Details.Rows.Clear()
        dgv_Details.Rows.Add()
        dgv_Details_Total.Rows.Clear()
        dgv_Details_Total.Rows.Add()

        dtp_Date.Enabled = True
        dtp_Date.BackColor = Color.White
        lbl_Company.Tag = 1

        NoCalc_Status = False
        Mov_Status = False
    End Sub

    Private Sub Extra_loom_Incentive_Entry_KeyPress(sender As Object, e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        Try
            If Asc(e.KeyChar) = 27 Then

                If pnl_Filter.Visible = True Then
                    btn_Filter_Close_Click(sender, e)
                    Exit Sub

                Else
                    If MessageBox.Show("Do you want to Close?", "FOR CLOSING ENTRY...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) <> Windows.Forms.DialogResult.Yes Then
                        Exit Sub
                    Else
                        Me.Close()
                    End If

                End If

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT CLOSE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try
    End Sub
    Private Sub Payroll_Employee_Production_Entry_Activated(sender As Object, e As EventArgs) Handles Me.Activated

        Try

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Grid_Loom_No.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "LOOMNO" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Grid_Loom_No.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Employee.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "EMPLOYEE" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Employee.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(Cbo_Grid_Cloth_Name.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "CLOTH" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                Cbo_Grid_Cloth_Name.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Shift.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "SHIFT" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Shift.Text = Trim(Common_Procedures.Master_Return.Return_Value)
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
            'MessageBox.Show(ex.Message, "DOES NOT SHOW...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

        FrmLdSTS = False
    End Sub
    Private Sub Extra_loom_Incentive_Entry_Load(sender As Object, e As System.EventArgs) Handles Me.Load

        Dim da As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim dt3 As New DataTable
        Dim dt4 As New DataTable


        Me.Text = ""

        con.Open()
        pnl_Filter.Visible = False
        pnl_Filter.Left = (Me.Width - pnl_Filter.Width) \ 2
        pnl_Filter.Top = (Me.Height - pnl_Filter.Height) \ 2


        AddHandler msk_date.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Grid_Loom_No.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Shift.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Employee.GotFocus, AddressOf ControlGotFocus
        AddHandler Cbo_Grid_Cloth_Name.GotFocus, AddressOf ControlGotFocus

        AddHandler Cbo_Filter_Loomno.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Filter_EmployeeName.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Filter_Quality.GotFocus, AddressOf ControlGotFocus

        AddHandler msk_date.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Grid_Loom_No.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Shift.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Employee.LostFocus, AddressOf ControlLostFocus
        AddHandler Cbo_Grid_Cloth_Name.LostFocus, AddressOf ControlLostFocus

        AddHandler Cbo_Filter_Loomno.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Filter_EmployeeName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Filter_Quality.LostFocus, AddressOf ControlLostFocus


        lbl_Company.Text = ""
        lbl_Company.Tag = 0
        lbl_Company.Visible = False
        Common_Procedures.CompIdNo = 0


        Filter_Status = False
        FrmLdSTS = True
        new_record()
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



            ElseIf pnl_Back.Enabled = True Then
                dgv1 = dgv_Details

            End If

            If IsNothing(dgv1) = False Then

                With dgv1

                    If keyData = Keys.Enter Or keyData = Keys.Down Or keyData = Keys.Up Then

                        Common_Procedures.DGVGrid_Cursor_Focus(dgv1, keyData, cbo_Shift, Nothing, dgvDet_CboBx_ColNos_Arr, dgtxt_Details, cbo_Shift, 3,,, 3)

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






    Private Sub dgtxt_Details_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgtxt_Details.Enter
        Try
            dgv_Details.EditingControl.BackColor = Color.Lime
            dgv_Details.EditingControl.ForeColor = Color.Blue
            dgtxt_Details.SelectAll()
        Catch ex As Exception
            '--
        End Try

    End Sub

    Private Sub ControlGotFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim txtbx As TextBox
        Dim combobx As ComboBox
        Dim msktxtbx As MaskedTextBox

        On Error Resume Next

        If TypeOf Me.ActiveControl Is TextBox Or TypeOf Me.ActiveControl Is ComboBox Or TypeOf Me.ActiveControl Is Button Or TypeOf Me.ActiveControl Is MaskedTextBox Then
            Me.ActiveControl.BackColor = Color.Lime
            Me.ActiveControl.ForeColor = Color.Blue
        End If

        If TypeOf Me.ActiveControl Is TextBox Then
            txtbx = Me.ActiveControl
            txtbx.SelectAll()
        ElseIf TypeOf Me.ActiveControl Is ComboBox Then
            combobx = Me.ActiveControl
            combobx.SelectAll()
        ElseIf TypeOf Me.ActiveControl Is MaskedTextBox Then
            msktxtbx = Me.ActiveControl
            msktxtbx.SelectAll()
        End If

        If Me.ActiveControl.Name <> cbo_Grid_Loom_No.Name Then
            cbo_Grid_Loom_No.Visible = False
        End If
        If Me.ActiveControl.Name <> Cbo_Grid_Cloth_Name.Name Then
            Cbo_Grid_Cloth_Name.Visible = False
        End If

        Grid_Cell_DeSelect()

        Prec_ActCtrl = Me.ActiveControl

    End Sub

    Private Sub ControlLostFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        On Error Resume Next
        If IsNothing(Prec_ActCtrl) = False Then
            If TypeOf Prec_ActCtrl Is Button Then
                Prec_ActCtrl.BackColor = Color.FromArgb(41, 57, 85)
                Prec_ActCtrl.ForeColor = Color.White
            Else
                Prec_ActCtrl.BackColor = Color.White
                Prec_ActCtrl.ForeColor = Color.Black
            End If
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
        If Not IsNothing(dgv_Details.CurrentCell) Then dgv_Details.CurrentCell.Selected = False
        If Not IsNothing(dgv_Details_Total.CurrentCell) Then dgv_Details_Total.CurrentCell.Selected = False
        If Not IsNothing(dgv_Filter_Details.CurrentCell) Then dgv_Filter_Details.CurrentCell.Selected = False
        'dgv_Details.CurrentCell.Selected = False
        'dgv_Details_Total.CurrentCell.Selected = False
        'dgv_Filter_Details.CurrentCell.Selected = False
    End Sub


    Dim da1 As New SqlClient.SqlDataAdapter
    Dim da2 As New SqlClient.SqlDataAdapter
    Dim dt1 As New DataTable
    Dim dt2 As New DataTable
    Dim NewCode As String
    Dim n As Integer
    Dim SNo As Integer
    Dim LockSTS As Boolean = False

    Private Sub move_record(ByVal no As String)
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim NewCode As String
        Dim n As Integer
        Dim SNo As Integer
        Dim LockSTS As Boolean = False
        If Val(no) = 0 Then Exit Sub

        clear()

        Mov_Status = True


        NewCode = Trim(Pk_Condition) & Val(lbl_Company.Tag) & "-" & Trim(no) & "/" & Trim(Common_Procedures.FnYearCode)

        Try

            da1 = New SqlClient.SqlDataAdapter("select * from Textile_Payroll_Employee_Production_Head  Where Payroll_Employee_Production_Code = '" & Trim(NewCode) & "'", con)
            da1.Fill(dt1)

            If dt1.Rows.Count > 0 Then

                lbl_RefNo.Text = dt1.Rows(0).Item("Payroll_Employee_Production_no").ToString
                msk_date.Text = dt1.Rows(0).Item("Payroll_Employee_Production_Date").ToString
                msk_date.Tag = msk_date.Text
                cbo_Employee.Text = Common_Procedures.Employee_IdNoToName(con, dt1.Rows(0).Item("Employee_Idno").ToString)
                cbo_Employee.Tag = cbo_Employee.Text
                cbo_Shift.Text = Common_Procedures.Shift_IdNoToName(con, dt1.Rows(0).Item("Shift_Idno").ToString)
                cbo_Shift.Tag = cbo_Shift.Text

                dt2 = New DataTable
                da2 = New SqlClient.SqlDataAdapter("select * from Textile_Payroll_Employee_Production_Details a Where a.Payroll_Employee_Production_Code = '" & Trim(NewCode) & "' ", con)

                da2.Fill(dt2)

                dgv_Details.Rows.Clear()
                SNo = 0

                If dt2.Rows.Count > 0 Then

                    For i = 0 To dt2.Rows.Count - 1

                        n = dgv_Details.Rows.Add()

                        SNo = SNo + 1
                        dgv_Details.Rows(n).Cells(dgvCol_Details.SLNO).Value = Val(SNo)
                        dgv_Details.Rows(n).Cells(dgvCol_Details.LOOM_NO).Value = Common_Procedures.Loom_IdNoToName(con, Val(dt2.Rows(i).Item("Loom_IdNo").ToString))
                        dgv_Details.Rows(n).Cells(dgvCol_Details.CLOTH_NAME).Value = Common_Procedures.Cloth_IdNoToName(con, Val(dt2.Rows(i).Item("Cloth_Idno").ToString))
                        dgv_Details.Rows(n).Cells(dgvCol_Details.PRODUCTION_METER).Value = Format(Val(dt2.Rows(i).Item("Production_Meters").ToString), "########0.00")
                        dgv_Details.Rows(n).Cells(dgvCol_Details.DAMAGE_PCS).Value = Format(Val(dt2.Rows(i).Item("Damage_Pcs").ToString), "########0.00")
                        dgv_Details.Rows(n).Cells(dgvCol_Details.DAMAGE_METER).Value = Format(Val(dt2.Rows(i).Item("Damage_Meters").ToString), "########0.00")
                        dgv_Details.Rows(n).Cells(dgvCol_Details.SOUND_METER).Value = Format(Val(dt2.Rows(i).Item("Type1_Meters").ToString), "########0.00")
                        dgv_Details.Rows(n).Cells(dgvCol_Details.COOLIE_METER).Value = Format(Val(dt2.Rows(i).Item("Coolie_Per_Meter").ToString), "########0.00")
                        dgv_Details.Rows(n).Cells(dgvCol_Details.AMOUNT).Value = Format(Val(dt2.Rows(i).Item("Amount").ToString), "########0.00")




                    Next i

                End If

                With dgv_Details_Total
                    If .RowCount = 0 Then .Rows.Add()

                    .Rows(0).Cells(dgvCol_Details.LOOM_NO).Value = Format(Val(dt1.Rows(0).Item("Total_No_Of_Looms").ToString), "########0")
                    .Rows(0).Cells(dgvCol_Details.PRODUCTION_METER).Value = Format(Val(dt1.Rows(0).Item("Total_Production_Meter").ToString), "########0.00")
                    .Rows(0).Cells(dgvCol_Details.DAMAGE_PCS).Value = Format(Val(dt1.Rows(0).Item("Total_Damage_Pcs").ToString), "#########0.00")
                    .Rows(0).Cells(dgvCol_Details.DAMAGE_METER).Value = Format(Val(dt1.Rows(0).Item("Total_Damage_Meters").ToString), "#########0.00")
                    .Rows(0).Cells(dgvCol_Details.SOUND_METER).Value = Format(Val(dt1.Rows(0).Item("Total_Type1_Meters").ToString), "#########0.00")
                    .Rows(0).Cells(dgvCol_Details.AMOUNT).Value = Format(Val(dt1.Rows(0).Item("Total_Amount").ToString), "#########0.00")
                End With

                dt2.Clear()
                dt2.Dispose()
                da2.Dispose()

            End If

            dt1.Clear()
            dt1.Dispose()
            da1.Dispose()


            Grid_Cell_DeSelect()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT MOVE RECORDS...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

        If msk_date.Visible And msk_date.Enabled Then msk_date.Focus()

        NoCalc_Status = False
        Mov_Status = False


    End Sub

    Public Sub delete_record() Implements Interface_MDIActions.delete_record
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim cmd As New SqlClient.SqlCommand
        Dim trans As SqlClient.SqlTransaction
        Dim NewCode As String = ""
        Dim vDbName As String = ""



        If Trim(TrnTo_DbName) <> "" Then
            vDbName = Trim(TrnTo_DbName) & ".."
        End If
        NewCode = Trim(Pk_Condition) & Val(lbl_Company.Tag) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)



        If MessageBox.Show("Do you want to Delete?", "FOR DELETION...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) <> Windows.Forms.DialogResult.Yes Then
            Exit Sub
        End If

        If New_Entry = True Then
            MessageBox.Show("This is New Entry", "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
        End If


        Dt1.Clear()
        Dt1.Dispose()
        Da.Dispose()
        trans = con.BeginTransaction

        Try

            NewCode = Trim(Pk_Condition) & Val(lbl_Company.Tag) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            cmd.Connection = con
            cmd.Transaction = trans




            cmd.CommandText = "Delete from Textile_Payroll_Employee_Production_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Payroll_Employee_Production_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "delete from Textile_Payroll_Employee_Production_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Payroll_Employee_Production_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            trans.Commit()

            Dt1.Dispose()
            Da.Dispose()
            trans.Dispose()
            cmd.Dispose()

            new_record()

            MessageBox.Show("Deleted Sucessfully!!!", "FOR DELETION...", MessageBoxButtons.OKCancel, MessageBoxIcon.Information)

        Catch ex As Exception
            trans.Rollback()
            MessageBox.Show(ex.Message, "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

        If msk_date.Enabled = True And msk_date.Visible = True Then msk_date.Focus()

    End Sub

    Public Sub filter_record() Implements Interface_MDIActions.filter_record
        If Filter_Status = False Then

            Dim da As New SqlClient.SqlDataAdapter
            Dim dt1 As New DataTable
            Dim dt2 As New DataTable
            Dim dt3 As New DataTable

            da = New SqlClient.SqlDataAdapter("select EMployee_Name from PayRoll_Employee_Head  order by EMployee_Name", con)
            da.Fill(dt1)
            cbo_Filter_EmployeeName.DataSource = dt1
            cbo_Filter_EmployeeName.DisplayMember = "EMployee_Name"

            da = New SqlClient.SqlDataAdapter("select Cloth_Name from Cloth_head order by Cloth_Name", con)
            da.Fill(dt2)
            cbo_Filter_Quality.DataSource = dt2
            cbo_Filter_Quality.DisplayMember = "Cloth_Name"


            da = New SqlClient.SqlDataAdapter("select Loom_Name from Loom_head order by Loom_Name", con)
            da.Fill(dt3)
            cbo_Filter_Quality.DataSource = dt3
            cbo_Filter_Quality.DisplayMember = "Loom_Name"




            dtp_Filter_Fromdate.Text = Common_Procedures.Company_FromDate   '.Date.ToShortDateString
            dtp_Filter_ToDate.Text = Common_Procedures.Company_ToDate   '.Date.ToShortDateString
            cbo_Filter_EmployeeName.Text = ""

            cbo_Filter_EmployeeName.SelectedIndex = -1

            dgv_Filter_Details.Rows.Clear()

        End If

        pnl_Filter.Visible = True
        pnl_Filter.Enabled = True
        pnl_Back.Enabled = False
        If dtp_Filter_Fromdate.Enabled And dtp_Filter_Fromdate.Visible Then dtp_Filter_Fromdate.Focus()

    End Sub


    Public Sub insert_record() Implements Interface_MDIActions.insert_record
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim movno As String, inpno As String
        Dim RecCode As String


        Try

            inpno = InputBox("Enter New Ref No.", "FOR NEW Ref INSERTION...")

            RecCode = Trim(Pk_Condition) & Val(lbl_Company.Tag) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

            Da = New SqlClient.SqlDataAdapter("select Payroll_Employee_Production_no from Textile_Payroll_Employee_Production_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Payroll_Employee_Production_Code = '" & Trim(RecCode) & "'", con)
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
                    MessageBox.Show("Invalid Dc No", "DOES NOT INSERT NEW DC...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

                Else
                    new_record()
                    Insert_Entry = True
                    lbl_RefNo.Text = Trim(UCase(inpno))

                End If

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT INSERT NEW REF...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Public Sub movefirst_record() Implements Interface_MDIActions.movefirst_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim movno As String

        Try

            da = New SqlClient.SqlDataAdapter("select top 1 Payroll_Employee_Production_no from Textile_Payroll_Employee_Production_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Payroll_Employee_Production_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby, Payroll_Employee_Production_no", con)
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

    Public Sub movelast_record() Implements Interface_MDIActions.movelast_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim movno As String = ""

        Try
            da = New SqlClient.SqlDataAdapter("select top 1 Payroll_Employee_Production_no from Textile_Payroll_Employee_Production_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Payroll_Employee_Production_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, Payroll_Employee_Production_no desc", con)
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

    Public Sub movenext_record() Implements Interface_MDIActions.movenext_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim movno As String = ""
        Dim OrdByNo As Single = 0

        Try

            OrdByNo = Common_Procedures.OrderBy_CodeToValue(Trim(lbl_RefNo.Text))

            da = New SqlClient.SqlDataAdapter("select top 1 Payroll_Employee_Production_no from Textile_Payroll_Employee_Production_Head where for_orderby > " & Str(OrdByNo) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and Payroll_Employee_Production_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby, Payroll_Employee_Production_no", con)
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

            da = New SqlClient.SqlDataAdapter("select top 1 Payroll_Employee_Production_no from Textile_Payroll_Employee_Production_Head where for_orderby < " & Str(OrdByNo) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and Payroll_Employee_Production_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, Payroll_Employee_Production_no desc", con)
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
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim NewID As Integer = 0

        Try
            clear()

            New_Entry = True

            lbl_RefNo.Text = Common_Procedures.get_MaxCode(con, "Textile_Payroll_Employee_Production_Head", "Payroll_Employee_Production_Code", "For_OrderBy", "", Val(lbl_Company.Tag), Common_Procedures.FnYearCode)
            ' dtp_Time.Text = Format(Now, "hh:mm tt").ToString

            lbl_RefNo.ForeColor = Color.Red

            If dtp_Date.Enabled And dtp_Date.Visible Then dtp_Date.Focus()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR NEW RECORD...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

        dt.Dispose()
        da.Dispose()

        If msk_date.Visible And msk_date.Enabled Then msk_date.Focus()
    End Sub

    Public Sub open_record() Implements Interface_MDIActions.open_record
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim movno As String, inpno As String
        Dim RecCode As String

        Try

            inpno = InputBox("Enter Ref.No.", "FOR FINDING...")

            RecCode = Trim(Pk_Condition) & Val(lbl_Company.Tag) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

            Da = New SqlClient.SqlDataAdapter("select Payroll_Employee_Production_no from Textile_Payroll_Employee_Production_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Payroll_Employee_Production_Code = '" & Trim(RecCode) & "'", con)
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

    Public Sub print_record() Implements Interface_MDIActions.print_record

    End Sub

    Public Sub save_record() Implements Interface_MDIActions.save_record


        Dim cmd As New SqlClient.SqlCommand
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim tr As SqlClient.SqlTransaction
        Dim NewCode As String = ""
        Dim EMP_ID As Integer = 0
        Dim Recv_ID As Integer = 0 'cbo_Rec_Ledger
        Dim Rec_ID As Integer = 0
        Dim Trans_ID As Integer = 0
        Dim Clo_ID As Integer = 0
        Dim EdsCnt_ID As Integer = 0
        Dim Sno As Integer = 0
        Dim Partcls As String = ""
        Dim PBlNo As String = ""
        Dim YCnt_ID As Integer = 0
        Dim Quality_ID As Integer = 0
        Dim vTotPrd_Mtr As String, vTotAMount As String, vDaMage_mtr As String, vcoolie_mtr As String, vTot_lms As String, vTot_Typ1_Mtrs As String
        Dim EntID As String = ""
        Dim Nr As Integer = 0
        Dim UserIdNo As Integer = 0
        Dim Shift_Id = 0
        Dim Loom_Id = 0

        UserIdNo = Common_Procedures.User.IdNo

        NewCode = Trim(Pk_Condition) & Val(lbl_Company.Tag) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)





        If pnl_Back.Enabled = False Then
            MessageBox.Show("Close Other Windows", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        If IsDate(msk_date.Text) = False Then
            MessageBox.Show("Invalid Date", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If msk_date.Enabled And msk_date.Visible Then msk_date.Focus()
            Exit Sub
        End If

        If Not (Convert.ToDateTime(msk_date.Text) >= Common_Procedures.Company_FromDate And Convert.ToDateTime(msk_date.Text) <= Common_Procedures.Company_ToDate) Then
            MessageBox.Show("Date is out of financial range", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If msk_date.Enabled And msk_date.Visible Then msk_date.Focus()
            Exit Sub
        End If


        For i = 0 To dgv_Details.RowCount - 1

            If Trim(dgv_Details.Rows(i).Cells(dgvCol_Details.LOOM_NO).Value) <> "" Then

                Quality_ID = Common_Procedures.Cloth_NameToIdNo(con, dgv_Details.Rows(i).Cells(dgvCol_Details.CLOTH_NAME).Value)

                If Quality_ID = 0 Then
                    MessageBox.Show("Invalid Cloth Name", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    If dgv_Details.Enabled And dgv_Details.Visible Then
                        dgv_Details.Focus()
                        dgv_Details.CurrentCell = dgv_Details.Rows(i).Cells(dgvCol_Details.CLOTH_NAME)
                        dgv_Details.CurrentCell.Selected = True

                    End If
                    Exit Sub
                End If


            End If
        Next


        EMP_ID = Common_Procedures.Employee_NameToIdNo(con, cbo_Employee.Text)
        Shift_Id = Common_Procedures.Shift_NameToIdNo(con, cbo_Shift.Text)

        If EMP_ID = 0 Then
            MessageBox.Show("Invalid Employee Name", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            cbo_Employee.Focus()
            Exit Sub
        End If

        If Shift_Id = 0 Then
            MessageBox.Show("Invalid Shift", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            cbo_Shift.Focus()
            Exit Sub
        End If


        vTotPrd_Mtr = 0 : vTotAMount = 0 : vDaMage_mtr = 0 : vcoolie_mtr = 0 : vTot_lms = 0 : vTot_Typ1_Mtrs = 0
        If dgv_Details_Total.RowCount > 0 Then
            vTot_lms = Val(dgv_Details_Total.Rows(0).Cells(dgvCol_Details.LOOM_NO).Value())
            vTotPrd_Mtr = Val(dgv_Details_Total.Rows(0).Cells(dgvCol_Details.PRODUCTION_METER).Value())
            vDaMage_mtr = Val(dgv_Details_Total.Rows(0).Cells(dgvCol_Details.DAMAGE_PCS).Value())
            vTot_Typ1_Mtrs = Val(dgv_Details_Total.Rows(0).Cells(dgvCol_Details.SOUND_METER).Value())
            vTotAMount = Val(dgv_Details_Total.Rows(0).Cells(dgvCol_Details.AMOUNT).Value())
        End If

        tr = con.BeginTransaction

        Try

            If Insert_Entry = True Or New_Entry = False Then
                NewCode = Trim(Pk_Condition) & Val(lbl_Company.Tag) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            Else

                lbl_RefNo.Text = Common_Procedures.get_MaxCode(con, "Textile_Payroll_Employee_Production_Head", "Payroll_Employee_Production_Code", "For_OrderBy", "", Val(lbl_Company.Tag), Common_Procedures.FnYearCode, tr)

                NewCode = Trim(Pk_Condition) & Val(lbl_Company.Tag) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            End If

            cmd.Connection = con
            cmd.Transaction = tr

            cmd.Parameters.Clear()
            cmd.Parameters.AddWithValue("@RefDate", Convert.ToDateTime(msk_date.Text))


            If New_Entry = True Then

                cmd.CommandText = "Insert into Textile_Payroll_Employee_Production_Head(    Payroll_Employee_Production_Code    ,             Company_idNo          ,       Payroll_Employee_Production_No  ,                                for_OrderBy                              ,         Employee_Idno     ,         Shift_Idno           , Payroll_Employee_Production_Date ,    Total_Production_Meter    ,      Total_Damage_Pcs         ,       Total_Amount           ,           Total_No_Of_Looms  , Total_Type1_Meters  ) " &
                                                                            "Values (               '" & Trim(NewCode) & "'     , " & Str(Val(lbl_Company.Tag)) & ",     '" & Trim(lbl_RefNo.Text) & "'     , " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text))) & " ,  " & Str(Val(EMP_ID)) & " , " & Str(Val(Shift_Id)) & " , @RefDate              ,  " & Str(Val(vTotPrd_Mtr)) & "," & Str(Val(vDaMage_mtr)) & " ,  " & Str(Val(vTotAMount)) & ", " & Str(Val(vTot_lms)) & "   , " & Str(Val(vTot_Typ1_Mtrs)) & ")"
                cmd.ExecuteNonQuery()

            Else

                cmd.CommandText = "Update Textile_Payroll_Employee_Production_Head set  Payroll_Employee_Production_no='" & Trim(lbl_RefNo.Text) & "',for_OrderBy=" & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text))) & ",Payroll_Employee_Production_Date=@RefDate, Employee_Idno   = " & Str(Val(EMP_ID)) & " ,Shift_Idno =  " & Str(Val(Shift_Id)) & ",   Total_Production_Meter=" & Str(Val(vTotPrd_Mtr)) & ",Total_Damage_Pcs=" & Str(Val(vDaMage_mtr)) & " , Total_Amount =" & Str(Val(vTotAMount)) & ",Total_No_Of_Looms =" & Str(Val(vTot_lms)) & " ,Total_Type1_Meters   = " & Str(Val(vTot_Typ1_Mtrs)) & "  Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Payroll_Employee_Production_Code = '" & Trim(NewCode) & "' "
                cmd.ExecuteNonQuery()

            End If


            cmd.CommandText = "Delete from Textile_Payroll_Employee_Production_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Payroll_Employee_Production_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()


            With dgv_Details
                Sno = 0


                For i = 0 To .RowCount - 1

                    If Trim(.Rows(i).Cells(dgvCol_Details.LOOM_NO).Value) <> "" And Val(.Rows(i).Cells(dgvCol_Details.PRODUCTION_METER).Value) <> 0 Then

                        Sno = Sno + 1

                        Loom_Id = Common_Procedures.Loom_NameToIdNo(con, .Rows(i).Cells(dgvCol_Details.LOOM_NO).Value, tr)
                        Quality_ID = Common_Procedures.Cloth_NameToIdNo(con, .Rows(i).Cells(dgvCol_Details.CLOTH_NAME).Value, tr)



                        cmd.CommandText = "Insert into Textile_Payroll_Employee_Production_Details  ( Payroll_Employee_Production_Code  ,            Company_idNo            ,   Payroll_Employee_Production_No  ,                                               for_OrderBy                , Payroll_Employee_Production_Date ,            Sl_No       ,        Employee_Idno     ,                 Cloth_Idno     ,     Loom_Idno          ,          Shift_Idno     ,                                    Production_Meters                   ,                                       Damage_Pcs                   ,                          Damage_Meters                            ,              Type1_Meters            ,                                     Coolie_Per_Meter        ,                                        Amount ) " &
                              " Values                                                              ('" & Trim(NewCode) & "'             , " & Str(Val(lbl_Company.Tag)) & " ,  '" & Trim(lbl_RefNo.Text) & "'   ,  " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text))) & ",              @RefDate              , " & Str(Val(Sno)) & "  ,  " & Str(Val(EMP_ID)) & ", " & Str(Val(Quality_ID)) & " ," & Str(Val(Loom_Id)) & "," & Str(Val(Shift_Id)) & ", " & Str(Val(.Rows(i).Cells(dgvCol_Details.PRODUCTION_METER).Value)) & ", " & Str(Val(.Rows(i).Cells(dgvCol_Details.DAMAGE_PCS).Value)) & ", " & Str(Val(.Rows(i).Cells(dgvCol_Details.DAMAGE_METER).Value)) & " , " & Str(Val(.Rows(i).Cells(dgvCol_Details.SOUND_METER).Value)) & ", " & Str(Val(.Rows(i).Cells(dgvCol_Details.COOLIE_METER).Value)) & ", " & Str(Val(.Rows(i).Cells(dgvCol_Details.AMOUNT).Value)) & ")"
                        cmd.ExecuteNonQuery()





                    End If
                Next
            End With

            tr.Commit()

            Dt1.Dispose()
            Da.Dispose()

            MessageBox.Show("Saved Sucessfully!!!", "FOR SAVING...", MessageBoxButtons.OK, MessageBoxIcon.Information)


            If New_Entry = True Then
                new_record()
            Else
                move_record(lbl_RefNo.Text)
            End If


        Catch ex As Exception
            tr.Rollback()

            MessageBox.Show(ex.Message, "FOR SAVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)


        Finally
            If msk_date.Enabled And msk_date.Visible Then msk_date.Focus()

        End Try








    End Sub




    Private Sub msk_Date_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles msk_date.KeyUp
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
    Private Sub msk_Date_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles msk_date.LostFocus

        If IsDate(msk_date.Text) = True Then
            If Microsoft.VisualBasic.DateAndTime.Day(Convert.ToDateTime(msk_date.Text)) <= 31 Or Microsoft.VisualBasic.DateAndTime.Month(Convert.ToDateTime(msk_date.Text)) <= 31 Then
                If Microsoft.VisualBasic.DateAndTime.Year(Convert.ToDateTime(msk_date.Text)) <= 2050 And Microsoft.VisualBasic.DateAndTime.Year(Convert.ToDateTime(msk_date.Text)) >= 2000 Then
                    dtp_Date.Value = Convert.ToDateTime(msk_date.Text)
                End If
            End If

        End If
    End Sub

    Private Sub dtp_Date_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dtp_Date.TextChanged
        If IsDate(dtp_Date.Text) = True Then
            msk_date.Text = dtp_Date.Text
        End If
    End Sub

    Private Sub msk_Date_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles msk_date.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        ' dgv_Details.Focus()
        If e.KeyCode = 40 Then
            cbo_Employee.Focus()
        End If
    End Sub

    Private Sub dgv_Details_CellEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellEnter
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim Dt2 As New DataTable
        Dim Dt3 As New DataTable
        Dim Dt4 As New DataTable
        Dim Rect As Rectangle

        With dgv_Details

            ' dgv_ActCtrlName = .Name.ToString
            dgv_Details.Tag = .CurrentCell.Value


            If Val(.CurrentRow.Cells(0).Value) = 0 Then
                .CurrentRow.Cells(0).Value = .CurrentRow.Index + 1
            End If

            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1282" Then '---- BRT SIZING
                If e.RowIndex > 0 And e.ColumnIndex = 1 Then
                    If Val(.CurrentRow.Cells(1).Value) = 0 And e.RowIndex = .RowCount - 1 Then
                        .CurrentRow.Cells(1).Value = .Rows(e.RowIndex - 1).Cells(1).Value
                        .CurrentRow.Cells(7).Value = .Rows(e.RowIndex - 1).Cells(7).Value
                        .CurrentRow.Cells(8).Value = .Rows(e.RowIndex - 1).Cells(8).Value
                    End If
                End If
            End If





            'If e.ColumnIndex = dgvCol_Details.CLOTH_NAME Then

            '    If Cbo_Grid_Cloth_Name.Visible = False Or Val(Cbo_Grid_Cloth_Name.Tag) <> e.RowIndex Then

            '        'dgv_ActCtrlName = dgv_Details.Name

            '        Cbo_Grid_Cloth_Name.Tag = -1
            '        Da = New SqlClient.SqlDataAdapter("select Cloth_Name from Cloth_head Order by Cloth_Name", con)
            '        Dt1 = New DataTable
            '        Da.Fill(Dt1)
            '        Cbo_Grid_Cloth_Name.DataSource = Dt1
            '        Cbo_Grid_Cloth_Name.DisplayMember = "Cloth_Name"

            '        Rect = .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False)

            '        Cbo_Grid_Cloth_Name.Left = .Left + Rect.Left  '  .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Left
            '        Cbo_Grid_Cloth_Name.Top = .Top + Rect.Top  ' .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Top
            '        Cbo_Grid_Cloth_Name.Width = Rect.Width  ' .CurrentCell.Size.Width
            '        Cbo_Grid_Cloth_Name.Height = Rect.Height  ' rect.Height

            '        Cbo_Grid_Cloth_Name.Text = .CurrentCell.Value  '  Trim(.CurrentRow.Cells(e.ColumnIndex).Value)

            '        Cbo_Grid_Cloth_Name.Tag = Val(e.RowIndex)
            '        Cbo_Grid_Cloth_Name.Visible = True

            '        Cbo_Grid_Cloth_Name.BringToFront()
            '        Cbo_Grid_Cloth_Name.Focus()

            '    End If
            'Else

            '    Cbo_Grid_Cloth_Name.Visible = False
            'End If

            If e.ColumnIndex = dgvCol_Details.LOOM_NO Then


                If .CurrentCell.RowIndex > 0 And Trim(.CurrentRow.Cells(dgvCol_Details.LOOM_NO).Value) = "" Then
                    Get_Loom_No_From_Loom_Master()
                End If


                If cbo_Grid_Loom_No.Visible = False Or Val(cbo_Grid_Loom_No.Tag) <> e.RowIndex Then

                    'dgv_ActCtrlName = dgv_Details.Name

                    cbo_Grid_Loom_No.Tag = -1
                    Da = New SqlClient.SqlDataAdapter("select Loom_Name from Loom_head Order by Loom_Name", con)
                    Dt1 = New DataTable
                    Da.Fill(Dt1)
                    cbo_Grid_Loom_No.DataSource = Dt1
                    cbo_Grid_Loom_No.DisplayMember = "Loom_Name"

                    Rect = .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False)

                    cbo_Grid_Loom_No.Left = .Left + Rect.Left  '  .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Left
                    cbo_Grid_Loom_No.Top = .Top + Rect.Top  ' .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Top
                    cbo_Grid_Loom_No.Width = Rect.Width  ' .CurrentCell.Size.Width
                    cbo_Grid_Loom_No.Height = Rect.Height  ' rect.Height

                    cbo_Grid_Loom_No.Text = .CurrentCell.Value  '  Trim(.CurrentRow.Cells(e.ColumnIndex).Value)
                    loomno_tag = Trim(UCase(dgv_Details.CurrentRow.Cells(dgvCol_Details.LOOM_NO).Value))


                    cbo_Grid_Loom_No.Tag = Val(e.RowIndex)
                    cbo_Grid_Loom_No.Visible = True

                    cbo_Grid_Loom_No.BringToFront()
                    cbo_Grid_Loom_No.Focus()


                End If
            Else

                cbo_Grid_Loom_No.Visible = False
            End If

            If e.ColumnIndex = dgvCol_Details.PRODUCTION_METER Then

                If .CurrentCell.RowIndex > 0 And Trim(.CurrentRow.Cells(dgvCol_Details.LOOM_NO).Value) = "" Then
                    Get_Loom_No_From_Loom_Master()
                End If

            End If


        End With

    End Sub


    Private Sub dgv_Details_CellLeave(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellLeave

        If FrmLdSTS = True Or Mov_Status = True Then Exit Sub

        With dgv_Details
            If .CurrentCell.ColumnIndex <> dgvCol_Details.LOOM_NO And .CurrentCell.ColumnIndex <> dgvCol_Details.CLOTH_NAME And .CurrentCell.ColumnIndex <> dgvCol_Details.SLNO Then
                .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = Format(Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value), "#########0.00")
            End If

            If .CurrentCell.ColumnIndex = dgvCol_Details.LOOM_NO Then
                If Trim(UCase(loomno_tag)) <> Trim(UCase(dgv_Details.CurrentRow.Cells(dgvCol_Details.LOOM_NO).Value)) Then
                    loomno_tag = Trim(UCase(dgv_Details.CurrentRow.Cells(dgvCol_Details.LOOM_NO).Value))
                    Get_Cloth_Name_From_Loom_Creation()
                End If
            End If
        End With


    End Sub
    Private Sub dgv_Details_CellValueChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellValueChanged
        Dim vEffPerc As String = ""
        Dim Incnt_amt As String = ""
        Dim Coolie_Amt As String = ""
        Dim Sound_Mtr As String = 0

        If FrmLdSTS = True Or Mov_Status = True Then Exit Sub


        Try


            With dgv_Details

                If IsNothing(dgv_Details.CurrentCell) Then Exit Sub

                If .Visible Then

                    Coolie_Amt = 0
                    Sound_Mtr = 0


                    If Trim(UCase(dgv_Details.Tag)) <> Trim(UCase(dgv_Details.CurrentCell.Value)) Then
                        'dgv_Details.Tag = .CurrentCell.Value
                        Amount_Calculation(e.RowIndex, e.ColumnIndex)
                    End If
                    'If Trim(UCase(dgv_Details.Tag)) <> Trim(UCase(dgv_Details.CurrentRow.Cells(dgvCol_Details.LOOM_NO).Value)) And Trim(UCase(dgv_Details.Tag)) <> Trim(UCase(cbo_Grid_Loom_No.Text)) Then
                    '    'dgv_Details.Tag = .CurrentCell.Value
                    '    Get_Cloth_Name_From_Loom_Creation()
                    'End If


                    dgv_Details.Tag = .CurrentCell.Value



                End If

            End With

        Catch ex As Exception
            '-----
        End Try

    End Sub

    Private Sub dgv_Details_EditingControlShowing(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewEditingControlShowingEventArgs) Handles dgv_Details.EditingControlShowing
        dgtxt_Details = CType(dgv_Details.EditingControl, DataGridViewTextBoxEditingControl)
    End Sub


    Private Sub dgv_Details_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_Details.KeyUp

        If e.Control = True And UCase(Chr(e.KeyCode)) = "D" Then

            With dgv_Details

                If .CurrentRow.Index = .RowCount - 1 Then
                    For i = 1 To .Columns.Count - 1
                        .Rows(.CurrentRow.Index).Cells(i).Value = ""
                    Next

                Else
                    .Rows.RemoveAt(.CurrentRow.Index)

                End If

                Total_Calculation()

            End With

        End If

    End Sub

    Private Sub cbo_Grid_Shift_1_Employee_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Employee.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "PayRoll_Employee_Head", "Employee_Name", "(date_status<>1)", "(Employee_IdNo = 0)")
        cbo_Employee.Tag = cbo_Employee.Text
    End Sub

    Private Sub cbo_Grid_Shift_1_Employee_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Employee.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Employee, dtp_Date, cbo_Shift, "PayRoll_Employee_Head", "Employee_Name", "(date_status<>1)", "(Employee_IdNo = 0)")
    End Sub

    Private Sub cbo_Grid_Shift_1_Employee_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Employee.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Employee, cbo_Shift, "PayRoll_Employee_Head", "Employee_Name", "(date_status<>1)", "(Employee_IdNo = 0)")
    End Sub
    Private Sub cbo_Grid_Shift_1_Employee_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Employee.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Dim f As New Payroll_Employee_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Employee.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If

    End Sub

    Private Sub Cbo_Grid_Cloth_Name_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles Cbo_Grid_Cloth_Name.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Cloth_head", "Cloth_Name", "(Close_Status=0)", "(Cloth_Idno = 0)")
    End Sub
    Private Sub Cbo_Grid_Cloth_Name_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Cbo_Grid_Cloth_Name.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, Cbo_Grid_Cloth_Name, Nothing, Nothing, "Cloth_head", "Cloth_Name", "(Close_Status=0)", "(Cloth_Idno = 0)")

        With dgv_Details

            If (e.KeyValue = 38 And Cbo_Grid_Cloth_Name.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
                .Focus()
                .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex - 1)
            End If

            If (e.KeyValue = 40 And Cbo_Grid_Cloth_Name.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
                .Focus()
                dgv_Details.CurrentCell = dgv_Details.Rows(dgv_Details.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 1)
            End If

        End With
    End Sub

    Private Sub Cbo_Grid_Cloth_Name_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Cbo_Grid_Cloth_Name.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, Cbo_Grid_Cloth_Name, Nothing, "Cloth_head", "Cloth_Name", "(Close_Status=0)", "(Cloth_Idno = 0)")
        If Asc(e.KeyChar) = 13 Then
            With dgv_Details
                .Focus()
                dgv_Details.CurrentCell = dgv_Details.Rows(dgv_Details.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 1)

            End With

        End If
    End Sub


    Private Sub Cbo_Grid_Cloth_Name_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Cbo_Grid_Cloth_Name.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Dim f As New Cloth_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = Cbo_Grid_Cloth_Name.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If
    End Sub

    Private Sub Cbo_Grid_Cloth_Name_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles Cbo_Grid_Cloth_Name.TextChanged
        Try
            If FrmLdSTS = True Then Exit Sub

            If Cbo_Grid_Cloth_Name.Visible Then
                With dgv_Details

                    If IsNothing(dgv_Details.CurrentCell) = True Then Exit Sub

                    If Val(Cbo_Grid_Cloth_Name.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = dgvCol_Details.CLOTH_NAME Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(Cbo_Grid_Cloth_Name.Text)
                    End If
                End With
            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub Total_Calculation()
        Dim Sno As Integer
        Dim vTotPrd_Mtr As String, vTotAMount As String, vDaMage_Pcs As String, vcoolie_mtr As String, vtot_Lms As String
        Dim tot_Type1_Meters As String = 0
        Dim tot_damage_Meters As String = 0


        Sno = 0


        vTotPrd_Mtr = 0 : vTotAMount = 0 : vDaMage_Pcs = 0 : vcoolie_mtr = 0 : vtot_Lms = 0 : tot_Type1_Meters = 0 : tot_damage_Meters = 0

        With dgv_Details
            For i = 0 To .Rows.Count - 1

                Sno = Sno + 1
                .Rows(i).Cells(0).Value = Val(Sno)

                If Trim(.Rows(i).Cells(dgvCol_Details.LOOM_NO).Value) <> "" Then

                    vtot_Lms = vtot_Lms + 1

                    vTotPrd_Mtr = vTotPrd_Mtr + Val(.Rows(i).Cells(dgvCol_Details.PRODUCTION_METER).Value)
                    vDaMage_Pcs = vDaMage_Pcs + Val(.Rows(i).Cells(dgvCol_Details.DAMAGE_PCS).Value)
                    tot_damage_Meters = tot_damage_Meters + Val(.Rows(i).Cells(dgvCol_Details.DAMAGE_METER).Value)
                    tot_Type1_Meters = tot_Type1_Meters + Val(.Rows(i).Cells(dgvCol_Details.SOUND_METER).Value)
                    vTotAMount = vTotAMount + Val(.Rows(i).Cells(dgvCol_Details.AMOUNT).Value)

                End If

            Next

        End With

        With dgv_Details_Total
            If .RowCount = 0 Then .Rows.Add()

            .Rows(0).Cells(dgvCol_Details.LOOM_NO).Value = Format(Val(vtot_Lms), "########0")
            .Rows(0).Cells(dgvCol_Details.PRODUCTION_METER).Value = Format(Val(vTotPrd_Mtr), "########0.00")
            .Rows(0).Cells(dgvCol_Details.DAMAGE_PCS).Value = Format(Val(vDaMage_Pcs), "########0.00")
            .Rows(0).Cells(dgvCol_Details.DAMAGE_METER).Value = Format(Val(tot_damage_Meters), "########0.00")
            .Rows(0).Cells(dgvCol_Details.SOUND_METER).Value = Format(Val(tot_Type1_Meters), "########0.00")
            .Rows(0).Cells(dgvCol_Details.AMOUNT).Value = Format(Val(vTotAMount), "########0.00")

        End With

    End Sub


    Private Sub btn_Filter_Close_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Filter_Close.Click
        pnl_Back.Enabled = True
        pnl_Filter.Visible = False
        Filter_Status = False
    End Sub

    Private Sub btn_Filter_Show_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Filter_Show.Click
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim n As Integer
        Dim Led_IdNo As Integer, Loom_IdNo As Integer, Clo_IdNo As Integer
        Dim Condt As String = ""

        Try

            Condt = ""
            Led_IdNo = 0
            Clo_IdNo = 0

            If IsDate(dtp_Filter_Fromdate.Value) = True And IsDate(dtp_Filter_ToDate.Value) = True Then
                Condt = "a.Payroll_Employee_Production_Date between '" & Trim(Format(dtp_Filter_Fromdate.Value, "MM/dd/yyyy")) & "' and '" & Trim(Format(dtp_Filter_ToDate.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_Filter_Fromdate.Value) = True Then
                Condt = "a.Payroll_Employee_Production_Date = '" & Trim(Format(dtp_Filter_Fromdate.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_Filter_ToDate.Value) = True Then
                Condt = "a.Payroll_Employee_Production_Date = '" & Trim(Format(dtp_Filter_ToDate.Value, "MM/dd/yyyy")) & "' "
            End If

            If Trim(cbo_Filter_EmployeeName.Text) <> "" Then
                Led_IdNo = Common_Procedures.Employee_NameToIdNo(con, cbo_Filter_EmployeeName.Text)
            End If
            If Trim(cbo_Filter_Quality.Text) <> "" Then
                Clo_IdNo = Common_Procedures.Cloth_NameToIdNo(con, cbo_Filter_Quality.Text)
            End If
            If Trim(Cbo_Filter_Loomno.Text) <> "" Then
                Loom_IdNo = Common_Procedures.Loom_NameToIdNo(con, Cbo_Filter_Loomno.Text)
            End If




            If Val(Led_IdNo) <> 0 Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " a.Payroll_Employee_Production_Code IN ( select z1.Payroll_Employee_Production_Code from Textile_Payroll_Employee_Production_Details z1 where z1.Employee_Idno = " & Str(Val(Led_IdNo)) & " )"
            End If

            If Val(Clo_IdNo) <> 0 Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " a.Payroll_Employee_Production_Code IN ( select z1.Payroll_Employee_Production_Code from Textile_Payroll_Employee_Production_Details z1 where z1.Cloth_Idno = " & Str(Val(Clo_IdNo)) & " )"
            End If

            If Val(Loom_IdNo) <> 0 Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " a.Payroll_Employee_Production_Code IN ( select z1.Payroll_Employee_Production_Code from Textile_Payroll_Employee_Production_Details z1 where z1.Loom_Idno = " & Str(Val(Loom_IdNo)) & " )"
            End If



            da = New SqlClient.SqlDataAdapter("select a.* from Textile_Payroll_Employee_Production_Head a  where a.company_idno =" & Str(Val(lbl_Company.Tag)) & "and a.Payroll_Employee_Production_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' " & IIf(Trim(Condt) <> "", " and ", "") & Condt & " Order by a.Payroll_Employee_Production_Date, a.for_orderby, a.Payroll_Employee_Production_no", con)
            'da = New SqlClient.SqlDataAdapter("select a.*, e.Ledger_Name from Rewinding_Delivery_Head a left outer join Rewinding_Delivery_Details b on a.Rewinding_Delivery_Code = b.Rewinding_Delivery_Code left outer join Ledger_head e on a.Ledger_idno = e.Ledger_idno where a.company_idno =" & Str(Val(lbl_Company.Tag)) & "and a.Rewinding_Delivery_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' " & IIf(Trim(Condt) <> "", " and ", "") & Condt & " Order by a.Rewinding_Delivery_Date, a.for_orderby, a.Rewinding_Delivery_No", con)
            da.Fill(dt2)

            dgv_Filter_Details.Rows.Clear()

            If dt2.Rows.Count > 0 Then

                For i = 0 To dt2.Rows.Count - 1

                    n = dgv_Filter_Details.Rows.Add()

                    dgv_Filter_Details.Rows(n).Cells(0).Value = i + 1
                    dgv_Filter_Details.Rows(n).Cells(1).Value = dt2.Rows(i).Item("Payroll_Employee_Production_no").ToString
                    dgv_Filter_Details.Rows(n).Cells(2).Value = Format(Convert.ToDateTime(dt2.Rows(i).Item("Payroll_Employee_Production_Date").ToString), "dd-MM-yyyy")

                    dgv_Filter_Details.Rows(n).Cells(3).Value = Val(dt2.Rows(i).Item("Total_No_Of_Looms").ToString)
                    dgv_Filter_Details.Rows(n).Cells(4).Value = Format(Val(dt2.Rows(i).Item("Total_Production_Meter").ToString), "########0.00")
                    dgv_Filter_Details.Rows(n).Cells(5).Value = Format(Val(dt2.Rows(i).Item("Total_Amount").ToString), "########0.00")

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


    Private Sub dtp_Filter_Fromdate_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dtp_Filter_Fromdate.KeyPress
        If Asc(e.KeyChar) = 13 Then
            dtp_Filter_ToDate.Focus()
        End If

    End Sub

    Private Sub dtp_Filter_ToDate_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dtp_Filter_ToDate.KeyPress
        If Asc(e.KeyChar) = 13 Then
            cbo_Filter_EmployeeName.Focus()
        End If
    End Sub

    Private Sub cbo_Filter_Quality_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Filter_Quality.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Cloth_head", "Cloth_Name", "(Close_Status=0)", "(Cloth_Idno = 0)")

    End Sub

    Private Sub cbo_Filter_Quality_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Filter_Quality.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_Quality, cbo_Filter_EmployeeName, Cbo_Filter_Loomno, "Cloth_head", "Cloth_Name", "(Close_Status=0)", "(Cloth_Idno = 0)")

    End Sub

    Private Sub cbo_Filter_Quality_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Filter_Quality.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Filter_Quality, Cbo_Filter_Loomno, "Cloth_head", "Cloth_Name", "(Close_Status=0)", "(Cloth_Idno = 0)")
    End Sub

    Private Sub cbo_Filter_EmployeeName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Filter_EmployeeName.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "PayRoll_Employee_Head", "Employee_Name", "(date_status<>1)", "(Employee_IdNo = 0)")

    End Sub


    Private Sub cbo_Filter_EmployeeName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Filter_EmployeeName.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_EmployeeName, dtp_Filter_ToDate, cbo_Filter_Quality, "PayRoll_Employee_Head", "Employee_Name", "(date_status<>1)", "(Employee_IdNo = 0)")

    End Sub

    Private Sub cbo_Filter_EmployeeName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Filter_EmployeeName.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Filter_EmployeeName, cbo_Filter_Quality, "PayRoll_Employee_Head", "Employee_Name", "(date_status<>1)", "(Employee_IdNo = 0)")

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




    Private Sub btn_save_Click(sender As System.Object, e As System.EventArgs) Handles btn_save.Click
        save_record()

    End Sub

    Private Sub dgtxt_Details_TextChanged(sender As Object, e As System.EventArgs) Handles dgtxt_Details.TextChanged
        Try
            With dgv_Details
                If .Rows.Count > 0 Then
                    .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(dgtxt_Details.Text)
                End If
            End With

        Catch ex As Exception
            '---

        End Try
    End Sub

    Private Sub btn_close_Click(sender As System.Object, e As System.EventArgs) Handles btn_close.Click
        Me.Close()

    End Sub


    Private Sub cbo_Grid_Loom_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Grid_Loom_No.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Loom_HEAD", "Loom_name", "", "(Loom_idno = 0)")
        'cbo_Grid_Loom_No.Tag = cbo_Grid_Loom_No.Text
    End Sub

    Private Sub cbo_Grid_Loom_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Grid_Loom_No.KeyDown

        vcbo_KeyDwnVal = e.KeyValue

        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Grid_Loom_No, Nothing, Nothing, "Loom_HEAD", "Loom_name", "", "(Loom_idno = 0)")

        With dgv_Details

            If (e.KeyValue = 38 And cbo_Grid_Loom_No.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
                e.Handled = True
                If .CurrentRow.Index <= 0 Then
                    cbo_Shift.Focus()

                Else
                    .Focus()
                    .CurrentCell = dgv_Details.Rows(dgv_Details.CurrentRow.Index - 1).Cells(dgvCol_Details.PRODUCTION_METER)

                End If

            End If


            If (e.KeyValue = 40 And cbo_Grid_Loom_No.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
                e.Handled = True
                If Trim(.Rows(.CurrentRow.Index).Cells(dgvCol_Details.LOOM_NO).Value) = "" And .CurrentRow.Index = .Rows.Count - 1 Then
                    If MessageBox.Show("Do you want to save", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = DialogResult.Yes Then
                        save_record()
                    Else
                        msk_date.Focus()
                    End If

                Else
                    .Focus()
                    .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(dgvCol_Details.PRODUCTION_METER)

                End If
            End If

        End With
    End Sub

    Private Sub cbo_Grid_Loom_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Grid_Loom_No.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Grid_Loom_No, Nothing, "Loom_HEAD", "Loom_name", "", "(Loom_idno = 0)")

        If Asc(e.KeyChar) = 13 Then

            With dgv_Details

                If Trim(.Rows(.CurrentRow.Index).Cells(dgvCol_Details.LOOM_NO).Value) = "" And .CurrentRow.Index = .Rows.Count - 1 Then
                    If MessageBox.Show("Do you want to save", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = DialogResult.Yes Then
                        save_record()
                    Else
                        msk_date.Focus()
                    End If

                Else

                    .Focus()
                    .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(dgvCol_Details.PRODUCTION_METER)

                End If

            End With

        End If

    End Sub

    Private Sub cbo_Grid_Loom_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Grid_Loom_No.KeyUp
        'If e.Control = True And UCase(Chr(e.KeyCode)) = "D" Then
        '    dgv_Details_KeyUp(sender, e)
        'End If
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Dim f As New LoomNo_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Grid_Loom_No.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()
        End If
    End Sub



    Private Sub cbo_Grid_Loom_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Grid_Loom_No.TextChanged
        Try
            If cbo_Grid_Loom_No.Visible Then

                If IsNothing(dgv_Details.CurrentCell) Then Exit Sub
                With dgv_Details
                    If Val(cbo_Grid_Loom_No.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = dgvCol_Details.LOOM_NO Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_Grid_Loom_No.Text)
                    End If
                End With
                'If Trim(UCase(dgv_Details.Tag)) <> Trim(UCase(dgv_Details.CurrentCell.Value)) Then
                '    dgv_Details.Tag = dgv_Details.CurrentCell.Value
                '    Get_Cloth_Name_From_Loom_Creation()
                'End If
            End If


        Catch ex As Exception
            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub
    Private Sub cbo_Grid_Shift_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Shift.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Shift_Head", "Shift_Name", "", "(Shift_IdNo = 0)")
        cbo_Shift.Tag = cbo_Shift.Text
    End Sub

    Private Sub cbo_Grid_Shift_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Shift.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Shift, cbo_Employee, Nothing, "Shift_Head", "Shift_Name", "", "(Shift_IdNo = 0)")
        vcbo_KeyDwnVal = e.KeyValue

        With dgv_Details
            If (e.KeyValue = 40 And cbo_Shift.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then

                If Trim(UCase(msk_date.Text)) <> Trim(UCase(msk_date.Tag)) Or Trim(UCase(cbo_Employee.Text)) <> Trim(UCase(cbo_Employee.Tag)) Or Trim(UCase(cbo_Shift.Text)) <> Trim(UCase(cbo_Shift.Tag)) Then
                    Check_and_Get_LoomNo_List(sender)
                End If

                With dgv_Details

                    If IsNothing(.CurrentCell) Then Exit Sub

                    If .Visible And .RowCount > 0 Then

                        .Focus()

                        .CurrentCell = .Rows(0).Cells(dgvCol_Details.LOOM_NO)
                        .CurrentCell.Selected = True
                        cbo_Grid_Loom_No.Focus()
                        cbo_Grid_Loom_No.BringToFront()
                        cbo_Grid_Loom_No.BackColor = Color.Lime

                    End If

                End With
            End If

        End With

    End Sub

    Private Sub cbo_Grid_Shift_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Shift.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Shift, Nothing, "Shift_Head", "Shift_Name", "", "(Shift_IdNo = 0)")

        If Asc(e.KeyChar) = 13 Then


            'If Trim(UCase(msk_date.Text)) <> Trim(UCase(msk_date.Tag)) Or Trim(dgv_Details.Rows(0).Cells(1).Value) = "" Then
            '    Check_and_Get_LoomNo_List(sender)
            'End If

            With dgv_Details

                If IsNothing(.CurrentCell) Then Exit Sub

                If .Visible And .RowCount > 0 Then

                    .Focus()

                    .CurrentCell = .Rows(0).Cells(dgvCol_Details.LOOM_NO)
                    .CurrentCell.Selected = True
                    cbo_Grid_Loom_No.Focus()
                    cbo_Grid_Loom_No.BringToFront()
                    cbo_Grid_Loom_No.BackColor = Color.Lime


                End If

            End With

        End If

    End Sub
    Private Sub dgtxt_Details_KeyPress(sender As Object, e As KeyPressEventArgs) Handles dgtxt_Details.KeyPress
        If cbo_Shift.Visible Then
            With dgv_Details
                If .CurrentCell.ColumnIndex <> dgvCol_Details.LOOM_NO And .CurrentCell.ColumnIndex <> dgvCol_Details.CLOTH_NAME Then
                    If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
                        e.Handled = True
                    End If
                End If
            End With
        End If
    End Sub
    Private Sub msk_date_KeyPress(sender As Object, e As KeyPressEventArgs) Handles msk_date.KeyPress
        If Asc(e.KeyChar) = 13 Then

            cbo_Employee.Focus()
        End If
    End Sub
    Private Sub Cbo_Filter_Loomno_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles Cbo_Filter_Loomno.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Loom_HEAD", "Loom_name", "", "(Loom_idno = 0)")
    End Sub
    Private Sub Cbo_Filter_Loomno_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Cbo_Filter_Loomno.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, Cbo_Filter_Loomno, cbo_Filter_Quality, btn_Filter_Show, "Loom_HEAD", "Loom_name", "", "(Loom_idno = 0)")
    End Sub
    Private Sub Cbo_Filter_Loomno_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Cbo_Filter_Loomno.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, Cbo_Filter_Loomno, btn_Filter_Show, "Loom_HEAD", "Loom_name", "", "(Loom_idno = 0)")
    End Sub
    Private Sub dtp_Filter_Fromdate_KeyDown(sender As Object, e As KeyEventArgs) Handles dtp_Filter_Fromdate.KeyDown
        If e.KeyCode = 40 Then
            dtp_Filter_ToDate.Focus()
        End If
    End Sub
    Private Sub dtp_Filter_ToDate_KeyDown(sender As Object, e As KeyEventArgs) Handles dtp_Filter_ToDate.KeyDown
        If e.KeyCode = 40 Then
            cbo_Filter_EmployeeName.Focus()
        ElseIf e.KeyCode = 38 Then
            dtp_Filter_Fromdate.Focus()
        End If
    End Sub

    Private Sub btn_Selection_Click(sender As Object, e As EventArgs) Handles btn_List_LoomDetails.Click
        Check_and_Get_LoomNo_List(sender)
    End Sub
    Private Sub Check_and_Get_LoomNo_List(sender As System.Object)
        Dim Cmd As New SqlClient.SqlCommand
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim movno As String = ""
        Dim NewCode As String = ""
        Dim Cat_ID As Integer = 0

        Dim EMP_ID = 0
        Dim Shift_Id = 0
        Try


            If IsDate(msk_date.Text) = False Then
                MessageBox.Show("Invalid Date", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                If msk_date.Enabled And msk_date.Visible Then msk_date.Focus()
                Exit Sub
            End If


            If Not (Convert.ToDateTime(msk_date.Text) >= Common_Procedures.Company_FromDate And Convert.ToDateTime(msk_date.Text) <= Common_Procedures.Company_ToDate) Then
                MessageBox.Show("Date is out of financial range", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                If msk_date.Enabled And msk_date.Visible Then msk_date.Focus()
                Exit Sub
            End If

            EMP_ID = Common_Procedures.Employee_NameToIdNo(con, cbo_Employee.Text)
            Shift_Id = Common_Procedures.Shift_NameToIdNo(con, cbo_Shift.Text)

            If EMP_ID = 0 Then
                MessageBox.Show("Invalid Employee Name", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                cbo_Employee.Focus()
                Exit Sub
            End If

            If Shift_Id = 0 Then
                MessageBox.Show("Invalid Shift", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                cbo_Shift.Focus()
                Exit Sub
            End If


            Cmd.Connection = con

            Cmd.Parameters.Clear()
            Cmd.Parameters.AddWithValue("@EntryDate", Convert.ToDateTime(msk_date.Text))

            NewCode = Trim(Pk_Condition) & Val(lbl_Company.Tag) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            Cmd.CommandText = "Select Payroll_Employee_Production_NO from Textile_Payroll_Employee_Production_Head Where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Payroll_Employee_Production_dATE = @EntryDate and Employee_Idno = " & Str(Val(EMP_ID)) & " and Shift_Idno = " & Str(Val(Shift_Id)) & "  and Payroll_Employee_Production_Code <> '" & Trim(NewCode) & "' Order by Payroll_Employee_Production_dATE, for_orderby, Payroll_Employee_Production_No"
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
                End If
            End If



            msk_date.Tag = msk_date.Text

        Catch ex As Exception

        End Try
    End Sub

    Private Sub msk_date_GotFocus(sender As Object, e As EventArgs) Handles msk_date.GotFocus
        msk_date.Tag = msk_date.Text
    End Sub
    Public Sub Amount_Calculation(vCurrow As Integer, vCurCol As Integer)
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim movno As String = ""
        Dim NewCode As String = ""
        Dim Coolie_Amt As String = ""
        Dim Cloth_ID As Integer = 0
        Dim Mtr_Pcs = ""
        Dim StkIn_For = ""
        Dim Damage_Meter = ""
        Dim Emp_Wages = ""

        If FrmLdSTS = True Or NoCalc_Status = True Or Mov_Status = True Then Exit Sub


        With dgv_Details

            If IsNothing(.CurrentCell) Then Exit Sub

            If .RowCount > 0 Then

                Mtr_Pcs = 0
                StkIn_For = ""
                Damage_Meter = 0
                Emp_Wages = 0

                ' --- GET CLOTH  DAMAGE METERS_PCS

                If vCurCol = dgvCol_Details.CLOTH_NAME Or vCurCol = dgvCol_Details.PRODUCTION_METER Or vCurCol = dgvCol_Details.DAMAGE_PCS Or vCurCol = dgvCol_Details.SOUND_METER Or vCurCol = dgvCol_Details.COOLIE_METER Then


                    '    If vCurCol = dgvCol_Details.PRODUCTION_METER Or vCurCol = dgvCol_Details.DAMAGE_PCS Then

                    If Trim(.CurrentRow.Cells(dgvCol_Details.CLOTH_NAME).Value) <> "" Then

                        Cloth_ID = Common_Procedures.Cloth_NameToIdNo(con, .CurrentRow.Cells(dgvCol_Details.CLOTH_NAME).Value)


                        Mtr_Pcs = 0
                        StkIn_For = ""
                        Damage_Meter = 0

                        Da = New SqlClient.SqlDataAdapter("Select Stock_In , Meters_Pcs ,Employee_Wages_Per_Meter from Cloth_Head Where Cloth_IdNo = " & Str(Val(Cloth_ID)), con)
                        dt2 = New DataTable
                        Da.Fill(dt2)
                        If dt2.Rows.Count > 0 Then
                            StkIn_For = dt2.Rows(0)("Stock_In").ToString
                            Mtr_Pcs = Val(dt2.Rows(0)("Meters_Pcs").ToString)
                            Emp_Wages = Val(dt2.Rows(0)("Employee_Wages_Per_Meter").ToString)
                        End If
                        dt2.Clear()

                    End If


                    If Trim(UCase(StkIn_For)) = "PCS" And Val(Mtr_Pcs) <> 0 Then
                        If Val(.Rows(vCurrow).Cells(dgvCol_Details.DAMAGE_PCS).Value) <> 0 Then
                            Damage_Meter = Format(Val(.Rows(vCurrow).Cells(dgvCol_Details.DAMAGE_PCS).Value) * Val(Mtr_Pcs), "#########0.00")
                        End If
                    End If

                    .Rows(vCurrow).Cells(dgvCol_Details.DAMAGE_METER).Value = Format(Val(Damage_Meter), "#######0.00")

                    ' --- SOUND METERS

                    If Val(.Rows(vCurrow).Cells(dgvCol_Details.PRODUCTION_METER).Value) <> 0 Or Val(.Rows(vCurrow).Cells(dgvCol_Details.DAMAGE_METER).Value) <> 0 Then

                        .Rows(vCurrow).Cells(dgvCol_Details.SOUND_METER).Value = Format(Val(.Rows(vCurrow).Cells(dgvCol_Details.PRODUCTION_METER).Value) - Val(.Rows(vCurrow).Cells(dgvCol_Details.DAMAGE_METER).Value), "#######0.00")

                    End If

                    '   End If

                    ' --- COOLIE RATE

                    If .CurrentRow.Cells(dgvCol_Details.COOLIE_METER).Value = 0 Then
                        .CurrentRow.Cells(dgvCol_Details.COOLIE_METER).Value = Format(Val(Emp_Wages), "########0.00")
                    End If

                    ' --- COOLIE AMOUNT 

                    Coolie_Amt = 0

                    If Val(.Rows(vCurrow).Cells(dgvCol_Details.COOLIE_METER).Value) <> 0 And Val(.Rows(vCurrow).Cells(dgvCol_Details.SOUND_METER).Value) <> 0 Then
                        Coolie_Amt = (Val(.Rows(vCurrow).Cells(dgvCol_Details.SOUND_METER).Value) * Val(.Rows(vCurrow).Cells(dgvCol_Details.COOLIE_METER).Value))
                    End If

                    .Rows(vCurrow).Cells(dgvCol_Details.AMOUNT).Value = Format(Val(Coolie_Amt), "#######0.00")


                End If
            End If


            Total_Calculation()

        End With

    End Sub

    Private Sub cbo_Shift_Leave(sender As Object, e As EventArgs) Handles cbo_Shift.Leave

        If Trim(UCase(msk_date.Text)) <> Trim(UCase(msk_date.Tag)) Or Trim(UCase(cbo_Employee.Text)) <> Trim(UCase(cbo_Employee.Tag)) Or Trim(UCase(cbo_Shift.Text)) <> Trim(UCase(cbo_Shift.Tag)) Then
            Check_and_Get_LoomNo_List(sender)
        End If

    End Sub
    Private Sub Get_Cloth_Name_From_Loom_Creation()

        If IsNothing(dgv_Details.CurrentCell) Then Exit Sub
        If dgv_Details.RowCount = 0 Then Exit Sub

        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim Loom_ID As Integer = 0
        Dim Cloth_Name As String = ""

        Loom_ID = 0
        Cloth_Name = ""


        Try

            If Trim(dgv_Details.Rows(dgv_Details.CurrentRow.Index).Cells(dgvCol_Details.LOOM_NO).Value) <> "" Then


                Loom_ID = Common_Procedures.Loom_NameToIdNo(con, dgv_Details.Rows(dgv_Details.CurrentRow.Index).Cells(dgvCol_Details.LOOM_NO).Value)

                If Loom_ID = 0 Then Exit Sub

                Da = New SqlClient.SqlDataAdapter("Select b.Cloth_Name From Loom_Head a LEFT OUTER JOIN Cloth_Head b on a.Cloth_Idno = b.Cloth_Idno where Loom_Idno = " & Str(Val(Loom_ID)) & " ", con)
                Dt = New DataTable
                Da.Fill(Dt)

                Cloth_Name = ""
                If Dt.Rows.Count > 0 Then
                    Cloth_Name = Dt.Rows(0)(0).ToString
                End If
                dgv_Details.Rows(dgv_Details.CurrentRow.Index).Cells(dgvCol_Details.CLOTH_NAME).Value = Trim(Cloth_Name)

            End If


        Catch ex As Exception
            MessageBox.Show(ex.Message, "GET CLOTH NAME FROM LOOM")
        End Try

    End Sub
    Private Sub Get_Loom_No_From_Loom_Master()
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable

        Dim Loom_Id As Integer = 0
        Dim vNxt_Loom_No As String = ""

        If IsNothing(dgv_Details.CurrentCell) Then Exit Sub
        If dgv_Details.RowCount = 0 Then Exit Sub


        vNxt_Loom_No = ""
        Loom_Id = 0

        Try
            With dgv_Details

                Loom_Id = Common_Procedures.Loom_NameToIdNo(con, Trim(dgv_Details.Rows(dgv_Details.CurrentRow.Index - 1).Cells(dgvCol_Details.LOOM_NO).Value))

                If Loom_Id = 0 Then
                    MessageBox.Show("Invalid Previous Loom No", "DOES NOT GET NEXT LOOM NO...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Exit Sub
                End If


                Da = New SqlClient.SqlDataAdapter(" select top 1  Loom_name from Loom_head where LmNo_OrderBy >  " & Val(Loom_Id) & " Order by LmNo_OrderBy  ", con)
                Dt = New DataTable
                Da.Fill(Dt)


                vNxt_Loom_No = ""

                If Dt.Rows.Count > 0 Then

                    vNxt_Loom_No = Dt.Rows(0)(0).ToString

                End If

                .Rows(.CurrentCell.RowIndex).Cells(dgvCol_Details.LOOM_NO).Value = Trim(vNxt_Loom_No)


            End With

            Get_Cloth_Name_From_Loom_Creation()

        Catch ex As Exception

            MessageBox.Show(ex.Message, "GET LOOM NAME")

        End Try

    End Sub

End Class