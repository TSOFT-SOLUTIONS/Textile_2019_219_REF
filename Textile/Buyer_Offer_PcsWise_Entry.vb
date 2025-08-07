Public Class Buyer_Offer_PcsWise_Entry
    Implements Interface_MDIActions

    Private con As New SqlClient.SqlConnection(Common_Procedures.Connection_String)
    Private con1 As New SqlClient.SqlConnection(Common_Procedures.ConnectionString_CompanyGroupdetails)
    Private FrmLdSTS As Boolean = False
    Private New_Entry As Boolean = False
    Private Insert_Entry As Boolean = False
    Private Filter_Status As Boolean = False
    Private Pk_Condition As String = "BUYOF-"
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

    Private Sub clear()

        NoCalc_Status = True

        New_Entry = False
        Insert_Entry = False

        Pnl_Back.Enabled = True
        pnl_Filter.Visible = False
        pnl_Selection.Visible = False
        pnl_Print.Visible = False
        Lbl_DelvCode.Text = ""

        lbl_RefNo.Text = ""
        lbl_RefNo.ForeColor = Color.Black

        vmskOldText = ""
        vmskSelStrt = -1

        msk_date.Text = ""
        dtp_Date.Text = ""
        'cbo_Cloth.Text = ""
        cbo_ClothType.Text = Common_Procedures.ClothType_IdNoToName(con, 1)
        'cbo_Cloth.Text = ""
        cbo_Bale_Bundle.Text = "BALE"
        txt_LotSelction.Text = ""
        txt_PcsSelction.Text = ""
        lbl_UserName.Text = "USER : " & Trim(UCase(Common_Procedures.User_IdNoToName(con1, Common_Procedures.User.IdNo)))

        txt_Folding.Text = 100
        txt_Note.Text = ""
        chk_SelectAll.Checked = False

        dgv_Details.Rows.Clear()
        dgv_Details_Total.Rows.Clear()
        dgv_Details_Total.Rows.Add()

        If Filter_Status = False Then
            dtp_Filter_Fromdate.Text = Common_Procedures.Company_FromDate
            dtp_Filter_ToDate.Text = Common_Procedures.Company_ToDate
            cbo_Filter_Cloth.Text = ""
            cbo_Filter_PartyName.Text = ""
            txt_Filter_LotNo.Text = ""
            txt_Filter_PcsNo.Text = ""
            txt_Filter_RollNo.Text = ""
            cbo_Filter_Cloth.SelectedIndex = -1
            cbo_Filter_PartyName.SelectedIndex = -1
            dgv_Filter_Details.Rows.Clear()
        End If

        cbo_PartyName.Enabled = True
        cbo_PartyName.BackColor = Color.White

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
            Msktxbx.SelectAll()
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
        If Not IsNothing(dgv_Details.CurrentCell) Then  dgv_Details.CurrentCell.Selected = False
        If Not IsNothing(dgv_Details_Total.CurrentCell) Then dgv_Details_Total.CurrentCell.Selected = False
        If Not IsNothing(dgv_Filter_Details.CurrentCell) Then dgv_Filter_Details.CurrentCell.Selected = False
    End Sub

    Private Sub Buyer_Offer_PcsWise_Entry_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated
        Dim da As SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim NoofComps As Integer
        Dim CompCondt As String

        Try

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_PartyName.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "LEDGER" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_PartyName.Text = Trim(Common_Procedures.Master_Return.Return_Value)
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
            '----MessageBox.Show(ex.Message, "DOES NOT SHOW...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

        FrmLdSTS = False
    End Sub

    Private Sub Buyer_Offer_PcsWise_Entry_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        On Error Resume Next
        con.Close()
        con.Dispose()
        Common_Procedures.Last_Closed_FormName = Me.Name
    End Sub

    Private Sub Buyer_Offer_PcsWise_Entry_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
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

    Private Sub Buyer_Offer_PcsWise_Entry_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Me.Text = ""

        con.Open()

        dtp_Date.Text = ""
        msk_date.Text = ""


        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1464" Then '---- MANI OMEGA FABRICS (THIRUCHENKODU)
            dgv_Details.Columns(1).HeaderText = "Doff No"
            dgv_Details.Columns(2).HeaderText = "FER No."

            dgv_Selection.Columns(1).HeaderText = "Doff No"
            dgv_Selection.Columns(2).HeaderText = "FER No."

            dgv_Details.Columns(7).ReadOnly = True

            Label18.Text = "FER No."
            Label16.Text = "Doff No"

        End If

        cbo_Bale_Bundle.Items.Clear()
        cbo_Bale_Bundle.Items.Add("BALE")
        cbo_Bale_Bundle.Items.Add("BUNDLE")

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

        btn_UserModification.Visible = False
        If Common_Procedures.settings.User_Modifications_Show_Status = 1 Then
            If Val(Common_Procedures.User.IdNo) = 1 Or Common_Procedures.User.Show_UserModification_Status = 1 Then
                btn_UserModification.Visible = True
            End If
        End If

        AddHandler msk_date.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Cloth.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_ClothType.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Bale_Bundle.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_BuyerRefNo.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Folding.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Note.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Filter_Cloth.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Filter_PartyName.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Filter_LotNo.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Filter_PcsNo.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Filter_RollNo.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_PrintFrom.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_PrintTo.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_PartyName.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_save.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_Print.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_close.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_Print_Ok.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_Print_Cancel.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_LotSelction.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_PcsSelction.GotFocus, AddressOf ControlGotFocus

        AddHandler cbo_PartyName.LostFocus, AddressOf ControlLostFocus
        AddHandler msk_date.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Cloth.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_ClothType.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Bale_Bundle.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Folding.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_BuyerRefNo.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Note.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Filter_Cloth.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Filter_PartyName.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Filter_LotNo.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Filter_PcsNo.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Filter_RollNo.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_PrintFrom.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_PrintTo.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_save.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_Print.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_close.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_Print_Ok.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_Print_Cancel.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_LotSelction.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_PcsSelction.LostFocus, AddressOf ControlLostFocus

        AddHandler msk_date.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_BuyerRefNo.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Filter_LotNo.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Filter_PcsNo.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Filter_RollNo.KeyDown, AddressOf TextBoxControlKeyDown


        AddHandler msk_date.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_BuyerRefNo.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_PrintFrom.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Filter_RollNo.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Filter_LotNo.KeyPress, AddressOf TextBoxControlKeyPress

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

            Else
                dgv1 = dgv_Details

            End If

            If IsNothing(dgv1) = False Then

                With dgv1


                    If keyData = Keys.Enter Or keyData = Keys.Down Then
                        If .CurrentCell.ColumnIndex >= 21 Then
                            If .CurrentCell.RowIndex = .RowCount - 1 Then
                                txt_Note.Focus()

                            Else
                                .CurrentCell = .Rows(.CurrentCell.RowIndex + 1).Cells(7)

                            End If

                        Else
                            If .CurrentCell.ColumnIndex <= 6 Then
                                .CurrentCell = .Rows(.CurrentRow.Index).Cells(7)
                            ElseIf .CurrentCell.ColumnIndex = 7 Then
                                .CurrentCell = .Rows(.CurrentRow.Index).Cells(9)
                            ElseIf .CurrentCell.ColumnIndex = 11 Then
                                .CurrentCell = .Rows(.CurrentRow.Index).Cells(13)
                            ElseIf .CurrentCell.ColumnIndex = 14 Then
                                .CurrentCell = .Rows(.CurrentRow.Index).Cells(20)
                            Else
                                .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 1)
                            End If

                        End If

                        Return True

                        ''ElseIf keyData = Keys.Down Then
                        ''    'If .CurrentCell.ColumnIndex >= .ColumnCount - 1 Then
                        ''    If .CurrentCell.RowIndex = .RowCount - 1 Then
                        ''        'txt_Note.Focus()

                        ''    Else
                        ''        .CurrentCell = .Rows(.CurrentCell.RowIndex + 1).Cells(.CurrentCell.ColumnIndex)

                        ''    End If

                        ''    'Else
                        ''    '    .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 1)

                        ''    'End If

                        ''    Return True

                    ElseIf keyData = Keys.Up Then

                        If .CurrentCell.ColumnIndex <= 7 Then

                            If .CurrentCell.RowIndex = 0 Then
                                If txt_Folding.Enabled And txt_Folding.Visible Then txt_Folding.Focus() Else msk_date.Focus()

                            Else
                                .CurrentCell = .Rows(.CurrentCell.RowIndex - 1).Cells(21)

                            End If

                        Else

                            If .CurrentCell.ColumnIndex = 23 Then
                                .CurrentCell = .Rows(.CurrentRow.Index).Cells(21)
                            ElseIf .CurrentCell.ColumnIndex = 20 Then
                                .CurrentCell = .Rows(.CurrentRow.Index).Cells(14)
                            Else
                                .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex - 1)
                            End If

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
        Dim da3 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim dt3 As New DataTable
        Dim NewCode As String
        Dim n As Integer, I As Integer, J As Integer
        Dim SNo As Integer
        Dim LockSTS As Boolean = False
        Dim vPackSlpCd As String = "", vRejPcsPackSlpCd As String = "", vBitsPcsPackSlpCd As String = ""

        If Val(no) = 0 Then Exit Sub

        clear()

        NewCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(no) & "/" & Trim(Common_Procedures.FnYearCode)

        Try

            da1 = New SqlClient.SqlDataAdapter("select a.* from Buyer_Offer_Head a Where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Buyer_Offer_Code = '" & Trim(NewCode) & "'", con)
            dt1 = New DataTable
            da1.Fill(dt1)

            If dt1.Rows.Count > 0 Then

                lbl_RefNo.Text = dt1.Rows(0).Item("Buyer_offer_No").ToString
                dtp_Date.Text = dt1.Rows(0).Item("Buyer_Offer_Date").ToString
                msk_date.Text = dtp_Date.Text
                cbo_PartyName.Text = Common_Procedures.Ledger_IdNoToName(con, Val(dt1.Rows(0).Item("Ledger_IdNo").ToString))
                cbo_Cloth.Text = Common_Procedures.Cloth_IdNoToName(con, Val(dt1.Rows(0).Item("Cloth_IdNo").ToString))
                cbo_ClothType.Text = Common_Procedures.ClothType_IdNoToName(con, Val(dt1.Rows(0).Item("ClothType_IdNo").ToString))
                txt_BuyerRefNo.Text = dt1.Rows(0).Item("Buyer_RefNo").ToString
                txt_Note.Text = dt1.Rows(0).Item("Note").ToString
                txt_Folding.Text = Format(Val(dt1.Rows(0).Item("Folding").ToString), "########0.00")

                lbl_UserName.Text = "USER : " & Trim(UCase(Common_Procedures.User_IdNoToName(con1, Val(dt1.Rows(0).Item("User_IdNo").ToString))))

                'If IsDBNull(dt1.Rows(0).Item("Delivery_Code").ToString) = False Then
                '    If Trim(dt1.Rows(0).Item("Delivery_Code").ToString) <> "" Then
                '        LockSTS = True
                '    End If
                'End If

                da2 = New SqlClient.SqlDataAdapter("select a.*, b.clothtype_name as Pcs_ClothTypeName, c.Ledger_Name as Pcs_PartyName, d.cloth_name as Pcs_ClothName from Buyer_Offer_Details a INNER JOIN ClothType_Head b ON a.Pcs_ClothType_IdNo <> 0 and a.Pcs_ClothType_IdNo = b.ClothType_IdNo LEFT OUTER JOIN Ledger_Head c ON a.Pcs_PartyIdNo <> 0 and a.Pcs_PartyIdNo = c.Ledger_Idno LEFT OUTER JOIN Cloth_Head d ON a.Pcs_Cloth_IdNo <> 0 and a.Pcs_Cloth_IdNo = d.Cloth_Idno where a.Buyer_Offer_Code = '" & Trim(NewCode) & "' Order by a.sl_no", con)
                dt2 = New DataTable
                da2.Fill(dt2)

                dgv_Details.Rows.Clear()
                SNo = 0

                If dt2.Rows.Count > 0 Then

                    For I = 0 To dt2.Rows.Count - 1

                        n = dgv_Details.Rows.Add()

                        SNo = SNo + 1
                        dgv_Details.Rows(n).Cells(0).Value = Val(SNo)
                        dgv_Details.Rows(n).Cells(1).Value = dt2.Rows(I).Item("Lot_No").ToString
                        dgv_Details.Rows(n).Cells(2).Value = dt2.Rows(I).Item("Pcs_NO").ToString
                        dgv_Details.Rows(n).Cells(3).Value = dt2.Rows(I).Item("Pcs_ClothTypeName").ToString
                        dgv_Details.Rows(n).Cells(4).Value = Format(Val(dt2.Rows(I).Item("Meters").ToString), "########0.00")

                        dgv_Details.Rows(n).Cells(5).Value = Format(Val(dt2.Rows(I).Item("Weight").ToString), "########0.000")
                        If Val(dgv_Details.Rows(n).Cells(5).Value) = 0 Then dgv_Details.Rows(n).Cells(5).Value = ""

                        dgv_Details.Rows(n).Cells(6).Value = Format(Val(dt2.Rows(I).Item("Weight_Meter").ToString), "########0.000")
                        If Val(dgv_Details.Rows(n).Cells(6).Value) = 0 Then dgv_Details.Rows(n).Cells(6).Value = ""

                        dgv_Details.Rows(n).Cells(7).Value = dt2.Rows(I).Item("Party_PieceNo").ToString
                        dgv_Details.Rows(n).Cells(8).Value = Format(Val(dt2.Rows(I).Item("Pass_Meters").ToString), "########0.00")
                        If Val(dgv_Details.Rows(n).Cells(8).Value) = 0 Then dgv_Details.Rows(n).Cells(8).Value = ""
                        dgv_Details.Rows(n).Cells(9).Value = Format(Val(dt2.Rows(I).Item("Less_Meters").ToString), "########0.00")
                        If Val(dgv_Details.Rows(n).Cells(9).Value) = 0 Then dgv_Details.Rows(n).Cells(9).Value = ""
                        dgv_Details.Rows(n).Cells(10).Value = Format(Val(dt2.Rows(I).Item("Reject_Meters").ToString), "########0.00")
                        If Val(dgv_Details.Rows(n).Cells(10).Value) = 0 Then dgv_Details.Rows(n).Cells(10).Value = ""

                        dgv_Details.Rows(n).Cells(11).Value = Val(dt2.Rows(I).Item("Points").ToString)
                        If Val(dgv_Details.Rows(n).Cells(11).Value) = 0 Then dgv_Details.Rows(n).Cells(11).Value = ""
                        dgv_Details.Rows(n).Cells(12).Value = Val(dt2.Rows(I).Item("Point_Per_PassMeter").ToString)
                        If Val(dgv_Details.Rows(n).Cells(12).Value) = 0 Then dgv_Details.Rows(n).Cells(12).Value = ""

                        dgv_Details.Rows(n).Cells(13).Value = dt2.Rows(I).Item("Grade").ToString
                        dgv_Details.Rows(n).Cells(14).Value = dt2.Rows(I).Item("Reject_New_PieceNo").ToString

                        dgv_Details.Rows(n).Cells(15).Value = dt2.Rows(I).Item("lot_code").ToString
                        dgv_Details.Rows(n).Cells(16).Value = dt2.Rows(I).Item("Pcs_PartyName").ToString
                        dgv_Details.Rows(n).Cells(17).Value = dt2.Rows(I).Item("Pcs_ClothName").ToString

                        vPackSlpCd = ""
                        da3 = New SqlClient.SqlDataAdapter("Select a.* from Weaver_ClothReceipt_Piece_Details a Where a.Lot_Code = '" & Trim(dt2.Rows(I).Item("lot_code").ToString) & "' and  a.Piece_No = '" & Trim(dt2.Rows(I).Item("Pcs_NO").ToString) & "'", con)
                        dt3 = New DataTable
                        da3.Fill(dt3)
                        If dt3.Rows.Count > 0 Then
                            If Val(dt2.Rows(0).Item("Pcs_ClothType_IdNo").ToString) = 5 Then
                                vPackSlpCd = dt3.Rows(0).Item("PackingSlip_Code_Type5").ToString
                            ElseIf Val(dt2.Rows(0).Item("Pcs_ClothType_IdNo").ToString) = 4 Then
                                vPackSlpCd = dt3.Rows(0).Item("PackingSlip_Code_Type4").ToString
                            ElseIf Val(dt2.Rows(0).Item("Pcs_ClothType_IdNo").ToString) = 3 Then
                                vPackSlpCd = dt3.Rows(0).Item("PackingSlip_Code_Type3").ToString
                            ElseIf Val(dt2.Rows(0).Item("Pcs_ClothType_IdNo").ToString) = 2 Then
                                vPackSlpCd = dt3.Rows(0).Item("PackingSlip_Code_Type2").ToString
                            Else
                                vPackSlpCd = dt3.Rows(0).Item("PackingSlip_Code_Type1").ToString
                            End If

                        End If
                        dt3.Clear()

                        vRejPcsPackSlpCd = ""
                        If Trim(dt2.Rows(I).Item("Reject_New_PieceNo").ToString) <> "" Then
                            da3 = New SqlClient.SqlDataAdapter("Select a.* from Weaver_ClothReceipt_Piece_Details a Where a.Weaver_Piece_Checking_Code = '" & Trim(NewCode) & "'  and a.Weaver_ClothReceipt_Code = '" & Trim(NewCode) & "' and a.Lot_Code = '" & Trim(dt2.Rows(I).Item("lot_code").ToString) & "' and  a.Piece_No = '" & Trim(dt2.Rows(I).Item("Reject_New_PieceNo").ToString) & "'", con)
                            dt3 = New DataTable
                            da3.Fill(dt3)
                            If dt3.Rows.Count > 0 Then
                                If Val(dt3.Rows(0).Item("Type5_Meters").ToString) <> 0 Then
                                    vRejPcsPackSlpCd = dt3.Rows(0).Item("PackingSlip_Code_Type5").ToString
                                ElseIf Val(dt3.Rows(0).Item("Type4_Meters").ToString) <> 0 Then
                                    vRejPcsPackSlpCd = dt3.Rows(0).Item("PackingSlip_Code_Type4").ToString
                                ElseIf Val(dt3.Rows(0).Item("Type3_Meters").ToString) <> 0 Then
                                    vRejPcsPackSlpCd = dt3.Rows(0).Item("PackingSlip_Code_Type3").ToString
                                ElseIf Val(dt3.Rows(0).Item("Type2_Meters").ToString) <> 0 Then
                                    vRejPcsPackSlpCd = dt3.Rows(0).Item("PackingSlip_Code_Type2").ToString
                                Else
                                    vRejPcsPackSlpCd = dt3.Rows(0).Item("PackingSlip_Code_Type1").ToString
                                End If
                            End If
                            dt3.Clear()
                        End If

                        dgv_Details.Rows(n).Cells(18).Value = vPackSlpCd
                        dgv_Details.Rows(n).Cells(19).Value = vRejPcsPackSlpCd

                        dgv_Details.Rows(n).Cells(20).Value = dt2.Rows(I).Item("Bits_Meters").ToString
                        dgv_Details.Rows(n).Cells(21).Value = dt2.Rows(I).Item("Bits_New_PieceNo").ToString

                        vBitsPcsPackSlpCd = ""
                        If Trim(dt2.Rows(I).Item("Bits_New_PieceNo").ToString) <> "" Then
                            da3 = New SqlClient.SqlDataAdapter("Select a.* from Weaver_ClothReceipt_Piece_Details a Where a.Weaver_Piece_Checking_Code = '" & Trim(NewCode) & "'  and a.Weaver_ClothReceipt_Code = '" & Trim(NewCode) & "' and a.Lot_Code = '" & Trim(dt2.Rows(I).Item("lot_code").ToString) & "' and  a.Piece_No = '" & Trim(dt2.Rows(I).Item("Bits_New_PieceNo").ToString) & "'", con)
                            dt3 = New DataTable
                            da3.Fill(dt3)
                            If dt3.Rows.Count > 0 Then
                                If Val(dt3.Rows(0).Item("Type5_Meters").ToString) <> 0 Then
                                    vBitsPcsPackSlpCd = dt3.Rows(0).Item("PackingSlip_Code_Type5").ToString
                                ElseIf Val(dt3.Rows(0).Item("Type4_Meters").ToString) <> 0 Then
                                    vBitsPcsPackSlpCd = dt3.Rows(0).Item("PackingSlip_Code_Type4").ToString
                                ElseIf Val(dt3.Rows(0).Item("Type3_Meters").ToString) <> 0 Then
                                    vBitsPcsPackSlpCd = dt3.Rows(0).Item("PackingSlip_Code_Type3").ToString
                                ElseIf Val(dt3.Rows(0).Item("Type2_Meters").ToString) <> 0 Then
                                    vBitsPcsPackSlpCd = dt3.Rows(0).Item("PackingSlip_Code_Type2").ToString
                                Else
                                    vBitsPcsPackSlpCd = dt3.Rows(0).Item("PackingSlip_Code_Type1").ToString
                                End If
                            End If
                            dt3.Clear()
                        End If

                        dgv_Details.Rows(n).Cells(22).Value = vBitsPcsPackSlpCd

                        If Val(dt2.Rows(I).Item("Loom_IdNo").ToString) <> 0 Then
                            dgv_Details.Rows(n).Cells(23).Value = Common_Procedures.Loom_IdNoToName(con, Val(dt2.Rows(I).Item("Loom_IdNo").ToString))
                        Else
                            dgv_Details.Rows(n).Cells(23).Value = dt2.Rows(I).Item("Loom_No").ToString
                        End If

                        If Trim(vPackSlpCd) <> "" Or Trim(vRejPcsPackSlpCd) <> "" Or Trim(vBitsPcsPackSlpCd) <> "" Then
                            For J = 0 To dgv_Details.ColumnCount - 1
                                dgv_Details.Rows(n).Cells(J).Style.BackColor = Color.LightGray
                                dgv_Details.Rows(n).Cells(J).Style.ForeColor = Color.Red
                            Next

                            LockSTS = True
                        End If

                    Next I

                End If

                With dgv_Details_Total

                    If .RowCount = 0 Then .Rows.Add()
                    .Rows(0).Cells(2).Value = Val(dt1.Rows(0).Item("Pcs").ToString)
                    .Rows(0).Cells(4).Value = Format(Val(dt1.Rows(0).Item("Meters").ToString), "########0.00")
                    .Rows(0).Cells(5).Value = Format(Val(dt1.Rows(0).Item("Total_Weight").ToString), "########0.000")

                    .Rows(0).Cells(8).Value = Format(Val(dt1.Rows(0).Item("Total_Passed_Meters").ToString), "########0.00")
                    .Rows(0).Cells(9).Value = Format(Val(dt1.Rows(0).Item("Total_Less_Meters").ToString), "########0.00")
                    .Rows(0).Cells(10).Value = Format(Val(dt1.Rows(0).Item("Total_Reject_Meters").ToString), "########0.00")
                    .Rows(0).Cells(11).Value = Val(dt1.Rows(0).Item("Total_Points").ToString)
                    .Rows(0).Cells(20).Value = Format(Val(dt1.Rows(0).Item("Total_Bits_Meters").ToString), "########0.00")

                End With

                dt2.Clear()
                dt2.Dispose()
                da2.Dispose()

            End If

            dt1.Clear()
            dt1.Dispose()
            da1.Dispose()

            If LockSTS = True Then

                cbo_PartyName.Enabled = False
                cbo_PartyName.BackColor = Color.LightGray

                cbo_Cloth.Enabled = False
                cbo_Cloth.BackColor = Color.LightGray

                cbo_ClothType.Enabled = False
                cbo_ClothType.BackColor = Color.LightGray

                txt_Folding.Enabled = False
                txt_Folding.BackColor = Color.LightGray

                'btn_Selection.Enabled = False

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

        Dim vOrdByNo As String = ""

        vOrdByNo = Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text)

        NewCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)
        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.DeletingEntry, Common_Procedures.UR.ClothSales_Buyer_offer_Entry, New_Entry, Me, con, "Buyer_Offer_Head", "Buyer_Offer_Code", NewCode, "Buyer_Offer_Date", "(Buyer_Offer_Code = '" & Trim(NewCode) & "')") = False Then Exit Sub


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

        NewCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Da = New SqlClient.SqlDataAdapter("select count(*) from Weaver_ClothReceipt_Piece_Details Where (BuyerOffer_Code_Type1 = '" & Trim(NewCode) & "' and PackingSlip_Code_Type1 <> '') or (BuyerOffer_Code_Type2 = '" & Trim(NewCode) & "' and PackingSlip_Code_Type2 <> '') or (BuyerOffer_Code_Type3 = '" & Trim(NewCode) & "' and PackingSlip_Code_Type3 <> '') or (BuyerOffer_Code_Type4 = '" & Trim(NewCode) & "' and PackingSlip_Code_Type4 <> '') or (BuyerOffer_Code_Type5 = '" & Trim(NewCode) & "' and PackingSlip_Code_Type5 <> '')", con)
        Dt1 = New DataTable
        Da.Fill(Dt1)
        If Dt1.Rows.Count > 0 Then
            If IsDBNull(Dt1.Rows(0)(0).ToString) = False Then
                If Val(Dt1.Rows(0)(0).ToString) > 0 Then
                    MessageBox.Show("Already this RollNo prepared for some piece", "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                    Exit Sub
                End If
            End If
        End If
        Dt1.Clear()

        Da = New SqlClient.SqlDataAdapter("select count(*) from Weaver_ClothReceipt_Piece_Details Where Weaver_Piece_Checking_Code = '" & Trim(NewCode) & "' and Weaver_ClothReceipt_Code = '" & Trim(NewCode) & "' and (PackingSlip_Code_Type1 <> ''  or PackingSlip_Code_Type2 <> ''  or PackingSlip_Code_Type3 <> ''  or PackingSlip_Code_Type4 <> ''  or PackingSlip_Code_Type5 <> ''  or BuyerOffer_Code_Type1 <> ''  or BuyerOffer_Code_Type2 <> ''  or BuyerOffer_Code_Type3 <> ''  or BuyerOffer_Code_Type4 <> '' or BuyerOffer_Code_Type5 <> '') ", con)
        Dt1 = New DataTable
        Da.Fill(Dt1)
        If Dt1.Rows.Count > 0 Then
            If IsDBNull(Dt1.Rows(0)(0).ToString) = False Then
                If Val(Dt1.Rows(0)(0).ToString) > 0 Then
                    MessageBox.Show("Already this Some Rejected Piece was baled", "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                    Exit Sub
                End If
            End If
        End If
        Dt1.Clear()

        trans = con.BeginTransaction

        Try

            cmd.Connection = con
            cmd.Transaction = trans
            Call Common_Procedures.User_Modification_Updation(con, "HEAD", Me.Name, "DELETE", "Buyer_Offer_head", "Buyer_Offer_Code", Val(lbl_Company.Tag), NewCode, lbl_RefNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, True, "", "", "Buyer_Offer_Code, Company_IdNo, for_OrderBy", trans)
            Call Common_Procedures.User_Modification_Updation(con, "DETAILS", Me.Name, "DELETE", "Buyer_Offer_Details", "Buyer_Offer_Code", Val(lbl_Company.Tag), NewCode, lbl_RefNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, True, "Lot_No,Pcs_No,Pcs_ClothType_IdNo,Meters,Weight,Weight_Meter,Party_PieceNo,Pass_Meters,Less_Meters,Reject_Meters,Points,Point_Per_PassMeter,Grade,Reject_New_PieceNo,Lot_Code,Pcs_PartyIdNo,Pcs_Cloth_IdNo,Bits_Meters,Bits_New_PieceNo,Loom_IdNo,Loom_No", "Sl_No", "Buyer_Offer_Code, For_OrderBy, Company_IdNo, Buyer_Offer_No, Buyer_Offer_Date, Ledger_Idno", trans)

            cmd.CommandText = "Delete from Weaver_ClothReceipt_Piece_Details Where Weaver_Piece_Checking_Code = '" & Trim(NewCode) & "'  and Weaver_ClothReceipt_Code = '" & Trim(NewCode) & "' and Create_Status = 0 and PackingSlip_Code_Type1 = '' and PackingSlip_Code_Type2 = '' and PackingSlip_Code_Type3 = '' and PackingSlip_Code_Type4 = '' and PackingSlip_Code_Type5 = '' and BuyerOffer_Code_Type1 = '' and BuyerOffer_Code_Type2 = '' and BuyerOffer_Code_Type3 = '' and BuyerOffer_Code_Type4 = '' and BuyerOffer_Code_Type5 = ''"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Stock_Cloth_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "update Weaver_ClothReceipt_Piece_Details set BuyerOffer_Code_Type1 = '', BuyerOffer_No_Type1 = '', Buyer_RefNo_Type1 = '', BuyerOffer_Party_PieceNo_Type1 = '', BuyerOffer_Passed_Meters_Type1 = 0, BuyerOffer_Less_Meters_Type1 = 0, BuyerOffer_Reject_Meters_Type1 = 0, BuyerOffer_Points_Type1 = 0, BuyerOffer_Points_Per_PassMeter_Type1 = 0, BuyerOffer_Grade_Type1 = '', BuyerOffer_Rejection_PieceNo_Type1 = '', BuyerOffer_Bits_Meters_Type1 = 0, BuyerOffer_Bits_PieceNo_Type1 = '' Where BuyerOffer_Code_Type1 = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "update Weaver_ClothReceipt_Piece_Details set BuyerOffer_Code_Type2 = '', BuyerOffer_No_Type2 = '', Buyer_RefNo_Type2 = '', BuyerOffer_Party_PieceNo_Type2 = '', BuyerOffer_Passed_Meters_Type2 = 0, BuyerOffer_Less_Meters_Type2 = 0, BuyerOffer_Reject_Meters_Type2 = 0, BuyerOffer_Points_Type2 = 0, BuyerOffer_Points_Per_PassMeter_Type2 = 0, BuyerOffer_Grade_Type2 = '', BuyerOffer_Rejection_PieceNo_Type2 = '', BuyerOffer_Bits_Meters_Type2 = 0, BuyerOffer_Bits_PieceNo_Type2 = '' Where BuyerOffer_Code_Type2 = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "update Weaver_ClothReceipt_Piece_Details set BuyerOffer_Code_Type3 = '', BuyerOffer_No_Type3 = '', Buyer_RefNo_Type3 = '', BuyerOffer_Party_PieceNo_Type3 = '', BuyerOffer_Passed_Meters_Type3 = 0, BuyerOffer_Less_Meters_Type3 = 0, BuyerOffer_Reject_Meters_Type3 = 0, BuyerOffer_Points_Type3 = 0, BuyerOffer_Points_Per_PassMeter_Type3 = 0, BuyerOffer_Grade_Type3 = '', BuyerOffer_Rejection_PieceNo_Type3 = '', BuyerOffer_Bits_Meters_Type3 = 0, BuyerOffer_Bits_PieceNo_Type3 = '' Where BuyerOffer_Code_Type3 = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "update Weaver_ClothReceipt_Piece_Details set BuyerOffer_Code_Type4 = '', BuyerOffer_No_Type4 = '', Buyer_RefNo_Type4 = '', BuyerOffer_Party_PieceNo_Type4 = '', BuyerOffer_Passed_Meters_Type4 = 0, BuyerOffer_Less_Meters_Type4 = 0, BuyerOffer_Reject_Meters_Type4 = 0, BuyerOffer_Points_Type4 = 0, BuyerOffer_Points_Per_PassMeter_Type4 = 0, BuyerOffer_Grade_Type4 = '', BuyerOffer_Rejection_PieceNo_Type4 = '', BuyerOffer_Bits_Meters_Type4 = 0, BuyerOffer_Bits_PieceNo_Type4 = '' Where BuyerOffer_Code_Type4 = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "update Weaver_ClothReceipt_Piece_Details set BuyerOffer_Code_Type5 = '', BuyerOffer_No_Type5 = '', Buyer_RefNo_Type5 = '', BuyerOffer_Party_PieceNo_Type5 = '', BuyerOffer_Passed_Meters_Type5 = 0, BuyerOffer_Less_Meters_Type5 = 0, BuyerOffer_Reject_Meters_Type5 = 0, BuyerOffer_Points_Type5 = 0, BuyerOffer_Points_Per_PassMeter_Type5 = 0, BuyerOffer_Grade_Type5 = '', BuyerOffer_Rejection_PieceNo_Type5 = '', BuyerOffer_Bits_Meters_Type5 = 0, BuyerOffer_Bits_PieceNo_Type5 = '' Where BuyerOffer_Code_Type5 = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "delete from Buyer_Offer_Details where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Buyer_Offer_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "delete from Buyer_Offer_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Buyer_Offer_Code = '" & Trim(NewCode) & "'"
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
            cbo_Filter_PartyName.DataSource = dt2
            cbo_Filter_PartyName.DisplayMember = "ClothType_Name"


            dtp_Filter_Fromdate.Text = Common_Procedures.Company_FromDate
            dtp_Filter_ToDate.Text = Common_Procedures.Company_ToDate
            cbo_Filter_Cloth.Text = ""
            cbo_Filter_PartyName.Text = ""
            txt_Filter_LotNo.Text = ""
            txt_Filter_PcsNo.Text = ""
            txt_Filter_RollNo.Text = ""


            cbo_Filter_Cloth.SelectedIndex = -1
            cbo_Filter_PartyName.SelectedIndex = -1
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

            da = New SqlClient.SqlDataAdapter("select top 1 Buyer_offer_No from Buyer_Offer_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Buyer_Offer_Code like '" & Trim(Pk_Condition) & "%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby, Buyer_offer_No", con)
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

            OrdByNo = Common_Procedures.OrderBy_CodeToValue(Trim(lbl_RefNo.Text))

            da = New SqlClient.SqlDataAdapter("select top 1 Buyer_offer_No from Buyer_Offer_Head where for_orderby > " & Str(Val(OrdByNo)) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and Buyer_Offer_Code like '" & Trim(Pk_Condition) & "%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby, Buyer_offer_No", con)
            dt = New DataTable
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

            da = New SqlClient.SqlDataAdapter("select top 1 Buyer_offer_No from Buyer_Offer_Head where for_orderby < " & Str(Val(OrdByNo)) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and Buyer_Offer_Code like '" & Trim(Pk_Condition) & "%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, Buyer_offer_No desc", con)
            dt = New DataTable
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
            da = New SqlClient.SqlDataAdapter("select top 1 Buyer_offer_No from Buyer_Offer_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Buyer_Offer_Code like '" & Trim(Pk_Condition) & "%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, Buyer_offer_No desc", con)
            dt = New DataTable
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

            lbl_RefNo.Text = Common_Procedures.get_MaxCode(con, "Buyer_Offer_Head", "Buyer_Offer_Code", "For_OrderBy", "(Buyer_Offer_Code LIKE '" & Trim(Pk_Condition) & "%')", Val(lbl_Company.Tag), Common_Procedures.FnYearCode)

            lbl_RefNo.ForeColor = Color.Red


            msk_date.Text = Date.Today.ToShortDateString
            da = New SqlClient.SqlDataAdapter("select top 1 * from Buyer_Offer_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Buyer_Offer_Code like '" & Trim(Pk_Condition) & "%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, Buyer_offer_No desc", con)
            dt1 = New DataTable
            da.Fill(dt1)
            If dt1.Rows.Count > 0 Then
                If Val(Common_Procedures.settings.PreviousEntryDate_ByDefault) = 1 Then '---- M.S Textiles (Tirupur)
                    If dt1.Rows(0).Item("Buyer_Offer_Date").ToString <> "" Then msk_date.Text = dt1.Rows(0).Item("Buyer_Offer_Date").ToString
                End If

                cbo_PartyName.Text = Common_Procedures.Ledger_IdNoToName(con, Val(dt1.Rows(0).Item("Ledger_IdNo").ToString))
                cbo_Cloth.Text = Common_Procedures.Cloth_IdNoToName(con, Val(dt1.Rows(0).Item("Cloth_IdNo").ToString))
                cbo_ClothType.Text = Common_Procedures.ClothType_IdNoToName(con, Val(dt1.Rows(0).Item("ClothType_IdNo").ToString))
                txt_Folding.Text = Format(Val(dt1.Rows(0).Item("Folding").ToString), "########0.00")

            End If
            dt1.Clear()


            If msk_date.Enabled And msk_date.Visible Then
                msk_date.Focus()
                msk_date.SelectionStart = 0
            End If

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

            RecCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

            Da = New SqlClient.SqlDataAdapter("select Buyer_offer_No from Buyer_Offer_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Buyer_Offer_Code = '" & Trim(RecCode) & "'", con)
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
        Dim RecCode As String

        ' If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.PackinSlip_Entry, "~L~") = 0 And InStr(Common_Procedures.UR.PackinSlip_Entry, "~I~") = 0 Then MessageBox.Show("You have No Rights to Insert", "DOES NOT INSERT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error) : Exit Sub
        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.InsertingEntry, Common_Procedures.UR.ClothSales_Buyer_offer_Entry, New_Entry, Me) = False Then Exit Sub

        Try

            inpno = InputBox("Enter New Bale No.", "FOR NEW RECEIPT INSERTION...")

            RecCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

            Da = New SqlClient.SqlDataAdapter("select Buyer_offer_No from Buyer_Offer_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Buyer_Offer_Code = '" & Trim(RecCode) & "'", con)
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
                    MessageBox.Show("Invalid Ref No", "DOES NOT INSERT NEW BUYER OFFER NO...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

                Else
                    new_record()
                    Insert_Entry = True
                    lbl_RefNo.Text = Trim(UCase(inpno))

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
        Dim Dt2 As New DataTable
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
        Dim dparty_ID As Integer = 0
        Dim vTotPassMtrs As Single = 0
        Dim vTotLessMtrs As Single = 0
        Dim vTotRejMtrs As Single = 0
        Dim vTotBitMtrs As Single = 0
        Dim vTotPts As Single = 0
        Dim Nr As Long = 0
        Dim vStkOf_Pos_IdNo As Integer = 0
        Dim StkDelvTo_ID As Integer = 0, StkRecFrm_ID As Integer = 0
        Dim Rej_T1Mtrs As Single = 0
        Dim Rej_T2Mtrs As Single = 0
        Dim Rej_T3Mtrs As Single = 0
        Dim Rej_T4Mtrs As Single = 0
        Dim Rej_T5Mtrs As Single = 0
        Dim vLmIdNo As Integer = 0
        Dim vLmNo As String = ""



        Dim vOrdByNo As String = ""

        vOrdByNo = Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text)

        If Val(lbl_Company.Tag) = 0 Then
            MessageBox.Show("Invalid Company Selection", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
        End If

        ' If Common_Procedures.UserRight_Check(Common_Procedures.UR.PackinSlip_Entry, New_Entry) = False Then Exit Sub
        NewCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.SavingEntry, Common_Procedures.UR.ClothSales_Buyer_offer_Entry, New_Entry, Me, con, "Buyer_Offer_Head", "Buyer_Offer_Code", NewCode, "Buyer_Offer_Date", "(Buyer_Offer_Code = '" & Trim(NewCode) & "')", "(Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Buyer_Offer_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "')", "for_Orderby desc, Buyer_Offer_No desc", dtp_Date.Value.Date) = False Then Exit Sub


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

        led_id = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_PartyName.Text)
        If led_id = 0 Then
            MessageBox.Show("Invalid Ledger Name", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If cbo_PartyName.Enabled Then cbo_PartyName.Focus()
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

        With dgv_Details

            For i = 0 To .RowCount - 1

                If Val(.Rows(i).Cells(4).Value) <> 0 Then

                    If Trim(.Rows(i).Cells(7).Value) = "" Then
                        MessageBox.Show("Invalid Party Pcs.No", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                        If .Enabled And .Visible Then
                            .Focus()
                            .CurrentCell = .Rows(i).Cells(7)
                        End If
                        Exit Sub
                    End If

                    If Val(.Rows(i).Cells(8).Value) = 0 And Val(.Rows(i).Cells(9).Value) = 0 And Val(.Rows(i).Cells(10).Value) = 0 Then
                        MessageBox.Show("Invalid Pass/Less/Rejection Meters", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                        If .Enabled And .Visible Then
                            .Focus()
                            .CurrentCell = .Rows(i).Cells(8)
                        End If
                        Exit Sub
                    End If

                    If Val(.Rows(i).Cells(4).Value) <> (Val(.Rows(i).Cells(8).Value) + Val(.Rows(i).Cells(9).Value) + Val(.Rows(i).Cells(10).Value) + Val(.Rows(i).Cells(20).Value)) Then
                        MessageBox.Show("Mismatch of Piece Meters and Pass/Less/Rejection/Bits Meters", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                        If .Enabled And .Visible Then
                            .Focus()
                            .CurrentCell = .Rows(i).Cells(8)
                        End If
                        Exit Sub
                    End If

                    If Val(.Rows(i).Cells(10).Value) <> 0 And Trim(.Rows(i).Cells(14).Value) = "" Then
                        MessageBox.Show("Invalid Rejection Pcs No", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                        If .Enabled And .Visible Then
                            .Focus()
                            .CurrentCell = .Rows(i).Cells(14)
                        End If
                        Exit Sub
                    End If

                    If Val(.Rows(i).Cells(20).Value) <> 0 And Trim(.Rows(i).Cells(21).Value) = "" Then
                        MessageBox.Show("Invalid Bits Pcs No", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                        If .Enabled And .Visible Then
                            .Focus()
                            .CurrentCell = .Rows(i).Cells(21)
                        End If
                        Exit Sub
                    End If

                End If

            Next

        End With


        vTotMtrs = 0 : vTotPcs = 0 : vTotWgt = 0
        vTotPassMtrs = 0 : vTotLessMtrs = 0 : vTotRejMtrs = 0 : vTotPts = 0 : vTotBitMtrs = 0
        If dgv_Details_Total.RowCount > 0 Then

            vTotMtrs = Val(dgv_Details_Total.Rows(0).Cells(4).Value())
            vTotPcs = Val(dgv_Details_Total.Rows(0).Cells(2).Value())
            vTotWgt = Val(dgv_Details_Total.Rows(0).Cells(5).Value())

            vTotPassMtrs = Val(dgv_Details_Total.Rows(0).Cells(8).Value())
            vTotLessMtrs = Val(dgv_Details_Total.Rows(0).Cells(9).Value())
            vTotRejMtrs = Val(dgv_Details_Total.Rows(0).Cells(10).Value())
            vTotPts = Val(dgv_Details_Total.Rows(0).Cells(11).Value())
            vTotBitMtrs = Val(dgv_Details_Total.Rows(0).Cells(20).Value())

        End If

        tr = con.BeginTransaction

        Try

            If Insert_Entry = True Or New_Entry = False Then
                NewCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            Else

                lbl_RefNo.Text = Common_Procedures.get_MaxCode(con, "Buyer_Offer_Head", "Buyer_Offer_Code", "For_OrderBy", "(Buyer_Offer_Code LIKE '" & Trim(Pk_Condition) & "%')", Val(lbl_Company.Tag), Common_Procedures.FnYearCode, tr)

                NewCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            End If

            cmd.Connection = con
            cmd.Transaction = tr

            cmd.Parameters.Clear()
            cmd.Parameters.AddWithValue("@EntryDate", Convert.ToDateTime(msk_date.Text))

            If New_Entry = True Then

                cmd.CommandText = "Insert into Buyer_Offer_Head (   Buyer_Offer_Code    ,                 Company_IdNo     ,           Buyer_offer_No      ,                               for_OrderBy                              , Buyer_Offer_Date,       Ledger_IdNo       ,             Buyer_RefNo            ,           Cloth_IdNo      ,          ClothType_IdNo    ,               Folding             ,           Pcs            ,         Meters             ,         Total_Weight      ,        Total_Passed_Meters     ,          Total_Less_Meters     ,       Total_Reject_Meters     ,          Total_Points     ,       Total_Bits_Meters       ,               Note           ,             User_IdNo                    ) " & _
                                    "          Values           ('" & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_RefNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text))) & ",    @EntryDate   , " & Str(Val(led_id)) & ", '" & Trim(txt_BuyerRefNo.Text) & "',  " & Str(Val(Clth_ID)) & ", " & Str(Val(Clthty_ID)) & ", " & Str(Val(txt_Folding.Text)) & ", " & Str(Val(vTotPcs)) & ", " & Str(Val(vTotMtrs)) & " , " & Str(Val(vTotWgt)) & " , " & Str(Val(vTotPassMtrs)) & " , " & Str(Val(vTotLessMtrs)) & " , " & Str(Val(vTotRejMtrs)) & " , " & Str(Val(vTotPts)) & " , " & Str(Val(vTotBitMtrs)) & " , '" & Trim(txt_Note.Text) & "', " & Val(Common_Procedures.User.IdNo) & " ) "
                cmd.ExecuteNonQuery()

            Else

                Call Common_Procedures.User_Modification_Updation(con, "HEAD", Me.Name, "OLD", "Buyer_Offer_head", "Buyer_Offer_Code", Val(lbl_Company.Tag), NewCode, lbl_RefNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, False, "", "", "Buyer_Offer_Code, Company_IdNo, for_OrderBy", tr)
                Call Common_Procedures.User_Modification_Updation(con, "DETAILS", Me.Name, "OLD", "Buyer_Offer_Details", "Buyer_Offer_Code", Val(lbl_Company.Tag), NewCode, lbl_RefNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, False, "Lot_No,Pcs_No,Pcs_ClothType_IdNo,Meters,Weight,Weight_Meter,Party_PieceNo,Pass_Meters,Less_Meters,Reject_Meters,Points,Point_Per_PassMeter,Grade,Reject_New_PieceNo,Lot_Code,Pcs_PartyIdNo,Pcs_Cloth_IdNo,Bits_Meters,Bits_New_PieceNo,Loom_IdNo,Loom_No", "Sl_No", "Buyer_Offer_Code, For_OrderBy, Company_IdNo, Buyer_Offer_No, Buyer_Offer_Date, Ledger_Idno", tr)


                cmd.CommandText = "Update Buyer_Offer_Head set Buyer_Offer_Date = @EntryDate, Ledger_IdNo = " & Str(Val(led_id)) & " , Buyer_RefNo = '" & Trim(txt_BuyerRefNo.Text) & "' , Cloth_IdNo = " & Str(Val(Clth_ID)) & " , ClothType_IdNo = " & Str(Val(Clthty_ID)) & " , Folding = " & Str(Val(txt_Folding.Text)) & ", Pcs = " & Str(Val(vTotPcs)) & ", Meters = " & Str(Val(vTotMtrs)) & " , Total_Weight = " & Str(Val(vTotWgt)) & " , Total_Passed_Meters = " & Str(Val(vTotPassMtrs)) & " , Total_Less_Meters = " & Str(Val(vTotLessMtrs)) & " , Total_Reject_Meters = " & Str(Val(vTotRejMtrs)) & " , Total_Points = " & Str(Val(vTotPts)) & " , Total_Bits_Meters = " & Str(Val(vTotBitMtrs)) & " , Note = '" & Trim(txt_Note.Text) & "' , User_IdNo = " & Val(Common_Procedures.User.IdNo) & " Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Buyer_Offer_Code = '" & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()

                cmd.CommandText = "update Weaver_ClothReceipt_Piece_Details set BuyerOffer_Code_Type1 = '', BuyerOffer_No_Type1 = '', Buyer_RefNo_Type1 = '', BuyerOffer_Party_PieceNo_Type1 = '', BuyerOffer_Passed_Meters_Type1 = 0, BuyerOffer_Less_Meters_Type1 = 0, BuyerOffer_Reject_Meters_Type1 = 0, BuyerOffer_Points_Type1 = 0, BuyerOffer_Points_Per_PassMeter_Type1 = 0, BuyerOffer_Grade_Type1 = '', BuyerOffer_Rejection_PieceNo_Type1 = '', BuyerOffer_Bits_Meters_Type1 = 0, BuyerOffer_Bits_PieceNo_Type1 = '' Where BuyerOffer_Code_Type1 = '" & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()

                cmd.CommandText = "update Weaver_ClothReceipt_Piece_Details set BuyerOffer_Code_Type2 = '', BuyerOffer_No_Type2 = '', Buyer_RefNo_Type2 = '', BuyerOffer_Party_PieceNo_Type2 = '', BuyerOffer_Passed_Meters_Type2 = 0, BuyerOffer_Less_Meters_Type2 = 0, BuyerOffer_Reject_Meters_Type2 = 0, BuyerOffer_Points_Type2 = 0, BuyerOffer_Points_Per_PassMeter_Type2 = 0, BuyerOffer_Grade_Type2 = '', BuyerOffer_Rejection_PieceNo_Type2 = '', BuyerOffer_Bits_Meters_Type2 = 0, BuyerOffer_Bits_PieceNo_Type2 = ''  Where BuyerOffer_Code_Type2 = '" & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()

                cmd.CommandText = "update Weaver_ClothReceipt_Piece_Details set BuyerOffer_Code_Type3 = '', BuyerOffer_No_Type3 = '', Buyer_RefNo_Type3 = '', BuyerOffer_Party_PieceNo_Type3 = '', BuyerOffer_Passed_Meters_Type3 = 0, BuyerOffer_Less_Meters_Type3 = 0, BuyerOffer_Reject_Meters_Type3 = 0, BuyerOffer_Points_Type3 = 0, BuyerOffer_Points_Per_PassMeter_Type3 = 0, BuyerOffer_Grade_Type3 = '', BuyerOffer_Rejection_PieceNo_Type3 = '', BuyerOffer_Bits_Meters_Type3 = 0, BuyerOffer_Bits_PieceNo_Type3 = ''  Where BuyerOffer_Code_Type3 = '" & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()

                cmd.CommandText = "update Weaver_ClothReceipt_Piece_Details set BuyerOffer_Code_Type4 = '', BuyerOffer_No_Type4 = '', Buyer_RefNo_Type4 = '', BuyerOffer_Party_PieceNo_Type4 = '', BuyerOffer_Passed_Meters_Type4 = 0, BuyerOffer_Less_Meters_Type4 = 0, BuyerOffer_Reject_Meters_Type4 = 0, BuyerOffer_Points_Type4 = 0, BuyerOffer_Points_Per_PassMeter_Type4 = 0, BuyerOffer_Grade_Type4 = '', BuyerOffer_Rejection_PieceNo_Type4 = '', BuyerOffer_Bits_Meters_Type4 = 0, BuyerOffer_Bits_PieceNo_Type4 = ''  Where BuyerOffer_Code_Type4 = '" & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()

                cmd.CommandText = "update Weaver_ClothReceipt_Piece_Details set BuyerOffer_Code_Type5 = '', BuyerOffer_No_Type5 = '', Buyer_RefNo_Type5 = '', BuyerOffer_Party_PieceNo_Type5 = '', BuyerOffer_Passed_Meters_Type5 = 0, BuyerOffer_Less_Meters_Type5 = 0, BuyerOffer_Reject_Meters_Type5 = 0, BuyerOffer_Points_Type5 = 0, BuyerOffer_Points_Per_PassMeter_Type5 = 0, BuyerOffer_Grade_Type5 = '', BuyerOffer_Rejection_PieceNo_Type5 = '', BuyerOffer_Bits_Meters_Type5 = 0, BuyerOffer_Bits_PieceNo_Type5 = ''  Where BuyerOffer_Code_Type5 = '" & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()

            End If
            Call Common_Procedures.User_Modification_Updation(con, "HEAD", Me.Name, "NEW", "Buyer_Offer_head", "Buyer_Offer_Code", Val(lbl_Company.Tag), NewCode, lbl_RefNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, False, "", "", "Buyer_Offer_Code, Company_IdNo, for_OrderBy", tr)
          
            EntID = Trim(Pk_Condition) & Trim(lbl_RefNo.Text)
            If Trim(lbl_RefNo.Text) <> "" Then
                Partcls = "BuyerOffer :  Buyer-RefNo. " & Trim(txt_BuyerRefNo.Text)
            End If
            PBlNo = Trim(lbl_RefNo.Text)

            cmd.CommandText = "Delete from Buyer_Offer_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Buyer_Offer_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Stock_Cloth_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Weaver_ClothReceipt_Piece_Details Where Weaver_Piece_Checking_Code = '" & Trim(NewCode) & "'  and Weaver_ClothReceipt_Code = '" & Trim(NewCode) & "' and Create_Status = 0 and PackingSlip_Code_Type1 = '' and PackingSlip_Code_Type2 = ''  and PackingSlip_Code_Type3 = '' and PackingSlip_Code_Type4 = '' and PackingSlip_Code_Type5 = '' and BuyerOffer_Code_Type1 = '' and BuyerOffer_Code_Type2 = '' and BuyerOffer_Code_Type3 = '' and BuyerOffer_Code_Type4 = '' and BuyerOffer_Code_Type5 = ''"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Truncate table " & Trim(Common_Procedures.EntryTempSubTable) & ""
            cmd.ExecuteNonQuery()

            vStkOf_Pos_IdNo = Val(Common_Procedures.CommonLedger.Godown_Ac)

            With dgv_Details
                Sno = 0

                For i = 0 To .RowCount - 1

                    If Val(.Rows(i).Cells(4).Value) <> 0 Then

                        dCloTyp_ID = Common_Procedures.ClothType_NameToIdNo(con, .Rows(i).Cells(3).Value, tr)
                        dparty_ID = Common_Procedures.Ledger_AlaisNameToIdNo(con, .Rows(i).Cells(16).Value, tr)
                        dClo_ID = Common_Procedures.Cloth_NameToIdNo(con, .Rows(i).Cells(17).Value, tr)

                        vLmNo = .Rows(i).Cells(23).Value
                        vLmIdNo = Common_Procedures.Loom_NameToIdNo(con, .Rows(i).Cells(23).Value, tr)

                        Sno = Sno + 1
                        cmd.CommandText = "Insert into Buyer_Offer_Details (     Buyer_Offer_Code   ,                 Company_IdNo     ,          Buyer_offer_No       ,                               for_OrderBy                              , Buyer_Offer_Date,        Ledger_IdNo      ,           Cloth_IdNo      ,          ClothType_IdNo     ,                  Folding           ,              Sl_No    ,                     Lot_No              ,                    Pcs_No              ,        Pcs_ClothType_IdNo   ,                      Meters              ,                      Weight              ,                      Weight_Meter        ,                    Party_PieceNo        ,                      Pass_Meters         ,                      Less_Meters         ,                      Reject_Meters        ,                      Points               ,                    Point_Per_PassMeter    ,                    Grade                 ,                    Reject_New_PieceNo    ,                    Lot_Code              ,          Pcs_PartyIdNo      ,       Pcs_Cloth_IdNo     ,                      Bits_Meters           ,                    Bits_New_PieceNo     ,             Loom_IdNo     ,          Loom_No       ) " & _
                                            "          Values              ( '" & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_RefNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text))) & ",     @EntryDate  , " & Str(Val(led_id)) & ",  " & Str(Val(Clth_ID)) & ",  " & Str(Val(Clthty_ID)) & ",  " & Str(Val(txt_Folding.Text)) & ",  " & Str(Val(Sno)) & ",  '" & Trim(.Rows(i).Cells(1).Value) & "', '" & Trim(.Rows(i).Cells(2).Value) & "', " & Str(Val(dCloTyp_ID)) & ", " & Str(Val(.Rows(i).Cells(4).Value)) & ", " & Str(Val(.Rows(i).Cells(5).Value)) & ", " & Str(Val(.Rows(i).Cells(6).Value)) & ", '" & Trim(.Rows(i).Cells(7).Value) & "' , " & Str(Val(.Rows(i).Cells(8).Value)) & ", " & Str(Val(.Rows(i).Cells(9).Value)) & ", " & Str(Val(.Rows(i).Cells(10).Value)) & ", " & Str(Val(.Rows(i).Cells(11).Value)) & ", " & Str(Val(.Rows(i).Cells(12).Value)) & ", '" & Trim(.Rows(i).Cells(13).Value) & "' , '" & Trim(.Rows(i).Cells(14).Value) & "' , '" & Trim(.Rows(i).Cells(15).Value) & "' , " & Str(Val(dparty_ID)) & " , " & Str(Val(dClo_ID)) & ", " & Str(Val(.Rows(i).Cells(20).Value)) & " , '" & Trim(.Rows(i).Cells(21).Value) & "', " & Str(Val(vLmIdNo)) & " , '" & Trim(vLmNo) & "'  ) "
                        cmd.ExecuteNonQuery()

                        If dCloTyp_ID = 1 Then
                            Nr = 0
                            cmd.CommandText = "Update Weaver_ClothReceipt_Piece_Details set BuyerOffer_Code_Type1 = '" & Trim(NewCode) & "', BuyerOffer_No_Type1 = '" & Trim(lbl_RefNo.Text) & "', Buyer_RefNo_Type1 = '" & Trim(txt_BuyerRefNo.Text) & "', BuyerOffer_Party_PieceNo_Type1 = '" & Trim(.Rows(i).Cells(7).Value) & "', BuyerOffer_Passed_Meters_Type1 = " & Str(Val(.Rows(i).Cells(8).Value)) & ", BuyerOffer_Less_Meters_Type1 = " & Str(Val(.Rows(i).Cells(9).Value)) & ", BuyerOffer_Reject_Meters_Type1 = " & Str(Val(.Rows(i).Cells(10).Value)) & ", BuyerOffer_Points_Type1 = " & Str(Val(.Rows(i).Cells(11).Value)) & ", BuyerOffer_Points_Per_PassMeter_Type1 = " & Str(Val(.Rows(i).Cells(12).Value)) & ", BuyerOffer_Grade_Type1 = '" & Trim(.Rows(i).Cells(13).Value) & "' , BuyerOffer_Rejection_PieceNo_Type1 = '" & Trim(.Rows(i).Cells(14).Value) & "', BuyerOffer_Bits_Meters_Type1 = " & Str(Val(.Rows(i).Cells(20).Value)) & ", BuyerOffer_Bits_PieceNo_Type1 = '" & Trim(.Rows(i).Cells(21).Value) & "' Where lot_code = '" & Trim(.Rows(i).Cells(15).Value) & "' and Piece_No = '" & Trim(.Rows(i).Cells(2).Value) & "' and BuyerOffer_Code_Type1 = ''"
                            Nr = cmd.ExecuteNonQuery()
                            If Nr = 0 Then
                                Throw New ApplicationException("Mismatch of Piece Details")
                                Exit Sub
                            End If

                            If Val(.Rows(i).Cells(9).Value) <> 0 Then
                                cmd.CommandText = "Insert into " & Trim(Common_Procedures.EntryTempSubTable) & "(Int1, Meters1) values (" & Str(Val(dClo_ID)) & ", " & Str(Val(.Rows(i).Cells(9).Value)) & ")"
                                cmd.ExecuteNonQuery()
                            End If
                            'If Val(.Rows(i).Cells(10).Value) <> 0 Then
                            '    cmd.CommandText = "Insert into " & Trim(Common_Procedures.EntryTempSubTable) & "(Int1, Meters1, Currency4) values (" & Str(Val(dClo_ID)) & ", " & Str(Val(.Rows(i).Cells(10).Value)) & ", " & Str(Val(.Rows(i).Cells(10).Value)) & ")"
                            '    cmd.ExecuteNonQuery()
                            'End If
                            If Val(.Rows(i).Cells(20).Value) <> 0 Then
                                cmd.CommandText = "Insert into " & Trim(Common_Procedures.EntryTempSubTable) & " ( Int1, Meters1, Currency3 ) Values ( " & Str(Val(dClo_ID)) & ", " & Str(Val(.Rows(i).Cells(20).Value)) & ", " & Str(Val(.Rows(i).Cells(20).Value)) & " )"
                                cmd.ExecuteNonQuery()
                            End If


                        ElseIf dCloTyp_ID = 2 Then
                            Nr = 0
                            cmd.CommandText = "update Weaver_ClothReceipt_Piece_Details set BuyerOffer_Code_Type2 = '" & Trim(NewCode) & "', BuyerOffer_No_Type2 = '" & Trim(lbl_RefNo.Text) & "', Buyer_RefNo_Type2 = '" & Trim(txt_BuyerRefNo.Text) & "', BuyerOffer_Party_PieceNo_Type2 = '" & Trim(.Rows(i).Cells(7).Value) & "', BuyerOffer_Passed_Meters_Type2 = " & Str(Val(.Rows(i).Cells(8).Value)) & ", BuyerOffer_Less_Meters_Type2 = " & Str(Val(.Rows(i).Cells(9).Value)) & ", BuyerOffer_Reject_Meters_Type2 = " & Str(Val(.Rows(i).Cells(10).Value)) & ", BuyerOffer_Points_Type2 = " & Str(Val(.Rows(i).Cells(11).Value)) & ", BuyerOffer_Points_Per_PassMeter_Type2 = " & Str(Val(.Rows(i).Cells(12).Value)) & ", BuyerOffer_Grade_Type2 = '" & Trim(.Rows(i).Cells(13).Value) & "' , BuyerOffer_Rejection_PieceNo_Type2 = '" & Trim(.Rows(i).Cells(14).Value) & "', BuyerOffer_Bits_Meters_Type2 = " & Str(Val(.Rows(i).Cells(20).Value)) & ", BuyerOffer_Bits_PieceNo_Type2 = '" & Trim(.Rows(i).Cells(21).Value) & "'   Where lot_code = '" & Trim(.Rows(i).Cells(15).Value) & "' and Piece_No = '" & Trim(.Rows(i).Cells(2).Value) & "' and BuyerOffer_Code_Type2 = ''"
                            Nr = cmd.ExecuteNonQuery()
                            If Nr = 0 Then
                                Throw New ApplicationException("Mismatch of Piece Details")
                                Exit Sub
                            End If

                            If Val(.Rows(i).Cells(9).Value) <> 0 Then
                                cmd.CommandText = "Insert into " & Trim(Common_Procedures.EntryTempSubTable) & "(Int1, Meters2) values (" & Str(Val(dClo_ID)) & ", " & Str(Val(.Rows(i).Cells(9).Value)) & ")"
                                cmd.ExecuteNonQuery()
                            End If
                            'If Val(.Rows(i).Cells(10).Value) <> 0 Then
                            '    cmd.CommandText = "Insert into " & Trim(Common_Procedures.EntryTempSubTable) & "(Int1, Meters2, Currency4) values (" & Str(Val(dClo_ID)) & ", " & Str(Val(.Rows(i).Cells(10).Value)) & ", " & Str(Val(.Rows(i).Cells(10).Value)) & ")"
                            '    cmd.ExecuteNonQuery()
                            'End If
                            If Val(.Rows(i).Cells(20).Value) <> 0 Then
                                cmd.CommandText = "Insert into " & Trim(Common_Procedures.EntryTempSubTable) & " ( Int1, Meters2, Currency3 ) Values (" & Str(Val(dClo_ID)) & ", " & Str(Val(.Rows(i).Cells(20).Value)) & " , " & Str(Val(.Rows(i).Cells(20).Value)) & " )"
                                cmd.ExecuteNonQuery()
                            End If

                        ElseIf dCloTyp_ID = 3 Then
                            Nr = 0
                            cmd.CommandText = "update Weaver_ClothReceipt_Piece_Details set BuyerOffer_Code_Type3 = '" & Trim(NewCode) & "', BuyerOffer_No_Type3 = '" & Trim(lbl_RefNo.Text) & "', Buyer_RefNo_Type3 = '" & Trim(txt_BuyerRefNo.Text) & "', BuyerOffer_Party_PieceNo_Type3 = '" & Trim(.Rows(i).Cells(7).Value) & "', BuyerOffer_Passed_Meters_Type3 = " & Str(Val(.Rows(i).Cells(8).Value)) & ", BuyerOffer_Less_Meters_Type3 = " & Str(Val(.Rows(i).Cells(9).Value)) & ", BuyerOffer_Reject_Meters_Type3 = " & Str(Val(.Rows(i).Cells(10).Value)) & ", BuyerOffer_Points_Type3 = " & Str(Val(.Rows(i).Cells(11).Value)) & ", BuyerOffer_Points_Per_PassMeter_Type3 = " & Str(Val(.Rows(i).Cells(12).Value)) & ", BuyerOffer_Grade_Type3 = '" & Trim(.Rows(i).Cells(13).Value) & "' , BuyerOffer_Rejection_PieceNo_Type3 = '" & Trim(.Rows(i).Cells(14).Value) & "', BuyerOffer_Bits_Meters_Type3 = " & Str(Val(.Rows(i).Cells(20).Value)) & ", BuyerOffer_Bits_PieceNo_Type3 = '" & Trim(.Rows(i).Cells(21).Value) & "' Where lot_code = '" & Trim(.Rows(i).Cells(15).Value) & "' and Piece_No = '" & Trim(.Rows(i).Cells(2).Value) & "' and BuyerOffer_Code_Type3 = ''"
                            Nr = cmd.ExecuteNonQuery()
                            If Nr = 0 Then
                                Throw New ApplicationException("Mismatch of Piece Details")
                                Exit Sub
                            End If

                            If Val(.Rows(i).Cells(9).Value) <> 0 Then
                                cmd.CommandText = "Insert into " & Trim(Common_Procedures.EntryTempSubTable) & "(Int1, Meters3) values (" & Str(Val(dClo_ID)) & ", " & Str(Val(.Rows(i).Cells(9).Value)) & ")"
                                cmd.ExecuteNonQuery()
                            End If
                            'If Val(.Rows(i).Cells(10).Value) <> 0 Then
                            '    cmd.CommandText = "Insert into " & Trim(Common_Procedures.EntryTempSubTable) & "(Int1, Meters3, Currency4) values (" & Str(Val(dClo_ID)) & ", " & Str(Val(.Rows(i).Cells(10).Value)) & ", " & Str(Val(.Rows(i).Cells(10).Value)) & ")"
                            '    cmd.ExecuteNonQuery()
                            'End If
                            If Val(.Rows(i).Cells(20).Value) <> 0 Then
                                cmd.CommandText = "Insert into " & Trim(Common_Procedures.EntryTempSubTable) & " ( Int1, Meters3, Currency3 ) Values (" & Str(Val(dClo_ID)) & ", " & Str(Val(.Rows(i).Cells(20).Value)) & " , " & Str(Val(.Rows(i).Cells(20).Value)) & " )"
                                cmd.ExecuteNonQuery()
                            End If

                        ElseIf dCloTyp_ID = 4 Then
                            Nr = 0
                            cmd.CommandText = "update Weaver_ClothReceipt_Piece_Details set BuyerOffer_Code_Type4 = '" & Trim(NewCode) & "', BuyerOffer_No_Type4 = '" & Trim(lbl_RefNo.Text) & "', Buyer_RefNo_Type4 = '" & Trim(txt_BuyerRefNo.Text) & "', BuyerOffer_Party_PieceNo_Type4 = '" & Trim(.Rows(i).Cells(7).Value) & "', BuyerOffer_Passed_Meters_Type4 = " & Str(Val(.Rows(i).Cells(8).Value)) & ", BuyerOffer_Less_Meters_Type4 = " & Str(Val(.Rows(i).Cells(9).Value)) & ", BuyerOffer_Reject_Meters_Type4 = " & Str(Val(.Rows(i).Cells(10).Value)) & ", BuyerOffer_Points_Type4 = " & Str(Val(.Rows(i).Cells(11).Value)) & ", BuyerOffer_Points_Per_PassMeter_Type4 = " & Str(Val(.Rows(i).Cells(12).Value)) & ", BuyerOffer_Grade_Type4 = '" & Trim(.Rows(i).Cells(13).Value) & "' , BuyerOffer_Rejection_PieceNo_Type4 = '" & Trim(.Rows(i).Cells(14).Value) & "', BuyerOffer_Bits_Meters_Type4 = " & Str(Val(.Rows(i).Cells(20).Value)) & ", BuyerOffer_Bits_PieceNo_Type4 = '" & Trim(.Rows(i).Cells(21).Value) & "' Where lot_code = '" & Trim(.Rows(i).Cells(15).Value) & "' and Piece_No = '" & Trim(.Rows(i).Cells(2).Value) & "' and BuyerOffer_Code_Type4 = ''"
                            Nr = cmd.ExecuteNonQuery()
                            If Nr = 0 Then
                                Throw New ApplicationException("Mismatch of Piece Details")
                                Exit Sub
                            End If

                            If Val(.Rows(i).Cells(9).Value) <> 0 Then
                                cmd.CommandText = "Insert into " & Trim(Common_Procedures.EntryTempSubTable) & "(Int1, Meters4) values (" & Str(Val(dClo_ID)) & ", " & Str(Val(.Rows(i).Cells(9).Value)) & ")"
                                cmd.ExecuteNonQuery()
                            End If
                            'If Val(.Rows(i).Cells(10).Value) <> 0 Then
                            '    cmd.CommandText = "Insert into " & Trim(Common_Procedures.EntryTempSubTable) & "(Int1, Meters4, Currency4) values (" & Str(Val(dClo_ID)) & ", " & Str(Val(.Rows(i).Cells(10).Value)) & ", " & Str(Val(.Rows(i).Cells(10).Value)) & ")"
                            '    cmd.ExecuteNonQuery()
                            'End If
                            If Val(.Rows(i).Cells(20).Value) <> 0 Then
                                cmd.CommandText = "Insert into " & Trim(Common_Procedures.EntryTempSubTable) & " ( Int1, Meters4, Currency3 ) Values (" & Str(Val(dClo_ID)) & ", " & Str(Val(.Rows(i).Cells(20).Value)) & " , " & Str(Val(.Rows(i).Cells(20).Value)) & " )"
                                cmd.ExecuteNonQuery()
                            End If

                        ElseIf dCloTyp_ID = 5 Then
                            Nr = 0
                            cmd.CommandText = "update Weaver_ClothReceipt_Piece_Details set BuyerOffer_Code_Type5 = '" & Trim(NewCode) & "', BuyerOffer_No_Type5 = '" & Trim(lbl_RefNo.Text) & "', Buyer_RefNo_Type5 = '" & Trim(txt_BuyerRefNo.Text) & "', BuyerOffer_Party_PieceNo_Type5 = '" & Trim(.Rows(i).Cells(7).Value) & "', BuyerOffer_Passed_Meters_Type5 = " & Str(Val(.Rows(i).Cells(8).Value)) & ", BuyerOffer_Less_Meters_Type5 = " & Str(Val(.Rows(i).Cells(9).Value)) & ", BuyerOffer_Reject_Meters_Type5 = " & Str(Val(.Rows(i).Cells(10).Value)) & ", BuyerOffer_Points_Type5 = " & Str(Val(.Rows(i).Cells(11).Value)) & ", BuyerOffer_Points_Per_PassMeter_Type5 = " & Str(Val(.Rows(i).Cells(12).Value)) & ", BuyerOffer_Grade_Type5 = '" & Trim(.Rows(i).Cells(13).Value) & "' , BuyerOffer_Rejection_PieceNo_Type5 = '" & Trim(.Rows(i).Cells(14).Value) & "', BuyerOffer_Bits_Meters_Type5 = " & Str(Val(.Rows(i).Cells(20).Value)) & ", BuyerOffer_Bits_PieceNo_Type5 = '" & Trim(.Rows(i).Cells(21).Value) & "'   Where lot_code = '" & Trim(.Rows(i).Cells(15).Value) & "' and Piece_No = '" & Trim(.Rows(i).Cells(2).Value) & "' and BuyerOffer_Code_Type5 = ''"
                            Nr = cmd.ExecuteNonQuery()
                            If Nr = 0 Then
                                Throw New ApplicationException("Mismatch of Piece Details")
                                Exit Sub
                            End If

                            If Val(.Rows(i).Cells(9).Value) <> 0 Then
                                cmd.CommandText = "Insert into " & Trim(Common_Procedures.EntryTempSubTable) & "(Int1, Meters5) values (" & Str(Val(dClo_ID)) & ", " & Str(Val(.Rows(i).Cells(9).Value)) & ")"
                                cmd.ExecuteNonQuery()
                            End If
                            'If Val(.Rows(i).Cells(10).Value) <> 0 Then
                            '    cmd.CommandText = "Insert into " & Trim(Common_Procedures.EntryTempSubTable) & "(Int1, Meters5, Currency4) values (" & Str(Val(dClo_ID)) & ", " & Str(Val(.Rows(i).Cells(10).Value)) & ", " & Str(Val(.Rows(i).Cells(10).Value)) & ")"
                            '    cmd.ExecuteNonQuery()
                            'End If
                            If Val(.Rows(i).Cells(20).Value) <> 0 Then
                                cmd.CommandText = "Insert into " & Trim(Common_Procedures.EntryTempSubTable) & " ( Int1, Meters5, Currency3 ) Values (" & Str(Val(dClo_ID)) & ", " & Str(Val(.Rows(i).Cells(20).Value)) & " , " & Str(Val(.Rows(i).Cells(20).Value)) & " )"
                                cmd.ExecuteNonQuery()
                            End If

                        End If


                        '--- Reject Pcs Updation
                        If Val(.Rows(i).Cells(10).Value) <> 0 And Trim(.Rows(i).Cells(15).Value) <> "" And Trim(.Rows(i).Cells(14).Value) <> "" Then

                            Rej_T1Mtrs = 0
                            Rej_T2Mtrs = 0
                            Rej_T3Mtrs = 0
                            Rej_T4Mtrs = 0
                            Rej_T5Mtrs = 0

                            If dCloTyp_ID = 5 Then
                                Rej_T5Mtrs = Val(.Rows(i).Cells(10).Value)

                            ElseIf dCloTyp_ID = 4 Then
                                Rej_T4Mtrs = Val(.Rows(i).Cells(10).Value)

                            ElseIf dCloTyp_ID = 3 Then
                                Rej_T3Mtrs = Val(.Rows(i).Cells(10).Value)

                            ElseIf dCloTyp_ID = 2 Then
                                Rej_T2Mtrs = Val(.Rows(i).Cells(10).Value)

                            Else
                                Rej_T1Mtrs = Val(.Rows(i).Cells(10).Value)

                            End If

                            Nr = 0
                            cmd.CommandText = "Update Weaver_ClothReceipt_Piece_Details set Weaver_Piece_Checking_Code = '" & Trim(NewCode) & "',  Weaver_Piece_Checking_No = '" & Trim(lbl_RefNo.Text) & "', Weaver_Piece_Checking_Date = @EntryDate, Ledger_Idno = " & Str(Val(dparty_ID)) & ", StockOff_IdNo = " & Str(Val(vStkOf_Pos_IdNo)) & ", Cloth_IdNo = " & Str(Val(Clth_ID)) & ", Folding_Checking = " & Str(Val(txt_Folding.Text)) & ", Folding = " & Str(Val(txt_Folding.Text)) & ", Sl_No = " & Str(Val(i + 2000)) & ", PieceNo_OrderBy = " & Str(Val(Common_Procedures.OrderBy_CodeToValue(Val(.Rows(i).Cells(14).Value)))) & ", ReceiptMeters_Checking = 0, Receipt_Meters = 0, Loom_No = '', Pick = 0, Width = 0, Type1_Meters = " & Str(Val(Rej_T1Mtrs)) & ", Type2_Meters = " & Str(Val(Rej_T2Mtrs)) & ", Type3_Meters = " & Str(Val(Rej_T3Mtrs)) & ", Type4_Meters = " & Str(Val(Rej_T4Mtrs)) & ", Type5_Meters = " & Str(Val(Rej_T5Mtrs)) & ", Total_Checking_Meters = " & Str(Val(.Rows(i).Cells(10).Value)) & ", Weight = " & Str(Val(.Rows(i).Cells(10).Value) * Val(.Rows(i).Cells(6).Value)) & ", Weight_Meter = " & Str(Val(.Rows(i).Cells(6).Value)) & " Where Weaver_Piece_Checking_Code = '" & Trim(NewCode) & "' and Weaver_ClothReceipt_Code = '" & Trim(NewCode) & "' and Lot_Code = '" & Trim(.Rows(i).Cells(15).Value) & "' and Piece_No = '" & Trim(.Rows(i).Cells(14).Value) & "'"
                            Nr = cmd.ExecuteNonQuery()

                            If Nr = 0 Then
                                cmd.CommandText = "Insert into Weaver_ClothReceipt_Piece_Details ( Weaver_Piece_Checking_Code ,             Company_IdNo         ,     Weaver_Piece_Checking_No   ,  Weaver_Piece_Checking_Date, Weaver_ClothReceipt_Code,    Weaver_ClothReceipt_No     ,                               for_orderby                              , Weaver_ClothReceipt_Date,                    Lot_Code             ,                    Lot_No              ,           Ledger_Idno      ,            StockOff_IdNo         ,           Cloth_IdNo     ,            Folding_Checking       ,             Folding               ,           Sl_No           ,                    Piece_No             ,                                PieceNo_OrderBy                                          ,  ReceiptMeters_Checking,  Receipt_Meters , Loom_No , Pick,  Width,         Type1_Meters        ,         Type2_Meters        ,         Type3_Meters         ,         Type4_Meters        ,       Type5_Meters          ,            Total_Checking_Meters          ,                      Weight                                               ,                       Weight_Meter         ) " & _
                                                    "          Values                            (    '" & Trim(NewCode) & "' , " & Str(Val(lbl_Company.Tag)) & ",  '" & Trim(lbl_RefNo.Text) & "',        @EntryDate          , '" & Trim(NewCode) & "' , '" & Trim(lbl_RefNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text))) & ",      @EntryDate         , '" & Trim(.Rows(i).Cells(15).Value) & "', '" & Trim(.Rows(i).Cells(1).Value) & "', " & Str(Val(dparty_ID)) & ", " & Str(Val(vStkOf_Pos_IdNo)) & ", " & Str(Val(dClo_ID)) & ", " & Str(Val(txt_Folding.Text)) & ", " & Str(Val(txt_Folding.Text)) & ", " & Str(Val(i + 2000)) & ", '" & Trim(.Rows(i).Cells(14).Value) & "',  " & Str(Val(Common_Procedures.OrderBy_CodeToValue(Trim(.Rows(i).Cells(14).Value)))) & ",             0          ,        0        ,    ''   ,  0  ,    0  , " & Str(Val(Rej_T1Mtrs)) & ", " & Str(Val(Rej_T2Mtrs)) & ",  " & Str(Val(Rej_T3Mtrs)) & ", " & Str(Val(Rej_T4Mtrs)) & ", " & Str(Val(Rej_T5Mtrs)) & ", " & Str(Val(.Rows(i).Cells(10).Value)) & ", " & Str(Val(.Rows(i).Cells(10).Value) * Val(.Rows(i).Cells(6).Value)) & " ,  " & Str(Val(.Rows(i).Cells(6).Value)) & " ) "
                                cmd.ExecuteNonQuery()
                            End If

                        End If


                        '--- Bits Pcs Updation
                        If Val(.Rows(i).Cells(20).Value) <> 0 And Trim(.Rows(i).Cells(15).Value) <> "" And Trim(.Rows(i).Cells(21).Value) <> "" Then

                            Nr = 0
                            cmd.CommandText = "Update Weaver_ClothReceipt_Piece_Details set Weaver_Piece_Checking_Code = '" & Trim(NewCode) & "',  Weaver_Piece_Checking_No = '" & Trim(lbl_RefNo.Text) & "', Weaver_Piece_Checking_Date = @EntryDate, Ledger_Idno = " & Str(Val(dparty_ID)) & ", StockOff_IdNo = " & Str(Val(vStkOf_Pos_IdNo)) & ", Cloth_IdNo = " & Str(Val(Clth_ID)) & ", Folding_Checking = " & Str(Val(txt_Folding.Text)) & ", Folding = " & Str(Val(txt_Folding.Text)) & ", Sl_No = " & Str(Val(i + 3000)) & ", PieceNo_OrderBy = " & Str(Val(Common_Procedures.OrderBy_CodeToValue(Val(.Rows(i).Cells(21).Value)))) & ", ReceiptMeters_Checking = 0, Receipt_Meters = 0, Loom_No = '', Pick = 0, Width = 0, Type1_Meters = 0, Type2_Meters = 0, Type3_Meters = " & Str(Val(Val(.Rows(i).Cells(20).Value))) & ", Type4_Meters = 0, Type5_Meters = 0, Total_Checking_Meters = " & Str(Val(.Rows(i).Cells(20).Value)) & ", Weight = " & Str(Val(.Rows(i).Cells(20).Value) * Val(.Rows(i).Cells(6).Value)) & ", Weight_Meter = " & Str(Val(.Rows(i).Cells(6).Value)) & " Where Weaver_Piece_Checking_Code = '" & Trim(NewCode) & "' and Weaver_ClothReceipt_Code = '" & Trim(NewCode) & "' and Lot_Code = '" & Trim(.Rows(i).Cells(15).Value) & "' and Piece_No = '" & Trim(.Rows(i).Cells(21).Value) & "'"
                            Nr = cmd.ExecuteNonQuery()

                            If Nr = 0 Then
                                cmd.CommandText = "Insert into Weaver_ClothReceipt_Piece_Details ( Weaver_Piece_Checking_Code ,             Company_IdNo         ,     Weaver_Piece_Checking_No   ,  Weaver_Piece_Checking_Date, Weaver_ClothReceipt_Code,    Weaver_ClothReceipt_No     ,                               for_orderby                              , Weaver_ClothReceipt_Date,                    Lot_Code             ,                    Lot_No              ,           Ledger_Idno      ,            StockOff_IdNo         ,           Cloth_IdNo     ,            Folding_Checking       ,             Folding               ,           Sl_No           ,                    Piece_No             ,                                PieceNo_OrderBy                                          ,  ReceiptMeters_Checking,  Receipt_Meters , Loom_No , Pick,  Width, Type1_Meters , Type2_Meters ,                       Type3_Meters         , Type4_Meters , Type5_Meters ,            Total_Checking_Meters          ,                      Weight                                               ,                       Weight_Meter         ) " & _
                                                    "          Values                            (    '" & Trim(NewCode) & "' , " & Str(Val(lbl_Company.Tag)) & ",  '" & Trim(lbl_RefNo.Text) & "',        @EntryDate          , '" & Trim(NewCode) & "' , '" & Trim(lbl_RefNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text))) & ",      @EntryDate         , '" & Trim(.Rows(i).Cells(15).Value) & "', '" & Trim(.Rows(i).Cells(1).Value) & "', " & Str(Val(dparty_ID)) & ", " & Str(Val(vStkOf_Pos_IdNo)) & ", " & Str(Val(dClo_ID)) & ", " & Str(Val(txt_Folding.Text)) & ", " & Str(Val(txt_Folding.Text)) & ", " & Str(Val(i + 3000)) & ", '" & Trim(.Rows(i).Cells(21).Value) & "',  " & Str(Val(Common_Procedures.OrderBy_CodeToValue(Trim(.Rows(i).Cells(21).Value)))) & ",             0          ,        0        ,    ''   ,  0  ,    0  ,     0        ,      0       ,  " & Str(Val(.Rows(i).Cells(20).Value)) & ",     0        ,      0       , " & Str(Val(.Rows(i).Cells(20).Value)) & ", " & Str(Val(.Rows(i).Cells(20).Value) * Val(.Rows(i).Cells(6).Value)) & " ,  " & Str(Val(.Rows(i).Cells(6).Value)) & " ) "
                                cmd.ExecuteNonQuery()
                            End If

                        End If


                    End If

                Next
                Call Common_Procedures.User_Modification_Updation(con, "DETAILS", Me.Name, "NEW", "Buyer_Offer_Details", "Buyer_Offer_Code", Val(lbl_Company.Tag), NewCode, lbl_RefNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, False, "Lot_No,Pcs_No,Pcs_ClothType_IdNo,Meters,Weight,Weight_Meter,Party_PieceNo,Pass_Meters,Less_Meters,Reject_Meters,Points,Point_Per_PassMeter,Grade,Reject_New_PieceNo,Lot_Code,Pcs_PartyIdNo,Pcs_Cloth_IdNo,Bits_Meters,Bits_New_PieceNo,Loom_IdNo,Loom_No", "Sl_No", "Buyer_Offer_Code, For_OrderBy, Company_IdNo, Buyer_Offer_No, Buyer_Offer_Date, Ledger_Idno", tr)

            End With



            Da = New SqlClient.SqlDataAdapter("Select Int1 as Cloth_IdNo, sum(Meters1) as Less_Type1Mtrs, sum(Meters2) as Less_Type2Mtrs, sum(Meters3) as Less_Type3Mtrs, sum(Meters4) as Less_Type4Mtrs, sum(Meters5) as Less_Type5Mtrs, sum(Currency3) as Add_Type3Mtrs , sum(Currency4) as Add_Type4Mtrs from " & Trim(Common_Procedures.EntryTempSubTable) & " Group by Int1 Having sum(Meters1) <> 0 or sum(Meters2) <> 0 or sum(Meters3) <> 0 or sum(Meters4) <> 0 or sum(Meters5) <> 0 or sum(Currency4) <> 0 order by Int1", con)
            Da.SelectCommand.Transaction = tr
            Dt2 = New DataTable
            Da.Fill(Dt2)
            If Dt2.Rows.Count > 0 Then
                For i = 0 To Dt2.Rows.Count - 1

                    If Val(Dt2.Rows(i).Item("Less_Type1Mtrs").ToString) <> 0 Or Val(Dt2.Rows(i).Item("Less_Type2Mtrs").ToString) <> 0 Or Val(Dt2.Rows(i).Item("Less_Type3Mtrs").ToString) <> 0 Or Val(Dt2.Rows(i).Item("Less_Type4Mtrs").ToString) <> 0 Or Val(Dt2.Rows(i).Item("Less_Type5Mtrs").ToString) <> 0 Then

                        StkDelvTo_ID = 0
                        StkRecFrm_ID = Val(Common_Procedures.CommonLedger.Godown_Ac)

                        cmd.CommandText = "Insert into Stock_Cloth_Processing_Details ( Reference_Code     ,             Company_IdNo         ,             Reference_No      ,                               for_OrderBy                              , Reference_Date,              StockOff_IdNo       ,        DeliveryTo_Idno        ,         ReceivedFrom_Idno     ,         Entry_ID     ,      Party_Bill_No   ,      Particulars       ,        Sl_No            ,                             Cloth_Idno                        ,                 Folding            ,  UnChecked_Meters ,                             Meters_Type1                           ,                             Meters_Type2                           ,                       Meters_Type3                                   ,                             Meters_Type4                           ,                             Meters_Type5                            ) " & _
                                              "        Values ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_RefNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text))) & ",   @EntryDate  , " & Str(Val(vStkOf_Pos_IdNo)) & ", " & Str(Val(StkDelvTo_ID)) & ", " & Str(Val(StkRecFrm_ID)) & ", '" & Trim(EntID) & "', '" & Trim(PBlNo) & "', '" & Trim(Partcls) & "', " & Str(Val(i + 1)) & " , " & Str(Val(Val(Dt2.Rows(i).Item("Cloth_IdNo").ToString))) & ", " & Str(Val(txt_Folding.Text)) & " ,        0          , " & Str(Val(Val(Dt2.Rows(i).Item("Less_Type1Mtrs").ToString))) & " , " & Str(Val(Val(Dt2.Rows(i).Item("Less_Type2Mtrs").ToString))) & " ,   " & Str(Val(Val(Dt2.Rows(i).Item("Less_Type3Mtrs").ToString))) & " , " & Str(Val(Val(Dt2.Rows(i).Item("Less_Type4Mtrs").ToString))) & " , " & Str(Val(Val(Dt2.Rows(i).Item("Less_Type5Mtrs").ToString))) & "  ) "
                        cmd.ExecuteNonQuery()

                    End If

                    If Val(Dt2.Rows(i).Item("Add_Type3Mtrs").ToString) <> 0 Or Val(Dt2.Rows(i).Item("Add_Type4Mtrs").ToString) <> 0 Then

                        StkDelvTo_ID = Val(Common_Procedures.CommonLedger.Godown_Ac)
                        StkRecFrm_ID = 0

                        cmd.CommandText = "Insert into Stock_Cloth_Processing_Details ( Reference_Code     ,             Company_IdNo         ,             Reference_No      ,                               for_OrderBy                              , Reference_Date,              StockOff_IdNo       ,        DeliveryTo_Idno        ,         ReceivedFrom_Idno     ,         Entry_ID     ,      Party_Bill_No   ,      Particulars       ,           Sl_No            ,                             Cloth_Idno                        ,                 Folding            ,  UnChecked_Meters , Meters_Type1 , Meters_Type2 ,                             Meters_Type3                          ,                             Meters_Type4                          , Meters_Type5 ) " & _
                                              "        Values ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_RefNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text))) & ",   @EntryDate  , " & Str(Val(vStkOf_Pos_IdNo)) & ", " & Str(Val(StkDelvTo_ID)) & ", " & Str(Val(StkRecFrm_ID)) & ", '" & Trim(EntID) & "', '" & Trim(PBlNo) & "', '" & Trim(Partcls) & "', " & Str(Val(i + 5001)) & " , " & Str(Val(Val(Dt2.Rows(i).Item("Cloth_IdNo").ToString))) & ", " & Str(Val(txt_Folding.Text)) & " ,        0          ,      0       ,      0       , " & Str(Val(Val(Dt2.Rows(i).Item("Add_Type3Mtrs").ToString))) & " , " & Str(Val(Val(Dt2.Rows(i).Item("Add_Type4Mtrs").ToString))) & " ,       0      ) "
                        cmd.ExecuteNonQuery()

                    End If

                Next
            End If
            Dt2.Clear()


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
        Dim TotPassMtrs As Single, TotRejMtrs As Single, TotLessMtrs As Single, TotPts As Single
        Dim TotBitsMtrs As Single = 0

        Try
            If FrmLdSTS = True Then Exit Sub

            Sno = 0
            TotPcs = 0
            TotMtrs = 0
            TotWgt = 0
            TotPassMtrs = 0
            TotRejMtrs = 0
            TotLessMtrs = 0
            TotPts = 0
            TotBitsMtrs = 0

            With dgv_Details
                For i = 0 To .RowCount - 1
                    Sno = Sno + 1
                    .Rows(i).Cells(0).Value = Sno
                    If Val(.Rows(i).Cells(4).Value) <> 0 Then
                        TotPcs = TotPcs + 1
                        TotMtrs = TotMtrs + Val(.Rows(i).Cells(4).Value)
                        TotWgt = TotWgt + Val(.Rows(i).Cells(5).Value)

                        TotPassMtrs = TotPassMtrs + Val(.Rows(i).Cells(8).Value)
                        TotRejMtrs = TotRejMtrs + Val(.Rows(i).Cells(9).Value)
                        TotLessMtrs = TotLessMtrs + Val(.Rows(i).Cells(10).Value)
                        TotPts = TotPts + Val(.Rows(i).Cells(11).Value)
                        TotBitsMtrs = TotBitsMtrs + Val(.Rows(i).Cells(20).Value)

                    End If
                Next
            End With

            With dgv_Details_Total
                If .RowCount = 0 Then .Rows.Add()
                .Rows(0).Cells(2).Value = Val(TotPcs)
                .Rows(0).Cells(4).Value = Format(Val(TotMtrs), "########0.00")
                .Rows(0).Cells(5).Value = Format(Val(TotWgt), "########0.000")

                .Rows(0).Cells(8).Value = Format(Val(TotPassMtrs), "########0.00")
                .Rows(0).Cells(9).Value = Format(Val(TotRejMtrs), "########0.00")
                .Rows(0).Cells(10).Value = Format(Val(TotLessMtrs), "########0.00")
                .Rows(0).Cells(11).Value = Val(TotPts)
                .Rows(0).Cells(20).Value = Format(Val(TotBitsMtrs), "########0.00")

            End With


        Catch ex As Exception
            '---
        End Try

    End Sub

    Private Sub cbo_Cloth_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Cloth.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Cloth_Head", "Cloth_Name", "", "Cloth_Name")
    End Sub

    Private Sub cbo_Cloth_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Cloth.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Cloth, txt_BuyerRefNo, cbo_ClothType, "Cloth_Head", "Cloth_Name", "", "Cloth_Name")
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

    Private Sub cbo_Bale_Bundle_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Bale_Bundle.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "", "", "", "")
    End Sub

    Private Sub cbo_Bale_Bundle_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Bale_Bundle.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Bale_Bundle, cbo_ClothType, txt_Folding, "", "", "", "")
    End Sub

    Private Sub cbo_Bale_Bundle_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Bale_Bundle.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Bale_Bundle, txt_Folding, "", "", "", "")
    End Sub

    Private Sub txt_Folding_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_Folding.KeyDown
        If e.KeyValue = 38 Then SendKeys.Send("+{TAB}")
        If e.KeyValue = 40 Then
            If dgv_Details.Rows.Count > 0 Then
                dgv_Details.Focus()
                dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(7)
                dgv_Details.CurrentCell.Selected = True
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

                If dgv_Details.Rows.Count > 0 Then
                    dgv_Details.Focus()
                    dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(7)
                    dgv_Details.CurrentCell.Selected = True
                Else
                    txt_Note.Focus()
                End If

            End If

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
            If dgv_Details.Rows.Count > 0 Then
                dgv_Details.Focus()
                dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(7)
                dgv_Details.CurrentCell.Selected = True
            Else
                txt_Folding.Focus()
            End If
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

    Private Sub dgv_Details_CellEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellEnter
        Try
            With dgv_Details
                If .Visible Then
                    If .CurrentCell.ColumnIndex = 7 Or .CurrentCell.ColumnIndex = 9 Or .CurrentCell.ColumnIndex = 10 Or .CurrentCell.ColumnIndex = 11 Or .CurrentCell.ColumnIndex = 13 Or .CurrentCell.ColumnIndex = 14 Then

                        .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex).ReadOnly = False
                        If Trim(.Rows(.CurrentCell.RowIndex).Cells(18).Value) <> "" Or Trim(.Rows(.CurrentCell.RowIndex).Cells(19).Value) <> "" Then
                            .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex).ReadOnly = True
                        End If


                    End If
                End If
            End With

        Catch ex As Exception
            '------

        End Try
    End Sub

    Private Sub dgv_Details_CellLeave(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellLeave
        With dgv_Details
            If .CurrentCell.ColumnIndex = 4 And .CurrentCell.ColumnIndex = 8 And .CurrentCell.ColumnIndex = 9 And .CurrentCell.ColumnIndex = 10 Then
                If Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value) <> 0 Then
                    .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = Format(Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value), "#########0.00")
                Else
                    .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = ""
                End If
            End If

            If .CurrentCell.ColumnIndex = 5 And .CurrentCell.ColumnIndex = 6 Then
                If Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value) <> 0 Then
                    .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = Format(Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value), "#########0.000")
                Else
                    .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = ""
                End If
            End If
        End With

    End Sub

    Private Sub dgv_Details_CellValueChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellValueChanged
        Dim Pts_PasMtr As Single = 0

        Try
            If IsNothing(dgv_Details.CurrentCell) Then Exit Sub
            With dgv_Details
                If .Visible Then
                    If .Rows.Count > 0 Then
                        If e.ColumnIndex = 4 Or e.ColumnIndex = 9 Or e.ColumnIndex = 10 Or e.ColumnIndex = 20 Then
                            .Rows(e.RowIndex).Cells(8).Value = Format(Val(.Rows(e.RowIndex).Cells(4).Value) - Val(.Rows(e.RowIndex).Cells(9).Value) - Val(.Rows(e.RowIndex).Cells(10).Value) - Val(.Rows(e.RowIndex).Cells(20).Value), "#########0.00")
                        End If
                        If e.ColumnIndex = 8 Or e.ColumnIndex = 11 Then
                            Pts_PasMtr = 0
                            If Val(.Rows(e.RowIndex).Cells(8).Value) > 0 Then
                                Pts_PasMtr = Format(Val(.Rows(e.RowIndex).Cells(11).Value) / Val(.Rows(e.RowIndex).Cells(8).Value) * 100, "#########0.00")
                            End If
                            .Rows(e.RowIndex).Cells(12).Value = Format(Val(Pts_PasMtr), "#########0.00")

                            If Val(.Rows(e.RowIndex).Cells(12).Value) < 10 Then
                                .Rows(e.RowIndex).Cells(13).Value = "A"
                            ElseIf Val(.Rows(e.RowIndex).Cells(12).Value) < 20 Then
                                .Rows(e.RowIndex).Cells(13).Value = "B"
                            Else
                                .Rows(e.RowIndex).Cells(13).Value = "C"
                            End If

                        End If
                        If e.ColumnIndex = 2 Or e.ColumnIndex = 4 Or e.ColumnIndex = 5 Or e.ColumnIndex = 8 Or e.ColumnIndex = 9 Or e.ColumnIndex = 10 Or e.ColumnIndex = 11 Or e.ColumnIndex = 20 Then
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

    Private Sub dgv_Details_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_Details.KeyUp
        Dim i As Integer = 0
        Dim n As Integer = 0
        Dim S As String = ""

        Try

            If e.Control = True And UCase(Chr(e.KeyCode)) = "D" Then

                With dgv_Details

                    n = .CurrentRow.Index

                    If Trim(.Rows(n).Cells(18).Value) = "" And Trim(.Rows(n).Cells(19).Value) = "" And Trim(.Rows(n).Cells(22).Value) = "" Then

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

                        Total_Calculation()

                    Else

                        S = ""
                        If Trim(.Rows(n).Cells(18).Value) <> "" Then
                            S = "Already Roll Packed = " & Trim(.Rows(n).Cells(18).Value)
                        End If
                        If Trim(.Rows(n).Cells(19).Value) <> "" Then
                            S = Trim(S) & IIf(Trim(S) <> "", " and " & Chr(13), "") & " Already Rejected Pcs Baled/Delivered = " & Trim(.Rows(n).Cells(19).Value)
                        End If
                        If Trim(.Rows(n).Cells(22).Value) <> "" Then
                            S = Trim(S) & IIf(Trim(S) <> "", " and " & Chr(13), "") & " Already Bits Pcs Baled/Delivered = " & Trim(.Rows(n).Cells(22).Value)
                        End If
                        MessageBox.Show(S, "DOES NOT REMOVE THIS ROW...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                        Exit Sub

                    End If

                End With

            End If

        Catch ex As Exception
            '------------

        End Try

    End Sub

    Private Sub dgv_Details_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgv_Details.LostFocus
        On Error Resume Next
        If Not IsNothing(dgv_Details.CurrentCell) Then dgv_Details.CurrentCell.Selected = False
    End Sub

    Private Sub dgv_Details_RowsAdded(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewRowsAddedEventArgs) Handles dgv_Details.RowsAdded
        Dim n As Integer = 0

        Try
            If IsNothing(dgv_Details.CurrentCell) Then Exit Sub
            With dgv_Details
                n = .RowCount
                .Rows(n - 1).Cells(0).Value = Val(n)
            End With

        Catch ex As Exception
            '-------

        End Try

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

    Private Sub dgtxt_Details_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgtxt_Details.KeyDown
        Try
            With dgv_Details
                If .Visible Then
                    If .CurrentCell.ColumnIndex = 4 Or .CurrentCell.ColumnIndex = 5 Or .CurrentCell.ColumnIndex = 6 Or .CurrentCell.ColumnIndex = 8 Or .CurrentCell.ColumnIndex = 9 Or .CurrentCell.ColumnIndex = 10 Or .CurrentCell.ColumnIndex = 11 Or .CurrentCell.ColumnIndex = 13 Or .CurrentCell.ColumnIndex = 14 Or .CurrentCell.ColumnIndex = 20 Or .CurrentCell.ColumnIndex = 21 Then

                        If Trim(.Rows(.CurrentCell.RowIndex).Cells(18).Value) <> "" Or Trim(.Rows(.CurrentCell.RowIndex).Cells(19).Value) <> "" Or Trim(.Rows(.CurrentCell.RowIndex).Cells(22).Value) <> "" Then
                            e.Handled = True
                            e.SuppressKeyPress = True
                        End If

                    End If
                End If
            End With

        Catch ex As Exception
            '------

        End Try
    End Sub

    Private Sub dgtxt_Details_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dgtxt_Details.KeyPress
        Try
            With dgv_Details
                If .Visible Then
                    If .CurrentCell.ColumnIndex = 4 Or .CurrentCell.ColumnIndex = 5 Or .CurrentCell.ColumnIndex = 6 Or .CurrentCell.ColumnIndex = 8 Or .CurrentCell.ColumnIndex = 9 Or .CurrentCell.ColumnIndex = 10 Or .CurrentCell.ColumnIndex = 11 Or .CurrentCell.ColumnIndex = 20 Then

                        'If Trim(.Rows(.CurrentCell.RowIndex).Cells(18).Value) = "" And Trim(.Rows(.CurrentCell.RowIndex).Cells(19).Value) = "" And Trim(.Rows(.CurrentCell.RowIndex).Cells(22).Value) = "" Then

                        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
                            e.Handled = True
                        End If

                        'Else
                        '    e.Handled = True

                        'End If

                    End If
                End If
            End With

        Catch ex As Exception
            '------

        End Try

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
        Dim Clo_IdNo As Integer, Led_IdNo As Integer
        Dim Condt As String = ""

        Try

            Condt = ""

            Clo_IdNo = 0
            Led_IdNo = 0

            If IsDate(dtp_Filter_Fromdate.Value) = True And IsDate(dtp_Filter_ToDate.Value) = True Then
                Condt = "a.Buyer_Offer_Date between '" & Trim(Format(dtp_Filter_Fromdate.Value, "MM/dd/yyyy")) & "' and '" & Trim(Format(dtp_Filter_ToDate.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_Filter_Fromdate.Value) = True Then
                Condt = "a.Buyer_Offer_Date = '" & Trim(Format(dtp_Filter_Fromdate.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_Filter_ToDate.Value) = True Then
                Condt = "a.Buyer_Offer_Date = '" & Trim(Format(dtp_Filter_ToDate.Value, "MM/dd/yyyy")) & "' "
            End If

            If Trim(cbo_Filter_Cloth.Text) <> "" Then
                Clo_IdNo = Common_Procedures.Cloth_NameToIdNo(con, cbo_Filter_Cloth.Text)
            End If

            If Trim(cbo_Filter_PartyName.Text) <> "" Then
                Led_IdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Filter_PartyName.Text)
            End If


            If Val(Clo_IdNo) <> 0 Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & "a.Cloth_IdNo = " & Str(Val(Clo_IdNo))
            End If

            If Val(Led_IdNo) <> 0 Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & "a.Ledger_IdNo = " & Str(Val(Led_IdNo))
            End If
            If Trim(txt_Filter_LotNo.Text) <> "" Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " a.Buyer_Offer_Code IN (select z1.Buyer_Offer_Code from Buyer_Offer_Details z1 where z1.Lot_No = '" & Trim(txt_Filter_LotNo.Text) & "') "
            End If
            If Trim(txt_Filter_PcsNo.Text) <> "" Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " a.Buyer_Offer_Code IN (select z2.Buyer_Offer_Code from Buyer_Offer_Details z2 where z2.Pcs_No = '" & Trim(txt_Filter_PcsNo.Text) & "') "
            End If
            If Trim(txt_Filter_RollNo.Text) <> "" Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " a.Buyer_RefNo = '" & Trim(txt_Filter_RollNo.Text) & "' "
            End If


            da = New SqlClient.SqlDataAdapter("select a.*, b.ledger_name, c.Cloth_name from Buyer_Offer_Head a Inner join Ledger_Head b on a.ledger_idno <> 0 and a.ledger_idno = b.ledger_idno Inner join Cloth_Head c on a.cloth_idno <> 0 and a.cloth_idno = c.cloth_idno where a.company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Buyer_Offer_Code LIKE '" & Trim(Pk_Condition) & "%/" & Trim(Common_Procedures.FnYearCode) & "' " & IIf(Trim(Condt) <> "", " and ", "") & Condt & " Order by a.for_orderby, a.Buyer_offer_No", con)
            dt2 = New DataTable
            da.Fill(dt2)

            dgv_Filter_Details.Rows.Clear()

            If dt2.Rows.Count > 0 Then

                For i = 0 To dt2.Rows.Count - 1

                    n = dgv_Filter_Details.Rows.Add()

                    dgv_Filter_Details.Rows(n).Cells(0).Value = i + 1
                    dgv_Filter_Details.Rows(n).Cells(1).Value = dt2.Rows(i).Item("Buyer_offer_No").ToString
                    dgv_Filter_Details.Rows(n).Cells(2).Value = Format(Convert.ToDateTime(dt2.Rows(i).Item("Buyer_Offer_Date").ToString), "dd-MM-yyyy")
                    dgv_Filter_Details.Rows(n).Cells(3).Value = dt2.Rows(i).Item("Ledger_Name").ToString
                    dgv_Filter_Details.Rows(n).Cells(4).Value = dt2.Rows(i).Item("Buyer_RefNo").ToString
                    dgv_Filter_Details.Rows(n).Cells(5).Value = dt2.Rows(i).Item("Cloth_Name").ToString
                    dgv_Filter_Details.Rows(n).Cells(6).Value = dt2.Rows(i).Item("Pcs").ToString
                    dgv_Filter_Details.Rows(n).Cells(7).Value = Format(Val(dt2.Rows(i).Item("Total_Passed_Meters").ToString), "########0.00")
                    dgv_Filter_Details.Rows(n).Cells(8).Value = Format(Val(dt2.Rows(i).Item("Total_Weight").ToString), "########0.000")

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
            cbo_Filter_PartyName.Focus()
        End If
    End Sub

    Private Sub cbo_Filter_Cloth_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Filter_Cloth.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Cloth_Head", "Cloth_Name", "", "Cloth_Name")

    End Sub

    Private Sub cbo_Filter_Cloth_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Filter_Cloth.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_Cloth, cbo_Filter_PartyName, txt_Filter_RollNo, "Cloth_Head", "Cloth_Name", "", "Cloth_Name")

    End Sub

    Private Sub cbo_Filter_Cloth_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Filter_Cloth.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Filter_Cloth, txt_Filter_RollNo, "Cloth_Head", "Cloth_Name", "", "Cloth_Name")

    End Sub

    Private Sub cbo_Filter_PartyName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Filter_PartyName.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "ClothType_Head", "ClothType_Name", "", "ClothType_Name")

    End Sub

    Private Sub cbo_Filter_PartyName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Filter_PartyName.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_PartyName, dtp_Filter_ToDate, cbo_Filter_Cloth, "ClothType_Head", "ClothType_Name", "", "ClothType_Name")

    End Sub

    Private Sub cbo_Filter_PartyName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Filter_PartyName.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Filter_PartyName, cbo_Filter_Cloth, "ClothType_Head", "ClothType_Name", "", "ClothType_Name")

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

    Private Sub chk_SelectAll_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chk_SelectAll.CheckedChanged
        Dim i As Integer
        Dim J As Integer

        With dgv_Selection

            For i = 0 To .Rows.Count - 1
                .Rows(i).Cells(8).Value = ""
                For J = 0 To .ColumnCount - 1
                    .Rows(i).Cells(J).Style.ForeColor = Color.Black
                Next J
            Next i

            If chk_SelectAll.Checked = True Then
                For i = 0 To .Rows.Count - 1
                    Select_Piece(i)
                Next i
            End If

            If .Rows.Count > 0 Then
                .Focus()
                .CurrentCell = .Rows(0).Cells(0)
                .CurrentCell.Selected = True
            End If

        End With

    End Sub

    Private Sub btn_Selection_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Selection.Click
        Dim Cmd As New SqlClient.SqlCommand
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim Dt2 As New DataTable
        Dim i As Integer, j As Integer, n As Integer, SNo As Integer
        Dim CloIdNo As Integer, CloTypIdNo As Integer
        Dim NewCode As String
        Dim CompIDCondt As String
        Dim StkOff_IdNo As Integer = 0
        Dim Cnt_GrpIdNos As String
        Dim Cnt_IdNo As Integer, Cnt_UndIdNo As Integer
        Dim Cnt_Cond As String
        Dim vRejPcsPackSlpCd As String = "", vBitsPcsPackSlpCd As String = ""
        Dim vLmIdNo As Long = 0
        Dim vLmNo As String = ""

        StkOff_IdNo = Common_Procedures.CommonLedger.Godown_Ac   '----Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_PartyName.Text)

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
        If Common_Procedures.settings.EntrySelection_Combine_AllCompany = 1 Then
            CompIDCondt = ""
            If Trim(UCase(Common_Procedures.User.Type)) = "ACCOUNT" Then
                CompIDCondt = "(tZ.Company_Type <> 'UNACCOUNT')"
            End If
        End If

        NewCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)


        Cnt_IdNo = CloIdNo

        Cnt_UndIdNo = Val(Cnt_IdNo)

        Da = New SqlClient.SqlDataAdapter("select * from Cloth_head where Cloth_idno = " & Str(Val(Cnt_UndIdNo)), con)
        Dt1 = New DataTable
        Da.Fill(Dt1)
        If Dt1.Rows.Count > 0 Then
            If IsDBNull(Dt1.Rows(0).Item("Cloth_StockUnder_IdNo").ToString) = False Then
                If Val(Dt1.Rows(0).Item("Cloth_StockUnder_IdNo").ToString) <> 0 Then Cnt_UndIdNo = Val(Dt1.Rows(0).Item("Cloth_StockUnder_IdNo").ToString)
            End If
        End If
        Dt1.Clear()

        Da = New SqlClient.SqlDataAdapter("select * from Cloth_head where Cloth_StockUnder_IdNo = " & Str(Val(Cnt_UndIdNo)), con)
        Dt1 = New DataTable
        Da.Fill(Dt1)

        Cnt_GrpIdNos = ""
        If Dt1.Rows.Count > 0 Then
            For i = 0 To Dt1.Rows.Count - 1
                Cnt_GrpIdNos = Trim(Cnt_GrpIdNos) & IIf(Trim(Cnt_GrpIdNos) <> "", ", ", "") & Trim(Val(Dt1.Rows(i).Item("Cloth_IdNo")))
            Next
        End If
        If Trim(Cnt_GrpIdNos) <> "" Then
            Cnt_GrpIdNos = "(" & Cnt_GrpIdNos & ")"
        Else
            Cnt_GrpIdNos = "(" & Trim(Val(Cnt_IdNo)) & ")"
        End If

        Cnt_Cond = "(a.Cloth_idno = " & Str(Cnt_IdNo) & " or a.Cloth_idno IN " & Trim(Cnt_GrpIdNos) & ")"

        With dgv_Selection

            chk_SelectAll.Checked = False

            .Rows.Clear()

            SNo = 0

            If CloTypIdNo = 1 Then

                Da = New SqlClient.SqlDataAdapter("select a.* , c.Ledger_Name, d.cloth_name from Weaver_ClothReceipt_Piece_Details a  INNER JOIN Company_Head tZ ON a.company_idno = tZ.company_idno LEFT OUTER JOIN Ledger_Head c ON a.Ledger_IdNo <> 0 and a.Ledger_IdNo = c.Ledger_IdNo LEFT OUTER JOIN Cloth_Head d ON a.Cloth_IdNo <> 0 and a.Cloth_IdNo = d.Cloth_IdNo Where " & CompIDCondt & IIf(Trim(CompIDCondt) <> "", " and ", "") & " a.Type1_Meters <> 0 and a.BuyerOffer_Code_Type1 = '" & Trim(NewCode) & "' and " & Cnt_Cond & IIf(Cnt_Cond <> "", " and ", " ") & "  a.Folding = " & Str(Val(txt_Folding.Text)) & " and (a.StockOff_IdNo = 4 or a.StockOff_IdNo = 5) order by a.Weaver_ClothReceipt_Date, a.for_orderby, a.Weaver_ClothReceipt_No, a.PieceNo_OrderBy, a.Piece_No", con)
                Dt1 = New DataTable
                Da.Fill(Dt1)

                If Dt1.Rows.Count > 0 Then

                    For i = 0 To Dt1.Rows.Count - 1

                        n = .Rows.Add()

                        SNo = SNo + 1

                        .Rows(n).Cells(0).Value = Val(SNo)

                        .Rows(n).Cells(1).Value = Dt1.Rows(i).Item("Lot_No").ToString
                        .Rows(n).Cells(2).Value = Dt1.Rows(i).Item("Piece_No").ToString
                        .Rows(n).Cells(3).Value = Common_Procedures.ClothType.Type1
                        .Rows(n).Cells(4).Value = Format(Val(Dt1.Rows(i).Item("Type1_Meters").ToString), "#########0.00")
                        .Rows(n).Cells(5).Value = Format(Val(Dt1.Rows(i).Item("Type1_Meters").ToString) * Val(Dt1.Rows(i).Item("Weight_Meter").ToString), "#########0.000")
                        If Val(.Rows(n).Cells(5).Value) = 0 Then .Rows(n).Cells(5).Value = ""
                        .Rows(n).Cells(6).Value = Format(Val(Dt1.Rows(i).Item("Weight_Meter").ToString), "#########0.000")
                        If Val(.Rows(n).Cells(6).Value) = 0 Then .Rows(n).Cells(6).Value = ""
                        .Rows(n).Cells(7).Value = Dt1.Rows(i).Item("Ledger_Name").ToString
                        .Rows(n).Cells(8).Value = "1"
                        .Rows(n).Cells(9).Value = Dt1.Rows(i).Item("Lot_Code").ToString
                        .Rows(n).Cells(10).Value = Dt1.Rows(i).Item("Cloth_Name").ToString

                        .Rows(n).Cells(11).Value = Dt1.Rows(i).Item("BuyerOffer_Party_PieceNo_Type1").ToString
                        .Rows(n).Cells(12).Value = Val(Dt1.Rows(i).Item("BuyerOffer_Passed_Meters_Type1").ToString)
                        .Rows(n).Cells(13).Value = Val(Dt1.Rows(i).Item("BuyerOffer_Less_Meters_Type1").ToString)
                        .Rows(n).Cells(14).Value = Val(Dt1.Rows(i).Item("BuyerOffer_Reject_Meters_Type1").ToString)
                        .Rows(n).Cells(15).Value = Val(Dt1.Rows(i).Item("BuyerOffer_Points_Type1").ToString)
                        .Rows(n).Cells(16).Value = Val(Dt1.Rows(i).Item("BuyerOffer_Points_Per_PassMeter_Type1").ToString)
                        .Rows(n).Cells(17).Value = Dt1.Rows(i).Item("BuyerOffer_Grade_Type1").ToString
                        .Rows(n).Cells(18).Value = Dt1.Rows(i).Item("BuyerOffer_Rejection_PieceNo_Type1").ToString

                        .Rows(n).Cells(19).Value = Dt1.Rows(i).Item("PackingSlip_Code_Type1").ToString

                        vRejPcsPackSlpCd = ""
                        If Trim(Dt1.Rows(i).Item("BuyerOffer_Rejection_PieceNo_Type1").ToString) <> "" Then
                            Da = New SqlClient.SqlDataAdapter("Select a.* from Weaver_ClothReceipt_Piece_Details a  INNER JOIN Company_Head tZ ON a.company_idno = tZ.company_idno Where a.Lot_Code = '" & Trim(Dt1.Rows(i).Item("lot_code").ToString) & "' and  a.Piece_No = '" & Trim(Dt1.Rows(i).Item("BuyerOffer_Rejection_PieceNo_Type1").ToString) & "'", con)
                            Dt2 = New DataTable
                            Da.Fill(Dt2)
                            If Dt2.Rows.Count > 0 Then
                                If Val(Dt2.Rows(0).Item("Type5_Meters").ToString) <> 0 Then
                                    vRejPcsPackSlpCd = Dt2.Rows(0).Item("PackingSlip_Code_Type5").ToString
                                ElseIf Val(Dt2.Rows(0).Item("Type4_Meters").ToString) <> 0 Then
                                    vRejPcsPackSlpCd = Dt2.Rows(0).Item("PackingSlip_Code_Type4").ToString
                                ElseIf Val(Dt2.Rows(0).Item("Type3_Meters").ToString) <> 0 Then
                                    vRejPcsPackSlpCd = Dt2.Rows(0).Item("PackingSlip_Code_Type3").ToString
                                ElseIf Val(Dt2.Rows(0).Item("Type2_Meters").ToString) <> 0 Then
                                    vRejPcsPackSlpCd = Dt2.Rows(0).Item("PackingSlip_Code_Type2").ToString
                                Else
                                    vRejPcsPackSlpCd = Dt2.Rows(0).Item("PackingSlip_Code_Type1").ToString
                                End If
                            End If
                            Dt2.Clear()
                        End If
                        .Rows(n).Cells(20).Value = vRejPcsPackSlpCd


                        .Rows(n).Cells(21).Value = Val(Dt1.Rows(i).Item("BuyerOffer_Bits_Meters_Type1").ToString)
                        .Rows(n).Cells(22).Value = Dt1.Rows(i).Item("BuyerOffer_Bits_PieceNo_Type1").ToString
                        vBitsPcsPackSlpCd = ""
                        If Trim(Dt1.Rows(i).Item("BuyerOffer_Bits_PieceNo_Type1").ToString) <> "" Then
                            Da = New SqlClient.SqlDataAdapter("Select a.* from Weaver_ClothReceipt_Piece_Details a  INNER JOIN Company_Head tZ ON a.company_idno = tZ.company_idno Where a.Lot_Code = '" & Trim(Dt1.Rows(i).Item("lot_code").ToString) & "' and  a.Piece_No = '" & Trim(Dt1.Rows(i).Item("BuyerOffer_Bits_PieceNo_Type1").ToString) & "'", con)
                            Dt2 = New DataTable
                            Da.Fill(Dt2)
                            If Dt2.Rows.Count > 0 Then
                                If Val(Dt2.Rows(0).Item("Type5_Meters").ToString) <> 0 Then
                                    vBitsPcsPackSlpCd = Dt2.Rows(0).Item("PackingSlip_Code_Type5").ToString
                                ElseIf Val(Dt2.Rows(0).Item("Type4_Meters").ToString) <> 0 Then
                                    vBitsPcsPackSlpCd = Dt2.Rows(0).Item("PackingSlip_Code_Type4").ToString
                                ElseIf Val(Dt2.Rows(0).Item("Type3_Meters").ToString) <> 0 Then
                                    vBitsPcsPackSlpCd = Dt2.Rows(0).Item("PackingSlip_Code_Type3").ToString
                                ElseIf Val(Dt2.Rows(0).Item("Type2_Meters").ToString) <> 0 Then
                                    vBitsPcsPackSlpCd = Dt2.Rows(0).Item("PackingSlip_Code_Type2").ToString
                                Else
                                    vBitsPcsPackSlpCd = Dt2.Rows(0).Item("PackingSlip_Code_Type1").ToString
                                End If
                            End If
                            Dt2.Clear()
                        End If
                        .Rows(n).Cells(23).Value = vBitsPcsPackSlpCd


                        vLmIdNo = 0
                        If IsDBNull(Dt1.Rows(i).Item("Loom_IdNo").ToString) = False Then
                            vLmIdNo = Val(Dt1.Rows(i).Item("Loom_IdNo").ToString)
                        End If

                        vLmNo = ""
                        If vLmIdNo <> 0 Then
                            vLmNo = Common_Procedures.Loom_IdNoToName(con, vLmIdNo)

                        Else
                            If IsDBNull(Dt1.Rows(i).Item("Loom_No").ToString) = False Then
                                vLmNo = Dt1.Rows(i).Item("Loom_No").ToString
                            End If

                        End If

                        .Rows(n).Cells(24).Value = vLmNo

                        For j = 0 To .ColumnCount - 1
                            .Rows(i).Cells(j).Style.ForeColor = Color.Red
                        Next
                        If Trim(.Rows(n).Cells(19).Value) <> "" Or Trim(.Rows(n).Cells(20).Value) <> "" Or Trim(.Rows(n).Cells(23).Value) <> "" Then
                            For j = 0 To .ColumnCount - 1
                                .Rows(n).Cells(j).Style.BackColor = Color.LightGray
                            Next
                        End If

                    Next

                End If
                Dt1.Clear()


                Da = New SqlClient.SqlDataAdapter("select a.*, c.Ledger_Name, d.Cloth_Name from Weaver_ClothReceipt_Piece_Details a  INNER JOIN Company_Head tZ ON a.company_idno = tZ.company_idno LEFT OUTER JOIN Ledger_Head c ON a.Ledger_IdNo <> 0 and a.Ledger_IdNo = c.Ledger_IdNo  LEFT OUTER JOIN Cloth_Head d ON a.Cloth_IdNo <> 0 and a.Cloth_IdNo = d.Cloth_IdNo where " & CompIDCondt & IIf(Trim(CompIDCondt) <> "", " and ", "") & " a.Type1_Meters <> 0 and a.PackingSlip_Code_Type1 = '' and a.BuyerOffer_Code_Type1 = '' and " & Cnt_Cond & IIf(Cnt_Cond <> "", " and ", " ") & "  a.Folding = " & Str(Val(txt_Folding.Text)) & " and (a.StockOff_IdNo = 4 or a.StockOff_IdNo = 5) order by a.Weaver_ClothReceipt_Date, a.for_orderby, a.Weaver_ClothReceipt_No, a.PieceNo_OrderBy, a.Piece_No", con)
                Dt1 = New DataTable
                Da.Fill(Dt1)

                If Dt1.Rows.Count > 0 Then

                    For i = 0 To Dt1.Rows.Count - 1

                        n = .Rows.Add()

                        SNo = SNo + 1
                        .Rows(n).Cells(0).Value = Val(SNo)

                        .Rows(n).Cells(1).Value = Dt1.Rows(i).Item("Lot_No").ToString
                        .Rows(n).Cells(2).Value = Dt1.Rows(i).Item("Piece_No").ToString
                        .Rows(n).Cells(3).Value = Common_Procedures.ClothType.Type1
                        .Rows(n).Cells(4).Value = Format(Val(Dt1.Rows(i).Item("Type1_Meters").ToString), "#########0.00")
                        .Rows(n).Cells(5).Value = Format(Val(Dt1.Rows(i).Item("Type1_Meters").ToString) * Val(Dt1.Rows(i).Item("Weight_Meter").ToString), "#########0.000")
                        If Val(.Rows(n).Cells(5).Value) = 0 Then .Rows(n).Cells(5).Value = ""
                        .Rows(n).Cells(6).Value = Format(Val(Dt1.Rows(i).Item("Weight_Meter").ToString), "#########0.000")
                        If Val(.Rows(n).Cells(6).Value) = 0 Then .Rows(n).Cells(6).Value = ""
                        .Rows(n).Cells(7).Value = Dt1.Rows(i).Item("Ledger_Name").ToString
                        .Rows(n).Cells(8).Value = ""
                        .Rows(n).Cells(9).Value = Dt1.Rows(i).Item("Lot_Code").ToString
                        .Rows(n).Cells(10).Value = Dt1.Rows(i).Item("Cloth_Name").ToString

                        .Rows(n).Cells(11).Value = ""
                        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1464" Then '---- MANI OMEGA FABRICS (THIRUCHENKODU)
                            .Rows(n).Cells(11).Value = Dt1.Rows(i).Item("Piece_No").ToString
                        End If
                        .Rows(n).Cells(12).Value = ""
                        .Rows(n).Cells(13).Value = ""
                        .Rows(n).Cells(14).Value = ""
                        .Rows(n).Cells(15).Value = ""
                        .Rows(n).Cells(16).Value = ""
                        .Rows(n).Cells(17).Value = ""
                        .Rows(n).Cells(18).Value = ""

                        .Rows(n).Cells(19).Value = ""
                        .Rows(n).Cells(20).Value = ""

                        .Rows(n).Cells(21).Value = ""
                        .Rows(n).Cells(22).Value = ""
                        .Rows(n).Cells(23).Value = ""

                        vLmIdNo = 0
                        If IsDBNull(Dt1.Rows(i).Item("Loom_IdNo").ToString) = False Then
                            vLmIdNo = Val(Dt1.Rows(i).Item("Loom_IdNo").ToString)
                        End If

                        vLmNo = ""
                        If vLmIdNo <> 0 Then
                            vLmNo = Common_Procedures.Loom_IdNoToName(con, vLmIdNo)

                        Else
                            If IsDBNull(Dt1.Rows(i).Item("Loom_No").ToString) = False Then
                                vLmNo = Dt1.Rows(i).Item("Loom_No").ToString
                            End If

                        End If

                        .Rows(n).Cells(24).Value = vLmNo

                    Next

                End If
                Dt1.Clear()

            End If

            If CloTypIdNo = 1 Or CloTypIdNo = 2 Then

                Da = New SqlClient.SqlDataAdapter("select a.* , C.Ledger_Name, d.cloth_name from Weaver_ClothReceipt_Piece_Details a  INNER JOIN Company_Head tZ ON a.company_idno = tZ.company_idno LEFT OUTER JOIN Ledger_Head c ON a.Ledger_IdNo <> 0 and a.Ledger_IdNo = c.Ledger_IdNo LEFT OUTER JOIN Cloth_Head d ON a.Cloth_IdNo <> 0 and a.Cloth_IdNo = d.Cloth_IdNo where " & CompIDCondt & IIf(Trim(CompIDCondt) <> "", " and ", "") & " a.Type2_Meters <> 0 and a.BuyerOffer_Code_Type2 = '" & Trim(NewCode) & "'  and " & Cnt_Cond & IIf(Cnt_Cond <> "", " and ", " ") & "  a.Folding = " & Str(Val(txt_Folding.Text)) & "   and (a.StockOff_IdNo = 4 or a.StockOff_IdNo = 5) order by a.Weaver_ClothReceipt_Date, a.for_orderby, a.Weaver_ClothReceipt_No, a.PieceNo_OrderBy, a.Piece_No", con)
                Dt1 = New DataTable
                Da.Fill(Dt1)

                If Dt1.Rows.Count > 0 Then

                    For i = 0 To Dt1.Rows.Count - 1

                        n = .Rows.Add()

                        SNo = SNo + 1
                        .Rows(n).Cells(0).Value = Val(SNo)

                        .Rows(n).Cells(1).Value = Dt1.Rows(i).Item("Lot_No").ToString
                        .Rows(n).Cells(2).Value = Dt1.Rows(i).Item("Piece_No").ToString
                        .Rows(n).Cells(3).Value = Common_Procedures.ClothType.Type2
                        .Rows(n).Cells(4).Value = Format(Val(Dt1.Rows(i).Item("Type2_Meters").ToString), "#########0.00")
                        .Rows(n).Cells(5).Value = Format(Val(Dt1.Rows(i).Item("Type2_Meters").ToString) * Val(Dt1.Rows(i).Item("Weight_Meter").ToString), "#########0.000")
                        If Val(.Rows(n).Cells(5).Value) = 0 Then .Rows(n).Cells(5).Value = ""
                        .Rows(n).Cells(6).Value = Format(Val(Dt1.Rows(i).Item("Weight_Meter").ToString), "#########0.000")
                        If Val(.Rows(n).Cells(6).Value) = 0 Then .Rows(n).Cells(6).Value = ""
                        .Rows(n).Cells(7).Value = Dt1.Rows(i).Item("Ledger_Name").ToString
                        .Rows(n).Cells(8).Value = "1"
                        .Rows(n).Cells(9).Value = Dt1.Rows(i).Item("Lot_Code").ToString
                        .Rows(n).Cells(10).Value = Dt1.Rows(i).Item("cloth_name").ToString

                        .Rows(n).Cells(11).Value = Dt1.Rows(i).Item("BuyerOffer_Party_PieceNo_Type2").ToString
                        .Rows(n).Cells(12).Value = Val(Dt1.Rows(i).Item("BuyerOffer_Passed_Meters_Type2").ToString)
                        .Rows(n).Cells(13).Value = Val(Dt1.Rows(i).Item("BuyerOffer_Less_Meters_Type2").ToString)
                        .Rows(n).Cells(14).Value = Val(Dt1.Rows(i).Item("BuyerOffer_Reject_Meters_Type2").ToString)
                        .Rows(n).Cells(15).Value = Val(Dt1.Rows(i).Item("BuyerOffer_Points_Type2").ToString)
                        .Rows(n).Cells(16).Value = Val(Dt1.Rows(i).Item("BuyerOffer_Points_Per_PassMeter_Type2").ToString)
                        .Rows(n).Cells(17).Value = Dt1.Rows(i).Item("BuyerOffer_Grade_Type2").ToString
                        .Rows(n).Cells(18).Value = Dt1.Rows(i).Item("BuyerOffer_Rejection_PieceNo_Type2").ToString

                        .Rows(n).Cells(19).Value = Dt1.Rows(i).Item("PackingSlip_Code_Type2").ToString

                        vRejPcsPackSlpCd = ""
                        If Trim(Dt1.Rows(i).Item("BuyerOffer_Rejection_PieceNo_Type2").ToString) <> "" Then
                            Da = New SqlClient.SqlDataAdapter("Select a.* from Weaver_ClothReceipt_Piece_Details a  INNER JOIN Company_Head tZ ON a.company_idno = tZ.company_idno Where a.Lot_Code = '" & Trim(Dt1.Rows(i).Item("lot_code").ToString) & "' and  a.Piece_No = '" & Trim(Dt1.Rows(i).Item("BuyerOffer_Rejection_PieceNo_Type2").ToString) & "'", con)
                            Dt2 = New DataTable
                            Da.Fill(Dt2)
                            If Dt2.Rows.Count > 0 Then
                                If Val(Dt2.Rows(0).Item("Type5_Meters").ToString) <> 0 Then
                                    vRejPcsPackSlpCd = Dt2.Rows(0).Item("PackingSlip_Code_Type5").ToString
                                ElseIf Val(Dt2.Rows(0).Item("Type4_Meters").ToString) <> 0 Then
                                    vRejPcsPackSlpCd = Dt2.Rows(0).Item("PackingSlip_Code_Type4").ToString
                                ElseIf Val(Dt2.Rows(0).Item("Type3_Meters").ToString) <> 0 Then
                                    vRejPcsPackSlpCd = Dt2.Rows(0).Item("PackingSlip_Code_Type3").ToString
                                ElseIf Val(Dt2.Rows(0).Item("Type2_Meters").ToString) <> 0 Then
                                    vRejPcsPackSlpCd = Dt2.Rows(0).Item("PackingSlip_Code_Type2").ToString
                                Else
                                    vRejPcsPackSlpCd = Dt2.Rows(0).Item("PackingSlip_Code_Type1").ToString
                                End If
                            End If
                            Dt2.Clear()
                        End If
                        .Rows(n).Cells(20).Value = vRejPcsPackSlpCd

                        .Rows(n).Cells(21).Value = Val(Dt1.Rows(i).Item("BuyerOffer_Bits_Meters_Type2").ToString)
                        .Rows(n).Cells(22).Value = Dt1.Rows(i).Item("BuyerOffer_Bits_PieceNo_Type2").ToString
                        vBitsPcsPackSlpCd = ""
                        If Trim(Dt1.Rows(i).Item("BuyerOffer_Bits_PieceNo_Type2").ToString) <> "" Then
                            Da = New SqlClient.SqlDataAdapter("Select a.* from Weaver_ClothReceipt_Piece_Details a  INNER JOIN Company_Head tZ ON a.company_idno = tZ.company_idno Where a.Lot_Code = '" & Trim(Dt1.Rows(i).Item("lot_code").ToString) & "' and  a.Piece_No = '" & Trim(Dt1.Rows(i).Item("BuyerOffer_Bits_PieceNo_Type2").ToString) & "'", con)
                            Dt2 = New DataTable
                            Da.Fill(Dt2)
                            If Dt2.Rows.Count > 0 Then
                                If Val(Dt2.Rows(0).Item("Type5_Meters").ToString) <> 0 Then
                                    vBitsPcsPackSlpCd = Dt2.Rows(0).Item("PackingSlip_Code_Type5").ToString
                                ElseIf Val(Dt2.Rows(0).Item("Type4_Meters").ToString) <> 0 Then
                                    vBitsPcsPackSlpCd = Dt2.Rows(0).Item("PackingSlip_Code_Type4").ToString
                                ElseIf Val(Dt2.Rows(0).Item("Type3_Meters").ToString) <> 0 Then
                                    vBitsPcsPackSlpCd = Dt2.Rows(0).Item("PackingSlip_Code_Type3").ToString
                                ElseIf Val(Dt2.Rows(0).Item("Type2_Meters").ToString) <> 0 Then
                                    vBitsPcsPackSlpCd = Dt2.Rows(0).Item("PackingSlip_Code_Type2").ToString
                                Else
                                    vBitsPcsPackSlpCd = Dt2.Rows(0).Item("PackingSlip_Code_Type1").ToString
                                End If
                            End If
                            Dt2.Clear()
                        End If
                        .Rows(n).Cells(23).Value = vBitsPcsPackSlpCd

                        vLmIdNo = 0
                        If IsDBNull(Dt1.Rows(i).Item("Loom_IdNo").ToString) = False Then
                            vLmIdNo = Val(Dt1.Rows(i).Item("Loom_IdNo").ToString)
                        End If

                        vLmNo = ""
                        If vLmIdNo <> 0 Then
                            vLmNo = Common_Procedures.Loom_IdNoToName(con, vLmIdNo)

                        Else
                            If IsDBNull(Dt1.Rows(i).Item("Loom_No").ToString) = False Then
                                vLmNo = Dt1.Rows(i).Item("Loom_No").ToString
                            End If

                        End If

                        .Rows(n).Cells(24).Value = vLmNo

                        For j = 0 To .ColumnCount - 1
                            .Rows(i).Cells(j).Style.ForeColor = Color.Red
                        Next
                        If Trim(.Rows(n).Cells(19).Value) <> "" Or Trim(.Rows(n).Cells(20).Value) <> "" Then
                            For j = 0 To .ColumnCount - 1
                                .Rows(n).Cells(j).Style.BackColor = Color.LightGray
                            Next
                        End If

                    Next

                End If
                Dt1.Clear()


                Da = New SqlClient.SqlDataAdapter("select a.* , c.Ledger_Name, d.cloth_name from Weaver_ClothReceipt_Piece_Details a  INNER JOIN Company_Head tZ ON a.company_idno = tZ.company_idno LEFT OUTER JOIN Ledger_Head c ON a.Ledger_IdNo = c.Ledger_IdNo LEFT OUTER JOIN Cloth_Head d ON a.Cloth_IdNo <> 0 and a.Cloth_IdNo = d.Cloth_IdNo where " & CompIDCondt & IIf(Trim(CompIDCondt) <> "", " and ", "") & " a.Type2_Meters <> 0 and a.PackingSlip_Code_Type2 = '' and a.BuyerOffer_Code_Type2 = ''  and " & Cnt_Cond & IIf(Cnt_Cond <> "", " and ", " ") & " a.Folding = " & Str(Val(txt_Folding.Text)) & "  and (a.StockOff_IdNo = 4 or a.StockOff_IdNo = 5) order by a.Weaver_ClothReceipt_Date, a.for_orderby, a.Weaver_ClothReceipt_No, a.PieceNo_OrderBy, a.Piece_No", con)
                Dt1 = New DataTable
                Da.Fill(Dt1)

                If Dt1.Rows.Count > 0 Then

                    For i = 0 To Dt1.Rows.Count - 1

                        n = .Rows.Add()

                        SNo = SNo + 1
                        .Rows(n).Cells(0).Value = Val(SNo)

                        .Rows(n).Cells(1).Value = Dt1.Rows(i).Item("Lot_No").ToString
                        .Rows(n).Cells(2).Value = Dt1.Rows(i).Item("Piece_No").ToString
                        .Rows(n).Cells(3).Value = Common_Procedures.ClothType.Type2
                        .Rows(n).Cells(4).Value = Format(Val(Dt1.Rows(i).Item("Type2_Meters").ToString), "#########0.00")
                        .Rows(n).Cells(5).Value = Format(Val(Dt1.Rows(i).Item("Type2_Meters").ToString) * Val(Dt1.Rows(i).Item("Weight_Meter").ToString), "#########0.000")
                        If Val(.Rows(n).Cells(5).Value) = 0 Then .Rows(n).Cells(5).Value = ""
                        .Rows(n).Cells(6).Value = Format(Val(Dt1.Rows(i).Item("Weight_Meter").ToString), "#########0.000")
                        If Val(.Rows(n).Cells(6).Value) = 0 Then .Rows(n).Cells(6).Value = ""
                        .Rows(n).Cells(7).Value = Dt1.Rows(i).Item("Ledger_Name").ToString
                        .Rows(n).Cells(8).Value = ""
                        .Rows(n).Cells(9).Value = Dt1.Rows(i).Item("Lot_Code").ToString
                        .Rows(n).Cells(10).Value = Dt1.Rows(i).Item("cloth_name").ToString

                        .Rows(n).Cells(11).Value = ""
                        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1464" Then '---- MANI OMEGA FABRICS (THIRUCHENKODU)
                            .Rows(n).Cells(11).Value = Dt1.Rows(i).Item("Piece_No").ToString
                        End If

                        .Rows(n).Cells(12).Value = ""
                        .Rows(n).Cells(13).Value = ""
                        .Rows(n).Cells(14).Value = ""
                        .Rows(n).Cells(15).Value = ""
                        .Rows(n).Cells(16).Value = ""
                        .Rows(n).Cells(17).Value = ""
                        .Rows(n).Cells(18).Value = ""
                        .Rows(n).Cells(19).Value = ""
                        .Rows(n).Cells(20).Value = ""
                        .Rows(n).Cells(21).Value = ""
                        .Rows(n).Cells(22).Value = ""
                        .Rows(n).Cells(23).Value = ""

                        vLmIdNo = 0
                        If IsDBNull(Dt1.Rows(i).Item("Loom_IdNo").ToString) = False Then
                            vLmIdNo = Val(Dt1.Rows(i).Item("Loom_IdNo").ToString)
                        End If

                        vLmNo = ""
                        If vLmIdNo <> 0 Then
                            vLmNo = Common_Procedures.Loom_IdNoToName(con, vLmIdNo)

                        Else
                            If IsDBNull(Dt1.Rows(i).Item("Loom_No").ToString) = False Then
                                vLmNo = Dt1.Rows(i).Item("Loom_No").ToString
                            End If

                        End If

                        .Rows(n).Cells(24).Value = vLmNo

                    Next

                End If
                Dt1.Clear()
            End If

            If CloTypIdNo = 3 Then

                Da = New SqlClient.SqlDataAdapter("select a.* , c.Ledger_Name, d.cloth_name from Weaver_ClothReceipt_Piece_Details a  INNER JOIN Company_Head tZ ON a.company_idno = tZ.company_idno  LEFT OUTER JOIN Ledger_Head c ON a.Ledger_IdNo = c.Ledger_IdNo LEFT OUTER JOIN Cloth_Head d ON a.Cloth_IdNo <> 0 and a.Cloth_IdNo = d.Cloth_IdNo where " & CompIDCondt & IIf(Trim(CompIDCondt) <> "", " and ", "") & " a.Type3_Meters <> 0 and a.BuyerOffer_Code_Type3 = '" & Trim(NewCode) & "'  and " & Cnt_Cond & IIf(Cnt_Cond <> "", " and ", " ") & "  a.Folding = " & Str(Val(txt_Folding.Text)) & "  and (a.StockOff_IdNo = 4 or a.StockOff_IdNo = 5) order by a.Weaver_ClothReceipt_Date, a.for_orderby, a.Weaver_ClothReceipt_No, a.PieceNo_OrderBy, a.Piece_No", con)
                Dt1 = New DataTable
                Da.Fill(Dt1)

                If Dt1.Rows.Count > 0 Then

                    For i = 0 To Dt1.Rows.Count - 1

                        n = .Rows.Add()

                        SNo = SNo + 1
                        .Rows(n).Cells(0).Value = Val(SNo)

                        .Rows(n).Cells(1).Value = Dt1.Rows(i).Item("Lot_No").ToString
                        .Rows(n).Cells(2).Value = Dt1.Rows(i).Item("Piece_No").ToString
                        .Rows(n).Cells(3).Value = Common_Procedures.ClothType.Type3
                        .Rows(n).Cells(4).Value = Format(Val(Dt1.Rows(i).Item("Type3_Meters").ToString), "#########0.00")
                        .Rows(n).Cells(5).Value = Format(Val(Dt1.Rows(i).Item("Type3_Meters").ToString) * Val(Dt1.Rows(i).Item("Weight_Meter").ToString), "#########0.000")
                        If Val(.Rows(n).Cells(5).Value) = 0 Then .Rows(n).Cells(5).Value = ""
                        .Rows(n).Cells(6).Value = Format(Val(Dt1.Rows(i).Item("Weight_Meter").ToString), "#########0.000")
                        If Val(.Rows(n).Cells(6).Value) = 0 Then .Rows(n).Cells(6).Value = ""
                        .Rows(n).Cells(7).Value = Dt1.Rows(i).Item("Ledger_Name").ToString
                        .Rows(n).Cells(8).Value = "1"
                        .Rows(n).Cells(9).Value = Dt1.Rows(i).Item("Lot_Code").ToString
                        .Rows(n).Cells(10).Value = Dt1.Rows(i).Item("cloth_name").ToString

                        .Rows(n).Cells(11).Value = Dt1.Rows(i).Item("BuyerOffer_Party_PieceNo_Type3").ToString
                        .Rows(n).Cells(12).Value = Val(Dt1.Rows(i).Item("BuyerOffer_Passed_Meters_Type3").ToString)
                        .Rows(n).Cells(13).Value = Val(Dt1.Rows(i).Item("BuyerOffer_Less_Meters_Type3").ToString)
                        .Rows(n).Cells(14).Value = Val(Dt1.Rows(i).Item("BuyerOffer_Reject_Meters_Type3").ToString)
                        .Rows(n).Cells(15).Value = Val(Dt1.Rows(i).Item("BuyerOffer_Points_Type3").ToString)
                        .Rows(n).Cells(16).Value = Val(Dt1.Rows(i).Item("BuyerOffer_Points_Per_PassMeter_Type3").ToString)
                        .Rows(n).Cells(17).Value = Dt1.Rows(i).Item("BuyerOffer_Grade_Type3").ToString
                        .Rows(n).Cells(18).Value = Dt1.Rows(i).Item("BuyerOffer_Rejection_PieceNo_Type3").ToString

                        .Rows(n).Cells(19).Value = Dt1.Rows(i).Item("PackingSlip_Code_Type3").ToString

                        vRejPcsPackSlpCd = ""
                        If Trim(Dt1.Rows(i).Item("BuyerOffer_Rejection_PieceNo_Type3").ToString) <> "" Then
                            Da = New SqlClient.SqlDataAdapter("Select a.* from Weaver_ClothReceipt_Piece_Details a  INNER JOIN Company_Head tZ ON a.company_idno = tZ.company_idno Where a.Lot_Code = '" & Trim(Dt1.Rows(i).Item("lot_code").ToString) & "' and  a.Piece_No = '" & Trim(Dt1.Rows(i).Item("BuyerOffer_Rejection_PieceNo_Type3").ToString) & "'", con)
                            Dt2 = New DataTable
                            Da.Fill(Dt2)
                            If Dt2.Rows.Count > 0 Then
                                If Val(Dt2.Rows(0).Item("Type5_Meters").ToString) <> 0 Then
                                    vRejPcsPackSlpCd = Dt2.Rows(0).Item("PackingSlip_Code_Type5").ToString
                                ElseIf Val(Dt2.Rows(0).Item("Type4_Meters").ToString) <> 0 Then
                                    vRejPcsPackSlpCd = Dt2.Rows(0).Item("PackingSlip_Code_Type4").ToString
                                ElseIf Val(Dt2.Rows(0).Item("Type3_Meters").ToString) <> 0 Then
                                    vRejPcsPackSlpCd = Dt2.Rows(0).Item("PackingSlip_Code_Type3").ToString
                                ElseIf Val(Dt2.Rows(0).Item("Type2_Meters").ToString) <> 0 Then
                                    vRejPcsPackSlpCd = Dt2.Rows(0).Item("PackingSlip_Code_Type2").ToString
                                Else
                                    vRejPcsPackSlpCd = Dt2.Rows(0).Item("PackingSlip_Code_Type1").ToString
                                End If
                            End If
                            Dt2.Clear()
                        End If
                        .Rows(n).Cells(20).Value = vRejPcsPackSlpCd

                        .Rows(n).Cells(21).Value = Val(Dt1.Rows(i).Item("BuyerOffer_Bits_Meters_Type3").ToString)
                        .Rows(n).Cells(22).Value = Dt1.Rows(i).Item("BuyerOffer_Bits_PieceNo_Type3").ToString
                        vBitsPcsPackSlpCd = ""
                        If Trim(Dt1.Rows(i).Item("BuyerOffer_Bits_PieceNo_Type3").ToString) <> "" Then
                            Da = New SqlClient.SqlDataAdapter("Select a.* from Weaver_ClothReceipt_Piece_Details a  INNER JOIN Company_Head tZ ON a.company_idno = tZ.company_idno Where a.Lot_Code = '" & Trim(Dt1.Rows(i).Item("lot_code").ToString) & "' and  a.Piece_No = '" & Trim(Dt1.Rows(i).Item("BuyerOffer_Bits_PieceNo_Type3").ToString) & "'", con)
                            Dt2 = New DataTable
                            Da.Fill(Dt2)
                            If Dt2.Rows.Count > 0 Then
                                If Val(Dt2.Rows(0).Item("Type5_Meters").ToString) <> 0 Then
                                    vBitsPcsPackSlpCd = Dt2.Rows(0).Item("PackingSlip_Code_Type5").ToString
                                ElseIf Val(Dt2.Rows(0).Item("Type4_Meters").ToString) <> 0 Then
                                    vBitsPcsPackSlpCd = Dt2.Rows(0).Item("PackingSlip_Code_Type4").ToString
                                ElseIf Val(Dt2.Rows(0).Item("Type3_Meters").ToString) <> 0 Then
                                    vBitsPcsPackSlpCd = Dt2.Rows(0).Item("PackingSlip_Code_Type3").ToString
                                ElseIf Val(Dt2.Rows(0).Item("Type2_Meters").ToString) <> 0 Then
                                    vBitsPcsPackSlpCd = Dt2.Rows(0).Item("PackingSlip_Code_Type2").ToString
                                Else
                                    vBitsPcsPackSlpCd = Dt2.Rows(0).Item("PackingSlip_Code_Type1").ToString
                                End If
                            End If
                            Dt2.Clear()
                        End If
                        .Rows(n).Cells(23).Value = vBitsPcsPackSlpCd

                        vLmIdNo = 0
                        If IsDBNull(Dt1.Rows(i).Item("Loom_IdNo").ToString) = False Then
                            vLmIdNo = Val(Dt1.Rows(i).Item("Loom_IdNo").ToString)
                        End If

                        vLmNo = ""
                        If vLmIdNo <> 0 Then
                            vLmNo = Common_Procedures.Loom_IdNoToName(con, vLmIdNo)

                        Else
                            If IsDBNull(Dt1.Rows(i).Item("Loom_No").ToString) = False Then
                                vLmNo = Dt1.Rows(i).Item("Loom_No").ToString
                            End If

                        End If

                        .Rows(n).Cells(24).Value = vLmNo

                        For j = 0 To .ColumnCount - 1
                            .Rows(i).Cells(j).Style.ForeColor = Color.Red
                        Next
                        If Trim(.Rows(n).Cells(19).Value) <> "" Or Trim(.Rows(n).Cells(20).Value) <> "" Then
                            For j = 0 To .ColumnCount - 1
                                .Rows(n).Cells(j).Style.BackColor = Color.LightGray
                            Next
                        End If

                    Next

                End If
                Dt1.Clear()


                Da = New SqlClient.SqlDataAdapter("select a.* ,C.Ledger_Name, d.Cloth_Name from Weaver_ClothReceipt_Piece_Details a  INNER JOIN Company_Head tZ ON a.company_idno = tZ.company_idno  LEFT OUTER JOIN Ledger_Head c ON a.Ledger_IdNo <> 0 and a.Ledger_IdNo = c.Ledger_IdNo  LEFT OUTER JOIN Cloth_Head d ON a.cloth_IdNo <> 0 and a.cloth_IdNo = d.cloth_IdNo where " & CompIDCondt & IIf(Trim(CompIDCondt) <> "", " and ", "") & " a.Type3_Meters <> 0 and a.PackingSlip_Code_Type3 = '' and a.BuyerOffer_Code_Type3 = ''  and " & Cnt_Cond & IIf(Cnt_Cond <> "", " and ", " ") & "  a.Folding = " & Str(Val(txt_Folding.Text)) & "  and (a.StockOff_IdNo = 4 or a.StockOff_IdNo = 5) order by a.Weaver_ClothReceipt_Date, a.for_orderby, a.Weaver_ClothReceipt_No, a.PieceNo_OrderBy, a.Piece_No", con)
                Dt1 = New DataTable
                Da.Fill(Dt1)

                If Dt1.Rows.Count > 0 Then

                    For i = 0 To Dt1.Rows.Count - 1

                        n = .Rows.Add()

                        SNo = SNo + 1
                        .Rows(n).Cells(0).Value = Val(SNo)

                        .Rows(n).Cells(1).Value = Dt1.Rows(i).Item("Lot_No").ToString
                        .Rows(n).Cells(2).Value = Dt1.Rows(i).Item("Piece_No").ToString
                        .Rows(n).Cells(3).Value = Common_Procedures.ClothType.Type3
                        .Rows(n).Cells(4).Value = Format(Val(Dt1.Rows(i).Item("Type3_Meters").ToString), "#########0.00")
                        .Rows(n).Cells(5).Value = Format(Val(Dt1.Rows(i).Item("Type3_Meters").ToString) * Val(Dt1.Rows(i).Item("Weight_Meter").ToString), "#########0.000")
                        If Val(.Rows(n).Cells(5).Value) = 0 Then .Rows(n).Cells(5).Value = ""
                        .Rows(n).Cells(6).Value = Format(Val(Dt1.Rows(i).Item("Weight_Meter").ToString), "#########0.000")
                        If Val(.Rows(n).Cells(6).Value) = 0 Then .Rows(n).Cells(6).Value = ""
                        .Rows(n).Cells(7).Value = Dt1.Rows(i).Item("Ledger_Name").ToString
                        .Rows(n).Cells(8).Value = ""
                        .Rows(n).Cells(9).Value = Dt1.Rows(i).Item("Lot_Code").ToString
                        .Rows(n).Cells(10).Value = Dt1.Rows(i).Item("Cloth_Name").ToString

                        .Rows(n).Cells(11).Value = ""
                        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1464" Then '---- MANI OMEGA FABRICS (THIRUCHENKODU)
                            .Rows(n).Cells(11).Value = Dt1.Rows(i).Item("Piece_No").ToString
                        End If

                        .Rows(n).Cells(12).Value = ""
                        .Rows(n).Cells(13).Value = ""
                        .Rows(n).Cells(14).Value = ""
                        .Rows(n).Cells(15).Value = ""
                        .Rows(n).Cells(16).Value = ""
                        .Rows(n).Cells(17).Value = ""
                        .Rows(n).Cells(18).Value = ""
                        .Rows(n).Cells(19).Value = ""
                        .Rows(n).Cells(20).Value = ""
                        .Rows(n).Cells(21).Value = ""
                        .Rows(n).Cells(22).Value = ""
                        .Rows(n).Cells(23).Value = ""

                        vLmIdNo = 0
                        If IsDBNull(Dt1.Rows(i).Item("Loom_IdNo").ToString) = False Then
                            vLmIdNo = Val(Dt1.Rows(i).Item("Loom_IdNo").ToString)
                        End If

                        vLmNo = ""
                        If vLmIdNo <> 0 Then
                            vLmNo = Common_Procedures.Loom_IdNoToName(con, vLmIdNo)

                        Else
                            If IsDBNull(Dt1.Rows(i).Item("Loom_No").ToString) = False Then
                                vLmNo = Dt1.Rows(i).Item("Loom_No").ToString
                            End If

                        End If

                        .Rows(n).Cells(24).Value = vLmNo

                    Next

                End If
                Dt1.Clear()
            End If

            If CloTypIdNo = 4 Then

                Da = New SqlClient.SqlDataAdapter("select a.* , C.Ledger_Name, d.cloth_name from Weaver_ClothReceipt_Piece_Details a  INNER JOIN Company_Head tZ ON a.company_idno = tZ.company_idno LEFT OUTER JOIN Ledger_Head c ON a.Ledger_IdNo <> 0 and a.Ledger_IdNo = c.Ledger_IdNo LEFT OUTER JOIN Cloth_Head d ON d.Cloth_IdNo <> 0 and a.Cloth_IdNo = d.Cloth_IdNo where " & CompIDCondt & IIf(Trim(CompIDCondt) <> "", " and ", "") & " a.Type4_Meters <> 0 and a.BuyerOffer_Code_Type4 = '" & Trim(NewCode) & "'  and " & Cnt_Cond & IIf(Cnt_Cond <> "", " and ", " ") & "  a.Folding = " & Str(Val(txt_Folding.Text)) & "  and (a.StockOff_IdNo = 4 or a.StockOff_IdNo = 5) order by a.Weaver_ClothReceipt_Date, a.for_orderby, a.Weaver_ClothReceipt_No, a.PieceNo_OrderBy, a.Piece_No", con)
                Dt1 = New DataTable
                Da.Fill(Dt1)

                If Dt1.Rows.Count > 0 Then

                    For i = 0 To Dt1.Rows.Count - 1

                        n = .Rows.Add()

                        SNo = SNo + 1
                        .Rows(n).Cells(0).Value = Val(SNo)

                        .Rows(n).Cells(1).Value = Dt1.Rows(i).Item("Lot_No").ToString
                        .Rows(n).Cells(2).Value = Dt1.Rows(i).Item("Piece_No").ToString
                        .Rows(n).Cells(3).Value = Common_Procedures.ClothType.Type4
                        .Rows(n).Cells(4).Value = Format(Val(Dt1.Rows(i).Item("Type4_Meters").ToString), "#########0.000")
                        .Rows(n).Cells(5).Value = Format(Val(Dt1.Rows(i).Item("Type4_Meters").ToString) * Val(Dt1.Rows(i).Item("Weight_Meter").ToString), "#########0.000")
                        If Val(.Rows(n).Cells(5).Value) = 0 Then .Rows(n).Cells(5).Value = ""
                        .Rows(n).Cells(6).Value = Format(Val(Dt1.Rows(i).Item("Weight_Meter").ToString), "#########0.000")
                        If Val(.Rows(n).Cells(6).Value) = 0 Then .Rows(n).Cells(6).Value = ""
                        .Rows(n).Cells(7).Value = Dt1.Rows(i).Item("Ledger_Name").ToString
                        .Rows(n).Cells(8).Value = "1"
                        .Rows(n).Cells(9).Value = Dt1.Rows(i).Item("Lot_Code").ToString
                        .Rows(n).Cells(10).Value = Dt1.Rows(i).Item("Cloth_Name").ToString

                        .Rows(n).Cells(11).Value = Dt1.Rows(i).Item("BuyerOffer_Party_PieceNo_Type4").ToString
                        .Rows(n).Cells(12).Value = Val(Dt1.Rows(i).Item("BuyerOffer_Passed_Meters_Type4").ToString)
                        .Rows(n).Cells(13).Value = Val(Dt1.Rows(i).Item("BuyerOffer_Less_Meters_Type4").ToString)
                        .Rows(n).Cells(14).Value = Val(Dt1.Rows(i).Item("BuyerOffer_Reject_Meters_Type4").ToString)
                        .Rows(n).Cells(15).Value = Val(Dt1.Rows(i).Item("BuyerOffer_Points_Type4").ToString)
                        .Rows(n).Cells(16).Value = Val(Dt1.Rows(i).Item("BuyerOffer_Points_Per_PassMeter_Type4").ToString)
                        .Rows(n).Cells(17).Value = Dt1.Rows(i).Item("BuyerOffer_Grade_Type4").ToString
                        .Rows(n).Cells(18).Value = Dt1.Rows(i).Item("BuyerOffer_Rejection_PieceNo_Type4").ToString

                        .Rows(n).Cells(19).Value = Dt1.Rows(i).Item("PackingSlip_Code_Type4").ToString

                        vRejPcsPackSlpCd = ""
                        If Trim(Dt1.Rows(i).Item("BuyerOffer_Rejection_PieceNo_Type4").ToString) <> "" Then
                            Da = New SqlClient.SqlDataAdapter("Select a.* from Weaver_ClothReceipt_Piece_Details a  INNER JOIN Company_Head tZ ON a.company_idno = tZ.company_idno Where a.Lot_Code = '" & Trim(Dt1.Rows(i).Item("lot_code").ToString) & "' and  a.Piece_No = '" & Trim(Dt1.Rows(i).Item("BuyerOffer_Rejection_PieceNo_Type4").ToString) & "'", con)
                            Dt2 = New DataTable
                            Da.Fill(Dt2)
                            If Dt2.Rows.Count > 0 Then
                                If Val(Dt2.Rows(0).Item("Type5_Meters").ToString) <> 0 Then
                                    vRejPcsPackSlpCd = Dt2.Rows(0).Item("PackingSlip_Code_Type5").ToString
                                ElseIf Val(Dt2.Rows(0).Item("Type4_Meters").ToString) <> 0 Then
                                    vRejPcsPackSlpCd = Dt2.Rows(0).Item("PackingSlip_Code_Type4").ToString
                                ElseIf Val(Dt2.Rows(0).Item("Type3_Meters").ToString) <> 0 Then
                                    vRejPcsPackSlpCd = Dt2.Rows(0).Item("PackingSlip_Code_Type3").ToString
                                ElseIf Val(Dt2.Rows(0).Item("Type2_Meters").ToString) <> 0 Then
                                    vRejPcsPackSlpCd = Dt2.Rows(0).Item("PackingSlip_Code_Type2").ToString
                                Else
                                    vRejPcsPackSlpCd = Dt2.Rows(0).Item("PackingSlip_Code_Type1").ToString
                                End If
                            End If
                            Dt2.Clear()
                        End If
                        .Rows(n).Cells(20).Value = vRejPcsPackSlpCd

                        .Rows(n).Cells(21).Value = Val(Dt1.Rows(i).Item("BuyerOffer_Bits_Meters_Type4").ToString)
                        .Rows(n).Cells(22).Value = Dt1.Rows(i).Item("BuyerOffer_Bits_PieceNo_Type4").ToString
                        vBitsPcsPackSlpCd = ""
                        If Trim(Dt1.Rows(i).Item("BuyerOffer_Bits_PieceNo_Type4").ToString) <> "" Then
                            Da = New SqlClient.SqlDataAdapter("Select a.* from Weaver_ClothReceipt_Piece_Details a  INNER JOIN Company_Head tZ ON a.company_idno = tZ.company_idno Where a.Lot_Code = '" & Trim(Dt1.Rows(i).Item("lot_code").ToString) & "' and  a.Piece_No = '" & Trim(Dt1.Rows(i).Item("BuyerOffer_Bits_PieceNo_Type4").ToString) & "'", con)
                            Dt2 = New DataTable
                            Da.Fill(Dt2)
                            If Dt2.Rows.Count > 0 Then
                                If Val(Dt2.Rows(0).Item("Type5_Meters").ToString) <> 0 Then
                                    vBitsPcsPackSlpCd = Dt2.Rows(0).Item("PackingSlip_Code_Type5").ToString
                                ElseIf Val(Dt2.Rows(0).Item("Type4_Meters").ToString) <> 0 Then
                                    vBitsPcsPackSlpCd = Dt2.Rows(0).Item("PackingSlip_Code_Type4").ToString
                                ElseIf Val(Dt2.Rows(0).Item("Type3_Meters").ToString) <> 0 Then
                                    vBitsPcsPackSlpCd = Dt2.Rows(0).Item("PackingSlip_Code_Type3").ToString
                                ElseIf Val(Dt2.Rows(0).Item("Type2_Meters").ToString) <> 0 Then
                                    vBitsPcsPackSlpCd = Dt2.Rows(0).Item("PackingSlip_Code_Type2").ToString
                                Else
                                    vBitsPcsPackSlpCd = Dt2.Rows(0).Item("PackingSlip_Code_Type1").ToString
                                End If
                            End If
                            Dt2.Clear()
                        End If
                        .Rows(n).Cells(23).Value = vBitsPcsPackSlpCd

                        vLmIdNo = 0
                        If IsDBNull(Dt1.Rows(i).Item("Loom_IdNo").ToString) = False Then
                            vLmIdNo = Val(Dt1.Rows(i).Item("Loom_IdNo").ToString)
                        End If

                        vLmNo = ""
                        If vLmIdNo <> 0 Then
                            vLmNo = Common_Procedures.Loom_IdNoToName(con, vLmIdNo)

                        Else
                            If IsDBNull(Dt1.Rows(i).Item("Loom_No").ToString) = False Then
                                vLmNo = Dt1.Rows(i).Item("Loom_No").ToString
                            End If

                        End If

                        .Rows(n).Cells(24).Value = vLmNo

                        For j = 0 To .ColumnCount - 1
                            .Rows(i).Cells(j).Style.ForeColor = Color.Red
                        Next
                        If Trim(.Rows(n).Cells(19).Value) <> "" Or Trim(.Rows(n).Cells(20).Value) <> "" Then
                            For j = 0 To .ColumnCount - 1
                                .Rows(n).Cells(j).Style.BackColor = Color.LightGray
                            Next
                        End If

                    Next

                End If
                Dt1.Clear()


                Da = New SqlClient.SqlDataAdapter("select a.* , C.Ledger_Name, d.cloth_name from Weaver_ClothReceipt_Piece_Details a  INNER JOIN Company_Head tZ ON a.company_idno = tZ.company_idno  LEFT OUTER JOIN Ledger_Head c ON a.Ledger_IdNo <> 0 and a.Ledger_IdNo = c.Ledger_IdNo LEFT OUTER JOIN Cloth_Head d ON a.Cloth_IdNo <> 0 and a.Cloth_IdNo = d.Cloth_IdNo where " & CompIDCondt & IIf(Trim(CompIDCondt) <> "", " and ", "") & " a.Type4_Meters <> 0 and a.PackingSlip_Code_Type4 = '' and a.BuyerOffer_Code_Type4 = ''  and " & Cnt_Cond & IIf(Cnt_Cond <> "", " and ", " ") & "  a.Folding = " & Str(Val(txt_Folding.Text)) & "  and (a.StockOff_IdNo = 4 or a.StockOff_IdNo = 5) order by a.Weaver_ClothReceipt_Date, a.for_orderby, a.Weaver_ClothReceipt_No, a.PieceNo_OrderBy, a.Piece_No", con)
                Dt1 = New DataTable
                Da.Fill(Dt1)

                If Dt1.Rows.Count > 0 Then

                    For i = 0 To Dt1.Rows.Count - 1

                        n = .Rows.Add()

                        SNo = SNo + 1
                        .Rows(n).Cells(0).Value = Val(SNo)

                        .Rows(n).Cells(1).Value = Dt1.Rows(i).Item("Lot_No").ToString
                        .Rows(n).Cells(2).Value = Dt1.Rows(i).Item("Piece_No").ToString
                        .Rows(n).Cells(3).Value = Common_Procedures.ClothType.Type4
                        .Rows(n).Cells(4).Value = Format(Val(Dt1.Rows(i).Item("Type4_Meters").ToString), "#########0.00")
                        .Rows(n).Cells(5).Value = Format(Val(Dt1.Rows(i).Item("Type4_Meters").ToString) * Val(Dt1.Rows(i).Item("Weight_Meter").ToString), "#########0.000")
                        If Val(.Rows(n).Cells(5).Value) = 0 Then .Rows(n).Cells(5).Value = ""
                        .Rows(n).Cells(6).Value = Format(Val(Dt1.Rows(i).Item("Weight_Meter").ToString), "#########0.000")
                        If Val(.Rows(n).Cells(6).Value) = 0 Then .Rows(n).Cells(6).Value = ""
                        .Rows(n).Cells(7).Value = Dt1.Rows(i).Item("Ledger_Name").ToString
                        .Rows(n).Cells(8).Value = ""
                        .Rows(n).Cells(9).Value = Dt1.Rows(i).Item("Lot_Code").ToString
                        .Rows(n).Cells(10).Value = Dt1.Rows(i).Item("Cloth_Name").ToString

                        .Rows(n).Cells(11).Value = ""
                        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1464" Then '---- MANI OMEGA FABRICS (THIRUCHENKODU)
                            .Rows(n).Cells(11).Value = Dt1.Rows(i).Item("Piece_No").ToString
                        End If

                        .Rows(n).Cells(12).Value = ""
                        .Rows(n).Cells(13).Value = ""
                        .Rows(n).Cells(14).Value = ""
                        .Rows(n).Cells(15).Value = ""
                        .Rows(n).Cells(16).Value = ""
                        .Rows(n).Cells(17).Value = ""
                        .Rows(n).Cells(18).Value = ""
                        .Rows(n).Cells(19).Value = ""
                        .Rows(n).Cells(20).Value = ""
                        .Rows(n).Cells(21).Value = ""
                        .Rows(n).Cells(22).Value = ""
                        .Rows(n).Cells(23).Value = ""

                        vLmIdNo = 0
                        If IsDBNull(Dt1.Rows(i).Item("Loom_IdNo").ToString) = False Then
                            vLmIdNo = Val(Dt1.Rows(i).Item("Loom_IdNo").ToString)
                        End If

                        vLmNo = ""
                        If vLmIdNo <> 0 Then
                            vLmNo = Common_Procedures.Loom_IdNoToName(con, vLmIdNo)

                        Else
                            If IsDBNull(Dt1.Rows(i).Item("Loom_No").ToString) = False Then
                                vLmNo = Dt1.Rows(i).Item("Loom_No").ToString
                            End If

                        End If

                        .Rows(n).Cells(24).Value = vLmNo

                    Next

                End If
                Dt1.Clear()
            End If
            If CloTypIdNo = 5 Then

                Da = New SqlClient.SqlDataAdapter("select a.*, c.Ledger_Name, d.cloth_name from Weaver_ClothReceipt_Piece_Details a LEFT OUTER JOIN Ledger_Head c ON a.Ledger_IdNo <> 0 and a.Ledger_IdNo = c.Ledger_IdNo LEFT OUTER JOIN Cloth_Head d ON a.Cloth_IdNo <> 0 and a.Cloth_IdNo = d.Cloth_IdNo where " & CompIDCondt & IIf(Trim(CompIDCondt) <> "", " and ", "") & " a.Type5_Meters <> 0 and a.BuyerOffer_Code_Type5 = '" & Trim(NewCode) & "'  and " & Cnt_Cond & IIf(Cnt_Cond <> "", " and ", " ") & "  a.Folding = " & Str(Val(txt_Folding.Text)) & " and (a.StockOff_IdNo = 4 or a.StockOff_IdNo = 5) order by a.Weaver_ClothReceipt_Date, a.for_orderby, a.Weaver_ClothReceipt_No, a.PieceNo_OrderBy, a.Piece_No", con)
                Dt1 = New DataTable
                Da.Fill(Dt1)

                If Dt1.Rows.Count > 0 Then

                    For i = 0 To Dt1.Rows.Count - 1

                        n = .Rows.Add()

                        SNo = SNo + 1
                        .Rows(n).Cells(0).Value = Val(SNo)

                        .Rows(n).Cells(1).Value = Dt1.Rows(i).Item("Lot_No").ToString
                        .Rows(n).Cells(2).Value = Dt1.Rows(i).Item("Piece_No").ToString
                        .Rows(n).Cells(3).Value = Common_Procedures.ClothType.Type5
                        .Rows(n).Cells(4).Value = Format(Val(Dt1.Rows(i).Item("Type5_Meters").ToString), "#########0.00")
                        .Rows(n).Cells(5).Value = Format(Val(Dt1.Rows(i).Item("Type5_Meters").ToString) * Val(Dt1.Rows(i).Item("Weight_Meter").ToString), "#########0.000")
                        If Val(.Rows(n).Cells(5).Value) = 0 Then .Rows(n).Cells(5).Value = ""
                        .Rows(n).Cells(6).Value = Format(Val(Dt1.Rows(i).Item("Weight_Meter").ToString), "#########0.000")
                        If Val(.Rows(n).Cells(6).Value) = 0 Then .Rows(n).Cells(6).Value = ""
                        .Rows(n).Cells(7).Value = Dt1.Rows(i).Item("Ledger_Name").ToString
                        .Rows(n).Cells(8).Value = "1"
                        .Rows(n).Cells(9).Value = Dt1.Rows(i).Item("Lot_Code").ToString
                        .Rows(n).Cells(10).Value = Dt1.Rows(i).Item("Cloth_Name").ToString

                        .Rows(n).Cells(11).Value = Dt1.Rows(i).Item("BuyerOffer_Party_PieceNo_Type5").ToString
                        .Rows(n).Cells(12).Value = Val(Dt1.Rows(i).Item("BuyerOffer_Passed_Meters_Type5").ToString)
                        .Rows(n).Cells(13).Value = Val(Dt1.Rows(i).Item("BuyerOffer_Less_Meters_Type5").ToString)
                        .Rows(n).Cells(14).Value = Val(Dt1.Rows(i).Item("BuyerOffer_Reject_Meters_Type5").ToString)
                        .Rows(n).Cells(15).Value = Val(Dt1.Rows(i).Item("BuyerOffer_Points_Type5").ToString)
                        .Rows(n).Cells(16).Value = Val(Dt1.Rows(i).Item("BuyerOffer_Points_Per_PassMeter_Type5").ToString)
                        .Rows(n).Cells(17).Value = Dt1.Rows(i).Item("BuyerOffer_Grade_Type5").ToString
                        .Rows(n).Cells(18).Value = Dt1.Rows(i).Item("BuyerOffer_Rejection_PieceNo_Type5").ToString

                        .Rows(n).Cells(19).Value = Dt1.Rows(i).Item("PackingSlip_Code_Type5").ToString

                        vRejPcsPackSlpCd = ""
                        If Trim(Dt1.Rows(i).Item("BuyerOffer_Rejection_PieceNo_Type5").ToString) <> "" Then
                            Da = New SqlClient.SqlDataAdapter("Select a.* from Weaver_ClothReceipt_Piece_Details a Where a.Lot_Code = '" & Trim(Dt1.Rows(i).Item("lot_code").ToString) & "' and  a.Piece_No = '" & Trim(Dt1.Rows(i).Item("BuyerOffer_Rejection_PieceNo_Type5").ToString) & "'", con)
                            Dt2 = New DataTable
                            Da.Fill(Dt2)
                            If Dt2.Rows.Count > 0 Then
                                If Val(Dt2.Rows(0).Item("Type5_Meters").ToString) <> 0 Then
                                    vRejPcsPackSlpCd = Dt2.Rows(0).Item("PackingSlip_Code_Type5").ToString
                                ElseIf Val(Dt2.Rows(0).Item("Type4_Meters").ToString) <> 0 Then
                                    vRejPcsPackSlpCd = Dt2.Rows(0).Item("PackingSlip_Code_Type4").ToString
                                ElseIf Val(Dt2.Rows(0).Item("Type3_Meters").ToString) <> 0 Then
                                    vRejPcsPackSlpCd = Dt2.Rows(0).Item("PackingSlip_Code_Type3").ToString
                                ElseIf Val(Dt2.Rows(0).Item("Type2_Meters").ToString) <> 0 Then
                                    vRejPcsPackSlpCd = Dt2.Rows(0).Item("PackingSlip_Code_Type2").ToString
                                Else
                                    vRejPcsPackSlpCd = Dt2.Rows(0).Item("PackingSlip_Code_Type1").ToString
                                End If
                            End If
                            Dt2.Clear()
                        End If
                        .Rows(n).Cells(20).Value = vRejPcsPackSlpCd

                        .Rows(n).Cells(21).Value = Val(Dt1.Rows(i).Item("BuyerOffer_Bits_Meters_Type5").ToString)
                        .Rows(n).Cells(22).Value = Dt1.Rows(i).Item("BuyerOffer_Bits_PieceNo_Type5").ToString
                        vBitsPcsPackSlpCd = ""
                        If Trim(Dt1.Rows(i).Item("BuyerOffer_Bits_PieceNo_Type5").ToString) <> "" Then
                            Da = New SqlClient.SqlDataAdapter("Select a.* from Weaver_ClothReceipt_Piece_Details a Where a.Lot_Code = '" & Trim(Dt1.Rows(i).Item("lot_code").ToString) & "' and  a.Piece_No = '" & Trim(Dt1.Rows(i).Item("BuyerOffer_Bits_PieceNo_Type5").ToString) & "'", con)
                            Dt2 = New DataTable
                            Da.Fill(Dt2)
                            If Dt2.Rows.Count > 0 Then
                                If Val(Dt2.Rows(0).Item("Type5_Meters").ToString) <> 0 Then
                                    vBitsPcsPackSlpCd = Dt2.Rows(0).Item("PackingSlip_Code_Type5").ToString
                                ElseIf Val(Dt2.Rows(0).Item("Type4_Meters").ToString) <> 0 Then
                                    vBitsPcsPackSlpCd = Dt2.Rows(0).Item("PackingSlip_Code_Type4").ToString
                                ElseIf Val(Dt2.Rows(0).Item("Type3_Meters").ToString) <> 0 Then
                                    vBitsPcsPackSlpCd = Dt2.Rows(0).Item("PackingSlip_Code_Type3").ToString
                                ElseIf Val(Dt2.Rows(0).Item("Type2_Meters").ToString) <> 0 Then
                                    vBitsPcsPackSlpCd = Dt2.Rows(0).Item("PackingSlip_Code_Type2").ToString
                                Else
                                    vBitsPcsPackSlpCd = Dt2.Rows(0).Item("PackingSlip_Code_Type1").ToString
                                End If
                            End If
                            Dt2.Clear()
                        End If
                        .Rows(n).Cells(23).Value = vBitsPcsPackSlpCd

                        vLmIdNo = 0
                        If IsDBNull(Dt1.Rows(i).Item("Loom_IdNo").ToString) = False Then
                            vLmIdNo = Val(Dt1.Rows(i).Item("Loom_IdNo").ToString)
                        End If

                        vLmNo = ""
                        If vLmIdNo <> 0 Then
                            vLmNo = Common_Procedures.Loom_IdNoToName(con, vLmIdNo)

                        Else
                            If IsDBNull(Dt1.Rows(i).Item("Loom_No").ToString) = False Then
                                vLmNo = Dt1.Rows(i).Item("Loom_No").ToString
                            End If

                        End If

                        .Rows(n).Cells(24).Value = vLmNo

                        For j = 0 To .ColumnCount - 1
                            .Rows(i).Cells(j).Style.ForeColor = Color.Red
                        Next
                        If Trim(.Rows(n).Cells(19).Value) <> "" Or Trim(.Rows(n).Cells(20).Value) <> "" Then
                            For j = 0 To .ColumnCount - 1
                                .Rows(n).Cells(j).Style.BackColor = Color.LightGray
                            Next
                        End If


                    Next

                End If
                Dt1.Clear()


                Da = New SqlClient.SqlDataAdapter("select a.*, c.Ledger_Name, d.cloth_name from Weaver_ClothReceipt_Piece_Details a LEFT OUTER JOIN Ledger_Head c ON a.Ledger_IdNo <> 0 and a.Ledger_IdNo = c.Ledger_IdNo LEFT OUTER JOIN Cloth_Head d ON a.Cloth_IdNo <> 0 and a.Cloth_IdNo = d.Cloth_IdNo where " & CompIDCondt & IIf(Trim(CompIDCondt) <> "", " and ", "") & " a.Type5_Meters <> 0 and a.PackingSlip_Code_Type5 = '' and a.BuyerOffer_Code_Type5 = '' and " & Cnt_Cond & IIf(Cnt_Cond <> "", " and ", " ") & "  a.Folding = " & Str(Val(txt_Folding.Text)) & "  and (a.StockOff_IdNo = 4 or a.StockOff_IdNo = 5) order by a.Weaver_ClothReceipt_Date, a.for_orderby, a.Weaver_ClothReceipt_No, a.PieceNo_OrderBy, a.Piece_No", con)
                Dt1 = New DataTable
                Da.Fill(Dt1)

                If Dt1.Rows.Count > 0 Then

                    For i = 0 To Dt1.Rows.Count - 1

                        n = .Rows.Add()

                        SNo = SNo + 1
                        .Rows(n).Cells(0).Value = Val(SNo)

                        .Rows(n).Cells(1).Value = Dt1.Rows(i).Item("Lot_No").ToString
                        .Rows(n).Cells(2).Value = Dt1.Rows(i).Item("Piece_No").ToString
                        .Rows(n).Cells(3).Value = Common_Procedures.ClothType.Type5
                        .Rows(n).Cells(4).Value = Format(Val(Dt1.Rows(i).Item("Type5_Meters").ToString), "#########0.00")
                        .Rows(n).Cells(5).Value = Format(Val(Dt1.Rows(i).Item("Type5_Meters").ToString) * Val(Dt1.Rows(i).Item("Weight_Meter").ToString), "#########0.000")
                        If Val(.Rows(n).Cells(5).Value) = 0 Then .Rows(n).Cells(5).Value = ""
                        .Rows(n).Cells(6).Value = Format(Val(Dt1.Rows(i).Item("Weight_Meter").ToString), "#########0.000")
                        If Val(.Rows(n).Cells(6).Value) = 0 Then .Rows(n).Cells(6).Value = ""
                        .Rows(n).Cells(7).Value = Dt1.Rows(i).Item("Ledger_Name").ToString
                        .Rows(n).Cells(8).Value = ""
                        .Rows(n).Cells(9).Value = Dt1.Rows(i).Item("Lot_Code").ToString
                        .Rows(n).Cells(10).Value = Dt1.Rows(i).Item("Cloth_Name").ToString

                        .Rows(n).Cells(11).Value = ""
                        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1464" Then '---- MANI OMEGA FABRICS (THIRUCHENKODU)
                            .Rows(n).Cells(11).Value = Dt1.Rows(i).Item("Piece_No").ToString
                        End If

                        .Rows(n).Cells(12).Value = ""
                        .Rows(n).Cells(13).Value = ""
                        .Rows(n).Cells(14).Value = ""
                        .Rows(n).Cells(15).Value = ""
                        .Rows(n).Cells(16).Value = ""
                        .Rows(n).Cells(17).Value = ""
                        .Rows(n).Cells(18).Value = ""
                        .Rows(n).Cells(19).Value = ""
                        .Rows(n).Cells(20).Value = ""
                        .Rows(n).Cells(21).Value = ""
                        .Rows(n).Cells(22).Value = ""
                        .Rows(n).Cells(23).Value = ""

                        vLmIdNo = 0
                        If IsDBNull(Dt1.Rows(i).Item("Loom_IdNo").ToString) = False Then
                            vLmIdNo = Val(Dt1.Rows(i).Item("Loom_IdNo").ToString)
                        End If

                        vLmNo = ""
                        If vLmIdNo <> 0 Then
                            vLmNo = Common_Procedures.Loom_IdNoToName(con, vLmIdNo)

                        Else
                            If IsDBNull(Dt1.Rows(i).Item("Loom_No").ToString) = False Then
                                vLmNo = Dt1.Rows(i).Item("Loom_No").ToString
                            End If

                        End If

                        .Rows(n).Cells(24).Value = vLmNo


                    Next

                End If
                Dt1.Clear()
            End If
        End With

        Cmd.Connection = con

        Cmd.CommandText = "truncate table " & Trim(Common_Procedures.EntryTempSubTable) & ""
        Cmd.ExecuteNonQuery()

        Cmd.CommandText = "Insert into " & Trim(Common_Procedures.EntryTempSubTable) & " ( Int1, Name1, Name2, Meters1 ) select sl_no, Lot_Code, Pcs_No, Meters from Buyer_Offer_Details Where Buyer_Offer_Code = '" & Trim(NewCode) & "'"
        Cmd.ExecuteNonQuery()

        pnl_Selection.Visible = True
        Pnl_Back.Enabled = False
        dgv_Selection.Focus()

    End Sub

    Private Sub dgv_Selection_CellClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Selection.CellClick
        Select_Piece(e.RowIndex)
    End Sub

    Private Sub dgv_Selection_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_Selection.KeyDown
        Dim n As Integer = 0

        On Error Resume Next

        If e.KeyCode = Keys.Enter Or e.KeyCode = Keys.Space Then
            If dgv_Selection.CurrentCell.RowIndex >= 0 Then

                n = dgv_Selection.CurrentCell.RowIndex

                Select_Piece(n)

                e.Handled = True

            End If

        End If
    End Sub

    Private Sub Select_Piece(ByVal RwIndx As Integer)
        Dim Cmd As New SqlClient.SqlCommand
        Dim i As Integer = 0
        Dim S As String = ""
        Dim MxId As Integer = 0

        With dgv_Selection

            Cmd.Connection = con

            If .RowCount > 0 And RwIndx >= 0 Then

                If Trim(dgv_Selection.Rows(RwIndx).Cells(19).Value) = "" And Trim(dgv_Selection.Rows(RwIndx).Cells(20).Value) = "" And Trim(dgv_Selection.Rows(RwIndx).Cells(23).Value) = "" Then

                    .Rows(RwIndx).Cells(8).Value = (Val(.Rows(RwIndx).Cells(8).Value) + 1) Mod 2

                    If Val(.Rows(RwIndx).Cells(8).Value) = 1 Then

                        For i = 0 To .ColumnCount - 1
                            .Rows(RwIndx).Cells(i).Style.ForeColor = Color.Red
                        Next

                        MxId = Common_Procedures.get_MaxIdNo(con, "" & Trim(Common_Procedures.EntryTempSubTable) & "", "Int1", "")

                        Cmd.CommandText = "Insert into " & Trim(Common_Procedures.EntryTempSubTable) & " ( Int1, Name1, Name2, Meters1 ) Values (" & Str(Val(MxId)) & ", '" & Trim(.Rows(RwIndx).Cells(9).Value) & "', '" & Trim(.Rows(RwIndx).Cells(2).Value) & "', " & Str(Val(.Rows(RwIndx).Cells(4).Value)) & " ) "
                        Cmd.ExecuteNonQuery()

                    Else

                        .Rows(RwIndx).Cells(8).Value = ""

                        For i = 0 To .ColumnCount - 1
                            .Rows(RwIndx).Cells(i).Style.ForeColor = Color.Blue
                        Next

                        Cmd.CommandText = "Delete from " & Trim(Common_Procedures.EntryTempSubTable) & " where Name1 = '" & Trim(.Rows(RwIndx).Cells(9).Value) & "' and Name2 = '" & Trim(.Rows(RwIndx).Cells(2).Value) & "'"
                        Cmd.ExecuteNonQuery()

                    End If

                Else


                    S = ""
                    If Trim(dgv_Selection.Rows(RwIndx).Cells(19).Value) <> "" Then
                        S = "Already Roll Packed = " & Trim(dgv_Selection.Rows(RwIndx).Cells(19).Value)
                    End If

                    If Trim(dgv_Selection.Rows(RwIndx).Cells(20).Value) <> "" Then
                        S = Trim(S) & IIf(Trim(S) <> "", " and " & Chr(13), "") & " Already Rejected Pcs Baled/Delivered = " & Trim(dgv_Selection.Rows(RwIndx).Cells(20).Value)
                    End If

                    If Trim(dgv_Selection.Rows(RwIndx).Cells(23).Value) <> "" Then
                        S = Trim(S) & IIf(Trim(S) <> "", " and " & Chr(13), "") & " Already Bits Pcs Baled/Delivered = " & Trim(dgv_Selection.Rows(RwIndx).Cells(23).Value)
                    End If

                    MessageBox.Show(S, "DOES NOT REMOVE THIS ROW...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

                    Exit Sub

                End If

            End If

        End With

    End Sub

    Private Sub btn_Close_Selection_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Close_Selection.Click
        Piece_Selection()
    End Sub

    Private Sub Piece_Selection()
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim n As Integer = 0
        Dim sno As Integer = 0
        Dim I As Integer = 0, J As Integer = 0, K As Integer = 0

        Try

            Pnl_Back.Enabled = True
            pnl_Selection.Visible = False
            dgv_Details.Rows.Clear()

            sno = 0

            Da = New SqlClient.SqlDataAdapter("Select * from " & Trim(Common_Procedures.EntryTempSubTable) & " Where Name1 <> '' and Name2 <> '' Order by Int1 ", con)
            Dt1 = New DataTable
            Da.Fill(Dt1)
            If Dt1.Rows.Count > 0 Then

                For J = 0 To Dt1.Rows.Count - 1

                    For I = 0 To dgv_Selection.RowCount - 1

                        If Val(dgv_Selection.Rows(I).Cells(8).Value) = 1 Then

                            If Trim(UCase(dgv_Selection.Rows(I).Cells(9).Value)) = Trim(UCase(Dt1.Rows(J).Item("Name1").ToString)) And Trim(UCase(dgv_Selection.Rows(I).Cells(2).Value)) = Trim(UCase(Dt1.Rows(J).Item("Name2").ToString)) Then

                                n = dgv_Details.Rows.Add()

                                sno = sno + 1
                                dgv_Details.Rows(n).Cells(0).Value = sno
                                dgv_Details.Rows(n).Cells(1).Value = dgv_Selection.Rows(I).Cells(1).Value
                                dgv_Details.Rows(n).Cells(2).Value = dgv_Selection.Rows(I).Cells(2).Value
                                dgv_Details.Rows(n).Cells(3).Value = dgv_Selection.Rows(I).Cells(3).Value
                                dgv_Details.Rows(n).Cells(4).Value = dgv_Selection.Rows(I).Cells(4).Value
                                If Val(dgv_Selection.Rows(I).Cells(5).Value) <> 0 Then
                                    dgv_Details.Rows(n).Cells(5).Value = dgv_Selection.Rows(I).Cells(5).Value
                                End If
                                If Val(dgv_Selection.Rows(I).Cells(6).Value) <> 0 Then
                                    dgv_Details.Rows(n).Cells(6).Value = dgv_Selection.Rows(I).Cells(6).Value
                                End If
                                dgv_Details.Rows(n).Cells(7).Value = dgv_Selection.Rows(I).Cells(11).Value
                                dgv_Details.Rows(n).Cells(8).Value = dgv_Selection.Rows(I).Cells(12).Value
                                dgv_Details.Rows(n).Cells(9).Value = dgv_Selection.Rows(I).Cells(13).Value
                                If Val(dgv_Details.Rows(n).Cells(9).Value) = 0 Then dgv_Details.Rows(n).Cells(9).Value = ""
                                dgv_Details.Rows(n).Cells(10).Value = dgv_Selection.Rows(I).Cells(14).Value
                                If Val(dgv_Details.Rows(n).Cells(10).Value) = 0 Then dgv_Details.Rows(n).Cells(10).Value = ""
                                dgv_Details.Rows(n).Cells(11).Value = dgv_Selection.Rows(I).Cells(15).Value
                                If Val(dgv_Details.Rows(n).Cells(11).Value) = 0 Then dgv_Details.Rows(n).Cells(11).Value = ""
                                dgv_Details.Rows(n).Cells(12).Value = dgv_Selection.Rows(I).Cells(16).Value
                                If Val(dgv_Details.Rows(n).Cells(12).Value) = 0 Then dgv_Details.Rows(n).Cells(12).Value = ""
                                dgv_Details.Rows(n).Cells(13).Value = dgv_Selection.Rows(I).Cells(17).Value
                                dgv_Details.Rows(n).Cells(14).Value = dgv_Selection.Rows(I).Cells(18).Value
                                dgv_Details.Rows(n).Cells(15).Value = dgv_Selection.Rows(I).Cells(9).Value
                                dgv_Details.Rows(n).Cells(16).Value = dgv_Selection.Rows(I).Cells(7).Value
                                dgv_Details.Rows(n).Cells(17).Value = dgv_Selection.Rows(I).Cells(10).Value
                                dgv_Details.Rows(n).Cells(18).Value = dgv_Selection.Rows(I).Cells(19).Value
                                dgv_Details.Rows(n).Cells(19).Value = dgv_Selection.Rows(I).Cells(20).Value
                                dgv_Details.Rows(n).Cells(20).Value = dgv_Selection.Rows(I).Cells(21).Value
                                dgv_Details.Rows(n).Cells(21).Value = dgv_Selection.Rows(I).Cells(22).Value
                                dgv_Details.Rows(n).Cells(22).Value = dgv_Selection.Rows(I).Cells(23).Value
                                dgv_Details.Rows(n).Cells(23).Value = dgv_Selection.Rows(I).Cells(24).Value

                                If Trim(dgv_Details.Rows(n).Cells(18).Value) <> "" Or Trim(dgv_Details.Rows(n).Cells(19).Value) <> "" Or Trim(dgv_Details.Rows(n).Cells(22).Value) <> "" Then
                                    For K = 0 To dgv_Details.ColumnCount - 1
                                        dgv_Details.Rows(n).Cells(K).Style.BackColor = Color.LightGray
                                        dgv_Details.Rows(n).Cells(K).Style.ForeColor = Color.Red
                                    Next
                                End If

                            End If

                        End If

                    Next I

                Next
            End If
            Dt1.Clear()


            For I = 0 To dgv_Selection.RowCount - 1

                If Val(dgv_Selection.Rows(I).Cells(8).Value) = 1 And Trim(dgv_Selection.Rows(I).Cells(9).Value) <> "" And Trim(dgv_Selection.Rows(I).Cells(2).Value) <> "" Then

                    Da = New SqlClient.SqlDataAdapter("Select * from " & Trim(Common_Procedures.EntryTempSubTable) & " where Name1 = '" & Trim(dgv_Selection.Rows(I).Cells(9).Value) & "' and Name2 = '" & Trim(dgv_Selection.Rows(I).Cells(2).Value) & "' and Name1 <> '' and Name2 <> ''", con)
                    Dt1 = New DataTable
                    Da.Fill(Dt1)
                    If Dt1.Rows.Count = 0 Then

                        n = dgv_Details.Rows.Add()

                        sno = sno + 1
                        dgv_Details.Rows(n).Cells(0).Value = sno
                        dgv_Details.Rows(n).Cells(1).Value = dgv_Selection.Rows(I).Cells(1).Value
                        dgv_Details.Rows(n).Cells(2).Value = dgv_Selection.Rows(I).Cells(2).Value
                        dgv_Details.Rows(n).Cells(3).Value = dgv_Selection.Rows(I).Cells(3).Value
                        dgv_Details.Rows(n).Cells(4).Value = dgv_Selection.Rows(I).Cells(4).Value
                        If Val(dgv_Selection.Rows(I).Cells(5).Value) <> 0 Then
                            dgv_Details.Rows(n).Cells(5).Value = dgv_Selection.Rows(I).Cells(5).Value
                        End If
                        If Val(dgv_Selection.Rows(I).Cells(6).Value) <> 0 Then
                            dgv_Details.Rows(n).Cells(6).Value = dgv_Selection.Rows(I).Cells(6).Value
                        End If
                        dgv_Details.Rows(n).Cells(7).Value = dgv_Selection.Rows(I).Cells(11).Value
                        dgv_Details.Rows(n).Cells(8).Value = dgv_Selection.Rows(I).Cells(12).Value
                        dgv_Details.Rows(n).Cells(9).Value = dgv_Selection.Rows(I).Cells(13).Value
                        If Val(dgv_Details.Rows(n).Cells(9).Value) = 0 Then dgv_Details.Rows(n).Cells(9).Value = ""
                        dgv_Details.Rows(n).Cells(10).Value = dgv_Selection.Rows(I).Cells(14).Value
                        If Val(dgv_Details.Rows(n).Cells(10).Value) = 0 Then dgv_Details.Rows(n).Cells(10).Value = ""
                        dgv_Details.Rows(n).Cells(11).Value = dgv_Selection.Rows(I).Cells(15).Value
                        If Val(dgv_Details.Rows(n).Cells(11).Value) = 0 Then dgv_Details.Rows(n).Cells(11).Value = ""
                        dgv_Details.Rows(n).Cells(12).Value = dgv_Selection.Rows(I).Cells(16).Value
                        If Val(dgv_Details.Rows(n).Cells(12).Value) = 0 Then dgv_Details.Rows(n).Cells(12).Value = ""
                        dgv_Details.Rows(n).Cells(13).Value = dgv_Selection.Rows(I).Cells(17).Value
                        dgv_Details.Rows(n).Cells(14).Value = dgv_Selection.Rows(I).Cells(18).Value
                        dgv_Details.Rows(n).Cells(15).Value = dgv_Selection.Rows(I).Cells(9).Value
                        dgv_Details.Rows(n).Cells(16).Value = dgv_Selection.Rows(I).Cells(7).Value
                        dgv_Details.Rows(n).Cells(17).Value = dgv_Selection.Rows(I).Cells(10).Value
                        dgv_Details.Rows(n).Cells(18).Value = dgv_Selection.Rows(I).Cells(19).Value
                        dgv_Details.Rows(n).Cells(19).Value = dgv_Selection.Rows(I).Cells(20).Value
                        dgv_Details.Rows(n).Cells(20).Value = dgv_Selection.Rows(I).Cells(21).Value
                        dgv_Details.Rows(n).Cells(21).Value = dgv_Selection.Rows(I).Cells(22).Value
                        dgv_Details.Rows(n).Cells(22).Value = dgv_Selection.Rows(I).Cells(23).Value
                        dgv_Details.Rows(n).Cells(23).Value = dgv_Selection.Rows(I).Cells(24).Value

                        If Trim(dgv_Details.Rows(n).Cells(18).Value) <> "" Or Trim(dgv_Details.Rows(n).Cells(19).Value) <> "" Or Trim(dgv_Details.Rows(n).Cells(22).Value) <> "" Then
                            For J = 0 To dgv_Details.ColumnCount - 1
                                dgv_Details.Rows(n).Cells(J).Style.BackColor = Color.LightGray
                                dgv_Details.Rows(n).Cells(J).Style.ForeColor = Color.Red
                            Next
                        End If


                    End If
                    Dt1.Clear()

                End If

            Next I

            Total_Calculation()

            Pnl_Back.Enabled = True
            pnl_Selection.Visible = False
            If dgv_Details.Rows.Count > 0 Then
                dgv_Details.Focus()
                dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(7)
                dgv_Details.CurrentCell.Selected = True
            Else
                txt_Note.Focus()
            End If
            'If txt_Note.Enabled And txt_Note.Visible Then txt_Note.Focus()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT SELECT PIECE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

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
        Dim prtFrm As Single = 0
        Dim prtTo As Single = 0
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim Condt As String = ""
        Dim PpSzSTS As Boolean = False
        'Dim ps As Printing.PaperSize
        Dim NewCode As String

        NewCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)
        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.PrintEntry, Common_Procedures.UR.ClothSales_Buyer_offer_Entry, New_Entry) = False Then Exit Sub

        Try

            da1 = New SqlClient.SqlDataAdapter("select a.* from Buyer_Offer_Head a Where a.Buyer_Offer_Code = '" & Trim(NewCode) & "'", con)
            dt1 = New DataTable
            da1.Fill(dt1)

            If dt1.Rows.Count <= 0 Then

                MessageBox.Show("No Entry Found", "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)
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

    Private Sub dgtxt_Details_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgtxt_Details.KeyUp
        If e.Control = True And UCase(Chr(e.KeyCode)) = "D" Then
            dgv_Details_KeyUp(sender, e)
        End If
    End Sub

    Private Sub cbo_PartyName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_PartyName.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "(AccountsGroup_IdNo IN (select tZ1.AccountsGroup_IdNo from AccountsGroup_Head tZ1 Where tZ1.Parent_Idno LIKE '%~10~4~%' or tZ1.Parent_Idno LIKE '%~14~11~%' ) or Show_In_All_Entry = 1)", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_PartyName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_PartyName.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_PartyName, msk_date, txt_BuyerRefNo, "Ledger_AlaisHead", "Ledger_DisplayName", "( AccountsGroup_IdNo IN (select tZ1.AccountsGroup_IdNo from AccountsGroup_Head tZ1 Where tZ1.Parent_Idno LIKE '%~10~4~%' or tZ1.Parent_Idno LIKE '%~14~11~%' ) or Show_In_All_Entry = 1 )", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_PartyName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_PartyName.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_PartyName, txt_BuyerRefNo, "Ledger_AlaisHead", "Ledger_DisplayName", "( AccountsGroup_IdNo IN (select tZ1.AccountsGroup_IdNo from AccountsGroup_Head tZ1 Where tZ1.Parent_Idno LIKE '%~10~4~%' or tZ1.Parent_Idno LIKE '%~14~11~%' ) or Show_In_All_Entry = 1 )", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_PartyName_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_PartyName.KeyUp
        If e.Control = False And e.KeyValue = 17 Then

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

    Private Sub txt_PcsSelction_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_PcsSelction.KeyDown
        If e.KeyValue = 40 Then
            If dgv_Selection.Rows.Count > 0 Then
                dgv_Selection.Focus()
                dgv_Selection.CurrentCell = dgv_Selection.Rows(0).Cells(0)
                dgv_Selection.CurrentCell.Selected = True
            End If
        End If
        If (e.KeyValue = 38) Then txt_LotSelction.Focus()

    End Sub

    Private Sub txt_PcsSelction_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_PcsSelction.KeyPress
        If Asc(e.KeyChar) = 13 Then

            If Trim(txt_PcsSelction.Text) <> "" Or Trim(txt_PcsSelction.Text) <> "" Then
                btn_lot_Pcs_selection_Click(sender, e)

            Else
                If dgv_Selection.Rows.Count > 0 Then
                    dgv_Selection.Focus()
                    dgv_Selection.CurrentCell = dgv_Selection.Rows(0).Cells(0)
                    dgv_Selection.CurrentCell.Selected = True
                End If

            End If

        End If
    End Sub

    Private Sub txt_LotSelction_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_LotSelction.KeyDown
        If (e.KeyValue = 40) Then
            txt_PcsSelction.Focus()
        End If
    End Sub

    Private Sub txt_LotSelction_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_LotSelction.KeyPress
        If Asc(e.KeyChar) = 13 Then
            txt_PcsSelction.Focus()
        End If
    End Sub

    Private Sub btn_lot_Pcs_selection_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_lot_Pcs_selection.Click
        Dim LtNo As String
        Dim PcsNo As String
        Dim i As Integer

        If Trim(txt_LotSelction.Text) <> "" Or Trim(txt_PcsSelction.Text) <> "" Then

            LtNo = Trim(txt_LotSelction.Text)
            PcsNo = Trim(txt_PcsSelction.Text)

            For i = 0 To dgv_Selection.Rows.Count - 1
                If Trim(UCase(LtNo)) = Trim(UCase(dgv_Selection.Rows(i).Cells(1).Value)) And Trim(UCase(PcsNo)) = Trim(UCase(dgv_Selection.Rows(i).Cells(2).Value)) Then
                    Call Select_Piece(i)
                    dgv_Selection.CurrentCell = dgv_Selection.Rows(i).Cells(0)
                    If i >= 9 Then dgv_Selection.FirstDisplayedScrollingRowIndex = i - 8

                    Exit For
                End If
            Next

            txt_LotSelction.Text = ""
            txt_PcsSelction.Text = ""
            If txt_LotSelction.Enabled = True Then txt_LotSelction.Focus()

        End If
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

    Private Sub msk_date_KeyPress(sender As Object, e As System.Windows.Forms.KeyPressEventArgs) Handles msk_date.KeyPress
        If Trim(UCase(e.KeyChar)) = "D" Then
            msk_date.Text = Date.Today
            msk_date.SelectionStart = 0
        End If
    End Sub

    Private Sub msk_Date_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles msk_date.KeyUp
        'If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
        '    msk_date.Text = Date.Today
        'End If
        If IsDate(msk_date.Text) = True Then
            If e.KeyCode = 107 Then
                msk_date.Text = DateAdd("D", 1, Convert.ToDateTime(msk_date.Text))
            ElseIf e.KeyCode = 109 Then
                msk_date.Text = DateAdd("D", -1, Convert.ToDateTime(msk_date.Text))
            End If
        End If
        If e.KeyCode = 46 Or e.KeyCode = 8 Then

            Common_Procedures.maskEdit_Date_ON_DelBackSpace(sender, e, vmskOldText, vmskSelStrt)
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
        vmskOldText = ""
        vmskSelStrt = -1
        If e.KeyCode = 46 Or e.KeyCode = 8 Then
            vmskOldText = msk_date.Text
            vmskSelStrt = msk_date.SelectionStart
        End If

    End Sub

    Private Sub dgtxt_Details_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgtxt_Details.TextChanged
        Try
            With dgv_Details
                If .Rows.Count <> 0 Then
                    .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(dgtxt_Details.Text)
                End If
            End With

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub PrintDocument1_BeginPrint(ByVal sender As Object, ByVal e As System.Drawing.Printing.PrintEventArgs) Handles PrintDocument1.BeginPrint
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim NewCode As String = ""

        prn_HdDt.Clear()
        prn_DetDt.Clear()

        prn_PageNo = 0
        prn_HdIndx = 0
        prn_DetIndx = 0
        prn_HdMxIndx = 0
        prn_DetMxIndx = 0
        prn_Count = 1
        Erase prn_DetAr
        Erase prn_HdAr

        prn_HdAr = New String(100, 10) {}

        prn_DetAr = New String(100, 50, 10) {}

        NewCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Try

            da1 = New SqlClient.SqlDataAdapter("select a.*, tZ.*, c.Cloth_Name ,  E.* from Buyer_Offer_Head a INNER JOIN Company_head tZ ON a.company_idno = tZ.Company_Idno INNER JOIN Cloth_Head c ON a.Cloth_IdNo = c.Cloth_IdNo LEFT OUTER JOIN LEDGER_Head E ON A.Ledger_IdNo = E.Ledger_IdNo Where a.Buyer_Offer_Code = '" & Trim(NewCode) & "' Order by a.Buyer_Offer_Date, a.for_OrderBy, a.Buyer_Offer_No, a.Buyer_Offer_Code", con)
            prn_HdDt = New DataTable
            da1.Fill(prn_HdDt)

            If prn_HdDt.Rows.Count > 0 Then

                da2 = New SqlClient.SqlDataAdapter("select a.* from Buyer_Offer_Details a where a.Buyer_Offer_Code = '" & Trim(NewCode) & "'  order by a.Sl_No", con)
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

    Private Sub PrintDocument1_PrintPage(ByVal sender As System.Object, ByVal e As System.Drawing.Printing.PrintPageEventArgs) Handles PrintDocument1.PrintPage
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

        NoofItems_PerPage = 30

        Erase LnAr
        Erase ClAr

        LnAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        ClAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}

        ClAr(1) = Val(65) : ClAr(2) = 65 : ClAr(3) = 70 : ClAr(4) = 65 : ClAr(5) = 90 : ClAr(6) = 90 : ClAr(7) = 95 : ClAr(8) = 70 : ClAr(9) = 70
        ClAr(10) = PageWidth - (LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9))

        TxtHgt = 18.5

        EntryCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

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

                        CurY = CurY + TxtHgt
                        Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetDt.Rows(prn_DetIndx).Item("Lot_No").ToString), LMargin + 15, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetDt.Rows(prn_DetIndx).Item("Pcs_NO").ToString), LMargin + ClAr(1) + 10, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Meters").ToString), " #######0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) - 5, CurY, 1, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Party_PieceNo").ToString, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + 10, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Pass_Meters").ToString), " #######0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 1, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("lESS_Meters").ToString), " #######0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY, 1, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Reject_Meters").ToString) + Val(prn_DetDt.Rows(prn_DetIndx).Item("Bits_Meters").ToString), " #######0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Points").ToString), " #######0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 10, CurY, 1, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Point_Per_PassMeter").ToString), " #######0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) - 10, CurY, 1, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetDt.Rows(prn_DetIndx).Item("Grade").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + 10, CurY, 0, 0, pFont)

                        NoofDets = NoofDets + 1

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
        Dim dt3 As New DataTable
        Dim dt4 As New DataTable
        Dim p1Font As Font
        Dim strHeight As Single
        Dim Cmp_Add As String = ""
        Dim C1 As Single, W1, W2 As Single, S1, S2 As Single
        Dim Cmp_Name, Desc As String, Cmp_Add1 As String, Cmp_Add2 As String
        Dim Cmp_PhNo As String, Cmp_TinNo As String, Cmp_CstNo As String, Cmp_EMail As String

        PageNo = PageNo + 1

        CurY = TMargin + 30

        'da2 = New SqlClient.SqlDataAdapter("select a.*, b.Ledger_Name, c.Ledger_Name, d.Company_Description as Transport_Name from ClothSales_Invoice_Head a  INNER JOIN Ledger_Head b ON b.Ledger_IdNo = a.Ledger_Idno LEFT OUTER JOIN Ledger_Head c ON c.Ledger_IdNo = a.Transport_IdNo LEFT OUTER JOIN Company_Head d ON d.Company_IdNo = a.Company_IdNo where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.ClothSales_Invoice_Code = '" & Trim(EntryCode) & "' Order by a.For_OrderBy", con)
        'da2.Fill(dt2)
        'If dt2.Rows.Count > NoofItems_PerPage Then
        '    Common_Procedures.Print_To_PrintDocument(e, "Page : " & Trim(Val(PageNo)), PageWidth - 10, CurY - TxtHgt, 1, 0, pFont)
        'End If
        'dt2.Clear()

        prn_Count = prn_Count + 1

        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(1) = CurY
        Desc = ""
        Cmp_Name = "" : Cmp_Add1 = "" : Cmp_Add2 = ""
        Cmp_PhNo = "" : Cmp_TinNo = "" : Cmp_CstNo = "" : Cmp_EMail = ""

        Desc = prn_HdDt.Rows(0).Item("Company_Description").ToString
        Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString
        Cmp_Add1 = prn_HdDt.Rows(0).Item("Company_Address1").ToString & " " & prn_HdDt.Rows(0).Item("Company_Address2").ToString
        Cmp_Add2 = prn_HdDt.Rows(0).Item("Company_Address3").ToString & " " & prn_HdDt.Rows(0).Item("Company_Address4").ToString

        If Trim(prn_HdDt.Rows(0).Item("Company_PhoneNo").ToString) <> "" Then
            Cmp_PhNo = "PHONE : " & prn_HdDt.Rows(0).Item("Company_PhoneNo").ToString
        End If
        If Trim(prn_HdDt.Rows(0).Item("Company_TinNo").ToString) <> "" Then
            Cmp_TinNo = "TIN NO.: " & prn_HdDt.Rows(0).Item("Company_TinNo").ToString
        End If
        If Trim(prn_HdDt.Rows(0).Item("Company_CstNo").ToString) <> "" Then
            Cmp_CstNo = "CST NO.: " & prn_HdDt.Rows(0).Item("Company_CstNo").ToString
        End If
        If Trim(prn_HdDt.Rows(0).Item("Company_EMail").ToString) <> "" Then
            Cmp_EMail = "MAIL ID : " & prn_HdDt.Rows(0).Item("Company_EMail").ToString
        End If

        p1Font = New Font("Calibri", 15, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "BUYER OFFER", LMargin, CurY, 2, PrintWidth, p1Font)

        CurY = CurY + TxtHgt
        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height


        'If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1018" Then '---- M.K Textiles (Palladam)
        '    e.Graphics.DrawImage(DirectCast(Global.Textile.My.Resources.Resources.Company_Logo_MK, Drawing.Image), LMargin + 20, CurY, 112, 80)
        '    'e.Graphics.DrawImage(DirectCast(Global.Textile.My.Resources.Resources.Company_Logo_MK_2, Drawing.Image), LMargin + 20, CurY, 115, 80)
        '    'e.Graphics.DrawImage(DirectCast(Global.Textile.My.Resources.Resources.Company_Logo_MK, Drawing.Image), LMargin + 20, CurY, 75, 75)
        'End If

        p1Font = New Font("Calibri", 18, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Name, LMargin, CurY, 2, PrintWidth, p1Font)
        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height


        CurY = CurY + strHeight - 1
        Common_Procedures.Print_To_PrintDocument(e, Desc, LMargin, CurY, 2, PrintWidth, pFont)

        CurY = CurY + TxtHgt - 1
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Add1, LMargin, CurY, 2, PrintWidth, pFont)

        CurY = CurY + TxtHgt - 1
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Add2, LMargin, CurY, 2, PrintWidth, pFont)
        CurY = CurY + TxtHgt - 1
        Common_Procedures.Print_To_PrintDocument(e, Cmp_PhNo, LMargin, CurY, 2, PrintWidth, pFont)
        CurY = CurY + TxtHgt - 1
        Common_Procedures.Print_To_PrintDocument(e, Cmp_TinNo, LMargin + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, Cmp_EMail, LMargin, CurY, 2, PrintWidth, pFont)
        Common_Procedures.Print_To_PrintDocument(e, Cmp_CstNo, PageWidth - 10, CurY, 1, 0, pFont)

        CurY = CurY + TxtHgt + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(2) = CurY

        C1 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5)
        W1 = e.Graphics.MeasureString("INVOICE DATE  : ", pFont).Width
        S1 = e.Graphics.MeasureString("TO     :    ", pFont).Width
        W2 = e.Graphics.MeasureString("Despatch To   : ", pFont).Width
        S2 = e.Graphics.MeasureString("Sent Through  : ", pFont).Width


        CurY = CurY + 10
        p1Font = New Font("Calibri", 12, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "BUYER  :  " & "M/s." & prn_HdDt.Rows(0).Item("Ledger_Name").ToString, LMargin + 10, CurY, 0, 0, p1Font)
        Common_Procedures.Print_To_PrintDocument(e, "NO", LMargin + C1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Buyer_offer_No").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, p1Font)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address1").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address2").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
        'Common_Procedures.Print_To_PrintDocument(e, "ORDER NO", LMargin + C1 + 10, CurY, 0, 0, pFont)
        'Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
        'Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Party_OrderNo").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, p1Font)
        p1Font = New Font("Calibri", 14, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "DATE", LMargin + C1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Buyer_offer_Date").ToString), "dd-MM-yyyy").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, pFont)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address3").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)


        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address4").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
        'If Trim(prn_HdDt.Rows(0).Item("Dc_No").ToString) <> "" Then
        '    Common_Procedures.Print_To_PrintDocument(e, "DC NO : " & prn_HdDt.Rows(0).Item("Dc_No").ToString, LMargin + C1 + 10, CurY, 0, 0, pFont)
        'End If
        'If Trim(prn_HdDt.Rows(0).Item("Dc_Date").ToString) <> "" Then
        '    Common_Procedures.Print_To_PrintDocument(e, "DC DATE : " & prn_HdDt.Rows(0).Item("Dc_Date").ToString, LMargin + C1 + 100, CurY, 0, 0, pFont)
        'End If

        CurY = CurY + TxtHgt
        If Trim(prn_HdDt.Rows(0).Item("Ledger_TinNo").ToString) <> "" Then
            Common_Procedures.Print_To_PrintDocument(e, " TIN : " & prn_HdDt.Rows(0).Item("Ledger_TinNo").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
        End If


        CurY = CurY + TxtHgt
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        e.Graphics.DrawLine(Pens.Black, LMargin + C1, CurY, LMargin + C1, LnAr(2))

        Try

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "QUALITY", LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("cloth_nAME").ToString, LMargin + W1 + 25, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt + 10
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(2) = CurY

            CurY = CurY + TxtHgt - 10
            Common_Procedures.Print_To_PrintDocument(e, "LOT NO", LMargin, CurY, 2, ClAr(1), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "PCS NO", LMargin + ClAr(1), CurY, 2, ClAr(2), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "METERS", LMargin + ClAr(1) + ClAr(2), CurY, 2, ClAr(3), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "P.PCS.NO", LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, 2, ClAr(4), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "PASS METER", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 2, ClAr(5), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "LESS METER", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, 2, ClAr(6), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "REJECT METER", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, 2, ClAr(7), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "POINTS", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, 2, ClAr(8), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "PTS/PASS", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, 2, ClAr(9), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "GRADE", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9), CurY, 2, ClAr(10), pFont)

            CurY = CurY + TxtHgt + 10
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(3) = CurY

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub Printing_Format1_PageFooter(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single, ByVal NoofDets As Integer, ByVal is_LastPage As Boolean)
        Dim i As Integer
        Dim p1Font As Font
        Dim W1, W2 As Single
        Dim C1 As Single

        C1 = ClAr(1) + ClAr(2) + ClAr(3)
        For i = NoofDets + 1 To NoofItems_PerPage
            CurY = CurY + TxtHgt
        Next

        W1 = e.Graphics.MeasureString("Empty Gunnies  :", pFont).Width
        W2 = e.Graphics.MeasureString("Empty Cones  :", pFont).Width

        CurY = CurY + TxtHgt
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(5) = CurY

        CurY = CurY + TxtHgt - 10
        If is_LastPage = True Then
            Common_Procedures.Print_To_PrintDocument(e, " TOTAL", LMargin + 5, CurY, 2, ClAr(4), pFont)

            Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(0).Item("Pcs").ToString), LMargin + ClAr(1) + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Meters").ToString), " #######0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) - 5, CurY, 1, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Total_Passed_Meters").ToString), " #######0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 1, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Total_Less_Meters").ToString), " #######0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY, 1, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Total_Reject_Meters").ToString) + Val(prn_HdDt.Rows(0).Item("Total_Bits_Meters").ToString), " #######0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Total_Points").ToString), " #######0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 10, CurY, 1, 0, pFont)

        End If

        CurY = CurY + TxtHgt + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(6) = CurY

        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1), CurY, LMargin + ClAr(1), LnAr(2))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2), CurY, LMargin + ClAr(1) + ClAr(2), LnAr(2))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3), LnAr(2))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), LnAr(2))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(2))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), LnAr(2))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), LnAr(2))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), LnAr(2))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9), LnAr(2))


        CurY = CurY + TxtHgt - 10

        p1Font = New Font("Calibri", 12, FontStyle.Bold)

        Common_Procedures.Print_To_PrintDocument(e, "For " & Trim(prn_HdDt.Rows(0).Item("Company_Name").ToString), PageWidth - 15, CurY, 1, 0, p1Font)
        CurY = CurY + TxtHgt
        CurY = CurY + TxtHgt
        CurY = CurY + TxtHgt

        p1Font = New Font("Calibri", 12, FontStyle.Bold)

        Common_Procedures.Print_To_PrintDocument(e, "AUTHORISED SIGNATORY ", PageWidth - 5, CurY, 1, 0, pFont)
        CurY = CurY + TxtHgt + 10

        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        e.Graphics.DrawLine(Pens.Black, LMargin, LnAr(1), LMargin, CurY)
        e.Graphics.DrawLine(Pens.Black, PageWidth, LnAr(1), PageWidth, CurY)

    End Sub

    Private Sub txt_Filter_PcsNo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Filter_PcsNo.KeyPress
        If Asc(e.KeyChar) = 13 Then
            e.Handled = True
            btn_Filter_Show.Focus()
            btn_Filter_Show_Click(sender, e)
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