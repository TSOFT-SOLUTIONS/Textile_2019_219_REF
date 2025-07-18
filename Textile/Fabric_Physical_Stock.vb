Public Class Fabric_Physical_Stock
    Implements Interface_MDIActions

    Private con As New SqlClient.SqlConnection(Common_Procedures.Connection_String)
    Private con1 As New SqlClient.SqlConnection(Common_Procedures.ConnectionString_CompanyGroupdetails)
    Private FrmLdSTS As Boolean = False
    Private New_Entry As Boolean = False
    Private Insert_Entry As Boolean = False
    Private Filter_Status As Boolean = False
    Private Pk_Condition As String = "FABPS-"
    Private NoCalc_Status As Boolean = False
    Private Prec_ActCtrl As New Control
    Private vcbo_KeyDwnVal As Double
    Private vCbo_ItmNm As String

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

    Private dgvDet_CboBx_ColNos_Arr As Integer() = {1}


    Private Sub clear()

        NoCalc_Status = True


        chk_Verified_Status.Checked = False

        New_Entry = False
        Insert_Entry = False
        pnl_Selection.Visible = False
        pnl_Back.Enabled = True
        pnl_Filter.Visible = False

        lbl_DcNo.Text = ""
        lbl_DcNo.ForeColor = Color.Black

        msk_date.Text = ""
        msk_date.SelectionStart = 0
        dtp_Date.Text = ""
        cbo_Godown.Text = ""
        ' cbo_RecFrom.Text = Common_Procedures.Ledger_IdNoToName(con, 4)
        'cbo_TransportName.Text = ""
        'cbo_Grid_CountName.Text = ""
        'cbo_Grid_MillName.Text = ""
        'cbo_Grid_YarnType.Text = ""
        ' cbo_Vechile.Text = ""
        cbo_Grid_ClothName.Text = ""


        cbo_Filter_CountName.Text = ""
        cbo_Filter_MillName.Text = ""
        cbo_Filter_PartyName.Text = ""

        '  txt_Freight.Text = ""
        ' txt_Empty_Beam.Text = ""
        lbl_OrderCode.Text = ""
        ' lbl_OrderNo.Text = ""
        'txt_Party_DcNo.Text = ""
        lbl_UserName.Text = "USER : " & Trim(UCase(Common_Procedures.User_IdNoToName(con1, Common_Procedures.User.IdNo)))

        dgv_FabricDetails.Rows.Clear()
        dgv_FabricDetails_Total.Rows.Clear()
        dgv_FabricDetails_Total.Rows.Add()

        If Filter_Status = False Then
            dtp_Filter_Fromdate.Text = ""
            dtp_Filter_ToDate.Text = ""
            cbo_Filter_PartyName.Text = ""
            cbo_Filter_CountName.Text = ""
            cbo_Filter_PartyName.SelectedIndex = -1
            cbo_Filter_CountName.SelectedIndex = -1
            dgv_Filter_Details.Rows.Clear()
        End If

        Grid_Cell_DeSelect()

        'cbo_Grid_CountName.Visible = False
        'cbo_Grid_MillName.Visible = False
        'cbo_Grid_YarnType.Visible = False
        cbo_Grid_ClothName.Visible = False

        NoCalc_Status = False
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

        If Me.ActiveControl.Name <> cbo_Grid_ClothName.Name Then
            cbo_Grid_ClothName.Visible = False
        End If
        'If Me.ActiveControl.Name <> cbo_Grid_MillName.Name Then
        '    cbo_Grid_MillName.Visible = False
        'End If
        'If Me.ActiveControl.Name <> cbo_Grid_YarnType.Name Then
        '    cbo_Grid_YarnType.Visible = False
        'End If



        If Me.ActiveControl.Name <> dgv_FabricDetails_Total.Name Then
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
            End If
        End If

    End Sub

    Private Sub Grid_DeSelect()
        On Error Resume Next
        If Not IsNothing(dgv_FabricDetails.CurrentCell) Then dgv_FabricDetails.CurrentCell.Selected = False
        If Not IsNothing(dgv_FabricDetails_Total.CurrentCell) Then dgv_FabricDetails_Total.CurrentCell.Selected = False
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
        If Not IsNothing(dgv_FabricDetails.CurrentCell) Then dgv_FabricDetails.CurrentCell.Selected = False
        If Not IsNothing(dgv_FabricDetails_Total.CurrentCell) Then dgv_FabricDetails_Total.CurrentCell.Selected = False
        If Not IsNothing(dgv_Filter_Details.CurrentCell) Then dgv_Filter_Details.CurrentCell.Selected = False
    End Sub

    Private Sub Sizing_Yarn_Delivery_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated
        Dim da As SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim NoofComps As Integer
        Dim CompCondt As String

        Try

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Godown.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "LEDGER" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Godown.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If
            'If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_RecFrom.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "LEDGER" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
            '    cbo_RecFrom.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            'End If

            'If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_TransportName.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "TRANSPORT" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
            '    cbo_TransportName.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            'End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Grid_ClothName.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "YARN" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Grid_ClothName.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            'If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Grid_CountName.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "COUNT" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
            '    cbo_Grid_CountName.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            'End If

            'If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Grid_MillName.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "MILL" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
            '    cbo_Grid_MillName.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            'End If

            Common_Procedures.Master_Return.Form_Name = ""
            Common_Procedures.Master_Return.Control_Name = ""
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            If FrmLdSTS = True Then

                lbl_Company.Text = ""
                lbl_Company.Tag = 0
                Common_Procedures.CompIdNo = 0

                Me.Text = ""

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

                If Val(NoofComps) = 1 Then

                    da = New SqlClient.SqlDataAdapter("select Company_IdNo, Company_Name from Company_Head Where " & CompCondt & IIf(Trim(CompCondt) <> "", " and ", "") & " Company_IdNo <> 0 Order by Company_IdNo", con)
                    dt1 = New DataTable
                    da.Fill(dt1)

                    If dt1.Rows.Count > 0 Then
                        If IsDBNull(dt1.Rows(0)(0).ToString) = False Then
                            Common_Procedures.CompIdNo = Val(dt1.Rows(0)(0).ToString)
                        End If

                    End If
                    dt1.Clear()

                Else

                    Dim f As New Company_Selection
                    f.ShowDialog()

                End If

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

                    new_record()

                Else
                    MessageBox.Show("Invalid Company Selection", "DOES NOT SELECT...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    'Me.Close()
                    Exit Sub

                End If

            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "DOES NOT SHOW...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        FrmLdSTS = False
    End Sub

    Private Sub Sizing_Yarn_Delivery_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        On Error Resume Next
        con.Close()
        con.Dispose()
        Common_Procedures.Last_Closed_FormName = Me.Name
    End Sub

    Private Sub Sizing_Yarn_Delivery_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim dt3 As New DataTable
        Dim dt4 As New DataTable
        Dim dt5 As New DataTable
        Dim dt6 As New DataTable
        Dim dt7 As New DataTable
        Dim dt8 As New DataTable

        Me.Text = ""

        con.Open()

        da = New SqlClient.SqlDataAdapter("select Ledger_DisplayName from Ledger_AlaisHead where (Ledger_IdNo = 0 or Ledger_Type = 'SIZING' ) order by Ledger_DisplayName", con)
        da.Fill(dt1)
        cbo_Godown.DataSource = dt1
        cbo_Godown.DisplayMember = "Ledger_DisplayName"



        'da = New SqlClient.SqlDataAdapter("select Ledger_DisplayName from Ledger_AlaisHead where (Ledger_IdNo = 0  or Ledger_Type = 'GODOWN' or Ledger_Type = 'WEAVER' or Ledger_Type = 'REWINDING' or Ledger_Type = 'JOBWORKER' ) order by Ledger_DisplayName", con)
        'da.Fill(dt2)
        'cbo_RecFrom.DataSource = dt2
        'cbo_RecFrom.DisplayMember = "Ledger_DisplayName"

        'da = New SqlClient.SqlDataAdapter("select Ledger_DisplayName from Ledger_AlaisHead where (Ledger_IdNo = 0 or Ledger_Type = 'TRANSPORT') order by Ledger_DisplayName", con)
        'da.Fill(dt3)
        'cbo_TransportName.DataSource = dt3
        'cbo_TransportName.DisplayMember = "Ledger_DisplayName"


        'da = New SqlClient.SqlDataAdapter("select distinct(Vechile_No) from Fabric_Physical_Stock_Head order by Vechile_No", con)
        'da.Fill(dt7)
        'cbo_Vechile.DataSource = dt7
        'cbo_Vechile.DisplayMember = "Vechile_No"

        'da = New SqlClient.SqlDataAdapter("select mill_name from Mill_Head order by mill_name", con)
        'da.Fill(dt4)
        'cbo_Grid_MillName.DataSource = dt4
        'cbo_Grid_MillName.DisplayMember = "mill_name"

        'da = New SqlClient.SqlDataAdapter("select count_name from Count_Head order by count_name", con)
        'da.Fill(dt5)
        'cbo_Grid_CountName.DataSource = dt5
        'cbo_Grid_CountName.DisplayMember = "count_name"

        'da = New SqlClient.SqlDataAdapter("select yarn_type from YarnType_Head order by yarn_type", con)
        'da.Fill(dt6)
        'cbo_Grid_YarnType.DataSource = dt6
        'cbo_Grid_YarnType.DisplayMember = "yarn_type"


        'cbo_Grid_CountName.Visible = False
        'cbo_Grid_MillName.Visible = False
        'cbo_Grid_YarnType.Visible = False

        dtp_Date.Text = ""
        msk_date.Text = ""
        msk_date.SelectionStart = 0

        'btn_Selection.Visible = False
        'If Common_Procedures.settings.Internal_Order_Entry_Status = 1 Then
        '    btn_Selection.Visible = True
        'End If

        pnl_Filter.Visible = False
        pnl_Filter.Left = (Me.Width - pnl_Filter.Width) \ 2
        pnl_Filter.Top = (Me.Height - pnl_Filter.Height) \ 2


        pnl_Selection.Visible = False
        pnl_Selection.Left = (Me.Width - pnl_Selection.Width) \ 2
        pnl_Selection.Top = ((Me.Height - pnl_Selection.Height) \ 2)
        pnl_Selection.BringToFront()



        chk_Verified_Status.Visible = False
        If Common_Procedures.settings.Vefified_Status = 1 Then
            If Val(Common_Procedures.User.IdNo) = 1 Or Common_Procedures.User.Show_Verified_Status = 1 Then chk_Verified_Status.Visible = True
        End If

        dgv_FabricDetails.Columns(3).HeaderText = Trim(UCase(Common_Procedures.ClothType.Type1) & "/" & "METERS")
        dgv_FabricDetails.Columns(4).HeaderText = Trim(UCase(Common_Procedures.ClothType.Type2) & "/" & "METERS")
        dgv_FabricDetails.Columns(5).HeaderText = Trim(UCase(Common_Procedures.ClothType.Type3) & "/" & "METERS")
        dgv_FabricDetails.Columns(6).HeaderText = Trim(UCase(Common_Procedures.ClothType.Type4) & "/" & "METERS")
        dgv_FabricDetails.Columns(7).HeaderText = Trim(UCase(Common_Procedures.ClothType.Type5) & "/" & "METERS")

        dgv_FabricDetails.Columns(9).HeaderText = Trim(UCase(Common_Procedures.ClothType.Type1) & "/" & "BALES")
        dgv_FabricDetails.Columns(10).HeaderText = Trim(UCase(Common_Procedures.ClothType.Type2) & "/" & "BALES")
        dgv_FabricDetails.Columns(11).HeaderText = Trim(UCase(Common_Procedures.ClothType.Type3) & "/" & "BALES")
        dgv_FabricDetails.Columns(12).HeaderText = Trim(UCase(Common_Procedures.ClothType.Type4) & "/" & "BALES")
        dgv_FabricDetails.Columns(13).HeaderText = Trim(UCase(Common_Procedures.ClothType.Type5) & "/" & "BALES")


        AddHandler chk_Verified_Status.GotFocus, AddressOf ControlGotFocus
        AddHandler chk_Verified_Status.LostFocus, AddressOf ControlLostFocus


        AddHandler msk_date.GotFocus, AddressOf ControlGotFocus
        AddHandler dtp_Date.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Godown.GotFocus, AddressOf ControlGotFocus
        'AddHandler cbo_RecFrom.GotFocus, AddressOf ControlGotFocus
        'AddHandler cbo_TransportName.GotFocus, AddressOf ControlGotFocus
        'AddHandler cbo_Vechile.GotFocus, AddressOf ControlGotFocus
        'AddHandler cbo_Grid_CountName.GotFocus, AddressOf ControlGotFocus
        'AddHandler cbo_Grid_MillName.GotFocus, AddressOf ControlGotFocus
        'AddHandler cbo_Grid_YarnType.GotFocus, AddressOf ControlGotFocus
        'AddHandler txt_Empty_Beam.GotFocus, AddressOf ControlGotFocus
        'AddHandler txt_Freight.GotFocus, AddressOf ControlGotFocus
        'AddHandler txt_Party_DcNo.GotFocus, AddressOf ControlGotFocus

        AddHandler cbo_Filter_CountName.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Filter_MillName.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Filter_PartyName.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Grid_ClothName.GotFocus, AddressOf ControlGotFocus

        AddHandler msk_date.LostFocus, AddressOf ControlLostFocus
        AddHandler dtp_Date.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Godown.LostFocus, AddressOf ControlLostFocus
        'AddHandler cbo_RecFrom.LostFocus, AddressOf ControlLostFocus
        'AddHandler cbo_TransportName.LostFocus, AddressOf ControlLostFocus
        'AddHandler cbo_Vechile.LostFocus, AddressOf ControlLostFocus
        'AddHandler cbo_Grid_CountName.LostFocus, AddressOf ControlLostFocus
        'AddHandler cbo_Grid_MillName.LostFocus, AddressOf ControlLostFocus
        'AddHandler cbo_Grid_YarnType.LostFocus, AddressOf ControlLostFocus
        'AddHandler txt_Empty_Beam.LostFocus, AddressOf ControlLostFocus
        'AddHandler txt_Freight.LostFocus, AddressOf ControlLostFocus
        'AddHandler txt_Party_DcNo.LostFocus, AddressOf ControlLostFocus

        AddHandler cbo_Grid_ClothName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Filter_CountName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Filter_MillName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Filter_PartyName.LostFocus, AddressOf ControlLostFocus

        AddHandler msk_date.KeyDown, AddressOf TextBoxControlKeyDown
        'AddHandler txt_Empty_Beam.KeyDown, AddressOf TextBoxControlKeyDown
        'AddHandler txt_Party_DcNo.KeyDown, AddressOf TextBoxControlKeyDown

        AddHandler msk_date.KeyPress, AddressOf TextBoxControlKeyPress
        'AddHandler txt_Empty_Beam.KeyPress, AddressOf TextBoxControlKeyPress
        'AddHandler txt_Party_DcNo.KeyPress, AddressOf TextBoxControlKeyPress


        AddHandler dtp_Filter_Fromdate.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler dtp_Filter_ToDate.KeyDown, AddressOf TextBoxControlKeyDown

        lbl_Company.Text = ""
        lbl_Company.Tag = 0
        lbl_Company.Visible = False
        Common_Procedures.CompIdNo = 0

        Filter_Status = False
        FrmLdSTS = True
        new_record()
    End Sub
    Private Sub Sizing_Yarn_Delivery_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress

        Try
            If Asc(e.KeyChar) = 27 Then

                If pnl_Filter.Visible = True Then
                    btn_Filter_Close_Click(sender, e)
                    Exit Sub
                ElseIf pnl_Selection.Visible = True Then
                    btn_Close_Selection_Click(sender, e)
                    Exit Sub
                Else
                    Close_Form()

                End If

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT CLOSE...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Protected Overrides Function ProcessCmdKey(ByRef msg As System.Windows.Forms.Message, ByVal keyData As System.Windows.Forms.Keys) As Boolean
        Dim dgv1 As New DataGridView


        If ActiveControl.Name = dgv_FabricDetails.Name Or TypeOf ActiveControl Is DataGridViewTextBoxEditingControl Then

            On Error Resume Next

            dgv1 = Nothing

            If ActiveControl.Name = dgv_FabricDetails.Name Then
                dgv1 = dgv_FabricDetails

            ElseIf dgv_FabricDetails.IsCurrentRowDirty = True Then
                dgv1 = dgv_FabricDetails

            Else
                dgv1 = dgv_FabricDetails

            End If

            If IsNothing(dgv1) = False Then

                With dgv1

                    If keyData = Keys.Enter Or keyData = Keys.Down Or keyData = Keys.Up Then

                        Common_Procedures.DGVGrid_Cursor_Focus(dgv1, keyData, cbo_Godown, Nothing, dgvDet_CboBx_ColNos_Arr, dgtxt_Details, msk_date)

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
            da1 = New SqlClient.SqlDataAdapter("select a.* from Fabric_Physical_Stock_Head a Where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Fabric_Physical_Stock_Code = '" & Trim(NewCode) & "'", con)
            dt1 = New DataTable
            da1.Fill(dt1)

            If dt1.Rows.Count > 0 Then

                lbl_DcNo.Text = dt1.Rows(0).Item("Fabric_Physical_Stock_No").ToString
                dtp_Date.Text = dt1.Rows(0).Item("Fabric_Physical_Stock_Date").ToString
                msk_date.Text = dtp_Date.Text
                msk_date.SelectionStart = 0
                cbo_Godown.Text = Common_Procedures.Ledger_IdNoToName(con, Val(dt1.Rows(0).Item("Godown").ToString))


                'lbl_UserName.Text = "USER : " & Trim(UCase(Common_Procedures.User_IdNoToName(con1, Val(dt1.Rows(0).Item("User_IdNo").ToString))))
                '' lbl_OrderNo.Text = dt1.Rows(0).Item("Our_Order_No").ToString
                'lbl_OrderCode.Text = dt1.Rows(0).Item("Own_Order_Code").ToString



                'If Val(dt1.Rows(0).Item("Verified_Status").ToString) = 1 Then chk_Verified_Status.Checked = True


                da2 = New SqlClient.SqlDataAdapter("select a.*, b.Cloth_Name from Fabric_Physical_Stock_Details a INNER JOIN Cloth_Head b on a.Cloth_IdNo = b.Cloth_IdNo  where a.Fabric_Physical_Stock_Code = '" & Trim(NewCode) & "' Order by a.sl_no", con)
                dt2 = New DataTable
                da2.Fill(dt2)

                dgv_FabricDetails.Rows.Clear()
                SNo = 0

                If dt2.Rows.Count > 0 Then

                    For i = 0 To dt2.Rows.Count - 1

                        n = dgv_FabricDetails.Rows.Add()

                        SNo = SNo + 1
                        dgv_FabricDetails.Rows(n).Cells(0).Value = Val(SNo)
                        dgv_FabricDetails.Rows(n).Cells(1).Value = dt2.Rows(i).Item("Cloth_Name").ToString
                        'If Val(dt2.Rows(i).Item("Total_Uncheck_Meters").ToString) <> 0 Then
                        '    .Rows(n).Cells(2).Value = Format(Val(dt2.Rows(i).Item("Total_Uncheck_Meters").ToString), "########0.00")
                        'End If
                        dgv_FabricDetails.Rows(n).Cells(2).Value = Format(Val(dt2.Rows(i).Item("Total_Uncheck_Meters").ToString), "########0.00")
                        dgv_FabricDetails.Rows(n).Cells(3).Value = Val(dt2.Rows(i).Item("Type1_Pcs_Meters").ToString)
                        dgv_FabricDetails.Rows(n).Cells(4).Value = Val(dt2.Rows(i).Item("Type2_Pcs_Meters").ToString)
                        dgv_FabricDetails.Rows(n).Cells(5).Value = Val(dt2.Rows(i).Item("Type3_Pcs_Meters").ToString)
                        dgv_FabricDetails.Rows(n).Cells(6).Value = Val(dt2.Rows(i).Item("Type4_Pcs_Meters").ToString)
                        dgv_FabricDetails.Rows(n).Cells(7).Value = Val(dt2.Rows(i).Item("Type5_Pcs_Meters").ToString)
                        dgv_FabricDetails.Rows(n).Cells(8).Value = Format(Val(dt2.Rows(i).Item("Total_Pcs_Meters").ToString), "########0.00")
                        dgv_FabricDetails.Rows(n).Cells(9).Value = Val(dt2.Rows(i).Item("Type1_Bales_Meters").ToString)
                        dgv_FabricDetails.Rows(n).Cells(10).Value = Val(dt2.Rows(i).Item("Type2_Bales_Meters").ToString)
                        dgv_FabricDetails.Rows(n).Cells(11).Value = Val(dt2.Rows(i).Item("Type3_Bales_Meters").ToString)
                        dgv_FabricDetails.Rows(n).Cells(12).Value = Val(dt2.Rows(i).Item("Type4_Bales_Meters").ToString)
                        dgv_FabricDetails.Rows(n).Cells(13).Value = Val(dt2.Rows(i).Item("Type5_Bales_Meters").ToString)
                        dgv_FabricDetails.Rows(n).Cells(14).Value = Format(Val(dt2.Rows(i).Item("Total_Bales_Meters").ToString), "########0.00")
                        dgv_FabricDetails.Rows(n).Cells(15).Value = Format(Val(dt2.Rows(i).Item("Total_Fabric_Meters").ToString), "########0.00")
                    Next i

                End If

                With dgv_FabricDetails_Total
                    If .RowCount = 0 Then .Rows.Add()
                    .Rows(0).Cells(2).Value = Format(Val(dt1.Rows(0).Item("Total_Uncheck_Meters").ToString), "########0.00")
                    .Rows(0).Cells(8).Value = Format(Val(dt1.Rows(0).Item("Total_Pcs_Meters").ToString), "########0.00")
                    .Rows(0).Cells(14).Value = Format(Val(dt1.Rows(0).Item("Total_Bales_Meters").ToString), "########0.00")
                    .Rows(0).Cells(15).Value = Format(Val(dt1.Rows(0).Item("Total_Fabric_Meters").ToString), "########0.00")

                    .Rows(0).Cells(3).Value = Format(Val(dt1.Rows(0).Item("Total_Type1_Pcs_Meters").ToString), "########0.00")
                    .Rows(0).Cells(4).Value = Format(Val(dt1.Rows(0).Item("Total_Type2_Pcs_Meters").ToString), "########0.00")
                    .Rows(0).Cells(5).Value = Format(Val(dt1.Rows(0).Item("Total_Type3_Pcs_Meters").ToString), "########0.00")
                    .Rows(0).Cells(6).Value = Format(Val(dt1.Rows(0).Item("Total_Type4_Pcs_Meters").ToString), "########0.00")
                    .Rows(0).Cells(7).Value = Format(Val(dt1.Rows(0).Item("Total_Type5_Pcs_Meters").ToString), "########0.00")

                    .Rows(0).Cells(9).Value = Format(Val(dt1.Rows(0).Item("Total_Type1_Bales_Meters").ToString), "########0.00")
                    .Rows(0).Cells(10).Value = Format(Val(dt1.Rows(0).Item("Total_Type2_Bales_Meters").ToString), "########0.00")
                    .Rows(0).Cells(11).Value = Format(Val(dt1.Rows(0).Item("Total_Type3_Bales_Meters").ToString), "########0.00")
                    .Rows(0).Cells(12).Value = Format(Val(dt1.Rows(0).Item("Total_Type4_Bales_Meters").ToString), "########0.00")
                    .Rows(0).Cells(13).Value = Format(Val(dt1.Rows(0).Item("Total_Type5_Bales_Meters").ToString), "########0.00")


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

        If msk_date.Visible And msk_date.Enabled Then msk_date.Focus()

    End Sub

    Public Sub delete_record() Implements Interface_MDIActions.delete_record
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim cmd As New SqlClient.SqlCommand
        Dim trans As SqlClient.SqlTransaction
        Dim NewCode As String = ""

        '  If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.fabric_physical_stock_Entry, "~L~") = 0 And InStr(Common_Procedures.UR.fabric_physical_stock_Entry, "~D~") = 0 Then MessageBox.Show("You have No Rights to Delete", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error) : Exit Sub

        If Common_Procedures.settings.Vefified_Status = 1 Then
            NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            If Val(Common_Procedures.get_FieldValue(con, "Fabric_Physical_Stock_Head", "Verified_Status", "(Fabric_Physical_Stock_Code = '" & Trim(NewCode) & "')")) = 1 Then
                MessageBox.Show("Entry Already Verified", "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                Exit Sub
            End If
        End If


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

            NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            cmd.Connection = con
            cmd.Transaction = trans

            Common_Procedures.Voucher_Deletion(con, Val(lbl_Company.Tag), Trim(Pk_Condition) & Trim(NewCode), trans)

            cmd.CommandText = "Delete from Stock_Yarn_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Stock_Empty_BeamBagCone_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "delete from Fabric_Physical_Stock_Details where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Fabric_Physical_Stock_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()
            cmd.CommandText = "delete from Fabric_Physical_Stock_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Fabric_Physical_Stock_Code = '" & Trim(NewCode) & "'"
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

            If msk_date.Enabled = True And msk_date.Visible = True Then msk_date.Focus()

        End Try

    End Sub

    Public Sub filter_record() Implements Interface_MDIActions.filter_record

        If Filter_Status = False Then

            Dim da As New SqlClient.SqlDataAdapter
            Dim dt1 As New DataTable
            Dim dt2 As New DataTable
            Dim dt3 As New DataTable

            da = New SqlClient.SqlDataAdapter("select Ledger_DisplayName from Ledger_AlaisHead where (Ledger_IdNo = 0 or Ledger_Type = 'SIZING') order by Ledger_DisplayName", con)
            da.Fill(dt1)
            cbo_Filter_PartyName.DataSource = dt1
            cbo_Filter_PartyName.DisplayMember = "Ledger_DisplayName"

            da = New SqlClient.SqlDataAdapter("select count_name from count_head order by count_name", con)
            da.Fill(dt2)
            cbo_Filter_CountName.DataSource = dt2
            cbo_Filter_CountName.DisplayMember = "count_name"

            da = New SqlClient.SqlDataAdapter("select Mill_name from Mill_head order by Mill_name", con)
            da.Fill(dt2)
            cbo_Filter_MillName.DataSource = dt2
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
        pnl_Filter.BringToFront()
        Pnl_Back.Enabled = False
        If dtp_Filter_Fromdate.Enabled And dtp_Filter_Fromdate.Visible Then dtp_Filter_Fromdate.Focus()

    End Sub

    Public Sub movefirst_record() Implements Interface_MDIActions.movefirst_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim movno As String

        Try

            da = New SqlClient.SqlDataAdapter("select top 1 Fabric_Physical_Stock_No from Fabric_Physical_Stock_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Fabric_Physical_Stock_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby, Fabric_Physical_Stock_No", con)
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

            OrdByNo = Common_Procedures.OrderBy_CodeToValue(Trim(lbl_DcNo.Text))

            da = New SqlClient.SqlDataAdapter("select top 1 Fabric_Physical_Stock_No from Fabric_Physical_Stock_Head where for_orderby > " & Str(Val(OrdByNo)) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and Fabric_Physical_Stock_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby, Fabric_Physical_Stock_No", con)
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

            OrdByNo = Common_Procedures.OrderBy_CodeToValue(Trim(lbl_DcNo.Text))

            da = New SqlClient.SqlDataAdapter("select top 1 Fabric_Physical_Stock_No from Fabric_Physical_Stock_Head where for_orderby < " & Str(Val(OrdByNo)) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and Fabric_Physical_Stock_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, Fabric_Physical_Stock_No desc", con)
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
            da = New SqlClient.SqlDataAdapter("select top 1 Fabric_Physical_Stock_No from Fabric_Physical_Stock_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Fabric_Physical_Stock_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, Fabric_Physical_Stock_No desc", con)
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
        Dim dt1 As New DataTable
        Dim NewID As Integer = 0

        Try
            clear()

            New_Entry = True

            lbl_DcNo.Text = Common_Procedures.get_MaxCode(con, "Fabric_Physical_Stock_Head", "Fabric_Physical_Stock_Code", "For_OrderBy", "", Val(lbl_Company.Tag), Common_Procedures.FnYearCode)

            lbl_DcNo.ForeColor = Color.Red

            msk_date.Text = Date.Today.ToShortDateString
            msk_date.SelectionStart = 0
            da = New SqlClient.SqlDataAdapter("select top 1 * from Fabric_Physical_Stock_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Fabric_Physical_Stock_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, Fabric_Physical_Stock_No desc", con)
            dt1 = New DataTable
            da.Fill(dt1)
            If dt1.Rows.Count > 0 Then
                If Val(Common_Procedures.settings.PreviousEntryDate_ByDefault) = 1 Then '---- M.S Textiles (Tirupur)
                    If dt1.Rows(0).Item("Fabric_Physical_Stock_Date").ToString <> "" Then msk_date.Text = dt1.Rows(0).Item("Fabric_Physical_Stock_Date").ToString
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
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim movno As String, inpno As String
        Dim RecCode As String

        Try

            inpno = InputBox("Enter Dc.No.", "FOR FINDING...")

            RecCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

            Da = New SqlClient.SqlDataAdapter("select Fabric_Physical_Stock_No from Fabric_Physical_Stock_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Fabric_Physical_Stock_Code = '" & Trim(RecCode) & "'", con)
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
                MessageBox.Show("Dc No. does not exists", "DOES NOT FIND...", MessageBoxButtons.OK, MessageBoxIcon.Error)

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

        ' If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.fabric_physical_stock_Entry, "~L~") = 0 And InStr(Common_Procedures.UR.fabric_physical_stock_Entry, "~I~") = 0 Then MessageBox.Show("You have No Rights to Insert", "DOES NOT INSERT...", MessageBoxButtons.OK, MessageBoxIcon.Error) : Exit Sub

        Try

            inpno = InputBox("Enter New Dc No.", "FOR NEW RECEIPT INSERTION...")

            RecCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

            Da = New SqlClient.SqlDataAdapter("select Fabric_Physical_Stock_No from Fabric_Physical_Stock_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Fabric_Physical_Stock_Code = '" & Trim(RecCode) & "'", con)
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
                    MessageBox.Show("Invalid Dc No", "DOES NOT INSERT NEW RECEIPT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

                Else
                    new_record()
                    Insert_Entry = True
                    lbl_DcNo.Text = Trim(UCase(inpno))

                End If

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT INSERT NEW RECEIPT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Public Sub save_record() Implements Interface_MDIActions.save_record
        Dim cmd As New SqlClient.SqlCommand
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim tr As SqlClient.SqlTransaction
        Dim NewCode As String = ""
        Dim Gn_ID As Integer = 0
        Dim Rec_ID As Integer = 0
        Dim Trans_ID As Integer = 0
        Dim Clo_ID As Integer = 0
        Dim EdsCnt_ID As Integer = 0
        Dim Sno As Integer = 0
        Dim Partcls As String = ""
        Dim PBlNo As String = ""
        Dim YCnt_ID As Integer = 0
        Dim cloth_ID As Integer = 0
        Dim YMil_ID As Integer = 0
        'Dim vTotYrnBags As Single, vTotYrnCones As Single, vTotYrnWeight As Single
        Dim vTotUnchkMts As Single, vTotPcs As Single, vTotBales As Single, vTotFabMts As Single
        Dim EntID As String = ""
        Dim Usr_ID As Integer = 0
        Dim vType1_Pcs As Single, vType2_Pcs As Single, vType3_Pcs As Single, vType4_Pcs As Single, vType5_Pcs As Single
        Dim vType1_bales As Single, vType2_bales As Single, vType3_bales As Single, vType4_bales As Single, vType5_bales As Single

        Dim OurOrd_No As String = ""







        Dim Verified_STS As String = ""



        If Val(lbl_Company.Tag) = 0 Then
            MessageBox.Show("Invalid Company Selection", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If
        'If Common_Procedures.UserRight_Check(Common_Procedures.UR.Fabric_physical_stock_entry, New_Entry) = False Then Exit Sub


        'If Common_Procedures.settings.Vefified_Status = 1 Then
        '    If Not (Val(Common_Procedures.User.IdNo) = 1 Or Common_Procedures.User.Show_Verified_Status = 1) Then
        '        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        '        If Val(Common_Procedures.get_FieldValue(con, " Fabric_Physical_Stock_Head", "Verified_Status", "( Fabric_Physical_Stock_Code = '" & Trim(NewCode) & "')")) = 1 Then
        '            MessageBox.Show("Entry Already Verified", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
        '            Exit Sub
        '        End If
        '    End If
        'End If


        If Pnl_Back.Enabled = False Then
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

        Gn_ID = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Godown.Text)
        If Gn_ID = 0 Then
            MessageBox.Show("Invalid Godown Name", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If cbo_Godown.Enabled And cbo_Godown.Visible Then cbo_Godown.Focus()
            Exit Sub
        End If

        '  Rec_ID = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_RecFrom.Text)
        'If Rec_ID = 0 Then Rec_ID = 4

        'If Delv_ID = Rec_ID Then
        '    MessageBox.Show("Invalid Party Name, Does not accept same name", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
        '    If cbo_Godown.Enabled And cbo_Godown.Visible Then cbo_Godown.Focus()
        '    Exit Sub
        'End If
        'If Trim(lbl_OrderCode.Text) <> "" Then


        '    If Delv_ID <> 0 Then

        '        Da = New SqlClient.SqlDataAdapter("select a.*,b.* from Own_Order_Head a INNER JOIN Own_order_Sizing_Details b ON a.Own_Order_Code =b.Own_Order_Code where a.Own_Order_Code = '" & Trim(lbl_OrderCode.Text) & "' and  b.Ledger_idno = " & Str(Val(Delv_ID)), con)
        '        Dt1 = New DataTable
        '        Da.Fill(Dt1)
        '        If Dt1.Rows.Count > 0 Then
        '            OurOrd_No = Dt1.Rows(0).Item("Order_No").ToString

        '        End If
        '    End If
        'If Trim(OurOrd_No) <> Trim(lbl_OrderNo.Text) Then
        '    MessageBox.Show("Invalid Mismatch Of Order No for this Party Name", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
        '    If cbo_Godown.Enabled And cbo_Godown.Visible Then cbo_Godown.Focus()
        '    Exit Sub
        'End If
        ' End If

        'If Trim(txt_Party_DcNo.Text) <> "" Then
        '    NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)
        '    Da = New SqlClient.SqlDataAdapter("select * from Fabric_Physical_Stock_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and DeliveryTo_IdNo = " & Str(Val(Delv_ID)) & " and Party_dcno = '" & Trim(txt_Party_DcNo.Text) & "' and Fabric_Physical_Stock_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "' and Fabric_Physical_Stock_Code <> '" & Trim(NewCode) & "'", con)
        '    Dt1 = New DataTable
        '    Da.Fill(Dt1)
        '    If Dt1.Rows.Count > 0 Then
        '        MessageBox.Show("Duplicate Party Dc No to this Party", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
        '        If txt_Party_DcNo.Enabled And txt_Party_DcNo.Visible Then txt_Party_DcNo.Focus()
        '        Exit Sub
        '    End If
        '    Dt1.Clear()
        'End If
        ' Trans_ID = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_TransportName.Text)
        lbl_UserName.Text = Common_Procedures.User.IdNo
        For i = 0 To dgv_FabricDetails.RowCount - 1

            If Val(dgv_FabricDetails.Rows(i).Cells(15).Value) <> 0 Then

                cloth_ID = Common_Procedures.Cloth_NameToIdNo(con, dgv_FabricDetails.Rows(i).Cells(1).Value)
                If Val(cloth_ID) = 0 Then
                    MessageBox.Show("Invalid ClothName", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    dgv_FabricDetails.CurrentCell = dgv_FabricDetails.Rows(0).Cells(1)
                    dgv_FabricDetails.Focus()
                    Exit Sub
                End If

            End If

        Next

        Verified_STS = 0
        If chk_Verified_Status.Checked = True Then Verified_STS = 1

    
        vTotUnchkMts = 0 : vTotPcs = 0 : vTotBales = 0 : vTotFabMts = 0
        vType1_Pcs = 0 : vType2_Pcs = 0 : vType3_Pcs = 0 : vType4_Pcs = 0 : vType5_Pcs = 0
        vType1_bales = 0 : vType2_bales = 0 : vType3_bales = 0 : vType4_bales = 0 : vType5_bales = 0

        If dgv_FabricDetails_Total.RowCount > 0 Then
            vTotUnchkMts = Val(dgv_FabricDetails_Total.Rows(0).Cells(2).Value())
            vTotPcs = Val(dgv_FabricDetails_Total.Rows(0).Cells(8).Value())
            vTotBales = Val(dgv_FabricDetails_Total.Rows(0).Cells(14).Value())
            vTotFabMts = Val(dgv_FabricDetails_Total.Rows(0).Cells(15).Value())

            vType1_Pcs = Val(dgv_FabricDetails_Total.Rows(0).Cells(3).Value())
            vType2_Pcs = Val(dgv_FabricDetails_Total.Rows(0).Cells(4).Value())
            vType3_Pcs = Val(dgv_FabricDetails_Total.Rows(0).Cells(5).Value())
            vType4_Pcs = Val(dgv_FabricDetails_Total.Rows(0).Cells(6).Value())
            vType5_Pcs = Val(dgv_FabricDetails_Total.Rows(0).Cells(7).Value())

            vType1_bales = Val(dgv_FabricDetails_Total.Rows(0).Cells(8).Value())
            vType2_bales = Val(dgv_FabricDetails_Total.Rows(0).Cells(9).Value())
            vType3_bales = Val(dgv_FabricDetails_Total.Rows(0).Cells(10).Value())
            vType4_bales = Val(dgv_FabricDetails_Total.Rows(0).Cells(11).Value())
            vType5_bales = Val(dgv_FabricDetails_Total.Rows(0).Cells(12).Value())

        End If

        tr = con.BeginTransaction

        Try

            If Insert_Entry = True Or New_Entry = False Then
                NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            Else

                lbl_DcNo.Text = Common_Procedures.get_MaxCode(con, "Fabric_Physical_Stock_Head", "Fabric_Physical_Stock_Code", "For_OrderBy", "", Val(lbl_Company.Tag), Common_Procedures.FnYearCode, tr)

                NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            End If

            cmd.Connection = con
            cmd.Transaction = tr

            cmd.Parameters.Clear()
            cmd.Parameters.AddWithValue("@DcDate", Convert.ToDateTime(msk_date.Text))

            If New_Entry = True Then


                cmd.CommandText = "Insert into Fabric_Physical_Stock_Head(Fabric_Physical_Stock_Code,      Company_IdNo,                  Fabric_Physical_Stock_No,                    for_OrderBy,                                      Fabric_Physical_Stock_Date,         Godown,                  Total_Uncheck_Meters,       Total_Pcs_Meters,         Total_Bales_Meters ,          Total_Fabric_Meters    ,              Total_Type1_Pcs_Meters,    Total_Type2_Pcs_Meters,       Total_Type3_Pcs_Meters,        Total_Type4_Pcs_Meters,       Total_Type5_Pcs_Meters,             Total_Type1_Bales_Meters,    Total_Type2_Bales_Meters,      Total_Type3_Bales_Meters,    Total_Type4_Bales_Meters,         Total_Type5_Bales_Meters) " & _
                                                               " Values ('" & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_DcNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_DcNo.Text))) & ",      @DcDate,            " & Str(Val(Gn_ID)) & ",  " & Str(Val(vTotUnchkMts)) & ", " & Str(Val(vTotPcs)) & ", " & Str(Val(vTotBales)) & " ,  " & Str(Val(vTotFabMts)) & ",   " & Str(Val(vType1_Pcs)) & ",  " & Str(Val(vType2_Pcs)) & ",  " & Str(Val(vType3_Pcs)) & ",  " & Str(Val(vType4_Pcs)) & ", " & Str(Val(vType5_Pcs)) & ",     " & Str(Val(vType1_bales)) & ",     " & Str(Val(vType2_bales)) & ",   " & Str(Val(vType3_bales)) & ",   " & Str(Val(vType4_bales)) & ",  " & Str(Val(vType5_bales)) & " )"
                cmd.ExecuteNonQuery()

            Else
                cmd.CommandText = "Update Fabric_Physical_Stock_Head set Fabric_Physical_Stock_Date = @DcDate, Godown = " & Str(Val(Gn_ID)) & " ,  Total_Uncheck_Meters =  " & Str(Val(vTotUnchkMts)) & ", Total_Pcs_Meters = " & Str(Val(vTotPcs)) & ", Total_Bales_Meters = " & Str(Val(vTotBales)) & "  ,Total_Fabric_Meters = " & Str(Val(vTotFabMts)) & ",Total_Type1_Pcs_Meters = " & Str(Val(vType1_Pcs)) & " ,Total_Type2_Pcs_Meters =" & Str(Val(vType2_Pcs)) & ",  Total_Type3_Pcs_Meters =" & Str(Val(vType3_Pcs)) & ", Total_Type4_Pcs_Meters =" & Str(Val(vType4_Pcs)) & ", Total_Type5_Pcs_Meters=" & Str(Val(vType5_Pcs)) & " ,  Total_Type1_Bales_Meters=" & Str(Val(vType1_bales)) & ",Total_Type2_Bales_Meters =" & Str(Val(vType2_bales)) & ", Total_Type3_Bales_Meters =" & Str(Val(vType3_bales)) & ",Total_Type4_Bales_Meters=" & Str(Val(vType4_bales)) & ",Total_Type5_Bales_Meters=" & Str(Val(vType5_bales)) & "  Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Fabric_Physical_Stock_Code = '" & Trim(NewCode) & "' "
                cmd.ExecuteNonQuery()

            End If

            EntID = Trim(Pk_Condition) & Trim(lbl_DcNo.Text)
            Partcls = "Delv : Dc.No. " & Trim(lbl_DcNo.Text)
            PBlNo = Trim(lbl_DcNo.Text)

            cmd.CommandText = "Delete from Fabric_Physical_Stock_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Fabric_Physical_Stock_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            'cmd.CommandText = "Delete from Stock_Yarn_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            'cmd.ExecuteNonQuery()

            'cmd.CommandText = "Delete from Stock_Empty_BeamBagCone_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            'cmd.ExecuteNonQuery()

            With dgv_FabricDetails
                Sno = 0

                For i = 0 To .RowCount - 1

                    If Val(.Rows(i).Cells(15).Value) <> 0 Then

                        Sno = Sno + 1

                        cloth_ID = Common_Procedures.Cloth_NameToIdNo(con, .Rows(i).Cells(1).Value, tr)

                        cmd.CommandText = "Insert into Fabric_Physical_Stock_Details(Fabric_Physical_Stock_Code,      Company_IdNo,              Fabric_Physical_Stock_No,                   for_OrderBy,                                   Fabric_Physical_Stock_Date,  Sl_No,                Cloth_IdNo,                       Total_Uncheck_Meters,                  Type1_Pcs_Meters,                      Type2_Pcs_Meters,                                 Type3_Pcs_Meters,                         Type4_Pcs_Meters,                       Type5_Pcs_Meters ,                                     Total_Pcs_Meters,                   Type1_Bales_Meters,                          Type2_Bales_Meters,                        Type3_Bales_Meters,                     Type4_Bales_Meters,                                Type5_Bales_Meters,                      Total_Bales_Meters,                             Total_Fabric_Meters ) " & _
                                                                         "  Values ('" & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_DcNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_DcNo.Text))) & ", @DcDate, " & Str(Val(Sno)) & ",  " & Str(Val(cloth_ID)) & ", " & Str(Val(.Rows(i).Cells(2).Value)) & ", " & Str(Val(.Rows(i).Cells(3).Value)) & ", " & Str(Val(.Rows(i).Cells(4).Value)) & ",  " & Str(Val(.Rows(i).Cells(5).Value)) & ", " & Str(Val(.Rows(i).Cells(6).Value)) & " ,  " & Str(Val(.Rows(i).Cells(7).Value)) & ",  " & Str(Val(.Rows(i).Cells(8).Value)) & ",  " & Str(Val(.Rows(i).Cells(9).Value)) & " ," & Str(Val(.Rows(i).Cells(10).Value)) & ", " & Str(Val(.Rows(i).Cells(11).Value)) & ", " & Str(Val(.Rows(i).Cells(12).Value)) & ", " & Str(Val(.Rows(i).Cells(13).Value)) & " ," & Str(Val(.Rows(i).Cells(14).Value)) & " , " & Str(Val(.Rows(i).Cells(15).Value)) & ")"
                        cmd.ExecuteNonQuery()

                        'cmd.CommandText = "Insert into Stock_Yarn_Processing_Details(Reference_Code, Company_IdNo, Reference_No, for_OrderBy, Reference_Date, DeliveryTo_Idno, ReceivedFrom_Idno, Entry_ID, Particulars, Party_Bill_No, Sl_No, Count_IdNo, Yarn_Type, Mill_IdNo, Bags, Cones, Weight, DeliveryToIdno_ForParticulars, ReceivedFromIdno_ForParticulars ) Values ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_DcNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_DcNo.Text))) & ", @DcDate, " & Str(Val(Delv_ID)) & ", " & Str(Val(Rec_ID)) & ", '" & Trim(EntID) & "', '" & Trim(Partcls) & "', '" & Trim(PBlNo) & "', " & Str(Val(Sno)) & ", " & Str(Val(YCnt_ID)) & ", '" & Trim(.Rows(i).Cells(2).Value) & "', " & Str(Val(YMil_ID)) & ", " & Str(Val(.Rows(i).Cells(4).Value)) & ", " & Str(Val(.Rows(i).Cells(5).Value)) & ", " & Str(Val(.Rows(i).Cells(6).Value)) & ", " & Str(Val(Delv_ID)) & ", " & Str(Val(Rec_ID)) & " )"
                        'cmd.ExecuteNonQuery()

                    End If

                Next

            End With

            'If Val(txt_Empty_Beam.Text) <> 0 Or Val(vTotYrnBags) <> 0 Or Val(vTotYrnCones) <> 0 Then
            '    cmd.CommandText = "Insert into Stock_Empty_BeamBagCone_Processing_Details ( Reference_Code, Company_IdNo, Reference_No, for_OrderBy, Reference_Date, DeliveryTo_Idno, ReceivedFrom_Idno, Entry_ID, Party_Bill_No, Particulars, Sl_No, Empty_Beam, Yarn_Bags, Yarn_Cones ) Values ( '" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_DcNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_DcNo.Text))) & ", @DcDate, " & Str(Val(Delv_ID)) & ", " & Str(Val(Rec_ID)) & ", '" & Trim(EntID) & "', '" & Trim(PBlNo) & "', '" & Trim(Partcls) & "', 1, " & Str(Val(txt_Empty_Beam.Text)) & ", " & Str(Val(vTotYrnBags)) & ", " & Str(Val(vTotYrnCones)) & ")"
            '    cmd.ExecuteNonQuery()
            'End If

            'Dim vVou_LedIdNos As String = "", vVou_Amts As String = "", vVou_ErrMsg As String = ""

            'vVou_LedIdNos = Trans_ID & "|" & Val(Common_Procedures.CommonLedger.Transport_Charges_Ac)
            'vVou_Amts = Val(txt_Freight.Text) & "|" & -1 * Val(txt_Freight.Text)
            'If Common_Procedures.Voucher_Updation(con, "Wea.YDelv", Val(lbl_Company.Tag), Trim(Pk_Condition) & Trim(NewCode), Trim(lbl_DcNo.Text), Convert.ToDateTime(msk_date.Text), Partcls, vVou_LedIdNos, vVou_Amts, vVou_ErrMsg, tr) = False Then
            '    Throw New ApplicationException(vVou_ErrMsg)
            'End If

            tr.Commit()

            Dt1.Dispose()
            Da.Dispose()

            MessageBox.Show("Saved Sucessfully!!!", "FOR SAVING...", MessageBoxButtons.OK, MessageBoxIcon.Information)

            If Val(Common_Procedures.settings.OnSave_MoveTo_NewEntry_Status) = 1 Then
                If New_Entry = True Then
                    new_record()
                Else
                    move_record(lbl_DcNo.Text)
                End If
            Else
                move_record(lbl_DcNo.Text)
            End If


        Catch ex As Exception
            tr.Rollback()
            MessageBox.Show(ex.Message, "FOR SAVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally
            If msk_date.Enabled And msk_date.Visible Then msk_date.Focus()

        End Try

    End Sub

    Private Sub cbo_Godown_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Godown.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_IdNo = 0 or Ledger_Type = 'GODOWN')", "(Ledger_idno = 0)")
    End Sub

    Private Sub cbo_Godown_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Godown.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Godown, msk_date, dgv_FabricDetails, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_IdNo = 0 or Ledger_Type = 'GODOWN')", "(Ledger_idno = 0)")
    End Sub

    Private Sub cbo_Godown_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Godown.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Godown, Nothing, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_IdNo = 0 or Ledger_Type = 'GODOWN')", "(Ledger_idno = 0)")
        If Asc(e.KeyChar) = 13 Then
            dgv_FabricDetails.Focus()
            dgv_FabricDetails.CurrentCell = dgv_FabricDetails.Rows(0).Cells(1)
        End If
    End Sub

    Private Sub cbo_Godown_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Godown.KeyUp
        If e.Control = False And e.KeyValue = 17 Then

            Common_Procedures.MDI_LedType = "GODOWN"
            Dim f As New Ledger_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Godown.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If
    End Sub




    Private Sub txt_Freight_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs)
        If e.KeyValue = 38 Then SendKeys.Send("+{TAB}")
        If (e.KeyValue = 40) Then
            If dgv_FabricDetails.Rows.Count > 0 Then
                dgv_FabricDetails.Focus()
                dgv_FabricDetails.CurrentCell = dgv_FabricDetails.Rows(0).Cells(1)

            Else
                btn_save.Focus()

            End If
        End If

    End Sub

    Private Sub txt_Freight_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
            e.Handled = True
        End If
        If Asc(e.KeyChar) = 13 Then
            If dgv_FabricDetails.Rows.Count > 0 Then
                dgv_FabricDetails.Focus()
                dgv_FabricDetails.CurrentCell = dgv_FabricDetails.Rows(0).Cells(1)

            Else
                btn_save.Focus()

            End If
        End If
    End Sub


    Private Sub dgv_FabricDetails_CellEndEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_FabricDetails.CellEndEdit
        dgv_FabricDetails_CellLeave(sender, e)
    End Sub

    Private Sub dgv_FabricDetails_CellEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_FabricDetails.CellEnter
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim Dt2 As New DataTable
        Dim Dt3 As New DataTable
        Dim rect As Rectangle

        With dgv_FabricDetails
            If Val(.CurrentRow.Cells(0).Value) = 0 Then
                .CurrentRow.Cells(0).Value = .CurrentRow.Index + 1
            End If

            'If Trim(.CurrentRow.Cells(2).Value) = "" Then
            '    .CurrentRow.Cells(2).Value = "MILL"
            'End If

            If e.ColumnIndex = 1 Then

                If cbo_Grid_ClothName.Visible = False Or Val(cbo_Grid_ClothName.Tag) <> e.RowIndex Then

                    cbo_Grid_ClothName.Tag = -1
                    Da = New SqlClient.SqlDataAdapter("select cloth_Name from cloth_head order by cloth_Name", con)
                    Dt1 = New DataTable
                    Da.Fill(Dt1)
                    cbo_Grid_ClothName.DataSource = Dt1
                    cbo_Grid_ClothName.DisplayMember = "cloth_Name"

                    rect = .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False)

                    cbo_Grid_ClothName.Left = .Left + rect.Left  '  .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Left
                    cbo_Grid_ClothName.Top = .Top + rect.Top  ' .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Top

                    cbo_Grid_ClothName.Width = rect.Width  ' .CurrentCell.Size.Width
                    cbo_Grid_ClothName.Height = rect.Height  ' rect.Height
                    cbo_Grid_ClothName.Text = .CurrentCell.Value  '  Trim(.CurrentRow.Cells(e.ColumnIndex).Value)

                    cbo_Grid_ClothName.Tag = Val(e.RowIndex)
                    cbo_Grid_ClothName.Visible = True

                    cbo_Grid_ClothName.BringToFront()
                    cbo_Grid_ClothName.Focus()


                End If


            Else

                cbo_Grid_ClothName.Visible = False

            End If


        End With
    End Sub

    Private Sub dgv_FabricDetails_CellLeave(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_FabricDetails.CellLeave
        'With dgv_FabricDetails
        '    If .CurrentCell.ColumnIndex = 6 Then
        '        If Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value) <> 0 Then
        '            .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = Format(Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value), "#########0.000")
        '        End If
        '    End If
        'End With

        With dgv_FabricDetails
            If .CurrentCell.ColumnIndex = 2 Or .CurrentCell.ColumnIndex = 3 Or .CurrentCell.ColumnIndex = 4 Or .CurrentCell.ColumnIndex = 5 Or .CurrentCell.ColumnIndex = 6 Or .CurrentCell.ColumnIndex = 7 Or .CurrentCell.ColumnIndex = 8 Or .CurrentCell.ColumnIndex = 9 Or .CurrentCell.ColumnIndex = 10 Or .CurrentCell.ColumnIndex = 11 Or .CurrentCell.ColumnIndex = 12 Or .CurrentCell.ColumnIndex = 13 Or .CurrentCell.ColumnIndex = 14 Or .CurrentCell.ColumnIndex = 15 Then
                If Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value) <> 0 Then
                    .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = Format(Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value), "#########0.00")
                Else
                    .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = ""
                End If
            End If
        End With

    End Sub

    Private Sub dgv_FabricDetails_CellValueChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_FabricDetails.CellValueChanged
        On Error Resume Next
        If IsNothing(dgv_FabricDetails.CurrentCell) Then Exit Sub
        With dgv_FabricDetails
            If .Visible Then
                If .CurrentCell.ColumnIndex = 2 Or .CurrentCell.ColumnIndex = 3 Or .CurrentCell.ColumnIndex = 4 Or .CurrentCell.ColumnIndex = 5 Or .CurrentCell.ColumnIndex = 6 Or .CurrentCell.ColumnIndex = 7 Or .CurrentCell.ColumnIndex = 8 Or .CurrentCell.ColumnIndex = 9 Or .CurrentCell.ColumnIndex = 10 Or .CurrentCell.ColumnIndex = 11 Or .CurrentCell.ColumnIndex = 12 Or .CurrentCell.ColumnIndex = 13 Or .CurrentCell.ColumnIndex = 15 Then
                    .Rows(.CurrentCell.RowIndex).Cells(8).Value = Format(Val(.Rows(.CurrentCell.RowIndex).Cells(3).Value) + Val(.Rows(.CurrentCell.RowIndex).Cells(4).Value) + Val(.Rows(.CurrentCell.RowIndex).Cells(5).Value) + Val(.Rows(.CurrentCell.RowIndex).Cells(6).Value) + Val(.Rows(.CurrentCell.RowIndex).Cells(7).Value), "##########0.00")
                    .Rows(.CurrentCell.RowIndex).Cells(14).Value = Format(Val(.Rows(.CurrentCell.RowIndex).Cells(9).Value) + Val(.Rows(.CurrentCell.RowIndex).Cells(10).Value) + Val(.Rows(.CurrentCell.RowIndex).Cells(11).Value) + Val(.Rows(.CurrentCell.RowIndex).Cells(12).Value) + Val(.Rows(.CurrentCell.RowIndex).Cells(13).Value), "##########0.00")
                    .Rows(.CurrentCell.RowIndex).Cells(15).Value = Format(Val(.Rows(.CurrentCell.RowIndex).Cells(2).Value) + Val(.Rows(.CurrentCell.RowIndex).Cells(8).Value) + Val(.Rows(.CurrentCell.RowIndex).Cells(14).Value), "##########0.00")
                    ' TotalYarnTaken_Calculation()
                    TotalMeter_Calculation()
                End If
            End If
        End With
    End Sub

    Private Sub dgv_FabricDetails_EditingControlShowing(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewEditingControlShowingEventArgs) Handles dgv_FabricDetails.EditingControlShowing
        dgtxt_Details = CType(dgv_FabricDetails.EditingControl, DataGridViewTextBoxEditingControl)
    End Sub

    Private Sub dgv_FabricDetails_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_FabricDetails.KeyDown

        With dgv_FabricDetails

            If e.KeyCode = Keys.Up Then
                If .CurrentCell.RowIndex = 1 Then
                    .CurrentCell.Selected = False
                    'txt_Freight.Focus()
                    cbo_Godown.Focus()
                End If
            End If
            If e.KeyCode = Keys.Left Then
                If .CurrentCell.RowIndex = 0 And .CurrentCell.ColumnIndex <= 1 Then
                    .CurrentCell.Selected = False
                    '  txt_Freight.Focus()
                    cbo_Godown.Focus()
                    'SendKeys.Send("{RIGHT}")
                End If
            End If

            If e.KeyCode = Keys.Enter Then
                e.SuppressKeyPress = True
                e.Handled = True

                If .CurrentCell.RowIndex = .RowCount - 1 And .CurrentCell.ColumnIndex >= 1 And Trim(.CurrentRow.Cells(1).Value) = "" Then

                    If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                        save_record()
                    End If

                Else
                    SendKeys.Send("{Tab}")

                End If


            End If

        End With


    End Sub

    Private Sub dgv_FabricDetails_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_FabricDetails.KeyUp
        Dim i As Integer
        Dim n As Integer

        If e.Control = True And UCase(Chr(e.KeyCode)) = "D" Then

            With dgv_FabricDetails

                n = .CurrentRow.Index

                If .CurrentCell.RowIndex = .Rows.Count - 1 Then
                    For i = 0 To .ColumnCount - 1
                        .Rows(n).Cells(i).Value = ""
                    Next

                Else
                    .Rows.RemoveAt(n)

                End If

                '  TotalYarnTaken_Calculation()
                TotalMeter_Calculation()

            End With

        End If

    End Sub

    Private Sub dgv_FabricDetails_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgv_FabricDetails.LostFocus
        On Error Resume Next
        If Not IsNothing(dgv_FabricDetails.CurrentCell) Then dgv_FabricDetails.CurrentCell.Selected = False
    End Sub

    Private Sub dgv_FabricDetails_RowsAdded(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewRowsAddedEventArgs) Handles dgv_FabricDetails.RowsAdded
        Dim n As Integer

        'With dgv_FabricDetails
        '    n = .RowCount
        '    .Rows(n - 1).Cells(0).Value = Val(n)
        '    .Rows(n - 1).Cells(2).Value = "MILL"
        'End With
    End Sub
    Private Sub get_MillCount_Details()
        Dim q As Single = 0
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim Cn_bag As Single
        Dim Wgt_Bag As Single
        Dim Wgt_Cn As Single
        Dim CntID As Integer
        Dim MilID As Integer

        CntID = Common_Procedures.Count_NameToIdNo(con, dgv_FabricDetails.Rows(dgv_FabricDetails.CurrentRow.Index).Cells(1).Value)
        MilID = Common_Procedures.Mill_NameToIdNo(con, dgv_FabricDetails.Rows(dgv_FabricDetails.CurrentRow.Index).Cells(3).Value)

        Wgt_Bag = 0 : Wgt_Cn = 0 : Cn_bag = 0

        If CntID <> 0 And MilID <> 0 Then

            Da = New SqlClient.SqlDataAdapter("select * from Mill_Count_Details where mill_idno = " & Str(Val(MilID)) & " and count_idno = " & Str(Val(CntID)), con)
            Da.Fill(Dt)
            With dgv_FabricDetails

                If Dt.Rows.Count > 0 Then
                    If IsDBNull(Dt.Rows(0)(0).ToString) = False Then
                        Wgt_Bag = Dt.Rows(0).Item("Weight_Bag").ToString
                        Wgt_Cn = Dt.Rows(0).Item("Weight_Cone").ToString
                        Cn_bag = Dt.Rows(0).Item("Cones_Bag").ToString
                    End If
                End If

                Dt.Clear()
                Dt.Dispose()
                Da.Dispose()

                If .CurrentCell.ColumnIndex = 4 Or .CurrentCell.ColumnIndex = 5 Then
                    If .CurrentCell.ColumnIndex = 4 Then
                        If Val(Cn_bag) <> 0 Then
                            .Rows(.CurrentRow.Index).Cells(5).Value = .Rows(.CurrentRow.Index).Cells(4).Value * Val(Cn_bag)
                        End If

                        If Val(Wgt_Bag) <> 0 Then
                            .Rows(.CurrentRow.Index).Cells(6).Value = Format(Val(.Rows(.CurrentRow.Index).Cells(4).Value) * Val(Wgt_Bag), "#########0.000")
                        End If

                    End If

                    If .CurrentCell.ColumnIndex = 5 Then
                        If Val(Wgt_Cn) <> 0 Then
                            .Rows(.CurrentRow.Index).Cells(6).Value = Format(.Rows(.CurrentRow.Index).Cells(5).Value * Val(Wgt_Cn), "##########0.000")
                        End If

                    End If

                End If

            End With

        End If

    End Sub

    Private Sub TotalYarnTaken_Calculation()
        'Dim Sno As Integer
        'Dim TotBags As Single, TotCones As Single, TotWeight As Single

        'Sno = 0
        'TotBags = 0
        'TotCones = 0
        'TotWeight = 0
        'With dgv_FabricDetails
        '    For i = 0 To .RowCount - 1
        '        Sno = Sno + 1
        '        .Rows(i).Cells(0).Value = Sno
        '        If Val(.Rows(i).Cells(6).Value) <> 0 Then
        '            TotBags = TotBags + Val(.Rows(i).Cells(4).Value)
        '            TotCones = TotCones + Val(.Rows(i).Cells(5).Value)
        '            TotWeight = TotWeight + Val(.Rows(i).Cells(6).Value)
        '        End If
        '    Next
        'End With

        'With dgv_FabricDetails_Total
        '    If .RowCount = 0 Then .Rows.Add()
        '    .Rows(0).Cells(4).Value = Val(TotBags)
        '    .Rows(0).Cells(5).Value = Val(TotCones)
        '    .Rows(0).Cells(6).Value = Format(Val(TotWeight), "########0.000")
        'End With

    End Sub

    Private Sub TotalMeter_Calculation()
        Dim sno As Integer = 0
        Dim UnchkMtr As Single = 0
        Dim Tot_Pcs_Mtrs As Single = 0, Tot_Bales_Mtrs As Single = 0
        Dim Tot_Fab_Mtrs As Single = 0, fabric_Mtrs As Single = 0

        Dim tot_sound_pcs As Single = 0, tot_sec_pcs As Single = 0, tot_bit_pcs As Single = 0, tot_rej_pcs As Single = 0, tot_othr_pcs As Single = 0
        Dim tot_sound_bales As Single = 0, tot_sec_bales As Single = 0, tot_bit_bales As Single = 0, tot_rej_bales As Single = 0, tot_othr_bales As Single = 0

        'On Error Resume Next

       
        UnchkMtr = 0 : Tot_Pcs_Mtrs = 0 : Tot_Bales_Mtrs = 0 : Tot_Fab_Mtrs = 0 : fabric_Mtrs = 0
        With dgv_FabricDetails
            For i = 0 To .RowCount - 1
                Sno = Sno + 1
                .Rows(i).Cells(0).Value = Sno
                If Val(.Rows(i).Cells(2).Value) <> 0 Or Val(.Rows(i).Cells(3).Value) <> 0 Or Val(.Rows(i).Cells(4).Value) <> 0 Or Val(.Rows(i).Cells(5).Value) <> 0 Or Val(.Rows(i).Cells(6).Value) <> 0 Or Val(.Rows(i).Cells(7).Value) <> 0 Or Val(.Rows(i).Cells(9).Value) <> 0 Or Val(.Rows(i).Cells(10).Value) <> 0 Or Val(.Rows(i).Cells(11).Value) <> 0 Or Val(.Rows(i).Cells(12).Value) <> 0 Or Val(.Rows(i).Cells(13).Value) <> 0 Then

                    UnchkMtr = UnchkMtr + Val(.Rows(i).Cells(2).Value)
                    Tot_Pcs_Mtrs = Tot_Pcs_Mtrs + Val(.Rows(i).Cells(3).Value) + Val(.Rows(i).Cells(4).Value) + Val(.Rows(i).Cells(5).Value) + Val(.Rows(i).Cells(6).Value) + Val(.Rows(i).Cells(7).Value)
                    Tot_Bales_Mtrs = Tot_Bales_Mtrs + Val(.Rows(i).Cells(9).Value) + Val(.Rows(i).Cells(10).Value) + Val(.Rows(i).Cells(11).Value) + Val(.Rows(i).Cells(12).Value) + Val(.Rows(i).Cells(13).Value)
                    Tot_Fab_Mtrs = Tot_Fab_Mtrs + UnchkMtr + Tot_Pcs_Mtrs + Tot_Bales_Mtrs

                    fabric_Mtrs = fabric_Mtrs + Val(.Rows(i).Cells(15).Value)
                    tot_sound_pcs = tot_sound_pcs + Val(.Rows(i).Cells(3).Value)
                    tot_sec_pcs = tot_sec_pcs + Val(.Rows(i).Cells(4).Value)
                    tot_bit_pcs = tot_bit_pcs + Val(.Rows(i).Cells(5).Value)
                    tot_rej_pcs = tot_rej_pcs + Val(.Rows(i).Cells(6).Value)
                    tot_othr_pcs = tot_othr_pcs + Val(.Rows(i).Cells(7).Value)

                    tot_sound_bales = tot_sound_bales + Val(.Rows(i).Cells(9).Value)
                    tot_sec_bales = tot_sec_bales + Val(.Rows(i).Cells(10).Value)
                    tot_bit_bales = tot_bit_bales + Val(.Rows(i).Cells(11).Value)
                    tot_rej_bales = tot_rej_bales + Val(.Rows(i).Cells(12).Value)
                    tot_othr_bales = tot_othr_bales + Val(.Rows(i).Cells(13).Value)

                End If
            Next
        End With


        With dgv_FabricDetails_Total
            If .RowCount = 0 Then .Rows.Add()
            .Rows(0).Cells(2).Value = Format(Val(UnchkMtr), "########0.00")
            .Rows(0).Cells(8).Value = Format(Val(Tot_Pcs_Mtrs), "########0.00")
            .Rows(0).Cells(14).Value = Format(Val(Tot_Bales_Mtrs), "########0.00")
            .Rows(0).Cells(15).Value = Format(Val(fabric_Mtrs), "########0.00")

            .Rows(0).Cells(3).Value = Format(Val(tot_sound_pcs), "########0.00")
            .Rows(0).Cells(4).Value = Format(Val(tot_sec_pcs), "########0.00")
            .Rows(0).Cells(5).Value = Format(Val(tot_bit_pcs), "########0.00")
            .Rows(0).Cells(6).Value = Format(Val(tot_rej_pcs), "########0.00")
            .Rows(0).Cells(7).Value = Format(Val(tot_othr_pcs), "########0.00")

            .Rows(0).Cells(9).Value = Format(Val(tot_sound_bales), "########0.00")
            .Rows(0).Cells(10).Value = Format(Val(tot_sec_bales), "########0.00")
            .Rows(0).Cells(11).Value = Format(Val(tot_bit_bales), "########0.00")
            .Rows(0).Cells(12).Value = Format(Val(tot_rej_bales), "########0.00")
            .Rows(0).Cells(13).Value = Format(Val(tot_othr_bales), "########0.00")

        End With
    End Sub

    'Private Sub cbo_Grid_MillName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs)
    '    Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Mill_Head", "Mill_Name", "", "(Mill_IdNo = 0)")
    'End Sub

    'Private Sub cbo_Grid_MillName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs)
    '    Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Grid_MillName, cbo_Grid_YarnType, Nothing, "Mill_Head", "Mill_Name", "", "(Mill_IdNo = 0)")

    '    With dgv_FabricDetails

    '        If (e.KeyValue = 38 And cbo_Grid_MillName.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
    '            .Focus()
    '            .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex - 1)
    '        End If

    '        If (e.KeyValue = 40 And cbo_Grid_MillName.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
    '            .Focus()
    '            .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 1)
    '        End If

    '    End With



    'End Sub

    'Private Sub cbo_Grid_MillName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
    '    Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Grid_MillName, Nothing, "Mill_Head", "Mill_Name", "", "(Mill_IdNo = 0)")
    '    If Asc(e.KeyChar) = 13 Then

    '        With dgv_FabricDetails

    '            .Focus()
    '            .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)

    '        End With

    '    End If
    'End Sub

    'Private Sub cbo_Grid_MillName_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs)
    '    If e.Control = False And e.KeyValue = 17 Then
    '        Dim f As New Mill_Creation

    '        Common_Procedures.Master_Return.Form_Name = Me.Name
    '        Common_Procedures.Master_Return.Control_Name = cbo_Grid_MillName.Name
    '        Common_Procedures.Master_Return.Return_Value = ""
    '        Common_Procedures.Master_Return.Master_Type = ""

    '        f.MdiParent = MDIParent1
    '        f.Show()

    '    End If
    'End Sub
    'Private Sub cbo_Grid_MillName_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
    '    Try
    '        If cbo_Grid_MillName.Visible Then
    '            With dgv_FabricDetails
    '                If Val(cbo_Grid_MillName.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = 3 Then
    '                    .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_Grid_MillName.Text)
    '                End If
    '            End With
    '        End If

    '    Catch ex As Exception
    '        'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

    '    End Try
    'End Sub
    'Private Sub cbo_Grid_CountName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs)
    '    Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Grid_CountName, Nothing, cbo_Grid_YarnType, "Count_Head", "Count_Name", "", "(Count_IdNo = 0)")
    '    With dgv_FabricDetails
    '        With dgv_FabricDetails

    '            If (e.KeyValue = 38 And cbo_Grid_CountName.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
    '                If .CurrentCell.RowIndex = 0 Then
    '                    txt_Freight.Focus()
    '                Else
    '                    .Focus()
    '                    .CurrentCell = .Rows(.CurrentRow.Index - 1).Cells(.ColumnCount - 1)
    '                End If
    '            End If
    '            If (e.KeyValue = 40 And cbo_Grid_CountName.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
    '                .Focus()
    '                .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 1)
    '            End If

    '        End With

    '    End With
    'End Sub

    'Private Sub cbo_Grid_CountName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
    '    Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Grid_CountName, Nothing, "Count_Head", "Count_Name", "", "(Count_IdNo = 0)")
    '    If Asc(e.KeyChar) = 13 Then

    '        With dgv_FabricDetails
    '            If .CurrentCell.RowIndex = .Rows.Count - 1 And Trim(.Rows(.CurrentCell.RowIndex).Cells.Item(1).Value) = "" Then
    '                If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
    '                    save_record()
    '                End If
    '            Else
    '                .Focus()
    '                .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)
    '            End If
    '        End With
    '    End If

    'End Sub

    'Private Sub cbo_Grid_CountName_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs)
    '    If e.Control = False And e.KeyValue = 17 Then
    '        Dim f As New Count_Creation

    '        Common_Procedures.Master_Return.Form_Name = Me.Name
    '        Common_Procedures.Master_Return.Control_Name = cbo_Grid_CountName.Name
    '        Common_Procedures.Master_Return.Return_Value = ""
    '        Common_Procedures.Master_Return.Master_Type = ""

    '        f.MdiParent = MDIParent1
    '        f.Show()

    '    End If
    'End Sub

    'Private Sub cbo_Grid_CountName_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
    '    Try
    '        If cbo_Grid_CountName.Visible Then
    '            With dgv_FabricDetails
    '                If Val(cbo_Grid_CountName.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = 1 Then
    '                    .Rows(.CurrentCell.RowIndex).Cells.Item(1).Value = Trim(cbo_Grid_CountName.Text)
    '                End If
    '            End With
    '        End If

    '    Catch ex As Exception
    '        'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

    '    End Try
    'End Sub

    Private Sub btn_close_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_close.Click
        Me.Close()
    End Sub

    Private Sub btn_save_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_save.Click
        save_record()
    End Sub

    Private Sub dgtxt_Details_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgtxt_Details.Enter
        dgv_FabricDetails.EditingControl.BackColor = Color.Lime
        dgv_FabricDetails.EditingControl.ForeColor = Color.Blue
        dgtxt_Details.SelectAll()
    End Sub

    

    Private Sub dgtxt_Details_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dgtxt_Details.KeyPress
        With dgv_FabricDetails
            If .Visible Then
                If .CurrentCell.ColumnIndex = 2 Or .CurrentCell.ColumnIndex = 3 Or .CurrentCell.ColumnIndex = 4 Or .CurrentCell.ColumnIndex = 5 Or .CurrentCell.ColumnIndex = 6 Or .CurrentCell.ColumnIndex = 7 Or .CurrentCell.ColumnIndex = 9 Or .CurrentCell.ColumnIndex = 10 Or .CurrentCell.ColumnIndex = 11 Or .CurrentCell.ColumnIndex = 12 Or .CurrentCell.ColumnIndex = 13 Then

                    If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
                        e.Handled = True
                    End If

                End If
            End If
        End With
    End Sub

    Private Sub txt_Empty_Beam_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
            e.Handled = True
        End If
    End Sub
    Private Sub btn_Filter_Close_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Filter_Close.Click
        Pnl_Back.Enabled = True
        pnl_Filter.Visible = False
        Filter_Status = False
    End Sub
    Private Sub btn_Filter_Show_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Filter_Show.Click
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim n As Integer
        Dim Led_IdNo As Integer, Del_IdNo As Integer, Cnt_IdNo As Integer, Mil_IdNo As Integer
        Dim Condt As String = ""

        Try

            Condt = ""
            Del_IdNo = 0
            Cnt_IdNo = 0

            If IsDate(dtp_Filter_Fromdate.Value) = True And IsDate(dtp_Filter_ToDate.Value) = True Then
                Condt = "a.Fabric_Physical_Stock_Date between '" & Trim(Format(dtp_Filter_Fromdate.Value, "MM/dd/yyyy")) & "' and '" & Trim(Format(dtp_Filter_ToDate.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_Filter_Fromdate.Value) = True Then
                Condt = "a.Fabric_Physical_Stock_Date = '" & Trim(Format(dtp_Filter_Fromdate.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_Filter_ToDate.Value) = True Then
                Condt = "a.Fabric_Physical_Stock_Date = '" & Trim(Format(dtp_Filter_ToDate.Value, "MM/dd/yyyy")) & "' "
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
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " (a.DeliveryTo_IdNo = " & Str(Val(Led_IdNo)) & ")"
            End If
            If Val(Cnt_IdNo) <> 0 Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " a.Fabric_Physical_Stock_Code IN ( select z1.Fabric_Physical_Stock_Code from Fabric_Physical_Stock_Details z1 where z1.Count_IdNo = " & Str(Val(Cnt_IdNo)) & " )"
                'Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " b.Count_IdNo = " & Str(Val(Cnt_IdNo)) & ""
            End If

            If Val(Mil_IdNo) <> 0 Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " a.Fabric_Physical_Stock_Code IN ( select z2.Fabric_Physical_Stock_Code from Fabric_Physical_Stock_Details z2 where z2.Mill_IdNo = " & Str(Val(Mil_IdNo)) & " )"
                'Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " b.Mill_IdNo = " & Str(Val(Mil_IdNo))
            End If

            da = New SqlClient.SqlDataAdapter("select a.*, e.Ledger_Name from Fabric_Physical_Stock_Head a inner join Ledger_head e on a.DeliveryTo_IdNo = e.Ledger_idno where a.company_idno =" & Str(Val(lbl_Company.Tag)) & "and a.Fabric_Physical_Stock_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' " & IIf(Trim(Condt) <> "", " and ", "") & Condt & " Order by a.Fabric_Physical_Stock_Date, a.for_orderby, a.Fabric_Physical_Stock_No", con)
            'da = New SqlClient.SqlDataAdapter("select a.*, e.Ledger_Name from Fabric_Physical_Stock_Head a left outer join Fabric_Physical_Stock_Details b on a.Fabric_Physical_Stock_Code = b.Fabric_Physical_Stock_Code left outer join Ledger_head e on a.Ledger_idno = e.Ledger_idno where a.company_idno =" & Str(Val(lbl_Company.Tag)) & "and a.Fabric_Physical_Stock_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' " & IIf(Trim(Condt) <> "", " and ", "") & Condt & " Order by a.Fabric_Physical_Stock_Date, a.for_orderby, a.Fabric_Physical_Stock_No", con)
            da.Fill(dt2)

            dgv_Filter_Details.Rows.Clear()

            If dt2.Rows.Count > 0 Then

                For i = 0 To dt2.Rows.Count - 1

                    n = dgv_Filter_Details.Rows.Add()

                    dgv_Filter_Details.Rows(n).Cells(0).Value = i + 1
                    dgv_Filter_Details.Rows(n).Cells(1).Value = dt2.Rows(i).Item("Fabric_Physical_Stock_No").ToString
                    dgv_Filter_Details.Rows(n).Cells(2).Value = Format(Convert.ToDateTime(dt2.Rows(i).Item("Fabric_Physical_Stock_Date").ToString), "dd-MM-yyyy")
                    dgv_Filter_Details.Rows(n).Cells(3).Value = dt2.Rows(i).Item("Ledger_Name").ToString
                    dgv_Filter_Details.Rows(n).Cells(4).Value = Val(dt2.Rows(i).Item("Total_Bags").ToString)
                    dgv_Filter_Details.Rows(n).Cells(5).Value = Val(dt2.Rows(i).Item("Total_Cones").ToString)
                    dgv_Filter_Details.Rows(n).Cells(6).Value = Format(Val(dt2.Rows(i).Item("Total_Weight").ToString), "########0.000")

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
            cbo_Filter_PartyName.Focus()
        End If
    End Sub

    Private Sub cbo_Filter_PartyName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Filter_PartyName.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "", "(Ledger_idno = 0)")
    End Sub

    Private Sub cbo_Filter_PartyName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Filter_PartyName.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_PartyName, dtp_Filter_ToDate, cbo_Filter_CountName, "Ledger_AlaisHead", "Ledger_DisplayName", "", "(Ledger_idno = 0)")

    End Sub

    Private Sub cbo_Filter_PartyName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Filter_PartyName.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Filter_PartyName, cbo_Filter_CountName, "Ledger_AlaisHead", "Ledger_DisplayName", "", "(Ledger_idno = 0)")
    End Sub

    Private Sub cbo_Filter_CountName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Filter_CountName.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Count_Head", "Count_Name", "", "(Count_IdNo = 0)")
    End Sub


    Private Sub cbo_Filter_CountName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Filter_CountName.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_CountName, cbo_Filter_PartyName, cbo_Filter_MillName, "Count_Head", "Count_Name", "", "(Count_IdNo = 0)")

    End Sub

    Private Sub cbo_Filter_CountName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Filter_CountName.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Filter_CountName, cbo_Filter_MillName, "Count_Head", "Count_Name", "", "(Count_IdNo = 0)")

    End Sub

    Private Sub Open_FilterEntry()
        Dim movno As String

        On Error Resume Next

        movno = Trim(dgv_Filter_Details.CurrentRow.Cells(1).Value)

        If Val(movno) <> 0 Then
            Filter_Status = True
            move_record(movno)
            Pnl_Back.Enabled = True
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

    Private Sub cbo_Filter_MillName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Filter_MillName.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_MillName, cbo_Filter_CountName, btn_Filter_Show, "Mill_Head", "Mill_Name", "", "(Mill_IdNo = 0)")
    End Sub

    Private Sub cbo_Filter_MillName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Filter_MillName.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Filter_MillName, btn_Filter_Show, "Mill_Head", "Mill_Name", "", "(Mill_IdNo = 0)")
    End Sub

    Private Sub btn_Print_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Print.Click
        Common_Procedures.Print_OR_Preview_Status = 1
        print_record()
    End Sub

    Public Sub print_record() Implements Interface_MDIActions.print_record
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim NewCode As String

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Try

            da1 = New SqlClient.SqlDataAdapter("select * from Fabric_Physical_Stock_Head Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Fabric_Physical_Stock_Code = '" & Trim(NewCode) & "'", con)
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

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        prn_HdDt.Clear()
        prn_DetDt.Clear()
        prn_DetIndx = 0
        prn_DetSNo = 0
        prn_PageNo = 0

        Try

            da1 = New SqlClient.SqlDataAdapter("select a.*, b.*, c.*, d.Ledger_Name as Ledger1_Name , e.Transport_Name  from Fabric_Physical_Stock_Head a INNER JOIN Company_Head b ON a.Company_IdNo = b.Company_IdNo INNER JOIN Ledger_Head c ON a.DeliveryTo_IdNo = c.Ledger_IdNo Left Outer JOIN Ledger_Head d ON a.ReceivedFrom_IdNo = d.Ledger_IdNo Left Outer JOIN Transport_Head e ON a.Transport_IdNo = e.Transport_IdNo where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Fabric_Physical_Stock_Code = '" & Trim(NewCode) & "'", con)
            prn_HdDt = New DataTable
            da1.Fill(prn_HdDt)

            If prn_HdDt.Rows.Count > 0 Then

                da2 = New SqlClient.SqlDataAdapter("select a.*, b.Mill_name, d.Count_name  from Fabric_Physical_Stock_Details a INNER JOIN Mill_Head b ON a.Mill_idno = b.Mill_idno LEFT OUTER JOIN Count_Head d ON a.Count_idno = d.Count_idno where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Fabric_Physical_Stock_Code = '" & Trim(NewCode) & "' Order by a.Sl_No", con)
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
        Dim ItmNm1 As String, ItmNm2 As String

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
            .Left = 20
            .Right = 40
            .Top = 20
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

        NoofItems_PerPage = 6

        Erase LnAr
        Erase ClAr

        LnAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        ClAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}

        ClAr(1) = Val(45) : ClAr(2) = 100 : ClAr(3) = 250 : ClAr(4) = 120 : ClAr(5) = 80 : ClAr(6) = 70
        ClAr(7) = PageWidth - (LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6))

        TxtHgt = 18.5

        EntryCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Try
            If prn_HdDt.Rows.Count > 0 Then

                Printing_Format1_PageHeader(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, prn_PageNo, NoofItems_PerPage, CurY, LnAr, ClAr)

                NoofDets = 0

                CurY = CurY - 10

                If prn_DetDt.Rows.Count > 0 Then

                    Do While prn_DetIndx <= prn_DetDt.Rows.Count - 1

                        If NoofDets >= NoofItems_PerPage Then

                            CurY = CurY + TxtHgt

                            Common_Procedures.Print_To_PrintDocument(e, "Continued...", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + 10, CurY, 0, 0, pFont)

                            NoofDets = NoofDets + 1

                            Printing_Format1_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClAr, NoofDets, False)

                            e.HasMorePages = True
                            Return

                        End If

                        ItmNm1 = Trim(prn_DetDt.Rows(prn_DetIndx).Item("Mill_Name").ToString)
                        ItmNm2 = ""
                        If Len(ItmNm1) > 30 Then
                            For I = 15 To 1 Step -1
                                If Mid$(Trim(ItmNm1), I, 1) = " " Or Mid$(Trim(ItmNm1), I, 1) = "," Or Mid$(Trim(ItmNm1), I, 1) = "." Or Mid$(Trim(ItmNm1), I, 1) = "-" Or Mid$(Trim(ItmNm1), I, 1) = "/" Or Mid$(Trim(ItmNm1), I, 1) = "_" Or Mid$(Trim(ItmNm1), I, 1) = "(" Or Mid$(Trim(ItmNm1), I, 1) = ")" Or Mid$(Trim(ItmNm1), I, 1) = "\" Or Mid$(Trim(ItmNm1), I, 1) = "[" Or Mid$(Trim(ItmNm1), I, 1) = "]" Or Mid$(Trim(ItmNm1), I, 1) = "{" Or Mid$(Trim(ItmNm1), I, 1) = "}" Then Exit For
                            Next I
                            If I = 0 Then I = 30
                            ItmNm2 = Microsoft.VisualBasic.Right(Trim(ItmNm1), Len(ItmNm1) - I)
                            ItmNm1 = Microsoft.VisualBasic.Left(Trim(ItmNm1), I - 1)
                        End If

                        CurY = CurY + TxtHgt
                        Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetDt.Rows(prn_DetIndx).Item("Sl_No").ToString), LMargin + 15, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetDt.Rows(prn_DetIndx).Item("Yarn_Type").ToString), LMargin + ClAr(1) + 10, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm1), LMargin + ClAr(1) + ClAr(2) + 10, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Count_Name").ToString, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + 10, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(prn_DetIndx).Item("Bags").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 1, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(prn_DetIndx).Item("Cones").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY, 1, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Weight").ToString), " #######0.000"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, pFont)

                        NoofDets = NoofDets + 1

                        If Trim(ItmNm2) <> "" Then
                            CurY = CurY + TxtHgt - 5
                            Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm2), LMargin + ClAr(1) + 10, CurY, 0, 0, pFont)
                            NoofDets = NoofDets + 1
                        End If

                        prn_DetIndx = prn_DetIndx + 1

                    Loop

                End If

                Printing_Format1_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClAr, NoofDets, True)

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        e.HasMorePages = False

    End Sub

    Private Sub Printing_Format1_PageHeader(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByRef PageNo As Integer, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single)
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt2 As New DataTable
        Dim p1Font As Font
        Dim Cmp_Name As String, Cmp_Add1 As String, Cmp_Add2 As String
        Dim Cmp_PhNo As String, Cmp_TinNo As String, Cmp_CstNo As String, Cmp_GSTNo As String
        Dim strHeight As Single
        Dim C1 As Single
        Dim W1 As Single
        Dim S1 As Single
        Dim Entry_Date As Date = Convert.ToDateTime(prn_HdDt.Rows(0).Item("Fabric_Physical_Stock_Date").ToString)

        PageNo = PageNo + 1

        CurY = TMargin

        da2 = New SqlClient.SqlDataAdapter("select a.*, b.Mill_name, d.Count_name  from Fabric_Physical_Stock_Details a INNER JOIN Mill_Head b ON a.Mill_idno = b.Mill_idno LEFT OUTER JOIN Count_Head d ON a.Count_idno = d.Count_idno where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Fabric_Physical_Stock_Code = '" & Trim(EntryCode) & "' Order by a.Sl_No", con)
        da2.Fill(dt2)

        If dt2.Rows.Count > NoofItems_PerPage Then
            Common_Procedures.Print_To_PrintDocument(e, "Page : " & Trim(Val(PageNo)), PageWidth - 10, CurY - TxtHgt, 1, 0, pFont)
        End If
        dt2.Clear()

        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(1) = CurY

        Cmp_Name = "" : Cmp_Add1 = "" : Cmp_Add2 = ""
        Cmp_PhNo = "" : Cmp_TinNo = "" : Cmp_CstNo = "" : Cmp_GSTNo = ""

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
        If Trim(prn_HdDt.Rows(0).Item("Company_GSTinNo").ToString) <> "" Then
            Cmp_GSTNo = "GSTIN : " & prn_HdDt.Rows(0).Item("Company_GSTinNo").ToString
        End If

        CurY = CurY + TxtHgt - 10
        p1Font = New Font("Calibri", 18, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Name, LMargin, CurY, 2, PrintWidth, p1Font)
        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height

        CurY = CurY + strHeight - 1
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Add1, LMargin, CurY, 2, PrintWidth, pFont)

        CurY = CurY + TxtHgt - 1
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Add2, LMargin, CurY, 2, PrintWidth, pFont)
        CurY = CurY + TxtHgt - 1
        Common_Procedures.Print_To_PrintDocument(e, Cmp_PhNo, LMargin, CurY, 2, PrintWidth, pFont)
        'CurY = CurY + TxtHgt - 1
        'Common_Procedures.Print_To_PrintDocument(e, Cmp_TinNo, LMargin + 10, CurY, 0, 0, pFont)
        'Common_Procedures.Print_To_PrintDocument(e, Cmp_CstNo, PageWidth - 10, CurY, 1, 0, pFont)

        pFont = New Font("Calibri", 11, FontStyle.Bold)
        If Entry_Date >= Common_Procedures.GST_Start_Date Then
            Common_Procedures.Print_To_PrintDocument(e, Cmp_GSTNo, LMargin + 10, CurY, 0, 0, pFont)
        Else
            Common_Procedures.Print_To_PrintDocument(e, Cmp_TinNo, LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Cmp_CstNo, PageWidth - 10, CurY, 1, 0, pFont)
        End If

        CurY = CurY + TxtHgt - 5
        pFont = New Font("Calibri", 11, FontStyle.Regular)
        p1Font = New Font("Calibri", 16, FontStyle.Bold)
        CurY = CurY + TxtHgt - 10
        Common_Procedures.Print_To_PrintDocument(e, "YARN DELIVERY", LMargin, CurY, 2, PrintWidth, p1Font)
        CurY = CurY - 10
        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height

        CurY = CurY + strHeight + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(2) = CurY

        C1 = ClAr(1) + ClAr(2) + ClAr(3)
        W1 = e.Graphics.MeasureString("D.C DATE  : ", pFont).Width
        S1 = e.Graphics.MeasureString("TO  :  ", pFont).Width


        CurY = CurY + TxtHgt - 10
        p1Font = New Font("Calibri", 12, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "TO  :  " & "M/s." & prn_HdDt.Rows(0).Item("Ledger_MainName").ToString, LMargin + 10, CurY, 0, 0, p1Font)
        Common_Procedures.Print_To_PrintDocument(e, "DC.NO", LMargin + C1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Fabric_Physical_Stock_No").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, p1Font)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address1").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, "DATE", LMargin + C1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Fabric_Physical_Stock_Date").ToString), "dd-MM-yyyy").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, pFont)
        p1Font = New Font("Calibri", 14, FontStyle.Bold)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address2").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
        CurY = CurY + TxtHgt
        p1Font = New Font("Calibri", 11, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "GST NO", LMargin + S1 + 12, CurY, 0, 0, p1Font)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + S1 + 65, CurY, 0, 0, p1Font)
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString, LMargin + S1 + 75, CurY, 0, 0, p1Font)

        CurY = CurY + TxtHgt + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(3) = CurY

        e.Graphics.DrawLine(Pens.Black, LMargin + C1, CurY, LMargin + C1, LnAr(2))

        CurY = CurY + TxtHgt - 10
        pFont = New Font("Calibri", 11, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "SNO", LMargin, CurY, 2, ClAr(1), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "YARN TYPE", LMargin + ClAr(1), CurY, 2, ClAr(2), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "MILL NAME", LMargin + ClAr(1) + ClAr(2), CurY, 2, ClAr(3), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "COUNT", LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, 2, ClAr(4), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "BAG", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 2, ClAr(5), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "CONE", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, 2, ClAr(6), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "WEIGHT(Kgs)", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 5, CurY, 2, ClAr(7), pFont)

        pFont = New Font("Calibri", 11, FontStyle.Regular)
        CurY = CurY + TxtHgt + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(4) = CurY


    End Sub

    Private Sub Printing_Format1_PageFooter(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single, ByVal NoofDets As Integer, ByVal is_LastPage As Boolean)
        Dim i As Integer
        Dim Cmp_Name As String
        Dim p1Font As Font
        Dim W1 As Single

        For i = NoofDets + 1 To NoofItems_PerPage
            CurY = CurY + TxtHgt
        Next

        W1 = e.Graphics.MeasureString(" Vehicle No :", pFont).Width

        CurY = CurY + TxtHgt
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(5) = CurY

        CurY = CurY + TxtHgt - 10
        If is_LastPage = True Then
            Common_Procedures.Print_To_PrintDocument(e, " TOTAL", LMargin + ClAr(1) + ClAr(2) + 10, CurY, 2, ClAr(4), pFont)

            Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(0).Item("Total_Bags").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 1, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(0).Item("Total_Cones").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY, 1, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Total_Weight").ToString), " #######0.000"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, pFont)

        End If

        CurY = CurY + TxtHgt + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(6) = CurY

        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1), CurY, LMargin + ClAr(1), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2), CurY, LMargin + ClAr(1) + ClAr(2), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), LnAr(3))

        CurY = CurY + 10

        Common_Procedures.Print_To_PrintDocument(e, " Vehicle No : ", LMargin + 20, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Vechile_No").ToString, LMargin + W1 + 30, CurY, 0, 0, pFont)
        If Val(prn_HdDt.Rows(0).Item("Empty_Beam").ToString) <> 0 Then
            Common_Procedures.Print_To_PrintDocument(e, "Empty Beam : " & Val(prn_HdDt.Rows(0).Item("Empty_Beam").ToString), PageWidth - 20, CurY, 1, 0, pFont)
        End If
        CurY = CurY + TxtHgt + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

        CurY = CurY + TxtHgt
        CurY = CurY + TxtHgt

        Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString
        p1Font = New Font("Calibri", 12, FontStyle.Bold)

        Common_Procedures.Print_To_PrintDocument(e, "Receiver's Signature", LMargin + 20, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, "For " & Cmp_Name, PageWidth - 15, CurY, 1, 0, p1Font)

        CurY = CurY + TxtHgt + 10

        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

        e.Graphics.DrawLine(Pens.Black, LMargin, LnAr(1), LMargin, CurY)
        e.Graphics.DrawLine(Pens.Black, PageWidth, LnAr(1), PageWidth, CurY)

    End Sub

    Private Sub dgtxt_Details_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgtxt_Details.KeyUp
        If e.Control = True And UCase(Chr(e.KeyCode)) = "D" Then
            dgv_FabricDetails_KeyUp(sender, e)
        End If
    End Sub

    Private Sub msk_date_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles msk_date.KeyPress
        If Trim(UCase(e.KeyChar)) = "D" Then
            msk_date.Text = Date.Today
            msk_date.SelectionStart = 0
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
    Private Sub dtp_Date_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dtp_Date.TextChanged
        If IsDate(dtp_Date.Text) = True Then
            msk_date.Text = dtp_Date.Text
            msk_date.SelectionStart = 0
        End If
    End Sub
    Private Sub msk_Date_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles msk_date.KeyDown
        vcbo_KeyDwnVal = e.KeyValue

        If e.KeyCode = 38 Then
            e.Handled = True
        End If

        vmskOldText = ""
        vmskSelStrt = -1
        If e.KeyCode = 46 Or e.KeyCode = 8 Then
            vmskOldText = msk_date.Text
            vmskSelStrt = msk_date.SelectionStart
        End If

    End Sub
    Private Sub btn_Selection_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim i As Integer, j As Integer, n As Integer, SNo As Integer
        Dim LedIdNo As Integer
        Dim NewCode As String
        Dim Ent_Rate As Single = 0
        Dim Ent_Wgt As Single = 0
        Dim Ent_Pcs As Single = 0
        Dim NR As Single = 0

        LedIdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Godown.Text)

        'If LedIdNo = 0 Then
        '    MessageBox.Show("Invalid Party Name", "DOES NOT SELECT PAVU...", MessageBoxButtons.OK, MessageBoxIcon.Error)
        '    If cbo_PartyName.Enabled And cbo_PartyName.Visible Then cbo_PartyName.Focus()
        '    Exit Sub
        'End If

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        With dgv_Selection
            If Val(LedIdNo) <> 0 Then

                .Rows.Clear()
                SNo = 0

                Da = New SqlClient.SqlDataAdapter("select a.*, b.Cloth_Name,c.*  from Own_Order_Head a INNER JOIN CLOTH_Head b ON  b.Cloth_IdNo = a.Cloth_Idno  INNER JOIN Own_Order_Sizing_Details c ON  c.Own_Order_Code = a.Own_Order_Code LEFT OUTER JOIN Fabric_Physical_Stock_Head d ON d.Own_Order_Code = a.Own_Order_Code  where a.Fabric_Physical_Stock_Code = '" & Trim(NewCode) & "'   and c.Ledger_IdNo = " & Val(LedIdNo) & " order by a.Own_Order_Date, a.for_orderby, a.Own_Order_No", con)
                Dt1 = New DataTable
                Da.Fill(Dt1)

                If Dt1.Rows.Count > 0 Then

                    For i = 0 To Dt1.Rows.Count - 1

                        n = .Rows.Add()

                        Ent_Rate = 0


                        SNo = SNo + 1
                        .Rows(n).Cells(0).Value = Val(SNo)

                        .Rows(n).Cells(1).Value = Dt1.Rows(i).Item("Own_Order_No").ToString
                        .Rows(n).Cells(2).Value = Format(Convert.ToDateTime(Dt1.Rows(i).Item("Own_Order_Date").ToString), "dd-MM-yyyy")
                        .Rows(n).Cells(3).Value = Dt1.Rows(i).Item("Order_No").ToString


                        .Rows(n).Cells(4).Value = Dt1.Rows(i).Item("Cloth_Name").ToString

                        .Rows(n).Cells(5).Value = Format(Val(Dt1.Rows(i).Item("Order_Meters").ToString), "#########0.000")
                        .Rows(n).Cells(6).Value = "1"
                        .Rows(n).Cells(7).Value = Dt1.Rows(i).Item("Own_Order_Code").ToString

                        For j = 0 To .ColumnCount - 1
                            .Rows(i).Cells(j).Style.ForeColor = Color.Red
                        Next

                    Next

                End If
                Dt1.Clear()

                Da = New SqlClient.SqlDataAdapter("select a.*, b.Cloth_Name from Own_Order_Head a INNER JOIN Cloth_Head b ON  b.Cloth_Idno = a.Cloth_Idno INNER JOIN Own_Order_Sizing_Details c ON  c.Own_Order_Code = a.Own_Order_Code   LEFT OUTER JOIN Fabric_Physical_Stock_Head d ON d.Fabric_Physical_Stock_Code = a.Own_Order_Code    where a.Fabric_Physical_Stock_Code = ''  and c.Ledger_IdNo = " & Val(LedIdNo) & " order by a.Own_Order_Date, a.for_orderby, a.Own_Order_No", con)
                Dt1 = New DataTable
                NR = Da.Fill(Dt1)

                If Dt1.Rows.Count > 0 Then

                    For i = 0 To Dt1.Rows.Count - 1

                        n = .Rows.Add()

                        SNo = SNo + 1
                        .Rows(n).Cells(0).Value = Val(SNo)
                        .Rows(n).Cells(1).Value = Dt1.Rows(i).Item("OWn_Order_No").ToString
                        .Rows(n).Cells(2).Value = Format(Convert.ToDateTime(Dt1.Rows(i).Item("Own_Order_Date").ToString), "dd-MM-yyyy")
                        .Rows(n).Cells(3).Value = Dt1.Rows(i).Item("Order_No").ToString


                        .Rows(n).Cells(4).Value = Dt1.Rows(i).Item("Cloth_Name").ToString

                        .Rows(n).Cells(5).Value = Format(Val(Dt1.Rows(i).Item("Order_Meters").ToString), "#########0.000")
                        .Rows(n).Cells(6).Value = ""
                        .Rows(n).Cells(7).Value = Dt1.Rows(i).Item("Own_Order_Code").ToString

                    Next

                End If
                Dt1.Clear()
            Else
                .Rows.Clear()
                SNo = 0

                Da = New SqlClient.SqlDataAdapter("select a.*, b.Cloth_Name  from Own_Order_Head a INNER JOIN CLOTH_Head b ON  b.Cloth_IdNo = a.Cloth_Idno   LEFT OUTER JOIN Fabric_Physical_Stock_Head d ON d.Own_Order_Code = a.Own_Order_Code  where a.Fabric_Physical_Stock_Code = '" & Trim(NewCode) & "'   order by a.Own_Order_Date, a.for_orderby, a.Own_Order_No", con)
                Dt1 = New DataTable
                Da.Fill(Dt1)

                If Dt1.Rows.Count > 0 Then

                    For i = 0 To Dt1.Rows.Count - 1

                        n = .Rows.Add()

                        Ent_Rate = 0


                        SNo = SNo + 1
                        .Rows(n).Cells(0).Value = Val(SNo)

                        .Rows(n).Cells(1).Value = Dt1.Rows(i).Item("Own_Order_No").ToString
                        .Rows(n).Cells(2).Value = Format(Convert.ToDateTime(Dt1.Rows(i).Item("Own_Order_Date").ToString), "dd-MM-yyyy")
                        .Rows(n).Cells(3).Value = Dt1.Rows(i).Item("Order_No").ToString

                        .Rows(n).Cells(4).Value = Dt1.Rows(i).Item("Cloth_Name").ToString

                        .Rows(n).Cells(5).Value = Format(Val(Dt1.Rows(i).Item("Order_Meters").ToString), "#########0.000")
                        .Rows(n).Cells(6).Value = "1"
                        .Rows(n).Cells(7).Value = Dt1.Rows(i).Item("Own_Order_Code").ToString

                        For j = 0 To .ColumnCount - 1
                            .Rows(i).Cells(j).Style.ForeColor = Color.Red
                        Next

                    Next

                End If
                Dt1.Clear()

                Da = New SqlClient.SqlDataAdapter("select a.*, b.Cloth_Name from Own_Order_Head a INNER JOIN Cloth_Head b ON  b.Cloth_Idno = a.Cloth_Idno    LEFT OUTER JOIN Fabric_Physical_Stock_Head d ON d.Fabric_Physical_Stock_Code = a.Own_Order_Code    where a.Fabric_Physical_Stock_Code = ''   order by a.Own_Order_Date, a.for_orderby, a.Own_Order_No", con)
                Dt1 = New DataTable
                NR = Da.Fill(Dt1)

                If Dt1.Rows.Count > 0 Then

                    For i = 0 To Dt1.Rows.Count - 1

                        n = .Rows.Add()

                        SNo = SNo + 1
                        .Rows(n).Cells(0).Value = Val(SNo)
                        .Rows(n).Cells(1).Value = Dt1.Rows(i).Item("Own_Order_No").ToString
                        .Rows(n).Cells(2).Value = Format(Convert.ToDateTime(Dt1.Rows(i).Item("Own_Order_Date").ToString), "dd-MM-yyyy")
                        .Rows(n).Cells(3).Value = Dt1.Rows(i).Item("Order_No").ToString

                        .Rows(n).Cells(4).Value = Dt1.Rows(i).Item("Cloth_Name").ToString

                        .Rows(n).Cells(5).Value = Format(Val(Dt1.Rows(i).Item("Order_Meters").ToString), "#########0.000")
                        .Rows(n).Cells(6).Value = ""
                        .Rows(n).Cells(7).Value = Dt1.Rows(i).Item("Own_Order_Code").ToString

                    Next

                End If
                Dt1.Clear()
            End If
        End With

        pnl_Selection.Visible = True
        Pnl_Back.Enabled = False
        dgv_Selection.Focus()

    End Sub

    Private Sub dgv_Selection_CellClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Selection.CellClick
        Select_Piece(e.RowIndex)
    End Sub

    Private Sub Select_Piece(ByVal RwIndx As Integer)
        Dim i As Integer

        With dgv_Selection

            If .RowCount > 0 And RwIndx >= 0 Then

                .Rows(RwIndx).Cells(6).Value = (Val(.Rows(RwIndx).Cells(6).Value) + 1) Mod 2

                If Val(.Rows(RwIndx).Cells(6).Value) = 1 Then

                    For i = 0 To .ColumnCount - 1
                        .Rows(RwIndx).Cells(i).Style.ForeColor = Color.Red
                    Next

                Else
                    .Rows(RwIndx).Cells(6).Value = ""

                    For i = 0 To .ColumnCount - 1
                        .Rows(RwIndx).Cells(i).Style.ForeColor = Color.Black
                    Next

                End If

            End If

        End With

    End Sub

    Private Sub dgv_Selection_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_Selection.KeyDown
        Dim n As Integer

        On Error Resume Next

        If e.KeyCode = Keys.Enter Or e.KeyCode = Keys.Space Then
            If dgv_Selection.CurrentCell.RowIndex >= 0 Then

                n = dgv_Selection.CurrentCell.RowIndex

                Select_Piece(n)

                e.Handled = True

            End If
        End If

    End Sub

    Private Sub btn_Close_Selection_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Close_Selection.Click
        'Close_Receipt_Selection()
    End Sub

    'Private Sub Close_Receipt_Selection()
    '    Dim Da1 As New SqlClient.SqlDataAdapter
    '    Dim Dt1 As New DataTable
    '    Dim n As Integer = 0
    '    Dim sno As Integer = 0
    '    Dim i As Integer = 0
    '    Dim j As Integer = 0

    '    lbl_OrderNo.Text = ""
    '    lbl_OrderCode.Text = ""

    '    For i = 0 To dgv_Selection.RowCount - 1

    '        If Val(dgv_Selection.Rows(i).Cells(6).Value) = 1 Then

    '            ' lbl_RecCode.Text = dgv_Selection.Rows(i).Cells(8).Value

    '            lbl_OrderNo.Text = dgv_Selection.Rows(i).Cells(3).Value
    '            lbl_OrderCode.Text = dgv_Selection.Rows(i).Cells(7).Value

    '        End If

    '    Next

    '    pnl_Back.Enabled = True
    '    pnl_Selection.Visible = False

    '    If cbo_RecFrom.Enabled And cbo_RecFrom.Visible Then cbo_RecFrom.Focus()



    'End Sub

    Private Sub cbo_Grid_ClothName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Grid_ClothName.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "cloth_head", "cloth_Name", "", "(cloth_IdNo = 0)")
    End Sub

    Private Sub cbo_Grid_ClothName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Grid_ClothName.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Grid_ClothName, Nothing, Nothing, "cloth_head", "cloth_Name", "", "(cloth_IdNo = 0)")
        With dgv_FabricDetails

            If (e.KeyValue = 38 And cbo_Grid_ClothName.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
                .Focus()
                .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex - 1)
            End If

            If (e.KeyValue = 40 And cbo_Grid_ClothName.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
                .Focus()
                .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 1)
            End If

        End With

    End Sub

    Private Sub cbo_Grid_ClothName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Grid_ClothName.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Grid_ClothName, Nothing, "cloth_head", "cloth_Name", "", "(cloth_IdNo = 0)")
        If Asc(e.KeyChar) = 13 Then

            With dgv_FabricDetails

                If .CurrentCell.RowIndex = .Rows.Count - 1 And Trim(.Rows(.CurrentCell.RowIndex).Cells.Item(1).Value) = "" Then
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

    Private Sub cbo_Grid_ClothName_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Grid_ClothName.KeyUp
        If e.Control = False And e.KeyValue = 17 Then
            Dim f As New Cloth_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Grid_ClothName.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If
    End Sub

    Private Sub cbo_Grid_ClothName_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Grid_ClothName.TextChanged
        Try
            If cbo_Grid_ClothName.Visible Then
                If IsNothing(dgv_FabricDetails.CurrentCell) Then Exit Sub
                With dgv_FabricDetails
                    If Val(cbo_Grid_ClothName.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = 1 Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_Grid_ClothName.Text)
                    End If
                End With
            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub dgtxt_Details_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgtxt_Details.TextChanged
        Try
            With dgv_FabricDetails
                If .Rows.Count <> 0 Then
                    .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(dgtxt_Details.Text)
                End If
            End With

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try
    End Sub
End Class
