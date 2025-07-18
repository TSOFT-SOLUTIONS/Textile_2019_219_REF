Public Class Sizing_Warping_Entry
    Implements Interface_MDIActions

    Const NOSAVE = -1
    Const PDSaveFull = 1

    Private Con As New SqlClient.SqlConnection(Common_Procedures.Connection_String)
    Private con1 As New SqlClient.SqlConnection(Common_Procedures.ConnectionString_CompanyGroupdetails)
    Private FrmLdSTS As Boolean = False
    Private New_Entry As Boolean = False
    Private Insert_Entry As Boolean = False
    Private Filter_Status As Boolean = False
    Private Pk_Condition As String = "WARPG-"
    Private Prec_ActCtrl As New Control
    Private vCbo_ItmNm As String
    Private vcbo_KeyDwnVal As Double
    Private Prev_kyData As Keys
    Private Ctrl_kyData As Boolean
    Private prn_Count As Integer = 0
    Private WithEvents dgtxt_Details As New DataGridViewTextBoxEditingControl
    Private WithEvents ppd As New PrintPreviewDialog
    Private sum_Total_Amount As Single
    Private PrntCnt2ndPageSTS As Boolean = False
    Private prn_HdDt As New DataTable
    Private prn_DetDt As New DataTable
    Private prn_PageNo As Integer
    Private prn_DetIndx As Integer
    Private prn_DetSNo As Integer
    Private Prn_YrnStkSumm_2ndPage_STS As Boolean = False
    Private Prn_YrnStkSumm_StartIndx As Integer = 0
    Private Prn_SizingDetails_2ndPage_STS As Boolean = False
    Private Prn_SizingDetails_StartIndx As Integer = 0
    Private Prn_YrnTknDet_2ndPage_STS As Boolean = False
    Private Prn_YrnTknDet_StartIndx As Integer = 0
    Private Prn_SizInvoice_2ndPage_STS As Boolean = False
    Private Prn_SizInvoice_StartIndx As Integer = 0

    Private prn_Yrn_OpWt As Single, prn_Yrn_RcptWt As Single, prn_Yrn_DelvWt As Single
    Private prn_Yrn_ConsWt As Single, prn_Yrn_ExShWt As Single
    Private prn_Yrn_TknWt As Single, prn_Yrn_BayCnWt As Single
    Private prn_Yrn_RwExShWt As Single, prn_Yrn_RwExShStNo As String
    Private prn_Yrn_TranfrWt As Single, prn_Yrn_TranfrNo As String
    Private prn_Yrn_ExcSht_Wt As Single, prn_Yrn_ExcSht_No As String
    Private prn_Yrn_OpStNo As String, prn_Yrn_RcptNo As String, prn_Yrn_DcNo As String
    Private prn_Yrn_MillRcptWt As Double, prn_Yrn_RWRcptWt As Double

    Private prn_EmpBm_Op As Single, prn_EmpBm_Rcpt As Single, prn_EmpBm_Delv As Single, prn_EmpBm_Cons As Single
    Private prn_EmpBm_OpStNo As String, prn_EmpBm_RcptNo As String, prn_EmpBm_DcNo As String

    Private prn_EmpBg_Op As Single, prn_EmpBg_Rcpt As Single, prn_EmpBg_Delv As Single
    Private prn_EmpBg_OpStNo As String, prn_EmpBg_RcptNo As String, prn_EmpBg_DcNo As String

    Private prn_Amt_Op As Single, prn_Amt_Rcpt As Single, prn_Amt_CurSet As Single
    Private prn_Amt_OpStNo As String, prn_Amt_RcptNo As String

    Private Prn_TtSizBms As Single
    Private Prn_TtSizGrsWgt As Single, Prn_TtSizTrWgt As Single, Prn_TtSizNetWgt As Single
    Private Prn_TtSizPcs As Single, Prn_TtSizMtrs As Single

    Private Prn_TtYSBgs As Single, Prn_TtYSCns As Single, Prn_TtYSWgt As Single


    Private WithEvents dgtxt_SizingDetails_Set1 As New DataGridViewTextBoxEditingControl
    Private WithEvents dgtxt_SizingDetails_Set2 As New DataGridViewTextBoxEditingControl
    Private WithEvents dgtxt_SizingDetails_Set3 As New DataGridViewTextBoxEditingControl

    Private WithEvents dgtxt_WarpingDetails_Set1 As New DataGridViewTextBoxEditingControl
    Private WithEvents dgtxt_WarpingDetails_Set2 As New DataGridViewTextBoxEditingControl
    Private WithEvents dgtxt_WarpingDetails_Set3 As New DataGridViewTextBoxEditingControl

    Private WithEvents dgtxt_BabyConeDetails As New DataGridViewTextBoxEditingControl
    Private WithEvents dgtxt_YarnTakenDetails As New DataGridViewTextBoxEditingControl
    Private WithEvents dgtxt_ChemicalDetails As New DataGridViewTextBoxEditingControl

    Private Siz_EmpRws_Ad_STS As Boolean

    Private Print_PDF_Status As Boolean = False

    Private StmtPrintFrmt As String = ""
    Private StmtPrint_InvDetails_Status As Boolean = False

    Private prn_Status As Integer
    Private prn_Pgno, SlNo As Integer
    Private TtSizBms As Single
    Private TtSizGrsWgt As Single, TtSizTrWgt As Single, TtSizNetWgt As Single
    Private TtSizPcs As Single, TtSizMtrs As Single



    Private prn_TotCopies As Integer = 0

    Private Sub clear()

        New_Entry = False
        Insert_Entry = False
        Print_PDF_Status = False

        pnl_Back.Enabled = True

        pnl_Filter.Visible = False
        lbl_SetNo.ForeColor = Color.Black

        txt_Remarks.Text = ""

        lbl_Total_Warping_Beams.Text = ""
        lbl_Total_Warping_NetWeight.Text = ""
        lbl_Total_Warping_TareWeight.Text = ""
        lbl_Total_Warping_GrossWeight.Text = ""
        lbl_Total_Warping_Ends.Text = ""
        dgv_WarpingDetails_Set1.Rows.Clear()
        dgv_WarpingDetails_Set1.Rows.Add()
        dgv_WarpingDetails_Total_Set1.Rows.Clear()
        dgv_WarpingDetails_Total_Set1.Rows.Add()

        dgv_WarpingDetails_Set2.Rows.Clear()
        dgv_WarpingDetails_Set2.Rows.Add()
        dgv_WarpingDetails_Total_Set2.Rows.Clear()
        dgv_WarpingDetails_Total_Set2.Rows.Add()

        dgv_WarpingDetails_Set3.Rows.Clear()
        dgv_WarpingDetails_Set3.Rows.Add()
        dgv_WarpingDetails_Total_Set3.Rows.Clear()
        dgv_WarpingDetails_Total_Set3.Rows.Add()

        dgv_BabyConeDetails.Enabled = True
        dgv_WarpingDetails_Set1.Enabled = True
        dgv_WarpingDetails_Set2.Enabled = True
        dgv_WarpingDetails_Set3.Enabled = True
        dgv_YarnTakenDetails.Enabled = True

        dgv_YarnTakenDetails.Rows.Clear()
        dgv_YarnTakenDetails_Total.Rows.Clear()
        dgv_YarnTakenDetails_Total.Rows.Add()
        lbl_BabyCone_NetWeight.Text = ""

        lbl_ExcessShort_GrsYarn.Text = ""
        txt_ExcessShort_AddLess.Text = ""
        txt_BabyCone_AddLessWgt.Text = ""

        dgv_BabyConeDetails.Rows.Clear()
        dgv_BabyConeDetails_Total.Rows.Clear()
        dgv_BabyConeDetails_Total.Rows.Add()
        cbo_Rw_MillName.Text = ""
        chk_RewindingStatus.Checked = False
        txt_RewindingCones.Text = ""
        txt_RwExcSht.Text = ""
        txt_BabyCone_TareWeight.Text = ""
        txt_BabyCone_AddLessWgt.Text = ""

        lbl_ExcessShort.Text = "0"
        cbo_Meters_Yards.Text = "YARDS"
        txt_WarpMeters.Text = "0"
        lbl_BeamCount.Text = "0"
        txt_PcsLength.Text = "0"
        txt_TapeLength.Text = ""
        cbo_BeamWidth.Text = ""
        cbo_MillName.Text = ""
        cbo_CountName.Text = ""
        txt_Ends.Text = "0"
        lbl_SetNo.Text = ""
        cbo_Ledger.Text = ""
        dtp_Date.Text = ""
        chk_SocietySet.Checked = False

        lbl_YarnTaken.Text = ""
        lbl_ConsumedYarn.Text = ""
        lbl_BabyConeWeight.Text = ""
        lbl_ExcessShort.Text = ""

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1102" Then
            txt_WgtEmYBag.Text = "0.160"
            txt_WgtEmYCone.Text = "0.052"
        Else
            txt_WgtEmYBag.Text = ""
            txt_WgtEmYCone.Text = ""
        End If

        cbo_Grid_CountName.Visible = False
        cbo_Grid_CountName.Text = ""

        cbo_Grid_CountName.Visible = False
        cbo_Grid_CountName.Text = ""

        cbo_Grid_MillName.Visible = False
        cbo_Grid_MillName.Text = ""

        cbo_Grid_FrontWarper2.Visible = False
        cbo_Grid_FrontWarper2.Text = ""

        cbo_Grid_FrontWarper_1.Visible = False
        cbo_Grid_FrontWarper_1.Text = ""

        cbo_Grid_BackWarper1.Visible = False
        cbo_Grid_BackWarper1.Text = ""

        cbo_Grid_BackWarper2.Visible = False
        cbo_Grid_BackWarper2.Text = ""

        cbo_Grid_Helper1.Visible = False
        cbo_Grid_Helper1.Text = ""

        cbo_Grid_Helper2.Visible = False
        cbo_Grid_Helper2.Text = ""

        cbo_Ends2_FrontWarper2.Visible = False
        cbo_Ends2_FrontWarper2.Text = ""

        cbo_Ends2_FrontWarper1.Visible = False
        cbo_Ends2_FrontWarper1.Text = ""

        cbo_Ends2_BackWarper1.Visible = False
        cbo_Ends2_BackWarper1.Text = ""

        cbo_Ends2_BackWarper2.Visible = False
        cbo_Ends2_BackWarper2.Text = ""

        cbo_Ends2_Helper1.Visible = False
        cbo_Ends2_Helper1.Text = ""

        cbo_Ends2_Helper2.Visible = False
        cbo_Ends2_Helper2.Text = ""

        cbo_Ends3_FrontWarper2.Visible = False
        cbo_Ends3_FrontWarper2.Text = ""

        cbo_Ends3_FrontWarper1.Visible = False
        cbo_Ends3_FrontWarper1.Text = ""

        cbo_Ends3_BackWarper1.Visible = False
        cbo_Ends3_BackWarper1.Text = ""

        cbo_Ends3_BackWarper2.Visible = False
        cbo_Ends3_BackWarper2.Text = ""

        cbo_Ends3_Helper1.Visible = False
        cbo_Ends3_Helper1.Text = ""

        cbo_Ends3_Helper2.Visible = False
        cbo_Ends3_Helper2.Text = ""


        cbo_Ends1_Shift.Visible = False
        cbo_Ends1_Shift.Text = ""

        cbo_Ends2_Shift.Visible = False
        cbo_Ends2_Shift.Text = ""

        Cbo_Ends3_Shift.Visible = False
        Cbo_Ends3_Shift.Text = ""




        chk_RewindingStatus.Checked = False
        txt_RewindingCones.Enabled = False
        cbo_Rw_MillName.Enabled = False

        txt_InvoiceCode.Text = ""
        txt_BabyCone_DeliveryWeight.Text = ""

        cbo_Ledger.Enabled = True
        cbo_Ledger.BackColor = Color.White

        cbo_CountName.Enabled = True
        cbo_CountName.BackColor = Color.White

        cbo_MillName.Enabled = True
        cbo_MillName.BackColor = Color.White


        cbo_BeamWidth.Enabled = True
        cbo_BeamWidth.BackColor = Color.White


        cbo_Meters_Yards.Enabled = True
        cbo_Meters_Yards.BackColor = Color.White


        txt_WarpMeters.Enabled = True
        txt_WarpMeters.BackColor = Color.White

        txt_TapeLength.Enabled = True
        txt_TapeLength.BackColor = Color.White

        txt_Ends.Enabled = True
        txt_Ends.BackColor = Color.White

        txt_BabyCone_TareWeight.Enabled = True
        txt_BabyCone_TareWeight.BackColor = Color.White

        txt_BabyCone_TareWeight.Enabled = True
        txt_BabyCone_TareWeight.BackColor = Color.White

        txt_BabyCone_AddLessWgt.Enabled = True
        txt_BabyCone_AddLessWgt.BackColor = Color.White

        txt_WgtEmYBag.Enabled = True
        txt_WgtEmYBag.BackColor = Color.White

        txt_WgtEmYCone.Enabled = True
        txt_WgtEmYCone.BackColor = Color.White

        chk_RewindingStatus.Enabled = True
        chk_RewindingStatus.BackColor = Color.White

        txt_RewindingCones.Enabled = True
        txt_RewindingCones.BackColor = Color.White

        cbo_Rw_MillName.Enabled = True
        cbo_Rw_MillName.BackColor = Color.White

        txt_RwExcSht.Enabled = True
        txt_RwExcSht.BackColor = Color.White

        If Filter_Status = False Then
            dtp_Filter_Fromdate.Text = Common_Procedures.Company_FromDate
            dtp_Filter_ToDate.Text = Common_Procedures.Company_ToDate
            cbo_Filter_PartyName.Text = ""
            cbo_Filter_CountName.Text = ""
            cbo_Filter_PartyName.SelectedIndex = -1
            cbo_Filter_CountName.SelectedIndex = -1
            dgv_Filter_Details.Rows.Clear()
        End If


        On Error Resume Next
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1163" Then
            tab_Main.SelectTab(0)
            tab_WarpingDeatils.SelectTab(0)
        End If

        'dgv_WarpingDetails_Set1.CurrentCell = dgv_WarpingDetails_Set1.Rows(0).Cells(1)
        dgv_WarpingDetails_Set1.CurrentCell.Selected = False

        Grid_Cell_DeSelect()
        lbl_UserName.Text = "USER : " & Trim(UCase(Common_Procedures.User_IdNoToName(con1, Common_Procedures.User.IdNo)))
    End Sub

    Private Sub ControlGotFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim txtbx As TextBox
        Dim combobx As ComboBox

        On Error Resume Next

        If TypeOf Me.ActiveControl Is TextBox Or TypeOf Me.ActiveControl Is ComboBox Or TypeOf Prec_ActCtrl Is CheckBox Then
            Me.ActiveControl.BackColor = Color.Lime
            Me.ActiveControl.ForeColor = Color.Blue
        ElseIf TypeOf Me.ActiveControl Is Button Then
            Me.ActiveControl.BackColor = Color.Lime
            Me.ActiveControl.ForeColor = Color.Blue
        ElseIf TypeOf Me.ActiveControl Is CheckBox Then
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

        If Me.ActiveControl.Name <> cbo_Grid_CountName.Name Then
            cbo_Grid_CountName.Visible = False
        End If
        If Me.ActiveControl.Name <> cbo_Grid_MillName.Name Then
            cbo_Grid_MillName.Visible = False
        End If
        If Me.ActiveControl.Name <> cbo_Grid_YarnType.Name Then
            cbo_Grid_YarnType.Visible = False
        End If


        If Me.ActiveControl.Name <> cbo_Grid_FrontWarper2.Name Then
            cbo_Grid_FrontWarper2.Visible = False
        End If
        If Me.ActiveControl.Name <> cbo_Grid_BackWarper1.Name Then
            cbo_Grid_BackWarper1.Visible = False
        End If
        If Me.ActiveControl.Name <> cbo_Grid_BackWarper2.Name Then
            cbo_Grid_BackWarper2.Visible = False
        End If
        If Me.ActiveControl.Name <> cbo_Grid_FrontWarper_1.Name Then
            cbo_Grid_FrontWarper_1.Visible = False
        End If

        If Me.ActiveControl.Name <> cbo_Grid_Helper1.Name Then
            cbo_Grid_Helper1.Visible = False
        End If
        If Me.ActiveControl.Name <> cbo_Grid_Helper2.Name Then
            cbo_Grid_Helper2.Visible = False
        End If

        If Me.ActiveControl.Name <> cbo_Ends2_Helper1.Name Then
            cbo_Ends2_Helper1.Visible = False
        End If
        If Me.ActiveControl.Name <> cbo_Ends2_Helper2.Name Then
            cbo_Ends2_Helper2.Visible = False
        End If

        If Me.ActiveControl.Name <> cbo_Ends2_FrontWarper2.Name Then
            cbo_Ends2_FrontWarper2.Visible = False
        End If
        If Me.ActiveControl.Name <> cbo_Ends2_BackWarper1.Name Then
            cbo_Ends2_BackWarper1.Visible = False
        End If
        If Me.ActiveControl.Name <> cbo_Ends2_BackWarper2.Name Then
            cbo_Ends2_BackWarper2.Visible = False
        End If
        If Me.ActiveControl.Name <> cbo_Ends2_FrontWarper1.Name Then
            cbo_Ends2_FrontWarper1.Visible = False
        End If

        If Me.ActiveControl.Name <> cbo_Ends3_Helper1.Name Then
            cbo_Ends3_Helper1.Visible = False
        End If
        If Me.ActiveControl.Name <> cbo_Ends3_Helper2.Name Then
            cbo_Ends3_Helper2.Visible = False
        End If

        If Me.ActiveControl.Name <> cbo_Ends3_FrontWarper2.Name Then
            cbo_Ends3_FrontWarper2.Visible = False
        End If
        If Me.ActiveControl.Name <> cbo_Ends3_BackWarper1.Name Then
            cbo_Ends3_BackWarper1.Visible = False
        End If
        If Me.ActiveControl.Name <> cbo_Ends3_BackWarper2.Name Then
            cbo_Ends3_BackWarper2.Visible = False
        End If
        If Me.ActiveControl.Name <> cbo_Ends3_FrontWarper1.Name Then
            cbo_Ends3_FrontWarper1.Visible = False
        End If

        If Me.ActiveControl.Name <> cbo_Ends1_Shift.Name Then
            cbo_Ends1_Shift.Visible = False
        End If

        If Me.ActiveControl.Name <> cbo_Ends2_Shift.Name Then
            cbo_Ends2_Shift.Visible = False
        End If

        If Me.ActiveControl.Name <> Cbo_Ends3_Shift.Name Then
            Cbo_Ends3_Shift.Visible = False
        End If



        Grid_Cell_DeSelect()

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
            ElseIf TypeOf Prec_ActCtrl Is CheckBox Then
                Prec_ActCtrl.BackColor = Color.LightSkyBlue
                Prec_ActCtrl.ForeColor = Color.Blue
            End If

        End If
    End Sub

    Private Sub TextBoxControlKeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs)
        On Error Resume Next
        If e.KeyValue = 38 Then e.Handled = True : SendKeys.Send("+{TAB}")
        If e.KeyValue = 40 Then e.Handled = True : SendKeys.Send("{TAB}")
    End Sub

    Private Sub TextBoxControlKeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
        On Error Resume Next
        If Asc(e.KeyChar) = 13 Then e.Handled = True : SendKeys.Send("{TAB}")
    End Sub

    Private Sub Grid_Cell_DeSelect()
        On Error Resume Next
        'dgv_AmountDetails.CurrentCell.Selected = False
        If Not IsNothing(dgv_BabyConeDetails.CurrentCell) Then dgv_BabyConeDetails.CurrentCell.Selected = False

        If Not IsNothing(dgv_YarnTakenDetails.CurrentCell) Then dgv_YarnTakenDetails.CurrentCell.Selected = False

        If Not IsNothing(dgv_WarpingDetails_Set1.CurrentCell) Then dgv_WarpingDetails_Set1.CurrentCell.Selected = False

    End Sub

    Private Sub AllGrid_Cell_DeSelect()
        On Error Resume Next

        'dgv_AmountDetails_Total.CurrentCell.Selected = False
        dgv_BabyConeDetails.CurrentCell.Selected = False

        'dgv_YarnStockSummary_Total.CurrentCell.Selected = False
        dgv_YarnTakenDetails.CurrentCell.Selected = False
        'dgv_YarnTakenDetails_Total.CurrentCell.Selected = False

        dgv_WarpingDetails_Set1.CurrentCell.Selected = False
        dgv_WarpingDetails_Set2.CurrentCell.Selected = False
        dgv_WarpingDetails_Set3.CurrentCell.Selected = False
        'dgv_WarpingDetails_Total_Set1.CurrentCell.Selected = False
        'dgv_WarpingDetails_Total_Set2.CurrentCell.Selected = False
        'dgv_WarpingDetails_Total_Set3.CurrentCell.Selected = False
    End Sub

    Private Sub move_record(ByVal no As String)
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim NewCode As String
        Dim i As Integer, j As Integer, n As Integer
        Dim SNo As Integer
        Dim Siz_Lck_STS As Boolean = False
        Dim LockSTS As Boolean = False
        If Val(no) = 0 Then Exit Sub

        clear()

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(no) & "/" & Trim(Common_Procedures.FnYearCode)

        Try

            da1 = New SqlClient.SqlDataAdapter("select a.*, b.Ledger_Name, c.mill_name, d.count_name, e.Beam_Width_Name from Warping_Head a INNER JOIN Ledger_Head b ON a.Ledger_IdNo = b.Ledger_IdNo LEFT OUTER JOIN Mill_Head c ON a.Mill_IdNo = c.Mill_IdNo LEFT OUTER JOIN count_Head d ON a.count_IdNo = d.count_IdNo LEFT OUTER JOIN Beam_Width_Head e ON a.Beam_Width_IdNo = e.Beam_Width_IdNo Where a.Warp_Code = '" & Trim(NewCode) & "'", Con)
            dt1 = New DataTable
            da1.Fill(dt1)

            If dt1.Rows.Count > 0 Then
                lbl_SetNo.Text = dt1.Rows(0).Item("Warp_No").ToString
                dtp_Date.Text = dt1.Rows(0).Item("Warp_Date").ToString

                cbo_Ledger.Text = dt1.Rows(0).Item("Ledger_Name").ToString
                cbo_CountName.Text = dt1.Rows(0).Item("count_name").ToString

                cbo_MillName.Tag = dt1.Rows(0).Item("mill_name").ToString
                cbo_MillName.Text = dt1.Rows(0).Item("mill_name").ToString

                cbo_BeamWidth.Text = dt1.Rows(0).Item("Beam_Width_Name").ToString
                txt_Ends.Text = dt1.Rows(0).Item("ends_name").ToString
                txt_PcsLength.Text = dt1.Rows(0).Item("pcs_length").ToString

                cbo_Location.Text = Common_Procedures.Ledger_IdNoToName(Con, Val(dt1.Rows(0).Item("BabyCone_Location_IdNo").ToString))
                cbo_DeliveryTo.Text = Common_Procedures.Ledger_IdNoToName(Con, Val(dt1.Rows(0).Item("BabyCone_Delivery_To_IdNo").ToString))

                If Val(dt1.Rows(0).Item("NoNeedPostingFlag").ToString) Then
                    chk_NoNeedPosting.Checked = True
                Else
                    chk_NoNeedPosting.Checked = False
                End If

                txt_TapeLength.Text = Val(dt1.Rows(0).Item("tape_length").ToString)

                cbo_Meters_Yards.Text = dt1.Rows(0).Item("Meters_Yards").ToString
                txt_WarpMeters.Text = dt1.Rows(0).Item("warp_meters").ToString

                lbl_BeamCount.Text = dt1.Rows(0).Item("beam_count").ToString


                txt_Remarks.Text = dt1.Rows(0).Item("remarks").ToString

                txt_WgtEmYBag.Text = Format(Val(dt1.Rows(0).Item("EmptyBag_weight").ToString), "#########0.000")
                txt_WgtEmYCone.Text = Format(Val(dt1.Rows(0).Item("EmptyCone_weight").ToString), "#########0.000")

                txt_BabyCone_TareWeight.Text = Format(Val(dt1.Rows(0).Item("total_baby_tare_weight").ToString), "#########0.000")

                lbl_BabyCone_NetWeight.Text = Format(Val(dt1.Rows(0).Item("total_baby_net_weight").ToString), "#########0.000")
                lbl_UserName.Text = "USER : " & Trim(UCase(Common_Procedures.User_IdNoToName(con1, Val(dt1.Rows(0).Item("User_IdNo").ToString))))


                If Val(dt1.Rows(0).Item("rw_status").ToString) = 1 Then
                    chk_RewindingStatus.Checked = True
                End If

                lbl_Total_Warping_Beams.Text = dt1.Rows(0).Item("Total_Warping_Beams").ToString
                lbl_Total_Warping_Ends.Text = dt1.Rows(0).Item("total_warping_ends").ToString
                lbl_Total_Warping_GrossWeight.Text = Format(Val(dt1.Rows(0).Item("total_warping_gross_weight").ToString), "#########0.000")
                lbl_Total_Warping_TareWeight.Text = Format(Val(dt1.Rows(0).Item("total_warping_tare_weight").ToString), "#########0.000")
                lbl_Total_Warping_NetWeight.Text = Format(Val(dt1.Rows(0).Item("total_warping_net_weight").ToString), "#########0.000")

                If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1102" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1144" Then

                    lbl_ConsumedYarn.Text = Format(Val(lbl_Total_Warping_NetWeight.Text), "#########0.0")
                    lbl_BabyConeWeight.Text = Format(Val(lbl_BabyCone_NetWeight.Text), "#########0")
                    txt_BabyCone_AddLessWgt.Text = Format(Val(dt1.Rows(0).Item("total_baby_tare_AddLessweight").ToString), "#########0.0")

                    lbl_ExcessShort_GrsYarn.Text = Format(Val(dt1.Rows(0).Item("excess_short_Grossyarn").ToString), "#########0.0")
                    txt_ExcessShort_AddLess.Text = Format(Val(dt1.Rows(0).Item("excess_short_AddLessyarn").ToString), "#########0.0")
                    lbl_ExcessShort.Text = Format(Val(dt1.Rows(0).Item("excess_short_yarn").ToString), "#########0.0")

                Else

                    lbl_ConsumedYarn.Text = Format(Val(lbl_Total_Warping_NetWeight.Text), "#########0.000")
                    lbl_BabyConeWeight.Text = Format(Val(lbl_BabyCone_NetWeight.Text), "#########0.000")
                    lbl_ExcessShort.Text = Format(Val(dt1.Rows(0).Item("excess_short_yarn").ToString), "#########0.000")

                End If

                If Val(dt1.Rows(0).Item("SocietySet_Status").ToString) = 1 Then
                    chk_SocietySet.Checked = True
                End If

                txt_InvoiceCode.Text = dt1.Rows(0).Item("invoice_code").ToString

                txt_BabyCone_DeliveryWeight.Text = 0
                da2 = New SqlClient.SqlDataAdapter("select sum(Delivered_Weight) as Delivered_BabyCone_Weight from Stock_BabyCone_Processing_Details a where a.Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' and a.Warp_Code = '" & Trim(NewCode) & "'", Con)
                dt2 = New DataTable
                da2.Fill(dt2)
                If dt2.Rows.Count > 0 Then
                    If IsDBNull(dt2.Rows(0).Item("Delivered_BabyCone_Weight").ToString) = False Then
                        txt_BabyCone_DeliveryWeight.Text = Val(txt_BabyCone_DeliveryWeight.Text) + Val(dt2.Rows(0).Item("Delivered_BabyCone_Weight").ToString)
                    End If
                End If
                dt2.Clear()

                da2 = New SqlClient.SqlDataAdapter("select sum(RwDelivered_Weight) as RwDelivered_BabyCone_Weight from Stock_RewindingBabyCone_Processing_Details a where a.Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' and a.Warp_Code = '" & Trim(NewCode) & "'", Con)
                dt2 = New DataTable
                da2.Fill(dt2)
                If dt2.Rows.Count > 0 Then
                    If IsDBNull(dt2.Rows(0).Item("RwDelivered_BabyCone_Weight").ToString) = False Then
                        txt_BabyCone_DeliveryWeight.Text = Val(txt_BabyCone_DeliveryWeight.Text) + Val(dt2.Rows(0).Item("RwDelivered_BabyCone_Weight").ToString)
                    End If
                End If
                dt2.Clear()

                If IsDBNull(dt1.Rows(0).Item("Set_Code").ToString) = False Then
                    If Trim(dt1.Rows(0).Item("Set_Code").ToString) <> "" Then
                        LockSTS = True
                    End If
                End If

                da2 = New SqlClient.SqlDataAdapter("select a.* from Warping_Details a where a.Warp_Code = '" & Trim(NewCode) & "' and a.Warp_SlNo = 1 Order by a.sl_no", Con)
                dt2 = New DataTable
                da2.Fill(dt2)

                dgv_WarpingDetails_Set1.Rows.Clear()
                SNo = 0

                If dt2.Rows.Count > 0 Then

                    For i = 0 To dt2.Rows.Count - 1

                        n = dgv_WarpingDetails_Set1.Rows.Add()

                        SNo = SNo + 1
                        dgv_WarpingDetails_Set1.Rows(n).Cells(0).Value = Val(SNo)
                        dgv_WarpingDetails_Set1.Rows(n).Cells(1).Value = dt2.Rows(i).Item("Ends_Name").ToString
                        dgv_WarpingDetails_Set1.Rows(n).Cells(2).Value = Format(Val(dt2.Rows(i).Item("Gross_Weight").ToString), "#########0.0")

                        dgv_WarpingDetails_Set1.Rows(n).Cells(3).Value = Format(Val(dt2.Rows(i).Item("Tare_Weight").ToString), "#########0.0")
                        If Val(dgv_WarpingDetails_Set1.Rows(n).Cells(3).Value) = 0 Then dgv_WarpingDetails_Set1.Rows(n).Cells(3).Value = ""

                        dgv_WarpingDetails_Set1.Rows(n).Cells(4).Value = Format(Val(dt2.Rows(i).Item("Net_Weight").ToString), "#########0.0")
                        dgv_WarpingDetails_Set1.Rows(n).Cells(5).Value = dt2.Rows(i).Item("Shift").ToString
                        dgv_WarpingDetails_Set1.Rows(n).Cells(6).Value = dt2.Rows(i).Item("Warp_beam_No").ToString

                        dgv_WarpingDetails_Set1.Rows(n).Cells(7).Value = Format(Val(dt2.Rows(i).Item("Start_Time").ToString), "#########0.00")
                        If Val(dgv_WarpingDetails_Set1.Rows(n).Cells(7).Value) = 0 Then dgv_WarpingDetails_Set1.Rows(n).Cells(7).Value = ""

                        dgv_WarpingDetails_Set1.Rows(n).Cells(8).Value = Format(Val(dt2.Rows(i).Item("End_Time").ToString), "#########0.00")
                        If Val(dgv_WarpingDetails_Set1.Rows(n).Cells(8).Value) = 0 Then dgv_WarpingDetails_Set1.Rows(n).Cells(8).Value = ""

                        dgv_WarpingDetails_Set1.Rows(n).Cells(9).Value = Format(Val(dt2.Rows(i).Item("Total_Time").ToString), "#########0.00")
                        If Val(dgv_WarpingDetails_Set1.Rows(n).Cells(9).Value) = 0 Then dgv_WarpingDetails_Set1.Rows(n).Cells(9).Value = ""

                        dgv_WarpingDetails_Set1.Rows(n).Cells(10).Value = Common_Procedures.Employee_IdNoToName(Con, dt2.Rows(i).Item("Front_Warper_1_IdNo").ToString)
                        dgv_WarpingDetails_Set1.Rows(n).Cells(11).Value = Format(Val(dt2.Rows(i).Item("Front_Warper_1_Meters").ToString), "#########0.00")
                        If Val(dgv_WarpingDetails_Set1.Rows(n).Cells(11).Value) = 0 Then dgv_WarpingDetails_Set1.Rows(n).Cells(11).Value = ""

                        dgv_WarpingDetails_Set1.Rows(n).Cells(12).Value = Common_Procedures.Employee_IdNoToName(Con, dt2.Rows(i).Item("Back_Warper_1_IdNo").ToString)
                        dgv_WarpingDetails_Set1.Rows(n).Cells(13).Value = Format(Val(dt2.Rows(i).Item("Back_Warper_1_Meters").ToString), "#########0.00")
                        If Val(dgv_WarpingDetails_Set1.Rows(n).Cells(13).Value) = 0 Then dgv_WarpingDetails_Set1.Rows(n).Cells(13).Value = ""

                        dgv_WarpingDetails_Set1.Rows(n).Cells(14).Value = Common_Procedures.Employee_IdNoToName(Con, dt2.Rows(i).Item("Helper_1_IdNo").ToString)
                        dgv_WarpingDetails_Set1.Rows(n).Cells(15).Value = Format(Val(dt2.Rows(i).Item("Helper_1_Meters").ToString), "#########0.00")
                        If Val(dgv_WarpingDetails_Set1.Rows(n).Cells(15).Value) = 0 Then dgv_WarpingDetails_Set1.Rows(n).Cells(15).Value = ""

                        dgv_WarpingDetails_Set1.Rows(n).Cells(16).Value = Common_Procedures.Employee_IdNoToName(Con, dt2.Rows(i).Item("Front_Warper_2_IdNo").ToString)
                        dgv_WarpingDetails_Set1.Rows(n).Cells(17).Value = Format(Val(dt2.Rows(i).Item("Front_Warper_2_Meters").ToString), "#########0.00")
                        If Val(dgv_WarpingDetails_Set1.Rows(n).Cells(17).Value) = 0 Then dgv_WarpingDetails_Set1.Rows(n).Cells(17).Value = ""

                        dgv_WarpingDetails_Set1.Rows(n).Cells(18).Value = Common_Procedures.Employee_IdNoToName(Con, dt2.Rows(i).Item("Back_Warper_2_IdNo").ToString)
                        dgv_WarpingDetails_Set1.Rows(n).Cells(19).Value = Format(Val(dt2.Rows(i).Item("Back_Warper_2_Meters").ToString), "#########0.00")
                        If Val(dgv_WarpingDetails_Set1.Rows(n).Cells(19).Value) = 0 Then dgv_WarpingDetails_Set1.Rows(n).Cells(19).Value = ""

                        dgv_WarpingDetails_Set1.Rows(n).Cells(20).Value = Common_Procedures.Employee_IdNoToName(Con, dt2.Rows(i).Item("Helper_2_IdNo").ToString)
                        dgv_WarpingDetails_Set1.Rows(n).Cells(21).Value = Format(Val(dt2.Rows(i).Item("Helper_2_Meters").ToString), "#########0.00")
                        If Val(dgv_WarpingDetails_Set1.Rows(n).Cells(21).Value) = 0 Then dgv_WarpingDetails_Set1.Rows(n).Cells(21).Value = ""

                        dgv_WarpingDetails_Set1.Rows(n).Cells(22).Value = dt2.Rows(i).Item("Remarks").ToString

                        If Trim(txt_InvoiceCode.Text) <> "" Then
                            For j = 0 To dgv_WarpingDetails_Set1.ColumnCount - 1
                                dgv_WarpingDetails_Set1.Rows(n).Cells(j).Style.BackColor = Color.LightGray
                            Next
                        End If

                        If Trim(dt1.Rows(0).Item("Set_Code").ToString) <> "" Then
                            For j = 0 To dgv_WarpingDetails_Set1.ColumnCount - 1
                                dgv_WarpingDetails_Set1.Rows(n).Cells(j).Style.BackColor = Color.LightGray
                            Next
                            LockSTS = True
                        End If
                    Next i

                End If

                dgv_WarpingDetails_Set1.Rows.Add()

                da2 = New SqlClient.SqlDataAdapter("select a.* from Warping_Details a where a.Warp_Code = '" & Trim(NewCode) & "' and a.Warp_SlNo = 2 Order by a.sl_no", Con)
                dt2 = New DataTable
                da2.Fill(dt2)

                dgv_WarpingDetails_Set2.Rows.Clear()
                SNo = 0

                If dt2.Rows.Count > 0 Then

                    For i = 0 To dt2.Rows.Count - 1

                        n = dgv_WarpingDetails_Set2.Rows.Add()

                        SNo = SNo + 1
                        dgv_WarpingDetails_Set2.Rows(n).Cells(0).Value = Val(SNo)
                        dgv_WarpingDetails_Set2.Rows(n).Cells(1).Value = Val(dt2.Rows(i).Item("Ends_Name").ToString)
                        dgv_WarpingDetails_Set2.Rows(n).Cells(2).Value = Format(Val(dt2.Rows(i).Item("Gross_Weight").ToString), "#########0.0")
                        dgv_WarpingDetails_Set2.Rows(n).Cells(3).Value = Format(Val(dt2.Rows(i).Item("Tare_Weight").ToString), "#########0.0")
                        dgv_WarpingDetails_Set2.Rows(n).Cells(4).Value = Format(Val(dt2.Rows(i).Item("Net_Weight").ToString), "#########0.0")
                        dgv_WarpingDetails_Set2.Rows(n).Cells(5).Value = dt2.Rows(i).Item("Shift").ToString
                        dgv_WarpingDetails_Set2.Rows(n).Cells(6).Value = dt2.Rows(i).Item("Warp_beam_No").ToString

                        dgv_WarpingDetails_Set2.Rows(n).Cells(7).Value = Format(Val(dt2.Rows(i).Item("Start_Time").ToString), "#########0.00")
                        If Val(dgv_WarpingDetails_Set2.Rows(n).Cells(7).Value) = 0 Then dgv_WarpingDetails_Set2.Rows(n).Cells(7).Value = ""

                        dgv_WarpingDetails_Set2.Rows(n).Cells(8).Value = Format(Val(dt2.Rows(i).Item("End_Time").ToString), "#########0.00")
                        If Val(dgv_WarpingDetails_Set2.Rows(n).Cells(8).Value) = 0 Then dgv_WarpingDetails_Set2.Rows(n).Cells(8).Value = ""

                        dgv_WarpingDetails_Set2.Rows(n).Cells(9).Value = Format(Val(dt2.Rows(i).Item("Total_Time").ToString), "#########0.00")
                        If Val(dgv_WarpingDetails_Set2.Rows(n).Cells(9).Value) = 0 Then dgv_WarpingDetails_Set2.Rows(n).Cells(9).Value = ""

                        dgv_WarpingDetails_Set2.Rows(n).Cells(10).Value = Common_Procedures.Employee_IdNoToName(Con, dt2.Rows(i).Item("Front_Warper_1_IdNo").ToString)
                        dgv_WarpingDetails_Set2.Rows(n).Cells(11).Value = Format(Val(dt2.Rows(i).Item("Front_Warper_1_Meters").ToString), "#########0.00")
                        If Val(dgv_WarpingDetails_Set2.Rows(n).Cells(11).Value) = 0 Then dgv_WarpingDetails_Set2.Rows(n).Cells(11).Value = ""

                        dgv_WarpingDetails_Set2.Rows(n).Cells(12).Value = Common_Procedures.Employee_IdNoToName(Con, dt2.Rows(i).Item("Back_Warper_1_IdNo").ToString)
                        dgv_WarpingDetails_Set2.Rows(n).Cells(13).Value = Format(Val(dt2.Rows(i).Item("Back_Warper_1_Meters").ToString), "#########0.00")
                        If Val(dgv_WarpingDetails_Set2.Rows(n).Cells(13).Value) = 0 Then dgv_WarpingDetails_Set2.Rows(n).Cells(13).Value = ""

                        dgv_WarpingDetails_Set2.Rows(n).Cells(14).Value = Common_Procedures.Employee_IdNoToName(Con, dt2.Rows(i).Item("Helper_1_IdNo").ToString)
                        dgv_WarpingDetails_Set2.Rows(n).Cells(15).Value = Format(Val(dt2.Rows(i).Item("Helper_1_Meters").ToString), "#########0.00")
                        If Val(dgv_WarpingDetails_Set2.Rows(n).Cells(15).Value) = 0 Then dgv_WarpingDetails_Set2.Rows(n).Cells(15).Value = ""

                        dgv_WarpingDetails_Set2.Rows(n).Cells(16).Value = Common_Procedures.Employee_IdNoToName(Con, dt2.Rows(i).Item("Front_Warper_2_IdNo").ToString)
                        dgv_WarpingDetails_Set2.Rows(n).Cells(17).Value = Format(Val(dt2.Rows(i).Item("Front_Warper_2_Meters").ToString), "#########0.00")
                        If Val(dgv_WarpingDetails_Set2.Rows(n).Cells(17).Value) = 0 Then dgv_WarpingDetails_Set2.Rows(n).Cells(17).Value = ""

                        dgv_WarpingDetails_Set2.Rows(n).Cells(18).Value = Common_Procedures.Employee_IdNoToName(Con, dt2.Rows(i).Item("Back_Warper_2_IdNo").ToString)
                        dgv_WarpingDetails_Set2.Rows(n).Cells(19).Value = Format(Val(dt2.Rows(i).Item("Back_Warper_2_Meters").ToString), "#########0.00")
                        If Val(dgv_WarpingDetails_Set2.Rows(n).Cells(19).Value) = 0 Then dgv_WarpingDetails_Set2.Rows(n).Cells(19).Value = ""

                        dgv_WarpingDetails_Set2.Rows(n).Cells(20).Value = Common_Procedures.Employee_IdNoToName(Con, dt2.Rows(i).Item("Helper_2_IdNo").ToString)
                        dgv_WarpingDetails_Set2.Rows(n).Cells(21).Value = Format(Val(dt2.Rows(i).Item("Helper_2_Meters").ToString), "#########0.00")
                        If Val(dgv_WarpingDetails_Set2.Rows(n).Cells(21).Value) = 0 Then dgv_WarpingDetails_Set2.Rows(n).Cells(21).Value = ""

                        dgv_WarpingDetails_Set2.Rows(n).Cells(22).Value = dt2.Rows(i).Item("Remarks").ToString
                        If Trim(txt_InvoiceCode.Text) <> "" Then
                            For j = 0 To dgv_WarpingDetails_Set2.ColumnCount - 1
                                dgv_WarpingDetails_Set2.Rows(n).Cells(j).Style.BackColor = Color.LightGray
                            Next
                        End If
                        If Trim(dt1.Rows(0).Item("Set_Code").ToString) <> "" Then
                            For j = 0 To dgv_WarpingDetails_Set2.ColumnCount - 1
                                dgv_WarpingDetails_Set2.Rows(n).Cells(j).Style.BackColor = Color.LightGray
                            Next
                            LockSTS = True
                        End If
                    Next i

                End If
                dgv_WarpingDetails_Set2.Rows.Add()


                da2 = New SqlClient.SqlDataAdapter("select a.* from Warping_Details a where a.Warp_Code = '" & Trim(NewCode) & "' and a.Warp_SlNo = 3 Order by a.sl_no", Con)
                dt2 = New DataTable
                da2.Fill(dt2)

                dgv_WarpingDetails_Set3.Rows.Clear()
                SNo = 0

                If dt2.Rows.Count > 0 Then

                    For i = 0 To dt2.Rows.Count - 1

                        n = dgv_WarpingDetails_Set3.Rows.Add()

                        SNo = SNo + 1
                        dgv_WarpingDetails_Set3.Rows(n).Cells(0).Value = Val(SNo)
                        dgv_WarpingDetails_Set3.Rows(n).Cells(1).Value = Val(dt2.Rows(i).Item("Ends_Name").ToString)
                        dgv_WarpingDetails_Set3.Rows(n).Cells(2).Value = Format(Val(dt2.Rows(i).Item("Gross_Weight").ToString), "#########0.0")

                        dgv_WarpingDetails_Set3.Rows(n).Cells(3).Value = Format(Val(dt2.Rows(i).Item("Tare_Weight").ToString), "#########0.0")
                        If Val(dgv_WarpingDetails_Set3.Rows(n).Cells(3).Value) = 0 Then dgv_WarpingDetails_Set3.Rows(n).Cells(3).Value = ""

                        dgv_WarpingDetails_Set3.Rows(n).Cells(4).Value = Format(Val(dt2.Rows(i).Item("Net_Weight").ToString), "#########0.0")
                        dgv_WarpingDetails_Set3.Rows(n).Cells(5).Value = dt2.Rows(i).Item("Shift").ToString
                        dgv_WarpingDetails_Set3.Rows(n).Cells(6).Value = dt2.Rows(i).Item("Warp_beam_No").ToString
                        dgv_WarpingDetails_Set3.Rows(n).Cells(7).Value = Format(Val(dt2.Rows(i).Item("Start_Time").ToString), "#########0.00")
                        If Val(dgv_WarpingDetails_Set3.Rows(n).Cells(7).Value) = 0 Then dgv_WarpingDetails_Set3.Rows(n).Cells(7).Value = ""

                        dgv_WarpingDetails_Set3.Rows(n).Cells(8).Value = Format(Val(dt2.Rows(i).Item("End_Time").ToString), "#########0.00")
                        If Val(dgv_WarpingDetails_Set3.Rows(n).Cells(8).Value) = 0 Then dgv_WarpingDetails_Set3.Rows(n).Cells(8).Value = ""

                        dgv_WarpingDetails_Set3.Rows(n).Cells(9).Value = Format(Val(dt2.Rows(i).Item("Total_Time").ToString), "#########0.00")
                        If Val(dgv_WarpingDetails_Set3.Rows(n).Cells(9).Value) = 0 Then dgv_WarpingDetails_Set3.Rows(n).Cells(9).Value = ""

                        dgv_WarpingDetails_Set3.Rows(n).Cells(10).Value = Common_Procedures.Employee_IdNoToName(Con, dt2.Rows(i).Item("Front_Warper_1_IdNo").ToString)
                        dgv_WarpingDetails_Set3.Rows(n).Cells(11).Value = Format(Val(dt2.Rows(i).Item("Front_Warper_1_Meters").ToString), "#########0.00")
                        If Val(dgv_WarpingDetails_Set3.Rows(n).Cells(11).Value) = 0 Then dgv_WarpingDetails_Set3.Rows(n).Cells(11).Value = ""

                        dgv_WarpingDetails_Set3.Rows(n).Cells(12).Value = Common_Procedures.Employee_IdNoToName(Con, dt2.Rows(i).Item("Back_Warper_1_IdNo").ToString)
                        dgv_WarpingDetails_Set3.Rows(n).Cells(13).Value = Format(Val(dt2.Rows(i).Item("Back_Warper_1_Meters").ToString), "#########0.00")
                        If Val(dgv_WarpingDetails_Set3.Rows(n).Cells(13).Value) = 0 Then dgv_WarpingDetails_Set3.Rows(n).Cells(13).Value = ""

                        dgv_WarpingDetails_Set3.Rows(n).Cells(14).Value = Common_Procedures.Employee_IdNoToName(Con, dt2.Rows(i).Item("Helper_1_IdNo").ToString)
                        dgv_WarpingDetails_Set3.Rows(n).Cells(15).Value = Format(Val(dt2.Rows(i).Item("Helper_1_Meters").ToString), "#########0.00")
                        If Val(dgv_WarpingDetails_Set3.Rows(n).Cells(15).Value) = 0 Then dgv_WarpingDetails_Set3.Rows(n).Cells(15).Value = ""

                        dgv_WarpingDetails_Set3.Rows(n).Cells(16).Value = Common_Procedures.Employee_IdNoToName(Con, dt2.Rows(i).Item("Front_Warper_2_IdNo").ToString)
                        dgv_WarpingDetails_Set3.Rows(n).Cells(17).Value = Format(Val(dt2.Rows(i).Item("Front_Warper_2_Meters").ToString), "#########0.00")
                        If Val(dgv_WarpingDetails_Set3.Rows(n).Cells(17).Value) = 0 Then dgv_WarpingDetails_Set3.Rows(n).Cells(17).Value = ""

                        dgv_WarpingDetails_Set3.Rows(n).Cells(18).Value = Common_Procedures.Employee_IdNoToName(Con, dt2.Rows(i).Item("Back_Warper_2_IdNo").ToString)
                        dgv_WarpingDetails_Set3.Rows(n).Cells(19).Value = Format(Val(dt2.Rows(i).Item("Back_Warper_2_Meters").ToString), "#########0.00")
                        If Val(dgv_WarpingDetails_Set3.Rows(n).Cells(19).Value) = 0 Then dgv_WarpingDetails_Set3.Rows(n).Cells(19).Value = ""

                        dgv_WarpingDetails_Set3.Rows(n).Cells(20).Value = Common_Procedures.Employee_IdNoToName(Con, dt2.Rows(i).Item("Helper_2_IdNo").ToString)
                        dgv_WarpingDetails_Set3.Rows(n).Cells(21).Value = Format(Val(dt2.Rows(i).Item("Helper_2_Meters").ToString), "#########0.00")
                        If Val(dgv_WarpingDetails_Set3.Rows(n).Cells(21).Value) = 0 Then dgv_WarpingDetails_Set3.Rows(n).Cells(21).Value = ""

                        dgv_WarpingDetails_Set3.Rows(n).Cells(22).Value = dt2.Rows(i).Item("Remarks").ToString

                        If Trim(txt_InvoiceCode.Text) <> "" Then
                            For j = 0 To dgv_WarpingDetails_Set3.ColumnCount - 1
                                dgv_WarpingDetails_Set3.Rows(n).Cells(j).Style.BackColor = Color.LightGray
                            Next
                        End If

                        If Trim(dt1.Rows(0).Item("Set_Code").ToString) <> "" Then
                            For j = 0 To dgv_WarpingDetails_Set3.ColumnCount - 1
                                dgv_WarpingDetails_Set3.Rows(n).Cells(j).Style.BackColor = Color.LightGray
                            Next
                            LockSTS = True
                        End If
                    Next i

                End If

                dgv_WarpingDetails_Set3.Rows.Add()

                TotalWarping_Calculation()


                da2 = New SqlClient.SqlDataAdapter("select a.*, b.Count_Name, c.Mill_Name from Warping_YarnTaken_Details a INNER JOIN Count_Head b on a.Count_IdNo = b.Count_IdNo INNER JOIN Mill_Head c on a.Mill_IdNo = c.Mill_IdNo where a.Warp_Code = '" & Trim(NewCode) & "' Order by a.sl_no", Con)
                dt2 = New DataTable
                da2.Fill(dt2)

                dgv_YarnTakenDetails.Rows.Clear()
                SNo = 0

                If dt2.Rows.Count > 0 Then

                    For i = 0 To dt2.Rows.Count - 1

                        n = dgv_YarnTakenDetails.Rows.Add()

                        SNo = SNo + 1
                        dgv_YarnTakenDetails.Rows(n).Cells(0).Value = Val(SNo)
                        dgv_YarnTakenDetails.Rows(n).Cells(1).Value = dt2.Rows(i).Item("Count_Name").ToString
                        dgv_YarnTakenDetails.Rows(n).Cells(2).Value = dt2.Rows(i).Item("Yarn_Type").ToString
                        dgv_YarnTakenDetails.Rows(n).Cells(3).Value = dt2.Rows(i).Item("BabyCone_Warpcode_forSelection").ToString
                        dgv_YarnTakenDetails.Rows(n).Cells(4).Value = dt2.Rows(i).Item("Mill_Name").ToString

                        dgv_YarnTakenDetails.Rows(n).Cells(5).Value = Val(dt2.Rows(i).Item("Bags").ToString)
                        If Val(dgv_YarnTakenDetails.Rows(n).Cells(5).Value) = 0 Then dgv_YarnTakenDetails.Rows(n).Cells(5).Value = ""

                        dgv_YarnTakenDetails.Rows(n).Cells(6).Value = Format(Val(dt2.Rows(i).Item("Weight_Cone").ToString), "########0.000")
                        If Val(dgv_YarnTakenDetails.Rows(n).Cells(6).Value) = 0 Then dgv_YarnTakenDetails.Rows(n).Cells(6).Value = ""

                        dgv_YarnTakenDetails.Rows(n).Cells(7).Value = Val(dt2.Rows(i).Item("Cones").ToString)
                        If Val(dgv_YarnTakenDetails.Rows(n).Cells(7).Value) = 0 Then dgv_YarnTakenDetails.Rows(n).Cells(7).Value = ""

                        dgv_YarnTakenDetails.Rows(n).Cells(8).Value = Format(Val(dt2.Rows(i).Item("Gross_Weight").ToString), "########0.000")
                        dgv_YarnTakenDetails.Rows(n).Cells(9).Value = Format(Val(dt2.Rows(i).Item("Tare_Weight").ToString), "########0.000")
                        dgv_YarnTakenDetails.Rows(n).Cells(10).Value = Format(Val(dt2.Rows(i).Item("Weight").ToString), "########0.000")

                        dgv_YarnTakenDetails.Rows(n).Cells(11).Value = Common_Procedures.Ledger_IdNoToName(Con, Val(dt2.Rows(i).Item("Location_IdNo").ToString))
                        dgv_YarnTakenDetails.Rows(n).Cells(12).Value = dt2.Rows(i).Item("Lot_No").ToString

                        If Trim(dt1.Rows(0).Item("Set_Code").ToString) <> "" Then
                            For j = 0 To dgv_YarnTakenDetails.ColumnCount - 1
                                dgv_YarnTakenDetails.Rows(n).Cells(j).Style.BackColor = Color.LightGray
                            Next
                            LockSTS = True
                        End If

                    Next i

                End If

                TotalYarnTaken_Calculation()

                da2 = New SqlClient.SqlDataAdapter("select a.* from Warping_BabyCone_Details a where a.Warp_Code = '" & Trim(NewCode) & "' Order by a.sl_no", Con)
                dt2 = New DataTable
                da2.Fill(dt2)

                dgv_BabyConeDetails.Rows.Clear()
                SNo = 0

                If dt2.Rows.Count > 0 Then

                    For i = 0 To dt2.Rows.Count - 1

                        n = dgv_BabyConeDetails.Rows.Add()

                        SNo = SNo + 1
                        dgv_BabyConeDetails.Rows(n).Cells(0).Value = Val(SNo)
                        dgv_BabyConeDetails.Rows(n).Cells(1).Value = Val(dt2.Rows(i).Item("Bags").ToString)
                        If Val(dgv_BabyConeDetails.Rows(n).Cells(1).Value) = 0 Then dgv_BabyConeDetails.Rows(n).Cells(1).Value = ""

                        dgv_BabyConeDetails.Rows(n).Cells(2).Value = Val(dt2.Rows(i).Item("Cones").ToString)
                        If Val(dgv_BabyConeDetails.Rows(n).Cells(2).Value) = 0 Then dgv_BabyConeDetails.Rows(n).Cells(2).Value = ""

                        dgv_BabyConeDetails.Rows(n).Cells(3).Value = Format(Val(dt2.Rows(i).Item("Gross_Weight").ToString), "#########0.000")

                        dgv_BabyConeDetails.Rows(n).Cells(4).Value = Format(Val(dt2.Rows(i).Item("Tare_Weight").ToString), "########0.000")
                        dgv_BabyConeDetails.Rows(n).Cells(5).Value = Format(Val(dt2.Rows(i).Item("Net_Weight").ToString), "########0.000")
                        dgv_BabyConeDetails.Rows(n).Cells(6).Value = dt2.Rows(i).Item("Lot_No").ToString

                        If Val(txt_BabyCone_DeliveryWeight.Text) <> 0 Then
                            For j = 0 To dgv_BabyConeDetails.ColumnCount - 1
                                dgv_BabyConeDetails.Rows(n).Cells(j).Style.BackColor = Color.LightGray
                            Next
                        End If
                        If Trim(dt1.Rows(0).Item("Set_Code").ToString) <> "" Then
                            For j = 0 To dgv_BabyConeDetails.ColumnCount - 1
                                dgv_BabyConeDetails.Rows(n).Cells(j).Style.BackColor = Color.LightGray
                            Next
                            LockSTS = True
                        End If
                    Next i

                End If

                If Val(txt_BabyCone_DeliveryWeight.Text) <> 0 Then
                    txt_BabyCone_TareWeight.Enabled = False
                    txt_BabyCone_TareWeight.BackColor = Color.LightGray

                    chk_RewindingStatus.Enabled = False
                    chk_RewindingStatus.BackColor = Color.LightGray

                    txt_RewindingCones.Enabled = False
                    txt_RewindingCones.BackColor = Color.LightGray

                    cbo_Rw_MillName.Enabled = False
                    cbo_Rw_MillName.BackColor = Color.LightGray

                    txt_RwExcSht.Enabled = False
                    txt_RwExcSht.BackColor = Color.LightGray


                End If

                Total_BabyCone_Calculation()

                txt_RewindingCones.Text = Val(dt1.Rows(0).Item("rw_cones").ToString)
                cbo_Rw_MillName.Text = Common_Procedures.Mill_IdNoToName(Con, dt1.Rows(0).Item("rw_millidno").ToString)

                txt_RwExcSht.Text = Format(Val(dt1.Rows(0).Item("Rw_ExcessShort").ToString), "#########0.000")
                If Val(txt_RwExcSht.Text) = 0 Then
                    txt_RwExcSht.Text = ""
                End If



                dt2.Clear()

            End If

            dt1.Clear()

            If Siz_Lck_STS = True Or Trim(txt_InvoiceCode.Text) <> "" Or Val(txt_InvoiceCode.Text) <> 0 Then

                cbo_Ledger.Enabled = False
                cbo_Ledger.BackColor = Color.LightGray

                cbo_CountName.Enabled = False
                cbo_CountName.BackColor = Color.LightGray

                cbo_MillName.Enabled = False
                cbo_MillName.BackColor = Color.LightGray

            End If


            If LockSTS = True Then

                cbo_Ledger.Enabled = False
                cbo_Ledger.BackColor = Color.LightGray

                cbo_CountName.Enabled = False
                cbo_CountName.BackColor = Color.LightGray

                cbo_MillName.Enabled = False
                cbo_MillName.BackColor = Color.LightGray

                cbo_BeamWidth.Enabled = False
                cbo_BeamWidth.BackColor = Color.LightGray

                txt_WarpMeters.Enabled = False
                txt_WarpMeters.BackColor = Color.LightGray

                txt_Ends.Enabled = False
                txt_Ends.BackColor = Color.LightGray

                txt_PcsLength.Enabled = False
                txt_PcsLength.BackColor = Color.LightGray

                txt_TapeLength.Enabled = False
                txt_TapeLength.BackColor = Color.LightGray

                cbo_Meters_Yards.Enabled = False
                cbo_Meters_Yards.BackColor = Color.LightGray

                txt_BabyCone_TareWeight.Enabled = False
                txt_BabyCone_TareWeight.BackColor = Color.LightGray

                txt_BabyCone_AddLessWgt.Enabled = False
                txt_BabyCone_AddLessWgt.BackColor = Color.LightGray

                txt_WgtEmYBag.Enabled = False
                txt_WgtEmYBag.BackColor = Color.LightGray

                txt_WgtEmYCone.Enabled = False
                txt_WgtEmYCone.BackColor = Color.LightGray

                chk_RewindingStatus.Enabled = False
                chk_RewindingStatus.BackColor = Color.LightGray

                txt_RewindingCones.Enabled = False
                txt_RewindingCones.BackColor = Color.LightGray

                cbo_Rw_MillName.Enabled = False
                cbo_Rw_MillName.BackColor = Color.LightGray

                txt_RwExcSht.Enabled = False
                txt_RwExcSht.BackColor = Color.LightGray

                dgv_BabyConeDetails.Enabled = False
                dgv_WarpingDetails_Set1.Enabled = False
                dgv_WarpingDetails_Set2.Enabled = False
                dgv_WarpingDetails_Set3.Enabled = False
                dgv_YarnTakenDetails.Enabled = False

            End If

            '    Get_EmptyBag_Cone_Weight()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT MOVE RECORDS...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Finally

            dt1.Dispose()
            da1.Dispose()

            dt2.Dispose()
            da2.Dispose()

            Grid_Cell_DeSelect()
            If cbo_Ledger.Enabled And cbo_Ledger.Visible Then cbo_Ledger.Focus()

        End Try

    End Sub

    Public Sub delete_record() Implements Interface_MDIActions.delete_record
        Dim cmd As New SqlClient.SqlCommand
        Dim trans As SqlClient.SqlTransaction
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim Dt1 As New DataTable
        Dim NewCode As String = ""
        Dim UID As Single = 0
        Dim vUsrNm As String = "", vAcPwd As String = "", vUnAcPwd As String = ""


        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_SetNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1036" Then '----- KALAIMAGAL TEXTILES (AVINASHI)
            Common_Procedures.Password_Input = ""
            Dim g As New Admin_Password
            g.ShowDialog()

            UID = 1
            Common_Procedures.get_Admin_Name_PassWord_From_DB(vUsrNm, vAcPwd, vUnAcPwd)

            vAcPwd = Common_Procedures.Decrypt(Trim(vAcPwd), Trim(Common_Procedures.UserCreation_AcPassWord.passPhrase) & Trim(Val(UID)) & Trim(UCase(vUsrNm)), Trim(Common_Procedures.UserCreation_AcPassWord.saltValue) & Trim(Val(UID)) & Trim(UCase(vUsrNm)))
            vUnAcPwd = Common_Procedures.Decrypt(Trim(vUnAcPwd), Trim(Common_Procedures.UserCreation_UnAcPassWord.passPhrase) & Trim(Val(UID)) & Trim(UCase(vUsrNm)), Trim(Common_Procedures.UserCreation_UnAcPassWord.saltValue) & Trim(Val(UID)) & Trim(UCase(vUsrNm)))

            If Trim(Common_Procedures.Password_Input) <> Trim(vAcPwd) And Trim(Common_Procedures.Password_Input) <> Trim(vUnAcPwd) Then
                MessageBox.Show("Invalid Admin Password", "ADMIN PASSWORD FAILED...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                Exit Sub
            End If
        End If



        ' If Common_Procedures.UserRight_Check(Common_Procedures.UserRightsCheckFor.DeletingEntry, Common_Procedures.UR.entry_warping_statement, New_Entry, Me, Con, "Warping_Head", "warp_Code", NewCode, "Warp_Date", "(Warp_Code = '" & Trim(NewCode) & "')") = False Then Exit Sub

        'If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.Statement_Entry, "~L~") = 0 And InStr(Common_Procedures.UR.Statement_Entry, "~D~") = 0 Then MessageBox.Show("You have No Rights to Delete", "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error) : Exit Sub

        If MessageBox.Show("Do you want to Delete?", "FOR DELETION...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) <> Windows.Forms.DialogResult.Yes Then
            Exit Sub
        End If

        If New_Entry = True Then
            MessageBox.Show("This is New Entry", "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
        End If



        Da = New SqlClient.SqlDataAdapter("select count(*) from Warping_Head Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Warp_Code = '" & Trim(NewCode) & "' and  Set_Code <> ''", Con)
        Dt1 = New DataTable
        Da.Fill(Dt1)
        If Dt1.Rows.Count > 0 Then
            If IsDBNull(Dt1.Rows(0)(0).ToString) = False Then
                If Val(Dt1.Rows(0)(0).ToString) > 0 Then
                    MessageBox.Show("Already Statement Prepared", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Exit Sub
                End If
            End If
        End If
        Dt1.Clear()


        Da = New SqlClient.SqlDataAdapter("select sum(Delivered_Weight) from Stock_BabyCone_Processing_Details where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'", Con)
        Dt = New DataTable
        Da.Fill(Dt)
        If Dt.Rows.Count > 0 Then
            If IsDBNull(Dt.Rows(0)(0).ToString) = False Then
                If Val(Dt.Rows(0)(0).ToString) > 0 Then
                    MessageBox.Show("BabyCone Delivered to Party", "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                    Exit Sub
                End If
            End If
        End If
        Dt.Clear()

        Da = New SqlClient.SqlDataAdapter("select sum(RwDelivered_Weight) from Stock_RewindingBabyCone_Processing_Details where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'", Con)
        Dt = New DataTable
        Da.Fill(Dt)
        If Dt.Rows.Count > 0 Then
            If IsDBNull(Dt.Rows(0)(0).ToString) = False Then
                If Val(Dt.Rows(0)(0).ToString) > 0 Then
                    MessageBox.Show("BabyCone Delivered for Rewinding", "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                    Exit Sub
                End If
            End If
        End If
        Dt.Clear()



        trans = Con.BeginTransaction

        Try

            cmd.Connection = Con
            cmd.Transaction = trans


            cmd.CommandText = "Delete from Warping_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Warp_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Warping_YarnTaken_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Warp_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Warping_BabyCone_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Warp_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Warping_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Warp_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Stock_Yarn_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()


            trans.Commit()

            new_record()

            MessageBox.Show("Deleted Sucessfully!!!", "FOR DELETION...", MessageBoxButtons.OKCancel, MessageBoxIcon.Information)

        Catch ex As Exception
            trans.Rollback()
            MessageBox.Show(ex.Message, "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Finally
            trans.Dispose()
            cmd.Dispose()
            Da.Dispose()
            Dt.Dispose()

            If cbo_Ledger.Enabled And cbo_Ledger.Visible Then cbo_Ledger.Focus()

        End Try

    End Sub

    Public Sub filter_record() Implements Interface_MDIActions.filter_record

        If Filter_Status = False Then

            Dim da As New SqlClient.SqlDataAdapter
            Dim dt1 As New DataTable
            Dim dt2 As New DataTable
            Dim dt3 As New DataTable

            da = New SqlClient.SqlDataAdapter("select a.Ledger_DisplayName from Ledger_AlaisHead a, ledger_head b where (a.Ledger_IdNo = 0 or b.AccountsGroup_IdNo = 10 ) and a.Ledger_IdNo = b.Ledger_IdNo order by a.Ledger_DisplayName", Con)
            da.Fill(dt1)
            cbo_Filter_PartyName.DataSource = dt1
            cbo_Filter_PartyName.DisplayMember = "Ledger_DisplayName"

            da = New SqlClient.SqlDataAdapter("select Count_name from Count_Head order by count_name", Con)
            da.Fill(dt2)
            cbo_Filter_CountName.DataSource = dt2
            cbo_Filter_CountName.DisplayMember = "count_name"

            da = New SqlClient.SqlDataAdapter("select Mill_name from Mill_Head order by Mill_name", Con)
            da.Fill(dt3)
            cbo_Filter_MillName.DataSource = dt3
            cbo_Filter_MillName.DisplayMember = "Mill_name"

            dtp_Filter_Fromdate.Text = Common_Procedures.Company_FromDate
            dtp_Filter_ToDate.Text = Common_Procedures.Company_ToDate
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
        pnl_Back.Enabled = False
        If dtp_Filter_Fromdate.Enabled And dtp_Filter_Fromdate.Visible Then dtp_Filter_Fromdate.Focus()
    End Sub

    Public Sub movefirst_record() Implements Interface_MDIActions.movefirst_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim movno As String

        Try

            da = New SqlClient.SqlDataAdapter("select top 1 Warp_No from Warping_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Warp_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby, Warp_No", Con)
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

            OrdByNo = Common_Procedures.OrderBy_CodeToValue(Trim(lbl_SetNo.Text))

            da = New SqlClient.SqlDataAdapter("select top 1 Warp_No from Warping_Head where for_orderby > " & Str(Format(Val(OrdByNo), "#########0.00")) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and Warp_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby, Warp_No", Con)
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

            OrdByNo = Common_Procedures.OrderBy_CodeToValue(Trim(lbl_SetNo.Text))

            da = New SqlClient.SqlDataAdapter("select top 1 Warp_No from Warping_Head where for_orderby < " & Str(Format(Val(OrdByNo), "########0.00")) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and Warp_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, Warp_No desc", Con)
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

        'Try
        da = New SqlClient.SqlDataAdapter("select top 1 Warp_No from Warping_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Warp_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, Warp_No desc", Con)
        da.Fill(dt)

        movno = ""
        If dt.Rows.Count > 0 Then
            If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                movno = dt.Rows(0)(0).ToString
            End If
        End If

        If Val(movno) <> 0 Then move_record(movno)

        'Catch ex As Exception
        '    MessageBox.Show(ex.Message, "FOR  MOVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        'End Try

    End Sub

    Public Sub new_record() Implements Interface_MDIActions.new_record
        Dim Da1 As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable

        Try

            clear()

            New_Entry = True

            lbl_SetNo.Text = Common_Procedures.get_MaxCode(Con, "Warping_Head", "Warp_Code", "For_OrderBy", "", Val(lbl_Company.Tag), Common_Procedures.FnYearCode)

            lbl_SetNo.ForeColor = Color.Red

            Da1 = New SqlClient.SqlDataAdapter("select Top 1 a.* from Warping_Head a where a.company_idno = " & Str(Val(lbl_Company.Tag)) & " and a.Warp_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by a.for_Orderby desc, a.Warp_No desc", Con)
            Da1.Fill(Dt1)
            If Dt1.Rows.Count > 0 Then
                If Dt1.Rows(0).Item("Meters_Yards").ToString <> "" Then cbo_Meters_Yards.Text = Dt1.Rows(0).Item("Meters_Yards").ToString
            End If
            Dt1.Clear()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR NEW RECORD...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Finally
            Dt1.Dispose()
            Da1.Dispose()

            If cbo_Ledger.Enabled And cbo_Ledger.Visible Then cbo_Ledger.Focus()

        End Try

    End Sub

    Public Sub open_record() Implements Interface_MDIActions.open_record
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim movno As String, inpno As String
        Dim RecCode As String

        Try

            inpno = InputBox("Enter Set No.", "FOR FINDING...")

            RecCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

            Da = New SqlClient.SqlDataAdapter("select Warp_No from Warping_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Warp_Code = '" & Trim(RecCode) & "'", Con)
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
                MessageBox.Show("Set No. does not exists", "DOES NOT FIND...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

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

        Try

            'If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.Statement_Entry, "~L~") = 0 And InStr(Common_Procedures.UR.Statement_Entry, "~I~") = 0 Then MessageBox.Show("You have No Rights to Insert", "DOES NOT INSERT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error) : Exit Sub

            inpno = InputBox("Enter New Set No.", "FOR NEW SET INSERTION...")

            RecCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

            Da = New SqlClient.SqlDataAdapter("select Warp_No from Warping_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Warp_Code = '" & Trim(RecCode) & "'", Con)
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
                    MessageBox.Show("Invalid Set No", "DOES NOT INSERT NEW RECEIPT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

                Else
                    new_record()
                    Insert_Entry = True
                    lbl_SetNo.Text = Trim(UCase(inpno))

                End If

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT INSERT NEW RECEIPT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Public Sub save_record() Implements Interface_MDIActions.save_record
        Dim cmd As New SqlClient.SqlCommand
        Dim tr As SqlClient.SqlTransaction
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim vByWarpCd As String = "", vByWarpNo As String = ""
        Dim NewCode As String = ""
        Dim OrdByNo As Single = 0
        Dim led_id As Integer
        Dim trans_id As Integer = 0
        Dim Bw_id As Integer = 0
        Dim Cnt_ID As Integer = 0
        Dim Mil_ID As Integer, RwMil_ID As Integer
        Dim Sno As Integer = 0
        Dim Partcls As String = ""
        Dim vYrnPartcls As String = ""
        Dim Prtcls2 As String = ""
        Dim PBlNo As String = ""
        Dim EntID As String = ""
        Dim vOrdByNo As String = 0
        Dim vSetCd As String, vSetNo As String
        '  Dim Dup_BmNo As String
        Dim YCnt_ID As Integer = 0
        Dim Delv_ID As Integer, Rec_ID As Integer
        Dim YMil_ID As Integer = 0
        Dim YLoc_ID As Integer = 0
        ' Dim Itm_ID As Integer
        Dim Selc_WarpCode As String
        Dim sWarpWgt As String
        Dim vWrpSlNo As Integer, vSizSlNo As Integer, vNoofEnds As Integer, vNoofWrpYrds As Integer
        Dim vTotEnds As Single = 0, vTotWrpMtrs As Single = 0
        Dim a() As String
        Dim i As Integer
        Dim vTotYrnBags As Single, vTotYrnCones As Single, vTotYrnWeight As Single, vTotYrnGrsWeight As Single, vTotYrnTrWeight As Single
        Dim vTotBabyBags As Single, vTotBabyCones As Single, vTotBabyGrsWgt As Single
        'Dim vTotChemQty As Single, vTotChemAmt As Single
        Dim vRwSTS As Integer = 0, vSocSetSTS As Integer = 0
        Dim v_W_SlNo As Integer
        Dim v_W_Mtrs As Single
        Dim Nr As Long
        Dim vWarpWgt1 As Single, vWarpWgt2 As Single, vWarpWgt3 As Single
        Dim vSizMtr1 As Single, vSizMtr2 As Single, vSizMtr3 As Single
        Dim Emp_ID As Integer
        Dim FtEmp_ID As Integer, Ft2Emp_ID As Integer
        Dim BkEmp_ID As Integer, Bk2Emp_ID As Integer
        Dim HrEmp_ID As Integer, Hr2Emp_ID As Integer
        Dim Eds2FtEmp_ID As Integer, Eds2Ft2Emp_ID As Integer
        Dim Eds2BkEmp_ID As Integer, Eds2Bk2Emp_ID As Integer
        Dim Eds2HrEmp_ID As Integer, Eds2Hr2Emp_ID As Integer
        Dim Eds3FtEmp_ID As Integer, Eds3Ft2Emp_ID As Integer
        Dim Eds3BkEmp_ID As Integer, Eds3Bk2Emp_ID As Integer
        Dim Eds3HrEmp_ID As Integer, Eds3Hr2Emp_ID As Integer
        Dim Wt As Single, Cns As Integer
        Dim YLotNo As String = ""
        Dim YLocID As Integer = 0
        Dim Yrn_Typ As String
        Dim Mid As Integer, Bgs As Integer
        Dim StNo As String

        Dim Mtrs_Yrds As String = ""
        '  Dim lckdt As Date
        Dim UserIdNo As Integer = 0

        ' newly added 08Aug2019
        Dim v_NoNeed_Posting As Integer = 0
        Dim babyCone_DelvIdNo As Integer = 0
        Dim babyCone_LocIdNo As Integer = 0


        v_NoNeed_Posting = chk_NoNeedPosting.Checked



        UserIdNo = Common_Procedures.User.IdNo

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_SetNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)
        'If Common_Procedures.UserRight_Check(Common_Procedures.UserRightsCheckFor.SavingEntry, Common_Procedures.UR.Yarn_Receipt_Entry, New_Entry, Me, Con, "Warping_Head", "warp_Code", NewCode, "warp_Date", "(warp_Code = '" & Trim(NewCode) & "')", "(Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and warp_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "')", "for_Orderby desc, warp_No desc", dtp_Date.Value.Date) = False Then Exit Sub

        'If Common_Procedures.UserRight_Check(Common_Procedures.UserRightsCheckFor.SavingEntry, Common_Procedures.UR.Statement_Entry, New_Entry) = False Then Exit Sub

        If Val(lbl_Company.Tag) = 0 Then
            MessageBox.Show("Invalid Company Selection", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
        End If

        If pnl_Back.Enabled = False Then
            MessageBox.Show("Close Other Windows", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
        End If

        If IsDate(dtp_Date.Text) = False Then
            MessageBox.Show("Invalid Date", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If dtp_Date.Enabled And dtp_Date.Visible Then dtp_Date.Focus()
            Exit Sub
        End If

        If Not (dtp_Date.Value.Date >= Common_Procedures.Company_FromDate And dtp_Date.Value.Date <= Common_Procedures.Company_ToDate) Then
            MessageBox.Show("Date is out of financial range", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If dtp_Date.Enabled And dtp_Date.Visible Then dtp_Date.Focus()
            Exit Sub
        End If

        led_id = Common_Procedures.Ledger_AlaisNameToIdNo(Con, cbo_Ledger.Text)
        If led_id = 0 Then
            MessageBox.Show("Invalid Party Name", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If cbo_Ledger.Enabled And cbo_Ledger.Visible Then cbo_Ledger.Focus()
            Exit Sub
        End If

        Mil_ID = Common_Procedures.Mill_NameToIdNo(Con, cbo_MillName.Text)
        If Mil_ID = 0 Then
            MessageBox.Show("Invalid Mill Name", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If cbo_MillName.Enabled And cbo_MillName.Visible Then cbo_MillName.Focus()
            Exit Sub
        End If

        Cnt_ID = Common_Procedures.Count_NameToIdNo(Con, cbo_CountName.Text)
        If Cnt_ID = 0 Then
            MessageBox.Show("Invalid Mill Name", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If cbo_CountName.Enabled And cbo_CountName.Visible Then cbo_CountName.Focus()
            Exit Sub
        End If

        'If Val(txt_Ends.Text) = 0 Then
        '    MessageBox.Show("Invalid Ends Name", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
        '    If txt_Ends.Enabled Then txt_Ends.Focus()
        '    Exit Sub
        'End If

        If Val(txt_WarpMeters.Text) = 0 Then
            MessageBox.Show("Invalid Warp Meters", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If txt_WarpMeters.Enabled Then txt_WarpMeters.Focus()
            Exit Sub
        End If

        Bw_id = Common_Procedures.BeamWidth_NameToIdNo(Con, cbo_BeamWidth.Text)

        With dgv_WarpingDetails_Set1
            For i = 0 To .RowCount - 1

                If Val(.Rows(i).Cells(4).Value) <> 0 Then

                    If Val(.Rows(i).Cells(1).Value) = 0 Then
                        MessageBox.Show("Invalid Ends Name", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                        tab_Main.SelectTab(0)
                        tab_WarpingDeatils.SelectTab(0)
                        If .Rows.Count <= 0 Then .Rows.Add()
                        .Focus()
                        .CurrentCell = .Rows(i).Cells(1)
                        .CurrentCell.Selected = True
                        Exit Sub
                    End If

                    If Val(.Rows(i).Cells(11).Value) <> 0 Then
                        Emp_ID = Common_Procedures.Employee_NameToIdNo(Con, .Rows(i).Cells(10).Value)
                        If Val(Emp_ID) = 0 Then
                            MessageBox.Show("Invalid Front Warper1", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                            tab_Main.SelectTab(0)
                            tab_WarpingDeatils.SelectTab(0)
                            If .Rows.Count <= 0 Then .Rows.Add()
                            .Focus()
                            .CurrentCell = .Rows(i).Cells(10)
                            .CurrentCell.Selected = True
                            Exit Sub
                        End If
                    End If

                    If Val(.Rows(i).Cells(13).Value) <> 0 Then
                        Emp_ID = Common_Procedures.Employee_NameToIdNo(Con, .Rows(i).Cells(12).Value)
                        If Val(Emp_ID) = 0 Then
                            MessageBox.Show("Invalid Back Warper1", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                            tab_Main.SelectTab(0)
                            tab_WarpingDeatils.SelectTab(0)
                            If .Rows.Count <= 0 Then .Rows.Add()
                            .Focus()
                            .CurrentCell = .Rows(i).Cells(12)
                            .CurrentCell.Selected = True
                            Exit Sub
                        End If
                    End If

                    If Val(.Rows(i).Cells(15).Value) <> 0 Then
                        Emp_ID = Common_Procedures.Employee_NameToIdNo(Con, .Rows(i).Cells(14).Value)
                        If Val(Emp_ID) = 0 Then
                            MessageBox.Show("Invalid Helper1", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                            tab_Main.SelectTab(0)
                            tab_WarpingDeatils.SelectTab(0)
                            If .Rows.Count <= 0 Then .Rows.Add()
                            .Focus()
                            .CurrentCell = .Rows(i).Cells(14)
                            .CurrentCell.Selected = True
                            Exit Sub
                        End If
                    End If

                    If Val(.Rows(i).Cells(17).Value) <> 0 Then
                        Emp_ID = Common_Procedures.Employee_NameToIdNo(Con, .Rows(i).Cells(16).Value)
                        If Val(Emp_ID) = 0 Then
                            MessageBox.Show("Invalid front Warper1", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                            tab_Main.SelectTab(0)
                            tab_WarpingDeatils.SelectTab(0)
                            If .Rows.Count <= 0 Then .Rows.Add()
                            .Focus()
                            .CurrentCell = .Rows(i).Cells(16)
                            .CurrentCell.Selected = True
                            Exit Sub
                        End If
                    End If

                    If Val(.Rows(i).Cells(19).Value) <> 0 Then
                        Emp_ID = Common_Procedures.Employee_NameToIdNo(Con, .Rows(i).Cells(18).Value)
                        If Val(Emp_ID) = 0 Then
                            MessageBox.Show("Invalid Back Warper2", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                            tab_Main.SelectTab(0)
                            tab_WarpingDeatils.SelectTab(0)
                            If .Rows.Count <= 0 Then .Rows.Add()
                            .Focus()
                            .CurrentCell = .Rows(i).Cells(18)
                            .CurrentCell.Selected = True
                            Exit Sub
                        End If
                    End If

                    If Val(.Rows(i).Cells(21).Value) <> 0 Then
                        Emp_ID = Common_Procedures.Employee_NameToIdNo(Con, .Rows(i).Cells(20).Value)
                        If Val(Emp_ID) = 0 Then
                            MessageBox.Show("Invalid Helper2", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                            tab_Main.SelectTab(0)
                            tab_WarpingDeatils.SelectTab(0)
                            If .Rows.Count <= 0 Then .Rows.Add()
                            .Focus()
                            .CurrentCell = .Rows(i).Cells(20)
                            .CurrentCell.Selected = True
                            Exit Sub
                        End If
                    End If

                End If

            Next

        End With

        With dgv_WarpingDetails_Set2

            For i = 0 To .RowCount - 1

                If Val(.Rows(i).Cells(4).Value) <> 0 Then

                    If Val(.Rows(i).Cells(1).Value) = 0 Then
                        MessageBox.Show("Invalid Ends Name", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                        tab_Main.SelectTab(0)
                        tab_WarpingDeatils.SelectTab(1)
                        If .Rows.Count <= 0 Then .Rows.Add()
                        .Focus()
                        .CurrentCell = .Rows(i).Cells(1)
                        .CurrentCell.Selected = True
                        Exit Sub
                    End If

                    If Val(.Rows(i).Cells(11).Value) <> 0 Then
                        Emp_ID = Common_Procedures.Employee_NameToIdNo(Con, .Rows(i).Cells(10).Value)
                        If Val(Emp_ID) = 0 Then
                            MessageBox.Show("Invalid Front Warper1", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                            tab_Main.SelectTab(0)
                            tab_WarpingDeatils.SelectTab(1)
                            If .Rows.Count <= 0 Then .Rows.Add()
                            .Focus()
                            .CurrentCell = .Rows(i).Cells(10)
                            .CurrentCell.Selected = True
                            Exit Sub
                        End If
                    End If

                    If Val(.Rows(i).Cells(13).Value) <> 0 Then
                        Emp_ID = Common_Procedures.Employee_NameToIdNo(Con, .Rows(i).Cells(12).Value)
                        If Val(Emp_ID) = 0 Then
                            MessageBox.Show("Invalid Back Warper1", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                            tab_Main.SelectTab(0)
                            tab_WarpingDeatils.SelectTab(1)
                            If .Rows.Count <= 0 Then .Rows.Add()
                            .Focus()
                            .CurrentCell = .Rows(i).Cells(12)
                            .CurrentCell.Selected = True
                            Exit Sub
                        End If
                    End If

                    If Val(.Rows(i).Cells(15).Value) <> 0 Then
                        Emp_ID = Common_Procedures.Employee_NameToIdNo(Con, .Rows(i).Cells(14).Value)
                        If Val(Emp_ID) = 0 Then
                            MessageBox.Show("Invalid Back Warper1", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                            tab_Main.SelectTab(0)
                            tab_WarpingDeatils.SelectTab(1)
                            If .Rows.Count <= 0 Then .Rows.Add()
                            .Focus()
                            .CurrentCell = .Rows(i).Cells(14)
                            .CurrentCell.Selected = True
                            Exit Sub
                        End If
                    End If

                    If Val(.Rows(i).Cells(17).Value) <> 0 Then
                        Emp_ID = Common_Procedures.Employee_NameToIdNo(Con, .Rows(i).Cells(16).Value)
                        If Val(Emp_ID) = 0 Then
                            MessageBox.Show("Invalid Helper1 ", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                            tab_Main.SelectTab(0)
                            tab_WarpingDeatils.SelectTab(1)
                            If .Rows.Count <= 0 Then .Rows.Add()
                            .Focus()
                            .CurrentCell = .Rows(i).Cells(16)
                            .CurrentCell.Selected = True
                            Exit Sub
                        End If
                    End If

                    If Val(.Rows(i).Cells(19).Value) <> 0 Then
                        Emp_ID = Common_Procedures.Employee_NameToIdNo(Con, .Rows(i).Cells(18).Value)
                        If Val(Emp_ID) = 0 Then
                            MessageBox.Show("Invalid Back Warper2", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                            tab_Main.SelectTab(0)
                            tab_WarpingDeatils.SelectTab(1)
                            If .Rows.Count <= 0 Then .Rows.Add()
                            .Focus()
                            .CurrentCell = .Rows(i).Cells(18)
                            .CurrentCell.Selected = True
                            Exit Sub
                        End If
                    End If

                    If Val(.Rows(i).Cells(21).Value) <> 0 Then
                        Emp_ID = Common_Procedures.Employee_NameToIdNo(Con, .Rows(i).Cells(20).Value)
                        If Val(Emp_ID) = 0 Then
                            MessageBox.Show("Invalid Helper2", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                            tab_Main.SelectTab(0)
                            tab_WarpingDeatils.SelectTab(1)
                            If .Rows.Count <= 0 Then .Rows.Add()
                            .Focus()
                            .CurrentCell = .Rows(i).Cells(20)
                            .CurrentCell.Selected = True
                            Exit Sub
                        End If

                    End If

                End If

            Next

        End With

        With dgv_WarpingDetails_Set3

            For i = 0 To .RowCount - 1

                If Val(.Rows(i).Cells(4).Value) <> 0 Then
                    If Val(.Rows(i).Cells(1).Value) = 0 Then
                        MessageBox.Show("Invalid Ends Name", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                        tab_Main.SelectTab(0)
                        tab_WarpingDeatils.SelectTab(2)
                        If .Rows.Count <= 0 Then .Rows.Add()
                        .Focus()
                        .CurrentCell = .Rows(i).Cells(1)
                        .CurrentCell.Selected = True

                        Exit Sub

                    End If

                    If Val(.Rows(i).Cells(11).Value) <> 0 Then
                        Emp_ID = Common_Procedures.Employee_NameToIdNo(Con, .Rows(i).Cells(10).Value)
                        If Val(Emp_ID) = 0 Then
                            MessageBox.Show("Invalid Front Warper1", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                            tab_Main.SelectTab(0)
                            tab_WarpingDeatils.SelectTab(2)
                            If .Rows.Count <= 0 Then .Rows.Add()
                            .Focus()
                            .CurrentCell = .Rows(i).Cells(10)
                            .CurrentCell.Selected = True
                            Exit Sub
                        End If
                    End If

                    If Val(.Rows(i).Cells(13).Value) <> 0 Then
                        Emp_ID = Common_Procedures.Employee_NameToIdNo(Con, .Rows(i).Cells(12).Value)
                        If Val(Emp_ID) = 0 Then
                            MessageBox.Show("Invalid Back Warper1", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                            tab_Main.SelectTab(0)
                            tab_WarpingDeatils.SelectTab(2)
                            If .Rows.Count <= 0 Then .Rows.Add()
                            .Focus()
                            .CurrentCell = .Rows(i).Cells(12)
                            .CurrentCell.Selected = True
                            Exit Sub
                        End If
                    End If

                    If Val(.Rows(i).Cells(15).Value) <> 0 Then
                        Emp_ID = Common_Procedures.Employee_NameToIdNo(Con, .Rows(i).Cells(14).Value)
                        If Val(Emp_ID) = 0 Then
                            MessageBox.Show("Invalid Back Warper1", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                            tab_Main.SelectTab(0)
                            tab_WarpingDeatils.SelectTab(2)
                            If .Rows.Count <= 0 Then .Rows.Add()
                            .Focus()
                            .CurrentCell = .Rows(i).Cells(14)
                            .CurrentCell.Selected = True
                            Exit Sub
                        End If
                    End If

                    If Val(.Rows(i).Cells(17).Value) <> 0 Then
                        Emp_ID = Common_Procedures.Employee_NameToIdNo(Con, .Rows(i).Cells(16).Value)
                        If Val(Emp_ID) = 0 Then
                            MessageBox.Show("Invalid Helper1 ", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                            tab_Main.SelectTab(0)
                            tab_WarpingDeatils.SelectTab(2)
                            If .Rows.Count <= 0 Then .Rows.Add()
                            .Focus()
                            .CurrentCell = .Rows(i).Cells(16)
                            .CurrentCell.Selected = True
                            Exit Sub
                        End If
                    End If

                    If Val(.Rows(i).Cells(19).Value) <> 0 Then
                        Emp_ID = Common_Procedures.Employee_NameToIdNo(Con, .Rows(i).Cells(18).Value)
                        If Val(Emp_ID) = 0 Then
                            MessageBox.Show("Invalid Back Warper2", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                            tab_Main.SelectTab(0)
                            tab_WarpingDeatils.SelectTab(2)
                            If .Rows.Count <= 0 Then .Rows.Add()
                            .Focus()
                            .CurrentCell = .Rows(i).Cells(18)
                            .CurrentCell.Selected = True
                            Exit Sub
                        End If
                    End If

                    If Val(.Rows(i).Cells(21).Value) <> 0 Then
                        Emp_ID = Common_Procedures.Employee_NameToIdNo(Con, .Rows(i).Cells(20).Value)
                        If Val(Emp_ID) = 0 Then
                            MessageBox.Show("Invalid Helper2", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                            tab_Main.SelectTab(0)
                            tab_WarpingDeatils.SelectTab(2)
                            If .Rows.Count <= 0 Then .Rows.Add()
                            .Focus()
                            .CurrentCell = .Rows(i).Cells(20)
                            .CurrentCell.Selected = True
                            Exit Sub
                        End If

                    End If

                End If

            Next

        End With



        For i = 0 To dgv_YarnTakenDetails.RowCount - 1

            If Val(dgv_YarnTakenDetails.Rows(i).Cells(8).Value) <> 0 Then

                YCnt_ID = Common_Procedures.Count_NameToIdNo(Con, dgv_YarnTakenDetails.Rows(i).Cells(1).Value)
                If Val(YCnt_ID) = 0 Then
                    MessageBox.Show("Invalid CountName", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                    tab_Main.SelectTab(2)
                    dgv_YarnTakenDetails.Focus()
                    dgv_YarnTakenDetails.CurrentCell = dgv_YarnTakenDetails.Rows(i).Cells(1)
                    Exit Sub
                End If

                If Trim(dgv_YarnTakenDetails.Rows(i).Cells(2).Value) = "" Then
                    MessageBox.Show("Invalid Yarn Type", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                    tab_Main.SelectTab(2)
                    dgv_YarnTakenDetails.Focus()
                    dgv_YarnTakenDetails.CurrentCell = dgv_YarnTakenDetails.Rows(i).Cells(2)
                    Exit Sub
                End If


                If Common_Procedures.settings.CustomerCode = "1288" Then
                    Dim l As Integer = Common_Procedures.Ledger_AlaisNameToIdNo(Con, Trim(dgv_YarnTakenDetails.Rows(i).Cells(11).Value))
                    If l = 0 Then
                        MessageBox.Show("Invalid Location Name", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                        tab_Main.SelectTab(1)
                        dgv_YarnTakenDetails.Focus()
                        dgv_YarnTakenDetails.CurrentCell = dgv_YarnTakenDetails.Rows(i).Cells(11)
                        Exit Sub
                    End If
                End If

                If Trim(UCase(dgv_YarnTakenDetails.Rows(i).Cells(2).Value)) = "BABY" Then
                    If Trim(UCase(dgv_YarnTakenDetails.Rows(i).Cells(3).Value)) = "" Then
                        MessageBox.Show("Invalid SetNo", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                        tab_Main.SelectTab(2)
                        dgv_YarnTakenDetails.Focus()
                        dgv_YarnTakenDetails.CurrentCell = dgv_YarnTakenDetails.Rows(i).Cells(3)
                        Exit Sub
                    End If
                End If

                YMil_ID = Common_Procedures.Mill_NameToIdNo(Con, dgv_YarnTakenDetails.Rows(i).Cells(4).Value)
                If Val(YMil_ID) = 0 Then
                    MessageBox.Show("Invalid Mill Name", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                    tab_Main.SelectTab(2)
                    dgv_YarnTakenDetails.Focus()
                    dgv_YarnTakenDetails.CurrentCell = dgv_YarnTakenDetails.Rows(i).Cells(4)
                    Exit Sub
                End If

            End If

        Next

        TotalWarping_Calculation()
        Total_BabyCone_Calculation()
        TotalYarnTaken_Calculation()
        Excess_Calculation()

        vWrpSlNo = 1
        vSizSlNo = 0
        sWarpWgt = ""
        vWarpWgt1 = 0 : vWarpWgt2 = 0 : vWarpWgt3 = 0

        If dgv_WarpingDetails_Total_Set1.RowCount > 0 Then
            sWarpWgt = Trim(Val(dgv_WarpingDetails_Total_Set1.Rows(0).Cells(4).Value()))
            vWarpWgt1 = Trim(Val(dgv_WarpingDetails_Total_Set1.Rows(0).Cells(4).Value()))
        End If
        If dgv_WarpingDetails_Total_Set2.RowCount > 0 Then
            If Val(dgv_WarpingDetails_Total_Set2.Rows(0).Cells(4).Value()) <> 0 Then
                sWarpWgt = Trim(sWarpWgt) & "," & Trim(Val(dgv_WarpingDetails_Total_Set2.Rows(0).Cells(4).Value()))
                vWrpSlNo = vWrpSlNo + 1
                vWarpWgt2 = Trim(Val(dgv_WarpingDetails_Total_Set2.Rows(0).Cells(4).Value()))
            End If
        End If
        If dgv_WarpingDetails_Total_Set3.RowCount > 0 Then
            If Val(dgv_WarpingDetails_Total_Set3.Rows(0).Cells(4).Value()) <> 0 Then
                sWarpWgt = Trim(sWarpWgt) & "," & Trim(Val(dgv_WarpingDetails_Total_Set3.Rows(0).Cells(4).Value()))
                vWrpSlNo = vWrpSlNo + 1
                vWarpWgt3 = Trim(Val(dgv_WarpingDetails_Total_Set3.Rows(0).Cells(4).Value()))
            End If
        End If

        vSizSlNo = 1
        vSizMtr1 = 0 : vSizMtr2 = 0 : vSizMtr3 = 0



        a = Split(Trim(txt_Ends.Text), ",")
        vNoofEnds = 0
        vTotEnds = 0
        For i = 0 To UBound(a)
            vTotEnds = vTotEnds + Val(a(i))
            vNoofEnds = vNoofEnds + 1
        Next

        a = Split(Trim(txt_WarpMeters.Text), ",")
        vTotWrpMtrs = 0
        vNoofWrpYrds = 0
        For i = 0 To UBound(a)
            vTotWrpMtrs = vTotWrpMtrs + Val(a(i))
            vNoofWrpYrds = vNoofWrpYrds + 1
        Next

        'If Val(vWrpSlNo) <> Val(vSizSlNo) Then
        '    MessageBox.Show("Invalid Warping & Sizing Details for DoubleSet", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
        '    If txt_WarpMeters.Enabled Then txt_WarpMeters.Focus()
        '    Exit Sub
        'End If

        If Val(vWrpSlNo) <> 0 And Val(vNoofEnds) <> 0 And Val(vWrpSlNo) > Val(vNoofEnds) Then
            MessageBox.Show("Invalid Ends for Double Ends Set", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            tab_Main.SelectTab(0)
            tab_WarpingDeatils.SelectTab(0)
            'If dgv_WarpingDetails_Set1.Rows.Count <= 0 Then dgv_WarpingDetails_Set1.Rows.Add()
            'dgv_WarpingDetails_Set1.Focus()
            'dgv_WarpingDetails_Set1.CurrentCell = dgv_WarpingDetails_Set1.Rows(0).Cells(1)
            'dgv_WarpingDetails_Set1.CurrentCell.Selected = True
            Exit Sub
        End If

        If Val(vWrpSlNo) <> 0 And Val(vNoofWrpYrds) <> 0 And Val(vWrpSlNo) > Val(vNoofWrpYrds) Then
            MessageBox.Show("Invalid WarpMeters for Double Ends Set", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If txt_WarpMeters.Enabled Then txt_WarpMeters.Focus()
            Exit Sub
        End If

        If Val(vSizSlNo) <> 0 And Val(vNoofEnds) <> 0 And Val(vSizSlNo) > Val(vNoofEnds) Then
            MessageBox.Show("Invalid Ends for Double Ends Set", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            tab_Main.SelectTab(0)
            tab_WarpingDeatils.SelectTab(0)
            'If dgv_WarpingDetails_Set1.Rows.Count <= 0 Then dgv_WarpingDetails_Set1.Rows.Add()
            'dgv_WarpingDetails_Set1.Focus()
            'dgv_WarpingDetails_Set1.CurrentCell = dgv_WarpingDetails_Set1.Rows(0).Cells(1)
            'dgv_WarpingDetails_Set1.CurrentCell.Selected = True
            Exit Sub
        End If

        If Val(vSizSlNo) <> 0 And Val(vNoofWrpYrds) <> 0 And Val(vSizSlNo) > Val(vNoofWrpYrds) Then
            MessageBox.Show("Invalid WarpMeters for Double Ends Set", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If txt_WarpMeters.Enabled Then txt_WarpMeters.Focus()
            Exit Sub
        End If

        vRwSTS = 0
        If chk_RewindingStatus.Checked = True Then vRwSTS = 1

        vSocSetSTS = 0
        If chk_SocietySet.Checked = True Then vSocSetSTS = 1

        RwMil_ID = Common_Procedures.Mill_NameToIdNo(Con, cbo_Rw_MillName.Text)
        If Val(RwMil_ID) = 0 Then
            If dgv_YarnTakenDetails.RowCount > 0 Then
                RwMil_ID = Common_Procedures.Mill_NameToIdNo(Con, dgv_YarnTakenDetails.Rows(0).Cells(4).Value())
            End If
        End If

        vTotYrnBags = 0 : vTotYrnCones = 0 : vTotYrnWeight = 0 : vTotYrnGrsWeight = 0 : vTotYrnTrWeight = 0
        If dgv_YarnTakenDetails_Total.RowCount > 0 Then
            vTotYrnBags = Val(dgv_YarnTakenDetails_Total.Rows(0).Cells(5).Value())
            vTotYrnCones = Val(dgv_YarnTakenDetails_Total.Rows(0).Cells(7).Value())
            vTotYrnGrsWeight = Val(dgv_YarnTakenDetails_Total.Rows(0).Cells(8).Value())
            vTotYrnTrWeight = Val(dgv_YarnTakenDetails_Total.Rows(0).Cells(9).Value())
            vTotYrnWeight = Val(dgv_YarnTakenDetails_Total.Rows(0).Cells(10).Value())
        End If

        vTotBabyBags = 0 : vTotBabyCones = 0 : vTotBabyGrsWgt = 0
        If dgv_BabyConeDetails_Total.RowCount > 0 Then
            vTotBabyBags = Val(dgv_BabyConeDetails_Total.Rows(0).Cells(1).Value())
            vTotBabyCones = Val(dgv_BabyConeDetails_Total.Rows(0).Cells(2).Value())
            vTotBabyGrsWgt = Val(dgv_BabyConeDetails_Total.Rows(0).Cells(3).Value())
        End If

        Mtrs_Yrds = "METERS"
        If Trim(UCase(cbo_Meters_Yards.Text)) = "YARDS" Then
            Mtrs_Yrds = "YARDS"
        End If

        'NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_SetNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)
        'Da = New SqlClient.SqlDataAdapter("select count(*) from Warping_Head Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Warp_Code = '" & Trim(NewCode) & "' and  Set_Code <> ''", Con)
        'Dt1 = New DataTable
        'Da.Fill(Dt1)
        'If Dt1.Rows.Count > 0 Then
        '    If IsDBNull(Dt1.Rows(0)(0).ToString) = False Then
        '        If Val(Dt1.Rows(0)(0).ToString) > 0 Then
        '            MessageBox.Show("Already Statement Prepared", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
        '            Exit Sub
        '        End If
        '    End If
        'End If
        'Dt1.Clear()

        tr = Con.BeginTransaction

        Try

            If Insert_Entry = True Or New_Entry = False Then
                NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_SetNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            Else

                lbl_SetNo.Text = Common_Procedures.get_MaxCode(Con, "Warping_Head", "Warp_Code", "For_OrderBy", "", Val(lbl_Company.Tag), Common_Procedures.FnYearCode, tr)

                NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_SetNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            End If


            Selc_WarpCode = Trim(lbl_SetNo.Text) & "/" & Trim(Common_Procedures.FnYearCode) & "/" & Trim(Val(lbl_Company.Tag))

            OrdByNo = Val(Common_Procedures.OrderBy_CodeToValue(lbl_SetNo.Text))

            cmd.Connection = Con
            cmd.Transaction = tr

            cmd.Parameters.Clear()
            cmd.Parameters.AddWithValue("@SetDate", dtp_Date.Value.Date)

            babyCone_DelvIdNo = Common_Procedures.Ledger_NameToIdNo(Con, cbo_DeliveryTo.Text, tr)
            babyCone_LocIdNo = Common_Procedures.Ledger_NameToIdNo(Con, cbo_Location.Text, tr)


            If New_Entry = True Then

                cmd.CommandText = "Insert into Warping_Head (User_IdNo, Warp_Code, Warpcode_forSelection      , Company_IdNo                  , Warp_No                        , for_OrderBy, Warp_Date, ledger_idno, count_idno, mill_idno, Beam_Width_Idno, ends_name, pcs_length, tape_length, meters_yards_type, warp_meters, beam_count, excess_short_yarn,  remarks, warping_slno, sizing_slno, total_warpmeters, warp_weight, Total_Warping_Beams, total_warping_ends, total_warping_gross_weight, total_warping_tare_weight, total_warping_net_weight,  total_yarn_bags, total_yarn_cones,total_yarnGross_weight,total_yarnTare_weight, total_yarn_weight, total_baby_bags, total_baby_cones, total_baby_gross_weight, total_baby_tare_weight, total_baby_net_weight, rw_status, rw_cones, rw_millidno, Rw_ExcessShort, SocietySet_Status, Meters_Yards, invoice_code, invoice_increment  , EmptyBag_weight , EmptyCone_weight    ,  excess_short_Grossyarn  , excess_short_AddLessyarn , total_baby_tare_AddLessweight ,NoNeedPostingFlag,	BabyCone_Delivery_To_IdNo,	BabyCone_Location_IdNo ) " &
                                        " Values (" & Str(UserIdNo) & ",'" & Trim(NewCode) & "', '" & Trim(Selc_WarpCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_SetNo.Text) & "', " & Str(Val(OrdByNo)) & ", @SetDate, " & Str(Val(led_id)) & ", " & Str(Val(Cnt_ID)) & ", " & Str(Val(Mil_ID)) & ", " & Str(Val(Bw_id)) & ", '" & Trim(txt_Ends.Text) & "', '" & Trim(txt_PcsLength.Text) & "', '" & Trim(txt_TapeLength.Text) & "', 'METER', '" & Trim(txt_WarpMeters.Text) & "', '" & Trim(lbl_BeamCount.Text) & "', " & Str(Val(lbl_ExcessShort.Text)) & ", '" & Trim(txt_Remarks.Text) & "', " & Str(Val(vWrpSlNo)) & ", " & Str(Val(vSizSlNo)) & ", " & Str(Val(vTotWrpMtrs)) & ", '" & Trim(sWarpWgt) & "', " & Str(Val(lbl_Total_Warping_Beams.Text)) & ", " & Str(Val(lbl_Total_Warping_Ends.Text)) & ", " & Str(Val(lbl_Total_Warping_GrossWeight.Text)) & ", " & Str(Val(lbl_Total_Warping_TareWeight.Text)) & ", " & Str(Val(lbl_Total_Warping_NetWeight.Text)) & ",  " &
                                           Str(Val(vTotYrnBags)) & ", " & Str(Val(vTotYrnCones)) & ", " & Str(Val(vTotYrnGrsWeight)) & ", " & Str(Val(vTotYrnTrWeight)) & ", " & Str(Val(vTotYrnWeight)) & ", " & Str(Val(vTotBabyBags)) & ", " & Str(Val(vTotBabyCones)) & ", " & Str(Val(vTotBabyGrsWgt)) & ", " & Str(Val(txt_BabyCone_TareWeight.Text)) & ", " & Str(Val(lbl_BabyCone_NetWeight.Text)) & ",  " & Str(Val(vRwSTS)) & ", " & Str(Val(txt_RewindingCones.Text)) & ", " & Str(Val(RwMil_ID)) & ", " & Str(Val(txt_RwExcSht.Text)) & ", " & Str(Val(vSocSetSTS)) & ", '" & Trim(Mtrs_Yrds) & "', '', 0 , " & Str(Val(txt_WgtEmYBag.Text)) & " , " & Str(Val(txt_WgtEmYCone.Text)) & "  , " & Str(Val(lbl_ExcessShort_GrsYarn.Text)) & "  , " & Str(Val(txt_ExcessShort_AddLess.Text)) & "  , " & Str(Val(txt_BabyCone_AddLessWgt.Text)) & "," & Str(Val(v_NoNeed_Posting)) & "," & Str(Val(babyCone_DelvIdNo)) & "," & Str(Val(babyCone_LocIdNo)) & ")"
                cmd.ExecuteNonQuery()

            Else

                cmd.CommandText = "Update Warping_Head set User_IdNo=" & Str(UserIdNo) & ",Warp_Date = @SetDate, ledger_idno = " & Str(Val(led_id)) & ", count_idno = " & Str(Val(Cnt_ID)) & ", mill_idno = " & Str(Val(Mil_ID)) & ", Beam_Width_Idno = " & Str(Val(Bw_id)) & ", ends_name = '" & Trim(txt_Ends.Text) & "', pcs_length = '" & Trim(txt_PcsLength.Text) & "', tape_length = '" & Trim(txt_TapeLength.Text) & "', meters_yards_type = 'METER', warp_meters = '" & Trim(txt_WarpMeters.Text) & "', beam_count = '" & Trim(lbl_BeamCount.Text) & "', excess_short_yarn = " & Str(Val(lbl_ExcessShort.Text)) & ", remarks = '" & Trim(txt_Remarks.Text) & "', warping_slno = " & Str(Val(vWrpSlNo)) & ", sizing_slno = " & Str(Val(vSizSlNo)) & ", total_warpmeters = " & Str(Val(vTotWrpMtrs)) & ", warp_weight = '" & Trim(sWarpWgt) & "', EmptyBag_weight = " & Str(Val(txt_WgtEmYBag.Text)) & ", EmptyCone_weight = " & Str(Val(txt_WgtEmYCone.Text)) & " ,Total_Warping_Beams = " & Str(Val(lbl_Total_Warping_Beams.Text)) & ", total_warping_ends = " & Str(Val(lbl_Total_Warping_Ends.Text)) & ", total_warping_gross_weight = " & Str(Val(lbl_Total_Warping_GrossWeight.Text)) & ", total_warping_tare_weight = " & Str(Val(lbl_Total_Warping_TareWeight.Text)) & ", total_warping_net_weight = " & Str(Val(lbl_Total_Warping_NetWeight.Text)) & ", " &
                                        "  total_yarn_bags = " & Str(Val(vTotYrnBags)) & ", total_yarn_cones = " & Str(Val(vTotYrnCones)) & ", total_yarnGross_weight = " & Str(Val(vTotYrnGrsWeight)) & " ,total_yarnTare_weight= " & Str(Val(vTotYrnTrWeight)) & " ,  total_yarn_weight = " & Str(Val(vTotYrnWeight)) & ", total_baby_bags = " & Str(Val(vTotBabyBags)) & ", total_baby_cones = " & Str(Val(vTotBabyCones)) & ", total_baby_gross_weight = " & Str(Val(vTotBabyGrsWgt)) & ", total_baby_tare_weight = " & Str(Val(txt_BabyCone_TareWeight.Text)) & ", total_baby_net_weight = " & Str(Val(lbl_BabyCone_NetWeight.Text)) & ", rw_status = " & Str(Val(vRwSTS)) & ", rw_cones = " & Str(Val(txt_RewindingCones.Text)) & ", rw_millidno = " & Str(Val(RwMil_ID)) & ", Rw_ExcessShort = " & Str(Val(txt_RwExcSht.Text)) & ",excess_short_Grossyarn = " & Str(Val(lbl_ExcessShort_GrsYarn.Text)) & ",excess_short_AddLessyarn = " & Str(Val(txt_ExcessShort_AddLess.Text)) & ",total_baby_tare_AddLessweight = " & Str(Val(txt_BabyCone_AddLessWgt.Text)) & ", SocietySet_Status = " & Str(Val(vSocSetSTS)) & ", Meters_Yards = '" & Trim(Mtrs_Yrds) & "',NoNeedPostingFlag=" & Str(Val(v_NoNeed_Posting)) & ",	BabyCone_Delivery_To_IdNo=" & Str(Val(babyCone_DelvIdNo)) & ",	BabyCone_Location_IdNo=" & Str(Val(babyCone_LocIdNo)) & " Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Warp_Code = '" & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()

                Da = New SqlClient.SqlDataAdapter("Select * from Warping_YarnTaken_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Warp_Code = '" & Trim(NewCode) & "' and yarn_type = 'BABY'", Con)
                Da.SelectCommand.Transaction = tr
                Dt1 = New DataTable
                Da.Fill(Dt1)

                If Dt1.Rows.Count > 0 Then

                    For i = 0 To Dt1.Rows.Count - 1

                        Nr = 0
                        cmd.CommandText = "Update Stock_BabyCone_Processing_Details set Delivered_Bags = Delivered_Bags - " & Str(Val(Dt1.Rows(i).Item("Bags").ToString)) & ", Delivered_Cones = Delivered_Cones - " & Str(Val(Dt1.Rows(i).Item("cones").ToString)) & ", Delivered_Weight = Delivered_Weight - " & Str(Val(Dt1.Rows(i).Item("Weight").ToString)) & " Where Warpcode_forSelection = '" & Trim(Dt1.Rows(i).Item("BabyCone_Warpcode_forSelection").ToString) & "'"
                        Nr = cmd.ExecuteNonQuery()

                    Next i

                End If
                Dt1.Clear()


            End If

            Partcls = "Stmt : Set.No. " & Trim(lbl_SetNo.Text)
            PBlNo = Trim(lbl_SetNo.Text)
            EntID = Trim(Pk_Condition) & Trim(lbl_SetNo.Text)

            cmd.CommandText = "Delete from Warping_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Warp_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Warping_YarnTaken_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Warp_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Warping_BabyCone_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Warp_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Stock_Yarn_Processing_Details Where SoftwareType_IdNo = " & Str(Val(Common_Procedures.SoftwareTypes.Sizing_Software)) & " ,  Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()


            With dgv_WarpingDetails_Set1
                Sno = 0

                v_W_SlNo = 1
                v_W_Mtrs = 0

                a = Split(Trim(txt_WarpMeters.Text), ",")
                If UBound(a) >= 0 Then v_W_Mtrs = Val(a(0))

                For i = 0 To .RowCount - 1

                    If Val(.Rows(i).Cells(4).Value) <> 0 Then

                        Sno = Sno + 1
                        FtEmp_ID = Common_Procedures.Employee_NameToIdNo(Con, .Rows(i).Cells(10).Value, tr)
                        BkEmp_ID = Common_Procedures.Employee_NameToIdNo(Con, .Rows(i).Cells(12).Value, tr)
                        HrEmp_ID = Common_Procedures.Employee_NameToIdNo(Con, .Rows(i).Cells(14).Value, tr)
                        Ft2Emp_ID = Common_Procedures.Employee_NameToIdNo(Con, .Rows(i).Cells(16).Value, tr)
                        Bk2Emp_ID = Common_Procedures.Employee_NameToIdNo(Con, .Rows(i).Cells(18).Value, tr)
                        Hr2Emp_ID = Common_Procedures.Employee_NameToIdNo(Con, .Rows(i).Cells(20).Value, tr)

                        cmd.CommandText = "Insert into Warping_Details(Warp_Code, Company_IdNo, Warp_No, for_OrderBy, Warp_Date, Ledger_IdNo, count_idno, Mill_IdNo, Warp_SlNo, Sl_No, Ends_Name, Gross_Weight, Tare_Weight, Net_Weight,Shift,Warp_Beam_No,Start_Time,End_Time,Total_Time,Front_Warper_1_IdNo,Front_Warper_1_Meters,Back_Warper_1_IdNo,Back_Warper_1_meters,Helper_1_IdNo,Helper_1_Meters,Front_Warper_2_IdNo,Front_Warper_2_Meters,Back_Warper_2_IdNo,Back_Warper_2_meters,Helper_2_IdNo,Helper_2_Meters,Remarks, Warp_Meters ) Values ('" & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_SetNo.Text) & "', " & Str(Val(OrdByNo)) & ", @SetDate, " & Str(Val(led_id)) & ", " & Str(Val(Cnt_ID)) & ", " & Str(Val(Mil_ID)) & ", " & Str(Val(v_W_SlNo)) & ", " & Str(Val(Sno)) & "," & Val(.Rows(i).Cells(1).Value) & ", " & Str(Val(.Rows(i).Cells(2).Value)) & ",  " & Str(Val(.Rows(i).Cells(3).Value)) & ", " & Str(Val(.Rows(i).Cells(4).Value)) & ",'" & Trim(.Rows(i).Cells(5).Value) & "','" & Trim(.Rows(i).Cells(6).Value) & "', " & Str(Val(.Rows(i).Cells(7).Value)) & ", " & Str(Val(.Rows(i).Cells(8).Value)) & "," & Str(Val(.Rows(i).Cells(9).Value)) & "," & Val(FtEmp_ID) & ", " & Str(Val(.Rows(i).Cells(11).Value)) & "," & Val(BkEmp_ID) & ", " & Str(Val(.Rows(i).Cells(13).Value)) & "," & Val(HrEmp_ID) & ", " & Str(Val(.Rows(i).Cells(15).Value)) & ", " & Val(Ft2Emp_ID) & ", " & Str(Val(.Rows(i).Cells(17).Value)) & "," & Val(Bk2Emp_ID) & ", " & Str(Val(.Rows(i).Cells(19).Value)) & "," & Val(Hr2Emp_ID) & ", " & Str(Val(.Rows(i).Cells(21).Value)) & ",'" & Trim(.Rows(i).Cells(22).Value) & "'," & Str(Val(v_W_Mtrs)) & ")"
                        cmd.ExecuteNonQuery()

                    End If

                Next



            End With

            With dgv_WarpingDetails_Set2
                Sno = 100

                v_W_SlNo = 2
                v_W_Mtrs = 0

                a = Split(Trim(txt_WarpMeters.Text), ",")
                If UBound(a) >= 1 Then v_W_Mtrs = Val(a(1))

                For i = 0 To .RowCount - 1

                    If Val(.Rows(i).Cells(4).Value) <> 0 Then

                        Sno = Sno + 1
                        Eds2FtEmp_ID = Common_Procedures.Employee_NameToIdNo(Con, .Rows(i).Cells(10).Value, tr)
                        Eds2BkEmp_ID = Common_Procedures.Employee_NameToIdNo(Con, .Rows(i).Cells(12).Value, tr)
                        Eds2HrEmp_ID = Common_Procedures.Employee_NameToIdNo(Con, .Rows(i).Cells(14).Value, tr)
                        Eds2Ft2Emp_ID = Common_Procedures.Employee_NameToIdNo(Con, .Rows(i).Cells(16).Value, tr)
                        Eds2Bk2Emp_ID = Common_Procedures.Employee_NameToIdNo(Con, .Rows(i).Cells(18).Value, tr)
                        Eds2Hr2Emp_ID = Common_Procedures.Employee_NameToIdNo(Con, .Rows(i).Cells(20).Value, tr)

                        cmd.CommandText = "Insert into Warping_Details(Warp_Code, Company_IdNo, Warp_No, for_OrderBy, Warp_Date, Ledger_IdNo, count_idno, Mill_IdNo, Warp_SlNo, Sl_No, Ends_Name, Gross_Weight, Tare_Weight, Net_Weight,Shift,Warp_Beam_No,Start_Time,End_Time,Total_Time,Front_Warper_1_IdNo,Front_Warper_1_Meters,Back_Warper_1_IdNo,Back_Warper_1_meters,Helper_1_IdNo,Helper_1_Meters,Front_Warper_2_IdNo,Front_Warper_2_Meters,Back_Warper_2_IdNo,Back_Warper_2_meters,Helper_2_IdNo,Helper_2_Meters,Remarks, Warp_Meters ) Values ('" & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_SetNo.Text) & "', " & Str(Val(OrdByNo)) & ", @SetDate, " & Str(Val(led_id)) & ", " & Str(Val(Cnt_ID)) & ", " & Str(Val(Mil_ID)) & ", " & Str(Val(v_W_SlNo)) & ", " & Str(Val(Sno)) & ", " & Str(Val(.Rows(i).Cells(1).Value)) & ", " & Val(.Rows(i).Cells(2).Value) & ", " & Str(Val(.Rows(i).Cells(3).Value)) & ", " & Str(Val(.Rows(i).Cells(4).Value)) & ", '" & Trim(.Rows(i).Cells(5).Value) & "', '" & Trim(.Rows(i).Cells(6).Value) & "'," & Str(Val(.Rows(i).Cells(7).Value)) & ", " & Str(Val(.Rows(i).Cells(8).Value)) & "," & Str(Val(.Rows(i).Cells(9).Value)) & "," & Val(Eds2FtEmp_ID) & ", " & Str(Val(.Rows(i).Cells(11).Value)) & "," & Val(Eds2BkEmp_ID) & ", " & Str(Val(.Rows(i).Cells(13).Value)) & "," & Val(Eds2HrEmp_ID) & ", " & Str(Val(.Rows(i).Cells(15).Value)) & ", " & Val(Eds2Hr2Emp_ID) & ", " & Str(Val(.Rows(i).Cells(17).Value)) & "," & Val(Eds2Bk2Emp_ID) & ", " & Str(Val(.Rows(i).Cells(19).Value)) & "," & Val(Eds2Hr2Emp_ID) & ", " & Str(Val(.Rows(i).Cells(21).Value)) & ",'" & Trim(.Rows(i).Cells(22).Value) & "'," & Str(Val(v_W_Mtrs)) & ")"
                        cmd.ExecuteNonQuery()

                    End If

                Next
            End With

            With dgv_WarpingDetails_Set3
                Sno = 200

                v_W_SlNo = 3
                v_W_Mtrs = 0

                a = Split(Trim(txt_WarpMeters.Text), ",")
                If UBound(a) >= 2 Then v_W_Mtrs = Val(a(2))

                For i = 0 To .RowCount - 1

                    If Val(.Rows(i).Cells(4).Value) <> 0 Then

                        Sno = Sno + 1
                        Eds3FtEmp_ID = Common_Procedures.Employee_NameToIdNo(Con, .Rows(i).Cells(10).Value, tr)
                        Eds3BkEmp_ID = Common_Procedures.Employee_NameToIdNo(Con, .Rows(i).Cells(12).Value, tr)
                        Eds3HrEmp_ID = Common_Procedures.Employee_NameToIdNo(Con, .Rows(i).Cells(14).Value, tr)
                        Eds3Ft2Emp_ID = Common_Procedures.Employee_NameToIdNo(Con, .Rows(i).Cells(16).Value, tr)
                        Eds3Bk2Emp_ID = Common_Procedures.Employee_NameToIdNo(Con, .Rows(i).Cells(18).Value, tr)
                        Eds3Hr2Emp_ID = Common_Procedures.Employee_NameToIdNo(Con, .Rows(i).Cells(20).Value, tr)

                        cmd.CommandText = "Insert into Warping_Details(Warp_Code, Company_IdNo, Warp_No, for_OrderBy, Warp_Date, Ledger_IdNo, count_idno, Mill_IdNo, Warp_SlNo, Sl_No, Ends_Name, Gross_Weight, Tare_Weight, Net_Weight,Shift,Warp_Beam_No,Start_Time,End_Time,Total_Time,Front_Warper_1_IdNo,Front_Warper_1_Meters,Back_Warper_1_IdNo,Back_Warper_1_meters,Helper_1_IdNo,Helper_1_Meters,Front_Warper_2_IdNo,Front_Warper_2_Meters,Back_Warper_2_IdNo,Back_Warper_2_meters,Helper_2_IdNo,Helper_2_Meters,Remarks, Warp_Meters ) Values ('" & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_SetNo.Text) & "', " & Str(Val(OrdByNo)) & ", @SetDate, " & Str(Val(led_id)) & ", " & Str(Val(Cnt_ID)) & ", " & Str(Val(Mil_ID)) & ", " & Str(Val(v_W_SlNo)) & ", " & Str(Val(Sno)) & ", " & Str(Val(.Rows(i).Cells(1).Value)) & ", " & Val(.Rows(i).Cells(2).Value) & ", " & Str(Val(.Rows(i).Cells(3).Value)) & ", " & Str(Val(.Rows(i).Cells(4).Value)) & ",'" & Trim(.Rows(i).Cells(5).Value) & "','" & Trim(.Rows(i).Cells(6).Value) & "', " & Str(Val(.Rows(i).Cells(7).Value)) & ", " & Str(Val(.Rows(i).Cells(8).Value)) & "," & Str(Val(.Rows(i).Cells(9).Value)) & "," & Val(Eds3FtEmp_ID) & ", " & Str(Val(.Rows(i).Cells(11).Value)) & "," & Val(Eds3BkEmp_ID) & ", " & Str(Val(.Rows(i).Cells(13).Value)) & "," & Val(Eds3HrEmp_ID) & ", " & Str(Val(.Rows(i).Cells(15).Value)) & ", " & Val(Eds3Ft2Emp_ID) & ", " & Str(Val(.Rows(i).Cells(17).Value)) & "," & Val(Eds3Bk2Emp_ID) & ", " & Str(Val(.Rows(i).Cells(19).Value)) & "," & Val(Eds3Hr2Emp_ID) & ", " & Str(Val(.Rows(i).Cells(21).Value)) & ",'" & Trim(.Rows(i).Cells(22).Value) & "', " & Str(Val(v_W_Mtrs)) & ")"
                        cmd.ExecuteNonQuery()

                    End If

                Next
            End With



            With dgv_YarnTakenDetails
                Sno = 0

                For i = 0 To .RowCount - 1

                    If Val(.Rows(i).Cells(8).Value) <> 0 Then

                        Sno = Sno + 1

                        YCnt_ID = Common_Procedures.Count_NameToIdNo(Con, dgv_YarnTakenDetails.Rows(i).Cells(1).Value, tr)

                        YMil_ID = Common_Procedures.Mill_NameToIdNo(Con, dgv_YarnTakenDetails.Rows(i).Cells(4).Value, tr)
                        YLoc_ID = Common_Procedures.Ledger_NameToIdNo(Con, dgv_YarnTakenDetails.Rows(i).Cells(11).Value, tr)

                        cmd.CommandText = "Insert into Warping_YarnTaken_Details ( Warp_Code ,              Company_IdNo        ,               Warp_No          ,            for_OrderBy   , Warp_Date,           Ledger_IdNo   ,            Sl_No     ,           count_idno     ,                  Yarn_Type             , BabyCone_Warpcode_forSelection,           Mill_IdNo      ,                     Bags                 ,                   Weight_Cone            ,                     Cones                ,                 Gross_Weight                ,                Tare_Weight            ,     Weight               ,Location_IdNo,Lot_No  ) " &
                                                                " Values ( '" & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_SetNo.Text) & "', " & Str(Val(OrdByNo)) & ", @SetDate, " & Str(Val(led_id)) & ", " & Str(Val(Sno)) & ", " & Str(Val(YCnt_ID)) & ", '" & Trim(.Rows(i).Cells(2).Value) & "',         '" & Trim(.Rows(i).Cells(3).Value) & "'                 , " & Str(Val(YMil_ID)) & ", " & Str(Val(.Rows(i).Cells(5).Value)) & ", " & Str(Val(.Rows(i).Cells(6).Value)) & ", " & Str(Val(.Rows(i).Cells(7).Value)) & ", " & Str(Val(.Rows(i).Cells(8).Value)) & ", " & Str(Val(.Rows(i).Cells(9).Value)) & ", " & Str(Val(.Rows(i).Cells(10).Value)) & "," & Str(Val(YLoc_ID)) & ",'" & Trim(.Rows(i).Cells(12).Value) & "' )"
                        cmd.ExecuteNonQuery()

                        If Trim(Common_Procedures.settings.CustomerCode) = "1288" Then

                            Delv_ID = led_id
                            Rec_ID = 0
                            If Val(.Rows(i).Cells(8).Value) < 0 Then
                                Delv_ID = 0
                                Rec_ID = led_id
                            End If

                            Prtcls2 = "YarnTkn : Set.No. " & Trim(lbl_SetNo.Text)

                            vSetCd = Trim(NewCode)
                            vSetNo = Trim(lbl_SetNo.Text)

                            vOrdByNo = Common_Procedures.OrderBy_CodeToValue(Trim(lbl_SetNo.Text))

                            vYrnPartcls = Prtcls2

                            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1288" Then

                                cmd.CommandText = "Insert into Stock_Yarn_Processing_Details ( SoftwareType_IdNo                         , Reference_Code,                        Company_IdNo         ,            Reference_No       ,           for_OrderBy    , Reference_Date,        DeliveryTo_Idno   ,    ReceivedFrom_Idno    ,     Party_Bill_No    ,                Sl_No      ,           Count_IdNo     ,                   Yarn_Type            ,          Mill_IdNo       ,                               Bags                 ,                               Cones                ,                               Weight               ,        Particulars     ,  Posting_For,         Set_Code      ,            Set_No  ,      WareHouse_IdNo   ,Lot_No  ) " &
                                                "   Values ( " & Str(Val(Common_Procedures.SoftwareTypes.Sizing_Software)) & " , ,'" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_SetNo.Tag) & "', " & Str(Val(vOrdByNo)) & ",     @SetDate  , " & Str(Val(Delv_ID)) & ", " & Str(Val(Rec_ID)) & ", '" & Trim(PBlNo) & "', " & Str(-1 * Val(Sno)) & ", " & Str(Val(YCnt_ID)) & ", '" & Trim(.Rows(i).Cells(2).Value) & "', " & Str(Val(YMil_ID)) & ", " & Str(Math.Abs(Val(.Rows(i).Cells(5).Value))) & ", " & Str(Math.Abs(Val(.Rows(i).Cells(7).Value))) & ", " & Str(Math.Abs(Val(.Rows(i).Cells(8).Value))) & ", '" & Trim(vYrnPartcls) & "',  'YARNTAKEN', '" & Trim(vSetCd) & "', '" & Trim(vSetNo) & "'," & Str(Val(YLoc_ID)) & ",'" & Trim(.Rows(i).Cells(12).Value) & "') "
                                cmd.ExecuteNonQuery()

                            End If
                        End If

                        Prtcls2 = ""

                        Prtcls2 = "YarnTkn : Warp.No. " & Trim(lbl_SetNo.Text)

                        vByWarpCd = ""
                        vByWarpNo = ""

                        If Trim(UCase(dgv_YarnTakenDetails.Rows(i).Cells(2).Value)) = "BABY" And Trim(Trim(dgv_YarnTakenDetails.Rows(i).Cells(3).Value)) <> "" Then
                            Da = New SqlClient.SqlDataAdapter("select a.Warp_Code, a.Warp_No from Warping_Head a where a.Warpcode_forSelection = '" & Trim(Trim(dgv_YarnTakenDetails.Rows(i).Cells(3).Value)) & "'", Con)
                            Da.SelectCommand.Transaction = tr
                            Dt1 = New DataTable
                            Da.Fill(Dt1)
                            If Dt1.Rows.Count > 0 Then
                                vByWarpCd = Dt1.Rows(0).Item("Warp_Code").ToString
                                vByWarpNo = Dt1.Rows(0).Item("Warp_No").ToString
                            End If
                            Dt1.Clear()
                        End If

                    End If

                Next

            End With

            With dgv_BabyConeDetails
                Sno = 0

                For i = 0 To .RowCount - 1

                    If Val(.Rows(i).Cells(3).Value) <> 0 Then

                        Sno = Sno + 1

                        cmd.CommandText = "Insert into Warping_BabyCone_Details(Warp_Code, Company_IdNo, Warp_No, for_OrderBy, Warp_Date, Ledger_IdNo, Mill_IdNo, Count_IdNo, Sl_No, Bags, Cones, Gross_Weight,Tare_Weight,		Net_Weight,			Lot_No) Values ('" & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_SetNo.Text) & "', " & Str(Val(OrdByNo)) & ", @SetDate, " & Str(Val(led_id)) & ", " & Str(Val(Mil_ID)) & ", " & Str(Val(Cnt_ID)) & ", " & Str(Val(Sno)) & ", " & Str(Val(.Rows(i).Cells(1).Value)) & ", " & Str(Val(.Rows(i).Cells(2).Value)) & ", " & Str(Val(.Rows(i).Cells(3).Value)) & ", " & Str(Val(.Rows(i).Cells(4).Value)) & ", " & Str(Val(.Rows(i).Cells(5).Value)) & ", '" & Trim(.Rows(i).Cells(6).Value) & "')"
                        cmd.ExecuteNonQuery()

                    End If

                Next

                If (Trim(Common_Procedures.settings.CustomerCode)) = "1288" Then

                    If Val(lbl_Total_Warping_NetWeight.Text) <> 0 Then

                        Prtcls2 = "Cons : Set.No. " & Trim(lbl_SetNo.Text)
                        Bgs = 0
                        Cns = 0
                        YLotNo = Trim(dgv_YarnTakenDetails.Rows(0).Cells(12).Value)
                        'YLocID = Trim(dgv_YarnTakenDetails.Rows(0).Cells(11).Value)
                        YLocID = Common_Procedures.Ledger_NameToIdNo(Con, dgv_YarnTakenDetails.Rows(0).Cells(11).Value, tr)

                        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1102" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1163" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1144" Then '---- Ganesh karthik Sizing (Somanur)
                            For i = 0 To dgv_YarnTakenDetails.RowCount - 1
                                If Val(dgv_YarnTakenDetails.Rows(i).Cells(8).Value) <> 0 Then
                                    If Trim(UCase(dgv_YarnTakenDetails.Rows(i).Cells(2).Value)) = "MILL" Then
                                        Bgs = Bgs + Val(dgv_YarnTakenDetails.Rows(i).Cells(5).Value)
                                        Cns = Cns + Val(dgv_YarnTakenDetails.Rows(i).Cells(7).Value)
                                    End If
                                End If
                            Next

                        Else
                            Bgs = Val(vTotYrnBags)

                            If chk_RewindingStatus.Checked = True Then
                                Cns = Val(vTotYrnCones)
                            End If

                        End If

                        vYrnPartcls = Prtcls2
                        'If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1220" Then '---- Meenashi Sizing (Somanur)
                        '    vYrnPartcls = vYrnPartcls & ",  Mill :  " & Trim(cbo_MillName.Text)
                        'End If


                        cmd.CommandText = "Insert into Stock_Yarn_Processing_Details (SoftwareType_IdNo , Reference_Code, Company_IdNo, Reference_No, for_OrderBy, Reference_Date, DeliveryTo_Idno, ReceivedFrom_Idno, Party_Bill_No, Sl_No, Count_IdNo, Yarn_Type, Mill_IdNo, Bags, Cones, Weight, Particulars, Posting_For, Set_Code, Set_No,     WareHouse_IdNo   ,Lot_No ) Values (" & Str(Val(Common_Procedures.SoftwareTypes.Sizing_Software)) & " , '" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_SetNo.Tag) & "', " & Str(Val(vOrdByNo)) & ", @SetDate, " & Str(Val(led_id)) & ", 0, '" & Trim(PBlNo) & "', 1, " & Str(Val(Cnt_ID)) & ", 'MILL', " & Str(Val(Mil_ID)) & ", " & Str(Val(Bgs)) & ", " & Str(Val(Cns)) & ", " & Str(Val(lbl_Total_Warping_NetWeight.Text)) & ", '" & Trim(vYrnPartcls) & "', 'CONSUMEDYARN', '" & Trim(NewCode) & "', '" & Trim(lbl_SetNo.Tag) & "'," & Str(Val(YLocID)) & ",'" & Trim(YLotNo) & "' )"
                        cmd.ExecuteNonQuery()

                        ' YARN RECEIPT MODEL
                        'cmd.CommandText = "Insert into Stock_Yarn_Processing_Details(                   Reference_Code            ,                 Company_IdNo      ,               Reference_No         ,            for_OrderBy     , Reference_Date , DeliveryTo_Idno,     ReceivedFrom_Idno    ,      Party_Bill_No    ,               Sl_No   ,             Count_IdNo   ,                           Yarn_Type                ,            Mill_IdNo     ,                           Weight_Bag                 ,                             Cones_Bag                ,                         Weight_Cone                  ,                           Bags                       ,                            Cones                     ,                             Weight                   ,          Particulars        , Posting_For, Set_Code, Set_No,     WareHouse_IdNo   ,Lot_No  ) " & _
                        '         "Values                                   ('" & Trim(Pk_Condition) & Trim(NewCode) & "' , " & Str(Val(lbl_Company.Tag)) & " , '" & Trim(lbl_ReceiptNo.Text) & "' , " & Str(Val(vOrdByNo)) & " ,  @ReceiptDate  ,          0     , " & Str(Val(led_id)) & " , '" & Trim(PBlNo) & "' , " & Str(Val(Sno)) & " , " & Str(Val(Cnt_ID)) & " , '" & Trim(dgv_Details.Rows(i).Cells(2).Value) & "' , " & Str(Val(Mil_ID)) & " , " & Str(Val(dgv_Details.Rows(i).Cells(4).Value)) & " , " & Str(Val(dgv_Details.Rows(i).Cells(5).Value)) & " , " & Str(Val(dgv_Details.Rows(i).Cells(6).Value)) & " , " & Str(Val(dgv_Details.Rows(i).Cells(7).Value)) & " , " & Str(Val(dgv_Details.Rows(i).Cells(8).Value)) & " , " & Str(Val(dgv_Details.Rows(i).Cells(9).Value)) & " , '" & Trim(vYrnPartcls) & "' ,   'RECEIPT',      '' ,    '' , " & Str(Val(Gd_Id)) & ",'" & Trim(dgv_Details.Rows(i).Cells(11).Value) & "')"
                        'cmd.ExecuteNonQuery()



                    End If

                    If Val(lbl_ExcessShort.Text) <> 0 Then

                        Delv_ID = 0 : Rec_ID = 0
                        If Val(lbl_ExcessShort.Text) < 0 Then
                            Delv_ID = Val(led_id)
                            Prtcls2 = "Short : Set.No. " & Trim(lbl_SetNo.Text)
                        Else
                            Rec_ID = Val(led_id)
                            Prtcls2 = "Excess : Set.No. " & Trim(lbl_SetNo.Text)
                        End If

                        vYrnPartcls = Prtcls2
                        'If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1220" Then '---- Meenashi Sizing (Somanur)
                        '    vYrnPartcls = vYrnPartcls & ",  Mill :  " & Trim(cbo_MillName.Text)
                        'End If


                        cmd.CommandText = "Insert into Stock_Yarn_Processing_Details (   SoftwareType_IdNo  ,                                      Reference_Code,              Company_IdNo         ,             Reference_No      ,         for_OrderBy      , Reference_Date,          DeliveryTo_Idno ,      ReceivedFrom_Idno  ,        Party_Bill_No , Sl_No,         Count_IdNo      , Yarn_Type,          Mill_IdNo      , Bags, Cones,                      Weight                     ,         Particulars    ,  Posting_For  ,          Set_Code      ,             Set_No          ,WareHouse_IdNo,Lot_No   ) " &
                                               " Values (" & Str(Val(Common_Procedures.SoftwareTypes.Sizing_Software)) & " , '" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_SetNo.Tag) & "', " & Str(Val(vOrdByNo)) & ",    @SetDate   , " & Str(Val(Delv_ID)) & ", " & Str(Val(Rec_ID)) & ", '" & Trim(PBlNo) & "',   2  , " & Str(Val(Cnt_ID)) & ",   'MILL' , " & Str(Val(Mil_ID)) & ",   0 ,    0 , " & Str(Math.Abs(Val(lbl_ExcessShort.Text))) & ", '" & Trim(vYrnPartcls) & "', 'CONSUMEDYARN', '" & Trim(NewCode) & "', '" & Trim(lbl_SetNo.Tag) & "'," & Str(Val(YLocID)) & ",'" & Trim(YLotNo) & "') "
                        cmd.ExecuteNonQuery()



                    End If


                    ' 07Aug2019 here we make sure to check No need Posting 
                    If Val(lbl_BabyCone_NetWeight.Text) > 0 And chk_NoNeedPosting.Checked = False Then

                        If chk_RewindingStatus.Checked = True Then
                            Yrn_Typ = "R/W"
                            Mid = RwMil_ID
                            Bgs = 0
                            Cns = Val(txt_RewindingCones.Text)
                            StNo = ""
                            Prtcls2 = "R/W.Cn : Set.No. " & Trim(lbl_SetNo.Text)
                            vYrnPartcls = Prtcls2
                            'If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1220" Then '---- Meenashi Sizing (Somanur)
                            '    vYrnPartcls = vYrnPartcls & ",  Mill :  " & Trim(cbo_Rw_MillName.Text)
                            'End If

                        Else
                            Yrn_Typ = "BABY"
                            Mid = Mil_ID
                            Bgs = Val(vTotBabyBags)
                            Cns = Val(vTotBabyCones)
                            StNo = Trim(lbl_SetNo.Tag)
                            Prtcls2 = "Baby.Cn : Set.No. " & Trim(lbl_SetNo.Text)
                            vYrnPartcls = Prtcls2
                            'If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1220" Then '---- Meenashi Sizing (Somanur)
                            '    vYrnPartcls = vYrnPartcls & ",  Mill :  " & Trim(cbo_MillName.Text)
                            'End If

                        End If

                        Wt = Val(lbl_BabyCone_NetWeight.Text) + Val(txt_RwExcSht.Text)


                        cmd.CommandText = "Insert into Stock_Yarn_Processing_Details (         SoftwareType_IdNo  ,                      Reference_Code,                          Company_IdNo         ,            Reference_No       ,           for_OrderBy    , Reference_Date, DeliveryTo_Idno,     ReceivedFrom_Idno   ,     Party_Bill_No    , Sl_No,           Count_IdNo    ,         Yarn_Type      ,         Mill_IdNo    ,            Bags      ,           Cones      ,          Weight     ,        Particulars     ,  Posting_For,         Set_Code       ,            Set_No         ,WareHouse_IdNo,Lot_No     ) " &
                                            "   Values (" & Str(Val(Common_Procedures.SoftwareTypes.Sizing_Software)) & " ,  '" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_SetNo.Tag) & "', " & Str(Val(vOrdByNo)) & ",     @SetDate  ,        0       , " & Str(Val(led_id)) & ", '" & Trim(PBlNo) & "',   3  , " & Str(Val(Cnt_ID)) & ", '" & Trim(Yrn_Typ) & "', " & Str(Val(Mid)) & ", " & Str(Val(Bgs)) & ", " & Str(Val(Cns)) & ", " & Str(Val(Wt)) & ", '" & Trim(vYrnPartcls) & "',  'YARNTAKEN', '" & Trim(NewCode) & "', '" & Trim(lbl_SetNo.Tag) & "'," & Str(Val(YLocID)) & ",'" & Trim(YLotNo) & "' ) "
                        cmd.ExecuteNonQuery()



                    End If

                End If

            End With

            tr.Commit()

            MessageBox.Show("Saved Sucessfully!!!", "FOR SAVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Information)

            If Val(Common_Procedures.settings.OnSave_MoveTo_NewEntry_Status) = 1 Then
                If New_Entry = True Then
                    new_record()
                Else
                    move_record(lbl_SetNo.Text)
                End If
            Else
                move_record(lbl_SetNo.Text)
            End If

        Catch ex As Exception
            tr.Rollback()
            MessageBox.Show(ex.Message, "FOR SAVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Finally

            cmd.Dispose()
            tr.Dispose()

            If cbo_Ledger.Enabled And cbo_Ledger.Visible Then cbo_Ledger.Focus()

        End Try

    End Sub

    Private Sub Warping_Entry_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated
        Dim dt1 As New DataTable

        Try

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Ledger.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "LEDGER" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Ledger.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_CountName.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "COUNT" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_CountName.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Grid_CountName.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "COUNT" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Grid_CountName.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_MillName.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "MILL" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_MillName.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Grid_MillName.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "MILL" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Grid_MillName.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Rw_MillName.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "MILL" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Rw_MillName.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_BeamWidth.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "BEAMWIDTH" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_BeamWidth.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If



            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Ends2_FrontWarper1.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "EMPLOYEE" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Ends2_FrontWarper1.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Ends2_FrontWarper2.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "EMPLOYEE" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Ends2_FrontWarper2.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Ends2_BackWarper1.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "EMPLOYEE" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Ends2_BackWarper1.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Ends2_BackWarper2.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "EMPLOYEE" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Ends2_BackWarper2.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Ends2_Helper1.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "EMPLOYEE" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Ends2_Helper1.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If
            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Ends2_Helper2.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "EMPLOYEE" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Ends2_Helper2.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Grid_FrontWarper_1.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "EMPLOYEE" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Grid_FrontWarper_1.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Grid_FrontWarper2.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "EMPLOYEE" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Grid_FrontWarper2.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Grid_BackWarper1.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "EMPLOYEE" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Grid_BackWarper1.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Grid_BackWarper2.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "EMPLOYEE" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Grid_BackWarper2.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Grid_Helper1.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "EMPLOYEE" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Grid_Helper1.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If
            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Grid_Helper2.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "EMPLOYEE" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Grid_Helper2.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If


            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Ends3_FrontWarper1.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "EMPLOYEE" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Ends3_FrontWarper1.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Ends3_FrontWarper2.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "EMPLOYEE" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Ends3_FrontWarper2.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Ends3_BackWarper1.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "EMPLOYEE" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Ends3_BackWarper1.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Ends3_BackWarper2.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "EMPLOYEE" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Ends3_BackWarper2.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Ends3_Helper1.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "EMPLOYEE" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Ends3_Helper1.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If
            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Ends3_Helper2.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "EMPLOYEE" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Ends3_Helper2.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If








            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            If FrmLdSTS = True Then

                lbl_Company.Text = ""
                lbl_Company.Tag = 0
                Common_Procedures.CompIdNo = 0

                Me.Text = ""

                lbl_Company.Text = Common_Procedures.get_Company_From_CompanySelection(Con)
                lbl_Company.Tag = Val(Common_Procedures.CompIdNo)

                Me.Text = lbl_Company.Text

                new_record()

            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "DOES NOT SHOW...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

        FrmLdSTS = False


    End Sub

    Private Sub Warping_Entry_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
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
        Dim dt10 As New DataTable
        Dim dt11 As New DataTable
        Dim dt12 As New DataTable
        Dim dt13 As New DataTable
        Dim dt14 As New DataTable
        Dim dt15 As New DataTable
        Dim dt16 As New DataTable
        Dim dt17 As New DataTable
        Dim dt18 As New DataTable
        Dim dt19 As New DataTable
        Dim dt20 As New DataTable
        Dim dt21 As New DataTable
        Dim dt22 As New DataTable
        Dim dt23 As New DataTable
        Dim dt24 As New DataTable
        Dim dt25 As New DataTable
        Dim dt26 As New DataTable
        Dim dt27 As New DataTable
        Dim dt28 As New DataTable
        Dim dt29 As New DataTable
        Dim dt30 As New DataTable
        Dim dt31 As New DataTable
        Dim dt32 As New DataTable
        Dim dt33 As New DataTable
        Dim dt34 As New DataTable
        Dim dt35 As New DataTable
        Dim dt36 As New DataTable
        Dim dt37 As New DataTable
        Dim dt38 As New DataTable
        Dim dt39 As New DataTable
        Dim dt40 As New DataTable
        Dim dt41 As New DataTable
        Dim dt42 As New DataTable
        Dim dt43 As New DataTable
        Dim dt44 As New DataTable
        Dim dt45 As New DataTable
        Dim dt46 As New DataTable
        Dim dt47 As New DataTable
        Dim dt48 As New DataTable
        Dim dt49 As New DataTable
        Dim dt50 As New DataTable
        Dim dt51 As New DataTable
        Dim dt52 As New DataTable
        Dim dt53 As New DataTable

        Dim dtLocation As New DataTable

        Dim i As Integer = 0

        Me.Text = ""

        Con.Open()

        da = New SqlClient.SqlDataAdapter("select a.Ledger_DisplayName from Ledger_AlaisHead a where (Ledger_Type = 'GODOWN') and (Ledger_IdNo = 0) order by a.Ledger_DisplayName", Con)
        da.Fill(dtLocation)
        cbo_Ledger.DataSource = dtLocation
        cbo_Ledger.DisplayMember = "Ledger_DisplayName"


        da = New SqlClient.SqlDataAdapter("select a.Ledger_DisplayName from Ledger_AlaisHead a, ledger_head b where (a.Ledger_IdNo = 0 or b.AccountsGroup_IdNo = 10) and a.Ledger_IdNo = b.Ledger_IdNo order by a.Ledger_DisplayName", Con)
        da.Fill(dt1)
        cbo_Ledger.DataSource = dt1
        cbo_Ledger.DisplayMember = "Ledger_DisplayName"

        da = New SqlClient.SqlDataAdapter("select count_name from Count_Head order by count_name", Con)
        da.Fill(dt2)
        cbo_CountName.DataSource = dt2
        cbo_CountName.DisplayMember = "count_name"

        da = New SqlClient.SqlDataAdapter("select mill_name from Mill_Head order by mill_name", Con)
        da.Fill(dt3)
        cbo_MillName.DataSource = dt3
        cbo_MillName.DisplayMember = "mill_name"

        da = New SqlClient.SqlDataAdapter("select Beam_Width_Name from Beam_Width_Head order by Beam_Width_Name", Con)
        da.Fill(dt4)
        cbo_BeamWidth.DataSource = dt4
        cbo_BeamWidth.DisplayMember = "Beam_Width_Name"

        da = New SqlClient.SqlDataAdapter("select mill_name from Mill_Head order by mill_name", Con)
        da.Fill(dt5)
        cbo_Grid_MillName.DataSource = dt5
        cbo_Grid_MillName.DisplayMember = "mill_name"


        da = New SqlClient.SqlDataAdapter("select count_name from Count_Head order by count_name", Con)
        da.Fill(dt7)
        cbo_Grid_CountName.DataSource = dt7
        cbo_Grid_CountName.DisplayMember = "count_name"

        da = New SqlClient.SqlDataAdapter("select mill_name from Mill_Head order by mill_name", Con)
        da.Fill(dt8)
        cbo_Rw_MillName.DataSource = dt8
        cbo_Rw_MillName.DisplayMember = "mill_name"

        da = New SqlClient.SqlDataAdapter("select yarn_type from YarnType_Head where yarn_type <> 'BABY' order by yarn_type", Con)
        da.Fill(dt9)
        cbo_Grid_YarnType.DataSource = dt9
        cbo_Grid_YarnType.DisplayMember = "yarn_type"

        da = New SqlClient.SqlDataAdapter("select Employee_name from PayRoll_Employee_Head order by Employee_name", Con)
        da.Fill(dt10)
        cbo_Grid_FrontWarper_1.DataSource = dt10
        cbo_Grid_FrontWarper_1.DisplayMember = "Employee_name"

        da = New SqlClient.SqlDataAdapter("select Employee_name from PayRoll_Employee_Head order by Employee_name", Con)
        da.Fill(dt11)
        cbo_Grid_FrontWarper2.DataSource = dt11
        cbo_Grid_FrontWarper2.DisplayMember = "Employee_name"

        da = New SqlClient.SqlDataAdapter("select Employee_name from PayRoll_Employee_Head order by Employee_name", Con)
        da.Fill(dt12)
        cbo_Grid_BackWarper1.DataSource = dt12
        cbo_Grid_BackWarper1.DisplayMember = "Employee_name"

        da = New SqlClient.SqlDataAdapter("select Employee_name from PayRoll_Employee_Head order by Employee_name", Con)
        da.Fill(dt13)
        cbo_Grid_BackWarper2.DataSource = dt13
        cbo_Grid_BackWarper2.DisplayMember = "Employee_name"

        da = New SqlClient.SqlDataAdapter("select Employee_name from PayRoll_Employee_Head order by Employee_name", Con)
        da.Fill(dt14)
        cbo_Grid_Helper1.DataSource = dt14
        cbo_Grid_Helper1.DisplayMember = "Employee_name"

        da = New SqlClient.SqlDataAdapter("select Employee_name from PayRoll_Employee_Head order by Employee_name", Con)
        da.Fill(dt15)
        cbo_Grid_Helper2.DataSource = dt15
        cbo_Grid_Helper2.DisplayMember = "Employee_name"

        da = New SqlClient.SqlDataAdapter("select Employee_name from PayRoll_Employee_Head order by Employee_name", Con)
        da.Fill(dt16)
        cbo_Ends2_FrontWarper1.DataSource = dt16
        cbo_Ends2_FrontWarper1.DisplayMember = "Employee_name"

        da = New SqlClient.SqlDataAdapter("select Employee_name from PayRoll_Employee_Head order by Employee_name", Con)
        da.Fill(dt17)
        cbo_Ends2_FrontWarper2.DataSource = dt17
        cbo_Ends2_FrontWarper2.DisplayMember = "Employee_name"

        da = New SqlClient.SqlDataAdapter("select Employee_name from PayRoll_Employee_Head order by Employee_name", Con)
        da.Fill(dt18)
        cbo_Ends2_BackWarper1.DataSource = dt18
        cbo_Ends2_BackWarper1.DisplayMember = "Employee_name"

        da = New SqlClient.SqlDataAdapter("select Employee_name from PayRoll_Employee_Head order by Employee_name", Con)
        da.Fill(dt19)
        cbo_Ends2_BackWarper2.DataSource = dt19
        cbo_Ends2_BackWarper2.DisplayMember = "Employee_name"

        da = New SqlClient.SqlDataAdapter("select Employee_name from PayRoll_Employee_Head order by Employee_name", Con)
        da.Fill(dt21)
        cbo_Ends2_Helper1.DataSource = dt21
        cbo_Ends2_Helper1.DisplayMember = "Employee_name"

        da = New SqlClient.SqlDataAdapter("select Employee_name from PayRoll_Employee_Head order by Employee_name", Con)
        da.Fill(dt22)
        cbo_Ends2_Helper2.DataSource = dt22
        cbo_Ends2_Helper2.DisplayMember = "Employee_name"

        da = New SqlClient.SqlDataAdapter("select Employee_name from PayRoll_Employee_Head order by Employee_name", Con)
        da.Fill(dt23)
        cbo_Ends3_FrontWarper1.DataSource = dt23
        cbo_Ends3_FrontWarper1.DisplayMember = "Employee_name"

        da = New SqlClient.SqlDataAdapter("select Employee_name from PayRoll_Employee_Head order by Employee_name", Con)
        da.Fill(dt24)
        cbo_Ends3_FrontWarper2.DataSource = dt24
        cbo_Ends3_FrontWarper2.DisplayMember = "Employee_name"

        da = New SqlClient.SqlDataAdapter("select Employee_name from PayRoll_Employee_Head order by Employee_name", Con)
        da.Fill(dt25)
        cbo_Ends3_BackWarper1.DataSource = dt25
        cbo_Ends3_BackWarper1.DisplayMember = "Employee_name"

        da = New SqlClient.SqlDataAdapter("select Employee_name from PayRoll_Employee_Head order by Employee_name", Con)
        da.Fill(dt26)
        cbo_Ends3_BackWarper2.DataSource = dt26
        cbo_Ends3_BackWarper2.DisplayMember = "Employee_name"

        da = New SqlClient.SqlDataAdapter("select Employee_name from PayRoll_Employee_Head order by Employee_name", Con)
        da.Fill(dt27)
        cbo_Ends3_Helper1.DataSource = dt27
        cbo_Ends3_Helper1.DisplayMember = "Employee_name"


        da = New SqlClient.SqlDataAdapter("select Employee_name from PayRoll_Employee_Head order by Employee_name", Con)
        da.Fill(dt28)
        cbo_Ends3_Helper2.DataSource = dt28
        cbo_Ends3_Helper2.DisplayMember = "Employee_name"



        da = New SqlClient.SqlDataAdapter("select distinct(Warpcode_forSelection) from Stock_BabyCone_Processing_Details order by Warpcode_forSelection", Con)
        da.Fill(dt53)
        cbo_Grid_SetNo.DataSource = dt53
        cbo_Grid_SetNo.DisplayMember = "Warpcode_forSelection"

        cbo_Meters_Yards.Items.Clear()
        cbo_Meters_Yards.Items.Add("METERS")
        cbo_Meters_Yards.Items.Add("YARDS")

        cbo_Ends1_Shift.Items.Clear()
        cbo_Ends1_Shift.Items.Add("")
        cbo_Ends1_Shift.Items.Add("DAY")
        cbo_Ends1_Shift.Items.Add("NIGHT")

        cbo_Ends2_Shift.Items.Clear()
        cbo_Ends2_Shift.Items.Add("")
        cbo_Ends2_Shift.Items.Add("DAY")
        cbo_Ends2_Shift.Items.Add("NIGHT")

        Cbo_Ends3_Shift.Items.Clear()
        Cbo_Ends3_Shift.Items.Add("")
        Cbo_Ends3_Shift.Items.Add("DAY")
        Cbo_Ends3_Shift.Items.Add("NIGHT")




        pnl_Filter.Visible = False
        pnl_Filter.Left = (Me.Width - pnl_Filter.Width) \ 2
        pnl_Filter.Top = (Me.Height - pnl_Filter.Height) \ 2


        pnl_StatementPrint.Visible = False
        pnl_StatementPrint.BringToFront()
        pnl_StatementPrint.Left = (Me.Width - pnl_StatementPrint.Width) \ 2
        pnl_StatementPrint.Top = (Me.Height - pnl_StatementPrint.Height) \ 2

        chk_SocietySet.Visible = False
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1015" Then  '---- WinTraack Textiles Private Limited(Sizing Unit)
            chk_SocietySet.Visible = True
        End If

        lbl_RwExcSht_Caption.Visible = False
        txt_RwExcSht.Visible = False
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1012" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1288" Then '---- Avinashi Sizing (Avinashi)
            lbl_RwExcSht_Caption.Visible = True
            txt_RwExcSht.Visible = True
        End If

        For i = 5 To 22
            dgv_WarpingDetails_Set1.Columns(i).Visible = False
            dgv_WarpingDetails_Set2.Columns(i).Visible = False
            dgv_WarpingDetails_Set3.Columns(i).Visible = False
        Next
        If Val(Common_Procedures.settings.Statement_Production_Wages_For_Sizing) = 1 Then
            For i = 5 To 22
                dgv_WarpingDetails_Set1.Columns(i).Visible = True
                dgv_WarpingDetails_Set2.Columns(i).Visible = True
                dgv_WarpingDetails_Set3.Columns(i).Visible = True
            Next
        End If



        AddHandler dtp_Date.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Ledger.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_MillName.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Rw_MillName.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_BeamWidth.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_CountName.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Grid_CountName.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Grid_MillName.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Grid_YarnType.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Grid_BackWarper1.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Grid_BackWarper2.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Grid_FrontWarper_1.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Grid_FrontWarper2.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Grid_Helper1.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Grid_Helper2.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Grid_SetNo.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_BabyCone_AddLessWgt.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_ExcessShort_AddLess.GotFocus, AddressOf ControlGotFocus

        AddHandler cbo_Ends2_BackWarper1.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Ends2_BackWarper2.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Ends2_FrontWarper1.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Ends2_FrontWarper2.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Ends2_Helper1.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Ends2_Helper2.GotFocus, AddressOf ControlGotFocus

        AddHandler cbo_Ends3_BackWarper1.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Ends3_BackWarper2.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Ends3_FrontWarper1.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Ends3_FrontWarper2.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Ends3_Helper1.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Ends3_Helper2.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Ends1_Shift.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Ends2_Shift.GotFocus, AddressOf ControlGotFocus
        AddHandler Cbo_Ends3_Shift.GotFocus, AddressOf ControlGotFocus

        AddHandler txt_BabyCone_DeliveryWeight.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_BabyCone_TareWeight.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_WgtEmYBag.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_WgtEmYCone.GotFocus, AddressOf ControlGotFocus

        AddHandler lbl_BeamCount.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Ends.GotFocus, AddressOf ControlGotFocus
        AddHandler lbl_ExcessShort.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_InvoiceCode.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_PcsLength.GotFocus, AddressOf ControlGotFocus

        AddHandler txt_Remarks.GotFocus, AddressOf ControlGotFocus
        AddHandler chk_RewindingStatus.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_RewindingCones.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_RwExcSht.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_TapeLength.GotFocus, AddressOf ControlGotFocus

        AddHandler txt_WarpMeters.GotFocus, AddressOf ControlGotFocus

        AddHandler dgv_WarpingDetails_Set1.GotFocus, AddressOf ControlGotFocus
        AddHandler dgv_WarpingDetails_Set2.GotFocus, AddressOf ControlGotFocus
        AddHandler dgv_WarpingDetails_Set3.GotFocus, AddressOf ControlGotFocus


        AddHandler dgv_BabyConeDetails.GotFocus, AddressOf ControlGotFocus
        AddHandler dtp_Filter_Fromdate.GotFocus, AddressOf ControlGotFocus
        AddHandler dtp_Filter_ToDate.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Filter_PartyName.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Filter_MillName.GotFocus, AddressOf ControlGotFocus
        AddHandler chk_SocietySet.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Filter_CountName.GotFocus, AddressOf ControlGotFocus


        AddHandler btn_close.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_Filter_Close.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_Filter_Show.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_save.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_print.GotFocus, AddressOf ControlGotFocus

        AddHandler btn_StmtPrntOk.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_StatmtCancel.GotFocus, AddressOf ControlGotFocus



        AddHandler dtp_Date.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Ledger.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_MillName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Rw_MillName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_BeamWidth.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_CountName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Grid_CountName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Grid_MillName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Grid_YarnType.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Grid_BackWarper1.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Grid_BackWarper2.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Grid_FrontWarper2.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Grid_FrontWarper_1.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Grid_Helper1.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Grid_SetNo.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Grid_Helper2.LostFocus, AddressOf ControlLostFocus
        AddHandler dtp_Filter_Fromdate.LostFocus, AddressOf ControlLostFocus
        AddHandler dtp_Filter_ToDate.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Ends2_BackWarper1.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Ends2_BackWarper2.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Ends2_FrontWarper1.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Ends2_FrontWarper2.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Ends2_Helper1.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Ends2_Helper2.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_BabyCone_AddLessWgt.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_ExcessShort_AddLess.LostFocus, AddressOf ControlLostFocus

        AddHandler cbo_Ends3_BackWarper1.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Ends3_BackWarper2.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Ends3_FrontWarper1.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Ends3_FrontWarper2.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Ends3_Helper1.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Ends3_Helper2.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Ends1_Shift.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Ends2_Shift.LostFocus, AddressOf ControlLostFocus
        AddHandler Cbo_Ends3_Shift.LostFocus, AddressOf ControlLostFocus


        AddHandler lbl_BeamCount.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_BabyCone_DeliveryWeight.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_BabyCone_TareWeight.LostFocus, AddressOf ControlLostFocus

        AddHandler txt_Ends.LostFocus, AddressOf ControlLostFocus
        AddHandler lbl_ExcessShort.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_InvoiceCode.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_PcsLength.LostFocus, AddressOf ControlLostFocus

        AddHandler txt_Remarks.LostFocus, AddressOf ControlLostFocus
        AddHandler chk_RewindingStatus.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_RewindingCones.LostFocus, AddressOf ControlLostFocus

        AddHandler txt_RwExcSht.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_TapeLength.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_WarpMeters.LostFocus, AddressOf ControlLostFocus

        AddHandler dgv_WarpingDetails_Set1.LostFocus, AddressOf ControlLostFocus
        AddHandler dgv_WarpingDetails_Set2.LostFocus, AddressOf ControlLostFocus
        AddHandler dgv_WarpingDetails_Set3.LostFocus, AddressOf ControlLostFocus

        AddHandler dgv_BabyConeDetails.LostFocus, AddressOf ControlLostFocus
        AddHandler chk_SocietySet.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Filter_CountName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Filter_PartyName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Filter_MillName.LostFocus, AddressOf ControlLostFocus

        AddHandler txt_WgtEmYBag.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_WgtEmYCone.LostFocus, AddressOf ControlLostFocus


        AddHandler btn_close.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_Filter_Close.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_Filter_Show.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_save.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_print.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_StmtPrntOk.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_StatmtCancel.LostFocus, AddressOf ControlLostFocus

        AddHandler dtp_Date.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_BabyCone_DeliveryWeight.KeyDown, AddressOf TextBoxControlKeyDown

        AddHandler lbl_BeamCount.KeyDown, AddressOf TextBoxControlKeyDown


        AddHandler txt_Ends.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler lbl_ExcessShort.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_InvoiceCode.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_PcsLength.KeyDown, AddressOf TextBoxControlKeyDown

        AddHandler txt_RewindingCones.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_TapeLength.KeyDown, AddressOf TextBoxControlKeyDown
        ' AddHandler txt_WarpMeters.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler dtp_Filter_Fromdate.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler dtp_Filter_ToDate.KeyDown, AddressOf TextBoxControlKeyDown

        '  AddHandler txt_ExcessShort_AddLess.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_WgtEmYCone.KeyDown, AddressOf TextBoxControlKeyDown

        AddHandler txt_WgtEmYBag.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_WgtEmYCone.KeyPress, AddressOf TextBoxControlKeyPress
        '  AddHandler txt_ExcessShort_AddLess.KeyPress, AddressOf TextBoxControlKeyPress

        AddHandler dtp_Date.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_BabyCone_DeliveryWeight.KeyPress, AddressOf TextBoxControlKeyPress

        AddHandler lbl_BeamCount.KeyPress, AddressOf TextBoxControlKeyPress

        AddHandler lbl_ExcessShort.KeyPress, AddressOf TextBoxControlKeyPress

        AddHandler txt_InvoiceCode.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_PcsLength.KeyPress, AddressOf TextBoxControlKeyPress
        'AddHandler txt_PickUpPerc_Party.KeyPress, AddressOf TextBoxControlKeyPress

        'AddHandler txt_Remarks.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_RewindingCones.KeyPress, AddressOf TextBoxControlKeyPress
        'AddHandler txt_RwExcSht.KeyPress, AddressOf TextBoxControlKeyPress


        AddHandler txt_TapeLength.KeyPress, AddressOf TextBoxControlKeyPress
        ' AddHandler txt_WarpMeters.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler dtp_Filter_Fromdate.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler dtp_Filter_ToDate.KeyPress, AddressOf TextBoxControlKeyPress

        ' newly added control for KKP 1288
        AddHandler chk_NoNeedPosting.GotFocus, AddressOf ControlGotFocus
        AddHandler chk_NoNeedPosting.LostFocus, AddressOf ControlLostFocus

        AddHandler cbo_DeliveryTo.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_DeliveryTo.LostFocus, AddressOf ControlLostFocus

        AddHandler cbo_Location.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Location.LostFocus, AddressOf ControlLostFocus

        AddHandler cbo_Grid_Location.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Grid_Location.LostFocus, AddressOf ControlLostFocus


        lbl_Company.Text = ""
        lbl_Company.Tag = 0
        lbl_Company.Visible = False
        Common_Procedures.CompIdNo = 0

        Filter_Status = False
        FrmLdSTS = True

        ' Form Design is varied based on customer Setting 

        If Common_Procedures.settings.CustomerCode = "1288" Then
            '' KKP - Namakkal 
            dgv_YarnTakenDetails.Columns(11).Visible = True
            dgv_YarnTakenDetails.Columns(12).Visible = True

            dgv_BabyConeDetails.Width = 700
            dgv_BabyConeDetails_Total.Width = 700

            dgv_BabyConeDetails.Columns(4).Visible = True
            dgv_BabyConeDetails.Columns(5).Visible = True
            dgv_BabyConeDetails.Columns(6).Visible = True

            chk_RewindingStatus.Visible = False
            lbl_rwCones.Visible = False
            txt_RewindingCones.Visible = False
            lbl_rwMillname.Visible = False
            cbo_Rw_MillName.Visible = False
            lbl_RwExcSht_Caption.Visible = False
            txt_RwExcSht.Visible = False

            lbl_Tarewt.Visible = False
            txt_BabyCone_TareWeight.Visible = False
            lbl_AddLess.Visible = False
            txt_BabyCone_AddLessWgt.Visible = False
            lbl_netWt.Visible = False
            lbl_BabyCone_NetWeight.Visible = False


            chk_NoNeedPosting.Visible = True
            lbl_DeliveryTo.Visible = True
            cbo_DeliveryTo.Visible = True
            lbl_Location.Visible = True
            cbo_Location.Visible = True

            chk_NoNeedPosting.Left = lbl_emptycone.Left
            chk_NoNeedPosting.Top = lbl_emptycone.Top

            lbl_DeliveryTo.Left = lbl_netWt.Left - 30
            lbl_DeliveryTo.Top = lbl_netWt.Top

            cbo_DeliveryTo.Left = lbl_BabyCone_NetWeight.Left - 30
            cbo_DeliveryTo.Top = lbl_BabyCone_NetWeight.Top

            lbl_Location.Left = lbl_AddLess.Left
            lbl_Location.Top = lbl_AddLess.Top

            cbo_Location.Left = txt_BabyCone_AddLessWgt.Left
            cbo_Location.Top = txt_BabyCone_AddLessWgt.Top

            dgv_BabyConeDetails.Top = dgv_BabyConeDetails.Top + 50

            lbl_emptybag.Top = dgv_BabyConeDetails.Top - 50
            txt_WgtEmYBag.Top = dgv_BabyConeDetails.Top - 50

            lbl_emptycone.Top = dgv_BabyConeDetails.Top - 50
            lbl_emptycone.Left = txt_WgtEmYBag.Left + txt_WgtEmYBag.Width + 20
            txt_WgtEmYCone.Top = dgv_BabyConeDetails.Top - 50
            txt_WgtEmYCone.Left = lbl_emptycone.Left + lbl_emptycone.Width + 20

        Else
            '' Other design
            dgv_YarnTakenDetails.Columns(11).Visible = False
            dgv_YarnTakenDetails.Columns(12).Visible = False

            dgv_BabyConeDetails.Width = 423
            dgv_BabyConeDetails_Total.Width = 423

            dgv_BabyConeDetails.Columns(4).Visible = False
            dgv_BabyConeDetails.Columns(5).Visible = False
            dgv_BabyConeDetails.Columns(6).Visible = False

            chk_RewindingStatus.Visible = True
            lbl_rwCones.Visible = True
            txt_RewindingCones.Visible = True
            lbl_rwMillname.Visible = True
            cbo_Rw_MillName.Visible = True
            lbl_RwExcSht_Caption.Visible = True
            txt_RwExcSht.Visible = True

            lbl_Tarewt.Visible = True
            txt_BabyCone_TareWeight.Visible = True
            lbl_AddLess.Visible = True
            txt_BabyCone_AddLessWgt.Visible = True
            lbl_netWt.Visible = True
            lbl_BabyCone_NetWeight.Visible = True

            chk_NoNeedPosting.Visible = False
            lbl_DeliveryTo.Visible = False
            cbo_DeliveryTo.Visible = False
            lbl_Location.Visible = False
            cbo_Location.Visible = False

        End If

        new_record()

    End Sub

    Private Sub Statement_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        On Error Resume Next
        Con.Close()
        Con.Dispose()
        Common_Procedures.Last_Closed_FormName = Me.Name
    End Sub

    Private Sub Statement_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress

        Try
            If Asc(e.KeyChar) = 27 Then

                If pnl_Filter.Visible = True Then
                    btn_Filter_Close_Click(sender, e)
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
            MessageBox.Show(ex.Message, "DOES NOT CLOSE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Protected Overrides Function ProcessCmdKey(ByRef msg As System.Windows.Forms.Message, ByVal keyData As System.Windows.Forms.Keys) As Boolean
        Dim dgv1 As New DataGridView
        Dim i As Integer
        Dim Pr_kyData As Keys

        If ActiveControl.Name = dgv_WarpingDetails_Set1.Name Or ActiveControl.Name = dgv_WarpingDetails_Set2.Name Or ActiveControl.Name = dgv_WarpingDetails_Set3.Name Or ActiveControl.Name = dgv_YarnTakenDetails.Name Or ActiveControl.Name = dgv_BabyConeDetails.Name Or TypeOf ActiveControl Is DataGridViewTextBoxEditingControl Then

            On Error Resume Next

            dgv1 = Nothing

            If ActiveControl.Name = dgv_WarpingDetails_Set1.Name Then
                dgv1 = dgv_WarpingDetails_Set1

            ElseIf ActiveControl.Name = dgv_WarpingDetails_Set2.Name Then
                dgv1 = dgv_WarpingDetails_Set2

            ElseIf ActiveControl.Name = dgv_WarpingDetails_Set3.Name Then
                dgv1 = dgv_WarpingDetails_Set3


            ElseIf ActiveControl.Name = dgv_YarnTakenDetails.Name Then
                dgv1 = dgv_YarnTakenDetails

            ElseIf ActiveControl.Name = dgv_BabyConeDetails.Name Then
                dgv1 = dgv_BabyConeDetails



            ElseIf dgv_WarpingDetails_Set1.IsCurrentRowDirty = True Then
                dgv1 = dgv_WarpingDetails_Set1

            ElseIf dgv_WarpingDetails_Set2.IsCurrentRowDirty = True Then
                dgv1 = dgv_WarpingDetails_Set2

            ElseIf dgv_WarpingDetails_Set3.IsCurrentRowDirty = True Then
                dgv1 = dgv_WarpingDetails_Set3



            ElseIf dgv_YarnTakenDetails.IsCurrentRowDirty = True Then
                dgv1 = dgv_YarnTakenDetails

            ElseIf dgv_BabyConeDetails.IsCurrentRowDirty = True Then
                dgv1 = dgv_BabyConeDetails



            ElseIf tab_Main.SelectedIndex = 0 Then
                If tab_WarpingDeatils.SelectedIndex = 0 Then
                    dgv1 = dgv_WarpingDetails_Set1

                ElseIf tab_WarpingDeatils.SelectedIndex = 1 Then
                    dgv1 = dgv_WarpingDetails_Set2

                ElseIf tab_WarpingDeatils.SelectedIndex = 2 Then
                    dgv1 = dgv_WarpingDetails_Set3

                End If



            ElseIf tab_Main.SelectedIndex = 1 Then
                dgv1 = dgv_YarnTakenDetails

            ElseIf tab_Main.SelectedIndex = 2 Then
                dgv1 = dgv_BabyConeDetails


            Else
                Return MyBase.ProcessCmdKey(msg, keyData)
                Exit Function

            End If


            Pr_kyData = Prev_kyData
            Prev_kyData = keyData

            With dgv1

                '-------------------------- WARPING DETAILS (SET1)

                If dgv1.Name = dgv_WarpingDetails_Set1.Name Or dgv1.Name = dgv_WarpingDetails_Set2.Name Or dgv1.Name = dgv_WarpingDetails_Set3.Name Then


                    If (keyData = Keys.Enter Or keyData = Keys.Down Or keyData = 131085) Then

                        If .CurrentCell.ColumnIndex >= .ColumnCount - 1 Or (.CurrentCell.ColumnIndex >= 3 And .Columns(5).Visible = False) Or Pr_kyData = 131089 Then

                            If .CurrentCell.RowIndex = .RowCount - 1 Then

                                If .RowCount = 1 Then
                                    tab_Main.SelectTab(1)
                                    ' tab_SizingDetails.SelectTab(0)
                                    'dgv_SizingDetails_Set1.Focus()
                                    'dgv_SizingDetails_Set1.CurrentCell = dgv_SizingDetails_Set1.Rows(0).Cells(1)
                                    'dgv_SizingDetails_Set1.CurrentCell.Selected = True

                                Else
                                    If dgv1.Name = dgv_WarpingDetails_Set1.Name Then
                                        tab_WarpingDeatils.SelectTab(1)
                                        'If dgv_WarpingDetails_Set2.Rows.Count <= 0 Then dgv_WarpingDetails_Set2.Rows.Add()
                                        'dgv_WarpingDetails_Set2.Focus()
                                        'dgv_WarpingDetails_Set2.CurrentCell = dgv_WarpingDetails_Set2.Rows(0).Cells(1)
                                        'dgv_WarpingDetails_Set2.CurrentCell.Selected = True

                                    ElseIf dgv1.Name = dgv_WarpingDetails_Set2.Name Then
                                        tab_WarpingDeatils.SelectTab(2)
                                        'If dgv_WarpingDetails_Set3.Rows.Count <= 0 Then dgv_WarpingDetails_Set3.Rows.Add()
                                        'dgv_WarpingDetails_Set3.Focus()
                                        'dgv_WarpingDetails_Set3.CurrentCell = dgv_WarpingDetails_Set3.Rows(0).Cells(1)
                                        'dgv_WarpingDetails_Set3.CurrentCell.Selected = True

                                    Else
                                        tab_Main.SelectTab(1)
                                        ' tab_SizingDetails.SelectTab(0)
                                        'dgv_SizingDetails_Set1.Focus()
                                        'dgv_SizingDetails_Set1.CurrentCell = dgv_SizingDetails_Set1.Rows(0).Cells(1)
                                        'dgv_SizingDetails_Set1.CurrentCell.Selected = True

                                    End If

                                End If

                            Else
                                .CurrentCell = .Rows(.CurrentCell.RowIndex + 1).Cells(1)

                            End If

                        Else

                            If .CurrentCell.RowIndex = .RowCount - 1 And .CurrentCell.ColumnIndex >= 1 And ((.CurrentCell.ColumnIndex <> 1 And Val(.CurrentRow.Cells(1).Value) = 0) Or (.CurrentCell.ColumnIndex = 1 And Val(dgtxt_WarpingDetails_Set1.Text) = 0 And Val(dgtxt_WarpingDetails_Set2.Text) = 0 And Val(dgtxt_WarpingDetails_Set3.Text) = 0)) Then

                                If .RowCount = 1 Then
                                    tab_Main.SelectTab(1)
                                    ' tab_SizingDetails.SelectTab(0)
                                    'dgv_SizingDetails_Set1.Focus()
                                    'dgv_SizingDetails_Set1.CurrentCell = dgv_SizingDetails_Set1.Rows(0).Cells(1)
                                    'dgv_SizingDetails_Set1.CurrentCell.Selected = True

                                Else
                                    If dgv1.Name = dgv_WarpingDetails_Set1.Name Then
                                        tab_WarpingDeatils.SelectTab(1)
                                        'If dgv_WarpingDetails_Set2.Rows.Count <= 0 Then dgv_WarpingDetails_Set2.Rows.Add()
                                        'dgv_WarpingDetails_Set2.Focus()
                                        'dgv_WarpingDetails_Set2.CurrentCell = dgv_WarpingDetails_Set2.Rows(0).Cells(1)
                                        'dgv_WarpingDetails_Set2.CurrentCell.Selected = True

                                    ElseIf dgv1.Name = dgv_WarpingDetails_Set2.Name Then
                                        tab_WarpingDeatils.SelectTab(2)
                                        'If dgv_WarpingDetails_Set3.Rows.Count <= 0 Then dgv_WarpingDetails_Set3.Rows.Add()
                                        'dgv_WarpingDetails_Set3.Focus()
                                        'dgv_WarpingDetails_Set3.CurrentCell = dgv_WarpingDetails_Set3.Rows(0).Cells(1)
                                        'dgv_WarpingDetails_Set3.CurrentCell.Selected = True

                                    Else
                                        tab_Main.SelectTab(1)
                                        ' tab_SizingDetails.SelectTab(0)
                                        'dgv_SizingDetails_Set1.Focus()
                                        'dgv_SizingDetails_Set1.CurrentCell = dgv_SizingDetails_Set1.Rows(0).Cells(1)
                                        'dgv_SizingDetails_Set1.CurrentCell.Selected = True

                                    End If

                                End If

                            Else
                                .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 1)

                            End If


                        End If

                        Return True

                    ElseIf keyData = Keys.Up Then

                        If .CurrentCell.ColumnIndex <= 1 Then
                            If .CurrentCell.RowIndex = 0 Then

                                If dgv1.Name = dgv_WarpingDetails_Set2.Name Then
                                    tab_WarpingDeatils.SelectTab(0)
                                    'If dgv_WarpingDetails_Set1.Rows.Count <= 0 Then dgv_WarpingDetails_Set1.Rows.Add()
                                    'dgv_WarpingDetails_Set1.CurrentCell = dgv_WarpingDetails_Set1.Rows(0).Cells(1)
                                    'dgv_WarpingDetails_Set1.Focus()
                                    'dgv_WarpingDetails_Set1.CurrentCell.Selected = True

                                ElseIf dgv1.Name = dgv_WarpingDetails_Set3.Name Then
                                    tab_WarpingDeatils.SelectTab(1)
                                    'If dgv_WarpingDetails_Set2.Rows.Count <= 0 Then dgv_WarpingDetails_Set2.Rows.Add()
                                    'dgv_WarpingDetails_Set2.Focus()
                                    'dgv_WarpingDetails_Set2.CurrentCell = dgv_WarpingDetails_Set2.Rows(0).Cells(1)
                                    'dgv_WarpingDetails_Set2.CurrentCell.Selected = True

                                Else
                                    If txt_Ends.Enabled And txt_Ends.Visible Then txt_Ends.Focus() Else txt_PcsLength.Focus()

                                End If

                            Else
                                .CurrentCell = .Rows(.CurrentCell.RowIndex - 1).Cells(3) ' .Rows(.CurrentCell.RowIndex - 1).Cells(22)

                            End If

                        Else
                            .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex - 1)

                        End If

                        Return True

                    Else
                        Return MyBase.ProcessCmdKey(msg, keyData)

                    End If


                    '-------------------------- SIZING DETAILS (SET1)



                    '----------- YARN TAKEN DETAILS

                ElseIf dgv1.Name = dgv_YarnTakenDetails.Name Then

                    If keyData = Keys.Enter Or keyData = Keys.Down Then

                        If .CurrentCell.ColumnIndex >= .ColumnCount - 1 Then

                            If .CurrentCell.RowIndex = .RowCount - 1 Then
                                tab_Main.SelectTab(3)
                                dgv_BabyConeDetails.Focus()
                                dgv_BabyConeDetails.CurrentCell = dgv_BabyConeDetails.Rows(0).Cells(1)
                                dgv_BabyConeDetails.CurrentCell.Selected = True
                                'dgv_YarnTakenDetails.Focus()
                                'dgv_YarnTakenDetails.CurrentCell = dgv_YarnTakenDetails.Rows(0).Cells(1)
                                'dgv_YarnTakenDetails.CurrentCell.Selected = True
                            Else
                                .CurrentCell = .Rows(.CurrentCell.RowIndex + 1).Cells(1)

                            End If
                        ElseIf .CurrentCell.ColumnIndex = 3 Then
                            dgv_YarnTakenDetails.Focus()
                            .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(4)
                        ElseIf .CurrentCell.ColumnIndex = 10 Then
                            dgv_YarnTakenDetails.Focus()
                            dgv_YarnTakenDetails.CurrentCell = dgv_YarnTakenDetails.Rows(.CurrentCell.RowIndex + 1).Cells(1)
                            dgv_YarnTakenDetails.CurrentCell.Selected = True
                        Else

                            If .CurrentCell.RowIndex = .RowCount - 1 And .CurrentCell.ColumnIndex >= 1 And Trim(.CurrentRow.Cells(1).Value) = "" Then
                                tab_Main.SelectTab(3)
                                'dgv_BabyConeDetails.Focus()
                                'dgv_BabyConeDetails.CurrentCell = dgv_BabyConeDetails.Rows(0).Cells(1)
                                'dgv_BabyConeDetails.CurrentCell.Selected = True

                            Else
                                .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 1)
                                'dgv_YarnTakenDetails.Focus()
                                'dgv_YarnTakenDetails.CurrentCell = dgv_YarnTakenDetails.Rows(.CurrentCell.RowIndex + 1).Cells(1)
                                'dgv_YarnTakenDetails.CurrentCell.Selected = True
                            End If

                        End If

                        Return True

                    ElseIf keyData = Keys.Up Then
                        If .CurrentCell.ColumnIndex <= 1 Then
                            If .CurrentCell.RowIndex = 0 Then
                                tab_Main.SelectTab(1)

                                'dgv_SizingDetails_Set1.Focus()
                                'dgv_SizingDetails_Set1.CurrentCell = dgv_SizingDetails_Set1.Rows(0).Cells(1)
                                'dgv_SizingDetails_Set1.CurrentCell.Selected = True

                            Else
                                .CurrentCell = .Rows(.CurrentCell.RowIndex - 1).Cells(.Columns.Count - 1)

                            End If

                        Else
                            .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex - 1)

                        End If

                        Return True

                    Else
                        Return MyBase.ProcessCmdKey(msg, keyData)

                    End If


                    '----------- BABY CONE DETAILS

                ElseIf dgv1.Name = dgv_BabyConeDetails.Name Then

                    If keyData = Keys.Enter Or keyData = Keys.Down Then

                        If .CurrentCell.ColumnIndex >= .ColumnCount - 1 Then

                            If .CurrentCell.RowIndex = .RowCount - 1 Then
                                txt_WgtEmYBag.Focus()

                                'tab_Main.SelectTab(4)
                                'dgv_ChemicalDetails.Focus()
                                'dgv_ChemicalDetails.CurrentCell = dgv_ChemicalDetails.Rows(0).Cells(1)
                                'dgv_ChemicalDetails.CurrentCell.Selected = True
                                'If cbo_Grid_ItemName.Visible And cbo_Grid_ItemName.Enabled Then
                                '    cbo_Grid_ItemName.Focus()
                                'End If

                            Else
                                .CurrentCell = .Rows(.CurrentCell.RowIndex + 1).Cells(1)

                            End If

                        Else

                            If .CurrentCell.RowIndex = .RowCount - 1 And .CurrentCell.ColumnIndex >= 1 And Trim(.CurrentRow.Cells(1).Value) = "" Then
                                txt_WgtEmYBag.Focus()
                                'tab_Main.SelectTab(4)
                                'dgv_ChemicalDetails.Focus()
                                'dgv_ChemicalDetails.CurrentCell = dgv_ChemicalDetails.Rows(0).Cells(1)
                                'dgv_ChemicalDetails.CurrentCell.Selected = True
                                'If cbo_Grid_ItemName.Visible And cbo_Grid_ItemName.Enabled Then
                                '    cbo_Grid_ItemName.Focus()
                                'End If

                            Else
                                .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 1)

                            End If

                        End If

                        Return True

                    ElseIf keyData = Keys.Up Then
                        If .CurrentCell.ColumnIndex <= 1 Then
                            If .CurrentCell.RowIndex = 0 Then
                                tab_Main.SelectTab(2)
                                'dgv_YarnTakenDetails.Focus()
                                'dgv_YarnTakenDetails.CurrentCell = dgv_YarnTakenDetails.Rows(0).Cells(1)
                                'dgv_YarnTakenDetails.CurrentCell.Selected = True
                                'If cbo_Grid_CountName.Visible And cbo_Grid_CountName.Enabled Then
                                '    cbo_Grid_CountName.Focus()
                                'End If

                            Else
                                .CurrentCell = .Rows(.CurrentCell.RowIndex - 1).Cells(.Columns.Count - 1)

                            End If

                        Else
                            .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex - 1)

                        End If

                        Return True

                    Else
                        Return MyBase.ProcessCmdKey(msg, keyData)

                    End If


                    '----------- CHEMICAL DETAILS


                Else
                    Return MyBase.ProcessCmdKey(msg, keyData)

                End If

            End With

        Else

            Return MyBase.ProcessCmdKey(msg, keyData)

        End If

    End Function

    Private Sub cbo_Ledger_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Ledger.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, Con, "Ledger_AlaisHead", "Ledger_DisplayName", "(AccountsGroup_IdNo = 10 and Close_Status = 0 )", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_Ledger_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Ledger.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, Con, cbo_Ledger, txt_Remarks, dtp_Date, "Ledger_AlaisHead", "Ledger_DisplayName", "(AccountsGroup_IdNo = 10 and Close_Status = 0 )", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_Ledger_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Ledger.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, Con, cbo_Ledger, dtp_Date, "Ledger_AlaisHead", "Ledger_DisplayName", "(AccountsGroup_IdNo = 10 and Close_Status = 0 )", "(Ledger_IdNo = 0)")
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

    Private Sub cbo_CountName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_CountName.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, Con, "Count_Head", "Count_Name", "", "(Count_IdNo = 0)")
    End Sub

    Private Sub cbo_CountName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_CountName.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, Con, cbo_CountName, cbo_MillName, cbo_Meters_Yards, "Count_Head", "Count_Name", "", "(Count_IdNo = 0)")
    End Sub

    Private Sub cbo_CountName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_CountName.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, Con, cbo_CountName, cbo_Meters_Yards, "Count_Head", "Count_Name", "", "(Count_IdNo = 0)")
    End Sub

    Private Sub cbo_CountName_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_CountName.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Dim f As New Count_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_CountName.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If
    End Sub

    Private Sub cbo_MillName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_MillName.GotFocus
        cbo_MillName.Tag = cbo_MillName.Text
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, Con, "Mill_Head", "Mill_Name", "", "(Mill_IdNo = 0)")
    End Sub

    Private Sub cbo_MillName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_MillName.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, Con, cbo_MillName, dtp_Date, cbo_CountName, "Mill_Head", "Mill_Name", "", "(Mill_IdNo = 0)")
    End Sub

    Private Sub cbo_MillName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_MillName.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, Con, cbo_MillName, cbo_CountName, "Mill_Head", "Mill_Name", "", "(Mill_IdNo = 0)")
    End Sub

    Private Sub cbo_MillName_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_MillName.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Dim f As New Mill_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_MillName.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If
    End Sub

    Private Sub cbo_BeamWidth_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_BeamWidth.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, Con, "Beam_Width_Head", "Beam_Width_Name", "", "(Beam_Width_IdNo = 0)")
    End Sub

    Private Sub cbo_Beamwidth_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_BeamWidth.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, Con, cbo_BeamWidth, cbo_CountName, txt_WarpMeters, "Beam_Width_Head", "Beam_Width_Name", "", "(Beam_Width_IdNo = 0)")
    End Sub

    Private Sub cbo_BeamWidth_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_BeamWidth.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, Con, cbo_BeamWidth, txt_WarpMeters, "Beam_Width_Head", "Beam_Width_Name", "", "(Beam_Width_IdNo = 0)")
    End Sub

    Private Sub cbo_BeamWidth_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_BeamWidth.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Dim f As New Beam_Width_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_BeamWidth.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If
    End Sub

    Private Sub dgv_WarpingDetails_Set1_CellEndEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_WarpingDetails_Set1.CellEndEdit
        dgv_WarpingDetails_Set1_CellLeave(sender, e)
    End Sub

    Private Sub dgv_WarpingDetails_Set1_CellEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_WarpingDetails_Set1.CellEnter
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim Dt2 As New DataTable
        Dim rect As Rectangle
        Dim Wmtr() As String
        Dim Mtrs As Single = 0

        With dgv_WarpingDetails_Set1

            If Val(.CurrentRow.Cells(0).Value) = 0 Then
                .CurrentRow.Cells(0).Value = .CurrentRow.Index + 1
            End If

            If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1028" Then '---- Chinnu Sizing (Palladam)
                If e.RowIndex > 0 And e.ColumnIndex = 1 Then
                    If Val(.CurrentRow.Cells(1).Value) = 0 And e.RowIndex = .RowCount - 1 Then
                        .CurrentRow.Cells(1).Value = .Rows(e.RowIndex - 1).Cells(1).Value
                    End If
                End If
            End If

            If e.ColumnIndex = 5 Then

                If e.RowIndex > 0 Then
                    If Trim(.CurrentRow.Cells(e.ColumnIndex).Value) = "" Then
                        .CurrentRow.Cells(e.ColumnIndex).Value = .Rows(e.RowIndex - 1).Cells(e.ColumnIndex).Value
                    End If
                End If

                If cbo_Ends1_Shift.Visible = False Or Val(cbo_Ends1_Shift.Tag) <> e.RowIndex Then

                    rect = .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False)

                    cbo_Ends1_Shift.Left = .Left + rect.Left
                    cbo_Ends1_Shift.Top = .Top + rect.Top

                    cbo_Ends1_Shift.Width = rect.Width
                    cbo_Ends1_Shift.Height = rect.Height
                    cbo_Ends1_Shift.Text = .CurrentCell.Value

                    cbo_Ends1_Shift.Tag = Val(e.RowIndex)
                    cbo_Ends1_Shift.Visible = True

                    cbo_Ends1_Shift.BringToFront()
                    cbo_Ends1_Shift.Focus()

                End If

            Else
                cbo_Ends1_Shift.Visible = False
                cbo_Ends1_Shift.Tag = -1
                cbo_Ends1_Shift.Text = ""
            End If

            If e.ColumnIndex = 10 Then

                If cbo_Grid_FrontWarper_1.Visible = False Or Val(cbo_Grid_FrontWarper_1.Tag) <> e.RowIndex Then

                    cbo_Grid_FrontWarper_1.Tag = -1
                    Da = New SqlClient.SqlDataAdapter("select Employee_Name from PayRoll_Employee_Head order by Employee_Name", Con)
                    Dt1 = New DataTable
                    Da.Fill(Dt1)
                    cbo_Grid_FrontWarper_1.DataSource = Dt1
                    cbo_Grid_FrontWarper_1.DisplayMember = "Employee_Name"

                    rect = .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False)

                    cbo_Grid_FrontWarper_1.Left = .Left + rect.Left
                    cbo_Grid_FrontWarper_1.Top = .Top + rect.Top

                    cbo_Grid_FrontWarper_1.Width = rect.Width
                    cbo_Grid_FrontWarper_1.Height = rect.Height
                    cbo_Grid_FrontWarper_1.Text = .CurrentCell.Value

                    cbo_Grid_FrontWarper_1.Tag = Val(e.RowIndex)
                    cbo_Grid_FrontWarper_1.Visible = True

                    cbo_Grid_FrontWarper_1.BringToFront()
                    cbo_Grid_FrontWarper_1.Focus()


                End If

            Else
                cbo_Grid_FrontWarper_1.Visible = False
                cbo_Grid_FrontWarper_1.Tag = -1
                cbo_Grid_FrontWarper_1.Text = ""

            End If

            If e.ColumnIndex = 12 Then

                If cbo_Grid_BackWarper1.Visible = False Or Val(cbo_Grid_BackWarper1.Tag) <> e.RowIndex Then

                    cbo_Grid_BackWarper1.Tag = -1
                    Da = New SqlClient.SqlDataAdapter("select Employee_Name from PayRoll_Employee_Head order by Employee_Name", Con)
                    Dt1 = New DataTable
                    Da.Fill(Dt1)
                    cbo_Grid_BackWarper1.DataSource = Dt1
                    cbo_Grid_BackWarper1.DisplayMember = "Employee_name"

                    rect = .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False)

                    cbo_Grid_BackWarper1.Left = .Left + rect.Left
                    cbo_Grid_BackWarper1.Top = .Top + rect.Top

                    cbo_Grid_BackWarper1.Width = rect.Width
                    cbo_Grid_BackWarper1.Height = rect.Height
                    cbo_Grid_BackWarper1.Text = .CurrentCell.Value

                    cbo_Grid_BackWarper1.Tag = Val(e.RowIndex)
                    cbo_Grid_BackWarper1.Visible = True

                    cbo_Grid_BackWarper1.BringToFront()
                    cbo_Grid_BackWarper1.Focus()



                End If

            Else
                cbo_Grid_BackWarper1.Visible = False
                cbo_Grid_BackWarper1.Tag = -1
                cbo_Grid_BackWarper1.Text = ""

            End If


            If .CurrentCell.ColumnIndex = 14 Then

                If cbo_Grid_Helper1.Visible = False Or Val(cbo_Grid_Helper1.Tag) <> e.RowIndex Then

                    cbo_Grid_Helper1.Tag = -1
                    Da = New SqlClient.SqlDataAdapter("select Employee_Name from PayRoll_Employee_Head order by Employee_Name", Con)
                    Dt1 = New DataTable
                    Da.Fill(Dt1)
                    cbo_Grid_Helper1.DataSource = Dt1
                    cbo_Grid_Helper1.DisplayMember = "Employee_Name"

                    rect = .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False)

                    cbo_Grid_Helper1.Left = .Left + rect.Left
                    cbo_Grid_Helper1.Top = .Top + rect.Top

                    cbo_Grid_Helper1.Width = rect.Width
                    cbo_Grid_Helper1.Height = rect.Height
                    cbo_Grid_Helper1.Text = .CurrentCell.Value

                    cbo_Grid_Helper1.Tag = Val(e.RowIndex)
                    cbo_Grid_Helper1.Visible = True

                    cbo_Grid_Helper1.BringToFront()
                    cbo_Grid_Helper1.Focus()

                End If

            Else
                cbo_Grid_Helper1.Visible = False
                cbo_Grid_Helper1.Tag = -1
                cbo_Grid_Helper1.Text = ""

            End If

            If e.ColumnIndex = 16 Then

                If cbo_Grid_FrontWarper2.Visible = False Or Val(cbo_Grid_FrontWarper2.Tag) <> e.RowIndex Then

                    cbo_Grid_FrontWarper2.Tag = -1
                    Da = New SqlClient.SqlDataAdapter("select Employee_Name from PayRoll_Employee_Head order by Employee_Name", Con)
                    Dt1 = New DataTable
                    Da.Fill(Dt1)
                    cbo_Grid_FrontWarper2.DataSource = Dt1
                    cbo_Grid_FrontWarper2.DisplayMember = "Employee_Name"

                    rect = .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False)

                    cbo_Grid_FrontWarper2.Left = .Left + rect.Left
                    cbo_Grid_FrontWarper2.Top = .Top + rect.Top

                    cbo_Grid_FrontWarper2.Width = rect.Width
                    cbo_Grid_FrontWarper2.Height = rect.Height
                    cbo_Grid_FrontWarper2.Text = .CurrentCell.Value

                    cbo_Grid_FrontWarper2.Tag = Val(e.RowIndex)
                    cbo_Grid_FrontWarper2.Visible = True

                    cbo_Grid_FrontWarper2.BringToFront()
                    cbo_Grid_FrontWarper2.Focus()


                End If

            Else
                cbo_Grid_FrontWarper2.Visible = False
                cbo_Grid_FrontWarper2.Tag = -1
                cbo_Grid_FrontWarper2.Text = ""

            End If

            If e.ColumnIndex = 18 Then

                If cbo_Grid_BackWarper2.Visible = False Or Val(cbo_Grid_BackWarper2.Tag) <> e.RowIndex Then

                    cbo_Grid_BackWarper2.Tag = -1
                    Da = New SqlClient.SqlDataAdapter("select Employee_Name from PayRoll_Employee_Head order by Employee_Name", Con)
                    Dt1 = New DataTable
                    Da.Fill(Dt1)
                    cbo_Grid_BackWarper2.DataSource = Dt1
                    cbo_Grid_BackWarper2.DisplayMember = "Employee_name"

                    rect = .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False)

                    cbo_Grid_BackWarper2.Left = .Left + rect.Left
                    cbo_Grid_BackWarper2.Top = .Top + rect.Top

                    cbo_Grid_BackWarper2.Width = rect.Width
                    cbo_Grid_BackWarper2.Height = rect.Height
                    cbo_Grid_BackWarper2.Text = .CurrentCell.Value

                    cbo_Grid_BackWarper2.Tag = Val(e.RowIndex)
                    cbo_Grid_BackWarper2.Visible = True

                    cbo_Grid_BackWarper2.BringToFront()
                    cbo_Grid_BackWarper2.Focus()



                End If

            Else
                cbo_Grid_BackWarper2.Visible = False
                cbo_Grid_BackWarper2.Tag = -1
                cbo_Grid_BackWarper2.Text = ""

            End If

            If .CurrentCell.ColumnIndex = 20 Then

                If cbo_Grid_Helper2.Visible = False Or Val(cbo_Grid_Helper2.Tag) <> e.RowIndex Then

                    cbo_Grid_Helper2.Tag = -1
                    Da = New SqlClient.SqlDataAdapter("select Employee_Name from PayRoll_Employee_Head order by Employee_Name", Con)
                    Dt1 = New DataTable
                    Da.Fill(Dt1)
                    cbo_Grid_Helper2.DataSource = Dt1
                    cbo_Grid_Helper2.DisplayMember = "Employee_Name"

                    rect = .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False)

                    cbo_Grid_Helper2.Left = .Left + rect.Left
                    cbo_Grid_Helper2.Top = .Top + rect.Top

                    cbo_Grid_Helper2.Width = rect.Width
                    cbo_Grid_Helper2.Height = rect.Height
                    cbo_Grid_Helper2.Text = .CurrentCell.Value

                    cbo_Grid_Helper2.Tag = Val(e.RowIndex)
                    cbo_Grid_Helper2.Visible = True

                    cbo_Grid_Helper2.BringToFront()
                    cbo_Grid_Helper2.Focus()

                End If

            Else
                cbo_Grid_Helper2.Visible = False
                cbo_Grid_Helper2.Tag = -1
                cbo_Grid_Helper2.Text = ""

            End If

            If .CurrentCell.ColumnIndex = 11 Or .CurrentCell.ColumnIndex = 13 Then

                If Trim(.CurrentRow.Cells(.CurrentCell.ColumnIndex - 1).Value) <> "" And Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value) = 0 Then
                    Erase Wmtr
                    If Trim(txt_WarpMeters.Text) <> "" Then
                        Wmtr = Split(Trim(txt_WarpMeters.Text), ",")
                        If UBound(Wmtr) >= 0 Then
                            .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = Val(Wmtr(0))
                        End If
                    End If
                End If

            End If

            If .CurrentCell.ColumnIndex = 15 Then

                If Trim(.CurrentRow.Cells(.CurrentCell.ColumnIndex - 1).Value) <> "" And Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value) = 0 Then

                    Erase Wmtr

                    If Trim(txt_WarpMeters.Text) <> "" Then
                        Wmtr = Split(Trim(txt_WarpMeters.Text), ",")

                        If UBound(Wmtr) >= 0 Then
                            Mtrs = Format(Val(Wmtr(0)) * 2 / 3, "#########0.00")

                            .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = Val(Wmtr(0))

                            If Val(.CurrentRow.Cells(11).Value) = Val(Wmtr(0)) And Val(.CurrentRow.Cells(13).Value) = Val(Wmtr(0)) Then
                                .CurrentRow.Cells(11).Value = Mtrs
                                .CurrentRow.Cells(13).Value = Mtrs
                                .CurrentRow.Cells(15).Value = Mtrs
                            End If

                        End If

                    End If

                End If

            End If

            If .CurrentCell.ColumnIndex = 17 Then

                If Trim(.CurrentRow.Cells(.CurrentCell.ColumnIndex - 1).Value) <> "" And Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value) = 0 Then
                    Erase Wmtr
                    If Trim(txt_WarpMeters.Text) <> "" Then
                        Wmtr = Split(Trim(txt_WarpMeters.Text), ",")
                        If UBound(Wmtr) >= 0 Then

                            If Val(.CurrentRow.Cells(15).Value) = 0 Then
                                If Val(.CurrentRow.Cells(11).Value) = Val(Wmtr(0)) Then
                                    .CurrentRow.Cells(11).Value = Val(Format(Val(Wmtr(0)) / 2, "#########0.00"))
                                    .CurrentRow.Cells(17).Value = Val(Format(Val(Wmtr(0)) / 2, "#########0.00"))
                                End If

                            Else
                                Mtrs = Format(Val(Wmtr(0)) * 2 / 3, "#########0.00")
                                If Val(.CurrentRow.Cells(11).Value) = Val(Mtrs) And Val(.CurrentRow.Cells(15).Value) = Val(Mtrs) Then
                                    .CurrentRow.Cells(11).Value = Val(Format(Mtrs / 2, "#########0.00"))
                                    .CurrentRow.Cells(17).Value = Val(Format(Mtrs / 2, "#########0.00"))
                                End If

                            End If

                        End If

                    End If

                End If

            End If

            If .CurrentCell.ColumnIndex = 19 Then

                If Trim(.CurrentRow.Cells(.CurrentCell.ColumnIndex - 1).Value) <> "" And Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value) = 0 Then
                    Erase Wmtr
                    If Trim(txt_WarpMeters.Text) <> "" Then
                        Wmtr = Split(Trim(txt_WarpMeters.Text), ",")
                        If UBound(Wmtr) >= 0 Then

                            If Val(.CurrentRow.Cells(15).Value) = 0 Then
                                If Val(.CurrentRow.Cells(13).Value) = Val(Wmtr(0)) Then
                                    .CurrentRow.Cells(13).Value = Val(Format(Val(Wmtr(0)) / 2, "#########0.00"))
                                    .CurrentRow.Cells(19).Value = Val(Format(Val(Wmtr(0)) / 2, "#########0.00"))
                                End If

                            Else
                                Mtrs = Format(Val(Wmtr(0)) * 2 / 3, "#########0.00")
                                If Val(.CurrentRow.Cells(13).Value) = Val(Mtrs) And Val(.CurrentRow.Cells(15).Value) = Val(Mtrs) Then
                                    .CurrentRow.Cells(13).Value = Val(Format(Mtrs / 2, "#########0.00"))
                                    .CurrentRow.Cells(19).Value = Val(Format(Mtrs / 2, "#########0.00"))
                                End If

                            End If

                        End If

                    End If

                End If

            End If

            If .CurrentCell.ColumnIndex = 21 Then

                If Trim(.CurrentRow.Cells(.CurrentCell.ColumnIndex - 1).Value) <> "" And Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value) = 0 Then

                    Erase Wmtr

                    If Trim(txt_WarpMeters.Text) <> "" Then

                        Wmtr = Split(Trim(txt_WarpMeters.Text), ",")

                        If UBound(Wmtr) >= 0 Then

                            Mtrs = Format(Val(Wmtr(0)) * 2 / 3, "##########0.00")

                            If Val(.CurrentRow.Cells(11).Value) = Val(Mtrs / 2) And Val(.CurrentRow.Cells(13).Value) = Val(Mtrs / 2) And Val(.CurrentRow.Cells(15).Value) = Val(Mtrs) And Val(.CurrentRow.Cells(17).Value) = Val(Mtrs / 2) And Val(.CurrentRow.Cells(19).Value) = Val(Mtrs / 2) Then
                                Mtrs = Format(Val(Wmtr(0)) * 2 / 6, "#########0.00")
                                .CurrentRow.Cells(11).Value = Val(Mtrs)
                                .CurrentRow.Cells(13).Value = Val(Mtrs)
                                .CurrentRow.Cells(15).Value = Val(Mtrs)
                                .CurrentRow.Cells(17).Value = Val(Mtrs)
                                .CurrentRow.Cells(19).Value = Val(Mtrs)
                                .CurrentRow.Cells(21).Value = Val(Mtrs)
                            End If

                        End If

                    End If

                End If

            End If

        End With

    End Sub

    Private Sub dgv_WarpingDetails_Set1_CellLeave(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_WarpingDetails_Set1.CellLeave
        Dim Tm As String = ""

        Try

            With dgv_WarpingDetails_Set1
                If .CurrentCell.ColumnIndex = 2 Or .CurrentCell.ColumnIndex = 3 Or .CurrentCell.ColumnIndex = 4 Then
                    If Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value) <> 0 Then
                        .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = Format(Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value), "#########0.0")
                    Else
                        .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = ""
                    End If
                End If

                If .CurrentCell.ColumnIndex = 11 Or .CurrentCell.ColumnIndex = 13 Or .CurrentCell.ColumnIndex = 15 Or .CurrentCell.ColumnIndex = 17 Or .CurrentCell.ColumnIndex = 19 Or .CurrentCell.ColumnIndex = 21 Then
                    If Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value) <> 0 Then
                        .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = Format(Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value), "#########0.00")
                    Else
                        .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = ""
                    End If
                End If

                If .CurrentCell.ColumnIndex = 7 Or .CurrentCell.ColumnIndex = 8 Then
                    Tm = Replace(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value, ".", ":")
                    If Trim(Tm) <> "" Then
                        If Trim(Tm) = Trim(Val(Tm)) Then
                            Tm = Trim(Tm) & ":00"
                        End If
                        If IsDate(Tm) = True Then
                            .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = Format(Convert.ToDateTime(Tm), "hh:mm tt").ToString
                        End If
                    End If
                End If

            End With

        Catch ex As Exception
            '-----

        End Try

    End Sub

    Private Sub dgv_WarpingDetails_Set1_CellValueChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_WarpingDetails_Set1.CellValueChanged
        Dim TotMins As Long = 0
        Dim h As Long = 0
        Dim m As Long = 0
        Dim TmFrm As String = ""
        Dim TmTo As String = ""

        Try

            If FrmLdSTS = True Then Exit Sub
            If IsNothing(dgv_WarpingDetails_Set1.CurrentCell) Then Exit Sub
            With dgv_WarpingDetails_Set1

                If .Visible Then

                    If .Rows.Count > 0 Then

                        If .CurrentCell.ColumnIndex = 1 Or .CurrentCell.ColumnIndex = 2 Or .CurrentCell.ColumnIndex = 3 Then
                            If .CurrentCell.ColumnIndex = 2 Or .CurrentCell.ColumnIndex = 3 Then
                                .CurrentRow.Cells(4).Value = Format(Val(.CurrentRow.Cells(2).Value) - Val(.CurrentRow.Cells(3).Value), "#########0.0")
                            End If
                            TotalWarping_Calculation()
                        End If

                        If (.CurrentCell.ColumnIndex = 2 Or .CurrentCell.ColumnIndex = 3 Or .CurrentCell.ColumnIndex = 6 Or .CurrentCell.ColumnIndex = 7 Or .CurrentCell.ColumnIndex = 8 Or .CurrentCell.ColumnIndex = 11 Or .CurrentCell.ColumnIndex = 13 Or .CurrentCell.ColumnIndex = 15 Or .CurrentCell.ColumnIndex = 17 Or .CurrentCell.ColumnIndex = 19 Or .CurrentCell.ColumnIndex = 21) And Val(.CurrentCell.Value) <> 0 Then
                            If .CurrentRow.Index = .Rows.Count - 1 Then
                                .Rows.Add()
                            End If
                        End If

                        If .CurrentCell.ColumnIndex = 7 Or .CurrentCell.ColumnIndex = 8 Then

                            TmFrm = .CurrentRow.Cells(7).Value
                            TmTo = .CurrentRow.Cells(8).Value

                            If Trim(TmFrm) <> "" And Trim(TmTo) <> "" Then

                                If IsDate(TmFrm) = True And IsDate(TmTo) = True Then
                                    TotMins = DateDiff("n", TmFrm, TmTo)

                                    h = Int(TotMins / 60)
                                    m = TotMins - (h * 60)

                                    .CurrentRow.Cells(9).Value = Format(h, "00") & ":" & Format(m, "00")

                                End If

                            End If

                        End If

                    End If

                End If

            End With

        Catch ex As Exception
            '-----

        End Try
    End Sub

    Private Sub dgv_WarpingDetails_Set1_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_WarpingDetails_Set1.KeyUp
        Dim i As Integer = 0

        Try
            If e.Control = True And UCase(Chr(e.KeyCode)) = "D" Then

                With dgv_WarpingDetails_Set1

                    If Trim(txt_InvoiceCode.Text) = "" Then

                        If .Rows.Count > 0 Then

                            If .CurrentRow.Index = .RowCount - 1 Then
                                For i = 1 To .Columns.Count - 1
                                    .Rows(.CurrentRow.Index).Cells(i).Value = ""
                                Next

                            Else
                                .Rows.RemoveAt(.CurrentRow.Index)

                            End If

                            TotalWarping_Calculation()

                        End If

                    Else
                        MessageBox.Show("Invoice Prepared", "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                        Exit Sub

                    End If

                End With

            End If

        Catch ex As Exception
            '-------

        End Try

    End Sub

    Private Sub dgv_WarpingDetails_Set1_RowsAdded(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewRowsAddedEventArgs) Handles dgv_WarpingDetails_Set1.RowsAdded
        Dim n As Integer = 0

        Try
            If IsNothing(dgv_WarpingDetails_Set1.CurrentCell) Then Exit Sub
            With dgv_WarpingDetails_Set1
                n = .RowCount
                .Rows(n - 1).Cells(0).Value = Val(n)
            End With

        Catch ex As Exception
            '-----

        End Try
    End Sub

    Private Sub dgv_WarpingDetails_Set2_CellEndEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_WarpingDetails_Set2.CellEndEdit
        dgv_WarpingDetails_Set2_CellLeave(sender, e)
    End Sub

    Private Sub dgv_WarpingDetails_Set2_CellEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_WarpingDetails_Set2.CellEnter
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim Dt2 As New DataTable
        Dim rect As Rectangle
        With dgv_WarpingDetails_Set2

            If Val(.CurrentRow.Cells(0).Value) = 0 Then
                .CurrentRow.Cells(0).Value = .CurrentRow.Index + 1
            End If

            If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1028" Then '---- Chinnu Sizing (Palladam)
                If e.RowIndex > 0 And e.ColumnIndex = 1 Then
                    If Val(.CurrentRow.Cells(1).Value) = 0 And e.RowIndex = .RowCount - 1 Then
                        .CurrentRow.Cells(1).Value = .Rows(e.RowIndex - 1).Cells(1).Value
                    End If
                End If
            End If

            If e.ColumnIndex = 5 Then

                If cbo_Ends2_Shift.Visible = False Or Val(cbo_Ends2_Shift.Tag) <> e.RowIndex Then

                    rect = .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False)

                    cbo_Ends2_Shift.Left = .Left + rect.Left
                    cbo_Ends2_Shift.Top = .Top + rect.Top

                    cbo_Ends2_Shift.Width = rect.Width
                    cbo_Ends2_Shift.Height = rect.Height
                    cbo_Ends2_Shift.Text = .CurrentCell.Value

                    cbo_Ends2_Shift.Tag = Val(e.RowIndex)
                    cbo_Ends2_Shift.Visible = True

                    cbo_Ends2_Shift.BringToFront()
                    cbo_Ends2_Shift.Focus()

                End If

            Else
                cbo_Ends2_Shift.Visible = False
                cbo_Ends2_Shift.Tag = -1
                cbo_Ends2_Shift.Text = ""
            End If
            If e.ColumnIndex = 10 Then

                If cbo_Ends2_FrontWarper1.Visible = False Or Val(cbo_Ends2_FrontWarper1.Tag) <> e.RowIndex Then

                    cbo_Ends2_FrontWarper1.Tag = -1
                    Da = New SqlClient.SqlDataAdapter("select Employee_Name from PayRoll_Employee_Head order by Employee_Name", Con)
                    Dt1 = New DataTable
                    Da.Fill(Dt1)
                    cbo_Ends2_FrontWarper1.DataSource = Dt1
                    cbo_Ends2_FrontWarper1.DisplayMember = "Employee_Name"

                    rect = .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False)

                    cbo_Ends2_FrontWarper1.Left = .Left + rect.Left
                    cbo_Ends2_FrontWarper1.Top = .Top + rect.Top

                    cbo_Ends2_FrontWarper1.Width = rect.Width
                    cbo_Ends2_FrontWarper1.Height = rect.Height
                    cbo_Ends2_FrontWarper1.Text = .CurrentCell.Value

                    cbo_Ends2_FrontWarper1.Tag = Val(e.RowIndex)
                    cbo_Ends2_FrontWarper1.Visible = True

                    cbo_Ends2_FrontWarper1.BringToFront()
                    cbo_Ends2_FrontWarper1.Focus()


                End If

            Else
                cbo_Ends2_FrontWarper1.Visible = False
                cbo_Ends2_FrontWarper1.Tag = -1
                cbo_Ends2_FrontWarper1.Text = ""

            End If

            If e.ColumnIndex = 12 Then

                If cbo_Ends2_BackWarper1.Visible = False Or Val(cbo_Ends2_BackWarper1.Tag) <> e.RowIndex Then

                    cbo_Ends2_BackWarper1.Tag = -1
                    Da = New SqlClient.SqlDataAdapter("select Employee_Name from PayRoll_Employee_Head order by Employee_Name", Con)
                    Dt1 = New DataTable
                    Da.Fill(Dt1)
                    cbo_Ends2_BackWarper1.DataSource = Dt1
                    cbo_Ends2_BackWarper1.DisplayMember = "Employee_name"

                    rect = .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False)

                    cbo_Ends2_BackWarper1.Left = .Left + rect.Left
                    cbo_Ends2_BackWarper1.Top = .Top + rect.Top

                    cbo_Ends2_BackWarper1.Width = rect.Width
                    cbo_Ends2_BackWarper1.Height = rect.Height
                    cbo_Ends2_BackWarper1.Text = .CurrentCell.Value

                    cbo_Ends2_BackWarper1.Tag = Val(e.RowIndex)
                    cbo_Ends2_BackWarper1.Visible = True

                    cbo_Ends2_BackWarper1.BringToFront()
                    cbo_Ends2_BackWarper1.Focus()



                End If

            Else
                cbo_Ends2_BackWarper1.Visible = False
                cbo_Ends2_BackWarper1.Tag = -1
                cbo_Ends2_BackWarper1.Text = ""

            End If


            If .CurrentCell.ColumnIndex = 14 Then

                If cbo_Ends2_Helper1.Visible = False Or Val(cbo_Ends2_Helper1.Tag) <> e.RowIndex Then

                    cbo_Ends2_Helper1.Tag = -1
                    Da = New SqlClient.SqlDataAdapter("select Employee_Name from PayRoll_Employee_Head order by Employee_Name", Con)
                    Dt1 = New DataTable
                    Da.Fill(Dt1)
                    cbo_Ends2_Helper1.DataSource = Dt1
                    cbo_Ends2_Helper1.DisplayMember = "Employee_Name"

                    rect = .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False)

                    cbo_Ends2_Helper1.Left = .Left + rect.Left
                    cbo_Ends2_Helper1.Top = .Top + rect.Top

                    cbo_Ends2_Helper1.Width = rect.Width
                    cbo_Ends2_Helper1.Height = rect.Height
                    cbo_Ends2_Helper1.Text = .CurrentCell.Value

                    cbo_Ends2_Helper1.Tag = Val(e.RowIndex)
                    cbo_Ends2_Helper1.Visible = True

                    cbo_Ends2_Helper1.BringToFront()
                    cbo_Ends2_Helper1.Focus()

                End If

            Else
                cbo_Ends2_Helper1.Visible = False
                cbo_Ends2_Helper1.Tag = -1
                cbo_Ends2_Helper1.Text = ""

            End If
            If e.ColumnIndex = 16 Then

                If cbo_Ends2_FrontWarper2.Visible = False Or Val(cbo_Ends2_FrontWarper2.Tag) <> e.RowIndex Then

                    cbo_Ends2_FrontWarper2.Tag = -1
                    Da = New SqlClient.SqlDataAdapter("select Employee_Name from PayRoll_Employee_Head order by Employee_Name", Con)
                    Dt1 = New DataTable
                    Da.Fill(Dt1)
                    cbo_Ends2_FrontWarper2.DataSource = Dt1
                    cbo_Ends2_FrontWarper2.DisplayMember = "Employee_Name"

                    rect = .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False)

                    cbo_Ends2_FrontWarper2.Left = .Left + rect.Left
                    cbo_Ends2_FrontWarper2.Top = .Top + rect.Top

                    cbo_Ends2_FrontWarper2.Width = rect.Width
                    cbo_Ends2_FrontWarper2.Height = rect.Height
                    cbo_Ends2_FrontWarper2.Text = .CurrentCell.Value

                    cbo_Ends2_FrontWarper2.Tag = Val(e.RowIndex)
                    cbo_Ends2_FrontWarper2.Visible = True

                    cbo_Ends2_FrontWarper2.BringToFront()
                    cbo_Ends2_FrontWarper2.Focus()


                End If

            Else
                cbo_Ends2_FrontWarper2.Visible = False
                cbo_Ends2_FrontWarper2.Tag = -1
                cbo_Ends2_FrontWarper2.Text = ""

            End If

            If e.ColumnIndex = 18 Then

                If cbo_Ends2_BackWarper2.Visible = False Or Val(cbo_Ends2_BackWarper2.Tag) <> e.RowIndex Then

                    cbo_Ends2_BackWarper2.Tag = -1
                    Da = New SqlClient.SqlDataAdapter("select Employee_Name from PayRoll_Employee_Head order by Employee_Name", Con)
                    Dt1 = New DataTable
                    Da.Fill(Dt1)
                    cbo_Ends2_BackWarper2.DataSource = Dt1
                    cbo_Ends2_BackWarper2.DisplayMember = "Employee_name"

                    rect = .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False)

                    cbo_Ends2_BackWarper2.Left = .Left + rect.Left
                    cbo_Ends2_BackWarper2.Top = .Top + rect.Top

                    cbo_Ends2_BackWarper2.Width = rect.Width
                    cbo_Ends2_BackWarper2.Height = rect.Height
                    cbo_Ends2_BackWarper2.Text = .CurrentCell.Value

                    cbo_Ends2_BackWarper2.Tag = Val(e.RowIndex)
                    cbo_Ends2_BackWarper2.Visible = True

                    cbo_Ends2_BackWarper2.BringToFront()
                    cbo_Ends2_BackWarper2.Focus()



                End If

            Else
                cbo_Ends2_BackWarper2.Visible = False
                cbo_Ends2_BackWarper2.Tag = -1
                cbo_Ends2_BackWarper2.Text = ""

            End If


            If .CurrentCell.ColumnIndex = 20 Then

                If cbo_Ends2_Helper2.Visible = False Or Val(cbo_Ends2_Helper2.Tag) <> e.RowIndex Then

                    cbo_Ends2_Helper2.Tag = -1
                    Da = New SqlClient.SqlDataAdapter("select Employee_Name from PayRoll_Employee_Head order by Employee_Name", Con)
                    Dt1 = New DataTable
                    Da.Fill(Dt1)
                    cbo_Ends2_Helper2.DataSource = Dt1
                    cbo_Ends2_Helper2.DisplayMember = "Employee_Name"

                    rect = .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False)

                    cbo_Ends2_Helper2.Left = .Left + rect.Left
                    cbo_Ends2_Helper2.Top = .Top + rect.Top

                    cbo_Ends2_Helper2.Width = rect.Width
                    cbo_Ends2_Helper2.Height = rect.Height
                    cbo_Ends2_Helper2.Text = .CurrentCell.Value

                    cbo_Ends2_Helper2.Tag = Val(e.RowIndex)
                    cbo_Ends2_Helper2.Visible = True

                    cbo_Ends2_Helper2.BringToFront()
                    cbo_Ends2_Helper2.Focus()

                End If

            Else
                cbo_Ends2_Helper2.Visible = False
                cbo_Ends2_Helper2.Tag = -1
                cbo_Ends2_Helper2.Text = ""

            End If
        End With
    End Sub

    Private Sub dgv_WarpingDetails_Set2_CellLeave(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_WarpingDetails_Set2.CellLeave
        With dgv_WarpingDetails_Set2
            If .CurrentCell.ColumnIndex = 2 Or .CurrentCell.ColumnIndex = 3 Or .CurrentCell.ColumnIndex = 4 Then
                If Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value) <> 0 Then
                    .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = Format(Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value), "#########0.0")
                End If
            End If
            If .CurrentCell.ColumnIndex = 7 Or .CurrentCell.ColumnIndex = 8 Or .CurrentCell.ColumnIndex = 9 Or .CurrentCell.ColumnIndex = 11 Or .CurrentCell.ColumnIndex = 13 Or .CurrentCell.ColumnIndex = 15 Or .CurrentCell.ColumnIndex = 17 Or .CurrentCell.ColumnIndex = 19 Or .CurrentCell.ColumnIndex = 21 Then
                If Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value) <> 0 Then
                    .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = Format(Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value), "#########0.00")
                Else
                    .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = ""
                End If
            End If
        End With
    End Sub

    Private Sub dgv_WarpingDetails_Set2_CellValueChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_WarpingDetails_Set2.CellValueChanged
        Try
            If FrmLdSTS = True Then Exit Sub
            With dgv_WarpingDetails_Set2
                If .Visible Then
                    If .CurrentCell.ColumnIndex = 1 Or .CurrentCell.ColumnIndex = 2 Or .CurrentCell.ColumnIndex = 3 Then
                        If .CurrentCell.ColumnIndex = 2 Or .CurrentCell.ColumnIndex = 3 Then
                            .CurrentRow.Cells(4).Value = Format(Val(.CurrentRow.Cells(2).Value) - Val(.CurrentRow.Cells(3).Value), "#########0.0")
                        End If
                        TotalWarping_Calculation()
                    End If
                    If (.CurrentCell.ColumnIndex = 2 Or .CurrentCell.ColumnIndex = 3) And Val(.CurrentCell.Value) <> 0 Then
                        If .CurrentRow.Index = .Rows.Count - 1 Then
                            .Rows.Add()
                        End If
                    End If
                End If
            End With

        Catch ex As Exception
            '-----

        End Try
    End Sub

    Private Sub dgv_WarpingDetails_Set2_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_WarpingDetails_Set2.KeyUp
        vcbo_KeyDwnVal = e.KeyValue
        Dim i As Integer

        If e.Control = True And UCase(Chr(e.KeyCode)) = "D" Then

            With dgv_WarpingDetails_Set2

                If Trim(txt_InvoiceCode.Text) = "" Then

                    If .CurrentRow.Index = .RowCount - 1 Then
                        For i = 1 To .Columns.Count - 1
                            .Rows(.CurrentRow.Index).Cells(i).Value = ""
                        Next

                    Else
                        .Rows.RemoveAt(.CurrentRow.Index)

                    End If

                    TotalWarping_Calculation()

                Else
                    MessageBox.Show("Invoice Prepared", "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                    Exit Sub

                End If

            End With

        End If

    End Sub

    Private Sub dgv_WarpingDetails_Set2_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgv_WarpingDetails_Set2.LostFocus
        On Error Resume Next
        dgv_WarpingDetails_Set2.CurrentCell.Selected = False
    End Sub

    Private Sub dgv_WarpingDetails_Set2_RowsAdded(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewRowsAddedEventArgs) Handles dgv_WarpingDetails_Set2.RowsAdded
        Dim n As Integer

        With dgv_WarpingDetails_Set2
            n = .RowCount
            .Rows(n - 1).Cells(0).Value = Val(n)
        End With
    End Sub

    Private Sub dgv_WarpingDetails_Set3_CellEndEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_WarpingDetails_Set3.CellEndEdit
        dgv_WarpingDetails_Set3_CellLeave(sender, e)
    End Sub

    Private Sub dgv_WarpingDetails_Set3_CellEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_WarpingDetails_Set3.CellEnter
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim Dt2 As New DataTable
        Dim rect As Rectangle
        With dgv_WarpingDetails_Set3

            If Val(.CurrentRow.Cells(0).Value) = 0 Then
                .CurrentRow.Cells(0).Value = .CurrentRow.Index + 1
            End If

            If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1028" Then '---- Chinnu Sizing (Palladam)
                If e.RowIndex > 0 And e.ColumnIndex = 1 Then
                    If Val(.CurrentRow.Cells(1).Value) = 0 And e.RowIndex = .RowCount - 1 Then
                        .CurrentRow.Cells(1).Value = .Rows(e.RowIndex - 1).Cells(1).Value
                    End If
                End If
            End If

            If e.ColumnIndex = 5 Then

                If Cbo_Ends3_Shift.Visible = False Or Val(Cbo_Ends3_Shift.Tag) <> e.RowIndex Then

                    rect = .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False)

                    Cbo_Ends3_Shift.Left = .Left + rect.Left
                    Cbo_Ends3_Shift.Top = .Top + rect.Top

                    Cbo_Ends3_Shift.Width = rect.Width
                    Cbo_Ends3_Shift.Height = rect.Height
                    Cbo_Ends3_Shift.Text = .CurrentCell.Value

                    Cbo_Ends3_Shift.Tag = Val(e.RowIndex)
                    Cbo_Ends3_Shift.Visible = True

                    Cbo_Ends3_Shift.BringToFront()
                    Cbo_Ends3_Shift.Focus()

                End If

            Else
                Cbo_Ends3_Shift.Visible = False
                Cbo_Ends3_Shift.Tag = -1
                Cbo_Ends3_Shift.Text = ""
            End If

            If e.ColumnIndex = 10 Then

                If cbo_Ends3_FrontWarper1.Visible = False Or Val(cbo_Ends3_FrontWarper1.Tag) <> e.RowIndex Then

                    cbo_Ends3_FrontWarper1.Tag = -1
                    Da = New SqlClient.SqlDataAdapter("select Employee_Name from PayRoll_Employee_Head order by Employee_Name", Con)
                    Dt1 = New DataTable
                    Da.Fill(Dt1)
                    cbo_Ends3_FrontWarper1.DataSource = Dt1
                    cbo_Ends3_FrontWarper1.DisplayMember = "Employee_Name"

                    rect = .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False)

                    cbo_Ends3_FrontWarper1.Left = .Left + rect.Left
                    cbo_Ends3_FrontWarper1.Top = .Top + rect.Top

                    cbo_Ends3_FrontWarper1.Width = rect.Width
                    cbo_Ends3_FrontWarper1.Height = rect.Height
                    cbo_Ends3_FrontWarper1.Text = .CurrentCell.Value

                    cbo_Ends3_FrontWarper1.Tag = Val(e.RowIndex)
                    cbo_Ends3_FrontWarper1.Visible = True

                    cbo_Ends3_FrontWarper1.BringToFront()
                    cbo_Ends3_FrontWarper1.Focus()


                End If

            Else
                cbo_Ends3_FrontWarper1.Visible = False
                cbo_Ends3_FrontWarper1.Tag = -1
                cbo_Ends3_FrontWarper1.Text = ""

            End If

            If e.ColumnIndex = 12 Then

                If cbo_Ends3_BackWarper1.Visible = False Or Val(cbo_Ends3_BackWarper1.Tag) <> e.RowIndex Then

                    cbo_Ends3_BackWarper1.Tag = -1
                    Da = New SqlClient.SqlDataAdapter("select Employee_Name from PayRoll_Employee_Head order by Employee_Name", Con)
                    Dt1 = New DataTable
                    Da.Fill(Dt1)
                    cbo_Ends3_BackWarper1.DataSource = Dt1
                    cbo_Ends3_BackWarper1.DisplayMember = "Employee_name"

                    rect = .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False)

                    cbo_Ends3_BackWarper1.Left = .Left + rect.Left
                    cbo_Ends3_BackWarper1.Top = .Top + rect.Top

                    cbo_Ends3_BackWarper1.Width = rect.Width
                    cbo_Ends3_BackWarper1.Height = rect.Height
                    cbo_Ends3_BackWarper1.Text = .CurrentCell.Value

                    cbo_Ends3_BackWarper1.Tag = Val(e.RowIndex)
                    cbo_Ends3_BackWarper1.Visible = True

                    cbo_Ends3_BackWarper1.BringToFront()
                    cbo_Ends3_BackWarper1.Focus()



                End If

            Else
                cbo_Ends3_BackWarper1.Visible = False
                cbo_Ends3_BackWarper1.Tag = -1
                cbo_Ends3_BackWarper1.Text = ""

            End If


            If .CurrentCell.ColumnIndex = 14 Then

                If cbo_Ends3_Helper1.Visible = False Or Val(cbo_Ends3_Helper1.Tag) <> e.RowIndex Then

                    cbo_Ends3_Helper1.Tag = -1
                    Da = New SqlClient.SqlDataAdapter("select Employee_Name from PayRoll_Employee_Head order by Employee_Name", Con)
                    Dt1 = New DataTable
                    Da.Fill(Dt1)
                    cbo_Ends3_Helper1.DataSource = Dt1
                    cbo_Ends3_Helper1.DisplayMember = "Employee_Name"

                    rect = .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False)

                    cbo_Ends3_Helper1.Left = .Left + rect.Left
                    cbo_Ends3_Helper1.Top = .Top + rect.Top

                    cbo_Ends3_Helper1.Width = rect.Width
                    cbo_Ends3_Helper1.Height = rect.Height
                    cbo_Ends3_Helper1.Text = .CurrentCell.Value

                    cbo_Ends3_Helper1.Tag = Val(e.RowIndex)
                    cbo_Ends3_Helper1.Visible = True

                    cbo_Ends3_Helper1.BringToFront()
                    cbo_Ends3_Helper1.Focus()

                End If

            Else
                cbo_Ends3_Helper1.Visible = False
                cbo_Ends3_Helper1.Tag = -1
                cbo_Ends3_Helper1.Text = ""

            End If
            If e.ColumnIndex = 16 Then

                If cbo_Ends3_FrontWarper2.Visible = False Or Val(cbo_Ends3_FrontWarper2.Tag) <> e.RowIndex Then

                    cbo_Ends3_FrontWarper2.Tag = -1
                    Da = New SqlClient.SqlDataAdapter("select Employee_Name from PayRoll_Employee_Head order by Employee_Name", Con)
                    Dt1 = New DataTable
                    Da.Fill(Dt1)
                    cbo_Ends3_FrontWarper2.DataSource = Dt1
                    cbo_Ends3_FrontWarper2.DisplayMember = "Employee_Name"

                    rect = .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False)

                    cbo_Ends3_FrontWarper2.Left = .Left + rect.Left
                    cbo_Ends3_FrontWarper2.Top = .Top + rect.Top

                    cbo_Ends3_FrontWarper2.Width = rect.Width
                    cbo_Ends3_FrontWarper2.Height = rect.Height
                    cbo_Ends3_FrontWarper2.Text = .CurrentCell.Value

                    cbo_Ends3_FrontWarper2.Tag = Val(e.RowIndex)
                    cbo_Ends3_FrontWarper2.Visible = True

                    cbo_Ends3_FrontWarper2.BringToFront()
                    cbo_Ends3_FrontWarper2.Focus()


                End If

            Else
                cbo_Ends3_FrontWarper2.Visible = False
                cbo_Ends3_FrontWarper2.Tag = -1
                cbo_Ends3_FrontWarper2.Text = ""

            End If

            If e.ColumnIndex = 18 Then

                If cbo_Ends3_BackWarper2.Visible = False Or Val(cbo_Ends3_BackWarper2.Tag) <> e.RowIndex Then

                    cbo_Ends3_BackWarper2.Tag = -1
                    Da = New SqlClient.SqlDataAdapter("select Employee_Name from PayRoll_Employee_Head order by Employee_Name", Con)
                    Dt1 = New DataTable
                    Da.Fill(Dt1)
                    cbo_Ends3_BackWarper2.DataSource = Dt1
                    cbo_Ends3_BackWarper2.DisplayMember = "Employee_name"

                    rect = .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False)

                    cbo_Ends3_BackWarper2.Left = .Left + rect.Left
                    cbo_Ends3_BackWarper2.Top = .Top + rect.Top

                    cbo_Ends3_BackWarper2.Width = rect.Width
                    cbo_Ends3_BackWarper2.Height = rect.Height
                    cbo_Ends3_BackWarper2.Text = .CurrentCell.Value

                    cbo_Ends3_BackWarper2.Tag = Val(e.RowIndex)
                    cbo_Ends3_BackWarper2.Visible = True

                    cbo_Ends3_BackWarper2.BringToFront()
                    cbo_Ends3_BackWarper2.Focus()



                End If

            Else
                cbo_Ends3_BackWarper2.Visible = False
                cbo_Ends3_BackWarper2.Tag = -1
                cbo_Ends3_BackWarper2.Text = ""

            End If


            If .CurrentCell.ColumnIndex = 20 Then

                If cbo_Ends3_Helper2.Visible = False Or Val(cbo_Ends3_Helper2.Tag) <> e.RowIndex Then

                    cbo_Ends3_Helper2.Tag = -1
                    Da = New SqlClient.SqlDataAdapter("select Employee_Name from PayRoll_Employee_Head order by Employee_Name", Con)
                    Dt1 = New DataTable
                    Da.Fill(Dt1)
                    cbo_Ends3_Helper2.DataSource = Dt1
                    cbo_Ends3_Helper2.DisplayMember = "Employee_Name"

                    rect = .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False)

                    cbo_Ends3_Helper2.Left = .Left + rect.Left
                    cbo_Ends3_Helper2.Top = .Top + rect.Top

                    cbo_Ends3_Helper2.Width = rect.Width
                    cbo_Ends3_Helper2.Height = rect.Height
                    cbo_Ends3_Helper2.Text = .CurrentCell.Value

                    cbo_Ends3_Helper2.Tag = Val(e.RowIndex)
                    cbo_Ends3_Helper2.Visible = True

                    cbo_Ends3_Helper2.BringToFront()
                    cbo_Ends3_Helper2.Focus()

                End If

            Else
                cbo_Ends3_Helper2.Visible = False
                cbo_Ends3_Helper2.Tag = -1
                cbo_Ends3_Helper2.Text = ""

            End If
        End With
    End Sub

    Private Sub dgv_WarpingDetails_Set3_CellLeave(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_WarpingDetails_Set3.CellLeave
        With dgv_WarpingDetails_Set3
            If .CurrentCell.ColumnIndex = 2 Or .CurrentCell.ColumnIndex = 3 Or .CurrentCell.ColumnIndex = 4 Then
                If Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value) <> 0 Then
                    .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = Format(Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value), "#########0.0")
                End If
            End If
            If .CurrentCell.ColumnIndex = 7 Or .CurrentCell.ColumnIndex = 8 Or .CurrentCell.ColumnIndex = 9 Or .CurrentCell.ColumnIndex = 11 Or .CurrentCell.ColumnIndex = 13 Or .CurrentCell.ColumnIndex = 15 Or .CurrentCell.ColumnIndex = 17 Or .CurrentCell.ColumnIndex = 19 Or .CurrentCell.ColumnIndex = 21 Then
                If Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value) <> 0 Then
                    .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = Format(Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value), "#########0.00")
                Else
                    .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = ""
                End If
            End If
        End With
    End Sub

    Private Sub dgv_WarpingDetails_Set3_CellValueChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_WarpingDetails_Set3.CellValueChanged
        Try
            If FrmLdSTS = True Then Exit Sub
            With dgv_WarpingDetails_Set3
                If .Visible Then
                    If .CurrentCell.ColumnIndex = 1 Or .CurrentCell.ColumnIndex = 2 Or .CurrentCell.ColumnIndex = 3 Then
                        If .CurrentCell.ColumnIndex = 2 Or .CurrentCell.ColumnIndex = 3 Then
                            .CurrentRow.Cells(4).Value = Format(Val(.CurrentRow.Cells(2).Value) - Val(.CurrentRow.Cells(3).Value), "#########0.0")
                        End If
                        TotalWarping_Calculation()
                    End If
                    If (.CurrentCell.ColumnIndex = 2 Or .CurrentCell.ColumnIndex = 3) And Val(.CurrentCell.Value) <> 0 Then
                        If .CurrentRow.Index = .Rows.Count - 1 Then
                            .Rows.Add()
                        End If
                    End If
                End If
            End With

        Catch ex As Exception
            '-----

        End Try
    End Sub

    Private Sub dgv_WarpingDetails_Set3_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_WarpingDetails_Set3.KeyUp
        Dim i As Integer

        If e.Control = True And UCase(Chr(e.KeyCode)) = "D" Then

            With dgv_WarpingDetails_Set3

                If Trim(txt_InvoiceCode.Text) = "" Then

                    If .CurrentRow.Index = .RowCount - 1 Then
                        For i = 1 To .Columns.Count - 1
                            .Rows(.CurrentRow.Index).Cells(i).Value = ""
                        Next

                    Else
                        .Rows.RemoveAt(.CurrentRow.Index)

                    End If

                    TotalWarping_Calculation()

                Else
                    MessageBox.Show("Invoice Prepared", "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                    Exit Sub

                End If

            End With

        End If

    End Sub

    Private Sub dgv_WarpingDetails_Set3_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgv_WarpingDetails_Set3.LostFocus
        On Error Resume Next
        dgv_WarpingDetails_Set3.CurrentCell.Selected = False
    End Sub

    Private Sub dgv_WarpingDetails_Set3_RowsAdded(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewRowsAddedEventArgs) Handles dgv_WarpingDetails_Set3.RowsAdded
        Dim n As Integer

        With dgv_WarpingDetails_Set3
            n = .RowCount
            .Rows(n - 1).Cells(0).Value = Val(n)
        End With
    End Sub

    Private Sub dgv_WarpingDetails_Total_Set1_CellValueChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_WarpingDetails_Total_Set1.CellValueChanged
        'If e.ColumnIndex = 1 Then
        '    TotalEnds_Calculation()
        'End If
        If e.ColumnIndex = 1 Or e.ColumnIndex = 4 Then
            BeamCount_Calculation()
        End If
    End Sub

    Private Sub dgv_WarpingDetails_Total_Set2_CellValueChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_WarpingDetails_Total_Set2.CellValueChanged
        'If e.ColumnIndex = 1 Then
        '    TotalEnds_Calculation()
        'End If
        If e.ColumnIndex = 1 Or e.ColumnIndex = 4 Then
            BeamCount_Calculation()
        End If
    End Sub

    Private Sub dgv_WarpingDetails_Total_Set3_CellValueChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_WarpingDetails_Total_Set3.CellValueChanged
        'If e.ColumnIndex = 1 Then
        '    TotalEnds_Calculation()
        'End If
        If e.ColumnIndex = 1 Or e.ColumnIndex = 4 Then
            BeamCount_Calculation()
        End If
    End Sub

    Private Sub TotalEnds_Calculation()
        'Dim TotEnds As Integer

        'TotEnds = 0
        'If dgv_WarpingDetails_Total_Set1.RowCount > 0 Then
        '    TotEnds = Val(dgv_WarpingDetails_Total_Set1.Rows(0).Cells(1).Value())
        'End If
        'txt_Ends.Text = Val(TotEnds)

        'TotEnds = 0
        'If dgv_WarpingDetails_Total_Set2.RowCount > 0 Then
        '    TotEnds = Val(dgv_WarpingDetails_Total_Set2.Rows(0).Cells(1).Value())
        'End If
        'If Val(TotEnds) <> 0 Then
        '    txt_Ends.Text = Trim(txt_Ends.Text) & IIf(Trim(txt_Ends.Text) <> "", ",", "") & Val(TotEnds)
        'End If

        'TotEnds = 0
        'If dgv_WarpingDetails_Total_Set3.RowCount > 0 Then
        '    TotEnds = Val(dgv_WarpingDetails_Total_Set3.Rows(0).Cells(1).Value())
        'End If
        'If Val(TotEnds) <> 0 Then
        '    txt_Ends.Text = Trim(txt_Ends.Text) & IIf(Trim(txt_Ends.Text) <> "", ",", "") & Val(TotEnds)
        'End If

    End Sub

    Private Sub TotalWarping_Calculation()
        Dim Sno As Integer
        Dim TotWrpBms As Integer, TotWrpEnds As Integer
        Dim TotGrsWt As Single, TotTrWt As Single, TotNtWt As Single
        Dim GTtWrpBms As Integer, GTtWrpEnds As Integer
        Dim GTtGrsWt As Single, GTtTrWt As Single, GTtNtWt As Single

        GTtWrpBms = 0
        GTtWrpEnds = 0
        GTtGrsWt = 0
        GTtTrWt = 0
        GTtNtWt = 0

        Sno = 0
        TotWrpBms = 0
        TotWrpEnds = 0
        TotGrsWt = 0
        TotTrWt = 0
        TotNtWt = 0
        With dgv_WarpingDetails_Set1
            For i = 0 To .RowCount - 1
                Sno = Sno + 1
                .Rows(i).Cells(0).Value = Sno
                If Val(.Rows(i).Cells(4).Value) <> 0 Then
                    TotWrpBms = TotWrpBms + 1
                    TotWrpEnds = TotWrpEnds + Val(.Rows(i).Cells(1).Value)
                    TotGrsWt = TotGrsWt + Val(.Rows(i).Cells(2).Value)
                    TotTrWt = TotTrWt + Val(.Rows(i).Cells(3).Value)
                    TotNtWt = TotNtWt + Val(.Rows(i).Cells(4).Value)
                End If
            Next
        End With

        With dgv_WarpingDetails_Total_Set1
            If .RowCount = 0 Then .Rows.Add()
            .Rows(0).Cells(0).Value = Val(TotWrpBms)
            .Rows(0).Cells(1).Value = Val(TotWrpEnds)
            .Rows(0).Cells(2).Value = Format(Val(TotGrsWt), "########0.000")
            .Rows(0).Cells(3).Value = Format(Val(TotTrWt), "########0.000")
            .Rows(0).Cells(4).Value = Format(Val(TotNtWt), "########0.000")
        End With

        GTtWrpBms = GTtWrpBms + TotWrpBms
        GTtWrpEnds = GTtWrpEnds + TotWrpEnds
        GTtGrsWt = GTtGrsWt + TotGrsWt
        GTtTrWt = GTtTrWt + TotTrWt
        GTtNtWt = GTtNtWt + TotNtWt

        Sno = 0
        TotWrpBms = 0
        TotWrpEnds = 0
        TotGrsWt = 0
        TotTrWt = 0
        TotNtWt = 0
        With dgv_WarpingDetails_Set2
            For i = 0 To .RowCount - 1
                Sno = Sno + 1
                .Rows(i).Cells(0).Value = Sno
                If Val(.Rows(i).Cells(4).Value) <> 0 Then
                    TotWrpBms = TotWrpBms + 1
                    TotWrpEnds = TotWrpEnds + Val(.Rows(i).Cells(1).Value)
                    TotGrsWt = TotGrsWt + Val(.Rows(i).Cells(2).Value)
                    TotTrWt = TotTrWt + Val(.Rows(i).Cells(3).Value)
                    TotNtWt = TotNtWt + Val(.Rows(i).Cells(4).Value)
                End If
            Next
        End With

        With dgv_WarpingDetails_Total_Set2
            If .RowCount = 0 Then .Rows.Add()
            .Rows(0).Cells(0).Value = Val(TotWrpBms)
            .Rows(0).Cells(1).Value = Val(TotWrpEnds)
            .Rows(0).Cells(2).Value = Format(Val(TotGrsWt), "########0.000")
            .Rows(0).Cells(3).Value = Format(Val(TotTrWt), "########0.000")
            .Rows(0).Cells(4).Value = Format(Val(TotNtWt), "########0.000")
        End With

        GTtWrpBms = GTtWrpBms + TotWrpBms
        GTtWrpEnds = GTtWrpEnds + TotWrpEnds
        GTtGrsWt = GTtGrsWt + TotGrsWt
        GTtTrWt = GTtTrWt + TotTrWt
        GTtNtWt = GTtNtWt + TotNtWt

        Sno = 0
        TotWrpBms = 0
        TotWrpEnds = 0
        TotGrsWt = 0
        TotTrWt = 0
        TotNtWt = 0
        With dgv_WarpingDetails_Set3
            For i = 0 To .RowCount - 1
                Sno = Sno + 1
                .Rows(i).Cells(0).Value = Sno
                If Val(.Rows(i).Cells(4).Value) <> 0 Then
                    TotWrpBms = TotWrpBms + 1
                    TotWrpEnds = TotWrpEnds + Val(.Rows(i).Cells(1).Value)
                    TotGrsWt = TotGrsWt + Val(.Rows(i).Cells(2).Value)
                    TotTrWt = TotTrWt + Val(.Rows(i).Cells(3).Value)
                    TotNtWt = TotNtWt + Val(.Rows(i).Cells(4).Value)
                End If
            Next
        End With

        With dgv_WarpingDetails_Total_Set3
            If .RowCount = 0 Then .Rows.Add()
            .Rows(0).Cells(0).Value = Val(TotWrpBms)
            .Rows(0).Cells(1).Value = Val(TotWrpEnds)
            .Rows(0).Cells(2).Value = Format(Val(TotGrsWt), "########0.000")
            .Rows(0).Cells(3).Value = Format(Val(TotTrWt), "########0.000")
            .Rows(0).Cells(4).Value = Format(Val(TotNtWt), "########0.000")
        End With

        GTtWrpBms = GTtWrpBms + TotWrpBms
        GTtWrpEnds = GTtWrpEnds + TotWrpEnds
        GTtGrsWt = GTtGrsWt + TotGrsWt
        GTtTrWt = GTtTrWt + TotTrWt
        GTtNtWt = GTtNtWt + TotNtWt

        lbl_Total_Warping_Beams.Text = GTtWrpBms
        lbl_Total_Warping_Ends.Text = GTtWrpEnds
        lbl_Total_Warping_GrossWeight.Text = Format(Val(GTtGrsWt), "#########0.000")
        lbl_Total_Warping_TareWeight.Text = Format(Val(GTtTrWt), "#########0.000")
        lbl_Total_Warping_NetWeight.Text = Format(Val(GTtNtWt), "#########0.000")

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1102" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1144" Then
            lbl_ConsumedYarn.Text = Format(Val(GTtNtWt), "#########0.0")
        Else
            lbl_ConsumedYarn.Text = Format(Val(GTtNtWt), "#########0.000")

        End If

    End Sub

    Private Sub TabPage5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TabPage5.Click
        If dgv_WarpingDetails_Set1.Rows.Count <= 0 Then dgv_WarpingDetails_Set1.Rows.Add()
        dgv_WarpingDetails_Set1.Focus()
        dgv_WarpingDetails_Set1.CurrentCell = dgv_WarpingDetails_Set1.Rows(0).Cells(1)
        dgv_WarpingDetails_Set1.CurrentCell.Selected = True
    End Sub

    Private Sub TabPage6_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TabPage6.Click
        If dgv_WarpingDetails_Set2.Rows.Count <= 0 Then dgv_WarpingDetails_Set2.Rows.Add()
        dgv_WarpingDetails_Set2.Focus()
        dgv_WarpingDetails_Set2.CurrentCell = dgv_WarpingDetails_Set2.Rows(0).Cells(1)
        dgv_WarpingDetails_Set2.CurrentCell.Selected = True
    End Sub

    Private Sub TabPage9_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TabPage9.Click
        If dgv_WarpingDetails_Set3.Rows.Count <= 0 Then dgv_WarpingDetails_Set3.Rows.Add()
        dgv_WarpingDetails_Set3.Focus()
        dgv_WarpingDetails_Set3.CurrentCell = dgv_WarpingDetails_Set3.Rows(0).Cells(1)
        dgv_WarpingDetails_Set3.CurrentCell.Selected = True
    End Sub

    Private Sub tab_WarpingDeatils_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tab_WarpingDeatils.SelectedIndexChanged
        If tab_WarpingDeatils.SelectedIndex = 0 Then
            If dgv_WarpingDetails_Set1.Rows.Count <= 0 Then dgv_WarpingDetails_Set1.Rows.Add()
            dgv_WarpingDetails_Set1.CurrentCell = dgv_WarpingDetails_Set1.Rows(0).Cells(1)
            dgv_WarpingDetails_Set1.Focus()
            dgv_WarpingDetails_Set1.CurrentCell.Selected = True

        ElseIf tab_WarpingDeatils.SelectedIndex = 1 Then
            If dgv_WarpingDetails_Set2.Rows.Count <= 0 Then dgv_WarpingDetails_Set2.Rows.Add()
            dgv_WarpingDetails_Set2.CurrentCell = dgv_WarpingDetails_Set2.Rows(0).Cells(1)
            dgv_WarpingDetails_Set2.Focus()
            dgv_WarpingDetails_Set2.CurrentCell.Selected = True

        ElseIf tab_WarpingDeatils.SelectedIndex = 2 Then
            If dgv_WarpingDetails_Set3.Rows.Count <= 0 Then dgv_WarpingDetails_Set3.Rows.Add()
            dgv_WarpingDetails_Set3.CurrentCell = dgv_WarpingDetails_Set3.Rows(0).Cells(1)
            dgv_WarpingDetails_Set3.Focus()
            dgv_WarpingDetails_Set3.CurrentCell.Selected = True

        End If
    End Sub

    Private Sub tab_Main_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tab_Main.SelectedIndexChanged
        If tab_Main.SelectedIndex = 0 Then
            tab_WarpingDeatils.SelectTab(0)
            If dgv_WarpingDetails_Set1.Rows.Count <= 0 Then dgv_WarpingDetails_Set1.Rows.Add()
            dgv_WarpingDetails_Set1.CurrentCell = dgv_WarpingDetails_Set1.Rows(0).Cells(1)
            dgv_WarpingDetails_Set1.Focus()
            dgv_WarpingDetails_Set1.CurrentCell.Selected = True



        ElseIf tab_Main.SelectedIndex = 1 Then
            dgv_YarnTakenDetails.CurrentCell = dgv_YarnTakenDetails.Rows(0).Cells(1)
            dgv_YarnTakenDetails.Focus()
            dgv_YarnTakenDetails.CurrentCell.Selected = True
            If cbo_Grid_CountName.Visible And cbo_Grid_CountName.Enabled Then
                cbo_Grid_CountName.Focus()
            End If

        ElseIf tab_Main.SelectedIndex = 2 Then
            dgv_BabyConeDetails.CurrentCell = dgv_BabyConeDetails.Rows(0).Cells(1)
            dgv_BabyConeDetails.Focus()
            dgv_BabyConeDetails.CurrentCell.Selected = True



        End If
    End Sub

    Private Sub dgv_YarnTakenDetails_CellEndEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_YarnTakenDetails.CellEndEdit
        dgv_YarnTakenDetails_CellLeave(sender, e)
        If dgv_YarnTakenDetails.CurrentRow.Cells(2).Value = "MILL" Then
            If dgv_YarnTakenDetails.CurrentCell.ColumnIndex = 5 Or dgv_YarnTakenDetails.CurrentCell.ColumnIndex = 7 Then
                get_MillCount_Details()
            End If
        End If
    End Sub

    Private Sub dgv_YarnTakenDetails_CellEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_YarnTakenDetails.CellEnter
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim Dt1 As New DataTable
        Dim Dt2 As New DataTable
        Dim Dtloc As New DataTable
        Dim rect As Rectangle
        Dim CntID As Integer
        Dim MilID As Integer
        Dim Cns_Bg As Single, Wt_Cn As String

        With dgv_YarnTakenDetails

            If Val(.CurrentRow.Cells(0).Value) = 0 Then
                .CurrentRow.Cells(0).Value = .CurrentRow.Index + 1
            End If

            If e.ColumnIndex = 1 And dgv_YarnTakenDetails.Enabled = True Then

                If cbo_Grid_CountName.Visible = False Or Val(cbo_Grid_CountName.Tag) <> e.RowIndex Then

                    cbo_Grid_CountName.Tag = -1
                    Da = New SqlClient.SqlDataAdapter("select Count_Name from Count_Head order by Count_Name", Con)
                    Dt1 = New DataTable
                    Da.Fill(Dt1)
                    cbo_Grid_CountName.DataSource = Dt1
                    cbo_Grid_CountName.DisplayMember = "Count_Name"

                    rect = .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False)

                    cbo_Grid_CountName.Left = .Left + rect.Left
                    cbo_Grid_CountName.Top = .Top + rect.Top

                    cbo_Grid_CountName.Width = rect.Width
                    cbo_Grid_CountName.Height = rect.Height
                    cbo_Grid_CountName.Text = .CurrentCell.Value

                    cbo_Grid_CountName.Tag = Val(e.RowIndex)
                    cbo_Grid_CountName.Visible = True

                    cbo_Grid_CountName.BringToFront()
                    cbo_Grid_CountName.Focus()

                End If

            Else

                cbo_Grid_CountName.Visible = False
                cbo_Grid_CountName.Tag = -1
                cbo_Grid_CountName.Text = ""

            End If

            If e.ColumnIndex = 2 And dgv_YarnTakenDetails.Enabled = True Then

                If cbo_Grid_YarnType.Visible = False Or Val(cbo_Grid_YarnType.Tag) <> e.RowIndex Then

                    cbo_Grid_YarnType.Tag = -1
                    Da = New SqlClient.SqlDataAdapter("select Yarn_Type from YarnType_Head order by Yarn_Type", Con)
                    Dt1 = New DataTable
                    Da.Fill(Dt1)
                    cbo_Grid_YarnType.DataSource = Dt1
                    cbo_Grid_YarnType.DisplayMember = "Yarn_Type"

                    rect = .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False)

                    cbo_Grid_YarnType.Left = .Left + rect.Left
                    cbo_Grid_YarnType.Top = .Top + rect.Top

                    cbo_Grid_YarnType.Width = rect.Width
                    cbo_Grid_YarnType.Height = rect.Height
                    cbo_Grid_YarnType.Text = .CurrentCell.Value

                    cbo_Grid_YarnType.Tag = Val(e.RowIndex)
                    cbo_Grid_YarnType.Visible = True

                    cbo_Grid_YarnType.BringToFront()
                    cbo_Grid_YarnType.Focus()


                End If

            Else
                cbo_Grid_YarnType.Visible = False
                cbo_Grid_YarnType.Tag = -1
                cbo_Grid_YarnType.Text = ""

            End If

            If e.ColumnIndex = 3 And Trim(UCase(.CurrentRow.Cells(2).Value)) = "BABY" And dgv_YarnTakenDetails.Enabled = True Then

                If cbo_Grid_SetNo.Visible = False Or Val(cbo_Grid_SetNo.Tag) <> e.RowIndex Then

                    cbo_Grid_SetNo.Tag = -1
                    Da = New SqlClient.SqlDataAdapter("select distinct(Warpcode_forSelection) from Stock_BabyCone_Processing_Details order by Warpcode_forSelection", Con)
                    Dt1 = New DataTable
                    Da.Fill(Dt1)
                    cbo_Grid_SetNo.DataSource = Dt1
                    cbo_Grid_SetNo.DisplayMember = "distinct(Warpcode_forSelection)"

                    rect = .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False)

                    cbo_Grid_SetNo.Left = .Left + rect.Left
                    cbo_Grid_SetNo.Top = .Top + rect.Top

                    cbo_Grid_SetNo.Width = rect.Width
                    cbo_Grid_SetNo.Height = rect.Height
                    cbo_Grid_SetNo.Text = .CurrentCell.Value

                    cbo_Grid_SetNo.Tag = Val(e.RowIndex)
                    cbo_Grid_SetNo.Visible = True

                    cbo_Grid_SetNo.BringToFront()
                    cbo_Grid_SetNo.Focus()


                End If

            Else
                cbo_Grid_SetNo.Visible = False
                cbo_Grid_SetNo.Tag = -1
                cbo_Grid_SetNo.Text = ""

            End If

            If .CurrentCell.ColumnIndex = 4 And dgv_YarnTakenDetails.Enabled = True Then

                If cbo_Grid_MillName.Visible = False Or Val(cbo_Grid_MillName.Tag) <> e.RowIndex Then

                    cbo_Grid_MillName.Tag = -1
                    Da = New SqlClient.SqlDataAdapter("select Mill_Name from Mill_Head order by Mill_Name", Con)
                    Dt1 = New DataTable
                    Da.Fill(Dt1)
                    cbo_Grid_MillName.DataSource = Dt1
                    cbo_Grid_MillName.DisplayMember = "Mill_Name"

                    rect = .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False)

                    cbo_Grid_MillName.Left = .Left + rect.Left
                    cbo_Grid_MillName.Top = .Top + rect.Top

                    cbo_Grid_MillName.Width = rect.Width
                    cbo_Grid_MillName.Height = rect.Height
                    cbo_Grid_MillName.Text = .CurrentCell.Value

                    cbo_Grid_MillName.Tag = Val(e.RowIndex)
                    cbo_Grid_MillName.Visible = True

                    cbo_Grid_MillName.BringToFront()
                    cbo_Grid_MillName.Focus()


                End If

            Else
                cbo_Grid_MillName.Visible = False
                cbo_Grid_MillName.Tag = -1
                cbo_Grid_MillName.Text = ""

            End If


            If .CurrentCell.ColumnIndex = 11 And dgv_YarnTakenDetails.Enabled = True Then

                If cbo_Grid_Location.Visible = False Or Val(cbo_Grid_Location.Tag) <> e.RowIndex Then

                    cbo_Grid_Location.Tag = -1
                    Da = New SqlClient.SqlDataAdapter("select Ledger_DisplayName from Ledger_AlaisHead Where (Ledger_Type = 'GODOWN') and (Ledger_IdNo = 0) order by Ledger_DisplayName ", Con)
                    Dtloc = New DataTable
                    Da.Fill(Dtloc)
                    cbo_Grid_Location.DataSource = Dtloc
                    cbo_Grid_Location.DisplayMember = "Ledger_DisplayName"

                    rect = .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False)

                    cbo_Grid_Location.Left = .Left + rect.Left
                    cbo_Grid_Location.Top = .Top + rect.Top

                    cbo_Grid_Location.Width = rect.Width
                    cbo_Grid_Location.Height = rect.Height
                    cbo_Grid_Location.Text = .CurrentCell.Value

                    cbo_Grid_Location.Tag = Val(e.RowIndex)
                    cbo_Grid_Location.Visible = True

                    cbo_Grid_Location.BringToFront()
                    cbo_Grid_Location.Focus()


                End If

            Else
                cbo_Grid_Location.Visible = False
                cbo_Grid_Location.Tag = -1
                cbo_Grid_Location.Text = ""

            End If

            If .CurrentCell.ColumnIndex = 5 And Val(.CurrentRow.Cells(6).Value) = 0 And dgv_YarnTakenDetails.Enabled = True Then

                CntID = Common_Procedures.Count_NameToIdNo(Con, .CurrentRow.Cells(1).Value)
                MilID = Common_Procedures.Mill_NameToIdNo(Con, .CurrentRow.Cells(4).Value)

                If CntID <> 0 And MilID <> 0 And Trim(UCase(.CurrentRow.Cells(2).Value)) = "MILL" Then

                    Cns_Bg = 0 : Wt_Cn = 0
                    Da = New SqlClient.SqlDataAdapter("select * from Mill_Count_Details where mill_idno = " & Str(Val(MilID)) & " and count_idno = " & Str(Val(CntID)), Con)
                    Da.Fill(Dt)

                    If Dt.Rows.Count > 0 Then
                        Cns_Bg = Val(Dt.Rows(0).Item("Cones_Bag").ToString)
                        Wt_Cn = Val(Dt.Rows(0).Item("Weight_Cone").ToString)
                    End If

                    Dt.Clear()
                    Dt.Dispose()
                    Da.Dispose()



                    If Val(Wt_Cn) <> 0 Then
                        .CurrentRow.Cells(6).Value = Format(Val(Wt_Cn), "#########0.000")
                    End If


                End If

            End If

        End With

    End Sub

    Private Sub dgv_YarnTakenDetails_CellLeave(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_YarnTakenDetails.CellLeave
        With dgv_YarnTakenDetails

            If Common_Procedures.settings.CustomerCode = "1102" Then
                If .CurrentCell.ColumnIndex = 6 Or .CurrentCell.ColumnIndex = 8 Or .CurrentCell.ColumnIndex = 10 Then
                    If Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value) <> 0 Then
                        .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = Format(Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value), "#########0.000")
                    End If
                End If
            Else
                If .CurrentCell.ColumnIndex = 6 Or .CurrentCell.ColumnIndex = 8 Or .CurrentCell.ColumnIndex = 10 Then
                    If Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value) <> 0 Then
                        .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = Format(Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value), "#########0.000")
                    End If
                End If
            End If

            If .CurrentCell.ColumnIndex = 3 Then
                If Trim(.CurrentRow.Cells(2).Value) = "BABY" And Trim(.CurrentRow.Cells(3).Value) <> "" Then
                    get_BabyCone_Details(.CurrentRow.Index)
                End If
            End If

        End With
    End Sub

    Private Sub get_BabyCone_Details(ByVal CurRw As Integer)
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim CntID As Integer
        Dim NewCode As String
        Dim Ent_Bgs As Integer, Ent_Cns As Integer
        Dim Ent_Wgt As Single

        With dgv_YarnTakenDetails

            CntID = Common_Procedures.Count_NameToIdNo(Con, .Rows(CurRw).Cells(1).Value)

            If CntID <> 0 And Trim(.Rows(CurRw).Cells(3).Value) <> "" And Trim(UCase(.Rows(CurRw).Cells(2).Value)) = "BABY" Then

                NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_SetNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

                'Condt = "( " & Cmp_Cond & IIf(Trim(Cmp_Cond) <> "", " and ", "") & " a.Ledger_IdNo = " & Str(Val(Led_ID)) & " and a.Count_IdNo = " & Str(Val(Cnt_ID)) & " and (  (a.Baby_Weight - a.Delivered_Weight) > 0 or (a.Warpcode_forSelection IN (select z.BabyCone_Warpcode_forSelection from Warping_YarnTaken_Details z where z.company_idno = " & Str(Val(lbl_Company.Tag)) & " and z.Warp_Code = '" & Trim(NewCode) & "') ) ) )"

                Da = New SqlClient.SqlDataAdapter("select a.*, b.mill_name, c.bags as DelvEnt_Bags, c.cones as DelvEnt_cones, c.Weight as DelvEnt_Weight from Stock_BabyCone_Processing_Details a INNER JOIN mill_head b ON  a.mill_idno = b.mill_idno LEFT OUTER JOIN Warping_YarnTaken_Details c ON c.Warp_Code = '" & Trim(NewCode) & "' and c.yarn_type = 'BABY' and c.BabyCone_Warpcode_forSelection = a.Warpcode_forSelection where a.Warpcode_forSelection = '" & Trim(.Rows(CurRw).Cells(3).Value) & "' and a.count_idno = " & Str(Val(CntID)), Con)
                'Da = New SqlClient.SqlDataAdapter("select a.*, b.mill_name, c.bags as DelvEnt_Bags, c.cones as DelvEnt_cones, c.Weight as DelvEnt_Weight from Stock_BabyCone_Processing_Details a INNER JOIN mill_head b ON  a.mill_idno = b.mill_idno LEFT OUTER JOIN Yarn_Delivery_Details c ON c.Yarn_Delivery_Code = '" & Trim(NewCode) & "' and c.yarn_type = 'BABY' and a.Warpcode_forSelection = c.Warpcode_forSelection where a.Warpcode_forSelection = '" & Trim(.Rows(CurRw).Cells(3).Value) & "' and a.count_idno = " & Str(Val(CntID)), Con)
                Da.Fill(Dt)

                If Dt.Rows.Count > 0 Then

                    Ent_Bgs = 0 : Ent_Cns = 0 : Ent_Wgt = 0

                    If IsDBNull(Dt.Rows(0).Item("DelvEnt_Bags").ToString) = False Then Ent_Bgs = Val(Dt.Rows(0).Item("DelvEnt_Bags").ToString)
                    If IsDBNull(Dt.Rows(0).Item("DelvEnt_cones").ToString) = False Then Ent_Cns = Val(Dt.Rows(0).Item("DelvEnt_cones").ToString)
                    If IsDBNull(Dt.Rows(0).Item("DelvEnt_Weight").ToString) = False Then Ent_Wgt = Val(Dt.Rows(0).Item("DelvEnt_Weight").ToString)

                    .Rows(CurRw).Cells(4).Value = Dt.Rows(0).Item("Mill_Name").ToString
                    .Rows(CurRw).Cells(5).Value = Format(Val(Dt.Rows(0).Item("Baby_Bags").ToString) - Val(Dt.Rows(0).Item("Delivered_Bags").ToString) + Ent_Bgs, "#########0.000")
                    .Rows(CurRw).Cells(7).Value = Format(Val(Dt.Rows(0).Item("Baby_Cones").ToString) - Val(Dt.Rows(0).Item("Delivered_Cones").ToString) + Ent_Cns, "#########0.000")
                    .Rows(CurRw).Cells(8).Value = Format(Val(Dt.Rows(0).Item("Baby_Weight").ToString) - Val(Dt.Rows(0).Item("Delivered_Weight").ToString) + Ent_Wgt, "#########0.000")
                    .Rows(CurRw).Cells(6).Value = Format(Val(.Rows(CurRw).Cells(8).Value) / Val(.Rows(CurRw).Cells(7).Value), "#########0.000")

                    TotalYarnTaken_Calculation()

                End If

                Dt.Clear()
                Dt.Dispose()
                Da.Dispose()

            End If
        End With
    End Sub

    Private Sub dgv_YarnTakenDetails_CellValueChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_YarnTakenDetails.CellValueChanged
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim Dt1 As New DataTable
        Dim CntID As Integer
        Dim MilID As Integer
        Dim Cns_Bg As Single, Wt_Cn As String, Em_CnWgt As Single, Em_BgWgt As Single, Tot_TrWgt As Single

        On Error Resume Next

        With dgv_YarnTakenDetails

            If .Visible Then
                CntID = Common_Procedures.Count_NameToIdNo(Con, .CurrentRow.Cells(1).Value)
                MilID = Common_Procedures.Mill_NameToIdNo(Con, .CurrentRow.Cells(4).Value)

                Em_CnWgt = 0 : Em_BgWgt = 0
                If MilID <> 0 And Trim(UCase(.CurrentRow.Cells(2).Value)) <> "MILL" Then

                    Da = New SqlClient.SqlDataAdapter("select * from Mill_hEAD where mill_idno = " & Str(Val(MilID)), Con)
                    Da.Fill(Dt1)

                    If Dt1.Rows.Count > 0 Then
                        Em_CnWgt = Val(Dt1.Rows(0).Item("Weight_EmptyCone").ToString)
                        Em_BgWgt = Val(Dt1.Rows(0).Item("Weight_EmptyBag").ToString)

                    End If

                    Dt1.Clear()
                End If

                If e.ColumnIndex = 5 Then

                    If CntID <> 0 And MilID <> 0 And Trim(UCase(.CurrentRow.Cells(2).Value)) = "MILL" Then

                        Cns_Bg = 0 : Wt_Cn = 0
                        Da = New SqlClient.SqlDataAdapter("select * from Mill_Count_Details where mill_idno = " & Str(Val(MilID)) & " and count_idno = " & Str(Val(CntID)), Con)
                        Da.Fill(Dt)

                        If Dt.Rows.Count > 0 Then
                            Cns_Bg = Val(Dt.Rows(0).Item("Cones_Bag").ToString)
                            Wt_Cn = Val(Dt.Rows(0).Item("Weight_Cone").ToString)
                        End If

                        Dt.Clear()

                        If Val(Cns_Bg) <> 0 Then
                            .CurrentRow.Cells(7).Value = Val(.CurrentRow.Cells(5).Value) * Val(Cns_Bg)
                        End If
                        If Val(Wt_Cn) <> 0 Then
                            .CurrentRow.Cells(6).Value = Format(Val(Wt_Cn), "#########0.000")
                            .CurrentRow.Cells(10).Value = Format(Val(.CurrentRow.Cells(7).Value) * Val(Wt_Cn), "#########0.000")
                        End If

                    End If

                End If

                If e.ColumnIndex = 5 Or e.ColumnIndex = 6 Or e.ColumnIndex = 7 Or e.ColumnIndex = 8 Or e.ColumnIndex = 9 Or e.ColumnIndex = 10 Then
                    If e.ColumnIndex = 6 Or e.ColumnIndex = 7 Then
                        If Val(.CurrentRow.Cells(6).Value) <> 0 And Val(.CurrentRow.Cells(7).Value) <> 0 Then
                            .CurrentRow.Cells(8).Value = Format((Val(.CurrentRow.Cells(6).Value) * Val(.CurrentRow.Cells(7).Value)) + Val(.CurrentRow.Cells(9).Value), "#########0.0")
                            '.CurrentRow.Cells(10).Value = Format(Val(.CurrentRow.Cells(6).Value) * Val(.CurrentRow.Cells(7).Value), "#########0.0")
                        End If
                    End If
                    Tot_TrWgt = 0

                    If e.ColumnIndex = 5 Or e.ColumnIndex = 7 Then
                        If Val(Em_BgWgt) <> 0 Or Val(Em_CnWgt) <> 0 Then
                            Tot_TrWgt = Format(Val(.CurrentRow.Cells(5).Value) * Val(Em_BgWgt), "#########0.0")
                            Tot_TrWgt = Tot_TrWgt + Format(Val(.CurrentRow.Cells(7).Value) * Val(Em_CnWgt), "#########0.0")
                            .CurrentRow.Cells(9).Value = Format(Val(Tot_TrWgt), "#########0.0")
                        End If
                    End If

                    If e.ColumnIndex = 8 Or e.ColumnIndex = 9 Then
                        .CurrentRow.Cells(10).Value = Format(Val(.CurrentRow.Cells(8).Value) - Val(.CurrentRow.Cells(9).Value), "#########0.0")
                    End If


                    TotalYarnTaken_Calculation()
                End If
            End If
        End With
    End Sub

    Private Sub dgv_YarnTakenDetails_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_YarnTakenDetails.KeyUp
        Dim i As Integer

        If e.Control = True And UCase(Chr(e.KeyCode)) = "D" Then

            With dgv_YarnTakenDetails

                If .CurrentRow.Index = .RowCount - 1 Then
                    For i = 1 To .Columns.Count - 1
                        .Rows(.CurrentRow.Index).Cells(i).Value = ""
                    Next

                Else
                    .Rows.RemoveAt(.CurrentRow.Index)

                End If

                TotalYarnTaken_Calculation()

            End With
        End If

    End Sub

    Private Sub dgv_YarnTakenDetails_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgv_YarnTakenDetails.LostFocus
        On Error Resume Next
        dgv_YarnTakenDetails.CurrentCell.Selected = False
    End Sub

    Private Sub dgv_YarnTakenDetails_RowsAdded(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewRowsAddedEventArgs) Handles dgv_YarnTakenDetails.RowsAdded
        Dim n As Integer

        With dgv_YarnTakenDetails
            n = .RowCount
            .Rows(n - 1).Cells(0).Value = Val(n)
        End With
    End Sub

    Private Sub TotalYarnTaken_Calculation()
        Dim Sno As Integer
        Dim TotBags As Single, TotCones As Single, TotWeight As Single, TotGrsWeight As Single, TotTrWeight As Single

        Sno = 0
        TotBags = 0
        TotCones = 0
        TotWeight = 0
        TotGrsWeight = 0
        TotTrWeight = 0

        With dgv_YarnTakenDetails
            For i = 0 To .RowCount - 1
                Sno = Sno + 1
                .Rows(i).Cells(0).Value = Sno
                If Val(.Rows(i).Cells(10).Value) <> 0 Then
                    TotBags = TotBags + Val(.Rows(i).Cells(5).Value)
                    TotCones = TotCones + Val(.Rows(i).Cells(7).Value)
                    TotGrsWeight = TotGrsWeight + Val(.Rows(i).Cells(8).Value)
                    TotTrWeight = TotTrWeight + Val(.Rows(i).Cells(9).Value)
                    TotWeight = TotWeight + Val(.Rows(i).Cells(10).Value)
                End If
            Next
        End With

        With dgv_YarnTakenDetails_Total
            If .RowCount = 0 Then .Rows.Add()
            .Rows(0).Cells(5).Value = Val(TotBags)
            .Rows(0).Cells(7).Value = Val(TotCones)
            .Rows(0).Cells(8).Value = Format(Val(TotGrsWeight), "########0.0")
            .Rows(0).Cells(9).Value = Format(Val(TotTrWeight), "########0.0")
            .Rows(0).Cells(10).Value = Format(Val(TotWeight), "########0.0")
        End With

        lbl_YarnTaken.Text = Format(Val(TotWeight), "#########0.0")

    End Sub

    Private Sub dgv_BabyConeDetails_CellEndEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_BabyConeDetails.CellEndEdit
        dgv_BabyConeDetails_CellLeave(sender, e)
    End Sub

    Private Sub dgv_BabyConeDetails_CellEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_BabyConeDetails.CellEnter
        With dgv_BabyConeDetails
            If Val(.CurrentRow.Cells(0).Value) = 0 Then
                .CurrentRow.Cells(0).Value = .CurrentRow.Index + 1
            End If
        End With
    End Sub

    Private Sub dgv_BabyConeDetails_CellLeave(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_BabyConeDetails.CellLeave
        With dgv_BabyConeDetails
            If .CurrentCell.ColumnIndex = 3 Or .CurrentCell.ColumnIndex = 4 Or .CurrentCell.ColumnIndex = 5 Then
                If Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value) <> 0 Then
                    If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1102" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1144" Then
                        .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = Format(Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value), "#########0.0")
                    Else
                        .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = Format(Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value), "#########0.000")

                    End If
                End If
            End If
        End With
    End Sub

    Private Sub dgv_BabyConeDetails_CellValueChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_BabyConeDetails.CellValueChanged
        On Error Resume Next
        If IsNothing(dgv_BabyConeDetails.CurrentCell) Then Exit Sub
        With dgv_BabyConeDetails
            If .Visible Then
                If .CurrentCell.ColumnIndex = 1 Or .CurrentCell.ColumnIndex = 2 Or .CurrentCell.ColumnIndex = 3 Then
                    Total_BabyCone_Calculation()
                End If
            End If
        End With
    End Sub

    Private Sub dgv_BabyConeDetails_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_BabyConeDetails.KeyUp
        Dim i As Integer

        If e.Control = True And UCase(Chr(e.KeyCode)) = "D" Then

            With dgv_BabyConeDetails

                If Val(txt_BabyCone_DeliveryWeight.Text) = 0 Then

                    If .CurrentRow.Index = .RowCount - 1 Then
                        For i = 1 To .Columns.Count - 1
                            .Rows(.CurrentRow.Index).Cells(i).Value = ""
                        Next

                    Else
                        .Rows.RemoveAt(.CurrentRow.Index)

                    End If

                    Total_BabyCone_Calculation()


                Else
                    MessageBox.Show("BabyCone delivered", "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                    Exit Sub

                End If

            End With

        End If

    End Sub

    Private Sub dgv_BabyConeDetails_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgv_BabyConeDetails.LostFocus
        On Error Resume Next
        If Not IsNothing(dgv_BabyConeDetails.CurrentCell) Then dgv_BabyConeDetails.CurrentCell.Selected = False
    End Sub

    Private Sub dgv_BabyConeDetails_RowsAdded(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewRowsAddedEventArgs) Handles dgv_BabyConeDetails.RowsAdded
        Dim n As Integer
        If IsNothing(dgv_BabyConeDetails.CurrentCell) Then Exit Sub
        With dgv_BabyConeDetails
            n = .RowCount
            .Rows(n - 1).Cells(0).Value = Val(n)
        End With
    End Sub

    Private Sub Total_BabyCone_Calculation()
        Dim Sno As Integer
        Dim TotBags As Single, TotCones As Single, TotWeight As Single

        Sno = 0
        TotBags = 0
        TotCones = 0
        TotWeight = 0
        With dgv_BabyConeDetails
            For i = 0 To .RowCount - 1
                Sno = Sno + 1
                .Rows(i).Cells(0).Value = Sno
                If Val(.Rows(i).Cells(3).Value) <> 0 Then
                    TotBags = TotBags + Val(.Rows(i).Cells(1).Value)
                    TotCones = TotCones + Val(.Rows(i).Cells(2).Value)
                    TotWeight = TotWeight + Val(.Rows(i).Cells(3).Value)
                End If
            Next
        End With

        With dgv_BabyConeDetails_Total
            If .RowCount = 0 Then .Rows.Add()
            .Rows(0).Cells(1).Value = Val(TotBags)
            .Rows(0).Cells(2).Value = Val(TotCones)
            .Rows(0).Cells(3).Value = Format(Val(TotWeight), "########0.0")
        End With

        BabyCone_TareWeight_Calculation(0)

        BabyCone_NetWeight_Calculation()

    End Sub

    Private Sub BabyCone_NetWeight_Calculation()
        Dim Sno As Integer
        Dim TotWgt As Integer
        Dim TotBags As Single, TotCones As Single, TotWeight As Single

        Sno = 0
        TotWgt = 0
        TotBags = 0
        TotCones = 0
        TotWeight = 0

        With dgv_BabyConeDetails_Total
            If .RowCount = 0 Then .Rows.Add()
            TotWeight = .Rows(0).Cells(3).Value
        End With

        lbl_BabyCone_NetWeight.Text = Format(Val(TotWeight) - Val(txt_BabyCone_TareWeight.Text) + Val(txt_BabyCone_AddLessWgt.Text), "########0.0")

        lbl_BabyConeWeight.Text = Format(Val(lbl_BabyCone_NetWeight.Text), "#########0.0")

        TotWgt = Val(lbl_BabyCone_NetWeight.Text)

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1102" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1144" Then

            If TotWgt < 100 Then
                TotWgt = TotWgt - 2
            Else
                TotWgt = TotWgt - 3
            End If

            lbl_BabyConeWeight.Text = Format(Val(TotWgt), "#########0")

        End If

        txt_RewindingCones.Text = ""
        If chk_RewindingStatus.Checked = True Then

            If Trim(Common_Procedures.settings.CustomerCode) = "1288" Then
                txt_RewindingCones.Text = Format(Val(lbl_BabyConeWeight.Text) / 1.9, "#########0")
            Else
                txt_RewindingCones.Text = Format(Val(lbl_BabyConeWeight.Text) / 1.5, "#########0")
            End If


        End If

    End Sub




    Private Sub cbo_Grid_CountName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Grid_CountName.GotFocus
        Dim Cnt_ID As Integer = 0
        Dim CountCondt As String = ""

        Cnt_ID = Common_Procedures.Count_NameToIdNo(Con, cbo_CountName.Text)

        CountCondt = ""
        If dgv_YarnTakenDetails.Rows.Count > 0 Then
            If Val(cbo_Grid_CountName.Tag) <= 0 Then
                CountCondt = "(Count_IdNo = " & Str(Val(Cnt_ID)) & ")"
            End If
        End If

        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, Con, "Count_Head", "Count_Name", CountCondt, "(Count_IdNo = 0)")

    End Sub

    Private Sub cbo_Grid_CountName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Grid_CountName.KeyDown
        Dim Cnt_ID As Integer = 0
        Dim CountCondt As String = ""

        Cnt_ID = Common_Procedures.Count_NameToIdNo(Con, cbo_CountName.Text)

        CountCondt = ""
        If dgv_YarnTakenDetails.Rows.Count > 0 Then
            If Val(cbo_Grid_CountName.Tag) <= 0 Then
                CountCondt = "(Count_IdNo = " & Str(Val(Cnt_ID)) & ")"
            End If
        End If

        vcbo_KeyDwnVal = e.KeyValue

        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, Con, cbo_Grid_CountName, Nothing, Nothing, "Count_Head", "Count_Name", CountCondt, "(Count_IdNo = 0)")

        With dgv_YarnTakenDetails

            If (e.KeyValue = 38 And cbo_Grid_CountName.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then

                If Val(.CurrentCell.RowIndex) <= 0 Then
                    tab_Main.SelectTab(0)
                    ' tab_yzingDetails.SelectTab(0)
                    'dgv_SizingDetails_Set1.Focus()
                    'dgv_SizingDetails_Set1.CurrentCell = dgv_SizingDetails_Set1.Rows(0).Cells(1)
                    'dgv_SizingDetails_Set1.CurrentCell.Selected = True

                Else
                    .Focus()
                    .CurrentCell = .Rows(.CurrentCell.RowIndex - 1).Cells(8)
                    .CurrentCell.Selected = True

                End If
            End If

            If (e.KeyValue = 40 And cbo_Grid_CountName.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then

                If .CurrentCell.RowIndex = .RowCount - 1 And .CurrentCell.ColumnIndex >= 1 And Trim(.CurrentRow.Cells(1).Value) = "" Then
                    tab_Main.SelectTab(2)
                    'dgv_BabyConeDetails.Focus()
                    'dgv_BabyConeDetails.CurrentCell = dgv_BabyConeDetails.Rows(0).Cells(1)
                    'dgv_BabyConeDetails.CurrentCell.Selected = True

                Else
                    .Focus()
                    .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)
                    .CurrentCell.Selected = True

                End If

            End If

        End With

    End Sub

    Private Sub cbo_Grid_CountName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Grid_CountName.KeyPress
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim Cnt_ID As Integer = 0
        Dim CountCondt As String = ""

        Cnt_ID = Common_Procedures.Count_NameToIdNo(Con, cbo_CountName.Text)

        CountCondt = ""
        If dgv_YarnTakenDetails.Rows.Count > 0 Then
            If Val(cbo_Grid_CountName.Tag) <= 0 Then
                CountCondt = "(Count_IdNo = " & Str(Val(Cnt_ID)) & ")"
            End If
        End If

        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, Con, cbo_Grid_CountName, Nothing, "Count_Head", "Count_Name", CountCondt, "(Count_IdNo = 0)")

        If Asc(e.KeyChar) = 13 Then

            With dgv_YarnTakenDetails

                .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_Grid_CountName.Text)
                If .CurrentCell.RowIndex = .RowCount - 1 And .CurrentCell.ColumnIndex >= 1 And Trim(.CurrentRow.Cells(1).Value) = "" Then
                    tab_Main.SelectTab(2)
                    'dgv_BabyConeDetails.Focus()
                    'dgv_BabyConeDetails.CurrentCell = dgv_BabyConeDetails.Rows(0).Cells(1)
                    'dgv_BabyConeDetails.CurrentCell.Selected = True

                Else
                    .Focus()
                    .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)
                    .CurrentCell.Selected = True

                End If
            End With

        End If

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

                If IsNothing(dgv_YarnTakenDetails.CurrentCell) Then Exit Sub
                With dgv_YarnTakenDetails
                    If Val(cbo_Grid_CountName.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = 1 Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_Grid_CountName.Text)
                    End If
                End With
            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub cbo_Grid_MillName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Grid_MillName.GotFocus
        Dim Mil_ID As Integer = 0
        Dim Mill_Condt As String = ""

        Mil_ID = Common_Procedures.Mill_NameToIdNo(Con, cbo_MillName.Text)

        Mill_Condt = ""
        If dgv_YarnTakenDetails.Rows.Count > 0 Then
            If dgv_YarnTakenDetails.CurrentCell.RowIndex = 0 Then
                Mill_Condt = "(Mill_IdNo = " & Str(Val(Mil_ID)) & ")"
            End If
        End If

        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, Con, "Mill_Head", "Mill_Name", Mill_Condt, "(Mill_IdNo = 0)")

    End Sub



    Private Sub cbo_Grid_Location_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Grid_Location.GotFocus


        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, Con, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'GODOWN')", "(Ledger_IdNo = 0)")

    End Sub




    Private Sub cbo_Grid_MillName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Grid_MillName.KeyPress
        Dim Mil_ID As Integer = 0
        Dim Mill_Condt As String = ""

        Mil_ID = Common_Procedures.Mill_NameToIdNo(Con, cbo_MillName.Text)

        Mill_Condt = ""
        If dgv_YarnTakenDetails.Rows.Count > 0 Then
            If dgv_YarnTakenDetails.CurrentCell.RowIndex = 0 Then
                Mill_Condt = "(Mill_IdNo = " & Str(Val(Mil_ID)) & ")"
            End If
        End If

        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, Con, cbo_Grid_MillName, Nothing, "Mill_Head", "Mill_Name", Mill_Condt, "(Mill_IdNo = 0)")

        If Asc(e.KeyChar) = 13 Then
            With dgv_YarnTakenDetails
                If .Rows.Count > 0 Then
                    .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_Grid_MillName.Text)
                    .Focus()
                    .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)
                    .CurrentCell.Selected = True
                End If
            End With
        End If
    End Sub


    Private Sub cbo_Grid_Location_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Grid_Location.KeyPress

        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, Con, cbo_Grid_Location, Nothing, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'GODOWN')", "(Ledger_IdNo = 0)")

        If Asc(e.KeyChar) = 13 Then
            With dgv_YarnTakenDetails
                If .Rows.Count > 0 Then
                    .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_Grid_Location.Text)
                    .Focus()
                    .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)
                    .CurrentCell.Selected = True
                End If
            End With
        End If
    End Sub

    Private Sub cbo_Grid_MillName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Grid_MillName.KeyDown
        Dim dep_idno As Integer = 0
        Dim Mil_ID As Integer = 0
        Dim Mill_Condt As String = ""

        Mil_ID = Common_Procedures.Mill_NameToIdNo(Con, cbo_MillName.Text)

        Mill_Condt = ""
        If dgv_YarnTakenDetails.Rows.Count > 0 Then
            If dgv_YarnTakenDetails.CurrentCell.RowIndex = 0 Then
                Mill_Condt = "(Mill_IdNo = " & Str(Val(Mil_ID)) & ")"
            End If
        End If

        vcbo_KeyDwnVal = e.KeyValue

        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, Con, cbo_Grid_MillName, Nothing, Nothing, "Mill_Head", "Mill_Name", Mill_Condt, "(Mill_IdNo = 0)")

        With dgv_YarnTakenDetails

            If (e.KeyValue = 38 And cbo_Grid_MillName.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
                If .Rows.Count > 0 Then
                    .Focus()
                    .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex - 1)
                    .CurrentCell.Selected = True
                End If
            End If

            If (e.KeyValue = 40 And cbo_Grid_MillName.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
                If .Rows.Count > 0 Then
                    .Focus()
                    .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)
                    .CurrentCell.Selected = True
                End If
            End If

        End With

    End Sub

    Private Sub cbo_Grid_Location_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Grid_Location.KeyDown
        Dim dep_idno As Integer = 0
        Dim Mil_ID As Integer = 0
        Dim Mill_Condt As String = ""

        vcbo_KeyDwnVal = e.KeyValue

        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, Con, cbo_Grid_Location, Nothing, Nothing, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'GODOWN')", "(Ledger_IdNo = 0)")

        With dgv_YarnTakenDetails

            If (e.KeyValue = 38 And cbo_Grid_Location.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
                If .Rows.Count > 0 Then
                    .Focus()
                    .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex - 1)
                    .CurrentCell.Selected = True
                End If
            End If

            If (e.KeyValue = 40 And cbo_Grid_Location.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
                If .Rows.Count > 0 Then
                    .Focus()
                    .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)
                    .CurrentCell.Selected = True
                End If
            End If

        End With

    End Sub

    Private Sub get_MillCount_Details()
        Dim q As Single = 0
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim Cn_bag As String
        Dim Wgt_Bag As String
        Dim Wgt_Cn As String
        Dim CntID As Integer
        Dim MilID As Integer

        CntID = Common_Procedures.Count_NameToIdNo(Con, dgv_YarnTakenDetails.Rows(dgv_YarnTakenDetails.CurrentRow.Index).Cells(1).Value)
        MilID = Common_Procedures.Mill_NameToIdNo(Con, dgv_YarnTakenDetails.Rows(dgv_YarnTakenDetails.CurrentRow.Index).Cells(4).Value)

        Wgt_Bag = 0 : Wgt_Cn = 0 : Cn_bag = 0

        If CntID <> 0 And MilID <> 0 Then

            Da = New SqlClient.SqlDataAdapter("select * from Mill_Count_Details where mill_idno = " & Str(Val(MilID)) & " and count_idno = " & Str(Val(CntID)), Con)
            Da.Fill(Dt)
            With dgv_YarnTakenDetails

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

                If .CurrentCell.ColumnIndex = 5 Or .CurrentCell.ColumnIndex = 5 Then
                    If .CurrentCell.ColumnIndex = 5 Then
                        If Val(Cn_bag) <> 0 Then
                            .Rows(.CurrentRow.Index).Cells(7).Value = Val(.Rows(.CurrentRow.Index).Cells(5).Value) * Val(Cn_bag)
                        End If

                        If Val(Wgt_Bag) <> 0 Then
                            .Rows(.CurrentRow.Index).Cells(8).Value = Format(Val(.Rows(.CurrentRow.Index).Cells(5).Value) * Val(Wgt_Bag), "#########0.000")
                        End If

                    End If

                    If .CurrentCell.ColumnIndex = 7 Then
                        If Val(Wgt_Cn) <> 0 Then
                            .Rows(.CurrentRow.Index).Cells(8).Value = Format(Val(.Rows(.CurrentRow.Index).Cells(7).Value) * Val(Wgt_Cn), "##########0.000")
                        End If

                    End If

                End If

            End With

        End If

    End Sub
    Private Sub cbo_Grid_MillName_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Grid_MillName.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Dim f As New Mill_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Grid_MillName.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If
    End Sub

    Private Sub cbo_Grid_Location_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Grid_Location.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Dim f As New Ledger_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Grid_Location.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If
    End Sub

    Private Sub cbo_Grid_MillName_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Grid_MillName.TextChanged
        Try
            If cbo_Grid_MillName.Visible Then

                If IsNothing(dgv_YarnTakenDetails.CurrentCell) Then Exit Sub
                With dgv_YarnTakenDetails
                    If Val(cbo_Grid_MillName.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = 4 Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_Grid_MillName.Text)
                    End If
                End With
            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub cbo_Grid_Location_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Grid_Location.TextChanged
        Try
            If cbo_Grid_Location.Visible Then

                If IsNothing(dgv_YarnTakenDetails.CurrentCell) Then Exit Sub
                With dgv_YarnTakenDetails
                    If Val(cbo_Grid_Location.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = 11 Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_Grid_Location.Text)
                    End If
                End With
            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub cbo_Grid_YarnType_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Grid_YarnType.KeyDown
        Dim dep_idno As Integer = 0

        vcbo_KeyDwnVal = e.KeyValue

        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, Con, cbo_Grid_YarnType, Nothing, Nothing, "YarnType_Head", "Yarn_Type", "", "(Yarn_Type = '')")

        With dgv_YarnTakenDetails

            If (e.KeyValue = 38 And cbo_Grid_YarnType.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
                .Focus()
                .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex - 1)
                .CurrentCell.Selected = True
            End If

            If (e.KeyValue = 40 And cbo_Grid_YarnType.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
                .Focus()
                .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)
                .CurrentCell.Selected = True
            End If

        End With

    End Sub

    Private Sub cbo_Grid_YarnType_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Grid_YarnType.KeyPress

        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, Con, cbo_Grid_YarnType, Nothing, "YarnType_Head", "Yarn_Type", "", "(Yarn_Type = '')")

        If Asc(e.KeyChar) = 13 Then
            With dgv_YarnTakenDetails
                .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_Grid_YarnType.Text)
                .Focus()
                .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)
                .CurrentCell.Selected = True
            End With
        End If

    End Sub

    Private Sub cbo_Grid_YarnType_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Grid_YarnType.GotFocus
        With cbo_Grid_YarnType
            If Trim(.Text) = "" Then .Text = "MILL"
            Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, Con, "YarnType_Head", "Yarn_Type", "", "(Yarn_Type = '')")
        End With
    End Sub

    Private Sub cbo_Grid_YarnType_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Grid_YarnType.TextChanged
        Try
            If cbo_Grid_YarnType.Visible Then

                If IsNothing(dgv_YarnTakenDetails.CurrentCell) Then Exit Sub
                With dgv_YarnTakenDetails
                    If Val(cbo_Grid_YarnType.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = 2 Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_Grid_YarnType.Text)
                    End If
                End With
            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub txt_Ends_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Ends.KeyPress
        If Asc(e.KeyChar) = 13 Then
            tab_Main.SelectTab(0)
            tab_WarpingDeatils.SelectTab(0)
            If dgv_WarpingDetails_Set1.Rows.Count <= 0 Then dgv_WarpingDetails_Set1.Rows.Add()
            dgv_WarpingDetails_Set1.Focus()
            dgv_WarpingDetails_Set1.CurrentCell = dgv_WarpingDetails_Set1.Rows(0).Cells(1)
            dgv_WarpingDetails_Set1.CurrentCell.Selected = True
        End If
    End Sub

    Private Sub txt_WarpMeters_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_WarpMeters.KeyDown
        If e.KeyCode = 38 Then SendKeys.Send("+{TAB}")
        If e.KeyCode = 40 Then
            tab_Main.SelectTab(0)
            tab_WarpingDeatils.SelectTab(0)
            If dgv_WarpingDetails_Set1.Rows.Count <= 0 Then dgv_WarpingDetails_Set1.Rows.Add()
            dgv_WarpingDetails_Set1.Focus()
            dgv_WarpingDetails_Set1.CurrentCell = dgv_WarpingDetails_Set1.Rows(0).Cells(1)
            dgv_WarpingDetails_Set1.CurrentCell.Selected = True
        End If
    End Sub

    Private Sub txt_ExcessShort_AddLess_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_ExcessShort_AddLess.KeyDown
        If e.KeyCode = 38 Then
            chk_RewindingStatus.Focus()

        End If

        If e.KeyCode = 40 Then
            txt_Remarks.Focus()
        End If
    End Sub

    Private Sub txt_ExcessShort_AddLess_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_ExcessShort_AddLess.KeyPress
        If Asc(e.KeyChar) = 13 Then txt_Remarks.Focus()
    End Sub

    Private Sub txt_Remarks_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_Remarks.KeyDown
        If e.KeyCode = 38 Then
            tab_Main.SelectTab(0)
            tab_WarpingDeatils.SelectTab(0)
            If dgv_WarpingDetails_Set1.Rows.Count <= 0 Then dgv_WarpingDetails_Set1.Rows.Add()
            dgv_WarpingDetails_Set1.Focus()
            dgv_WarpingDetails_Set1.CurrentCell = dgv_WarpingDetails_Set1.Rows(0).Cells(1)
            dgv_WarpingDetails_Set1.CurrentCell.Selected = True

        End If

        If e.KeyCode = 40 Then
            If cbo_Ledger.Enabled And cbo_Ledger.Visible Then cbo_Ledger.Focus() Else dtp_Date.Focus()
        End If

    End Sub

    Private Sub txt_Remarks_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Remarks.KeyPress
        If Asc(e.KeyChar) = 13 Then
            If MessageBox.Show("Do you want to save ?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = DialogResult.Yes Then
                save_record()
            Else
                If cbo_Ledger.Enabled And cbo_Ledger.Visible Then cbo_Ledger.Focus()
            End If
        End If

    End Sub
    Private Sub txt_BabyCone_AddLessWgt_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_BabyCone_AddLessWgt.KeyDown
        If e.KeyCode = 40 Then chk_RewindingStatus.Focus() ' SendKeys.Send("{TAB}")
        If e.KeyCode = 38 Then
            txt_BabyCone_TareWeight.Focus()
            'dgv_BabyConeDetails.Focus()
            'dgv_BabyConeDetails.CurrentCell = dgv_BabyConeDetails.Rows(0).Cells(1)
            'dgv_BabyConeDetails.CurrentCell.Selected = True
        End If
    End Sub

    Private Sub txt_BabyCone_AddLessWgt_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_BabyCone_AddLessWgt.KeyPress
        If Asc(e.KeyChar) = 13 Then chk_RewindingStatus.Focus() '  SendKeys.Send("{TAB}")
    End Sub

    Private Sub txt_BabyCone_TareWeight_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_BabyCone_TareWeight.KeyDown
        If e.KeyCode = 40 Then txt_BabyCone_AddLessWgt.Focus() ' SendKeys.Send("{TAB}")
        If e.KeyCode = 38 Then
            SendKeys.Send("+{Tab}")
            'dgv_BabyConeDetails.Focus()
            'dgv_BabyConeDetails.CurrentCell = dgv_BabyConeDetails.Rows(0).Cells(1)
            'dgv_BabyConeDetails.CurrentCell.Selected = True
        End If
    End Sub

    Private Sub txt_BabyCone_TareWeight_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_BabyCone_TareWeight.KeyPress
        If Asc(e.KeyChar) = 13 Then txt_BabyCone_AddLessWgt.Focus() '  SendKeys.Send("{TAB}")
    End Sub

    Private Sub cbo_Location_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Location.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, Con, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'GODOWN')", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_DeliveryTo_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_DeliveryTo.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, Con, "Ledger_AlaisHead", "Ledger_DisplayName", "(AccountsGroup_IdNo = 10 and Close_Status = 0 )", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_Rw_MillName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Rw_MillName.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, Con, "Mill_Head", "Mill_Name", "", "(Mill_IdNo = 0)")
    End Sub


    Private Sub cbo_Rw_MillName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Rw_MillName.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, Con, cbo_Rw_MillName, txt_RewindingCones, Nothing, "Mill_Head", "Mill_Name", "", "(Mill_IdNo = 0)")

        If (e.KeyValue = 40 And cbo_Rw_MillName.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then

            If txt_RwExcSht.Visible = True And txt_RwExcSht.Enabled = True Then
                txt_RwExcSht.Focus()
            Else
                txt_Remarks.Focus()
            End If



        End If

    End Sub



    Private Sub cbo_DeliveryTo_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_DeliveryTo.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, Con, cbo_DeliveryTo, txt_RewindingCones, Nothing, "Ledger_AlaisHead", "Ledger_DisplayName", "(AccountsGroup_IdNo = 10 and Close_Status = 0 )", "(Ledger_IdNo = 0)")

        If (e.KeyValue = 40 And cbo_DeliveryTo.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then

            cbo_Location.Focus()

        End If

    End Sub

    Private Sub cbo_Location_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Location.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, Con, cbo_Location, Nothing, Nothing, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'GODOWN')", "(Ledger_IdNo = 0)")

        If (e.KeyValue = 40 And cbo_Location.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then

            If txt_RwExcSht.Visible = True And txt_RwExcSht.Enabled = True Then
                txt_RwExcSht.Focus()
            Else
                txt_Remarks.Focus()
            End If



        End If

    End Sub


    Private Sub cbo_DeliveryTo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_DeliveryTo.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, Con, cbo_DeliveryTo, Nothing, "Ledger_AlaisHead", "Ledger_DisplayName", "(AccountsGroup_IdNo = 10 and Close_Status = 0 )", "(Ledger_IdNo = 0)")
        If Asc(e.KeyChar) = 13 Then
            cbo_Location.Focus()

        End If
    End Sub

    Private Sub cbo_Rw_MillName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Rw_MillName.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, Con, cbo_Rw_MillName, Nothing, "Mill_Head", "Mill_Name", "", "(Mill_IdNo = 0)")
        If Asc(e.KeyChar) = 13 Then

            If txt_RwExcSht.Visible = True And txt_RwExcSht.Enabled = True Then
                txt_RwExcSht.Focus()
            Else
                txt_Remarks.Focus()
            End If



        End If
    End Sub


    Private Sub cbo_Location_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Location.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, Con, cbo_Rw_MillName, Nothing, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'GODOWN')", "(Ledger_IdNo = 0)")
        If Asc(e.KeyChar) = 13 Then

            If txt_RwExcSht.Visible = True And txt_RwExcSht.Enabled = True Then
                txt_RwExcSht.Focus()
            Else
                txt_Remarks.Focus()
            End If



        End If
    End Sub

    Private Sub cbo_Rw_MillName_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Rw_MillName.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Dim f As New Mill_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Rw_MillName.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If

    End Sub

    Private Sub cbo_DeliveryTo_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_DeliveryTo.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Common_Procedures.UR.Ledr_Wea_Siz_Rw_Trans_JbWrk_Creation = Common_Procedures.UR.Ledger_Creation
            Common_Procedures.MDI_LedType = ""
            Dim f As New Ledger_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_DeliveryTo.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If

    End Sub

    Private Sub cbo_Location_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Location.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Common_Procedures.UR.Ledr_Wea_Siz_Rw_Trans_JbWrk_Creation = Common_Procedures.UR.Ledger_Creation
            Common_Procedures.MDI_LedType = ""            
            Dim f As New Ledger_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Location.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If

    End Sub






    Private Sub chk_RewindingStatus_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chk_RewindingStatus.CheckedChanged
        If chk_RewindingStatus.Checked = True Then
            txt_RewindingCones.Enabled = True
            cbo_Rw_MillName.Enabled = True
            Total_BabyCone_Calculation()
        Else
            txt_RewindingCones.Text = ""
            txt_RewindingCones.Enabled = False
            cbo_Rw_MillName.Enabled = False
        End If
    End Sub

    Private Sub chk_RewindingStatus_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles chk_RewindingStatus.KeyDown
        If e.KeyCode = 40 Then
            If chk_RewindingStatus.Checked = True Then
                txt_RewindingCones.Focus()

            ElseIf txt_RwExcSht.Enabled = True And txt_RwExcSht.Visible = True Then
                txt_RwExcSht.Focus()

            Else
                txt_ExcessShort_AddLess.Focus()

            End If
        End If
        If e.KeyCode = 38 Then txt_BabyCone_TareWeight.Focus() ' SendKeys.Send("+{TAB}")
    End Sub

    Private Sub chk_RewindingStatus_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles chk_RewindingStatus.KeyPress
        If Asc(e.KeyChar) = 13 Then
            If chk_RewindingStatus.Checked = True Then
                txt_RewindingCones.Focus()

            ElseIf txt_RwExcSht.Enabled = True And txt_RwExcSht.Visible = True Then
                txt_RwExcSht.Focus()

            Else
                txt_ExcessShort_AddLess.Focus()
            End If
        End If

    End Sub

    Private Sub BeamCount_Calculation()
        Dim eds() As String
        Dim Wpm() As String
        Dim wwg() As String
        Dim bmc As String
        Dim i As Integer
        Dim Mtr_Divisor As Single, Yrd_Divisor As Single, v As Single
        Dim sWarpWgt As String

        On Error Resume Next

        If chk_SocietySet.Checked = True Then
            lbl_BeamCount.Text = Val(cbo_CountName.Text)

        Else

            Mtr_Divisor = 0
            Yrd_Divisor = 0
            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1006" Then Mtr_Divisor = 1695 '---- Divya Sizing (Thekkalur)
            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1012" Then Mtr_Divisor = 1690 '---- Avinashi Sizing (Avinashi)

            If Val(Yrd_Divisor) = 0 Then Yrd_Divisor = 1848
            If Val(Mtr_Divisor) = 0 Then Mtr_Divisor = 1690 ' 1693

            sWarpWgt = ""
            If dgv_WarpingDetails_Total_Set1.RowCount > 0 Then
                sWarpWgt = Trim(Val(dgv_WarpingDetails_Total_Set1.Rows(0).Cells(4).Value()))
            End If
            If dgv_WarpingDetails_Total_Set2.RowCount > 0 Then
                If Val(dgv_WarpingDetails_Total_Set2.Rows(0).Cells(4).Value()) <> 0 Then
                    sWarpWgt = Trim(sWarpWgt) & "," & Trim(Val(dgv_WarpingDetails_Total_Set2.Rows(0).Cells(4).Value()))
                End If
            End If
            If dgv_WarpingDetails_Total_Set3.RowCount > 0 Then
                If Val(dgv_WarpingDetails_Total_Set3.Rows(0).Cells(4).Value()) <> 0 Then
                    sWarpWgt = Trim(sWarpWgt) & "," & Trim(Val(dgv_WarpingDetails_Total_Set3.Rows(0).Cells(4).Value()))
                End If
            End If

            eds = Split(lbl_Total_Warping_Ends.Text, ",")
            Wpm = Split(txt_WarpMeters.Text, ",")
            wwg = Split(sWarpWgt, ",")


            bmc = ""
            For i = 0 To UBound(wwg)
                If Val(wwg(i)) <> 0 Then
                    v = 0
                    If Trim(UCase(cbo_Meters_Yards.Text)) = "YARDS" Then
                        v = Format(Val(eds(i)) * Val(Wpm(i)) / Val(Yrd_Divisor) / Val(wwg(i)), "########0.00")
                    Else
                        v = Format(Val(eds(i)) * Val(Wpm(i)) / Val(Mtr_Divisor) / Val(wwg(i)), "########0.00")
                    End If

                    bmc = Trim(bmc) & IIf(Trim(bmc) <> "", ", ", "") & Trim(Format(Val(v), "########0.00"))
                End If
            Next i

            lbl_BeamCount.Text = bmc

        End If

    End Sub

    Private Sub txt_WarpMeters_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_WarpMeters.KeyPress
        If Asc(e.KeyChar) = 13 Then
            tab_Main.SelectTab(0)
            tab_WarpingDeatils.SelectTab(0)
            If dgv_WarpingDetails_Set1.Rows.Count <= 0 Then dgv_WarpingDetails_Set1.Rows.Add()
            dgv_WarpingDetails_Set1.Focus()
            dgv_WarpingDetails_Set1.CurrentCell = dgv_WarpingDetails_Set1.Rows(0).Cells(1)
            dgv_WarpingDetails_Set1.CurrentCell.Selected = True
        End If
    End Sub

    Private Sub txt_WarpMeters_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_WarpMeters.TextChanged
        BeamCount_Calculation()
        Elongation_Calculation()
    End Sub

    Private Sub txt_Ends_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_Ends.TextChanged
        BeamCount_Calculation()
    End Sub

    Private Sub Excess_Calculation()
        Dim Exsh As Single, vTotYrnTknWeight As Single

        vTotYrnTknWeight = 0
        If dgv_YarnTakenDetails_Total.RowCount > 0 Then
            vTotYrnTknWeight = Val(dgv_YarnTakenDetails_Total.Rows(0).Cells(10).Value())
        End If

        Exsh = Val(lbl_Total_Warping_NetWeight.Text) + Val(lbl_BabyCone_NetWeight.Text) - Val(vTotYrnTknWeight)

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1102" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1144" Then
            lbl_ExcessShort_GrsYarn.Text = Format(Val(Exsh), "##########0.0")
            lbl_ExcessShort.Text = Format(Val(lbl_ExcessShort_GrsYarn.Text) - Val(txt_ExcessShort_AddLess.Text), "##########0.0")

        Else

            lbl_ExcessShort_GrsYarn.Text = Format(Val(Exsh), "##########0.000")
            lbl_ExcessShort.Text = Format(Val(lbl_ExcessShort_GrsYarn.Text) - Val(txt_ExcessShort_AddLess.Text), "##########0.000")

        End If

    End Sub

    Private Sub dgv_YarnTakenDetails_Total_CellValueChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_YarnTakenDetails_Total.CellValueChanged
        If e.ColumnIndex = 5 Or e.ColumnIndex = 6 Or e.ColumnIndex = 7 Or e.ColumnIndex = 8 Or e.ColumnIndex = 9 Or e.ColumnIndex = 10 Then
            Excess_Calculation()
        End If
    End Sub

    Private Sub lbl_BabyCone_NetWeight_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbl_BabyCone_NetWeight.TextChanged
        Excess_Calculation()
    End Sub

    Private Sub lbl_Total_Warping_NetWeight_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbl_Total_Warping_NetWeight.TextChanged
        Excess_Calculation()
        'PickUp_Calculation()
    End Sub

    Private Function Sizing_Beam_Weight_Calculation_From_PickUp(ByVal SizingSlNo As Double, ByVal SizingMtr As Double, ByVal PckUpPerc As Double, ByVal TareWgt As Double) As Double
        Dim Wgt As Double = 0
        Dim Mtrs As Double, Wgt_Mtr As Double, GrsWt As Double, X As Double
        Dim Wpm() As String
        Dim Wwg() As String
        Dim sWarpWgt As String
        Dim WrpMtr As Double = 0, WrpWgt As Double = 0

        Sizing_Beam_Weight_Calculation_From_PickUp = 0

        Try
            sWarpWgt = ""
            If dgv_WarpingDetails_Total_Set1.RowCount > 0 Then
                sWarpWgt = Trim(Val(dgv_WarpingDetails_Total_Set1.Rows(0).Cells(4).Value()))
            Else
                sWarpWgt = "0"
            End If
            If dgv_WarpingDetails_Total_Set2.RowCount > 0 Then
                sWarpWgt = Trim(sWarpWgt) & "," & Trim(Val(dgv_WarpingDetails_Total_Set2.Rows(0).Cells(4).Value()))
            Else
                sWarpWgt = Trim(sWarpWgt) & ",0"
            End If
            If dgv_WarpingDetails_Total_Set3.RowCount > 0 Then
                sWarpWgt = Trim(sWarpWgt) & "," & Trim(Val(dgv_WarpingDetails_Total_Set3.Rows(0).Cells(4).Value()))
            Else
                sWarpWgt = Trim(sWarpWgt) & ",0"
            End If

            Wpm = Split(txt_WarpMeters.Text, ",")
            Wwg = Split(sWarpWgt, ",")

            WrpMtr = 0
            If (SizingSlNo - 1) <= UBound(Wpm) Then
                WrpMtr = Val(Wpm(SizingSlNo - 1))
            End If

            WrpWgt = 0
            If (SizingSlNo - 1) <= UBound(Wwg) Then
                WrpWgt = Val(Wwg(SizingSlNo - 1))
            End If

            Mtrs = 0
            If Trim(UCase(cbo_Meters_Yards.Text)) = "YARDS" Then
                Mtrs = Val(WrpMtr) / 1.0936
                '        or
                'Mtrs  = Val(WrpMtr) * 0.9144
            Else
                Mtrs = Val(WrpMtr)
            End If

            If Val(txt_TapeLength.Text) <> 0 Then Mtrs = Mtrs * 39.37 / Val(txt_TapeLength.Text)

            Wgt_Mtr = 0
            If Mtrs <> 0 Then Wgt_Mtr = Val(WrpWgt) / Mtrs
            X = Wgt_Mtr * Val(SizingMtr)

            GrsWt = (X + (X * Val(PckUpPerc) / 100)) + Val(TareWgt)

            Select Case GrsWt - Int(GrsWt)
                Case Is < 0.1
                    GrsWt = Int(GrsWt)
                Case Is < 0.2
                    GrsWt = Int(GrsWt) + 0.2
                Case Is < 0.3
                    GrsWt = Int(GrsWt) + 0.2
                Case Is < 0.4
                    GrsWt = Int(GrsWt) + 0.4
                Case Is < 0.5
                    GrsWt = Int(GrsWt) + 0.4
                Case Is < 0.6
                    GrsWt = Int(GrsWt) + 0.6
                Case Is < 0.7
                    GrsWt = Int(GrsWt) + 0.6
                Case Is < 0.8
                    GrsWt = Int(GrsWt) + 0.8
                Case Is < 0.9
                    GrsWt = Int(GrsWt) + 0.8
                Case Else
                    GrsWt = Int(GrsWt) + 1
            End Select

            Sizing_Beam_Weight_Calculation_From_PickUp = Format(Val(GrsWt), "#########0.000")

        Catch ex As Exception
            MessageBox.Show(ex.Message, "ERROR IN BEAMWISE WEIGHT CALCULATION...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Function

    Private Sub Elongation_Calculation()
        Dim X As Single
        Dim ElgPerc As Single = 0
        Dim ElgMtr As Single = 0
        Dim vTotWrpMtrs As Single
        Dim a() As String

        vTotWrpMtrs = 0

        a = Split(Trim(txt_WarpMeters.Text), ",")
        For i = 0 To UBound(a)
            vTotWrpMtrs = vTotWrpMtrs + Val(a(i))
        Next

        X = 0
        If Trim(UCase(cbo_Meters_Yards.Text)) = "YARDS" Then
            If Val(txt_TapeLength.Text) > 0 Then X = (Val(vTotWrpMtrs) * 36) / Val(txt_TapeLength.Text)
        Else
            If Val(txt_TapeLength.Text) > 0 Then X = (Val(vTotWrpMtrs) * 39.37) / Val(txt_TapeLength.Text)
        End If

        ElgPerc = 0
        ElgMtr = 0
        If X <> 0 Then
            '  ElgMtr = Val(lbl_Total_Sizing_Meters.Text) - X
            ElgPerc = Val(ElgMtr) / X * 100
        End If

    End Sub



    Private Sub txt_TapeLength_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txt_TapeLength.TextChanged
        Elongation_Calculation()
    End Sub



    Private Sub dgv_WarpingDetails_Set1_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgv_WarpingDetails_Set1.LostFocus
        On Error Resume Next
        If Not IsNothing(dgv_WarpingDetails_Set1.CurrentCell) Then dgv_WarpingDetails_Set1.CurrentCell.Selected = False
    End Sub

    Private Sub txt_BabyCone_TareWeight_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txt_BabyCone_TareWeight.TextChanged
        'Total_BabyCone_Calculation()
        BabyCone_NetWeight_Calculation()
    End Sub





    Private Sub dgv_YarnTakenDetails_EditingControlShowing(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewEditingControlShowingEventArgs) Handles dgv_YarnTakenDetails.EditingControlShowing
        dgtxt_YarnTakenDetails = CType(dgv_YarnTakenDetails.EditingControl, DataGridViewTextBoxEditingControl)
    End Sub

    Private Sub dgtxt_YarnTakenDetails_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgtxt_YarnTakenDetails.Enter
        dgv_YarnTakenDetails.EditingControl.BackColor = Color.Lime
        dgv_YarnTakenDetails.EditingControl.ForeColor = Color.Blue
        dgtxt_YarnTakenDetails.SelectAll()
    End Sub

    Private Sub dgtxt_YarnTakenDetails_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dgtxt_YarnTakenDetails.KeyPress

        If dgv_YarnTakenDetails.CurrentCell.ColumnIndex <> 12 Then
            If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
                e.Handled = True
            End If

        End If
    End Sub



    Private Sub dgv_WarpingDetails_Set1_EditingControlShowing(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewEditingControlShowingEventArgs) Handles dgv_WarpingDetails_Set1.EditingControlShowing
        dgtxt_WarpingDetails_Set1 = CType(dgv_WarpingDetails_Set1.EditingControl, DataGridViewTextBoxEditingControl)
    End Sub

    Private Sub dgv_WarpingDetails_Set2_EditingControlShowing(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewEditingControlShowingEventArgs) Handles dgv_WarpingDetails_Set2.EditingControlShowing
        dgtxt_WarpingDetails_Set2 = CType(dgv_WarpingDetails_Set2.EditingControl, DataGridViewTextBoxEditingControl)
    End Sub

    Private Sub dgv_WarpingDetails_Set3_EditingControlShowing(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewEditingControlShowingEventArgs) Handles dgv_WarpingDetails_Set3.EditingControlShowing
        dgtxt_WarpingDetails_Set3 = CType(dgv_WarpingDetails_Set3.EditingControl, DataGridViewTextBoxEditingControl)
    End Sub

    Private Sub dgtxt_WarpingDetails_Set1_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgtxt_WarpingDetails_Set1.Enter
        dgv_WarpingDetails_Set1.EditingControl.BackColor = Color.Lime
        dgv_WarpingDetails_Set1.EditingControl.ForeColor = Color.Blue
        dgtxt_WarpingDetails_Set1.SelectAll()
    End Sub

    Private Sub dgtxt_WarpingDetails_Set1_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgtxt_WarpingDetails_Set1.KeyDown
        If Trim(txt_InvoiceCode.Text) <> "" Then
            e.SuppressKeyPress = True
            e.Handled = True
        End If
    End Sub

    Private Sub dgtxt_WarpingDetails_Set1_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dgtxt_WarpingDetails_Set1.KeyPress
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable

        Try

            With dgv_WarpingDetails_Set1
                If Trim(txt_InvoiceCode.Text) <> "" Then
                    e.Handled = True
                Else
                    If .CurrentCell.ColumnIndex = 1 Or .CurrentCell.ColumnIndex = 2 Or .CurrentCell.ColumnIndex = 3 Or .CurrentCell.ColumnIndex = 4 Or .CurrentCell.ColumnIndex = 11 Or .CurrentCell.ColumnIndex = 13 Or .CurrentCell.ColumnIndex = 15 Or .CurrentCell.ColumnIndex = 17 Or .CurrentCell.ColumnIndex = 19 Or .CurrentCell.ColumnIndex = 21 Then
                        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True

                    End If

                    If .CurrentCell.ColumnIndex = 7 Or .CurrentCell.ColumnIndex = 8 Then
                        If UCase(e.KeyChar) = "D" Or UCase(e.KeyChar) = "T" Then

                            Da = New SqlClient.SqlDataAdapter("select getdate() as servertime", Con)
                            Dt1 = New DataTable
                            Da.Fill(Dt1)

                            If Dt1.Rows.Count > 0 Then
                                dgtxt_WarpingDetails_Set1.Text = Format(Convert.ToDateTime(Dt1.Rows(0).Item("servertime").ToString), "hh:mm tt").ToString
                                '.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = dgtxt_WarpingDetails_Set1.Text
                            End If
                            Dt1.Clear()

                            Dt1.Dispose()
                            Da.Dispose()

                            e.Handled = True

                            dgtxt_WarpingDetails_Set1.SelectAll()

                        ElseIf UCase(e.KeyChar) = "A" Or UCase(e.KeyChar) = "P" Then

                            If Trim(dgtxt_WarpingDetails_Set1.Text) = Trim(Val(dgtxt_WarpingDetails_Set1.Text)) Then
                                dgtxt_WarpingDetails_Set1.Text = Trim(dgtxt_WarpingDetails_Set1.Text) & ":00"
                            End If
                            If Microsoft.VisualBasic.Right(Trim(dgtxt_WarpingDetails_Set1.Text), 1) = ":" Then
                                dgtxt_WarpingDetails_Set1.Text = Trim(dgtxt_WarpingDetails_Set1.Text) & "00"
                            End If
                            dgtxt_WarpingDetails_Set1.Text = Trim(Replace(dgtxt_WarpingDetails_Set1.Text, "AM", ""))
                            dgtxt_WarpingDetails_Set1.Text = Trim(Replace(dgtxt_WarpingDetails_Set1.Text, "PM", ""))
                            If UCase(e.KeyChar) = "A" Then
                                dgtxt_WarpingDetails_Set1.Text = Microsoft.VisualBasic.Left(dgtxt_WarpingDetails_Set1.Text, 5) & " AM"
                            Else
                                dgtxt_WarpingDetails_Set1.Text = Microsoft.VisualBasic.Left(dgtxt_WarpingDetails_Set1.Text, 5) & " PM"
                            End If

                            e.Handled = True

                            SendKeys.Send("{END}")

                        ElseIf Asc(e.KeyChar) = 46 Then
                            e.KeyChar = ":"

                        Else
                            If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True

                        End If

                    End If

                End If
            End With

        Catch ex As Exception
            '-----

        End Try

    End Sub

    Private Sub dgtxt_WarpingDetails_Set2_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgtxt_WarpingDetails_Set2.Enter
        dgv_WarpingDetails_Set2.EditingControl.BackColor = Color.Lime
        dgv_WarpingDetails_Set2.EditingControl.ForeColor = Color.Blue
        dgtxt_WarpingDetails_Set2.SelectAll()
    End Sub

    Private Sub dgtxt_WarpingDetails_Set2_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgtxt_WarpingDetails_Set2.KeyDown
        If Trim(txt_InvoiceCode.Text) <> "" Then
            e.SuppressKeyPress = True
            e.Handled = True
        End If
    End Sub

    Private Sub dgtxt_WarpingDetails_Set2_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dgtxt_WarpingDetails_Set2.KeyPress
        With dgv_WarpingDetails_Set2
            If Trim(txt_InvoiceCode.Text) <> "" Then
                e.Handled = True
            Else
                If .CurrentCell.ColumnIndex = 1 Or .CurrentCell.ColumnIndex = 2 Or .CurrentCell.ColumnIndex = 3 Or .CurrentCell.ColumnIndex = 4 Or .CurrentCell.ColumnIndex = 7 Or .CurrentCell.ColumnIndex = 8 Or .CurrentCell.ColumnIndex = 9 Or .CurrentCell.ColumnIndex = 11 Or .CurrentCell.ColumnIndex = 13 Or .CurrentCell.ColumnIndex = 15 Or .CurrentCell.ColumnIndex = 17 Or .CurrentCell.ColumnIndex = 19 Or .CurrentCell.ColumnIndex = 21 Then
                    If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True

                End If
            End If
        End With

    End Sub

    Private Sub dgtxt_WarpingDetails_Set3_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgtxt_WarpingDetails_Set3.Enter
        dgv_WarpingDetails_Set3.EditingControl.BackColor = Color.Lime
        dgv_WarpingDetails_Set3.EditingControl.ForeColor = Color.Blue
        dgtxt_WarpingDetails_Set3.SelectAll()
    End Sub

    Private Sub dgtxt_WarpingDetails_Set3_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgtxt_WarpingDetails_Set3.KeyDown
        If Trim(txt_InvoiceCode.Text) <> "" Then
            e.SuppressKeyPress = True
            e.Handled = True
        End If
    End Sub

    Private Sub dgtxt_WarpingDetails_Set3_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dgtxt_WarpingDetails_Set3.KeyPress

        With dgv_WarpingDetails_Set3
            If Trim(txt_InvoiceCode.Text) <> "" Then
                e.Handled = True
            Else
                If .CurrentCell.ColumnIndex = 1 Or .CurrentCell.ColumnIndex = 2 Or .CurrentCell.ColumnIndex = 3 Or .CurrentCell.ColumnIndex = 4 Or .CurrentCell.ColumnIndex = 7 Or .CurrentCell.ColumnIndex = 8 Or .CurrentCell.ColumnIndex = 9 Or .CurrentCell.ColumnIndex = 11 Or .CurrentCell.ColumnIndex = 13 Or .CurrentCell.ColumnIndex = 15 Or .CurrentCell.ColumnIndex = 17 Or .CurrentCell.ColumnIndex = 19 Or .CurrentCell.ColumnIndex = 21 Then
                    If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True

                End If
            End If
        End With
    End Sub

    Private Sub dgv_BabyConeDetails_EditingControlShowing(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewEditingControlShowingEventArgs) Handles dgv_BabyConeDetails.EditingControlShowing
        dgtxt_BabyConeDetails = CType(dgv_BabyConeDetails.EditingControl, DataGridViewTextBoxEditingControl)
    End Sub

    Private Sub dgtxt_BabyConeDetails_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgtxt_BabyConeDetails.Enter
        dgv_BabyConeDetails.EditingControl.BackColor = Color.Lime
        dgv_BabyConeDetails.EditingControl.ForeColor = Color.Blue
        dgtxt_BabyConeDetails.SelectAll()
    End Sub

    Private Sub dgtxt_BabyConeDetails_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgtxt_BabyConeDetails.KeyDown
        If Val(txt_BabyCone_DeliveryWeight.Text) <> 0 Then
            e.SuppressKeyPress = True
            e.Handled = True
        End If
    End Sub

    Private Sub dgtxt_BabyConeDetails_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dgtxt_BabyConeDetails.KeyPress

        If Val(txt_BabyCone_DeliveryWeight.Text) <> 0 Then
            e.Handled = True

        Else

            If dgv_BabyConeDetails.CurrentCell.ColumnIndex <> 6 Then
                If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
            End If


        End If

    End Sub


    Private Sub dgtxt_WarpingDetails_Set1_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgtxt_WarpingDetails_Set1.KeyUp
        dgv_WarpingDetails_Set1_KeyUp(sender, e)
    End Sub

    Private Sub dgtxt_WarpingDetails_Set2_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgtxt_WarpingDetails_Set2.KeyUp
        dgv_WarpingDetails_Set2_KeyUp(sender, e)
    End Sub

    Private Sub dgtxt_WarpingDetails_Set3_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgtxt_WarpingDetails_Set3.KeyUp
        dgv_WarpingDetails_Set3_KeyUp(sender, e)
    End Sub



    Private Sub dgtxt_YarnTakenDetails_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgtxt_YarnTakenDetails.KeyUp
        dgv_YarnTakenDetails_KeyUp(sender, e)
    End Sub

    Private Sub dgtxt_BabyConeDetails_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgtxt_BabyConeDetails.KeyUp
        dgv_BabyConeDetails_KeyUp(sender, e)
    End Sub



    Private Sub chk_SocietySet_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chk_SocietySet.CheckedChanged
        BeamCount_Calculation()
    End Sub

    Private Sub chk_SocietySet_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles chk_SocietySet.KeyDown
        If e.KeyCode = 38 Then SendKeys.Send("+{TAB}")
        If e.KeyCode = 40 Then btn_save.Focus() ' SendKeys.Send("{TAB}")
    End Sub

    Private Sub chk_SocietySet_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles chk_SocietySet.KeyPress
        If Asc(e.KeyChar) = 13 Then
            If MessageBox.Show("Do you want to save ?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = DialogResult.Yes Then
                save_record()
            Else
                If cbo_Ledger.Enabled And cbo_Ledger.Visible Then cbo_Ledger.Focus()
            End If
        End If
    End Sub

    Private Sub txt_RwExcSht_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_RwExcSht.KeyDown
        If e.KeyValue = 38 Or (e.Control = True And e.KeyValue = 38) Then
            If cbo_Rw_MillName.Enabled Then
                cbo_Rw_MillName.Focus()
            Else
                chk_RewindingStatus.Focus()
            End If
        End If
        If e.KeyValue = 40 Or (e.Control = True And e.KeyValue = 40) Then
            tab_Main.SelectTab(4)
        End If
    End Sub

    Private Sub txt_RwExcSht_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_RwExcSht.KeyPress
        If Asc(e.KeyChar) = 13 Then
            txt_Remarks.Focus()
            'tab_Main.SelectTab(4)
        End If
    End Sub

    Private Sub btn_save_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_save.Click
        save_record()
    End Sub

    'Private Sub btn_Print_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
    '  prin

    'End Sub

    Private Sub btn_PDF_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Common_Procedures.Print_OR_Preview_Status = 1
        Print_PDF_Status = True
        print_record()
        Print_PDF_Status = False
    End Sub

    Private Sub btn_SMS_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        'Dim request As HttpWebRequest
        'Dim response As HttpWebResponse = Nothing
        'Dim url As String
        Dim i As Integer = 0
        Dim smstxt As String = ""
        Dim PhNo As String = ""
        Dim Led_IdNo As Integer = 0

        Try

            Led_IdNo = Common_Procedures.Ledger_AlaisNameToIdNo(Con, cbo_Ledger.Text)
            'If Led_IdNo  = 0 Then Exit Sub

            PhNo = Common_Procedures.get_FieldValue(Con, "Ledger_Head", "Ledger_MobileNo", "(Ledger_IdNo = " & Str(Val(Led_IdNo)) & ")")

            smstxt = "SPECIFICATION " & vbCrLf & vbCrLf
            smstxt = smstxt & "SETNO-" & Trim(lbl_SetNo.Text) & vbCrLf & "DATE-" & Trim(dtp_Date.Text)
            smstxt = smstxt & vbCrLf & "Avg.Count-" & Trim(Val(lbl_BeamCount.Text))
            smstxt = smstxt & vbCrLf & "Consumed-" & Trim(Val(lbl_Total_Warping_NetWeight.Text))
            If Val(lbl_ExcessShort.Text) < 0 Then
                smstxt = smstxt & vbCrLf & "Short-" & Trim(Math.Abs(Val(lbl_ExcessShort.Text)))
            Else
                smstxt = smstxt & vbCrLf & "Excess-" & Trim(Val(lbl_ExcessShort.Text))
            End If

            'smstxt = "SETNO-" & Trim(lbl_SetNo.Text) & "%2C+" & "DATE-" & Trim(dtp_Date.Text)
            'smstxt = smstxt & "%2C+" & "Consumed-" & Trim(Val(lbl_Total_Warping_NetWeight.Text))
            'If Val(lbl_ExcessShort.Text) < 0 Then
            '    smstxt = smstxt & "%2C+" & "Short-" & Trim(Math.Abs(Val(lbl_ExcessShort.Text)))
            'Else
            '    smstxt = smstxt & "%2C+" & "Excess-" & Trim(Val(lbl_ExcessShort.Text))
            'End If

            Sms_Entry.vSmsPhoneNo = Trim(PhNo)
            Sms_Entry.vSmsMessage = Trim(smstxt)


            Dim f1 As New Sms_Entry


            f1.MdiParent = MDIParent1
            f1.Show()

            ' ''url = "http://sms.shamsoft.in/app/smsapi/index.php?key=355C7A0B5595B2&routeid=73&type=text&contacts=" & Trim(PhNo) & "&senderid=WEBSMS&msg=" & Trim(smstxt)

            ' ''--THIS IS Working (jenilla)
            ''url = "http://103.16.101.52:8080/bulksms/bulksms?username=nila-jenial&password=nila123&type=0&dlr=1&destination=918508403222&source=JENIAL&message=" & Trim(smstxt)

            ' ''THIS IS OK
            ' ''url = "http://sms.shamsoft.in/app/smsapi/index.php?key=355C7A0B5595B2&routeid=73&type=text&contacts=8508403222&senderid=WEBSMS&msg=Hello+People%2C+have+a+great+day"

            ' ''url = "http://sms.shamsoft.in/app/smsapi/index.php?key=355C7A0B5595B2&routeid=14&type=text&contacts=97656XXXXX,98012XXXXX&senderid=DEMO&msg=Hello+People%2C+have+a+great+day"

            ' ''url = "http://103.16.101.52:8080/bulksms/bulksms?username=nila-jenial&password=nila123&type=0&dlr=1&destination=918508403222&source=JENIAL&message=testmsg"

            ' ''url = "http://103.16.101.52:8080/bulksms/bulksms?username=nila-jenial&password=nila123&type=0&dlr=1&destination=918508403222&source=JENIAL&message=testmsg"

            ''request = DirectCast(WebRequest.Create(url), HttpWebRequest)

            ''response = DirectCast(request.GetResponse(), HttpWebResponse)

            ''If Trim(UCase(response.StatusDescription)) = "OK" Then
            ''    MessageBox.Show("Sucessfully Sent...", "FOR SENDING SMS...", MessageBoxButtons.OKCancel, MessageBoxIcon.Information)
            ''    'MessageBox.Show("Response: " & response.StatusDescription)
            ''Else
            ''    MessageBox.Show("Failed to sent SMS...", "FOR SENDING SMS...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            ''End If

            ' ''WebBrowser1.Navigate("http://103.16.101.52:8080/bulksms/bulksms?username=nila-jenial&password=nila123&type=0&dlr=1&destination=918508403222&source=JENIAL&message=testmsg")
            ' ''MsgBox("sms send")

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT SEND SMS...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub btn_EMail_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim Led_IdNo As Integer
        Dim MailTxt As String

        Try


            Led_IdNo = Common_Procedures.Ledger_AlaisNameToIdNo(Con, cbo_Ledger.Text)
            'If Led_IdNo  = 0 Then Exit Sub


            MailTxt = "SIZING SPECIFICATION " & vbCrLf & vbCrLf

            MailTxt = MailTxt & "SET.NO:" & Trim(lbl_SetNo.Text) & vbCrLf & "SET.DATE:" & Trim(dtp_Date.Text) & vbCrLf

            'For i = 0 To dgv_Details.Rows.Count - 1

            '    If Val(dgv_Details.Rows(i).Cells(5).Value) <> 0 Then
            '        MailTxt = Trim(MailTxt) & IIf(Trim(MailTxt) <> "", vbCrLf, "") & Trim(dgv_Details.Rows(i).Cells(3).Value) & "       -          " & Val(dgv_Details.Rows(i).Cells(5).Value) & " " & Trim(dgv_Details.Rows(i).Cells(6).Value)
            '    End If

            'Next

            EMAIL_Entry.vMailID = Common_Procedures.get_FieldValue(Con, "Ledger_Head", "Ledger_Mail", "(Ledger_IdNo = " & Str(Val(Led_IdNo)) & ")")
            EMAIL_Entry.vSubJect = "Specification for SetNo : " & Trim(lbl_SetNo.Text)
            EMAIL_Entry.vMessage = Trim(MailTxt)

            Dim f1 As New EMAIL_Entry
            f1.MdiParent = MDIParent1
            f1.Show()

            ' ''MailTxt = "SIZING SPECIFICATION " & vbCrLf & vbCrLf

            ' ''MailTxt = MailTxt & "SET.NO:" & Trim(lbl_SetNo.Text) & vbCrLf & "SET.DATE:" & Trim(dtp_Date.Text) & vbCrLf

            '' ''For i = 0 To dgv_Details.Rows.Count - 1

            '' ''    If Val(dgv_Details.Rows(i).Cells(5).Value) <> 0 Then
            '' ''        MailTxt = Trim(MailTxt) & IIf(Trim(MailTxt) <> "", vbCrLf, "") & Trim(dgv_Details.Rows(i).Cells(3).Value) & "       -          " & Val(dgv_Details.Rows(i).Cells(5).Value) & " " & Trim(dgv_Details.Rows(i).Cells(6).Value)
            '' ''    End If

            '' ''Next


            ' ''Dim SmtpServer As New SmtpClient()
            ' ''Dim mail As New MailMessage()
            ' ''Dim MailTxt As String

            ' ''MailTxt = "SIZING SPECIFICATION " & vbCrLf & vbCrLf

            ' ''MailTxt = MailTxt & "SET.NO:" & Trim(lbl_SetNo.Text) & vbCrLf & "SET.DATE:" & Trim(dtp_Date.Text) & vbCrLf

            '' ''For i = 0 To dgv_Details.Rows.Count - 1

            '' ''    If Val(dgv_Details.Rows(i).Cells(5).Value) <> 0 Then
            '' ''        MailTxt = Trim(MailTxt) & IIf(Trim(MailTxt) <> "", vbCrLf, "") & Trim(dgv_Details.Rows(i).Cells(3).Value) & "       -          " & Val(dgv_Details.Rows(i).Cells(5).Value) & " " & Trim(dgv_Details.Rows(i).Cells(6).Value)
            '' ''    End If

            '' ''Next

            ' ''SmtpServer.Port = 587
            ' ''SmtpServer.Host = "smtp.gmail.com"
            ' ''SmtpServer.UseDefaultCredentials = False
            ' ''SmtpServer.EnableSsl = True

            ' ''SmtpServer.Credentials = New Net.NetworkCredential("tsoft.tirupur@gmail.com", "GOLD@tn39av7417")
            '' ''SmtpServer.Credentials = New Net.NetworkCredential("varalakshmithanges@gmail.com", "GOLD@tn39av7417")
            '' ''SmtpServer.Credentials = New Net.NetworkCredential("varalakshmithanges@gmail.com", "gold&VL@19=rj")
            '' ''SmtpServer.Credentials = New Net.NetworkCredential("t.thanges@gmail.com", "gold@tn39av7417")
            '' ''SmtpServer.Credentials = New Net.NetworkCredential("t.thanges@gmail.com", "cikysrpmkzbwliuc")
            '' ''SmtpServer.Credentials = New Net.NetworkCredential("varalakshmithanges@gmail.com", "thanges19")
            '' ''SmtpServer.Credentials = New Net.NetworkCredential("tsoft.tirupur@gmail.com", "8508403221")
            '' ''SmtpServer.Credentials = New Net.NetworkCredential("t.thanges@gmail.com", "rj17052012")

            ' ''mail = New MailMessage()
            '' ''mail.From = New MailAddress("varalakshmithanges@gmail.com")
            ' ''mail.From = New MailAddress("tsoft.tirupur@gmail.com")
            '' ''mail.From = New MailAddress("t.thanges@gmail.com")
            '' ''srirajatex@gmail.com
            ' ''mail.To.Add("thanges@rediffmail.com")
            ' ''mail.Subject = "Sizing Specification"
            ' ''mail.Body = Trim(MailTxt)

            ' ''Dim attachment As System.Net.Mail.Attachment
            ' ''attachment = New System.Net.Mail.Attachment("your attachment file")
            ' ''mail.Attachments.Add(attachment)

            ' ''SmtpServer.Send(mail)

            ' ''MessageBox.Show("Mail send Sucessfully", "FOR MAILING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Information)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT SEND MAIL...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub btn_close_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_close.Click
        Me.Close()
    End Sub
    Private Sub PrintPreview_Toolstrip_ItemClicked(ByVal sender As Object, ByVal e As ToolStripItemClickedEventArgs)
        'If it is the print button that was clicked: run the printdialog
        If LCase(e.ClickedItem.Name) = LCase("printToolStripButton") Then

            Try
                'chk_Printed.Checked = True
                'chk_Printed.Visible = True
                Update_PrintOut_Status()

            Catch ex As Exception
                MsgBox("Print Error: " & ex.Message)

            End Try
        End If
    End Sub

    Private Sub PrintPreview_Shown(ByVal sender As Object, ByVal e As System.EventArgs)
        'Capture the click events for the toolstrip in the dialog box when the dialog is shown
        Dim ts As ToolStrip = CType(sender.Controls(1), ToolStrip)
        AddHandler ts.ItemClicked, AddressOf PrintPreview_Toolstrip_ItemClicked
    End Sub

    Private Sub Update_PrintOut_Status(Optional ByVal sqltr As SqlClient.SqlTransaction = Nothing)
        Dim cmd As New SqlClient.SqlCommand
        Dim NewCode As String = ""
        Dim vPrnSTS As Integer = 0


        Try

            NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_SetNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            cmd.Connection = Con
            If IsNothing(sqltr) = False Then
                cmd.Transaction = sqltr
            End If

            cmd.CommandText = "Update Rewinding_Receipt_Head set PrintOut_Status = " & Str(Val(vPrnSTS)) & " where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Rewinding_Receipt_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            'If chk_Printed.Checked = True Then
            '    chk_Printed.Visible = True
            '    If Val(Common_Procedures.User.IdNo) = 1 Then
            '        chk_Printed.Enabled = True
            '    End If
            'End If

            cmd.Dispose()

        Catch ex As Exception
            MsgBox(ex.Message)

        End Try

    End Sub

    Public Sub print_record() Implements Interface_MDIActions.print_record
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim NewCode As String
        Dim PpSzSTS As Boolean = False
        Dim ps As Printing.PaperSize
        Dim I As Integer = 0

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_SetNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Try

            da1 = New SqlClient.SqlDataAdapter("select * from Warping_Head Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Warp_Code = '" & Trim(NewCode) & "'", Con)
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

        If Common_Procedures.settings.CustomerCode = "1288" Then
            PrintDocument1.DefaultPageSettings.Landscape = True
        Else
            PrintDocument1.DefaultPageSettings.Landscape = False
        End If

        If Common_Procedures.settings.CustomerCode = "1282" Then

            For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
                If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A5 Then
                    ps = PrintDocument1.PrinterSettings.PaperSizes(I)
                    PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = ps
                    PrintDocument1.DefaultPageSettings.PaperSize = ps
                    PpSzSTS = True
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

                ppd.WindowState = FormWindowState.Maximized
                ppd.StartPosition = FormStartPosition.CenterScreen
                'ppd.ClientSize = New Size(600, 600)

                AddHandler ppd.Shown, AddressOf PrintPreview_Shown
                ppd.ShowDialog()

            Catch ex As Exception
                MsgBox("The printing operation failed" & vbCrLf & ex.Message, MsgBoxStyle.Critical, "DOES NOT SHOW PRINT PREVIEW...")

            End Try

        End If

    End Sub






    Private Sub cbo_Grid_FrontWarper_1_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Grid_FrontWarper_1.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, Con, cbo_Grid_FrontWarper_1, Nothing, "PayRoll_Employee_Head", "Employee_Name", "", "(Employee_IdNo = 0)")
        If Asc(e.KeyChar) = 13 Then
            With dgv_WarpingDetails_Set1
                .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_Grid_FrontWarper_1.Text)
                .Focus()
                .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)
                .CurrentCell.Selected = True
            End With
        End If
    End Sub

    Private Sub cbo_Grid_FrontWarper_1_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Grid_FrontWarper_1.KeyDown
        Dim dep_idno As Integer = 0

        vcbo_KeyDwnVal = e.KeyValue

        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, Con, cbo_Grid_FrontWarper_1, Nothing, Nothing, "PayRoll_Employee_Head", "Employee_Name", "", "(Employee_IdNo = 0)")

        With dgv_WarpingDetails_Set1

            If (e.KeyValue = 38 And cbo_Grid_FrontWarper_1.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
                .Focus()
                .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex - 1)
                .CurrentCell.Selected = True
            End If

            If (e.KeyValue = 40 And cbo_Grid_FrontWarper_1.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
                .Focus()
                .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)
                .CurrentCell.Selected = True
            End If

            If e.Control = True And e.KeyValue = 13 Then
                With dgv_WarpingDetails_Set1
                    .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_Grid_FrontWarper_1.Text)
                    .Focus()
                    If .CurrentCell.RowIndex < .Rows.Count - 1 Then
                        .CurrentCell = .Rows(.CurrentCell.RowIndex + 1).Cells(1)
                    Else
                        .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)
                    End If
                    .CurrentCell.Selected = True
                    e.Handled = True
                    e.SuppressKeyPress = True
                End With
            End If

        End With

    End Sub

    Private Sub cbo_Grid_FromtWarper_1_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Grid_FrontWarper_1.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Dim f As New Payroll_Employee_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Grid_FrontWarper_1.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If
    End Sub

    Private Sub cbo_Grid_FrontWarper_1_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Grid_FrontWarper_1.TextChanged
        Try
            If cbo_Grid_FrontWarper_1.Visible Then

                If IsNothing(dgv_WarpingDetails_Set1.CurrentCell) Then Exit Sub
                With dgv_WarpingDetails_Set1
                    If Val(cbo_Grid_FrontWarper_1.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = 10 Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_Grid_FrontWarper_1.Text)
                    End If
                End With
            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub cbo_Grid_BackWarper1_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Grid_BackWarper1.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, Con, cbo_Grid_BackWarper1, Nothing, "PayRoll_Employee_Head", "Employee_Name", "", "(Employee_IdNo = 0)")
        If Asc(e.KeyChar) = 13 Then
            With dgv_WarpingDetails_Set1
                .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_Grid_BackWarper1.Text)
                .Focus()
                .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)
                .CurrentCell.Selected = True
            End With
        End If
    End Sub

    Private Sub cbo_Grid_BackWarper1_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Grid_BackWarper1.KeyDown
        Dim dep_idno As Integer = 0

        vcbo_KeyDwnVal = e.KeyValue

        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, Con, cbo_Grid_BackWarper1, Nothing, Nothing, "PayRoll_Employee_Head", "Employee_Name", "", "(Employee_IdNo = 0)")

        With dgv_WarpingDetails_Set1

            If (e.KeyValue = 38 And cbo_Grid_BackWarper1.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
                .Focus()
                .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex - 1)
                .CurrentCell.Selected = True
            End If

            If (e.KeyValue = 40 And cbo_Grid_BackWarper1.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
                .Focus()
                .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)
                .CurrentCell.Selected = True
            End If

            If e.Control = True And e.KeyValue = 13 Then
                .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_Grid_BackWarper1.Text)
                .Focus()
                If .CurrentCell.RowIndex < .Rows.Count - 1 Then
                    .CurrentCell = .Rows(.CurrentCell.RowIndex + 1).Cells(1)
                Else
                    .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)
                End If
                .CurrentCell.Selected = True
                e.Handled = True
                e.SuppressKeyPress = True
            End If

        End With

    End Sub

    Private Sub cbo_Grid_BackWarper_1_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Grid_BackWarper1.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Dim f As New Payroll_Employee_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Grid_BackWarper1.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If
    End Sub

    Private Sub cbo_Grid_BackWarper_1_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Grid_BackWarper1.TextChanged
        Try
            If cbo_Grid_BackWarper1.Visible Then

                If IsNothing(dgv_WarpingDetails_Set1.CurrentCell) Then Exit Sub
                With dgv_WarpingDetails_Set1
                    If Val(cbo_Grid_BackWarper1.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = 12 Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_Grid_BackWarper1.Text)
                    End If
                End With
            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub cbo_Grid_Helper1_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Grid_Helper1.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, Con, cbo_Grid_Helper1, Nothing, "PayRoll_Employee_Head", "Employee_Name", "", "(Employee_IdNo = 0)")
        If Asc(e.KeyChar) = 13 Then
            With dgv_WarpingDetails_Set1
                .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_Grid_Helper1.Text)
                .Focus()
                .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)
                .CurrentCell.Selected = True
            End With
        End If
    End Sub

    Private Sub cbo_Grid_Helper1_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Grid_Helper1.KeyDown
        Dim dep_idno As Integer = 0

        vcbo_KeyDwnVal = e.KeyValue

        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, Con, cbo_Grid_Helper1, Nothing, Nothing, "PayRoll_Employee_Head", "Employee_Name", "", "(Employee_IdNo = 0)")

        With dgv_WarpingDetails_Set1

            If (e.KeyValue = 38 And cbo_Grid_Helper1.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
                .Focus()
                .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex - 1)
                .CurrentCell.Selected = True
            End If

            If (e.KeyValue = 40 And cbo_Grid_Helper1.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
                .Focus()
                .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)
                .CurrentCell.Selected = True
            End If

            If e.Control = True And e.KeyValue = 13 Then
                .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_Grid_Helper1.Text)
                .Focus()
                If .CurrentCell.RowIndex < .Rows.Count - 1 Then
                    .CurrentCell = .Rows(.CurrentCell.RowIndex + 1).Cells(1)
                Else
                    .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)
                End If
                .CurrentCell.Selected = True
                e.Handled = True
                e.SuppressKeyPress = True
            End If

        End With

    End Sub

    Private Sub cbo_Grid_Helper1_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Grid_Helper1.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Dim f As New Payroll_Employee_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Grid_Helper1.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If
    End Sub

    Private Sub cbo_Grid_Helper1_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Grid_Helper1.TextChanged
        Try
            If cbo_Grid_Helper1.Visible Then

                If IsNothing(dgv_WarpingDetails_Set1.CurrentCell) Then Exit Sub
                With dgv_WarpingDetails_Set1
                    If Val(cbo_Grid_Helper1.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = 14 Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_Grid_Helper1.Text)
                    End If
                End With
            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try
    End Sub


    Private Sub cbo_Grid_FrontWarper2_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Grid_FrontWarper2.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, Con, cbo_Grid_FrontWarper2, Nothing, "PayRoll_Employee_Head", "Employee_Name", "", "(Employee_IdNo = 0)")
        If Asc(e.KeyChar) = 13 Then
            With dgv_WarpingDetails_Set1
                .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_Grid_FrontWarper2.Text)
                .Focus()
                .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)
                .CurrentCell.Selected = True
            End With
        End If
    End Sub

    Private Sub cbo_Grid_FrontWarper2_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Grid_FrontWarper2.KeyDown
        Dim dep_idno As Integer = 0

        vcbo_KeyDwnVal = e.KeyValue

        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, Con, cbo_Grid_FrontWarper2, Nothing, Nothing, "PayRoll_Employee_Head", "Employee_Name", "", "(Employee_IdNo = 0)")

        With dgv_WarpingDetails_Set1

            If (e.KeyValue = 38 And cbo_Grid_FrontWarper2.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
                .Focus()
                .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex - 1)
                .CurrentCell.Selected = True
            End If

            If (e.KeyValue = 40 And cbo_Grid_FrontWarper2.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
                .Focus()
                .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)
                .CurrentCell.Selected = True
            End If

            If e.Control = True And e.KeyValue = 13 Then
                .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_Grid_FrontWarper2.Text)
                .Focus()
                If .CurrentCell.RowIndex < .Rows.Count - 1 Then
                    .CurrentCell = .Rows(.CurrentCell.RowIndex + 1).Cells(1)
                Else
                    .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)
                End If
                .CurrentCell.Selected = True
                e.Handled = True
                e.SuppressKeyPress = True
            End If

        End With

    End Sub

    Private Sub cbo_Grid_FromtWarper2_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Grid_FrontWarper2.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Dim f As New Payroll_Employee_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Grid_FrontWarper2.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If
    End Sub

    Private Sub cbo_Grid_FrontWarper2_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Grid_FrontWarper2.TextChanged
        Try
            If cbo_Grid_FrontWarper2.Visible Then

                If IsNothing(dgv_WarpingDetails_Set1.CurrentCell) Then Exit Sub
                With dgv_WarpingDetails_Set1
                    If Val(cbo_Grid_FrontWarper2.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = 16 Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_Grid_FrontWarper2.Text)
                    End If
                End With
            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try
    End Sub
    Private Sub cbo_Grid_BackWarper2_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Grid_BackWarper2.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, Con, cbo_Grid_BackWarper2, Nothing, "PayRoll_Employee_Head", "Employee_Name", "", "(Employee_IdNo = 0)")
        If Asc(e.KeyChar) = 13 Then
            With dgv_WarpingDetails_Set1
                .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_Grid_BackWarper2.Text)
                .Focus()
                .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)
                .CurrentCell.Selected = True
            End With
        End If
    End Sub

    Private Sub cbo_Grid_BackWarper2_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Grid_BackWarper2.KeyDown
        Dim dep_idno As Integer = 0

        vcbo_KeyDwnVal = e.KeyValue

        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, Con, cbo_Grid_BackWarper2, Nothing, Nothing, "PayRoll_Employee_Head", "Employee_Name", "", "(Employee_IdNo = 0)")

        With dgv_WarpingDetails_Set1

            If (e.KeyValue = 38 And cbo_Grid_BackWarper2.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
                .Focus()
                .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex - 1)
                .CurrentCell.Selected = True
            End If

            If (e.KeyValue = 40 And cbo_Grid_BackWarper2.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
                .Focus()
                .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)
                .CurrentCell.Selected = True
            End If

            If e.Control = True And e.KeyValue = 13 Then
                .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_Grid_BackWarper2.Text)
                .Focus()
                If .CurrentCell.RowIndex < .Rows.Count - 1 Then
                    .CurrentCell = .Rows(.CurrentCell.RowIndex + 1).Cells(1)
                Else
                    .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)
                End If
                .CurrentCell.Selected = True
                e.Handled = True
                e.SuppressKeyPress = True
            End If

        End With

    End Sub

    Private Sub cbo_Grid_BackWarper2_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Grid_BackWarper2.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Dim f As New Payroll_Employee_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Grid_BackWarper2.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If
    End Sub

    Private Sub cbo_Grid_BackWarper2_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Grid_BackWarper2.TextChanged
        Try
            If cbo_Grid_BackWarper2.Visible Then

                If IsNothing(dgv_WarpingDetails_Set1.CurrentCell) Then Exit Sub
                With dgv_WarpingDetails_Set1
                    If Val(cbo_Grid_BackWarper2.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = 18 Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_Grid_BackWarper2.Text)
                    End If
                End With
            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try
    End Sub
    Private Sub cbo_Grid_Helper2_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Grid_Helper2.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, Con, cbo_Grid_Helper2, Nothing, "PayRoll_Employee_Head", "Employee_Name", "", "(Employee_IdNo = 0)")
        If Asc(e.KeyChar) = 13 Then
            With dgv_WarpingDetails_Set1
                .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_Grid_Helper2.Text)
                .Focus()
                .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)
                .CurrentCell.Selected = True
            End With
        End If
    End Sub

    Private Sub cbo_Grid_Helper2_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Grid_Helper2.KeyDown
        Dim dep_idno As Integer = 0

        vcbo_KeyDwnVal = e.KeyValue

        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, Con, cbo_Grid_Helper2, Nothing, Nothing, "PayRoll_Employee_Head", "Employee_Name", "", "(Employee_IdNo = 0)")

        With dgv_WarpingDetails_Set1

            If (e.KeyValue = 38 And cbo_Grid_Helper2.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
                .Focus()
                .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex - 1)
                .CurrentCell.Selected = True
            End If

            If (e.KeyValue = 40 And cbo_Grid_Helper2.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
                .Focus()
                .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)
                .CurrentCell.Selected = True
            End If

            If e.Control = True And e.KeyValue = 13 Then
                .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_Grid_Helper2.Text)
                .Focus()
                If .CurrentCell.RowIndex < .Rows.Count - 1 Then
                    .CurrentCell = .Rows(.CurrentCell.RowIndex + 1).Cells(1)
                Else
                    .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)
                End If
                .CurrentCell.Selected = True
                e.Handled = True
                e.SuppressKeyPress = True
            End If

        End With

    End Sub

    Private Sub cbo_Grid_Helper2_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Grid_Helper2.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Dim f As New Payroll_Employee_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Grid_Helper2.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If
    End Sub

    Private Sub cbo_Grid_Helper2_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Grid_Helper2.TextChanged
        Try
            If cbo_Grid_Helper2.Visible Then

                If IsNothing(dgv_WarpingDetails_Set1.CurrentCell) Then Exit Sub
                With dgv_WarpingDetails_Set1
                    If Val(cbo_Grid_Helper2.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = 20 Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_Grid_Helper2.Text)
                    End If
                End With
            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try
    End Sub
    Private Sub cbo_Ends2_FrontWarper_1_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Ends2_FrontWarper1.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, Con, cbo_Grid_FrontWarper_1, Nothing, "PayRoll_Employee_Head", "Employee_Name", "", "(Employee_IdNo = 0)")
        If Asc(e.KeyChar) = 13 Then
            With dgv_WarpingDetails_Set2
                .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_Ends2_FrontWarper1.Text)
                .Focus()
                .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)
                .CurrentCell.Selected = True
            End With
        End If
    End Sub

    Private Sub cbo_Ends2_FrontWarper_1_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Ends2_FrontWarper1.KeyDown
        Dim dep_idno As Integer = 0

        vcbo_KeyDwnVal = e.KeyValue

        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, Con, cbo_Ends2_FrontWarper1, Nothing, Nothing, "PayRoll_Employee_Head", "Employee_Name", "", "(Employee_IdNo = 0)")

        With dgv_WarpingDetails_Set2

            If (e.KeyValue = 38 And cbo_Ends2_FrontWarper1.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
                .Focus()
                .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex - 1)
                .CurrentCell.Selected = True
            End If

            If (e.KeyValue = 40 And cbo_Ends2_FrontWarper1.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
                .Focus()
                .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)
                .CurrentCell.Selected = True
            End If


            If e.Control = True And e.KeyValue = 13 Then
                With dgv_WarpingDetails_Set2
                    .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_Ends2_FrontWarper1.Text)
                    .Focus()
                    If .CurrentCell.RowIndex < .Rows.Count - 1 Then
                        .CurrentCell = .Rows(.CurrentCell.RowIndex + 1).Cells(1)
                    Else
                        .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)
                    End If
                    .CurrentCell.Selected = True
                    e.Handled = True
                    e.SuppressKeyPress = True
                End With
            End If


        End With

    End Sub

    Private Sub cbo_Ends2_FromtWarper_1_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Ends2_FrontWarper1.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Dim f As New Payroll_Employee_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Ends2_FrontWarper1.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If
    End Sub

    Private Sub cbo_Ends_FrontWarper_1_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Ends2_FrontWarper1.TextChanged
        Try
            If cbo_Ends2_FrontWarper1.Visible Then
                With dgv_WarpingDetails_Set2
                    If Val(cbo_Ends2_FrontWarper1.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = 10 Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_Ends2_FrontWarper1.Text)
                    End If
                End With
            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try
    End Sub


    Private Sub cbo_Ends2_BackWarper1_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Ends2_BackWarper1.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, Con, cbo_Ends2_BackWarper1, Nothing, "PayRoll_Employee_Head", "Employee_Name", "", "(Employee_IdNo = 0)")
        If Asc(e.KeyChar) = 13 Then
            With dgv_WarpingDetails_Set2
                .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_Ends2_BackWarper1.Text)
                .Focus()
                .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)
                .CurrentCell.Selected = True
            End With
        End If
    End Sub

    Private Sub cbo_Ends2_BackWarper1_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Ends2_BackWarper1.KeyDown
        Dim dep_idno As Integer = 0

        vcbo_KeyDwnVal = e.KeyValue

        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, Con, cbo_Ends2_BackWarper1, Nothing, Nothing, "PayRoll_Employee_Head", "Employee_Name", "", "(Employee_IdNo = 0)")

        With dgv_WarpingDetails_Set2

            If (e.KeyValue = 38 And cbo_Ends2_BackWarper1.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
                .Focus()
                .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex - 1)
                .CurrentCell.Selected = True
            End If

            If (e.KeyValue = 40 And cbo_Ends2_BackWarper1.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
                .Focus()
                .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)
                .CurrentCell.Selected = True
            End If



            If e.Control = True And e.KeyValue = 13 Then
                With dgv_WarpingDetails_Set2
                    .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_Ends2_BackWarper1.Text)
                    .Focus()
                    If .CurrentCell.RowIndex < .Rows.Count - 1 Then
                        .CurrentCell = .Rows(.CurrentCell.RowIndex + 1).Cells(1)
                    Else
                        .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)
                    End If
                    .CurrentCell.Selected = True
                    e.Handled = True
                    e.SuppressKeyPress = True
                End With
            End If
        End With

    End Sub

    Private Sub cbo_Ends2_BackWarper_1_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Ends2_BackWarper1.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Dim f As New Payroll_Employee_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Ends2_BackWarper1.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If
    End Sub

    Private Sub cbo_Ends2_BackWarper_1_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Ends2_BackWarper1.TextChanged
        Try
            If cbo_Ends2_BackWarper1.Visible Then
                With dgv_WarpingDetails_Set2
                    If Val(cbo_Ends2_BackWarper1.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = 12 Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_Ends2_BackWarper1.Text)
                    End If
                End With
            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try
    End Sub
    Private Sub cbo_Ends2_Helper_1_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Ends2_Helper1.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, Con, cbo_Ends2_Helper1, Nothing, "PayRoll_Employee_Head", "Employee_Name", "", "(Employee_IdNo = 0)")
        If Asc(e.KeyChar) = 13 Then
            With dgv_WarpingDetails_Set2
                .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_Ends2_Helper1.Text)
                .Focus()
                .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)
                .CurrentCell.Selected = True
            End With
        End If
    End Sub

    Private Sub cbo_Ends2_Helper_1_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Ends2_Helper1.KeyDown
        Dim dep_idno As Integer = 0

        vcbo_KeyDwnVal = e.KeyValue

        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, Con, cbo_Ends2_Helper1, Nothing, Nothing, "PayRoll_Employee_Head", "Employee_Name", "", "(Employee_IdNo = 0)")

        With dgv_WarpingDetails_Set2

            If (e.KeyValue = 38 And cbo_Ends2_Helper1.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
                .Focus()
                .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex - 1)
                .CurrentCell.Selected = True
            End If

            If (e.KeyValue = 40 And cbo_Ends2_Helper1.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
                .Focus()
                .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)
                .CurrentCell.Selected = True
            End If

            If e.Control = True And e.KeyValue = 13 Then
                With dgv_WarpingDetails_Set2
                    .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_Ends2_Helper1.Text)
                    .Focus()
                    If .CurrentCell.RowIndex < .Rows.Count - 1 Then
                        .CurrentCell = .Rows(.CurrentCell.RowIndex + 1).Cells(1)
                    Else
                        .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)
                    End If
                    .CurrentCell.Selected = True
                    e.Handled = True
                    e.SuppressKeyPress = True
                End With
            End If

        End With

    End Sub

    Private Sub cbo_Ends2_Helper1_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Ends2_Helper1.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Dim f As New Payroll_Employee_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Ends2_Helper1.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If
    End Sub

    Private Sub cbo_Ends2_Helper1_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Ends2_Helper1.TextChanged
        Try
            If cbo_Ends2_Helper1.Visible Then
                With dgv_WarpingDetails_Set2
                    If Val(cbo_Ends2_Helper1.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = 14 Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_Ends2_Helper1.Text)
                    End If
                End With
            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try
    End Sub


    Private Sub cbo_Ends_FrontWarper2_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Ends2_FrontWarper2.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, Con, cbo_Ends2_FrontWarper2, Nothing, "PayRoll_Employee_Head", "Employee_Name", "", "(Employee_IdNo = 0)")
        If Asc(e.KeyChar) = 13 Then
            With dgv_WarpingDetails_Set2
                .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_Ends2_FrontWarper2.Text)
                .Focus()
                .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)
                .CurrentCell.Selected = True
            End With
        End If
    End Sub

    Private Sub cbo_Ends2_FrontWarper2_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Ends2_FrontWarper2.KeyDown
        Dim dep_idno As Integer = 0

        vcbo_KeyDwnVal = e.KeyValue

        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, Con, cbo_Ends2_FrontWarper2, Nothing, Nothing, "PayRoll_Employee_Head", "Employee_Name", "", "(Employee_IdNo = 0)")

        With dgv_WarpingDetails_Set2

            If (e.KeyValue = 38 And cbo_Ends2_FrontWarper2.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
                .Focus()
                .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex - 1)
                .CurrentCell.Selected = True
            End If

            If (e.KeyValue = 40 And cbo_Ends2_FrontWarper2.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
                .Focus()
                .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)
                .CurrentCell.Selected = True
            End If
            If e.Control = True And e.KeyValue = 13 Then
                With dgv_WarpingDetails_Set2
                    .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_Ends2_FrontWarper2.Text)
                    .Focus()
                    If .CurrentCell.RowIndex < .Rows.Count - 1 Then
                        .CurrentCell = .Rows(.CurrentCell.RowIndex + 1).Cells(1)
                    Else
                        .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)
                    End If
                    .CurrentCell.Selected = True
                    e.Handled = True
                    e.SuppressKeyPress = True
                End With
            End If
        End With

    End Sub

    Private Sub cbo_Ends2_FromtWarper2_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Ends2_FrontWarper2.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Dim f As New Payroll_Employee_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Ends2_FrontWarper2.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If
    End Sub

    Private Sub cbo_Ends2_FrontWarper2_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Ends2_FrontWarper2.TextChanged
        Try
            If cbo_Ends2_FrontWarper2.Visible Then
                With dgv_WarpingDetails_Set2
                    If Val(cbo_Ends2_FrontWarper2.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = 16 Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_Ends2_FrontWarper2.Text)
                    End If
                End With
            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try
    End Sub
    Private Sub cbo_Ends2_BackWarper2_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Ends2_BackWarper2.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, Con, cbo_Ends2_BackWarper2, Nothing, "PayRoll_Employee_Head", "Employee_Name", "", "(Employee_IdNo = 0)")
        If Asc(e.KeyChar) = 13 Then
            With dgv_WarpingDetails_Set2
                .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_Ends2_BackWarper2.Text)
                .Focus()
                .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)
                .CurrentCell.Selected = True
            End With
        End If
    End Sub

    Private Sub cbo_Ends2_BackWarper2_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Ends2_BackWarper2.KeyDown
        Dim dep_idno As Integer = 0

        vcbo_KeyDwnVal = e.KeyValue

        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, Con, cbo_Ends2_BackWarper2, Nothing, Nothing, "PayRoll_Employee_Head", "Employee_Name", "", "(Employee_IdNo = 0)")

        With dgv_WarpingDetails_Set2

            If (e.KeyValue = 38 And cbo_Ends2_BackWarper2.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
                .Focus()
                .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex - 1)
                .CurrentCell.Selected = True
            End If

            If (e.KeyValue = 40 And cbo_Ends2_BackWarper2.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
                .Focus()
                .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)
                .CurrentCell.Selected = True
            End If

            If e.Control = True And e.KeyValue = 13 Then
                With dgv_WarpingDetails_Set2
                    .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_Ends2_BackWarper2.Text)
                    .Focus()
                    If .CurrentCell.RowIndex < .Rows.Count - 1 Then
                        .CurrentCell = .Rows(.CurrentCell.RowIndex + 1).Cells(1)
                    Else
                        .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)
                    End If
                    .CurrentCell.Selected = True
                    e.Handled = True
                    e.SuppressKeyPress = True
                End With
            End If
        End With

    End Sub

    Private Sub cbo_Ends2_BackWarper2_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Ends2_BackWarper2.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Dim f As New Payroll_Employee_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Ends2_BackWarper2.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If
    End Sub

    Private Sub cbo_Ends2_BackWarper2_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Ends2_BackWarper2.TextChanged
        Try
            If cbo_Ends2_BackWarper2.Visible Then
                With dgv_WarpingDetails_Set2
                    If Val(cbo_Ends2_BackWarper2.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = 18 Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_Ends2_BackWarper2.Text)
                    End If
                End With
            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try
    End Sub
    Private Sub cbo_Ends2_Helper2_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Ends2_Helper2.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, Con, cbo_Ends2_Helper2, Nothing, "PayRoll_Employee_Head", "Employee_Name", "", "(Employee_IdNo = 0)")
        If Asc(e.KeyChar) = 13 Then
            With dgv_WarpingDetails_Set2
                .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_Ends2_Helper2.Text)
                .Focus()
                .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)
                .CurrentCell.Selected = True
            End With
        End If
    End Sub

    Private Sub cbo_Ends2_Helper2_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Ends2_Helper2.KeyDown
        Dim dep_idno As Integer = 0

        vcbo_KeyDwnVal = e.KeyValue

        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, Con, cbo_Ends2_Helper2, Nothing, Nothing, "PayRoll_Employee_Head", "Employee_Name", "", "(Employee_IdNo = 0)")

        With dgv_WarpingDetails_Set2

            If (e.KeyValue = 38 And cbo_Ends2_Helper2.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
                .Focus()
                .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex - 1)
                .CurrentCell.Selected = True
            End If

            If (e.KeyValue = 40 And cbo_Ends2_Helper2.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
                .Focus()
                .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)
                .CurrentCell.Selected = True
            End If

            If e.Control = True And e.KeyValue = 13 Then
                With dgv_WarpingDetails_Set2
                    .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_Ends2_Helper2.Text)
                    .Focus()
                    If .CurrentCell.RowIndex < .Rows.Count - 1 Then
                        .CurrentCell = .Rows(.CurrentCell.RowIndex + 1).Cells(1)
                    Else
                        .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)
                    End If
                    .CurrentCell.Selected = True
                    e.Handled = True
                    e.SuppressKeyPress = True
                End With
            End If
        End With

    End Sub

    Private Sub cbo_Ends2_Helper2_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Ends2_Helper2.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Dim f As New Payroll_Employee_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Ends2_Helper2.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If
    End Sub

    Private Sub cbo_Ends2_Helper2_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Ends2_Helper2.TextChanged
        Try
            If cbo_Ends2_Helper2.Visible Then
                With dgv_WarpingDetails_Set2
                    If Val(cbo_Ends2_Helper2.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = 20 Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_Ends2_Helper2.Text)
                    End If
                End With
            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try
    End Sub



    Private Sub cbo_Ends3_FrontWarper_1_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Ends3_FrontWarper1.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, Con, cbo_Grid_FrontWarper_1, Nothing, "PayRoll_Employee_Head", "Employee_Name", "", "(Employee_IdNo = 0)")
        If Asc(e.KeyChar) = 13 Then
            With dgv_WarpingDetails_Set3
                .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_Ends3_FrontWarper1.Text)
                .Focus()
                .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)
                .CurrentCell.Selected = True
            End With
        End If
    End Sub

    Private Sub cbo_Ends3_FrontWarper_1_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Ends3_FrontWarper1.KeyDown
        Dim dep_idno As Integer = 0

        vcbo_KeyDwnVal = e.KeyValue

        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, Con, cbo_Ends3_FrontWarper1, Nothing, Nothing, "PayRoll_Employee_Head", "Employee_Name", "", "(Employee_IdNo = 0)")

        With dgv_WarpingDetails_Set3

            If (e.KeyValue = 38 And cbo_Ends3_FrontWarper1.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
                .Focus()
                .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex - 1)
                .CurrentCell.Selected = True
            End If

            If (e.KeyValue = 40 And cbo_Ends3_FrontWarper1.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
                .Focus()
                .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)
                .CurrentCell.Selected = True
            End If
            If e.Control = True And e.KeyValue = 13 Then
                With dgv_WarpingDetails_Set3
                    .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_Ends3_FrontWarper1.Text)
                    .Focus()
                    If .CurrentCell.RowIndex < .Rows.Count - 1 Then
                        .CurrentCell = .Rows(.CurrentCell.RowIndex + 1).Cells(1)
                    Else
                        .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)
                    End If
                    .CurrentCell.Selected = True
                    e.Handled = True
                    e.SuppressKeyPress = True
                End With
            End If
        End With

    End Sub

    Private Sub cbo_Ends3_FromtWarper_1_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Ends3_FrontWarper1.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Dim f As New Payroll_Employee_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Ends3_FrontWarper1.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If
    End Sub

    Private Sub cbo_Ends3_FrontWarper_1_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Ends3_FrontWarper1.TextChanged
        Try
            If cbo_Ends3_FrontWarper1.Visible Then
                With dgv_WarpingDetails_Set3
                    If Val(cbo_Ends3_FrontWarper1.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = 10 Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_Ends3_FrontWarper1.Text)
                    End If
                End With
            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try
    End Sub


    Private Sub cbo_Ends3_BackWarper1_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Ends3_BackWarper1.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, Con, cbo_Ends3_BackWarper1, Nothing, "PayRoll_Employee_Head", "Employee_Name", "", "(Employee_IdNo = 0)")
        If Asc(e.KeyChar) = 13 Then
            With dgv_WarpingDetails_Set3
                .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_Ends3_BackWarper1.Text)
                .Focus()
                .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)
                .CurrentCell.Selected = True
            End With
        End If
    End Sub

    Private Sub cbo_Ends3_BackWarper1_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Ends3_BackWarper1.KeyDown
        Dim dep_idno As Integer = 0

        vcbo_KeyDwnVal = e.KeyValue

        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, Con, cbo_Ends3_BackWarper1, Nothing, Nothing, "PayRoll_Employee_Head", "Employee_Name", "", "(Employee_IdNo = 0)")

        With dgv_WarpingDetails_Set3

            If (e.KeyValue = 38 And cbo_Ends3_BackWarper1.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
                .Focus()
                .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex - 1)
                .CurrentCell.Selected = True
            End If

            If (e.KeyValue = 40 And cbo_Ends3_BackWarper1.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
                .Focus()
                .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)
                .CurrentCell.Selected = True
            End If

            If e.Control = True And e.KeyValue = 13 Then
                With dgv_WarpingDetails_Set3
                    .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_Ends3_BackWarper1.Text)
                    .Focus()
                    If .CurrentCell.RowIndex < .Rows.Count - 1 Then
                        .CurrentCell = .Rows(.CurrentCell.RowIndex + 1).Cells(1)
                    Else
                        .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)
                    End If
                    .CurrentCell.Selected = True
                    e.Handled = True
                    e.SuppressKeyPress = True
                End With
            End If
        End With

    End Sub

    Private Sub cbo_Ends3_BackWarper_1_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Ends3_BackWarper1.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Dim f As New Payroll_Employee_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Ends3_BackWarper1.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If
    End Sub

    Private Sub cbo_Ends3_BackWarper_1_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Ends3_BackWarper1.TextChanged
        Try
            If cbo_Ends3_BackWarper1.Visible Then
                With dgv_WarpingDetails_Set3
                    If Val(cbo_Ends3_BackWarper1.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = 10 Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_Ends3_BackWarper1.Text)
                    End If
                End With
            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try
    End Sub
    Private Sub cbo_Ends3_Helper_1_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Ends3_Helper1.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, Con, cbo_Ends3_Helper1, Nothing, "PayRoll_Employee_Head", "Employee_Name", "", "(Employee_IdNo = 0)")
        If Asc(e.KeyChar) = 13 Then
            With dgv_WarpingDetails_Set3
                .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_Ends3_Helper1.Text)
                .Focus()
                .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)
                .CurrentCell.Selected = True
            End With
        End If
    End Sub

    Private Sub cbo_Ends3_Helper_1_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Ends3_Helper1.KeyDown
        Dim dep_idno As Integer = 0

        vcbo_KeyDwnVal = e.KeyValue

        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, Con, cbo_Ends3_Helper1, Nothing, Nothing, "PayRoll_Employee_Head", "Employee_Name", "", "(Employee_IdNo = 0)")

        With dgv_WarpingDetails_Set3

            If (e.KeyValue = 38 And cbo_Ends3_Helper1.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
                .Focus()
                .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex - 1)
                .CurrentCell.Selected = True
            End If

            If (e.KeyValue = 40 And cbo_Ends3_Helper1.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
                .Focus()
                .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)
                .CurrentCell.Selected = True
            End If

            If e.Control = True And e.KeyValue = 13 Then
                With dgv_WarpingDetails_Set3
                    .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_Ends3_Helper1.Text)
                    .Focus()
                    If .CurrentCell.RowIndex < .Rows.Count - 1 Then
                        .CurrentCell = .Rows(.CurrentCell.RowIndex + 1).Cells(1)
                    Else
                        .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)
                    End If
                    .CurrentCell.Selected = True
                    e.Handled = True
                    e.SuppressKeyPress = True
                End With
            End If
        End With

    End Sub

    Private Sub cbo_Ends3_Helper1_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Ends3_Helper1.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Dim f As New Payroll_Employee_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Ends3_Helper1.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If
    End Sub

    Private Sub cbo_Ends3_Helper1_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Ends3_Helper1.TextChanged
        Try
            If cbo_Ends3_Helper1.Visible Then
                With dgv_WarpingDetails_Set3
                    If Val(cbo_Ends3_Helper1.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = 14 Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_Ends3_Helper1.Text)
                    End If
                End With
            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try
    End Sub


    Private Sub cbo_Ends3_FrontWarper2_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Ends3_FrontWarper2.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, Con, cbo_Ends3_FrontWarper2, Nothing, "PayRoll_Employee_Head", "Employee_Name", "", "(Employee_IdNo = 0)")
        If Asc(e.KeyChar) = 13 Then
            With dgv_WarpingDetails_Set3
                .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_Ends3_FrontWarper2.Text)
                .Focus()
                .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)
                .CurrentCell.Selected = True
            End With
        End If
    End Sub

    Private Sub cbo_Ends3_FrontWarper2_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Ends3_FrontWarper2.KeyDown
        Dim dep_idno As Integer = 0

        vcbo_KeyDwnVal = e.KeyValue

        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, Con, cbo_Ends3_FrontWarper2, Nothing, Nothing, "PayRoll_Employee_Head", "Employee_Name", "", "(Employee_IdNo = 0)")

        With dgv_WarpingDetails_Set3

            If (e.KeyValue = 38 And cbo_Ends3_FrontWarper2.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
                .Focus()
                .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex - 1)
                .CurrentCell.Selected = True
            End If

            If (e.KeyValue = 40 And cbo_Ends3_FrontWarper2.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
                .Focus()
                .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)
                .CurrentCell.Selected = True
            End If
            If e.Control = True And e.KeyValue = 13 Then
                With dgv_WarpingDetails_Set3
                    .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_Ends3_FrontWarper2.Text)
                    .Focus()
                    If .CurrentCell.RowIndex < .Rows.Count - 1 Then
                        .CurrentCell = .Rows(.CurrentCell.RowIndex + 1).Cells(1)
                    Else
                        .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)
                    End If
                    .CurrentCell.Selected = True
                    e.Handled = True
                    e.SuppressKeyPress = True
                End With
            End If
        End With

    End Sub

    Private Sub cbo_Ends3_FromtWarper2_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Ends3_FrontWarper2.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Dim f As New Payroll_Employee_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Ends3_FrontWarper2.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If
    End Sub

    Private Sub cbo_Ends3_FrontWarper2_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Ends3_FrontWarper2.TextChanged
        Try
            If cbo_Ends3_FrontWarper2.Visible Then
                With dgv_WarpingDetails_Set3
                    If Val(cbo_Ends3_FrontWarper2.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = 16 Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_Ends3_FrontWarper2.Text)
                    End If
                End With
            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try
    End Sub
    Private Sub cbo_Ends3_BackWarper2_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Ends3_BackWarper2.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, Con, cbo_Ends3_BackWarper2, Nothing, "PayRoll_Employee_Head", "Employee_Name", "", "(Employee_IdNo = 0)")
        If Asc(e.KeyChar) = 13 Then
            With dgv_WarpingDetails_Set3
                .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_Ends3_BackWarper2.Text)
                .Focus()
                .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)
                .CurrentCell.Selected = True
            End With
        End If
    End Sub

    Private Sub cbo_Ends3_BackWarper2_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Ends3_BackWarper2.KeyDown
        Dim dep_idno As Integer = 0

        vcbo_KeyDwnVal = e.KeyValue

        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, Con, cbo_Ends3_BackWarper2, Nothing, Nothing, "PayRoll_Employee_Head", "Employee_Name", "", "(Employee_IdNo = 0)")

        With dgv_WarpingDetails_Set3

            If (e.KeyValue = 38 And cbo_Ends3_BackWarper2.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
                .Focus()
                .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex - 1)
                .CurrentCell.Selected = True
            End If

            If (e.KeyValue = 40 And cbo_Ends3_BackWarper2.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
                .Focus()
                .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)
                .CurrentCell.Selected = True
            End If
            If e.Control = True And e.KeyValue = 13 Then
                With dgv_WarpingDetails_Set3
                    .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_Ends3_BackWarper2.Text)
                    .Focus()
                    If .CurrentCell.RowIndex < .Rows.Count - 1 Then
                        .CurrentCell = .Rows(.CurrentCell.RowIndex + 1).Cells(1)
                    Else
                        .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)
                    End If
                    .CurrentCell.Selected = True
                    e.Handled = True
                    e.SuppressKeyPress = True
                End With
            End If
        End With

    End Sub

    Private Sub cbo_Ends3_BackWarper2_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Ends3_BackWarper2.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Dim f As New Payroll_Employee_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Ends3_BackWarper2.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If
    End Sub

    Private Sub cbo_Ends3_BackWarper2_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Ends3_BackWarper2.TextChanged
        Try
            If cbo_Ends3_BackWarper2.Visible Then
                With dgv_WarpingDetails_Set3
                    If Val(cbo_Ends3_BackWarper2.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = 18 Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_Ends3_BackWarper2.Text)
                    End If
                End With
            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try
    End Sub
    Private Sub cbo_Ends3_Helper2_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Ends3_Helper2.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, Con, cbo_Ends2_Helper2, Nothing, "PayRoll_Employee_Head", "Employee_Name", "", "(Employee_IdNo = 0)")
        If Asc(e.KeyChar) = 13 Then
            With dgv_WarpingDetails_Set3
                .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_Ends3_Helper2.Text)
                .Focus()
                .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)
                .CurrentCell.Selected = True
            End With
        End If
    End Sub

    Private Sub cbo_Ends3_Helper2_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Ends3_Helper2.KeyDown
        Dim dep_idno As Integer = 0

        vcbo_KeyDwnVal = e.KeyValue

        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, Con, cbo_Ends3_Helper2, Nothing, Nothing, "PayRoll_Employee_Head", "Employee_Name", "", "(Employee_IdNo = 0)")

        With dgv_WarpingDetails_Set3

            If (e.KeyValue = 38 And cbo_Ends3_Helper2.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
                .Focus()
                .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex - 1)
                .CurrentCell.Selected = True
            End If

            If (e.KeyValue = 40 And cbo_Ends3_Helper2.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
                .Focus()
                .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)
                .CurrentCell.Selected = True
            End If

            If e.Control = True And e.KeyValue = 13 Then
                With dgv_WarpingDetails_Set3
                    .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_Ends3_Helper2.Text)
                    .Focus()
                    If .CurrentCell.RowIndex < .Rows.Count - 1 Then
                        .CurrentCell = .Rows(.CurrentCell.RowIndex + 1).Cells(1)
                    Else
                        .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)
                    End If
                    .CurrentCell.Selected = True
                    e.Handled = True
                    e.SuppressKeyPress = True
                End With
            End If

        End With

    End Sub

    Private Sub cbo_Ends3_Helper2_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Ends3_Helper2.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Dim f As New Payroll_Employee_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Ends3_Helper2.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If
    End Sub

    Private Sub cbo_Ends3_Helper2_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Ends3_Helper2.TextChanged
        Try
            If cbo_Ends3_Helper2.Visible Then
                With dgv_WarpingDetails_Set3
                    If Val(cbo_Ends3_Helper2.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = 20 Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_Ends3_Helper2.Text)
                    End If
                End With
            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try
    End Sub





    Private Sub cbo_Ends1_Shift_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Ends1_Shift.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, Con, cbo_Ends1_Shift, Nothing, "", "", "", "")
        If Asc(e.KeyChar) = 13 Then
            With dgv_WarpingDetails_Set1
                .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_Ends1_Shift.Text)
                .Focus()
                .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)
                .CurrentCell.Selected = True
            End With
        End If
    End Sub

    Private Sub cbo_Ends1_Shift_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Ends1_Shift.KeyDown
        Dim dep_idno As Integer = 0

        vcbo_KeyDwnVal = e.KeyValue

        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, Con, cbo_Ends1_Shift, Nothing, Nothing, "", "", "", "")

        With dgv_WarpingDetails_Set1

            If (e.KeyValue = 38 And cbo_Ends1_Shift.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
                .Focus()
                .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex - 1)
                .CurrentCell.Selected = True
            End If

            If (e.KeyValue = 40 And cbo_Ends1_Shift.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
                .Focus()
                .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)
                .CurrentCell.Selected = True
            End If

            If e.Control = True And e.KeyValue = 13 Then
                With dgv_WarpingDetails_Set1
                    .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_Ends1_Shift.Text)
                    .Focus()
                    If .CurrentCell.RowIndex < .Rows.Count - 1 Then
                        .CurrentCell = .Rows(.CurrentCell.RowIndex + 1).Cells(1)
                    Else
                        .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)
                    End If

                    .CurrentCell.Selected = True
                    e.Handled = True
                    e.SuppressKeyPress = True

                End With
            End If

            Ctrl_kyData = e.Control

        End With

    End Sub

    Private Sub cbo_Ends1_Shift_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Ends1_Shift.TextChanged
        Try
            If cbo_Ends1_Shift.Visible Then
                With dgv_WarpingDetails_Set1
                    If Val(cbo_Ends1_Shift.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = 5 Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_Ends1_Shift.Text)
                    End If
                End With
            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub cbo_Ends_Shift2_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Ends2_Shift.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, Con, cbo_Ends2_Shift, Nothing, "", "", "", "")
        If Asc(e.KeyChar) = 13 Then
            With dgv_WarpingDetails_Set2
                .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_Ends2_Shift.Text)
                .Focus()
                .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)
                .CurrentCell.Selected = True
            End With
        End If
    End Sub

    Private Sub cbo_Ends2_Shift_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Ends2_Shift.KeyDown
        Dim dep_idno As Integer = 0

        vcbo_KeyDwnVal = e.KeyValue

        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, Con, cbo_Ends2_Shift, Nothing, Nothing, "", "", "", "")

        With dgv_WarpingDetails_Set2

            If (e.KeyValue = 38 And cbo_Ends2_Shift.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
                .Focus()
                .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex - 1)
                .CurrentCell.Selected = True
            End If

            If (e.KeyValue = 40 And cbo_Ends2_Shift.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
                .Focus()
                .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)
                .CurrentCell.Selected = True
            End If

            If e.Control = True And e.KeyValue = 13 Then
                With dgv_WarpingDetails_Set2
                    .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_Ends2_Shift.Text)
                    .Focus()
                    If .CurrentCell.RowIndex < .Rows.Count - 1 Then
                        .CurrentCell = .Rows(.CurrentCell.RowIndex + 1).Cells(1)
                    Else
                        .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)
                    End If
                    .CurrentCell.Selected = True
                    e.Handled = True
                    e.SuppressKeyPress = True
                End With
            End If
        End With

    End Sub



    Private Sub cbo_Ends2_Shift_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Ends2_Shift.TextChanged
        Try
            If cbo_Ends2_Shift.Visible Then
                With dgv_WarpingDetails_Set2
                    If Val(cbo_Ends2_Shift.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = 5 Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_Ends2_Shift.Text)
                    End If
                End With
            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub cbo_Ends3_Shift1_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Cbo_Ends3_Shift.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, Con, Cbo_Ends3_Shift, Nothing, "", "", "", "")
        If Asc(e.KeyChar) = 13 Then
            With dgv_WarpingDetails_Set3
                .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(Cbo_Ends3_Shift.Text)
                .Focus()
                .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)
                .CurrentCell.Selected = True
            End With
        End If
    End Sub

    Private Sub cbo_Ends3_Shift_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Cbo_Ends3_Shift.KeyDown
        Dim dep_idno As Integer = 0

        vcbo_KeyDwnVal = e.KeyValue

        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, Con, Cbo_Ends3_Shift, Nothing, Nothing, "", "", "", "")

        With dgv_WarpingDetails_Set3

            If (e.KeyValue = 38 And Cbo_Ends3_Shift.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
                .Focus()
                .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex - 1)
                .CurrentCell.Selected = True
            End If

            If (e.KeyValue = 40 And Cbo_Ends3_Shift.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
                .Focus()
                .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)
                .CurrentCell.Selected = True
            End If
            If e.Control = True And e.KeyValue = 13 Then
                With dgv_WarpingDetails_Set3
                    .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(Cbo_Ends3_Shift.Text)
                    .Focus()
                    If .CurrentCell.RowIndex < .Rows.Count - 1 Then
                        .CurrentCell = .Rows(.CurrentCell.RowIndex + 1).Cells(1)
                    Else
                        .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)
                    End If
                    .CurrentCell.Selected = True
                    e.Handled = True
                    e.SuppressKeyPress = True
                End With
            End If
        End With

    End Sub



    Private Sub cbo_Ends3_Shift_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles Cbo_Ends3_Shift.TextChanged
        Try
            If Cbo_Ends3_Shift.Visible Then
                With dgv_WarpingDetails_Set3
                    If Val(Cbo_Ends3_Shift.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = 5 Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(Cbo_Ends3_Shift.Text)
                    End If
                End With
            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

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
        Dim Led_IdNo As Integer, Cnt_IdNo As Integer, Mil_IdNo As Integer
        Dim Condt As String = ""

        Try

            Condt = ""
            Led_IdNo = 0
            Cnt_IdNo = 0
            Mil_IdNo = 0

            If IsDate(dtp_Filter_Fromdate.Value) = True And IsDate(dtp_Filter_ToDate.Value) = True Then
                Condt = "a.Warp_Date between '" & Trim(Format(dtp_Filter_Fromdate.Value, "MM/dd/yyyy")) & "' and '" & Trim(Format(dtp_Filter_ToDate.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_Filter_Fromdate.Value) = True Then
                Condt = "a.Warp_Date = '" & Trim(Format(dtp_Filter_Fromdate.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_Filter_ToDate.Value) = True Then
                Condt = "a.Warp_Date = '" & Trim(Format(dtp_Filter_ToDate.Value, "MM/dd/yyyy")) & "' "
            End If

            If Trim(cbo_Filter_PartyName.Text) <> "" Then
                Led_IdNo = Common_Procedures.Ledger_AlaisNameToIdNo(Con, cbo_Filter_PartyName.Text)
            End If

            If Trim(cbo_Filter_CountName.Text) <> "" Then
                Cnt_IdNo = Common_Procedures.Count_NameToIdNo(Con, cbo_Filter_CountName.Text)
            End If

            If Trim(cbo_Filter_MillName.Text) <> "" Then
                Mil_IdNo = Common_Procedures.Mill_NameToIdNo(Con, cbo_Filter_MillName.Text)
            End If

            If Val(Led_IdNo) <> 0 Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & "a.Ledger_IdNo = " & Str(Val(Led_IdNo))
            End If

            If Val(Cnt_IdNo) <> 0 Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " a.Count_IdNo = " & Str(Val(Cnt_IdNo))
            End If

            If Val(Mil_IdNo) <> 0 Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " a.Mill_IdNo = " & Str(Val(Mil_IdNo))
            End If

            da = New SqlClient.SqlDataAdapter("select a.*, b.Ledger_Name from Warping_Head a INNER JOIN Ledger_Head b on a.Ledger_IdNo = b.Ledger_IdNo where a.company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Warp_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "' " & IIf(Trim(Condt) <> "", " and ", "") & Condt & " Order by a.for_orderby, a.Warp_No", Con)
            da.Fill(dt2)

            dgv_Filter_Details.Rows.Clear()

            If dt2.Rows.Count > 0 Then

                For i = 0 To dt2.Rows.Count - 1

                    n = dgv_Filter_Details.Rows.Add()

                    dgv_Filter_Details.Rows(n).Cells(0).Value = i + 1
                    dgv_Filter_Details.Rows(n).Cells(1).Value = dt2.Rows(i).Item("Warp_No").ToString
                    dgv_Filter_Details.Rows(n).Cells(2).Value = Format(Convert.ToDateTime(dt2.Rows(i).Item("Warp_Date").ToString), "dd-MM-yyyy")
                    dgv_Filter_Details.Rows(n).Cells(3).Value = dt2.Rows(i).Item("Ledger_Name").ToString
                    dgv_Filter_Details.Rows(n).Cells(4).Value = Val(dt2.Rows(i).Item("Warp_Meters").ToString)
                    dgv_Filter_Details.Rows(n).Cells(5).Value = Val(dt2.Rows(i).Item("Ends_Name").ToString)
                    dgv_Filter_Details.Rows(n).Cells(6).Value = Format(Val(dt2.Rows(i).Item("Total_Warping_Net_Weight").ToString), "########0.000")
                    '  dgv_Filter_Details.Rows(n).Cells(7).Value = Format(Val(dt2.Rows(i).Item("Total_Pavu_Net_Weight").ToString), "########0.000")
                    dgv_Filter_Details.Rows(n).Cells(7).Value = Format(Val(dt2.Rows(i).Item("Total_Baby_Net_Weight").ToString), "########0.000")
                    dgv_Filter_Details.Rows(n).Cells(8).Value = Format(Val(dt2.Rows(i).Item("Excess_Short_Yarn").ToString), "########0.000")
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


    Private Sub cbo_Filter_CountName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Filter_CountName.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, Con, cbo_Filter_CountName, cbo_Filter_PartyName, cbo_Filter_MillName, "Count_Head", "Count_Name", "", "(Count_IdNo = 0)")
    End Sub

    Private Sub cbo_Filter_CountName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Filter_CountName.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, Con, cbo_Filter_CountName, cbo_Filter_MillName, "Count_Head", "Count_Name", "", "(Count_IdNo = 0)")
    End Sub
    Private Sub cbo_Filter_MillName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Filter_MillName.KeyDown
        vcbo_KeyDwnVal = e.KeyValue

        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, Con, cbo_Filter_MillName, cbo_Filter_CountName, btn_Filter_Show, "Mill_Head", "Mill_Name", "", "(Mill_IdNo = 0)")

    End Sub

    Private Sub cbo_Filter_MillName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Filter_MillName.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, Con, cbo_Filter_MillName, btn_Filter_Show, "Mill_Head", "Mill_Name", "", "(Mill_IdNo = 0)")
        If Asc(e.KeyChar) = 13 Then
            btn_Filter_Show_Click(sender, e)
        End If
    End Sub

    Private Sub cbo_Filter_PartyName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Filter_PartyName.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, Con, cbo_Filter_PartyName, dtp_Filter_ToDate, cbo_Filter_CountName, "Ledger_AlaisHead", "Ledger_DisplayName", "(AccountsGroup_IdNo = 10 )", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_Filter_PartyName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Filter_PartyName.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, Con, cbo_Filter_PartyName, cbo_Filter_CountName, "Ledger_AlaisHead", "Ledger_DisplayName", "(AccountsGroup_IdNo = 10 )", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub dgtxt_WarpingDetails_Set1_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgtxt_WarpingDetails_Set1.TextChanged
        Try
            With dgv_WarpingDetails_Set1

                If .Visible Then
                    If .Rows.Count > 0 Then

                        .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(dgtxt_WarpingDetails_Set1.Text)

                    End If
                End If
            End With

        Catch ex As NullReferenceException
            '---MessageBox.Show(ex.Message, "ERROR WHILE DETAILS TEXTBOX TEXTCHANGE....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Catch ex As ObjectDisposedException
            '---MessageBox.Show(ex.Message, "ERROR WHILE DETAILS TEXTBOX TEXTCHANGE....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "ERROR WHILE DETAILS TEXTBOX TEXTCHANGE....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub cbo_Meters_Yards_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Meters_Yards.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, Con, cbo_Meters_Yards, cbo_CountName, txt_WarpMeters, "", "", "", "")
    End Sub

    Private Sub cbo_Meters_Yards_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Meters_Yards.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, Con, cbo_Meters_Yards, txt_WarpMeters, "", "", "", "")
    End Sub

    Private Sub cbo_Meters_Yards_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Meters_Yards.TextChanged
        If Trim(UCase(cbo_Meters_Yards.Text)) = "YARDS" Then
            lbl_Meters_Yards.Text = "Warp Yards"
        Else
            lbl_Meters_Yards.Text = "Warp Meters"
        End If
        BeamCount_Calculation()
    End Sub

    Private Sub txt_WgtEmYBag_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_WgtEmYBag.KeyDown
        If e.KeyCode = 40 Then SendKeys.Send("{Tab}") ' SendKeys.Send("{TAB}")
        If e.KeyCode = 38 Then
            SendKeys.Send("+{Tab}")
            dgv_BabyConeDetails.Focus()
            dgv_BabyConeDetails.CurrentCell = dgv_BabyConeDetails.Rows(0).Cells(1)
            dgv_BabyConeDetails.CurrentCell.Selected = True
        End If
    End Sub

    Private Sub txt_WgtEmYBag_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_WgtEmYBag.TextChanged
        BabyCone_TareWeight_Calculation(1)
    End Sub

    Private Sub txt_WgtEmYCone_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_WgtEmYCone.TextChanged
        BabyCone_TareWeight_Calculation(1)
    End Sub

    Private Sub BabyCone_TareWeight_Calculation(ByVal MustCal As Integer)
        Dim BagWgt As Double = 0
        Dim ConeWgt As Double = 0
        Dim TareWgt As Double = 0

        With dgv_BabyConeDetails_Total
            If Val(txt_WgtEmYBag.Text) <> 0 Or Val(txt_WgtEmYCone.Text) <> 0 Or MustCal = 1 Then
                BagWgt = Val(.Rows(0).Cells(1).Value) * Val(txt_WgtEmYBag.Text)
                ConeWgt = Val(.Rows(0).Cells(2).Value) * Val(txt_WgtEmYCone.Text)
                TareWgt = BagWgt + ConeWgt
                txt_BabyCone_TareWeight.Text = Format(Val(TareWgt), "#######0.0")
            End If
        End With
    End Sub

    Private Sub cbo_SetNo_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Grid_SetNo.GotFocus
        Dim NewCode As String
        Dim Led_ID As Integer, Cnt_ID As Integer
        Dim Condt As String
        Dim Cmp_Cond As String = ""

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_SetNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Led_ID = Common_Procedures.Ledger_AlaisNameToIdNo(Con, cbo_Ledger.Text)

        Cnt_ID = Common_Procedures.Count_NameToIdNo(Con, cbo_CountName.Text)

        Cmp_Cond = ""
        If Val(Common_Procedures.settings.StatementPrint_InStock_Combine_AllCompany) = 0 Then
            Cmp_Cond = "a.Company_IdNo = " & Str(Val(lbl_Company.Tag))
        End If

        Condt = "( " & Cmp_Cond & IIf(Trim(Cmp_Cond) <> "", " and ", "") & " a.Ledger_IdNo = " & Str(Val(Led_ID)) & " and a.Count_IdNo = " & Str(Val(Cnt_ID)) & " and (  (a.Baby_Weight - a.Delivered_Weight) > 0 or (a.Warpcode_forSelection IN (select z.BabyCone_Warpcode_forSelection from Warping_YarnTaken_Details z where z.company_idno = " & Str(Val(lbl_Company.Tag)) & " and z.Warp_Code = '" & Trim(NewCode) & "') ) ) )"

        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, Con, "Stock_BabyCone_Processing_Details a", "Warpcode_forSelection", Condt, "(Reference_Code = '')")

        cbo_Grid_SetNo.Tag = cbo_Grid_SetNo.Text

    End Sub

    Private Sub cbo_setno_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Grid_SetNo.KeyDown
        Dim NewCode As String
        Dim Led_ID As Integer, Cnt_ID As Integer
        Dim Condt As String
        Dim Cmp_Cond As String


        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_SetNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Led_ID = Common_Procedures.Ledger_AlaisNameToIdNo(Con, cbo_Ledger.Text)

        Cnt_ID = Common_Procedures.Count_NameToIdNo(Con, cbo_CountName.Text)

        Cmp_Cond = ""
        If Val(Common_Procedures.settings.StatementPrint_InStock_Combine_AllCompany) = 0 Then
            Cmp_Cond = "a.Company_IdNo = " & Str(Val(lbl_Company.Tag))
        End If

        Condt = "( " & Cmp_Cond & IIf(Trim(Cmp_Cond) <> "", " and ", "") & " a.Ledger_IdNo = " & Str(Val(Led_ID)) & " and a.Count_IdNo = " & Str(Val(Cnt_ID)) & " and (  (a.Baby_Weight - a.Delivered_Weight) > 0 or (a.Warpcode_forSelection IN (select z.BabyCone_Warpcode_forSelection from Warping_YarnTaken_Details z where z.company_idno = " & Str(Val(lbl_Company.Tag)) & " and z.Warp_Code = '" & Trim(NewCode) & "') ) ) )"

        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, Con, cbo_Grid_SetNo, Nothing, Nothing, "Stock_BabyCone_Processing_Details", "Warpcode_forSelection", Condt, "(Reference_Code = '')")
        With dgv_YarnTakenDetails

            If (e.KeyValue = 38 And cbo_Grid_SetNo.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
                .Focus()
                .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex - 1)
                .CurrentCell.Selected = True
            End If

            If (e.KeyValue = 40 And cbo_Grid_SetNo.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
                .Focus()
                .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)
                .CurrentCell.Selected = True
            End If

        End With
    End Sub

    Private Sub cbo_setno_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Grid_SetNo.KeyPress
        Dim NewCode As String
        Dim Led_ID As Integer, Cnt_ID As Integer
        Dim Condt As String
        Dim Cmp_Cond As String = ""

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_SetNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Led_ID = Common_Procedures.Ledger_AlaisNameToIdNo(Con, cbo_Ledger.Text)

        Cnt_ID = Common_Procedures.Count_NameToIdNo(Con, cbo_CountName.Text)

        Cmp_Cond = ""
        If Val(Common_Procedures.settings.StatementPrint_InStock_Combine_AllCompany) = 0 Then
            Cmp_Cond = "a.Company_IdNo = " & Str(Val(lbl_Company.Tag))
        End If

        Condt = "( " & Cmp_Cond & IIf(Trim(Cmp_Cond) <> "", " and ", "") & " a.Ledger_IdNo = " & Str(Val(Led_ID)) & " and a.Count_IdNo = " & Str(Val(Cnt_ID)) & " and (  (a.Baby_Weight - a.Delivered_Weight) > 0 or (a.Warpcode_forSelection IN (select z.BabyCone_Warpcode_forSelection from Warping_YarnTaken_Details z where z.company_idno = " & Str(Val(lbl_Company.Tag)) & " and z.Warp_Code = '" & Trim(NewCode) & "') ) ) )"

        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, Con, cbo_Grid_SetNo, Nothing, "Stock_BabyCone_Processing_Details a", "Warpcode_forSelection", Condt, "(Reference_Code = '')")
        If Asc(e.KeyChar) = 13 Then
            With dgv_YarnTakenDetails
                .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_Grid_SetNo.Text)
                .Focus()
                .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)
                .CurrentCell.Selected = True
            End With
        End If
    End Sub

    Private Sub cbo_Grid_SetNo_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Grid_SetNo.TextChanged
        Try
            If cbo_Grid_SetNo.Visible Then

                If IsNothing(dgv_YarnTakenDetails.CurrentCell) Then Exit Sub
                With dgv_YarnTakenDetails
                    If Val(cbo_Grid_SetNo.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = 3 Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_Grid_SetNo.Text)
                    End If
                End With
            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub dgv_YarnTakenDetails_CellMouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellMouseEventArgs) Handles dgv_YarnTakenDetails.CellMouseClick

    End Sub

    Private Sub dgtxt_YarnTakenDetails_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgtxt_YarnTakenDetails.TextChanged
        Try
            With dgv_YarnTakenDetails
                If .Visible Then
                    If .Rows.Count > 0 Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(dgtxt_YarnTakenDetails.Text)
                    End If
                End If
            End With

        Catch ex As NullReferenceException
            '---MessageBox.Show(ex.Message, "ERROR WHILE DETAILS TEXTBOX TEXTCHANGE....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Catch ex As ObjectDisposedException
            '---MessageBox.Show(ex.Message, "ERROR WHILE DETAILS TEXTBOX TEXTCHANGE....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "ERROR WHILE DETAILS TEXTBOX TEXTCHANGE....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub dgtxt_BabyConeDetails_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgtxt_BabyConeDetails.TextChanged
        Try
            With dgv_BabyConeDetails

                If .Visible Then
                    If .Rows.Count > 0 Then

                        .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(dgtxt_BabyConeDetails.Text)

                    End If
                End If
            End With

        Catch ex As NullReferenceException
            '---MessageBox.Show(ex.Message, "ERROR WHILE DETAILS TEXTBOX TEXTCHANGE....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Catch ex As ObjectDisposedException
            '---MessageBox.Show(ex.Message, "ERROR WHILE DETAILS TEXTBOX TEXTCHANGE....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "ERROR WHILE DETAILS TEXTBOX TEXTCHANGE....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub dgtxt_WarpingDetails_Set2_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgtxt_WarpingDetails_Set2.TextChanged
        Try
            With dgv_WarpingDetails_Set2

                If .Visible Then
                    If .Rows.Count > 0 Then

                        .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(dgtxt_WarpingDetails_Set2.Text)

                    End If
                End If
            End With

        Catch ex As NullReferenceException
            '---MessageBox.Show(ex.Message, "ERROR WHILE DETAILS TEXTBOX TEXTCHANGE....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Catch ex As ObjectDisposedException
            '---MessageBox.Show(ex.Message, "ERROR WHILE DETAILS TEXTBOX TEXTCHANGE....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "ERROR WHILE DETAILS TEXTBOX TEXTCHANGE....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub dgtxt_WarpingDetails_Set3_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgtxt_WarpingDetails_Set3.TextChanged
        Try
            With dgv_WarpingDetails_Set3

                If .Visible Then
                    If .Rows.Count > 0 Then

                        .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(dgtxt_WarpingDetails_Set3.Text)

                    End If
                End If
            End With

        Catch ex As NullReferenceException
            '---MessageBox.Show(ex.Message, "ERROR WHILE DETAILS TEXTBOX TEXTCHANGE....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Catch ex As ObjectDisposedException
            '---MessageBox.Show(ex.Message, "ERROR WHILE DETAILS TEXTBOX TEXTCHANGE....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "ERROR WHILE DETAILS TEXTBOX TEXTCHANGE....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try
    End Sub


    Private Sub cbo_MillName_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_MillName.TextChanged
        If cbo_MillName.Tag <> cbo_MillName.Text Then
            Get_EmptyBag_Cone_Weight()
        End If
    End Sub

    Private Sub Get_EmptyBag_Cone_Weight()

        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim Dt1 As New DataTable
        Dim CntID As Integer
        Dim MilID As Integer
        Dim Cns_Bg As Single, Wt_Cn As Single, Em_CnWgt As Single, Em_BgWgt As Single, Tot_TrWgt As Single

        On Error Resume Next

        MilID = Common_Procedures.Mill_NameToIdNo(Con, cbo_MillName.Text)

        Em_CnWgt = 0 : Em_BgWgt = 0
        If MilID <> 0 Then

            Da = New SqlClient.SqlDataAdapter("select * from Mill_hEAD where mill_idno = " & Str(Val(MilID)), Con)
            Da.Fill(Dt1)

            If Dt1.Rows.Count > 0 Then
                Em_CnWgt = Val(Dt1.Rows(0).Item("Weight_EmptyCone").ToString)
                Em_BgWgt = Val(Dt1.Rows(0).Item("Weight_EmptyBag").ToString)
            End If

            Dt1.Clear()
            Dt1.Dispose()
            Da.Dispose()
        End If

        If Em_BgWgt <> 0 Then
            txt_WgtEmYBag.Text = Format(Val(Em_BgWgt), "#########0.000")
        End If
        If Em_CnWgt <> 0 Then
            txt_WgtEmYCone.Text = Format(Val(Em_CnWgt), "#########0.000")
        End If

    End Sub

    Private Sub txt_BabyCone_AddLessWgt_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_BabyCone_AddLessWgt.TextChanged
        BabyCone_NetWeight_Calculation()
    End Sub


    Private Sub txt_ExcessShort_AddLess_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_ExcessShort_AddLess.TextChanged
        Excess_Calculation()
    End Sub

    Private Sub btn_SMS_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_SMS.Click
        'Dim request As HttpWebRequest
        'Dim response As HttpWebResponse = Nothing
        'Dim url As String
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt2 As New DataTable
        Dim NewCode As String
        Dim i As Integer = 0
        Dim smstxt As String = ""
        Dim PhNo As String = ""
        Dim Led_IdNo As Integer = 0

        Try

            Led_IdNo = Common_Procedures.Ledger_AlaisNameToIdNo(Con, cbo_Ledger.Text)

            PhNo = Common_Procedures.get_FieldValue(Con, "Ledger_Head", "Ledger_MobileNo", "(Ledger_IdNo = " & Str(Val(Led_IdNo)) & ")")

            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1102" Then

                smstxt = "WARPING DETAILS " & vbCrLf
                smstxt = smstxt & (cbo_Ledger.Text)
                smstxt = smstxt & vbCrLf & "DATE-" & Trim(dtp_Date.Text)
                smstxt = smstxt & vbCrLf & "Avg.Count-" & Trim(Val(lbl_BeamCount.Text))
                smstxt = smstxt & vbCrLf & "Cons.Yarn-" & Trim(Val(lbl_Total_Warping_NetWeight.Text))
                If Val(lbl_ExcessShort.Text) < 0 Then
                    smstxt = smstxt & vbCrLf & "Short-" & Trim(Math.Abs(Val(lbl_ExcessShort.Text)))
                Else
                    smstxt = smstxt & vbCrLf & "Excess-" & Trim(Val(lbl_ExcessShort.Text))
                End If
                smstxt = smstxt & vbCrLf & "Mill Name-" & Trim(cbo_MillName.Text)
                smstxt = smstxt & vbCrLf & "Warp Breaks-" & Trim(txt_Remarks.Text)

            ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1144" Then
                smstxt = "WARPING DETAILS " & vbCrLf
                smstxt = smstxt & (cbo_Ledger.Text)
                smstxt = smstxt & vbCrLf & "DATE-" & Trim(dtp_Date.Text)
                smstxt = smstxt & vbCrLf & "Avg.Count-" & Trim(Val(lbl_BeamCount.Text))
                smstxt = smstxt & vbCrLf & "Cons.Yarn-" & Trim(Val(lbl_Total_Warping_NetWeight.Text))
                If Val(lbl_ExcessShort.Text) < 0 Then
                    smstxt = smstxt & vbCrLf & "Short-" & Trim(Math.Abs(Val(lbl_ExcessShort.Text)))
                Else
                    smstxt = smstxt & vbCrLf & "Excess-" & Trim(Val(lbl_ExcessShort.Text))
                End If
                smstxt = smstxt & vbCrLf & "Mill Name-" & Trim(cbo_MillName.Text)


            Else
                smstxt = "WARPING STATEMENT " & vbCrLf
                smstxt = smstxt & "SETNO-" & Trim(lbl_SetNo.Text) & vbCrLf & "DATE-" & Trim(dtp_Date.Text)
                smstxt = smstxt & vbCrLf & "Avg.Count-" & Trim(Val(lbl_BeamCount.Text))
                smstxt = smstxt & vbCrLf & "Cons.Yarn-" & Trim(Val(lbl_Total_Warping_NetWeight.Text))
                If Val(lbl_ExcessShort.Text) < 0 Then
                    smstxt = smstxt & vbCrLf & "Short-" & Trim(Math.Abs(Val(lbl_ExcessShort.Text)))
                Else
                    smstxt = smstxt & vbCrLf & "Excess-" & Trim(Val(lbl_ExcessShort.Text))
                End If
                smstxt = smstxt & vbCrLf & "Mill Name-" & Trim(cbo_MillName.Text)

            End If

            NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_SetNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)


            da2 = New SqlClient.SqlDataAdapter("select sum(a.Bags) as TotalBags from Warping_YarnTaken_Details a  where a.Yarn_Type = 'MILL' and  a.Warp_Code = '" & Trim(NewCode) & "'", Con)
            dt2 = New DataTable
            da2.Fill(dt2)

            If dt2.Rows.Count > 0 Then
                For i = 0 To dt2.Rows.Count - 1
                    If Val(dt2.Rows(i).Item("TotalBags").ToString) <> 0 Then
                        smstxt = smstxt & vbCrLf & "Mill Bags -" & Trim(Val(dt2.Rows(i).Item("TotalBags").ToString))
                    End If
                Next i
            End If
            dt2.Clear()

            smstxt = smstxt & vbCrLf & " Thanks! " & vbCrLf
            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1102" Then
                smstxt = smstxt & "GKT SIZING "
            Else '
                smstxt = smstxt & Common_Procedures.Company_IdNoToName(Con, Val(lbl_Company.Tag))
            End If

            If Common_Procedures.settings.CustomerCode = "1102" Then
                Sms_Entry.vSmsPhoneNo = Trim(PhNo) & "," & "9361188135"
            Else
                Sms_Entry.vSmsPhoneNo = Trim(PhNo)
            End If

            Sms_Entry.vSmsMessage = Trim(smstxt)

            Dim f1 As New Sms_Entry


            f1.MdiParent = MDIParent1
            f1.Show()

            ' ''url = "http://sms.shamsoft.in/app/smsapi/index.php?key=355C7A0B5595B2&routeid=73&type=text&contacts=" & Trim(PhNo) & "&senderid=WEBSMS&msg=" & Trim(smstxt)

            ' ''--THIS IS Working (jenilla)
            ''url = "http://103.16.101.52:8080/bulksms/bulksms?username=nila-jenial&password=nila123&type=0&dlr=1&destination=918508403222&source=JENIAL&message=" & Trim(smstxt)

            ' ''THIS IS OK
            ' ''url = "http://sms.shamsoft.in/app/smsapi/index.php?key=355C7A0B5595B2&routeid=73&type=text&contacts=8508403222&senderid=WEBSMS&msg=Hello+People%2C+have+a+great+day"

            ' ''url = "http://sms.shamsoft.in/app/smsapi/index.php?key=355C7A0B5595B2&routeid=14&type=text&contacts=97656XXXXX,98012XXXXX&senderid=DEMO&msg=Hello+People%2C+have+a+great+day"

            ' ''url = "http://103.16.101.52:8080/bulksms/bulksms?username=nila-jenial&password=nila123&type=0&dlr=1&destination=918508403222&source=JENIAL&message=testmsg"

            ' ''url = "http://103.16.101.52:8080/bulksms/bulksms?username=nila-jenial&password=nila123&type=0&dlr=1&destination=918508403222&source=JENIAL&message=testmsg"

            ''request = DirectCast(WebRequest.Create(url), HttpWebRequest)

            ''response = DirectCast(request.GetResponse(), HttpWebResponse)

            ''If Trim(UCase(response.StatusDescription)) = "OK" Then
            ''    MessageBox.Show("Sucessfully Sent...", "FOR SENDING SMS...", MessageBoxButtons.OKCancel, MessageBoxIcon.Information)
            ''    'MessageBox.Show("Response: " & response.StatusDescription)
            ''Else
            ''    MessageBox.Show("Failed to sent SMS...", "FOR SENDING SMS...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            ''End If

            ' ''WebBrowser1.Navigate("http://103.16.101.52:8080/bulksms/bulksms?username=nila-jenial&password=nila123&type=0&dlr=1&destination=918508403222&source=JENIAL&message=testmsg")
            ' ''MsgBox("sms send")

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT SEND SMS...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub


    Private Sub PrintDocument1_BeginPrint(ByVal sender As Object, ByVal e As System.Drawing.Printing.PrintEventArgs) Handles PrintDocument1.BeginPrint
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim NewCode As String

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_SetNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        prn_HdDt.Clear()
        prn_DetDt.Clear()
        prn_DetIndx = 0
        prn_DetSNo = 0
        prn_PageNo = 0
        prn_Count = 0
        Try

            da1 = New SqlClient.SqlDataAdapter("Select a.*, b.*, c.*, Csh.State_Name as Company_State_Name, Csh.State_Code as Company_State_Code, Lsh.State_Name as Ledger_State_Name, Lsh.State_Code as Ledger_State_Code,c.Ledger_Name as DelName , c.Ledger_Address1 as DelAdd1 ,c.Ledger_Address2 as DelAdd2, c.Ledger_Address3 as DelAdd3 ,c.Ledger_Address4 as DelAdd4,c.Ledger_GSTinNo as DelGSTinNo,DSH.State_Name as DelState_Name ,DSH.State_Code as Delivery_State_Code from Warping_Head a INNER JOIN Company_Head b ON a.Company_IdNo <> 0 and a.Company_IdNo = b.Company_IdNo  LEFT OUTER JOIN State_Head Csh ON b.Company_State_IdNo = csh.State_IdNo INNER JOIN Ledger_Head c ON a.Ledger_IdNo <> 0 and a.Ledger_IdNo = c.Ledger_IdNo LEFT OUTER JOIN State_Head Lsh ON c.ledger_State_IdNo = Lsh.State_IdNo LEFT OUTER JOIN State_HEad DSH on c.Ledger_State_IdNo = DSH.State_IdNo  where a.company_idno = " & Str(Val(lbl_Company.Tag)) & " and a.Warp_Code = '" & Trim(NewCode) & "'", Con)
            prn_HdDt = New DataTable
            da1.Fill(prn_HdDt)

            If prn_HdDt.Rows.Count > 0 Then

                da2 = New SqlClient.SqlDataAdapter("select a.*, b.Mill_Name, c.* from Warping_YarnTaken_Details a INNER JOIN Mill_Head b on a.Mill_IdNo = b.Mill_IdNo INNER JOIN Count_Head c on a.Count_IdNo = c.Count_IdNo  where a.company_idno = " & (Val(lbl_Company.Tag)) & " and a.Warp_Code = '" & Trim(NewCode) & "' Order by a.sl_no", Con)
                prn_DetDt = New DataTable
                da2.Fill(prn_DetDt)

                da2.Dispose()

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
        ' If prn_Status = 1 Then
        '  If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1288" Then
        Printing_Format1(e)

        'End If
        ' End If
    End Sub

    Private Sub Printing_Format1(ByRef e As System.Drawing.Printing.PrintPageEventArgs)
        Dim EntryCode As String
        Dim I As Integer, NoofDets As Integer, NoofItems_PerPage As Integer
        Dim pFont As Font
        Dim LMargin As Single, RMargin As Single, TMargin As Single, BMargin As Single
        Dim PrintWidth As Single, PrintHeight As Single
        Dim PageWidth As Single, PageHeight As Single
        Dim CurY As Single, TxtHgt As Single
        Dim LnAr(15) As Single, ClArr(15) As Single
        Dim ItmNm1 As String, ItmNm2 As String
        Dim CtmNm1 As String, CtmNm2 As String
        'Dim ps As Printing.PaperSize
        Dim strHeight As Single = 0
        Dim PpSzSTS As Boolean = False
        Dim PS As Printing.PaperSize

        Dim PCnt As Integer = 0, PrntCnt As Integer = 0
        Dim TpMargin As Single = 0


        PrntCnt = 1
        If Val(Common_Procedures.settings.YarnDelivery_Print_2Copy_In_SinglePage) = 1 Then
            For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
                If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
                    PS = PrintDocument1.PrinterSettings.PaperSizes(I)
                    PrintDocument1.DefaultPageSettings.PaperSize = PS
                    e.PageSettings.PaperSize = PS
                    Exit For
                End If
            Next


        Else

            If PpSzSTS = False Then
                For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
                    If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
                        PS = PrintDocument1.PrinterSettings.PaperSizes(I)
                        PrintDocument1.DefaultPageSettings.PaperSize = PS
                        e.PageSettings.PaperSize = PS
                        Exit For
                    End If
                Next


            Else
                For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
                    If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A5 Then
                        PS = PrintDocument1.PrinterSettings.PaperSizes(I)
                        PrintDocument1.DefaultPageSettings.PaperSize = PS
                        e.PageSettings.PaperSize = PS
                        PpSzSTS = True
                        Exit For
                    End If
                Next
            End If

        End If

        If Val(Common_Procedures.settings.YarnDelivery_Print_2Copy_In_SinglePage) = 1 Then
            If PrntCnt2ndPageSTS = False Then
                PrntCnt = 2
            End If
        End If

        With PrintDocument1.DefaultPageSettings.Margins
            .Left = 10
            .Right = 30
            .Top = 30
            .Bottom = 30
            LMargin = .Left
            RMargin = .Right
            TMargin = .Top
            BMargin = .Bottom
        End With

        pFont = New Font("Calibri", 8, FontStyle.Regular)

        e.Graphics.TextRenderingHint = Drawing.Text.TextRenderingHint.AntiAlias

        With PrintDocument1.DefaultPageSettings.PaperSize
            PrintWidth = .Width - RMargin - LMargin
            PrintHeight = .Height - TMargin - BMargin
            PageWidth = .Width - RMargin
            PageHeight = .Height - BMargin
        End With

        If Common_Procedures.settings.CustomerCode = "1288" Then
            PrintDocument1.DefaultPageSettings.Landscape = True
        End If

        If PrintDocument1.DefaultPageSettings.Landscape = True Then
            With PrintDocument1.DefaultPageSettings.PaperSize
                PrintWidth = .Height - TMargin - BMargin
                PrintHeight = .Width - RMargin - LMargin
                PageWidth = .Height - TMargin
                PageHeight = .Width - RMargin
            End With
        End If


        If (Trim(UCase(Common_Procedures.settings.CustomerCode))) = "1288" Then
            NoofItems_PerPage = 13 ' 6 ' 5
        Else
            NoofItems_PerPage = 4 ' 6 ' 5
        End If

        Erase LnAr
        Erase ClArr

        LnAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        ClArr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}

        ClArr(1) = 30
        ClArr(2) = 50 : ClArr(3) = 50 : ClArr(4) = 250 : ClArr(5) = 200 : ClArr(6) = 80 : ClArr(7) = 90 : ClArr(8) = 80
        ClArr(9) = 65
        'ClArr(11) = PageWidth - (LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + ClArr(9) + ClArr(10))

        'ClArr(11) = 100

        'ClArr(12) = 95
        'ClArr(13) = 95
        'ClArr(13) = PageWidth - (LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + ClArr(9) + ClArr(10) + ClArr(11) + ClArr(12))






        TxtHgt = 18 ' 18.8  ' e.Graphics.MeasureString("A", pFont).Height  ' 20

        EntryCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_SetNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)
        PrntCnt2ndPageSTS = False
        TpMargin = TMargin

        For PCnt = 1 To PrntCnt
            If Val(Common_Procedures.settings.YarnDelivery_Print_2Copy_In_SinglePage) = 1 Then
                If PCnt = 1 Then
                    prn_PageNo = 0

                    prn_DetIndx = 0
                    prn_DetSNo = 0
                    ' prn_NoofBmDets = 0
                    TpMargin = TMargin

                Else

                    prn_PageNo = 0
                    ' prn_NoofBmDets = 0
                    prn_DetIndx = 0
                    prn_DetSNo = 0

                    TpMargin = 580 + TMargin  ' 600 + TMargin

                End If
            End If


            Try

                If prn_HdDt.Rows.Count > 0 Then

                    Printing_Format1_PageHeader(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TpMargin, BMargin, PageWidth - 50, PrintWidth, prn_PageNo, NoofItems_PerPage, CurY, LnAr, ClArr)

                    Try

                        NoofDets = 0

                        CurY = CurY - 10

                        If prn_DetDt.Rows.Count > 0 Then

                            sum_Total_Amount = 0

                            Do While prn_DetIndx <= prn_DetDt.Rows.Count - 1

                                If NoofDets >= NoofItems_PerPage Then
                                    CurY = CurY + TxtHgt

                                    Common_Procedures.Print_To_PrintDocument(e, "Continued....", PageWidth - 10, CurY, 1, 0, pFont)

                                    NoofDets = NoofDets + 1

                                    Printing_Format1_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth - 50, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, NoofDets, False)

                                    e.HasMorePages = True
                                    Return

                                End If

                                prn_DetSNo = prn_DetSNo + 1

                                ItmNm1 = Trim(prn_DetDt.Rows(prn_DetIndx).Item("Mill_Name").ToString)
                                ItmNm2 = ""
                                If Len(ItmNm1) > 35 Then
                                    For I = 35 To 1 Step -1
                                        If Mid$(Trim(ItmNm1), I, 1) = " " Or Mid$(Trim(ItmNm1), I, 1) = "," Or Mid$(Trim(ItmNm1), I, 1) = "." Or Mid$(Trim(ItmNm1), I, 1) = "-" Or Mid$(Trim(ItmNm1), I, 1) = "/" Or Mid$(Trim(ItmNm1), I, 1) = "_" Or Mid$(Trim(ItmNm1), I, 1) = "(" Or Mid$(Trim(ItmNm1), I, 1) = ")" Or Mid$(Trim(ItmNm1), I, 1) = "\" Or Mid$(Trim(ItmNm1), I, 1) = "[" Or Mid$(Trim(ItmNm1), I, 1) = "]" Or Mid$(Trim(ItmNm1), I, 1) = "{" Or Mid$(Trim(ItmNm1), I, 1) = "}" Then Exit For
                                    Next I
                                    If I = 0 Then I = 35
                                    ItmNm2 = Microsoft.VisualBasic.Right(Trim(ItmNm1), Len(ItmNm1) - I)
                                    ItmNm1 = Microsoft.VisualBasic.Left(Trim(ItmNm1), I - 1)
                                End If

                                CtmNm1 = Trim(prn_DetDt.Rows(prn_DetIndx).Item("Count_Name").ToString)
                                CtmNm2 = ""
                                If Len(CtmNm1) > 10 Then
                                    For I = 10 To 1 Step -1
                                        If Mid$(Trim(CtmNm1), I, 1) = " " Or Mid$(Trim(CtmNm1), I, 1) = "," Or Mid$(Trim(CtmNm1), I, 1) = "." Or Mid$(Trim(CtmNm1), I, 1) = "-" Or Mid$(Trim(CtmNm1), I, 1) = "/" Or Mid$(Trim(CtmNm1), I, 1) = "_" Or Mid$(Trim(CtmNm1), I, 1) = "(" Or Mid$(Trim(CtmNm1), I, 1) = ")" Or Mid$(Trim(CtmNm1), I, 1) = "\" Or Mid$(Trim(CtmNm1), I, 1) = "[" Or Mid$(Trim(CtmNm1), I, 1) = "]" Or Mid$(Trim(CtmNm1), I, 1) = "{" Or Mid$(Trim(CtmNm1), I, 1) = "}" Then Exit For
                                    Next I
                                    If I = 0 Then I = 10
                                    CtmNm2 = Microsoft.VisualBasic.Right(Trim(CtmNm1), Len(CtmNm1) - I)
                                    CtmNm1 = Microsoft.VisualBasic.Left(Trim(CtmNm1), I - 1)
                                End If


                                CurY = CurY + TxtHgt



                                Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(prn_DetIndx).Item("Sl_No").ToString), LMargin + 25, CurY, 1, 0, pFont)
                                Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Yarn_Type").ToString, LMargin + ClArr(1) + 10, CurY, 0, 0, pFont)
                                If IsDBNull(prn_DetDt.Rows(prn_DetIndx).Item("Warp_no").ToString) = False Then
                                    If Trim(prn_DetDt.Rows(prn_DetIndx).Item("Warp_no").ToString) <> "" Then
                                        Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetDt.Rows(prn_DetIndx).Item("Warp_no").ToString), LMargin + ClArr(1) + ClArr(2) + 10, CurY, 0, 0, pFont)
                                    End If
                                End If
                                Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm1), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + 10, CurY, 0, 0, pFont)

                                Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Count_Name").ToString, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + 5, CurY, 0, 0, pFont)

                                Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Weight_Cone").ToString, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + 10, CurY, 0, 0, pFont)


                                Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("cones").ToString, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) - 10, CurY, 1, 0, pFont)
                                Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Weight").ToString), "########0.000"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) - 10, CurY, 1, 0, pFont)


                                Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Lot_No").ToString, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + ClArr(9) - 10, CurY, 1, 0, pFont)

                                Common_Procedures.Print_To_PrintDocument(e, Common_Procedures.Ledger_IdNoToName(Con, Val(prn_DetDt.Rows(prn_DetIndx).Item("Location_IdNo").ToString)), PageWidth - 130, CurY, 1, 0, pFont)



                                '   sum_Total_Amount += Val(prn_DetDt.Rows(prn_DetIndx).Item("Amount").ToString)


                                NoofDets = NoofDets + 1

                                'If Trim(ItmNm2) <> "" Or Trim(CtmNm2) <> "" Then
                                '    CurY = CurY + TxtHgt - 5
                                '    Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm2), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + 10, CurY, 0, 0, pFont)
                                '    NoofDets = NoofDets + 1
                                '    Common_Procedures.Print_To_PrintDocument(e, Trim(CtmNm2), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + 5, CurY, 0, 0, pFont)
                                '    NoofDets = NoofDets + 1
                                'End If

                                prn_DetIndx = prn_DetIndx + 1

                            Loop

                        End If

                        Printing_Format1_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth - 50, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, NoofDets, True)

                    Catch ex As Exception

                        MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

                    End Try

                End If

            Catch ex As Exception

                MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

            End Try
            If Val(Common_Procedures.settings.YarnDelivery_Print_2Copy_In_SinglePage) = 1 Then
                If PCnt = 1 And PrntCnt = 2 And PrntCnt2ndPageSTS = False Then
                    If prn_DetDt.Rows.Count > 4 Then
                        PrntCnt2ndPageSTS = True
                        e.HasMorePages = True
                        Return
                    End If
                End If
            End If
            PrntCnt2ndPageSTS = False

        Next PCnt
LOOP2:

        prn_Count = prn_Count + 1

        e.HasMorePages = False

        If Val(prn_TotCopies) > 1 Then
            If prn_Count < Val(prn_TotCopies) Then

                prn_DetIndx = 0
                prn_PageNo = 0
                prn_DetIndx = 0
                prn_PageNo = 0
                'prn_NoofBmDets = 0


                e.HasMorePages = True
                Return

            End If

        End If


    End Sub



    Private Sub Printing_Format1_PageHeader(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByRef PageNo As Integer, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single)
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt2 As New DataTable
        Dim dt3 As New DataTable
        Dim Cmp_Name As String, Cmp_Add1 As String, Cmp_Add2 As String, Cmp_Add3 As String, Cmp_Add4 As String
        Dim Cmp_PhNo As String, Cmp_TinNo As String, Cmp_CstNo As String, Cmp_Email As String
        Dim Cmp_StateCap As String, Cmp_StateNm As String, Cmp_StateCode As String, Cmp_GSTIN_Cap As String, Cmp_GSTIN_No As String
        Dim p1Font As Font
        Dim strHeight As Single = 0, strWidth As Single = 0
        Dim W1 As Single, C1 As Single, S1 As Single, K1 As Single, M1 As Single
        Dim CurX As Single = 0
        Dim Hsn_Code As String = ""
        Dim Led_Name As String, Led_Add1 As String, Led_Add2 As String, Led_Add3 As String, Led_Add4 As String
        Dim Led_GstNo As String

        PageNo = PageNo + 1

        CurY = TMargin

        da2 = New SqlClient.SqlDataAdapter("select a.*, b.Mill_Name, c.Count_Name  from Warping_YarnTaken_Details a INNER JOIN Mill_Head b on a.Mill_IdNo = b.Mill_IdNo LEFT OUTER JOIN Count_Head c on a.Count_IdNo = c.Count_IdNo where a.Warp_Code = '" & Trim(EntryCode) & "' and c.Count_Name = '" & Trim(dgv_YarnTakenDetails.Rows(0).Cells(1).Value) & "' Order by a.sl_no", Con)
        da2.Fill(dt2)
        If dt2.Rows.Count > NoofItems_PerPage Then
            Common_Procedures.Print_To_PrintDocument(e, "Page : " & Trim(Val(PageNo)), PageWidth - 10, CurY - TxtHgt, 1, 0, pFont)

        End If
        dt2.Clear()
        da2 = New SqlClient.SqlDataAdapter("select a.*, b.Mill_Name, c.*  from Warping_YarnTaken_Details a INNER JOIN Mill_Head b on a.Mill_IdNo = b.Mill_IdNo LEFT OUTER JOIN Count_Head c on a.Count_IdNo = c.Count_IdNo where a.Warp_Code = '" & Trim(EntryCode) & "' and c.Count_Name = '" & Trim(dgv_YarnTakenDetails.Rows(0).Cells(1).Value) & "' Order by a.sl_no", Con)
        da2.Fill(dt3)
        If dt3.Rows.Count > 0 Then

            Hsn_Code = dt3.Rows(0).Item("Count_Hsn_Code").ToString
        End If
        dt3.Clear()

        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(1) = CurY

        Cmp_Name = "" : Cmp_Add1 = "" : Cmp_Add2 = "" : Cmp_Add3 = "" : Cmp_Add4 = ""
        Cmp_PhNo = "" : Cmp_TinNo = "" : Cmp_CstNo = "" : Cmp_Email = ""
        Cmp_StateCap = "" : Cmp_StateNm = "" : Cmp_StateCode = "" : Cmp_GSTIN_Cap = "" : Cmp_GSTIN_No = ""
        Led_Name = "" : Led_Add1 = "" : Led_Add2 = "" : Led_Add3 = "" : Led_Add4 = "" : Led_GstNo = ""

        Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString
        Cmp_Add1 = prn_HdDt.Rows(0).Item("Company_Address1").ToString
        Cmp_Add2 = prn_HdDt.Rows(0).Item("Company_Address2").ToString
        Cmp_Add3 = prn_HdDt.Rows(0).Item("Company_Address3").ToString
        Cmp_Add4 = prn_HdDt.Rows(0).Item("Company_Address4").ToString

        Led_Name = prn_HdDt.Rows(0).Item("Ledger_Name").ToString
        Led_Add1 = prn_HdDt.Rows(0).Item("Ledger_Address1").ToString
        Led_Add2 = prn_HdDt.Rows(0).Item("Ledger_Address2").ToString
        Led_Add3 = prn_HdDt.Rows(0).Item("Ledger_Address3").ToString
        Led_Add4 = prn_HdDt.Rows(0).Item("Ledger_Address4").ToString

        If Trim(prn_HdDt.Rows(0).Item("Company_PhoneNo").ToString) <> "" Then
            Cmp_PhNo = "PHONE NO.:" & prn_HdDt.Rows(0).Item("Company_PhoneNo").ToString
        End If
        If Trim(prn_HdDt.Rows(0).Item("Company_TinNo").ToString) <> "" Then
            Cmp_TinNo = "TIN NO.: " & prn_HdDt.Rows(0).Item("Company_TinNo").ToString
        End If
        If Trim(prn_HdDt.Rows(0).Item("Company_CstNo").ToString) <> "" Then
            Cmp_CstNo = "CST NO.: " & prn_HdDt.Rows(0).Item("Company_CstNo").ToString
        End If
        If Trim(prn_HdDt.Rows(0).Item("Company_EMail").ToString) <> "" Then
            Cmp_Email = "E-mail : " & Trim(prn_HdDt.Rows(0).Item("Company_EMail").ToString)
        End If
        If Trim(prn_HdDt.Rows(0).Item("Company_State_Name").ToString) <> "" Then
            Cmp_StateCap = "STATE : "
            Cmp_StateNm = prn_HdDt.Rows(0).Item("Company_State_Name").ToString
        End If
        If Trim(prn_HdDt.Rows(0).Item("Company_State_Code").ToString) <> "" Then
            Cmp_StateCode = "CODE :" & prn_HdDt.Rows(0).Item("Company_State_Code").ToString
        End If
        If Trim(prn_HdDt.Rows(0).Item("Company_GSTinNo").ToString) <> "" Then
            Cmp_GSTIN_Cap = "GSTIN : "
            Cmp_GSTIN_No = "GSTIN : " & prn_HdDt.Rows(0).Item("Company_GSTinNo").ToString
        End If
        If Trim(prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString) <> "" Then

            Led_GstNo = "GSTIN : " & prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString
        End If


        CurY = CurY + TxtHgt - 10
        M1 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6)
        If (Common_Procedures.settings.CustomerCode) = "1282" Then
            p1Font = New Font("Calibri", 12, FontStyle.Bold)
        Else
            p1Font = New Font("Calibri", 14, FontStyle.Bold)
        End If
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Name, LMargin + 10, CurY, 0, 0, p1Font)
        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height
        'Common_Procedures.Print_To_PrintDocument(e, "YARN SIZED TO :", LMargin + M1 + 10, CurY, 0, 0, pFont)

        ' p1Font = New Font("Calibri", 9, FontStyle.Regular)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Add1, LMargin + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, "M/s." & Led_Name, LMargin + M1 + 10, CurY, 0, 0, p1Font, , True, PageWidth)
        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Add2, LMargin + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, Led_Add1, LMargin + M1 + 10, CurY, 0, 0, pFont)
        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Add3, LMargin + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, Led_Add2, LMargin + M1 + 10, CurY, 0, 0, pFont)
        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Add4, LMargin + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, Led_Add3, LMargin + M1 + 10, CurY, 0, 0, pFont)
        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, Cmp_PhNo, LMargin + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, Led_Add4, LMargin + M1 + 10, CurY, 0, 0, pFont)
        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, Cmp_GSTIN_No, LMargin + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, Led_GstNo, LMargin + M1 + 10, CurY, 0, 0, pFont)
        CurY = CurY + TxtHgt
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(2) = CurY
        e.Graphics.DrawLine(Pens.Black, LMargin + M1, LnAr(2), LMargin + M1, LnAr(1))


        CurY = CurY + TxtHgt - 12
        p1Font = New Font("Calibri", 12, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "WARPING STATEMENT", LMargin, CurY, 2, PrintWidth, p1Font)
        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height


        Try

            C1 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4)
            W1 = e.Graphics.MeasureString("BOOK NO            : ", pFont).Width
            K1 = e.Graphics.MeasureString("SIZING BOOK NO : ", pFont).Width
            S1 = e.Graphics.MeasureString("TO    :", pFont).Width

            CurY = CurY + TxtHgt

            Common_Procedures.Print_To_PrintDocument(e, "SET.NO", LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 10, CurY, 0, 0, pFont)
            p1Font = New Font("Calibri", 9, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Warp_no").ToString, LMargin + W1 + 25, CurY, 0, 0, p1Font)




            CurY = CurY + TxtHgt

            Common_Procedures.Print_To_PrintDocument(e, "DATE & TIME", LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, (prn_HdDt.Rows(0).Item("WArp_date").ToString).ToString, LMargin + W1 + 25, CurY, 0, 0, pFont)
            '  Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Entry_Time_Text").ToString, PageWidth, CurY, 1, 0, pFont)



            CurY = CurY + TxtHgt

            Common_Procedures.Print_To_PrintDocument(e, "HSN CODE", LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, (Hsn_Code), LMargin + W1 + 25, CurY, 0, 0, pFont)








            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(3) = CurY


            CurY = CurY + TxtHgt - 12
            Common_Procedures.Print_To_PrintDocument(e, "SNO", LMargin, CurY, 2, ClAr(1), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "TYPE", LMargin + ClAr(1), CurY, 2, ClAr(2), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "SET NO", LMargin + ClAr(1) + ClAr(2), CurY, 2, ClAr(3), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "MILL NAME", LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, 2, ClAr(4), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "COUNT", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 2, ClAr(5), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "CONE WEIGHT", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, 2, ClAr(6), pFont)

            Common_Procedures.Print_To_PrintDocument(e, "NO OF CONES", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, 2, ClAr(7), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "WEIGHT", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, 2, ClAr(8), pFont)

            Common_Procedures.Print_To_PrintDocument(e, "LOT NO", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, 2, ClAr(9), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "LOCATION", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + 20, CurY, 2, ClAr(8), pFont)
            ' Common_Procedures.Print_To_PrintDocument(e, "LOCATION", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10), CurY, 2, ClAr(11), pFont)

            'Common_Procedures.Print_To_PrintDocument(e, "LOTNO", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11), CurY, 2, ClAr(10) + ClAr(11), pFont)
            'Common_Procedures.Print_To_PrintDocument(e, "LOCATION", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12), CurY, 2, ClAr(10), pFont)


            CurY = CurY + TxtHgt
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
        Dim C1 As Single
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim Del_Add1 As String = "", Del_Add2 As String = "", nGST_No As String = ""
        Try

            For I = NoofDets + 1 To NoofItems_PerPage
                CurY = CurY + TxtHgt
            Next

            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(5) = CurY

            CurY = CurY + TxtHgt - 10
            If is_LastPage = True Then
                Common_Procedures.Print_To_PrintDocument(e, " TOTAL", LMargin + ClAr(3) + 40, CurY, 2, ClAr(4), pFont)

                If Val(prn_HdDt.Rows(0).Item("Total_yarn_cones").ToString) <> 0 Then

                    'Dim infor As String = Val(prn_HdDt.Rows(0).Item("Total_yarn_cones").ToString) & " " & Common_Procedures.Bag_Type_IdNoToName(con, prn_HdDt.Rows(0).Item("BagType_IdNo"))
                    Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(0).Item("Total_yarn_cones").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, pFont)
                End If
                'If Val(prn_HdDt.Rows(0).Item("Total_yarn_Cones").ToString) <> 0 Then
                '    Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(0).Item("Total_yarn_Cones").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 10, CurY, 1, 0, pFont)
                'End If
                'sum_Total_Amount

                If Val(prn_HdDt.Rows(0).Item("Total_yarn_Weight").ToString) <> 0 Then
                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Total_yarn_Weight").ToString), "#######0.000"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 10, CurY, 1, 0, pFont)

                End If




            End If


            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(6) = CurY

            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1), CurY, LMargin + ClAr(1), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2), CurY, LMargin + ClAr(1) + ClAr(2), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9), LnAr(3))
            ' e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10), LnAr(3))

            'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11), LnAr(3))
            'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12), LnAr(3))

            C1 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4)
            CurY = CurY + TxtHgt + 35

            p1Font = New Font("Calibri", 10, FontStyle.Bold)

            'If Trim(Common_Procedures.settings.CustomerCode) = "1282" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1288" Then

            '    If Common_Procedures.Vendor_IdNoToName(Con, Val(prn_HdDt.Rows(0).Item("Vendor_IdNo").ToString)) <> "" Then
            '        da = New SqlClient.SqlDataAdapter("SELECT a.* From Vendor_Head a Where a.Vendor_IdNo = " & Val(prn_HdDt.Rows(0).Item("Vendor_IdNo").ToString) & " ", Con)
            '        dt = New DataTable
            '        da.Fill(dt)


            '        If dt.Rows.Count > 0 Then
            '            Del_Add1 = dt.Rows(0).Item("Vendor_address1").ToString & "," & dt.Rows(0).Item("Vendor_Address2").ToString
            '            Del_Add2 = dt.Rows(0).Item("Vendor_address3").ToString & "," & dt.Rows(0).Item("Vendor_Address4").ToString
            '            If Trim(dt.Rows(0).Item("GST_No").ToString) <> "" Then nGST_No = "GSTIN : " & dt.Rows(0).Item("GST_No").ToString
            '        End If


            '        Common_Procedures.Print_To_PrintDocument(e, "DELIVERY TO   : " & Common_Procedures.Vendor_IdNoToName(Con, Val(prn_HdDt.Rows(0).Item("Vendor_IdNo").ToString)), LMargin + 10, CurY, 0, 0, pFont)
            '        CurY = CurY + TxtHgt
            '        Common_Procedures.Print_To_PrintDocument(e, " " & Trim(Del_Add1), LMargin + 30, CurY, 0, 0, pFont)
            '        CurY = CurY + TxtHgt
            '        Common_Procedures.Print_To_PrintDocument(e, " " & Trim(Del_Add2), LMargin + 30, CurY, 0, 0, pFont)
            '        CurY = CurY + TxtHgt
            '        Common_Procedures.Print_To_PrintDocument(e, " " & Trim(nGST_No), LMargin + 30, CurY, 0, 0, pFont)

            '    Else

            '        Del_Add1 = prn_HdDt.Rows(0).Item("DelAdd1").ToString & "," & prn_HdDt.Rows(0).Item("DelAdd2").ToString
            '        Del_Add2 = prn_HdDt.Rows(0).Item("DelAdd3").ToString & "," & prn_HdDt.Rows(0).Item("DelAdd4").ToString

            '        Common_Procedures.Print_To_PrintDocument(e, "DELIVERY TO   : " & prn_HdDt.Rows(0).Item("DelName").ToString, LMargin + 10, CurY, 0, 0, pFont)
            '        CurY = CurY + TxtHgt
            '        Common_Procedures.Print_To_PrintDocument(e, " " & Trim(Del_Add1), LMargin + 30, CurY, 0, 0, pFont)
            '        CurY = CurY + TxtHgt
            '        Common_Procedures.Print_To_PrintDocument(e, " " & Trim(Del_Add2), LMargin + 30, CurY, 0, 0, pFont)
            '    End If

            'Else

            '    Del_Add1 = prn_HdDt.Rows(0).Item("DelAdd1").ToString & "," & prn_HdDt.Rows(0).Item("DelAdd2").ToString
            '    Del_Add2 = prn_HdDt.Rows(0).Item("DelAdd3").ToString & "," & prn_HdDt.Rows(0).Item("DelAdd4").ToString


            '    Common_Procedures.Print_To_PrintDocument(e, "DELIVERY TO   : " & prn_HdDt.Rows(0).Item("DelName").ToString, LMargin + 10, CurY, 0, 0, p1Font)
            '    CurY = CurY + TxtHgt
            '    Common_Procedures.Print_To_PrintDocument(e, " " & Trim(Del_Add1), LMargin + 30, CurY, 0, 0, pFont)
            '    CurY = CurY + TxtHgt
            '    Common_Procedures.Print_To_PrintDocument(e, " " & Trim(Del_Add2), LMargin + 30, CurY, 0, 0, pFont)
            '    If Trim(prn_HdDt.Rows(0).Item("DelGSTinNo").ToString) <> "" Then
            '        CurY = CurY + TxtHgt
            '        Common_Procedures.Print_To_PrintDocument(e, " GSTIN " & prn_HdDt.Rows(0).Item("DelGSTinNo").ToString, LMargin + 10, CurY, 0, 0, pFont)
            '    End If

            'End If

            'p1Font = New Font("Calibri", 10, FontStyle.Bold)
            'CurY = CurY + TxtHgt
            'If Trim(prn_HdDt.Rows(0).Item("Remarks").ToString) <> "" Then
            '    Common_Procedures.Print_To_PrintDocument(e, " Remarks : " & Trim(prn_HdDt.Rows(0).Item("Remarks").ToString), LMargin + 10, CurY, 0, 0, p1Font)
            'End If


            ''Common_Procedures.Print_To_PrintDocument(e, "DELIVERY TO   : " & prn_HdDt.Rows(0).Item("DelName").ToString, LMargin + 10, CurY, 0, 0, pFont)
            ''CurY = CurY + TxtHgt
            ''Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("DelAdd1").ToString, LMargin + 30, CurY, 0, 0, pFont)
            'If Val(prn_HdDt.Rows(0).Item("Empty_Beam").ToString) <> 0 Then
            '    Common_Procedures.Print_To_PrintDocument(e, " Empty Beam : " & Trim(prn_HdDt.Rows(0).Item("Empty_Beam").ToString), LMargin + C1 + 10, CurY, 0, 0, pFont)
            'End If
            ''Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("DelAdd2").ToString, LMargin + 30, CurY, 0, 0, pFont)
            'If Val(prn_HdDt.Rows(0).Item("Empty_Bags").ToString) <> 0 Then
            '    CurY = CurY + TxtHgt
            '    Common_Procedures.Print_To_PrintDocument(e, " Empty Bags : " & Trim(prn_HdDt.Rows(0).Item("Empty_Bags").ToString), LMargin + C1 + 10, CurY, 0, 0, pFont)
            'End If

            ''Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("DelAdd3").ToString, LMargin + 30, CurY, 0, 0, pFont)
            'If Val(prn_HdDt.Rows(0).Item("Empty_Cones").ToString) <> 0 Then
            '    CurY = CurY + TxtHgt
            '    Common_Procedures.Print_To_PrintDocument(e, " Empty Cones : " & Trim(prn_HdDt.Rows(0).Item("Empty_Cones").ToString), LMargin + C1 + 10, CurY, 0, 0, pFont)
            'End If
            ''CurY = CurY + TxtHgt
            ''Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("DelAdd4").ToString, LMargin + 30, CurY, 0, 0, pFont)



            'CurY = CurY + TxtHgt
            'Common_Procedures.Print_To_PrintDocument(e, "This is to certify that there is no sale indicated in this delivery and the yarn sized is returned back to party after warping and sizing job work.", LMargin + 10, CurY, 0, 0, pFont)

            'CurY = CurY + TxtHgt
            'e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            'LnAr(7) = CurY

            'CurY = CurY + TxtHgt
            'If Val(Common_Procedures.User.IdNo) <> 1 Then
            '    Common_Procedures.Print_To_PrintDocument(e, "(" & Trim(Common_Procedures.User.Name) & ")", LMargin + 400, CurY, 0, 0, pFont)
            'End If
            'CurY = CurY + TxtHgt

            'If Trim(prn_HdDt.Rows(0).Item("Company_Division_Name").ToString) <> "" Then
            '    Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString
            '    Cmp_Name = Replace(Cmp_Name, Trim(prn_HdDt.Rows(0).Item("Company_Division_Name").ToString), "")

            'Else
            '    Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString

            'End If
            Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString
            If Trim(Common_Procedures.settings.CustomerCode) = "1288" Then
                Common_Procedures.Print_To_PrintDocument(e, "Supervisor's ", LMargin + 20, CurY, 0, 0, pFont)
            Else
                Common_Procedures.Print_To_PrintDocument(e, "Supervisor's Signature", LMargin + 20, CurY, 0, 0, pFont)
            End If

            If Trim(Common_Procedures.settings.CustomerCode) = "1288" Then
                Common_Procedures.Print_To_PrintDocument(e, "Prepared By ", LMargin + 120, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "Checked By  ", LMargin + 250, CurY, 0, 0, pFont)

            ElseIf Trim(Common_Procedures.settings.CustomerCode) = "1282" Then
                Common_Procedures.Print_To_PrintDocument(e, "Checked By ", LMargin + 250, CurY, 0, 0, pFont)

            Else
                Common_Procedures.Print_To_PrintDocument(e, "Prepared By ", LMargin + 250, CurY, 0, 0, pFont)
            End If



            p1Font = New Font("Calibri", 12, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "For " & Cmp_Name, LMargin + PageWidth - 60, CurY, 1, 0, p1Font)

            'e.Graphics.DrawLine(Pens.Black, PageWidth, CurY, PageWidth, LnAr(1))

            CurY = CurY + TxtHgt + 10

            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            e.Graphics.DrawLine(Pens.Black, LMargin, LnAr(1), LMargin, CurY)
            e.Graphics.DrawLine(Pens.Black, PageWidth, LnAr(1), PageWidth, CurY)

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub





    Private Sub btn_print_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_print.Click
        print_record()

    End Sub


End Class