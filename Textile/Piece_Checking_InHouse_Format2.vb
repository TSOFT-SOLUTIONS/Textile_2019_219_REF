Public Class Piece_Checking_InHouse_Format2
    Implements Interface_MDIActions

    Private con As New SqlClient.SqlConnection(Common_Procedures.Connection_String)
    Private FrmLdSTS As Boolean = False
    Private New_Entry As Boolean = False
    Private Insert_Entry As Boolean = False
    Private Filter_Status As Boolean = False

    Private Pk_Condition As String = "PCSCH-"
    Private Pk_Condition2 As String = "PCDOF-"
    Private NoCalc_Status As Boolean = False
    Private Prec_ActCtrl As New Control
    Private vcbo_KeyDwnVal As Double
    Private vCbo_ItmNm As String

    Private WithEvents dgtxt_Details As New DataGridViewTextBoxEditingControl

    Private Sub clear()

        NoCalc_Status = True

        New_Entry = False
        Insert_Entry = False

        Pnl_Back.Enabled = True
        pnl_Filter.Visible = False

        lbl_ChkNo.Text = ""
        lbl_ChkNo.ForeColor = Color.Black

        dtp_Date.Text = ""
        lbl_PartyName.Text = ""
        txt_Folding.Text = ""
        cbo_Grid_ClothType.Text = ""
        Label5.Text = Common_Procedures.settings.ClothReceipt_LotNo_OR_RollNo_Text
        cbo_Filter_PartyName.Text = ""

        lbl_RecMtrs.Text = ""
        txt_RollNo.Text = ""
        txt_RollNo.Tag = ""
        lbl_RecDate.Text = ""
        lbl_RecCode.Text = ""
        lbl_WidthType.Text = ""
        lbl_LoomNo.Text = ""

        dgv_Details.Rows.Clear()
        dgv_Details_Total.Rows.Clear()
        dgv_Details_Total.Rows.Add()

        If Filter_Status = False Then
            dtp_Filter_Fromdate.Text = ""
            dtp_Filter_ToDate.Text = ""
            cbo_Filter_PartyName.Text = ""

            cbo_Filter_PartyName.SelectedIndex = -1

            dgv_Filter_Details.Rows.Clear()

        End If

        Grid_Cell_DeSelect()

        txt_RollNo.Enabled = True
        txt_RollNo.BackColor = Color.White


        txt_Folding.Enabled = True
        txt_Folding.BackColor = Color.White

        btn_Selection.Enabled = True

        cbo_Grid_ClothType.Visible = False

        NoCalc_Status = False

    End Sub

    Private Sub ControlGotFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim txtbx As TextBox
        Dim combobx As ComboBox
        On Error Resume Next

        If TypeOf Me.ActiveControl Is TextBox Or TypeOf Me.ActiveControl Is ComboBox Or TypeOf Prec_ActCtrl Is Button Then
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

        If Me.ActiveControl.Name <> cbo_ClothName.Name Then
            cbo_ClothName.Visible = False
        End If

        If Me.ActiveControl.Name <> cbo_Grid_ClothType.Name Then
            cbo_Grid_ClothType.Visible = False
        End If

        If Me.ActiveControl.Name <> dgv_Details.Name Then
            Grid_DeSelect()
        End If

        Prec_ActCtrl = Me.ActiveControl

    End Sub

    Private Sub ControlLostFocus(ByVal sender As Object, ByVal e As System.EventArgs)

        On Error Resume Next

        If IsNothing(Prec_ActCtrl) = False Then
            If TypeOf Prec_ActCtrl Is TextBox Or TypeOf Prec_ActCtrl Is ComboBox Then
                Prec_ActCtrl.BackColor = Color.White
                Prec_ActCtrl.ForeColor = Color.Black
            ElseIf TypeOf Prec_ActCtrl Is Button Then
                Prec_ActCtrl.BackColor = Color.FromArgb(41, 57, 85)
                Prec_ActCtrl.ForeColor = Color.White
            End If
        End If

    End Sub

    Private Sub Grid_DeSelect()
        On Error Resume Next
        dgv_Details.CurrentCell.Selected = False
        dgv_Details_Total.CurrentCell.Selected = False
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
        dgv_Details.CurrentCell.Selected = False
        dgv_Details_Total.CurrentCell.Selected = False
        dgv_Filter_Details.CurrentCell.Selected = False
    End Sub

    Private Sub Piece_Checking_InHouse_Format2_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated

        Try
            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_ClothName.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "CLOTH" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_ClothName.Text = Trim(Common_Procedures.Master_Return.Return_Value)
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

    Private Sub Piece_Checking_InHouse_Format2_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        con.Close()
        con.Dispose()
    End Sub

    Private Sub Piece_Checking_InHouse_Format2_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim dt3 As New DataTable

        Me.Text = ""

        con.Open()

        da = New SqlClient.SqlDataAdapter("select Cloth_Name from Cloth_Head order by Cloth_Name", con)
        da.Fill(dt3)
        cbo_ClothName.DataSource = dt3
        cbo_ClothName.DisplayMember = "Cloth_Name"

        da = New SqlClient.SqlDataAdapter("select ClothType_Name from ClothType_Head where ClothType_IdNo Between 0 and 5 order by ClothType_Name", con)
        dt1 = New DataTable
        da.Fill(dt1)
        cbo_Grid_ClothType.DataSource = dt1
        cbo_Grid_ClothType.DisplayMember = "ClothType_Name"


        cbo_Grid_ClothType.Visible = False

        dtp_Date.Text = ""

        pnl_Filter.Visible = False
        pnl_Filter.Left = (Me.Width - pnl_Filter.Width) \ 2
        pnl_Filter.Top = (Me.Height - pnl_Filter.Height) \ 2

        AddHandler dtp_Date.GotFocus, AddressOf ControlGotFocus
        AddHandler lbl_PartyName.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Folding.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Grid_ClothType.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_RollNo.GotFocus, AddressOf ControlGotFocus
        AddHandler lbl_RecMtrs.GotFocus, AddressOf ControlGotFocus
        AddHandler lbl_RecDate.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Folding.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_ClothName.GotFocus, AddressOf ControlGotFocus

        AddHandler cbo_Filter_PartyName.GotFocus, AddressOf ControlGotFocus

        AddHandler dtp_Date.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Folding.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Grid_ClothType.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_RollNo.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Folding.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_ClothName.LostFocus, AddressOf ControlLostFocus

        AddHandler cbo_Filter_PartyName.LostFocus, AddressOf ControlLostFocus

        AddHandler dtp_Date.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler dtp_Filter_Fromdate.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler dtp_Filter_ToDate.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_RollNo.KeyDown, AddressOf TextBoxControlKeyDown

        AddHandler dtp_Date.KeyPress, AddressOf TextBoxControlKeyPress
        'AddHandler txt_RollNo.KeyPress, AddressOf TextBoxControlKeyPress

        lbl_Company.Text = ""
        lbl_Company.Tag = 0
        lbl_Company.Visible = False
        Common_Procedures.CompIdNo = 0

        Filter_Status = False
        FrmLdSTS = True
        new_record()

    End Sub

    Private Sub Piece_Checking_InHouse_Format2_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress

        Try
            If Asc(e.KeyChar) = 27 Then

                If pnl_Filter.Visible = True Then
                    btn_Filter_Close_Click(sender, e)
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

    Protected Overrides Function ProcessCmdKey(ByRef msg As System.Windows.Forms.Message, ByVal keyData As System.Windows.Forms.Keys) As Boolean
        Dim dgv1 As New DataGridView

        On Error Resume Next

        If ActiveControl.Name = dgv_Details.Name Or TypeOf ActiveControl Is DataGridViewTextBoxEditingControl Then

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

                    If keyData = Keys.Enter Or keyData = Keys.Down Then
                        If .CurrentCell.ColumnIndex >= .ColumnCount - 3 Then
                            If .CurrentCell.RowIndex = .RowCount - 1 Then
                                If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = Windows.Forms.DialogResult.Yes Then
                                    save_record()
                                Else
                                    dtp_Date.Focus()
                                End If

                            Else
                                .CurrentCell = .Rows(.CurrentCell.RowIndex + 1).Cells(1)

                            End If

                        Else
                            If .CurrentCell.RowIndex = .RowCount - 1 And .CurrentCell.ColumnIndex >= 1 And Trim(.CurrentRow.Cells(1).Value) = "" Then
                                If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = Windows.Forms.DialogResult.Yes Then
                                    save_record()
                                Else
                                    dtp_Date.Focus()
                                End If


                            Else
                                .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 1)

                            End If

                        End If

                        Return True

                    ElseIf keyData = Keys.Up Then

                        If .CurrentCell.ColumnIndex <= 1 Then
                            If .CurrentCell.RowIndex = 0 Then
                                If txt_Folding.Enabled Then txt_Folding.Focus() Else dtp_Date.Focus()

                            Else
                                .CurrentCell = .Rows(.CurrentCell.RowIndex - 1).Cells(.ColumnCount - 3)

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
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim NewCode As String
        Dim n As Integer, i As Integer, j As Integer
        Dim SNo As Integer
        Dim LockSTS As Boolean = False

        If Val(no) = 0 Then Exit Sub

        clear()

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(no) & "/" & Trim(Common_Procedures.FnYearCode)

        Try
            da1 = New SqlClient.SqlDataAdapter("select *  from  Weaver_Piece_Checking_Head  Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Weaver_Piece_Checking_Code = '" & Trim(NewCode) & "' and Receipt_Type = 'L'", con)
            dt1 = New DataTable
            da1.Fill(dt1)

            If dt1.Rows.Count > 0 Then

                lbl_ChkNo.Text = dt1.Rows(0).Item("Weaver_Piece_Checking_No").ToString
                dtp_Date.Text = dt1.Rows(0).Item("Weaver_Piece_Checking_Date").ToString
                txt_RollNo.Text = Val(dt1.Rows(0).Item("Piece_Receipt_No").ToString)
                txt_RollNo.Tag = txt_RollNo.Text
                lbl_RecCode.Text = dt1.Rows(0).Item("Piece_Receipt_Code").ToString
                lbl_RecDate.Text = Format(Convert.ToDateTime(dt1.Rows(0).Item("Piece_Receipt_Date").ToString), "dd-MM-yyyy").ToString
                lbl_PartyName.Text = Common_Procedures.Ledger_IdNoToName(con, Val(dt1.Rows(0).Item("Ledger_IdNo").ToString))
                lbl_RecMtrs.Text = dt1.Rows(0).Item("ReceiptMeters_Receipt").ToString
                txt_Folding.Text = Val(dt1.Rows(0).Item("Folding").ToString)
                lbl_LoomNo.Text = Common_Procedures.Loom_IdNoToName(con, Val(dt1.Rows(0).Item("Loom_IdNo").ToString))
                lbl_WidthType.Text = dt1.Rows(0).Item("Width_Type").ToString

                dt2.Clear()

                LockSTS = False

                da2 = New SqlClient.SqlDataAdapter("select a.* , B.Cloth_Name from Weaver_ClothReceipt_Piece_Details a INNER JOIN Cloth_Head b ON a.Cloth_IdNo = b.Cloth_IdNo where a.Weaver_Piece_Checking_Code = '" & Trim(NewCode) & "' Order by a.Piece_No", con)
                dt2 = New DataTable
                da2.Fill(dt2)

                With dgv_Details

                    .Rows.Clear()
                    SNo = 0

                    If dt2.Rows.Count > 0 Then

                        For i = 0 To dt2.Rows.Count - 1

                            n = .Rows.Add()

                            .Rows(n).Cells(0).Value = dt2.Rows(i).Item("Piece_No").ToString
                            .Rows(n).Cells(9).Value = ""
                            .Rows(n).Cells(10).Value = ""

                            .Rows(n).Cells(1).Value = (dt2.Rows(i).Item("Cloth_Name").ToString)

                            If Val(dt2.Rows(i).Item("Type1_Meters").ToString) <> 0 Then
                                .Rows(n).Cells(2).Value = Common_Procedures.ClothType.Type1
                                .Rows(n).Cells(3).Value = Format(Val(dt2.Rows(i).Item("Type1_Meters").ToString), "########0.00")
                                .Rows(n).Cells(10).Value = dt2.Rows(i).Item("PackingSlip_Code_Type1").ToString
                                If Trim(.Rows(n).Cells(10).Value) <> "" Then
                                    .Rows(n).Cells(9).Value = "1"

                                    LockSTS = True
                                    For j = 0 To .ColumnCount - 1
                                        .Rows(n).Cells(j).Style.BackColor = Color.Gainsboro
                                        .Rows(n).Cells(j).Style.ForeColor = Color.Red
                                    Next
                                End If

                            ElseIf Val(dt2.Rows(i).Item("Type2_Meters").ToString) <> 0 Then
                                .Rows(n).Cells(2).Value = Common_Procedures.ClothType.Type2
                                .Rows(n).Cells(3).Value = Format(Val(dt2.Rows(i).Item("Type2_Meters").ToString), "########0.00")
                                .Rows(n).Cells(10).Value = dt2.Rows(i).Item("PackingSlip_Code_Type2").ToString
                                If Trim(.Rows(n).Cells(10).Value) <> "" Then
                                    .Rows(n).Cells(9).Value = "1"
                                    .Rows(n).Cells(2).Style.ForeColor = Color.Red
                                    LockSTS = True
                                    For j = 0 To .ColumnCount - 1
                                        .Rows(n).Cells(j).Style.BackColor = Color.Gainsboro
                                        .Rows(n).Cells(j).Style.ForeColor = Color.Red
                                    Next
                                End If

                            ElseIf Val(dt2.Rows(i).Item("Type3_Meters").ToString) <> 0 Then
                                .Rows(n).Cells(2).Value = Common_Procedures.ClothType.Type3
                                .Rows(n).Cells(3).Value = Format(Val(dt2.Rows(i).Item("Type3_Meters").ToString), "########0.00")
                                .Rows(n).Cells(10).Value = dt2.Rows(i).Item("PackingSlip_Code_Type3").ToString
                                If Trim(.Rows(n).Cells(10).Value) <> "" Then
                                    .Rows(n).Cells(9).Value = "1"
                                    .Rows(n).Cells(2).Style.ForeColor = Color.Red
                                    LockSTS = True
                                    For j = 0 To .ColumnCount - 1
                                        .Rows(n).Cells(j).Style.BackColor = Color.Gainsboro
                                        .Rows(n).Cells(j).Style.ForeColor = Color.Red
                                    Next
                                End If

                            ElseIf Val(dt2.Rows(i).Item("Type4_Meters").ToString) <> 0 Then
                                .Rows(n).Cells(2).Value = Common_Procedures.ClothType.Type4
                                .Rows(n).Cells(3).Value = Format(Val(dt2.Rows(i).Item("Type4_Meters").ToString), "########0.00")
                                .Rows(n).Cells(10).Value = dt2.Rows(i).Item("PackingSlip_Code_Type4").ToString
                                If Trim(.Rows(n).Cells(10).Value) <> "" Then
                                    .Rows(n).Cells(9).Value = "1"
                                    .Rows(n).Cells(2).Style.ForeColor = Color.Red
                                    LockSTS = True
                                    For j = 0 To .ColumnCount - 1
                                        .Rows(n).Cells(j).Style.BackColor = Color.Gainsboro
                                        .Rows(n).Cells(j).Style.ForeColor = Color.Red
                                    Next
                                End If

                            ElseIf Val(dt2.Rows(i).Item("Type5_Meters").ToString) <> 0 Then
                                .Rows(n).Cells(2).Value = Common_Procedures.ClothType.Type5
                                .Rows(n).Cells(3).Value = Format(Val(dt2.Rows(i).Item("Type5_Meters").ToString), "########0.00")
                                .Rows(n).Cells(10).Value = dt2.Rows(i).Item("PackingSlip_Code_Type5").ToString
                                If Trim(.Rows(n).Cells(10).Value) <> "" Then
                                    .Rows(n).Cells(9).Value = "1"
                                    .Rows(n).Cells(2).Style.ForeColor = Color.Red
                                    LockSTS = True
                                    For j = 0 To .ColumnCount - 1
                                        .Rows(n).Cells(j).Style.BackColor = Color.Gainsboro
                                        .Rows(n).Cells(j).Style.ForeColor = Color.Red
                                    Next
                                End If
                            End If

                            .Rows(n).Cells(4).Value = Format(Val(dt2.Rows(i).Item("Crimp_Percentage").ToString), "########0.00")
                            .Rows(n).Cells(5).Value = Format(Val(dt2.Rows(i).Item("ConsumedPavu_Receipt").ToString), "########0.00")
                            .Rows(n).Cells(6).Value = Format(Val(dt2.Rows(i).Item("ConsumedYarn_Receipt").ToString), "########0.000")

                            .Rows(n).Cells(7).Value = Format(Val(dt2.Rows(i).Item("Weight").ToString), "########0.000")

                            .Rows(n).Cells(8).Value = Format(Val(dt2.Rows(i).Item("Weight_Meter").ToString), "########0.000")

                        Next i

                    End If

                End With

                With dgv_Details_Total
                    If .RowCount = 0 Then .Rows.Add()
                    .Rows(0).Cells(3).Value = Format(Val(dt1.Rows(0).Item("Total_Checking_Meters").ToString), "########0.00")
                    .Rows(0).Cells(5).Value = Format(Val(dt1.Rows(0).Item("Total_ConsumedPavu").ToString), "########0.00")
                    .Rows(0).Cells(6).Value = Format(Val(dt1.Rows(0).Item("Total_ConsumedYarn").ToString), "########0.000")
                    .Rows(0).Cells(7).Value = Format(Val(dt1.Rows(0).Item("Total_Weight").ToString), "########0.000")

                End With

                dt2.Clear()

                da2 = New SqlClient.SqlDataAdapter("select Weaver_Wages_Code from Weaver_Cloth_Receipt_Head Where Weaver_Piece_Checking_Code = '" & Trim(NewCode) & "'", con)
                dt2 = New DataTable
                da2.Fill(dt2)

                If dt2.Rows.Count > 0 Then
                    If IsDBNull(dt2.Rows(0).Item("Weaver_Wages_Code").ToString) = False Then
                        If Trim(dt2.Rows(0).Item("Weaver_Wages_Code").ToString) <> "" Then
                            LockSTS = True
                        End If
                    End If
                End If
                dt1.Clear()

                If LockSTS = True Then

                    txt_RollNo.Enabled = False
                    txt_RollNo.BackColor = Color.Gainsboro


                    txt_Folding.Enabled = False
                    txt_Folding.BackColor = Color.Gainsboro

                    btn_Selection.Enabled = False

                End If

                dt2.Dispose()
                da2.Dispose()

            Else
                new_record()

            End If

            dt1.Clear()
            dt1.Dispose()
            da1.Dispose()

            Grid_Cell_DeSelect()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT MOVE RECORDS...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        If dtp_Date.Visible And dtp_Date.Enabled Then dtp_Date.Focus()

    End Sub

    Public Sub delete_record() Implements Interface_MDIActions.delete_record
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim cmd As New SqlClient.SqlCommand
        Dim trans As SqlClient.SqlTransaction
        Dim NewCode As String = ""

        If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.Inhouse_Piece_Checking_Entry, "~L~") = 0 And InStr(Common_Procedures.UR.Inhouse_Piece_Checking_Entry, "~D~") = 0 Then MessageBox.Show("You have No Rights to Delete", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error) : Exit Sub

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

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_ChkNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Da = New SqlClient.SqlDataAdapter("select COUNT(*) from Weaver_ClothReceipt_Piece_Details Where Weaver_Piece_Checking_Code = '" & Trim(NewCode) & "' and (PackingSlip_Code_Type1 <> '' or PackingSlip_Code_Type2 <> ''or PackingSlip_Code_Type3 <> ''or PackingSlip_Code_Type4 <> ''or PackingSlip_Code_Type5 <> '')", con)
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

        Da = New SqlClient.SqlDataAdapter("select Weaver_Wages_Code from Weaver_Cloth_Receipt_Head Where Weaver_Piece_Checking_Code = '" & Trim(NewCode) & "'", con)
        Dt1 = New DataTable
        Da.Fill(Dt1)

        If Dt1.Rows.Count > 0 Then
            If IsDBNull(Dt1.Rows(0).Item("Weaver_Wages_Code").ToString) = False Then
                If Trim(Dt1.Rows(0).Item("Weaver_Wages_Code").ToString) <> "" Then
                    MessageBox.Show("Weaver Wages prepared", "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                    Exit Sub

                End If
            End If
        End If
        Dt1.Clear()

        trans = con.BeginTransaction

        Try

            cmd.Connection = con
            cmd.Transaction = trans

            cmd.CommandText = "Update Stock_Yarn_Processing_Details set Weight = b.ConsumedYarn_Receipt from Stock_Yarn_Processing_Details a, Weaver_Cloth_Receipt_Head b Where b.Weaver_Piece_Checking_Code = '" & Trim(NewCode) & "' and a.Reference_Code = '" & Trim(Pk_Condition2) & "' + b.Weaver_ClothReceipt_Code and b.Weaver_Wages_Code = ''"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Update Stock_Pavu_Processing_Details set Meters = b.ConsumedPavu_Receipt from Stock_Pavu_Processing_Details a, Weaver_Cloth_Receipt_Head b Where b.Weaver_Piece_Checking_Code = '" & Trim(NewCode) & "' and a.Reference_Code = '" & Trim(Pk_Condition2) & "' + b.Weaver_ClothReceipt_Code and b.Weaver_Wages_Code = ''"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Update Stock_Cloth_Processing_Details set reference_date = (case when b.Weaver_Wages_Code <> '' then b.Weaver_Wages_Date else b.Weaver_ClothReceipt_Date end), Folding = b.Folding_Receipt, UnChecked_Meters = (case when b.Weaver_Wages_Code = '' then b.ReceiptMeters_Receipt else 0 end), Meters_Type1 = (case when b.Weaver_Wages_Code <> '' then b.Type1_Wages_Meters else 0 end), Meters_Type2 = (case when b.Weaver_Wages_Code <> '' then b.Type2_Wages_Meters else 0 end), Meters_Type3 = (case when b.Weaver_Wages_Code <> '' then b.Type3_Wages_Meters else 0 end), Meters_Type4 = (case when b.Weaver_Wages_Code <> '' then b.Type4_Wages_Meters else 0 end), Meters_Type5 = (case when b.Weaver_Wages_Code <> '' then b.Type5_Wages_Meters else 0 end) from Stock_Cloth_Processing_Details a, Weaver_Cloth_Receipt_Head b Where b.Weaver_Piece_Checking_Code = '" & Trim(NewCode) & "' and a.Reference_Code = '" & Trim(Pk_Condition2) & "' + b.Weaver_ClothReceipt_Code"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Update Weaver_Cloth_Receipt_Head set Weaver_Piece_Checking_Code = '', Weaver_Piece_Checking_Increment = Weaver_Piece_Checking_Increment - 1, Weaver_Piece_Checking_Date = Null, Receipt_Meters = ReceiptMeters_Receipt, Folding = Folding_Receipt, Consumed_Yarn = ConsumedYarn_Receipt, Consumed_Pavu = ConsumedPavu_Receipt, Folding_Checking = 0, ReceiptMeters_Checking = 0, ConsumedYarn_Checking = 0, ConsumedPavu_Checking = 0, Type1_Checking_Meters = 0, Type2_Checking_Meters = 0, Type3_Checking_Meters = 0, Type4_Checking_Meters = 0, Type5_Checking_Meters = 0, Total_Checking_Meters = 0 Where Weaver_Piece_Checking_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "delete from Weaver_ClothReceipt_Piece_Details where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Weaver_Piece_Checking_Code = '" & Trim(NewCode) & "' AND Create_Status = 0 "
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Update Weaver_ClothReceipt_Piece_Details set Weaver_Piece_Checking_Code = '',  Weaver_Piece_Checking_No = '', Weaver_Piece_Checking_Date = Null, Receipt_Meters = ReceiptMeters_Receipt, ReceiptMeters_Checking = 0, Loom_No = '', Pick = 0, Width = 0, Type1_Meters = 0, Type2_Meters = 0, Type3_Meters = 0, Type4_Meters  = 0, Type5_Meters = 0, Total_Checking_Meters = 0, Weight = 0, Weight_Meter = 0 Where Weaver_Piece_Checking_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "delete from Weaver_Piece_Checking_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Weaver_Piece_Checking_Code = '" & Trim(NewCode) & "'"
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

            If dtp_Date.Enabled = True And dtp_Date.Visible = True Then dtp_Date.Focus()

        End Try

    End Sub

    Public Sub filter_record() Implements Interface_MDIActions.filter_record

        If Filter_Status = False Then

            Dim da As New SqlClient.SqlDataAdapter
            Dim dt1 As New DataTable

            da = New SqlClient.SqlDataAdapter("select Ledger_DisplayName from Ledger_AlaisHead where (Ledger_IdNo = 0 or Ledger_Type = 'GODOWN' or Ledger_Type = 'JOBWORKER' or Ledger_Type = 'WEAVER') order by Ledger_DisplayName", con)
            dt1 = New DataTable
            da.Fill(dt1)
            cbo_Filter_PartyName.DataSource = dt1
            cbo_Filter_PartyName.DisplayMember = "Ledger_DisplayName"

            dtp_Filter_Fromdate.Text = Common_Procedures.Company_FromDate
            dtp_Filter_ToDate.Text = Common_Procedures.Company_ToDate
            cbo_Filter_PartyName.Text = ""

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

            da = New SqlClient.SqlDataAdapter("select top 1 Weaver_Piece_Checking_No from Weaver_Piece_Checking_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Weaver_Piece_Checking_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' and Receipt_Type = 'L' Order by for_Orderby, Weaver_Piece_Checking_No", con)
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

            OrdByNo = Common_Procedures.OrderBy_CodeToValue(Trim(lbl_ChkNo.Text))

            da = New SqlClient.SqlDataAdapter("select top 1 Weaver_Piece_Checking_No from Weaver_Piece_Checking_Head where for_orderby > " & Str(Val(OrdByNo)) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and Weaver_Piece_Checking_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' and Receipt_Type = 'L' Order by for_Orderby, Weaver_Piece_Checking_No", con)
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

            OrdByNo = Common_Procedures.OrderBy_CodeToValue(Trim(lbl_ChkNo.Text))

            da = New SqlClient.SqlDataAdapter("select top 1 Weaver_Piece_Checking_No from Weaver_Piece_Checking_Head where for_orderby < " & Str(Val(OrdByNo)) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and Weaver_Piece_Checking_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' and Receipt_Type = 'L'  Order by for_Orderby desc, Weaver_Piece_Checking_No desc", con)
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
            da = New SqlClient.SqlDataAdapter("select top 1 Weaver_Piece_Checking_No from Weaver_Piece_Checking_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Weaver_Piece_Checking_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' and Receipt_Type = 'L' Order by for_Orderby desc, Weaver_Piece_Checking_No desc", con)
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
        Dim dt As New DataTable
        Dim NewID As Integer = 0

        Try
            clear()

            New_Entry = True

            lbl_ChkNo.Text = Common_Procedures.get_MaxCode(con, "Weaver_Piece_Checking_Head", "Weaver_Piece_Checking_Code", "For_OrderBy", "", Val(lbl_Company.Tag), Common_Procedures.FnYearCode)

            lbl_ChkNo.ForeColor = Color.Red

            If dtp_Date.Enabled And dtp_Date.Visible Then dtp_Date.Focus()

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

            inpno = InputBox("Enter Checking No.", "FOR FINDING...")

            RecCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

            Da = New SqlClient.SqlDataAdapter("select Weaver_Piece_Checking_No from Weaver_Piece_Checking_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Weaver_Piece_Checking_Code = '" & Trim(RecCode) & "' and Receipt_Type = 'L'", con)
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

        If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.Inhouse_Piece_Checking_Entry, "~L~") = 0 And InStr(Common_Procedures.UR.Inhouse_Piece_Checking_Entry, "~I~") = 0 Then MessageBox.Show("You have No Rights to Insert", "DOES NOT INSERT...", MessageBoxButtons.OK, MessageBoxIcon.Error) : Exit Sub

        Try

            inpno = InputBox("Enter New Checking No.", "FOR NEW CHECKING INSERTION...")

            RecCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

            Da = New SqlClient.SqlDataAdapter("select Weaver_Piece_Checking_No from Weaver_Piece_Checking_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Weaver_Piece_Checking_Code = '" & Trim(RecCode) & "'", con)
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
                    MessageBox.Show("Invalid Checking No", "DOES NOT INSERT NEW CHECKING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

                Else
                    new_record()
                    Insert_Entry = True
                    lbl_ChkNo.Text = Trim(UCase(inpno))

                End If

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT INSERT NEW CHECKING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Public Sub save_record() Implements Interface_MDIActions.save_record
        Dim cmd As New SqlClient.SqlCommand
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim tr As SqlClient.SqlTransaction
        Dim NewCode As String = ""

        Dim Led_ID As Integer = 0
        Dim Clo_ID As Integer = 0
        Dim CloTyp_ID As Integer = 0

        Dim Sno As Integer = 0
        Dim Nr As Integer = 0
        Dim Partcls As String = ""
        Dim PBlNo As String = ""
        Dim EntID As String = ""

        Dim vTot_Typ1Mtrs As Single, vTotConPav As Single, vTotConYrn As Single
        Dim vTot_Typ2Mtrs As Single
        Dim vTot_Typ3Mtrs As Single
        Dim vTot_Typ5Mtrs As Single
        Dim vTot_Typ4Mtrs As Single
        Dim vTot_ChkMtrs As Single
        Dim vTot_Wgt As Single

        Dim vTot_100Fld_Typ1Mtrs As Single
        Dim vTot_100Fld_Typ2Mtrs As Single
        Dim vTot_100Fld_Typ3Mtrs As Single
        Dim vTot_100Fld_Typ4Mtrs As Single
        Dim vTot_100Fld_Typ5Mtrs As Single
        Dim vTot_100Fld_ChkMtr As Single

        Dim Lm_ID As Integer = 0
        Dim Wdth_Typ As String = ""
        Dim WagesCode As String = ""

        Dim ConsYarn As Single = 0
        Dim ConsPavu As Single = 0

        If Val(lbl_Company.Tag) = 0 Then
            MessageBox.Show("Invalid Company Selection", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        If Common_Procedures.UserRight_Check(Common_Procedures.UR.Inhouse_Piece_Checking_Entry, New_Entry) = False Then Exit Sub

        If Pnl_Back.Enabled = False Then
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

        Led_ID = Common_Procedures.Ledger_AlaisNameToIdNo(con, lbl_PartyName.Text)
        If Led_ID = 0 Then
            MessageBox.Show("Invalid Party Name", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If txt_RollNo.Enabled And txt_RollNo.Visible Then txt_RollNo.Focus()
            Exit Sub
        End If

        'Clo_ID = Common_Procedures.Cloth_NameToIdNo(con, lbl_ClothName.Text)
        'If Clo_ID = 0 Then
        '    MessageBox.Show("Invalid Cloth Name", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
        '    If txt_RollNo.Enabled And txt_RollNo.Visible Then txt_RollNo.Focus()
        '    Exit Sub
        'End If


        If Val(txt_Folding.Text) = 0 Then
            MessageBox.Show("Invalid Folding", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If txt_Folding.Enabled And txt_Folding.Visible Then txt_Folding.Focus()
            Exit Sub
        End If

        With dgv_Details
            For i = 0 To .RowCount - 1

                If Val(.Rows(i).Cells(3).Value) <> 0 Then

                    Clo_ID = Common_Procedures.Cloth_NameToIdNo(con, .Rows(i).Cells(1).Value)
                    If Clo_ID = 0 Then
                        MessageBox.Show("Invalid Cloth Name", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        .Focus()
                        .CurrentCell = .Rows(0).Cells(1)
                        Exit Sub
                    End If

                    If Trim(.Rows(i).Cells(2).Value) = "" Then
                        MessageBox.Show("Invalid Cloth Type", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        .Focus()
                        .CurrentCell = .Rows(0).Cells(2)
                        Exit Sub
                    End If

                    CloTyp_ID = Common_Procedures.ClothType_NameToIdNo(con, .Rows(i).Cells(2).Value)
                    If CloTyp_ID = 0 Then
                        MessageBox.Show("Invalid Cloth Type", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        .Focus()
                        .CurrentCell = .Rows(0).Cells(2)
                        Exit Sub
                    End If

                End If

            Next

        End With

        Total_Calculation()

        vTot_Typ1Mtrs = 0 : vTot_Typ2Mtrs = 0 : vTot_Typ3Mtrs = 0 : vTot_Typ4Mtrs = 0 : vTot_Typ5Mtrs = 0
        With dgv_Details
            For i = 0 To .RowCount - 1

                If Val(.Rows(i).Cells(2).Value) <> 0 Then

                    CloTyp_ID = Common_Procedures.ClothType_NameToIdNo(con, .Rows(i).Cells(1).Value)
                    If CloTyp_ID <> 0 Then

                        If CloTyp_ID = 1 Then
                            vTot_Typ1Mtrs = vTot_Typ1Mtrs + Val(.Rows(i).Cells(2).Value)

                        ElseIf CloTyp_ID = 2 Then
                            vTot_Typ2Mtrs = vTot_Typ2Mtrs + Val(.Rows(i).Cells(2).Value)

                        ElseIf CloTyp_ID = 3 Then
                            vTot_Typ3Mtrs = vTot_Typ3Mtrs + Val(.Rows(i).Cells(2).Value)

                        ElseIf CloTyp_ID = 4 Then
                            vTot_Typ4Mtrs = vTot_Typ4Mtrs + Val(.Rows(i).Cells(2).Value)

                        ElseIf CloTyp_ID = 5 Then
                            vTot_Typ5Mtrs = vTot_Typ5Mtrs + Val(.Rows(i).Cells(2).Value)

                        End If

                    End If

                End If

            Next

        End With

        vTot_ChkMtrs = 0 : vTot_Wgt = 0 : vTotConPav = 0 : vTotConYrn = 0
        If dgv_Details_Total.RowCount > 0 Then
            vTot_ChkMtrs = Val(dgv_Details_Total.Rows(0).Cells(3).Value())
            vTotConPav = Val(dgv_Details_Total.Rows(0).Cells(5).Value())
            vTotConYrn = Val(dgv_Details_Total.Rows(0).Cells(6).Value())
            vTot_Wgt = Val(dgv_Details_Total.Rows(0).Cells(7).Value())
        End If

        vTot_100Fld_Typ1Mtrs = Format(Val(vTot_Typ1Mtrs) * Val(txt_Folding.Text) / 100, "########0.00")
        vTot_100Fld_Typ2Mtrs = Format(Val(vTot_Typ2Mtrs) * Val(txt_Folding.Text) / 100, "########0.00")
        vTot_100Fld_Typ3Mtrs = Format(Val(vTot_Typ3Mtrs) * Val(txt_Folding.Text) / 100, "########0.00")
        vTot_100Fld_Typ4Mtrs = Format(Val(vTot_Typ4Mtrs) * Val(txt_Folding.Text) / 100, "########0.00")
        vTot_100Fld_Typ5Mtrs = Format(Val(vTot_Typ5Mtrs) * Val(txt_Folding.Text) / 100, "########0.00")
        vTot_100Fld_ChkMtr = Format(Val(vTot_ChkMtrs) * Val(txt_Folding.Text) / 100, "########0.00")

        tr = con.BeginTransaction

        Try

            If Insert_Entry = True Or New_Entry = False Then
                NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_ChkNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            Else

                lbl_ChkNo.Text = Common_Procedures.get_MaxCode(con, "Weaver_Piece_Checking_Head", "Weaver_Piece_Checking_Code", "For_OrderBy", "", Val(lbl_Company.Tag), Common_Procedures.FnYearCode, tr)

                NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_ChkNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            End If

            Da = New SqlClient.SqlDataAdapter("select Weaver_Wages_Code, Loom_IdNo, Width_Type from Weaver_Cloth_Receipt_Head Where Weaver_ClothReceipt_Code = '" & Trim(lbl_RecCode.Text) & "'", con)
            Da.SelectCommand.Transaction = tr
            Dt1 = New DataTable
            Da.Fill(Dt1)

            WagesCode = ""
            Lm_ID = 0
            Wdth_Typ = ""
            If Dt1.Rows.Count > 0 Then
                If IsDBNull(Dt1.Rows(0).Item("Weaver_Wages_Code").ToString) = False Then
                    WagesCode = Dt1.Rows(0).Item("Weaver_Wages_Code").ToString
                End If
                Lm_ID = Val(Dt1.Rows(0).Item("Loom_IdNo").ToString)
                Wdth_Typ = Dt1.Rows(0).Item("Width_Type").ToString
            End If
            Dt1.Clear()

            cmd.Connection = con
            cmd.Transaction = tr

            cmd.Parameters.Clear()
            cmd.Parameters.AddWithValue("@ChkDate", dtp_Date.Value.Date)
            cmd.Parameters.AddWithValue("@RecDate", CDate(lbl_RecDate.Text))

            Lm_ID = Common_Procedures.Loom_NameToIdNo(con, Trim(lbl_LoomNo.Text), tr)

            If New_Entry = True Then
                cmd.CommandText = "Insert into Weaver_Piece_Checking_Head ( Receipt_Type, Weaver_Piece_Checking_Code,             Company_IdNo         ,      Weaver_Piece_Checking_No ,                               for_OrderBy                              , Weaver_Piece_Checking_Date,      Receipt_PkCondition     ,           Piece_Receipt_Code    ,         Piece_Receipt_No       , Piece_Receipt_Date,         Ledger_IdNo     ,         Cloth_IdNo ,             ReceiptMeters_Receipt ,                Folding              ,     Total_Checking_Receipt_Meters ,          Total_Type1_Meters    ,      Total_Type2_Meters         ,   Total_Type3_Meters           ,     Total_Type4_Meters          ,     Total_Type5_Meters         ,     Total_Checking_Meters     ,     Total_Weight          ,     Total_Type1Meters_100Folding      ,     Total_Type2Meters_100Folding      ,     Total_Type3Meters_100Folding      ,      Total_Type4Meters_100Folding      ,     Total_Type5Meters_100Folding      ,      Total_Meters_100Folding         ,         Excess_Short_Meter   , Loom_Idno   ,  Width_Type                        , Total_ConsumedYarn , Total_ConsumedPavu    ) " & _
                                    "          Values                     (     'L'     ,    '" & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_ChkNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_ChkNo.Text))) & ",      @ChkDate             , '" & Trim(Pk_Condition2) & "', '" & Trim(lbl_RecCode.Text) & "', '" & Trim(txt_RollNo.Text) & "',      @RecDate     , " & Str(Val(Led_ID)) & ", " & Val(Clo_ID) & ", " & Str(Val(lbl_RecMtrs.Text)) & ",  " & Val(txt_Folding.Text) & ", " & Str(Val(lbl_RecMtrs.Text)) & ", " & Str(Val(vTot_Typ1Mtrs)) & ",  " & Str(Val(vTot_Typ2Mtrs)) & ", " & Str(Val(vTot_Typ3Mtrs)) & ",  " & Str(Val(vTot_Typ4Mtrs)) & ", " & Str(Val(vTot_Typ5Mtrs)) & ", " & Str(Val(vTot_ChkMtrs)) & ", " & Str(Val(vTot_Wgt)) & ", " & Str(Val(vTot_100Fld_Typ1Mtrs)) & ", " & Str(Val(vTot_100Fld_Typ2Mtrs)) & ", " & Str(Val(vTot_100Fld_Typ3Mtrs)) & ",  " & Str(Val(vTot_100Fld_Typ4Mtrs)) & ", " & Str(Val(vTot_100Fld_Typ5Mtrs)) & ",  " & Str(Val(vTot_100Fld_ChkMtr)) & ", " & Str(Val(lbl_ExcSht.Text)) & "  , " & Str(Val(Lm_ID)) & ",  '" & Trim(lbl_WidthType.Text) & "' ," & Str(Val(vTotConYrn)) & " , " & Str(Val(vTotConPav)) & "  ) "
                cmd.ExecuteNonQuery()

            Else
                cmd.CommandText = "Update Weaver_Piece_Checking_Head set Weaver_Piece_Checking_Date = @ChkDate, Ledger_IdNo = " & Str(Val(Led_ID)) & ", Piece_Receipt_Code = '" & Trim(lbl_RecCode.Text) & "', Piece_Receipt_No = '" & Trim(txt_RollNo.Text) & "', Piece_Receipt_Date = @chkDate, Cloth_IdNo = " & Val(Clo_ID) & ", ReceiptMeters_Receipt = " & Str(Val(lbl_RecMtrs.Text)) & ", Folding = " & Val(txt_Folding.Text) & ", Total_Type1_Meters = " & Str(Val(vTot_Typ1Mtrs)) & ",  Total_Type2_Meters = " & Str(Val(vTot_Typ2Mtrs)) & ", Total_Type3_Meters = " & Str(Val(vTot_Typ3Mtrs)) & ", Total_Type4_Meters = " & Str(Val(vTot_Typ4Mtrs)) & ", Total_Type5_Meters = " & Str(Val(vTot_Typ5Mtrs)) & ", Total_Checking_Receipt_Meters = " & Str(Val(vTot_ChkMtrs)) & ", Total_Checking_Meters = " & Str(Val(vTot_ChkMtrs)) & ", Total_Weight = " & Str(Val(vTot_Wgt)) & ", Total_Type1Meters_100Folding = " & Str(Val(vTot_100Fld_Typ1Mtrs)) & ", Total_Type2Meters_100Folding = " & Str(Val(vTot_100Fld_Typ2Mtrs)) & ", Total_Type3Meters_100Folding = " & Str(Val(vTot_100Fld_Typ3Mtrs)) & ", Total_Type4Meters_100Folding = " & Str(Val(vTot_100Fld_Typ4Mtrs)) & ", Total_Type5Meters_100Folding = " & Str(Val(vTot_100Fld_Typ5Mtrs)) & ", Total_Meters_100Folding  =  " & Str(Val(vTot_100Fld_ChkMtr)) & ", Excess_Short_Meter = " & Str(Val(lbl_ExcSht.Text)) & ", Loom_Idno = " & Str(Val(Lm_ID)) & " , Width_Type = '" & Trim(lbl_WidthType.Text) & "' , Total_ConsumedYarn = " & Str(Val(vTotConYrn)) & " , Total_ConsumedPavu = " & Str(Val(vTotConPav)) & "   Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Weaver_Piece_Checking_Code = '" & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()

                cmd.CommandText = "Update Stock_Yarn_Processing_Details set Weight = b.ConsumedYarn_Receipt from Stock_Yarn_Processing_Details a, Weaver_Cloth_Receipt_Head b Where b.Weaver_Piece_Checking_Code = '" & Trim(NewCode) & "' and a.Reference_Code = '" & Trim(Pk_Condition2) & "' + b.Weaver_ClothReceipt_Code and b.Weaver_Wages_Code = ''"
                cmd.ExecuteNonQuery()

                cmd.CommandText = "Update Stock_Pavu_Processing_Details set Meters = b.ConsumedPavu_Receipt from Stock_Pavu_Processing_Details a, Weaver_Cloth_Receipt_Head b Where b.Weaver_Piece_Checking_Code = '" & Trim(NewCode) & "' and a.Reference_Code = '" & Trim(Pk_Condition2) & "' + b.Weaver_ClothReceipt_Code and b.Weaver_Wages_Code = ''"
                cmd.ExecuteNonQuery()

                cmd.CommandText = "Update Stock_Cloth_Processing_Details set reference_date = (case when b.Weaver_Wages_Code <> '' then b.Weaver_Wages_Date else b.Weaver_ClothReceipt_Date end), Folding = b.Folding_Receipt, UnChecked_Meters = (case when b.Weaver_Wages_Code = '' then b.ReceiptMeters_Receipt else 0 end), Meters_Type1 = (case when b.Weaver_Wages_Code <> '' then b.Type1_Wages_Meters else 0 end), Meters_Type2 = (case when b.Weaver_Wages_Code <> '' then b.Type2_Wages_Meters else 0 end), Meters_Type3 = (case when b.Weaver_Wages_Code <> '' then b.Type3_Wages_Meters else 0 end), Meters_Type4 = (case when b.Weaver_Wages_Code <> '' then b.Type4_Wages_Meters else 0 end), Meters_Type5 = (case when b.Weaver_Wages_Code <> '' then b.Type5_Wages_Meters else 0 end) from Stock_Cloth_Processing_Details a, Weaver_Cloth_Receipt_Head b Where b.Weaver_Piece_Checking_Code = '" & Trim(NewCode) & "' and a.Reference_Code = '" & Trim(Pk_Condition2) & "' + b.Weaver_ClothReceipt_Code"
                cmd.ExecuteNonQuery()

                cmd.CommandText = "Update Weaver_Cloth_Receipt_Head set Weaver_Piece_Checking_Code = '', Weaver_Piece_Checking_Increment = Weaver_Piece_Checking_Increment - 1, Weaver_Piece_Checking_Date = Null, Receipt_Meters = ReceiptMeters_Receipt, Folding = Folding_Receipt, Consumed_Yarn = ConsumedYarn_Receipt, Consumed_Pavu = ConsumedPavu_Receipt, Folding_Checking = 0, ReceiptMeters_Checking = 0, ConsumedYarn_Checking = 0, ConsumedPavu_Checking = 0, Type1_Checking_Meters = 0, Type2_Checking_Meters = 0, Type3_Checking_Meters = 0, Type4_Checking_Meters = 0, Type5_Checking_Meters = 0, Total_Checking_Meters = 0 Where Weaver_Piece_Checking_Code = '" & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()

            End If

            EntID = Trim(Pk_Condition2) & Trim(txt_RollNo.Text)
            Partcls = "Doff : Roll.No. " & Trim(txt_RollNo.Text)
            PBlNo = Trim(txt_RollNo.Text)

            ConsYarn = vTotConYrn
            'ConsYarn = Val(Common_Procedures.get_Weft_ConsumedYarn(con, Clo_ID, Val(lbl_RecMtrs.Text), tr))

            ConsPavu = vTotConPav
            'ConsPavu = Val(Common_Procedures.get_Pavu_Consumption(con, Clo_ID, Lm_ID, Val(lbl_RecMtrs.Text), Trim(Wdth_Typ), tr))

            cmd.CommandText = "Update Weaver_Cloth_Receipt_Head set Weaver_Piece_Checking_Code = '" & Trim(NewCode) & "', Weaver_Piece_Checking_Increment = Weaver_Piece_Checking_Increment + 1, Weaver_Piece_Checking_Date = @ChkDate, Folding_Checking = " & Str(Val(txt_Folding.Text)) & ", Folding = " & Str(Val(txt_Folding.Text)) & ", ReceiptMeters_Checking = " & Str(Val(lbl_RecMtrs.Text)) & ", Receipt_Meters = " & Str(Val(lbl_RecMtrs.Text)) & ", ConsumedYarn_Checking = " & Str(Val(ConsYarn)) & ", Consumed_Yarn = " & Str(Val(ConsYarn)) & ", ConsumedPavu_Checking = " & Str(Val(ConsPavu)) & ", Consumed_Pavu = " & Str(Val(ConsPavu)) & ", Type1_Checking_Meters = " & Str(Val(vTot_Typ1Mtrs)) & ", Type2_Checking_Meters = " & Str(Val(vTot_Typ2Mtrs)) & ", Type3_Checking_Meters = " & Str(Val(vTot_Typ3Mtrs)) & ", Type4_Checking_Meters = " & Str(Val(vTot_Typ4Mtrs)) & ", Type5_Checking_Meters = " & Str(Val(vTot_Typ5Mtrs)) & ", Total_Checking_Meters = " & Str(Val(vTot_ChkMtrs)) & " Where Weaver_ClothReceipt_Code = '" & Trim(lbl_RecCode.Text) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Weaver_ClothReceipt_Piece_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Weaver_Piece_Checking_Code = '" & Trim(NewCode) & "' and Lot_Code = '" & Trim(lbl_RecCode.Text) & "' and PackingSlip_Code_Type1 = '' and PackingSlip_Code_Type2 = ''  and PackingSlip_Code_Type3 = ''  and PackingSlip_Code_Type4 = ''  and PackingSlip_Code_Type5 = ''"
            Nr = cmd.ExecuteNonQuery()

            With dgv_Details
                Sno = 0

                For i = 0 To .RowCount - 1

                    If Val(.Rows(i).Cells(3).Value) <> 0 Then

                        Sno = Sno + 1

                        Clo_ID = Common_Procedures.Cloth_NameToIdNo(con, .Rows(i).Cells(1).Value, tr)

                        CloTyp_ID = Common_Procedures.ClothType_NameToIdNo(con, .Rows(i).Cells(2).Value, tr)

                        Nr = 0
                        cmd.CommandText = "Update Weaver_ClothReceipt_Piece_Details set Weaver_Piece_Checking_Code = '" & Trim(NewCode) & "',  Weaver_Piece_Checking_No = '" & Trim(lbl_ChkNo.Text) & "', Weaver_Piece_Checking_Date = @ChkDate, Folding_Receipt = " & Str(Val(txt_Folding.Text)) & ", Folding_Checking = " & Str(Val(txt_Folding.Text)) & ", Folding = " & Str(Val(txt_Folding.Text)) & ", Sl_No = " & Str(Val(Sno)) & ", PieceNo_OrderBy = " & Str(Val(Common_Procedures.OrderBy_CodeToValue(Val(.Rows(i).Cells(0).Value)))) & ",   Cloth_IdNo  =  " & Str(Val(Clo_ID)) & " , ReceiptMeters_Checking = " & Str(Val(lbl_RecMtrs.Text)) & ", Receipt_Meters = " & Str(Val(lbl_RecMtrs.Text)) & ", Type" & Trim(Val(CloTyp_ID)) & "_Meters = " & Str(Val(.Rows(i).Cells(3).Value)) & ", Total_Checking_Meters = " & Str(Val(.Rows(i).Cells(3).Value)) & ", Weight = " & Str(Val(.Rows(i).Cells(7).Value)) & ", Weight_Meter = " & Str(Val(.Rows(i).Cells(8).Value)) & " , Crimp_Percentage =  " & Str(Val(.Rows(i).Cells(4).Value)) & " , ConsumedPavu_Receipt = " & Str(Val(.Rows(i).Cells(5).Value)) & ", ConsumedYarn_Receipt =  " & Str(Val(.Rows(i).Cells(6).Value)) & "  where Weaver_ClothReceipt_Code = '" & Trim(Pk_Condition2) & Trim(lbl_RecCode.Text) & "' and Lot_Code = '" & Trim(lbl_RecCode.Text) & "' and Piece_No = '" & Trim(.Rows(i).Cells(0).Value) & "'"
                        Nr = cmd.ExecuteNonQuery()

                        If Nr = 0 Then
                            cmd.CommandText = "Insert into Weaver_ClothReceipt_Piece_Details ( Weaver_Piece_Checking_Code ,             Company_IdNo         ,     Weaver_Piece_Checking_No   ,  Weaver_Piece_Checking_Date,               Weaver_ClothReceipt_Code                ,    Weaver_ClothReceipt_No      ,                               for_orderby                               , Weaver_ClothReceipt_Date,           Lot_Code              ,               Lot_No           ,           Cloth_IdNo    ,            Folding_Receipt        ,             Folding_Checking       ,             Folding               ,           Sl_No      ,                 Piece_No               ,                                PieceNo_OrderBy                                         ,            ReceiptMeters_Checking  ,                Receipt_Meters      ,   Type" & Trim(Val(CloTyp_ID)) & "_Meters ,                   Total_Checking_Meters  ,                     Weight                ,                   Weight_Meter           , Crimp_Percentage                            , ConsumedPavu_Receipt                          , ConsumedYarn_Receipt ) " & _
                                                "     Values                                 (    '" & Trim(NewCode) & "' , " & Str(Val(lbl_Company.Tag)) & ",  '" & Trim(lbl_ChkNo.Text) & "',            @ChkDate        , '" & Trim(Pk_Condition2) & Trim(lbl_RecCode.Text) & "', '" & Trim(txt_RollNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(txt_RollNo.Text))) & ",      @RecDate           , '" & Trim(lbl_RecCode.Text) & "', '" & Trim(txt_RollNo.Text) & "', " & Str(Val(Clo_ID)) & ", " & Str(Val(txt_Folding.Text)) & ", " & Str(Val(txt_Folding.Text)) & ", " & Str(Val(txt_Folding.Text)) & ", " & Str(Val(Sno)) & ", '" & Trim(.Rows(i).Cells(0).Value) & "',  " & Str(Val(Common_Procedures.OrderBy_CodeToValue(Trim(.Rows(i).Cells(0).Value)))) & ",  " & Str(Val(lbl_RecMtrs.Text)) & ",  " & Str(Val(lbl_RecMtrs.Text)) & ", " & Str(Val(.Rows(i).Cells(3).Value)) & "  , " & Str(Val(.Rows(i).Cells(3).Value)) & ", " & Str(Val(.Rows(i).Cells(7).Value)) & " , " & Str(Val(.Rows(i).Cells(8).Value)) & " ,  " & Str(Val(.Rows(i).Cells(4).Value)) & " , " & Str(Val(.Rows(i).Cells(5).Value)) & " , " & Str(Val(.Rows(i).Cells(6).Value)) & " ) "
                            cmd.ExecuteNonQuery()
                        End If

                        'cmd.CommandText = "Insert into Weaver_ClothReceipt_Piece_Details ( Weaver_ClothReceipt_Code, Company_IdNo, Weaver_Piece_Checking_No, for_OrderBy, Weaver_Piece_Checking_Date, Piece_No, Type1_Meters,Type2_Meters,Type3_Meters,Type4_Meters,Type5_Meters, Receipt_Meters, Weight, Weight_Meter ) Values ('" & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_ChkNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_ChkNo.Text))) & ", @ChkDate, '" & Trim(.Rows(i).Cells(0).Value) & "', 1,2,3,4,5,  " & Str(Val(.Rows(i).Cells(2).Value)) & ", " & Str(Val(.Rows(i).Cells(3).Value)) & ", " & Str(Val(.Rows(i).Cells(4).Value)) & " ,'" & Str(Trim(.Rows(i).Cells(4).Value)) & "')"
                        'cmd.ExecuteNonQuery()

                    End If

                Next

            End With

            If Trim(WagesCode) = "" Then

                cmd.CommandText = "Update Stock_Yarn_Processing_Details set Weight = " & Str(Val(ConsYarn)) & " Where Reference_Code = '" & Trim(Pk_Condition2) & Trim(lbl_RecCode.Text) & "'"
                cmd.ExecuteNonQuery()

                cmd.CommandText = "Update Stock_Pavu_Processing_Details set Meters = " & Str(Val(ConsPavu)) & " Where Reference_Code = '" & Trim(Pk_Condition2) & Trim(lbl_RecCode.Text) & "'"
                cmd.ExecuteNonQuery()

            End If

            If Val(vTot_Typ1Mtrs) <> 0 Or Val(vTot_Typ2Mtrs) <> 0 Or Val(vTot_Typ3Mtrs) <> 0 Or Val(vTot_Typ4Mtrs) <> 0 Or Val(vTot_Typ5Mtrs) <> 0 Then
                cmd.CommandText = "Update Stock_Cloth_Processing_Details set reference_date = @ChkDate,  Folding = " & Str(Val(txt_Folding.Text)) & ", UnChecked_Meters = 0, Meters_Type1 = " & Str(Val(vTot_Typ1Mtrs)) & ", Meters_Type2 = " & Str(Val(vTot_Typ2Mtrs)) & ", Meters_Type3 = " & Str(Val(vTot_Typ3Mtrs)) & ", Meters_Type4 = " & Str(Val(vTot_Typ4Mtrs)) & ", Meters_Type5 = " & Str(Val(vTot_Typ5Mtrs)) & " Where Reference_Code = '" & Trim(Pk_Condition2) & Trim(lbl_RecCode.Text) & "'"
                cmd.ExecuteNonQuery()

            End If

            tr.Commit()

            Dt1.Dispose()
            Da.Dispose()

            MessageBox.Show("Saved Sucessfully!!!", "FOR SAVING...", MessageBoxButtons.OK, MessageBoxIcon.Information)
            If Val(Common_Procedures.settings.OnSave_MoveTo_NewEntry_Status) = 1 Then
                If New_Entry = True Then
                    new_record()
                Else
                    move_record(lbl_ChkNo.Text)
                End If
            Else
                move_record(lbl_ChkNo.Text)
            End If


        Catch ex As Exception
            tr.Rollback()
            MessageBox.Show(ex.Message, "FOR SAVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally
            If dtp_Date.Enabled And dtp_Date.Visible Then dtp_Date.Focus()

        End Try

    End Sub


    Private Sub txt_Folding_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_Folding.KeyDown
        If e.KeyValue = 38 Then SendKeys.Send("+{TAB}")
        If (e.KeyValue = 40) Then
            If dgv_Details.Rows.Count > 0 Then

                dgv_Details.Focus()
                dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(1)

            Else
                btn_save.Focus()

            End If
        End If
    End Sub

    Private Sub txt_Folding_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Folding.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
            e.Handled = True
        End If
        If Asc(e.KeyChar) = 13 Then
            If dgv_Details.Rows.Count > 0 Then
                dgv_Details.Focus()
                dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(1)

            Else
                If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = Windows.Forms.DialogResult.Yes Then
                    save_record()
                Else
                    dtp_Date.Focus()
                End If

            End If
        End If
    End Sub

    Private Sub dgv_Details_CellEndEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellEndEdit
        dgv_Details_CellLeave(sender, e)
    End Sub

    Private Sub dgv_Details_CellEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellEnter
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim Dt2 As New DataTable
        Dim Dt3 As New DataTable
        Dim rect As Rectangle
        Dim n As Integer = 0

        With dgv_Details
            If Trim(.CurrentRow.Cells(0).Value) = "" Then
                n = .RowCount
                .CurrentRow.Cells(0).Value = Chr(65 + n)
            End If

            If e.ColumnIndex = 1 And Trim(.CurrentRow.Cells(10).Value) = "" Then

                If cbo_ClothName.Visible = False Or Val(cbo_ClothName.Tag) <> e.RowIndex Then

                    'Da = New SqlClient.SqlDataAdapter("select ClothType_Name from ClothType_Head where ClothType_IdNo Between 0 to 5 order by ClothType_Name", con)

                    cbo_ClothName.Tag = -1
                    Da = New SqlClient.SqlDataAdapter("select Cloth_Name from Cloth_Head order by Cloth_Name", con)
                    Dt2 = New DataTable
                    Da.Fill(Dt2)
                    cbo_ClothName.DataSource = Dt2
                    cbo_ClothName.DisplayMember = "Cloth_Name"

                    rect = .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False)

                    cbo_ClothName.Left = .Left + rect.Left  '  .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Left
                    cbo_ClothName.Top = .Top + rect.Top  ' .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Top
                    cbo_ClothName.Width = rect.Width  ' .CurrentCell.Size.Width
                    cbo_ClothName.Height = rect.Height  ' rect.Height

                    cbo_ClothName.Text = .CurrentCell.Value  '  Trim(.CurrentRow.Cells(e.ColumnIndex).Value)

                    cbo_ClothName.Tag = Val(e.RowIndex)
                    cbo_ClothName.Visible = True

                    cbo_ClothName.BringToFront()
                    cbo_ClothName.Focus()

                End If

            Else
                cbo_ClothName.Visible = False

            End If

            If Trim(.CurrentRow.Cells(2).Value) = "" And .CurrentRow.Index = 0 Then
                .CurrentRow.Cells(2).Value = Common_Procedures.ClothType_IdNoToName(con, 1)
            End If

            If e.ColumnIndex = 2 And Trim(.CurrentRow.Cells(10).Value) = "" Then

                If cbo_Grid_ClothType.Visible = False Or Val(cbo_Grid_ClothType.Tag) <> e.RowIndex Then

                    'Da = New SqlClient.SqlDataAdapter("select ClothType_Name from ClothType_Head where ClothType_IdNo Between 0 to 5 order by ClothType_Name", con)

                    cbo_Grid_ClothType.Tag = -1
                    Da = New SqlClient.SqlDataAdapter("select ClothType_Name from ClothType_Head where ClothType_Idno Between 0 and 5 order by ClothType_Name", con)
                    Dt2 = New DataTable
                    Da.Fill(Dt2)
                    cbo_Grid_ClothType.DataSource = Dt2
                    cbo_Grid_ClothType.DisplayMember = "ClothType_Name"

                    rect = .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False)

                    cbo_Grid_ClothType.Left = .Left + rect.Left  '  .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Left
                    cbo_Grid_ClothType.Top = .Top + rect.Top  ' .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Top
                    cbo_Grid_ClothType.Width = rect.Width  ' .CurrentCell.Size.Width
                    cbo_Grid_ClothType.Height = rect.Height  ' rect.Height

                    cbo_Grid_ClothType.Text = .CurrentCell.Value  '  Trim(.CurrentRow.Cells(e.ColumnIndex).Value)

                    cbo_Grid_ClothType.Tag = Val(e.RowIndex)
                    cbo_Grid_ClothType.Visible = True

                    cbo_Grid_ClothType.BringToFront()
                    cbo_Grid_ClothType.Focus()

                End If

            Else

                cbo_Grid_ClothType.Visible = False

            End If

        End With
    End Sub

    Private Sub dgv_Details_CellLeave(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellLeave
        With dgv_Details

            If .CurrentCell.ColumnIndex = 3 Or .CurrentCell.ColumnIndex = 4 Or .CurrentCell.ColumnIndex = 5 Then
                If Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value) <> 0 Then
                    .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = Format(Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value), "#########0.00")
                End If
            End If

            If .CurrentCell.ColumnIndex = 6 Or .CurrentCell.ColumnIndex = 7 Or .CurrentCell.ColumnIndex = 8 Then
                If Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value) <> 0 Then
                    .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = Format(Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value), "#########0.000")
                Else
                    .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = ""
                End If
            End If

        End With

    End Sub

    Private Sub dgv_Details_CellValueChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellValueChanged
        Try
            With dgv_Details
                If .Visible Then
                    If .Rows.Count > 0 Then
                        If e.ColumnIndex = 3 Or e.ColumnIndex = 5 Or e.ColumnIndex = 6 Or e.ColumnIndex = 7 Then

                            If Val(.CurrentRow.Cells(3).Value) <> 0 Then
                                .CurrentRow.Cells(8).Value = Format(Val(.CurrentRow.Cells(7).Value) / Val(.CurrentRow.Cells(3).Value), "#########0.000")
                            Else
                                .CurrentRow.Cells(8).Value = 0
                            End If

                            ConsumedPavu_Calculation()
                            ConsumedYarn_Calculation()
                            Total_Calculation()

                        End If
                    End If
                End If
            End With

        Catch ex As Exception
            '------

        End Try
    End Sub

    Private Sub dgv_Details_EditingControlShowing(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewEditingControlShowingEventArgs) Handles dgv_Details.EditingControlShowing
        dgtxt_Details = CType(dgv_Details.EditingControl, DataGridViewTextBoxEditingControl)
    End Sub

    Private Sub dgv_Details_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_Details.KeyUp
        Dim i As Integer
        Dim n As Integer
        Dim nrw As Integer
        Dim PNO As String
        Dim S As String


        If e.Control = True And (UCase(Chr(e.KeyCode)) = "A" Or UCase(Chr(e.KeyCode)) = "I" Or e.KeyCode = Keys.Insert) Then

            With dgv_Details

                n = .CurrentRow.Index

                PNO = Trim(UCase(.Rows(n).Cells(0).Value))

                If Common_Procedures.settings.ClothReceipt_PieceNo_Concept = "1,2,3" Then

                    S = Replace(Trim(PNO), Val(PNO), "")
                    PNO = Val(PNO)

                    If Trim(UCase(S)) <> "Z" Then
                        S = Trim(UCase(S))
                        If Trim(S) = "" Then S = "A" Else S = Trim(Chr(Asc(S) + 1))
                    End If

                Else


                    If Len(PNO) = 1 Then
                        S = "1"

                    Else

                        S = Microsoft.VisualBasic.Right(PNO, Len(PNO) - 1)
                        S = Val(S) + 1

                        PNO = Microsoft.VisualBasic.Left(PNO, 1)

                    End If

                End If

                If n <> .Rows.Count - 1 Then
                    If Trim(UCase(PNO)) & Trim(UCase(S)) = Trim(UCase(.Rows(n + 1).Cells(0).Value)) Then
                        MessageBox.Show("Already Piece Inserted", "DES NOT INSERT NEW PIECE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                        Exit Sub
                    End If
                End If

                nrw = n + 1

                dgv_Details.Rows.Insert(nrw, 1)

                dgv_Details.Rows(nrw).Cells(0).Value = Trim(UCase(PNO)) & S

                dgv_Details.Rows(nrw).Cells(1).Value = .Rows(n).Cells(1).Value
                dgv_Details.Rows(nrw).Cells(2).Value = .Rows(n).Cells(2).Value
                If Val(.Rows(n).Cells(4).Value) <> 0 Then
                    dgv_Details.Rows(nrw).Cells(4).Value = Val(.Rows(n).Cells(4).Value)
                End If

            End With

        End If

        If e.Control = True And UCase(Chr(e.KeyCode)) = "D" Then

            With dgv_Details

                If Trim(.CurrentRow.Cells(10).Value) = "" Then

                    n = .CurrentRow.Index

                    If .CurrentCell.RowIndex = .Rows.Count - 1 Then
                        For i = 0 To .ColumnCount - 1
                            .Rows(n).Cells(i).Value = ""
                        Next

                    Else
                        .Rows.RemoveAt(n)

                    End If

                    Total_Calculation()

                End If

            End With

        End If
    End Sub

    Private Sub dgv_Details_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgv_Details.LostFocus
        On Error Resume Next
        dgv_Details.CurrentCell.Selected = False
    End Sub

    'Private Sub dgv_Details_RowsAdded(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewRowsAddedEventArgs) Handles dgv_Details.RowsAdded
    '    Dim n As Integer
    '    With dgv_Details
    '        n = .RowCount
    '        .Rows(n - 1).Cells(0).Value = Chr(65 + n - 1)
    '    End With
    'End Sub

    Private Sub Total_Calculation()
        Dim Sno As Integer
        Dim TotMtrs As Single, TotWgt As Single, TotConsPav As Single, TotConsYrn As Single

        Sno = -1
        TotMtrs = 0
        TotWgt = 0
        TotConsPav = 0
        TotConsYrn = 0

        With dgv_Details
            For i = 0 To .RowCount - 1
                Sno = Sno + 1

                '.Rows(i).Cells(0).Value = Chr(65 + Sno)
                '.Rows(i).Cells(0).Value = Sno

                If Trim(.Rows(i).Cells(2).Value) <> "" Or Val(.Rows(i).Cells(3).Value) <> 0 Then
                    TotMtrs = TotMtrs + Val(.Rows(i).Cells(3).Value)
                    TotConsPav = TotConsPav + Val(.Rows(i).Cells(5).Value)
                    TotConsYrn = TotConsYrn + Val(.Rows(i).Cells(6).Value)
                    TotWgt = TotWgt + Val(.Rows(i).Cells(7).Value)

                End If
            Next
        End With

        With dgv_Details_Total
            If .RowCount = 0 Then .Rows.Add()
            .Rows(0).Cells(3).Value = Format(Val(TotMtrs), "#########0.00")
            .Rows(0).Cells(5).Value = Format(Val(TotConsPav), "#########0.00")
            .Rows(0).Cells(6).Value = Format(Val(TotConsYrn), "#########0.000")
            .Rows(0).Cells(7).Value = Format(Val(TotWgt), "#########0.000")

        End With

        lbl_ExcSht.Text = Format(Val(TotMtrs) - Val(lbl_RecMtrs.Text), "#########0.00")

    End Sub

    Private Sub cbo_Grid_ClothType_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Grid_ClothType.KeyDown

        With dgv_Details

            If .Rows.Count > 0 Then

                Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Grid_ClothType, Nothing, Nothing, "ClothType_Head", "ClothType_Name", "(ClothType_IdNo BetWeen 1 and 5)", "(ClothType_IdNo = 0)")

                If (e.KeyValue = 38 And cbo_Grid_ClothType.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
                    .Focus()
                    .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex - 1)

                End If

                If (e.KeyValue = 40 And cbo_Grid_ClothType.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then

                    .Focus()
                    .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 1)

                End If

            End If

        End With

    End Sub

    Private Sub cbo_Grid_Type_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Grid_ClothType.KeyPress

        With dgv_Details

            If .Rows.Count > 0 Then
                Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Grid_ClothType, Nothing, "ClothType_Head", "ClothType_Name", "(ClothType_IdNo BetWeen 1 and 5)", "(ClothType_IdNo = 0)")
                If Asc(e.KeyChar) = 13 Then

                    .Focus()
                    .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)

                End If
            End If
        End With
    End Sub

    Private Sub cbo_Grid_Type_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Grid_ClothType.TextChanged
        Try
            If cbo_Grid_ClothType.Visible Then
                With dgv_Details
                    If .Rows.Count > 0 Then
                        If Val(cbo_Grid_ClothType.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = 2 Then
                            .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_Grid_ClothType.Text)
                        End If
                    End If
                End With
            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub btn_close_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_close.Click
        Me.Close()
    End Sub

    Private Sub btn_save_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_save.Click
        save_record()
    End Sub

    Private Sub dgtxt_Details_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dgtxt_Details.KeyPress
        With dgv_Details
            If .Visible Then
                If .Rows.Count > 0 Then
                    If .CurrentCell.ColumnIndex = 2 Or .CurrentCell.ColumnIndex = 3 Or .CurrentCell.ColumnIndex = 4 Then
                        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
                            e.Handled = True
                        End If
                    End If
                End If
            End If
        End With
    End Sub

    Private Sub dgtxt_Details_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgtxt_Details.Enter
        dgv_Details.EditingControl.BackColor = Color.Lime
        dgv_Details.EditingControl.ForeColor = Color.Blue
        dgv_Details.SelectAll()
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
        Dim Led_IdNo As Integer, Del_IdNo As Integer, Cnt_IdNo As Integer
        Dim Condt As String = ""

        Try

            Condt = ""
            Del_IdNo = 0
            Cnt_IdNo = 0

            If IsDate(dtp_Filter_Fromdate.Value) = True And IsDate(dtp_Filter_ToDate.Value) = True Then
                Condt = "a.Weaver_Piece_Checking_Date between '" & Trim(Format(dtp_Filter_Fromdate.Value, "MM/dd/yyyy")) & "' and '" & Trim(Format(dtp_Filter_ToDate.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_Filter_Fromdate.Value) = True Then
                Condt = "a.Weaver_Piece_Checking_Date = '" & Trim(Format(dtp_Filter_Fromdate.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_Filter_ToDate.Value) = True Then
                Condt = "a.Weaver_Piece_Checking_Date = '" & Trim(Format(dtp_Filter_ToDate.Value, "MM/dd/yyyy")) & "' "
            End If

            If Trim(cbo_Filter_PartyName.Text) <> "" Then
                Led_IdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Filter_PartyName.Text)
            End If

            If Val(Led_IdNo) <> 0 Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " (a.DeliveryTo_IdNo = " & Str(Val(Led_IdNo)) & ")"
            End If

            da = New SqlClient.SqlDataAdapter("select a.*, e.Ledger_Name from Weaver_Piece_Checking_Head a inner join Ledger_head e on a.Ledger_IdNo = e.Ledger_idno where a.Receipt_Type = 'L' and a.company_idno =" & Str(Val(lbl_Company.Tag)) & "and a.Weaver_Piece_Checking_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' " & IIf(Trim(Condt) <> "", " and ", "") & Condt & " Order by a.Weaver_Piece_Checking_Date, a.for_orderby, a.Weaver_Piece_Checking_No", con)

            da.Fill(dt2)

            dgv_Filter_Details.Rows.Clear()

            If dt2.Rows.Count > 0 Then

                For i = 0 To dt2.Rows.Count - 1

                    n = dgv_Filter_Details.Rows.Add()

                    dgv_Filter_Details.Rows(n).Cells(0).Value = i + 1
                    dgv_Filter_Details.Rows(n).Cells(1).Value = dt2.Rows(i).Item("Weaver_Piece_Checking_No").ToString
                    dgv_Filter_Details.Rows(n).Cells(2).Value = Format(Convert.ToDateTime(dt2.Rows(i).Item("Weaver_Piece_Checking_Date").ToString), "dd-MM-yyyy")
                    dgv_Filter_Details.Rows(n).Cells(3).Value = dt2.Rows(i).Item("Ledger_Name").ToString
                    dgv_Filter_Details.Rows(n).Cells(4).Value = Format(Val(dt2.Rows(i).Item("Total_Checking_Meters").ToString), "########0.000")
                    dgv_Filter_Details.Rows(n).Cells(5).Value = Val(dt2.Rows(i).Item("Total_Weight").ToString)


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
    Private Sub cbo_ClothName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_ClothName.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_ClothName, Nothing, Nothing, "Cloth_Head", "Cloth_Name", "", "(Cloth_idno = 0)")
        With dgv_Details

            If (e.KeyValue = 38 And cbo_ClothName.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
                If .CurrentCell.RowIndex = 0 Then
                    If txt_Folding.Enabled Then txt_Folding.Focus() Else txt_Folding.Focus()

                Else
                    .Focus()
                    .CurrentCell = .Rows(.CurrentRow.Index - 1).Cells(.ColumnCount - 3)
                End If

            End If

            If (e.KeyValue = 40 And cbo_ClothName.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then

                If .CurrentCell.RowIndex = .Rows.Count - 1 And Trim(.Rows(.CurrentCell.RowIndex).Cells.Item(1).Value) = "" Then

                    If MessageBox.Show("Do you want to save ?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = DialogResult.Yes Then
                        save_record()
                    Else
                        dtp_date.Focus()
                    End If

                Else
                    .Focus()
                    .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)

                End If

            End If

        End With

    End Sub

    Private Sub cbo_ClothName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_ClothName.KeyPress
        Dim Da2 As New SqlClient.SqlDataAdapter
        Dim Dt2 As New DataTable
        Dim crimp As Single = 0
        Dim clth_idno As Integer = 0


        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_ClothName, Nothing, "Cloth_Head", "Cloth_Name", "", "(Cloth_idno = 0)")

        If Asc(e.KeyChar) = 13 Then

            With dgv_Details

                If Trim(cbo_ClothName.Text) <> "" Then

                    clth_idno = Common_Procedures.Cloth_NameToIdNo(con, Trim(cbo_ClothName.Text))

                    Da2 = New SqlClient.SqlDataAdapter("select a.* from Cloth_Head a Where a.Cloth_IdNo = " & Str(Val(clth_idno)), con)
                    Dt2 = New DataTable
                    Da2.Fill(Dt2)

                    crimp = 0
                    If Dt2.Rows.Count > 0 Then
                        If IsDBNull(Dt2.Rows(0)(0).ToString) = False Then
                            crimp = Val(Dt2.Rows(0).Item("Crimp_Percentage").ToString)
                        End If
                    End If

                    Dt2.Dispose()
                    Da2.Dispose()

                    If Val(crimp) <> 0 Then .Rows(.CurrentRow.Index).Cells(4).Value = Format(Val(crimp), "#########0.00")

                End If

                If .CurrentCell.RowIndex = .Rows.Count - 1 And Trim(.Rows(.CurrentCell.RowIndex).Cells.Item(1).Value) = "" Then
                    If MessageBox.Show("Do you want to save ?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = DialogResult.Yes Then
                        save_record()
                    Else
                        dtp_Date.Focus()
                    End If
                Else
                    .Focus()
                    .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)

                End If

            End With

        End If
    End Sub

    Private Sub cbo_ClothName_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_ClothName.TextChanged
        Try
            If cbo_ClothName.Visible Then
                With dgv_Details
                    If Val(cbo_ClothName.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = 1 Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_ClothName.Text)

                        ConsumedPavu_Calculation()
                        ConsumedYarn_Calculation()
                        Total_Calculation()
                    End If
                End With
            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub cbo_Filter_PartyName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Filter_PartyName.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_PartyName, dtp_Filter_ToDate, btn_Filter_Show, "Ledger_AlaisHead", "Ledger_DisplayName", " (  Ledger_Type = 'WEAVER' OR Ledger_Type = 'JOBWORKER')", "(Ledger_idno = 0)")
    End Sub

    Private Sub cbo_Filter_PartyName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Filter_PartyName.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Filter_PartyName, btn_Filter_Show, "Ledger_AlaisHead", "Ledger_DisplayName", " (  Ledger_Type = 'WEAVER' OR Ledger_Type = 'JOBWORKER')", "(Ledger_idno = 0)")
    End Sub

    Private Sub ConsumedPavu_Calculation()
        Dim CloID As Integer
        Dim ConsPavu As Single
        Dim LmID As Integer
        Dim NoofBeams As Integer = 0

        CloID = Common_Procedures.Cloth_NameToIdNo(con, dgv_Details.CurrentRow.Cells(1).Value)

        ConsPavu = Common_Procedures.get_Pavu_Consumption(con, CloID, LmID, dgv_Details.CurrentRow.Cells(3).Value, Trim(lbl_WidthType.Text), , dgv_Details.CurrentRow.Cells(4).Value)

        dgv_Details.CurrentRow.Cells(5).Value = Format(ConsPavu, "#########0.00")

    End Sub

    Private Sub ConsumedYarn_Calculation()
        Dim CloID As Integer
        Dim ConsYarn As Single
        'Dim WgtMtr As Single

        CloID = Common_Procedures.Cloth_NameToIdNo(con, dgv_Details.CurrentRow.Cells(1).Value)

        ConsYarn = Common_Procedures.get_Weft_ConsumedYarn(con, CloID, Val(dgv_Details.CurrentRow.Cells(3).Value))

        ''WgtMtr = Val(Common_Procedures.get_FieldValue(con, "cloth_head", "Weight_Meter_Weft", "(cloth_idno = " & Str(Val(CloID)) & ")"))
        ''ConsYarn = Val(txt_Meters.Text) * Val(WgtMtr)

        dgv_Details.CurrentRow.Cells(6).Value = Format(ConsYarn, "#########0.000")

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

    Public Sub print_record() Implements Interface_MDIActions.print_record
        '-----
    End Sub

    Private Sub cbo_QualityName_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs)
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Dim f As New Cloth_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_ClothName.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If
    End Sub

    Public Sub Get_LotDetails(ByVal LtNo As String)
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim Dt2 As New DataTable
        Dim Dt3 As New DataTable
        Dim LtCd As String
        Dim n As Integer

        LtCd = LtNo
        If Not (Trim(LtNo) Like "*/??-??") Then LtCd = LtCd & "/" & Trim(Common_Procedures.FnYearCode)
        LtCd = Trim(Val(lbl_Company.Tag)) & "-" & Trim(LtCd)

        Da = New SqlClient.SqlDataAdapter("Select Weaver_Piece_Checking_No from Weaver_Piece_Checking_Head where Receipt_PkCondition = '" & Trim(Pk_Condition2) & "' and Piece_Receipt_Code = '" & Trim(LtCd) & "' and Receipt_Type = 'L'", con)
        Dt1 = New DataTable
        Da.Fill(Dt1)
        If Dt1.Rows.Count > 0 Then
            Call move_record(Dt1.Rows(0).Item("Weaver_Piece_Checking_No").ToString)

        Else

            new_record()

            txt_RollNo.Text = Trim(LtNo)

            Da = New SqlClient.SqlDataAdapter("select a.*, b.ledger_name, c.cloth_name , d.Loom_Name , E.* , e.Crimp_Percentage AS Crimp , e.ReceiptMeters_Receipt as ReceiptMeter , e.ConsumedPavu_Receipt as ConsumedPavu , e.ConsumedYarn_Receipt as ConsumedYarn from Weaver_Cloth_Receipt_Head a LEFT OUTER JOIN Weaver_ClothReceipt_Piece_Details e ON e.Weaver_ClothReceipt_Code = 'PCDOF-' +  a.Weaver_ClothReceipt_Code LEFT OUTER JOIN Loom_Head d ON a.Loom_IdNo = d.Loom_IdNo , ledger_head b, cloth_head c  where a.Weaver_ClothReceipt_Code = '" & Trim(LtCd) & "' and a.Receipt_Type = 'L' and a.ledger_idno = b.ledger_idno and E.cloth_idno = c.cloth_idno ", con)
            Dt2 = New DataTable
            Da.Fill(Dt2)

            If Dt2.Rows.Count > 0 Then
                If IsDBNull(Dt2.Rows(0).Item("Weaver_Piece_Checking_Date").ToString) = False Then
                    If IsDate(Dt2.Rows(0).Item("Weaver_Piece_Checking_Date").ToString) = True Then
                        dtp_Date.Text = Dt2.Rows(0).Item("Weaver_Piece_Checking_Date").ToString
                    End If
                End If
                With dgv_Details
                    For i = 0 To Dt2.Rows.Count - 1

                        n = .Rows.Add()

                        .Rows(n).Cells(0).Value = Dt2.Rows(i).Item("Piece_No").ToString
                        .Rows(n).Cells(1).Value = Dt2.Rows(i).Item("Cloth_Name").ToString
                        .Rows(n).Cells(2).Value = Common_Procedures.ClothType_IdNoToName(con, 1)  'Trim(UCase("SOUND"))
                        .Rows(n).Cells(3).Value = Format(Val(Dt2.Rows(i).Item("ReceiptMeter").ToString), "########0.00")
                        .Rows(n).Cells(4).Value = Format(Val(Dt2.Rows(i).Item("Crimp").ToString), "########0.00")
                        .Rows(n).Cells(5).Value = Format(Val(Dt2.Rows(i).Item("ConsumedPavu").ToString), "########0.00")
                        .Rows(n).Cells(6).Value = Format(Val(Dt2.Rows(i).Item("ConsumedYarn").ToString), "########0.000")

                    Next i
                End With

                lbl_RecCode.Text = Dt2.Rows(0).Item("Weaver_ClothReceipt_Code").ToString
                lbl_RecDate.Text = Dt2.Rows(0).Item("Weaver_ClothReceipt_Date").ToString
                lbl_PartyName.Text = Dt2.Rows(0).Item("ledger_name").ToString
                lbl_RecMtrs.Text = Dt2.Rows(0).Item("Receipt_Meters").ToString
                txt_Folding.Text = Val(Dt2.Rows(0).Item("Folding").ToString)
                lbl_LoomNo.Text = Dt2.Rows(0).Item("Loom_Name").ToString
                lbl_WidthType.Text = Dt2.Rows(0).Item("Width_Type").ToString



            End If
            Dt2.Clear()

        End If
        Dt1.Clear()

        Dt1.Dispose()
        Dt2.Dispose()
        Da.Dispose()

    End Sub

    Private Sub txt_RollNo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_RollNo.KeyPress
        If Asc(e.KeyChar) = 13 Then
            If Trim(txt_RollNo.Text) <> "" Then
                If Trim(txt_RollNo.Text) <> Trim(txt_RollNo.Tag) Then
                    Get_LotDetails(txt_RollNo.Text)

                    txt_RollNo.Tag = txt_RollNo.Text
                End If
            End If
            If txt_Folding.Enabled Then
                txt_Folding.Focus()
            Else
                dgv_Details.Focus()
                dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(1)
            End If
        End If
    End Sub

    Private Sub btn_Selection_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Selection.Click
        Get_LotDetails(txt_RollNo.Text)
        If txt_Folding.Enabled Then txt_Folding.Focus()
    End Sub

    Private Sub dgtxt_Details_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgtxt_Details.KeyUp
        dgv_Details_KeyUp(sender, e)
    End Sub

    Private Sub dgv_Details_CellContentClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellContentClick

    End Sub
End Class