Public Class Roll_Or_Bundle_Packing
    Implements Interface_MDIActions

    Private con As New SqlClient.SqlConnection(Common_Procedures.Connection_String)
    Private con1 As New SqlClient.SqlConnection(Common_Procedures.ConnectionString_CompanyGroupdetails)
    Private FrmLdSTS As Boolean = False
    Private New_Entry As Boolean = False
    Private Insert_Entry As Boolean = False
    Private Filter_Status As Boolean = False
    Private Pk_Condition As String = "PFINS-"
    Private Prec_ActCtrl As New Control
    Private vcbo_KeyDwnVal As Double
    Private prn_HdDt As New DataTable
    Private prn_DetDt As New DataTable
    Private prn_HedDetDt As New DataTable

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
    Private WithEvents dgtxt_rolldetails As New DataGridViewTextBoxEditingControl
    Private WithEvents dgtxt_details As New DataGridViewTextBoxEditingControl

    Private dgv_LevColNo As Integer
    Private dgv_ActCtrlName As String = ""

    Private Sub clear()
        New_Entry = False
        Insert_Entry = False

        pnl_Back.Enabled = True
        pnl_Filter.Visible = False
        pnl_Selection.Visible = False
        cbo_RollOrBundle.Text = "ROLL"
        lbl_RefNo.Text = ""
        lbl_RefNo.ForeColor = Color.Black

        chk_Reprocessing.Checked = False

        dtp_Date.Text = ""
        cbo_Ledger.Text = ""
        lbl_UserName.Text = "USER : " & Trim(UCase(Common_Procedures.User_IdNoToName(con1, Common_Procedures.User.IdNo)))

        lbl_PassPercc.Text = ""
        lbl_RejectPerc.Text = ""

        cbo_Ledger.Enabled = True
        cbo_Ledger.BackColor = Color.White

        dgv_RollDetails.Rows.Clear()
        dgv_RollDetails_Total.Rows.Clear()
        dgv_Details.Rows.Clear()
        dgv_Details_Total.Rows.Clear()
        Grid_DeSelect()


        dgv_ActCtrlName = ""

        dgv_RollDetails.Tag = ""
        dgv_Details.Tag = ""
        dgv_LevColNo = -1

    End Sub
    Private Sub Grid_DeSelect()
        On Error Resume Next
        If Not IsNothing(dgv_RollDetails.CurrentCell) Then dgv_RollDetails.CurrentCell.Selected = False
        If Not IsNothing(dgv_Details.CurrentCell) Then dgv_Details.CurrentCell.Selected = False
    End Sub
    Private Sub ControlGotFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim txtbx As TextBox
        Dim combobx As ComboBox
        Dim chkbx As CheckBox
        On Error Resume Next

        Grid_Cell_DeSelect()

        If TypeOf Me.ActiveControl Is TextBox Or TypeOf Me.ActiveControl Is ComboBox Or TypeOf Me.ActiveControl Is CheckBox Then
            Me.ActiveControl.BackColor = Color.Lime
            Me.ActiveControl.ForeColor = Color.Blue
        End If

        If TypeOf Me.ActiveControl Is TextBox Then
            txtbx = Me.ActiveControl
            txtbx.SelectAll()
        ElseIf TypeOf Me.ActiveControl Is ComboBox Then
            combobx = Me.ActiveControl
            combobx.SelectAll()
        ElseIf TypeOf Me.ActiveControl Is CheckBox Then
            chkbx = Me.ActiveControl
        End If


        If Me.ActiveControl.Name <> dgv_RollDetails_Total.Name Then
            Grid_DeSelect()
        End If

        If Me.ActiveControl.Name <> dgv_Details_Total.Name Then
            Grid_DeSelect()
        End If

        If Me.ActiveControl.Name <> dgv_RollDetails.Name Then
            Common_Procedures.Hide_CurrentStock_Display()
        End If

        Prec_ActCtrl = Me.ActiveControl

    End Sub

    Private Sub ControlLostFocus(ByVal sender As Object, ByVal e As System.EventArgs)

        On Error Resume Next

        If IsNothing(Prec_ActCtrl) = False Then
            If TypeOf Prec_ActCtrl Is TextBox Or TypeOf Prec_ActCtrl Is ComboBox Then
                Prec_ActCtrl.BackColor = Color.White
                Prec_ActCtrl.ForeColor = Color.Black
            ElseIf TypeOf Prec_ActCtrl Is CheckBox Then
                Prec_ActCtrl.BackColor = Color.LightSkyBlue
                Prec_ActCtrl.ForeColor = Color.Maroon
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
        If Not IsNothing(dgv_RollDetails.CurrentCell) Then dgv_RollDetails.CurrentCell.Selected = False
        If Not IsNothing(dgv_RollDetails_Total.CurrentCell) Then dgv_RollDetails_Total.CurrentCell.Selected = False
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
        Dim LockSTS As Boolean = False
        If Val(no) = 0 Then Exit Sub

        clear()

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(no) & "/" & Trim(Common_Procedures.FnYearCode)

        Try

            da1 = New SqlClient.SqlDataAdapter("select a.*, b.Ledger_Name   from Processed_Fabric_inspection_Head a INNER JOIN Ledger_Head b ON a.Ledger_IdNo = b.Ledger_IdNo  Where a.Processed_Fabric_inspection_Code = '" & Trim(NewCode) & "'", con)
            da1.Fill(dt1)

            If dt1.Rows.Count > 0 Then
                lbl_RefNo.Text = dt1.Rows(0).Item("Processed_Fabric_inspection_No").ToString
                dtp_Date.Text = dt1.Rows(0).Item("Processed_Fabric_inspection_Date").ToString
                cbo_RollOrBundle.Text = dt1.Rows(0).Item("Roll_Or_Bundle").ToString
                cbo_Ledger.Text = dt1.Rows(0).Item("Ledger_Name").ToString
              
                If Val(dt1.Rows(0).Item("ReProcessing_Status").ToString) = 1 Then
                    chk_Reprocessing.Checked = True
                Else
                    chk_Reprocessing.Checked = False
                End If

                lbl_PassPercc.Text = Format(Val(dt1.Rows(0).Item("Pass_Percentage").ToString), "########0.00")
                lbl_RejectPerc.Text = Format(Val(dt1.Rows(0).Item("Reject_Percentage").ToString), "########0.00")


                lbl_UserName.Text = "USER : " & Trim(UCase(Common_Procedures.User_IdNoToName(con1, Val(dt1.Rows(0).Item("User_IdNo").ToString))))

                'lbl_Processing_Receipt_Code.Text = dt1.Rows(0).Item("Cloth_Processing_Receipt_Code").ToString
                'lbl_Processing_Receipt_slno.Text = dt1.Rows(0).Item("Cloth_Processing_Receipt_Slno").ToString

                da2 = New SqlClient.SqlDataAdapter("select a.*  from Processed_Fabric_inspection_Details a   where a.Processed_Fabric_inspection_Code = '" & Trim(NewCode) & "' Order by Sl_No", con)
                dt2 = New DataTable
                da2.Fill(dt2)

                dgv_RollDetails.Rows.Clear()
                SNo = 0

                If dt2.Rows.Count > 0 Then

                    For i = 0 To dt2.Rows.Count - 1

                        n = dgv_RollDetails.Rows.Add()

                        SNo = SNo + 1
                        dgv_RollDetails.Rows(n).Cells(0).Value = Val(SNo)
                        dgv_RollDetails.Rows(n).Cells(1).Value = dt2.Rows(i).Item("Roll_No").ToString
                        dgv_RollDetails.Rows(n).Cells(2).Value = dt2.Rows(i).Item("Pcs_no").ToString
                        dgv_RollDetails.Rows(n).Cells(3).Value = Format(Val(dt2.Rows(i).Item("Meters").ToString), "########0.00")
                        dgv_RollDetails.Rows(n).Cells(4).Value = Format(Val(dt2.Rows(i).Item("Weight").ToString), "########0.000")
                        dgv_RollDetails.Rows(n).Cells(5).Value = dt2.Rows(i).Item("Sales_Invoice_Code").ToString

                        If Trim(dgv_RollDetails.Rows(n).Cells(5).Value) <> "" Then
                            For j = 0 To dgv_RollDetails.ColumnCount - 1
                                dgv_RollDetails.Rows(n).Cells(j).Style.BackColor = Color.LightGray
                            Next j
                            LockSTS = True
                        End If

                    Next i

                End If


                'With dgv_Details_Total
                '    If .RowCount = 0 Then .Rows.Add()
                '    .Rows(0).Cells(2).Value = Val(dt1.Rows(0).Item("Total_Pcs").ToString)
                '    .Rows(0).Cells(3).Value = Format(Val(dt1.Rows(0).Item("Total_Meters").ToString), "########0.00")
                '    .Rows(0).Cells(4).Value = Format(Val(dt1.Rows(0).Item("Total_Weight").ToString), "########0.000")
                'End With
                Total_Calculation()

                da2 = New SqlClient.SqlDataAdapter("select a.*, C.Cloth_Name as Fp_Item_Name,d.Colour_Name,e.Lot_No,f.Process_Name from Processed_Fabric_inspection_Receipt_Details a  INNER JOIN Cloth_Head C ON c.Cloth_Idno = a.Item_Idno LEFT OUTER JOIN Colour_Head d ON d.Colour_IdNo = a.Colour_IdNo LEFT OUTER JOIN Lot_Head e ON e.Lot_IdNo = a.Lot_IdNo LEFT OUTER JOIN Process_Head f ON f.Process_IdNo = a.Processing_Idno where a.Processed_Fabric_inspection_Code = '" & Trim(NewCode) & "' Order by Sl_No", con)
                dt2 = New DataTable
                da2.Fill(dt2)

                dgv_Details.Rows.Clear()
                SNo = 0

                If dt2.Rows.Count > 0 Then

                    For i = 0 To dt2.Rows.Count - 1

                        n = dgv_Details.Rows.Add()

                        SNo = SNo + 1
                        dgv_Details.Rows(n).Cells(0).Value = Val(SNo)
                        dgv_Details.Rows(n).Cells(1).Value = dt2.Rows(i).Item("Receipt_No").ToString
                        dgv_Details.Rows(n).Cells(2).Value = dt2.Rows(i).Item("Dc_Rc_No").ToString
                        dgv_Details.Rows(n).Cells(3).Value = dt2.Rows(i).Item("Fp_Item_Name").ToString
                        dgv_Details.Rows(n).Cells(4).Value = dt2.Rows(i).Item("Colour_Name").ToString
                        dgv_Details.Rows(n).Cells(5).Value = dt2.Rows(i).Item("Process_Name").ToString
                        dgv_Details.Rows(n).Cells(6).Value = dt2.Rows(i).Item("Lot_No").ToString
                        dgv_Details.Rows(n).Cells(7).Value = Val(dt2.Rows(i).Item("Pcs").ToString)
                        dgv_Details.Rows(n).Cells(8).Value = Format(Val(dt2.Rows(i).Item("Meters").ToString), "########0.00")
                        dgv_Details.Rows(n).Cells(9).Value = Format(Val(dt2.Rows(i).Item("Weight").ToString), "########0.000")
                        dgv_Details.Rows(n).Cells(10).Value = Val(dt2.Rows(i).Item("Reject_Pcs").ToString)
                        dgv_Details.Rows(n).Cells(11).Value = Format(Val(dt2.Rows(i).Item("Reject_Meters").ToString), "########0.00")
                        dgv_Details.Rows(n).Cells(12).Value = Format(Val(dt2.Rows(i).Item("Reject_Weight").ToString), "########0.000")

                        dgv_Details.Rows(n).Cells(15).Value = dt2.Rows(i).Item("Processed_Fabric_Inspection_Details_Slno").ToString

                        dgv_Details.Rows(n).Cells(13).Value = dt2.Rows(i).Item("Cloth_Processing_Receipt_Code").ToString
                        dgv_Details.Rows(n).Cells(14).Value = dt2.Rows(i).Item("Cloth_Processing_Receipt_Slno").ToString
                        'dgv_Details.Rows(n).Cells(15).Value = dt2.Rows(i).Item("Processed_Fabric_Inspection_Code").ToString

                        'lbl_Processing_Receipt_Code.Text = dt1.Rows(0).Item("Cloth_Processing_Receipt_Code").ToString
                        'lbl_Processing_Receipt_slno.Text = dt1.Rows(0).Item("Cloth_Processing_Receipt_Slno").ToString


                    Next i

                End If

                TotalReceipt_Calculation()


                'With dgv_Details_Total
                '    If .RowCount = 0 Then .Rows.Add()
                '    .Rows(0).Cells(7).Value = Val(dt1.Rows(0).Item("Total_Pcs").ToString)
                '    .Rows(0).Cells(8).Value = Format(Val(dt1.Rows(0).Item("Total_Meters").ToString), "########0.00")
                '    .Rows(0).Cells(9).Value = Format(Val(dt1.Rows(0).Item("Total_Weight").ToString), "########0.000")
                '    .Rows(0).Cells(10).Value = Val(dt1.Rows(0).Item("Total_RejectPcs").ToString)
                '    .Rows(0).Cells(11).Value = Format(Val(dt1.Rows(0).Item("Total_RejectMeters").ToString), "########0.00")
                '    .Rows(0).Cells(12).Value = Format(Val(dt1.Rows(0).Item("Total_RejectWeight").ToString), "########0.000")

                'End With

                Grid_DeSelect()
                Grid_Cell_DeSelect()

                If LockSTS = True Then
                    cbo_Ledger.Enabled = False
                    cbo_Ledger.BackColor = Color.LightGray
                End If

                dt2.Clear()


                dt2.Dispose()
                da2.Dispose()

            End If

            dgv_ActCtrlName = ""

            dt1.Clear()
            dt1.Dispose()
            da1.Dispose()
        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        If dtp_Date.Visible And dtp_Date.Enabled Then dtp_Date.Focus()

    End Sub

    Private Sub Processed_Fabric_Inspection_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated
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

    Private Sub Processed_Fabric_Inspection_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        con.Close()
        con.Dispose()
    End Sub

    Private Sub Processed_Fabric_Inspection_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        Try
            If Asc(e.KeyChar) = 27 Then

                If pnl_Filter.Visible = True Then
                    'btn_Filter_Close_Click(sender, e)
                    'Exit Sub
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

    Private Sub Processed_Fabric_Inspection_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim dt3 As New DataTable
        Dim dt4 As New DataTable
        Dim dt5 As New DataTable
        Dim dt6 As New DataTable
        Dim dt7 As New DataTable

        Me.Text = ""

        con.Open()


        pnl_Filter.Visible = False
        pnl_Filter.Left = (Me.Width - pnl_Filter.Width) \ 2
        pnl_Filter.Top = (Me.Height - pnl_Filter.Height) \ 2

        pnl_Selection.Visible = False
        pnl_Selection.Left = (Me.Width - pnl_Selection.Width) \ 2
        pnl_Selection.Top = (Me.Height - pnl_Selection.Height) \ 2
        pnl_Selection.BringToFront()

        cbo_RollOrBundle.Items.Clear()
        cbo_RollOrBundle.Items.Add("")
        cbo_RollOrBundle.Items.Add("ROLL")
        cbo_RollOrBundle.Items.Add("BUNDLE")

        AddHandler dtp_Date.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_RollOrBundle.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Ledger.GotFocus, AddressOf ControlGotFocus
        AddHandler chk_Reprocessing.GotFocus, AddressOf ControlGotFocus
      
        'AddHandler cbo_Filter_ProcessName.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Filter_PartyName.GotFocus, AddressOf ControlGotFocus

        AddHandler dtp_Date.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_RollOrBundle.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Ledger.LostFocus, AddressOf ControlLostFocus
        AddHandler chk_Reprocessing.LostFocus, AddressOf ControlLostFocus
        

        ' AddHandler cbo_Filter_ProcessName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Filter_PartyName.LostFocus, AddressOf ControlLostFocus

        AddHandler dtp_Date.KeyDown, AddressOf TextBoxControlKeyDown
       
        AddHandler dtp_Date.KeyPress, AddressOf TextBoxControlKeyPress
      
        AddHandler dtp_Filter_Fromdate.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler dtp_Filter_ToDate.KeyDown, AddressOf TextBoxControlKeyDown
        '  AddHandler txt_filterpono.KeyDown, AddressOf TextBoxControlKeyDown

        AddHandler dtp_Filter_Fromdate.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler dtp_Filter_ToDate.KeyPress, AddressOf TextBoxControlKeyPress
        '  AddHandler txt_filterpono.KeyPress, AddressOf TextBoxControlKeyPress


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

        On Error Resume Next

        If ActiveControl.Name = dgv_RollDetails.Name Or ActiveControl.Name = dgv_Details.Name Or TypeOf ActiveControl Is DataGridViewTextBoxEditingControl Then

            dgv1 = Nothing

            If ActiveControl.Name = dgv_RollDetails.Name Then
                dgv1 = dgv_RollDetails

            ElseIf dgv_RollDetails.IsCurrentRowDirty = True Then
                dgv1 = dgv_RollDetails

                'ElseIf pnl_Back.Enabled = True Then
                '    dgv1 = dgv_RollDetails

            ElseIf ActiveControl.Name = dgv_Details.Name Then
                dgv1 = dgv_RollDetails

            ElseIf Trim(UCase(dgv_ActCtrlName)) = Trim(UCase(dgv_RollDetails.Name.ToString)) Then
                dgv1 = dgv_RollDetails

            ElseIf Trim(UCase(dgv_ActCtrlName)) = Trim(UCase(dgv_Details.Name.ToString)) Then
                dgv1 = dgv_Details

            End If

            If IsNothing(dgv1) = False Then

                With dgv1
                    If dgv1.Name = dgv_RollDetails.Name Then

                        If keyData = Keys.Enter Or keyData = Keys.Down Then
                            If .CurrentCell.ColumnIndex >= .ColumnCount - 2 Then
                                If .CurrentCell.RowIndex = .RowCount - 1 Then
                                    ' txt_RejectPcs.Focus()

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
                                    If dgv_Details.Rows.Count > 0 Then
                                        dgv_Details.Focus()
                                        dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(7)
                                        dgv_Details.CurrentCell.Selected = True
                                    Else
                                        cbo_Ledger.Focus()
                                    End If
                                Else
                                    .CurrentCell = .Rows(.CurrentCell.RowIndex - 1).Cells(.ColumnCount - 2)

                                End If

                            Else
                                .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex - 1)

                            End If

                            Return True

                        Else
                            Return MyBase.ProcessCmdKey(msg, keyData)

                        End If

                    ElseIf dgv1.Name = dgv_Details.Name Then
                        If keyData = Keys.Enter Or keyData = Keys.Down Then

                            If .CurrentCell.ColumnIndex >= .ColumnCount - 4 Then

                                If .CurrentCell.RowIndex >= .Rows.Count - 1 Then

                                    If dgv_RollDetails.Rows.Count > 0 Then
                                        dgv_RollDetails.Focus()
                                        dgv_RollDetails.CurrentCell = dgv_Details.Rows(0).Cells(1)
                                        dgv_RollDetails.CurrentCell.Selected = True
                                    Else
                                        btn_save.Focus()
                                    End If

                                Else
                                    .CurrentCell = .Rows(.CurrentCell.RowIndex + 1).Cells(7)


                                End If


                                'ElseIf .CurrentCell.ColumnIndex < 4 Then
                                '    .CurrentCell = .Rows(.CurrentRow.Index).Cells(4)

                            Else
                                .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)

                            End If

                            Return True

                        ElseIf keyData = Keys.Up Then

                            If .CurrentCell.ColumnIndex = 7 Then
                                If .CurrentCell.RowIndex = 0 Then
                                    cbo_Ledger.Focus()

                                Else
                                    .CurrentCell = .Rows(.CurrentCell.RowIndex - 1).Cells(7)

                                End If

                                'ElseIf .CurrentCell.ColumnIndex = 7 Then
                                '    .CurrentCell = .Rows(.CurrentCell.RowIndex - 1).Cells(7)

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
        Dim Qa As Windows.Forms.DialogResult
        Dim vINS_PCS As Single = 0
        Dim vINS_MTR As Single = 0
        Dim vINS_WGT As Single = 0


        '  If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.Entry_Processed_Fabric_Inspection, "~L~") = 0 And InStr(Common_Procedures.UR.Entry_Processed_Fabric_Inspection, "~D~") = 0 Then MessageBox.Show("You have No Rights to Delete", "DOES NOT INSERT...", MessageBoxButtons.OK, MessageBoxIcon.Error) : Exit Sub

        Qa = MessageBox.Show("Do you want to Delete?", "FOR DELETION...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2)
        If Qa = Windows.Forms.DialogResult.No Or Qa = Windows.Forms.DialogResult.Cancel Then
            Exit Sub
        End If

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        If New_Entry = True Then
            MessageBox.Show("This is New Entry", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        Da = New SqlClient.SqlDataAdapter("select Count(*) from Processed_Fabric_inspection_Details Where Processed_Fabric_Inspection_Code = '" & Trim(NewCode) & "' and Sales_Invoice_Code <> ''", con)
        Dt1 = New DataTable
        Da.Fill(Dt1)

        If Dt1.Rows.Count > 0 Then
            If IsDBNull(Dt1.Rows(0)(0).ToString) = False Then
                If Val(Dt1.Rows(0)(0).ToString) > 0 Then
                    MessageBox.Show("Already some pieces invoiced", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Exit Sub
                End If
            End If
        End If
        Dt1.Clear()

        trans = con.BeginTransaction

        Try

            cmd.Connection = con
            cmd.Transaction = trans

            'cmd.CommandText = "Truncate Table TempTable_For_NegativeStock"
            'cmd.ExecuteNonQuery()

            '----cmd by lalitth

            'cmd.CommandText = "Update Textile_Processing_Receipt_Details set Processed_Fabric_inspection_Code = '', Inspection_Meters = 0, Inspection_Pcs = 0,  Inspection_Weight = 0 from Textile_Processing_Receipt_Details Where Processed_Fabric_inspection_Code = '" & Trim(NewCode) & "'"
            'cmd.ExecuteNonQuery()

            '----cmd by lalitth

            Da = New SqlClient.SqlDataAdapter("select * from Processed_Fabric_inspection_Receipt_Details where Processed_Fabric_inspection_Code = '" & Trim(NewCode) & "' ", con)
            Da.SelectCommand.Transaction = trans
            Da.Fill(Dt1)

            cmd.CommandText = "Delete from Processed_Fabric_inspection_Receipt_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Processed_Fabric_inspection_Code = '" & Trim(NewCode) & "' "
            cmd.ExecuteNonQuery()

            If Dt1.Rows.Count > 0 Then
                For I = 0 To Dt1.Rows.Count - 1

                    vINS_PCS = 0
                    vINS_MTR = 0
                    vINS_WGT = 0

                    Common_Procedures.get_ProcessedFabric_inspection_Details(con, Dt1.Rows(I).Item("Cloth_Processing_Receipt_Code").ToString, Dt1.Rows(I).Item("Cloth_Processing_Receipt_Slno").ToString, Val(vINS_PCS), Val(vINS_MTR), Val(vINS_WGT), trans)

                    cmd.CommandText = " Update Textile_Processing_Receipt_Details set Inspection_Meters =  " & Str(Val(vINS_MTR)) & ",Inspection_Pcs =  " & Str(Val(vINS_PCS)) & ",Inspection_Weight =  " & Str(Val(vINS_WGT)) & " Where Cloth_Processing_Receipt_Code  = '" & Trim(Dt1.Rows(I).Item("Cloth_Processing_Receipt_Code").ToString) & "' and  Cloth_Processing_Receipt_Slno = " & Val(Dt1.Rows(I).Item("Cloth_Processing_Receipt_Slno").ToString) & " "
                    cmd.ExecuteNonQuery()

                Next I

            End If



            'cmd.CommandText = "Update Textile_Processing_Receipt_Details Set Inspection_Meters = a.Inspection_Meters - (b.Meters + b.Reject_Meters), Inspection_Pcs = a.Inspection_Pcs - (b.Pcs + b.Reject_Pcs) ,  Inspection_Weight = a.Inspection_Weight - (b.Weight + b.Reject_Weight) from Textile_Processing_Receipt_Details a, Processed_Fabric_inspection_Receipt_Details b Where b.Processed_Fabric_inspection_Code = '" & Trim(NewCode) & "' and a.Cloth_Processing_receipt_Code = b.Cloth_Processing_receipt_Code and a.Cloth_Processing_Receipt_Slno = b.Cloth_Processing_Receipt_Slno"
            'cmd.ExecuteNonQuery()
            'cmd.CommandText = "Update Textile_Processing_Receipt_Details set Inspection_Meters =0, Inspection_Pcs = 0 ,  Inspection_Weight = 0,Processed_Fabric_inspection_Code='' from Textile_Processing_Receipt_Details a, Processed_Fabric_inspection_Receipt_Details b Where a.Processed_Fabric_inspection_Code = '" & Trim(NewCode) & "' "
            'cmd.ExecuteNonQuery()
            'cmd.CommandText = "Update Textile_Processing_Receipt_Details set Inspection_Meters = a.Inspection_Meters - (b.Meters ), Inspection_Pcs = a.Inspection_Pcs - (b.Pcs ) ,  Inspection_Weight = a.Inspection_Weight - (b.Weight ) from Textile_Processing_Receipt_Details a, Processed_Fabric_inspection_Receipt_Details b Where b.Processed_Fabric_inspection_Code = '" & Trim(NewCode) & "' and a.Cloth_Processing_receipt_Code = b.Cloth_Processing_receipt_Code and a.Cloth_Processing_Receipt_Slno = b.Cloth_Processing_Receipt_Slno"
            'cmd.ExecuteNonQuery()


            cmd.CommandText = "Delete from Processed_Fabric_inspection_Receipt_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Processed_Fabric_inspection_Code = '" & Trim(NewCode) & "' "
            cmd.ExecuteNonQuery()


            cmd.CommandText = "Delete from Stock_Cloth_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Processed_Fabric_inspection_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Processed_Fabric_inspection_Code = '" & Trim(NewCode) & "' "
            cmd.ExecuteNonQuery()


            cmd.CommandText = "delete from Processed_Fabric_inspection_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Processed_Fabric_inspection_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            trans.Commit()

            Dt1.Dispose()
            Da.Dispose()
            trans.Dispose()
            cmd.Dispose()

            new_record()

            MessageBox.Show("Deleted Successfully!!!", "FOR DELETION...", MessageBoxButtons.OK, MessageBoxIcon.Information)

        Catch ex As Exception
            trans.Rollback()
            MessageBox.Show(ex.Message, "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        If dtp_Date.Enabled = True And dtp_Date.Visible = True Then dtp_Date.Focus()

    End Sub
    Public Sub filter_record() Implements Interface_MDIActions.filter_record

        If Filter_Status = False Then



            dtp_Filter_Fromdate.Text = ""
            dtp_Filter_ToDate.Text = ""
            cbo_Filter_PartyName.Text = ""
            ' cbo_Filter_ProcessName.Text = ""

            cbo_Filter_PartyName.SelectedIndex = -1
            ' cbo_Filter_ProcessName.SelectedIndex = -1
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

            da = New SqlClient.SqlDataAdapter("select top 1 Processed_Fabric_inspection_No from Processed_Fabric_inspection_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Processed_Fabric_inspection_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby, Processed_Fabric_inspection_No", con)
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

            da = New SqlClient.SqlDataAdapter("select top 1 Processed_Fabric_inspection_No from Processed_Fabric_inspection_Head where for_orderby > " & Str(Val(OrdByNo)) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and Processed_Fabric_inspection_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby, Processed_Fabric_inspection_No", con)
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

            da = New SqlClient.SqlDataAdapter("select top 1 Processed_Fabric_inspection_No from Processed_Fabric_inspection_Head where for_orderby < " & Str(Val(OrdByNo)) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and Processed_Fabric_inspection_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, Processed_Fabric_inspection_No desc", con)
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
            da = New SqlClient.SqlDataAdapter("select top 1 Processed_Fabric_inspection_No from Processed_Fabric_inspection_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Processed_Fabric_inspection_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, Processed_Fabric_inspection_No desc", con)
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

            lbl_RefNo.Text = Common_Procedures.get_MaxCode(con, "Processed_Fabric_inspection_Head", "Processed_Fabric_inspection_Code", "For_OrderBy", "", Val(lbl_Company.Tag), Common_Procedures.FnYearCode)

            lbl_RefNo.ForeColor = Color.Red

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

            inpno = InputBox("Enter Ref.No.", "FOR FINDING...")

            RecCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

            Da = New SqlClient.SqlDataAdapter("select Processed_Fabric_inspection_No from Processed_Fabric_inspection_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Processed_Fabric_inspection_Code = '" & Trim(RecCode) & "'", con)
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
                MessageBox.Show("Ref No. does not exists", "DOES NOT FIND...", MessageBoxButtons.OK, MessageBoxIcon.Error)

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

        ' If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.Entry_Processed_Fabric_Inspection, "~L~") = 0 And InStr(Common_Procedures.UR.Entry_Processed_Fabric_Inspection, "~I~") = 0 Then MessageBox.Show("You have No Rights to Insert", "DOES NOT INSERT...", MessageBoxButtons.OK, MessageBoxIcon.Error) : Exit Sub

        Try

            inpno = InputBox("Enter New Ref No.", "FOR NEW REF INSERTION...")

            RecCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

            Da = New SqlClient.SqlDataAdapter("select Processed_Fabric_inspection_No from Processed_Fabric_inspection_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Processed_Fabric_inspection_Code = '" & Trim(RecCode) & "'", con)
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
                    MessageBox.Show("Invalid Ref No", "DOES NOT INSERT NEW REF...", MessageBoxButtons.OK, MessageBoxIcon.Error)

                Else
                    new_record()
                    Insert_Entry = True
                    lbl_RefNo.Text = Trim(UCase(inpno))

                End If

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT INSERT NEW REF...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Public Sub save_record() Implements Interface_MDIActions.save_record
        Dim cmd As New SqlClient.SqlCommand
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim tr As SqlClient.SqlTransaction
        Dim NewCode As String = ""
        Dim Sew_ID As Integer = 0
        Dim Col_ID As Integer = 0
        Dim Fb_ID As Integer = 0
        Dim Itgry_ID As Integer = 0
        Dim Sno As Integer = 0
        Dim SLno As Integer = 0
        Dim Srno As Integer = 0
        Dim Partcls As String = ""
        Dim EntID As String = ""
        Dim Itfp_ID As Integer = 0
        Dim PBlNo As String = ""
        Dim vTotPcs As Single, vTotMtrs As Single, vtotRolls As Single
        Dim vTotrejPcs As Single, vTotrejMtrs As Single, vtotrejweight As Single
        Dim vTotrecPcs As Single, vTotrecMtrs As Single, vtotrecweight As Single

        Dim Proc_ID As Integer = 0
        Dim Lot_ID As Integer = 0
        Dim vTotWeight As Single
        Dim Tr_ID As Integer = 0
        Dim WagesCode As String = ""
        Dim PcsChkCode As String = ""
        Dim Nr As Integer = 0
        Dim vINS_PCS As String, vINS_MTR As String, vINS_WGT As String
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt2 As New DataTable
        Dim Delv_Mtr As Single = 0
        Dim Excs_Mtr_Rec As Single = 0
        Dim Rec_Mtr As Single = 0
        Dim Excs_Mtr_Retn As Single = 0
        Dim Allow_Sht_Perc As Single = 0
        Dim Retn_Mtr As Single = 0
        Dim Ent_Sht_Perc As Single = 0
        Dim Ent_Sht_Mtr As Single = 0
        Dim Allow_Sht_Mtr As Single = 0
        Dim ReProcSts As Integer = 0
        Dim Roll_Code As String
        Dim ChkMeter As Single = 0
        Dim vTotInsPec As String = 0

        If Val(lbl_Company.Tag) = 0 Then
            MessageBox.Show("Invalid Company Selection", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        ' If Common_Procedures.UserRight_Check(Common_Procedures.UR.Entry_Processed_Fabric_Inspection, New_Entry) = False Then Exit Sub

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

        If cbo_RollOrBundle.Visible = True Then
            If Trim(cbo_RollOrBundle.Text) = "" Then
                MessageBox.Show("Invalid Packing Type", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                If cbo_RollOrBundle.Enabled Then cbo_RollOrBundle.Focus()
                Exit Sub
            End If
        End If
        Sew_ID = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Ledger.Text)
        If Sew_ID = 0 Then
            MessageBox.Show("Invalid Party Name", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If cbo_Ledger.Enabled And cbo_Ledger.Visible Then cbo_Ledger.Focus()
            Exit Sub
        End If

      lbl_UserName.Text = Common_Procedures.User.IdNo

        ReProcSts = 0
        If chk_Reprocessing.Checked = True Then ReProcSts = 1

        With dgv_RollDetails
            For i = 0 To dgv_RollDetails.RowCount - 1
                If Val(.Rows(i).Cells(3).Value) <> 0 Then

                    If Val(dgv_RollDetails.Rows(i).Cells(2).Value) = 0 Then
                        MessageBox.Show("Invalid Pcs..", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        If dgv_RollDetails.Enabled Then dgv_RollDetails.Focus()
                        dgv_RollDetails.CurrentCell = dgv_RollDetails.Rows(0).Cells(2)
                        Exit Sub
                    End If

                    If Val(dgv_RollDetails.Rows(i).Cells(3).Value) = 0 Then
                        MessageBox.Show("Invalid Meters..", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        If dgv_RollDetails.Enabled Then dgv_RollDetails.Focus()
                        dgv_RollDetails.CurrentCell = dgv_RollDetails.Rows(0).Cells(3)
                        Exit Sub
                    End If

                End If


            Next
        End With

        With dgv_Details
            For i = 0 To dgv_Details.RowCount - 1
                If (.Rows(i).Cells(1).Value) <> "" Or (.Rows(i).Cells(2).Value) <> "" Then

                    If Val(dgv_Details.Rows(i).Cells(8).Value) = 0 Then
                        MessageBox.Show("Invalid Meters..", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        If dgv_Details.Enabled Then dgv_Details.Focus()
                        dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(8)
                        Exit Sub
                    End If

                End If
            Next
        End With

        Total_Calculation()
        vTotPcs = 0 : vTotMtrs = 0 : vTotWeight = 0
        If dgv_RollDetails_Total.RowCount > 0 Then
            vtotRolls = Val(dgv_RollDetails_Total.Rows(0).Cells(1).Value())
            vTotPcs = Val(dgv_RollDetails_Total.Rows(0).Cells(2).Value())
            vTotMtrs = Val(dgv_RollDetails_Total.Rows(0).Cells(3).Value())
            vTotWeight = Val(dgv_RollDetails_Total.Rows(0).Cells(4).Value())
        End If

        vTotrejPcs = 0 : vTotrejMtrs = 0 : vTotrejWeight = 0 : vTotrecPcs = 0 : vTotrecMtrs = 0 : vTotrecWeight = 0
        If dgv_Details_Total.RowCount > 0 Then

            vTotrecPcs = Val(dgv_Details_Total.Rows(0).Cells(7).Value())
            vTotrecMtrs = Val(dgv_Details_Total.Rows(0).Cells(8).Value())
            vtotrecweight = Val(dgv_Details_Total.Rows(0).Cells(9).Value())
            vTotrejPcs = Val(dgv_Details_Total.Rows(0).Cells(10).Value())
            vTotrejMtrs = Val(dgv_Details_Total.Rows(0).Cells(11).Value())
            vtotrejweight = Val(dgv_Details_Total.Rows(0).Cells(12).Value())
        End If

        Dt1.Clear()

        tr = con.BeginTransaction

        Try

            If Insert_Entry = True Or New_Entry = False Then
                NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            Else

                lbl_RefNo.Text = Common_Procedures.get_MaxCode(con, "Processed_Fabric_inspection_Head", "Processed_Fabric_inspection_Code", "For_OrderBy", "", Val(lbl_Company.Tag), Common_Procedures.FnYearCode, tr)

                NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            End If

            cmd.Connection = con
            cmd.Transaction = tr

            cmd.Parameters.Clear()
            cmd.Parameters.AddWithValue("@RefDate", dtp_Date.Value.Date)

            'cmd.CommandText = "Truncate Table TempTable_For_NegativeStock"
            'cmd.ExecuteNonQuery()


            If New_Entry = True Then

                cmd.CommandText = "Insert into Processed_Fabric_inspection_Head ( Processed_Fabric_inspection_Code, Company_IdNo, Processed_Fabric_inspection_No, for_OrderBy, Processed_Fabric_inspection_Date, Ledger_IdNo, ReProcessing_Status, Total_Pcs, Total_Meters, Total_Weight, Roll_Or_Bundle, Total_ReceiptPcs, Total_ReceiptMeters, Total_ReceiptWeight ,Total_RejectPcs, Total_RejectMeters, Total_RejectWeight ,   User_idNo , Pass_Percentage , Reject_Percentage) Values ('" & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_RefNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text))) & ", @RefDate, " & Str(Val(Sew_ID)) & "," & Val(ReProcSts) & "," & Str(Val(vTotPcs)) & ", " & Str(Val(vTotMtrs)) & ", " & Str(Val(vTotWeight)) & " ,'" & Trim(cbo_RollOrBundle.Text) & "' ," & Str(Val(vTotrecPcs)) & ", " & Str(Val(vTotrecMtrs)) & ", " & Str(Val(vtotrecweight)) & " , " & Str(Val(vTotrejPcs)) & ", " & Str(Val(vTotrejMtrs)) & ", " & Str(Val(vtotrejweight)) & "," & Val(lbl_UserName.Text) & " , " & Str(Val(lbl_PassPercc.Text)) & "," & Val(lbl_RejectPerc.Text) & " )"
                cmd.ExecuteNonQuery()

            Else

                cmd.CommandText = "Update Processed_Fabric_inspection_Head set Processed_Fabric_inspection_Date = @RefDate, Ledger_IdNo = " & Val(Sew_ID) & ", Total_Pcs = " & Val(vTotPcs) & " , Total_Meters = " & Val(vTotMtrs) & ",Total_Weight = " & Val(vTotWeight) & " ,Total_ReceiptPcs = " & Val(vTotrecPcs) & " , Total_ReceiptMeters = " & Val(vTotrecMtrs) & ",Total_ReceiptWeight = " & Val(vtotrecweight) & ",Total_RejectPcs = " & Val(vTotrejPcs) & " ,Pass_Percentage=  " & Val(lbl_PassPercc.Text) & " , Reject_Percentage =  " & Val(lbl_RejectPerc.Text) & ", Total_RejectMeters = " & Val(vTotrejMtrs) & ",Total_RejectWeight = " & Val(vtotrejweight) & "  ,ReProcessing_Status = " & Val(ReProcSts) & " ,Roll_Or_Bundle = '" & Trim(cbo_RollOrBundle.Text) & "', User_idnO =" & Val(lbl_UserName.Text) & "  Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Processed_Fabric_inspection_Code = '" & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()

                'cmd.CommandText = "Update Textile_Processing_Receipt_Details set Processed_Fabric_inspection_Code = '', Inspection_Meters = 0, Inspection_Pcs = 0,  Inspection_Weight = 0 from Textile_Processing_Receipt_Details Where Processed_Fabric_inspection_Code = '" & Trim(NewCode) & "'"
                'cmd.ExecuteNonQuery()
                'cmd.CommandText = "Update Textile_Processing_Receipt_Details set Inspection_Meters = a.Inspection_Meters - (b.Meters ), Inspection_Pcs = a.Inspection_Pcs - (b.Pcs ) ,  Inspection_Weight = a.Inspection_Weight - (b.Weight ) from Textile_Processing_Receipt_Details a, Processed_Fabric_inspection_Receipt_Details b Where b.Processed_Fabric_inspection_Code = '" & Trim(NewCode) & "' and a.Cloth_Processing_receipt_Code = b.Cloth_Processing_receipt_Code and a.Cloth_Processing_Receipt_Slno = b.Cloth_Processing_Receipt_Slno"
                'cmd.ExecuteNonQuery()



                Da = New SqlClient.SqlDataAdapter("select * from Processed_Fabric_inspection_Receipt_Details where Processed_Fabric_inspection_Code = '" & Trim(NewCode) & "' ", con)
                Da.SelectCommand.Transaction = tr
                Da.Fill(Dt1)

                cmd.CommandText = "Delete from Processed_Fabric_inspection_Receipt_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Processed_Fabric_inspection_Code = '" & Trim(NewCode) & "' "
                cmd.ExecuteNonQuery()

                If Dt1.Rows.Count > 0 Then
                    For I = 0 To Dt1.Rows.Count - 1

                        vINS_PCS = 0
                        vINS_MTR = 0
                        vINS_WGT = 0

                        Common_Procedures.get_ProcessedFabric_inspection_Details(con, Dt1.Rows(I).Item("Cloth_Processing_Receipt_Code").ToString, Dt1.Rows(I).Item("Cloth_Processing_Receipt_Slno").ToString, vINS_PCS, vINS_MTR, vINS_WGT, tr)

                        cmd.CommandText = " Update Textile_Processing_Receipt_Details set Inspection_Meters =  " & Str(Val(vINS_MTR)) & ",Inspection_Pcs =  " & Str(Val(vINS_PCS)) & ",Inspection_Weight =  " & Str(Val(vINS_WGT)) & " Where Cloth_Processing_Receipt_Code  = '" & Trim(Dt1.Rows(I).Item("Cloth_Processing_Receipt_Code").ToString) & "' And Cloth_Processing_Receipt_Slno = " & Val(Dt1.Rows(I).Item("Cloth_Processing_Receipt_Slno").ToString) & " "
                        cmd.ExecuteNonQuery()

                    Next I

                End If





            End If


            cmd.CommandText = "Delete from Stock_Cloth_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Processed_Fabric_inspection_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Processed_Fabric_inspection_Code = '" & Trim(NewCode) & "' "
            cmd.ExecuteNonQuery()

            'cmd.CommandText = "Delete from Processed_Fabric_inspection_Receipt_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Processed_Fabric_inspection_Code = '" & Trim(NewCode) & "' "
            'cmd.ExecuteNonQuery()

            Partcls = "Ref : Dc.No. " & Trim(lbl_RefNo.Text)
            PBlNo = Trim(lbl_RefNo.Text)
            EntID = Trim(Pk_Condition) & Trim(lbl_RefNo.Text)

            ChkMeter = vTotrecMtrs - vTotrejMtrs

            If Format(Val(vTotMtrs), "#######0") <> Format(Val(ChkMeter), "#######0") Then
                tr.Rollback()
                MessageBox.Show("Mismatch of Receipt and Inspection Meters", "FOR SAVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                If dtp_Date.Enabled And dtp_Date.Visible Then dtp_Date.Focus()
                Exit Sub
            End If

            With dgv_Details
                Sno = 0
                SLno = 0
                For i = 0 To .RowCount - 1

                    If Val(.Rows(i).Cells(8).Value) <> 0 Then
                        Sno = Sno + 1
                        SLno = SLno + 1

                        Itfp_ID = Common_Procedures.Cloth_NameToIdNo(con, .Rows(i).Cells(3).Value, tr)
                        Col_ID = Common_Procedures.Colour_NameToIdNo(con, .Rows(i).Cells(4).Value, tr)
                        Lot_ID = Common_Procedures.Lot_NoToIdNo(con, .Rows(i).Cells(6).Value, tr)
                        Proc_ID = Common_Procedures.Process_NameToIdNo(con, .Rows(i).Cells(5).Value, tr)


                        cmd.CommandText = "Insert into Processed_Fabric_inspection_Receipt_Details ( Processed_Fabric_inspection_Code,              Company_IdNo        , Processed_Fabric_Inspection_No,                               for_OrderBy                              , Processed_Fabric_Inspection_date ,          Sl_No         ,            Receipt_No                  ,     Dc_Rc_No                           ,      Ledger_Idno     ,        Item_Idno         , Colour_Idno        , Processing_Idno      , Lot_IdNo           , Pcs                                 ,  Meters                             , Weight                                   ,                       Reject_Pcs            ,                     Reject_Meters         ,   Reject_Weight                          ,         Cloth_Processing_Receipt_Code    ,       Cloth_Processing_Receipt_Slno         ) " &
                                                                                 " Values  (  '" & Trim(NewCode) & "'                , " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_RefNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text))) & ",           @RefDate               ,  " & Str(Val(Sno)) & " , '" & Trim(.Rows(i).Cells(1).Value) & "', '" & Trim(.Rows(i).Cells(2).Value) & "' ,  " & Val(Sew_ID) & ", " & Str(Val(Itfp_ID)) & ", " & Val(Col_ID) & ", " & Val(Proc_ID) & " , " & Val(Lot_ID) & " , " & Val(.Rows(i).Cells(7).Value) & ", " & Val(.Rows(i).Cells(8).Value) & ", " & Str(Val(.Rows(i).Cells(9).Value)) & ", " & Str(Val(.Rows(i).Cells(10).Value)) & " ," & Str(Val(.Rows(i).Cells(11).Value)) & " , '" & Trim(.Rows(i).Cells(12).Value) & "' , '" & Trim(.Rows(i).Cells(13).Value) & "' ,  " & Str(Val(.Rows(i).Cells(14).Value)) & " )"
                        cmd.ExecuteNonQuery()


                        vINS_PCS = 0
                        vINS_MTR = 0
                        vINS_WGT = 0
                        Common_Procedures.get_ProcessedFabric_inspection_Details(con, Trim(.Rows(i).Cells(13).Value), Val(.Rows(i).Cells(14).Value), vINS_PCS, vINS_MTR, vINS_WGT, tr)
                        '***********************************************************
                        cmd.CommandText = "Update Textile_Processing_Receipt_Details Set Processed_Fabric_inspection_Code = '" & Trim(NewCode) & "' , Inspection_Meters =  " & Str(Val(vINS_MTR)) & " , Inspection_Pcs =  " & Str(Val(vINS_PCS)) & " ,Inspection_Weight = " & Str(Val(vINS_WGT)) & " where Cloth_Processing_Receipt_Code = '" & Trim(.Rows(i).Cells(13).Value) & "' and Cloth_Processing_Receipt_Slno = " & Str(Val(.Rows(i).Cells(14).Value)) & " and Ledger_IdNo = " & Str(Val(Sew_ID))
                        Nr = cmd.ExecuteNonQuery()

                        If Nr = 0 Then
                            tr.Rollback()
                            MessageBox.Show("Mismatch of Receipt and Inspection details", "FOR SAVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                            If dtp_Date.Enabled And dtp_Date.Visible Then dtp_Date.Focus()
                            Exit Sub
                        End If

                        '  ----Total Receipt Meters
                        cmd.CommandText = "Insert into Stock_Cloth_Processing_Details ( Reference_Code,         Company_IdNo                       ,           Reference_No        ,                               for_OrderBy                              , Reference_Date, DeliveryTo_Idno ,ReceivedFrom_Idno                                         ,       Entry_ID     ,       Party_Bill_No  ,       Particulars      , Sl_No  , Cloth_Idno        ,   Meters_Type1                     ,StockOff_IdNo                                                    ,Weight                              ,Pcs                              ,Colour_IdNo        ,Process_IdNo       ) " &
                                                                   " Values ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_RefNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text))) & ",  @RefDate     , 0               ," & Str(Val(Common_Procedures.CommonLedger.Godown_Ac)) & ", '" & Trim(EntID) & "', '" & Trim(PBlNo) & "', '" & Trim(Partcls) & "',  " & Val(SLno) & "     ," & Str(Itfp_ID) & " , " & Val(.Rows(i).Cells(8).Value) & ",   " & Str(Val(Common_Procedures.CommonLedger.Godown_Ac)) & "    ," & Val(.Rows(i).Cells(9).Value) & "," & Val(.Rows(i).Cells(7).Value) & "," & Str(Col_ID) & "," & Str(Proc_ID) & ") "
                        cmd.ExecuteNonQuery()

                        SLno = SLno + 1

                        '   ---- Rejected Meters
                        cmd.CommandText = "Insert into Stock_Cloth_Processing_Details ( Reference_Code,         Company_IdNo                       ,           Reference_No        ,                               for_OrderBy                              , Reference_Date  , DeliveryTo_Idno                                           , ReceivedFrom_Idno   ,         Entry_ID     ,       Party_Bill_No  ,       Particulars        , Sl_No , Cloth_Idno           ,   Meters_Type5                    ,StockOff_IdNo                                                ,Reject_Pcs                                 ,Colour_IdNo        ,Process_IdNo        ,Reject_Weight) " &
                                                         " Values ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_RefNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text))) & ",  @RefDate     , " & Str(Val(Common_Procedures.CommonLedger.Godown_Ac)) & ",   0                 , '" & Trim(EntID) & "', '" & Trim(PBlNo) & "', '" & Trim(Partcls) & "'  , " & Val(SLno) & "    ," & Str(Itfp_ID) & " , " & Val(.Rows(i).Cells(11).Value) & ",   " & Str(Val(Common_Procedures.CommonLedger.Godown_Ac)) & " ," & Val(.Rows(i).Cells(10).Value) & "," & Str(Col_ID) & "," & Str(Proc_ID) & "," & Val(.Rows(i).Cells(12).Value) & ") "
                        cmd.ExecuteNonQuery()

                    End If

                    cmd.CommandText = "Update Textile_Processing_Receipt_Details set Processed_Fabric_inspection_Code = '" & Trim(NewCode) & "' Where Cloth_Processing_Receipt_Code = '" & Trim(.Rows(i).Cells(13).Value) & "' and Ledger_IdNo = " & Val(Sew_ID)
                    cmd.ExecuteNonQuery()
                Next
            End With


            If dgv_Details.RowCount > 0 Then
                If Val(dgv_Details.Rows(0).Cells(8).Value) <> 0 Then
                    Itfp_ID = Common_Procedures.Cloth_NameToIdNo(con, dgv_Details.Rows(0).Cells(3).Value, tr)
                End If
            End If

            With dgv_RollDetails
                Srno = 0
                For i = 0 To .RowCount - 1

                    If Val(.Rows(i).Cells(3).Value) <> 0 Then
                        Srno = Srno + 1

                        Roll_Code = Trim(.Rows(i).Cells(1).Value) & "/" & Trim(Common_Procedures.FnYearCode) & "/" & Trim(Val(lbl_Company.Tag))

                        cmd.CommandText = "Insert into Processed_Fabric_inspection_Details(Processed_Fabric_inspection_Code, Company_IdNo, Processed_Fabric_Inspection_No, for_OrderBy, Processed_Fabric_Inspection_date,Fabric_idNo,Colour_IdNo,Process_IdNo, Sl_No, Ledger_IdNo,Roll_No, Pcs_No,Meters,Weight, Roll_Code , Sales_Invoice_Code , Roll_Or_Bundle  ) Values ('" & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_RefNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text))) & ", @RefDate , " & Val(Itfp_ID) & " ," & Val(Col_ID) & " ," & Val(Proc_ID) & " ," & Str(Val(Srno)) & ", " & Val(Sew_ID) & " ,  '" & Trim(.Rows(i).Cells(1).Value) & "','" & Trim(.Rows(i).Cells(2).Value) & "',  " & Val(.Rows(i).Cells(3).Value) & ", " & Val(.Rows(i).Cells(4).Value) & ",'" & Trim(Roll_Code) & "' , '" & Trim(.Rows(i).Cells(5).Value) & "' , '" & Trim(cbo_RollOrBundle.Text) & "'  )"
                        cmd.ExecuteNonQuery()

                    End If
                Next
            End With


            '  ----Inspected Meters
            If cbo_RollOrBundle.Text = "ROLL" Then
                cmd.CommandText = "Insert into Stock_Cloth_Processing_Details ( Reference_Code,         Company_IdNo                       ,           Reference_No        ,                               for_OrderBy                              , Reference_Date, DeliveryTo_Idno                                            ,  ReceivedFrom_Idno ,         Entry_ID     ,       Party_Bill_No  ,       Particulars      ,  Sl_No      , Cloth_Idno        ,   Meters_Type2           ,StockOff_IdNo                                                   ,Weight              ,Rolls                            ,Colour_IdNo        ,Process_IdNo    ,Pass_Percentage ,  Reject_Percentage ) " & _
                                 " Values ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_RefNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text))) & ",  @RefDate     , " & Str(Val(Common_Procedures.CommonLedger.Godown_Ac)) & ",   0                 , '" & Trim(EntID) & "', '" & Trim(PBlNo) & "', '" & Trim(Partcls) & "', 100           ," & Str(Itfp_ID) & " , " & Str(Val(vTotMtrs)) & ",    " & Str(Val(Common_Procedures.CommonLedger.Godown_Ac)) & "     ," & Str(Val(vTotWeight)) & "," & Str(Val(vtotRolls)) & "," & Str(Col_ID) & "," & Str(Proc_ID) & " , " & Str(lbl_PassPercc.Text) & "  ," & Str(lbl_RejectPerc.Text) & ") "
                cmd.ExecuteNonQuery()

            Else
                cmd.CommandText = "Insert into Stock_Cloth_Processing_Details ( Reference_Code,         Company_IdNo                       ,           Reference_No        ,                               for_OrderBy                              , Reference_Date, DeliveryTo_Idno                                            ,  ReceivedFrom_Idno ,         Entry_ID     ,       Party_Bill_No  ,       Particulars      ,  Sl_No      , Cloth_Idno        ,   Meters_Type3          ,StockOff_IdNo                                                    ,Weight                      ,Bundle                            ,Colour_IdNo        ,Process_IdNo   ,Pass_Percentage , Reject_Percentage      ) " & _
                                            " Values ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_RefNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text))) & ",  @RefDate     , " & Str(Val(Common_Procedures.CommonLedger.Godown_Ac)) & ",   0                 , '" & Trim(EntID) & "', '" & Trim(PBlNo) & "', '" & Trim(Partcls) & "',100         ," & Str(Itfp_ID) & " , " & Str(Val(vTotMtrs)) & ",    " & Str(Val(Common_Procedures.CommonLedger.Godown_Ac)) & "     ," & Str(Val(vTotWeight)) & "," & Str(Val(vtotRolls)) & "," & Str(Col_ID) & "," & Str(Proc_ID) & " , " & Str(lbl_PassPercc.Text) & "  ," & Str(lbl_RejectPerc.Text) & " ) "
                cmd.ExecuteNonQuery()

            End If



            'cmd.CommandText = "Update Textile_Processing_Receipt_Details set Inspection_Meters = Inspection_Meters + (" & Val(vTotMtrs) & "+ " & Val(txt_RejectMtr.Text) & " ), Inspection_Pcs = Inspection_Pcs + (" & Val(txt_RecPcs.Text) & " - " & Val(txt_RejectPcs.Text) & ") ,   Inspection_Weight = Inspection_Weight +(" & Val(txt_RecWeight.Text) & " - " & Val(txt_RejectWgt.Text) & ")  Where Cloth_Processing_Receipt_Code = '" & Trim(lbl_Processing_Receipt_Code.Text) & "' and Cloth_Processing_Receipt_Slno = " & Str(Val(lbl_Processing_Receipt_slno.Text)) & " and Ledger_IdNo = " & Str(Val(Sew_ID))
            'cmd.ExecuteNonQuery()


            tr.Commit()

            Dt1.Dispose()
            Da.Dispose()
            If New_Entry = True Then new_record()

            MessageBox.Show("Saved Successfully!!!", "FOR SAVING...", MessageBoxButtons.OK, MessageBoxIcon.Information)

        Catch ex As Exception
            tr.Rollback()
            MessageBox.Show(ex.Message, "FOR SAVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        If dtp_Date.Enabled And dtp_Date.Visible Then dtp_Date.Focus()

    End Sub
    Private Sub TotalReceipt_Calculation()
        Dim vTotPcs As Single, vTotMtrs As Single, vtotweight As Single, vTotRejPcs As Single, vTotRejMtrs As Single, vtotRejweight As Single

        Dim i As Integer
        Dim sno As Integer


        vTotPcs = 0 : vTotMtrs = 0 : vtotweight = 0 : sno = 0 : vTotRejPcs = 0 : vTotRejMtrs = 0 : vtotRejweight = 0
        With dgv_Details
            For i = 0 To dgv_Details.Rows.Count - 1

                sno = sno + 1
                .Rows(i).Cells(0).Value = sno

                If Val(dgv_Details.Rows(i).Cells(8).Value) <> 0 Then
                    vTotPcs = vTotPcs + Val(dgv_Details.Rows(i).Cells(7).Value)
                    vTotMtrs = vTotMtrs + Val(dgv_Details.Rows(i).Cells(8).Value)
                    vtotweight = vtotweight + Val(dgv_Details.Rows(i).Cells(9).Value)
                    vTotRejPcs = vTotRejPcs + Val(dgv_Details.Rows(i).Cells(10).Value)
                    vTotRejMtrs = vTotRejMtrs + Val(dgv_Details.Rows(i).Cells(11).Value)
                    vtotRejweight = vtotRejweight + Val(dgv_Details.Rows(i).Cells(12).Value)
                End If
            Next
        End With

        If dgv_Details_Total.Rows.Count <= 0 Then dgv_Details_Total.Rows.Add()

        dgv_Details_Total.Rows(0).Cells(7).Value = Val(vTotPcs)
        dgv_Details_Total.Rows(0).Cells(8).Value = Format(Val(vTotMtrs), "#########0.00")
        dgv_Details_Total.Rows(0).Cells(9).Value = Format(Val(vtotweight), "#########0.000")
        dgv_Details_Total.Rows(0).Cells(10).Value = Val(vTotRejPcs)
        dgv_Details_Total.Rows(0).Cells(11).Value = Format(Val(vTotRejMtrs), "#########0.00")
        dgv_Details_Total.Rows(0).Cells(12).Value = Format(Val(vtotRejweight), "#########0.000")

        If Val(vTotRejMtrs) <> 0 Then
            lbl_RejectPerc.Text = Format(Val(vTotRejMtrs) / Val(vTotMtrs) * 100, "#########0.00")
            lbl_PassPercc.Text = Format(100 - Val(lbl_RejectPerc.Text), "#########0.00")
        Else
            lbl_PassPercc.Text = 100
            lbl_RejectPerc.Text = 0.0
        End If

    End Sub

    Private Sub Total_Calculation()
        Dim vTotPcs As Single, vTotMtrs As Single, vtotweight As Single, vtotRolls As Single

        Dim i As Integer
        Dim sno As Integer


        vTotPcs = 0 : vTotMtrs = 0 : vtotweight = 0 : sno = 0
        With dgv_RollDetails
            For i = 0 To dgv_RollDetails.Rows.Count - 1

                sno = sno + 1
                .Rows(i).Cells(0).Value = sno

                If Val(dgv_RollDetails.Rows(i).Cells(3).Value) <> 0 Then
                    vtotRolls = vtotRolls + 1
                    vTotPcs = vTotPcs + 1
                    vTotMtrs = vTotMtrs + Val(dgv_RollDetails.Rows(i).Cells(3).Value)
                    vtotweight = vtotweight + Val(dgv_RollDetails.Rows(i).Cells(4).Value)
                End If
            Next
        End With
        If dgv_RollDetails_Total.Rows.Count <= 0 Then dgv_RollDetails_Total.Rows.Add()
        dgv_RollDetails_Total.Rows(0).Cells(1).Value = Val(vtotRolls)
        dgv_RollDetails_Total.Rows(0).Cells(2).Value = Val(vTotPcs)
        dgv_RollDetails_Total.Rows(0).Cells(3).Value = Format(Val(vTotMtrs), "#########0.00")
        dgv_RollDetails_Total.Rows(0).Cells(4).Value = Format(Val(vtotweight), "#########0.000")

     

    End Sub
    Private Sub cbo_Ledger_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Ledger.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", " (Ledger_Type = '' and ( AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14) )", "(Ledger_idno = 0)")

    End Sub
    Private Sub cbo_Ledger_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Ledger.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Ledger, dtp_Date, Nothing, "Ledger_AlaisHead", "Ledger_DisplayName", " (Ledger_Type = '' and ( AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14) )", "(Ledger_idno = 0)")
    End Sub

    Private Sub cbo_Ledger_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Ledger.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Ledger, Nothing, "Ledger_AlaisHead", "Ledger_DisplayName", " (Ledger_Type = '' and ( AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14) ) ", "(Ledger_idno = 0)")
        If Asc(e.KeyChar) = 13 Then
            If MessageBox.Show("Do you want to select Inspection:", "FOR PROCESSED FABRIC INSPECTION SELECTION...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = DialogResult.Yes Then
                btn_Selection_Click(sender, e)

            Else

                If dgv_RollDetails.Rows.Count > 0 Then
                    dgv_RollDetails.Focus()
                    dgv_RollDetails.CurrentCell = dgv_RollDetails.Rows(0).Cells(1)
                End If
            End If

        End If
    End Sub

    Private Sub cbo_Ledger_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Ledger.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Dim f As New Ledger_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Ledger.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()
        End If
    End Sub

   

    Private Sub txt_RejectPcs_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs)
        If (e.KeyValue) = 38 Then
            '  If dgv_Details.Enabled And dgv_Details.Visible Then
            If dgv_RollDetails.Rows.Count > 0 Then
                dgv_RollDetails.Focus()
                dgv_RollDetails.CurrentCell = dgv_RollDetails.Rows(0).Cells(1)
                dgv_RollDetails.CurrentCell.Selected = True

            Else
                '  txt_RecWeight.Focus()

            End If
        End If
        If (e.KeyValue = 40) Then
            '  txt_RejectMtr.Focus()
        End If
    End Sub

    Private Sub txt_RejectWgt_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
            e.Handled = True
        End If
        If Asc(e.KeyChar) = 13 Then
            If MessageBox.Show("Do you want to save ?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = DialogResult.Yes Then
                save_record()
            Else
                dtp_Date.Focus()
            End If
        End If
    End Sub

    Private Sub dgv_rollDetails_CellEndEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_RollDetails.CellEndEdit
        dgv_rollDetails_CellLeave(sender, e)
    End Sub
    Private Sub dgv_rollDetails_EditingControlShowing(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewEditingControlShowingEventArgs) Handles dgv_RollDetails.EditingControlShowing
        dgtxt_rolldetails = CType(dgv_RollDetails.EditingControl, DataGridViewTextBoxEditingControl)
    End Sub
    Private Sub dgtxt_rollDetails_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgtxt_rolldetails.Enter

        dgv_ActCtrlName = dgv_RollDetails.Name.ToString

        dgv_RollDetails.EditingControl.BackColor = Color.Lime
        dgv_RollDetails.EditingControl.ForeColor = Color.Blue
    End Sub
    Private Sub dgtxt_rolldetails_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgtxt_rolldetails.KeyDown
        'Try

        '    With dgv_Details

        '        If e.KeyValue = Keys.Delete Then
        '            If Trim(.Rows(.CurrentCell.RowIndex).Cells(14).Value) <> "" Then
        '                e.Handled = True
        '            End If
        '        End If
        '    End With

        'Catch ex As Exception

        'End Try
    End Sub

    Private Sub dgtxt_rollDetails_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dgtxt_rolldetails.KeyPress
        Try


            With dgv_RollDetails

                If Trim(dgv_RollDetails.Rows(dgv_RollDetails.CurrentCell.RowIndex).Cells(5).Value) <> "" Then
                    e.Handled = True

                Else
                    If Val(dgv_RollDetails.CurrentCell.ColumnIndex.ToString) = 2 Or Val(dgv_RollDetails.CurrentCell.ColumnIndex.ToString) = 3 Or Val(dgv_RollDetails.CurrentCell.ColumnIndex.ToString) = 4 Then

                        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
                            e.Handled = True
                        End If
                    End If

                End If


            End With
        Catch ex As Exception

        End Try
    End Sub
    Private Sub dgv_rollDetails_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_RollDetails.KeyUp
        Dim n As Integer

        If e.Control = True And UCase(Chr(e.KeyCode)) = "D" Then

            With dgv_RollDetails

                n = .CurrentRow.Index

                If Trim(.Rows(n).Cells(5).Value) <> "" Then
                    MessageBox.Show("Already this Roll was invoiced", "DOES NOT REMOVE...", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                    Exit Sub
                End If

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


    Private Sub dgv_rollDetails_RowsAdded(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewRowsAddedEventArgs) Handles dgv_RollDetails.RowsAdded
        Dim n As Integer

        With dgv_RollDetails
            n = .RowCount
            .Rows(n - 1).Cells(0).Value = Val(n)
        End With
    End Sub
    Private Sub dgv_rollDetails_CellLeave(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_RollDetails.CellLeave

        With dgv_RollDetails
            ' dgv_LevColNo = .CurrentCell.ColumnIndex
            dgv_ActCtrlName = .Name.ToString

            If .CurrentCell.ColumnIndex = 3 Then
                If Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value) <> 0 Then
                    .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = Format(Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value), "#########0.00")
                End If
            End If
            If .CurrentCell.ColumnIndex = 4 Then
                If Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value) <> 0 Then
                    .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = Format(Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value), "#########0.000")
                End If
            End If
        End With
    End Sub

    Private Sub dgv_rollDetails_CellValueChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_RollDetails.CellValueChanged
        On Error Resume Next
        With dgv_RollDetails

            If IsNothing(dgv_RollDetails.CurrentCell) Then Exit Sub
            If .Visible Then
                If .CurrentCell.ColumnIndex = 2 Or .CurrentCell.ColumnIndex = 3 Or .CurrentCell.ColumnIndex = 4 Then
                    Total_Calculation()
                End If
            End If
        End With
    End Sub

    Private Sub btn_Selection_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Selection.Click
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim Dt2 As New DataTable
        Dim i As Integer, j As Integer, n As Integer, SNo As Integer
        Dim LedIdNo As Integer
        Dim NewCode As String
        Dim CompIDCondt As String
        Dim Ent_Pcs As Single = 0
        Dim Ent_Mtrs As Single = 0, Ent_Wgt As Single = 0

        LedIdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Ledger.Text)

        If LedIdNo = 0 Then
            MessageBox.Show("Invalid Party Name", "DOES NOT SELECT PROCESSING RECEIPT...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If cbo_Ledger.Enabled And cbo_Ledger.Visible Then cbo_Ledger.Focus()
            Exit Sub
        End If

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)


        CompIDCondt = "(a.company_idno = " & Str(Val(lbl_Company.Tag)) & ")"
        If Trim(UCase(Common_Procedures.settings.CompanyName)) = "-----~~~" Then
            CompIDCondt = ""
        End If

        With dgv_Selection

            .Rows.Clear()
            SNo = 0

            'Da = New SqlClient.SqlDataAdapter("select a.* , b.* , b.item_Idno, e.Ledger_Name as Transportname,h.Receipt_Pcs As Ent_Pcs, h.Receipt_Meters as Ent_Mtrs, h.Receipt_Weight As Ent_Wgt, h.Receipt_Qty As Ent_Qty, g.Cloth_Name as Fp_Item_Name , I.Lot_No , j.Process_Name , k.Colour_Name  from Textile_Processing_Delivery_Head a INNER JOIN Textile_Processing_Delivery_Details b ON a.ClothProcess_Delivery_Code = b.Cloth_Processing_Delivery_Code INNER JOIN Cloth_Head g ON g.Cloth_Idno = b.Item_to_IdNo  LEFT OUTER JOIN Lot_Head i ON b.Lot_IdNo = i.Lot_IdNo LEFT OUTER JOIN Process_Head J ON J.Process_IdNo = b.Processing_Idno LEFT OUTER JOIN Colour_Head k ON b.Colour_IdNo = k.Colour_IdNo LEFT OUTER JOIN Ledger_Head e ON a.Transport_IdNo = e.Ledger_IdNo LEFT OUTER JOIN Textile_Processing_Receipt_Details h ON h.Cloth_Processing_Receipt_Code = '" & Trim(NewCode) & "' and b.Cloth_Processing_Delivery_Code = h.Cloth_Processing_Delivery_Code and b.Cloth_Processing_Delivery_SlNo = h.Cloth_Processing_Delivery_SlNo Where " & CompIDCondt & IIf(Trim(CompIDCondt) <> "", " and ", "") & " a.ledger_Idno = " & Str(Val(LedIdNo)) & " and ((b.Delivery_Meters - b.Receipt_Meters - b.Return_Meters) > 0 or h.Receipt_Meters > 0 ) order by a.ClothProcess_Delivery_Date, a.for_orderby, a.ClothProcess_Delivery_No", con)
            'Dt1 = New DataTable
            'nr = Da.Fill(Dt1)

            Da = New SqlClient.SqlDataAdapter("select a.*,b.*, c.Cloth_Name as Fabric_name, d.Colour_Name,e.Lot_No ,f.Process_Name, g.Pcs as Ent_Pcs ,g.Meters as Ent_Mtrs , g.Weight as Ent_Wgt ,g.Reject_Pcs as Ent_RejPcs ,g.Reject_Meters as Ent_RejMtrs , g.Reject_Weight as Ent_RejWgt  from Textile_Processing_Receipt_Details a INNER JOIN Textile_Processing_Receipt_Head B ON b.ClothProcess_Receipt_Code = a.Cloth_Processing_Receipt_Code INNER JOIN Cloth_Head c ON c.Cloth_Idno = a.Item_To_Idno LEFT OUTER JOIN Colour_Head d ON d.Colour_IdNo = a.Colour_IdNo LEFT OUTER JOIN Lot_Head e ON e.Lot_IdNo = a.Lot_IdNo LEFT OUTER JOIN Process_Head f ON f.Process_IdNo = a.Processing_Idno  LEFT OUTER JOIN Processed_Fabric_Inspection_Receipt_Details g ON g.Processed_Fabric_inspection_Code = '" & Trim(NewCode) & "' and  g.Cloth_Processing_Receipt_Code = a.Cloth_Processing_Receipt_Code and a.Cloth_Processing_Receipt_SlNo = g.Cloth_Processing_Receipt_SlNo LEFT OUTER JOIN Textile_Processing_Receipt_Head h ON h.ClothProcess_Receipt_Code = a.Cloth_Processing_Receipt_Code where  " & CompIDCondt & IIf(Trim(CompIDCondt) <> "", " and ", "") & " a.ledger_Idno = " & Str(Val(LedIdNo)) & " and ((a.Receipt_Meters - a.Inspection_Meters) > 0 or g.Meters > 0 )  order by a.Cloth_Processing_Receipt_Date, a.for_orderby, a.Cloth_Processing_Receipt_No", con)

            Dt1 = New DataTable
            Da.Fill(Dt1)

            If Dt1.Rows.Count > 0 Then

                For i = 0 To Dt1.Rows.Count - 1

                    n = .Rows.Add()

                    Ent_Wgt = 0
                    Ent_Pcs = 0
                    Ent_Mtrs = 0

                    If IsDBNull(Dt1.Rows(i).Item("Ent_Pcs").ToString) = False Then
                        Ent_Pcs = Val(Dt1.Rows(i).Item("Ent_Pcs").ToString)
                    End If

                    If IsDBNull(Dt1.Rows(i).Item("Ent_Mtrs").ToString) = False Then
                        Ent_Mtrs = Val(Dt1.Rows(i).Item("Ent_Mtrs").ToString)
                    End If
                    If IsDBNull(Dt1.Rows(i).Item("Ent_Wgt").ToString) = False Then
                        Ent_Wgt = Val(Dt1.Rows(i).Item("Ent_Wgt").ToString)
                    End If

                    SNo = SNo + 1
                    .Rows(n).Cells(0).Value = Val(SNo)

                    .Rows(n).Cells(0).Value = Val(SNo)
                    .Rows(n).Cells(1).Value = Dt1.Rows(i).Item("ClothProcess_Receipt_No").ToString
                    .Rows(n).Cells(2).Value = Dt1.Rows(i).Item("Dc_Rc_No").ToString
                    .Rows(n).Cells(3).Value = Dt1.Rows(i).Item("Job_No").ToString
                    .Rows(n).Cells(4).Value = Dt1.Rows(i).Item("Fabric_Name").ToString
                    .Rows(n).Cells(5).Value = Dt1.Rows(i).Item("Colour_Name").ToString
                    .Rows(n).Cells(6).Value = Dt1.Rows(i).Item("Process_Name").ToString
                    .Rows(n).Cells(7).Value = Dt1.Rows(i).Item("Lot_No").ToString
                    .Rows(n).Cells(8).Value = Val(Dt1.Rows(i).Item("Receipt_Pcs").ToString) - Val(Dt1.Rows(i).Item("Inspection_pcs").ToString) + Val(Ent_Pcs)
                    .Rows(n).Cells(9).Value = Format(Val(Dt1.Rows(i).Item("Receipt_Meters").ToString) - Val(Dt1.Rows(i).Item("Inspection_Meters").ToString) + Val(Ent_Mtrs), "#########0.00")
                    .Rows(n).Cells(10).Value = Format(Val(Dt1.Rows(i).Item("Receipt_Weight").ToString) - Val(Dt1.Rows(i).Item("Inspection_Weight").ToString) + Val(Ent_Wgt), "#########0.000")
                    If Ent_Mtrs > 0 Then
                        .Rows(n).Cells(11).Value = "1"
                        For j = 0 To .ColumnCount - 1
                            .Rows(n).Cells(j).Style.ForeColor = Color.Red
                        Next

                    Else
                        .Rows(n).Cells(11).Value = ""

                    End If

                    .Rows(n).Cells(12).Value = Dt1.Rows(i).Item("Cloth_Processing_Receipt_Code").ToString
                    .Rows(n).Cells(13).Value = Dt1.Rows(i).Item("Cloth_Processing_Receipt_Slno").ToString
                    .Rows(n).Cells(14).Value = Ent_Pcs

                    .Rows(n).Cells(15).Value = Ent_Mtrs
                    .Rows(n).Cells(16).Value = Ent_Wgt

                    .Rows(n).Cells(17).Value = Val(Dt1.Rows(i).Item("Ent_RejPcs").ToString)
                    .Rows(n).Cells(18).Value = Val(Dt1.Rows(i).Item("Ent_RejMtrs").ToString)
                    .Rows(n).Cells(19).Value = Val(Dt1.Rows(i).Item("Ent_RejWgt").ToString)

                Next

            End If
            Dt1.Clear()

        End With

        pnl_Selection.Visible = True
        pnl_Back.Enabled = False
        dgv_Selection.Focus()

    End Sub


    Private Sub dgv_Selection_CellClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Selection.CellClick
        Select_Piece(e.RowIndex)
    End Sub

    Private Sub Select_Piece(ByVal RwIndx As Integer)
        Dim i As Integer

        With dgv_Selection

            If .RowCount > 0 And RwIndx >= 0 Then

                .Rows(RwIndx).Cells(11).Value = (Val(.Rows(RwIndx).Cells(11).Value) + 1) Mod 2
                If Val(.Rows(RwIndx).Cells(11).Value) = 1 Then

                    For i = 0 To .ColumnCount - 1
                        .Rows(RwIndx).Cells(i).Style.ForeColor = Color.Red
                    Next


                Else
                    .Rows(RwIndx).Cells(11).Value = ""

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
        Close_Inspection_Selection()
    End Sub

    Private Sub Close_Inspection_Selection()
        Dim Da1 As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim n As Integer = 0
        Dim sno As Integer = 0
        Dim i As Integer = 0
        Dim j As Integer = 0

        dgv_Details.Rows.Clear()

        For i = 0 To dgv_Selection.RowCount - 1

            If Val(dgv_Selection.Rows(i).Cells(11).Value) = 1 Then

                n = dgv_Details.Rows.Add()

                sno = sno + 1
                dgv_Details.Rows(n).Cells(0).Value = Val(sno)
                dgv_Details.Rows(n).Cells(1).Value = dgv_Selection.Rows(i).Cells(1).Value
                dgv_Details.Rows(n).Cells(2).Value = dgv_Selection.Rows(i).Cells(2).Value
                dgv_Details.Rows(n).Cells(3).Value = dgv_Selection.Rows(i).Cells(4).Value
                dgv_Details.Rows(n).Cells(4).Value = dgv_Selection.Rows(i).Cells(5).Value
                dgv_Details.Rows(n).Cells(5).Value = dgv_Selection.Rows(i).Cells(6).Value
                dgv_Details.Rows(n).Cells(6).Value = dgv_Selection.Rows(i).Cells(7).Value

                If Val(dgv_Selection.Rows(i).Cells(14).Value) <> 0 Then
                    dgv_Details.Rows(n).Cells(7).Value = dgv_Selection.Rows(i).Cells(14).Value
                Else
                    dgv_Details.Rows(n).Cells(7).Value = dgv_Selection.Rows(i).Cells(8).Value
                End If

                If Val(dgv_Selection.Rows(i).Cells(15).Value) <> 0 Then
                    dgv_Details.Rows(n).Cells(8).Value = dgv_Selection.Rows(i).Cells(15).Value
                Else
                    dgv_Details.Rows(n).Cells(8).Value = dgv_Selection.Rows(i).Cells(9).Value
                End If

                If Val(dgv_Selection.Rows(i).Cells(16).Value) <> 0 Then
                    dgv_Details.Rows(n).Cells(9).Value = dgv_Selection.Rows(i).Cells(16).Value
                Else
                    dgv_Details.Rows(n).Cells(9).Value = dgv_Selection.Rows(i).Cells(10).Value
                End If

                dgv_Details.Rows(n).Cells(10).Value = dgv_Selection.Rows(i).Cells(17).Value
                dgv_Details.Rows(n).Cells(11).Value = dgv_Selection.Rows(i).Cells(18).Value
                dgv_Details.Rows(n).Cells(12).Value = dgv_Selection.Rows(i).Cells(19).Value

                dgv_Details.Rows(n).Cells(13).Value = dgv_Selection.Rows(i).Cells(12).Value
                dgv_Details.Rows(n).Cells(14).Value = dgv_Selection.Rows(i).Cells(13).Value

            End If

        Next

        TotalReceipt_Calculation()

        pnl_Back.Enabled = True
        pnl_Selection.Visible = False
        '    If txt_RecPcs.Visible And txt_RecPcs.Enabled Then txt_RecPcs.Focus()
        If dgv_Details.Rows.Count > 0 Then
            dgv_Details.Focus()
            dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(7)
            dgv_Details.CurrentCell.Selected = True
        Else
            btn_save.Focus()
        End If

    End Sub

    Private Sub btn_save_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_save.Click
        save_record()
    End Sub

    Private Sub btn_close_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_close.Click
        Me.Close()
    End Sub

    Private Sub cbo_RollOrBundle_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_RollOrBundle.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_RollOrBundle, dtp_Date, cbo_Ledger, "", "", "", "")
    End Sub

    Private Sub cbo_RollOrBundle_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_RollOrBundle.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_RollOrBundle, cbo_Ledger, "", "", "", "")

    End Sub


    Private Sub txt_RecWeight_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
            e.Handled = True
        End If
        If Asc(e.KeyChar) = 13 Then
            If dgv_RollDetails.Rows.Count > 0 Then
                dgv_RollDetails.Focus()
                dgv_RollDetails.CurrentCell = dgv_RollDetails.Rows(0).Cells(1)
                dgv_RollDetails.CurrentCell.Selected = True

            Else
                '  txt_RejectPcs.Focus()

            End If
        End If
    End Sub


    Private Sub dgv_Details_CellEndEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellEndEdit
        With dgv_Details

            If .CurrentCell.ColumnIndex = 9 Then
                If Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value) <> 0 Then
                    .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = Format(Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value), "#########0.00")
                End If
            End If
            If .CurrentCell.ColumnIndex = 10 Then
                If Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value) <> 0 Then
                    .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = Format(Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value), "#########0.000")
                End If
            End If
            TotalReceipt_Calculation()

        End With
    End Sub

    Private Sub dgv_Details_CellEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellEnter
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim Dt2 As New DataTable
        Dim Dt3 As New DataTable
        Dim Dt4 As New DataTable

        With dgv_Details
            dgv_ActCtrlName = .Name.ToString


            If Val(.CurrentRow.Cells(0).Value) = 0 Then
                .CurrentRow.Cells(0).Value = .CurrentRow.Index + 1
            End If


            'If e.ColumnIndex = 2 Then

            '    If cbo_itemfp.Visible = False Or Val(cbo_itemfp.Tag) <> e.RowIndex Then

            '        cbo_itemfp.Tag = -1
            '        Da = New SqlClient.SqlDataAdapter("select Processed_Item_Name from Processed_Item_Head where Processed_Item_Type = 'FP' order by Processed_Item_Name", con)
            '        Dt1 = New DataTable
            '        Da.Fill(Dt1)
            '        cbo_itemfp.DataSource = Dt1
            '        cbo_itemfp.DisplayMember = "Procesed_Item_Name"

            '        rect = .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False)

            '        cbo_itemfp.Left = .Left + rect.Left  '  .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Left
            '        cbo_itemfp.Top = .Top + rect.Top  ' .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Top
            '        cbo_itemfp.Width = rect.Width  ' .CurrentCell.Size.Width
            '        cbo_itemfp.Height = rect.Height  ' rect.Height

            '        cbo_itemfp.Text = .CurrentCell.Value  '  Trim(.CurrentRow.Cells(e.ColumnIndex).Value)

            '        cbo_itemfp.Tag = Val(e.RowIndex)
            '        cbo_itemfp.Visible = True

            '        cbo_itemfp.BringToFront()
            '        cbo_itemfp.Focus()



            '    End If

            'Else

            '    cbo_itemfp.Visible = False

            'End If

            'If e.ColumnIndex = 3 Then

            '    If cbo_Colour.Visible = False Or Val(cbo_Colour.Tag) <> e.RowIndex Then

            '        cbo_Colour.Tag = -1
            '        Da = New SqlClient.SqlDataAdapter("select Colour_Name from Colour_Head order by Colour_Name", con)
            '        Dt2 = New DataTable
            '        Da.Fill(Dt2)
            '        cbo_Colour.DataSource = Dt2
            '        cbo_Colour.DisplayMember = "Colour_Name"

            '        rect = .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False)

            '        cbo_Colour.Left = .Left + rect.Left
            '        cbo_Colour.Top = .Top + rect.Top
            '        cbo_Colour.Width = rect.Width
            '        cbo_Colour.Height = rect.Height

            '        cbo_Colour.Text = .CurrentCell.Value

            '        cbo_Colour.Tag = Val(e.RowIndex)
            '        cbo_Colour.Visible = True

            '        cbo_Colour.BringToFront()
            '        cbo_Colour.Focus()



            '    End If

            'Else

            '    cbo_Colour.Visible = False


            'End If


            'If e.ColumnIndex = 4 Then

            '    If cbo_Processing.Visible = False Or Val(cbo_Processing.Tag) <> e.RowIndex Then

            '        cbo_Processing.Tag = -1
            '        Da = New SqlClient.SqlDataAdapter("select Process_Name from Process_Head order by Process_Name", con)
            '        Dt3 = New DataTable
            '        Da.Fill(Dt3)
            '        cbo_Processing.DataSource = Dt3
            '        cbo_Processing.DisplayMember = "Process_Name"

            '        rect = .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False)

            '        cbo_Processing.Left = .Left + rect.Left  '  .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Left
            '        cbo_Processing.Top = .Top + rect.Top  ' .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Top
            '        cbo_Processing.Width = rect.Width  ' .CurrentCell.Size.Width
            '        cbo_Processing.Height = rect.Height  ' rect.Height

            '        cbo_Processing.Text = .CurrentCell.Value  '  Trim(.CurrentRow.Cells(e.ColumnIndex).Value)

            '        cbo_Processing.Tag = Val(e.RowIndex)
            '        cbo_Processing.Visible = True

            '        cbo_Processing.BringToFront()
            '        cbo_Processing.Focus()

            '        'cbo_Grid_CountName.Visible = False
            '        'cbo_Grid_MillName.Visible = False

            '    End If

            'Else

            '    cbo_Processing.Visible = False
            '    'cbo_Grid_MillName.Tag = -1
            '    'cbo_Grid_MillName.Text = ""

            'End If

            'If e.ColumnIndex = 5 Then

            '    If cbo_LotNo.Visible = False Or Val(cbo_LotNo.Tag) <> e.RowIndex Then

            '        cbo_LotNo.Tag = -1
            '        Da = New SqlClient.SqlDataAdapter("select Lot_No from Lot_Head order by Lot_No", con)
            '        Dt4 = New DataTable
            '        Da.Fill(Dt4)
            '        cbo_LotNo.DataSource = Dt4
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

            '        'cbo_Grid_CountName.Visible = False
            '        'cbo_Grid_MillName.Visible = False

            '    End If

            'Else

            '    cbo_LotNo.Visible = False
            '    'cbo_Grid_MillName.Tag = -1
            '    'cbo_Grid_MillName.Text = ""

            'End If

            'If e.ColumnIndex = 8 And dgv_LevColNo <> 8 Then
            '    '   Show_Item_CurrentStock(e.RowIndex)
            '    .Focus()
            'End If

        End With
    End Sub

    Private Sub dgv_Details_CellLeave(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellLeave

        With dgv_Details
            dgv_LevColNo = .CurrentCell.ColumnIndex
            If .CurrentCell.ColumnIndex = 7 Or .CurrentCell.ColumnIndex = 8 Or .CurrentCell.ColumnIndex = 10 Or .CurrentCell.ColumnIndex = 11 Then
                If Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value) <> 0 Then
                    .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = Format(Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value), "#########0.00")
                End If
            End If
            If .CurrentCell.ColumnIndex = 9 Or .CurrentCell.ColumnIndex = 12 Then
                If Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value) <> 0 Then
                    .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = Format(Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value), "#########0.000")
                End If
            End If
        End With
    End Sub

    Private Sub dgv_Details_CellValueChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellValueChanged
        On Error Resume Next
        If IsNothing(dgv_Details.CurrentCell) Then Exit Sub
        With dgv_Details
            dgv_ActCtrlName = .Name.ToString

            If .Visible Then
                If .CurrentCell.ColumnIndex = 11 Or .CurrentCell.ColumnIndex = 7 Or .CurrentCell.ColumnIndex = 9 Or .CurrentCell.ColumnIndex = 10 Or .CurrentCell.ColumnIndex = 8 Or .CurrentCell.ColumnIndex = 12 Then
                    TotalReceipt_Calculation()
                End If
            End If
        End With
    End Sub


    Private Sub dgv_Details_EditingControlShowing(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewEditingControlShowingEventArgs) Handles dgv_Details.EditingControlShowing
        dgtxt_details = CType(dgv_Details.EditingControl, DataGridViewTextBoxEditingControl)
    End Sub
    Private Sub dgtxt_Details_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgtxt_details.Enter
        dgv_Details.EditingControl.BackColor = Color.Lime
    End Sub
    Private Sub dgtxt_details_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgtxt_details.KeyDown
        Try

            With dgv_Details

                If e.KeyValue = Keys.Delete Then
                    If Trim(.Rows(.CurrentCell.RowIndex).Cells(13).Value) <> "" Then
                        e.Handled = True
                    End If
                    If Trim(.Rows(.CurrentCell.RowIndex).Cells(14).Value) <> "" Then
                        e.Handled = True
                    End If
                End If
            End With

        Catch ex As Exception

        End Try
    End Sub

    Private Sub dgtxt_Details_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dgtxt_details.KeyPress
        Try


            With dgv_Details


                'If Trim(.Rows(.CurrentCell.RowIndex).Cells(14).Value) <> "" Then
                '    e.Handled = True
                'ElseIf Trim(.Rows(.CurrentCell.RowIndex).Cells(15).Value) <> "" Then
                '    e.Handled = True
                'Else
                If Val(dgv_Details.CurrentCell.ColumnIndex.ToString) = 11 Or Val(dgv_Details.CurrentCell.ColumnIndex.ToString) = 7 Or Val(dgv_Details.CurrentCell.ColumnIndex.ToString) = 8 Or Val(dgv_Details.CurrentCell.ColumnIndex.ToString) = 9 Or Val(dgv_Details.CurrentCell.ColumnIndex.ToString) = 10 Or Val(dgv_Details.CurrentCell.ColumnIndex.ToString) = 12 Then

                    If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
                        e.Handled = True
                    End If
                End If
                ' End If
            End With
        Catch ex As Exception

        End Try
    End Sub
    Private Sub dgv_Details_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_Details.KeyUp
        Dim n As Integer

        If e.Control = True And UCase(Chr(e.KeyCode)) = "D" Then

            With dgv_Details

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

            TotalReceipt_Calculation()

        End If


    End Sub


    Private Sub dgv_Details_RowsAdded(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewRowsAddedEventArgs) Handles dgv_Details.RowsAdded
        Dim n As Integer
        If IsNothing(dgv_Details.CurrentCell) Then Exit Sub
        With dgv_Details
            n = .RowCount
            .Rows(n - 1).Cells(0).Value = Val(n)
        End With
    End Sub

    Public Sub print_record() Implements Interface_MDIActions.print_record
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim NewCode As String
        Dim PpSzSTS As Boolean = False
        Dim I As Integer = 0
        Dim ps As Printing.PaperSize

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Try

            da1 = New SqlClient.SqlDataAdapter("select * from Processed_Fabric_inspection_Head Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Processed_Fabric_inspection_Code = '" & Trim(NewCode) & "'", con)
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


        For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
            ps = PrintDocument1.PrinterSettings.PaperSizes(I)
            'Debug.Print(ps.PaperName)
            If ps.Width = 800 And ps.Height = 600 Then
                PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = ps
                PrintDocument1.DefaultPageSettings.PaperSize = ps
                PpSzSTS = True
                Exit For
            End If
        Next

        If PpSzSTS = False Then
            For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
                If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.GermanStandardFanfold Then
                    ps = PrintDocument1.PrinterSettings.PaperSizes(I)
                    PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = ps
                    PrintDocument1.DefaultPageSettings.PaperSize = ps
                    PpSzSTS = True
                    Exit For
                End If
            Next

            If PpSzSTS = False Then
                For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
                    If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
                        ps = PrintDocument1.PrinterSettings.PaperSizes(I)
                        PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = ps
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

                        For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
                            ps = PrintDocument1.PrinterSettings.PaperSizes(I)
                            'Debug.Print(ps.PaperName)
                            If ps.Width = 800 And ps.Height = 600 Then
                                PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = ps
                                PrintDocument1.DefaultPageSettings.PaperSize = ps
                                PpSzSTS = True
                                Exit For
                            End If
                        Next

                        If PpSzSTS = False Then
                            For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
                                If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.GermanStandardFanfold Then
                                    ps = PrintDocument1.PrinterSettings.PaperSizes(I)
                                    PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = ps
                                    PrintDocument1.DefaultPageSettings.PaperSize = ps
                                    PpSzSTS = True
                                    Exit For
                                End If
                            Next

                            If PpSzSTS = False Then
                                For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
                                    If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
                                        ps = PrintDocument1.PrinterSettings.PaperSizes(I)
                                        PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = ps
                                        PrintDocument1.DefaultPageSettings.PaperSize = ps
                                        Exit For
                                    End If
                                Next
                            End If

                        End If

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
        Dim da3 As New SqlClient.SqlDataAdapter
        Dim NewCode As String

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        prn_HdDt.Clear()
        prn_DetDt.Clear()

        prn_HedDetDt.Clear()
        prn_DetIndx = 0
        prn_DetSNo = 0
        prn_PageNo = 0
        '  prn_Count = 0

        Try

            da1 = New SqlClient.SqlDataAdapter("select a.* from Processed_Fabric_inspection_Head a INNER JOIN Company_Head b ON a.Company_IdNo = b.Company_IdNo  where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Processed_Fabric_inspection_Code = '" & Trim(NewCode) & "'", con)
            prn_HdDt = New DataTable
            da1.Fill(prn_HdDt)

            If prn_HdDt.Rows.Count > 0 Then

                da2 = New SqlClient.SqlDataAdapter("select a.*, C.Cloth_Name as Fp_Item_Name,d.Colour_Name,e.Lot_No,f.Process_Name from Processed_Fabric_inspection_Receipt_Details a  INNER JOIN Cloth_Head C ON c.Cloth_Idno = a.Item_Idno LEFT OUTER JOIN Colour_Head d ON d.Colour_IdNo = a.Colour_IdNo LEFT OUTER JOIN Lot_Head e ON e.Lot_IdNo = a.Lot_IdNo LEFT OUTER JOIN Process_Head f ON f.Process_IdNo = a.Processing_Idno where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Processed_Fabric_inspection_Code = '" & Trim(NewCode) & "' Order by a.Sl_No", con)
                prn_DetDt = New DataTable
                da2.Fill(prn_DetDt)

                da3 = New SqlClient.SqlDataAdapter("select a.* from Processed_Fabric_inspection_Details a  where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Processed_Fabric_inspection_Code = '" & Trim(NewCode) & "' Order by a.Sl_No", con)
                prn_HedDetDt = New DataTable
                da3.Fill(prn_HedDetDt)

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

        'For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
        '    ps = PrintDocument1.PrinterSettings.PaperSizes(I)
        '    If ps.Width = 800 And ps.Height = 600 Then
        '        PrintDocument1.DefaultPageSettings.PaperSize = ps
        '        e.PageSettings.PaperSize = ps
        '        PpSzSTS = True
        '        Exit For
        '    End If
        'Next

        'If PpSzSTS = False Then

        '    For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
        '        If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.GermanStandardFanfold Then
        '            ps = PrintDocument1.PrinterSettings.PaperSizes(I)
        '            PrintDocument1.DefaultPageSettings.PaperSize = ps
        '            e.PageSettings.PaperSize = ps
        '            PpSzSTS = True
        '            Exit For
        '        End If
        '    Next

        'If PpSzSTS = False Then
        For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
            If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
                ps = PrintDocument1.PrinterSettings.PaperSizes(I)
                PrintDocument1.DefaultPageSettings.PaperSize = ps
                e.PageSettings.PaperSize = ps
                Exit For
            End If
        Next
        'End If

        'End If

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

        NoofItems_PerPage = 34

        Erase LnAr
        Erase ClAr

        LnAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        ClAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}

        ClAr(1) = Val(45) : ClAr(2) = 220 : ClAr(3) = 180 : ClAr(4) = 150
        ClAr(5) = PageWidth - (LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4))

        TxtHgt = 18.5

        EntryCode = Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Try
            If prn_HdDt.Rows.Count > 0 Then

                Printing_Format1_PageHeader(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, prn_PageNo, NoofItems_PerPage, CurY, LnAr, ClAr)

                NoofDets = 0

                CurY = CurY - 10

                If prn_HedDetDt.Rows.Count > 0 Then

                    Do While prn_DetIndx <= prn_HedDetDt.Rows.Count - 1

                        If NoofDets >= NoofItems_PerPage Then

                            CurY = CurY + TxtHgt

                            Common_Procedures.Print_To_PrintDocument(e, "Continued...", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + 10, CurY, 0, 0, pFont)

                            NoofDets = NoofDets + 1

                            Printing_Format1_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClAr, NoofDets, False)

                            e.HasMorePages = True
                            Return

                        End If

                        CurY = CurY + TxtHgt
                        Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HedDetDt.Rows(prn_DetIndx).Item("Sl_No").ToString), LMargin + 15, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HedDetDt.Rows(prn_DetIndx).Item("Roll_No").ToString), LMargin + ClAr(1) + 10, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HedDetDt.Rows(prn_DetIndx).Item("Pcs_no").ToString), LMargin + ClAr(1) + ClAr(2) + 10, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HedDetDt.Rows(prn_DetIndx).Item("Meters").ToString), " #######0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) - 10, CurY, 1, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HedDetDt.Rows(prn_DetIndx).Item("Weight").ToString), " #######0.000"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 1, 0, pFont)

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
        Dim p1Font As Font
        Dim Cmp_Name As String, Cmp_Add1 As String, Cmp_Add2 As String
        Dim Cmp_PhNo As String, Cmp_TinNo As String, Cmp_CstNo As String
        Dim strHeight As Single
        Dim C1 As Single
        Dim W1 As Single
        Dim S1 As Single
        Dim W2, S2 As Single

        PageNo = PageNo + 1

        da2 = New SqlClient.SqlDataAdapter("select a.*  from Company_Head a Where a.Company_IdNo = " & Str(Val(1)) & "", con)
        dt2 = New DataTable
        da2.Fill(dt2)

        CurY = TMargin

        If dt2.Rows.Count > NoofItems_PerPage Then
            Common_Procedures.Print_To_PrintDocument(e, "Page : " & Trim(Val(PageNo)), PageWidth - 10, CurY - TxtHgt, 1, 0, pFont)
        End If

        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(1) = CurY

        Cmp_Name = "" : Cmp_Add1 = "" : Cmp_Add2 = ""
        Cmp_PhNo = "" : Cmp_TinNo = "" : Cmp_CstNo = ""

        Cmp_Name = dt2.Rows(0).Item("Company_Name").ToString
        Cmp_Add1 = dt2.Rows(0).Item("Company_Address1").ToString & " " & dt2.Rows(0).Item("Company_Address2").ToString
        Cmp_Add2 = dt2.Rows(0).Item("Company_Address3").ToString & " " & dt2.Rows(0).Item("Company_Address4").ToString
        If Trim(dt2.Rows(0).Item("Company_PhoneNo").ToString) <> "" Then
            Cmp_PhNo = "PHONE NO.:" & dt2.Rows(0).Item("Company_PhoneNo").ToString
        End If
        If Trim(dt2.Rows(0).Item("Company_TinNo").ToString) <> "" Then
            Cmp_TinNo = "TIN NO.: " & dt2.Rows(0).Item("Company_TinNo").ToString
        End If
        If Trim(dt2.Rows(0).Item("Company_CstNo").ToString) <> "" Then
            Cmp_CstNo = "CST NO.: " & dt2.Rows(0).Item("Company_CstNo").ToString
        End If
        dt2.Clear()

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
        CurY = CurY + TxtHgt - 1
        Common_Procedures.Print_To_PrintDocument(e, Cmp_TinNo, LMargin + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, Cmp_CstNo, PageWidth - 10, CurY, 1, 0, pFont)

        CurY = CurY + TxtHgt - 5
        p1Font = New Font("Calibri", 16, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "FABRIC INSPECTION", LMargin, CurY, 2, PrintWidth, p1Font)
        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height

        CurY = CurY + strHeight + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(2) = CurY

        C1 = ClAr(1) + ClAr(2) + ClAr(3)
        W1 = e.Graphics.MeasureString("REF DATE  : ", pFont).Width
        S1 = e.Graphics.MeasureString("TO  :  ", pFont).Width

        CurY = CurY + TxtHgt - 10
        p1Font = New Font("Calibri", 12, FontStyle.Bold)
        '   Common_Procedures.Print_To_PrintDocument(e, "TO  :  " & "M/s." & prn_HdDt.Rows(0).Item("Ledger_MainName").ToString, LMargin + 10, CurY, 0, 0, p1Font)
        Common_Procedures.Print_To_PrintDocument(e, "REF.NO", LMargin + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Processed_Fabric_inspection_No").ToString, LMargin + W1 + 30, CurY, 0, 0, p1Font)

        'CurY = CurY + TxtHgt
        'Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address1").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
        'p1Font = New Font("Calibri", 14, FontStyle.Bold)

        CurY = CurY + TxtHgt
        ' Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address2").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, "DATE", LMargin + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Processed_Fabric_inspection_Date").ToString), "dd-MM-yyyy").ToString, LMargin + W1 + 30, CurY, 0, 0, pFont)

        CurY = CurY + TxtHgt + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(3) = CurY

        W2 = e.Graphics.MeasureString("Order No     : ", pFont).Width
        S2 = e.Graphics.MeasureString("Agent Name  : ", pFont).Width

        CurY = CurY + 10

        Common_Procedures.Print_To_PrintDocument(e, "ITEM NAME", LMargin + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, " :  ", LMargin + S2 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(0).Item("Fp_Item_Name").ToString, LMargin + S2 + 30, CurY, 0, 0, pFont)

        Common_Procedures.Print_To_PrintDocument(e, "COLOUR ", LMargin + C1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, " :  ", LMargin + W2 + C1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(0).Item("Colour_Name").ToString, LMargin + W2 + C1 + 30, CurY, 0, 0, pFont)


        CurY = CurY + TxtHgt + 10

        Common_Procedures.Print_To_PrintDocument(e, "PROCESS", LMargin + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, " :  ", LMargin + S2 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(0).Item("Process_Name").ToString, LMargin + S2 + 30, CurY, 0, 0, pFont)

        Common_Procedures.Print_To_PrintDocument(e, "LOT NO", LMargin + C1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, " :  ", LMargin + W2 + C1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(0).Item("Lot_No").ToString, LMargin + W2 + C1 + 30, CurY, 0, 0, pFont)


        CurY = CurY + TxtHgt + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(4) = CurY

        e.Graphics.DrawLine(Pens.Black, LMargin + C1, CurY, LMargin + C1, LnAr(2))

        CurY = CurY + TxtHgt - 10
        Common_Procedures.Print_To_PrintDocument(e, "SNO", LMargin, CurY, 2, ClAr(1), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "ROLL NO", LMargin + ClAr(1), CurY, 2, ClAr(2), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "PCS NO", LMargin + ClAr(1) + ClAr(2), CurY, 2, ClAr(3), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "METER", LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, 2, ClAr(4), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "WEIGHT", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 2, ClAr(5), pFont)
      
        CurY = CurY + TxtHgt + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(5) = CurY

    End Sub

    Private Sub Printing_Format1_PageFooter(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single, ByVal NoofDets As Integer, ByVal is_LastPage As Boolean)
        Dim i As Integer
        Dim Cmp_Name As String
        Dim p1Font As Font
        Dim W1 As Single


        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt2 As New DataTable

        da2 = New SqlClient.SqlDataAdapter("select a.*  from Company_Head a Where a.Company_IdNo = " & Str(Val(1)) & "", con)
        dt2 = New DataTable
        da2.Fill(dt2)
        For i = NoofDets + 1 To NoofItems_PerPage
            CurY = CurY + TxtHgt
        Next

        W1 = e.Graphics.MeasureString(" Vehicle No :", pFont).Width

        CurY = CurY + TxtHgt
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(6) = CurY

        CurY = CurY + TxtHgt - 10
        If is_LastPage = True Then
            Common_Procedures.Print_To_PrintDocument(e, " TOTAL", LMargin + ClAr(1) + 30, CurY, 2, ClAr(4), pFont)
            Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(0).Item("Total_Pcs").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) - 10, CurY, 1, 0, pFont)

            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Total_Meters").ToString), "#######.0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) - 10, CurY, 1, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Total_Weight").ToString), " #######0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 1, 0, pFont)

            '    .Rows(0).Cells(2).Value = Val(dt1.Rows(0).Item("Total_Pcs").ToString)
            '    .Rows(0).Cells(3).Value = Format(Val(dt1.Rows(0).Item("Total_Meters").ToString), "########0.00")
            '    .Rows(0).Cells(4).Value = Format(Val(dt1.Rows(0).Item("Total_Weight").ToString), "########0.000")

        End If

        CurY = CurY + TxtHgt + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(7) = CurY

        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1), CurY, LMargin + ClAr(1), LnAr(4))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2), CurY, LMargin + ClAr(1) + ClAr(2), LnAr(4))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3), LnAr(4))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), LnAr(4))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(4))
      
        CurY = CurY + 10


        CurY = CurY + TxtHgt
        CurY = CurY + TxtHgt

        Cmp_Name = dt2.Rows(0).Item("Company_Name").ToString
        p1Font = New Font("Calibri", 12, FontStyle.Bold)
        dt2.Clear()

        Common_Procedures.Print_To_PrintDocument(e, "Receiver's Signature", LMargin + 20, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, "For " & Cmp_Name, PageWidth - 15, CurY, 1, 0, p1Font)

        CurY = CurY + TxtHgt + 10

        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

        e.Graphics.DrawLine(Pens.Black, LMargin, LnAr(1), LMargin, CurY)
        e.Graphics.DrawLine(Pens.Black, PageWidth, LnAr(1), PageWidth, CurY)

    End Sub

    Private Sub btn_Print_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Print.Click
        print_record()
    End Sub
End Class
