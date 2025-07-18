Public Class OE_Mixing_Entry

    Implements Interface_MDIActions

    Private con As New SqlClient.SqlConnection(Common_Procedures.Connection_String)
    Private FrmLdSTS As Boolean = False
    Private New_Entry As Boolean = False
    Private Insert_Entry As Boolean = False
    Private Filter_Status As Boolean = False
    Private Pk_Condition As String = "CTNMX-"
    Private Prec_ActCtrl As New Control
    Private vcbo_KeyDwnVal As Double
    Private prn_HdDt As New DataTable
    Private prn_DetDt As New DataTable
    Private prn_PageNo As Integer
    Private prn_DetIndx As Integer
    Private prn_DetSNo As Integer

    Private vPrn_PvuEdsCnt As String
    Private vPrn_PvuTotBms As Integer
    Private vPrn_PvuTotMtrs As Single
    Private vPrn_PvuSetNo As String
    Private vPrn_PvuBmNos1 As String
    Private vPrn_PvuBmNos2 As String
    Private vPrn_PvuBmNos3 As String
    Private vPrn_PvuBmNos4 As String
    Private WithEvents dgtxt_MixingDetails As New DataGridViewTextBoxEditingControl
    Private WithEvents dgtxt_WasteDetails As New DataGridViewTextBoxEditingControl
    Private dgv_ActCtrlName As String = ""
    Private dgv_LevColNo As Integer

    Private Sub clear()
        New_Entry = False
        Insert_Entry = False

        pnl_Back.Enabled = True
        pnl_Filter.Visible = False
        pnl_Selection.Visible = False
        lbl_MixNo.Text = ""
        lbl_MixNo.ForeColor = Color.Black

        dtp_Date.Text = ""
        lbl_NetWgt.Text = ""
        cbo_Count.Text = ""
        txt_Time.Text = ""
        txt_TareWgt.Text = ""
        txt_Remarks.Text = ""
        dgv_MixingDetails.Rows.Clear()
        dgv_wasteDetails.Rows.Clear()

        dgv_MixingDetails_Total.Rows.Clear()
        dgv_MixingDetails_Total.Rows.Add()

        dgv_WasteDetails_Total.Rows.Clear()
        dgv_WasteDetails_Total.Rows.Add()

        Grid_DeSelect()

        cbo_Variety.Visible = False
        cbo_Variety.Tag = -1
        cbo_LotNo.Visible = False
        cbo_LotNo.Tag = -1
        cbo_Closed.Visible = False
        cbo_Closed.Tag = -1

        cbo_Colour.Visible = False
        cbo_Colour.Tag = -1
        cbo_GridVariety_Waste.Visible = False
        cbo_GridVariety_Waste.Tag = -1
        cbo_GridLotNo.Visible = False
        cbo_GridLotNo.Tag = -1

        cbo_Variety.Text = ""
        cbo_LotNo.Text = ""
        cbo_Closed.Text = "YES"

        cbo_Colour.Text = ""
        cbo_GridVariety_Waste.Text = ""
        cbo_GridLotNo.Text = ""

        'dgv_Details.Tag = ""
        'dgv_LevColNo = -1

        dgv_ActCtrlName = ""

    End Sub

    Private Sub Grid_DeSelect()
        On Error Resume Next
        If Not IsNothing(dgv_MixingDetails.CurrentCell) Then dgv_MixingDetails.CurrentCell.Selected = False
        If Not IsNothing(dgv_wasteDetails.CurrentCell) Then dgv_wasteDetails.CurrentCell.Selected = False
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

        If Me.ActiveControl.Name <> cbo_Variety.Name Then
            cbo_Variety.Visible = False
        End If
        If Me.ActiveControl.Name <> cbo_LotNo.Name Then
            cbo_LotNo.Visible = False
        End If

        If Me.ActiveControl.Name <> cbo_Closed.Name Then
            cbo_Closed.Visible = False
        End If

        If Me.ActiveControl.Name <> cbo_Colour.Name Then
            cbo_Colour.Visible = False
        End If
        If Me.ActiveControl.Name <> cbo_GridVariety_Waste.Name Then
            cbo_GridVariety_Waste.Visible = False
        End If
        If Me.ActiveControl.Name <> cbo_GridLotNo.Name Then
            cbo_GridLotNo.Visible = False
        End If

        If Me.ActiveControl.Name <> dgv_MixingDetails.Name Then
            Grid_DeSelect()
        End If

        If Me.ActiveControl.Name <> dgv_wasteDetails.Name Then
            Grid_DeSelect()
        End If

        'If Me.ActiveControl.Name <> dgv_MixingDetails.Name Then
        '    Common_Procedures.Hide_CurrentStock_Display()
        'End If

        Prec_ActCtrl = Me.ActiveControl

    End Sub

    Private Sub ControlLostFocus(ByVal sender As Object, ByVal e As System.EventArgs)

        On Error Resume Next

        If IsNothing(Prec_ActCtrl) = False Then
            If TypeOf Prec_ActCtrl Is TextBox Or TypeOf Prec_ActCtrl Is ComboBox Then
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

    Private Sub Grid_Cell_DeSelect()
        On Error Resume Next
        If Not IsNothing(dgv_wasteDetails.CurrentCell) Then dgv_wasteDetails.CurrentCell.Selected = False
        If Not IsNothing(dgv_MixingDetails.CurrentCell) Then dgv_MixingDetails.CurrentCell.Selected = False
        If Not IsNothing(dgv_MixingDetails_Total.CurrentCell) Then dgv_MixingDetails_Total.CurrentCell.Selected = False
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
        Dim LockSTS As Boolean = False
        If Val(no) = 0 Then Exit Sub

        clear()

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(no) & "/" & Trim(Common_Procedures.FnYearCode)

        Try

            da1 = New SqlClient.SqlDataAdapter("select a.*, b.Count_Name from Mixing_Head a INNER JOIN Count_Head b ON a.Count_IdNo = b.Count_IdNo   Where a.Mixing_Code = '" & Trim(NewCode) & "'", con)
            da1.Fill(dt1)

            If dt1.Rows.Count > 0 Then

                lbl_MixNo.Text = dt1.Rows(0).Item("Mixing_No").ToString
                dtp_Date.Text = dt1.Rows(0).Item("Mixing_Date").ToString
                cbo_Count.Text = dt1.Rows(0).Item("Count_Name").ToString
                txt_Time.Text = (dt1.Rows(0).Item("Mixing_Time_Text").ToString)
                txt_TareWgt.Text = Format(Val(dt1.Rows(0).Item("Tare_Weight").ToString), "########0.000")
                lbl_GrossWeight.Text = Format(Val(dt1.Rows(0).Item("Gross_Weight").ToString), "########0.000")
                lbl_NetWgt.Text = Format(Val(dt1.Rows(0).Item("Net_Weight").ToString), "########0.000")
                txt_Remarks.Text = dt1.Rows(0).Item("Remarks").ToString

                'If IsDBNull(dt1.Rows(0).Item("BobinSales_Invoice_Code").ToString) = False Then
                '    If Trim(dt1.Rows(0).Item("BobinSales_Invoice_Code").ToString) <> "" Then LockSTS = True
                'End If

                da2 = New SqlClient.SqlDataAdapter("select a.*,  c.Variety_Name from Mixing_Details a LEFT OUTER JOIN Variety_Head c ON a.Variety_IdNo = c.Variety_IdNo  Where a.Mixing_Code = '" & Trim(NewCode) & "' Order by a.Sl_No", con)
                dt2 = New DataTable
                da2.Fill(dt2)

                dgv_MixingDetails.Rows.Clear()
                SNo = 0

                If dt2.Rows.Count > 0 Then

                    For i = 0 To dt2.Rows.Count - 1

                        n = dgv_MixingDetails.Rows.Add()


                        SNo = SNo + 1
                        dgv_MixingDetails.Rows(n).Cells(0).Value = Val(SNo)
                        dgv_MixingDetails.Rows(n).Cells(1).Value = dt2.Rows(i).Item("Variety_Name").ToString
                        dgv_MixingDetails.Rows(n).Cells(2).Value = dt2.Rows(i).Item("Lot_No").ToString
                        dgv_MixingDetails.Rows(n).Cells(3).Value = dt2.Rows(i).Item("Bale").ToString

                        dgv_MixingDetails.Rows(n).Cells(4).Value = dt2.Rows(i).Item("Bale_No").ToString

                        dgv_MixingDetails.Rows(n).Cells(5).Value = Format(Val(dt2.Rows(i).Item("Mixing_Weight").ToString), "########0.000")
                        dgv_MixingDetails.Rows(n).Cells(6).Value = dt2.Rows(i).Item("Bale_Closed").ToString
                        dgv_MixingDetails.Rows(n).Cells(7).Value = dt2.Rows(i).Item("Cotton_Purchase_Code").ToString
                        dgv_MixingDetails.Rows(n).Cells(8).Value = dt2.Rows(i).Item("Cotton_Purchase_Details_SlNo").ToString

                        '    For j = 0 To dgv_WasteDetails.ColumnCount - 1
                        '        dgv_WasteDetails.Rows(n).Cells(j).Style.BackColor = Color.LightGray
                        '    Next j
                        '    LockSTS = True
                        'End If
                    Next i

                End If
                dt2.Clear()

                With dgv_MixingDetails_Total
                    If .RowCount = 0 Then .Rows.Add()
                    .Rows(0).Cells(3).Value = Val(dt1.Rows(0).Item("Total_Bales").ToString)
                    .Rows(0).Cells(4).Value = Val(dt1.Rows(0).Item("Total_Mixing_BaleNo").ToString)
                    .Rows(0).Cells(5).Value = Format(Val(dt1.Rows(0).Item("Total_Mixing_Weight").ToString), "########0.000")
                End With


                da2 = New SqlClient.SqlDataAdapter("select a.*, b.Variety_Name, c.Count_Name from Mixing_Waste_Details a LEFT OUTER JOIN Variety_Head b ON a.Variety_IdNo = b.Variety_IdNo LEFT OUTER JOIN Count_Head c ON a.Count_IdNo = c.Count_IdNo  where a.Mixing_Code = '" & Trim(NewCode) & "' Order by a.Sl_No", con)
                dt2 = New DataTable
                da2.Fill(dt2)

                dgv_wasteDetails.Rows.Clear()
                SNo = 0

                If dt2.Rows.Count > 0 Then

                    For i = 0 To dt2.Rows.Count - 1

                        n = dgv_wasteDetails.Rows.Add()

                        SNo = SNo + 1
                        dgv_wasteDetails.Rows(n).Cells(0).Value = Val(SNo)

                        dgv_wasteDetails.Rows(n).Cells(1).Value = dt2.Rows(i).Item("Variety_Name").ToString
                        dgv_wasteDetails.Rows(n).Cells(2).Value = dt2.Rows(i).Item("Lot_No").ToString
                        dgv_wasteDetails.Rows(n).Cells(3).Value = dt2.Rows(i).Item("Bale").ToString
                        dgv_wasteDetails.Rows(n).Cells(4).Value = dt2.Rows(i).Item("Bale_No").ToString
                        dgv_wasteDetails.Rows(n).Cells(5).Value = Format(Val(dt2.Rows(i).Item("Weight").ToString), "########0.000")
                        dgv_wasteDetails.Rows(n).Cells(6).Value = dt2.Rows(i).Item("Cotton_Waste_Code").ToString
                        dgv_wasteDetails.Rows(n).Cells(7).Value = Val(dt2.Rows(i).Item("Cotton_Waste_Details_Slno").ToString)
                    Next i

                End If

                With dgv_WasteDetails_Total
                    If .RowCount = 0 Then .Rows.Add()
                    .Rows(0).Cells(3).Value = Val(dt1.Rows(0).Item("Total_WasteBale").ToString)
                    .Rows(0).Cells(4).Value = Val(dt1.Rows(0).Item("Total_Waste_baleNo").ToString)
                    .Rows(0).Cells(5).Value = Format(Val(dt1.Rows(0).Item("Total_Weight").ToString), "########0.000")
                End With

            End If
            dt1.Clear()

            'If LockSTS = True Then
            '    cbo_Ledger.Enabled = False
            '    cbo_Ledger.BackColor = Color.LightGray

            '    cbo_GridLotNo.Enabled = False
            '    cbo_GridLotNo.BackColor = Color.LightGray

            '    cbo_GridVariety.Enabled = False
            '    cbo_GridVariety.BackColor = Color.LightGray

            '    cbo_Colour.Enabled = False
            '    cbo_Colour.BackColor = Color.LightGray

            '    cbo_Variety.Enabled = False
            '    cbo_Variety.BackColor = Color.LightGray

            '    cbo_LotNo.Enabled = False
            '    cbo_LotNo.BackColor = Color.LightGray

            '    cbo_Closed.Enabled = False
            '    cbo_Closed.BackColor = Color.LightGray

            '    dgv_MixingDetails.ReadOnly = True
            '    dgv_WasteDetails.ReadOnly = True

            'End If

            Grid_DeSelect()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally
            dgv_ActCtrlName = ""
            dt1.Dispose()
            da1.Dispose()
            dt2.Dispose()
            da2.Dispose()

        End Try

        If dtp_Date.Visible And dtp_Date.Enabled Then dtp_Date.Focus()

    End Sub

    Private Sub Cotton_Mixing_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Activated

        Try

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Count.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "LEDGER" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Count.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If



            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Variety.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "VARIETY" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Variety.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_LotNo.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "LOT" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_LotNo.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Colour.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "COLOUR" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Colour.Text = Trim(Common_Procedures.Master_Return.Return_Value)
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
            '----MessageBox.Show(ex.Message, "DOES NOT SHOW...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        FrmLdSTS = False

    End Sub

    Private Sub Cotton_Mixing_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load


        Me.Text = ""

        con.Open()



        cbo_Closed.Items.Clear()
        cbo_Closed.Items.Add("YES")
        cbo_Closed.Items.Add("NO")

        ' cbo_Variety.Visible = False


        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1155" Then    'krg

            dgv_MixingDetails.Columns(1).Width = dgv_wasteDetails.Columns(1).Width
            dgv_MixingDetails.Columns(4).Visible = False
            dgv_MixingDetails_Total.Columns(4).Visible = False
            dgv_wasteDetails.Columns(4).Visible = False
            dgv_WasteDetails_Total.Columns(4).Visible = False


        End If

        pnl_Filter.Visible = False
        pnl_Filter.Left = (Me.Width - pnl_Filter.Width) \ 2
        pnl_Filter.Top = (Me.Height - pnl_Filter.Height) \ 2


        pnl_Selection.Visible = False
        pnl_Selection.Left = (Me.Width - pnl_Selection.Width) \ 2
        pnl_Selection.Top = (Me.Height - pnl_Selection.Height) \ 2
        pnl_Selection.BringToFront()


        Pnl_Waste_Selection.Visible = False
        Pnl_Waste_Selection.Left = (Me.Width - Pnl_Waste_Selection.Width) \ 2
        Pnl_Waste_Selection.Top = (Me.Height - Pnl_Waste_Selection.Height) \ 2
        Pnl_Waste_Selection.BringToFront()

        AddHandler dtp_Date.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Variety.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_LotNo.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Closed.GotFocus, AddressOf ControlGotFocus

        AddHandler cbo_GridVariety_Waste.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Colour.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_GridLotNo.GotFocus, AddressOf ControlGotFocus

        AddHandler cbo_Count.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Time.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_TareWgt.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Remarks.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Waste_BaleNoSelection.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Waste_LotNoSelection.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_BaleNoSelection.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_LotNoSelection.GotFocus, AddressOf ControlGotFocus

        AddHandler cbo_Filter_PartyName.GotFocus, AddressOf ControlGotFocus


        AddHandler dtp_Date.LostFocus, AddressOf ControlLostFocus

        AddHandler cbo_Variety.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_LotNo.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Closed.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Colour.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_GridVariety_Waste.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_GridLotNo.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Count.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Time.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_TareWgt.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Remarks.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Waste_BaleNoSelection.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Waste_LotNoSelection.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_BaleNoSelection.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_LotNoSelection.LostFocus, AddressOf ControlLostFocus


        AddHandler cbo_Filter_PartyName.LostFocus, AddressOf ControlLostFocus

        AddHandler dtp_Date.KeyDown, AddressOf TextBoxControlKeyDown

        'AddHandler txt_Remarks.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Time.KeyDown, AddressOf TextBoxControlKeyDown

        AddHandler dtp_Date.KeyPress, AddressOf TextBoxControlKeyPress

        AddHandler dtp_Filter_Fromdate.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler dtp_Filter_ToDate.KeyDown, AddressOf TextBoxControlKeyDown

        AddHandler dtp_Filter_Fromdate.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler dtp_Filter_ToDate.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Time.KeyPress, AddressOf TextBoxControlKeyPress

        lbl_Company.Text = ""
        lbl_Company.Tag = 0
        lbl_Company.Visible = False
        Common_Procedures.CompIdNo = 0

        Filter_Status = False
        FrmLdSTS = True
        new_record()

    End Sub

    Private Sub Cotton_Mixing_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles MyBase.FormClosed
        con.Close()
        con.Dispose()
    End Sub

    Private Sub Cotton_Mixing_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles MyBase.KeyPress

        Try
            If Asc(e.KeyChar) = 27 Then

                If pnl_Filter.Visible = True Then
                    btn_Filter_Close_Click(sender, e)
                    Exit Sub
                ElseIf pnl_Selection.Visible = True Then
                    btn_Close_Selection_Click(sender, e)
                    Exit Sub
                    'ElseIf Pnl_Waste_Selection.Visible = True Then
                    '    btn_Waste_Close_Selection_Click(sender, e)
                    '    Exit Sub
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
        Dim i As Integer

        If ActiveControl.Name = dgv_MixingDetails.Name Or ActiveControl.Name = dgv_wasteDetails.Name Or TypeOf ActiveControl Is DataGridViewTextBoxEditingControl Then

            On Error Resume Next

            dgv1 = Nothing

            If ActiveControl.Name = dgv_MixingDetails.Name Then
                dgv1 = dgv_MixingDetails

            ElseIf dgv_MixingDetails.IsCurrentRowDirty = True Then
                dgv1 = dgv_MixingDetails

            ElseIf dgv_ActCtrlName = dgv_MixingDetails.Name Then
                dgv1 = dgv_MixingDetails


            ElseIf ActiveControl.Name = dgv_wasteDetails.Name Then
                dgv1 = dgv_wasteDetails

            ElseIf dgv_wasteDetails.IsCurrentRowDirty = True Then
                dgv1 = dgv_wasteDetails

            ElseIf dgv_ActCtrlName = dgv_wasteDetails.Name Then
                dgv1 = dgv_wasteDetails
            End If

            If IsNothing(dgv1) = True Then
                Return MyBase.ProcessCmdKey(msg, keyData)
                Exit Function
            End If

            With dgv1

                If dgv1.Name = dgv_MixingDetails.Name Then

                    If keyData = Keys.Enter Or keyData = Keys.Down Then
                        If .CurrentCell.ColumnIndex >= .ColumnCount - 2 Then
                            If .CurrentCell.RowIndex = .RowCount - 1 Then

                                If dgv_wasteDetails.Rows.Count > 0 Then
                                    dgv_wasteDetails.Focus()
                                    dgv_wasteDetails.CurrentCell = dgv_wasteDetails.Rows(0).Cells(1)
                                    dgv_wasteDetails.CurrentCell.Selected = True
                                End If


                            Else
                                .Focus()
                                .CurrentCell = .Rows(.CurrentCell.RowIndex + 1).Cells(1)

                            End If

                        Else

                            If .CurrentCell.RowIndex = .RowCount - 1 And .CurrentCell.ColumnIndex >= 1 And ((.CurrentCell.ColumnIndex <> 1 And Val(.CurrentRow.Cells(1).Value) = 0) Or (.CurrentCell.ColumnIndex = 1 And Val(dgtxt_MixingDetails.Text) = 0)) Then
                                For i = 0 To .Columns.Count - 1
                                    .Rows(.CurrentCell.RowIndex).Cells(i).Value = ""
                                Next

                                If dgv_wasteDetails.Rows.Count > 0 Then
                                    dgv_wasteDetails.Focus()
                                    dgv_wasteDetails.CurrentCell = dgv_wasteDetails.Rows(0).Cells(1)
                                    dgv_wasteDetails.CurrentCell.Selected = True
                                End If

                            ElseIf .CurrentCell.ColumnIndex = 3 Then
                                If .Columns(4).Visible = True Then
                                    .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 1)
                                Else
                                    .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 2)

                                End If


                            Else
                                .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 1)

                            End If
                        End If

                        Return True

                    ElseIf keyData = Keys.Up Then

                        If .CurrentCell.ColumnIndex <= 1 Then
                            If .CurrentCell.RowIndex = 0 Then

                                cbo_Count.Focus()

                            Else
                                .Focus()
                                .CurrentCell = .Rows(.CurrentCell.RowIndex - 1).Cells(.ColumnCount - 2)

                            End If
                        ElseIf .CurrentCell.ColumnIndex = 5 Then
                            If .Columns(4).Visible = True Then
                                .Focus()
                                .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex - 1)
                            Else
                                .Focus()
                                .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex - 2)

                            End If

                        Else
                            .Focus()
                            .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex - 1)

                        End If

                        Return True

                    Else
                        Return MyBase.ProcessCmdKey(msg, keyData)

                    End If


                ElseIf dgv1.Name = dgv_wasteDetails.Name Then

                    If keyData = Keys.Enter Or keyData = Keys.Down Then
                        If .CurrentCell.ColumnIndex >= .ColumnCount - 3 Then
                            If .CurrentCell.RowIndex = .RowCount - 1 Then

                                txt_TareWgt.Focus()

                            Else
                                .Focus()
                                .CurrentCell = .Rows(.CurrentCell.RowIndex + 1).Cells(1)


                            End If

                        Else

                            If .CurrentCell.RowIndex = .RowCount - 1 And .CurrentCell.ColumnIndex >= 1 And ((.CurrentCell.ColumnIndex <> 1 And Val(.CurrentRow.Cells(1).Value) = 0) Or (.CurrentCell.ColumnIndex = 1 And Val(dgtxt_WasteDetails.Text) = 0)) Then
                                For i = 0 To .Columns.Count - 1
                                    .Rows(.CurrentCell.RowIndex).Cells(i).Value = ""
                                Next

                                txt_TareWgt.Focus()
                            ElseIf .CurrentCell.ColumnIndex = 3 Then
                                If .Columns(4).Visible = True Then
                                    .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 1)
                                Else
                                    .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 2)

                                End If
                            Else
                                .Focus()
                                .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 1)

                            End If


                        End If

                        Return True

                    ElseIf keyData = Keys.Up Then

                        If .CurrentCell.ColumnIndex <= 1 Then
                            If .CurrentCell.RowIndex = 0 Then
                                dgv_MixingDetails.Focus()
                                dgv_MixingDetails.CurrentCell = dgv_MixingDetails.Rows(0).Cells(4)


                            Else
                                .CurrentCell = .Rows(.CurrentCell.RowIndex - 1).Cells(.ColumnCount - 1)

                            End If
                        ElseIf .CurrentCell.ColumnIndex = 5 Then
                            If .Columns(4).Visible = True Then
                                .Focus()
                                .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex - 1)
                            Else
                                .Focus()
                                .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex - 2)

                            End If
                        Else
                            .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex - 1)

                        End If

                        Return True

                    Else
                        Return MyBase.ProcessCmdKey(msg, keyData)

                    End If

                End If

            End With

        Else

            Return MyBase.ProcessCmdKey(msg, keyData)

        End If

    End Function


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
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim cmd As New SqlClient.SqlCommand
        Dim trans As SqlClient.SqlTransaction
        Dim NewCode As String = ""

        ' If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.Processing_Receipt_Entry, "~L~") = 0 And InStr(Common_Procedures.UR.Processing_Receipt_Entry, "~D~") = 0 Then MessageBox.Show("You have No Rights to Delete", "DOES NOT INSERT...", MessageBoxButtons.OK, MessageBoxIcon.Error) : Exit Sub

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_MixNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)
        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.DeletingEntry, Common_Procedures.UR.OEENTRY_MIXING_ENTRY, New_Entry, Me, con, "Mixing_Head", "Mixing_Code", NewCode, "Mixing_Date", "(Mixing_Code = '" & Trim(NewCode) & "')") = False Then Exit Sub


        If MessageBox.Show("Do you want to Delete?", "FOR DELETION...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) <> Windows.Forms.DialogResult.Yes Then
            Exit Sub
        End If

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_MixNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)
        If New_Entry = True Then
            MessageBox.Show("This is New Entry", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        trans = con.BeginTransaction

        Try

            cmd.Connection = con
            cmd.Transaction = trans

            cmd.CommandText = "Delete from Stock_Waste_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()
            cmd.CommandText = "Delete from Stock_Cotton_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Stock_Mixing_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Update Cotton_Purchase_Bale_Details set Mixing_Code = '',Mixing_Increment = Mixing_Increment - 1 Where Mixing_Code = '" & Trim(NewCode) & "' "
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Mixing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Mixing_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Mixing_Waste_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Mixing_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "delete from Mixing_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Mixing_Code = '" & Trim(NewCode) & "'"
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

        If dtp_Date.Enabled = True And dtp_Date.Visible = True Then dtp_Date.Focus()

    End Sub

    Public Sub filter_record() Implements Interface_MDIActions.filter_record

        If Filter_Status = False Then

            Dim da As New SqlClient.SqlDataAdapter
            Dim dt1 As New DataTable
            Dim dt2 As New DataTable
            Dim dt3 As New DataTable

            'da = New SqlClient.SqlDataAdapter("select Ledger_DisplayName from Ledger_AlaisHead order by Ledger_DisplayName", con)
            'da.Fill(dt1)
            'cbo_Filter_PartyName.DataSource = dt1
            'cbo_Filter_PartyName.DisplayMember = "Ledger_DisplayName"

            'da = New SqlClient.SqlDataAdapter("select EndsCount_name from EndsCount_head order by EndsCount_name", con)
            'da.Fill(dt2)
            'cbo_Filter_EndsName.DataSource = dt2
            'cbo_Filter_EndsName.DisplayMember = "EndsCount_name"


            dtp_Filter_Fromdate.Text = ""
            dtp_Filter_ToDate.Text = ""
            cbo_Filter_PartyName.Text = ""


            cbo_Filter_PartyName.SelectedIndex = -1

            dgv_Filter_Details.Rows.Clear()

        End If

        pnl_Filter.Visible = True
        pnl_Filter.Enabled = True
        pnl_Filter.BringToFront()
        pnl_Back.Enabled = False
        If dtp_Filter_Fromdate.Enabled And dtp_Filter_Fromdate.Visible Then dtp_Filter_Fromdate.Focus()

    End Sub

    Public Sub movefirst_record() Implements Interface_MDIActions.movefirst_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim movno As String = ""

        Try

            da = New SqlClient.SqlDataAdapter("select top 1 Mixing_No from Mixing_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Mixing_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby, Mixing_No", con)
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

            OrdByNo = Common_Procedures.OrderBy_CodeToValue(Trim(lbl_MixNo.Text))

            da = New SqlClient.SqlDataAdapter("select top 1 Mixing_No from Mixing_Head where for_orderby > " & Str(Val(OrdByNo)) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and Mixing_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby, Mixing_No", con)
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

            OrdByNo = Common_Procedures.OrderBy_CodeToValue(Trim(lbl_MixNo.Text))

            da = New SqlClient.SqlDataAdapter("select top 1 Mixing_No from Mixing_Head where for_orderby < " & Str(Val(OrdByNo)) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and Mixing_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, Mixing_No desc", con)
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
            da = New SqlClient.SqlDataAdapter("select top 1 Mixing_No from Mixing_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Mixing_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, Mixing_No desc", con)
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

            lbl_MixNo.Text = Common_Procedures.get_MaxCode(con, "Mixing_Head", "Mixing_Code", "For_OrderBy", "", Val(lbl_Company.Tag), Common_Procedures.FnYearCode)

            lbl_MixNo.ForeColor = Color.Red

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

            inpno = InputBox("Enter Mix.No.", "FOR FINDING...")

            RecCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

            Da = New SqlClient.SqlDataAdapter("select Mixing_No from Mixing_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Mixing_Code = '" & Trim(RecCode) & "'", con)
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
                MessageBox.Show("Mix No. does not exists", "DOES NOT FIND...", MessageBoxButtons.OK, MessageBoxIcon.Error)

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

        'If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.Processing_Receipt_Entry, "~L~") = 0 And InStr(Common_Procedures.UR.Processing_Receipt_Entry, "~I~") = 0 Then MessageBox.Show("You have No Rights to Insert", "DOES NOT INSERT...", MessageBoxButtons.OK, MessageBoxIcon.Error) : Exit Sub
        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.InsertingEntry, Common_Procedures.UR.OEENTRY_MIXING_ENTRY, New_Entry, Me) = False Then Exit Sub

        Try

            inpno = InputBox("Enter New Mix No.", "FOR NEW DELIVERY INSERTION...")

            RecCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

            Da = New SqlClient.SqlDataAdapter("select Mixing_No from Mixing_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Mixing_Code = '" & Trim(RecCode) & "'", con)
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
                    MessageBox.Show("Invalid Mix No", "DOES NOT INSERT NEW DELIVERY...", MessageBoxButtons.OK, MessageBoxIcon.Error)

                Else
                    new_record()
                    Insert_Entry = True
                    lbl_MixNo.Text = Trim(UCase(inpno))

                End If

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT INSERT NEW DELIVERY...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Public Sub save_record() Implements Interface_MDIActions.save_record
        Dim cmd As New SqlClient.SqlCommand
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim tr As SqlClient.SqlTransaction
        Dim NewCode As String = ""
        Dim Cnt_ID As Integer = 0
        Dim Ens_ID As Integer = 0
        Dim Sno As Integer = 0
        Dim Partcls As String = ""
        Dim EntID As String = ""
        Dim GdLt_ID As Integer = 0
        Dim Delv_ID As Integer = 0
        Dim Rec_ID As Integer = 0
        Dim Lt_ID As Integer = 0
        Dim Clr_ID As Integer = 0
        Dim Vrty_ID As Integer = 0
        Dim Gdvrty_ID As Integer = 0
        Dim vEdsCnt_ID As Integer = 0
        Dim PBlNo As String = ""
        Dim vTotMixBlNo As Single, vTotMixWgt As Single, vTotBales As Single
        Dim vTotWstBlNo As Single, vTotWstWgt As Single, vTotWstBale As Single
        Dim Nr As Integer = 0
        Dim vOrdByNo As Single = 0

        If Val(lbl_Company.Tag) = 0 Then
            MessageBox.Show("Invalid Company Selection", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        ' If Common_Procedures.UserRight_Check(Common_Procedures.UR.Processing_Receipt_Entry, New_Entry) = False Then Exit Sub

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_MixNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)
        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.SavingEntry, Common_Procedures.UR.OEENTRY_MIXING_ENTRY, New_Entry, Me, con, "Mixing_Head", "Mixing_Code", NewCode, "Mixing_Date", "(Mixing_Code = '" & Trim(NewCode) & "')", "(Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Mixing_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "')", "for_Orderby desc, Mixing_No desc", dtp_Date.Value.Date) = False Then Exit Sub


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

        Cnt_ID = Common_Procedures.Count_NameToIdNo(con, cbo_Count.Text)
        If Cnt_ID = 0 Then
            MessageBox.Show("Invalid Count Name", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If cbo_Count.Enabled And cbo_Count.Visible Then cbo_Count.Focus()
            Exit Sub
        End If

        Delv_ID = 0  ' Led_ID

        Rec_ID = Val(Common_Procedures.CommonLedger.Godown_Ac)


        With dgv_MixingDetails

            For i = 0 To .RowCount - 1

                If Val(.Rows(i).Cells(5).Value) <> 0 Then

                    If Trim(.Rows(i).Cells(1).Value) = "" Then

                        MessageBox.Show("Invalid Variety Name", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        If .Enabled And .Visible Then
                            .Focus()
                            .CurrentCell = .Rows(i).Cells(1)
                        End If

                        Exit Sub

                    End If

                    If Val(.Rows(i).Cells(5).Value) = 0 Then
                        MessageBox.Show("Invalid Weight..", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        If .Enabled Then .Focus()
                        .CurrentCell = .Rows(0).Cells(5)
                        Exit Sub
                    End If

                End If

            Next
        End With

        With dgv_wasteDetails

            For i = 0 To .RowCount - 1

                If Val(.Rows(i).Cells(5).Value) <> 0 Then

                    If Trim(.Rows(i).Cells(1).Value) = "" Then

                        MessageBox.Show("Invalid Variety Name", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        If .Enabled And .Visible Then
                            .Focus()
                            .CurrentCell = .Rows(i).Cells(1)
                        End If

                        Exit Sub

                    End If

                    If Val(.Rows(i).Cells(5).Value) = 0 Then
                        MessageBox.Show("Invalid Weight..", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        If .Enabled Then .Focus()
                        .CurrentCell = .Rows(0).Cells(5)
                        Exit Sub
                    End If

                End If

            Next

        End With

        Total_Calculation()

        vTotMixBlNo = 0 : vTotMixWgt = 0 : vTotBales = 0
        If dgv_MixingDetails_Total.RowCount > 0 Then
            vTotBales = Val(dgv_MixingDetails_Total.Rows(0).Cells(3).Value())
            vTotMixBlNo = Val(dgv_MixingDetails_Total.Rows(0).Cells(4).Value())
            vTotMixWgt = Val(dgv_MixingDetails_Total.Rows(0).Cells(5).Value())
        End If

        vTotWstBlNo = 0 : vTotWstWgt = 0 : vTotWstBale = 0
        If dgv_WasteDetails_Total.RowCount > 0 Then
            vTotWstBale = Val(dgv_WasteDetails_Total.Rows(0).Cells(3).Value())
            vTotWstBlNo = Val(dgv_WasteDetails_Total.Rows(0).Cells(4).Value())
            vTotWstWgt = Val(dgv_WasteDetails_Total.Rows(0).Cells(5).Value())
        End If

        'If (Val(txt_OurBobin.Text) + Val(txt_PartyBobin.Text)) <> Val(vTotBbns) Then
        '    MessageBox.Show("Invalid Bobins..", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
        '    If txt_PartyBobin.Enabled Then txt_PartyBobin.Focus()
        '    Exit Sub
        'End If

        tr = con.BeginTransaction

        Try

            If Insert_Entry = True Or New_Entry = False Then
                NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_MixNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            Else

                lbl_MixNo.Text = Common_Procedures.get_MaxCode(con, "Mixing_Head", "Mixing_Code", "For_OrderBy", "", Val(lbl_Company.Tag), Common_Procedures.FnYearCode, tr)

                NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_MixNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            End If



            cmd.Connection = con
            cmd.Transaction = tr

            cmd.Parameters.Clear()
            cmd.Parameters.AddWithValue("@MixDate", dtp_Date.Value.Date)

            vOrdByNo = Val(Common_Procedures.OrderBy_CodeToValue(lbl_MixNo.Text))

            If New_Entry = True Then

                cmd.CommandText = "Insert into Mixing_Head ( Mixing_Code, Company_IdNo, Mixing_No, for_OrderBy, Mixing_Date, Count_IdNo, Mixing_Time_Text, Total_Mixing_BaleNo, Total_Mixing_Weight, Total_Waste_baleNo, Total_Weight, Gross_Weight ,Tare_Weight, Net_Weight,  Remarks , Total_Bales ,Total_WasteBale ) Values ('" & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_MixNo.Text) & "', " & Str(vOrdByNo) & ", @MixDate, " & Str(Val(Cnt_ID)) & ",'" & Trim(txt_Time.Text) & "', " & Str(Val(vTotMixBlNo)) & " , " & Str(Val(vTotMixWgt)) & ",  " & Str(Val(vTotWstBlNo)) & " , " & Str(Val(vTotWstWgt)) & ", " & Val(lbl_GrossWeight.Text) & ", " & Str(Val(txt_TareWgt.Text)) & " , " & Str(Val(lbl_NetWgt.Text)) & ",  '" & Trim(txt_Remarks.Text) & "' ," & Str(Val(vTotBales)) & " , " & Val(vTotWstBale) & ")"
                cmd.ExecuteNonQuery()

            Else
                cmd.CommandText = "Update Mixing_Head set Mixing_Date = @MixDate, Count_IdNo = " & Val(Cnt_ID) & ",  Mixing_Time_Text = '" & Trim(txt_Time.Text) & "', Total_Mixing_baleNo = " & Str(Val(vTotMixBlNo)) & ", Total_Mixing_Weight = " & Val(vTotMixWgt) & " , Total_Waste_BaleNo = " & Val(vTotWstBlNo) & ", Total_Weight = " & Val(vTotWstWgt) & ",Gross_Weight = " & Val(lbl_GrossWeight.Text) & " ,Total_Bales = " & Val(vTotBales) & " ,Total_WasteBale = " & Val(vTotWstBale) & " , Tare_Weight = " & Val(txt_TareWgt.Text) & ", Net_Weight = " & Val(lbl_NetWgt.Text) & ",  Remarks = '" & Trim(txt_Remarks.Text) & "' Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Mixing_Code = '" & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()

                cmd.CommandText = "Update Cotton_Purchase_Bale_Details set Mixing_Code = '', Mixing_Increment = Mixing_Increment - 1 Where Mixing_Code = '" & Trim(NewCode) & "' "
                cmd.ExecuteNonQuery()

            End If


            Partcls = "Mix : Mix.No. " & Trim(lbl_MixNo.Text)
            PBlNo = Trim(lbl_MixNo.Text)
            EntID = Trim(Pk_Condition) & Trim(lbl_MixNo.Text)

            cmd.CommandText = "Delete from Mixing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Mixing_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Stock_Cotton_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Mixing_Waste_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Mixing_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()
            cmd.CommandText = "Delete from Stock_Waste_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Stock_Mixing_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            With dgv_MixingDetails
                Sno = 0
                For i = 0 To .RowCount - 1

                    If Trim(.Rows(i).Cells(1).Value) <> "" And Val(.Rows(i).Cells(5).Value) <> 0 Then

                        Sno = Sno + 1

                        Vrty_ID = Common_Procedures.Variety_NameToIdNo(con, .Rows(i).Cells(1).Value, tr)

                        cmd.CommandText = "Insert into Mixing_Details ( Mixing_Code, Company_IdNo, Mixing_No, for_OrderBy, Mixing_Date, Sl_No,Count_IdNo, Variety_IdNo , Lot_No, bale , Bale_No,  Mixing_Weight, Bale_Closed ,Cotton_Purchase_Code ,Cotton_Purchase_Details_SlNo ) Values ('" & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_MixNo.Text) & "', " & Str(vOrdByNo) & ", @MixDate," & Str(Val(Sno)) & "," & Str(Val(Cnt_ID)) & ", " & Str(Val(Vrty_ID)) & " , '" & Trim(.Rows(i).Cells(2).Value) & "' , " & Val(.Rows(i).Cells(3).Value) & " , '" & Trim(.Rows(i).Cells(4).Value) & "' ,  " & Val(.Rows(i).Cells(5).Value) & ",  '" & Trim(.Rows(i).Cells(6).Value) & "','" & Trim(.Rows(i).Cells(7).Value) & "' ," & Val(.Rows(i).Cells(8).Value) & " )"
                        cmd.ExecuteNonQuery()

                        Nr = 0
                        cmd.CommandText = "Update Cotton_Purchase_Bale_Details set Mixing_Code = '" & Trim(NewCode) & "' , Mixing_Increment = Mixing_Increment + 1  Where Cotton_Purchase_Code = '" & Trim(.Rows(i).Cells(7).Value) & "' and Detail_SlNo = " & Val(.Rows(i).Cells(8).Value) & " and Bale_No ='" & Trim(.Rows(i).Cells(4).Value) & "' "
                        Nr = cmd.ExecuteNonQuery()

                        cmd.CommandText = "Insert into Stock_cotton_Processing_Details ( Reference_Code                        ,             Company_IdNo         ,           Reference_No        ,                               For_OrderBy             ,        Reference_Date,     Party_Bill_No     , Entry_ID               ,  Sl_No         ,       Variety_IdNo   ,   Bale      ,         Weight             ) " &
                                                             "   Values  ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_MixNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_MixNo.Text))) & ",    @MixDate   , '" & Trim(PBlNo) & "', '" & Trim(Partcls) & "' ," & Str(Val(Sno)) & ", " & Str(Val(Vrty_ID)) & ",  " & Str(-1 * Val(.Rows(i).Cells(3).Value)) & " ," & Str(-1 * Val(.Rows(i).Cells(5).Value)) & " )"
                        cmd.ExecuteNonQuery()

                    End If

                Next

            End With



            With dgv_wasteDetails

                Sno = 0
                For i = 0 To .RowCount - 1

                    If Val(.Rows(i).Cells(5).Value) <> 0 Then

                        Sno = Sno + 1

                        ' Clr_ID = Common_Procedures.Colour_NameToIdNo(con, .Rows(i).Cells(1).Value, tr)
                        Gdvrty_ID = Common_Procedures.Variety_NameToIdNo(con, .Rows(i).Cells(1).Value, tr)

                        cmd.CommandText = "Insert into Mixing_Waste_Details ( Mixing_Code, Company_IdNo, Mixing_No, for_OrderBy, Mixing_Date, Sl_No, Count_IdNo, Variety_IdNo, Lot_No, bale , Bale_No , Weight,Cotton_Waste_Code,Cotton_Waste_Details_slno) Values ('" & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_MixNo.Text) & "', " & Str(vOrdByNo) & ", @MixDate," & Str(Val(Sno)) & "," & Val(Cnt_ID) & " ,   " & Str(Val(Gdvrty_ID)) & ", '" & Trim(.Rows(i).Cells(2).Value) & "', " & Str(Val(.Rows(i).Cells(3).Value)) & " ,'" & Trim(.Rows(i).Cells(4).Value) & "', " & Val(.Rows(i).Cells(5).Value) & ",'" & Trim(.Rows(i).Cells(6).Value) & "', " & Val(.Rows(i).Cells(7).Value) & ")"
                        cmd.ExecuteNonQuery()


                        cmd.CommandText = "Insert into Stock_Waste_Processing_Details (      SoftwareType_IdNo                           ,                    Reference_Code           ,             Company_IdNo         ,           Reference_No        ,                               For_OrderBy                         ,        Reference_Date,     Party_Bill_No   , Entry_ID       ,   Sl_No      ,              Count_IdNo      ,   Variety_IdNo           ,                                  Lot_No      ,                      Bale_No              ,                           Bale                 ,                           Weight               ) " &
                                                               "   Values  (" & Str(Val(Common_Procedures.SoftwareTypes.OE_Software)) & ", '" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_MixNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_MixNo.Text))) & ",    @MixDate   , '" & Trim(PBlNo) & "', '" & Trim(EntID) & "' ," & Str(Val(Sno)) & ", " & Str(Val(Cnt_ID)) & "," & Str(Val(Gdvrty_ID)) & ",  '" & Trim(.Rows(i).Cells(2).Value) & "','" & Trim(.Rows(i).Cells(4).Value) & "', " & Str(-1 * Val(.Rows(i).Cells(3).Value)) & " , " & Str(-1 * Val(.Rows(i).Cells(5).Value)) & " ) "
                        cmd.ExecuteNonQuery()

                    End If

                Next

            End With
            cmd.CommandText = "Insert into Stock_Mixing_Processing_Details ( Reference_Code                        ,             Company_IdNo         ,           Reference_No        ,                               For_OrderBy                         ,        Reference_Date,  Entry_ID          ,   Party_Bill_No   ,   Sl_No              ,   Count_IdNo   ,      Weight             ) " &
                                                           "   Values  ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_MixNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_MixNo.Text))) & ",    @MixDate   , '" & Trim(EntID) & "' ,'" & Trim(PBlNo) & "', " & Str(Val(Sno)) & ", " & Str(Val(Cnt_ID)) & ",  " & Val(lbl_NetWgt.Text) & " )"
            cmd.ExecuteNonQuery()

            tr.Commit()

            Dt1.Dispose()
            Da.Dispose()

            If Val(Common_Procedures.settings.OnSave_MoveTo_NewEntry_Status) = 1 Then
                If New_Entry = True Then
                    new_record()
                Else
                    move_record(Trim(lbl_MixNo.Text))
                End If
            Else
                move_record(Trim(lbl_MixNo.Text))
            End If

            MessageBox.Show("Saved Sucessfully!!!", "FOR SAVING...", MessageBoxButtons.OK, MessageBoxIcon.Information)

        Catch ex As Exception
            tr.Rollback()
            MessageBox.Show(ex.Message, "FOR SAVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        If dtp_Date.Enabled And dtp_Date.Visible Then dtp_Date.Focus()

    End Sub

    Private Sub Total_Calculation()
        Dim vTotMixBlNo As Single, vTotmixWgt As Single, vTotBale As Single
        Dim vTotWstBlNo As Single, vTotWstWgt As Single, vTotWstBale As Single
        Dim i As Integer
        Dim sno As Integer
        'Dim vGrsWgt As Single

        vTotMixBlNo = 0 : vTotmixWgt = 0 : vTotBale = 0
        With dgv_MixingDetails
            For i = 0 To .Rows.Count - 1

                sno = sno + 1

                .Rows(i).Cells(0).Value = sno

                If Val(.Rows(i).Cells(3).Value) <> 0 Or Val(.Rows(i).Cells(5).Value) <> 0 Then
                    vTotBale = vTotBale + Val(.Rows(i).Cells(3).Value)
                    ' vTotMixBlNo = vTotMixBlNo + 1
                    vTotmixWgt = vTotmixWgt + Val(.Rows(i).Cells(5).Value)
                    If Trim(.Rows(i).Cells(4).Value) <> "" Then
                        vTotMixBlNo = vTotMixBlNo + 1
                    End If
                End If
            Next
        End With

        If dgv_MixingDetails_Total.Rows.Count <= 0 Then dgv_MixingDetails_Total.Rows.Add()
        dgv_MixingDetails_Total.Rows(0).Cells(3).Value = Val(vTotBale)
        dgv_MixingDetails_Total.Rows(0).Cells(4).Value = Val(vTotMixBlNo)
        dgv_MixingDetails_Total.Rows(0).Cells(5).Value = Format(Val(vTotmixWgt), "#########0.00")
        'If Val(dgv_MixingDetails.Rows(i).Cells(3).Value) <> 0 Then

        'End If
        vTotWstBlNo = 0 : vTotWstWgt = 0 : vTotWstBale = 0
        sno = 0
        With dgv_wasteDetails
            For i = 0 To .Rows.Count - 1

                sno = sno + 1

                .Rows(i).Cells(0).Value = sno

                If Val(.Rows(i).Cells(3).Value) <> 0 Or Val(.Rows(i).Cells(5).Value) <> 0 Then
                    vTotWstBlNo = vTotWstBlNo + 1
                    vTotWstBale = vTotWstBale + Val(.Rows(i).Cells(3).Value)
                    vTotWstWgt = vTotWstWgt + Val(.Rows(i).Cells(5).Value)

                End If

            Next
        End With

        If dgv_WasteDetails_Total.Rows.Count <= 0 Then dgv_WasteDetails_Total.Rows.Add()
        dgv_WasteDetails_Total.Rows(0).Cells(3).Value = Val(vTotWstBale)
        dgv_WasteDetails_Total.Rows(0).Cells(4).Value = Val(vTotWstBlNo)
        dgv_WasteDetails_Total.Rows(0).Cells(5).Value = Format(Val(vTotWstWgt), "#########0.000")

        '    lbl_GrossWeight.Text = Val(vTotmixWgt) + Val(vTotWstWgt)
        lbl_NetWgt.Text = Format(Val(vTotmixWgt) + Val(vTotWstWgt) - Val(txt_TareWgt.Text), "###########0.0000")


    End Sub

    'Private Sub Meters_Calculation()
    '    Dim i As Integer
    '    Dim sno As Integer
    '    Dim vtotMtrs As Single

    '    vtotMtrs = 0 : sno = 0
    '    With dgv_MixingDetails
    '        For i = 0 To dgv_MixingDetails.Rows.Count - 1

    '            sno = sno + 1

    '            .Rows(i).Cells(0).Value = sno


    '            vtotMtrs = Val(dgv_MixingDetails.Rows(i).Cells(4).Value) * Val(dgv_MixingDetails.Rows(i).Cells(5).Value)

    '            dgv_MixingDetails.Rows(i).Cells(6).Value = Format(Val(vtotMtrs), "#########0.00")

    '        Next
    '    End With
    '    Total_Calculation()

    'End Sub

    Private Sub cbo_Ledger_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Count.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Count_Head", "Count_Name", "", "(Count_idno = 0)")
    End Sub

    Private Sub cbo_Ledger_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Count.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Count, txt_Time, Nothing, "Count_Head", "Count_Name", "", "(Count_idno = 0)")

        'With dgv_MixingDetails

        If (e.KeyValue = 40 And cbo_Count.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then


            If dgv_MixingDetails.Rows.Count > 0 Then
                dgv_MixingDetails.Focus()
                dgv_MixingDetails.CurrentCell = dgv_MixingDetails.Rows(0).Cells(1)
                dgv_MixingDetails.CurrentCell.Selected = True
            Else
                dgv_wasteDetails.Focus()
                dgv_wasteDetails.CurrentCell = dgv_wasteDetails.Rows(0).Cells(1)
                dgv_wasteDetails.CurrentCell.Selected = True

            End If



        End If
        ' End With
    End Sub

    Private Sub cbo_Ledger_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Count.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Count, Nothing, "Count_Head", "Count_Name", "", "(Count_idno = 0)")

        If Asc(e.KeyChar) = 13 Then
            If MessageBox.Show("Do you want to select Purchase :", "FOR PURCHASE SELECTION...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = DialogResult.Yes Then
                btn_Selection_Click(sender, e)

            Else
                If dgv_MixingDetails.RowCount > 0 Then
                    If dgv_MixingDetails.Rows.Count > 0 Then
                        dgv_MixingDetails.Focus()
                        dgv_MixingDetails.CurrentCell = dgv_MixingDetails.Rows(0).Cells(1)
                        dgv_MixingDetails.CurrentCell.Selected = True

                    End If

                End If
                'If MessageBox.Show("Do you want to select Waste Details :", "FOR WASTE SELECTION...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = DialogResult.Yes Then
                '    btn_Waste_Selection_Click(sender, e)
                'End If
            End If

        End If




    End Sub

    Private Sub cbo_Ledger_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Count.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then


            Dim f As New Count_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Count.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()
        End If
    End Sub

    Private Sub dgv_MixingDetails_CellEndEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_MixingDetails.CellEndEdit
        Try
            With dgv_MixingDetails

                If .CurrentCell.ColumnIndex = 5 Then
                    If Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value) <> 0 Then
                        .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = Format(Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value), "#########0.000")
                    End If
                End If

                'Meters_Calculation()

            End With

        Catch ex As Exception
            '-----
        End Try
    End Sub

    Private Sub dgv_MixingDetails_CellEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_MixingDetails.CellEnter
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim Dt2 As New DataTable
        Dim Dt3 As New DataTable
        Dim Dt4 As New DataTable
        Dim rect As Rectangle

        Try

            With dgv_MixingDetails

                dgv_ActCtrlName = .Name

                If Val(.CurrentRow.Cells(0).Value) = 0 Then
                    .CurrentRow.Cells(0).Value = .CurrentRow.Index + 1
                End If

                'If e.ColumnIndex = 2 Then

                '    If cbo_LotNo.Visible = False Or Val(cbo_LotNo.Tag) <> e.RowIndex Then

                '        cbo_LotNo.Tag = -1
                '        Da = New SqlClient.SqlDataAdapter("select Lot_No from Lot_Head order by Lot_No", con)
                '        Dt2 = New DataTable
                '        Da.Fill(Dt2)
                '        cbo_LotNo.DataSource = Dt2
                '        cbo_LotNo.DisplayMember = "Lot_No"

                '        rect = .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False)

                '        cbo_LotNo.Left = .Left + rect.Left  '  .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Left
                '        cbo_LotNo.Top = .Top + rect.Top  ' .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Top
                '        cbo_LotNo.Width = rect.Width  ' .CurrentCell.Size.Width
                '        cbo_LotNo.Height = rect.Height  ' rect.Height

                '        cbo_LotNo.Text = .CurrentCell.Value  '  Trim(.CurrentRow.Cells(e.ColumnIndex).Value)

                '        cbo_LotNo.Tag = Val(e.RowIndex)
                '        cbo_LotNo.Visible = True

                '        cbo_LotNo.BringToFront()
                '        cbo_LotNo.Focus()

                '    End If

                'Else


                '    cbo_LotNo.Visible = False

                'End If

                If e.ColumnIndex = 1 Then

                    If cbo_Variety.Visible = False Or Val(cbo_Variety.Tag) <> e.RowIndex Then

                        cbo_Variety.Tag = -1
                        Da = New SqlClient.SqlDataAdapter("select Variety_Name from Variety_Head WHERE variety_type <> 'WASTE' order by Variety_Name", con)
                        Dt2 = New DataTable
                        Da.Fill(Dt2)
                        cbo_Variety.DataSource = Dt2
                        cbo_Variety.DisplayMember = "Variety_Name"

                        rect = .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False)

                        cbo_Variety.Left = .Left + rect.Left  '  .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Left
                        cbo_Variety.Top = .Top + rect.Top  ' .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Top
                        cbo_Variety.Width = rect.Width  ' .CurrentCell.Size.Width
                        cbo_Variety.Height = rect.Height  ' rect.Height

                        cbo_Variety.Text = .CurrentCell.Value  '  Trim(.CurrentRow.Cells(e.ColumnIndex).Value)

                        cbo_Variety.Tag = Val(e.RowIndex)
                        cbo_Variety.Visible = True

                        cbo_Variety.BringToFront()
                        cbo_Variety.Focus()


                    End If
                Else
                    cbo_Variety.Visible = False


                End If
                If e.ColumnIndex = 6 Then

                    If cbo_Closed.Visible = False Or Val(cbo_Closed.Tag) <> e.RowIndex Then

                        rect = .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False)

                        cbo_Closed.Left = .Left + rect.Left
                        cbo_Closed.Top = .Top + rect.Top

                        cbo_Closed.Width = rect.Width
                        cbo_Closed.Height = rect.Height
                        cbo_Closed.Text = .CurrentCell.Value

                        cbo_Closed.Tag = Val(e.RowIndex)
                        cbo_Closed.Visible = True

                        cbo_Closed.BringToFront()
                        cbo_Closed.Focus()

                    End If

                Else
                    cbo_Closed.Visible = False

                End If

            End With

        Catch ex As Exception
            '-----

        End Try

    End Sub

    Private Sub dgv_MixingDetails_CellLeave(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_MixingDetails.CellLeave

        Try
            With dgv_MixingDetails

                If .CurrentCell.ColumnIndex = 5 Then
                    If Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value) <> 0 Then
                        .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = Format(Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value), "#########0.000")
                    End If
                End If

            End With

        Catch ex As Exception
            '-----
        End Try

    End Sub

    Private Sub dgv_MixingDetails_CellValueChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_MixingDetails.CellValueChanged
        Try
            If IsNothing(dgv_MixingDetails.CurrentCell) Then Exit Sub
            With dgv_MixingDetails
                If .Visible Then
                    If .Rows.Count > 0 Then
                        If .CurrentCell.ColumnIndex = 3 Or .CurrentCell.ColumnIndex = 4 Or .CurrentCell.ColumnIndex = 5 Then
                            Total_Calculation()
                        End If
                    End If
                End If
            End With

        Catch ex As Exception
            '-----
        End Try
    End Sub

    Private Sub dgv_MixingDetails_EditingControlShowing(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewEditingControlShowingEventArgs) Handles dgv_MixingDetails.EditingControlShowing
        Try
            dgtxt_MixingDetails = CType(dgv_MixingDetails.EditingControl, DataGridViewTextBoxEditingControl)
        Catch ex As Exception
            '----
        End Try
    End Sub

    Private Sub dgtxt_mIXINGDetails_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgtxt_MixingDetails.Enter
        Try
            dgv_ActCtrlName = dgv_MixingDetails.Name
            dgv_MixingDetails.EditingControl.BackColor = Color.Lime
            dgv_MixingDetails.EditingControl.ForeColor = Color.Blue
            dgtxt_MixingDetails.SelectAll()
        Catch ex As Exception
            '----
        End Try
    End Sub

    Private Sub dgtxt_MixingDetails_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgtxt_MixingDetails.KeyDown
        Try
            vcbo_KeyDwnVal = e.KeyValue

        Catch ex As Exception
            MessageBox.Show(ex.Message, "ERROR WHILE DETAILS TEXT KEYDOWN....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub dgtxt_BobinDetails_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dgtxt_MixingDetails.KeyPress

        Try
            With dgv_MixingDetails

                If Val(dgv_MixingDetails.CurrentCell.ColumnIndex.ToString) = 5 Or Val(dgv_MixingDetails.CurrentCell.ColumnIndex.ToString) = 3 Then

                    If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
                        e.Handled = True
                    End If
                End If

            End With

        Catch ex As Exception
            '-----

        End Try

    End Sub

    Private Sub dgv_MixingDetails_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_MixingDetails.KeyUp
        Dim n As Integer = 0

        Try
            If e.Control = True And UCase(Chr(e.KeyCode)) = "D" Then

                With dgv_MixingDetails

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

        Catch ex As Exception
            '------
        End Try

    End Sub


    Private Sub dgv_Details_RowsAdded(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewRowsAddedEventArgs) Handles dgv_MixingDetails.RowsAdded
        Dim n As Integer = 0

        Try
            With dgv_MixingDetails
                n = .RowCount
                .Rows(n - 1).Cells(0).Value = Val(n)
                .Rows(n - 1).Cells(6).Value = "YES"
            End With

        Catch ex As Exception
            '-----

        End Try
    End Sub

    Private Sub dgv_MixingDetails_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgv_MixingDetails.LostFocus
        On Error Resume Next
        dgv_MixingDetails.CurrentCell.Selected = False
    End Sub

    Private Sub cbo_Variety_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Variety.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Variety_Head", "Variety_Name", "(variety_type = '')", "(Variety_IdNo = 0)")
    End Sub

    Private Sub cbo_Variety_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Variety.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Variety, Nothing, Nothing, "Variety_Head", "Variety_Name", "(variety_type = '')", "(Variety_IdNo = 0)")

        With dgv_MixingDetails
            If (e.KeyValue = 38 And cbo_Variety.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
                '.Focus()
                '.CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex - 1)
                If Val(.CurrentCell.RowIndex) <= 0 Then
                    cbo_Count.Focus()
                Else
                    .Focus()
                    .CurrentCell = .Rows(.CurrentCell.RowIndex - 1).Cells(.ColumnCount - 3)
                    .CurrentCell.Selected = True

                End If
            End If

            If (e.KeyValue = 40 And cbo_Variety.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
                .Focus()
                .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 1)
            End If
        End With
    End Sub

    Private Sub cbo_Variety_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Variety.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Variety, Nothing, "Variety_Head", "Variety_Name", "(variety_type = '')", "(Variety_IdNo = 0)")

        If Asc(e.KeyChar) = 13 Then

            With dgv_MixingDetails

                'If .Rows.Count > 0 Then
                '    .Focus()
                '    .Rows(.CurrentCell.RowIndex).Cells.Item(1).Value = Trim(cbo_Variety.Text)
                '    .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)
                'End If

                .Rows(.CurrentCell.RowIndex).Cells.Item(1).Value = Trim(cbo_Variety.Text)
                If .CurrentCell.RowIndex = .RowCount - 1 And .CurrentCell.ColumnIndex >= 1 And Trim(.CurrentRow.Cells(1).Value) = "" Then
                    If dgv_wasteDetails.Rows.Count > 0 Then
                        dgv_wasteDetails.Focus()
                        dgv_wasteDetails.CurrentCell = dgv_wasteDetails.Rows(0).Cells(1)
                        dgv_wasteDetails.CurrentCell.Selected = True

                    Else
                        txt_Remarks.Focus()

                    End If

                Else
                    .Focus()
                    .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)
                End If
            End With

        End If
    End Sub

    Private Sub cbo_Variety_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Variety.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Dim f As New Variety_Creation("")

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Variety.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If
    End Sub


    Private Sub cbo_Variety_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Variety.TextChanged
        Try
            If cbo_Variety.Visible Then
                With dgv_MixingDetails
                    If .Rows.Count > 0 Then
                        If Val(cbo_Variety.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = 1 Then
                            .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_Variety.Text)
                        End If
                    End If
                End With
            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub cbo_LotNo_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_LotNo.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Lot_Head", "Lot_No", "", "(Lot_IdNo = 0)")
    End Sub

    Private Sub cbo_LotNo_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_LotNo.KeyDown
        Dim dep_idno As Integer = 0

        Try
            vcbo_KeyDwnVal = e.KeyValue

            Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_LotNo, Nothing, Nothing, "Lot_Head", "Lot_No", "", "(Lot_IdNo = 0)")
            With dgv_MixingDetails
                If (e.KeyValue = 38 And cbo_LotNo.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
                    .Focus()
                    .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex - 1)
                End If

                If (e.KeyValue = 40 And cbo_LotNo.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then

                    .Focus()
                    .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(3)

                End If

            End With

        Catch ex As Exception
            '-----

        End Try

    End Sub

    Private Sub cbo_LotNo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_LotNo.KeyPress
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable

        Try
            Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_LotNo, Nothing, "Lot_Head", "Lot_No", "", "(Lot_IdNo = 0)")

            If Asc(e.KeyChar) = 13 Then
                With dgv_MixingDetails

                    .Rows(.CurrentCell.RowIndex).Cells.Item(2).Value = Trim(cbo_LotNo.Text)
                    If .CurrentCell.ColumnIndex >= 2 Then
                        .Focus()
                        .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(3)
                    End If
                End With
            End If

        Catch ex As Exception
            '-----
        End Try

    End Sub

    Private Sub cbo_LotNo_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_LotNo.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Dim f As New LotNo_creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_LotNo.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If
    End Sub

    Private Sub cbo_LotNo_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_LotNo.TextChanged
        Try
            If cbo_LotNo.Visible Then
                With dgv_MixingDetails
                    If .Rows.Count > 0 Then
                        If Val(cbo_LotNo.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = 2 Then
                            .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_LotNo.Text)
                        End If
                    End If
                End With
            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub cbo_Closed_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Closed.KeyDown

        Try
            vcbo_KeyDwnVal = e.KeyValue

            Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Closed, Nothing, Nothing, "", "", "", "")

            With dgv_MixingDetails

                If (e.KeyValue = 38 And cbo_Closed.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
                    .Focus()
                    .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex - 1)
                End If

                If (e.KeyValue = 40 And cbo_Closed.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
                    If .CurrentRow.Index <> .Rows.Count - 1 Then
                        .Focus()
                        .CurrentCell = .Rows(.CurrentCell.RowIndex + 1).Cells(1)
                    Else
                        txt_TareWgt.Focus()

                    End If

                End If

            End With

        Catch ex As Exception
            '-----

        End Try

    End Sub

    Private Sub cbo_Closed_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Closed.KeyPress

        Try
            Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Closed, Nothing, "", "", "", "")

            If Asc(e.KeyChar) = 13 Then

                With dgv_MixingDetails
                    If .Rows.Count > 0 Then
                        If .CurrentCell.ColumnIndex >= .ColumnCount - 3 Then
                            If .CurrentRow.Index <> .Rows.Count - 1 Then
                                .Focus()
                                .CurrentCell = .Rows(.CurrentCell.RowIndex + 1).Cells(1)
                            Else
                                If dgv_wasteDetails.RowCount > 0 Then

                                    dgv_wasteDetails.Focus()
                                    dgv_wasteDetails.CurrentCell = dgv_wasteDetails.Rows(0).Cells(1)
                                Else
                                    txt_TareWgt.Focus()
                                End If

                            End If

                        Else
                            .Focus()
                            .Rows(.CurrentCell.RowIndex).Cells.Item(6).Value = Trim(cbo_Closed.Text)
                            .CurrentCell = .Rows(.CurrentCell.RowIndex + 1).Cells(1)

                        End If
                    End If

                End With

            End If

        Catch ex As Exception
            '-----
        End Try

    End Sub

    Private Sub cbo_BorderSize_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Closed.TextChanged
        Try
            If cbo_Closed.Visible Then
                With dgv_MixingDetails
                    If Val(cbo_Closed.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = 6 Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_Closed.Text)
                    End If
                End With
            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub



    Private Sub btn_save_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_save.Click
        save_record()
    End Sub

    Private Sub btn_close_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_close.Click
        Me.Close()
    End Sub

    Private Sub btn_Filter_Show_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Filter_Show.Click
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim n As Integer, i As Integer
        Dim Cnt_IdNo As Integer, proc_IdNo As Integer
        Dim Condt As String = ""


        Try

            Condt = ""
            Cnt_IdNo = 0
            proc_IdNo = 0

            If IsDate(dtp_Filter_Fromdate.Value) = True And IsDate(dtp_Filter_ToDate.Value) = True Then
                Condt = "a.Mixing_Date between '" & Trim(Format(dtp_Filter_Fromdate.Value, "MM/dd/yyyy")) & "' and '" & Trim(Format(dtp_Filter_ToDate.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_Filter_Fromdate.Value) = True Then
                Condt = "a.Mixing_Date = '" & Trim(Format(dtp_Filter_Fromdate.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_Filter_ToDate.Value) = True Then
                Condt = "a.Mixing_Date = '" & Trim(Format(dtp_Filter_ToDate.Value, "MM/dd/yyyy")) & "' "
            End If

            If Trim(cbo_Filter_PartyName.Text) <> "" Then
                Cnt_IdNo = Common_Procedures.Count_NameToIdNo(con, cbo_Filter_PartyName.Text)
            End If



            If Val(Cnt_IdNo) <> 0 Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & "a.cOUNT_IdNo = " & Str(Val(Cnt_IdNo))
            End If






            da = New SqlClient.SqlDataAdapter("select a.*, b.Count_Name from Mixing_Head a INNER JOIN Count_Head b on a.Count_IdNo = b.Count_IdNo  where a.company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Mixing_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "' " & IIf(Trim(Condt) <> "", " and ", "") & Condt & " Order by a.for_orderby, a.Mixing_No", con)
            da.Fill(dt2)

            dgv_Filter_Details.Rows.Clear()

            If dt2.Rows.Count > 0 Then

                For i = 0 To dt2.Rows.Count - 1

                    n = dgv_Filter_Details.Rows.Add()


                    dgv_Filter_Details.Rows(n).Cells(0).Value = dt2.Rows(i).Item("Mixing_No").ToString
                    dgv_Filter_Details.Rows(n).Cells(1).Value = Format(Convert.ToDateTime(dt2.Rows(i).Item("Mixing_Date").ToString), "dd-MM-yyyy")
                    dgv_Filter_Details.Rows(n).Cells(2).Value = dt2.Rows(i).Item("Count_Name").ToString
                    dgv_Filter_Details.Rows(n).Cells(3).Value = Format(Val(dt2.Rows(i).Item("Tare_Weight").ToString), "###########0.000")
                    dgv_Filter_Details.Rows(n).Cells(4).Value = Format(Val(dt2.Rows(i).Item("Net_Weight").ToString), "########0.00")


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
    Private Sub cbo_Filter_PartyName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Filter_PartyName.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Count_Head", "Count_Name", "", "(Count_idno = 0)")
    End Sub

    Private Sub cbo_Filter_PartyName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Filter_PartyName.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_PartyName, dtp_Filter_ToDate, btn_Filter_Show, "Ledger_AlaisHead", "Ledger_DisplayName", " (Ledger_Type = '' and ( AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14) )", "(Ledger_idno = 0)")

    End Sub

    Private Sub cbo_Filter_PartyName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Filter_PartyName.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Filter_PartyName, btn_Filter_Show, "Ledger_AlaisHead", "Ledger_DisplayName", " (Ledger_Type = '' and ( AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14) )", "(Ledger_idno = 0)")
    End Sub



    Private Sub Open_FilterEntry()
        Dim movno As String

        On Error Resume Next

        movno = Trim(dgv_Filter_Details.CurrentRow.Cells(0).Value)

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


    Private Sub btn_Print_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Print.Click
        Common_Procedures.Print_OR_Preview_Status = 1
        print_record()
    End Sub

    Public Sub print_record() Implements Interface_MDIActions.print_record
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim NewCode As String
        'Dim ps As Printing.PaperSize
        Dim PpSzSTS As Boolean = False

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_MixNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)
        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.PrintEntry, Common_Procedures.UR.OEENTRY_MIXING_ENTRY, New_Entry) = False Then Exit Sub

        Try

            da1 = New SqlClient.SqlDataAdapter("select * from Mixing_Head Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Mixing_Code = '" & Trim(NewCode) & "'", con)
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

        Dim pkCustomSize1 As New System.Drawing.Printing.PaperSize("PAPER 8X6", 800, 600)
        PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = pkCustomSize1
        PrintDocument1.DefaultPageSettings.PaperSize = pkCustomSize1

        If Val(Common_Procedures.Print_OR_Preview_Status) = 1 Then

            Try

                PrintDocument1.Print()

                'PrintDialog1.PrinterSettings = PrintDocument1.PrinterSettings
                'If PrintDialog1.ShowDialog() = Windows.Forms.DialogResult.OK Then
                '    PrintDocument1.PrinterSettings = PrintDialog1.PrinterSettings
                '    PrintDocument1.Print()
                'End If

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
        Dim cmd As New SqlClient.SqlCommand
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim Dt1 As New DataTable
        Dim Dt2 As New DataTable
        Dim Dtbl1 As New DataTable
        Dim nr As Integer = 0
        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_MixNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        prn_HdDt.Clear()
        prn_DetDt.Clear()
        prn_DetIndx = 0
        prn_DetSNo = 0
        prn_PageNo = 0

        Try
            Dim sql As String = "select a.*, b.* from Mixing_Head a INNER JOIN Company_Head b ON a.Company_IdNo = b.Company_IdNo  where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Mixing_Code = '" & Trim(NewCode) & "'"
            da1 = New SqlClient.SqlDataAdapter(sql, con)
            prn_HdDt = New DataTable
            da1.Fill(prn_HdDt)

            If prn_HdDt.Rows.Count > 0 Then
                cmd.Connection = con
                'cmd.CommandText = "Truncate table " & Trim(Common_Procedures.EntryTempTable) & ""
                'cmd.ExecuteNonQuery()

                'cmd.CommandText = "Insert into " & Trim(Common_Procedures.EntryTempTable) & "( Name1   , Name2          ,Name3     ,Name4          ,Name5         ) " & _
                '                        "select  b.EndscOUNT_Name , c.Colour_name , a.Bobins , ''  , a.Meters from Mixing_Details a INNER JOIN EndscOUNT_Head b ON a.EndscOUNT_idno = b.endscOUNT_idno LEFT OUTER JOIN Colour_Head c ON a.Colour_idno = c.Colour_idno  where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Mixing_Code = '" & Trim(NewCode) & "' Order by a.Sl_No"
                'nr = cmd.ExecuteNonQuery()

                'cmd.CommandText = "Insert into " & Trim(Common_Procedures.EntryTempTable) & "( Name1   , Name2          ,Name3     ,Name4          ,Name5         ) " & _
                '                       "select  b.cOUNT_Name , c.Colour_name , a.Noof_Jumbos , a.Noof_Cones  , a.Weight  from Mixing_Waste_Details a INNER JOIN cOUNT_Head b ON a.cOUNT_idno = b.cOUNT_idno LEFT OUTER JOIN Colour_Head c ON a.Colour_idno = c.Colour_idno  where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Mixing_Code = '" & Trim(NewCode) & "' Order by a.Sl_No"
                'nr = cmd.ExecuteNonQuery()

                da2 = New SqlClient.SqlDataAdapter("select *  from Mixing_DETAILS a INNER JOIN Company_Head b ON a.Company_IdNo = b.Company_IdNo  where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Mixing_Code = '" & Trim(NewCode) & "'", con)
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

        'Printing_Delivery_Format1(e)

        Printing_Format1(e)


    End Sub

    Private Sub Printing_Delivery_Format1(ByRef e As System.Drawing.Printing.PrintPageEventArgs)
        Dim pFont As Font
        'Dim ps As Printing.PaperSize
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
        Dim SNo As Integer

        Dim pkCustomSize1 As New System.Drawing.Printing.PaperSize("PAPER 8X6", 800, 600)
        PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = pkCustomSize1
        PrintDocument1.DefaultPageSettings.PaperSize = pkCustomSize1

        With PrintDocument1.DefaultPageSettings.Margins
            .Left = 10 ' 30
            .Right = 40
            .Top = 10 ' 30
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

        NoofItems_PerPage = 7

        Erase LnAr
        Erase ClAr

        LnAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        ClAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}

        ClAr(1) = 35 : ClAr(2) = 240 : ClAr(3) = 135 : ClAr(4) = 100 : ClAr(5) = 100
        ClAr(6) = PageWidth - (LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5))

        TxtHgt = 17.75 ' 18

        EntryCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_MixNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Try
            If prn_HdDt.Rows.Count > 0 Then

                Printing_Delivery_Format1_PageHeader(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, prn_PageNo, NoofItems_PerPage, CurY, LnAr, ClAr)

                NoofDets = 0

                CurY = CurY - 10

                If prn_DetDt.Rows.Count > 0 Then

                    Do While prn_DetIndx <= prn_DetDt.Rows.Count - 1

                        If NoofDets >= NoofItems_PerPage Then

                            CurY = CurY + TxtHgt

                            Common_Procedures.Print_To_PrintDocument(e, "Continued...", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY, 1, 0, pFont)

                            NoofDets = NoofDets + 1

                            Printing_Delivery_Format1_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClAr, NoofDets, False)

                            NoofDets = 0
                            e.HasMorePages = True

                            Return

                        End If

                        ItmNm1 = Trim(prn_DetDt.Rows(prn_DetIndx).Item("Name1").ToString)
                        ItmNm2 = ""
                        If Len(ItmNm1) > 25 Then
                            For I = 15 To 1 Step -1
                                If Mid$(Trim(ItmNm1), I, 1) = " " Or Mid$(Trim(ItmNm1), I, 1) = "," Or Mid$(Trim(ItmNm1), I, 1) = "." Or Mid$(Trim(ItmNm1), I, 1) = "-" Or Mid$(Trim(ItmNm1), I, 1) = "/" Or Mid$(Trim(ItmNm1), I, 1) = "_" Or Mid$(Trim(ItmNm1), I, 1) = "(" Or Mid$(Trim(ItmNm1), I, 1) = ")" Or Mid$(Trim(ItmNm1), I, 1) = "\" Or Mid$(Trim(ItmNm1), I, 1) = "[" Or Mid$(Trim(ItmNm1), I, 1) = "]" Or Mid$(Trim(ItmNm1), I, 1) = "{" Or Mid$(Trim(ItmNm1), I, 1) = "}" Then Exit For
                            Next I
                            If I = 0 Then I = 25
                            ItmNm2 = Microsoft.VisualBasic.Right(Trim(ItmNm1), Len(ItmNm1) - I)
                            ItmNm1 = Microsoft.VisualBasic.Left(Trim(ItmNm1), I - 1)
                        End If

                        CurY = CurY + TxtHgt
                        SNo = SNo + 1
                        Common_Procedures.Print_To_PrintDocument(e, Trim(Val(SNo)), LMargin + 10, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm1), LMargin + ClAr(1) + 10, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Name2").ToString, LMargin + ClAr(1) + ClAr(2) + 10, CurY, 0, 0, pFont)
                        If Val(prn_DetDt.Rows(prn_DetIndx).Item("Name3").ToString) <> 0 Then
                            Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(prn_DetIndx).Item("Name3").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) - 10, CurY, 1, 0, pFont)
                        End If
                        If Val(prn_DetDt.Rows(prn_DetIndx).Item("Name4").ToString) <> 0 Then
                            Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(prn_DetIndx).Item("Name4").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 1, 0, pFont)
                        End If
                        If Val(prn_DetDt.Rows(prn_DetIndx).Item("Name5").ToString) <> 0 Then
                            Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Name5").ToString, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY, 1, 0, pFont)
                        End If

                        NoofDets = NoofDets + 1

                        If Trim(ItmNm2) <> "" Then
                            CurY = CurY + TxtHgt - 5
                            Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm2), LMargin + ClAr(1) + 10, CurY, 0, 0, pFont)
                            NoofDets = NoofDets + 1
                        End If

                        prn_DetIndx = prn_DetIndx + 1

                    Loop

                End If

                Printing_Delivery_Format1_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClAr, NoofDets, True)

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        e.HasMorePages = False

    End Sub

    Private Sub Printing_Delivery_Format1_PageHeader(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByRef PageNo As Integer, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single)
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt2 As New DataTable
        Dim p1Font As Font
        Dim Cmp_Name As String, Cmp_Add1 As String, Cmp_Add2 As String
        Dim Cmp_PhNo As String, Cmp_TinNo As String, Cmp_CstNo As String
        Dim strHeight As Single
        Dim C1 As Single
        Dim W1 As Single
        Dim S1, s2 As Single

        PageNo = PageNo + 1

        CurY = TMargin

        da2 = New SqlClient.SqlDataAdapter("select a.* from " & Trim(Common_Procedures.EntryTempTable) & " a ", con)
        da2.Fill(dt2)
        If dt2.Rows.Count > NoofItems_PerPage Then
            Common_Procedures.Print_To_PrintDocument(e, "Page : " & Trim(Val(PageNo)), PageWidth - 10, CurY - TxtHgt, 1, 0, pFont)
        End If
        dt2.Clear()

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

        CurY = CurY + TxtHgt - 10
        p1Font = New Font("Calibri", 18, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Name, LMargin, CurY, 2, PrintWidth, p1Font)
        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height


        CurY = CurY + strHeight - 1
        p1Font = New Font("Calibri", 9, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Add1 & " " & Cmp_Add2, LMargin, CurY, 2, PrintWidth, p1Font)
        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height

        'CurY = CurY + TxtHgt - 1
        'Common_Procedures.Print_To_PrintDocument(e, Cmp_Add2, LMargin, CurY, 2, PrintWidth, pFont)
        CurY = CurY + strHeight - 1
        Common_Procedures.Print_To_PrintDocument(e, Cmp_PhNo, LMargin, CurY, 2, PrintWidth, p1Font)
        CurY = CurY + strHeight - 1
        Common_Procedures.Print_To_PrintDocument(e, Cmp_TinNo, LMargin + 10, CurY, 0, 0, p1Font)
        Common_Procedures.Print_To_PrintDocument(e, Cmp_CstNo, PageWidth - 10, CurY, 1, 0, p1Font)

        CurY = CurY + TxtHgt - 5
        p1Font = New Font("Calibri", 16, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "BOBIN DELIVERY", LMargin, CurY, 2, PrintWidth, p1Font)
        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height

        CurY = CurY + strHeight
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(2) = CurY

        C1 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4)
        'C1 = ClAr(1) + ClAr(2) + ClAr(3)
        W1 = e.Graphics.MeasureString("ORDER NO : ", pFont).Width
        'w2 = e.Graphics.MeasureString("DESP.TO : ", pFont).Width
        'S1 = e.Graphics.MeasureString("TO  :  ", pFont).Width
        s2 = e.Graphics.MeasureString("TRANSPORT :  ", pFont).Width

        CurY = CurY + TxtHgt - 10
        p1Font = New Font("Calibri", 12, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "TO  :  " & "", LMargin + 10, CurY, 0, 0, p1Font)
        Common_Procedures.Print_To_PrintDocument(e, "DC.NO", LMargin + C1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Mixing_No").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, p1Font)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, " " & "", LMargin + S1 + 10, CurY, 0, 0, pFont)
        p1Font = New Font("Calibri", 14, FontStyle.Bold)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, " " & "", LMargin + S1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, "DATE", LMargin + C1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Mixing_Date").ToString), "dd-MM-yyyy").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, pFont)
        ''CurY = CurY + TxtHgt + 10
        ''If prn_HdDt.Rows(0).Item("Party_OrderNo").ToString <> "" Then
        ''    Common_Procedures.Print_To_PrintDocument(e, "ORDER NO", LMargin + C1 + 10, CurY, 0, 0, pFont)
        ''    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
        ''    Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Party_OrderNo").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, p1Font)
        ''End If

        CurY = CurY + TxtHgt + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(3) = CurY



        CurY = CurY + TxtHgt + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(3) = CurY

        e.Graphics.DrawLine(Pens.Black, LMargin + C1, CurY, LMargin + C1, LnAr(2))

        CurY = CurY + TxtHgt - 10
        Common_Procedures.Print_To_PrintDocument(e, "SNO", LMargin, CurY, 2, ClAr(1), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "VARIETY", LMargin + ClAr(1), CurY, 2, ClAr(2), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "LOTNO", LMargin + ClAr(1), CurY + TxtHgt, 2, ClAr(2), pFont)

        Common_Procedures.Print_To_PrintDocument(e, "BALE", LMargin + ClAr(1) + ClAr(2), CurY, 2, ClAr(3), pFont)

        Common_Procedures.Print_To_PrintDocument(e, "BALE NO", LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, 2, ClAr(4), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "WEIGHT", LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY + TxtHgt, 2, ClAr(4), pFont)

        CurY = CurY + TxtHgt + TxtHgt + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(4) = CurY

    End Sub

    Private Sub Printing_Delivery_Format1_PageFooter(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single, ByVal NoofDets As Integer, ByVal is_LastPage As Boolean)
        Dim i As Integer
        Dim Cmp_Name As String
        Dim p1Font As Font
        Dim W1 As Single = 0
        Dim C1 As Single = 0
        Dim s2 As Single = 0
        Dim vprn_BlNos As String = ""

        For i = NoofDets + 1 To NoofItems_PerPage
            CurY = CurY + TxtHgt
        Next

        C1 = ClAr(1) + ClAr(2) + ClAr(3)
        W1 = e.Graphics.MeasureString("TOTAL BOBIN : ", pFont).Width
        'w2 = e.Graphics.MeasureString("DESP.TO : ", pFont).Width
        'S1 = e.Graphics.MeasureString("TO  :  ", pFont).Width
        s2 = e.Graphics.MeasureString("TOTAL BOBIN :  ", pFont).Width

        CurY = CurY + TxtHgt
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(5) = CurY

        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1), CurY, LMargin + ClAr(1), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2), CurY, LMargin + ClAr(1) + ClAr(2), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(3))



        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, "TOTAL WEIGHT ", LMargin + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, " :  ", LMargin + s2 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Net_Weight").ToString), "#######0.000"), LMargin + s2 + 30, CurY, 0, 0, pFont)

        CurY = CurY + TxtHgt + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(6) = CurY

        'vprn_BlNos = ""
        'For i = 0 To prn_DetDt.Rows.Count - 1
        '    If Trim(prn_DetDt.Rows(i).Item("Bales_Nos").ToString) <> "" Then
        '        vprn_BlNos = Trim(vprn_BlNos) & IIf(Trim(vprn_BlNos) <> "", ", ", "") & prn_DetDt.Rows(i).Item("Bales_Nos").ToString
        '    End If
        'Next
        'Common_Procedures.Print_To_PrintDocument(e, "BALES NOS : " & vprn_BlNos, LMargin + 10, CurY, 0, 0, pFont)

        CurY = CurY + TxtHgt + 25

        Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString
        p1Font = New Font("Calibri", 12, FontStyle.Bold)


        Common_Procedures.Print_To_PrintDocument(e, "For " & Cmp_Name, PageWidth - 15, CurY, 1, 0, p1Font)

        CurY = CurY + TxtHgt + 10

        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

        e.Graphics.DrawLine(Pens.Black, LMargin, LnAr(1), LMargin, CurY)
        e.Graphics.DrawLine(Pens.Black, PageWidth, LnAr(1), PageWidth, CurY)

    End Sub

    Private Sub btn_Filter_Close_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Filter_Close.Click
        pnl_Back.Enabled = True
        pnl_Filter.Visible = False
        Filter_Status = False
    End Sub

    Private Sub txt_Remarks_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_Remarks.KeyDown
        If e.KeyValue = 40 Then
            If MessageBox.Show("Do you want to save ?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = DialogResult.Yes Then
                save_record()
            Else
                dtp_Date.Focus()
            End If
        End If

        If e.KeyValue = 38 Then
            txt_TareWgt.Focus()
        End If
    End Sub

    Private Sub txt_Remarks_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Remarks.KeyPress
        If Asc(e.KeyChar) = 13 Then
            If MessageBox.Show("Do you want to save ?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = DialogResult.Yes Then
                save_record()
            Else
                dtp_Date.Focus()
            End If
        End If
    End Sub


    Private Sub txt_TareWgt_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_TareWgt.KeyDown
        If e.KeyValue = 40 Then
            txt_Remarks.Focus()
        End If
        If e.KeyValue = 38 Then
            If dgv_wasteDetails.Rows.Count > 0 Then
                dgv_wasteDetails.Focus()
                dgv_wasteDetails.CurrentCell = dgv_wasteDetails.Rows(0).Cells(4)
                dgv_wasteDetails.CurrentCell.Selected = True



            End If
        End If

    End Sub

    Private Sub txt_TareWgt_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_TareWgt.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
        If Asc(e.KeyChar) = 13 Then

            txt_Remarks.Focus()

        End If
        '  Total_Calculation()
    End Sub

    Private Sub cbo_Colour_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Colour.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Colour_Head", "Colour_Name", "", "(Colour_IdNo = 0)")
    End Sub

    Private Sub cbo_colour_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Colour.KeyDown

        Try
            vcbo_KeyDwnVal = e.KeyValue
            Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Colour, Nothing, Nothing, "Colour_Head", "Colour_Name", "", "(Colour_IdNo = 0)")

            With dgv_wasteDetails

                If (e.KeyValue = 38 And cbo_Colour.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then

                    If Val(.CurrentCell.RowIndex) <= 0 Then
                        If dgv_MixingDetails.Rows.Count > 0 Then
                            dgv_MixingDetails.Focus()
                            dgv_MixingDetails.CurrentCell = dgv_MixingDetails.Rows(0).Cells(1)
                            dgv_MixingDetails.CurrentCell.Selected = True
                        Else
                            cbo_Count.Focus()
                        End If

                    Else
                        .Focus()
                        .CurrentCell = .Rows(.CurrentCell.RowIndex - 1).Cells(.ColumnCount - 1)
                        .CurrentCell.Selected = True

                    End If

                End If

                If (e.KeyValue = 40 And cbo_Colour.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then

                    If .CurrentCell.RowIndex = .Rows.Count - 1 And Trim(.Rows(.CurrentCell.RowIndex).Cells.Item(1).Value) = "" Then
                        txt_TareWgt.Focus()

                    Else
                        .Focus()
                        .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)

                    End If

                End If

            End With

        Catch ex As Exception
            '-----
        End Try

    End Sub

    Private Sub cbo_Colour_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Colour.KeyPress

        Try

            Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Colour, Nothing, "Colour_Head", "Color_Name", "", "(Colour_IdNo = 0)")

            If Asc(e.KeyChar) = 13 Then

                With dgv_wasteDetails
                    If .Rows.Count > 0 Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(1).Value = Trim(cbo_Colour.Text)
                        If .CurrentCell.RowIndex = .RowCount - 1 And .CurrentCell.ColumnIndex >= 1 And Trim(.CurrentRow.Cells(1).Value) = "" Then
                            txt_TareWgt.Focus()
                        Else
                            .Focus()
                            .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)
                        End If

                    End If

                End With

            End If

        Catch ex As Exception
            '------

        End Try

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

    Private Sub cbo_Colour_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Colour.TextChanged
        Try
            If cbo_Colour.Visible Then
                With dgv_wasteDetails
                    If Val(cbo_Colour.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = 1 Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_Colour.Text)
                    End If
                End With
            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub cbo_GridVariety_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_GridVariety_Waste.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Variety_Head", "Variety_Name", "(variety_type = 'WASTE')", "(Variety_IdNo = 0)")
        'Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Variety_Head", "Variety_Name", "(Variety_IdNo = 1)", "(Variety_IdNo = 0)")
    End Sub

    Private Sub cbo_GridVariety_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_GridVariety_Waste.KeyDown
        Dim dep_idno As Integer = 0

        Try
            vcbo_KeyDwnVal = e.KeyValue

            Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_GridVariety_Waste, Nothing, Nothing, "Variety_Head", "Variety_Name", "(variety_type = 'WASTE')", "(Variety_IdNo = 0)")
            With dgv_wasteDetails

                If (e.KeyValue = 38 And cbo_GridVariety_Waste.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then

                    If Val(.CurrentCell.RowIndex) <= 0 Then
                        If dgv_MixingDetails.Rows.Count > 0 Then
                            dgv_MixingDetails.Focus()
                            dgv_MixingDetails.CurrentCell = dgv_MixingDetails.Rows(0).Cells(1)
                            dgv_MixingDetails.CurrentCell.Selected = True

                        End If

                    Else
                        .Focus()
                        .CurrentCell = .Rows(.CurrentCell.RowIndex - 1).Cells(.ColumnCount - 1)
                        .CurrentCell.Selected = True

                    End If

                End If

                If (e.KeyValue = 40 And cbo_GridVariety_Waste.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then

                    If .CurrentCell.RowIndex = .Rows.Count - 1 And Trim(.Rows(.CurrentCell.RowIndex).Cells.Item(1).Value) = "" Then
                        txt_TareWgt.Focus()

                    Else
                        .Focus()
                        .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)

                    End If

                End If


            End With

        Catch ex As Exception
            '-----
        End Try

    End Sub

    Private Sub cbo_GridVariety_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_GridVariety_Waste.KeyPress
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable

        Try
            Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_GridVariety_Waste, Nothing, "Variety_Head", "Variety_Name", "(variety_type = 'WASTE')", "(Variety_IdNo = 0)")

            If Asc(e.KeyChar) = 13 Then
                With dgv_wasteDetails
                    If .Rows.Count > 0 Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(1).Value = Trim(cbo_GridVariety_Waste.Text)
                        If .CurrentCell.RowIndex = .RowCount - 1 And .CurrentCell.ColumnIndex >= 1 And Trim(.CurrentRow.Cells(1).Value) = "" Then
                            txt_TareWgt.Focus()
                        Else
                            .Focus()
                            .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)
                        End If

                    End If

                End With
            End If

        Catch ex As Exception
            '-----

        End Try

    End Sub

    Private Sub cbo_GridVariety_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_GridVariety_Waste.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Dim f As New Variety_Creation("WASTE")

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_GridVariety_Waste.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If
    End Sub

    Private Sub cbo_GridVariety_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_GridVariety_Waste.TextChanged

        Try
            If cbo_GridVariety_Waste.Visible Then
                With dgv_wasteDetails
                    If .Rows.Count > 0 Then
                        If Val(cbo_GridVariety_Waste.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = 1 Then
                            .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_GridVariety_Waste.Text)
                        End If
                    End If
                End With
            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub cbo_GridLotNO_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_GridLotNo.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Lot_Head", "Lot_No", "", "(Lot_IdNo = 0)")
    End Sub

    Private Sub cbo_GridLotNo_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_GridLotNo.KeyDown

        vcbo_KeyDwnVal = e.KeyValue

        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_GridLotNo, Nothing, Nothing, "Lot_Head", "Lot_No", "", "(Lot_IdNo = 0)")

        With dgv_wasteDetails

            If (e.KeyValue = 38 And cbo_GridLotNo.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
                .Focus()
                .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex - 1)
            End If

            If (e.KeyValue = 40 And cbo_GridLotNo.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
                .Focus()
                .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 1)
            End If

        End With

    End Sub

    Private Sub cbo_GridLotNo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_GridLotNo.KeyPress

        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_GridLotNo, Nothing, "Lot_Head", "Lot_No", "", "(Lot_IdNo = 0)")

        If Asc(e.KeyChar) = 13 Then

            With dgv_wasteDetails

                .Focus()
                .Rows(.CurrentCell.RowIndex).Cells.Item(2).Value = Trim(cbo_GridLotNo.Text)
                .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)

            End With

        End If

    End Sub

    Private Sub cbo_GridLotNo_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_GridLotNo.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Dim f As New LotNo_creation()

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_GridLotNo.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If
    End Sub

    Private Sub cbo_GridLotNo_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_GridLotNo.TextChanged

        Try
            If cbo_GridLotNo.Visible Then
                With dgv_wasteDetails
                    If .Rows.Count > 0 Then
                        If Val(cbo_GridLotNo.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = 2 Then
                            .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_GridLotNo.Text)
                        End If
                    End If
                End With
            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub dgv_WasteDetails_CellEndEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_wasteDetails.CellEndEdit
        Try
            With dgv_wasteDetails

                If .Rows.Count > 0 Then

                    If .CurrentCell.ColumnIndex = 5 Then
                        If Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value) <> 0 Then
                            .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = Format(Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value), "#########0.000")
                        End If
                    End If

                End If

            End With

        Catch ex As Exception
            '-----
        End Try


    End Sub

    Private Sub dgv_WasteDetails_CellEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_wasteDetails.CellEnter
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim Dt2 As New DataTable
        Dim Dt3 As New DataTable
        Dim Dt4 As New DataTable
        Dim rect As Rectangle

        Try


            With dgv_wasteDetails

                dgv_ActCtrlName = .Name

                If Val(.CurrentRow.Cells(0).Value) = 0 Then
                    .CurrentRow.Cells(0).Value = .CurrentRow.Index + 1
                End If

                'If e.ColumnIndex = 1 Then

                '    If cbo_Colour.Visible = False Or Val(cbo_Colour.Tag) <> e.RowIndex Then

                '        cbo_Colour.Tag = -1
                '        Da = New SqlClient.SqlDataAdapter("select Colour_Name from Colour_Head Order by Colour_Name", con)
                '        Dt2 = New DataTable
                '        Da.Fill(Dt2)
                '        cbo_Colour.DataSource = Dt2
                '        cbo_Colour.DisplayMember = "Colour_Name"

                '        Rect = .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False)

                '        cbo_Colour.Left = .Left + Rect.Left  '  .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Left
                '        cbo_Colour.Top = .Top + Rect.Top  ' .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Top
                '        cbo_Colour.Width = Rect.Width  ' .CurrentCell.Size.Width
                '        cbo_Colour.Height = Rect.Height  ' rect.Height

                '        cbo_Colour.Text = .CurrentCell.Value  '  Trim(.CurrentRow.Cells(e.ColumnIndex).Value)

                '        cbo_Colour.Tag = Val(e.RowIndex)
                '        cbo_Colour.Visible = True

                '        cbo_Colour.BringToFront()
                '        cbo_Colour.Focus()


                '    End If

                'Else

                '    cbo_Colour.Visible = False

                'End If

                If e.ColumnIndex = 1 Then

                    If cbo_GridVariety_Waste.Visible = False Or Val(cbo_GridVariety_Waste.Tag) <> e.RowIndex Then

                        cbo_GridVariety_Waste.Tag = -1
                        Da = New SqlClient.SqlDataAdapter("select variety_Name from variety_Head WHERE variety_type = 'WASTE' order by variety_Name", con)
                        Dt2 = New DataTable
                        Da.Fill(Dt2)
                        cbo_GridVariety_Waste.DataSource = Dt2
                        cbo_GridVariety_Waste.DisplayMember = "variety_Name"

                        rect = .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False)

                        cbo_GridVariety_Waste.Left = .Left + rect.Left  '  .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Left
                        cbo_GridVariety_Waste.Top = .Top + rect.Top  ' .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Top
                        cbo_GridVariety_Waste.Width = rect.Width  ' .CurrentCell.Size.Width
                        cbo_GridVariety_Waste.Height = rect.Height  ' rect.Height

                        cbo_GridVariety_Waste.Text = .CurrentCell.Value  '  Trim(.CurrentRow.Cells(e.ColumnIndex).Value)

                        cbo_GridVariety_Waste.Tag = Val(e.RowIndex)
                        cbo_GridVariety_Waste.Visible = True

                        cbo_GridVariety_Waste.BringToFront()
                        cbo_GridVariety_Waste.Focus()

                    End If

                Else


                    cbo_GridVariety_Waste.Visible = False

                End If

                If e.ColumnIndex = 2 Then

                    If cbo_GridLotNo.Visible = False Or Val(cbo_GridLotNo.Tag) <> e.RowIndex Then

                        cbo_GridLotNo.Tag = -1
                        Da = New SqlClient.SqlDataAdapter("select Lot_No from Lot_Head Order by Lot_No", con)
                        Dt2 = New DataTable
                        Da.Fill(Dt2)
                        cbo_GridLotNo.DataSource = Dt2
                        cbo_GridLotNo.DisplayMember = "Lot_No"

                        rect = .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False)

                        cbo_GridLotNo.Left = .Left + rect.Left  '  .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Left
                        cbo_GridLotNo.Top = .Top + rect.Top  ' .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Top
                        cbo_GridLotNo.Width = rect.Width  ' .CurrentCell.Size.Width
                        cbo_GridLotNo.Height = rect.Height  ' rect.Height

                        cbo_GridLotNo.Text = .CurrentCell.Value  '  Trim(.CurrentRow.Cells(e.ColumnIndex).Value)

                        cbo_GridLotNo.Tag = Val(e.RowIndex)
                        cbo_GridLotNo.Visible = True

                        cbo_GridLotNo.BringToFront()
                        cbo_GridLotNo.Focus()

                    End If

                Else

                    cbo_GridLotNo.Visible = False

                End If

            End With

        Catch ex As Exception
            '----

        End Try

    End Sub

    Private Sub dgv_WasteDetails_CellLeave(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_wasteDetails.CellLeave
        Try
            With dgv_wasteDetails
                If .CurrentCell.ColumnIndex = 5 Then
                    If Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value) <> 0 Then
                        .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = Format(Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value), "#########0.000")
                    End If
                End If
            End With

        Catch ex As Exception
            '------
        End Try
    End Sub

    Private Sub dgv_WasteDetails_CellValueChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_wasteDetails.CellValueChanged

        Try

            If IsNothing(dgv_wasteDetails.CurrentCell) Then Exit Sub

            With dgv_wasteDetails
                If .Visible Then
                    If .Rows.Count > 0 Then
                        If .CurrentCell.ColumnIndex = 3 Or .CurrentCell.ColumnIndex = 5 Then
                            Total_Calculation()
                        End If
                    End If
                End If
            End With

        Catch ex As Exception
            '-----

        End Try

    End Sub

    Private Sub dgv_WasteDetails_EditingControlShowing(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewEditingControlShowingEventArgs) Handles dgv_wasteDetails.EditingControlShowing
        Try
            dgtxt_WasteDetails = CType(dgv_wasteDetails.EditingControl, DataGridViewTextBoxEditingControl)
        Catch ex As Exception
            '-----
        End Try
    End Sub

    Private Sub dgtxt_KuriDetails_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgtxt_WasteDetails.Enter
        dgv_ActCtrlName = dgv_wasteDetails.Name
        dgv_wasteDetails.EditingControl.BackColor = Color.Lime
        dgv_wasteDetails.EditingControl.ForeColor = Color.Blue
        dgtxt_WasteDetails.SelectAll()
    End Sub

    Private Sub dgtxt_KuriDetails_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dgtxt_WasteDetails.KeyPress

        With dgv_wasteDetails

            If Val(dgv_wasteDetails.CurrentCell.ColumnIndex.ToString) = 3 Or Val(dgv_wasteDetails.CurrentCell.ColumnIndex.ToString) = 5 Then

                If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
                    e.Handled = True
                End If
            End If

        End With

    End Sub

    Private Sub dgv_WasteDetails_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_wasteDetails.KeyUp
        Dim n As Integer

        Try
            If e.Control = True And UCase(Chr(e.KeyCode)) = "D" Then

                With dgv_wasteDetails

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

        Catch ex As Exception
            '-----

        End Try


    End Sub

    Private Sub dgv_WasteDetails_RowsAdded(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewRowsAddedEventArgs) Handles dgv_wasteDetails.RowsAdded
        Dim n As Integer = 0

        Try
            With dgv_wasteDetails
                n = .RowCount
                .Rows(n - 1).Cells(0).Value = Val(n)
            End With

        Catch ex As Exception
            '-----
        End Try

    End Sub

    Private Sub dgv_WasteDetails_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgv_wasteDetails.LostFocus
        On Error Resume Next
        dgv_wasteDetails.CurrentCell.Selected = False
    End Sub

    Private Sub dgtxt_BobinDetails_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgtxt_MixingDetails.KeyUp
        dgv_MixingDetails_KeyUp(sender, e)
    End Sub

    Private Sub dgtxt_KuriDetails_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgtxt_WasteDetails.KeyUp
        dgv_WasteDetails_KeyUp(sender, e)
    End Sub

    Private Sub btn_Selection_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Selection.Click
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim Dt2 As New DataTable
        Dim i As Integer, j As Integer, n As Integer, SNo As Integer
        Dim NewCode As String
        Dim CompIDCondt As String
        Dim Ent_MixWgt As Single = 0

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_MixNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        CompIDCondt = "(a.company_idno = " & Str(Val(lbl_Company.Tag)) & ")"
        If Trim(UCase(Common_Procedures.settings.CompanyName)) = "-----~~~" Then
            CompIDCondt = ""
        End If


        With dgv_Selection

            .Rows.Clear()
            SNo = 0

            Da = New SqlClient.SqlDataAdapter("select a.*,C.Variety_Name,d.* from Cotton_Purchase_Bale_details a  INNER JOIN Variety_Head C ON C.Variety_IdNo = A.Variety_IdNo LEFT OUTER JOIN Mixing_Details d ON d.Cotton_purchase_Code = a.Cotton_purchase_Code and d.Bale_No = a.Bale_No  where a.Mixing_Code = '" & Trim(NewCode) & "'  order by a.Cotton_PURCHASE_Date, a.for_orderby, a.Cotton_Purchase_No", con)
            Dt1 = New DataTable
            Da.Fill(Dt1)

            If Dt1.Rows.Count > 0 Then

                For i = 0 To Dt1.Rows.Count - 1

                    n = .Rows.Add()


                    SNo = SNo + 1
                    .Rows(n).Cells(0).Value = Val(SNo)
                    .Rows(n).Cells(1).Value = Dt1.Rows(i).Item("Cotton_purchase_No").ToString
                    .Rows(n).Cells(2).Value = Dt1.Rows(i).Item("Bale_No").ToString
                    ' .Rows(n).Cells(3).Value = Dt1.Rows(i).Item("Bale_Nos").ToString
                    .Rows(n).Cells(3).Value = Dt1.Rows(i).Item("Variety_Name").ToString
                    .Rows(n).Cells(4).Value = Format(Val(Dt1.Rows(i).Item("Weight").ToString), "#########0.000")
                    .Rows(n).Cells(5).Value = "1"
                    .Rows(n).Cells(6).Value = Dt1.Rows(i).Item("Cotton_Purchase_Code").ToString
                    .Rows(n).Cells(7).Value = Dt1.Rows(i).Item("Detail_SlNo").ToString

                    For j = 0 To .ColumnCount - 1
                        .Rows(i).Cells(j).Style.ForeColor = Color.Red
                    Next

                Next

            End If
            Dt1.Clear()

            Da = New SqlClient.SqlDataAdapter("select a.* , c.Variety_Name from Cotton_Purchase_Bale_details a INNER JOIN Variety_Head c ON c.Variety_IdNo = A.Variety_IdNo where a.Mixing_Code  = '' order by a.Cotton_Purchase_Date, a.for_orderby, a.Cotton_Purchase_No", con)
            Dt1 = New DataTable
            Da.Fill(Dt1)

            If Dt1.Rows.Count > 0 Then

                For i = 0 To Dt1.Rows.Count - 1

                    n = .Rows.Add()

                    SNo = SNo + 1
                    '.Rows(n).Cells(0).Value = Val(SNo)


                    .Rows(n).Cells(0).Value = Val(SNo)
                    .Rows(n).Cells(1).Value = Dt1.Rows(i).Item("Cotton_purchase_No").ToString
                    .Rows(n).Cells(2).Value = Dt1.Rows(i).Item("Bale_No").ToString
                    ' .Rows(n).Cells(3).Value = Dt1.Rows(i).Item("Bale_Nos").ToString
                    .Rows(n).Cells(3).Value = Dt1.Rows(i).Item("Variety_Name").ToString
                    .Rows(n).Cells(4).Value = Format(Val(Dt1.Rows(i).Item("Weight").ToString), "#########0.000")
                    .Rows(n).Cells(5).Value = ""
                    .Rows(n).Cells(6).Value = Dt1.Rows(i).Item("Cotton_Purchase_Code").ToString
                    .Rows(n).Cells(7).Value = Dt1.Rows(i).Item("Detail_SlNo").ToString

                Next

            End If
            Dt1.Clear()

        End With


        pnl_Selection.Visible = True
        pnl_Back.Enabled = False
        If dgv_Selection.Enabled And dgv_Selection.Visible Then
            dgv_Selection.Focus()
            If dgv_Selection.Rows.Count > 0 Then
                dgv_Selection.CurrentCell = dgv_Selection.Rows(0).Cells(0)
                dgv_Selection.CurrentCell.Selected = True
            End If
        End If


    End Sub

    Private Sub dgv_Selection_CellClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Selection.CellClick
        Select_Piece(e.RowIndex)
    End Sub

    Private Sub Select_Piece(ByVal RwIndx As Integer)
        Dim i As Integer

        With dgv_Selection

            If .RowCount > 0 And RwIndx >= 0 Then

                .Rows(RwIndx).Cells(5).Value = (Val(.Rows(RwIndx).Cells(5).Value) + 1) Mod 2

                If Val(.Rows(RwIndx).Cells(5).Value) = 1 Then

                    For i = 0 To .ColumnCount - 1
                        .Rows(RwIndx).Cells(i).Style.ForeColor = Color.Red
                    Next

                Else
                    .Rows(RwIndx).Cells(5).Value = ""

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
        Cotton_Purchase_Selection()
    End Sub

    Private Sub Cotton_Purchase_Selection()
        Dim Da1 As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim n As Integer = 0
        Dim SNo As Integer = 0
        Dim i As Integer = 0
        Dim j As Integer = 0


        With dgv_MixingDetails

            dgv_MixingDetails.Rows.Clear()

            For i = 0 To dgv_Selection.RowCount - 1

                If Val(dgv_Selection.Rows(i).Cells(5).Value) = 1 Then
                    n = .Rows.Add()

                    SNo = SNo + 1
                    .Rows(n).Cells(0).Value = Val(SNo)
                    .Rows(n).Cells(1).Value = dgv_Selection.Rows(i).Cells(3).Value
                    .Rows(n).Cells(2).Value = dgv_Selection.Rows(i).Cells(1).Value
                    .Rows(n).Cells(3).Value = 1
                    .Rows(n).Cells(4).Value = dgv_Selection.Rows(i).Cells(2).Value
                    .Rows(n).Cells(5).Value = Format(Val(dgv_Selection.Rows(i).Cells(4).Value), "#########0.000")

                    .Rows(n).Cells(7).Value = dgv_Selection.Rows(i).Cells(6).Value
                    .Rows(n).Cells(8).Value = dgv_Selection.Rows(i).Cells(7).Value

                End If

            Next


        End With



        Total_Calculation()

        pnl_Back.Enabled = True
        pnl_Selection.Visible = False

        If dgv_MixingDetails.Rows.Count > 0 Then
            dgv_MixingDetails.Focus()
            dgv_MixingDetails.CurrentCell = dgv_MixingDetails.Rows(0).Cells(2)
        End If
    End Sub

    Private Sub btn_Set_Bm_selection_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Lot_Ble_selection.Click
        Dim LtNo As String
        Dim BleNo As String
        Dim i As Integer

        If Trim(txt_LotNoSelection.Text) <> "" Or Trim(txt_BaleNoSelection.Text) <> "" Then

            LtNo = Trim(txt_LotNoSelection.Text)
            BleNo = Trim(txt_BaleNoSelection.Text)

            For i = 0 To dgv_Selection.Rows.Count - 1
                If Trim(UCase(LtNo)) = Trim(UCase(dgv_Selection.Rows(i).Cells(1).Value)) And Trim(UCase(BleNo)) = Trim(UCase(dgv_Selection.Rows(i).Cells(2).Value)) Then
                    Call Select_Piece(i)

                    dgv_Selection.CurrentCell = dgv_Selection.Rows(i).Cells(0)
                    If i >= 8 Then dgv_Selection.FirstDisplayedScrollingRowIndex = i - 7

                    Exit For

                End If
            Next

            txt_LotNoSelection.Text = ""
            txt_BaleNoSelection.Text = ""
            If txt_LotNoSelection.Enabled = True Then txt_LotNoSelection.Focus()

        End If
    End Sub
    Private Sub txt_LotNoSelection_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_LotNoSelection.KeyDown
        If (e.KeyValue = 40) Then
            txt_BaleNoSelection.Focus()
        End If
    End Sub

    Private Sub txt_LotNoSelection_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_LotNoSelection.KeyPress
        If Asc(e.KeyChar) = 13 Then
            txt_BaleNoSelection.Focus()
        End If
    End Sub

    Private Sub txt_BaleNoSelection_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_BaleNoSelection.KeyDown
        If e.KeyValue = 40 Then
            If dgv_Selection.Rows.Count > 0 Then
                dgv_Selection.Focus()
                dgv_Selection.CurrentCell = dgv_Selection.Rows(0).Cells(0)
                dgv_Selection.CurrentCell.Selected = True
            End If
        End If
        If (e.KeyValue = 38) Then txt_LotNoSelection.Focus()
    End Sub

    Private Sub txt_BaleNoSelection_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_BaleNoSelection.KeyPress
        If Asc(e.KeyChar) = 13 Then

            If Trim(txt_BaleNoSelection.Text) <> "" Or Trim(txt_LotNoSelection.Text) <> "" Then
                btn_Set_Bm_selection_Click(sender, e)

            Else
                If dgv_Selection.Rows.Count > 0 Then
                    dgv_Selection.Focus()
                    dgv_Selection.CurrentCell = dgv_Selection.Rows(0).Cells(0)
                    dgv_Selection.CurrentCell.Selected = True
                End If

            End If

        End If
    End Sub
    'Private Sub btn_Waste_Selection_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Waste_selection.Click
    '    Dim Da As New SqlClient.SqlDataAdapter
    '    Dim Dt1 As New DataTable
    '    Dim Dt2 As New DataTable
    '    Dim i As Integer, j As Integer, n As Integer, SNo As Integer
    '    Dim NewCode As String
    '    Dim CompIDCondt As String
    '    Dim Ent_WsteWgt As Single = 0



    '    NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_MixNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

    '    CompIDCondt = "(a.company_idno = " & Str(Val(lbl_Company.Tag)) & ")"
    '    If Trim(UCase(Common_Procedures.settings.CompanyName)) = "-----~~~" Then
    '        CompIDCondt = ""
    '    End If



    '    With dgv_Waste_selection



    '        .Rows.Clear()
    '        SNo = 0

    '        Da = New SqlClient.SqlDataAdapter("select a.*, b.*, c.Variety_Name,d.Colour_Name , h.Weight as Ent_Wste_Weight from Cotton_Waste_Head a INNER JOIN Cotton_Waste_details b ON a.Cotton_Waste_Code = b.Cotton_Waste_Code  LEFT OUTER JOIN Variety_Head c ON a.Variety_IdNo = c.Variety_IdNo  LEFT OUTER JOIN Colour_Head d ON a.colour_IdNo = d.Colour_IdNo  LEFT OUTER JOIN Mixing_Waste_Details h ON h.Mixing_Code = '" & Trim(NewCode) & "' and b.Cotton_Waste_Code = h.Cotton_Waste_Code and b.Cotton_Waste_Details_Slno = h.Cotton_Waste_Details_Slno Where " & CompIDCondt & IIf(Trim(CompIDCondt) <> "", " and ", "") & "  ((b.Weight -  b.Waste_Weight) > 0 or h.Weight > 0 ) order by a.Cotton_Waste_Date, a.for_orderby, a.Cotton_Waste_No", con)
    '        Dt1 = New DataTable
    '        Da.Fill(Dt1)


    '        If Dt1.Rows.Count > 0 Then

    '            For i = 0 To Dt1.Rows.Count - 1

    '                n = .Rows.Add()



    '                Ent_WsteWgt = 0




    '                If IsDBNull(Dt1.Rows(i).Item("Ent_Wste_Weight").ToString) = False Then
    '                    Ent_WsteWgt = Val(Dt1.Rows(i).Item("Ent_Wste_Weight").ToString)
    '                End If

    '                SNo = SNo + 1
    '                .Rows(n).Cells(0).Value = Val(SNo)
    '                .Rows(n).Cells(1).Value = Dt1.Rows(i).Item("Colour_Name").ToString
    '                .Rows(n).Cells(2).Value = Dt1.Rows(i).Item("Variety_Name").ToString
    '                .Rows(n).Cells(3).Value = Dt1.Rows(i).Item("Lot_No").ToString
    '                .Rows(n).Cells(4).Value = Dt1.Rows(i).Item("Bale_No").ToString
    '                .Rows(n).Cells(5).Value = Format(Val(Dt1.Rows(i).Item("Weight").ToString) - Val(Dt1.Rows(i).Item("Waste_Weight").ToString) + Val(Ent_WsteWgt), "#########0.00")

    '                If Ent_WsteWgt > 0 Then
    '                    .Rows(n).Cells(6).Value = "1"
    '                    For j = 0 To .ColumnCount - 1
    '                        .Rows(n).Cells(j).Style.ForeColor = Color.Red
    '                    Next

    '                Else
    '                    .Rows(n).Cells(6).Value = ""

    '                End If

    '                .Rows(n).Cells(7).Value = Dt1.Rows(i).Item("Cotton_Waste_Code").ToString
    '                .Rows(n).Cells(8).Value = Dt1.Rows(i).Item("Cotton_Waste_Details_Slno").ToString
    '                .Rows(n).Cells(9).Value = Val(Ent_WsteWgt)



    '            Next
    '        End If
    '        Dt1.Clear()

    '    End With




    '    Pnl_Waste_Selection.Visible = True
    '    pnl_Back.Enabled = False
    '    If dgv_Waste_selection.Enabled And dgv_Waste_selection.Visible Then
    '        dgv_Waste_selection.Focus()
    '        If dgv_Waste_selection.Rows.Count > 0 Then
    '            dgv_Waste_selection.CurrentCell = dgv_Waste_selection.Rows(0).Cells(0)
    '            dgv_Waste_selection.CurrentCell.Selected = True
    '        End If
    '    End If


    'End Sub

    'Private Sub dgv_Waste_Selection_CellClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Waste_selection.CellClick
    '    Select_WastePiece(e.RowIndex)
    'End Sub

    'Private Sub Select_WastePiece(ByVal RwIndx As Integer)
    '    Dim i As Integer

    '    With dgv_Waste_selection

    '        If .RowCount > 0 And RwIndx >= 0 Then

    '            .Rows(RwIndx).Cells(6).Value = (Val(.Rows(RwIndx).Cells(6).Value) + 1) Mod 2

    '            If Val(.Rows(RwIndx).Cells(6).Value) = 1 Then

    '                For i = 0 To .ColumnCount - 1
    '                    .Rows(RwIndx).Cells(i).Style.ForeColor = Color.Red
    '                Next


    '            Else
    '                .Rows(RwIndx).Cells(6).Value = ""

    '                For i = 0 To .ColumnCount - 1
    '                    .Rows(RwIndx).Cells(i).Style.ForeColor = Color.Black
    '                Next

    '            End If

    '        End If

    '    End With

    'End Sub

    'Private Sub dgv_Waste_Selection_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_Waste_selection.KeyDown
    '    Dim n As Integer

    '    On Error Resume Next

    '    If e.KeyCode = Keys.Enter Or e.KeyCode = Keys.Space Then
    '        If dgv_Waste_selection.CurrentCell.RowIndex >= 0 Then

    '            n = dgv_Waste_selection.CurrentCell.RowIndex

    '            Select_WastePiece(n)

    '            e.Handled = True

    '        End If
    '    End If
    'End Sub

    'Private Sub btn_Waste_Close_Selection_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Waste_Close_Selection.Click
    '    Waste_Selection()
    'End Sub

    'Private Sub Waste_Selection()
    '    Dim Da1 As New SqlClient.SqlDataAdapter
    '    Dim Dt1 As New DataTable
    '    Dim n As Integer = 0
    '    Dim SNo As Integer = 0
    '    Dim i As Integer = 0
    '    Dim j As Integer = 0


    '    With dgv_wasteDetails

    '        dgv_wasteDetails.Rows.Clear()

    '        For i = 0 To dgv_Waste_selection.RowCount - 1

    '            If Val(dgv_Waste_selection.Rows(i).Cells(6).Value) = 1 Then
    '                n = .Rows.Add()

    '                SNo = SNo + 1
    '                .Rows(n).Cells(0).Value = Val(SNo)
    '                .Rows(n).Cells(1).Value = dgv_Waste_selection.Rows(i).Cells(1).Value
    '                .Rows(n).Cells(2).Value = dgv_Waste_selection.Rows(i).Cells(2).Value
    '                .Rows(n).Cells(3).Value = dgv_Waste_selection.Rows(i).Cells(3).Value
    '                .Rows(n).Cells(4).Value = dgv_Waste_selection.Rows(i).Cells(4).Value


    '                .Rows(n).Cells(6).Value = dgv_Waste_selection.Rows(i).Cells(7).Value
    '                .Rows(n).Cells(7).Value = dgv_Waste_selection.Rows(i).Cells(8).Value

    '                If Val(dgv_Waste_selection.Rows(i).Cells(9).Value) <> 0 Then
    '                    .Rows(n).Cells(5).Value = dgv_Waste_selection.Rows(i).Cells(9).Value
    '                Else
    '                    .Rows(n).Cells(5).Value = dgv_Waste_selection.Rows(i).Cells(5).Value
    '                End If


    '            End If



    '        Next


    '    End With



    '    Total_Calculation()

    '    pnl_Back.Enabled = True
    '    Pnl_Waste_Selection.Visible = False

    '    If dgv_wasteDetails.Rows.Count > 0 Then
    '        dgv_wasteDetails.Focus()
    '        dgv_wasteDetails.CurrentCell = dgv_wasteDetails.Rows(0).Cells(5)
    '    End If
    'End Sub

    'Private Sub btn_Waste_Lot_Bale_selection_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Waste_Lot_bale_Selection.Click
    '    Dim LtNo As String
    '    Dim BleNo As String
    '    Dim i As Integer

    '    If Trim(txt_Waste_LotNoSelection.Text) <> "" Or Trim(txt_Waste_BaleNoSelection.Text) <> "" Then

    '        LtNo = Trim(txt_Waste_LotNoSelection.Text)
    '        BleNo = Trim(txt_Waste_BaleNoSelection.Text)

    '        For i = 0 To dgv_Waste_selection.Rows.Count - 1
    '            If Trim(UCase(LtNo)) = Trim(UCase(dgv_Waste_selection.Rows(i).Cells(3).Value)) And Trim(UCase(BleNo)) = Trim(UCase(dgv_Waste_selection.Rows(i).Cells(4).Value)) Then
    '                Call Select_WastePiece(i)

    '                dgv_Waste_selection.CurrentCell = dgv_Waste_selection.Rows(i).Cells(0)
    '                If i >= 9 Then dgv_Waste_selection.FirstDisplayedScrollingRowIndex = i - 8

    '                Exit For

    '            End If
    '        Next

    '        txt_Waste_LotNoSelection.Text = ""
    '        txt_Waste_BaleNoSelection.Text = ""
    '        If txt_Waste_LotNoSelection.Enabled = True Then txt_Waste_LotNoSelection.Focus()

    '    End If
    'End Sub
    Private Sub txt_Waste_LotNoSelection_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_Waste_LotNoSelection.KeyDown
        If (e.KeyValue = 40) Then
            txt_Waste_BaleNoSelection.Focus()
        End If
    End Sub

    Private Sub txt_Waste_LotNoSelection_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Waste_LotNoSelection.KeyPress
        If Asc(e.KeyChar) = 13 Then
            txt_Waste_BaleNoSelection.Focus()
        End If
    End Sub

    'Private Sub txt_Waste_BaleNoSelection_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_Waste_BaleNoSelection.KeyDown
    '    If e.KeyValue = 40 Then
    '        If dgv_Waste_selection.Rows.Count > 0 Then
    '            dgv_Waste_selection.Focus()
    '            dgv_Waste_selection.CurrentCell = dgv_Waste_selection.Rows(0).Cells(0)
    '            dgv_Waste_selection.CurrentCell.Selected = True
    '        End If
    '    End If
    '    If (e.KeyValue = 38) Then txt_Waste_LotNoSelection.Focus()
    'End Sub

    'Private Sub txt_Waste_BaleNoSelection_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Waste_BaleNoSelection.KeyPress
    '    If Asc(e.KeyChar) = 13 Then

    '        If Trim(txt_Waste_BaleNoSelection.Text) <> "" Or Trim(txt_Waste_LotNoSelection.Text) <> "" Then
    '            btn_Waste_Lot_Bale_selection_Click(sender, e)

    '        Else
    '            If dgv_Waste_selection.Rows.Count > 0 Then
    '                dgv_Waste_selection.Focus()
    '                dgv_Waste_selection.CurrentCell = dgv_Waste_selection.Rows(0).Cells(0)
    '                dgv_Waste_selection.CurrentCell.Selected = True
    '            End If

    '        End If

    '    End If
    'End Sub
    Private Sub dtp_Date_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dtp_Date.LostFocus
        txt_Time.Text = Format(Now, "Short Time")
    End Sub

    Private Sub txt_TareWgt_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_TareWgt.TextChanged
        Total_Calculation()
    End Sub



    Private Sub dgv_MixingDetails_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_MixingDetails.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
    End Sub




    Private Sub Printing_Format1(ByRef e As System.Drawing.Printing.PrintPageEventArgs)
        Dim cmd As New SqlClient.SqlCommand
        Dim Da1 As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim EntryCode As String
        Dim I As Integer, NoofDets As Integer, NoofItems_PerPage As Integer
        Dim pFont As Font
        Dim LMargin As Single, RMargin As Single, TMargin As Single, BMargin As Single
        Dim PrintWidth As Single, PrintHeight As Single
        Dim PageWidth As Single, PageHeight As Single
        Dim CurY As Single, TxtHgt As Single
        Dim LnAr(15) As Single, ClArr(15) As Single
        Dim ItmNm1 As String, ItmNm2 As String
        Dim ps As Printing.PaperSize
        Dim strHeight As Single = 0
        Dim PpSzSTS As Boolean = False
        Dim W1 As Single = 0
        Dim SNo As Integer

        For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
            If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
                ps = PrintDocument1.PrinterSettings.PaperSizes(I)
                PrintDocument1.DefaultPageSettings.PaperSize = ps
                e.PageSettings.PaperSize = ps
                Exit For
            End If
        Next

        With PrintDocument1.DefaultPageSettings.Margins
            .Left = 40
            .Right = 45
            .Top = 45
            .Bottom = 45
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

        NoofItems_PerPage = 10 '11 ' 6

        Erase LnAr
        Erase ClArr

        LnAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        ClArr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}

        ClArr(1) = Val(50) 'SNO
        ClArr(2) = 170      'PARTICULARS
        ClArr(3) = 70       'LOT NO
        ClArr(4) = 65       'BALE
        ClArr(5) = 70       'BALE NO
        ClArr(6) = 90       'WEIGHT
        ClArr(8) = PageWidth - (LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5)) 'STATUS

        TxtHgt = 18.5 ' 19 ' e.Graphics.MeasureString("A", pFont).Height  ' 20

        EntryCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_MixNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Try

            If prn_HdDt.Rows.Count > 0 Then

                Printing_Format1_PageHeader(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, prn_PageNo, NoofItems_PerPage, CurY, LnAr, ClArr)


                ' W1 = e.Graphics.MeasureString("No.of Beams  : ", pFont).Width

                NoofDets = 0

                CurY = CurY - 10

                'CurY = CurY + TxtHgt
                'Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Mill_Name").ToString, LMargin + ClArr(1) + ClArr(2) + 10, CurY, 0, 0, pFont)

                If prn_DetDt.Rows.Count > 0 Then

                    Do While prn_DetIndx <= prn_DetDt.Rows.Count - 1

                        If NoofDets >= NoofItems_PerPage Then
                            CurY = CurY + TxtHgt

                            Common_Procedures.Print_To_PrintDocument(e, "Continued....", PageWidth - 10, CurY, 1, 0, pFont)

                            NoofDets = NoofDets + 1

                            Printing_Format1_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, NoofDets, False)

                            e.HasMorePages = True
                            Return

                        End If

                        prn_DetSNo = prn_DetSNo + 1

                        If Trim(prn_HdDt.Rows(0).Item("Description").ToString) <> "" Then
                            ItmNm1 = Trim(prn_HdDt.Rows(0).Item("Description").ToString)

                        Else
                            ItmNm1 = Trim(prn_DetDt.Rows(prn_DetIndx).Item("Variety_Name").ToString)

                        End If


                        ItmNm2 = ""
                        If Len(ItmNm1) > 18 Then
                            For I = 18 To 1 Step -1
                                If Mid$(Trim(ItmNm1), I, 1) = " " Or Mid$(Trim(ItmNm1), I, 1) = "," Or Mid$(Trim(ItmNm1), I, 1) = "." Or Mid$(Trim(ItmNm1), I, 1) = "-" Or Mid$(Trim(ItmNm1), I, 1) = "/" Or Mid$(Trim(ItmNm1), I, 1) = "_" Or Mid$(Trim(ItmNm1), I, 1) = "(" Or Mid$(Trim(ItmNm1), I, 1) = ")" Or Mid$(Trim(ItmNm1), I, 1) = "\" Or Mid$(Trim(ItmNm1), I, 1) = "[" Or Mid$(Trim(ItmNm1), I, 1) = "]" Or Mid$(Trim(ItmNm1), I, 1) = "{" Or Mid$(Trim(ItmNm1), I, 1) = "}" Then Exit For
                            Next I
                            If I = 0 Then I = 18
                            ItmNm2 = Microsoft.VisualBasic.Right(Trim(ItmNm1), Len(ItmNm1) - I)
                            ItmNm1 = Microsoft.VisualBasic.Left(Trim(ItmNm1), I - 1)
                        End If

                        CurY = CurY + TxtHgt

                        SNo = SNo + 1
                        Common_Procedures.Print_To_PrintDocument(e, Trim(Val(SNo)), LMargin + 10, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm1), LMargin + ClArr(1) + 10, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Lot_No").ToString, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) - 10, CurY, 1, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Bale").ToString), "########0.00"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) - 10, CurY, 1, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Bale_No").ToString, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + 10, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Mixing_Weight").ToString, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) - 10, CurY, 1, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Bale_Closed").ToString) * Val(prn_HdDt.Rows(0).Item("Rate").ToString), "#########0.00"), PageWidth - 10, CurY, 1, 0, pFont)

                        NoofDets = NoofDets + 1

                        If Trim(ItmNm2) <> "" Then
                            CurY = CurY + TxtHgt - 5
                            Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm2), LMargin + ClArr(1) + ClArr(2) + 10, CurY, 0, 0, pFont)
                            NoofDets = NoofDets + 1
                        End If




                        prn_DetIndx = prn_DetIndx + 1

                    Loop

                End If


                Printing_Format1_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, NoofDets, True)

                'If Trim(prn_InpOpts) <> "" Then
                '    If prn_Count < Len(Trim(prn_InpOpts)) Then


                '        If Val(prn_InpOpts) <> "0" Then
                '            prn_DetIndx = 0
                '            prn_DetSNo = 0
                '            prn_PageNo = 0

                '            e.HasMorePages = True
                '            Return
                '        End If

                '    End If
                'End If



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
        Dim strHeight As Single
        Dim C1 As Single, C2 As Single, W1 As Single, W2 As Single, S1 As Single, W3 As Single
        Dim Cmp_Name As String, Cmp_Add1 As String, Cmp_Add2 As String, Cmp_EMail As String
        Dim Cmp_PhNo As String, Cmp_TinNo As String, Cmp_CstNo As String, Cmp_panno As String
        Dim Cmp_StateNm As String = "", Cmp_StateCode As String = "", Cmp_GSTIN_No As String = ""
        Dim S As String
        Dim ItmNm1 As String, ItmNm2 As String
        Dim i As Integer = 0
        Dim Inv_No As String = ""
        Dim InvSubNo As String = ""


        PageNo = PageNo + 1



        CurY = TMargin



        da2 = New SqlClient.SqlDataAdapter("select a.*, b.Ledger_Name from Cotton_Waste_Sales_Head a INNER JOIN Ledger_Head b ON b.Ledger_IdNo = a.Ledger_Idno  where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Cotton_Waste_Sales_Code = '" & Trim(EntryCode) & "' Order by a.For_OrderBy", con)
        da2.Fill(dt2)
        If dt2.Rows.Count > NoofItems_PerPage Then
            Common_Procedures.Print_To_PrintDocument(e, "Page : " & Trim(Val(PageNo)), PageWidth - 10, CurY - TxtHgt, 1, 0, pFont)
        End If
        dt2.Clear()

        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(1) = CurY

        Cmp_Name = "" : Cmp_Add1 = "" : Cmp_Add2 = "" : Cmp_panno = ""
        Cmp_PhNo = "" : Cmp_TinNo = "" : Cmp_CstNo = "" : Cmp_EMail = ""

        Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString
        Cmp_Add1 = prn_HdDt.Rows(0).Item("Company_Address1").ToString & " " & prn_HdDt.Rows(0).Item("Company_Address2").ToString
        Cmp_Add2 = prn_HdDt.Rows(0).Item("Company_Address3").ToString & " " & prn_HdDt.Rows(0).Item("Company_Address4").ToString

        If Trim(prn_HdDt.Rows(0).Item("Company_PhoneNo").ToString) <> "" Then
            Cmp_PhNo = "PHONE NO.:" & prn_HdDt.Rows(0).Item("Company_PhoneNo").ToString
        End If

        If Trim(prn_HdDt.Rows(0).Item("Company_EMail").ToString) <> "" Then
            Cmp_EMail = "MAIL ID : " & prn_HdDt.Rows(0).Item("Company_EMail").ToString
        End If

        If Trim(prn_HdDt.Rows(0).Item("Company_PanNo").ToString) <> "" Then
            Cmp_panno = "PAN NO.: " & prn_HdDt.Rows(0).Item("Company_PanNo").ToString
        End If

        'If Trim(prn_HdDt.Rows(0).Item("State_Name").ToString) <> "" Then
        '    Cmp_StateNm = "STATE :" & prn_HdDt.Rows(0).Item("State_Name").ToString
        'End If

        'If Trim(prn_HdDt.Rows(0).Item("State_Code").ToString) <> "" Then
        '    Cmp_StateCode = "CODE : " & prn_HdDt.Rows(0).Item("State_Code").ToString
        'End If
        'If Trim(prn_HdDt.Rows(0).Item("Company_GSTinNo").ToString) <> "" Then
        '    Cmp_GSTIN_No = "GSTIN : " & prn_HdDt.Rows(0).Item("Company_GSTinNo").ToString
        'End If
        'e.Graphics.DrawImage(DirectCast(Global.OESpinning.My.Resources.Resources.KmtOe, Drawing.Image), LMargin + 15, CurY + 5, 120, 100)

        CurY = CurY + TxtHgt - 10
        p1Font = New Font("Calibri", 18, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Name, LMargin, CurY, 2, PrintWidth, p1Font)
        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height

        CurY = CurY + strHeight
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Company_Description").ToString, LMargin, CurY, 2, PrintWidth, pFont)

        CurY = CurY + +TxtHgt - 1
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Add1, LMargin, CurY, 2, PrintWidth, pFont)

        CurY = CurY + TxtHgt - 1
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Add2, LMargin, CurY, 2, PrintWidth, pFont)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, Cmp_StateNm & "    " & Cmp_StateCode, LMargin, CurY, 2, PrintWidth, pFont)
        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, Cmp_PhNo, LMargin, CurY, 2, PrintWidth, pFont)
        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, Cmp_panno, LMargin + 5, CurY, 0, PrintWidth, pFont)
        Common_Procedures.Print_To_PrintDocument(e, Cmp_EMail, LMargin, CurY, 2, PrintWidth, pFont)
        Common_Procedures.Print_To_PrintDocument(e, Cmp_GSTIN_No, PageWidth - 10, CurY, 1, 0, pFont)


        'CurY = CurY + TxtHgt
        'p1Font = New Font("Calibri", 16, FontStyle.Bold)
        'Common_Procedures.Print_To_PrintDocument(e, "TAX INVOICE", LMargin, CurY, 2, PrintWidth, p1Font)
        'strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height

        CurY = CurY + strHeight
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(2) = CurY

        Try
            C1 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5)
            C2 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) - 50
            W3 = e.Graphics.MeasureString("STATE CODE  ", pFont).Width

            W1 = e.Graphics.MeasureString("BILL  NO   : ", pFont).Width
            S1 = e.Graphics.MeasureString("TO         :    ", pFont).Width
            W2 = e.Graphics.MeasureString("BILL        NO      : ", pFont).Width

            'CurY = CurY + TxtHgt
            'p1Font = New Font("Calibri", 12, FontStyle.Bold)
            'Common_Procedures.Print_To_PrintDocument(e, "FROM :  " & "M/s." & prn_HdDt.Rows(0).Item("Ledger_Name").ToString, LMargin + 10, CurY, 0, 0, p1Font)
            'Common_Procedures.Print_To_PrintDocument(e, "BILL No", LMargin + C1 + 10, CurY, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
            'If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1088" Then '---- Kalaimagal Textiles (OE) (palladam)
            '    Inv_No = prn_HdDt.Rows(0).Item("Cotton_Waste_Sales_No").ToString
            '    InvSubNo = Replace(Trim(Inv_No), Trim(Val(Inv_No)), "")

            '    If prn_HdDt.Rows(0).Item("Invoice_PrefixNo").ToString <> "" Then
            '        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Invoice_PrefixNo").ToString & "-" & Trim(Format(Val(Inv_No), "######0000")) & Trim(InvSubNo), LMargin + C1 + W1 + 30, CurY, 0, 0, p1Font)
            '    Else
            '        Common_Procedures.Print_To_PrintDocument(e, Trim(Format(Val(Inv_No), "######0000")) & Trim(InvSubNo), LMargin + C1 + W1 + 30, CurY, 0, 0, p1Font)
            '    End If

            'Else
            '    If prn_HdDt.Rows(0).Item("Invoice_PrefixNo").ToString <> "" Then
            '        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Invoice_PrefixNo").ToString & "-" & prn_HdDt.Rows(0).Item("Cotton_Waste_Sales_No").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, p1Font)
            '    Else
            '        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Cotton_Waste_Sales_No").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, p1Font)
            '    End If
            'End If

            'CurY = CurY + TxtHgt
            'Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address1").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
            'p1Font = New Font("Calibri", 14, FontStyle.Bold)

            'CurY = CurY + TxtHgt
            'Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address2").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, "BILL Date", LMargin + C1 + 10, CurY, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Cotton_Waste_Sales_Date").ToString), "dd-MM-yyyy").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, pFont)

            'CurY = CurY + TxtHgt
            'Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address3").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)

            'CurY = CurY + TxtHgt
            'Common_Procedures.Print_To_PrintDocument(e, "PAN NO", LMargin + 10, CurY, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, " : " & prn_HdDt.Rows(0).Item("PAN_NO").ToString, LMargin + W3 + 10, CurY, 0, 0, pFont)

            'CurY = CurY + TxtHgt
            'Common_Procedures.Print_To_PrintDocument(e, "GSTIN  ", LMargin + 10, CurY, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, " : " & prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString, LMargin + W3 + 10, CurY, 0, 0, pFont)

            'CurY = CurY + TxtHgt
            'Common_Procedures.Print_To_PrintDocument(e, "STATE NAME ", LMargin + 10, CurY, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, " : " & prn_HdDt.Rows(0).Item("Ledger_State_Name").ToString, LMargin + W3 + 10, CurY, 0, 0, pFont)

            'CurY = CurY + TxtHgt
            'Common_Procedures.Print_To_PrintDocument(e, "STATE CODE  ", LMargin + 10, CurY, 0, 0, pFont)

            'Common_Procedures.Print_To_PrintDocument(e, " : " & prn_HdDt.Rows(0).Item("Ledger_State_Code").ToString, LMargin + W3 + 10, CurY, 0, 0, pFont)


            'CurY = CurY + TxtHgt + 10
            'e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            'LnAr(3) = CurY

            'e.Graphics.DrawLine(Pens.Black, LMargin + C1, LnAr(3), LMargin + C1, LnAr(2))
            'CurY = CurY + TxtHgt - 10

            'Common_Procedures.Print_To_PrintDocument(e, "Delivery Address", LMargin + 10, CurY, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W2 + 10, CurY, 0, 0, pFont)


            'CurY = CurY + TxtHgt
            'ItmNm1 = Trim(prn_HdDt.Rows(0).Item("Delivery_Address").ToString)
            'ItmNm2 = ""
            'If Len(ItmNm1) > 20 Then
            '    For i = 20 To 1 Step -1
            '        If Mid$(Trim(ItmNm1), i, 1) = " " Or Mid$(Trim(ItmNm1), i, 1) = "," Or Mid$(Trim(ItmNm1), i, 1) = "." Or Mid$(Trim(ItmNm1), i, 1) = "-" Or Mid$(Trim(ItmNm1), i, 1) = "/" Or Mid$(Trim(ItmNm1), i, 1) = "_" Or Mid$(Trim(ItmNm1), i, 1) = "(" Or Mid$(Trim(ItmNm1), i, 1) = ")" Or Mid$(Trim(ItmNm1), i, 1) = "\" Or Mid$(Trim(ItmNm1), i, 1) = "[" Or Mid$(Trim(ItmNm1), i, 1) = "]" Or Mid$(Trim(ItmNm1), i, 1) = "{" Or Mid$(Trim(ItmNm1), i, 1) = "}" Then Exit For
            '    Next i
            '    If i = 0 Then i = 20
            '    ItmNm2 = Microsoft.VisualBasic.Right(Trim(ItmNm1), Len(ItmNm1) - i)
            '    ItmNm1 = Microsoft.VisualBasic.Left(Trim(ItmNm1), i - 1)
            'End If
            'Common_Procedures.Print_To_PrintDocument(e, ItmNm1, LMargin + W2 + 30, CurY, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, "Vehicle No : " & prn_HdDt.Rows(0).Item("Vechile_No").ToString, LMargin + C1 + 25, CurY, 0, 0, pFont)

            'If ItmNm2 <> "" Then
            '    CurY = CurY + TxtHgt
            '    Common_Procedures.Print_To_PrintDocument(e, ItmNm2, LMargin + W2 + 30, CurY, 0, 0, pFont)
            'End If

            'CurY = CurY + TxtHgt
            'ItmNm1 = ""
            'ItmNm1 = Trim(prn_HdDt.Rows(0).Item("Delivery_Address1").ToString)
            'ItmNm2 = ""
            'If Len(ItmNm1) > 20 Then
            '    For i = 20 To 1 Step -1
            '        If Mid$(Trim(ItmNm1), i, 1) = " " Or Mid$(Trim(ItmNm1), i, 1) = "," Or Mid$(Trim(ItmNm1), i, 1) = "." Or Mid$(Trim(ItmNm1), i, 1) = "-" Or Mid$(Trim(ItmNm1), i, 1) = "/" Or Mid$(Trim(ItmNm1), i, 1) = "_" Or Mid$(Trim(ItmNm1), i, 1) = "(" Or Mid$(Trim(ItmNm1), i, 1) = ")" Or Mid$(Trim(ItmNm1), i, 1) = "\" Or Mid$(Trim(ItmNm1), i, 1) = "[" Or Mid$(Trim(ItmNm1), i, 1) = "]" Or Mid$(Trim(ItmNm1), i, 1) = "{" Or Mid$(Trim(ItmNm1), i, 1) = "}" Then Exit For
            '    Next i
            '    If i = 0 Then i = 20
            '    ItmNm2 = Microsoft.VisualBasic.Right(Trim(ItmNm1), Len(ItmNm1) - i)
            '    ItmNm1 = Microsoft.VisualBasic.Left(Trim(ItmNm1), i - 1)
            'End If
            'Common_Procedures.Print_To_PrintDocument(e, ItmNm1, LMargin + W2 + 30, CurY, 0, 0, pFont)
            'If ItmNm2 <> "" Then
            '    CurY = CurY + TxtHgt
            '    Common_Procedures.Print_To_PrintDocument(e, ItmNm2, LMargin + W2 + 30, CurY, 0, 0, pFont)
            'End If





            CurY = CurY + TxtHgt + 10
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(4) = CurY

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "NO", LMargin, CurY, 2, ClAr(1), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "PARTICULARS", LMargin + ClAr(1), CurY, 2, ClAr(2), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "LOT NO", LMargin + ClAr(1) + ClAr(2), CurY, 2, ClAr(3), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "BALE", LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, 2, ClAr(4), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "BALE NO", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 2, ClAr(5), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "WEIGHT", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, 2, ClAr(6), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "STATUS", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, 2, ClAr(8), pFont)

            CurY = CurY + TxtHgt + 10
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(5) = CurY

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub Printing_Format1_PageFooter(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single, ByVal NoofDets As Integer, ByVal is_LastPage As Boolean)
        Dim p1Font As Font
        Dim I As Integer
        Dim Cmp_Name As String
        Dim C1 As Single, W1 As Single
        Dim vprn_BlNos As String = ""
        Dim Rup1 As String, Rup2 As String
        Dim BankNm1 As String = ""
        Dim BankNm2 As String = ""
        Dim BankNm3 As String = ""
        Dim BankNm4 As String = ""

        Try

            For I = NoofDets + 1 To NoofItems_PerPage

                CurY = CurY + TxtHgt

                prn_DetIndx = prn_DetIndx + 1

            Next

            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(6) = CurY

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "TOTAL", LMargin + ClAr(1) + 30, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "" & Trim(prn_HdDt.Rows(0).Item("Net_Weight").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY, 1, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "" & Trim(prn_HdDt.Rows(0).Item("Amount").ToString), PageWidth - 10, CurY, 1, 0, pFont)




            CurY = CurY + TxtHgt

            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(7) = CurY

            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1), CurY, LMargin + ClAr(1), LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2), CurY, LMargin + ClAr(1) + ClAr(2), LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3), LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), LnAr(4))

            C1 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 50
            W1 = e.Graphics.MeasureString("DISCOUNT    : ", pFont).Width
            CurY = CurY + TxtHgt - 10








            CurY = CurY + TxtHgt + 5

            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, PageWidth, CurY)
            LnAr(8) = CurY
            CurY = CurY + TxtHgt - 10
            'Common_Procedures.Print_To_PrintDocument(e, "E & O.E", LMargin + ClAr(1) + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "NET AMOUNT", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 0, 0, pFont)
            p1Font = New Font("Calibri", 12, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, " " & Trim(prn_HdDt.Rows(0).Item("Net_Amount").ToString), PageWidth - 10, CurY, 1, 0, p1Font)

            CurY = CurY + TxtHgt + 5
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(9) = CurY
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), LnAr(4))
            CurY = CurY + TxtHgt - 5
            'BmsInWrds = Common_Procedures.Rupees_Converstion(Val(prn_HdDt.Rows(0).Item("Net_Amount").ToString))
            'BmsInWrds = Replace(Trim(LCase(BmsInWrds)), "", "")

            Rup1 = ""
            Rup2 = ""
            If is_LastPage = True Then
                Rup1 = Common_Procedures.Rupees_Converstion(Val(prn_HdDt.Rows(0).Item("Net_Amount").ToString))
                If Len(Rup1) > 80 Then
                    For I = 80 To 1 Step -1
                        If Mid$(Trim(Rup1), I, 1) = " " Then Exit For
                    Next I
                    If I = 0 Then I = 80
                    Rup2 = Microsoft.VisualBasic.Right(Trim(Rup1), Len(Rup1) - I)
                    Rup1 = Microsoft.VisualBasic.Left(Trim(Rup1), I - 1)
                End If
            End If

            ' CurY = CurY + TxtHgt - 10
            If is_LastPage = True Then
                Common_Procedures.Print_To_PrintDocument(e, "Rupees : " & Rup1, LMargin + 10, CurY, 0, 0, p1Font)
            End If
            If Trim(Rup2) <> "" Then
                CurY = CurY + TxtHgt
                If is_LastPage = True Then
                    Common_Procedures.Print_To_PrintDocument(e, "         " & Rup2, LMargin + 10, CurY, 0, 0, p1Font)
                End If
            End If

            CurY = CurY + TxtHgt + 10
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)


            'CurY = CurY + TxtHgt
            'Common_Procedures.Print_To_PrintDocument(e, "BANK DETAILS : " & Trim(prn_HdDt.Rows(0).Item("Company_Bank_Ac_Details").ToString), LMargin + 10, CurY, 0, 0, p1Font)

            CurY = CurY + TxtHgt + 10
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)



            Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString
            p1Font = New Font("Calibri", 12, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "For " & Cmp_Name, PageWidth - 15, CurY, 1, 0, p1Font)
            'Common_Procedures.Print_To_PrintDocument(e, "3. Subject to Tirupur jurisdiction. ", LMargin + 10, CurY, 0, 0, pFont)

            '' Common_Procedures.Print_To_PrintDocument(e, "For ", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 30, CurY, 1, 0, p1Font)
            CurY = CurY + TxtHgt
            'Common_Procedures.Print_To_PrintDocument(e, "4. Interest at the rate of 24% will be charge from the due date.", LMargin + 10, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            'Common_Procedures.Print_To_PrintDocument(e, "4. All payment should be made by A/C payesr cheque or draft.", LMargin + 10, CurY, 0, 0, pFont)


            CurY = CurY + TxtHgt + 10
            Common_Procedures.Print_To_PrintDocument(e, "Receiver's Signature", LMargin + 20, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "Prepared By ", LMargin + 250, CurY, 0, 0, pFont)



            Common_Procedures.Print_To_PrintDocument(e, "AUTHORISED SIGNATORY ", PageWidth - 5, CurY, 1, 0, pFont)
            CurY = CurY + TxtHgt + 10

            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            e.Graphics.DrawLine(Pens.Black, LMargin, LnAr(1), LMargin, CurY)
            e.Graphics.DrawLine(Pens.Black, PageWidth, LnAr(1), PageWidth, CurY)

            'If Print_PDF_Status = True Then
            '    CurY = CurY + TxtHgt - 15
            '    p1Font = New Font("Calibri", 9, FontStyle.Regular)
            '    Common_Procedures.Print_To_PrintDocument(e, "This computer generated invoice, so need sign", LMargin + 10, CurY, 0, 0, p1Font)
            'End If

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub


End Class