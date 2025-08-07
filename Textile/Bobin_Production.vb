Public Class Bobin_Production

    Implements Interface_MDIActions

    Private con As New SqlClient.SqlConnection(Common_Procedures.Connection_String)
    Private FrmLdSTS As Boolean = False
    Private New_Entry As Boolean = False
    Private Insert_Entry As Boolean = False
    Private Filter_Status As Boolean = False
    Private Pk_Condition As String = "BBNPD-"
    Private Prec_ActCtrl As New Control
    Private vcbo_KeyDwnVal As Double

    Private WithEvents dgtxt_BobinDetails As New DataGridViewTextBoxEditingControl
    Private dgv_ActCtrlName As String = ""
    Private dgv_LevColNo As Integer
    Public vmskOldText As String = ""
    Public vmskSelStrt As Integer = -1

    Public Sub New()
        ' This call is required by the designer.
        InitializeComponent()
        ' Add any initialization after the InitializeComponent() call.
    End Sub

    Private Sub clear()
        New_Entry = False
        Insert_Entry = False

        pnl_back.Enabled = True
        pnl_filter.Visible = False

        vmskOldText = ""
        vmskSelStrt = -1

        lbl_RefNo.Text = ""
        lbl_RefNo.ForeColor = Color.Black
        dtp_Date.Text = ""
        msk_Date.Text = ""
        cbo_employee.Text = ""
        cbo_PartyName.Text = ""
        cbo_Colour.Text = ""
        Cbo_EndsCount.Text = ""
        txt_Meters.Text = ""
        txt_No_Bobins.Text = ""
        txt_Salary.Text = ""
        txt_SalaryBobin.Text = ""
        txt_PartyBobin.Text = ""
        txt_OurOwnBobin.Text = ""
        txt_Meter_Bobin.Text = ""
        cbo_BobinSize.Text = ""

        dgv_BobinDetails.Rows.Clear()
        dgv_BobinDetails_Total.Rows.Clear()

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1116" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1380" Then '---- St.LOURDU MATHA TEX (Somanur)
            txt_ReelWeight.Text = "0.052"
            txt_Meter_Reel.Text = "3000"
        Else
            txt_ReelWeight.Text = ""
            txt_Meter_Reel.Text = ""
        End If

        Grid_DeSelect()

        cbo_BobinCount.Visible = False
        cbo_BobinCount.Tag = -1
        cbo_BobinColour.Visible = False
        cbo_BobinColour.Tag = -1

        cbo_BobinCount.Text = ""
        cbo_BobinColour.Text = ""


        dgv_ActCtrlName = ""
    End Sub

    Private Sub Grid_DeSelect()
        On Error Resume Next
        If Not IsNothing(dgv_BobinDetails.CurrentCell) Then dgv_BobinDetails.CurrentCell.Selected = False
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

        If Me.ActiveControl.Name <> cbo_BobinColour.Name Then
            cbo_BobinColour.Visible = False
        End If
        If Me.ActiveControl.Name <> cbo_BobinColour.Name Then
            cbo_BobinColour.Visible = False
        End If
        If Me.ActiveControl.Name <> dgv_BobinDetails.Name Then
            Grid_DeSelect()
        End If

        Prec_ActCtrl = Me.ActiveControl

    End Sub

    Private Sub ControlLostFocus(ByVal sender As Object, ByVal e As System.EventArgs)

        On Error Resume Next

        If IsNothing(Prec_ActCtrl) = False Then
            If TypeOf Prec_ActCtrl Is TextBox Or TypeOf Prec_ActCtrl Is ComboBox Or TypeOf Prec_ActCtrl Is MaskedTextBox Then
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

    Private Sub Grid_Cell_DeSelect()
        On Error Resume Next
        If Not IsNothing(dgv_filter.CurrentCell) Then dgv_filter.CurrentCell.Selected = False
        If Not IsNothing(dgv_BobinDetails.CurrentCell) Then dgv_BobinDetails.CurrentCell.Selected = False
        If Not IsNothing(dgv_BobinDetails_Total.CurrentCell) Then dgv_BobinDetails_Total.CurrentCell.Selected = False

    End Sub

    Protected Overrides Function ProcessCmdKey(ByRef msg As System.Windows.Forms.Message, ByVal keyData As System.Windows.Forms.Keys) As Boolean
        Dim dgv1 As New DataGridView

        On Error Resume Next

        If ActiveControl.Name = dgv_BobinDetails.Name Or TypeOf ActiveControl Is DataGridViewTextBoxEditingControl Then

            dgv1 = Nothing

            If ActiveControl.Name = dgv_BobinDetails.Name Then
                dgv1 = dgv_BobinDetails

            ElseIf dgv_BobinDetails.IsCurrentRowDirty = True Then
                dgv1 = dgv_BobinDetails
            ElseIf pnl_back.Enabled = True Then
                dgv1 = dgv_BobinDetails

            End If

            If IsNothing(dgv1) = False Then

                With dgv1


                    If keyData = Keys.Enter Or keyData = Keys.Down Then
                        If .CurrentCell.ColumnIndex >= .ColumnCount - 2 Then
                            If .CurrentCell.RowIndex = .RowCount - 1 Then
                                txt_Salary.Focus()

                            Else
                                .CurrentCell = .Rows(.CurrentCell.RowIndex + 1).Cells(1)

                            End If

                        Else
                            .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 1)

                        End If

                        Return True

                    ElseIf keyData = Keys.Up Then

                        If .CurrentCell.ColumnIndex <= 1 Then
                            If .CurrentCell.RowIndex = 0 Then
                                btn_save.Focus()

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

                End With

            Else

                Return MyBase.ProcessCmdKey(msg, keyData)

            End If

        Else

            Return MyBase.ProcessCmdKey(msg, keyData)

        End If

    End Function

    Private Sub move_record(ByVal no As String)
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt2 As New DataTable
        Dim NewCode As String
        Dim LockSTS As Boolean = False
        Dim Sno As Integer = 0
        Dim n As Integer = 0

        If Val(no) = 0 Then Exit Sub

        clear()

        Try

            NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(no) & "/" & Trim(Common_Procedures.FnYearCode)

            da1 = New SqlClient.SqlDataAdapter("select a.* from Bobin_Production_Head a  Where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Bobin_Production_code = '" & Trim(NewCode) & "'", con)
            dt1 = New DataTable
            da1.Fill(dt1)

            If dt1.Rows.Count > 0 Then

                lbl_RefNo.Text = dt1.Rows(0).Item("Bobin_Production_No").ToString
                dtp_Date.Text = dt1.Rows(0).Item("Bobin_Production_Date").ToString
                msk_Date.Text = dtp_Date.Text
                cbo_employee.Text = Common_Procedures.Employee_Simple_IdNoToName(con, dt1.Rows(0).Item("Employee_IdNo").ToString)
                cbo_PartyName.Text = Common_Procedures.Ledger_IdNoToName(con, dt1.Rows(0).Item("Ledger_IdNo").ToString)
                cbo_Colour.Text = Common_Procedures.Colour_IdNoToName(con, dt1.Rows(0).Item("Colour_IdNo").ToString)
                Cbo_EndsCount.Text = Common_Procedures.EndsCount_IdNoToName(con, dt1.Rows(0).Item("EndsCount_IdNo").ToString)
                txt_No_Bobins.Text = Val(dt1.Rows(0).Item("No_Of_Bobin").ToString)
                txt_Meters.Text = Format(Val(dt1.Rows(0).Item("Meters").ToString), "########0.00")
                txt_SalaryBobin.Text = Format(Val(dt1.Rows(0).Item("Salary_Bobin").ToString), "########0.00")
                txt_Salary.Text = Format(Val(dt1.Rows(0).Item("Salary").ToString), "########0.00")
                txt_PartyBobin.Text = Format(Val(dt1.Rows(0).Item("Party_Bobin").ToString), "########0.00")
                txt_OurOwnBobin.Text = Format(Val(dt1.Rows(0).Item("OurOwn_Bobin").ToString), "########0.00")

                txt_Meter_Reel.Text = Format(Val(dt1.Rows(0).Item("Meter_Reel").ToString), "########0.00")
                txt_ReelWeight.Text = Format(Val(dt1.Rows(0).Item("Reel_Weight").ToString), "########0.000")
                txt_Meter_Bobin.Text = Format(Val(dt1.Rows(0).Item("Meter_Bobin").ToString), "########0.00")

                cbo_BobinSize.Text = Common_Procedures.BobinSize_IdNoToName(con, dt1.Rows(0).Item("Bobin_Size_IdNo").ToString)

                da2 = New SqlClient.SqlDataAdapter("select a.*, b.Count_Name, c.Colour_Name from Bobin_Production_Details a INNER JOIN count_Head b ON a.Count_IdNo = b.Count_IdNo LEFT OUTER JOIN Colour_Head c ON a.Colour_IdNo = c.Colour_IdNo  Where a.Bobin_Production_code = '" & Trim(NewCode) & "' Order by a.Sl_No", con)
                dt2 = New DataTable
                da2.Fill(dt2)

                dgv_BobinDetails.Rows.Clear()
                Sno = 0

                If dt2.Rows.Count > 0 Then

                    For i = 0 To dt2.Rows.Count - 1

                        n = dgv_BobinDetails.Rows.Add()

                        Sno = Sno + 1
                        dgv_BobinDetails.Rows(n).Cells(0).Value = Val(Sno)
                        dgv_BobinDetails.Rows(n).Cells(1).Value = dt2.Rows(i).Item("Count_Name").ToString
                        dgv_BobinDetails.Rows(n).Cells(2).Value = Val(dt2.Rows(i).Item("Ends").ToString)
                        dgv_BobinDetails.Rows(n).Cells(3).Value = dt2.Rows(i).Item("Colour_Name").ToString
                        dgv_BobinDetails.Rows(n).Cells(4).Value = Format(Val(dt2.Rows(i).Item("Bobin_Consumption").ToString), "########0.000")

                    Next i

                End If
                dt2.Clear()

                With dgv_BobinDetails_Total
                    If .RowCount = 0 Then .Rows.Add()
                    .Rows(0).Cells(2).Value = Val(dt1.Rows(0).Item("Total_Ends").ToString)
                    .Rows(0).Cells(4).Value = Format(Val(dt1.Rows(0).Item("Total_Consumption").ToString), "########0.000")
                End With
            End If


            dt1.Dispose()
            da1.Dispose()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally
            da1.Dispose()
            dt1.Dispose()

            If msk_Date.Visible And msk_Date.Enabled Then msk_Date.Focus()
        End Try

    End Sub

    Private Sub Bobin_Production_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated

        Try

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_PartyName.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "LEDGER" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_PartyName.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If
            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_employee.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "EMPLOYEE" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_employee.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If
            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Colour.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "COLOUR" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Colour.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If
            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(Cbo_EndsCount.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "ENDSCOUNT" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                Cbo_EndsCount.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If
            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_BobinSize.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "BOBINSIZE" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_BobinSize.Text = Trim(Common_Procedures.Master_Return.Return_Value)
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

    Private Sub Bobin_Production_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim Dt2 As New DataTable
        Dim dt3 As New DataTable
        Dim dt4 As New DataTable
        Dim dt5 As New DataTable
        Dim dt6 As New DataTable
        Dim dt7 As New DataTable
        Me.Text = ""

        con.Open()

        Da = New SqlClient.SqlDataAdapter("select Ledger_DisplayName from Ledger_AlaisHead  order by Ledger_DisplayName", con)
        Da.Fill(Dt1)
        cbo_PartyName.DataSource = Dt1
        cbo_PartyName.DisplayMember = "Ledger_DisplayName"

        Da = New SqlClient.SqlDataAdapter("select Employee_Name from Employee_head order by Employee_Name", con)
        Da.Fill(Dt2)
        cbo_employee.DataSource = Dt2
        cbo_employee.DisplayMember = "Employee_Name"

        Da = New SqlClient.SqlDataAdapter("select Colour_Name from Colour_Head order by Colour_Name", con)
        Da.Fill(dt3)
        cbo_Colour.DataSource = dt3
        cbo_Colour.DisplayMember = "Colour_Name"

        Da = New SqlClient.SqlDataAdapter("select EndsCount_Name from EndsCount_Head order by EndsCount_Name", con)
        Da.Fill(dt4)
        Cbo_EndsCount.DataSource = dt4
        Cbo_EndsCount.DisplayMember = "EndsCount_Name"

        Da = New SqlClient.SqlDataAdapter("SELECT Bobin_Size_Name FROM Bobin_Size_Head ORDER BY Bobin_Size_Name", con)
        Da.Fill(dt5)
        cbo_BobinSize.DataSource = dt5
        cbo_BobinSize.DisplayMember = "Bobin_Size_Name"


        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1116" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1380" Then '---- St.LOURDU MATHA TEX (Somanur)
            If Common_Procedures.User.IdNo <> 1 Then
                txt_ReelWeight.Enabled = False
                txt_Meter_Reel.Enabled = False
            End If
        End If

        btn_UserModification.Visible = False
        If Common_Procedures.settings.User_Modifications_Show_Status = 1 Then
            If Val(Common_Procedures.User.IdNo) = 1 Or Common_Procedures.User.Show_UserModification_Status = 1 Then
                btn_UserModification.Visible = True
            End If
        End If

        pnl_filter.Visible = False
        pnl_filter.Left = (Me.Width - pnl_filter.Width) \ 2
        pnl_filter.Top = ((Me.Height - pnl_filter.Height) \ 2) + 20
        AddHandler msk_Date.GotFocus, AddressOf ControlGotFocus
        AddHandler dtp_Date.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_PartyName.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_employee.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Colour.GotFocus, AddressOf ControlGotFocus
        AddHandler Cbo_EndsCount.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Meters.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_No_Bobins.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_PartyBobin.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_OurOwnBobin.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_ReelWeight.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Meter_Reel.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Meter_Bobin.GotFocus, AddressOf ControlGotFocus

        AddHandler txt_Salary.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_SalaryBobin.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_BobinColour.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_BobinCount.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_BobinSize.GotFocus, AddressOf ControlGotFocus

        AddHandler cbo_Filter_Employee.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Filter_PartyName.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Filter_Colour.GotFocus, AddressOf ControlGotFocus

        AddHandler msk_Date.LostFocus, AddressOf ControlLostFocus
        AddHandler dtp_Date.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_employee.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_PartyName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_BobinColour.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_BobinCount.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Meters.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_No_Bobins.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_PartyBobin.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_OurOwnBobin.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_ReelWeight.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Meter_Reel.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Meter_Bobin.LostFocus, AddressOf ControlLostFocus

        AddHandler cbo_Colour.LostFocus, AddressOf ControlLostFocus
        AddHandler Cbo_EndsCount.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Salary.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_SalaryBobin.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_BobinSize.LostFocus, AddressOf ControlLostFocus

        AddHandler cbo_Filter_Employee.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Filter_PartyName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Filter_Colour.LostFocus, AddressOf ControlLostFocus


        'AddHandler msk_Date.KeyDown, AddressOf TextBoxControlKeyDown
        'AddHandler dtp_Date.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler dtp_FilterFrom_date.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler dtp_FilterTo_date.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_No_Bobins.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Meters.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_SalaryBobin.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_PartyBobin.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_OurOwnBobin.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_ReelWeight.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Meter_Reel.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Meter_Bobin.KeyDown, AddressOf TextBoxControlKeyDown


        'AddHandler msk_Date.KeyPress, AddressOf TextBoxControlKeyPress
        'AddHandler dtp_Date.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler dtp_FilterFrom_date.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler dtp_FilterTo_date.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_No_Bobins.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Meters.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_SalaryBobin.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_PartyBobin.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_OurOwnBobin.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_ReelWeight.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Meter_Reel.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Meter_Bobin.KeyPress, AddressOf TextBoxControlKeyPress

        lbl_Company.Text = ""
        lbl_Company.Tag = 0
        lbl_Company.Visible = False
        Common_Procedures.CompIdNo = 0

        dgv_filter.RowTemplate.Height = 27

        Filter_Status = False
        FrmLdSTS = True
        new_record()

    End Sub

    Private Sub Bobin_Production_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        On Error Resume Next
        con.Close()
        con.Dispose()
        Common_Procedures.Last_Closed_FormName = Me.Name
    End Sub

    Private Sub Bobin_Production_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress

        Try

            If Asc(e.KeyChar) = 27 Then

                If pnl_filter.Visible = True Then
                    btn_closefilter_Click(sender, e)
                    Exit Sub


                Else

                    If MessageBox.Show("Do you want to Close?", "FOR CLOSING ENTRY...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) <> Windows.Forms.DialogResult.Yes Then
                        Exit Sub

                    Else
                        Close_Form()


                    End If


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
        Dim vOrdByNo As String = ""

        vOrdByNo = Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text)

        If MessageBox.Show("Do you want to Delete?", "FOR DELETION...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) <> Windows.Forms.DialogResult.Yes Then
            Exit Sub
        End If

        'If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.Inhouse_Bobin_Production, "~L~") = 0 And InStr(Common_Procedures.UR.Inhouse_Bobin_Production, "~D~") = 0 Then MessageBox.Show("You have No Rights to Delete", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error) : Exit Sub
        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.DeletingEntry, Common_Procedures.UR.Bobin_Production_Entry, New_Entry, Me, con, "Bobin_Production_Head", "Bobin_Production_Code", NewCode, "Bobin_Production_Date", "(Bobin_Production_Code = '" & Trim(NewCode) & "')") = False Then Exit Sub

        If New_Entry = True Then
            MessageBox.Show("This is New Entry", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        tr = con.BeginTransaction

        Try

            cmd.Connection = con
            cmd.Transaction = tr

            Call Common_Procedures.User_Modification_Updation(con, "HEAD", Me.Name, "DELETE", "Bobin_Production_Head", "Bobin_Production_Code", Val(lbl_Company.Tag), NewCode, lbl_RefNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, True, "", "", "Bobin_Production_Code, Company_IdNo, for_OrderBy", tr)

            Call Common_Procedures.User_Modification_Updation(con, "DETAILS", Me.Name, "DELETE", "Bobin_Production_Details", "Bobin_Production_Code", Val(lbl_Company.Tag), NewCode, lbl_RefNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, True, " Count_IdNo, Colour_IdNo, Ends,  Bobin_Consumption", "Sl_No", "Bobin_Production_Code, For_OrderBy, Company_IdNo, Bobin_Production_No, Bobin_Production_Date, Ledger_Idno", tr)


            cmd.CommandText = "Delete from Stock_Empty_BeamBagCone_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Stock_Pavu_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Stock_Yarn_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "delete from Bobin_Production_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Bobin_Production_code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Bobin_Production_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Bobin_Production_code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            tr.Commit()

            new_record()

            MessageBox.Show("Deleted Sucessfully!!!", "FOR DELETION...", MessageBoxButtons.OK, MessageBoxIcon.Information)

        Catch ex As Exception

            tr.Rollback()
            MessageBox.Show(ex.Message, "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        If msk_Date.Enabled And msk_Date.Visible Then msk_Date.Focus()

    End Sub

    Public Sub filter_record() Implements Interface_MDIActions.filter_record
        If Filter_Status = False Then
            Dim da As New SqlClient.SqlDataAdapter
            Dim dt1 As New DataTable
            Dim dt2 As New DataTable
            Dim dt3 As New DataTable

            da = New SqlClient.SqlDataAdapter("select Ledger_DisplayName from Ledger_AlaisHead  order by Ledger_DisplayName", con)
            da.Fill(dt1)
            cbo_Filter_PartyName.DataSource = dt1
            cbo_Filter_PartyName.DisplayMember = "Ledger_DisplayName"

            da = New SqlClient.SqlDataAdapter("select Employee_Name from Employee_head order by Employee_Name", con)
            da.Fill(dt3)
            cbo_Filter_Employee.DataSource = dt3
            cbo_Filter_Employee.DisplayMember = "Employee_Name"

            da = New SqlClient.SqlDataAdapter("select Colour_Name from Colour_Head order by Colour_Name", con)
            da.Fill(dt1)
            cbo_Filter_Colour.DataSource = dt1
            cbo_Filter_Colour.DisplayMember = "Colour_Name"

            dtp_FilterFrom_date.Text = Common_Procedures.Company_FromDate
            dtp_FilterTo_date.Text = Common_Procedures.Company_ToDate
            pnl_filter.Text = ""
            cbo_Filter_PartyName.SelectedIndex = -1
            cbo_Filter_Employee.SelectedIndex = -1
            cbo_Filter_Colour.SelectedIndex = -1

            dgv_filter.Rows.Clear()

            da.Dispose()

        End If

        pnl_filter.Visible = True
        pnl_filter.Enabled = True
        pnl_filter.BringToFront()
        pnl_back.Enabled = False
        If dtp_FilterFrom_date.Enabled And dtp_FilterFrom_date.Visible Then dtp_FilterFrom_date.Focus()

    End Sub

    Public Sub insert_record() Implements Interface_MDIActions.insert_record
        Dim cmd As New SqlClient.SqlCommand
        Dim dr As SqlClient.SqlDataReader
        Dim movno As String, inpno As String
        Dim NewCode As String

        'If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.Inhouse_Bobin_Production, "~L~") = 0 And InStr(Common_Procedures.UR.Inhouse_Bobin_Production, "~I~") = 0 Then MessageBox.Show("You have No Rights to Insert", "DOES NOT INSERT...", MessageBoxButtons.OK, MessageBoxIcon.Error) : Exit Sub
        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.InsertingEntry, Common_Procedures.UR.Bobin_Production_Entry, New_Entry, Me) = False Then Exit Sub

        Try

            inpno = InputBox("Enter New Ref.No.", "FOR INSERTION...")

            NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

            cmd.Connection = con
            cmd.CommandText = "select Bobin_Production_No from Bobin_Production_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Bobin_Production_code = '" & Trim(NewCode) & "'"
            dr = cmd.ExecuteReader

            movno = ""
            If dr.HasRows Then
                If dr.Read Then
                    If IsDBNull(dr(0).ToString) = False Then
                        movno = dr(0).ToString
                    End If
                End If
            End If

            dr.Close()
            cmd.Dispose()

            If Val(movno) <> 0 Then
                move_record(movno)

            Else
                If Val(inpno) = 0 Then
                    MessageBox.Show("Invalid Ref.No", "DOES NOT INSERT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

                Else
                    new_record()
                    Insert_Entry = True
                    lbl_RefNo.Text = Trim(UCase(inpno))

                End If

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT FIND...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Public Sub movefirst_record() Implements Interface_MDIActions.movefirst_record
        Dim cmd As New SqlClient.SqlCommand
        Dim dr As SqlClient.SqlDataReader
        Dim movno As String

        Try

            cmd.Connection = con
            cmd.CommandText = "select top 1 Bobin_Production_No from Bobin_Production_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Bobin_Production_code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby, Bobin_Production_No"
            dr = cmd.ExecuteReader

            movno = ""
            If dr.HasRows Then
                If dr.Read Then
                    If IsDBNull(dr(0).ToString) = False Then
                        movno = dr(0).ToString
                    End If
                End If
            End If

            dr.Close()

            If Val(movno) <> 0 Then move_record(movno)

        Catch ex As Exception

            MessageBox.Show(ex.Message, "FOR  MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Public Sub movelast_record() Implements Interface_MDIActions.movelast_record
        Dim cmd As New SqlClient.SqlCommand
        Dim dr As SqlClient.SqlDataReader
        Dim movno As String

        Try
            cmd.Connection = con
            cmd.CommandText = "select top 1 Bobin_Production_No from Bobin_Production_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Bobin_Production_code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, Bobin_Production_No desc"
            dr = cmd.ExecuteReader

            movno = ""
            If dr.HasRows Then
                If dr.Read Then
                    If IsDBNull(dr(0).ToString) = False Then
                        movno = dr(0).ToString
                    End If
                End If
            End If

            dr.Close()

            If Val(movno) <> 0 Then move_record(movno)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR  MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Public Sub movenext_record() Implements Interface_MDIActions.movenext_record
        Dim cmd As New SqlClient.SqlCommand
        Dim dr As SqlClient.SqlDataReader
        Dim movno As String = ""
        Dim OrdByNo As Single = 0

        Try

            OrdByNo = Common_Procedures.OrderBy_CodeToValue(Trim(lbl_RefNo.Text))

            cmd.Connection = con
            cmd.CommandText = "select top 1 Bobin_Production_No from Bobin_Production_Head where for_orderby > " & Str(OrdByNo) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and Bobin_Production_code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby, Bobin_Production_No"
            dr = cmd.ExecuteReader

            movno = ""
            If dr.HasRows Then
                If dr.Read Then
                    If IsDBNull(dr(0).ToString) = False Then
                        movno = dr(0).ToString
                    End If
                End If
            End If

            dr.Close()

            If Val(movno) <> 0 Then move_record(movno)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR  MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Public Sub moveprevious_record() Implements Interface_MDIActions.moveprevious_record
        Dim cmd As New SqlClient.SqlCommand
        Dim dr As SqlClient.SqlDataReader
        Dim movno As String = ""
        Dim OrdByNo As Single = 0

        Try

            OrdByNo = Common_Procedures.OrderBy_CodeToValue(Trim(lbl_RefNo.Text))

            cmd.Connection = con
            cmd.CommandText = "select top 1 Bobin_Production_No from Bobin_Production_Head where for_orderby < " & Str(OrdByNo) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and  Bobin_Production_code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, Bobin_Production_No desc"
            dr = cmd.ExecuteReader

            movno = ""
            If dr.HasRows Then
                If dr.Read Then
                    If IsDBNull(dr(0).ToString) = False Then
                        movno = dr(0).ToString
                    End If
                End If
            End If

            dr.Close()

            If Val(movno) <> 0 Then move_record(movno)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR  MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub

    Public Sub new_record() Implements Interface_MDIActions.new_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim dt1 As New DataTable
        Dim NewID As Integer = 0

        Try
            clear()

            New_Entry = True

            da = New SqlClient.SqlDataAdapter("select max(for_orderby) from Bobin_Production_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Bobin_Production_code like '%/" & Trim(Common_Procedures.FnYearCode) & "' ", con)
            da.Fill(dt)

            NewID = 0
            If dt.Rows.Count > 0 Then
                If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                    NewID = Val(dt.Rows(0)(0).ToString)
                End If
            End If

            NewID = NewID + 1

            lbl_RefNo.Text = NewID
            lbl_RefNo.ForeColor = Color.Red

            msk_Date.Text = Date.Today.ToShortDateString
            da = New SqlClient.SqlDataAdapter("select top 1 * from Bobin_Production_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Bobin_Production_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, Bobin_Production_No desc", con)
            dt1 = New DataTable
            da.Fill(dt1)
            If dt1.Rows.Count > 0 Then
                If Val(Common_Procedures.settings.PreviousEntryDate_ByDefault) = 1 Then '---- M.S Textiles (Tirupur)
                    If dt1.Rows(0).Item("Bobin_Production_Date").ToString <> "" Then msk_Date.Text = dt1.Rows(0).Item("Bobin_Production_Date").ToString
                End If
            End If
            dt1.Clear()


            If msk_Date.Enabled And msk_Date.Visible Then
                msk_Date.Focus()
                msk_Date.SelectionStart = 0
            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR NEW RECORD...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
        dt1.Dispose()
        da.Dispose()
    End Sub

    Public Sub open_record() Implements Interface_MDIActions.open_record
        Dim cmd As New SqlClient.SqlCommand
        Dim dr As SqlClient.SqlDataReader
        Dim movno As String, inpno As String
        Dim NewCode As String

        Try

            inpno = InputBox("Enter Ref.No", "FOR FINDING...")

            NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

            cmd.Connection = con
            cmd.CommandText = "select Bobin_Production_No from Bobin_Production_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Bobin_Production_code = '" & Trim(NewCode) & "'"
            dr = cmd.ExecuteReader

            movno = ""
            If dr.HasRows Then
                If dr.Read Then
                    If IsDBNull(dr(0).ToString) = False Then
                        movno = dr(0).ToString
                    End If
                End If
            End If

            dr.Close()
            cmd.Dispose()

            If Val(movno) <> 0 Then
                move_record(movno)

            Else
                MessageBox.Show("Ref.No. Does not exists", "DOES NOT FIND...", MessageBoxButtons.OK, MessageBoxIcon.Error)

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT FIND...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Public Sub save_record() Implements Interface_MDIActions.save_record
        Dim cmd As New SqlClient.SqlCommand
        Dim tr As SqlClient.SqlTransaction
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim NewCode As String = ""
        Dim nr As Long = 0
        Dim led_id As Integer = 0
        Dim Emp_ID As Integer = 0
        Dim Clr_ID As Integer = 0
        Dim Clo_ID3 As Integer = 0
        Dim EdsCnt_ID As Integer = 0
        Dim Cnt_ID As Integer = 0
        Dim Lm_ID As Integer = 0
        Dim Partcls As String = ""
        Dim EntID As String = ""
        Dim PBlNo As String = ""
        Dim NoofInpBmsInLom As Integer = 0
        Dim NoofKnotBmsInCD As Integer = 0
        Dim NoofKnotBmsInLom As Integer = 0
        Dim vTotends As Single, vTotconMtrs As Single
        Dim decnt_ID As Integer = 0
        Dim declr_ID As Integer = 0
        Dim Sno As Integer = 0

        Dim BbnSz_ID As Integer = 0
        Dim vOrdByNo As String = ""

        vOrdByNo = Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text)


        If Val(lbl_Company.Tag) = 0 Then
            MessageBox.Show("Invalid Company Selection", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If
        If pnl_back.Enabled = False Then
            MessageBox.Show("Close all other windows", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        ' If Common_Procedures.UserRight_Check(Common_Procedures.UR.Inhouse_Bobin_Production, New_Entry) = False Then Exit Sub
        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.SavingEntry, Common_Procedures.UR.Bobin_Production_Entry, New_Entry, Me, con, "Bobin_Production_Head", "Bobin_Production_Code", NewCode, "Bobin_Production_Date", "(Bobin_Production_Code = '" & Trim(NewCode) & "')", "(Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Bobin_Production_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "')", "for_Orderby desc, Bobin_Production_No desc", dtp_Date.Value.Date) = False Then Exit Sub


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
        Emp_ID = Common_Procedures.Employee_Simple_NameToIdNo(con, cbo_employee.Text)
        If Emp_ID = 0 Then
            MessageBox.Show("Invalid Employee Name", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If cbo_employee.Enabled Then cbo_employee.Focus()
            Exit Sub
        End If

        led_id = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_PartyName.Text)
        If led_id = 0 Then
            MessageBox.Show("Invalid Ledger Name", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If cbo_PartyName.Enabled Then cbo_PartyName.Focus()
            Exit Sub
        End If

        Clr_ID = Common_Procedures.Colour_NameToIdNo(con, cbo_Colour.Text)

        If Clr_ID = 0 Then
            MessageBox.Show("Invalid Colour Name", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If cbo_Colour.Enabled Then cbo_Colour.Focus()
            Exit Sub
        End If

        EdsCnt_ID = Common_Procedures.EndsCount_NameToIdNo(con, Cbo_EndsCount.Text)
        If EdsCnt_ID = 0 Then
            MessageBox.Show("Invalid Ends/Count", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If Cbo_EndsCount.Enabled Then Cbo_EndsCount.Focus()
            Exit Sub
        End If

        If Val(txt_Meters.Text) = 0 Then
            MessageBox.Show("Invalid  Meters", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If txt_Meters.Enabled Then txt_Meters.Focus()
            Exit Sub
        End If

        BbnSz_ID = Common_Procedures.BobinSize_NameToIdNo(con, Trim(cbo_BobinSize.Text))

        With dgv_BobinDetails

            For i = 0 To .RowCount - 1

                If Val(.Rows(i).Cells(4).Value) <> 0 Then

                    If Trim(.Rows(i).Cells(1).Value) = "" Then
                        MessageBox.Show("Invalid Count Name", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        If .Enabled And .Visible Then
                            .Focus()
                            .CurrentCell = .Rows(i).Cells(1)
                        End If
                        Exit Sub
                    End If

                    If Trim(.Rows(i).Cells(3).Value) = "" Then
                        MessageBox.Show("Invalid Colour Name", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        If .Enabled And .Visible Then
                            .Focus()
                            .CurrentCell = .Rows(i).Cells(1)
                        End If
                        Exit Sub
                    End If

                    If Val(.Rows(i).Cells(4).Value) = 0 Then
                        MessageBox.Show("Invalid Consumption..", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        If .Enabled Then .Focus()
                        .CurrentCell = .Rows(0).Cells(6)
                        Exit Sub
                    End If

                End If

            Next
        End With

        Total_Calculation()

        vTotends = 0 : vTotconMtrs = 0
        If dgv_BobinDetails_Total.RowCount > 0 Then
            vTotends = Val(dgv_BobinDetails_Total.Rows(0).Cells(2).Value())
            vTotconMtrs = Val(dgv_BobinDetails_Total.Rows(0).Cells(4).Value())
        End If

        tr = con.BeginTransaction

        Try

            If Insert_Entry = True Or New_Entry = False Then
                NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            Else

                lbl_RefNo.Text = Common_Procedures.get_MaxCode(con, "Bobin_Production_Head", "Bobin_Production_code", "for_OrderBy", "", Val(lbl_Company.Tag), Common_Procedures.FnYearCode, tr)

                NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            End If

            cmd.Connection = con
            cmd.Transaction = tr

            cmd.Parameters.Clear()
            cmd.Parameters.AddWithValue("@BobinDate", dtp_Date.Value.Date)

            vOrdByNo = Val(Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text))

            If New_Entry = True Then

                cmd.CommandText = "Insert into Bobin_Production_Head (  Bobin_Production_code,           Company_IdNo      ,        Bobin_Production_No     ,                               for_OrderBy                          , Bobin_Production_Date, Employee_IdNo       ,    Ledger_IdNo      ,  Colour_IdNo        , EndsCount_IdNo              ,      No_Of_Bobin                ,                 Meters         , Total_Ends            , Total_Consumption        ,   Salary_Bobin                    , Salary                       ,  Party_Bobin                         , OurOwn_Bobin                           ,  Meter_Reel                           ,  Reel_Weight                          , Meter_Bobin                       , Bobin_Size_IdNo          ) " & _
                "Values                                              ('" & Trim(NewCode) & "', " & Val(lbl_Company.Tag) & ", '" & Trim(lbl_RefNo.Text) & "' , " & Val(Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text)) & " ,     @BobinDate       , " & Val(Emp_ID) & " , " & Val(led_id) & " , " & Val(Clr_ID) & " , " & Str(Val(EdsCnt_ID)) & " , " & Val(txt_No_Bobins.Text) & " ,  " & Val(txt_Meters.Text) & "  , " & Val(vTotends) & " , " & Val(vTotconMtrs) & " , " & Val(txt_SalaryBobin.Text) & " , " & Val(txt_Salary.Text) & " , " & Str(Val(txt_PartyBobin.Text)) & ", " & Str(Val(txt_OurOwnBobin.Text)) & " , " & Str(Val(txt_Meter_Reel.Text)) & " , " & Str(Val(txt_ReelWeight.Text)) & " , " & Val(txt_Meter_Bobin.Text) & " ," & Str(Val(BbnSz_ID)) & ") "
                cmd.ExecuteNonQuery()

            Else

                Call Common_Procedures.User_Modification_Updation(con, "HEAD", Me.Name, "OLD", "Bobin_Production_Head", "Bobin_Production_Code", Val(lbl_Company.Tag), NewCode, lbl_RefNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, False, "", "", "Bobin_Production_Code, Company_IdNo, for_OrderBy", tr)

                Call Common_Procedures.User_Modification_Updation(con, "DETAILS", Me.Name, "OLD", "Bobin_Production_Details", "Bobin_Production_Code", Val(lbl_Company.Tag), NewCode, lbl_RefNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, False, " Count_IdNo, Colour_IdNo, Ends,  Bobin_Consumption", "Sl_No", "Bobin_Production_Code, For_OrderBy, Company_IdNo, Bobin_Production_No, Bobin_Production_Date, Ledger_Idno", tr)


                cmd.CommandText = "Update Bobin_Production_Head set Bobin_Production_Date = @BobinDate, Employee_IdNo = " & Val(Emp_ID) & ", Ledger_IdNo = " & Str(Val(led_id)) & ", Colour_IdNo = " & Str(Val(Clr_ID)) & ",    EndsCount_IdNo = " & Str(Val(EdsCnt_ID)) & ",No_Of_Bobin = " & Str(Val(txt_No_Bobins.Text)) & ", Meters = " & Str(Val(txt_Meters.Text)) & " ,Total_Ends =  " & Val(vTotends) & " , Total_Consumption = " & Val(vTotconMtrs) & " ,Salary_Bobin =" & Val(txt_SalaryBobin.Text) & " , Salary = " & Val(txt_Salary.Text) & " ,Meter_Bobin=" & Val(txt_Meter_Bobin.Text) & " , Party_Bobin = " & Str(Val(txt_PartyBobin.Text)) & " , Meter_Reel = " & Str(Val(txt_Meter_Reel.Text)) & " , Reel_Weight = " & Str(Val(txt_ReelWeight.Text)) & " , OurOwn_Bobin = " & Str(Val(txt_OurOwnBobin.Text)) & " , Bobin_Size_IdNo = " & Str(Val(BbnSz_ID)) & "  Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Bobin_Production_code = '" & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()

            End If
            Call Common_Procedures.User_Modification_Updation(con, "HEAD", Me.Name, "NEW", "Bobin_Production_Head", "Bobin_Production_Code", Val(lbl_Company.Tag), NewCode, lbl_RefNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, False, "", "", "Bobin_Production_Code, Company_IdNo, for_OrderBy", tr)

           
            Partcls = "BobProd : Ref.No. " & Trim(lbl_RefNo.Text)
            PBlNo = Trim(lbl_RefNo.Text)
            EntID = Trim(Pk_Condition) & Trim(lbl_RefNo.Text)

            cmd.CommandText = "Delete from Bobin_Production_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Bobin_Production_code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Stock_Pavu_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Stock_Yarn_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Stock_Empty_BeamBagCone_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "truncate table " & Trim(Common_Procedures.EntryTempTable) & ""
            cmd.ExecuteNonQuery()

            With dgv_BobinDetails
                Sno = 0
                For i = 0 To .RowCount - 1

                    If Trim(.Rows(i).Cells(1).Value) <> "" And Val(.Rows(i).Cells(4).Value) <> 0 Then

                        Sno = Sno + 1

                        decnt_ID = Common_Procedures.Count_NameToIdNo(con, .Rows(i).Cells(1).Value, tr)

                        declr_ID = Common_Procedures.Colour_NameToIdNo(con, .Rows(i).Cells(3).Value, tr)

                        cmd.CommandText = "Insert into Bobin_Production_Details (  Bobin_Production_code,           Company_IdNo           ,        Bobin_Production_No      ,                               for_OrderBy                          , Bobin_Production_Date,     Sl_No             , Count_IdNo                , Colour_IdNo                , Ends                                ,  Bobin_Consumption                  ) " & _
                                                                        " Values ('" & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_RefNo.Text) & "'," & Val(Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text)) & " , @BobinDate ,              " & Str(Val(Sno)) & " , " & Str(Val(decnt_ID)) & ", " & Str(Val(declr_ID)) & " , " & Val(.Rows(i).Cells(2).Value) & ", " & Val(.Rows(i).Cells(4).Value) & ")"
                        cmd.ExecuteNonQuery()

                        cmd.CommandText = "insert into " & Trim(Common_Procedures.EntryTempTable) & "(int1, int2, weight1) values (" & Str(Val(decnt_ID)) & ", " & Str(Val(declr_ID)) & ", " & Val(.Rows(i).Cells(4).Value) & ")"
                        cmd.ExecuteNonQuery()

                    End If

                Next
                Call Common_Procedures.User_Modification_Updation(con, "DETAILS", Me.Name, "NEW", "Bobin_Production_Details", "Bobin_Production_Code", Val(lbl_Company.Tag), NewCode, lbl_RefNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, False, " Count_IdNo, Colour_IdNo, Ends,  Bobin_Consumption", "Sl_No", "Bobin_Production_Code, For_OrderBy, Company_IdNo, Bobin_Production_No, Bobin_Production_Date, Ledger_Idno", tr)

            End With

            da = New SqlClient.SqlDataAdapter("select int1 as Count_ID, int2 as Colour_ID, sum(weight1) as Weight from " & Trim(Common_Procedures.EntryTempTable) & " group by int1, int2 order by int1, int2", con)
            da.SelectCommand.Transaction = tr
            dt1 = New DataTable
            da.Fill(dt1)

            If dt1.Rows.Count > 0 Then

                Sno = 0
                For i = 0 To dt1.Rows.Count - 1

                    Sno = Sno + 1
                    cmd.CommandText = "Insert into Stock_Yarn_Processing_Details ( Reference_Code, Company_IdNo, Reference_No, for_OrderBy, Reference_Date, DeliveryTo_Idno, ReceivedFrom_Idno, Entry_ID, Party_Bill_No, Particulars, Sl_No, Count_IdNo, Yarn_Type, Mill_IdNo, Colour_IdNo, Jumbo, Cones, Weight) Values ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_RefNo.Text) & "', " & Str(vOrdByNo) & ", @BobinDate, 0, " & Str(Val(Common_Procedures.CommonLedger.Godown_Ac)) & ", '" & Trim(EntID) & "', '" & Trim(PBlNo) & "', '" & Trim(Partcls) & "', " & Str(Val(Sno)) & ", " & Str(Val(dt1.Rows(i).Item("Count_ID").ToString)) & ", 'MILL', 0, " & Str(Val(dt1.Rows(i).Item("Colour_ID").ToString)) & ", 0, 0, " & Str(Val(dt1.Rows(i).Item("weight").ToString)) & " )"
                    cmd.ExecuteNonQuery()

                Next

            End If

            cmd.CommandText = "Insert into Stock_Pavu_Processing_Details ( Reference_Code, Company_IdNo                     , Reference_No                  , for_OrderBy          , Reference_Date, DeliveryTo_Idno                                           , ReceivedFrom_Idno, StockOf_IdNo            , Entry_ID             , Party_Bill_No        , Particulars            , Sl_No                , EndsCount_IdNo             , Colour_IdNo             , Bobins                              , Meters                            , Bobin_Size_IdNo           ) " & _
            "Values                        ( '" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_RefNo.Text) & "', " & Str(vOrdByNo) & ", @BobinDate    , " & Str(Val(Common_Procedures.CommonLedger.Godown_Ac)) & ", 0                , " & Str(Val(led_id)) & ", '" & Trim(EntID) & "', '" & Trim(PBlNo) & "', '" & Trim(Partcls) & "', " & Str(Val(Sno)) & ", " & Str(Val(EdsCnt_ID)) & ", " & Str(Val(Clr_ID)) & ", " & Str(Val(txt_No_Bobins.Text)) & ", " & Str(Val(txt_Meters.Text)) & " , " & Str(Val(BbnSz_ID)) & ")"
            cmd.ExecuteNonQuery()

            If Val(txt_OurOwnBobin.Text) <> 0 Then
                cmd.CommandText = "Insert into Stock_Empty_BeamBagCone_Processing_Details ( Reference_Code, Company_IdNo, Reference_No, for_OrderBy, Reference_Date, DeliveryTo_Idno, ReceivedFrom_Idno, Entry_ID, Party_Bill_No, Particulars, Sl_No, Empty_Bobin ) Values ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_RefNo.Text) & "', " & Str(vOrdByNo) & ", @BobinDate, " & Str(Val(led_id)) & ", " & Str(Val(Common_Procedures.CommonLedger.Godown_Ac)) & ", '" & Trim(EntID) & "', '" & Trim(PBlNo) & "', '" & Trim(Partcls) & "', 1, " & Str(Val(txt_OurOwnBobin.Text)) & " )"
                cmd.ExecuteNonQuery()
            End If

            If Val(txt_PartyBobin.Text) <> 0 Then
                cmd.CommandText = "Insert into Stock_Empty_BeamBagCone_Processing_Details ( Reference_Code, Company_IdNo, Reference_No, for_OrderBy, Reference_Date, DeliveryTo_Idno, ReceivedFrom_Idno, Entry_ID, Party_Bill_No, Particulars, Sl_No, EmptyBobin_Party ) Values ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_RefNo.Text) & "', " & Str(vOrdByNo) & ", @BobinDate, " & Str(Val(led_id)) & ", 0, '" & Trim(EntID) & "', '" & Trim(PBlNo) & "', '" & Trim(Partcls) & "', 2, " & Str(Val(txt_PartyBobin.Text)) & " )"
                cmd.ExecuteNonQuery()
            End If

            tr.Commit()

            move_record(lbl_RefNo.Text)

            MessageBox.Show("Saved Sucessfully!!!", "FOR SAVING...", MessageBoxButtons.OK, MessageBoxIcon.Information)

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
            MessageBox.Show(ex.Message, "FOR SAVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally
            dt1.Dispose()
            da.Dispose()
            cmd.Dispose()
            tr.Dispose()

            If msk_Date.Enabled And msk_Date.Visible Then msk_Date.Focus()

        End Try

    End Sub

    Public Sub print_record() Implements Interface_MDIActions.print_record
        '------ No Print
    End Sub

    Private Sub cbo_PartyName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_PartyName.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "", "(Ledger_idno = 0)")
    End Sub

    Private Sub cbo_PartyName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_PartyName.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_PartyName, Nothing, Nothing, "Ledger_AlaisHead", "Ledger_DisplayName", "", "(Ledger_idno = 0)")
        If e.KeyCode = 38 And cbo_PartyName.DroppedDown = False Or (e.KeyCode = 38 And e.Control = True) Then
            If cbo_Colour.Visible Then
                cbo_Colour.Focus()
            Else
                cbo_employee.Focus()
            End If
        End If
        If e.KeyCode = 40 And cbo_PartyName.DroppedDown = False Or (e.KeyCode = 40 And e.Control = True) Then
            If Cbo_EndsCount.Visible Then
                Cbo_EndsCount.Focus()
            Else
                cbo_BobinSize.Focus()
            End If
        End If
    End Sub

    Private Sub cbo_PartyName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_PartyName.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_PartyName, Nothing, "Ledger_AlaisHead", "Ledger_DisplayName", "", "(Ledger_idno = 0)")
        If Asc(e.KeyChar) = 13 Then
            If Cbo_EndsCount.Visible Then
                Cbo_EndsCount.Focus()
            Else
                cbo_BobinSize.Focus()
            End If
        End If
    End Sub

    Private Sub cbo_partyname_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_PartyName.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then

            Common_Procedures.UR.Ledr_Wea_Siz_Rw_Trans_JbWrk_Creation = Common_Procedures.UR.Ledger_Creation
            Common_Procedures.MDI_LedType = ""
            Dim f As New Ledger_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_PartyName.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1

            f.Show()

        End If
    End Sub

    Private Sub cbo_Colour_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Colour.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Colour, cbo_employee, cbo_PartyName, "Colour_Head", "Colour_Name", "", "(Colour_idno = 0)")

    End Sub

    Private Sub cbo_Colour_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Colour.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Colour, cbo_PartyName, "Colour_Head", "Colour_Name", "", "(Colour_idno = 0)")

    End Sub

    Private Sub cbo_employee_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_employee.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Employee_head", "Employee_Name", "", "(Employee_IdNo)")
    End Sub

    Private Sub cbo_Shift_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_employee.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_employee, msk_Date, Nothing, "Employee_head", "Employee_Name", "", "(Employee_IdNo = 0)")
        If e.KeyCode = 40 And cbo_employee.DroppedDown = False Or (e.KeyCode = 40 And e.Control = True) Then
            If cbo_Colour.Visible Then
                cbo_Colour.Focus()
            Else
                cbo_PartyName.Focus()
            End If
        End If
    End Sub

    Private Sub cbo_Shift_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_employee.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_employee, Nothing, "Employee_head", "Employee_Name", "", "(Employee_IdNo)")
        If Asc(e.KeyChar) = 13 Then
            e.Handled = True
            If cbo_Colour.Visible = True Then
                cbo_Colour.Focus()
            Else
                cbo_PartyName.Focus()
            End If
        End If
    End Sub

    Private Sub cbo_Filter_PartyName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Filter_PartyName.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "", "(Ledger_idno = 0)")
    End Sub

    Private Sub cbo_Filter_PartyName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Filter_PartyName.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_PartyName, cbo_Filter_Employee, cbo_Filter_Colour, "Ledger_AlaisHead", "Ledger_DisplayName", "", "(Ledger_idno = 0)")
    End Sub

    Private Sub cbo_Filter_PartyName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Filter_PartyName.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Filter_PartyName, cbo_Filter_Colour, "Ledger_AlaisHead", "Ledger_DisplayName", "", "(Ledger_idno = 0)")
    End Sub


    Private Sub btn_filtershow_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_filtershow.Click
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim n As Integer
        Dim Led_IdNo As Integer, Emp_IdNo As Integer, Clr_IdNo As Integer
        Dim Condt As String = ""

        Try

            Condt = ""
            Led_IdNo = 0
            Emp_IdNo = 0


            If IsDate(dtp_FilterFrom_date.Value) = True And IsDate(dtp_FilterTo_date.Value) = True Then
                Condt = "a.Bobin_Production_Date between '" & Trim(Format(dtp_FilterFrom_date.Value, "MM/dd/yyyy")) & "' and '" & Trim(Format(dtp_FilterTo_date.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_FilterFrom_date.Value) = True Then
                Condt = "a.Bobin_Production_Date = '" & Trim(Format(dtp_FilterFrom_date.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_FilterTo_date.Value) = True Then
                Condt = "a. Bobin_Production_Date= '" & Trim(Format(dtp_FilterTo_date.Value, "MM/dd/yyyy")) & "' "
            End If

            If Trim(cbo_Filter_PartyName.Text) <> "" Then
                Led_IdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Filter_PartyName.Text)
            End If

            If Trim(cbo_Filter_Colour.Text) <> "" Then
                Clr_IdNo = Common_Procedures.Colour_NameToIdNo(con, cbo_Filter_Colour.Text)
            End If

            If Val(Led_IdNo) <> 0 Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " (a.Ledger_Idno = " & Str(Val(Led_IdNo)) & ")"
            End If

            If Trim(cbo_Filter_Employee.Text) <> "" Then
                Emp_IdNo = Common_Procedures.Employee_NameToIdNo(con, cbo_Filter_Employee.Text)
            End If
            If Val(Emp_IdNo) <> 0 Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " (a.Employee_Idno = " & Str(Val(Emp_IdNo)) & ")"
            End If

            If Val(Clr_IdNo) <> 0 Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " (a.Colour_IdNo = " & Str(Val(Clr_IdNo)) & ")"
            End If


            da = New SqlClient.SqlDataAdapter("select a.*, b.Ledger_Name , c.Employee_Name from Bobin_Production_Head a INNER JOIN Ledger_Head b ON a.Ledger_IdNo = b.Ledger_IdNo INNER JOIN Employee_head c ON a.Employee_IdNo = c.Employee_IdNo  where a.company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Bobin_Production_code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "' " & IIf(Trim(Condt) <> "", " and ", "") & Condt & " Order by a.for_orderby, a.Bobin_Production_No", con)
            dt2 = New DataTable
            da.Fill(dt2)

            dgv_filter.Rows.Clear()

            If dt2.Rows.Count > 0 Then

                For i = 0 To dt2.Rows.Count - 1

                    n = dgv_filter.Rows.Add()

                    dgv_filter.Rows(n).Cells(0).Value = dt2.Rows(i).Item("Bobin_Production_No").ToString
                    dgv_filter.Rows(n).Cells(1).Value = Format(Convert.ToDateTime(dt2.Rows(i).Item("Bobin_Production_Date").ToString), "dd-MM-yyyy")
                    dgv_filter.Rows(n).Cells(2).Value = dt2.Rows(i).Item("Ledger_Name").ToString
                    dgv_filter.Rows(n).Cells(3).Value = dt2.Rows(i).Item("Employee_Name").ToString
                    dgv_filter.Rows(n).Cells(4).Value = Format(Val(dt2.Rows(i).Item("Meters").ToString), "########0.00")




                Next i

            End If

            dt2.Clear()
            dt2.Dispose()
            da.Dispose()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT FILTER...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        If dgv_filter.Visible And dgv_filter.Enabled Then dgv_filter.Focus()

    End Sub

    Private Sub cbo_Filter_Employee_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Filter_Employee.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Employee_head", "Employee_Name", "", "(Employee_idno = 0)")
    End Sub
    Private Sub cbo_Filter_EmployeeName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Filter_Employee.KeyDown

        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_Employee, dtp_FilterTo_date, cbo_Filter_PartyName, "Employee_head", "Employee_Name", "", "(Employee_idno = 0)")
    End Sub

    Private Sub cbo_Filter_EmployeeName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Filter_Employee.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Filter_Employee, cbo_Filter_PartyName, "Employee_head", "Employee_Name", "", "(Employee_idno = 0)")
    End Sub

    Private Sub cbo_Colour_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_BobinColour.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Colour_Head", "Colour_Name", "", "(Colour_IdNo = 0)")

    End Sub
    Private Sub cbo_BorderName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_BobinColour.KeyDown
        Dim dep_idno As Integer = 0

        vcbo_KeyDwnVal = e.KeyValue

        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_BobinColour, Nothing, Nothing, "Colour_Head", "Colour_Name", "", "(Colour_IdNo = 0)")
        With dgv_BobinDetails

            If (e.KeyValue = 38 And cbo_BobinColour.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
                .Focus()
                .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex - 1)
            End If

            If (e.KeyValue = 40 And cbo_BobinColour.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
                .Focus()
                .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 1)
            End If

        End With
    End Sub

    Private Sub cbo_BorderName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_BobinColour.KeyPress
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_BobinColour, Nothing, "Colour_Head", "Colour_Name", "", "(Colour_IdNo = 0)")

        If Asc(e.KeyChar) = 13 Then
            With dgv_BobinDetails

                .Focus()
                .Rows(.CurrentCell.RowIndex).Cells.Item(3).Value = Trim(cbo_BobinColour.Text)
                .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)

            End With
        End If

    End Sub
    Private Sub cbo_BorderName_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_BobinColour.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Dim f As New Color_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_BobinColour.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If
    End Sub

    Private Sub cbo_BorderName_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_BobinColour.TextChanged
        Try
            If cbo_BobinColour.Visible Then
                With dgv_BobinDetails
                    If Val(cbo_BobinColour.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = 3 Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_BobinColour.Text)
                    End If
                End With
            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub
    Private Sub cbo_BobinCount_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_BobinCount.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Count_Head", "Count_Name", "", "(Count_IdNo = 0)")

    End Sub
    Private Sub cbo_Ends_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_BobinCount.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_BobinCount, Nothing, Nothing, "Count_Head", "Count_Name", "", "(Count_IdNo = 0)")

        With dgv_BobinDetails

            If (e.KeyValue = 38 And cbo_BobinCount.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then

                If Val(.CurrentCell.RowIndex) <= 0 Then
                    txt_Salary.Focus()

                Else
                    .Focus()
                    .CurrentCell = .Rows(.CurrentCell.RowIndex - 1).Cells(.ColumnCount - 2)
                    .CurrentCell.Selected = True

                End If
            End If

            If (e.KeyValue = 40 And cbo_BobinCount.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then

                If .CurrentCell.RowIndex = .Rows.Count - 1 And Trim(.Rows(.CurrentCell.RowIndex).Cells.Item(1).Value) = "" Then
                    If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                        save_record()
                    End If
                Else
                    .Focus()
                    .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)

                End If

            End If

        End With

    End Sub

    Private Sub cbo_Ends_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_BobinCount.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_BobinCount, Nothing, "Count_Head", "Count_Name", "", "(Count_IdNo = 0)")

        If Asc(e.KeyChar) = 13 Then

            With dgv_BobinDetails

                .Focus()
                .Rows(.CurrentCell.RowIndex).Cells.Item(1).Value = Trim(cbo_BobinCount.Text)
                If .CurrentCell.RowIndex = .RowCount - 1 And .CurrentCell.ColumnIndex >= 1 And Trim(.CurrentRow.Cells(1).Value) = "" Then
                    If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                        save_record()
                    End If
                Else
                    .Focus()
                    .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)
                End If

            End With

        End If
    End Sub

    Private Sub cbo_Ends_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_BobinCount.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Dim f As New Count_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_BobinCount.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If
    End Sub

    Private Sub cbo_Ends_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_BobinCount.TextChanged
        Try
            If cbo_BobinCount.Visible Then
                With dgv_BobinDetails
                    If Val(cbo_BobinCount.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = 1 Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_BobinCount.Text)
                    End If
                End With
            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub
    Private Sub Total_Calculation()
        Dim vTotBbnS As Single, vTotMtrs As Single
        Dim i As Integer
        Dim sno As Integer

        vTotBbnS = 0 : vTotMtrs = 0
        With dgv_BobinDetails
            For i = 0 To .Rows.Count - 1

                sno = sno + 1

                .Rows(i).Cells(0).Value = sno

                If Val(.Rows(i).Cells(4).Value) <> 0 Then

                    vTotBbnS = vTotBbnS + Val(.Rows(i).Cells(2).Value)
                    vTotMtrs = vTotMtrs + Val(.Rows(i).Cells(4).Value)

                End If
            Next
        End With

        If dgv_BobinDetails_Total.Rows.Count <= 0 Then dgv_BobinDetails_Total.Rows.Add()

        dgv_BobinDetails_Total.Rows(0).Cells(2).Value = Val(vTotBbnS)
        dgv_BobinDetails_Total.Rows(0).Cells(4).Value = Format(Val(vTotMtrs), "#########0.000")

    End Sub
    Private Sub dgv_filter_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgv_filter.DoubleClick
        Open_FilterEntry()
    End Sub

    Private Sub btn_filtershow_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles btn_filtershow.KeyPress
        If Asc(e.KeyChar) = 13 Then
            cbo_Filter_PartyName.Focus()
        End If
    End Sub

    Private Sub btn_closefilter_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Close_Filter.Click
        pnl_back.Enabled = True
        pnl_filter.Visible = False
        Filter_Status = False
    End Sub

    Private Sub dgv_filter_CellDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_filter.CellDoubleClick
        Open_FilterEntry()
    End Sub

    Private Sub dgv_filter_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_filter.KeyDown
        If e.KeyCode = 13 Then
            Open_FilterEntry()
        End If
    End Sub

    Private Sub Open_FilterEntry()
        Dim movno As String

        movno = Trim(dgv_filter.CurrentRow.Cells(0).Value)

        If Val(movno) <> 0 Then
            Filter_Status = True
            move_record(movno)
            pnl_back.Enabled = True
            pnl_filter.Visible = False
        End If

    End Sub

    Private Sub Cbo_EndsCount_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles Cbo_EndsCount.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "EndsCount_Head", "EndsCount_Name", "", "(EndsCount_IdNo = 0 )")
    End Sub

    Private Sub cbo_EndsCount_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Cbo_EndsCount.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, Cbo_EndsCount, cbo_PartyName, cbo_BobinSize, "EndsCount_Head", "EndsCount_Name", "", "(EndsCount_IdNo = 0)")

    End Sub

    Private Sub cbo_EndsCount_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Cbo_EndsCount.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, Cbo_EndsCount, cbo_BobinSize, "EndsCount_Head", "EndsCount_Name", "", "(EndsCount_IdNo = 0 )")
    End Sub

    _
    Private Sub cbo_EndsCount_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Cbo_EndsCount.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Dim f As New EndsCount_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = Cbo_EndsCount.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If

    End Sub

    Private Sub btn_save_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_save.Click
        save_record()
    End Sub

    Private Sub btn_close_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_close.Click
        Me.Close()
    End Sub



    Private Sub cbo_Colour_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Colour.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then

            Dim f As New Color_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Colour.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1

            f.Show()

        End If
    End Sub

    Private Sub cbo_employee_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_employee.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then

            Dim f As New EmployeeCreation_Simple

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_employee.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1

            f.Show()

        End If
    End Sub

    Private Sub dgv_KuriDetails_CellEndEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_BobinDetails.CellEndEdit
        'Try
        '    With dgv_BobinDetails

        '        If .Rows.Count > 0 Then

        '            If .CurrentCell.ColumnIndex = 4 Then
        '                If Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value) <> 0 Then
        '                    .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = Format(Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value), "#########0.000")
        '                End If
        '            End If

        '        End If

        '    End With

        'Catch ex As Exception
        '    '-----
        'End Try
        dgv_BobinDetails_CellLeave(sender, e)

    End Sub

    Private Sub dgv_KuriDetails_CellEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_BobinDetails.CellEnter
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim Dt2 As New DataTable
        Dim Dt3 As New DataTable
        Dim Dt4 As New DataTable
        Dim Rect As Rectangle

        With dgv_BobinDetails

            dgv_ActCtrlName = .Name.ToString

            If Val(.CurrentRow.Cells(0).Value) = 0 Then
                .CurrentRow.Cells(0).Value = .CurrentRow.Index + 1
            End If

            If e.ColumnIndex = 1 Then

                If cbo_BobinCount.Visible = False Or Val(cbo_BobinCount.Tag) <> e.RowIndex Then

                    dgv_ActCtrlName = dgv_BobinDetails.Name

                    cbo_BobinCount.Tag = -1
                    Da = New SqlClient.SqlDataAdapter("select Count_Name from Count_Head Order by Count_Name", con)
                    Dt2 = New DataTable
                    Da.Fill(Dt2)
                    cbo_BobinCount.DataSource = Dt2
                    cbo_BobinCount.DisplayMember = "Count_Name"

                    Rect = .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False)

                    cbo_BobinCount.Left = .Left + Rect.Left  '  .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Left
                    cbo_BobinCount.Top = .Top + Rect.Top  ' .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Top
                    cbo_BobinCount.Width = Rect.Width  ' .CurrentCell.Size.Width
                    cbo_BobinCount.Height = Rect.Height  ' rect.Height

                    cbo_BobinCount.Text = .CurrentCell.Value  '  Trim(.CurrentRow.Cells(e.ColumnIndex).Value)

                    cbo_BobinCount.Tag = Val(e.RowIndex)
                    cbo_BobinCount.Visible = True

                    cbo_BobinCount.BringToFront()
                    cbo_BobinCount.Focus()

                    'cbo_Grid_CountName.Visible = False
                    'cbo_Grid_MillName.Visible = False

                End If

            Else

                cbo_BobinCount.Visible = False

            End If

            If e.ColumnIndex = 3 Then

                If cbo_BobinColour.Visible = False Or Val(cbo_BobinColour.Tag) <> e.RowIndex Then

                    cbo_BobinColour.Tag = -1
                    Da = New SqlClient.SqlDataAdapter("select Colour_Name from Colour_Head order by Colour_Name", con)
                    Dt2 = New DataTable
                    Da.Fill(Dt2)
                    cbo_BobinColour.DataSource = Dt2
                    cbo_BobinColour.DisplayMember = "Colour_Name"

                    Rect = .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False)

                    cbo_BobinColour.Left = .Left + Rect.Left  '  .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Left
                    cbo_BobinColour.Top = .Top + Rect.Top  ' .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Top
                    cbo_BobinColour.Width = Rect.Width  ' .CurrentCell.Size.Width
                    cbo_BobinColour.Height = Rect.Height  ' rect.Height

                    cbo_BobinColour.Text = .CurrentCell.Value  '  Trim(.CurrentRow.Cells(e.ColumnIndex).Value)

                    cbo_BobinColour.Tag = Val(e.RowIndex)
                    cbo_BobinColour.Visible = True

                    cbo_BobinColour.BringToFront()
                    cbo_BobinColour.Focus()

                End If

            Else

                'cbo_Grid_MillName.Tag = -1
                'cbo_Grid_MillName.Text = ""
                cbo_BobinColour.Visible = False

            End If


        End With

    End Sub

    Private Sub consumption_calculation(ByVal CurRow As Integer, ByVal CurCol As Integer)
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim cnt As Single = 0
        Dim Cnt_ID, EndsCnt_ID As Integer
        Dim conspn As Single = 0
        Dim MTRBOB As Single = 0
        Dim Tot_Reel As Single = 0
        Dim Reel_Wgt As Single = 0
        Dim AA As String = ""

        Try

            With dgv_BobinDetails
                If .Visible Then
                    If .Rows.Count > 0 Then

                        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1027" Then
                            Cnt_ID = 0
                            If Trim(.Rows(CurRow).Cells(1).Value) <> "" Then

                                Cnt_ID = Common_Procedures.Count_NameToIdNo(con, Trim(.Rows(CurRow).Cells(1).Value))

                            End If

                            da = New SqlClient.SqlDataAdapter("select a.* from Count_Head a Where a.Count_IdNo = " & Str(Val(Cnt_ID)), con)
                            dt = New DataTable
                            da.Fill(dt)

                            If dt.Rows.Count > 0 Then
                                If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                                    cnt = Format(Val(dt.Rows(0).Item("Resultant_Count").ToString), "#######0")
                                End If
                            End If

                            dt.Dispose()
                            da.Dispose()

                            EndsCnt_ID = 0

                            If Trim(Cbo_EndsCount.Text) <> "" Then

                                EndsCnt_ID = Common_Procedures.EndsCount_NameToIdNo(con, Trim(Cbo_EndsCount.Text))

                            End If

                            da1 = New SqlClient.SqlDataAdapter("select a.* from EndsCount_Head a Where a.EndsCount_IdNo = " & Str(Val(EndsCnt_ID)), con)
                            dt1 = New DataTable
                            da1.Fill(dt1)

                            If dt1.Rows.Count > 0 Then
                                If IsDBNull(dt1.Rows(0)(0).ToString) = False Then
                                    AA = Trim(dt1.Rows(0).Item("Cotton_Polyester_Jari").ToString)
                                End If
                            End If

                            dt1.Dispose()
                            da1.Dispose()

                            If Trim(UCase(AA)) = "JARI" Then

                                'If Val(txt_No_Bobins.Text) <> 0 And Val(txt_Meters.Text) <> 0 And Val(.Rows(CurRow).Cells(2).Value) <> 0 Then

                                MTRBOB = 0

                                If Val(txt_No_Bobins.Text) <> 0 Then
                                    MTRBOB = Val(txt_Meters.Text) / Val(txt_No_Bobins.Text)
                                End If

                                conspn = (Val(txt_No_Bobins.Text) * Val(MTRBOB) * Val(.Rows(CurRow).Cells(2).Value)) / 3150

                                'If Val(conspn) <> 0 Then

                                .Rows(CurRow).Cells(4).Value = Format(Val(conspn) * 0.053, "#######0.000")

                                'End If

                                'End If

                            Else

                                If Val(txt_No_Bobins.Text) <> 0 And Val(txt_Meters.Text) <> 0 And Val(.Rows(CurRow).Cells(2).Value) <> 0 And Val(cnt) <> 0 Then

                                    conspn = (Val(txt_Meters.Text) * Val(.Rows(CurRow).Cells(2).Value) * Val(cnt)) / 9000000
                                    'conspn = (Val(txt_No_Bobins.Text) * Val(txt_Meters.Text) * Val(.Rows(CurRow).Cells(2).Value) * Val(cnt)) / 9000000
                                    .Rows(CurRow).Cells(4).Value = Format(Val(conspn), "#######0.000")

                                End If
                            End If


                        Else

                            If Val(txt_No_Bobins.Text) <> 0 And Val(txt_Meters.Text) <> 0 And Val(.Rows(CurRow).Cells(2).Value) <> 0 And Val(txt_Meter_Reel.Text) <> 0 Then
                                Tot_Reel = 0
                                Reel_Wgt = 0

                                Tot_Reel = (Val(txt_Meters.Text) * Val(.Rows(CurRow).Cells(2).Value)) / Val(txt_Meter_Reel.Text)

                                If Val(txt_Meter_Reel.Text) = 3000 Then
                                    txt_ReelWeight.Text = 0.052
                                ElseIf Val(txt_Meter_Reel.Text) = 3150 Then
                                    txt_ReelWeight.Text = 0.054
                                ElseIf Val(txt_Meter_Reel.Text) = 3300 Then
                                    txt_ReelWeight.Text = 0.056
                                ElseIf Val(txt_Meter_Reel.Text) = 3450 Then
                                    txt_ReelWeight.Text = 0.058
                                End If

                                If Val(txt_ReelWeight.Text) = 0 Then
                                    txt_ReelWeight.Text = 0.054
                                End If

                                conspn = Val(Tot_Reel) * Val(txt_ReelWeight.Text)
                                'conspn = (Val(txt_No_Bobins.Text) * Val(txt_Meters.Text) * Val(.Rows(CurRow).Cells(2).Value) * Val(cnt)) / 9000000
                                .Rows(CurRow).Cells(4).Value = Format(Val(conspn), "#######0.000")

                            End If

                        End If
                        'If CurCol = 1 Or CurCol = 2 Then


                        'End If
                    End If
                End If
            End With

            Total_Calculation()

        Catch ex As Exception
            '-----

        End Try
    End Sub

    Private Sub dgv_BobinDetails_CellLeave(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_BobinDetails.CellLeave
        With dgv_BobinDetails
            If .CurrentCell.ColumnIndex = 4 Then
                If Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value) <> 0 Then
                    .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = Format(Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value), "#########0.000")
                End If
            End If
        End With
        Total_Calculation()
    End Sub

    Private Sub dgv_KuriDetails_CellValueChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_BobinDetails.CellValueChanged

        On Error Resume Next


        If IsNothing(dgv_BobinDetails.CurrentCell) Then Exit Sub
        With dgv_BobinDetails
            If .Visible Then

                If .CurrentCell.ColumnIndex = 1 Or .CurrentCell.ColumnIndex = 2 Then
                    salary_calculation()
                    consumption_calculation(.CurrentCell.RowIndex, .CurrentCell.ColumnIndex)

                End If

            End If
        End With

    End Sub

    Private Sub dgv_KuriDetails_EditingControlShowing(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewEditingControlShowingEventArgs) Handles dgv_BobinDetails.EditingControlShowing
        dgtxt_BobinDetails = CType(dgv_BobinDetails.EditingControl, DataGridViewTextBoxEditingControl)
    End Sub

    Private Sub dgtxt_KuriDetails_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgtxt_BobinDetails.Enter
        dgv_ActCtrlName = dgv_BobinDetails.Name
        dgv_BobinDetails.EditingControl.BackColor = Color.Lime
        dgv_BobinDetails.EditingControl.ForeColor = Color.Blue
        dgtxt_BobinDetails.SelectAll()
    End Sub

    Private Sub dgtxt_KuriDetails_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dgtxt_BobinDetails.KeyPress

        With dgv_BobinDetails

            If Val(dgv_BobinDetails.CurrentCell.ColumnIndex.ToString) = 2 Then

                If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
                    e.Handled = True
                End If

            End If

        End With

    End Sub

    Private Sub dgv_KuriDetails_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_BobinDetails.KeyUp
        Dim n As Integer

        If e.Control = True And UCase(Chr(e.KeyCode)) = "D" Then

            With dgv_BobinDetails

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

            Total_Calculation()

        End If

    End Sub

    Private Sub dgv_KuriDetails_RowsAdded(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewRowsAddedEventArgs) Handles dgv_BobinDetails.RowsAdded
        Dim n As Integer


        If IsNothing(dgv_BobinDetails.CurrentCell) Then Exit Sub
        With dgv_BobinDetails
            n = .RowCount
            .Rows(n - 1).Cells(0).Value = Val(n)
        End With

    End Sub

    Private Sub dgv_KuriDetails_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgv_BobinDetails.LostFocus
        On Error Resume Next
        If Not IsNothing(dgv_BobinDetails.CurrentCell) Then dgv_BobinDetails.CurrentCell.Selected = False
    End Sub

    Private Sub txt_No_Bobins_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_No_Bobins.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub txt_No_Bobins_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_No_Bobins.TextChanged
        Dim i As Integer = 0

        On Error Resume Next

        With dgv_BobinDetails
            If .Visible Then

                For i = 0 To .Rows.Count - 1
                    consumption_calculation(i, 8)
                Next

            End If
        End With
        salary_calculation()
        txt_Salary.Text = Val(txt_No_Bobins.Text) * Val(txt_SalaryBobin.Text)

    End Sub

    Private Sub txt_Meters_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_Meters.TextChanged
        Dim i As Integer = 0

        On Error Resume Next

        With dgv_BobinDetails
            If .Visible Then

                For i = 0 To .Rows.Count - 1
                    consumption_calculation(i, 8)
                Next

            End If
        End With
    End Sub

    Private Sub btn_SaveAs_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_SaveAs.Click
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim NewID As Integer = 0

        Try
            'clear()

            New_Entry = True

            da = New SqlClient.SqlDataAdapter("select max(for_orderby) from Bobin_Production_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Bobin_Production_code like '%/" & Trim(Common_Procedures.FnYearCode) & "' ", con)
            da.Fill(dt)

            NewID = 0
            If dt.Rows.Count > 0 Then
                If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                    NewID = Val(dt.Rows(0)(0).ToString)
                End If
            End If

            NewID = NewID + 1

            lbl_RefNo.Text = NewID
            lbl_RefNo.ForeColor = Color.Red

            If dtp_Date.Enabled And dtp_Date.Visible Then dtp_Date.Focus()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR NEW RECORD...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub
    Private Sub txt_Meters_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Meters.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub txt_Salary_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_Salary.KeyDown
        If e.KeyCode = 40 Then
            If dgv_BobinDetails.Rows.Count > 0 Then
                dgv_BobinDetails.Focus()
                dgv_BobinDetails.CurrentCell = dgv_BobinDetails.Rows(0).Cells(1)
                dgv_BobinDetails.CurrentCell.Selected = True

            Else
                If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                    save_record()
                End If

            End If
        End If
        If e.KeyCode = 38 Then SendKeys.Send("+{TAB}")
    End Sub

    Private Sub txt_Salary_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Salary.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
        If Asc(e.KeyChar) = 13 Then
            If dgv_BobinDetails.Rows.Count > 0 Then
                dgv_BobinDetails.Focus()
                dgv_BobinDetails.CurrentCell = dgv_BobinDetails.Rows(0).Cells(1)
                dgv_BobinDetails.CurrentCell.Selected = True

            Else
                If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                    save_record()
                End If

            End If

        End If
    End Sub

    Private Sub txt_SalaryBobin_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_SalaryBobin.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub txt_SalaryBobin_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_SalaryBobin.TextChanged
        txt_Salary.Text = Val(txt_No_Bobins.Text) * Val(txt_SalaryBobin.Text)
    End Sub

    Private Sub cbo_Filter_Colour_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Filter_Colour.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Colour_Head", "Colour_Name", "", "(Colour_idno = 0)")
    End Sub

    Private Sub cbo_Filter_Colour_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Filter_Colour.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_Colour, cbo_Filter_PartyName, btn_filtershow, "Colour_Head", "Colour_Name", "", "(Colour_idno = 0)")
    End Sub

    Private Sub cbo_Filter_Colour_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Filter_Colour.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Filter_Colour, btn_filtershow, "Colour_Head", "Colour_Name", "", "(Colour_idno = 0)")
    End Sub

    Private Sub txt_Meter_Bobin_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Meter_Bobin.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub txt_Meter_Bobin_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_Meter_Bobin.LostFocus
        txt_Meters.Text = Val(txt_No_Bobins.Text) * Val(txt_Meter_Bobin.Text)
    End Sub
    Private Sub txt_No_Bobins_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_No_Bobins.LostFocus
        txt_Meters.Text = Val(txt_No_Bobins.Text) * Val(txt_Meter_Bobin.Text)
    End Sub

    Private Sub salary_calculation()
        Dim Rate As Double = 0
        Dim ends As Double = 0
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1116" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1380" Then '---- St.LOURDU MATHA TEX (Somanur)
            If dgv_BobinDetails.Rows.Count > 0 Then
                ends = dgv_BobinDetails.Rows(0).Cells(2).Value
            End If

            If ends >= 0 And ends <= 59 Then
                Rate = 0.0125
            ElseIf ends > 59 And ends <= 100 Then
                Rate = 0.015
            ElseIf ends > 100 And ends <= 200 Then
                Rate = 0.02
            ElseIf ends > 200 And ends <= 300 Then
                Rate = 0.03
            End If

            txt_SalaryBobin.Text = Rate * Val(txt_Meter_Bobin.Text)
        End If


    End Sub

    Private Sub txt_Meter_Bobin_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_Meter_Bobin.TextChanged
        salary_calculation()
    End Sub
    Private Sub msk_Date_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles msk_Date.KeyDown
        vcbo_KeyDwnVal = e.KeyValue

        If e.KeyCode = 40 Then
            e.Handled = True : e.SuppressKeyPress = True
            cbo_employee.Focus()
        End If

        If e.KeyCode = 38 Then
            e.Handled = True : e.SuppressKeyPress = True
            txt_Salary.Focus()
        End If

        vmskOldText = ""
        vmskSelStrt = -1
        If e.KeyCode = 46 Or e.KeyCode = 8 Then
            vmskOldText = msk_Date.Text
            vmskSelStrt = msk_Date.SelectionStart
        End If

    End Sub

    Private Sub msk_Date_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles msk_Date.KeyPress
        If Trim(UCase(e.KeyChar)) = "D" Then
            msk_Date.Text = Date.Today
            msk_Date.SelectionStart = 0
        End If
        If Asc(e.KeyChar) = 13 Then
            e.Handled = True
            cbo_employee.Focus()
        End If
    End Sub


    Private Sub msk_Date_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles msk_Date.KeyUp
        Dim vmRetTxt As String = ""
        Dim vmRetSelStrt As Integer = -1

        If e.KeyCode = 107 Then
            msk_Date.Text = DateAdd("D", 1, Convert.ToDateTime(msk_Date.Text))
            msk_Date.SelectionStart = 0
        ElseIf e.KeyCode = 109 Then
            msk_Date.Text = DateAdd("D", -1, Convert.ToDateTime(msk_Date.Text))
            msk_Date.SelectionStart = 0
        End If

        If e.KeyCode = 46 Or e.KeyCode = 8 Then
            Common_Procedures.maskEdit_Date_ON_DelBackSpace(sender, e, vmskOldText, vmskSelStrt)
        End If

    End Sub

    Private Sub dtp_Date_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dtp_Date.KeyDown

        If e.KeyCode = 40 Then
            e.Handled = True : e.SuppressKeyPress = True
            msk_Date.Focus()
        End If

        If e.KeyCode = 38 Then
            e.Handled = True : e.SuppressKeyPress = True
            msk_Date.Focus()
        End If
    End Sub

    Private Sub dtp_Date_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dtp_Date.TextChanged

        If IsDate(dtp_Date.Text) = True Then

            msk_Date.Text = dtp_Date.Text
            msk_Date.SelectionStart = 0
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

    Private Sub dtp_Date_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dtp_Date.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            dtp_Date.Text = Date.Today
        End If
    End Sub

    Private Sub cbo_BobinSize_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_BobinSize.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Bobin_Size_Head", "Bobin_Size_Name", "", "(Bobin_Size_IdNo = 0)")
    End Sub

    Private Sub cbo_BobinSize_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_BobinSize.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_BobinSize, Cbo_EndsCount, txt_No_Bobins, "Bobin_Size_Head", "Bobin_Size_Name", "", "(Bobin_Size_IdNo = 0)")
        'If e.KeyCode = 38 And cbo_BobinSize.DroppedDown = False Or (e.KeyCode = 38 And e.Control = True) Then
        '    If Cbo_EndsCount.Visible Then
        '        Cbo_EndsCount.Focus()
        '    Else
        '        cbo_PartyName.Focus()
        '    End If
        'End If
        'If e.KeyCode = 40 And cbo_BobinSize.DroppedDown = False Or (e.KeyCode = 40 And e.Control = True) Then
        '    If txt_No_Bobins.Visible Then
        '        txt_No_Bobins.Focus()
        '    Else
        '        txt_Meter_Bobin.Focus()
        '    End If
        'End If
    End Sub

    Private Sub cbo_BobinSize_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_BobinSize.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_BobinSize, txt_No_Bobins, "Bobin_Size_Head", "Bobin_Size_Name", "", "(Bobin_Size_IdNo = 0)")
        'If Asc(e.KeyChar) = 13 Then
        '    If txt_No_Bobins.Visible Then
        '        txt_No_Bobins.Focus()
        '    Else
        '        txt_Meter_Bobin.Focus()
        '    End If
        'End If
    End Sub

    Private Sub cbo_BobinSize_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_BobinSize.KeyUp
        If e.KeyCode = 17 And e.Control = False Then
            e.Handled = True
            Dim F As New Bobin_Size_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_BobinSize.Name
            Common_Procedures.Master_Return.Master_Type = ""
            Common_Procedures.Master_Return.Return_Value = ""

            F.MdiParent = MDIParent1
            F.Show()
        End If
    End Sub

    Private Sub btn_UserModification_Click(sender As Object, e As System.EventArgs) Handles btn_UserModification.Click
        If Val(Common_Procedures.User.IdNo) = 1 Then
            Dim f1 As New User_Modifications
            f1.Entry_Name = Me.Name
            f1.Entry_PkValue = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)
            f1.ShowDialog()
        End If
    End Sub
End Class