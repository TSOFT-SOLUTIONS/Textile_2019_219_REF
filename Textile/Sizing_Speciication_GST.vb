Imports System.Security.AccessControl
Public Class Sizing_Speciication_GST
    Implements Interface_MDIActions

    Private con As New SqlClient.SqlConnection(Common_Procedures.Connection_String)
    Private con1 As New SqlClient.SqlConnection(Common_Procedures.ConnectionString_CompanyGroupdetails)
    Private FrmLdSTS As Boolean = False
    Private MovSTS As Boolean = False
    Private New_Entry As Boolean = False
    Private Insert_Entry As Boolean = False
    Private Filter_RowNo As Integer = -1
    Private Filter_Status As Boolean = False
    Private Pk_Condition As String = "GSSPC-"
    Private Pk_Condition2 As String = "GSTDS-"
    Private Pk_Condition3 As String = "GSPRC-"
    Private Prec_ActCtrl As New Control
    Private vcbo_KeyDwnVal As Double
    Private WithEvents dgtxt_PavuDetails As New DataGridViewTextBoxEditingControl
    Private WithEvents dgtxt_YarnDetails As New DataGridViewTextBoxEditingControl
    Private dgv_ActiveCtrl_Name As String
    Private LastNo As String = ""
    Private SaveAll_STS As Boolean = False

    Public vmskOldText As String = ""
    Public vmskSelStrt As Integer = -1

    Private vCLO_CONDT As String = ""
    Private FnYearCode1 As String = ""
    Private FnYearCode2 As String = ""
    Private prn_DetDt As New DataTable
    Private prn_DetIndx As Integer



    Public Sub New()
        FrmLdSTS = True
        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

    End Sub
    Private Enum dgvCol_Details As Integer
        Slno            '0
        Beam_No         '1
        Ends_Count      '2
        Pcs             '3
        meters          '4
        beam_width      '5
        Beam_Type       '6
        Gross_Weight    '7
        Net_Weight      '8
        Warp_Weight     '9
        Sts             '10
        pavu_receipt_code   '11
        '
        'Slno    '0
        'Beam_No  '1
        'Pcs      '2
        'meters       '3
        'beam_width   '4
        'Gross_Weight '5
        'Net_Weight  '6
        'Warp_Weight  '7
        'Sts         '8
        'pavu_receipt_code   '9
    End Enum
    Private Sub clear()
        New_Entry = False
        Insert_Entry = False
        MovSTS = False

        chk_Verified_Status.Checked = False
        pnl_Back.Enabled = True
        pnl_Filter.Visible = False
        pnl_Selection.Visible = False
        pnl_BeamDetails.Visible = False
        pnl_OwnOrderSelection.Visible = False
        lbl_NewSTS.Visible = False

        dgv_PavuDetails.Rows.Clear()
        dgv_PavuDetails.Rows.Add()

        dgv_PavuDetails_Total.Rows.Clear()
        dgv_PavuDetails_Total.Rows.Add()

        dgv_YarnDetails.Rows.Clear()
        dgv_YarnDetails.Rows(0).Cells(2).Value = "MILL"

        dgv_YarnDetails_Total.Rows.Clear()
        dgv_YarnDetails_Total.Rows.Add()

        lbl_RefNo.Text = ""
        lbl_RefNo.ForeColor = Color.Black
        lbl_BabyWgt.Text = ""

        msk_Date.Text = ""
        dtp_Date.Text = ""
        txt_SetNo.Text = ""
        cbo_Sizing.Text = ""
        cbo_Type.Text = "DIRECT"
        cbo_Sizing.Tag = ""
        cbo_EndsCount.Text = ""
        txt_PcsLength.Text = ""
        txt_ExcessShort.Text = ""
        txt_BabyBag.Text = ""
        txt_BabyWt.Text = ""
        Chk_RWSts.Checked = True
        txt_RwCns.Text = ""
        txt_RWES.Text = ""
        txt_InvNo.Text = ""
        lbl_InvoiceAmt.Text = ""
        lbl_OrderCode.Text = ""
        lbl_OrderNo.Text = ""
        chk_TaxAmount_RoundOff_STS.Checked = False

        msk_InvoiceDate.Text = ""
        dtp_InvoiceDate.Text = ""
        msk_InvoiceDate.Text = ""
        cbo_YarnStock.Text = "CONSUMED YARN"
        txt_ConsumedYarn.Enabled = False

        lbl_Avg_Count.Text = ""
        lbl_Elogation.Text = ""
        txt_PickUp.Text = ""
        txt_TapeLength.Text = ""
        txt_TdsPerc.Text = ""
        txt_RwBags.Text = ""

        lbl_TdsAmount.Text = ""
        lbl_NetAmount.Text = ""

        txt_WarpMtr.Text = ""
        txt_WindingRate.Text = ""
        txt_PackingRate.Text = ""
        txt_ConsumedYarn.Text = ""
        txt_AddLess.Text = ""
        txt_YarnTaken.Text = ""
        txt_InvRate.Text = ""
        txt_TotalBeams.Text = ""

        cbo_BeamCount_Type.Text = "METERS"
        lbl_WindingAmt.Text = ""
        lbl_PackingAmt.Text = ""
        lbl_Elogation.Text = ""
        lbl_InvoiceAmt.Text = ""
        lbl_GrossAmt.Text = ""
        lbl_UserName.Text = "USER : " & Trim(UCase(Common_Procedures.User_IdNoToName(con1, Common_Procedures.User.IdNo)))
        txt_CGST_Percentage.Text = "2.5"
        txt_SGST_Percentage.Text = "2.5"
        lbl_CGST_Amount.Text = ""
        lbl_SGST_Amount.Text = ""
        lbl_Taxable_Value.Text = ""
        lbl_RoundOff.Text = ""
        cbo_Grid_EndsCount.Text = ""

        txt_Sizing_Net_Wgt.Text = ""


        cbo_ClothSales_OrderCode_forSelection.Text = ""

        If Filter_Status = False Then
            dtp_Filter_Fromdate.Text = ""
            dtp_Filter_ToDate.Text = ""
            cbo_Filter_PartyName.Text = ""
            cbo_Filter_CountName.Text = ""
            cbo_Filter_MillName.Text = ""
            txt_Filter_SetNo.Text = ""
            cbo_Filter_PartyName.SelectedIndex = -1
            cbo_Filter_CountName.SelectedIndex = -1
            cbo_Filter_MillName.SelectedIndex = -1
            dgv_Filter_Details.Rows.Clear()
        End If

        cbo_Grid_CountName.Visible = False
        cbo_Grid_MillName.Visible = False
        cbo_Grid_YarnType.Visible = False
        cbo_Grid_BeamWidth.Text = ""

        cbo_Grid_BeamWidth.Visible = False
        cbo_Grid_BeamWidth.Tag = -1
        cbo_Grid_CountName.Tag = -1
        cbo_Grid_MillName.Tag = -1
        cbo_Grid_YarnType.Tag = -1

        cbo_Grid_CountName.Text = ""
        cbo_Grid_MillName.Text = ""
        cbo_Grid_YarnType.Text = ""
        cbo_jobcardno.Text = ""

        lbl_Elongation_Perc.Text = ""
        lbl_PickUp_Perc.Text = ""

        lbl_UserName_CreatedBy.Text = ""
        lbl_UserName_ModifiedBy.Text = ""

        Cbo_RateFor.Text = "WEIGHT"


        cbo_Type.Enabled = True
        cbo_Type.BackColor = Color.White

        cbo_Sizing.Enabled = True
        cbo_Sizing.BackColor = Color.White

        btn_Selection.Enabled = True

        txt_SetNo.Enabled = True
        txt_SetNo.BackColor = Color.White

        cbo_EndsCount.Enabled = True
        cbo_EndsCount.BackColor = Color.White

        txt_BabyBag.Enabled = True
        txt_BabyBag.BackColor = Color.White

        txt_BabyWt.Enabled = True
        txt_BabyWt.BackColor = Color.White

        Chk_RWSts.Enabled = True
        Chk_RWSts.BackColor = Color.White

        txt_RwBags.Enabled = True
        txt_RwBags.BackColor = Color.White

        txt_RwCns.Enabled = True
        txt_RwCns.BackColor = Color.White

        txt_RWES.Enabled = True
        txt_RWES.BackColor = Color.White

        Grid_Cell_DeSelect()

        dgv_ActiveCtrl_Name = ""

        dgv_BeamDetails.Rows.Clear()

    End Sub

    Private Sub ControlGotFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim txtbx As TextBox
        Dim combobx As ComboBox
        Dim msktxbx As MaskedTextBox

        On Error Resume Next

        If TypeOf Me.ActiveControl Is TextBox Or TypeOf Me.ActiveControl Is ComboBox Or TypeOf Me.ActiveControl Is CheckBox Or TypeOf Me.ActiveControl Is MaskedTextBox Then
            Me.ActiveControl.BackColor = Color.Lime
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

        If Me.ActiveControl.Name <> cbo_Grid_MillName.Name Then
            cbo_Grid_MillName.Visible = False
        End If
        If Me.ActiveControl.Name <> cbo_Grid_CountName.Name Then
            cbo_Grid_CountName.Visible = False
        End If
        If Me.ActiveControl.Name <> cbo_Grid_YarnType.Name Then
            cbo_Grid_YarnType.Visible = False
        End If
        If Me.ActiveControl.Name <> cbo_Grid_BeamWidth.Name Then
            cbo_Grid_BeamWidth.Visible = False
        End If
        If Me.ActiveControl.Name <> cbo_Grid_BeamType.Name Then
            cbo_Grid_BeamType.Visible = False
        End If
        If Me.ActiveControl.Name <> cbo_Grid_Yarn_LotNo.Name Then
            cbo_Grid_Yarn_LotNo.Visible = False
        End If

        Grid_Cell_DeSelect()

        Prec_ActCtrl = Me.ActiveControl

    End Sub

    Private Sub ControlLostFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        On Error Resume Next
        If IsNothing(Prec_ActCtrl) = False Then
            If TypeOf Prec_ActCtrl Is TextBox Or TypeOf Prec_ActCtrl Is ComboBox Or TypeOf Prec_ActCtrl Is CheckBox Or TypeOf Prec_ActCtrl Is MaskedTextBox Then
                Prec_ActCtrl.BackColor = Color.White
                Prec_ActCtrl.ForeColor = Color.Black
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

    Private Sub move_record(ByVal no As String)
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim da3 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim dt3 As New DataTable
        Dim NewCode As String
        Dim n As Integer
        Dim SNo As Integer
        Dim LockSTS As Boolean = False
        Dim babyLockSTS As Boolean = False
        Dim I As Integer = 0, J As Integer = 0

        If Val(no) = 0 Then Exit Sub

        clear()

        MovSTS = True

        NewCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(no) & "/" & Trim(Common_Procedures.FnYearCode)

        Try

            da1 = New SqlClient.SqlDataAdapter("select a.*, b.Ledger_Name as SizingName, d.EndsCount_Name from Sizing_Specification_Head a INNER JOIN Ledger_Head b ON a.Ledger_IdNo = b.Ledger_IdNo LEFT OUTER JOIN EndsCount_Head d ON a.EndsCount_IdNo = d.EndsCount_IdNo Where a.Sizing_Specification_Code = '" & Trim(NewCode) & "'", con)
            da1.Fill(dt1)

            If dt1.Rows.Count > 0 Then
                lbl_RefNo.Text = dt1.Rows(0).Item("Sizing_Specification_No").ToString
                dtp_Date.Text = dt1.Rows(0).Item("Sizing_Specification_Date").ToString
                msk_Date.Text =
                    dtp_Date.Text

                cbo_Type.Text = dt1.Rows(0).Item("Selection_Type").ToString
                cbo_Sizing.Text = dt1.Rows(0).Item("SizingName").ToString
                txt_SetNo.Text = dt1.Rows(0).Item("Set_No").ToString
                lbl_SetCode.Text = dt1.Rows(0).Item("Set_Code").ToString

                cbo_EndsCount.Text = dt1.Rows(0).Item("EndsCount_Name").ToString
                txt_PcsLength.Text = dt1.Rows(0).Item("Pcs_Length").ToString

                If Val(dt1.Rows(0).Item("created_useridno").ToString) <> 0 Then
                    If IsDate(dt1.Rows(0).Item("created_DateTime").ToString) = True And Trim(dt1.Rows(0).Item("created_DateTime_Text").ToString) <> "" Then
                        lbl_UserName_CreatedBy.Text = "Created by " & Trim(UCase(Common_Procedures.User_IdNoToName(con, Val(dt1.Rows(0).Item("created_useridno").ToString)))) & " @ " & Trim(dt1.Rows(0).Item("created_DateTime_Text").ToString)
                    Else
                        lbl_UserName_CreatedBy.Text = "USER : " & Trim(UCase(Common_Procedures.User_IdNoToName(con, Val(dt1.Rows(0).Item("created_useridno").ToString))))
                    End If
                End If
                If Val(dt1.Rows(0).Item("Last_modified_useridno").ToString) <> 0 Then
                    If IsDate(dt1.Rows(0).Item("Last_modified_DateTime").ToString) = True And Trim(dt1.Rows(0).Item("Last_modified_DateTime_Text").ToString) <> "" Then
                        lbl_UserName_ModifiedBy.Text = "Last Modified by " & Trim(UCase(Common_Procedures.User_IdNoToName(con, Val(dt1.Rows(0).Item("Last_modified_useridno").ToString)))) & " @ " & Trim(dt1.Rows(0).Item("Last_modified_DateTime_Text").ToString)
                    End If
                End If

                txt_BabyBag.Text = dt1.Rows(0).Item("BabyCone_bags").ToString
                txt_BabyWt.Text = dt1.Rows(0).Item("BabyCone_Weight").ToString
                If Val(dt1.Rows(0).Item("Rewinding_Status").ToString) = 0 Then
                    Chk_RWSts.Checked = False
                End If
                txt_RwCns.Text = dt1.Rows(0).Item("Rewinding_Cones").ToString
                txt_RwBags.Text = Val(dt1.Rows(0).Item("Rewinding_Bags").ToString)
                txt_RWES.Text = dt1.Rows(0).Item("Rewinding_Excess").ToString

                cbo_YarnStock.Text = dt1.Rows(0).Item("YarnStock_Basis").ToString

                cbo_BeamCount_Type.Text = dt1.Rows(0).Item("BeamCount_Type").ToString
                txt_TapeLength.Text = Val(dt1.Rows(0).Item("Tape_Length").ToString)
                txt_PickUp.Text = Val(dt1.Rows(0).Item("PickUp_Perc").ToString)
                lbl_UserName.Text = "USER : " & Trim(UCase(Common_Procedures.User_IdNoToName(con1, Val(dt1.Rows(0).Item("User_IdNo").ToString))))
                lbl_Taxable_Value.Text = dt1.Rows(0).Item("Total_Taxable_Amount").ToString
                txt_CGST_Percentage.Text = dt1.Rows(0).Item("CGST_Percentage").ToString
                lbl_CGST_Amount.Text = dt1.Rows(0).Item("CGST_Amount").ToString
                txt_SGST_Percentage.Text = dt1.Rows(0).Item("SGST_Percentage").ToString
                lbl_SGST_Amount.Text = dt1.Rows(0).Item("SGST_Amount").ToString

                chk_TaxAmount_RoundOff_STS.Checked = False
                If IsDBNull(dt1.Rows(0).Item("TaxAmount_RoundOff_Status").ToString) = False Then
                    If Val(dt1.Rows(0).Item("TaxAmount_RoundOff_Status").ToString) = 1 Then chk_TaxAmount_RoundOff_STS.Checked = True Else chk_TaxAmount_RoundOff_STS.Checked = False
                End If
                lbl_OrderNo.Text = dt1.Rows(0).Item("Our_Order_No").ToString
                lbl_OrderCode.Text = dt1.Rows(0).Item("Own_Order_Code").ToString
                lbl_RoundOff.Text = Format(Val(dt1.Rows(0).Item("RoundOff_Amount").ToString), "########0.00")
                cbo_jobcardno.Text = dt1.Rows(0).Item("Sizing_JobCode_forSelection").ToString


                If Val(dt1.Rows(0).Item("Verified_Status").ToString) = 1 Then chk_Verified_Status.Checked = True

                lbl_Elongation_Perc.Text = Val(dt1.Rows(0).Item("Sizing_Elongation_Percentage").ToString)
                lbl_PickUp_Perc.Text = Val(dt1.Rows(0).Item("Sizing_PickUp_Percentage").ToString)
                Cbo_RateFor.Text = dt1.Rows(0).Item("Rate_For").ToString

                cbo_ClothSales_OrderCode_forSelection.Text = dt1.Rows(0).Item("ClothSales_OrderCode_forSelection").ToString

                txt_Sizing_Net_Wgt.Text = Format(Val(dt1.Rows(0).Item("Sizing_net_weight").ToString), "########0.000")

                da3 = New SqlClient.SqlDataAdapter("select a.* from Stock_BabyCone_Processing_Details a Where a. Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a. Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'", con)
                da3.Fill(dt3)

                If dt3.Rows.Count > 0 Then
                    If (Chk_RWSts.Checked) = False And txt_BabyBag.Text <> 0 Then
                        If Val(dt3.Rows(0).Item("Delivered_Weight").ToString) <> 0 Then
                            babyLockSTS = True
                            If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1035" Then
                                lbl_BabyWgt.Text = Val(dt3.Rows(0).Item("Delivered_Weight").ToString)
                            End If
                        End If
                    End If
                End If

                dt3.Clear()

                da2 = New SqlClient.SqlDataAdapter("select a.*, b.Pavu_Delivery_Code, b.Pavu_Delivery_Increment, b.Beam_Knotting_Code, b.Loom_Idno, b.Production_Meters, b.Close_Status ,d.EndsCount_Name from Sizing_SpecificationPavu_Details a, Stock_SizedPavu_Processing_Details b Left Outer join EndsCount_Head d on b.EndsCount_IdNo = d.EndsCount_IdNo where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Sizing_Specification_Code = '" & Trim(NewCode) & "' and a.Set_Code = b.Set_Code and a.Beam_No = b.Beam_No Order by a.Sl_No", con)

                'da2 = New SqlClient.SqlDataAdapter("select a.*, b.Pavu_Delivery_Code, b.Pavu_Delivery_Increment, b.Beam_Knotting_Code, b.Loom_Idno, b.Production_Meters, b.Close_Status from Sizing_SpecificationPavu_Details a, Stock_SizedPavu_Processing_Details b where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Sizing_Specification_Code = '" & Trim(NewCode) & "' and a.Set_Code = b.Set_Code and a.Beam_No = b.Beam_No Order by a.Sl_No", con)
                dt2 = New DataTable
                da2.Fill(dt2)

                'da2 = New SqlClient.SqlDataAdapter("select * from Stock_SizedPavu_Processing_Details where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' Order by Sl_No", con)
                'dt2 = New DataTable
                'da2.Fill(dt2)

                dgv_PavuDetails.Rows.Clear()
                SNo = 0

                If dt2.Rows.Count > 0 Then

                    For I = 0 To dt2.Rows.Count - 1

                        n = dgv_PavuDetails.Rows.Add()

                        SNo = SNo + 1
                        dgv_PavuDetails.Rows(n).Cells(dgvCol_Details.Slno).Value = Val(SNo)
                        dgv_PavuDetails.Rows(n).Cells(dgvCol_Details.Beam_No).Value = dt2.Rows(I).Item("Beam_No").ToString
                        dgv_PavuDetails.Rows(n).Cells(dgvCol_Details.Ends_Count).Value = Trim(dt2.Rows(I).Item("EndsCount_Name").ToString)
                        If Val(dt2.Rows(I).Item("Noof_Pcs").ToString) <> 0 Then
                            dgv_PavuDetails.Rows(n).Cells(dgvCol_Details.Pcs).Value = Val(dt2.Rows(I).Item("Noof_Pcs").ToString)
                        End If
                        If Common_Procedures.settings.Weaver_Zari_Kuri_Entries_Status = 1 Then
                            dgv_PavuDetails.Rows(n).Cells(dgvCol_Details.meters).Value = Format(Val(dt2.Rows(I).Item("Meters").ToString), "########0.000")

                        Else
                            dgv_PavuDetails.Rows(n).Cells(dgvCol_Details.meters).Value = Format(Val(dt2.Rows(I).Item("Meters").ToString), "########0.00")

                        End If

                        dgv_PavuDetails.Rows(n).Cells(dgvCol_Details.beam_width).Value = Common_Procedures.BeamWidth_IdNoToName(con, Val(dt2.Rows(I).Item("Beam_Width_IdNo").ToString))
                        dgv_PavuDetails.Rows(n).Cells(dgvCol_Details.Sts).Value = ""
                        dgv_PavuDetails.Rows(n).Cells(dgvCol_Details.pavu_receipt_code).Value = dt2.Rows(I).Item("Sizing_Pavu_Receipt_Code").ToString

                        If Trim(UCase(cbo_Type.Text)) <> "RECEIPT" And Trim(UCase(cbo_Type.Text)) <> "SIZING-UNIT PAVU DELIVERY" Then
                            If Trim(dt2.Rows(I).Item("Pavu_Delivery_Code").ToString) <> "" Or Val(dt2.Rows(I).Item("Pavu_Delivery_Increment").ToString) <> 0 Or Trim(dt2.Rows(I).Item("Beam_Knotting_Code").ToString) <> "" Or Val(dt2.Rows(I).Item("Production_Meters").ToString) <> 0 Or Val(dt2.Rows(I).Item("Close_Status").ToString) <> 0 Then
                                dgv_PavuDetails.Rows(n).Cells(dgvCol_Details.Sts).Value = "1"
                                For J = 0 To dgv_PavuDetails.ColumnCount - 1
                                    dgv_PavuDetails.Rows(n).Cells(J).Style.BackColor = Color.LightGray
                                    dgv_PavuDetails.Rows(n).Cells(J).Style.ForeColor = Color.Red
                                Next
                                LockSTS = True
                            End If
                        End If
                        dgv_PavuDetails.Rows(n).Cells(dgvCol_Details.Gross_Weight).Value = Format(Val(dt2.Rows(I).Item("gross_Weight").ToString), "########0.000")
                        dgv_PavuDetails.Rows(n).Cells(dgvCol_Details.Net_Weight).Value = Format(Val(dt2.Rows(I).Item("Net_Weight").ToString), "########0.000")
                        dgv_PavuDetails.Rows(n).Cells(dgvCol_Details.Warp_Weight).Value = Format(Val(dt2.Rows(I).Item("Warp_Weight").ToString), "########0.000")

                        dgv_PavuDetails.Rows(n).Cells(dgvCol_Details.Beam_Type).Value = Common_Procedures.LoomType_IdNoToName(con, Val(dt2.Rows(I).Item("LoomType_Idno").ToString))

                    Next I

                End If

                dt2.Clear()

                dgv_PavuDetails.Rows.Add()

                TotalPavu_Calculation()

                'With dgv_PavuDetails_Total
                '    If .RowCount = 0 Then .Rows.Add()
                '    .Rows(0).Cells(1).Value = Val(dt1.Rows(0).Item("Total_Beam").ToString)
                '    .Rows(0).Cells(3).Value = Format(Val(dt1.Rows(0).Item("Total_Meters").ToString), "########0.00")
                'End With

                da2 = New SqlClient.SqlDataAdapter("select a.*, b.Count_Name, c.Mill_Name from Sizing_SpecificationYarn_Details a INNER JOIN Count_Head b on a.Count_IdNo = b.Count_IdNo INNER JOIN Mill_Head c on a.Mill_IdNo = c.Mill_IdNo where a.Sizing_Specification_Code = '" & Trim(NewCode) & "' Order by a.sl_no", con)
                dt2 = New DataTable
                da2.Fill(dt2)

                dgv_YarnDetails.Rows.Clear()
                SNo = 0

                If dt2.Rows.Count > 0 Then

                    For I = 0 To dt2.Rows.Count - 1

                        n = dgv_YarnDetails.Rows.Add()

                        SNo = SNo + 1
                        dgv_YarnDetails.Rows(n).Cells(0).Value = Val(SNo)
                        dgv_YarnDetails.Rows(n).Cells(1).Value = dt2.Rows(I).Item("Count_Name").ToString
                        dgv_YarnDetails.Rows(n).Cells(2).Value = dt2.Rows(I).Item("Yarn_Type").ToString
                        dgv_YarnDetails.Rows(n).Cells(3).Value = dt2.Rows(I).Item("Mill_Name").ToString
                        dgv_YarnDetails.Rows(n).Cells(4).Value = Val(dt2.Rows(I).Item("Bags").ToString)
                        dgv_YarnDetails.Rows(n).Cells(5).Value = Val(dt2.Rows(I).Item("Cones").ToString)
                        dgv_YarnDetails.Rows(n).Cells(6).Value = Format(Val(dt2.Rows(I).Item("Weight").ToString), "########0.000")
                        dgv_YarnDetails.Rows(n).Cells(7).Value = Common_Procedures.YarnLotEntryReferenceCode_to_LotCodeSelection(con, dt2.Rows(I).Item("Lot_Entry_ReferenceCode").ToString)

                        ' dgv_YarnDetails.Rows(n).Cells(7).Value = dt2.Rows(I).Item("Rewinding_Delivery_Code").ToString
                        'If (Chk_RWSts.Checked) = False And txt_BabyBag.Text <> 0 Then
                        '    If Trim(UCase(dgv_YarnDetails.Rows(I).Cells(2).Value)) = "BABY" Then
                        '        For J = 0 To dgv_YarnDetails.ColumnCount - 1
                        '            dgv_YarnDetails.Rows(n).Cells(J).Style.BackColor = Color.LightGray
                        '        Next J
                        '        LockSTS = True
                        '    End If
                        'End If
                    Next I

                End If

                dt2.Clear()

                TotalYarnTaken_Calculation()

                'With dgv_YarnDetails_Total
                '    If .RowCount = 0 Then .Rows.Add()
                '    .Rows(0).Cells(4).Value = Val(dt1.Rows(0).Item("Total_Bags").ToString)
                '    .Rows(0).Cells(5).Value = Val(dt1.Rows(0).Item("Total_Cones").ToString)
                '    .Rows(0).Cells(6).Value = Format(Val(dt1.Rows(0).Item("Total_Weight").ToString), "########0.000")
                'End With

                lbl_Avg_Count.Text = Format(Val(dt1.Rows(0).Item("Average_Count").ToString), "#########0.00")
                lbl_Elogation.Text = Format(Val(dt1.Rows(0).Item("Elongation").ToString), "#########0.00")

                txt_ConsumedYarn.Text = Format(Val(dt1.Rows(0).Item("Consumed_Yarn").ToString), "#########0.000")
                txt_YarnTaken.Text = Format(Val(dt1.Rows(0).Item("Yarn_Taken").ToString), "#########0.000")

                txt_InvNo.Text = dt1.Rows(0).Item("Sizing_Invoice_No").ToString
                dtp_InvoiceDate.Text = dt1.Rows(0).Item("Invoice_Date").ToString
                msk_InvoiceDate.Text = dtp_InvoiceDate.Text
                msk_InvoiceDate.Text = dt1.Rows(0).Item("Invoice_Date").ToString

                txt_InvRate.Text = Format(Val(dt1.Rows(0).Item("Invoice_Rate").ToString), "#########0.00")
                txt_PackingRate.Text = Format(Val(dt1.Rows(0).Item("Packing_Rate").ToString), "#########0.00")
                txt_WarpMtr.Text = Format(Val(dt1.Rows(0).Item("Warp_Meters").ToString), "#########0.00")
                txt_WindingRate.Text = Format(Val(dt1.Rows(0).Item("Winding_Rate").ToString), "#########0.00")

                txt_TotalBeams.Text = Val(dt1.Rows(0).Item("Total_PlanBeams").ToString)

                lbl_GrossAmt.Text = Format(Val(dt1.Rows(0).Item("Gross_Amount").ToString), "#########0.00")
                lbl_WindingAmt.Text = Format(Val(dt1.Rows(0).Item("Winding_Amount").ToString), "#########0.00")
                lbl_PackingAmt.Text = Format(Val(dt1.Rows(0).Item("Packing_Amount").ToString), "#########0.00")


                txt_AddLess.Text = Format(Val(dt1.Rows(0).Item("Add_Less").ToString), "#########0.00")

                lbl_InvoiceAmt.Text = Format(Val(dt1.Rows(0).Item("Amount").ToString), "#########0.00")
                txt_TdsPerc.Text = Val(dt1.Rows(0).Item("Tds_Percentage").ToString)
                lbl_TdsAmount.Text = Format(Val(dt1.Rows(0).Item("Tds_Amount").ToString), "#########0.00")
                lbl_NetAmount.Text = Format(Val(dt1.Rows(0).Item("Net_Amount").ToString), "#########0.00")

                txt_ExcessShort.Text = Format(Val(dt1.Rows(0).Item("Excess_Short").ToString), "#########0.000")

            End If

            dt1.Clear()

            If LockSTS = True Then
                cbo_Type.Enabled = False
                cbo_Type.BackColor = Color.LightGray

                cbo_Sizing.Enabled = False
                cbo_Sizing.BackColor = Color.LightGray

                btn_Selection.Enabled = False

                txt_SetNo.Enabled = False
                txt_SetNo.BackColor = Color.LightGray

                cbo_EndsCount.Enabled = False
                cbo_EndsCount.BackColor = Color.LightGray
            End If

            If babyLockSTS = True Then

                cbo_Sizing.Enabled = False
                cbo_Sizing.BackColor = Color.LightGray

                txt_BabyBag.Enabled = False
                txt_BabyBag.BackColor = Color.LightGray

                txt_SetNo.Enabled = False
                txt_SetNo.BackColor = Color.LightGray

                txt_BabyWt.Enabled = False
                txt_BabyWt.BackColor = Color.LightGray

                Chk_RWSts.Enabled = False
                Chk_RWSts.BackColor = Color.LightGray

                txt_RwBags.Enabled = False
                txt_RwBags.BackColor = Color.LightGray

                txt_RwCns.Enabled = False
                txt_RwCns.BackColor = Color.LightGray

                txt_RWES.Enabled = False
                txt_RWES.BackColor = Color.LightGray

            End If

            Grid_Cell_DeSelect()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT MOVE RECORDS...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Finally
            dt1.Dispose()
            da1.Dispose()

            dt2.Dispose()
            da2.Dispose()

            MovSTS = False

            If msk_Date.Visible And msk_Date.Enabled Then msk_Date.Focus()
        End Try

    End Sub

    Private Sub Grid_Cell_DeSelect()
        On Error Resume Next
        If Not IsNothing(dgv_PavuDetails.CurrentCell) Then dgv_PavuDetails.CurrentCell.Selected = False
        If Not IsNothing(dgv_YarnDetails.CurrentCell) Then dgv_YarnDetails.CurrentCell.Selected = False
    End Sub

    Private Sub Sizing_Specification_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated

        Try

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Sizing.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "LEDGER" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Sizing.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_EndsCount.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "ENDSCOUNT" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_EndsCount.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Grid_CountName.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "COUNT" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Grid_CountName.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Grid_MillName.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "MILL" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Grid_MillName.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Grid_BeamWidth.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "BEAMWIDTH" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Grid_BeamWidth.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Grid_EndsCount.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "ENDSCOUNT" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Grid_EndsCount.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Grid_BeamType.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "LOOMTYPE" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Grid_BeamType.Text = Trim(Common_Procedures.Master_Return.Return_Value)
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
            'MessageBox.Show(ex.Message, "DOES NOT SHOW...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

        FrmLdSTS = False


    End Sub

    Private Sub Sizing_Specification_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        FrmLdSTS = True

        Me.Text = ""

        dgv_YarnDetails.Columns(7).Visible = False


        lbl_AvgCount_Caption.Text = "Avg.Count"
        'lbl_Consumbed_yarn.Text = "Consumed"

        btn_BarcodePrint.Visible = False


        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1018" Then '---- M.K Textiles (Palladam)
            lbl_AvgCount_Caption.Text = "Party Dc.No"
            lbl_OrderedBeams.Visible = True
            txt_TotalBeams.Visible = True
            btn_BeamDetail.Visible = True
        End If

        btn_SaveAll.Visible = False
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "-1035-" Then
            btn_SaveAll.Visible = True
        End If
        btn_UserModification.Visible = False
        If Common_Procedures.settings.User_Modifications_Show_Status = 1 Then
            If Val(Common_Procedures.User.IdNo) = 1 Or Common_Procedures.User.Show_UserModification_Status = 1 Then
                btn_UserModification.Visible = True
            End If
        End If
        con.Open()

        'da = New SqlClient.SqlDataAdapter("select Ledger_DisplayName from Ledger_AlaisHead Where (ledger_type = 'SIZING' or Ledger_IdNo = 0) order by Ledger_DisplayName", con)
        'da.Fill(dt1)
        'cbo_Sizing.DataSource = dt1
        'cbo_Sizing.DisplayMember = "Ledger_DisplayName"

        'da = New SqlClient.SqlDataAdapter("select distinct(EndsCount_Name) from EndsCount_Head order by EndsCount_Name", con)
        'da.Fill(dt3)
        'cbo_EndsCount.DataSource = dt3
        'cbo_EndsCount.DisplayMember = "EndsCount_Name"

        'da = New SqlClient.SqlDataAdapter("select mill_name from Mill_Head order by mill_name", con)
        'da.Fill(dt4)
        'cbo_Grid_MillName.DataSource = dt4
        'cbo_Grid_MillName.DisplayMember = "mill_name"

        'da = New SqlClient.SqlDataAdapter("select count_name from Count_Head order by count_name", con)
        'da.Fill(dt5)
        'cbo_Grid_CountName.DataSource = dt5
        'cbo_Grid_CountName.DisplayMember = "count_name"

        'da = New SqlClient.SqlDataAdapter("select yarn_type from YarnType_Head order by yarn_type", con)
        'da.Fill(dt6)
        'cbo_Grid_YarnType.DataSource = dt6
        'cbo_Grid_YarnType.DisplayMember = "yarn_type"

        If Common_Procedures.settings.Weaver_Zari_Kuri_Entries_Status = 1 Then
            dgv_PavuDetails.Columns(dgvCol_Details.meters).HeaderText = "MTR Or WGT"
        End If


        If Common_Procedures.settings.Show_Sales_OrderNumber_in_ALLEntry_Status = 1 Then

            cbo_ClothSales_OrderCode_forSelection.Visible = True
            cbo_ClothSales_OrderCode_forSelection.BackColor = Color.White
            lbl_ClothSales_OrderCode_forSelection_Caption.Visible = True

            cbo_Sizing.Width = txt_SetNo.Width
            btn_Selection.Left = Label20.Left - 10

            FnYearCode1 = ""
            FnYearCode2 = ""
            Common_Procedures.get_FnYearCode_of_Last_2_Years(FnYearCode1, FnYearCode2)

        Else

            cbo_ClothSales_OrderCode_forSelection.Visible = False
            lbl_ClothSales_OrderCode_forSelection_Caption.Visible = False

        End If



        cbo_Grid_CountName.Visible = False
        cbo_Grid_MillName.Visible = False
        cbo_Grid_YarnType.Visible = False
        cbo_Grid_EndsCount.Visible = False

        msk_Date.Text = ""
        dtp_Date.Text = ""
        txt_SetNo.Text = ""
        cbo_Sizing.Text = ""
        cbo_Sizing.Tag = ""
        cbo_EndsCount.Text = ""

        cbo_EndsCount.Text = ""

        cbo_BeamCount_Type.Items.Add("")
        cbo_BeamCount_Type.Items.Add("METERS")
        cbo_BeamCount_Type.Items.Add("YARDS")

        cbo_YarnStock.Items.Add("")
        cbo_YarnStock.Items.Add("CONSUMED YARN")
        cbo_YarnStock.Items.Add("YARN TAKEN")
        txt_ConsumedYarn.Enabled = False

        cbo_Type.Items.Add("")
        cbo_Type.Items.Add("DIRECT")
        cbo_Type.Items.Add("RECEIPT")
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1087" Then
            cbo_Type.Items.Add("SIZING-UNIT PAVU DELIVERY")
        End If


        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1376" Then
            dgv_PavuDetails.Columns(dgvCol_Details.Gross_Weight).Visible = True
            dgv_PavuDetails.Columns(dgvCol_Details.Net_Weight).Visible = True
            dgv_PavuDetails.Columns(dgvCol_Details.Warp_Weight).Visible = True

            dgv_PavuDetails_Total.Columns(dgvCol_Details.Gross_Weight).Visible = True
            dgv_PavuDetails_Total.Columns(dgvCol_Details.Net_Weight).Visible = True
            dgv_PavuDetails_Total.Columns(dgvCol_Details.Warp_Weight).Visible = True

            dgv_PavuDetails.Enabled = ScrollBars.Both

        End If

        lbl_JobCardNo_Caption.Visible = False
        cbo_jobcardno.Visible = False
        If Common_Procedures.settings.Show_Sizing_JobCard_Entry_Status = 1 Then
            lbl_JobCardNo_Caption.Visible = True
            cbo_jobcardno.Visible = True
        End If

        dgv_YarnDetails.Columns(7).Visible = False

        If Common_Procedures.settings.Show_Yarn_LotNo_Status = 1 Then

            dgv_YarnDetails.Columns(7).Visible = True

            dgv_YarnDetails.Columns(1).Width = 70
            dgv_YarnDetails.Columns(4).Width = 45
            dgv_YarnDetails.Columns(5).Width = 50
            dgv_YarnDetails.Columns(6).Width = 70


            dgv_YarnDetails_Total.Columns(1).Width = dgv_YarnDetails.Columns(1).Width
            dgv_YarnDetails_Total.Columns(4).Width = dgv_YarnDetails.Columns(4).Width
            dgv_YarnDetails_Total.Columns(5).Width = dgv_YarnDetails.Columns(5).Width
            dgv_YarnDetails_Total.Columns(6).Width = dgv_YarnDetails.Columns(6).Width




        End If


        pnl_Filter.Visible = False
        pnl_Filter.Left = (Me.Width - pnl_Filter.Width) \ 2
        pnl_Filter.Top = (Me.Height - pnl_Filter.Height) \ 2

        pnl_Selection.Visible = False
        pnl_Selection.Left = (Me.Width - pnl_Selection.Width) \ 2
        pnl_Selection.Top = (Me.Height - pnl_Selection.Height) \ 2
        pnl_Selection.BringToFront()

        pnl_OwnOrderSelection.Visible = False
        pnl_OwnOrderSelection.Left = (Me.Width - pnl_OwnOrderSelection.Width) \ 2
        pnl_OwnOrderSelection.Top = (Me.Height - pnl_OwnOrderSelection.Height) \ 2
        pnl_OwnOrderSelection.BringToFront()

        pnl_BeamDetails.Visible = False
        pnl_BeamDetails.Left = (Me.Width - pnl_BeamDetails.Width) \ 2
        pnl_BeamDetails.Top = (Me.Height - pnl_BeamDetails.Height) \ 2
        pnl_BeamDetails.BringToFront()

        cbo_Verified_Sts.Items.Clear()
        cbo_Verified_Sts.Items.Add("")
        cbo_Verified_Sts.Items.Add("YES")
        cbo_Verified_Sts.Items.Add("NO")

        If Trim(Common_Procedures.settings.CustomerCode) = "1249" Or Trim(Common_Procedures.settings.CustomerCode) = "1116" Then


            chk_Verified_Status.Visible = True
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
        End If

        If Trim(Common_Procedures.settings.CustomerCode) = "1464" Then

            txt_ExcessShort.ReadOnly = True
            Cbo_RateFor.Visible = True
            lbl_Rate_For.Visible = True

            Cbo_RateFor.Items.Clear()
            Cbo_RateFor.Items.Add(" ")
            Cbo_RateFor.Items.Add("METER")
            Cbo_RateFor.Items.Add("WEIGHT")

        Else

            Cbo_RateFor.Visible = False
            lbl_Rate_For.Visible = False

            Label39.Left = Label46.Left
            txt_InvRate.Left = lbl_Taxable_Value.Left
            txt_InvRate.Width = lbl_Taxable_Value.Width

        End If

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1608" Then
            btn_BarcodePrint.Visible = True
        End If



        AddHandler chk_Verified_Status.GotFocus, AddressOf ControlGotFocus
        AddHandler chk_Verified_Status.LostFocus, AddressOf ControlLostFocus


        AddHandler msk_Date.GotFocus, AddressOf ControlGotFocus
        AddHandler dtp_Date.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Sizing.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_EndsCount.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Grid_MillName.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Grid_CountName.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Grid_BeamWidth.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Grid_YarnType.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_YarnStock.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Type.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_CGST_Percentage.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_SGST_Percentage.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_ClothSales_OrderCode_forSelection.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Sizing_Net_Wgt.GotFocus, AddressOf ControlGotFocus

        AddHandler txt_TapeLength.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_TdsPerc.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_PickUp.GotFocus, AddressOf ControlGotFocus

        AddHandler cbo_Grid_YarnType.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_BabyBag.GotFocus, AddressOf ControlGotFocus

        AddHandler txt_BabyWt.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_ExcessShort.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_InvNo.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_PcsLength.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_RwCns.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_RWES.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_SetNo.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Filter_SetNo.GotFocus, AddressOf ControlGotFocus
        AddHandler dtp_Filter_Fromdate.GotFocus, AddressOf ControlGotFocus
        AddHandler dtp_Filter_ToDate.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Filter_PartyName.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Filter_MillName.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Filter_EndsCount.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Filter_CountName.GotFocus, AddressOf ControlGotFocus
        AddHandler Chk_RWSts.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_RwBags.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_save.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_close.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_jobcardno.GotFocus, AddressOf ControlGotFocus


        AddHandler txt_WarpMtr.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_YarnTaken.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_AddLess.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_WindingRate.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_PackingRate.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_InvRate.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_ConsumedYarn.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_BeamCount_Type.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_TotalBeams.GotFocus, AddressOf ControlGotFocus
        AddHandler msk_InvoiceDate.GotFocus, AddressOf ControlGotFocus
        AddHandler lbl_PickUp_Perc.GotFocus, AddressOf ControlGotFocus
        AddHandler lbl_Elongation_Perc.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Grid_EndsCount.GotFocus, AddressOf ControlGotFocus
        AddHandler Cbo_RateFor.Enter, AddressOf ControlGotFocus
        AddHandler cbo_Grid_BeamType.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Grid_Yarn_LotNo.GotFocus, AddressOf ControlGotFocus

        AddHandler msk_Date.LostFocus, AddressOf ControlLostFocus
        AddHandler dtp_Date.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Sizing.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Grid_CountName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Grid_MillName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Grid_YarnType.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_EndsCount.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Grid_BeamWidth.LostFocus, AddressOf ControlLostFocus
        AddHandler lbl_InvoiceAmt.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_BabyBag.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_BabyWt.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_ExcessShort.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_CGST_Percentage.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_SGST_Percentage.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_ClothSales_OrderCode_forSelection.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Sizing_Net_Wgt.LostFocus, AddressOf ControlLostFocus

        AddHandler cbo_YarnStock.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Type.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_TapeLength.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_TdsPerc.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_PickUp.LostFocus, AddressOf ControlLostFocus

        AddHandler txt_Filter_SetNo.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_InvNo.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_PcsLength.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_RwCns.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_RWES.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_SetNo.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_jobcardno.LostFocus, AddressOf ControlLostFocus

        AddHandler dtp_Filter_Fromdate.LostFocus, AddressOf ControlLostFocus
        AddHandler dtp_Filter_ToDate.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Filter_PartyName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Filter_CountName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Filter_EndsCount.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Filter_MillName.LostFocus, AddressOf ControlLostFocus
        AddHandler Chk_RWSts.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_RwBags.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_save.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_close.LostFocus, AddressOf ControlLostFocus

        AddHandler txt_WarpMtr.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_YarnTaken.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_AddLess.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_WindingRate.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_PackingRate.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_InvRate.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_ConsumedYarn.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_BeamCount_Type.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_TotalBeams.LostFocus, AddressOf ControlLostFocus
        AddHandler msk_InvoiceDate.LostFocus, AddressOf ControlLostFocus
        AddHandler lbl_PickUp_Perc.LostFocus, AddressOf ControlLostFocus
        AddHandler lbl_Elongation_Perc.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Grid_EndsCount.LostFocus, AddressOf ControlLostFocus
        AddHandler Cbo_RateFor.Leave, AddressOf ControlLostFocus
        AddHandler cbo_Grid_BeamType.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Grid_Yarn_LotNo.LostFocus, AddressOf ControlLostFocus

        'AddHandler msk_Date.KeyDown, AddressOf TextBoxControlKeyDown
        'AddHandler txt_TotalBeams.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_BabyWt.KeyDown, AddressOf TextBoxControlKeyDown

        'AddHandler txt_TdsPerc.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_BabyBag.KeyDown, AddressOf TextBoxControlKeyDown

        AddHandler txt_InvNo.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_PcsLength.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_PickUp.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_RWES.KeyDown, AddressOf TextBoxControlKeyDown
        'AddHandler txt_SetNo.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler dtp_Filter_Fromdate.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler dtp_Filter_ToDate.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_CGST_Percentage.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_SGST_Percentage.KeyDown, AddressOf TextBoxControlKeyDown

        'AddHandler msk_InvoiceDate.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler Chk_RWSts.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_RwBags.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_ExcessShort.KeyDown, AddressOf TextBoxControlKeyDown

        AddHandler txt_AddLess.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_WarpMtr.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_YarnTaken.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_WindingRate.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_PackingRate.KeyDown, AddressOf TextBoxControlKeyDown
        'AddHandler txt_InvRate.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_ConsumedYarn.KeyDown, AddressOf TextBoxControlKeyDown

        'AddHandler msk_Date.KeyPress, AddressOf TextBoxControlKeyPress
        'AddHandler txt_TotalBeams.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_BabyWt.KeyPress, AddressOf TextBoxControlKeyPress

        'AddHandler msk_InvoiceDate.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_PickUp.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_BabyBag.KeyPress, AddressOf TextBoxControlKeyPress

        AddHandler txt_TapeLength.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_InvNo.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_PcsLength.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_RwCns.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_RWES.KeyPress, AddressOf TextBoxControlKeyPress
        'AddHandler txt_SetNo.KeyPress, AddressOf TextBoxControlKeyPress

        AddHandler dtp_Filter_Fromdate.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler dtp_Filter_ToDate.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler Chk_RWSts.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_RwBags.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_ExcessShort.KeyPress, AddressOf TextBoxControlKeyPress

        AddHandler txt_AddLess.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_WarpMtr.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_YarnTaken.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_WindingRate.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_PackingRate.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_InvRate.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_ConsumedYarn.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_CGST_Percentage.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_SGST_Percentage.KeyPress, AddressOf TextBoxControlKeyPress
        lbl_Company.Text = ""
        lbl_Company.Tag = 0
        lbl_Company.Visible = False
        Common_Procedures.CompIdNo = 0

        Filter_Status = False
        FrmLdSTS = True
        new_record()

    End Sub

    Private Sub Sizing_Specification_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        On Error Resume Next
        con.Close()
        con.Dispose()
        Common_Procedures.Last_Closed_FormName = Me.Name
    End Sub

    Private Sub Sizing_Specification_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress

        Try

            If Asc(e.KeyChar) = 27 Then


                If pnl_Filter.Visible = True Then
                    btn_Filter_Close_Click(sender, e)
                    Exit Sub

                ElseIf pnl_Selection.Visible = True Then
                    btn_Close_Selection_Click(sender, e)
                    Exit Sub

                ElseIf pnl_BeamDetails.Visible = True Then
                    btn_BeamClose_Click(sender, e)
                    Exit Sub
                ElseIf pnl_OwnOrderSelection.Visible = True Then
                    btn_Close_OwnOrderSelection_Click(sender, e)
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

    Protected Overrides Function ProcessCmdKey(ByRef msg As System.Windows.Forms.Message, ByVal keyData As System.Windows.Forms.Keys) As Boolean
        Dim dgv1 As New DataGridView
        Dim i As Integer

        If ActiveControl.Name = dgv_YarnDetails.Name Or ActiveControl.Name = dgv_PavuDetails.Name Or TypeOf ActiveControl Is DataGridViewTextBoxEditingControl Then
            On Error Resume Next

            dgv1 = Nothing

            If ActiveControl.Name = dgv_YarnDetails.Name Then
                dgv1 = dgv_YarnDetails

            ElseIf dgv_YarnDetails.IsCurrentRowDirty = True Then
                dgv1 = dgv_YarnDetails

            ElseIf dgv_ActiveCtrl_Name = dgv_YarnDetails.Name Then
                dgv1 = dgv_YarnDetails

            ElseIf ActiveControl.Name = dgv_PavuDetails.Name Then
                dgv1 = dgv_PavuDetails

            ElseIf dgv_PavuDetails.IsCurrentRowDirty = True Then
                dgv1 = dgv_PavuDetails

            ElseIf dgv_ActiveCtrl_Name = dgv_PavuDetails.Name Then
                dgv1 = dgv_PavuDetails

            End If

            If IsNothing(dgv1) = True Then
                Return MyBase.ProcessCmdKey(msg, keyData)
                Exit Function
            End If

            With dgv1

                If dgv1.Name = dgv_YarnDetails.Name Then

                    If keyData = Keys.Enter Or keyData = Keys.Down Then
                        If .CurrentCell.ColumnIndex >= .ColumnCount - 1 Then
                            If .CurrentCell.RowIndex = .RowCount - 1 Then
                                txt_TapeLength.Focus()

                            Else
                                .CurrentCell = .Rows(.CurrentCell.RowIndex + 1).Cells(1)

                            End If
                        ElseIf .CurrentCell.ColumnIndex = 6 Then

                            If dgv_YarnDetails.Columns(7).Visible Then
                                .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 1)
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
                                If Trim(UCase(cbo_Type.Text)) = "RECEIPT" Or Trim(UCase(cbo_Type.Text)) = "SIZING-UNIT PAVU DELIVERY" Then
                                    cbo_YarnStock.Focus()

                                Else
                                    If dgv_PavuDetails.Rows.Count = 0 Then dgv_PavuDetails.Rows.Add()
                                    dgv_PavuDetails.CurrentCell = dgv_PavuDetails.Rows(0).Cells(dgvCol_Details.Beam_No)
                                    dgv_PavuDetails.Focus()

                                End If

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


                ElseIf dgv1.Name = dgv_PavuDetails.Name Then

                    If keyData = Keys.Enter Or keyData = Keys.Down Then
                        If .CurrentCell.ColumnIndex >= .ColumnCount - 2 Then
                            If .CurrentCell.RowIndex = .RowCount - 1 Then
                                If Trim(UCase(cbo_Type.Text)) = "RECEIPT" Or Trim(UCase(cbo_Type.Text)) = "SIZING-UNIT PAVU DELIVERY" Then
                                    dgv_YarnDetails.Focus()
                                    dgv_YarnDetails.CurrentCell = dgv_YarnDetails.Rows(0).Cells(dgvCol_Details.Beam_No)

                                Else
                                    .Rows.Add()
                                    .CurrentCell = .Rows(.CurrentCell.RowIndex + 1).Cells(1)

                                End If
                            ElseIf .CurrentCell.ColumnIndex = dgvCol_Details.beam_width Then
                                If .Columns(dgvCol_Details.Gross_Weight).Visible = True Then
                                    .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(dgvCol_Details.Gross_Weight)
                                Else
                                    dgv_YarnDetails.Focus()
                                    dgv_YarnDetails.CurrentCell = dgv_YarnDetails.Rows(0).Cells(1)
                                End If


                            Else
                                .CurrentCell = .Rows(.CurrentCell.RowIndex + 1).Cells(1)

                            End If

                        Else

                            If .CurrentCell.RowIndex = .RowCount - 1 And .CurrentCell.ColumnIndex >= 1 And ((.CurrentCell.ColumnIndex <> 1 And Val(.CurrentRow.Cells(1).Value) = 0) Or (.CurrentCell.ColumnIndex = 1 And Val(dgtxt_PavuDetails.Text) = 0)) Then
                                For i = 0 To .Columns.Count - 1
                                    .Rows(.CurrentCell.RowIndex).Cells(i).Value = ""
                                Next

                                dgv_YarnDetails.Focus()
                                dgv_YarnDetails.CurrentCell = dgv_YarnDetails.Rows(0).Cells(1)

                            Else
                                .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 1)

                            End If


                        End If

                        Return True

                    ElseIf keyData = Keys.Up Then

                        If .CurrentCell.ColumnIndex <= 1 Then
                            If .CurrentCell.RowIndex = 0 Then
                                If txt_TotalBeams.Visible And txt_TotalBeams.Enabled Then
                                    txt_TotalBeams.Focus()
                                ElseIf cbo_jobcardno.Visible And cbo_jobcardno.Enabled Then
                                    cbo_jobcardno.Focus()
                                Else
                                    cbo_YarnStock.Focus()
                                End If

                            Else
                                .CurrentCell = .Rows(.CurrentCell.RowIndex - 1).Cells(.ColumnCount - 2)

                            End If

                        Else
                            If .CurrentCell.ColumnIndex = dgvCol_Details.Sts Then
                                If dgv_PavuDetails.Columns(dgvCol_Details.Warp_Weight).Visible Then
                                    .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex - 1)
                                Else
                                    .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(dgvCol_Details.Beam_Type)
                                End If
                            Else
                                    .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex - 1)
                            End If


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

    Public Sub delete_record() Implements Interface_MDIActions.delete_record
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim cmd As New SqlClient.SqlCommand
        Dim trans As SqlClient.SqlTransaction
        Dim NewCode As String = ""
        Dim EntID As String = ""
        Dim Partcls As String = ""
        Dim PBlNo As String = ""
        Dim YrnPartcls As String = ""
        Dim pCnt_ID As Integer = 0
        Dim pEdsCnt_Nm As String = ""
        Dim vOrdByNo As String = ""

        vOrdByNo = Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text)

        NewCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.DeletingEntry, Common_Procedures.UR.Sizing_Specification_Entry, New_Entry, Me, con, "Sizing_Specification_Head", "Sizing_Specification_Code", NewCode, "Sizing_Specification_Date", "(Sizing_Specification_Code = '" & Trim(NewCode) & "')") = False Then Exit Sub

        '   If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.Sizing_Specification_Entry, "~L~") = 0 And InStr(Common_Procedures.UR.Sizing_Specification_Entry, "~D~") = 0 Then MessageBox.Show("You have No Rights to Delete", "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error) : Exit Sub

        If MessageBox.Show("Do you want to Delete?", "FOR DELETION...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) <> Windows.Forms.DialogResult.Yes Then
            Exit Sub
        End If

        If New_Entry = True Then
            MessageBox.Show("This is New Entry", "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
        End If

        NewCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Da = New SqlClient.SqlDataAdapter("select count(*) from Stock_SizedPavu_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' and  ( Pavu_Delivery_Code <> '' or Pavu_Delivery_Increment <> 0 or Beam_Knotting_Code <> '' or Loom_Idno <> 0 or Production_Meters <> 0 or Close_Status <> 0)", con)
        Dt1 = New DataTable
        Da.Fill(Dt1)
        If Dt1.Rows.Count > 0 Then
            If IsDBNull(Dt1.Rows(0)(0).ToString) = False Then
                If Val(Dt1.Rows(0)(0).ToString) > 0 Then
                    MessageBox.Show("Already some Pavu Delivered", "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                    Exit Sub
                End If
            End If
        End If
        Dt1.Clear()

        Da = New SqlClient.SqlDataAdapter("select sum(Delivered_Weight) from Stock_BabyCone_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'", con)
        Dt1 = New DataTable
        Da.Fill(Dt1)
        If Dt1.Rows.Count > 0 Then
            If IsDBNull(Dt1.Rows(0)(0).ToString) = False Then
                If Val(Dt1.Rows(0)(0).ToString) > 0 Then
                    MessageBox.Show("Already Some Baby Cones Delivered for this order", "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                    Exit Sub
                End If
            End If
        End If
        Dt1.Clear()

        trans = con.BeginTransaction

        Try

            cmd.Connection = con
            cmd.Transaction = trans
            Call Common_Procedures.User_Modification_Updation(con, "HEAD", Me.Name, "DELETE", "Sizing_Specification_head", "Sizing_Specification_Code", Val(lbl_Company.Tag), NewCode, lbl_RefNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, True, "", "", "Sizing_Specification_Code, Company_IdNo, for_OrderBy", trans)
            Call Common_Procedures.User_Modification_Updation(con, "DETAILS", Me.Name, "DELETE", "Sizing_SpecificationPavu_Details", "Sizing_Specification_Code", Val(lbl_Company.Tag), NewCode, lbl_RefNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, True, " Beam_No ,Noof_Pcs,Meters_Pc,Meters,   Beam_Width_IdNo    ,     Sizing_Pavu_Receipt_No,      Sizing_Pavu_Receipt_Code", "Sl_No", "Sizing_Specification_Code, For_OrderBy, Company_IdNo, Sizing_Specification_No, Sizing_Specification_Date, Ledger_Idno", trans)
            Call Common_Procedures.User_Modification_Updation(con, "DETAILS", Me.Name, "DELETE", "Sizing_SpecificationYarn_Details", "Sizing_Specification_Code", Val(lbl_Company.Tag), NewCode, lbl_RefNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, True, " count_idno, Yarn_Type, Mill_IdNo, Bags, Cones, Weight", "Sl_No", "Sizing_Specification_Code, For_OrderBy, Company_IdNo, Sizing_Specification_No, Sizing_Specification_Date, Ledger_Idno", trans)

            If Common_Procedures.VoucherBill_Deletion(con, Trim(Pk_Condition) & Trim(NewCode), trans) = False Then
                Throw New ApplicationException("Error on Voucher Bill Deletion")
            End If

            Common_Procedures.Voucher_Deletion(con, Val(lbl_Company.Tag), Trim(Pk_Condition) & Trim(NewCode), trans)
            Common_Procedures.Voucher_Deletion(con, Val(lbl_Company.Tag), Trim(Pk_Condition2) & Trim(NewCode), trans)

            Da = New SqlClient.SqlDataAdapter("select * from Sizing_Pavu_Receipt_Head Where Sizing_Specification_Code = '" & Trim(NewCode) & "'", con)
            Da.SelectCommand.Transaction = trans
            Dt1 = New DataTable
            Da.Fill(Dt1)
            If Dt1.Rows.Count > 0 Then
                For i = 0 To Dt1.Rows.Count - 1
                    If Val(Dt1.Rows(i).Item("Total_Meters").ToString) <> 0 Then

                        EntID = Trim(Pk_Condition3) & Trim(Dt1.Rows(i).Item("Sizing_Pavu_Receipt_No").ToString)
                        Partcls = "Pavu Rcpt : Set No. " & Trim(Dt1.Rows(i).Item("Set_No").ToString)
                        PBlNo = Trim(Dt1.Rows(i).Item("Set_No").ToString)

                        pEdsCnt_Nm = Common_Procedures.get_FieldValue(con, "EndsCount_Head", "EndsCount_Name", "(EndsCount_IdNo = " & Str(Val(Dt1.Rows(i).Item("EndsCount_IdNo").ToString)) & ")", , trans)
                        pCnt_ID = Val(Common_Procedures.get_FieldValue(con, "EndsCount_Head", "Count_IdNo", "(EndsCount_IdNo = " & Str(Val(Dt1.Rows(i).Item("EndsCount_IdNo").ToString)) & ")", , trans))

                        YrnPartcls = Partcls & ", EndsCount : " & Trim(pEdsCnt_Nm) & ", Beams : " & Trim(Val(Dt1.Rows(i).Item("Total_Beam").ToString)) & ", Meters : " & Trim(Val(Dt1.Rows(i).Item("Total_Meters").ToString))

                        cmd.CommandText = "Delete from Stock_Yarn_Processing_Details Where Reference_Code = '" & Trim(Pk_Condition3) & Trim(Dt1.Rows(i).Item("Sizing_Pavu_Receipt_Code").ToString) & "'"
                        cmd.ExecuteNonQuery()

                        cmd.Parameters.Clear()
                        cmd.Parameters.AddWithValue("@PavuReceiptDate", Dt1.Rows(i).Item("Sizing_Pavu_Receipt_Date"))

                        cmd.CommandText = "Insert into Stock_Yarn_Processing_Details ( Reference_Code, Company_IdNo, Reference_No, for_OrderBy, Reference_Date, DeliveryTo_Idno, ReceivedFrom_Idno, Entry_ID, Particulars, Party_Bill_No, Sl_No, Count_IdNo, Yarn_Type, Mill_IdNo, Bags, Cones, Weight ) Values ( '" & Trim(Pk_Condition3) & Trim(Dt1.Rows(i).Item("Sizing_Pavu_Receipt_Code").ToString) & "', " & Str(Val(Dt1.Rows(i).Item("Company_IdNo").ToString)) & ", '" & Trim(Dt1.Rows(i).Item("Sizing_Pavu_Receipt_No").ToString) & "', " & Str(Val(Dt1.Rows(i).Item("for_OrderBy").ToString)) & ", @PavuReceiptDate, 0, " & Str(Val(Dt1.Rows(i).Item("Ledger_IdNo").ToString)) & ", '" & Trim(EntID) & "', '" & Trim(YrnPartcls) & "', '" & Trim(PBlNo) & "', 1, " & Str(Val(pCnt_ID)) & ", 'MILL', 0, 0, 0, 0 )"
                        cmd.ExecuteNonQuery()

                    End If

                Next
            End If
            Dt1.Clear()

            cmd.CommandText = "Update Sizing_Pavu_Receipt_Head set Sizing_Specification_Code = '' Where Sizing_Specification_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Update SizSoft_Pavu_Delivery_Head set Sizing_Specification_Code = '' Where Sizing_Specification_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Stock_BabyCone_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Stock_Empty_BeamBagCone_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Stock_SizedPavu_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' and Pavu_Delivery_Code = '' and Pavu_Delivery_Increment = 0 and Beam_Knotting_Code = '' and Loom_Idno = 0 and Production_Meters = 0 and Close_Status = 0"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Stock_Pavu_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Stock_Yarn_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Stock_Empty_BeamBagCone_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "delete from Sizing_SpecificationPavu_Details where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Sizing_Specification_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "delete from Sizing_SpecificationYarn_Details where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Sizing_Specification_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "delete from Sizing_Specification_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Sizing_Specification_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            trans.Commit()

            Dt1.Dispose()
            Da.Dispose()
            trans.Dispose()
            cmd.Dispose()

            new_record()

            MessageBox.Show("Deleted Sucessfully!!!", "FOR DELETION...", MessageBoxButtons.OKCancel, MessageBoxIcon.Information)

        Catch ex As Exception
            trans.Rollback()
            MessageBox.Show(ex.Message, "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

        If msk_Date.Enabled = True And msk_Date.Visible = True Then msk_Date.Focus()
    End Sub

    Public Sub filter_record() Implements Interface_MDIActions.filter_record

        If Filter_Status = False Then

            Dim da As New SqlClient.SqlDataAdapter
            Dim dt1 As New DataTable
            Dim dt2 As New DataTable
            Dim dt3 As New DataTable

            da = New SqlClient.SqlDataAdapter("select Ledger_DisplayName from Ledger_AlaisHead where (Ledger_IdNo = 0 or Ledger_Type = 'SIZING') order by Ledger_DisplayName", con)
            da.Fill(dt1)
            cbo_Filter_PartyName.DataSource = dt1
            cbo_Filter_PartyName.DisplayMember = "Ledger_DisplayName"

            da = New SqlClient.SqlDataAdapter("select count_name from count_head order by count_name", con)
            da.Fill(dt2)
            cbo_Filter_CountName.DataSource = dt2
            cbo_Filter_CountName.DisplayMember = "count_name"

            da = New SqlClient.SqlDataAdapter("select distinct(EndsCount_Name) from EndsCount_Head order by EndsCount_Name", con)
            da.Fill(dt3)
            cbo_Filter_EndsCount.DataSource = dt3
            cbo_Filter_EndsCount.DisplayMember = "EndsCount_Name"

            dtp_Filter_Fromdate.Text = Common_Procedures.Company_FromDate
            dtp_Filter_ToDate.Text = Common_Procedures.Company_ToDate
            cbo_Filter_PartyName.Text = ""
            cbo_Filter_CountName.Text = ""
            cbo_Filter_EndsCount.Text = ""
            cbo_Filter_MillName.Text = ""
            txt_Filter_SetNo.Text = ""

            cbo_Filter_PartyName.SelectedIndex = -1
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

            da = New SqlClient.SqlDataAdapter("select top 1 Sizing_Specification_No from Sizing_Specification_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Sizing_Specification_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' and Sizing_Specification_Code LIKE '" & Trim(Pk_Condition) & "%' Order by for_Orderby, Sizing_Specification_No", con)
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

            da = New SqlClient.SqlDataAdapter("select top 1 Sizing_Specification_No from Sizing_Specification_Head where for_orderby > " & Str(Val(OrdByNo)) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and Sizing_Specification_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' and Sizing_Specification_Code LIKE '" & Trim(Pk_Condition) & "%' Order by for_Orderby, Sizing_Specification_No", con)
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

            da = New SqlClient.SqlDataAdapter("select top 1 Sizing_Specification_No from Sizing_Specification_Head where for_orderby < " & Str(Val(OrdByNo)) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and Sizing_Specification_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' and Sizing_Specification_Code LIKE '" & Trim(Pk_Condition) & "%' Order by for_Orderby desc, Sizing_Specification_No desc", con)
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
            da = New SqlClient.SqlDataAdapter("select top 1 Sizing_Specification_No from Sizing_Specification_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Sizing_Specification_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' and Sizing_Specification_Code LIKE '" & Trim(Pk_Condition) & "%' Order by for_Orderby desc, Sizing_Specification_No desc", con)
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
        Dim Dt1 As New DataTable

        Try

            clear()

            New_Entry = True
            lbl_NewSTS.Visible = True

            lbl_RefNo.Text = Common_Procedures.get_MaxCode(con, "Sizing_Specification_Head", "Sizing_Specification_Code", "For_OrderBy", "( Sizing_Specification_Code LIKE '" & Trim(Pk_Condition) & "%' )", Val(lbl_Company.Tag), Common_Procedures.FnYearCode)

            lbl_RefNo.ForeColor = Color.Red

            msk_Date.Text = Date.Today.ToShortDateString
            da = New SqlClient.SqlDataAdapter("select top 1 * from Sizing_Specification_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Sizing_Specification_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' and Sizing_Specification_Code LIKE '" & Trim(Pk_Condition) & "%'   Order by for_Orderby desc, Sizing_Specification_No desc", con)
            Dt1 = New DataTable
            da.Fill(Dt1)

            If Dt1.Rows.Count > 0 Then
                If Val(Common_Procedures.settings.PreviousEntryDate_ByDefault) = 1 Then '---- M.S Textiles (Tirupur)
                    If Dt1.Rows(0).Item("Sizing_Specification_Date").ToString <> "" Then msk_Date.Text = Dt1.Rows(0).Item("Sizing_Specification_Date").ToString
                End If
                If Dt1.Rows(0).Item("Selection_Type").ToString <> "" Then cbo_Type.Text = Dt1.Rows(0).Item("Selection_Type").ToString
                If Dt1.Rows(0).Item("YarnStock_Basis").ToString <> "" Then cbo_YarnStock.Text = Dt1.Rows(0).Item("YarnStock_Basis").ToString
                If Dt1.Rows(0).Item("Tds_Percentage").ToString <> "" Then txt_TdsPerc.Text = Val(Dt1.Rows(0).Item("Tds_Percentage").ToString)
                If Dt1.Rows(0).Item("Rewinding_Status").ToString <> "" Then
                    If Val(Dt1.Rows(0).Item("Rewinding_Status").ToString) = 1 Then
                        Chk_RWSts.Checked = True
                    Else
                        Chk_RWSts.Checked = False
                    End If

                End If
                If Dt1.Rows(0).Item("CGST_Percentage").ToString <> "" Then txt_CGST_Percentage.Text = Dt1.Rows(0).Item("CGST_Percentage").ToString
                If Dt1.Rows(0).Item("SGST_Percentage").ToString <> "" Then txt_SGST_Percentage.Text = Dt1.Rows(0).Item("SGST_Percentage").ToString
                If IsDBNull(Dt1.Rows(0).Item("TaxAmount_RoundOff_Status").ToString) = False Then
                    If Val(Dt1.Rows(0).Item("TaxAmount_RoundOff_Status").ToString) = 1 Then chk_TaxAmount_RoundOff_STS.Checked = True Else chk_TaxAmount_RoundOff_STS.Checked = False
                End If

                '                If Dt1.Rows(0).Item("Rate_For").ToString <> "" Then Cbo_RateFor.Text = Dt1.Rows(0).Item("Rate_For").ToString

            End If
            Dt1.Clear()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR NEW RECORD...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Finally
            Dt1.Dispose()
            da.Dispose()

            If msk_Date.Enabled And msk_Date.Visible Then
                msk_Date.Focus()
                msk_Date.SelectionStart = 0
            End If


        End Try

    End Sub

    Public Sub open_record() Implements Interface_MDIActions.open_record
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim movno As String, inpno As String
        Dim RecCode As String

        Try

            inpno = InputBox("Enter Set.No.", "FOR FINDING...")

            'RecCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

            'Da = New SqlClient.SqlDataAdapter("select Sizing_Specification_No from Sizing_Specification_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Sizing_Specification_Code = '" & Trim(RecCode) & "'", con)
            'Da.Fill(Dt)

            RecCode = Trim(inpno)

            'old
            'Da = New SqlClient.SqlDataAdapter("select Sizing_Specification_No from Sizing_Specification_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Set_No = '" & Trim(RecCode) & "'", con)
            Da = New SqlClient.SqlDataAdapter("select Sizing_Specification_No from Sizing_Specification_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Set_No = '" & Trim(RecCode) & "' and Sizing_Specification_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "'", con)
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
                MessageBox.Show("Set.No. does not exists", "DOES NOT FIND...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

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

        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.InsertingEntry, Common_Procedures.UR.Sizing_Specification_Entry, New_Entry, Me) = False Then Exit Sub


        '  If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.Sizing_Specification_Entry, "~L~") = 0 And InStr(Common_Procedures.UR.Sizing_Specification_Entry, "~I~") = 0 Then MessageBox.Show("You have No Rights to Insert", "DOES NOT INSERT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error) : Exit Sub

        Try

            inpno = InputBox("Enter New Ref No.", "FOR NEW REF INSERTION...")

            RecCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

            Da = New SqlClient.SqlDataAdapter("select Sizing_Specification_No from Sizing_Specification_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Sizing_Specification_Code = '" & Trim(RecCode) & "'", con)
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
                    MessageBox.Show("Invalid Ref No", "DOES NOT INSERT NEW REF...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

                Else
                    new_record()
                    Insert_Entry = True
                    lbl_RefNo.Text = Trim(UCase(inpno))

                End If

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT INSERT NEW REF...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Public Sub save_record() Implements Interface_MDIActions.save_record
        Dim cmd As New SqlClient.SqlCommand
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim da3 As New SqlClient.SqlDataAdapter
        Dim tr As SqlClient.SqlTransaction
        Dim NewCode As String = ""
        Dim Led_ID As Integer = 0
        Dim Clo_ID As Integer = 0
        Dim EdsCnt_ID As Integer = 0, vDET_EdsCnt_ID As Integer = 0
        Dim EdsCnt_ID_Header As Integer = 0
        Dim pEdsCnt_Nm As String = ""
        Dim Nr As Integer = 0
        Dim Sno As Integer = 0
        Dim Slno As Integer = 0
        Dim Partcls As String = "", YrnPartcls As String = ""
        Dim PBlNo As String = ""
        Dim vTotPvuBms As Single, vTotPvuMtrs As Single, vTotgrswt As Single, vTotNetwt As Single, vTotWarpwt As Single
        Dim YCnt_ID As Integer = 0
        Dim vTotPvuStk As Single
        Dim YMil_ID As Integer = 0
        Dim ByCnCnt_ID As Integer = 0
        Dim ByCnMil_ID As Integer = 0
        Dim vTotYrnBags As Single, vTotYrnCones As Single, vTotYrnWeight As Single
        Dim EntID As String = ""
        Dim Dup_SetNoBmNo As String = ""
        Dim Mtr_Pc As Single = 0
        Dim pCnt_ID As Integer
        Dim pEds_Nm As String
        Dim vSetCd As String
        Dim Selc_SetCode As String
        Dim VouBil As String = ""
        Dim Del_ID As Integer, Rec_ID As Integer
        Dim Stock_In As String
        Dim mtrspcs As Single = 0
        Dim RWStatus As Integer = 0
        Dim YrnTyp As String = ""
        Dim YrnBgs As Single = 0
        Dim YrnCns As Single = 0
        Dim YrnWgt As Single = 0
        Dim StkAt_IdNo As Integer = 0
        Dim SizPvuRecCode As String = "", SizPvuRecNo As String = ""
        Dim Bw_ID As Integer = 0
        Dim vSetNo As String = ""
        Dim Usr_ID As Integer = 0
        Dim vVou_BlAmt As Double = 0
        Dim RndOff_STS As Integer = 0
        Dim OurOrd_No As String = ""
        Dim vINVDATE As String
        Dim vSET_COMPIDNO As Integer = 0
        Dim Verified_STS As String = ""
        Dim vOrdByNo As String = ""
        Dim vLoomType_Idno As Integer = 0
        Dim vLOT_ENT_REFCODE As String = ""
        Dim vSTKPAVUMTRS As String = 0
        Dim vCREATED_DTTM_TXT As String = ""
        Dim vMODIFIED_DTTM_TXT As String = ""
        vOrdByNo = Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text)

        Verified_STS = 0
        If chk_Verified_Status.Checked = True Then Verified_STS = 1


        If Val(lbl_Company.Tag) = 0 Then
            MessageBox.Show("Invalid Company Selection", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
        End If

        If Val(Common_Procedures.User.IdNo) = 0 Then
            MessageBox.Show("Invalid User Name", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
        End If

        NewCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)


        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.SavingEntry, Common_Procedures.UR.Sizing_Specification_Entry, New_Entry, Me, con, "Sizing_Specification_Head", "Sizing_Specification_Code", NewCode, "Sizing_Specification_Date", "(Sizing_Specification_Code = '" & Trim(NewCode) & "')", "(Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Sizing_Specification_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "')", "for_Orderby desc, Sizing_Specification_No desc", dtp_Date.Value.Date) = False Then Exit Sub

        '  If Common_Procedures.UserRight_Check(Common_Procedures.UR.Sizing_Specification_Entry, New_Entry) = False Then Exit Sub

        If pnl_Back.Enabled = False Then
            MessageBox.Show("Close Other Windows", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
        End If

        If IsDate(msk_Date.Text) = False Then
            MessageBox.Show("Invalid Date", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If msk_Date.Enabled And msk_Date.Visible Then msk_Date.Focus()
            Exit Sub
        End If

        If Not (Convert.ToDateTime(msk_Date.Text) >= Common_Procedures.Company_FromDate And Convert.ToDateTime(msk_Date.Text) <= Common_Procedures.Company_ToDate) Then
            MessageBox.Show("Date is out of financial range", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If msk_Date.Enabled And msk_Date.Visible Then msk_Date.Focus()
            Exit Sub
        End If

        Led_ID = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Sizing.Text)
        If Led_ID = 0 Then
            MessageBox.Show("Invalid Party Name", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If cbo_Sizing.Enabled And cbo_Sizing.Visible Then cbo_Sizing.Focus()
            Exit Sub
        End If
        'If Trim(lbl_OrderCode.Text) <> "" Then
        '    If Led_ID <> 0 Then
        '        Da = New SqlClient.SqlDataAdapter("select a.*,b.* from Own_Order_Head a INNER JOIN Own_order_Sizing_Details b ON a.Own_Order_Code =b.Own_Order_Code where a.Own_Order_Code = '" & Trim(lbl_OrderCode.Text) & "' and  b.Ledger_idno = " & Str(Val(Led_ID)), con)
        '        Dt1 = New DataTable
        '        Da.Fill(Dt1)
        '        If Dt1.Rows.Count > 0 Then
        '            OurOrd_No = Dt1.Rows(0).Item("Order_No").ToString
        '        End If
        '        Dt1.Clear()
        '    End If
        '    If Trim(OurOrd_No) <> Trim(lbl_OrderNo.Text) Then
        '        MessageBox.Show("Invalid Mismatch Of Order No for this Party Name", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
        '        If cbo_Sizing.Enabled And cbo_Sizing.Visible Then cbo_Sizing.Focus()
        '        Exit Sub
        '    End If
        'End If
        If Trim(txt_SetNo.Text) = "" Then
            MessageBox.Show("Invalid Set No", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If txt_SetNo.Enabled And txt_SetNo.Visible Then txt_SetNo.Focus()
            Exit Sub
        End If

        If Trim(UCase(cbo_Type.Text)) = Trim(UCase("SIZING-UNIT PAVU DELIVERY")) Then
            vSetCd = "SSPDC-SZSPC-" & Trim(Val(lbl_Company.Tag)) & "-" & Trim(txt_SetNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)
            Selc_SetCode = Trim(txt_SetNo.Text) & "/" & Trim(Common_Procedures.FnYearCode) & "/" & Trim(Val(lbl_Company.Tag)) & "/SZSPC-/SSPDC-"
            lbl_SetCode.Text = Trim(vSetCd)

        Else

            If Trim(lbl_SetCode.Text) = "" Or Trim(UCase(cbo_Type.Text)) = "DIRECT" Or (Trim(UCase(cbo_Type.Text)) <> "RECEIPT" And Trim(UCase(cbo_Type.Text)) <> "SIZING-UNIT PAVU DELIVERY") Then

                vSetCd = Trim(Val(lbl_Company.Tag)) & "-" & Trim(txt_SetNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)
                Selc_SetCode = Trim(txt_SetNo.Text) & "/" & Trim(Common_Procedures.FnYearCode) & "/" & Trim(Val(lbl_Company.Tag))
                lbl_SetCode.Text = Trim(vSetCd)

            Else

                vSetCd = Trim(lbl_SetCode.Text)

                vSET_COMPIDNO = 0
                Da = New SqlClient.SqlDataAdapter("Select a.* from Sizing_Pavu_Receipt_Details a Where a.set_Code = '" & Trim(lbl_SetCode.Text) & "'", con)
                Dt1 = New DataTable
                Da.Fill(Dt1)
                If Dt1.Rows.Count > 0 Then
                    vSET_COMPIDNO = Dt1.Rows(0).Item("company_idno").ToString
                End If
                Dt1.Clear()

                If vSET_COMPIDNO = 0 Then
                    vSET_COMPIDNO = Val(lbl_Company.Tag)
                End If

                Selc_SetCode = Trim(txt_SetNo.Text) & "/" & Trim(Common_Procedures.FnYearCode) & "/" & Trim(Val(vSET_COMPIDNO))

            End If

        End If

        If Trim(lbl_SetCode.Text) = "" Then
            MessageBox.Show("Invalid SetCode", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If txt_SetNo.Enabled And txt_SetNo.Visible Then txt_SetNo.Focus()
            Exit Sub
        End If

        lbl_UserName.Text = Common_Procedures.User.IdNo

        EdsCnt_ID_Header = 0
        If cbo_EndsCount.Visible Then
            EdsCnt_ID_Header = Common_Procedures.EndsCount_NameToIdNo(con, cbo_EndsCount.Text)
        End If
        If EdsCnt_ID_Header = 0 Then EdsCnt_ID_Header = Common_Procedures.EndsCount_NameToIdNo(con, dgv_PavuDetails.Rows(0).Cells(dgvCol_Details.Ends_Count).Value)
        '    MessageBox.Show("Invalid Ends Count", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
        '    If cbo_EndsCount.Enabled And cbo_EndsCount.Visible Then cbo_EndsCount.Focus()
        '    Exit Sub
        'End If

        If txt_TotalBeams.Visible = True And Val(txt_TotalBeams.Text) = 0 Then
            MessageBox.Show("Invalid Total Order Beams for this Set", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If txt_TotalBeams.Enabled And txt_TotalBeams.Visible Then txt_TotalBeams.Focus()
            Exit Sub
        End If

        Usr_ID = Common_Procedures.User_NameToIdNo(con1, lbl_UserName.Text)

        With dgv_PavuDetails

            For i = 0 To .RowCount - 1

                If Trim(.Rows(i).Cells(dgvCol_Details.Beam_No).Value) <> "" Or Val(.Rows(i).Cells(dgvCol_Details.meters).Value) <> 0 Then

                    If Trim(.Rows(i).Cells(dgvCol_Details.Beam_No).Value) = "" Then
                        MessageBox.Show("Invalid Beam No", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                        If .Enabled And .Visible Then
                            .Focus()
                            .CurrentCell = .Rows(i).Cells(dgvCol_Details.Beam_No)
                            .CurrentCell.Selected = True
                        End If
                        Exit Sub
                    End If

                    If InStr(1, Trim(.Rows(i).Cells(dgvCol_Details.Beam_No).Value), " ") > 0 Then
                        MessageBox.Show("Invalid Beam No, Spaces not allowed in BeamNo", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                        If .Enabled And .Visible Then
                            .Focus()
                            .CurrentCell = .Rows(i).Cells(dgvCol_Details.Beam_No)
                            .CurrentCell.Selected = True
                        End If
                        Exit Sub
                    End If


                    If Val(.Rows(i).Cells(dgvCol_Details.meters).Value) = 0 Then
                        MessageBox.Show("Invalid Meters", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                        If .Enabled And .Visible Then
                            .Focus()
                            .CurrentCell = .Rows(i).Cells(dgvCol_Details.meters)
                            .CurrentCell.Selected = True
                        End If
                        Exit Sub
                    End If

                    If InStr(1, Trim(UCase(Dup_SetNoBmNo)), "~" & Trim(UCase(.Rows(i).Cells(dgvCol_Details.Beam_No).Value)) & "~") > 0 Then
                        MessageBox.Show("Duplicate BeamNo ", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                        If .Enabled And .Visible Then
                            .Focus()
                            .CurrentCell = .Rows(i).Cells(dgvCol_Details.Beam_No)
                            .CurrentCell.Selected = True
                        End If
                        Exit Sub
                    End If

                    Dup_SetNoBmNo = Trim(Dup_SetNoBmNo) & "~" & Trim(UCase(.Rows(i).Cells(dgvCol_Details.Beam_No).Value)) & "~"

                    If Trim(.Rows(i).Cells(dgvCol_Details.Ends_Count).Value) = "" Then
                        MessageBox.Show("Invalid Ends Count", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        If .Enabled And .Visible Then
                            .Focus()
                            .CurrentCell = .Rows(i).Cells(dgvCol_Details.Ends_Count)
                            .CurrentCell.Selected = True
                        End If
                        Exit Sub
                    End If

                End If

            Next i
        End With

        vTotPvuBms = 0 : vTotPvuMtrs = 0 : vTotgrswt = 0 : vTotNetwt = 0 : vTotWarpwt = 0

        If dgv_PavuDetails_Total.RowCount > 0 Then
            vTotPvuBms = Val(dgv_PavuDetails_Total.Rows(0).Cells(dgvCol_Details.Beam_No).Value())
            vTotPvuMtrs = Val(dgv_PavuDetails_Total.Rows(0).Cells(dgvCol_Details.meters).Value())
            vTotgrswt = Val(dgv_PavuDetails_Total.Rows(0).Cells(dgvCol_Details.Gross_Weight).Value())
            vTotNetwt = Val(dgv_PavuDetails_Total.Rows(0).Cells(dgvCol_Details.Net_Weight).Value())
            vTotWarpwt = Val(dgv_PavuDetails_Total.Rows(0).Cells(dgvCol_Details.Warp_Weight).Value())
        End If

        For i = 0 To dgv_YarnDetails.RowCount - 1

            If Val(dgv_YarnDetails.Rows(i).Cells(6).Value) <> 0 Then

                YCnt_ID = Common_Procedures.Count_NameToIdNo(con, dgv_YarnDetails.Rows(i).Cells(1).Value)
                If Val(YCnt_ID) = 0 Then
                    MessageBox.Show("Invalid CountName", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                    dgv_YarnDetails.CurrentCell = dgv_YarnDetails.Rows(0).Cells(1)
                    dgv_YarnDetails.Focus()
                    Exit Sub
                End If

                If Trim(dgv_YarnDetails.Rows(i).Cells(2).Value) = "" Then
                    MessageBox.Show("Invalid Yarn Type", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                    dgv_YarnDetails.CurrentCell = dgv_YarnDetails.Rows(0).Cells(2)
                    dgv_YarnDetails.Focus()
                    Exit Sub
                End If

                YMil_ID = Common_Procedures.Mill_NameToIdNo(con, dgv_YarnDetails.Rows(i).Cells(3).Value)
                If Val(YMil_ID) = 0 Then
                    MessageBox.Show("Invalid Mill Name", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                    dgv_YarnDetails.CurrentCell = dgv_YarnDetails.Rows(0).Cells(3)
                    dgv_YarnDetails.Focus()
                    Exit Sub
                End If



            End If

        Next

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1019" Then '---- SUBHAM Textiles (Somanur)
            If Trim(txt_SetNo.Text) <> "" Then
                NewCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)
                Da = New SqlClient.SqlDataAdapter("select * from Sizing_Specification_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Sizing_Specification_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "' and Ledger_IdNo = " & Str(Val(Led_ID)) & " and Set_No = '" & Trim(txt_SetNo.Text) & "' and Sizing_Specification_Code <> '" & Trim(NewCode) & "'", con)
                Dt1 = New DataTable
                Da.Fill(Dt1)
                If Dt1.Rows.Count > 0 Then
                    MessageBox.Show("Duplicate Set No to this Party", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                    If txt_SetNo.Enabled And txt_SetNo.Visible Then txt_SetNo.Focus()
                    Exit Sub
                End If
                Dt1.Clear()
            End If

            If Trim(txt_InvNo.Text) <> "" Then
                NewCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)
                Da = New SqlClient.SqlDataAdapter("select * from Sizing_Specification_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Sizing_Specification_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "' and Ledger_IdNo = " & Str(Val(Led_ID)) & " and Sizing_Invoice_No = '" & Trim(txt_InvNo.Text) & "' and Sizing_Specification_Code <> '" & Trim(NewCode) & "'", con)
                Dt1 = New DataTable
                Da.Fill(Dt1)
                If Dt1.Rows.Count > 0 Then
                    MessageBox.Show("Duplicate Invoice No to this Party", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                    If txt_InvNo.Enabled And txt_InvNo.Visible Then txt_InvNo.Focus()
                    Exit Sub
                End If
                Dt1.Clear()
            End If
        End If

        If Val(lbl_NetAmount.Text) <> 0 Then
            If IsDate(msk_InvoiceDate.Text) = False Then
                MessageBox.Show("Invalid Invoice Date", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                If msk_InvoiceDate.Enabled And msk_InvoiceDate.Visible Then msk_InvoiceDate.Focus()
                Exit Sub
            End If
        End If

        vTotYrnBags = 0 : vTotYrnCones = 0 : vTotYrnWeight = 0
        If dgv_YarnDetails_Total.RowCount > 0 Then
            vTotYrnBags = Val(dgv_YarnDetails_Total.Rows(0).Cells(4).Value())
            vTotYrnCones = Val(dgv_YarnDetails_Total.Rows(0).Cells(5).Value())
            vTotYrnWeight = Val(dgv_YarnDetails_Total.Rows(0).Cells(6).Value())
        End If

        RndOff_STS = 0
        If chk_TaxAmount_RoundOff_STS.Checked = True Then RndOff_STS = 1

        If Val(lbl_NetAmount.Text) = 0 Then lbl_NetAmount.Text = "0"

        tr = con.BeginTransaction


        Try

            If Insert_Entry = True Or New_Entry = False Then

                NewCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            Else

                lbl_RefNo.Text = Common_Procedures.get_MaxCode(con, "Sizing_Specification_Head", "Sizing_Specification_Code", "For_OrderBy", "Sizing_Specification_Code LIKE '" & Trim(Pk_Condition) & "%'", Val(lbl_Company.Tag), Common_Procedures.FnYearCode, tr)

                NewCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            End If

            RWStatus = 0
            If Chk_RWSts.Checked = True Then RWStatus = 1

            cmd.Connection = con
            cmd.Transaction = tr


            cmd.Parameters.Clear()
            cmd.Parameters.AddWithValue("@EntryDate", Convert.ToDateTime(msk_Date.Text))
            If IsDate(msk_InvoiceDate.Text) = True Then
                cmd.Parameters.AddWithValue("@InvoiceDate", Convert.ToDateTime(msk_InvoiceDate.Text))
            End If

            cmd.CommandText = "Truncate Table TempTable_For_NegativeStock"
            cmd.ExecuteNonQuery()


            vCREATED_DTTM_TXT = ""
            vMODIFIED_DTTM_TXT = ""

            vCREATED_DTTM_TXT = Trim(Format(Now, "dd-MM-yyyy hh:mm tt"))
            cmd.Parameters.AddWithValue("@createddatetime", Now)


            vMODIFIED_DTTM_TXT = Trim(Format(Now, "dd-MM-yyyy hh:mm tt"))
            cmd.Parameters.AddWithValue("@modifieddatetime", Now)

            If New_Entry = True Then

                cmd.CommandText = "Insert into Sizing_Specification_Head( Sizing_Specification_Code, Company_IdNo                     , Sizing_Specification_No       , for_OrderBy                                                            , Sizing_Specification_Date, Ledger_IdNo             , Set_No                        ,       Set_Code    , EndsCount_IdNo             ,Pcs_Length                      ,Excess_Short                        , BabyCone_bags               , BabyCone_Weight              , Rewinding_Status    , Rewinding_Bags              ,Rewinding_Cones            ,Rewinding_Excess           ,Sizing_Invoice_No              , Amount                          , Total_Beam                  , Total_Meters                 , Total_Bags                   , Total_Cones                   , Total_Weight                     , Average_Count                     , YarnStock_Basis                    , Tape_Length                           , PickUp_Perc                        , Elongation                         ,Tds_Percentage                       , Tds_Amount                          , Net_Amount                                 , Invoice_Date , Selection_Type                 , Add_Less                          ,Consumed_Yarn                            ,Invoice_Rate                        , Packing_Rate                          ,Warp_Meters                         , Winding_Rate                            , Yarn_Taken                             , BeamCount_Type                         , Winding_Amount                      , Packing_Amount                        , Gross_Amount                        , Total_PlanBeams                                                                              ,  User_idNo                    ,Total_Taxable_Amount                    ,CGST_Percentage                           ,CGST_Amount                           ,SGST_Percentage                            ,                SGST_Amount           ,     TaxAmount_RoundOff_Status , Our_Order_No     , Own_Order_Code                                      , RoundOff_Amount      ,Verified_Status                          ,Total_Pavu_Gross_Weight,Total_Pavu_net_Weight,Total_Pavu_warp_Weight       ,        Sizing_JobCode_forSelection  ,     Sizing_Elongation_Percentage            ,  Sizing_PickUp_Percentage        ,   Rate_For                       ,               ClothSales_OrderCode_forSelection                  , Sizing_net_weight                     ,    created_useridno           ,   created_DateTime,          created_DateTime_Text    , Last_modified_useridno, Last_modified_DateTime, Last_modified_DateTime_Text  ) " &
                                                            " Values ('" & Trim(NewCode) & "'   , " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_RefNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text))) & ", @EntryDate               , " & Str(Val(Led_ID)) & ", '" & Trim(txt_SetNo.Text) & "', '" & Trim(vSetCd) & "', " & Str(Val(EdsCnt_ID_Header)) & ", " & Val(txt_PcsLength.Text) & ", " & Val(txt_ExcessShort.Text) & " ," & Val(txt_BabyBag.Text) & " , " & Val(txt_BabyWt.Text) & " ," & Val(RWStatus) & "," & Val(txt_RwBags.Text) & " ," & Val(txt_RwCns.Text) & ", " & Val(txt_RWES.Text) & ", '" & Trim(txt_InvNo.Text) & "', " & Val(lbl_InvoiceAmt.Text) & ", " & Str(Val(vTotPvuBms)) & ", " & Str(Val(vTotPvuMtrs)) & ", " & Str(Val(vTotYrnBags)) & ", " & Str(Val(vTotYrnCones)) & ", " & Str(Val(vTotYrnWeight)) & " ,  '" & Trim(lbl_Avg_Count.Text) & "', '" & Trim(cbo_YarnStock.Text) & "' , " & Str(Val(txt_TapeLength.Text)) & " , " & Str(Val(txt_PickUp.Text)) & " ," & Str(Val(lbl_Elogation.Text)) & " , " & Str(Val(txt_TdsPerc.Text)) & " , " & Str(Val(lbl_TdsAmount.Text)) & " , " & Str(Val(CSng(lbl_NetAmount.Text))) & " , " & IIf(IsDate(msk_InvoiceDate.Text) = True, "@InvoiceDate", "Null") & ", '" & Trim(cbo_Type.Text) & "' , " & Str(Val(txt_AddLess.Text)) & " , " & Str(Val(txt_ConsumedYarn.Text)) & " ," & Str(Val(txt_InvRate.Text)) & " , " & Str(Val(txt_PackingRate.Text)) & " , " & Str(Val(txt_WarpMtr.Text)) & " ,  " & Str(Val(txt_WindingRate.Text)) & " , " & Str(Val(txt_YarnTaken.Text)) & " , '" & Trim(cbo_BeamCount_Type.Text) & "' ," & Str(Val(lbl_WindingAmt.Text)) & " , " & Str(Val(lbl_PackingAmt.Text)) & " ," & Str(Val(lbl_GrossAmt.Text)) & " ," & Str(Val(txt_TotalBeams.Text)) & ", " & Val(Common_Procedures.User.IdNo) & "," & Str(Val(lbl_Taxable_Value.Text)) & "," & Str(Val(txt_CGST_Percentage.Text)) & "," & Str(Val(lbl_CGST_Amount.Text)) & "," & Str(Val(txt_SGST_Percentage.Text)) & "," & Str(Val(lbl_SGST_Amount.Text)) & ", " & Str(Val(RndOff_STS)) & " ,'" & Trim(lbl_OrderNo.Text) & "' ,    '" & Trim(lbl_OrderCode.Text) & "' , " & Val(lbl_RoundOff.Text) & ", " & Val(Verified_STS) & "," & Val(vTotgrswt) & "," & Val(vTotNetwt) & "," & Val(vTotWarpwt) & ", '" & Trim(cbo_jobcardno.Text) & "'    ," & Val(lbl_Elongation_Perc.Text) & "," & Val(lbl_PickUp_Perc.Text) & " , '" & Trim(Cbo_RateFor.Text) & "' ,   '" & Trim(cbo_ClothSales_OrderCode_forSelection.Text) & "'     , " & Val(txt_Sizing_Net_Wgt.Text) & "  ,        " & Str(Val(Common_Procedures.User.IdNo)) & ",  @createddatetime ,  '" & Trim(vCREATED_DTTM_TXT) & "',              0        ,     NUll              ,          ''      ) "
                cmd.ExecuteNonQuery()

            Else

                Call Common_Procedures.User_Modification_Updation(con, "HEAD", Me.Name, "OLD", "Sizing_Specification_head", "Sizing_Specification_Code", Val(lbl_Company.Tag), NewCode, lbl_RefNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, False, "", "", "Sizing_Specification_Code,Sizing_Specification_Date, Company_IdNo, for_OrderBy", tr)
                Call Common_Procedures.User_Modification_Updation(con, "DETAILS", Me.Name, "OLD", "Sizing_SpecificationPavu_Details", "Sizing_Specification_Code", Val(lbl_Company.Tag), NewCode, lbl_RefNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, False, " Beam_No ,Noof_Pcs,Meters_Pc,Meters,   Beam_Width_IdNo    ,     Sizing_Pavu_Receipt_No,      Sizing_Pavu_Receipt_Code", "Sl_No", "Sizing_Specification_Code, For_OrderBy, Company_IdNo, Sizing_Specification_No, Sizing_Specification_Date, Ledger_Idno", tr)
                Call Common_Procedures.User_Modification_Updation(con, "DETAILS", Me.Name, "OLD", "Sizing_SpecificationYarn_Details", "Sizing_Specification_Code", Val(lbl_Company.Tag), NewCode, lbl_RefNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, False, " count_idno, Yarn_Type, Mill_IdNo, Bags, Cones, Weight", "Sl_No", "Sizing_Specification_Code, For_OrderBy, Company_IdNo, Sizing_Specification_No, Sizing_Specification_Date, Ledger_Idno", tr)

                cmd.CommandText = "Update Sizing_Specification_Head set Sizing_Specification_Date = @EntryDate, Ledger_IdNo = " & Str(Val(Led_ID)) & ", Set_No = '" & Trim(txt_SetNo.Text) & "',  Set_Code = '" & Trim(vSetCd) & "', EndsCount_IdNo = " & Str(Val(EdsCnt_ID_Header)) & ", Pcs_Length = '" & Trim(txt_PcsLength.Text) & "' ,Excess_Short = " & Val(txt_ExcessShort.Text) & " ,BabyCone_bags = " & Val(txt_BabyBag.Text) & " ,BabyCone_Weight = " & Val(txt_BabyWt.Text) & " ,Rewinding_Status = " & Val(RWStatus) & " ,Rewinding_Bags = " & Val(txt_RwBags.Text) & " , Rewinding_Cones = " & Val(txt_RwCns.Text) & ",Rewinding_Excess = " & Val(txt_RWES.Text) & ",Sizing_Invoice_No = '" & Trim(txt_InvNo.Text) & "', Amount = " & Val(lbl_InvoiceAmt.Text) & " , Total_Beam = " & Str(Val(vTotPvuBms)) & ", Total_Meters = " & Str(Val(vTotPvuMtrs)) & ", Total_Bags = " & Str(Val(vTotYrnBags)) & ", Total_Cones = " & Str(Val(vTotYrnCones)) & ", Total_Weight = " & Str(Val(vTotYrnWeight)) & " , Average_Count = '" & Trim(lbl_Avg_Count.Text) & "' , YarnStock_Basis = '" & Trim(cbo_YarnStock.Text) & "' , Tape_Length = " & Str(Val(txt_TapeLength.Text)) & " , PickUp_Perc =  " & Str(Val(txt_PickUp.Text)) & "   , Elongation = " & Str(Val(lbl_Elogation.Text)) & " ,Tds_Percentage =  " & Str(Val(txt_TdsPerc.Text)) & "   , Tds_Amount =  " & Str(Val(lbl_TdsAmount.Text)) & ", Net_Amount =  " & Str(Val(CSng(lbl_NetAmount.Text))) & ", Invoice_Date =  " & IIf(IsDate(msk_InvoiceDate.Text) = True, "@InvoiceDate", "Null") & ",  Selection_Type = '" & Trim(cbo_Type.Text) & "' , Add_Less = " & Str(Val(txt_AddLess.Text)) & " ,Consumed_Yarn = " & Str(Val(txt_ConsumedYarn.Text)) & " ,Invoice_Rate = " & Str(Val(txt_InvRate.Text)) & " , Packing_Rate = " & Str(Val(txt_PackingRate.Text)) & " , Warp_Meters = " & Str(Val(txt_WarpMtr.Text)) & " , Winding_Rate = " & Str(Val(txt_WindingRate.Text)) & " , Yarn_Taken = " & Str(Val(txt_YarnTaken.Text)) & " , BeamCount_Type = '" & Trim(cbo_BeamCount_Type.Text) & "' , Winding_Amount = " & Str(Val(lbl_WindingAmt.Text)) & " , Packing_Amount = " & Str(Val(lbl_PackingAmt.Text)) & " , Gross_Amount =  " & Str(Val(lbl_GrossAmt.Text)) & "  , Total_PlanBeams =  " & Str(Val(txt_TotalBeams.Text)) & ",  User_idNo = " & Val(Common_Procedures.User.IdNo) & ",Total_Taxable_Amount =" & Str(Val(lbl_Taxable_Value.Text)) & ",CGST_Percentage =" & Str(Val(txt_CGST_Percentage.Text)) & " ,CGST_Amount =" & Str(Val(lbl_CGST_Amount.Text)) & " ,SGST_Percentage =" & Str(Val(txt_SGST_Percentage.Text)) & ",SGST_Amount =" & Str(Val(lbl_SGST_Amount.Text)) & ", TaxAmount_RoundOff_Status = " & Str(Val(RndOff_STS)) & ",Our_Order_No = '" & Trim(lbl_OrderNo.Text) & "',Own_Order_Code = '" & Trim(lbl_OrderCode.Text) & "',RoundOff_Amount =  " & Val(lbl_RoundOff.Text) & " ,Verified_Status= " & Val(Verified_STS) & " ,Total_Pavu_Gross_Weight=" & Val(vTotgrswt) & ",Total_Pavu_net_Weight=" & Val(vTotNetwt) & ",Total_Pavu_Warp_Weight=" & Val(vTotWarpwt) & ", Sizing_JobCode_forSelection ='" & Trim(cbo_jobcardno.Text) & "' , Sizing_Elongation_Percentage =" & Val(lbl_Elongation_Perc.Text) & "  ,  Sizing_PickUp_Percentage = " & Val(lbl_PickUp_Perc.Text) & "   ,Rate_For =  '" & Trim(Cbo_RateFor.Text) & "' , ClothSales_OrderCode_forSelection = '" & Trim(cbo_ClothSales_OrderCode_forSelection.Text) & "' , Sizing_net_weight = " & Val(txt_Sizing_Net_Wgt.Text) & "  , Last_modified_useridno = " & Str(Val(Common_Procedures.User.IdNo)) & ", Last_modified_DateTime = @modifieddatetime, Last_modified_DateTime_Text = '" & Trim(vMODIFIED_DTTM_TXT) & "' Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & "  and Sizing_Specification_Code = '" & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()

                Da = New SqlClient.SqlDataAdapter("select * from Sizing_Pavu_Receipt_Head Where Sizing_Specification_Code = '" & Trim(NewCode) & "'", con)
                Da.SelectCommand.Transaction = tr
                Dt1 = New DataTable
                Da.Fill(Dt1)
                If Dt1.Rows.Count > 0 Then

                    For i = 0 To Dt1.Rows.Count - 1

                        If Val(Dt1.Rows(i).Item("Total_Meters").ToString) <> 0 Then

                            EntID = Trim(Pk_Condition3) & Trim(Dt1.Rows(i).Item("Sizing_Pavu_Receipt_No").ToString)
                            Partcls = "Pavu Rcpt : Set No. " & Trim(Dt1.Rows(i).Item("Set_No").ToString)
                            PBlNo = Trim(Dt1.Rows(i).Item("Set_No").ToString)

                            pEdsCnt_Nm = Common_Procedures.get_FieldValue(con, "EndsCount_Head", "EndsCount_Name", "(EndsCount_IdNo = " & Str(Val(Dt1.Rows(i).Item("EndsCount_IdNo").ToString)) & ")", , tr)
                            pCnt_ID = Val(Common_Procedures.get_FieldValue(con, "EndsCount_Head", "Count_IdNo", "(EndsCount_IdNo = " & Str(Val(Dt1.Rows(i).Item("EndsCount_IdNo").ToString)) & ")", , tr))

                            YrnPartcls = Partcls & ", EndsCount : " & Trim(pEdsCnt_Nm) & ", Beams : " & Trim(Val(Dt1.Rows(i).Item("Total_Beam").ToString)) & ", Meters : " & Trim(Val(Dt1.Rows(i).Item("Total_Meters").ToString))

                            cmd.CommandText = "Delete from Stock_Yarn_Processing_Details Where Reference_Code = '" & Trim(Pk_Condition3) & Trim(Dt1.Rows(i).Item("Sizing_Pavu_Receipt_Code").ToString) & "'"
                            cmd.ExecuteNonQuery()

                            cmd.Parameters.Clear()
                            cmd.Parameters.AddWithValue("@PavuReceiptDate", Dt1.Rows(i).Item("Sizing_Pavu_Receipt_Date"))

                            cmd.CommandText = "Insert into Stock_Yarn_Processing_Details ( Reference_Code, Company_IdNo, Reference_No, for_OrderBy, Reference_Date, DeliveryTo_Idno, ReceivedFrom_Idno, Entry_ID, Particulars, Party_Bill_No, Sl_No, Count_IdNo, Yarn_Type, Mill_IdNo, Bags, Cones, Weight ,ClothSales_OrderCode_forSelection) Values ( '" & Trim(Pk_Condition3) & Trim(Dt1.Rows(i).Item("Sizing_Pavu_Receipt_Code").ToString) & "', " & Str(Val(Dt1.Rows(i).Item("Company_IdNo").ToString)) & ", '" & Trim(Dt1.Rows(i).Item("Sizing_Pavu_Receipt_No").ToString) & "', " & Str(Val(Dt1.Rows(i).Item("for_OrderBy").ToString)) & ", @PavuReceiptDate, 0, " & Str(Val(Dt1.Rows(i).Item("Ledger_IdNo").ToString)) & ", '" & Trim(EntID) & "', '" & Trim(YrnPartcls) & "', '" & Trim(PBlNo) & "', 1, " & Str(Val(pCnt_ID)) & ", 'MILL', 0, 0, 0, 0 ,'" & Trim(cbo_ClothSales_OrderCode_forSelection.Text) & "')"
                            cmd.ExecuteNonQuery()

                        End If

                    Next

                End If

                Call Common_Procedures.User_Modification_Updation(con, "HEAD", Me.Name, "NEW", "Sizing_Specification_head", "Sizing_Specification_Code", Val(lbl_Company.Tag), NewCode, lbl_RefNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, False, "", "", "Sizing_Specification_Code,Sizing_Specification_Date, Company_IdNo, for_OrderBy", tr)

                Dt1.Clear()

                cmd.Parameters.Clear()
                cmd.Parameters.AddWithValue("@EntryDate", Convert.ToDateTime(msk_Date.Text))
                If IsDate(msk_InvoiceDate.Text) = True Then
                    cmd.Parameters.AddWithValue("@InvoiceDate", Convert.ToDateTime(msk_InvoiceDate.Text))
                End If

                cmd.CommandText = "Update Sizing_Pavu_Receipt_Head set Sizing_Specification_Code = '' Where Sizing_Specification_Code = '" & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()

                cmd.CommandText = "Update SizSoft_Pavu_Delivery_Head set Sizing_Specification_Code = '' Where Sizing_Specification_Code = '" & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()

            End If

            EntID = Trim(Pk_Condition) & Trim(lbl_RefNo.Text)
            Partcls = "Stmt : Set.No. " & Trim(txt_SetNo.Text)
            PBlNo = Trim(txt_SetNo.Text)

            cmd.CommandText = "Delete from Sizing_SpecificationYarn_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Sizing_Specification_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Stock_Pavu_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Stock_Yarn_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Stock_Empty_BeamBagCone_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Stock_SizedPavu_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' and Pavu_Delivery_Code = '' and Pavu_Delivery_Increment = 0 and Beam_Knotting_Code = '' and Loom_Idno = 0 and Production_Meters = 0 and Close_Status = 0"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Sizing_SpecificationPavu_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Sizing_Specification_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Stock_BabyCone_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'  and Delivered_Weight = 0"
            cmd.ExecuteNonQuery()

            'pCnt_ID = Val(Common_Procedures.get_FieldValue(con, "EndsCount_Head", "Count_IdNo", "(EndsCount_IdNo = " & Str(Val(EdsCnt_ID)) & ")", , tr))
            'pEds_Nm = Val(Common_Procedures.get_FieldValue(con, "EndsCount_Head", "Ends_Name", "(EndsCount_IdNo = " & Str(Val(EdsCnt_ID)) & ")", , tr))

            StkAt_IdNo = Val(Led_ID)
            If Val(Common_Procedures.settings.SizingSpecification_AutoTransfer_PavuStock_To_Godown) = 1 Then
                StkAt_IdNo = Val(Common_Procedures.CommonLedger.Godown_Ac)
            End If

            cmd.CommandText = "Truncate table " & Trim(Common_Procedures.EntryTempTable)
            cmd.ExecuteNonQuery()


            With dgv_PavuDetails
                Sno = 0
                For i = 0 To dgv_PavuDetails.RowCount - 1

                    If Val(dgv_PavuDetails.Rows(i).Cells(dgvCol_Details.meters).Value) <> 0 Then

                        Sno = Sno + 1

                        Bw_ID = Common_Procedures.BeamWidth_NameToIdNo(con, .Rows(i).Cells(dgvCol_Details.beam_width).Value, tr)
                        vDET_EdsCnt_ID = Common_Procedures.EndsCount_NameToIdNo(con, .Rows(i).Cells(dgvCol_Details.Ends_Count).Value, tr)
                        '                        EdsCnt_ID = Common_Procedures.EndsCount_NameToIdNo(con, .Rows(i).Cells(dgvCol_Details.Ends_Count).Value, tr)

                        pCnt_ID = Val(Common_Procedures.get_FieldValue(con, "EndsCount_Head", "Count_IdNo", "(EndsCount_IdNo = " & Str(Val(vDET_EdsCnt_ID)) & ")", , tr))
                        pEds_Nm = Val(Common_Procedures.get_FieldValue(con, "EndsCount_Head", "Ends_Name", "(EndsCount_IdNo = " & Str(Val(vDET_EdsCnt_ID)) & ")", , tr))

                        vLoomType_Idno = Common_Procedures.LoomType_NameToIdNo(con, .Rows(i).Cells(dgvCol_Details.Beam_Type).Value, tr)

                        Mtr_Pc = 0
                        If Val(.Rows(i).Cells(dgvCol_Details.Pcs).Value) <> 0 Then
                            Mtr_Pc = Format(Val(.Rows(i).Cells(dgvCol_Details.meters).Value) / Val(.Rows(i).Cells(dgvCol_Details.Pcs).Value), "#########0.00")
                        End If

                        SizPvuRecCode = ""
                        SizPvuRecNo = ""
                        If Trim(UCase(cbo_Type.Text)) = "RECEIPT" Then

                            SizPvuRecCode = Trim(.Rows(i).Cells(dgvCol_Details.pavu_receipt_code).Value)

                            SizPvuRecNo = ""
                            vSetCd = ""
                            'SizPvuRecNo = Common_Procedures.get_FieldValue(con, "Sizing_Pavu_Receipt_Head", "Sizing_Pavu_Receipt_No", "(Sizing_Pavu_Receipt_Code = '" & Trim(.Rows(i).Cells(dgvCol_Details.pavu_receipt_code).Value) & "')", , tr)
                            Da = New SqlClient.SqlDataAdapter("select a.Sizing_Pavu_Receipt_No, b.set_code from Sizing_Pavu_Receipt_Head a, Stock_SizedPavu_Processing_Details b Where a.Sizing_Pavu_Receipt_Code = '" & Trim(.Rows(i).Cells(dgvCol_Details.pavu_receipt_code).Value) & "' and 'SZPRC-' + a.Sizing_Pavu_Receipt_Code = b.Reference_Code", con)
                            Da.SelectCommand.Transaction = tr
                            Dt1 = New DataTable
                            Da.Fill(Dt1)
                            If Dt1.Rows.Count > 0 Then
                                SizPvuRecNo = Dt1.Rows(0).Item("Sizing_Pavu_Receipt_No").ToString
                                vSetCd = Dt1.Rows(0).Item("set_code").ToString
                            End If
                            Dt1.Clear()

                            cmd.CommandText = "Update Sizing_Pavu_Receipt_Head set Sizing_Specification_Code = '" & Trim(NewCode) & "'  Where Sizing_Pavu_Receipt_Code = '" & Trim(.Rows(i).Cells(dgvCol_Details.pavu_receipt_code).Value) & "'"
                            cmd.ExecuteNonQuery()

                            cmd.CommandText = "Delete from Stock_Yarn_Processing_Details Where Reference_Code = '" & Trim(Pk_Condition3) & Trim(.Rows(i).Cells(dgvCol_Details.pavu_receipt_code).Value) & "'"
                            cmd.ExecuteNonQuery()

                        ElseIf Trim(UCase(cbo_Type.Text)) = "SIZING-UNIT PAVU DELIVERY" Then

                            SizPvuRecCode = Trim(.Rows(i).Cells(dgvCol_Details.pavu_receipt_code).Value)
                            SizPvuRecNo = Common_Procedures.get_FieldValue(con, "SizSoft_Pavu_Delivery_Head", "Pavu_Delivery_No", "(Pavu_Delivery_Code = '" & Trim(.Rows(i).Cells(dgvCol_Details.pavu_receipt_code).Value) & "')", , tr)

                            cmd.CommandText = "Update SizSoft_Pavu_Delivery_Head set Sizing_Specification_Code = '" & Trim(NewCode) & "'  Where Pavu_Delivery_Code = '" & Trim(.Rows(i).Cells(dgvCol_Details.pavu_receipt_code).Value) & "'"
                            cmd.ExecuteNonQuery()

                        End If


                        cmd.CommandText = "Insert into Sizing_SpecificationPavu_Details (    Sizing_Specification_Code, Company_IdNo                     , Sizing_Specification_No         , for_OrderBy                                  ,                  Sizing_Specification_Date,     Ledger_IdNo     ,             Set_Code      ,           Set_No          ,             Sl_No              ,                    Beam_No             ,          Noof_Pcs                       ,          Meters_Pc              ,                      Meters                   ,   Beam_Width_IdNo    ,     Sizing_Pavu_Receipt_No,      Sizing_Pavu_Receipt_Code                                                            ,       Gross_weight                                                ,           Net_weight                                              ,                             Warp_weight                        ,  EndsCount_IdNo              ,                LoomType_Idno ) " &
                                                         "    Values              ( '" & Trim(NewCode) & "'       , " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_RefNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text))) & ",   @EntryDate  , " & Str(Val(Led_ID)) & ", '" & Trim(vSetCd) & "', '" & Trim(txt_SetNo.Text) & "',          " & Str(Val(Sno)) & ", '" & Trim(.Rows(i).Cells(dgvCol_Details.Beam_No).Value) & "',     " & Str(Val(.Rows(i).Cells(dgvCol_Details.Pcs).Value)) & ", " & Str(Val(Mtr_Pc)) & "    , " & Str(Val(.Rows(i).Cells(dgvCol_Details.meters).Value)) & "     , " & Str(Val(Bw_ID)) & ",'" & Trim(SizPvuRecNo) & "', '" & Trim(SizPvuRecCode) & "' ," & Str(Val(.Rows(i).Cells(dgvCol_Details.Gross_Weight).Value)) & ", " & Str(Val(.Rows(i).Cells(dgvCol_Details.Net_Weight).Value)) & ", " & Str(Val(.Rows(i).Cells(dgvCol_Details.Warp_Weight).Value)) & " ,  " & Str(Val(vDET_EdsCnt_ID)) & " ,  " & Str(Val(vLoomType_Idno)) & ") "
                        cmd.ExecuteNonQuery()


                        If Trim(UCase(cbo_Type.Text)) <> "RECEIPT" And Trim(UCase(cbo_Type.Text)) <> "SIZING-UNIT PAVU DELIVERY" Then

                            Nr = 0
                            cmd.CommandText = "Update Stock_SizedPavu_Processing_Details set Reference_Date = @EntryDate, Sl_No = " & Str(Val(Sno)) & " " &
                                                " Where Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' and Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Set_Code = '" & Trim(vSetCd) & "' and Beam_No = '" & Trim(.Rows(i).Cells(1).Value) & "' And EndsCount_IdNo = " & Str(Val(vDET_EdsCnt_ID)) & "   "
                            Nr = cmd.ExecuteNonQuery()

                            If Nr = 0 Then
                                cmd.CommandText = "Insert into Stock_SizedPavu_Processing_Details (                     Reference_Code          ,              Company_IdNo        ,               Reference_No    ,                               for_OrderBy                              , Reference_Date,         Ledger_IdNo     ,           StockAt_IdNo      ,         Set_Code      ,           Set_No              ,    setcode_forSelection     ,      Ends_Name         ,     count_idno           ,         EndsCount_IdNo     , Mill_IdNo,  Beam_Width_IdNo, Sizing_SlNo,         Sl_No        ,                    Beam_No             ,                               ForOrderBy_BeamNo                                 , Gross_Weight, Tare_Weight, Net_Weight,                      Noof_Pcs            ,          Meters_Pc      ,                      Meters                                                                                                 ,                LoomType_Idno         ,  ClothSales_OrderCode_forSelection ) " &
                                                        "    Values                           ( '" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_RefNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text))) & ",   @EntryDate  , " & Str(Val(Led_ID)) & ", " & Str(Val(StkAt_IdNo)) & ", '" & Trim(vSetCd) & "', '" & Trim(txt_SetNo.Text) & "', '" & Trim(Selc_SetCode) & "', '" & Trim(pEds_Nm) & "', " & Str(Val(pCnt_ID)) & ", " & Str(Val(vDET_EdsCnt_ID)) & ",     0    ,  " & Str(Val(Bw_ID)) & " ,    0     , " & Str(Val(Sno)) & ", '" & Trim(.Rows(i).Cells(dgvCol_Details.Beam_No).Value) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(.Rows(i).Cells(dgvCol_Details.Beam_No).Value))) & ",      0      ,       0    ,      0    , " & Str(Val(.Rows(i).Cells(dgvCol_Details.Pcs).Value)) & ", " & Str(Val(Mtr_Pc)) & ", " & Str(Val(.Rows(i).Cells(dgvCol_Details.meters).Value)) & " , " & Str(Val(vLoomType_Idno)) & " , '" & Trim(cbo_ClothSales_OrderCode_forSelection.Text) & "') "
                                cmd.ExecuteNonQuery()
                            End If


                        End If

                        cmd.CommandText = "Insert into " & Trim(Common_Procedures.EntryTempTable) & "(Int1, Int2, Meters1) Values (" & Str(Val(vDET_EdsCnt_ID)) & ", 1, " & Str(Val(.Rows(i).Cells(dgvCol_Details.meters).Value)) & ")"
                        cmd.ExecuteNonQuery()

                    End If

                Next
                Call Common_Procedures.User_Modification_Updation(con, "DETAILS", Me.Name, "NEW", "Sizing_SpecificationPavu_Details", "Sizing_Specification_Code", Val(lbl_Company.Tag), NewCode, lbl_RefNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, False, " Beam_No ,Noof_Pcs,Meters_Pc,Meters,   Beam_Width_IdNo    ,     Sizing_Pavu_Receipt_No,      Sizing_Pavu_Receipt_Code", "Sl_No", "Sizing_Specification_Code, For_OrderBy, Company_IdNo, Sizing_Specification_No, Sizing_Specification_Date, Ledger_Idno", tr)

            End With


            Dim Stk_DelvMtr As String = 0, Stk_RecMtr As String = 0
            Dim Delv_Ledtype As String = ""
            Dim Rec_Ledtype As String = ""
            Dim vPVUSTK_DelVID As Integer
            Dim vPvuBMS As String = 0
            Dim vPvuMtrs As String = 0
            Dim vSTKPVUQTY As String = 0
            Dim Da4 As New SqlClient.SqlDataAdapter
            Dim Dt4 As New DataTable

            If Val(vTotPvuMtrs) <> 0 Then


                Del_ID = Val(Led_ID)
                Rec_ID = 0
                If Val(Common_Procedures.settings.SizingSpecification_AutoTransfer_PavuStock_To_Godown) = 1 Then
                    Del_ID = Val(Common_Procedures.CommonLedger.Godown_Ac)
                    Rec_ID = 0

                Else
                    If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1018" Then '---- M.K Textiles (Palladam)
                        If (Trim(UCase(NewCode)) = "1-27/15-16" And Trim(UCase(txt_SetNo.Text)) = "1607" And Led_ID = 104) Or (Trim(UCase(NewCode)) = "1-5/15-16" And Trim(UCase(txt_SetNo.Text)) = "716" And Led_ID = 117) Then '---- M.K Textiles (Palladam)
                            Del_ID = Val(Common_Procedures.CommonLedger.Godown_Ac)
                            Rec_ID = 0
                        End If
                    End If

                End If

                Da4 = New SqlClient.SqlDataAdapter("Select Int1 as endscount_idno, sum(int2) as pavu_beams , sum(Meters1) as pavu_meters  from  " & Trim(Common_Procedures.EntryTempTable) & " group by Int1 Having sum(Meters1) <> 0 ", con)
                Da4.SelectCommand.Transaction = tr
                Dt4 = New DataTable
                Da4.Fill(Dt4)

                If Dt4.Rows.Count > 0 Then
                    For K = 0 To Dt4.Rows.Count - 1

                        EdsCnt_ID = Val(Dt4.Rows(K).Item("endscount_idno").ToString)
                        vPvuBMS = Val(Dt4.Rows(K).Item("pavu_beams").ToString)
                        vPvuMtrs = Val(Dt4.Rows(K).Item("pavu_meters").ToString)


                        Da = New SqlClient.SqlDataAdapter("Select Stock_In , Meters_Pcs from EndsCount_Head Where EndsCount_IdNo = " & Str(Val(EdsCnt_ID)), con)
                        Da.SelectCommand.Transaction = tr
                        dt2 = New DataTable
                        Da.Fill(dt2)


                        Stock_In = ""
                        mtrspcs = 0
                        If dt2.Rows.Count > 0 Then
                            Stock_In = dt2.Rows(0)("Stock_In").ToString
                            mtrspcs = Val(dt2.Rows(0)("Meters_Pcs").ToString)
                        End If
                        dt2.Clear()

                        If Trim(UCase(Stock_In)) = "PCS" Then
                            If Val(mtrspcs) = 0 Then mtrspcs = 1
                            vTotPvuStk = vPvuMtrs / mtrspcs

                        Else
                            vTotPvuStk = vPvuMtrs

                        End If


                        'If Trim(UCase(Stock_In)) = "PCS" Then
                        '    If Val(mtrspcs) = 0 Then mtrspcs = 1
                        '    vTotPvuStk = vTotPvuMtrs / mtrspcs

                        'Else
                        '    vTotPvuStk = vTotPvuMtrs

                        'End If

                        'Del_ID = Val(Led_ID)
                        'Rec_ID = 0
                        'If Val(Common_Procedures.settings.SizingSpecification_AutoTransfer_PavuStock_To_Godown) = 1 Then
                        '    Del_ID = Val(Common_Procedures.CommonLedger.Godown_Ac)
                        '    Rec_ID = 0

                        'Else
                        '    If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1018" Then '---- M.K Textiles (Palladam)
                        '        If (Trim(UCase(NewCode)) = "1-27/15-16" And Trim(UCase(txt_SetNo.Text)) = "1607" And Led_ID = 104) Or (Trim(UCase(NewCode)) = "1-5/15-16" And Trim(UCase(txt_SetNo.Text)) = "716" And Led_ID = 117) Then '---- M.K Textiles (Palladam)
                        '            Del_ID = Val(Common_Procedures.CommonLedger.Godown_Ac)
                        '            Rec_ID = 0
                        '        End If
                        '    End If

                        'End If

                        cmd.CommandText = "Insert into Stock_Pavu_Processing_Details(           Reference_Code                  ,                    Company_IdNo   ,               Reference_No    ,                                   for_OrderBy                             , Reference_Date,       DeliveryTo_Idno     ,           ReceivedFrom_Idno   , Cloth_Idno    ,       Entry_ID        ,    Party_Bill_No  ,           Particulars     ,   Sl_No ,             EndsCount_IdNo    ,     Sized_Beam              ,        Meters                      ,                     ClothSales_OrderCode_forSelection       ) " &
                                                                            "Values ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_RefNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text))) & ",          @EntryDate, " & Str(Val(Del_ID)) & ", " & Str(Val(Rec_ID)) & "      ,        0      , '" & Trim(EntID) & "', '" & Trim(PBlNo) & "', '" & Trim(Partcls) & "', " & Str(Val(K)) & "   , " & Str(Val(EdsCnt_ID)) & ", " & Str(Val(vPvuBMS)) & ", " & Str(Val(vTotPvuStk)) & " ,'" & Trim(cbo_ClothSales_OrderCode_forSelection.Text) & "' )"
                        cmd.ExecuteNonQuery()

                        'cmd.CommandText = "Insert into Stock_Pavu_Processing_Details(           Reference_Code                  ,                    Company_IdNo   ,               Reference_No    ,                                   for_OrderBy                             , Reference_Date,       DeliveryTo_Idno     ,           ReceivedFrom_Idno   , Cloth_Idno    ,       Entry_ID        ,    Party_Bill_No  ,           Particulars     ,   Sl_No ,             EndsCount_IdNo    ,     Sized_Beam              ,        Meters               ) " &
                        '                                                    "Values ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_RefNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text))) & ",          @EntryDate, " & Str(Val(Del_ID)) & ", " & Str(Val(Rec_ID)) & "      ,        0      , '" & Trim(EntID) & "', '" & Trim(PBlNo) & "', '" & Trim(Partcls) & "',        1   , " & Str(Val(EdsCnt_ID)) & ", " & Str(Val(vTotPvuBms)) & ", " & Str(Val(vTotPvuStk)) & " )"
                        'cmd.ExecuteNonQuery()

                    Next K

                End If
            End If


            With dgv_YarnDetails

                YrnPartcls = Partcls & Trim(.Rows(0).Cells(3).Value) & ", EndsCount : " & Trim(dgv_PavuDetails.Rows(0).Cells(dgvCol_Details.Ends_Count).Value) & ", Beams : " & Trim(Val(vTotPvuBms)) & ", Meters : " & Trim(Val(vTotPvuMtrs))
                'YrnPartcls = Partcls & Trim(.Rows(0).Cells(3).Value) & ", EndsCount : " & Trim(cbo_EndsCount.Text) & ", Beams : " & Trim(Val(vTotPvuBms)) & ", Meters : " & Trim(Val(vTotPvuMtrs))

                Sno = 0
                ByCnCnt_ID = 0
                ByCnMil_ID = 0

                For i = 0 To .RowCount - 1

                    If Val(.Rows(i).Cells(6).Value) <> 0 Then

                        Sno = Sno + 1

                        YCnt_ID = Common_Procedures.Count_NameToIdNo(con, .Rows(i).Cells(1).Value, tr)

                        YMil_ID = Common_Procedures.Mill_NameToIdNo(con, .Rows(i).Cells(3).Value, tr)

                        If ByCnCnt_ID = 0 Then
                            ByCnCnt_ID = YCnt_ID
                            ByCnMil_ID = YMil_ID
                        End If

                        vLOT_ENT_REFCODE = ""
                        If Trim(.Rows(i).Cells(7).Value) <> "" Then
                            vLOT_ENT_REFCODE = Common_Procedures.YarnLotCodeSelection_To_LotEntryReferenceCode(con, .Rows(i).Cells(7).Value, tr)
                        End If

                        cmd.CommandText = "Insert into Sizing_SpecificationYarn_Details(Sizing_Specification_Code, Company_IdNo, Sizing_Specification_No, for_OrderBy, Sizing_Specification_Date, Sl_No, count_idno, Yarn_Type, Mill_IdNo, Bags, Cones, Weight  , LotCode_forSelection, Lot_Entry_ReferenceCode ) Values ('" & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_RefNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text))) & ", @EntryDate,  " & Str(Val(Sno)) & ", " & Str(Val(YCnt_ID)) & ", '" & Trim(.Rows(i).Cells(2).Value) & "', " & Str(Val(YMil_ID)) & ", " & Str(Val(.Rows(i).Cells(4).Value)) & ", " & Str(Val(.Rows(i).Cells(5).Value)) & ", " & Str(Val(.Rows(i).Cells(6).Value)) & " ,'" & Trim(.Rows(i).Cells(7).Value) & "', '" & Trim(vLOT_ENT_REFCODE) & "')"
                        cmd.ExecuteNonQuery()

                        cmd.CommandText = "Insert into Stock_Yarn_Processing_Details ( Reference_Code, Company_IdNo, Reference_No, for_OrderBy, Reference_Date, DeliveryTo_Idno, ReceivedFrom_Idno, Entry_ID, Particulars, Party_Bill_No, Sl_No, Count_IdNo, Yarn_Type, Mill_IdNo, Bags, Cones, Weight , Sizing_JobCode_forSelection ,LotCode_forSelection , Lot_Entry_ReferenceCode, ClothSales_OrderCode_forSelection) Values ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_RefNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text))) & ", @EntryDate, 0, " & Str(Val(Led_ID)) & ", '" & Trim(EntID) & "', '" & Trim(YrnPartcls) & "', '" & Trim(PBlNo) & "', " & Str(Val(Sno)) & ", " & Str(Val(YCnt_ID)) & ", '" & Trim(.Rows(i).Cells(2).Value) & "', " & Str(Val(YMil_ID)) & ", " & Str(Val(.Rows(i).Cells(4).Value)) & ", " & Str(Val(.Rows(i).Cells(5).Value)) & ", " & Str(Val(.Rows(i).Cells(6).Value)) & " ,'" & Trim(cbo_jobcardno.Text) & "' ,  '" & Trim(.Rows(i).Cells(7).Value) & "', '" & Trim(vLOT_ENT_REFCODE) & "','" & Trim(cbo_ClothSales_OrderCode_forSelection.Text) & "')"
                        cmd.ExecuteNonQuery()

                    End If

                Next
                Call Common_Procedures.User_Modification_Updation(con, "DETAILS", Me.Name, "NEW", "Sizing_SpecificationYarn_Details", "Sizing_Specification_Code", Val(lbl_Company.Tag), NewCode, lbl_RefNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, False, " count_idno, Yarn_Type, Mill_IdNo, Bags, Cones, Weight", "Sl_No", "Sizing_Specification_Code, For_OrderBy, Company_IdNo, Sizing_Specification_No, Sizing_Specification_Date, Ledger_Idno", tr)

                If Val(vTotYrnWeight) = 0 Then
                    cmd.CommandText = "Insert into Stock_Yarn_Processing_Details ( Reference_Code, Company_IdNo, Reference_No, for_OrderBy, Reference_Date, DeliveryTo_Idno, ReceivedFrom_Idno, Entry_ID, Particulars, Party_Bill_No, Sl_No, Count_IdNo, Yarn_Type, Mill_IdNo, Bags, Cones, Weight , LotCode_forSelection , Lot_Entry_ReferenceCode ,ClothSales_OrderCode_forSelection ) Values ( '" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_RefNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text))) & ", @EntryDate, 0, " & Str(Val(Led_ID)) & ", '" & Trim(EntID) & "', '" & Trim(YrnPartcls) & "', '" & Trim(PBlNo) & "', -51, " & Str(Val(pCnt_ID)) & ", 'MILL', 0, 0, 0, 0 ,  '" & Trim(.Rows(0).Cells(7).Value) & "', '" & Trim(vLOT_ENT_REFCODE) & "','" & Trim(cbo_ClothSales_OrderCode_forSelection.Text) & "')"
                    cmd.ExecuteNonQuery()
                End If

            End With

            If Val(txt_ExcessShort.Text) <> 0 Or Val(txt_BabyWt.Text) <> 0 Then

                If Trim(UCase(cbo_YarnStock.Text)) = "YARN TAKEN" Then

                    If Chk_RWSts.Checked = True Then
                        YrnTyp = "R/W"
                        YrnBgs = Val(txt_RwBags.Text)
                        YrnCns = Val(txt_RwCns.Text)
                        YrnWgt = Val(txt_BabyWt.Text) + Val(txt_RWES.Text)

                    Else
                        YrnTyp = "BABY"
                        YrnBgs = Val(txt_BabyBag.Text)
                        YrnCns = Val(vTotYrnCones)
                        YrnWgt = Val(txt_BabyWt.Text)

                    End If

                Else
                    YrnTyp = "MILL"
                    YrnBgs = 0
                    YrnCns = 0
                    YrnWgt = Val(txt_ExcessShort.Text)

                End If

                Del_ID = 0 : Rec_ID = 0

                If Val(YrnWgt) < 0 Then
                    Rec_ID = Led_ID
                Else
                    Del_ID = Led_ID
                End If

                cmd.CommandText = "Insert into Stock_Yarn_Processing_Details ( Reference_Code, Company_IdNo, Reference_No, for_OrderBy, Reference_Date, DeliveryTo_Idno, ReceivedFrom_Idno, Entry_ID, Particulars, Party_Bill_No, Sl_No, Count_IdNo, Yarn_Type, Mill_IdNo, Bags, Cones, Weight , Sizing_JobCode_forSelection ,ClothSales_OrderCode_forSelection) Values ( '" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_RefNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text))) & ", @EntryDate, " & Str(Val(Del_ID)) & ", " & Str(Val(Rec_ID)) & ", '" & Trim(EntID) & "', '" & Trim(Partcls) & "', '" & Trim(PBlNo) & "', -200, " & Str(Val(ByCnCnt_ID)) & ", '" & Trim(YrnTyp) & "', " & Str(Val(ByCnMil_ID)) & ", " & Str(Val(YrnBgs)) & ", " & Str(Val(YrnCns)) & ", " & Str(Math.Abs(Val(YrnWgt))) & " ,'" & Trim(cbo_jobcardno.Text) & "' ,'" & Trim(cbo_ClothSales_OrderCode_forSelection.Text) & "' )"
                cmd.ExecuteNonQuery()

            End If

            Slno = Slno + 1
            If Val(vTotYrnBags) <> 0 Or Val(vTotYrnCones) <> 0 Then
                cmd.CommandText = "Insert into Stock_Empty_BeamBagCone_Processing_Details(Reference_Code, Company_IdNo, Reference_No, for_OrderBy, Reference_Date, DeliveryTo_Idno, ReceivedFrom_Idno, Entry_ID, Party_Bill_No, Sl_No, Empty_Bags, Empty_Cones, Particulars , Beam_Width_IdNo) Values ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_RefNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text))) & ", @EntryDate, 0, " & Str(Val(Led_ID)) & ", '" & Trim(EntID) & "', '" & Trim(PBlNo) & "', " & Str(Val(Slno)) & ", " & Str(Val(vTotYrnBags)) & ", " & Str(Val(vTotYrnCones)) & ", '" & Trim(Partcls) & "', " & Str(Val(Bw_ID)) & ")"
                cmd.ExecuteNonQuery()
            End If

            If Trim(UCase(cbo_Type.Text)) <> "RECEIPT" Then

                da1 = New SqlClient.SqlDataAdapter("select a.Beam_Width_IdNo , Count(Beam_No) as Beams , a.LoomType_Idno from Sizing_SpecificationPavu_Details a where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Sizing_Specification_Code = '" & Trim(NewCode) & "' group by a.Beam_Width_IdNo , a.LoomType_Idno ", con)
                'da1 = New SqlClient.SqlDataAdapter("select a.Beam_Width_IdNo , Count(Beam_No) as Beams from Sizing_SpecificationPavu_Details a where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Sizing_Specification_Code = '" & Trim(NewCode) & "' group by a.Beam_Width_IdNo", con)
                da1.SelectCommand.Transaction = tr
                Dt1 = New DataTable
                da1.Fill(Dt1)

                If Dt1.Rows.Count > 0 Then
                    For I = 0 To Dt1.Rows.Count - 1
                        Slno = Slno + 1
                        cmd.CommandText = "Insert into Stock_Empty_BeamBagCone_Processing_Details(Reference_Code, Company_IdNo, Reference_No, for_OrderBy, Reference_Date, DeliveryTo_Idno, ReceivedFrom_Idno, Entry_ID, Party_Bill_No, Sl_No, Beam_Width_IdNo, Pavu_Beam, Particulars  , LoomType_Idno ) Values ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_RefNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text))) & ", @EntryDate, " & Str(Val(Led_ID)) & ", 0, '" & Trim(EntID) & "', '" & Trim(PBlNo) & "', " & Str(Val(Slno)) & " ," & Str(Val(Dt1.Rows(I).Item("Beam_Width_IdNo").ToString)) & ", " & Str(Val(Dt1.Rows(I).Item("Beams").ToString)) & " , '" & Trim(Partcls) & "' , " & Str(Val(Dt1.Rows(I).Item("LoomType_Idno").ToString)) & ")"
                        'cmd.CommandText = "Insert into Stock_Empty_BeamBagCone_Processing_Details(Reference_Code, Company_IdNo, Reference_No, for_OrderBy, Reference_Date, DeliveryTo_Idno, ReceivedFrom_Idno, Entry_ID, Party_Bill_No, Sl_No, Beam_Width_IdNo, Pavu_Beam, Particulars) Values ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_RefNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text))) & ", @EntryDate, " & Str(Val(Led_ID)) & ", 0, '" & Trim(EntID) & "', '" & Trim(PBlNo) & "', " & Str(Val(Slno)) & " ," & Str(Val(Dt1.Rows(I).Item("Beam_Width_IdNo").ToString)) & ", " & Str(Val(Dt1.Rows(I).Item("Beams").ToString)) & " , '" & Trim(Partcls) & "')"
                        cmd.ExecuteNonQuery()
                        Slno = Slno + 1
                        cmd.CommandText = "Insert into Stock_Empty_BeamBagCone_Processing_Details(Reference_Code, Company_IdNo, Reference_No, for_OrderBy, Reference_Date, DeliveryTo_Idno, ReceivedFrom_Idno, Entry_ID, Party_Bill_No, Sl_No, Beam_Width_IdNo, Empty_Beam, Particulars  , LoomType_Idno ) Values ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_RefNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text))) & ", @EntryDate, 0, " & Str(Val(Led_ID)) & ", '" & Trim(EntID) & "', '" & Trim(PBlNo) & "', " & Str(Val(Slno)) & " , " & Str(Val(Dt1.Rows(I).Item("Beam_Width_IdNo").ToString)) & ", " & Str(Val(Dt1.Rows(I).Item("Beams").ToString)) & " , '" & Trim(Partcls) & "' , " & Str(Val(Dt1.Rows(I).Item("LoomType_Idno").ToString)) & ")"
                        'cmd.CommandText = "Insert into Stock_Empty_BeamBagCone_Processing_Details(Reference_Code, Company_IdNo, Reference_No, for_OrderBy, Reference_Date, DeliveryTo_Idno, ReceivedFrom_Idno, Entry_ID, Party_Bill_No, Sl_No, Beam_Width_IdNo, Empty_Beam, Particulars) Values ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_RefNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text))) & ", @EntryDate, 0, " & Str(Val(Led_ID)) & ", '" & Trim(EntID) & "', '" & Trim(PBlNo) & "', " & Str(Val(Slno)) & " , " & Str(Val(Dt1.Rows(I).Item("Beam_Width_IdNo").ToString)) & ", " & Str(Val(Dt1.Rows(I).Item("Beams").ToString)) & " , '" & Trim(Partcls) & "')"
                        cmd.ExecuteNonQuery()
                    Next
                End If

            End If

            If Val(Chk_RWSts.Checked) = 0 Then

                Nr = 0
                cmd.CommandText = "Update Stock_BabyCone_Processing_Details set " &
                         " DeliveryTo_Idno = " & Str(Val(Led_ID)) & ", " &
                         " Mill_Idno = " & Str(Val(ByCnMil_ID)) & ", " &
                        " Count_Idno = " & Str(Val(ByCnCnt_ID)) & ", " &
                        " Baby_Bags = " & Str(Val(txt_BabyBag.Text)) & ", " &
                        " Baby_Cones = " & Str(Val(vTotYrnCones)) & ", " &
                        " Baby_Weight = " & Str(Val(txt_BabyWt.Text)) & " " &
                        " Where Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' and " &
                        " Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Set_Code = '" & Trim(vSetCd) & "'"
                Nr = cmd.ExecuteNonQuery()

                If Nr = 0 And Val(txt_BabyWt.Text) <> 0 Then

                    cmd.CommandText = "Insert into Stock_BabyCone_Processing_Details(                   Reference_Code            ,                 Company_IdNo     ,              Reference_No     ,                                 For_OrderBy                             , Reference_Date,   DeliveryTo_Idno  , ReceivedFrom_Idno,           Set_Code    ,               Set_No          ,       setcode_forSelection  , Ends_Name, Yarn_Type,         Mill_Idno      ,          Count_IdNo     , Bag_No,                 Baby_Bags          ,                 Baby_Cones     ,                 Baby_Weight       , Delivered_Bags, Delivered_Cones, Delivered_Weight) " &
                                  " Values                                      ( '" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Val(lbl_RefNo.Text) & "' , " & Str(Common_Procedures.OrderBy_CodeToValue(Trim(lbl_RefNo.Text))) & ",   @EntryDate  , " & Str(Led_ID) & ",          0       ,'" & Trim(vSetCd) & "' , '" & Trim(txt_SetNo.Text) & "', '" & Trim(Selc_SetCode) & "', ''       ,   'BABY' , " & Str(ByCnMil_ID) & ", " & Str(ByCnCnt_ID) & ",   1    , " & Str(Val(txt_BabyBag.Text)) & " , " & Str(Val(vTotYrnCones)) & " , " & Str(Val(txt_BabyWt.Text)) & " ,        0     ,          0    ,          0       )"

                    cmd.ExecuteNonQuery()

                End If

            End If



            If Val(Common_Procedures.settings.Negative_Stock_Restriction_for_Yarn_Stock) = 1 Then

                cmd.CommandText = "Insert into TempTable_For_NegativeStock ( Stock_Type, Reference_Code, Reference_Date, Company_Idno, Ledger_Idno      , Count_IdNo, Yarn_Type, Mill_IdNo ) " &
                                      " Select                               'YARN'    , Reference_Code, Reference_Date, Company_IdNo, ReceivedFrom_Idno, Count_IdNo, Yarn_Type, Mill_IdNo from Stock_Yarn_Processing_Details where Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()

                If Common_Procedures.Check_is_Negative_Stock_Status(con, tr) = True Then Exit Sub

            End If



            Common_Procedures.Voucher_Deletion(con, Val(lbl_Company.Tag), Trim(Pk_Condition) & Trim(NewCode), tr)
            Common_Procedures.Voucher_Deletion(con, Val(lbl_Company.Tag), Trim(Pk_Condition2) & Trim(NewCode), tr)



            Dim vLed_IdNos As String = "", vVou_Amts As String = "", ErrMsg As String = ""
            vLed_IdNos = Led_ID & "|" & Val(Common_Procedures.CommonLedger.Sizing_Charges_Ac) & "|24|25|26|30"

            vVou_Amts = Val(CSng(lbl_GrossAmt.Text)) & "|" & -1 * Val(lbl_Taxable_Value.Text) & "|" & -1 * Val(lbl_CGST_Amount.Text) & "|" & -1 * Val(lbl_SGST_Amount.Text) & "|" & -1 * 0 & "|" & -1 * Val(lbl_RoundOff.Text)

            'vVou_Amts = Val(CSng(lbl_GrossAmt.Text)) & "|" & -1 * (Val(CSng(lbl_GrossAmt.Text)) - Val(lbl_CGST_Amount.Text) - Val(lbl_SGST_Amount.Text)) & "|" & -1 * Val(lbl_CGST_Amount.Text) & "|" & -1 * Val(lbl_SGST_Amount.Text) & "|" & -1 * 0

            If IsDate(msk_InvoiceDate.Text) = True Then
                vINVDATE = Trim(msk_InvoiceDate.Text)
            Else
                vINVDATE = Trim(msk_Date.Text)
            End If

            If Common_Procedures.Voucher_Updation(con, "GST-Siz.Spec", Val(lbl_Company.Tag), Trim(Pk_Condition) & Trim(NewCode), Trim(lbl_RefNo.Text), Convert.ToDateTime(vINVDATE), "Bill No : " & Trim(txt_InvNo.Text), vLed_IdNos, vVou_Amts, ErrMsg, tr, Common_Procedures.SoftwareTypes.Textile_Software) = False Then
                Throw New ApplicationException(ErrMsg)
            End If

            Common_Procedures.Voucher_Deletion(con, Val(lbl_Company.Tag), Trim(Pk_Condition2) & Trim(NewCode), tr)
            If Val(lbl_TdsAmount.Text) <> 0 Then
                vLed_IdNos = Led_ID & "|" & Val(Common_Procedures.CommonLedger.TDS_Payable_Ac)
                vVou_Amts = -1 * Val(lbl_TdsAmount.Text) & "|" & Val(lbl_TdsAmount.Text)
                If Common_Procedures.Voucher_Updation(con, "GST-Siz.Tds", Val(lbl_Company.Tag), Trim(Pk_Condition2) & Trim(NewCode), Trim(lbl_RefNo.Text), Convert.ToDateTime(vINVDATE), "Bill No. : " & Trim(txt_InvNo.Text) & ",  Set No. : " & Trim(txt_SetNo.Text), vLed_IdNos, vVou_Amts, ErrMsg, tr, Common_Procedures.SoftwareTypes.Textile_Software) = False Then
                    Throw New ApplicationException(ErrMsg)
                    Exit Sub
                End If
            End If

            '----Bill Posting

            vVou_BlAmt = Val(CSng(lbl_NetAmount.Text))

            VouBil = Common_Procedures.VoucherBill_Posting(con, Val(lbl_Company.Tag), Convert.ToDateTime(vINVDATE), Led_ID, Trim(lbl_RefNo.Text), 0, Val(vVou_BlAmt), "CR", Trim(Pk_Condition) & Trim(NewCode), tr, Common_Procedures.SoftwareTypes.Textile_Software, SaveAll_STS)
            If Trim(UCase(VouBil)) = "ERROR" Then
                Throw New ApplicationException("Error on Voucher Bill Posting")
            End If


            tr.Commit()

            Dt1.Dispose()
            Da.Dispose()

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
            SaveAll_STS = False
            Timer1.Enabled = False

            If InStr(1, Trim(LCase(ex.Message)), Trim(LCase("IX_Stock_SizedPavu_Processing_Details_2"))) > 0 Then
                MessageBox.Show("Duplicate SetNo/BeamNo", "FOR SAVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            ElseIf InStr(1, Trim(LCase(ex.Message)), Trim(LCase("IX_Stock_SizedPavu_Processing_Details_1"))) > 0 Then
                MessageBox.Show("Duplicate SetNo/BeamNo", "FOR SAVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            ElseIf InStr(1, Trim(LCase(ex.Message)), Trim(LCase("PK_SizedPavu_Processing_Details"))) > 0 Then
                MessageBox.Show("Duplicate SetNo/BeamNo", "FOR SAVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Else
                MessageBox.Show(ex.Message, "FOR SAVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            End If

        Finally
            If msk_Date.Enabled And msk_Date.Visible Then msk_Date.Focus() : msk_Date.SelectionStart = 0

        End Try

    End Sub

    Private Sub cbo_Type_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Type.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Type, dtp_Date, cbo_Sizing, "", "", "", "")
    End Sub

    Private Sub cbo_Type_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Type.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Type, cbo_Sizing, "", "", " ", "", True)
    End Sub

    Private Sub cbo_Sizing_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Sizing.GotFocus
        cbo_Sizing.Tag = cbo_Sizing.Text
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "(ledger_idno = 0 or Ledger_Type = 'SIZING' or Show_In_All_Entry = 1)", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_Sizing_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Sizing.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Sizing, cbo_Type, Nothing, "Ledger_AlaisHead", "Ledger_DisplayName", "(ledger_idno = 0 or Ledger_Type = 'SIZING'  or Show_In_All_Entry = 1)", "(Ledger_IdNo = 0)")

        If (e.KeyValue = 40 And cbo_Sizing.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
            If cbo_ClothSales_OrderCode_forSelection.Visible = True Then
                cbo_ClothSales_OrderCode_forSelection.Focus()
            Else
                txt_SetNo.Focus()
            End If

        End If




    End Sub

    Private Sub cbo_Sizing_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Sizing.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Sizing, Nothing, "Ledger_AlaisHead", "Ledger_DisplayName", "(ledger_idno = 0 or Ledger_Type = 'SIZING'  or Show_In_All_Entry = 1)", "(Ledger_IdNo = 0)")
        If Asc(e.KeyChar) = 13 Then

            If Trim(UCase(cbo_Sizing.Tag)) <> Trim(UCase(cbo_Sizing.Text)) Then
                get_Sizing_TdsPerc()
            End If

            If Trim(UCase(cbo_Type.Text)) = "RECEIPT" Or Trim(UCase(cbo_Type.Text)) = "SIZING-UNIT PAVU DELIVERY" Then

                If MessageBox.Show("Do you want to select Pavu Receipt:", "FOR PAVU RECEIPT...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = DialogResult.Yes Then
                    btn_Selection_Click(sender, e)

                Else
                    If txt_SetNo.Enabled And txt_SetNo.Visible Then
                        txt_SetNo.Focus()

                    ElseIf cbo_ClothSales_OrderCode_forSelection.Enabled And cbo_ClothSales_OrderCode_forSelection.Visible = True Then
                        cbo_ClothSales_OrderCode_forSelection.Focus()

                    ElseIf txt_TotalBeams.Enabled And txt_TotalBeams.Visible Then
                        txt_TotalBeams.Focus()
                    Else
                        cbo_YarnStock.Focus()
                    End If

                End If

                'ElseIf Trim(UCase(cbo_Type.Text)) = "DIRECT" And Common_Procedures.settings.Internal_Order_Entry_Status = 1 Then

                '    If MessageBox.Show("Do you want to select Internal Order:", "FOR INTERNAL ORDER SELECTION...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = DialogResult.Yes Then
                '        btn_Selection_Click(sender, e)

                '    Else

                '        If cbo_ClothSales_OrderCode_forSelection.Enabled And cbo_ClothSales_OrderCode_forSelection.Visible = True Then
                '            cbo_ClothSales_OrderCode_forSelection.Focus()
                '        ElseIf txt_SetNo.Enabled Then
                '            txt_SetNo.Focus()
                '        Else
                '            txt_TotalBeams.Focus()
                '        End If

                '    End If

            Else

                If cbo_ClothSales_OrderCode_forSelection.Enabled And cbo_ClothSales_OrderCode_forSelection.Visible = True Then
                    cbo_ClothSales_OrderCode_forSelection.Focus()
                ElseIf txt_SetNo.Enabled Then
                    txt_SetNo.Focus()
                Else
                    txt_TotalBeams.Focus()
                End If

            End If
        End If
    End Sub

    Private Sub cbo_Sizing_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Sizing.LostFocus
        If Trim(UCase(cbo_Sizing.Tag)) <> Trim(UCase(cbo_Sizing.Text)) Then
            get_Sizing_TdsPerc()
        End If
    End Sub

    Private Sub cbo_Ledger_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Sizing.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Common_Procedures.UR.Ledr_Wea_Siz_Rw_Trans_JbWrk_Creation = Common_Procedures.UR.Ledger_Creation
            Common_Procedures.MDI_LedType = "SIZING"
            Dim f As New Ledger_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Sizing.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()
        End If
    End Sub

    Private Sub cbo_EndsCount_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_EndsCount.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "EndsCount_Head", "EndsCount_Name", "", "(EndsCount_IdNo = 0)")
    End Sub

    Private Sub cbo_EndsCount_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_EndsCount.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_EndsCount, txt_SetNo, txt_PcsLength, "EndsCount_Head", "EndsCount_Name", "", "(EndsCount_IdNo = 0)")
    End Sub

    Private Sub cbo_EndsCountName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_EndsCount.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_EndsCount, txt_PcsLength, "EndsCount_Head", "EndsCount_Name", "", "(EndsCount_IdNo = 0)")
    End Sub



    Private Sub cbo_EndsCount_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_EndsCount.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
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
                Condt = "a.Sizing_Specification_Date between '" & Trim(Format(dtp_Filter_Fromdate.Value, "MM/dd/yyyy")) & "' and '" & Trim(Format(dtp_Filter_ToDate.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_Filter_Fromdate.Value) = True Then
                Condt = "a.Sizing_Specification_Date = '" & Trim(Format(dtp_Filter_Fromdate.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_Filter_ToDate.Value) = True Then
                Condt = "a.Sizing_Specification_Date = '" & Trim(Format(dtp_Filter_ToDate.Value, "MM/dd/yyyy")) & "' "
            End If

            If Trim(cbo_Filter_PartyName.Text) <> "" Then
                Led_IdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Filter_PartyName.Text)
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


            If Val(Led_IdNo) <> 0 Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & "a.Ledger_IdNo = " & Str(Val(Led_IdNo))
            End If

            If Val(Cnt_IdNo) <> 0 Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " a.Sizing_Specification_Code IN (select z1.Sizing_Specification_Code from Sizing_SpecificationYarn_Details z1 where z1.Count_IdNo = " & Str(Val(Cnt_IdNo)) & ")"
            End If

            If Val(Mil_IdNo) <> 0 Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " a.Sizing_Specification_Code IN (select z1.Sizing_Specification_Code from Sizing_SpecificationYarn_Details z1 where z1.Mill_IdNo = " & Str(Val(Mil_IdNo)) & ")"
            End If

            If Val(EdsCnt_IdNo) <> 0 Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " a.EndsCount_IdNo = " & Str(Val(EdsCnt_IdNo))
            End If

            If Trim(txt_Filter_SetNo.Text) <> "" Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " a.Set_No = '" & Trim(txt_Filter_SetNo.Text) & "'"
            End If

            If cbo_Verified_Sts.Visible = True And Trim(cbo_Verified_Sts.Text) <> "" Then

                If Trim(cbo_Verified_Sts.Text) = "YES" Then
                    Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " a.Verified_Status = 1 "
                ElseIf Trim(cbo_Verified_Sts.Text) = "NO" Then
                    Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " a.Verified_Status = 0 "
                End If

            End If


            da = New SqlClient.SqlDataAdapter("select a.*, b.Ledger_Name as SizingName , c.*  from Sizing_Specification_Head a INNER JOIN Ledger_Head b on a.Ledger_IdNo = b.Ledger_IdNo LEFT OUTER JOIN EndsCount_Head c ON c.EndsCount_IdNo = a.EndsCount_IdNo where a.company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Sizing_Specification_Code LIKE '" & Trim(Pk_Condition) & "%' AND a.Sizing_Specification_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "' " & IIf(Trim(Condt) <> "", " and ", "") & Condt & " Order by a.for_orderby, a.Sizing_Specification_No", con)
            da.Fill(dt2)

            dgv_Filter_Details.Rows.Clear()

            If dt2.Rows.Count > 0 Then

                For i = 0 To dt2.Rows.Count - 1

                    n = dgv_Filter_Details.Rows.Add()

                    dgv_Filter_Details.Rows(n).Cells(0).Value = i + 1
                    dgv_Filter_Details.Rows(n).Cells(1).Value = dt2.Rows(i).Item("Sizing_Specification_No").ToString
                    dgv_Filter_Details.Rows(n).Cells(2).Value = dt2.Rows(i).Item("Set_No").ToString
                    dgv_Filter_Details.Rows(n).Cells(3).Value = Format(Convert.ToDateTime(dt2.Rows(i).Item("Sizing_Specification_Date").ToString), "dd-MM-yyyy")
                    dgv_Filter_Details.Rows(n).Cells(4).Value = dt2.Rows(i).Item("SizingName").ToString
                    dgv_Filter_Details.Rows(n).Cells(5).Value = dt2.Rows(i).Item("EndsCount_Name").ToString
                    dgv_Filter_Details.Rows(n).Cells(6).Value = Val(dt2.Rows(i).Item("Total_Beam").ToString)
                    dgv_Filter_Details.Rows(n).Cells(7).Value = Format(Val(dt2.Rows(i).Item("Total_Meters").ToString), "########0.00")
                    dgv_Filter_Details.Rows(n).Cells(8).Value = Val(dt2.Rows(i).Item("Total_Bags").ToString)
                    dgv_Filter_Details.Rows(n).Cells(9).Value = Val(dt2.Rows(i).Item("Total_Cones").ToString)
                    dgv_Filter_Details.Rows(n).Cells(10).Value = Format(Val(dt2.Rows(i).Item("Total_Weight").ToString), "########0.000")

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

    Private Sub cbo_Filter_PartyName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Filter_PartyName.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_IdNo = 0 or Ledger_Type = 'SIZING' or Show_In_All_Entry = 1)", "(Ledger_IdNo = 0)")
    End Sub


    Private Sub cbo_Filter_PartyName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Filter_PartyName.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_PartyName, dtp_Filter_ToDate, cbo_Filter_CountName, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_IdNo = 0 or Ledger_Type = 'SIZING' )", "(Ledger_IdNo = 0)")

    End Sub

    Private Sub cbo_Filter_PartyName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Filter_PartyName.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Filter_PartyName, cbo_Filter_CountName, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_IdNo = 0 or Ledger_Type = 'SIZING' )", "(Ledger_IdNo = 0)")

    End Sub

    Private Sub cbo_Filter_CountName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Filter_CountName.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Count_Head", "Count_Name", "", "(Count_IdNo = 0)")
    End Sub

    Private Sub cbo_Filter_CountName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Filter_CountName.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_CountName, cbo_Filter_PartyName, cbo_Filter_MillName, "Count_Head", "Count_Name", "", "(Count_IdNo = 0)")
    End Sub

    Private Sub cbo_Filter_CountName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Filter_CountName.KeyPress

        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Filter_CountName, cbo_Filter_MillName, "Count_Head", "Count_Name", "", "(Count_IdNo = 0)")

    End Sub


    Private Sub Open_BeamReceiptEntry()
        Dim movno As String

        On Error Resume Next

        movno = Trim(dgv_BeamDetails.CurrentRow.Cells(1).Value)

        If Val(movno) <> 0 Then
            move_record(movno)
            pnl_Back.Enabled = True
            pnl_BeamDetails.Visible = False
        End If

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

    Private Sub dgv_PavuDetails_CellEndEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_PavuDetails.CellEndEdit
        dgv_PavuDetails_CellLeave(sender, e)
    End Sub

    Private Sub dgv_PavuDetails_CellEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_PavuDetails.CellEnter
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim Dt2 As New DataTable
        Dim Rect As Rectangle
        With dgv_PavuDetails
            dgv_ActiveCtrl_Name = .Name

            If Val(.CurrentRow.Cells(dgvCol_Details.Slno).Value) = 0 Then
                .CurrentRow.Cells(dgvCol_Details.Slno).Value = .CurrentRow.Index + 1
            End If

            If e.ColumnIndex = dgvCol_Details.beam_width Then

                If Trim(UCase(cbo_Type.Text)) = "RECEIPT" Then 'Or Trim(UCase(cbo_Type.Text)) = "SIZING-UNIT PAVU DELIVERY" Then

                    cbo_Grid_BeamWidth.Enabled = False
                    cbo_Grid_BeamWidth.BackColor = Color.LightGray
                Else
                    cbo_Grid_BeamWidth.Enabled = True
                    cbo_Grid_BeamWidth.BackColor = Color.White
                End If

                If cbo_Grid_BeamWidth.Visible = False Or Val(cbo_Grid_BeamWidth.Tag) <> e.RowIndex Then

                    cbo_Grid_BeamWidth.Tag = -1
                    Da = New SqlClient.SqlDataAdapter("select Beam_Width_Name from Beam_Width_Head order by Beam_Width_Name", con)
                    Dt1 = New DataTable
                    Da.Fill(Dt1)
                    cbo_Grid_BeamWidth.DataSource = Dt1
                    cbo_Grid_BeamWidth.DisplayMember = "Beam_Width_Name"

                    Rect = .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False)

                    cbo_Grid_BeamWidth.Left = .Left + Rect.Left
                    cbo_Grid_BeamWidth.Top = .Top + Rect.Top

                    cbo_Grid_BeamWidth.Width = Rect.Width
                    cbo_Grid_BeamWidth.Height = Rect.Height
                    cbo_Grid_BeamWidth.Text = .CurrentCell.Value

                    cbo_Grid_BeamWidth.Tag = Val(e.RowIndex)
                    cbo_Grid_BeamWidth.Visible = True

                    cbo_Grid_BeamWidth.BringToFront()
                    cbo_Grid_BeamWidth.Focus()



                End If

            Else
                cbo_Grid_BeamWidth.Visible = False

            End If
            If e.RowIndex > 0 And e.ColumnIndex = dgvCol_Details.Beam_No Then
                If Val(.CurrentRow.Cells(dgvCol_Details.Beam_No).Value) = 0 And e.RowIndex = .RowCount - 1 Then
                    .CurrentRow.Cells(dgvCol_Details.Beam_No).Value = Val(.Rows(e.RowIndex - 1).Cells(dgvCol_Details.Beam_No).Value) + 1
                    .CurrentRow.Cells(dgvCol_Details.Ends_Count).Value = Trim(.Rows(e.RowIndex - 1).Cells(dgvCol_Details.Ends_Count).Value)
                    .CurrentRow.Cells(dgvCol_Details.Pcs).Value = .Rows(e.RowIndex - 1).Cells(dgvCol_Details.Pcs).Value
                    .CurrentRow.Cells(dgvCol_Details.meters).Value = .Rows(e.RowIndex - 1).Cells(dgvCol_Details.meters).Value
                    .CurrentRow.Cells(dgvCol_Details.beam_width).Value = .Rows(e.RowIndex - 1).Cells(dgvCol_Details.beam_width).Value
                    .CurrentRow.Cells(dgvCol_Details.Gross_Weight).Value = .Rows(e.RowIndex - 1).Cells(dgvCol_Details.Gross_Weight).Value
                    .CurrentRow.Cells(dgvCol_Details.Net_Weight).Value = .Rows(e.RowIndex - 1).Cells(dgvCol_Details.Net_Weight).Value
                    .CurrentRow.Cells(dgvCol_Details.Warp_Weight).Value = .Rows(e.RowIndex - 1).Cells(dgvCol_Details.Warp_Weight).Value

                    .CurrentRow.Cells(dgvCol_Details.Beam_Type).Value = .Rows(e.RowIndex - 1).Cells(dgvCol_Details.Beam_Type).Value

                    '.Rows.Add()
                End If
                If e.ColumnIndex = 1 And e.RowIndex = .RowCount - 1 And Val(.CurrentRow.Cells(dgvCol_Details.Pcs).Value) = 0 And Val(.CurrentRow.Cells(dgvCol_Details.meters).Value) = 0 Then
                    .CurrentRow.Cells(dgvCol_Details.Beam_No).Value = Val(.Rows(e.RowIndex - 1).Cells(dgvCol_Details.Beam_No).Value) + 1
                    .CurrentRow.Cells(dgvCol_Details.Ends_Count).Value = Trim(.Rows(e.RowIndex - 1).Cells(dgvCol_Details.Ends_Count).Value)
                    .CurrentRow.Cells(dgvCol_Details.Pcs).Value = .Rows(e.RowIndex - 1).Cells(dgvCol_Details.Pcs).Value
                    .CurrentRow.Cells(dgvCol_Details.meters).Value = .Rows(e.RowIndex - 1).Cells(dgvCol_Details.meters).Value
                    .CurrentRow.Cells(dgvCol_Details.beam_width).Value = .Rows(e.RowIndex - 1).Cells(dgvCol_Details.beam_width).Value
                    .CurrentRow.Cells(dgvCol_Details.Gross_Weight).Value = .Rows(e.RowIndex - 1).Cells(dgvCol_Details.Gross_Weight).Value
                    .CurrentRow.Cells(dgvCol_Details.Net_Weight).Value = .Rows(e.RowIndex - 1).Cells(dgvCol_Details.Net_Weight).Value
                    .CurrentRow.Cells(dgvCol_Details.Warp_Weight).Value = .Rows(e.RowIndex - 1).Cells(dgvCol_Details.Warp_Weight).Value

                    .CurrentRow.Cells(dgvCol_Details.Beam_Type).Value = .Rows(e.RowIndex - 1).Cells(dgvCol_Details.Beam_Type).Value

                    '.Rows.Add()
                End If
            End If
            If e.ColumnIndex = dgvCol_Details.Ends_Count Then

                If .CurrentCell.RowIndex > 0 And .CurrentRow.Cells(dgvCol_Details.Ends_Count).Value = "" Then
                    .CurrentRow.Cells(dgvCol_Details.Ends_Count).Value = Trim(.Rows(e.RowIndex - 1).Cells(dgvCol_Details.Ends_Count).Value)
                End If

                'If Val(dgv_PavuDetails.Rows(dgv_PavuDetails.CurrentCell.RowIndex).Cells(dgvCol_Details.Sts).Value) <> 0 Then
                If Trim(UCase(cbo_Type.Text)) = "RECEIPT"  Or Trim(UCase(cbo_Type.Text)) = "SIZING-UNIT PAVU DELIVERY" Then

                    cbo_Grid_EndsCount.Enabled = False
                    cbo_Grid_EndsCount.BackColor = Color.LightGray
                Else
                    cbo_Grid_EndsCount.Enabled = True
                    cbo_Grid_EndsCount.BackColor = Color.White
                End If

                If cbo_Grid_EndsCount.Visible = False Or Val(cbo_Grid_EndsCount.Tag) <> e.RowIndex Then

                    cbo_Grid_EndsCount.Tag = -1
                    Da = New SqlClient.SqlDataAdapter("select Count_Name from Count_Head order by Count_Name", con)
                    Dt1 = New DataTable
                    Da.Fill(Dt1)
                    cbo_Grid_EndsCount.DataSource = Dt1
                    cbo_Grid_EndsCount.DisplayMember = "Count_Name"
                    Rect = .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False)

                    cbo_Grid_EndsCount.Left = .Left + Rect.Left  '  .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Left
                    cbo_Grid_EndsCount.Top = .Top + Rect.Top  ' .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Top

                    cbo_Grid_EndsCount.Width = Rect.Width  ' .CurrentCell.Size.Width
                    cbo_Grid_EndsCount.Height = Rect.Height  ' rect.Height
                    cbo_Grid_EndsCount.Text = .CurrentCell.Value  '  Trim(.CurrentRow.Cells(e.ColumnIndex).Value)

                    cbo_Grid_EndsCount.Tag = Val(e.RowIndex)
                    cbo_Grid_EndsCount.Visible = True

                    cbo_Grid_EndsCount.BringToFront()
                    cbo_Grid_EndsCount.Focus()


                End If

            Else

                cbo_Grid_EndsCount.Visible = False

            End If

            If e.ColumnIndex = dgvCol_Details.Beam_Type Then

                If Trim(UCase(cbo_Type.Text)) = "RECEIPT" Then 'Or Trim(UCase(cbo_Type.Text)) = "SIZING-UNIT PAVU DELIVERY" Then

                    cbo_Grid_BeamType.Enabled = False
                    cbo_Grid_BeamType.BackColor = Color.LightGray
                Else
                    cbo_Grid_BeamType.Enabled = True
                    cbo_Grid_BeamType.BackColor = Color.White
                End If

                If cbo_Grid_BeamType.Visible = False Or Val(cbo_Grid_BeamType.Tag) <> e.RowIndex Then

                    'dgv_ActCtrlName = dgv_PavuDetails.Name

                    cbo_Grid_BeamType.Tag = -1
                    Rect = .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False)

                    cbo_Grid_BeamType.Left = .Left + Rect.Left  '  .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Left
                    cbo_Grid_BeamType.Top = .Top + Rect.Top  ' .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Top
                    cbo_Grid_BeamType.Width = Rect.Width  ' .CurrentCell.Size.Width
                    cbo_Grid_BeamType.Height = Rect.Height  ' rect.Height

                    cbo_Grid_BeamType.Text = .CurrentCell.Value  '  Trim(.CurrentRow.Cells(e.ColumnIndex).Value)

                    cbo_Grid_BeamType.Tag = Val(e.RowIndex)
                    cbo_Grid_BeamType.Visible = True

                    cbo_Grid_BeamType.BringToFront()
                    cbo_Grid_BeamType.Focus()

                End If

            Else

                cbo_Grid_BeamType.Visible = False

            End If

        End With
    End Sub

    Private Sub dgv_PavuDetails_CellLeave(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_PavuDetails.CellLeave
        With dgv_PavuDetails
            If .CurrentCell.ColumnIndex = dgvCol_Details.meters Or .CurrentCell.ColumnIndex = dgvCol_Details.Gross_Weight Or .CurrentCell.ColumnIndex = dgvCol_Details.Net_Weight Or .CurrentCell.ColumnIndex = dgvCol_Details.Warp_Weight Then
                If Common_Procedures.settings.Weaver_Zari_Kuri_Entries_Status = 1 Then
                    .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = Format(Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value), "#########0.000")
                Else
                    .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = Format(Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value), "#########0.00")

                End If
            End If
        End With
    End Sub

    Private Sub dgv_PavuDetails_CellValueChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_PavuDetails.CellValueChanged
        Try

            If FrmLdSTS = True Then Exit Sub
            If MovSTS = True Then Exit Sub

            If IsNothing(dgv_PavuDetails.CurrentCell) Then Exit Sub

            With dgv_PavuDetails
                If .Visible Then

                    If e.ColumnIndex = dgvCol_Details.Pcs Then
                        If Common_Procedures.settings.Weaver_Zari_Kuri_Entries_Status = 1 Then
                            .CurrentRow.Cells(dgvCol_Details.meters).Value = Format(Val(.CurrentRow.Cells(dgvCol_Details.Pcs).Value) * Val(txt_PcsLength.Text), "#########0.000")
                        Else
                            .CurrentRow.Cells(dgvCol_Details.meters).Value = Format(Val(.CurrentRow.Cells(dgvCol_Details.Pcs).Value) * Val(txt_PcsLength.Text), "#########0.00")

                        End If

                    End If
                    If e.ColumnIndex = dgvCol_Details.meters Or .CurrentCell.ColumnIndex = dgvCol_Details.Gross_Weight Or .CurrentCell.ColumnIndex = dgvCol_Details.Net_Weight Or .CurrentCell.ColumnIndex = dgvCol_Details.Warp_Weight Then
                        TotalPavu_Calculation()
                    End If
                    If (.CurrentCell.ColumnIndex = dgvCol_Details.Pcs Or .CurrentCell.ColumnIndex = dgvCol_Details.meters Or .CurrentCell.ColumnIndex = dgvCol_Details.Gross_Weight Or .CurrentCell.ColumnIndex = dgvCol_Details.Net_Weight Or .CurrentCell.ColumnIndex = dgvCol_Details.Warp_Weight) And Val(.CurrentCell.Value) <> 0 Then
                        If .CurrentRow.Index = .Rows.Count - 1 Then
                            .Rows.Add()
                        End If
                    End If

                End If
            End With

        Catch ex As Exception
            '----

        End Try

    End Sub

    Private Sub dgv_PavuDetails_EditingControlShowing(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewEditingControlShowingEventArgs) Handles dgv_PavuDetails.EditingControlShowing
        dgtxt_PavuDetails = CType(dgv_PavuDetails.EditingControl, DataGridViewTextBoxEditingControl)
    End Sub

    Private Sub dgv_PavuDetails_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgv_PavuDetails.GotFocus
        cbo_Grid_CountName.Visible = False
        cbo_Grid_MillName.Visible = False
        cbo_Grid_YarnType.Visible = False
    End Sub

    Private Sub dgv_PavuDetails_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_PavuDetails.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
    End Sub

    Private Sub dgv_PavuDetails_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_PavuDetails.KeyUp
        Dim n As Integer

        If e.Control = True And UCase(Chr(e.KeyCode)) = "D" Then

            With dgv_PavuDetails

                If Val(.Rows(.CurrentRow.Index).Cells(dgvCol_Details.Sts).Value) = 0 Then

                    n = .CurrentRow.Index

                    If .Rows.Count = 1 Then
                        For i = 0 To .Columns.Count - 1
                            .Rows(n).Cells(i).Value = ""
                        Next

                    Else
                        .Rows.RemoveAt(n)

                    End If

                    TotalPavu_Calculation()

                Else
                    MessageBox.Show("Already Pavu delivered", "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                    Exit Sub

                End If

            End With

        End If

    End Sub

    Private Sub dgv_Details_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgv_PavuDetails.LostFocus
        On Error Resume Next
        If IsNothing(dgv_PavuDetails.CurrentCell) Then Exit Sub
        dgv_PavuDetails.CurrentCell.Selected = False
    End Sub

    Private Sub dgv_PavuDetails_RowsAdded(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewRowsAddedEventArgs) Handles dgv_PavuDetails.RowsAdded
        Dim n As Integer

        If IsNothing(dgv_PavuDetails.CurrentCell) Then Exit Sub

        With dgv_PavuDetails
            n = .RowCount
            .Rows(n - 1).Cells(dgvCol_Details.Slno).Value = Val(n)
        End With
    End Sub

    Private Sub TotalPavu_Calculation()


        Dim Sno As Integer
        Dim TotBms As Single, TotPcs As Single, TotMtrs As Single
        Dim Tot_grswt As Single, Tot_Netswt As Single, Tot_Warpwt As Single

        If FrmLdSTS = True Then Exit Sub

        Sno = 0
        TotBms = 0
        TotPcs = 0
        TotMtrs = 0
        With dgv_PavuDetails
            For i = 0 To .RowCount - 1
                Sno = Sno + 1
                .Rows(i).Cells(dgvCol_Details.Slno).Value = Sno
                If Val(.Rows(i).Cells(dgvCol_Details.meters).Value) <> 0 Or Val(.Rows(i).Cells(dgvCol_Details.Gross_Weight).Value) <> 0 Or Val(.Rows(i).Cells(dgvCol_Details.Net_Weight).Value) <> 0 Or Val(.Rows(i).Cells(dgvCol_Details.Warp_Weight).Value) <> 0 Then
                    TotBms = TotBms + 1
                    TotPcs = TotPcs + Val(.Rows(i).Cells(dgvCol_Details.Pcs).Value)
                    TotMtrs = TotMtrs + Val(.Rows(i).Cells(dgvCol_Details.meters).Value)
                    Tot_grswt = Tot_grswt + Val(.Rows(i).Cells(dgvCol_Details.Gross_Weight).Value)
                    Tot_Netswt = Tot_Netswt + Val(.Rows(i).Cells(dgvCol_Details.Net_Weight).Value)
                    Tot_Warpwt = Tot_Warpwt + Val(.Rows(i).Cells(dgvCol_Details.Warp_Weight).Value)
                End If
            Next
        End With

        With dgv_PavuDetails_Total
            If .RowCount = 0 Then .Rows.Add()
            .Rows(0).Cells(dgvCol_Details.Beam_No).Value = Val(TotBms)
            .Rows(0).Cells(dgvCol_Details.Pcs).Value = Format(Val(TotPcs), "########0.00")
            If Common_Procedures.settings.Weaver_Zari_Kuri_Entries_Status = 1 Then
                .Rows(0).Cells(dgvCol_Details.meters).Value = Format(Val(TotMtrs), "########0.000")
            Else
                .Rows(0).Cells(dgvCol_Details.meters).Value = Format(Val(TotMtrs), "########0.00")
            End If
            .Rows(0).Cells(dgvCol_Details.Gross_Weight).Value = Format(Val(Tot_grswt), "########0.000")
            .Rows(0).Cells(dgvCol_Details.Net_Weight).Value = Format(Val(Tot_Netswt), "########0.000")
            .Rows(0).Cells(dgvCol_Details.Warp_Weight).Value = Format(Val(Tot_Warpwt), "########0.000")

        End With
        NetAmount_Calculation()
        Elogation_Calculation()

    End Sub

    Private Sub dgv_YarnDetails_CellEndEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_YarnDetails.CellEndEdit

        'If Trim(UCase(dgv_YarnDetails.CurrentRow.Cells(2).Value)) = "MILL" Then
        '    If dgv_YarnDetails.CurrentCell.ColumnIndex = 4 Or dgv_YarnDetails.CurrentCell.ColumnIndex = 5 Then
        '        get_MillCount_Details()
        '    End If
        'End If

    End Sub

    Private Sub dgv_YarnDetails_CellEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_YarnDetails.CellEnter
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim Dt2 As New DataTable
        Dim Dt3 As New DataTable
        Dim rect As Rectangle


        With dgv_YarnDetails
            dgv_YarnDetails.Tag = .CurrentCell.Value

            dgv_ActiveCtrl_Name = .Name

            If Val(.CurrentRow.Cells(0).Value) = 0 Then
                .CurrentRow.Cells(0).Value = .CurrentRow.Index + 1
            End If

            If Trim(.CurrentRow.Cells(2).Value) = "" Then
                .CurrentRow.Cells(2).Value = "MILL"
            End If

            If e.ColumnIndex = 1 And Val(lbl_BabyWgt.Text) = 0 Then

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

            If e.ColumnIndex = 2 And Val(lbl_BabyWgt.Text) = 0 Then

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

            If e.ColumnIndex = 3 And Val(lbl_BabyWgt.Text) = 0 Then

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

            If e.ColumnIndex = 7 Then

                If cbo_Grid_Yarn_LotNo.Visible = False Or Val(cbo_Grid_Yarn_LotNo.Tag) <> e.RowIndex Then

                    cbo_Grid_Yarn_LotNo.Tag = -1
                    'Da = New SqlClient.SqlDataAdapter("select LotCode_forSelection from Yarn_Lot_Head " &
                    '                                  "where Count_IdNo = (Select Count_IdNo from Count_Head where Count_Name = '" & dgv_YarnDetails.CurrentRow.Cells(1).Value & "') " &
                    '                                  " and Mill_IdNo = (Select Mill_IdNo from Mill_Head where Mill_Name = '" & dgv_YarnDetails.CurrentRow.Cells(3).Value & "') order by LotCode_forSelection", con)
                    'Dt2 = New DataTable
                    'Da.Fill(Dt2)
                    'cbo_Grid_YarnType.DataSource = Dt2
                    'cbo_Grid_YarnType.DisplayMember = "LotCode_forSelection"

                    rect = .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False)

                    cbo_Grid_Yarn_LotNo.Left = .Left + rect.Left  '  .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Left
                    cbo_Grid_Yarn_LotNo.Top = .Top + rect.Top  ' .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Top
                    cbo_Grid_Yarn_LotNo.Width = rect.Width  ' .CurrentCell.Size.Width
                    cbo_Grid_Yarn_LotNo.Height = rect.Height  ' rect.Height

                    cbo_Grid_Yarn_LotNo.Text = .CurrentCell.Value  '  Trim(.CurrentRow.Cells(e.ColumnIndex).Value)

                    cbo_Grid_Yarn_LotNo.Tag = Val(e.RowIndex)
                    cbo_Grid_Yarn_LotNo.Visible = True

                    cbo_Grid_Yarn_LotNo.BringToFront()
                    cbo_Grid_Yarn_LotNo.Focus()

                End If

            Else

                cbo_Grid_Yarn_LotNo.Visible = False

            End If


        End With
    End Sub

    Private Sub dgv_YarnDetails_CellLeave(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_YarnDetails.CellLeave
        With dgv_YarnDetails

            If .CurrentCell.ColumnIndex = 6 Then
                If Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value) <> 0 Then
                    .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = Format(Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value), "#########0.000")
                Else
                    .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = ""
                End If
            End If
        End With
    End Sub

    Private Sub dgv_YarnDetails_CellValueChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_YarnDetails.CellValueChanged

        Try
            If IsNothing(dgv_YarnDetails.CurrentCell) Then Exit Sub

            With dgv_YarnDetails
                If .Visible Then
                    If .CurrentCell.ColumnIndex = 4 Or .CurrentCell.ColumnIndex = 5 Or .CurrentCell.ColumnIndex = 6 Then
                        If Val(dgv_YarnDetails.Tag) <> Val(.CurrentCell.Value) Then
                            If Trim(UCase(.CurrentRow.Cells(2).Value)) = "MILL" Then
                                If .CurrentCell.ColumnIndex = 4 Or .CurrentCell.ColumnIndex = 5 Then
                                    get_MillCount_Details()
                                End If
                            End If
                            TotalYarnTaken_Calculation()
                        End If
                    End If
                End If
            End With

        Catch ex As Exception
            '----

        End Try

    End Sub

    Private Sub dgv_YarnDetails_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_YarnDetails.KeyDown
        On Error Resume Next
        vcbo_KeyDwnVal = e.KeyValue
    End Sub

    Private Sub dgv_YarnDetails_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_YarnDetails.KeyUp
        Dim i As Integer
        Dim n As Integer

        If e.Control = True And UCase(Chr(e.KeyCode)) = "D" Then

            With dgv_YarnDetails

                n = .CurrentRow.Index

                If .Rows.Count = 1 Then
                    For i = 0 To .Columns.Count - 1
                        .Rows(n).Cells(i).Value = ""
                    Next

                Else

                    .Rows.RemoveAt(n)

                End If

                TotalYarnTaken_Calculation()

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

    Private Sub Excess_Calculation()
        Dim Exsh As Single = 0

        Exsh = Val(txt_ConsumedYarn.Text) + Val(txt_BabyWt.Text) - Val(txt_YarnTaken.Text)
        txt_ExcessShort.Text = Format(Val(Exsh), "##########0.000")

    End Sub

    Private Sub Elogation_Calculation()
        Dim xx As Single
        Dim Elgmtr As Single
        Dim SizMtr As Single
        Dim aa As Single
        Dim ElgPerc As Single = 0


        xx = 0
        aa = 0
        Elgmtr = 0
        SizMtr = 0

        If Trim(UCase(cbo_BeamCount_Type.Text)) = "YARDS" Then
            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1081" Then '---- S.Ravichandran Textiles (Erode)
                aa = 36.5
            Else
                aa = 36
            End If

        Else
            aa = 39.37
        End If

        xx = Format(Val(txt_WarpMtr.Text), "########0.00")

        If Val(txt_TapeLength.Text) <> 0 Then
            xx = Format((Val(txt_WarpMtr.Text) * aa) / (Val(txt_TapeLength.Text)), "########0.00")
        End If

        SizMtr = 0
        If dgv_PavuDetails_Total.Rows.Count > 0 Then
            SizMtr = Val(dgv_PavuDetails_Total.Rows(0).Cells(dgvCol_Details.meters).Value)
        End If

        Elgmtr = Val(SizMtr) - Val(xx)

        lbl_Elogation.Text = Format(Val(Elgmtr), "#########0.00")

        If xx <> 0 Then
            ElgPerc = Val(Elgmtr) / xx * 100
            lbl_Elongation_Perc.Text = Format(Val(ElgPerc), "#####0.00")
        Else
            lbl_Elongation_Perc.Text = 0
        End If

    End Sub

    Private Sub AverageCount_Calculation()
        Dim xx As Single
        Dim Bmcnt As Single
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim ends As Single
        Dim rate As Single = 0
        Dim endsCnt_idno As Integer = 0
        Dim Mtr_Divisor As Single, Yrd_Divisor As Single

        If Trim(UCase(cbo_EndsCount.Text)) <> "" Then
            endsCnt_idno = Common_Procedures.EndsCount_NameToIdNo(con, Trim(cbo_EndsCount.Text))

            da = New SqlClient.SqlDataAdapter("select a.*  from EndsCount_Head a  Where a.EndsCount_IdNo = " & Str(Val(endsCnt_idno)), con)
            dt = New DataTable
            da.Fill(dt)

            ends = 0
            If dt.Rows.Count > 0 Then
                If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                    ends = Val(dt.Rows(0).Item("Ends_Name").ToString)
                End If
            End If

            dt.Dispose()
            da.Dispose()
        End If

        Mtr_Divisor = 1690
        Yrd_Divisor = 1848
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1035" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1036" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1489" Then  '---- Kalaimagal Textiles (Avinashi)     OR  KVP WEAVES (ANNUR)
            Mtr_Divisor = 1693
        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1123" Then '---- Shanthi Sizing(Somanur)  or  SRI NIKITHA SIZING MILLS (SOMANUR)
            Mtr_Divisor = 1689.81
        End If

        Bmcnt = 0
        xx = 0
        If Trim(UCase(cbo_BeamCount_Type.Text)) = "METERS" Then
            xx = (Val(txt_WarpMtr.Text) * Val(ends)) / Mtr_Divisor

        Else
            xx = (Val(txt_WarpMtr.Text) * Val(ends)) / Yrd_Divisor

        End If

        If Val(txt_ConsumedYarn.Text) <> 0 Then
            Bmcnt = Format(Val(xx) / Val(txt_ConsumedYarn.Text), "#########0.00")
        End If

        lbl_Avg_Count.Text = Format(Val(Bmcnt), "#########0.00")

    End Sub

    Private Sub NetAmount_Calculation()
        If FrmLdSTS = True Then Exit Sub

        Dim GrsAmt As String = ""
        Dim TdsAmt As String = ""
        Dim NtAmt As String = ""
        Dim vStrNetAmt As String = ""
        Dim vStrGrsAmt As String = ""
        Dim vPackgAmt As String = ""


        If Trim(UCase(Cbo_RateFor.Text)) = Trim(UCase("METER")) Then
            lbl_InvoiceAmt.Text = Format(Val(txt_InvRate.Text) * Val(txt_WarpMtr.Text), "#########0.00")
        Else
            lbl_InvoiceAmt.Text = Format(Val(txt_InvRate.Text) * Val(txt_ConsumedYarn.Text), "#########0.00")
        End If


        vPackgAmt = 0
        If dgv_PavuDetails_Total.Rows.Count > 0 Then
            vPackgAmt = Format(Val(txt_PackingRate.Text) * Val(dgv_PavuDetails_Total.Rows(0).Cells(dgvCol_Details.Beam_No).Value), "#########0.00")
        End If
        lbl_PackingAmt.Text = Format(Val(vPackgAmt), "#########0.00")

        lbl_WindingAmt.Text = ""
        If Chk_RWSts.Checked = True Then
            lbl_WindingAmt.Text = Format(Val(txt_WindingRate.Text) * Val(txt_BabyWt.Text), "#########0.00")
        End If

        lbl_Taxable_Value.Text = Format(Val(lbl_InvoiceAmt.Text) + Val(lbl_PackingAmt.Text) + Val(lbl_WindingAmt.Text) + Val(txt_AddLess.Text), "###########0.00")

        'GrsAmt = Format(Val(lbl_InvoiceAmt.Text) + Val(lbl_PackingAmt.Text) + Val(lbl_WindingAmt.Text) + Val(txt_AddLess.Text), "###########0")
        'lbl_GrossAmt.Text = Format(Val(GrsAmt), "###########0.00")
        'lbl_Taxable_Value.Text = Format(Val(lbl_GrossAmt.Text), "###########0.00")

        'Gst
        lbl_CGST_Amount.Text = Format(Val(lbl_Taxable_Value.Text) * Val(txt_CGST_Percentage.Text) / 100, "##########0.00")
        lbl_SGST_Amount.Text = Format(Val(lbl_Taxable_Value.Text) * Val(txt_SGST_Percentage.Text) / 100, "##########0.00")

        If chk_TaxAmount_RoundOff_STS.Checked = True Then
            lbl_Taxable_Value.Text = Format(Val(lbl_Taxable_Value.Text), "###########0")
            lbl_Taxable_Value.Text = Format(Val(lbl_Taxable_Value.Text), "###########0.00")

            lbl_CGST_Amount.Text = Format(Val(lbl_CGST_Amount.Text), "##########0")
            lbl_CGST_Amount.Text = Format(Val(lbl_CGST_Amount.Text), "##########0.00")

            lbl_SGST_Amount.Text = Format(Val(lbl_SGST_Amount.Text), "##########0")
            lbl_SGST_Amount.Text = Format(Val(lbl_SGST_Amount.Text), "##########0.00")
        End If

        GrsAmt = Format(Val(lbl_Taxable_Value.Text) + Val(lbl_CGST_Amount.Text) + Val(lbl_SGST_Amount.Text), "###########0")
        lbl_GrossAmt.Text = Format(Val(GrsAmt), "###########0.00")

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1101" Then    '------AVR MILLS PRIVATE LMITED(GOBI)  -- ITS WRONG BUT THEY NEED FOR NETAMOUNT(THEY KNEW ITS WRONG)
            TdsAmt = Format(Val(lbl_GrossAmt.Text) * Val(txt_TdsPerc.Text) / 100, "#########000.00")
        Else
            TdsAmt = Format(Val(lbl_Taxable_Value.Text) * Val(txt_TdsPerc.Text) / 100, "###########0.00").ToString
        End If
        lbl_TdsAmount.Text = Format(Val(TdsAmt), "#########0")

        NtAmt = Format(Val(lbl_GrossAmt.Text) - Val(lbl_TdsAmount.Text), "##########0")

        vStrGrsAmt = Format(Val(lbl_Taxable_Value.Text) + Val(lbl_CGST_Amount.Text) + Val(lbl_SGST_Amount.Text), "###########0.00")

        lbl_RoundOff.Text = Format(Val(CSng(lbl_GrossAmt.Text)) - Val(vStrGrsAmt), "#########0.00")

        lbl_NetAmount.Text = Format(Val(NtAmt), "##########0.00")

        lbl_NetAmount.Text = Common_Procedures.Currency_Format(Val(CSng(lbl_NetAmount.Text)))

    End Sub

    Private Sub TotalYarnTaken_Calculation()
        Dim Sno As Integer
        Dim TotBags As Single, TotCones As Single, TotWeight As Single

        If FrmLdSTS = True Then Exit Sub

        Sno = 0
        TotBags = 0
        TotCones = 0
        TotWeight = 0
        With dgv_YarnDetails
            For i = 0 To .RowCount - 1
                Sno = Sno + 1
                .Rows(i).Cells(0).Value = Sno
                If Val(.Rows(i).Cells(6).Value) <> 0 Then
                    TotBags = TotBags + Val(.Rows(i).Cells(4).Value)
                    TotCones = TotCones + Val(.Rows(i).Cells(5).Value)
                    TotWeight = TotWeight + Val(.Rows(i).Cells(6).Value)
                End If
            Next
        End With

        With dgv_YarnDetails_Total
            If .RowCount = 0 Then .Rows.Add()
            .Rows(0).Cells(4).Value = Val(TotBags)
            .Rows(0).Cells(5).Value = Val(TotCones)
            .Rows(0).Cells(6).Value = Format(Val(TotWeight), "########0.000")
        End With

        If Trim(UCase(cbo_YarnStock.Text)) = Trim(UCase("CONSUMED YARN")) Then
            txt_ConsumedYarn.Text = Format(Val(dgv_YarnDetails_Total.Rows(0).Cells(6).Value), "########0.000")
        Else
            txt_YarnTaken.Text = Format(Val(dgv_YarnDetails_Total.Rows(0).Cells(6).Value), "########0.000")
        End If
        NetAmount_Calculation()



    End Sub

    Private Sub cbo_Grid_CountName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Grid_CountName.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Count_Head", "Count_Name", "", "(Count_IdNo = 0)")
    End Sub

    Private Sub cbo_Grid_CountName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Grid_CountName.KeyDown
        Dim dep_idno As Integer = 0

        vcbo_KeyDwnVal = e.KeyValue

        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Grid_CountName, Nothing, Nothing, "Count_Head", "Count_Name", "", "(Count_IdNo = 0)")

        With dgv_YarnDetails

            If (e.KeyValue = 38 And cbo_Grid_CountName.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
                If Trim(UCase(cbo_Type.Text)) = "RECEIPT" Or Trim(UCase(cbo_Type.Text)) = "SIZING-UNIT PAVU DELIVERY" Then
                    If Val(.CurrentCell.RowIndex) <= 0 Then
                        cbo_YarnStock.Focus()

                    Else
                        .Focus()

                        If dgv_YarnDetails.Columns(7).Visible Then
                            .CurrentCell = .Rows(.CurrentCell.RowIndex - 1).Cells(7)
                        Else
                            .CurrentCell = .Rows(.CurrentCell.RowIndex - 1).Cells(6)
                        End If

                        .CurrentCell.Selected = True

                        End If

                        Else

                    If Val(.CurrentCell.RowIndex) <= 0 Then
                        If dgv_PavuDetails.Rows.Count = 0 Then dgv_PavuDetails.Rows.Add()
                        dgv_PavuDetails.CurrentCell = dgv_PavuDetails.Rows(0).Cells(dgvCol_Details.Beam_No)
                        dgv_PavuDetails.Focus()
                        dgv_PavuDetails.CurrentCell.Selected = True

                    Else
                        .Focus()
                        If dgv_YarnDetails.Columns(7).Visible Then
                            .CurrentCell = .Rows(.CurrentCell.RowIndex - 1).Cells(7)
                        Else
                            .CurrentCell = .Rows(.CurrentCell.RowIndex - 1).Cells(6)
                        End If
                        .CurrentCell.Selected = True

                    End If

                End If

            End If

            If (e.KeyValue = 40 And cbo_Grid_CountName.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then

                If .CurrentCell.RowIndex = .RowCount - 1 And .CurrentCell.ColumnIndex >= 1 And Trim(.CurrentRow.Cells(1).Value) = "" Then
                    txt_TapeLength.Focus()

                Else
                    .Focus()
                    .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)

                End If

            End If

        End With

    End Sub

    Private Sub cbo_Grid_CountName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Grid_CountName.KeyPress
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable

        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Grid_CountName, Nothing, "Count_Head", "Count_Name", "", "(Count_IdNo = 0)")

        If Asc(e.KeyChar) = 13 Then

            With dgv_YarnDetails

                .Item(.CurrentCell.ColumnIndex, .CurrentRow.Index).Value = Trim(cbo_Grid_CountName.Text)
                If .CurrentCell.RowIndex = .RowCount - 1 And .CurrentCell.ColumnIndex >= 1 And Trim(.CurrentRow.Cells(1).Value) = "" Then
                    txt_TapeLength.Focus()

                Else
                    .Focus()
                    .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)

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
                With dgv_YarnDetails
                    If Val(cbo_Grid_CountName.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = 1 Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(1).Value = Trim(cbo_Grid_CountName.Text)
                    End If
                End With
            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub cbo_Grid_MillName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Grid_MillName.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Mill_Head", "Mill_Name", "", "(Mill_IdNo = 0)")
    End Sub

    Private Sub cbo_Grid_MillName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Grid_MillName.KeyDown
        Dim dep_idno As Integer = 0

        vcbo_KeyDwnVal = e.KeyValue

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

    Private Sub dgv_YarnDetails_EditingControlShowing(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewEditingControlShowingEventArgs) Handles dgv_YarnDetails.EditingControlShowing
        dgtxt_YarnDetails = CType(dgv_YarnDetails.EditingControl, DataGridViewTextBoxEditingControl)
    End Sub

    Private Sub dgtxt_YarnDetails_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgtxt_YarnDetails.Enter
        dgtxt_YarnDetails.Tag = dgtxt_YarnDetails.Text
        dgv_ActiveCtrl_Name = dgv_YarnDetails.Name
        dgv_YarnDetails.EditingControl.BackColor = Color.Lime
        dgv_YarnDetails.EditingControl.ForeColor = Color.Blue
        dgv_YarnDetails.SelectAll()
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
            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub cbo_Grid_YarnType_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Grid_YarnType.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "YarnType_Head", "Yarn_Type", "", "(Yarn_Type='')")
    End Sub

    Private Sub cbo_Grid_YarnType_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Grid_YarnType.KeyDown
        Dim dep_idno As Integer = 0

        vcbo_KeyDwnVal = e.KeyValue

        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Grid_YarnType, Nothing, Nothing, "YarnType_Head", "Yarn_Type", "", "(Yarn_Type = '')")

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


        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Grid_YarnType, Nothing, "YarnType_Head", "Yarn_Type", "", "(Yarn_Type='')")

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
            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub btn_save_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_save.Click
        save_record()
    End Sub

    Private Sub btn_close_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_close.Click
        Me.Close()
    End Sub

    Private Sub cbo_Filter_MillName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Filter_MillName.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Mill_Head", "Mill_Name", "", "(Mill_IdNo = 0)")
    End Sub

    Private Sub cbo_Filter_MillName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Filter_MillName.KeyDown
        vcbo_KeyDwnVal = e.KeyValue

        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_MillName, cbo_Filter_CountName, cbo_Filter_EndsCount, "Mill_Head", "Mill_Name", "", "(Mill_IdNo = 0)")

    End Sub

    Private Sub cbo_Filter_MillName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Filter_MillName.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Filter_MillName, cbo_Filter_EndsCount, "Mill_Head", "Mill_Name", "", "(Mill_IdNo = 0)")
        'If Asc(e.KeyChar) = 13 Then
        '    btn_Filter_Show_Click(sender, e)
        'End If
    End Sub

    Private Sub cbo_Filter_EndsCount_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Filter_EndsCount.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "EndsCount_Head", "EndsCount_Name", "", "(EndsCount_IdNo = 0)")
    End Sub

    Private Sub cbo_Filter_EndsCountName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Filter_EndsCount.KeyDown
        vcbo_KeyDwnVal = e.KeyValue

        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_EndsCount, cbo_Filter_MillName, txt_Filter_SetNo, "EndsCount_Head", "EndsCount_Name", "", "(EndsCount_IdNo = 0)")

    End Sub

    Private Sub cbo_Filter_EndsCountName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Filter_EndsCount.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Filter_EndsCount, txt_Filter_SetNo, "EndsCount_Head", "EndsCount_Name", "", "(EndsCount_IdNo = 0)")
        'If Asc(e.KeyChar) = 13 Then
        '    btn_Filter_Show_Click(sender, e)
        'End If
    End Sub

    Public Sub print_record() Implements Interface_MDIActions.print_record
        '-----
    End Sub

    Private Sub txt_BabyWt_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_BabyWt.KeyDown
        'If e.KeyCode = 40 Then
        '    If dgv_PavuDetails.Rows.Count = 0 Then dgv_PavuDetails.Rows.Add()
        '    dgv_PavuDetails.CurrentCell = dgv_PavuDetails.Rows(0).Cells(dgvCol_Details.Beam_No)
        '    dgv_PavuDetails.Focus()
        '    dgv_PavuDetails.CurrentCell.Selected = True
        'End If
        'If e.KeyCode = 38 Then SendKeys.Send("+{TAB}")
    End Sub

    Private Sub txt_RWES_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_RWES.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub txt_RwCns_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_RwCns.KeyDown
        If e.KeyCode = 40 Then SendKeys.Send("{TAB}")
        If e.KeyCode = 38 Then SendKeys.Send("+{TAB}")

    End Sub

    Private Sub txt_RwCns_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_RwCns.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub txt_PcsLength_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_PcsLength.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub txt_ExcessShort_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_ExcessShort.KeyPress
        If Common_Procedures.Accept_NegativeNumbers(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub txt_BabyWt_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_BabyWt.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
        'If Asc(e.KeyChar) = 13 Then
        '    If dgv_PavuDetails.Rows.Count = 0 Then dgv_PavuDetails.Rows.Add()
        '    dgv_PavuDetails.CurrentCell = dgv_PavuDetails.Rows(0).Cells(dgvCol_Details.Beam_No)
        '    dgv_PavuDetails.Focus()
        '    dgv_PavuDetails.CurrentCell.Selected = True
        'End If
    End Sub

    Private Sub Chk_RWSts_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Chk_RWSts.Click
        If Chk_RWSts.Checked = True Then

            txt_RwBags.Enabled = True
            txt_RwCns.Enabled = True
            txt_RWES.Enabled = True
        Else

            txt_RwBags.Enabled = False
            txt_RwCns.Enabled = False
            txt_RWES.Enabled = False

        End If

        If Chk_RWSts.Checked = True And Val(txt_RwCns.Text) = 0 And Val(txt_BabyWt.Text) <> 0 Then
            txt_RwCns.Text = Format(Val(txt_BabyWt.Text) / 1.5, "##########0")
        End If

    End Sub

    Private Sub Chk_RW_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Chk_RWSts.KeyDown
        If e.KeyCode = 40 Then SendKeys.Send("{TAB}")
        If e.KeyCode = 38 Then SendKeys.Send("+{TAB}")
    End Sub

    Private Sub txt_Filter_SetNo_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_Filter_SetNo.KeyDown
        If e.KeyValue = 40 Or (e.Control = True And e.KeyValue = 40) Then
            If Common_Procedures.settings.CustomerCode = "1249" Then
                cbo_Verified_Sts.Focus()
            Else
                btn_Filter_Show.Focus()

            End If

        End If
        If e.KeyCode = 38 Then cbo_Filter_EndsCount.Focus()
    End Sub

    Private Sub txt_Filter_SetNo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Filter_SetNo.KeyPress
        If Asc(e.KeyChar) = 13 Then
            If Common_Procedures.settings.CustomerCode = "1249" Then
                cbo_Verified_Sts.Focus()
            Else
                btn_Filter_Show.Focus()

            End If
        End If
    End Sub

    Private Sub cbo_Verified_Sts_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Verified_Sts.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "", "", "", "")

    End Sub
    Private Sub cbo_Verified_Sts_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Verified_Sts.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Verified_Sts, txt_Filter_SetNo, btn_Filter_Show, "", "", "", "")
    End Sub

    Private Sub cbo_Verified_Sts_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Verified_Sts.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Verified_Sts, btn_Filter_Show, "", "", "", "")
    End Sub

    Private Sub dgtxt_PavuDetails_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgtxt_PavuDetails.Enter
        dgv_ActiveCtrl_Name = dgv_PavuDetails.Name
        dgv_PavuDetails.EditingControl.BackColor = Color.Lime
        dgv_PavuDetails.EditingControl.ForeColor = Color.Blue
        dgv_PavuDetails.SelectAll()
    End Sub

    Private Sub dgtxt_YarnDetails_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dgtxt_YarnDetails.KeyPress
        With dgv_YarnDetails

            If .CurrentCell.ColumnIndex = 4 Or .CurrentCell.ColumnIndex = 5 Or .CurrentCell.ColumnIndex = 6 Then

                If Val(lbl_BabyWgt.Text) <> 0 Then
                    e.Handled = True
                Else
                    If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
                        e.Handled = True
                    End If
                End If

            End If
        End With

    End Sub

    Private Sub dgtxt_yarnDetails_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgtxt_YarnDetails.KeyUp

        If e.Control = True And UCase(Chr(e.KeyCode)) = "D" Then
            dgv_YarnDetails_KeyUp(sender, e)
        End If

    End Sub

    Private Sub dgtxt_PavuDetails_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgtxt_PavuDetails.KeyDown
        If Trim(UCase(cbo_Type.Text)) = "RECEIPT" Or Trim(UCase(cbo_Type.Text)) = "SIZING-UNIT PAVU DELIVERY" Then
            e.Handled = True
            e.SuppressKeyPress = True

        Else

            If Val(dgv_PavuDetails.Rows(dgv_PavuDetails.CurrentCell.RowIndex).Cells(dgvCol_Details.Sts).Value) <> 0 Then
                e.SuppressKeyPress = True
                e.Handled = True
            End If

        End If
    End Sub

    Private Sub dgtxt_PavuDetails_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dgtxt_PavuDetails.KeyPress
        If Trim(UCase(cbo_Type.Text)) = "RECEIPT" Or Trim(UCase(cbo_Type.Text)) = "SIZING-UNIT PAVU DELIVERY" Then
            e.Handled = True

        Else

            If Val(dgv_PavuDetails.Rows(dgv_PavuDetails.CurrentCell.RowIndex).Cells(dgvCol_Details.Sts).Value) <> 0 Then
                e.Handled = True
            Else
                If dgv_PavuDetails.CurrentCell.ColumnIndex = dgvCol_Details.Pcs Or dgv_PavuDetails.CurrentCell.ColumnIndex = dgvCol_Details.meters Or dgv_PavuDetails.CurrentCell.ColumnIndex = dgvCol_Details.Gross_Weight Or dgv_PavuDetails.CurrentCell.ColumnIndex = dgvCol_Details.Net_Weight Or dgv_PavuDetails.CurrentCell.ColumnIndex = dgvCol_Details.Warp_Weight Then
                    If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
                End If
            End If

        End If
    End Sub

    Private Sub dgtxt_PavuDetails_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgtxt_PavuDetails.KeyUp
        If Trim(UCase(cbo_Type.Text)) <> "RECEIPT" And Trim(UCase(cbo_Type.Text)) <> "SIZING-UNIT PAVU DELIVERY" Then
            If e.Control = True And UCase(Chr(e.KeyCode)) = "D" Then
                dgv_PavuDetails_KeyUp(sender, e)
            End If
        End If

    End Sub

    Private Sub txt_TapeLength_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_TapeLength.KeyDown
        If e.KeyCode = 38 Then
            If dgv_YarnDetails.Rows.Count > 0 Then
                dgv_YarnDetails.Focus()
                dgv_YarnDetails.CurrentCell = dgv_YarnDetails.Rows(0).Cells(1)

            Else
                txt_TotalBeams.Focus()

            End If
        End If

        If e.KeyCode = 40 Then SendKeys.Send("{TAB}")
    End Sub

    Private Sub txt_TapeLength_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_TapeLength.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub txt_TdsPerc_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_TdsPerc.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
        If Asc(e.KeyChar) = 13 Then

            If txt_Sizing_Net_Wgt.Visible And txt_Sizing_Net_Wgt.Enabled Then
                txt_Sizing_Net_Wgt.Focus()

            ElseIf lbl_PickUp_Perc.Visible And lbl_PickUp_Perc.Enabled Then
                lbl_PickUp_Perc.Focus()
            Else

                If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                    save_record()
                Else
                    msk_Date.Focus()
                End If
            End If
        End If
    End Sub

    Private Sub txt_Elogation_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub txt_PickUp_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_PickUp.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub txt_TdsPerc_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_TdsPerc.TextChanged
        NetAmount_Calculation()
    End Sub

    Private Sub txt_Amount_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        NetAmount_Calculation()
    End Sub

    Private Sub btn_Selection_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Selection.Click
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim i As Integer, j As Integer, n As Integer, SNo As Integer
        Dim LedIdNo As Integer
        Dim NewCode As String
        Dim Ent_Rate As Single = 0
        Dim Ent_Wgt As Single = 0
        Dim Ent_Pcs As Single = 0
        Dim NR As Single = 0
        Dim LedCondt As String = ""
        Dim CompIDCondt As String

        If Trim(UCase(cbo_Type.Text)) <> "RECEIPT" And Trim(UCase(cbo_Type.Text)) <> "SIZING-UNIT PAVU DELIVERY" Then Exit Sub

        If New_Entry = False Then
            If Trim(UCase(cbo_Type.Text)) <> "RECEIPT" And Trim(UCase(cbo_Type.Text)) <> "SIZING-UNIT PAVU DELIVERY" Then
                MessageBox.Show("Invalid Entry Type", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                If cbo_Type.Enabled And cbo_Type.Visible Then cbo_Type.Focus()
                Exit Sub
            End If
        End If

        CompIDCondt = "(a.company_idno = " & Str(Val(lbl_Company.Tag)) & ")"
        If Common_Procedures.settings.EntrySelection_Combine_AllCompany = 1 Then
            CompIDCondt = ""
            If Trim(UCase(Common_Procedures.User.Type)) = "ACCOUNT" Then
                CompIDCondt = "(tZ.Company_Type <> 'UNACCOUNT')"
            End If
        End If

        LedIdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Sizing.Text)
        LedCondt = ""
        If LedIdNo <> 0 Then
            LedCondt = "(a.Ledger_Idno = " & Str(Val(LedIdNo)) & ")"
        End If

        NewCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        If Trim(UCase(cbo_Type.Text)) = "RECEIPT" Then

            With dgv_Selection

                .Rows.Clear()
                SNo = 0

                Da = New SqlClient.SqlDataAdapter("Select a.*, c.Ledger_Name, d.EndsCount_Name from Sizing_Pavu_Receipt_Head a INNER JOIN Company_Head tZ ON a.company_idno <> 0 and a.company_idno = tZ.company_idno INNER JOIN Ledger_Head c ON a.Ledger_Idno = c.Ledger_Idno INNER JOIN EndsCount_Head d ON a.EndsCount_IdNo = d.EndsCount_IdNo where  " & CompIDCondt & IIf(Trim(CompIDCondt) <> "", " and ", "") & "  " & LedCondt & IIf(Trim(LedCondt) <> "", " and ", "") & " a.Sizing_Specification_Code = '" & Trim(NewCode) & "' order by a.Sizing_Pavu_Receipt_Date, a.for_orderby, a.Sizing_Pavu_Receipt_No", con)
                Dt1 = New DataTable
                Da.Fill(Dt1)

                If Dt1.Rows.Count > 0 Then

                    For i = 0 To Dt1.Rows.Count - 1

                        n = .Rows.Add()

                        SNo = SNo + 1
                        .Rows(n).Cells(0).Value = Val(SNo)

                        .Rows(n).Cells(1).Value = Dt1.Rows(i).Item("Sizing_Pavu_Receipt_No").ToString
                        .Rows(n).Cells(2).Value = Format(Convert.ToDateTime(Dt1.Rows(i).Item("Sizing_Pavu_Receipt_Date").ToString), "dd-MM-yyyy")
                        .Rows(n).Cells(3).Value = Dt1.Rows(i).Item("Set_No").ToString
                        .Rows(n).Cells(4).Value = Dt1.Rows(i).Item("Ledger_Name").ToString
                        .Rows(n).Cells(5).Value = Dt1.Rows(i).Item("EndsCount_Name").ToString
                        .Rows(n).Cells(6).Value = Dt1.Rows(i).Item("Total_Beam").ToString
                        .Rows(n).Cells(7).Value = Dt1.Rows(i).Item("Total_Meters").ToString
                        .Rows(n).Cells(8).Value = "1"
                        .Rows(n).Cells(9).Value = Dt1.Rows(i).Item("Sizing_Pavu_Receipt_Code").ToString
                        .Rows(n).Cells(10).Value = Dt1.Rows(i).Item("Our_Order_No").ToString
                        .Rows(n).Cells(11).Value = Dt1.Rows(i).Item("OWN_Order_Code").ToString
                        For j = 0 To .ColumnCount - 1
                            .Rows(i).Cells(j).Style.ForeColor = Color.Red
                        Next

                    Next

                End If
                Dt1.Clear()

                Da = New SqlClient.SqlDataAdapter("select a.*, c.Ledger_Name, d.EndsCount_Name from Sizing_Pavu_Receipt_Head a  INNER JOIN Company_Head tZ ON a.company_idno <> 0 and a.company_idno = tZ.company_idno INNER JOIN Ledger_Head c ON a.Ledger_Idno = c.Ledger_Idno INNER JOIN EndsCount_Head d ON a.EndsCount_IdNo = d.EndsCount_IdNo where  " & CompIDCondt & IIf(Trim(CompIDCondt) <> "", " and ", "") & "  " & LedCondt & IIf(Trim(LedCondt) <> "", " and ", "") & " a.Sizing_Specification_Code = ''  order by a.Sizing_Pavu_Receipt_Date, a.for_orderby, a.Sizing_Pavu_Receipt_No", con)
                Dt1 = New DataTable
                Da.Fill(Dt1)

                If Dt1.Rows.Count > 0 Then

                    For i = 0 To Dt1.Rows.Count - 1

                        n = .Rows.Add()

                        SNo = SNo + 1
                        .Rows(n).Cells(0).Value = Val(SNo)
                        .Rows(n).Cells(1).Value = Dt1.Rows(i).Item("Sizing_Pavu_Receipt_No").ToString
                        .Rows(n).Cells(2).Value = Format(Convert.ToDateTime(Dt1.Rows(i).Item("Sizing_Pavu_Receipt_Date").ToString), "dd-MM-yyyy")
                        .Rows(n).Cells(3).Value = Dt1.Rows(i).Item("Set_No").ToString
                        .Rows(n).Cells(4).Value = Dt1.Rows(i).Item("Ledger_Name").ToString
                        .Rows(n).Cells(5).Value = Dt1.Rows(i).Item("EndsCount_Name").ToString
                        .Rows(n).Cells(6).Value = Dt1.Rows(i).Item("Total_Beam").ToString
                        .Rows(n).Cells(7).Value = Dt1.Rows(i).Item("Total_Meters").ToString
                        .Rows(n).Cells(8).Value = ""
                        .Rows(n).Cells(9).Value = Dt1.Rows(i).Item("Sizing_Pavu_Receipt_Code").ToString
                        .Rows(n).Cells(10).Value = Dt1.Rows(i).Item("Our_Order_No").ToString
                        .Rows(n).Cells(11).Value = Dt1.Rows(i).Item("OWN_Order_Code").ToString

                        For j = 0 To .ColumnCount - 1
                            .Rows(i).Cells(j).Style.ForeColor = Color.Black
                        Next

                    Next

                End If
                Dt1.Clear()
                pnl_Selection.Visible = True
                pnl_Back.Enabled = False
                dgv_Selection.Focus()

            End With


        ElseIf Trim(UCase(cbo_Type.Text)) = "SIZING-UNIT PAVU DELIVERY" Then

            LedCondt = ""
            If LedIdNo <> 0 Then
                LedCondt = "(a.Sizing_Idno = " & Str(Val(LedIdNo)) & ")"
            End If

            With dgv_Selection

                .Rows.Clear()
                SNo = 0


                Da = New SqlClient.SqlDataAdapter("Select a.*, c.Ledger_Name, d.EndsCount_Name from SizSoft_Pavu_Delivery_Head a INNER JOIN Ledger_Head c ON a.Sizing_Idno = c.Ledger_Idno LEFT OUTER JOIN EndsCount_Head d ON d.EndsCount_IdNo = a.First_EndsCount_IdNo Where " & LedCondt & IIf(Trim(LedCondt) <> "", " and ", "") & " a.Sizing_Specification_Code = '" & Trim(NewCode) & "' order by a.Pavu_Delivery_Date, a.for_orderby, a.Pavu_Delivery_No", con)
                'Da = New SqlClient.SqlDataAdapter("Select a.*, c.Ledger_Name, d.EndsCount_Name, (select top 1 sq3.Set_No from SizSoft_Pavu_Delivery_Details sq3 Where sq3.Pavu_Delivery_Code = a.Pavu_Delivery_Code Order by sq3.Sl_no) as Pavu_SetNo from SizSoft_Pavu_Delivery_Head a INNER JOIN Ledger_Head c ON a.Ledger_Idno = c.Ledger_Idno LEFT OUTER JOIN EndsCount_Head d ON d.EndsCount_IdNo = (select sq2.EndsCount_IdNo from SizSoft_Pavu_Delivery_Details sq1 INNER JOIN EndsCount_Head sq2 ON sq2.Ends_Name = sq1.Ends_Name and sq2.Count_IdNo = sq1.Count_IdNo Where sq1.Pavu_Delivery_Code = a.Pavu_Delivery_Code ) where " & LedCondt & IIf(Trim(LedCondt) <> "", " and ", "") & " a.Sizing_Specification_Code = '" & Trim(NewCode) & "' order by a.Pavu_Delivery_Date, a.for_orderby, a.Pavu_Delivery_No", con)
                Dt1 = New DataTable
                Da.Fill(Dt1)

                If Dt1.Rows.Count > 0 Then

                    For i = 0 To Dt1.Rows.Count - 1

                        n = .Rows.Add()

                        SNo = SNo + 1
                        .Rows(n).Cells(0).Value = Val(SNo)

                        .Rows(n).Cells(1).Value = Dt1.Rows(i).Item("Pavu_Delivery_No").ToString
                        .Rows(n).Cells(2).Value = Format(Convert.ToDateTime(Dt1.Rows(i).Item("Pavu_Delivery_Date").ToString), "dd-MM-yyyy")
                        .Rows(n).Cells(3).Value = Dt1.Rows(i).Item("First_SetNo").ToString
                        .Rows(n).Cells(4).Value = Dt1.Rows(i).Item("Ledger_Name").ToString
                        .Rows(n).Cells(5).Value = Dt1.Rows(i).Item("EndsCount_Name").ToString
                        .Rows(n).Cells(6).Value = Dt1.Rows(i).Item("Total_Beam").ToString
                        .Rows(n).Cells(7).Value = Dt1.Rows(i).Item("Total_Meters").ToString
                        .Rows(n).Cells(8).Value = "1"
                        .Rows(n).Cells(9).Value = Dt1.Rows(i).Item("Pavu_Delivery_Code").ToString
                        .Rows(n).Cells(10).Value = ""
                        .Rows(n).Cells(11).Value = ""

                        For j = 0 To .ColumnCount - 1
                            .Rows(i).Cells(j).Style.ForeColor = Color.Red
                        Next

                    Next

                End If
                Dt1.Clear()

                Da = New SqlClient.SqlDataAdapter("Select a.*, c.Ledger_Name, d.EndsCount_Name from SizSoft_Pavu_Delivery_Head a INNER JOIN Ledger_Head c ON a.Sizing_Idno = c.Ledger_Idno LEFT OUTER JOIN EndsCount_Head d ON d.EndsCount_IdNo = a.First_EndsCount_IdNo Where " & LedCondt & IIf(Trim(LedCondt) <> "", " and ", "") & " ISNULL(a.Sizing_Specification_Code, '') = '' order by a.Pavu_Delivery_Date, a.for_orderby, a.Pavu_Delivery_No", con)
                'Da = New SqlClient.SqlDataAdapter("Select a.*, c.Ledger_Name, d.EndsCount_Name, (select top 1 sq3.Set_No from SizSoft_Pavu_Delivery_Details sq3 Where sq3.Pavu_Delivery_Code = a.Pavu_Delivery_Code Order by sq3.Sl_no) as Pavu_SetNo from SizSoft_Pavu_Delivery_Head a INNER JOIN Ledger_Head c ON a.Ledger_Idno = c.Ledger_Idno LEFT OUTER JOIN EndsCount_Head d ON d.EndsCount_IdNo = (select sq2.EndsCount_IdNo from SizSoft_Pavu_Delivery_Details sq1 INNER JOIN EndsCount_Head sq2 ON sq2.Ends_Name = sq1.Ends_Name and sq2.Count_IdNo = sq1.Count_IdNo Where sq1.Pavu_Delivery_Code = a.Pavu_Delivery_Code ) where " & LedCondt & IIf(Trim(LedCondt) <> "", " and ", "") & " ISNULL(a.Sizing_Specification_Code, '') = '' order by a.Pavu_Delivery_Date, a.for_orderby, a.Pavu_Delivery_No", con)
                Dt1 = New DataTable
                Da.Fill(Dt1)

                If Dt1.Rows.Count > 0 Then

                    For i = 0 To Dt1.Rows.Count - 1

                        n = .Rows.Add()

                        SNo = SNo + 1
                        .Rows(n).Cells(0).Value = Val(SNo)

                        .Rows(n).Cells(1).Value = Dt1.Rows(i).Item("Pavu_Delivery_No").ToString
                        .Rows(n).Cells(2).Value = Format(Convert.ToDateTime(Dt1.Rows(i).Item("Pavu_Delivery_Date").ToString), "dd-MM-yyyy")
                        .Rows(n).Cells(3).Value = Dt1.Rows(i).Item("First_SetNo").ToString
                        .Rows(n).Cells(4).Value = Dt1.Rows(i).Item("Ledger_Name").ToString
                        .Rows(n).Cells(5).Value = Dt1.Rows(i).Item("EndsCount_Name").ToString
                        .Rows(n).Cells(6).Value = Dt1.Rows(i).Item("Total_Beam").ToString
                        .Rows(n).Cells(7).Value = Dt1.Rows(i).Item("Total_Meters").ToString
                        .Rows(n).Cells(8).Value = ""
                        .Rows(n).Cells(9).Value = Dt1.Rows(i).Item("Pavu_Delivery_Code").ToString
                        .Rows(n).Cells(10).Value = ""
                        .Rows(n).Cells(11).Value = ""

                    Next

                End If
                Dt1.Clear()

                pnl_Selection.Visible = True
                pnl_Back.Enabled = False
                dgv_Selection.Focus()

            End With



        ElseIf Trim(UCase(cbo_Type.Text)) = "DIRECT" And Common_Procedures.settings.Internal_Order_Entry_Status = 1 Then

            'With dgv_OwnOrderSelection
            '    If Val(LedIdNo) <> 0 Then

            '        .Rows.Clear()
            '        SNo = 0

            '        Da = New SqlClient.SqlDataAdapter("select a.*, b.Cloth_Name,c.*  from Own_Order_Head a INNER JOIN CLOTH_Head b ON  b.Cloth_IdNo = a.Cloth_Idno  INNER JOIN Own_Order_Sizing_Details c ON  c.Own_Order_Code = a.Own_Order_Code LEFT OUTER JOIN Sizing_Specification_Head d ON d.Own_Order_Code = a.Own_Order_Code  where a.Sizing_Specification_Code = '" & Trim(NewCode) & "'   and c.Ledger_IdNo = " & Val(LedIdNo) & " order by a.Own_Order_Date, a.for_orderby, a.Own_Order_No", con)
            '        Dt1 = New DataTable
            '        Da.Fill(Dt1)

            '        If Dt1.Rows.Count > 0 Then

            '            For i = 0 To Dt1.Rows.Count - 1

            '                n = .Rows.Add()

            '                Ent_Rate = 0


            '                SNo = SNo + 1
            '                .Rows(n).Cells(0).Value = Val(SNo)

            '                .Rows(n).Cells(1).Value = Dt1.Rows(i).Item("Own_Order_No").ToString
            '                .Rows(n).Cells(2).Value = Format(Convert.ToDateTime(Dt1.Rows(i).Item("Own_Order_Date").ToString), "dd-MM-yyyy")
            '                .Rows(n).Cells(3).Value = Dt1.Rows(i).Item("Order_No").ToString


            '                .Rows(n).Cells(4).Value = Dt1.Rows(i).Item("Cloth_Name").ToString

            '                .Rows(n).Cells(5).Value = Format(Val(Dt1.Rows(i).Item("Order_Meters").ToString), "#########0.000")
            '                .Rows(n).Cells(6).Value = "1"
            '                .Rows(n).Cells(7).Value = Dt1.Rows(i).Item("Own_Order_Code").ToString

            '                For j = 0 To .ColumnCount - 1
            '                    .Rows(i).Cells(j).Style.ForeColor = Color.Red
            '                Next

            '            Next

            '        End If
            '        Dt1.Clear()

            '        Da = New SqlClient.SqlDataAdapter("select a.*, b.Cloth_Name from Own_Order_Head a INNER JOIN Cloth_Head b ON  b.Cloth_Idno = a.Cloth_Idno INNER JOIN Own_Order_Sizing_Details c ON  c.Own_Order_Code = a.Own_Order_Code   LEFT OUTER JOIN Sizing_Specification_Head d ON d.Sizing_Specification_Code = a.Own_order_Code    where a.Sizing_Specification_Code = ''  and c.Ledger_IdNo = " & Val(LedIdNo) & " order by a.Own_Order_Date, a.for_orderby, a.Own_Order_No", con)
            '        Dt1 = New DataTable
            '        Da.Fill(Dt1)

            '        If Dt1.Rows.Count > 0 Then

            '            For i = 0 To Dt1.Rows.Count - 1

            '                n = .Rows.Add()

            '                SNo = SNo + 1
            '                .Rows(n).Cells(0).Value = Val(SNo)
            '                .Rows(n).Cells(1).Value = Dt1.Rows(i).Item("OWn_Order_No").ToString
            '                .Rows(n).Cells(2).Value = Format(Convert.ToDateTime(Dt1.Rows(i).Item("Own_Order_Date").ToString), "dd-MM-yyyy")
            '                .Rows(n).Cells(3).Value = Dt1.Rows(i).Item("Order_No").ToString


            '                .Rows(n).Cells(4).Value = Dt1.Rows(i).Item("Cloth_Name").ToString

            '                .Rows(n).Cells(5).Value = Format(Val(Dt1.Rows(i).Item("Order_Meters").ToString), "#########0.000")
            '                .Rows(n).Cells(6).Value = ""
            '                .Rows(n).Cells(7).Value = Dt1.Rows(i).Item("Own_Order_Code").ToString

            '            Next

            '        End If
            '        Dt1.Clear()
            '    Else
            '        .Rows.Clear()
            '        SNo = 0

            '        Da = New SqlClient.SqlDataAdapter("select a.*, b.Cloth_Name  from Own_Order_Head a INNER JOIN CLOTH_Head b ON  b.Cloth_IdNo = a.Cloth_Idno   LEFT OUTER JOIN Sizing_Specification_Head d ON d.Own_Order_Code = a.Own_Order_Code  where a.Sizing_Specification_Code = '" & Trim(NewCode) & "'   order by a.Own_Order_Date, a.for_orderby, a.Own_Order_No", con)
            '        Dt1 = New DataTable
            '        Da.Fill(Dt1)

            '        If Dt1.Rows.Count > 0 Then

            '            For i = 0 To Dt1.Rows.Count - 1

            '                n = .Rows.Add()

            '                Ent_Rate = 0


            '                SNo = SNo + 1
            '                .Rows(n).Cells(0).Value = Val(SNo)

            '                .Rows(n).Cells(1).Value = Dt1.Rows(i).Item("Own_Order_No").ToString
            '                .Rows(n).Cells(2).Value = Format(Convert.ToDateTime(Dt1.Rows(i).Item("Own_Order_Date").ToString), "dd-MM-yyyy")
            '                .Rows(n).Cells(3).Value = Dt1.Rows(i).Item("Order_No").ToString

            '                .Rows(n).Cells(4).Value = Dt1.Rows(i).Item("Cloth_Name").ToString

            '                .Rows(n).Cells(5).Value = Format(Val(Dt1.Rows(i).Item("Order_Meters").ToString), "#########0.000")
            '                .Rows(n).Cells(6).Value = "1"
            '                .Rows(n).Cells(7).Value = Dt1.Rows(i).Item("Own_Order_Code").ToString

            '                For j = 0 To .ColumnCount - 1
            '                    .Rows(i).Cells(j).Style.ForeColor = Color.Red
            '                Next

            '            Next

            '        End If
            '        Dt1.Clear()

            '        Da = New SqlClient.SqlDataAdapter("select a.*, b.Cloth_Name from Own_Order_Head a INNER JOIN Cloth_Head b ON  b.Cloth_Idno = a.Cloth_Idno    LEFT OUTER JOIN Sizing_Specification_Head d ON d.Sizing_Specification_Code = a.Own_Order_Code    where a.Sizing_Specification_Code = ''   order by a.Own_Order_Date, a.for_orderby, a.Own_Order_No", con)
            '        Dt1 = New DataTable
            '        Da.Fill(Dt1)

            '        If Dt1.Rows.Count > 0 Then

            '            For i = 0 To Dt1.Rows.Count - 1

            '                n = .Rows.Add()

            '                SNo = SNo + 1
            '                .Rows(n).Cells(0).Value = Val(SNo)
            '                .Rows(n).Cells(1).Value = Dt1.Rows(i).Item("Own_Order_No").ToString
            '                .Rows(n).Cells(2).Value = Format(Convert.ToDateTime(Dt1.Rows(i).Item("Own_Order_Date").ToString), "dd-MM-yyyy")
            '                .Rows(n).Cells(3).Value = Dt1.Rows(i).Item("Order_No").ToString

            '                .Rows(n).Cells(4).Value = Dt1.Rows(i).Item("Cloth_Name").ToString

            '                .Rows(n).Cells(5).Value = Format(Val(Dt1.Rows(i).Item("Order_Meters").ToString), "#########0.000")
            '                .Rows(n).Cells(6).Value = ""
            '                .Rows(n).Cells(7).Value = Dt1.Rows(i).Item("Own_Order_Code").ToString

            '            Next

            '        End If
            '        Dt1.Clear()
            '    End If

            '    pnl_OwnOrderSelection.Visible = True
            '    pnl_Back.Enabled = False
            '    dgv_OwnOrderSelection.Focus()
            'End With

        End If
    End Sub

    Private Sub dgv_Selection_CellClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Selection.CellClick
        Select_PavuReceipt(e.RowIndex)
    End Sub

    Private Sub Select_PavuReceipt(ByVal RwIndx As Integer)

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

                Select_PavuReceipt(n)

                e.Handled = True

            End If
        End If

    End Sub

    Private Sub btn_Close_Selection_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Close_Selection.Click
        Close_PavuReceipt_Selection()
    End Sub

    Private Sub Close_PavuReceipt_Selection()
        Dim Da1 As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim n As Integer = 0
        Dim sno As Integer = 0
        Dim i As Integer = 0
        Dim j As Integer = 0
        Dim Set_No As String = ""
        Dim Cnt_ID = 0
        Dim vFirst_EdsCntID = 0
        Dim vEdsCnt_Name = ""

        If New_Entry = False Then
            If Trim(UCase(cbo_Type.Text)) <> "RECEIPT" And Trim(UCase(cbo_Type.Text)) <> "SIZING-UNIT PAVU DELIVERY" Then
                MessageBox.Show("Invalid Entry Type", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                If cbo_Type.Enabled And cbo_Type.Visible Then cbo_Type.Focus()
                Exit Sub
            End If
        End If


        dgv_PavuDetails.Rows.Clear()

        Set_No = ""

        For i = 0 To dgv_Selection.RowCount - 1

            If Val(dgv_Selection.Rows(i).Cells(8).Value) = 1 Then

                If Trim(Set_No) <> "" Then
                    If Trim(UCase(Set_No)) <> Trim(UCase(dgv_Selection.Rows(i).Cells(3).Value)) Then
                        MessageBox.Show("Select Same SetNo", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                        If dgv_Selection.Enabled And dgv_Selection.Visible Then
                            dgv_Selection.Focus()
                            dgv_Selection.CurrentCell.Selected = True
                        End If
                        Exit Sub
                    End If
                End If

                Set_No = Trim(dgv_Selection.Rows(i).Cells(3).Value)

            End If

        Next

        pnl_Back.Enabled = True

        MovSTS = True
        For i = 0 To dgv_Selection.RowCount - 1

            If Val(dgv_Selection.Rows(i).Cells(8).Value) = 1 Then

                If Trim(UCase(cbo_Type.Text)) = "RECEIPT" Then

                    Da1 = New SqlClient.SqlDataAdapter("select a.*,b.EndsCount_name from Sizing_Pavu_Receipt_Details a Inner Join EndsCount_head b on  a.EndsCount_IdNo = b.EndsCount_IdNo where a.Sizing_Pavu_Receipt_Code  = '" & Trim(dgv_Selection.Rows(i).Cells(9).Value) & "' order by a.sl_no", con)
                    'Da1 = New SqlClient.SqlDataAdapter("select a.* from Sizing_Pavu_Receipt_Details a where a.Sizing_Pavu_Receipt_Code  = '" & Trim(dgv_Selection.Rows(i).Cells(9).Value) & "' order by a.sl_no", con)
                    Dt1 = New DataTable
                    Da1.Fill(Dt1)

                    cbo_Type.Text = "RECEIPT"
                    txt_SetNo.Text = dgv_Selection.Rows(i).Cells(3).Value
                    cbo_Sizing.Text = dgv_Selection.Rows(i).Cells(4).Value
                    cbo_EndsCount.Text = dgv_Selection.Rows(i).Cells(5).Value
                    lbl_OrderNo.Text = dgv_Selection.Rows(i).Cells(10).Value
                    lbl_OrderCode.Text = dgv_Selection.Rows(i).Cells(11).Value
                    lbl_SetCode.Text = ""

                    If Dt1.Rows.Count > 0 Then

                        lbl_SetCode.Text = Dt1.Rows(0).Item("set_code").ToString
                        txt_PcsLength.Text = Val(Dt1.Rows(0).Item("Meters_Pc").ToString)

                        For j = 0 To Dt1.Rows.Count - 1

                            n = dgv_PavuDetails.Rows.Add()

                            sno = sno + 1
                            dgv_PavuDetails.Rows(n).Cells(dgvCol_Details.Slno).Value = Val(sno)
                            dgv_PavuDetails.Rows(n).Cells(dgvCol_Details.Beam_No).Value = Dt1.Rows(j).Item("Beam_No").ToString
                            dgv_PavuDetails.Rows(n).Cells(dgvCol_Details.Ends_Count).Value = Dt1.Rows(j).Item("EndsCount_name").ToString
                            dgv_PavuDetails.Rows(n).Cells(dgvCol_Details.Pcs).Value = Dt1.Rows(j).Item("Pcs").ToString
                            dgv_PavuDetails.Rows(n).Cells(dgvCol_Details.meters).Value = Dt1.Rows(j).Item("Meters").ToString
                            dgv_PavuDetails.Rows(n).Cells(dgvCol_Details.Sts).Value = ""
                            dgv_PavuDetails.Rows(n).Cells(dgvCol_Details.pavu_receipt_code).Value = Dt1.Rows(j).Item("Sizing_Pavu_Receipt_Code").ToString

                            dgv_PavuDetails.Rows(n).Cells(dgvCol_Details.beam_width).Value = Common_Procedures.BeamWidth_IdNoToName(con, Val(Dt1.Rows(j).Item("Beam_Width_IdNo").ToString))
                            dgv_PavuDetails.Rows(n).Cells(dgvCol_Details.Beam_Type).Value = Common_Procedures.LoomType_IdNoToName(con, Val(Dt1.Rows(j).Item("LoomType_Idno").ToString))


                        Next

                    End If
                    Dt1.Clear()


                ElseIf Trim(UCase(cbo_Type.Text)) = "SIZING-UNIT PAVU DELIVERY" Then


                    Da1 = New SqlClient.SqlDataAdapter("select a.* from SizSoft_Pavu_Delivery_Details a Inner Join EndsCount_head b on  a.EndsCount_IdNo = b.EndsCount_IdNo  where a.Pavu_Delivery_Code  = '" & Trim(dgv_Selection.Rows(i).Cells(9).Value) & "' order by a.sl_no", con)
                    'Da1 = New SqlClient.SqlDataAdapter("select a.* from SizSoft_Pavu_Delivery_Details a where a.Pavu_Delivery_Code  = '" & Trim(dgv_Selection.Rows(i).Cells(9).Value) & "' order by a.sl_no", con)
                    Dt1 = New DataTable
                    Da1.Fill(Dt1)

                    cbo_Type.Text = "SIZING-UNIT PAVU DELIVERY"
                    txt_SetNo.Text = dgv_Selection.Rows(i).Cells(3).Value
                    cbo_Sizing.Text = dgv_Selection.Rows(i).Cells(4).Value
                    cbo_EndsCount.Text = dgv_Selection.Rows(i).Cells(5).Value
                    lbl_OrderNo.Text = dgv_Selection.Rows(i).Cells(10).Value
                    lbl_OrderCode.Text = dgv_Selection.Rows(i).Cells(11).Value
                    lbl_SetCode.Text = ""

                    If Dt1.Rows.Count > 0 Then

                        txt_PcsLength.Text = Val(Dt1.Rows(0).Item("Meters_Pc").ToString)
                        lbl_SetCode.Text = Dt1.Rows(0).Item("set_code").ToString

                        For j = 0 To Dt1.Rows.Count - 1

                            n = dgv_PavuDetails.Rows.Add()

                            sno = sno + 1
                            dgv_PavuDetails.Rows(n).Cells(dgvCol_Details.Slno).Value = Val(sno)
                            dgv_PavuDetails.Rows(n).Cells(dgvCol_Details.Beam_No).Value = Dt1.Rows(j).Item("Beam_No").ToString


                            vFirst_EdsCntID = Common_Procedures.get_FieldValue(con, "EndsCount_Head", "EndsCount_IdNo", "(Ends_Name = '" & Trim(Dt1.Rows(j).Item("Ends_Name").ToString) & "' and Count_IdNo = " & Str(Val(Dt1.Rows(j).Item("Count_IdNo").ToString)) & ")")
                            vEdsCnt_Name = Common_Procedures.get_FieldValue(con, "EndsCount_Head", "EndsCount_Name", "(EndsCount_IdNo = " & Str(Val(vFirst_EdsCntID)) & ")")

                            dgv_PavuDetails.Rows(n).Cells(dgvCol_Details.Ends_Count).Value = Trim(vEdsCnt_Name)

                            dgv_PavuDetails.Rows(n).Cells(dgvCol_Details.Pcs).Value = Dt1.Rows(j).Item("Noof_Pcs").ToString
                            'dgv_PavuDetails.Rows(n).Cells(dgvCol_Details.Pcs).Value = Dt1.Rows(j).Item("Pcs").ToString
                            dgv_PavuDetails.Rows(n).Cells(dgvCol_Details.meters).Value = Dt1.Rows(j).Item("Meters").ToString
                            dgv_PavuDetails.Rows(n).Cells(dgvCol_Details.Sts).Value = ""
                            'dgv_PavuDetails.Rows(n).Cells(dgvCol_Details.pavu_receipt_code).Value = Dt1.Rows(j).Item("Sizing_Pavu_Receipt_Code").ToString
                            dgv_PavuDetails.Rows(n).Cells(dgvCol_Details.pavu_receipt_code).Value = Dt1.Rows(j).Item("Pavu_Delivery_Code").ToString
                        Next

                    End If
                    Dt1.Clear()

                End If




            End If
            Dt1.Clear()

        Next

        MovSTS = False
        TotalPavu_Calculation()

        pnl_Back.Enabled = True
        pnl_Selection.Visible = False
        If txt_SetNo.Enabled And txt_SetNo.Visible Then
            txt_SetNo.Focus()
        ElseIf txt_TotalBeams.Enabled And txt_TotalBeams.Visible Then
            txt_TotalBeams.Focus()
        Else
            cbo_YarnStock.Focus()
        End If
        'If dgv_PavuDetails.Enabled And dgv_PavuDetails.Visible Then
        '    If dgv_PavuDetails.Rows.Count > 0 Then
        '        btn_save.Focus()
        '    Else
        '        dtp_Date.Focus()
        '    End If
        'End If

    End Sub

    Private Sub txt_WarpMtr_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_WarpMtr.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub txt_WarpMtr_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_WarpMtr.TextChanged
        Elogation_Calculation()
        AverageCount_Calculation()
        PickUp_Calculation()
        NetAmount_Calculation()
    End Sub

    Private Sub txt_TapeLength_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_TapeLength.TextChanged
        Elogation_Calculation()
        PickUp_Calculation()

    End Sub

    Private Sub txt_ConsumedYarn_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_ConsumedYarn.TextChanged
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1018" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1029" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1464" Then '---- M.K Textiles (Palladam)            Excess_Calculation()
            Excess_Calculation()
        End If
        AverageCount_Calculation()
        PickUp_Calculation()
        NetAmount_Calculation()
    End Sub

    Private Sub cbo_EndsCount_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_EndsCount.TextChanged
        AverageCount_Calculation()
    End Sub

    Private Sub txt_AddLess_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_AddLess.TextChanged
        NetAmount_Calculation()
    End Sub

    Private Sub txt_PackingRate_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_PackingRate.TextChanged

        NetAmount_Calculation()
    End Sub

    Private Sub txt_WindingRate_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_WindingRate.TextChanged
        NetAmount_Calculation()
    End Sub

    Private Sub txt_InvRate_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_InvRate.TextChanged
        NetAmount_Calculation()
    End Sub

    Private Sub cbo_YarnStock_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_YarnStock.TextChanged

        If Trim(UCase(cbo_YarnStock.Text)) = "CONSUMED YARN" Then
            txt_ConsumedYarn.Enabled = False
            txt_YarnTaken.Enabled = True
        Else
            txt_ConsumedYarn.Enabled = True
            txt_YarnTaken.Enabled = False
        End If
        If dgv_YarnDetails_Total.RowCount > 1 Then
            If Trim(UCase(cbo_YarnStock.Text)) = "CONSUMED YARN" Then
                If Val(dgv_YarnDetails_Total.Rows(0).Cells(6).Value) <> 0 Then
                    txt_ConsumedYarn.Text = Format(Val(dgv_YarnDetails_Total.Rows(0).Cells(6).Value), "########0.000")
                End If
            Else
                If Val(dgv_YarnDetails_Total.Rows(0).Cells(6).Value) <> 0 Then
                    txt_YarnTaken.Text = Format(Val(dgv_YarnDetails_Total.Rows(0).Cells(6).Value), "########0.000")
                End If
            End If
        End If
        TotalYarnTaken_Calculation()

    End Sub

    Private Sub txt_RwCns_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_RwCns.TextChanged
        NetAmount_Calculation()
    End Sub

    Private Sub txt_BabyWt_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_BabyWt.TextChanged
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1018" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1029" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1464" Then '---- M.K Textiles (Palladam)
            Excess_Calculation()
        End If
        NetAmount_Calculation()
    End Sub

    Private Sub cbo_YarnStock_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_YarnStock.KeyDown

        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_YarnStock, Nothing, Nothing, "", "", "", "")
        'Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_YarnStock, txt_PcsLength, txt_TotalBeams, "", "", "", "")


        Try
            With txt_TotalBeams
                If (e.KeyValue = 38 And sender.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
                    e.Handled = True
                    e.SuppressKeyPress = True

                    If txt_PcsLength.Enabled And txt_PcsLength.Visible Then
                        txt_PcsLength.Focus()

                    Else
                        msk_Date.Focus()
                    End If


                ElseIf (e.KeyValue = 40 And sender.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
                    e.Handled = True
                    e.SuppressKeyPress = True

                    If cbo_jobcardno.Visible And cbo_jobcardno.Enabled Then
                        cbo_jobcardno.Focus()

                    ElseIf txt_TotalBeams.Visible And txt_TotalBeams.Enabled Then
                        txt_TotalBeams.Focus()

                    ElseIf Trim(UCase(cbo_Type.Text)) = "RECEIPT" Or Trim(UCase(cbo_Type.Text)) = "SIZING-UNIT PAVU DELIVERY" Then
                        dgv_YarnDetails.Focus()
                        dgv_YarnDetails.CurrentCell = dgv_YarnDetails.Rows(0).Cells(1)

                    Else
                        dgv_PavuDetails.Focus()
                        dgv_PavuDetails.CurrentCell = dgv_PavuDetails.Rows(0).Cells(dgvCol_Details.Beam_No)
                        dgv_PavuDetails.CurrentCell.Selected = True


                    End If

                End If
            End With

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT SELECT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub cbo_YarnStock_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_YarnStock.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_YarnStock, Nothing, "", "", "", "", False)

        Try
            With txt_TotalBeams
                If Asc(e.KeyChar) = 13 Then
                    If cbo_jobcardno.Visible And cbo_jobcardno.Enabled Then
                        cbo_jobcardno.Focus()

                    ElseIf txt_TotalBeams.Visible And txt_TotalBeams.Enabled Then
                        txt_TotalBeams.Focus()

                    ElseIf Trim(UCase(cbo_Type.Text)) = "RECEIPT" Or Trim(UCase(cbo_Type.Text)) = "SIZING-UNIT PAVU DELIVERY" Then
                        dgv_YarnDetails.Focus()
                        dgv_YarnDetails.CurrentCell = dgv_YarnDetails.Rows(0).Cells(1)

                    Else

                        dgv_PavuDetails.Focus()
                        dgv_PavuDetails.CurrentCell = dgv_PavuDetails.Rows(0).Cells(dgvCol_Details.Beam_No)
                        dgv_PavuDetails.CurrentCell.Selected = True


                    End If

                End If
            End With

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT SELECT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub txt_TotalBeams_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_TotalBeams.KeyDown
        'Try
        '    With txt_TotalBeams
        If e.KeyValue = 40 Then
            If Trim(UCase(cbo_Type.Text)) = "DIRECT" Then

                dgv_PavuDetails.Focus()
                dgv_PavuDetails.CurrentCell = dgv_PavuDetails.Rows(0).Cells(1)
                dgv_PavuDetails.CurrentCell.Selected = True


            Else

                dgv_YarnDetails.Focus()
                dgv_YarnDetails.CurrentCell = dgv_YarnDetails.Rows(0).Cells(1)

            End If

        End If
        If e.KeyValue = 38 Then
            If cbo_jobcardno.Visible And cbo_jobcardno.Enabled Then
                cbo_jobcardno.Focus()
            Else
                cbo_YarnStock.Focus()

            End If
        End If
        '    End With

        'Catch ex As Exception
        '    MessageBox.Show(ex.Message, "DOES NOT SELECT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        'End Try
    End Sub

    Private Sub txt_TotalBeams_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_TotalBeams.KeyPress
        If Asc(e.KeyChar) = 13 Then
            If Trim(UCase(cbo_Type.Text)) = "DIRECT" Then
                If dgv_PavuDetails.Rows.Count > 0 Then
                    dgv_PavuDetails.Focus()
                    dgv_PavuDetails.CurrentCell = dgv_PavuDetails.Rows(0).Cells(1)
                    dgv_PavuDetails.CurrentCell.Selected = True
                Else
                    txt_WarpMtr.Focus()
                End If

            Else
                If dgv_YarnDetails.Rows.Count > 0 Then
                    dgv_YarnDetails.Focus()
                    dgv_YarnDetails.CurrentCell = dgv_YarnDetails.Rows(0).Cells(1)

                Else
                    txt_WarpMtr.Focus()
                End If
            End If

        End If
    End Sub

    Private Sub cbo_BeamCount_Type_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_BeamCount_Type.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_BeamCount_Type, txt_WarpMtr, txt_ConsumedYarn, "", "", "", "")

    End Sub

    Private Sub cbo_BeamCount_Type_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_BeamCount_Type.KeyPress

        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_BeamCount_Type, txt_ConsumedYarn, "", "", "", "", False)
        Elogation_Calculation()
        AverageCount_Calculation()
        PickUp_Calculation()
    End Sub

    Private Sub txt_YarnTaken_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_YarnTaken.TextChanged
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1018" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1029" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1464" Then '---- M.K Textiles (Palladam)
            Excess_Calculation()
        End If
    End Sub

    Private Sub btn_BeamClose_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_BeamClose.Click
        pnl_Back.Enabled = True
        pnl_BeamDetails.Visible = False
    End Sub

    Private Sub btn_BeamDetail_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_BeamDetail.Click
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim n As Integer, i As Integer
        Dim SNo As Integer
        Try

            With dgv_BeamDetails

                .Rows.Clear()
                SNo = 0

                da = New SqlClient.SqlDataAdapter("select a.*, c.Ledger_Name ,  d.EndsCount_Name from Sizing_Specification_Head a INNER JOIN Ledger_Head c ON c.Ledger_Idno  = A.Ledger_Idno INNER JOIN EndsCount_Head d ON a.EndsCount_IdNo = d.EndsCount_IdNo where a.Total_PlanBeams <> a.Total_Beam order by a.Sizing_Specification_Date, a.for_orderby, a.Sizing_Specification_No", con)
                dt1 = New DataTable
                da.Fill(dt1)

                If dt1.Rows.Count > 0 Then

                    For i = 0 To dt1.Rows.Count - 1

                        n = .Rows.Add()

                        SNo = SNo + 1
                        .Rows(n).Cells(0).Value = Val(SNo)

                        .Rows(n).Cells(1).Value = dt1.Rows(i).Item("Sizing_Specification_No").ToString
                        .Rows(n).Cells(2).Value = Format(Convert.ToDateTime(dt1.Rows(i).Item("Sizing_Specification_Date").ToString), "dd-MM-yyyy")
                        .Rows(n).Cells(3).Value = dt1.Rows(i).Item("Set_No").ToString
                        .Rows(n).Cells(4).Value = dt1.Rows(i).Item("Ledger_Name").ToString
                        .Rows(n).Cells(5).Value = dt1.Rows(i).Item("EndsCount_Name").ToString
                        .Rows(n).Cells(6).Value = dt1.Rows(i).Item("Total_PlanBeams").ToString
                        .Rows(n).Cells(7).Value = dt1.Rows(i).Item("Total_Beam").ToString
                        .Rows(n).Cells(8).Value = Val(dt1.Rows(i).Item("Total_PlanBeams").ToString - Val(dt1.Rows(i).Item("Total_Beam").ToString))

                    Next

                End If
                dt1.Clear()

            End With
            dt2.Clear()
            dt2.Dispose()
            da.Dispose()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT FILTER...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

        pnl_BeamDetails.Visible = True
        pnl_Back.Enabled = False
        dgv_BeamDetails.Focus()
    End Sub


    Private Sub dgv_BeamDetails_CellDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_BeamDetails.CellDoubleClick
        Open_BeamReceiptEntry()
    End Sub

    Private Sub dgv_BeamDetails_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_BeamDetails.KeyDown
        If e.KeyCode = 13 Then
            Open_BeamReceiptEntry()
        End If
    End Sub

    Private Sub cbo_Type_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Type.LostFocus
        If Trim(cbo_Type.Text) = "" Or (Trim(UCase(cbo_Type.Text)) <> "" And Trim(UCase(cbo_Type.Text)) <> "DIRECT" And Trim(UCase(cbo_Type.Text)) <> "RECEIPT" And Trim(UCase(cbo_Type.Text)) <> "SIZING-UNIT PAVU DELIVERY") Then
            cbo_Type.Text = "DIRECT"
        End If
    End Sub

    Private Sub cbo_Type_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Type.TextChanged
        txt_SetNo.Enabled = True
        cbo_EndsCount.Enabled = True
        cbo_Grid_EndsCount.Enabled = True
        txt_PcsLength.Enabled = True
        If Trim(UCase(cbo_Type.Text)) = "RECEIPT" Or Trim(UCase(cbo_Type.Text)) = "SIZING-UNIT PAVU DELIVERY" Then
            txt_SetNo.Enabled = False
            cbo_EndsCount.Enabled = False
            cbo_Grid_EndsCount.Enabled = False
            txt_PcsLength.Enabled = False
        End If
    End Sub
    Private Sub cbo_Grid_BeamWidth_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Grid_BeamWidth.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Beam_Width_Head", "Beam_Width_Name", "", "(Beam_Width_IdNo = 0)")
    End Sub

    Private Sub cbo_Grid_BeamWidth_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Grid_BeamWidth.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Grid_BeamWidth, Nothing, Nothing, "Beam_Width_Head", "Beam_Width_Name", "", "(Beam_Width_IdNo = 0)")
        vcbo_KeyDwnVal = e.KeyValue

        With dgv_PavuDetails

            If (e.KeyValue = 38 And cbo_Grid_BeamWidth.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
                .Focus()
                .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex - 1)
            End If

            If (e.KeyValue = 40 And cbo_Grid_BeamWidth.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
                .Focus()
                .CurrentCell = .Rows(.CurrentRow.Index).Cells(dgvCol_Details.Beam_Type)
                '.CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 4)
            End If

        End With

    End Sub

    Private Sub cbo_Grid_BeamWidth_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Grid_BeamWidth.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Grid_BeamWidth, Nothing, "Beam_Width_Head", "Beam_Width_Name", "", "(Beam_Width_IdNo = 0)")

        If Asc(e.KeyChar) = 13 Then

            With dgv_PavuDetails
                e.Handled = True
                .Focus()
                .CurrentCell = .Rows(.CurrentRow.Index).Cells(dgvCol_Details.Beam_Type)

            End With

        End If

    End Sub

    Private Sub cbo_Grid_BeamWidth_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Grid_BeamWidth.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Dim f As New Beam_Width_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Grid_BeamWidth.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()
        End If
    End Sub
    Private Sub cbo_Grid_BeamWidth_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Grid_BeamWidth.TextChanged
        If IsNothing(dgv_PavuDetails.CurrentCell) Then Exit Sub

        Try
            If cbo_Grid_BeamWidth.Visible Then
                With dgv_PavuDetails
                    If Val(cbo_Grid_BeamWidth.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = dgvCol_Details.beam_width Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_Grid_BeamWidth.Text)
                    End If
                End With
            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub dtp_InvoiceDate_KeyDown(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles dtp_InvoiceDate.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
    End Sub

    Private Sub dtp_InvoiceDate_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles dtp_InvoiceDate.ValueChanged
        msk_InvoiceDate.Text = dtp_InvoiceDate.Text
    End Sub

    Private Sub dtp_InvoiceDate_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles dtp_InvoiceDate.Enter
        'msk_InvoiceDate.Focus()
        'msk_InvoiceDate.SelectionStart = 0
        msk_InvoiceDate.Focus()
        msk_InvoiceDate.SelectionStart = 0
    End Sub

    Private Sub dtp_Date_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dtp_Date.KeyPress
        If Asc(e.KeyChar) = 13 Then
            e.Handled = True
            msk_Date.Focus()
        End If
    End Sub
    Private Sub dtp_Date_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles dtp_Date.ValueChanged
        msk_Date.Text = dtp_Date.Text
    End Sub

    Private Sub dtp_Date_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles dtp_Date.Enter
        msk_Date.Focus()
        msk_Date.SelectionStart = 0
    End Sub
    Private Sub dtp_Date_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dtp_Date.TextChanged
        If IsDate(dtp_Date.Text) = True Then
            msk_Date.Text = dtp_Date.Text
            msk_Date.SelectionStart = 0
        End If
    End Sub
    Private Sub dtp_Date_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dtp_Date.KeyDown
        If e.KeyCode = 40 Then
            e.Handled = True : e.SuppressKeyPress = True
            msk_Date.Focus()
        End If

        If e.KeyCode = 38 Then
            e.Handled = True : e.SuppressKeyPress = True
            msk_Date.Focus()
        End If
    End Sub

    Private Sub msk_Date_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles msk_Date.KeyPress
        If Trim(UCase(e.KeyChar)) = "D" Then
            msk_Date.Text = Date.Today
            msk_Date.SelectionStart = 0
        End If
        If Asc(e.KeyChar) = 13 Then
            e.Handled = True
            cbo_Type.Focus()
        End If
    End Sub
    Private Sub msk_Date_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles msk_Date.KeyUp
        Dim vmRetTxt As String = ""
        Dim vmRetSelStrt As Integer = -1

        If e.KeyCode = 107 Then
            msk_Date.Text = DateAdd("D", 1, Convert.ToDateTime(msk_Date.Text))
            msk_Date.SelectionStart = 0
        ElseIf e.KeyCode = 109 Then
            msk_Date.Text = DateAdd("D", -1, Convert.ToDateTime(msk_Date.Text))
            msk_Date.SelectionStart = 0
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


    Private Sub msk_InvoiceDate_KeyPress(sender As Object, e As System.Windows.Forms.KeyPressEventArgs) Handles msk_InvoiceDate.KeyPress
        If UCase(Chr(Asc(e.KeyChar))) = "D" Then
            msk_InvoiceDate.Text = Date.Today
            msk_InvoiceDate.SelectionStart = 0
        End If

        If Asc(e.KeyChar) = 13 Then

            If Cbo_RateFor.Visible And Cbo_RateFor.Enabled Then
                Cbo_RateFor.Focus()
            Else
                txt_InvRate.Focus()
            End If

        End If

    End Sub

    Private Sub msk_InvoiceDate_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles msk_InvoiceDate.KeyUp
        Dim vmsRetTxt As String = ""
        Dim vmsRetvl As Integer = -1
        If IsDate(msk_InvoiceDate.Text) = True Then
            If e.KeyCode = 107 Then
                msk_InvoiceDate.Text = DateAdd("D", 1, Convert.ToDateTime(msk_InvoiceDate.Text))
                msk_InvoiceDate.SelectionStart = 0
            ElseIf e.KeyCode = 109 Then
                msk_InvoiceDate.Text = DateAdd("D", -1, Convert.ToDateTime(msk_InvoiceDate.Text))
                msk_InvoiceDate.SelectionStart = 0
            End If
        End If
        If e.KeyCode = 46 Or e.KeyCode = 8 Then
            Common_Procedures.maskEdit_Date_ON_DelBackSpace(sender, e, vmskOldText, vmskSelStrt)
        End If


    End Sub

    Private Sub msk_InvoiceDate_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles msk_InvoiceDate.LostFocus

        If IsDate(msk_InvoiceDate.Text) = True Then
            If Microsoft.VisualBasic.DateAndTime.Day(Convert.ToDateTime(msk_InvoiceDate.Text)) <= 31 Or Microsoft.VisualBasic.DateAndTime.Month(Convert.ToDateTime(msk_InvoiceDate.Text)) <= 31 Then
                If Microsoft.VisualBasic.DateAndTime.Year(Convert.ToDateTime(msk_InvoiceDate.Text)) <= 2050 And Microsoft.VisualBasic.DateAndTime.Year(Convert.ToDateTime(msk_InvoiceDate.Text)) >= 2000 Then
                    dtp_InvoiceDate.Value = Convert.ToDateTime(msk_InvoiceDate.Text)
                End If
            End If

        End If
    End Sub
    Private Sub msk_Date_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles msk_Date.KeyDown
        vcbo_KeyDwnVal = e.KeyValue

        If e.KeyCode = 40 Then
            e.Handled = True : e.SuppressKeyPress = True
            cbo_Type.Focus()
        End If

        If e.KeyCode = 38 Then
            e.Handled = True
        End If

        vmskOldText = ""
        vmskSelStrt = -1
        If e.KeyCode = 46 Or e.KeyCode = 8 Then
            vmskOldText = msk_Date.Text
            vmskSelStrt = msk_Date.SelectionStart
        End If

    End Sub
    Private Sub msk_InvoiceDate_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles msk_InvoiceDate.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        vmskOldText = ""
        vmskSelStrt = -1
        If e.KeyCode = 46 Or e.KeyCode = 8 Then
            vmskOldText = msk_Date.Text
            vmskSelStrt = msk_Date.SelectionStart
        End If
        If e.KeyCode = 38 Then
            txt_InvNo.Focus()
        ElseIf e.KeyCode = 40 Then

            If Cbo_RateFor.Visible And Cbo_RateFor.Enabled Then
                Cbo_RateFor.Focus()
            Else
                txt_InvRate.Focus()
            End If

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
        save_record()
        If Trim(UCase(LastNo)) = Trim(UCase(lbl_RefNo.Text)) Then
            Timer1.Enabled = False
            SaveAll_STS = False
            MessageBox.Show("All entries saved sucessfully", "FOR SAVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Information)

        Else
            movenext_record()

        End If
    End Sub
    Private Sub txt_CGST_Percentage_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_CGST_Percentage.TextChanged
        NetAmount_Calculation()
    End Sub

    Private Sub txt_SGST_Percentage_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_SGST_Percentage.TextChanged
        NetAmount_Calculation()
    End Sub


    Private Sub chk_TaxAmount_RoundOff_STS_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chk_TaxAmount_RoundOff_STS.CheckedChanged
        NetAmount_Calculation()
    End Sub

    Private Sub get_Sizing_TdsPerc()
        Dim da As SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim Led_ID As Integer = 0

        If Trim(UCase(cbo_Sizing.Tag)) <> Trim(UCase(cbo_Sizing.Text)) Then
            cbo_Sizing.Tag = cbo_Sizing.Text

            Led_ID = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Sizing.Text)

            da = New SqlClient.SqlDataAdapter("select a.* from ledger_head a where a.ledger_idno = " & Str(Val(Led_ID)), con)
            dt = New DataTable
            da.Fill(dt)
            If dt.Rows.Count > 0 Then
                If IsDBNull(dt.Rows(0)("Tds_Perc").ToString) = False Then
                    txt_TdsPerc.Text = dt.Rows(0)("Tds_Perc").ToString
                End If
            End If
            dt.Dispose()
            da.Dispose()

        End If

    End Sub
    Private Sub dgv_OwnOrderSelection_CellClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_OwnOrderSelection.CellClick
        Select_OwnOrderPiece(e.RowIndex)
    End Sub

    Private Sub Select_OwnOrderPiece(ByVal RwIndx As Integer)
        Dim i As Integer

        With dgv_OwnOrderSelection

            If .RowCount > 0 And RwIndx >= 0 Then

                .Rows(RwIndx).Cells(6).Value = (Val(.Rows(RwIndx).Cells(6).Value) + 1) Mod 2

                If Val(.Rows(RwIndx).Cells(6).Value) = 1 Then

                    For i = 0 To .ColumnCount - 1
                        .Rows(RwIndx).Cells(i).Style.ForeColor = Color.Red
                    Next

                Else
                    .Rows(RwIndx).Cells(6).Value = ""

                    For i = 0 To .ColumnCount - 1
                        .Rows(RwIndx).Cells(i).Style.ForeColor = Color.Black
                    Next

                End If

            End If

        End With

    End Sub

    Private Sub dgv_OwnOrderSelection_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_OwnOrderSelection.KeyDown
        Dim n As Integer

        On Error Resume Next

        If e.KeyCode = Keys.Enter Or e.KeyCode = Keys.Space Then
            If dgv_OwnOrderSelection.CurrentCell.RowIndex >= 0 Then

                n = dgv_OwnOrderSelection.CurrentCell.RowIndex

                Select_OwnOrderPiece(n)

                e.Handled = True

            End If
        End If

    End Sub

    Private Sub btn_Close_OwnOrderSelection_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Close_OwnOrderSelection.Click
        Close_OwnOrder_Selection()
    End Sub

    Private Sub Close_OwnOrder_Selection()
        Dim Da1 As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim n As Integer = 0
        Dim sno As Integer = 0
        Dim i As Integer = 0
        Dim j As Integer = 0

        lbl_OrderNo.Text = ""
        lbl_OrderCode.Text = ""

        For i = 0 To dgv_OwnOrderSelection.RowCount - 1

            If Val(dgv_OwnOrderSelection.Rows(i).Cells(6).Value) = 1 Then

                ' lbl_RecCode.Text = dgv_Selection.Rows(i).Cells(8).Value

                lbl_OrderNo.Text = dgv_OwnOrderSelection.Rows(i).Cells(3).Value
                lbl_OrderCode.Text = dgv_OwnOrderSelection.Rows(i).Cells(7).Value

            End If

        Next

        pnl_Back.Enabled = True
        pnl_OwnOrderSelection.Visible = False
        If txt_SetNo.Enabled And txt_SetNo.Visible Then txt_SetNo.Focus()

    End Sub

    Private Sub btn_UserModification_Click(sender As System.Object, e As System.EventArgs) Handles btn_UserModification.Click
        If Val(Common_Procedures.User.IdNo) = 1 Then
            Dim f1 As New User_Modifications
            f1.Entry_Name = Me.Name
            f1.Entry_PkValue = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)
            f1.ShowDialog()
        End If
    End Sub

    Private Sub dtp_InvoiceDate_KeyUp(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles dtp_InvoiceDate.KeyUp
        If e.KeyCode = 17 And e.Control = False And vcbo_KeyDwnVal = e.KeyValue Then
            dtp_InvoiceDate.Text = Date.Today
        End If
    End Sub

    Private Sub dtp_InvoiceDate_TextChanged(sender As Object, e As System.EventArgs) Handles dtp_InvoiceDate.TextChanged
        If IsDate(dtp_InvoiceDate.Text) = True Then
            msk_InvoiceDate.Text = dtp_InvoiceDate.Text
            msk_InvoiceDate.SelectionStart = 0
        End If
    End Sub

    Private Sub cbo_jobcardno_GotFocus(sender As Object, e As EventArgs) Handles cbo_jobcardno.GotFocus
        Dim Led_ID As Integer
        Led_ID = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Sizing.Text)

        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Sizing_JobCard_Head", "Sizing_JobCode_forSelection", "(ledger_idno = " & Str(Val(Led_ID)) & ")", "(Sizing_JobCode_forSelection = '')")
    End Sub

    Private Sub cbo_jobcardno_KeyDown(sender As Object, e As KeyEventArgs) Handles cbo_jobcardno.KeyDown
        Dim Led_ID As Integer
        Led_ID = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Sizing.Text)
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_jobcardno, Nothing, Nothing, "Sizing_JobCard_Head", "Sizing_JobCode_forSelection", "(ledger_idno = " & Str(Val(Led_ID)) & ")", "(Sizing_JobCode_forSelection = '')")
        If (e.KeyValue = 38 And sender.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
            If cbo_YarnStock.Visible And cbo_YarnStock.Enabled Then
                cbo_YarnStock.Focus()
            ElseIf txt_PcsLength.Enabled And txt_PcsLength.Visible Then
                txt_PcsLength.Focus()
            ElseIf cbo_Sizing.Enabled And cbo_Sizing.Visible Then
                cbo_Sizing.Focus()
            Else
                msk_Date.Focus()
            End If
        ElseIf (e.KeyValue = 40 And sender.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
            If txt_TotalBeams.Visible And txt_TotalBeams.Enabled Then
                txt_TotalBeams.Focus()
            Else
                If dgv_PavuDetails.Rows.Count > 0 Then
                    dgv_PavuDetails.Focus()
                    dgv_PavuDetails.CurrentCell = dgv_PavuDetails.Rows(0).Cells(1)
                    dgv_PavuDetails.CurrentCell.Selected = True
                Else
                    txt_WarpMtr.Focus()
                End If
            End If
        End If

    End Sub

    Private Sub cbo_jobcardno_KeyPress(sender As Object, e As KeyPressEventArgs) Handles cbo_jobcardno.KeyPress
        Dim Led_ID As Integer
        Led_ID = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Sizing.Text)
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_jobcardno, Nothing, "Sizing_JobCard_Head", "Sizing_JobCode_forSelection", "(ledger_idno = " & Str(Val(Led_ID)) & ")", "(Sizing_JobCode_forSelection = '')")

        If Asc(e.KeyChar) = 13 Then
            If txt_TotalBeams.Visible And txt_TotalBeams.Enabled Then
                txt_TotalBeams.Focus()
            Else
                If dgv_PavuDetails.Rows.Count > 0 Then
                    dgv_PavuDetails.Focus()
                    dgv_PavuDetails.CurrentCell = dgv_PavuDetails.Rows(0).Cells(1)
                    dgv_PavuDetails.CurrentCell.Selected = True
                Else
                    txt_WarpMtr.Focus()
                End If
            End If

        End If
    End Sub

    Private Sub txt_AddLess_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txt_AddLess.KeyPress
        If Common_Procedures.Accept_NegativeNumbers(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub dgtxt_YarnDetails_TextChanged(sender As Object, e As EventArgs) Handles dgtxt_YarnDetails.TextChanged
        Try
            With dgv_YarnDetails
                If .Visible Then
                    If .Rows.Count > 0 Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(dgtxt_YarnDetails.Text)
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

    Private Sub dgtxt_PavuDetails_TextChanged(sender As Object, e As EventArgs) Handles dgtxt_PavuDetails.TextChanged
        Try
            With dgv_PavuDetails
                If .Visible Then
                    If .Rows.Count > 0 Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(dgtxt_PavuDetails.Text)
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
    Private Sub PickUp_Calculation()
        Dim P_Up As Double = 0
        Dim P_up_Act As Double = 0
        Dim vWGT As String = 0
        Dim vTapLen As String = 0
        Dim vWrpEnds1 As String = 0
        Dim vWrpEnds2 As String = 0
        Dim vWrpEnds3 As String = 0
        Dim vSizMtrs1 As String = 0, vSizWgt1 As String = 0
        Dim vSizMtrs2 As String = 0, vSizWgt2 As String = 0
        Dim vSizMtrs3 As String = 0, vSizWgt3 As String = 0
        Dim TotPckUP As String = ""
        Dim Inx As Integer = 0



        P_up_Act = 0
        If Val(txt_ConsumedYarn.Text) <> 0 Then
            P_up_Act = Format((Val(txt_Sizing_Net_Wgt.Text) - Val(txt_ConsumedYarn.Text)) / Val(txt_ConsumedYarn.Text) * 100, "########0.000")
        End If
        lbl_PickUp_Perc.Text = Format(Val(P_up_Act), "######0.00")


    End Sub
    Private Sub lbl_PickUp_Perc_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles lbl_PickUp_Perc.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
        If Asc(e.KeyChar) = 13 Then

            If lbl_Elongation_Perc.Visible And lbl_Elongation_Perc.Enabled Then
                lbl_Elongation_Perc.Focus()
            Else

                If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                    save_record()
                Else
                    msk_Date.Focus()
                End If
            End If
        End If
    End Sub

    Private Sub lbl_PickUp_Perc_KeyDown(sender As Object, e As KeyEventArgs) Handles lbl_PickUp_Perc.KeyDown
        If e.KeyCode = 38 Then
            txt_TdsPerc.Focus()
        ElseIf e.KeyCode = 40 Then
            If lbl_Elongation_Perc.Visible And lbl_Elongation_Perc.Enabled Then
                lbl_Elongation_Perc.Focus()
            Else

                If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                    save_record()
                Else
                    msk_Date.Focus()
                End If
            End If
        End If
    End Sub

    Private Sub lbl_Elongation_Perc_KeyPress(sender As Object, e As KeyPressEventArgs) Handles lbl_Elongation_Perc.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
        If Asc(e.KeyChar) = 13 Then

            If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                save_record()
            Else
                msk_Date.Focus()
            End If
        End If
    End Sub

    Private Sub lbl_Elongation_Perc_KeyDown(sender As Object, e As KeyEventArgs) Handles lbl_Elongation_Perc.KeyDown
        If e.KeyCode = 38 Then
            lbl_PickUp_Perc.Focus()
        ElseIf e.KeyCode = 40 Then
            If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                save_record()
            Else
                msk_Date.Focus()
            End If
        End If
    End Sub

    Private Sub txt_TdsPerc_KeyDown(sender As Object, e As KeyEventArgs) Handles txt_TdsPerc.KeyDown
        If e.KeyCode = 38 Then
            txt_SGST_Percentage.Focus()
        ElseIf e.KeyCode = 40 Then

            If txt_Sizing_Net_Wgt.Visible And txt_Sizing_Net_Wgt.Enabled Then
                txt_Sizing_Net_Wgt.Focus()

            ElseIf lbl_PickUp_Perc.Visible And lbl_PickUp_Perc.Enabled Then
                lbl_PickUp_Perc.Focus()
            Else
                If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                    save_record()
                Else
                    msk_Date.Focus()
                End If
            End If
        End If
    End Sub
    Private Sub cbo_Grid_EndsCount_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Grid_EndsCount.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "EndsCount_Head", "EndsCount_Name", "", "(EndsCount_IdNo = 0)")
    End Sub
    Private Sub cbo_Grid_EndsCount_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Grid_EndsCount.KeyDown
        vcbo_KeyDwnVal = e.KeyValue

        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Grid_EndsCount, Nothing, Nothing, "EndsCount_Head", "EndsCount_Name", "", "(EndsCount_IdNo = 0)")
        With dgv_PavuDetails
            If e.KeyCode = 38 And cbo_Grid_EndsCount.DroppedDown = False Or (e.Control = True And e.KeyCode = 38) Then
                If .Visible Then
                    e.Handled = True
                    e.SuppressKeyPress = True
                    .Focus()
                    .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex - 1)
                    .CurrentCell.Selected = True
                End If
            End If

            If e.KeyCode = 40 And cbo_Grid_EndsCount.DroppedDown = False Or (e.Control = True And e.KeyCode = 40) Then
                If .Visible Then
                    e.Handled = True
                    e.SuppressKeyPress = True
                    .Focus()
                    .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)
                    .CurrentCell.Selected = True
                End If
            End If

        End With
    End Sub

    Private Sub cbo_Grid_EndsCount_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Grid_EndsCount.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Grid_EndsCount, Nothing, "EndsCount_Head", "EndsCount_Name", "", "(EndsCount_IdNo = 0)")

        With dgv_PavuDetails

            If .Visible Then
                If Asc(e.KeyChar) = 13 Then
                    .Focus()
                    .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)
                    .CurrentCell.Selected = True
                End If
            End If

        End With
    End Sub
    Private Sub cbo_Grid_EndsCount_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Grid_EndsCount.KeyUp
        If Trim(UCase(cbo_Type.Text)) <> "RECEIPT" And Trim(UCase(cbo_Type.Text)) <> "SIZING-UNIT PAVU DELIVERY" Then

            If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
                Dim f As New EndsCount_Creation

                Common_Procedures.Master_Return.Form_Name = Me.Name
                Common_Procedures.Master_Return.Control_Name = cbo_EndsCount.Name
                Common_Procedures.Master_Return.Return_Value = ""
                Common_Procedures.Master_Return.Master_Type = ""

                f.MdiParent = MDIParent1
                f.Show()

            End If
        End If

    End Sub
    Private Sub cbo_Grid_EndsCount_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Grid_EndsCount.TextChanged
        Try
            With dgv_PavuDetails
                If cbo_Grid_EndsCount.Visible = True Then

                    If Val(cbo_Grid_EndsCount.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = dgvCol_Details.Ends_Count Then
                        .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex).Value = Trim(cbo_Grid_EndsCount.Text)
                    End If

                End If

            End With
        Catch ex As Exception

        End Try
    End Sub

    Private Sub Cbo_RateFor_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles Cbo_RateFor.Enter
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "", "", "", "")
    End Sub

    Private Sub Cbo_RateFor_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Cbo_RateFor.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, sender, msk_InvoiceDate, txt_InvRate, "", "", "", "")
    End Sub

    Private Sub Cbo_RateFor_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Cbo_RateFor.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, Cbo_RateFor, txt_InvRate, "", "", "", "")
    End Sub

    Private Sub txt_InvRate_KeyDown(sender As Object, e As KeyEventArgs) Handles txt_InvRate.KeyDown
        If e.KeyCode = 38 Then
            If Cbo_RateFor.Visible Then
                Cbo_RateFor.Focus()
            Else
                msk_InvoiceDate.Focus()
            End If
        ElseIf e.KeyCode = 40 Then
            txt_WindingRate.Focus()
        End If
    End Sub
    Private Sub Cbo_RateFor_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles Cbo_RateFor.TextChanged
        NetAmount_Calculation()
    End Sub
    Private Sub cbo_Grid_BeamType_GotFocus(sender As Object, e As EventArgs) Handles cbo_Grid_BeamType.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "LoomTYpe_Head", "LoomType_Name", "", "(LoomType_IdNo = 0)")
    End Sub
    Private Sub cbo_Grid_BeamType_KeyPress(sender As Object, e As KeyPressEventArgs) Handles cbo_Grid_BeamType.KeyPress

        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Grid_BeamType, "", "LoomType_Head", "LoomType_Name", "", "(LoomType_IdNo = 0)")

        If Asc(e.KeyChar) = 13 Then

            With dgv_PavuDetails

                If dgv_PavuDetails.Columns(dgvCol_Details.Gross_Weight).Visible Then
                    .Focus()
                    .CurrentCell = .Rows(.CurrentRow.Index).Cells(dgvCol_Details.Gross_Weight)
                    .CurrentCell.Selected = True
                Else
                    If .CurrentCell.RowIndex = .RowCount - 1 Then

                        .Rows.Add()
                        .Focus()
                        dgv_PavuDetails.CurrentCell = dgv_PavuDetails.Rows(dgv_PavuDetails.CurrentRow.Index + 1).Cells(dgvCol_Details.Beam_No)
                        .CurrentCell.Selected = True
                    Else
                        .Focus()
                        dgv_PavuDetails.CurrentCell = dgv_PavuDetails.Rows(dgv_PavuDetails.CurrentRow.Index + 1).Cells(dgvCol_Details.Beam_No)
                        .CurrentCell.Selected = True
                    End If

                End If
            End With

        End If


    End Sub
    Private Sub cbo_Grid_BeamType_KeyDown(sender As Object, e As KeyEventArgs) Handles cbo_Grid_BeamType.KeyDown

        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Grid_BeamType, cbo_Grid_BeamWidth, "", "LoomTYpe_Head", "LoomTYpe_Name", "", "(LoomTYpe_IdNo = 0)")

        With dgv_PavuDetails

            If (e.KeyValue = 38 And cbo_Grid_BeamType.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then

                .Focus()
                .CurrentCell = .Rows(.CurrentRow.Index).Cells(dgvCol_Details.beam_width)
                .CurrentCell.Selected = True
            End If


            If (e.KeyValue = 40 And cbo_Grid_BeamType.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then

                If dgv_PavuDetails.Columns(dgvCol_Details.Gross_Weight).Visible Then
                    .Focus()
                    .CurrentCell = .Rows(.CurrentRow.Index).Cells(dgvCol_Details.Gross_Weight)
                    .CurrentCell.Selected = True
                Else
                    If .CurrentCell.RowIndex = .RowCount - 1 Then

                        .Rows.Add()
                        .Focus()
                        dgv_PavuDetails.CurrentCell = dgv_PavuDetails.Rows(dgv_PavuDetails.CurrentRow.Index + 1).Cells(dgvCol_Details.Beam_No)
                        .CurrentCell.Selected = True
                    Else
                        .Focus()
                        dgv_PavuDetails.CurrentCell = dgv_PavuDetails.Rows(dgv_PavuDetails.CurrentRow.Index + 1).Cells(dgvCol_Details.Beam_No)
                        .CurrentCell.Selected = True
                    End If

                End If

            End If

        End With


    End Sub
    Private Sub cbo_Grid_BeamType_KeyUp(sender As Object, e As KeyEventArgs) Handles cbo_Grid_BeamType.KeyUp

        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Dim f As New LoomType_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Grid_BeamType.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If


    End Sub
    Private Sub cbo_Grid_BeamType_TextChanged(sender As Object, e As EventArgs) Handles cbo_Grid_BeamType.TextChanged
        If IsNothing(dgv_PavuDetails.CurrentCell) Then Exit Sub

        Try
            If cbo_Grid_BeamType.Visible Then
                With dgv_PavuDetails
                    If Val(cbo_Grid_BeamType.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = dgvCol_Details.Beam_Type Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_Grid_BeamType.Text)
                    End If
                End With
            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub
    Private Sub cbo_Grid_Yarn_LotNo_Enter(sender As Object, e As EventArgs) Handles cbo_Grid_Yarn_LotNo.Enter
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Yarn_Lot_Head", "LotCode_forSelection", "Count_IdNo = (Select Count_IdNo from Count_Head where Count_Name = '" & dgv_YarnDetails.CurrentRow.Cells(1).Value & "') and Mill_IdNo = (Select Mill_IdNo from Mill_Head where Mill_Name = '" & dgv_YarnDetails.CurrentRow.Cells(3).Value & "')", "(Lot_No = '')")
    End Sub

    Private Sub cbo_Grid_Yarn_LotNo_TextChanged(sender As Object, e As EventArgs) Handles cbo_Grid_Yarn_LotNo.TextChanged

        Try
            If cbo_Grid_Yarn_LotNo.Visible Then

                If IsNothing(dgv_YarnDetails.CurrentCell) Then Exit Sub

                With dgv_YarnDetails
                    If Val(cbo_Grid_Yarn_LotNo.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = 7 Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(7).Value = Trim(cbo_Grid_Yarn_LotNo.Text)
                    End If
                End With
            End If

        Catch ex As Exception

        End Try

    End Sub

    Private Sub cbo_Grid_Yarn_LotNo_KeyDown(sender As Object, e As KeyEventArgs) Handles cbo_Grid_Yarn_LotNo.KeyDown

        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Grid_Yarn_LotNo, Nothing, Nothing, "Yarn_Lot_Head", "LotCode_forSelection", "Count_IdNo = (Select Count_IdNo from Count_Head where Count_Name = '" & dgv_YarnDetails.CurrentRow.Cells(1).Value & "') and Mill_IdNo = (Select Mill_IdNo from Mill_Head where Mill_Name = '" & dgv_YarnDetails.CurrentRow.Cells(3).Value & "')", "(Lot_No = '')")

        With dgv_YarnDetails

            If (e.KeyValue = 38 And cbo_Grid_Yarn_LotNo.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
                .Focus()
                .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex - 1)
            End If

            If (e.KeyValue = 40 And cbo_Grid_Yarn_LotNo.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
                .Focus()
                If .CurrentCell.ColumnIndex < .ColumnCount - 1 Then
                    .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 1)
                Else
                    If .CurrentRow.Index < .RowCount - 1 Then
                        .CurrentCell = .Rows(.CurrentRow.Index + 1).Cells(1)
                    End If
                End If
            End If

        End With

    End Sub

    Private Sub cbo_Grid_Yarn_LotNo_KeyPress(sender As Object, e As KeyPressEventArgs) Handles cbo_Grid_Yarn_LotNo.KeyPress

        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Grid_Yarn_LotNo, Nothing, "Yarn_Lot_Head", "LotCode_forSelection", "Count_IdNo = (Select Count_IdNo from Count_Head where Count_Name = '" & dgv_YarnDetails.CurrentRow.Cells(1).Value & "') and Mill_IdNo = (Select Mill_IdNo from Mill_Head where Mill_Name = '" & dgv_YarnDetails.CurrentRow.Cells(3).Value & "')", "(Lot_No = '')")

        If Asc(e.KeyChar) = 13 Then

            With dgv_YarnDetails

                .Focus()
                If .CurrentCell.ColumnIndex < .ColumnCount - 1 Then
                    .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 1)
                Else
                    If .CurrentRow.Index < .RowCount - 1 Then
                        .CurrentCell = .Rows(.CurrentRow.Index + 1).Cells(1)
                    End If
                End If

            End With

        End If

    End Sub

    Private Sub cbo_Grid_Yarn_LotNo_KeyUp(sender As Object, e As KeyEventArgs) Handles cbo_Grid_Yarn_LotNo.KeyUp

        If e.Control = False And e.KeyValue = 17 Then
            Dim f As New Yarn_Lot_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Grid_Yarn_LotNo.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If

    End Sub

    Private Sub cbo_Sizing_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbo_Sizing.SelectedIndexChanged

    End Sub

    Private Sub txt_SetNo_TextChanged(sender As Object, e As EventArgs) Handles txt_SetNo.TextChanged

    End Sub

    Private Sub txt_SetNo_KeyDown(sender As Object, e As KeyEventArgs) Handles txt_SetNo.KeyDown
        If (e.KeyValue) = 40 Then
            txt_PcsLength.Focus()
        End If

        If e.KeyValue = 38 Then
            If cbo_ClothSales_OrderCode_forSelection.Visible = True Then
                cbo_ClothSales_OrderCode_forSelection.Focus()
            Else
                cbo_Sizing.Focus()
            End If
        End If

    End Sub

    Private Sub cbo_ClothSales_OrderCode_forSelection_KeyPress(sender As Object, e As KeyPressEventArgs) Handles cbo_ClothSales_OrderCode_forSelection.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, sender, txt_SetNo, "ClothSales_Order_Head", "ClothSales_OrderCode_forSelection", vCLO_CONDT, "(ClothSales_Order_Code = '999999/00-00')")
    End Sub

    Private Sub cbo_ClothSales_OrderCode_forSelection_KeyDown(sender As Object, e As KeyEventArgs) Handles cbo_ClothSales_OrderCode_forSelection.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, sender, cbo_Sizing, txt_SetNo, "ClothSales_Order_Head", "ClothSales_OrderCode_forSelection", vCLO_CONDT, "(ClothSales_Order_Code = '999999/00-00')")
    End Sub

    Private Sub cbo_ClothSales_OrderCode_forSelection_Enter(sender As Object, e As EventArgs) Handles cbo_ClothSales_OrderCode_forSelection.Enter
        vCLO_CONDT = "(ClothSales_Order_Code LIKE '%/" & Trim(FnYearCode1) & "' or ClothSales_Order_Code LIKE '%/" & Trim(FnYearCode2) & "' and Order_Close_Status = 0 )"
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "ClothSales_Order_Head", "ClothSales_OrderCode_forSelection", vCLO_CONDT, "(ClothSales_Order_Code = '999999/00-00')")
    End Sub

    Private Sub txt_SetNo_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txt_SetNo.KeyPress
        If Asc(e.KeyChar) = 13 Then
            txt_PcsLength.Focus()
        End If
    End Sub
    Private Sub btn_BarcodePrint_Click(sender As Object, e As EventArgs) Handles btn_BarcodePrint.Click
        'Common_Procedures.Print_OR_Preview_Status = 0
        'Prn_BarcodeSticker = True
        Printing_BarCode_Sticker_Format1_1608_DosPrint()
    End Sub
    Private Sub Printing_BarCode_Sticker_Format1_1608_DosPrint()
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim NewCode As String = ""
        Dim PrnTxt As String = ""
        Dim LnCnt As Integer = 0
        Dim I As Integer = 0
        Dim NoofItems_PerPage As Integer
        Dim vBarCode As String = ""
        Dim vFldMtrs As String = "", vPcs As String = ""
        Dim ItmNm1 As String, ItmNm2 As String
        Dim prn_DetAr(,) As String

        prn_DetDt.Clear()
        prn_DetIndx = 0

        Erase prn_DetAr
        prn_DetAr = New String(100, 10) {}

        Try

            NewCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)


            da2 = New SqlClient.SqlDataAdapter("select a.*, Lh.Ledger_Name,d.Ends_Name from Sizing_SpecificationPavu_Details a INNER JOIN Sizing_Specification_Head hd on hd.Sizing_Specification_Code = a.Sizing_Specification_Code inner Join Ledger_Head Lh ON hd.Ledger_IdNo = lh.Ledger_IdNo LEFT OUTER JOIN EndsCount_Head d ON hd.EndsCount_IdNo = d.EndsCount_IdNo  where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Sizing_Specification_Code = '" & Trim(NewCode) & "' Order by a.sl_no", con)
            prn_DetDt = New DataTable
            da2.Fill(prn_DetDt)

            If prn_DetDt.Rows.Count > 0 Then

                For I = 0 To prn_DetDt.Rows.Count - 1

                    prn_DetIndx = prn_DetIndx + 1

                    prn_DetAr(prn_DetIndx, 0) = Trim(prn_DetDt.Rows(I).Item("Ledger_Name").ToString)
                    prn_DetAr(prn_DetIndx, 1) = Trim(prn_DetDt.Rows(I).Item("Set_No").ToString)
                    prn_DetAr(prn_DetIndx, 2) = Trim(prn_DetDt.Rows(I).Item("Beam_No").ToString)
                    prn_DetAr(prn_DetIndx, 3) = Trim(prn_DetDt.Rows(I).Item("Ends_Name").ToString)
                    prn_DetAr(prn_DetIndx, 4) = Format(Val(prn_DetDt.Rows(I).Item("Meters").ToString), "##########0.00")
                    prn_DetAr(prn_DetIndx, 5) = Trim(prn_DetDt.Rows(I).Item("Set_Code").ToString) & "/" & prn_DetDt.Rows(I).Item("Beam_No").ToString

                Next

                Common_Procedures.Printing_BarCode_Sticker_Format1_1608_DosPrint(prn_DetDt, prn_DetAr, prn_DetIndx)
            Else

                MessageBox.Show("This is New Entry", "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Exit Sub

            End If

            da1.Dispose()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub txt_Sizing_Net_Wgt_TextChanged(sender As Object, e As EventArgs) Handles txt_Sizing_Net_Wgt.TextChanged
        PickUp_Calculation()
    End Sub

    Private Sub txt_Sizing_Net_Wgt_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txt_Sizing_Net_Wgt.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True

        If Asc(e.KeyChar) = 13 Then
            lbl_PickUp_Perc.Focus()
        End If

    End Sub

    Private Sub txt_Sizing_Net_Wgt_KeyDown(sender As Object, e As KeyEventArgs) Handles txt_Sizing_Net_Wgt.KeyDown
        If e.KeyCode = 40 Then
            lbl_PickUp_Perc.Focus()
        End If
        If e.KeyCode = 38 Then
            txt_TdsPerc.Focus()
        End If

    End Sub
End Class