Public Class Bale_UnPacking_Entry
    Implements Interface_MDIActions

    Private con As New SqlClient.SqlConnection(Common_Procedures.Connection_String)
    Private con1 As New SqlClient.SqlConnection(Common_Procedures.ConnectionString_CompanyGroupdetails)
    Private FrmLdSTS As Boolean = False
    Private New_Entry As Boolean = False
    Private Insert_Entry As Boolean = False
    Private Filter_Status As Boolean = False
    Private Pk_Condition As String = "UNPAK-"
    Private PkCondition_RollPacking As String = "RLPCK-"
    Private PkCondition_BaleDirectEntry As String = "BALES-"
    Private NoCalc_Status As Boolean = False
    Private Prec_ActCtrl As New Control
    Private vcbo_KeyDwnVal As Double
    Private vCbo_ItmNm As String

    Private WithEvents dgtxt_Details As New DataGridViewTextBoxEditingControl

    Private prn_HdDt As New DataTable
    Private prn_DetDt As New DataTable
    Private prn_PageNo As Integer
    Private prn_DetIndx As Integer
    Private prn_HdAr(100, 10) As String
    Private prn_DetAr(100, 50, 10) As String
    Private prn_DetMxIndx As Integer
    Private prn_HdMxIndx As Integer
    Private prn_HdIndx As Integer
    Private prn_DetSNo As Integer
    Private prn_Count As Integer = 0
    Public vmskOldText As String = ""
    Public vmskSelStrt As Integer = -1
    Public nr As Integer = 0

    Private Sub clear()

        NoCalc_Status = True

        New_Entry = False
        Insert_Entry = False

        Pnl_Back.Enabled = True
        pnl_Filter.Visible = False
        pnl_Selection.Visible = False
        pnl_Print.Visible = False
        vmskOldText = ""
        vmskSelStrt = -1
        lbl_PackingSlipCode.Text = ""

        lbl_UnPackNo.Text = ""
        lbl_UnPackNo.ForeColor = Color.Black

        lbl_PackingSlipCode.Text = ""
        lbl_Packing_Slip_Date.Text = ""

        msk_date.Text = ""
        dtp_Date.Text = ""
        cbo_PartyName_StockOF.Text = Common_Procedures.Ledger_IdNoToName(con, Val(Common_Procedures.CommonLedger.OwnSort_Ac))
        'cbo_PartyName.Text = Common_Procedures.Ledger_IdNoToName(con, Val(Common_Procedures.CommonLedger.Godown_Ac))
        ' cbo_Cloth.Text = ""
        cbo_ClothType.Text = Common_Procedures.ClothType_IdNoToName(con, 1)
        'cbo_Bale_Bundle.Text = "BALE"
        cbo_Godown_StockIN.Text = "GODOWN"
        txt_BaleSelection.Text = ""

        lbl_UserName.Text = "USER : " & Trim(UCase(Common_Procedures.User_IdNoToName(con1, Common_Procedures.User.IdNo)))

        txt_Folding.Text = 100
        txt_Note.Text = ""

        dgv_Details.Rows.Clear()
        dgv_Details_Total.Rows.Clear()
        dgv_Details_Total.Rows.Add()


        If Filter_Status = False Then
            dtp_Filter_Fromdate.Text = ""
            dtp_Filter_ToDate.Text = ""
            cbo_Filter_Cloth.Text = ""
            cbo_Filter_ClothType.Text = ""
            cbo_Filter_Cloth.SelectedIndex = -1
            cbo_Filter_ClothType.SelectedIndex = -1
            dgv_Filter_Details.Rows.Clear()
        End If

        cbo_Cloth.Enabled = True
        cbo_Cloth.BackColor = Color.White

        cbo_ClothType.Enabled = True
        cbo_ClothType.BackColor = Color.White

        txt_Folding.Enabled = True
        txt_Folding.BackColor = Color.White

        btn_Selection.Enabled = True

        Grid_Cell_DeSelect()
        NoCalc_Status = False

    End Sub

    Private Sub ControlGotFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim txtbx As TextBox
        Dim combobx As ComboBox
        Dim Msktxbx As MaskedTextBox
        On Error Resume Next

        If TypeOf Me.ActiveControl Is TextBox Or TypeOf Me.ActiveControl Is ComboBox Or TypeOf Me.ActiveControl Is CheckBox Or TypeOf Me.ActiveControl Is Button Or TypeOf Me.ActiveControl Is MaskedTextBox Then
            Me.ActiveControl.BackColor = Color.SpringGreen
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

        If Me.ActiveControl.Name <> dgv_Details_Total.Name Then
            Grid_DeSelect()
        End If

        Prec_ActCtrl = Me.ActiveControl

    End Sub

    Private Sub ControlLostFocus(ByVal sender As Object, ByVal e As System.EventArgs)

        On Error Resume Next

        If IsNothing(Prec_ActCtrl) = False Then
            If TypeOf Prec_ActCtrl Is TextBox Or TypeOf Prec_ActCtrl Is ComboBox Or TypeOf Prec_ActCtrl Is CheckBox Or TypeOf Prec_ActCtrl Is MaskedTextBox Then
                Prec_ActCtrl.BackColor = Color.White
                Prec_ActCtrl.ForeColor = Color.Black
            ElseIf TypeOf Prec_ActCtrl Is Button Then
                Prec_ActCtrl.BackColor = Color.FromArgb(2, 57, 111)
                Prec_ActCtrl.ForeColor = Color.White
            End If
        End If

    End Sub

    Private Sub Grid_DeSelect()
        On Error Resume Next
        If Not IsNothing(dgv_Details.CurrentCell) Then dgv_Details.CurrentCell.Selected = False
        If Not IsNothing(dgv_Details_Total.CurrentCell) Then dgv_Details_Total.CurrentCell.Selected = False
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
        If Not IsNothing(dgv_Details.CurrentCell) Then dgv_Details.CurrentCell.Selected = False
        If Not IsNothing(dgv_Details_Total.CurrentCell) Then dgv_Details_Total.CurrentCell.Selected = False
        If Not IsNothing(dgv_Filter_Details.CurrentCell) Then dgv_Filter_Details.CurrentCell.Selected = False
    End Sub

    Private Sub Packing_Slip_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated
        Dim da As SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim NoofComps As Integer
        Dim CompCondt As String

        Try

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_PartyName_StockOF.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "LEDGER" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_PartyName_StockOF.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Cloth.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "CLOTH" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Cloth.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_ClothType.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "CLOTH TYPE" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_ClothType.Text = Trim(Common_Procedures.Master_Return.Return_Value)
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
                    MessageBox.Show("Invalid Company Selection", "DOES NOT SELECT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                    'Me.Close()
                    Exit Sub

                End If

            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "DOES NOT SHOW...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

        FrmLdSTS = False
    End Sub

    Private Sub Packing_Slip_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        On Error Resume Next
        con.Close()
        con.Dispose()
        Common_Procedures.Last_Closed_FormName = Me.Name
    End Sub

    Private Sub Packing_Slip_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        Try
            If Asc(e.KeyChar) = 27 Then

                If pnl_Filter.Visible = True Then
                    btn_Filter_Close_Click(sender, e)
                    Exit Sub

                ElseIf pnl_Selection.Visible = True Then
                    btn_Close_Selection_Click(sender, e)
                    Exit Sub

                ElseIf pnl_Print.Visible = True Then
                    btn_print_Close_Click(sender, e)
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
            MessageBox.Show(ex.Message, "DOES NOT CLOSE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub Packing_Slip_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim dt3 As New DataTable

        con.Open()

        dgv_Details.Columns(1).HeaderText = Trim(UCase(Common_Procedures.settings.ClothReceipt_LotNo_OR_RollNo_Text))
        dgv_Selection.Columns(1).HeaderText = Trim(UCase(Common_Procedures.settings.ClothReceipt_LotNo_OR_RollNo_Text))

        Me.Text = ""

        cbo_PartyName_StockOF.Visible = False
        lbl_PartyName_StockOF_Caption.Visible = False
        If Common_Procedures.settings.JOBWORKENTRY_Status = 1 Then
            cbo_PartyName_StockOF.Visible = True
            lbl_PartyName_StockOF_Caption.Visible = True
        End If

        cbo_Godown_StockIN.Visible = False
        lbl_Godown_StockIN_Caption.Visible = False
        If Common_Procedures.settings.Multi_Godown_Status = 1 Then
            cbo_Godown_StockIN.Visible = True
            lbl_Godown_StockIN_Caption.Visible = True

            If Common_Procedures.settings.JOBWORKENTRY_Status = 0 Then
                lbl_Godown_StockIN_Caption.Left = lbl_PartyName_StockOF_Caption.Left
                cbo_Godown_StockIN.Left = cbo_PartyName_StockOF.Left
                cbo_Godown_StockIN.Width = cbo_PartyName_StockOF.Width
            End If

        End If

        da = New SqlClient.SqlDataAdapter("select Ledger_DisplayName from Ledger_AlaisHead where (Ledger_IdNo = 0 or Ledger_Type = 'GODOWN' or Ledger_Type = 'JOBWORKER' or Show_In_All_Entry = 1) order by Ledger_DisplayName", con)
        da.Fill(dt3)
        cbo_PartyName_StockOF.DataSource = dt3
        cbo_PartyName_StockOF.DisplayMember = "Ledger_DisplayName"

        da = New SqlClient.SqlDataAdapter("select Cloth_Name from Cloth_Head order by Cloth_Name", con)
        da.Fill(dt1)
        cbo_Cloth.DataSource = dt1
        cbo_Cloth.DisplayMember = "Cloth_Name"

        da = New SqlClient.SqlDataAdapter("select ClothType_Name from ClothType_Head order by ClothType_Name", con)
        da.Fill(dt2)
        cbo_ClothType.DataSource = dt2
        cbo_ClothType.DisplayMember = "ClothType_Name"

        dtp_Date.Text = ""
        msk_date.Text = ""

        'cbo_Bale_Bundle.Items.Clear()
        'cbo_Bale_Bundle.Items.Add("BALE")
        'cbo_Bale_Bundle.Items.Add("BUNDLE")
        'cbo_Bale_Bundle.Items.Add("ROLL")

        pnl_Filter.Visible = False
        pnl_Filter.Left = (Me.Width - pnl_Filter.Width) \ 2
        pnl_Filter.Top = (Me.Height - pnl_Filter.Height) \ 2

        pnl_Selection.Visible = False
        pnl_Selection.Left = (Me.Width - pnl_Selection.Width) \ 2
        pnl_Selection.Top = (Me.Height - pnl_Selection.Height) \ 2
        pnl_Selection.BringToFront()

        pnl_Print.Visible = False
        pnl_Print.Left = (Me.Width - pnl_Print.Width) \ 2
        pnl_Print.Top = (Me.Height - pnl_Print.Height) \ 2
        pnl_Print.BringToFront()


        AddHandler msk_date.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Cloth.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_ClothType.GotFocus, AddressOf ControlGotFocus
        'AddHandler cbo_Bale_Bundle.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Folding.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Note.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Filter_Cloth.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Filter_ClothType.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_PrintFrom.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_PrintTo.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_PartyName_StockOF.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_save.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_Print.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_close.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_Print_Ok.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_Print_Cancel.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_BaleSelection.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Godown_StockIN.GotFocus, AddressOf ControlGotFocus

        AddHandler cbo_PartyName_StockOF.LostFocus, AddressOf ControlLostFocus
        AddHandler msk_date.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Cloth.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_ClothType.LostFocus, AddressOf ControlLostFocus
        'AddHandler cbo_Bale_Bundle.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Folding.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Note.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Filter_Cloth.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Filter_ClothType.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_PrintFrom.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_PrintTo.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_save.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_Print.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_close.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_Print_Ok.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_Print_Cancel.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_BaleSelection.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Godown_StockIN.LostFocus, AddressOf ControlLostFocus

        AddHandler msk_date.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler msk_date.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_PrintFrom.KeyPress, AddressOf TextBoxControlKeyPress

        lbl_Company.Text = ""
        lbl_Company.Tag = 0
        lbl_Company.Visible = False
        Common_Procedures.CompIdNo = 0

        Filter_Status = False
        FrmLdSTS = True
        new_record()

    End Sub

    Protected Overrides Function ProcessCmdKey(ByRef msg As System.Windows.Forms.Message, ByVal keyData As System.Windows.Forms.Keys) As Boolean
        Dim dgv1 As New DataGridView

        If ActiveControl.Name = dgv_Details.Name Or TypeOf ActiveControl Is DataGridViewTextBoxEditingControl Then

            On Error Resume Next

            dgv1 = Nothing

            If ActiveControl.Name = dgv_Details.Name Then
                dgv1 = dgv_Details

            ElseIf dgv_Details.IsCurrentRowDirty = True Then
                dgv1 = dgv_Details

            End If

            If IsNothing(dgv1) = False Then

                With dgv1


                    If keyData = Keys.Enter Or keyData = Keys.Down Then
                        If .CurrentCell.ColumnIndex >= .ColumnCount - 1 Then
                            If .CurrentCell.RowIndex = .RowCount - 1 Then
                                txt_Note.Focus()

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
                                txt_Folding.Focus()

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
            MessageBox.Show(ex.Message, "DOES NOT CLOSE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

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
        Dim LockSTS As Boolean = False

        If Val(no) = 0 Then Exit Sub

        clear()

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(no) & "/" & Trim(Common_Procedures.FnYearCode)

        Try
            da1 = New SqlClient.SqlDataAdapter("select a.* from Bale_UnPacking_Head a Where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Bale_UnPacking_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'", con)
            dt1 = New DataTable
            da1.Fill(dt1)

            If dt1.Rows.Count > 0 Then

                lbl_UnPackNo.Text = dt1.Rows(0).Item("Bale_UnPacking_No").ToString
                dtp_Date.Text = dt1.Rows(0).Item("Bale_UnPacking_Date").ToString
                msk_date.Text = dtp_Date.Text
                cbo_PartyName_StockOF.Text = Common_Procedures.Ledger_IdNoToName(con, Val(dt1.Rows(0).Item("Ledger_IdNo").ToString))
                cbo_Cloth.Text = Common_Procedures.Cloth_IdNoToName(con, Val(dt1.Rows(0).Item("Cloth_IdNo").ToString))
                cbo_ClothType.Text = Common_Procedures.ClothType_IdNoToName(con, Val(dt1.Rows(0).Item("ClothType_IdNo").ToString))
                cbo_Godown_StockIN.Text = Common_Procedures.Ledger_IdNoToName(con, Val(dt1.Rows(0).Item("WareHouse_IdNo").ToString))

                txt_Note.Text = dt1.Rows(0).Item("Note").ToString
                txt_Folding.Text = Format(Val(dt1.Rows(0).Item("Folding").ToString), "########0.00")
                lbl_PackingSlipCode.Text = dt1.Rows(0).Item("Packing_Slip_Code").ToString
                lbl_Packing_Slip_Date.Text = Format(dt1.Rows(0).Item("Packing_Slip_Date"), "dd/MM/yyyy")

                lbl_UserName.Text = "USER : " & Trim(UCase(Common_Procedures.User_IdNoToName(con1, Val(dt1.Rows(0).Item("User_IdNo").ToString))))



                da2 = New SqlClient.SqlDataAdapter("select a.*, b.clothtype_name , c.Ledger_Name, d.cloth_name from Bale_UnPacking_Details a INNER JOIN ClothType_Head b ON a.ClothType_IdNo <> 0 and a.ClothType_IdNo = b.ClothType_IdNo LEFT OUTER JOIN Ledger_Head c ON a.Party_Idno <> 0 and a.Party_Idno = c.Ledger_Idno LEFT OUTER JOIN Cloth_Head d ON a.Cloth_Idno <> 0 and a.Cloth_Idno = d.Cloth_Idno where a.Bale_UnPacking_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' Order by a.sl_no", con)
                dt2 = New DataTable
                da2.Fill(dt2)

                dgv_Details.Rows.Clear()
                SNo = 0

                If dt2.Rows.Count > 0 Then

                    For i = 0 To dt2.Rows.Count - 1

                        n = dgv_Details.Rows.Add()

                        SNo = SNo + 1
                        dgv_Details.Rows(n).Cells(0).Value = Val(SNo)
                        dgv_Details.Rows(n).Cells(1).Value = dt2.Rows(i).Item("Lot_No").ToString
                        dgv_Details.Rows(n).Cells(2).Value = dt2.Rows(i).Item("Pcs_NO").ToString
                        dgv_Details.Rows(n).Cells(3).Value = dt2.Rows(i).Item("clothtype_name").ToString
                        dgv_Details.Rows(n).Cells(4).Value = Format(Val(dt2.Rows(i).Item("Meters").ToString), "########0.00")
                        If Val(dt2.Rows(i).Item("Weight").ToString) <> 0 Then
                            dgv_Details.Rows(n).Cells(5).Value = Format(Val(dt2.Rows(i).Item("Weight").ToString), "########0.000")
                        End If
                        If Val(dt2.Rows(i).Item("Weight_Meter").ToString) <> 0 Then
                            dgv_Details.Rows(n).Cells(6).Value = Format(Val(dt2.Rows(i).Item("Weight_Meter").ToString), "########0.000")
                        End If

                        dgv_Details.Rows(n).Cells(7).Value = dt2.Rows(i).Item("Ledger_Name").ToString
                        dgv_Details.Rows(n).Cells(8).Value = dt2.Rows(i).Item("lot_code").ToString
                        dgv_Details.Rows(n).Cells(9).Value = dt2.Rows(i).Item("cloth_name").ToString

                        If Val(dt2.Rows(i).Item("Loom_IdNo").ToString) <> 0 Then
                            dgv_Details.Rows(n).Cells(10).Value = Common_Procedures.Loom_IdNoToName(con, Val(dt2.Rows(i).Item("Loom_IdNo").ToString))
                        Else
                            dgv_Details.Rows(n).Cells(10).Value = dt2.Rows(i).Item("Loom_No").ToString
                        End If



                    Next i

                End If

                With dgv_Details_Total

                    If .RowCount = 0 Then .Rows.Add()
                    .Rows(0).Cells(2).Value = Val(dt1.Rows(0).Item("Total_Pcs").ToString)
                    .Rows(0).Cells(4).Value = Format(Val(dt1.Rows(0).Item("Total_Meters").ToString), "########0.00")
                    .Rows(0).Cells(5).Value = Format(Val(dt1.Rows(0).Item("Total_Weight").ToString), "########0.000")

                End With

                dt2.Clear()
                dt2.Dispose()
                da2.Dispose()


            Else

                new_record()

            End If

            dt1.Clear()
            dt1.Dispose()
            da1.Dispose()

            If LockSTS = True Then

                cbo_Cloth.Enabled = False
                cbo_Cloth.BackColor = Color.LightGray

                cbo_ClothType.Enabled = False
                cbo_ClothType.BackColor = Color.LightGray

                txt_Folding.Enabled = False
                txt_Folding.BackColor = Color.LightGray

                btn_Selection.Enabled = False

            End If

            Grid_Cell_DeSelect()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT MOVE RECORDS...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

        If msk_date.Visible And msk_date.Enabled Then msk_date.Focus()

    End Sub

    Public Sub delete_record() Implements Interface_MDIActions.delete_record
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim cmd As New SqlClient.SqlCommand
        Dim trans As SqlClient.SqlTransaction
        Dim NewCode As String = ""

        ' If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.PackinSlip_Entry, "~L~") = 0 And InStr(Common_Procedures.UR.PackinSlip_Entry, "~D~") = 0 Then MessageBox.Show("You have No Rights to Delete", "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error) : Exit Sub

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

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_UnPackNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Da = New SqlClient.SqlDataAdapter("select PackingSlip_Code_Type1 from Weaver_ClothReceipt_Piece_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Bale_UnPacking_Code_Type1 = '" & Trim(Pk_Condition) & Trim(NewCode) & "'", con)
        Dt1 = New DataTable
        Da.Fill(Dt1)
        If Dt1.Rows.Count > 0 Then
            For i = 0 To Dt1.Rows.Count - 1
                If IsDBNull(Dt1.Rows(i).Item("PackingSlip_Code_Type1")) = False Then
                    If Trim(Dt1.Rows(i).Item("PackingSlip_Code_Type1").ToString) <> "" Then
                        If Trim(Dt1.Rows(i).Item("PackingSlip_Code_Type1").ToString) <> Trim(lbl_PackingSlipCode.Text) Then
                            MessageBox.Show("Already Packing slip Prepared" & vbCrLf & "Packing Slip No.: " & Dt1.Rows(i).Item("PackingSlip_Code_Type1").ToString, "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                            Exit Sub
                        End If
                    End If
                End If
            Next
        End If
        Da.Dispose()
        Dt1.Clear()
        Dt1.Dispose()

        Da = New SqlClient.SqlDataAdapter("select PackingSlip_Code_Type2  from Weaver_ClothReceipt_Piece_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Bale_UnPacking_Code_Type2 = '" & Trim(Pk_Condition) & Trim(NewCode) & "'", con)
        Dt1 = New DataTable
        Da.Fill(Dt1)
        If Dt1.Rows.Count > 0 Then
            For i = 0 To Dt1.Rows.Count - 1
                If IsDBNull(Dt1.Rows(i).Item("PackingSlip_Code_Type2")) = False Then
                    If Trim(Dt1.Rows(i).Item("PackingSlip_Code_Type2").ToString) <> "" Then
                        If Trim(Dt1.Rows(i).Item("PackingSlip_Code_Type2").ToString) <> Trim(lbl_PackingSlipCode.Text) Then
                            MessageBox.Show("Already Packing slip Prepared" & vbCrLf & "Packing Slip No.: " & Dt1.Rows(i).Item("PackingSlip_Code_Type2").ToString, "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                            Exit Sub
                        End If
                    End If
                End If
            Next
        End If
        Da.Dispose()
        Dt1.Clear()
        Dt1.Dispose()

        Da = New SqlClient.SqlDataAdapter("select PackingSlip_Code_Type3  from Weaver_ClothReceipt_Piece_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Bale_UnPacking_Code_Type3 = '" & Trim(Pk_Condition) & Trim(NewCode) & "'", con)
        Dt1 = New DataTable
        Da.Fill(Dt1)
        If Dt1.Rows.Count > 0 Then
            For i = 0 To Dt1.Rows.Count - 1

                If IsDBNull(Dt1.Rows(i).Item("PackingSlip_Code_Type3")) = False Then
                    If Trim(Dt1.Rows(i).Item("PackingSlip_Code_Type3").ToString) <> "" Then
                        If Trim(Dt1.Rows(i).Item("PackingSlip_Code_Type3").ToString) <> Trim(lbl_PackingSlipCode.Text) Then
                            MessageBox.Show("Already Packing slip Prepared" & vbCrLf & "Packing Slip No.: " & Dt1.Rows(i).Item("PackingSlip_Code_Type3").ToString, "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                            Exit Sub
                        End If
                    End If
                End If
            Next
        End If
        Da.Dispose()
        Dt1.Clear()
        Dt1.Dispose()

        Da = New SqlClient.SqlDataAdapter("select PackingSlip_Code_Type4  from Weaver_ClothReceipt_Piece_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Bale_UnPacking_Code_Type4 = '" & Trim(Pk_Condition) & Trim(NewCode) & "'", con)
        Dt1 = New DataTable
        Da.Fill(Dt1)
        If Dt1.Rows.Count > 0 Then
            For i = 0 To Dt1.Rows.Count - 1

                If IsDBNull(Dt1.Rows(i).Item("PackingSlip_Code_Type4")) = False Then
                    If Trim(Dt1.Rows(i).Item("PackingSlip_Code_Type4").ToString) <> "" Then
                        If Trim(Dt1.Rows(i).Item("PackingSlip_Code_Type4").ToString) <> Trim(lbl_PackingSlipCode.Text) Then
                            MessageBox.Show("Already Packing slip Prepared" & vbCrLf & "Packing Slip No.: " & Dt1.Rows(i).Item("PackingSlip_Code_Type4").ToString, "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                            Exit Sub
                        End If
                    End If
                End If
            Next
        End If
        Da.Dispose()
        Dt1.Clear()
        Dt1.Dispose()

        Da = New SqlClient.SqlDataAdapter("select PackingSlip_Code_Type5  from Weaver_ClothReceipt_Piece_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Bale_UnPacking_Code_Type5 = '" & Trim(Pk_Condition) & Trim(NewCode) & "'", con)
        Dt1 = New DataTable
        Da.Fill(Dt1)
        If Dt1.Rows.Count > 0 Then
            For i = 0 To Dt1.Rows.Count - 1

                If IsDBNull(Dt1.Rows(i).Item("PackingSlip_Code_Type5")) = False Then
                    If Trim(Dt1.Rows(i).Item("PackingSlip_Code_Type5").ToString) <> "" Then
                        If Trim(Dt1.Rows(i).Item("PackingSlip_Code_Type5").ToString) <> Trim(lbl_PackingSlipCode.Text) Then
                            MessageBox.Show("Already Packing slip Prepared" & vbCrLf & "Packing Slip No.: " & Dt1.Rows(i).Item("PackingSlip_Code_Type5").ToString, "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                            Exit Sub
                        End If
                    End If
                End If
            Next
        End If
        Da.Dispose()
        Dt1.Clear()
        Dt1.Dispose()

        trans = con.BeginTransaction

        Try

            cmd.Connection = con
            cmd.Transaction = trans

            cmd.CommandText = "update Weaver_ClothReceipt_Piece_Details set Bale_UnPacking_Code_Type1 ='' ,Bale_UnPacking_Increment_Type1 = Bale_UnPacking_Increment_Type1- 1 ,  PackingSlip_Code_Type1 = '" & Trim(lbl_PackingSlipCode.Text) & "', PackingSlip_Inc_Type1 = PackingSlip_Inc_Type1 + 1 Where Bale_UnPacking_Code_Type1 = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "update Weaver_ClothReceipt_Piece_Details set Bale_UnPacking_Code_Type2 ='' ,Bale_UnPacking_Increment_Type2 = Bale_UnPacking_Increment_Type2 - 1 , PackingSlip_Code_Type2 = '" & Trim(lbl_PackingSlipCode.Text) & "', PackingSlip_Inc_Type2 = PackingSlip_Inc_Type2 + 1 Where Bale_UnPacking_Code_Type2 = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "update Weaver_ClothReceipt_Piece_Details set Bale_UnPacking_Code_Type3 ='' ,Bale_UnPacking_Increment_Type3 = Bale_UnPacking_Increment_Type3 - 1 , PackingSlip_Code_Type3 = '" & Trim(lbl_PackingSlipCode.Text) & "', PackingSlip_Inc_Type3 = PackingSlip_Inc_Type3 + 1 Where Bale_UnPacking_Code_Type3 = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "update Weaver_ClothReceipt_Piece_Details set Bale_UnPacking_Code_Type4 ='' ,Bale_UnPacking_Increment_Type4 = Bale_UnPacking_Increment_Type4 - 1 , PackingSlip_Code_Type4 = '" & Trim(lbl_PackingSlipCode.Text) & "', PackingSlip_Inc_Type4 = PackingSlip_Inc_Type4 + 1 Where Bale_UnPacking_Code_Type4 = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "update Weaver_ClothReceipt_Piece_Details set Bale_UnPacking_Code_Type5 ='' ,Bale_UnPacking_Increment_Type5 = Bale_UnPacking_Increment_Type5 - 1 , PackingSlip_Code_Type5 = '" & Trim(lbl_PackingSlipCode.Text) & "', PackingSlip_Inc_Type5 = PackingSlip_Inc_Type5 + 1 Where Bale_UnPacking_Code_Type5 = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()


            cmd.CommandText = "update Packing_Slip_Head set Delivery_Date =null, Delivery_Code ='' ,Delivery_No = '' , Delivery_Increment = Delivery_Increment - 1  Where Packing_Slip_Code = '" & Trim(lbl_PackingSlipCode.Text) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "delete from Bale_UnPacking_Details where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Bale_UnPacking_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "delete from Bale_UnPacking_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Bale_UnPacking_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
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

            da = New SqlClient.SqlDataAdapter("select Cloth_Name from Cloth_Head order by Cloth_Name", con)
            da.Fill(dt1)
            cbo_Filter_Cloth.DataSource = dt1
            cbo_Filter_Cloth.DisplayMember = "Cloth_Name"

            da = New SqlClient.SqlDataAdapter("select ClothType_Name from ClothType_head order by ClothType_Name", con)
            da.Fill(dt2)
            cbo_Filter_ClothType.DataSource = dt2
            cbo_Filter_ClothType.DisplayMember = "ClothType_Name"


            dtp_Filter_Fromdate.Text = ""
            dtp_Filter_ToDate.Text = ""
            cbo_Filter_Cloth.Text = ""
            cbo_Filter_ClothType.Text = ""


            cbo_Filter_Cloth.SelectedIndex = -1
            cbo_Filter_ClothType.SelectedIndex = -1
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

            da = New SqlClient.SqlDataAdapter("select top 1 Bale_UnPacking_No from Bale_UnPacking_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Bale_UnPacking_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' and Bale_UnPacking_Code NOT LIKE '" & Trim(PkCondition_RollPacking) & "%' and Bale_UnPacking_Code NOT LIKE '" & Trim(PkCondition_BaleDirectEntry) & "%' Order by for_Orderby, Bale_UnPacking_No", con)
            dt = New DataTable
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

            OrdByNo = Common_Procedures.OrderBy_CodeToValue(Trim(lbl_UnPackNo.Text))

            da = New SqlClient.SqlDataAdapter("select top 1 Bale_UnPacking_No from Bale_UnPacking_Head where for_orderby > " & Str(Val(OrdByNo)) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and Bale_UnPacking_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' and Bale_UnPacking_Code NOT LIKE '" & Trim(PkCondition_RollPacking) & "%'  and Bale_UnPacking_Code NOT LIKE '" & Trim(PkCondition_BaleDirectEntry) & "%' Order by for_Orderby, Bale_UnPacking_No", con)
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

            OrdByNo = Common_Procedures.OrderBy_CodeToValue(Trim(lbl_UnPackNo.Text))

            da = New SqlClient.SqlDataAdapter("select top 1 Bale_UnPacking_No from Bale_UnPacking_Head where for_orderby < " & Str(Val(OrdByNo)) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and Bale_UnPacking_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' and Bale_UnPacking_Code NOT LIKE '" & Trim(PkCondition_RollPacking) & "%'  and Bale_UnPacking_Code NOT LIKE '" & Trim(PkCondition_BaleDirectEntry) & "%' Order by for_Orderby desc, Bale_UnPacking_No desc", con)
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
            da = New SqlClient.SqlDataAdapter("select top 1 Bale_UnPacking_No from Bale_UnPacking_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Bale_UnPacking_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' and Bale_UnPacking_Code NOT LIKE '" & Trim(PkCondition_RollPacking) & "%'  and Bale_UnPacking_Code NOT LIKE '" & Trim(PkCondition_BaleDirectEntry) & "%' Order by for_Orderby desc, Bale_UnPacking_No desc", con)
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
        Dim dt1 As New DataTable
        Dim NewID As Integer = 0

        Try
            clear()

            New_Entry = True

            lbl_UnPackNo.Text = Common_Procedures.get_MaxCode(con, "Bale_UnPacking_Head", "Bale_UnPacking_Code", "For_OrderBy", "(Bale_UnPacking_Code NOT LIKE '" & Trim(PkCondition_RollPacking) & "%'  and Bale_UnPacking_Code NOT LIKE '" & Trim(PkCondition_BaleDirectEntry) & "%' )", Val(lbl_Company.Tag), Common_Procedures.FnYearCode)

            lbl_UnPackNo.ForeColor = Color.Red


            msk_date.Text = Date.Today.ToShortDateString
            da = New SqlClient.SqlDataAdapter("select top 1 * from Bale_UnPacking_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Bale_UnPacking_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' and Bale_UnPacking_Code NOT LIKE '" & Trim(PkCondition_RollPacking) & "%'  and Bale_UnPacking_Code NOT LIKE '" & Trim(PkCondition_BaleDirectEntry) & "%' Order by for_Orderby desc, Bale_UnPacking_No desc", con)
            dt1 = New DataTable
            da.Fill(dt1)
            If dt1.Rows.Count > 0 Then
                If Val(Common_Procedures.settings.PreviousEntryDate_ByDefault) = 1 Then '---- M.S Textiles (Tirupur)
                    If dt1.Rows(0).Item("Bale_UnPacking_Date").ToString <> "" Then msk_date.Text = dt1.Rows(0).Item("Bale_UnPacking_Date").ToString
                End If

                cbo_PartyName_StockOF.Text = Common_Procedures.Ledger_IdNoToName(con, Val(dt1.Rows(0).Item("Ledger_IdNo").ToString))
                cbo_Cloth.Text = Common_Procedures.Cloth_IdNoToName(con, Val(dt1.Rows(0).Item("Cloth_IdNo").ToString))
                cbo_ClothType.Text = Common_Procedures.ClothType_IdNoToName(con, Val(dt1.Rows(0).Item("ClothType_IdNo").ToString))
                'cbo_Bale_Bundle.Text = dt1.Rows(0).Item("Bale_Bundle").ToString

                txt_Folding.Text = Format(Val(dt1.Rows(0).Item("Folding").ToString), "########0.00")
                cbo_Godown_StockIN.Text = Common_Procedures.Ledger_IdNoToName(con, Val(dt1.Rows(0).Item("WareHouse_IdNo").ToString))

            End If
            dt1.Clear()


            If msk_date.Enabled And msk_date.Visible Then msk_date.Focus() : msk_date.SelectionStart = 0

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR NEW RECORD...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

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

            inpno = InputBox("Enter Bale.No.", "FOR FINDING...")

            RecCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

            Da = New SqlClient.SqlDataAdapter("select Bale_UnPacking_No from Bale_UnPacking_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Bale_UnPacking_Code = '" & Trim(RecCode) & "'", con)
            Dt = New DataTable
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
                MessageBox.Show("Rec No. does not exists", "DOES NOT FIND...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT FIND...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Public Sub insert_record() Implements Interface_MDIActions.insert_record
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim movno As String, inpno As String
        Dim RecCode As String

        ' If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.PackinSlip_Entry, "~L~") = 0 And InStr(Common_Procedures.UR.PackinSlip_Entry, "~I~") = 0 Then MessageBox.Show("You have No Rights to Insert", "DOES NOT INSERT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error) : Exit Sub

        Try

            inpno = InputBox("Enter New Bale No.", "FOR NEW RECEIPT INSERTION...")

            RecCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

            Da = New SqlClient.SqlDataAdapter("select Bale_UnPacking_No from Bale_UnPacking_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Bale_UnPacking_Code = '" & Trim(RecCode) & "' and Bale_UnPacking_Code NOT LIKE '" & Trim(PkCondition_RollPacking) & "%'  and Bale_UnPacking_Code NOT LIKE '" & Trim(PkCondition_BaleDirectEntry) & "%' ", con)
            Dt = New DataTable
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
                    MessageBox.Show("Invalid Bale No", "DOES NOT INSERT NEW Bale NO...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

                Else
                    new_record()
                    Insert_Entry = True
                    lbl_UnPackNo.Text = Trim(UCase(inpno))

                End If

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT INSERT NEW Bale No...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Public Sub save_record() Implements Interface_MDIActions.save_record
        Dim cmd As New SqlClient.SqlCommand
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim tr As SqlClient.SqlTransaction
        Dim NewCode As String = ""
        Dim Clth_ID As Integer = 0
        Dim Clthty_ID As Integer = 0
        Dim dCloTyp_ID As Integer = 0
        Dim dClo_ID As Integer = 0
        Dim Trans_ID As Integer = 0
        Dim led_id As Integer = 0
        Dim EdsCnt_ID As Integer = 0
        Dim Sno As Integer = 0
        Dim Partcls As String = ""
        Dim PBlNo As String = ""
        Dim vTotMtrs As Single, vTotPcs As Single, vTotWgt As Single
        Dim EntID As String = ""
        Dim party_ID As Integer = 0
        Dim vLmIdNo As Integer = 0
        Dim vLmNo As String = ""
        Dim Gdwn_ID As Integer = 0

        If Val(lbl_Company.Tag) = 0 Then
            MessageBox.Show("Invalid Company Selection", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
        End If

        '  If Common_Procedures.UserRight_Check(Common_Procedures.UR.PackinSlip_Entry, New_Entry) = False Then Exit Sub

        If Pnl_Back.Enabled = False Then
            MessageBox.Show("Close Other Windows", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
        End If

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
        lbl_UserName.Text = Common_Procedures.User.IdNo
        led_id = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_PartyName_StockOF.Text)
        If led_id = 0 Then
            MessageBox.Show("Invalid Ledger Name", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If cbo_PartyName_StockOF.Enabled Then cbo_PartyName_StockOF.Focus()
            Exit Sub
        End If

        Clth_ID = Common_Procedures.Cloth_NameToIdNo(con, cbo_Cloth.Text)
        If Clth_ID = 0 Then
            MessageBox.Show("Invalid Cloth Name", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If cbo_Cloth.Enabled And cbo_Cloth.Visible Then cbo_Cloth.Focus()
            Exit Sub
        End If

        Clthty_ID = Common_Procedures.ClothType_NameToIdNo(con, cbo_ClothType.Text)
        If Clthty_ID = 0 Then
            MessageBox.Show("Invalid Cloth Type Name", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If cbo_ClothType.Enabled And cbo_ClothType.Visible Then cbo_ClothType.Focus()
            Exit Sub
        End If
        Gdwn_ID = Common_Procedures.Ledger_NameToIdNo(con, cbo_Godown_StockIN.Text)
        vTotMtrs = 0 : vTotPcs = 0 : vTotWgt = 0
        If dgv_Details_Total.RowCount > 0 Then
            vTotMtrs = Val(dgv_Details_Total.Rows(0).Cells(4).Value())
            vTotPcs = Val(dgv_Details_Total.Rows(0).Cells(2).Value())
            vTotWgt = Val(dgv_Details_Total.Rows(0).Cells(5).Value())

        End If


        tr = con.BeginTransaction

        Try

            If Insert_Entry = True Or New_Entry = False Then
                NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_UnPackNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            Else

                lbl_UnPackNo.Text = Common_Procedures.get_MaxCode(con, "Bale_UnPacking_Head", "Bale_UnPacking_Code", "For_OrderBy", "(Bale_UnPacking_Code NOT LIKE '" & Trim(PkCondition_RollPacking) & "%' and Bale_UnPacking_Code NOT LIKE '" & Trim(PkCondition_BaleDirectEntry) & "%' )", Val(lbl_Company.Tag), Common_Procedures.FnYearCode, tr)

                NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_UnPackNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            End If



            '===Checking 

            Da = New SqlClient.SqlDataAdapter("select PackingSlip_Code_Type1  from Weaver_ClothReceipt_Piece_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Bale_UnPacking_Code_Type1 = '" & Trim(Pk_Condition) & Trim(NewCode) & "'", con)
            If IsNothing(tr) = False Then
                Da.SelectCommand.Transaction = tr
            End If
            Dt1 = New DataTable
            Da.Fill(Dt1)
            If Dt1.Rows.Count > 0 Then
                For i = 0 To Dt1.Rows.Count - 1
                    If IsDBNull(Dt1.Rows(i).Item("PackingSlip_Code_Type1")) = False Then
                        If Trim(Dt1.Rows(i).Item("PackingSlip_Code_Type1").ToString) <> "" Then
                            If Trim(Dt1.Rows(i).Item("PackingSlip_Code_Type1").ToString) <> Trim(lbl_PackingSlipCode.Text) Then
                                MessageBox.Show("Already Packing slip Prepared" & vbCrLf & "Packing Slip No.: " & Dt1.Rows(i).Item("PackingSlip_Code_Type1").ToString, "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                                Exit Sub
                            End If
                        End If
                    End If
                Next
            End If
            Da.Dispose()
            Dt1.Clear()
            Dt1.Dispose()

            Da = New SqlClient.SqlDataAdapter("select PackingSlip_Code_Type2  from Weaver_ClothReceipt_Piece_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Bale_UnPacking_Code_Type2 = '" & Trim(Pk_Condition) & Trim(NewCode) & "'", con)
            If IsNothing(tr) = False Then
                Da.SelectCommand.Transaction = tr
            End If
            Dt1 = New DataTable
            Da.Fill(Dt1)

            If Dt1.Rows.Count > 0 Then
                For i = 0 To Dt1.Rows.Count - 1

                    If IsDBNull(Dt1.Rows(i).Item("PackingSlip_Code_Type2")) = False Then
                        If Trim(Dt1.Rows(i).Item("PackingSlip_Code_Type2").ToString) <> "" Then
                            If Trim(Dt1.Rows(i).Item("PackingSlip_Code_Type2").ToString) <> Trim(lbl_PackingSlipCode.Text) Then
                                MessageBox.Show("Already Packing slip Prepared" & vbCrLf & "Packing Slip No.: " & Dt1.Rows(i).Item("PackingSlip_Code_Type2").ToString, "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                                Exit Sub
                            End If
                        End If
                    End If
                Next
            End If
            Da.Dispose()
            Dt1.Clear()
            Dt1.Dispose()

            Da = New SqlClient.SqlDataAdapter("select PackingSlip_Code_Type3  from Weaver_ClothReceipt_Piece_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Bale_UnPacking_Code_Type3 = '" & Trim(Pk_Condition) & Trim(NewCode) & "'", con)
            If IsNothing(tr) = False Then
                Da.SelectCommand.Transaction = tr
            End If
            Dt1 = New DataTable
            Da.Fill(Dt1)
            If Dt1.Rows.Count > 0 Then
                For i = 0 To Dt1.Rows.Count - 1

                    If IsDBNull(Dt1.Rows(i).Item("PackingSlip_Code_Type3")) = False Then
                        If Trim(Dt1.Rows(i).Item("PackingSlip_Code_Type3").ToString) <> "" Then
                            If Trim(Dt1.Rows(i).Item("PackingSlip_Code_Type3").ToString) <> Trim(lbl_PackingSlipCode.Text) Then
                                MessageBox.Show("Already Packing slip Prepared" & vbCrLf & "Packing Slip No.: " & Dt1.Rows(i).Item("PackingSlip_Code_Type3").ToString, "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                                Exit Sub
                            End If
                        End If
                    End If
                Next
            End If
            Da.Dispose()
            Dt1.Clear()
            Dt1.Dispose()

            Da = New SqlClient.SqlDataAdapter("select PackingSlip_Code_Type4  from Weaver_ClothReceipt_Piece_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Bale_UnPacking_Code_Type4 = '" & Trim(Pk_Condition) & Trim(NewCode) & "'", con)
            If IsNothing(tr) = False Then
                Da.SelectCommand.Transaction = tr
            End If
            Dt1 = New DataTable
            Da.Fill(Dt1)
            If Dt1.Rows.Count > 0 Then
                For i = 0 To Dt1.Rows.Count - 1

                    If IsDBNull(Dt1.Rows(i).Item("PackingSlip_Code_Type4")) = False Then
                        If Trim(Dt1.Rows(i).Item("PackingSlip_Code_Type4").ToString) <> "" Then
                            If Trim(Dt1.Rows(i).Item("PackingSlip_Code_Type4").ToString) <> Trim(lbl_PackingSlipCode.Text) Then
                                MessageBox.Show("Already Packing slip Prepared" & vbCrLf & "Packing Slip No.: " & Dt1.Rows(i).Item("PackingSlip_Code_Type4").ToString, "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                                Exit Sub
                            End If
                        End If
                    End If
                Next
            End If
            Da.Dispose()
            Dt1.Clear()
            Dt1.Dispose()

            Da = New SqlClient.SqlDataAdapter("select PackingSlip_Code_Type5  from Weaver_ClothReceipt_Piece_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Bale_UnPacking_Code_Type5 = '" & Trim(Pk_Condition) & Trim(NewCode) & "'", con)
            If IsNothing(tr) = False Then
                Da.SelectCommand.Transaction = tr
            End If
            Dt1 = New DataTable
            Da.Fill(Dt1)
            If Dt1.Rows.Count > 0 Then
                For i = 0 To Dt1.Rows.Count - 1

                    If IsDBNull(Dt1.Rows(i).Item("PackingSlip_Code_Type5")) = False Then
                        If Trim(Dt1.Rows(i).Item("PackingSlip_Code_Type5").ToString) <> "" Then
                            If Trim(Dt1.Rows(i).Item("PackingSlip_Code_Type5").ToString) <> Trim(lbl_PackingSlipCode.Text) Then
                                MessageBox.Show("Already Packing slip Prepared" & vbCrLf & "Packing Slip No.: " & Dt1.Rows(i).Item("PackingSlip_Code_Type5").ToString, "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                                Exit Sub
                            End If
                        End If
                    End If
                Next
            End If
            Da.Dispose()
            Dt1.Clear()
            Dt1.Dispose()

            cmd.Connection = con
            cmd.Transaction = tr

            cmd.Parameters.Clear()
            cmd.Parameters.AddWithValue("@UnPackDate", Convert.ToDateTime(msk_date.Text))
            cmd.Parameters.AddWithValue("@PackingSlipDate", Convert.ToDateTime(lbl_Packing_Slip_Date.Text))

            If New_Entry = True Then

                cmd.CommandText = "Insert into Bale_UnPacking_Head(   Bale_UnPacking_Code  ,                 Company_IdNo     ,        Bale_UnPacking_No       ,                           for_OrderBy                                    , Bale_UnPacking_Date     ,              Cloth_IdNo   ,           ClothType_IdNo    ,                  Folding            ,              Total_Pcs    ,              Total_Meters  ,           Total_Weight    ,               Note            ,           Ledger_IdNo   ,            User_IdNo          ,     WareHouse_IdNo  ,Packing_Slip_Code                         ,Packing_Slip_Date) " & _
                                  "Values                         ('" & Trim(Pk_Condition) & Trim(NewCode) & "' , " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_UnPackNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_UnPackNo.Text))) & " ,        @UnPackDate      ,  " & Str(Val(Clth_ID)) & ", " & Str(Val(Clthty_ID)) & " ,  " & Str(Val(txt_Folding.Text)) & " , " & Str(Val(vTotPcs)) & " , " & Str(Val(vTotMtrs)) & " , " & Str(Val(vTotWgt)) & " , '" & Trim(txt_Note.Text) & "' , " & Str(Val(led_id)) & "," & Val(lbl_UserName.Text) & " ," & Val(Gdwn_ID) & " ,'" & Trim(lbl_PackingSlipCode.Text) & "',@PackingSlipDate )"
                cmd.ExecuteNonQuery()

            Else
                cmd.CommandText = "Update Bale_UnPacking_Head set Bale_UnPacking_Date = @UnPackDate, Packing_Slip_Date  = @PackingSlipDate ,Packing_Slip_Code ='" & Trim(lbl_PackingSlipCode.Text) & "' , Cloth_IdNo = " & Str(Val(Clth_ID)) & " , ClothType_IdNo = " & Str(Val(Clthty_ID)) & "   ,   Folding = " & Str(Val(txt_Folding.Text)) & ", Total_Pcs = " & Str(Val(vTotPcs)) & ", Total_Weight = " & Str(Val(vTotWgt)) & " , Total_Meters = " & Str(Val(vTotMtrs)) & " , Note = '" & Trim(txt_Note.Text) & "' , Ledger_IdNo = " & Str(Val(led_id)) & " , User_IdNo = " & Val(lbl_UserName.Text) & ", WareHouse_IdNo = " & Val(Gdwn_ID) & " Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Bale_UnPacking_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()


                nr = 0
                cmd.CommandText = "update Weaver_ClothReceipt_Piece_Details set Bale_UnPacking_Code_Type1 ='' ,Bale_UnPacking_Increment_Type1 = Bale_UnPacking_Increment_Type1 - 1 ,  PackingSlip_Code_Type1 = '" & Trim(lbl_PackingSlipCode.Text) & "', PackingSlip_Inc_Type1 = PackingSlip_Inc_Type1 + 1 Where Bale_UnPacking_Code_Type1 = '" & Trim(Pk_Condition) & Trim(NewCode) & "' "
                nr = cmd.ExecuteNonQuery()

                nr = 0
                cmd.CommandText = "update Weaver_ClothReceipt_Piece_Details set Bale_UnPacking_Code_Type2 ='' ,Bale_UnPacking_Increment_Type2 = Bale_UnPacking_Increment_Type2- 1 , PackingSlip_Code_Type2 = '" & Trim(lbl_PackingSlipCode.Text) & "', PackingSlip_Inc_Type2 = PackingSlip_Inc_Type2 + 1 Where Bale_UnPacking_Code_Type2 = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
                nr = cmd.ExecuteNonQuery()

                nr = 0
                cmd.CommandText = "update Weaver_ClothReceipt_Piece_Details set Bale_UnPacking_Code_Type3 ='' ,Bale_UnPacking_Increment_Type3 = Bale_UnPacking_Increment_Type3 - 1 , PackingSlip_Code_Type3 = '" & Trim(lbl_PackingSlipCode.Text) & "', PackingSlip_Inc_Type3 = PackingSlip_Inc_Type3 + 1 Where Bale_UnPacking_Code_Type3 = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
                nr = cmd.ExecuteNonQuery()

                nr = 0
                cmd.CommandText = "update Weaver_ClothReceipt_Piece_Details set Bale_UnPacking_Code_Type4 ='' ,Bale_UnPacking_Code_Type1 = Bale_UnPacking_Increment_Type4 - 1 , PackingSlip_Code_Type4 = '" & Trim(lbl_PackingSlipCode.Text) & "', PackingSlip_Inc_Type4 = PackingSlip_Inc_Type4 + 1 Where Bale_UnPacking_Code_Type4 = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
                nr = cmd.ExecuteNonQuery()

                nr = 0
                cmd.CommandText = "update Weaver_ClothReceipt_Piece_Details set Bale_UnPacking_Code_Type5 ='' ,Bale_UnPacking_Increment_Type5 = Bale_UnPacking_Increment_Type5 - 1 , PackingSlip_Code_Type5 = '" & Trim(lbl_PackingSlipCode.Text) & "', PackingSlip_Inc_Type5 = PackingSlip_Inc_Type5 + 1 Where Bale_UnPacking_Code_Type5 = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
                nr = cmd.ExecuteNonQuery()

                cmd.CommandText = "update Packing_Slip_Head set Delivery_Date =null, Delivery_Code ='' ,Delivery_No = '' , Delivery_Increment = Delivery_Increment - 1  Where Packing_Slip_Code = '" & Trim(lbl_PackingSlipCode.Text) & "'"
                cmd.ExecuteNonQuery()


            End If

            cmd.CommandText = "update Packing_Slip_Head set Delivery_Date = @UnPackDate , Delivery_Code ='" & Trim(Pk_Condition) & Trim(NewCode) & "' ,Delivery_No = '" & Trim(lbl_UnPackNo.Text) & "' ,Delivery_Increment = Delivery_Increment + 1  Where Packing_Slip_Code = '" & Trim(lbl_PackingSlipCode.Text) & "'"
            cmd.ExecuteNonQuery()


            EntID = Trim(Pk_Condition) & Trim(lbl_UnPackNo.Text)
            If Trim(lbl_UnPackNo.Text) <> "" Then
                Partcls = "UNPACK : No. " & Trim(lbl_UnPackNo.Text)
            End If
            PBlNo = Trim(lbl_UnPackNo.Text)

            cmd.CommandText = "Delete from Bale_UnPacking_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Bale_UnPacking_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            With dgv_Details
                Sno = 0

                For i = 0 To .RowCount - 1

                    If Val(.Rows(i).Cells(4).Value) <> 0 Then

                        dCloTyp_ID = Common_Procedures.ClothType_NameToIdNo(con, .Rows(i).Cells(3).Value, tr)
                        party_ID = Common_Procedures.Ledger_AlaisNameToIdNo(con, .Rows(i).Cells(7).Value, tr)
                        dClo_ID = Common_Procedures.Cloth_NameToIdNo(con, .Rows(i).Cells(9).Value, tr)

                        vLmNo = .Rows(i).Cells(10).Value
                        vLmIdNo = Common_Procedures.Loom_NameToIdNo(con, .Rows(i).Cells(10).Value, tr)

                        Sno = Sno + 1

                        cmd.CommandText = "Insert into Bale_UnPacking_Details (   Bale_UnPacking_Code ,              Company_IdNo        ,            Bale_UnPacking_No   ,                               for_OrderBy                               , Bale_UnPacking_Date,          Cloth_IdNo      ,                  Folding           ,           Sl_No      ,                     Lot_No              ,                    Pcs_No              ,           ClothType_IdNo    ,                      Meters              ,                      Weight              ,                      Weight_Meter        ,             Party_IdNo     ,                    Lot_Code             ,             Loom_IdNo     ,          Loom_No         ,Packing_Slip_Code ) " & _
                                            "          Values               ('" & Trim(Pk_Condition) & Trim(NewCode) & "'  , " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_UnPackNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_UnPackNo.Text))) & ",    @UnPackDate        , " & Str(Val(dClo_ID)) & ",  " & Str(Val(txt_Folding.Text)) & ", " & Str(Val(Sno)) & ",  '" & Trim(.Rows(i).Cells(1).Value) & "', '" & Trim(.Rows(i).Cells(2).Value) & "', " & Str(Val(dCloTyp_ID)) & ", " & Str(Val(.Rows(i).Cells(4).Value)) & ", " & Str(Val(.Rows(i).Cells(5).Value)) & ", " & Str(Val(.Rows(i).Cells(6).Value)) & ", " & Str(Val(party_ID)) & " , '" & Trim(.Rows(i).Cells(8).Value) & "' , " & Str(Val(vLmIdNo)) & " , '" & Trim(vLmNo) & "' ,'" & Trim(lbl_PackingSlipCode.Text) & "') "
                        cmd.ExecuteNonQuery()




                        If dCloTyp_ID = 1 Then
                            nr = 0
                            cmd.CommandText = "update Weaver_ClothReceipt_Piece_Details set  Bale_UnPacking_Code_Type1 = '" & Trim(Pk_Condition) & Trim(NewCode) & "' ,Bale_UnPacking_Increment_Type1 =Bale_UnPacking_Increment_Type1 +1 ,PackingSlip_Code_Type1 = '', PackingSlip_Inc_Type1 = PackingSlip_Inc_Type1 - 1  Where PackingSlip_Code_Type1 = '" & Trim(lbl_PackingSlipCode.Text) & "' and lot_code = '" & Trim(.Rows(i).Cells(8).Value) & "' and Piece_No = '" & Trim(.Rows(i).Cells(2).Value) & "'"
                            nr = cmd.ExecuteNonQuery()

                        ElseIf dCloTyp_ID = 2 Then
                            cmd.CommandText = "update Weaver_ClothReceipt_Piece_Details set   Bale_UnPacking_Code_Type2 = '" & Trim(Pk_Condition) & Trim(NewCode) & "', Bale_UnPacking_Increment_Type2 =Bale_UnPacking_Increment_Type2 +1 ,PackingSlip_Code_Type2 = '', PackingSlip_Inc_Type2 = PackingSlip_Inc_Type2 - 1 Where PackingSlip_Code_Type2 ='" & Trim(lbl_PackingSlipCode.Text) & "' and lot_code = '" & Trim(.Rows(i).Cells(8).Value) & "' and Piece_No = '" & Trim(.Rows(i).Cells(2).Value) & "'"
                            cmd.ExecuteNonQuery()

                        ElseIf dCloTyp_ID = 3 Then
                            cmd.CommandText = "update Weaver_ClothReceipt_Piece_Details set   Bale_UnPacking_Code_Type3 = '" & Trim(Pk_Condition) & Trim(NewCode) & "',Bale_UnPacking_Increment_Type3 =Bale_UnPacking_Increment_Type3 +1 ,PackingSlip_Code_Type3 = '', PackingSlip_Inc_Type3 = PackingSlip_Inc_Type3 - 1 Where PackingSlip_Code_Type3 = '" & Trim(lbl_PackingSlipCode.Text) & "' and lot_code = '" & Trim(.Rows(i).Cells(8).Value) & "' and Piece_No = '" & Trim(.Rows(i).Cells(2).Value) & "'"
                            cmd.ExecuteNonQuery()

                        ElseIf dCloTyp_ID = 4 Then
                            cmd.CommandText = "update Weaver_ClothReceipt_Piece_Details set   Bale_UnPacking_Code_Type4 = '" & Trim(Pk_Condition) & Trim(NewCode) & "' ,Bale_UnPacking_Increment_Type4 =Bale_UnPacking_Increment_Type4 +1 , PackingSlip_Code_Type4 = '', PackingSlip_Inc_Type4 = PackingSlip_Inc_Type4 - 1  Where PackingSlip_Code_Type4 ='" & Trim(lbl_PackingSlipCode.Text) & "' and lot_code = '" & Trim(.Rows(i).Cells(8).Value) & "' and Piece_No = '" & Trim(.Rows(i).Cells(2).Value) & "'"
                            cmd.ExecuteNonQuery()

                        ElseIf dCloTyp_ID = 5 Then
                            cmd.CommandText = "update Weaver_ClothReceipt_Piece_Details set   Bale_UnPacking_Code_Type5 = '" & Trim(Pk_Condition) & Trim(NewCode) & "' ,Bale_UnPacking_Increment_Type5 =Bale_UnPacking_Increment_Type5 +1 ,PackingSlip_Code_Type5 = '', PackingSlip_Inc_Type5 = PackingSlip_Inc_Type5 - 1 Where PackingSlip_Code_Type5 = '" & Trim(lbl_PackingSlipCode.Text) & "' and lot_code = '" & Trim(.Rows(i).Cells(8).Value) & "' and Piece_No = '" & Trim(.Rows(i).Cells(2).Value) & "'"
                            cmd.ExecuteNonQuery()

                        End If


                    End If

                Next
            End With

            tr.Commit()

            MessageBox.Show("Saved Sucessfully!!!", "FOR SAVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Information)

            If Val(Common_Procedures.settings.OnSave_MoveTo_NewEntry_Status) = 1 Then
                If New_Entry = True Then
                    new_record()
                Else
                    move_record(lbl_UnPackNo.Text)
                End If

            Else
                move_record(lbl_UnPackNo.Text)

            End If


        Catch ex As Exception
            tr.Rollback()
            If (InStr(Trim(ex.Message), "CK_Weaver_ClothReceipt_Piece_Details_6")) > 0 Or (InStr(Trim(ex.Message), "CK_Weaver_ClothReceipt_Piece_Details_7")) Or (InStr(Trim(ex.Message), "CK_Weaver_ClothReceipt_Piece_Details_8")) Or (InStr(Trim(ex.Message), "CK_Weaver_ClothReceipt_Piece_Details_9")) Or (InStr(Trim(ex.Message), "CK_Weaver_ClothReceipt_Piece_Details_9")) Then
                MessageBox.Show("Already Bale UnPacked!", "FOR SAVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning)
            
            Else
                MessageBox.Show(ex.Message, "FOR SAVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            End If





        Finally
            Dt1.Dispose()
            Da.Dispose()
            tr.Dispose()
            cmd.Dispose()

            If msk_date.Enabled And msk_date.Visible Then msk_date.Focus()

        End Try

    End Sub

    Private Sub Total_Calculation()
        Dim Sno As Integer
        Dim TotMtrs As Single, TotPcs As Single, TotWgt As Single

        If FrmLdSTS = True Then Exit Sub

        Sno = 0
        TotPcs = 0
        TotMtrs = 0
        TotWgt = 0

        With dgv_Details
            For i = 0 To .RowCount - 1
                Sno = Sno + 1
                .Rows(i).Cells(0).Value = Sno
                If Val(.Rows(i).Cells(4).Value) <> 0 Then
                    TotPcs = TotPcs + 1
                    TotMtrs = TotMtrs + Val(.Rows(i).Cells(4).Value)
                    TotWgt = TotWgt + Val(.Rows(i).Cells(5).Value)

                End If
            Next
        End With

        With dgv_Details_Total
            If .RowCount = 0 Then .Rows.Add()
            .Rows(0).Cells(2).Value = Val(TotPcs)
            .Rows(0).Cells(4).Value = Format(Val(TotMtrs), "########0.00")
            .Rows(0).Cells(5).Value = Format(Val(TotWgt), "########0.000")

        End With

    End Sub

    Private Sub cbo_Cloth_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Cloth.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Cloth_Head", "Cloth_Name", "", "Cloth_Name")

    End Sub
    Private Sub cbo_Cloth_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Cloth.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Cloth, cbo_Godown_StockIN, cbo_ClothType, "Cloth_Head", "Cloth_Name", "", "Cloth_Name")
        If e.KeyCode = 38 And cbo_Cloth.DroppedDown = False Or (e.Control = True And e.KeyCode = 38) Then
            If cbo_Godown_StockIN.Visible = True Then
                cbo_Godown_StockIN.Focus()
            Else
                cbo_PartyName_StockOF.Focus()
            End If
        End If

    End Sub

    Private Sub cbo_Cloth_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Cloth.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Cloth, cbo_ClothType, "Cloth_Head", "Cloth_Name", "", "Cloth_Name")
    End Sub

    Private Sub cbo_Cloth_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Cloth.KeyUp
        If e.Control = False And e.KeyValue = 17 Then
            Dim f As New Cloth_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Cloth.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""
            f.MdiParent = MDIParent1
            f.Show()

        End If
    End Sub

    Private Sub cbo_ClothType_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_ClothType.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "ClothType_Head", "ClothType_Name", "", "ClothType_Name")

    End Sub
    Private Sub cbo_ClothType_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_ClothType.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_ClothType, cbo_Cloth, txt_Folding, "ClothType_Head", "ClothType_Name", "", "ClothType_Name")

    End Sub

    Private Sub cbo_ClothType_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_ClothType.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_ClothType, txt_Folding, "ClothType_Head", "ClothType_Name", "", "ClothType_Name")
    End Sub

    'Private Sub cbo_Bale_Bundle_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs)
    '    Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "", "", "", "")

    'End Sub
    'Private Sub cbo_Bale_Bundle_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs)
    '    Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Bale_Bundle, cbo_ClothType, txt_Folding, "", "", "", "")

    'End Sub

    'Private Sub cbo_Bale_Bundle_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
    '    Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Bale_Bundle, txt_Folding, "", "", "", "")
    'End Sub

    Private Sub txt_Folding_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_Folding.KeyDown
        If e.KeyValue = 38 Then e.Handled = True : SendKeys.Send("+{TAB}")
        If e.KeyValue = 40 Then
            e.Handled = True
            If MessageBox.Show("Do you want to select Piece", "FOR PIECE SELECTION...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = DialogResult.Yes Then
                btn_Selection_Click(sender, e)
            Else
                txt_Note.Focus()
            End If
        End If
    End Sub

    Private Sub txt_Folding_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Folding.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
            e.Handled = True
        End If
        If Asc(e.KeyChar) = 13 Then

            If MessageBox.Show("Do you want to select Piece", "FOR PIECE SELECTION...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = DialogResult.Yes Then
                btn_Selection_Click(sender, e)
            Else
                txt_Note.Focus()
            End If
            'If dgv_Details.Rows.Count > 0 Then
            '    dgv_Details.Focus()
            '    dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(1)

            'Else
            '    txt_Note.Focus()

            'End If
        End If
    End Sub

    Private Sub txt_Note_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_Note.KeyDown
        If (e.KeyValue = 40) Then
            If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                save_record()
            Else
                msk_date.Focus()
            End If
        End If
        If (e.KeyValue = 38) Then
            txt_Folding.Focus()
        End If

    End Sub

    Private Sub txt_Note_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Note.KeyPress
        If Asc(e.KeyChar) = 13 Then
            If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                save_record()
            Else
                msk_date.Focus()
            End If
        End If
    End Sub

    Private Sub dgv_Details_CellEndEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellEndEdit

        If dgv_Details.CurrentCell.ColumnIndex = 2 Or dgv_Details.CurrentCell.ColumnIndex = 4 Or dgv_Details.CurrentCell.ColumnIndex = 5 Then
            Total_Calculation()
        End If

    End Sub

    Private Sub dgv_Details_CellLeave(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellLeave
        With dgv_Details
            If .CurrentCell.ColumnIndex = 6 And .CurrentCell.ColumnIndex = 5 Then
                If Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value) <> 0 Then
                    .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = Format(Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value), "#########0.000")
                End If
            End If
        End With
    End Sub

    Private Sub dgv_Details_CellValueChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellValueChanged

        Try
            If IsNothing(dgv_Details.CurrentCell) Then Exit Sub

            With dgv_Details
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

    Private Sub dgv_Details_EditingControlShowing(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewEditingControlShowingEventArgs) Handles dgv_Details.EditingControlShowing
        dgtxt_Details = CType(dgv_Details.EditingControl, DataGridViewTextBoxEditingControl)
    End Sub

    Private Sub dgv_Details_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_Details.KeyDown
        With dgv_Details

            If e.KeyCode = Keys.Up Then
                If .CurrentCell.RowIndex = 0 Then
                    .CurrentCell.Selected = False
                    txt_Folding.Focus()
                End If
            End If
            If e.KeyCode = Keys.Left Then
                If .CurrentCell.RowIndex = 0 And .CurrentCell.ColumnIndex <= 0 Then
                    .CurrentCell.Selected = False
                    txt_Folding.Focus()
                    'SendKeys.Send("{RIGHT}")
                End If
            End If

            If e.KeyCode = Keys.Enter Then
                e.SuppressKeyPress = True
                e.Handled = True

                If .CurrentCell.RowIndex = .RowCount - 1 And .CurrentCell.ColumnIndex >= 1 And Trim(.CurrentRow.Cells(1).Value) = "" Then

                    txt_Note.Focus()

                Else
                    SendKeys.Send("{Tab}")

                End If


            End If

        End With

    End Sub

    Private Sub dgv_Details_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_Details.KeyUp
        Dim i As Integer

        If e.Control = True And UCase(Chr(e.KeyCode)) = "D" Then

            With dgv_Details

                .Rows.RemoveAt(.CurrentRow.Index)

                For i = 0 To .Rows.Count - 1
                    .Rows(i).Cells(0).Value = i + 1
                Next

            End With
        End If
    End Sub

    Private Sub dgv_Details_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgv_Details.LostFocus
        On Error Resume Next
        If Not IsNothing(dgv_Details.CurrentCell) Then dgv_Details.CurrentCell.Selected = False

    End Sub

    Private Sub dgv_Details_RowsAdded(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewRowsAddedEventArgs) Handles dgv_Details.RowsAdded
        Dim n As Integer
        If IsNothing(dgv_Details.CurrentCell) Then Exit Sub

        With dgv_Details
            n = .RowCount
            .Rows(n - 1).Cells(0).Value = Val(n)

        End With
    End Sub
    Private Sub btn_close_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_close.Click
        Me.Close()
    End Sub

    Private Sub btn_save_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_save.Click
        save_record()
    End Sub
    Private Sub dgtxt_Details_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgtxt_Details.Enter
        dgv_Details.EditingControl.BackColor = Color.Lime
        dgv_Details.EditingControl.ForeColor = Color.Blue
        dgtxt_Details.SelectAll()
    End Sub
    Private Sub dgtxt_Details_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dgtxt_Details.KeyPress
        With dgv_Details
            If .Visible Then
                If .CurrentCell.ColumnIndex = 4 Or .CurrentCell.ColumnIndex = 5 Or .CurrentCell.ColumnIndex = 6 Then

                    If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
                        e.Handled = True
                    End If

                End If
            End If
        End With
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
        Dim n As Integer, i As Integer
        Dim Led_IdNo As Integer, Cnt_IdNo As Integer
        Dim Condt As String = ""
        Dim EdsCnt_IdNo As Integer, Mil_IdNo As Integer

        Try

            Condt = ""
            Led_IdNo = 0
            Cnt_IdNo = 0
            Mil_IdNo = 0
            EdsCnt_IdNo = 0

            If IsDate(dtp_Filter_Fromdate.Value) = True And IsDate(dtp_Filter_ToDate.Value) = True Then
                Condt = "a.Bale_UnPacking_Date between '" & Trim(Format(dtp_Filter_Fromdate.Value, "MM/dd/yyyy")) & "' and '" & Trim(Format(dtp_Filter_ToDate.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_Filter_Fromdate.Value) = True Then
                Condt = "a.Bale_UnPacking_Date = '" & Trim(Format(dtp_Filter_Fromdate.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_Filter_ToDate.Value) = True Then
                Condt = "a.Bale_UnPacking_Date = '" & Trim(Format(dtp_Filter_ToDate.Value, "MM/dd/yyyy")) & "' "
            End If

            If Trim(cbo_Filter_Cloth.Text) <> "" Then
                Led_IdNo = Common_Procedures.Cloth_NameToIdNo(con, cbo_Filter_Cloth.Text)
            End If

            If Trim(cbo_Filter_ClothType.Text) <> "" Then
                Cnt_IdNo = Common_Procedures.ClothType_NameToIdNo(con, cbo_Filter_ClothType.Text)
            End If



            If Val(Led_IdNo) <> 0 Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & "a.Cloth_IdNo = " & Str(Val(Led_IdNo))
            End If

            If Val(Cnt_IdNo) <> 0 Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & "a.ClothType_IdNo = " & Str(Val(Cnt_IdNo))
            End If



            da = New SqlClient.SqlDataAdapter("select a.* , b.Cloth_name from Bale_UnPacking_Head a Inner join Cloth_Head b on a.cloth_idno = b.cloth_idno where a.company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Bale_UnPacking_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "'  and a.Bale_UnPacking_Code NOT LIKE '" & Trim(PkCondition_RollPacking) & "%'  and a.Bale_UnPacking_Code NOT LIKE '" & Trim(PkCondition_BaleDirectEntry) & "%' " & IIf(Trim(Condt) <> "", " and ", "") & Condt & " Order by a.for_orderby, a.Bale_UnPacking_No", con)
            da.Fill(dt2)

            dgv_Filter_Details.Rows.Clear()

            If dt2.Rows.Count > 0 Then

                For i = 0 To dt2.Rows.Count - 1

                    n = dgv_Filter_Details.Rows.Add()

                    dgv_Filter_Details.Rows(n).Cells(0).Value = i + 1
                    dgv_Filter_Details.Rows(n).Cells(1).Value = dt2.Rows(i).Item("Bale_UnPacking_No").ToString
                    dgv_Filter_Details.Rows(n).Cells(2).Value = Format(Convert.ToDateTime(dt2.Rows(i).Item("Bale_UnPacking_Date").ToString), "dd-MM-yyyy")
                    dgv_Filter_Details.Rows(n).Cells(3).Value = dt2.Rows(i).Item("Cloth_Name").ToString
                    dgv_Filter_Details.Rows(n).Cells(4).Value = dt2.Rows(i).Item("Total_Pcs").ToString
                    dgv_Filter_Details.Rows(n).Cells(5).Value = Format(Val(dt2.Rows(i).Item("Total_Meters").ToString), "########0.00")
                    dgv_Filter_Details.Rows(n).Cells(6).Value = Format(Val(dt2.Rows(i).Item("Total_Weight").ToString), "########0.000")

                Next i

            End If

            dt2.Clear()
            dt2.Dispose()
            da.Dispose()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT FILTER...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

        If dgv_Filter_Details.Visible And dgv_Filter_Details.Enabled Then dgv_Filter_Details.Focus()

    End Sub


    Private Sub dtp_Filter_Fromdate_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dtp_Filter_Fromdate.KeyPress
        If Asc(e.KeyChar) = 13 Then
            dtp_Filter_ToDate.Focus()
        End If

    End Sub

    Private Sub dtp_Filter_ToDate_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dtp_Filter_ToDate.KeyPress

        If Asc(e.KeyChar) = 13 Then
            cbo_Filter_Cloth.Focus()
        End If
    End Sub

    Private Sub cbo_Filter_Cloth_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Filter_Cloth.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Cloth_Head", "Cloth_Name", "", "Cloth_Name")

    End Sub

    Private Sub cbo_Filter_Cloth_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Filter_Cloth.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_Cloth, dtp_Filter_ToDate, cbo_Filter_ClothType, "Cloth_Head", "Cloth_Name", "", "Cloth_Name")

    End Sub

    Private Sub cbo_Filter_Cloth_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Filter_Cloth.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Filter_Cloth, cbo_Filter_ClothType, "Cloth_Head", "Cloth_Name", "", "Cloth_Name")

    End Sub

    Private Sub cbo_Filter_ClothType_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Filter_ClothType.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "ClothType_Head", "ClothType_Name", "", "ClothType_Name")

    End Sub

    Private Sub cbo_Filter_ClothType_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Filter_ClothType.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_ClothType, cbo_Filter_Cloth, btn_Filter_Show, "ClothType_Head", "ClothType_Name", "", "ClothType_Name")

    End Sub

    Private Sub cbo_Filter_ClothType_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Filter_ClothType.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Filter_ClothType, btn_Filter_Show, "ClothType_Head", "ClothType_Name", "", "ClothType_Name")

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

    Private Sub chk_SelectAll_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim i As Integer
        Dim J As Integer
        Dim v1stVisiRow As Integer = 0

        With dgv_Selection

            For i = 0 To .Rows.Count - 1
                If .Rows(i).Visible = True Then
                    .Rows(i).Cells(8).Value = ""
                    For J = 0 To .ColumnCount - 1
                        .Rows(i).Cells(J).Style.ForeColor = Color.Black
                    Next J
                End If
            Next i

            v1stVisiRow = -1
            'If chk_SelectAll.Checked = True Then
            '    For i = 0 To .Rows.Count - 1
            '        If .Rows(i).Visible = True Then
            '            Select_Piece(i)
            '            If v1stVisiRow = -1 Then v1stVisiRow = i
            '        End If
            '    Next i
            'End If

            If .Rows.Count > 0 Then

                If v1stVisiRow >= 0 Then
                    .Focus()
                    .CurrentCell = .Rows(v1stVisiRow).Cells(0)
                    .CurrentCell.Selected = True
                Else
                    txt_BaleSelection.Focus()
                End If

            End If

        End With

    End Sub

    Private Sub btn_Selection_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Selection.Click
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim i As Integer, j As Integer, n As Integer, SNo As Integer
        Dim CloIdNo As Integer, CloTypIdNo As Integer
        Dim NewCode As String
        Dim CompIDCondt As String
        Dim vLed_IdNo As Integer = 0
        Dim Clo_GrpIdNos As String
        Dim Clo_TypId As Integer = 0
        Dim Clo_UndIdNo As Integer
        Dim CloID_Cond As String = ""
        Dim PcsMtrs As Double = 0
        Dim vLmIdNo As Long = 0
        Dim vLmNo As String = ""
        Dim vGod_ID As Integer = 0


        vLed_IdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_PartyName_StockOF.Text)
        If cbo_PartyName_StockOF.Visible = True Then
            If vLed_IdNo = 0 Then
                MessageBox.Show("Invalid StockOf (OwnSort / JobworkerName)", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                If cbo_PartyName_StockOF.Enabled And cbo_PartyName_StockOF.Visible Then cbo_PartyName_StockOF.Focus()
                Exit Sub
            End If
        End If
        If vLed_IdNo = 0 Then vLed_IdNo = Common_Procedures.CommonLedger.OwnSort_Ac

        vGod_ID = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Godown_StockIN.Text)
        If cbo_Godown_StockIN.Visible = True Then
            If vGod_ID = 0 Then
                MessageBox.Show("Invalid Fabric Godown Name", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                If cbo_Godown_StockIN.Enabled And cbo_Godown_StockIN.Visible Then cbo_Godown_StockIN.Focus()
                Exit Sub
            End If
        End If
        If vGod_ID = 0 Then vGod_ID = Common_Procedures.CommonLedger.Godown_Ac


        CloIdNo = Common_Procedures.Cloth_NameToIdNo(con, cbo_Cloth.Text)
        If CloIdNo = 0 Then
            MessageBox.Show("Invalid Cloth Name", "DOES NOT SELECT PAVU...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If cbo_Cloth.Enabled And cbo_Cloth.Visible Then cbo_Cloth.Focus()
            Exit Sub
        End If

        CloTypIdNo = Common_Procedures.ClothType_NameToIdNo(con, cbo_ClothType.Text)
        If CloTypIdNo = 0 Then
            MessageBox.Show("Invalid Cloth Type", "DOES NOT SELECT PAVU...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If cbo_ClothType.Enabled And cbo_ClothType.Visible Then cbo_ClothType.Focus()
            Exit Sub
        End If

        If Val(txt_Folding.Text) = 0 Then
            MessageBox.Show("Invalid Folding", "DOES NOT SELECT PAVU...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If txt_Folding.Enabled And txt_Folding.Visible Then txt_Folding.Focus()
            Exit Sub
        End If

        CompIDCondt = "(a.company_idno = " & Str(Val(lbl_Company.Tag)) & ")"
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1155" And Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1204" Then '----KRG TEXTILE MILLS (PALLADAM)
            If Common_Procedures.settings.EntrySelection_Combine_AllCompany = 1 Then
                CompIDCondt = ""
                If Trim(UCase(Common_Procedures.User.Type)) = "ACCOUNT" Then
                    CompIDCondt = "(Company_Type <> 'UNACCOUNT')"
                End If
            End If
        End If

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_UnPackNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)



        Clo_UndIdNo = CloIdNo

        Da = New SqlClient.SqlDataAdapter("select * from Cloth_head where Cloth_idno = " & Str(Val(Clo_UndIdNo)), con)
        Dt1 = New DataTable
        Da.Fill(Dt1)
        If Dt1.Rows.Count > 0 Then
            If IsDBNull(Dt1.Rows(0).Item("Cloth_StockUnder_IdNo").ToString) = False Then
                If Val(Dt1.Rows(0).Item("Cloth_StockUnder_IdNo").ToString) <> 0 Then Clo_UndIdNo = Val(Dt1.Rows(0).Item("Cloth_StockUnder_IdNo").ToString)
            End If
        End If
        Dt1.Clear()

        Da = New SqlClient.SqlDataAdapter("select * from Cloth_head where Cloth_StockUnder_IdNo = " & Str(Val(Clo_UndIdNo)), con)
        Dt1 = New DataTable
        Da.Fill(Dt1)

        Clo_GrpIdNos = ""
        If Dt1.Rows.Count > 0 Then
            For i = 0 To Dt1.Rows.Count - 1
                Clo_GrpIdNos = Trim(Clo_GrpIdNos) & IIf(Trim(Clo_GrpIdNos) <> "", ", ", "") & Trim(Val(Dt1.Rows(i).Item("Cloth_IdNo")))
            Next
        End If
        If Trim(Clo_GrpIdNos) <> "" Then
            Clo_GrpIdNos = "(" & Clo_GrpIdNos & ")"
        Else
            Clo_GrpIdNos = "(" & Trim(Val(CloIdNo)) & ")"
        End If

        CloID_Cond = "(a.Cloth_idno = " & Str(CloIdNo) & " or a.Cloth_idno IN " & Trim(Clo_GrpIdNos) & ")"

        Clo_TypId = Val(Common_Procedures.ClothType_NameToIdNo(con, Trim(cbo_ClothType.Text)))

        If Val(Clo_TypId) <> 0 Then
            CloID_Cond = CloID_Cond & " and a.ClothType_Idno = " & Clo_TypId
        End If


        'If cbo_Godown_StockIN.Visible = True Then
        '    CloID_Cond = CloID_Cond & IIf(CloID_Cond <> "", " and ", " ") & "(a.WareHouse_idno = " & Str(vGod_ID) & ")"
        'End If

        'If vLed_IdNo = 4 Or vLed_IdNo = 5 Then
        '    CloID_Cond = CloID_Cond & IIf(CloID_Cond <> "", " and ", " ") & "(a.StockOff_IdNo = 4 or a.StockOff_IdNo = 5)"
        'Else
        '    CloID_Cond = CloID_Cond & IIf(CloID_Cond <> "", " and ", " ") & "(a.StockOff_IdNo = " & Str(vLed_IdNo) & ")"
        'End If

        With dgv_Selection
            '  chk_SelectAll.Checked = False
            .Rows.Clear()
            SNo = 0

            '    If CloTypIdNo = 1 Then

            Da = New SqlClient.SqlDataAdapter("select a.* , c.Ledger_Name, d.cloth_name , Ct.* from Packing_Slip_Head a  INNER JOIN Company_Head tZ ON a.company_idno = tZ.company_idno LEFT OUTER JOIN Ledger_Head c ON a.Ledger_IdNo = c.Ledger_IdNo LEFT OUTER JOIN Cloth_Head d ON a.Cloth_IdNo <> 0 and a.Cloth_IdNo = d.Cloth_IdNo left outer join ClothType_Head ct ON a.ClothType_IdNo = ct.ClothType_IdNo  Where " & CompIDCondt & IIf(Trim(CompIDCondt) <> "", " and ", "") & " a.Total_Meters <> 0 and a.Delivery_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' and " & CloID_Cond & IIf(CloID_Cond <> "", " and ", " ") & "  a.Folding = " & Str(Val(txt_Folding.Text)) & " order by a.Packing_Slip_Date, a.for_orderby, a.Packing_Slip_No", con)
            Dt1 = New DataTable
            Da.Fill(Dt1)

            If Dt1.Rows.Count > 0 Then

                For i = 0 To Dt1.Rows.Count - 1

                    n = .Rows.Add()

                    SNo = SNo + 1
                    .Rows(n).Cells(0).Value = Val(SNo)

                    .Rows(n).Cells(1).Value = Dt1.Rows(i).Item("Packing_Slip_no").ToString
                    .Rows(n).Cells(2).Value = Dt1.Rows(i).Item("Packing_Slip_Date")
                    .Rows(n).Cells(3).Value = Dt1.Rows(i).Item("ClothType_Name").ToString
                    .Rows(n).Cells(4).Value = Dt1.Rows(i).Item("cloth_name").ToString

                    .Rows(n).Cells(5).Value = Format(Val(Dt1.Rows(i).Item("Total_Meters").ToString), "#########0.00")
                    .Rows(n).Cells(6).Value = Format(Val(Dt1.Rows(i).Item("Total_Weight").ToString), "#########0.000")


                    .Rows(n).Cells(8).Value = "1"
                    .Rows(n).Cells(9).Value = Dt1.Rows(i).Item("Packing_Slip_Code").ToString
                    .Rows(n).Cells(10).Value = Dt1.Rows(i).Item("Cloth_IdNo").ToString
                    .Rows(n).Cells(11).Value = Dt1.Rows(i).Item("Delivery_Code").ToString





                    For j = 0 To .ColumnCount - 1
                        .Rows(i).Cells(j).Style.ForeColor = Color.Red
                    Next

                Next

            End If
            Dt1.Clear()

            Da = New SqlClient.SqlDataAdapter("select a.* , c.Ledger_Name, d.cloth_name ,Ct.* from Packing_Slip_Head a  INNER JOIN Company_Head tZ ON a.company_idno = tZ.company_idno LEFT OUTER JOIN Ledger_Head c ON a.Ledger_IdNo = c.Ledger_IdNo LEFT OUTER JOIN Cloth_Head d ON a.Cloth_IdNo <> 0 and a.Cloth_IdNo = d.Cloth_IdNo left outer join ClothType_Head ct ON a.ClothType_IdNo = ct.ClothType_IdNo Where " & CompIDCondt & IIf(Trim(CompIDCondt) <> "", " and ", "") & " a.Total_Meters <> 0 and a.Delivery_Code = '' and " & CloID_Cond & IIf(CloID_Cond <> "", " and ", " ") & "  a.Folding = " & Str(Val(txt_Folding.Text)) & " order by a.Packing_Slip_Date, a.for_orderby, a.Packing_Slip_No", con)
            Dt1 = New DataTable
            Da.Fill(Dt1)

            If Dt1.Rows.Count > 0 Then

                For i = 0 To Dt1.Rows.Count - 1

                    n = .Rows.Add()

                    SNo = SNo + 1
                    .Rows(n).Cells(0).Value = Val(SNo)
                    .Rows(n).Cells(1).Value = Dt1.Rows(i).Item("Packing_Slip_No").ToString
                    .Rows(n).Cells(2).Value = Dt1.Rows(i).Item("Packing_Slip_Date")
                    .Rows(n).Cells(3).Value = Dt1.Rows(i).Item("ClothType_Name").ToString
                    .Rows(n).Cells(4).Value = Dt1.Rows(i).Item("cloth_name").ToString

                    .Rows(n).Cells(5).Value = Format(Val(Dt1.Rows(i).Item("Total_Meters").ToString), "#########0.00")
                    .Rows(n).Cells(6).Value = Format(Val(Dt1.Rows(i).Item("Total_Weight").ToString), "#########0.000")


                    .Rows(n).Cells(8).Value = ""
                    .Rows(n).Cells(9).Value = Dt1.Rows(i).Item("Packing_Slip_Code").ToString
                    .Rows(n).Cells(10).Value = Dt1.Rows(i).Item("Cloth_IdNo").ToString
                    .Rows(n).Cells(11).Value = Dt1.Rows(i).Item("Delivery_Code").ToString

                Next

            End If
            Dt1.Clear()

            '  End If
        End With

        'If CloTypIdNo = 1 Or CloTypIdNo = 2 Then

        '    Da = New SqlClient.SqlDataAdapter("select a.* , C.Ledger_Name, d.cloth_name from Weaver_ClothReceipt_Piece_Details a  INNER JOIN Company_Head tZ ON a.company_idno = tZ.company_idno LEFT OUTER JOIN Ledger_Head c ON a.Ledger_IdNo <> 0 and a.Ledger_IdNo = c.Ledger_IdNo LEFT OUTER JOIN Cloth_Head d ON a.Cloth_IdNo <> 0 and a.Cloth_IdNo = d.Cloth_IdNo where " & CompIDCondt & IIf(Trim(CompIDCondt) <> "", " and ", "") & " a.Type2_Meters <> 0 and a.PackingSlip_Code_Type2 = '" & Trim(NewCode) & "'  and " & CloID_Cond & IIf(CloID_Cond <> "", " and ", " ") & "  a.Folding = " & Str(Val(txt_Folding.Text)) & "   order by a.Weaver_ClothReceipt_Date, a.for_orderby, a.Weaver_ClothReceipt_No, a.PieceNo_OrderBy, a.Piece_No", con)
        '    Dt1 = New DataTable
        '    Da.Fill(Dt1)

        '    If Dt1.Rows.Count > 0 Then

        '        For i = 0 To Dt1.Rows.Count - 1

        '            n = .Rows.Add()

        '            SNo = SNo + 1
        '            .Rows(n).Cells(0).Value = Val(SNo)

        '            .Rows(n).Cells(1).Value = Dt1.Rows(i).Item("Lot_No").ToString
        '            .Rows(n).Cells(2).Value = Dt1.Rows(i).Item("Piece_No").ToString
        '            .Rows(n).Cells(3).Value = Common_Procedures.ClothType.Type2

        '            If Trim(Dt1.Rows(i).Item("BuyerOffer_Code_Type2").ToString) <> "" And Val(Dt1.Rows(i).Item("BuyerOffer_Passed_Meters_Type2").ToString) <> 0 Then
        '                PcsMtrs = Val(Dt1.Rows(i).Item("BuyerOffer_Passed_Meters_Type2").ToString)
        '            Else
        '                PcsMtrs = Val(Dt1.Rows(i).Item("Type2_Meters").ToString)
        '            End If

        '            .Rows(n).Cells(4).Value = Format(Val(PcsMtrs), "#########0.00")
        '            .Rows(n).Cells(5).Value = Format(Val(PcsMtrs) * Val(Dt1.Rows(i).Item("Weight_Meter").ToString), "#########0.000")
        '            .Rows(n).Cells(6).Value = Val(Dt1.Rows(i).Item("Weight_Meter").ToString)
        '            .Rows(n).Cells(7).Value = Dt1.Rows(i).Item("Ledger_Name").ToString
        '            .Rows(n).Cells(8).Value = "1"
        '            .Rows(n).Cells(9).Value = Dt1.Rows(i).Item("Lot_Code").ToString
        '            .Rows(n).Cells(10).Value = Dt1.Rows(i).Item("cloth_name").ToString

        '            vLmIdNo = 0
        '            If IsDBNull(Dt1.Rows(i).Item("Loom_IdNo").ToString) = False Then
        '                vLmIdNo = Val(Dt1.Rows(i).Item("Loom_IdNo").ToString)
        '            End If

        '            vLmNo = ""
        '            If vLmIdNo <> 0 Then
        '                vLmNo = Common_Procedures.Loom_IdNoToName(con, vLmIdNo)

        '            Else
        '                If IsDBNull(Dt1.Rows(i).Item("Loom_No").ToString) = False Then
        '                    vLmNo = Dt1.Rows(i).Item("Loom_No").ToString
        '                End If

        '            End If

        '            .Rows(n).Cells(11).Value = vLmNo

        '            For j = 0 To .ColumnCount - 1
        '                .Rows(i).Cells(j).Style.ForeColor = Color.Red
        '            Next

        '        Next

        '    End If
        '    Dt1.Clear()


        '        Da = New SqlClient.SqlDataAdapter("select a.* , c.Ledger_Name, d.cloth_name from Weaver_ClothReceipt_Piece_Details a  INNER JOIN Company_Head tZ ON a.company_idno = tZ.company_idno LEFT OUTER JOIN Ledger_Head c ON a.Ledger_IdNo = c.Ledger_IdNo LEFT OUTER JOIN Cloth_Head d ON a.Cloth_IdNo <> 0 and a.Cloth_IdNo = d.Cloth_IdNo where " & CompIDCondt & IIf(Trim(CompIDCondt) <> "", " and ", "") & " a.Type2_Meters <> 0 and a.PackingSlip_Code_Type2 = ''  and " & CloID_Cond & IIf(CloID_Cond <> "", " and ", " ") & " a.Folding = " & Str(Val(txt_Folding.Text)) & "  order by a.Weaver_ClothReceipt_Date, a.for_orderby, a.Weaver_ClothReceipt_No, a.PieceNo_OrderBy, a.Piece_No", con)
        '        Dt1 = New DataTable
        '        Da.Fill(Dt1)

        '        If Dt1.Rows.Count > 0 Then

        '            For i = 0 To Dt1.Rows.Count - 1

        '                n = .Rows.Add()

        '                SNo = SNo + 1
        '                .Rows(n).Cells(0).Value = Val(SNo)

        '                .Rows(n).Cells(1).Value = Dt1.Rows(i).Item("Lot_No").ToString
        '                .Rows(n).Cells(2).Value = Dt1.Rows(i).Item("Piece_No").ToString
        '                .Rows(n).Cells(3).Value = Common_Procedures.ClothType.Type2

        '                If Trim(Dt1.Rows(i).Item("BuyerOffer_Code_Type2").ToString) <> "" And Val(Dt1.Rows(i).Item("BuyerOffer_Passed_Meters_Type2").ToString) <> 0 Then
        '                    PcsMtrs = Val(Dt1.Rows(i).Item("BuyerOffer_Passed_Meters_Type2").ToString)
        '                Else
        '                    PcsMtrs = Val(Dt1.Rows(i).Item("Type2_Meters").ToString)
        '                End If

        '                .Rows(n).Cells(4).Value = Format(Val(PcsMtrs), "#########0.00")
        '                .Rows(n).Cells(5).Value = Format(Val(PcsMtrs) * Val(Dt1.Rows(i).Item("Weight_Meter").ToString), "#########0.000")
        '                .Rows(n).Cells(6).Value = Val(Dt1.Rows(i).Item("Weight_Meter").ToString)
        '                .Rows(n).Cells(7).Value = Dt1.Rows(i).Item("Ledger_Name").ToString
        '                .Rows(n).Cells(8).Value = ""
        '                .Rows(n).Cells(9).Value = Dt1.Rows(i).Item("Lot_Code").ToString
        '                .Rows(n).Cells(10).Value = Dt1.Rows(i).Item("cloth_name").ToString

        '                vLmIdNo = 0
        '                If IsDBNull(Dt1.Rows(i).Item("Loom_IdNo").ToString) = False Then
        '                    vLmIdNo = Val(Dt1.Rows(i).Item("Loom_IdNo").ToString)
        '                End If

        '                vLmNo = ""
        '                If vLmIdNo <> 0 Then
        '                    vLmNo = Common_Procedures.Loom_IdNoToName(con, vLmIdNo)

        '                Else
        '                    If IsDBNull(Dt1.Rows(i).Item("Loom_No").ToString) = False Then
        '                        vLmNo = Dt1.Rows(i).Item("Loom_No").ToString
        '                    End If

        '                End If

        '                .Rows(n).Cells(11).Value = vLmNo

        '            Next

        '        End If
        '        Dt1.Clear()
        '    End If

        '    If CloTypIdNo = 1 Or CloTypIdNo = 3 Then

        '        Da = New SqlClient.SqlDataAdapter("select a.* , c.Ledger_Name, d.cloth_name from Weaver_ClothReceipt_Piece_Details a  INNER JOIN Company_Head tZ ON a.company_idno = tZ.company_idno LEFT OUTER JOIN Ledger_Head c ON a.Ledger_IdNo = c.Ledger_IdNo LEFT OUTER JOIN Cloth_Head d ON a.Cloth_IdNo <> 0 and a.Cloth_IdNo = d.Cloth_IdNo where " & CompIDCondt & IIf(Trim(CompIDCondt) <> "", " and ", "") & " a.Type3_Meters <> 0 and a.PackingSlip_Code_Type3 = '" & Trim(NewCode) & "'  and " & CloID_Cond & IIf(CloID_Cond <> "", " and ", " ") & "  a.Folding = " & Str(Val(txt_Folding.Text)) & "  order by a.Weaver_ClothReceipt_Date, a.for_orderby, a.Weaver_ClothReceipt_No, a.PieceNo_OrderBy, a.Piece_No", con)
        '        Dt1 = New DataTable
        '        Da.Fill(Dt1)

        '        If Dt1.Rows.Count > 0 Then

        '            For i = 0 To Dt1.Rows.Count - 1

        '                n = .Rows.Add()

        '                SNo = SNo + 1
        '                .Rows(n).Cells(0).Value = Val(SNo)

        '                .Rows(n).Cells(1).Value = Dt1.Rows(i).Item("Lot_No").ToString
        '                .Rows(n).Cells(2).Value = Dt1.Rows(i).Item("Piece_No").ToString
        '                .Rows(n).Cells(3).Value = Common_Procedures.ClothType.Type3

        '                If Trim(Dt1.Rows(i).Item("BuyerOffer_Code_Type3").ToString) <> "" And Val(Dt1.Rows(i).Item("BuyerOffer_Passed_Meters_Type3").ToString) <> 0 Then
        '                    PcsMtrs = Val(Dt1.Rows(i).Item("BuyerOffer_Passed_Meters_Type3").ToString)
        '                Else
        '                    PcsMtrs = Val(Dt1.Rows(i).Item("Type3_Meters").ToString)
        '                End If

        '                .Rows(n).Cells(4).Value = Format(Val(PcsMtrs), "#########0.00")
        '                .Rows(n).Cells(5).Value = Format(Val(PcsMtrs) * Val(Dt1.Rows(i).Item("Weight_Meter").ToString), "#########0.000")
        '                .Rows(n).Cells(6).Value = Val(Dt1.Rows(i).Item("Weight_Meter").ToString)
        '                .Rows(n).Cells(7).Value = Dt1.Rows(i).Item("Ledger_Name").ToString
        '                .Rows(n).Cells(8).Value = "1"
        '                .Rows(n).Cells(9).Value = Dt1.Rows(i).Item("Lot_Code").ToString
        '                .Rows(n).Cells(10).Value = Dt1.Rows(i).Item("cloth_name").ToString

        '                vLmIdNo = 0
        '                If IsDBNull(Dt1.Rows(i).Item("Loom_IdNo").ToString) = False Then
        '                    vLmIdNo = Val(Dt1.Rows(i).Item("Loom_IdNo").ToString)
        '                End If

        '                vLmNo = ""
        '                If vLmIdNo <> 0 Then
        '                    vLmNo = Common_Procedures.Loom_IdNoToName(con, vLmIdNo)

        '                Else
        '                    If IsDBNull(Dt1.Rows(i).Item("Loom_No").ToString) = False Then
        '                        vLmNo = Dt1.Rows(i).Item("Loom_No").ToString
        '                    End If

        '                End If

        '                .Rows(n).Cells(11).Value = vLmNo

        '                For j = 0 To .ColumnCount - 1
        '                    .Rows(i).Cells(j).Style.ForeColor = Color.Red
        '                Next

        '            Next

        '        End If
        '        Dt1.Clear()


        '        Da = New SqlClient.SqlDataAdapter("select a.* ,C.Ledger_Name, d.Cloth_Name from Weaver_ClothReceipt_Piece_Details a  INNER JOIN Company_Head tZ ON a.company_idno = tZ.company_idno LEFT OUTER JOIN Ledger_Head c ON a.Ledger_IdNo <> 0 and a.Ledger_IdNo = c.Ledger_IdNo  LEFT OUTER JOIN Cloth_Head d ON a.cloth_IdNo <> 0 and a.cloth_IdNo = d.cloth_IdNo where " & CompIDCondt & IIf(Trim(CompIDCondt) <> "", " and ", "") & " a.Type3_Meters <> 0 and a.PackingSlip_Code_Type3 = ''  and " & CloID_Cond & IIf(CloID_Cond <> "", " and ", " ") & "  a.Folding = " & Str(Val(txt_Folding.Text)) & "  order by a.Weaver_ClothReceipt_Date, a.for_orderby, a.Weaver_ClothReceipt_No, a.PieceNo_OrderBy, a.Piece_No", con)
        '        Dt1 = New DataTable
        '        Da.Fill(Dt1)

        '        If Dt1.Rows.Count > 0 Then

        '            For i = 0 To Dt1.Rows.Count - 1

        '                n = .Rows.Add()

        '                SNo = SNo + 1
        '                .Rows(n).Cells(0).Value = Val(SNo)

        '                .Rows(n).Cells(1).Value = Dt1.Rows(i).Item("Lot_No").ToString
        '                .Rows(n).Cells(2).Value = Dt1.Rows(i).Item("Piece_No").ToString
        '                .Rows(n).Cells(3).Value = Common_Procedures.ClothType.Type3

        '                If Trim(Dt1.Rows(i).Item("BuyerOffer_Code_Type3").ToString) <> "" And Val(Dt1.Rows(i).Item("BuyerOffer_Passed_Meters_Type3").ToString) <> 0 Then
        '                    PcsMtrs = Val(Dt1.Rows(i).Item("BuyerOffer_Passed_Meters_Type3").ToString)
        '                Else
        '                    PcsMtrs = Val(Dt1.Rows(i).Item("Type3_Meters").ToString)
        '                End If

        '                .Rows(n).Cells(4).Value = Format(Val(PcsMtrs), "#########0.00")
        '                .Rows(n).Cells(5).Value = Format(Val(PcsMtrs) * Val(Dt1.Rows(i).Item("Weight_Meter").ToString), "#########0.000")
        '                .Rows(n).Cells(6).Value = Val(Dt1.Rows(i).Item("Weight_Meter").ToString)
        '                .Rows(n).Cells(7).Value = Dt1.Rows(i).Item("Ledger_Name").ToString
        '                .Rows(n).Cells(8).Value = ""
        '                .Rows(n).Cells(9).Value = Dt1.Rows(i).Item("Lot_Code").ToString
        '                .Rows(n).Cells(10).Value = Dt1.Rows(i).Item("Cloth_Name").ToString

        '                vLmIdNo = 0
        '                If IsDBNull(Dt1.Rows(i).Item("Loom_IdNo").ToString) = False Then
        '                    vLmIdNo = Val(Dt1.Rows(i).Item("Loom_IdNo").ToString)
        '                End If

        '                vLmNo = ""
        '                If vLmIdNo <> 0 Then
        '                    vLmNo = Common_Procedures.Loom_IdNoToName(con, vLmIdNo)

        '                Else
        '                    If IsDBNull(Dt1.Rows(i).Item("Loom_No").ToString) = False Then
        '                        vLmNo = Dt1.Rows(i).Item("Loom_No").ToString
        '                    End If

        '                End If

        '                .Rows(n).Cells(11).Value = vLmNo

        '            Next

        '        End If
        '        Dt1.Clear()
        '    End If

        '    If CloTypIdNo = 1 Or CloTypIdNo = 4 Then

        '        Da = New SqlClient.SqlDataAdapter("select a.* , C.Ledger_Name, d.cloth_name from Weaver_ClothReceipt_Piece_Details a  INNER JOIN Company_Head tZ ON a.company_idno = tZ.company_idno LEFT OUTER JOIN Ledger_Head c ON a.Ledger_IdNo <> 0 and a.Ledger_IdNo = c.Ledger_IdNo LEFT OUTER JOIN Cloth_Head d ON d.Cloth_IdNo <> 0 and a.Cloth_IdNo = d.Cloth_IdNo where " & CompIDCondt & IIf(Trim(CompIDCondt) <> "", " and ", "") & " a.Type4_Meters <> 0 and a.PackingSlip_Code_Type4 = '" & Trim(NewCode) & "'  and " & CloID_Cond & IIf(CloID_Cond <> "", " and ", " ") & "  a.Folding = " & Str(Val(txt_Folding.Text)) & "  order by a.Weaver_ClothReceipt_Date, a.for_orderby, a.Weaver_ClothReceipt_No, a.PieceNo_OrderBy, a.Piece_No", con)
        '        Dt1 = New DataTable
        '        Da.Fill(Dt1)

        '        If Dt1.Rows.Count > 0 Then

        '            For i = 0 To Dt1.Rows.Count - 1

        '                n = .Rows.Add()

        '                SNo = SNo + 1
        '                .Rows(n).Cells(0).Value = Val(SNo)

        '                .Rows(n).Cells(1).Value = Dt1.Rows(i).Item("Lot_No").ToString
        '                .Rows(n).Cells(2).Value = Dt1.Rows(i).Item("Piece_No").ToString
        '                .Rows(n).Cells(3).Value = Common_Procedures.ClothType.Type4

        '                If Trim(Dt1.Rows(i).Item("BuyerOffer_Code_Type4").ToString) <> "" And Val(Dt1.Rows(i).Item("BuyerOffer_Passed_Meters_Type4").ToString) <> 0 Then
        '                    PcsMtrs = Val(Dt1.Rows(i).Item("BuyerOffer_Passed_Meters_Type4").ToString)
        '                Else
        '                    PcsMtrs = Val(Dt1.Rows(i).Item("Type4_Meters").ToString)
        '                End If

        '                .Rows(n).Cells(4).Value = Format(Val(PcsMtrs), "#########0.00")
        '                .Rows(n).Cells(5).Value = Format(Val(PcsMtrs) * Val(Dt1.Rows(i).Item("Weight_Meter").ToString), "#########0.000")
        '                .Rows(n).Cells(6).Value = Val(Dt1.Rows(i).Item("Weight_Meter").ToString)
        '                .Rows(n).Cells(7).Value = Dt1.Rows(i).Item("Ledger_Name").ToString
        '                .Rows(n).Cells(8).Value = "1"
        '                .Rows(n).Cells(9).Value = Dt1.Rows(i).Item("Lot_Code").ToString
        '                .Rows(n).Cells(10).Value = Dt1.Rows(i).Item("Cloth_Name").ToString

        '                vLmIdNo = 0
        '                If IsDBNull(Dt1.Rows(i).Item("Loom_IdNo").ToString) = False Then
        '                    vLmIdNo = Val(Dt1.Rows(i).Item("Loom_IdNo").ToString)
        '                End If

        '                vLmNo = ""
        '                If vLmIdNo <> 0 Then
        '                    vLmNo = Common_Procedures.Loom_IdNoToName(con, vLmIdNo)

        '                Else
        '                    If IsDBNull(Dt1.Rows(i).Item("Loom_No").ToString) = False Then
        '                        vLmNo = Dt1.Rows(i).Item("Loom_No").ToString
        '                    End If

        '                End If

        '                .Rows(n).Cells(11).Value = vLmNo

        '                For j = 0 To .ColumnCount - 1
        '                    .Rows(i).Cells(j).Style.ForeColor = Color.Red
        '                Next

        '            Next

        '        End If
        '        Dt1.Clear()


        '        Da = New SqlClient.SqlDataAdapter("select a.* , C.Ledger_Name, d.cloth_name from Weaver_ClothReceipt_Piece_Details a  INNER JOIN Company_Head tZ ON a.company_idno = tZ.company_idno  LEFT OUTER JOIN Ledger_Head c ON a.Ledger_IdNo <> 0 and a.Ledger_IdNo = c.Ledger_IdNo LEFT OUTER JOIN Cloth_Head d ON a.Cloth_IdNo <> 0 and a.Cloth_IdNo = d.Cloth_IdNo where " & CompIDCondt & IIf(Trim(CompIDCondt) <> "", " and ", "") & " a.Type4_Meters <> 0 and a.PackingSlip_Code_Type4 = ''  and " & CloID_Cond & IIf(CloID_Cond <> "", " and ", " ") & "  a.Folding = " & Str(Val(txt_Folding.Text)) & "  order by a.Weaver_ClothReceipt_Date, a.for_orderby, a.Weaver_ClothReceipt_No, a.PieceNo_OrderBy, a.Piece_No", con)
        '        Dt1 = New DataTable
        '        Da.Fill(Dt1)

        '        If Dt1.Rows.Count > 0 Then

        '            For i = 0 To Dt1.Rows.Count - 1

        '                n = .Rows.Add()

        '                SNo = SNo + 1
        '                .Rows(n).Cells(0).Value = Val(SNo)

        '                .Rows(n).Cells(1).Value = Dt1.Rows(i).Item("Lot_No").ToString
        '                .Rows(n).Cells(2).Value = Dt1.Rows(i).Item("Piece_No").ToString
        '                .Rows(n).Cells(3).Value = Common_Procedures.ClothType.Type4

        '                If Trim(Dt1.Rows(i).Item("BuyerOffer_Code_Type4").ToString) <> "" And Val(Dt1.Rows(i).Item("BuyerOffer_Passed_Meters_Type4").ToString) <> 0 Then
        '                    PcsMtrs = Val(Dt1.Rows(i).Item("BuyerOffer_Passed_Meters_Type4").ToString)
        '                Else
        '                    PcsMtrs = Val(Dt1.Rows(i).Item("Type4_Meters").ToString)
        '                End If

        '                .Rows(n).Cells(4).Value = Format(Val(PcsMtrs), "#########0.000")
        '                .Rows(n).Cells(5).Value = Format(Val(PcsMtrs) * Val(Dt1.Rows(i).Item("Weight_Meter").ToString), "#########0.000")
        '                .Rows(n).Cells(6).Value = Val(Dt1.Rows(i).Item("Weight_Meter").ToString)
        '                .Rows(n).Cells(7).Value = Dt1.Rows(i).Item("Ledger_Name").ToString
        '                .Rows(n).Cells(8).Value = ""
        '                .Rows(n).Cells(9).Value = Dt1.Rows(i).Item("Lot_Code").ToString
        '                .Rows(n).Cells(10).Value = Dt1.Rows(i).Item("Cloth_Name").ToString

        '                vLmIdNo = 0
        '                If IsDBNull(Dt1.Rows(i).Item("Loom_IdNo").ToString) = False Then
        '                    vLmIdNo = Val(Dt1.Rows(i).Item("Loom_IdNo").ToString)
        '                End If

        '                vLmNo = ""
        '                If vLmIdNo <> 0 Then
        '                    vLmNo = Common_Procedures.Loom_IdNoToName(con, vLmIdNo)

        '                Else
        '                    If IsDBNull(Dt1.Rows(i).Item("Loom_No").ToString) = False Then
        '                        vLmNo = Dt1.Rows(i).Item("Loom_No").ToString
        '                    End If

        '                End If

        '                .Rows(n).Cells(11).Value = vLmNo

        '            Next

        '        End If
        '        Dt1.Clear()
        '    End If
        '    If CloTypIdNo = 5 Then

        '        Da = New SqlClient.SqlDataAdapter("select a.*, c.Ledger_Name, d.cloth_name from Weaver_ClothReceipt_Piece_Details a  INNER JOIN Company_Head tZ ON a.company_idno = tZ.company_idno LEFT OUTER JOIN Ledger_Head c ON a.Ledger_IdNo <> 0 and a.Ledger_IdNo = c.Ledger_IdNo LEFT OUTER JOIN Cloth_Head d ON a.Cloth_IdNo <> 0 and a.Cloth_IdNo = d.Cloth_IdNo where " & CompIDCondt & IIf(Trim(CompIDCondt) <> "", " and ", "") & " a.Type5_Meters <> 0 and a.PackingSlip_Code_Type5 = '" & Trim(NewCode) & "'  and " & CloID_Cond & IIf(CloID_Cond <> "", " and ", " ") & "  a.Folding = " & Str(Val(txt_Folding.Text)) & " order by a.Weaver_ClothReceipt_Date, a.for_orderby, a.Weaver_ClothReceipt_No, a.PieceNo_OrderBy, a.Piece_No", con)
        '        Dt1 = New DataTable
        '        Da.Fill(Dt1)

        '        If Dt1.Rows.Count > 0 Then

        '            For i = 0 To Dt1.Rows.Count - 1

        '                n = .Rows.Add()

        '                SNo = SNo + 1
        '                .Rows(n).Cells(0).Value = Val(SNo)

        '                .Rows(n).Cells(1).Value = Dt1.Rows(i).Item("Lot_No").ToString
        '                .Rows(n).Cells(2).Value = Dt1.Rows(i).Item("Piece_No").ToString
        '                .Rows(n).Cells(3).Value = Common_Procedures.ClothType.Type5

        '                If Trim(Dt1.Rows(i).Item("BuyerOffer_Code_Type5").ToString) <> "" And Val(Dt1.Rows(i).Item("BuyerOffer_Passed_Meters_Type5").ToString) <> 0 Then
        '                    PcsMtrs = Val(Dt1.Rows(i).Item("BuyerOffer_Passed_Meters_Type5").ToString)
        '                Else
        '                    PcsMtrs = Val(Dt1.Rows(i).Item("Type5_Meters").ToString)
        '                End If

        '                .Rows(n).Cells(4).Value = Format(Val(PcsMtrs), "#########0.00")
        '                .Rows(n).Cells(5).Value = Format(Val(PcsMtrs) * Val(Dt1.Rows(i).Item("Weight_Meter").ToString), "#########0.000")
        '                .Rows(n).Cells(6).Value = Val(Dt1.Rows(i).Item("Weight_Meter").ToString)
        '                .Rows(n).Cells(7).Value = Dt1.Rows(i).Item("Ledger_Name").ToString
        '                .Rows(n).Cells(8).Value = "1"
        '                .Rows(n).Cells(9).Value = Dt1.Rows(i).Item("Lot_Code").ToString
        '                .Rows(n).Cells(10).Value = Dt1.Rows(i).Item("Cloth_Name").ToString

        '                vLmIdNo = 0
        '                If IsDBNull(Dt1.Rows(i).Item("Loom_IdNo").ToString) = False Then
        '                    vLmIdNo = Val(Dt1.Rows(i).Item("Loom_IdNo").ToString)
        '                End If

        '                vLmNo = ""
        '                If vLmIdNo <> 0 Then
        '                    vLmNo = Common_Procedures.Loom_IdNoToName(con, vLmIdNo)

        '                Else
        '                    If IsDBNull(Dt1.Rows(i).Item("Loom_No").ToString) = False Then
        '                        vLmNo = Dt1.Rows(i).Item("Loom_No").ToString
        '                    End If

        '                End If

        '                .Rows(n).Cells(11).Value = vLmNo

        '                For j = 0 To .ColumnCount - 1
        '                    .Rows(i).Cells(j).Style.ForeColor = Color.Red
        '                Next

        '            Next

        '        End If
        '        Dt1.Clear()


        '        Da = New SqlClient.SqlDataAdapter("select a.*, c.Ledger_Name, d.cloth_name from Weaver_ClothReceipt_Piece_Details a  INNER JOIN Company_Head tZ ON a.company_idno = tZ.company_idno LEFT OUTER JOIN Ledger_Head c ON a.Ledger_IdNo <> 0 and a.Ledger_IdNo = c.Ledger_IdNo LEFT OUTER JOIN Cloth_Head d ON a.Cloth_IdNo <> 0 and a.Cloth_IdNo = d.Cloth_IdNo where " & CompIDCondt & IIf(Trim(CompIDCondt) <> "", " and ", "") & " a.Type5_Meters <> 0 and a.PackingSlip_Code_Type5 = '' and " & CloID_Cond & IIf(CloID_Cond <> "", " and ", " ") & "  a.Folding = " & Str(Val(txt_Folding.Text)) & "  order by a.Weaver_ClothReceipt_Date, a.for_orderby, a.Weaver_ClothReceipt_No, a.PieceNo_OrderBy, a.Piece_No", con)
        '        Dt1 = New DataTable
        '        Da.Fill(Dt1)

        '        If Dt1.Rows.Count > 0 Then

        '            For i = 0 To Dt1.Rows.Count - 1

        '                n = .Rows.Add()

        '                SNo = SNo + 1
        '                .Rows(n).Cells(0).Value = Val(SNo)

        '                .Rows(n).Cells(1).Value = Dt1.Rows(i).Item("Lot_No").ToString
        '                .Rows(n).Cells(2).Value = Dt1.Rows(i).Item("Piece_No").ToString
        '                .Rows(n).Cells(3).Value = Common_Procedures.ClothType.Type5

        '                If Trim(Dt1.Rows(i).Item("BuyerOffer_Code_Type5").ToString) <> "" And Val(Dt1.Rows(i).Item("BuyerOffer_Passed_Meters_Type5").ToString) <> 0 Then
        '                    PcsMtrs = Val(Dt1.Rows(i).Item("BuyerOffer_Passed_Meters_Type5").ToString)
        '                Else
        '                    PcsMtrs = Val(Dt1.Rows(i).Item("Type5_Meters").ToString)
        '                End If

        '                .Rows(n).Cells(4).Value = Format(Val(PcsMtrs), "#########0.00")
        '                .Rows(n).Cells(5).Value = Format(Val(PcsMtrs) * Val(Dt1.Rows(i).Item("Weight_Meter").ToString), "#########0.000")
        '                .Rows(n).Cells(6).Value = Val(Dt1.Rows(i).Item("Weight_Meter").ToString)
        '                .Rows(n).Cells(7).Value = Dt1.Rows(i).Item("Ledger_Name").ToString
        '                .Rows(n).Cells(8).Value = ""
        '                .Rows(n).Cells(9).Value = Dt1.Rows(i).Item("Lot_Code").ToString
        '                .Rows(n).Cells(10).Value = Dt1.Rows(i).Item("Cloth_Name").ToString

        '                vLmIdNo = 0
        '                If IsDBNull(Dt1.Rows(i).Item("Loom_IdNo").ToString) = False Then
        '                    vLmIdNo = Val(Dt1.Rows(i).Item("Loom_IdNo").ToString)
        '                End If

        '                vLmNo = ""
        '                If vLmIdNo <> 0 Then
        '                    vLmNo = Common_Procedures.Loom_IdNoToName(con, vLmIdNo)

        '                Else
        '                    If IsDBNull(Dt1.Rows(i).Item("Loom_No").ToString) = False Then
        '                        vLmNo = Dt1.Rows(i).Item("Loom_No").ToString
        '                    End If

        '                End If

        '                .Rows(n).Cells(11).Value = vLmNo

        '            Next

        '        End If
        '        Dt1.Clear()
        '    End If
        'End With

        pnl_Selection.Visible = True
        Pnl_Back.Enabled = False
        dgv_Selection.Focus()

    End Sub

    Private Sub dgv_Selection_CellClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Selection.CellClick
        Select_Piece(e.RowIndex)
        btn_Close_Selection_Click(sender, e)
    End Sub

    Private Sub dgv_Selection_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_Selection.KeyDown
        Dim n As Integer

        On Error Resume Next

        If e.KeyCode = Keys.Enter Or e.KeyCode = Keys.Space Then
            If dgv_Selection.CurrentCell.RowIndex >= 0 Then

                n = dgv_Selection.CurrentCell.RowIndex

                Select_Piece(n)

                btn_Close_Selection_Click(sender, e)

                e.Handled = True

            End If
        End If
    End Sub

    Private Sub Select_Piece(ByVal RwIndx As Integer)
        Dim i As Integer

        With dgv_Selection

            If .RowCount > 0 And RwIndx >= 0 Then

                .Rows(RwIndx).Cells(8).Value = (Val(.Rows(RwIndx).Cells(8).Value) + 1) Mod 2

                If Val(.Rows(RwIndx).Cells(8).Value) = 1 Then

                    For i = 0 To .ColumnCount - 1
                        .Rows(RwIndx).Cells(i).Style.ForeColor = Color.Red
                    Next

                Else

                    .Rows(RwIndx).Cells(8).Value = ""

                    For i = 0 To .ColumnCount - 1
                        .Rows(RwIndx).Cells(i).Style.ForeColor = Color.Blue
                    Next

                End If

            End If

        End With


    End Sub

    Private Sub btn_Close_Selection_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Close_Selection.Click
        Piece_Selection()
    End Sub

    Private Sub Piece_Selection()
        Dim n As Integer = 0
        Dim sno As Integer = 0
        Dim dt As New DataTable
        Dim da As New SqlClient.SqlDataAdapter
        Dim vLmIdNo As Long = 0
        Dim vLmNo As String = ""

        dgv_Details.Rows.Clear()

        sno = 0
        For i = 0 To dgv_Selection.RowCount - 1

            If Val(dgv_Selection.Rows(i).Cells(8).Value) = 1 Then


                da = New SqlClient.SqlDataAdapter("select a.* , b.Cloth_name ,ct.* ,lh.* from Packing_Slip_Details a left outer join Ledger_Head lh on a.Party_IdNo = lh.LEdger_Idno left outer join Cloth_Head b on a.cloth_idno = b.cloth_idno LEFT OUTER JOIN ClothType_Head CT ON A.ClothType_IdNo = CT.ClothType_IdNo where a.company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Packing_Slip_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "'  and a.Packing_Slip_Code = '" & Trim(dgv_Selection.Rows(i).Cells(9).Value) & "'  Order by a.for_orderby, a.Packing_Slip_No", con)
                da.Fill(dt)

                dgv_Details.Rows.Clear()

                If dt.Rows.Count > 0 Then
                    For j = 0 To dt.Rows.Count - 1
                        n = dgv_Details.Rows.Add()

                        sno = sno + 1
                        dgv_Details.Rows(n).Cells(0).Value = Val(sno)

                        dgv_Details.Rows(n).Cells(1).Value = dt.Rows(j).Item("Lot_No").ToString
                        dgv_Details.Rows(n).Cells(2).Value = dt.Rows(j).Item("Pcs_No").ToString
                        dgv_Details.Rows(n).Cells(3).Value = Trim(dt.Rows(j).Item("ClothType_Name").ToString)


                        dgv_Details.Rows(n).Cells(4).Value = Format(Val(dt.Rows(j).Item("Meters").ToString), "#########0.00")
                        dgv_Details.Rows(n).Cells(5).Value = Format(Val(dt.Rows(j).Item("Weight").ToString), "#########0.00")
                        dgv_Details.Rows(n).Cells(6).Value = Val(dt.Rows(j).Item("Weight_Meter").ToString)
                        dgv_Details.Rows(n).Cells(7).Value = Trim(dt.Rows(j).Item("Ledger_Name").ToString)
                        dgv_Details.Rows(n).Cells(8).Value = Trim(dt.Rows(j).Item("Lot_Code").ToString)
                        dgv_Details.Rows(n).Cells(9).Value = Trim(dt.Rows(j).Item("Cloth_Name").ToString)

                        vLmIdNo = 0
                        If IsDBNull(dt.Rows(j).Item("Loom_IdNo").ToString) = False Then
                            vLmIdNo = Val(dt.Rows(j).Item("Loom_IdNo").ToString)
                        End If

                        vLmNo = ""
                        If vLmIdNo <> 0 Then
                            vLmNo = Common_Procedures.Loom_IdNoToName(con, vLmIdNo)

                        Else
                            If IsDBNull(dt.Rows(j).Item("Loom_No").ToString) = False Then
                                vLmNo = dt.Rows(j).Item("Loom_No").ToString
                            End If

                        End If

                        dgv_Details.Rows(n).Cells(10).Value = vLmNo



                        lbl_PackingSlipCode.Text = Trim(dt.Rows(j).Item("Packing_Slip_Code").ToString)
                        lbl_Packing_Slip_Date.Text = dt.Rows(j).Item("Packing_Slip_date")
                    Next j
                End If


            End If

        Next i

        Total_Calculation()

        Pnl_Back.Enabled = True
        pnl_Selection.Visible = False
        If txt_Note.Enabled And txt_Note.Visible Then txt_Note.Focus()

    End Sub

    Private Sub btn_Print_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Print.Click
        Common_Procedures.Print_OR_Preview_Status = 1
        print_record()
    End Sub

    Private Sub btn_print_Close_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Close_Print.Click
        Pnl_Back.Enabled = True
        pnl_Print.Visible = False
    End Sub

    Public Sub print_record() Implements Interface_MDIActions.print_record
        pnl_Print.Visible = True
        Pnl_Back.Enabled = False
        txt_PrintFrom.Text = lbl_UnPackNo.Text
        txt_PrintTo.Text = lbl_UnPackNo.Text
        If txt_PrintFrom.Enabled And txt_PrintFrom.Visible Then
            txt_PrintFrom.Focus()
            txt_PrintFrom.SelectAll()
        End If
    End Sub

    Private Sub btn_Print_Cancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Print_Cancel.Click
        btn_print_Close_Click(sender, e)
    End Sub

    Private Sub btn_Print_Ok_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Print_Ok.Click
        Printing_Bale()
    End Sub

    Private Sub txt_PrintFrom_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_PrintFrom.KeyDown
        If e.KeyCode = Keys.Down Then
            txt_PrintTo.Focus()
        End If
    End Sub

    Private Sub txt_PrintTo_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_PrintTo.KeyDown
        If e.KeyCode = Keys.Down Then
            btn_Print_Ok.Focus()
        End If
        If e.KeyCode = Keys.Up Then
            txt_PrintFrom.Focus()
        End If
    End Sub

    Private Sub txt_PrintTo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_PrintTo.KeyPress
        If Asc(e.KeyChar) = 13 Then
            Printing_Bale()
        End If
    End Sub

    Public Sub Printing_Bale()
        Dim prtFrm As Single = 0
        Dim prtTo As Single = 0
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim Condt As String = ""
        Dim PpSzSTS As Boolean = False
        Dim ps As Printing.PaperSize

        If Val(txt_PrintFrom.Text) = 0 Then Exit Sub
        If Val(txt_PrintTo.Text) = 0 Then Exit Sub

        prtFrm = Val(Common_Procedures.OrderBy_CodeToValue(txt_PrintFrom.Text))
        prtTo = Val(Common_Procedures.OrderBy_CodeToValue(txt_PrintTo.Text))

        Condt = ""
        If Val(txt_PrintFrom.Text) <> 0 And Val(txt_PrintTo.Text) <> 0 Then
            Condt = " a.for_OrderBy between " & Str(Val(prtFrm)) & " and " & Trim(prtTo)

        ElseIf Val(txt_PrintFrom.Text) <> 0 Then
            Condt = " a.for_OrderBy = " & Str(Val(prtFrm))

        Else
            Exit Sub

        End If

        Try

            da1 = New SqlClient.SqlDataAdapter("select a.*, b.Cloth_Name from Bale_UnPacking_Head a INNER JOIN Cloth_Head b ON a.Cloth_IdNo = b.Cloth_IdNo where a.company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Bale_UnPacking_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "' and a.Bale_UnPacking_Code NOT LIKE '" & Trim(PkCondition_RollPacking) & "%'  and a.Bale_UnPacking_Code NOT LIKE '" & Trim(PkCondition_BaleDirectEntry) & "%' " & IIf(Trim(Condt) <> "", " and ", "") & Condt, con)
            dt1 = New DataTable
            da1.Fill(dt1)

            If dt1.Rows.Count <= 0 Then

                MessageBox.Show("No Entry Found", "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                Exit Sub

            End If

            dt1.Dispose()
            da1.Dispose()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1019" Then '---- SUBHAM Textiles (Somanur)
            For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
                If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
                    ps = PrintDocument1.PrinterSettings.PaperSizes(I)
                    PrintDocument1.DefaultPageSettings.PaperSize = ps
                    'e.PageSettings.PaperSize = ps
                    Exit For
                End If
            Next

        Else
            For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
                If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.GermanStandardFanfold Then
                    ps = PrintDocument1.PrinterSettings.PaperSizes(I)
                    PrintDocument1.DefaultPageSettings.PaperSize = ps
                    PpSzSTS = True
                    Exit For
                End If
            Next

            If PpSzSTS = False Then
                For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
                    If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
                        ps = PrintDocument1.PrinterSettings.PaperSizes(I)
                        PrintDocument1.DefaultPageSettings.PaperSize = ps
                        Exit For
                    End If
                Next
            End If

        End If


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
                MessageBox.Show("The printing operation failed" & vbCrLf & ex.Message, "DOES NOT SHOW PRINT PREVIEW...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

            End Try

        Else

            Try

                Dim ppd As New PrintPreviewDialog

                ppd.Document = PrintDocument1

                ppd.WindowState = FormWindowState.Normal
                ppd.StartPosition = FormStartPosition.CenterScreen
                ppd.ClientSize = New Size(600, 600)

                ppd.ShowDialog()
                'If PageSetupDialog1.ShowDialog() = Windows.Forms.DialogResult.OK Then
                '    PrintDocument1.DefaultPageSettings = PageSetupDialog1.PageSettings
                '    ppd.ShowDialog()
                'End If

            Catch ex As Exception
                MsgBox("The printing operation failed" & vbCrLf & ex.Message, MsgBoxStyle.Critical, "DOES NOT SHOW PRINT PREVIEW...")

            End Try

        End If

        Pnl_Back.Enabled = True
        pnl_Print.Visible = False

    End Sub

    Private Sub PrintDocument1_BeginPrint(ByVal sender As Object, ByVal e As System.Drawing.Printing.PrintEventArgs) Handles PrintDocument1.BeginPrint
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim prtFrm As Single = 0
        Dim prtTo As Single = 0
        Dim Condt As String = ""

        If Val(txt_PrintFrom.Text) = 0 Then Exit Sub
        If Val(txt_PrintTo.Text) = 0 Then Exit Sub

        prtFrm = Val(Common_Procedures.OrderBy_CodeToValue(txt_PrintFrom.Text))
        prtTo = Val(Common_Procedures.OrderBy_CodeToValue(txt_PrintTo.Text))

        Condt = ""
        If Val(txt_PrintFrom.Text) <> 0 And Val(txt_PrintTo.Text) <> 0 Then
            Condt = " a.for_OrderBy between " & Str(Val(prtFrm)) & " and " & Trim(prtTo)

        ElseIf Val(txt_PrintFrom.Text) <> 0 Then
            Condt = " a.for_OrderBy = " & Str(Val(prtFrm))

        Else
            Exit Sub

        End If

        prn_HdDt.Clear()
        prn_DetDt.Clear()

        prn_PageNo = 0
        prn_HdIndx = 1
        prn_DetIndx = 0
        prn_HdMxIndx = 0
        prn_DetMxIndx = 0
        prn_Count = 1
        Erase prn_DetAr
        Erase prn_HdAr

        prn_HdAr = New String(100, 10) {}

        prn_DetAr = New String(100, 50, 10) {}

        Try

            da1 = New SqlClient.SqlDataAdapter("select a.*, tZ.*, c.Cloth_Name from Bale_UnPacking_Head a INNER JOIN Company_head tZ ON a.company_idno = tZ.Company_Idno INNER JOIN Cloth_Head c ON a.Cloth_IdNo = c.Cloth_IdNo Where a.company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Bale_UnPacking_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "'  and a.Bale_UnPacking_Code NOT LIKE '" & Trim(PkCondition_RollPacking) & "%' and a.Bale_UnPacking_Code NOT LIKE '" & Trim(PkCondition_BaleDirectEntry) & "%' " & IIf(Trim(Condt) <> "", " and ", "") & Condt & " order by a.for_orderby, a.Bale_UnPacking_Code", con)
            prn_HdDt = New DataTable
            da1.Fill(prn_HdDt)

            If prn_HdDt.Rows.Count > 0 Then
                For i = 0 To prn_HdDt.Rows.Count - 1

                    prn_HdMxIndx = prn_HdMxIndx + 1

                    prn_HdAr(prn_HdMxIndx, 1) = Trim(prn_HdDt.Rows(i).Item("Bale_UnPacking_No").ToString)
                    prn_HdAr(prn_HdMxIndx, 2) = Trim(prn_HdDt.Rows(i).Item("Cloth_Name").ToString)
                    prn_HdAr(prn_HdMxIndx, 3) = Val(prn_HdDt.Rows(i).Item("Total_Pcs").ToString)
                    prn_HdAr(prn_HdMxIndx, 4) = Format(Val(prn_HdDt.Rows(i).Item("Total_Meters").ToString), "#########0.00")

                    prn_DetMxIndx = 0

                    da2 = New SqlClient.SqlDataAdapter("select a.* from Bale_UnPacking_Details a where a.company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Bale_UnPacking_Code = '" & Trim(prn_HdDt.Rows(i).Item("Bale_UnPacking_Code").ToString) & "' order by a.Sl_No", con)
                    prn_DetDt = New DataTable
                    da2.Fill(prn_DetDt)

                    If prn_DetDt.Rows.Count > 0 Then
                        For j = 0 To prn_DetDt.Rows.Count - 1
                            If Val(prn_DetDt.Rows(j).Item("Meters").ToString) <> 0 Then
                                prn_DetMxIndx = prn_DetMxIndx + 1

                                prn_DetAr(prn_HdMxIndx, prn_DetMxIndx, 1) = Trim(prn_DetDt.Rows(j).Item("Lot_No").ToString)
                                prn_DetAr(prn_HdMxIndx, prn_DetMxIndx, 2) = Trim(prn_DetDt.Rows(j).Item("Pcs_No").ToString)
                                prn_DetAr(prn_HdMxIndx, prn_DetMxIndx, 3) = Format(Val(prn_DetDt.Rows(j).Item("Meters").ToString), "#########0.00")

                            End If
                        Next j
                    End If

                Next i

            Else
                MessageBox.Show("This is New Entry", "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

            End If

            da1.Dispose()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub PrintDocument1_PrintPage(ByVal sender As System.Object, ByVal e As System.Drawing.Printing.PrintPageEventArgs) Handles PrintDocument1.PrintPage
        Dim EntryCode As String

        If prn_HdDt.Rows.Count <= 0 Then Exit Sub

        EntryCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_UnPackNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1249" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1516" Then '---- VAIPAV TEXTILES PVT LTD (SOMANUR) AND ---- VIPIN TEXTILES (SOMANUR) 
            Common_Procedures.Printing_PackingSlip_Format2(PrintDocument1, e, prn_HdDt, prn_HdMxIndx, prn_DetMxIndx, prn_HdAr, prn_DetAr, prn_PageNo, prn_Count, prn_HdIndx, prn_DetIndx)
        Else
            Common_Procedures.Printing_PackingSlip_Format1(PrintDocument1, e, prn_HdDt, prn_HdMxIndx, prn_DetMxIndx, prn_HdAr, prn_DetAr, prn_PageNo, prn_Count, prn_HdIndx, prn_DetIndx)
        End If

    End Sub

    Private Sub dgtxt_Details_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgtxt_Details.KeyUp
        If e.Control = True And UCase(Chr(e.KeyCode)) = "D" Then
            dgv_Details_KeyUp(sender, e)
        End If
    End Sub

    Private Sub cbo_PartyName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_PartyName_StockOF.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_IdNo = 4 or Ledger_Type = 'OWNSORT' or Ledger_Type = 'JOBWORKER' or Show_In_All_Entry = 1)", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_PartyName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_PartyName_StockOF.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_PartyName_StockOF, msk_date, Nothing, "Ledger_AlaisHead", "Ledger_DisplayName", "( Ledger_IdNo = 4 or Ledger_Type = 'OWNSORT' or Ledger_Type = 'JOBWORKER' or Show_In_All_Entry = 1)", "(Ledger_IdNo = 0)")
        If e.KeyCode = 40 And cbo_PartyName_StockOF.DroppedDown = False Or (e.Control = True And e.KeyCode = 40) Then
            If cbo_Godown_StockIN.Visible = True Then
                cbo_Godown_StockIN.Focus()
            Else
                cbo_Cloth.Focus()
            End If
        End If
    End Sub

    Private Sub cbo_PartyName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_PartyName_StockOF.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_PartyName_StockOF, Nothing, "Ledger_AlaisHead", "Ledger_DisplayName", "( Ledger_IdNo = 4 or Ledger_Type = 'OWNSORT' or Ledger_Type = 'JOBWORKER' or Show_In_All_Entry = 1)", "(Ledger_IdNo = 0)")
        If Asc(e.KeyChar) = 13 Then
            If cbo_Godown_StockIN.Visible = True Then
                cbo_Godown_StockIN.Focus()
            Else
                cbo_Cloth.Focus()
            End If
        End If
    End Sub

    Private Sub cbo_PartyName_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_PartyName_StockOF.KeyUp
        If e.Control = False And e.KeyValue = 17 Then
            Common_Procedures.MDI_LedType = "JOBWORKER"
            Dim f As New Ledger_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_PartyName_StockOF.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()
        End If
    End Sub

    Private Sub txt_PcsSelction_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs)
        If e.KeyValue = 40 Then
            If dgv_Selection.Rows.Count > 0 Then
                dgv_Selection.Focus()
                dgv_Selection.CurrentCell = dgv_Selection.Rows(0).Cells(0)
                dgv_Selection.CurrentCell.Selected = True
            End If
        End If
        If (e.KeyValue = 38) Then txt_BaleSelection.Focus()

    End Sub



    Private Sub txt_LotSelction_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_BaleSelection.KeyDown
        'If (e.KeyValue = 40) Then
        '    txt_PcsSelction.Focus()
        'End If
    End Sub

    Private Sub txt_LotSelction_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_BaleSelection.KeyPress
        'If Asc(e.KeyChar) = 13 Then
        '    txt_PcsSelction.Focus()
        'End If
    End Sub

    Private Sub btn_lot_Pcs_selection_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Lot_Pcs_Selection.Click

        'If Trim(txt_LotSelction.Text) <> "" Or Trim(txt_PcsSelction.Text) <> "" Then

        '    LtNo = Trim(txt_LotSelction.Text)
        '    PcsNo = Trim(txt_PcsSelction.Text)

        '    For i = 0 To dgv_Selection.Rows.Count - 1
        '        If dgv_Selection.Rows(i).Visible = True Then
        '            If Trim(UCase(LtNo)) = Trim(UCase(dgv_Selection.Rows(i).Cells(1).Value)) And Trim(UCase(PcsNo)) = Trim(UCase(dgv_Selection.Rows(i).Cells(2).Value)) Then
        '                Call Select_Piece(i)
        '                dgv_Selection.CurrentCell = dgv_Selection.Rows(i).Cells(0)
        '                If i >= 9 Then dgv_Selection.FirstDisplayedScrollingRowIndex = i - 8

        '                Exit For
        '            End If
        '        End If
        '    Next

        '    txt_LotSelction.Text = ""
        '    txt_PcsSelction.Text = ""
        '    If txt_LotSelction.Enabled = True Then txt_LotSelction.Focus()

        'End If
    End Sub

    Private Sub Grid_Selection(ByVal RwIndx As Integer)
        'Dim i As Integer

        'With dgv_Selection

        '    If .RowCount > 0 And RwIndx >= 0 Then

        '        .Rows(RwIndx).Cells(8).Value = (Val(.Rows(RwIndx).Cells(8).Value) + 1) Mod 2

        '        If Val(.Rows(RwIndx).Cells(8).Value) = 0 Then

        '            .Rows(RwIndx).Cells(8).Value = ""

        '            For i = 0 To .ColumnCount - 1
        '                .Rows(RwIndx).Cells(i).Style.ForeColor = Color.Blue
        '            Next

        '        Else
        '            For i = 0 To .ColumnCount - 1
        '                .Rows(RwIndx).Cells(i).Style.ForeColor = Color.Red
        '            Next

        '        End If

        '    End If
        '    If txt_LotSelction.Enabled = True Then txt_LotSelction.Focus()

        'End With

    End Sub





    Private Sub msk_Date_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles msk_date.KeyDown
        vcbo_KeyDwnVal = e.KeyValue


        vmskOldText = ""
        vmskSelStrt = -1
        If e.KeyCode = 46 Or e.KeyCode = 8 Then
            vmskOldText = msk_Date.Text
            vmskSelStrt = msk_Date.SelectionStart
        End If

    End Sub

 


    Private Sub msk_Date_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles msk_date.KeyUp
        Dim vmRetTxt As String = ""
        Dim vmRetSelStrt As Integer = -1

        If Trim(UCase(e.KeyCode)) = "D" And vcbo_KeyDwnVal = e.KeyValue Then
            msk_date.Text = Date.Today
            msk_date.SelectionStart = 0
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

    Private Sub dtp_Date_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dtp_Date.TextChanged

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

    Private Sub dtp_Date_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dtp_Date.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            dtp_Date.Text = Date.Today
        End If
    End Sub

    Private Sub btn_Meter_Selection_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Meter_Selection.Click
        Dim vFirstRowNo As Integer = -1

            btn_ShowAll_Selection_Click(sender, e)

    
    End Sub

    Private Sub btn_ShowAll_Selection_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_ShowAll_Selection.Click
        Dim i As Integer = 0
        Dim CurRow As Integer = 0

        'Try
        '    For i = 0 To dgv_Selection.Rows.Count - 1
        '        dgv_Selection.Rows(i).Visible = True
        '    Next
        '    txt_MeterSelction.Text = ""
        'Catch ex As Exception
        '    '---
        'End Try
    End Sub

    Private Sub txt_MeterSelction_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs)
        If e.KeyValue = 40 Then
            If dgv_Selection.Rows.Count > 0 Then
                dgv_Selection.Focus()
                dgv_Selection.CurrentCell = dgv_Selection.Rows(0).Cells(0)
                dgv_Selection.CurrentCell.Selected = True
            End If
        End If
        'If (e.KeyValue = 38) Then txt_PcsSelction.Focus()
    End Sub

    Private Sub txt_MeterSelction_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
        If Asc(e.KeyChar) = 13 Then
            btn_Meter_Selection_Click(sender, e)
        End If
    End Sub

    Private Sub cbo_Godown_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Godown_StockIN.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'GODOWN' or Show_In_All_Entry = 1) and Close_status = 0 ", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_Godown_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Godown_StockIN.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Godown_StockIN, Nothing, cbo_Cloth, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'GODOWN' or Show_In_All_Entry = 1) and Close_status = 0 ", "(Ledger_IdNo = 0)")
        If (e.KeyValue = 38 And cbo_Godown_StockIN.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
            If cbo_PartyName_StockOF.Visible Then
                cbo_PartyName_StockOF.Focus()
            Else
                msk_date.Focus()
            End If
        End If
    End Sub

    Private Sub cbo_Godown_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Godown_StockIN.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Godown_StockIN, cbo_Cloth, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'GODOWN' or Show_In_All_Entry = 1) and Close_status = 0 ", "(Ledger_IdNo = 0)")
    End Sub


End Class