Public Class JobWork_Weaving_Pavu_Yarn_Delivery
    Implements Interface_MDIActions

    Private con As New SqlClient.SqlConnection(Common_Procedures.Connection_String)
    Private FrmLdSTS As Boolean = False
    Private New_Entry As Boolean = False
    Private Insert_Entry As Boolean = False
    Private Filter_RowNo As Integer = -1
    Private Filter_Status As Boolean = False
    Private Pk_Condition As String = "JWPYD-"
    Private Prec_ActCtrl As New Control
    Private prn_HdDt As New DataTable
    Private prn_DetDt As New DataTable
    Private prn_DetDt1 As New DataTable
    Private prn_PageNo As Integer
    Private prn_HeadIndx As Integer
    Private prn_Prev_HeadIndx As Integer
    Private prn_DetIndx As Integer
    Private prn_DetSNo As Integer
    Private vcbo_KeyDwnVal As Double
    Private prn_FromNo As String
    Private prn_ToNo As String

    Private WithEvents dgtxt_Details As New DataGridViewTextBoxEditingControl

    Private vPrn_PvuEdsCnt As String
    Private vPrn_PvuNPcs As Integer
    Private vPrn_PvuTotBms As Integer
    Private vPrn_PvuTotMtrs As Single
    Private vPrn_PvuSetNo As String
    Private vPrn_PvuBmNos1 As String
    Private vPrn_PvuBmNos2 As String
    Private vPrn_PvuBmNos3 As String
    Private vPrn_PvuBmNos4 As String
    Private prn_Status As Integer
    Private prn_Count As Integer
    Private prn_HdAr(200, 10) As String
    Private prn_DetAr(200, 10) As String
    Private prn_InpOpts As String = ""
    Private prn_OriDupTri As String = ""
    Private prn_DetMxIndx As Integer = 0
    Private prn_NoofBmDets As Integer = 0

    Private SaveAll_STS As Boolean = False
    Private LastNo As String = ""
    Public vmskOldText As String = ""
    Public vmskSelStrt As Integer = -1

    Private Sub clear()
        New_Entry = False
        Insert_Entry = False
        chk_SelectAll.Checked = False

        pnl_Back.Enabled = True
        pnl_Filter.Visible = False
        pnl_Selection.Visible = False
        pnl_Print.Visible = False
        pnl_PrintRange.Visible = False

        lbl_DcNo.Text = ""
        lbl_DcNo.ForeColor = Color.Black
        vmskOldText = ""
        vmskSelStrt = -1
        msk_Date.Text = ""
        dtp_Date.Text = ""
        txt_KuraiPavuBeam.Text = ""
        txt_PartyDcNo.Text = ""
        txt_JJFormNo.Text = ""
        txt_KuraiPavuMeters.Text = ""
        txt_NoOfBobin.Text = ""
        cbo_DeliveryTo.Text = ""
        cbo_DeliveryTo.Tag = ""
        cbo_EndsCount.Text = ""

        cbo_EndsCount.Text = ""
        cbo_Transport.Text = ""
        cbo_VehicleNo.Text = ""
        cbo_JobworkerName.Text = ""
        cbo_JobworkerName.Tag = ""

        txt_Freight.Text = ""
        txt_Note.Text = ""

        dgv_PavuDetails.Rows.Clear()

        dgv_PavuDetails_Total.Rows.Clear()
        dgv_PavuDetails_Total.Rows.Add()

        dgv_YarnDetails.Rows.Clear()
        dgv_YarnDetails.Rows(0).Cells(2).Value = "MILL"

        dgv_YarnDetails_Total.Rows.Clear()
        dgv_YarnDetails_Total.Rows.Add()

        cbo_DeliveryTo.Enabled = True
        cbo_DeliveryTo.BackColor = Color.White

        cbo_JobworkerName.Enabled = True
        cbo_JobworkerName.BackColor = Color.White

        cbo_EndsCount.Enabled = True
        cbo_EndsCount.BackColor = Color.White

        cbo_Grid_CountName.Enabled = True
        cbo_Grid_CountName.BackColor = Color.White

        If Filter_Status = False Then
            dtp_Filter_Fromdate.Text = ""
            dtp_Filter_ToDate.Text = ""
            cbo_Filter_DeliveryName.Text = ""
            cbo_Filter_CountName.Text = ""
            cbo_Filter_MillName.Text = ""

            cbo_Filter_DeliveryName.SelectedIndex = -1
            cbo_Filter_CountName.SelectedIndex = -1
            cbo_Filter_MillName.SelectedIndex = -1
            dgv_Filter_Details.Rows.Clear()
        End If

        cbo_Grid_CountName.Visible = False
        cbo_Grid_MillName.Visible = False
        cbo_Grid_YarnType.Visible = False

        cbo_Grid_CountName.Tag = -1
        cbo_Grid_MillName.Tag = -1
        cbo_Grid_YarnType.Tag = -1

        cbo_Grid_CountName.Text = ""
        cbo_Grid_MillName.Text = ""
        cbo_Grid_YarnType.Text = ""

    End Sub

    Private Sub Grid_DeSelect()
        On Error Resume Next
        If Not IsNothing(dgv_YarnDetails.CurrentCell) Then dgv_YarnDetails.CurrentCell.Selected = False
        If Not IsNothing(dgv_PavuDetails.CurrentCell) Then dgv_PavuDetails.CurrentCell.Selected = False
    End Sub

    Private Sub ControlGotFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim txtbx As TextBox
        Dim combobx As ComboBox
        Dim mskdtxbx As MaskedTextBox
        On Error Resume Next

        If TypeOf Me.ActiveControl Is TextBox Or TypeOf Me.ActiveControl Is ComboBox Or TypeOf Me.ActiveControl Is Button Or TypeOf Me.ActiveControl Is CheckBox Or TypeOf Me.ActiveControl Is MaskedTextBox Then
            Me.ActiveControl.BackColor = Color.Lime
            Me.ActiveControl.ForeColor = Color.Blue
        End If

        If TypeOf Me.ActiveControl Is TextBox Then
            txtbx = Me.ActiveControl
            txtbx.SelectAll()
        ElseIf TypeOf Me.ActiveControl Is MaskedTextBox Then
            mskdtxbx = Me.ActiveControl
            mskdtxbx.SelectionStart = 0
        ElseIf TypeOf Me.ActiveControl Is ComboBox Then
            combobx = Me.ActiveControl
            combobx.SelectAll()
        End If

        If Me.ActiveControl.Name <> cbo_Grid_CountName.Name Then
            cbo_Grid_CountName.Visible = False
        End If
        If Me.ActiveControl.Name <> cbo_Grid_MillName.Name Then
            cbo_Grid_MillName.Visible = False
        End If
        If Me.ActiveControl.Name <> cbo_Grid_YarnType.Name Then
            cbo_Grid_YarnType.Visible = False
        End If

        If Me.ActiveControl.Name <> dgv_YarnDetails.Name Then
            Common_Procedures.Hide_CurrentStock_Display()
        End If
        Grid_DeSelect()
        Prec_ActCtrl = Me.ActiveControl

    End Sub

    Private Sub ControlLostFocus(ByVal sender As Object, ByVal e As System.EventArgs)

        On Error Resume Next

        If IsNothing(Prec_ActCtrl) = False Then
            If TypeOf Prec_ActCtrl Is TextBox Or TypeOf Prec_ActCtrl Is ComboBox Or TypeOf Prec_ActCtrl Is MaskedTextBox Then
                Prec_ActCtrl.BackColor = Color.White
                Prec_ActCtrl.ForeColor = Color.Black
            ElseIf TypeOf Prec_ActCtrl Is Button Then
                Prec_ActCtrl.BackColor = Color.FromArgb(41, 57, 85)
                Prec_ActCtrl.ForeColor = Color.White
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

            da1 = New SqlClient.SqlDataAdapter("select a.*, b.Ledger_Name as DelvName, c.Ledger_Name as TransportName, d.EndsCount_Name,e.Ledger_Name as RecFromName, f.Ledger_Name as Yarn_ReceivedFrom_Name from JobWork_Weaving_PavuYarn_Delivery_Head a INNER JOIN Ledger_Head b ON a.DeliveryTo_Idno = b.Ledger_IdNo LEFT OUTER JOIN Ledger_Head c ON a.Transport_IdNo = c.Ledger_IdNo LEFT OUTER JOIN EndsCount_Head d ON a.EndsCount_IdNo = d.EndsCount_IdNo LEFT OUTER JOIN Ledger_Head e ON a.ReceivedFrom_Idno = e.Ledger_IdNo LEFT OUTER JOIN Ledger_Head f ON a.Yarn_ReceivedFrom_IdNo = f.Ledger_IdNo Where a.JobWork_Weaving_PavuYarn_Delivery_Code = '" & Trim(NewCode) & "'", con)
            da1.Fill(dt1)

            If dt1.Rows.Count > 0 Then

                lbl_DcNo.Text = dt1.Rows(0).Item("JobWork_Weaving_PavuYarn_Delivery_No").ToString
                dtp_Date.Text = dt1.Rows(0).Item("JobWork_Weaving_PavuYarn_Delivery_Date").ToString
                msk_Date.Text = dtp_Date.Text

                cbo_DeliveryTo.Text = dt1.Rows(0).Item("DelvName").ToString
                txt_KuraiPavuBeam.Text = dt1.Rows(0).Item("Empty_Beam").ToString
                txt_NoOfBobin.Text = dt1.Rows(0).Item("Empty_Bobin").ToString
                txt_KuraiPavuMeters.Text = Val(dt1.Rows(0).Item("Meters").ToString)
                cbo_EndsCount.Text = dt1.Rows(0).Item("EndsCount_Name").ToString
                cbo_VehicleNo.Text = dt1.Rows(0).Item("Vehicle_No").ToString
                cbo_Transport.Text = dt1.Rows(0).Item("TransportName").ToString
                cbo_JobworkerName.Text = dt1.Rows(0).Item("RecFromName").ToString
                txt_PartyDcNo.Text = dt1.Rows(0).Item("Party_DcNo").ToString
                txt_JJFormNo.Text = dt1.Rows(0).Item("JJ_FormNo").ToString
                txt_Freight.Text = Val(dt1.Rows(0).Item("Freight_Charge").ToString)
                txt_Note.Text = dt1.Rows(0).Item("Note").ToString

                LockSTS = False
                If IsDBNull(dt1.Rows(0).Item("PavuGate_Pass_Code").ToString) = False Then
                    If Trim(dt1.Rows(0).Item("PavuGate_Pass_Code").ToString) <> "" Then
                        LockSTS = True
                    End If
                End If
                If IsDBNull(dt1.Rows(0).Item("YarnGate_Pass_Code").ToString) = False Then
                    If Trim(dt1.Rows(0).Item("YarnGate_Pass_Code").ToString) <> "" Then
                        LockSTS = True
                    End If
                End If

                da2 = New SqlClient.SqlDataAdapter("Select a.*, b.Pavu_Delivery_Increment, c.EndsCount_Name, d.Beam_Width_Name from JobWork_Weaving_Pavu_Delivery_Details a INNER JOIN Stock_SizedPavu_Processing_Details b ON a.Set_Code = b.Set_Code and a.Beam_No = b.Beam_No INNER JOIN EndsCount_Head c ON a.EndsCount_IdNo = c.EndsCount_IdNo LEFT OUTER JOIN Beam_Width_Head d ON a.Beam_Width_Idno = d.Beam_Width_Idno where a.JobWork_Weaving_PavuYarn_Delivery_Code = '" & Trim(NewCode) & "' Order by a.Sl_No", con)
                dt2 = New DataTable
                da2.Fill(dt2)

                dgv_PavuDetails.Rows.Clear()
                SNo = 0

                If dt2.Rows.Count > 0 Then

                    For i = 0 To dt2.Rows.Count - 1

                        n = dgv_PavuDetails.Rows.Add()

                        SNo = SNo + 1
                        dgv_PavuDetails.Rows(n).Cells(0).Value = Val(SNo)
                        dgv_PavuDetails.Rows(n).Cells(1).Value = dt2.Rows(i).Item("Set_No").ToString
                        dgv_PavuDetails.Rows(n).Cells(2).Value = dt2.Rows(i).Item("Beam_No").ToString
                        dgv_PavuDetails.Rows(n).Cells(3).Value = dt2.Rows(i).Item("Pcs").ToString
                        If Common_Procedures.settings.Weaver_Zari_Kuri_Entries_Status = 1 Then

                            dgv_PavuDetails.Rows(n).Cells(4).Value = Format(Val(dt2.Rows(i).Item("Meters").ToString), "########0.000")

                        Else
                            dgv_PavuDetails.Rows(n).Cells(4).Value = Format(Val(dt2.Rows(i).Item("Meters").ToString), "########0.00")

                        End If

                        dgv_PavuDetails.Rows(n).Cells(5).Value = dt2.Rows(i).Item("EndsCount_Name").ToString
                        dgv_PavuDetails.Rows(n).Cells(6).Value = dt2.Rows(i).Item("Beam_Width_Name").ToString
                        dgv_PavuDetails.Rows(n).Cells(7).Value = ""
                        dgv_PavuDetails.Rows(n).Cells(8).Value = dt2.Rows(i).Item("Noof_Used").ToString
                        dgv_PavuDetails.Rows(n).Cells(9).Value = dt2.Rows(i).Item("set_code").ToString
                        dgv_PavuDetails.Rows(n).Cells(10).Value = dt2.Rows(i).Item("Pavu_Delivery_Increment").ToString

                        If Val(dgv_PavuDetails.Rows(n).Cells(8).Value) > 0 And Val(dgv_PavuDetails.Rows(n).Cells(8).Value) <> Val(dgv_PavuDetails.Rows(n).Cells(10).Value) Then
                            dgv_PavuDetails.Rows(n).Cells(7).Value = "1"
                        End If

                    Next i

                End If

                With dgv_PavuDetails_Total
                    If .RowCount = 0 Then .Rows.Add()
                    .Rows(0).Cells(2).Value = Val(dt1.Rows(0).Item("Total_Beam").ToString)
                    If Common_Procedures.settings.Weaver_Zari_Kuri_Entries_Status = 1 Then

                        .Rows(0).Cells(4).Value = Format(Val(dt1.Rows(0).Item("Total_Meters").ToString), "########0.000")
                    Else
                        .Rows(0).Cells(4).Value = Format(Val(dt1.Rows(0).Item("Total_Meters").ToString), "########0.00")
                    End If

                End With

                dt2.Clear()

                da2 = New SqlClient.SqlDataAdapter("select a.*, b.Count_Name, c.Mill_Name from JobWork_Weaving_Yarn_Delivery_Details a INNER JOIN Count_Head b on a.Count_IdNo = b.Count_IdNo INNER JOIN Mill_Head c on a.Mill_IdNo = c.Mill_IdNo where a.JobWork_Weaving_PavuYarn_Delivery_Code = '" & Trim(NewCode) & "' Order by a.sl_no", con)
                dt2 = New DataTable
                da2.Fill(dt2)

                dgv_YarnDetails.Rows.Clear()
                SNo = 0

                If dt2.Rows.Count > 0 Then

                    For i = 0 To dt2.Rows.Count - 1

                        n = dgv_YarnDetails.Rows.Add()

                        SNo = SNo + 1
                        dgv_YarnDetails.Rows(n).Cells(0).Value = Val(SNo)

                        dgv_YarnDetails.Rows(n).Cells(1).Value = dt2.Rows(i).Item("Count_Name").ToString
                        dgv_YarnDetails.Rows(n).Cells(2).Value = dt2.Rows(i).Item("Yarn_Type").ToString
                        dgv_YarnDetails.Rows(n).Cells(3).Value = dt2.Rows(i).Item("Mill_Name").ToString
                        dgv_YarnDetails.Rows(n).Cells(4).Value = Val(dt2.Rows(i).Item("Bags").ToString)
                        dgv_YarnDetails.Rows(n).Cells(5).Value = Val(dt2.Rows(i).Item("Cones").ToString)
                        dgv_YarnDetails.Rows(n).Cells(6).Value = Format(Val(dt2.Rows(i).Item("Weight").ToString), "########0.000")
                        dgv_YarnDetails.Rows(n).Cells(7).Value = Format(Val(dt2.Rows(i).Item("Thiri").ToString), "########0.000")

                    Next i

                End If

                With dgv_YarnDetails_Total
                    If .RowCount = 0 Then .Rows.Add()
                    .Rows(0).Cells(4).Value = Val(dt1.Rows(0).Item("Total_Bags").ToString)
                    .Rows(0).Cells(5).Value = Val(dt1.Rows(0).Item("Total_Cones").ToString)
                    .Rows(0).Cells(6).Value = Format(Val(dt1.Rows(0).Item("Total_Weight").ToString), "########0.000")
                End With

                dt2.Clear()

                dt2.Dispose()
                da2.Dispose()

            End If

            dt1.Clear()
            dt1.Dispose()
            da1.Dispose()

            If LockSTS = True Then
                cbo_DeliveryTo.Enabled = False
                cbo_DeliveryTo.BackColor = Color.LightGray

                cbo_JobworkerName.Enabled = False
                cbo_JobworkerName.BackColor = Color.LightGray

                cbo_EndsCount.Enabled = False
                cbo_EndsCount.BackColor = Color.LightGray

                cbo_Grid_CountName.Enabled = False
                cbo_Grid_CountName.BackColor = Color.LightGray


                msk_Date.Enabled = False
                msk_Date.BackColor = Color.LightGray

                dtp_Date.Enabled = False
                dtp_Date.BackColor = Color.LightGray

                dgv_YarnDetails.AllowUserToAddRows = False
                dgv_PavuDetails.AllowUserToAddRows = False

                btn_Selection.Enabled = False

            End If

            Grid_Cell_DeSelect()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT MOVE RECORDS...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        If msk_Date.Visible And msk_Date.Enabled Then msk_Date.Focus()


    End Sub

    Private Sub Grid_Cell_DeSelect()
        On Error Resume Next
        If Not IsNothing(dgv_PavuDetails.CurrentCell) Then dgv_PavuDetails.CurrentCell.Selected = False
        If Not IsNothing(dgv_YarnDetails.CurrentCell) Then dgv_YarnDetails.CurrentCell.Selected = False
    End Sub

    Private Sub JobWork_JobWork_Weaving_PavuYarn_Receipt_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated
        Dim da As SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim NoofComps As Integer
        Dim CompCondt As String

        Try

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_DeliveryTo.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "LEDGER" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_DeliveryTo.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            'If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_VehicleNo.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "CLOTH" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
            '    cbo_VehicleNo.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            'End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_EndsCount.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "ENDSCOUNT" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_EndsCount.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Transport.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "LEDGER" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Transport.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Grid_CountName.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "COUNT" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Grid_CountName.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Grid_MillName.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "MILL" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Grid_MillName.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_JobworkerName.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "LEDGER" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_JobworkerName.Text = Trim(Common_Procedures.Master_Return.Return_Value)
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

    Private Sub JobWork_JobWork_Weaving_PavuYarn_Receipt_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
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

        Me.Text = ""

        con.Open()

        If Common_Procedures.settings.Weaver_Zari_Kuri_Entries_Status = 1 Then

            dgv_PavuDetails.Columns(4).HeaderText = "MTR Or WGT"
            lbl_Bobin.Visible = True
            txt_NoOfBobin.Visible = True
        End If

        If Common_Procedures.settings.Weaver_Zari_Kuri_Entries_Status = 1 Then

            dgv_Selection.Columns(4).HeaderText = "MTR Or WGT"
        End If

        cbo_Grid_CountName.Visible = False
        cbo_Grid_MillName.Visible = False
        cbo_Grid_YarnType.Visible = False
        msk_Date.Text = ""
        dtp_Date.Text = ""
        txt_KuraiPavuBeam.Text = ""
        cbo_DeliveryTo.Text = ""
        cbo_DeliveryTo.Tag = ""
        cbo_EndsCount.Text = ""

        cbo_EndsCount.Text = ""
        cbo_VehicleNo.Text = ""

        dgv_YarnDetails.Columns(7).Visible = False
        dgv_YarnDetails_Total.Columns(7).Visible = False
        If Val(Common_Procedures.settings.Weaver_YarnStock_InThiri_Status) = 1 Then
            dgv_YarnDetails.Columns(7).Visible = True
            dgv_YarnDetails_Total.Columns(7).Visible = True

        Else

            dgv_YarnDetails.Columns(1).Width = dgv_YarnDetails.Columns(1).Width + 25
            dgv_YarnDetails.Columns(3).Width = dgv_YarnDetails.Columns(3).Width + 50
            dgv_YarnDetails.Columns(6).Width = dgv_YarnDetails.Columns(6).Width + 15

            dgv_YarnDetails_Total.Columns(1).Width = dgv_YarnDetails_Total.Columns(1).Width + 25
            dgv_YarnDetails_Total.Columns(3).Width = dgv_YarnDetails_Total.Columns(3).Width + 50
            dgv_YarnDetails_Total.Columns(6).Width = dgv_YarnDetails_Total.Columns(6).Width + 15

        End If

        AddHandler msk_Date.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_EndsCount.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_DeliveryTo.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_VehicleNo.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Grid_MillName.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Grid_CountName.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Grid_YarnType.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_JobworkerName.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Transport.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_BeamNoSelection.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_PartyDcNo.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_JJFormNo.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_NoOfBobin.GotFocus, AddressOf ControlGotFocus



        AddHandler txt_KuraiPavuBeam.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Freight.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_KuraiPavuMeters.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Note.GotFocus, AddressOf ControlGotFocus

        AddHandler cbo_Grid_YarnType.GotFocus, AddressOf ControlGotFocus

        AddHandler dtp_Filter_Fromdate.GotFocus, AddressOf ControlGotFocus
        AddHandler dtp_Filter_ToDate.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Filter_MillName.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Filter_EndsCount.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Filter_CountName.GotFocus, AddressOf ControlGotFocus

        AddHandler btn_save.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_close.GotFocus, AddressOf ControlGotFocus

        AddHandler btn_Print.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_Print_Bale.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_Print_Invoice.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_Print_Cancel.GotFocus, AddressOf ControlGotFocus


        AddHandler txt_PrintRange_FromNo.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_PrintRange_ToNo.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_Print_PrintRange.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_Cancel_PrintRange.GotFocus, AddressOf ControlGotFocus


        AddHandler msk_Date.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Grid_CountName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_DeliveryTo.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_VehicleNo.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Grid_MillName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Grid_YarnType.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_EndsCount.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_KuraiPavuBeam.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Freight.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_KuraiPavuMeters.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Note.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_JobworkerName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Transport.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_BeamNoSelection.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_PartyDcNo.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_JJFormNo.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_NoOfBobin.LostFocus, AddressOf ControlLostFocus


        AddHandler dtp_Filter_Fromdate.LostFocus, AddressOf ControlLostFocus
        AddHandler dtp_Filter_ToDate.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Filter_CountName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Filter_EndsCount.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Filter_MillName.LostFocus, AddressOf ControlLostFocus


        AddHandler btn_Print.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_Print_Bale.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_Print_Invoice.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_Print_Cancel.LostFocus, AddressOf ControlLostFocus

        AddHandler txt_PrintRange_FromNo.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_PrintRange_ToNo.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_Print_PrintRange.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_Cancel_PrintRange.LostFocus, AddressOf ControlLostFocus

        AddHandler txt_KuraiPavuBeam.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_KuraiPavuMeters.KeyDown, AddressOf TextBoxControlKeyDown

        AddHandler msk_Date.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_PartyDcNo.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_PrintRange_FromNo.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_JJFormNo.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_NoOfBobin.KeyDown, AddressOf TextBoxControlKeyDown


        AddHandler msk_Date.KeyPress, AddressOf TextBoxControlKeyPress

        AddHandler txt_KuraiPavuBeam.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_KuraiPavuMeters.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_PartyDcNo.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_PrintRange_FromNo.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_JJFormNo.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_NoOfBobin.KeyPress, AddressOf TextBoxControlKeyPress


        pnl_Filter.Visible = False
        pnl_Filter.Left = (Me.Width - pnl_Filter.Width) \ 2
        pnl_Filter.Top = (Me.Height - pnl_Filter.Height) \ 2

        pnl_Selection.Visible = False
        pnl_Selection.Left = (Me.Width - pnl_Selection.Width) \ 2
        pnl_Selection.Top = (Me.Height - pnl_Selection.Height) \ 2

        pnl_Print.Visible = False
        pnl_Print.Left = (Me.Width - pnl_Print.Width) \ 2
        pnl_Print.Top = (Me.Height - pnl_Print.Height) \ 2
        pnl_Print.BringToFront()

        pnl_PrintRange.Visible = False
        pnl_PrintRange.Left = (Me.Width - pnl_PrintRange.Width) \ 2
        pnl_PrintRange.Top = (Me.Height - pnl_PrintRange.Height) \ 2
        pnl_PrintRange.BringToFront()

        lbl_Company.Text = ""
        lbl_Company.Tag = 0
        lbl_Company.Visible = False
        Common_Procedures.CompIdNo = 0

        'Dgv_Details Columns- 40,150,100,325,85,95,115
        'Dgv_Details Columns- 40,110,100,280,85,95,100,100 After Add Thiri

        Filter_Status = False
        FrmLdSTS = True
        new_record()

    End Sub

    Private Sub JobWork_Weaving_PavuYarn_Delivery_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        On Error Resume Next
        con.Close()
        con.Dispose()
        Common_Procedures.Last_Closed_FormName = Me.Name
    End Sub

    Private Sub JobWork_Weaving_PavuYarn_Delivery_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress

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

                ElseIf pnl_PrintRange.Visible = True Then
                    btn_Close_PrintRange_Click(sender, e)
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

    Protected Overrides Function ProcessCmdKey(ByRef msg As System.Windows.Forms.Message, ByVal keyData As System.Windows.Forms.Keys) As Boolean
        Dim dgv1 As New DataGridView
        Dim LCol As Integer = 0

        On Error Resume Next

        If ActiveControl.Name = dgv_YarnDetails.Name Or TypeOf ActiveControl Is DataGridViewTextBoxEditingControl Then

            dgv1 = Nothing

            If ActiveControl.Name = dgv_YarnDetails.Name Then
                dgv1 = dgv_YarnDetails

            ElseIf dgv_YarnDetails.IsCurrentRowDirty = True Then

                dgv1 = dgv_YarnDetails

            ElseIf pnl_Back.Enabled = True Then
                dgv1 = dgv_YarnDetails

            End If

            If IsNothing(dgv1) = False Then

                With dgv1

                    LCol = 7
                    If dgv_YarnDetails.Columns(7).Visible = False Then LCol = 6

                    If keyData = Keys.Enter Or keyData = Keys.Down Then

                        If .CurrentCell.ColumnIndex >= LCol Then
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
                                txt_Freight.Focus()

                            Else
                                .CurrentCell = .Rows(.CurrentCell.RowIndex - 1).Cells(LCol)

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

    Public Sub delete_record() Implements Interface_MDIActions.delete_record
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim cmd As New SqlClient.SqlCommand
        Dim trans As SqlClient.SqlTransaction
        Dim NewCode As String = ""
        Dim Qa As Windows.Forms.DialogResult
        Dim Nr As Integer = 0

        ' If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.JobWork_Weaving_PavuYarn_Delivery_Entry, "~L~") = 0 And InStr(Common_Procedures.UR.JobWork_Weaving_PavuYarn_Delivery_Entry, "~D~") = 0 Then MessageBox.Show("You have No Rights to Delete", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error) : Exit Sub

        Qa = MessageBox.Show("Do you want to Delete?", "FOR DELETION...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2)
        If Qa = Windows.Forms.DialogResult.No Or Qa = Windows.Forms.DialogResult.Cancel Then
            Exit Sub
        End If

        If New_Entry = True Then
            MessageBox.Show("This is New Entry", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If


        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Da = New SqlClient.SqlDataAdapter("select * from JobWork_Weaving_PavuYarn_Delivery_Head Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and JobWork_Weaving_PavuYarn_Delivery_Code = '" & Trim(NewCode) & "'", con)
        Dt1 = New DataTable
        Da.Fill(Dt1)

        If Dt1.Rows.Count > 0 Then

            If IsDBNull(Dt1.Rows(0).Item("PAVUGate_Pass_Code").ToString) = False Then
                If Trim(Dt1.Rows(0).Item("PAVUGate_Pass_Code").ToString) <> "" Then
                    MessageBox.Show("Already Piece Delivered", "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                    Exit Sub
                End If
            End If
            If IsDBNull(Dt1.Rows(0).Item("YarnGate_Pass_Code").ToString) = False Then
                If Trim(Dt1.Rows(0).Item("YarnGate_Pass_Code").ToString) <> "" Then
                    MessageBox.Show("Already Piece Delivered", "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                    Exit Sub
                End If
            End If
        End If
        Dt1.Clear()
        Dt1.Dispose()
        Da.Dispose()

        Da = New SqlClient.SqlDataAdapter("select count(*) from Stock_SizedPavu_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' and  ( Pavu_Delivery_Code <> '' or Pavu_Delivery_Increment <> 0)", con)
        Dt1 = New DataTable
        Da.Fill(Dt1)
        If Dt1.Rows.Count > 0 Then
            If IsDBNull(Dt1.Rows(0)(0).ToString) = False Then
                If Val(Dt1.Rows(0)(0).ToString) > 0 Then
                    MessageBox.Show("Already some Pavu Delivered", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Exit Sub
                End If
            End If
        End If
        Dt1.Clear()


        trans = con.BeginTransaction

        Try

            NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)



            cmd.Connection = con
            cmd.Transaction = trans

            Common_Procedures.Voucher_Deletion(con, Val(lbl_Company.Tag), Trim(Pk_Condition) & Trim(NewCode), trans)

            Da = New SqlClient.SqlDataAdapter("Select * from JobWork_Weaving_Pavu_Delivery_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and JobWork_Weaving_PavuYarn_Delivery_Code = '" & Trim(NewCode) & "'", con)
            Da.SelectCommand.Transaction = trans
            Dt1 = New DataTable
            Da.Fill(Dt1)

            If Dt1.Rows.Count > 0 Then
                For i = 0 To Dt1.Rows.Count - 1

                    Nr = 0
                    cmd.CommandText = "update Stock_SizedPavu_Processing_Details set " _
                              & " StockAt_IdNo = " & Str(Val(Dt1.Rows(i).Item("ReceivedFrom_IdNo").ToString)) & ", " _
                              & " Pavu_Delivery_Increment = Pavu_Delivery_Increment - 1 " _
                              & " Where " _
                              & " StockAt_IdNo = " & Str(Val(Dt1.Rows(i).Item("DeliveryTo_IdNo").ToString)) & " and " _
                              & " Set_Code = '" & Trim(Dt1.Rows(i).Item("Set_Code").ToString) & "' and " _
                              & " beam_no = '" & Trim(Dt1.Rows(i).Item("Beam_No").ToString) & "' and " _
                              & " Pavu_Delivery_Increment = " & Str(Val(Dt1.Rows(i).Item("Noof_Used").ToString))
                    Nr = cmd.ExecuteNonQuery

                    If Nr = 0 Then
                        Throw New ApplicationException("Some Pavu Delivered to Others")
                        Exit Sub
                    End If

                Next
            End If
            Dt1.Clear()

            cmd.CommandText = "Delete from Stock_Pavu_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Stock_Yarn_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Stock_Empty_BeamBagCone_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "delete from JobWork_Weaving_Pavu_Delivery_Details where company_idno = " & Str(Val(lbl_Company.Tag)) & " and JobWork_Weaving_PavuYarn_Delivery_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "delete from JobWork_Weaving_Yarn_Delivery_Details where company_idno = " & Str(Val(lbl_Company.Tag)) & " and JobWork_Weaving_PavuYarn_Delivery_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "delete from JobWork_Weaving_PavuYarn_Delivery_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and JobWork_Weaving_PavuYarn_Delivery_Code = '" & Trim(NewCode) & "'"
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

        If msk_Date.Enabled = True And msk_Date.Visible = True Then msk_Date.Focus()

    End Sub

    Public Sub filter_record() Implements Interface_MDIActions.filter_record

        If Filter_Status = False Then

            Dim da As New SqlClient.SqlDataAdapter
            Dim dt1 As New DataTable
            Dim dt2 As New DataTable
            Dim dt3 As New DataTable

            da = New SqlClient.SqlDataAdapter("select Ledger_DisplayName from Ledger_AlaisHead where (Ledger_IdNo = 0 or AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14 ) order by Ledger_DisplayName", con)
            da.Fill(dt1)
            cbo_Filter_DeliveryName.DataSource = dt1
            cbo_Filter_DeliveryName.DisplayMember = "Ledger_DisplayName"


            da = New SqlClient.SqlDataAdapter("select count_name from count_head order by count_name", con)
            da.Fill(dt2)
            cbo_Filter_CountName.DataSource = dt2
            cbo_Filter_CountName.DisplayMember = "count_name"

            da = New SqlClient.SqlDataAdapter("select distinct(EndsCount_Name) from EndsCount_Head order by EndsCount_Name", con)
            da.Fill(dt3)
            cbo_Filter_EndsCount.DataSource = dt3
            cbo_Filter_EndsCount.DisplayMember = "EndsCount_Name"

            dtp_Filter_Fromdate.Text = ""
            dtp_Filter_ToDate.Text = ""
            cbo_Filter_DeliveryName.Text = ""
            cbo_Filter_CountName.Text = ""
            cbo_Filter_EndsCount.Text = ""
            cbo_Filter_MillName.Text = ""
            cbo_Filter_DeliveryName.SelectedIndex = -1
            cbo_Filter_CountName.SelectedIndex = -1
            cbo_Filter_EndsCount.SelectedIndex = -1
            cbo_Filter_MillName.SelectedIndex = -1
            dgv_Filter_Details.Rows.Clear()

        End If

        pnl_Filter.Visible = True
        pnl_Filter.Enabled = True
        pnl_Filter.BringToFront()
        pnl_Back.Enabled = False
        If Filter_Status = True Then
            If dgv_Filter_Details.Rows.Count > 0 And Filter_RowNo >= 0 Then
                dgv_Filter_Details.Focus()
                dgv_Filter_Details.CurrentCell = dgv_Filter_Details.Rows(Filter_RowNo).Cells(0)
                dgv_Filter_Details.CurrentCell.Selected = True
            Else
                If dtp_Filter_Fromdate.Enabled And dtp_Filter_Fromdate.Visible Then dtp_Filter_Fromdate.Focus()

            End If

        Else
            If dtp_Filter_Fromdate.Enabled And dtp_Filter_Fromdate.Visible Then dtp_Filter_Fromdate.Focus()

        End If
    End Sub

    Public Sub movefirst_record() Implements Interface_MDIActions.movefirst_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim movno As String

        Try

            da = New SqlClient.SqlDataAdapter("select top 1 JobWork_Weaving_PavuYarn_Delivery_No from JobWork_Weaving_PavuYarn_Delivery_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and JobWork_Weaving_PavuYarn_Delivery_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby, JobWork_Weaving_PavuYarn_Delivery_No", con)
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

            da = New SqlClient.SqlDataAdapter("select top 1 JobWork_Weaving_PavuYarn_Delivery_No from JobWork_Weaving_PavuYarn_Delivery_Head where for_orderby > " & Str(Val(OrdByNo)) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and JobWork_Weaving_PavuYarn_Delivery_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby, JobWork_Weaving_PavuYarn_Delivery_No", con)
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

            da = New SqlClient.SqlDataAdapter("select top 1 JobWork_Weaving_PavuYarn_Delivery_No from JobWork_Weaving_PavuYarn_Delivery_Head where for_orderby < " & Str(Val(OrdByNo)) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and JobWork_Weaving_PavuYarn_Delivery_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, JobWork_Weaving_PavuYarn_Delivery_No desc", con)
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
            da = New SqlClient.SqlDataAdapter("select top 1 JobWork_Weaving_PavuYarn_Delivery_No from JobWork_Weaving_PavuYarn_Delivery_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and JobWork_Weaving_PavuYarn_Delivery_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, JobWork_Weaving_PavuYarn_Delivery_No desc", con)
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

            lbl_DcNo.Text = Common_Procedures.get_MaxCode(con, "JobWork_Weaving_PavuYarn_Delivery_Head", "JobWork_Weaving_PavuYarn_Delivery_Code", "For_OrderBy", "", Val(lbl_Company.Tag), Common_Procedures.FnYearCode)

            txt_JJFormNo.Text = Common_Procedures.get_MaxCode(con, "JobWork_Weaving_PavuYarn_Delivery_Head", "JobWork_Weaving_PavuYarn_Delivery_Code", "JJ_Form_OrderByNo", "", Val(lbl_Company.Tag), Common_Procedures.FnYearCode)

            lbl_DcNo.ForeColor = Color.Red

            msk_Date.Text = Date.Today.ToShortDateString
            da = New SqlClient.SqlDataAdapter("select top 1 * from JobWork_Weaving_PavuYarn_Delivery_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and JobWork_Weaving_PavuYarn_Delivery_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, JobWork_Weaving_PavuYarn_Delivery_No desc", con)
            dt1 = New DataTable
            da.Fill(dt1)
            If dt1.Rows.Count > 0 Then
                If Val(Common_Procedures.settings.PreviousEntryDate_ByDefault) = 1 Then '---- M.S Textiles (Tirupur)
                    If dt1.Rows(0).Item("JobWork_Weaving_PavuYarn_Delivery_Date").ToString <> "" Then msk_Date.Text = dt1.Rows(0).Item("JobWork_Weaving_PavuYarn_Delivery_Date").ToString
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

            inpno = InputBox("Enter Dc.No.", "FOR FINDING...")

            RecCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

            Da = New SqlClient.SqlDataAdapter("select JobWork_Weaving_PavuYarn_Delivery_No from JobWork_Weaving_PavuYarn_Delivery_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and JobWork_Weaving_PavuYarn_Delivery_Code = '" & Trim(RecCode) & "'", con)
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

        ' If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.JobWork_Weaving_PavuYarn_Delivery_Entry, "~L~") = 0 And InStr(Common_Procedures.UR.JobWork_Weaving_PavuYarn_Delivery_Entry, "~I~") = 0 Then MessageBox.Show("You have No Rights to Insert", "DOES NOT INSERT...", MessageBoxButtons.OK, MessageBoxIcon.Error) : Exit Sub

        Try

            inpno = InputBox("Enter New Dc No.", "FOR NEW RECEIPT INSERTION...")

            RecCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

            Da = New SqlClient.SqlDataAdapter("select JobWork_Weaving_PavuYarn_Delivery_No from JobWork_Weaving_PavuYarn_Delivery_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and JobWork_Weaving_PavuYarn_Delivery_Code = '" & Trim(RecCode) & "'", con)
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
        Dim Del_ID As Integer = 0
        Dim Clo_ID As Integer = 0
        Dim Led_ID As Integer = 0
        Dim Trans_ID As Integer = 0
        Dim KuPvu_EdsCnt_ID As Integer = 0
        Dim SzPvu_EdsCnt_ID As Integer = 0
        Dim Sno As Integer = 0, YSno As Integer = 0, EbSno As Integer = 0
        Dim Nr As Integer = 0
        Dim Partcls As String = ""
        Dim PBlNo As String = ""
        Dim vTotPvuBms As Single, vTotPvuMtrs As Single
        Dim YCnt_ID As Integer = 0
        Dim PvuRec_ID As Integer = 0
        Dim YrnRec_ID As Integer = 0
        Dim YMil_ID As Integer = 0
        Dim vTotYrnBags As Single, vTotYrnCones As Single, vTotYrnWeight As Single, vTotYrnThiri As Single
        Dim EntID As String = ""
        Dim Bw_IdNo As Integer = 0
        Dim Pavu_DelvInc As Integer = 0
        Dim Ent_NoofUsed As Integer = 0
        Dim Thiri_val As Single = 0
        Dim Stock_Weight As Single = 0
        Dim Stock_In As String
        Dim mtrspcs As Single
        Dim dt2 As New DataTable
        Dim vTotPvuStk As Single = 0
        Dim Delv_Ledtype As String = ""
        Dim Rec_Ledtype As String = ""
        Dim Stk_DelvIdNo As Integer, Stk_RecIdNo As Integer
        Dim Prtcls_DelvIdNo As Integer, Prtcls_RecIdNo As Integer, Empty_Bms As Integer

        If Val(lbl_Company.Tag) = 0 Then
            MessageBox.Show("Invalid Company Selection", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        '  If Common_Procedures.UserRight_Check(Common_Procedures.UR.JobWork_Weaving_PavuYarn_Delivery_Entry, New_Entry) = False Then Exit Sub

        If pnl_Back.Enabled = False Then
            MessageBox.Show("Close Other Windows", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        If IsDate(msk_Date.Text) = False Then
            MessageBox.Show("Invalid Date", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If msk_Date.Enabled And msk_Date.Visible Then msk_Date.Focus()
            Exit Sub
        End If

        If Not (Convert.ToDateTime(msk_Date.Text) >= Common_Procedures.Company_FromDate And Convert.ToDateTime(msk_Date.Text) <= Common_Procedures.Company_ToDate) Then
            MessageBox.Show("Date is out of financial range", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If msk_Date.Enabled And msk_Date.Visible Then msk_Date.Focus()
            Exit Sub
        End If

        Del_ID = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_DeliveryTo.Text)
        If Del_ID = 0 Then
            MessageBox.Show("Invalid Delivery Name", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If cbo_DeliveryTo.Enabled And cbo_DeliveryTo.Visible Then cbo_DeliveryTo.Focus()
            Exit Sub
        End If

        PvuRec_ID = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_JobworkerName.Text)
        If PvuRec_ID = 0 Then
            MessageBox.Show("Invalid Jobworker Name", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If cbo_JobworkerName.Enabled And cbo_JobworkerName.Visible Then cbo_JobworkerName.Focus()
            Exit Sub
        End If

        YrnRec_ID = PvuRec_ID

        KuPvu_EdsCnt_ID = Common_Procedures.EndsCount_NameToIdNo(con, cbo_EndsCount.Text)
        If KuPvu_EdsCnt_ID = 0 And Val(txt_KuraiPavuMeters.Text) <> 0 Then
            MessageBox.Show("Invalid Ends Count", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If cbo_EndsCount.Enabled And cbo_EndsCount.Visible Then cbo_EndsCount.Focus()
            Exit Sub
        End If

        Trans_ID = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Transport.Text)

        With dgv_PavuDetails

            For i = 0 To .RowCount - 1

                If Val(dgv_PavuDetails.Rows(i).Cells(4).Value) <> 0 Then

                    If Trim(dgv_PavuDetails.Rows(i).Cells(1).Value) = "" Then
                        MessageBox.Show("Invalid Set No", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        If dgv_PavuDetails.Enabled And dgv_PavuDetails.Visible Then
                            dgv_PavuDetails.CurrentCell = dgv_PavuDetails.Rows(i).Cells(1)
                            dgv_PavuDetails.Focus()
                        End If
                        Exit Sub
                    End If

                    If Trim(dgv_PavuDetails.Rows(i).Cells(2).Value) = "" Then
                        MessageBox.Show("Invalid Beam No", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        If dgv_PavuDetails.Enabled And dgv_PavuDetails.Visible Then
                            dgv_PavuDetails.CurrentCell = dgv_PavuDetails.Rows(i).Cells(2)
                            dgv_PavuDetails.Focus()
                        End If
                        Exit Sub
                    End If

                    If Trim(dgv_PavuDetails.Rows(i).Cells(5).Value) = "" Then
                        MessageBox.Show("Invalid Ends Count", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        If dgv_PavuDetails.Enabled And dgv_PavuDetails.Visible Then
                            dgv_PavuDetails.CurrentCell = dgv_PavuDetails.Rows(i).Cells(5)
                            dgv_PavuDetails.Focus()
                        End If
                        Exit Sub
                    End If

                End If

            Next

        End With

        vTotPvuBms = 0 : vTotPvuMtrs = 0 : vTotYrnThiri = 0
        If dgv_PavuDetails_Total.RowCount > 0 Then
            vTotPvuBms = Val(dgv_PavuDetails_Total.Rows(0).Cells(2).Value())
            vTotPvuMtrs = Val(dgv_PavuDetails_Total.Rows(0).Cells(4).Value())
        End If

        For i = 0 To dgv_YarnDetails.RowCount - 1

            If Val(dgv_YarnDetails.Rows(i).Cells(6).Value) <> 0 Then

                YCnt_ID = Common_Procedures.Count_NameToIdNo(con, dgv_YarnDetails.Rows(i).Cells(1).Value)
                If Val(YCnt_ID) = 0 Then
                    MessageBox.Show("Invalid CountName", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    dgv_YarnDetails.CurrentCell = dgv_YarnDetails.Rows(0).Cells(1)
                    dgv_YarnDetails.Focus()
                    Exit Sub
                End If

                If Trim(dgv_YarnDetails.Rows(i).Cells(2).Value) = "" Then
                    MessageBox.Show("Invalid Yarn Type", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    dgv_YarnDetails.CurrentCell = dgv_YarnDetails.Rows(0).Cells(2)
                    dgv_YarnDetails.Focus()
                    Exit Sub
                End If

                YMil_ID = Common_Procedures.Mill_NameToIdNo(con, dgv_YarnDetails.Rows(i).Cells(3).Value)
                If Val(YMil_ID) = 0 Then
                    MessageBox.Show("Invalid Mill Name", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    dgv_YarnDetails.CurrentCell = dgv_YarnDetails.Rows(0).Cells(3)
                    dgv_YarnDetails.Focus()
                    Exit Sub
                End If

            End If

        Next

        vTotYrnBags = 0 : vTotYrnCones = 0 : vTotYrnWeight = 0 : vTotYrnThiri = 0
        If dgv_YarnDetails_Total.RowCount > 0 Then
            vTotYrnBags = Val(dgv_YarnDetails_Total.Rows(0).Cells(4).Value())
            vTotYrnCones = Val(dgv_YarnDetails_Total.Rows(0).Cells(5).Value())
            vTotYrnWeight = Val(dgv_YarnDetails_Total.Rows(0).Cells(6).Value())
            If dgv_YarnDetails_Total.Columns(7).Visible = True Then
                vTotYrnThiri = Val(dgv_YarnDetails_Total.Rows(0).Cells(7).Value())
            End If
        End If

        If Trim(txt_JJFormNo.Text) <> "" Then
            NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)
            Da = New SqlClient.SqlDataAdapter("select * from JobWork_Weaving_PavuYarn_Delivery_Head where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and JobWork_Weaving_PavuYarn_Delivery_Code <> '" & Trim(NewCode) & "' and JobWork_Weaving_PavuYarn_Delivery_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "' and JJ_FormNo = '" & Trim(txt_JJFormNo.Text) & "'", con)
            Dt1 = New DataTable
            Da.Fill(Dt1)
            If Dt1.Rows.Count > 0 Then
                MessageBox.Show("Duplicate JJ Form No", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                If txt_JJFormNo.Enabled And txt_JJFormNo.Visible Then txt_JJFormNo.Focus()
                Exit Sub
            End If
            Dt1.Clear()
        End If

        tr = con.BeginTransaction

        Try

            If Insert_Entry = True Or New_Entry = False Then
                NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            Else

                lbl_DcNo.Text = Common_Procedures.get_MaxCode(con, "JobWork_Weaving_PavuYarn_Delivery_Head", "JobWork_Weaving_PavuYarn_Delivery_Code", "For_OrderBy", "", Val(lbl_Company.Tag), Common_Procedures.FnYearCode, tr)

                NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            End If

            cmd.Connection = con
            cmd.Transaction = tr

            cmd.Parameters.Clear()
            cmd.Parameters.AddWithValue("@EntryDate", Convert.ToDateTime(msk_Date.Text))

            If New_Entry = True Then
                cmd.CommandText = "Insert into JobWork_Weaving_PavuYarn_Delivery_Head(JobWork_Weaving_PavuYarn_Delivery_Code, Company_IdNo, JobWork_Weaving_PavuYarn_Delivery_No, for_OrderBy, JobWork_Weaving_PavuYarn_Delivery_Date, DeliveryTo_Idno, Empty_Beam, Meters, Party_DcNo, EndsCount_IdNo, Vehicle_No , Transport_Idno , ReceivedFrom_Idno , Yarn_ReceivedFrom_IdNo, Freight_Charge , Note , Total_Beam, Total_Meters, Total_Bags, Total_Cones, Total_Weight, Total_Thiri, JJ_Form_OrderByNo, JJ_FormNo , Empty_Bobin ) Values ('" & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_DcNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_DcNo.Text))) & ", @EntryDate, " & Str(Val(Del_ID)) & ", " & Val(txt_KuraiPavuBeam.Text) & "," & Val(txt_KuraiPavuMeters.Text) & " , '" & Trim(txt_PartyDcNo.Text) & "' , " & Str(Val(KuPvu_EdsCnt_ID)) & ", '" & Trim(cbo_VehicleNo.Text) & "' , " & Val(Trans_ID) & ", " & Val(PvuRec_ID) & ", " & Val(YrnRec_ID) & ", " & Val(txt_Freight.Text) & " , '" & Trim(txt_Note.Text) & "', " & Str(Val(vTotPvuBms)) & ", " & Str(Val(vTotPvuMtrs)) & ", " & Str(Val(vTotYrnBags)) & ", " & Str(Val(vTotYrnCones)) & ", " & Str(Val(vTotYrnWeight)) & ", " & Str(Val(vTotYrnThiri)) & ", " & Str(Val(Common_Procedures.OrderBy_CodeToValue(txt_JJFormNo.Text))) & ", '" & Trim(txt_JJFormNo.Text) & "' , " & Str(Val(txt_NoOfBobin.Text)) & " )"
                cmd.ExecuteNonQuery()

            Else
                cmd.CommandText = "Update JobWork_Weaving_PavuYarn_Delivery_Head set JobWork_Weaving_PavuYarn_Delivery_Date = @EntryDate, DeliveryTo_Idno = " & Str(Val(Del_ID)) & ", Empty_Beam = " & Val(txt_KuraiPavuBeam.Text) & ", Meters = " & Val(txt_KuraiPavuMeters.Text) & " , Party_DcNo = '" & Trim(txt_PartyDcNo.Text) & "' ,  EndsCount_IdNo = " & Str(Val(KuPvu_EdsCnt_ID)) & ",Vehicle_No = '" & Trim(cbo_VehicleNo.Text) & "' , Transport_Idno = " & Val(Trans_ID) & " ,ReceivedFrom_Idno = " & Val(PvuRec_ID) & ", Yarn_ReceivedFrom_IdNo = " & Val(YrnRec_ID) & ", Freight_Charge = " & Val(txt_Freight.Text) & " , Note = '" & Trim(txt_Note.Text) & "' , Total_Beam = " & Str(Val(vTotPvuBms)) & ", Total_Meters = " & Str(Val(vTotPvuMtrs)) & ", Total_Bags = " & Str(Val(vTotYrnBags)) & ", Total_Cones = " & Str(Val(vTotYrnCones)) & ", Total_Weight = " & Str(Val(vTotYrnWeight)) & " , Total_Thiri = " & Str(Val(vTotYrnThiri)) & ", JJ_Form_OrderByNo = " & Str(Val(Common_Procedures.OrderBy_CodeToValue(txt_JJFormNo.Text))) & ", JJ_FormNo = '" & Trim(txt_JJFormNo.Text) & "' , Empty_Bobin = " & Str(Val(txt_NoOfBobin.Text)) & "  Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & "  and JobWork_Weaving_PavuYarn_Delivery_Code = '" & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()

                Da = New SqlClient.SqlDataAdapter("Select * from JobWork_Weaving_Pavu_Delivery_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and JobWork_Weaving_PavuYarn_Delivery_Code = '" & Trim(NewCode) & "'", con)
                Da.SelectCommand.Transaction = tr
                Dt1 = New DataTable
                Da.Fill(Dt1)

                If Dt1.Rows.Count > 0 Then
                    For i = 0 To Dt1.Rows.Count - 1

                        cmd.CommandText = "update Stock_SizedPavu_Processing_Details set " _
                                  & " StockAt_IdNo = " & Str(Val(Dt1.Rows(i).Item("ReceivedFrom_IdNo").ToString)) & ", " _
                                  & " Pavu_Delivery_Increment = Pavu_Delivery_Increment - 1 " _
                                  & " Where " _
                                  & " StockAt_IdNo = " & Str(Val(Dt1.Rows(i).Item("DeliveryTo_IdNo").ToString)) & " and " _
                                  & " Set_Code = '" & Trim(Dt1.Rows(i).Item("Set_Code").ToString) & "' and " _
                                  & " beam_no = '" & Trim(Dt1.Rows(i).Item("Beam_No").ToString) & "' and " _
                                  & " Pavu_Delivery_Increment = " & Str(Val(Dt1.Rows(i).Item("Noof_Used").ToString))
                        cmd.ExecuteNonQuery()

                    Next
                End If
                Dt1.Clear()

            End If

            EntID = Trim(Pk_Condition) & Trim(lbl_DcNo.Text)
            If Trim(txt_PartyDcNo.Text) <> "" Then
                Partcls = "Weav.Delv : P.DcNo. " & Trim(txt_PartyDcNo.Text)
                PBlNo = Trim(txt_PartyDcNo.Text)
            Else
                Partcls = "Weav.Delv : Dc.No. " & Trim(lbl_DcNo.Text)
                PBlNo = Trim(lbl_DcNo.Text)
            End If

            Delv_Ledtype = Common_Procedures.get_FieldValue(con, "Ledger_Head", "Ledger_Type", "(Ledger_IdNo = " & Str(Val(Del_ID)) & ")", , tr)
            Rec_Ledtype = Common_Procedures.get_FieldValue(con, "Ledger_Head", "Ledger_Type", "(Ledger_IdNo = " & Str(Val(YrnRec_ID)) & ")", , tr)

            cmd.CommandText = "Delete from JobWork_Weaving_Pavu_Delivery_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and JobWork_Weaving_PavuYarn_Delivery_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from JobWork_Weaving_Yarn_Delivery_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and JobWork_Weaving_PavuYarn_Delivery_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Stock_Pavu_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Stock_Yarn_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Stock_Empty_BeamBagCone_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            If Val(txt_KuraiPavuMeters.Text) <> 0 And Val(KuPvu_EdsCnt_ID) <> 0 Then
                Stk_DelvIdNo = Del_ID
                Stk_RecIdNo = 0

                Prtcls_DelvIdNo = Del_ID
                Prtcls_RecIdNo = PvuRec_ID

                cmd.CommandText = "Insert into Stock_Pavu_Processing_Details(Reference_Code, Company_IdNo, Reference_No, for_OrderBy, Reference_Date, DeliveryTo_Idno, ReceivedFrom_Idno, Cloth_Idno, Entry_ID, Party_Bill_No, Particulars, Sl_No, EndsCount_IdNo, Sized_Beam, Meters,DeliveryToIdno_ForParticulars,ReceivedFromIdno_ForParticulars) Values ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_DcNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_DcNo.Text))) & ", @EntryDate, " & Str(Val(Stk_DelvIdNo)) & ", " & Str(Val(Stk_RecIdNo)) & ", " & Str(Val(Clo_ID)) & ", '" & Trim(EntID) & "', '" & Trim(PBlNo) & "', '" & Trim(Partcls) & "', -100, " & Str(Val(KuPvu_EdsCnt_ID)) & ", " & Str(Val(txt_KuraiPavuBeam.Text)) & ", " & Str(Val(txt_KuraiPavuMeters.Text)) & ", " & Str(Val(Prtcls_DelvIdNo)) & ", " & Str(Val(Prtcls_RecIdNo)) & " )"
                cmd.ExecuteNonQuery()

                Stk_DelvIdNo = PvuRec_ID
                Stk_RecIdNo = 0
                cmd.CommandText = "Insert into Stock_Pavu_Processing_Details(Reference_Code, Company_IdNo, Reference_No, for_OrderBy, Reference_Date, DeliveryTo_Idno, ReceivedFrom_Idno, Cloth_Idno, Entry_ID, Party_Bill_No, Particulars, Sl_No, EndsCount_IdNo, Sized_Beam, Meters, DeliveryToIdno_ForParticulars, ReceivedFromIdno_ForParticulars) Values ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_DcNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_DcNo.Text))) & ", @EntryDate, " & Str(Val(Stk_DelvIdNo)) & ", " & Str(Val(Stk_RecIdNo)) & ", " & Str(Val(Clo_ID)) & ", '" & Trim(EntID) & "', '" & Trim(PBlNo) & "', '" & Trim(Partcls) & "', -101, " & Str(Val(KuPvu_EdsCnt_ID)) & ", " & Str(Val(txt_KuraiPavuBeam.Text)) & ", " & Str(Val(txt_KuraiPavuMeters.Text)) & ", " & Str(Val(Prtcls_DelvIdNo)) & ", " & Str(Val(Prtcls_RecIdNo)) & " )"
                cmd.ExecuteNonQuery()
            End If

            cmd.CommandText = "truncate table " & Trim(Common_Procedures.EntryTempTable) & ""
            cmd.ExecuteNonQuery()

            With dgv_PavuDetails
                Sno = 0
                For i = 0 To dgv_PavuDetails.RowCount - 1

                    If Val(dgv_PavuDetails.Rows(i).Cells(4).Value) <> 0 Then

                        Sno = Sno + 1

                        SzPvu_EdsCnt_ID = Common_Procedures.EndsCount_NameToIdNo(con, .Rows(i).Cells(5).Value, tr)

                        Bw_IdNo = Common_Procedures.BeamWidth_NameToIdNo(con, .Rows(i).Cells(6).Value, tr)

                        Ent_NoofUsed = 0
                        If Val(.Rows(i).Cells(8).Value) = 0 Or (Val(.Rows(i).Cells(8).Value) > 0 And Val(.Rows(i).Cells(8).Value) = Val(.Rows(i).Cells(10).Value)) Then

                            Nr = 0
                            cmd.CommandText = "update Stock_SizedPavu_Processing_Details set StockAt_IdNo = " & Str(Val(Del_ID)) & ", Pavu_Delivery_Increment = Pavu_Delivery_Increment + 1 " & _
                                                        " Where  Set_Code = '" & Trim(.Rows(i).Cells(9).Value) & "' and Beam_No = '" & Trim(.Rows(i).Cells(2).Value) & "' and StockAt_IdNo = " & Str(Val(PvuRec_ID))
                            Nr = cmd.ExecuteNonQuery()

                            If Nr = 0 Then
                                Throw New ApplicationException("Invalid Received From Name")
                                Exit Sub
                            End If

                            Ent_NoofUsed = Val(Common_Procedures.get_FieldValue(con, "Stock_SizedPavu_Processing_Details", "Pavu_Delivery_Increment", "(Set_Code = '" & Trim(.Rows(i).Cells(9).Value) & "' and Beam_No = '" & Trim(.Rows(i).Cells(2).Value) & "' )", , tr))

                        Else
                            Ent_NoofUsed = Val(.Rows(i).Cells(8).Value)

                        End If

                        cmd.CommandText = "Insert into JobWork_Weaving_Pavu_Delivery_Details(JobWork_Weaving_PavuYarn_Delivery_Code,              Company_IdNo        ,     JobWork_Weaving_PavuYarn_Delivery_No     ,                               for_OrderBy                             , JobWork_Weaving_PavuYarn_Delivery_Date,       DeliveryTo_IdNo    ,      ReceivedFrom_IdNo  ,          Sl_No       ,               Set_No                   ,                 Beam_No                ,                    Pcs                    ,                    Meters                ,             EndsCount_IdNo       ,           Beam_Width_IdNo,              Noof_Used        ,                  Set_Code              ) " & _
                                                    " Values  (  '" & Trim(NewCode) & "'           , " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_DcNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_DcNo.Text))) & ",        @EntryDate       ,  " & Str(Val(Del_ID)) & ", " & Str(Val(PvuRec_ID)) & ", " & Str(Val(Sno)) & ", '" & Trim(.Rows(i).Cells(1).Value) & "', '" & Trim(.Rows(i).Cells(2).Value) & "', " & Str(Val(.Rows(i).Cells(3).Value)) & " , " & Str(Val(.Rows(i).Cells(4).Value)) & ", " & Str(Val(SzPvu_EdsCnt_ID)) & ", " & Str(Val(Bw_IdNo)) & ", " & Str(Val(Ent_NoofUsed)) & ", '" & Trim(.Rows(i).Cells(9).Value) & "')"
                        cmd.ExecuteNonQuery()

                        cmd.CommandText = "insert into " & Trim(Common_Procedures.EntryTempTable) & "(Int1, Int2, Meters1) values (" & Str(Val(SzPvu_EdsCnt_ID)) & ", 1, " & Str(Val(.Rows(i).Cells(4).Value)) & ")"
                        cmd.ExecuteNonQuery()

                    End If

                Next

            End With

            Da = New SqlClient.SqlDataAdapter("select Int1 as PavuEndsCount_IdNo, sum(Int2) as PavuBeam, sum(Meters1) as PavuMeters from " & Trim(Common_Procedures.EntryTempTable) & " group by Int1 having sum(Int2) <> 0 or sum(Meters1) <> 0", con)
            Da.SelectCommand.Transaction = tr
            Dt1 = New DataTable
            Da.Fill(Dt1)

            Sno = 0
            If Dt1.Rows.Count > 0 Then
                For i = 0 To Dt1.Rows.Count - 1

                    vTotPvuMtrs = 0
                    vTotPvuMtrs = Str(Val(Dt1.Rows(i).Item("PavuMeters").ToString))

                    Stock_In = ""
                    mtrspcs = 0

                    Da = New SqlClient.SqlDataAdapter("Select Stock_In , Meters_Pcs from  EndsCount_Head Where EndsCount_IdNo = " & Str(Val(Dt1.Rows(i).Item("PavuEndsCount_IdNo").ToString)), con)
                    Da.SelectCommand.Transaction = tr
                    dt2 = New DataTable
                    Da.Fill(dt2)
                    If dt2.Rows.Count > 0 Then
                        Stock_In = dt2.Rows(0)("Stock_In").ToString
                        mtrspcs = Val(dt2.Rows(0)("Meters_Pcs").ToString)
                    End If
                    dt2.Clear()

                    If Trim(UCase(Stock_In)) = "PCS" Then
                        If Val(mtrspcs) = 0 Then mtrspcs = 1
                        vTotPvuStk = vTotPvuMtrs / mtrspcs

                    Else
                        vTotPvuStk = vTotPvuMtrs

                    End If

                    Stk_DelvIdNo = Del_ID
                    Stk_RecIdNo = 0

                    Prtcls_DelvIdNo = Del_ID
                    Prtcls_RecIdNo = PvuRec_ID

                    Sno = Sno + 1
                    cmd.CommandText = "Insert into Stock_Pavu_Processing_Details(Reference_Code, Company_IdNo, Reference_No, for_OrderBy, Reference_Date, DeliveryTo_Idno, ReceivedFrom_Idno, Cloth_Idno, Entry_ID, Party_Bill_No, Particulars, Sl_No, EndsCount_IdNo, Sized_Beam, Meters, DeliveryToIdno_ForParticulars, ReceivedFromIdno_ForParticulars ) Values ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_DcNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_DcNo.Text))) & ", @EntryDate, " & Str(Val(Stk_DelvIdNo)) & ", " & Str(Val(Stk_RecIdNo)) & ", " & Str(Val(Clo_ID)) & ", '" & Trim(EntID) & "', '" & Trim(PBlNo) & "', '" & Trim(Partcls) & "', " & Str(Val(Sno)) & ", " & Str(Val(Dt1.Rows(i).Item("PavuEndsCount_IdNo").ToString)) & ", " & Str(Val(Dt1.Rows(i).Item("PavuBeam").ToString)) & ", " & Str(Val(vTotPvuStk)) & ", " & Str(Val(Prtcls_DelvIdNo)) & ", " & Str(Val(Prtcls_RecIdNo)) & " )"
                    cmd.ExecuteNonQuery()

                    Stk_DelvIdNo = PvuRec_ID
                    Stk_RecIdNo = 0

                    Sno = Sno + 1
                    cmd.CommandText = "Insert into Stock_Pavu_Processing_Details(Reference_Code, Company_IdNo, Reference_No, for_OrderBy, Reference_Date, DeliveryTo_Idno, ReceivedFrom_Idno, Cloth_Idno, Entry_ID, Party_Bill_No, Particulars, Sl_No, EndsCount_IdNo, Sized_Beam, Meters, DeliveryToIdno_ForParticulars, ReceivedFromIdno_ForParticulars ) Values ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_DcNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_DcNo.Text))) & ", @EntryDate, " & Str(Val(Stk_DelvIdNo)) & ", " & Str(Val(Stk_RecIdNo)) & ", " & Str(Val(Clo_ID)) & ", '" & Trim(EntID) & "', '" & Trim(PBlNo) & "', '" & Trim(Partcls) & "', " & Str(Val(Sno)) & ", " & Str(Val(Dt1.Rows(i).Item("PavuEndsCount_IdNo").ToString)) & ", " & Str(Val(Dt1.Rows(i).Item("PavuBeam").ToString)) & ", " & Str(Val(vTotPvuStk)) & ", " & Str(Val(Prtcls_DelvIdNo)) & ", " & Str(Val(Prtcls_RecIdNo)) & " )"
                    cmd.ExecuteNonQuery()

                Next

            End If

            Dt1.Clear()

            With dgv_YarnDetails
                Sno = 0
                YSno = 0
                For i = 0 To .RowCount - 1

                    If Val(.Rows(i).Cells(6).Value) <> 0 Then

                        Sno = Sno + 1

                        YCnt_ID = Common_Procedures.Count_NameToIdNo(con, .Rows(i).Cells(1).Value, tr)
                        YMil_ID = Common_Procedures.Mill_NameToIdNo(con, .Rows(i).Cells(3).Value, tr)

                        Thiri_val = 0
                        If .Columns(7).Visible = True Then
                            Thiri_val = Val(.Rows(i).Cells(7).Value)
                        End If

                        cmd.CommandText = "Insert into JobWork_Weaving_Yarn_Delivery_Details(JobWork_Weaving_PavuYarn_Delivery_Code, Company_IdNo, JobWork_Weaving_PavuYarn_Delivery_No, for_OrderBy, JobWork_Weaving_PavuYarn_Delivery_Date, Sl_No, count_idno, Yarn_Type, Mill_IdNo,  Bags, Cones, Weight , Thiri ) Values ('" & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_DcNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_DcNo.Text))) & ", @EntryDate,  " & Str(Val(Sno)) & ", " & Str(Val(YCnt_ID)) & ", '" & Trim(.Rows(i).Cells(2).Value) & "', " & Str(Val(YMil_ID)) & ", " & Str(Val(.Rows(i).Cells(4).Value)) & ", " & Str(Val(.Rows(i).Cells(5).Value)) & ", " & Str(Val(.Rows(i).Cells(6).Value)) & " ,  " & Str(Val(Thiri_val)) & " )"
                        cmd.ExecuteNonQuery()

                        Stock_Weight = Val(.Rows(i).Cells(6).Value)
                        If Trim(UCase(Delv_Ledtype)) = "WEAVER" Then
                            If .Columns(7).Visible = True Then
                                Stock_Weight = 0
                                If Val(.Rows(i).Cells(7).Value) <> 0 Then
                                    Stock_Weight = Val(.Rows(i).Cells(7).Value)
                                End If
                            End If
                        End If

                        If Val(Stock_Weight) <> 0 Then

                            Stk_DelvIdNo = Del_ID
                            Stk_RecIdNo = 0
                            Prtcls_RecIdNo = YrnRec_ID
                            Prtcls_DelvIdNo = 0

                            YSno = YSno + 1
                            cmd.CommandText = "Insert into Stock_Yarn_Processing_Details ( Reference_Code, Company_IdNo, Reference_No, for_OrderBy, Reference_Date, DeliveryTo_Idno, ReceivedFrom_Idno, Entry_ID, Particulars, Party_Bill_No, Sl_No, Count_IdNo, Yarn_Type, Mill_IdNo, Bags, Cones, Weight, DeliveryToIdno_ForParticulars, ReceivedFromIdno_ForParticulars ) Values ( '" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_DcNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_DcNo.Text))) & ", @EntryDate, " & Str(Val(Stk_DelvIdNo)) & ", " & Str(Val(Stk_RecIdNo)) & ", '" & Trim(EntID) & "', '" & Trim(Partcls) & "', '" & Trim(PBlNo) & "', " & Str(Val(YSno)) & ", " & Str(Val(YCnt_ID)) & ", '" & Trim(.Rows(i).Cells(2).Value) & "', " & Str(Val(YMil_ID)) & ", " & Str(Val(.Rows(i).Cells(4).Value)) & ", " & Str(Val(.Rows(i).Cells(5).Value)) & ", " & Str(Val(Stock_Weight)) & ", " & Str(Val(Prtcls_DelvIdNo)) & ", " & Str(Val(Prtcls_RecIdNo)) & " )"
                            cmd.ExecuteNonQuery()

                        End If

                        Stock_Weight = Val(.Rows(i).Cells(6).Value)
                        If Trim(UCase(Rec_Ledtype)) = "WEAVER" Then
                            If .Columns(7).Visible = True Then
                                Stock_Weight = 0
                                If Val(.Rows(i).Cells(7).Value) <> 0 Then
                                    Stock_Weight = Val(.Rows(i).Cells(7).Value)
                                End If
                            End If
                        End If

                        If Val(Stock_Weight) <> 0 Then

                            Stk_DelvIdNo = YrnRec_ID
                            Stk_RecIdNo = 0
                            Prtcls_DelvIdNo = 0
                            Prtcls_RecIdNo = Del_ID

                            YSno = YSno + 1
                            cmd.CommandText = "Insert into Stock_Yarn_Processing_Details(Reference_Code, Company_IdNo, Reference_No, for_OrderBy, Reference_Date, DeliveryTo_Idno, ReceivedFrom_Idno, Entry_ID, Particulars, Party_Bill_No, Sl_No, Count_IdNo, Yarn_Type, Mill_IdNo, Bags, Cones, Weight, DeliveryToIdno_ForParticulars, ReceivedFromIdno_ForParticulars ) Values ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_DcNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_DcNo.Text))) & ", @EntryDate, " & Str(Val(Stk_DelvIdNo)) & ", " & Str(Val(Stk_RecIdNo)) & ", '" & Trim(EntID) & "', '" & Trim(Partcls) & "', '" & Trim(PBlNo) & "', " & Str(Val(YSno)) & ", " & Str(Val(YCnt_ID)) & ", '" & Trim(.Rows(i).Cells(2).Value) & "', " & Str(Val(YMil_ID)) & ", " & Str(Val(.Rows(i).Cells(4).Value)) & ", " & Str(Val(.Rows(i).Cells(5).Value)) & ", " & Str(Val(Stock_Weight)) & ", " & Str(Val(Prtcls_DelvIdNo)) & ", " & Str(Val(Prtcls_RecIdNo)) & " )"
                            cmd.ExecuteNonQuery()

                        End If

                    End If

                Next

            End With

            Empty_Bms = 0
            If Common_Procedures.settings.Weaver_Zari_Kuri_Entries_Status = 1 Then

                Empty_Bms = Val(txt_KuraiPavuBeam.Text)
            Else
                Empty_Bms = Val(txt_KuraiPavuBeam.Text) + Val(vTotPvuBms)
            End If

            EbSno = 0
            If Val(Empty_Bms) <> 0 Or Val(txt_NoOfBobin.Text) <> 0 Then
                Stk_DelvIdNo = Del_ID
                Stk_RecIdNo = 0

                EbSno = EbSno + 1
                cmd.CommandText = "Insert into Stock_Empty_BeamBagCone_Processing_Details(Reference_Code, Company_IdNo, Reference_No, for_OrderBy, Reference_Date, DeliveryTo_Idno, ReceivedFrom_Idno, Party_Bill_No, Entry_ID, Sl_No, Beam_Width_IdNo, Empty_Beam, Empty_Bobin, Pavu_Beam, Yarn_Bags, Yarn_Cones, Particulars) Values ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_DcNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_DcNo.Text))) & ", @EntryDate, " & Str(Val(Stk_DelvIdNo)) & ", " & Str(Val(Stk_RecIdNo)) & ", '" & Trim(PBlNo) & "', '" & Trim(EntID) & "', " & Str(Val(EbSno)) & ", 0, 0, " & Str(Val(txt_NoOfBobin.Text)) & ", " & Str(Val(Empty_Bms)) & " ,  0, 0, '" & Trim(Partcls) & "')"
                cmd.ExecuteNonQuery()

                Stk_DelvIdNo = PvuRec_ID
                Stk_RecIdNo = 0

                EbSno = EbSno + 1
                cmd.CommandText = "Insert into Stock_Empty_BeamBagCone_Processing_Details(Reference_Code, Company_IdNo, Reference_No, for_OrderBy, Reference_Date, DeliveryTo_Idno, ReceivedFrom_Idno, Party_Bill_No, Entry_ID, Sl_No, Beam_Width_IdNo, Empty_Beam, Empty_Bobin, Pavu_Beam, Yarn_Bags, Yarn_Cones, Particulars) Values ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_DcNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_DcNo.Text))) & ", @EntryDate, " & Str(Val(Stk_DelvIdNo)) & ", " & Str(Val(Stk_RecIdNo)) & ", '" & Trim(PBlNo) & "', '" & Trim(EntID) & "', " & Str(Val(EbSno)) & ", 0, 0, " & Str(Val(txt_NoOfBobin.Text)) & ", " & Str(Val(Empty_Bms)) & " ,  0, 0, '" & Trim(Partcls) & "')"
                cmd.ExecuteNonQuery()
            End If
            If Val(vTotYrnBags) <> 0 Or Val(vTotYrnCones) <> 0 Then
                Stk_DelvIdNo = Del_ID
                Stk_RecIdNo = 0

                EbSno = EbSno + 1
                cmd.CommandText = "Insert into Stock_Empty_BeamBagCone_Processing_Details(Reference_Code, Company_IdNo, Reference_No, for_OrderBy, Reference_Date, DeliveryTo_Idno, ReceivedFrom_Idno, Party_Bill_No, Entry_ID, Sl_No, Beam_Width_IdNo, Empty_Beam, Pavu_Beam, Yarn_Bags, Yarn_Cones, Particulars) Values ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_DcNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_DcNo.Text))) & ", @EntryDate, " & Str(Val(Stk_DelvIdNo)) & ", " & Str(Val(Stk_RecIdNo)) & ", '" & Trim(PBlNo) & "', '" & Trim(EntID) & "', " & Str(Val(EbSno)) & ", 0, 0, 0, " & Str(Val(vTotYrnBags)) & ", " & Str(Val(vTotYrnCones)) & ", '" & Trim(Partcls) & "')"
                cmd.ExecuteNonQuery()

                Stk_DelvIdNo = YrnRec_ID
                Stk_RecIdNo = 0

                EbSno = EbSno + 1
                cmd.CommandText = "Insert into Stock_Empty_BeamBagCone_Processing_Details(Reference_Code, Company_IdNo, Reference_No, for_OrderBy, Reference_Date, DeliveryTo_Idno, ReceivedFrom_Idno, Party_Bill_No, Entry_ID, Sl_No, Beam_Width_IdNo, Empty_Beam, Pavu_Beam, Yarn_Bags, Yarn_Cones, Particulars) Values ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_DcNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_DcNo.Text))) & ", @EntryDate, " & Str(Val(Stk_DelvIdNo)) & ", " & Str(Val(Stk_RecIdNo)) & ", '" & Trim(PBlNo) & "', '" & Trim(EntID) & "', " & Str(Val(EbSno)) & ", 0, 0, 0, " & Str(Val(vTotYrnBags)) & ", " & Str(Val(vTotYrnCones)) & ", '" & Trim(Partcls) & "')"
                cmd.ExecuteNonQuery()

            End If


            Dim vLed_IdNos As String = "", vVou_Amts As String = "", ErrMsg As String = ""

            vLed_IdNos = Trans_ID & "|" & Val(Common_Procedures.CommonLedger.Transport_Charges_Ac)
            vVou_Amts = Val(txt_Freight.Text) & "|" & -1 * Val(txt_Freight.Text)
            If Common_Procedures.Voucher_Updation(con, "JbWPY.Delv", Val(lbl_Company.Tag), Trim(Pk_Condition) & Trim(NewCode), Trim(lbl_DcNo.Text), Convert.ToDateTime(msk_Date.Text), Partcls, vLed_IdNos, vVou_Amts, ErrMsg, tr, Common_Procedures.SoftwareTypes.Textile_Software) = False Then
                Throw New ApplicationException(ErrMsg)
            End If

            tr.Commit()

            Dt1.Dispose()
            Da.Dispose()

            If Val(Common_Procedures.settings.OnSave_MoveTo_NewEntry_Status) = 1 Then
                If New_Entry = True Then
                    new_record()
                Else
                    move_record(lbl_DcNo.Text)
                End If
            Else
                move_record(lbl_DcNo.Text)
            End If

            If SaveAll_STS = False Then
                MessageBox.Show("Saved Sucessfully!!!", "FOR SAVING...", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If

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
            If msk_Date.Enabled And msk_Date.Visible Then msk_Date.Focus()

        End Try

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

        CntID = Common_Procedures.Count_NameToIdNo(con, dgv_YarnDetails.Rows(dgv_YarnDetails.CurrentRow.Index).Cells(1).Value)
        MilID = Common_Procedures.Mill_NameToIdNo(con, dgv_YarnDetails.Rows(dgv_YarnDetails.CurrentRow.Index).Cells(3).Value)

        Wgt_Bag = 0 : Wgt_Cn = 0 : Cn_bag = 0

        If CntID <> 0 And MilID <> 0 Then

            Da = New SqlClient.SqlDataAdapter("select * from Mill_Count_Details where mill_idno = " & Str(Val(MilID)) & " and count_idno = " & Str(Val(CntID)), con)
            Da.Fill(Dt)
            With dgv_YarnDetails

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

    Private Sub cbo_DelvAt_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_DeliveryTo.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'SIZING' or Ledger_Type = 'WEAVER')", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_Ledger_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_DeliveryTo.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_DeliveryTo, msk_Date, cbo_JobworkerName, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'SIZING' or Ledger_Type = 'WEAVER')", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_Ledger_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_DeliveryTo.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_DeliveryTo, cbo_JobworkerName, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'SIZING' or Ledger_Type = 'WEAVER')", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_Ledger_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_DeliveryTo.KeyUp
        If e.Control = False And e.KeyValue = 17 Then

            Common_Procedures.MDI_LedType = "WEAVER"
            Dim f As New Ledger_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_DeliveryTo.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()
        End If
    End Sub

    Private Sub cbo_VehicleNo_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_VehicleNo.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "JobWork_Weaving_PavuYarn_Delivery_Head", "Vehicle_No", "", "(Vehicle_No <> '')")
    End Sub

    Private Sub cbo_VehicleNo_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_VehicleNo.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_VehicleNo, cbo_Transport, txt_Freight, "JobWork_Weaving_PavuYarn_Delivery_Head", "Vehicle_No", "", "(Vehicle_No <> '')")
    End Sub

    Private Sub cbo_VehicleNo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_VehicleNo.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_VehicleNo, txt_Freight, "JobWork_Weaving_PavuYarn_Delivery_Head", "Vehicle_No", "", "(Vehicle_No <> '')", False)
    End Sub

    Private Sub cbo_EndsCount_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_EndsCount.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "EndsCount_Head", "EndsCount_Name", "", "(EndsCount_IdNo = 0)")
    End Sub

    Private Sub cbo_EndsCount_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_EndsCount.KeyDown

        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_EndsCount, txt_PartyDcNo, txt_KuraiPavuMeters, "EndsCount_Head", "EndsCount_Name", "", "(EndsCount_IdNo = 0)")
    End Sub

    Private Sub cbo_EndsCount_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_EndsCount.KeyPress

        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_EndsCount, txt_KuraiPavuMeters, "EndsCount_Head", "EndsCount_Name", "", "(EndsCount_IdNo = 0)")
    End Sub

    Private Sub cbo_EndsCount_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_EndsCount.KeyUp
        If e.Control = False And e.KeyValue = 17 Then
            Dim f As New EndsCount_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_EndsCount.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

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
        Dim n As Integer, i As Integer
        Dim Del_IdNo As Integer, Cnt_IdNo As Integer
        Dim Condt As String = ""
        Dim EdsCnt_IdNo As Integer, Mil_IdNo As Integer

        Try

            Condt = ""
            Del_IdNo = 0
            Cnt_IdNo = 0
            Mil_IdNo = 0
            EdsCnt_IdNo = 0

            If IsDate(dtp_Filter_Fromdate.Value) = True And IsDate(dtp_Filter_ToDate.Value) = True Then
                Condt = "a.JobWork_Weaving_PavuYarn_Delivery_Date between '" & Trim(Format(dtp_Filter_Fromdate.Value, "MM/dd/yyyy")) & "' and '" & Trim(Format(dtp_Filter_ToDate.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_Filter_Fromdate.Value) = True Then
                Condt = "a.JobWork_Weaving_PavuYarn_Delivery_Date = '" & Trim(Format(dtp_Filter_Fromdate.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_Filter_ToDate.Value) = True Then
                Condt = "a.JobWork_Weaving_PavuYarn_Delivery_Date = '" & Trim(Format(dtp_Filter_ToDate.Value, "MM/dd/yyyy")) & "' "
            End If

            If Trim(cbo_Filter_DeliveryName.Text) <> "" Then
                Del_IdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Filter_DeliveryName.Text)
            End If

            If Trim(cbo_Filter_CountName.Text) <> "" Then
                Cnt_IdNo = Common_Procedures.Count_NameToIdNo(con, cbo_Filter_CountName.Text)
            End If

            If Trim(cbo_Filter_MillName.Text) <> "" Then
                Mil_IdNo = Common_Procedures.Mill_NameToIdNo(con, cbo_Filter_MillName.Text)
            End If

            If Trim(cbo_Filter_CountName.Text) <> "" Then
                EdsCnt_IdNo = Common_Procedures.EndsCount_NameToIdNo(con, cbo_Filter_EndsCount.Text)
            End If


            If Val(Del_IdNo) <> 0 Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & "a.DeliveryTo_Idno = " & Str(Val(Del_IdNo))
            End If

            If Val(Cnt_IdNo) <> 0 Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " a.JobWork_Weaving_PavuYarn_Delivery_Code IN (select z1.JobWork_Weaving_PavuYarn_Delivery_Code from JobWork_Weaving_Yarn_Delivery_Details z1 where z1.Count_IdNo = " & Str(Val(Cnt_IdNo)) & ")"
            End If

            If Val(Mil_IdNo) <> 0 Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " a.JobWork_Weaving_PavuYarn_Delivery_Code IN (select z1.JobWork_Weaving_PavuYarn_Delivery_Code from JobWork_Weaving_Yarn_Delivery_Details z1 where z1.Mill_IdNo = " & Str(Val(Mil_IdNo)) & ")"
            End If

            If Val(EdsCnt_IdNo) <> 0 Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " a.EndsCount_IdNo = " & Str(Val(EdsCnt_IdNo))
            End If

            da = New SqlClient.SqlDataAdapter("select a.*, b.Ledger_Name as Deliv_Name from JobWork_Weaving_PavuYarn_Delivery_Head a INNER JOIN Ledger_Head b on a.DeliveryTo_Idno = b.Ledger_IdNo where a.company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.JobWork_Weaving_PavuYarn_Delivery_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "' " & IIf(Trim(Condt) <> "", " and ", "") & Condt & " Order by a.for_orderby, a.JobWork_Weaving_PavuYarn_Delivery_No", con)
            da.Fill(dt2)

            dgv_Filter_Details.Rows.Clear()

            If dt2.Rows.Count > 0 Then

                For i = 0 To dt2.Rows.Count - 1

                    n = dgv_Filter_Details.Rows.Add()

                    dgv_Filter_Details.Rows(n).Cells(0).Value = i + 1
                    dgv_Filter_Details.Rows(n).Cells(1).Value = dt2.Rows(i).Item("JobWork_Weaving_PavuYarn_Delivery_No").ToString
                    dgv_Filter_Details.Rows(n).Cells(2).Value = Format(Convert.ToDateTime(dt2.Rows(i).Item("JobWork_Weaving_PavuYarn_Delivery_Date").ToString), "dd-MM-yyyy")
                    dgv_Filter_Details.Rows(n).Cells(3).Value = dt2.Rows(i).Item("Deliv_Name").ToString
                    dgv_Filter_Details.Rows(n).Cells(4).Value = Val(dt2.Rows(i).Item("Total_Beam").ToString)
                    dgv_Filter_Details.Rows(n).Cells(5).Value = Format(Val(dt2.Rows(i).Item("Total_Meters").ToString), "########0.00")
                    dgv_Filter_Details.Rows(n).Cells(6).Value = Val(dt2.Rows(i).Item("Total_Bags").ToString)
                    dgv_Filter_Details.Rows(n).Cells(7).Value = Val(dt2.Rows(i).Item("Total_Cones").ToString)
                    dgv_Filter_Details.Rows(n).Cells(8).Value = Format(Val(dt2.Rows(i).Item("Total_Weight").ToString), "########0.000")

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

    Private Sub dtp_Filter_ToDate_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dtp_Filter_ToDate.KeyDown
        If e.KeyCode = 40 Then SendKeys.Send("{TAB}")
        If e.KeyCode = 38 Then SendKeys.Send("+{TAB}")
    End Sub


    Private Sub dtp_Filter_ToDate_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dtp_Filter_ToDate.KeyPress
        If Asc(e.KeyChar) = 13 Then
            cbo_Filter_DeliveryName.Focus()
        End If
    End Sub

    Private Sub cbo_Filter_DeliveryName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Filter_DeliveryName.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'SIZING' or Ledger_Type = 'WEAVER' or Ledger_Type = 'JOBWORKER'or Ledger_Type = 'REWINDING')", "(Ledger_IdNo = 0)")

    End Sub

    Private Sub cbo_Filter_PartyName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Filter_DeliveryName.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_DeliveryName, dtp_Filter_ToDate, cbo_Filter_CountName, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'SIZING' or Ledger_Type = 'WEAVER' or Ledger_Type = 'JOBWORKER'or Ledger_Type = 'REWINDING')", "(Ledger_IdNo = 0)")

    End Sub

    Private Sub cbo_Filter_PartyName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Filter_DeliveryName.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Filter_DeliveryName, cbo_Filter_CountName, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'SIZING' or Ledger_Type = 'WEAVER' or Ledger_Type = 'JOBWORKER'or Ledger_Type = 'REWINDING')", "(Ledger_IdNo = 0)")

    End Sub

    Private Sub cbo_Filter_CountName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Filter_CountName.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Count_Head", "Count_Name", "", "(Count_IdNo = 0)")

    End Sub


    Private Sub cbo_Filter_CountName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Filter_CountName.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_CountName, cbo_Filter_DeliveryName, cbo_Filter_MillName, "Count_Head", "Count_Name", "", "(Count_IdNo = 0)")

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
            Filter_RowNo = dgv_Filter_Details.CurrentRow.Index
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

    Private Sub dgtxt_Details_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgtxt_Details.Enter
        dgv_YarnDetails.EditingControl.BackColor = Color.Lime
        dgv_YarnDetails.EditingControl.ForeColor = Color.Blue
        dgtxt_Details.SelectAll()
    End Sub
    Private Sub dgv_PavuDetails_CellEndEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_PavuDetails.CellEndEdit
        TotalPavu_Calculation()
        'SendKeys.Send("{up}")
        'SendKeys.Send("{Tab}")
    End Sub

    Private Sub dgv_PavuDetails_CellEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_PavuDetails.CellEnter

        With dgv_PavuDetails
            If Val(.CurrentRow.Cells(0).Value) = 0 Then
                .CurrentRow.Cells(0).Value = .CurrentRow.Index + 1
            End If


        End With

    End Sub

    Private Sub dgv_PavuDetails_CellLeave(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_PavuDetails.CellLeave
        Try
            With dgv_PavuDetails
                If .Rows.Count >= 0 Then
                    If .CurrentCell.ColumnIndex = 4 Then
                        If Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value) <> 0 Then
                            If Common_Procedures.settings.Weaver_Zari_Kuri_Entries_Status = 1 Then

                                .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = Format(Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value), "#########0.000")
                            Else
                                .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = Format(Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value), "#########0.00")
                            End If

                        End If
                    End If
                End If

            End With

        Catch ex As Exception
            '----

        End Try
    End Sub

    Private Sub dgv_PavuDetails_CellValueChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_PavuDetails.CellValueChanged
        On Error Resume Next

        If IsNothing(dgv_PavuDetails.CurrentCell) Then Exit Sub
        With dgv_PavuDetails
            If .Visible Then
                If .CurrentCell.ColumnIndex = 4 Then
                    TotalPavu_Calculation()
                End If
            End If
        End With
    End Sub

    Private Sub dgv_PavuDetails_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_PavuDetails.KeyDown

        On Error Resume Next

        With dgv_PavuDetails

            If e.KeyCode = Keys.Up Then
                If .CurrentCell.RowIndex = 0 Then
                    .CurrentCell.Selected = False
                    e.SuppressKeyPress = True
                    e.Handled = True
                    cbo_JobworkerName.Focus()
                End If
            End If

            If e.KeyCode = Keys.Down Then
                If .CurrentCell.RowIndex = .RowCount - 1 Then
                    .CurrentCell.Selected = False
                    e.SuppressKeyPress = True
                    e.Handled = True

                    dgv_YarnDetails.CurrentCell = dgv_YarnDetails.Rows(0).Cells(1)
                    dgv_YarnDetails.Focus()

                End If
            End If

            If e.KeyCode = Keys.Enter Then
                e.SuppressKeyPress = True
                e.Handled = True

                If .CurrentCell.RowIndex = .RowCount - 1 And .CurrentCell.ColumnIndex >= 1 And Trim(.CurrentRow.Cells(1).Value) = "" Then
                    dgv_YarnDetails.Focus()
                    dgv_YarnDetails.CurrentCell = dgv_YarnDetails.Rows(0).Cells(1)

                Else
                    SendKeys.Send("{Tab}")

                End If

            End If

        End With

    End Sub

    Private Sub dgv_PavuDetails_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_PavuDetails.KeyUp
        Dim n As Integer

        If e.Control = True And UCase(Chr(e.KeyCode)) = "D" Then

            With dgv_PavuDetails

                n = .CurrentRow.Index

                If Val(.Rows(n).Cells(8).Value) > 0 And Val(.Rows(n).Cells(8).Value) <> Val(.Rows(n).Cells(10).Value) Then
                    MessageBox.Show("Cannot Delete" & Chr(13) & "Already this pavu delivered to others")
                    Exit Sub
                End If

                If n = .Rows.Count - 1 Then

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

            TotalPavu_Calculation()

        End If

    End Sub

    Private Sub dgv_Details_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgv_PavuDetails.LostFocus
        On Error Resume Next
        If Not IsNothing(dgv_PavuDetails.CurrentCell) Then dgv_PavuDetails.CurrentCell.Selected = False
    End Sub

    Private Sub dgv_PavuDetails_RowsAdded(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewRowsAddedEventArgs) Handles dgv_PavuDetails.RowsAdded
        Dim n As Integer

        If IsNothing(dgv_PavuDetails.CurrentCell) Then Exit Sub
        With dgv_PavuDetails
            n = .RowCount
            .Rows(n - 1).Cells(0).Value = Val(n)
        End With
    End Sub

    Private Sub TotalPavu_Calculation()
        Dim Sno As Integer
        Dim TotBms As Single, TotMtrs As Single

        Sno = 0
        TotBms = 0
        TotMtrs = 0
        With dgv_PavuDetails
            For i = 0 To .RowCount - 1
                Sno = Sno + 1
                .Rows(i).Cells(0).Value = Sno
                If Val(.Rows(i).Cells(4).Value) <> 0 Then
                    TotBms = TotBms + 1
                    TotMtrs = TotMtrs + Val(.Rows(i).Cells(4).Value)
                End If
            Next
        End With

        With dgv_PavuDetails_Total
            If .RowCount = 0 Then .Rows.Add()
            .Rows(0).Cells(2).Value = Val(TotBms)
            If Common_Procedures.settings.Weaver_Zari_Kuri_Entries_Status = 1 Then

                .Rows(0).Cells(4).Value = Format(Val(TotMtrs), "########0.000")
            Else
                .Rows(0).Cells(4).Value = Format(Val(TotMtrs), "########0.00")
            End If

        End With

    End Sub

    Private Sub dgv_YarnDetails_CellEndEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_YarnDetails.CellEndEdit
        Try

            If dgv_YarnDetails.CurrentRow.Cells(2).Value = "MILL" Then
                If dgv_YarnDetails.CurrentCell.ColumnIndex = 4 Or dgv_YarnDetails.CurrentCell.ColumnIndex = 5 Then
                    get_MillCount_Details()
                End If
            End If
            dgv_PavuDetails_CellLeave(sender, e)

        Catch ex As Exception

        End Try
    End Sub

    Private Sub dgv_YarnDetails_CellEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_YarnDetails.CellEnter
        Try
            Dim Da As New SqlClient.SqlDataAdapter
            Dim Dt1 As New DataTable
            Dim Dt2 As New DataTable
            Dim Dt3 As New DataTable
            Dim rect As Rectangle

            With dgv_YarnDetails
                If Val(.CurrentRow.Cells(0).Value) = 0 Then
                    .CurrentRow.Cells(0).Value = .CurrentRow.Index + 1
                End If

                If Trim(.CurrentRow.Cells(2).Value) = "" Then
                    .CurrentRow.Cells(2).Value = "MILL"
                End If

                If e.ColumnIndex = 1 Then

                    If cbo_Grid_CountName.Visible = False Or Val(cbo_Grid_CountName.Tag) <> e.RowIndex Then

                        cbo_Grid_CountName.Tag = -1
                        Da = New SqlClient.SqlDataAdapter("select Count_Name from Count_Head order by Count_Name", con)
                        Dt1 = New DataTable
                        Da.Fill(Dt1)
                        cbo_Grid_CountName.DataSource = Dt1
                        cbo_Grid_CountName.DisplayMember = "Count_Name"

                        rect = .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False)

                        cbo_Grid_CountName.Left = .Left + rect.Left  '  .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Left
                        cbo_Grid_CountName.Top = .Top + rect.Top  ' .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Top

                        cbo_Grid_CountName.Width = rect.Width  ' .CurrentCell.Size.Width
                        cbo_Grid_CountName.Height = rect.Height  ' rect.Height
                        cbo_Grid_CountName.Text = .CurrentCell.Value  '  Trim(.CurrentRow.Cells(e.ColumnIndex).Value)

                        cbo_Grid_CountName.Tag = Val(e.RowIndex)
                        cbo_Grid_CountName.Visible = True

                        cbo_Grid_CountName.BringToFront()
                        cbo_Grid_CountName.Focus()

                        'cbo_Grid_MillName.Visible = False
                        'cbo_Grid_YarnType.Visible = False

                    End If


                Else

                    cbo_Grid_CountName.Visible = False
                    'cbo_Grid_CountName.Tag = -1
                    'cbo_Grid_CountName.Text = ""

                End If

                If e.ColumnIndex = 2 Then

                    If cbo_Grid_YarnType.Visible = False Or Val(cbo_Grid_YarnType.Tag) <> e.RowIndex Then

                        cbo_Grid_YarnType.Tag = -1
                        Da = New SqlClient.SqlDataAdapter("select Yarn_Type from YarnType_Head order by Yarn_Type", con)
                        Dt2 = New DataTable
                        Da.Fill(Dt2)
                        cbo_Grid_YarnType.DataSource = Dt2
                        cbo_Grid_YarnType.DisplayMember = "Yarn_Type"

                        rect = .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False)

                        cbo_Grid_YarnType.Left = .Left + rect.Left  '  .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Left
                        cbo_Grid_YarnType.Top = .Top + rect.Top  ' .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Top
                        cbo_Grid_YarnType.Width = rect.Width  ' .CurrentCell.Size.Width
                        cbo_Grid_YarnType.Height = rect.Height  ' rect.Height

                        cbo_Grid_YarnType.Text = .CurrentCell.Value  '  Trim(.CurrentRow.Cells(e.ColumnIndex).Value)

                        cbo_Grid_YarnType.Tag = Val(e.RowIndex)
                        cbo_Grid_YarnType.Visible = True

                        cbo_Grid_YarnType.BringToFront()
                        cbo_Grid_YarnType.Focus()

                        'cbo_Grid_CountName.Visible = False
                        'cbo_Grid_YarnType.Visible = False

                    End If

                Else

                    cbo_Grid_YarnType.Visible = False
                    'cbo_Grid_YarnType.Tag = -1
                    'cbo_Grid_YarnType.Text = ""

                End If

                If e.ColumnIndex = 3 Then

                    If cbo_Grid_MillName.Visible = False Or Val(cbo_Grid_MillName.Tag) <> e.RowIndex Then

                        cbo_Grid_MillName.Tag = -1
                        Da = New SqlClient.SqlDataAdapter("select Mill_Name from Mill_Head order by Mill_Name", con)
                        Dt3 = New DataTable
                        Da.Fill(Dt3)
                        cbo_Grid_MillName.DataSource = Dt3
                        cbo_Grid_MillName.DisplayMember = "Mill_Name"

                        rect = .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False)

                        cbo_Grid_MillName.Left = .Left + rect.Left  '  .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Left
                        cbo_Grid_MillName.Top = .Top + rect.Top  ' .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Top
                        cbo_Grid_MillName.Width = rect.Width  ' .CurrentCell.Size.Width
                        cbo_Grid_MillName.Height = rect.Height  ' rect.Height

                        cbo_Grid_MillName.Text = .CurrentCell.Value  '  Trim(.CurrentRow.Cells(e.ColumnIndex).Value)

                        cbo_Grid_MillName.Tag = Val(e.RowIndex)
                        cbo_Grid_MillName.Visible = True

                        cbo_Grid_MillName.BringToFront()
                        cbo_Grid_MillName.Focus()

                        'cbo_Grid_CountName.Visible = False
                        'cbo_Grid_MillName.Visible = False

                    End If

                Else

                    cbo_Grid_MillName.Visible = False
                    'cbo_Grid_MillName.Tag = -1
                    'cbo_Grid_MillName.Text = ""

                End If

            End With

        Catch ex As Exception

        End Try
    End Sub

    Private Sub dgv_YarnDetails_CellLeave(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_YarnDetails.CellLeave
        Try

            With dgv_YarnDetails

                'If e.ColumnIndex = 1 Then
                '    cbo_Grid_CountName.Visible = False
                '    'cbo_Grid_CountName.Tag = -1
                '    'cbo_Grid_CountName.Text = ""
                'End If

                'If e.ColumnIndex = 2 Then
                '    cbo_Grid_YarnType.Visible = False
                '    'cbo_Grid_YarnType.Tag = -1
                '    'cbo_Grid_YarnType.Text = ""
                'End If

                'If e.ColumnIndex = 3 Then
                '    cbo_Grid_MillName.Visible = False
                '    'cbo_Grid_MillName.Tag = -1
                '    'cbo_Grid_MillName.Text = ""
                'End If

                If .CurrentCell.ColumnIndex = 6 Or .CurrentCell.ColumnIndex = 7 Then
                    If Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value) <> 0 Then
                        .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = Format(Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value), "#########0.000")
                    End If
                End If
            End With
        Catch ex As Exception

        End Try
    End Sub

    Private Sub dgv_YarnDetails_CellValueChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_YarnDetails.CellValueChanged

        On Error Resume Next
        With dgv_YarnDetails
            If .Visible Then
                If e.ColumnIndex = 1 Or e.ColumnIndex = 6 Then
                    If .Columns(7).Visible = True Then
                        Thiri_Calculation()
                    End If
                End If
                If e.ColumnIndex = 4 Or e.ColumnIndex = 5 Or e.ColumnIndex = 6 Or e.ColumnIndex = 7 Then
                    TotalYarnTaken_Calculation()
                End If
            End If
        End With
    End Sub

    Private Sub dgv_YarnDetails_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_YarnDetails.KeyDown

        On Error Resume Next

        With dgv_YarnDetails

            'MsgBox("dgv_YarnDetails_KeyDown : " & .CurrentCell.ColumnIndex())

            If e.KeyCode = Keys.Up Then
                If .CurrentCell.RowIndex = 0 Then
                    .CurrentCell.Selected = False
                    dgv_PavuDetails.CurrentCell = dgv_PavuDetails.Rows(0).Cells(1)
                    dgv_PavuDetails.Focus()
                    dgv_PavuDetails.CurrentCell.Selected = True
                    'SendKeys.Send("{RIGHT}")
                End If
            End If
            If e.KeyCode = Keys.Left Then
                If .CurrentCell.RowIndex = 0 And .CurrentCell.ColumnIndex <= 0 Then
                    .CurrentCell.Selected = False
                    dgv_PavuDetails.CurrentCell = dgv_PavuDetails.Rows(0).Cells(1)
                    dgv_PavuDetails.Focus()
                    dgv_PavuDetails.CurrentCell.Selected = True
                    'SendKeys.Send("{RIGHT}")
                End If
            End If

            If e.KeyCode = Keys.Enter Then
                e.SuppressKeyPress = True
                e.Handled = True

                SendKeys.Send("{Tab}")

            End If


        End With

    End Sub

    Private Sub dgv_YarnDetails_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_YarnDetails.KeyUp
        Dim i As Integer

        If e.Control = True And UCase(Chr(e.KeyCode)) = "D" Then

            With dgv_YarnDetails

                .Rows.RemoveAt(.CurrentRow.Index)

                For i = 0 To .Rows.Count - 1
                    .Rows(i).Cells(0).Value = i + 1
                Next

            End With
        End If

    End Sub

    Private Sub dgv_YarnDetails_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgv_YarnDetails.LostFocus
        On Error Resume Next
        dgv_YarnDetails.CurrentCell.Selected = False
    End Sub

    Private Sub dgv_YarnDetails_RowsAdded(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewRowsAddedEventArgs) Handles dgv_YarnDetails.RowsAdded
        Dim n As Integer

        With dgv_YarnDetails
            n = .RowCount
            .Rows(n - 1).Cells(0).Value = Val(n)
            .Rows(n - 1).Cells(2).Value = "MILL"
        End With
    End Sub
    Private Sub Thiri_Calculation()
        Dim q As Single = 0
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim count_val As Single
        Dim CntID As Integer

        CntID = Common_Procedures.Count_NameToIdNo(con, dgv_YarnDetails.Rows(dgv_YarnDetails.CurrentRow.Index).Cells(1).Value)

        count_val = 0

        Da = New SqlClient.SqlDataAdapter("select (Resultant_Count) from Count_Head where count_idno = " & Str(Val(CntID)), con)
        Da.Fill(Dt)
        With dgv_YarnDetails

            If Dt.Rows.Count > 0 Then
                If IsDBNull(Dt.Rows(0)(0).ToString) = False Then
                    count_val = Dt.Rows(0).Item("Resultant_Count").ToString
                End If
            End If

            Dt.Clear()
            Dt.Dispose()
            Da.Dispose()

            'If .CurrentCell.ColumnIndex = 4 Or .CurrentCell.ColumnIndex = 5 Or .CurrentCell.ColumnIndex = 6 Then
            ' If .CurrentCell.ColumnIndex = 4 Then
            If Val(.Rows(.CurrentRow.Index).Cells(6).Value) <> 0 Then
                .Rows(.CurrentRow.Index).Cells(7).Value = Format(count_val * 11 / 50 * .Rows(.CurrentRow.Index).Cells(6).Value, "##########0.000")
            Else
                .Rows(.CurrentRow.Index).Cells(7).Value = ""
            End If
            'End If

            'End If

        End With

        'End If

    End Sub
    Private Sub TotalYarnTaken_Calculation()
        Dim Sno As Integer
        Dim TotBags As Single, TotCones As Single, TotWeight As Single, TotThiri As Single

        Sno = 0
        TotBags = 0
        TotCones = 0
        TotWeight = 0
        TotThiri = 0
        With dgv_YarnDetails
            For i = 0 To .RowCount - 1
                Sno = Sno + 1
                .Rows(i).Cells(0).Value = Sno
                If Val(.Rows(i).Cells(6).Value) <> 0 Then
                    TotBags = TotBags + Val(.Rows(i).Cells(4).Value)
                    TotCones = TotCones + Val(.Rows(i).Cells(5).Value)
                    TotWeight = TotWeight + Val(.Rows(i).Cells(6).Value)
                    If .Columns(7).Visible = True Then
                        TotThiri = TotThiri + Val(.Rows(i).Cells(7).Value)
                    End If
                End If
            Next
        End With

        With dgv_YarnDetails_Total
            If .RowCount = 0 Then .Rows.Add()
            .Rows(0).Cells(4).Value = Val(TotBags)
            .Rows(0).Cells(5).Value = Val(TotCones)
            .Rows(0).Cells(6).Value = Format(Val(TotWeight), "########0.000")
            If .Columns(7).Visible = True Then
                .Rows(0).Cells(7).Value = Format(Val(TotThiri), "########0.000")
            End If

        End With

    End Sub

    Private Sub cbo_Grid_CountName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Grid_CountName.GotFocus
        Try
            Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Count_Head", "Count_Name", "", "(Count_IdNo = 0)")

        Catch ex As Exception

        End Try


    End Sub

    Private Sub cbo_Grid_CountName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Grid_CountName.KeyDown
        Try
            vcbo_KeyDwnVal = e.KeyValue
            Dim dep_idno As Integer = 0

            Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Grid_CountName, Nothing, cbo_Grid_YarnType, "Count_Head", "Count_Name", "", "(Count_IdNo = 0)")

            With dgv_YarnDetails

                If (e.KeyValue = 38 And cbo_Grid_CountName.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then

                    If Val(.CurrentCell.RowIndex) <= 0 Then
                        txt_Freight.Focus()


                    Else
                        .Focus()
                        .CurrentCell = .Rows(.CurrentCell.RowIndex - 1).Cells(6)
                        .CurrentCell.Selected = True


                    End If
                End If

                If (e.KeyValue = 40 And cbo_Grid_CountName.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then

                    If .CurrentCell.RowIndex = .RowCount - 1 And .CurrentCell.ColumnIndex >= 1 And Trim(.CurrentRow.Cells(1).Value) = "" Then


                        txt_Note.Focus()

                    Else
                        .Focus()
                        .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)

                    End If

                End If

            End With

        Catch ex As Exception

        End Try
    End Sub

    Private Sub cbo_Grid_CountName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Grid_CountName.KeyPress

        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable

        Try


            Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Grid_CountName, Nothing, "Count_Head", "Count_Name", "", "(Count_IdNo = 0)")

            If Asc(e.KeyChar) = 13 Then

                With dgv_YarnDetails

                    .Item(.CurrentCell.ColumnIndex, .CurrentRow.Index).Value = Trim(cbo_Grid_CountName.Text)
                    If .CurrentCell.RowIndex = .RowCount - 1 And .CurrentCell.ColumnIndex >= 1 And Trim(.CurrentRow.Cells(1).Value) = "" Then


                        txt_Note.Focus()
                    Else
                        .Focus()
                        .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)

                    End If
                End With

            End If

        Catch ex As Exception

        End Try
    End Sub

    Private Sub cbo_Grid_CountName_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Grid_CountName.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Dim f As New Count_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Grid_CountName.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If
    End Sub

    Private Sub cbo_Grid_CountName_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Grid_CountName.TextChanged
        Try
            If cbo_Grid_CountName.Visible Then

                If IsNothing(dgv_YarnDetails.CurrentCell) Then Exit Sub
                With dgv_YarnDetails
                    If Val(cbo_Grid_CountName.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = 1 Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(1).Value = Trim(cbo_Grid_CountName.Text)
                    End If
                End With
            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub cbo_Grid_MillName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Grid_MillName.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Mill_Head", "Mill_Name", "", "(Mill_IdNo = 0)")
    End Sub
    Private Sub cbo_Grid_MillName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Grid_MillName.KeyDown
        Dim dep_idno As Integer = 0

        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Grid_MillName, Nothing, Nothing, "Mill_Head", "Mill_Name", "", "(Mill_IdNo = 0)")

        With dgv_YarnDetails

            If (e.KeyValue = 38 And cbo_Grid_MillName.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then

                .Focus()
                .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex - 1)
            End If

            If (e.KeyValue = 40 And cbo_Grid_MillName.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then

                .Focus()
                .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)
                .CurrentCell.Selected = True

            End If

        End With
    End Sub

    Private Sub cbo_Grid_MillName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Grid_MillName.KeyPress
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable

        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Grid_MillName, Nothing, "Mill_Head", "Mill_Name", "", "(Mill_IdNo = 0)")

        If Asc(e.KeyChar) = 13 Then

            With dgv_YarnDetails
                .Focus()
                .Item(.CurrentCell.ColumnIndex, .CurrentRow.Index).Value = Trim(cbo_Grid_MillName.Text)
                .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)
                .CurrentCell.Selected = True
            End With

        End If
    End Sub

    Private Sub cbo_Grid_MillName_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Grid_MillName.KeyUp
        If e.Control = False And e.KeyValue = 17 Then
            Dim f As New Mill_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Grid_MillName.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If
    End Sub

    Private Sub cbo_Grid_MillName_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Grid_MillName.TextChanged
        Try
            If cbo_Grid_MillName.Visible Then

                If IsNothing(dgv_YarnDetails.CurrentCell) Then Exit Sub
                With dgv_YarnDetails
                    If Val(cbo_Grid_MillName.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = 3 Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_Grid_MillName.Text)
                    End If
                End With
            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub cbo_Grid_YarnType_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Grid_YarnType.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "YarnType_Head", "Yarn_Type", "", "Yarn_Type")
    End Sub

    Private Sub cbo_Grid_YarnType_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Grid_YarnType.KeyDown
        Dim dep_idno As Integer = 0

        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Grid_YarnType, Nothing, Nothing, "YarnType_Head", "Yarn_Type", "", "Yarn_Type")

        With dgv_YarnDetails

            If (e.KeyValue = 38 And cbo_Grid_YarnType.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then

                .Focus()
                .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex - 1)
            End If

            If (e.KeyValue = 40 And cbo_Grid_YarnType.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then

                .Focus()
                .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)

            End If

        End With
    End Sub

    Private Sub cbo_Grid_YarnType_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Grid_YarnType.KeyPress
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable


        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Grid_YarnType, Nothing, "YarnType_Head", "Yarn_Type", "", "Yarn_Type")

        If Asc(e.KeyChar) = 13 Then

            With dgv_YarnDetails

                .Focus()
                .Item(.CurrentCell.ColumnIndex, .CurrentRow.Index).Value = Trim(cbo_Grid_YarnType.Text)
                .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)
            End With

        End If
    End Sub

    Private Sub cbo_Grid_YarnType_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Grid_YarnType.TextChanged
        Try
            If cbo_Grid_YarnType.Visible Then

                If IsNothing(dgv_YarnDetails.CurrentCell) Then Exit Sub
                With dgv_YarnDetails
                    If Val(cbo_Grid_YarnType.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = 2 Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_Grid_YarnType.Text)
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

    'Private Sub dgv_YarnDetails_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgv_YarnDetails.GotFocus
    '    'dgv_YarnDetails.Focus()
    '    'dgv_YarnDetails.CurrentCell.Selected = True
    'End Sub

    Private Sub cbo_Filter_MillName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Filter_MillName.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Mill_Head", "Mill_Name", "", "(Mill_IdNo = 0)")

    End Sub

    Private Sub cbo_Filter_MillName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Filter_MillName.KeyDown

        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_MillName, cbo_Filter_CountName, cbo_Filter_EndsCount, "Mill_Head", "Mill_Name", "", "(Mill_IdNo = 0)")

    End Sub

    Private Sub cbo_Filter_MillName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Filter_MillName.KeyPress

        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Filter_MillName, cbo_Filter_EndsCount, "Mill_Head", "Mill_Name", "", "(Mill_IdNo = 0)")

    End Sub

    Private Sub cbo_Filter_EndsCount_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Filter_EndsCount.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "EndsCount_Head", "EndsCount_Name", "", "(EndsCount_IdNo = 0)")
    End Sub

    Private Sub cbo_Filter_EndsCount_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Filter_EndsCount.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_EndsCount, cbo_Filter_MillName, btn_Filter_Show, "EndsCount_Head", "EndsCount_Name", "", "(EndsCount_IdNo = 0)")
    End Sub

    Private Sub cbo_Filter_EndsCount_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Filter_EndsCount.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Filter_EndsCount, btn_Filter_Show, "EndsCount_Head", "EndsCount_Name", "", "(EndsCount_IdNo = 0)")
    End Sub

    Public Sub print_record() Implements Interface_MDIActions.print_record
        prn_FromNo = Trim(lbl_DcNo.Text)
        prn_ToNo = Trim(lbl_DcNo.Text)

        pnl_Print.Visible = True
        pnl_Back.Enabled = False
        If btn_Print_Invoice.Enabled And btn_Print_Invoice.Visible Then
            btn_Print_Invoice.Focus()
        End If
    End Sub

    Private Sub Printing_Delivery()
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim NewCode As String
        Dim ps As Printing.PaperSize
        Dim OrdBy_FrmNo As Single = 0, OrdByToNo As Single = 0
        Dim PpSzSTS As Boolean = False

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Try

            OrdBy_FrmNo = Common_Procedures.OrderBy_CodeToValue(prn_FromNo)
            OrdByToNo = Common_Procedures.OrderBy_CodeToValue(prn_ToNo)

            da1 = New SqlClient.SqlDataAdapter("select * from JobWork_Weaving_PavuYarn_Delivery_Head Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and for_orderby between " & Str(Format(Val(OrdBy_FrmNo), "########0.00")) & " and " & Str(Format(Val(OrdByToNo), "########0.00")) & " and JobWork_Weaving_PavuYarn_Delivery_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "'", con)
            'da1 = New SqlClient.SqlDataAdapter("select * from JobWork_Weaving_PavuYarn_Delivery_Head Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and JobWork_Weaving_PavuYarn_Delivery_Code = '" & Trim(NewCode) & "'", con)
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

        If prn_Status = 2 Then
            prn_InpOpts = InputBox("Select    -   0. None" & Chr(13) & "                  1. Original" & Chr(13) & "                  2. Duplicate" & Chr(13) & "                  3. Triplicate" & Chr(13) & "                  4. All", "FOR FORMJJ PRINTING...", "4")
            prn_InpOpts = Replace(Trim(prn_InpOpts), "4", "123")
        End If

        If prn_Status = 1 Then

            'Dim pkCustomSize1 As New System.Drawing.Printing.PaperSize("PAPER 8X6", 800, 600)
            'PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = pkCustomSize1
            'PrintDocument1.DefaultPageSettings.PaperSize = pkCustomSize1
            'PrintDocument1.DefaultPageSettings.Landscape = False

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

        Else

            For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
                If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
                    ps = PrintDocument1.PrinterSettings.PaperSizes(I)
                    PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = ps
                    PrintDocument1.DefaultPageSettings.PaperSize = ps
                    Exit For
                End If
            Next

        End If

        If Val(Common_Procedures.Print_OR_Preview_Status) = 1 Then

            Try
                If Val(Common_Procedures.settings.Printing_Show_PrintDialogue) = 1 Then
                    PrintDialog1.PrinterSettings = PrintDocument1.PrinterSettings
                    If PrintDialog1.ShowDialog() = Windows.Forms.DialogResult.OK Then
                        PrintDocument1.PrinterSettings = PrintDialog1.PrinterSettings

                        If prn_Status = 1 Then

                            'Dim pkCustomSize1 As New System.Drawing.Printing.PaperSize("PAPER 8X6", 800, 600)
                            'PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = pkCustomSize1
                            'PrintDocument1.DefaultPageSettings.PaperSize = pkCustomSize1
                            'PrintDocument1.DefaultPageSettings.Landscape = False

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

                        Else

                            For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
                                If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
                                    ps = PrintDocument1.PrinterSettings.PaperSizes(I)
                                    PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = ps
                                    PrintDocument1.DefaultPageSettings.PaperSize = ps
                                    Exit For
                                End If
                            Next

                        End If

                        PrintDocument1.Print()
                    End If

                Else
                    PrintDocument1.Print()

                End If

            Catch ex As Exception
                MessageBox.Show("The printing operation failed" & vbCrLf & ex.Message, "DOES NOT SHOW PRINT PREVIEW...", MessageBoxButtons.OK, MessageBoxIcon.Error)

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
    End Sub

    Private Sub PrintDocument1_BeginPrint(ByVal sender As Object, ByVal e As System.Drawing.Printing.PrintEventArgs) Handles PrintDocument1.BeginPrint
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim cmd As New SqlClient.SqlCommand
        Dim NewCode As String
        Dim i As Integer, k As Integer
        Dim vDup_SetNo As String
        Dim vPvu_BmNo As String, vDup_BmNo As String
        Dim W1 As Single = 0
        Dim FsNo As Single, LsNo As Single
        Dim FsBeamNo As String, LsBeamNo As String
        Dim OrdBy_FrmNo As Single = 0, OrdByToNo As Single = 0

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        prn_HdDt.Clear()
        prn_DetDt.Clear()
        prn_Prev_HeadIndx = -100
        prn_HeadIndx = 0
        prn_DetIndx = 0
        prn_DetSNo = 0
        prn_PageNo = 0
        prn_DetMxIndx = 0
        prn_NoofBmDets = 0
        prn_Count = 0

        Erase prn_HdAr
        Erase prn_DetAr

        prn_HdAr = New String(200, 10) {}
        prn_DetAr = New String(200, 10) {}

        Try

            OrdBy_FrmNo = Common_Procedures.OrderBy_CodeToValue(prn_FromNo)
            OrdByToNo = Common_Procedures.OrderBy_CodeToValue(prn_ToNo)

            da1 = New SqlClient.SqlDataAdapter("select a.*, b.*, c.Ledger_TinNo , c.Ledger_CstNo, c.Ledger_Name as Del_Name, c.Ledger_Address1, c.Ledger_Address2, c.Ledger_Address3, c.Ledger_Address4, d.Ledger_Name as Transport_Name, e.Area_Name, f.EndsCount_Name, g.Ledger_Name as Pavu_RecFrom_Name, h.Ledger_Name as Yarn_RecFrom_Name from JobWork_Weaving_PavuYarn_Delivery_Head a INNER JOIN Company_Head b ON a.Company_IdNo = b.Company_IdNo INNER JOIN Ledger_Head c ON a.DeliveryTo_Idno = c.Ledger_IdNo LEFT OUTER JOIN Ledger_Head d ON a.Transport_Idno = d.Ledger_IdNo  LEFT OUTER JOIN Area_Head e ON b.Area_Idno = e.Area_Idno LEFT OUTER JOIN EndsCount_Head f ON a.EndsCount_Idno = f.EndsCount_Idno LEFT OUTER JOIN Ledger_Head g ON a.ReceivedFrom_Idno = g.Ledger_IdNo LEFT OUTER JOIN Ledger_Head h ON a.Yarn_ReceivedFrom_Idno = h.Ledger_IdNo where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.for_orderby between " & Str(Format(Val(OrdBy_FrmNo), "########0.00")) & " and " & Str(Format(Val(OrdByToNo), "########0.00")) & " and a.JobWork_Weaving_PavuYarn_Delivery_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by a.for_orderby, a.JobWork_Weaving_PavuYarn_Delivery_No", con)
            'da1 = New SqlClient.SqlDataAdapter("select a.*, b.*, c.Ledger_TinNo , c.Ledger_CstNo, c.Ledger_Name as Del_Name, c.Ledger_Address1, c.Ledger_Address2, c.Ledger_Address3, c.Ledger_Address4, d.Ledger_Name as Transport_Name, e.Area_Name, f.EndsCount_Name, g.Ledger_Name as Pavu_RecFrom_Name, h.Ledger_Name as Yarn_RecFrom_Name from JobWork_Weaving_PavuYarn_Delivery_Head a INNER JOIN Company_Head b ON a.Company_IdNo = b.Company_IdNo INNER JOIN Ledger_Head c ON a.DeliveryTo_Idno = c.Ledger_IdNo LEFT OUTER JOIN Ledger_Head d ON a.Transport_Idno = d.Ledger_IdNo  LEFT OUTER JOIN Area_Head e ON b.Area_Idno = e.Area_Idno LEFT OUTER JOIN EndsCount_Head f ON a.EndsCount_Idno = f.EndsCount_Idno LEFT OUTER JOIN Ledger_Head g ON a.ReceivedFrom_Idno = g.Ledger_IdNo LEFT OUTER JOIN Ledger_Head h ON a.Yarn_ReceivedFrom_Idno = h.Ledger_IdNo where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.JobWork_Weaving_PavuYarn_Delivery_Code = '" & Trim(NewCode) & "'", con)
            prn_HdDt = New DataTable
            da1.Fill(prn_HdDt)

            If prn_HdDt.Rows.Count > 0 Then

                da2 = New SqlClient.SqlDataAdapter("select a.*, b.Mill_Name, c.Count_Name from JobWork_Weaving_Yarn_Delivery_Details a LEFT OUTER JOIN Mill_Head b on a.Mill_IdNo = b.Mill_IdNo INNER JOIN Count_Head c on a.Count_IdNo = c.Count_IdNo where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.JobWork_Weaving_PavuYarn_Delivery_Code = '" & Trim(NewCode) & "' Order by a.sl_no", con)
                prn_DetDt = New DataTable
                da2.Fill(prn_DetDt)

                vPrn_PvuEdsCnt = ""
                vPrn_PvuTotBms = 0
                vPrn_PvuTotMtrs = 0 : vPrn_PvuNPcs = 0
                vPrn_PvuSetNo = "" : vDup_SetNo = ""
                vDup_BmNo = "" : vPvu_BmNo = ""
                vPrn_PvuBmNos1 = "" : vPrn_PvuBmNos2 = "" : vPrn_PvuBmNos3 = "" : vPrn_PvuBmNos4 = ""

                cmd.Connection = con

                cmd.CommandText = "truncate table " & Trim(Common_Procedures.EntryTempTable) & ""
                cmd.ExecuteNonQuery()

                da1 = New SqlClient.SqlDataAdapter("select a.*, b.* from JobWork_Weaving_Pavu_Delivery_Details a INNER JOIN EndsCount_Head b ON a.EndsCount_IdNo = b.EndsCount_IdNo where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.JobWork_Weaving_PavuYarn_Delivery_Code = '" & Trim(NewCode) & "' Order by a.sl_no", con)
                Dt1 = New DataTable
                da1.Fill(Dt1)

                If Dt1.Rows.Count > 0 Then

                    vPrn_PvuEdsCnt = Dt1.Rows(0).Item("EndsCount_Name").ToString

                    For i = 0 To Dt1.Rows.Count - 1

                        vPrn_PvuTotBms = Val(vPrn_PvuTotBms) + 1
                        vPrn_PvuTotMtrs = vPrn_PvuTotMtrs + Val(Dt1.Rows(i).Item("Meters").ToString)
                        vPrn_PvuNPcs = vPrn_PvuNPcs + Val(Dt1.Rows(i).Item("Noof_Used").ToString)

                        If InStr(1, "~" & Trim(UCase(vDup_SetNo)) & "~", "~" & Trim(UCase(Dt1.Rows(i).Item("Set_No").ToString)) & "~") = 0 Then
                            vDup_SetNo = Trim(vDup_SetNo) & "~" & Trim(Dt1.Rows(i).Item("Set_No").ToString) & "~"
                            vPrn_PvuSetNo = vPrn_PvuSetNo & IIf(Trim(vPrn_PvuSetNo) <> "", ", ", "") & Dt1.Rows(i).Item("Set_No").ToString
                        End If

                        If InStr(1, "~" & Trim(UCase(vDup_BmNo)) & "~", "~" & Trim(UCase(Dt1.Rows(i).Item("Set_No").ToString)) & "^" & Trim(UCase(Dt1.Rows(i).Item("Beam_No").ToString)) & "~") = 0 Then
                            vDup_BmNo = Trim(vDup_BmNo) & "~" & Trim(Dt1.Rows(i).Item("Set_No").ToString) & "^" & Trim(UCase(Dt1.Rows(i).Item("Beam_No").ToString)) & "~"

                            cmd.CommandText = "Insert into " & Trim(Common_Procedures.EntryTempTable) & "(Name1, Meters1) values ('" & Trim(Dt1.Rows(i).Item("Beam_No").ToString) & "', " & Common_Procedures.OrderBy_CodeToValue(Trim(Dt1.Rows(i).Item("Beam_No").ToString)) & " )"
                            cmd.ExecuteNonQuery()

                        End If

                    Next i

                End If

                Dt1.Clear()

                '--

                da1 = New SqlClient.SqlDataAdapter("select a.*, b.* from JobWork_Weaving_Pavu_Delivery_Details a INNER JOIN EndsCount_Head b ON a.EndsCount_IdNo = b.EndsCount_IdNo where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.JobWork_Weaving_PavuYarn_Delivery_Code = '" & Trim(NewCode) & "' Order by a.sl_no", con)
                prn_DetDt1 = New DataTable
                da1.Fill(prn_DetDt1)

                If prn_DetDt1.Rows.Count > 0 Then
                    For i = 0 To prn_DetDt1.Rows.Count - 1

                        If Val(prn_DetDt1.Rows(i).Item("Meters").ToString) <> 0 Then
                            prn_DetMxIndx = prn_DetMxIndx + 1
                            prn_DetAr(prn_DetMxIndx, 1) = Trim(prn_DetDt1.Rows(i).Item("Ends_Name").ToString)
                            prn_DetAr(prn_DetMxIndx, 2) = Trim(prn_DetDt1.Rows(i).Item("Beam_No").ToString)
                            prn_DetAr(prn_DetMxIndx, 3) = Val(prn_DetDt1.Rows(i).Item("Pcs").ToString)
                            prn_DetAr(prn_DetMxIndx, 4) = Format(Val(prn_DetDt1.Rows(i).Item("Meters").ToString), "#########0.00")
                        End If

                    Next i

                End If
                Dt1.Clear()

                'prn_DetMxIndx = prn_DetMxIndx + 1
                'prn_DetAr(prn_DetMxIndx, 1) = "--------------------"
                'prn_DetAr(prn_DetMxIndx, 2) = "--------------------"
                'prn_DetAr(prn_DetMxIndx, 3) = "--------------------"
                'prn_DetAr(prn_DetMxIndx, 4) = "--------------------"


                'prn_DetMxIndx = prn_DetMxIndx + 1
                ''prn_DetAr(prn_DetMxIndx, 1) = Trim(prn_DetDt1.Rows(i).Item("EndsCount_Name").ToString)
                ''prn_DetAr(prn_DetMxIndx, 2) = Trim(prn_DetDt1.Rows(i).Item("Beam_No").ToString)
                ''prn_DetAr(prn_DetMxIndx, 3) = Val(prn_DetDt1.Rows(i).Item("Pcs").ToString)
                ''prn_DetAr(prn_DetMxIndx, 4) = Format(Val(prn_DetDt1.Rows(i).Item("Meters").ToString), "#########0.00")


                'prn_DetMxIndx = prn_DetMxIndx + 1
                'prn_DetAr(prn_DetMxIndx, 1) = "--------------------"
                'prn_DetAr(prn_DetMxIndx, 2) = "--------------------"
                'prn_DetAr(prn_DetMxIndx, 3) = "--------------------"
                'prn_DetAr(prn_DetMxIndx, 4) = "--------------------"

                '--

                vPvu_BmNo = ""
                FsNo = 0 : LsNo = 0
                FsBeamNo = "" : LsBeamNo = ""

                da1 = New SqlClient.SqlDataAdapter("Select Name1 as Beam_No, Meters1 as fororderby_beamno from " & Trim(Common_Procedures.EntryTempTable) & " where Name1 <> '' order by Meters1, Name1", con)
                Dt1 = New DataTable
                da1.Fill(Dt1)

                If Dt1.Rows.Count > 0 Then
                    FsNo = Dt1.Rows(0).Item("fororderby_beamno").ToString
                    LsNo = Dt1.Rows(0).Item("fororderby_beamno").ToString

                    FsBeamNo = Trim(UCase(Dt1.Rows(0).Item("Beam_No").ToString))
                    LsBeamNo = Trim(UCase(Dt1.Rows(0).Item("Beam_No").ToString))

                    For i = 1 To Dt1.Rows.Count - 1
                        If LsNo + 1 = Val(Dt1.Rows(i).Item("fororderby_beamno").ToString) Then
                            LsNo = Val(Dt1.Rows(i).Item("fororderby_beamno").ToString)
                            LsBeamNo = Trim(UCase(Dt1.Rows(i).Item("Beam_No").ToString))

                        Else
                            If FsNo = LsNo Then
                                vPvu_BmNo = vPvu_BmNo & Trim(FsBeamNo) & ","
                            Else
                                vPvu_BmNo = vPvu_BmNo & Trim(FsBeamNo) & "-" & Trim(LsBeamNo) & ","
                            End If
                            FsNo = Dt1.Rows(i).Item("fororderby_beamno").ToString
                            LsNo = Dt1.Rows(i).Item("fororderby_beamno").ToString

                            FsBeamNo = Trim(UCase(Dt1.Rows(i).Item("Beam_No").ToString))
                            LsBeamNo = Trim(UCase(Dt1.Rows(i).Item("Beam_No").ToString))

                        End If

                    Next

                    If FsNo = LsNo Then vPvu_BmNo = vPvu_BmNo & Trim(FsBeamNo) Else vPvu_BmNo = vPvu_BmNo & Trim(FsBeamNo) & "-" & Trim(LsBeamNo)

                End If
                Dt1.Clear()


                vPrn_PvuBmNos1 = Trim(vPvu_BmNo)
                vPrn_PvuBmNos2 = ""
                vPrn_PvuBmNos3 = ""
                vPrn_PvuBmNos4 = ""
                If Len(vPrn_PvuBmNos1) > 18 Then
                    For i = 18 To 1 Step -1
                        If Mid$(Trim(vPrn_PvuBmNos1), i, 1) = " " Or Mid$(Trim(vPrn_PvuBmNos1), i, 1) = "," Or Mid$(Trim(vPrn_PvuBmNos1), i, 1) = "." Or Mid$(Trim(vPrn_PvuBmNos1), i, 1) = "-" Or Mid$(Trim(vPrn_PvuBmNos1), i, 1) = "/" Or Mid$(Trim(vPrn_PvuBmNos1), i, 1) = "_" Or Mid$(Trim(vPrn_PvuBmNos1), i, 1) = "(" Or Mid$(Trim(vPrn_PvuBmNos1), i, 1) = ")" Or Mid$(Trim(vPrn_PvuBmNos1), i, 1) = "\" Or Mid$(Trim(vPrn_PvuBmNos1), i, 1) = "[" Or Mid$(Trim(vPrn_PvuBmNos1), i, 1) = "]" Or Mid$(Trim(vPrn_PvuBmNos1), i, 1) = "{" Or Mid$(Trim(vPrn_PvuBmNos1), i, 1) = "}" Then Exit For
                    Next i
                    If i = 0 Then i = 18
                    vPrn_PvuBmNos2 = Microsoft.VisualBasic.Right(Trim(vPrn_PvuBmNos1), Len(vPrn_PvuBmNos1) - i)
                    vPrn_PvuBmNos1 = Microsoft.VisualBasic.Left(Trim(vPrn_PvuBmNos1), i - 1)
                End If

                If Len(vPrn_PvuBmNos2) > 23 Then
                    For i = 23 To 1 Step -1
                        If Mid$(Trim(vPrn_PvuBmNos2), i, 1) = " " Or Mid$(Trim(vPrn_PvuBmNos2), i, 1) = "," Or Mid$(Trim(vPrn_PvuBmNos2), i, 1) = "." Or Mid$(Trim(vPrn_PvuBmNos2), i, 1) = "-" Or Mid$(Trim(vPrn_PvuBmNos2), i, 1) = "/" Or Mid$(Trim(vPrn_PvuBmNos2), i, 1) = "_" Or Mid$(Trim(vPrn_PvuBmNos2), i, 1) = "(" Or Mid$(Trim(vPrn_PvuBmNos2), i, 1) = ")" Or Mid$(Trim(vPrn_PvuBmNos2), i, 1) = "\" Or Mid$(Trim(vPrn_PvuBmNos2), i, 1) = "[" Or Mid$(Trim(vPrn_PvuBmNos2), i, 1) = "]" Or Mid$(Trim(vPrn_PvuBmNos2), i, 1) = "{" Or Mid$(Trim(vPrn_PvuBmNos2), i, 1) = "}" Then Exit For
                    Next i
                    If i = 0 Then i = 23
                    vPrn_PvuBmNos3 = Microsoft.VisualBasic.Right(Trim(vPrn_PvuBmNos2), Len(vPrn_PvuBmNos2) - i)
                    vPrn_PvuBmNos2 = Microsoft.VisualBasic.Left(Trim(vPrn_PvuBmNos2), i - 1)
                End If

                If Len(vPrn_PvuBmNos3) > 23 Then
                    For i = 23 To 1 Step -1
                        If Mid$(Trim(vPrn_PvuBmNos3), i, 1) = " " Or Mid$(Trim(vPrn_PvuBmNos3), i, 1) = "," Or Mid$(Trim(vPrn_PvuBmNos3), i, 1) = "." Or Mid$(Trim(vPrn_PvuBmNos3), i, 1) = "-" Or Mid$(Trim(vPrn_PvuBmNos3), i, 1) = "/" Or Mid$(Trim(vPrn_PvuBmNos3), i, 1) = "_" Or Mid$(Trim(vPrn_PvuBmNos3), i, 1) = "(" Or Mid$(Trim(vPrn_PvuBmNos3), i, 1) = ")" Or Mid$(Trim(vPrn_PvuBmNos3), i, 1) = "\" Or Mid$(Trim(vPrn_PvuBmNos3), i, 1) = "[" Or Mid$(Trim(vPrn_PvuBmNos3), i, 1) = "]" Or Mid$(Trim(vPrn_PvuBmNos3), i, 1) = "{" Or Mid$(Trim(vPrn_PvuBmNos3), i, 1) = "}" Then Exit For
                    Next i
                    If i = 0 Then i = 23
                    vPrn_PvuBmNos4 = Microsoft.VisualBasic.Right(Trim(vPrn_PvuBmNos3), Len(vPrn_PvuBmNos3) - i)
                    vPrn_PvuBmNos3 = Microsoft.VisualBasic.Left(Trim(vPrn_PvuBmNos3), i - 1)
                End If

                da1 = New SqlClient.SqlDataAdapter("select top 1 a.Yarn_Type, c.Count_Name, d.Mill_Name, b.Total_Bags, b.Total_Cones, b.Total_Weight, b.Total_Thiri from JobWork_Weaving_Yarn_Delivery_Details a INNER JOIN JobWork_Weaving_PavuYarn_Delivery_Head b ON a.JobWork_Weaving_PavuYarn_Delivery_Code = b.JobWork_Weaving_PavuYarn_Delivery_Code INNER JOIN Count_Head c on a.Count_IdNo = c.Count_IdNo LEFT OUTER JOIN Mill_Head d on a.Mill_IdNo = d.Mill_IdNo where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.JobWork_Weaving_PavuYarn_Delivery_Code = '" & Trim(NewCode) & "' Order by a.sl_no", con)
                prn_DetDt1 = New DataTable
                da1.Fill(prn_DetDt1)

                k = 0
                If prn_DetDt1.Rows.Count > 0 Then

                    For i = 0 To prn_DetDt1.Rows.Count - 1

                        If Val(prn_DetDt1.Rows(i).Item("Total_Weight").ToString) <> 0 Then
                            k = k + 1
                            prn_DetAr(k + 100, 1) = Trim(prn_DetDt1.Rows(i).Item("Mill_Name").ToString)
                            k = k + 1
                            prn_DetAr(k + 100, 1) = Trim(prn_DetDt1.Rows(i).Item("Count_Name").ToString)
                            k = k + 1
                            prn_DetAr(k + 100, 1) = Val(prn_DetDt1.Rows(i).Item("Total_Bags").ToString)
                            k = k + 1
                            prn_DetAr(k + 100, 1) = Val(prn_DetDt1.Rows(i).Item("Total_Cones").ToString)
                            k = k + 1
                            prn_DetAr(k + 100, 1) = Format(Val(prn_DetDt1.Rows(i).Item("Total_Weight").ToString), "#########0.000")
                            If Val(prn_DetDt1.Rows(i).Item("Total_Thiri").ToString) <> 0 Then
                                k = k + 1
                                prn_DetAr(k + 100, 1) = Format(Val(prn_DetDt1.Rows(i).Item("Total_Thiri").ToString), "#########0.000")
                            End If
                        End If

                    Next i

                End If
                Dt1.Clear()

                If k > prn_DetMxIndx Then prn_DetMxIndx = k

            Else
                MessageBox.Show("This is New Entry", "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

            End If

            da1.Dispose()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub PrintDocument1_PrintPage(ByVal sender As System.Object, ByVal e As System.Drawing.Printing.PrintPageEventArgs) Handles PrintDocument1.PrintPage
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim cmd As New SqlClient.SqlCommand
        Dim NewCode As String
        Dim i As Integer, k As Integer
        Dim vDup_SetNo As String
        Dim vPvu_BmNo As String, vDup_BmNo As String
        Dim W1 As Single = 0
        Dim FsNo As Single, LsNo As Single
        Dim FsBeamNo As String, LsBeamNo As String

        If prn_HdDt.Rows.Count <= 0 Then Exit Sub

        If prn_Prev_HeadIndx <> prn_HeadIndx Then

            NewCode = prn_HdDt.Rows(prn_HeadIndx).Item("JobWork_Weaving_PavuYarn_Delivery_Code").ToString

            prn_DetIndx = 0
            prn_DetSNo = 0
            prn_PageNo = 0
            prn_DetMxIndx = 0
            prn_NoofBmDets = 0
            prn_Count = 0

            Erase prn_HdAr
            Erase prn_DetAr

            prn_HdAr = New String(200, 10) {}
            prn_DetAr = New String(200, 10) {}

            da1 = New SqlClient.SqlDataAdapter("select a.*, b.*, c.Ledger_TinNo , c.Ledger_CstNo, c.Ledger_Name as Del_Name, c.Ledger_Address1, c.Ledger_Address2, c.Ledger_Address3, c.Ledger_Address4, d.Ledger_Name as Transport_Name, e.Area_Name, f.EndsCount_Name, g.Ledger_Name as Pavu_RecFrom_Name, h.Ledger_Name as Yarn_RecFrom_Name from JobWork_Weaving_PavuYarn_Delivery_Head a INNER JOIN Company_Head b ON a.Company_IdNo = b.Company_IdNo INNER JOIN Ledger_Head c ON a.DeliveryTo_Idno = c.Ledger_IdNo LEFT OUTER JOIN Ledger_Head d ON a.Transport_Idno = d.Ledger_IdNo  LEFT OUTER JOIN Area_Head e ON b.Area_Idno = e.Area_Idno LEFT OUTER JOIN EndsCount_Head f ON a.EndsCount_Idno = f.EndsCount_Idno LEFT OUTER JOIN Ledger_Head g ON a.ReceivedFrom_Idno = g.Ledger_IdNo LEFT OUTER JOIN Ledger_Head h ON a.Yarn_ReceivedFrom_Idno = h.Ledger_IdNo where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.JobWork_Weaving_PavuYarn_Delivery_Code = '" & Trim(NewCode) & "' Order by a.for_orderby, a.JobWork_Weaving_PavuYarn_Delivery_No", con)
            Dt1 = New DataTable
            da1.Fill(Dt1)

            If Dt1.Rows.Count > 0 Then

                da2 = New SqlClient.SqlDataAdapter("select a.*, b.Mill_Name, c.Count_Name from JobWork_Weaving_Yarn_Delivery_Details a LEFT OUTER JOIN Mill_Head b on a.Mill_IdNo = b.Mill_IdNo INNER JOIN Count_Head c on a.Count_IdNo = c.Count_IdNo where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.JobWork_Weaving_PavuYarn_Delivery_Code = '" & Trim(NewCode) & "' Order by a.sl_no", con)
                prn_DetDt = New DataTable
                da2.Fill(prn_DetDt)

                vPrn_PvuEdsCnt = ""
                vPrn_PvuTotBms = 0
                vPrn_PvuTotMtrs = 0 : vPrn_PvuNPcs = 0
                vPrn_PvuSetNo = "" : vDup_SetNo = ""
                vDup_BmNo = "" : vPvu_BmNo = ""
                vPrn_PvuBmNos1 = "" : vPrn_PvuBmNos2 = "" : vPrn_PvuBmNos3 = "" : vPrn_PvuBmNos4 = ""

                cmd.Connection = con

                cmd.CommandText = "truncate table " & Trim(Common_Procedures.EntryTempTable) & ""
                cmd.ExecuteNonQuery()

                da1 = New SqlClient.SqlDataAdapter("select a.*, b.* from JobWork_Weaving_Pavu_Delivery_Details a INNER JOIN EndsCount_Head b ON a.EndsCount_IdNo = b.EndsCount_IdNo where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.JobWork_Weaving_PavuYarn_Delivery_Code = '" & Trim(NewCode) & "' Order by a.sl_no", con)
                Dt1 = New DataTable
                da1.Fill(Dt1)

                If Dt1.Rows.Count > 0 Then

                    vPrn_PvuEdsCnt = Dt1.Rows(0).Item("EndsCount_Name").ToString

                    For i = 0 To Dt1.Rows.Count - 1

                        vPrn_PvuTotBms = Val(vPrn_PvuTotBms) + 1
                        vPrn_PvuTotMtrs = vPrn_PvuTotMtrs + Val(Dt1.Rows(i).Item("Meters").ToString)
                        vPrn_PvuNPcs = vPrn_PvuNPcs + Val(Dt1.Rows(i).Item("Noof_Used").ToString)

                        If InStr(1, "~" & Trim(UCase(vDup_SetNo)) & "~", "~" & Trim(UCase(Dt1.Rows(i).Item("Set_No").ToString)) & "~") = 0 Then
                            vDup_SetNo = Trim(vDup_SetNo) & "~" & Trim(Dt1.Rows(i).Item("Set_No").ToString) & "~"
                            vPrn_PvuSetNo = vPrn_PvuSetNo & IIf(Trim(vPrn_PvuSetNo) <> "", ", ", "") & Dt1.Rows(i).Item("Set_No").ToString
                        End If

                        If InStr(1, "~" & Trim(UCase(vDup_BmNo)) & "~", "~" & Trim(UCase(Dt1.Rows(i).Item("Set_No").ToString)) & "^" & Trim(UCase(Dt1.Rows(i).Item("Beam_No").ToString)) & "~") = 0 Then
                            vDup_BmNo = Trim(vDup_BmNo) & "~" & Trim(Dt1.Rows(i).Item("Set_No").ToString) & "^" & Trim(UCase(Dt1.Rows(i).Item("Beam_No").ToString)) & "~"

                            cmd.CommandText = "Insert into " & Trim(Common_Procedures.EntryTempTable) & "(Name1, Meters1) values ('" & Trim(Dt1.Rows(i).Item("Beam_No").ToString) & "', " & Common_Procedures.OrderBy_CodeToValue(Trim(Dt1.Rows(i).Item("Beam_No").ToString)) & " )"
                            cmd.ExecuteNonQuery()

                        End If

                    Next i

                End If

                Dt1.Clear()

                '--

                da1 = New SqlClient.SqlDataAdapter("select a.*, b.* from JobWork_Weaving_Pavu_Delivery_Details a INNER JOIN EndsCount_Head b ON a.EndsCount_IdNo = b.EndsCount_IdNo where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.JobWork_Weaving_PavuYarn_Delivery_Code = '" & Trim(NewCode) & "' Order by a.sl_no", con)
                prn_DetDt1 = New DataTable
                da1.Fill(prn_DetDt1)

                If prn_DetDt1.Rows.Count > 0 Then
                    For i = 0 To prn_DetDt1.Rows.Count - 1

                        If Val(prn_DetDt1.Rows(i).Item("Meters").ToString) <> 0 Then
                            prn_DetMxIndx = prn_DetMxIndx + 1
                            prn_DetAr(prn_DetMxIndx, 1) = Trim(prn_DetDt1.Rows(i).Item("Ends_Name").ToString)
                            prn_DetAr(prn_DetMxIndx, 2) = Trim(prn_DetDt1.Rows(i).Item("Beam_No").ToString)
                            prn_DetAr(prn_DetMxIndx, 3) = Val(prn_DetDt1.Rows(i).Item("Pcs").ToString)
                            prn_DetAr(prn_DetMxIndx, 4) = Format(Val(prn_DetDt1.Rows(i).Item("Meters").ToString), "#########0.00")
                        End If

                    Next i

                End If
                Dt1.Clear()

                If Val(prn_HdDt.Rows(prn_HeadIndx).Item("Meters").ToString) <> 0 Then
                    prn_DetMxIndx = prn_DetMxIndx + 1
                    prn_DetAr(prn_DetMxIndx, 1) = Trim(prn_HdDt.Rows(prn_HeadIndx).Item("EndsCount_Name").ToString)
                    prn_DetAr(prn_DetMxIndx, 2) = ""
                    prn_DetAr(prn_DetMxIndx, 3) = ""
                    prn_DetAr(prn_DetMxIndx, 4) = Format(Val(prn_HdDt.Rows(prn_HeadIndx).Item("Meters").ToString), "#########0.00")
                End If

                'prn_DetMxIndx = prn_DetMxIndx + 1
                'prn_DetAr(prn_DetMxIndx, 1) = "--------------------"
                'prn_DetAr(prn_DetMxIndx, 2) = "--------------------"
                'prn_DetAr(prn_DetMxIndx, 3) = "--------------------"
                'prn_DetAr(prn_DetMxIndx, 4) = "--------------------"


                'prn_DetMxIndx = prn_DetMxIndx + 1
                ''prn_DetAr(prn_DetMxIndx, 1) = Trim(prn_DetDt1.Rows(i).Item("EndsCount_Name").ToString)
                ''prn_DetAr(prn_DetMxIndx, 2) = Trim(prn_DetDt1.Rows(i).Item("Beam_No").ToString)
                ''prn_DetAr(prn_DetMxIndx, 3) = Val(prn_DetDt1.Rows(i).Item("Pcs").ToString)
                ''prn_DetAr(prn_DetMxIndx, 4) = Format(Val(prn_DetDt1.Rows(i).Item("Meters").ToString), "#########0.00")


                'prn_DetMxIndx = prn_DetMxIndx + 1
                'prn_DetAr(prn_DetMxIndx, 1) = "--------------------"
                'prn_DetAr(prn_DetMxIndx, 2) = "--------------------"
                'prn_DetAr(prn_DetMxIndx, 3) = "--------------------"
                'prn_DetAr(prn_DetMxIndx, 4) = "--------------------"

                '--

                vPvu_BmNo = ""
                FsNo = 0 : LsNo = 0
                FsBeamNo = "" : LsBeamNo = ""

                da1 = New SqlClient.SqlDataAdapter("Select Name1 as Beam_No, Meters1 as fororderby_beamno from " & Trim(Common_Procedures.EntryTempTable) & " where Name1 <> '' order by Meters1, Name1", con)
                Dt1 = New DataTable
                da1.Fill(Dt1)

                If Dt1.Rows.Count > 0 Then
                    FsNo = Dt1.Rows(0).Item("fororderby_beamno").ToString
                    LsNo = Dt1.Rows(0).Item("fororderby_beamno").ToString

                    FsBeamNo = Trim(UCase(Dt1.Rows(0).Item("Beam_No").ToString))
                    LsBeamNo = Trim(UCase(Dt1.Rows(0).Item("Beam_No").ToString))

                    For i = 1 To Dt1.Rows.Count - 1
                        If LsNo + 1 = Val(Dt1.Rows(i).Item("fororderby_beamno").ToString) Then
                            LsNo = Val(Dt1.Rows(i).Item("fororderby_beamno").ToString)
                            LsBeamNo = Trim(UCase(Dt1.Rows(i).Item("Beam_No").ToString))

                        Else
                            If FsNo = LsNo Then
                                vPvu_BmNo = vPvu_BmNo & Trim(FsBeamNo) & ","
                            Else
                                vPvu_BmNo = vPvu_BmNo & Trim(FsBeamNo) & "-" & Trim(LsBeamNo) & ","
                            End If
                            FsNo = Dt1.Rows(i).Item("fororderby_beamno").ToString
                            LsNo = Dt1.Rows(i).Item("fororderby_beamno").ToString

                            FsBeamNo = Trim(UCase(Dt1.Rows(i).Item("Beam_No").ToString))
                            LsBeamNo = Trim(UCase(Dt1.Rows(i).Item("Beam_No").ToString))

                        End If

                    Next

                    If FsNo = LsNo Then vPvu_BmNo = vPvu_BmNo & Trim(FsBeamNo) Else vPvu_BmNo = vPvu_BmNo & Trim(FsBeamNo) & "-" & Trim(LsBeamNo)

                End If
                Dt1.Clear()


                vPrn_PvuBmNos1 = Trim(vPvu_BmNo)
                vPrn_PvuBmNos2 = ""
                vPrn_PvuBmNos3 = ""
                vPrn_PvuBmNos4 = ""
                If Len(vPrn_PvuBmNos1) > 18 Then
                    For i = 18 To 1 Step -1
                        If Mid$(Trim(vPrn_PvuBmNos1), i, 1) = " " Or Mid$(Trim(vPrn_PvuBmNos1), i, 1) = "," Or Mid$(Trim(vPrn_PvuBmNos1), i, 1) = "." Or Mid$(Trim(vPrn_PvuBmNos1), i, 1) = "-" Or Mid$(Trim(vPrn_PvuBmNos1), i, 1) = "/" Or Mid$(Trim(vPrn_PvuBmNos1), i, 1) = "_" Or Mid$(Trim(vPrn_PvuBmNos1), i, 1) = "(" Or Mid$(Trim(vPrn_PvuBmNos1), i, 1) = ")" Or Mid$(Trim(vPrn_PvuBmNos1), i, 1) = "\" Or Mid$(Trim(vPrn_PvuBmNos1), i, 1) = "[" Or Mid$(Trim(vPrn_PvuBmNos1), i, 1) = "]" Or Mid$(Trim(vPrn_PvuBmNos1), i, 1) = "{" Or Mid$(Trim(vPrn_PvuBmNos1), i, 1) = "}" Then Exit For
                    Next i
                    If i = 0 Then i = 18
                    vPrn_PvuBmNos2 = Microsoft.VisualBasic.Right(Trim(vPrn_PvuBmNos1), Len(vPrn_PvuBmNos1) - i)
                    vPrn_PvuBmNos1 = Microsoft.VisualBasic.Left(Trim(vPrn_PvuBmNos1), i - 1)
                End If

                If Len(vPrn_PvuBmNos2) > 23 Then
                    For i = 23 To 1 Step -1
                        If Mid$(Trim(vPrn_PvuBmNos2), i, 1) = " " Or Mid$(Trim(vPrn_PvuBmNos2), i, 1) = "," Or Mid$(Trim(vPrn_PvuBmNos2), i, 1) = "." Or Mid$(Trim(vPrn_PvuBmNos2), i, 1) = "-" Or Mid$(Trim(vPrn_PvuBmNos2), i, 1) = "/" Or Mid$(Trim(vPrn_PvuBmNos2), i, 1) = "_" Or Mid$(Trim(vPrn_PvuBmNos2), i, 1) = "(" Or Mid$(Trim(vPrn_PvuBmNos2), i, 1) = ")" Or Mid$(Trim(vPrn_PvuBmNos2), i, 1) = "\" Or Mid$(Trim(vPrn_PvuBmNos2), i, 1) = "[" Or Mid$(Trim(vPrn_PvuBmNos2), i, 1) = "]" Or Mid$(Trim(vPrn_PvuBmNos2), i, 1) = "{" Or Mid$(Trim(vPrn_PvuBmNos2), i, 1) = "}" Then Exit For
                    Next i
                    If i = 0 Then i = 23
                    vPrn_PvuBmNos3 = Microsoft.VisualBasic.Right(Trim(vPrn_PvuBmNos2), Len(vPrn_PvuBmNos2) - i)
                    vPrn_PvuBmNos2 = Microsoft.VisualBasic.Left(Trim(vPrn_PvuBmNos2), i - 1)
                End If

                If Len(vPrn_PvuBmNos3) > 23 Then
                    For i = 23 To 1 Step -1
                        If Mid$(Trim(vPrn_PvuBmNos3), i, 1) = " " Or Mid$(Trim(vPrn_PvuBmNos3), i, 1) = "," Or Mid$(Trim(vPrn_PvuBmNos3), i, 1) = "." Or Mid$(Trim(vPrn_PvuBmNos3), i, 1) = "-" Or Mid$(Trim(vPrn_PvuBmNos3), i, 1) = "/" Or Mid$(Trim(vPrn_PvuBmNos3), i, 1) = "_" Or Mid$(Trim(vPrn_PvuBmNos3), i, 1) = "(" Or Mid$(Trim(vPrn_PvuBmNos3), i, 1) = ")" Or Mid$(Trim(vPrn_PvuBmNos3), i, 1) = "\" Or Mid$(Trim(vPrn_PvuBmNos3), i, 1) = "[" Or Mid$(Trim(vPrn_PvuBmNos3), i, 1) = "]" Or Mid$(Trim(vPrn_PvuBmNos3), i, 1) = "{" Or Mid$(Trim(vPrn_PvuBmNos3), i, 1) = "}" Then Exit For
                    Next i
                    If i = 0 Then i = 23
                    vPrn_PvuBmNos4 = Microsoft.VisualBasic.Right(Trim(vPrn_PvuBmNos3), Len(vPrn_PvuBmNos3) - i)
                    vPrn_PvuBmNos3 = Microsoft.VisualBasic.Left(Trim(vPrn_PvuBmNos3), i - 1)
                End If

                da1 = New SqlClient.SqlDataAdapter("select top 1 a.Yarn_Type, c.Count_Name, d.Mill_Name, b.Total_Bags, b.Total_Cones, b.Total_Weight, b.Total_Thiri from JobWork_Weaving_Yarn_Delivery_Details a INNER JOIN JobWork_Weaving_PavuYarn_Delivery_Head b ON a.JobWork_Weaving_PavuYarn_Delivery_Code = b.JobWork_Weaving_PavuYarn_Delivery_Code INNER JOIN Count_Head c on a.Count_IdNo = c.Count_IdNo LEFT OUTER JOIN Mill_Head d on a.Mill_IdNo = d.Mill_IdNo where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.JobWork_Weaving_PavuYarn_Delivery_Code = '" & Trim(NewCode) & "' Order by a.sl_no", con)
                prn_DetDt1 = New DataTable
                da1.Fill(prn_DetDt1)

                k = 0
                If prn_DetDt1.Rows.Count > 0 Then

                    For i = 0 To prn_DetDt1.Rows.Count - 1

                        If Val(prn_DetDt1.Rows(i).Item("Total_Weight").ToString) <> 0 Then
                            k = k + 1
                            prn_DetAr(k + 100, 1) = Trim(prn_DetDt1.Rows(i).Item("Mill_Name").ToString)
                            k = k + 1
                            prn_DetAr(k + 100, 1) = Trim(prn_DetDt1.Rows(i).Item("Count_Name").ToString)
                            k = k + 1
                            prn_DetAr(k + 100, 1) = Val(prn_DetDt1.Rows(i).Item("Total_Bags").ToString)
                            k = k + 1
                            prn_DetAr(k + 100, 1) = Val(prn_DetDt1.Rows(i).Item("Total_Cones").ToString)
                            k = k + 1
                            prn_DetAr(k + 100, 1) = Format(Val(prn_DetDt1.Rows(i).Item("Total_Weight").ToString), "#########0.000")
                            If Val(prn_DetDt1.Rows(i).Item("Total_Thiri").ToString) <> 0 Then
                                k = k + 1
                                prn_DetAr(k + 100, 1) = Format(Val(prn_DetDt1.Rows(i).Item("Total_Thiri").ToString), "#########0.000")
                            End If
                        End If

                    Next i

                End If
                Dt1.Clear()

                If k > prn_DetMxIndx Then prn_DetMxIndx = k

            Else
                MessageBox.Show("This is New Entry", "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Exit Sub

            End If

            Dt1.Clear()

            Dt1.Dispose()
            da1.Dispose()

        End If

        If prn_Status = 1 Then
            If dgv_YarnDetails.Columns(7).Visible = True Then
                Printing_Format2(e)

            Else
                Printing_Format3(e)
                'Printing_Format1(e)

            End If

        Else
            Printing_FormJJ(e)

        End If


    End Sub

    Private Sub btn_Print_Cancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Print_Cancel.Click
        btn_print_Close_Click(sender, e)
    End Sub

    Private Sub btn_print_Close_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Close_Print.Click
        pnl_Back.Enabled = True
        pnl_Print.Visible = False
    End Sub

    Private Sub btn_Print_Invoice_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Print_Invoice.Click
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1040" Then '---- M.S Textiles (Tirupur)

            txt_PrintRange_FromNo.Text = prn_FromNo
            txt_PrintRange_ToNo.Text = prn_ToNo

            pnl_PrintRange.Visible = True
            pnl_Back.Enabled = False
            pnl_Print.Visible = False

            If txt_PrintRange_FromNo.Enabled Then txt_PrintRange_FromNo.Focus()

        Else
            prn_Status = 1
            Printing_Delivery()
            btn_print_Close_Click(sender, e)

        End If

    End Sub

    Private Sub btn_Print_Preprint_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Print_Bale.Click
        prn_Status = 2
        Printing_FormJJ()
        btn_print_Close_Click(sender, e)
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

        'PrintDocument pd = new PrintDocument();
        'pd.DefaultPageSettings.PaperSize = new System.Drawing.Printing.PaperSize("PaperA4", 840, 1180);
        'pd.Print();

        PpSzSTS = False

        For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
            ps = PrintDocument1.PrinterSettings.PaperSizes(I)
            'Debug.Print(ps.PaperName)
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

        NoofItems_PerPage = 8 ' 6

        Erase LnAr
        Erase ClArr

        LnAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        ClArr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}

        ClArr(1) = Val(35) : ClArr(2) = 50 : ClArr(3) = 130 : ClArr(4) = 65 : ClArr(5) = 65 : ClArr(6) = 70 : ClArr(7) = 85
        ClArr(8) = PageWidth - (LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7))

        TxtHgt = 18 ' 19 ' e.Graphics.MeasureString("A", pFont).Height  ' 20

        EntryCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Try

            prn_Prev_HeadIndx = prn_HeadIndx

            If prn_HdDt.Rows.Count > 0 Then

                If prn_HeadIndx <= prn_HdDt.Rows.Count - 1 Then

                    Printing_Format1_PageHeader(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, prn_PageNo, NoofItems_PerPage, CurY, LnAr, ClArr)


                    W1 = e.Graphics.MeasureString("No.of Beams  : ", pFont).Width

                    NoofDets = 0

                    CurY = CurY - 10

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

                            ItmNm1 = Trim(prn_DetDt.Rows(prn_DetIndx).Item("Mill_Name").ToString)
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

                            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetDt.Rows(prn_DetIndx).Item("Sl_No").ToString), LMargin + 15, CurY, 0, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Yarn_Type").ToString, LMargin + ClArr(1) + 10, CurY, 0, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm1), LMargin + ClArr(1) + ClArr(2) + 10, CurY, 0, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Count_Name").ToString, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + 10, CurY, 0, 0, pFont)
                            If Val(prn_DetDt.Rows(prn_DetIndx).Item("Bags").ToString) <> 0 Then
                                Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(prn_DetIndx).Item("Bags").ToString), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) - 10, CurY, 1, 0, pFont)
                            End If
                            If Val(prn_DetDt.Rows(prn_DetIndx).Item("Cones").ToString) <> 0 Then
                                Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(prn_DetIndx).Item("Cones").ToString), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) - 10, CurY, 1, 0, pFont)
                            End If
                            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Weight").ToString), "########0.000"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) - 10, CurY, 1, 0, pFont)

                            If prn_DetIndx = 0 Then
                                Common_Procedures.Print_To_PrintDocument(e, "Ends Count", LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + 10, CurY, 0, 0, pFont)
                                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + W1 + 10, CurY, 0, 0, pFont)
                                Common_Procedures.Print_To_PrintDocument(e, Trim(vPrn_PvuEdsCnt), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + W1 + 25, CurY, 0, 0, pFont)

                            ElseIf prn_DetIndx = 1 Then
                                Common_Procedures.Print_To_PrintDocument(e, "No.of Beams", LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + 10, CurY, 0, 0, pFont)
                                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + W1 + 10, CurY, 0, 0, pFont)
                                Common_Procedures.Print_To_PrintDocument(e, Trim(Val(vPrn_PvuTotBms)), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + W1 + 25, CurY, 0, 0, pFont)

                            ElseIf prn_DetIndx = 2 Then
                                Common_Procedures.Print_To_PrintDocument(e, "Meters", LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + 10, CurY, 0, 0, pFont)
                                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + W1 + 10, CurY, 0, 0, pFont)
                                Common_Procedures.Print_To_PrintDocument(e, Trim(Format(Val(vPrn_PvuTotMtrs), "#########0.00")), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + W1 + 25, CurY, 0, 0, pFont)

                            ElseIf prn_DetIndx = 3 Then
                                Common_Procedures.Print_To_PrintDocument(e, "Noof_Used", LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + 10, CurY, 0, 0, pFont)
                                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + W1 + 10, CurY, 0, 0, pFont)
                                Common_Procedures.Print_To_PrintDocument(e, Trim(Val(vPrn_PvuNPcs)), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + W1 + 25, CurY, 0, 0, pFont)


                            ElseIf prn_DetIndx = 4 Then
                                Common_Procedures.Print_To_PrintDocument(e, "Set No", LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + 10, CurY, 0, 0, pFont)
                                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + W1 + 10, CurY, 0, 0, pFont)
                                Common_Procedures.Print_To_PrintDocument(e, Trim(vPrn_PvuSetNo), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + W1 + 25, CurY, 0, 0, pFont)

                            ElseIf prn_DetIndx = 5 Then
                                Common_Procedures.Print_To_PrintDocument(e, "Beam No", LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + 10, CurY, 0, 0, pFont)
                                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + W1 + 10, CurY, 0, 0, pFont)
                                Common_Procedures.Print_To_PrintDocument(e, Trim(vPrn_PvuBmNos1), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + W1 + 25, CurY, 0, 0, pFont)

                            ElseIf prn_DetIndx = 6 Then
                                Common_Procedures.Print_To_PrintDocument(e, Trim(vPrn_PvuBmNos2), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + 50, CurY, 0, 0, pFont)

                            ElseIf prn_DetIndx = 7 Then
                                Common_Procedures.Print_To_PrintDocument(e, Trim(vPrn_PvuBmNos3), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + 50, CurY, 0, 0, pFont)

                            ElseIf prn_DetIndx = 8 Then
                                Common_Procedures.Print_To_PrintDocument(e, Trim(vPrn_PvuBmNos4), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + 50, CurY, 0, 0, pFont)

                            End If

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

                End If

            End If

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        prn_HeadIndx = prn_HeadIndx + 1

        If prn_HeadIndx <= prn_HdDt.Rows.Count - 1 Then
            e.HasMorePages = True
        Else
            e.HasMorePages = False
        End If

    End Sub

    Private Sub Printing_Format1_PageHeader(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByRef PageNo As Integer, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single)
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt2 As New DataTable
        Dim p1Font As Font
        Dim strHeight As Single
        Dim C1 As Single, W1 As Single, S1 As Single
        Dim Cmp_Name As String, Cmp_Add1 As String, Cmp_Add2 As String
        Dim Cmp_PhNo As String, Cmp_TinNo As String, Cmp_CstNo As String

        PageNo = PageNo + 1

        CurY = TMargin

        da2 = New SqlClient.SqlDataAdapter("select a.*, b.Mill_Name, c.Count_Name  from JobWork_Weaving_Yarn_Delivery_Details a INNER JOIN Mill_Head b on a.Mill_IdNo = b.Mill_IdNo LEFT OUTER JOIN Count_Head c on a.Count_IdNo = c.Count_IdNo where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.JobWork_Weaving_PavuYarn_Delivery_Code = '" & Trim(EntryCode) & "' Order by a.sl_no", con)
        da2.Fill(dt2)

        If dt2.Rows.Count > NoofItems_PerPage Then
            Common_Procedures.Print_To_PrintDocument(e, "Page : " & Trim(Val(PageNo)), PageWidth - 10, CurY - TxtHgt, 1, 0, pFont)
        End If
        dt2.Clear()

        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(1) = CurY

        Cmp_Name = "" : Cmp_Add1 = "" : Cmp_Add2 = ""
        Cmp_PhNo = "" : Cmp_TinNo = "" : Cmp_CstNo = ""

        Cmp_Name = prn_HdDt.Rows(prn_HeadIndx).Item("Company_Name").ToString
        Cmp_Add1 = prn_HdDt.Rows(prn_HeadIndx).Item("Company_Address1").ToString & " " & prn_HdDt.Rows(prn_HeadIndx).Item("Company_Address2").ToString
        Cmp_Add2 = prn_HdDt.Rows(prn_HeadIndx).Item("Company_Address3").ToString & " " & prn_HdDt.Rows(prn_HeadIndx).Item("Company_Address4").ToString
        If Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Company_PhoneNo").ToString) <> "" Then
            Cmp_PhNo = "PHONE NO.:" & prn_HdDt.Rows(prn_HeadIndx).Item("Company_PhoneNo").ToString
        End If
        If Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Company_TinNo").ToString) <> "" Then
            Cmp_TinNo = "TIN NO.: " & prn_HdDt.Rows(prn_HeadIndx).Item("Company_TinNo").ToString
        End If
        If Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Company_CstNo").ToString) <> "" Then
            Cmp_CstNo = "CST NO.: " & prn_HdDt.Rows(prn_HeadIndx).Item("Company_CstNo").ToString
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
        CurY = CurY + TxtHgt - 1
        Common_Procedures.Print_To_PrintDocument(e, Cmp_TinNo, LMargin + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, Cmp_CstNo, PageWidth - 10, CurY, 1, 0, pFont)

        CurY = CurY + TxtHgt - 5
        p1Font = New Font("Calibri", 16, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "DELIVERY", LMargin, CurY, 2, PrintWidth, p1Font)
        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height

        'CurY = CurY + TxtHgt

        CurY = CurY + strHeight + 10 ' + 150
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(2) = CurY

        Try
            C1 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6)
            W1 = e.Graphics.MeasureString("D.C.NO    : ", pFont).Width
            S1 = e.Graphics.MeasureString("FROM  :    ", pFont).Width

            CurY = CurY + TxtHgt - 10
            p1Font = New Font("Calibri", 12, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "FROM  :  " & "M/s." & prn_HdDt.Rows(prn_HeadIndx).Item("Del_Name").ToString, LMargin + 10, CurY, 0, 0, p1Font)
            Common_Procedures.Print_To_PrintDocument(e, "DC.NO", LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(prn_HeadIndx).Item("JobWork_Weaving_PavuYarn_Delivery_No").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, p1Font)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(prn_HeadIndx).Item("Ledger_Address1").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
            p1Font = New Font("Calibri", 14, FontStyle.Bold)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(prn_HeadIndx).Item("Ledger_Address2").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "DATE", LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(prn_HeadIndx).Item("JobWork_Weaving_PavuYarn_Delivery_Date").ToString), "dd-MM-yyyy").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(prn_HeadIndx).Item("Ledger_Address3").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(prn_HeadIndx).Item("Ledger_Address4").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
            'If Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Party_DcNo").ToString) <> "" Then
            '    Common_Procedures.Print_To_PrintDocument(e, "PARTY D.C.NO", LMargin + C1 + 10, CurY, 0, 0, pFont)
            '    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
            '    Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(prn_HeadIndx).Item("Party_DcNo").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, pFont)
            'End If
            CurY = CurY + TxtHgt + 5
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(3) = CurY

            e.Graphics.DrawLine(Pens.Black, LMargin + C1, LnAr(3), LMargin + C1, LnAr(2))

            CurY = CurY + TxtHgt - 10
            Common_Procedures.Print_To_PrintDocument(e, "SNO", LMargin, CurY, 2, ClAr(1), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "TYPE", LMargin + ClAr(1), CurY, 2, ClAr(2), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "MILL NAME", LMargin + ClAr(1) + ClAr(2), CurY, 2, ClAr(3), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "COUNT", LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, 2, ClAr(4), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "BAGS", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 2, ClAr(5), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "CONES", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, 2, ClAr(6), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "WEIGHT", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, 2, ClAr(7), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "PAVU DETAILS", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, 2, ClAr(8), pFont)

            CurY = CurY + TxtHgt + 10
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(4) = CurY

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub Printing_Format1_PageFooter(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single, ByVal NoofDets As Integer, ByVal is_LastPage As Boolean)
        Dim p1Font As Font
        Dim I As Integer
        Dim Cmp_Name As String
        Dim W1 As Single = 0

        W1 = e.Graphics.MeasureString("No.of Beams  : ", pFont).Width

        Try

            For I = NoofDets + 1 To NoofItems_PerPage

                CurY = CurY + TxtHgt



                If prn_DetIndx = 0 Then
                    Common_Procedures.Print_To_PrintDocument(e, "Ends Count", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + 10, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + W1 + 10, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, Trim(vPrn_PvuEdsCnt), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + W1 + 25, CurY, 0, 0, pFont)

                ElseIf prn_DetIndx = 1 Then
                    Common_Procedures.Print_To_PrintDocument(e, "No.of Beams", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + 10, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + W1 + 10, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, Trim(Val(vPrn_PvuTotBms)), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + W1 + 25, CurY, 0, 0, pFont)

                ElseIf prn_DetIndx = 2 Then
                    Common_Procedures.Print_To_PrintDocument(e, "Meters", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + 10, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + W1 + 10, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, Trim(Format(Val(vPrn_PvuTotMtrs), "#########0.00")), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + W1 + 25, CurY, 0, 0, pFont)

                ElseIf prn_DetIndx = 3 Then
                    Common_Procedures.Print_To_PrintDocument(e, "Noof_Used", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + 10, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + W1 + 10, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, Trim(Val(vPrn_PvuNPcs)), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + W1 + 25, CurY, 0, 0, pFont)


                ElseIf prn_DetIndx = 4 Then
                    Common_Procedures.Print_To_PrintDocument(e, "Set No", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + 10, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + W1 + 10, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, Trim(vPrn_PvuSetNo), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + W1 + 25, CurY, 0, 0, pFont)

                ElseIf prn_DetIndx = 5 Then
                    Common_Procedures.Print_To_PrintDocument(e, "Beam No", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + 10, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + W1 + 10, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, Trim(vPrn_PvuBmNos1), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + W1 + 25, CurY, 0, 0, pFont)

                ElseIf prn_DetIndx = 6 Then
                    Common_Procedures.Print_To_PrintDocument(e, Trim(vPrn_PvuBmNos2), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + 50, CurY, 0, 0, pFont)

                ElseIf prn_DetIndx = 7 Then
                    Common_Procedures.Print_To_PrintDocument(e, Trim(vPrn_PvuBmNos3), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + 50, CurY, 0, 0, pFont)

                ElseIf prn_DetIndx = 8 Then
                    Common_Procedures.Print_To_PrintDocument(e, Trim(vPrn_PvuBmNos4), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + 50, CurY, 0, 0, pFont)

                End If

                prn_DetIndx = prn_DetIndx + 1

            Next

            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(5) = CurY

            CurY = CurY + TxtHgt - 10
            If is_LastPage = True Then
                Common_Procedures.Print_To_PrintDocument(e, " TOTAL", LMargin + ClAr(1) + ClAr(2) + 30, CurY, 2, ClAr(4), pFont)
            End If

            If Val(prn_HdDt.Rows(prn_HeadIndx).Item("Total_Bags").ToString) <> 0 Then
                If is_LastPage = True Then
                    Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(prn_HeadIndx).Item("Total_Bags").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 1, 0, pFont)
                End If
            End If
            If Val(prn_HdDt.Rows(prn_HeadIndx).Item("Total_Cones").ToString) <> 0 Then
                If is_LastPage = True Then
                    Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(prn_HeadIndx).Item("Total_Cones").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY, 1, 0, pFont)
                End If
            End If
            If Val(prn_HdDt.Rows(prn_HeadIndx).Item("Total_Weight").ToString) <> 0 Then
                If is_LastPage = True Then
                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(prn_HeadIndx).Item("Total_Weight").ToString), "#########0.000"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, pFont)
                End If
            End If


            CurY = CurY + TxtHgt - 15

            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(6) = CurY

            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1), CurY, LMargin + ClAr(1), LnAr(3))

            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2), CurY, LMargin + ClAr(1) + ClAr(2), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), LnAr(3))
            'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), LnAr(3))
            'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9), LnAr(3))

            'CurY = CurY + TxtHgt - 5

            'Common_Procedures.Print_To_PrintDocument(e, "Through : " & Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Vehicle_No").ToString), LMargin + 10, CurY, 0, 0, pFont)
            'If Val(prn_HdDt.Rows(prn_HeadIndx).Item("Empty_Beam").ToString) <> 0 Then
            '    Common_Procedures.Print_To_PrintDocument(e, " Empty Beams : " & Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Empty_Beam").ToString), PageWidth - 250, CurY, 0, 0, pFont)
            'End If

            'CurY = CurY + TxtHgt + 10
            'e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            'LnAr(7) = CurY

            CurY = CurY + TxtHgt
            'If Val(Common_Procedures.User.IdNo) <> 1 Then
            '    Common_Procedures.Print_To_PrintDocument(e, "(" & Trim(Common_Procedures.User.Name) & ")", LMargin + 400, CurY, 0, 0, pFont)
            'End If

            'CurY = CurY + TxtHgt
            CurY = CurY + TxtHgt
            Cmp_Name = prn_HdDt.Rows(prn_HeadIndx).Item("Company_Name").ToString
            Common_Procedures.Print_To_PrintDocument(e, "Receiver's Signature", LMargin + 20, CurY, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, "Prepared By ", LMargin + 300, CurY, 0, 0, pFont)
            p1Font = New Font("Calibri", 12, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "For " & Cmp_Name, PageWidth - 15, CurY, 1, 0, p1Font)

            CurY = CurY + TxtHgt + 10

            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            e.Graphics.DrawLine(Pens.Black, LMargin, LnAr(1), LMargin, CurY)
            e.Graphics.DrawLine(Pens.Black, PageWidth, LnAr(1), PageWidth, CurY)

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    'With Thiri
    Private Sub Printing_Format2(ByRef e As System.Drawing.Printing.PrintPageEventArgs)
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

        'PrintDocument pd = new PrintDocument();
        'pd.DefaultPageSettings.PaperSize = new System.Drawing.Printing.PaperSize("PaperA4", 840, 1180);
        'pd.Print();

        For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
            ps = PrintDocument1.PrinterSettings.PaperSizes(I)
            'Debug.Print(ps.PaperName)
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

        NoofItems_PerPage = 8 ' 6

        Erase LnAr
        Erase ClArr

        LnAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        ClArr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}

        ClArr(1) = Val(35) : ClArr(2) = 50 : ClArr(3) = 130 : ClArr(4) = 65 : ClArr(5) = 65 : ClArr(6) = 70 : ClArr(7) = 85
        ClArr(8) = PageWidth - (LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7))

        TxtHgt = 18 ' 19 ' e.Graphics.MeasureString("A", pFont).Height  ' 20

        EntryCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Try

            prn_Prev_HeadIndx = prn_HeadIndx

            If prn_HdDt.Rows.Count > 0 Then

                Printing_Format2_PageHeader(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, prn_PageNo, NoofItems_PerPage, CurY, LnAr, ClArr)


                W1 = e.Graphics.MeasureString("No.of Beams  : ", pFont).Width

                NoofDets = 0

                CurY = CurY - 10

                If prn_DetDt.Rows.Count > 0 Then

                    Do While prn_DetIndx <= prn_DetDt.Rows.Count - 1

                        If NoofDets >= NoofItems_PerPage Then
                            CurY = CurY + TxtHgt

                            Common_Procedures.Print_To_PrintDocument(e, "Continued....", PageWidth - 10, CurY, 1, 0, pFont)

                            NoofDets = NoofDets + 1

                            Printing_Format2_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, NoofDets, False)

                            e.HasMorePages = True
                            Return

                        End If

                        prn_DetSNo = prn_DetSNo + 1

                        ItmNm1 = Trim(prn_DetDt.Rows(prn_DetIndx).Item("Mill_Name").ToString)
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

                        Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetDt.Rows(prn_DetIndx).Item("Sl_No").ToString), LMargin + 15, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Yarn_Type").ToString, LMargin + ClArr(1) + 10, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm1), LMargin + ClArr(1) + ClArr(2) + 10, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Count_Name").ToString, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + 10, CurY, 0, 0, pFont)
                        If Val(prn_DetDt.Rows(prn_DetIndx).Item("Bags").ToString) <> 0 Then
                            Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(prn_DetIndx).Item("Bags").ToString), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) - 10, CurY, 1, 0, pFont)
                        End If
                        If Val(prn_DetDt.Rows(prn_DetIndx).Item("Cones").ToString) <> 0 Then
                            Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(prn_DetIndx).Item("Cones").ToString), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) - 10, CurY, 1, 0, pFont)
                        End If
                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Thiri").ToString), "########0.000"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) - 10, CurY, 1, 0, pFont)

                        If prn_DetIndx = 0 Then
                            Common_Procedures.Print_To_PrintDocument(e, "Ends Count", LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + 10, CurY, 0, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + W1 + 10, CurY, 0, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, Trim(vPrn_PvuEdsCnt), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + W1 + 25, CurY, 0, 0, pFont)

                        ElseIf prn_DetIndx = 1 Then
                            Common_Procedures.Print_To_PrintDocument(e, "No.of Beams", LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + 10, CurY, 0, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + W1 + 10, CurY, 0, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, Trim(Val(vPrn_PvuTotBms)), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + W1 + 25, CurY, 0, 0, pFont)

                        ElseIf prn_DetIndx = 2 Then
                            Common_Procedures.Print_To_PrintDocument(e, "Meters", LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + 10, CurY, 0, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + W1 + 10, CurY, 0, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, Trim(Format(Val(vPrn_PvuTotMtrs), "#########0.00")), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + W1 + 25, CurY, 0, 0, pFont)

                        ElseIf prn_DetIndx = 3 Then
                            Common_Procedures.Print_To_PrintDocument(e, "Noof_Used", LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + 10, CurY, 0, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + W1 + 10, CurY, 0, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, Trim(Val(vPrn_PvuNPcs)), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + W1 + 25, CurY, 0, 0, pFont)


                        ElseIf prn_DetIndx = 4 Then
                            Common_Procedures.Print_To_PrintDocument(e, "Set No", LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + 10, CurY, 0, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + W1 + 10, CurY, 0, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, Trim(vPrn_PvuSetNo), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + W1 + 25, CurY, 0, 0, pFont)

                        ElseIf prn_DetIndx = 5 Then
                            Common_Procedures.Print_To_PrintDocument(e, "Beam No", LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + 10, CurY, 0, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + W1 + 10, CurY, 0, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, Trim(vPrn_PvuBmNos1), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + W1 + 25, CurY, 0, 0, pFont)

                        ElseIf prn_DetIndx = 6 Then
                            Common_Procedures.Print_To_PrintDocument(e, Trim(vPrn_PvuBmNos2), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + 50, CurY, 0, 0, pFont)

                        ElseIf prn_DetIndx = 7 Then
                            Common_Procedures.Print_To_PrintDocument(e, Trim(vPrn_PvuBmNos3), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + 50, CurY, 0, 0, pFont)

                        ElseIf prn_DetIndx = 8 Then
                            Common_Procedures.Print_To_PrintDocument(e, Trim(vPrn_PvuBmNos4), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + 50, CurY, 0, 0, pFont)

                        End If

                        NoofDets = NoofDets + 1

                        If Trim(ItmNm2) <> "" Then
                            CurY = CurY + TxtHgt - 5
                            Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm2), LMargin + ClArr(1) + ClArr(2) + 10, CurY, 0, 0, pFont)
                            NoofDets = NoofDets + 1
                        End If

                        prn_DetIndx = prn_DetIndx + 1

                    Loop

                End If


                Printing_Format2_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, NoofDets, True)


            End If

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        e.HasMorePages = False

    End Sub

    Private Sub Printing_Format2_PageHeader(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByRef PageNo As Integer, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single)
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt2 As New DataTable
        Dim p1Font As Font
        Dim strHeight As Single
        Dim C1 As Single, W1 As Single, S1 As Single
        Dim Cmp_Name As String, Cmp_Add1 As String, Cmp_Add2 As String
        Dim Cmp_PhNo As String, Cmp_TinNo As String, Cmp_CstNo As String

        PageNo = PageNo + 1

        CurY = TMargin

        da2 = New SqlClient.SqlDataAdapter("select a.*, b.Mill_Name, c.Count_Name  from JobWork_Weaving_Yarn_Delivery_Details a INNER JOIN Mill_Head b on a.Mill_IdNo = b.Mill_IdNo LEFT OUTER JOIN Count_Head c on a.Count_IdNo = c.Count_IdNo where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.JobWork_Weaving_PavuYarn_Delivery_Code = '" & Trim(EntryCode) & "' Order by a.sl_no", con)
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
        Common_Procedures.Print_To_PrintDocument(e, "DELIVERY", LMargin, CurY, 2, PrintWidth, p1Font)
        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height

        'CurY = CurY + TxtHgt

        CurY = CurY + strHeight + 10 ' + 150
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(2) = CurY

        Try
            C1 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6)
            W1 = e.Graphics.MeasureString("D.C.NO    : ", pFont).Width
            S1 = e.Graphics.MeasureString("FROM  :    ", pFont).Width

            CurY = CurY + TxtHgt - 10
            p1Font = New Font("Calibri", 12, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "FROM  :  " & "M/s." & prn_HdDt.Rows(0).Item("Del_Name").ToString, LMargin + 10, CurY, 0, 0, p1Font)
            Common_Procedures.Print_To_PrintDocument(e, "DC.NO", LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("JobWork_Weaving_PavuYarn_Delivery_No").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, p1Font)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address1").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
            p1Font = New Font("Calibri", 14, FontStyle.Bold)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address2").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "DATE", LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("JobWork_Weaving_PavuYarn_Delivery_Date").ToString), "dd-MM-yyyy").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address3").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address4").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
            'If Trim(prn_HdDt.Rows(0).Item("Party_DcNo").ToString) <> "" Then
            '    Common_Procedures.Print_To_PrintDocument(e, "PARTY D.C.NO", LMargin + C1 + 10, CurY, 0, 0, pFont)
            '    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
            '    Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Party_DcNo").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, pFont)
            'End If
            CurY = CurY + TxtHgt + 5
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(3) = CurY

            e.Graphics.DrawLine(Pens.Black, LMargin + C1, LnAr(3), LMargin + C1, LnAr(2))

            CurY = CurY + TxtHgt - 10
            Common_Procedures.Print_To_PrintDocument(e, "SNO", LMargin, CurY, 2, ClAr(1), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "TYPE", LMargin + ClAr(1), CurY, 2, ClAr(2), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "MILL NAME", LMargin + ClAr(1) + ClAr(2), CurY, 2, ClAr(3), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "COUNT", LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, 2, ClAr(4), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "BAGS", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 2, ClAr(5), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "CONES", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, 2, ClAr(6), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "THIRI", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, 2, ClAr(7), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "PAVU DETAILS", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, 2, ClAr(8), pFont)

            CurY = CurY + TxtHgt + 10
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(4) = CurY

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub Printing_Format2_PageFooter(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single, ByVal NoofDets As Integer, ByVal is_LastPage As Boolean)
        Dim p1Font As Font
        Dim I As Integer
        Dim Cmp_Name As String
        Dim W1 As Single = 0

        W1 = e.Graphics.MeasureString("No.of Beams  : ", pFont).Width

        Try

            For I = NoofDets + 1 To NoofItems_PerPage

                CurY = CurY + TxtHgt



                If prn_DetIndx = 0 Then
                    Common_Procedures.Print_To_PrintDocument(e, "Ends Count", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + 10, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + W1 + 10, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, Trim(vPrn_PvuEdsCnt), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + W1 + 25, CurY, 0, 0, pFont)

                ElseIf prn_DetIndx = 1 Then
                    Common_Procedures.Print_To_PrintDocument(e, "No.of Beams", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + 10, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + W1 + 10, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, Trim(Val(vPrn_PvuTotBms)), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + W1 + 25, CurY, 0, 0, pFont)

                ElseIf prn_DetIndx = 2 Then
                    Common_Procedures.Print_To_PrintDocument(e, "Meters", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + 10, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + W1 + 10, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, Trim(Format(Val(vPrn_PvuTotMtrs), "#########0.00")), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + W1 + 25, CurY, 0, 0, pFont)

                ElseIf prn_DetIndx = 3 Then
                    Common_Procedures.Print_To_PrintDocument(e, "Noof_Used", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + 10, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + W1 + 10, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, Trim(Val(vPrn_PvuNPcs)), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + W1 + 25, CurY, 0, 0, pFont)


                ElseIf prn_DetIndx = 4 Then
                    Common_Procedures.Print_To_PrintDocument(e, "Set No", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + 10, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + W1 + 10, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, Trim(vPrn_PvuSetNo), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + W1 + 25, CurY, 0, 0, pFont)

                ElseIf prn_DetIndx = 5 Then
                    Common_Procedures.Print_To_PrintDocument(e, "Beam No", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + 10, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + W1 + 10, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, Trim(vPrn_PvuBmNos1), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + W1 + 25, CurY, 0, 0, pFont)

                ElseIf prn_DetIndx = 6 Then
                    Common_Procedures.Print_To_PrintDocument(e, Trim(vPrn_PvuBmNos2), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + 50, CurY, 0, 0, pFont)

                ElseIf prn_DetIndx = 7 Then
                    Common_Procedures.Print_To_PrintDocument(e, Trim(vPrn_PvuBmNos3), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + 50, CurY, 0, 0, pFont)

                ElseIf prn_DetIndx = 8 Then
                    Common_Procedures.Print_To_PrintDocument(e, Trim(vPrn_PvuBmNos4), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + 50, CurY, 0, 0, pFont)

                End If

                prn_DetIndx = prn_DetIndx + 1

            Next

            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(5) = CurY

            CurY = CurY + TxtHgt - 10
            If is_LastPage = True Then
                Common_Procedures.Print_To_PrintDocument(e, " TOTAL", LMargin + ClAr(1) + ClAr(2) + 30, CurY, 2, ClAr(4), pFont)
            End If

            If Val(prn_HdDt.Rows(0).Item("Total_Bags").ToString) <> 0 Then
                If is_LastPage = True Then
                    Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(0).Item("Total_Bags").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 1, 0, pFont)
                End If
            End If
            If Val(prn_HdDt.Rows(0).Item("Total_Cones").ToString) <> 0 Then
                If is_LastPage = True Then
                    Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(0).Item("Total_Cones").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY, 1, 0, pFont)
                End If
            End If
            If Val(prn_HdDt.Rows(0).Item("Total_Thiri").ToString) <> 0 Then
                If is_LastPage = True Then
                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Total_Thiri").ToString), "#########0.000"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, pFont)
                End If
            End If


            CurY = CurY + TxtHgt - 15

            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(6) = CurY

            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1), CurY, LMargin + ClAr(1), LnAr(3))

            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2), CurY, LMargin + ClAr(1) + ClAr(2), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), LnAr(3))
            'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), LnAr(3))
            'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9), LnAr(3))

            'CurY = CurY + TxtHgt - 5

            'Common_Procedures.Print_To_PrintDocument(e, "Through : " & Trim(prn_HdDt.Rows(0).Item("Vehicle_No").ToString), LMargin + 10, CurY, 0, 0, pFont)
            'If Val(prn_HdDt.Rows(0).Item("Empty_Beam").ToString) <> 0 Then
            '    Common_Procedures.Print_To_PrintDocument(e, " Empty Beams : " & Trim(prn_HdDt.Rows(0).Item("Empty_Beam").ToString), PageWidth - 250, CurY, 0, 0, pFont)
            'End If

            'CurY = CurY + TxtHgt + 10
            'e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            'LnAr(7) = CurY

            CurY = CurY + TxtHgt
            'If Val(Common_Procedures.User.IdNo) <> 1 Then
            '    Common_Procedures.Print_To_PrintDocument(e, "(" & Trim(Common_Procedures.User.Name) & ")", LMargin + 400, CurY, 0, 0, pFont)
            'End If

            'CurY = CurY + TxtHgt
            CurY = CurY + TxtHgt
            Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString
            Common_Procedures.Print_To_PrintDocument(e, "Receiver's Signature", LMargin + 20, CurY, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, "Prepared By ", LMargin + 300, CurY, 0, 0, pFont)
            p1Font = New Font("Calibri", 12, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "For " & Cmp_Name, PageWidth - 15, CurY, 1, 0, p1Font)

            CurY = CurY + TxtHgt + 10

            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            e.Graphics.DrawLine(Pens.Black, LMargin, LnAr(1), LMargin, CurY)
            e.Graphics.DrawLine(Pens.Black, PageWidth, LnAr(1), PageWidth, CurY)

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub Printing_Format3(ByRef e As System.Drawing.Printing.PrintPageEventArgs)
        Dim cmd As New SqlClient.SqlCommand
        Dim Da1 As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim EntryCode As String
        Dim I As Integer = 0
        Dim NoofDets As Integer, NoofItems_PerPage As Integer
        Dim pFont As Font
        Dim LMargin As Single, RMargin As Single, TMargin As Single, BMargin As Single
        Dim PrintWidth As Single, PrintHeight As Single
        Dim PageWidth As Single, PageHeight As Single
        Dim CurY As Single, TxtHgt As Single
        Dim LnAr(15) As Single, ClArr(15) As Single
        Dim MilNm1 As String = "", MilNm2 As String = ""
        Dim ps As Printing.PaperSize
        Dim strHeight As Single = 0
        Dim PpSzSTS As Boolean = False
        Dim W1 As Single = 0
        Dim Inc As Integer = 0

        'Dim pkCustomSize1 As New System.Drawing.Printing.PaperSize("PAPER 8X6", 800, 600)
        'PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = pkCustomSize1
        'PrintDocument1.DefaultPageSettings.PaperSize = pkCustomSize1
        'e.PageSettings.PaperSize = pkCustomSize1
        'PrintDocument1.DefaultPageSettings.Landscape = False

        For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
            ps = PrintDocument1.PrinterSettings.PaperSizes(I)
            If ps.Width = 800 And ps.Height = 600 Then
                PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = ps
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
                    PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = ps
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
                        PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = ps
                        PrintDocument1.DefaultPageSettings.PaperSize = ps
                        e.PageSettings.PaperSize = ps
                        Exit For
                    End If
                Next
            End If

        End If

        With PrintDocument1.DefaultPageSettings.Margins
            If PrintDocument1.DefaultPageSettings.PaperSize.Width < 850 Then
                .Left = 20
                .Right = 50
            Else
                .Left = 30
                .Right = 30
            End If

            .Top = 10
            .Bottom = 30
            LMargin = .Left
            RMargin = .Right
            TMargin = .Top
            BMargin = .Bottom

        End With

        pFont = New Font("Calibri", 10, FontStyle.Regular)

        'e.Graphics.TextRenderingHint = Drawing.Text.TextRenderingHint.AntiAlias

        With PrintDocument1.DefaultPageSettings.PaperSize
            PrintWidth = .Width - RMargin - LMargin
            PrintHeight = .Height - TMargin - BMargin
            PageWidth = .Width - RMargin
            PageHeight = .Height - BMargin
        End With
        'If PrintDocument1.DefaultPageSettings.Landscape = True Then
        '    With PrintDocument1.DefaultPageSettings.PaperSize
        '        PrintWidth = .Height - TMargin - BMargin
        '        PrintHeight = .Width - RMargin - LMargin
        '        PageWidth = .Height - TMargin
        '        PageHeight = .Width - RMargin
        '    End With
        'End If

        NoofItems_PerPage = 8 ' 6

        Erase LnAr
        Erase ClArr
        LnAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        ClArr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}

        ClArr(1) = 65 : ClArr(2) = 58 : ClArr(3) = 52 : ClArr(4) = 72
        ClArr(5) = 65 : ClArr(6) = 58 : ClArr(7) = 52 : ClArr(8) = 72
        ClArr(9) = PageWidth - (LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8))

        TxtHgt = e.Graphics.MeasureString("A", pFont).Height
        TxtHgt = 16.8 ' 17 ' 18 ' 19 ' e.Graphics.MeasureString("A", pFont).Height  ' 20

        EntryCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Try

            prn_Prev_HeadIndx = prn_HeadIndx

            If prn_HdDt.Rows.Count > 0 Then

                If prn_HeadIndx <= prn_HdDt.Rows.Count - 1 Then

                    Printing_Format3_PageHeader(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, prn_PageNo, NoofItems_PerPage, CurY, LnAr, ClArr)

                    W1 = e.Graphics.MeasureString("No.of Beams  : ", pFont).Width

                    NoofDets = 0
                    Inc = 0

                    CurY = CurY - 10

                    If prn_DetMxIndx > 0 Then

                        Do While prn_NoofBmDets < prn_DetMxIndx

                            If NoofDets >= NoofItems_PerPage Then

                                CurY = CurY + TxtHgt

                                Common_Procedures.Print_To_PrintDocument(e, "Continued....", PageWidth - 10, CurY, 1, 0, pFont)

                                NoofDets = NoofDets + 1

                                Printing_Format3_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, NoofDets, False)

                                prn_DetIndx = prn_DetIndx + NoofItems_PerPage

                                e.HasMorePages = True

                                Return

                            End If

                            prn_DetSNo = prn_DetSNo + 1

                            'MilNm1 = Trim(prn_DetAr(101, 1))
                            'MilNm2 = ""
                            'If Len(MilNm1) > 18 Then
                            '    For I = 18 To 1 Step -1
                            '        If Mid$(Trim(MilNm1), I, 1) = " " Or Mid$(Trim(MilNm1), I, 1) = "," Or Mid$(Trim(MilNm1), I, 1) = "." Or Mid$(Trim(MilNm1), I, 1) = "-" Or Mid$(Trim(MilNm1), I, 1) = "/" Or Mid$(Trim(MilNm1), I, 1) = "_" Or Mid$(Trim(MilNm1), I, 1) = "(" Or Mid$(Trim(MilNm1), I, 1) = ")" Or Mid$(Trim(MilNm1), I, 1) = "\" Or Mid$(Trim(MilNm1), I, 1) = "[" Or Mid$(Trim(MilNm1), I, 1) = "]" Or Mid$(Trim(MilNm1), I, 1) = "{" Or Mid$(Trim(MilNm1), I, 1) = "}" Then Exit For
                            '    Next I
                            '    If I = 0 Then I = 18
                            '    MilNm2 = Microsoft.VisualBasic.Right(Trim(MilNm1), Len(MilNm1) - I)
                            '    MilNm1 = Microsoft.VisualBasic.Left(Trim(MilNm1), I - 1)
                            'End If

                            prn_DetIndx = prn_DetIndx + 1

                            CurY = CurY + TxtHgt

                            If Val(prn_DetAr(prn_DetIndx, 4)) <> 0 Then

                                Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr(prn_DetIndx, 1)), LMargin + 10, CurY, 0, 0, pFont)
                                Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr(prn_DetIndx, 2)), LMargin + ClArr(1) + 10, CurY, 0, 0, pFont)
                                If Val(prn_DetAr(prn_DetIndx, 3)) <> 0 Then
                                    Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr(prn_DetIndx, 3)), LMargin + ClArr(1) + ClArr(2) + ClArr(3) - 10, CurY, 1, 0, pFont)
                                End If
                                Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr(prn_DetIndx, 4)), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) - 10, CurY, 1, 0, pFont)

                                If prn_DetIndx > 6 Then
                                    prn_NoofBmDets = prn_NoofBmDets + 1
                                End If

                            End If

                            If Val(prn_DetAr(prn_DetIndx + NoofItems_PerPage, 4)) <> 0 Then

                                Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr(prn_DetIndx + NoofItems_PerPage, 1)), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + 10, CurY, 0, 0, pFont)
                                Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr(prn_DetIndx + NoofItems_PerPage, 2)), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + 10, CurY, 0, 0, pFont)
                                If Val(prn_DetAr(prn_DetIndx + NoofItems_PerPage, 3)) <> 0 Then
                                    Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr(prn_DetIndx + NoofItems_PerPage, 3)), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) - 10, CurY, 1, 0, pFont)
                                End If
                                Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr(prn_DetIndx + NoofItems_PerPage, 4)), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) - 10, CurY, 1, 0, pFont)

                                prn_NoofBmDets = prn_NoofBmDets + 1

                            End If

                            W1 = e.Graphics.MeasureString("MILL NAME : ", pFont).Width

                            If prn_DetIndx = 1 Then

                                If Trim(prn_DetAr(prn_DetIndx + 100, 1)) <> "" Then
                                    Common_Procedures.Print_To_PrintDocument(e, "Mill NAME", LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + 10, CurY, 0, 0, pFont)
                                    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + W1 + 10, CurY, 0, 0, pFont)
                                    Common_Procedures.Print_To_PrintDocument(e, Microsoft.VisualBasic.Left(Trim(prn_DetAr(prn_DetIndx + 100, 1)), 15), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + W1 + 25, CurY, 0, 0, pFont)
                                End If

                                prn_NoofBmDets = prn_NoofBmDets + 1

                            ElseIf prn_DetIndx = 2 Then
                                Inc = Inc + 5
                                If Trim(prn_DetAr(prn_DetIndx + 100, 1)) <> "" Then
                                    Common_Procedures.Print_To_PrintDocument(e, "Count", LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + 10, CurY + Inc, 0, 0, pFont)
                                    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + W1 + 10, CurY + Inc, 0, 0, pFont)
                                    Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr(prn_DetIndx + 100, 1)), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + W1 + 25, CurY + Inc, 0, 0, pFont)
                                End If

                                prn_NoofBmDets = prn_NoofBmDets + 1


                            ElseIf prn_DetIndx = 3 Then
                                Inc = Inc + 5
                                If Val(prn_DetAr(prn_DetIndx + 100, 1)) <> 0 Then
                                    Common_Procedures.Print_To_PrintDocument(e, "Bags", LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + 10, CurY + Inc, 0, 0, pFont)
                                    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + W1 + 10, CurY + Inc, 0, 0, pFont)
                                    Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr(prn_DetIndx + 100, 1)), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + W1 + 25, CurY + Inc, 0, 0, pFont)
                                End If

                                prn_NoofBmDets = prn_NoofBmDets + 1

                            ElseIf prn_DetIndx = 4 Then
                                Inc = Inc + 5
                                If Val(prn_DetAr(prn_DetIndx + 100, 1)) <> 0 Then
                                    Common_Procedures.Print_To_PrintDocument(e, "Cones", LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + 10, CurY + Inc, 0, 0, pFont)
                                    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + W1 + 10, CurY + Inc, 0, 0, pFont)
                                    Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr(prn_DetIndx + 100, 1)), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + W1 + 25, CurY + Inc, 0, 0, pFont)
                                End If

                                prn_NoofBmDets = prn_NoofBmDets + 1

                            ElseIf prn_DetIndx = 5 Then
                                Inc = Inc + 5
                                If Val(prn_DetAr(prn_DetIndx + 100, 1)) <> 0 Then
                                    Common_Procedures.Print_To_PrintDocument(e, "Weight (Kg)", LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + 10, CurY + Inc, 0, 0, pFont)
                                    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + W1 + 10, CurY + Inc, 0, 0, pFont)
                                    Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr(prn_DetIndx + 100, 1)), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + W1 + 25, CurY + Inc, 0, 0, pFont)
                                End If

                                prn_NoofBmDets = prn_NoofBmDets + 1

                            ElseIf prn_DetIndx = 6 Then
                                Inc = Inc + 5
                                If Val(prn_DetAr(prn_DetIndx + 100, 1)) <> 0 Then
                                    Common_Procedures.Print_To_PrintDocument(e, "Thiri", LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + 10, CurY + Inc, 0, 0, pFont)
                                    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + W1 + 10, CurY + Inc, 0, 0, pFont)
                                    Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr(prn_DetIndx + 100, 1)), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + W1 + 25, CurY + Inc, 0, 0, pFont)
                                End If

                                prn_NoofBmDets = prn_NoofBmDets + 1

                            End If

                            NoofDets = NoofDets + 1

                            'If Trim(MilNm2) <> "" Then
                            '    CurY = CurY + TxtHgt - 5
                            '    Common_Procedures.Print_To_PrintDocument(e, Trim(MilNm2), LMargin + ClArr(1) + ClArr(2) + 10, CurY, 0, 0, pFont)
                            '    NoofDets = NoofDets + 1
                            'End If

                            'prn_DetIndx = prn_DetIndx + 1

                        Loop

                    End If

                    Printing_Format3_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, NoofDets, True)

                End If

            End If

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        prn_HeadIndx = prn_HeadIndx + 1

        If prn_HeadIndx <= prn_HdDt.Rows.Count - 1 Then
            e.HasMorePages = True
        Else
            e.HasMorePages = False
        End If

    End Sub


    Private Sub Printing_Format3_PageHeader(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByRef PageNo As Integer, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single)
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim OrdByNo As Single = 0
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt2 As New DataTable
        Dim dt3 As New DataTable
        Dim dt4 As New DataTable
        Dim p1Font As Font
        Dim strHeight As Single
        Dim W1 As Single, N1 As Single, M1 As Single
        Dim Arr(300, 5) As String
        Dim Cmp_Name As String, Cmp_Add1 As String, Cmp_Add2 As String
        Dim Cmp_PhNo As String, Cmp_TinNo As String, Cmp_CstNo As String
        Dim vPrn_DcNo As String = ""

        PageNo = PageNo + 1

        CurY = TMargin

        da2 = New SqlClient.SqlDataAdapter("select a.*, b.Mill_Name, c.Count_Name  from JobWork_Weaving_Yarn_Delivery_Details a INNER JOIN Mill_Head b on a.Mill_IdNo = b.Mill_IdNo LEFT OUTER JOIN Count_Head c on a.Count_IdNo = c.Count_IdNo where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.JobWork_Weaving_PavuYarn_Delivery_Code = '" & Trim(EntryCode) & "' Order by a.sl_no", con)
        da2.Fill(dt2)

        If dt2.Rows.Count > NoofItems_PerPage Then
            Common_Procedures.Print_To_PrintDocument(e, "Page : " & Trim(Val(PageNo)), PageWidth - 10, CurY - TxtHgt, 1, 0, pFont)
        End If
        dt2.Clear()

        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(1) = CurY

        Cmp_Name = "" : Cmp_Add1 = "" : Cmp_Add2 = ""
        Cmp_PhNo = "" : Cmp_TinNo = "" : Cmp_CstNo = ""

        Cmp_Name = prn_HdDt.Rows(prn_HeadIndx).Item("Company_Name").ToString
        Cmp_Add1 = prn_HdDt.Rows(prn_HeadIndx).Item("Company_Address1").ToString & " " & prn_HdDt.Rows(prn_HeadIndx).Item("Company_Address2").ToString
        Cmp_Add2 = prn_HdDt.Rows(prn_HeadIndx).Item("Company_Address3").ToString & " " & prn_HdDt.Rows(prn_HeadIndx).Item("Company_Address4").ToString
        If Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Company_PhoneNo").ToString) <> "" Then
            Cmp_PhNo = "PHONE NO.:" & prn_HdDt.Rows(prn_HeadIndx).Item("Company_PhoneNo").ToString
        End If
        If Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Company_TinNo").ToString) <> "" Then
            Cmp_TinNo = "TIN NO.: " & prn_HdDt.Rows(prn_HeadIndx).Item("Company_TinNo").ToString
        End If
        If Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Company_CstNo").ToString) <> "" Then
            Cmp_CstNo = "CST NO.: " & prn_HdDt.Rows(prn_HeadIndx).Item("Company_CstNo").ToString
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
        CurY = CurY + TxtHgt - 1
        Common_Procedures.Print_To_PrintDocument(e, Cmp_TinNo, LMargin + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, Cmp_CstNo, PageWidth - 10, CurY, 1, 0, pFont)

        CurY = CurY + TxtHgt - 10
        p1Font = New Font("Calibri", 16, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "DELIVERY", LMargin, CurY, 2, PrintWidth, p1Font)
        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height

        'CurY = CurY + TxtHgt

        CurY = CurY + strHeight + 5 ' + 150
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(2) = CurY

        Try
            M1 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5)
            W1 = e.Graphics.MeasureString("D.C.NO    : ", pFont).Width
            N1 = e.Graphics.MeasureString("TO   :", pFont).Width

            CurY = CurY + TxtHgt - 10
            p1Font = New Font("Calibri", 10, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "TO  :  " & "M/s." & prn_HdDt.Rows(prn_HeadIndx).Item("Del_Name").ToString, LMargin + 10, CurY, 0, 0, p1Font)
            Common_Procedures.Print_To_PrintDocument(e, "DC.NO", LMargin + M1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + M1 + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(prn_HeadIndx).Item("JobWork_Weaving_PavuYarn_Delivery_No").ToString, LMargin + M1 + W1 + 30, CurY, 0, 0, p1Font)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(prn_HeadIndx).Item("Ledger_Address1").ToString, LMargin + N1 + 10, CurY, 0, 0, pFont)
            p1Font = New Font("Calibri", 14, FontStyle.Bold)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(prn_HeadIndx).Item("Ledger_Address2").ToString, LMargin + N1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "DATE", LMargin + M1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + M1 + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(prn_HeadIndx).Item("JobWork_Weaving_PavuYarn_Delivery_Date").ToString), "dd-MM-yyyy").ToString, LMargin + M1 + W1 + 30, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(prn_HeadIndx).Item("Ledger_Address3").ToString, LMargin + N1 + 10, CurY, 0, 0, pFont)
            If Trim(vPrn_PvuSetNo) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, "SET NO", LMargin + M1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + M1 + W1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, Trim(vPrn_PvuSetNo), LMargin + M1 + W1 + 30, CurY, 0, 0, pFont)
            End If

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(prn_HeadIndx).Item("Ledger_Address4").ToString, LMargin + N1 + 10, CurY, 0, 0, pFont)
            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1040" Then '---- M.S Textiles (Tirupur)

                vPrn_DcNo = ""

                OrdByNo = Common_Procedures.OrderBy_CodeToValue(Trim(prn_HdDt.Rows(prn_HeadIndx).Item("JobWork_Weaving_PavuYarn_Delivery_No").ToString))
                Da = New SqlClient.SqlDataAdapter("select top 1 JobWork_Weaving_PavuYarn_Delivery_No from JobWork_Weaving_PavuYarn_Delivery_Head where DeliveryTo_Idno = " & Str(Val(prn_HdDt.Rows(prn_HeadIndx).Item("DeliveryTo_Idno").ToString)) & " and for_orderby < " & Str(Format(Val(OrdByNo), "######0.00")) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and JobWork_Weaving_PavuYarn_Delivery_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, JobWork_Weaving_PavuYarn_Delivery_No desc", con)
                Dt1 = New DataTable
                Da.Fill(Dt1)
                If Dt1.Rows.Count > 0 Then
                    vPrn_DcNo = Dt1.Rows(0).Item("JobWork_Weaving_PavuYarn_Delivery_No").ToString
                End If
                Dt1.Clear()
                If Trim(vPrn_DcNo) <> "" Then
                    Common_Procedures.Print_To_PrintDocument(e, "Prev Dc.No", LMargin + M1 + 10, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + M1 + W1 + 10, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, Trim(vPrn_DcNo), LMargin + M1 + W1 + 30, CurY, 0, 0, pFont)
                End If

            End If
            'If Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Party_DcNo").ToString) <> "" Then
            '    Common_Procedures.Print_To_PrintDocument(e, "PARTY D.C.NO", LMargin + C1 + 10, CurY, 0, 0, pFont)
            '    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
            '    Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(prn_HeadIndx).Item("Party_DcNo").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, pFont)
            'End If
            CurY = CurY + TxtHgt + 5
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(3) = CurY

            ' e.Graphics.DrawLine(Pens.Black, LMargin + C1, LnAr(3), LMargin + C1, LnAr(2))

            e.Graphics.DrawLine(Pens.Black, LMargin + M1, LnAr(3), LMargin + M1, LnAr(2))
            ' e.Graphics.DrawLine(Pens.Black, LMargin + M1 + 4, LnAr(3), LMargin + M1 + 4, LnAr(2))

            CurY = CurY + TxtHgt - 10
            Common_Procedures.Print_To_PrintDocument(e, "ENDS", LMargin, CurY, 2, ClAr(1), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "BEAM", LMargin + ClAr(1), CurY, 2, ClAr(2), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "PCS", LMargin + ClAr(1) + ClAr(2), CurY, 2, ClAr(3), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "METERS", LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, 2, ClAr(4), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "ENDS", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 2, ClAr(5), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "BEAM", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + 3, CurY, 2, ClAr(6), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "PCS", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, 2, ClAr(7), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "METERS", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, 2, ClAr(8), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "YARN DETAILS", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, 2, ClAr(9), pFont)
            '  Common_Procedures.Print_To_PrintDocument(e, "ENDS COUNT", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9), CurY, 2, ClAr(10), pFont)

            CurY = CurY + TxtHgt + 10
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(4) = CurY

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub Printing_Format3_PageFooter(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single, ByVal NoofDets As Integer, ByVal is_LastPage As Boolean)
        Dim p1Font As Font
        Dim Cmp_Name As String
        Dim I As Integer
        Dim From_name As String

        Try

            For I = NoofDets + 1 To NoofItems_PerPage
                CurY = CurY + TxtHgt

                'If I = 1 Then
                '    Common_Procedures.Print_To_PrintDocument(e, "Mill NAME", LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + 10, CurY, 0, 0, pFont)
                '    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + W1 + 10, CurY, 0, 0, pFont)
                '    Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr(I + 100, 1)), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + W1 + 25, CurY, 0, 0, pFont)

                'ElseIf I = 2 Then
                '    Common_Procedures.Print_To_PrintDocument(e, "Count", LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + 10, CurY, 0, 0, pFont)
                '    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + W1 + 10, CurY, 0, 0, pFont)
                '    Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr(I + 100, 1)), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + W1 + 25, CurY, 0, 0, pFont)

                'ElseIf I = 3 Then
                '    Common_Procedures.Print_To_PrintDocument(e, "Bags", LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + 10, CurY, 0, 0, pFont)
                '    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + W1 + 10, CurY, 0, 0, pFont)
                '    Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr(I + 100, 1)), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + W1 + 25, CurY, 0, 0, pFont)

                'ElseIf I = 4 Then
                '    Common_Procedures.Print_To_PrintDocument(e, "Cones", LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + 10, CurY, 0, 0, pFont)
                '    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + W1 + 10, CurY, 0, 0, pFont)
                '    Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr(I + 100, 1)), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + W1 + 25, CurY, 0, 0, pFont)

                'ElseIf I = 5 Then
                '    Common_Procedures.Print_To_PrintDocument(e, "Weight (Kg)", LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + 10, CurY, 0, 0, pFont)
                '    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + W1 + 10, CurY, 0, 0, pFont)
                '    Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr(I + 100, 1)), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + W1 + 25, CurY, 0, 0, pFont)

                'ElseIf I = 6 Then

                '    If Val(prn_DetAr(I + 100, 1)) <> 0 Then
                '        Common_Procedures.Print_To_PrintDocument(e, "Thiri", LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + 10, CurY, 0, 0, pFont)
                '        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + W1 + 10, CurY, 0, 0, pFont)
                '        Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr(I + 100, 1)), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + W1 + 25, CurY, 0, 0, pFont)
                '    End If

                'End If

            Next

            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(5) = CurY

            CurY = CurY + TxtHgt - 10

            If is_LastPage = True Then

                If (prn_DetMxIndx Mod (NoofItems_PerPage * 2)) <= NoofItems_PerPage Then

                    If Val(prn_HdDt.Rows(prn_HeadIndx).Item("Total_beam").ToString) <> 0 Then
                        Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(prn_HeadIndx).Item("Total_Beam").ToString), LMargin + ClAr(1) + ClAr(2) - 10, CurY, 1, 0, pFont)
                    End If

                    If Val(prn_HdDt.Rows(prn_HeadIndx).Item("Total_Meters").ToString) <> 0 Then
                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(prn_HeadIndx).Item("Total_Meters").ToString), "#########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) - 10, CurY, 1, 0, pFont)
                    End If

                Else

                    If Val(prn_HdDt.Rows(prn_HeadIndx).Item("Total_Beam").ToString) <> 0 Then
                        Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(prn_HeadIndx).Item("Total_Beam").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY, 1, 0, pFont)
                    End If

                    If Val(prn_HdDt.Rows(prn_HeadIndx).Item("Total_Meters").ToString) <> 0 Then
                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(prn_HeadIndx).Item("Total_Meters").ToString), "#########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 10, CurY, 1, 0, pFont)
                    End If

                End If

            End If

            CurY = CurY + TxtHgt + 10
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(6) = CurY


            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1), CurY, LMargin + ClAr(1), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2), CurY, LMargin + ClAr(1) + ClAr(2), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + 4, CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + 4, LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9), LnAr(3))

            CurY = CurY + TxtHgt - 5
            Common_Procedures.Print_To_PrintDocument(e, "Received Beams and Yarn as per above details.", LMargin + 20, CurY, 0, 0, pFont)

            'Pavu_RecFrom_Name, h.Ledger_Name as Yarn_RecFrom_Name

            From_name = ""
            If prn_HdDt.Rows(prn_HeadIndx).Item("Yarn_RecFrom_Name").ToString <> "" And Val(prn_HdDt.Rows(prn_HeadIndx).Item("Total_Weight").ToString) <> 0 Then
                If prn_HdDt.Rows(prn_HeadIndx).Item("Pavu_RecFrom_Name").ToString <> "" And (Val(prn_HdDt.Rows(prn_HeadIndx).Item("Total_Meters").ToString <> 0) Or Val(prn_HdDt.Rows(prn_HeadIndx).Item("Meters").ToString <> 0)) Then
                    From_name = "Rec.From (Yarn) : " & Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Yarn_RecFrom_Name").ToString)
                Else
                    From_name = "Rec.From : " & Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Yarn_RecFrom_Name").ToString)
                End If
            End If

            If prn_HdDt.Rows(prn_HeadIndx).Item("Pavu_RecFrom_Name").ToString <> "" And (Val(prn_HdDt.Rows(prn_HeadIndx).Item("Total_Meters").ToString <> 0) Or Val(prn_HdDt.Rows(prn_HeadIndx).Item("Meters").ToString <> 0)) Then
                If prn_HdDt.Rows(prn_HeadIndx).Item("Yarn_RecFrom_Name").ToString <> "" And Val(prn_HdDt.Rows(prn_HeadIndx).Item("Total_Weight").ToString) <> 0 Then
                    From_name = From_name & IIf(Trim(From_name) <> "", "         ", "") & "Rec.From (Pavu) : " & Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Pavu_RecFrom_Name").ToString)
                Else
                    From_name = From_name & IIf(Trim(From_name) <> "", "         ", "") & "Rec.From : " & Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Pavu_RecFrom_Name").ToString)
                End If
            End If

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, From_name, LMargin + 20, CurY, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, "Rec.From : " & From_name, LMargin + 20, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            CurY = CurY + TxtHgt - 5
            Cmp_Name = prn_HdDt.Rows(prn_HeadIndx).Item("Company_Name").ToString
            Common_Procedures.Print_To_PrintDocument(e, "Receiver's Signature", LMargin + 20, CurY, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, "Prepared By ", LMargin + 300, CurY, 0, 0, pFont)
            p1Font = New Font("Calibri", 12, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "For " & Cmp_Name, PageWidth - 15, CurY, 1, 0, p1Font)

            CurY = CurY + TxtHgt + 8

            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            e.Graphics.DrawLine(Pens.Black, LMargin, LnAr(1), LMargin, CurY)
            e.Graphics.DrawLine(Pens.Black, PageWidth, LnAr(1), PageWidth, CurY)

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub Printing_FormJJ()
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim NewCode As String
        Dim ps As Printing.PaperSize
        Dim OrdBy_FrmNo As Single = 0, OrdByToNo As Single = 0
        Dim PpSzSTS As Boolean = False

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Try

            da1 = New SqlClient.SqlDataAdapter("select * from JobWork_Weaving_PavuYarn_Delivery_Head Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and JobWork_Weaving_PavuYarn_Delivery_Code = '" & Trim(NewCode) & "'", con)
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

        prn_InpOpts = InputBox("Select    -   0. None" & Chr(13) & "                  1. Original" & Chr(13) & "                  2. Duplicate" & Chr(13) & "                  3. Triplicate" & Chr(13) & "                  4. All", "FOR FORMJJ PRINTING...", "4")
        prn_InpOpts = Replace(Trim(prn_InpOpts), "4", "123")

        For I = 0 To PrintDocument2.PrinterSettings.PaperSizes.Count - 1
            If PrintDocument2.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
                ps = PrintDocument2.PrinterSettings.PaperSizes(I)
                PrintDocument2.DefaultPageSettings.PaperSize = ps
                Exit For
            End If
        Next


        If Val(Common_Procedures.Print_OR_Preview_Status) = 1 Then
            Try
                If Val(Common_Procedures.settings.Printing_Show_PrintDialogue) = 1 Then
                    PrintDialog1.PrinterSettings = PrintDocument2.PrinterSettings
                    If PrintDialog1.ShowDialog() = Windows.Forms.DialogResult.OK Then
                        PrintDocument2.PrinterSettings = PrintDialog1.PrinterSettings
                        PrintDocument2.Print()
                    End If

                Else
                    PrintDocument2.Print()

                End If

            Catch ex As Exception
                MessageBox.Show("The printing operation failed" & vbCrLf & ex.Message, "DOES NOT SHOW PRINT PREVIEW...", MessageBoxButtons.OK, MessageBoxIcon.Error)

            End Try


        Else
            Try

                Dim ppd As New PrintPreviewDialog

                ppd.Document = PrintDocument2


                ppd.WindowState = FormWindowState.Normal
                ppd.StartPosition = FormStartPosition.CenterScreen
                ppd.ClientSize = New Size(600, 600)

                ppd.ShowDialog()
                'If PageSetupDialog1.ShowDialog() = Windows.Forms.DialogResult.OK Then
                '    PrintDocument2.DefaultPageSettings = PageSetupDialog1.PageSettings
                '    ppd.ShowDialog()
                'End If

            Catch ex As Exception
                MsgBox("The printing operation failed" & vbCrLf & ex.Message, MsgBoxStyle.Critical, "DOES NOT SHOW PRINT PREVIEW...")

            End Try

        End If
    End Sub

    Private Sub PrintDocument2_BeginPrint(ByVal sender As Object, ByVal e As System.Drawing.Printing.PrintEventArgs) Handles PrintDocument2.BeginPrint
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim cmd As New SqlClient.SqlCommand
        Dim NewCode As String
        Dim i As Integer, k As Integer
        Dim vDup_SetNo As String
        Dim vPvu_BmNo As String, vDup_BmNo As String
        Dim W1 As Single = 0
        Dim FsNo As Single, LsNo As Single
        Dim FsBeamNo As String, LsBeamNo As String

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        prn_HdDt.Clear()
        prn_DetDt.Clear()
        prn_DetIndx = 0
        prn_DetSNo = 0
        prn_PageNo = 0
        prn_DetMxIndx = 0
        prn_NoofBmDets = 0
        prn_Count = 0

        Erase prn_HdAr
        Erase prn_DetAr

        prn_HdAr = New String(200, 10) {}
        prn_DetAr = New String(200, 10) {}

        Try

            da1 = New SqlClient.SqlDataAdapter("select a.*, b.*, c.Ledger_TinNo , c.Ledger_CstNo, c.Ledger_Name as Del_Name, c.Ledger_Address1, c.Ledger_Address2, c.Ledger_Address3, c.Ledger_Address4, d.Ledger_Name as Transport_Name, e.Area_Name, f.EndsCount_Name, g.Ledger_Name as Pavu_RecFrom_Name, h.Ledger_Name as Yarn_RecFrom_Name from JobWork_Weaving_PavuYarn_Delivery_Head a INNER JOIN Company_Head b ON a.Company_IdNo = b.Company_IdNo INNER JOIN Ledger_Head c ON a.DeliveryTo_Idno = c.Ledger_IdNo LEFT OUTER JOIN Ledger_Head d ON a.Transport_Idno = d.Ledger_IdNo  LEFT OUTER JOIN Area_Head e ON b.Area_Idno = e.Area_Idno LEFT OUTER JOIN EndsCount_Head f ON a.EndsCount_Idno = f.EndsCount_Idno LEFT OUTER JOIN Ledger_Head g ON a.ReceivedFrom_Idno = g.Ledger_IdNo LEFT OUTER JOIN Ledger_Head h ON a.Yarn_ReceivedFrom_Idno = h.Ledger_IdNo where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.JobWork_Weaving_PavuYarn_Delivery_Code = '" & Trim(NewCode) & "'", con)
            prn_HdDt = New DataTable
            da1.Fill(prn_HdDt)

            If prn_HdDt.Rows.Count > 0 Then

                da2 = New SqlClient.SqlDataAdapter("select a.*, b.Mill_Name, c.Count_Name from JobWork_Weaving_Yarn_Delivery_Details a LEFT OUTER JOIN Mill_Head b on a.Mill_IdNo = b.Mill_IdNo INNER JOIN Count_Head c on a.Count_IdNo = c.Count_IdNo where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.JobWork_Weaving_PavuYarn_Delivery_Code = '" & Trim(NewCode) & "' Order by a.sl_no", con)
                prn_DetDt = New DataTable
                da2.Fill(prn_DetDt)

                vPrn_PvuEdsCnt = ""
                vPrn_PvuTotBms = 0
                vPrn_PvuTotMtrs = 0 : vPrn_PvuNPcs = 0
                vPrn_PvuSetNo = "" : vDup_SetNo = ""
                vDup_BmNo = "" : vPvu_BmNo = ""
                vPrn_PvuBmNos1 = "" : vPrn_PvuBmNos2 = "" : vPrn_PvuBmNos3 = "" : vPrn_PvuBmNos4 = ""

                cmd.Connection = con

                cmd.CommandText = "truncate table " & Trim(Common_Procedures.EntryTempTable) & ""
                cmd.ExecuteNonQuery()

                da1 = New SqlClient.SqlDataAdapter("select a.*, b.* from JobWork_Weaving_Pavu_Delivery_Details a INNER JOIN EndsCount_Head b ON a.EndsCount_IdNo = b.EndsCount_IdNo where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.JobWork_Weaving_PavuYarn_Delivery_Code = '" & Trim(NewCode) & "' Order by a.sl_no", con)
                Dt1 = New DataTable
                da1.Fill(Dt1)

                If Dt1.Rows.Count > 0 Then
                    vPrn_PvuEdsCnt = Dt1.Rows(0).Item("EndsCount_Name").ToString

                    For i = 0 To Dt1.Rows.Count - 1

                        vPrn_PvuTotBms = Val(vPrn_PvuTotBms) + 1
                        vPrn_PvuTotMtrs = vPrn_PvuTotMtrs + Val(Dt1.Rows(i).Item("Meters").ToString)
                        vPrn_PvuNPcs = vPrn_PvuNPcs + Val(Dt1.Rows(i).Item("Noof_Used").ToString)

                        If InStr(1, "~" & Trim(UCase(vDup_SetNo)) & "~", "~" & Trim(UCase(Dt1.Rows(i).Item("Set_No").ToString)) & "~") = 0 Then
                            vDup_SetNo = Trim(vDup_SetNo) & "~" & Trim(Dt1.Rows(i).Item("Set_No").ToString) & "~"
                            vPrn_PvuSetNo = vPrn_PvuSetNo & IIf(Trim(vPrn_PvuSetNo) <> "", ", ", "") & Dt1.Rows(i).Item("Set_No").ToString
                        End If

                        If InStr(1, "~" & Trim(UCase(vDup_BmNo)) & "~", "~" & Trim(UCase(Dt1.Rows(i).Item("Set_No").ToString)) & "^" & Trim(UCase(Dt1.Rows(i).Item("Beam_No").ToString)) & "~") = 0 Then
                            vDup_BmNo = Trim(vDup_BmNo) & "~" & Trim(Dt1.Rows(i).Item("Set_No").ToString) & "^" & Trim(UCase(Dt1.Rows(i).Item("Beam_No").ToString)) & "~"

                            cmd.CommandText = "Insert into " & Trim(Common_Procedures.EntryTempTable) & "(Name1, Meters1) values ('" & Trim(Dt1.Rows(i).Item("Beam_No").ToString) & "', " & Common_Procedures.OrderBy_CodeToValue(Trim(Dt1.Rows(i).Item("Beam_No").ToString)) & " )"
                            cmd.ExecuteNonQuery()

                        End If

                    Next i

                End If

                Dt1.Clear()

                '--

                da1 = New SqlClient.SqlDataAdapter("select a.*, b.* from JobWork_Weaving_Pavu_Delivery_Details a INNER JOIN EndsCount_Head b ON a.EndsCount_IdNo = b.EndsCount_IdNo where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.JobWork_Weaving_PavuYarn_Delivery_Code = '" & Trim(NewCode) & "' Order by a.sl_no", con)
                prn_DetDt1 = New DataTable
                da1.Fill(prn_DetDt1)

                If prn_DetDt1.Rows.Count > 0 Then
                    For i = 0 To prn_DetDt1.Rows.Count - 1

                        If Val(prn_DetDt1.Rows(i).Item("Meters").ToString) <> 0 Then
                            prn_DetMxIndx = prn_DetMxIndx + 1
                            prn_DetAr(prn_DetMxIndx, 1) = Trim(prn_DetDt1.Rows(i).Item("Ends_Name").ToString)
                            prn_DetAr(prn_DetMxIndx, 2) = Trim(prn_DetDt1.Rows(i).Item("Beam_No").ToString)
                            prn_DetAr(prn_DetMxIndx, 3) = Val(prn_DetDt1.Rows(i).Item("Pcs").ToString)
                            prn_DetAr(prn_DetMxIndx, 4) = Format(Val(prn_DetDt1.Rows(i).Item("Meters").ToString), "#########0.00")
                        End If

                    Next i

                End If
                Dt1.Clear()

                'prn_DetMxIndx = prn_DetMxIndx + 1
                'prn_DetAr(prn_DetMxIndx, 1) = "--------------------"
                'prn_DetAr(prn_DetMxIndx, 2) = "--------------------"
                'prn_DetAr(prn_DetMxIndx, 3) = "--------------------"
                'prn_DetAr(prn_DetMxIndx, 4) = "--------------------"


                'prn_DetMxIndx = prn_DetMxIndx + 1
                ''prn_DetAr(prn_DetMxIndx, 1) = Trim(prn_DetDt1.Rows(i).Item("EndsCount_Name").ToString)
                ''prn_DetAr(prn_DetMxIndx, 2) = Trim(prn_DetDt1.Rows(i).Item("Beam_No").ToString)
                ''prn_DetAr(prn_DetMxIndx, 3) = Val(prn_DetDt1.Rows(i).Item("Pcs").ToString)
                ''prn_DetAr(prn_DetMxIndx, 4) = Format(Val(prn_DetDt1.Rows(i).Item("Meters").ToString), "#########0.00")


                'prn_DetMxIndx = prn_DetMxIndx + 1
                'prn_DetAr(prn_DetMxIndx, 1) = "--------------------"
                'prn_DetAr(prn_DetMxIndx, 2) = "--------------------"
                'prn_DetAr(prn_DetMxIndx, 3) = "--------------------"
                'prn_DetAr(prn_DetMxIndx, 4) = "--------------------"

                '--

                vPvu_BmNo = ""
                FsNo = 0 : LsNo = 0
                FsBeamNo = "" : LsBeamNo = ""

                da1 = New SqlClient.SqlDataAdapter("Select Name1 as Beam_No, Meters1 as fororderby_beamno from " & Trim(Common_Procedures.EntryTempTable) & " where Name1 <> '' order by Meters1, Name1", con)
                Dt1 = New DataTable
                da1.Fill(Dt1)

                If Dt1.Rows.Count > 0 Then
                    FsNo = Dt1.Rows(0).Item("fororderby_beamno").ToString
                    LsNo = Dt1.Rows(0).Item("fororderby_beamno").ToString

                    FsBeamNo = Trim(UCase(Dt1.Rows(0).Item("Beam_No").ToString))
                    LsBeamNo = Trim(UCase(Dt1.Rows(0).Item("Beam_No").ToString))

                    For i = 1 To Dt1.Rows.Count - 1
                        If LsNo + 1 = Val(Dt1.Rows(i).Item("fororderby_beamno").ToString) Then
                            LsNo = Val(Dt1.Rows(i).Item("fororderby_beamno").ToString)
                            LsBeamNo = Trim(UCase(Dt1.Rows(i).Item("Beam_No").ToString))

                        Else
                            If FsNo = LsNo Then
                                vPvu_BmNo = vPvu_BmNo & Trim(FsBeamNo) & ","
                            Else
                                vPvu_BmNo = vPvu_BmNo & Trim(FsBeamNo) & "-" & Trim(LsBeamNo) & ","
                            End If
                            FsNo = Dt1.Rows(i).Item("fororderby_beamno").ToString
                            LsNo = Dt1.Rows(i).Item("fororderby_beamno").ToString

                            FsBeamNo = Trim(UCase(Dt1.Rows(i).Item("Beam_No").ToString))
                            LsBeamNo = Trim(UCase(Dt1.Rows(i).Item("Beam_No").ToString))

                        End If

                    Next

                    If FsNo = LsNo Then vPvu_BmNo = vPvu_BmNo & Trim(FsBeamNo) Else vPvu_BmNo = vPvu_BmNo & Trim(FsBeamNo) & "-" & Trim(LsBeamNo)

                End If
                Dt1.Clear()


                vPrn_PvuBmNos1 = Trim(vPvu_BmNo)
                vPrn_PvuBmNos2 = ""
                vPrn_PvuBmNos3 = ""
                vPrn_PvuBmNos4 = ""
                If Len(vPrn_PvuBmNos1) > 18 Then
                    For i = 18 To 1 Step -1
                        If Mid$(Trim(vPrn_PvuBmNos1), i, 1) = " " Or Mid$(Trim(vPrn_PvuBmNos1), i, 1) = "," Or Mid$(Trim(vPrn_PvuBmNos1), i, 1) = "." Or Mid$(Trim(vPrn_PvuBmNos1), i, 1) = "-" Or Mid$(Trim(vPrn_PvuBmNos1), i, 1) = "/" Or Mid$(Trim(vPrn_PvuBmNos1), i, 1) = "_" Or Mid$(Trim(vPrn_PvuBmNos1), i, 1) = "(" Or Mid$(Trim(vPrn_PvuBmNos1), i, 1) = ")" Or Mid$(Trim(vPrn_PvuBmNos1), i, 1) = "\" Or Mid$(Trim(vPrn_PvuBmNos1), i, 1) = "[" Or Mid$(Trim(vPrn_PvuBmNos1), i, 1) = "]" Or Mid$(Trim(vPrn_PvuBmNos1), i, 1) = "{" Or Mid$(Trim(vPrn_PvuBmNos1), i, 1) = "}" Then Exit For
                    Next i
                    If i = 0 Then i = 18
                    vPrn_PvuBmNos2 = Microsoft.VisualBasic.Right(Trim(vPrn_PvuBmNos1), Len(vPrn_PvuBmNos1) - i)
                    vPrn_PvuBmNos1 = Microsoft.VisualBasic.Left(Trim(vPrn_PvuBmNos1), i - 1)
                End If

                If Len(vPrn_PvuBmNos2) > 23 Then
                    For i = 23 To 1 Step -1
                        If Mid$(Trim(vPrn_PvuBmNos2), i, 1) = " " Or Mid$(Trim(vPrn_PvuBmNos2), i, 1) = "," Or Mid$(Trim(vPrn_PvuBmNos2), i, 1) = "." Or Mid$(Trim(vPrn_PvuBmNos2), i, 1) = "-" Or Mid$(Trim(vPrn_PvuBmNos2), i, 1) = "/" Or Mid$(Trim(vPrn_PvuBmNos2), i, 1) = "_" Or Mid$(Trim(vPrn_PvuBmNos2), i, 1) = "(" Or Mid$(Trim(vPrn_PvuBmNos2), i, 1) = ")" Or Mid$(Trim(vPrn_PvuBmNos2), i, 1) = "\" Or Mid$(Trim(vPrn_PvuBmNos2), i, 1) = "[" Or Mid$(Trim(vPrn_PvuBmNos2), i, 1) = "]" Or Mid$(Trim(vPrn_PvuBmNos2), i, 1) = "{" Or Mid$(Trim(vPrn_PvuBmNos2), i, 1) = "}" Then Exit For
                    Next i
                    If i = 0 Then i = 23
                    vPrn_PvuBmNos3 = Microsoft.VisualBasic.Right(Trim(vPrn_PvuBmNos2), Len(vPrn_PvuBmNos2) - i)
                    vPrn_PvuBmNos2 = Microsoft.VisualBasic.Left(Trim(vPrn_PvuBmNos2), i - 1)
                End If

                If Len(vPrn_PvuBmNos3) > 23 Then
                    For i = 23 To 1 Step -1
                        If Mid$(Trim(vPrn_PvuBmNos3), i, 1) = " " Or Mid$(Trim(vPrn_PvuBmNos3), i, 1) = "," Or Mid$(Trim(vPrn_PvuBmNos3), i, 1) = "." Or Mid$(Trim(vPrn_PvuBmNos3), i, 1) = "-" Or Mid$(Trim(vPrn_PvuBmNos3), i, 1) = "/" Or Mid$(Trim(vPrn_PvuBmNos3), i, 1) = "_" Or Mid$(Trim(vPrn_PvuBmNos3), i, 1) = "(" Or Mid$(Trim(vPrn_PvuBmNos3), i, 1) = ")" Or Mid$(Trim(vPrn_PvuBmNos3), i, 1) = "\" Or Mid$(Trim(vPrn_PvuBmNos3), i, 1) = "[" Or Mid$(Trim(vPrn_PvuBmNos3), i, 1) = "]" Or Mid$(Trim(vPrn_PvuBmNos3), i, 1) = "{" Or Mid$(Trim(vPrn_PvuBmNos3), i, 1) = "}" Then Exit For
                    Next i
                    If i = 0 Then i = 23
                    vPrn_PvuBmNos4 = Microsoft.VisualBasic.Right(Trim(vPrn_PvuBmNos3), Len(vPrn_PvuBmNos3) - i)
                    vPrn_PvuBmNos3 = Microsoft.VisualBasic.Left(Trim(vPrn_PvuBmNos3), i - 1)
                End If

                da1 = New SqlClient.SqlDataAdapter("select top 1 a.Yarn_Type, c.Count_Name, d.Mill_Name, b.Total_Bags, b.Total_Cones, b.Total_Weight, b.Total_Thiri from JobWork_Weaving_Yarn_Delivery_Details a INNER JOIN JobWork_Weaving_PavuYarn_Delivery_Head b ON a.JobWork_Weaving_PavuYarn_Delivery_Code = b.JobWork_Weaving_PavuYarn_Delivery_Code INNER JOIN Count_Head c on a.Count_IdNo = c.Count_IdNo LEFT OUTER JOIN Mill_Head d on a.Mill_IdNo = d.Mill_IdNo where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.JobWork_Weaving_PavuYarn_Delivery_Code = '" & Trim(NewCode) & "' Order by a.sl_no", con)
                prn_DetDt1 = New DataTable
                da1.Fill(prn_DetDt1)

                k = 0
                If prn_DetDt1.Rows.Count > 0 Then

                    For i = 0 To prn_DetDt1.Rows.Count - 1

                        If Val(prn_DetDt1.Rows(i).Item("Total_Weight").ToString) <> 0 Then
                            k = k + 1
                            prn_DetAr(k + 100, 1) = Trim(prn_DetDt1.Rows(i).Item("Mill_Name").ToString)
                            k = k + 1
                            prn_DetAr(k + 100, 1) = Trim(prn_DetDt1.Rows(i).Item("Count_Name").ToString)
                            k = k + 1
                            prn_DetAr(k + 100, 1) = Val(prn_DetDt1.Rows(i).Item("Total_Bags").ToString)
                            k = k + 1
                            prn_DetAr(k + 100, 1) = Val(prn_DetDt1.Rows(i).Item("Total_Cones").ToString)
                            k = k + 1
                            prn_DetAr(k + 100, 1) = Format(Val(prn_DetDt1.Rows(i).Item("Total_Weight").ToString), "#########0.000")
                            If Val(prn_DetDt1.Rows(i).Item("Total_Thiri").ToString) <> 0 Then
                                k = k + 1
                                prn_DetAr(k + 100, 1) = Format(Val(prn_DetDt1.Rows(i).Item("Total_Thiri").ToString), "#########0.000")
                            End If
                        End If

                    Next i

                End If
                Dt1.Clear()

                If k > prn_DetMxIndx Then prn_DetMxIndx = k

            Else
                MessageBox.Show("This is New Entry", "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

            End If

            da1.Dispose()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub PrintDocument2_PrintPage(ByVal sender As System.Object, ByVal e As System.Drawing.Printing.PrintPageEventArgs) Handles PrintDocument2.PrintPage
        If prn_HdDt.Rows.Count <= 0 Then Exit Sub
        Printing_FormJJ(e)
    End Sub

    Private Sub Printing_FormJJ(ByRef e As System.Drawing.Printing.PrintPageEventArgs)
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
        Dim ps As Printing.PaperSize
        Dim strHeight As Single = 0
        Dim PpSzSTS As Boolean = False
        Dim W1 As Single = 0
        Dim SNo As Integer = 0
        Dim flperc As Single = 0
        Dim flmtr As Single = 0
        Dim fmtr As Single = 0
        Dim LnAr(15) As Single, ClArr(15) As Single
        Dim ItmNm1 As String, ItmNm2 As String


        'For I = 0 To PrintDocument2.PrinterSettings.PaperSizes.Count - 1
        '    If PrintDocument2.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
        '        ps = PrintDocument2.PrinterSettings.PaperSizes(I)
        '        PrintDocument2.DefaultPageSettings.PaperSize = ps
        '        PrintDocument2.PrinterSettings.DefaultPageSettings.PaperSize = ps
        '        e.PageSettings.PaperSize = ps
        '        Exit For
        '    End If
        'Next
        For I = 0 To PrintDocument2.PrinterSettings.PaperSizes.Count - 1
            If PrintDocument2.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
                ps = PrintDocument2.PrinterSettings.PaperSizes(I)
                PrintDocument2.DefaultPageSettings.PaperSize = ps
                e.PageSettings.PaperSize = ps
                Exit For
            End If
        Next
        '  End If

        With PrintDocument2.DefaultPageSettings.Margins
            .Left = 20
            .Right = 65
            .Top = 50 ' 60
            .Bottom = 40
            LMargin = .Left
            RMargin = .Right
            TMargin = .Top
            BMargin = .Bottom
        End With


        pFont = New Font("Calibri", 10, FontStyle.Regular)

        e.Graphics.TextRenderingHint = Drawing.Text.TextRenderingHint.AntiAlias

        With PrintDocument2.DefaultPageSettings.PaperSize
            PrintWidth = .Width - RMargin - LMargin
            PrintHeight = .Height - TMargin - BMargin
            PageWidth = .Width - RMargin
            PageHeight = .Height - BMargin
        End With
        If PrintDocument2.DefaultPageSettings.Landscape = True Then
            With PrintDocument2.DefaultPageSettings.PaperSize
                PrintWidth = .Height - TMargin - BMargin
                PrintHeight = .Width - RMargin - LMargin
                PageWidth = .Height - TMargin
                PageHeight = .Width - RMargin
            End With
        End If

        NoofItems_PerPage = 5 ' 6

        Erase LnAr
        Erase ClArr

        LnAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        ClArr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}

        ClArr(1) = Val(35) : ClArr(2) = 50 : ClArr(3) = 130 : ClArr(4) = 65 : ClArr(5) = 65 : ClArr(6) = 70 : ClArr(7) = 85
        ClArr(8) = PageWidth - (LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7))

        TxtHgt = 18.5 ' 19 ' e.Graphics.MeasureString("A", pFont).Height  ' 20

        TxtHgt = 19  ' e.Graphics.MeasureString("A", pFont).Height  ' 20

        ''=========================================================================================================
        ''------  START OF PREPRINT POINTS
        ''=========================================================================================================

        'pFont = New Font("Calibri", 11, FontStyle.Regular)

        'Dim CurX As Single = 0
        'Dim pFont1 As Font

        'pFont1 = New Font("Calibri", 8, FontStyle.Regular)

        'For I = 100 To 1100 Step 300

        '    CurY = I
        '    For J = 1 To 850 Step 40

        '        CurX = J
        '        Common_Procedures.Print_To_PrintDocument(e, CurX, CurX, CurY - 20, 0, 0, pFont1)
        '        Common_Procedures.Print_To_PrintDocument(e, "|", CurX, CurY, 0, 0, pFont)

        '        CurX = J + 20
        '        Common_Procedures.Print_To_PrintDocument(e, "|", CurX, CurY, 0, 0, pFont)
        '        Common_Procedures.Print_To_PrintDocument(e, "|", CurX, CurY + 20, 0, 0, pFont)
        '        Common_Procedures.Print_To_PrintDocument(e, CurX, CurX, CurY + 40, 0, 0, pFont1)

        '    Next

        'Next

        'For I = 200 To 800 Step 250

        '    CurX = I
        '    For J = 1 To 1200 Step 40

        '        CurY = J
        '        Common_Procedures.Print_To_PrintDocument(e, "-", CurX, CurY, 0, 0, pFont)
        '        Common_Procedures.Print_To_PrintDocument(e, "   " & CurY, CurX, CurY, 0, 0, pFont1)

        '        CurY = J + 20
        '        Common_Procedures.Print_To_PrintDocument(e, "--", CurX, CurY, 0, 0, pFont)
        '        Common_Procedures.Print_To_PrintDocument(e, "   " & CurY, CurX, CurY, 0, 0, pFont1)

        '    Next

        'Next

        'e.HasMorePages = False

        'Exit Sub

        ''=========================================================================================================
        ''------  END OF PREPRINT POINTS
        ''=========================================================================================================

        EntryCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Try
            If prn_HdDt.Rows.Count > 0 Then

                Printing_FormJJ_PageHeader(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, prn_PageNo, NoofItems_PerPage, CurY, LnAr, ClArr)

                NoofDets = 0

                CurY = CurY - 10
                W1 = e.Graphics.MeasureString("No.of Beams  : ", pFont).Width

                NoofDets = 0

                CurY = CurY - 10

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

                        ItmNm1 = Trim(prn_DetDt.Rows(prn_DetIndx).Item("Mill_Name").ToString)
                        ItmNm2 = ""
                        If Len(ItmNm1) > 18 Then
                            For I = 18 To 1 Step -1
                                If Mid$(Trim(ItmNm1), I, 1) = " " Or Mid$(Trim(ItmNm1), I, 1) = "," Or Mid$(Trim(ItmNm1), I, 1) = "." Or Mid$(Trim(ItmNm1), I, 1) = "-" Or Mid$(Trim(ItmNm1), I, 1) = "/" Or Mid$(Trim(ItmNm1), I, 1) = "_" Or Mid$(Trim(ItmNm1), I, 1) = "(" Or Mid$(Trim(ItmNm1), I, 1) = ")" Or Mid$(Trim(ItmNm1), I, 1) = "\" Or Mid$(Trim(ItmNm1), I, 1) = "[" Or Mid$(Trim(ItmNm1), I, 1) = "]" Or Mid$(Trim(ItmNm1), I, 1) = "{" Or Mid$(Trim(ItmNm1), I, 1) = "}" Then Exit For
                            Next I
                            If I = 0 Then I = 18
                            ItmNm2 = Microsoft.VisualBasic.Right(Trim(ItmNm1), Len(ItmNm1) - I)
                            ItmNm1 = Microsoft.VisualBasic.Left(Trim(ItmNm1), I - 1)
                        End If

                        CurY = CurY + TxtHgt + 5

                        Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetDt.Rows(prn_DetIndx).Item("Sl_No").ToString), LMargin + 15, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Yarn_Type").ToString, LMargin + ClArr(1) + 10, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm1), LMargin + ClArr(1) + ClArr(2) + 10, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Count_Name").ToString, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + 10, CurY, 0, 0, pFont)
                        If Val(prn_DetDt.Rows(prn_DetIndx).Item("Bags").ToString) <> 0 Then
                            Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(prn_DetIndx).Item("Bags").ToString), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) - 10, CurY, 1, 0, pFont)
                        End If
                        If Val(prn_DetDt.Rows(prn_DetIndx).Item("Cones").ToString) <> 0 Then
                            Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(prn_DetIndx).Item("Cones").ToString), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) - 10, CurY, 1, 0, pFont)
                        End If
                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Weight").ToString), "########0.000"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) - 10, CurY, 1, 0, pFont)

                        If prn_DetIndx = 0 Then
                            Common_Procedures.Print_To_PrintDocument(e, "Ends Count", LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + 10, CurY, 0, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + W1 + 10, CurY, 0, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, Trim(vPrn_PvuEdsCnt), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + W1 + 25, CurY, 0, 0, pFont)

                        ElseIf prn_DetIndx = 1 Then
                            Common_Procedures.Print_To_PrintDocument(e, "No.of Beams", LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + 10, CurY, 0, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + W1 + 10, CurY, 0, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, Trim(Val(vPrn_PvuTotBms)), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + W1 + 25, CurY, 0, 0, pFont)

                        ElseIf prn_DetIndx = 2 Then
                            Common_Procedures.Print_To_PrintDocument(e, "Meters", LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + 10, CurY, 0, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + W1 + 10, CurY, 0, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, Trim(Format(Val(vPrn_PvuTotMtrs), "#########0.00")), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + W1 + 25, CurY, 0, 0, pFont)

                        ElseIf prn_DetIndx = 3 Then
                            Common_Procedures.Print_To_PrintDocument(e, "Pcs", LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + 10, CurY, 0, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + W1 + 10, CurY, 0, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, Trim(Val(vPrn_PvuNPcs)), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + W1 + 25, CurY, 0, 0, pFont)


                        ElseIf prn_DetIndx = 4 Then
                            Common_Procedures.Print_To_PrintDocument(e, "Set No", LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + 10, CurY, 0, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + W1 + 10, CurY, 0, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, Trim(vPrn_PvuSetNo), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + W1 + 25, CurY, 0, 0, pFont)

                        ElseIf prn_DetIndx = 5 Then
                            Common_Procedures.Print_To_PrintDocument(e, "Beam No", LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + 10, CurY, 0, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + W1 + 10, CurY, 0, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, Trim(vPrn_PvuBmNos1), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + W1 + 25, CurY, 0, 0, pFont)

                        ElseIf prn_DetIndx = 6 Then
                            Common_Procedures.Print_To_PrintDocument(e, Trim(vPrn_PvuBmNos2), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + 50, CurY, 0, 0, pFont)

                        ElseIf prn_DetIndx = 7 Then
                            Common_Procedures.Print_To_PrintDocument(e, Trim(vPrn_PvuBmNos3), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + 50, CurY, 0, 0, pFont)

                        ElseIf prn_DetIndx = 8 Then
                            Common_Procedures.Print_To_PrintDocument(e, Trim(vPrn_PvuBmNos4), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + 50, CurY, 0, 0, pFont)

                        End If

                        NoofDets = NoofDets + 1

                        If Trim(ItmNm2) <> "" Then
                            CurY = CurY + TxtHgt - 5
                            Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm2), LMargin + ClArr(1) + ClArr(2) + 10, CurY, 0, 0, pFont)
                            NoofDets = NoofDets + 1
                        End If

                        prn_DetIndx = prn_DetIndx + 1

                    Loop

                End If

                Printing_FormJJ_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, NoofDets, True)

                If Trim(prn_InpOpts) <> "" Then
                    If prn_Count < Len(Trim(prn_InpOpts)) Then


                        If Val(prn_InpOpts) <> "0" Then
                            prn_DetIndx = 0
                            prn_DetSNo = 0
                            prn_PageNo = 0

                            e.HasMorePages = True
                            Return
                        End If

                    End If
                End If

            End If


        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        e.HasMorePages = False

    End Sub

    Private Sub Printing_FormJJ_PageHeader(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByRef PageNo As Integer, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClArr() As Single)
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt2 As New DataTable
        Dim da3 As New SqlClient.SqlDataAdapter
        Dim dt3 As New DataTable
        Dim p1Font As Font
        Dim strHeight As Single
        Dim C1 As Single, W1, W2 As Single, S1, S2 As Single
        Dim Cmp_Name, Desc As String, Cmp_Add1 As String, Cmp_Add2, Cmp_Add4 As String
        Dim Cmp_PhNo As String, Cmp_TinNo As String, Cmp_CstNo As String
        Dim S As String
        Dim goods_value As Single = 0
        Dim Yarn_value As Single = 0
        Dim pavu_value As Single = 0
        Dim NewCode As String
        Dim To_Add As String = ""

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        PageNo = PageNo + 1

        CurY = TMargin

        da2 = New SqlClient.SqlDataAdapter("select Sum(a.Weight * b.Rate_Kg) as Value_Of_Yarn from JobWork_Weaving_Yarn_Delivery_Details a INNER JOIN Mill_Count_Details b ON a.Mill_IdNo = b.Mill_IdNo and a.Count_IdNo = b.Count_IdNo Where a.JobWork_Weaving_PavuYarn_Delivery_Code = '" & Trim(NewCode) & "'", con)
        dt2 = New DataTable
        da2.Fill(dt2)

        da3 = New SqlClient.SqlDataAdapter("select Sum(a.Meters * b.Rate) as Value_Of_Pavu from JobWork_Weaving_Pavu_Delivery_Details a INNER JOIN EndsCount_Head b ON a.EndsCount_IdNo = b.EndsCount_IdNo Where a.JobWork_Weaving_PavuYarn_Delivery_Code = '" & Trim(NewCode) & "'", con)
        dt3 = New DataTable
        da3.Fill(dt3)

        If dt2.Rows.Count > 0 Then
            Yarn_value = Format(Val(dt2.Rows(0).Item("Value_Of_Yarn").ToString), "#######0.00")
        End If

        If dt3.Rows.Count > 0 Then
            pavu_value = Format(Val(dt3.Rows(0).Item("Value_Of_Pavu").ToString), "#######0.00")
        End If

        goods_value = Yarn_value + pavu_value

        dt2.Clear()
        dt3.Clear()

        prn_Count = prn_Count + 1

        prn_OriDupTri = ""
        If Trim(prn_InpOpts) <> "" Then
            If prn_Count <= Len(Trim(prn_InpOpts)) Then

                S = Mid$(Trim(prn_InpOpts), prn_Count, 1)

                If Val(S) = 1 Then
                    prn_OriDupTri = "ORIGINAL"
                ElseIf Val(S) = 2 Then
                    prn_OriDupTri = "DUPLICATE"
                ElseIf Val(S) = 3 Then
                    prn_OriDupTri = "TRIPLICATE"
                Else
                    If Trim(prn_InpOpts) <> "0" And Val(prn_InpOpts) = "0" And Len(Trim(prn_InpOpts)) > 2 Then
                        prn_OriDupTri = Trim(prn_InpOpts)
                    End If
                End If
            End If
        End If

        p1Font = New Font("Calibri", 20, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "FORM JJ", LMargin + 10, CurY - TxtHgt - 10, 0, 0, p1Font)

        If Trim(prn_OriDupTri) <> "" Then
            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_OriDupTri), PageWidth - 10, CurY - TxtHgt, 1, 0, pFont)
        End If

        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(1) = CurY

        Desc = ""
        Cmp_Name = "" : Cmp_Add1 = "" : Cmp_Add2 = ""
        Cmp_PhNo = "" : Cmp_TinNo = "" : Cmp_CstNo = ""

        Desc = prn_HdDt.Rows(0).Item("Company_Description").ToString
        Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString
        Cmp_Add1 = prn_HdDt.Rows(0).Item("Company_Address1").ToString & " " & prn_HdDt.Rows(0).Item("Company_Address2").ToString
        Cmp_Add2 = prn_HdDt.Rows(0).Item("Company_Address3").ToString & " " & prn_HdDt.Rows(0).Item("Company_Address4").ToString
        Cmp_Add4 = prn_HdDt.Rows(0).Item("Company_Address4").ToString

        If Trim(prn_HdDt.Rows(0).Item("Company_TinNo").ToString) <> "" Then
            Cmp_TinNo = "TIN NO.: " & prn_HdDt.Rows(0).Item("Company_TinNo").ToString
        End If
        If Trim(prn_HdDt.Rows(0).Item("Company_CstNo").ToString) <> "" Then
            Cmp_CstNo = "CST NO.: " & prn_HdDt.Rows(0).Item("Company_CstNo").ToString
        End If

        p1Font = New Font("Calibri", 15, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "DELIVERY NOTE", LMargin, CurY, 2, PrintWidth, p1Font)
        CurY = CurY + TxtHgt + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(2) = CurY

        CurY = CurY + TxtHgt - 10
        Common_Procedures.Print_To_PrintDocument(e, "[See rule 15(3), 15(18), 15(19), 15(20), 15(21)]", LMargin, CurY, 2, PrintWidth, pFont)
        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, "(for sales / stock transfer / works contract / labour)", LMargin, CurY, 2, PrintWidth, pFont)
        CurY = CurY + TxtHgt

        Common_Procedures.Print_To_PrintDocument(e, "Asst Year : " & Trim(Common_Procedures.FnYearCode), LMargin, CurY, 2, PrintWidth, pFont)
        'Common_Procedures.Print_To_PrintDocument(e, "Asst Year : 15-16", LMargin, CurY, 2, PrintWidth, pFont)
        Common_Procedures.Print_To_PrintDocument(e, Cmp_TinNo, LMargin + 10, CurY, 0, 0, pFont)
        If prn_HdDt.Rows(0).Item("JJ_FormNo").ToString <> "" Then
            Common_Procedures.Print_To_PrintDocument(e, "NO  :  " & prn_HdDt.Rows(0).Item("JJ_FormNo").ToString, PageWidth - 10, CurY, 1, 0, pFont)
        Else
            Common_Procedures.Print_To_PrintDocument(e, "NO  :  " & prn_HdDt.Rows(0).Item("JobWork_Weaving_PavuYarn_Delivery_No").ToString, PageWidth - 10, CurY, 1, 0, pFont)
        End If

        CurY = CurY + TxtHgt
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(3) = CurY

        Try
            C1 = ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5)

            W1 = e.Graphics.MeasureString("INVOICE DATE  : ", pFont).Width
            S1 = e.Graphics.MeasureString("TO     :    ", pFont).Width
            W2 = e.Graphics.MeasureString("Despatch To   : ", pFont).Width
            S2 = e.Graphics.MeasureString("Sent Through  : ", pFont).Width

            CurY = CurY + TxtHgt - 10
            Common_Procedures.Print_To_PrintDocument(e, "1.(a) Name and address of the", LMargin + 10, CurY, 0, 0, pFont)
            p1Font = New Font("Calibri", 12, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, Cmp_Name, LMargin + C1 + 10, CurY, 0, 0, p1Font)
            strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "Consigner", LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Cmp_Add1, LMargin + C1 + 10, CurY, 0, 0, p1Font)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, Cmp_Add2, LMargin + C1 + 10, CurY, 0, 0, p1Font)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "(b).TIN", LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Cmp_TinNo, LMargin + C1 + 10, CurY, 0, 0, p1Font)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "(c).CST Registration No", LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Cmp_CstNo, LMargin + C1 + 10, CurY, 0, 0, p1Font)

            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(4) = CurY

            CurY = CurY + TxtHgt - 10
            Common_Procedures.Print_To_PrintDocument(e, "2.(a) Name and address of the", LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Del_Name").ToString, LMargin + C1 + 10, CurY, 0, 0, p1Font)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "consignee / branch / agent", LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Ledger_Address1").ToString, LMargin + C1 + 10, CurY, 0, 0, p1Font)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Ledger_Address2").ToString, LMargin + C1 + 10, CurY, 0, 0, p1Font)
            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Ledger_Address4").ToString, LMargin + C1 + 10, CurY, 0, 0, p1Font)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "(b).TIN", LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Ledger_TinNo").ToString, LMargin + C1 + 10, CurY, 0, 0, p1Font)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "(c).CST Registration No.", LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Ledger_CstNo").ToString, LMargin + C1 + 10, CurY, 0, 0, p1Font)

            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(5) = CurY

            CurY = CurY + TxtHgt - 10
            Common_Procedures.Print_To_PrintDocument(e, "3 Address", LMargin + 10, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "(i) from which goods are consigned.", LMargin + 10, CurY, 0, 0, pFont)

            If Trim(prn_HdDt.Rows(0).Item("Area_Name").ToString) <> "" Then
                Cmp_Add4 = Trim(prn_HdDt.Rows(0).Item("Area_Name").ToString)

            ElseIf Trim(prn_HdDt.Rows(0).Item("Company_Address4").ToString) <> "" Then
                Cmp_Add4 = Trim(prn_HdDt.Rows(0).Item("Company_Address4").ToString)

            ElseIf Trim(prn_HdDt.Rows(0).Item("Company_Address3").ToString) <> "" Then
                Cmp_Add4 = Trim(prn_HdDt.Rows(0).Item("Company_Address3").ToString)

            ElseIf Trim(prn_HdDt.Rows(0).Item("Company_Address2").ToString) <> "" Then
                Cmp_Add4 = Trim(prn_HdDt.Rows(0).Item("Company_Address2").ToString)

            End If

            Common_Procedures.Print_To_PrintDocument(e, Cmp_Add4, LMargin + C1 + 10, CurY, 0, 0, p1Font)

            If Trim(prn_HdDt.Rows(0).Item("Ledger_Address4").ToString) <> "" Then
                To_Add = Trim(prn_HdDt.Rows(0).Item("Ledger_Address4").ToString)

            ElseIf Trim(prn_HdDt.Rows(0).Item("Ledger_Address3").ToString) <> "" Then
                To_Add = Trim(prn_HdDt.Rows(0).Item("Ledger_Address3").ToString)

            ElseIf Trim(prn_HdDt.Rows(0).Item("Ledger_Address2").ToString) <> "" Then
                To_Add = Trim(prn_HdDt.Rows(0).Item("Ledger_Address2").ToString)

            End If

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "(ii) to which goods are consigned.", LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, To_Add, LMargin + C1 + 10, CurY, 0, 0, p1Font)

            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(6) = CurY

            CurY = CurY + TxtHgt - 10
            Common_Procedures.Print_To_PrintDocument(e, "4.Description of goods consigned", LMargin + 10, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "(a) Name of the goods", LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "Pavu & Yarn Bags", LMargin + C1 + 10, CurY, 0, 0, p1Font)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "(b) Quantity Or Weight", LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "Yarn Bags : " & prn_HdDt.Rows(0).Item("Total_Bags").ToString & "      Pavu Meters : " & prn_HdDt.Rows(0).Item("Total_Meters").ToString, LMargin + C1 + 10, CurY, 0, 0, p1Font)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "(c) Value of the goods", LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "Rs. " & Common_Procedures.Currency_Format(Val(goods_value)), LMargin + C1 + 10, CurY, 0, 0, p1Font)

            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(7) = CurY

            CurY = CurY + TxtHgt - 10
            Common_Procedures.Print_To_PrintDocument(e, "5.Purpose of transport", LMargin + 10, CurY, 0, 0, pFont)
            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "(a) for sale / purchase", LMargin + 10, CurY, 0, 0, pFont)
            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "(b) for shipment", LMargin + 10, CurY, 0, 0, pFont)
            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "(c) transfer to branch/head office", LMargin + 10, CurY, 0, 0, pFont)
            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "/Consignment agent", LMargin + 10, CurY, 0, 0, pFont)
            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "(d) for executionof works contract", LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "(e) FOR LABOUR WORK / PROCESSING", LMargin + C1 + 10, CurY, 0, 0, p1Font)
            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "(e) for labour work / processing", LMargin + 10, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(8) = CurY

            CurY = CurY + TxtHgt - 10
            Common_Procedures.Print_To_PrintDocument(e, "6.To Whom delivered for transport", LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Transport_Name").ToString, LMargin + C1 + 10, CurY, 0, 0, p1Font)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "and vehicle no, if any", LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Vehicle_No").ToString, LMargin + C1 + 10, CurY, 0, 0, p1Font)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "7.Remarks, if any", LMargin + 10, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(9) = CurY

            CurY = CurY + TxtHgt - 10
            Common_Procedures.Print_To_PrintDocument(e, "SNO", LMargin, CurY, 2, ClArr(1), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "TYPE", LMargin + ClArr(1), CurY, 2, ClArr(2), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "MILL NAME", LMargin + ClArr(1) + ClArr(2), CurY, 2, ClArr(3), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "COUNT", LMargin + ClArr(1) + ClArr(2) + ClArr(3), CurY, 2, ClArr(4), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "BAGS", LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4), CurY, 2, ClArr(5), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "CONES", LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5), CurY, 2, ClArr(6), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "WEIGHT", LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6), CurY, 2, ClArr(7), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "PAVU DETAILS", LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7), CurY, 2, ClArr(8), pFont)

            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(10) = CurY


        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub Printing_FormJJ_PageFooter(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClArr() As Single, ByVal NoofDets As Integer, ByVal is_LastPage As Boolean)
        Dim p1Font As Font
        Dim I As Integer
        Dim W1 As Single = 0
        Dim vprn_BlNos As String = ""
        Dim BankNm1 As String = ""
        Dim BankNm2 As String = ""
        Dim BankNm3 As String = ""
        Dim BankNm4 As String = ""
        Dim Cmp_Name As String

        Try
            W1 = e.Graphics.MeasureString("No.of Beams  : ", pFont).Width

            For I = NoofDets + 1 To NoofItems_PerPage

                CurY = CurY + TxtHgt



                If prn_DetIndx = 0 Then
                    Common_Procedures.Print_To_PrintDocument(e, "Ends Count", LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + 10, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + W1 + 10, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, Trim(vPrn_PvuEdsCnt), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + W1 + 25, CurY, 0, 0, pFont)

                ElseIf prn_DetIndx = 1 Then
                    Common_Procedures.Print_To_PrintDocument(e, "No.of Beams", LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + 10, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + W1 + 10, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, Trim(Val(vPrn_PvuTotBms)), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + W1 + 25, CurY, 0, 0, pFont)

                ElseIf prn_DetIndx = 2 Then
                    Common_Procedures.Print_To_PrintDocument(e, "Meters", LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + 10, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + W1 + 10, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, Trim(Format(Val(vPrn_PvuTotMtrs), "#########0.00")), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + W1 + 25, CurY, 0, 0, pFont)

                ElseIf prn_DetIndx = 3 Then
                    Common_Procedures.Print_To_PrintDocument(e, "Noof_Used", LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + 10, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + W1 + 10, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, Trim(Val(vPrn_PvuNPcs)), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + W1 + 25, CurY, 0, 0, pFont)


                ElseIf prn_DetIndx = 4 Then
                    Common_Procedures.Print_To_PrintDocument(e, "Set No", LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + 10, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + W1 + 10, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, Trim(vPrn_PvuSetNo), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + W1 + 25, CurY, 0, 0, pFont)

                ElseIf prn_DetIndx = 5 Then
                    Common_Procedures.Print_To_PrintDocument(e, "Beam No", LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + 10, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + W1 + 10, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, Trim(vPrn_PvuBmNos1), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + W1 + 25, CurY, 0, 0, pFont)

                ElseIf prn_DetIndx = 6 Then
                    Common_Procedures.Print_To_PrintDocument(e, Trim(vPrn_PvuBmNos2), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + 50, CurY, 0, 0, pFont)

                ElseIf prn_DetIndx = 7 Then
                    Common_Procedures.Print_To_PrintDocument(e, Trim(vPrn_PvuBmNos3), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + 50, CurY, 0, 0, pFont)

                ElseIf prn_DetIndx = 8 Then
                    Common_Procedures.Print_To_PrintDocument(e, Trim(vPrn_PvuBmNos4), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + 50, CurY, 0, 0, pFont)

                End If

                prn_DetIndx = prn_DetIndx + 1

            Next

            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(5) = CurY

            CurY = CurY + TxtHgt - 10
            If is_LastPage = True Then
                Common_Procedures.Print_To_PrintDocument(e, " TOTAL", LMargin + ClArr(1) + ClArr(2) + 30, CurY, 2, ClArr(4), pFont)
            End If

            If Val(prn_HdDt.Rows(0).Item("Total_Bags").ToString) <> 0 Then
                If is_LastPage = True Then
                    Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(0).Item("Total_Bags").ToString), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) - 10, CurY, 1, 0, pFont)
                End If
            End If
            If Val(prn_HdDt.Rows(0).Item("Total_Cones").ToString) <> 0 Then
                If is_LastPage = True Then
                    Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(0).Item("Total_Cones").ToString), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) - 10, CurY, 1, 0, pFont)
                End If
            End If
            If Val(prn_HdDt.Rows(0).Item("Total_Weight").ToString) <> 0 Then
                If is_LastPage = True Then
                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Total_Weight").ToString), "#########0.000"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) - 10, CurY, 1, 0, pFont)
                End If
            End If


            CurY = CurY + TxtHgt - 15

            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(6) = CurY

            e.Graphics.DrawLine(Pens.Black, LMargin + ClArr(1), CurY, LMargin + ClArr(1), LnAr(9))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClArr(1) + ClArr(2) + ClArr(3), CurY, LMargin + ClArr(1) + ClArr(2) + ClArr(3), LnAr(9))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClArr(1) + ClArr(2), CurY, LMargin + ClArr(1) + ClArr(2), LnAr(9))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4), CurY, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4), LnAr(9))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5), CurY, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6), CurY, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6), LnAr(9))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7), CurY, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7), LnAr(9))

            CurY = CurY + TxtHgt - 10
            Common_Procedures.Print_To_PrintDocument(e, "We certify that to the best of my/our knowledge the particulare are true, correct and complete.", LMargin + 10, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt + 10
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(12) = CurY

            CurY = CurY + TxtHgt + 5

            p1Font = New Font("Calibri", 12, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "Signature :", LMargin + 10, CurY, 0, 0, p1Font)

            p1Font = New Font("Calibri", 12, FontStyle.Bold)
            Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString

            Common_Procedures.Print_To_PrintDocument(e, "For " & Cmp_Name, PageWidth - 15, CurY, 1, 0, p1Font)

            'Common_Procedures.Print_To_PrintDocument(e, "Signature :", LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + 10, CurY, 0, 0, p1Font)

            CurY = CurY + TxtHgt + 10
            CurY = CurY + TxtHgt

            Common_Procedures.Print_To_PrintDocument(e, "Name :", LMargin + 10, CurY, 0, 0, p1Font)

            Common_Procedures.Print_To_PrintDocument(e, "AUTHORISED SIGNATORY ", PageWidth - 5, CurY, 1, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, "Name :", LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + 10, CurY, 0, 0, p1Font)

            CurY = CurY + TxtHgt + 10
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(13) = CurY

            CurY = CurY + TxtHgt - 10
            Common_Procedures.Print_To_PrintDocument(e, "Name and signature of the person to whom the goods were", LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "Name and signature of the consigner /", LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + 10, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "delivered for transporting with status of person signing", LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "his employee / his representative", LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + 10, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            CurY = CurY + TxtHgt

            p1Font = New Font("Calibri", 12, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "Place : ", LMargin + 10, CurY, 0, 0, p1Font)
            Common_Procedures.Print_To_PrintDocument(e, "Date : " & Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("JobWork_Weaving_PavuYarn_Delivery_Date").ToString), "dd-MM-yyyy"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + 10, CurY, 0, 0, p1Font)

            CurY = CurY + TxtHgt + 10

            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            e.Graphics.DrawLine(Pens.Black, LMargin, LnAr(1), LMargin, CurY)
            e.Graphics.DrawLine(Pens.Black, PageWidth, LnAr(1), PageWidth, CurY)

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub cbo_Transport_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Transport.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'TRANSPORT')", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_Transport_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Transport.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Transport, Nothing, cbo_VehicleNo, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'TRANSPORT')", "(Ledger_IdNo = 0)")

        If (e.KeyValue = 38 And cbo_Transport.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then

            SendKeys.Send("+{TAB}")

        End If

    End Sub

    Private Sub txt_Frieght_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_Freight.KeyDown
        If e.KeyCode = 40 Then SendKeys.Send("{TAB}")
        If e.KeyCode = 38 Then cbo_VehicleNo.Focus()
    End Sub

    Private Sub cbo_Transport_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Transport.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Transport, cbo_VehicleNo, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'TRANSPORT')", "(Ledger_IdNo = 0)")

    End Sub

    Private Sub cbo_Transport_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Transport.KeyUp
        If e.Control = False And e.KeyValue = 17 Then
            Common_Procedures.UR.Ledr_Wea_Siz_Rw_Trans_JbWrk_Creation = Common_Procedures.UR.Ledger_Creation
            Common_Procedures.MDI_LedType = "TRANSPORT"
            Dim f As New Ledger_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Transport.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If
    End Sub

    Private Sub txt_KuraiPavuBeam_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_KuraiPavuBeam.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub txt_KuraiPavuMeters_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_KuraiPavuMeters.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub txt_Frieght_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Freight.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
        If Asc(e.KeyChar) = 13 Then
            dgv_YarnDetails.Focus()
            dgv_YarnDetails.CurrentCell = dgv_YarnDetails.Rows(0).Cells(1)
            dgv_YarnDetails.CurrentCell.Selected = True
        End If
    End Sub

    Private Sub txt_Note_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_Note.KeyDown
        If e.KeyCode = 40 Then
            If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                save_record()
            Else
                msk_Date.Focus()

            End If
        End If

        If e.KeyCode = 38 Then
            dgv_YarnDetails.Focus()
            dgv_YarnDetails.CurrentCell = dgv_YarnDetails.Rows(0).Cells(1)
            dgv_YarnDetails.CurrentCell.Selected = True
        End If
    End Sub

    Private Sub txt_Note_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Note.KeyPress
        If Asc(e.KeyChar) = 13 Then
            If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                save_record()
            Else
                msk_Date.Focus()

            End If
        End If
    End Sub

    Private Sub cbo_JobworkerName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_JobworkerName.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_IdNo = 0 or Ledger_Type = 'JOBWORKER')", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_JobworkerName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_JobworkerName.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_JobworkerName, txt_PartyDcNo, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_IdNo = 0 or Ledger_Type = 'JOBWORKER')", "(Ledger_IdNo = 0)")
        If Asc(e.KeyChar) = 13 Then

            If MessageBox.Show("Do you want to select Pavu  :", "FOR PAVU SELECTION...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = DialogResult.Yes Then
                btn_Selection_Click(sender, e)

            Else
                txt_PartyDcNo.Focus()

            End If

        End If
    End Sub

    Private Sub cbo_JobworkerName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_JobworkerName.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_JobworkerName, cbo_DeliveryTo, txt_PartyDcNo, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_IdNo = 0 or Ledger_Type = 'JOBWORKER')", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_JobworkerName_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_JobworkerName.KeyUp
        If e.Control = False And e.KeyValue = 17 Then

            Common_Procedures.MDI_LedType = "JOBWORKER"
            Dim f As New Ledger_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_JobworkerName.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()
        End If
    End Sub

    Private Sub btn_Print_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Print.Click
        print_record()
    End Sub

    Private Sub btn_Selection_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Selection.Click
        Dim Cmd As New SqlClient.SqlCommand
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim i As Integer, j As Integer, n As Integer, SNo As Integer
        Dim LedNo As Integer
        Dim NewCode As String

        LedNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_JobworkerName.Text)

        If LedNo = 0 Then
            MessageBox.Show("Invalid Party Name", "DOES NOT SELECT PAVU...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If cbo_JobworkerName.Enabled And cbo_JobworkerName.Visible Then cbo_JobworkerName.Focus()
            Exit Sub
        End If

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Cmd.Connection = con

        Cmd.CommandText = "truncate table " & Trim(Common_Procedures.EntryTempSubTable) & ""
        Cmd.ExecuteNonQuery()

        With dgv_Selection

            chk_SelectAll.Checked = False

            .Rows.Clear()

            SNo = 0

            Da = New SqlClient.SqlDataAdapter("select a.noof_used as Ent_NoofUsed, b.*, c.EndsCount_Name, d.Beam_Width_Name from JobWork_Weaving_Pavu_Delivery_Details a INNER JOIN Stock_SizedPavu_Processing_Details b ON a.Set_Code = b.Set_Code and a.Beam_No = b.Beam_No INNER JOIN EndsCount_Head c ON a.EndsCount_IdNo = c.EndsCount_IdNo LEFT OUTER JOIN Beam_Width_Head d ON b.Beam_Width_Idno = d.Beam_Width_Idno where a.company_idno = " & Str(Val(lbl_Company.Tag)) & " and a.JobWork_Weaving_PavuYarn_Delivery_Code = '" & Trim(NewCode) & "' and a.ReceivedFrom_IdNo = " & Str(Val(LedNo)) & " order by a.for_orderby, a.sl_no, a.Set_Code, b.ForOrderBy_BeamNo, a.Beam_No", con)
            Dt1 = New DataTable
            Da.Fill(Dt1)

            If Dt1.Rows.Count > 0 Then

                For i = 0 To Dt1.Rows.Count - 1

                    n = .Rows.Add()

                    SNo = SNo + 1
                    .Rows(n).Cells(0).Value = Val(SNo)
                    .Rows(n).Cells(1).Value = Dt1.Rows(i).Item("Set_No").ToString
                    .Rows(n).Cells(2).Value = Dt1.Rows(i).Item("Beam_No").ToString
                    .Rows(n).Cells(3).Value = Val(Dt1.Rows(i).Item("Noof_Pcs").ToString)
                    If Common_Procedures.settings.Weaver_Zari_Kuri_Entries_Status = 1 Then

                        .Rows(n).Cells(4).Value = Format(Val(Dt1.Rows(i).Item("Meters").ToString), "#########0.000")

                    Else
                        .Rows(n).Cells(4).Value = Format(Val(Dt1.Rows(i).Item("Meters").ToString), "#########0.00")

                    End If

                    .Rows(n).Cells(5).Value = Dt1.Rows(i).Item("EndsCount_Name").ToString
                    .Rows(n).Cells(6).Value = Dt1.Rows(i).Item("Beam_Width_Name").ToString
                    .Rows(n).Cells(7).Value = "1"
                    .Rows(n).Cells(8).Value = Dt1.Rows(i).Item("Ent_NoofUsed").ToString
                    .Rows(n).Cells(9).Value = Dt1.Rows(i).Item("Set_Code").ToString
                    .Rows(n).Cells(10).Value = Dt1.Rows(i).Item("Pavu_Delivery_Increment").ToString

                    For j = 0 To .ColumnCount - 1
                        .Rows(i).Cells(j).Style.ForeColor = Color.Red
                        If Val(.Rows(n).Cells(8).Value) <> Val(.Rows(n).Cells(8).Value) Then
                            .Rows(i).Cells(j).Style.BackColor = Color.Gray
                        End If
                    Next

                    Cmd.CommandText = "Insert into " & Trim(Common_Procedures.EntryTempSubTable) & " ( Int1, Name1, Name2, Meters1 ) Values (" & Str(Val(SNo)) & ", '" & Trim(Dt1.Rows(i).Item("Set_Code").ToString) & "', '" & Trim(Dt1.Rows(i).Item("Beam_No").ToString) & "', " & Str(Val(Dt1.Rows(i).Item("Meters").ToString)) & ") "
                    Cmd.ExecuteNonQuery()

                Next

            End If
            Dt1.Clear()

            Da = New SqlClient.SqlDataAdapter("select a.*, b.EndsCount_Name, c.Beam_Width_Name from Stock_SizedPavu_Processing_Details a LEFT OUTER JOIN EndsCount_Head b ON a.EndsCount_IdNo = b.EndsCount_IdNo LEFT OUTER JOIN Beam_Width_Head c ON a.Beam_Width_Idno = c.Beam_Width_Idno where a.StockAt_IdNo = " & Str(Val(LedNo)) & " and (a.Pavu_Delivery_Code = '' and a.Beam_Knotting_Code = '' and a.Close_Status = 0) order by a.for_orderby, a.Set_Code, a.ForOrderBy_BeamNo, a.Beam_No, a.sl_no", con)
            Dt1 = New DataTable
            Da.Fill(Dt1)

            If Dt1.Rows.Count > 0 Then

                For i = 0 To Dt1.Rows.Count - 1

                    n = .Rows.Add()

                    SNo = SNo + 1
                    .Rows(n).Cells(0).Value = Val(SNo)
                    .Rows(n).Cells(1).Value = Dt1.Rows(i).Item("Set_No").ToString
                    .Rows(n).Cells(2).Value = Dt1.Rows(i).Item("Beam_No").ToString
                    .Rows(n).Cells(3).Value = Val(Dt1.Rows(i).Item("Noof_Pcs").ToString)
                    If Common_Procedures.settings.Weaver_Zari_Kuri_Entries_Status = 1 Then

                        .Rows(n).Cells(4).Value = Format(Val(Dt1.Rows(i).Item("Meters").ToString) - Val(Dt1.Rows(i).Item("Production_Meters").ToString), "#########0.000")

                    Else
                        .Rows(n).Cells(4).Value = Format(Val(Dt1.Rows(i).Item("Meters").ToString) - Val(Dt1.Rows(i).Item("Production_Meters").ToString), "#########0.00")

                    End If
                    '.Rows(n).Cells(4).Value = Format(Val(Dt1.Rows(i).Item("Meters").ToString), "#########0.00")

                    .Rows(n).Cells(5).Value = Dt1.Rows(i).Item("EndsCount_Name").ToString
                    .Rows(n).Cells(6).Value = Dt1.Rows(i).Item("Beam_Width_Name").ToString
                    .Rows(n).Cells(7).Value = ""
                    .Rows(n).Cells(8).Value = "-9999"
                    .Rows(n).Cells(9).Value = Dt1.Rows(i).Item("Set_Code").ToString
                    .Rows(n).Cells(10).Value = Dt1.Rows(i).Item("Pavu_Delivery_Increment").ToString

                Next

            End If
            Dt1.Clear()

        End With

        pnl_Selection.Visible = True
        pnl_Back.Enabled = False
        If txt_BeamNoSelection.Enabled And txt_BeamNoSelection.Visible Then txt_BeamNoSelection.Focus()
        'If dgv_Selection.Rows.Count > 0 Then
        '    dgv_Selection.Focus()
        '    dgv_Selection.CurrentCell = dgv_Selection.Rows(0).Cells(0)
        'End If

    End Sub

    Private Sub dgv_Selection_CellClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Selection.CellClick
        Select_Pavu(e.RowIndex)
    End Sub

    Private Sub Select_Pavu(ByVal RwIndx As Integer)
        Dim Cmd As New SqlClient.SqlCommand
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim i As Integer = 0
        Dim MxId As Integer = 0

        Try

            With dgv_Selection

                Cmd.Connection = con

                If .RowCount > 0 And RwIndx >= 0 Then

                    If Val(.Rows(RwIndx).Cells(8).Value) > 0 And Val(.Rows(RwIndx).Cells(8).Value) <> Val(.Rows(RwIndx).Cells(10).Value) Then
                        MessageBox.Show("Cannot deselect" & Chr(13) & "Already this pavu delivered to others")
                        Exit Sub
                    End If

                    .Rows(RwIndx).Cells(7).Value = (Val(.Rows(RwIndx).Cells(7).Value) + 1) Mod 2

                    If Val(.Rows(RwIndx).Cells(7).Value) = 1 Then
                        For i = 0 To .ColumnCount - 1
                            .Rows(RwIndx).Cells(i).Style.ForeColor = Color.Red
                        Next

                        MxId = Common_Procedures.get_MaxIdNo(con, "" & Trim(Common_Procedures.EntryTempSubTable) & "", "Int1", "")

                        Cmd.CommandText = "Insert into " & Trim(Common_Procedures.EntryTempSubTable) & " ( Int1, Name1, Name2, Meters1 ) Values (" & Str(Val(MxId)) & ", '" & Trim(.Rows(RwIndx).Cells(9).Value) & "', '" & Trim(.Rows(RwIndx).Cells(2).Value) & "', " & Str(Val(.Rows(RwIndx).Cells(4).Value)) & " ) "
                        Cmd.ExecuteNonQuery()

                    Else
                        .Rows(RwIndx).Cells(7).Value = ""
                        For i = 0 To .ColumnCount - 1
                            .Rows(RwIndx).Cells(i).Style.ForeColor = Color.Black
                        Next

                        Cmd.CommandText = "Delete from " & Trim(Common_Procedures.EntryTempSubTable) & " where Name1 = '" & Trim(.Rows(RwIndx).Cells(9).Value) & "' and Name2 = '" & Trim(.Rows(RwIndx).Cells(2).Value) & "'"
                        Cmd.ExecuteNonQuery()

                    End If

                End If

            End With

        Catch ex As Exception

        End Try
    End Sub

    Private Sub dgv_Selection_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_Selection.KeyDown
        On Error Resume Next

        If e.KeyCode = Keys.Enter Or e.KeyCode = Keys.Space Then
            If dgv_Selection.CurrentCell.RowIndex >= 0 Then
                Select_Pavu(dgv_Selection.CurrentCell.RowIndex)
                e.Handled = True
            End If
        End If
    End Sub

    Private Sub btn_Close_Selection_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Close_Selection.Click
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim n As Integer
        Dim sno As Integer
        Dim I As Integer, J As Integer
        Try


            With dgv_PavuDetails

                .Rows.Clear()

                Da = New SqlClient.SqlDataAdapter("Select * from " & Trim(Common_Procedures.EntryTempSubTable) & " Where Name1 <> '' and Name2 <> '' Order by Int1 ", con)
                Dt1 = New DataTable
                Da.Fill(Dt1)
                If Dt1.Rows.Count > 0 Then
                    For J = 0 To Dt1.Rows.Count - 1

                        For I = 0 To dgv_Selection.RowCount - 1

                            If Val(dgv_Selection.Rows(I).Cells(7).Value) = 1 And Trim(UCase(dgv_Selection.Rows(I).Cells(9).Value)) = Trim(UCase(Dt1.Rows(J).Item("Name1").ToString)) And Trim(UCase(dgv_Selection.Rows(I).Cells(2).Value)) = Trim(UCase(Dt1.Rows(J).Item("Name2").ToString)) Then

                                n = .Rows.Add()

                                sno = sno + 1
                                .Rows(n).Cells(0).Value = Val(sno)
                                .Rows(n).Cells(1).Value = dgv_Selection.Rows(I).Cells(1).Value
                                .Rows(n).Cells(2).Value = dgv_Selection.Rows(I).Cells(2).Value
                                .Rows(n).Cells(3).Value = dgv_Selection.Rows(I).Cells(3).Value
                                If Common_Procedures.settings.Weaver_Zari_Kuri_Entries_Status = 1 Then

                                    .Rows(n).Cells(4).Value = Format(Val(dgv_Selection.Rows(I).Cells(4).Value), "#########0.000")
                                Else
                                    .Rows(n).Cells(4).Value = Format(Val(dgv_Selection.Rows(I).Cells(4).Value), "#########0.00")

                                End If

                                .Rows(n).Cells(5).Value = dgv_Selection.Rows(I).Cells(5).Value
                                .Rows(n).Cells(6).Value = dgv_Selection.Rows(I).Cells(6).Value

                                .Rows(n).Cells(8).Value = ""

                                If Val(dgv_Selection.Rows(I).Cells(8).Value) > 0 Then

                                    If Val(dgv_Selection.Rows(I).Cells(8).Value) <> Val(dgv_Selection.Rows(I).Cells(10).Value) Then
                                        .Rows(n).Cells(7).Value = "1"
                                    Else
                                        .Rows(n).Cells(7).Value = ""
                                    End If

                                    .Rows(n).Cells(8).Value = dgv_Selection.Rows(I).Cells(8).Value

                                End If


                                .Rows(n).Cells(9).Value = dgv_Selection.Rows(I).Cells(9).Value

                                .Rows(n).Cells(10).Value = dgv_Selection.Rows(I).Cells(10).Value

                                Exit For

                            End If

                        Next I

                    Next J

                End If
                Dt1.Clear()


                For i = 0 To dgv_Selection.RowCount - 1

                    If Val(dgv_Selection.Rows(I).Cells(7).Value) = 1 And Trim(dgv_Selection.Rows(I).Cells(9).Value) <> "" And Trim(dgv_Selection.Rows(I).Cells(2).Value) <> "" Then

                        Da = New SqlClient.SqlDataAdapter("Select * from " & Trim(Common_Procedures.EntryTempSubTable) & " where Name1 = '" & Trim(dgv_Selection.Rows(I).Cells(9).Value) & "' and Name2 = '" & Trim(dgv_Selection.Rows(I).Cells(2).Value) & "' and Name1 <> '' and Name2 <> ''", con)
                        Dt1 = New DataTable
                        Da.Fill(Dt1)
                        If Dt1.Rows.Count = 0 Then

                            n = .Rows.Add()

                            sno = sno + 1
                            .Rows(n).Cells(0).Value = Val(sno)
                            .Rows(n).Cells(1).Value = dgv_Selection.Rows(I).Cells(1).Value
                            .Rows(n).Cells(2).Value = dgv_Selection.Rows(I).Cells(2).Value
                            .Rows(n).Cells(3).Value = dgv_Selection.Rows(I).Cells(3).Value
                            If Common_Procedures.settings.Weaver_Zari_Kuri_Entries_Status = 1 Then

                                .Rows(n).Cells(4).Value = Format(Val(dgv_Selection.Rows(I).Cells(4).Value), "#########0.000")

                            Else
                                .Rows(n).Cells(4).Value = Format(Val(dgv_Selection.Rows(I).Cells(4).Value), "#########0.00")

                            End If

                            .Rows(n).Cells(5).Value = dgv_Selection.Rows(I).Cells(5).Value
                            .Rows(n).Cells(6).Value = dgv_Selection.Rows(I).Cells(6).Value

                            .Rows(n).Cells(8).Value = ""

                            If Val(dgv_Selection.Rows(I).Cells(8).Value) > 0 Then

                                If Val(dgv_Selection.Rows(I).Cells(8).Value) <> Val(dgv_Selection.Rows(I).Cells(10).Value) Then
                                    .Rows(n).Cells(7).Value = "1"
                                Else
                                    .Rows(n).Cells(7).Value = ""
                                End If

                                .Rows(n).Cells(8).Value = dgv_Selection.Rows(I).Cells(8).Value

                            End If


                            .Rows(n).Cells(9).Value = dgv_Selection.Rows(I).Cells(9).Value

                            .Rows(n).Cells(10).Value = dgv_Selection.Rows(I).Cells(10).Value

                        End If
                        Dt1.Clear()

                    End If

                Next

            End With

            TotalPavu_Calculation()

            Grid_Cell_DeSelect()

            pnl_Back.Enabled = True
            pnl_Selection.Visible = False
            If txt_PartyDcNo.Enabled And txt_PartyDcNo.Visible Then txt_PartyDcNo.Focus()

        Catch ex As Exception

        End Try

    End Sub

    Private Sub dgv_YarnDetails_EditingControlShowing(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewEditingControlShowingEventArgs) Handles dgv_YarnDetails.EditingControlShowing
        dgtxt_Details = CType(dgv_YarnDetails.EditingControl, DataGridViewTextBoxEditingControl)
    End Sub

    Private Sub dgtxt_Details_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgtxt_Details.KeyUp
        If e.Control = True And UCase(Chr(e.KeyCode)) = "D" Then
            dgv_YarnDetails_KeyUp(sender, e)
        End If
    End Sub

    Private Sub txt_BeamNoSelection_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_BeamNoSelection.KeyDown
        If e.KeyValue = 40 Then
            If dgv_Selection.Rows.Count > 0 Then
                dgv_Selection.Focus()
                dgv_Selection.CurrentCell = dgv_Selection.Rows(0).Cells(0)
                dgv_Selection.CurrentCell.Selected = True
            End If
        End If
    End Sub

    Private Sub txt_BeamNoSelection_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_BeamNoSelection.KeyPress
        If Asc(e.KeyChar) = 13 Then
            If Trim(txt_BeamNoSelection.Text) <> "" Then
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

    Private Sub btn_Set_Bm_selection_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Set_Bm_selection.Click
        Dim StNo As String = ""
        Dim BmNo As String = ""
        Dim i As Integer = 0

        If Trim(txt_BeamNoSelection.Text) <> "" Then

            BmNo = Trim(txt_BeamNoSelection.Text)

            For i = 0 To dgv_Selection.Rows.Count - 1
                If Trim(UCase(BmNo)) = Trim(UCase(dgv_Selection.Rows(i).Cells(2).Value)) Then
                    Call Select_Pavu(i)

                    dgv_Selection.CurrentCell = dgv_Selection.Rows(i).Cells(0)
                    If i >= 11 Then dgv_Selection.FirstDisplayedScrollingRowIndex = i - 10

                    Exit For

                End If
            Next

            txt_BeamNoSelection.Text = ""
            If txt_BeamNoSelection.Enabled = True Then txt_BeamNoSelection.Focus()

        End If
    End Sub

    Private Sub chk_SelectAll_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chk_SelectAll.CheckedChanged
        Dim i As Integer
        Dim J As Integer

        With dgv_Selection

            For i = 0 To .Rows.Count - 1
                .Rows(i).Cells(7).Value = ""
                For J = 0 To .ColumnCount - 1
                    .Rows(i).Cells(J).Style.ForeColor = Color.Black
                Next J
            Next i

            If chk_SelectAll.Checked = True Then
                For i = 0 To .Rows.Count - 1
                    Select_Pavu(i)
                Next i
            End If

            If .Rows.Count > 0 Then
                .Focus()
                .CurrentCell = .Rows(0).Cells(0)
                .CurrentCell.Selected = True
            End If

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
    Private Sub msk_Date_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles msk_Date.LostFocus

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

            msk_Date.Text = dtp_Date.Text
            msk_Date.SelectionStart = 0
        End If
    End Sub

    Private Sub dtp_Date_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dtp_Date.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
    End Sub

    Private Sub dtp_Date_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dtp_Date.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            dtp_Date.Text = Date.Today
        End If
    End Sub
    Private Sub dtp_Date_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles dtp_Date.ValueChanged
        msk_Date.Text = dtp_Date.Text
    End Sub

    Private Sub dtp_Date_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles dtp_Date.Enter
        msk_Date.Focus()
        msk_Date.SelectionStart = 0
    End Sub

    Private Sub btn_Close_PrintRange_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Close_PrintRange.Click
        pnl_Back.Enabled = True
        pnl_PrintRange.Visible = False
    End Sub

    Private Sub btn_Cancel_PrintRange_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Cancel_PrintRange.Click
        pnl_Back.Enabled = True
        pnl_PrintRange.Visible = False
    End Sub

    Private Sub btn_Print_PrintRange_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Print_PrintRange.Click
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim I As Integer = 0
        Dim OrdBy_FrmNo As Single = 0, OrdByToNo As Single = 0

        Try

            If Val(txt_PrintRange_FromNo.Text) = 0 Then
                MessageBox.Show("Invalid From No", "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                txt_PrintRange_FromNo.Focus()
                Exit Sub
            End If

            If Val(txt_PrintRange_ToNo.Text) = 0 Then
                MessageBox.Show("Invalid To No", "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                txt_PrintRange_ToNo.Focus()
                Exit Sub
            End If

            prn_FromNo = Trim(txt_PrintRange_FromNo.Text)
            prn_ToNo = Trim(txt_PrintRange_ToNo.Text)

            btn_Close_PrintRange_Click(sender, e)

            prn_Status = 1
            Printing_Delivery()

        Catch ex As Exception
            MsgBox("The printing operation failed" & vbCrLf & ex.Message, MsgBoxStyle.Critical, "DOES NOT PRINT...")

        Finally
            dt1.Dispose()
            da1.Dispose()

        End Try

    End Sub

    Private Sub txt_PrintRange_ToNo_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_PrintRange_ToNo.KeyDown
        If e.KeyValue = 38 Then txt_PrintRange_FromNo.Focus()
        If e.KeyValue = 40 Then btn_Print_PrintRange.Focus()
    End Sub

    Private Sub txt_PrintRange_ToNo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_PrintRange_ToNo.KeyPress
        If Asc(e.KeyChar) = 13 Then
            btn_Print_PrintRange_Click(sender, e)
        End If
    End Sub

    Private Sub txt_NoOfBobin_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_NoOfBobin.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub cbo_JobworkerName_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbo_JobworkerName.SelectedIndexChanged

    End Sub

    Private Sub cbo_DeliveryTo_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbo_DeliveryTo.SelectedIndexChanged

    End Sub

    Private Sub btn_SaveAll_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_SaveAll.Click

        SaveAll_STS = True

        LastNo = ""
        movelast_record()

        LastNo = lbl_DcNo.Text

        movefirst_record()
        Timer1.Enabled = True

    End Sub

    Private Sub Timer1_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer1.Tick
        save_record()
        If Trim(UCase(LastNo)) = Trim(UCase(lbl_DcNo.Text)) Then
            Timer1.Enabled = False
            SaveAll_STS = False
            MessageBox.Show("All entries saved sucessfully", "FOR SAVING...", MessageBoxButtons.OK, MessageBoxIcon.Information)

        Else
            movenext_record()

        End If
    End Sub

   
End Class