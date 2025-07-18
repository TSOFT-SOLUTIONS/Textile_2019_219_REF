Imports System.IO
Public Class Pavu_Yarn_Delivery
    Implements Interface_MDIActions

    Private con As New SqlClient.SqlConnection(Common_Procedures.Connection_String)
    Private con1 As New SqlClient.SqlConnection(Common_Procedures.ConnectionString_CompanyGroupdetails)
    Private FrmLdSTS As Boolean = False
    Private New_Entry As Boolean = False
    Private Insert_Entry As Boolean = False
    Private Filter_RowNo As Integer = -1
    Private Filter_Status As Boolean = False
    Private Pk_Condition As String = "PYDLV-"
    Private Pk_Condition1 As String = "PYDFP-"
    Private Pk_Condition2 As String = "PYDFY-"
    Private Prec_ActCtrl As New Control
    Private prn_HdDt As New DataTable
    Private prn_DetDt As New DataTable
    Private prn_DetDt1 As New DataTable
    Private prn_PageNo As Integer
    Private prn_HeadIndx As Integer
    Private prn_Prev_HeadIndx As Integer
    Private prn_DetIndx As Integer
    Private prn_DetSNo As Integer
    Private prn_PageSize_SetUP_STS As Boolean
    Private vcbo_KeyDwnVal As Double
    Private prn_FromNo As String
    Private prn_ToNo As String
    Private NoCalc_Status As Boolean = False
    Private WithEvents dgtxt_Details As New DataGridViewTextBoxEditingControl
    Private ContSts As Boolean = False
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
    Public vmskOldText As String = ""
    Public vmskSelStrt As Integer = -1
    Private vprn_Tot_Bgs_Bms As Integer = 0
    Private vprn_Tot_Wgt_Mtr As String = 0
    Private vprn_Tot_Amt As String = 0
    Private LastNo As String = ""
    Private SaveAll_Sts As Boolean = False
    Private Prnt_sts As Boolean = False

    Private vCLO_CONDT As String = ""
    Private FnYearCode1 As String = ""
    Private FnYearCode2 As String = ""


    Private Sub clear()

        NoCalc_Status = True
        New_Entry = False
        Insert_Entry = False
        chk_SelectAll.Checked = False

        chk_Verified_Status.Checked = False

        pnl_Back.Enabled = True
        pnl_Filter.Visible = False
        pnl_Selection.Visible = False
        pnl_Print.Visible = False
        pnl_PrintRange.Visible = False

        vmskOldText = ""
        vmskSelStrt = -1

        lbl_DcNo.Text = ""
        lbl_DcNo.ForeColor = Color.Black

        msk_Date.Text = ""
        dtp_Date.Text = ""
        txt_KuraiPavuBeam.Text = ""
        txt_PartyDcNo.Text = ""
        txt_JJFormNo.Text = ""
        txt_KuraiPavuMeters.Text = ""
        txt_NoOfBobin.Text = ""
        cbo_DelvAt.Text = ""
        cbo_DelvAt.Tag = ""
        cbo_EndsCount.Text = ""

        cbo_EndsCount.Text = ""
        cbo_Transport.Text = ""
        cbo_VehicleNo.Text = ""
        txt_Rate.Text = ""
        lbl_Amount.Text = ""
        lbl_Freight_Pavu.Text = ""
        cbo_TransportMode.Text = ""
        txt_DateTime_Of_Supply.Text = ""
        txt_place_Supply.Text = ""
        cbo_Grid_RateFor.Text = "BAG"
        cbo_PavuRecForm.Text = Common_Procedures.Ledger_IdNoToName(con, 4)
        cbo_YarnRecForm.Text = Common_Procedures.Ledger_IdNoToName(con, 4)
        lbl_UserName.Text = "USER : " & Trim(UCase(Common_Procedures.User_IdNoToName(con1, Common_Procedures.User.IdNo)))

        txt_Freight.Text = ""
        txt_Note.Text = ""
        rtbEWBResponse.Text = ""
        txt_Eway_Bill_No.Text = ""
        'txt_EWBNo.Text = ""
        lbl_Total_Value.Text = ""


        cbo_ClothSales_OrderCode_forSelection.Text = ""

        If cbo_WidthType.Visible Then cbo_WidthType.Text = ""
        dgv_PavuDetails.Rows.Clear()

        dgv_PavuDetails_Total.Rows.Clear()
        dgv_PavuDetails_Total.Rows.Add()

        dgv_YarnDetails.Rows.Clear()
        dgv_YarnDetails.Rows(0).Cells(2).Value = "MILL"

        dgv_YarnDetails_Total.Rows.Clear()
        dgv_YarnDetails_Total.Rows.Add()

        cbo_DelvAt.Enabled = True
        cbo_DelvAt.BackColor = Color.White

        cbo_PavuRecForm.Enabled = True
        cbo_PavuRecForm.BackColor = Color.White

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
        cbo_Grid_RateFor.Visible = False

        cbo_Grid_CountName.Tag = -1
        cbo_Grid_MillName.Tag = -1
        cbo_Grid_YarnType.Tag = -1
        cbo_Grid_RateFor.Tag = -1

        cbo_Grid_CountName.Text = ""
        cbo_Grid_MillName.Text = ""
        cbo_Grid_YarnType.Text = ""
        cbo_Grid_RateFor.Text = ""
        NoCalc_Status = False

        Grp_EWB.Visible = False




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
        NoCalc_Status = True

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(no) & "/" & Trim(Common_Procedures.FnYearCode)

        Try

            da1 = New SqlClient.SqlDataAdapter("select a.*, b.Ledger_Name as DelvName, c.Ledger_Name as TransportName, d.EndsCount_Name,e.Ledger_Name as RecFromName, f.Ledger_Name as Yarn_ReceivedFrom_Name from PavuYarn_Delivery_Head a INNER JOIN Ledger_Head b ON a.DeliveryTo_Idno = b.Ledger_IdNo LEFT OUTER JOIN Ledger_Head c ON a.Transport_IdNo = c.Ledger_IdNo LEFT OUTER JOIN EndsCount_Head d ON a.EndsCount_IdNo = d.EndsCount_IdNo LEFT OUTER JOIN Ledger_Head e ON a.ReceivedFrom_Idno = e.Ledger_IdNo LEFT OUTER JOIN Ledger_Head f ON a.Yarn_ReceivedFrom_IdNo = f.Ledger_IdNo Where a.PavuYarn_Delivery_Code = '" & Trim(NewCode) & "'", con)
            da1.Fill(dt1)

            If dt1.Rows.Count > 0 Then

                lbl_DcNo.Text = dt1.Rows(0).Item("PavuYarn_Delivery_No").ToString
                dtp_Date.Text = dt1.Rows(0).Item("PavuYarn_Delivery_Date").ToString
                msk_Date.Text = dtp_Date.Text

                cbo_DelvAt.Text = dt1.Rows(0).Item("DelvName").ToString
                txt_KuraiPavuBeam.Text = dt1.Rows(0).Item("Empty_Beam").ToString
                txt_NoOfBobin.Text = dt1.Rows(0).Item("Empty_Bobin").ToString
                txt_KuraiPavuMeters.Text = Val(dt1.Rows(0).Item("Meters").ToString)
                cbo_EndsCount.Text = dt1.Rows(0).Item("EndsCount_Name").ToString
                cbo_VehicleNo.Text = dt1.Rows(0).Item("Vehicle_No").ToString
                cbo_Transport.Text = dt1.Rows(0).Item("TransportName").ToString
                cbo_PavuRecForm.Text = dt1.Rows(0).Item("RecFromName").ToString
                cbo_YarnRecForm.Text = dt1.Rows(0).Item("Yarn_ReceivedFrom_Name").ToString
                txt_PartyDcNo.Text = dt1.Rows(0).Item("Party_DcNo").ToString
                txt_JJFormNo.Text = dt1.Rows(0).Item("JJ_FormNo").ToString
                txt_Freight.Text = Val(dt1.Rows(0).Item("Freight_Charge").ToString)
                txt_Note.Text = dt1.Rows(0).Item("Note").ToString
                txt_Rate.Text = (dt1.Rows(0).Item("Rate").ToString)
                lbl_Amount.Text = dt1.Rows(0).Item("Amount").ToString
                lbl_Freight_Pavu.Text = (dt1.Rows(0).Item("Freight_Pavu").ToString)
                cbo_TransportMode.Text = dt1.Rows(0).Item("Transportation_Mode").ToString
                txt_DateTime_Of_Supply.Text = dt1.Rows(0).Item("Date_Time_Of_Supply").ToString
                txt_place_Supply.Text = dt1.Rows(0).Item("Place_Of_Supply").ToString
                cbo_Cloth.Text = Common_Procedures.Cloth_IdNoToName(con, Val(dt1.Rows(0).Item("Cloth_IdNo").ToString))
                lbl_UserName.Text = "USER : " & Trim(UCase(Common_Procedures.User_IdNoToName(con1, Val(dt1.Rows(0).Item("User_IdNo").ToString))))
                cbo_WidthType.Text = dt1.Rows(0).Item("Width_Type").ToString
                If Val(dt1.Rows(0).Item("Verified_Status").ToString) = 1 Then chk_Verified_Status.Checked = True
                txt_Eway_Bill_No.Text = dt1.Rows(0).Item("Eway_BillNo").ToString
                cbo_RateFor.Text = dt1.Rows(0).Item("Rate_for").ToString
                lbl_Total_Value.Text = Format(Val(dt1.Rows(0).Item("Total_Delivery_Value").ToString), "########0.00")

                cbo_ClothSales_OrderCode_forSelection.Text = dt1.Rows(0).Item("ClothSales_OrderCode_forSelection").ToString

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


                da2 = New SqlClient.SqlDataAdapter("Select a.*, b.Pavu_Delivery_Increment, c.EndsCount_Name, d.Beam_Width_Name from Pavu_Delivery_Details a INNER JOIN Stock_SizedPavu_Processing_Details b ON a.Set_Code = b.Set_Code and a.Beam_No = b.Beam_No INNER JOIN EndsCount_Head c ON a.EndsCount_IdNo = c.EndsCount_IdNo LEFT OUTER JOIN Beam_Width_Head d ON a.Beam_Width_Idno = d.Beam_Width_Idno where a.PavuYarn_Delivery_Code = '" & Trim(NewCode) & "' Order by a.Sl_No", con)
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
                    .Rows(0).Cells(4).Value = Format(Val(dt1.Rows(0).Item("Total_Meters").ToString), "########0.00")
                    If Common_Procedures.settings.Weaver_Zari_Kuri_Entries_Status = 1 Then
                        .Rows(0).Cells(7).Value = Format(Val(dt1.Rows(0).Item("Total_Thiri").ToString), "########0.000")
                    End If
                    'If Common_Procedures.settings.Weaver_Zari_Kuri_Entries_Status = 1 Then

                    '  .Rows(0).Cells(4).Value = Format(Val(dt1.Rows(0).Item("Total_Meters").ToString), "########0.000")
                    'Else
                    '   .Rows(0).Cells(4).Value = Format(Val(dt1.Rows(0).Item("Total_Meters").ToString), "########0.00")
                    ' End If

                End With

                dt2.Clear()

                da2 = New SqlClient.SqlDataAdapter("select a.*, b.Count_Name, c.Mill_Name from Yarn_Delivery_Details a INNER JOIN Count_Head b on a.Count_IdNo = b.Count_IdNo INNER JOIN Mill_Head c on a.Mill_IdNo = c.Mill_IdNo where a.PavuYarn_Delivery_Code = '" & Trim(NewCode) & "' Order by a.sl_no", con)
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
                        dgv_YarnDetails.Rows(n).Cells(8).Value = dt2.Rows(i).Item("Rate_For").ToString
                        dgv_YarnDetails.Rows(n).Cells(9).Value = Format(Val(dt2.Rows(i).Item("Rate").ToString), "########0.00")
                        dgv_YarnDetails.Rows(n).Cells(10).Value = Format(Val(dt2.Rows(i).Item("Amount").ToString), "########0.000")
                    Next i

                End If

                With dgv_YarnDetails_Total
                    If .RowCount = 0 Then .Rows.Add()
                    .Rows(0).Cells(4).Value = Val(dt1.Rows(0).Item("Total_Bags").ToString)
                    .Rows(0).Cells(5).Value = Val(dt1.Rows(0).Item("Total_Cones").ToString)
                    .Rows(0).Cells(6).Value = Format(Val(dt1.Rows(0).Item("Total_Weight").ToString), "########0.000")
                    .Rows(0).Cells(7).Value = Format(Val(dt1.Rows(0).Item("Total_Thiri").ToString), "########0.000")
                    .Rows(0).Cells(10).Value = Format(Val(dt1.Rows(0).Item("Total_Amount").ToString), "########0.000")
                End With

                dt2.Clear()

                dt2.Dispose()
                da2.Dispose()

            End If

            dt1.Clear()
            dt1.Dispose()
            da1.Dispose()

            If LockSTS = True Then
                cbo_DelvAt.Enabled = False
                cbo_DelvAt.BackColor = Color.LightGray

                cbo_PavuRecForm.Enabled = False
                cbo_PavuRecForm.BackColor = Color.LightGray

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
            NoCalc_Status = False
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

    Private Sub JobWork_PavuYarn_Receipt_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated

        Try

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_DelvAt.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "LEDGER" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_DelvAt.Text = Trim(Common_Procedures.Master_Return.Return_Value)
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

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_PavuRecForm.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "LEDGER" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_PavuRecForm.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If
            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_YarnRecForm.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "LEDGER" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_YarnRecForm.Text = Trim(Common_Procedures.Master_Return.Return_Value)
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

    Private Sub JobWork_PavuYarn_Receipt_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
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

        cbo_Cloth.Visible = False
        lbl_Cloth.Visible = False
        If Val(Common_Procedures.settings.Weaver_YarnStock_InMeter_Status) = 1 Or Val(Common_Procedures.settings.CustomerCode) = "1408" Then
            dgv_YarnDetails.Columns(7).HeaderText = "METERS"
            lbl_Cloth.Visible = True
            cbo_Cloth.Visible = True
        End If
        cbo_Grid_RateFor.Items.Clear()
        cbo_Grid_RateFor.Items.Add("")
        cbo_Grid_RateFor.Items.Add("BAG")
        cbo_Grid_RateFor.Items.Add("KG")

        cbo_Verified_Sts.Items.Clear()
        cbo_Verified_Sts.Items.Add("")
        cbo_Verified_Sts.Items.Add("YES")
        cbo_Verified_Sts.Items.Add("NO")

        cbo_RateFor.Visible = True
        cbo_RateFor.Items.Clear()
        cbo_RateFor.Items.Add("METER")
        cbo_RateFor.Items.Add("PAVU")

        cbo_Grid_CountName.Visible = False
        cbo_Grid_MillName.Visible = False
        cbo_Grid_YarnType.Visible = False
        msk_Date.Text = ""
        dtp_Date.Text = ""
        ' txt_KuraiPavuBeam.Text = ""
        cbo_DelvAt.Text = ""
        cbo_DelvAt.Tag = ""
        cbo_EndsCount.Text = ""

        cbo_EndsCount.Text = ""
        cbo_VehicleNo.Text = ""

        cbo_filter_beamNo.Text = ""

        dgv_YarnDetails.Columns(7).Visible = False
        dgv_YarnDetails_Total.Columns(7).Visible = False
        If Val(Common_Procedures.settings.Weaver_YarnStock_InThiri_Status) = 1 Or Val(Common_Procedures.settings.Weaver_YarnStock_InMeter_Status) = 1 Then
            dgv_YarnDetails.Columns(7).Visible = True
            dgv_YarnDetails_Total.Columns(7).Visible = True

        Else

            dgv_YarnDetails.Columns(1).Width = dgv_YarnDetails.Columns(1).Width + 10
            dgv_YarnDetails.Columns(3).Width = dgv_YarnDetails.Columns(3).Width + 50
            dgv_YarnDetails.Columns(6).Width = dgv_YarnDetails.Columns(6).Width + 20

            dgv_YarnDetails_Total.Columns(1).Width = dgv_YarnDetails_Total.Columns(1).Width + 10
            dgv_YarnDetails_Total.Columns(3).Width = dgv_YarnDetails_Total.Columns(3).Width + 50
            dgv_YarnDetails_Total.Columns(6).Width = dgv_YarnDetails_Total.Columns(6).Width + 20

        End If
        btn_UserModification.Visible = False
        If Common_Procedures.settings.User_Modifications_Show_Status = 1 Then
            If Val(Common_Procedures.User.IdNo) = 1 Or Common_Procedures.User.Show_UserModification_Status = 1 Then
                btn_UserModification.Visible = True
            End If
        End If
        chk_Verified_Status.Visible = False
        If Common_Procedures.settings.Vefified_Status = 1 Then
            If Val(Common_Procedures.User.IdNo) = 1 Or Common_Procedures.User.Show_Verified_Status = 1 Then
                chk_Verified_Status.Visible = True
                lbl_verfied_sts.Visible = True
                cbo_Verified_Sts.Visible = True


            End If
        Else
            chk_Verified_Status.Visible = False
            lbl_verfied_sts.Visible = False
            cbo_Verified_Sts.Visible = False
        End If

        cbo_WidthType.Items.Clear()
        cbo_WidthType.Items.Add("")
        cbo_WidthType.Items.Add("SINGLE")
        cbo_WidthType.Items.Add("DOUBLE")
        cbo_WidthType.Items.Add("TRIPLE")
        cbo_WidthType.Items.Add("FOURTH")


        cbo_WidthType.Visible = False
        lbl_Widthtype.Visible = False
        If Common_Procedures.settings.AutoLoom_PavuWidthWiseConsumption_IN_Delivery = 1 Then
            cbo_WidthType.Visible = True
            lbl_Widthtype.Visible = True
        End If

        If Common_Procedures.settings.Show_Sales_OrderNumber_in_ALLEntry_Status = 1 Then

            cbo_ClothSales_OrderCode_forSelection.Visible = True
            lbl_ClothSales_OrderCode_forSelection_Caption.Visible = True

            FnYearCode1 = ""
            FnYearCode2 = ""
            Common_Procedures.get_FnYearCode_of_Last_2_Years(FnYearCode1, FnYearCode2)

        Else

            cbo_ClothSales_OrderCode_forSelection.Visible = False
            lbl_ClothSales_OrderCode_forSelection_Caption.Visible = False

        End If

        AddHandler msk_Date.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Grid_RateFor.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_EndsCount.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_DelvAt.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_VehicleNo.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Grid_MillName.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Grid_CountName.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Grid_YarnType.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_PavuRecForm.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_YarnRecForm.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Transport.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_BeamNoSelection.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_PartyDcNo.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_JJFormNo.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_NoOfBobin.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_TransportMode.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_DateTime_Of_Supply.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_place_Supply.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Rate.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_WidthType.GotFocus, AddressOf ControlGotFocus

        AddHandler txt_KuraiPavuBeam.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Freight.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_KuraiPavuMeters.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Note.GotFocus, AddressOf ControlGotFocus

        AddHandler cbo_Grid_YarnType.GotFocus, AddressOf ControlGotFocus

        AddHandler dtp_Filter_Fromdate.GotFocus, AddressOf ControlGotFocus
        AddHandler dtp_Filter_ToDate.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Filter_MillName.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Filter_EndsCount.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_filter_beamNo.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Filter_CountName.GotFocus, AddressOf ControlGotFocus

        AddHandler btn_save.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_close.GotFocus, AddressOf ControlGotFocus

        AddHandler btn_Print.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_FormJJ_PrintOption.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_Delivery_PrintOption.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_Cancel_PrintOption.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Rate.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_TransportMode.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_DateTime_Of_Supply.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_place_Supply.LostFocus, AddressOf ControlLostFocus

        AddHandler txt_PrintRange_FromNo.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_PrintRange_ToNo.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_Print_PrintRange.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_Cancel_PrintRange.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Cloth.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_RateFor.GotFocus, AddressOf ControlGotFocus

        AddHandler cbo_Cloth.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Grid_RateFor.LostFocus, AddressOf ControlLostFocus
        AddHandler msk_Date.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Grid_CountName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_DelvAt.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_VehicleNo.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Grid_MillName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Grid_YarnType.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_EndsCount.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_KuraiPavuBeam.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Freight.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_KuraiPavuMeters.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Note.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_PavuRecForm.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_YarnRecForm.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Transport.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_BeamNoSelection.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_PartyDcNo.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_JJFormNo.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_NoOfBobin.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_WidthType.LostFocus, AddressOf ControlLostFocus

        AddHandler dtp_Filter_Fromdate.LostFocus, AddressOf ControlLostFocus
        AddHandler dtp_Filter_ToDate.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Filter_CountName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Filter_EndsCount.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_filter_beamNo.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Filter_MillName.LostFocus, AddressOf ControlLostFocus


        AddHandler btn_Print.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_FormJJ_PrintOption.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_Delivery_PrintOption.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_Cancel_PrintOption.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Rate.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_PrintRange_FromNo.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_PrintRange_ToNo.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_Print_PrintRange.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_Cancel_PrintRange.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_RateFor.LostFocus, AddressOf ControlLostFocus

        AddHandler txt_KuraiPavuBeam.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_KuraiPavuMeters.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_DateTime_Of_Supply.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_place_Supply.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler msk_Date.KeyDown, AddressOf TextBoxControlKeyDown
        '   AddHandler txt_PartyDcNo.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_PrintRange_FromNo.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_JJFormNo.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_NoOfBobin.KeyDown, AddressOf TextBoxControlKeyDown


        AddHandler msk_Date.KeyPress, AddressOf TextBoxControlKeyPress

        AddHandler txt_KuraiPavuBeam.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_KuraiPavuMeters.KeyPress, AddressOf TextBoxControlKeyPress
        '   AddHandler txt_PartyDcNo.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_PrintRange_FromNo.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_JJFormNo.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_NoOfBobin.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_DateTime_Of_Supply.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_place_Supply.KeyPress, AddressOf TextBoxControlKeyPress

        AddHandler txt_Eway_Bill_No.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Eway_Bill_No.LostFocus, AddressOf ControlLostFocus

        AddHandler cbo_ClothSales_OrderCode_forSelection.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_ClothSales_OrderCode_forSelection.LostFocus, AddressOf ControlLostFocus

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

    Private Sub PavuYarn_Delivery_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        On Error Resume Next
        con.Close()
        con.Dispose()
        Common_Procedures.Last_Closed_FormName = Me.Name
    End Sub

    Private Sub PavuYarn_Delivery_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress

        Try

            If Asc(e.KeyChar) = 27 Then


                If pnl_Filter.Visible = True Then
                    btn_Filter_Close_Click(sender, e)
                    Exit Sub

                ElseIf pnl_Selection.Visible = True Then
                    btn_Close_Selection_Click(sender, e)
                    Exit Sub

                ElseIf pnl_Print.Visible = True Then
                    btn_Close_PrintOption_Click(sender, e)
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

                    ' LCol = 7
                    ' If dgv_YarnDetails.Columns(7).Visible = False Then LCol = 6

                    If keyData = Keys.Enter Or keyData = Keys.Down Then

                        If .CurrentCell.ColumnIndex >= .ColumnCount - 2 Then
                            If .CurrentCell.RowIndex = .RowCount - 1 Then
                                txt_Note.Focus()

                            Else
                                .CurrentCell = .Rows(.CurrentCell.RowIndex + 1).Cells(1)

                            End If
                        ElseIf .CurrentCell.ColumnIndex = 6 Then
                            If dgv_YarnDetails.Columns(7).Visible = False Then
                                .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(8)
                            Else
                                .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(7)
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
                        ElseIf .CurrentCell.ColumnIndex = 8 Then
                            If dgv_YarnDetails.Columns(7).Visible = False Then
                                .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(6)
                            Else
                                .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(7)
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
        Dim Nr As Integer = 0
        Dim vOrdByNo As String = ""

        vOrdByNo = Common_Procedures.OrderBy_CodeToValue(lbl_DcNo.Text)

        '  If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.PavuYarn_Delivery_Entry, "~L~") = 0 And InStr(Common_Procedures.UR.PavuYarn_Delivery_Entry, "~D~") = 0 Then MessageBox.Show("You have No Rights to Delete", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error) : Exit Sub
        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.DeletingEntry, Common_Procedures.UR.PavuYarn_Delivery_Entry, New_Entry, Me, con, "PavuYarn_Delivery_Head", "PavuYarn_Delivery_Code", NewCode, "PavuYarn_Delivery_Date", "(PavuYarn_Delivery_Code = '" & Trim(NewCode) & "')") = False Then Exit Sub


        If Common_Procedures.settings.Vefified_Status = 1 Then
            NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            If Val(Common_Procedures.get_FieldValue(con, "PavuYarn_Delivery_Head", "Verified_Status", "(PavuYarn_Delivery_Code = '" & Trim(NewCode) & "')")) = 1 Then
                MessageBox.Show("Entry Already Verified", "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                Exit Sub
            End If
        End If

        Qa = MessageBox.Show("Do you want to Delete?", "FOR DELETION...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2)
        If Qa = Windows.Forms.DialogResult.No Or Qa = Windows.Forms.DialogResult.Cancel Then
            Exit Sub
        End If

        If New_Entry = True Then
            MessageBox.Show("This is New Entry", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If


        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Da = New SqlClient.SqlDataAdapter("select * from PavuYarn_Delivery_Head Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and PavuYarn_Delivery_Code = '" & Trim(NewCode) & "'", con)
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

            Call Common_Procedures.User_Modification_Updation(con, "HEAD", Me.Name, "DELETE", "PavuYarn_Delivery_head", "PavuYarn_Delivery_Code", Val(lbl_Company.Tag), NewCode, lbl_DcNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, True, "", "", "PavuYarn_Delivery_Code, Company_IdNo, for_OrderBy", trans)
            Call Common_Procedures.User_Modification_Updation(con, "DETAILS", Me.Name, "DELETE", "Pavu_Delivery_Details", "PavuYarn_Delivery_Code", Val(lbl_Company.Tag), NewCode, lbl_DcNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, True, " Set_No,Beam_No,Pcs,Meters,EndsCount_IdNo,Beam_Width_IdNo,Noof_Used,Set_Code", "Sl_No", "PavuYarn_Delivery_Code, For_OrderBy, Company_IdNo, PavuYarn_Delivery_No, PavuYarn_Delivery_Date, Ledger_Idno", trans)
            Call Common_Procedures.User_Modification_Updation(con, "DETAILS", Me.Name, "DELETE", "Yarn_Delivery_Details", "PavuYarn_Delivery_Code", Val(lbl_Company.Tag), NewCode, lbl_DcNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, True, " count_idno, Yarn_Type, Mill_IdNo,  Bags, Cones, Weight , Thiri,Rate_For,Rate,Amount", "Sl_No", "PavuYarn_Delivery_Code, For_OrderBy, Company_IdNo, PavuYarn_Delivery_No, PavuYarn_Delivery_Date, Ledger_Idno", trans)


            Common_Procedures.Voucher_Deletion(con, Val(lbl_Company.Tag), Trim(Pk_Condition) & Trim(NewCode), trans)
            Common_Procedures.Voucher_Deletion(con, Val(lbl_Company.Tag), Trim(Pk_Condition1) & Trim(NewCode), trans)
            Common_Procedures.Voucher_Deletion(con, Val(lbl_Company.Tag), Trim(Pk_Condition2) & Trim(NewCode), trans)


            cmd.CommandText = "Truncate Table TempTable_For_NegativeStock"
            cmd.ExecuteNonQuery()


            If Val(Common_Procedures.settings.Negative_Stock_Restriction_for_Yarn_Stock) = 1 Then

                cmd.CommandText = "Insert into TempTable_For_NegativeStock ( Stock_Type, Reference_Code, Reference_Date, Company_Idno, Ledger_Idno    , Count_IdNo, Yarn_Type, Mill_IdNo ) " &
                                      " Select                               'YARN'    , Reference_Code, Reference_Date, Company_IdNo, DeliveryTo_Idno, Count_IdNo, Yarn_Type, Mill_IdNo from Stock_Yarn_Processing_Details where Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()

            End If

            If Val(Common_Procedures.settings.Negative_Stock_Restriction_for_Pavu_Stock) = 1 Then

                cmd.CommandText = "Insert into TempTable_For_NegativeStock ( Stock_Type, Reference_Code, Reference_Date, Company_Idno, Ledger_Idno    , EndsCount_IdNo ) " &
                                      " Select                               'PAVU'    , Reference_Code, Reference_Date, Company_IdNo, DeliveryTo_Idno, EndsCount_IdNo from Stock_Pavu_Processing_Details where Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()

            End If


            Da = New SqlClient.SqlDataAdapter("Select * from Pavu_Delivery_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and PavuYarn_Delivery_Code = '" & Trim(NewCode) & "'", con)
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

            cmd.CommandText = "delete from Pavu_Delivery_Details where company_idno = " & Str(Val(lbl_Company.Tag)) & " and PavuYarn_Delivery_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "delete from Yarn_Delivery_Details where company_idno = " & Str(Val(lbl_Company.Tag)) & " and PavuYarn_Delivery_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "delete from PavuYarn_Delivery_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and PavuYarn_Delivery_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            If Val(Common_Procedures.settings.Negative_Stock_Restriction_for_Yarn_Stock) = 1 Or Val(Common_Procedures.settings.Negative_Stock_Restriction_for_Pavu_Stock) = 1 Then
                If Common_Procedures.Check_is_Negative_Stock_Status(con, trans) = True Then Exit Sub
            End If


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
            cbo_filter_beamNo.Text = ""
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

            da = New SqlClient.SqlDataAdapter("select top 1 PavuYarn_Delivery_No from PavuYarn_Delivery_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and PavuYarn_Delivery_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby, PavuYarn_Delivery_No", con)
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

            da = New SqlClient.SqlDataAdapter("select top 1 PavuYarn_Delivery_No from PavuYarn_Delivery_Head where for_orderby > " & Str(Val(OrdByNo)) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and PavuYarn_Delivery_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby, PavuYarn_Delivery_No", con)
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

            da = New SqlClient.SqlDataAdapter("select top 1 PavuYarn_Delivery_No from PavuYarn_Delivery_Head where for_orderby < " & Str(Val(OrdByNo)) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and PavuYarn_Delivery_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, PavuYarn_Delivery_No desc", con)
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
            da = New SqlClient.SqlDataAdapter("select top 1 PavuYarn_Delivery_No from PavuYarn_Delivery_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and PavuYarn_Delivery_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, PavuYarn_Delivery_No desc", con)
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

            lbl_DcNo.Text = Common_Procedures.get_MaxCode(con, "PavuYarn_Delivery_Head", "PavuYarn_Delivery_Code", "For_OrderBy", "", Val(lbl_Company.Tag), Common_Procedures.FnYearCode)

            txt_JJFormNo.Text = Common_Procedures.get_MaxCode(con, "PavuYarn_Delivery_Head", "PavuYarn_Delivery_Code", "JJ_Form_OrderByNo", "", Val(lbl_Company.Tag), Common_Procedures.FnYearCode)

            lbl_DcNo.ForeColor = Color.Red

            msk_Date.Text = Date.Today.ToShortDateString
            da = New SqlClient.SqlDataAdapter("select top 1 * from PavuYarn_Delivery_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and PavuYarn_Delivery_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, PavuYarn_Delivery_No desc", con)
            dt1 = New DataTable
            da.Fill(dt1)
            If dt1.Rows.Count > 0 Then
                If Val(Common_Procedures.settings.PreviousEntryDate_ByDefault) = 1 Or Trim(Common_Procedures.settings.CustomerCode) = "1040" Then '---- M.S Textiles (Tirupur)
                    If dt1.Rows(0).Item("PavuYarn_Delivery_Date").ToString <> "" Then msk_Date.Text = dt1.Rows(0).Item("PavuYarn_Delivery_Date").ToString
                End If

                If IsDBNull(dt1.Rows(0).Item("Rate_for").ToString) = False Then
                    If dt1.Rows(0).Item("Rate_for").ToString <> "" Then cbo_Grid_RateFor.Text = dt1.Rows(0).Item("Rate_for").ToString
                End If

                If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1098" Then
                    If dt1.Rows(0).Item("Vehicle_No").ToString <> "" Then cbo_VehicleNo.Text = dt1.Rows(0).Item("Vehicle_No").ToString
                End If

            End If
            dt1.Clear()
            If msk_Date.Enabled And msk_Date.Visible Then msk_Date.Focus() : msk_Date.SelectionStart = 0

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

            Da = New SqlClient.SqlDataAdapter("select PavuYarn_Delivery_No from PavuYarn_Delivery_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and PavuYarn_Delivery_Code = '" & Trim(RecCode) & "'", con)
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

        'If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.PavuYarn_Delivery_Entry, "~L~") = 0 And InStr(Common_Procedures.UR.PavuYarn_Delivery_Entry, "~I~") = 0 Then MessageBox.Show("You have No Rights to Insert", "DOES NOT INSERT...", MessageBoxButtons.OK, MessageBoxIcon.Error) : Exit Sub
        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.InsertingEntry, Common_Procedures.UR.PavuYarn_Delivery_Entry, New_Entry, Me) = False Then Exit Sub

        Try

            inpno = InputBox("Enter New Dc No.", "FOR NEW RECEIPT INSERTION...")

            RecCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

            Da = New SqlClient.SqlDataAdapter("select PavuYarn_Delivery_No from PavuYarn_Delivery_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and PavuYarn_Delivery_Code = '" & Trim(RecCode) & "'", con)
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
        Dim Sno As Integer = 0, YSno As Integer = 0
        Dim Nr As Integer = 0
        Dim Partcls As String = ""
        Dim PBlNo As String = ""
        Dim vTotPvuBms As Single, vTotPvuMtrs As Single
        Dim YCnt_ID As Integer = 0
        Dim PvuRec_ID As Integer = 0
        Dim YrnRec_ID As Integer = 0
        Dim YMil_ID As Integer = 0
        Dim vTotYrnBags As Single, vTotYrnCones As Single, vTotYrnWeight As Single, vTotYrnThiri As Single, vTotAmt As Single
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
        Dim PvuRec_Ledtype As String = "", YrnRec_Ledtype As String = ""
        Dim Stk_DelvIdNo As Integer, Stk_RecIdNo As Integer
        Dim Prtcls_DelvIdNo As Integer, Prtcls_RecIdNo As Integer, Empty_Bms As Integer
        Dim vWeaFrgt_Partcls As String = ""
        Dim vWtPerBag As Single = 0
        Dim vTotYrnFrght As Single = 0
        Dim vWdTyp As Single = 0
        Dim vOrdByNo As String = 0
        Dim vTotPvuStkAlLoomMtr As Single = 0
        Dim Stk_DelvMtr As Single, Stk_RecMtr As Single
        Dim vENTDB_DelvToIDno As String = 0

        Dim vAmnt As String = 0
        Dim vTxPerc As String = 0
        Dim vCntId As Integer = 0
        Dim vItmGrp_Id As Integer = 0
        Dim vStateCode_Sts As Integer = 0
        Dim vCgstVal As String = 0
        Dim vSgstVal As String = 0
        Dim vIgstVal As String = 0

        vOrdByNo = Common_Procedures.OrderBy_CodeToValue(lbl_DcNo.Text)

        Dim Verified_STS As String = ""
        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)


        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.SavingEntry, Common_Procedures.UR.PavuYarn_Delivery_Entry, New_Entry, Me, con, "PavuYarn_Delivery_Head", "PavuYarn_Delivery_Code", NewCode, "PavuYarn_Delivery_Date", "(PavuYarn_Delivery_Code = '" & Trim(NewCode) & "')", "(Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and PavuYarn_Delivery_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "')", "for_Orderby desc, PavuYarn_Delivery_No desc", dtp_Date.Value.Date) = False Then Exit Sub

        If Val(lbl_Company.Tag) = 0 Then
            MessageBox.Show("Invalid Company Selection", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        ' If Common_Procedures.UserRight_Check(Common_Procedures.UR.PavuYarn_Delivery_Entry, New_Entry) = False Then Exit Sub


        If Common_Procedures.settings.Vefified_Status = 1 Then
            If Not (Val(Common_Procedures.User.IdNo) = 1 Or Common_Procedures.User.Show_Verified_Status = 1) Then
                NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

                If Val(Common_Procedures.get_FieldValue(con, "PavuYarn_Delivery_Head", "Verified_Status", "(PavuYarn_Delivery_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "')")) = 1 Then
                    MessageBox.Show("Entry Already Verified", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                    Exit Sub
                End If
            End If
        End If

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

        Del_ID = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_DelvAt.Text)
        If Del_ID = 0 Then
            MessageBox.Show("Invalid Delivery Name", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If cbo_DelvAt.Enabled And cbo_DelvAt.Visible Then cbo_DelvAt.Focus()
            Exit Sub
        End If

        Clo_ID = Common_Procedures.Cloth_NameToIdNo(con, cbo_Cloth.Text)
        lbl_UserName.Text = Common_Procedures.User.IdNo

        PvuRec_ID = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_PavuRecForm.Text)
        If PvuRec_ID = 0 Then PvuRec_ID = 4

        YrnRec_ID = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_YarnRecForm.Text)
        If YrnRec_ID = 0 Then YrnRec_ID = 4

        Verified_STS = 0
        If chk_Verified_Status.Checked = True Then Verified_STS = 1


        'If cbo_WidthType.Visible And cbo_WidthType.Text = "" Then
        '    MessageBox.Show("Invalid Width Type", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
        '    If cbo_WidthType.Enabled And cbo_WidthType.Visible Then cbo_WidthType.Focus()
        '    Exit Sub
        'End If

        KuPvu_EdsCnt_ID = Common_Procedures.EndsCount_NameToIdNo(con, cbo_EndsCount.Text)
        If KuPvu_EdsCnt_ID = 0 And Val(txt_KuraiPavuMeters.Text) <> 0 Then
            MessageBox.Show("Invalid Ends Count", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If cbo_EndsCount.Enabled And cbo_EndsCount.Visible Then cbo_EndsCount.Focus()
            Exit Sub
        End If

        Trans_ID = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Transport.Text)

        If Trim(txt_PartyDcNo.Text) <> "" Then
            NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)
            Da = New SqlClient.SqlDataAdapter("select * from PavuYarn_Delivery_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and DeliveryTo_IdNo = " & Str(Val(Del_ID)) & " and Party_dcno = '" & Trim(txt_PartyDcNo.Text) & "' and PavuYarn_Delivery_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "' and PavuYarn_Delivery_Code <> '" & Trim(NewCode) & "'", con)
            Dt1 = New DataTable
            Da.Fill(Dt1)
            If Dt1.Rows.Count > 0 Then
                MessageBox.Show("Duplicate Party Dc No to this Party", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                If txt_PartyDcNo.Enabled And txt_PartyDcNo.Visible Then txt_PartyDcNo.Focus()
                Exit Sub
            End If
            Dt1.Clear()
        End If
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


        '--------------
        Dim vCompStCd As String = 0
        Dim vLdStCd As String = 0

        vCompStCd = Common_Procedures.get_FieldValue(con, "Company_Head", "Company_State_Idno", " (Company_idno = " & Val(lbl_Company.Tag) & ") ")
        vLdStCd = Common_Procedures.get_FieldValue(con, "Ledger_Head", "Ledger_State_Idno", " (Ledger_idno = " & Val(Del_ID) & ") ")

        If Val(vCompStCd) = Val(vLdStCd) Then
            vStateCode_Sts = 1
        Else
            vStateCode_Sts = 0
        End If
        '--------------


        NoCalc_Status = False
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

        vTotYrnBags = 0 : vTotYrnCones = 0 : vTotYrnWeight = 0 : vTotYrnThiri = 0 : vTotAmt = 0
        If dgv_YarnDetails_Total.RowCount > 0 Then
            vTotYrnBags = Val(dgv_YarnDetails_Total.Rows(0).Cells(4).Value())
            vTotYrnCones = Val(dgv_YarnDetails_Total.Rows(0).Cells(5).Value())
            vTotYrnWeight = Val(dgv_YarnDetails_Total.Rows(0).Cells(6).Value())
            vTotAmt = Val(dgv_YarnDetails_Total.Rows(0).Cells(10).Value())
            If dgv_YarnDetails_Total.Columns(7).Visible = True Then
                vTotYrnThiri = Val(dgv_YarnDetails_Total.Rows(0).Cells(7).Value())
            End If
        End If

        Total_YarnPavu_Amount_Calculation()

        If Trim(txt_JJFormNo.Text) <> "" Then
            NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)
            Da = New SqlClient.SqlDataAdapter("select * from PavuYarn_Delivery_Head where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and PavuYarn_Delivery_Code <> '" & Trim(NewCode) & "' and PavuYarn_Delivery_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "' and JJ_FormNo = '" & Trim(txt_JJFormNo.Text) & "'", con)
            Dt1 = New DataTable
            Da.Fill(Dt1)
            If Dt1.Rows.Count > 0 Then
                MessageBox.Show("Duplicate JJ Form No", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                If txt_JJFormNo.Enabled And txt_JJFormNo.Visible Then txt_JJFormNo.Focus()
                Exit Sub
            End If
            Dt1.Clear()
        End If

        lbl_Freight_Pavu.Text = Val(Common_Procedures.get_FieldValue(con, "Ledger_Head", "Freight_Pavu", "(Ledger_IdNo = " & Str(Val(Del_ID)) & ")"))

        tr = con.BeginTransaction

        Try

            If Insert_Entry = True Or New_Entry = False Then
                NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            Else

                lbl_DcNo.Text = Common_Procedures.get_MaxCode(con, "PavuYarn_Delivery_Head", "PavuYarn_Delivery_Code", "For_OrderBy", "", Val(lbl_Company.Tag), Common_Procedures.FnYearCode, tr)

                NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            End If

            cmd.Connection = con
            cmd.Transaction = tr

            cmd.Parameters.Clear()
            cmd.Parameters.AddWithValue("@EntryDate", Convert.ToDateTime(msk_Date.Text))


            vOrdByNo = Common_Procedures.OrderBy_CodeToValue(lbl_DcNo.Text)

            cmd.CommandText = "Truncate Table TempTable_For_NegativeStock"
            cmd.ExecuteNonQuery()


            If New_Entry = True Then

                cmd.CommandText = "Insert into PavuYarn_Delivery_Head(PavuYarn_Delivery_Code ,              Company_IdNo        ,       PavuYarn_Delivery_No    ,         for_OrderBy       , PavuYarn_Delivery_Date,        DeliveryTo_Idno   ,                Empty_Beam          ,                   Meters             ,               Party_DcNo           ,           EndsCount_IdNo          ,               Vehicle_No           ,       Transport_Idno ,  ReceivedFrom_Idno    , Yarn_ReceivedFrom_IdNo,             Freight_Charge    ,               Note            ,            Total_Beam        ,            Total_Meters       ,             Total_Bags        ,              Total_Cones       ,            Total_Weight         ,              Total_Thiri       ,                                                 JJ_Form_OrderByNo          ,               JJ_FormNo           ,               Empty_Bobin            ,     Cloth_IdNo     ,              User_idno         ,             Freight_Pavu           ,             Rate           ,             Amount           ,               Transportation_Mode      ,               Date_Time_Of_Supply           ,               Place_Of_Supply         ,          Total_Amount    ,Verified_Status                ,    Width_Type                      ,               Eway_BillNo            ,                EWB_No         ,           Rate_For                        ,   Total_Delivery_Value , ClothSales_OrderCode_forSelection) " &
                                  " Values                            ('" & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_DcNo.Text) & "' , " & Str(Val(vOrdByNo)) & ",           @EntryDate  , " & Str(Val(Del_ID)) & " , " & Val(txt_KuraiPavuBeam.Text) & "," & Val(txt_KuraiPavuMeters.Text) & " , '" & Trim(txt_PartyDcNo.Text) & "' , " & Str(Val(KuPvu_EdsCnt_ID)) & " , '" & Trim(cbo_VehicleNo.Text) & "' , " & Val(Trans_ID) & ", " & Val(PvuRec_ID) & ", " & Val(YrnRec_ID) & ", " & Val(txt_Freight.Text) & " , '" & Trim(txt_Note.Text) & "' , " & Str(Val(vTotPvuBms)) & " , " & Str(Val(vTotPvuMtrs)) & " , " & Str(Val(vTotYrnBags)) & " , " & Str(Val(vTotYrnCones)) & " , " & Str(Val(vTotYrnWeight)) & " , " & Str(Val(vTotYrnThiri)) & " , " & Str(Val(Common_Procedures.OrderBy_CodeToValue(txt_JJFormNo.Text))) & " , '" & Trim(txt_JJFormNo.Text) & "' , " & Str(Val(txt_NoOfBobin.Text)) & " ," & Val(Clo_ID) & " ,  " & Val(lbl_UserName.Text) & ", " & Val(lbl_Freight_Pavu.Text) & " , " & Val(txt_Rate.Text) & " , " & Val(lbl_Amount.Text) & " , '" & Trim(cbo_TransportMode.Text) & "' , '" & Trim(txt_DateTime_Of_Supply.Text) & "' , '" & Trim(txt_place_Supply.Text) & "' , " & Str(Val(vTotAmt)) & " ," & Val(Verified_STS) & " ,'" & Trim(cbo_WidthType.Text) & "' , '" & Trim(txt_Eway_Bill_No.Text) & "' ,'" & Trim(txt_Eway_Bill_No.Text) & "' , '" & Trim(cbo_RateFor.Text) & "'  ,   " & Val(lbl_Total_Value.Text) & " , '" & Trim(cbo_ClothSales_OrderCode_forSelection.Text) & "'     )"
                cmd.ExecuteNonQuery()

            Else

                Call Common_Procedures.User_Modification_Updation(con, "HEAD", Me.Name, "OLD", "PavuYarn_Delivery_head", "PavuYarn_Delivery_Code", Val(lbl_Company.Tag), NewCode, lbl_DcNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, False, "", "", "PavuYarn_Delivery_Code, Company_IdNo, for_OrderBy", tr)
                Call Common_Procedures.User_Modification_Updation(con, "DETAILS", Me.Name, "OLD", "Pavu_Delivery_Details", "PavuYarn_Delivery_Code", Val(lbl_Company.Tag), NewCode, lbl_DcNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, False, " Set_No,Beam_No,Pcs,Meters,EndsCount_IdNo,Beam_Width_IdNo,Noof_Used,Set_Code", "Sl_No", "PavuYarn_Delivery_Code, For_OrderBy, Company_IdNo, PavuYarn_Delivery_No, PavuYarn_Delivery_Date, Ledger_Idno", tr)
                Call Common_Procedures.User_Modification_Updation(con, "DETAILS", Me.Name, "OLD", "Yarn_Delivery_Details", "PavuYarn_Delivery_Code", Val(lbl_Company.Tag), NewCode, lbl_DcNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, False, " count_idno, Yarn_Type, Mill_IdNo,  Bags, Cones, Weight , Thiri,Rate_For,Rate,Amount", "Sl_No", "PavuYarn_Delivery_Code, For_OrderBy, Company_IdNo, PavuYarn_Delivery_No, PavuYarn_Delivery_Date, Ledger_Idno", tr)

                If Val(Common_Procedures.settings.Negative_Stock_Restriction_for_Yarn_Stock) = 1 Then

                    vENTDB_DelvToIDno = Val(Common_Procedures.get_FieldValue(con, "PavuYarn_Delivery_Head", "DeliveryTo_Idno", "(PavuYarn_Delivery_Code = '" & Trim(NewCode) & "')", , tr))

                    If Val(vENTDB_DelvToIDno) <> Val(Del_ID) Then

                        cmd.CommandText = "Insert into TempTable_For_NegativeStock ( Stock_Type, Reference_Code, Reference_Date, Company_Idno, Ledger_Idno    , Count_IdNo, Yarn_Type, Mill_IdNo ) " &
                                            " Select                               'YARN'    , Reference_Code, Reference_Date, Company_IdNo, DeliveryTo_Idno, Count_IdNo, Yarn_Type, Mill_IdNo from Stock_Yarn_Processing_Details where Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
                        cmd.ExecuteNonQuery()

                        cmd.CommandText = "Insert into TempTable_For_NegativeStock ( Stock_Type, Reference_Code, Reference_Date, Company_Idno, Ledger_Idno    , EndsCount_IdNo ) " &
                                            " Select                                 'PAVU'    , Reference_Code, Reference_Date, Company_IdNo, DeliveryTo_Idno, EndsCount_IdNo from Stock_Pavu_Processing_Details where Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
                        cmd.ExecuteNonQuery()

                    End If

                End If

                cmd.CommandText = "Update PavuYarn_Delivery_Head set PavuYarn_Delivery_Date = @EntryDate, DeliveryTo_Idno = " & Str(Val(Del_ID)) & ", Empty_Beam = " & Val(txt_KuraiPavuBeam.Text) & ", Meters = " & Val(txt_KuraiPavuMeters.Text) & " , Party_DcNo = '" & Trim(txt_PartyDcNo.Text) & "' ,  EndsCount_IdNo = " & Str(Val(KuPvu_EdsCnt_ID)) & ",Vehicle_No = '" & Trim(cbo_VehicleNo.Text) & "' , Transport_Idno = " & Val(Trans_ID) & " ,Cloth_IdNo = " & Val(Clo_ID) & ", ReceivedFrom_Idno = " & Val(PvuRec_ID) & ", Yarn_ReceivedFrom_IdNo = " & Val(YrnRec_ID) & ", Freight_Charge = " & Val(txt_Freight.Text) & " , Note = '" & Trim(txt_Note.Text) & "' , Total_Beam = " & Str(Val(vTotPvuBms)) & ", Total_Meters = " & Str(Val(vTotPvuMtrs)) & ", Total_Bags = " & Str(Val(vTotYrnBags)) & ", Total_Cones = " & Str(Val(vTotYrnCones)) & ", Total_Weight = " & Str(Val(vTotYrnWeight)) & " , Total_Thiri = " & Str(Val(vTotYrnThiri)) & ", JJ_Form_OrderByNo = " & Str(Val(Common_Procedures.OrderBy_CodeToValue(txt_JJFormNo.Text))) & ", JJ_FormNo = '" & Trim(txt_JJFormNo.Text) & "' , Empty_Bobin = " & Str(Val(txt_NoOfBobin.Text)) & " , User_idNo =  " & Val(lbl_UserName.Text) & " ,Freight_Pavu  =  " & Val(lbl_Freight_Pavu.Text) & " , Rate =  " & Val(txt_Rate.Text) & " ,Amount =  " & Val(lbl_Amount.Text) & " ,Transportation_Mode = '" & Trim(cbo_TransportMode.Text) & "' ,Date_Time_Of_Supply = '" & Trim(txt_DateTime_Of_Supply.Text) & "' ,Place_Of_Supply = '" & Trim(txt_place_Supply.Text) & "',Total_Amount = " & Str(Val(vTotAmt)) & ",Verified_Status= " & Val(Verified_STS) & " ,Width_Type='" & Trim(cbo_WidthType.Text) & "',Eway_BillNo='" & Trim(txt_Eway_Bill_No.Text) & "',EWB_No='" & Trim(txt_Eway_Bill_No.Text) & "' , Rate_For =  '" & Trim(cbo_RateFor.Text) & "' , Total_Delivery_Value = " & Val(lbl_Total_Value.Text) & "  , ClothSales_OrderCode_forSelection = '" & Trim(cbo_ClothSales_OrderCode_forSelection.Text) & "'  Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & "  and PavuYarn_Delivery_Code = '" & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()

                Da = New SqlClient.SqlDataAdapter("Select * from Pavu_Delivery_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and PavuYarn_Delivery_Code = '" & Trim(NewCode) & "'", con)
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
            Call Common_Procedures.User_Modification_Updation(con, "HEAD", Me.Name, "NEW", "PavuYarn_Delivery_head", "PavuYarn_Delivery_Code", Val(lbl_Company.Tag), NewCode, lbl_DcNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, False, "", "", "PavuYarn_Delivery_Code, Company_IdNo, for_OrderBy", tr)


            cmd.CommandText = "truncate table " & Trim(Common_Procedures.EntryTempTable) & ""
            cmd.ExecuteNonQuery()

            EntID = Trim(Pk_Condition) & Trim(lbl_DcNo.Text)
            If Trim(Common_Procedures.settings.CustomerCode) = "1204" Then
                Partcls = "Delv : Dc.No. " & Trim(lbl_DcNo.Text)
                PBlNo = Trim(lbl_DcNo.Text)
            Else
                If Trim(txt_PartyDcNo.Text) <> "" Then
                    Partcls = "Delv : P.DcNo. " & Trim(txt_PartyDcNo.Text)
                    PBlNo = Trim(txt_PartyDcNo.Text)
                Else
                    Partcls = "Delv : Dc.No. " & Trim(lbl_DcNo.Text)
                    PBlNo = Trim(lbl_DcNo.Text)
                End If
            End If

            Delv_Ledtype = Common_Procedures.get_FieldValue(con, "Ledger_Head", "Ledger_Type", "(Ledger_IdNo = " & Str(Val(Del_ID)) & ")", , tr)
            PvuRec_Ledtype = Common_Procedures.get_FieldValue(con, "Ledger_Head", "Ledger_Type", "(Ledger_IdNo = " & Str(Val(PvuRec_ID)) & ")", , tr)
            YrnRec_Ledtype = Common_Procedures.get_FieldValue(con, "Ledger_Head", "Ledger_Type", "(Ledger_IdNo = " & Str(Val(YrnRec_ID)) & ")", , tr)

            cmd.CommandText = "Delete from Pavu_Delivery_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and PavuYarn_Delivery_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Yarn_Delivery_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and PavuYarn_Delivery_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Stock_Pavu_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Stock_Yarn_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Stock_Empty_BeamBagCone_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            If Val(txt_KuraiPavuMeters.Text) <> 0 And Val(KuPvu_EdsCnt_ID) <> 0 Then
                cmd.CommandText = "insert into " & Trim(Common_Procedures.EntryTempTable) & "(Int1, Int2, Meters1) values (" & Str(Val(KuPvu_EdsCnt_ID)) & ", " & Str(Val(txt_KuraiPavuBeam.Text)) & ", " & Str(Val(txt_KuraiPavuMeters.Text)) & ")"
                cmd.ExecuteNonQuery()
                'cmd.CommandText = "Insert into Stock_Pavu_Processing_Details(Reference_Code, Company_IdNo, Reference_No, for_OrderBy, Reference_Date, DeliveryTo_Idno, ReceivedFrom_Idno, Cloth_Idno, Entry_ID, Party_Bill_No, Particulars, Sl_No, EndsCount_IdNo, Sized_Beam, Meters) Values ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_DcNo.Text) & "', " & Str(Val(vOrdByNo)) & ", @EntryDate, " & Str(Val(Del_ID)) & ", " & Str(Val(PvuRec_ID)) & ", " & Str(Val(Clo_ID)) & ", '" & Trim(EntID) & "', '" & Trim(PBlNo) & "', '" & Trim(Partcls) & "', -100, " & Str(Val(KuPvu_EdsCnt_ID)) & ", " & Str(Val(txt_KuraiPavuBeam.Text)) & ", " & Str(Val(txt_KuraiPavuMeters.Text)) & " )"
                'cmd.ExecuteNonQuery()
            End If


            With dgv_PavuDetails
                Sno = 0
                For i = 0 To dgv_PavuDetails.RowCount - 1

                    If Val(dgv_PavuDetails.Rows(i).Cells(4).Value) <> 0 Then

                        Sno = Sno + 1

                        SzPvu_EdsCnt_ID = Common_Procedures.EndsCount_NameToIdNo(con, .Rows(i).Cells(5).Value, tr)

                        Bw_IdNo = Common_Procedures.BeamWidth_NameToIdNo(con, .Rows(i).Cells(6).Value, tr)

                        ' Pavu_DelvInc = Val(Common_Procedures.get_FieldValue(con, "Stock_SizedPavu_Processing_Details", "Pavu_Delivery_Increment", "(Set_Code = '" & Trim(.Rows(i).Cells(9).Value) & "' and Beam_No = '" & Trim(.Rows(i).Cells(2).Value) & "' )", , tr))

                        vCntId = Common_Procedures.get_FieldValue(con, "Endscount_head", "Count_idno", "(Endscount_idno = " & Val(SzPvu_EdsCnt_ID) & ") ", , tr)

                        vItmGrp_Id = Common_Procedures.get_FieldValue(con, "Count_Head", "ItemGroup_idno", "(Count_idno = " & Val(vCntId) & ") ", , tr)

                        vTxPerc = Common_Procedures.get_FieldValue(con, "itemGroup_Head", "Item_Gst_Percentage", "(ItemGroup_idno = " & Val(vItmGrp_Id) & ") ", , tr)


                        If cbo_RateFor.Text = "METER" Then
                            vAmnt = Format(Val(.Rows(i).Cells(4).Value) * Val(txt_Rate.Text), "############0.00")
                        ElseIf cbo_RateFor.Text = "PAVU" Then
                            vAmnt = Format(1 * Val(txt_Rate.Text), "############0.00")
                        End If


                        If Val(vStateCode_Sts) = 1 Then
                            vCgstVal = Format(((Val(vAmnt) * Val(vTxPerc) / 100) / 2), "############0.00")
                            vSgstVal = vCgstVal
                            vIgstVal = 0
                        Else
                            vIgstVal = Format((Val(vAmnt) * Val(vTxPerc) / 100), "############0.00")
                            vCgstVal = 0
                            vSgstVal = 0
                        End If


                        Ent_NoofUsed = 0
                        If Val(.Rows(i).Cells(8).Value) = 0 Or (Val(.Rows(i).Cells(8).Value) > 0 And Val(.Rows(i).Cells(8).Value) = Val(.Rows(i).Cells(10).Value)) Then

                            Nr = 0
                            cmd.CommandText = "update Stock_SizedPavu_Processing_Details set StockAt_IdNo = " & Str(Val(Del_ID)) & ", Pavu_Delivery_Increment = Pavu_Delivery_Increment + 1 " &
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

                        cmd.CommandText = "Insert into Pavu_Delivery_Details(PavuYarn_Delivery_Code,              Company_IdNo        ,     PavuYarn_Delivery_No     ,           for_OrderBy     , PavuYarn_Delivery_Date,       DeliveryTo_IdNo    ,      ReceivedFrom_IdNo  ,          Sl_No       ,               Set_No                   ,                 Beam_No                ,                    Pcs                    ,                    Meters                ,             EndsCount_IdNo       ,           Beam_Width_IdNo,              Noof_Used        ,                  Set_Code                     ,                 Rate    ,              Amount            ,               Tax_Perc     ,   CGST_AMOUNT     ,            SGST_AMOUNT     ,    IGST_AMOUNT) " &
                                                    " Values  (  '" & Trim(NewCode) & "'           , " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_DcNo.Text) & "', " & Str(Val(vOrdByNo)) & ",        @EntryDate       ,  " & Str(Val(Del_ID)) & ", " & Str(Val(PvuRec_ID)) & ", " & Str(Val(Sno)) & ", '" & Trim(.Rows(i).Cells(1).Value) & "', '" & Trim(.Rows(i).Cells(2).Value) & "', " & Str(Val(.Rows(i).Cells(3).Value)) & " , " & Str(Val(.Rows(i).Cells(4).Value)) & ", " & Str(Val(SzPvu_EdsCnt_ID)) & ", " & Str(Val(Bw_IdNo)) & ", " & Str(Val(Ent_NoofUsed)) & ", '" & Trim(.Rows(i).Cells(9).Value) & "'  , " & Val(txt_Rate.Text) & " ,   " & Str(Val(vAmnt)) & "    ,      " & Val(vTxPerc) & " , " & Val(vCgstVal) & " , " & Val(vSgstVal) & " , " & Val(vIgstVal) & ")"
                        cmd.ExecuteNonQuery()

                        cmd.CommandText = "insert into " & Trim(Common_Procedures.EntryTempTable) & "(Int1, Int2, Meters1, Name6) values (" & Str(Val(SzPvu_EdsCnt_ID)) & ", 1, " & Str(Val(.Rows(i).Cells(4).Value)) & " , '" & Trim(.Rows(i).Cells(2).Value) & "')"
                        'cmd.CommandText = "insert into " & Trim(Common_Procedures.EntryTempTable) & "(Int1, Int2, Meters1) values (" & Str(Val(SzPvu_EdsCnt_ID)) & ", 1, " & Str(Val(.Rows(i).Cells(4).Value)) & ")"
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

                    Stk_DelvMtr = 0 : Stk_RecMtr = 0
                    If Trim(UCase(Stock_In)) = "PCS" Then
                        If Val(mtrspcs) = 0 Then mtrspcs = 1
                        vTotPvuStk = vTotPvuMtrs / mtrspcs

                        Stk_DelvMtr = vTotPvuStk
                        Stk_RecMtr = vTotPvuStk

                    Else
                        ' 
                        If Common_Procedures.settings.AutoLoom_PavuWidthWiseConsumption_IN_Delivery = 1 And cbo_WidthType.Visible = True And Trim(cbo_WidthType.Text) <> "" Then

                            If Trim(UCase(cbo_WidthType.Text)) = "FOURTH" Then
                                vWdTyp = 2
                            ElseIf Trim(UCase(cbo_WidthType.Text)) = "TRIPLE" Then
                                vWdTyp = 1.5
                            ElseIf Trim(UCase(cbo_WidthType.Text)) = "DOUBLE" Then
                                vWdTyp = 1
                            Else
                                vWdTyp = 0.5
                            End If

                            vTotPvuStkAlLoomMtr = vTotPvuMtrs * vWdTyp

                            If Trim(UCase(Delv_Ledtype)) = "WEAVER" Then
                                Stk_DelvMtr = vTotPvuStkAlLoomMtr
                            Else
                                Stk_DelvMtr = vTotPvuMtrs
                            End If

                            If Trim(UCase(PvuRec_Ledtype)) = "WEAVER" Then
                                Stk_RecMtr = vTotPvuStkAlLoomMtr
                            Else
                                Stk_RecMtr = vTotPvuMtrs
                            End If

                        Else

                            vTotPvuStk = vTotPvuMtrs

                            Stk_DelvMtr = vTotPvuStk
                            Stk_RecMtr = vTotPvuStk

                        End If

                    End If


                    Stk_DelvIdNo = 0 : Stk_RecIdNo = 0
                    If Trim(UCase(Delv_Ledtype)) = "JOBWORKER" Then

                        Stk_DelvIdNo = 0
                        Stk_RecIdNo = Del_ID

                        Sno = Sno + 1
                        cmd.CommandText = "Insert into Stock_Pavu_Processing_Details(Reference_Code, Company_IdNo, Reference_No, for_OrderBy, Reference_Date, DeliveryTo_Idno, ReceivedFrom_Idno, Cloth_Idno, Entry_ID, Party_Bill_No, Particulars, Sl_No, EndsCount_IdNo, Sized_Beam, Meters ,DeliveryToIdno_ForParticulars ,ReceivedFromIdno_ForParticulars, ClothSales_OrderCode_forSelection) Values ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_DcNo.Text) & "', " & Str(Val(vOrdByNo)) & ", @EntryDate, " & Str(Val(Stk_DelvIdNo)) & ", " & Str(Val(Stk_RecIdNo)) & ", " & Str(Val(Clo_ID)) & ", '" & Trim(EntID) & "', '" & Trim(PBlNo) & "', '" & Trim(Partcls) & "', " & Str(Val(Sno)) & ", " & Str(Val(Dt1.Rows(i).Item("PavuEndsCount_IdNo").ToString)) & ", " & Str(Val(Dt1.Rows(i).Item("PavuBeam").ToString)) & ", " & Str(Val(vTotPvuStk)) & " ," & Str(Val(Stk_DelvIdNo)) & ", " & Str(Val(Stk_RecIdNo)) & " , '" & Trim(cbo_ClothSales_OrderCode_forSelection.Text) & "')"
                        cmd.ExecuteNonQuery()

                        Stk_DelvIdNo = 0
                        Stk_RecIdNo = PvuRec_ID

                        Sno = Sno + 1
                        cmd.CommandText = "Insert into Stock_Pavu_Processing_Details(Reference_Code, Company_IdNo, Reference_No, for_OrderBy, Reference_Date, DeliveryTo_Idno, ReceivedFrom_Idno, Cloth_Idno, Entry_ID, Party_Bill_No, Particulars, Sl_No, EndsCount_IdNo, Sized_Beam, Meters,DeliveryToIdno_ForParticulars ,ReceivedFromIdno_ForParticulars, ClothSales_OrderCode_forSelection) Values ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_DcNo.Text) & "', " & Str(Val(vOrdByNo)) & ", @EntryDate, " & Str(Val(Stk_DelvIdNo)) & ", " & Str(Val(Stk_RecIdNo)) & ", " & Str(Val(Clo_ID)) & ", '" & Trim(EntID) & "', '" & Trim(PBlNo) & "', '" & Trim(Partcls) & "', " & Str(Val(Sno)) & ", " & Str(Val(Dt1.Rows(i).Item("PavuEndsCount_IdNo").ToString)) & ", " & Str(Val(Dt1.Rows(i).Item("PavuBeam").ToString)) & ", " & Str(Val(vTotPvuStk)) & "," & Str(Val(Stk_DelvIdNo)) & ", " & Str(Val(Stk_RecIdNo)) & " , '" & Trim(cbo_ClothSales_OrderCode_forSelection.Text) & "')"
                        cmd.ExecuteNonQuery()

                    Else

                        Stk_DelvIdNo = Del_ID
                        Stk_RecIdNo = PvuRec_ID

                        If Val(Stk_DelvMtr) = Val(Stk_RecMtr) Then

                            Sno = Sno + 1
                            cmd.CommandText = "Insert into Stock_Pavu_Processing_Details (         Reference_Code                     ,               Company_IdNo       ,             Reference_No     ,              for_OrderBy  , Reference_Date,            DeliveryTo_Idno    ,         ReceivedFrom_Idno    ,          Cloth_Idno     ,          Entry_ID    ,        Party_Bill_No ,        Particulars     ,            Sl_No     ,                         EndsCount_IdNo                           ,                         Sized_Beam                     ,            Meters            ,    DeliveryToIdno_ForParticulars ,  ReceivedFromIdno_ForParticulars , ClothSales_OrderCode_forSelection) " &
                                                "          Values                        ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_DcNo.Text) & "', " & Str(Val(vOrdByNo)) & ",   @EntryDate  , " & Str(Val(Stk_DelvIdNo)) & ", " & Str(Val(Stk_RecIdNo)) & ", " & Str(Val(Clo_ID)) & ", '" & Trim(EntID) & "', '" & Trim(PBlNo) & "', '" & Trim(Partcls) & "', " & Str(Val(Sno)) & ", " & Str(Val(Dt1.Rows(i).Item("PavuEndsCount_IdNo").ToString)) & ", " & Str(Val(Dt1.Rows(i).Item("PavuBeam").ToString)) & ", " & Str(Val(Stk_DelvMtr)) & ", " & Str(Val(Stk_DelvIdNo)) & "   , " & Str(Val(Stk_RecIdNo)) & "    , '" & Trim(cbo_ClothSales_OrderCode_forSelection.Text) & "' )"
                            cmd.ExecuteNonQuery()

                        Else

                            Sno = Sno + 1
                            cmd.CommandText = "Insert into Stock_Pavu_Processing_Details (           Reference_Code                   ,                 Company_IdNo     ,              Reference_No    ,             for_OrderBy   , Reference_Date,         DeliveryTo_Idno       , ReceivedFrom_Idno,            Cloth_Idno   ,          Entry_ID    ,       Party_Bill_No  ,       Particulars      ,            Sl_No     ,                         EndsCount_IdNo                           ,                         Sized_Beam                     ,            Meters           ,   DeliveryToIdno_ForParticulars , ReceivedFromIdno_ForParticulars  , ClothSales_OrderCode_forSelection) " &
                                                "          Values                        ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_DcNo.Text) & "', " & Str(Val(vOrdByNo)) & ",  @EntryDate   , " & Str(Val(Stk_DelvIdNo)) & ",           0      , " & Str(Val(Clo_ID)) & ", '" & Trim(EntID) & "', '" & Trim(PBlNo) & "', '" & Trim(Partcls) & "', " & Str(Val(Sno)) & ", " & Str(Val(Dt1.Rows(i).Item("PavuEndsCount_IdNo").ToString)) & ", " & Str(Val(Dt1.Rows(i).Item("PavuBeam").ToString)) & ", " & Str(Val(Stk_DelvMtr)) & ", " & Str(Val(Stk_DelvIdNo)) & "  ,  " & Str(Val(Stk_RecIdNo)) & "  , '" & Trim(cbo_ClothSales_OrderCode_forSelection.Text) & "' )"
                            cmd.ExecuteNonQuery()

                            Sno = Sno + 1
                            cmd.CommandText = "Insert into Stock_Pavu_Processing_Details (              Reference_Code                ,                   Company_IdNo   ,            Reference_No      ,              for_OrderBy  , Reference_Date, DeliveryTo_Idno,          ReceivedFrom_Idno   ,            Cloth_Idno   ,           Entry_ID   ,        Party_Bill_No ,          Particulars   ,           Sl_No      ,                         EndsCount_IdNo                           ,                         Sized_Beam                     ,              Meters         ,   DeliveryToIdno_ForParticulars, ReceivedFromIdno_ForParticulars ) " &
                                                "           Values                       ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_DcNo.Text) & "', " & Str(Val(vOrdByNo)) & ",   @EntryDate  ,       0        , " & Str(Val(Stk_RecIdNo)) & ", " & Str(Val(Clo_ID)) & ", '" & Trim(EntID) & "', '" & Trim(PBlNo) & "', '" & Trim(Partcls) & "', " & Str(Val(Sno)) & ", " & Str(Val(Dt1.Rows(i).Item("PavuEndsCount_IdNo").ToString)) & ", " & Str(Val(Dt1.Rows(i).Item("PavuBeam").ToString)) & ", " & Str(Val(Stk_RecMtr)) & ", " & Str(Val(Stk_DelvIdNo)) & " , " & Str(Val(Stk_RecIdNo)) & "   ) "
                            cmd.ExecuteNonQuery()

                        End If

                    End If

                    'Sno = Sno + 1
                    'cmd.CommandText = "Insert into Stock_Pavu_Processing_Details(Reference_Code, Company_IdNo, Reference_No, for_OrderBy, Reference_Date, DeliveryTo_Idno, ReceivedFrom_Idno,DeliveryToIdno_ForParticulars,ReceivedFromIdno_ForParticulars, Cloth_Idno, Entry_ID, Party_Bill_No, Particulars, Sl_No, EndsCount_IdNo, Sized_Beam, Meters) Values ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_DcNo.Text) & "', " & Str(Val(vOrdByNo)) & ", @EntryDate, " & Str(Val(Del_ID)) & ", 0," & Str(Val(Del_ID)) & "," & Str(Val(PvuRec_ID)) & ", 0, '" & Trim(EntID) & "', '" & Trim(PBlNo) & "', '" & Trim(Partcls) & "', " & Str(Val(Sno)) & ", " & Str(Val(Dt1.Rows(i).Item("PavuEndsCount_IdNo").ToString)) & ", " & Str(Val(Dt1.Rows(i).Item("PavuBeam").ToString)) & ", " & Str(Val(Stk_DelvMtr)) & " )"
                    'cmd.ExecuteNonQuery()

                    'Sno = Sno + 1
                    'cmd.CommandText = "Insert into Stock_Pavu_Processing_Details(Reference_Code, Company_IdNo, Reference_No, for_OrderBy, Reference_Date, DeliveryTo_Idno, ReceivedFrom_Idno,DeliveryToIdno_ForParticulars,ReceivedFromIdno_ForParticulars, Cloth_Idno, Entry_ID, Party_Bill_No, Particulars, Sl_No, EndsCount_IdNo, Sized_Beam, Meters) Values ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_DcNo.Text) & "', " & Str(Val(vOrdByNo)) & ", @EntryDate, 0, " & Str(Val(PvuRec_ID)) & "," & Str(Val(Del_ID)) & "," & Str(Val(PvuRec_ID)) & ", 0, '" & Trim(EntID) & "', '" & Trim(PBlNo) & "', '" & Trim(Partcls) & "', " & Str(Val(Sno)) & ", " & Str(Val(Dt1.Rows(i).Item("PavuEndsCount_IdNo").ToString)) & ", " & Str(Val(Dt1.Rows(i).Item("PavuBeam").ToString)) & ", " & Str(Val(Stk_RecMtr)) & " )"
                    'cmd.ExecuteNonQuery()

                Next

            End If
            Dt1.Clear()
            Call Common_Procedures.User_Modification_Updation(con, "DETAILS", Me.Name, "NEW", "Pavu_Delivery_Details", "PavuYarn_Delivery_Code", Val(lbl_Company.Tag), NewCode, lbl_DcNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, False, " Set_No,Beam_No,Pcs,Meters,EndsCount_IdNo,Beam_Width_IdNo,Noof_Used,Set_Code", "Sl_No", "PavuYarn_Delivery_Code, For_OrderBy, Company_IdNo, PavuYarn_Delivery_No, PavuYarn_Delivery_Date, Ledger_Idno", tr)


            cmd.CommandText = "Truncate table " & Trim(Common_Procedures.EntryTempSimpleTable) & ""
            cmd.ExecuteNonQuery()


            vTotYrnFrght = 0
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

                        cmd.CommandText = "Insert into Yarn_Delivery_Details(PavuYarn_Delivery_Code, Company_IdNo, PavuYarn_Delivery_No, for_OrderBy, PavuYarn_Delivery_Date, Sl_No, count_idno, Yarn_Type, Mill_IdNo,  Bags, Cones, Weight , Thiri,Rate_For,Rate,Amount ) Values ('" & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_DcNo.Text) & "', " & Str(Val(vOrdByNo)) & ", @EntryDate,  " & Str(Val(Sno)) & ", " & Str(Val(YCnt_ID)) & ", '" & Trim(.Rows(i).Cells(2).Value) & "', " & Str(Val(YMil_ID)) & ", " & Str(Val(.Rows(i).Cells(4).Value)) & ", " & Str(Val(.Rows(i).Cells(5).Value)) & ", " & Str(Val(.Rows(i).Cells(6).Value)) & " ,  " & Str(Val(Thiri_val)) & ",'" & Trim(.Rows(i).Cells(8).Value) & "'," & Str(Val(.Rows(i).Cells(9).Value)) & " ," & Str(Val(.Rows(i).Cells(10).Value)) & ")"
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

                            Stk_DelvIdNo = 0 : Stk_RecIdNo = 0
                            Prtcls_DelvIdNo = 0 : Prtcls_RecIdNo = 0
                            If Trim(UCase(Delv_Ledtype)) = "JOBWORKER" Then
                                Stk_RecIdNo = Del_ID
                                Prtcls_DelvIdNo = YrnRec_ID

                            Else
                                Stk_DelvIdNo = Del_ID
                                Prtcls_RecIdNo = YrnRec_ID

                            End If

                            YSno = YSno + 1
                            cmd.CommandText = "Insert into Stock_Yarn_Processing_Details ( Reference_Code, Company_IdNo, Reference_No, for_OrderBy, Reference_Date, DeliveryTo_Idno, ReceivedFrom_Idno, Entry_ID, Particulars, Party_Bill_No, Sl_No, Count_IdNo, Yarn_Type, Mill_IdNo, Bags, Cones, Weight, DeliveryToIdno_ForParticulars, ReceivedFromIdno_ForParticulars , ClothSales_OrderCode_forSelection) Values ( '" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_DcNo.Text) & "', " & Str(Val(vOrdByNo)) & ", @EntryDate, " & Str(Val(Stk_DelvIdNo)) & ", " & Str(Val(Stk_RecIdNo)) & ", '" & Trim(EntID) & "', '" & Trim(Partcls) & "', '" & Trim(PBlNo) & "', " & Str(Val(YSno)) & ", " & Str(Val(YCnt_ID)) & ", '" & Trim(.Rows(i).Cells(2).Value) & "', " & Str(Val(YMil_ID)) & ", " & Str(Val(.Rows(i).Cells(4).Value)) & ", " & Str(Val(.Rows(i).Cells(5).Value)) & ", " & Str(Val(Stock_Weight)) & ", " & Str(Val(Prtcls_DelvIdNo)) & ", " & Str(Val(Prtcls_RecIdNo)) & " , '" & Trim(cbo_ClothSales_OrderCode_forSelection.Text) & "' )"
                            cmd.ExecuteNonQuery()

                        End If

                        Stock_Weight = Val(.Rows(i).Cells(6).Value)
                        If Trim(UCase(YrnRec_Ledtype)) = "WEAVER" Then
                            If .Columns(7).Visible = True Then
                                Stock_Weight = 0
                                If Val(.Rows(i).Cells(7).Value) <> 0 Then
                                    Stock_Weight = Val(.Rows(i).Cells(7).Value)
                                End If
                            End If
                        End If

                        If Val(Stock_Weight) <> 0 Then

                            Stk_DelvIdNo = 0 : Stk_RecIdNo = 0
                            Prtcls_DelvIdNo = 0 : Prtcls_RecIdNo = 0
                            If Trim(UCase(YrnRec_Ledtype)) = "JOBWORKER" Then
                                Stk_DelvIdNo = YrnRec_ID
                                Prtcls_RecIdNo = Del_ID
                            Else
                                Stk_RecIdNo = YrnRec_ID
                                Prtcls_DelvIdNo = Del_ID
                            End If

                            YSno = YSno + 1
                            cmd.CommandText = "Insert into Stock_Yarn_Processing_Details(Reference_Code, Company_IdNo, Reference_No, for_OrderBy, Reference_Date, DeliveryTo_Idno, ReceivedFrom_Idno, Entry_ID, Particulars, Party_Bill_No, Sl_No, Count_IdNo, Yarn_Type, Mill_IdNo, Bags, Cones, Weight, DeliveryToIdno_ForParticulars, ReceivedFromIdno_ForParticulars , ClothSales_OrderCode_forSelection ) Values ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_DcNo.Text) & "', " & Str(Val(vOrdByNo)) & ", @EntryDate, " & Str(Val(Stk_DelvIdNo)) & ", " & Str(Val(Stk_RecIdNo)) & ", '" & Trim(EntID) & "', '" & Trim(Partcls) & "', '" & Trim(PBlNo) & "', " & Str(Val(YSno)) & ", " & Str(Val(YCnt_ID)) & ", '" & Trim(.Rows(i).Cells(2).Value) & "', " & Str(Val(YMil_ID)) & ", " & Str(Val(.Rows(i).Cells(4).Value)) & ", " & Str(Val(.Rows(i).Cells(5).Value)) & ", " & Str(Val(Stock_Weight)) & ", " & Str(Val(Prtcls_DelvIdNo)) & ", " & Str(Val(Prtcls_RecIdNo)) & " , '" & Trim(cbo_ClothSales_OrderCode_forSelection.Text) & "' )"
                            cmd.ExecuteNonQuery()

                        End If
                        If Val(.Rows(i).Cells(4).Value) <> 0 Then

                            vWtPerBag = Format(Val(.Rows(i).Cells(6).Value) / Val(.Rows(i).Cells(4).Value), "#########0.000")

                            Da = New SqlClient.SqlDataAdapter("select a.* from Ledger_Freight_Charge_Details a where a.ledger_idno = " & Str(Val(Del_ID)) & " and (a.From_Weight <= " & Str(Val(vWtPerBag)) & " and  a.To_Weight >=  " & Str(Val(vWtPerBag)) & ") and a.Freight_Bag <> 0 Order by a.Sl_No", con)
                            Da.SelectCommand.Transaction = tr
                            Dt1 = New DataTable
                            Da.Fill(Dt1)
                            If Dt1.Rows.Count > 0 Then
                                If IsDBNull(Dt1.Rows(0)("Freight_Bag").ToString) = False Then
                                    vTotYrnFrght = Val(vTotYrnFrght) + (Val(.Rows(i).Cells(4).Value) * Val(Dt1.Rows(0)("Freight_Bag").ToString))
                                End If
                            End If
                            Dt1.Clear()

                            cmd.CommandText = "Insert into " & Trim(Common_Procedures.EntryTempSimpleTable) & " (Weight1, Int1) Values (" & Str(Val(vWtPerBag)) & ", " & Str(Val(.Rows(i).Cells(4).Value)) & ")"
                            cmd.ExecuteNonQuery()

                        End If
                    End If


                Next
                Call Common_Procedures.User_Modification_Updation(con, "DETAILS", Me.Name, "NEW", "Yarn_Delivery_Details", "PavuYarn_Delivery_Code", Val(lbl_Company.Tag), NewCode, lbl_DcNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, False, " count_idno, Yarn_Type, Mill_IdNo,  Bags, Cones, Weight , Thiri,Rate_For,Rate,Amount", "Sl_No", "PavuYarn_Delivery_Code, For_OrderBy, Company_IdNo, PavuYarn_Delivery_No, PavuYarn_Delivery_Date, Ledger_Idno", tr)

            End With

            vWeaFrgt_Partcls = Partcls
            Da = New SqlClient.SqlDataAdapter("Select Weight1 as Wgt_Per_Bag, sum(Int1) as NoofBags from " & Trim(Common_Procedures.EntryTempSimpleTable) & " Group by Weight1 Having sum(Int1) <> 0 Order by Weight1", con)
            Da.SelectCommand.Transaction = tr
            Dt1 = New DataTable
            Da.Fill(Dt1)
            If Dt1.Rows.Count > 0 Then
                For i = 0 To Dt1.Rows.Count - 1
                    If IsDBNull(Dt1.Rows(i)("NoofBags").ToString) = False Then
                        vWeaFrgt_Partcls = Trim(vWeaFrgt_Partcls) & IIf(Trim(vWeaFrgt_Partcls) <> "", ", ", "") & Val(Dt1.Rows(i)("Wgt_Per_Bag").ToString) & " Kg - " & Val(Dt1.Rows(i)("NoofBags").ToString) & " Bags"
                    End If
                Next i
            End If
            Dt1.Clear()


            Empty_Bms = 0
            If Common_Procedures.settings.Weaver_Zari_Kuri_Entries_Status = 1 Then

                Empty_Bms = Val(txt_KuraiPavuBeam.Text)
            Else
                Empty_Bms = Val(txt_KuraiPavuBeam.Text) + Val(vTotPvuBms)
            End If

            If Val(Empty_Bms) <> 0 Or Val(txt_NoOfBobin.Text) <> 0 Then
                cmd.CommandText = "Insert into Stock_Empty_BeamBagCone_Processing_Details(Reference_Code, Company_IdNo                     , Reference_No                 , for_OrderBy               , Reference_Date, DeliveryTo_Idno         , ReceivedFrom_Idno          , Party_Bill_No        , Entry_ID             , Sl_No, Beam_Width_IdNo, Empty_Beam, Empty_Bobin                         , Pavu_Beam                   , Yarn_Bags, Yarn_Cones, Particulars            ) " &
                "Values                                    ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_DcNo.Text) & "', " & Str(Val(vOrdByNo)) & ", @EntryDate    , " & Str(Val(Del_ID)) & ", " & Str(Val(PvuRec_ID)) & ", '" & Trim(PBlNo) & "', '" & Trim(EntID) & "', 1    , 0              , 0         , " & Str(Val(txt_NoOfBobin.Text)) & ", " & Str(Val(Empty_Bms)) & " ,  0       , 0         , '" & Trim(Partcls) & "')"
                cmd.ExecuteNonQuery()
            End If
            If Val(vTotYrnBags) <> 0 Or Val(vTotYrnCones) <> 0 Then
                cmd.CommandText = "Insert into Stock_Empty_BeamBagCone_Processing_Details(Reference_Code, Company_IdNo, Reference_No, for_OrderBy, Reference_Date, DeliveryTo_Idno, ReceivedFrom_Idno, Party_Bill_No, Entry_ID, Sl_No, Beam_Width_IdNo, Empty_Beam, Pavu_Beam, Yarn_Bags, Yarn_Cones, Particulars) Values ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_DcNo.Text) & "', " & Str(Val(vOrdByNo)) & ", @EntryDate, " & Str(Val(Del_ID)) & ", " & Str(Val(YrnRec_ID)) & ", '" & Trim(PBlNo) & "', '" & Trim(EntID) & "', 2, 0, 0, 0, " & Str(Val(vTotYrnBags)) & ", " & Str(Val(vTotYrnCones)) & ", '" & Trim(Partcls) & "')"
                cmd.ExecuteNonQuery()
            End If

            Dim vLed_IdNos As String = "", vVou_Amts As String = "", ErrMsg As String = ""
            Dim YarnFrg_pavu As Single = 0, Pavu_FrgPavu As Single = 0, Pavu_Bems As Single = 0

            Pavu_Bems = Val(vTotPvuBms) + Val(txt_KuraiPavuBeam.Text)

            Pavu_FrgPavu = Format(Val(lbl_Freight_Pavu.Text) * Val(Pavu_Bems), "##########0.00")
            vLed_IdNos = Trans_ID & "|" & Val(Common_Procedures.CommonLedger.Transport_Charges_Ac)
            vVou_Amts = Val(txt_Freight.Text) & "|" & -1 * Val(txt_Freight.Text)
            If Common_Procedures.Voucher_Updation(con, "PY.Delv", Val(lbl_Company.Tag), Trim(Pk_Condition) & Trim(NewCode), Trim(lbl_DcNo.Text), Convert.ToDateTime(msk_Date.Text), Partcls, vLed_IdNos, vVou_Amts, ErrMsg, tr, Common_Procedures.SoftwareTypes.Textile_Software) = False Then
                Throw New ApplicationException(ErrMsg)
            End If

            vLed_IdNos = Del_ID & "|" & Val(Common_Procedures.CommonLedger.Freight_Charges_Ac)
            vVou_Amts = Format(-1 * Val(Pavu_FrgPavu), "#########0.00") & "|" & Format(Val(Pavu_FrgPavu), "###########0.00")


            If Common_Procedures.Voucher_Updation(con, "PY.Delv.PavuFrg", Val(lbl_Company.Tag), Trim(Pk_Condition1) & Trim(NewCode), Trim(lbl_DcNo.Text), Convert.ToDateTime(msk_Date.Text), Partcls & ", Beams : " & Pavu_Bems, vLed_IdNos, vVou_Amts, ErrMsg, tr, Common_Procedures.SoftwareTypes.Textile_Software) = False Then
                Throw New ApplicationException(ErrMsg)
            End If

            vLed_IdNos = Del_ID & "|" & Val(Common_Procedures.CommonLedger.Freight_Charges_Ac)
            vVou_Amts = Format(-1 * Val(vTotYrnFrght), "#########0.00") & "|" & Format(Val(vTotYrnFrght), "###########0.00")
            If Common_Procedures.Voucher_Updation(con, "PY.Delv.YarnFrg", Val(lbl_Company.Tag), Trim(Pk_Condition2) & Trim(NewCode), Trim(lbl_DcNo.Text), Convert.ToDateTime(msk_Date.Text), vWeaFrgt_Partcls, vLed_IdNos, vVou_Amts, ErrMsg, tr, Common_Procedures.SoftwareTypes.Textile_Software) = False Then
                Throw New ApplicationException(ErrMsg)
            End If

            If Val(Common_Procedures.settings.Negative_Stock_Restriction_for_Yarn_Stock) = 1 Or Val(Common_Procedures.settings.Negative_Stock_Restriction_for_Pavu_Stock) = 1 Then

                If Val(Common_Procedures.settings.Negative_Stock_Restriction_for_Yarn_Stock) = 1 Then
                    cmd.CommandText = "Insert into TempTable_For_NegativeStock ( Stock_Type, Reference_Code, Reference_Date, Company_Idno, Ledger_Idno      , Count_IdNo, Yarn_Type, Mill_IdNo ) " &
                      " Select                               'YARN'    , Reference_Code, Reference_Date, Company_IdNo, ReceivedFrom_Idno, Count_IdNo, Yarn_Type, Mill_IdNo from Stock_Yarn_Processing_Details where Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
                    cmd.ExecuteNonQuery()
                End If

                If Val(Common_Procedures.settings.Negative_Stock_Restriction_for_Pavu_Stock) = 1 Then
                    cmd.CommandText = "Insert into TempTable_For_NegativeStock ( Stock_Type, Reference_Code, Reference_Date, Company_Idno, Ledger_Idno      , EndsCount_IdNo ) " &
                      " Select                               'PAVU'    , Reference_Code, Reference_Date, Company_IdNo, ReceivedFrom_Idno, EndsCount_IdNo from Stock_Pavu_Processing_Details where Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
                    cmd.ExecuteNonQuery()
                End If

                If Common_Procedures.Check_is_Negative_Stock_Status(con, tr) = True Then Exit Sub

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



            If SaveAll_Sts <> True Then
                MessageBox.Show("Saved Successfully!!!", "FOR SAVING...", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If



            'If Val(Common_Procedures.settings.OnSave_MoveTo_NewEntry_Status) = 1 Then
            '    If New_Entry = True Then
            '        new_record()
            '    Else
            '        move_record(lbl_DcNo.Text)
            '    End If
            'Else
            '    move_record(lbl_DcNo.Text)
            'End If


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

    Private Sub cbo_DelvAt_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_DelvAt.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "( ( Ledger_Type = 'SIZING' or Ledger_Type = 'WEAVER' or Ledger_Type = 'JOBWORKER' or Ledger_Type = 'REWINDING' or (Ledger_Type = '' and Stock_Maintenance_Status = 1) or Show_In_All_Entry = 1 ) and Close_status = 0 )", "(Ledger_IdNo = 0)")
        'If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1040" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1032" Then '---- M.S Textiles (Tirupur)
        '    Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "( ( Ledger_Type = 'SIZING' or Ledger_Type = 'WEAVER' or Ledger_Type = 'JOBWORKER' or Ledger_Type = 'REWINDING' or (Ledger_Type = '' and Stock_Maintenance_Status = 1) ) or Show_In_All_Entry = 1 )", "(Ledger_IdNo = 0)")
        'Else
        '    Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "", "(Ledger_IdNo = 0)")
        'End If
    End Sub

    Private Sub cbo_Ledger_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_DelvAt.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_DelvAt, msk_Date, cbo_PavuRecForm, "Ledger_AlaisHead", "Ledger_DisplayName", "( ( Ledger_Type = 'SIZING' or Ledger_Type = 'WEAVER' or Ledger_Type = 'JOBWORKER' or Ledger_Type = 'REWINDING' or (Ledger_Type = '' and Stock_Maintenance_Status = 1) or Show_In_All_Entry = 1 ) and Close_status = 0 )", "(Ledger_IdNo = 0)")
        'If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1040" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1032" Then '---- M.S Textiles (Tirupur)
        '    Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_DelvAt, msk_Date, cbo_PavuRecForm, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'SIZING' or Ledger_Type = '' or Ledger_Type = 'WEAVER' or Ledger_Type = 'JOBWORKER'or Ledger_Type = 'REWINDING'  or (Ledger_Type = '' and Stock_Maintenance_Status = 1) ) or Show_In_All_Entry =1 ", "(Ledger_IdNo = 0)")
        'Else
        '    Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_DelvAt, msk_Date, cbo_PavuRecForm, "Ledger_AlaisHead", "Ledger_DisplayName", "", "(Ledger_IdNo = 0)")
        'End If
    End Sub

    Private Sub cbo_Ledger_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_DelvAt.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_DelvAt, cbo_PavuRecForm, "Ledger_AlaisHead", "Ledger_DisplayName", "( ( Ledger_Type = 'SIZING' or Ledger_Type = 'WEAVER' or Ledger_Type = 'JOBWORKER' or Ledger_Type = 'REWINDING' or (Ledger_Type = '' and Stock_Maintenance_Status = 1) or Show_In_All_Entry = 1 ) and Close_status = 0 )", "(Ledger_IdNo = 0)")
        'If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1040" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1032" Then '---- M.S Textiles (Tirupur)
        '    Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_DelvAt, cbo_PavuRecForm, "Ledger_AlaisHead", "Ledger_DisplayName", "( ( Ledger_Type = 'SIZING' or Ledger_Type = 'WEAVER' or Ledger_Type = 'JOBWORKER'or Ledger_Type = 'REWINDING' or (Ledger_Type = '' and Stock_Maintenance_Status = 1)  ) or Show_In_All_Entry = 1 )", "(Ledger_IdNo = 0)")
        'Else
        '    Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_DelvAt, cbo_PavuRecForm, "Ledger_AlaisHead", "Ledger_DisplayName", "", "(Ledger_IdNo = 0)")
        'End If
    End Sub

    Private Sub cbo_Ledger_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_DelvAt.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Common_Procedures.MDI_LedType = "WEAVER"
            Dim f As New Ledger_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_DelvAt.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()
        End If
    End Sub

    Private Sub cbo_VehicleNo_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_VehicleNo.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "PavuYarn_Delivery_Head", "Vehicle_No", "", "(Vehicle_No <> '')")
    End Sub

    Private Sub cbo_VehicleNo_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_VehicleNo.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_VehicleNo, cbo_Transport, txt_Freight, "PavuYarn_Delivery_Head", "Vehicle_No", "", "(Vehicle_No <> '')")
    End Sub

    Private Sub cbo_VehicleNo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_VehicleNo.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_VehicleNo, txt_Freight, "PavuYarn_Delivery_Head", "Vehicle_No", "", "(Vehicle_No <> '')", False)
    End Sub

    Private Sub cbo_EndsCount_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_EndsCount.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "EndsCount_Head", "EndsCount_Name", "", "(EndsCount_IdNo = 0)")
    End Sub

    Private Sub cbo_EndsCount_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_EndsCount.KeyDown

        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_EndsCount, txt_Eway_Bill_No, txt_KuraiPavuMeters, "EndsCount_Head", "EndsCount_Name", "", "(EndsCount_IdNo = 0)")
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
                Condt = "a.PavuYarn_Delivery_Date between '" & Trim(Format(dtp_Filter_Fromdate.Value, "MM/dd/yyyy")) & "' and '" & Trim(Format(dtp_Filter_ToDate.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_Filter_Fromdate.Value) = True Then
                Condt = "a.PavuYarn_Delivery_Date = '" & Trim(Format(dtp_Filter_Fromdate.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_Filter_ToDate.Value) = True Then
                Condt = "a.PavuYarn_Delivery_Date = '" & Trim(Format(dtp_Filter_ToDate.Value, "MM/dd/yyyy")) & "' "
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
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " a.PavuYarn_Delivery_Code IN (select z1.PavuYarn_Delivery_Code from Yarn_Delivery_Details z1 where z1.Count_IdNo = " & Str(Val(Cnt_IdNo)) & ")"
            End If

            If Val(Mil_IdNo) <> 0 Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " a.PavuYarn_Delivery_Code IN (select z1.PavuYarn_Delivery_Code from Yarn_Delivery_Details z1 where z1.Mill_IdNo = " & Str(Val(Mil_IdNo)) & ")"
            End If

            If Val(EdsCnt_IdNo) <> 0 Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " a.EndsCount_IdNo = " & Str(Val(EdsCnt_IdNo))
            End If

            If cbo_Verified_Sts.Visible = True And Trim(cbo_Verified_Sts.Text) <> "" Then

                If Trim(cbo_Verified_Sts.Text) = "YES" Then
                    Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " a.Verified_Status = 1 "
                ElseIf Trim(cbo_Verified_Sts.Text) = "NO" Then
                    Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " a.Verified_Status = 0 "
                End If

            End If


            If Trim(cbo_filter_beamNo.Text) <> "" Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " a.PavuYarn_Delivery_Code IN (select z1.PavuYarn_Delivery_Code from Pavu_Delivery_Details z1 where z1.Beam_No = '" & Trim(cbo_filter_beamNo.Text) & "' )"
            End If


            da = New SqlClient.SqlDataAdapter("select a.*, b.Ledger_Name as Deliv_Name from PavuYarn_Delivery_Head a INNER JOIN Ledger_Head b on a.DeliveryTo_Idno = b.Ledger_IdNo where a.company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.PavuYarn_Delivery_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "' " & IIf(Trim(Condt) <> "", " and ", "") & Condt & " Order by a.for_orderby, a.PavuYarn_Delivery_No", con)
            da.Fill(dt2)

            dgv_Filter_Details.Rows.Clear()

            If dt2.Rows.Count > 0 Then

                For i = 0 To dt2.Rows.Count - 1

                    n = dgv_Filter_Details.Rows.Add()

                    dgv_Filter_Details.Rows(n).Cells(0).Value = i + 1
                    dgv_Filter_Details.Rows(n).Cells(1).Value = dt2.Rows(i).Item("PavuYarn_Delivery_No").ToString
                    dgv_Filter_Details.Rows(n).Cells(2).Value = Format(Convert.ToDateTime(dt2.Rows(i).Item("PavuYarn_Delivery_Date").ToString), "dd-MM-yyyy")
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
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "( Ledger_Type = 'GODOWN' or Ledger_Type = 'SIZING' or Ledger_Type = 'WEAVER' or Ledger_Type = 'JOBWORKER' or Ledger_Type = 'REWINDING' or (Ledger_Type = '' and Stock_Maintenance_Status = 1) or Show_In_All_Entry = 1 ) ", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_Filter_PartyName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Filter_DeliveryName.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_DeliveryName, dtp_Filter_ToDate, cbo_Filter_CountName, "Ledger_AlaisHead", "Ledger_DisplayName", "( Ledger_Type = 'GODOWN' or Ledger_Type = 'SIZING' or Ledger_Type = 'WEAVER' or Ledger_Type = 'JOBWORKER' or Ledger_Type = 'REWINDING' or (Ledger_Type = '' and Stock_Maintenance_Status = 1) or Show_In_All_Entry = 1 ) ", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_Filter_PartyName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Filter_DeliveryName.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Filter_DeliveryName, cbo_Filter_CountName, "Ledger_AlaisHead", "Ledger_DisplayName", "( Ledger_Type = 'GODOWN' or Ledger_Type = 'SIZING' or Ledger_Type = 'WEAVER' or Ledger_Type = 'JOBWORKER' or Ledger_Type = 'REWINDING' or (Ledger_Type = '' and Stock_Maintenance_Status = 1) or Show_In_All_Entry = 1 ) ", "(Ledger_IdNo = 0)")
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
                If IsNothing(.CurrentCell) Then Exit Sub
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
                    cbo_PavuRecForm.Focus()
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
        If Not IsNothing(dgv_PavuDetails.CurrentCell) Then
            If IsNothing(dgv_PavuDetails.CurrentCell) Then Exit Sub
            With dgv_PavuDetails
                n = .RowCount
                .Rows(n - 1).Cells(0).Value = Val(n)
            End With
        End If
    End Sub

    Private Sub TotalPavu_Calculation()
        Dim Sno As Integer
        Dim TotBms As Single, TotMtrs As Single
        Dim Bems As Single = 0
        Dim Mtrs As Single = 0
        If NoCalc_Status = True Then Exit Sub
        ' If FrmLdSTS = True Or NoCalc_Status = True Then Exit Sub
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
        'If Common_Procedures.settings.CustomerCode = "1204" Then
        '    Bems = Val(txt_KuraiPavuBeam.Text) + Val(TotBms)
        '    lbl_Amount.Text = Format(Val(Bems) * Val(txt_Rate.Text), "############0.00")
        'Else
        '    Mtrs = Val(txt_KuraiPavuMeters.Text) + Val(TotMtrs)
        '    lbl_Amount.Text = Format(Val(Mtrs) * Val(txt_Rate.Text), "############0.00")
        'End If

        If Val(TotMtrs) = 0 Then
            TotMtrs = Format(Val(txt_KuraiPavuMeters.Text), "########0.00")
        End If
        If Val(TotBms) = 0 Then
            TotBms = Val(txt_KuraiPavuBeam.Text)
        End If


        If cbo_RateFor.Text = "METER" Then
            lbl_Amount.Text = Format(Val(TotMtrs) * Val(txt_Rate.Text), "############0.00")
        ElseIf cbo_RateFor.Text = "PAVU" Then
            lbl_Amount.Text = Format(Val(TotBms) * Val(txt_Rate.Text), "############0.00")
        End If

        Total_YarnPavu_Amount_Calculation()


    End Sub

    Private Sub dgv_YarnDetails_CellEndEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_YarnDetails.CellEndEdit
        Try

            If dgv_YarnDetails.CurrentRow.Cells(2).Value = "MILL" Then
                If dgv_YarnDetails.CurrentCell.ColumnIndex = 4 Or dgv_YarnDetails.CurrentCell.ColumnIndex = 5 Then
                    get_MillCount_Details()
                End If
            End If
            If e.ColumnIndex = 6 Then
                get_Bag_Details()
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
                If e.ColumnIndex = 8 Then

                    If cbo_Grid_RateFor.Visible = False Or Val(cbo_Grid_RateFor.Tag) <> e.RowIndex Then

                        rect = .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False)

                        cbo_Grid_RateFor.Left = .Left + rect.Left
                        cbo_Grid_RateFor.Top = .Top + rect.Top

                        cbo_Grid_RateFor.Width = rect.Width
                        cbo_Grid_RateFor.Height = rect.Height
                        cbo_Grid_RateFor.Text = .CurrentCell.Value

                        cbo_Grid_RateFor.Tag = Val(e.RowIndex)
                        cbo_Grid_RateFor.Visible = True

                        cbo_Grid_RateFor.BringToFront()
                        cbo_Grid_RateFor.Focus()

                    End If

                Else
                    cbo_Grid_RateFor.Visible = False

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

        If IsNothing(dgv_YarnDetails.CurrentCell) Then Exit Sub
        With dgv_YarnDetails
            If .Visible Then
                If e.ColumnIndex = 1 Or e.ColumnIndex = 6 Then
                    If .Columns(7).Visible = True Then
                        If Val(Common_Procedures.settings.Weaver_YarnStock_InThiri_Status = 1) Then
                            Thiri_Calculation()
                        ElseIf Val(Common_Procedures.settings.Weaver_YarnStock_InMeter_Status = 1) Then
                            Meter_Calculation()
                        End If
                    End If
                End If
                If e.ColumnIndex = 4 Or e.ColumnIndex = 5 Or e.ColumnIndex = 6 Or e.ColumnIndex = 7 Then
                    TotalYarnTaken_Calculation()
                End If
                If .CurrentCell.ColumnIndex = 4 Or .CurrentCell.ColumnIndex = 6 Or .CurrentCell.ColumnIndex = 9 Then
                    Amount_Calculation()

                End If

                If .CurrentCell.ColumnIndex = 8 Or .CurrentCell.ColumnIndex = 9 Or .CurrentCell.ColumnIndex = 10 Then
                    Total_YarnPavu_Amount_Calculation()
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
        If Not IsNothing(dgv_YarnDetails.CurrentCell) Then dgv_YarnDetails.CurrentCell.Selected = False
    End Sub

    Private Sub dgv_YarnDetails_RowsAdded(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewRowsAddedEventArgs) Handles dgv_YarnDetails.RowsAdded
        Dim n As Integer

        If IsNothing(dgv_YarnDetails.CurrentCell) Then Exit Sub
        With dgv_YarnDetails
            n = .RowCount
            .Rows(n - 1).Cells(0).Value = Val(n)
            .Rows(n - 1).Cells(2).Value = "MILL"
        End With
    End Sub

    Private Sub Meter_Calculation()
        Dim q As Single = 0
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim Weft_Cons As Single
        Dim dt3 As New DataTable
        Dim dt4 As New DataTable
        Dim Clo_IdNo As Integer = 0
        Dim edscnt_id As Integer = 0
        Dim cnt_id As Integer = 0
        Dim mtrs As Single = 0

        Clo_IdNo = Common_Procedures.Cloth_NameToIdNo(con, cbo_Cloth.Text)
        Weft_Cons = 0

        Da = New SqlClient.SqlDataAdapter("select * from Cloth_Head where Cloth_idno = " & Str(Val(Clo_IdNo)), con)
        Da.Fill(Dt)
        With dgv_YarnDetails

            If Dt.Rows.Count > 0 Then
                If IsDBNull(Dt.Rows(0)(0).ToString) = False Then
                    Weft_Cons = Dt.Rows(0).Item("Weight_Meter_Weft").ToString
                End If
            End If

            Dt.Clear()
            Dt.Dispose()
            Da.Dispose()

            If Val(.Rows(.CurrentRow.Index).Cells(6).Value) <> 0 Then
                .Rows(.CurrentRow.Index).Cells(7).Value = Format(Val(.Rows(.CurrentRow.Index).Cells(6).Value) / Weft_Cons, "##########0.00")
            Else
                .Rows(.CurrentRow.Index).Cells(7).Value = Val(.Rows(.CurrentRow.Index).Cells(6).Value)
            End If

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
        Dim TotBags As Single, TotCones As Single, TotWeight As Single, TotThiri As Single, TotAmt As Single

        Sno = 0
        TotBags = 0
        TotCones = 0
        TotWeight = 0
        TotThiri = 0
        TotAmt = 0
        With dgv_YarnDetails
            For i = 0 To .RowCount - 1
                Sno = Sno + 1
                .Rows(i).Cells(0).Value = Sno
                If Val(.Rows(i).Cells(6).Value) <> 0 Then
                    TotBags = TotBags + Val(.Rows(i).Cells(4).Value)
                    TotCones = TotCones + Val(.Rows(i).Cells(5).Value)
                    TotWeight = TotWeight + Val(.Rows(i).Cells(6).Value)
                    TotAmt = TotAmt + Val(.Rows(i).Cells(10).Value)
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
            .Rows(0).Cells(10).Value = Format(Val(TotAmt), "########0.000")
            If .Columns(7).Visible = True Then
                .Rows(0).Cells(7).Value = Format(Val(TotThiri), "########0.000")
            End If

        End With
        '
    End Sub

    Private Sub cbo_Grid_CountName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Grid_CountName.GotFocus
        Try
            Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Count_Head", "Count_Name", "", "(Count_IdNo = 0)")

        Catch ex As Exception

        End Try

    End Sub

    Private Sub cbo_Grid_CountName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Grid_CountName.KeyDown
        Try

            Dim dep_idno As Integer = 0

            Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Grid_CountName, Nothing, cbo_Grid_YarnType, "Count_Head", "Count_Name", "", "(Count_IdNo = 0)")

            With dgv_YarnDetails

                If (e.KeyValue = 38 And cbo_Grid_CountName.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then

                    If Val(.CurrentCell.RowIndex) <= 0 Then

                        If cbo_ClothSales_OrderCode_forSelection.Visible = True Then
                            cbo_ClothSales_OrderCode_forSelection.Focus()
                        ElseIf cbo_WidthType.Visible = True Then
                            cbo_WidthType.Focus()
                        Else
                            txt_Freight.Focus()
                        End If


                    Else


                        .Focus()
                        .CurrentCell = .Rows(.CurrentCell.RowIndex - 1).Cells(6)
                        .CurrentCell.Selected = True


                    End If
                End If

                If (e.KeyValue = 40 And cbo_Grid_CountName.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then

                    If .CurrentCell.RowIndex = .RowCount - 1 And .CurrentCell.ColumnIndex >= 1 And Trim(.CurrentRow.Cells(1).Value) = "" Then


                        cbo_TransportMode.Focus()

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


                        cbo_TransportMode.Focus()
                    Else
                        .Focus()
                        .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)

                    End If
                End With

            End If

        Catch ex As Exception

        End Try
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
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_EndsCount, cbo_Filter_MillName, Nothing, "EndsCount_Head", "EndsCount_Name", "", "(EndsCount_IdNo = 0)")
        If e.KeyValue = 40 Or (e.Control = True And e.KeyValue = 40) Then
            If Common_Procedures.settings.CustomerCode = "1249" Then
                cbo_Verified_Sts.Focus()


            ElseIf cbo_Filter_BeamNo.Visible = True Then
                cbo_filter_beamNo.Focus()

            Else
                btn_Filter_Show.Focus()

            End If

        End If
    End Sub

    Private Sub cbo_Filter_EndsCount_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Filter_EndsCount.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Filter_EndsCount, Nothing, "EndsCount_Head", "EndsCount_Name", "", "(EndsCount_IdNo = 0)")
        If Asc(e.KeyChar) = 13 Then
            If Common_Procedures.settings.CustomerCode = "1249" Then
                cbo_Verified_Sts.Focus()
            ElseIf cbo_Filter_BeamNo.Visible = True Then
                cbo_filter_beamNo.Focus()
            Else
                btn_Filter_Show.Focus()

            End If
        End If
    End Sub

    Private Sub cbo_Verified_Sts_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Verified_Sts.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "", "", "", "")

    End Sub
    Private Sub cbo_Verified_Sts_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Verified_Sts.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Verified_Sts, cbo_Filter_EndsCount, Nothing, "", "", "", "")



        If e.KeyCode = 40 Then
            If cbo_filter_beamNo.Visible = True Then
                cbo_filter_beamNo.Focus()
            Else
                btn_Filter_Show.Focus()
            End If
        End If

    End Sub

    Private Sub cbo_Verified_Sts_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Verified_Sts.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Verified_Sts, Nothing, "", "", "", "")



        If Asc(e.KeyChar) = 13 Then
            If cbo_filter_beamNo.Visible = True Then
                cbo_filter_beamNo.Focus()
            Else
                btn_Filter_Show.Focus()
            End If
        End If


    End Sub


    Public Sub print_record() Implements Interface_MDIActions.print_record

        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.PrintEntry, Common_Procedures.UR.PavuYarn_Delivery_Entry, New_Entry) = False Then Exit Sub

        prn_FromNo = Trim(lbl_DcNo.Text)
        prn_ToNo = Trim(lbl_DcNo.Text)

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

            pnl_Back.Enabled = True
            pnl_Print.Visible = False


            'btn_Close_PrintOption_Click(sender, e)

        End If

        'pnl_Print.Visible = True
        'pnl_Back.Enabled = False
        'If btn_Delivery_PrintOption.Enabled And btn_Delivery_PrintOption.Visible Then
        '    btn_Delivery_PrintOption.Focus()
        'End If
    End Sub


    Private Sub Printing_Delivery()
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim NewCode As String
        'Dim ps As Printing.PaperSize
        Dim OrdBy_FrmNo As Single = 0, OrdByToNo As Single = 0
        Dim PpSzSTS As Boolean = False

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Try

            OrdBy_FrmNo = Common_Procedures.OrderBy_CodeToValue(prn_FromNo)
            OrdByToNo = Common_Procedures.OrderBy_CodeToValue(prn_ToNo)

            da1 = New SqlClient.SqlDataAdapter("select * from PavuYarn_Delivery_Head Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and for_orderby between " & Str(Format(Val(OrdBy_FrmNo), "########0.00")) & " and " & Str(Format(Val(OrdByToNo), "########0.00")) & " and PavuYarn_Delivery_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "'", con)
            'da1 = New SqlClient.SqlDataAdapter("select * from PavuYarn_Delivery_Head Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and PavuYarn_Delivery_Code = '" & Trim(NewCode) & "'", con)
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

        set_PaperSize_For_PrintDocument1()

        If Val(Common_Procedures.Print_OR_Preview_Status) = 1 Then

            Try
                If Val(Common_Procedures.settings.Printing_Show_PrintDialogue) = 1 Then
                    PrintDialog1.PrinterSettings = PrintDocument1.PrinterSettings
                    If PrintDialog1.ShowDialog() = Windows.Forms.DialogResult.OK Then
                        PrintDocument1.PrinterSettings = PrintDialog1.PrinterSettings

                        set_PaperSize_For_PrintDocument1()

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

                ppd.WindowState = FormWindowState.Maximized
                ppd.StartPosition = FormStartPosition.CenterScreen
                'ppd.ClientSize = New Size(600, 600)
                ppd.PrintPreviewControl.AutoZoom = True
                ppd.PrintPreviewControl.Zoom = 1.0
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
        prn_PageSize_SetUP_STS = False
        prn_PageNo = 0
        prn_DetMxIndx = 0
        prn_NoofBmDets = 0
        prn_Count = 0

        vprn_Tot_Bgs_Bms = 0
        vprn_Tot_Wgt_Mtr = 0
        vprn_Tot_Amt = 0

        Erase prn_HdAr
        Erase prn_DetAr

        prn_HdAr = New String(200, 10) {}
        prn_DetAr = New String(200, 10) {}

        Try

            OrdBy_FrmNo = Common_Procedures.OrderBy_CodeToValue(prn_FromNo)
            OrdByToNo = Common_Procedures.OrderBy_CodeToValue(prn_ToNo)

            da1 = New SqlClient.SqlDataAdapter("select a.*, b.*, c.Ledger_MainName as Del_Name,c.Tamil_Name as DelTamil_Name, c.Ledger_Tamil_Address1, c.Ledger_Tamil_Address2, c.Ledger_Address1, c.Ledger_Address2, c.Ledger_Address3, c.Ledger_Address4, c.Ledger_PhoneNo, c.Ledger_GSTinNo, c.Pan_No, c.Aadhar_No, c.Ledger_TinNo, d.Ledger_Name as Transport_Name, e.Area_Name, f.EndsCount_Name, g.Ledger_Name as Pavu_RecFrom_Name, h.Ledger_Name as Yarn_RecFrom_Name ,Csh.State_Name as Company_State_Name, Csh.State_Code as Company_State_Code, Lsh.State_Name as Ledger_State_Name, Lsh.State_Code as Ledger_State_Code  from PavuYarn_Delivery_Head a INNER JOIN Company_Head b ON a.Company_IdNo = b.Company_IdNo LEFT OUTER JOIN State_Head Csh ON b.Company_State_IdNo = Csh.State_IdNo INNER JOIN Ledger_Head c ON a.DeliveryTo_Idno = c.Ledger_IdNo LEFT OUTER JOIN State_Head Lsh ON c.Ledger_State_IdNo = Lsh.State_IdNo  LEFT OUTER JOIN Ledger_Head d ON a.Transport_Idno = d.Ledger_IdNo  LEFT OUTER JOIN Area_Head e ON b.Area_Idno = e.Area_Idno LEFT OUTER JOIN EndsCount_Head f ON a.EndsCount_Idno = f.EndsCount_Idno LEFT OUTER JOIN Ledger_Head g ON a.ReceivedFrom_Idno = g.Ledger_IdNo LEFT OUTER JOIN Ledger_Head h ON a.Yarn_ReceivedFrom_Idno = h.Ledger_IdNo where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.for_orderby between " & Str(Format(Val(OrdBy_FrmNo), "########0.00")) & " and " & Str(Format(Val(OrdByToNo), "########0.00")) & " and a.PavuYarn_Delivery_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by a.for_orderby, a.PavuYarn_Delivery_No", con)
            'da1 = New SqlClient.SqlDataAdapter("select a.*, b.*, cl.* ,c.Ledger_MainName as Del_Name,c.Tamil_Name as DelTamil_Name, c.Ledger_Tamil_Address1, c.Ledger_Tamil_Address2, c.Ledger_Address1, c.Ledger_Address2, c.Ledger_Address3, c.Ledger_Address4, c.Ledger_PhoneNo, c.Ledger_GSTinNo, c.Pan_No, c.Aadhar_No, c.Ledger_TinNo, d.Ledger_Name as Transport_Name, e.Area_Name, f.EndsCount_Name, g.Ledger_Name as Pavu_RecFrom_Name, h.Ledger_Name as Yarn_RecFrom_Name ,Csh.State_Name as Company_State_Name, Csh.State_Code as Company_State_Code, Lsh.State_Name as Ledger_State_Name, Lsh.State_Code as Ledger_State_Code  from PavuYarn_Delivery_Head a INNER JOIN Company_Head b ON a.Company_IdNo = b.Company_IdNo LEFT OUTER JOIN State_Head Csh ON b.Company_State_IdNo = Csh.State_IdNo INNER JOIN Ledger_Head c ON a.DeliveryTo_Idno = c.Ledger_IdNo LEFT OUTER JOIN State_Head Lsh ON c.Ledger_State_IdNo = Lsh.State_IdNo  LEFT OUTER JOIN Ledger_Head d ON a.Transport_Idno = d.Ledger_IdNo  LEFT OUTER JOIN Area_Head e ON b.Area_Idno = e.Area_Idno LEFT OUTER JOIN EndsCount_Head f ON a.EndsCount_Idno = f.EndsCount_Idno LEFT OUTER JOIN Ledger_Head g ON a.ReceivedFrom_Idno = g.Ledger_IdNo LEFT OUTER JOIN Ledger_Head h ON a.Yarn_ReceivedFrom_Idno = h.Ledger_IdNo LEFT OUTER JOIN cloth_head cl ON cl.EndsCount_Idno = f.EndsCount_Idno where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.for_orderby between " & Str(Format(Val(OrdBy_FrmNo), "########0.00")) & " and " & Str(Format(Val(OrdByToNo), "########0.00")) & " and a.PavuYarn_Delivery_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by a.for_orderby, a.PavuYarn_Delivery_No", con)
            prn_HdDt = New DataTable
            da1.Fill(prn_HdDt)

            If prn_HdDt.Rows.Count > 0 Then

                'Debug.Print(prn_HdDt.Rows(0).Item("Pan_No").ToString)

                da2 = New SqlClient.SqlDataAdapter("select a.*, b.Mill_Name, c.*,d.* from Yarn_Delivery_Details a LEFT OUTER JOIN Mill_Head b on a.Mill_IdNo = b.Mill_IdNo INNER JOIN Count_Head c on a.Count_IdNo = c.Count_IdNo LEFT OUTER JOIN itemgroup_head d ON c.itemgroup_idno = d.itemgroup_idno where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.PavuYarn_Delivery_Code = '" & Trim(NewCode) & "' Order by a.sl_no", con)
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

                da1 = New SqlClient.SqlDataAdapter("select a.*, b.* from Pavu_Delivery_Details a INNER JOIN EndsCount_Head b ON a.EndsCount_IdNo = b.EndsCount_IdNo where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.PavuYarn_Delivery_Code = '" & Trim(NewCode) & "' Order by a.sl_no", con)
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

                da1 = New SqlClient.SqlDataAdapter("select a.*, b.* from Pavu_Delivery_Details a INNER JOIN EndsCount_Head b ON a.EndsCount_IdNo = b.EndsCount_IdNo where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.PavuYarn_Delivery_Code = '" & Trim(NewCode) & "' Order by a.sl_no", con)
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

                da1 = New SqlClient.SqlDataAdapter("select top 1 a.Yarn_Type, c.Count_Name, d.Mill_Name, b.Total_Bags, b.Total_Cones, b.Total_Weight, b.Total_Thiri from Yarn_Delivery_Details a INNER JOIN PavuYarn_Delivery_Head b ON a.PavuYarn_Delivery_Code = b.PavuYarn_Delivery_Code INNER JOIN Count_Head c on a.Count_IdNo = c.Count_IdNo LEFT OUTER JOIN Mill_Head d on a.Mill_IdNo = d.Mill_IdNo where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.PavuYarn_Delivery_Code = '" & Trim(NewCode) & "' Order by a.sl_no", con)
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

            NewCode = prn_HdDt.Rows(prn_HeadIndx).Item("PavuYarn_Delivery_Code").ToString

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

            da1 = New SqlClient.SqlDataAdapter("select a.*, b.*, c.*, d.Ledger_Name as Transport_Name, e.Area_Name, f.EndsCount_Name, g.Ledger_Name as Pavu_RecFrom_Name, h.Ledger_Name as Yarn_RecFrom_Name ,Csh.State_Name as Company_State_Name, Csh.State_Code as Company_State_Code, Lsh.State_Name as Ledger_State_Name, Lsh.State_Code as Ledger_State_Code  from PavuYarn_Delivery_Head a INNER JOIN Company_Head b ON a.Company_IdNo = b.Company_IdNo LEFT OUTER JOIN State_Head Csh ON b.Company_State_IdNo = Csh.State_IdNo INNER JOIN Ledger_Head c ON a.DeliveryTo_Idno = c.Ledger_IdNo LEFT OUTER JOIN State_Head Lsh ON c.Ledger_State_IdNo = Lsh.State_IdNo  LEFT OUTER JOIN Ledger_Head d ON a.Transport_Idno = d.Ledger_IdNo  LEFT OUTER JOIN Area_Head e ON b.Area_Idno = e.Area_Idno LEFT OUTER JOIN EndsCount_Head f ON a.EndsCount_Idno = f.EndsCount_Idno LEFT OUTER JOIN Ledger_Head g ON a.ReceivedFrom_Idno = g.Ledger_IdNo LEFT OUTER JOIN Ledger_Head h ON a.Yarn_ReceivedFrom_Idno = h.Ledger_IdNo where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & "  and a.PavuYarn_Delivery_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by a.for_orderby, a.PavuYarn_Delivery_No", con)
            Dt1 = New DataTable
            da1.Fill(Dt1)

            If Dt1.Rows.Count > 0 Then

                da2 = New SqlClient.SqlDataAdapter("select a.*, b.Mill_Name, c.*,d.* from Yarn_Delivery_Details a LEFT OUTER JOIN Mill_Head b on a.Mill_IdNo = b.Mill_IdNo INNER JOIN Count_Head c on a.Count_IdNo = c.Count_IdNo LEFT OUTER JOIN itemgroup_head d ON c.itemgroup_idno = d.itemgroup_idno where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.PavuYarn_Delivery_Code = '" & Trim(NewCode) & "' Order by a.sl_no", con)
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

                da1 = New SqlClient.SqlDataAdapter("select a.*, b.* from Pavu_Delivery_Details a INNER JOIN EndsCount_Head b ON a.EndsCount_IdNo = b.EndsCount_IdNo where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.PavuYarn_Delivery_Code = '" & Trim(NewCode) & "' Order by a.sl_no", con)
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

                da1 = New SqlClient.SqlDataAdapter("select a.*, b.* from Pavu_Delivery_Details a INNER JOIN EndsCount_Head b ON a.EndsCount_IdNo = b.EndsCount_IdNo where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.PavuYarn_Delivery_Code = '" & Trim(NewCode) & "' Order by a.sl_no", con)
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
                            prn_DetAr(prn_DetMxIndx, 5) = Trim(prn_DetDt1.Rows(i).Item("Set_No").ToString)

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

                da1 = New SqlClient.SqlDataAdapter("select top 1 a.Yarn_Type, c.Count_Name, d.Mill_Name, b.Total_Bags, b.Total_Cones, b.Total_Weight, b.Total_Thiri from Yarn_Delivery_Details a INNER JOIN PavuYarn_Delivery_Head b ON a.PavuYarn_Delivery_Code = b.PavuYarn_Delivery_Code INNER JOIN Count_Head c on a.Count_IdNo = c.Count_IdNo LEFT OUTER JOIN Mill_Head d on a.Mill_IdNo = d.Mill_IdNo where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.PavuYarn_Delivery_Code = '" & Trim(NewCode) & "' Order by a.sl_no", con)
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

        If dgv_YarnDetails.Columns(7).Visible = True Then
            Printing_Format2(e)
        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1242" Then
            Printing_Format2Gst(e)
        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1204" Then

            Printing_Format_1204(e)
        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1408" Then
            Printing_Format_1408(e)

        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1420" Then
            Printing_Format1420(e)

        Else
            Printing_Format3(e)
            'Printing_Format1(e)
        End If

    End Sub

    Private Sub btn_Cancel_PrintOption_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Cancel_PrintOption.Click
        btn_Close_PrintOption_Click(sender, e)
    End Sub

    Private Sub btn_Close_PrintOption_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Close_PrintOption.Click
        pnl_Back.Enabled = True
        pnl_Print.Visible = False
    End Sub

    Private Sub btn_Delivery_PrintOption_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Delivery_PrintOption.Click
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
            btn_Close_PrintOption_Click(sender, e)

        End If

    End Sub

    Private Sub btn_FormJJ_PrintOption_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_FormJJ_PrintOption.Click
        prn_Status = 2
        Printing_FormJJ()
        btn_Close_PrintOption_Click(sender, e)
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
        'Dim ps As Printing.PaperSize
        Dim strHeight As Single = 0
        Dim PpSzSTS As Boolean = False
        Dim W1 As Single = 0


        If prn_PageSize_SetUP_STS = False Then
            set_PaperSize_For_PrintDocument1()
            prn_PageSize_SetUP_STS = True
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

        da2 = New SqlClient.SqlDataAdapter("select a.*, b.Mill_Name, c.Count_Name  from Yarn_Delivery_Details a INNER JOIN Mill_Head b on a.Mill_IdNo = b.Mill_IdNo LEFT OUTER JOIN Count_Head c on a.Count_IdNo = c.Count_IdNo where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.PavuYarn_Delivery_Code = '" & Trim(EntryCode) & "' Order by a.sl_no", con)
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
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(prn_HeadIndx).Item("PavuYarn_Delivery_No").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, p1Font)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(prn_HeadIndx).Item("Ledger_Address1").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
            p1Font = New Font("Calibri", 14, FontStyle.Bold)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(prn_HeadIndx).Item("Ledger_Address2").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "DATE", LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(prn_HeadIndx).Item("PavuYarn_Delivery_Date").ToString), "dd-MM-yyyy").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, pFont)

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
        'Dim ps As Printing.PaperSize
        Dim strHeight As Single = 0
        Dim PpSzSTS As Boolean = False
        Dim W1 As Single = 0

        If prn_PageSize_SetUP_STS = False Then
            set_PaperSize_For_PrintDocument1()
            prn_PageSize_SetUP_STS = True
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


                W1 = e.Graphics.MeasureString("Ends Count : ", pFont).Width

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
        Dim Cmp_PhNo As String, Cmp_TinNo As String, Cmp_CstNo As String, Cmp_GSTNo As String
        Dim Entry_Date As Date = Convert.ToDateTime(prn_HdDt.Rows(0).Item("PavuYarn_Delivery_Date").ToString)

        PageNo = PageNo + 1

        CurY = TMargin

        da2 = New SqlClient.SqlDataAdapter("select a.*, b.Mill_Name, c.Count_Name  from Yarn_Delivery_Details a INNER JOIN Mill_Head b on a.Mill_IdNo = b.Mill_IdNo LEFT OUTER JOIN Count_Head c on a.Count_IdNo = c.Count_IdNo where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.PavuYarn_Delivery_Code = '" & Trim(EntryCode) & "' Order by a.sl_no", con)
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
        CurY = CurY + TxtHgt - 1
        If Entry_Date >= Common_Procedures.GST_Start_Date Then
            Common_Procedures.Print_To_PrintDocument(e, Cmp_GSTNo, LMargin + 10, CurY, 0, 0, pFont)
        Else
            Common_Procedures.Print_To_PrintDocument(e, Cmp_TinNo, LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Cmp_CstNo, PageWidth - 10, CurY, 1, 0, pFont)
        End If
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
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("PavuYarn_Delivery_No").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, p1Font)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address1").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
            p1Font = New Font("Calibri", 14, FontStyle.Bold)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address2").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "DATE", LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("PavuYarn_Delivery_Date").ToString), "dd-MM-yyyy").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address3").ToString & " " & prn_HdDt.Rows(0).Item("Ledger_Address4").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            If Entry_Date >= Common_Procedures.GST_Start_Date Then
                If prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString <> "" Then
                    Common_Procedures.Print_To_PrintDocument(e, "GSTIN_NO  : " & prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
                End If
            Else
                If prn_HdDt.Rows(0).Item("Ledger_TinNo").ToString <> "" Then
                    Common_Procedures.Print_To_PrintDocument(e, "TIN NO : " & prn_HdDt.Rows(0).Item("Ledger_TinNo").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
                End If
            End If
            'Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address4").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
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
        'Dim ps As Printing.PaperSize
        Dim strHeight As Single = 0
        Dim PpSzSTS As Boolean = False
        Dim W1 As Single = 0
        Dim Inc As Integer = 0

        If prn_PageSize_SetUP_STS = False Then
            set_PaperSize_For_PrintDocument1()
            prn_PageSize_SetUP_STS = True
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
        Dim Cmp_PhNo As String, Cmp_TinNo As String, Cmp_CstNo As String, Cmp_GSTNo As String
        Dim vPrn_DcNo As String = ""
        Dim Entry_Date As Date = Convert.ToDateTime(prn_HdDt.Rows(prn_HeadIndx).Item("PavuYarn_Delivery_Date").ToString)
        Dim strWidth As Single = 0
        Dim CurX As Single = 0
        PageNo = PageNo + 1

        CurY = TMargin

        da2 = New SqlClient.SqlDataAdapter("select a.*, b.Mill_Name, c.Count_Name  from Yarn_Delivery_Details a INNER JOIN Mill_Head b on a.Mill_IdNo = b.Mill_IdNo LEFT OUTER JOIN Count_Head c on a.Count_IdNo = c.Count_IdNo where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.PavuYarn_Delivery_Code = '" & Trim(EntryCode) & "' Order by a.sl_no", con)
        da2.Fill(dt2)

        If dt2.Rows.Count > NoofItems_PerPage Then
            Common_Procedures.Print_To_PrintDocument(e, "Page : " & Trim(Val(PageNo)), PageWidth - 10, CurY - TxtHgt, 1, 0, pFont)
        End If
        dt2.Clear()

        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(1) = CurY

        Cmp_Name = "" : Cmp_Add1 = "" : Cmp_Add2 = ""
        Cmp_PhNo = "" : Cmp_TinNo = "" : Cmp_CstNo = "" : Cmp_GSTNo = ""

        Cmp_Name = prn_HdDt.Rows(prn_HeadIndx).Item("Company_Name").ToString
        Cmp_Add1 = prn_HdDt.Rows(prn_HeadIndx).Item("Company_Address1").ToString & " " & prn_HdDt.Rows(prn_HeadIndx).Item("Company_Address2").ToString
        Cmp_Add2 = prn_HdDt.Rows(prn_HeadIndx).Item("Company_Address3").ToString & " " & prn_HdDt.Rows(prn_HeadIndx).Item("Company_Address4").ToString
        If Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Company_PhoneNo").ToString) <> "" Then
            Cmp_PhNo = "PHONE NO.:" & prn_HdDt.Rows(prn_HeadIndx).Item("Company_PhoneNo").ToString
        End If

        If Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Company_TinNo").ToString) <> "" Then
            Cmp_TinNo = "TIN NO.: " & prn_HdDt.Rows(prn_HeadIndx).Item("Company_TinNo").ToString
        End If
        If Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Company_panNo").ToString) <> "" Then
            Cmp_CstNo = "PAN NO.: " & prn_HdDt.Rows(prn_HeadIndx).Item("Company_panNo").ToString
        End If
        If Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Company_GSTinNo").ToString) <> "" Then
            Cmp_GSTNo = "GSTIN : " & prn_HdDt.Rows(prn_HeadIndx).Item("Company_GSTinNo").ToString
        End If
        CurY = CurY + TxtHgt - 10
        p1Font = New Font("Calibri", 18, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Name, LMargin, CurY, 2, PrintWidth, p1Font)
        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height

        If Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Company_logo_Image").ToString) <> "" Then

            If IsDBNull(prn_HdDt.Rows(prn_HeadIndx).Item("Company_logo_Image")) = False Then

                Dim imageData As Byte() = DirectCast(prn_HdDt.Rows(prn_HeadIndx).Item("Company_logo_Image"), Byte())
                If Not imageData Is Nothing Then
                    Using ms As New MemoryStream(imageData, 0, imageData.Length)
                        ms.Write(imageData, 0, imageData.Length)

                        If imageData.Length > 0 Then

                            '.BackgroundImage = Image.FromStream(ms)

                            ' e.Graphics.DrawImage(DirectCast(pic_IRN_QRCode_Image_forPrinting.BackgroundImage, Drawing.Image), PageWidth - 108, CurY + 10, 90, 90)
                            e.Graphics.DrawImage(DirectCast(Image.FromStream(ms), Drawing.Image), LMargin + 10, CurY, 110, 90)

                        End If

                    End Using

                End If

            End If

        End If

        CurY = CurY + strHeight - 1
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Add1, LMargin, CurY, 2, PrintWidth, pFont)

        CurY = CurY + TxtHgt - 1
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Add2, LMargin, CurY, 2, PrintWidth, pFont)
        CurY = CurY + TxtHgt - 1
        Common_Procedures.Print_To_PrintDocument(e, Cmp_PhNo, LMargin, CurY, 2, PrintWidth, pFont)
        CurY = CurY + TxtHgt - 1
        If Entry_Date >= Common_Procedures.GST_Start_Date Then
            Common_Procedures.Print_To_PrintDocument(e, Cmp_GSTNo & "   /   " & Cmp_CstNo, LMargin + 10, CurY, 0, 0, pFont)
        Else
            Common_Procedures.Print_To_PrintDocument(e, Cmp_TinNo, LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Cmp_CstNo, PageWidth - 10, CurY, 1, 0, pFont)
        End If
        CurY = CurY + TxtHgt - 10
        p1Font = New Font("Calibri", 16, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "DELIVERY", LMargin, CurY, 2, PrintWidth, p1Font)
        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height

        'CurY = CurY + TxtHgt

        CurY = CurY + strHeight + 5 ' + 150
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(2) = CurY

        Try

            M1 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6)
            W1 = e.Graphics.MeasureString("FORM JJ NO    : ", pFont).Width
            N1 = e.Graphics.MeasureString("TO   :", pFont).Width

            CurY = CurY + TxtHgt - 10
            p1Font = New Font("Calibri", 10, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "TO  :  " & "M/s." & prn_HdDt.Rows(prn_HeadIndx).Item("Del_Name").ToString, LMargin + 10, CurY, 0, 0, p1Font)
            Common_Procedures.Print_To_PrintDocument(e, "DC.NO", LMargin + M1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + M1 + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(prn_HeadIndx).Item("PavuYarn_Delivery_No").ToString, LMargin + M1 + W1 + 30, CurY, 0, 0, p1Font)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(prn_HeadIndx).Item("Ledger_Address1").ToString, LMargin + N1 + 10, CurY, 0, 0, pFont)
            p1Font = New Font("Calibri", 14, FontStyle.Bold)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(prn_HeadIndx).Item("Ledger_Address2").ToString, LMargin + N1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "DATE", LMargin + M1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + M1 + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(prn_HeadIndx).Item("PavuYarn_Delivery_Date").ToString), "dd-MM-yyyy").ToString, LMargin + M1 + W1 + 30, CurY, 0, 0, pFont)




            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(prn_HeadIndx).Item("Ledger_Address3").ToString & " " & prn_HdDt.Rows(prn_HeadIndx).Item("Ledger_Address4").ToString, LMargin + N1 + 10, CurY, 0, 0, pFont)
            If Trim(vPrn_PvuSetNo) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, "SET NO", LMargin + M1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + M1 + W1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, Trim(vPrn_PvuSetNo), LMargin + M1 + W1 + 30, CurY, 0, 0, pFont)
            End If

            CurY = CurY + TxtHgt

            If prn_HdDt.Rows(prn_HeadIndx).Item("Ledger_GSTinNo").ToString <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, "GSTIN  : " & prn_HdDt.Rows(prn_HeadIndx).Item("Ledger_GSTinNo").ToString, LMargin + N1 + 10, CurY, 0, 0, pFont)
            ElseIf Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Pan_No").ToString) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, "PAN  : " & prn_HdDt.Rows(prn_HeadIndx).Item("Pan_No").ToString, LMargin + N1 + 10, CurY, 0, 0, pFont)
                'strWidth = e.Graphics.MeasureString("GSTIN : " & prn_HdDt.Rows(prn_HeadIndx).Item("Ledger_GSTinNo").ToString, p1Font).Width
                'CurX = LMargin + N1 + 10 + strWidth
                'Common_Procedures.Print_To_PrintDocument(e, "      PAN : " & prn_HdDt.Rows(prn_HeadIndx).Item("Pan_No").ToString, CurX, CurY, 0, PrintWidth, p1Font)
            End If





            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1040" Then '---- M.S Textiles (Tirupur)

                vPrn_DcNo = ""

                OrdByNo = Common_Procedures.OrderBy_CodeToValue(Trim(prn_HdDt.Rows(prn_HeadIndx).Item("PavuYarn_Delivery_No").ToString))
                Da = New SqlClient.SqlDataAdapter("select top 1 PavuYarn_Delivery_No from PavuYarn_Delivery_Head where DeliveryTo_Idno = " & Str(Val(prn_HdDt.Rows(prn_HeadIndx).Item("DeliveryTo_Idno").ToString)) & " and for_orderby < " & Str(Format(Val(OrdByNo), "######0.00")) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and PavuYarn_Delivery_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, PavuYarn_Delivery_No desc", con)
                Dt1 = New DataTable
                Da.Fill(Dt1)
                If Dt1.Rows.Count > 0 Then
                    vPrn_DcNo = Dt1.Rows(0).Item("PavuYarn_Delivery_No").ToString
                End If
                Dt1.Clear()
                If Trim(vPrn_DcNo) <> "" Then
                    Common_Procedures.Print_To_PrintDocument(e, "Prev Dc.No", LMargin + M1 + 10, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + M1 + W1 + 10, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, Trim(vPrn_DcNo), LMargin + M1 + W1 + 30, CurY, 0, 0, pFont)
                End If

            ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1065" Then '---- Logu Tex (Palladam)
                If Trim(prn_HdDt.Rows(prn_HeadIndx).Item("JJ_FormNo").ToString) <> "" Then
                    Common_Procedures.Print_To_PrintDocument(e, "FORM JJ NO", LMargin + M1 + 10, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + M1 + W1 + 10, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(prn_HeadIndx).Item("JJ_FormNo").ToString), LMargin + M1 + W1 + 30, CurY, 0, 0, pFont)
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
            If Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Vehicle_No").ToString) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, "Vehicle No. : " & Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Vehicle_No").ToString), PageWidth - 200, CurY, 1, 0, pFont)
            End If


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
            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1037" Then '---- Prakash Textiles (Somanur)
                'CurY = CurY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, "Note : " & prn_HdDt.Rows(prn_HeadIndx).Item("Note").ToString, PageWidth - 5, CurY, 1, 0, pFont)
            End If

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

            da1 = New SqlClient.SqlDataAdapter("select * from PavuYarn_Delivery_Head Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and PavuYarn_Delivery_Code = '" & Trim(NewCode) & "'", con)
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
                ppd.ClientSize = New Size(900, 800)

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

            da1 = New SqlClient.SqlDataAdapter("select a.*, b.*, c.Ledger_TinNo , c.Ledger_CstNo, c.Ledger_Name as Del_Name, c.Ledger_Address1, c.Ledger_Address2, c.Ledger_Address3, c.Ledger_Address4, d.Ledger_Name as Transport_Name, e.Area_Name, f.EndsCount_Name, g.Ledger_Name as Pavu_RecFrom_Name, h.Ledger_Name as Yarn_RecFrom_Name from PavuYarn_Delivery_Head a INNER JOIN Company_Head b ON a.Company_IdNo = b.Company_IdNo INNER JOIN Ledger_Head c ON a.DeliveryTo_Idno = c.Ledger_IdNo LEFT OUTER JOIN Ledger_Head d ON a.Transport_Idno = d.Ledger_IdNo  LEFT OUTER JOIN Area_Head e ON b.Area_Idno = e.Area_Idno LEFT OUTER JOIN EndsCount_Head f ON a.EndsCount_Idno = f.EndsCount_Idno LEFT OUTER JOIN Ledger_Head g ON a.ReceivedFrom_Idno = g.Ledger_IdNo LEFT OUTER JOIN Ledger_Head h ON a.Yarn_ReceivedFrom_Idno = h.Ledger_IdNo where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.PavuYarn_Delivery_Code = '" & Trim(NewCode) & "'", con)
            prn_HdDt = New DataTable
            da1.Fill(prn_HdDt)

            If prn_HdDt.Rows.Count > 0 Then

                da2 = New SqlClient.SqlDataAdapter("select a.*, b.Mill_Name, c.Count_Name from Yarn_Delivery_Details a LEFT OUTER JOIN Mill_Head b on a.Mill_IdNo = b.Mill_IdNo INNER JOIN Count_Head c on a.Count_IdNo = c.Count_IdNo where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.PavuYarn_Delivery_Code = '" & Trim(NewCode) & "' Order by a.sl_no", con)
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

                da1 = New SqlClient.SqlDataAdapter("select a.*, b.* from Pavu_Delivery_Details a INNER JOIN EndsCount_Head b ON a.EndsCount_IdNo = b.EndsCount_IdNo where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.PavuYarn_Delivery_Code = '" & Trim(NewCode) & "' Order by a.sl_no", con)
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

                da1 = New SqlClient.SqlDataAdapter("select a.*, b.* from Pavu_Delivery_Details a INNER JOIN EndsCount_Head b ON a.EndsCount_IdNo = b.EndsCount_IdNo where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.PavuYarn_Delivery_Code = '" & Trim(NewCode) & "' Order by a.sl_no", con)
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

                da1 = New SqlClient.SqlDataAdapter("select top 1 a.Yarn_Type, c.Count_Name, d.Mill_Name, b.Total_Bags, b.Total_Cones, b.Total_Weight, b.Total_Thiri from Yarn_Delivery_Details a INNER JOIN PavuYarn_Delivery_Head b ON a.PavuYarn_Delivery_Code = b.PavuYarn_Delivery_Code INNER JOIN Count_Head c on a.Count_IdNo = c.Count_IdNo LEFT OUTER JOIN Mill_Head d on a.Mill_IdNo = d.Mill_IdNo where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.PavuYarn_Delivery_Code = '" & Trim(NewCode) & "' Order by a.sl_no", con)
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
                        'If Val(Common_Procedures.settings.Weaver_YarnStock_InThiri_Status = 1) Then
                        '    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Thiri").ToString), "########0.00"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) - 10, CurY, 1, 0, pFont)
                        'Else
                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Weight").ToString), "########0.000"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) - 10, CurY, 1, 0, pFont)
                        'End If

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

        da2 = New SqlClient.SqlDataAdapter("SELECT a.Amount as Yarn_Amount, a.Total_Amount as Pavu_Amount FROM PavuYarn_Delivery_Head a INNER JOIN Yarn_Delivery_Details b ON a.PavuYarn_Delivery_Code = b.PavuYarn_Delivery_Code WHERE a.PavuYarn_Delivery_Code = '" & Trim(NewCode) & "'", con)
        'da2 = New SqlClient.SqlDataAdapter("select Sum(a.Weight * b.Rate_Kg) as Value_Of_Yarn from Yarn_Delivery_Details a INNER JOIN Mill_Count_Details b ON a.Mill_IdNo = b.Mill_IdNo and a.Count_IdNo = b.Count_IdNo Where a.PavuYarn_Delivery_Code = '" & Trim(NewCode) & "'", con)
        dt2 = New DataTable
        da2.Fill(dt2)

        If dt2.Rows.Count > 0 Then

            Yarn_value = Format(Val(dt2.Rows(0).Item("Yarn_Amount").ToString), "########0.00")
            pavu_value = Format(Val(dt2.Rows(0).Item("Pavu_Amount").ToString), "########0.00")

        End If

        'If dt3.Rows.Count > 0 Then
        '  pavu_value = Format(Val(dt3.Rows(0).Item("Value_Of_Pavu").ToString), "#######0.00")
        ' End If

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
            Common_Procedures.Print_To_PrintDocument(e, "NO  :  " & prn_HdDt.Rows(0).Item("PavuYarn_Delivery_No").ToString, PageWidth - 10, CurY, 1, 0, pFont)
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

            pFont = New Font("Calibri", 10, FontStyle.Bold)
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
            'If Trim(UCase(Common_Procedures.settings.Weaver_YarnStock_InMeter_Status)) = 1 Then
            '    If Val(prn_HdDt.Rows(0).Item("Total_Thiri").ToString) <> 0 Then
            '        If is_LastPage = True Then
            '            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Total_Thiri").ToString), "#########0.000"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) - 10, CurY, 1, 0, pFont)
            '        End If
            '    End If
            'Else
            If Val(prn_HdDt.Rows(0).Item("Total_Weight").ToString) <> 0 Then
                If is_LastPage = True Then
                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Total_Weight").ToString), "#########0.000"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) - 10, CurY, 1, 0, pFont)
                End If
            End If
            'End If


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
            Common_Procedures.Print_To_PrintDocument(e, "Date : " & Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("PavuYarn_Delivery_Date").ToString), "dd-MM-yyyy"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + 10, CurY, 0, 0, p1Font)

            CurY = CurY + TxtHgt + 10

            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            e.Graphics.DrawLine(Pens.Black, LMargin, LnAr(1), LMargin, CurY)
            e.Graphics.DrawLine(Pens.Black, PageWidth, LnAr(1), PageWidth, CurY)

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub
    Public Sub Get_vehicle_from_Transport()
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim transport_id As Integer
        transport_id = Common_Procedures.Ledger_NameToIdNo(con, cbo_Transport.Text)
        Da = New SqlClient.SqlDataAdapter("select vehicle_no from ledger_head where ledger_idno=" & Str(Val(transport_id)) & "", con)
        Dt = New DataTable
        Da.Fill(Dt)
        If Dt.Rows.Count <> 0 Then
            cbo_VehicleNo.Text = Dt.Rows(0).Item("vehicle_no").ToString


        End If
        Dt.Clear()
    End Sub
    Private Sub cbo_Transport_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Transport.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'TRANSPORT')", "(Ledger_IdNo = 0)")
        If Common_Procedures.settings.CustomerCode = "1186" Then
            Get_vehicle_from_Transport()
        End If

    End Sub

    Private Sub cbo_Transport_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Transport.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Transport, Nothing, cbo_VehicleNo, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'TRANSPORT')", "(Ledger_IdNo = 0)")

        If (e.KeyValue = 38 And cbo_Transport.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then

            SendKeys.Send("+{TAB}")

        End If
        If Common_Procedures.settings.CustomerCode = "1186" Then
            Get_vehicle_from_Transport()
        End If

    End Sub

    Private Sub txt_Frieght_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_Freight.KeyDown

        If (e.KeyValue = 40) Then
            If cbo_WidthType.Visible Then
                cbo_WidthType.Focus()
            ElseIf cbo_ClothSales_OrderCode_forSelection.Visible = True Then
                cbo_ClothSales_OrderCode_forSelection.Focus()
            Else
                SendKeys.Send("{TAB}")
            End If

        End If

        If e.KeyCode = 38 Then cbo_VehicleNo.Focus()
    End Sub

    Private Sub cbo_Transport_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Transport.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Transport, cbo_VehicleNo, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'TRANSPORT')", "(Ledger_IdNo = 0)")
        If Common_Procedures.settings.CustomerCode = "1186" Then
            Get_vehicle_from_Transport()
        End If
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
            If cbo_Cloth.Visible = False Then
                If cbo_WidthType.Visible = True Then
                    cbo_WidthType.Focus()

                ElseIf cbo_ClothSales_OrderCode_forSelection.Visible = True Then
                    cbo_ClothSales_OrderCode_forSelection.Focus()

                Else
                    dgv_YarnDetails.Focus()
                    dgv_YarnDetails.CurrentCell = dgv_YarnDetails.Rows(0).Cells(1)
                    dgv_YarnDetails.CurrentCell.Selected = True
                End If

            Else

                cbo_Cloth.Focus()


            End If

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
            txt_Rate.Focus()
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

    Private Sub cbo_RecForm_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_PavuRecForm.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "( ( Ledger_Type = 'WEAVER' or Ledger_Type = 'SIZING' or Ledger_Type = 'GODOWN' or Ledger_Type = 'REWINDING' or Ledger_Type = 'JOBWORKER' or (Ledger_Type = '' and Stock_Maintenance_Status = 1) or Show_In_All_Entry = 1) and Close_status = 0 )", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_RecForm_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_PavuRecForm.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_PavuRecForm, cbo_YarnRecForm, "Ledger_AlaisHead", "Ledger_DisplayName", "( ( Ledger_Type = 'WEAVER' or Ledger_Type = 'SIZING' or Ledger_Type = 'GODOWN' or Ledger_Type = 'REWINDING' or Ledger_Type = 'JOBWORKER' or (Ledger_Type = '' and Stock_Maintenance_Status = 1) or Show_In_All_Entry = 1) and Close_status = 0 )", "(Ledger_IdNo = 0)")
        If Asc(e.KeyChar) = 13 Then

            If MessageBox.Show("Do you want to select Pavu  :", "FOR PAVU SELECTION...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = DialogResult.Yes Then
                btn_Selection_Click(sender, e)

            Else
                txt_PartyDcNo.Focus()

            End If

        End If
    End Sub

    Private Sub cbo_RecForm_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_PavuRecForm.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_PavuRecForm, cbo_DelvAt, cbo_YarnRecForm, "Ledger_AlaisHead", "Ledger_DisplayName", "( ( Ledger_Type = 'WEAVER' or Ledger_Type = 'SIZING' or Ledger_Type = 'GODOWN' or Ledger_Type = 'REWINDING' or Ledger_Type = 'JOBWORKER' or (Ledger_Type = '' and Stock_Maintenance_Status = 1) or Show_In_All_Entry = 1) and Close_status = 0 )", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_RecForm_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_PavuRecForm.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then

            Common_Procedures.MDI_LedType = "SIZING"
            Dim f As New Ledger_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_PavuRecForm.Name
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
        Dim CompIDCondt As String

        LedNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_PavuRecForm.Text)

        If LedNo = 0 Then
            MessageBox.Show("Invalid Party Name", "DOES NOT SELECT PAVU...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If cbo_PavuRecForm.Enabled And cbo_PavuRecForm.Visible Then cbo_PavuRecForm.Focus()
            Exit Sub
        End If

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        CompIDCondt = "(a.company_idno = " & Str(Val(lbl_Company.Tag)) & ")"
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1155" Then
            If Common_Procedures.settings.EntrySelection_Combine_AllCompany = 1 Then
                CompIDCondt = ""
                If Trim(UCase(Common_Procedures.User.Type)) = "ACCOUNT" Then
                    CompIDCondt = "(Company_Type <> 'UNACCOUNT')"
                End If
            End If
        End If

        Cmd.Connection = con

        Cmd.CommandText = "truncate table " & Trim(Common_Procedures.EntryTempSubTable) & ""
        Cmd.ExecuteNonQuery()

        With dgv_Selection

            chk_SelectAll.Checked = False

            .Rows.Clear()

            SNo = 0

            Da = New SqlClient.SqlDataAdapter("select a.noof_used as Ent_NoofUsed, b.*, c.EndsCount_Name, d.Beam_Width_Name from Pavu_Delivery_Details a INNER JOIN Company_Head tZ ON a.company_idno = tZ.company_idno INNER JOIN Stock_SizedPavu_Processing_Details b ON a.Set_Code = b.Set_Code and a.Beam_No = b.Beam_No INNER JOIN EndsCount_Head c ON a.EndsCount_IdNo = c.EndsCount_IdNo LEFT OUTER JOIN Beam_Width_Head d ON b.Beam_Width_Idno = d.Beam_Width_Idno where " & CompIDCondt & IIf(Trim(CompIDCondt) <> "", " and ", "") & " a.PavuYarn_Delivery_Code = '" & Trim(NewCode) & "' and a.ReceivedFrom_IdNo = " & Str(Val(LedNo)) & " order by a.for_orderby, a.sl_no, a.Set_Code, b.ForOrderBy_BeamNo, a.Beam_No", con)
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

            Da = New SqlClient.SqlDataAdapter("select a.*, b.EndsCount_Name, c.Beam_Width_Name from Stock_SizedPavu_Processing_Details a  INNER JOIN Company_Head tZ ON a.company_idno = tZ.company_idno LEFT OUTER JOIN EndsCount_Head b ON a.EndsCount_IdNo = b.EndsCount_IdNo LEFT OUTER JOIN Beam_Width_Head c ON a.Beam_Width_Idno = c.Beam_Width_Idno where " & CompIDCondt & IIf(Trim(CompIDCondt) <> "", " and ", "") & "  a.StockAt_IdNo = " & Str(Val(LedNo)) & " and (a.Pavu_Delivery_Code = '' and a.Beam_Knotting_Code = '' and a.Close_Status = 0) order by a.for_orderby, a.Set_Code, a.ForOrderBy_BeamNo, a.Beam_No, a.sl_no", con)
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


                For I = 0 To dgv_Selection.RowCount - 1

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

    Private Sub cbo_YarnRecForm_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_YarnRecForm.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "( ( Ledger_Type = 'WEAVER' or Ledger_Type = 'SIZING' or Ledger_Type = 'GODOWN' or Ledger_Type = 'REWINDING' or Ledger_Type = 'JOBWORKER' or (Ledger_Type = '' and Stock_Maintenance_Status = 1) or Show_In_All_Entry = 1) and Close_status = 0 )", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_YarnRecForm_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_YarnRecForm.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_YarnRecForm, txt_PartyDcNo, "Ledger_AlaisHead", "Ledger_DisplayName", "( ( Ledger_Type = 'WEAVER' or Ledger_Type = 'SIZING' or Ledger_Type = 'GODOWN' or Ledger_Type = 'REWINDING' or Ledger_Type = 'JOBWORKER' or (Ledger_Type = '' and Stock_Maintenance_Status = 1) or Show_In_All_Entry = 1) and Close_status = 0 )", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_YarnRecForm_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_YarnRecForm.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_YarnRecForm, cbo_PavuRecForm, txt_PartyDcNo, "Ledger_AlaisHead", "Ledger_DisplayName", "( ( Ledger_Type = 'WEAVER' or Ledger_Type = 'SIZING' or Ledger_Type = 'GODOWN' or Ledger_Type = 'REWINDING' or Ledger_Type = 'JOBWORKER' or (Ledger_Type = '' and Stock_Maintenance_Status = 1) or Show_In_All_Entry = 1) and Close_status = 0 )", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_YarnRecForm_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_YarnRecForm.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then

            Common_Procedures.UR.Ledr_Wea_Siz_Rw_Trans_JbWrk_Creation = Common_Procedures.UR.Weaver_Creation
            Common_Procedures.MDI_LedType = "WEAVER"
            Dim f As New Ledger_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_YarnRecForm.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()
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
    Private Sub cbo_Cloth_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Cloth.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Cloth_Head", "cloth_name", "", "(cloth_idno=0)")
    End Sub

    Private Sub cbo_cloth_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Cloth.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Cloth, txt_Freight, Nothing, "Cloth_Head", "cloth_name", "", "(cloth_idno=0)")
        If (e.KeyValue = 40 And cbo_Cloth.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
            If cbo_WidthType.Visible Then
                cbo_WidthType.Focus()
            ElseIf cbo_ClothSales_OrderCode_forSelection.Visible = True Then
                cbo_ClothSales_OrderCode_forSelection.Focus()
            Else
                dgv_YarnDetails.Focus()
                dgv_YarnDetails.CurrentCell = dgv_YarnDetails.Rows(0).Cells(1)
                dgv_YarnDetails.CurrentCell.Selected = True
            End If

        End If
    End Sub

    Private Sub cbo_cloth_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Cloth.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Cloth, Nothing, "Cloth_Head", "cloth_name", "", "(cloth_idno=0)")

        If Asc(e.KeyChar) = 13 Then
            If cbo_WidthType.Visible Then
                cbo_WidthType.Focus()
            ElseIf cbo_ClothSales_OrderCode_forSelection.Visible = True Then
                cbo_ClothSales_OrderCode_forSelection.Focus()
            Else
                dgv_YarnDetails.Focus()
                dgv_YarnDetails.CurrentCell = dgv_YarnDetails.Rows(0).Cells(1)
                dgv_YarnDetails.CurrentCell.Selected = True
            End If

        End If

    End Sub

    Private Sub set_PaperSize_For_PrintDocument1()
        Dim I As Integer = 0
        Dim PpSzSTS As Boolean = False
        Dim ps As Printing.PaperSize


        If Val(Common_Procedures.settings.Printing_For_HalfSheet_Set_Custom8X6_As_Default_PaperSize) = 1 Then
            Dim pkCustomSize1 As New System.Drawing.Printing.PaperSize("PAPER 8X6", 800, 600)
            PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = pkCustomSize1
            PrintDocument1.DefaultPageSettings.PaperSize = pkCustomSize1
            PrintDocument1.DefaultPageSettings.Landscape = False

        ElseIf Val(Common_Procedures.settings.Printing_For_HalfSheet_Set_A4_As_Default_PaperSize) = 1 Then
            For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
                If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
                    ps = PrintDocument1.PrinterSettings.PaperSizes(I)
                    PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = ps
                    PrintDocument1.DefaultPageSettings.PaperSize = ps
                    Exit For
                End If
            Next



        Else


            PpSzSTS = False

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

        End If
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1204" Then  '--- kohinoorTextile
            For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
                If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A5 Then
                    ps = PrintDocument1.PrinterSettings.PaperSizes(I)
                    PrintDocument1.DefaultPageSettings.PaperSize = ps
                    PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = ps
                    PrintDocument1.DefaultPageSettings.Landscape = True
                    PpSzSTS = True
                    Exit For
                End If
            Next
        End If
    End Sub


    Private Function get_GST_Tax_Percentage_For_Printing(ByVal EntryCode As String) As Single
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim Dt2 As New DataTable
        Dim TaxPerc As Single = 0
        Dim LedIdNo As Integer = 0

        Dim InterStateStatus As Boolean = False
        TaxPerc = 0


        LedIdNo = 0
        InterStateStatus = False
        LedIdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_DelvAt.Text)
        Da = New SqlClient.SqlDataAdapter("select a.*, d.* ,e.*  from Yarn_Delivery_Details a  LEFT OUTER JOIN Count_Head d ON A.Count_idno = d.Count_idno LEFT OUTER JOIN itemgroup_head e ON d.itemgroup_idno = e.itemgroup_idno Where PavuYarn_Delivery_Code = '" & Trim(EntryCode) & "'", con)
        Dt1 = New DataTable
        Da.Fill(Dt1)
        If Dt1.Rows.Count > 0 Then
            If Dt1.Rows.Count = 1 Then

                Da = New SqlClient.SqlDataAdapter("select a.*,  d.* ,e.*  from Yarn_Delivery_Details a  LEFT OUTER JOIN Count_Head d ON a.Count_idno = d.Count_idno LEFT OUTER JOIN itemgroup_head e ON d.itemgroup_idno = e.itemgroup_idno Where PavuYarn_Delivery_Code = '" & Trim(EntryCode) & "'", con)
                Dt2 = New DataTable
                Da.Fill(Dt2)
                If Dt2.Rows.Count > 0 Then
                    If InterStateStatus = True Then
                        TaxPerc = Val(Dt2.Rows(0).Item("Item_GST_Percentage").ToString)
                    Else
                        TaxPerc = Val(Dt2.Rows(0).Item("Item_GST_Percentage").ToString) / 2
                    End If
                End If
                Dt2.Clear()

            End If
        End If

        Dt2.Dispose()
        Da.Dispose()

        get_GST_Tax_Percentage_For_Printing = Format(Val(TaxPerc), "#########0.00")

    End Function

    Private Sub Printing_GST_HSN_Details_Format1(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByRef CurY As Single, ByVal LMargin As Integer, ByVal PageWidth As Integer, ByVal PrintWidth As Double, ByVal LnAr As Single)
        Dim cmd As New SqlClient.SqlCommand
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim Da1 As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim I As Integer, Cgst_Perc As Single = 0
        Dim p1Font As Font
        Dim p2Font As Font
        Dim SubClAr(15) As Single
        Dim ItmNm1 As String, ItmNm2 As String
        Dim SNo As Integer = 0
        Dim Sgst_Perc As Single = 0
        Dim Igst_Perc As Single = 0
        Dim Ttl_TaxAmt As Double, Ttl_CGst As Double, Ttl_Sgst As Double, Ttl_igst As Double
        Dim LnAr2 As Single
        Dim BmsInWrds As String
        Dim TaxAmt As Double, CGstAmt As Double, SgstAmt As Double, IgstAmt As Double
        Dim NoofItems_Increment As Integer
        Dim NoofDets As Integer
        Dim LedIdNo As Integer = 0
        Dim Hsn_Code As String = ""
        Dim Ass_value As Double = 0, gst_per As Double = 0
        Dim InterStateStatus As Boolean = False


        Try

            cmd.Connection = con

            cmd.CommandText = "Truncate table " & Trim(Common_Procedures.EntryTempTable) & ""
            cmd.ExecuteNonQuery()

            TxtHgt = TxtHgt - 1

            p2Font = New Font("Calibri", 9, FontStyle.Regular)
            InterStateStatus = False
            Ttl_TaxAmt = 0 : Ttl_CGst = 0 : Ttl_Sgst = 0 : Ttl_igst = 0 : Cgst_Perc = 0 : Sgst_Perc = 0 : Igst_Perc = 0
            TaxAmt = 0 : CGstAmt = 0 : SgstAmt = 0 : IgstAmt = 0
            Erase SubClAr
            LedIdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_DelvAt.Text)
            InterStateStatus = Common_Procedures.Is_InterState_Party(con, Val(lbl_Company.Tag), LedIdNo)
            SubClAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}

            SubClAr(1) = 110 : SubClAr(2) = 150 : SubClAr(3) = 65 : SubClAr(4) = 80 : SubClAr(5) = 65 : SubClAr(6) = 80 : SubClAr(7) = 55
            SubClAr(8) = PageWidth - (LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5) + SubClAr(6) + SubClAr(7))

            Common_Procedures.Print_To_PrintDocument(e, "HSN/SAC", LMargin, CurY + 5, 2, SubClAr(1), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "TAXABLE AMOUNT", LMargin + SubClAr(1), CurY + 5, 2, SubClAr(2), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "CGST", LMargin + SubClAr(1) + SubClAr(2), CurY, 2, SubClAr(3) + SubClAr(4), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "SGST ", LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4), CurY, 2, SubClAr(5) + SubClAr(6), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "IGST", LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5) + SubClAr(6), CurY, 2, SubClAr(7) + SubClAr(8), pFont)

            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin + SubClAr(1) + SubClAr(2), CurY, PageWidth, CurY)
            LnAr2 = CurY
            CurY = CurY + 5

            Common_Procedures.Print_To_PrintDocument(e, "%", LMargin + SubClAr(1) + SubClAr(2), CurY, 2, SubClAr(3), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "Amount", LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3), CurY, 2, SubClAr(4), pFont)

            Common_Procedures.Print_To_PrintDocument(e, "%", LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4), CurY, 2, SubClAr(5), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "Amount", LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5), CurY, 2, SubClAr(6), pFont)

            Common_Procedures.Print_To_PrintDocument(e, "%", LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5) + SubClAr(6), CurY, 2, SubClAr(7), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "Amount", LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5) + SubClAr(6) + SubClAr(7), CurY, 2, SubClAr(8), pFont)

            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

            Da1 = New SqlClient.SqlDataAdapter("select a.*, b.Mill_name, d.* ,e.*  from Yarn_Delivery_Details a INNER JOIN Mill_Head b ON a.Mill_idno = b.Mill_idno LEFT OUTER JOIN Count_Head d ON a.Count_idno = d.Count_idno LEFT OUTER JOIN itemgroup_head e ON d.itemgroup_idno = e.itemgroup_idno where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.PavuYarn_Delivery_Code = '" & Trim(EntryCode) & "' Order by a.Sl_No", con)
            Dt1 = New DataTable
            Da1.Fill(Dt1)
            If Dt1.Rows.Count > 0 Then
                For I = 0 To Dt1.Rows.Count - 1
                    Hsn_Code = Trim(Dt1.Rows(I).Item("Item_HSN_Code").ToString)
                    gst_per = (Dt1.Rows(I).Item("Item_GST_Percentage").ToString)
                    Ass_value = (Dt1.Rows(I).Item("Weight").ToString) * (Dt1.Rows(I).Item("Rate").ToString)

                    cmd.CommandText = "Insert into " & Trim(Common_Procedures.EntryTempTable) & " (                    Name1        ,         Currency1            ,   Currency2          ) " &
                                      "            Values    (       '" & Trim(Hsn_Code) & "'   , " & (Val(gst_per)) & ", " & Str(Val(Ass_value)) & " ) "
                    cmd.ExecuteNonQuery()
                Next
            End If

            cmd.CommandText = "Truncate table " & Trim(Common_Procedures.EntryTempSubTable) & ""
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Insert into " & Trim(Common_Procedures.EntryTempSubTable) & "(Int1, Int2, Meters1) Select a.EndsCount_IdNo, count(a.Beam_No), sum(a.Meters) from Pavu_Delivery_Details a Where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.PavuYarn_Delivery_Code = '" & Trim(EntryCode) & "' group by a.EndsCount_IdNo"
            cmd.ExecuteNonQuery()
            If Val(prn_HdDt.Rows(0).Item("Meters").ToString) <> 0 Then
                cmd.CommandText = "Insert into " & Trim(Common_Procedures.EntryTempSubTable) & "(Int1, Int2, Meters1) values (" & Str(Val(prn_HdDt.Rows(0).Item("EndsCount_IdNo").ToString)) & ", " & Str(Val(prn_HdDt.Rows(0).Item("Empty_Beam").ToString)) & ", " & Str(Val(prn_HdDt.Rows(0).Item("Meters").ToString)) & ")"
                cmd.ExecuteNonQuery()
            End If

            Da1 = New SqlClient.SqlDataAdapter("Select b.Endscount_Name, IG.Item_HSN_Code, IG.Item_GST_Percentage, sum(a.Meters1) as PavuMtrs, sum(a.Int2) as Beams from " & Trim(Common_Procedures.EntryTempSubTable) & " a INNER JOIN EndsCount_Head b ON a.Int1 = b.EndsCount_IdNo INNER JOIN Count_Head c ON b.Count_IdNo = c.Count_IdNo LEFT OUTER JOIN ItemGroup_Head IG ON c.ItemGroup_IdNo = IG.ItemGroup_IdNo  group by b.EndsCount_Name, IG.Item_HSN_Code, IG.Item_GST_Percentage Order by b.EndsCount_Name, IG.Item_HSN_Code, IG.Item_GST_Percentage", con)
            Dt1 = New DataTable
            Da1.Fill(Dt1)
            If Dt1.Rows.Count > 0 Then
                For I = 0 To Dt1.Rows.Count - 1
                    Hsn_Code = Trim(Dt1.Rows(I).Item("Item_HSN_Code").ToString)
                    gst_per = (Dt1.Rows(I).Item("Item_GST_Percentage").ToString)
                    Ass_value = Val(Dt1.Rows(I).Item("Beams").ToString) * Val(prn_HdDt.Rows(0).Item("Rate").ToString)

                    cmd.CommandText = "Insert into " & Trim(Common_Procedures.EntryTempTable) & " (                    Name1        ,         Currency1            ,   Currency2          ) " &
                                      "            Values    (       '" & Trim(Hsn_Code) & "'   , " & (Val(gst_per)) & ", " & Str(Val(Ass_value)) & " ) "
                    cmd.ExecuteNonQuery()

                Next

            End If

            Da = New SqlClient.SqlDataAdapter("select Name1 as HSN_Code, Currency1 as GST_Percentage, sum(Currency2) as Assessable_Value from " & Trim(Common_Procedures.EntryTempTable) & " group by name1, Currency1 Having sum(Currency2) <> 0 ", con)
            Dt = New DataTable
            Da.Fill(Dt)

            If Dt.Rows.Count > 0 Then

                prn_DetIndx = 0
                NoofDets = 0
                NoofItems_Increment = 0

                CurY = CurY - 20

                Do While prn_DetIndx <= Dt.Rows.Count - 1

                    ItmNm1 = Trim(Dt.Rows(prn_DetIndx).Item("HSN_Code").ToString)

                    ItmNm2 = ""
                    If Len(ItmNm1) > 40 Then
                        For I = 35 To 1 Step -1
                            If Mid$(Trim(ItmNm1), I, 1) = " " Or Mid$(Trim(ItmNm1), I, 1) = "," Or Mid$(Trim(ItmNm1), I, 1) = "." Or Mid$(Trim(ItmNm1), I, 1) = "-" Or Mid$(Trim(ItmNm1), I, 1) = "/" Or Mid$(Trim(ItmNm1), I, 1) = "_" Or Mid$(Trim(ItmNm1), I, 1) = "(" Or Mid$(Trim(ItmNm1), I, 1) = ")" Or Mid$(Trim(ItmNm1), I, 1) = "\" Or Mid$(Trim(ItmNm1), I, 1) = "[" Or Mid$(Trim(ItmNm1), I, 1) = "]" Or Mid$(Trim(ItmNm1), I, 1) = "{" Or Mid$(Trim(ItmNm1), I, 1) = "}" Then Exit For
                        Next I
                        If I = 0 Then I = 40
                        ItmNm2 = Microsoft.VisualBasic.Right(Trim(ItmNm1), Len(ItmNm1) - I)
                        ItmNm1 = Microsoft.VisualBasic.Left(Trim(ItmNm1), I - 1)
                    End If
                    If InterStateStatus = True Then
                        Igst_Perc = (Dt.Rows(prn_DetIndx).Item("GST_Percentage").ToString)
                    Else
                        Cgst_Perc = Format(Val(Dt.Rows(prn_DetIndx).Item("GST_Percentage").ToString) / 2, "############0.00")
                        Sgst_Perc = Format(Val(Dt.Rows(prn_DetIndx).Item("GST_Percentage").ToString) / 2, "############0.00")

                    End If

                    TaxAmt = (Dt.Rows(prn_DetIndx).Item("Assessable_Value").ToString)
                    CGstAmt = Format(Val(TaxAmt) * (Cgst_Perc) / 100, "###########0.00")
                    SgstAmt = Format(Val(TaxAmt) * (Sgst_Perc) / 100, "###########0.00")
                    IgstAmt = Format(Val(TaxAmt) * (Igst_Perc) / 100, "###########0.00")
                    CurY = CurY + TxtHgt + 3

                    Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm1), LMargin + 10, CurY, 0, 0, p2Font)
                    Common_Procedures.Print_To_PrintDocument(e, IIf(Val(TaxAmt) <> 0, Common_Procedures.Currency_Format(Val(TaxAmt)), ""), LMargin + SubClAr(1) + SubClAr(2) - 10, CurY, 1, 0, p2Font)
                    Common_Procedures.Print_To_PrintDocument(e, IIf(Val(Cgst_Perc) <> 0, Common_Procedures.Currency_Format(Val(Cgst_Perc)), ""), LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) - 10, CurY, 1, 0, p2Font)
                    Common_Procedures.Print_To_PrintDocument(e, IIf(Val(CGstAmt) <> 0, Common_Procedures.Currency_Format(Val(CGstAmt)), ""), LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) - 5, CurY, 1, 0, p2Font)
                    Common_Procedures.Print_To_PrintDocument(e, IIf(Val(Sgst_Perc) <> 0, Common_Procedures.Currency_Format(Val(Sgst_Perc)), ""), LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5) - 10, CurY, 1, 0, p2Font)
                    Common_Procedures.Print_To_PrintDocument(e, IIf(Val(SgstAmt) <> 0, Common_Procedures.Currency_Format(Val(SgstAmt)), ""), LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5) + SubClAr(6) - 5, CurY, 1, 0, p2Font)
                    Common_Procedures.Print_To_PrintDocument(e, IIf(Val(Igst_Perc) <> 0, Common_Procedures.Currency_Format(Val(Igst_Perc)), ""), LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5) + SubClAr(6) + SubClAr(7) - 10, CurY, 1, 0, p2Font)
                    Common_Procedures.Print_To_PrintDocument(e, IIf(Val(IgstAmt) <> 0, Common_Procedures.Currency_Format(Val(IgstAmt)), ""), LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5) + SubClAr(6) + SubClAr(7) + SubClAr(8) - 5, CurY, 1, 0, p2Font)

                    NoofItems_Increment = NoofItems_Increment + 1

                    NoofDets = NoofDets + 1

                    Ttl_TaxAmt = Ttl_TaxAmt + Val(TaxAmt)
                    Ttl_CGst = Ttl_CGst + Val(CGstAmt)
                    Ttl_Sgst = Ttl_Sgst + Val(SgstAmt)
                    Ttl_igst = Ttl_igst + Val(IgstAmt)
                    prn_DetIndx = prn_DetIndx + 1
                Loop

            End If

            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            CurY = CurY + TxtHgt - 15
            Common_Procedures.Print_To_PrintDocument(e, "Total", LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, IIf(Val(Ttl_TaxAmt) <> 0, Common_Procedures.Currency_Format(Val(Ttl_TaxAmt)), ""), LMargin + SubClAr(1) + SubClAr(2) - 10, CurY, 1, 0, p2Font)
            Common_Procedures.Print_To_PrintDocument(e, IIf(Val(Ttl_CGst) <> 0, Common_Procedures.Currency_Format(Val(Ttl_CGst)), ""), LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) - 5, CurY, 1, 0, p2Font)
            Common_Procedures.Print_To_PrintDocument(e, IIf(Val(Ttl_Sgst) <> 0, Common_Procedures.Currency_Format(Val(Ttl_Sgst)), ""), LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5) + SubClAr(6) - 5, CurY, 1, 0, p2Font)
            Common_Procedures.Print_To_PrintDocument(e, IIf(Val(Ttl_igst) <> 0, Common_Procedures.Currency_Format(Val(Ttl_igst)), ""), LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5) + SubClAr(6) + SubClAr(7) + SubClAr(8) - 5, CurY, 1, 0, p2Font)
            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

            e.Graphics.DrawLine(Pens.Black, LMargin + SubClAr(1), CurY, LMargin + SubClAr(1), LnAr)
            e.Graphics.DrawLine(Pens.Black, LMargin + SubClAr(1) + SubClAr(2), CurY, LMargin + SubClAr(1) + SubClAr(2), LnAr)
            e.Graphics.DrawLine(Pens.Black, LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3), CurY, LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3), LnAr2)
            e.Graphics.DrawLine(Pens.Black, LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4), CurY, LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4), LnAr)
            e.Graphics.DrawLine(Pens.Black, LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5), CurY, LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5), LnAr2)

            e.Graphics.DrawLine(Pens.Black, LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5) + SubClAr(6), CurY, LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5) + SubClAr(6), LnAr)
            e.Graphics.DrawLine(Pens.Black, LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5) + SubClAr(6) + SubClAr(7), CurY, LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5) + SubClAr(6) + SubClAr(7), LnAr2)


            CurY = CurY + 5
            p1Font = New Font("Calibri", 12, FontStyle.Regular)
            BmsInWrds = ""
            BmsInWrds = Common_Procedures.Rupees_Converstion(Val(Ttl_CGst) + Val(Ttl_Sgst) + Val(Ttl_igst))
            BmsInWrds = Replace(Trim(BmsInWrds), "", "")


            p1Font = New Font("Calibri", 10, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "Tax Amount (In Words) : " & BmsInWrds & " ", LMargin + 10, CurY, 0, 0, p1Font)

            CurY = CurY + TxtHgt + 5
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try



    End Sub


    Private Sub Printing_GST_HSN_Details_Format1_111(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByRef CurY As Single, ByVal LMargin As Integer, ByVal PageWidth As Integer, ByVal PrintWidth As Double, ByVal LnAr As Single)
        Dim cmd As New SqlClient.SqlCommand
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim Da1 As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim I As Integer, Cgst_Perc As Single = 0
        Dim p1Font As Font
        Dim p2Font As Font
        Dim SubClAr(15) As Single
        Dim ItmNm1 As String, ItmNm2 As String
        Dim SNo As Integer = 0
        Dim Sgst_Perc As Single = 0
        Dim Igst_Perc As Single = 0
        Dim Ttl_TaxAmt As Double, Ttl_CGst As Double, Ttl_Sgst As Double, Ttl_igst As Double
        Dim LnAr2 As Single
        Dim BmsInWrds As String
        Dim TaxAmt As Double, CGstAmt As Double, SgstAmt As Double, IgstAmt As Double
        Dim NoofItems_Increment As Integer
        Dim NoofDets As Integer
        Dim LedIdNo As Integer = 0
        Dim Hsn_Code As String = ""
        Dim Ass_value As Double = 0, gst_per As Double = 0
        Dim InterStateStatus As Boolean = False
        ' Dim cmd As New SqlClient.SqlCommand
        Try
            cmd.Connection = con

            cmd.CommandText = "Truncate table " & Trim(Common_Procedures.EntryTempTable) & ""
            cmd.ExecuteNonQuery()

            TxtHgt = TxtHgt - 1

            p2Font = New Font("Calibri", 9, FontStyle.Regular)
            InterStateStatus = False
            Ttl_TaxAmt = 0 : Ttl_CGst = 0 : Ttl_Sgst = 0 : Ttl_igst = 0 : Cgst_Perc = 0 : Sgst_Perc = 0 : Igst_Perc = 0
            TaxAmt = 0 : CGstAmt = 0 : SgstAmt = 0 : IgstAmt = 0
            Erase SubClAr
            LedIdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_DelvAt.Text)
            InterStateStatus = Common_Procedures.Is_InterState_Party(con, Val(lbl_Company.Tag), LedIdNo)
            SubClAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}

            SubClAr(1) = 110 : SubClAr(2) = 150 : SubClAr(3) = 65 : SubClAr(4) = 80 : SubClAr(5) = 65 : SubClAr(6) = 80 : SubClAr(7) = 55
            SubClAr(8) = PageWidth - (LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5) + SubClAr(6) + SubClAr(7))

            Common_Procedures.Print_To_PrintDocument(e, "HSN/SAC", LMargin, CurY + 5, 2, SubClAr(1), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "TAXABLE AMOUNT", LMargin + SubClAr(1), CurY + 5, 2, SubClAr(2), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "CGST", LMargin + SubClAr(1) + SubClAr(2), CurY, 2, SubClAr(3) + SubClAr(4), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "SGST ", LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4), CurY, 2, SubClAr(5) + SubClAr(6), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "IGST", LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5) + SubClAr(6), CurY, 2, SubClAr(7) + SubClAr(8), pFont)

            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin + SubClAr(1) + SubClAr(2), CurY, PageWidth, CurY)
            LnAr2 = CurY
            CurY = CurY + 5

            Common_Procedures.Print_To_PrintDocument(e, "%", LMargin + SubClAr(1) + SubClAr(2), CurY, 2, SubClAr(3), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "Amount", LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3), CurY, 2, SubClAr(4), pFont)

            Common_Procedures.Print_To_PrintDocument(e, "%", LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4), CurY, 2, SubClAr(5), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "Amount", LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5), CurY, 2, SubClAr(6), pFont)

            Common_Procedures.Print_To_PrintDocument(e, "%", LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5) + SubClAr(6), CurY, 2, SubClAr(7), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "Amount", LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5) + SubClAr(6) + SubClAr(7), CurY, 2, SubClAr(8), pFont)

            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

            Da1 = New SqlClient.SqlDataAdapter("select a.*, b.Mill_name, d.* ,e.*  from Yarn_Delivery_Details a INNER JOIN Mill_Head b ON a.Mill_idno = b.Mill_idno LEFT OUTER JOIN Count_Head d ON a.Count_idno = d.Count_idno LEFT OUTER JOIN itemgroup_head e ON d.itemgroup_idno = e.itemgroup_idno where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.PavuYarn_Delivery_Code = '" & Trim(EntryCode) & "' Order by a.Sl_No", con)
            Dt1 = New DataTable
            Da1.Fill(Dt1)
            If Dt1.Rows.Count > 0 Then
                For I = 0 To Dt1.Rows.Count - 1
                    Hsn_Code = Trim(Dt1.Rows(I).Item("Item_HSN_Code").ToString)
                    gst_per = (Dt1.Rows(I).Item("Item_GST_Percentage").ToString)
                    Ass_value = (Dt1.Rows(I).Item("Weight").ToString) * (prn_HdDt.Rows(0).Item("Rate").ToString)

                    cmd.CommandText = "Insert into " & Trim(Common_Procedures.EntryTempTable) & " (                    Name1        ,         Currency1            ,   Currency2          ) " &
                                      "            Values    (       '" & Trim(Hsn_Code) & "'   , " & (Val(gst_per)) & ", " & Str(Val(Ass_value)) & " ) "
                    cmd.ExecuteNonQuery()
                Next
            End If

            Da = New SqlClient.SqlDataAdapter("select Name1 as HSN_Code, Currency1 as GST_Percentage, sum(Currency2) as Assessable_Value from " & Trim(Common_Procedures.EntryTempTable) & " group by name1, Currency1 Having sum(Currency2) <> 0 ", con)
            Dt = New DataTable
            Da.Fill(Dt)

            If Dt.Rows.Count > 0 Then

                prn_DetIndx = 0
                NoofDets = 0
                NoofItems_Increment = 0

                CurY = CurY - 20

                Do While prn_DetIndx <= Dt.Rows.Count - 1

                    ItmNm1 = Trim(Dt.Rows(prn_DetIndx).Item("HSN_Code").ToString)

                    ItmNm2 = ""
                    If Len(ItmNm1) > 40 Then
                        For I = 35 To 1 Step -1
                            If Mid$(Trim(ItmNm1), I, 1) = " " Or Mid$(Trim(ItmNm1), I, 1) = "," Or Mid$(Trim(ItmNm1), I, 1) = "." Or Mid$(Trim(ItmNm1), I, 1) = "-" Or Mid$(Trim(ItmNm1), I, 1) = "/" Or Mid$(Trim(ItmNm1), I, 1) = "_" Or Mid$(Trim(ItmNm1), I, 1) = "(" Or Mid$(Trim(ItmNm1), I, 1) = ")" Or Mid$(Trim(ItmNm1), I, 1) = "\" Or Mid$(Trim(ItmNm1), I, 1) = "[" Or Mid$(Trim(ItmNm1), I, 1) = "]" Or Mid$(Trim(ItmNm1), I, 1) = "{" Or Mid$(Trim(ItmNm1), I, 1) = "}" Then Exit For
                        Next I
                        If I = 0 Then I = 40
                        ItmNm2 = Microsoft.VisualBasic.Right(Trim(ItmNm1), Len(ItmNm1) - I)
                        ItmNm1 = Microsoft.VisualBasic.Left(Trim(ItmNm1), I - 1)
                    End If
                    If InterStateStatus = True Then
                        Igst_Perc = (Dt.Rows(prn_DetIndx).Item("GST_Percentage").ToString)
                    Else
                        Cgst_Perc = Format(Val(Dt.Rows(prn_DetIndx).Item("GST_Percentage").ToString) / 2, "############0.00")
                        Sgst_Perc = Format(Val(Dt.Rows(prn_DetIndx).Item("GST_Percentage").ToString) / 2, "############0.00")

                    End If

                    TaxAmt = (Dt.Rows(prn_DetIndx).Item("Assessable_Value").ToString)
                    CGstAmt = Format(Val(TaxAmt) * (Cgst_Perc) / 100, "###########0.00")
                    SgstAmt = Format(Val(TaxAmt) * (Sgst_Perc) / 100, "###########0.00")
                    IgstAmt = Format(Val(TaxAmt) * (Igst_Perc) / 100, "###########0.00")
                    CurY = CurY + TxtHgt + 3

                    Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm1), LMargin + 10, CurY, 0, 0, p2Font)
                    Common_Procedures.Print_To_PrintDocument(e, IIf(Val(TaxAmt) <> 0, Common_Procedures.Currency_Format(Val(TaxAmt)), ""), LMargin + SubClAr(1) + SubClAr(2) - 10, CurY, 1, 0, p2Font)
                    Common_Procedures.Print_To_PrintDocument(e, IIf(Val(Cgst_Perc) <> 0, Common_Procedures.Currency_Format(Val(Cgst_Perc)), ""), LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) - 10, CurY, 1, 0, p2Font)
                    Common_Procedures.Print_To_PrintDocument(e, IIf(Val(CGstAmt) <> 0, Common_Procedures.Currency_Format(Val(CGstAmt)), ""), LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) - 5, CurY, 1, 0, p2Font)
                    Common_Procedures.Print_To_PrintDocument(e, IIf(Val(Sgst_Perc) <> 0, Common_Procedures.Currency_Format(Val(Sgst_Perc)), ""), LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5) - 10, CurY, 1, 0, p2Font)
                    Common_Procedures.Print_To_PrintDocument(e, IIf(Val(SgstAmt) <> 0, Common_Procedures.Currency_Format(Val(SgstAmt)), ""), LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5) + SubClAr(6) - 5, CurY, 1, 0, p2Font)
                    Common_Procedures.Print_To_PrintDocument(e, IIf(Val(Igst_Perc) <> 0, Common_Procedures.Currency_Format(Val(Igst_Perc)), ""), LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5) + SubClAr(6) + SubClAr(7) - 10, CurY, 1, 0, p2Font)
                    Common_Procedures.Print_To_PrintDocument(e, IIf(Val(IgstAmt) <> 0, Common_Procedures.Currency_Format(Val(IgstAmt)), ""), LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5) + SubClAr(6) + SubClAr(7) + SubClAr(8) - 5, CurY, 1, 0, p2Font)

                    NoofItems_Increment = NoofItems_Increment + 1

                    NoofDets = NoofDets + 1

                    Ttl_TaxAmt = Ttl_TaxAmt + Val(TaxAmt)
                    Ttl_CGst = Ttl_CGst + Val(CGstAmt)
                    Ttl_Sgst = Ttl_Sgst + Val(SgstAmt)
                    Ttl_igst = Ttl_igst + Val(IgstAmt)
                    prn_DetIndx = prn_DetIndx + 1
                Loop

            End If

            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            CurY = CurY + TxtHgt - 15
            Common_Procedures.Print_To_PrintDocument(e, "Total", LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, IIf(Val(Ttl_TaxAmt) <> 0, Common_Procedures.Currency_Format(Val(Ttl_TaxAmt)), ""), LMargin + SubClAr(1) + SubClAr(2) - 10, CurY, 1, 0, p2Font)
            Common_Procedures.Print_To_PrintDocument(e, IIf(Val(Ttl_CGst) <> 0, Common_Procedures.Currency_Format(Val(Ttl_CGst)), ""), LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) - 5, CurY, 1, 0, p2Font)
            Common_Procedures.Print_To_PrintDocument(e, IIf(Val(Ttl_Sgst) <> 0, Common_Procedures.Currency_Format(Val(Ttl_Sgst)), ""), LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5) + SubClAr(6) - 5, CurY, 1, 0, p2Font)
            Common_Procedures.Print_To_PrintDocument(e, IIf(Val(Ttl_igst) <> 0, Common_Procedures.Currency_Format(Val(Ttl_igst)), ""), LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5) + SubClAr(6) + SubClAr(7) + SubClAr(8) - 5, CurY, 1, 0, p2Font)
            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

            e.Graphics.DrawLine(Pens.Black, LMargin + SubClAr(1), CurY, LMargin + SubClAr(1), LnAr)
            e.Graphics.DrawLine(Pens.Black, LMargin + SubClAr(1) + SubClAr(2), CurY, LMargin + SubClAr(1) + SubClAr(2), LnAr)
            e.Graphics.DrawLine(Pens.Black, LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3), CurY, LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3), LnAr2)
            e.Graphics.DrawLine(Pens.Black, LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4), CurY, LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4), LnAr)
            e.Graphics.DrawLine(Pens.Black, LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5), CurY, LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5), LnAr2)

            e.Graphics.DrawLine(Pens.Black, LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5) + SubClAr(6), CurY, LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5) + SubClAr(6), LnAr)
            e.Graphics.DrawLine(Pens.Black, LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5) + SubClAr(6) + SubClAr(7), CurY, LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5) + SubClAr(6) + SubClAr(7), LnAr2)


            CurY = CurY + 5
            p1Font = New Font("Calibri", 12, FontStyle.Regular)
            BmsInWrds = ""
            BmsInWrds = Common_Procedures.Rupees_Converstion(Val(Ttl_CGst) + Val(Ttl_Sgst) + Val(Ttl_igst))
            BmsInWrds = Replace(Trim(BmsInWrds), "", "")


            p1Font = New Font("Calibri", 10, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "Tax Amount (In Words) : " & BmsInWrds & " ", LMargin + 10, CurY, 0, 0, p1Font)

            CurY = CurY + TxtHgt + 5
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try



    End Sub

    Private Sub txt_rate_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_Rate.KeyDown
        If e.KeyCode = 40 Then
            txt_Note.Focus()
        End If

        If e.KeyCode = 38 Then SendKeys.Send("+{TAB}")
    End Sub

    Private Sub txt_rate_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Rate.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
        If Asc(e.KeyChar) = 13 Then
            txt_Note.Focus()
        End If
    End Sub

    Private Sub txt_rate_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_Rate.TextChanged
        TotalPavu_Calculation()

    End Sub
    Private Sub cbo_TransportMode_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_TransportMode.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "PavuYarn_Delivery_Head", "Transportation_Mode", "", "")
    End Sub
    Private Sub cbo_TransportMode_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_TransportMode.KeyDown
        Try
            Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_TransportMode, Nothing, txt_DateTime_Of_Supply, "PavuYarn_Delivery_Head", "Transportation_Mode", "", "")
            If (e.KeyValue = 38 And cbo_TransportMode.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
                If dgv_PavuDetails.Rows.Count > 0 Then
                    dgv_PavuDetails.Focus()
                    dgv_PavuDetails.CurrentCell = dgv_PavuDetails.Rows(0).Cells(1)

                Else
                    txt_Freight.Focus()

                End If
            End If
        Catch ex As Exception
            'MessageBox.Show(ex.Message, "DOES NOT SELECT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub cbo_TransportMode_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_TransportMode.KeyPress
        Try
            Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_TransportMode, txt_DateTime_Of_Supply, "PavuYarn_Delivery_Head", "Transportation_Mode", "", "", False)
        Catch ex As Exception
            'MessageBox.Show(ex.Message, "DOES NOT SELECT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub Printing_Format2Gst(ByRef e As System.Drawing.Printing.PrintPageEventArgs)
        Dim EntryCode As String
        Dim I As Integer, J As Integer, NoofDets As Integer, NoofItems_PerPage As Integer
        Dim pFont As Font
        Dim LMargin As Single, RMargin As Single, TMargin As Single, BMargin As Single
        Dim PrintWidth As Single, PrintHeight As Single
        Dim PageWidth As Single, PageHeight As Single
        Dim CurY As Single, TxtHgt As Single
        Dim Cmp_Name As String
        Dim LnAr(15) As Single, ClArr(15) As Single
        Dim ItmNm1 As String, ItmNm2 As String
        Dim ps As Printing.PaperSize
        Dim W1 As Single = 0
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt2 As New DataTable
        Dim NewCode As String = ""
        Dim Cmd As New SqlClient.SqlCommand

        Cmd.Connection = con

        For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
            If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
                ps = PrintDocument1.PrinterSettings.PaperSizes(I)
                PrintDocument1.DefaultPageSettings.PaperSize = ps
                Exit For
            End If
        Next

        With PrintDocument1.DefaultPageSettings.Margins
            .Left = 30 ' 30 '60
            .Right = 60
            .Top = 40 ' 60
            .Bottom = 40
            LMargin = .Left
            RMargin = .Right
            TMargin = .Top
            BMargin = .Bottom
        End With

        pFont = New Font("Calibri", 9, FontStyle.Regular)
        'pFont = New Font("Calibri", 12, FontStyle.Regular)

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

        TxtHgt = 19 ' e.Graphics.MeasureString("A", pFont).Height  ' 20


        NoofItems_PerPage = 12


        Erase LnAr
        Erase ClArr

        LnAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        ClArr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}

        ClArr(1) = Val(30) : ClArr(2) = 250 : ClArr(3) = 60 : ClArr(4) = 55 : ClArr(5) = 60 : ClArr(6) = 85 : ClArr(7) = 75
        ClArr(8) = PageWidth - (LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7))


        ' ClArr(2) = 230 : ClArr(3) = 85 : ClArr(4) = 50 : ClArr(5) = 80 : ClArr(6) = 55 : ClArr(7) = 80
        ' ClArr(8) = PageWidth - (LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7))

        CurY = TMargin

        Cmp_Name = Trim(prn_HdDt.Rows(0).Item("Company_Name").ToString)

        EntryCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Try

            prn_Prev_HeadIndx = prn_HeadIndx

            If prn_HdDt.Rows.Count > 0 Then

                Printing_Format2Gst_PageHeader(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, prn_PageNo, NoofItems_PerPage, CurY, LnAr, ClArr)

                W1 = e.Graphics.MeasureString("Ends Count : ", pFont).Width

                NoofDets = 0
                'DetSNo = 0
                CurY = CurY - 10
                prn_DetIndx = 0

                If prn_DetMxIndx > 0 Then

                    If prn_DetDt.Rows.Count > 0 Then

                        da2 = New SqlClient.SqlDataAdapter("select b.Count_Name, c.Mill_Name, d.Item_HSN_Code, d.Item_GST_Percentage, a.Rate as YarnRate, sum(a.Bags) as YarnBag, sum(a.Weight) as YarnWeight, sum(a.Amount) as YarnAmount from Yarn_Delivery_Details a INNER JOIN Count_Head b on a.Count_IdNo = b.Count_IdNo LEFT OUTER JOIN Mill_Head c ON a.Mill_IdNo = c.Mill_IdNo LEFT OUTER JOIN itemgroup_head d ON b.itemgroup_idno = d.itemgroup_idno Where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.PavuYarn_Delivery_Code = '" & Trim(EntryCode) & "' Group by b.Count_Name, c.Mill_Name, d.Item_HSN_Code, d.Item_GST_Percentage, a.Rate Order by b.Count_Name, c.Mill_Name, d.Item_HSN_Code, d.Item_GST_Percentage, a.Rate", con)
                        dt2 = New DataTable
                        da2.Fill(dt2)
                        If dt2.Rows.Count > 0 Then

                            For I = 0 To dt2.Rows.Count - 1

                                prn_DetSNo = prn_DetSNo + 1

                                ItmNm1 = "Yarn - " & Trim(prn_DetDt.Rows(prn_DetIndx).Item("Count_Name").ToString) & " " & Trim(prn_DetDt.Rows(prn_DetIndx).Item("Mill_Name").ToString)
                                ItmNm2 = ""
                                If Len(ItmNm1) > 25 Then
                                    For J = 25 To 1 Step -1
                                        If Mid$(Trim(ItmNm1), J, 1) = " " Or Mid$(Trim(ItmNm1), J, 1) = "," Or Mid$(Trim(ItmNm1), J, 1) = "." Or Mid$(Trim(ItmNm1), J, 1) = "-" Or Mid$(Trim(ItmNm1), J, 1) = "/" Or Mid$(Trim(ItmNm1), J, 1) = "_" Or Mid$(Trim(ItmNm1), J, 1) = "(" Or Mid$(Trim(ItmNm1), J, 1) = ")" Or Mid$(Trim(ItmNm1), J, 1) = "\" Or Mid$(Trim(ItmNm1), J, 1) = "[" Or Mid$(Trim(ItmNm1), J, 1) = "]" Or Mid$(Trim(ItmNm1), J, 1) = "{" Or Mid$(Trim(ItmNm1), J, 1) = "}" Then Exit For
                                    Next J
                                    If J = 0 Then J = 25
                                    ItmNm2 = Microsoft.VisualBasic.Right(Trim(ItmNm1), Len(ItmNm1) - J)
                                    ItmNm1 = Microsoft.VisualBasic.Left(Trim(ItmNm1), J - 1)
                                End If

                                CurY = CurY + TxtHgt

                                Common_Procedures.Print_To_PrintDocument(e, Trim(Val(prn_DetSNo)), LMargin + 15, CurY, 0, 0, pFont)
                                Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm1), LMargin + ClArr(1) + 10, CurY, 0, 0, pFont)
                                Common_Procedures.Print_To_PrintDocument(e, dt2.Rows(I).Item("Item_HSN_Code").ToString, LMargin + ClArr(1) + ClArr(2) + 10, CurY, 0, 0, pFont)
                                Common_Procedures.Print_To_PrintDocument(e, dt2.Rows(I).Item("Item_GST_Percentage").ToString, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) - 5, CurY, 1, 0, pFont)

                                If Val(dt2.Rows(I).Item("YarnBag").ToString) <> 0 Then
                                    Common_Procedures.Print_To_PrintDocument(e, Val(dt2.Rows(I).Item("YarnBag").ToString), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) - 10, CurY, 1, 0, pFont)
                                End If
                                Common_Procedures.Print_To_PrintDocument(e, (dt2.Rows(I).Item("YarnWeight").ToString), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) - 10, CurY, 1, 0, pFont)
                                Common_Procedures.Print_To_PrintDocument(e, (dt2.Rows(I).Item("YarnRate").ToString), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) - 10, CurY, 1, 0, pFont)
                                Common_Procedures.Print_To_PrintDocument(e, Format(Val(dt2.Rows(I).Item("YarnAmount").ToString), " #######0.00"), PageWidth - 10, CurY, 1, 0, pFont)


                                vprn_Tot_Bgs_Bms = Val(vprn_Tot_Bgs_Bms) + Val(dt2.Rows(I).Item("YarnBag").ToString)
                                vprn_Tot_Wgt_Mtr = Format(Val(vprn_Tot_Wgt_Mtr) + Val(dt2.Rows(I).Item("YarnWeight").ToString), "##########0.000")
                                vprn_Tot_Amt = Format(Val(vprn_Tot_Amt) + Val(dt2.Rows(I).Item("YarnAmount").ToString), "##########0.000")

                                NoofDets = NoofDets + 1

                                If Trim(ItmNm2) <> "" Then
                                    CurY = CurY + TxtHgt - 5
                                    Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm2), LMargin + ClArr(1) + 10, CurY, 0, 0, pFont)
                                    NoofDets = NoofDets + 1
                                End If

                                prn_DetIndx = prn_DetIndx + 1

                            Next

                        End If
                        dt2.Clear()

                    End If


                    If prn_DetDt1.Rows.Count > 0 Or Val(prn_HdDt.Rows(0).Item("Meters").ToString) <> 0 Then

                        Cmd.CommandText = "Truncate table " & Trim(Common_Procedures.EntryTempTable) & ""
                        Cmd.ExecuteNonQuery()

                        Cmd.CommandText = "Insert into " & Trim(Common_Procedures.EntryTempTable) & "(Int1, Int2, Meters1) Select a.EndsCount_IdNo, count(a.Beam_No), sum(a.Meters) from Pavu_Delivery_Details a Where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.PavuYarn_Delivery_Code = '" & Trim(EntryCode) & "' group by a.EndsCount_IdNo"
                        Cmd.ExecuteNonQuery()
                        If Val(prn_HdDt.Rows(0).Item("Meters").ToString) <> 0 Then
                            Cmd.CommandText = "Insert into " & Trim(Common_Procedures.EntryTempTable) & "(Int1, Int2, Meters1) values (" & Str(Val(prn_HdDt.Rows(0).Item("EndsCount_IdNo").ToString)) & ", " & Str(Val(prn_HdDt.Rows(0).Item("Empty_Beam").ToString)) & ", " & Str(Val(prn_HdDt.Rows(0).Item("Meters").ToString)) & ")"
                            Cmd.ExecuteNonQuery()
                        End If

                        da2 = New SqlClient.SqlDataAdapter("Select b.Endscount_Name, IG.Item_HSN_Code, IG.Item_GST_Percentage, sum(a.Meters1) as PavuMtrs, sum(a.Int2) as Beams from " & Trim(Common_Procedures.EntryTempTable) & " a INNER JOIN EndsCount_Head b ON a.Int1 = b.EndsCount_IdNo INNER JOIN Count_Head c ON b.Count_IdNo = c.Count_IdNo LEFT OUTER JOIN ItemGroup_Head IG ON c.ItemGroup_IdNo = IG.ItemGroup_IdNo  group by b.EndsCount_Name, IG.Item_HSN_Code, IG.Item_GST_Percentage Order by b.EndsCount_Name, IG.Item_HSN_Code, IG.Item_GST_Percentage", con)
                        dt2 = New DataTable
                        da2.Fill(dt2)
                        If dt2.Rows.Count > 0 Then
                            prn_DetIndx = 0
                            For I = 0 To dt2.Rows.Count - 1
                                CurY = CurY + TxtHgt
                                prn_DetSNo = prn_DetSNo + 1

                                Common_Procedures.Print_To_PrintDocument(e, prn_DetSNo, LMargin + 15, CurY, 0, 0, pFont)
                                Common_Procedures.Print_To_PrintDocument(e, " Pavu - " & dt2.Rows(I).Item("Endscount_Name").ToString, LMargin + ClArr(1) + 10, CurY, 0, 0, pFont)
                                Common_Procedures.Print_To_PrintDocument(e, dt2.Rows(I).Item("Item_HSN_Code").ToString, LMargin + ClArr(1) + ClArr(2) + 10, CurY, 0, 0, pFont)
                                Common_Procedures.Print_To_PrintDocument(e, dt2.Rows(I).Item("Item_GST_Percentage").ToString, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) - 5, CurY, 1, 0, pFont)
                                Common_Procedures.Print_To_PrintDocument(e, dt2.Rows(I).Item("Beams").ToString, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) - 10, CurY, 1, 0, pFont)
                                Common_Procedures.Print_To_PrintDocument(e, dt2.Rows(I).Item("PavuMtrs").ToString, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) - 10, CurY, 1, 0, pFont)
                                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Rate").ToString, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) - 10, CurY, 1, 0, pFont)
                                Common_Procedures.Print_To_PrintDocument(e, Format(Val(dt2.Rows(I).Item("beams").ToString) * Val(prn_HdDt.Rows(0).Item("Rate").ToString), "##########0.00"), PageWidth - 10, CurY, 1, 0, pFont)

                                vprn_Tot_Bgs_Bms = Val(vprn_Tot_Bgs_Bms) + Val(dt2.Rows(I).Item("Beams").ToString)
                                vprn_Tot_Wgt_Mtr = Format(Val(vprn_Tot_Wgt_Mtr) + Val(dt2.Rows(I).Item("PavuMtrs").ToString), "##########0.000")
                                vprn_Tot_Amt = Format(Val(vprn_Tot_Amt) + Val(Format(Val(dt2.Rows(I).Item("beams").ToString) * Val(prn_HdDt.Rows(0).Item("Rate").ToString), "##########0.00")), "##########0.000")

                                NoofDets = NoofDets + 1

                            Next

                        End If
                        dt2.Clear()

                    End If


                End If


                Printing_Format2Gst_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, Cmp_Name, NoofDets, True)


            End If

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

        e.HasMorePages = False

    End Sub

    Private Sub Printing_Format2Gst_PageHeader(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByRef PageNo As Integer, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single)
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt2 As New DataTable
        Dim p1Font As Font
        Dim Cmp_Name As String, Cmp_Add1 As String, Cmp_Add2 As String
        Dim Cmp_PhNo As String, Cmp_TinNo As String, Cmp_CstNo As String
        Dim Cmp_StateCap As String, Cmp_StateNm As String, Cmp_StateCode As String, Cmp_GSTIN_Cap As String, Cmp_GSTIN_No As String
        Dim strHeight As Single
        Dim Led_Name As String, Led_Add1 As String, Led_Add2 As String, Led_Add3 As String, Led_Add4 As String, Led_TinNo As String
        Dim Led_GSTTinNo As String, Led_State As String, Led_StateCd As String, LedAadhar_No As String, Led_PanNo As String
        Dim LedNmAr(10) As String
        Dim Cmp_Desc As String, Cmp_Email As String
        Dim Cen1 As Single = 0
        Dim W1 As Single = 0
        Dim W2 As Single = 0
        Dim LInc As Integer = 0
        Dim prn_OriDupTri As String = ""
        Dim S As String = ""
        Dim Led_PhNo As String
        Dim CurX As Single = 0
        Dim strWidth As Single = 0
        Dim BlockInvNoY As Single = 0
        Dim Trans_Nm As String = ""
        Dim Indx As Integer = 0
        Dim HdWd As Single = 0
        Dim H1 As Single = 0
        Dim W3 As Single = 0
        Dim C1 As Single = 0, C2 As Single = 0, C3 As Single = 0
        Dim i As Integer = 0
        Dim ItmNm1 As String, ItmNm2 As String

        PageNo = PageNo + 1

        CurY = TMargin

        prn_Count = prn_Count + 1

        'da2 = New SqlClient.SqlDataAdapter("select a.*, d.*  from Yarn_Delivery_Details a  LEFT OUTER JOIN Count_Head d ON a.Count_idno = d.Count_idno where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.PavuYarn_Delivery_Code = '" & Trim(EntryCode) & "' Order by a.Sl_No", con)
        'da2.Fill(dt2)
        'If dt2.Rows.Count > NoofItems_PerPage Then
        '    Common_Procedures.Print_To_PrintDocument(e, "Page : " & Trim(Val(PageNo)), PageWidth - 10, CurY - TxtHgt, 1, 0, pFont)
        'End If
        'dt2.Clear()

        'p1Font = New Font("Calibri", 12, FontStyle.Regular)
        'Common_Procedures.Print_To_PrintDocument(e, "INVOICE", LMargin, CurY - TxtHgt, 2, PrintWidth, p1Font)
        ' CurY = CurY + TxtHgt '+ 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(1) = CurY

        Cmp_Name = "" : Cmp_Add1 = "" : Cmp_Add2 = ""
        Cmp_PhNo = "" : Cmp_TinNo = "" : Cmp_CstNo = ""
        Cmp_Desc = "" : Cmp_Email = ""
        Cmp_StateCap = "" : Cmp_StateNm = "" : Cmp_StateCode = "" : Cmp_GSTIN_Cap = "" : Cmp_GSTIN_No = ""

        Cmp_Name = Trim(prn_HdDt.Rows(0).Item("Company_Name").ToString)

        Cmp_Add1 = Trim(prn_HdDt.Rows(0).Item("Company_Address1").ToString)
        If Trim(Cmp_Add1) <> "" Then
            If Microsoft.VisualBasic.Right(Trim(Cmp_Add1), 1) = "," Then
                Cmp_Add1 = Trim(Cmp_Add1) & ", " & Trim(prn_HdDt.Rows(0).Item("Company_Address2").ToString)
            Else
                Cmp_Add1 = Trim(Cmp_Add1) & " " & Trim(prn_HdDt.Rows(0).Item("Company_Address2").ToString)
            End If
        Else
            Cmp_Add1 = Trim(prn_HdDt.Rows(0).Item("Company_Address2").ToString)
        End If

        Cmp_Add2 = Trim(prn_HdDt.Rows(0).Item("Company_Address3").ToString)
        If Trim(Cmp_Add2) <> "" Then
            If Microsoft.VisualBasic.Right(Trim(Cmp_Add2), 1) = "," Then
                Cmp_Add2 = Trim(Cmp_Add2) & ", " & Trim(prn_HdDt.Rows(0).Item("Company_Address4").ToString)
            Else
                Cmp_Add2 = Trim(Cmp_Add2) & " " & Trim(prn_HdDt.Rows(0).Item("Company_Address4").ToString)
            End If
        Else
            Cmp_Add2 = Trim(prn_HdDt.Rows(0).Item("Company_Address4").ToString)
        End If

        If Trim(prn_HdDt.Rows(0).Item("Company_PhoneNo").ToString) <> "" Then
            Cmp_PhNo = "PHONE : " & Trim(prn_HdDt.Rows(0).Item("Company_PhoneNo").ToString)
        End If

        If Trim(prn_HdDt.Rows(0).Item("Company_TinNo").ToString) <> "" Then
            Cmp_TinNo = "TIN NO. : " & Trim(prn_HdDt.Rows(0).Item("Company_TinNo").ToString)
        End If
        If Trim(prn_HdDt.Rows(0).Item("Company_CstNo").ToString) <> "" Then
            Cmp_CstNo = "CST NO. : " & Trim(prn_HdDt.Rows(0).Item("Company_CstNo").ToString)
        End If
        If Trim(prn_HdDt.Rows(0).Item("Company_Description").ToString) <> "" Then
            Cmp_Desc = "(" & Trim(prn_HdDt.Rows(0).Item("Company_Description").ToString) & ")"
        End If
        If Trim(prn_HdDt.Rows(0).Item("Company_EMail").ToString) <> "" Then
            Cmp_Email = "E-mail : " & Trim(prn_HdDt.Rows(0).Item("Company_EMail").ToString)
        End If

        '***** GST START *****
        If Trim(prn_HdDt.Rows(0).Item("Company_State_Name").ToString) <> "" Then
            Cmp_StateCap = "STATE : "
            Cmp_StateNm = prn_HdDt.Rows(0).Item("Company_State_Name").ToString
        End If
        If Trim(prn_HdDt.Rows(0).Item("Company_State_Code").ToString) <> "" Then
            Cmp_StateCode = "CODE :" & prn_HdDt.Rows(0).Item("Company_State_Code").ToString
        End If
        If Trim(prn_HdDt.Rows(0).Item("Company_GSTinNo").ToString) <> "" Then
            Cmp_GSTIN_Cap = "GSTIN : "
            Cmp_GSTIN_No = prn_HdDt.Rows(0).Item("Company_GSTinNo").ToString
        End If
        '***** GST END *****

        CurY = CurY + TxtHgt - 10

        p1Font = New Font("Calibri", 18, FontStyle.Bold)

        If Trim(Common_Procedures.settings.CustomerCode) = "1154" Then
            e.Graphics.DrawString(Cmp_Name, p1Font, Brushes.Green, 266, CurY)
            'Common_Procedures.Print_To_PrintDocument(e, Cmp_Name, LMargin, CurY, 2, PrintWidth, p1Font)
        Else
            Common_Procedures.Print_To_PrintDocument(e, Cmp_Name, LMargin, CurY, 2, PrintWidth, p1Font)
        End If
        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height



        ItmNm1 = Trim(prn_HdDt.Rows(0).Item("Company_Description").ToString)
        ItmNm2 = ""
        If Trim(ItmNm1) <> "" Then
            ItmNm1 = "(" & Trim(ItmNm1) & ")"
            If Len(ItmNm1) > 85 Then
                For i = 85 To 1 Step -1
                    If Mid$(Trim(ItmNm1), i, 1) = " " Or Mid$(Trim(ItmNm1), i, 1) = "," Or Mid$(Trim(ItmNm1), i, 1) = "." Or Mid$(Trim(ItmNm1), i, 1) = "-" Or Mid$(Trim(ItmNm1), i, 1) = "/" Or Mid$(Trim(ItmNm1), i, 1) = "_" Or Mid$(Trim(ItmNm1), i, 1) = "(" Or Mid$(Trim(ItmNm1), i, 1) = ")" Or Mid$(Trim(ItmNm1), i, 1) = "\" Or Mid$(Trim(ItmNm1), i, 1) = "[" Or Mid$(Trim(ItmNm1), i, 1) = "]" Or Mid$(Trim(ItmNm1), i, 1) = "{" Or Mid$(Trim(ItmNm1), i, 1) = "}" Then Exit For
                Next i
                If i = 0 Then i = 85
                ItmNm2 = Microsoft.VisualBasic.Right(Trim(ItmNm1), Len(ItmNm1) - i)
                ItmNm1 = Microsoft.VisualBasic.Left(Trim(ItmNm1), i - 1)
            End If
        End If

        If Trim(ItmNm1) <> "" Then
            CurY = CurY + strHeight - 1
            p1Font = New Font("Calibri", 10, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm1), LMargin, CurY, 2, PrintWidth, p1Font)
            strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height
        End If

        If Trim(ItmNm2) <> "" Then
            CurY = CurY + strHeight - 1
            p1Font = New Font("Calibri", 10, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm2), LMargin, CurY, 2, PrintWidth, p1Font)
            strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height

        End If
        CurY = CurY + strHeight
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Add1, LMargin, CurY, 2, PrintWidth, pFont)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Add2, LMargin, CurY, 2, PrintWidth, pFont)

        '***** GST START *****
        CurY = CurY + TxtHgt

        p1Font = New Font("Calibri", 11, FontStyle.Bold)
        strWidth = e.Graphics.MeasureString(Trim(Cmp_StateCap & Cmp_GSTIN_Cap), p1Font).Width
        strWidth = e.Graphics.MeasureString(Trim(Cmp_StateCap & Cmp_StateNm & "     " & Cmp_GSTIN_Cap & Cmp_GSTIN_No), pFont).Width
        If PrintWidth > strWidth Then
            CurX = LMargin + (PrintWidth - strWidth) / 2
        Else
            CurX = LMargin
        End If
        pFont = New Font("Calibri", 11, FontStyle.Regular)
        p1Font = New Font("Calibri", 11, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, Cmp_StateCap, CurX, CurY, 0, 0, p1Font)
        strWidth = e.Graphics.MeasureString(Cmp_StateCap, p1Font).Width
        CurX = CurX + strWidth
        Common_Procedures.Print_To_PrintDocument(e, Cmp_StateNm, CurX, CurY, 0, 0, pFont)

        strWidth = e.Graphics.MeasureString(Cmp_StateNm, pFont).Width
        p1Font = New Font("Calibri", 11, FontStyle.Bold)
        CurX = CurX + strWidth
        Common_Procedures.Print_To_PrintDocument(e, "     " & Cmp_GSTIN_Cap, CurX, CurY, 0, PrintWidth, p1Font)
        strWidth = e.Graphics.MeasureString("     " & Cmp_GSTIN_Cap, p1Font).Width
        CurX = CurX + strWidth
        Common_Procedures.Print_To_PrintDocument(e, Cmp_GSTIN_No, CurX, CurY, 0, 0, pFont)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, Trim(Cmp_PhNo & "   " & Cmp_Email), LMargin, CurY, 2, PrintWidth, pFont)
        pFont = New Font("Calibri", 10, FontStyle.Regular)
        '***** GST END *****
        CurY = CurY + TxtHgt
        p1Font = New Font("Calibri", 16, FontStyle.Bold)

        Common_Procedures.Print_To_PrintDocument(e, "FORM GST DELIVERY CHALLAN", LMargin, CurY, 2, PrintWidth, p1Font)


        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height
        CurY = CurY + TxtHgt + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(2) = CurY

        Try

            Led_Name = "" : Led_Add1 = "" : Led_Add2 = "" : Led_Add3 = "" : Led_Add4 = "" : Led_TinNo = "" : Led_PhNo = "" : Led_GSTTinNo = "" : Led_State = ""

            Led_StateCd = ""
            LedAadhar_No = "" : Led_PanNo = ""
            Led_Name = "M/s. " & Trim(prn_HdDt.Rows(0).Item("Del_Name").ToString)

            Led_Add1 = Trim(prn_HdDt.Rows(0).Item("Ledger_Address1").ToString)
            Led_Add2 = Trim(prn_HdDt.Rows(0).Item("Ledger_Address2").ToString)
            Led_Add3 = Trim(prn_HdDt.Rows(0).Item("Ledger_Address3").ToString) & " " & Trim(prn_HdDt.Rows(0).Item("Ledger_Address4").ToString)
            'Led_Add4 = ""  'Trim(prn_HdDt.Rows(0).Item("Ledger_Address4").ToString)
            '***** GST START *****
            Led_TinNo = "Tin No : " & Trim(prn_HdDt.Rows(0).Item("Ledger_TinNo").ToString)
            If Trim(prn_HdDt.Rows(0).Item("Ledger_PhoneNo").ToString) <> "" Then Led_PhNo = "Phone No : " & Trim(prn_HdDt.Rows(0).Item("Ledger_PhoneNo").ToString)

            Led_State = "State : " & Trim(prn_HdDt.Rows(0).Item("Ledger_State_Name").ToString) & "  Code  :" & Trim(prn_HdDt.Rows(0).Item("Ledger_State_Code").ToString)
            Led_StateCd = Trim(prn_HdDt.Rows(0).Item("Ledger_State_Code").ToString)
            If Trim(prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString) <> "" Then Led_GSTTinNo = " GSTIN : " & Trim(prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString)
            If Trim(prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString) = "" Then
                If Trim(prn_HdDt.Rows(0).Item("Pan_No").ToString) <> "" Then Led_GSTTinNo = "Pan No : " & Trim(prn_HdDt.Rows(0).Item("Pan_No").ToString)
                If Trim(prn_HdDt.Rows(0).Item("Aadhar_No").ToString) <> "" Then LedAadhar_No = "AAdhar No : " & Trim(prn_HdDt.Rows(0).Item("Aadhar_No").ToString)
            End If
            '***** GST END *****



            Erase LedNmAr
            LedNmAr = New String(10) {}
            LInc = 0

            LInc = LInc + 1
            LedNmAr(LInc) = Led_Name

            If Trim(Led_Add1) <> "" Then
                LInc = LInc + 1
                LedNmAr(LInc) = Led_Add1
            End If

            If Trim(Led_Add2) <> "" Then
                LInc = LInc + 1
                LedNmAr(LInc) = Led_Add2
            End If

            If Trim(Led_Add3) <> "" Then
                LInc = LInc + 1
                LedNmAr(LInc) = Led_Add3
            End If

            'If Trim(Led_Add4) <> "" Then
            '    LInc = LInc + 1
            '    LedNmAr(LInc) = Led_Add4
            'End If
            '***** GST START *****
            If Trim(Led_State) <> "" Then
                LInc = LInc + 1
                LedNmAr(LInc) = Led_State
            End If

            If Trim(Led_PhNo) <> "" Then
                LInc = LInc + 1
                LedNmAr(LInc) = Led_PhNo
            End If

            If Trim(Led_GSTTinNo) <> "" Then
                LInc = LInc + 1
                LedNmAr(LInc) = Led_GSTTinNo
            ElseIf Trim(Led_GSTTinNo) = "" Then
                LInc = LInc + 1
                LedNmAr(LInc) = LedAadhar_No
            End If
            'If Trim(LedAadhar_No) <> "" Then

            'End If
            '***** GST END *****

            Cen1 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4)
            W1 = e.Graphics.MeasureString("Date & Time of Supply : ", pFont).Width
            W2 = e.Graphics.MeasureString("TO :", pFont).Width

            CurY = CurY + TxtHgt
            BlockInvNoY = CurY

            '***** GST START *****
            Common_Procedures.Print_To_PrintDocument(e, "TO : ", LMargin + 10, CurY, 0, 0, pFont)
            p1Font = New Font("Calibri", 11, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, Trim(LedNmAr(1)), LMargin + W2 + 10, CurY, 0, 0, p1Font)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, Trim(LedNmAr(2)), LMargin + W2 + 10, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, Trim(LedNmAr(3)), LMargin + W2 + 10, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, Trim(LedNmAr(4)), LMargin + W2 + 10, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, Trim(LedNmAr(5)), LMargin + W2 + 10, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, Trim(LedNmAr(6)), LMargin + W2 + 10, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, Trim(LedNmAr(7)), LMargin + W2 + 10, CurY, 0, 0, pFont)
            'CurY = CurY + TxtHgt
            'Common_Procedures.Print_To_PrintDocument(e, Trim(LedNmAr(8)), LMargin + W2 + 10, CurY, 0, 0, pFont)
            '***** GST END *****


            '------------------- Invoice No Block

            '***** GST START *****
            p1Font = New Font("Calibri", 14, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "Delivery Note No.", LMargin + Cen1 + 10, BlockInvNoY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + Cen1 + W1 + 10, BlockInvNoY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("PavuYarn_Delivery_No").ToString, LMargin + Cen1 + W1 + 30, BlockInvNoY, 0, 0, p1Font)

            BlockInvNoY = BlockInvNoY + TxtHgt

            Common_Procedures.Print_To_PrintDocument(e, "Delivery Note Date", LMargin + Cen1 + 10, BlockInvNoY + 4, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + Cen1 + W1 + 10, BlockInvNoY + 4, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("PavuYarn_Delivery_Date").ToString), "dd-MM-yyyy"), LMargin + Cen1 + W1 + 30, BlockInvNoY + 4, 0, 0, pFont)


            BlockInvNoY = BlockInvNoY + TxtHgt



            ' BlockInvNoY = BlockInvNoY + TxtHgt
            If Trim(prn_HdDt.Rows(0).Item("Transportation_Mode").ToString) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, "Mode of Transport", LMargin + Cen1 + 10, BlockInvNoY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + Cen1 + W1 + 10, BlockInvNoY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Transportation_Mode").ToString, LMargin + Cen1 + W1 + 30, BlockInvNoY, 0, 0, pFont)
            End If







            BlockInvNoY = BlockInvNoY + TxtHgt
            If Trim(prn_HdDt.Rows(0).Item("Vehicle_No").ToString) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, "Vehicle Number", LMargin + Cen1 + 10, BlockInvNoY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + Cen1 + W1 + 10, BlockInvNoY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Vehicle_No").ToString, LMargin + Cen1 + W1 + 30, BlockInvNoY, 0, 0, pFont)
            End If



            BlockInvNoY = BlockInvNoY + TxtHgt
            If Trim(prn_HdDt.Rows(0).Item("Date_Time_Of_Supply").ToString) <> "" Then
                p1Font = New Font("Calibri", 10, FontStyle.Regular)
                Common_Procedures.Print_To_PrintDocument(e, "Date & Time of Supply", LMargin + Cen1 + 10, BlockInvNoY, 0, 0, p1Font)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + Cen1 + W1 + 10, BlockInvNoY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("PavuYarn_Delivery_Date").ToString), "dd-MM-yyyy") & " " & prn_HdDt.Rows(0).Item("Date_Time_Of_Supply").ToString, LMargin + Cen1 + W1 + 30, BlockInvNoY, 0, 0, p1Font)
            End If
            BlockInvNoY = BlockInvNoY + TxtHgt
            If Trim(prn_HdDt.Rows(0).Item("Place_Of_Supply").ToString) <> "" Then
                p1Font = New Font("Calibri", 10, FontStyle.Regular)
                Common_Procedures.Print_To_PrintDocument(e, "Place of Supply", LMargin + Cen1 + 10, BlockInvNoY, 0, 0, p1Font)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + Cen1 + W1 + 10, BlockInvNoY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Place_Of_Supply").ToString, LMargin + Cen1 + W1 + 30, BlockInvNoY, 0, 0, p1Font)
            End If

            CurY = CurY + TxtHgt + 10
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(3) = CurY

            e.Graphics.DrawLine(Pens.Black, LMargin + Cen1, LnAr(3), LMargin + Cen1, LnAr(2))



            CurY = CurY + TxtHgt - 10

            Common_Procedures.Print_To_PrintDocument(e, "SNO", LMargin, CurY, 2, ClAr(1), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "Description of Goods", LMargin + ClAr(1), CurY, 2, ClAr(2), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "Hsn Code", LMargin + ClAr(1) + ClAr(2), CurY, 2, ClAr(3), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "GST %", LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, 2, ClAr(4), pFont)

            Common_Procedures.Print_To_PrintDocument(e, "BAG /", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 2, ClAr(5), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "BEAM", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY + TxtHgt, 2, ClAr(5), pFont)

            Common_Procedures.Print_To_PrintDocument(e, "WEIGHT/", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, 2, ClAr(6), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "METER", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY + TxtHgt, 2, ClAr(6), pFont)

            Common_Procedures.Print_To_PrintDocument(e, "RATE", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, 2, ClAr(7), pFont)

            Common_Procedures.Print_To_PrintDocument(e, "AMOUNT", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, 2, ClAr(8), pFont)

            CurY = CurY + TxtHgt + 20
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(4) = CurY

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub Printing_Format2Gst_PageFooter(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single, ByVal Cmp_Name As String, ByVal NoofDets As Integer, ByVal is_LastPage As Boolean)
        Dim p1Font As Font
        Dim Rup1 As String, Rup2 As String
        Dim I As Integer
        Dim vTaxPerc As Single = 0
        Dim BnkDetAr() As String
        Dim BankNm1 As String = ""
        Dim BankNm2 As String = ""
        Dim BankNm3 As String = ""
        Dim BankNm4 As String = ""
        Dim BInc As Integer
        Dim w1 As Single = 0
        Dim w2 As Single = 0
        Dim Jurs As String = ""
        Dim vNoofHsnCodes As Integer = 0
        Dim Yax As Single
        Dim LedIdNo As Integer = 0
        Dim ItmIdNo As Integer = 0
        Dim InterStateStatus As Boolean = False
        Dim Rnd_off As Single = 0
        Dim NetAmt As Single = 0
        Dim NtAmt As Single = 0
        Dim CgstAmt As Single = 0
        Dim IgstAmt As Single = 0
        Dim SgstAmt As Single = 0
        LedIdNo = 0
        InterStateStatus = False
        w1 = e.Graphics.MeasureString("Ends Count : ", pFont).Width

        LedIdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_DelvAt.Text)
        InterStateStatus = Common_Procedures.Is_InterState_Party(con, Val(lbl_Company.Tag), LedIdNo)
        Try

            For I = NoofDets + 1 To NoofItems_PerPage
                CurY = CurY + TxtHgt
                prn_DetIndx = prn_DetIndx + 1
            Next

            CurY = CurY + TxtHgt

            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

            CurY = CurY + TxtHgt - 10
            If is_LastPage = True Then
                Common_Procedures.Print_To_PrintDocument(e, "TOTAL", LMargin + ClAr(1) + ClAr(2) - 10, CurY, 1, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, vprn_Tot_Bgs_Bms, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 5, CurY, 1, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, Format(Val(vprn_Tot_Wgt_Mtr), " ###########0.000"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 5, CurY, 1, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, Format(Val(vprn_Tot_Amt), "########0.00"), PageWidth - 5, CurY, 1, 0, pFont)
            End If
            CurY = CurY + TxtHgt + 5

            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(5) = CurY

            e.Graphics.DrawLine(Pens.Black, LMargin, LnAr(5), LMargin, LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1), LnAr(5), LMargin + ClAr(1), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2), LnAr(5), LMargin + ClAr(1) + ClAr(2), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3), LnAr(5), LMargin + ClAr(1) + ClAr(2) + ClAr(3), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), LnAr(5), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(5), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), LnAr(5), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), LnAr(5), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), LnAr(5), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9), LnAr(5), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9), LnAr(3))

            If is_LastPage = True Then
                Erase BnkDetAr
                If Trim(prn_HdDt.Rows(0).Item("Company_Bank_Ac_Details").ToString) <> "" Then
                    BnkDetAr = Split(Trim(prn_HdDt.Rows(0).Item("Company_Bank_Ac_Details").ToString), ",")

                    BInc = -1
                    Yax = CurY

                    Yax = Yax + TxtHgt - 10
                    'If Val(prn_PageNo) = 1 Then
                    p1Font = New Font("Calibri", 12, FontStyle.Bold Or FontStyle.Underline)
                    Common_Procedures.Print_To_PrintDocument(e, "OUR BANK", LMargin + 20, Yax, 0, 0, p1Font)
                    'Common_Procedures.Print_To_PrintDocument(e, BnkDetAr(0), LMargin + 20, CurY, 0, 0, pFont)
                    'End If

                    p1Font = New Font("Calibri", 11, FontStyle.Bold)
                    BInc = BInc + 1
                    If UBound(BnkDetAr) >= BInc Then
                        Yax = Yax + TxtHgt
                        Common_Procedures.Print_To_PrintDocument(e, Trim(BnkDetAr(BInc)), LMargin + 20, Yax, 0, 0, p1Font)
                    End If

                    BInc = BInc + 1
                    If UBound(BnkDetAr) >= BInc Then
                        Yax = Yax + TxtHgt - 3
                        Common_Procedures.Print_To_PrintDocument(e, Trim(BnkDetAr(BInc)), LMargin + 20, Yax, 0, 0, p1Font)
                    End If

                    BInc = BInc + 1
                    If UBound(BnkDetAr) >= BInc Then
                        Yax = Yax + TxtHgt - 3
                        Common_Procedures.Print_To_PrintDocument(e, Trim(BnkDetAr(BInc)), LMargin + 20, Yax, 0, 0, p1Font)
                    End If

                    BInc = BInc + 1
                    If UBound(BnkDetAr) >= BInc Then
                        Yax = Yax + TxtHgt - 3
                        Common_Procedures.Print_To_PrintDocument(e, Trim(BnkDetAr(BInc)), LMargin + 20, Yax, 0, 0, p1Font)
                    End If

                End If

            End If


            CurY = CurY + TxtHgt - 10
            If is_LastPage = True Then
                p1Font = New Font("Calibri", 10, FontStyle.Bold)
                Common_Procedures.Print_To_PrintDocument(e, "Taxable Value", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, p1Font)
                Common_Procedures.Print_To_PrintDocument(e, Format(Val(vprn_Tot_Amt), "##########0.00"), PageWidth - 5, CurY, 1, 0, p1Font)
            End If




            ''vTaxPerc = get_GST_Tax_Percentage_For_Printing(EntryCode)
            CgstAmt = Format(Val(vprn_Tot_Amt) * 2.5 / 100, "########0.00")
            SgstAmt = Format(Val(vprn_Tot_Amt) * 2.5 / 100, "########0.00")



            If InterStateStatus = True Then
                CurY = CurY + TxtHgt
                If is_LastPage = True Then
                    IgstAmt = Format(Val(vprn_Tot_Amt) * 5 / 100, "########0.00")
                    Common_Procedures.Print_To_PrintDocument(e, "IGST @ 5 %", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, pFont)

                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Amount").ToString) * Val(5) / 100, "########0.00"), PageWidth - 5, CurY, 1, 0, pFont)
                End If

            Else
                CurY = CurY + TxtHgt

                If is_LastPage = True Then

                    Common_Procedures.Print_To_PrintDocument(e, "CGST @ 2.5 %", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, pFont)

                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(vprn_Tot_Amt) * 2.5 / 100, "########0.00"), PageWidth - 5, CurY, 1, 0, pFont)
                End If



                CurY = CurY + TxtHgt
                If is_LastPage = True Then

                    Common_Procedures.Print_To_PrintDocument(e, "SGST @ 2.5 %", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, pFont)

                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(vprn_Tot_Amt) * 2.5 / 100, "########0.00"), PageWidth - 5, CurY, 1, 0, pFont)
                End If

            End If

            NetAmt = Val(vprn_Tot_Amt) + Val(SgstAmt) + Val(CgstAmt) + Val(IgstAmt)
            NtAmt = Format(Val(NetAmt), "#########0")
            NtAmt = Common_Procedures.Currency_Format(Val(NtAmt))

            Rnd_off = Format(Val(CSng(NtAmt)) - Val(NetAmt), "#########0.00")
            CurY = CurY + TxtHgt
            ' If Val(prn_HdDt.Rows(0).Item("Round_Off").ToString) <> 0 Then
            If is_LastPage = True Then
                Common_Procedures.Print_To_PrintDocument(e, "Round Off", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, Format(Val(Rnd_off), "########0.00"), PageWidth - 5, CurY, 1, 0, pFont)
            End If
            ' End If

            CurY = CurY + TxtHgt + 5
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, PageWidth, CurY)


            CurY = CurY + TxtHgt - 10

            p1Font = New Font("Calibri", 15, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "Net Amount", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, p1Font)
            If is_LastPage = True Then
                p1Font = New Font("Calibri", 12, FontStyle.Bold)
                Common_Procedures.Print_To_PrintDocument(e, Common_Procedures.Currency_Format(Val(NtAmt)), PageWidth - 5, CurY, 1, 0, p1Font)
            End If

            CurY = CurY + TxtHgt + 10
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(6) = CurY

            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), LnAr(6), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), LnAr(5))

            Rup1 = ""
            Rup2 = ""
            If is_LastPage = True Then
                Rup1 = Common_Procedures.Rupees_Converstion(Val(NtAmt))
                If Len(Rup1) > 80 Then
                    For I = 80 To 1 Step -1
                        If Mid$(Trim(Rup1), I, 1) = " " Then Exit For
                    Next I
                    If I = 0 Then I = 80
                    Rup2 = Microsoft.VisualBasic.Right(Trim(Rup1), Len(Rup1) - I)
                    Rup1 = Microsoft.VisualBasic.Left(Trim(Rup1), I - 1)
                End If
            End If

            '***** GST START *****
            CurY = CurY + TxtHgt - 12
            p1Font = New Font("Calibri", 10, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "Amount Chargeable (In Words)  : " & Rup1, LMargin + 10, CurY, 0, 0, p1Font)
            If Trim(Rup2) <> "" Then
                CurY = CurY + TxtHgt - 5
                Common_Procedures.Print_To_PrintDocument(e, "                                " & Rup2, LMargin + 10, CurY, 0, 0, p1Font)
            End If
            '***** GST END *****

            CurY = CurY + TxtHgt + 5
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(10) = CurY

            'vNoofHsnCodes = get_GST_Noof_HSN_Codes_For_Printing(EntryCode)
            'If vNoofHsnCodes <> 0 Then
            Printing_GST_HSN_Details_Format1(e, EntryCode, TxtHgt, pFont, CurY, LMargin, PageWidth, PrintWidth, LnAr(10))
            'End If

            CurY = CurY + TxtHgt - 5

            p1Font = New Font("Calibri", 12, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "For " & Cmp_Name, PageWidth - 10, CurY, 1, 0, p1Font)



            CurY = CurY + TxtHgt
            CurY = CurY + TxtHgt
            CurY = CurY + TxtHgt

            Common_Procedures.Print_To_PrintDocument(e, "Prepared by", LMargin + 15, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "Checked by", LMargin + ClAr(1) + ClAr(2) - 20, CurY, 1, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "Authorised Signatory", PageWidth - 10, CurY, 1, 0, pFont)



            CurY = CurY + TxtHgt + 10

            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            e.Graphics.DrawLine(Pens.Black, LMargin, LnAr(1), LMargin, CurY)
            e.Graphics.DrawLine(Pens.Black, PageWidth, LnAr(1), PageWidth, CurY)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Function get_GST_Noof_HSN_Codes_For_Printing(ByVal EntryCode As String) As Integer
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim NoofHsnCodes As Integer = 0

        NoofHsnCodes = 0

        Da = New SqlClient.SqlDataAdapter("Select * from Yarn_Delivery_Details Where PavuYarn_Delivery_Code = '" & Trim(EntryCode) & "'", con)
        Dt1 = New DataTable
        Da.Fill(Dt1)
        If Dt1.Rows.Count > 0 Then
            NoofHsnCodes = Dt1.Rows.Count
        End If
        Dt1.Clear()

        Dt1.Dispose()
        Da.Dispose()

        get_GST_Noof_HSN_Codes_For_Printing = NoofHsnCodes

    End Function
    Private Sub get_Bag_Details()
        Dim q As Single = 0
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim Da1 As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim Fgr_bag As Single
        Dim Frm_Wt As Single
        Dim Frg_Bag As Single = 0
        Dim To_Wgt As Single
        Dim CntID As Integer
        Dim MilID As Integer
        Dim LedID As Integer = 0
        Dim cmd As New SqlClient.SqlCommand
        CntID = Common_Procedures.Count_NameToIdNo(con, dgv_YarnDetails.Rows(dgv_YarnDetails.CurrentRow.Index).Cells(1).Value)
        MilID = Common_Procedures.Mill_NameToIdNo(con, dgv_YarnDetails.Rows(dgv_YarnDetails.CurrentRow.Index).Cells(3).Value)
        LedID = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_DelvAt.Text)
        Frm_Wt = 0 : To_Wgt = 0 : Fgr_bag = 0


        cmd.Connection = con

        cmd.CommandText = "Truncate table " & Trim(Common_Procedures.EntryTempTable) & ""
        cmd.ExecuteNonQuery()
        If LedID <> 0 And CntID <> 0 And MilID <> 0 Then

            Da = New SqlClient.SqlDataAdapter("select * from Ledger_Freight_Charge_Details where   Ledger_idno = " & Str(Val(LedID)), con)
            Dt = New DataTable
            Da.Fill(Dt)




            If Dt.Rows.Count > 0 Then

                If IsDBNull(Dt.Rows(0)(0).ToString) = False Then
                    For I = 0 To Dt.Rows.Count - 1
                        Frm_Wt = Dt.Rows(I).Item("From_Weight").ToString
                        To_Wgt = Dt.Rows(I).Item("To_Weight").ToString
                        Fgr_bag = Dt.Rows(I).Item("Freight_Bag").ToString
                        cmd.CommandText = "Insert into " & Trim(Common_Procedures.EntryTempTable) & " (       Weight1      ,         Weight2            ,   Weight3               ,Weight4 ) " &
                                      "            Values    (       " & Val(Frm_Wt) & "   , " & (Val(To_Wgt)) & ", " & Str(Val(Fgr_bag)) & " ," & Val(dgv_YarnDetails.Rows(dgv_YarnDetails.CurrentRow.Index).Cells(6).Value) & ") "
                        cmd.ExecuteNonQuery()
                        Da1 = New SqlClient.SqlDataAdapter("select Weight1 as From_Wt, Weight2 as To_Wgt, Weight3 as Frg_Bag,Weight4 as Wgt from " & Trim(Common_Procedures.EntryTempTable) & " where Weight4 between " & Val(Frm_Wt) & " and " & Val(To_Wgt) & " ", con)
                        Dt1 = New DataTable
                        Da1.Fill(Dt1)

                        If Dt1.Rows.Count > 0 Then
                            Frg_Bag = Dt1.Rows(0).Item("Frg_Bag").ToString
                        End If
                    Next
                End If



            End If


            Dt.Clear()
            Dt.Dispose()
            Da.Dispose()
            With dgv_YarnDetails
                If .CurrentCell.ColumnIndex = 6 Then
                    If Val(Frg_Bag) <> 0 Then
                        If Val(.Rows(.CurrentRow.Index).Cells(4).Value) = 0 Then

                            .Rows(.CurrentRow.Index).Cells(4).Value = Format(.Rows(.CurrentRow.Index).Cells(6).Value / Val(Frg_Bag), "##########0")
                        End If
                    End If

                End If



            End With

        End If

    End Sub
    Private Sub Amount_Calculation()
        Dim vtotamt As Single

        Dim i As Integer
        Dim sno As Integer


        sno = 0
        With dgv_YarnDetails
            For i = 0 To dgv_YarnDetails.Rows.Count - 1

                sno = sno + 1

                .Rows(i).Cells(0).Value = sno

                If Val(dgv_YarnDetails.Rows(i).Cells(4).Value) <> 0 Or Val(dgv_YarnDetails.Rows(i).Cells(7).Value) <> 0 Then
                    If Trim(dgv_YarnDetails.Rows(i).Cells(8).Value) = "BAG" Then

                        vtotamt = Val(dgv_YarnDetails.Rows(i).Cells(4).Value) * Val(dgv_YarnDetails.Rows(i).Cells(9).Value)
                    ElseIf Trim(dgv_YarnDetails.Rows(i).Cells(8).Value) = "KG" Then
                        vtotamt = Val(dgv_YarnDetails.Rows(i).Cells(6).Value) * Val(dgv_YarnDetails.Rows(i).Cells(9).Value)


                    End If
                    dgv_YarnDetails.Rows(i).Cells(10).Value = Format(Val(vtotamt), "#########0.00")
                End If
            Next
        End With
        TotalYarnTaken_Calculation()

    End Sub

    Private Sub cbo_Grid_RateFor_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Grid_RateFor.KeyDown
        Dim dep_idno As Integer = 0

        vcbo_KeyDwnVal = e.KeyValue

        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Grid_RateFor, Nothing, Nothing, "", "", "", "")


        With dgv_YarnDetails

            If (e.KeyValue = 38 And cbo_Grid_RateFor.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
                .Focus()
                .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex - 1)
            End If

            If (e.KeyValue = 40 And cbo_Grid_RateFor.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
                .Focus()
                .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 1)
            End If

        End With
    End Sub

    Private Sub cbo_Grid_RateFor_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Grid_RateFor.KeyPress

        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Grid_RateFor, Nothing, "", "", "", "")

        If Asc(e.KeyChar) = 13 Then
            With dgv_YarnDetails

                .Focus()
                .Rows(.CurrentCell.RowIndex).Cells.Item(8).Value = Trim(cbo_Grid_RateFor.Text)
                .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)

            End With

        End If

    End Sub
    Private Sub cbo_Grid_RateFor_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Grid_RateFor.TextChanged
        Try
            If cbo_Grid_RateFor.Visible Then

                If IsNothing(dgv_YarnDetails.CurrentCell) Then Exit Sub
                With dgv_YarnDetails
                    If Val(cbo_Grid_RateFor.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = 8 Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_Grid_RateFor.Text)
                    End If
                End With
            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub txt_KuraiPavuBeam_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_KuraiPavuBeam.TextChanged
        'Dim TotBms As Single = 0
        'With dgv_PavuDetails
        '    For i = 0 To .RowCount - 1

        '        If Val(.Rows(i).Cells(4).Value) <> 0 Then
        '            TotBms = TotBms + 1

        '        End If
        '    Next
        'End With
        'lbl_Amount.Text = Format(Val(txt_KuraiPavuBeam.Text) + Val(TotBms) * Val(txt_Rate.Text), "###########0.00")
        TotalPavu_Calculation()
    End Sub

    Private Sub txt_DateTime_Of_Supply_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_DateTime_Of_Supply.GotFocus
        If Trim(txt_DateTime_Of_Supply.Text) = "" And New_Entry = True Then
            txt_DateTime_Of_Supply.Text = Format(Now, "hh:mm tt")
        End If
    End Sub

    Private Sub txt_DateTime_Of_Supply_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_DateTime_Of_Supply.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
    End Sub

    Private Sub txt_DateTime_Of_Supply_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_DateTime_Of_Supply.KeyPress
        If Trim(UCase(e.KeyChar)) = "D" Or Trim(UCase(e.KeyChar)) = "T" Then
            txt_DateTime_Of_Supply.Text = Format(Now, "hh:mm tt")
            e.Handled = True
            txt_DateTime_Of_Supply.SelectionStart = txt_DateTime_Of_Supply.TextLength
        End If
    End Sub

    Private Sub txt_DateTime_Of_Supply_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_DateTime_Of_Supply.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            txt_DateTime_Of_Supply.Text = Format(Now, "hh:mm tt")
            txt_DateTime_Of_Supply.SelectionStart = txt_DateTime_Of_Supply.TextLength
        End If
    End Sub

    Private Sub btn_SaveAll_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_SaveAll.Click
        Dim Pwd As String = ""

        Dim g As New Password
        g.ShowDialog()

        Pwd = Trim(Common_Procedures.Password_Input)

        If Trim(UCase(Pwd)) <> "TSSA7417" Then
            MessageBox.Show("Invalid Password?.....", "FAILED,.......", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
        End If

        SaveAll_Sts = True

        LastNo = ""
        movelast_record()

        LastNo = Val(lbl_DcNo.Text)

        movefirst_record()
        Timer1.Enabled = True

    End Sub

    Private Sub Timer1_Tick(ByVal sender As Object, ByVal e As System.EventArgs) Handles Timer1.Tick
        save_record()
        If Trim(UCase(LastNo)) = Trim(UCase(lbl_DcNo.Text)) Then
            MessageBox.Show("All Entries Saved Successfully!..", "SAVING,...", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Timer1.Enabled = False
            SaveAll_Sts = False
            Exit Sub
        Else
            movenext_record()
        End If
    End Sub

    Private Sub btn_UserModification_Click(sender As System.Object, e As System.EventArgs) Handles btn_UserModification.Click
        If Val(Common_Procedures.User.IdNo) = 1 Then
            Dim f1 As New User_Modifications
            f1.Entry_Name = Me.Name
            f1.Entry_PkValue = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)
            f1.ShowDialog()
        End If
    End Sub




    Private Sub cbo_WidthType_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_WidthType.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "", "", "", "")
    End Sub

    Private Sub cbo_WidthType_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_WidthType.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_WidthType, txt_Freight, Nothing, "", "", "", "")
        If (e.KeyValue = 40) Then

            If cbo_ClothSales_OrderCode_forSelection.Visible = True Then
                cbo_ClothSales_OrderCode_forSelection.Focus()

            Else


                If dgv_YarnDetails.Rows.Count > 0 Then



                    dgv_YarnDetails.Focus()
                    dgv_YarnDetails.CurrentCell = dgv_YarnDetails.Rows(0).Cells(1)

                Else
                    btn_save.Focus()

                End If

            End If

        End If
    End Sub

    Private Sub cbo_WidthType_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_WidthType.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_WidthType, Nothing, "", "", "", "")
        If Asc(e.KeyChar) = 13 Then


            If cbo_ClothSales_OrderCode_forSelection.Visible = True Then
                cbo_ClothSales_OrderCode_forSelection.Focus()

            Else

                If dgv_YarnDetails.Rows.Count > 0 Then
                    dgv_YarnDetails.Focus()
                    dgv_YarnDetails.CurrentCell = dgv_YarnDetails.Rows(0).Cells(1)


                Else


                    If MessageBox.Show("Do you want to save ?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = DialogResult.Yes Then
                        save_record()
                    Else
                        msk_Date.Focus()
                    End If
                End If


            End If
        End If

    End Sub
    Private Sub Printing_Format_1204(ByRef e As System.Drawing.Printing.PrintPageEventArgs)
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
        'Dim ps As Printing.PaperSize
        Dim strHeight As Single = 0
        Dim PpSzSTS As Boolean = False
        Dim W1 As Single = 0
        Dim Inc As Integer = 0

        If prn_PageSize_SetUP_STS = False Then
            set_PaperSize_For_PrintDocument1()
            prn_PageSize_SetUP_STS = True
        End If

        With PrintDocument1.DefaultPageSettings.Margins

            .Left = 50
            .Right = 10


            .Top = 10
            .Bottom = 50
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
        ' If prn_DetDt.Rows.Count = 4 Then
        If ContSts = True Then
            NoofItems_PerPage = 22
        Else
            NoofItems_PerPage = 13
        End If

        'ElseIf prn_DetDt.Rows.Count = 3 Then
        '    NoofItems_PerPage = 13
        'ElseIf prn_DetDt.Rows.Count = 2 Then

        '    NoofItems_PerPage = 15
        'ElseIf prn_DetDt.Rows.Count = 1 Then

        '    NoofItems_PerPage = 17
        'Else

        '    NoofItems_PerPage = 21


        ' End If
        ' 6

        Erase LnAr
        Erase ClArr
        LnAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        ClArr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}

        ClArr(1) = 200 : ClArr(2) = 60 : ClArr(3) = 65 : ClArr(4) = 60
        ClArr(5) = PageWidth - (LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4))
        '   ClArr(9) = PageWidth - (LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8))

        TxtHgt = e.Graphics.MeasureString("A", pFont).Height
        TxtHgt = 16 ' 18 ' 19 ' e.Graphics.MeasureString("A", pFont).Height  ' 20 16.8

        EntryCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Try

            prn_Prev_HeadIndx = prn_HeadIndx

            If prn_HdDt.Rows.Count > 0 Then

                If prn_HeadIndx <= prn_HdDt.Rows.Count - 1 Then

                    Printing_Format_1204_PageHeader(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, prn_PageNo, NoofItems_PerPage, CurY, LnAr, ClArr)

                    W1 = e.Graphics.MeasureString("No.of Beams  : ", pFont).Width

                    NoofDets = 0
                    Inc = 0

                    CurY = CurY - 10
                    ContSts = False
                    If prn_DetMxIndx > 0 Then

                        Do While prn_NoofBmDets < prn_DetMxIndx

                            If NoofDets >= NoofItems_PerPage Then

                                CurY = CurY + TxtHgt

                                Common_Procedures.Print_To_PrintDocument(e, "Continued....", PageWidth - 10, CurY, 1, 0, pFont)

                                NoofDets = NoofDets + 1

                                Printing_Format_1204_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, NoofDets, False)

                                ' prn_DetIndx = prn_DetIndx + NoofItems_PerPage

                                e.HasMorePages = True
                                ContSts = True
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

                            '  If Val(prn_DetAr(prn_DetIndx, 4)) <> 0 Then
                            If Val(prn_NoofBmDets) = 0 Then
                                Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Pavu_RecFrom_Name").ToString), LMargin + 10, CurY, 0, 0, pFont)

                            End If

                            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr(prn_DetIndx, 1)), LMargin + ClArr(1) + 10, CurY, 0, 0, pFont)
                            'If Val(prn_DetAr(prn_DetIndx, 3)) <> 0 Then
                            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr(prn_DetIndx, 5)), LMargin + ClArr(1) + ClArr(2) + 5, CurY, 0, 0, pFont)

                            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr(prn_DetIndx, 2)), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + 5, CurY, 0, 0, pFont)
                            'End If
                            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr(prn_DetIndx, 4)), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) - 10, CurY, 1, 0, pFont)

                            'If prn_DetIndx > 16 Then
                            '    prn_NoofBmDets = prn_NoofBmDets + 1
                            'End If


                            prn_NoofBmDets = prn_NoofBmDets + 1



                            NoofDets = NoofDets + 1

                            'If Trim(MilNm2) <> "" Then
                            '    CurY = CurY + TxtHgt - 5
                            '    Common_Procedures.Print_To_PrintDocument(e, Trim(MilNm2), LMargin + ClArr(1) + ClArr(2) + 10, CurY, 0, 0, pFont)
                            '    NoofDets = NoofDets + 1
                            'End If

                            'prn_DetIndx = prn_DetIndx + 1

                        Loop

                    End If

                    Printing_Format_1204_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, NoofDets, True)

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


    Private Sub Printing_Format_1204_PageHeader(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByRef PageNo As Integer, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single)
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
        Dim Cmp_PhNo As String, Cmp_TinNo As String, Cmp_CstNo As String, Cmp_GSTNo As String
        Dim vPrn_DcNo As String = ""
        Dim Entry_Date As Date = Convert.ToDateTime(prn_HdDt.Rows(0).Item("PavuYarn_Delivery_Date").ToString)
        Dim From_name As String
        PageNo = PageNo + 1

        CurY = TMargin + 10

        da2 = New SqlClient.SqlDataAdapter("select a.*, b.* from Pavu_Delivery_Details a INNER JOIN EndsCount_Head b ON a.EndsCount_IdNo = b.EndsCount_IdNo where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.PavuYarn_Delivery_Code = '" & Trim(EntryCode) & "' Order by a.sl_no", con)
        da2.Fill(dt2)

        If dt2.Rows.Count > NoofItems_PerPage Then
            Common_Procedures.Print_To_PrintDocument(e, "Page : " & Trim(Val(PageNo)), PageWidth - 10, CurY - TxtHgt, 1, 0, pFont)
        End If
        dt2.Clear()

        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(1) = CurY

        Cmp_Name = "" : Cmp_Add1 = "" : Cmp_Add2 = ""
        Cmp_PhNo = "" : Cmp_TinNo = "" : Cmp_CstNo = "" : Cmp_GSTNo = ""

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
        CurY = CurY + TxtHgt - 1
        If Entry_Date >= Common_Procedures.GST_Start_Date Then
            Common_Procedures.Print_To_PrintDocument(e, Cmp_GSTNo, LMargin + 10, CurY, 0, 0, pFont)
        Else
            Common_Procedures.Print_To_PrintDocument(e, Cmp_TinNo, LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Cmp_CstNo, PageWidth - 10, CurY, 1, 0, pFont)
        End If
        CurY = CurY + TxtHgt - 10
        p1Font = New Font("Calibri", 16, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "DELIVERY", LMargin, CurY, 2, PrintWidth, p1Font)
        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height

        'CurY = CurY + TxtHgt

        CurY = CurY + strHeight + 5 ' + 150
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(2) = CurY

        Try

            M1 = ClAr(1) + ClAr(2) + ClAr(3)
            W1 = e.Graphics.MeasureString("DC NO : ", pFont).Width
            N1 = e.Graphics.MeasureString("TO   :", pFont).Width

            CurY = CurY + TxtHgt - 10
            p1Font = New Font("Calibri", 10, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "TO  :  " & "M/s." & prn_HdDt.Rows(prn_HeadIndx).Item("Del_Name").ToString, LMargin + 10, CurY, 0, 0, p1Font)
            Common_Procedures.Print_To_PrintDocument(e, "DC.NO", LMargin + M1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + M1 + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(prn_HeadIndx).Item("PavuYarn_Delivery_No").ToString, LMargin + M1 + W1 + 30, CurY, 0, 0, p1Font)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(prn_HeadIndx).Item("Ledger_Address1").ToString, LMargin + N1 + 10, CurY, 0, 0, pFont)
            p1Font = New Font("Calibri", 14, FontStyle.Bold)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(prn_HeadIndx).Item("Ledger_Address2").ToString, LMargin + N1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "DATE", LMargin + M1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + M1 + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(prn_HeadIndx).Item("PavuYarn_Delivery_Date").ToString), "dd-MM-yyyy").ToString, LMargin + M1 + W1 + 30, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(prn_HeadIndx).Item("Ledger_Address3").ToString & " " & prn_HdDt.Rows(0).Item("Ledger_Address4").ToString, LMargin + N1 + 10, CurY, 0, 0, pFont)
            'If Trim(vPrn_PvuSetNo) <> "" Then
            '    Common_Procedures.Print_To_PrintDocument(e, "SET NO", LMargin + M1 + 10, CurY, 0, 0, pFont)
            '    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + M1 + W1 + 10, CurY, 0, 0, pFont)
            '    Common_Procedures.Print_To_PrintDocument(e, Trim(vPrn_PvuSetNo), LMargin + M1 + W1 + 30, CurY, 0, 0, pFont)
            'End If

            CurY = CurY + TxtHgt
            If Entry_Date >= Common_Procedures.GST_Start_Date Then
                If prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString <> "" Then
                    Common_Procedures.Print_To_PrintDocument(e, "GSTIN_NO  : " & prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString, LMargin + N1 + 10, CurY, 0, 0, pFont)
                End If
            Else
                If prn_HdDt.Rows(0).Item("Ledger_TinNo").ToString <> "" Then
                    Common_Procedures.Print_To_PrintDocument(e, "TIN NO : " & prn_HdDt.Rows(0).Item("Ledger_TinNo").ToString, LMargin + N1 + 10, CurY, 0, 0, pFont)
                End If
            End If



            CurY = CurY + TxtHgt
            If prn_DetDt.Rows.Count <> 0 Then


                If prn_PageNo = 1 Then

                    e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
                    LnAr(11) = CurY

                    Common_Procedures.Print_To_PrintDocument(e, "COUNT", LMargin, CurY, 2, ClAr(1) - 125, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, "TYPE", LMargin + 120, CurY, 1, ClAr(1), pFont)
                    Common_Procedures.Print_To_PrintDocument(e, "MILL", LMargin + ClAr(1), CurY, 2, ClAr(3), pFont)

                    Common_Procedures.Print_To_PrintDocument(e, "BAGS", LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, 2, ClAr(4), pFont)
                    Common_Procedures.Print_To_PrintDocument(e, "WEIGHT", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 2, ClAr(5), pFont)


                    CurY = CurY + TxtHgt
                    e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
                    LnAr(12) = CurY
                    If prn_DetDt.Rows.Count <> 0 Then


                        For I = 0 To prn_DetDt.Rows.Count - 1


                            CurY = CurY + TxtHgt

                            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetDt.Rows(I).Item("Count_Name").ToString), LMargin + 10, CurY, 0, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetDt.Rows(I).Item("yarn_Type").ToString), LMargin + 85, CurY, 0, 0, pFont)

                            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetDt.Rows(I).Item("Mill_Name").ToString), LMargin + ClAr(1) - 70, CurY, 0, 0, pFont)


                            Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(I).Item("Bags")).ToString, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) - 10, CurY, 1, 0, pFont)
                            'End If
                            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(I).Item("Weight").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 1, 0, pFont)

                        Next
                    Else
                        CurY = CurY + TxtHgt
                        CurY = CurY + TxtHgt
                        CurY = CurY + TxtHgt
                        CurY = CurY + TxtHgt
                    End If
                    CurY = CurY + TxtHgt + 10
                    e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
                    LnAr(13) = CurY





                    If Val(prn_HdDt.Rows(prn_HeadIndx).Item("Total_Bags").ToString) <> 0 Then
                        Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(prn_HeadIndx).Item("Total_Bags").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) - 10, CurY, 1, 0, pFont)
                    End If

                    If Val(prn_HdDt.Rows(prn_HeadIndx).Item("Total_weight").ToString) <> 0 Then
                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(prn_HeadIndx).Item("Total_weight").ToString), "#########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 1, 0, pFont)
                    End If

                    CurY = CurY + TxtHgt
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
                    e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
                    e.Graphics.DrawLine(Pens.Black, LMargin + 80, CurY, LMargin + 80, LnAr(11))
                    e.Graphics.DrawLine(Pens.Black, LMargin + 125, CurY, LMargin + 125, LnAr(11))
                    e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3), LnAr(11))
                    e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), LnAr(11))

                End If
            End If
            CurY = CurY + TxtHgt + 5

            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

            LnAr(3) = CurY

            CurY = CurY + TxtHgt - 10
            Common_Procedures.Print_To_PrintDocument(e, "SIZING", LMargin, CurY, 2, ClAr(1), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "ENDS", LMargin + ClAr(1), CurY, 2, ClAr(2), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "SET NO", LMargin + ClAr(1) + ClAr(2), CurY, 2, ClAr(3), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "BEAM NO", LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, 2, ClAr(4), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "METERS", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 2, ClAr(5), pFont)

            CurY = CurY + TxtHgt + 10
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(4) = CurY

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub Printing_Format_1204_PageFooter(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single, ByVal NoofDets As Integer, ByVal is_LastPage As Boolean)
        Dim p1Font As Font
        Dim Cmp_Name As String
        Dim I As Integer
        Dim From_name As String

        Try

            For I = NoofDets + 1 To NoofItems_PerPage
                CurY = CurY + TxtHgt



            Next

            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(5) = CurY

            CurY = CurY + TxtHgt - 10

            If is_LastPage = True Then



                If Val(prn_HdDt.Rows(prn_HeadIndx).Item("Total_Beam").ToString) <> 0 Then
                    Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(prn_HeadIndx).Item("Total_Beam").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) - 10, CurY, 1, 0, pFont)
                End If

                If Val(prn_HdDt.Rows(prn_HeadIndx).Item("Total_Meters").ToString) <> 0 Then
                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(prn_HeadIndx).Item("Total_Meters").ToString), "#########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 1, 0, pFont)
                End If


            End If

            CurY = CurY + TxtHgt + 10
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(6) = CurY


            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1), CurY, LMargin + ClAr(1), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2), CurY, LMargin + ClAr(1) + ClAr(2), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), LnAr(3))
            '  e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + 4, CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + 4, LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(3))
            ' e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), LnAr(3))
            '  e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), LnAr(3))
            ' e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), LnAr(3))
            ' e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9), LnAr(3))

            If Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Vehicle_No").ToString) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, "Vehicle No. : " & Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Vehicle_No").ToString), LMargin + 10, CurY, 0, 0, pFont)
            End If
            '--------------------Yarn details ------------------------------------------------------start
            CurY = CurY + TxtHgt - 5


            'e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            'LnAr(11) = CurY
            'If Prnt_sts <> True Then
            '    'CurY = CurY + TxtHgt - 10
            '    Common_Procedures.Print_To_PrintDocument(e, "COUNT", LMargin, CurY, 2, ClAr(1) - 125, pFont)
            '    Common_Procedures.Print_To_PrintDocument(e, "PARTICULARS", LMargin + ClAr(1), CurY, 1, ClAr(3), pFont)
            '    Common_Procedures.Print_To_PrintDocument(e, "BAGS", LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, 2, ClAr(4), pFont)
            '    Common_Procedures.Print_To_PrintDocument(e, "WEIGHT", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 2, ClAr(5), pFont)
            '    ' Common_Procedures.Print_To_PrintDocument(e, "METERS", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 2, ClAr(5), pFont)


            '    CurY = CurY + TxtHgt
            '    e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            '    LnAr(12) = CurY

            '    'CurY = CurY + TxtHgt - 10
            '    For I = 0 To prn_DetDt.Rows.Count - 1


            '        CurY = CurY + TxtHgt

            '        Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetDt.Rows(I).Item("Count_Name").ToString), LMargin + 10, CurY, 0, 0, pFont)
            '        'If Val(prn_DetAr(prn_DetIndx, 3)) <> 0 Then
            '        Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetDt.Rows(I).Item("Mill_Name").ToString), LMargin + ClAr(1) + 10, CurY, 1, 0, pFont)

            '        Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(I).Item("Bags")).ToString, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) - 10, CurY, 1, 0, pFont)
            '        'End If
            '        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(I).Item("Weight").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 1, 0, pFont)
            '    Next
            '    CurY = CurY + TxtHgt + 10
            '    e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            '    LnAr(13) = CurY





            '    If Val(prn_HdDt.Rows(prn_HeadIndx).Item("Total_Bags").ToString) <> 0 Then
            '        Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(prn_HeadIndx).Item("Total_Bags").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) - 10, CurY, 1, 0, pFont)
            '    End If

            '    If Val(prn_HdDt.Rows(prn_HeadIndx).Item("Total_weight").ToString) <> 0 Then
            '        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(prn_HeadIndx).Item("Total_weight").ToString), "#########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 1, 0, pFont)
            '    End If

            '    CurY = CurY + TxtHgt + 10

            '    e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            '    e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) - 125, CurY, LMargin + ClAr(1) - 125, LnAr(11))
            '    e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3), LnAr(11))
            '    e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), LnAr(11))
            '    ' e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4)+ ClAr(5), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), LnAr(11))


            '    '--------------------Yarn details ------------------------------------------------------end


            'CurY = CurY + TxtHgt - 5
            'Common_Procedures.Print_To_PrintDocument(e, "Received Beams and Yarn as per above details.", LMargin + 20, CurY, 0, 0, pFont)



            'From_name = ""
            'If prn_HdDt.Rows(prn_HeadIndx).Item("Yarn_RecFrom_Name").ToString <> "" And Val(prn_HdDt.Rows(prn_HeadIndx).Item("Total_Weight").ToString) <> 0 Then
            '    If prn_HdDt.Rows(prn_HeadIndx).Item("Pavu_RecFrom_Name").ToString <> "" And (Val(prn_HdDt.Rows(prn_HeadIndx).Item("Total_Meters").ToString <> 0) Or Val(prn_HdDt.Rows(prn_HeadIndx).Item("Meters").ToString <> 0)) Then
            '        From_name = "Rec.From (Yarn) : " & Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Yarn_RecFrom_Name").ToString)
            '    Else
            '        From_name = "Rec.From : " & Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Yarn_RecFrom_Name").ToString)
            '    End If
            'End If

            'If prn_HdDt.Rows(prn_HeadIndx).Item("Pavu_RecFrom_Name").ToString <> "" And (Val(prn_HdDt.Rows(prn_HeadIndx).Item("Total_Meters").ToString <> 0) Or Val(prn_HdDt.Rows(prn_HeadIndx).Item("Meters").ToString <> 0)) Then
            '    If prn_HdDt.Rows(prn_HeadIndx).Item("Yarn_RecFrom_Name").ToString <> "" And Val(prn_HdDt.Rows(prn_HeadIndx).Item("Total_Weight").ToString) <> 0 Then
            '        From_name = From_name & IIf(Trim(From_name) <> "", "         ", "") & "Rec.From (Pavu) : " & Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Pavu_RecFrom_Name").ToString)
            '    Else
            '        From_name = From_name & IIf(Trim(From_name) <> "", "         ", "") & "Rec.From : " & Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Pavu_RecFrom_Name").ToString)
            '    End If
            'End If

            'CurY = CurY + TxtHgt
            'Common_Procedures.Print_To_PrintDocument(e, From_name, LMargin + 20, CurY, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, "Rec.From : " & From_name, LMargin + 20, CurY, 0, 0, pFont)
            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1037" Then '---- Prakash Textiles (Somanur)
                'CurY = CurY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, "Note : " & prn_HdDt.Rows(prn_HeadIndx).Item("Note").ToString, PageWidth - 5, CurY, 1, 0, pFont)
            End If

            'Common_Procedures.Print_To_PrintDocument(e, "Rec.From : " & From_name, LMargin + 20, CurY, 0, 0, pFont)


            CurY = CurY + TxtHgt
            CurY = CurY + TxtHgt - 5
            Cmp_Name = prn_HdDt.Rows(prn_HeadIndx).Item("Company_Name").ToString

            'Common_Procedures.Print_To_PrintDocument(e, "Prepared By ", LMargin + 300, CurY, 0, 0, pFont)
            p1Font = New Font("Calibri", 12, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "For " & Cmp_Name, PageWidth - 15, CurY, 1, 0, p1Font)

            CurY = CurY + TxtHgt + 8
            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "Receiver's Signature", LMargin + 20, CurY, 0, 0, pFont)
            CurY = CurY + TxtHgt



            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            e.Graphics.DrawLine(Pens.Black, LMargin, LnAr(1), LMargin, CurY)
            e.Graphics.DrawLine(Pens.Black, PageWidth, LnAr(1), PageWidth, CurY)

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub cbo_Grid_CountName_KeyUp(sender As Object, e As KeyEventArgs) Handles cbo_Grid_CountName.KeyUp
        If e.Control = False And e.KeyValue = 17 Then
            Dim f As New Count_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Grid_CountName.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If
    End Sub


    Private Sub Printing_Format_1408(ByRef e As System.Drawing.Printing.PrintPageEventArgs)
        Dim EntryCode As String
        Dim I As Integer, J As Integer, NoofDets As Integer, NoofItems_PerPage As Integer
        Dim pFont As Font
        Dim LMargin As Single, RMargin As Single, TMargin As Single, BMargin As Single
        Dim PrintWidth As Single, PrintHeight As Single
        Dim PageWidth As Single, PageHeight As Single
        Dim CurY As Single, TxtHgt As Single
        Dim CurY1 As Single, TxtHgt1 As Single
        Dim Cmp_Name As String
        Dim LnAr(15) As Single, ClArr(15) As Single
        Dim ItmNm1 As String, ItmNm2 As String
        Dim ps As Printing.PaperSize
        Dim W1 As Single = 0
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt2 As New DataTable
        Dim NewCode As String = ""
        Dim Cmd As New SqlClient.SqlCommand
        Dim da3 As New SqlClient.SqlDataAdapter
        Dim dt3 As New DataTable
        Cmd.Connection = con
        vprn_Tot_Amt = 0
        For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
            If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
                ps = PrintDocument1.PrinterSettings.PaperSizes(I)
                PrintDocument1.DefaultPageSettings.PaperSize = ps
                Exit For
            End If
        Next

        With PrintDocument1.DefaultPageSettings.Margins
            .Left = 30 ' 30 '60
            .Right = 60
            .Top = 40 ' 60
            .Bottom = 40
            LMargin = .Left
            RMargin = .Right
            TMargin = .Top
            BMargin = .Bottom
        End With

        pFont = New Font("Calibri", 10, FontStyle.Regular)
        'pFont = New Font("Calibri", 12, FontStyle.Regular)

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

        TxtHgt = 19 ' e.Graphics.MeasureString("A", pFont).Height  ' 20


        NoofItems_PerPage = 12


        Erase LnAr
        Erase ClArr

        LnAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        ClArr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}

        ClArr(1) = Val(40) : ClArr(2) = 70 : ClArr(3) = 70 : ClArr(4) = 55 : ClArr(5) = 60 : ClArr(6) = 75 : ClArr(7) = 60 : ClArr(8) = 125 : ClArr(9) = 75
        ClArr(10) = PageWidth - (LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + ClArr(9))


        ' ClArr(2) = 230 : ClArr(3) = 85 : ClArr(4) = 50 : ClArr(5) = 80 : ClArr(6) = 55 : ClArr(7) = 80
        ' ClArr(8) = PageWidth - (LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7))

        CurY = TMargin

        Cmp_Name = Trim(prn_HdDt.Rows(0).Item("Company_Tamil_Name").ToString)

        EntryCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Try

            prn_Prev_HeadIndx = prn_HeadIndx

            If prn_HdDt.Rows.Count > 0 Then

                Printing_Format_1408_PageHeader(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, prn_PageNo, NoofItems_PerPage, CurY, LnAr, ClArr)

                W1 = e.Graphics.MeasureString("Ends Count : ", pFont).Width

                NoofDets = 0
                'DetSNo = 0
                CurY = CurY - 10
                prn_DetIndx = 0
                'prn_DetAr(prn_DetMxIndx, 1) = Trim(prn_DetDt1.Rows(I).Item("Ends_Name").ToString)
                'prn_DetAr(prn_DetMxIndx, 2) = Trim(prn_DetDt1.Rows(I).Item("Beam_No").ToString)
                'prn_DetAr(prn_DetMxIndx, 3) = Val(prn_DetDt1.Rows(I).Item("Pcs").ToString)
                'prn_DetAr(prn_DetMxIndx, 4) = Format(Val(prn_DetDt1.Rows(I).Item("Meters").ToString), "#########0.00")
                If prn_DetMxIndx > 0 Then

                    If prn_DetDt.Rows.Count > 0 Then

                        'da2 = New SqlClient.SqlDataAdapter("select b.Count_Name, c.Mill_Name, d.Item_HSN_Code, d.Item_GST_Percentage, a.Rate as YarnRate, sum(a.Bags) as YarnBag, sum(a.Weight) as YarnWeight, sum(a.Amount) as YarnAmount from Yarn_Delivery_Details a INNER JOIN Count_Head b on a.Count_IdNo = b.Count_IdNo LEFT OUTER JOIN Mill_Head c ON a.Mill_IdNo = c.Mill_IdNo LEFT OUTER JOIN itemgroup_head d ON b.itemgroup_idno = d.itemgroup_idno Where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.PavuYarn_Delivery_Code = '" & Trim(EntryCode) & "' Group by b.Count_Name, c.Mill_Name, d.Item_HSN_Code, d.Item_GST_Percentage, a.Rate Order by b.Count_Name, c.Mill_Name, d.Item_HSN_Code, d.Item_GST_Percentage, a.Rate", con)
                        da2 = New SqlClient.SqlDataAdapter("select a.Beam_No,a.meters from pavu_Delivery_Details a Where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.PavuYarn_Delivery_Code = '" & Trim(EntryCode) & "' ", con)
                        dt2 = New DataTable
                        da2.Fill(dt2)

                        da3 = New SqlClient.SqlDataAdapter("select a.*, b.Count_Name, c.Mill_Name,d.Item_HSN_Code from Yarn_Delivery_Details a INNER JOIN Count_Head b on a.Count_IdNo = b.Count_IdNo LEFT OUTER JOIN Mill_Head c ON a.Mill_IdNo = c.Mill_IdNo LEFT OUTER JOIN itemgroup_head d ON b.itemgroup_idno = d.itemgroup_idno where a.PavuYarn_Delivery_Code = '" & Trim(EntryCode) & "' Order by a.sl_no", con)
                        dt3 = New DataTable
                        da3.Fill(dt3)



                        If dt2.Rows.Count > 0 Then
                            CurY1 = CurY
                            For I = 0 To dt2.Rows.Count - 1

                                prn_DetSNo = prn_DetSNo + 1


                                CurY = CurY + TxtHgt

                                Common_Procedures.Print_To_PrintDocument(e, Trim(Val(prn_DetSNo)), LMargin + 15, CurY, 0, 0, pFont)
                                Common_Procedures.Print_To_PrintDocument(e, dt2.Rows(I).Item("Beam_No").ToString, LMargin + ClArr(1) + 10, CurY, 0, 0, pFont)
                                Common_Procedures.Print_To_PrintDocument(e, dt2.Rows(I).Item("meters").ToString, LMargin + ClArr(1) + ClArr(2) + 10, CurY, 0, 0, pFont)
                                ' Common_Procedures.Print_To_PrintDocument(e, dt2.Rows(I).Item("Item_GST_Percentage").ToString, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) - 5, CurY, 1, 0, pFont)
                                If dt3.Rows.Count > 0 Then

                                    For J = I To dt3.Rows.Count - 1
                                        Common_Procedures.Print_To_PrintDocument(e, dt3.Rows(I).Item("Count_Name").ToString, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) - 5, CurY, 1, 0, pFont)
                                        Common_Procedures.Print_To_PrintDocument(e, dt3.Rows(J).Item("Bags").ToString, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) - 10, CurY, 1, 0, pFont)
                                        Common_Procedures.Print_To_PrintDocument(e, dt3.Rows(J).Item("Weight").ToString, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) - 10, CurY, 1, 0, pFont)
                                        Common_Procedures.Print_To_PrintDocument(e, dt3.Rows(J).Item("Item_HSN_Code").ToString, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) - 10, CurY, 1, 0, pFont)

                                    Next


                                End If



                                'vprn_Tot_Bgs_Bms = Val(vprn_Tot_Bgs_Bms) + Val(dt2.Rows(I).Item("YarnBag").ToString)
                                'vprn_Tot_Wgt_Mtr = Format(Val(vprn_Tot_Wgt_Mtr) + Val(dt2.Rows(I).Item("YarnWeight").ToString), "##########0.000")

                                NoofDets = NoofDets + 1



                                prn_DetIndx = prn_DetIndx + 1

                            Next

                            CurY1 = CurY1 + TxtHgt

                            Common_Procedures.Print_To_PrintDocument(e, "T.Mtr", LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7), CurY1, 0, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Total_Meters").ToString, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) - 5, CurY1, 1, 0, pFont)

                            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Rate").ToString, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + ClArr(9) - 5, CurY1, 1, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("amount").ToString, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + ClArr(9) + ClArr(10) - 5, CurY1, 1, 0, pFont)
                            CurY1 = CurY1 + TxtHgt
                            e.Graphics.DrawLine(Pens.Black, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7), CurY1, PageWidth, CurY1)
                            CurY1 = CurY1 + TxtHgt
                            Common_Procedures.Print_To_PrintDocument(e, "T.Kgs", LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7), CurY1, 0, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Total_Weight").ToString, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) - 5, CurY1, 1, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, dt3.Rows(0).Item("Rate").ToString, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + ClArr(9) - 5, CurY1, 1, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Total_Amount").ToString, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + ClArr(9) + ClArr(10) - 5, CurY1, 1, 0, pFont)
                            CurY1 = CurY1 + TxtHgt
                            e.Graphics.DrawLine(Pens.Black, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7), CurY1, PageWidth, CurY1)
                            e.Graphics.DrawLine(Pens.Black, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + (ClArr(8) / 2) - 10, CurY1, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + +ClArr(7) + (ClArr(8) / 2) - 10, LnAr(4))
                            vprn_Tot_Amt = Format(Val(prn_HdDt.Rows(0).Item("amount").ToString) + Val(prn_HdDt.Rows(0).Item("Total_Amount").ToString), "##########0.000")


                        End If
                        dt2.Clear()
                        dt3.Clear()

                    End If


                    'If prn_DetDt1.Rows.Count > 0 Then


                    '    da2 = New SqlClient.SqlDataAdapter("select a.*, b.Count_Name, c.Mill_Name,d.Item_HSN_Code from Yarn_Delivery_Details a INNER JOIN Count_Head b on a.Count_IdNo = b.Count_IdNo LEFT OUTER JOIN Mill_Head c ON a.Mill_IdNo = c.Mill_IdNo LEFT OUTER JOIN itemgroup_head d ON b.itemgroup_idno = d.itemgroup_idno where a.PavuYarn_Delivery_Code = '" & Trim(EntryCode) & "' Order by a.sl_no", con)
                    '    dt2 = New DataTable
                    '    da2.Fill(dt2)
                    '    'da2 = New SqlClient.SqlDataAdapter("Select b.Endscount_Name, IG.Item_HSN_Code, IG.Item_GST_Percentage, sum(a.Meters1) as PavuMtrs, sum(a.Int2) as Beams from " & Trim(Common_Procedures.EntryTempTable) & " a INNER JOIN EndsCount_Head b ON a.Int1 = b.EndsCount_IdNo INNER JOIN Count_Head c ON b.Count_IdNo = c.Count_IdNo LEFT OUTER JOIN ItemGroup_Head IG ON c.ItemGroup_IdNo = IG.ItemGroup_IdNo  group by b.EndsCount_Name, IG.Item_HSN_Code, IG.Item_GST_Percentage Order by b.EndsCount_Name, IG.Item_HSN_Code, IG.Item_GST_Percentage", con)
                    '    'dt2 = New DataTable
                    '    'da2.Fill(dt2)
                    '    If dt2.Rows.Count > 0 Then
                    '        prn_DetIndx = 0
                    '        For I = 0 To dt2.Rows.Count - 1
                    '            CurY = CurY + TxtHgt
                    '            prn_DetSNo = prn_DetSNo + 1

                    '            ' Common_Procedures.Print_To_PrintDocument(e, prn_DetSNo, LMargin + 15, CurY, 0, 0, pFont)
                    '            'Common_Procedures.Print_To_PrintDocument(e, dt2.Rows(I).Item("Count_Name").ToString, LMargin + ClArr(1) + 10, CurY, 0, 0, pFont)
                    '            'Common_Procedures.Print_To_PrintDocument(e, dt2.Rows(I).Item("Bags").ToString, LMargin + ClArr(1) + ClArr(2) + 10, CurY, 0, 0, pFont)
                    '            'Common_Procedures.Print_To_PrintDocument(e, dt2.Rows(I).Item("Count_Name").ToString, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) - 5, CurY, 1, 0, pFont)


                    '            Common_Procedures.Print_To_PrintDocument(e, dt2.Rows(I).Item("Bags").ToString, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) - 10, CurY, 1, 0, pFont)
                    '            Common_Procedures.Print_To_PrintDocument(e, dt2.Rows(I).Item("Weight").ToString, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) - 10, CurY, 1, 0, pFont)
                    '            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Item_HSN_Code").ToString, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) - 10, CurY, 1, 0, pFont)
                    '            ' Common_Procedures.Print_To_PrintDocument(e, dt2.Rows(I).Item("Item_HSN_Code").ToString, PageWidth - 10, CurY, 1, 0, pFont)

                    '            'vprn_Tot_Bgs_Bms = Val(vprn_Tot_Bgs_Bms) + Val(dt2.Rows(I).Item("Beams").ToString)
                    '            'vprn_Tot_Wgt_Mtr = Format(Val(vprn_Tot_Wgt_Mtr) + Val(dt2.Rows(I).Item("PavuMtrs").ToString), "##########0.000")
                    '            'vprn_Tot_Amt = Format(Val(vprn_Tot_Amt) + Val(Format(Val(dt2.Rows(I).Item("beams").ToString) * Val(prn_HdDt.Rows(0).Item("Rate").ToString), "##########0.00")), "##########0.000")

                    '            NoofDets = NoofDets + 1

                    '        Next

                    '    End If
                    '    dt2.Clear()

                    'End If


                End If


                Printing_Format_1408_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, Cmp_Name, NoofDets, True)


            End If

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

        e.HasMorePages = False

    End Sub

    Private Sub Printing_Format_1408_PageHeader(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByRef PageNo As Integer, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single)
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt2 As New DataTable
        Dim p1Font As Font
        Dim p2Font As Font
        Dim Cmp_Name As String, Cmp_Add1 As String, Cmp_Add2 As String
        Dim Cmp_PhNo As String, Cmp_TinNo As String, Cmp_CstNo As String
        Dim Cmp_StateCap As String, Cmp_StateNm As String, Cmp_StateCode As String, Cmp_GSTIN_Cap As String, Cmp_GSTIN_No As String
        Dim strHeight As Single
        Dim Led_Name As String, Led_Add1 As String, Led_Add2 As String, Led_Add3 As String, Led_Add4 As String, Led_TinNo As String
        Dim Led_GSTTinNo As String, Led_State As String, Led_StateCd As String, LedAadhar_No As String, Led_PanNo As String
        Dim LedNmAr(10) As String
        Dim Cmp_Desc As String, Cmp_Email As String
        Dim Cen1 As Single = 0
        Dim W1 As Single = 0
        Dim W2 As Single = 0
        Dim LInc As Integer = 0
        Dim prn_OriDupTri As String = ""
        Dim S As String = ""
        Dim Led_PhNo As String
        Dim CurX As Single = 0
        Dim strWidth As Single = 0
        Dim BlockInvNoY As Single = 0
        Dim Trans_Nm As String = ""
        Dim Indx As Integer = 0
        Dim HdWd As Single = 0
        Dim H1 As Single = 0
        Dim W3 As Single = 0
        Dim C1 As Single = 0, C2 As Single = 0, C3 As Single = 0
        Dim i As Integer = 0
        Dim ItmNm1 As String, ItmNm2 As String
        Dim Ends_name As String = ""
        Dim vCloth_Reed As String = ""
        Dim vCloth_Pick As String = ""
        Dim vCloth_width As String = ""


        PageNo = PageNo + 1

        CurY = TMargin

        prn_Count = prn_Count + 1

        Ends_name = ""
        vCloth_Reed = ""
        vCloth_Pick = ""
        vCloth_width = ""

        da2 = New SqlClient.SqlDataAdapter("select top 1 a.*, b.*, tCH.* from Pavu_Delivery_Details a INNER JOIN EndsCount_Head b ON a.EndsCount_IdNo = b.EndsCount_IdNo LEFT OUTER JOIN cloth_head tCH ON tCH.EndsCount_Idno = a.EndsCount_Idno where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.PavuYarn_Delivery_Code = '" & Trim(EntryCode) & "' Order by a.Sl_No, tCH.Cloth_IdNo", con)
        da2.Fill(dt2)
        If dt2.Rows.Count > 0 Then
            'Common_Procedures.Print_To_PrintDocument(e, "Page : " & Trim(Val(PageNo)), PageWidth - 10, CurY - TxtHgt, 1, 0, pFont)
            Ends_name = dt2.Rows(0).Item("Ends_name").ToString()
            vCloth_Reed = dt2.Rows(0).Item("Cloth_Reed").ToString()
            vCloth_Pick = dt2.Rows(0).Item("Cloth_Pick").ToString()
            vCloth_width = dt2.Rows(0).Item("Cloth_width").ToString()
        End If
        dt2.Clear()

        'p1Font = New Font("Calibri", 12, FontStyle.Regular)
        'Common_Procedures.Print_To_PrintDocument(e, "INVOICE", LMargin, CurY - TxtHgt, 2, PrintWidth, p1Font)
        ' CurY = CurY + TxtHgt '+ 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(1) = CurY

        Cmp_Name = "" : Cmp_Add1 = "" : Cmp_Add2 = ""
        Cmp_PhNo = "" : Cmp_TinNo = "" : Cmp_CstNo = ""
        Cmp_Desc = "" : Cmp_Email = ""
        Cmp_StateCap = "" : Cmp_StateNm = "" : Cmp_StateCode = "" : Cmp_GSTIN_Cap = "" : Cmp_GSTIN_No = ""

        Cmp_Name = Trim(prn_HdDt.Rows(0).Item("Company_Tamil_Name").ToString)

        Cmp_Add1 = Trim(prn_HdDt.Rows(0).Item("Company_Tamil_Address1").ToString)
        'If Trim(Cmp_Add1) <> "" Then
        '    If Microsoft.VisualBasic.Right(Trim(Cmp_Add1), 1) = "," Then
        '        Cmp_Add1 = Trim(Cmp_Add1) & ", " & Trim(prn_HdDt.Rows(0).Item("Company_Tamil_Address2").ToString)
        '    Else
        '        Cmp_Add1 = Trim(Cmp_Add1) & " " & Trim(prn_HdDt.Rows(0).Item("Company_Tamil_Address2").ToString)
        '    End If
        'Else
        '    Cmp_Add1 = Trim(prn_HdDt.Rows(0).Item("Company_Tamil_Address1").ToString)
        'End If
        Cmp_Add2 = Trim(prn_HdDt.Rows(0).Item("Company_Tamil_Address2").ToString)
        'Cmp_Add2 = Trim(prn_HdDt.Rows(0).Item("Company_Tamil_Address3").ToString)
        'If Trim(Cmp_Add2) <> "" Then
        '    If Microsoft.VisualBasic.Right(Trim(Cmp_Add2), 1) = "," Then
        '        Cmp_Add2 = Trim(Cmp_Add2) & ", " & Trim(prn_HdDt.Rows(0).Item("Company_Tamil_Address4").ToString)
        '    Else
        '        Cmp_Add2 = Trim(Cmp_Add2) & " " & Trim(prn_HdDt.Rows(0).Item("Company_Tamil_Address4").ToString)
        '    End If
        'Else
        '    Cmp_Add2 = Trim(prn_HdDt.Rows(0).Item("Company_Tamil_Address4").ToString)
        'End If

        If Trim(prn_HdDt.Rows(0).Item("Company_PhoneNo").ToString) <> "" Then
            Cmp_PhNo = "PHONE : " & Trim(prn_HdDt.Rows(0).Item("Company_PhoneNo").ToString)
        End If

        If Trim(prn_HdDt.Rows(0).Item("Company_TinNo").ToString) <> "" Then
            Cmp_TinNo = "TIN NO. : " & Trim(prn_HdDt.Rows(0).Item("Company_TinNo").ToString)
        End If
        If Trim(prn_HdDt.Rows(0).Item("Company_CstNo").ToString) <> "" Then
            Cmp_CstNo = "CST NO. : " & Trim(prn_HdDt.Rows(0).Item("Company_CstNo").ToString)
        End If
        If Trim(prn_HdDt.Rows(0).Item("Company_Description").ToString) <> "" Then
            Cmp_Desc = "(" & Trim(prn_HdDt.Rows(0).Item("Company_Description").ToString) & ")"
        End If
        If Trim(prn_HdDt.Rows(0).Item("Company_EMail").ToString) <> "" Then
            Cmp_Email = "E-mail : " & Trim(prn_HdDt.Rows(0).Item("Company_EMail").ToString)
        End If

        '***** GST START *****
        If Trim(prn_HdDt.Rows(0).Item("Company_State_Name").ToString) <> "" Then
            Cmp_StateCap = "STATE : "
            Cmp_StateNm = prn_HdDt.Rows(0).Item("Company_State_Name").ToString
        End If
        If Trim(prn_HdDt.Rows(0).Item("Company_State_Code").ToString) <> "" Then
            Cmp_StateCode = "CODE :" & prn_HdDt.Rows(0).Item("Company_State_Code").ToString
        End If
        If Trim(prn_HdDt.Rows(0).Item("Company_GSTinNo").ToString) <> "" Then
            Cmp_GSTIN_Cap = "GSTIN : "
            Cmp_GSTIN_No = prn_HdDt.Rows(0).Item("Company_GSTinNo").ToString
        End If
        '***** GST END *****

        CurY = CurY + TxtHgt - 10

        p1Font = New Font("Calibri", 18, FontStyle.Bold)
        p2Font = New Font("SaiIndira", 18, FontStyle.Bold)
        If Trim(Common_Procedures.settings.CustomerCode) = "1154" Then
            e.Graphics.DrawString(Cmp_Name, p1Font, Brushes.Green, 266, CurY)
            'Common_Procedures.Print_To_PrintDocument(e, Cmp_Name, LMargin, CurY, 2, PrintWidth, p1Font)
        Else
            Common_Procedures.Print_To_PrintDocument(e, Cmp_Name, LMargin, CurY, 2, PrintWidth, p2Font)
        End If
        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height



        ItmNm1 = Trim(prn_HdDt.Rows(0).Item("Company_Description").ToString)
        ItmNm2 = ""
        If Trim(ItmNm1) <> "" Then
            ItmNm1 = "(" & Trim(ItmNm1) & ")"
            If Len(ItmNm1) > 85 Then
                For i = 85 To 1 Step -1
                    If Mid$(Trim(ItmNm1), i, 1) = " " Or Mid$(Trim(ItmNm1), i, 1) = "," Or Mid$(Trim(ItmNm1), i, 1) = "." Or Mid$(Trim(ItmNm1), i, 1) = "-" Or Mid$(Trim(ItmNm1), i, 1) = "/" Or Mid$(Trim(ItmNm1), i, 1) = "_" Or Mid$(Trim(ItmNm1), i, 1) = "(" Or Mid$(Trim(ItmNm1), i, 1) = ")" Or Mid$(Trim(ItmNm1), i, 1) = "\" Or Mid$(Trim(ItmNm1), i, 1) = "[" Or Mid$(Trim(ItmNm1), i, 1) = "]" Or Mid$(Trim(ItmNm1), i, 1) = "{" Or Mid$(Trim(ItmNm1), i, 1) = "}" Then Exit For
                Next i
                If i = 0 Then i = 85
                ItmNm2 = Microsoft.VisualBasic.Right(Trim(ItmNm1), Len(ItmNm1) - i)
                ItmNm1 = Microsoft.VisualBasic.Left(Trim(ItmNm1), i - 1)
            End If
        End If

        If Trim(ItmNm1) <> "" Then
            CurY = CurY + strHeight - 1
            p1Font = New Font("Calibri", 10, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm1), LMargin, CurY, 2, PrintWidth, p1Font)
            strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height
        End If

        If Trim(ItmNm2) <> "" Then
            CurY = CurY + strHeight - 1
            p1Font = New Font("Calibri", 10, FontStyle.Bold)

            Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm2), LMargin, CurY, 2, PrintWidth, p1Font)
            strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height

        End If
        p2Font = New Font("SaiIndira", 12, FontStyle.Regular)
        CurY = CurY + strHeight
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Add1, LMargin, CurY, 2, PrintWidth, p2Font)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Add2, LMargin, CurY, 2, PrintWidth, p2Font)

        '***** GST START *****
        CurY = CurY + TxtHgt

        p1Font = New Font("Calibri", 11, FontStyle.Bold)
        strWidth = e.Graphics.MeasureString(Trim(Cmp_StateCap & Cmp_GSTIN_Cap), p1Font).Width
        strWidth = e.Graphics.MeasureString(Trim(Cmp_StateCap & Cmp_StateNm & "     " & Cmp_GSTIN_Cap & Cmp_GSTIN_No), pFont).Width
        If PrintWidth > strWidth Then
            CurX = LMargin + (PrintWidth - strWidth) / 2
        Else
            CurX = LMargin
        End If
        pFont = New Font("Calibri", 11, FontStyle.Regular)
        p1Font = New Font("Calibri", 11, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, Cmp_StateCap, CurX, CurY, 0, 0, p1Font)
        strWidth = e.Graphics.MeasureString(Cmp_StateCap, p1Font).Width
        CurX = CurX + strWidth
        Common_Procedures.Print_To_PrintDocument(e, Cmp_StateNm, CurX, CurY, 0, 0, pFont)

        strWidth = e.Graphics.MeasureString(Cmp_StateNm, pFont).Width
        p1Font = New Font("Calibri", 11, FontStyle.Bold)
        CurX = CurX + strWidth
        Common_Procedures.Print_To_PrintDocument(e, "     " & Cmp_GSTIN_Cap, CurX, CurY, 0, PrintWidth, p1Font)
        strWidth = e.Graphics.MeasureString("     " & Cmp_GSTIN_Cap, p1Font).Width
        CurX = CurX + strWidth
        Common_Procedures.Print_To_PrintDocument(e, Cmp_GSTIN_No, CurX, CurY, 0, 0, pFont)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, Trim(Cmp_PhNo & "   " & Cmp_Email), LMargin, CurY, 2, PrintWidth, pFont)
        pFont = New Font("Calibri", 10, FontStyle.Regular)
        '***** GST END *****
        CurY = CurY + TxtHgt
        p1Font = New Font("SaiIndira", 16, FontStyle.Bold)

        Common_Procedures.Print_To_PrintDocument(e, "¦¼Ä¢Å¡¢ §¿¡ð", LMargin, CurY, 2, PrintWidth, p1Font)


        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height
        CurY = CurY + TxtHgt + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(2) = CurY

        Try

            Led_Name = "" : Led_Add1 = "" : Led_Add2 = "" : Led_Add3 = "" : Led_Add4 = "" : Led_TinNo = "" : Led_PhNo = "" : Led_GSTTinNo = "" : Led_State = ""

            Led_StateCd = ""
            LedAadhar_No = "" : Led_PanNo = ""
            Led_Name = Trim(prn_HdDt.Rows(0).Item("DelTamil_Name").ToString)

            Led_Add1 = Trim(prn_HdDt.Rows(0).Item("Ledger_Tamil_Address1").ToString)
            Led_Add2 = Trim(prn_HdDt.Rows(0).Item("Ledger_Tamil_Address2").ToString)
            'Led_Add3 = Trim(prn_HdDt.Rows(0).Item("Ledger_Address3").ToString) & " " & Trim(prn_HdDt.Rows(0).Item("Ledger_Address4").ToString)
            'Led_Add4 = ""  'Trim(prn_HdDt.Rows(0).Item("Ledger_Address4").ToString)
            '***** GST START *****
            Led_TinNo = "Tin No : " & Trim(prn_HdDt.Rows(0).Item("Ledger_TinNo").ToString)
            If Trim(prn_HdDt.Rows(0).Item("Ledger_PhoneNo").ToString) <> "" Then Led_PhNo = "Phone No : " & Trim(prn_HdDt.Rows(0).Item("Ledger_PhoneNo").ToString)

            Led_State = "State : " & Trim(prn_HdDt.Rows(0).Item("Ledger_State_Name").ToString) & "  Code  :" & Trim(prn_HdDt.Rows(0).Item("Ledger_State_Code").ToString)
            Led_StateCd = Trim(prn_HdDt.Rows(0).Item("Ledger_State_Code").ToString)
            If Trim(prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString) <> "" Then Led_GSTTinNo = " GSTIN : " & Trim(prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString)
            If Trim(prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString) = "" Then
                If Trim(prn_HdDt.Rows(0).Item("Pan_No").ToString) <> "" Then Led_GSTTinNo = "Pan No : " & Trim(prn_HdDt.Rows(0).Item("Pan_No").ToString)
                If Trim(prn_HdDt.Rows(0).Item("Aadhar_No").ToString) <> "" Then LedAadhar_No = "AAdhar No : " & Trim(prn_HdDt.Rows(0).Item("Aadhar_No").ToString)
            End If
            '***** GST END *****



            Erase LedNmAr
            LedNmAr = New String(10) {}
            LInc = 0

            LInc = LInc + 1
            LedNmAr(LInc) = Led_Name

            If Trim(Led_Add1) <> "" Then
                LInc = LInc + 1
                LedNmAr(LInc) = Led_Add1
            End If

            If Trim(Led_Add2) <> "" Then
                LInc = LInc + 1
                LedNmAr(LInc) = Led_Add2
            End If

            If Trim(Led_Add3) <> "" Then
                LInc = LInc + 1
                LedNmAr(LInc) = Led_Add3
            End If

            'If Trim(Led_Add4) <> "" Then
            '    LInc = LInc + 1
            '    LedNmAr(LInc) = Led_Add4
            'End If
            '***** GST START *****
            If Trim(Led_State) <> "" Then
                LInc = LInc + 1
                LedNmAr(LInc) = Led_State
            End If

            If Trim(Led_PhNo) <> "" Then
                LInc = LInc + 1
                LedNmAr(LInc) = Led_PhNo
            End If

            If Trim(Led_GSTTinNo) <> "" Then
                LInc = LInc + 1
                LedNmAr(LInc) = Led_GSTTinNo
            ElseIf Trim(Led_GSTTinNo) = "" Then
                LInc = LInc + 1
                LedNmAr(LInc) = LedAadhar_No
            End If
            'If Trim(LedAadhar_No) <> "" Then

            'End If
            '***** GST END *****

            Cen1 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6)
            W1 = e.Graphics.MeasureString("Date & Time of Supply : ", pFont).Width
            W2 = e.Graphics.MeasureString("TO :", pFont).Width

            CurY = CurY + TxtHgt
            BlockInvNoY = CurY

            '***** GST START *****
            p1Font = New Font("Calibri", 11, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "TO :    ", LMargin + 10, CurY, 0, 0, p1Font)
            p1Font = New Font("SaiIndira", 16, FontStyle.Bold)
            '"M/s. " & 

            Common_Procedures.Print_To_PrintDocument(e, Trim(Led_Name), LMargin + W2 + 10, CurY + 5, 0, 0, p1Font)
            p1Font = New Font("Calibri", 11, FontStyle.Bold)
            CurY = CurY + TxtHgt + 10
            p2Font = New Font("SaiIndira", 11, FontStyle.Regular)
            Common_Procedures.Print_To_PrintDocument(e, Trim(LedNmAr(2)), LMargin + W2 + 10, CurY, 0, 0, p2Font)

            CurY = CurY + TxtHgt + 5
            Common_Procedures.Print_To_PrintDocument(e, Trim(LedNmAr(3)), LMargin + W2 + 10, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, Trim(LedNmAr(4)), LMargin + W2 + 10, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, Trim(LedNmAr(5)), LMargin + W2 + 10, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, Trim(LedNmAr(6)), LMargin + W2 + 10, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, Trim(LedNmAr(7)), LMargin + W2 + 10, CurY, 0, 0, pFont)
            'CurY = CurY + TxtHgt
            'Common_Procedures.Print_To_PrintDocument(e, Trim(LedNmAr(8)), LMargin + W2 + 10, CurY, 0, 0, pFont)
            '***** GST END *****


            '------------------- Invoice No Block

            '***** GST START *****

            p1Font = New Font("SaiIndira", 12, FontStyle.Regular)
            Common_Procedures.Print_To_PrintDocument(e, "¦¿ . ", LMargin + Cen1 + 10, BlockInvNoY, 0, 0, p1Font)
            p1Font = New Font("Calibri", 14, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + Cen1 + W1 + 10, BlockInvNoY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("PavuYarn_Delivery_No").ToString, LMargin + Cen1 + W1 + 30, BlockInvNoY, 0, 0, p1Font)

            BlockInvNoY = BlockInvNoY + TxtHgt
            p1Font = New Font("SaiIndira", 12, FontStyle.Regular)
            Common_Procedures.Print_To_PrintDocument(e, "§¾¾¢", LMargin + Cen1 + 10, BlockInvNoY + 4, 0, 0, p1Font)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + Cen1 + W1 + 10, BlockInvNoY + 4, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("PavuYarn_Delivery_Date").ToString), "dd-MM-yyyy"), LMargin + Cen1 + W1 + 30, BlockInvNoY + 4, 0, 0, pFont)


            BlockInvNoY = BlockInvNoY + TxtHgt



            BlockInvNoY = BlockInvNoY + TxtHgt

            p1Font = New Font("SaiIndira", 10, FontStyle.Regular)
            Common_Procedures.Print_To_PrintDocument(e, "இ¨Æ", LMargin + Cen1 + 10, BlockInvNoY, 0, 0, p1Font)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + Cen1 + W1 + 10, BlockInvNoY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Trim(Ends_name), LMargin + Cen1 + W1 + 30, BlockInvNoY, 0, 0, pFont)



            BlockInvNoY = BlockInvNoY + TxtHgt
            If Trim(vCloth_Reed) <> "" Then
                p1Font = New Font("SaiIndira", 10, FontStyle.Regular)
                Common_Procedures.Print_To_PrintDocument(e, "¡£Î", LMargin + Cen1 + 10, BlockInvNoY, 0, 0, p1Font)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + Cen1 + W1 + 10, BlockInvNoY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, vCloth_Reed, LMargin + Cen1 + W1 + 30, BlockInvNoY, 0, 0, pFont)
            End If

            BlockInvNoY = BlockInvNoY + TxtHgt
            If Trim(vCloth_Pick) <> "" Then
                p1Font = New Font("SaiIndira", 10, FontStyle.Regular)
                Common_Procedures.Print_To_PrintDocument(e, "À¢ì", LMargin + Cen1 + 10, BlockInvNoY, 0, 0, p1Font)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + Cen1 + W1 + 10, BlockInvNoY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, vCloth_Pick, LMargin + Cen1 + W1 + 30, BlockInvNoY, 0, 0, pFont)
            End If
            BlockInvNoY = BlockInvNoY + TxtHgt
            If Trim(vCloth_width) <> "" Then
                p1Font = New Font("SaiIndira", 10, FontStyle.Regular)
                Common_Procedures.Print_To_PrintDocument(e, "«¸Äõ", LMargin + Cen1 + 10, BlockInvNoY, 0, 0, p1Font)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + Cen1 + W1 + 10, BlockInvNoY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, vCloth_width, LMargin + Cen1 + W1 + 30, BlockInvNoY, 0, 0, pFont)
            End If

            CurY = CurY + TxtHgt + 10
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(3) = CurY

            e.Graphics.DrawLine(Pens.Black, LMargin + Cen1, LnAr(3), LMargin + Cen1, LnAr(2))



            CurY = CurY + TxtHgt - 10
            p1Font = New Font("SaiIndira", 10, FontStyle.Regular)
            Common_Procedures.Print_To_PrintDocument(e, "Å.±ñ", LMargin, CurY, 2, ClAr(1), p1Font)
            Common_Procedures.Print_To_PrintDocument(e, "À£õ ¦¿.", LMargin + ClAr(1), CurY, 2, ClAr(2), p1Font)
            Common_Procedures.Print_To_PrintDocument(e, "Á£ð¼÷", LMargin + ClAr(1) + ClAr(2), CurY, 2, ClAr(3), p1Font)
            Common_Procedures.Print_To_PrintDocument(e, "¸×ñð", LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, 2, ClAr(4), p1Font)

            Common_Procedures.Print_To_PrintDocument(e, "¨À ", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 2, ClAr(5), p1Font)
            Common_Procedures.Print_To_PrintDocument(e, "WEIGHT", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, 2, ClAr(6), pFont)

            Common_Procedures.Print_To_PrintDocument(e, "HSN", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, 2, ClAr(7), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "TOTAL", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, 2, ClAr(8), pFont)

            Common_Procedures.Print_To_PrintDocument(e, "RATE", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, 2, ClAr(9), pFont)

            Common_Procedures.Print_To_PrintDocument(e, "AMOUNT", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9), CurY, 2, ClAr(10), pFont)
            CurY = CurY + 20
            Common_Procedures.Print_To_PrintDocument(e, "(Kgs)", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, 2, ClAr(6), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "CODE", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, 2, ClAr(7), pFont)
            CurY = CurY + TxtHgt + 5
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(4) = CurY

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub Printing_Format_1408_PageFooter(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single, ByVal Cmp_Name As String, ByVal NoofDets As Integer, ByVal is_LastPage As Boolean)
        Dim p1Font As Font
        Dim Rup1 As String, Rup2 As String
        Dim I As Integer
        Dim vTaxPerc As Single = 0
        Dim BnkDetAr() As String
        Dim BankNm1 As String = ""
        Dim BankNm2 As String = ""
        Dim BankNm3 As String = ""
        Dim BankNm4 As String = ""
        Dim BInc As Integer
        Dim w1 As Single = 0
        Dim w2 As Single = 0
        Dim Jurs As String = ""
        Dim vNoofHsnCodes As Integer = 0
        Dim Yax As Single
        Dim LedIdNo As Integer = 0
        Dim ItmIdNo As Integer = 0
        Dim InterStateStatus As Boolean = False
        Dim Rnd_off As Single = 0
        Dim NetAmt As Single = 0
        Dim NtAmt As Single = 0
        Dim CgstAmt As Single = 0
        Dim IgstAmt As Single = 0
        Dim SgstAmt As Single = 0
        LedIdNo = 0
        InterStateStatus = False
        w1 = e.Graphics.MeasureString("Ends Count : ", pFont).Width

        LedIdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_DelvAt.Text)
        InterStateStatus = Common_Procedures.Is_InterState_Party(con, Val(lbl_Company.Tag), LedIdNo)
        Try

            For I = NoofDets + 1 To NoofItems_PerPage
                CurY = CurY + TxtHgt
                prn_DetIndx = prn_DetIndx + 1
            Next

            CurY = CurY + TxtHgt

            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

            CurY = CurY + TxtHgt - 10
            If is_LastPage = True Then
                Common_Procedures.Print_To_PrintDocument(e, "TOTAL", LMargin + ClAr(1) + ClAr(2) - 10, CurY, 1, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Total_Meters").ToString, LMargin + ClAr(1) + ClAr(2) + ClAr(3) - 5, CurY, 1, 0, pFont)

            End If
            CurY = CurY + TxtHgt + 5

            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, PageWidth, CurY)
            LnAr(5) = CurY

            'e.Graphics.DrawLine(Pens.Black, LMargin, LnAr(5), LMargin, LnAr(3))
            'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1), LnAr(5), LMargin + ClAr(1), LnAr(3))
            'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2), LnAr(5), LMargin + ClAr(1) + ClAr(2), LnAr(3))
            'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3), LnAr(5), LMargin + ClAr(1) + ClAr(2) + ClAr(3), LnAr(3))
            'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), LnAr(5), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), LnAr(3))
            'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(5), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(3))
            'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), LnAr(5), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), LnAr(5), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), LnAr(5), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9), LnAr(5), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9), LnAr(3))




            CurY = CurY + TxtHgt - 10
            If is_LastPage = True Then
                p1Font = New Font("Calibri", 10, FontStyle.Bold)
                Common_Procedures.Print_To_PrintDocument(e, "Taxable Value", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 10, CurY, 1, 0, p1Font)
                Common_Procedures.Print_To_PrintDocument(e, Format(Val(vprn_Tot_Amt), "##########0.00"), PageWidth - 5, CurY, 1, 0, p1Font)
            End If




            ''vTaxPerc = get_GST_Tax_Percentage_For_Printing(EntryCode)
            CgstAmt = Format(Val(vprn_Tot_Amt) * 2.5 / 100, "########0.00")
            SgstAmt = Format(Val(vprn_Tot_Amt) * 2.5 / 100, "########0.00")



            If InterStateStatus = True Then
                CurY = CurY + TxtHgt
                If is_LastPage = True Then
                    IgstAmt = Format(Val(vprn_Tot_Amt) * 5 / 100, "########0.00")
                    Common_Procedures.Print_To_PrintDocument(e, "IGST @ 5 %", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 10, CurY, 1, 0, pFont)

                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Amount").ToString) * Val(5) / 100, "########0.00"), PageWidth - 5, CurY, 1, 0, pFont)
                End If

            Else
                CurY = CurY + TxtHgt

                If is_LastPage = True Then

                    Common_Procedures.Print_To_PrintDocument(e, "CGST @ 2.5 %", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 10, CurY, 1, 0, pFont)

                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(vprn_Tot_Amt) * 2.5 / 100, "########0.00"), PageWidth - 5, CurY, 1, 0, pFont)
                End If



                CurY = CurY + TxtHgt
                If is_LastPage = True Then

                    Common_Procedures.Print_To_PrintDocument(e, "SGST @ 2.5 %", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 10, CurY, 1, 0, pFont)

                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(vprn_Tot_Amt) * 2.5 / 100, "########0.00"), PageWidth - 5, CurY, 1, 0, pFont)
                End If

            End If

            NetAmt = Val(vprn_Tot_Amt) + Val(SgstAmt) + Val(CgstAmt) + Val(IgstAmt)
            NtAmt = Format(Val(NetAmt), "#########0")
            NtAmt = Common_Procedures.Currency_Format(Val(NtAmt))

            Rnd_off = Format(Val(CSng(NtAmt)) - Val(NetAmt), "#########0.00")

            If Val(Rnd_off) <> 0 Then
                CurY = CurY + TxtHgt
                If is_LastPage = True Then
                    Common_Procedures.Print_To_PrintDocument(e, "Round Off", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(Rnd_off), "########0.00"), PageWidth - 5, CurY, 1, 0, pFont)
                End If
            End If

            CurY = CurY + TxtHgt + 5
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, PageWidth, CurY)


            CurY = CurY + TxtHgt - 10

            p1Font = New Font("Calibri", 15, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "Net Amount", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 10, CurY, 1, 0, p1Font)
            If is_LastPage = True Then
                p1Font = New Font("Calibri", 12, FontStyle.Bold)
                Common_Procedures.Print_To_PrintDocument(e, Common_Procedures.Currency_Format(Val(NtAmt)), PageWidth - 5, CurY, 1, 0, p1Font)
            End If

            CurY = CurY + TxtHgt + 10
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(6) = CurY
            e.Graphics.DrawLine(Pens.Black, LMargin, LnAr(6), LMargin, LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1), LnAr(6), LMargin + ClAr(1), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2), LnAr(6), LMargin + ClAr(1) + ClAr(2), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3), LnAr(6), LMargin + ClAr(1) + ClAr(2) + ClAr(3), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), LnAr(6), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(6), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), LnAr(6), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), LnAr(6), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), LnAr(3))


            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), LnAr(6), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), LnAr(5))

            Rup1 = ""
            Rup2 = ""
            If is_LastPage = True Then
                Rup1 = Common_Procedures.Rupees_Converstion(Val(NtAmt))
                If Len(Rup1) > 80 Then
                    For I = 80 To 1 Step -1
                        If Mid$(Trim(Rup1), I, 1) = " " Then Exit For
                    Next I
                    If I = 0 Then I = 80
                    Rup2 = Microsoft.VisualBasic.Right(Trim(Rup1), Len(Rup1) - I)
                    Rup1 = Microsoft.VisualBasic.Left(Trim(Rup1), I - 1)
                End If
            End If

            '***** GST START *****
            CurY = CurY + TxtHgt - 12
            p1Font = New Font("Calibri", 10, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "Amount Chargeable (In Words)  : " & Rup1, LMargin + 10, CurY, 0, 0, p1Font)
            If Trim(Rup2) <> "" Then
                CurY = CurY + TxtHgt - 5
                Common_Procedures.Print_To_PrintDocument(e, "                                " & Rup2, LMargin + 10, CurY, 0, 0, p1Font)
            End If
            '***** GST END *****

            CurY = CurY + TxtHgt + 5
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

            CurY = CurY + 10
            p1Font = New Font("SaiIndira", 12, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, " §Áü¸ñ¼ ¨ºº¢í À£õ¸û  " & Val(prn_HdDt.Rows(0).Item("Total_Beam").ToString) & "  ìÌ ¦Á¡ò¾ Á£ð¼÷  " & Val(prn_HdDt.Rows(0).Item("Total_Meters").ToString) & "   °¨¼ áø  " & Val(prn_HdDt.Rows(0).Item("Total_Bags").ToString) & " ¨À Ð½¢  ", LMargin + 10, CurY, 0, 0, p1Font)
            CurY = CurY + TxtHgt + 5
            Common_Procedures.Print_To_PrintDocument(e, "¦¿º× ¦ºöÐ ¦¸¡ÎôÀ¾ü¸¡¸ ¦ÀüÚì¦¸¡ñ§¼¡õ . ", LMargin + 10, CurY, 0, 0, p1Font)
            CurY = CurY + TxtHgt
            e.Graphics.DrawImage(DirectCast(Global.Textile.My.Resources.Resources.Amirtham_1408_Description_footer, Drawing.Image), LMargin + 30, CurY + 5, 700, 60)
            CurY = CurY + TxtHgt + TxtHgt + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(10) = CurY

            'vNoofHsnCodes = get_GST_Noof_HSN_Codes_For_Printing(EntryCode)
            'If vNoofHsnCodes <> 0 Then
            'Printing_GST_HSN_Details_Format1(e, EntryCode, TxtHgt, pFont, CurY, LMargin, PageWidth, PrintWidth, LnAr(10))
            'End If

            CurY = CurY + TxtHgt - 5

            p1Font = New Font("Calibri", 12, FontStyle.Bold)
            p1Font = New Font("SaiIndira", 12, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "For  " & Cmp_Name, PageWidth - 10, CurY, 1, 0, p1Font)



            CurY = CurY + TxtHgt
            CurY = CurY + TxtHgt
            CurY = CurY + TxtHgt
            p1Font = New Font("SaiIndira", 10, FontStyle.Regular)
            Common_Procedures.Print_To_PrintDocument(e, "ÅñÊ ¦¿õÀ÷   :   " & prn_HdDt.Rows(0).Item("Vehicle_No").ToString, LMargin + 15, CurY, 0, 0, p1Font)
            Common_Procedures.Print_To_PrintDocument(e, "¦ÀÚÀÅ÷", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 1, 0, p1Font)
            Common_Procedures.Print_To_PrintDocument(e, "Authorised Signatory", PageWidth - 10, CurY, 1, 0, pFont)



            CurY = CurY + TxtHgt + 10

            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            e.Graphics.DrawLine(Pens.Black, LMargin, LnAr(1), LMargin, CurY)
            e.Graphics.DrawLine(Pens.Black, PageWidth, LnAr(1), PageWidth, CurY)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub btn_EWayBIll_Generation_Click(sender As Object, e As EventArgs) Handles btn_EWayBIll_Generation.Click

        btn_GENERATEEWB.Enabled = True
        Grp_EWB.Visible = True
        Grp_EWB.BringToFront()

        Grp_EWB.Location = New Point(200, 227)

    End Sub

    Private Sub txt_EWBNo_TextChanged(sender As Object, e As EventArgs) Handles txt_EWBNo.TextChanged
        txt_Eway_Bill_No.Text = txt_EWBNo.Text
    End Sub

    Private Sub btn_Close_EWB_Click(sender As Object, e As EventArgs) Handles btn_Close_EWB.Click
        Grp_EWB.Visible = False
    End Sub

    Private Sub btn_CancelEWB_1_Click(sender As Object, e As EventArgs) Handles btn_CancelEWB_1.Click
        Dim NewCode As String = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)
        Dim c As Integer = MsgBox("Are You Sure To Cancel This EWB ? ", vbYesNo)

        If c = vbNo Then Exit Sub

        con = New SqlClient.SqlConnection(Common_Procedures.Connection_String)
        con.Open()

        Dim ewb As New EWB(Val(lbl_Company.Tag))

        EWB.CancelEWB(txt_Eway_Bill_No.Text, NewCode, con, rtbEWBResponse, txt_EWBNo, "PavuYarn_Delivery_Head", "Eway_BillNo", "PavuYarn_Delivery_Code")

    End Sub

    Private Sub btn_PRINTEWB_Click(sender As Object, e As EventArgs) Handles btn_PRINTEWB.Click

        Dim ewb As New EWB(Val(lbl_Company.Tag))

        EWB.PrintEWB(txt_Eway_Bill_No.Text, rtbEWBResponse, 0)

    End Sub

    Private Sub btn_CheckConnectivity_Click(sender As Object, e As EventArgs) Handles btn_CheckConnectivity.Click
        con = New SqlClient.SqlConnection(Common_Procedures.Connection_String)
        con.Open()

        'Dim einv As New eInvoice(Val(lbl_Company.Tag))
        'einv.GetAuthToken(rtbEWBResponse)

        Dim ewb As New EWB(Val(lbl_Company.Tag))
        EWB.GetAuthToken(rtbEWBResponse)

    End Sub

    Private Sub btn_GENERATEEWB_Click(sender As Object, e As EventArgs) Handles btn_GENERATEEWB.Click

        Dim dt1 As New DataTable
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim dt2 As New DataTable

        con = New SqlClient.SqlConnection(Common_Procedures.Connection_String)
        con.Open()

        '   Dim NewCode As String = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)
        Dim NewCode As String = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        If Val(lbl_Total_Value.Text) = 0 Then
            MessageBox.Show("Invalid Rate", "DOES NOT GENERATE EWB...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If txt_Rate.Enabled And txt_Rate.Visible Then txt_Rate.Focus()
            Exit Sub
        End If


        Dim da As New SqlClient.SqlDataAdapter("Select Eway_BillNo from PavuYarn_Delivery_Head where PavuYarn_Delivery_Code = '" & NewCode & "'", con)
        Dim dt As New DataTable

        da.Fill(dt)

        If dt.Rows.Count = 0 Then
            MessageBox.Show("Please Save the Invoice before proceeding to generate EWB", "Please SAVE", MessageBoxButtons.OKCancel)
            dt.Clear()
            Exit Sub
        End If

        If Not IsDBNull(dt.Rows(0).Item(0)) Then
            If Len(Trim(dt.Rows(0).Item(0))) > 0 Then
                MessageBox.Show("EWB has been generated for this invoice already", "Redundant Request", MessageBoxButtons.OKCancel)
                dt.Clear()
                Exit Sub
            End If
        End If

        dt.Clear()

        Dim CMD As New SqlClient.SqlCommand
        CMD.Connection = con

        'Dim vSgst As String = 0
        'Dim vCgst As String = 0
        'Dim vIgst As String = 0

        'vSgst = ("a.TotalInvValue" * 5)

        'vSgst = vCgst

        'vIgst = 0

        CMD.CommandText = "Delete from EWB_Head Where InvCode = '" & NewCode & "'"
        CMD.ExecuteNonQuery()

        CMD.CommandText = "Insert into EWB_Head ([SupplyType]  ,[SubSupplyType]  , [SubSupplyDesc]  ,[DocType]  ,	[EWBGenDocNo]                           ,[EWBDocDate]        ,[FromGSTIN]       ,[FromTradeName]  ,[FromAddress1]   ,[FromAddress2]     ,[FromPlace] ," &
                         "[FromPINCode]     ,	[FromStateCode] ,[ActualFromStateCode] ,[ToGSTIN]       ,[ToTradeName],[ToAddress1]      ,[ToAddress2]    ,[ToPlace]       ,[ToPINCode]       ,[ToStateCode] , [ActualToStateCode] ," &
                         "[TransactionType],[OtherValue]                       ,	[Total_value]       ,	[CGST_Value],[SGST_Value],[IGST_Value]     ,	[CessValue],[CessNonAdvolValue],[TransporterID]    ,[TransporterName]," &
                         "[TransportDOCNo] ,[TransportDOCDate]    ,[TotalInvValue]    ,[TransMode]             ," &
                         "[VehicleNo]      ,[VehicleType]   , [InvCode], [ShippedToGSTIN], [ShippedToTradeName] ) " &
                         " " &
                         " " &
                         "  SELECT               'O'              , '4'             ,   'JOB WORK'              ,    'CHL'    , a.PavuYarn_Delivery_No ,a.PavuYarn_Delivery_Date          , C.Company_GSTINNo, C.Company_Name   ,C.Company_Address1+C.Company_Address2,c.Company_Address3+C.Company_Address4,C.Company_City ," &
                         " C.Company_PinCode    , FS.State_Code  ,FS.State_Code    ,L.Ledger_GSTINNo, L.Ledger_MainName, (case when a.DeliveryTo_IdNo <> 0 then tDELV.Ledger_Address1+tDELV.Ledger_Address2 else  L.Ledger_Address1+L.Ledger_Address2 end) as deliveryaddress1,  (case when a.DeliveryTo_IdNo <> 0 then tDELV.Ledger_Address3+tDELV.Ledger_Address4 else  L.Ledger_Address3+L.Ledger_Address4 end) as deliveryaddress2, (case when a.DeliveryTo_IdNo <> 0 then tDELV.City_Town else  L.City_Town end) as city_town_name, (case when a.DeliveryTo_IdNo <> 0 then tDELV.Pincode else  L.Pincode end) as pincodee, TS.State_Code, (case when a.DeliveryTo_IdNo <> 0 then TDCS.State_Code else TS.State_Code end) as actual_StateCode," &
                         " 1                    , 0 , a.Total_Delivery_Value    ,        ( Case When L.Ledger_Type ='Weaver' and L.Ledger_GSTINNo <> '' Then ((a.Total_Delivery_Value * Igh.Item_GST_Percentage / 100  / 2 )) else 0 end ) ,   ( Case When L.Ledger_Type ='Weaver' and L.Ledger_GSTINNo <> '' Then ((a.Total_Delivery_Value * Igh.Item_GST_Percentage / 100  / 2 )) else 0 end ) , 0   ,   0         ,0                   , t.Ledger_GSTINNo  , t.Ledger_Name ," &
                         " ''        ,''            ,  0        ,     '1'  AS TrMode ," &
                         " a.Vehicle_No, 'R', '" & NewCode & "', tDELV.Ledger_GSTINNo as ShippedTo_GSTIN, tDELV.Ledger_MainName as ShippedTo_LedgerName from PavuYarn_Delivery_Head a inner Join Company_Head C on a.Company_IdNo = C.Company_IdNo " &
                         " Inner Join Ledger_Head L ON a.DeliveryTo_IdNo = L.Ledger_IdNo Left Outer Join Ledger_Head tDELV on a.DeliveryTo_IdNo <> 0 and a.DeliveryTo_IdNo = tDELV.Ledger_IdNo left Outer Join Ledger_Head T on a.Transport_IdNo = T.Ledger_IdNo " &
                         " Left Outer Join State_Head FS On C.Company_State_IdNo = fs.State_IdNo left Outer Join State_Head TS on L.Ledger_State_IdNo = TS.State_IdNo  left Outer Join State_Head TDCS on tDELV.Ledger_State_IdNo = TDCS.State_IdNo " &
                         " LEFT OUTER JOIN Endscount_head Eh On Eh.EndsCount_Idno = a.EndsCount_Idno  LEFT OUTER JOIN Count_Head Ch on Ch.COunt_Idno = Eh.Count_idno Inner Join ItemGroup_Head IGH on Ch.ItemGroup_IdNo = IGH.ItemGroup_IdNo" &
                         " where a.PavuYarn_Delivery_Code = '" & Trim(NewCode) & "'"
        CMD.ExecuteNonQuery()




        'CMD.CommandText = "Insert into EWB_Head ([SupplyType]  ,[SubSupplyType]  , [SubSupplyDesc]  ,[DocType]  ,	[EWBGenDocNo]                           ,[EWBDocDate]        ,[FromGSTIN]       ,[FromTradeName]  ,[FromAddress1]   ,[FromAddress2]     ,[FromPlace] ," &
        '                 "[FromPINCode]     ,	[FromStateCode] ,[ActualFromStateCode] ,[ToGSTIN]       ,[ToTradeName],[ToAddress1]      ,[ToAddress2]    ,[ToPlace]       ,[ToPINCode]       ,[ToStateCode] , [ActualToStateCode] ," &
        '                 "[TransactionType],[OtherValue]                       ,	[Total_value]       ,	[CGST_Value],[SGST_Value],[IGST_Value]     ,	[CessValue],[CessNonAdvolValue],[TransporterID]    ,[TransporterName]," &
        '                 "[TransportDOCNo] ,[TransportDOCDate]    ,[TotalInvValue]    ,[TransMode]             ," &
        '                 "[VehicleNo]      ,[VehicleType]   , [InvCode], [ShippedToGSTIN], [ShippedToTradeName] ) " &
        '                 " " &
        '                 " " &
        '                 "  SELECT               'O'              , '4'             ,   'JOB WORK'              ,    'CHL'    , a.PavuYarn_Delivery_No ,a.PavuYarn_Delivery_Date          , C.Company_GSTINNo, C.Company_Name   ,C.Company_Address1+C.Company_Address2,c.Company_Address3+C.Company_Address4,C.Company_City ," &
        '                 " C.Company_PinCode    , FS.State_Code  ,FS.State_Code    ,L.Ledger_GSTINNo, L.Ledger_MainName, (case when a.DeliveryTo_IdNo <> 0 then tDELV.Ledger_Address1+tDELV.Ledger_Address2 else  L.Ledger_Address1+L.Ledger_Address2 end) as deliveryaddress1,  (case when a.DeliveryTo_IdNo <> 0 then tDELV.Ledger_Address3+tDELV.Ledger_Address4 else  L.Ledger_Address3+L.Ledger_Address4 end) as deliveryaddress2, (case when a.DeliveryTo_IdNo <> 0 then tDELV.City_Town else  L.City_Town end) as city_town_name, (case when a.DeliveryTo_IdNo <> 0 then tDELV.Pincode else  L.Pincode end) as pincodee, TS.State_Code, (case when a.DeliveryTo_IdNo <> 0 then TDCS.State_Code else TS.State_Code end) as actual_StateCode," &
        '                 " 1                    , 0 , a.Total_Delivery_Value    , 0 ,  0 , 0   ,   0         ,0                   , t.Ledger_GSTINNo  , t.Ledger_Name ," &
        '                 " ''        ,''            ,  0        ,     '1'  AS TrMode ," &
        '                 " a.Vehicle_No, 'R', '" & NewCode & "', tDELV.Ledger_GSTINNo as ShippedTo_GSTIN, tDELV.Ledger_MainName as ShippedTo_LedgerName from PavuYarn_Delivery_Head a inner Join Company_Head C on a.Company_IdNo = C.Company_IdNo " &
        '                 " Inner Join Ledger_Head L ON a.DeliveryTo_IdNo = L.Ledger_IdNo Left Outer Join Ledger_Head tDELV on a.DeliveryTo_IdNo <> 0 and a.DeliveryTo_IdNo = tDELV.Ledger_IdNo left Outer Join Ledger_Head T on a.Transport_IdNo = T.Ledger_IdNo " &
        '                 " Left Outer Join State_Head FS On C.Company_State_IdNo = fs.State_IdNo left Outer Join State_Head TS on L.Ledger_State_IdNo = TS.State_IdNo  left Outer Join State_Head TDCS on tDELV.Ledger_State_IdNo = TDCS.State_IdNo " &
        '                 " where a.PavuYarn_Delivery_Code = '" & Trim(NewCode) & "'"
        'CMD.ExecuteNonQuery()





        'If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1098" Then 'BannariAmman
        'CMD.CommandText = " Update EWB_Head Set CGST_Value = ( (Total_value * 5 / 100 ) / 2 ) , SGST_Value = ( (Total_value * 5 / 100 ) / 2 ) , TotalInvValue = ( Total_value + SGST_Value + CGST_Value )  where InvCode = '" & Trim(NewCode) & "' "
        'CMD.ExecuteNonQuery()


        'CMD.CommandText = " Update EWB_Head Set TotalInvValue = ( Total_value + SGST_Value + CGST_Value )  where InvCode = '" & Trim(NewCode) & "' "
        'CMD.ExecuteNonQuery()

        'End If


        CMD.CommandText = "Delete from EWB_Details Where InvCode = '" & NewCode & "'"
        CMD.ExecuteNonQuery()


        '************** Pavu-Details EndsCount  ****************
        Dim vCgst_Amt As String = 0
        Dim vSgst_Amt As String = 0
        Dim vIgst_AMt As String = 0

        da = New SqlClient.SqlDataAdapter(" Select  I.EndsCount_Name, (I.EndsCount_Name + ' - WARP'  ) , IG.Item_HSN_Code , ( Case When Lh.Ledger_Type ='Weaver' and Lh.Ledger_GSTINNo <> '' Then (IG.Item_GST_Percentage ) else 0 end ) , sum(Pd.Amount) As TaxableAmt, sum(Pd.Meters) as Qty, 1 , 'MTR' AS Units , tz.Company_State_IdNo , Lh.Ledger_State_Idno , pd.Rate , sum(Pd.Amount) , Pd.Tax_Perc, Sum(pd.Cgst_Amount) , Sum(pd.Sgst_Amount) , Sum(Pd.Igst_Amount)   " &
                                          " from PavuYarn_Delivery_Head SD Inner Join Pavu_Delivery_Details Pd On Pd.PavuYarn_Delivery_Code = Sd.PavuYarn_Delivery_Code  Inner Join EndsCount_Head I On PD.EndsCount_IdNo = I.EndsCount_IdNo INNER JOIN Count_Head Ch On Ch.Count_Idno = I.Count_Idno " &
                                          " Inner Join ItemGroup_Head IG on Ch.ItemGroup_IdNo = IG.ItemGroup_IdNo " &
                                          " INNER Join Ledger_Head Lh ON Lh.Ledger_Idno = Sd.DeliveryTo_IdNo INNER JOIN Company_Head tz On tz.Company_Idno = Sd.Company_Idno  Where SD.PavuYarn_Delivery_Code = '" & Trim(NewCode) & "' Group By " &
                                          " I.EndsCount_Name,IG.ItemGroup_Name,IG.Item_HSN_Code, IG.Item_GST_Percentage ,SD.Rate , Lh.Ledger_Type ,Lh.Ledger_GSTINNo , tz.Company_State_IdNo , Lh.Ledger_State_Idno ,pd.Rate , Pd.Tax_Perc  ", con)
        dt1 = New DataTable
        da.Fill(dt1)

        'da = New SqlClient.SqlDataAdapter(" Select  I.EndsCount_Name, (I.EndsCount_Name + ' - WARP'  ) , IG.Item_HSN_Code , ( Case When Lh.Ledger_Type ='Weaver' and Lh.Ledger_GSTINNo <> '' Then (IG.Item_GST_Percentage ) else 0 end ) , sum(Pd.Amount) As TaxableAmt, sum(Pd.Meters) as Qty, 1 , 'MTR' AS Units , tz.Company_State_IdNo , Lh.Ledger_State_Idno , pd.Rate , Pd.Amount , Pd.Tax_Perc, pd.Cgst_Amount , pd.Sgst_Amount , Pd.Igst_Amount   " &
        '                                  " from PavuYarn_Delivery_Head SD Inner Join Pavu_Delivery_Details Pd On Pd.PavuYarn_Delivery_Code = Sd.PavuYarn_Delivery_Code  Inner Join EndsCount_Head I On PD.EndsCount_IdNo = I.EndsCount_IdNo INNER JOIN Count_Head Ch On Ch.Count_Idno = I.Count_Idno " &
        '                                  " Inner Join ItemGroup_Head IG on Ch.ItemGroup_IdNo = IG.ItemGroup_IdNo " &
        '                                  " INNER Join Ledger_Head Lh ON Lh.Ledger_Idno = Sd.DeliveryTo_IdNo INNER JOIN Company_Head tz On tz.Company_Idno = Sd.Company_Idno  Where SD.PavuYarn_Delivery_Code = '" & Trim(NewCode) & "' Group By " &
        '                                  " I.EndsCount_Name,IG.ItemGroup_Name,IG.Item_HSN_Code, IG.Item_GST_Percentage ,SD.Rate , Lh.Ledger_Type ,Lh.Ledger_GSTINNo , tz.Company_State_IdNo , Lh.Ledger_State_Idno  ", con)
        'dt1 = New DataTable
        'da.Fill(dt1)


        If dt1.Rows.Count > 0 Then
            For I = 0 To dt1.Rows.Count - 1

                'If dt1.Rows(I).Item("Company_State_IdNo") = dt1.Rows(I).Item("Ledger_State_Idno") Then

                '    If Val(dt1.Rows(I).Item(3).ToString) <> 0 Then
                '        vCgst_Amt = ((dt1.Rows(I).Item(4) * Val(dt1.Rows(I).Item(3).ToString) / 100) / 2)
                '        vSgst_Amt = vCgst_Amt
                '        vIgst_AMt = 0
                '    Else
                '        vCgst_Amt = 0
                '        vSgst_Amt = 0
                '        vIgst_AMt = 0
                '    End If
                'Else
                '    If Val(dt1.Rows(I).Item(3).ToString) <> 0 Then
                '        vIgst_AMt = (dt1.Rows(I).Item(4) * Val(dt1.Rows(I).Item(3).ToString) / 100)
                '        vCgst_Amt = 0
                '        vSgst_Amt = 0
                '    Else
                '        vIgst_AMt = 0
                '        vCgst_Amt = 0
                '        vSgst_Amt = 0
                '    End If

                'End If


                CMD.CommandText = "Insert into EWB_Details ([SlNo]                               , [Product_Name]           ,	[Product_Description]     ,	[HSNCode]                 ,	[Quantity]                         ,      [QuantityUnit] ,          Tax_Perc                            ,	    [CessRate]         ,	[CessNonAdvol]  ,	        [TaxableAmount]          ,          InvCode          ,              Cgst_Value  ,                                          Sgst_Value           ,                                   Igst_Value) " &
                           " values                 (" & dt1.Rows(I).Item(6).ToString & ",'" & dt1.Rows(I).Item(0) & "', '" & dt1.Rows(I).Item(1) & "', '" & dt1.Rows(I).Item(2) & "', " & dt1.Rows(I).Item(5).ToString & ",        'MTR'         ,     " & dt1.Rows(I).Item(3).ToString & "    ,               0           ,           0                   ," & dt1.Rows(I).Item(4) & ",        '" & NewCode & "'    ,       " & dt1.Rows(I).Item(13).ToString & "  ,      " & dt1.Rows(I).Item(14).ToString & ",  " & dt1.Rows(I).Item(15).ToString & ")"
                CMD.ExecuteNonQuery()

                'CMD.CommandText = "Insert into EWB_Details ([SlNo]                               , [Product_Name]           ,	[Product_Description]     ,	[HSNCode]                 ,	[Quantity]                                ,[QuantityUnit] ,  Tax_Perc                         ,	[CessRate]         ,	[CessNonAdvol]  ,	[TaxableAmount]               ,InvCode          ,              Cgst_Value  ,                       Sgst_Value           ,       Igst_Value) " &
                '                  " values                 (" & dt1.Rows(I).Item(6).ToString & ",'" & dt1.Rows(I).Item(0) & "', '" & dt1.Rows(I).Item(1) & "', '" & dt1.Rows(I).Item(2) & "', " & dt1.Rows(I).Item(5).ToString & ",'MTR'          ," & dt1.Rows(I).Item(3).ToString & ", 0                  , 0                   ," & dt1.Rows(I).Item(4) & ",'" & NewCode & "'    ,       '" & Str(Val(vCgst_Amt)) & "'  ,      '" & Str(Val(vSgst_Amt)) & "',  '" & Str(Val(vIgst_AMt)) & "')"
                'CMD.ExecuteNonQuery()

            Next
        End If

        '************** Pavu-Head EndsCount  ****************

        Dim vMtrCond As String = ""
        Dim vMtrCond1 As String = ""

        Dim vUnit As String = ""
        Dim vTxbleAmt As String = 0

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1098" Then
            vMtrCond = "(Case When sum(SD.Meters) <> 0 Then sum(SD.Meters) else Sum(sd.Empty_Beam) end) as Qty"
            vMtrCond1 = " and ( (SD.Meters <> 0) or (Sd.Empty_Beam <> 0) )"

            If Val(txt_KuraiPavuBeam.Text) <> 0 Then
                If Val(txt_KuraiPavuMeters.Text) = 0 Then
                    vUnit = " 'NOS' AS Units "
                Else
                    vUnit = " 'MTR' AS Units "
                End If

            Else
                vUnit = " 'MTR' AS Units "
            End If

            If Trim(cbo_RateFor.Text) = "PAVU" Then
                vTxbleAmt = "sum(sd.Amount)"
            Else
                vTxbleAmt = " (sum(SD.Meters)*SD.Rate) As TaxableAmt "
            End If

        Else
            vMtrCond = "sum(SD.Meters) as Qty"
            vMtrCond1 = " and  (SD.Meters > 0)"
            vUnit = " 'MTR' AS Units "
        End If


        da = New SqlClient.SqlDataAdapter(" Select I.EndsCount_Name, (I.EndsCount_Name + ' - WARP'  ) , IG.Item_HSN_Code , ( Case When Lh.Ledger_Type ='Weaver' and Lh.Ledger_GSTINNo <> '' Then (IG.Item_GST_Percentage ) else 0 end )  , " & vTxbleAmt & " , " & vMtrCond & " , 201 as SlNo, " & vUnit & " , tz.Company_State_IdNo , Lh.Ledger_State_Idno " &
                                          " from PavuYarn_Delivery_Head SD Inner Join EndsCount_Head I On SD.EndsCount_IdNo = I.EndsCount_IdNo INNER JOIN Count_Head Ch On Ch.Count_Idno = I.Count_Idno " &
                                          " LEFT OUTER Join ItemGroup_Head IG on Ch.ItemGroup_IdNo = IG.ItemGroup_IdNo " &
                                          " INNER Join Ledger_Head Lh ON Lh.Ledger_Idno = Sd.DeliveryTo_IdNo INNER JOIN Company_Head tz On tz.Company_Idno = Sd.Company_Idno Where SD.PavuYarn_Delivery_Code = '" & Trim(NewCode) & "' " & Trim(vMtrCond1) & " " & 'and SD.Meters > 0 Group By " &
                                          " Group By I.EndsCount_Name,IG.ItemGroup_Name,IG.Item_HSN_Code, IG.Item_GST_Percentage,SD.Rate, Lh.Ledger_Type ,Lh.Ledger_GSTINNo , tz.Company_State_IdNo , Lh.Ledger_State_Idno  ", con)
        dt1 = New DataTable
        da.Fill(dt1)

        'da = New SqlClient.SqlDataAdapter(" Select I.EndsCount_Name, (I.EndsCount_Name + ' - WARP'  ) , IG.Item_HSN_Code , ( Case When Lh.Ledger_Type ='Weaver' and Lh.Ledger_GSTINNo <> '' Then (IG.Item_GST_Percentage ) else 0 end )  , (sum(SD.Meters)*SD.Rate) As TaxableAmt, sum(SD.Meters) as Qty, 201 as SlNo, 'MTR' AS Units , tz.Company_State_IdNo , Lh.Ledger_State_Idno " &
        '                                  " from PavuYarn_Delivery_Head SD Inner Join EndsCount_Head I On SD.EndsCount_IdNo = I.EndsCount_IdNo INNER JOIN Count_Head Ch On Ch.Count_Idno = I.Count_Idno " &
        '                                  " Inner Join ItemGroup_Head IG on Ch.ItemGroup_IdNo = IG.ItemGroup_IdNo " &
        '                                  " INNER Join Ledger_Head Lh ON Lh.Ledger_Idno = Sd.DeliveryTo_IdNo INNER JOIN Company_Head tz On tz.Company_Idno = Sd.Company_Idno Where SD.PavuYarn_Delivery_Code = '" & Trim(NewCode) & "' " & 'and SD.Meters > 0 Group By " &
        '                                  " Group By I.EndsCount_Name,IG.ItemGroup_Name,IG.Item_HSN_Code, IG.Item_GST_Percentage,SD.Rate, Lh.Ledger_Type ,Lh.Ledger_GSTINNo , tz.Company_State_IdNo , Lh.Ledger_State_Idno  ", con)
        'dt1 = New DataTable
        'da.Fill(dt1)
        If dt1.Rows.Count > 0 Then
            For I = 0 To dt1.Rows.Count - 1


                If dt1.Rows(I).Item("Company_State_IdNo") = dt1.Rows(I).Item("Ledger_State_Idno") Then

                    If Val(dt1.Rows(I).Item(3).ToString) <> 0 Then
                        vCgst_Amt = ((dt1.Rows(I).Item(4) * Val(dt1.Rows(I).Item(3).ToString) / 100) / 2)
                        vSgst_Amt = vCgst_Amt
                        vIgst_AMt = 0
                    Else
                        vCgst_Amt = 0
                        vSgst_Amt = 0
                        vIgst_AMt = 0
                    End If
                Else
                    If Val(dt1.Rows(I).Item(3).ToString) <> 0 Then
                        vIgst_AMt = (dt1.Rows(I).Item(4) * Val(dt1.Rows(I).Item(3).ToString) / 100)
                        vCgst_Amt = 0
                        vSgst_Amt = 0
                    Else
                        vIgst_AMt = 0
                        vCgst_Amt = 0
                        vSgst_Amt = 0
                    End If

                End If

                CMD.CommandText = "Insert into EWB_Details ( [SlNo]                              ,     [Product_Name]           ,	[Product_Description]     ,	[HSNCode]                     ,	[Quantity]                          ,                        [QuantityUnit] ,                                    Tax_Perc                           ,	[CessRate]         ,	[CessNonAdvol]  ,	[TaxableAmount]          ,      InvCode         ,                    Cgst_Value      ,                    Sgst_Value        ,                   Igst_Value  ) " &
                                  " values                 ( " & dt1.Rows(I).Item(6).ToString & ", '" & dt1.Rows(I).Item(0) & "', '" & dt1.Rows(I).Item(1) & "', '" & dt1.Rows(I).Item(2) & "', " & dt1.Rows(I).Item(5).ToString & ",       '" & dt1.Rows(I).Item(7).ToString & "'         , " & dt1.Rows(I).Item(3).ToString & ", 0                  , 0                   ," & dt1.Rows(I).Item(4) & ", '" & NewCode & "'    ,    '" & Str(Val(vCgst_Amt)) & "'      ,      '" & Str(Val(vSgst_Amt)) & "'    ,   '" & Str(Val(vIgst_AMt)) & "'   )"
                CMD.ExecuteNonQuery()

            Next
        End If

        '************** Yarn Delivery ****************


        da = New SqlClient.SqlDataAdapter(" Select  I.Count_Name, (CASE WHEN I.Count_Description <> '' THEN I.Count_Description ELSE I.Count_Name END),IG.Item_HSN_Code, ( Case When Lh.Ledger_Type ='Weaver' and Lh.Ledger_GSTINNo <> '' Then (IG.Item_GST_Percentage ) else 0 end )   , sum(a.Total_Amount) As TaxableAmt,sum(a.Total_Weight) as Qty, 1 , 'KGS' AS Units , tz.Company_State_IdNo , Lh.Ledger_State_Idno " &
                                          " from Yarn_Delivery_Details SD Inner Join PavuYarn_Delivery_Head a On a.PavuYarn_Delivery_Code = sd.PavuYarn_Delivery_Code Inner Join Count_Head I On SD.Count_IdNo = I.Count_IdNo Inner Join ItemGroup_Head IG on I.ItemGroup_IdNo = IG.ItemGroup_IdNo " &
                                          " INNER Join Ledger_Head Lh ON Lh.Ledger_Idno = a.DeliveryTo_IdNo INNER JOIN Company_Head tz On tz.Company_Idno = a.Company_Idno Where SD.PavuYarn_Delivery_Code = '" & Trim(NewCode) & "' Group By " &
                                          " I.Count_Name,I.Count_Description ,IG.ItemGroup_Name,IG.Item_HSN_Code,IG.ItemGroup_Name ,IG.Item_HSN_Code,IG.Item_GST_Percentage , Lh.Ledger_Type ,Lh.Ledger_GSTINNo , tz.Company_State_IdNo , Lh.Ledger_State_Idno ", con)
        dt1 = New DataTable
        da.Fill(dt1)

        For I = 0 To dt1.Rows.Count - 1


            If dt1.Rows(I).Item("Company_State_IdNo") = dt1.Rows(I).Item("Ledger_State_Idno") Then

                If Val(dt1.Rows(I).Item(3).ToString) <> 0 Then
                    vCgst_Amt = ((dt1.Rows(I).Item(4) * Val(dt1.Rows(I).Item(3).ToString) / 100) / 2)
                    vSgst_Amt = vCgst_Amt
                    vIgst_AMt = 0
                Else
                    vCgst_Amt = 0
                    vSgst_Amt = 0
                    vIgst_AMt = 0
                End If
            Else
                If Val(dt1.Rows(I).Item(3).ToString) <> 0 Then
                    vIgst_AMt = (dt1.Rows(I).Item(4) * Val(dt1.Rows(I).Item(3).ToString) / 100)
                    vCgst_Amt = 0
                    vSgst_Amt = 0
                Else
                    vIgst_AMt = 0
                    vCgst_Amt = 0
                    vSgst_Amt = 0
                End If

            End If

            CMD.CommandText = "Insert into EWB_Details ([SlNo]                               , [Product_Name]           ,	[Product_Description]     ,	[HSNCode]                 ,	[Quantity]                                ,[QuantityUnit] ,  Tax_Perc                         ,	[CessRate]         ,	[CessNonAdvol]  ,	[TaxableAmount]               ,InvCode       ,                   Cgst_Value  ,                       Sgst_Value ,                         Igst_Value) " &
                              " values                 (" & dt1.Rows(I).Item(6).ToString & ",'" & dt1.Rows(I).Item(0) & "', '" & dt1.Rows(I).Item(1) & "', '" & dt1.Rows(I).Item(2) & "', " & dt1.Rows(I).Item(5).ToString & ",'KGS'          ," & dt1.Rows(I).Item(3).ToString & ", 0                  , 0                   ," & dt1.Rows(I).Item(4) & ",'" & NewCode & "'   ,   '" & Str(Val(vCgst_Amt)) & "'        ,   '" & Str(Val(vSgst_Amt)) & "'     , '" & Str(Val(vIgst_AMt)) & "' )"

            CMD.ExecuteNonQuery()

        Next


        da1 = New SqlClient.SqlDataAdapter(" Select  * from EWB_Details Ewd  Where Ewd.InvCode = '" & Trim(NewCode) & "' and (Ewd.Cgst_Value <> 0 or Ewd.Sgst_Value <> 0 or Ewd.Igst_Value <> 0) ", con)
        dt2 = New DataTable
        da1.Fill(dt2)

        If dt2.Rows.Count > 0 Then

            If dt2.Rows(0).Item("Igst_Value") <> 0 Then

                CMD.CommandText = " Update EWB_Head Set IGST_Value = (select sum(Ed.Igst_Value) from EWB_Details Ed  where Ed.InvCode = '" & Trim(NewCode) & "' and Ed.Igst_Value <> 0) "
                CMD.ExecuteNonQuery()
            Else
                CMD.CommandText = " Update EWB_Head Set CGST_Value = (select sum(Ed.Cgst_Value) from EWB_Details Ed  where Ed.InvCode = '" & Trim(NewCode) & "' and Ed.Cgst_Value <> 0 ) "
                CMD.ExecuteNonQuery()

                CMD.CommandText = " Update EWB_Head Set SGST_Value = (select sum(Ed.Sgst_Value) from EWB_Details Ed where Ed.InvCode = '" & Trim(NewCode) & "' and Ed.Sgst_Value <> 0) "
                CMD.ExecuteNonQuery()
            End If

        End If
        dt2.Clear()




        'CMD.CommandText = "Update " & Trim(Common_Procedures.ReportTempTable) & " set Meters3 = (select max(z.Rate) from Yarn_Sales_Details z where 'YNSAL-' + z.Yarn_Sales_Code = " & Trim(Common_Procedures.ReportTempTable) & ".Name10 )"
        'CMD.ExecuteNonQuery()

        '----------------------


        btn_GENERATEEWB.Enabled = False

        Dim ewb As New EWB(Val(lbl_Company.Tag))
        EWB.GenerateEWB(NewCode, con, rtbEWBResponse, txt_EWBNo, "PavuYarn_Delivery_Head", "Eway_BillNo", "PavuYarn_Delivery_Code", Pk_Condition)

    End Sub

    Private Sub txt_PartyDcNo_KeyDown(sender As Object, e As KeyEventArgs) Handles txt_PartyDcNo.KeyDown

        If e.KeyValue = 38 Then
            cbo_YarnRecForm.Focus()

        End If


        If e.KeyValue = 40 Then
            If txt_JJFormNo.Visible Then
                txt_JJFormNo.Focus()
            Else
                txt_Eway_Bill_No.Focus()

            End If
        End If

    End Sub

    Private Sub txt_PartyDcNo_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txt_PartyDcNo.KeyPress
        If Asc(e.KeyChar) = 13 Then
            If txt_JJFormNo.Visible Then
                txt_JJFormNo.Focus()
            Else
                txt_Eway_Bill_No.Focus()

            End If
        End If
    End Sub

    Private Sub txt_Eway_Bill_No_KeyDown(sender As Object, e As KeyEventArgs) Handles txt_Eway_Bill_No.KeyDown
        If e.KeyValue = 38 Then
            txt_PartyDcNo.Focus()
        End If
        If e.KeyValue = 40 Then
            cbo_EndsCount.Focus()

        End If

    End Sub

    Private Sub txt_Eway_Bill_No_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txt_Eway_Bill_No.KeyPress
        If Asc(e.KeyChar) = 13 Then
            cbo_EndsCount.Focus()
        End If
    End Sub

    Private Sub txt_Eway_Bill_No_TextChanged(sender As Object, e As EventArgs) Handles txt_Eway_Bill_No.TextChanged
        txt_EWBNo.Text = txt_Eway_Bill_No.Text
    End Sub

    Private Sub btn_Detail_PRINTEWB_Click(sender As Object, e As EventArgs) Handles btn_Detail_PRINTEWB.Click
        Dim ewb As New EWB(Val(lbl_Company.Tag))

        EWB.PrintEWB(txt_Eway_Bill_No.Text, rtbEWBResponse, 1)
    End Sub

    Private Sub Total_YarnPavu_Amount_Calculation()
        Dim vTotValue As String
        Dim vTotYarn As String

        vTotValue = 0 : vTotYarn = 0

        With dgv_YarnDetails_Total

            If dgv_YarnDetails_Total.RowCount > 0 Then

                vTotYarn = Val(dgv_YarnDetails_Total.Rows(0).Cells(10).Value())
            End If

        End With


        vTotValue = Format(Val(lbl_Amount.Text) + Val(vTotYarn), "##########0.00")
        lbl_Total_Value.Text = Val(vTotValue)

    End Sub

    Private Sub lbl_Amount_TextChanged(sender As Object, e As EventArgs) Handles lbl_Amount.TextChanged
        Total_YarnPavu_Amount_Calculation()
    End Sub

    Private Sub cbo_RateFor_TextChanged(sender As Object, e As EventArgs) Handles cbo_RateFor.TextChanged
        TotalPavu_Calculation()
    End Sub

    Private Sub txt_KuraiPavuMeters_TextChanged(sender As Object, e As EventArgs) Handles txt_KuraiPavuMeters.TextChanged
        TotalPavu_Calculation()
    End Sub



    Private Sub cbo_Verified_Sts_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbo_Verified_Sts.SelectedIndexChanged

    End Sub

    Private Sub cbo_Filter_Beam_No_SelectedIndexChanged(sender As Object, e As EventArgs)

    End Sub

    Private Sub cbo_Filter_Beam_No_KeyDown(sender As Object, e As KeyEventArgs)
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_filter_beamNo, btn_Filter_Show, "", "Pavu_Delivery_Details", "Beam_No", "", "")



        If e.KeyCode = 38 Then

            If cbo_Verified_Sts.Visible = True Then


                cbo_Verified_Sts.Focus()
            Else

                cbo_Filter_EndsCount.Focus()

            End If

        End If


    End Sub

    Private Sub cbo_filter_beamNo_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbo_filter_beamNo.SelectedIndexChanged

    End Sub

    Private Sub cbo_filter_beamNo_KeyPress(sender As Object, e As KeyPressEventArgs) Handles cbo_filter_beamNo.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_filter_beamNo, btn_Filter_Show, "Pavu_Delivery_Details", "Beam_No", "", "")
    End Sub

    Private Sub cbo_filter_beamNo_KeyDown(sender As Object, e As KeyEventArgs) Handles cbo_filter_beamNo.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_filter_beamNo, "", btn_Filter_Show, "Pavu_Delivery_Details", "Beam_No", "", "")


        If e.KeyCode = 38 Then
            If cbo_Verified_Sts.Visible = True Then
                cbo_Verified_Sts.Focus()
            Else
                cbo_Filter_EndsCount.Focus()
            End If
        End If




    End Sub

    Private Sub cbo_filter_beamNo_GotFocus(sender As Object, e As EventArgs) Handles cbo_filter_beamNo.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Pavu_Delivery_Details", "Beam_No", "", "")
    End Sub



    Private Sub Printing_Format1420(ByRef e As System.Drawing.Printing.PrintPageEventArgs)
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
        'Dim ps As Printing.PaperSize
        Dim strHeight As Single = 0
        Dim PpSzSTS As Boolean = False
        Dim W1 As Single = 0
        Dim Inc As Integer = 0

        If prn_PageSize_SetUP_STS = False Then
            set_PaperSize_For_PrintDocument1()
            prn_PageSize_SetUP_STS = True
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

        NoofItems_PerPage = 35 '8 ' 6

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

                    Printing_Format1420_PageHeader(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, prn_PageNo, NoofItems_PerPage, CurY, LnAr, ClArr)

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

                                Printing_Format1420_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, NoofDets, False)

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

                    Printing_Format1420_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, NoofDets, True)

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


    Private Sub Printing_Format1420_PageHeader(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByRef PageNo As Integer, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single)
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
        Dim Cmp_PhNo As String, Cmp_TinNo As String, Cmp_CstNo As String, Cmp_GSTNo As String
        Dim vPrn_DcNo As String = ""
        Dim Entry_Date As Date = Convert.ToDateTime(prn_HdDt.Rows(prn_HeadIndx).Item("PavuYarn_Delivery_Date").ToString)
        Dim strWidth As Single = 0
        Dim CurX As Single = 0
        PageNo = PageNo + 1

        CurY = TMargin

        da2 = New SqlClient.SqlDataAdapter("select a.*, b.Mill_Name, c.Count_Name  from Yarn_Delivery_Details a INNER JOIN Mill_Head b on a.Mill_IdNo = b.Mill_IdNo LEFT OUTER JOIN Count_Head c on a.Count_IdNo = c.Count_IdNo where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.PavuYarn_Delivery_Code = '" & Trim(EntryCode) & "' Order by a.sl_no", con)
        da2.Fill(dt2)

        If dt2.Rows.Count > NoofItems_PerPage Then
            Common_Procedures.Print_To_PrintDocument(e, "Page : " & Trim(Val(PageNo)), PageWidth - 10, CurY - TxtHgt, 1, 0, pFont)
        End If
        dt2.Clear()

        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(1) = CurY

        Cmp_Name = "" : Cmp_Add1 = "" : Cmp_Add2 = ""
        Cmp_PhNo = "" : Cmp_TinNo = "" : Cmp_CstNo = "" : Cmp_GSTNo = ""

        Cmp_Name = prn_HdDt.Rows(prn_HeadIndx).Item("Company_Name").ToString
        Cmp_Add1 = prn_HdDt.Rows(prn_HeadIndx).Item("Company_Address1").ToString & " " & prn_HdDt.Rows(prn_HeadIndx).Item("Company_Address2").ToString
        Cmp_Add2 = prn_HdDt.Rows(prn_HeadIndx).Item("Company_Address3").ToString & " " & prn_HdDt.Rows(prn_HeadIndx).Item("Company_Address4").ToString
        If Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Company_PhoneNo").ToString) <> "" Then
            Cmp_PhNo = "PHONE NO.:" & prn_HdDt.Rows(prn_HeadIndx).Item("Company_PhoneNo").ToString
        End If

        If Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Company_TinNo").ToString) <> "" Then
            Cmp_TinNo = "TIN NO.: " & prn_HdDt.Rows(prn_HeadIndx).Item("Company_TinNo").ToString
        End If
        If Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Company_panNo").ToString) <> "" Then
            Cmp_CstNo = "PAN NO.: " & prn_HdDt.Rows(prn_HeadIndx).Item("Company_panNo").ToString
        End If
        If Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Company_GSTinNo").ToString) <> "" Then
            Cmp_GSTNo = "GSTIN : " & prn_HdDt.Rows(prn_HeadIndx).Item("Company_GSTinNo").ToString
        End If
        CurY = CurY + TxtHgt - 10
        p1Font = New Font("Calibri", 18, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Name, LMargin, CurY, 2, PrintWidth, p1Font)
        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height

        If Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Company_logo_Image").ToString) <> "" Then

            If IsDBNull(prn_HdDt.Rows(prn_HeadIndx).Item("Company_logo_Image")) = False Then

                Dim imageData As Byte() = DirectCast(prn_HdDt.Rows(prn_HeadIndx).Item("Company_logo_Image"), Byte())
                If Not imageData Is Nothing Then
                    Using ms As New MemoryStream(imageData, 0, imageData.Length)
                        ms.Write(imageData, 0, imageData.Length)

                        If imageData.Length > 0 Then

                            '.BackgroundImage = Image.FromStream(ms)

                            ' e.Graphics.DrawImage(DirectCast(pic_IRN_QRCode_Image_forPrinting.BackgroundImage, Drawing.Image), PageWidth - 108, CurY + 10, 90, 90)
                            e.Graphics.DrawImage(DirectCast(Image.FromStream(ms), Drawing.Image), LMargin + 10, CurY, 110, 90)

                        End If

                    End Using

                End If

            End If

        End If

        CurY = CurY + strHeight - 1
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Add1, LMargin, CurY, 2, PrintWidth, pFont)

        CurY = CurY + TxtHgt - 1
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Add2, LMargin, CurY, 2, PrintWidth, pFont)
        CurY = CurY + TxtHgt - 1
        Common_Procedures.Print_To_PrintDocument(e, Cmp_PhNo, LMargin, CurY, 2, PrintWidth, pFont)
        CurY = CurY + TxtHgt - 1
        If Entry_Date >= Common_Procedures.GST_Start_Date Then
            Common_Procedures.Print_To_PrintDocument(e, Cmp_GSTNo & "   /   " & Cmp_CstNo, LMargin + 10, CurY, 0, 0, pFont)
        Else
            Common_Procedures.Print_To_PrintDocument(e, Cmp_TinNo, LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Cmp_CstNo, PageWidth - 10, CurY, 1, 0, pFont)
        End If
        CurY = CurY + TxtHgt - 10
        p1Font = New Font("Calibri", 16, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "DELIVERY", LMargin, CurY, 2, PrintWidth, p1Font)
        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height

        'CurY = CurY + TxtHgt

        CurY = CurY + strHeight + 5 ' + 150
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(2) = CurY

        Try

            M1 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6)
            W1 = e.Graphics.MeasureString("FORM JJ NO    : ", pFont).Width
            N1 = e.Graphics.MeasureString("TO   :", pFont).Width

            CurY = CurY + TxtHgt - 10
            p1Font = New Font("Calibri", 10, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "TO  :  " & "M/s." & prn_HdDt.Rows(prn_HeadIndx).Item("Del_Name").ToString, LMargin + 10, CurY, 0, 0, p1Font)
            Common_Procedures.Print_To_PrintDocument(e, "DC.NO", LMargin + M1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + M1 + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(prn_HeadIndx).Item("PavuYarn_Delivery_No").ToString, LMargin + M1 + W1 + 30, CurY, 0, 0, p1Font)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(prn_HeadIndx).Item("Ledger_Address1").ToString, LMargin + N1 + 10, CurY, 0, 0, pFont)
            p1Font = New Font("Calibri", 14, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "DATE", LMargin + M1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + M1 + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(prn_HeadIndx).Item("PavuYarn_Delivery_Date").ToString), "dd-MM-yyyy").ToString, LMargin + M1 + W1 + 30, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(prn_HeadIndx).Item("Ledger_Address2").ToString, LMargin + N1 + 10, CurY, 0, 0, pFont)

            If prn_HdDt.Rows(prn_HeadIndx).Item("Party_DcNo").ToString <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, "P.DC.NO", LMargin + M1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + M1 + W1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(prn_HeadIndx).Item("Party_DcNo").ToString, LMargin + M1 + W1 + 30, CurY, 0, 0, pFont)
            End If

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(prn_HeadIndx).Item("Ledger_Address3").ToString & " " & prn_HdDt.Rows(prn_HeadIndx).Item("Ledger_Address4").ToString, LMargin + N1 + 10, CurY, 0, 0, pFont)
            If Trim(vPrn_PvuSetNo) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, "SET NO", LMargin + M1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + M1 + W1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, Trim(vPrn_PvuSetNo), LMargin + M1 + W1 + 30, CurY, 0, 0, pFont)
            End If

            CurY = CurY + TxtHgt

            If prn_HdDt.Rows(prn_HeadIndx).Item("Ledger_GSTinNo").ToString <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, "GSTIN  : " & prn_HdDt.Rows(prn_HeadIndx).Item("Ledger_GSTinNo").ToString, LMargin + N1 + 10, CurY, 0, 0, pFont)
            ElseIf Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Pan_No").ToString) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, "PAN  : " & prn_HdDt.Rows(prn_HeadIndx).Item("Pan_No").ToString, LMargin + N1 + 10, CurY, 0, 0, pFont)
                'strWidth = e.Graphics.MeasureString("GSTIN : " & prn_HdDt.Rows(prn_HeadIndx).Item("Ledger_GSTinNo").ToString, p1Font).Width
                'CurX = LMargin + N1 + 10 + strWidth
                'Common_Procedures.Print_To_PrintDocument(e, "      PAN : " & prn_HdDt.Rows(prn_HeadIndx).Item("Pan_No").ToString, CurX, CurY, 0, PrintWidth, p1Font)
            End If

            If prn_HdDt.Rows(prn_HeadIndx).Item("Eway_BillNo").ToString <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, "EWAYBILL.NO", LMargin + M1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + M1 + W1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(prn_HeadIndx).Item("Eway_BillNo").ToString, LMargin + M1 + W1 + 30, CurY, 0, 0, pFont)
            End If






            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1040" Then '---- M.S Textiles (Tirupur)

                vPrn_DcNo = ""

                OrdByNo = Common_Procedures.OrderBy_CodeToValue(Trim(prn_HdDt.Rows(prn_HeadIndx).Item("PavuYarn_Delivery_No").ToString))
                Da = New SqlClient.SqlDataAdapter("select top 1 PavuYarn_Delivery_No from PavuYarn_Delivery_Head where DeliveryTo_Idno = " & Str(Val(prn_HdDt.Rows(prn_HeadIndx).Item("DeliveryTo_Idno").ToString)) & " and for_orderby < " & Str(Format(Val(OrdByNo), "######0.00")) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and PavuYarn_Delivery_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, PavuYarn_Delivery_No desc", con)
                Dt1 = New DataTable
                Da.Fill(Dt1)
                If Dt1.Rows.Count > 0 Then
                    vPrn_DcNo = Dt1.Rows(0).Item("PavuYarn_Delivery_No").ToString
                End If
                Dt1.Clear()
                If Trim(vPrn_DcNo) <> "" Then
                    Common_Procedures.Print_To_PrintDocument(e, "Prev Dc.No", LMargin + M1 + 10, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + M1 + W1 + 10, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, Trim(vPrn_DcNo), LMargin + M1 + W1 + 30, CurY, 0, 0, pFont)
                End If

            ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1065" Then '---- Logu Tex (Palladam)
                If Trim(prn_HdDt.Rows(prn_HeadIndx).Item("JJ_FormNo").ToString) <> "" Then
                    Common_Procedures.Print_To_PrintDocument(e, "FORM JJ NO", LMargin + M1 + 10, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + M1 + W1 + 10, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(prn_HeadIndx).Item("JJ_FormNo").ToString), LMargin + M1 + W1 + 30, CurY, 0, 0, pFont)
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

    Private Sub Printing_Format1420_PageFooter(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single, ByVal NoofDets As Integer, ByVal is_LastPage As Boolean)
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
            If Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Vehicle_No").ToString) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, "Vehicle No. : " & Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Vehicle_No").ToString), PageWidth - 200, CurY, 1, 0, pFont)
            End If


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
            'If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1037" Then '---- Prakash Textiles (Somanur)




            CurY = CurY + TxtHgt
            CurY = CurY + TxtHgt

            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

            CurY = CurY + TxtHgt

            Common_Procedures.Print_To_PrintDocument(e, "Note : " & prn_HdDt.Rows(prn_HeadIndx).Item("Note").ToString, LMargin + 20, CurY, 0, 0, pFont)


            CurY = CurY + TxtHgt
            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

            'End If

            'Common_Procedures.Print_To_PrintDocument(e, "Rec.From : " & From_name, LMargin + 20, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
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

    Private Sub cbo_ClothSales_OrderCode_forSelection_KeyPress(sender As Object, e As KeyPressEventArgs) Handles cbo_ClothSales_OrderCode_forSelection.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, sender, Nothing, "ClothSales_Order_Head", "ClothSales_OrderCode_forSelection", vCLO_CONDT, "(ClothSales_Order_Code = '999999/00-00')")

        If Asc(e.KeyChar) = 13 Then

            If dgv_YarnDetails.Rows.Count > 0 Then

                dgv_YarnDetails.Focus()
                dgv_YarnDetails.CurrentCell = dgv_YarnDetails.Rows(0).Cells(1)


            Else


                If MessageBox.Show("Do you want to save ?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = DialogResult.Yes Then
                    save_record()
                Else
                    msk_Date.Focus()
                End If

            End If

        End If


    End Sub

    Private Sub cbo_ClothSales_OrderCode_forSelection_KeyDown(sender As Object, e As KeyEventArgs) Handles cbo_ClothSales_OrderCode_forSelection.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, sender, Nothing, Nothing, "ClothSales_Order_Head", "ClothSales_OrderCode_forSelection", vCLO_CONDT, "(ClothSales_Order_Code = '999999/00-00')")


        If (e.KeyValue = 40 And sender.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then

            If dgv_YarnDetails.Rows.Count > 0 Then
                dgv_YarnDetails.Focus()
                dgv_YarnDetails.CurrentCell = dgv_YarnDetails.Rows(0).Cells(1)


            Else


                If MessageBox.Show("Do you want to save ?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = DialogResult.Yes Then
                    save_record()
                Else
                    msk_Date.Focus()
                End If
            End If

        End If

        If (e.KeyValue = 38 And sender.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then

            If cbo_WidthType.Visible = True Then
                cbo_WidthType.Focus()
            ElseIf cbo_Cloth.Visible = True Then
                cbo_Cloth.Focus()
            Else
                txt_Freight.Focus()
            End If



        End If

    End Sub

    Private Sub cbo_ClothSales_OrderCode_forSelection_Enter(sender As Object, e As EventArgs) Handles cbo_ClothSales_OrderCode_forSelection.Enter
        vCLO_CONDT = "(ClothSales_Order_Code LIKE '%/" & Trim(FnYearCode1) & "' or ClothSales_Order_Code LIKE '%/" & Trim(FnYearCode2) & "' and Order_Close_Status = 0 )"
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "ClothSales_Order_Head", "ClothSales_OrderCode_forSelection", vCLO_CONDT, "(ClothSales_Order_Code = '999999/00-00')")
    End Sub

    
End Class