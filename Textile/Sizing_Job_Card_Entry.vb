Public Class Sizing_Job_Card_Entry
    Implements Interface_MDIActions

    Private con As New SqlClient.SqlConnection(Common_Procedures.Connection_String)
    Private con1 As New SqlClient.SqlConnection(Common_Procedures.ConnectionString_CompanyGroupdetails)
    Private FrmLdSTS As Boolean = False
    Private New_Entry As Boolean = False
    Private Insert_Entry As Boolean = False
    Private Filter_Status As Boolean = False
    Private Pk_Condition As String = "SZJOB-"
    Private Prec_ActCtrl As New Control
    Private vCbo_ItmNm As String
    Private vcbo_KeyDwnVal As Double

    Private WithEvents dgtxt_Details As New DataGridViewTextBoxEditingControl

    Private prn_HdDt As New DataTable
    Private prn_DetDt As New DataTable
    Private prn_PageNo As Integer
    Private prn_DetIndx As Integer
    Private prn_DetSNo As Integer

    Private Sub clear()

        New_Entry = False
        Insert_Entry = False

        pnl_Back.Enabled = True
        pnl_Filter.Visible = False



        lbl_JobNo.Text = ""
        lbl_JobNo.ForeColor = Color.Black

        dtp_Date.Text = ""
        txt_WarpMeters.Text = "0"
        txt_PcsLength.Text = "0"
        txt_TapeLength.Text = ""
        cbo_BeamWidth.Text = ""
        cbo_MillName.Text = ""
        cbo_CountName.Text = ""
        txt_Ends.Text = ""
        lbl_JobNo.Text = ""
        cbo_Ledger.Text = ""
        txt_Remarks.Text = ""


        txt_InvoiceCode.Text = ""
        txt_BabyCone_DeliveryWeight.Text = ""

        cbo_Ledger.Enabled = True
        cbo_Ledger.BackColor = Color.White

        cbo_CountName.Enabled = True
        cbo_CountName.BackColor = Color.White

        cbo_MillName.Enabled = True
        cbo_MillName.BackColor = Color.White

        'txt_BabyCone_TareWeight.Enabled = True
        'txt_BabyCone_TareWeight.BackColor = Color.White

        txt_SlNo.Text = ""
        cbo_CountName.Text = ""
        cbo_YarnType.Text = "MILL"
        cbo_MillName.Text = ""
        txt_Bags.Text = ""
        cbo_SetNo.Text = ""
        txt_Cones.Text = ""
        txt_Weight.Text = ""

        If Filter_Status = False Then
            dtp_Filter_Fromdate.Text = ""
            dtp_Filter_ToDate.Text = ""
            cbo_Filter_PartyName.Text = ""
            cbo_Filter_CountName.Text = ""
            cbo_Filter_PartyName.SelectedIndex = -1
            cbo_Filter_CountName.SelectedIndex = -1
            dgv_Filter_Details.Rows.Clear()
        End If

        chk_Printed.Checked = False
        chk_Printed.Enabled = False
        chk_Printed.Visible = False

        cbo_Ledger.Tag = ""
        cbo_YarnType.Tag = ""
        cbo_CountName.Tag = ""
        cbo_MillName.Tag = ""

        dgv_Details.Rows.Clear()
        dgv_Details_Total.Rows.Clear()
        dgv_Details_Total.Rows.Add()

        Grid_Cell_DeSelect()

        txt_SlNo.Text = "1"
        cbo_YarnType.Text = "MILL"
        lbl_UserName.Text = "USER : " & Trim(UCase(Common_Procedures.User_IdNoToName(con1, Common_Procedures.User.IdNo)))
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

        'If Me.ActiveControl.Name <> cbo_ItemName.Name Then
        '    cbo_ItemName.Visible = False
        'End If
        'If Me.ActiveControl.Name <> cbo_PackingType.Name Then
        '    cbo_PackingType.Visible = False
        'End If




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
        If Not IsNothing(dgv_Details.CurrentCell) Then dgv_Details.CurrentCell.Selected = False
        If Not IsNothing(dgv_Details_Total.CurrentCell) Then dgv_Details_Total.CurrentCell.Selected = False
        If Not IsNothing(dgv_Filter_Details.CurrentCell) Then dgv_Filter_Details.CurrentCell.Selected = False
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

            da1 = New SqlClient.SqlDataAdapter("select a.*, b.Ledger_Name, c.mill_name, d.count_name, e.Beam_Width_Name ,f.Transport_Name from Job_Card_Head a INNER JOIN Ledger_Head b ON a.Ledger_IdNo = b.Ledger_IdNo LEFT OUTER JOIN Mill_Head c ON a.Mill_IdNo = c.Mill_IdNo LEFT OUTER JOIN count_Head d ON a.count_IdNo = d.count_IdNo LEFT OUTER JOIN Beam_Width_Head e ON a.Beam_Width_IdNo = e.Beam_Width_IdNo LEFT OUTER JOIN Transport_Head f ON a.Transport_IdNo = f.Transport_IdNo Where a.Job_Card_Code = '" & Trim(NewCode) & "'", con)
            dt1 = New DataTable
            da1.Fill(dt1)
            lbl_UserName.Text = "USER : " & Trim(UCase(Common_Procedures.User_IdNoToName(con1, Val(dt1.Rows(0).Item("User_IdNo").ToString))))

            If dt1.Rows.Count > 0 Then
                lbl_JobNo.Text = dt1.Rows(0).Item("Job_Card_No").ToString
                dtp_Date.Text = dt1.Rows(0).Item("Job_Card_Date").ToString

                cbo_Ledger.Text = dt1.Rows(0).Item("Ledger_Name").ToString
                cbo_CountName.Text = dt1.Rows(0).Item("count_name").ToString
                cbo_MillName.Text = dt1.Rows(0).Item("mill_name").ToString

                cbo_BeamWidth.Text = dt1.Rows(0).Item("Beam_Width_Name").ToString
                txt_Ends.Text = dt1.Rows(0).Item("ends_name").ToString
                txt_PcsLength.Text = dt1.Rows(0).Item("pcs_length").ToString
                txt_TapeLength.Text = Val(dt1.Rows(0).Item("tape_length").ToString)
                txt_WarpMeters.Text = dt1.Rows(0).Item("warp_meters").ToString
                txt_Remarks.Text = dt1.Rows(0).Item("Remarks").ToString

                chk_Printed.Checked = False
                chk_Printed.Enabled = False
                chk_Printed.Visible = False
                If Val(dt1.Rows(0).Item("PrintOut_Status").ToString) = 1 Then
                    chk_Printed.Checked = True
                    chk_Printed.Visible = True
                    If Val(Common_Procedures.User.IdNo) = 1 Then
                        chk_Printed.Enabled = True
                    End If
                End If

                da2 = New SqlClient.SqlDataAdapter("select a.*, b.Count_Name, c.Mill_Name from Job_Card_Details a INNER JOIN Count_Head b on a.Count_IdNo = b.Count_IdNo INNER JOIN Mill_Head c on a.Mill_IdNo = c.Mill_IdNo where a.Job_Card_Code = '" & Trim(NewCode) & "' Order by a.sl_no", con)
                dt2 = New DataTable
                da2.Fill(dt2)

                dgv_Details.Rows.Clear()
                SNo = 0

                If dt2.Rows.Count > 0 Then

                    For i = 0 To dt2.Rows.Count - 1

                        n = dgv_Details.Rows.Add()

                        SNo = SNo + 1
                        dgv_Details.Rows(n).Cells(0).Value = Val(SNo)
                        dgv_Details.Rows(n).Cells(1).Value = dt2.Rows(i).Item("Count_Name").ToString
                        dgv_Details.Rows(n).Cells(2).Value = dt2.Rows(i).Item("Yarn_Type").ToString
                        dgv_Details.Rows(n).Cells(3).Value = dt2.Rows(i).Item("setcode_forSelection").ToString
                        dgv_Details.Rows(n).Cells(4).Value = dt2.Rows(i).Item("Mill_Name").ToString
                        dgv_Details.Rows(n).Cells(5).Value = Val(dt2.Rows(i).Item("Bags").ToString)
                        dgv_Details.Rows(n).Cells(6).Value = Val(dt2.Rows(i).Item("Cones").ToString)
                        dgv_Details.Rows(n).Cells(7).Value = Format(Val(dt2.Rows(i).Item("Weight").ToString), "########0.000")

                    Next i

                End If

                SNo = SNo + 1
                txt_SlNo.Text = Val(SNo)

                With dgv_Details_Total
                    If .RowCount = 0 Then .Rows.Add()
                    .Rows(0).Cells(5).Value = Val(dt1.Rows(0).Item("Total_Bags").ToString)
                    .Rows(0).Cells(6).Value = Val(dt1.Rows(0).Item("Total_Cones").ToString)
                    .Rows(0).Cells(7).Value = Format(Val(dt1.Rows(0).Item("Total_Weight").ToString), "########0.000")
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
            MessageBox.Show(ex.Message, "DOES NOT MOVE RECORDS...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        If cbo_Ledger.Visible And cbo_Ledger.Enabled Then cbo_Ledger.Focus()

    End Sub

    Private Sub Job_Card_Entry_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated

        Try

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Ledger.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "LEDGER" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Ledger.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If


            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_CountName.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "COUNT" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_CountName.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_GridMillName.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "MILL" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_GridMillName.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Grid_Countname.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "COUNT" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Grid_Countname.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_MillName.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "MILL" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_MillName.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If


            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_BeamWidth.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "BEAMWIDTH" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_BeamWidth.Text = Trim(Common_Procedures.Master_Return.Return_Value)
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

    Private Sub Job_Card_Entry_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim dt3 As New DataTable
        Dim dt4 As New DataTable
        Dim dt5 As New DataTable
        Dim dt6 As New DataTable
        Dim dt7 As New DataTable
        Dim dt8 As New DataTable
        Dim dt9 As New DataTable
        Dim dt10 As New DataTable

        Me.Text = ""

        con.Open()

        da = New SqlClient.SqlDataAdapter("select a.Ledger_DisplayName from Ledger_AlaisHead a, ledger_head b where (a.Ledger_IdNo = 0 or b.AccountsGroup_IdNo = 10 and Close_Status = 0) and a.Ledger_IdNo = b.Ledger_IdNo order by a.Ledger_DisplayName", con)
        da.Fill(dt1)
        cbo_Ledger.DataSource = dt1
        cbo_Ledger.DisplayMember = "Ledger_DisplayName"

        da = New SqlClient.SqlDataAdapter("select count_name from Count_Head order by count_name", con)
        da.Fill(dt2)
        cbo_CountName.DataSource = dt2
        cbo_CountName.DisplayMember = "count_name"


        da = New SqlClient.SqlDataAdapter("select mill_name from Mill_Head order by mill_name", con)
        da.Fill(dt3)
        cbo_MillName.DataSource = dt3
        cbo_MillName.DisplayMember = "mill_name"

        da = New SqlClient.SqlDataAdapter("select mill_name from Mill_Head order by mill_name", con)
        da.Fill(dt4)
        cbo_GridMillName.DataSource = dt4
        cbo_GridMillName.DisplayMember = "mill_name"

        da = New SqlClient.SqlDataAdapter("select Yarn_Type from YarnType_Head order by Yarn_Type", con)
        da.Fill(dt5)
        cbo_YarnType.DataSource = dt5
        cbo_YarnType.DisplayMember = "Yarn_Type"

        da = New SqlClient.SqlDataAdapter("select Beam_Width_Name from Beam_Width_Head order by Beam_Width_Name", con)
        da.Fill(dt6)
        cbo_BeamWidth.DataSource = dt6
        cbo_BeamWidth.DisplayMember = "Beam_Width_Name"

        da = New SqlClient.SqlDataAdapter("select distinct(setcode_forSelection) from Stock_BabyCone_Processing_Details order by setcode_forSelection", con)
        da.Fill(dt8)
        cbo_SetNo.DataSource = dt8
        cbo_SetNo.DisplayMember = "setcode_forSelection"

        da = New SqlClient.SqlDataAdapter("select count_name from Count_Head order by count_name", con)
        da.Fill(dt9)
        cbo_Grid_Countname.DataSource = dt9
        cbo_Grid_Countname.DisplayMember = "count_name"

        pnl_Filter.Visible = False
        pnl_Filter.Left = (Me.Width - pnl_Filter.Width) \ 2
        pnl_Filter.Top = (Me.Height - pnl_Filter.Height) \ 2
        pnl_Filter.BringToFront()

        btn_UserModification.Visible = False
        chk_Printed.Enabled = False
        If Val(Common_Procedures.User.IdNo) = 1 Then
            btn_UserModification.Visible = True
            chk_Printed.Enabled = True
        End If

        AddHandler dtp_Date.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Ledger.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_CountName.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_MillName.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_SetNo.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_YarnType.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_TapeLength.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Bags.GotFocus, AddressOf ControlGotFocus

        AddHandler txt_Cones.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Weight.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_PcsLength.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Remarks.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_SlNo.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_BeamWidth.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Grid_Countname.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Filter_CountName.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_GridMillName.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Ends.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_WarpMeters.GotFocus, AddressOf ControlGotFocus


        AddHandler dtp_Filter_Fromdate.GotFocus, AddressOf ControlGotFocus
        AddHandler dtp_Filter_ToDate.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Filter_PartyName.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Filter_MillName.GotFocus, AddressOf ControlGotFocus

        AddHandler btn_save.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_Add.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_close.GotFocus, AddressOf ControlGotFocus

        AddHandler dtp_Date.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Ledger.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_CountName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_MillName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_SetNo.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_YarnType.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Ends.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Bags.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Cones.LostFocus, AddressOf ControlLostFocus

        AddHandler txt_Weight.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Filter_CountName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Grid_Countname.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_WarpMeters.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_SlNo.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_BeamWidth.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_GridMillName.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_TapeLength.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_PcsLength.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Remarks.LostFocus, AddressOf ControlLostFocus


        'AddHandler txt_EmptyCones.LostFocus, AddressOf ControlLostFocus

        AddHandler dtp_Filter_Fromdate.LostFocus, AddressOf ControlLostFocus
        AddHandler dtp_Filter_ToDate.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Filter_PartyName.LostFocus, AddressOf ControlLostFocus

        AddHandler cbo_Filter_MillName.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_save.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_Add.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_close.LostFocus, AddressOf ControlLostFocus

        AddHandler dtp_Date.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_WarpMeters.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Bags.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Cones.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Weight.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_TapeLength.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_PcsLength.KeyDown, AddressOf TextBoxControlKeyDown

        AddHandler txt_Remarks.KeyDown, AddressOf TextBoxControlKeyDown

        AddHandler dtp_Filter_Fromdate.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler dtp_Filter_ToDate.KeyDown, AddressOf TextBoxControlKeyDown

        AddHandler dtp_Date.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_WarpMeters.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Bags.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Cones.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_TapeLength.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_PcsLength.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler dtp_Filter_Fromdate.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler dtp_Filter_ToDate.KeyPress, AddressOf TextBoxControlKeyPress

        lbl_Company.Text = ""
        lbl_Company.Tag = 0
        lbl_Company.Visible = False
        Common_Procedures.CompIdNo = 0

        Filter_Status = False
        FrmLdSTS = True
        new_record()

    End Sub

    Private Sub Job_Card_Entry_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        On Error Resume Next
        con.Close()
        con.Dispose()
        Common_Procedures.Last_Closed_FormName = Me.Name
    End Sub

    Private Sub Job_Card_Entry_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress

        Try
            If Asc(e.KeyChar) = 27 Then

                If pnl_Filter.Visible = True Then
                    btn_Filter_Close_Click(sender, e)
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

    Public Sub delete_record() Implements Interface_MDIActions.delete_record
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim cmd As New SqlClient.SqlCommand
        Dim trans As SqlClient.SqlTransaction
        Dim NewCode As String = ""
        Dim UID As Single = 0
        Dim vUsrNm As String = "", vAcPwd As String = "", vUnAcPwd As String = ""


        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1036" Then '----- KALAIMAGAL TEXTILES (AVINASHI)
            Common_Procedures.Password_Input = ""
            Dim g As New Admin_Password
            g.ShowDialog()

            UID = 1
            Common_Procedures.get_Admin_Name_PassWord_From_DB(vUsrNm, vAcPwd, vUnAcPwd)

            vAcPwd = Common_Procedures.Decrypt(Trim(vAcPwd), Trim(Common_Procedures.UserCreation_AcPassWord.passPhrase) & Trim(Val(UID)) & Trim(UCase(vUsrNm)), Trim(Common_Procedures.UserCreation_AcPassWord.saltValue) & Trim(Val(UID)) & Trim(UCase(vUsrNm)))
            vUnAcPwd = Common_Procedures.Decrypt(Trim(vUnAcPwd), Trim(Common_Procedures.UserCreation_UnAcPassWord.passPhrase) & Trim(Val(UID)) & Trim(UCase(vUsrNm)), Trim(Common_Procedures.UserCreation_UnAcPassWord.saltValue) & Trim(Val(UID)) & Trim(UCase(vUsrNm)))

            If Trim(Common_Procedures.Password_Input) <> Trim(vAcPwd) And Trim(Common_Procedures.Password_Input) <> Trim(vUnAcPwd) Then
                MessageBox.Show("Invalid Admin Password", "ADMIN PASSWORD FAILED...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                Exit Sub
            End If
        End If

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_JobNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        'If Common_Procedures.UserRight_Check(Common_Procedures.UserRightsCheckFor.DeletingEntry, Common_Procedures.UR.entry_jobcard_entry, New_Entry, Me, con, "job_card_Head", "Job_Card_Code", NewCode, "Job_Card_Date", "(Job_Card_Code = '" & Trim(NewCode) & "')") = False Then Exit Sub

        ' If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.Yarn_Delivery_Entry, "~L~") = 0 And InStr(Common_Procedures.UR.Yarn_Delivery_Entry, "~D~") = 0 Then MessageBox.Show("You have No Rights to Insert", "DOES NOT INSERT...", MessageBoxButtons.OK, MessageBoxIcon.Error) : Exit Sub


        If MessageBox.Show("Do you want to Delete?", "FOR DELETION...", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.No Then
            Exit Sub
        End If

        If New_Entry = True Then
            MessageBox.Show("This is New Entry", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        trans = con.BeginTransaction

        Try

            'NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_JobNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            cmd.Connection = con
            cmd.Transaction = trans

            Da = New SqlClient.SqlDataAdapter("Select * from Job_Card_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Job_Card_Code = '" & Trim(NewCode) & "' and yarn_type = 'BABY'", con)
            Da.SelectCommand.Transaction = trans
            Dt1 = New DataTable
            Da.Fill(Dt1)

            If Dt1.Rows.Count > 0 Then

                For i = 0 To Dt1.Rows.Count - 1

                    cmd.CommandText = "Update Stock_BabyCone_Processing_Details set Delivered_Bags = Delivered_Bags - " & Str(Val(Dt1.Rows(i).Item("Bags").ToString)) & ", Delivered_Cones = Delivered_Cones - " & Str(Val(Dt1.Rows(i).Item("cones").ToString)) & ", Delivered_Weight = Delivered_Weight - " & Str(Val(Dt1.Rows(i).Item("Weight").ToString)) & " Where setcode_forSelection = '" & Trim(Dt1.Rows(i).Item("setcode_forSelection").ToString) & "'"
                    'cmd.CommandText = "Update Stock_BabyCone_Processing_Details set Delivered_Bags = Delivered_Bags - " & Str(Val(Dt1.Rows(i).Item("Bags").ToString)) & ", Delivered_Cones = Delivered_Cones - " & Str(Val(Dt1.Rows(i).Item("cones").ToString)) & ", Delivered_Weight = Delivered_Weight - " & Str(Val(Dt1.Rows(i).Item("Weight").ToString)) & " Where setcode_forSelection = '" & Trim(Dt1.Rows(i).Item("setcode_forSelection").ToString) & "' and Company_IdNo = " & Str(Val(Dt1.Rows(i).Item("Company_IdNo").ToString))
                    cmd.ExecuteNonQuery()

                Next i

            End If
            Dt1.Clear()


            cmd.CommandText = "Delete from Stock_Empty_BeamBagCone_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Stock_Yarn_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "delete from Job_Card_Details where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Job_Card_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "delete from Job_Card_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Job_Card_Code = '" & Trim(NewCode) & "'"
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

        End Try

        If cbo_Ledger.Enabled = True And cbo_Ledger.Visible = True Then cbo_Ledger.Focus()

    End Sub

    Public Sub filter_record() Implements Interface_MDIActions.filter_record

        If Filter_Status = False Then

            Dim da As New SqlClient.SqlDataAdapter
            Dim dt1 As New DataTable
            Dim dt2 As New DataTable
            Dim dt3 As New DataTable

            da = New SqlClient.SqlDataAdapter("select a.Ledger_DisplayName from Ledger_AlaisHead a, ledger_head b where (a.Ledger_IdNo = 0 or b.AccountsGroup_IdNo = 10 ) and a.Ledger_IdNo = b.Ledger_IdNo order by a.Ledger_DisplayName", con)
            da.Fill(dt1)
            cbo_Filter_PartyName.DataSource = dt1
            cbo_Filter_PartyName.DisplayMember = "Ledger_DisplayName"

            da = New SqlClient.SqlDataAdapter("select Count_name from Count_Head order by count_name", con)
            da.Fill(dt2)
            cbo_Filter_CountName.DataSource = dt2
            cbo_Filter_CountName.DisplayMember = "count_name"

            da = New SqlClient.SqlDataAdapter("select Mill_name from Mill_Head order by Mill_name", con)
            da.Fill(dt3)
            cbo_Filter_MillName.DataSource = dt3
            cbo_Filter_MillName.DisplayMember = "Mill_name"

            dtp_Filter_Fromdate.Text = ""
            dtp_Filter_ToDate.Text = ""
            cbo_Filter_PartyName.Text = ""
            cbo_Filter_CountName.Text = ""
            cbo_Filter_MillName.Text = ""
            cbo_Filter_PartyName.SelectedIndex = -1
            cbo_Filter_CountName.SelectedIndex = -1
            cbo_Filter_MillName.SelectedIndex = -1
            dgv_Filter_Details.Rows.Clear()

        End If

        pnl_Filter.Visible = True
        pnl_Filter.Enabled = True
        pnl_Back.Enabled = False
        If dtp_Filter_Fromdate.Enabled And dtp_Filter_Fromdate.Visible Then dtp_Filter_Fromdate.Focus()

    End Sub

    Public Sub movefirst_record() Implements Interface_MDIActions.movefirst_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim movno As String

        Try

            da = New SqlClient.SqlDataAdapter("select top 1 Job_Card_No from Job_Card_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Job_Card_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby, Job_Card_No", con)
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

            OrdByNo = Common_Procedures.OrderBy_CodeToValue(Trim(lbl_JobNo.Text))

            da = New SqlClient.SqlDataAdapter("select top 1 Job_Card_No from Job_Card_Head where for_orderby > " & Str(OrdByNo) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and Job_Card_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby, Job_Card_No", con)
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

            OrdByNo = Common_Procedures.OrderBy_CodeToValue(Trim(lbl_JobNo.Text))

            da = New SqlClient.SqlDataAdapter("select top 1 Job_Card_No from Job_Card_Head where for_orderby < " & Str(OrdByNo) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and Job_Card_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, Job_Card_No desc", con)
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
            da = New SqlClient.SqlDataAdapter("select top 1 Job_Card_No from Job_Card_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Job_Card_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, Job_Card_No desc", con)
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

            lbl_JobNo.Text = Common_Procedures.get_MaxCode(con, "Job_Card_Head", "Job_Card_Code", "For_OrderBy", "", Val(lbl_Company.Tag), Common_Procedures.FnYearCode)

            lbl_JobNo.ForeColor = Color.Red

            If cbo_Ledger.Enabled And cbo_Ledger.Visible Then cbo_Ledger.Focus()

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
        Dim RecCode As String

        Try

            inpno = InputBox("Enter Job No.", "FOR FINDING...")

            RecCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

            Da = New SqlClient.SqlDataAdapter("select Job_Card_No from Job_Card_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Job_Card_Code = '" & Trim(RecCode) & "'", con)
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
                MessageBox.Show("Job No. does not exists", "DOES NOT FIND...", MessageBoxButtons.OK, MessageBoxIcon.Error)

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT FIND...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Public Sub insert_record() Implements Interface_MDIActions.insert_record
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim movno As String, inpno As String
        Dim RecCode As String

        If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.Yarn_Delivery_Entry, "~L~") = 0 And InStr(Common_Procedures.UR.Yarn_Delivery_Entry, "~I~") = 0 Then MessageBox.Show("You have No Rights to Insert", "DOES NOT INSERT...", MessageBoxButtons.OK, MessageBoxIcon.Error) : Exit Sub

        Try

            inpno = InputBox("Enter New Job No.", "FOR NEW JOB INSERTION...")

            RecCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

            Da = New SqlClient.SqlDataAdapter("select Job_Card_No from Job_Card_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Job_Card_Code = '" & Trim(RecCode) & "'", con)
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
                    MessageBox.Show("Invalid Job No", "DOES NOT INSERT NEW JOB...", MessageBoxButtons.OK, MessageBoxIcon.Error)

                Else
                    new_record()
                    Insert_Entry = True
                    lbl_JobNo.Text = Trim(UCase(inpno))

                End If

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT INSERT NEW JOB...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Public Sub save_record() Implements Interface_MDIActions.save_record
        Dim cmd As New SqlClient.SqlCommand
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim tr As SqlClient.SqlTransaction
        Dim NewCode As String = ""
        Dim Selc_SetCode As String
        Dim led_id As Integer = 0
        Dim Bw_id As Integer = 0
        Dim Cnt_ID As Integer = 0
        Dim Mil_ID As Integer = 0
        Dim Cnt_Grid_ID As Integer = 0
        Dim Mil_Grid_ID As Integer = 0
        Dim Sno As Integer = 0
        Dim Partcls As String = ""
        Dim PBlNo As String = ""
        Dim vTotBags As Single, vTotCones As Single, vTotWeight As Single
        Dim vSetCd As String, vSetNo As String
        Dim Nr As Long
        Dim vSELC_LOTCODE As String


        If Val(lbl_Company.Tag) = 0 Then
            MessageBox.Show("Invalid Company Selection", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_JobNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        'If Common_Procedures.UserRight_Check(Common_Procedures.UserRightsCheckFor.SavingEntry, Common_Procedures.UR.entry_jobcard_entry, New_Entry, Me, con, "job_card_Head", "Job_Card_Code", NewCode, "Job_Card_Date", "(Job_Card_Code = '" & Trim(NewCode) & "')") = False Then Exit Sub

        'If Common_Procedures.UserRight_Check(Common_Procedures.UserRightsCheckFor.SavingEntry, Common_Procedures.UR.Yarn_Delivery_Entry, New_Entry) = False Then Exit Sub

        If pnl_Back.Enabled = False Then
            MessageBox.Show("Close Other Windows", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        If IsDate(dtp_Date.Text) = False Then
            MessageBox.Show("Invalid Date", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If dtp_Date.Enabled And dtp_Date.Visible Then dtp_Date.Focus()
            Exit Sub
        End If

        If Not (dtp_Date.Value.Date >= Common_Procedures.Company_FromDate And dtp_Date.Value.Date <= Common_Procedures.Company_ToDate) Then
            MessageBox.Show("Date is out of financial range", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If dtp_Date.Enabled And dtp_Date.Visible Then dtp_Date.Focus()
            Exit Sub
        End If

        led_id = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Ledger.Text)
        If led_id = 0 Then
            MessageBox.Show("Invalid Party Name", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If cbo_Ledger.Enabled And cbo_Ledger.Visible Then cbo_Ledger.Focus()
            Exit Sub
        End If

        Mil_ID = Common_Procedures.Mill_NameToIdNo(con, cbo_MillName.Text)
        If Mil_ID = 0 Then
            MessageBox.Show("Invalid Mill Name", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If cbo_MillName.Enabled And cbo_MillName.Visible Then cbo_MillName.Focus()
            Exit Sub
        End If

        Cnt_ID = Common_Procedures.Count_NameToIdNo(con, cbo_CountName.Text)
        If Cnt_ID = 0 Then
            MessageBox.Show("Invalid Mill Name", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If cbo_CountName.Enabled And cbo_CountName.Visible Then cbo_CountName.Focus()
            Exit Sub
        End If

        If Trim(txt_Ends.Text) = "" Then
            MessageBox.Show("Invalid Ends Name", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If txt_Ends.Enabled Then txt_Ends.Focus()
            Exit Sub
        End If

        If Val(txt_WarpMeters.Text) = 0 Then
            MessageBox.Show("Invalid Warp Meters", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If txt_WarpMeters.Enabled Then txt_WarpMeters.Focus()
            Exit Sub
        End If


        Bw_id = Common_Procedures.BeamWidth_NameToIdNo(con, cbo_BeamWidth.Text)

        For i = 0 To dgv_Details.RowCount - 1

            If Val(dgv_Details.Rows(i).Cells(7).Value) <> 0 Then
                Cnt_Grid_ID = Common_Procedures.Count_NameToIdNo(con, Trim(dgv_Details.Rows(i).Cells(1).Value))
                If Cnt_Grid_ID = 0 Then
                    MessageBox.Show("Invalid Count Name", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    If cbo_Grid_Countname.Enabled And cbo_Grid_Countname.Visible Then cbo_Grid_Countname.Focus()
                    Exit Sub
                End If

                If Trim(dgv_Details.Rows(i).Cells(2).Value) = "" Then
                    MessageBox.Show("Invalid Yarn Type", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    If cbo_YarnType.Enabled And cbo_YarnType.Visible Then cbo_YarnType.Focus()
                    Exit Sub
                End If

                If Trim(UCase(dgv_Details.Rows(i).Cells(2).Value)) = "BABY" And Trim(dgv_Details.Rows(i).Cells(3).Value) = "" Then
                    MessageBox.Show("Invalid Set No", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    If cbo_SetNo.Enabled And cbo_SetNo.Visible Then cbo_SetNo.Focus()
                    Exit Sub
                End If


                Mil_Grid_ID = Common_Procedures.Mill_NameToIdNo(con, Trim(dgv_Details.Rows(i).Cells(4).Value))
                If Mil_Grid_ID = 0 Then
                    MessageBox.Show("Invalid Mill Name", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    If cbo_GridMillName.Enabled And cbo_GridMillName.Visible Then cbo_GridMillName.Focus()
                    Exit Sub
                End If


            End If

        Next

        vTotBags = 0 : vTotCones = 0 : vTotWeight = 0
        If dgv_Details_Total.RowCount > 0 Then
            vTotBags = Val(dgv_Details_Total.Rows(0).Cells(5).Value())
            vTotCones = Val(dgv_Details_Total.Rows(0).Cells(6).Value())
            vTotWeight = Val(dgv_Details_Total.Rows(0).Cells(7).Value())
        End If

        Selc_SetCode = Trim(lbl_JobNo.Text) & "/" & Trim(Common_Procedures.FnYearCode) & "/" & Trim(Val(lbl_Company.Tag))

        tr = con.BeginTransaction

        Try

            If Insert_Entry = True Or New_Entry = False Then
                NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_JobNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            Else

                lbl_JobNo.Text = Common_Procedures.get_MaxCode(con, "Job_Card_Head", "Job_Card_Code", "For_OrderBy", "", Val(lbl_Company.Tag), Common_Procedures.FnYearCode, tr)

                NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_JobNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            End If

            cmd.Connection = con
            cmd.Transaction = tr

            cmd.Parameters.Clear()
            cmd.Parameters.AddWithValue("@JobDate", dtp_Date.Value.Date)


            If New_Entry = True Then

                cmd.CommandText = "Insert into Job_Card_Head ( User_IdNo ,  Job_Card_Code,               setcode_forSelection,     Company_IdNo,                      Job_Card_No,                                      for_OrderBy               ,                           Job_Card_Date  ,           ledger_idno,          count_idno       ,      mill_idno          ,     Beam_Width_Idno   ,        ends_name                  ,    pcs_length                ,        tape_length              , meters_yards_type, warp_meters         ,                 Remarks             ,              total_bags         ,     total_cones        ,        total_weight  ) " &
                                                      " Values (" & Str(Common_Procedures.User.IdNo) & ",'" & Trim(NewCode) & "', '" & Trim(Selc_SetCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_JobNo.Text) & "',  " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_JobNo.Text))) & ", @JobDate, " & Str(Val(led_id)) & ", " & Str(Val(Cnt_ID)) & ", " & Str(Val(Mil_ID)) & ", " & Str(Val(Bw_id)) & ", '" & Trim(txt_Ends.Text) & "', '" & Trim(txt_PcsLength.Text) & "', '" & Trim(txt_TapeLength.Text) & "', 'METER', '" & Trim(txt_WarpMeters.Text) & "',  '" & Trim(txt_Remarks.Text) & "',  " & Str(Val(vTotBags)) & ", " & Str(Val(vTotCones)) & ", " & Str(Val(vTotWeight)) & " )"
                cmd.ExecuteNonQuery()

            Else

                cmd.CommandText = "Update Job_Card_Head set User_IdNo = " & Str(Common_Procedures.User.IdNo) & "  , Job_Card_Date = @JobDate, ledger_idno = " & Str(Val(led_id)) & ", count_idno = " & Str(Val(Cnt_ID)) & ", mill_idno = " & Str(Val(Mil_ID)) & ", Beam_Width_Idno = " & Str(Val(Bw_id)) & ", ends_name = '" & Trim(txt_Ends.Text) & "', pcs_length = '" & Trim(txt_PcsLength.Text) & "', tape_length = '" & Trim(txt_TapeLength.Text) & "', meters_yards_type = 'METER', warp_meters = '" & Trim(txt_WarpMeters.Text) & "', remarks = '" & Trim(txt_Remarks.Text) & "', total_bags = " & Str(Val(vTotBags)) & ", total_cones = " & Str(Val(vTotCones)) & ", total_weight = " & Str(Val(vTotWeight)) & " Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Job_Card_Code = '" & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()

            End If

            If Val(Common_Procedures.settings.StatementPrint_BookNo_IN_Stock_Particulars_Status) = 1 Then
                Partcls = "JobCard : Job.No. " & Trim(lbl_JobNo.Text)
                ' PBlNo = Trim(txt_BookNo.Text)
            Else
                Partcls = "JobCard : Job.No. " & Trim(lbl_JobNo.Text)
                PBlNo = Trim(lbl_JobNo.Text)
            End If

            cmd.CommandText = "Delete from Job_Card_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Job_Card_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Stock_Yarn_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Stock_Empty_BeamBagCone_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()


            Sno = 0
            For i = 0 To dgv_Details.RowCount - 1

                If Val(dgv_Details.Rows(i).Cells(7).Value) <> 0 Then

                    Cnt_Grid_ID = Common_Procedures.Count_NameToIdNo(con, Trim(dgv_Details.Rows(i).Cells(1).Value), tr)
                    Mil_Grid_ID = Common_Procedures.Mill_NameToIdNo(con, Trim(dgv_Details.Rows(i).Cells(4).Value), tr)

                    Sno = Sno + 1

                    cmd.CommandText = "Insert into Job_Card_Details(Job_Card_Code, Company_IdNo, Job_Card_No, for_OrderBy, Job_Card_Date, Ledger_IdNo, Sl_No, Count_IdNo, Yarn_Type, SetCode_ForSelection, Mill_IdNo, Bags, Cones, Weight) Values ('" & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_JobNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_JobNo.Text))) & ", @JobDate, " & Str(Val(led_id)) & ", " & Str(Val(Sno)) & ", " & Str(Val(Cnt_Grid_ID)) & ", '" & Trim(dgv_Details.Rows(i).Cells(2).Value) & "', '" & Trim(dgv_Details.Rows(i).Cells(3).Value) & "', " & Str(Val(Mil_Grid_ID)) & ", " & Str(Val(dgv_Details.Rows(i).Cells(5).Value)) & ", " & Str(Val(dgv_Details.Rows(i).Cells(6).Value)) & ", " & Str(Val(dgv_Details.Rows(i).Cells(7).Value)) & " )"
                    cmd.ExecuteNonQuery()

                    'cmd.CommandText = "Insert into Stock_Yarn_Processing_Details(SoftwareType_IdNo, Reference_Code, Company_IdNo, Reference_No, for_OrderBy, Reference_Date, DeliveryTo_Idno, ReceivedFrom_Idno, Party_Bill_No, Sl_No, Count_IdNo, Yarn_Type, Mill_IdNo, Bags, Cones, Weight, Particulars, Posting_For, Set_Code, Set_No) Values (" & Str(Val(Common_Procedures.SoftwareTypes.Sizing_Software)) & " , '" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_JobNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_JobNo.Text))) & ", @JobDate, " & Str(Val(led_id)) & ", 0, '" & Trim(PBlNo) & "', " & Str(Val(Sno)) & ", " & Str(Val(Cnt_Grid_ID)) & ", '" & Trim(dgv_Details.Rows(i).Cells(2).Value) & "', " & Str(Val(Mil_Grid_ID)) & ", " & Str(Val(dgv_Details.Rows(i).Cells(5).Value)) & ", " & Str(Val(dgv_Details.Rows(i).Cells(6).Value)) & ", " & Str(Val(dgv_Details.Rows(i).Cells(7).Value)) & ", '" & Trim(Partcls) & "', 'JOB CARD', '" & Trim(vSetCd) & "', '" & Trim(vSetNo) & "')"
                    'cmd.ExecuteNonQuery()

                End If

            Next

            'If Val(vTotBags) <> 0 Or Val(vTotCones) <> 0 Then
            '    cmd.CommandText = "Insert into Stock_Empty_BeamBagCone_Processing_Details(SoftwareType_IdNo  , Reference_Code, Company_IdNo, Reference_No, for_OrderBy, Reference_Date, DeliveryTo_Idno, ReceivedFrom_Idno, Party_Bill_No, Sl_No, Empty_Bags, Empty_Cones, Particulars) Values (" & Str(Val(Common_Procedures.SoftwareTypes.Sizing_Software)) & " , '" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_JobNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_JobNo.Text))) & ", @JobDate, " & Str(Val(led_id)) & ", 0, '" & Trim(PBlNo) & "', 1, " & Str(Val(vTotBags)) & ", " & Str(Val(vTotCones)) & ", '" & Trim(Partcls) & "' )"
            '    cmd.ExecuteNonQuery()
            'End If


            'If Val(Common_Procedures.User.IdNo) = 1 Then
            '    If chk_Printed.Visible = True Then
            '        If chk_Printed.Enabled = True Then
            '            Update_PrintOut_Status(tr)
            '        End If
            '    End If
            'End If


            tr.Commit()

            Dt1.Dispose()
            Da.Dispose()

            'move_record(lbl_JobNo.Text)

            'If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1017" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1036" Then '---- Sri Bhagavan Sizing (Palladam)
            '    If New_Entry = True Then
            '        new_record()
            '    End If
            'Else
            '    move_record(lbl_JobNo.Text)
            'End If

            MessageBox.Show("Saved Sucessfully!!!", "FOR SAVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Information)

            If Val(Common_Procedures.settings.OnSave_MoveTo_NewEntry_Status) = 1 Then
                If New_Entry = True Then
                    new_record()
                Else
                    move_record(lbl_JobNo.Text)
                End If
            Else
                move_record(lbl_JobNo.Text)
            End If

        Catch ex As Exception

            tr.Rollback()

            If InStr(1, Trim(LCase(ex.Message)), "ck_stock_babycone_processing_details") > 0 Then
                MessageBox.Show("Invalid Baby cone Details - Delivery Qty greater than production Qty", "DOES NOT SAVE", MessageBoxButtons.OK, MessageBoxIcon.Error)

            Else
                MessageBox.Show(ex.Message, "FOR SAVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

            End If

        Finally
            If dtp_Date.Enabled And dtp_Date.Visible Then dtp_Date.Focus()

        End Try

    End Sub

    Private Sub txt_SlNo_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_SlNo.KeyDown
        If e.KeyCode = 40 Then SendKeys.Send("{TAB}")
        If e.KeyCode = 38 Then txt_Ends.Focus() ' SendKeys.Send("+{TAB}")
    End Sub

    Private Sub txt_SlNo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_SlNo.KeyPress
        Dim i As Integer

        If Asc(e.KeyChar) = 13 Then

            If Val(txt_SlNo.Text) = 0 Then
                txt_Remarks.Focus()

            Else

                With dgv_Details

                    For i = 0 To .Rows.Count - 1
                        If Val(dgv_Details.Rows(i).Cells(0).Value) = Val(txt_SlNo.Text) Then

                            cbo_Grid_Countname.Text = .Rows(i).Cells(1).Value
                            cbo_YarnType.Text = .Rows(i).Cells(2).Value
                            cbo_SetNo.Text = .Rows(i).Cells(3).Value
                            cbo_GridMillName.Text = .Rows(i).Cells(4).Value
                            txt_Bags.Text = Val(.Rows(i).Cells(5).Value)
                            txt_Cones.Text = Val(.Rows(i).Cells(6).Value)
                            txt_Weight.Text = Format(Val(.Rows(i).Cells(7).Value), "########0.000")

                            Exit For

                        End If

                    Next

                End With

                SendKeys.Send("{TAB}")

            End If

        End If
    End Sub

    Private Sub cbo_YarnType_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_YarnType.GotFocus
        With cbo_YarnType

            If Trim(cbo_YarnType.Text) = "" Then cbo_YarnType.Text = "MILL"

            '.BackColor = Color.LemonChiffon
            '.ForeColor = Color.Blue
            '.SelectionStart = 0
            '.SelectionLength = .Text.Length
        End With
        cbo_YarnType.Tag = cbo_YarnType.Text

    End Sub

    Private Sub cbo_YarnType_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_YarnType.KeyDown

        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_YarnType, cbo_Grid_Countname, Nothing, "YarnType_Head", "Yarn_Type", "", "")

        If (e.KeyValue = 40 And cbo_YarnType.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then

            If Trim(UCase(cbo_YarnType.Text)) = "BABY" Then
                cbo_SetNo.Focus()

            Else
                cbo_GridMillName.Focus()

            End If



        End If

    End Sub

    Private Sub cbo_YarnType_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_YarnType.KeyPress

        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_YarnType, Nothing, "YarnType_Head", "Yarn_Type", "", "")
        If Asc(e.KeyChar) = 13 Then
            If Trim(UCase(cbo_YarnType.Text)) = "BABY" Then
                cbo_SetNo.Focus()

            Else
                cbo_GridMillName.Focus()

            End If
        End If

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
        Dim Led_IdNo As Integer, Cnt_IdNo As Integer, Mil_IdNo As Integer
        Dim Condt As String = ""

        Try

            Condt = ""
            Led_IdNo = 0
            Cnt_IdNo = 0
            Mil_IdNo = 0

            If IsDate(dtp_Filter_Fromdate.Value) = True And IsDate(dtp_Filter_ToDate.Value) = True Then
                Condt = "a.Job_Card_Date between '" & Trim(Format(dtp_Filter_Fromdate.Value, "MM/dd/yyyy")) & "' and '" & Trim(Format(dtp_Filter_ToDate.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_Filter_Fromdate.Value) = True Then
                Condt = "a.Job_Card_Date = '" & Trim(Format(dtp_Filter_Fromdate.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_Filter_ToDate.Value) = True Then
                Condt = "a.Job_Card_Date = '" & Trim(Format(dtp_Filter_ToDate.Value, "MM/dd/yyyy")) & "' "
            End If

            If Trim(cbo_Filter_PartyName.Text) <> "" Then
                Led_IdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Filter_PartyName.Text)
            End If

            If Trim(cbo_Filter_CountName.Text) <> "" Then
                Cnt_IdNo = Common_Procedures.Count_NameToIdNo(con, cbo_Filter_CountName.Text)
            End If

            If Trim(cbo_Filter_MillName.Text) <> "" Then
                Mil_IdNo = Common_Procedures.Mill_NameToIdNo(con, cbo_Filter_MillName.Text)
            End If

            If Val(Led_IdNo) <> 0 Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & "a.Ledger_IdNo = " & Str(Val(Led_IdNo))
            End If

            If Val(Cnt_IdNo) <> 0 Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " a.Job_Card_Code IN (select z1.Job_Card_Code from Job_Card_Details z1 where z1.Count_IdNo = " & Str(Val(Cnt_IdNo)) & ") "
            End If

            If Val(Mil_IdNo) <> 0 Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " a.Job_Card_Code IN (select z2.Job_Card_Code from Job_Card_Details z2 where z2.Mill_IdNo = " & Str(Val(Mil_IdNo)) & ") "
            End If

            da = New SqlClient.SqlDataAdapter("select a.*, b.Ledger_Name from Job_Card_Head a INNER JOIN Ledger_Head b on a.Ledger_IdNo = b.Ledger_IdNo where a.company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Job_Card_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "' " & IIf(Trim(Condt) <> "", " and ", "") & Condt & " Order by a.for_orderby, a.Job_Card_No", con)
            da.Fill(dt2)

            dgv_Filter_Details.Rows.Clear()

            If dt2.Rows.Count > 0 Then

                For i = 0 To dt2.Rows.Count - 1

                    n = dgv_Filter_Details.Rows.Add()

                    dgv_Filter_Details.Rows(n).Cells(0).Value = i + 1
                    dgv_Filter_Details.Rows(n).Cells(1).Value = dt2.Rows(i).Item("Job_Card_No").ToString
                    dgv_Filter_Details.Rows(n).Cells(2).Value = Format(Convert.ToDateTime(dt2.Rows(i).Item("Job_Card_Date").ToString), "dd-MM-yyyy")
                    dgv_Filter_Details.Rows(n).Cells(3).Value = dt2.Rows(i).Item("Ledger_Name").ToString
                    dgv_Filter_Details.Rows(n).Cells(4).Value = Val(dt2.Rows(i).Item("Total_Bags").ToString)
                    dgv_Filter_Details.Rows(n).Cells(5).Value = Val(dt2.Rows(i).Item("Total_Cones").ToString)
                    dgv_Filter_Details.Rows(n).Cells(6).Value = Format(Val(dt2.Rows(i).Item("Total_Weight").ToString), "########0.000")

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

    End Sub

    Private Sub dgv_Filter_Details_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_Filter_Details.KeyDown
        If e.KeyCode = 13 Then
            Open_FilterEntry()
        End If
    End Sub




    Private Sub cbo_BeamWidth_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_BeamWidth.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Dim f As New Beam_Width_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_BeamWidth.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()
        End If
    End Sub



    Private Sub cbo_Grid_Count_Name_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Grid_Countname.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Dim f As New Count_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Grid_Countname.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If
    End Sub




    Private Sub dgv_Details_CellValueChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellValueChanged
        On Error Resume Next

        If IsNothing(dgv_Details.CurrentCell) Then Exit Sub
        With dgv_Details
            If .Visible Then
                If .CurrentCell.ColumnIndex = 5 Or .CurrentCell.ColumnIndex = 6 Or .CurrentCell.ColumnIndex = 7 Then
                    Total_Calculation()
                End If
            End If
        End With
    End Sub

    Private Sub dgv_Details_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgv_Details.DoubleClick

        If Trim(dgv_Details.CurrentRow.Cells(1).Value) <> "" Then

            txt_SlNo.Text = Val(dgv_Details.CurrentRow.Cells(0).Value)
            cbo_Grid_Countname.Text = Trim(dgv_Details.CurrentRow.Cells(1).Value)
            cbo_YarnType.Text = Trim(dgv_Details.CurrentRow.Cells(2).Value)
            cbo_SetNo.Text = dgv_Details.CurrentRow.Cells(3).Value
            cbo_GridMillName.Text = dgv_Details.CurrentRow.Cells(4).Value
            txt_Bags.Text = Val(dgv_Details.CurrentRow.Cells(5).Value)
            txt_Cones.Text = Val(dgv_Details.CurrentRow.Cells(6).Value)
            txt_Weight.Text = Format(Val(dgv_Details.CurrentRow.Cells(7).Value), "########0.000")

            If txt_SlNo.Enabled And txt_SlNo.Visible Then txt_SlNo.Focus()

        End If

    End Sub

    Private Sub dgv_Details_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_Details.KeyDown
        vcbo_KeyDwnVal = e.KeyValue

    End Sub

    Private Sub dgv_Details_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_Details.KeyUp
        Dim n As Integer

        If e.Control = True And UCase(Chr(e.KeyCode)) = "D" Then

            With dgv_Details

                n = .CurrentRow.Index
                .Rows.RemoveAt(n)

                For i = 0 To .Rows.Count - 1
                    .Rows(i).Cells(0).Value = i + 1
                Next

            End With

            Total_Calculation()

            txt_SlNo.Text = dgv_Details.Rows.Count + 1
            cbo_CountName.Text = ""
            cbo_YarnType.Text = "MILL"
            cbo_SetNo.Text = ""
            cbo_MillName.Text = ""
            txt_Bags.Text = ""
            txt_Cones.Text = ""
            txt_Weight.Text = ""

            If cbo_Grid_Countname.Enabled And cbo_Grid_Countname.Visible Then cbo_Grid_Countname.Focus()

        End If

    End Sub

    Private Sub cbo_Ledger_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Ledger.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Common_Procedures.MDI_LedType = ""
            Dim f As New Ledger_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Ledger.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()
        End If
    End Sub

    Private Sub cbo_MillName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_MillName.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Mill_Head", "Mill_Name", "", "(Mill_IdNo = 0)")
    End Sub

    Private Sub cbo_Grid_MillName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_GridMillName.KeyDown

        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_GridMillName, Nothing, txt_Bags, "Mill_Head", "Mill_Name", "", "(Mill_IdNo = 0)")

        If (e.KeyValue = 38 And cbo_GridMillName.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then

            If Trim(UCase(cbo_YarnType.Text)) = "BABY" Then
                cbo_SetNo.Focus()

            Else
                cbo_YarnType.Focus()

            End If

        End If

    End Sub

    Private Sub cbo_GridMillName_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_GridMillName.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Dim f As New Mill_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_GridMillName.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()
        End If
    End Sub

    Private Sub cbo_MillName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_MillName.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_MillName, dtp_Date, cbo_CountName, "Mill_Head", "Mill_Name", "", "(Mill_IdNo = 0)")

    End Sub

    Private Sub cbo_MillName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_MillName.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_MillName, cbo_CountName, "Mill_Head", "Mill_Name", "", "(Mill_IdNo = 0)")
    End Sub

    Private Sub cbo_MillName_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_MillName.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Dim f As New Mill_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_MillName.Name
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
        Close_Form()
    End Sub

    Private Sub btn_Add_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Add.Click
        Dim n As Integer
        Dim MtchSTS As Boolean

        If Trim(cbo_Grid_Countname.Text) = "" Then
            MessageBox.Show("Invalid Count Name", "DOES NOT ADD...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If cbo_Grid_Countname.Enabled And cbo_Grid_Countname.Visible Then cbo_Grid_Countname.Focus()
            Exit Sub
        End If

        If Trim(cbo_YarnType.Text) = "" Then
            MessageBox.Show("Invalid Yarn Type", "DOES NOT ADD...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If cbo_YarnType.Enabled And cbo_YarnType.Visible Then cbo_YarnType.Focus()
            Exit Sub
        End If

        If Trim(UCase(cbo_YarnType.Text)) = "BABY" And Trim(cbo_SetNo.Text) = "" Then
            MessageBox.Show("Invalid Set No", "DOES NOT ADD...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If cbo_SetNo.Enabled And cbo_SetNo.Visible Then cbo_SetNo.Focus()
            Exit Sub
        End If

        If Trim(cbo_GridMillName.Text) = "" Then
            MessageBox.Show("Invalid MIll Name", "DOES NOT ADD...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If cbo_GridMillName.Enabled And cbo_GridMillName.Visible Then cbo_GridMillName.Focus()
            Exit Sub
        End If

        If Val(txt_Weight.Text) = 0 Then
            MessageBox.Show("Invalid Weight", "DOES NOT ADD...", MessageBoxButtons.OK)
            If txt_Weight.Enabled And txt_Weight.Visible Then txt_Weight.Focus()
            Exit Sub
        End If

        MtchSTS = False

        With dgv_Details

            For i = 0 To .Rows.Count - 1
                If Val(dgv_Details.Rows(i).Cells(0).Value) = Val(txt_SlNo.Text) Then

                    .Rows(i).Cells(1).Value = cbo_Grid_Countname.Text
                    .Rows(i).Cells(2).Value = cbo_YarnType.Text
                    .Rows(i).Cells(3).Value = cbo_SetNo.Text
                    .Rows(i).Cells(4).Value = cbo_GridMillName.Text
                    .Rows(i).Cells(5).Value = Val(txt_Bags.Text)
                    .Rows(i).Cells(6).Value = Val(txt_Cones.Text)
                    .Rows(i).Cells(7).Value = Format(Val(txt_Weight.Text), "########0.000")

                    .Rows(i).Selected = True

                    MtchSTS = True

                    If i >= 8 Then .FirstDisplayedScrollingRowIndex = i - 7

                    Exit For

                End If

            Next

            If MtchSTS = False Then

                n = .Rows.Add()
                .Rows(n).Cells(0).Value = txt_SlNo.Text
                .Rows(n).Cells(1).Value = cbo_Grid_Countname.Text
                .Rows(n).Cells(2).Value = cbo_YarnType.Text
                .Rows(n).Cells(3).Value = cbo_SetNo.Text
                .Rows(n).Cells(4).Value = cbo_GridMillName.Text
                .Rows(n).Cells(5).Value = Val(txt_Bags.Text)
                .Rows(n).Cells(6).Value = Val(txt_Cones.Text)
                .Rows(n).Cells(7).Value = Format(Val(txt_Weight.Text), "########0.000")

                .Rows(n).Selected = True

                If n >= 8 Then .FirstDisplayedScrollingRowIndex = n - 7

            End If

        End With

        Total_Calculation()

        txt_SlNo.Text = dgv_Details.Rows.Count + 1
        cbo_Grid_Countname.Text = ""
        cbo_YarnType.Text = "MILL"
        cbo_SetNo.Text = ""
        cbo_GridMillName.Text = ""
        txt_Bags.Text = ""
        txt_Cones.Text = ""
        txt_Weight.Text = ""

        If cbo_Grid_Countname.Enabled And cbo_Grid_Countname.Visible Then cbo_Grid_Countname.Focus()

    End Sub

    Private Sub btn_Delete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Delete.Click
        Dim n As Integer
        Dim MtchSTS As Boolean

        MtchSTS = False

        With dgv_Details

            For i = 0 To .Rows.Count - 1
                If Val(dgv_Details.Rows(i).Cells(0).Value) = Val(txt_SlNo.Text) Then

                    .Rows.RemoveAt(i)

                    MtchSTS = True

                    Exit For

                End If

            Next

            If MtchSTS = True Then
                For i = 0 To .Rows.Count - 1
                    .Rows(n).Cells(0).Value = i + 1
                Next
            End If

        End With

        Total_Calculation()

        txt_SlNo.Text = dgv_Details.Rows.Count + 1
        cbo_Grid_Countname.Text = ""
        cbo_YarnType.Text = "MILL"
        cbo_SetNo.Text = ""
        cbo_GridMillName.Text = ""
        txt_Bags.Text = ""
        txt_Cones.Text = ""
        txt_Weight.Text = ""

        If cbo_Grid_Countname.Enabled And cbo_Grid_Countname.Visible Then cbo_Grid_Countname.Focus()

    End Sub

    Private Sub txt_Bags_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txt_Bags.TextChanged
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim CntID As Integer
        Dim MilID As Integer
        Dim Cns_Bg As Single, Wt_Cn As Single

        CntID = Common_Procedures.Count_NameToIdNo(con, cbo_Grid_Countname.Text)
        MilID = Common_Procedures.Mill_NameToIdNo(con, cbo_GridMillName.Text)

        If CntID <> 0 And MilID <> 0 And Trim(UCase(cbo_YarnType.Text)) = "MILL" Then

            Cns_Bg = 0 : Wt_Cn = 0
            Da = New SqlClient.SqlDataAdapter("select * from Mill_Count_Details where mill_idno = " & Str(Val(MilID)) & " and count_idno = " & Str(Val(CntID)), con)
            Da.Fill(Dt)

            If Dt.Rows.Count > 0 Then
                Cns_Bg = Val(Dt.Rows(0).Item("Cones_Bag").ToString)
                Wt_Cn = Val(Dt.Rows(0).Item("Weight_Cone").ToString)
            End If

            Dt.Clear()
            Dt.Dispose()
            Da.Dispose()

            If Val(Cns_Bg) <> 0 Then
                txt_Cones.Text = Val(txt_Bags.Text) * Val(Cns_Bg)
            End If
            If Val(Wt_Cn) <> 0 Then
                txt_Weight.Text = Format(Val(txt_Cones.Text) * Val(Wt_Cn), "#########0.000")
            End If

        End If
    End Sub

    Private Sub txt_Cones_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txt_Cones.TextChanged
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim CntID As Integer
        Dim MilID As Integer
        Dim Cns_Bg As Single, Wt_Cn As Single

        CntID = Common_Procedures.Count_NameToIdNo(con, cbo_Grid_Countname.Text)
        MilID = Common_Procedures.Mill_NameToIdNo(con, cbo_GridMillName.Text)

        If CntID <> 0 And MilID <> 0 And Trim(UCase(cbo_YarnType.Text)) = "MILL" Then

            Cns_Bg = 0 : Wt_Cn = 0
            Da = New SqlClient.SqlDataAdapter("select * from Mill_Count_Details where mill_idno = " & Str(Val(MilID)) & " and count_idno = " & Str(Val(CntID)), con)
            Da.Fill(Dt)

            If Dt.Rows.Count > 0 Then
                Cns_Bg = Val(Dt.Rows(0).Item("Cones_Bag").ToString)
                Wt_Cn = Val(Dt.Rows(0).Item("Weight_Cone").ToString)
            End If

            Dt.Clear()
            Dt.Dispose()
            Da.Dispose()

            If Val(Wt_Cn) <> 0 Then
                txt_Weight.Text = Format(Val(txt_Cones.Text) * Val(Wt_Cn), "#########0.000")
            End If

        End If
    End Sub

    Private Sub cbo_CountName_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_CountName.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Dim f As New Count_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_CountName.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If

    End Sub

    Private Sub cbo_Grid_CountName_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Grid_Countname.LostFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Count_Head", "Count_Name", "", "(Count_IdNo = 0)")
    End Sub

    Private Sub cbo_CountName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_CountName.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_CountName, cbo_MillName, cbo_BeamWidth, "Count_Head", "Count_Name", "", "(Count_IdNo = 0)")
    End Sub

    Private Sub cbo_CountName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_CountName.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_CountName, cbo_BeamWidth, "Count_Head", "Count_Name", "", "(Count_IdNo = 0)")
    End Sub

    Private Sub cbo_SetNo_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_SetNo.GotFocus
        Dim NewCode As String
        Dim Led_ID As Integer, Cnt_ID As Integer
        Dim Condt As String
        Dim Cmp_Cond As String = ""

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_JobNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Led_ID = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Ledger.Text)

        Cnt_ID = Common_Procedures.Count_NameToIdNo(con, cbo_Grid_Countname.Text)

        Cmp_Cond = ""
        If Val(Common_Procedures.settings.StatementPrint_InStock_Combine_AllCompany) = 0 Then
            Cmp_Cond = "a.Company_IdNo = " & Str(Val(lbl_Company.Tag))
        End If

        Condt = "( " & Cmp_Cond & IIf(Trim(Cmp_Cond) <> "", " and ", "") & " a.Ledger_IdNo = " & Str(Val(Led_ID)) & " and a.Count_IdNo = " & Str(Val(Cnt_ID)) & " and (  (a.Baby_Weight - a.Delivered_Weight) > 0 or (a.setcode_forSelection IN (select z.setcode_forSelection from Job_Card_Details z where z.company_idno = " & Str(Val(lbl_Company.Tag)) & " and z.Job_Card_Code = '" & Trim(NewCode) & "') ) ) )"
        'Condt = "( a.company_idno = " & Str(Val(lbl_Company.Tag)) & " and a.Ledger_IdNo = " & Str(Val(Led_ID)) & " and a.Count_IdNo = " & Str(Val(Cnt_ID)) & " and (  (a.Baby_Weight - a.Delivered_Weight) > 0 or (a.setcode_forSelection IN (select z.setcode_forSelection from Job_Card_Details z where z.company_idno = " & Str(Val(lbl_Company.Tag)) & " and z.Job_Card_Code = '" & Trim(NewCode) & "') ) ) )"

        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Stock_BabyCone_Processing_Details a", "setcode_forSelection", Condt, "(Reference_Code = '')")

        cbo_SetNo.Tag = cbo_SetNo.Text

    End Sub

    Private Sub cbo_SetNo_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_SetNo.LostFocus
        If Trim(UCase(cbo_SetNo.Text)) <> Trim(UCase(cbo_SetNo.Tag)) Then
            get_BabyCone_Details()
        End If
    End Sub

    Private Sub cbo_setno_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_SetNo.KeyDown
        Dim NewCode As String
        Dim Led_ID As Integer, Cnt_ID As Integer
        Dim Condt As String
        Dim Cmp_Cond As String


        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_JobNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Led_ID = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Ledger.Text)

        Cnt_ID = Common_Procedures.Count_NameToIdNo(con, cbo_Grid_Countname.Text)

        Cmp_Cond = ""
        If Val(Common_Procedures.settings.StatementPrint_InStock_Combine_AllCompany) = 0 Then
            Cmp_Cond = "a.Company_IdNo = " & Str(Val(lbl_Company.Tag))
        End If

        Condt = "( " & Cmp_Cond & IIf(Trim(Cmp_Cond) <> "", " and ", "") & " a.Ledger_IdNo = " & Str(Val(Led_ID)) & " and a.Count_IdNo = " & Str(Val(Cnt_ID)) & " and (  (a.Baby_Weight - a.Delivered_Weight) > 0 or (a.setcode_forSelection IN (select z.setcode_forSelection from Job_Card_Details z where z.company_idno = " & Str(Val(lbl_Company.Tag)) & " and z.Job_Card_Code = '" & Trim(NewCode) & "') ) ) )"
        'Condt = "( a.company_idno = " & Str(Val(lbl_Company.Tag)) & " and a.Ledger_IdNo = " & Str(Val(Led_ID)) & " and a.Count_IdNo = " & Str(Val(Cnt_ID)) & " and (  (a.Baby_Weight - a.Delivered_Weight) > 0 or (a.setcode_forSelection IN (select z.setcode_forSelection from Job_Card_Details z where z.company_idno = " & Str(Val(lbl_Company.Tag)) & " and z.Job_Card_Code = '" & Trim(NewCode) & "') ) ) )"

        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_SetNo, cbo_YarnType, cbo_GridMillName, "Stock_BabyCone_Processing_Details", "setcode_forSelection", Condt, "(Reference_Code = '')")

    End Sub

    Private Sub cbo_setno_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_SetNo.KeyPress
        Dim NewCode As String
        Dim Led_ID As Integer, Cnt_ID As Integer
        Dim Condt As String
        Dim Cmp_Cond As String = ""

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_JobNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Led_ID = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Ledger.Text)

        Cnt_ID = Common_Procedures.Count_NameToIdNo(con, cbo_Grid_Countname.Text)

        Cmp_Cond = ""
        If Val(Common_Procedures.settings.StatementPrint_InStock_Combine_AllCompany) = 0 Then
            Cmp_Cond = "a.Company_IdNo = " & Str(Val(lbl_Company.Tag))
        End If

        Condt = "( " & Cmp_Cond & IIf(Trim(Cmp_Cond) <> "", " and ", "") & " a.Ledger_IdNo = " & Str(Val(Led_ID)) & " and a.Count_IdNo = " & Str(Val(Cnt_ID)) & " and (  (a.Baby_Weight - a.Delivered_Weight) > 0 or (a.setcode_forSelection IN (select z.setcode_forSelection from Job_Card_Details z where z.company_idno = " & Str(Val(lbl_Company.Tag)) & " and z.Job_Card_Code = '" & Trim(NewCode) & "') ) ) )"
        'Condt = "( a.company_idno = " & Str(Val(lbl_Company.Tag)) & " and a.Ledger_IdNo = " & Str(Val(Led_ID)) & " and a.Count_IdNo = " & Str(Val(Cnt_ID)) & " and (  (a.Baby_Weight - a.Delivered_Weight) > 0 or (a.setcode_forSelection IN (select z.setcode_forSelection from Job_Card_Details z where z.company_idno = " & Str(Val(lbl_Company.Tag)) & " and z.Job_Card_Code = '" & Trim(NewCode) & "') ) ) )"

        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_SetNo, cbo_GridMillName, "Stock_BabyCone_Processing_Details a", "setcode_forSelection", Condt, "(Reference_Code = '')")

    End Sub

    Private Sub cbo_Ledger_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Ledger.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Ledger, Nothing, dtp_Date, "Ledger_AlaisHead", "Ledger_DisplayName", "(AccountsGroup_IdNo = 10 and Close_Status = 0 )", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_Ledger_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Ledger.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Ledger, dtp_Date, "Ledger_AlaisHead", "Ledger_DisplayName", "(AccountsGroup_IdNo = 10 and Close_Status = 0 )", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_Filter_PartyName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Filter_PartyName.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_PartyName, dtp_Filter_ToDate, cbo_Filter_CountName, "Ledger_AlaisHead", "Ledger_DisplayName", "(AccountsGroup_IdNo = 10 )", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_Filter_PartyName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Filter_PartyName.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Filter_PartyName, cbo_Filter_CountName, "Ledger_AlaisHead", "Ledger_DisplayName", "(AccountsGroup_IdNo = 10 )", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_Grid_CountName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Grid_Countname.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Grid_Countname, txt_SlNo, Nothing, "Count_Head", "Count_Name", "", "(Count_IdNo = 0)")
        If (e.KeyValue = 40 And cbo_Grid_Countname.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
            If Trim(cbo_Grid_Countname.Text) <> "" Then
                cbo_YarnType.Focus()
            Else
                txt_Remarks.Focus()
            End If
        End If
    End Sub

    Private Sub cbo_Filter_CountName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Filter_CountName.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_CountName, cbo_Filter_PartyName, cbo_Filter_MillName, "Count_Head", "Count_Name", "", "(Count_IdNo = 0)")
    End Sub

    Private Sub cbo_Filter_CountName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Filter_CountName.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Filter_CountName, cbo_Filter_MillName, "Count_Head", "Count_Name", "", "(Count_IdNo = 0)")
    End Sub
    Private Sub cbo_Grid_CountName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Grid_Countname.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Grid_Countname, Nothing, "Count_Head", "Count_Name", "", "(Count_IdNo = 0)")
        If Asc(e.KeyChar) = 13 Then
            If Trim(cbo_Grid_Countname.Text) <> "" Then
                cbo_YarnType.Focus()
            Else
                txt_Remarks.Focus()
            End If
        End If
    End Sub
    Private Sub cbo_Beamwidth_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_BeamWidth.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_BeamWidth, cbo_CountName, txt_WarpMeters, "Beam_Width_Head", "Beam_Width_Name", "", "(Beam_Width_IdNo = 0)")
    End Sub

    Private Sub cbo_BeamWidth_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_BeamWidth.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_BeamWidth, txt_WarpMeters, "Beam_Width_Head", "Beam_Width_Name", "", "(Beam_Width_IdNo = 0)")
    End Sub


    Private Sub cbo_Grid_MillName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_GridMillName.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_GridMillName, txt_Bags, "Mill_Head", "Mill_Name", "", "(Mill_IdNo = 0)")

    End Sub

    Private Sub cbo_Filter_MillName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Filter_MillName.KeyDown
        vcbo_KeyDwnVal = e.KeyValue

        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_MillName, cbo_Filter_CountName, btn_Filter_Show, "Mill_Head", "Mill_Name", "", "(Mill_IdNo = 0)")

    End Sub

    Private Sub cbo_Filter_MillName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Filter_MillName.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Filter_MillName, btn_Filter_Show, "Mill_Head", "Mill_Name", "", "(Mill_IdNo = 0)")
        If Asc(e.KeyChar) = 13 Then
            btn_Filter_Show_Click(sender, e)
        End If
    End Sub

    Private Sub txt_WarpMeters_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_WarpMeters.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub txt_PcsLength_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_PcsLength.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub



    Private Sub txt_Weight_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Weight.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
        If Asc(e.KeyChar) = 13 Then
            btn_Add_Click(sender, e)
            'SendKeys.Send("{TAB}")
        End If
    End Sub



    Private Sub get_BabyCone_Details()
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim CntID As Integer
        Dim NewCode As String
        Dim Ent_Bgs As Integer, Ent_Cns As Integer
        Dim Ent_Wgt As Single


        CntID = Common_Procedures.Count_NameToIdNo(con, cbo_Grid_Countname.Text)

        If CntID <> 0 And Trim(cbo_SetNo.Text) <> "" And Trim(UCase(cbo_YarnType.Text)) = "BABY" Then

            NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_JobNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            Da = New SqlClient.SqlDataAdapter("select a.*, b.mill_name, c.bags as DelvEnt_Bags, c.cones as DelvEnt_cones, c.Weight as DelvEnt_Weight from Stock_BabyCone_Processing_Details a INNER JOIN mill_head b ON  a.mill_idno = b.mill_idno LEFT OUTER JOIN Job_Card_Details c ON c.Job_Card_Code = '" & Trim(NewCode) & "' and c.yarn_type = 'BABY' and a.SetCode_ForSelection = c.SetCode_ForSelection where a.setcode_forSelection = '" & Trim(cbo_SetNo.Text) & "' and a.count_idno = " & Str(Val(CntID)), con)
            Da.Fill(Dt)

            If Dt.Rows.Count > 0 Then

                Ent_Bgs = 0 : Ent_Cns = 0 : Ent_Wgt = 0

                If IsDBNull(Dt.Rows(0).Item("DelvEnt_Bags").ToString) = False Then Ent_Bgs = Val(Dt.Rows(0).Item("DelvEnt_Bags").ToString)
                If IsDBNull(Dt.Rows(0).Item("DelvEnt_cones").ToString) = False Then Ent_Cns = Val(Dt.Rows(0).Item("DelvEnt_cones").ToString)
                If IsDBNull(Dt.Rows(0).Item("DelvEnt_Weight").ToString) = False Then Ent_Wgt = Val(Dt.Rows(0).Item("DelvEnt_Weight").ToString)

                cbo_GridMillName.Text = Dt.Rows(0).Item("Mill_Name").ToString
                txt_Bags.Text = (Val(Dt.Rows(0).Item("Baby_Bags").ToString) - Val(Dt.Rows(0).Item("Delivered_Bags").ToString) + Ent_Bgs)
                txt_Cones.Text = (Val(Dt.Rows(0).Item("Baby_Cones").ToString) - Val(Dt.Rows(0).Item("Delivered_Cones").ToString) + Ent_Cns)
                txt_Weight.Text = (Val(Dt.Rows(0).Item("Baby_Weight").ToString) - Val(Dt.Rows(0).Item("Delivered_Weight").ToString) + Ent_Wgt)

            End If

            Dt.Clear()
            Dt.Dispose()
            Da.Dispose()

        End If

    End Sub

    Private Sub Total_Calculation()
        Dim Sno As Integer
        Dim TotBags As Single, TotCones As Single, TotWeight As Single

        Sno = 0
        TotBags = 0
        TotCones = 0
        TotWeight = 0
        With dgv_Details
            For i = 0 To .RowCount - 1
                Sno = Sno + 1
                .Rows(i).Cells(0).Value = Sno
                If Val(.Rows(i).Cells(7).Value) <> 0 Then
                    TotBags = TotBags + Val(.Rows(i).Cells(5).Value)
                    TotCones = TotCones + Val(.Rows(i).Cells(6).Value)
                    TotWeight = TotWeight + Val(.Rows(i).Cells(7).Value)
                End If
            Next
        End With

        With dgv_Details_Total
            If .RowCount = 0 Then .Rows.Add()
            .Rows(0).Cells(5).Value = Val(TotBags)
            .Rows(0).Cells(6).Value = Val(TotCones)
            .Rows(0).Cells(7).Value = Format(Val(TotWeight), "########0.000")
        End With

    End Sub
    Private Sub txt_EmptyBeam_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs)
        If e.KeyCode = 40 Then SendKeys.Send("{TAB}")
        If e.KeyCode = 38 Then cbo_Grid_Countname.Focus() ' SendKeys.Send("+{TAB}")
    End Sub
    Private Sub txt_Remarks_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Remarks.KeyPress
        If Asc(e.KeyChar) = 13 Then
            If MessageBox.Show("Do you want to save ?", "FOR SAVING...", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = DialogResult.Yes Then
                save_record()
            End If
        End If
    End Sub
    Public Sub print_record() Implements Interface_MDIActions.print_record
        'Dim da1 As New SqlClient.SqlDataAdapter
        'Dim dt1 As New DataTable
        'Dim NewCode As String

        'NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_JobNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        'Try

        '    da1 = New SqlClient.SqlDataAdapter("select a.*, b.* from Job_Card_Head a LEFT OUTER JOIN Ledger_Head b ON a.Ledger_IdNo = b.Ledger_IdNo where a.Job_Card_Code = '" & Trim(NewCode) & "'", con)
        '    da1.Fill(dt1)

        '    If dt1.Rows.Count <= 0 Then

        '        MessageBox.Show("This is New Entry", "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)
        '        Exit Sub

        '    End If

        '    dt1.Dispose()
        '    da1.Dispose()

        'Catch ex As Exception
        '    MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        'End Try

        'Dim pkCustomSize1 As New System.Drawing.Printing.PaperSize("PAPER 8X6", 800, 600)
        'PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = pkCustomSize1
        'PrintDocument1.DefaultPageSettings.PaperSize = pkCustomSize1

        'If Val(Common_Procedures.Print_OR_Preview_Status) = 1 Then
        '    Try
        '        PrintDocument1.Print()

        '        'PrintDialog1.PrinterSettings = PrintDocument1.PrinterSettings
        '        'If PrintDialog1.ShowDialog() = Windows.Forms.DialogResult.OK Then
        '        '    PrintDocument1.PrinterSettings = PrintDialog1.PrinterSettings
        '        '    PrintDocument1.Print()
        '        'End If

        '    Catch ex As Exception
        '        MessageBox.Show("The printing operation failed" & vbCrLf & ex.Message, "DOES NOT SHOW PRINT PREVIEW...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        '    End Try


        'Else

        '    Try

        '        Dim ppd As New PrintPreviewDialog

        '        ppd.Document = PrintDocument1

        '        ppd.WindowState = FormWindowState.Normal
        '        ppd.StartPosition = FormStartPosition.CenterScreen
        '        ppd.ClientSize = New Size(600, 600)

        '        ppd.ShowDialog()
        '        'If PageSetupDialog1.ShowDialog() = Windows.Forms.DialogResult.OK Then
        '        '    PrintDocument1.DefaultPageSettings = PageSetupDialog1.PageSettings
        '        '    ppd.ShowDialog()
        '        'End If

        '    Catch ex As Exception
        '        MsgBox("The printing operation failed" & vbCrLf & ex.Message, MsgBoxStyle.Critical, "DOES NOT SHOW PRINT PREVIEW...")

        '    End Try

        'End If

    End Sub

    'Private Sub PrintDocument1_BeginPrint(ByVal sender As Object, ByVal e As System.Drawing.Printing.PrintEventArgs) Handles PrintDocument1.BeginPrint
    '    Dim da1 As New SqlClient.SqlDataAdapter
    '    Dim da2 As New SqlClient.SqlDataAdapter
    '    Dim NewCode As String

    '    NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_JobNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

    '    prn_HdDt.Clear()
    '    prn_DetDt.Clear()
    '    prn_DetIndx = 0
    '    prn_DetSNo = 0
    '    prn_PageNo = 0

    '    Try

    '        da1 = New SqlClient.SqlDataAdapter("select a.*, b.*, c.*  from Job_Card_Head a INNER JOIN Company_Head b ON a.Company_IdNo = b.Company_IdNo INNER JOIN Ledger_Head c ON a.Ledger_IdNo = c.Ledger_IdNo where a.company_idno = " & Str(Val(lbl_Company.Tag)) & " and a.Job_Card_Code = '" & Trim(NewCode) & "'", con)
    '        da1.Fill(prn_HdDt)

    '        If prn_HdDt.Rows.Count > 0 Then

    '            da2 = New SqlClient.SqlDataAdapter("select a.*, b.Mill_Name, c.Count_Name, d.Job_Card_No from Job_Card_Details a INNER JOIN Mill_Head b on a.Mill_IdNo = b.Mill_IdNo INNER JOIN Count_Head c on a.Count_IdNo = c.Count_IdNo LEFT OUTER JOIN Job_Card_Head d ON a.SetCode_ForSelection <> '' and a.SetCode_ForSelection = d.setcode_forSelection where a.company_idno = " & Str(Val(lbl_Company.Tag)) & " and a.Job_Card_Code = '" & Trim(NewCode) & "' Order by a.sl_no", con)
    '            da2.Fill(prn_DetDt)

    '            da2.Dispose()

    '        Else
    '            MessageBox.Show("This is New Entry", "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

    '        End If

    '        da1.Dispose()

    '    Catch ex As Exception
    '        MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

    '    End Try

    'End Sub

    'Private Sub PrintDocument1_PrintPage(ByVal sender As System.Object, ByVal e As System.Drawing.Printing.PrintPageEventArgs) Handles PrintDocument1.PrintPage
    '    If prn_HdDt.Rows.Count <= 0 Then Exit Sub
    '    'If Trim(UCase(Common_Procedures.settings.CompanyName)) = "" Then
    '    Printing_Format1(e)
    '    'End If
    'End Sub

    'Private Sub Printing_Format1(ByRef e As System.Drawing.Printing.PrintPageEventArgs)
    '    Dim EntryCode As String
    '    Dim I As Integer, NoofDets As Integer, NoofItems_PerPage As Integer
    '    Dim pFont As Font
    '    Dim LMargin As Single, RMargin As Single, TMargin As Single, BMargin As Single
    '    Dim PrintWidth As Single, PrintHeight As Single
    '    Dim PageWidth As Single, PageHeight As Single
    '    Dim CurY As Single, TxtHgt As Single
    '    Dim LnAr(15) As Single, ClArr(15) As Single
    '    Dim ItmNm1 As String, ItmNm2 As String
    '    'Dim ps As Printing.PaperSize
    '    Dim strHeight As Single = 0
    '    Dim PpSzSTS As Boolean = False

    '    Dim pkCustomSize1 As New System.Drawing.Printing.PaperSize("PAPER 8X6", 800, 600)
    '    PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = pkCustomSize1
    '    PrintDocument1.DefaultPageSettings.PaperSize = pkCustomSize1

    '    ''PrintDocument pd = new PrintDocument();
    '    ''pd.DefaultPageSettings.PaperSize = new System.Drawing.Printing.PaperSize("PaperA4", 840, 1180);
    '    ''pd.Print();

    '    'For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
    '    '    ps = PrintDocument1.PrinterSettings.PaperSizes(I)
    '    '    Debug.Print(ps.PaperName)
    '    '    If ps.Width = 800 And ps.Height = 600 Then
    '    '        PrintDocument1.DefaultPageSettings.PaperSize = ps
    '    '        e.PageSettings.PaperSize = ps
    '    '        PpSzSTS = True
    '    '        Exit For
    '    '    End If
    '    'Next

    '    'If PpSzSTS = False Then
    '    '    For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
    '    '        If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.GermanStandardFanfold Then
    '    '            ps = PrintDocument1.PrinterSettings.PaperSizes(I)
    '    '            PrintDocument1.DefaultPageSettings.PaperSize = ps
    '    '            e.PageSettings.PaperSize = ps
    '    '            PpSzSTS = True
    '    '            Exit For
    '    '        End If
    '    '    Next

    '    '    If PpSzSTS = False Then
    '    '        For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
    '    '            If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
    '    '                ps = PrintDocument1.PrinterSettings.PaperSizes(I)
    '    '                PrintDocument1.DefaultPageSettings.PaperSize = ps
    '    '                e.PageSettings.PaperSize = ps
    '    '                Exit For
    '    '            End If
    '    '        Next
    '    '    End If

    '    'End If

    '    With PrintDocument1.DefaultPageSettings.Margins
    '        .Left = 30 ' 50
    '        .Right = 30  '50
    '        .Top = 25
    '        .Bottom = 30 ' 50
    '        LMargin = .Left
    '        RMargin = .Right
    '        TMargin = .Top
    '        BMargin = .Bottom
    '    End With

    '    pFont = New Font("Calibri", 11, FontStyle.Regular)

    '    e.Graphics.TextRenderingHint = Drawing.Text.TextRenderingHint.AntiAlias

    '    With PrintDocument1.DefaultPageSettings.PaperSize
    '        PrintWidth = .Width - RMargin - LMargin
    '        PrintHeight = .Height - TMargin - BMargin
    '        PageWidth = .Width - RMargin
    '        PageHeight = .Height - BMargin
    '    End With
    '    If PrintDocument1.DefaultPageSettings.Landscape = True Then
    '        With PrintDocument1.DefaultPageSettings.PaperSize
    '            PrintWidth = .Height - TMargin - BMargin
    '            PrintHeight = .Width - RMargin - LMargin
    '            PageWidth = .Height - TMargin
    '            PageHeight = .Width - RMargin
    '        End With
    '    End If

    '    NoofItems_PerPage = 5 ' 6 ' 5

    '    Erase LnAr
    '    Erase ClArr

    '    LnAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
    '    ClArr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}

    '    ClArr(1) = 40
    '    ClArr(2) = 60 : ClArr(3) = 80 : ClArr(4) = 210 : ClArr(5) = 100 : ClArr(6) = 70 : ClArr(7) = 70
    '    ClArr(8) = PageWidth - (LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7))

    '    'ClArr(1) = Val(40)
    '    'ClArr(2) = 60 : ClArr(3) = 80 : ClArr(4) = 150 : ClArr(5) = 100 : ClArr(6) = 70 : ClArr(7) = 70
    '    'ClArr(8) = PageWidth - (LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7))


    '    TxtHgt = 18.8  ' 19 ' e.Graphics.MeasureString("A", pFont).Height  ' 20

    '    EntryCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

    '    Try

    '        If prn_HdDt.Rows.Count > 0 Then

    '            Printing_Format1_PageHeader(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, prn_PageNo, NoofItems_PerPage, CurY, LnAr, ClArr)

    '            Try

    '                NoofDets = 0

    '                CurY = CurY - 10

    '                If prn_DetDt.Rows.Count > 0 Then

    '                    Do While prn_DetIndx <= prn_DetDt.Rows.Count - 1

    '                        If NoofDets >= NoofItems_PerPage Then
    '                            CurY = CurY + TxtHgt

    '                            Common_Procedures.Print_To_PrintDocument(e, "Continued....", PageWidth - 10, CurY, 1, 0, pFont)

    '                            NoofDets = NoofDets + 1

    '                            Printing_Format1_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, NoofDets, False)

    '                            e.HasMorePages = True
    '                            Return

    '                        End If

    '                        prn_DetSNo = prn_DetSNo + 1

    '                        ItmNm1 = Trim(prn_DetDt.Rows(prn_DetIndx).Item("Mill_Name").ToString)
    '                        ItmNm2 = ""
    '                        If Len(ItmNm1) > 18 Then
    '                            For I = 18 To 1 Step -1
    '                                If Mid$(Trim(ItmNm1), I, 1) = " " Or Mid$(Trim(ItmNm1), I, 1) = "," Or Mid$(Trim(ItmNm1), I, 1) = "." Or Mid$(Trim(ItmNm1), I, 1) = "-" Or Mid$(Trim(ItmNm1), I, 1) = "/" Or Mid$(Trim(ItmNm1), I, 1) = "_" Or Mid$(Trim(ItmNm1), I, 1) = "(" Or Mid$(Trim(ItmNm1), I, 1) = ")" Or Mid$(Trim(ItmNm1), I, 1) = "\" Or Mid$(Trim(ItmNm1), I, 1) = "[" Or Mid$(Trim(ItmNm1), I, 1) = "]" Or Mid$(Trim(ItmNm1), I, 1) = "{" Or Mid$(Trim(ItmNm1), I, 1) = "}" Then Exit For
    '                            Next I
    '                            If I = 0 Then I = 18
    '                            ItmNm2 = Microsoft.VisualBasic.Right(Trim(ItmNm1), Len(ItmNm1) - I)
    '                            ItmNm1 = Microsoft.VisualBasic.Left(Trim(ItmNm1), I - 1)
    '                        End If


    '                        CurY = CurY + TxtHgt

    '                        Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(prn_DetIndx).Item("Sl_No").ToString), LMargin + 25, CurY, 1, 0, pFont)
    '                        Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Yarn_Type").ToString, LMargin + ClArr(1) + 10, CurY, 0, 0, pFont)
    '                        If IsDBNull(prn_DetDt.Rows(prn_DetIndx).Item("Job_Card_No").ToString) = False Then
    '                            If Trim(prn_DetDt.Rows(prn_DetIndx).Item("Job_Card_No").ToString) <> "" Then
    '                                Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetDt.Rows(prn_DetIndx).Item("Job_Card_No").ToString), LMargin + ClArr(1) + ClArr(2) + 10, CurY, 0, 0, pFont)
    '                            End If
    '                        End If
    '                        Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm1), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + 10, CurY, 0, 0, pFont)
    '                        Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Count_Name").ToString, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + 10, CurY, 0, 0, pFont)
    '                        If Val(prn_DetDt.Rows(prn_DetIndx).Item("Bags").ToString) <> 0 Then
    '                            Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(prn_DetIndx).Item("Bags").ToString), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) - 10, CurY, 1, 0, pFont)
    '                        End If
    '                        If Val(prn_DetDt.Rows(prn_DetIndx).Item("Cones").ToString) <> 0 Then
    '                            Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(prn_DetIndx).Item("Cones").ToString), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) - 10, CurY, 1, 0, pFont)
    '                        End If
    '                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Weight").ToString), "########0.000"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) - 10, CurY, 1, 0, pFont)

    '                        NoofDets = NoofDets + 1

    '                        If Trim(ItmNm2) <> "" Then
    '                            CurY = CurY + TxtHgt - 5
    '                            Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm2), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + 10, CurY, 0, 0, pFont)
    '                            NoofDets = NoofDets + 1
    '                        End If

    '                        prn_DetIndx = prn_DetIndx + 1

    '                    Loop

    '                End If

    '                Printing_Format1_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, NoofDets, True)

    '            Catch ex As Exception

    '                MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

    '            End Try

    '        End If

    '    Catch ex As Exception

    '        MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

    '    End Try

    '    e.HasMorePages = False

    'End Sub

    'Private Sub Printing_Format1_PageHeader(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByRef PageNo As Integer, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single)
    '    Dim da2 As New SqlClient.SqlDataAdapter
    '    Dim dt2 As New DataTable
    '    Dim Cmp_Name As String, Cmp_Add1 As String, Cmp_Add2 As String
    '    Dim Cmp_PhNo As String, Cmp_TinNo As String, Cmp_CstNo As String
    '    Dim p1Font As Font
    '    Dim strHeight As Single
    '    Dim W1 As Single, C1 As Single, S1 As Single

    '    PageNo = PageNo + 1

    '    CurY = TMargin

    '    da2 = New SqlClient.SqlDataAdapter("select a.*, b.Mill_Name, c.Count_Name  from Job_Card_Details a INNER JOIN Mill_Head b on a.Mill_IdNo = b.Mill_IdNo LEFT OUTER JOIN Count_Head c on a.Count_IdNo = c.Count_IdNo where a.Job_Card_Code = '" & Trim(EntryCode) & "' Order by a.sl_no", con)
    '    da2.Fill(dt2)
    '    If dt2.Rows.Count > NoofItems_PerPage Then
    '        Common_Procedures.Print_To_PrintDocument(e, "Page : " & Trim(Val(PageNo)), PageWidth - 10, CurY - TxtHgt, 1, 0, pFont)
    '    End If
    '    dt2.Clear()


    '    e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
    '    LnAr(1) = CurY

    '    Cmp_Name = "" : Cmp_Add1 = "" : Cmp_Add2 = ""
    '    Cmp_PhNo = "" : Cmp_TinNo = "" : Cmp_CstNo = ""

    '    If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1015" Then
    '        Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString
    '        Cmp_Add1 = prn_HdDt.Rows(0).Item("Company_Address1").ToString
    '        Cmp_Add2 = prn_HdDt.Rows(0).Item("Company_Address2").ToString & " " & prn_HdDt.Rows(0).Item("Company_Address3").ToString & " " & prn_HdDt.Rows(0).Item("Company_Address4").ToString

    '    Else

    '        If Trim(prn_HdDt.Rows(0).Item("Company_Division_Name").ToString) <> "" Then
    '            Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString
    '            Cmp_Name = Replace(Cmp_Name, Trim(prn_HdDt.Rows(0).Item("Company_Division_Name").ToString), "")
    '            Cmp_Add1 = Trim(prn_HdDt.Rows(0).Item("Company_Division_Name").ToString)
    '            Cmp_Add2 = prn_HdDt.Rows(0).Item("Company_Address1").ToString & " " & prn_HdDt.Rows(0).Item("Company_Address2").ToString & " " & prn_HdDt.Rows(0).Item("Company_Address3").ToString & " " & prn_HdDt.Rows(0).Item("Company_Address4").ToString

    '        Else
    '            Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString
    '            Cmp_Add1 = prn_HdDt.Rows(0).Item("Company_Address1").ToString & " " & prn_HdDt.Rows(0).Item("Company_Address2").ToString
    '            Cmp_Add2 = prn_HdDt.Rows(0).Item("Company_Address3").ToString & " " & prn_HdDt.Rows(0).Item("Company_Address4").ToString

    '        End If

    '    End If
    '    'Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString
    '    'Cmp_Add1 = prn_HdDt.Rows(0).Item("Company_Address1").ToString & " " & prn_HdDt.Rows(0).Item("Company_Address2").ToString
    '    'Cmp_Add2 = prn_HdDt.Rows(0).Item("Company_Address3").ToString & " " & prn_HdDt.Rows(0).Item("Company_Address4").ToString
    '    If Trim(prn_HdDt.Rows(0).Item("Company_PhoneNo").ToString) <> "" Then
    '        Cmp_PhNo = "PHONE NO.:" & prn_HdDt.Rows(0).Item("Company_PhoneNo").ToString
    '    End If
    '    If Trim(prn_HdDt.Rows(0).Item("Company_TinNo").ToString) <> "" Then
    '        Cmp_TinNo = "TIN NO.: " & prn_HdDt.Rows(0).Item("Company_TinNo").ToString
    '    End If
    '    If Trim(prn_HdDt.Rows(0).Item("Company_CstNo").ToString) <> "" Then
    '        Cmp_CstNo = "CST NO.: " & prn_HdDt.Rows(0).Item("Company_CstNo").ToString
    '    End If

    '    CurY = CurY + TxtHgt - 10
    '    p1Font = New Font("Calibri", 18, FontStyle.Bold)
    '    Common_Procedures.Print_To_PrintDocument(e, Cmp_Name, LMargin, CurY, 2, PrintWidth, p1Font)
    '    strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height

    '    If Trim(prn_HdDt.Rows(0).Item("Company_Division_Name").ToString) <> "" Then
    '        p1Font = New Font("Calibri", 12, FontStyle.Bold)
    '    Else
    '        p1Font = New Font("Calibri", 9, FontStyle.Regular)
    '    End If
    '    CurY = CurY + strHeight
    '    Common_Procedures.Print_To_PrintDocument(e, Cmp_Add1, LMargin, CurY, 2, PrintWidth, p1Font)

    '    strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height
    '    CurY = CurY + strHeight
    '    Common_Procedures.Print_To_PrintDocument(e, Cmp_Add2, LMargin, CurY, 2, PrintWidth, pFont)

    '    'CurY = CurY + strHeight
    '    'Common_Procedures.Print_To_PrintDocument(e, Cmp_Add1 & " " & Cmp_Add2, LMargin, CurY, 2, PrintWidth, pFont)

    '    CurY = CurY + TxtHgt
    '    Common_Procedures.Print_To_PrintDocument(e, Cmp_PhNo, LMargin, CurY, 2, PrintWidth, pFont)
    '    CurY = CurY + TxtHgt
    '    Common_Procedures.Print_To_PrintDocument(e, Cmp_TinNo, LMargin + 10, CurY, 0, 0, pFont)
    '    Common_Procedures.Print_To_PrintDocument(e, Cmp_CstNo, PageWidth - 10, CurY, 1, 0, pFont)

    '    CurY = CurY + TxtHgt - 13  ' 10
    '    p1Font = New Font("Calibri", 14, FontStyle.Bold)
    '    Common_Procedures.Print_To_PrintDocument(e, "YARN DELIVERY NOTE", LMargin, CurY, 2, PrintWidth, p1Font)
    '    strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height

    '    'CurY = CurY + TxtHgt

    '    CurY = CurY + strHeight + 5 ' + 150
    '    e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
    '    LnAr(2) = CurY

    '    Try

    '        C1 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5)
    '        W1 = e.Graphics.MeasureString("BOOK NO  : ", pFont).Width
    '        S1 = e.Graphics.MeasureString("TO    :", pFont).Width

    '        CurY = CurY + TxtHgt - 5
    '        p1Font = New Font("Calibri", 11, FontStyle.Bold)
    '        Common_Procedures.Print_To_PrintDocument(e, "TO  :  " & "M/s. " & prn_HdDt.Rows(0).Item("Ledger_Name").ToString, LMargin + 10, CurY, 0, 0, p1Font)

    '        Common_Procedures.Print_To_PrintDocument(e, "DC.NO", LMargin + C1 + 10, CurY, 0, 0, pFont)
    '        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
    '        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Job_Card_No").ToString, LMargin + C1 + W1 + 25, CurY, 0, 0, p1Font)

    '        CurY = CurY + TxtHgt
    '        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Ledger_Address1").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
    '        p1Font = New Font("Calibri", 14, FontStyle.Bold)

    '        CurY = CurY + TxtHgt
    '        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Ledger_Address2").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
    '        Common_Procedures.Print_To_PrintDocument(e, "DATE", LMargin + C1 + 10, CurY, 0, 0, pFont)
    '        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
    '        Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Job_Card_Date").ToString), "dd-MM-yyyy").ToString, LMargin + C1 + W1 + 25, CurY, 0, 0, pFont)


    '        CurY = CurY + TxtHgt
    '        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Ledger_Address3").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)

    '        CurY = CurY + TxtHgt
    '        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Ledger_Address4").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
    '        If Trim(prn_HdDt.Rows(0).Item("Book_No").ToString) <> "" Then
    '            Common_Procedures.Print_To_PrintDocument(e, "BOOK NO", LMargin + C1 + 10, CurY, 0, 0, pFont)
    '            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
    '            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Book_No").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, pFont)
    '        End If

    '        CurY = CurY + TxtHgt + 5
    '        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
    '        LnAr(3) = CurY

    '        e.Graphics.DrawLine(Pens.Black, LMargin + C1, LnAr(3), LMargin + C1, LnAr(2))

    '        CurY = CurY + TxtHgt - 10
    '        Common_Procedures.Print_To_PrintDocument(e, "SNO", LMargin, CurY, 2, ClAr(1), pFont)
    '        Common_Procedures.Print_To_PrintDocument(e, "TYPE", LMargin + ClAr(1), CurY, 2, ClAr(2), pFont)
    '        Common_Procedures.Print_To_PrintDocument(e, "SET NO", LMargin + ClAr(1) + ClAr(2), CurY, 2, ClAr(3), pFont)
    '        Common_Procedures.Print_To_PrintDocument(e, "MILL NAME", LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, 2, ClAr(4), pFont)
    '        Common_Procedures.Print_To_PrintDocument(e, "COUNT", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 2, ClAr(5), pFont)
    '        Common_Procedures.Print_To_PrintDocument(e, "BAGS", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, 2, ClAr(6), pFont)
    '        Common_Procedures.Print_To_PrintDocument(e, "CONES", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, 2, ClAr(7), pFont)
    '        Common_Procedures.Print_To_PrintDocument(e, "WEIGHT", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, 2, ClAr(8), pFont)

    '        CurY = CurY + TxtHgt + 10
    '        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
    '        LnAr(4) = CurY

    '    Catch ex As Exception

    '        MessageBox.Show(ex.Message, "DOES NOT PRINT", MessageBoxButtons.OK, MessageBoxIcon.Error)

    '    End Try

    'End Sub

    '    Private Sub Printing_Format1_PageFooter(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single, ByVal NoofDets As Integer, ByVal is_LastPage As Boolean)
    '        Dim p1Font As Font
    '        Dim I As Integer
    '        Dim Cmp_Name As String

    '        Try

    '            For I = NoofDets + 1 To NoofItems_PerPage
    '                CurY = CurY + TxtHgt
    '            Next

    '            CurY = CurY + TxtHgt
    '            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
    '            LnAr(5) = CurY

    '            CurY = CurY + TxtHgt - 10
    '            If is_LastPage = True Then
    '                Common_Procedures.Print_To_PrintDocument(e, " TOTAL", LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, 2, ClAr(4), pFont)

    '                If Val(prn_HdDt.Rows(0).Item("Total_Bags").ToString) <> 0 Then
    '                    Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(0).Item("Total_Bags").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY, 1, 0, pFont)
    '                End If
    '                If Val(prn_HdDt.Rows(0).Item("Total_Cones").ToString) <> 0 Then
    '                    Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(0).Item("Total_Cones").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, pFont)
    '                End If
    '                If Val(prn_HdDt.Rows(0).Item("Total_Weight").ToString) <> 0 Then
    '                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Total_Weight").ToString), "#########0.000"), PageWidth - 10, CurY, 1, 0, pFont)
    '                End If
    '            End If

    '            CurY = CurY + TxtHgt - 15

    '            CurY = CurY + TxtHgt
    '            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
    '            LnAr(6) = CurY

    '            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1), CurY, LMargin + ClAr(1), LnAr(3))
    '            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2), CurY, LMargin + ClAr(1) + ClAr(2), LnAr(3))
    '            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3), LnAr(3))
    '            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), LnAr(3))
    '            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(3))
    '            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), LnAr(3))
    '            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), LnAr(3))

    '            CurY = CurY + TxtHgt - 5

    '            Common_Procedures.Print_To_PrintDocument(e, "Through : " & Trim(prn_HdDt.Rows(0).Item("Vehicle_No").ToString), LMargin + 10, CurY, 0, 0, pFont)
    '            If Val(prn_HdDt.Rows(0).Item("Empty_Beam").ToString) <> 0 Then
    '                Common_Procedures.Print_To_PrintDocument(e, " Empty Beam : " & Trim(prn_HdDt.Rows(0).Item("Empty_Beam").ToString), PageWidth - 430, CurY, 0, 0, pFont)
    '            End If
    '            If Val(prn_HdDt.Rows(0).Item("Empty_Bags").ToString) <> 0 Then
    '                Common_Procedures.Print_To_PrintDocument(e, " Empty Bags : " & Trim(prn_HdDt.Rows(0).Item("Empty_Bags").ToString), PageWidth - 280, CurY, 0, 0, pFont)
    '            End If
    '            If Val(prn_HdDt.Rows(0).Item("Empty_Cones").ToString) <> 0 Then
    '                Common_Procedures.Print_To_PrintDocument(e, " Empty Cones : " & Trim(prn_HdDt.Rows(0).Item("Empty_Cones").ToString), PageWidth - 150, CurY, 0, 0, pFont)
    '            End If

    '            CurY = CurY + TxtHgt + 10
    '            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
    '            LnAr(7) = CurY

    '            CurY = CurY + TxtHgt
    '            If Val(Common_Procedures.User.IdNo) <> 1 Then
    '                Common_Procedures.Print_To_PrintDocument(e, "(" & Trim(Common_Procedures.User.Name) & ")", LMargin + 400, CurY, 0, 0, pFont)
    '            End If
    '            CurY = CurY + TxtHgt

    '            If Trim(prn_HdDt.Rows(0).Item("Company_Division_Name").ToString) <> "" Then
    '                Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString
    '                Cmp_Name = Replace(Cmp_Name, Trim(prn_HdDt.Rows(0).Item("Company_Division_Name").ToString), "")

    '            Else
    '                Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString

    '            End If

    '            Common_Procedures.Print_To_PrintDocument(e, "Receiver's Signature", LMargin + 20, CurY, 0, 0, pFont)
    '            Common_Procedures.Print_To_PrintDocument(e, "Prepared By ", LMargin + 250, CurY, 0, 0, pFont)
    '            p1Font = New Font("Calibri", 12, FontStyle.Bold)
    '            Common_Procedures.Print_To_PrintDocument(e, "For " & Cmp_Name, LMargin + PageWidth - 75, CurY, 1, 0, p1Font)

    '            'e.Graphics.DrawLine(Pens.Black, PageWidth, CurY, PageWidth, LnAr(1))

    '            CurY = CurY + TxtHgt + 10

    '            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
    '            e.Graphics.DrawLine(Pens.Black, LMargin, LnAr(1), LMargin, CurY)
    '            e.Graphics.DrawLine(Pens.Black, PageWidth, LnAr(1), PageWidth, CurY)

    '        Catch ex As Exception

    '            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

    '        End Try

    '    End Sub

    Private Sub dgv_Details_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgv_Details.LostFocus
        On Error Resume Next
        If Not IsNothing(dgv_Details.CurrentCell) Then dgv_Details.CurrentCell.Selected = False
    End Sub



    Private Sub txt_EmptyBeam_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub txt_EmptyCones_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub txt_EmptyBags_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub txt_Ends_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_Ends.KeyDown
        If e.KeyCode = 40 Then txt_SlNo.Focus()
        If e.KeyCode = 38 Then txt_PcsLength.Focus() ' SendKeys.Send("+{TAB}")
    End Sub

    Private Sub txt_Ends_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Ends.KeyPress
        If Asc(e.KeyChar) = 13 Then
            txt_SlNo.Focus()
        End If
    End Sub


End Class