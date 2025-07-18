Public Class JobWork_PieceInspection_Entry
    Implements Interface_MDIActions

    Private con As New SqlClient.SqlConnection(Common_Procedures.Connection_String)
    Private FrmLdSTS As Boolean = False
    Private New_Entry As Boolean = False
    Private Insert_Entry As Boolean = False
    Private Filter_Status As Boolean = False
    Private Pk_Condition As String = "JWINS-"
    Private NoCalc_Status As Boolean = False
    Private Prec_ActCtrl As New Control
    Private WithEvents dgtxt_Details As New DataGridViewTextBoxEditingControl

    Private prn_HdDt As New DataTable
    Private prn_DetDt As New DataTable
    Private prn_PageNo As Integer
    Private prn_DetIndx As Integer
    Private prn_DetAr(50, 10) As String
    Private prn_DetMxIndx As Integer
    Private prn_NoofBmDets As Integer
    Private vcbo_KeyDwnVal As Double
    Public vmskOldText As String = ""
    Public vmskSelStrt As Integer = -1

    Private Sub clear()
        New_Entry = False
        Insert_Entry = False

        NoCalc_Status = False

        pnl_Back.Enabled = True
        pnl_Filter.Visible = False
        pnl_Selection.Visible = False
        vmskOldText = ""
        vmskSelStrt = -1
        lbl_CheckingNo.Text = ""
        lbl_CheckingNo.ForeColor = Color.Black
        msk_Date.Text = ""
        dtp_Date.Text = ""
        msk_DcDate.Text = ""
        lbl_WeftCount.Text = ""
        cbo_Ledger.Text = ""
        lbl_DcCode.Text = ""
        lbl_DcNo.Text = ""
        msk_DcDate.Text = ""
        lbl_EndsCount.Text = ""
        lbl_ConsYarn.Text = ""
        txt_Folding.Text = 100
        lbl_Delivery_Folding.Text = 100
        lbl_ClothName.Text = ""
        txt_Remarks.Text = ""
        txt_Party_Piece_Inspection_No.Text = ""

        If Filter_Status = False Then
            dtp_Filter_Fromdate.Text = Common_Procedures.Company_FromDate
            dtp_Filter_ToDate.Text = Common_Procedures.Company_ToDate
            cbo_Filter_PartyName.Text = ""
            cbo_Filter_ClothName.Text = ""
            cbo_Filter_PartyName.SelectedIndex = -1
            cbo_Filter_ClothName.SelectedIndex = -1
            dgv_Filter_Details.Rows.Clear()
        End If

        cbo_Ledger.Tag = ""

        dgv_Details.Rows.Clear()
        dgv_Details_Total.Rows.Clear()
        dgv_Details_Total.Rows.Add()

        Grid_Cell_DeSelect()

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
    Private Sub ControlGotFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim txtbx As TextBox
        Dim combobx As ComboBox
        Dim msktxbx As MaskedTextBox
        On Error Resume Next

        If TypeOf Me.ActiveControl Is TextBox Or TypeOf Me.ActiveControl Is ComboBox Or TypeOf Me.ActiveControl Is MaskedTextBox Then
            Me.ActiveControl.BackColor = Color.PaleGreen
            Me.ActiveControl.ForeColor = Color.Blue
        End If

        If TypeOf Me.ActiveControl Is TextBox Then
            txtbx = Me.ActiveControl
            txtbx.SelectAll()
        ElseIf TypeOf Me.ActiveControl Is ComboBox Then
            combobx = Me.ActiveControl
            combobx.SelectAll()
        ElseIf TypeOf Me.ActiveControl Is MaskedTextBox Then
            msktxbx = Me.ActiveControl
            msktxbx.SelectionStart = 0
        End If

        If Me.ActiveControl.Name <> dgv_Details.Name Then
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
        If Not IsNothing(dgv_Details.CurrentCell) Then dgv_Details.CurrentCell.Selected = False
        If Not IsNothing(dgv_Details_Total.CurrentCell) Then dgv_Details_Total.CurrentCell.Selected = False

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

        NoCalc_Status = True

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(no) & "/" & Trim(Common_Procedures.FnYearCode)

        Try

            da1 = New SqlClient.SqlDataAdapter("select a.*, b.Ledger_Name, c.Cloth_Name, d.EndsCount_Name, e.count_name from JobWork_Piece_Inspection_Head a INNER JOIN Ledger_Head b ON a.Ledger_IdNo = b.Ledger_IdNo INNER JOIN Cloth_Head c ON a.Cloth_IdNo = c.Cloth_IdNo LEFT OUTER JOIN EndsCount_Head d ON a.EndsCount_IdNo = d.EndsCount_IdNo LEFT OUTER JOIN Count_Head e ON a.Count_IdNo = e.Count_IdNo Where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.JobWork_Piece_Inspection_Code = '" & Trim(NewCode) & "'", con)
            dt1 = New DataTable
            da1.Fill(dt1)

            If dt1.Rows.Count > 0 Then
                lbl_CheckingNo.Text = dt1.Rows(0).Item("JobWork_Piece_Inspection_No").ToString
                dtp_Date.Text = dt1.Rows(0).Item("JobWork_Piece_Inspection_Date").ToString
                msk_Date.Text = dtp_Date.Text
                cbo_Ledger.Text = dt1.Rows(0).Item("Ledger_Name").ToString
                txt_Folding.Text = dt1.Rows(0).Item("Folding").ToString
                lbl_Delivery_Folding.Text = dt1.Rows(0).Item("Delivery_Folding").ToString
                lbl_DcCode.Text = dt1.Rows(0).Item("JobWork_Delivery_Code").ToString
                lbl_DcNo.Text = dt1.Rows(0).Item("JobWork_Delivery_No").ToString
                dtp_DcDate.Text = dt1.Rows(0).Item("JobWork_Delivery_Date").ToString
                msk_DcDate.Text = dtp_DcDate.Text
                lbl_ClothName.Text = dt1.Rows(0).Item("Cloth_Name").ToString
                lbl_EndsCount.Text = dt1.Rows(0).Item("EndsCount_Name").ToString
                lbl_WeftCount.Text = dt1.Rows(0).Item("count_name").ToString
                lbl_ConsYarn.Text = Format(Val(dt1.Rows(0).Item("Rough_Consumed_Yarn").ToString), "##########0.000")
                txt_Remarks.Text = dt1.Rows(0).Item("Remarks").ToString
                txt_Party_Piece_Inspection_No.Text = dt1.Rows(0).Item("Party_Piece_Inspection_No").ToString

                da2 = New SqlClient.SqlDataAdapter("select * from JobWork_Piece_Delivery_Details Where JobWork_Inspection_Code = '" & Trim(NewCode) & "' Order by JobWork_Piece_Delivery_Date, For_OrderBy, JobWork_Piece_Delivery_No", con)
                dt2 = New DataTable
                da2.Fill(dt2)

                dgv_Details.Rows.Clear()
                SNo = 0

                If dt2.Rows.Count > 0 Then

                    For i = 0 To dt2.Rows.Count - 1

                        n = dgv_Details.Rows.Add()

                        SNo = SNo + 1
                        dgv_Details.Rows(n).Cells(0).Value = Val(SNo)
                        dgv_Details.Rows(n).Cells(1).Value = dt2.Rows(i).Item("Lot_No").ToString & IIf(Trim(dt2.Rows(i).Item("pcs_No").ToString) <> "", "-", "") & dt2.Rows(i).Item("pcs_No").ToString
                        If Val(dt2.Rows(i).Item("Meters").ToString) <> 0 Then dgv_Details.Rows(n).Cells(2).Value = Format(Val(dt2.Rows(i).Item("Meters").ToString), "########0.00")
                        If Val(dt2.Rows(i).Item("Actual_Meters").ToString) <> 0 Then dgv_Details.Rows(n).Cells(3).Value = Format(Val(dt2.Rows(i).Item("Actual_Meters").ToString), "########0.00")
                        If Val(dt2.Rows(i).Item("Cloth_Type1_Meters").ToString) <> 0 Then dgv_Details.Rows(n).Cells(4).Value = Format(Val(dt2.Rows(i).Item("Cloth_Type1_Meters").ToString), "########0.00")
                        If Val(dt2.Rows(i).Item("Cloth_Type2_Meters").ToString) <> 0 Then dgv_Details.Rows(n).Cells(5).Value = Format(Val(dt2.Rows(i).Item("Cloth_Type2_Meters").ToString), "########0.00")
                        If Val(dt2.Rows(i).Item("Cloth_Type3_Meters").ToString) <> 0 Then dgv_Details.Rows(n).Cells(6).Value = Format(Val(dt2.Rows(i).Item("Cloth_Type3_Meters").ToString), "########0.00")
                        If Val(dt2.Rows(i).Item("Cloth_Type4_Meters").ToString) <> 0 Then dgv_Details.Rows(n).Cells(7).Value = Format(Val(dt2.Rows(i).Item("Cloth_Type4_Meters").ToString), "########0.00")
                        If Val(dt2.Rows(i).Item("Cloth_Type5_Meters").ToString) <> 0 Then dgv_Details.Rows(n).Cells(8).Value = Format(Val(dt2.Rows(i).Item("Cloth_Type5_Meters").ToString), "########0.00")
                        If Val(dt2.Rows(i).Item("Total_Checking_Meters").ToString) <> 0 Then dgv_Details.Rows(n).Cells(9).Value = Format(Val(dt2.Rows(i).Item("Total_Checking_Meters").ToString), "########0.00")
                        dgv_Details.Rows(n).Cells(10).Value = dt2.Rows(i).Item("JobWork_Piece_Delivery_Code").ToString
                        dgv_Details.Rows(n).Cells(11).Value = dt2.Rows(i).Item("Entry_PkCondition").ToString
                        dgv_Details.Rows(n).Cells(12).Value = dt2.Rows(i).Item("Lot_Code").ToString
                        dgv_Details.Rows(n).Cells(13).Value = dt2.Rows(i).Item("Pcs_No").ToString
                        If Val(dt2.Rows(i).Item("Shortage_Meters").ToString) <> 0 Then dgv_Details.Rows(n).Cells(14).Value = Format(Val(dt2.Rows(i).Item("Shortage_Meters").ToString), "########0.00")

                    Next i

                End If

                With dgv_Details_Total
                    If .RowCount = 0 Then .Rows.Add()
                    .Rows(0).Cells(1).Value = Val(dt1.Rows(0).Item("Total_Rolls").ToString)
                    .Rows(0).Cells(2).Value = Format(Val(dt1.Rows(0).Item("Total_Delivery_Meters").ToString), "########0.00")
                    .Rows(0).Cells(3).Value = Format(Val(dt1.Rows(0).Item("Total_Actual_Meters").ToString), "########0.00")
                    .Rows(0).Cells(4).Value = Format(Val(dt1.Rows(0).Item("Total_ClothType1_Meters").ToString), "########0.00")
                    .Rows(0).Cells(5).Value = Format(Val(dt1.Rows(0).Item("Total_ClothType2_Meters").ToString), "########0.00")
                    .Rows(0).Cells(6).Value = Format(Val(dt1.Rows(0).Item("Total_ClothType3_Meters").ToString), "########0.00")
                    .Rows(0).Cells(7).Value = Format(Val(dt1.Rows(0).Item("Total_ClothType4_Meters").ToString), "########0.00")
                    .Rows(0).Cells(8).Value = Format(Val(dt1.Rows(0).Item("Total_ClothType5_Meters").ToString), "########0.00")
                    .Rows(0).Cells(9).Value = Format(Val(dt1.Rows(0).Item("Total_Checking_Meters").ToString), "########0.00")
                    .Rows(0).Cells(14).Value = Format(Val(dt1.Rows(0).Item("Total_Shortage_Meters").ToString), "########0.00")
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

        NoCalc_Status = False

        If msk_Date.Visible And msk_Date.Enabled Then msk_Date.Focus()

    End Sub

    Private Sub JobWork_PieceInspection_Entry_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated
        Dim da As SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim NoofComps As Integer
        Dim CompCondt As String

        Try

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Ledger.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "LEDGER" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Ledger.Text = Trim(Common_Procedures.Master_Return.Return_Value)
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

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "DOES NOT SHOW...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        FrmLdSTS = False

    End Sub

    Private Sub JobWork_PieceInspection_Entry_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable

        Me.Text = ""

        dgv_Details.Columns(4).HeaderText = Trim(UCase(Common_Procedures.ClothType.Type1))
        dgv_Details.Columns(5).HeaderText = Trim(UCase(Common_Procedures.ClothType.Type2))
        dgv_Details.Columns(6).HeaderText = Trim(UCase(Common_Procedures.ClothType.Type3))
        dgv_Details.Columns(7).HeaderText = Trim(UCase(Common_Procedures.ClothType.Type4))
        dgv_Details.Columns(8).HeaderText = Trim(UCase(Common_Procedures.ClothType.Type5))

        dgv_Details.Columns(1).Visible = True
        dgv_Details_Total.Columns(1).Visible = True
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1052" Then '---- Shri Vedha Tex (Karumanthapatti) - Nithya Sizing

            dgv_Details.Columns(1).Visible = False
            dgv_Details_Total.Columns(1).Visible = False

            dgv_Details.Columns(2).Width = dgv_Details.Columns(2).Width + 10
            dgv_Details.Columns(3).Width = dgv_Details.Columns(3).Width + 20
            dgv_Details.Columns(4).Width = dgv_Details.Columns(4).Width + 20
            dgv_Details.Columns(5).Width = dgv_Details.Columns(5).Width + 20
            dgv_Details.Columns(6).Width = dgv_Details.Columns(6).Width + 20
            dgv_Details.Columns(7).Width = dgv_Details.Columns(7).Width + 20
            dgv_Details.Columns(8).Width = dgv_Details.Columns(8).Width + 10

            dgv_Details_Total.Columns(2).Width = dgv_Details_Total.Columns(2).Width + 10
            dgv_Details_Total.Columns(3).Width = dgv_Details_Total.Columns(3).Width + 20
            dgv_Details_Total.Columns(4).Width = dgv_Details_Total.Columns(4).Width + 20
            dgv_Details_Total.Columns(5).Width = dgv_Details_Total.Columns(5).Width + 20
            dgv_Details_Total.Columns(6).Width = dgv_Details_Total.Columns(6).Width + 20
            dgv_Details_Total.Columns(7).Width = dgv_Details_Total.Columns(7).Width + 20
            dgv_Details_Total.Columns(8).Width = dgv_Details_Total.Columns(8).Width + 10

        End If

        btn_Stock_RePosting.Visible = False
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1087" Then '---- Kalaimagal Textiles (Palladam)
            btn_Stock_RePosting.Visible = True
        End If

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1574" Then '---- VASSA TEXTILE MILLS PRIVATE LIMITED (PERUNDURAI)
            btn_get_Weft_CountName_from_Master.Visible = True
        End If

        con.Open()

        da = New SqlClient.SqlDataAdapter("select Ledger_DisplayName from Ledger_AlaisHead where (Ledger_IdNo = 0 or Ledger_Type = 'JOBWORKER') order by Ledger_DisplayName", con)
        da.Fill(dt1)
        cbo_Ledger.DataSource = dt1
        cbo_Ledger.DisplayMember = "Ledger_DisplayName"

        pnl_Filter.Visible = False
        pnl_Filter.Left = (Me.Width - pnl_Filter.Width) \ 2
        pnl_Filter.Top = (Me.Height - pnl_Filter.Height) \ 2

        pnl_Selection.Visible = False
        pnl_Selection.Left = (Me.Width - pnl_Selection.Width) \ 2
        pnl_Selection.Top = (Me.Height - pnl_Selection.Height) \ 2


        btn_UserModification.Visible = False
        If Common_Procedures.settings.User_Modifications_Show_Status = 1 Then
            If Val(Common_Procedures.User.IdNo) = 1 Or Common_Procedures.User.Show_UserModification_Status = 1 Then
                btn_UserModification.Visible = True
            End If
        End If

        AddHandler msk_Date.GotFocus, AddressOf ControlGotFocus
        AddHandler msk_DcDate.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Ledger.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Remarks.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Party_Piece_Inspection_No.GotFocus, AddressOf ControlGotFocus

        AddHandler msk_Date.LostFocus, AddressOf ControlLostFocus
        AddHandler msk_DcDate.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Ledger.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Remarks.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Party_Piece_Inspection_No.LostFocus, AddressOf ControlLostFocus

        AddHandler msk_Date.KeyDown, AddressOf TextBoxControlKeyDown

        AddHandler msk_Date.KeyPress, AddressOf TextBoxControlKeyPress

        lbl_Company.Text = ""
        lbl_Company.Tag = 0
        lbl_Company.Visible = False
        Common_Procedures.CompIdNo = 0

        Filter_Status = False
        FrmLdSTS = True
        new_record()

    End Sub

    Private Sub JobWork_PieceInspection_Entry_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        con.Close()
        con.Dispose()
    End Sub

    Private Sub JobWork_PieceInspection_Entry_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress

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

    Protected Overrides Function ProcessCmdKey(ByRef msg As System.Windows.Forms.Message, ByVal keyData As System.Windows.Forms.Keys) As Boolean
        Dim dgv1 As New DataGridView

        If ActiveControl.Name = dgv_Details.Name Or TypeOf ActiveControl Is DataGridViewTextBoxEditingControl Then

            On Error Resume Next

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

                    If keyData = Keys.Enter Or keyData = Keys.Down Then
                        If .CurrentCell.ColumnIndex >= 8 Then
                            If .CurrentCell.RowIndex = .RowCount - 1 Then
                                txt_Remarks.Focus()

                            Else
                                .CurrentCell = .Rows(.CurrentCell.RowIndex + 1).Cells(1)

                            End If

                        ElseIf .CurrentCell.ColumnIndex = 2 Then
                            .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 2)

                        Else
                            If .CurrentCell.RowIndex = .RowCount - 1 And .CurrentCell.ColumnIndex >= 1 And Trim(.CurrentRow.Cells(1).Value) = "" Then
                                txt_Remarks.Focus()


                            Else
                                .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 1)

                            End If

                        End If

                        Return True

                    ElseIf keyData = Keys.Up Then

                        If .CurrentCell.ColumnIndex <= 4 Then
                            If .CurrentCell.RowIndex = 0 Then
                                If cbo_Ledger.Enabled Then cbo_Ledger.Focus() Else dtp_Date.Focus()

                            Else
                                .CurrentCell = .Rows(.CurrentCell.RowIndex - 1).Cells(8)

                            End If

                            'ElseIf .CurrentCell.ColumnIndex = 4 Then
                            '    .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex - 2)


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

    Public Sub delete_record() Implements Interface_MDIActions.delete_record
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim cmd As New SqlClient.SqlCommand
        Dim trans As SqlClient.SqlTransaction
        Dim NewCode As String = ""
        Dim vOrdByNo As String = ""

        vOrdByNo = Common_Procedures.OrderBy_CodeToValue(lbl_DcNo.Text)

        ' If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.JobWork_Inspection_Entry, "~L~") = 0 And InStr(Common_Procedures.UR.JobWork_Inspection_Entry, "~D~") = 0 Then MessageBox.Show("You have No Rights to Delete", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error) : Exit Sub

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_CheckingNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.DeletingEntry, Common_Procedures.UR.Jobwork_Piece_Inspection_Entry, New_Entry, Me, con, "JobWork_Piece_Inspection_Head", "JobWork_Piece_Inspection_Code", NewCode, "JobWork_Piece_Inspection_Date", "(JobWork_Piece_Inspection_Code = '" & Trim(NewCode) & "')") = False Then Exit Sub







        If MessageBox.Show("Do you want to Delete?", "FOR DELETION...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) <> Windows.Forms.DialogResult.Yes Then
            Exit Sub
        End If

        If New_Entry = True Then
            MessageBox.Show("This is New Entry", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_CheckingNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Da = New SqlClient.SqlDataAdapter("select * from JobWork_Piece_Delivery_Head Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and JobWork_Inspection_Code = '" & Trim(NewCode) & "'", con)
        Dt1 = New DataTable
        Da.Fill(Dt1)
        If Dt1.Rows.Count > 0 Then
            If IsDBNull(Dt1.Rows(0).Item("JobWork_Bill_Code").ToString) = False Then
                If Trim(Dt1.Rows(0).Item("JobWork_Bill_Code").ToString) <> "" Then
                    MessageBox.Show("Already Conversion Bil prepared", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Exit Sub
                End If
            End If
        End If
        Dt1.Clear()

        trans = con.BeginTransaction

        Try

            cmd.Connection = con
            cmd.Transaction = trans
            Call Common_Procedures.User_Modification_Updation(con, "HEAD", Me.Name, "DELETE", "JobWork_Piece_Inspection_Head", "JobWork_Piece_Inspection_Code", Val(lbl_Company.Tag), NewCode, lbl_DcNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, True, "", "", "JobWork_Piece_Inspection_Code, Company_IdNo, for_OrderBy", trans)

            cmd.CommandText = "truncate table TempTable_For_Jobwork_Inspection_Stock_Posting"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Insert into TempTable_For_Jobwork_Inspection_Stock_Posting(JobWork_Piece_Delivery_Code, Entry_PkCondition, Lot_Code, Pcs_No, Cloth_Idno, Meters) select a.JobWork_Piece_Delivery_Code, a.Entry_PkCondition, a.Lot_Code, a.Pcs_No, b.Cloth_Idno, Meters from JobWork_Piece_Delivery_Details a INNER JOIN JobWork_Piece_Delivery_Head b ON a.Company_IdNo = b.Company_IdNo and a.JobWork_Piece_Delivery_Code = b.JobWork_Piece_Delivery_Code where a.JobWork_Inspection_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()


            cmd.CommandText = "Update JobWork_Piece_Delivery_Details set JobWork_Inspection_Code = '', JobWork_Inspection_Increment = JobWork_Inspection_Increment - 1, JobWork_Inspection_Date = Null, Actual_Meters = 0, Cloth_Type1_Meters = 0, Cloth_Type2_Meters = 0, Cloth_Type3_Meters = 0, Cloth_Type4_Meters = 0, Cloth_Type5_Meters = 0, Total_Checking_Meters = 0 Where JobWork_Inspection_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Update JobWork_Piece_Delivery_Head set JobWork_Inspection_Code = '', JobWork_Inspection_Increment = JobWork_Inspection_Increment - 1, JobWork_Inspection_Date = Null, Total_Actual_Meters = 0, Total_ClothType1_Meters = 0, Total_ClothType2_Meters = 0, Total_ClothType3_Meters = 0, Total_ClothType4_Meters = 0, Total_ClothType5_Meters = 0, Total_Checking_Meters = 0 Where JobWork_Inspection_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "delete from JobWork_Piece_Inspection_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and JobWork_Piece_Inspection_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1052" Then '---- Shri Vedha Tex (Karumanthapatti) - Nithya Sizing
                Stock_Posting_Pavu_and_yarn(NewCode, trans)
            End If

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

        If msk_Date.Visible And msk_Date.Enabled Then msk_Date.Focus()

    End Sub

    Public Sub filter_record() Implements Interface_MDIActions.filter_record

        If Filter_Status = False Then

            Dim da As New SqlClient.SqlDataAdapter
            Dim dt1 As New DataTable
            Dim dt2 As New DataTable
            Dim dt3 As New DataTable

            da = New SqlClient.SqlDataAdapter("select Ledger_DisplayName from Ledger_AlaisHead where (Ledger_IdNo = 0 or Ledger_Type = 'JOBWORKER') order by Ledger_DisplayName", con)
            da.Fill(dt1)
            cbo_Filter_PartyName.DataSource = dt1
            cbo_Filter_PartyName.DisplayMember = "Ledger_DisplayName"

            da = New SqlClient.SqlDataAdapter("select Cloth_name from Cloth_Head order by cloth_name", con)
            da.Fill(dt2)
            cbo_Filter_ClothName.DataSource = dt2
            cbo_Filter_ClothName.DisplayMember = "cloth_name"

            dtp_Filter_Fromdate.Text = Common_Procedures.Company_FromDate
            dtp_Filter_ToDate.Text = Common_Procedures.Company_ToDate
            cbo_Filter_PartyName.Text = ""
            cbo_Filter_ClothName.Text = ""
            cbo_Filter_PartyName.SelectedIndex = -1
            cbo_Filter_ClothName.SelectedIndex = -1
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

            da = New SqlClient.SqlDataAdapter("select top 1 JobWork_Piece_Inspection_No from JobWork_Piece_Inspection_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and JobWork_Piece_Inspection_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby, JobWork_Piece_Inspection_No", con)
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

            OrdByNo = Common_Procedures.OrderBy_CodeToValue(Trim(lbl_CheckingNo.Text))

            da = New SqlClient.SqlDataAdapter("select top 1 JobWork_Piece_Inspection_No from JobWork_Piece_Inspection_Head where for_orderby > " & Str(Val(OrdByNo)) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and JobWork_Piece_Inspection_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby, JobWork_Piece_Inspection_No", con)
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

            OrdByNo = Common_Procedures.OrderBy_CodeToValue(Trim(lbl_CheckingNo.Text))

            da = New SqlClient.SqlDataAdapter("select top 1 JobWork_Piece_Inspection_No from JobWork_Piece_Inspection_Head where for_orderby < " & Str(Val(OrdByNo)) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and JobWork_Piece_Inspection_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, JobWork_Piece_Inspection_No desc", con)
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
            da = New SqlClient.SqlDataAdapter("select top 1 JobWork_Piece_Inspection_No from JobWork_Piece_Inspection_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and JobWork_Piece_Inspection_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, JobWork_Piece_Inspection_No desc", con)
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

            lbl_CheckingNo.Text = Common_Procedures.get_MaxCode(con, "JobWork_Piece_Inspection_Head", "JobWork_Piece_Inspection_Code", "For_OrderBy", "", Val(lbl_Company.Tag), Common_Procedures.FnYearCode)

            lbl_CheckingNo.ForeColor = Color.Red

            msk_Date.Text = Date.Today.ToShortDateString
            da = New SqlClient.SqlDataAdapter("select top 1 * from JobWork_Piece_Inspection_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and JobWork_Piece_Inspection_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, JobWork_Piece_Inspection_No desc", con)
            dt1 = New DataTable
            da.Fill(dt1)
            If dt1.Rows.Count > 0 Then
                If Val(Common_Procedures.settings.PreviousEntryDate_ByDefault) = 1 Then '---- M.S Textiles (Tirupur)
                    If dt1.Rows(0).Item("JobWork_Piece_Inspection_Date").ToString <> "" Then msk_Date.Text = dt1.Rows(0).Item("JobWork_Piece_Inspection_Date").ToString
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
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim movno As String, inpno As String
        Dim RecCode As String

        Try

            inpno = InputBox("Enter Checking.No.", "FOR FINDING...")

            RecCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

            Da = New SqlClient.SqlDataAdapter("select JobWork_Piece_Inspection_No from JobWork_Piece_Inspection_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and JobWork_Piece_Inspection_Code = '" & Trim(RecCode) & "'", con)
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
                MessageBox.Show("Checking No. does not exists", "DOES NOT FIND...", MessageBoxButtons.OK, MessageBoxIcon.Error)

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

        '  If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.JobWork_Inspection_Entry, "~L~") = 0 And InStr(Common_Procedures.UR.JobWork_Inspection_Entry, "~I~") = 0 Then MessageBox.Show("You have No Rights to Insert", "DOES NOT INSERT...", MessageBoxButtons.OK, MessageBoxIcon.Error) : Exit Sub

        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.InsertingEntry, Common_Procedures.UR.Jobwork_Piece_Inspection_Entry, New_Entry, Me) = False Then Exit Sub



        Try

            inpno = InputBox("Enter New Dc No.", "FOR NEW DC INSERTION...")

            RecCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

            Da = New SqlClient.SqlDataAdapter("select JobWork_Piece_Inspection_No from JobWork_Piece_Inspection_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and JobWork_Piece_Inspection_Code = '" & Trim(RecCode) & "'", con)
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
                    MessageBox.Show("Invalid Dc No", "DOES NOT INSERT NEW DC...", MessageBoxButtons.OK, MessageBoxIcon.Error)

                Else
                    new_record()
                    Insert_Entry = True
                    lbl_CheckingNo.Text = Trim(UCase(inpno))

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
        Dim Led_id As Integer = 0
        Dim Clo_id As Integer = 0
        Dim EdsCnt_id As Integer
        Dim Cnt_id As Integer = 0
        Dim Sno As Integer = 0
        Dim Partcls As String = ""
        Dim PBlNo As String = ""
        Dim vTotRls As Single, vTotDcMtrs As Single, vTotActMtrs As Single, vTotMtrs1 As Single, vTotMtrs2 As Single
        Dim vTotMtrs3 As Single, vTotMtrs4 As Single, vTotMtrs5 As Single, vNtTtMtrs As Single, vTotExsShtMtrs As Single
        Dim Nr As Long
        Dim consyarn As Single = 0
        Dim PavuConsMtrs As Single = 0
        Dim Lm_ID As Integer = 0
        Dim vWdth_Typ As String = ""

        Dim vOrdByNo As String = ""

        vOrdByNo = Common_Procedures.OrderBy_CodeToValue(lbl_DcNo.Text)

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_CheckingNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.SavingEntry, Common_Procedures.UR.Jobwork_Piece_Inspection_Entry, New_Entry, Me, con, "JobWork_Piece_Inspection_Head", "JobWork_Piece_Inspection_Code", NewCode, "JobWork_Piece_Inspection_Date", "(JobWork_Piece_Inspection_Code = '" & Trim(NewCode) & "')", "(Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and JobWork_Piece_Inspection_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "')", "for_Orderby desc, JobWork_Piece_Inspection_No desc", dtp_Date.Value.Date) = False Then Exit Sub


        If Val(lbl_Company.Tag) = 0 Then
            MessageBox.Show("Invalid Company Selection", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        '  If Common_Procedures.UserRight_Check(Common_Procedures.UR.JobWork_Inspection_Entry, New_Entry) = False Then Exit Sub

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

        led_id = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Ledger.Text)
        If led_id = 0 Then
            MessageBox.Show("Invalid Party Name", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If cbo_Ledger.Enabled And cbo_Ledger.Visible Then cbo_Ledger.Focus()
            Exit Sub
        End If

        Clo_id = Common_Procedures.Cloth_NameToIdNo(con, lbl_ClothName.Text)
        If Clo_id = 0 Then
            MessageBox.Show("Invalid Cloth Name", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If cbo_Ledger.Enabled And cbo_Ledger.Visible Then cbo_Ledger.Focus()
            Exit Sub
        End If

        EdsCnt_id = Common_Procedures.EndsCount_NameToIdNo(con, lbl_EndsCount.Text)
        If EdsCnt_id = 0 Then
            MessageBox.Show("Invalid EndsCount", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If cbo_Ledger.Enabled And cbo_Ledger.Visible Then cbo_Ledger.Focus()
            Exit Sub
        End If

        Cnt_id = Common_Procedures.Count_NameToIdNo(con, lbl_WeftCount.Text)
        If Cnt_id = 0 Then
            MessageBox.Show("Invalid Count Name", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If cbo_Ledger.Enabled And cbo_Ledger.Visible Then cbo_Ledger.Focus()
            Exit Sub
        End If

        For i = 0 To dgv_Details.RowCount - 1

            If Trim(dgv_Details.Rows(i).Cells(1).Value) <> "" And Val(dgv_Details.Rows(i).Cells(2).Value) <> 0 Then

                If Val(dgv_Details.Rows(i).Cells(3).Value) <= 0 Then
                    MessageBox.Show("Invalid Actual Meters", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    dgv_Details.Focus()
                    dgv_Details.CurrentCell = dgv_Details.Rows(i).Cells(2)
                    dgv_Details.CurrentCell.Selected = True
                    Exit Sub
                End If

                If Val(dgv_Details.Rows(i).Cells(4).Value) <= 0 And Val(dgv_Details.Rows(i).Cells(5).Value) <= 0 And Val(dgv_Details.Rows(i).Cells(6).Value) <= 0 And Val(dgv_Details.Rows(i).Cells(7).Value) <= 0 And Val(dgv_Details.Rows(i).Cells(8).Value) <= 0 Then
                    MessageBox.Show("Invalid Checking Meters", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    dgv_Details.Focus()
                    dgv_Details.CurrentCell = dgv_Details.Rows(i).Cells(4)
                    dgv_Details.CurrentCell.Selected = True
                    Exit Sub
                End If

            End If

        Next

        NoCalc_Status = False
        Total_Calculation()

        vTotRls = 0 : vTotDcMtrs = 0 : vTotActMtrs = 0
        vTotMtrs1 = 0 : vTotMtrs2 = 0 : vTotMtrs3 = 0
        vTotMtrs4 = 0 : vTotMtrs5 = 0 : vNtTtMtrs = 0 : vTotExsShtMtrs = 0

        If dgv_Details_Total.RowCount > 0 Then
            vTotRls = Val(dgv_Details_Total.Rows(0).Cells(1).Value())
            vTotDcMtrs = Val(dgv_Details_Total.Rows(0).Cells(2).Value())
            vTotActMtrs = Val(dgv_Details_Total.Rows(0).Cells(3).Value())
            vTotMtrs1 = Val(dgv_Details_Total.Rows(0).Cells(4).Value())
            vTotMtrs2 = Val(dgv_Details_Total.Rows(0).Cells(5).Value())
            vTotMtrs3 = Val(dgv_Details_Total.Rows(0).Cells(6).Value())
            vTotMtrs4 = Val(dgv_Details_Total.Rows(0).Cells(7).Value())
            vTotMtrs5 = Val(dgv_Details_Total.Rows(0).Cells(8).Value())
            vNtTtMtrs = Val(dgv_Details_Total.Rows(0).Cells(9).Value())
            vTotExsShtMtrs = Val(dgv_Details_Total.Rows(0).Cells(14).Value())
        End If

        tr = con.BeginTransaction

        Try

            If Insert_Entry = True Or New_Entry = False Then
                NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_CheckingNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            Else

                lbl_CheckingNo.Text = Common_Procedures.get_MaxCode(con, "JobWork_Piece_Inspection_Head", "JobWork_Piece_Inspection_Code", "For_OrderBy", "", Val(lbl_Company.Tag), Common_Procedures.FnYearCode, tr)

                NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_CheckingNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            End If

            cmd.Connection = con
            cmd.Transaction = tr

            cmd.Parameters.Clear()
            cmd.Parameters.AddWithValue("@CheckingDate", Convert.ToDateTime(msk_Date.Text))
            cmd.Parameters.AddWithValue("@DcDate", Convert.ToDateTime(msk_DcDate.Text))

            If New_Entry = True Then

                cmd.CommandText = "Insert into JobWork_Piece_Inspection_Head(JobWork_Piece_Inspection_Code,               Company_IdNo       ,   JobWork_Piece_Inspection_No      ,                     for_OrderBy                                             , JobWork_Piece_Inspection_Date,          Ledger_IdNo    ,          JobWork_Delivery_Code ,      JobWork_Delivery_No     ,  JobWork_Delivery_Date,         Cloth_Idno      ,       EndsCount_IdNo       ,       Count_IdNo        ,               Remarks           ,          Total_Rolls     ,     Total_Delivery_Meters   ,      Total_Actual_Meters     ,  Total_ClothType1_Meters   ,   Total_ClothType2_Meters  ,   Total_ClothType3_Meters  ,  Total_ClothType4_Meters   , Total_ClothType5_Meters    , Total_Checking_Meters       ,             Party_Piece_Inspection_No             ,    Total_Shortage_Meters         ,                Folding            ,                  Delivery_Folding           , Rough_Consumed_Yarn ) " &
                                    "   Values                              (   '" & Trim(NewCode) & "'   , " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_CheckingNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_CheckingNo.Text))) & ",     @CheckingDate            , " & Str(Val(Led_id)) & ", '" & Trim(lbl_DcCode.Text) & "', '" & Trim(lbl_DcNo.Text) & "',          @DcDate      , " & Str(Val(Clo_id)) & ", " & Str(Val(EdsCnt_id)) & ", " & Str(Val(Cnt_id)) & ", '" & Trim(txt_Remarks.Text) & "', " & Str(Val(vTotRls)) & ", " & Str(Val(vTotDcMtrs)) & ", " & Str(Val(vTotActMtrs)) & ", " & Str(Val(vTotMtrs1)) & ", " & Str(Val(vTotMtrs2)) & ", " & Str(Val(vTotMtrs3)) & ", " & Str(Val(vTotMtrs4)) & ", " & Str(Val(vTotMtrs5)) & ", " & Str(Val(vNtTtMtrs)) & " ,'" & Trim(txt_Party_Piece_Inspection_No.Text) & "' , " & Str(Val(vTotExsShtMtrs)) & " , " & Str(Val(txt_Folding.Text)) & ", " & Str(Val(lbl_Delivery_Folding.Text)) & " , " & Str(Val(lbl_ConsYarn.Text)) & " ) "
                cmd.ExecuteNonQuery()

            Else
                Call Common_Procedures.User_Modification_Updation(con, "HEAD", Me.Name, "OLD", "JobWork_Piece_Inspection_Head", "JobWork_Piece_Inspection_Code", Val(lbl_Company.Tag), NewCode, lbl_DcNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, False, "", "", "JobWork_Piece_Inspection_Code, Company_IdNo, for_OrderBy", tr)

                '    Call Common_Procedures.User_Modification_Updation(con, "DETAILS", Me.Name, "OLD", "JobWork_Piece_Inspection_Details", "JobWork_Piece_Inspection_Code", Val(lbl_Company.Tag), NewCode, lbl_DcNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, False, "count_idno, Yarn_Type, Mill_IdNo, Bags, Cones, Weight ", "Sl_No", "JobWork_Piece_Inspection_Code, For_OrderBy, Company_IdNo, JobWork_Piece_Inspection_No, JobWork_Piece_Inspection_Date, Ledger_Idno", tr)

                Da = New SqlClient.SqlDataAdapter("select * from JobWork_Piece_Delivery_Head Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and JobWork_Inspection_Code = '" & Trim(NewCode) & "'", con)
                Da.SelectCommand.Transaction = tr
                Dt1 = New DataTable
                Da.Fill(Dt1)
                If Dt1.Rows.Count > 0 Then
                    If IsDBNull(Dt1.Rows(0).Item("JobWork_Bill_Code").ToString) = False Then
                        If Trim(Dt1.Rows(0).Item("JobWork_Bill_Code").ToString) <> "" Then
                            Throw New ApplicationException("Already Conversion Bil prepared")
                            Exit Sub
                        End If
                    End If
                End If
                Dt1.Clear()

                cmd.CommandText = "truncate table TempTable_For_Jobwork_Inspection_Stock_Posting"
                cmd.ExecuteNonQuery()

                cmd.CommandText = "Insert into TempTable_For_Jobwork_Inspection_Stock_Posting(JobWork_Piece_Delivery_Code, Entry_PkCondition, Lot_Code, Pcs_No, Cloth_Idno, Meters) select a.JobWork_Piece_Delivery_Code, a.Entry_PkCondition, a.Lot_Code, a.Pcs_No, b.Cloth_Idno, Meters from JobWork_Piece_Delivery_Details a INNER JOIN JobWork_Piece_Delivery_Head b ON a.Company_IdNo = b.Company_IdNo and a.JobWork_Piece_Delivery_Code = b.JobWork_Piece_Delivery_Code where a.JobWork_Inspection_Code = '" & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()

                cmd.CommandText = "Update JobWork_Piece_Inspection_Head set JobWork_Piece_Inspection_Date = @CheckingDate, Ledger_IdNo = " & Str(Val(Led_id)) & ", JobWork_Delivery_Code = '" & Trim(lbl_DcCode.Text) & "', JobWork_Delivery_No = '" & Trim(lbl_DcNo.Text) & "', JobWork_Delivery_Date = @DcDate, Cloth_Idno = " & Str(Val(Clo_id)) & ", EndsCount_IdNo = " & Str(Val(EdsCnt_id)) & ", Count_IdNo = " & Str(Val(Cnt_id)) & ", Remarks = '" & Trim(txt_Remarks.Text) & "', Total_Rolls = " & Str(Val(vTotRls)) & ", Total_Delivery_Meters = " & Str(Val(vTotDcMtrs)) & ", Total_Actual_Meters = " & Str(Val(vTotActMtrs)) & ", Total_ClothType1_Meters = " & Str(Val(vTotMtrs1)) & ", Total_ClothType2_Meters = " & Str(Val(vTotMtrs2)) & ", Total_ClothType3_Meters = " & Str(Val(vTotMtrs3)) & ", Total_ClothType4_Meters = " & Str(Val(vTotMtrs4)) & ", Total_ClothType5_Meters = " & Str(Val(vTotMtrs5)) & ", Total_Checking_Meters = " & Str(Val(vNtTtMtrs)) & ",Party_Piece_Inspection_No = '" & Trim(txt_Party_Piece_Inspection_No.Text) & "', Total_Shortage_Meters = " & Str(Val(vTotExsShtMtrs)) & " , Folding = " & Str(Val(txt_Folding.Text)) & " , Delivery_Folding = " & Str(Val(lbl_Delivery_Folding.Text)) & " , Rough_Consumed_Yarn = " & Str(Val(lbl_ConsYarn.Text)) & " Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and JobWork_Piece_Inspection_Code = '" & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()

                cmd.CommandText = "Update JobWork_Piece_Delivery_Details set JobWork_Inspection_Code = '', JobWork_Inspection_Increment = JobWork_Inspection_Increment - 1, JobWork_Inspection_Date = Null, Actual_Meters = 0, Cloth_Type1_Meters = 0, Cloth_Type2_Meters = 0, Cloth_Type3_Meters = 0, Cloth_Type4_Meters = 0, Cloth_Type5_Meters = 0, Total_Checking_Meters = 0 Where JobWork_Inspection_Code = '" & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()

                cmd.CommandText = "Update JobWork_Piece_Delivery_Head set JobWork_Inspection_Code = '', JobWork_Inspection_Increment = JobWork_Inspection_Increment - 1, JobWork_Inspection_Date = Null, Total_Actual_Meters = 0, Total_ClothType1_Meters = 0, Total_ClothType2_Meters = 0, Total_ClothType3_Meters = 0, Total_ClothType4_Meters = 0, Total_ClothType5_Meters = 0, Total_Checking_Meters = 0 , Inspection_Folding = 0 Where JobWork_Inspection_Code = '" & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()

                If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1052" Then '---- Shri Vedha Tex (Karumanthapatti) - Nithya Sizing



                    Stock_Posting_Pavu_and_yarn(NewCode, tr)



                End If

            End If
            Call Common_Procedures.User_Modification_Updation(con, "HEAD", Me.Name, "NEW", "JobWork_Piece_Inspection_Head", "JobWork_Piece_Inspection_Code", Val(lbl_Company.Tag), NewCode, lbl_DcNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, False, "", "", "JobWork_Piece_Inspection_Code, Company_IdNo, for_OrderBy", tr)

            
            Nr = 0
            cmd.CommandText = "Update JobWork_Piece_Delivery_Head set JobWork_Inspection_Code = '" & Trim(NewCode) & "', JobWork_Inspection_Increment = JobWork_Inspection_Increment + 1, JobWork_Inspection_Date = @CheckingDate, Total_Actual_Meters = " & Str(Val(vTotActMtrs)) & ", Total_ClothType1_Meters = " & Str(Val(vTotMtrs1)) & ", Total_ClothType2_Meters = " & Str(Val(vTotMtrs2)) & ", Total_ClothType3_Meters = " & Str(Val(vTotMtrs3)) & ", Total_ClothType4_Meters = " & Str(Val(vTotMtrs4)) & ", Total_ClothType5_Meters = " & Str(Val(vTotMtrs5)) & ", Total_Checking_Meters = " & Str(Val(vNtTtMtrs)) & " , Inspection_Folding = " & Str(Val(txt_Folding.Text)) & " Where JobWork_Piece_Delivery_Code = '" & Trim(lbl_DcCode.Text) & "' and Ledger_IdNo = " & Str(Val(Led_id))
            Nr = cmd.ExecuteNonQuery()

            If Nr = 0 Then
                MessageBox.Show("Invalid Delivery Details - Mismatch of Delivery details and Party", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                tr.Rollback()
                If dtp_Date.Enabled And dtp_Date.Visible Then dtp_Date.Focus()
                Exit Sub
            End If

            For i = 0 To dgv_Details.RowCount - 1

                If Trim(dgv_Details.Rows(i).Cells(1).Value) <> "" And Val(dgv_Details.Rows(i).Cells(2).Value) <> 0 And Trim(dgv_Details.Rows(i).Cells(10).Value) <> "" Then

                    Nr = 0
                    cmd.CommandText = "Update JobWork_Piece_Delivery_Details set JobWork_Inspection_Code = '" & Trim(NewCode) & "', JobWork_Inspection_Increment = JobWork_Inspection_Increment + 1, JobWork_Inspection_Date = @CheckingDate, Actual_Meters = " & Str(Val(dgv_Details.Rows(i).Cells(3).Value)) & ", Cloth_Type1_Meters = " & Str(Val(dgv_Details.Rows(i).Cells(4).Value)) & ", Cloth_Type2_Meters = " & Str(Val(dgv_Details.Rows(i).Cells(5).Value)) & ", Cloth_Type3_Meters = " & Str(Val(dgv_Details.Rows(i).Cells(6).Value)) & ", Cloth_Type4_Meters = " & Str(Val(dgv_Details.Rows(i).Cells(7).Value)) & ", Cloth_Type5_Meters = " & Str(Val(dgv_Details.Rows(i).Cells(8).Value)) & ", Total_Checking_Meters = " & Str(Val(dgv_Details.Rows(i).Cells(9).Value)) & " , Shortage_Meters = " & Str(Val(dgv_Details.Rows(i).Cells(14).Value)) & " Where JobWork_Piece_Delivery_Code = '" & Trim(dgv_Details.Rows(i).Cells(10).Value) & "' and Lot_Code = '" & Trim(dgv_Details.Rows(i).Cells(12).Value) & "' and Pcs_No = '" & Trim(dgv_Details.Rows(i).Cells(13).Value) & "' and Ledger_IdNo = " & Str(Val(Led_id))
                    Nr = cmd.ExecuteNonQuery()

                    If Nr = 0 Then
                        MessageBox.Show("Invalid Roll Details - Mismatch of details", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        tr.Rollback()
                        If dtp_Date.Enabled And dtp_Date.Visible Then dtp_Date.Focus()
                        Exit Sub
                    End If

                End If

            Next


            If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1052" Then '---- Shri Vedha Tex (Karumanthapatti) - Nithya Sizing
                cmd.CommandText = "truncate table TempTable_For_Jobwork_Inspection_Stock_Posting"
                cmd.ExecuteNonQuery()

                cmd.CommandText = "Insert into TempTable_For_Jobwork_Inspection_Stock_Posting(JobWork_Piece_Delivery_Code, Entry_PkCondition, Lot_Code, Pcs_No, Cloth_Idno, Meters) select a.JobWork_Piece_Delivery_Code, a.Entry_PkCondition, a.Lot_Code, a.Pcs_No, b.Cloth_Idno, Meters from JobWork_Piece_Delivery_Details a INNER JOIN JobWork_Piece_Delivery_Head b ON a.Company_IdNo = b.Company_IdNo and a.JobWork_Piece_Delivery_Code = b.JobWork_Piece_Delivery_Code where a.JobWork_Inspection_Code = '" & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()

                lbl_ConsYarn.Text = ""
                Stock_Posting_Pavu_and_yarn(NewCode, tr)

                cmd.CommandText = "Update JobWork_Piece_Inspection_Head set Rough_Consumed_Yarn = " & Str(Val(lbl_ConsYarn.Text)) & " Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and JobWork_Piece_Inspection_Code = '" & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()

            End If

            tr.Commit()

            Dt1.Dispose()
            Da.Dispose()

            move_record(lbl_CheckingNo.Text)

            MessageBox.Show("Saved Sucessfully!!!", "FOR SAVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Information)

        Catch ex As Exception
            tr.Rollback()
            MessageBox.Show(ex.Message, "FOR SAVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally
            If msk_Date.Enabled And msk_Date.Visible Then msk_Date.Focus()

        End Try

    End Sub

    Private Sub dtp_Date_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles dtp_Date.GotFocus
        Grid_Cell_DeSelect()
    End Sub

    Private Sub dtp_Date_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dtp_Date.KeyDown
        If e.KeyCode = 40 Then e.Handled = True : SendKeys.Send("{TAB}")
        If e.KeyCode = 38 Then e.Handled = True : txt_Remarks.Focus() ' SendKeys.Send("+{TAB}")
    End Sub

    Private Sub dtp_Date_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dtp_Date.KeyPress
        If Asc(e.KeyChar) = 13 Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    

    Private Sub cbo_Ledger_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Ledger.KeyDown
        Try
            With cbo_Ledger
                If e.KeyValue = 38 And .DroppedDown = False Then
                    e.Handled = True
                    msk_Date.Focus()
                    'SendKeys.Send("+{TAB}")
                ElseIf e.KeyValue = 40 And .DroppedDown = False Then
                    e.Handled = True
                    If dgv_Details.Rows.Count > 0 Then
                        dgv_Details.Focus()
                        dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(4)
                        dgv_Details.CurrentCell.Selected = True

                    Else
                        txt_Remarks.Focus()

                    End If
                    'SendKeys.Send("{TAB}")
                ElseIf e.KeyValue <> 13 And e.KeyValue <> 17 And e.KeyValue <> 27 And .DroppedDown = False Then
                    .DroppedDown = True
                End If
            End With

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT SELECT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub cbo_Ledger_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Ledger.KeyPress
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim Condt As String
        Dim FindStr As String

        Try

            With cbo_Ledger

                If Asc(e.KeyChar) <> 27 Then

                    If Asc(e.KeyChar) = 13 Then

                        If Trim(.Text) <> "" Then
                            If .DroppedDown = True Then
                                If Trim(.SelectedText) <> "" Then
                                    .Text = .SelectedText

                                Else
                                    If .Items.Count > 0 Then
                                        .SelectedIndex = 0
                                        .SelectedItem = .Items(0)
                                        .Text = .GetItemText(.SelectedItem)
                                    End If

                                End If
                            End If
                        End If

                        If MessageBox.Show("Do you want to select delivery :", "FOR DELIVERY SELECTION...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = DialogResult.Yes Then
                            btn_Selection_Click(sender, e)

                        Else

                            If dgv_Details.Rows.Count > 0 Then
                                dgv_Details.Focus()
                                dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(4)
                                dgv_Details.CurrentCell.Selected = True

                            Else
                                txt_Remarks.Focus()

                            End If

                        End If

                    Else

                        Condt = ""
                        FindStr = ""

                        If Asc(e.KeyChar) = 8 Then
                            If .SelectionStart <= 1 Then
                                .Text = ""
                            End If

                            If Trim(.Text) <> "" Then
                                If .SelectionLength = 0 Then
                                    FindStr = .Text.Substring(0, .Text.Length - 1)
                                Else
                                    FindStr = .Text.Substring(0, .SelectionStart - 1)
                                End If
                            End If

                        Else
                            If .SelectionLength = 0 Then
                                FindStr = .Text & e.KeyChar
                            Else
                                FindStr = .Text.Substring(0, .SelectionStart) & e.KeyChar
                            End If

                        End If

                        FindStr = LTrim(FindStr)

                        Condt = "(ledger_idno = 0 or Ledger_Type = 'JOBWORKER' )"
                        If Trim(FindStr) <> "" Then
                            Condt = " Ledger_Type = 'JOBWORKER' and (Ledger_DisplayName like '" & FindStr & "%' or Ledger_DisplayName like '% " & FindStr & "%') "
                        End If

                        da = New SqlClient.SqlDataAdapter("select Ledger_DisplayName from Ledger_AlaisHead where " & Condt & " Order by Ledger_DisplayName", con)
                        da.Fill(dt)

                        .DataSource = dt
                        .DisplayMember = "Ledger_DisplayName"

                        .Text = FindStr

                        .SelectionStart = FindStr.Length

                        e.Handled = True

                    End If

                End If

            End With

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT SELECT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

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
        Dim Led_IdNo As Integer, Cnt_IdNo As Integer
        Dim Condt As String = ""

        Try

            Condt = ""
            Led_IdNo = 0
            Cnt_IdNo = 0

            If IsDate(dtp_Filter_Fromdate.Value) = True And IsDate(dtp_Filter_ToDate.Value) = True Then
                Condt = "a.JobWork_Piece_Inspection_Date between '" & Trim(Format(dtp_Filter_Fromdate.Value, "MM/dd/yyyy")) & "' and '" & Trim(Format(dtp_Filter_ToDate.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_Filter_Fromdate.Value) = True Then
                Condt = "a.JobWork_Piece_Inspection_Date = '" & Trim(Format(dtp_Filter_Fromdate.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_Filter_ToDate.Value) = True Then
                Condt = "a.JobWork_Piece_Inspection_Date = '" & Trim(Format(dtp_Filter_ToDate.Value, "MM/dd/yyyy")) & "' "
            End If

            If Trim(cbo_Filter_PartyName.Text) <> "" Then
                Led_IdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Filter_PartyName.Text)
            End If

            If Trim(cbo_Filter_ClothName.Text) <> "" Then
                Cnt_IdNo = Common_Procedures.Count_NameToIdNo(con, cbo_Filter_ClothName.Text)
            End If



            If Val(Led_IdNo) <> 0 Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & "a.Ledger_IdNo = " & Str(Val(Led_IdNo))
            End If

            If Val(Cnt_IdNo) <> 0 Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " a.JobWork_Piece_Inspection_Code IN (select z1.JobWork_Piece_Inspection_Code from JobWork_Production_Head z1 where z1.Count_IdNo = " & Str(Val(Cnt_IdNo)) & ")"
            End If

            da = New SqlClient.SqlDataAdapter("select a.*, b.Ledger_Name from JobWork_Piece_Inspection_Head a INNER JOIN Ledger_Head b on a.Ledger_IdNo = b.Ledger_IdNo where a.company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.JobWork_Piece_Inspection_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "' " & IIf(Trim(Condt) <> "", " and ", "") & Condt & " Order by a.for_orderby, a.JobWork_Piece_Inspection_No", con)
            da.Fill(dt2)

            dgv_Filter_Details.Rows.Clear()

            If dt2.Rows.Count > 0 Then

                For i = 0 To dt2.Rows.Count - 1

                    n = dgv_Filter_Details.Rows.Add()

                    dgv_Filter_Details.Rows(n).Cells(0).Value = i + 1
                    dgv_Filter_Details.Rows(n).Cells(1).Value = dt2.Rows(i).Item("JobWork_Piece_Inspection_No").ToString
                    dgv_Filter_Details.Rows(n).Cells(2).Value = Format(Convert.ToDateTime(dt2.Rows(i).Item("JobWork_Piece_Inspection_Date").ToString), "dd-MM-yyyy")
                    dgv_Filter_Details.Rows(n).Cells(3).Value = dt2.Rows(i).Item("Ledger_Name").ToString
                    dgv_Filter_Details.Rows(n).Cells(4).Value = Val(dt2.Rows(i).Item("Total_Rolls").ToString)
                    dgv_Filter_Details.Rows(n).Cells(5).Value = Format(Val(dt2.Rows(i).Item("Total_Delivery_Meters").ToString), "########0.000")

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
            cbo_Filter_PartyName.Focus()
        End If
    End Sub

    Private Sub cbo_Filter_PartyName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Filter_PartyName.GotFocus
        With cbo_Filter_PartyName
            .BackColor = Color.lime
            .ForeColor = Color.Blue
            .SelectAll()
        End With
    End Sub

    Private Sub cbo_Filter_PartyName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Filter_PartyName.KeyDown
        Try
            With cbo_Filter_PartyName
                If e.KeyValue = 38 And .DroppedDown = False Then
                    e.Handled = True
                    dtp_Filter_ToDate.Focus()
                    'SendKeys.Send("+{TAB}")
                ElseIf e.KeyValue = 40 And .DroppedDown = False Then
                    e.Handled = True
                    cbo_Filter_ClothName.Focus()
                    'SendKeys.Send("{TAB}")
                ElseIf e.KeyValue <> 13 And .DroppedDown = False Then
                    .DroppedDown = True
                End If
            End With

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT SELECT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub cbo_Filter_PartyName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Filter_PartyName.KeyPress
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim Condt As String
        Dim FindStr As String

        With cbo_Filter_PartyName

            If Asc(e.KeyChar) = 13 Then

                If Trim(.Text) <> "" Then
                    If .DroppedDown = True Then
                        If Trim(.SelectedText) <> "" Then
                            .Text = .SelectedText
                        Else
                            If .Items.Count > 0 Then
                                .SelectedIndex = 0
                                .SelectedItem = .Items(0)
                                .Text = .GetItemText(.SelectedItem)
                            End If
                        End If
                    End If
                End If

                cbo_Filter_ClothName.Focus()

            Else

                Condt = ""
                FindStr = ""

                If Asc(e.KeyChar) = 8 Then
                    If .SelectionStart <= 1 Then
                        .Text = ""
                    End If

                    If Trim(.Text) <> "" Then
                        If .SelectionLength = 0 Then
                            FindStr = .Text.Substring(0, .Text.Length - 1)
                        Else
                            FindStr = .Text.Substring(0, .SelectionStart - 1)
                        End If
                    End If

                Else
                    If .SelectionLength = 0 Then
                        FindStr = .Text & e.KeyChar
                    Else
                        FindStr = .Text.Substring(0, .SelectionStart) & e.KeyChar
                    End If

                End If

                Condt = "(Ledger_IdNo = 0 or Ledger_Type = 'JOBWORKER' )"
                If Trim(FindStr) <> "" Then
                    Condt = " Ledger_Type = 'JOBWORKER' and (Ledger_DisplayName like '" & FindStr & "%' or Ledger_DisplayName like '% " & FindStr & "%') "
                End If

                da = New SqlClient.SqlDataAdapter("select Ledger_DisplayName from Ledger_AlaisHead where " & Condt & " order by Ledger_DisplayName", con)
                da.Fill(dt)

                .DataSource = dt
                .DisplayMember = "Ledger_DisplayName"

                .Text = Trim(FindStr)

                .SelectionStart = FindStr.Length

                e.Handled = True

            End If

        End With

    End Sub

    Private Sub cbo_Filter_ClothName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Filter_ClothName.GotFocus
        With cbo_Filter_ClothName
            .BackColor = Color.lime
            .ForeColor = Color.Blue
            .SelectionStart = 0
            .SelectionLength = .Text.Length
        End With
    End Sub

    Private Sub cbo_Filter_ClothName_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Filter_ClothName.LostFocus
        With cbo_Filter_ClothName
            .BackColor = Color.White
            .ForeColor = Color.Black
        End With
    End Sub

    Private Sub cbo_Filter_ClothName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Filter_ClothName.KeyDown
        Try
            With cbo_Filter_ClothName
                If e.KeyValue = 38 And .DroppedDown = False Then
                    e.Handled = True
                    cbo_Filter_PartyName.Focus()
                    'SendKeys.Send("+{TAB}")
                ElseIf e.KeyValue = 40 And .DroppedDown = False Then
                    e.Handled = True
                    btn_Filter_Show.Focus()
                    'SendKeys.Send("{TAB}")
                ElseIf e.KeyValue <> 13 And .DroppedDown = False Then
                    .DroppedDown = True
                End If
            End With

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT SELECT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub cbo_Filter_ClothName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Filter_ClothName.KeyPress
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim Condt As String
        Dim FindStr As String

        With cbo_Filter_ClothName

            If Asc(e.KeyChar) = 13 Then

                If Trim(.Text) <> "" Then
                    If .DroppedDown = True Then
                        If Trim(.SelectedText) <> "" Then
                            .Text = .SelectedText
                        Else
                            If .Items.Count > 0 Then
                                .SelectedIndex = 0
                                .SelectedItem = .Items(0)
                                .Text = .GetItemText(.SelectedItem)
                            End If
                        End If
                    End If
                End If

                btn_Filter_Show_Click(sender, e)

            Else

                Condt = ""
                FindStr = ""

                If Asc(e.KeyChar) = 8 Then
                    If .SelectionStart <= 1 Then
                        .Text = ""
                    End If

                    If Trim(.Text) <> "" Then
                        If .SelectionLength = 0 Then
                            FindStr = .Text.Substring(0, .Text.Length - 1)
                        Else
                            FindStr = .Text.Substring(0, .SelectionStart - 1)
                        End If
                    End If

                Else
                    If .SelectionLength = 0 Then
                        FindStr = .Text & e.KeyChar
                    Else
                        FindStr = .Text.Substring(0, .SelectionStart) & e.KeyChar
                    End If

                End If

                If Trim(FindStr) <> "" Then
                    Condt = " Where cloth_name like '" & Trim(FindStr) & "%' or cloth_name like '% " & Trim(FindStr) & "%' "
                End If

                da = New SqlClient.SqlDataAdapter("select cloth_name from Cloth_Head " & Condt & " order by cloth_name", con)
                da.Fill(dt)

                .DataSource = dt
                .DisplayMember = "cloth_name"

                .Text = Trim(FindStr)

                .SelectionStart = FindStr.Length

                e.Handled = True

            End If

        End With

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


    Private Sub cbo_Filter_PartyName_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Filter_PartyName.LostFocus
        With cbo_Filter_PartyName
            .BackColor = Color.White
            .ForeColor = Color.Black
        End With
    End Sub

    Private Sub dgv_Details_CellEndEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellEndEdit
        On Error Resume Next
        With dgv_Details
            If .Visible Then
                If .CurrentCell.ColumnIndex >= 2 And .CurrentCell.ColumnIndex <= 8 Then
                    Total_Calculation()
                End If
            End If
        End With
    End Sub

    Private Sub dgv_Details_CellLeave(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellLeave
        With dgv_Details
            If .CurrentCell.ColumnIndex >= 2 And .CurrentCell.ColumnIndex <= 9 Then
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

        Dim vEXSSHTMTRS As String

        If IsNothing(dgv_Details.CurrentCell) Then Exit Sub
        With dgv_Details
            If .Visible Then

                If e.ColumnIndex = 2 Or e.ColumnIndex = 9 Then

                    If Val(.Rows(e.RowIndex).Cells(9).Value) <> 0 Or Val(.Rows(e.RowIndex).Cells(2).Value) <> 0 Then

                        Dim vDELV_FOLDMTR As String = 0
                        Dim vDELV_FOLDPERC As String = 0
                        Dim vINS_FOLDPERC As String = 0

                        vDELV_FOLDPERC = Val(lbl_Delivery_Folding.Text)
                        If Val(vDELV_FOLDPERC) = 0 Then vDELV_FOLDPERC = 100
                        vINS_FOLDPERC = Val(txt_Folding.Text)
                        If Val(vINS_FOLDPERC) = 0 Then vINS_FOLDPERC = 100

                        vDELV_FOLDMTR = Format(Val(.Rows(e.RowIndex).Cells(2).Value) * Val(vDELV_FOLDPERC) / Val(vINS_FOLDPERC), "#########0.00")

                        .Rows(e.RowIndex).Cells(14).Value = Format(Val(.Rows(e.RowIndex).Cells(9).Value) - Val(vDELV_FOLDMTR), "#########0.00")

                    Else

                        .Rows(e.RowIndex).Cells(14).Value = ""
                    End If


                End If


                    If .CurrentCell.ColumnIndex >= 2 And .CurrentCell.ColumnIndex <= 8 Then
                    Total_Calculation()
                End If

            End If

        End With
    End Sub

    Private Sub dgv_Details_EditingControlShowing(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewEditingControlShowingEventArgs) Handles dgv_Details.EditingControlShowing
        dgtxt_Details = CType(dgv_Details.EditingControl, DataGridViewTextBoxEditingControl)
    End Sub

    Private Sub dgtxt_Details_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgtxt_Details.Enter
        dgv_Details.EditingControl.BackColor = Color.Lime
    End Sub

    Private Sub dgtxt_Details_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dgtxt_Details.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
            e.Handled = True
        End If
    End Sub

    Private Sub dgv_Details_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_Details.KeyDown
        On Error Resume Next

        With dgv_Details

            If e.KeyCode = Keys.Up Then
                If .CurrentCell.RowIndex = 0 Then
                    .CurrentCell.Selected = False
                    e.SuppressKeyPress = True
                    e.Handled = True
                    cbo_Ledger.Focus()
                End If
            End If

            If e.KeyCode = Keys.Down Then
                If .CurrentCell.RowIndex = .RowCount - 1 Then
                    .CurrentCell.Selected = False
                    e.SuppressKeyPress = True
                    e.Handled = True
                    txt_Remarks.Focus()
                End If
            End If

        End With

    End Sub

    Private Sub dgv_Details_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_Details.KeyUp
        Dim n As Integer

        If e.Control = True And UCase(Chr(e.KeyCode)) = "D" Then

            With dgv_Details

                NoCalc_Status = True

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

            NoCalc_Status = False
            Total_Calculation()

        End If

    End Sub

    Private Sub cbo_Ledger_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Ledger.KeyUp
        If e.Control = False And e.KeyValue = 17 Then
            Common_Procedures.MDI_LedType = "JOBWORKER"
            Dim f As New Ledger_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Ledger.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()
        End If
    End Sub

    Private Sub txt_Remarks_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_Remarks.GotFocus
       
        Grid_Cell_DeSelect()
    End Sub

    Private Sub txt_Remarks_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_Remarks.KeyDown
        'If e.KeyCode = 40 Then btn_save.Focus() ' SendKeys.Send("{TAB}")

        If e.KeyCode = 40 Then
            txt_Party_Piece_Inspection_No.Focus()
        End If

        If e.KeyCode = 38 Then
            If dgv_Details.Rows.Count > 0 Then
                dgv_Details.Focus()
                dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(3)
                dgv_Details.CurrentCell.Selected = True

            Else
                cbo_Ledger.Focus()

            End If
        End If
    End Sub

    Private Sub txt_Remarks_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Remarks.KeyPress
        If Asc(e.KeyChar) = 13 Then
            '    If MessageBox.Show("Do you want to save ?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = DialogResult.Yes Then
            '        save_record()
            '    Else
            '        msk_Date.Focus()
            '    End If
            txt_Party_Piece_Inspection_No.Focus()
        End If




    End Sub

    Private Sub Total_Calculation()
        Dim Sno As Integer
        Dim TotRls As Single, TotDcMtrs As Single, TotActMtrs As Single
        Dim TotMtrs1 As Single, TotMtrs2 As Single, TotMtrs3 As Single
        Dim TotMtrs4 As Single, TotMtrs5 As Single, NtTtMtrs As Single
        Dim TotExsshtmtrs As Single

        If NoCalc_Status = True Then Exit Sub

        Sno = 0
        TotRls = 0
        TotDcMtrs = 0 : TotActMtrs = 0
        TotMtrs1 = 0 : TotMtrs2 = 0 : TotMtrs3 = 0
        TotMtrs4 = 0 : TotMtrs5 = 0 : NtTtMtrs = 0 : TotExsshtmtrs = 0
        With dgv_Details
            For i = 0 To .RowCount - 1
                Sno = Sno + 1
                .Rows(i).Cells(0).Value = Sno
                If Trim(.Rows(i).Cells(1).Value) <> "" And Val(.Rows(i).Cells(2).Value) <> 0 Then

                    .Rows(i).Cells(9).Value = Format(Val(.Rows(i).Cells(4).Value) + Val(.Rows(i).Cells(5).Value) + Val(.Rows(i).Cells(6).Value) + Val(.Rows(i).Cells(7).Value) + Val(.Rows(i).Cells(8).Value), "#########0.00")
                    .Rows(i).Cells(3).Value = Format(Val(.Rows(i).Cells(9).Value), "#########0.00")

                    TotRls = TotRls + 1
                    TotDcMtrs = TotDcMtrs + Val(.Rows(i).Cells(2).Value)
                    TotActMtrs = TotActMtrs + Val(.Rows(i).Cells(3).Value)
                    TotMtrs1 = TotMtrs1 + Val(.Rows(i).Cells(4).Value)
                    TotMtrs2 = TotMtrs2 + Val(.Rows(i).Cells(5).Value)
                    TotMtrs3 = TotMtrs3 + Val(.Rows(i).Cells(6).Value)
                    TotMtrs4 = TotMtrs4 + Val(.Rows(i).Cells(7).Value)
                    TotMtrs5 = TotMtrs5 + Val(.Rows(i).Cells(8).Value)
                    NtTtMtrs = NtTtMtrs + Val(.Rows(i).Cells(9).Value)
                    TotExsshtmtrs = TotExsshtmtrs + Val(.Rows(i).Cells(14).Value)

                End If

            Next

        End With

        With dgv_Details_Total
            If .RowCount = 0 Then .Rows.Add()
            .Rows(0).Cells(1).Value = Val(TotRls)
            .Rows(0).Cells(2).Value = Format(Val(TotDcMtrs), "########0.00")
            .Rows(0).Cells(3).Value = Format(Val(TotActMtrs), "########0.00")
            .Rows(0).Cells(4).Value = Format(Val(TotMtrs1), "########0.00")
            .Rows(0).Cells(5).Value = Format(Val(TotMtrs2), "########0.00")
            .Rows(0).Cells(6).Value = Format(Val(TotMtrs3), "########0.00")
            .Rows(0).Cells(7).Value = Format(Val(TotMtrs4), "########0.00")
            .Rows(0).Cells(8).Value = Format(Val(TotMtrs5), "########0.00")
            .Rows(0).Cells(9).Value = Format(Val(NtTtMtrs), "########0.00")
            .Rows(0).Cells(14).Value = Format(Val(TotExsshtmtrs), "########0.00")
        End With

    End Sub

    Private Sub btn_save_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_save.Click
        save_record()
    End Sub

    Private Sub btn_close_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_close.Click
        Me.Close()
    End Sub

    Private Sub btn_Selection_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Selection.Click
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim i As Integer, j As Integer, n As Integer, SNo As Integer
        Dim LedIdNo As Integer
        Dim NewCode As String

        LedIdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Ledger.Text)

        If LedIdNo = 0 Then
            MessageBox.Show("Invalid Party Name", "DOES NOT SELECT PIECE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If cbo_Ledger.Enabled And cbo_Ledger.Visible Then cbo_Ledger.Focus()
            Exit Sub
        End If

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_CheckingNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        With dgv_Selection

            .Rows.Clear()

            SNo = 0

            Da = New SqlClient.SqlDataAdapter("select a.*, b.Cloth_Name from JobWork_Piece_Delivery_Head a INNER JOIN Cloth_Head b ON a.Cloth_IdNo <> 0 and a.Cloth_IdNo = b.Cloth_IdNo Where a.JobWork_Inspection_Code = '" & Trim(NewCode) & "' and a.ledger_Idno = " & Str(Val(LedIdNo)) & " order by a.JobWork_Piece_Delivery_Date, a.for_orderby, a.JobWork_Piece_Delivery_No", con)
            Dt1 = New DataTable
            Da.Fill(Dt1)

            If Dt1.Rows.Count > 0 Then

                For i = 0 To Dt1.Rows.Count - 1

                    n = .Rows.Add()

                    SNo = SNo + 1
                    .Rows(n).Cells(0).Value = Val(SNo)
                    .Rows(n).Cells(1).Value = Dt1.Rows(i).Item("JobWork_Piece_Delivery_No").ToString
                    .Rows(n).Cells(2).Value = Format(Convert.ToDateTime(Dt1.Rows(i).Item("JobWork_Piece_Delivery_Date").ToString), "dd-MM-yyyy").ToString
                    .Rows(n).Cells(3).Value = Dt1.Rows(i).Item("Cloth_Name").ToString
                    .Rows(n).Cells(4).Value = Val(Dt1.Rows(i).Item("Total_Rolls").ToString)
                    .Rows(n).Cells(5).Value = Format(Val(Dt1.Rows(i).Item("Total_Delivery_Meters").ToString), "#########0.00")
                    .Rows(n).Cells(6).Value = "1"
                    .Rows(n).Cells(7).Value = Dt1.Rows(i).Item("JobWork_Piece_Delivery_Code").ToString

                    For j = 0 To .ColumnCount - 1
                        .Rows(i).Cells(j).Style.ForeColor = Color.Red
                    Next

                Next

            End If
            Dt1.Clear()

            Da = New SqlClient.SqlDataAdapter("select a.*, b.Cloth_Name from JobWork_Piece_Delivery_Head a INNER JOIN Cloth_Head b ON a.Cloth_IdNo = b.Cloth_IdNo Where a.JobWork_Inspection_Code = '' and a.InHouse_Piece_Transfer_Code = '' and a.ledger_Idno = " & Str(Val(LedIdNo)) & " order by a.JobWork_Piece_Delivery_Date, a.for_orderby, a.JobWork_Piece_Delivery_No", con)
            Dt1 = New DataTable
            Da.Fill(Dt1)

            If Dt1.Rows.Count > 0 Then

                For i = 0 To Dt1.Rows.Count - 1

                    n = .Rows.Add()

                    SNo = SNo + 1
                    .Rows(n).Cells(0).Value = Val(SNo)
                    .Rows(n).Cells(1).Value = Dt1.Rows(i).Item("JobWork_Piece_Delivery_No").ToString
                    .Rows(n).Cells(2).Value = Format(Convert.ToDateTime(Dt1.Rows(i).Item("JobWork_Piece_Delivery_Date").ToString), "dd-MM-yyyy").ToString
                    .Rows(n).Cells(3).Value = Dt1.Rows(i).Item("Cloth_Name").ToString
                    .Rows(n).Cells(4).Value = Val(Dt1.Rows(i).Item("Total_Rolls").ToString)
                    .Rows(n).Cells(5).Value = Format(Val(Dt1.Rows(i).Item("Total_Delivery_Meters").ToString), "#########0.00")
                    .Rows(n).Cells(6).Value = ""
                    .Rows(n).Cells(7).Value = Dt1.Rows(i).Item("JobWork_Piece_Delivery_Code").ToString

                Next

            End If
            Dt1.Clear()

        End With

        pnl_Selection.Visible = True
        pnl_Back.Enabled = False
        dgv_Selection.Focus()

    End Sub

    Private Sub dgv_Selection_CellClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Selection.CellClick
        Select_Dc(e.RowIndex)
    End Sub

    Private Sub Select_Dc(ByVal RwIndx As Integer)
        Dim i As Integer

        With dgv_Selection

            If .RowCount > 0 And RwIndx >= 0 Then

                .Rows(RwIndx).Cells(6).Value = (Val(.Rows(RwIndx).Cells(6).Value) + 1) Mod 2

                If Val(.Rows(RwIndx).Cells(6).Value) = 0 Then .Rows(RwIndx).Cells(6).Value = ""


                If Val(.Rows(RwIndx).Cells(6).Value) = 0 Then
                    For i = 0 To .ColumnCount - 1
                        .Rows(RwIndx).Cells(i).Style.ForeColor = Color.Black
                    Next

                Else
                    For i = 0 To .ColumnCount - 1
                        .Rows(RwIndx).Cells(i).Style.ForeColor = Color.Red
                    Next

                End If

            End If

        End With

    End Sub

    Private Sub dgv_Selection_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_Selection.KeyDown
        On Error Resume Next

        If e.KeyCode = Keys.Enter Or e.KeyCode = Keys.Space Then
            If dgv_Selection.CurrentCell.RowIndex >= 0 Then
                e.Handled = True
                Select_Dc(dgv_Selection.CurrentCell.RowIndex)
            End If
        End If
    End Sub


    Private Sub btn_Close_Selection_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Close_Selection.Click
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim i As Integer, j As Integer, n As Integer
        Dim sno As Integer
        Dim EdsCnt_ID As Integer = 0
        Dim WftCnt_ID As Integer = 0
        Dim Clo_ID As Integer = 0
        Dim ConsYarn As String = 0

        pnl_Back.Enabled = True

        lbl_DcNo.Text = ""
        lbl_DcCode.Text = ""
        msk_DcDate.Text = ""
        lbl_ClothName.Text = ""
        lbl_EndsCount.Text = ""
        lbl_WeftCount.Text = ""

        dgv_Details.Rows.Clear()

        If Val(txt_Folding.Text) = 0 Then txt_Folding.Text = 100

        NoCalc_Status = True

        For i = 0 To dgv_Selection.RowCount - 1

            If Val(dgv_Selection.Rows(i).Cells(6).Value) = 1 Then

                lbl_DcNo.Text = dgv_Selection.Rows(i).Cells(1).Value
                lbl_DcCode.Text = dgv_Selection.Rows(i).Cells(7).Value
                msk_DcDate.Text = dgv_Selection.Rows(i).Cells(2).Value
                lbl_ClothName.Text = dgv_Selection.Rows(i).Cells(3).Value

                Da = New SqlClient.SqlDataAdapter("select a.* from JobWork_Piece_Delivery_Details a Where a.JobWork_Piece_Delivery_Code = '" & Trim(lbl_DcCode.Text) & "' order by a.JobWork_Piece_Delivery_Date, a.for_orderby, a.JobWork_Piece_Delivery_No", con)
                'Da = New SqlClient.SqlDataAdapter("select a.*, b.EndsCount_Name, c.count_name from JobWork_Piece_Delivery_Details a LEFT OUTER JOIN EndsCount_Head b ON a.EndsCount_IdNo = b.EndsCount_IdNo LEFT OUTER JOIN Count_Head c ON a.Count_IdNo = c.Count_IdNo Where a.JobWork_Piece_Delivery_Code = '" & Trim(lbl_DcCode.Text) & "' order by a.JobWork_Piece_Delivery_Date, a.for_orderby, a.JobWork_Piece_Delivery_No", con)
                Dt1 = New DataTable
                Da.Fill(Dt1)

                If Dt1.Rows.Count > 0 Then

                    If Trim(UCase(Dt1.Rows(0).Item("Entry_PkCondition").ToString)) = "JPROD-" Then
                        EdsCnt_ID = Val(Common_Procedures.get_FieldValue(con, "JobWork_Production_Head", "EndsCount_IdNo", "(JobWork_Production_Code = '" & Trim(Dt1.Rows(0).Item("Lot_Code").ToString) & "')"))
                        WftCnt_ID = Val(Common_Procedures.get_FieldValue(con, "JobWork_Production_Head", "Count_IdNo", "(JobWork_Production_Code = '" & Trim(Dt1.Rows(0).Item("Lot_Code").ToString) & "')"))

                    Else
                        EdsCnt_ID = Val(Common_Procedures.get_FieldValue(con, "Weaver_Cloth_Receipt_Head", "EndsCount_IdNo", "(Weaver_ClothReceipt_Code = '" & Trim(Dt1.Rows(0).Item("Lot_Code").ToString) & "')"))
                        WftCnt_ID = Val(Common_Procedures.get_FieldValue(con, "Weaver_Cloth_Receipt_Head", "Count_IdNo", "(Weaver_ClothReceipt_Code = '" & Trim(Dt1.Rows(0).Item("Lot_Code").ToString) & "')"))


                    End If

                    Clo_ID = Common_Procedures.Cloth_NameToIdNo(con, lbl_ClothName.Text)

                    If Val(EdsCnt_ID) = 0 Then
                        EdsCnt_ID = Val(Common_Procedures.get_FieldValue(con, "Cloth_EndsCount_Details", "EndsCount_IdNo", "(Cloth_Idno = " & Str(Clo_ID) & ")"))
                    End If

                    If Val(WftCnt_ID) = 0 Then
                        WftCnt_ID = Val(Common_Procedures.get_FieldValue(con, "Cloth_Head", "Cloth_WeftCount_IdNo", "(Cloth_Idno = " & Str(Clo_ID) & ")"))
                    End If



                    lbl_EndsCount.Text = Common_Procedures.EndsCount_IdNoToName(con, EdsCnt_ID)
                    lbl_WeftCount.Text = Common_Procedures.Count_IdNoToName(con, WftCnt_ID)
                    lbl_Delivery_Folding.Text = Val(Dt1.Rows(j).Item("Folding").ToString).ToString


                    For j = 0 To Dt1.Rows.Count - 1

                            n = dgv_Details.Rows.Add()

                            sno = sno + 1

                            dgv_Details.Rows(n).Cells(0).Value = Val(sno)
                            dgv_Details.Rows(n).Cells(1).Value = Dt1.Rows(j).Item("Lot_No").ToString & IIf(Trim(Dt1.Rows(j).Item("pcs_No").ToString) <> "", "-", "") & Dt1.Rows(j).Item("pcs_No").ToString
                            If Val(Dt1.Rows(j).Item("Meters").ToString) <> 0 Then dgv_Details.Rows(n).Cells(2).Value = Format(Val(Dt1.Rows(j).Item("Meters").ToString), "#########0.00")
                            If Val(Dt1.Rows(j).Item("Actual_Meters").ToString) <> 0 Then
                                dgv_Details.Rows(n).Cells(3).Value = Format(Val(Dt1.Rows(j).Item("Actual_Meters").ToString), "#########0.00")
                            Else
                                If Val(Dt1.Rows(j).Item("Meters").ToString) <> 0 Then dgv_Details.Rows(n).Cells(3).Value = Format(Val(Dt1.Rows(j).Item("Meters").ToString), "#########0.00")
                            End If


                            If Val(Dt1.Rows(j).Item("Total_Checking_Meters").ToString) <> 0 Then

                                If Val(Dt1.Rows(j).Item("Cloth_Type1_Meters").ToString) <> 0 Then
                                    dgv_Details.Rows(n).Cells(4).Value = Format(Val(Dt1.Rows(j).Item("Cloth_Type1_Meters").ToString), "#########0.00")
                                End If

                                If Val(Dt1.Rows(j).Item("Cloth_Type2_Meters").ToString) <> 0 Then
                                    dgv_Details.Rows(n).Cells(5).Value = Format(Val(Dt1.Rows(j).Item("Cloth_Type2_Meters").ToString), "#########0.00")
                                End If

                                If Val(Dt1.Rows(j).Item("Cloth_Type3_Meters").ToString) <> 0 Then
                                    dgv_Details.Rows(n).Cells(6).Value = Format(Val(Dt1.Rows(j).Item("Cloth_Type3_Meters").ToString), "#########0.00")
                                End If

                                If Val(Dt1.Rows(j).Item("Cloth_Type4_Meters").ToString) <> 0 Then
                                    dgv_Details.Rows(n).Cells(7).Value = Format(Val(Dt1.Rows(j).Item("Cloth_Type4_Meters").ToString), "#########0.00")
                                End If
                                If Val(Dt1.Rows(j).Item("Cloth_Type5_Meters").ToString) <> 0 Then
                                    dgv_Details.Rows(n).Cells(8).Value = Format(Val(Dt1.Rows(j).Item("Cloth_Type5_Meters").ToString), "#########0.00")
                                End If

                                If Val(Dt1.Rows(j).Item("Total_Checking_Meters").ToString) <> 0 Then
                                    dgv_Details.Rows(n).Cells(9).Value = Format(Val(Dt1.Rows(j).Item("Total_Checking_Meters").ToString), "#########0.00")
                                End If

                                If Val(Dt1.Rows(j).Item("Shortage_Meters").ToString) <> 0 Then
                                    dgv_Details.Rows(n).Cells(14).Value = Format(Val(Dt1.Rows(j).Item("Shortage_Meters").ToString), "#########0.00")
                                End If



                            Else

                                Dim vFOLDMTRS As String = 0
                                Dim vDELV_FOLDPERC As String = 0

                                vDELV_FOLDPERC = Val(Dt1.Rows(j).Item("Folding").ToString)
                                If Val(vDELV_FOLDPERC) = 0 Then vDELV_FOLDPERC = 100

                                vFOLDMTRS = Format(Val(Dt1.Rows(j).Item("Meters").ToString) * Val(vDELV_FOLDPERC) / Val(txt_Folding.Text), "#########0.00")

                                If Val(Dt1.Rows(j).Item("ClothType_IdNo").ToString) = 5 Then
                                    dgv_Details.Rows(n).Cells(8).Value = Format(Val(vFOLDMTRS), "#########0.00")
                                    'dgv_Details.Rows(n).Cells(8).Value = Format(Val(Dt1.Rows(j).Item("Meters").ToString), "#########0.00")

                                ElseIf Val(Dt1.Rows(j).Item("ClothType_IdNo").ToString) = 4 Then
                                    dgv_Details.Rows(n).Cells(7).Value = Format(Val(vFOLDMTRS), "#########0.00")
                                    'dgv_Details.Rows(n).Cells(7).Value = Format(Val(Dt1.Rows(j).Item("Meters").ToString), "#########0.00")

                                ElseIf Val(Dt1.Rows(j).Item("ClothType_IdNo").ToString) = 3 Then
                                    dgv_Details.Rows(n).Cells(6).Value = Format(Val(vFOLDMTRS), "#########0.00")
                                    'dgv_Details.Rows(n).Cells(6).Value = Format(Val(Dt1.Rows(j).Item("Meters").ToString), "#########0.00")

                                ElseIf Val(Dt1.Rows(j).Item("ClothType_IdNo").ToString) = 2 Then
                                    dgv_Details.Rows(n).Cells(5).Value = Format(Val(vFOLDMTRS), "#########0.00")
                                    'dgv_Details.Rows(n).Cells(5).Value = Format(Val(Dt1.Rows(j).Item("Meters").ToString), "#########0.00")

                                Else
                                    dgv_Details.Rows(n).Cells(4).Value = Format(Val(vFOLDMTRS), "#########0.00")
                                    'dgv_Details.Rows(n).Cells(4).Value = Format(Val(Dt1.Rows(j).Item("Meters").ToString), "#########0.00")

                                End If

                                dgv_Details.Rows(n).Cells(3).Value = Format(Val(vFOLDMTRS), "#########0.00")
                                'dgv_Details.Rows(n).Cells(3).Value = Format(Val(Dt1.Rows(j).Item("Meters").ToString), "#########0.00")

                                dgv_Details.Rows(n).Cells(9).Value = Format(Val(vFOLDMTRS), "#########0.00")
                                'dgv_Details.Rows(n).Cells(9).Value = Format(Val(Dt1.Rows(j).Item("Meters").ToString), "#########0.00")

                            End If

                            dgv_Details.Rows(n).Cells(10).Value = Dt1.Rows(j).Item("JobWork_Piece_Delivery_Code").ToString
                            dgv_Details.Rows(n).Cells(11).Value = Dt1.Rows(j).Item("Entry_PkCondition").ToString
                            dgv_Details.Rows(n).Cells(12).Value = Dt1.Rows(j).Item("Lot_Code").ToString
                            dgv_Details.Rows(n).Cells(13).Value = Dt1.Rows(j).Item("Pcs_No").ToString

                        Next j

                    End If

                    Exit For

            End If

        Next i

        NoCalc_Status = False

        Total_Calculation()

        Grid_Cell_DeSelect()

        pnl_Back.Enabled = True
        pnl_Selection.Visible = False
        If dgv_Details.Rows.Count > 0 Then
            dgv_Details.Focus()
            dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(4)
            dgv_Details.CurrentCell.Selected = True

        Else
            txt_Remarks.Focus()

        End If

      
    End Sub

    Private Sub dgv_Details_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgv_Details.LostFocus
        On Error Resume Next
        If Not IsNothing(dgv_Details.CurrentCell) Then dgv_Details.CurrentCell.Selected = False
    End Sub

    Public Sub print_record() Implements Interface_MDIActions.print_record
        '------- No Printing
    End Sub

    Public Sub Stock_Posting_Pavu_and_yarn(ByVal vPK_Code As String, Optional ByVal sqltr As SqlClient.SqlTransaction = Nothing)
        Dim Cmd As New SqlClient.SqlCommand
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Da1 As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim Dt2 As New DataTable
        Dim i As Integer = 0
        Dim Lm_ID As Integer = 0
        Dim vWdth_Typ As String = ""
        Dim vTotChk_Mtrs As String = 0
        Dim PavuConsMtrs As Single = 0
        Dim ConsYarn As Single = 0
        Dim vCalc_AutoLoom_JbWrk_PavuWidthWiseConsumption As Boolean
        Dim vDcCode As String = ""
        Dim vClo_ID As Integer = 0
        Dim vLm_IdNo As Integer = 0
        Dim vWdth_Type As String = ""
        Dim vCrmp_Perc As Single = 0
        Dim vEndsCnt_IdNo As Integer = 0
        Dim vPvuConsMtrs As Single = 0
        Dim vLed_type As String = ""
        Dim vDelv_ID As Integer = 0, vRec_ID As Integer = 0
        Dim vWftCnt_ID As Integer = 0
        Dim vConsYarn As Single = 0
        Dim vChk_Mtrs As Integer = 0
        Dim Nr As Long = 0
        Dim vWARPYARN_STOCK_POSTING_STS As String = 0
        Dim vCONSYARN_FORWARP As String = 0
        Dim vTOTCONSYARN As String = 0


        Dim v1STPC_FOLDPERC As String = "", vFOLDPERC As String = ""

        Cmd.Connection = con
        If IsNothing(sqltr) = False Then
            Cmd.Transaction = sqltr
        End If

        Cmd.CommandText = "Truncate table " & Trim(Common_Procedures.EntryTempSubTable) & ""
        Cmd.ExecuteNonQuery()

        v1STPC_FOLDPERC = 0
        If Common_Procedures.settings.JobWorker_Pavu_Yarn_Stock_Posting_IN_Production = 1 Then

            Da = New SqlClient.SqlDataAdapter("select JobWork_Piece_Delivery_Code, Entry_PkCondition, Lot_Code, Cloth_Idno, sum(Meters) from TempTable_For_Jobwork_Inspection_Stock_Posting where Lot_Code <> '' group by JobWork_Piece_Delivery_Code, Entry_PkCondition, Lot_Code, Cloth_Idno", con)
            If IsNothing(sqltr) = False Then
                Da.SelectCommand.Transaction = sqltr
            End If
            Dt1 = New DataTable
            Da.Fill(Dt1)

            If Dt1.Rows.Count > 0 Then

                For i = 0 To Dt1.Rows.Count - 1

                    vTotChk_Mtrs = 0
                    Lm_ID = 0
                    vWdth_Typ = ""
                    vFOLDPERC = 100

                    If Trim(UCase(Dt1.Rows(i).Item("Entry_PkCondition").ToString)) = "JPROD-" Then

                        Da1 = New SqlClient.SqlDataAdapter("Select a.Loom_IdNo, a.Width_Type, a.EndsCount_IdNo, a.Folding_Percentage from JobWork_Production_Head a Where a.JobWork_Production_Code = '" & Trim(Dt1.Rows(i).Item("Lot_Code").ToString) & "'", con)
                        Da1.SelectCommand.Transaction = sqltr
                        Dt2 = New DataTable
                        Da1.Fill(Dt2)
                        If Dt2.Rows.Count > 0 Then
                            vLm_IdNo = Val(Dt2.Rows(0).Item("Loom_IdNo").ToString)
                            vWdth_Type = Dt2.Rows(0).Item("Width_Type").ToString
                            vFOLDPERC = Val(Dt2.Rows(0).Item("Folding_Percentage").ToString)
                        End If
                        Dt2.Clear()
                        'Lm_ID = Val(Common_Procedures.get_FieldValue(con, "JobWork_Production_Head", "Loom_IdNo", "(JobWork_Production_Code = '" & Trim(Dt1.Rows(i).Item("Lot_Code").ToString) & "')", , sqltr))
                        'vWdth_Typ = Common_Procedures.get_FieldValue(con, "JobWork_Production_Head", "Width_Type", "(JobWork_Production_Code = '" & Trim(Dt1.Rows(i).Item("Lot_Code").ToString) & "')", , sqltr)

                        Cmd.CommandText = "truncate table " & Trim(Common_Procedures.ReportTempSubTable) & ""
                        Cmd.ExecuteNonQuery()

                        Cmd.CommandText = "Insert into " & Trim(Common_Procedures.ReportTempSubTable) & "(Meters1) select (case when (Cloth_Type1_Meters+Cloth_Type2_Meters+Cloth_Type3_Meters+Cloth_Type4_Meters+Cloth_Type5_Meters)  > 0 then (Cloth_Type1_Meters+Cloth_Type2_Meters+Cloth_Type3_Meters+Cloth_Type4_Meters+Cloth_Type5_Meters) else Meters end) from JobWork_Piece_Delivery_Details where Lot_Code = '" & Trim(Dt1.Rows(i).Item("Lot_Code").ToString) & "'"
                        Cmd.ExecuteNonQuery()

                        vTotChk_Mtrs = 0
                        Da = New SqlClient.SqlDataAdapter("select sum(Meters1) as Tot_Meters from " & Trim(Common_Procedures.ReportTempSubTable) & "", con)
                        If IsNothing(sqltr) = False Then
                            Da.SelectCommand.Transaction = sqltr
                        End If
                        Dt2 = New DataTable
                        Da.Fill(Dt2)
                        If Dt2.Rows.Count > 0 Then
                            If IsDBNull(Dt2.Rows(0).Item("Tot_Meters").ToString) = False Then
                                vTotChk_Mtrs = Val(Dt2.Rows(0).Item("Tot_Meters").ToString)
                            End If
                        End If
                        Dt2.Clear()

                    Else

                        Da1 = New SqlClient.SqlDataAdapter("Select Loom_IdNo, Width_Type, Crimp_Percentage, EndsCount_IdNo, Folding from Weaver_Cloth_Receipt_Head Where Weaver_ClothReceipt_Code = '" & Trim(Dt1.Rows(i).Item("Lot_Code").ToString) & "'", con)
                        Da1.SelectCommand.Transaction = sqltr
                        Dt2 = New DataTable
                        Da1.Fill(Dt2)
                        If Dt2.Rows.Count > 0 Then
                            vLm_IdNo = Val(Dt2.Rows(0).Item("Loom_IdNo").ToString)
                            vWdth_Type = Dt2.Rows(0).Item("Width_Type").ToString
                            vFOLDPERC = Val(Dt2.Rows(0).Item("Folding").ToString)
                        End If
                        Dt2.Clear()
                        'Lm_ID = Val(Common_Procedures.get_FieldValue(con, "Weaver_Cloth_Receipt_Head", "Loom_IdNo", "(Weaver_ClothReceipt_Code = '" & Trim(Dt1.Rows(i).Item("Lot_Code").ToString) & "')", , sqltr))
                        'vWdth_Typ = Common_Procedures.get_FieldValue(con, "Weaver_Cloth_Receipt_Head", "Width_Type", "(Weaver_ClothReceipt_Code = '" & Trim(Dt1.Rows(i).Item("Lot_Code").ToString) & "')", , sqltr)

                        Cmd.CommandText = "truncate table " & Trim(Common_Procedures.ReportTempSubTable) & ""
                        Cmd.ExecuteNonQuery()

                        Cmd.CommandText = "Insert into " & Trim(Common_Procedures.ReportTempSubTable) & "(Meters1) select Type1_Meters from Weaver_ClothReceipt_Piece_Details Where Lot_Code = '" & Trim(Dt1.Rows(i).Item("Lot_Code").ToString) & "' and (PackingSlip_Code_Type1 = '' or (PackingSlip_Code_Type1 <> '' and PackingSlip_Code_Type1 NOT LIKE 'JPCDC-%') )"
                        Cmd.ExecuteNonQuery()
                        Cmd.CommandText = "Insert into " & Trim(Common_Procedures.ReportTempSubTable) & "(Meters1) select Type2_Meters from Weaver_ClothReceipt_Piece_Details Where Lot_Code = '" & Trim(Dt1.Rows(i).Item("Lot_Code").ToString) & "' and (PackingSlip_Code_Type2 = '' or (PackingSlip_Code_Type2 <> '' and PackingSlip_Code_Type2 NOT LIKE 'JPCDC-%') )"
                        Cmd.ExecuteNonQuery()
                        Cmd.CommandText = "Insert into " & Trim(Common_Procedures.ReportTempSubTable) & "(Meters1) select Type3_Meters from Weaver_ClothReceipt_Piece_Details Where Lot_Code = '" & Trim(Dt1.Rows(i).Item("Lot_Code").ToString) & "' and (PackingSlip_Code_Type3 = '' or (PackingSlip_Code_Type3 <> '' and PackingSlip_Code_Type3 NOT LIKE 'JPCDC-%') )"
                        Cmd.ExecuteNonQuery()
                        Cmd.CommandText = "Insert into " & Trim(Common_Procedures.ReportTempSubTable) & "(Meters1) select Type4_Meters from Weaver_ClothReceipt_Piece_Details Where Lot_Code = '" & Trim(Dt1.Rows(i).Item("Lot_Code").ToString) & "' and (PackingSlip_Code_Type4 = '' or (PackingSlip_Code_Type4 <> '' and PackingSlip_Code_Type4 NOT LIKE 'JPCDC-%') )"
                        Cmd.ExecuteNonQuery()
                        Cmd.CommandText = "Insert into " & Trim(Common_Procedures.ReportTempSubTable) & "(Meters1) select Type5_Meters from Weaver_ClothReceipt_Piece_Details Where Lot_Code = '" & Trim(Dt1.Rows(i).Item("Lot_Code").ToString) & "' and (PackingSlip_Code_Type5 = '' or (PackingSlip_Code_Type5 <> '' and PackingSlip_Code_Type5 NOT LIKE 'JPCDC-%') )"
                        Cmd.ExecuteNonQuery()

                        Cmd.CommandText = "Insert into " & Trim(Common_Procedures.ReportTempSubTable) & "(Meters1) select (case when (Cloth_Type1_Meters+Cloth_Type2_Meters+Cloth_Type3_Meters+Cloth_Type4_Meters+Cloth_Type5_Meters) > 0 then (Cloth_Type1_Meters+Cloth_Type2_Meters+Cloth_Type3_Meters+Cloth_Type4_Meters+Cloth_Type5_Meters) else Meters end) from JobWork_Piece_Delivery_Details where Lot_Code = '" & Trim(Dt1.Rows(i).Item("Lot_Code").ToString) & "'"
                        Cmd.ExecuteNonQuery()

                        Da = New SqlClient.SqlDataAdapter("select sum(Meters1) as Tot_Meters from " & Trim(Common_Procedures.ReportTempSubTable) & "", con)
                        If IsNothing(sqltr) = False Then
                            Da.SelectCommand.Transaction = sqltr
                        End If
                        Dt2 = New DataTable
                        Da.Fill(Dt2)
                        If Dt2.Rows.Count > 0 Then
                            If IsDBNull(Dt2.Rows(0).Item("Tot_Meters").ToString) = False Then
                                vTotChk_Mtrs = Val(Dt2.Rows(0).Item("Tot_Meters").ToString)
                            End If
                        End If
                        Dt2.Clear()

                    End If


                    vFOLDPERC = Val(txt_Folding.Text)
                    If Val(vFOLDPERC) = 0 Then vFOLDPERC = 100
                    If Val(v1STPC_FOLDPERC) = 0 Then v1STPC_FOLDPERC = vFOLDPERC

                    vCalc_AutoLoom_JbWrk_PavuWidthWiseConsumption = True
                    If Common_Procedures.settings.JobWorker_PavuWidthWiseConsumption_IN_Delivery = 1 Then
                        vCalc_AutoLoom_JbWrk_PavuWidthWiseConsumption = False
                    End If

                    PavuConsMtrs = Common_Procedures.get_Pavu_Consumption(con, Val(Dt1.Rows(i).Item("Cloth_Idno").ToString), Lm_ID, Val(vTotChk_Mtrs), Trim(vWdth_Typ), sqltr, , , vCalc_AutoLoom_JbWrk_PavuWidthWiseConsumption, Val(vFOLDPERC))

                    Cmd.CommandText = "Update Stock_Pavu_Processing_Details set Meters = " & Str(Val(PavuConsMtrs)) & " Where Reference_Code = '" & Trim(Dt1.Rows(i).Item("Entry_PkCondition").ToString) & Trim(Dt1.Rows(i).Item("Lot_Code").ToString) & "'"
                    Cmd.ExecuteNonQuery()

                    ConsYarn = Common_Procedures.get_Weft_ConsumedYarn(con, Val(Dt1.Rows(i).Item("Cloth_Idno").ToString), Val(vTotChk_Mtrs), sqltr,, Val(vFOLDPERC))

                    Cmd.CommandText = "Update Stock_Yarn_Processing_Details set Weight = " & Str(Val(ConsYarn)) & " Where Reference_Code = '" & Trim(Dt1.Rows(i).Item("Entry_PkCondition").ToString) & Trim(Dt1.Rows(i).Item("Lot_Code").ToString) & "' and Sl_No < 100"
                    Cmd.ExecuteNonQuery()

                    vTOTCONSYARN = Val(vTOTCONSYARN) + Val(ConsYarn)


                Next

            End If
            Dt1.Clear()


        Else


            Da = New SqlClient.SqlDataAdapter("Select JobWork_Piece_Delivery_Code, Entry_PkCondition, Lot_Code, Pcs_No, Cloth_Idno, sum(Meters) as DcMeters from TempTable_For_Jobwork_Inspection_Stock_Posting Where Lot_Code <> '' group by JobWork_Piece_Delivery_Code, Entry_PkCondition, Lot_Code, Pcs_No, Cloth_Idno", con)
            If IsNothing(sqltr) = False Then
                Da.SelectCommand.Transaction = sqltr
            End If
            Dt1 = New DataTable
            Da.Fill(Dt1)

            If Dt1.Rows.Count > 0 Then

                vTotChk_Mtrs = 0
                vClo_ID = 0
                vDcCode = ""

                For i = 0 To Dt1.Rows.Count - 1

                    If Trim(Dt1.Rows(i).Item("Lot_Code").ToString) <> "" And Val(Dt1.Rows(i).Item("DcMeters").ToString) <> 0 Then

                        Cmd.CommandText = "truncate table " & Trim(Common_Procedures.ReportTempSubTable) & ""
                        Cmd.ExecuteNonQuery()

                        Cmd.CommandText = "Insert into " & Trim(Common_Procedures.ReportTempSubTable) & "(Meters1)  Select (case when (Cloth_Type1_Meters+Cloth_Type2_Meters+Cloth_Type3_Meters+Cloth_Type4_Meters+Cloth_Type5_Meters) > 0 then (Cloth_Type1_Meters+Cloth_Type2_Meters+Cloth_Type3_Meters+Cloth_Type4_Meters+Cloth_Type5_Meters) else Meters end) from JobWork_Piece_Delivery_Details Where JobWork_Piece_Delivery_Code = '" & Trim(Dt1.Rows(i).Item("JobWork_Piece_Delivery_Code").ToString) & "' and Lot_Code = '" & Trim(Dt1.Rows(i).Item("Lot_Code").ToString) & "'  and Pcs_No = '" & Trim(Dt1.Rows(i).Item("Pcs_No").ToString) & "'"
                        Cmd.ExecuteNonQuery()

                        vChk_Mtrs = 0
                        Da = New SqlClient.SqlDataAdapter("select sum(Meters1) as Tot_Meters from " & Trim(Common_Procedures.ReportTempSubTable) & "", con)
                        If IsNothing(sqltr) = False Then
                            Da.SelectCommand.Transaction = sqltr
                        End If
                        Dt2 = New DataTable
                        Da.Fill(Dt2)
                        If Dt2.Rows.Count > 0 Then
                            If IsDBNull(Dt2.Rows(0).Item("Tot_Meters").ToString) = False Then
                                vChk_Mtrs = Val(Dt2.Rows(0).Item("Tot_Meters").ToString)
                            End If
                        End If
                        Dt2.Clear()

                        vTotChk_Mtrs = Format(Val(vTotChk_Mtrs) + Val(vChk_Mtrs), "##########0.00")

                        vDcCode = Dt1.Rows(i).Item("JobWork_Piece_Delivery_Code").ToString
                        vClo_ID = Val(Dt1.Rows(i).Item("Cloth_Idno").ToString)

                        vLm_IdNo = 0
                        vWdth_Type = ""
                        vCrmp_Perc = 0
                        vEndsCnt_IdNo = 0
                        vFOLDPERC = 100

                        If Trim(UCase(Dt1.Rows(i).Item("Entry_PkCondition").ToString)) = "JPROD-" Then

                            Da1 = New SqlClient.SqlDataAdapter("Select a.Loom_IdNo, a.Width_Type, a.EndsCount_IdNo, a.Folding_Percentage   from JobWork_Production_Head a Where a.JobWork_Production_Code = '" & Trim(Dt1.Rows(i).Item("Lot_Code").ToString) & "'", con)
                            Da1.SelectCommand.Transaction = sqltr
                            Dt2 = New DataTable
                            Da1.Fill(Dt2)
                            If Dt2.Rows.Count > 0 Then
                                vLm_IdNo = Val(Dt2.Rows(0).Item("Loom_IdNo").ToString)
                                vWdth_Type = Dt2.Rows(0).Item("Width_Type").ToString
                                vCrmp_Perc = 0
                                vEndsCnt_IdNo = Val(Dt2.Rows(0).Item("EndsCount_IdNo").ToString)
                                vFOLDPERC = Val(Dt2.Rows(0).Item("Folding_Percentage").ToString)
                            End If
                            Dt2.Clear()


                        Else


                            Da1 = New SqlClient.SqlDataAdapter("Select Loom_IdNo, Width_Type, Crimp_Percentage, EndsCount_IdNo, Folding from Weaver_Cloth_Receipt_Head Where Weaver_ClothReceipt_Code = '" & Trim(Dt1.Rows(i).Item("Lot_Code").ToString) & "'", con)
                            Da1.SelectCommand.Transaction = sqltr
                            Dt2 = New DataTable
                            Da1.Fill(Dt2)
                            If Dt2.Rows.Count > 0 Then
                                vLm_IdNo = Val(Dt2.Rows(0).Item("Loom_IdNo").ToString)
                                vWdth_Type = Dt2.Rows(0).Item("Width_Type").ToString
                                vCrmp_Perc = Val(Dt2.Rows(0).Item("Crimp_Percentage").ToString)
                                vEndsCnt_IdNo = Val(Dt2.Rows(0).Item("EndsCount_IdNo").ToString)
                                vFOLDPERC = Val(Dt2.Rows(0).Item("Folding").ToString)
                            End If
                            Dt2.Clear()

                        End If

                        vFOLDPERC = Val(txt_Folding.Text)
                        If Val(vFOLDPERC) = 0 Then vFOLDPERC = 100
                        If Val(v1STPC_FOLDPERC) = 0 Then v1STPC_FOLDPERC = vFOLDPERC
                        If vEndsCnt_IdNo <> 0 And Val(vChk_Mtrs) Then

                            vCalc_AutoLoom_JbWrk_PavuWidthWiseConsumption = True
                            If Common_Procedures.settings.JobWorker_PavuWidthWiseConsumption_IN_Delivery = 1 Then
                                vCalc_AutoLoom_JbWrk_PavuWidthWiseConsumption = False
                            End If
                            vPvuConsMtrs = Common_Procedures.get_Pavu_Consumption(con, vClo_ID, vLm_IdNo, Val(vChk_Mtrs), vWdth_Type, sqltr, Val(vCrmp_Perc), , vCalc_AutoLoom_JbWrk_PavuWidthWiseConsumption, Val(vFOLDPERC))

                            If vPvuConsMtrs <> 0 Then

                                Cmd.CommandText = "Insert into " & Trim(Common_Procedures.EntryTempSubTable) & "(Int1, meters1) Values ( " & Str(Val(vEndsCnt_IdNo)) & ", " & Str(Val(vPvuConsMtrs)) & ")"
                                Cmd.ExecuteNonQuery()

                            End If

                        End If

                    End If

                Next i

                If Val(vClo_ID) <> 0 And Trim(vDcCode) <> "" And Val(vTotChk_Mtrs) <> 0 Then

                    vFOLDPERC = Val(txt_Folding.Text)
                    If Val(vFOLDPERC) = 0 Then vFOLDPERC = 100

                    ConsYarn = Common_Procedures.get_Weft_ConsumedYarn(con, Val(vClo_ID), Val(vTotChk_Mtrs), sqltr,, Val(vFOLDPERC))
                    'ConsYarn = Common_Procedures.get_Weft_ConsumedYarn(con, Val(vClo_ID), Val(vTotChk_Mtrs), sqltr,, Val(v1STPC_FOLDPERC))



                    Cmd.CommandText = "Update Stock_Yarn_Processing_Details set Weight = " & Str(Val(ConsYarn)) & " Where Reference_Code = 'JPCDC-" & Trim(vDcCode) & "' and Sl_No = 1"
                    Nr = Cmd.ExecuteNonQuery()

                    vTOTCONSYARN = Val(vTOTCONSYARN) + Val(ConsYarn)

                    vWARPYARN_STOCK_POSTING_STS = Common_Procedures.get_FieldValue(con, "JobWork_Piece_Delivery_Head", "Yarn_Stock_Posting_for_Warp_Status", "(JobWork_Piece_Delivery_Code = '" & Trim(lbl_DcCode.Text) & "')", , sqltr)

                    If Val(vWARPYARN_STOCK_POSTING_STS) = 1 Then

                        vCONSYARN_FORWARP = Common_Procedures.get_Warp_ConsumedYarn(con, Val(vClo_ID), Val(vTotChk_Mtrs), sqltr, Val(vFOLDPERC))
                        'vCONSYARN_FORWARP = Common_Procedures.get_Warp_ConsumedYarn(con, Val(vClo_ID), Val(vTotChk_Mtrs), sqltr, Val(v1STPC_FOLDPERC))
                        If Val(vCONSYARN_FORWARP) <> 0 Then
                            Cmd.CommandText = "Update Stock_Yarn_Processing_Details set Weight = " & Str(Val(vCONSYARN_FORWARP)) & " Where Reference_Code = 'JPCDC-" & Trim(vDcCode) & "' and Sl_No = 501"
                            Nr = Cmd.ExecuteNonQuery()
                        End If


                    Else

                        Da1 = New SqlClient.SqlDataAdapter("select Int1 as Endscount_IdNo, sum(meters1) as PavuMtrs from " & Trim(Common_Procedures.EntryTempSubTable) & " group by Int1 having sum(meters1) <> 0", con)
                        Da1.SelectCommand.Transaction = sqltr
                        Dt2 = New DataTable
                        Da1.Fill(Dt2)
                        If Dt2.Rows.Count > 0 Then
                            For i = 0 To Dt2.Rows.Count - 1

                                'If Val(vWARPYARN_STOCK_POSTING_STS) = 1 Then

                                '    vCONSYARN_FORWARP = Common_Procedures.get_Warp_ConsumedYarn(con, Val(Dt1.Rows(i).Item("Cloth_Idno").ToString), Val(Dt2.Rows(i).Item("PavuMtrs").ToString), sqltr)

                                '    If Val(vCONSYARN_FORWARP) <> 0 Then

                                '        Cmd.CommandText = "Update Stock_Yarn_Processing_Details set Weight = " & Str(Val(vCONSYARN_FORWARP)) & " Where Reference_Code = 'JPCDC-" & Trim(vDcCode) & "' and Sl_No > 100"
                                '        Nr = Cmd.ExecuteNonQuery()

                                '    End If

                                'Else

                                Cmd.CommandText = "Update Stock_Pavu_Processing_Details set Meters = " & Str(Val(Dt2.Rows(i).Item("PavuMtrs").ToString)) & " Where Reference_Code = 'JPCDC-" & Trim(vDcCode) & "' and EndsCount_IdNo = '" & Trim(Dt2.Rows(i).Item("EndsCount_IdNo").ToString) & "'"
                                Nr = Cmd.ExecuteNonQuery()

                                'End If

                            Next
                        End If
                        Dt2.Clear()

                    End If

                End If

            End If
            Dt1.Clear()





        End If

        lbl_ConsYarn.Text = Val(vTOTCONSYARN)

        Dt1.Dispose()
        Dt2.Dispose()
        Da.Dispose()

    End Sub

    Private Sub txt_Remarks_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_Remarks.LostFocus
        With cbo_Filter_PartyName
            .BackColor = Color.White
            .ForeColor = Color.Black
        End With
    End Sub

    Private Sub msk_Date_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles msk_Date.KeyDown
        vcbo_KeyDwnVal = e.KeyValue

        vmskOldText = ""
        vmskSelStrt = -1
        If e.KeyCode = 46 Or e.KeyCode = 8 Then
            vmskOldText = msk_Date.Text
            vmskSelStrt = msk_Date.SelectionStart
        End If

    End Sub

    Private Sub msk_Date_KeyPress(sender As Object, e As System.Windows.Forms.KeyPressEventArgs) Handles msk_Date.KeyPress
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

    Private Sub btn_Stock_RePosting_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Stock_RePosting.Click
        Dim cmd As New SqlClient.SqlCommand
        Dim tr As SqlClient.SqlTransaction
        Dim NewCode As String = ""

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_CheckingNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        tr = con.BeginTransaction

        Try

            cmd.Connection = con
            cmd.Transaction = tr

            cmd.CommandText = "truncate table TempTable_For_Jobwork_Inspection_Stock_Posting"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Insert into TempTable_For_Jobwork_Inspection_Stock_Posting ( JobWork_Piece_Delivery_Code,   Entry_PkCondition,   Lot_Code,   Pcs_No,   Cloth_Idno, Meters) " & _
                                "          Select                                         a.JobWork_Piece_Delivery_Code, a.Entry_PkCondition, a.Lot_Code, a.Pcs_No, b.Cloth_Idno, Meters from JobWork_Piece_Delivery_Details a INNER JOIN JobWork_Piece_Delivery_Head b ON a.JobWork_Piece_Delivery_Code = b.JobWork_Piece_Delivery_Code where a.JobWork_Inspection_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            Stock_Posting_Pavu_and_yarn(NewCode, tr)

            tr.Commit()

            MessageBox.Show("Saved Sucessfully!!!", "FOR SAVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Information)

        Catch ex As Exception
            tr.Rollback()
            MessageBox.Show(ex.Message, "FOR SAVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally
            tr.Dispose()
            cmd.Dispose()

            If msk_Date.Enabled And msk_Date.Visible Then msk_Date.Focus()

        End Try

    End Sub

    Private Sub btn_UserModification_Click(sender As Object, e As System.EventArgs) Handles btn_UserModification.Click
        If Val(Common_Procedures.User.IdNo) = 1 Then
            Dim f1 As New User_Modifications
            f1.Entry_Name = Me.Name
            f1.Entry_PkValue = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)
            f1.ShowDialog()
        End If
    End Sub

    Private Sub txt_Party_Piece_Inspection_No_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txt_Party_Piece_Inspection_No.KeyPress

        If Asc(e.KeyChar) = 13 Then
            If MessageBox.Show("Do you want to save ?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = DialogResult.Yes Then
                save_record()
            Else
                msk_Date.Focus()
            End If
        End If

    End Sub

    Private Sub txt_Party_Piece_Inspection_No_KeyDown(sender As Object, e As KeyEventArgs) Handles txt_Party_Piece_Inspection_No.KeyDown
        If e.KeyCode = 40 Then btn_save.Focus()

        If e.KeyCode = 38 Then
            txt_Remarks.Focus()
        End If
    End Sub

    Private Sub btn_get_Weft_CountName_from_Master_Click(sender As Object, e As EventArgs) Handles btn_get_Weft_CountName_from_Master.Click
        Dim cmd As New SqlClient.SqlCommand
        Dim Clo_IdNo As Integer
        Dim wftcnt_idno As Integer
        Dim Nr As Integer
        Dim NewCode As String

        If Trim(lbl_ClothName.Text) <> "" Then

            NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_CheckingNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            Clo_IdNo = Common_Procedures.Cloth_NameToIdNo(con, lbl_ClothName.Text)

            wftcnt_idno = Val(Common_Procedures.get_FieldValue(con, "cloth_head", "Cloth_WeftCount_IdNo", "(cloth_idno = " & Str(Val(Clo_IdNo)) & ")"))
            lbl_WeftCount.Text = Common_Procedures.Count_IdNoToName(con, wftcnt_idno)

            cmd.Connection = con

            cmd.CommandText = "Update JobWork_Piece_Inspection_Head set Count_IdNo = " & Str(Val(wftcnt_idno)) & " Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and JobWork_Piece_Inspection_Code = '" & Trim(NewCode) & "'"
            Nr = cmd.ExecuteNonQuery()

            cmd.CommandText = "Update Stock_Yarn_Processing_Details set Count_IdNo = " & Str(Val(wftcnt_idno)) & " Where Reference_Code = 'JPCDC-" & Trim(lbl_DcCode.Text) & "' and Sl_No = 1"
            Nr = cmd.ExecuteNonQuery()

            MessageBox.Show("Saved Sucessfully!!!", "FOR SAVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Information)

        End If

    End Sub

    Private Sub lbl_EndsCount_Click(sender As Object, e As EventArgs) Handles lbl_EndsCount.Click

    End Sub
End Class
