Public Class Piece_Joining
    Implements Interface_MDIActions

    Private con As New SqlClient.SqlConnection(Common_Procedures.Connection_String)
    Private con1 As New SqlClient.SqlConnection(Common_Procedures.ConnectionString_CompanyGroupdetails)
    Private FrmLdSTS As Boolean = False
    Private New_Entry As Boolean = False
    Private Insert_Entry As Boolean = False
    Private Filter_Status As Boolean = False
    Private Pk_Condition As String = "PCSJN-"
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

    Private SaveAll_STS As Boolean = False
    Private LastNo As String = ""

    Private Sub clear()

        NoCalc_Status = True

        New_Entry = False
        Insert_Entry = False

        Pnl_Back.Enabled = True
        pnl_Filter.Visible = False
        pnl_Selection.Visible = False

        lbl_RefNo.Text = ""
        lbl_RefNo.ForeColor = Color.Black

        msk_date.Text = ""
        dtp_Date.Text = ""
        cbo_PartyName.Text = Common_Procedures.Ledger_IdNoToName(con, Val(Common_Procedures.CommonLedger.Godown_Ac))
        cbo_Cloth.Text = ""
        cbo_ClothType.Text = Common_Procedures.ClothType_IdNoToName(con, 1)

        txt_LotSelction.Text = ""
        txt_PcsSelction.Text = ""
        lbl_UserName.Text = "USER : " & Trim(UCase(Common_Procedures.User_IdNoToName(con1, Common_Procedures.User.IdNo)))

        txt_Folding.Text = 100
        txt_Note.Text = ""
        txt_NewLotNo.Text = ""
        txt_NewPieceNo.Text = ""
        chk_SelectAll.Checked = False

        dgv_Details.Rows.Clear()
        dgv_Details_Total.Rows.Clear()
        dgv_Details_Total.Rows.Add()


        If Filter_Status = False Then
            dtp_Filter_Fromdate.Text = ""
            dtp_Filter_ToDate.Text = ""
            cbo_Filter_Cloth.Text = ""
            cbo_Filter_ClothType.Text = ""
            txt_Filter_LotNo.Text = ""
            txt_Filter_PcsNo.Text = ""
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

        txt_NewLotNo.Enabled = True
        txt_NewLotNo.BackColor = Color.White

        txt_NewPieceNo.Enabled = True
        txt_NewPieceNo.BackColor = Color.White


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

    Private Sub Piece_Excess_Short_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated
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

    Private Sub Piece_Excess_Short_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        On Error Resume Next
        con.Close()
        con.Dispose()
        Common_Procedures.Last_Closed_FormName = Me.Name
    End Sub

    Private Sub Piece_Excess_Short_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        Try
            If Asc(e.KeyChar) = 27 Then

                If pnl_Filter.Visible = True Then
                    btn_Filter_Close_Click(sender, e)
                    Exit Sub

                ElseIf pnl_Selection.Visible = True Then
                    btn_Close_Selection_Click(sender, e)
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

    Private Sub Piece_Excess_Short_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim dt3 As New DataTable

        dgv_Details.Columns(1).HeaderText = Trim(UCase(Common_Procedures.settings.ClothReceipt_LotNo_OR_RollNo_Text))
        dgv_Selection.Columns(1).HeaderText = Trim(UCase(Common_Procedures.settings.ClothReceipt_LotNo_OR_RollNo_Text))

        Me.Text = ""

        con.Open()

        da = New SqlClient.SqlDataAdapter("select Ledger_DisplayName from Ledger_AlaisHead where (Ledger_IdNo = 0 or Ledger_Type = 'GODOWN' or Ledger_Type = 'JOBWORKER' or Show_In_All_Entry = 1) order by Ledger_DisplayName", con)
        da.Fill(dt3)
        cbo_PartyName.DataSource = dt3
        cbo_PartyName.DisplayMember = "Ledger_DisplayName"

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


        pnl_Filter.Visible = False
        pnl_Filter.Left = (Me.Width - pnl_Filter.Width) \ 2
        pnl_Filter.Top = (Me.Height - pnl_Filter.Height) \ 2

        pnl_Selection.Visible = False
        pnl_Selection.Left = (Me.Width - pnl_Selection.Width) \ 2
        pnl_Selection.Top = (Me.Height - pnl_Selection.Height) \ 2
        pnl_Selection.BringToFront()

        btn_UserModification.Visible = False
        If Common_Procedures.settings.User_Modifications_Show_Status = 1 Then
            If Val(Common_Procedures.User.IdNo) = 1 Or Common_Procedures.User.Show_UserModification_Status = 1 Then
                btn_UserModification.Visible = True
            End If
        End If

        btn_SaveAll.Visible = False
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1087" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1234" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1267" Then
            btn_SaveAll.Visible = True
        End If

        AddHandler msk_date.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Cloth.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_ClothType.GotFocus, AddressOf ControlGotFocus

        AddHandler txt_Folding.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Note.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_NewLotNo.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_NewPieceNo.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Filter_Cloth.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Filter_ClothType.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Filter_LotNo.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Filter_PcsNo.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_PartyName.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_save.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_close.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_LotSelction.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_PcsSelction.GotFocus, AddressOf ControlGotFocus

        AddHandler cbo_PartyName.LostFocus, AddressOf ControlLostFocus
        AddHandler msk_date.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Cloth.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_ClothType.LostFocus, AddressOf ControlLostFocus

        AddHandler txt_Folding.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Note.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_NewLotNo.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_NewPieceNo.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Filter_Cloth.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Filter_ClothType.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Filter_LotNo.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Filter_PcsNo.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_save.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_close.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_LotSelction.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_PcsSelction.LostFocus, AddressOf ControlLostFocus

        AddHandler msk_date.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Filter_LotNo.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Filter_PcsNo.KeyDown, AddressOf TextBoxControlKeyDown

        AddHandler msk_date.KeyPress, AddressOf TextBoxControlKeyPress
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
            da1 = New SqlClient.SqlDataAdapter("select a.* from Piece_Joining_Head a Where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Piece_Joining_Code = '" & Trim(NewCode) & "'", con)
            dt1 = New DataTable
            da1.Fill(dt1)

            If dt1.Rows.Count > 0 Then

                lbl_RefNo.Text = dt1.Rows(0).Item("Piece_Joining_No").ToString
                dtp_Date.Text = dt1.Rows(0).Item("Piece_Joining_Date").ToString
                msk_date.Text = dtp_Date.Text
                cbo_PartyName.Text = Common_Procedures.Ledger_IdNoToName(con, Val(dt1.Rows(0).Item("Ledger_IdNo").ToString))
                cbo_Cloth.Text = Common_Procedures.Cloth_IdNoToName(con, Val(dt1.Rows(0).Item("Cloth_IdNo").ToString))
                cbo_ClothType.Text = Common_Procedures.ClothType_IdNoToName(con, Val(dt1.Rows(0).Item("ClothType_IdNo").ToString))

                txt_NewLotNo.Text = dt1.Rows(0).Item("New_LotNo").ToString
                txt_NewPieceNo.Text = dt1.Rows(0).Item("New_PieceNo").ToString
                txt_Note.Text = dt1.Rows(0).Item("Note").ToString
                txt_Folding.Text = Format(Val(dt1.Rows(0).Item("Folding").ToString), "########0.00")
                lbl_UserName.Text = "USER : " & Trim(UCase(Common_Procedures.User_IdNoToName(con1, Val(dt1.Rows(0).Item("User_IdNo").ToString))))

                da2 = New SqlClient.SqlDataAdapter("select a.*, b.clothtype_name, c.Ledger_Name, d.cloth_name from Piece_Joining_Details a INNER JOIN ClothType_Head b ON a.ClothType_IdNo <> 0 and a.ClothType_IdNo = b.ClothType_IdNo LEFT OUTER JOIN Ledger_Head c ON a.Party_Idno <> 0 and a.Party_Idno = c.Ledger_Idno LEFT OUTER JOIN Cloth_Head d ON a.Cloth_Idno <> 0 and a.Cloth_Idno = d.Cloth_Idno where a.Piece_Joining_Code = '" & Trim(NewCode) & "' Order by a.sl_no", con)
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

                    Next i

                End If

                With dgv_Details_Total

                    If .RowCount = 0 Then .Rows.Add()
                    .Rows(0).Cells(2).Value = Val(dt1.Rows(0).Item("Total_Pcs").ToString)
                    .Rows(0).Cells(4).Value = Format(Val(dt1.Rows(0).Item("Total_Meters").ToString), "########0.00")
                    .Rows(0).Cells(5).Value = Format(Val(dt1.Rows(0).Item("Total_Weight").ToString), "########0.000")

                End With
                dt2.Clear()

                lbl_BaleCode.Text = ""

                da2 = New SqlClient.SqlDataAdapter("select isnull(PackingSlip_Code_Type1,'') as BaleCode_Type1, isnull(PackingSlip_Code_Type2,'')  as BaleCode_Type2, isnull(PackingSlip_Code_Type3,'')  as BaleCode_Type3, isnull(PackingSlip_Code_Type4,'')  as BaleCode_Type4, isnull(PackingSlip_Code_Type5,'') as BaleCode_Type5,  isnull(BuyerOffer_Code_Type1,'')  as BuyerOfferCode_Type1, isnull(BuyerOffer_Code_Type2,'')  as BuyerOfferCode_Type2, isnull(BuyerOffer_Code_Type3,'')  as BuyerOfferCode_Type3,  isnull(BuyerOffer_Code_Type4, '')  as BuyerOfferCode_Type4,  isnull(BuyerOffer_Code_Type5, '')  as BuyerOfferCode_Type5 from Weaver_ClothReceipt_Piece_Details Where Weaver_Piece_Checking_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' and Weaver_ClothReceipt_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' and (PackingSlip_Code_Type1 <> '' or PackingSlip_Code_Type2 <> '' or PackingSlip_Code_Type3 <> '' or PackingSlip_Code_Type4 <> '' or PackingSlip_Code_Type5 <> '' or BuyerOffer_Code_Type1 <> '' or BuyerOffer_Code_Type2 <> '' or BuyerOffer_Code_Type3 <> '' or BuyerOffer_Code_Type4 <> '' or BuyerOffer_Code_Type5 <> '')", con)
                dt2 = New DataTable
                da2.Fill(dt2)
                If dt2.Rows.Count > 0 Then
                    lbl_BaleCode.Text = Trim(dt2.Rows(0).Item("BaleCode_Type1").ToString)
                    If Trim(lbl_BaleCode.Text) = "" Then
                        lbl_BaleCode.Text = Trim(dt2.Rows(0).Item("BaleCode_Type2").ToString)

                        If Trim(lbl_BaleCode.Text) = "" Then
                            lbl_BaleCode.Text = Trim(dt2.Rows(0).Item("BaleCode_Type3").ToString)

                            If Trim(lbl_BaleCode.Text) = "" Then
                                lbl_BaleCode.Text = Trim(dt2.Rows(0).Item("BaleCode_Type4").ToString)

                                If Trim(lbl_BaleCode.Text) = "" Then
                                    lbl_BaleCode.Text = Trim(dt2.Rows(0).Item("BaleCode_Type5").ToString)
                                End If
                            End If
                        End If
                    End If
                End If
                dt1.Clear()

                dt2.Dispose()
                da2.Dispose()

                If Trim(lbl_BaleCode.Text) <> "" Then

                    cbo_Cloth.Enabled = False
                    cbo_Cloth.BackColor = Color.LightGray

                    cbo_ClothType.Enabled = False
                    cbo_ClothType.BackColor = Color.LightGray

                    txt_Folding.Enabled = False
                    txt_Folding.BackColor = Color.LightGray

                    txt_NewLotNo.Enabled = False
                    txt_NewLotNo.BackColor = Color.LightGray

                    txt_NewPieceNo.Enabled = False
                    txt_NewPieceNo.BackColor = Color.LightGray

                    btn_Selection.Enabled = False

                End If

            End If

            dt1.Clear()
            dt1.Dispose()
            da1.Dispose()

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

        ' If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.PackinSlip_Entry, "~L~") = 0 And InStr(Common_Procedures.UR.PackinSlip_Entry, "~D~") = 0 Then MessageBox.Show("You have No Rights to Delete", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error) : Exit Sub

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)
        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.DeletingEntry, Common_Procedures.UR.Piece_Excess_Short_Entry, New_Entry, Me, con, "Piece_Joining_Head", "Piece_Joining_Code", NewCode, "Piece_Joining_Date", "(Piece_Joining_Code = '" & Trim(NewCode) & "')") = False Then Exit Sub


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

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Da = New SqlClient.SqlDataAdapter("select count(*) from Weaver_ClothReceipt_Piece_Details Where Weaver_Piece_Checking_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' and Weaver_ClothReceipt_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' and (PackingSlip_Code_Type1 <> '' or PackingSlip_Code_Type2 <> '' or PackingSlip_Code_Type3 <> '' or PackingSlip_Code_Type4 <> '' or PackingSlip_Code_Type5 <> '' or BuyerOffer_Code_Type1 <> '' or BuyerOffer_Code_Type2 <> '' or BuyerOffer_Code_Type3 <> '' or BuyerOffer_Code_Type4 <> '' or BuyerOffer_Code_Type5 <> '')", con)
        Dt1 = New DataTable
        Da.Fill(Dt1)
        If Dt1.Rows.Count > 0 Then
            If IsDBNull(Dt1.Rows(0)(0).ToString) = False Then
                If Val(Dt1.Rows(0)(0).ToString) <> 0 Then
                    MessageBox.Show("Packing Slip prepared", "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                    Exit Sub
                End If
            End If
        End If
        Dt1.Clear()

        trans = con.BeginTransaction

        Try

            cmd.Connection = con
            cmd.Transaction = trans


            Call Common_Procedures.User_Modification_Updation(con, "HEAD", Me.Name, "DELETE", "Piece_Joining_Head", "Piece_Joining_Code", Val(lbl_Company.Tag), NewCode, lbl_RefNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, True, "", "", "Piece_Joining_Code, Company_IdNo, for_OrderBy", trans)

            Call Common_Procedures.User_Modification_Updation(con, "DETAILS", Me.Name, "DELETE", "Piece_Joining_Details", "Piece_Joining_Code", Val(lbl_Company.Tag), NewCode, lbl_RefNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, True, "Lot_No, Pcs_No, ClothType_IdNo, Meters, Weight, Weight_Meter, Party_IdNo ,  Lot_Code", "Sl_No", "Piece_Joining_Code, For_OrderBy, Company_IdNo, Piece_Joining_No, Piece_Joining_Date, Ledger_Idno", trans)


            cmd.CommandText = "update Weaver_ClothReceipt_Piece_Details set PackingSlip_Code_Type1 = '', PackingSlip_Inc_Type1 = PackingSlip_Inc_Type1 - 1 Where PackingSlip_Code_Type1 = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "update Weaver_ClothReceipt_Piece_Details set PackingSlip_Code_Type2 = '', PackingSlip_Inc_Type2 = PackingSlip_Inc_Type2 - 1 Where PackingSlip_Code_Type2 = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "update Weaver_ClothReceipt_Piece_Details set PackingSlip_Code_Type3 = '', PackingSlip_Inc_Type3 = PackingSlip_Inc_Type3 - 1 Where PackingSlip_Code_Type3 = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "update Weaver_ClothReceipt_Piece_Details set PackingSlip_Code_Type4 = '', PackingSlip_Inc_Type4 = PackingSlip_Inc_Type4 - 1 Where PackingSlip_Code_Type4 = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "update Weaver_ClothReceipt_Piece_Details set PackingSlip_Code_Type5 = '', PackingSlip_Inc_Type5 = PackingSlip_Inc_Type5 - 1 Where PackingSlip_Code_Type5 = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete Weaver_ClothReceipt_Piece_Details Where Weaver_Piece_Checking_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' and Weaver_ClothReceipt_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' and (PackingSlip_Code_Type1 = '' and PackingSlip_Code_Type2 = '' and PackingSlip_Code_Type3 = '' and PackingSlip_Code_Type4 = '' and PackingSlip_Code_Type5 = '' and BuyerOffer_Code_Type1 = '' and BuyerOffer_Code_Type2 = '' and BuyerOffer_Code_Type3 = '' and BuyerOffer_Code_Type4 = '' and BuyerOffer_Code_Type5 = '')"
            cmd.ExecuteNonQuery()


            cmd.CommandText = "Delete from Stock_Cloth_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "delete from Piece_Joining_Details where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Piece_Joining_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()
            cmd.CommandText = "delete from Piece_Joining_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Piece_Joining_Code = '" & Trim(NewCode) & "'"
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


            dtp_Filter_Fromdate.Text = Common_Procedures.Company_FromDate
            dtp_Filter_ToDate.Text = Common_Procedures.Company_ToDate
            cbo_Filter_Cloth.Text = ""
            cbo_Filter_ClothType.Text = ""
            txt_Filter_LotNo.Text = ""
            txt_Filter_PcsNo.Text = ""

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

            da = New SqlClient.SqlDataAdapter("select top 1 Piece_Joining_No from Piece_Joining_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Piece_Joining_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby, Piece_Joining_No", con)
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
            MessageBox.Show(ex.Message, "FOR  MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Public Sub movenext_record() Implements Interface_MDIActions.movenext_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim movno As String = ""
        Dim OrdByNo As Single = 0

        Try

            OrdByNo = Common_Procedures.OrderBy_CodeToValue(Trim(lbl_RefNo.Text))

            da = New SqlClient.SqlDataAdapter("select top 1 Piece_Joining_No from Piece_Joining_Head where for_orderby > " & Str(Val(OrdByNo)) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and Piece_Joining_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby, Piece_Joining_No", con)
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
            MessageBox.Show(ex.Message, "FOR  MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Public Sub moveprevious_record() Implements Interface_MDIActions.moveprevious_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim movno As String = ""
        Dim OrdByNo As Single = 0

        Try

            OrdByNo = Common_Procedures.OrderBy_CodeToValue(Trim(lbl_RefNo.Text))

            da = New SqlClient.SqlDataAdapter("select top 1 Piece_Joining_No from Piece_Joining_Head where for_orderby < " & Str(Val(OrdByNo)) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and Piece_Joining_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, Piece_Joining_No desc", con)
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
            MessageBox.Show(ex.Message, "FOR  MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Public Sub movelast_record() Implements Interface_MDIActions.movelast_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim movno As String = ""

        Try
            da = New SqlClient.SqlDataAdapter("select top 1 Piece_Joining_No from Piece_Joining_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Piece_Joining_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, Piece_Joining_No desc", con)
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

            lbl_RefNo.Text = Common_Procedures.get_MaxCode(con, "Piece_Joining_Head", "Piece_Joining_Code", "For_OrderBy", "", Val(lbl_Company.Tag), Common_Procedures.FnYearCode)

            lbl_RefNo.ForeColor = Color.Red


            msk_date.Text = Date.Today.ToShortDateString
            da = New SqlClient.SqlDataAdapter("select top 1 * from Piece_Joining_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Piece_Joining_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, Piece_Joining_No desc", con)
            dt1 = New DataTable
            da.Fill(dt1)
            If dt1.Rows.Count > 0 Then
                If Val(Common_Procedures.settings.PreviousEntryDate_ByDefault) = 1 Then '---- M.S Textiles (Tirupur)
                    If dt1.Rows(0).Item("Piece_Joining_Date").ToString <> "" Then msk_date.Text = dt1.Rows(0).Item("Piece_Joining_Date").ToString
                End If

                cbo_PartyName.Text = Common_Procedures.Ledger_IdNoToName(con, Val(dt1.Rows(0).Item("Ledger_IdNo").ToString))
                cbo_Cloth.Text = Common_Procedures.Cloth_IdNoToName(con, Val(dt1.Rows(0).Item("Cloth_IdNo").ToString))
                cbo_ClothType.Text = Common_Procedures.ClothType_IdNoToName(con, Val(dt1.Rows(0).Item("ClothType_IdNo").ToString))


                txt_Folding.Text = Format(Val(dt1.Rows(0).Item("Folding").ToString), "########0.00")

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

            inpno = InputBox("Enter Ref.No.", "FOR FINDING...")

            RecCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

            Da = New SqlClient.SqlDataAdapter("select Piece_Joining_No from Piece_Joining_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Piece_Joining_Code = '" & Trim(RecCode) & "'", con)
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
                MessageBox.Show("Rec No. does not exists", "DOES NOT FIND...", MessageBoxButtons.OK, MessageBoxIcon.Error)

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

        '  If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.PackinSlip_Entry, "~L~") = 0 And InStr(Common_Procedures.UR.PackinSlip_Entry, "~I~") = 0 Then MessageBox.Show("You have No Rights to Insert", "DOES NOT INSERT...", MessageBoxButtons.OK, MessageBoxIcon.Error) : Exit Sub
        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.InsertingEntry, Common_Procedures.UR.Piece_Excess_Short_Entry, New_Entry, Me) = False Then Exit Sub

        Try

            inpno = InputBox("Enter New Ref No.", "FOR NEW REFNO INSERTION...")

            RecCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

            Da = New SqlClient.SqlDataAdapter("select Piece_Joining_No from Piece_Joining_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Piece_Joining_Code = '" & Trim(RecCode) & "' ", con)
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
                    MessageBox.Show("Invalid Ref No", "DOES NOT INSERT NEW REF NO...", MessageBoxButtons.OK, MessageBoxIcon.Error)

                Else
                    new_record()
                    Insert_Entry = True
                    lbl_RefNo.Text = Trim(UCase(inpno))

                End If

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT INSERT NEW Ref No...", MessageBoxButtons.OK, MessageBoxIcon.Error)

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
        Dim Nr As Integer = 0
        Dim vOrdByNo As String = ""
        Dim Delv_ID As Integer = 0
        Dim Rec_ID As Integer = 0
        Dim vNEW_LOOMNO As String = ""
        Dim vWgt_Mtr As String = 0
        Dim vNEW_LOTCODE As String = ""
        Dim vGdwnTo_IdNo As Integer = 0
        Dim vDUP_LMNO As String = ""
        Dim vLMNO As String = ""

        vOrdByNo = Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text)

        If Val(lbl_Company.Tag) = 0 Then
            MessageBox.Show("Invalid Company Selection", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
        End If

        'If Common_Procedures.UserRight_Check(Common_Procedures.UR.PackinSlip_Entry, New_Entry) = False Then Exit Sub

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)
        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.SavingEntry, Common_Procedures.UR.Piece_Excess_Short_Entry, New_Entry, Me, con, "Piece_Joining_Head", "Piece_Joining_Code", NewCode, "Piece_Joining_Date", "(Piece_Joining_Code = '" & Trim(NewCode) & "')", "(Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Piece_Joining_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "')", "for_Orderby desc, Piece_Joining_No desc", dtp_Date.Value.Date) = False Then Exit Sub

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

        lbl_UserName.Text = Common_Procedures.User.IdNo
        led_id = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_PartyName.Text)
        If led_id = 0 Then
            MessageBox.Show("Invalid Ledger Name", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If cbo_PartyName.Enabled Then cbo_PartyName.Focus()
            Exit Sub
        End If

        Clth_ID = Common_Procedures.Cloth_NameToIdNo(con, cbo_Cloth.Text)
        If Clth_ID = 0 Then
            MessageBox.Show("Invalid Cloth Name", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If cbo_Cloth.Enabled And cbo_Cloth.Visible Then cbo_Cloth.Focus()
            Exit Sub
        End If

        Clthty_ID = Common_Procedures.ClothType_NameToIdNo(con, cbo_ClothType.Text)
        If Clthty_ID = 0 Then
            MessageBox.Show("Invalid Cloth Type Name", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If cbo_ClothType.Enabled And cbo_ClothType.Visible Then cbo_ClothType.Focus()
            Exit Sub
        End If

        vTotMtrs = 0 : vTotPcs = 0 : vTotWgt = 0
        If dgv_Details_Total.RowCount > 0 Then

            vTotPcs = Val(dgv_Details_Total.Rows(0).Cells(2).Value())
            vTotMtrs = Val(dgv_Details_Total.Rows(0).Cells(4).Value())
            vTotWgt = Val(dgv_Details_Total.Rows(0).Cells(5).Value())

        End If


        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Da = New SqlClient.SqlDataAdapter("select count(*) from Weaver_ClothReceipt_Piece_Details Where Weaver_Piece_Checking_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' and Weaver_ClothReceipt_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' and (PackingSlip_Code_Type1 <> '' or PackingSlip_Code_Type2 <> '' or PackingSlip_Code_Type3 <> '' or PackingSlip_Code_Type4 <> '' or PackingSlip_Code_Type5 <> '' or BuyerOffer_Code_Type1 <> '' or BuyerOffer_Code_Type2 <> '' or BuyerOffer_Code_Type3 <> '' or BuyerOffer_Code_Type4 <> '' or BuyerOffer_Code_Type5 <> '')", con)
        Dt1 = New DataTable
        Da.Fill(Dt1)
        If Dt1.Rows.Count > 0 Then
            If IsDBNull(Dt1.Rows(0)(0).ToString) = False Then
                If Val(Dt1.Rows(0)(0).ToString) <> 0 Then
                    MessageBox.Show("Invalid Saving : Packing Slip prepared", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                    Exit Sub
                End If
            End If
        End If
        Dt1.Clear()


        tr = con.BeginTransaction

        Try

            If Insert_Entry = True Or New_Entry = False Then
                NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            Else

                lbl_RefNo.Text = Common_Procedures.get_MaxCode(con, "Piece_Joining_Head", "Piece_Joining_Code", "For_OrderBy", "", Val(lbl_Company.Tag), Common_Procedures.FnYearCode, tr)

                NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            End If

            cmd.Connection = con
            cmd.Transaction = tr

            cmd.Parameters.Clear()
            cmd.Parameters.AddWithValue("@RecDate", Convert.ToDateTime(msk_date.Text))

            vNEW_LOTCODE = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(txt_NewLotNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            If New_Entry = True Then

                cmd.CommandText = "Insert into Piece_Joining_Head (      Piece_Joining_Code ,                 Company_IdNo     ,            Piece_Joining_No   ,                               for_OrderBy                              , Piece_Joining_Date,           Cloth_IdNo      ,       ClothType_IdNo       ,                 Folding           ,            Total_Pcs     ,          Total_Meters      ,          Total_Weight     ,               Note           ,           Ledger_IdNo   ,               New_LotCode   ,               New_LotNo          ,             New_PieceNo            ,            User_IdNo           ) " &
                                    " Values                        ('" & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_RefNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text))) & ",      @RecDate     ,  " & Str(Val(Clth_ID)) & ", " & Str(Val(Clthty_ID)) & ", " & Str(Val(txt_Folding.Text)) & ", " & Str(Val(vTotPcs)) & ", " & Str(Val(vTotMtrs)) & " , " & Str(Val(vTotWgt)) & " , '" & Trim(txt_Note.Text) & "', " & Str(Val(led_id)) & ", '" & Trim(vNEW_LOTCODE) & "', '" & Trim(txt_NewLotNo.Text) & "', '" & Trim(txt_NewPieceNo.Text) & "', " & Val(lbl_UserName.Text) & " )"
                cmd.ExecuteNonQuery()

            Else

                Call Common_Procedures.User_Modification_Updation(con, "HEAD", Me.Name, "OLD", "Piece_Joining_Head", "Piece_Joining_Code", Val(lbl_Company.Tag), NewCode, lbl_RefNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, False, "", "", "Piece_Joining_Code, Company_IdNo, for_OrderBy", tr)

                Call Common_Procedures.User_Modification_Updation(con, "DETAILS", Me.Name, "OLD", "Piece_Joining_Details", "Piece_Joining_Code", Val(lbl_Company.Tag), NewCode, lbl_RefNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, False, "Lot_No, Pcs_No, ClothType_IdNo, Meters, Weight, Weight_Meter, Party_IdNo ,  Lot_Code", "Sl_No", "Piece_Joining_Code, For_OrderBy, Company_IdNo, Piece_Joining_No, Piece_Joining_Date, Ledger_Idno", tr)

                cmd.CommandText = "Update Piece_Joining_Head set Piece_Joining_Date = @RecDate, Cloth_IdNo = " & Str(Val(Clth_ID)) & " , ClothType_IdNo = " & Str(Val(Clthty_ID)) & " , Folding = " & Str(Val(txt_Folding.Text)) & ", Total_Pcs = " & Str(Val(vTotPcs)) & ", Total_Weight = " & Str(Val(vTotWgt)) & " , Total_Meters = " & Str(Val(vTotMtrs)) & " , Note = '" & Trim(txt_Note.Text) & "' , Ledger_IdNo = " & Str(Val(led_id)) & " , New_LotCode = '" & Trim(vNEW_LOTCODE) & "', New_LotNo = '" & Trim(txt_NewLotNo.Text) & "', New_PieceNo = '" & Trim(txt_NewPieceNo.Text) & "', User_IdNo = " & Val(lbl_UserName.Text) & " Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Piece_Joining_Code = '" & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()

                cmd.CommandText = "update Weaver_ClothReceipt_Piece_Details set PackingSlip_Code_Type1 = '', PackingSlip_Inc_Type1 = PackingSlip_Inc_Type1 - 1 Where PackingSlip_Code_Type1 = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()

                cmd.CommandText = "update Weaver_ClothReceipt_Piece_Details set PackingSlip_Code_Type2 = '', PackingSlip_Inc_Type2 = PackingSlip_Inc_Type2 - 1 Where PackingSlip_Code_Type2 = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()

                cmd.CommandText = "update Weaver_ClothReceipt_Piece_Details set PackingSlip_Code_Type3 = '', PackingSlip_Inc_Type3 = PackingSlip_Inc_Type3 - 1 Where PackingSlip_Code_Type3 = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
                Nr = cmd.ExecuteNonQuery()

                cmd.CommandText = "update Weaver_ClothReceipt_Piece_Details set PackingSlip_Code_Type4 = '', PackingSlip_Inc_Type4 = PackingSlip_Inc_Type4 - 1 Where PackingSlip_Code_Type4 = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()

                cmd.CommandText = "update Weaver_ClothReceipt_Piece_Details set PackingSlip_Code_Type5 = '', PackingSlip_Inc_Type5 = PackingSlip_Inc_Type5 - 1 Where PackingSlip_Code_Type5 = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()

                cmd.CommandText = "Delete Weaver_ClothReceipt_Piece_Details Where Weaver_Piece_Checking_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' and Weaver_ClothReceipt_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' and (PackingSlip_Code_Type1 = '' and PackingSlip_Code_Type2 = '' and PackingSlip_Code_Type3 = '' and PackingSlip_Code_Type4 = '' and PackingSlip_Code_Type5 = '' and BuyerOffer_Code_Type1 = '' and BuyerOffer_Code_Type2 = '' and BuyerOffer_Code_Type3 = '' and BuyerOffer_Code_Type4 = '' and BuyerOffer_Code_Type5 = '')"
                cmd.ExecuteNonQuery()


            End If
            Call Common_Procedures.User_Modification_Updation(con, "HEAD", Me.Name, "NEW", "Piece_Joining_Head", "Piece_Joining_Code", Val(lbl_Company.Tag), NewCode, lbl_RefNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, False, "", "", "Piece_Joining_Code, Company_IdNo, for_OrderBy", tr)



            EntID = Trim(Pk_Condition) & Trim(lbl_RefNo.Text)
            If Trim(lbl_RefNo.Text) <> "" Then
                Partcls = "Pcs Join : Ref.No. " & Trim(lbl_RefNo.Text)
            End If
            PBlNo = Trim(lbl_RefNo.Text)

            Delv_ID = 0
            Rec_ID = 0
            Rec_ID = Val(Common_Procedures.CommonLedger.Godown_Ac)
            Delv_ID = 0 ' Val(Common_Procedures.CommonLedger.Godown_Ac)


            cmd.CommandText = "Delete from Piece_Joining_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Piece_Joining_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Stock_Cloth_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            With dgv_Details
                Sno = 0
                vNEW_LOOMNO = ""

                For i = 0 To .RowCount - 1

                    If Val(.Rows(i).Cells(4).Value) <> 0 Then

                        dCloTyp_ID = Common_Procedures.ClothType_NameToIdNo(con, .Rows(i).Cells(3).Value, tr)
                        party_ID = Common_Procedures.Ledger_AlaisNameToIdNo(con, .Rows(i).Cells(7).Value, tr)
                        dClo_ID = Common_Procedures.Cloth_NameToIdNo(con, .Rows(i).Cells(9).Value, tr)


                        Da = New SqlClient.SqlDataAdapter("Select a.warehouse_idno, a.Loom_No, a.Loom_IdNo, b.Loom_Name from Weaver_ClothReceipt_Piece_Details a LEFT OUTER JOIN Loom_Head b ON a.Loom_IdNo = b.Loom_IdNo Where a.lot_code = '" & Trim(dgv_Details.Rows(i).Cells(8).Value) & "' and a.piece_no = '" & Trim(dgv_Details.Rows(i).Cells(2).Value) & "'", con)
                        Da.SelectCommand.Transaction = tr
                        Dt1 = New DataTable
                        Da.Fill(Dt1)
                        If Dt1.Rows.Count > 0 Then
                            If IsDBNull(Dt1.Rows(0).Item("warehouse_idno").ToString) = False Then
                                If Val(Dt1.Rows(0).Item("warehouse_idno").ToString) <> 0 Then
                                    vGdwnTo_IdNo = Val(Dt1.Rows(0).Item("warehouse_idno").ToString)
                                End If
                            End If

                            For k = 0 To Dt1.Rows.Count - 1

                                vLMNO = ""

                                If IsDBNull(Dt1.Rows(k).Item("Loom_IdNo").ToString) = False Then
                                    If Val(Dt1.Rows(k).Item("Loom_IdNo").ToString) <> 0 Then
                                        vLMNO = Trim(Dt1.Rows(k).Item("Loom_Name").ToString)
                                    End If
                                End If
                                If Trim(vLMNO) = "" Then
                                    If IsDBNull(Dt1.Rows(k).Item("Loom_No").ToString) = False Then
                                        vLMNO = Trim(Dt1.Rows(k).Item("Loom_No").ToString)
                                    End If
                                End If


                            Next k

                            If Trim(vLMNO) <> "" Then
                                If InStr(1, Trim(UCase(vDUP_LMNO)), "~" & Trim(UCase(vLMNO)) & "~") = 0 Then
                                    vDUP_LMNO = Trim(vDUP_LMNO) & "~" & Trim(vLMNO) & "~"
                                    vNEW_LOOMNO = Trim(vNEW_LOOMNO) & IIf(Trim(vNEW_LOOMNO) <> "", ", ", "") & Trim(vLMNO)
                                End If
                            End If

                        End If
                        Dt1.Clear()


                        Sno = Sno + 1
                        cmd.CommandText = "Insert into Piece_Joining_Details(Piece_Joining_Code, Company_IdNo, Piece_Joining_No, for_OrderBy, Piece_Joining_Date,  Cloth_IdNo, Folding, Sl_No,   Lot_No, Pcs_No, ClothType_IdNo, Meters, Weight, Weight_Meter, Party_IdNo ,  Lot_Code ) Values ('" & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_RefNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text))) & ", @RecDate, " & Str(Val(dClo_ID)) & ",  " & Str(Val(txt_Folding.Text)) & ",  " & Str(Val(Sno)) & ",  '" & Trim(.Rows(i).Cells(1).Value) & "', '" & Trim(.Rows(i).Cells(2).Value) & "', " & Str(Val(dCloTyp_ID)) & ", " & Str(Val(.Rows(i).Cells(4).Value)) & ", " & Str(Val(.Rows(i).Cells(5).Value)) & ", " & Str(Val(.Rows(i).Cells(6).Value)) & ", " & Str(Val(party_ID)) & " ,'" & Trim(.Rows(i).Cells(8).Value) & "' )"
                        cmd.ExecuteNonQuery()

                        Nr = 0
                        If dCloTyp_ID = 1 Then
                            cmd.CommandText = "update Weaver_ClothReceipt_Piece_Details set PackingSlip_Code_Type1 = '" & Trim(Pk_Condition) & Trim(NewCode) & "', PackingSlip_Inc_Type1 = PackingSlip_Inc_Type1 + 1 Where lot_code = '" & Trim(.Rows(i).Cells(8).Value) & "' and Piece_No = '" & Trim(.Rows(i).Cells(2).Value) & "'"
                            Nr = cmd.ExecuteNonQuery()

                        ElseIf dCloTyp_ID = 2 Then
                            cmd.CommandText = "update Weaver_ClothReceipt_Piece_Details set PackingSlip_Code_Type2 = '" & Trim(Pk_Condition) & Trim(NewCode) & "', PackingSlip_Inc_Type2 = PackingSlip_Inc_Type2 + 1 Where lot_code = '" & Trim(.Rows(i).Cells(8).Value) & "' and Piece_No = '" & Trim(.Rows(i).Cells(2).Value) & "'"
                            Nr = cmd.ExecuteNonQuery()

                        ElseIf dCloTyp_ID = 3 Then
                            cmd.CommandText = "update Weaver_ClothReceipt_Piece_Details set PackingSlip_Code_Type3 = '" & Trim(Pk_Condition) & Trim(NewCode) & "', PackingSlip_Inc_Type3 = PackingSlip_Inc_Type3 + 1 Where lot_code = '" & Trim(.Rows(i).Cells(8).Value) & "' and Piece_No = '" & Trim(.Rows(i).Cells(2).Value) & "'"
                            Nr = cmd.ExecuteNonQuery()

                        ElseIf dCloTyp_ID = 4 Then
                            cmd.CommandText = "update Weaver_ClothReceipt_Piece_Details set PackingSlip_Code_Type4 = '" & Trim(Pk_Condition) & Trim(NewCode) & "', PackingSlip_Inc_Type4 = PackingSlip_Inc_Type4 + 1 Where lot_code = '" & Trim(.Rows(i).Cells(8).Value) & "' and Piece_No = '" & Trim(.Rows(i).Cells(2).Value) & "'"
                            Nr = cmd.ExecuteNonQuery()

                        ElseIf dCloTyp_ID = 5 Then
                            cmd.CommandText = "update Weaver_ClothReceipt_Piece_Details set PackingSlip_Code_Type5 = '" & Trim(Pk_Condition) & Trim(NewCode) & "', PackingSlip_Inc_Type5 = PackingSlip_Inc_Type5 + 1 Where lot_code = '" & Trim(.Rows(i).Cells(8).Value) & "' and Piece_No = '" & Trim(.Rows(i).Cells(2).Value) & "'"
                            Nr = cmd.ExecuteNonQuery()

                        End If

                        If Nr <> 1 Then
                            Throw New ApplicationException("Invalid Pcs Details  -  LotNo. " & Trim(.Rows(i).Cells(8).Value) & " , Piece No = " & Trim(.Rows(i).Cells(2).Value))
                            Exit Sub
                        End If

                    End If


                    If Val(dgv_Details.Rows(i).Cells(4).Value) <> 0 Then

                        cmd.CommandText = "Insert into Stock_Cloth_Processing_Details ( Reference_Code,             Company_IdNo         ,           Reference_No        ,                               for_OrderBy                              , Reference_Date ,                                            StockOff_IdNo  ,     DeliveryTo_Idno      ,     ReceivedFrom_Idno   ,         Entry_ID     ,       Party_Bill_No  ,       Particulars      , Sl_No,           Cloth_Idno    ,                Folding            ,   Meters_Type" & Trim(Val(dCloTyp_ID)) & " ) " &
                                                " Values ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_RefNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text))) & ",   @RecDate   , " & Str(Val(Common_Procedures.CommonLedger.Godown_Ac)) & ", " & Str(Val(Delv_ID)) & ", " & Str(Val(Rec_ID)) & ", '" & Trim(EntID) & "', '" & Trim(PBlNo) & "', '" & Trim(Partcls) & "',  " & Str(Val(Sno)) & "  , " & Str(Val(dClo_ID)) & ", " & Str(Val(txt_Folding.Text)) & ", " & Str(Val(dgv_Details.Rows(i).Cells(4).Value)) & "        ) "
                        cmd.ExecuteNonQuery()

                    End If

                Next

                If Val(vTotMtrs) <> 0 Then

                    Rec_ID = 0
                    Delv_ID = Val(Common_Procedures.CommonLedger.Godown_Ac)

                    Sno = 5001

                    cmd.CommandText = "Insert into Stock_Cloth_Processing_Details ( Reference_Code,             Company_IdNo             ,           Reference_No        ,                               for_OrderBy                              , Reference_Date ,                                            StockOff_IdNo  ,     DeliveryTo_Idno      ,     ReceivedFrom_Idno   ,         Entry_ID       ,       Party_Bill_No  ,       Particulars      ,            Sl_No     ,           Cloth_Idno     ,                Folding            ,   Meters_Type" & Trim(Val(Clthty_ID)) & " ) " &
                                                " Values ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_RefNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text))) & ",   @RecDate , " & Str(Val(Common_Procedures.CommonLedger.Godown_Ac)) & ", " & Str(Val(Delv_ID)) & ", " & Str(Val(Rec_ID)) & ", '" & Trim(EntID) & "', '" & Trim(PBlNo) & "', '" & Trim(Partcls) & "', " & Str(Val(Sno)) & ", " & Str(Val(Clth_ID)) & ", " & Str(Val(txt_Folding.Text)) & ", " & Str(Val(vTotMtrs)) & " ) "
                    cmd.ExecuteNonQuery()

                End If

                Call Common_Procedures.User_Modification_Updation(con, "DETAILS", Me.Name, "NEW", "Piece_Joining_Details", "Piece_Joining_Code", Val(lbl_Company.Tag), NewCode, lbl_RefNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, False, "Lot_No, Pcs_No, ClothType_IdNo, Meters, Weight, Weight_Meter, Party_IdNo ,  Lot_Code", "Sl_No", "Piece_Joining_Code, For_OrderBy, Company_IdNo, Piece_Joining_No, Piece_Joining_Date, Ledger_Idno", tr)

            End With


            If vGdwnTo_IdNo = 0 Then vGdwnTo_IdNo = 4

            vWgt_Mtr = 0
            If Val(vTotMtrs) <> 0 Then
                vWgt_Mtr = Format(vTotWgt / vTotMtrs, "##########0.000")
            End If

            Nr = 0
            cmd.CommandText = "Update Weaver_ClothReceipt_Piece_Details set Weaver_Piece_Checking_Date = @RecDate, Ledger_Idno = " & Str(Val(led_id)) & ", StockOff_IdNo = " & Str(Val(led_id)) & ", Folding_Checking = " & Str(Val(txt_Folding.Text)) & ", Folding = " & Str(Val(txt_Folding.Text)) & ", Sl_No = " & Str(Val(txt_NewPieceNo.Text)) & ", Main_PieceNo = '" & Trim(Val(txt_NewPieceNo.Text)) & "', PieceNo_OrderBy = " & Str(Val(Common_Procedures.OrderBy_CodeToValue(txt_NewPieceNo.Text))) & ", ReceiptMeters_Checking = 0, Receipt_Meters = 0, Loom_No = '" & Trim(vNEW_LOOMNO) & "', Pick = 0, Width = 0, Type" & Trim(Val(Clthty_ID)) & "_Meters = " & Str(Val(vTotMtrs)) & ", Total_Checking_Meters = " & Str(Val(vTotMtrs)) & ", Weight = " & Str(Val(vTotWgt)) & ", Weight_Meter = " & Str(Val(vWgt_Mtr)) & ", " &
                                            " Beam_Knotting_Code = '', Beam_Knotting_No = '', Loom_IdNo = 0, Width_Type = '', Crimp_Percentage = 0, Set_Code1 = '', Set_No1 = '', Beam_No1 = '', Balance_Meters1 = 0, Set_Code2 = '', Set_No2 = '', Beam_No2 = '', Balance_Meters2 = 0, BeamConsumption_Meters = 0, warehouse_idno = " & Str(Val(vGdwnTo_IdNo)) & " " &
                                            " Where Weaver_Piece_Checking_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' and Weaver_ClothReceipt_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' and Lot_Code = '" & Trim(vNEW_LOTCODE) & "' and Piece_No = '" & Trim(txt_NewPieceNo.Text) & "'"
            Nr = cmd.ExecuteNonQuery()

            If Nr = 0 Then
                cmd.CommandText = "Insert into Weaver_ClothReceipt_Piece_Details (          Weaver_Piece_Checking_Code         ,             Company_IdNo         ,     Weaver_Piece_Checking_No   ,  Weaver_Piece_Checking_Date,         Ledger_Idno     ,         StockOff_IdNo   ,                Weaver_ClothReceipt_Code     ,      Weaver_ClothReceipt_No     ,                                for_orderby                             , Weaver_ClothReceipt_Date,        Lot_Code              ,           Lot_No                 ,           Cloth_IdNo     ,              Folding_Checking     ,               Folding             ,                 Sl_No                ,                 Piece_No         ,           Main_PieceNo                    ,                        PieceNo_OrderBy                                            , ReceiptMeters_Checking,  Receipt_Meters,             Loom_No        , Pick , Width , Type" & Trim(Val(Clthty_ID)) & "_Meters, Total_Checking_Meters   ,           Weight         ,          Weight_Meter     , Beam_Knotting_Code, Beam_Knotting_No, Loom_IdNo, Width_Type, Crimp_Percentage, Set_Code1, Set_No1, Beam_No1, Balance_Meters1, Set_Code2, Set_No2 , Beam_No2, Balance_Meters2, BeamConsumption_Meters ,          warehouse_idno        ) " &
                                                "     Values                     ( '" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ",  '" & Trim(lbl_RefNo.Text) & "',        @RecDate            , " & Str(Val(led_id)) & ", " & Str(Val(led_id)) & ", '" & Trim(Pk_Condition) & Trim(NewCode) & "',   '" & Trim(lbl_RefNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text))) & ",      @RecDate           , '" & Trim(vNEW_LOTCODE) & "', '" & Trim(txt_NewLotNo.Text) & "', " & Str(Val(Clth_ID)) & ", " & Str(Val(txt_Folding.Text)) & ", " & Str(Val(txt_Folding.Text)) & ", " & Str(Val(txt_NewPieceNo.Text)) & ", '" & Trim(txt_NewPieceNo.Text) & "', '" & Trim(Val(txt_NewPieceNo.Text)) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(Trim(txt_NewPieceNo.Text)))) & ",          0            ,         0      , '" & Trim(vNEW_LOOMNO) & "',   0  ,   0   ,         " & Str(Val(vTotMtrs)) & "   , " & Str(Val(vTotMtrs)) & ", " & Str(Val(vTotWgt)) & ", " & Str(Val(vWgt_Mtr)) & ",       ''          ,        ''       ,     0    ,    ''     ,       0         ,     ''   ,    ''  ,    ''   ,          0     ,     ''   ,    ''   ,    ''   ,        0       ,          0 , " & Str(Val(vGdwnTo_IdNo)) & " ) "
                cmd.ExecuteNonQuery()
            End If

            tr.Commit()

            If SaveAll_STS <> True Then
                MessageBox.Show("Saved Sucessfully!!!", "FOR SAVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Information)
            End If


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
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Cloth, cbo_PartyName, cbo_ClothType, "Cloth_Head", "Cloth_Name", "", "Cloth_Name")

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



    Private Sub txt_Folding_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_Folding.KeyDown
        If e.KeyValue = 38 Then SendKeys.Send("+{TAB}")
        If e.KeyValue = 40 Then txt_NewLotNo.Focus() ' txt_Note.Focus()
    End Sub

    Private Sub txt_Folding_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Folding.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
            e.Handled = True
        End If
        If Asc(e.KeyChar) = 13 Then


            If MessageBox.Show("Do you want to select Piece", "FOR PIECE SELECTION...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = DialogResult.Yes Then
                btn_Selection_Click(sender, e)

            Else
                txt_NewLotNo.Focus()
                'txt_Note.Focus()


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
            txt_NewPieceNo.Focus()
            'txt_Folding.Focus()
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
                Condt = "a.Piece_Joining_Date between '" & Trim(Format(dtp_Filter_Fromdate.Value, "MM/dd/yyyy")) & "' and '" & Trim(Format(dtp_Filter_ToDate.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_Filter_Fromdate.Value) = True Then
                Condt = "a.Piece_Joining_Date = '" & Trim(Format(dtp_Filter_Fromdate.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_Filter_ToDate.Value) = True Then
                Condt = "a.Piece_Joining_Date = '" & Trim(Format(dtp_Filter_ToDate.Value, "MM/dd/yyyy")) & "' "
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

            If Trim(txt_Filter_LotNo.Text) <> "" Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " a.Piece_Joining_Code IN (select z1.Piece_Joining_Code from Piece_Joining_Details z1 where z1.Lot_No = '" & Trim(txt_Filter_LotNo.Text) & "') "
            End If
            If Trim(txt_Filter_PcsNo.Text) <> "" Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " a.Piece_Joining_Code IN (select z2.Piece_Joining_Code from Piece_Joining_Details z2 where z2.Pcs_No = '" & Trim(txt_Filter_PcsNo.Text) & "') "
            End If

            da = New SqlClient.SqlDataAdapter("select a.* , b.Cloth_name from Piece_Joining_Head a Inner join Cloth_Head b on a.cloth_idno = b.cloth_idno where a.company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Piece_Joining_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "' " & IIf(Trim(Condt) <> "", " and ", "") & Condt & " Order by a.for_orderby, a.Piece_Joining_No", con)
            da.Fill(dt2)

            dgv_Filter_Details.Rows.Clear()

            If dt2.Rows.Count > 0 Then

                For i = 0 To dt2.Rows.Count - 1

                    n = dgv_Filter_Details.Rows.Add()

                    dgv_Filter_Details.Rows(n).Cells(0).Value = i + 1
                    dgv_Filter_Details.Rows(n).Cells(1).Value = dt2.Rows(i).Item("Piece_Joining_No").ToString
                    dgv_Filter_Details.Rows(n).Cells(2).Value = Format(Convert.ToDateTime(dt2.Rows(i).Item("Piece_Joining_Date").ToString), "dd-MM-yyyy")
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
            MessageBox.Show(ex.Message, "DOES NOT FILTER...", MessageBoxButtons.OK, MessageBoxIcon.Error)

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
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_ClothType, cbo_Filter_Cloth, txt_Filter_LotNo, "ClothType_Head", "ClothType_Name", "", "ClothType_Name")

    End Sub

    Private Sub cbo_Filter_ClothType_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Filter_ClothType.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Filter_ClothType, txt_Filter_LotNo, "ClothType_Head", "ClothType_Name", "", "ClothType_Name")

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
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim i As Integer, j As Integer, n As Integer, SNo As Integer
        Dim CloIdNo As Integer, CloTypIdNo As Integer
        Dim NewCode As String
        Dim CompIDCondt As String
        Dim vLed_IdNo As Integer = 0
        Dim Cnt_GrpIdNos As String
        Dim Cnt_IdNo As Integer, Cnt_UndIdNo As Integer
        Dim CloID_Cond As String

        vLed_IdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_PartyName.Text)

        CloIdNo = Common_Procedures.Cloth_NameToIdNo(con, cbo_Cloth.Text)
        If CloIdNo = 0 Then
            MessageBox.Show("Invalid Cloth Name", "DOES NOT SELECT PAVU...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If cbo_Cloth.Enabled And cbo_Cloth.Visible Then cbo_Cloth.Focus()
            Exit Sub
        End If

        CloTypIdNo = Common_Procedures.ClothType_NameToIdNo(con, cbo_ClothType.Text)
        If CloTypIdNo = 0 Then
            MessageBox.Show("Invalid Cloth Type", "DOES NOT SELECT PAVU...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If cbo_ClothType.Enabled And cbo_ClothType.Visible Then cbo_ClothType.Focus()
            Exit Sub
        End If

        If Val(txt_Folding.Text) = 0 Then
            MessageBox.Show("Invalid Folding", "DOES NOT SELECT PAVU...", MessageBoxButtons.OK, MessageBoxIcon.Error)
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
        'If Trim(UCase(Common_Procedures.settings.CompanyName)) = "-----~~~" Then
        '    CompIDCondt = ""
        'End If

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)


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

        CloID_Cond = "(a.Cloth_idno = " & Str(Cnt_IdNo) & " or a.Cloth_idno IN " & Trim(Cnt_GrpIdNos) & ")"

        If vLed_IdNo = 4 Or vLed_IdNo = 5 Then
            CloID_Cond = CloID_Cond & IIf(CloID_Cond <> "", " and ", " ") & "(a.StockOff_IdNo = 4 or a.StockOff_IdNo = 5)"
        Else
            CloID_Cond = CloID_Cond & IIf(CloID_Cond <> "", " and ", " ") & "(a.StockOff_IdNo = " & Str(Val(vLed_IdNo)) & ")"
        End If

        With dgv_Selection
            chk_SelectAll.Checked = False
            .Rows.Clear()
            SNo = 0

            If CloTypIdNo = 1 Then

                Da = New SqlClient.SqlDataAdapter("select a.* , c.Ledger_Name, d.cloth_name from Weaver_ClothReceipt_Piece_Details a  INNER JOIN Company_Head tZ ON a.company_idno <> 0 and a.company_idno = tZ.company_idno LEFT OUTER JOIN Ledger_Head c ON a.Ledger_IdNo = c.Ledger_IdNo LEFT OUTER JOIN Cloth_Head d ON a.Cloth_IdNo <> 0 and a.Cloth_IdNo = d.Cloth_IdNo Where " & CompIDCondt & IIf(Trim(CompIDCondt) <> "", " and ", "") & " a.Type1_Meters <> 0 and a.PackingSlip_Code_Type1 = '" & Trim(Pk_Condition) & Trim(NewCode) & "' and " & CloID_Cond & IIf(CloID_Cond <> "", " and ", " ") & "  a.Folding = " & Str(Val(txt_Folding.Text)) & " order by a.Weaver_ClothReceipt_Date, a.for_orderby, a.Weaver_ClothReceipt_No, a.PieceNo_OrderBy, a.Piece_No", con)
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
                        .Rows(n).Cells(4).Value = Dt1.Rows(i).Item("Type1_Meters").ToString
                        If Val(Dt1.Rows(i).Item("Weight").ToString) <> 0 And Val(Dt1.Rows(i).Item("Type1_Meters").ToString) = Val(Dt1.Rows(i).Item("Total_Checking_Meters").ToString) Then
                            .Rows(n).Cells(5).Value = Format(Val(Dt1.Rows(i).Item("Weight").ToString), "#########0.000")
                        Else
                            .Rows(n).Cells(5).Value = Format(Val(Dt1.Rows(i).Item("Type1_Meters").ToString) * Val(Dt1.Rows(i).Item("Weight_Meter").ToString), "#########0.000")
                        End If
                        .Rows(n).Cells(6).Value = Val(Dt1.Rows(i).Item("Weight_Meter").ToString)
                        .Rows(n).Cells(7).Value = Dt1.Rows(i).Item("Ledger_Name").ToString
                        .Rows(n).Cells(8).Value = "1"
                        .Rows(n).Cells(9).Value = Dt1.Rows(i).Item("Lot_Code").ToString
                        .Rows(n).Cells(10).Value = Dt1.Rows(i).Item("Cloth_Name").ToString

                        For j = 0 To .ColumnCount - 1
                            .Rows(i).Cells(j).Style.ForeColor = Color.Red
                        Next

                    Next

                End If
                Dt1.Clear()


                Dim vA1 As String = 0
                Dim vA2 As String = 0

                Da = New SqlClient.SqlDataAdapter("select a.*, c.Ledger_Name, d.Cloth_Name from Weaver_ClothReceipt_Piece_Details a  INNER JOIN Company_Head tZ ON a.company_idno <> 0 and a.company_idno = tZ.company_idno LEFT OUTER JOIN Ledger_Head c ON a.Ledger_IdNo <> 0 and a.Ledger_IdNo = c.Ledger_IdNo  LEFT OUTER JOIN Cloth_Head d ON a.Cloth_IdNo <> 0 and a.Cloth_IdNo = d.Cloth_IdNo where " & CompIDCondt & IIf(Trim(CompIDCondt) <> "", " and ", "") & " a.Type1_Meters <> 0 and a.PackingSlip_Code_Type1 = ''  and " & CloID_Cond & IIf(CloID_Cond <> "", " and ", " ") & "  a.Folding = " & Str(Val(txt_Folding.Text)) & " order by a.Weaver_ClothReceipt_Date, a.for_orderby, a.Weaver_ClothReceipt_No, a.PieceNo_OrderBy, a.Piece_No", con)
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
                        .Rows(n).Cells(4).Value = Dt1.Rows(i).Item("Type1_Meters").ToString

                        If Val(Dt1.Rows(i).Item("Weight").ToString) <> 0 And Val(Dt1.Rows(i).Item("Type1_Meters").ToString) = Val(Dt1.Rows(i).Item("Total_Checking_Meters").ToString) Then
                            .Rows(n).Cells(5).Value = Format(Val(Dt1.Rows(i).Item("Weight").ToString), "#########0.000")
                        Else
                            .Rows(n).Cells(5).Value = Format(Val(Dt1.Rows(i).Item("Type1_Meters").ToString) * Val(Dt1.Rows(i).Item("Weight_Meter").ToString), "#########0.000")
                        End If

                        .Rows(n).Cells(6).Value = Val(Dt1.Rows(i).Item("Weight_Meter").ToString)
                        .Rows(n).Cells(7).Value = Dt1.Rows(i).Item("Ledger_Name").ToString
                        .Rows(n).Cells(8).Value = ""
                        .Rows(n).Cells(9).Value = Dt1.Rows(i).Item("Lot_Code").ToString
                        .Rows(n).Cells(10).Value = Dt1.Rows(i).Item("Cloth_Name").ToString

                    Next

                End If
                Dt1.Clear()

            End If

            If CloTypIdNo = 1 Or CloTypIdNo = 2 Then

                Da = New SqlClient.SqlDataAdapter("select a.* , C.Ledger_Name, d.cloth_name from Weaver_ClothReceipt_Piece_Details a  INNER JOIN Company_Head tZ ON a.company_idno <> 0 and a.company_idno = tZ.company_idno LEFT OUTER JOIN Ledger_Head c ON a.Ledger_IdNo <> 0 and a.Ledger_IdNo = c.Ledger_IdNo LEFT OUTER JOIN Cloth_Head d ON a.Cloth_IdNo <> 0 and a.Cloth_IdNo = d.Cloth_IdNo Where " & CompIDCondt & IIf(Trim(CompIDCondt) <> "", " and ", "") & " a.Type2_Meters <> 0 and a.PackingSlip_Code_Type2 = '" & Trim(Pk_Condition) & Trim(NewCode) & "'  and " & CloID_Cond & IIf(CloID_Cond <> "", " and ", " ") & "  a.Folding = " & Str(Val(txt_Folding.Text)) & " order by a.Weaver_ClothReceipt_Date, a.for_orderby, a.Weaver_ClothReceipt_No, a.PieceNo_OrderBy, a.Piece_No", con)
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
                        .Rows(n).Cells(4).Value = Dt1.Rows(i).Item("Type2_Meters").ToString
                        If Val(Dt1.Rows(i).Item("Weight").ToString) <> 0 And Val(Dt1.Rows(i).Item("Type2_Meters").ToString) = Val(Dt1.Rows(i).Item("Total_Checking_Meters").ToString) Then
                            .Rows(n).Cells(5).Value = Format(Val(Dt1.Rows(i).Item("Weight").ToString), "#########0.000")
                        Else
                            .Rows(n).Cells(5).Value = Format(Val(Dt1.Rows(i).Item("Type2_Meters").ToString) * Val(Dt1.Rows(i).Item("Weight_Meter").ToString), "#########0.000")
                        End If
                        .Rows(n).Cells(6).Value = Val(Dt1.Rows(i).Item("Weight_Meter").ToString)
                        .Rows(n).Cells(7).Value = Dt1.Rows(i).Item("Ledger_Name").ToString
                        .Rows(n).Cells(8).Value = "1"
                        .Rows(n).Cells(9).Value = Dt1.Rows(i).Item("Lot_Code").ToString
                        .Rows(n).Cells(10).Value = Dt1.Rows(i).Item("cloth_name").ToString

                        For j = 0 To .ColumnCount - 1
                            .Rows(i).Cells(j).Style.ForeColor = Color.Red
                        Next

                    Next

                End If
                Dt1.Clear()


                Da = New SqlClient.SqlDataAdapter("select a.* , c.Ledger_Name, d.cloth_name from Weaver_ClothReceipt_Piece_Details a  INNER JOIN Company_Head tZ ON a.company_idno <> 0 and a.company_idno = tZ.company_idno LEFT OUTER JOIN Ledger_Head c ON a.Ledger_IdNo = c.Ledger_IdNo LEFT OUTER JOIN Cloth_Head d ON a.Cloth_IdNo <> 0 and a.Cloth_IdNo = d.Cloth_IdNo where " & CompIDCondt & IIf(Trim(CompIDCondt) <> "", " and ", "") & " a.Type2_Meters <> 0 and a.PackingSlip_Code_Type2 = ''  and " & CloID_Cond & IIf(CloID_Cond <> "", " and ", " ") & " a.Folding = " & Str(Val(txt_Folding.Text)) & " order by a.Weaver_ClothReceipt_Date, a.for_orderby, a.Weaver_ClothReceipt_No, a.PieceNo_OrderBy, a.Piece_No", con)
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
                        .Rows(n).Cells(4).Value = Dt1.Rows(i).Item("Type2_Meters").ToString
                        If Val(Dt1.Rows(i).Item("Weight").ToString) <> 0 And Val(Dt1.Rows(i).Item("Type2_Meters").ToString) = Val(Dt1.Rows(i).Item("Total_Checking_Meters").ToString) Then
                            .Rows(n).Cells(5).Value = Format(Val(Dt1.Rows(i).Item("Weight").ToString), "#########0.000")
                        Else
                            .Rows(n).Cells(5).Value = Format(Val(Dt1.Rows(i).Item("Type2_Meters").ToString) * Val(Dt1.Rows(i).Item("Weight_Meter").ToString), "#########0.000")
                        End If
                        .Rows(n).Cells(6).Value = Val(Dt1.Rows(i).Item("Weight_Meter").ToString)
                        .Rows(n).Cells(7).Value = Dt1.Rows(i).Item("Ledger_Name").ToString
                        .Rows(n).Cells(8).Value = ""
                        .Rows(n).Cells(9).Value = Dt1.Rows(i).Item("Lot_Code").ToString
                        .Rows(n).Cells(10).Value = Dt1.Rows(i).Item("cloth_name").ToString

                    Next

                End If
                Dt1.Clear()
            End If

            If CloTypIdNo = 3 Then

                Da = New SqlClient.SqlDataAdapter("select a.* , c.Ledger_Name, d.cloth_name from Weaver_ClothReceipt_Piece_Details a  INNER JOIN Company_Head tZ ON a.company_idno <> 0 and a.company_idno = tZ.company_idno LEFT OUTER JOIN Ledger_Head c ON a.Ledger_IdNo = c.Ledger_IdNo LEFT OUTER JOIN Cloth_Head d ON a.Cloth_IdNo <> 0 and a.Cloth_IdNo = d.Cloth_IdNo where " & CompIDCondt & IIf(Trim(CompIDCondt) <> "", " and ", "") & " a.Type3_Meters <> 0 and a.PackingSlip_Code_Type3 = '" & Trim(Pk_Condition) & Trim(NewCode) & "'  and " & CloID_Cond & IIf(CloID_Cond <> "", " and ", " ") & "  a.Folding = " & Str(Val(txt_Folding.Text)) & " order by a.Weaver_ClothReceipt_Date, a.for_orderby, a.Weaver_ClothReceipt_No, a.PieceNo_OrderBy, a.Piece_No", con)
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
                        .Rows(n).Cells(4).Value = Dt1.Rows(i).Item("Type3_Meters").ToString
                        If Val(Dt1.Rows(i).Item("Weight").ToString) <> 0 And Val(Dt1.Rows(i).Item("Type3_Meters").ToString) = Val(Dt1.Rows(i).Item("Total_Checking_Meters").ToString) Then
                            .Rows(n).Cells(5).Value = Format(Val(Dt1.Rows(i).Item("Weight").ToString), "#########0.000")
                        Else
                            .Rows(n).Cells(5).Value = Format(Val(Dt1.Rows(i).Item("Type3_Meters").ToString) * Val(Dt1.Rows(i).Item("Weight_Meter").ToString), "#########0.000")
                        End If
                        .Rows(n).Cells(6).Value = Val(Dt1.Rows(i).Item("Weight_Meter").ToString)
                        .Rows(n).Cells(7).Value = Dt1.Rows(i).Item("Ledger_Name").ToString
                        .Rows(n).Cells(8).Value = "1"
                        .Rows(n).Cells(9).Value = Dt1.Rows(i).Item("Lot_Code").ToString
                        .Rows(n).Cells(10).Value = Dt1.Rows(i).Item("cloth_name").ToString

                        For j = 0 To .ColumnCount - 1
                            .Rows(i).Cells(j).Style.ForeColor = Color.Red
                        Next

                    Next

                End If
                Dt1.Clear()


                Da = New SqlClient.SqlDataAdapter("select a.* ,C.Ledger_Name, d.Cloth_Name from Weaver_ClothReceipt_Piece_Details a  INNER JOIN Company_Head tZ ON a.company_idno <> 0 and a.company_idno = tZ.company_idno LEFT OUTER JOIN Ledger_Head c ON a.Ledger_IdNo <> 0 and a.Ledger_IdNo = c.Ledger_IdNo  LEFT OUTER JOIN Cloth_Head d ON a.cloth_IdNo <> 0 and a.cloth_IdNo = d.cloth_IdNo where " & CompIDCondt & IIf(Trim(CompIDCondt) <> "", " and ", "") & " a.Type3_Meters <> 0 and a.PackingSlip_Code_Type3 = ''  and " & CloID_Cond & IIf(CloID_Cond <> "", " and ", " ") & "  a.Folding = " & Str(Val(txt_Folding.Text)) & " order by a.Weaver_ClothReceipt_Date, a.for_orderby, a.Weaver_ClothReceipt_No, a.PieceNo_OrderBy, a.Piece_No", con)
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
                        .Rows(n).Cells(4).Value = Dt1.Rows(i).Item("Type3_Meters").ToString
                        If Val(Dt1.Rows(i).Item("Weight").ToString) <> 0 And Val(Dt1.Rows(i).Item("Type3_Meters").ToString) = Val(Dt1.Rows(i).Item("Total_Checking_Meters").ToString) Then
                            .Rows(n).Cells(5).Value = Format(Val(Dt1.Rows(i).Item("Weight").ToString), "#########0.000")
                        Else
                            .Rows(n).Cells(5).Value = Format(Val(Dt1.Rows(i).Item("Type3_Meters").ToString) * Val(Dt1.Rows(i).Item("Weight_Meter").ToString), "#########0.000")
                        End If
                        .Rows(n).Cells(6).Value = Val(Dt1.Rows(i).Item("Weight_Meter").ToString)
                        .Rows(n).Cells(7).Value = Dt1.Rows(i).Item("Ledger_Name").ToString
                        .Rows(n).Cells(8).Value = ""
                        .Rows(n).Cells(9).Value = Dt1.Rows(i).Item("Lot_Code").ToString
                        .Rows(n).Cells(10).Value = Dt1.Rows(i).Item("Cloth_Name").ToString

                    Next

                End If
                Dt1.Clear()
            End If

            If CloTypIdNo = 4 Then

                Da = New SqlClient.SqlDataAdapter("select a.* , C.Ledger_Name, d.cloth_name from Weaver_ClothReceipt_Piece_Details a  INNER JOIN Company_Head tZ ON a.company_idno <> 0 and a.company_idno = tZ.company_idno LEFT OUTER JOIN Ledger_Head c ON a.Ledger_IdNo <> 0 and a.Ledger_IdNo = c.Ledger_IdNo LEFT OUTER JOIN Cloth_Head d ON d.Cloth_IdNo <> 0 and a.Cloth_IdNo = d.Cloth_IdNo where " & CompIDCondt & IIf(Trim(CompIDCondt) <> "", " and ", "") & " a.Type4_Meters <> 0 and a.PackingSlip_Code_Type4 = '" & Trim(Pk_Condition) & Trim(NewCode) & "'  and " & CloID_Cond & IIf(CloID_Cond <> "", " and ", " ") & "  a.Folding = " & Str(Val(txt_Folding.Text)) & " order by a.Weaver_ClothReceipt_Date, a.for_orderby, a.Weaver_ClothReceipt_No, a.PieceNo_OrderBy, a.Piece_No", con)
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
                        .Rows(n).Cells(4).Value = Dt1.Rows(i).Item("Type4_Meters").ToString
                        If Val(Dt1.Rows(i).Item("Weight").ToString) <> 0 And Val(Dt1.Rows(i).Item("Type4_Meters").ToString) = Val(Dt1.Rows(i).Item("Total_Checking_Meters").ToString) Then
                            .Rows(n).Cells(5).Value = Format(Val(Dt1.Rows(i).Item("Weight").ToString), "#########0.000")
                        Else
                            .Rows(n).Cells(5).Value = Format(Val(Dt1.Rows(i).Item("Type4_Meters").ToString) * Val(Dt1.Rows(i).Item("Weight_Meter").ToString), "#########0.000")
                        End If
                        .Rows(n).Cells(6).Value = Val(Dt1.Rows(i).Item("Weight_Meter").ToString)
                        .Rows(n).Cells(7).Value = Dt1.Rows(i).Item("Ledger_Name").ToString
                        .Rows(n).Cells(8).Value = "1"
                        .Rows(n).Cells(9).Value = Dt1.Rows(i).Item("Lot_Code").ToString
                        .Rows(n).Cells(10).Value = Dt1.Rows(i).Item("Cloth_Name").ToString

                        For j = 0 To .ColumnCount - 1
                            .Rows(i).Cells(j).Style.ForeColor = Color.Red
                        Next

                    Next

                End If
                Dt1.Clear()


                Da = New SqlClient.SqlDataAdapter("select a.* , C.Ledger_Name, d.cloth_name from Weaver_ClothReceipt_Piece_Details a  INNER JOIN Company_Head tZ ON a.company_idno <> 0 and a.company_idno = tZ.company_idno LEFT OUTER JOIN Ledger_Head c ON a.Ledger_IdNo <> 0 and a.Ledger_IdNo = c.Ledger_IdNo LEFT OUTER JOIN Cloth_Head d ON a.Cloth_IdNo <> 0 and a.Cloth_IdNo = d.Cloth_IdNo where " & CompIDCondt & IIf(Trim(CompIDCondt) <> "", " and ", "") & " a.Type4_Meters <> 0 and a.PackingSlip_Code_Type4 = ''  and " & CloID_Cond & IIf(CloID_Cond <> "", " and ", " ") & "  a.Folding = " & Str(Val(txt_Folding.Text)) & " order by a.Weaver_ClothReceipt_Date, a.for_orderby, a.Weaver_ClothReceipt_No, a.PieceNo_OrderBy, a.Piece_No", con)
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
                        .Rows(n).Cells(4).Value = Dt1.Rows(i).Item("Type4_Meters").ToString
                        If Val(Dt1.Rows(i).Item("Weight").ToString) <> 0 And Val(Dt1.Rows(i).Item("Type4_Meters").ToString) = Val(Dt1.Rows(i).Item("Total_Checking_Meters").ToString) Then
                            .Rows(n).Cells(5).Value = Format(Val(Dt1.Rows(i).Item("Weight").ToString), "#########0.000")
                        Else
                            .Rows(n).Cells(5).Value = Format(Val(Dt1.Rows(i).Item("Type4_Meters").ToString) * Val(Dt1.Rows(i).Item("Weight_Meter").ToString), "#########0.000")
                        End If
                        .Rows(n).Cells(6).Value = Val(Dt1.Rows(i).Item("Weight_Meter").ToString)
                        .Rows(n).Cells(7).Value = Dt1.Rows(i).Item("Ledger_Name").ToString
                        .Rows(n).Cells(8).Value = ""
                        .Rows(n).Cells(9).Value = Dt1.Rows(i).Item("Lot_Code").ToString
                        .Rows(n).Cells(10).Value = Dt1.Rows(i).Item("Cloth_Name").ToString

                    Next

                End If
                Dt1.Clear()
            End If
            If CloTypIdNo = 5 Then

                Da = New SqlClient.SqlDataAdapter("select a.*, c.Ledger_Name, d.cloth_name from Weaver_ClothReceipt_Piece_Details a  INNER JOIN Company_Head tZ ON a.company_idno <> 0 and a.company_idno = tZ.company_idno LEFT OUTER JOIN Ledger_Head c ON a.Ledger_IdNo <> 0 and a.Ledger_IdNo = c.Ledger_IdNo LEFT OUTER JOIN Cloth_Head d ON a.Cloth_IdNo <> 0 and a.Cloth_IdNo = d.Cloth_IdNo where " & CompIDCondt & IIf(Trim(CompIDCondt) <> "", " and ", "") & " a.Type5_Meters <> 0 and a.PackingSlip_Code_Type5 = '" & Trim(Pk_Condition) & Trim(NewCode) & "'  and " & CloID_Cond & IIf(CloID_Cond <> "", " and ", " ") & "  a.Folding = " & Str(Val(txt_Folding.Text)) & " order by a.Weaver_ClothReceipt_Date, a.for_orderby, a.Weaver_ClothReceipt_No, a.PieceNo_OrderBy, a.Piece_No", con)
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
                        .Rows(n).Cells(4).Value = Dt1.Rows(i).Item("Type5_Meters").ToString
                        If Val(Dt1.Rows(i).Item("Weight").ToString) <> 0 And Val(Dt1.Rows(i).Item("Type5_Meters").ToString) = Val(Dt1.Rows(i).Item("Total_Checking_Meters").ToString) Then
                            .Rows(n).Cells(5).Value = Format(Val(Dt1.Rows(i).Item("Weight").ToString), "#########0.000")
                        Else
                            .Rows(n).Cells(5).Value = Format(Val(Dt1.Rows(i).Item("Type5_Meters").ToString) * Val(Dt1.Rows(i).Item("Weight_Meter").ToString), "#########0.000")
                        End If
                        .Rows(n).Cells(6).Value = Val(Dt1.Rows(i).Item("Weight_Meter").ToString)
                        .Rows(n).Cells(7).Value = Dt1.Rows(i).Item("Ledger_Name").ToString
                        .Rows(n).Cells(8).Value = "1"
                        .Rows(n).Cells(9).Value = Dt1.Rows(i).Item("Lot_Code").ToString
                        .Rows(n).Cells(10).Value = Dt1.Rows(i).Item("Cloth_Name").ToString

                        For j = 0 To .ColumnCount - 1
                            .Rows(i).Cells(j).Style.ForeColor = Color.Red
                        Next

                    Next

                End If
                Dt1.Clear()


                Da = New SqlClient.SqlDataAdapter("select a.*, c.Ledger_Name, d.cloth_name from Weaver_ClothReceipt_Piece_Details a  INNER JOIN Company_Head tZ ON a.company_idno <> 0 and a.company_idno = tZ.company_idno LEFT OUTER JOIN Ledger_Head c ON a.Ledger_IdNo <> 0 and a.Ledger_IdNo = c.Ledger_IdNo LEFT OUTER JOIN Cloth_Head d ON a.Cloth_IdNo <> 0 and a.Cloth_IdNo = d.Cloth_IdNo where " & CompIDCondt & IIf(Trim(CompIDCondt) <> "", " and ", "") & " a.Type5_Meters <> 0 and a.PackingSlip_Code_Type5 = '' and " & CloID_Cond & IIf(CloID_Cond <> "", " and ", " ") & "  a.Folding = " & Str(Val(txt_Folding.Text)) & "  order by a.Weaver_ClothReceipt_Date, a.for_orderby, a.Weaver_ClothReceipt_No, a.PieceNo_OrderBy, a.Piece_No", con)
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
                        .Rows(n).Cells(4).Value = Dt1.Rows(i).Item("Type5_Meters").ToString
                        If Val(Dt1.Rows(i).Item("Weight").ToString) <> 0 And Val(Dt1.Rows(i).Item("Type5_Meters").ToString) = Val(Dt1.Rows(i).Item("Total_Checking_Meters").ToString) Then
                            .Rows(n).Cells(5).Value = Format(Val(Dt1.Rows(i).Item("Weight").ToString), "#########0.000")
                        Else
                            .Rows(n).Cells(5).Value = Format(Val(Dt1.Rows(i).Item("Type5_Meters").ToString) * Val(Dt1.Rows(i).Item("Weight_Meter").ToString), "#########0.000")
                        End If
                        .Rows(n).Cells(6).Value = Val(Dt1.Rows(i).Item("Weight_Meter").ToString)
                        .Rows(n).Cells(7).Value = Dt1.Rows(i).Item("Ledger_Name").ToString
                        .Rows(n).Cells(8).Value = ""
                        .Rows(n).Cells(9).Value = Dt1.Rows(i).Item("Lot_Code").ToString
                        .Rows(n).Cells(10).Value = Dt1.Rows(i).Item("Cloth_Name").ToString

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
        Dim i As Integer = 0
        Dim vFIRSTLOTNO As String = ""
        Dim vFIRSTPCSNO As String = ""

        dgv_Details.Rows.Clear()

        sno = 0
        vFIRSTLOTNO = ""
        vFIRSTPCSNO = ""
        For i = 0 To dgv_Selection.RowCount - 1

            If Val(dgv_Selection.Rows(i).Cells(8).Value) = 1 Then

                n = dgv_Details.Rows.Add()


                sno = sno + 1
                dgv_Details.Rows(n).Cells(0).Value = sno
                dgv_Details.Rows(n).Cells(1).Value = dgv_Selection.Rows(i).Cells(1).Value
                dgv_Details.Rows(n).Cells(2).Value = dgv_Selection.Rows(i).Cells(2).Value
                dgv_Details.Rows(n).Cells(3).Value = dgv_Selection.Rows(i).Cells(3).Value
                dgv_Details.Rows(n).Cells(4).Value = dgv_Selection.Rows(i).Cells(4).Value
                If Val(dgv_Selection.Rows(i).Cells(5).Value) <> 0 Then
                    dgv_Details.Rows(n).Cells(5).Value = dgv_Selection.Rows(i).Cells(5).Value
                End If
                If Val(dgv_Selection.Rows(i).Cells(6).Value) <> 0 Then
                    dgv_Details.Rows(n).Cells(6).Value = dgv_Selection.Rows(i).Cells(6).Value
                End If
                dgv_Details.Rows(n).Cells(7).Value = dgv_Selection.Rows(i).Cells(7).Value
                dgv_Details.Rows(n).Cells(8).Value = dgv_Selection.Rows(i).Cells(9).Value
                dgv_Details.Rows(n).Cells(9).Value = dgv_Selection.Rows(i).Cells(10).Value

                If Trim(vFIRSTLOTNO) = "" Then
                    vFIRSTLOTNO = dgv_Details.Rows(n).Cells(1).Value
                    vFIRSTPCSNO = dgv_Details.Rows(n).Cells(2).Value
                End If


            End If

        Next i

        txt_NewLotNo.Text = Trim(vFIRSTLOTNO)
        txt_NewPieceNo.Text = Trim(vFIRSTPCSNO)

        Total_Calculation()

        Pnl_Back.Enabled = True
        pnl_Selection.Visible = False
        If txt_NewLotNo.Enabled And txt_NewLotNo.Visible Then
            txt_NewLotNo.Focus()
        Else
            If txt_Note.Enabled And txt_Note.Visible Then txt_Note.Focus()
        End If


    End Sub

    Public Sub print_record() Implements Interface_MDIActions.print_record
        '------
    End Sub

    Private Sub dgtxt_Details_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgtxt_Details.KeyUp
        If e.Control = True And UCase(Chr(e.KeyCode)) = "D" Then
            dgv_Details_KeyUp(sender, e)
        End If
    End Sub

    Private Sub cbo_PartyName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_PartyName.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'GODOWN' or Ledger_Type = 'JOBWORKER' or Show_In_All_Entry = 1)", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_PartyName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_PartyName.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_PartyName, msk_date, cbo_Cloth, "Ledger_AlaisHead", "Ledger_DisplayName", "( Ledger_Type = 'GODOWN' or Ledger_Type = 'JOBWORKER' or Show_In_All_Entry = 1)", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_PartyName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_PartyName.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_PartyName, cbo_Cloth, "Ledger_AlaisHead", "Ledger_DisplayName", "( Ledger_Type = 'GODOWN' or Ledger_Type = 'JOBWORKER' or Show_In_All_Entry = 1)", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_PartyName_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_PartyName.KeyUp
        If e.Control = False And e.KeyValue = 17 Then
            Common_Procedures.MDI_LedType = "JOBWORKER"
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

    Private Sub msk_date_KeyPress(sender As Object, e As System.Windows.Forms.KeyPressEventArgs) Handles msk_date.KeyPress
        If Trim(UCase(e.KeyChar)) = "D" Then
            msk_date.Text = Date.Today
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
    End Sub

    Private Sub txt_Filter_PcsNo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Filter_PcsNo.KeyPress
        If Asc(e.KeyChar) = 13 Then
            e.Handled = True
            btn_Filter_Show.Focus()
            btn_Filter_Show_Click(sender, e)
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

    Private Sub btn_SaveAll_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_SaveAll.Click
        Dim pwd As String = ""

        Dim g As New Password
        g.ShowDialog()

        pwd = Trim(Common_Procedures.Password_Input)

        If Trim(UCase(pwd)) <> "TSSA7417" Then
            MessageBox.Show("Invalid Password", "FAILED...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
        End If

        SaveAll_STS = True

        LastNo = ""
        movelast_record()

        LastNo = lbl_RefNo.Text

        movefirst_record()
        Timer1.Enabled = True

    End Sub

    Private Sub Timer1_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer1.Tick

        btn_Selection_Click(sender, e)

        btn_Close_Selection_Click(sender, e)

        save_record()

        If Trim(UCase(LastNo)) = Trim(UCase(lbl_RefNo.Text)) Then
            Timer1.Enabled = False
            SaveAll_STS = False
            MessageBox.Show("All entries saved sucessfully", "FOR SAVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Information)

        Else
            movenext_record()

        End If
    End Sub

    Private Sub txt_NewLotNo_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_NewLotNo.KeyDown
        If e.KeyValue = 38 Then txt_Folding.Focus()
        If e.KeyValue = 40 Then txt_NewPieceNo.Focus()
    End Sub

    Private Sub txt_NewLotNo_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txt_NewLotNo.KeyPress
        If Asc(e.KeyChar) = 13 Then
            txt_NewPieceNo.Focus()
        End If
    End Sub
    Private Sub txt_NewPieceNo_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_NewPieceNo.KeyDown
        If e.KeyValue = 38 Then txt_NewLotNo.Focus()
        If e.KeyValue = 40 Then txt_Note.Focus()
    End Sub

    Private Sub txt_NewPieceNo_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txt_NewPieceNo.KeyPress
        If Asc(e.KeyChar) = 13 Then
            txt_Note.Focus()
        End If
    End Sub

End Class