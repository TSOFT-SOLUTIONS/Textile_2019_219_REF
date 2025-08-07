Public Class Yarn_Transfer
    Implements Interface_MDIActions

    Private con As New SqlClient.SqlConnection(Common_Procedures.Connection_String)
    Private con1 As New SqlClient.SqlConnection(Common_Procedures.ConnectionString_CompanyGroupdetails)
    Private FrmLdSTS As Boolean = False
    Private New_Entry As Boolean = False
    Private Insert_Entry As Boolean = False
    Private Filter_Status As Boolean = False
    Private Pk_Condition As String = "YNTRA-"
    Private Prec_ActCtrl As New Control

    Private prn_HdDt As New DataTable
    Private prn_DetDt As New DataTable
    Private prn_PageNo As Integer
    Private prn_DetIndx As Integer
    Private prn_DetAr(50, 10) As String
    Private prn_DetMxIndx As Integer
    Private prn_NoofBmDets As Integer
    Private prn_DetSNo As Integer
    Private vcbo_KeyDwnVal As Double

    Public vmskOldText As String = ""
    Public vmskSelStrt As Integer = -1
    Private vLed_ID_Cond As Integer = 0

    Private vCLO_CONDT As String = ""
    Private FnYearCode1 As String = ""
    Private FnYearCode2 As String = ""

    Private Sub clear()
        New_Entry = False
        Insert_Entry = False
        pnl_filter.Visible = False
        pnl_back.Enabled = True
        lbl_RefNo.Text = ""
        lbl_RefNo.ForeColor = Color.Black
        dtp_Date.Text = ""
        msk_date.Text = ""
        cbo_PartyFrom.Text = ""
        cbo_PartyTo.Text = ""
        cbo_Millfrom.Text = ""
        cbo_MillTo.Text = ""
        txt_remarks.Text = ""
        cbo_Countfrom.Text = ""
        cbo_CountTo.Text = ""
        cbo_TypeFrom.Text = ""
        cbo_TypeTo.Text = ""
        txt_cones.Text = ""
        txt_bags.Text = ""
        txt_weightFrom.Text = ""
        txt_weightTo.Text = ""
        lbl_UserName.Text = "USER : " & Trim(UCase(Common_Procedures.User_IdNoToName(con1, Common_Procedures.User.IdNo)))


        cbo_weaving_job_no.Text = ""
        cbo_Sizing_JobCardNo.Text = ""

        cbo_ClothSales_OrderCode_forSelection_From.Text = ""
        cbo_ClothSales_OrderCode_forSelection_To.Text = ""

    End Sub

    Private Sub Yarn_Transfer_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated
        Dim da As SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim NoofComps As Integer
        Dim CompCondt As String

        Try

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_PartyFrom.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "LEDGER" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_PartyFrom.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If
            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_PartyTo.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "LEDGER" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_PartyTo.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If
            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Millfrom.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "MILL" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Millfrom.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If
            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_MillTo.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "MILL" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_MillTo.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If
            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Countfrom.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "COUNT" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Countfrom.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If
            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_CountTo.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "COUNT" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_CountTo.Text = Trim(Common_Procedures.Master_Return.Return_Value)
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

            'If FrmLdSTS = True Then

            '    lbl_Company.Text = ""
            '    lbl_Company.Tag = 0
            '    Common_Procedures.CompIdNo = 0

            '    Me.Text = ""

            '    CompCondt = ""
            '    If Trim(UCase(Common_Procedures.User.Type)) = "ACCOUNT" Then
            '        CompCondt = "Company_Type = 'ACCOUNT'"
            '    End If

            '    da = New SqlClient.SqlDataAdapter("select count(*) from Company_Head Where " & CompCondt & IIf(Trim(CompCondt) <> "", " and ", "") & " Company_IdNo <> 0", con)
            '    dt1 = New DataTable
            '    da.Fill(dt1)

            '    NoofComps = 0
            '    If dt1.Rows.Count > 0 Then
            '        If IsDBNull(dt1.Rows(0)(0).ToString) = False Then
            '            NoofComps = Val(dt1.Rows(0)(0).ToString)
            '        End If
            '    End If
            '    dt1.Clear()

            '    If Val(NoofComps) = 1 Then

            '        da = New SqlClient.SqlDataAdapter("select Company_IdNo, Company_Name from Company_Head Where " & CompCondt & IIf(Trim(CompCondt) <> "", " and ", "") & " Company_IdNo <> 0 Order by Company_IdNo", con)
            '        dt1 = New DataTable
            '        da.Fill(dt1)

            '        If dt1.Rows.Count > 0 Then
            '            If IsDBNull(dt1.Rows(0)(0).ToString) = False Then
            '                Common_Procedures.CompIdNo = Val(dt1.Rows(0)(0).ToString)
            '            End If
            '        End If
            '        dt1.Clear()

            '    Else

            '        Dim f As New Company_Selection
            '        f.ShowDialog()

            '    End If

            '    If Val(Common_Procedures.CompIdNo) <> 0 Then

            '        da = New SqlClient.SqlDataAdapter("select Company_IdNo, Company_Name from Company_Head where Company_IdNo = " & Str(Val(Common_Procedures.CompIdNo)), con)
            '        dt1 = New DataTable
            '        da.Fill(dt1)

            '        If dt1.Rows.Count > 0 Then
            '            If IsDBNull(dt1.Rows(0)(0).ToString) = False Then
            '                lbl_Company.Tag = Val(dt1.Rows(0)(0).ToString)
            '                lbl_Company.Text = Trim(dt1.Rows(0)(1).ToString)
            '                Me.Text = Trim(dt1.Rows(0)(1).ToString)
            '            End If
            '        End If
            '        dt1.Clear()

            '        new_record()

            '    Else
            '        'MessageBox.Show("Invalid Company Selection", "DOES NOT SELECT...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            '        Me.Close()
            '        Exit Sub


            '    End If

            'End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "DOES NOT SHOW...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        FrmLdSTS = False

    End Sub

    Private Sub Yarn_Transfer_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        con.Close()
        con.Dispose()
    End Sub

    Private Sub Yarn_Transfer_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        Dim da As SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim NoofComps As Integer
        Dim CompCondt As String

        Try

            If Asc(e.KeyChar) = 27 Then

                If pnl_filter.Visible = True Then
                    btn_closefilter_Click(sender, e)
                    Exit Sub
                End If

                lbl_Company.Tag = 0
                lbl_Company.Text = ""
                Me.Text = ""
                Common_Procedures.CompIdNo = 0

                CompCondt = ""
                If Trim(UCase(Common_Procedures.User.Type)) = "ACCOUNT" Then
                    CompCondt = "Company_Type = 'ACCOUNT'"
                End If

                da = New SqlClient.SqlDataAdapter("select count(*) from Company_Head Where " & CompCondt & IIf(Trim(CompCondt) <> "", " and ", "") & " Company_IdNo <> 0", con)
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

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT CLOSE...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub
    Private Sub ControlGotFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim txtbx As TextBox
        Dim combobx As ComboBox
        Dim Msktxbx As MaskedTextBox
        On Error Resume Next

        If TypeOf Me.ActiveControl Is TextBox Or TypeOf Me.ActiveControl Is ComboBox Or TypeOf Me.ActiveControl Is MaskedTextBox Then
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
            Msktxbx = Me.ActiveControl
            Msktxbx.SelectionStart = 0
        End If

        Prec_ActCtrl = Me.ActiveControl

    End Sub

    Private Sub ControlLostFocus(ByVal sender As Object, ByVal e As System.EventArgs)

        On Error Resume Next

        If IsNothing(Prec_ActCtrl) = False Then
            If TypeOf Prec_ActCtrl Is TextBox Or TypeOf Prec_ActCtrl Is ComboBox Or TypeOf Prec_ActCtrl Is MaskedTextBox Then
                Prec_ActCtrl.BackColor = Color.White
                Prec_ActCtrl.ForeColor = Color.Black
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

    Private Sub Yarn_Transfer_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim Dt2 As New DataTable
        Dim dt3 As New DataTable
        Dim Dt4 As New DataTable
        Dim dt5 As New DataTable
        Dim Dt6 As New DataTable
        Dim dt7 As New DataTable
        Me.Text = ""

        con.Open()

        Da = New SqlClient.SqlDataAdapter("select Ledger_DisplayName from Ledger_AlaisHead where (Ledger_IdNo = 0 or Ledger_Type = 'WEAVER' or Ledger_Type = 'SIZING' or Ledger_Type = 'REWINDING' or Ledger_Type = 'JOBWORKER') order by Ledger_DisplayName", con)
        'Da = New SqlClient.SqlDataAdapter("select a.Ledger_DisplayName from Ledger_AlaisHead a, ledger_head b where (Ledger_IdNo = 0 or Ledger_Type = 'WEAVER' or Ledger_Type = 'SIZING' or Ledger_Type = 'REWINDING' or Ledger_Type = 'JOBWORKER') and a.Ledger_IdNo = b.Ledger_IdNo order by a.Ledger_DisplayName", con)
        Da.Fill(Dt1)
        cbo_PartyFrom.DataSource = Dt1
        cbo_PartyFrom.DisplayMember = "Ledger_DisplayName"

        Da = New SqlClient.SqlDataAdapter("select Yarn_Type from YarnType_head order by Yarn_Type", con)
        Da.Fill(Dt2)
        cbo_TypeFrom.DataSource = Dt2
        cbo_TypeFrom.DisplayMember = "Yarn_Type"

        Da = New SqlClient.SqlDataAdapter("select Yarn_Type from YarnType_head order by Yarn_Type", con)
        Da.Fill(dt3)
        cbo_TypeTo.DataSource = dt3
        cbo_TypeTo.DisplayMember = "Yarn_Type"

        Da = New SqlClient.SqlDataAdapter("select Mill_Name from Mill_head order by Mill_Name", con)
        Da.Fill(Dt4)
        cbo_MillTo.DataSource = Dt4
        cbo_MillTo.DisplayMember = "Mill_Name"

        Da = New SqlClient.SqlDataAdapter("select Mill_Name from Mill_head order by Mill_Name", con)
        Da.Fill(dt5)
        cbo_Millfrom.DataSource = dt5
        cbo_Millfrom.DisplayMember = "Mill_Name"

        Da = New SqlClient.SqlDataAdapter("select Count_Name from Count_head order by Count_Name", con)
        Da.Fill(Dt6)
        cbo_Countfrom.DataSource = Dt6
        cbo_Countfrom.DisplayMember = "Count_Name"

        Da = New SqlClient.SqlDataAdapter("select Count_Name from Count_head order by Count_Name", con)
        Da.Fill(dt7)
        cbo_CountTo.DataSource = dt7
        cbo_CountTo.DisplayMember = "Count_Name"

        lbl_Company.Text = ""
        lbl_Company.Tag = 0
        lbl_Company.Visible = False
        Common_Procedures.CompIdNo = 0

        lbl_weaving_job_no.Visible = False
        cbo_weaving_job_no.Visible = False
        btn_UserModification.Visible = False

        lbl_Sizing_jobcardno_Caption.Visible = False
        cbo_Sizing_JobCardNo.Visible = False


        lbl_Sales_OrderNo_From.Visible = False
        lbl_Sales_OrderNo_To.Visible = False
        cbo_ClothSales_OrderCode_forSelection_From.Visible = False
        cbo_ClothSales_OrderCode_forSelection_To.Visible = False


        If Common_Procedures.settings.Show_Weaver_JobCard_Entry_STATUS = 1 Then
            lbl_weaving_job_no.Visible = True
            cbo_weaving_job_no.Visible = True
            cbo_weaving_job_no.BackColor = Color.White
        End If

        If Common_Procedures.settings.Show_Sizing_JobCard_Entry_Status = 1 Then

            lbl_Sizing_jobcardno_Caption.Visible = True
            cbo_Sizing_JobCardNo.Visible = True
            cbo_Sizing_JobCardNo.BackColor = Color.White

            If lbl_weaving_job_no.Visible = False And cbo_weaving_job_no.Visible = False Then
                lbl_Sizing_jobcardno_Caption.Left = Label1.Left
                cbo_Sizing_JobCardNo.Left = lbl_RefNo.Left
                cbo_Sizing_JobCardNo.Width = txt_remarks.Width

            End If

        End If

        If cbo_weaving_job_no.Visible = True And cbo_Sizing_JobCardNo.Visible = False Then
            cbo_weaving_job_no.Width = txt_remarks.Width
        End If


        If cbo_weaving_job_no.Visible = False And cbo_Sizing_JobCardNo.Visible = False And Common_Procedures.settings.Show_Sales_OrderNumber_in_ALLEntry_Status <> 1 Then
            lbl_remarks.Top = Label19.Bottom + 20
            txt_remarks.Top = Label19.Bottom + 15
        End If

        pnl_filter.Visible = False
        pnl_filter.Left = (Me.Width - pnl_filter.Width) \ 2
        pnl_filter.Top = ((Me.Height - pnl_filter.Height) \ 2) + 20

        If Common_Procedures.settings.User_Modifications_Show_Status = 1 Then
            If Val(Common_Procedures.User.IdNo) = 1 Or Common_Procedures.User.Show_UserModification_Status = 1 Then
                btn_UserModification.Visible = True
            End If
        End If

        If Common_Procedures.settings.Show_Sales_OrderNumber_in_ALLEntry_Status <> 1 Then
            lbl_remarks.Top = lbl_weaving_job_no.Bottom + 20
            txt_remarks.Top = lbl_weaving_job_no.Bottom + 15
        End If


        If Common_Procedures.settings.Show_Sales_OrderNumber_in_ALLEntry_Status <> 1 And Common_Procedures.settings.Show_Sizing_JobCard_Entry_Status <> 1 Then
            lbl_remarks.Top = Label16.Bottom + 15
            txt_remarks.Top = txt_weightFrom.Bottom + 10
        End If



        If Common_Procedures.settings.Show_Sales_OrderNumber_in_ALLEntry_Status = 1 Then

            lbl_Sales_OrderNo_From.Visible = True
            lbl_Sales_OrderNo_To.Visible = True
            cbo_ClothSales_OrderCode_forSelection_From.Visible = True
            cbo_ClothSales_OrderCode_forSelection_To.Visible = True

            FnYearCode1 = ""
            FnYearCode2 = ""
            Common_Procedures.get_FnYearCode_of_Last_2_Years(FnYearCode1, FnYearCode2)


            If cbo_Sizing_JobCardNo.Visible = False Then

                lbl_Sales_OrderNo_From.Top = Label16.Bottom + 20
                lbl_Sales_OrderNo_To.Top = Label21.Bottom + 20
                cbo_ClothSales_OrderCode_forSelection_From.Top = txt_weightFrom.Bottom + 20
                cbo_ClothSales_OrderCode_forSelection_To.Top = txt_weightTo.Bottom + 20


                lbl_remarks.Top = lbl_weaving_job_no.Bottom + 25
                txt_remarks.Top = cbo_weaving_job_no.Bottom + 20

            End If

        Else

            lbl_Sales_OrderNo_From.Visible = False
            lbl_Sales_OrderNo_To.Visible = False
            cbo_ClothSales_OrderCode_forSelection_From.Visible = False
            cbo_ClothSales_OrderCode_forSelection_To.Visible = False

        End If



        AddHandler msk_date.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Countfrom.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_CountTo.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Millfrom.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_MillTo.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_PartyFrom.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_PartyTo.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_TypeFrom.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_TypeTo.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_PartyNameFilter.GotFocus, AddressOf ControlGotFocus

        AddHandler txt_bags.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_cones.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_remarks.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_weightFrom.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_weightTo.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_weaving_job_no.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Sizing_JobCardNo.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_ClothSales_OrderCode_forSelection_From.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_ClothSales_OrderCode_forSelection_To.GotFocus, AddressOf ControlGotFocus

        AddHandler msk_date.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Countfrom.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_CountTo.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Millfrom.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_MillTo.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_PartyFrom.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_PartyTo.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_TypeFrom.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_TypeTo.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_PartyNameFilter.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_ClothSales_OrderCode_forSelection_From.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_ClothSales_OrderCode_forSelection_To.LostFocus, AddressOf ControlLostFocus

        AddHandler txt_bags.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_cones.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_remarks.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_weightFrom.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_weightTo.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_weaving_job_no.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Sizing_JobCardNo.LostFocus, AddressOf ControlLostFocus

        'AddHandler msk_date.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_bags.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_cones.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_weightFrom.KeyDown, AddressOf TextBoxControlKeyDown
        'AddHandler txt_weightTo.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler dtp_FilterFrom_date.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler dtp_FilterTo_date.KeyDown, AddressOf TextBoxControlKeyDown


        'AddHandler msk_date.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_bags.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_cones.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_weightFrom.KeyPress, AddressOf TextBoxControlKeyPress
        'AddHandler txt_weightTo.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler dtp_FilterFrom_date.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler dtp_FilterTo_date.KeyPress, AddressOf TextBoxControlKeyPress

        FrmLdSTS = True
        new_record()

    End Sub

    Private Sub move_record(ByVal no As String)
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim NewCode As String

        If Val(no) = 0 Then Exit Sub

        clear()

        Try

            NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(no) & "/" & Trim(Common_Procedures.FnYearCode)

            da1 = New SqlClient.SqlDataAdapter("select a.*, b.Ledger_Name, c.Mill_Name as Mill_Namefm ,d.Mill_Name ,e.Count_Name as Count_Namefm ,f.Count_Name,g.Yarn_type,h.Yarn_Type,i.Ledger_Name as Party_To from Yarn_Transfer_Head a INNER JOIN Ledger_Head b ON a.Ledger_IdNo = b.Ledger_IdNo INNER JOIN Ledger_Head I ON a.LedgerTo_IdNo = i.Ledger_IdNo  LEFT OUTER JOIN Mill_Head c ON a.Mill_IdNofrom = c.Mill_IdNo LEFT OUTER JOIN Mill_Head d ON a.Mill_IdNoto = d.Mill_IdNo LEFT OUTER JOIN Count_Head e ON a.Count_IdNofrom = e.Count_IdNo LEFT OUTER JOIN Count_Head f ON a.Count_IdNoto = f.Count_IdNo LEFT OUTER JOIN YarnType_Head g ON a.Yarn_Typefrom = g.Yarn_Type LEFT OUTER JOIN YarnType_Head h ON a.Yarn_Typeto=h.Yarn_type where a.Yarn_Transfer_Code = '" & Trim(NewCode) & "'", con)
            da1.Fill(dt1)

            If dt1.Rows.Count > 0 Then
                lbl_RefNo.Text = dt1.Rows(0).Item("Yarn_Transfer_No").ToString
                dtp_Date.Text = dt1.Rows(0).Item("Yarn_Transfer_Date").ToString
                msk_date.Text = dtp_Date.Text
                cbo_PartyFrom.Text = dt1.Rows(0).Item("Ledger_Name").ToString
                cbo_PartyTo.Text = dt1.Rows(0).Item("Party_To").ToString
                cbo_TypeFrom.Text = dt1.Rows(0).Item("Yarn_Typefrom").ToString
                cbo_TypeTo.Text = dt1.Rows(0).Item("Yarn_Typeto").ToString
                cbo_Millfrom.Text = dt1.Rows(0).Item("Mill_Namefm").ToString
                cbo_MillTo.Text = dt1.Rows(0).Item("Mill_Name").ToString
                cbo_Countfrom.Text = dt1.Rows(0).Item("Count_Namefm").ToString
                cbo_CountTo.Text = dt1.Rows(0).Item("Count_Name").ToString
                txt_remarks.Text = dt1.Rows(0).Item("Remarks").ToString
                txt_cones.Text = dt1.Rows(0).Item("Yarn_Cones").ToString
                txt_bags.Text = dt1.Rows(0).Item("Yarn_Bags").ToString
                txt_weightFrom.Text = dt1.Rows(0).Item("Yarn_Weight1").ToString
                txt_weightTo.Text = dt1.Rows(0).Item("Yarn_Weight2").ToString
                lbl_UserName.Text = "USER : " & Trim(UCase(Common_Procedures.User_IdNoToName(con1, Val(dt1.Rows(0).Item("User_IdNo").ToString))))

                cbo_weaving_job_no.Text = dt1.Rows(0).Item("Weaving_JobCode_forSelection").ToString
                cbo_Sizing_JobCardNo.Text = dt1.Rows(0).Item("Sizing_JobCode_forSelection").ToString

                cbo_ClothSales_OrderCode_forSelection_To.Text = dt1.Rows(0).Item("ClothSales_OrderCode_forSelection_To").ToString
                cbo_ClothSales_OrderCode_forSelection_From.Text = dt1.Rows(0).Item("ClothSales_OrderCode_forSelection_From").ToString


            End If

            dt1.Dispose()
            da1.Dispose()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        If cbo_PartyFrom.Visible And cbo_PartyFrom.Enabled Then cbo_PartyFrom.Focus()

    End Sub

    Public Sub delete_record() Implements Interface_MDIActions.delete_record
        Dim cmd As New SqlClient.SqlCommand
        Dim tr As SqlClient.SqlTransaction
        Dim NewCode As String = ""
        Dim vOrdByNo As String = ""
        vOrdByNo = Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text)

        If MessageBox.Show("Do you want to Delete?", "FOR DELETION...", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.No Then
            Exit Sub
        End If
        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.DeletingEntry, Common_Procedures.UR.Yarn_Transfer_Entry, New_Entry, Me, con, "Yarn_Transfer_Head", "Cotton_Bora_Stitching_Code", NewCode, "Yarn_Transfer_Date", "(Yarn_Transfer_Code = '" & Trim(NewCode) & "')") = False Then Exit Sub

        If New_Entry = True Then
            MessageBox.Show("This is New Entry", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        tr = con.BeginTransaction

        Try

            NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            cmd.Connection = con
            cmd.Transaction = tr

            Call Common_Procedures.User_Modification_Updation(con, "HEAD", Me.Name, "DELETE", "Yarn_Transfer_Head", "Yarn_Transfer_Code", Val(lbl_Company.Tag), NewCode, lbl_RefNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, True, "", "", "Yarn_Transfer_Code, Company_IdNo, for_OrderBy", tr)

            cmd.CommandText = "Truncate Table TempTable_For_NegativeStock"
            cmd.ExecuteNonQuery()

            If Val(Common_Procedures.settings.Negative_Stock_Restriction_for_Yarn_Stock) = 1 Then

                cmd.CommandText = "Insert into TempTable_For_NegativeStock ( Stock_Type, Reference_Code, Reference_Date, Company_Idno, Ledger_Idno    , Count_IdNo, Yarn_Type, Mill_IdNo ) " &
                                      " Select                               'YARN'    , Reference_Code, Reference_Date, Company_IdNo, DeliveryTo_Idno, Count_IdNo, Yarn_Type, Mill_IdNo from Stock_Yarn_Processing_Details where Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()

            End If

            cmd.CommandText = "Delete from Stock_Yarn_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "delete from Yarn_Transfer_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Yarn_Transfer_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            If Val(Common_Procedures.settings.Negative_Stock_Restriction_for_Yarn_Stock) = 1 Then
                If Common_Procedures.Check_is_Negative_Stock_Status(con, tr) = True Then Exit Sub
            End If

            tr.Commit()

            new_record()

            MessageBox.Show("Deleted Sucessfully!!!", "FOR DELETION...", MessageBoxButtons.OK, MessageBoxIcon.Information)

        Catch ex As Exception

            tr.Rollback()
            MessageBox.Show(ex.Message, "FOR DELETION...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        If msk_date.Enabled = True And msk_date.Visible = True Then msk_date.Focus()

    End Sub

    Public Sub filter_record() Implements Interface_MDIActions.filter_record
        If Filter_Status = False Then
            Dim da As New SqlClient.SqlDataAdapter
            Dim dt1 As New DataTable

            da = New SqlClient.SqlDataAdapter("select a.Ledger_DisplayName from Ledger_AlaisHead a, ledger_head b where (a.Ledger_IdNo = 0 or b.AccountsGroup_IdNo = 10) and a.Ledger_IdNo = b.Ledger_IdNo order by a.Ledger_DisplayName", con)
            da.Fill(dt1)
            cbo_PartyNameFilter.DataSource = dt1
            cbo_PartyNameFilter.DisplayMember = "Ledger_DisplayName"

            dtp_FilterFrom_date.Text = ""
            dtp_FilterTo_date.Text = ""
            pnl_filter.Text = ""
            cbo_PartyNameFilter.SelectedIndex = -1
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


        ' If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.Yarn_Transfer_Entry, "~L~") = 0 And InStr(Common_Procedures.UR.Yarn_Transfer_Entry, "~I~") = 0 Then MessageBox.Show("You have No Rights to Insert", "DOES NOT INSERT...", MessageBoxButtons.OK, MessageBoxIcon.Error) : Exit Sub
        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.InsertingEntry, Common_Procedures.UR.Yarn_Transfer_Entry, New_Entry, Me) = False Then Exit Sub

        Try

            inpno = InputBox("Enter New Ref No.", "FOR INSERTION...")

            NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

            cmd.Connection = con
            cmd.CommandText = "select Yarn_Transfer_No from Yarn_Transfer_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Yarn_Transfer_Code = '" & Trim(NewCode) & "'"
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
                    MessageBox.Show("Invalid Ref No.", "DOES NOT INSERT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

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
            cmd.CommandText = "select top 1 Yarn_Transfer_No from Yarn_Transfer_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Yarn_Transfer_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby, Yarn_Transfer_No"
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
            cmd.CommandText = "select top 1 Yarn_Transfer_No from Yarn_Transfer_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Yarn_Transfer_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, Yarn_Transfer_No desc"
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
            cmd.CommandText = "select top 1 Yarn_Transfer_No from Yarn_Transfer_Head where for_orderby > " & Str(OrdByNo) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and Yarn_Transfer_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby,Yarn_Transfer_No"
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
            cmd.CommandText = "select top 1 Yarn_Transfer_No from Yarn_Transfer_Head where for_orderby < " & Str(OrdByNo) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and Yarn_Transfer_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc,Yarn_Transfer_No desc"
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

            da = New SqlClient.SqlDataAdapter("select max(for_orderby) from Yarn_Transfer_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Yarn_Transfer_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' ", con)
            da.Fill(dt)

            NewID = 0
            If dt.Rows.Count > 0 Then
                If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                    NewID = Val(dt.Rows(0)(0).ToString)
                End If
            End If

            dt.Dispose()
            da.Dispose()

            NewID = NewID + 1

            lbl_RefNo.Text = NewID
            lbl_RefNo.ForeColor = Color.Red


            msk_date.Text = Date.Today.ToShortDateString
            da = New SqlClient.SqlDataAdapter("select top 1 * from Yarn_Transfer_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Yarn_Transfer_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, Yarn_Transfer_No desc", con)
            dt1 = New DataTable
            da.Fill(dt1)
            If dt1.Rows.Count > 0 Then
                If Val(Common_Procedures.settings.PreviousEntryDate_ByDefault) = 1 Then '---- M.S Textiles (Tirupur)
                    If dt1.Rows(0).Item("Yarn_Transfer_Date").ToString <> "" Then msk_date.Text = dt1.Rows(0).Item("Yarn_Transfer_Date").ToString
                End If
            End If
            dt1.Clear()

            If msk_date.Enabled And msk_date.Visible Then msk_date.Focus()

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

            inpno = InputBox("Enter Ref No", "FOR FINDING...")

            NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

            cmd.Connection = con
            cmd.CommandText = "select Yarn_Transfer_No from Yarn_Transfer_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Yarn_Transfer_Code = '" & Trim(NewCode) & "'"
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
                MessageBox.Show("Ref No. Does not exists", "DOES NOT FIND...", MessageBoxButtons.OK, MessageBoxIcon.Error)

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT FIND...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Public Sub save_record() Implements Interface_MDIActions.save_record
        Dim cmd As New SqlClient.SqlCommand
        Dim tr As SqlClient.SqlTransaction
        Dim da As SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim dt3 As New DataTable
        Dim dt4 As New DataTable
        Dim dt5 As New DataTable
        Dim NewCode As String = ""
        Dim NewNo As Long = 0
        Dim led_id As Integer = 0
        Dim led_id1 As Integer = 0
        Dim ty_id As Integer = 0
        Dim ty_id1 As Integer = 0
        Dim mil_id As Integer = 0
        Dim mil_id1 As Integer = 0
        Dim cou_id As Integer = 0
        Dim cou_id1 As Integer = 0
        Dim Partcls As String = ""
        Dim PBlNo As String = ""
        Dim EntID As String = ""
        Dim Led_Frmtype As String = ""
        Dim Led_Totype As String = ""
        Dim Stk_DelvIdNo As Integer = 0, Stk_RecIdNo As Integer = 0
        Dim Prtcls_DelvIdNo As Integer = 0, Prtcls_RecIdNo As Integer = 0
        Dim vENTDB_DelvToIDno As String = 0
        Dim vOrdByNo As String = ""


        vOrdByNo = Common_Procedures.OrderBy_CodeToValue(lbl_Company.Text)

        If pnl_back.Enabled = False Then
            MessageBox.Show("Close Other Windows", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        'If Common_Procedures.UserRight_Check(Common_Procedures.UR.Yarn_Transfer_Entry, New_Entry) = False Then Exit Sub

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)
        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.SavingEntry, Common_Procedures.UR.Yarn_Transfer_Entry, New_Entry, Me, con, "Yarn_Transfer_Head", "Yarn_Transfer_Code", NewCode, "Yarn_Transfer_Date", "(Yarn_Transfer_Code = '" & Trim(NewCode) & "')", "(Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Yarn_Transfer_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "')", "for_Orderby desc, Yarn_Transfer_No desc", dtp_Date.Value.Date) = False Then Exit Sub


        If Val(lbl_Company.Tag) = 0 Then
            MessageBox.Show("Invalid Company Selection", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
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

        led_id = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_PartyFrom.Text)

        If led_id = 0 Then
            MessageBox.Show("Invalid Party Name From", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If cbo_PartyFrom.Enabled Then cbo_PartyFrom.Focus()
            Exit Sub
        End If


        led_id1 = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_PartyTo.Text)

        If led_id1 = 0 Then
            MessageBox.Show("Invalid Party Name To", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If cbo_PartyTo.Enabled Then cbo_PartyTo.Focus()
            Exit Sub
        End If

        'If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1087" Then '---- Kalaimagal Textiles (Palladam)
        If Trim(UCase(cbo_TypeFrom.Text)) = "" Then
            MessageBox.Show("Invalid Yarn Type from", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If cbo_TypeFrom.Enabled Then cbo_TypeFrom.Focus()
            Exit Sub
        End If

        If Trim(UCase(cbo_TypeTo.Text)) = "" Then
            MessageBox.Show("Invalid Yarn Type To", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If cbo_TypeTo.Enabled Then cbo_TypeTo.Focus()
            Exit Sub
        End If
        'End If

        mil_id = Common_Procedures.Mill_NameToIdNo(con, cbo_Millfrom.Text)
        'If mil_id = 0 Then
        '    MessageBox.Show("Invalid MILL Name from", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
        '    If cbo_Millfrom.Enabled Then cbo_Millfrom.Focus()
        '    Exit Sub
        'End If

        mil_id1 = Common_Procedures.Mill_NameToIdNo(con, cbo_MillTo.Text)
        'If mil_id1 = 0 Then
        '    MessageBox.Show("Invalid Mill Name To", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
        '    If cbo_MillTo.Enabled Then cbo_MillTo.Focus()
        '    Exit Sub
        'End If

        cou_id = Common_Procedures.Count_NameToIdNo(con, cbo_Countfrom.Text)
        If cou_id = 0 Then
            MessageBox.Show("Invalid Count Name from", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If cbo_Countfrom.Enabled Then cbo_Countfrom.Focus()
            Exit Sub
        End If

        cou_id1 = Common_Procedures.Count_NameToIdNo(con, cbo_CountTo.Text)
        If cou_id1 = 0 Then
            MessageBox.Show("Invalid Count Name To", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If cbo_CountTo.Enabled Then cbo_CountTo.Focus()
            Exit Sub
        End If

        If Val(txt_weightFrom.Text) < 0 Then
            MessageBox.Show("Invalid Weight From", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If txt_weightFrom.Enabled Then txt_weightFrom.Focus()
            Exit Sub
        End If
        If Val(txt_weightTo.Text) < 0 Then
            MessageBox.Show("Invalid Weight To", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If txt_weightTo.Enabled Then txt_weightTo.Focus()
            Exit Sub
        End If

        lbl_UserName.Text = Common_Procedures.User.IdNo


        If Trim(cbo_weaving_job_no.Text) <> "" Then
            If Common_Procedures.Cross_Checking_For_Weaving_Job_Code_For_Selecion(con, Val(led_id), Trim(cbo_weaving_job_no.Text), Val(cou_id)) = True Then
                MessageBox.Show("MisMatch of Party Job No Details", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                If cbo_PartyFrom.Enabled Then cbo_PartyFrom.Focus()
                Exit Sub
            End If
        End If
        If Trim(cbo_Sizing_JobCardNo.Text) <> "" Then
            If Common_Procedures.Cross_Checking_For_Sizing_Job_Code_For_Selecion(con, Val(led_id), Trim(cbo_Sizing_JobCardNo.Text), Val(cou_id)) = True Then
                MessageBox.Show("MisMatch of Party Job No Details", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                If cbo_PartyFrom.Enabled Then cbo_PartyFrom.Focus()
                Exit Sub
            End If
        End If

        If Common_Procedures.settings.Sales_OrderNumber_compulsory_in_ALLEntry_Status = 1 Then
            If Trim(cbo_ClothSales_OrderCode_forSelection_From.Text) = "" Then
                MessageBox.Show("Invalid Sales Order No From", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                If cbo_ClothSales_OrderCode_forSelection_From.Enabled And cbo_ClothSales_OrderCode_forSelection_From.Visible Then cbo_ClothSales_OrderCode_forSelection_From.Focus()
                Exit Sub
            End If
        End If

        If Common_Procedures.settings.Sales_OrderNumber_compulsory_in_ALLEntry_Status = 1 Then
            If Trim(cbo_ClothSales_OrderCode_forSelection_To.Text) = "" Then
                MessageBox.Show("Invalid Sales Order No To", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                If cbo_ClothSales_OrderCode_forSelection_To.Enabled And cbo_ClothSales_OrderCode_forSelection_To.Visible Then cbo_ClothSales_OrderCode_forSelection_To.Focus()
                Exit Sub
            End If
        End If


        tr = con.BeginTransaction

        Try

            If Insert_Entry = True Or New_Entry = False Then
                NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            Else

                da = New SqlClient.SqlDataAdapter("select max(for_orderby) from Yarn_Transfer_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and  Yarn_Transfer_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' ", con)
                da.SelectCommand.Transaction = tr
                da.Fill(dt4)

                NewNo = 0
                If dt4.Rows.Count > 0 Then
                    If IsDBNull(dt4.Rows(0)(0).ToString) = False Then
                        NewNo = Int(Val(dt4.Rows(0)(0).ToString))
                        NewNo = Val(NewNo) + 1
                    End If
                End If
                dt4.Clear()
                If Trim(NewNo) = "" Then NewNo = Trim(lbl_RefNo.Text)

                lbl_RefNo.Text = Trim(NewNo)

                NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            End If

            cmd.Connection = con
            cmd.Transaction = tr

            cmd.Parameters.Clear()
            cmd.Parameters.AddWithValue("@ReceiptDate", Convert.ToDateTime(msk_date.Text))

            cmd.CommandText = "Truncate Table TempTable_For_NegativeStock"
            cmd.ExecuteNonQuery()

            If New_Entry = True Then

                cmd.CommandText = "Insert into Yarn_Transfer_Head( Yarn_Transfer_Code, Company_IdNo, Yarn_Transfer_No, for_OrderBy, Yarn_Transfer_Date, Ledger_IdNo,LedgerTo_IdNo, Yarn_Typefrom,Yarn_Typeto,Mill_IdNofrom,Mill_IdNoto,Count_IdNofrom,Count_IdNoto,Yarn_Bags,Yarn_Cones,Yarn_Weight1,Yarn_Weight2,Remarks, User_idNo ,Weaving_JobCode_forSelection ,Sizing_JobCode_forSelection , ClothSales_OrderCode_forSelection_From , ClothSales_OrderCode_forSelection_To ) Values ('" & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_RefNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text))) & ", @ReceiptDate, " & Val(led_id) & ", " & Val(led_id1) & ",'" & Trim(cbo_TypeFrom.Text) & "','" & Trim(cbo_TypeTo.Text) & "'," & Val(mil_id) & "," & Val(mil_id1) & "," & Val(cou_id) & "," & Val(cou_id1) & ", " & Val(txt_bags.Text) & "," & Val(txt_cones.Text) & "," & Val(txt_weightFrom.Text) & " , " & Val(txt_weightTo.Text) & ", '" & Trim(txt_remarks.Text) & "', " & Val(lbl_UserName.Text) & "  ,'" & Trim(cbo_weaving_job_no.Text) & "' , '" & Trim(cbo_Sizing_JobCardNo.Text) & "' , '" & Trim(cbo_ClothSales_OrderCode_forSelection_From.Text) & "',  '" & Trim(cbo_ClothSales_OrderCode_forSelection_To.Text) & "'    )"

                cmd.ExecuteNonQuery()

            Else


                Call Common_Procedures.User_Modification_Updation(con, "HEAD", Me.Name, "OLD", "Yarn_Transfer_Head", "Yarn_Transfer_Code", Val(lbl_Company.Tag), NewCode, lbl_RefNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, False, "", "", "Yarn_Transfer_Code, Company_IdNo, for_OrderBy", tr)

                If Val(Common_Procedures.settings.Negative_Stock_Restriction_for_Yarn_Stock) = 1 Then

                    vENTDB_DelvToIDno = Val(Common_Procedures.get_FieldValue(con, "Yarn_Transfer_Head", "LedgerTo_IdNo", "(Yarn_Transfer_Code = '" & Trim(NewCode) & "')", , tr))

                    If Val(vENTDB_DelvToIDno) <> Val(led_id1) Then

                        cmd.CommandText = "Insert into TempTable_For_NegativeStock ( Stock_Type, Reference_Code, Reference_Date, Company_Idno, Ledger_Idno    , Count_IdNo, Yarn_Type, Mill_IdNo ) " &
                                            " Select                               'YARN'    , Reference_Code, Reference_Date, Company_IdNo, DeliveryTo_Idno, Count_IdNo, Yarn_Type, Mill_IdNo from Stock_Yarn_Processing_Details where Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
                        cmd.ExecuteNonQuery()

                    End If

                End If

                cmd.CommandText = "Update  Yarn_Transfer_Head set Yarn_Transfer_Date = @ReceiptDate, Ledger_IdNo = " & Val(led_id) & ",LedgerTo_IdNo = " & Val(led_id1) & ", Yarn_Typefrom = '" & Trim(cbo_TypeFrom.Text) & "',Yarn_Typeto='" & Trim(cbo_TypeTo.Text) & "', Mill_IdNofrom = " & Val(mil_id) & ",Mill_IdNoto=" & Val(mil_id1) & ",Count_IdNofrom=" & Val(cou_id) & ",Count_IdNoto=" & Val(cou_id1) & ", Yarn_Bags = " & Val(txt_bags.Text) & ",Yarn_Cones= " & Val(txt_cones.Text) & ",Yarn_Weight1= " & Val(txt_weightFrom.Text) & ",Yarn_Weight2= " & Val(txt_weightTo.Text) & ", Remarks = '" & Trim(txt_remarks.Text) & "' , User_idNo = " & Val(lbl_UserName.Text) & "   ,Weaving_JobCode_forSelection =  '" & Trim(cbo_weaving_job_no.Text) & "' ,Sizing_JobCode_forSelection = '" & Trim(cbo_Sizing_JobCardNo.Text) & "' , ClothSales_OrderCode_forSelection_From = '" & Trim(cbo_ClothSales_OrderCode_forSelection_From.Text) & "' , ClothSales_OrderCode_forSelection_To = '" & Trim(cbo_ClothSales_OrderCode_forSelection_To.Text) & "'  Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and  Yarn_Transfer_Code = '" & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()

            End If

            Call Common_Procedures.User_Modification_Updation(con, "HEAD", Me.Name, "NEW", "Yarn_Transfer_Head", "Yarn_Transfer_Code", Val(lbl_Company.Tag), NewCode, lbl_RefNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, False, "", "", "Yarn_Transfer_Code, Company_IdNo, for_OrderBy", tr)


            cmd.CommandText = "Delete from Stock_Yarn_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            EntID = Trim(Pk_Condition) & Trim(lbl_RefNo.Text)
            Partcls = "Transfer : Ref.No." & Trim(lbl_RefNo.Text) & " Remarks : " & Trim(txt_remarks.Text)
            PBlNo = Trim(lbl_RefNo.Text)

            Led_Frmtype = Common_Procedures.get_FieldValue(con, "Ledger_Head", "Ledger_Type", "(Ledger_IdNo = " & Str(Val(led_id)) & ")", , tr)
            Led_Totype = Common_Procedures.get_FieldValue(con, "Ledger_Head", "Ledger_Type", "(Ledger_IdNo = " & Str(Val(led_id1)) & ")", , tr)

            If Val(txt_weightFrom.Text) <> 0 Then

                Stk_DelvIdNo = 0 : Stk_RecIdNo = 0
                Prtcls_DelvIdNo = 0 : Prtcls_RecIdNo = 0
                If Trim(UCase(Led_Frmtype)) = "JOBWORKER" Then
                    Stk_DelvIdNo = led_id
                    Prtcls_RecIdNo = led_id1

                Else
                    Stk_RecIdNo = led_id
                    Prtcls_DelvIdNo = led_id1

                End If

                cmd.CommandText = "Insert into Stock_Yarn_Processing_Details (    Reference_Code            ,                 Company_IdNo     ,             Reference_No      ,                               for_OrderBy                              , Reference_Date,       DeliveryTo_Idno         ,      ReceivedFrom_Idno       ,        Entry_ID      ,     Party_Bill_No     , Sl_No,         Count_IdNo      ,               Yarn_Type          ,           Mill_IdNo     ,                 Bags           ,                 Cones           ,                 Weight               ,          Particulars   , DeliveryToIdno_ForParticulars    , ReceivedFromIdno_ForParticulars   , Weaving_JobCode_forSelection       , Sizing_JobCode_forSelection                   ,                    ClothSales_OrderCode_forSelection          ) " &
                                    "          Values          ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_RefNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text))) & ",  @ReceiptDate , " & Str(Val(Stk_DelvIdNo)) & ", " & Str(Val(Stk_RecIdNo)) & ", '" & Trim(EntID) & "',  '" & Trim(PBlNo) & "',    1 , " & Str(Val(cou_id)) & ", '" & Trim(cbo_TypeFrom.Text) & "', " & Str(Val(mil_id)) & ", " & Str(Val(txt_bags.Text)) & ", " & Str(Val(txt_cones.Text)) & ", " & Str(Val(txt_weightFrom.Text)) & ", '" & Trim(Partcls) & "', " & Str(Val(Prtcls_DelvIdNo)) & " ,  " & Str(Val(Prtcls_RecIdNo)) & " ,'" & Trim(cbo_weaving_job_no.Text) & "' ,'" & Trim(cbo_Sizing_JobCardNo.Text) & "' , '" & Trim(cbo_ClothSales_OrderCode_forSelection_From.Text) & "' ) "
                cmd.ExecuteNonQuery()

            End If

            If Val(txt_weightTo.Text) <> 0 Then

                Stk_DelvIdNo = 0 : Stk_RecIdNo = 0
                Prtcls_DelvIdNo = 0 : Prtcls_RecIdNo = 0
                If Trim(UCase(Led_Totype)) = "JOBWORKER" Then
                    Stk_RecIdNo = led_id1
                    Prtcls_DelvIdNo = led_id

                Else
                    Stk_DelvIdNo = led_id1
                    Prtcls_RecIdNo = led_id

                End If

                cmd.CommandText = "Insert into Stock_Yarn_Processing_Details (    Reference_Code            ,                 Company_IdNo     ,             Reference_No      ,                               for_OrderBy                              , Reference_Date,          DeliveryTo_Idno      ,         ReceivedFrom_Idno    ,        Entry_ID      ,      Party_Bill_No    , Sl_No,         Count_IdNo       ,               Yarn_Type        ,           Mill_IdNo      ,                 Bags           ,                 Cones           ,                 Weight             ,          Particulars   , DeliveryToIdno_ForParticulars    ,   ReceivedFromIdno_ForParticulars  , Weaving_JobCode_forSelection       , Sizing_JobCode_forSelection                     ,                ClothSales_OrderCode_forSelection               ) " &
                                    "          Values          ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_RefNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text))) & ",  @ReceiptDate , " & Str(Val(Stk_DelvIdNo)) & ", " & Str(Val(Stk_RecIdNo)) & ", '" & Trim(EntID) & "',  '" & Trim(PBlNo) & "',    2 , " & Str(Val(cou_id1)) & ", '" & Trim(cbo_TypeTo.Text) & "', " & Str(Val(mil_id1)) & ", " & Str(Val(txt_bags.Text)) & ", " & Str(Val(txt_cones.Text)) & ", " & Str(Val(txt_weightTo.Text)) & ", '" & Trim(Partcls) & "', " & Str(Val(Prtcls_DelvIdNo)) & " ,  " & Str(Val(Prtcls_RecIdNo)) & "   ,'" & Trim(cbo_weaving_job_no.Text) & "' ,'" & Trim(cbo_Sizing_JobCardNo.Text) & "'  ,  '" & Trim(cbo_ClothSales_OrderCode_forSelection_To.Text) & "' ) "
                cmd.ExecuteNonQuery()

            End If

            If Val(Common_Procedures.settings.Negative_Stock_Restriction_for_Yarn_Stock) = 1 Then

                cmd.CommandText = "Insert into TempTable_For_NegativeStock ( Stock_Type, Reference_Code, Reference_Date, Company_Idno, Ledger_Idno      , Count_IdNo, Yarn_Type, Mill_IdNo ) " &
                                          " Select                               'YARN'    , Reference_Code, Reference_Date, Company_IdNo, ReceivedFrom_Idno, Count_IdNo, Yarn_Type, Mill_IdNo from Stock_Yarn_Processing_Details where Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()

                If Common_Procedures.Check_is_Negative_Stock_Status(con, tr) = True Then Exit Sub

            End If

            tr.Commit()

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

        End Try

        If msk_date.Enabled And msk_date.Visible Then msk_date.Focus()

    End Sub

    Private Sub cbo_partyname_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_PartyFrom.KeyUp
        If e.Control = False And e.KeyValue = 17 Then
            Common_Procedures.UR.Ledr_Wea_Siz_Rw_Trans_JbWrk_Creation = Common_Procedures.UR.Ledger_Creation
            Common_Procedures.MDI_LedType = ""
            Dim f As New Ledger_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_PartyFrom.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If

    End Sub

    Private Sub btn_Save_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_save.Click
        save_record()
    End Sub

    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_close.Click
        Me.Close()
    End Sub


    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

    End Sub

    Private Sub txt_remarks_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_remarks.KeyDown
        If e.KeyCode = 40 Then
            If MessageBox.Show("Do you want to save ?", "FOR SAVING...", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                save_record()
            End If
        End If
        If e.KeyCode = 38 Then

            If cbo_ClothSales_OrderCode_forSelection_To.Visible = True Then
                cbo_ClothSales_OrderCode_forSelection_To.Focus()
            ElseIf cbo_Sizing_JobCardNo.Visible = True Then
                cbo_Sizing_JobCardNo.Focus()
            Else
                txt_weightTo.Focus()
            End If

        End If 'SendKeys.Send("+{TAB}")
        'If e.KeyCode = 38 Then msk_date.Focus()
    End Sub


    Private Sub txt_remarks_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_remarks.KeyPress
        If Asc(e.KeyChar) = 13 Then
            If MessageBox.Show("Do you want to save ?", "FOR SAVING...", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                save_record()
            End If
        End If
    End Sub

    Private Sub btn_closefilter_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_closefilter.Click
        pnl_back.Enabled = True
        pnl_filter.Visible = False
        Filter_Status = False

    End Sub

    Private Sub btn_filtershow_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_filtershow.Click
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim n As Integer
        Dim Led_IdNo As Integer
        'dim Itm_IdNo As Integer
        Dim Condt As String = ""

        Try

            Condt = ""
            Led_IdNo = 0
            ' Itm_IdNo = 0

            If IsDate(dtp_FilterFrom_date.Value) = True And IsDate(dtp_FilterTo_date.Value) = True Then
                Condt = "a.Yarn_Transfer_Date between '" & Trim(Format(dtp_FilterFrom_date.Value, "MM/dd/yyyy")) & "' and '" & Trim(Format(dtp_FilterTo_date.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_FilterFrom_date.Value) = True Then
                Condt = "a.Yarn_Transfer_Date = '" & Trim(Format(dtp_FilterFrom_date.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_FilterTo_date.Value) = True Then
                Condt = "a.Yarn_Transfer _Date= '" & Trim(Format(dtp_FilterTo_date.Value, "MM/dd/yyyy")) & "' "
            End If

            If Trim(cbo_PartyNameFilter.Text) <> "" Then
                Led_IdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_PartyNameFilter.Text)
            End If

            If Val(Led_IdNo) <> 0 Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " (a.Ledger_Idno = " & Str(Val(Led_IdNo)) & ")"
            End If

            da = New SqlClient.SqlDataAdapter("select a.*, b.Ledger_Name from Yarn_Transfer_Head a INNER JOIN Ledger_Head b ON a.Ledger_IdNo = b.Ledger_IdNo  where a.company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Yarn_Transfer_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "' " & IIf(Trim(Condt) <> "", " and ", "") & Condt & " Order by a.for_orderby, a.Yarn_Transfer_No", con)
            da.Fill(dt2)

            dgv_filter.Rows.Clear()

            If dt2.Rows.Count > 0 Then

                For i = 0 To dt2.Rows.Count - 1

                    n = dgv_filter.Rows.Add()

                    dgv_filter.Rows(n).Cells(0).Value = " " & dt2.Rows(i).Item("Yarn_Transfer_No").ToString
                    dgv_filter.Rows(n).Cells(1).Value = Format(Convert.ToDateTime(dt2.Rows(i).Item("Yarn_Transfer_Date").ToString), "dd-MM-yyyy")
                    dgv_filter.Rows(n).Cells(2).Value = dt2.Rows(i).Item("Ledger_Name").ToString
                    dgv_filter.Rows(n).Cells(3).Value = dt2.Rows(i).Item("Yarn_Weight1").ToString
                    dgv_filter.Rows(n).Cells(4).Value = dt2.Rows(i).Item("Yarn_Weight2").ToString

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


    Private Sub dgv_filter_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgv_filter.DoubleClick
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

    Private Sub txt_weight1_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_weightFrom.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True

    End Sub
    Private Sub txt_weight2_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_weightTo.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
            e.Handled = True
        End If



        If Asc(e.KeyChar) = 13 Then
            If cbo_weaving_job_no.Visible = True Then
                cbo_weaving_job_no.Focus()
            ElseIf cbo_Sizing_JobCardNo.Visible = True Then
                cbo_Sizing_JobCardNo.Focus()
            ElseIf cbo_ClothSales_OrderCode_forSelection_From.Visible = True Then
                cbo_ClothSales_OrderCode_forSelection_From.Focus()

            Else
                txt_remarks.Focus()
            End If

        End If


    End Sub

    Private Sub txt_bags_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_bags.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub txt_cones_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_cones.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub cbo_PartyFrom_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_PartyFrom.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "( (Ledger_Type = 'WEAVER' or Ledger_Type = 'GODOWN' or Ledger_Type = 'SIZING' or Ledger_Type = 'REWINDING' or Ledger_Type = 'JOBWORKER' or (Ledger_Type = '' and Stock_Maintenance_Status = 1) or Show_In_All_Entry = 1 )  and Close_status = 0 )", "(Ledger_idno = 0)")
    End Sub

    Private Sub cbo_PartyFrom_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_PartyFrom.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_PartyFrom, msk_date, cbo_PartyTo, "Ledger_AlaisHead", "Ledger_DisplayName", "( (Ledger_Type = 'WEAVER' or Ledger_Type = 'GODOWN' or Ledger_Type = 'SIZING' or Ledger_Type = 'REWINDING' or Ledger_Type = 'JOBWORKER' or (Ledger_Type = '' and Stock_Maintenance_Status = 1) or Show_In_All_Entry = 1 )  and Close_status = 0 )", "(Ledger_idno = 0)")
    End Sub

    Private Sub cbo_PartyFrom_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_PartyFrom.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_PartyFrom, cbo_PartyTo, "Ledger_AlaisHead", "Ledger_DisplayName", "( (Ledger_Type = 'WEAVER' or Ledger_Type = 'GODOWN' or Ledger_Type = 'SIZING' or Ledger_Type = 'REWINDING' or Ledger_Type = 'JOBWORKER' or (Ledger_Type = '' and Stock_Maintenance_Status = 1) or Show_In_All_Entry = 1 )  and Close_status = 0 )", "(Ledger_idno = 0)")
    End Sub

    Private Sub cbo_TypeFrom_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_TypeFrom.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "YarnType_Head", "Yarn_Type", "", "Yarn_Type")
    End Sub

    Private Sub cbo_TypeFrom_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_TypeFrom.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_TypeFrom, cbo_PartyTo, cbo_TypeTo, "YarnType_Head", "Yarn_Type", "", "Yarn_Type")

    End Sub

    Private Sub cbo_TypeFrom_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_TypeFrom.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_TypeFrom, cbo_TypeTo, "YarnType_Head", "Yarn_Type", "", "Yarn_Type")
    End Sub

    Private Sub cbo_TypeTo_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_TypeTo.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "YarnType_Head", "Yarn_Type", "", "Yarn_Type")

    End Sub

    Private Sub cbo_Typeto_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_TypeTo.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_TypeTo, cbo_TypeFrom, cbo_Millfrom, "YarnType_Head", "Yarn_Type", "", "Yarn_Type")

    End Sub

    Private Sub cbo_Typeto_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_TypeTo.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_TypeTo, cbo_Millfrom, "YarnType_Head", "Yarn_Type", "", "Yarn_Type")
    End Sub

    Private Sub cbo_Millfrom_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Millfrom.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Mill_Head", "Mill_Name", "", "(Mill_IdNo = 0)")

    End Sub

    Private Sub cbo_Millfrom_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Millfrom.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Millfrom, cbo_TypeTo, cbo_MillTo, "Mill_Head", "Mill_Name", "", "(Mill_IdNo = 0)")

    End Sub

    Private Sub cbo_Millfrom_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Millfrom.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Millfrom, cbo_MillTo, "Mill_Head", "Mill_Name", "", "(Mill_IdNo = 0)")

    End Sub

    Private Sub cbo_Millfrom_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Millfrom.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Dim f As New Mill_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Millfrom.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If
    End Sub

    Private Sub cbo_MillTo_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_MillTo.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Mill_Head", "Mill_Name", "", "(Mill_IdNo = 0)")

    End Sub
    Private Sub cbo_MillTo_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_MillTo.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_MillTo, cbo_Millfrom, cbo_Countfrom, "Mill_Head", "Mill_Name", "", "(Mill_IdNo = 0)")

    End Sub

    Private Sub cbo_MillTo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_MillTo.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_MillTo, cbo_Countfrom, "Mill_Head", "Mill_Name", "", "(Mill_IdNo = 0)")

    End Sub

    Private Sub cbo_MillTo_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_MillTo.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Dim f As New Mill_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_MillTo.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If
    End Sub

    Private Sub cbo_Countfrom_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Countfrom.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Count_Head", "Count_Name", "", "(Count_IdNo = 0)")

    End Sub

    Private Sub cbo_Countfrom_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Countfrom.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Countfrom, cbo_MillTo, cbo_CountTo, "Count_Head", "Count_Name", "", "(Count_IdNo = 0)")

    End Sub


    Private Sub cbo_Countfrom_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Countfrom.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Countfrom, cbo_CountTo, "Count_Head", "Count_Name", "", "(Count_IdNo = 0)")
    End Sub

    Private Sub cbo_Countfrom_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Countfrom.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Dim f As New Count_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Countfrom.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If
    End Sub

    Private Sub cbo_CountTo_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_CountTo.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Count_Head", "Count_Name", "", "(Count_IdNo = 0)")

    End Sub
    Private Sub cbo_CountTo_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_CountTo.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_CountTo, cbo_Countfrom, txt_bags, "Count_Head", "Count_Name", "", "(Count_IdNo = 0)")

    End Sub


    Private Sub cbo_CountTo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_CountTo.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_CountTo, txt_bags, "Count_Head", "Count_Name", "", "(Count_IdNo = 0)")
    End Sub

    Private Sub cbo_CountTo_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_CountTo.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Dim f As New Count_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_CountTo.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If
    End Sub

    Private Sub cbo_PartyNameFilter_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_PartyNameFilter.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "( (Ledger_Type = 'WEAVER' or Ledger_Type = 'GODOWN' or Ledger_Type = 'SIZING' or Ledger_Type = 'REWINDING' or Ledger_Type = 'JOBWORKER' or (Ledger_Type = '' and Stock_Maintenance_Status = 1) or Show_In_All_Entry = 1 )  and Close_status = 0 )", "(Ledger_idno = 0)")
    End Sub

    Private Sub cbo_PartyNameFilter_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_PartyNameFilter.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_PartyNameFilter, dtp_FilterTo_date, btn_filtershow, "Ledger_AlaisHead", "Ledger_DisplayName", "( (Ledger_Type = 'WEAVER' or Ledger_Type = 'GODOWN' or Ledger_Type = 'SIZING' or Ledger_Type = 'REWINDING' or Ledger_Type = 'JOBWORKER' or (Ledger_Type = '' and Stock_Maintenance_Status = 1) or Show_In_All_Entry = 1 )  and Close_status = 0 )", "(Ledger_idno = 0)")
    End Sub

    Private Sub cbo_PartyNameFilter_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_PartyFrom.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_PartyNameFilter, btn_filtershow, "Ledger_AlaisHead", "Ledger_DisplayName", "( (Ledger_Type = 'WEAVER' or Ledger_Type = 'GODOWN' or Ledger_Type = 'SIZING' or Ledger_Type = 'REWINDING' or Ledger_Type = 'JOBWORKER' or (Ledger_Type = '' and Stock_Maintenance_Status = 1) or Show_In_All_Entry = 1 )  and Close_status = 0 )", "(Ledger_idno = 0)")
    End Sub

    Public Sub print_record() Implements Interface_MDIActions.print_record
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim NewCode As String

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.PrintEntry, Common_Procedures.UR.Yarn_Transfer_Entry, New_Entry) = False Then Exit Sub

        Try

            da1 = New SqlClient.SqlDataAdapter("select * from Yarn_Transfer_Head Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Yarn_Transfer_Code = '" & Trim(NewCode) & "'", con)
            dt1 = New DataTable
            da1.Fill(dt1)

            If dt1.Rows.Count <= 0 Then

                MessageBox.Show("This is New Entry", "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Exit Sub

            End If

            dt1.Dispose()
            da1.Dispose()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        If Val(Common_Procedures.Print_OR_Preview_Status) = 1 Then

            Try

                If Val(Common_Procedures.settings.Printing_Show_PrintDialogue) = 1 Then
                    PrintDialog1.PrinterSettings = PrintDocument1.PrinterSettings
                    If PrintDialog1.ShowDialog() = Windows.Forms.DialogResult.OK Then
                        PrintDocument1.PrinterSettings = PrintDialog1.PrinterSettings
                        PrintDocument1.Print()
                    End If

                Else
                    PrintDocument1.Print()

                End If
            Catch ex As Exception
                MessageBox.Show("The printing operation failed" & vbCrLf & ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

            End Try


        Else
            Try

                Dim ppd As New PrintPreviewDialog

                ppd.Document = PrintDocument1

                ppd.WindowState = FormWindowState.Normal
                ppd.StartPosition = FormStartPosition.CenterScreen
                ppd.ClientSize = New Size(600, 600)

                ppd.ShowDialog()

            Catch ex As Exception
                MsgBox("The printing operation failed" & vbCrLf & ex.Message, MsgBoxStyle.Critical, "DOES NOT SHOW PRINT PREVIEW...")

            End Try

        End If

    End Sub

    Private Sub PrintDocument1_BeginPrint(ByVal sender As Object, ByVal e As System.Drawing.Printing.PrintEventArgs) Handles PrintDocument1.BeginPrint
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim NewCode As String

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        prn_HdDt.Clear()
        prn_DetIndx = 0
        prn_DetSNo = 0
        prn_DetDt.Clear()
        prn_DetIndx = 0
        prn_DetSNo = 0
        prn_PageNo = 0

        Try

            da1 = New SqlClient.SqlDataAdapter("select a.*, b.* , c.* from Yarn_Transfer_Head a INNER JOIN Company_Head b ON a.Company_IdNo = b.Company_IdNo INNER JOIN Ledger_Head c ON a.Ledger_IdNo = c.Ledger_IdNo  where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Yarn_Transfer_Code = '" & Trim(NewCode) & "'", con)
            prn_HdDt = New DataTable
            da1.Fill(prn_HdDt)

            If prn_HdDt.Rows.Count > 0 Then

                da2 = New SqlClient.SqlDataAdapter("select a.*, b.Count_name, c.Mill_name , d.Mill_Name As Mill_NameTo , e.Count_Name as CountTo  from Yarn_Transfer_Head a LEFT OUTER JOIN Count_Head b ON a.Count_IdNofrom = b.Count_idno LEFT OUTER JOIN Count_Head e ON a.Count_IdNoTo = e.Count_idno LEFT OUTER JOIN Mill_Head c ON a.Mill_IdNofrom = c.Mill_idno LEFT OUTER JOIN Mill_Head d ON a.Mill_IdNoTo = d.Mill_idno where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Yarn_Transfer_Code = '" & Trim(NewCode) & "'", con)
                prn_DetDt = New DataTable
                da2.Fill(prn_DetDt)

            Else
                MessageBox.Show("This is New Entry", "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

            End If

            da1.Dispose()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try



    End Sub

    Private Sub PrintDocument1_PrintPage(ByVal sender As Object, ByVal e As System.Drawing.Printing.PrintPageEventArgs) Handles PrintDocument1.PrintPage
        If prn_HdDt.Rows.Count <= 0 Then Exit Sub

        Printing_Format1(e)

    End Sub
    Private Sub Printing_Format1(ByRef e As System.Drawing.Printing.PrintPageEventArgs)
        Dim pFont As Font
        Dim p1Font As Font
        Dim ps As Printing.PaperSize
        Dim LMargin As Single, RMargin As Single, TMargin As Single, BMargin As Single
        Dim PrintWidth As Single, PrintHeight As Single
        Dim PageWidth As Single, PageHeight As Single
        Dim I As Integer
        Dim NoofItems_PerPage As Integer, NoofDets As Integer
        Dim TxtHgt As Single
        Dim PpSzSTS As Boolean = False
        Dim LnAr(15) As Single, ClAr(15) As Single
        Dim EntryCode As String
        Dim CurY As Single
        Dim d1, M1, W1 As Single


        p1Font = New Font("Calibri", 11, FontStyle.Bold)

        For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
            ps = PrintDocument1.PrinterSettings.PaperSizes(I)
            If ps.Width = 800 And ps.Height = 600 Then
                PrintDocument1.DefaultPageSettings.PaperSize = ps
                e.PageSettings.PaperSize = ps
                PpSzSTS = True
                Exit For
            End If
        Next

        If PpSzSTS = False Then

            For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
                If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.GermanStandardFanfold Then
                    ps = PrintDocument1.PrinterSettings.PaperSizes(I)
                    PrintDocument1.DefaultPageSettings.PaperSize = ps
                    e.PageSettings.PaperSize = ps
                    PpSzSTS = True
                    Exit For
                End If
            Next

            If PpSzSTS = False Then
                For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
                    If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
                        ps = PrintDocument1.PrinterSettings.PaperSizes(I)
                        PrintDocument1.DefaultPageSettings.PaperSize = ps
                        e.PageSettings.PaperSize = ps
                        Exit For
                    End If
                Next
            End If

        End If

        With PrintDocument1.DefaultPageSettings.Margins
            .Left = 30
            .Right = 30
            .Top = 30
            .Bottom = 30
            LMargin = .Left
            RMargin = .Right
            TMargin = .Top
            BMargin = .Bottom
        End With

        pFont = New Font("Calibri", 11, FontStyle.Regular)

        e.Graphics.TextRenderingHint = Drawing.Text.TextRenderingHint.AntiAlias

        With PrintDocument1.DefaultPageSettings.PaperSize
            PrintWidth = .Width - RMargin - LMargin
            PrintHeight = .Height - TMargin - BMargin
            PageWidth = .Width - RMargin
            PageHeight = .Height - BMargin
        End With
        If PrintDocument1.DefaultPageSettings.Landscape = True Then
            With PrintDocument1.DefaultPageSettings.PaperSize
                PrintWidth = .Height - TMargin - BMargin
                PrintHeight = .Width - RMargin - LMargin
                PageWidth = .Height - TMargin
                PageHeight = .Width - RMargin
            End With
        End If


        d1 = e.Graphics.MeasureString("Count From    : ", pFont).Width

        Erase LnAr
        Erase ClAr

        LnAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        ClAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}

        ClAr(1) = Val(35) : ClAr(2) = 225 : ClAr(3) = 180 : ClAr(4) = 100 : ClAr(5) = 120
        ClAr(6) = PageWidth - (LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5))

        M1 = ClAr(1) + ClAr(2) + ClAr(3)
        TxtHgt = 19

        W1 = e.Graphics.MeasureString("Count To    :  ", pFont).Width

        EntryCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Try
            If prn_HdDt.Rows.Count > 0 Then

                Printing_Format1_PageHeader(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, prn_PageNo, NoofItems_PerPage, CurY, LnAr, ClAr)

                NoofDets = NoofDets + 1

                CurY = CurY + TxtHgt + 10
                Common_Procedures.Print_To_PrintDocument(e, "Mill From", LMargin + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + d1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Mill_Name").ToString, LMargin + d1 + 30, CurY, 0, 0, p1Font)

                Common_Procedures.Print_To_PrintDocument(e, "Mill To", LMargin + M1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + M1 + W1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(0).Item("Mill_NameTo").ToString, LMargin + M1 + W1 + 25, CurY, 0, 0, pFont)



                CurY = CurY + TxtHgt + 10
                Common_Procedures.Print_To_PrintDocument(e, "Count From", LMargin + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + d1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Count_Name").ToString, LMargin + d1 + 30, CurY, 0, 0, p1Font)

                Common_Procedures.Print_To_PrintDocument(e, "Count To", LMargin + M1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + M1 + W1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(0).Item("CountTo").ToString, LMargin + M1 + W1 + 25, CurY, 0, 0, pFont)


                CurY = CurY + TxtHgt + 10
                Common_Procedures.Print_To_PrintDocument(e, "Type From", LMargin + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + d1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Yarn_Typefrom").ToString, LMargin + d1 + 30, CurY, 0, 0, p1Font)

                Common_Procedures.Print_To_PrintDocument(e, "Type To", LMargin + M1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + M1 + W1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(0).Item("Yarn_Typeto").ToString, LMargin + M1 + W1 + 25, CurY, 0, 0, pFont)


                CurY = CurY + TxtHgt + 10
                Common_Procedures.Print_To_PrintDocument(e, "Bags", LMargin + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + d1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Yarn_Bags").ToString, LMargin + d1 + 30, CurY, 0, 0, p1Font)

                Common_Procedures.Print_To_PrintDocument(e, "Cones", LMargin + M1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + M1 + W1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(0).Item("Yarn_Cones").ToString, LMargin + M1 + W1 + 25, CurY, 0, 0, pFont)


                CurY = CurY + TxtHgt + 10
                Common_Procedures.Print_To_PrintDocument(e, "Weight From ", LMargin + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + d1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Yarn_Weight1").ToString), "#######0.000"), LMargin + d1 + 30, CurY, 0, 0, p1Font)

                Common_Procedures.Print_To_PrintDocument(e, "Weight To", LMargin + M1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + M1 + W1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(0).Item("Yarn_Weight2").ToString), "#######0.000"), LMargin + M1 + W1 + 25, CurY, 0, 0, pFont)


                NoofDets = NoofDets + 1

                prn_DetIndx = prn_DetIndx + 1

                CurY = CurY + TxtHgt + 10

                e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

            End If

            Printing_Format1_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClAr, NoofDets, True)



        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        e.HasMorePages = False

    End Sub

    Private Sub Printing_Format1_PageHeader(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByRef PageNo As Integer, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single)
        Dim p1Font As Font
        Dim Cmp_Name As String, Cmp_Add1 As String, Cmp_Add2 As String
        Dim Cmp_PhNo As String, Cmp_TinNo As String, Cmp_CstNo As String
        Dim strHeight As Single
        Dim C1, N1, M1, W1 As Single

        PageNo = PageNo + 1

        CurY = TMargin


        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(1) = CurY

        Cmp_Name = "" : Cmp_Add1 = "" : Cmp_Add2 = ""
        Cmp_PhNo = "" : Cmp_TinNo = "" : Cmp_CstNo = ""

        Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString
        Cmp_Add1 = prn_HdDt.Rows(0).Item("Company_Address1").ToString & " " & prn_HdDt.Rows(0).Item("Company_Address2").ToString
        Cmp_Add2 = prn_HdDt.Rows(0).Item("Company_Address3").ToString & " " & prn_HdDt.Rows(0).Item("Company_Address4").ToString
        If Trim(prn_HdDt.Rows(0).Item("Company_PhoneNo").ToString) <> "" Then
            Cmp_PhNo = "PHONE NO.:" & prn_HdDt.Rows(0).Item("Company_PhoneNo").ToString
        End If
        If Trim(prn_HdDt.Rows(0).Item("Company_TinNo").ToString) <> "" Then
            Cmp_TinNo = "TIN NO.: " & prn_HdDt.Rows(0).Item("Company_TinNo").ToString
        End If
        If Trim(prn_HdDt.Rows(0).Item("Company_CstNo").ToString) <> "" Then
            Cmp_CstNo = "CST NO.: " & prn_HdDt.Rows(0).Item("Company_CstNo").ToString
        End If

        CurY = CurY + 5
        p1Font = New Font("Calibri", 16, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "YARN TRANSFER", LMargin, CurY, 2, PrintWidth, p1Font)

        CurY = CurY + TxtHgt + 10
        p1Font = New Font("Calibri", 18, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Name, LMargin, CurY, 2, PrintWidth, p1Font)
        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height

        CurY = CurY + strHeight - 1
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Add1, LMargin, CurY, 2, PrintWidth, pFont)

        CurY = CurY + TxtHgt - 1
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Add2, LMargin, CurY, 2, PrintWidth, pFont)
        CurY = CurY + TxtHgt - 1
        Common_Procedures.Print_To_PrintDocument(e, Cmp_PhNo, LMargin, CurY, 2, PrintWidth, pFont)
        CurY = CurY + TxtHgt - 1
        Common_Procedures.Print_To_PrintDocument(e, Cmp_TinNo, LMargin + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, Cmp_CstNo, PageWidth - 10, CurY, 1, 0, pFont)


        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height

        CurY = CurY + strHeight
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(2) = CurY

        C1 = ClAr(1) + ClAr(2) + ClAr(3)


        Try

            N1 = e.Graphics.MeasureString("TO   : ", pFont).Width
            W1 = e.Graphics.MeasureString("SET NO  :  ", pFont).Width

            M1 = ClAr(1) + ClAr(2) + ClAr(3)

            CurY = CurY + TxtHgt - 10
            p1Font = New Font("Calibri", 11, FontStyle.Bold)

            Common_Procedures.Print_To_PrintDocument(e, "TO :  M/s. " & prn_HdDt.Rows(0).Item("Ledger_Name").ToString, LMargin + 10, CurY, 0, 0, p1Font)
            Common_Procedures.Print_To_PrintDocument(e, "REF.NO", LMargin + M1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + M1 + W1 + 10, CurY, 0, 0, pFont)
            p1Font = New Font("Calibri", 14, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Yarn_Transfer_No").ToString, LMargin + M1 + W1 + 25, CurY, 0, 0, p1Font)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Ledger_Address1").ToString, LMargin + N1 + 10, CurY, 0, 0, pFont)


            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Ledger_Address2").ToString, LMargin + N1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "DATE", LMargin + M1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + M1 + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Yarn_Transfer_Date").ToString), "dd-MM-yyyy"), LMargin + M1 + W1 + 25, CurY, 0, 0, pFont)


            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Ledger_Address3").ToString, LMargin + N1 + 10, CurY, 0, 0, pFont)

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
        CurY = CurY + TxtHgt + 10

        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        e.Graphics.DrawLine(Pens.Black, LMargin + 380, CurY, LMargin + 380, LnAr(2))

    End Sub

    Private Sub Printing_Format1_PageFooter(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single, ByVal NoofDets As Integer, ByVal is_LastPage As Boolean)
        Dim i As Integer
        Dim Cmp_Name As String
        Dim p1Font As Font


        For I = NoofDets + 1 To NoofItems_PerPage
            CurY = CurY + TxtHgt
        Next

        CurY = CurY + TxtHgt
        CurY = CurY + TxtHgt
        CurY = CurY + TxtHgt

        Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString
        p1Font = New Font("Calibri", 12, FontStyle.Bold)

        Common_Procedures.Print_To_PrintDocument(e, "For " & Cmp_Name, PageWidth - 15, CurY, 1, 0, p1Font)

        CurY = CurY + TxtHgt + 10

        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

        e.Graphics.DrawLine(Pens.Black, LMargin, LnAr(1), LMargin, CurY)
        e.Graphics.DrawLine(Pens.Black, PageWidth, LnAr(1), PageWidth, CurY)

    End Sub

    Private Sub btn_Print_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Print.Click
        Common_Procedures.Print_OR_Preview_Status = 1
        print_record()
    End Sub

    Private Sub cbo_PartyTo_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_PartyTo.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "( (Ledger_Type = 'WEAVER' or Ledger_Type = 'GODOWN' or Ledger_Type = 'SIZING' or Ledger_Type = 'REWINDING' or Ledger_Type = 'JOBWORKER' or (Ledger_Type = '' and Stock_Maintenance_Status = 1) or Show_In_All_Entry = 1 )  and Close_status = 0 )", "(Ledger_idno = 0)")
    End Sub

    Private Sub cbo_PartyTo_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_PartyTo.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_PartyTo, cbo_PartyFrom, cbo_TypeFrom, "Ledger_AlaisHead", "Ledger_DisplayName", "( (Ledger_Type = 'WEAVER' or Ledger_Type = 'GODOWN' or Ledger_Type = 'SIZING' or Ledger_Type = 'REWINDING' or Ledger_Type = 'JOBWORKER' or (Ledger_Type = '' and Stock_Maintenance_Status = 1) or Show_In_All_Entry = 1 )  and Close_status = 0 )", "(Ledger_idno = 0)")
    End Sub

    Private Sub cbo_PartyTo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_PartyTo.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_PartyTo, cbo_TypeFrom, "Ledger_AlaisHead", "Ledger_DisplayName", "( (Ledger_Type = 'WEAVER' or Ledger_Type = 'GODOWN' or Ledger_Type = 'SIZING' or Ledger_Type = 'REWINDING' or Ledger_Type = 'JOBWORKER' or (Ledger_Type = '' and Stock_Maintenance_Status = 1) or Show_In_All_Entry = 1 )  and Close_status = 0 )", "(Ledger_idno = 0)")
    End Sub

    Private Sub cbo_PartyTo_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_PartyTo.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Common_Procedures.UR.Ledr_Wea_Siz_Rw_Trans_JbWrk_Creation = Common_Procedures.UR.Ledger_Creation
            Common_Procedures.MDI_LedType = ""
            Dim f As New Ledger_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_PartyTo.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If
    End Sub

    Private Sub msk_date_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles msk_date.KeyPress
        If Trim(UCase(e.KeyChar)) = "D" Then
            msk_date.Text = Date.Today
            msk_date.SelectionStart = 0
        End If
        If Asc(e.KeyChar) = 13 Then
            e.Handled = True
            cbo_PartyFrom.Focus()
        End If
    End Sub

    Private Sub msk_Date_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles msk_date.KeyUp
        Dim vmRetTxt As String = ""
        Dim vmRetSelStrt As Integer = -1

        If e.KeyCode = 107 Then
            msk_date.Text = DateAdd("D", 1, Convert.ToDateTime(msk_date.Text))
            msk_date.SelectionStart = 0
        ElseIf e.KeyCode = 109 Then
            msk_date.Text = DateAdd("D", -1, Convert.ToDateTime(msk_date.Text))
            msk_date.SelectionStart = 0
        End If

        If e.KeyCode = 46 Or e.KeyCode = 8 Then
            Common_Procedures.maskEdit_Date_ON_DelBackSpace(sender, e, vmskOldText, vmskSelStrt)
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

    Private Sub dtp_Date_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dtp_Date.KeyDown
        If e.KeyCode = 40 Then
            e.Handled = True : e.SuppressKeyPress = True
            msk_date.Focus()
        End If

        If e.KeyCode = 38 Then
            e.Handled = True : e.SuppressKeyPress = True
            msk_date.Focus()
        End If
    End Sub

    Private Sub dtp_Date_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dtp_Date.KeyPress
        If Asc(e.KeyChar) = 13 Then
            e.Handled = True
            msk_date.Focus()
        End If
    End Sub
    Private Sub dtp_Date_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dtp_Date.TextChanged
        If IsDate(dtp_Date.Text) = True Then
            msk_date.Text = dtp_Date.Text
            msk_date.SelectionStart = 0
        End If
    End Sub
    Private Sub msk_Date_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles msk_date.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        If e.KeyCode = 40 Then
            e.Handled = True : e.SuppressKeyPress = True
            cbo_PartyFrom.Focus()
        End If

        If e.KeyCode = 38 Then
            e.Handled = True : e.SuppressKeyPress = True
            txt_remarks.Focus()
        End If

        vmskOldText = ""
        vmskSelStrt = -1
        If e.KeyCode = 46 Or e.KeyCode = 8 Then
            vmskOldText = msk_date.Text
            vmskSelStrt = msk_date.SelectionStart
        End If
    End Sub

    Private Sub btn_UserModification_Click(sender As System.Object, e As System.EventArgs) Handles btn_UserModification.Click
        If Val(Common_Procedures.User.IdNo) = 1 Then
            Dim f1 As New User_Modifications
            f1.Entry_Name = Me.Name
            f1.Entry_PkValue = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)
            f1.ShowDialog()
        End If
    End Sub
    Private Sub cbo_weaving_job_no_KeyDown(sender As Object, e As KeyEventArgs) Handles cbo_weaving_job_no.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_weaving_job_no, txt_weightTo, Nothing, "Weaving_JobCard_Head", "Weaving_JobCode_forSelection", " ledger_idno = " & Val(vLed_ID_Cond) & "  ", "(Ledger_IdNo = 0)")

        If (e.KeyValue = 40 And cbo_Sizing_JobCardNo.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
            If cbo_Sizing_JobCardNo.Visible Then
                cbo_Sizing_JobCardNo.Focus()
            Else
                txt_remarks.Focus()
            End If

        End If
    End Sub
    Private Sub cbo_weaving_job_no_KeyPress(sender As Object, e As KeyPressEventArgs) Handles cbo_weaving_job_no.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_weaving_job_no, Nothing, "Weaving_JobCard_Head", "Weaving_JobCode_forSelection", " ledger_idno = " & Val(vLed_ID_Cond) & "  ", "(Ledger_IdNo = 0)")
        If asc(e.KeyChar)= 13 Then
            If cbo_Sizing_JobCardNo.Visible Then
                cbo_Sizing_JobCardNo.Focus()
            Else
                txt_remarks.Focus()
            End If

        End If
    End Sub
    Private Sub cbo_weaving_job_no_GotFocus(sender As Object, e As EventArgs) Handles cbo_weaving_job_no.GotFocus
        vLed_ID_Cond = 0
        If Trim(cbo_PartyFrom.Text) <> "" Then
            vLed_ID_Cond = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_PartyFrom.Text)
        End If

        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Weaving_JobCard_Head", "Weaving_JobCode_forSelection", " ledger_idno = " & Val(vLed_ID_Cond) & "  ", "(Ledger_IdNo = 0)")
    End Sub
    Private Sub cbo_Sizing_JobCardNo_GotFocus(sender As Object, e As EventArgs) Handles cbo_Sizing_JobCardNo.GotFocus
        vLed_ID_Cond = 0
        If Trim(cbo_PartyFrom.Text) <> "" Then
            vLed_ID_Cond = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_PartyFrom.Text)
        End If

        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Sizing_JobCard_Head", "Sizing_JobCode_forSelection", " ledger_idno = " & Val(vLed_ID_Cond) & " ", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_Sizing_JobCardNo_KeyPress(sender As Object, e As KeyPressEventArgs) Handles cbo_Sizing_JobCardNo.KeyPress

        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Sizing_JobCardNo, txt_remarks, "Sizing_JobCard_Head", "Sizing_JobCode_forSelection", " ledger_idno = " & Val(vLed_ID_Cond) & " ", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_Sizing_JobCardNo_KeyDown(sender As Object, e As KeyEventArgs) Handles cbo_Sizing_JobCardNo.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Sizing_JobCardNo, Nothing, Nothing, "Sizing_JobCard_Head", "Sizing_JobCode_forSelection", " ledger_idno = " & Val(vLed_ID_Cond) & " ", "(Ledger_IdNo = 0)")

        If (e.KeyValue = 38 And cbo_Sizing_JobCardNo.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
            If cbo_weaving_job_no.Visible Then
                cbo_weaving_job_no.Focus()
            Else
                txt_weightTo.Focus()
            End If

        End If

        If (e.KeyValue = 40 And cbo_Sizing_JobCardNo.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then

            If cbo_ClothSales_OrderCode_forSelection_From.Visible = True Then
                cbo_ClothSales_OrderCode_forSelection_From.Focus()
            Else
                txt_remarks.Focus()
            End If



        End If


    End Sub

    Private Sub txt_cones_TextChanged(sender As Object, e As EventArgs) Handles txt_cones.TextChanged

    End Sub

    Private Sub cbo_ClothSales_OrderCode_forSelection_From_KeyPress(sender As Object, e As KeyPressEventArgs) Handles cbo_ClothSales_OrderCode_forSelection_From.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, sender, cbo_ClothSales_OrderCode_forSelection_To, "ClothSales_Order_Head", "ClothSales_OrderCode_forSelection", vCLO_CONDT, "(ClothSales_Order_Code = '999999/00-00')")
    End Sub

    Private Sub cbo_ClothSales_OrderCode_forSelection_From_KeyDown(sender As Object, e As KeyEventArgs) Handles cbo_ClothSales_OrderCode_forSelection_From.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, sender, Nothing, cbo_ClothSales_OrderCode_forSelection_To, "ClothSales_Order_Head", "ClothSales_OrderCode_forSelection", vCLO_CONDT, "(ClothSales_Order_Code = '999999/00-00')")

        If (e.KeyValue = 38 And cbo_ClothSales_OrderCode_forSelection_From.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then

            If cbo_Sizing_JobCardNo.Visible = True Then
                cbo_Sizing_JobCardNo.Focus()
            Else
                txt_weightTo.Focus()
            End If
        End If
    End Sub

    Private Sub cbo_ClothSales_OrderCode_forSelection_From_Enter(sender As Object, e As EventArgs) Handles cbo_ClothSales_OrderCode_forSelection_From.Enter
        vCLO_CONDT = "(ClothSales_Order_Code LIKE '%/" & Trim(FnYearCode1) & "' or ClothSales_Order_Code LIKE '%/" & Trim(FnYearCode2) & "' and Order_Close_Status = 0 )"

        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "ClothSales_Order_Head", "ClothSales_OrderCode_forSelection", vCLO_CONDT, "(ClothSales_Order_Code = '999999/00-00')")
    End Sub

    Private Sub cbo_ClothSales_OrderCode_forSelection_To_KeyPress(sender As Object, e As KeyPressEventArgs) Handles cbo_ClothSales_OrderCode_forSelection_To.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, sender, txt_remarks, "ClothSales_Order_Head", "ClothSales_OrderCode_forSelection", vCLO_CONDT, "(ClothSales_Order_Code = '999999/00-00')")
    End Sub

    Private Sub cbo_ClothSales_OrderCode_forSelection_To_KeyDown(sender As Object, e As KeyEventArgs) Handles cbo_ClothSales_OrderCode_forSelection_To.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, sender, cbo_ClothSales_OrderCode_forSelection_From, txt_remarks, "ClothSales_Order_Head", "ClothSales_OrderCode_forSelection", vCLO_CONDT, "(ClothSales_Order_Code = '999999/00-00')")
    End Sub

    Private Sub cbo_ClothSales_OrderCode_forSelection_To_Enter(sender As Object, e As EventArgs) Handles cbo_ClothSales_OrderCode_forSelection_To.Enter
        vCLO_CONDT = "(ClothSales_Order_Code LIKE '%/" & Trim(FnYearCode1) & "' or ClothSales_Order_Code LIKE '%/" & Trim(FnYearCode2) & "' and Order_Close_Status = 0 )"

        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "ClothSales_Order_Head", "ClothSales_OrderCode_forSelection", vCLO_CONDT, "(ClothSales_Order_Code = '999999/00-00')")
    End Sub

    Private Sub txt_remarks_TextChanged(sender As Object, e As EventArgs) Handles txt_remarks.TextChanged

    End Sub

    Private Sub cbo_ClothSales_OrderCode_forSelection_From_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbo_ClothSales_OrderCode_forSelection_From.SelectedIndexChanged

    End Sub

    Private Sub cbo_Sizing_JobCardNo_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbo_Sizing_JobCardNo.SelectedIndexChanged

    End Sub

    Private Sub txt_weightTo_TextChanged(sender As Object, e As EventArgs) Handles txt_weightTo.TextChanged

    End Sub

    Private Sub txt_weightTo_KeyDown(sender As Object, e As KeyEventArgs) Handles txt_weightTo.KeyDown
        If e.KeyCode = 40 Then

            If cbo_weaving_job_no.Visible = True Then
                cbo_weaving_job_no.Focus()

            Else
                cbo_ClothSales_OrderCode_forSelection_From.Focus()
            End If

        End If

        If e.KeyCode = 38 Then
            txt_weightFrom.Focus()
        End If

    End Sub
End Class
