Public Class Cloth_Creation
    Implements Interface_MDIActions

    Private con As New SqlClient.SqlConnection(Common_Procedures.Connection_String)
    Private New_Entry As Boolean = False
    Private Prec_ActCtrl As New Control
    Private vcbo_KeyDwnVal As Double
    Private FrmLdSTS As Boolean = False
    Private WithEvents dgtxt_EndsCountDetails As New DataGridViewTextBoxEditingControl
    Private WithEvents dgtxt_BobinDetails As New DataGridViewTextBoxEditingControl
    Private WithEvents dgtxt_KuriDetails As New DataGridViewTextBoxEditingControl
    Private WithEvents dgtxt_Additional_Weft_Details As New DataGridViewTextBoxEditingControl
    Private WithEvents dgtxt_Warp_Details As New DataGridViewTextBoxEditingControl
    Private WithEvents dgtxt_SalesRate_Details As New DataGridViewTextBoxEditingControl
    Private WithEvents dgtxt_FoldingWages_Details As New DataGridViewTextBoxEditingControl
    Private dgv_ActiveCtrl_Name As String
    Private TrnTo_DbName As String = ""

    Public Sub New()
        FrmLdSTS = True
        ' This call is required by the designer.
        InitializeComponent()
        ' Add any initialization after the InitializeComponent() call.
    End Sub

    Private Sub clear()

        pnl_Back.Enabled = True
        grp_Filter.Visible = False
        pnl_Bobin.Visible = False
        pnl_Additional_Weft_Details.Visible = False
        pnl_Mark_Wages.Visible = False


        lbl_IdNo.Text = ""
        lbl_IdNo.ForeColor = Color.Black
        cbo_Description.Text = ""
        cbo_quality_description.Text = ""

        cbo_ClothGroup.Text = ""
        txt_TapeLength.Text = ""
        txt_BeamLength.Text = ""
        txt_Weight_Meter_Yarn.Text = ""
        txt_Weight_Meter_Pavu.Text = ""
        txt_ReedSpace.Text = ""
        txt_Reed.Text = ""
        txt_Pick.Text = ""
        cbo_EndsCount.Tag = -1
        cbo_EndsCount.Text = ""
        txt_Width.Text = ""
        cbo_WarpCount.Text = ""
        cbo_WeftCount.Text = ""
        txt_Name.Text = ""
        cbo_StockIn.Text = "METER"
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1380" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1446" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1461" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1494" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1546" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1549" Then '---- R.M TEX & FABRICS (VIJAYAMANGALAM)
            cbo_StockIn.Text = "PCS"
        End If
        cbo_WeaverWages_for.Text = "METER"
        txt_MeterPcs.Text = ""
        Cbo_Article.Text = ""
        cbo_EndsCountMainName.Text = ""
        cbo_Weaver_Weft_Consumption.Text = "MTR"
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1005" Then
            cbo_Weaver_Weft_Consumption.Text = "PCS"
        End If
        chk_CloseStatus.Checked = False
        Chk_Multi_EndsCount.Checked = False
        Chk_Multi_Weft_Count.Checked = False
        chk_NonMoving_Cloth_Status.Checked = False
        cbo_ClothSet.Text = ""
        txt_AllowShortage_Perc_Processing.Text = ""
        cbo_ClothType.Text = "GREY"
        txt_Weave.Text = ""
        txt_Allowed_Excess_Doff_Meters_Percentage.Text = ""
        txt_PickActual.Text = ""
        txt_CrimpPercActual.Text = ""
        txt_Weight_Meter_YarnActual.Text = ""

        txt_sortno.Text = ""
        txt_CrimpPerc.Text = ""
        txt_Coolie_Type1.Text = ""
        txt_Coolie_Type2.Text = ""
        txt_Coolie_Type3.Text = ""
        txt_Coolie_Type4.Text = ""
        txt_Coolie_Type5.Text = ""
        txt_weight_min.Text = ""
        txt_weight_max.Text = ""
        txt_Weight_Meter_Fabric.Text = ""
        txt_RollTube_Wgt.Text = ""

        txt_Type1_Rate.Text = ""
        txt_Type2_Rate.Text = ""
        txt_Type3_Rate.Text = ""
        txt_Type4_Rate.Text = ""
        txt_Type5_Rate.Text = ""
        cbo_Transfer.Text = ""
        txt_TamilName.Text = ""
        txt_fabric_gsm.Text = ""

        Cbo_Pavu_Consumption_In_Meter_Weight.Text = "METER"
        cbo_Fabric_Processing_Reconsilation_Mtrs_Wgt.Text = "METER"

        txt_EPI_PPI.Text = ""
        cbo_fabric_name.Text = ""
        dgv_EndsCountDetails.Rows.Clear()
        dgv_BobinDetails.Rows.Clear()
        dgv_KuriDetails.Rows.Clear()
        cbo_fabric_category.Text = ""

        pnl_SalesRate_Details.Visible = False
        dgv_SalesRate_Details.Rows.Clear()
        dgv_MarkWages_Details.Rows.Clear()

        dgv_Additional_Weft_Details.Rows.Clear()
        Dgv_Warp_Count_Details.Rows.Clear()
        cbo_grid_Additional_Weft_Details.Visible = False
        Cbo_grid_Mts_Wgt.Visible = False
        Cbo_Grid_Gram_Percentage.Visible = False
        Cbo_Grid_Pile_Ground.Visible = False
        Cbo_Grid_EndsCount.Visible = False

        cbo_EndsCount.Visible = False
        cbo_EndsCount.Tag = -1
        cbo_EndsCount.Text = ""

        cbo_ItemGroup.Text = ""
        txt_Bale_Weight_from.Text = ""
        txt_bale_weight_to.Text = ""
        txt_wrap_waste_percentage.Text = ""
        txt_weft_waste_percentage.Text = ""
        txt_checking_wages_per_meter.Text = ""
        txt_folding_wages_per_meter.Text = ""

        New_Entry = False

        grp_Open.Visible = False
        New_Entry = False
        dgv_ActiveCtrl_Name = ""
        cbo_Slevedge.Text = ""
        txt_Slevedge_Waste.Text = ""
        txt_Employee_Wages_Per_Meter.Text = ""


    End Sub

    Private Sub ControlGotFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim txtbx As TextBox
        Dim combobx As ComboBox

        If FrmLdSTS = True Then Exit Sub

        On Error Resume Next

        If TypeOf Me.ActiveControl Is TextBox Or TypeOf Me.ActiveControl Is ComboBox Then
            Me.ActiveControl.BackColor = Color.Lime ' Color.MistyRose ' Color.lime
            Me.ActiveControl.ForeColor = Color.Blue
        End If

        If TypeOf Me.ActiveControl Is TextBox Then
            txtbx = Me.ActiveControl
            txtbx.SelectAll()

        ElseIf TypeOf Me.ActiveControl Is ComboBox Then
            combobx = Me.ActiveControl
            combobx.SelectAll()

        End If

        If Me.ActiveControl.Name <> cbo_EndsCount.Name Then
            cbo_EndsCount.Visible = False
        End If
        If Me.ActiveControl.Name <> cbo_GridCount.Name Then
            cbo_GridCount.Visible = False
        End If
        If Me.ActiveControl.Name <> cbo_grid_Additional_Weft_Details.Name Then
            cbo_grid_Additional_Weft_Details.Visible = False
        End If
        If Me.ActiveControl.Name <> Cbo_grid_Mts_Wgt.Name Then
            Cbo_grid_Mts_Wgt.Visible = False
        End If
        If Me.ActiveControl.Name <> Cbo_Grid_Gram_Percentage.Name Then
            Cbo_Grid_Gram_Percentage.Visible = False
        End If
        If Me.ActiveControl.Name <> Cbo_Grid_Pile_Ground.Name Then
            Cbo_Grid_Pile_Ground.Visible = False
        End If

        If Me.ActiveControl.Name <> Cbo_Grid_EndsCount.Name Then
            Cbo_Grid_EndsCount.Visible = False
        End If

        If Me.ActiveControl.Name <> cbo_GridEndsCount.Name Then
            cbo_GridEndsCount.Visible = False
        End If

        'If Me.ActiveControl.Name <> dgv_Filter.Name Then
        '    Grid_Cell_DeSelect()
        'End If

        Prec_ActCtrl = Me.ActiveControl

    End Sub

    Private Sub ControlLostFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        If FrmLdSTS = True Then Exit Sub

        On Error Resume Next

        If IsNothing(Prec_ActCtrl) = False Then
            If TypeOf Prec_ActCtrl Is TextBox Or TypeOf Prec_ActCtrl Is ComboBox Then
                Prec_ActCtrl.BackColor = Color.White
                Prec_ActCtrl.ForeColor = Color.Black
            End If
        End If
        Grid_DeSelect()
    End Sub

    Private Sub ControlLostFocus1(ByVal sender As Object, ByVal e As System.EventArgs)
        If FrmLdSTS = True Then Exit Sub

        On Error Resume Next

        If IsNothing(Prec_ActCtrl) = False Then
            If TypeOf Prec_ActCtrl Is TextBox Or TypeOf Prec_ActCtrl Is ComboBox Then
                Prec_ActCtrl.BackColor = Color.LightBlue
                Prec_ActCtrl.ForeColor = Color.Blue
            End If
        End If
        Grid_DeSelect()
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

    Private Sub Grid_DeSelect()
        If FrmLdSTS = True Then Exit Sub
        On Error Resume Next
        If Not IsNothing(dgv_EndsCountDetails.CurrentCell) Then dgv_EndsCountDetails.CurrentCell.Selected = False
        If Not IsNothing(dgv_BobinDetails.CurrentCell) Then dgv_BobinDetails.CurrentCell.Selected = False
        If Not IsNothing(dgv_KuriDetails.CurrentCell) Then dgv_KuriDetails.CurrentCell.Selected = False
        If Not IsNothing(dgv_SalesRate_Details.CurrentCell) Then dgv_SalesRate_Details.CurrentCell.Selected = False
        If Not IsNothing(dgv_Additional_Weft_Details.CurrentCell) Then dgv_Additional_Weft_Details.CurrentCell.Selected = False
        If Not IsNothing(Dgv_Warp_Count_Details.CurrentCell) Then Dgv_Warp_Count_Details.CurrentCell.Selected = False
    End Sub

    Private Sub move_record(ByVal idno As Integer)
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim dt2 As New DataTable
        Dim dt3 As New DataTable
        Dim dt4 As New DataTable
        Dim dt5 As New DataTable
        Dim dt6 As New DataTable
        Dim dt7 As New DataTable
        Dim slno, n, Sno As Integer

        If Val(idno) = 0 Then Exit Sub

        clear()

        Try

            da = New SqlClient.SqlDataAdapter("select a.*, b.Count_Name as Warp_Count, c.Count_Name as Weft_Count , d.Cloth_Name as stock_undername,e.EndsCount_Name ,IG.ItemGroup_Name from Cloth_Head a LEFT OUTER JOIN Count_Head b ON a.Cloth_WarpCount_IdNo = b.Count_IdNo LEFT OUTER JOIN Count_Head c ON a.Cloth_WeftCount_IdNo = c.Count_IdNo LEFT OUTER JOIN Cloth_Head d ON a.Cloth_Stockunder_IdNo = d.Cloth_IdNo LEFT OUTER JOIN EndsCount_Head E ON a.EndsCount_IdNo = E.EndsCount_IdNo  LEFT OUTER JOIN ItemGroup_Head IG ON a.ItemGroup_IdNo = IG.ItemGroup_IdNo  Where a.Cloth_Idno = " & Str(Val(idno)), con)
            dt = New DataTable
            da.Fill(dt)

            If dt.Rows.Count > 0 Then

                If IsDBNull(dt.Rows(0).Item("Cloth_Idno").ToString) = False Then

                    lbl_IdNo.Text = dt.Rows(0).Item("Cloth_Idno").ToString

                    cbo_ClothType.Text = dt.Rows(0).Item("Cloth_Type").ToString

                    txt_Name.Text = dt.Rows(0).Item("ClothMain_Name").ToString

                    txt_Allowed_Excess_Doff_Meters_Percentage.Text = Val(dt.Rows(0).Item("Excess_Doffing_Meters_Percentage_Allowed").ToString)

                    cbo_Description.Text = dt.Rows(0).Item("Cloth_Description").ToString
                    cbo_quality_description.Text = dt.Rows(0).Item("Cloth_Quality_Description").ToString
                    cbo_Transfer.Text = Common_Procedures.Cloth_IdNoToName(con, Val(dt.Rows(0).Item("Transfer_To_ClothIdno").ToString), TrnTo_DbName)
                    If Val(dt.Rows(0).Item("Cloth_Stockunder_IdNo").ToString) <> Val(dt.Rows(0).Item("Cloth_Idno").ToString) Then
                        cbo_ClothGroup.Text = dt.Rows(0).Item("stock_undername").ToString
                    End If

                    If Val(dt.Rows(0).Item("Close_Status").ToString) = 1 Then chk_CloseStatus.Checked = True
                    If Val(dt.Rows(0).Item("Multiple_EndsCount_Status").ToString) = 1 Then Chk_Multi_EndsCount.Checked = True
                    If Val(dt.Rows(0).Item("Multiple_WeftCount_Status").ToString) = 1 Then Chk_Multi_Weft_Count.Checked = True
                    If Val(dt.Rows(0).Item("NonMoving_Cloth_Status").ToString) = 1 Then chk_NonMoving_Cloth_Status.Checked = True

                    cbo_WarpCount.Text = dt.Rows(0).Item("Warp_Count").ToString
                    cbo_WeftCount.Text = dt.Rows(0).Item("Weft_Count").ToString
                    txt_ReedSpace.Text = dt.Rows(0).Item("Cloth_ReedSpace").ToString
                    txt_Reed.Text = dt.Rows(0).Item("Cloth_Reed").ToString
                    txt_Pick.Text = dt.Rows(0).Item("Cloth_Pick").ToString
                    txt_Width.Text = dt.Rows(0).Item("Cloth_Width").ToString
                    txt_BeamLength.Text = dt.Rows(0).Item("Beam_Length").ToString
                    txt_TapeLength.Text = dt.Rows(0).Item("Tape_Length").ToString
                    txt_Weight_Meter_Pavu.Text = Val(dt.Rows(0).Item("Weight_Meter_Warp").ToString)
                    txt_Weight_Meter_Yarn.Text = Val(dt.Rows(0).Item("Weight_Meter_Weft").ToString)
                    txt_CrimpPerc.Text = dt.Rows(0).Item("Crimp_Percentage").ToString

                    Cbo_Pavu_Consumption_In_Meter_Weight.Text = dt.Rows(0).Item("Pavu_Consumption_In_Meter_Weight").ToString
                    cbo_Fabric_Processing_Reconsilation_Mtrs_Wgt.Text = dt.Rows(0).Item("Fabric_Processing_Reconsilation_In_Meter_Weight").ToString

                    txt_PickActual.Text = dt.Rows(0).Item("ActualCloth_Pick").ToString
                    txt_CrimpPercActual.Text = dt.Rows(0).Item("ActualCrimp_Percentage").ToString
                    txt_Weight_Meter_YarnActual.Text = Val(dt.Rows(0).Item("ActualWeight_Meter_Weft").ToString)

                    txt_Coolie_Type1.Text = dt.Rows(0).Item("Wages_For_Type1").ToString
                    txt_Coolie_Type2.Text = dt.Rows(0).Item("Wages_For_Type2").ToString
                    txt_Coolie_Type3.Text = dt.Rows(0).Item("Wages_For_Type3").ToString
                    txt_Coolie_Type4.Text = dt.Rows(0).Item("Wages_For_Type4").ToString
                    txt_Coolie_Type5.Text = dt.Rows(0).Item("Wages_For_Type5").ToString
                    txt_wrap_waste_percentage.Text = dt.Rows(0).Item("Wrap_waste_percentage").ToString
                    txt_weft_waste_percentage.Text = dt.Rows(0).Item("Weft_waste_percentage").ToString
                    txt_checking_wages_per_meter.Text = dt.Rows(0).Item("Checking_Wages_per_meter").ToString
                    txt_folding_wages_per_meter.Text = dt.Rows(0).Item("folding_Wages_per_meter").ToString

                    txt_Type1_Rate.Text = Val(dt.Rows(0).Item("Sound_Rate").ToString)
                    txt_Type2_Rate.Text = Val(dt.Rows(0).Item("Seconds_Rate").ToString)
                    txt_Type3_Rate.Text = Val(dt.Rows(0).Item("Bits_Rate").ToString)
                    txt_Type4_Rate.Text = Val(dt.Rows(0).Item("Reject_Rate").ToString)
                    txt_Type5_Rate.Text = Val(dt.Rows(0).Item("Other_Rate").ToString)
                    txt_sortno.Text = dt.Rows(0).Item("Sort_No").ToString


                    txt_Bale_Weight_from.Text = Val(dt.Rows(0).Item("Bale_Weight_from").ToString)
                    txt_bale_weight_to.Text = Val(dt.Rows(0).Item("Bale_Weight_To").ToString)


                    txt_MeterPcs.Text = Val(dt.Rows(0).Item("Meters_Pcs").ToString)
                    cbo_StockIn.Text = dt.Rows(0).Item("Stock_In").ToString
                    cbo_WeaverWages_for.Text = dt.Rows(0).Item("WeaverWages_for").ToString
                    cbo_Weaver_Weft_Consumption.Text = dt.Rows(0).Item("Weaver_Weft_Consumption").ToString

                    txt_AllowShortage_Perc_Processing.Text = Val(dt.Rows(0).Item("Allow_Shortage_Perc").ToString)
                    txt_Weave.Text = dt.Rows(0).Item("Weave").ToString
                    Cbo_Article.Text = Common_Procedures.Article_IdNoToName(con, Val(dt.Rows(0).Item("Article_IdNo").ToString))
                    cbo_EndsCountMainName.Text = dt.Rows(0).Item("EndsCount_Name").ToString
                    cbo_ClothSet.Text = Common_Procedures.ClothSet_IdNoToName(con, Val(dt.Rows(0).Item("ClothSet_IdNo").ToString))
                    txt_TamilName.Text = dt.Rows(0)("Tamil_Name").ToString
                    txt_weight_min.Text = Val(dt.Rows(0).Item("Weight_Meter_Min").ToString)
                    txt_weight_max.Text = Val(dt.Rows(0).Item("Weight_Meter_Max").ToString)
                    txt_Weight_Meter_Fabric.Text = Val(dt.Rows(0).Item("Weight_Meter_Fabric").ToString)

                    cbo_ItemGroup.Text = dt.Rows(0).Item("ItemGroup_Name").ToString

                    txt_EPI_PPI.Text = dt.Rows(0).Item("EPI_PPI").ToString
                    cbo_fabric_name.Text = Common_Procedures.Fabric_IdNoToName(con, Val(dt.Rows(0)("Fabric_Name_idno").ToString))
                    cbo_fabric_category.Text = Common_Procedures.Fabric_Category_IdNoToName(con, Val(dt.Rows(0)("Fabric_Category_idno").ToString))

                    txt_RollTube_Wgt.Text = Val(dt.Rows(0).Item("RollTube_Wgt").ToString)
                    txt_fabric_gsm.Text = dt.Rows(0).Item("Fabric_GSM").ToString

                    cbo_Slevedge.Text = Common_Procedures.Slevedge_IdNoToName(con, Val(dt.Rows(0).Item("Slevedge_type_Idno").ToString))
                    txt_Slevedge_Waste.Text = Val(dt.Rows(0).Item("Slevedge_Waste").ToString)
                    txt_Employee_Wages_Per_Meter.Text = Val(dt.Rows(0).Item("Employee_Wages_Per_Meter").ToString)


                    cbo_loomtype.Text = Common_Procedures.LoomType_IdNoToName(con, Val(dt.Rows(0).Item("Loom_Type_idno").ToString))
                    da = New SqlClient.SqlDataAdapter("select a.*, b.EndsCount_Name from Cloth_EndsCount_Details a INNER JOIN EndsCount_Head b ON a.EndsCount_IdNo = b.EndsCount_IdNo where a.Cloth_Idno = " & Str(Val(idno)), con)
                    da.Fill(dt2)

                    dgv_EndsCountDetails.Rows.Clear()
                    slno = 0

                    If dt2.Rows.Count > 0 Then

                        For i = 0 To dt2.Rows.Count - 1

                            n = dgv_EndsCountDetails.Rows.Add()

                            slno = slno + 1
                            dgv_EndsCountDetails.Rows(n).Cells(0).Value = Val(slno)
                            dgv_EndsCountDetails.Rows(n).Cells(1).Value = dt2.Rows(i).Item("EndsCount_Name").ToString
                            dgv_EndsCountDetails.Rows(n).Cells(2).Value = dt2.Rows(i).Item("Mark").ToString

                        Next i

                        For i = 0 To dgv_EndsCountDetails.RowCount - 1
                            dgv_EndsCountDetails.Rows(i).Cells(0).Value = Val(i) + 1
                        Next

                    End If
                    dt2.Clear()
                    dt2.Dispose()


                    da = New SqlClient.SqlDataAdapter("select a.*, b.EndsCount_Name from Cloth_Bobin_Details a INNER JOIN EndsCount_Head b ON a.EndsCount_IdNo = b.EndsCount_IdNo where a.Cloth_Idno = " & Str(Val(idno)), con)
                    da.Fill(dt3)

                    dgv_BobinDetails.Rows.Clear()
                    slno = 0

                    If dt3.Rows.Count > 0 Then

                        For i = 0 To dt3.Rows.Count - 1

                            n = dgv_BobinDetails.Rows.Add()
                            dgv_BobinDetails.Rows(n).Cells(0).Value = dt3.Rows(i).Item("EndsCount_Name").ToString
                            dgv_BobinDetails.Rows(n).Cells(1).Value = Format(Val(dt3.Rows(i).Item("Cloth_Consumption").ToString), "########0.00000")

                        Next i

                    End If
                    dt3.Clear()
                    dt3.Dispose()

                    da = New SqlClient.SqlDataAdapter("select a.*, b.Count_Name from Cloth_Kuri_Details a INNER JOIN Count_Head b ON a.Count_IdNo = b.Count_IdNo where a.Cloth_Idno = " & Str(Val(idno)), con)
                    dt4 = New DataTable
                    da.Fill(dt4)

                    dgv_KuriDetails.Rows.Clear()
                    slno = 0

                    If dt4.Rows.Count > 0 Then

                        For i = 0 To dt4.Rows.Count - 1

                            n = dgv_KuriDetails.Rows.Add()


                            dgv_KuriDetails.Rows(n).Cells(0).Value = dt4.Rows(i).Item("Count_Name").ToString
                            dgv_KuriDetails.Rows(n).Cells(1).Value = Format(Val(dt4.Rows(i).Item("Cloth_Consumption").ToString), "#######0.00000")

                        Next i


                    End If
                    dt4.Clear()

                    da = New SqlClient.SqlDataAdapter("select a.*, b.Count_Name from Cloth_Additional_Weft_Details a INNER JOIN Count_Head b ON a.Count_IdNo = b.Count_IdNo where a.Cloth_Idno = " & Str(Val(idno)) & " Order by a.sl_no", con)
                    dt5 = New DataTable
                    da.Fill(dt5)

                    dgv_Additional_Weft_Details.Rows.Clear()
                    slno = 0

                    If dt5.Rows.Count > 0 Then

                        For i = 0 To dt5.Rows.Count - 1

                            n = dgv_Additional_Weft_Details.Rows.Add()

                            dgv_Additional_Weft_Details.Rows(n).Cells(0).Value = dt5.Rows(i).Item("Count_Name").ToString
                            dgv_Additional_Weft_Details.Rows(n).Cells(1).Value = dt5.Rows(i).Item("ConsumptionFor_Meters_Weight").ToString
                            dgv_Additional_Weft_Details.Rows(n).Cells(2).Value = dt5.Rows(i).Item("Gram_Perc_Type").ToString
                            dgv_Additional_Weft_Details.Rows(n).Cells(3).Value = Format(Val(dt5.Rows(i).Item("Consumption_Gram_Perc").ToString), "#######0.00")

                        Next i


                    End If
                    dt5.Clear()


                    da = New SqlClient.SqlDataAdapter("select a.*, b.EndsCount_Name from Cloth_EndsCount_Consumption_Details a INNER JOIN EndsCount_Head b ON a.EndsCount_IdNo = b.EndsCount_IdNo where a.Cloth_Idno = " & Str(Val(idno)) & " Order by a.sl_no", con)
                    dt6 = New DataTable
                    da.Fill(dt6)

                    Dgv_Warp_Count_Details.Rows.Clear()
                    slno = 0

                    If dt6.Rows.Count > 0 Then

                        For i = 0 To dt6.Rows.Count - 1

                            n = Dgv_Warp_Count_Details.Rows.Add()

                            Dgv_Warp_Count_Details.Rows(n).Cells(0).Value = Common_Procedures.EndsCount_IdNoToName(con, dt6.Rows(i).Item("EndsCount_Idno").ToString)
                            Dgv_Warp_Count_Details.Rows(n).Cells(1).Value = dt6.Rows(i).Item("Pile_Ground_Type").ToString
                            Dgv_Warp_Count_Details.Rows(n).Cells(2).Value = dt6.Rows(i).Item("Consumption_Perc").ToString


                        Next i


                    End If
                    dt6.Clear()

                    da = New SqlClient.SqlDataAdapter("Select a.* from Cloth_Master_Sales_Rate_Details a Where a.Cloth_IdNo = " & Str(Val(idno)) & " Order by a.FromDate_DateTime, a.ToDate_DateTime, a.sl_no", con)
                    dt4 = New DataTable
                    da.Fill(dt4)

                    With dgv_SalesRate_Details

                        .Rows.Clear()
                        Sno = 0

                        If dt4.Rows.Count > 0 Then

                            For i = 0 To dt4.Rows.Count - 1

                                n = .Rows.Add()

                                Sno = Sno + 1

                                .Rows(n).Cells(0).Value = Val(Sno)
                                .Rows(n).Cells(1).Value = dt4.Rows(i).Item("FromDate_Text").ToString
                                .Rows(n).Cells(2).Value = dt4.Rows(i).Item("ToDate_Text").ToString
                                .Rows(n).Cells(3).Value = Format(Val(dt4.Rows(i).Item("Type1_Sales_Rate").ToString), "########0.00")
                                .Rows(n).Cells(4).Value = Format(Val(dt4.Rows(i).Item("Type2_Sales_Rate").ToString), "########0.00")
                                .Rows(n).Cells(5).Value = Format(Val(dt4.Rows(i).Item("Type3_Sales_Rate").ToString), "########0.00")
                                .Rows(n).Cells(6).Value = Format(Val(dt4.Rows(i).Item("Type4_Sales_Rate").ToString), "########0.00")
                                .Rows(n).Cells(7).Value = Format(Val(dt4.Rows(i).Item("Type5_Sales_Rate").ToString), "########0.00")

                            Next i

                        End If
                        dt4.Clear()

                    End With

                    da = New SqlClient.SqlDataAdapter("Select a.* from Cloth_Master_Folding_Wages_Details a Where a.Cloth_IdNo = " & Str(Val(idno)) & " Order by a.sl_no ", con)
                    dt7 = New DataTable
                    da.Fill(dt7)

                    With dgv_MarkWages_Details

                        .Rows.Clear()
                        Sno = 0

                        If dt7.Rows.Count > 0 Then

                            For i = 0 To dt7.Rows.Count - 1

                                n = .Rows.Add()

                                .Rows(n).Cells(0).Value = dt7.Rows(i).Item("Cloth_Mark").ToString
                                .Rows(n).Cells(1).Value = Val(dt7.Rows(i).Item("Mark_Wages").ToString)

                            Next i

                        End If
                        dt7.Clear()

                    End With



                End If

            End If
            dt.Clear()
            dt.Dispose()






        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

        cbo_EndsCount.Tag = -1
        cbo_EndsCount.Text = ""
        cbo_EndsCount.Visible = False
        If cbo_ClothType.Enabled And cbo_ClothType.Visible Then
            cbo_ClothType.Focus()
        Else
            If txt_Name.Enabled And txt_Name.Visible Then txt_Name.Focus()
        End If



    End Sub

    Public Sub delete_record() Implements Interface_MDIActions.delete_record
        Dim cmd As New SqlClient.SqlCommand
        Dim tr As SqlClient.SqlTransaction
        Dim da As SqlClient.SqlDataAdapter
        Dim dt As DataTable


        '   If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.cloth_Creation, "~L~") = 0 And InStr(Common_Procedures.UR.cloth_Creation, "~D~") = 0 Then MessageBox.Show("You have No Rights to Delete", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error) : Exit Sub


        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.DeletingEntry, Common_Procedures.UR.cloth_Creation, New_Entry, Me) = False Then Exit Sub



        If MessageBox.Show("Do you want to Delete?", "FOR DELETION...", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.No Then
            Exit Sub
        End If
        If New_Entry = True Then
            MessageBox.Show("This is new entry", "DOES NOT DELETION....", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        da = New SqlClient.SqlDataAdapter("select count(*) from Stock_Cloth_Processing_Details where Cloth_Idno = " & Str(Val(lbl_IdNo.Text)), con)
        dt = New DataTable
        da.Fill(dt)
        If dt.Rows.Count > 0 Then
            If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                If Val(dt.Rows(0)(0).ToString) > 0 Then
                    MessageBox.Show("Already used this ClothName", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Exit Sub
                End If
            End If
        End If
        dt.Clear()

        da = New SqlClient.SqlDataAdapter("select count(*) from Beam_Knotting_Head where (Cloth_Idno1 = " & Str(Val(lbl_IdNo.Text)) & " or Cloth_Idno2 = " & Str(Val(lbl_IdNo.Text)) & " or Cloth_Idno3 = " & Str(Val(lbl_IdNo.Text)) & ")", con)
        dt = New DataTable
        da.Fill(dt)
        If dt.Rows.Count > 0 Then
            If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                If Val(dt.Rows(0)(0).ToString) > 0 Then
                    MessageBox.Show("Already used this ClothName", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Exit Sub
                End If
            End If
        End If
        dt.Clear()

        tr = con.BeginTransaction

        Try
            cmd.Connection = con
            cmd.Transaction = tr

            cmd.CommandText = "delete from Cloth_EndsCount_Consumption_Details where Cloth_Idno = " & Str(Val(lbl_IdNo.Text) & "")
            cmd.ExecuteNonQuery()

            cmd.CommandText = "delete from Cloth_Additional_Weft_Details where Cloth_Idno = " & Str(Val(lbl_IdNo.Text) & "")
            cmd.ExecuteNonQuery()

            cmd.CommandText = "delete from Cloth_Master_Sales_Rate_Details where Cloth_IdNo = " & Str(Val(lbl_IdNo.Text))
            cmd.ExecuteNonQuery()

            cmd.CommandText = "delete from Cloth_Bobin_Details where Cloth_Idno = " & Str(Val(lbl_IdNo.Text) & "")
            cmd.ExecuteNonQuery()

            cmd.CommandText = "delete from Cloth_Kuri_Details where Cloth_Idno = " & Str(Val(lbl_IdNo.Text) & "")
            cmd.ExecuteNonQuery()

            cmd.CommandText = "delete from Cloth_EndsCount_Details where Cloth_Idno = " & Str(Val(lbl_IdNo.Text) & "")
            cmd.ExecuteNonQuery()

            cmd.CommandText = "delete from Cloth_Head where Cloth_Idno = " & Str(Val(lbl_IdNo.Text))
            cmd.ExecuteNonQuery()

            tr.Commit()

            new_record()

            MessageBox.Show("Deleted Successfully!!!", "FOR DELETION...", MessageBoxButtons.OK, MessageBoxIcon.Information)

        Catch ex As Exception
            tr.Rollback()
            MessageBox.Show(ex.Message, "FOR DELETION...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        If cbo_ClothType.Enabled And cbo_ClothType.Visible Then
            cbo_ClothType.Focus()
        Else
            If txt_Name.Enabled And txt_Name.Visible Then txt_Name.Focus()
        End If

    End Sub

    Public Sub filter_record() Implements Interface_MDIActions.filter_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable

        da = New SqlClient.SqlDataAdapter("select a.Cloth_IdNo, a.Cloth_Name , a.Sort_No , a.Weave from Cloth_Head a where a.Cloth_Idno <> 0 Order by a.Cloth_Idno", con)
        da.Fill(dt)

        dgv_Filter.Columns.Clear()
        dgv_Filter.DataSource = dt

        dgv_Filter.RowHeadersVisible = False

        dgv_Filter.Columns(0).HeaderText = "IDNO"
        dgv_Filter.Columns(1).HeaderText = "ITEM NAME"

        dgv_Filter.Columns(2).HeaderText = "SORTNO"
        dgv_Filter.Columns(3).HeaderText = "WEAVE"

        dgv_Filter.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill

        dgv_Filter.Columns(0).FillWeight = 40
        dgv_Filter.Columns(1).FillWeight = 160

        dgv_Filter.Columns(0).FillWeight = 40
        dgv_Filter.Columns(1).FillWeight = 100

        pnl_Back.Enabled = False
        grp_Filter.Visible = True
        grp_Filter.BringToFront()
        dgv_Filter.Focus()

    End Sub

    Public Sub movefirst_record() Implements Interface_MDIActions.movefirst_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim movid As Integer = 0

        Try

            da = New SqlClient.SqlDataAdapter("select min(Cloth_Idno) from Cloth_Head Where Cloth_Idno <> 0", con)
            da.Fill(dt)

            movid = 0
            If dt.Rows.Count > 0 Then
                If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                    movid = Val(dt.Rows(0)(0).ToString)
                End If
            End If
            dt.Clear()
            dt.Dispose()
            da.Dispose()

            If movid <> 0 Then move_record(movid)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR  MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Public Sub movelast_record() Implements Interface_MDIActions.movelast_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim movid As Integer = 0

        Try

            da = New SqlClient.SqlDataAdapter("select max(Cloth_Idno) from Cloth_Head Where Cloth_Idno <> 0", con)
            da.Fill(dt)

            movid = 0
            If dt.Rows.Count > 0 Then
                If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                    movid = Val(dt.Rows(0)(0).ToString)
                End If
            End If
            dt.Clear()
            dt.Dispose()
            da.Dispose()

            If movid <> 0 Then move_record(movid)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR  MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Public Sub movenext_record() Implements Interface_MDIActions.movenext_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim movid As Integer = 0

        Try

            da = New SqlClient.SqlDataAdapter("select min(Cloth_Idno) from Cloth_Head Where Cloth_Idno > " & Val(lbl_IdNo.Text) & " and  Cloth_Idno <> 0", con)
            da.Fill(dt)

            movid = 0
            If dt.Rows.Count > 0 Then
                If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                    movid = Val(dt.Rows(0)(0).ToString)
                End If
            End If
            dt.Clear()
            dt.Dispose()
            da.Dispose()

            If movid <> 0 Then move_record(movid)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR  MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Public Sub moveprevious_record() Implements Interface_MDIActions.moveprevious_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim movid As Integer = 0

        Try

            da = New SqlClient.SqlDataAdapter("select max(Cloth_Idno) from Cloth_Head Where Cloth_Idno < " & Val(lbl_IdNo.Text) & " and  Cloth_Idno <> 0", con)
            da.Fill(dt)

            movid = 0
            If dt.Rows.Count > 0 Then
                If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                    movid = Val(dt.Rows(0)(0).ToString)
                End If
            End If
            dt.Clear()
            dt.Dispose()
            da.Dispose()

            If movid <> 0 Then move_record(movid)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR  MOVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try
    End Sub

    Public Sub new_record() Implements Interface_MDIActions.new_record
        Dim Da1 As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim newid As Integer = 0

        clear()

        Try

            New_Entry = True

            newid = Common_Procedures.get_MaxIdNo(con, "cloth_head", "cloth_idno", "")

            lbl_IdNo.Text = newid
            lbl_IdNo.ForeColor = Color.Red

            Da1 = New SqlClient.SqlDataAdapter("select top 1 a.*, b.Count_Name as Warp_Count, c.Count_Name as Weft_Count, IG.ItemGroup_Name from Cloth_Head a  LEFT OUTER JOIN Count_Head b ON a.Cloth_WarpCount_IdNo = b.Count_IdNo LEFT OUTER JOIN Count_Head c ON a.Cloth_WeftCount_IdNo = c.Count_IdNo LEFT OUTER JOIN ItemGroup_Head IG ON a.ItemGroup_IdNo = IG.ItemGroup_IdNo Where a.Cloth_Idno <> 0 Order by a.Cloth_Idno Desc", con)
            Dt1 = New DataTable
            Da1.Fill(Dt1)
            If Dt1.Rows.Count > 0 Then
                cbo_StockIn.Text = Dt1.Rows(0).Item("Stock_In").ToString
                cbo_WeaverWages_for.Text = Dt1.Rows(0).Item("WeaverWages_for").ToString
                cbo_ItemGroup.Text = Dt1.Rows(0).Item("ItemGroup_Name").ToString
                If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1380" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1446" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1461" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1494" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1546" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1549" Then '---- R.M TEX & FABRICS (VIJAYAMANGALAM)
                    cbo_WarpCount.Text = Dt1.Rows(0).Item("Warp_Count").ToString
                    cbo_WeftCount.Text = Dt1.Rows(0).Item("Weft_Count").ToString
                End If
            End If
            Dt1.Clear()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR NEW RECORD...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try
        If cbo_ClothType.Enabled And cbo_ClothType.Visible Then
            cbo_ClothType.Focus()
        Else

            If txt_Name.Enabled And txt_Name.Visible Then txt_Name.Focus()
        End If
        'If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1155" And Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1204" And Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1204" Then
        '    If dt.Rows(0).Item("Loom_Type").ToString <> "" Then cbo_loomtype.Text = dt.Rows(0).Item("Loom_Type").ToString
        'End If


    End Sub

    Public Sub open_record() Implements Interface_MDIActions.open_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable

        da = New SqlClient.SqlDataAdapter("select Cloth_Name from Cloth_Head order by Cloth_Name", con)
        da.Fill(dt)
        cbo_Open.DataSource = dt
        cbo_Open.DisplayMember = "Cloth_Name"

        grp_Open.Visible = True
        grp_Open.BringToFront()
        pnl_Back.Enabled = False
        If cbo_Open.Enabled And cbo_Open.Visible Then cbo_Open.Focus()

    End Sub

    Public Sub insert_record() Implements Interface_MDIActions.insert_record
        'MessageBox.Show("insert record")
    End Sub

    Public Sub print_record() Implements Interface_MDIActions.print_record
        'MessageBox.Show("print record")
    End Sub

    Public Sub save_record() Implements Interface_MDIActions.save_record
        Dim cmd As New SqlClient.SqlCommand
        Dim tr As SqlClient.SqlTransaction
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim dt2 As New DataTable
        Dim sur As String
        Dim YrnCons_For As String = ""
        Dim nr As Long = 0
        Dim WrpCnt_ID As Integer = 0
        Dim WftCnt_ID As Integer = 0
        Dim edscnt_id As Integer = 0
        Dim cnt_id As Integer = 0
        Dim Sno As Integer = 0
        Dim stk_id As Integer
        Dim Art_Id As Integer = 0
        Dim endscnt_id As Integer = 0
        Dim vendscnt_idno As Integer = 0
        Dim Transtk_Id As Integer = 0
        Dim ItemGrp_IDno As Integer = 0
        Dim ClthSet_Id As Integer = 0
        Dim loomtype_Id As Integer = 0
        Dim vSTS As Boolean = False
        Dim vToDate1STS As Boolean = False
        Dim vToDate2STS As Boolean = False
        Dim vFrmDate1 As Date
        Dim vToDate1 As Date
        Dim vFrmDate2 As Date
        Dim vToDate2 As Date
        Dim vBlank_ToDate_Count As Integer = 0
        Dim Vfab_Id As Integer = 0
        Dim VfabCatgy As Integer = 0
        Dim vCLOSESTS As Integer
        Dim vMULTIENDSCNTSTS As Integer
        Dim vMULTIWFTCNTSTS As Integer
        Dim vNONMOVESTS As Integer = 0
        Dim Clthname As String = ""
        Dim Slevedge_Id As Integer = 0

        If pnl_Back.Enabled = False Then
            MessageBox.Show("Close Other Windows", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
        End If

        'If Common_Procedures.UserRight_Check(Common_Procedures.UR.cloth_Creation, New_Entry) = False Then Exit Sub

        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.SavingEntry, Common_Procedures.UR.cloth_Creation, New_Entry, Me) = False Then Exit Sub

        If Trim(txt_Name.Text) = "" Then
            MessageBox.Show("Invalid Cloth Name", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If txt_Name.Enabled Then txt_Name.Focus()
            Exit Sub
        End If


        If cbo_ClothType.Visible = True Then
            If Trim(cbo_ClothType.Text) = "" Then
                MessageBox.Show("Invalid Cloth Type", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                If cbo_ClothType.Enabled Then cbo_ClothType.Focus()
                Exit Sub
            End If
        End If

        ItemGrp_IDno = Common_Procedures.ItemGroup_NameToIdNo(con, cbo_ItemGroup.Text)
        'If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1044" Then ' -----------------Ganesh Karthick Textiles
        If Val(ItemGrp_IDno) = 0 Then
            MessageBox.Show("Invalid Itemgroup Name(HSN Code)", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If cbo_ItemGroup.Enabled Then cbo_ItemGroup.Focus()
            Exit Sub
        End If
        'End If

        If Len(Trim(cbo_WarpCount.Text)) > 0 Then
            WrpCnt_ID = Common_Procedures.Count_NameToIdNo(con, cbo_WarpCount.Text)
        End If

        '------blocked by Deva ---- 2020-10-19
        'If Val(WrpCnt_ID) = 0 Then
        '    MessageBox.Show("Invalid Warp Count", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
        '    If cbo_WarpCount.Enabled Then cbo_WarpCount.Focus()
        '    Exit Sub
        'End If
        '/------blocked by Deva ---- 2020-10-19
        WftCnt_ID = 0
        If Len(Trim(cbo_WeftCount.Text)) > 0 Then
            WftCnt_ID = Common_Procedures.Count_NameToIdNo(con, cbo_WeftCount.Text)
        End If
        '------blocked by Deva ---- 2020-10-19
        'If Val(WftCnt_ID) = 0 Then
        '    MessageBox.Show("Invalid Weft Count", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
        '    If cbo_WeftCount.Enabled Then cbo_WeftCount.Focus()
        '    Exit Sub
        'End If
        '/------blocked by Deva ---- 2020-10-19
        Art_Id = Common_Procedures.Article_NameToIdNo(con, Cbo_Article.Text)

        ClthSet_Id = Common_Procedures.ClothSet_NameToIdNo(con, cbo_ClothSet.Text)

        Vfab_Id = Common_Procedures.Fabric_NameToIdNo(con, cbo_fabric_name.Text)
        VfabCatgy = Common_Procedures.Fabric_Category_NameToIdNo(con, cbo_fabric_category.Text)

        Slevedge_Id = Common_Procedures.Slevedge_NameToIdNo(con, cbo_Slevedge.Text)

        With dgv_EndsCountDetails
            For i = 0 To .RowCount - 1
                If Val(.Rows(i).Cells(2).Value) <> 0 Then
                    If Trim(.Rows(i).Cells(1).Value) = "" Then
                        MessageBox.Show("Invalid Ends Count Name", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        If dgv_EndsCountDetails.Enabled Then dgv_EndsCountDetails.Focus()
                        dgv_EndsCountDetails.CurrentCell = dgv_EndsCountDetails.Rows(i).Cells(1)
                        Exit Sub
                    End If
                End If
            Next
        End With


        With dgv_Additional_Weft_Details
            For i = 0 To .RowCount - 1
                If Trim(.Rows(i).Cells(0).Value) <> "" Or Val(.Rows(i).Cells(1).Value) <> 0 Then
                    cnt_id = Common_Procedures.Count_NameToIdNo(con, .Rows(i).Cells(0).Value)
                    If cnt_id = 0 Then
                        MessageBox.Show("Invalid Weft Count Name", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                        pnl_Additional_Weft_Details.Visible = True
                        pnl_Back.Enabled = False
                        If dgv_Additional_Weft_Details.Enabled Then dgv_Additional_Weft_Details.Focus()
                        dgv_Additional_Weft_Details.CurrentCell = dgv_Additional_Weft_Details.Rows(i).Cells(0)
                        Exit Sub
                    End If
                    If Val(.Rows(i).Cells(3).Value) = 0 Then
                        MessageBox.Show("Invalid Weft Consumption", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                        pnl_Additional_Weft_Details.Visible = True
                        pnl_Back.Enabled = False
                        If dgv_Additional_Weft_Details.Enabled Then dgv_Additional_Weft_Details.Focus()
                        dgv_Additional_Weft_Details.CurrentCell = dgv_Additional_Weft_Details.Rows(i).Cells(3)
                        Exit Sub
                    End If
                    'If cnt_id = WftCnt_ID Then
                    '    MessageBox.Show("Duplicate Weft Count Name", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                    '    pnl_Additional_Weft_Details.Visible = True
                    '    pnl_Back.Enabled = False
                    '    If dgv_Additional_Weft_Details.Enabled Then dgv_Additional_Weft_Details.Focus()
                    '    dgv_Additional_Weft_Details.CurrentCell = dgv_Additional_Weft_Details.Rows(i).Cells(0)
                    '    Exit Sub
                    'End If
                End If
            Next
        End With


        Clthname = Trim(txt_Name.Text)
        If Common_Procedures.settings.CustomerCode = "1186" Or Common_Procedures.settings.CustomerCode = "1428" Or Common_Procedures.settings.CustomerCode = "1461" Or Common_Procedures.settings.CustomerCode = "1464" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1494" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1530" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1546" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1549" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1569" Then

            If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1494" And Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1546" And Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1549" Then
                If Trim(txt_sortno.Text) = "" Then
                    MessageBox.Show("Invalid Sort No ", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    If txt_sortno.Enabled Then txt_sortno.Focus()
                    Exit Sub
                End If
            End If

            If Trim(txt_sortno.Text) <> "" Then
                Clthname = Trim(txt_sortno.Text) & " : " & Trim(txt_Name.Text)
            End If

        End If

        sur = Common_Procedures.Remove_NonCharacters(Trim(Clthname))

        Transtk_Id = Common_Procedures.Cloth_NameToIdNo(con, cbo_Transfer.Text, , TrnTo_DbName)
        If cbo_Transfer.Visible Then
            If Trim(cbo_Transfer.Text) <> "" Then
                If Val(Transtk_Id) = 0 Then
                    MessageBox.Show("Invalid Transfer Stock To", "DOES NOT SAVE", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    If cbo_Transfer.Enabled Then cbo_Transfer.Focus()
                    Exit Sub
                End If
            End If
        End If

        stk_id = Common_Procedures.Cloth_NameToIdNo(con, cbo_ClothGroup.Text)
        If Val(stk_id) = 0 Then
            stk_id = Val(lbl_IdNo.Text)
        End If

        loomtype_Id = Common_Procedures.LoomType_NameToIdNo(con, cbo_loomtype.Text)
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1267" Then '---- BRT Textile

            If loomtype_Id = 0 Then
                MessageBox.Show("Invalid Loom Type", "DOES NOT SAVE", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                If cbo_loomtype.Enabled Then cbo_loomtype.Focus()
                Exit Sub
            End If

        End If

        endscnt_id = Common_Procedures.EndsCount_NameToIdNo(con, cbo_EndsCountMainName.Text)
        If endscnt_id = 0 Then
            If dgv_EndsCountDetails.Rows.Count > 0 Then
                endscnt_id = Common_Procedures.EndsCount_NameToIdNo(con, dgv_EndsCountDetails.Rows(0).Cells(1).Value)
            End If
        End If
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1267" Then '---- BRT Textile
            If endscnt_id = 0 Then
                MessageBox.Show("Invalid Ends Count", "DOES NOT SAVE", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                If dgv_EndsCountDetails.Enabled And dgv_EndsCountDetails.Visible Then
                    dgv_EndsCountDetails.Focus()
                    dgv_EndsCountDetails.CurrentCell = dgv_EndsCountDetails.Rows(0).Cells(1)
                End If
                Exit Sub
            End If
        End If

        'If Val(txt_Weight_Meter_Yarn.Text) = 0 Then
        '    MessageBox.Show("Invalid Weft gram" & Chr(13) & "Does not accept zero in Weight/Meter(Weft Yarn)", "DOES NOT SAVE", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
        '    If txt_Weight_Meter_Yarn.Enabled Then txt_Weight_Meter_Yarn.Focus()
        '    Exit Sub
        'End If

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1267" Then '---- BRT Textile

            If Val(txt_Weight_Meter_Yarn.Text) = 0 Then
                MessageBox.Show("Invalid Weft gram" & Chr(13) & "Does not accept zero in Weight/Meter(Weft Yarn)", "DOES NOT SAVE", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                If txt_Weight_Meter_Yarn.Enabled Then txt_Weight_Meter_Yarn.Focus()
                Exit Sub
            End If

            'If Val(txt_Weight_Meter_Pavu.Text) = 0 Then
            '    MessageBox.Show("Invalid Warp gram" & Chr(13) & "Does not accept zero in Weight/Meter(Warp Yarn)", "DOES NOT SAVE", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            '    If txt_Weight_Meter_Pavu.Enabled Then txt_Weight_Meter_Pavu.Focus()
            '    Exit Sub
            'End If

            If Val(txt_Weight_Meter_Fabric.Text) = 0 Then
                MessageBox.Show("Invalid Piece Weight" & Chr(13) & "Does not accept zero in Weight/Meter(in KG)", "DOES NOT SAVE", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                If txt_Weight_Meter_Fabric.Enabled Then txt_Weight_Meter_Fabric.Focus()
                Exit Sub
            End If

        End If


        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1558" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1461" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1494" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1546" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1549" Then
            If Trim(UCase(cbo_StockIn.Text)) = Trim(UCase("PCS")) And Val(txt_MeterPcs.Text) = 0 And txt_MeterPcs.Enabled And txt_MeterPcs.Visible Then
                MessageBox.Show("Invalid Meters/Pc", "DOES NOT SAVE", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                If txt_MeterPcs.Enabled And txt_MeterPcs.Visible Then txt_MeterPcs.Focus()
                Exit Sub
            End If
        End If

        With dgv_SalesRate_Details

            For i = 0 To .RowCount - 1

                vFrmDate1 = #12:00:00 PM#
                vToDate1 = #12:00:00 PM#

                vToDate1STS = False

                vSTS = False
                If Trim(.Rows(i).Cells(1).Value) <> "" Then
                    If IsDate(.Rows(i).Cells(1).Value) = True Then
                        vSTS = True
                        vFrmDate1 = CDate(.Rows(i).Cells(1).Value)
                    End If
                End If

                If vSTS = True And (Val(.Rows(i).Cells(3).Value) <> 0 Or Val(.Rows(i).Cells(4).Value) <> 0 Or Val(.Rows(i).Cells(5).Value) <> 0 Or Val(.Rows(i).Cells(6).Value) <> 0 Or Val(.Rows(i).Cells(7).Value) <> 0) Then

                    vToDate1STS = False

                    If Trim(.Rows(i).Cells(2).Value) <> "" Then
                        If IsDate(.Rows(i).Cells(2).Value) = True Then
                            vToDate1STS = True
                            vToDate1 = CDate(.Rows(i).Cells(2).Value)
                        End If
                    End If

                    If vToDate1STS = False Then
                        vBlank_ToDate_Count = vBlank_ToDate_Count + 1
                        'MessageBox.Show("Invalid To Date in Rate Details", "DOES NOT SAVE", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                        'pnl_Back.Enabled = False
                        'pnl_SalesRate_Details.Visible = True
                        'If dgv_SalesRate_Details.Enabled And dgv_SalesRate_Details.Visible Then
                        '    dgv_SalesRate_Details.Focus()
                        '    dgv_SalesRate_Details.CurrentCell = dgv_SalesRate_Details.Rows(i).Cells(1)
                        'End If
                        'Exit Sub

                    Else

                        If DateDiff(DateInterval.Day, vToDate1, vFrmDate1) > 0 Then

                            MessageBox.Show("Invalid Date in Rate Details" & Chr(13) & "To Date lesser than from date", "DOES NOT SAVE", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                            pnl_Back.Enabled = False
                            pnl_SalesRate_Details.Visible = True
                            If dgv_SalesRate_Details.Enabled And dgv_SalesRate_Details.Visible Then
                                dgv_SalesRate_Details.Focus()
                                dgv_SalesRate_Details.CurrentCell = dgv_SalesRate_Details.Rows(i).Cells(1)
                            End If
                            Exit Sub

                        End If

                    End If

                    For j = i + 1 To .RowCount - 1

                        If j <> i Then

                            vFrmDate2 = #12:00:00 PM#
                            vToDate2 = #12:00:00 PM#

                            vToDate2STS = False

                            vSTS = False
                            If Trim(.Rows(j).Cells(1).Value) <> "" Then
                                If IsDate(.Rows(j).Cells(1).Value) = True Then
                                    vSTS = True
                                    vFrmDate2 = CDate(.Rows(j).Cells(1).Value)
                                End If
                            End If

                            If vSTS = True And (Val(.Rows(j).Cells(3).Value) <> 0 Or Val(.Rows(j).Cells(4).Value) <> 0 Or Val(.Rows(j).Cells(5).Value) <> 0 Or Val(.Rows(j).Cells(6).Value) <> 0 Or Val(.Rows(j).Cells(7).Value) <> 0) Then

                                If DateDiff(DateInterval.Day, vFrmDate2, vFrmDate1) > 0 Then

                                    MessageBox.Show("Invalid Date in Rate Details - from date should be grater than previous date ", "DOES NOT SAVE", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                                    pnl_Back.Enabled = False
                                    pnl_SalesRate_Details.Visible = True
                                    If dgv_SalesRate_Details.Enabled And dgv_SalesRate_Details.Visible Then
                                        dgv_SalesRate_Details.Focus()
                                        dgv_SalesRate_Details.CurrentCell = dgv_SalesRate_Details.Rows(j).Cells(1)
                                    End If
                                    Exit Sub

                                End If

                            End If

                        End If

                    Next j

                End If

            Next i

            If vBlank_ToDate_Count > 1 Then

                MessageBox.Show("Invalid To Date in Rate Details", "DOES NOT SAVE", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                pnl_Back.Enabled = False
                pnl_SalesRate_Details.Visible = True
                If dgv_SalesRate_Details.Enabled And dgv_SalesRate_Details.Visible Then
                    dgv_SalesRate_Details.Focus()
                    dgv_SalesRate_Details.CurrentCell = dgv_SalesRate_Details.Rows(0).Cells(1)
                End If
                Exit Sub
            End If

        End With


        vCLOSESTS = 0
        If chk_CloseStatus.Checked = True Then vCLOSESTS = 1

        vMULTIENDSCNTSTS = 0
        If Chk_Multi_EndsCount.Checked = True Then vMULTIENDSCNTSTS = 1

        vMULTIWFTCNTSTS = 0
        If Chk_Multi_Weft_Count.Checked = True Then vMULTIWFTCNTSTS = 1

        vNONMOVESTS = 0
        If chk_NonMoving_Cloth_Status.Checked = True Then vNONMOVESTS = 1

        tr = con.BeginTransaction

        Try


            cmd.Connection = con
            cmd.Transaction = tr

            If Trim(cbo_StockIn.Text) = "" Then
                cbo_StockIn.Text = "METER"
            End If
            If Trim(cbo_WeaverWages_for.Text) = "" Then
                cbo_WeaverWages_for.Text = "METER"
            End If

            If New_Entry = True Then

                lbl_IdNo.Text = Common_Procedures.get_MaxIdNo(con, "Cloth_Head", "Cloth_Idno", "", tr)

                cmd.CommandText = "Insert into Cloth_Head        (Cloth_Idno                    , Cloth_Name             ,Clothmain_Name       , Sur_Name            ,  Cloth_Description                 , Cloth_WarpCount_IdNo      , Cloth_WeftCount_IdNo      , Cloth_ReedSpace                       , Cloth_Reed                    , Cloth_Pick                    , Cloth_Width                       , Weight_Meter_Warp                         , Weight_Meter_Weft                         , Beam_Length                   , Tape_Length                   , Crimp_Percentage                  , Wages_For_Type1               , Wages_For_Type2                       , Wages_For_Type3               , Wages_For_Type4                   , Wages_For_Type5                   , Stock_In                      , Meters_Pcs                                , ActualCloth_Pick                      , ActualWeight_Meter_Weft                               , ActualCrimp_Percentage                    , Cloth_StockUnder_IdNo ,               Sound_Rate                    , Seconds_Rate                  ,Bits_Rate                      , Other_Rate                    ,Reject_Rate                    , Allow_Shortage_Perc               ,Cloth_Type                           ,Weave                         ,Article_IdNo           ,EndsCount_IdNo , Close_Status             ,Transfer_To_ClothIdno                      , Tamil_Name                        ,  ItemGroup_Idno            ,     Weaver_Weft_Consumption                      ,       ClothSet_IdNo    ,      Loom_Type_idno     ,                  Excess_Doffing_Meters_Percentage_Allowed   ,                        Sort_No  ,                  Weight_Meter_Min       ,           Weight_Meter_Max             ,                  Weight_Meter_Fabric          ,             Cloth_Quality_Description ,                          Bale_Weight_from     ,                            Bale_Weight_To  ,                           Wrap_waste_percentage ,                         Weft_waste_percentage   ,                                 EPI_PPI    ,                  Fabric_Name_idno      ,      Fabric_Category_idno   ,                   RollTube_Wgt           ,            Fabric_GSM,                        NonMoving_Cloth_Status   ,          Slevedge_Type_Idno       ,               WeaverWages_for             ,                     Slevedge_Waste       ,            Pavu_Consumption_In_Meter_Weight                ,             Fabric_Processing_Reconsilation_In_Meter_Weight         ,      Multiple_EndsCount_Status      ,      Multiple_WeftCount_Status      ,              Employee_Wages_Per_Meter              ,                  Checking_Wages_per_meter           ,                  folding_Wages_per_meter            ) " &
                                                 " values (" & Str(Val(lbl_IdNo.Text)) & ", '" & Trim(Clthname) & "','" & Trim(txt_Name.Text) & "' ,'" & Trim(sur) & "', '" & Trim(cbo_Description.Text) & "', " & Str(Val(WrpCnt_ID)) & ", " & Str(Val(WftCnt_ID)) & ", " & Str(Val(txt_ReedSpace.Text)) & ", " & Str(Val(txt_Reed.Text)) & ", " & Str(Val(txt_Pick.Text)) & ", " & Str(Val(txt_Width.Text)) & ", " & Str(Val(txt_Weight_Meter_Pavu.Text)) & "," & Val(txt_Weight_Meter_Yarn.Text) & "," & Val(txt_BeamLength.Text) & ", " & Val(txt_TapeLength.Text) & ", " & Val(txt_CrimpPerc.Text) & ", " & Val(txt_Coolie_Type1.Text) & ", " & Val(txt_Coolie_Type2.Text) & ", " & Val(txt_Coolie_Type3.Text) & ", " & Val(txt_Coolie_Type4.Text) & ", " & Val(txt_Coolie_Type5.Text) & ",'" & Trim(cbo_StockIn.Text) & "',   " & Str(Val(txt_MeterPcs.Text)) & " ,  " & Str(Val(txt_PickActual.Text)) & " ,  " & Str(Val(txt_Weight_Meter_YarnActual.Text)) & " ,  " & Str(Val(txt_CrimpPercActual.Text)) & " , " & Val(stk_id) & " , " & Val(txt_Type1_Rate.Text) & " , " & Val(txt_Type2_Rate.Text) & ", " & Val(txt_Type3_Rate.Text) & " ," & Val(txt_Type5_Rate.Text) & " , " & Val(txt_Type4_Rate.Text) & "," & Val(txt_AllowShortage_Perc_Processing.Text) & ",'" & Trim(cbo_ClothType.Text) & "','" & Trim(txt_Weave.Text) & "' ," & Val(Art_Id) & "," & Val(endscnt_id) & ", " & Str(Val(vCLOSESTS)) & ", " & Val(Transtk_Id) & ",'" & Trim(txt_TamilName.Text) & "' ," & Val(ItemGrp_IDno) & "   , '" & Trim(cbo_Weaver_Weft_Consumption.Text) & "'," & Val(ClthSet_Id) & "  ," & Val(loomtype_Id) & " ,  " & Val(txt_Allowed_Excess_Doff_Meters_Percentage.Text) & ", '" & Trim(txt_sortno.Text) & "' ,   " & Str(Val(txt_weight_min.Text)) & " ,  " & Str(Val(txt_weight_max.Text)) & " ,  " & Str(Val(txt_Weight_Meter_Fabric.Text)) & " , '" & Trim(cbo_quality_description.Text) & "', " & Str(Val(txt_Bale_Weight_from.Text)) & " ,  " & Str(Val(txt_bale_weight_to.Text)) & "  ,  " & Str(Val(txt_wrap_waste_percentage.Text)) & " ,   " & Str(Val(txt_weft_waste_percentage.Text)) & " ,  '" & Trim(txt_EPI_PPI.Text) & "'   ,   " & Str(Val(Vfab_Id)) & ",  " & Str(Val(VfabCatgy)) & " ,  " & Str(Val(txt_RollTube_Wgt.Text)) & " ,  " & Str(Val(txt_fabric_gsm.Text)) & ", " & Str(Val(vNONMOVESTS)) & "       ,      " & Str(Val(Slevedge_Id)) & " , '" & Trim(cbo_WeaverWages_for.Text) & "' , " & Str(Val(txt_Slevedge_Waste.Text)) & " ,  '" & Trim(Cbo_Pavu_Consumption_In_Meter_Weight.Text) & "' ,   '" & Trim(cbo_Fabric_Processing_Reconsilation_Mtrs_Wgt.Text) & "' , " & Str(Val(vMULTIENDSCNTSTS)) & "  ,  " & Str(Val(vMULTIWFTCNTSTS)) & "  ," & Str(Val(txt_Employee_Wages_Per_Meter.Text)) & " ,  " & Str(Val(txt_checking_wages_per_meter.Text)) & ",  " & Str(Val(txt_folding_wages_per_meter.Text)) & " ) "
                cmd.ExecuteNonQuery()

            Else

                cmd.CommandText = "update Cloth_Head Set Cloth_Name = '" & Trim(Clthname) & "',ClothMain_Name = '" & Trim(txt_Name.Text) & "', Sur_Name = '" & Trim(sur) & "', Cloth_Description = '" & Trim(cbo_Description.Text) & "', Cloth_WarpCount_IdNo = " & Str(Val(WrpCnt_ID)) & ", Cloth_WeftCount_IdNo = " & Str(Val(WftCnt_ID)) & ", Cloth_ReedSpace = " & Val(txt_ReedSpace.Text) & ", Cloth_Reed = " & Val(txt_Reed.Text) & ", Cloth_Pick = " & Val(txt_Pick.Text) & ", Cloth_Width = " & Val(txt_Width.Text) & ", Beam_Length = " & Str(Val(txt_BeamLength.Text)) & ", Tape_Length = " & Str(Val(txt_TapeLength.Text)) & ", Weight_Meter_Warp = " & Str(Val(txt_Weight_Meter_Pavu.Text)) & ", Weight_Meter_Weft = " & Str(Val(txt_Weight_Meter_Yarn.Text)) & ", Crimp_Percentage = " & Str(Val(txt_CrimpPerc.Text)) & ", Wages_For_Type1 = " & Val(txt_Coolie_Type1.Text) & ", Wages_For_Type2 = " & Val(txt_Coolie_Type2.Text) & ", Wages_For_Type3 = " & Val(txt_Coolie_Type3.Text) & ", Wages_For_Type4 = " & Val(txt_Coolie_Type4.Text) & ", Wages_For_Type5 = " & Val(txt_Coolie_Type5.Text) & " , Stock_In ='" & Trim(cbo_StockIn.Text) & "' , Meters_Pcs =  " & Str(Val(txt_MeterPcs.Text)) & " , ActualWeight_Meter_Weft = " & Str(Val(txt_Weight_Meter_YarnActual.Text)) & ", ActualCrimp_Percentage = " & Str(Val(txt_CrimpPercActual.Text)) & " , ActualCloth_Pick = " & Str(Val(txt_PickActual.Text)) & "  ,  Cloth_StockUnder_IdNo=" & Val(stk_id) & " ,  Sound_Rate = " & Val(txt_Type1_Rate.Text) & ", Seconds_Rate = " & Val(txt_Type2_Rate.Text) & " ,Bits_Rate = " & Val(txt_Type3_Rate.Text) & ", Other_Rate =" & Val(txt_Type5_Rate.Text) & " ,Reject_Rate =" & Val(txt_Type4_Rate.Text) & " , Allow_Shortage_Perc = " & Val(txt_AllowShortage_Perc_Processing.Text) & " ,Cloth_Type = '" & Trim(cbo_ClothType.Text) & "', Weave ='" & Trim(txt_Weave.Text) & "', Article_IdNo =" & Val(Art_Id) & " ,  EndsCount_idNo = " & Val(endscnt_id) & " , Close_Status =   " & Str(Val(vCLOSESTS)) & ",Transfer_To_ClothIdno = " & Val(Transtk_Id) & " ,Tamil_Name ='" & Trim(txt_TamilName.Text) & "',ItemGroup_IdNo = " & Str(Val(ItemGrp_IDno)) & ",Weaver_Weft_Consumption= '" & Trim(cbo_Weaver_Weft_Consumption.Text) & "',ClothSet_IdNo = " & Val(ClthSet_Id) & " ,Loom_Type_idno=" & Val(loomtype_Id) & " , Excess_Doffing_Meters_Percentage_Allowed =" & Val(txt_Allowed_Excess_Doff_Meters_Percentage.Text) & ",Sort_No = '" & Trim(txt_sortno.Text) & "',Weight_Meter_Min =" & Str(Val(txt_weight_min.Text)) & "  , Weight_Meter_Max = " & Str(Val(txt_weight_max.Text)) & " , Weight_Meter_Fabric = " & Str(Val(txt_Weight_Meter_Fabric.Text)) & ",Cloth_Quality_Description='" & Trim(cbo_quality_description.Text) & "',Bale_Weight_from =" & Str(Val(txt_Bale_Weight_from.Text)) & " , Bale_Weight_To= " & Str(Val(txt_bale_weight_to.Text)) & ", Wrap_waste_percentage =" & Str(Val(txt_wrap_waste_percentage.Text)) & " , Weft_waste_percentage = " & Str(Val(txt_weft_waste_percentage.Text)) & ",  EPI_PPI   = '" & Trim(txt_EPI_PPI.Text) & "', Fabric_Name_idno = " & Str(Val(Vfab_Id)) & " , Fabric_Category_idno= " & Str(Val(VfabCatgy)) & " , RollTube_Wgt = " & Str(Val(txt_RollTube_Wgt.Text)) & " ,  Fabric_GSM = " & Str(Val(txt_fabric_gsm.Text)) & " , NonMoving_Cloth_Status = " & Str(Val(vNONMOVESTS)) & ", Slevedge_Type_Idno = " & Str(Val(Slevedge_Id)) & " , WeaverWages_for = '" & Trim(cbo_WeaverWages_for.Text) & "'  , Slevedge_Waste = " & Str(Val(txt_Slevedge_Waste.Text)) & " , Pavu_Consumption_In_Meter_Weight = '" & Trim(Cbo_Pavu_Consumption_In_Meter_Weight.Text) & "' , Fabric_Processing_Reconsilation_In_Meter_Weight = '" & Trim(cbo_Fabric_Processing_Reconsilation_Mtrs_Wgt.Text) & "'  , Multiple_EndsCount_Status = " & Str(Val(vMULTIENDSCNTSTS)) & " , Multiple_WeftCount_Status = " & Str(Val(vMULTIWFTCNTSTS)) & " , Employee_Wages_Per_Meter = " & Str(Val(txt_Employee_Wages_Per_Meter.Text)) & " ,  Checking_Wages_per_meter = " & Str(Val(txt_checking_wages_per_meter.Text)) & ",  folding_Wages_per_meter = " & Str(Val(txt_folding_wages_per_meter.Text)) & " Where Cloth_Idno = " & Str(Val(lbl_IdNo.Text)) & ""
                nr = cmd.ExecuteNonQuery()

            End If

            cmd.CommandText = "delete from Cloth_EndsCount_Details where Cloth_Idno = " & Str(Val(lbl_IdNo.Text) & "")
            cmd.ExecuteNonQuery()

            With dgv_EndsCountDetails
                Sno = 0
                For i = 0 To .RowCount - 1

                    edscnt_id = Common_Procedures.EndsCount_NameToIdNo(con, .Rows(i).Cells(1).Value, tr)

                    If Val(edscnt_id) <> 0 Then

                        Sno = Sno + 1

                        cmd.CommandText = "Insert into Cloth_EndsCount_Details(Cloth_Idno, sl_No, EndsCount_IdNo, Mark) Values (" & Str(Val(lbl_IdNo.Text)) & ", " & Str(Val(Sno)) & ", " & Val(edscnt_id) & ", " & Val(.Rows(i).Cells(2).Value) & " )"
                        cmd.ExecuteNonQuery()
                    End If
                Next

            End With

            cmd.CommandText = "delete from Cloth_Bobin_Details where Cloth_Idno = " & Str(Val(lbl_IdNo.Text) & "")
            cmd.ExecuteNonQuery()

            With dgv_BobinDetails
                Sno = 0
                For i = 0 To .RowCount - 1

                    edscnt_id = Common_Procedures.EndsCount_NameToIdNo(con, .Rows(i).Cells(0).Value, tr)

                    If Val(edscnt_id) <> 0 Then

                        Sno = Sno + 1

                        cmd.CommandText = "Insert into Cloth_Bobin_Details(Cloth_Idno, sl_No, EndsCount_IdNo, Cloth_Consumption) Values (" & Str(Val(lbl_IdNo.Text)) & ", " & Str(Val(Sno)) & ", " & Val(edscnt_id) & ", " & Val(.Rows(i).Cells(1).Value) & " )"
                        cmd.ExecuteNonQuery()
                    End If
                Next

            End With

            cmd.CommandText = "delete from Cloth_Kuri_Details where Cloth_Idno = " & Str(Val(lbl_IdNo.Text) & "")
            cmd.ExecuteNonQuery()

            With dgv_KuriDetails
                Sno = 0
                For i = 0 To .RowCount - 1

                    cnt_id = Common_Procedures.Count_NameToIdNo(con, .Rows(i).Cells(0).Value, tr)

                    If Val(cnt_id) <> 0 Then

                        Sno = Sno + 1

                        cmd.CommandText = "Insert into Cloth_Kuri_Details(Cloth_Idno, sl_No, Count_IdNo, Cloth_Consumption) Values (" & Str(Val(lbl_IdNo.Text)) & ", " & Str(Val(Sno)) & ", " & Val(cnt_id) & ", " & Val(.Rows(i).Cells(1).Value) & " )"
                        cmd.ExecuteNonQuery()
                    End If
                Next

            End With


            cmd.CommandText = "delete from Cloth_Additional_Weft_Details where Cloth_Idno = " & Str(Val(lbl_IdNo.Text) & "")
            cmd.ExecuteNonQuery()

            With dgv_Additional_Weft_Details
                Sno = 0
                For i = 0 To .RowCount - 1

                    cnt_id = Common_Procedures.Count_NameToIdNo(con, .Rows(i).Cells(0).Value, tr)

                    If Val(cnt_id) <> 0 Then

                        Sno = Sno + 1

                        cmd.CommandText = "Insert into Cloth_Additional_Weft_Details ( Cloth_Idno, sl_No, Count_IdNo, ConsumptionFor_Meters_Weight , Gram_Perc_Type , Consumption_Gram_Perc) Values (" & Str(Val(lbl_IdNo.Text)) & ", " & Str(Val(Sno)) & ", " & Val(cnt_id) & ", '" & Trim(.Rows(i).Cells(1).Value) & "','" & Trim(.Rows(i).Cells(2).Value) & "'," & Val(.Rows(i).Cells(3).Value) & " )"
                        cmd.ExecuteNonQuery()

                    End If
                Next

            End With

            cmd.CommandText = "delete from Cloth_EndsCount_Consumption_Details where Cloth_Idno = " & Str(Val(lbl_IdNo.Text) & "")
            cmd.ExecuteNonQuery()

            With Dgv_Warp_Count_Details
                Sno = 0
                For i = 0 To .RowCount - 1


                    vendscnt_idno = Common_Procedures.EndsCount_NameToIdNo(con, .Rows(i).Cells(0).Value, tr)

                    If Val(vendscnt_idno) <> 0 Then

                        Sno = Sno + 1

                        cmd.CommandText = "Insert into Cloth_EndsCount_Consumption_Details ( Cloth_Idno, sl_No, EndsCount_IdNo, Pile_Ground_Type , Consumption_Perc) Values (" & Str(Val(lbl_IdNo.Text)) & ", " & Str(Val(Sno)) & ", " & Str(Val(vendscnt_idno)) & ", '" & Trim(.Rows(i).Cells(1).Value) & "','" & Trim(.Rows(i).Cells(2).Value) & "')"
                        cmd.ExecuteNonQuery()

                    End If

                Next

            End With

            cmd.CommandText = "delete from Cloth_Master_Sales_Rate_Details where Cloth_IdNo = " & Str(Val(lbl_IdNo.Text))
            cmd.ExecuteNonQuery()

            Sno = 0

            If btn_RateDetails.Visible = True Then

                With dgv_SalesRate_Details


                    For i = 0 To .RowCount - 1

                        vSTS = False

                        cmd.Parameters.Clear()

                        If Trim(.Rows(i).Cells(1).Value) <> "" Then
                            If IsDate(.Rows(i).Cells(1).Value) = True Then
                                cmd.Parameters.AddWithValue("@FromDate", CDate(.Rows(i).Cells(1).Value))
                                vSTS = True
                            End If
                        End If

                        If vSTS = True And (Val(.Rows(i).Cells(3).Value) <> 0 Or Val(.Rows(i).Cells(4).Value) <> 0 Or Val(.Rows(i).Cells(5).Value) <> 0 Or Val(.Rows(i).Cells(6).Value) <> 0 Or Val(.Rows(i).Cells(7).Value) <> 0) Then

                            If Trim(.Rows(i).Cells(2).Value) <> "" Then
                                If IsDate(.Rows(i).Cells(2).Value) = True Then
                                    cmd.Parameters.AddWithValue("@toDate", CDate(.Rows(i).Cells(2).Value))
                                End If
                            End If

                            Sno = Sno + 1

                            cmd.CommandText = "Insert into Cloth_Master_Sales_Rate_Details (              Cloth_IdNo        ,           Sl_No      ,                     FromDate_Text       ,                                              FromDate_DateTime           ,                    ToDate_Text          ,                                            ToDate_DateTime             ,                      Type1_Sales_Rate     ,          Type2_Sales_Rate                  ,                       Type3_Sales_Rate    ,                       Type4_Sales_Rate    ,                      Type5_Sales_Rate     )  " &
                                                " Values                                   ( " & Str(Val(lbl_IdNo.Text)) & ", " & Str(Val(Sno)) & ", '" & Trim(.Rows(i).Cells(1).Value) & "' , " & IIf(IsDate(.Rows(i).Cells(1).Value) = True, "@fromDate", "Null") & " , '" & Trim(.Rows(i).Cells(2).Value) & "' , " & IIf(IsDate(.Rows(i).Cells(2).Value) = True, "@toDate", "Null") & " , " & Str(Val(.Rows(i).Cells(3).Value)) & " ,   " & Str(Val(.Rows(i).Cells(4).Value)) & ",  " & Str(Val(.Rows(i).Cells(5).Value)) & ",  " & Str(Val(.Rows(i).Cells(6).Value)) & ", " & Str(Val(.Rows(i).Cells(7).Value)) & " ) "
                            cmd.ExecuteNonQuery()

                            cmd.CommandText = "Update Cloth_Head set Sound_Rate = " & Val(.Rows(i).Cells(3).Value) & ", Seconds_Rate = " & Val(.Rows(i).Cells(4).Value) & " , Bits_Rate = " & Val(.Rows(i).Cells(5).Value) & " , Reject_Rate =" & Val(.Rows(i).Cells(6).Value) & " , Other_Rate = " & Val(.Rows(i).Cells(7).Value) & " Where Cloth_Idno = " & Str(Val(lbl_IdNo.Text))
                            cmd.ExecuteNonQuery()

                        End If

                    Next

                End With

            Else


                Dim OpDate As Date

                OpDate = CDate("01-04-" & Microsoft.VisualBasic.Left(Trim(Common_Procedures.FnRange), 4))
                OpDate = DateAdd(DateInterval.Year, -1, OpDate)


                cmd.Parameters.Clear()

                cmd.Parameters.AddWithValue("@FromDate", OpDate)

                Sno = Sno + 1

                cmd.CommandText = "Insert into Cloth_Master_Sales_Rate_Details (              Cloth_IdNo        ,           Sl_No      ,       FromDate_Text    ,  FromDate_DateTime ,  ToDate_Text  , ToDate_DateTime ,                Type1_Sales_Rate       ,          Type2_Sales_Rate              ,               Type3_Sales_Rate        ,               Type4_Sales_Rate        ,               Type5_Sales_Rate        )  " &
                                                                   " Values      ( " & Str(Val(lbl_IdNo.Text)) & ", " & Str(Val(Sno)) & ", '" & Trim(OpDate) & "' ,      @fromDate     ,      ''       ,      Null       , " & Str(Val(txt_Type1_Rate.Text)) & " ,   " & Str(Val(txt_Type2_Rate.Text)) & ",  " & Str(Val(txt_Type3_Rate.Text)) & ",  " & Str(Val(txt_Type4_Rate.Text)) & ", " & Str(Val(txt_Type5_Rate.Text)) & " ) "
                cmd.ExecuteNonQuery()


            End If

            cmd.CommandText = "delete from Cloth_Master_Folding_Wages_Details where Cloth_IdNo = " & Str(Val(lbl_IdNo.Text))
            cmd.ExecuteNonQuery()

            Sno = 0

            If btn_Mark_WagesDetails.Visible = True Then

                With dgv_MarkWages_Details


                    For i = 0 To .RowCount - 1

                        If Val(.Rows(i).Cells(1).Value) <> 0 Then

                            Sno = Sno + 1

                            cmd.CommandText = "Insert into Cloth_Master_Folding_Wages_Details (              Cloth_IdNo        ,      Sl_No           , Cloth_Mark              ,                     Mark_Wages              )  " &
                                                " Values                                    ( " & Str(Val(lbl_IdNo.Text)) & " ,  " & Str(Val(Sno)) & ",'" & Trim(.Rows(i).Cells(0).Value) & "' , " & Str(Val(.Rows(i).Cells(1).Value)) & "  ) "
                            cmd.ExecuteNonQuery()

                        End If

                    Next

                End With


            End If



            tr.Commit()

            Common_Procedures.Master_Return.Return_Value = Trim(Clthname)
            Common_Procedures.Master_Return.Master_Type = "CLOTH"



            MessageBox.Show("Saved Successfully!!!", "FOR SAVING...", MessageBoxButtons.OK, MessageBoxIcon.Information)

            If Val(Common_Procedures.settings.OnSave_MoveTo_NewEntry_Status) = 1 Then

                If New_Entry = True Then
                    new_record()
                Else
                    move_record(lbl_IdNo.Text)
                End If

            Else
                move_record(lbl_IdNo.Text)

            End If


        Catch ex As Exception
            tr.Rollback()
            If InStr(1, Trim(LCase(ex.Message)), "ix_processed_item_salesname_head") > 0 Then
                MessageBox.Show("Duplicate Item SalesName", "DOES NOT SAVE", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Else
                MessageBox.Show(ex.Message, "FOR SAVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If

        Finally

            If cbo_ClothType.Enabled And cbo_ClothType.Visible Then
                cbo_ClothType.Focus()
            Else
                If txt_Name.Enabled And txt_Name.Visible Then txt_Name.Focus()
            End If

        End Try



    End Sub

    Protected Overrides Function ProcessCmdKey(ByRef msg As System.Windows.Forms.Message, ByVal keyData As System.Windows.Forms.Keys) As Boolean
        Dim dgv1 As New DataGridView

        If ActiveControl.Name = dgv_BobinDetails.Name Or ActiveControl.Name = dgv_SalesRate_Details.Name Or TypeOf ActiveControl Is DataGridViewTextBoxEditingControl Then

            On Error Resume Next

            dgv1 = Nothing

            If ActiveControl.Name = dgv_EndsCountDetails.Name Then
                dgv1 = dgv_EndsCountDetails

            ElseIf dgv_EndsCountDetails.IsCurrentRowDirty = True Then
                dgv1 = dgv_EndsCountDetails

            ElseIf dgv_ActiveCtrl_Name = dgv_EndsCountDetails.Name Then
                dgv1 = dgv_EndsCountDetails

            ElseIf ActiveControl.Name = dgv_BobinDetails.Name Then
                dgv1 = dgv_BobinDetails

            ElseIf ActiveControl.Name = dgv_SalesRate_Details.Name Then
                dgv1 = dgv_SalesRate_Details

            ElseIf dgv_BobinDetails.IsCurrentRowDirty = True Then
                dgv1 = dgv_BobinDetails

            ElseIf dgv_ActiveCtrl_Name = dgv_BobinDetails.Name Then
                dgv1 = dgv_BobinDetails

            ElseIf ActiveControl.Name = dgv_KuriDetails.Name Then
                dgv1 = dgv_KuriDetails

            ElseIf dgv_KuriDetails.IsCurrentRowDirty = True Then
                dgv1 = dgv_KuriDetails

            ElseIf ActiveControl.Name = dgv_Additional_Weft_Details.Name Then
                dgv1 = dgv_Additional_Weft_Details

            ElseIf dgv_Additional_Weft_Details.IsCurrentRowDirty = True Then
                dgv1 = dgv_Additional_Weft_Details

            ElseIf dgv_SalesRate_Details.IsCurrentRowDirty = True Then
                dgv1 = dgv_SalesRate_Details

            ElseIf dgv_ActiveCtrl_Name = dgv_KuriDetails.Name Then
                dgv1 = dgv_KuriDetails

            ElseIf dgv_ActiveCtrl_Name = dgv_Additional_Weft_Details.Name Then
                dgv1 = dgv_Additional_Weft_Details

            ElseIf dgv_ActiveCtrl_Name = Dgv_Warp_Count_Details.Name Then
                dgv1 = Dgv_Warp_Count_Details

            ElseIf dgv_ActiveCtrl_Name = dgv_SalesRate_Details.Name Then
                dgv1 = dgv_SalesRate_Details

            ElseIf dgv_ActiveCtrl_Name = dgv_MarkWages_Details.Name Then
                dgv1 = dgv_MarkWages_Details

            End If

            If IsNothing(dgv1) = True Then
                Return MyBase.ProcessCmdKey(msg, keyData)
                Exit Function
            End If


            With dgv1


                If dgv1.Name = dgv_EndsCountDetails.Name Then

                    If keyData = Keys.Enter Or keyData = Keys.Down Then
                        If .CurrentCell.ColumnIndex >= .ColumnCount - 1 Then
                            If .CurrentCell.RowIndex = .RowCount - 1 Then
                                cbo_EndsCountMainName.Focus()                                '

                            Else
                                .CurrentCell = .Rows(.CurrentCell.RowIndex + 1).Cells(1)

                            End If

                        Else
                            .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 1)

                        End If

                        Return True

                    ElseIf keyData = Keys.Up Then

                        If .CurrentCell.ColumnIndex <= 0 Then
                            If .CurrentCell.RowIndex = 0 Then
                                cbo_StockIn.Focus()

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

                ElseIf dgv1.Name = dgv_BobinDetails.Name Then

                    If keyData = Keys.Enter Or keyData = Keys.Down Then
                        If .CurrentCell.ColumnIndex >= .ColumnCount - 1 Then
                            If .CurrentCell.RowIndex = .RowCount - 1 Then
                                If dgv_KuriDetails.Rows.Count <= 0 Then dgv_KuriDetails.Rows.Add()
                                dgv_KuriDetails.Focus()
                                dgv_KuriDetails.CurrentCell = dgv_KuriDetails.Rows(0).Cells(1)
                                '

                            Else
                                .CurrentCell = .Rows(.CurrentCell.RowIndex + 1).Cells(0)

                            End If

                        Else
                            .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 1)

                        End If

                        Return True

                    ElseIf keyData = Keys.Up Then

                        If .CurrentCell.ColumnIndex <= 0 Then
                            If .CurrentCell.RowIndex = 0 Then
                                txt_MeterPcs.Focus()

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


                ElseIf dgv1.Name = dgv_SalesRate_Details.Name Then

                    If keyData = Keys.Enter Or keyData = Keys.Down Then
                        If .CurrentCell.ColumnIndex >= .ColumnCount - 1 Then
                            If .CurrentCell.RowIndex = .RowCount - 1 Then
                                Close_SalesRate_Details()

                            Else
                                .Focus()
                                .CurrentCell = .Rows(.CurrentCell.RowIndex + 1).Cells(1)


                            End If

                        Else

                            If .CurrentCell.RowIndex = .RowCount - 1 And .CurrentCell.ColumnIndex >= 1 And ((.CurrentCell.ColumnIndex <> 1 And Val(.CurrentRow.Cells(1).Value) = 0) Or (.CurrentCell.ColumnIndex = 1 And Val(dgtxt_SalesRate_Details.Text) = 0)) Then
                                For i = 0 To .Columns.Count - 1
                                    .Rows(.CurrentCell.RowIndex).Cells(i).Value = ""
                                Next

                                Close_SalesRate_Details()

                            ElseIf .CurrentCell.ColumnIndex = 1 Then
                                .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 2)

                            Else
                                .Focus()
                                .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 1)

                            End If


                        End If

                        Return True

                    ElseIf keyData = Keys.Up Then

                        If .CurrentCell.ColumnIndex <= 1 Then
                            If .CurrentCell.RowIndex = 0 Then
                                Close_SalesRate_Details()

                            Else
                                .CurrentCell = .Rows(.CurrentCell.RowIndex - 1).Cells(.ColumnCount - 1)

                            End If

                        ElseIf .CurrentCell.ColumnIndex = 3 Then
                            .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex - 2)

                        Else
                            .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex - 1)

                        End If

                        Return True

                    Else
                        Return MyBase.ProcessCmdKey(msg, keyData)

                    End If


                ElseIf dgv1.Name = dgv_KuriDetails.Name Then

                    If keyData = Keys.Enter Or keyData = Keys.Down Then
                        If .CurrentCell.ColumnIndex >= .ColumnCount - 1 Then
                            If .CurrentCell.RowIndex = .RowCount - 1 Then
                                btn_Save.Focus()

                            Else
                                .CurrentCell = .Rows(.CurrentCell.RowIndex + 1).Cells(0)

                            End If

                        Else
                            .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 1)

                        End If

                        Return True

                    ElseIf keyData = Keys.Up Then

                        If .CurrentCell.ColumnIndex <= 0 Then
                            If .CurrentCell.RowIndex = 0 Then
                                If dgv_BobinDetails.Rows.Count <= 0 Then dgv_BobinDetails.Rows.Add()
                                dgv_BobinDetails.Focus()
                                dgv_BobinDetails.CurrentCell = dgv_BobinDetails.Rows(0).Cells(1)

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


                ElseIf dgv1.Name = dgv_Additional_Weft_Details.Name Then

                    If keyData = Keys.Enter Or keyData = Keys.Down Then
                        If .CurrentCell.ColumnIndex >= .ColumnCount - 1 Then
                            If .CurrentCell.RowIndex = .RowCount - 1 Then
                                btn_Save.Focus()

                            Else
                                .CurrentCell = .Rows(.CurrentCell.RowIndex + 1).Cells(0)

                            End If

                        Else
                            .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 1)

                        End If

                        Return True

                    ElseIf keyData = Keys.Up Then

                        If .CurrentCell.ColumnIndex <= 0 Then
                            If .CurrentCell.RowIndex = 0 Then
                                '---

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


                ElseIf dgv1.Name = Dgv_Warp_Count_Details.Name Or dgv1.Name = dgv_MarkWages_Details.Name Then

                    If keyData = Keys.Enter Or keyData = Keys.Down Then
                        If .CurrentCell.ColumnIndex >= .ColumnCount - 1 Then
                            If .CurrentCell.RowIndex = .RowCount - 1 Then
                                btn_Save.Focus()

                            Else
                                .CurrentCell = .Rows(.CurrentCell.RowIndex + 1).Cells(0)

                            End If

                        Else
                            .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 1)

                        End If

                        Return True

                    ElseIf keyData = Keys.Up Then

                        If .CurrentCell.ColumnIndex <= 0 Then
                            If .CurrentCell.RowIndex = 0 Then
                                '---

                            Else
                                .CurrentCell = .Rows(.CurrentCell.RowIndex - 1).Cells(.ColumnCount - 1)

                            End If

                        Else
                            .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex - 1)

                        End If

                        Return True

                    ElseIf keyData = 27 Then

                        pnl_Back.Enabled = True
                        pnl_Mark_Wages.Visible = False
                        txt_Name.Focus()


                    Else
                        Return MyBase.ProcessCmdKey(msg, keyData)

                    End If

                End If
            End With

        Else

            Return MyBase.ProcessCmdKey(msg, keyData)

        End If


    End Function

    Private Sub Cloth_Creation_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated

        Dim da As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim dt3 As New DataTable
        Dim dt4 As New DataTable

        Try

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_WarpCount.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "COUNT" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                da = New SqlClient.SqlDataAdapter("select count_name from Count_Head order by count_name", con)
                da.Fill(dt1)
                cbo_WarpCount.DataSource = dt1
                cbo_WarpCount.DisplayMember = "count_name"

                cbo_WarpCount.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_WeftCount.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "COUNT" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                da = New SqlClient.SqlDataAdapter("select count_name from Count_Head order by count_name", con)
                da.Fill(dt2)
                cbo_WeftCount.DataSource = dt2
                cbo_WeftCount.DisplayMember = "count_name"

                cbo_WeftCount.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_EndsCount.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "ENDSCOUNT" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                da = New SqlClient.SqlDataAdapter("select EndsCount_Name from EndsCount_Head order by EndsCount_Name", con)
                da.Fill(dt3)
                cbo_EndsCount.DataSource = dt3
                cbo_EndsCount.DisplayMember = "EndsCount_Name"

                cbo_EndsCount.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If


            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_GridEndsCount.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "ENDSCOUNT" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                da = New SqlClient.SqlDataAdapter("select EndsCount_Name from EndsCount_Head order by EndsCount_Name", con)
                da.Fill(dt4)
                cbo_GridEndsCount.DataSource = dt4
                cbo_GridEndsCount.DisplayMember = "EndsCount_Name"

                cbo_GridEndsCount.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If



            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_GridCount.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "COUNT" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                da = New SqlClient.SqlDataAdapter("select count_name from Count_Head order by count_name", con)
                da.Fill(dt1)
                cbo_GridCount.DataSource = dt1
                cbo_GridCount.DisplayMember = "count_name"

                cbo_GridCount.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_grid_Additional_Weft_Details.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "COUNT" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                da = New SqlClient.SqlDataAdapter("select count_name from Count_Head order by count_name", con)
                da.Fill(dt1)
                cbo_grid_Additional_Weft_Details.DataSource = dt1
                cbo_grid_Additional_Weft_Details.DisplayMember = "count_name"

                cbo_grid_Additional_Weft_Details.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_ItemGroup.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "ITEMGROUP" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                da = New SqlClient.SqlDataAdapter("select ItemGroup_Name from ItemGroup_Head order by ItemGroup_Name", con)
                da.Fill(dt1)
                cbo_ItemGroup.DataSource = dt1
                cbo_ItemGroup.DisplayMember = "ItemGroup_Name"

                cbo_ItemGroup.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_ClothSet.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "CLOTHSET" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                da = New SqlClient.SqlDataAdapter("select ClothSet_Name from ClothSet_Head order by ClothSet_Name", con)
                da.Fill(dt1)
                cbo_ClothSet.DataSource = dt1
                cbo_ClothSet.DisplayMember = "ClothSet_Name"

                cbo_ClothSet.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_fabric_name.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "FABRIC" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_fabric_name.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If


            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_fabric_category.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "FABRIC CATEGORY" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_fabric_category.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Slevedge.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "SLEVEDGE" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Slevedge.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(Cbo_Article.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "ARTICLE" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                Cbo_Article.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            FrmLdSTS = False

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "DOES NOT SHOW...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub Cloth_Creation_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim TrnTo_CmpGrpIdNo As Integer = 0

        FrmLdSTS = True

        Me.Text = ""

        con.Open()

        cbo_Description.Width = 369
        cbo_Description.DropDownWidth = cbo_Description.Width

        cbo_Transfer.Visible = False
        lbl_TransferStockTo.Visible = False

        lbl_Slevedge_Waste.Visible = False
        txt_Slevedge_Waste.Visible = False

        lbl_Pavu_Consumption_In_Meter_Weight.Visible = False
        Cbo_Pavu_Consumption_In_Meter_Weight.Visible = False
        lbl_Fabric_Processing_Reconsilation_Mtrs_Wgt.Visible = False
        cbo_Fabric_Processing_Reconsilation_Mtrs_Wgt.Visible = False

        Grp_Multi_EndsCount.Visible = False
        Chk_Multi_EndsCount.Visible = False

        Grp_Multi_WeftCount.Visible = False
        Chk_Multi_Weft_Count.Visible = False

        lbl_Employee_Wages_Per_Meter.Visible = False
        txt_Employee_Wages_Per_Meter.Visible = lbl_Employee_Wages_Per_Meter.Visible

        btn_Mark_WagesDetails.Visible = False

        TrnTo_CmpGrpIdNo = Val(Common_Procedures.get_FieldValue(con, Trim(Common_Procedures.CompanyDetailsDataBaseName) & "..CompanyGroup_Head", "Transfer_To_CompanyGroupIdNo", "(CompanyGroup_IdNo = " & Str(Val(Common_Procedures.CompGroupIdNo)) & ")"))
        If Val(TrnTo_CmpGrpIdNo) <> 0 Then
            TrnTo_DbName = Common_Procedures.get_Company_DataBaseName(Trim(Val(TrnTo_CmpGrpIdNo)))
            cbo_Transfer.Visible = True
            lbl_TransferStockTo.Visible = True
            cbo_Description.Width = 120
        Else
            TrnTo_DbName = Common_Procedures.get_Company_DataBaseName(Trim(Val(Common_Procedures.CompGroupIdNo)))
        End If

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1005" Then '---- Jeno Textiles (Somanur)
            lbl_TapeLength_Caption.Text = "Weftgram for Mtr"
        End If

        lbl_Weft_Cons.Visible = False
        cbo_Weaver_Weft_Consumption.Visible = False
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1005" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1235" Then '---- Kalpana Cotton
            lbl_Weft_Cons.Visible = True
            cbo_Weaver_Weft_Consumption.Visible = True
            cbo_Weaver_Weft_Consumption.BackColor = Color.White
            cbo_Weaver_Weft_Consumption.Width = txt_MeterPcs.Width
        End If

        lbl_bale_weight_from.Visible = False
        lbl_bale_weight_to.Visible = False
        txt_Bale_Weight_from.Visible = False
        txt_bale_weight_to.Visible = False

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1267" Then '---- BRT Textile
            lbl_bale_weight_from.Visible = True
            txt_Bale_Weight_from.Visible = True
            txt_Bale_Weight_from.BackColor = Color.White
            txt_Bale_Weight_from.Width = txt_MeterPcs.Width
            lbl_bale_weight_to.Visible = True
            txt_bale_weight_to.Visible = True
            txt_bale_weight_to.BackColor = Color.White
            txt_bale_weight_to.Width = txt_MeterPcs.Width

            lbl_caption_GSM.Visible = True
            txt_fabric_gsm.Visible = True

            chk_NonMoving_Cloth_Status.Visible = True


            lbl_checking_wages_per_meter_caption.Visible = True
            lbl_checking_wages_per_meter_caption.Left = lbl_wrap_waste_percentage_caption.Left
            txt_checking_wages_per_meter.Visible = True
            txt_checking_wages_per_meter.BackColor = Color.White
            txt_checking_wages_per_meter.Left = txt_wrap_waste_percentage.Left
            txt_checking_wages_per_meter.Top = txt_wrap_waste_percentage.Top
            txt_checking_wages_per_meter.Width = txt_wrap_waste_percentage.Width

            lbl_folding_wages_per_meter_caption.Visible = True
            lbl_folding_wages_per_meter_caption.Left = lbl_weft_waste_percentage_caption.Left
            txt_folding_wages_per_meter.Visible = True
            txt_folding_wages_per_meter.BackColor = Color.White
            txt_folding_wages_per_meter.Left = txt_weft_waste_percentage.Left
            txt_folding_wages_per_meter.Top = txt_weft_waste_percentage.Top
            txt_folding_wages_per_meter.Width = txt_weft_waste_percentage.Width

        End If



        lbl_Coolie_Type1.Text = Trim(Common_Procedures.ClothType.Type1) & " Wages / Mtr"
        lbl_Coolie_Type2.Text = Trim(Common_Procedures.ClothType.Type2) & " Wages "
        lbl_Coolie_Type3.Text = Trim(Common_Procedures.ClothType.Type3) & " Wages "
        lbl_Coolie_Type4.Text = Trim(Common_Procedures.ClothType.Type4) & " Wages "
        lbl_Coolie_Type5.Text = Trim(Common_Procedures.ClothType.Type5) & " Wages "

        dgv_SalesRate_Details.Columns(3).HeaderText = Trim(UCase(Common_Procedures.ClothType.Type1))
        dgv_SalesRate_Details.Columns(4).HeaderText = Trim(UCase(Common_Procedures.ClothType.Type2))
        dgv_SalesRate_Details.Columns(5).HeaderText = Trim(UCase(Common_Procedures.ClothType.Type3))
        dgv_SalesRate_Details.Columns(6).HeaderText = Trim(UCase(Common_Procedures.ClothType.Type4))
        dgv_SalesRate_Details.Columns(7).HeaderText = Trim(UCase(Common_Procedures.ClothType.Type5))

        If Common_Procedures.settings.Weaver_Zari_Kuri_Entries_Status = 1 Then
            dgv_BobinDetails.Columns(1).HeaderText = "CONSUMPTION/PCS"
            dgv_KuriDetails.Columns(1).HeaderText = "CONSUMPTION/PCS"
        End If

        cbo_loomtype.Items.Clear()
        'cbo_loomtype.Items.Add("")
        'cbo_loomtype.Items.Add("POWER LOOM")
        'cbo_loomtype.Items.Add("AUTO LOOM")

        cbo_ClothType.Items.Clear()
        cbo_ClothType.Items.Add("GREY")
        cbo_ClothType.Items.Add("PROCESSED FABRIC")

        cbo_Weaver_Weft_Consumption.Items.Clear()
        cbo_Weaver_Weft_Consumption.Items.Add("")
        cbo_Weaver_Weft_Consumption.Items.Add("MTR")
        cbo_Weaver_Weft_Consumption.Items.Add("PCS")

        lbl_AllowShort.Visible = False
        txt_AllowShortage_Perc_Processing.Visible = False

        Cbo_Article.Visible = False
        lbl_Article.Visible = False

        lbl_Slevedge.Visible = False
        cbo_Slevedge.Visible = False



        If Val(Common_Procedures.settings.FabricProcessing_Entries_Status) = 1 Then

            lbl_IdNo.Width = cbo_WarpCount.Width

            cbo_ClothType.Visible = True
            cbo_ClothType.BackColor = Color.White

            Cbo_Article.Visible = True
            lbl_Article.Visible = True
            Cbo_Article.BackColor = Color.White

            Cbo_Article.Location = New Point(617, 358)
            lbl_Article.Location = New Point(503, 364)

            lbl_AllowShort.Visible = True
            lbl_AllowShort.Visible = True
            txt_AllowShortage_Perc_Processing.Visible = True

        Else

            'lbl_Weave.Left = lbl_Article.Left
            ''txt_Weave.Left = Cbo_Article.Left
            'txt_Weave.Width = 262

            'lbl_Slevedge.Location = New Point(502, 362)
            'txt_Slevedge.Location = New Point(617, 358)

            lbl_Slevedge.Visible = True
            cbo_Slevedge.Visible = True


        End If


        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1120" Then   '----- ALLWIN FABS (or) MARIA INTERNATIONAL (SOMANUR)

            Cbo_Article.Visible = True
            lbl_Article.Visible = True
            Cbo_Article.BackColor = Color.White

            Cbo_Article.Location = New Point(617, 358)
            lbl_Article.Location = New Point(503, 364)

            lbl_Slevedge.Visible = False
            cbo_Slevedge.Visible = False

        End If

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1413" Then '---- KAVIN GANGA (NOIDA)
            lbl_clothname.Text = "Fabric Specification"
            '  txt_Name.Left = 120
            lbl_clothname.Left = 10
            cbo_Description.Visible = False
            lbl.Visible = False
            txt_EPI_PPI.Visible = True
            cbo_fabric_name.Visible = True
            cbo_quality_description.Visible = False
            txt_Weave.Left = txt_EPI_PPI.Right
            lbl_printdescription.Text = "EPI*PPI"
            cbo_fabric_category.Visible = True
            lbl_cloth_group.Text = "Fabric Category"
            lbl_quality_descrption.Text = "Fabric Name"
            lbl_Weave.Location = New Point(275, 76)
            txt_Weave.Location = New Point(366, 71)
            txt_Weave.Width = 120

        End If


        cbo_loomtype.Visible = False
        Lbl_loomtype.Visible = False

        cbo_ClothSet.Visible = False
        lbl_ClothSet.Visible = False

        btn_RateDetails.Visible = False

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1155" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1158" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1267" Then

            cbo_loomtype.Visible = True
            Lbl_loomtype.Visible = True

            cbo_ClothSet.Visible = True
            lbl_ClothSet.Visible = True

            btn_RateDetails.Visible = True

            lbl_Type1Rate_Caption.Visible = False
            txt_Type1_Rate.Visible = False

            lbl_Type2Rate_Caption.Visible = False
            txt_Type2_Rate.Visible = False

            lbl_Type3Rate_Caption.Visible = False
            txt_Type3_Rate.Visible = False

            lbl_Type4Rate_Caption.Visible = False
            txt_Type4_Rate.Visible = False

            lbl_Type5Rate_Caption.Visible = False
            txt_Type5_Rate.Visible = False

        End If

        btn_Weft_Consumption_Details.Visible = False
        Btn_Warp_Consumption_Details.Visible = False

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1155" Then
            Grp_Multi_WeftCount.Visible = True
            Chk_Multi_Weft_Count.Visible = True

            dgv_Additional_Weft_Details.Columns(0).Width = 250
            dgv_Additional_Weft_Details.Columns(1).Width = 250
            dgv_Additional_Weft_Details.Columns(2).Visible = False
            dgv_Additional_Weft_Details.Columns(3).Visible = False

        End If

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1428" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1464" Then

            lbl_RollTube_Wgt_Caption.Visible = True
            txt_RollTube_Wgt.Visible = True

            lbl_RollTube_Wgt_Caption.Location = New Point(12, 396)
            txt_RollTube_Wgt.Location = New Point(116, 392)
            txt_RollTube_Wgt.Width = 134

        End If

        lbl_WeaverWages_for_Caption.Visible = False
        cbo_WeaverWages_for.Visible = False
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1380" Then '---- R.M TEX & FABRICS (VIJAYAMANGALAM)   OR  RM TEX & FABRICS (VIJAYAMANGALAM)
            lbl_WeaverWages_for_Caption.Visible = True
            cbo_WeaverWages_for.Visible = True
        End If

        lbl_Allowed_Excess_Doff_Meters_Percentage_Caption.Visible = False
        txt_Allowed_Excess_Doff_Meters_Percentage.Visible = False
        If Val(Common_Procedures.settings.AutoLoomStatus) = 1 Then
            lbl_Allowed_Excess_Doff_Meters_Percentage_Caption.Visible = True
            txt_Allowed_Excess_Doff_Meters_Percentage.Visible = True
        End If

        If Common_Procedures.User.IdNo <> 1 Then

            If Trim(Common_Procedures.UR.Formula_Weaver_Yarn_Consumption) = "" Then
                cbo_WeftCount.Enabled = False
                txt_ReedSpace.Enabled = False
                txt_Reed.Enabled = False
                txt_Pick.Enabled = False
                txt_Width.Enabled = False
                txt_Weight_Meter_YarnActual.Enabled = False
                btn_WeftCalculationActual.Enabled = False
                txt_Weight_Meter_Yarn.Enabled = False
                txt_Weight_Meter_Pavu.Enabled = False
                txt_Weight_Meter_Fabric.Enabled = False
                btn_WeftCalculation.Enabled = False
                Btn_wrap_calculation.Enabled = False
                txt_CrimpPerc.Enabled = False
                cbo_StockIn.Enabled = False
                cbo_WeaverWages_for.Enabled = False
            End If

            If Trim(Common_Procedures.UR.Formula_Weaver_Coolie) = "" Then
                txt_Coolie_Type1.Enabled = False
                txt_Coolie_Type2.Enabled = False
                txt_Coolie_Type3.Enabled = False
                txt_Coolie_Type4.Enabled = False
                txt_Coolie_Type5.Enabled = False

                txt_Type1_Rate.Enabled = False
                txt_Type2_Rate.Enabled = False
                txt_Type3_Rate.Enabled = False
                txt_Type4_Rate.Enabled = False
                txt_Type5_Rate.Enabled = False

            End If

        End If

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1569" Then ' --- SARATHY FABRICS 
            Label35.Text = "Po No"

            lbl_TapeLength_Caption.Visible = False
            txt_TapeLength.Visible = False

            lbl_Slevedge_Waste.Visible = True
            txt_Slevedge_Waste.Visible = True
            txt_Slevedge_Waste.BackColor = Color.White

            lbl_Slevedge_Waste.Left = lbl_TapeLength_Caption.Left
            txt_Slevedge_Waste.Left = txt_TapeLength.Left
            txt_Slevedge_Waste.Width = txt_TapeLength.Width


        End If

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1414" Then ' ----  MAHALAKSHMI TEX

            lbl_Employee_Wages_Per_Meter.Visible = True
            txt_Employee_Wages_Per_Meter.Visible = lbl_Employee_Wages_Per_Meter.Visible

            lbl_Employee_Wages_Per_Meter.Left = lbl_bale_weight_from.Left
            txt_Employee_Wages_Per_Meter.Left = txt_Bale_Weight_from.Left

            lbl_Employee_Wages_Per_Meter.Top = lbl_Slevedge.Bottom + 10
            txt_Employee_Wages_Per_Meter.Top = cbo_Slevedge.Bottom + 10

            btn_Mark_WagesDetails.Visible = True
            btn_Mark_WagesDetails.Left = lbl_bale_weight_to.Left

            btn_Mark_WagesDetails.Top = txt_Employee_Wages_Per_Meter.Top - 3

        End If


        txt_TamilName.Visible = False
        lbl_tamilname.Visible = False
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1114" Then '---- Sundara Mills
            txt_TamilName.Visible = True
            lbl_tamilname.Visible = True
        End If

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1186" Then '---- United weaves
            txt_wrap_waste_percentage.Visible = True
            txt_weft_waste_percentage.Visible = True
            lbl_wrap_waste_percentage_caption.Visible = True
            lbl_weft_waste_percentage_caption.Visible = True
        End If


        'If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1558" Then
        '    lbl_Pavu_Consumption_In_Meter_Weight.Visible = True
        '    Cbo_Pavu_Consumption_In_Meter_Weight.Visible = True

        '    Cbo_Pavu_Consumption_In_Meter_Weight.BackColor = Color.White
        '    Cbo_Pavu_Consumption_In_Meter_Weight.Left = txt_CrimpPerc.Left

        '    cbo_Fabric_Processing_Reconsilation_Mtrs_Wgt.Visible = True
        '    lbl_Fabric_Processing_Reconsilation_Mtrs_Wgt.Visible = True

        '    cbo_Fabric_Processing_Reconsilation_Mtrs_Wgt.BackColor = Color.White
        '    cbo_Fabric_Processing_Reconsilation_Mtrs_Wgt.Left = txt_Allowed_Excess_Doff_Meters_Percentage.Left

        'Else

        '    Lbl_loomtype.Left = lbl_Crimp.Left
        '    lbl_ClothSet.Left = lbl_Width.Left

        'End If

        If Common_Procedures.settings.FabricProcessing_Entries_Status = 1 Then
            lbl_Fabric_Processing_Reconsilation_Mtrs_Wgt.Visible = True
            cbo_Fabric_Processing_Reconsilation_Mtrs_Wgt.Visible = True
            cbo_Fabric_Processing_Reconsilation_Mtrs_Wgt.Width = cbo_ClothSet.Width
            cbo_Fabric_Processing_Reconsilation_Mtrs_Wgt.BackColor = Color.White
            cbo_Fabric_Processing_Reconsilation_Mtrs_Wgt.Left = txt_Allowed_Excess_Doff_Meters_Percentage.Left
        End If

        If Common_Procedures.settings.Cloth_WeftConsumption_Multiple_WeftCount_Status = 1 Then

            Grp_Multi_WeftCount.Visible = True
            Chk_Multi_Weft_Count.Visible = True

            dgv_Additional_Weft_Details.Columns(2).Visible = True
            dgv_Additional_Weft_Details.Columns(3).Visible = True

        End If


        If Common_Procedures.settings.Cloth_WarpConsumption_Multiple_EndsCount_Status = 1 Then

            Grp_Multi_EndsCount.Visible = True
            Chk_Multi_EndsCount.Visible = True

            lbl_Pavu_Consumption_In_Meter_Weight.Visible = True
            Cbo_Pavu_Consumption_In_Meter_Weight.Visible = True

        End If

        btn_Bobin.Visible = False
        If Common_Procedures.settings.Bobin_Zari_Kuri_Entries_Status = 1 Or Common_Procedures.settings.Weaver_Zari_Kuri_Entries_Status = 1 Then
            btn_Bobin.Visible = True
        End If

        grp_Open.Visible = False
        grp_Open.Left = (Me.Width - grp_Open.Width) '- 200
        grp_Open.Top = (Me.Height - grp_Open.Height) - 20

        grp_Filter.Visible = False
        grp_Filter.Left = (Me.Width - grp_Filter.Width) \ 2
        grp_Filter.Top = (Me.Height - grp_Filter.Height) \ 2

        pnl_Bobin.Visible = False
        pnl_Bobin.Left = (Me.Width - pnl_Bobin.Width) \ 2
        pnl_Bobin.Top = (Me.Height - pnl_Bobin.Height) \ 2

        pnl_Additional_Weft_Details.Visible = False
        pnl_Additional_Weft_Details.Left = (Me.Width - pnl_Additional_Weft_Details.Width) \ 2
        pnl_Additional_Weft_Details.Top = (Me.Height - pnl_Additional_Weft_Details.Height) \ 2

        Pnl_Warp_Consumption_Details.Visible = False
        Pnl_Warp_Consumption_Details.Left = (Me.Width - Pnl_Warp_Consumption_Details.Width) \ 2
        Pnl_Warp_Consumption_Details.Top = (Me.Height - Pnl_Warp_Consumption_Details.Height) \ 2

        pnl_SalesRate_Details.Visible = False
        pnl_SalesRate_Details.Left = (Me.Width - pnl_SalesRate_Details.Width) \ 2
        pnl_SalesRate_Details.Top = (Me.Height - pnl_SalesRate_Details.Height) \ 2

        pnl_Mark_Wages.Visible = False
        pnl_Mark_Wages.Left = (Me.Width - pnl_Mark_Wages.Width) \ 2
        pnl_Mark_Wages.Top = (Me.Height - pnl_Mark_Wages.Height) \ 2
        pnl_Mark_Wages.BringToFront()

        cbo_StockIn.Items.Add("")
        cbo_StockIn.Items.Add("METER")
        cbo_StockIn.Items.Add("PCS")

        Cbo_Pavu_Consumption_In_Meter_Weight.Items.Add("")
        Cbo_Pavu_Consumption_In_Meter_Weight.Items.Add("METER")
        Cbo_Pavu_Consumption_In_Meter_Weight.Items.Add("WEIGHT")

        Cbo_grid_Mts_Wgt.Items.Add("")
        Cbo_grid_Mts_Wgt.Items.Add("METER")
        Cbo_grid_Mts_Wgt.Items.Add("WEIGHT")

        Cbo_Grid_Gram_Percentage.Items.Add("")
        Cbo_Grid_Gram_Percentage.Items.Add("GRAM")
        Cbo_Grid_Gram_Percentage.Items.Add("%")

        Cbo_Grid_Pile_Ground.Items.Add("")
        Cbo_Grid_Pile_Ground.Items.Add("PILE")
        Cbo_Grid_Pile_Ground.Items.Add("GROUND")

        cbo_Fabric_Processing_Reconsilation_Mtrs_Wgt.Items.Add("")
        cbo_Fabric_Processing_Reconsilation_Mtrs_Wgt.Items.Add("METER")
        cbo_Fabric_Processing_Reconsilation_Mtrs_Wgt.Items.Add("WEIGHT")

        cbo_WeaverWages_for.Items.Add("")
        cbo_WeaverWages_for.Items.Add("METER")
        cbo_WeaverWages_for.Items.Add("PCS")

        'Cbo_Grid_Mtrs_Wgt.Items.Add("")
        'Cbo_Grid_Mtrs_Wgt.Items.Add("METER")
        'Cbo_Grid_Mtrs_Wgt.Items.Add("WEIGHT")

        AddHandler cbo_quality_description.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_ClothType.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Name.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Description.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_WarpCount.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_WeftCount.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_GridCount.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_grid_Additional_Weft_Details.GotFocus, AddressOf ControlGotFocus
        AddHandler Cbo_grid_Mts_Wgt.GotFocus, AddressOf ControlGotFocus
        AddHandler Cbo_Grid_Gram_Percentage.GotFocus, AddressOf ControlGotFocus
        AddHandler Cbo_Grid_Pile_Ground.GotFocus, AddressOf ControlGotFocus
        AddHandler Cbo_Grid_EndsCount.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_GridEndsCount.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_ClothGroup.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_EndsCountMainName.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_TamilName.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_loomtype.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_wrap_waste_percentage.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_weft_waste_percentage.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_checking_wages_per_meter.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_folding_wages_per_meter.GotFocus, AddressOf ControlGotFocus
        AddHandler Cbo_Pavu_Consumption_In_Meter_Weight.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Fabric_Processing_Reconsilation_Mtrs_Wgt.GotFocus, AddressOf ControlGotFocus

        AddHandler cbo_EndsCount.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Open.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Reed.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_ReedSpace.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Pick.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Weight_Meter_Pavu.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Weight_Meter_Yarn.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Weight_Meter_Fabric.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Width.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_BeamLength.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_TapeLength.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Coolie_Type1.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Coolie_Type2.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Coolie_Type3.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Coolie_Type4.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Coolie_Type5.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_CrimpPerc.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_StockIn.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_WeaverWages_for.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Transfer.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Weaver_Weft_Consumption.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_MeterPcs.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_PickActual.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_CrimpPercActual.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Weight_Meter_YarnActual.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_sortno.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_weight_max.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_weight_min.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_fabric_name.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_fabric_category.GotFocus, AddressOf ControlGotFocus

        AddHandler txt_Bale_Weight_from.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_bale_weight_to.GotFocus, AddressOf ControlGotFocus

        AddHandler cbo_ItemGroup.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_ClothSet.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_EPI_PPI.GotFocus, AddressOf ControlGotFocus


        AddHandler txt_Type1_Rate.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Type2_Rate.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Type3_Rate.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Type5_Rate.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Type4_Rate.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_AllowShortage_Perc_Processing.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Weave.GotFocus, AddressOf ControlGotFocus
        AddHandler Cbo_Article.GotFocus, AddressOf ControlGotFocus

        AddHandler txt_Allowed_Excess_Doff_Meters_Percentage.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_RollTube_Wgt.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_fabric_gsm.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Slevedge_Waste.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Employee_Wages_Per_Meter.GotFocus, AddressOf ControlGotFocus

        AddHandler cbo_quality_description.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Allowed_Excess_Doff_Meters_Percentage.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_loomtype.LostFocus, AddressOf ControlLostFocus
        AddHandler Cbo_Article.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_ClothType.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_ClothSet.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_AllowShortage_Perc_Processing.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Name.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Description.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_WarpCount.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_WeftCount.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_GridCount.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_grid_Additional_Weft_Details.LostFocus, AddressOf ControlLostFocus
        AddHandler Cbo_Grid_Pile_Ground.LostFocus, AddressOf ControlLostFocus
        AddHandler Cbo_Grid_EndsCount.LostFocus, AddressOf ControlLostFocus
        AddHandler Cbo_grid_Mts_Wgt.LostFocus, AddressOf ControlLostFocus
        AddHandler Cbo_Grid_Gram_Percentage.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_GridEndsCount.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_EndsCount.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Weaver_Weft_Consumption.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Open.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Reed.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_ReedSpace.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Pick.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Weight_Meter_Pavu.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Weight_Meter_Yarn.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Weight_Meter_Fabric.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Width.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_BeamLength.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_TapeLength.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Coolie_Type1.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Coolie_Type2.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Coolie_Type3.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Coolie_Type4.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Coolie_Type5.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_CrimpPerc.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_StockIn.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_WeaverWages_for.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Transfer.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_MeterPcs.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_PickActual.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_CrimpPercActual.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Weight_Meter_YarnActual.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_ClothGroup.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_EndsCountMainName.LostFocus, AddressOf ControlLostFocus
        AddHandler Cbo_Pavu_Consumption_In_Meter_Weight.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Fabric_Processing_Reconsilation_Mtrs_Wgt.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Type1_Rate.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Type2_Rate.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Type3_Rate.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Type5_Rate.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Type4_Rate.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Weave.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_TamilName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_ItemGroup.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_sortno.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_weight_max.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_weight_min.LostFocus, AddressOf ControlLostFocus

        AddHandler txt_Bale_Weight_from.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_bale_weight_to.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_wrap_waste_percentage.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_weft_waste_percentage.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_checking_wages_per_meter.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_folding_wages_per_meter.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_EPI_PPI.LostFocus, AddressOf ControlLostFocus

        AddHandler cbo_fabric_name.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_fabric_category.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_RollTube_Wgt.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_fabric_gsm.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Slevedge_Waste.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Employee_Wages_Per_Meter.LostFocus, AddressOf ControlLostFocus

        AddHandler txt_Reed.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_ReedSpace.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Pick.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Weight_Meter_Pavu.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_BeamLength.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Width.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Weight_Meter_Yarn.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Weight_Meter_Fabric.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_TapeLength.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Coolie_Type1.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Coolie_Type2.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Coolie_Type3.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Coolie_Type4.KeyDown, AddressOf TextBoxControlKeyDown
        ' AddHandler txt_Coolie_Type5.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_PickActual.KeyDown, AddressOf TextBoxControlKeyDown
        'AddHandler txt_CrimpPerc.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Weight_Meter_YarnActual.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_MeterPcs.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_weight_max.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_weight_min.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Bale_Weight_from.KeyDown, AddressOf TextBoxControlKeyDown
        'AddHandler txt_bale_weight_to.KeyDown, AddressOf TextBoxControlKeyDown

        AddHandler txt_Type1_Rate.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Type2_Rate.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Type3_Rate.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Type4_Rate.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Type5_Rate.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_sortno.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Slevedge_Waste.KeyDown, AddressOf TextBoxControlKeyDown


        AddHandler txt_Type1_Rate.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Type2_Rate.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Type3_Rate.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Type4_Rate.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Type5_Rate.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_MeterPcs.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Bale_Weight_from.KeyPress, AddressOf TextBoxControlKeyPress
        'AddHandler txt_bale_weight_to.KeyPress, AddressOf TextBoxControlKeyPress
        'AddHandler txt_Name.KeyPress, AddressOf TextBoxControlKeyPress
        ' AddHandler cbo_Description.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Reed.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_ReedSpace.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Pick.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Weight_Meter_Pavu.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Weight_Meter_Yarn.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Weight_Meter_Fabric.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Width.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_TapeLength.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_BeamLength.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Coolie_Type1.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Coolie_Type2.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Coolie_Type3.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Coolie_Type4.KeyPress, AddressOf TextBoxControlKeyPress
        ' AddHandler txt_Coolie_Type5.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_PickActual.KeyPress, AddressOf TextBoxControlKeyPress
        'AddHandler txt_CrimpPerc.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Weight_Meter_YarnActual.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_sortno.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_weight_max.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_weight_min.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Slevedge_Waste.KeyPress, AddressOf TextBoxControlKeyPress

        AddHandler cbo_Slevedge.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Slevedge.LostFocus, AddressOf ControlLostFocus



        new_record()

    End Sub

    Private Sub Cloth_Creation_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If Asc(e.KeyChar) = 27 Then

            If grp_Filter.Visible Then
                Call btn_CloseFilter_Click(sender, e)
                Exit Sub

            ElseIf pnl_SalesRate_Details.Visible Then
                Close_SalesRate_Details()
                Exit Sub

            ElseIf grp_Open.Visible Then
                Call btnClose_Click(sender, e)
                Exit Sub

            ElseIf pnl_Bobin.Visible Then
                Call btn_Close_Bobin_Click(sender, e)
                Exit Sub

            ElseIf pnl_Additional_Weft_Details.Visible Then
                Call btn_Close_Additional_Weft_Details_Click(sender, e)
                Exit Sub

            ElseIf pnl_Mark_Wages.Visible Then
                Call btn_Close_Mark_Wages_Click(sender, e)
                Exit Sub

            Else

                If MessageBox.Show("Do you want to Close?", "FOR CLOSING ENTRY...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) <> Windows.Forms.DialogResult.Yes Then
                    Exit Sub

                Else

                    Me.Close()

                End If

            End If

        End If

    End Sub


    Private Sub cbo_Open_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Open.GotFocus
        cbo_Open.BackColor = Color.Lime
        cbo_Open.ForeColor = Color.Blue
        cbo_Open.DroppedDown = True
    End Sub

    Private Sub cbo_Open_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Open.KeyPress
        Dim Indx As Integer
        Dim FindStr As String

        If Asc(e.KeyChar) = 13 Then
            btn_Find_Click(sender, e)
        End If

        If Asc(e.KeyChar) = 8 Then
            If cbo_Open.SelectionStart <= 1 Then
                cbo_Open.Text = ""
                Exit Sub
            End If

            If cbo_Open.SelectionLength = 0 Then
                FindStr = cbo_Open.Text.Substring(0, cbo_Open.Text.Length - 1)
            Else
                FindStr = cbo_Open.Text.Substring(0, cbo_Open.SelectionStart - 1)
            End If

        Else

            If cbo_Open.SelectionLength = 0 Then
                FindStr = cbo_Open.Text & e.KeyChar
            Else
                FindStr = cbo_Open.Text.Substring(0, cbo_Open.SelectionStart) & e.KeyChar
            End If

        End If

        Indx = cbo_Open.FindString(FindStr)

        If Indx <> -1 Then
            cbo_Open.SelectedText = ""
            cbo_Open.SelectedIndex = Indx
            cbo_Open.SelectionStart = FindStr.Length
            cbo_Open.SelectionLength = cbo_Open.Text.Length
        End If

        e.Handled = True

    End Sub

    Private Sub btn_Find_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Find.Click
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim movid As Integer = 0

        Try
            da = New SqlClient.SqlDataAdapter("select Cloth_Idno from Cloth_Head where Cloth_Name = '" & Trim(cbo_Open.Text) & "'", con)
            da.Fill(dt)

            If dt.Rows.Count > 0 Then
                If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                    movid = Val(dt.Rows(0)(0).ToString)
                End If
            End If

            If movid <> 0 Then
                move_record(movid)
                btnClose_Click(sender, e)
            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR FINDING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        'Me.Height = 400

    End Sub

    Private Sub btnClose_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClose.Click
        pnl_Back.Enabled = True
        grp_Open.Visible = False
    End Sub

    Private Sub cbo_WarpCount_GotFocus(sender As Object, e As System.EventArgs) Handles cbo_WarpCount.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Count_Head", "Count_Name", "", "(Count_IdNo = 0)")
    End Sub

    Private Sub cbo_WarpCount_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_WarpCount.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        If e.KeyValue = 38 And cbo_WarpCount.DroppedDown = False Then
            e.Handled = True
            ' cbo_ItemGroup.Focus()
            txt_sortno.Focus()
            'SendKeys.Send("+{TAB}")
        ElseIf e.KeyValue = 40 And cbo_WarpCount.DroppedDown = False Then
            e.Handled = True
            cbo_WeftCount.Focus()
            'SendKeys.Send("{TAB}")
        ElseIf e.KeyValue <> 13 And cbo_WarpCount.DroppedDown = False Then
            cbo_WarpCount.DroppedDown = True
        End If
    End Sub

    Private Sub cbo_WarpCount_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_WarpCount.KeyPress


        Try

            Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_WarpCount, cbo_WeftCount, "Count_Head", "Count_Name", "", "(Count_IdNo = 0)")

        Catch ex As Exception
            '---MessageBox.Show(ex.Message, "DOES NOT SELECT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub cbo_WeftCount_GotFocus(sender As Object, e As System.EventArgs) Handles cbo_WeftCount.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Count_Head", "Count_Name", "", "(Count_IdNo = 0)")
    End Sub

    Private Sub cbo_WeftCount_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_WeftCount.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        If e.KeyValue = 38 And cbo_WeftCount.DroppedDown = False Then
            e.Handled = True
            cbo_WarpCount.Focus()
            'SendKeys.Send("+{TAB}")
        ElseIf e.KeyValue = 40 And cbo_WeftCount.DroppedDown = False Then
            e.Handled = True
            txt_ReedSpace.Focus()
            'SendKeys.Send("{TAB}")
        ElseIf e.KeyValue <> 13 And cbo_WeftCount.DroppedDown = False Then
            cbo_WeftCount.DroppedDown = True
        End If
    End Sub
    Private Sub txt_BeamLength_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_BeamLength.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub btn_OpenFilter_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_OpenFilter.Click
        Dim movid As Integer = 0

        Try
            movid = Val(dgv_Filter.CurrentRow.Cells(0).Value)

            If Val(movid) <> 0 Then
                move_record(movid)
                pnl_Back.Enabled = True
                grp_Filter.Visible = False
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message)

        End Try
    End Sub

    Private Sub btn_CloseFilter_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_CloseFilter.Click
        pnl_Back.Enabled = True
        grp_Filter.Visible = False
    End Sub

    Private Sub dgv_Filter_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_Filter.KeyDown
        If e.KeyValue = 13 Then
            Call btn_OpenFilter_Click(sender, e)
        End If
    End Sub
    Private Sub txt_Weight_Meter_Pavu_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Weight_Meter_Pavu.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True

    End Sub

    Private Sub txt_CostRate_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_TapeLength.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub cbo_WeftCount_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_WeftCount.KeyPress

        Try

            Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_WeftCount, txt_ReedSpace, "Count_Head", "Count_Name", "", "(Count_IdNo = 0)")


        Catch ex As Exception
            '---MessageBox.Show(ex.Message, "DOES NOT SELECT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub txt_Weight_Meter_Yarn_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_Weight_Meter_Yarn.GotFocus

        If Val(txt_Weight_Meter_Yarn.Text) = 0 Then
            btn_WeftCalculation_Click(sender, e)
        End If

    End Sub

    Private Sub txt_Rate_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Weight_Meter_Yarn.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub cbo_WarpCount_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_WarpCount.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Dim f As New Count_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_WarpCount.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()
        End If
    End Sub

    Private Sub cbo_WeftCount_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_WeftCount.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Dim f As New Count_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_WeftCount.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()
        End If
    End Sub

    Private Sub txt_MinimumStock_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Width.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
            e.Handled = True
        End If



    End Sub

    Private Sub cbo_EndsCount_GotFocus(sender As Object, e As System.EventArgs) Handles cbo_EndsCount.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "EndsCount_Head", "EndsCount_Name", "", "(EndsCount_Idno = 0)")
    End Sub

    Private Sub cbo_EndsCount_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_EndsCount.KeyDown
        Try
            vcbo_KeyDwnVal = e.KeyValue
            With cbo_EndsCount
                If e.KeyValue = 38 And .DroppedDown = False Then
                    e.Handled = True

                    If Val(dgv_EndsCountDetails.CurrentCell.RowIndex) <= 0 Then
                        If txt_weft_waste_percentage.Visible Then
                            txt_weft_waste_percentage.Focus()
                        ElseIf txt_folding_wages_per_meter.Visible Then
                            txt_folding_wages_per_meter.Focus()
                        ElseIf cbo_ClothSet.Visible Then
                            cbo_ClothSet.Focus()
                        ElseIf cbo_loomtype.Visible Then
                            cbo_loomtype.Focus()
                        ElseIf cbo_Fabric_Processing_Reconsilation_Mtrs_Wgt.Visible Then
                            cbo_Fabric_Processing_Reconsilation_Mtrs_Wgt.Focus()

                        ElseIf txt_Allowed_Excess_Doff_Meters_Percentage.Visible Then
                            txt_Allowed_Excess_Doff_Meters_Percentage.Focus()

                        ElseIf cbo_fabric_name.Visible Then
                            cbo_fabric_name.Focus()
                        Else
                            cbo_quality_description.Focus()

                        End If

                    Else

                        dgv_EndsCountDetails.CurrentCell = dgv_EndsCountDetails.Rows(dgv_EndsCountDetails.CurrentCell.RowIndex - 1).Cells(2)
                        dgv_EndsCountDetails.CurrentCell.Selected = True
                        dgv_EndsCountDetails.Focus()
                        .Visible = False

                    End If


                ElseIf e.KeyValue = 40 And .DroppedDown = False Then

                    e.Handled = True

                    dgv_EndsCountDetails.CurrentCell = dgv_EndsCountDetails.Rows(dgv_EndsCountDetails.CurrentCell.RowIndex).Cells(dgv_EndsCountDetails.CurrentCell.ColumnIndex + 1)
                    dgv_EndsCountDetails.CurrentCell.Selected = True
                    dgv_EndsCountDetails.Focus()
                    .Visible = False

                ElseIf e.KeyValue <> 13 And e.KeyValue <> 17 And e.KeyValue <> 27 And .DroppedDown = False Then

                    .DroppedDown = True


                End If

            End With

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT SELECT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub cbo_EndsCount_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_EndsCount.KeyPress
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim Condt As String
        Dim FindStr As String

        Try

            With cbo_EndsCount

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


                        Me.dgv_EndsCountDetails.Rows(Me.dgv_EndsCountDetails.CurrentCell.RowIndex).Cells.Item(1).Value = Trim(cbo_EndsCount.Text)
                        If dgv_EndsCountDetails.CurrentRow.Index = dgv_EndsCountDetails.RowCount - 1 And dgv_EndsCountDetails.CurrentCell.ColumnIndex >= 1 And Trim(dgv_EndsCountDetails.CurrentRow.Cells(1).Value) = "" Then
                            cbo_EndsCountMainName.Focus()
                            'txt_Coolie_Type1.Focus()
                            dgv_EndsCountDetails.CurrentCell.Selected = False
                            'If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                            '    save_record()
                            'End If

                        Else
                            dgv_EndsCountDetails.CurrentCell = dgv_EndsCountDetails.Rows(dgv_EndsCountDetails.CurrentCell.RowIndex).Cells(dgv_EndsCountDetails.CurrentCell.ColumnIndex + 1)
                            dgv_EndsCountDetails.CurrentCell.Selected = True
                            dgv_EndsCountDetails.Focus()
                            cbo_EndsCount.Visible = False

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

                        Condt = ""
                        If Trim(FindStr) <> "" Then
                            Condt = " Where EndsCount_Name like '" & FindStr & "%' or EndsCount_Name like '% " & FindStr & "%' "
                        End If

                        da = New SqlClient.SqlDataAdapter("select EndsCount_Name from EndsCount_Head " & Condt & " order by EndsCount_Name", con)
                        da.Fill(dt)

                        .DataSource = dt
                        .DisplayMember = "EndsCount_Name"

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

    Private Sub dgv_EndsCountDetails_CellEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_EndsCountDetails.CellEnter
        dgv_ActiveCtrl_Name = dgv_EndsCountDetails.Name
        dgv_EndsCountDetails_GridCombo_Design()
    End Sub

    Private Sub dgv_EndsCountDetails_GridCombo_Design()
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable

        With dgv_EndsCountDetails
            If .CurrentCell.ColumnIndex = 1 Then

                If cbo_EndsCount.Visible = False Or Val(cbo_EndsCount.Tag) <> .CurrentCell.RowIndex Then

                    cbo_EndsCount.Tag = -1
                    Da = New SqlClient.SqlDataAdapter("select EndsCount_Name from EndsCount_Head order by EndsCount_Name", con)
                    Da.Fill(Dt1)
                    cbo_EndsCount.DataSource = Dt1
                    cbo_EndsCount.DisplayMember = "EndsCount_Name"

                    cbo_EndsCount.Left = .Left + .GetCellDisplayRectangle(.CurrentCell.ColumnIndex, .CurrentCell.RowIndex, False).Left
                    cbo_EndsCount.Top = .Top + .GetCellDisplayRectangle(.CurrentCell.ColumnIndex, .CurrentCell.RowIndex, False).Top
                    cbo_EndsCount.Width = .CurrentCell.Size.Width
                    cbo_EndsCount.Text = Trim(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value)

                    cbo_EndsCount.Tag = Val(.CurrentCell.RowIndex)
                    cbo_EndsCount.Visible = True

                    cbo_EndsCount.BringToFront()
                    cbo_EndsCount.Focus()

                Else
                    cbo_EndsCount.Visible = False
                    cbo_EndsCount.Tag = -1
                    cbo_EndsCount.Text = ""

                End If

            Else

                cbo_EndsCount.Visible = False
                cbo_EndsCount.Tag = -1
                cbo_EndsCount.Text = ""

            End If

        End With
    End Sub


    Private Sub dgv_EndsCountDetails_EditingControlShowing(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewEditingControlShowingEventArgs) Handles dgv_EndsCountDetails.EditingControlShowing

        Try
            With dgv_EndsCountDetails
                If .Rows.Count > 0 Then
                    dgtxt_EndsCountDetails = CType(dgv_EndsCountDetails.EditingControl, DataGridViewTextBoxEditingControl)
                End If
            End With

        Catch ex As Exception
            MessageBox.Show(ex.Message, "ERROR WHILE DETAILS EDITING SHOWING....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub dgtxt_EndsCountDetails_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgtxt_EndsCountDetails.Enter
        Try
            dgv_ActiveCtrl_Name = dgv_EndsCountDetails.Name
            dgv_EndsCountDetails.EditingControl.BackColor = Color.Lime
            dgv_EndsCountDetails.EditingControl.ForeColor = Color.Blue
            dgtxt_EndsCountDetails.SelectAll()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "ERROR WHILE DETAILS TEXT ENTER....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub dgtxt_EndsCountDetails_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dgtxt_EndsCountDetails.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub cbo_EndsCount_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_EndsCount.TextChanged
        Try
            If cbo_EndsCount.Visible Then
                If Val(cbo_EndsCount.Tag) = Val(dgv_EndsCountDetails.CurrentCell.RowIndex) And dgv_EndsCountDetails.CurrentCell.ColumnIndex = 1 Then
                    dgv_EndsCountDetails.Rows(dgv_EndsCountDetails.CurrentCell.RowIndex).Cells.Item(1).Value = Trim(cbo_EndsCount.Text)
                End If
            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub dgv_EndsCountDetails_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_EndsCountDetails.KeyUp
        Dim n As Integer
        Dim i As Integer

        Try

            If e.Control = True And UCase(Chr(e.KeyCode)) = "D" Then

                With dgv_EndsCountDetails
                    If .CurrentRow.Index = .RowCount - 1 Then
                        For i = 1 To .ColumnCount - 1
                            .Rows(.CurrentRow.Index).Cells(i).Value = ""

                        Next

                    Else

                        n = .CurrentRow.Index
                        .Rows.RemoveAt(n)
                    End If

                    For i = 0 To .Rows.Count - 1
                        .Rows(i).Cells(0).Value = i + 1

                    Next

                End With
            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub dgv_EndsCountDetails_RowsAdded(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewRowsAddedEventArgs) Handles dgv_EndsCountDetails.RowsAdded
        With dgv_EndsCountDetails
            If Val(.Rows(.RowCount - 1).Cells(0).Value) = 0 Then
                .Rows(.RowCount - 1).Cells(0).Value = .RowCount
            End If
        End With
    End Sub

    Private Sub txt_Width_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Pick.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True

    End Sub

    Private Sub txt_Cloth_Pick_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Reed.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True

    End Sub

    Private Sub txt_Cloth_ReedSpace_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_ReedSpace.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub txt_CrimpPercActual_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_CrimpPercActual.KeyDown
        If e.KeyValue = 40 Then
            If dgv_EndsCountDetails.Rows.Count <= 0 Then dgv_EndsCountDetails.Rows.Add()
            dgv_EndsCountDetails.Focus()
            dgv_EndsCountDetails.CurrentCell = dgv_EndsCountDetails.Rows(0).Cells(1)
            'SendKeys.Send("{TAB}")
        End If
        If e.KeyValue = 38 Then SendKeys.Send("+{TAB}")
        ' cbo_loomtype.Focus()

    End Sub

    Private Sub txt_CrimpPercActual_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_CrimpPercActual.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
        If Asc(e.KeyChar) = 13 Then
            If dgv_EndsCountDetails.Rows.Count <= 0 Then dgv_EndsCountDetails.Rows.Add()
            dgv_EndsCountDetails.Focus()
            dgv_EndsCountDetails.CurrentCell = dgv_EndsCountDetails.Rows(0).Cells(1)
        End If
    End Sub



    Private Sub txt_Coolie_Type1_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Coolie_Type1.KeyPress
        If Common_Procedures.Accept_NegativeNumbers(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub



    Private Sub txt_Coolie_Type2_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Coolie_Type2.KeyPress
        If Common_Procedures.Accept_NegativeNumbers(Asc(e.KeyChar)) = 0 Then e.Handled = True

    End Sub

    Private Sub txt_Coolie_Type3_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Coolie_Type3.KeyPress
        If Common_Procedures.Accept_NegativeNumbers(Asc(e.KeyChar)) = 0 Then e.Handled = True

    End Sub

    Private Sub txt_Coolie_Type4_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Coolie_Type4.KeyPress
        If Common_Procedures.Accept_NegativeNumbers(Asc(e.KeyChar)) = 0 Then e.Handled = True

    End Sub

    Private Sub txt_Coolie_Type5_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Coolie_Type5.KeyPress
        If Common_Procedures.Accept_NegativeNumbers(Asc(e.KeyChar)) = 0 Then e.Handled = True

        If Asc(e.KeyChar) = 13 Then
            If txt_fabric_gsm.Enabled And txt_fabric_gsm.Visible Then
                txt_fabric_gsm.Focus()
            ElseIf txt_Type5_Rate.Enabled And txt_Type5_Rate.Visible Then
                txt_Type5_Rate.Focus()
            Else
                txt_weight_min.Focus()
            End If
        End If
    End Sub

    Private Sub btn_Save_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Save.Click
        save_record()
    End Sub

    Private Sub btn_Close_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Close.Click
        Me.Close()
    End Sub

    Private Sub txt_otherRate_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_Type5_Rate.KeyDown
        'If e.KeyValue = 40 Then
        '    If dgv_BobinDetails.Rows.Count <= 0 Then dgv_BobinDetails.Rows.Add()
        '    dgv_BobinDetails.Focus()
        '    dgv_BobinDetails.CurrentCell = dgv_BobinDetails.Rows(0).Cells(0)
        'End If
        'If e.KeyValue = 38 Then SendKeys.Send("+{TAB}")
    End Sub

    Private Sub txt_otherRate_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Type5_Rate.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
        'If Asc(e.KeyChar) = 13 Then
        '    If dgv_BobinDetails.Rows.Count <= 0 Then dgv_BobinDetails.Rows.Add()
        '    dgv_BobinDetails.Focus()
        '    dgv_BobinDetails.CurrentCell = dgv_BobinDetails.Rows(0).Cells(0)
        'End If
    End Sub

    Private Sub cbo_StockIn_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_StockIn.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_StockIn, Nothing, Nothing, "", "", "", "")


        If (e.KeyValue = 38 And sender.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
            If txt_Type5_Rate.Enabled And txt_Type5_Rate.Visible Then
                txt_Type5_Rate.Focus()
            Else
                txt_Coolie_Type5.Focus()
            End If

        ElseIf (e.KeyValue = 40 And sender.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
            If txt_MeterPcs.Enabled And txt_MeterPcs.Visible Then
                txt_MeterPcs.Focus()
            ElseIf Cbo_Article.Enabled And Cbo_Article.Visible Then
                Cbo_Article.Focus()
            ElseIf txt_Weave.Enabled And txt_Weave.Visible Then
                txt_Weave.Focus()
            ElseIf txt_Bale_Weight_from.Enabled And txt_Bale_Weight_from.Visible Then
                txt_Bale_Weight_from.Focus()
            ElseIf txt_AllowShortage_Perc_Processing.Enabled And txt_AllowShortage_Perc_Processing.Visible Then
                txt_AllowShortage_Perc_Processing.Focus()
            Else
                If MessageBox.Show("Do you want to save", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = DialogResult.Yes Then
                    save_record()
                Else
                    txt_Name.Focus()
                End If
            End If

        End If


    End Sub

    Private Sub cbo_StockIn_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_StockIn.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_StockIn, txt_MeterPcs, "", "", "", "")

    End Sub

    Private Sub dgv_BobinDetails_CellEndEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_BobinDetails.CellEndEdit
        dgv_BobinDetails_CellLeave(sender, e)
    End Sub

    Private Sub dgv_BobinDetails_CellEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_BobinDetails.CellEnter
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim Dt2 As New DataTable
        Dim Dt3 As New DataTable
        Dim rect As Rectangle

        With dgv_BobinDetails
            dgv_ActiveCtrl_Name = .Name
            If e.ColumnIndex = 0 Then

                If cbo_GridEndsCount.Visible = False Or Val(cbo_GridEndsCount.Tag) <> e.RowIndex Then

                    cbo_GridEndsCount.Tag = -1
                    Da = New SqlClient.SqlDataAdapter("select EndsCount_Name from EndsCount_Head order by EndsCount_Name", con)
                    Dt1 = New DataTable
                    Da.Fill(Dt1)
                    cbo_GridEndsCount.DataSource = Dt1
                    cbo_GridEndsCount.DisplayMember = "EndsCount_Name"

                    rect = .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False)

                    cbo_GridEndsCount.Left = .Left + rect.Left  '  .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Left
                    cbo_GridEndsCount.Top = .Top + rect.Top  ' .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Top

                    cbo_GridEndsCount.Width = rect.Width  ' .CurrentCell.Size.Width
                    cbo_GridEndsCount.Height = rect.Height  ' rect.Height
                    cbo_GridEndsCount.Text = .CurrentCell.Value  '  Trim(.CurrentRow.Cells(e.ColumnIndex).Value)

                    cbo_GridEndsCount.Tag = Val(e.RowIndex)
                    cbo_GridEndsCount.Visible = True

                    cbo_GridEndsCount.BringToFront()
                    cbo_GridEndsCount.Focus()


                End If


            Else

                cbo_GridEndsCount.Visible = False

            End If
        End With
    End Sub

    Private Sub dgv_BobinDetails_CellLeave(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_BobinDetails.CellLeave
        With dgv_BobinDetails
            If .CurrentCell.ColumnIndex = 1 Then
                If Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value) <> 0 Then
                    .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = Format(Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value), "#########0.00000")
                End If
            End If
        End With
    End Sub

    Private Sub dgv_BobinDetails_EditingControlShowing(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewEditingControlShowingEventArgs) Handles dgv_BobinDetails.EditingControlShowing
        dgtxt_BobinDetails = CType(dgv_BobinDetails.EditingControl, DataGridViewTextBoxEditingControl)
    End Sub

    Private Sub dgv_BobinDetails_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_BobinDetails.KeyUp
        Dim i As Integer
        Dim n As Integer

        If e.Control = True And UCase(Chr(e.KeyCode)) = "D" Then

            With dgv_BobinDetails

                n = .CurrentRow.Index

                If .CurrentCell.RowIndex = .Rows.Count - 1 Then
                    For i = 0 To .ColumnCount - 1
                        .Rows(n).Cells(i).Value = ""
                    Next

                Else
                    .Rows.RemoveAt(n)

                End If

            End With

        End If
    End Sub

    Private Sub dgv_BobinDetails_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgv_BobinDetails.LostFocus
        On Error Resume Next
        dgv_BobinDetails.CurrentCell.Selected = False

    End Sub

    Private Sub cbo_GridCount_GotFocus(sender As Object, e As System.EventArgs) Handles cbo_GridCount.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Count_Head", "Count_Name", "", "(Count_Idno = 0)")
    End Sub

    Private Sub cbo_GridCount_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_GridCount.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_GridCount, Nothing, Nothing, "Count_Head", "Count_Name", "", "(Count_Idno = 0)")

        With dgv_KuriDetails

            If (e.KeyValue = 38 And cbo_GridCount.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
                If .CurrentCell.RowIndex = 0 Then
                    If dgv_BobinDetails.Rows.Count <= 0 Then dgv_BobinDetails.Rows.Add()
                    dgv_BobinDetails.Focus()
                    dgv_BobinDetails.CurrentCell = dgv_BobinDetails.Rows(0).Cells(1)

                Else
                    .Focus()
                    .CurrentCell = .Rows(.CurrentRow.Index - 1).Cells(.ColumnCount - 1)
                End If
            End If
            If (e.KeyValue = 40 And cbo_GridCount.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
                .Focus()
                .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 1)
            End If

        End With
    End Sub

    Private Sub cbo_GridCount_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_GridCount.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_GridCount, Nothing, "Count_Head", "Count_Name", "", "(Count_IdNo = 0)")
        If Asc(e.KeyChar) = 13 Then

            With dgv_KuriDetails
                If .CurrentCell.RowIndex = .Rows.Count - 1 And Trim(.Rows(.CurrentCell.RowIndex).Cells.Item(0).Value) = "" Then
                    If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                        save_record()
                    Else
                        txt_Name.Focus()
                    End If

                Else
                    .Focus()
                    .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)
                End If
            End With
        End If
    End Sub

    Private Sub cbo_GridEndsCount_GotFocus(sender As Object, e As System.EventArgs) Handles cbo_GridEndsCount.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "EndsCount_Head", "EndsCount_Name", "", "(EndsCount_Idno = 0)")
    End Sub

    Private Sub cbo_GridEndsCount_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_GridEndsCount.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_GridEndsCount, Nothing, Nothing, "EndsCount_Head", "EndsCount_Name", "", "(EndsCount_Idno = 0)")
        With dgv_BobinDetails

            If (e.KeyValue = 38 And cbo_GridEndsCount.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
                If .CurrentCell.RowIndex = 0 Then
                    'If txt_Weave.Visible And txt_Weave.Enabled Then
                    '    txt_Weave.Focus()
                    'Else
                    '    txt_MeterPcs.Focus()
                    'End If
                    'txt_SGST_Percentage.Focus()
                Else
                    .Focus()
                    .CurrentCell = .Rows(.CurrentRow.Index - 1).Cells(.ColumnCount - 1)
                End If
            End If
            If (e.KeyValue = 40 And cbo_GridEndsCount.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
                .Focus()
                .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 1)
            End If

        End With

    End Sub

    Private Sub cbo_GridEndsCount_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_GridEndsCount.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_GridEndsCount, Nothing, "EndsCount_Head", "EndsCount_Name", "", "(EndsCount_IdNo = 0)")
        If Asc(e.KeyChar) = 13 Then

            With dgv_BobinDetails
                If .CurrentCell.RowIndex = .Rows.Count - 1 And Trim(.Rows(.CurrentCell.RowIndex).Cells.Item(0).Value) = "" Then
                    If dgv_KuriDetails.Rows.Count <= 0 Then dgv_KuriDetails.Rows.Add()
                    dgv_KuriDetails.Focus()
                    dgv_KuriDetails.CurrentCell = dgv_KuriDetails.Rows(0).Cells(0)
                Else
                    .Focus()
                    .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)
                End If
            End With
        End If

    End Sub

    Private Sub cbo_GridEndsCount_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_GridEndsCount.TextChanged
        Try
            If cbo_GridEndsCount.Visible Then
                If IsNothing(dgv_BobinDetails.CurrentCell) Then Exit Sub
                With dgv_BobinDetails
                    If Val(cbo_GridEndsCount.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = 0 Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(0).Value = Trim(cbo_GridEndsCount.Text)
                    End If
                End With
            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub cbo_GridCount_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_GridCount.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Dim f As New Count_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_GridCount.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()
        End If

    End Sub

    Private Sub cbo_GridCount_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_GridCount.TextChanged
        Try
            If cbo_GridCount.Visible Then
                If IsNothing(dgv_KuriDetails.CurrentCell) Then Exit Sub
                With dgv_KuriDetails
                    If Val(cbo_GridCount.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = 0 Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(0).Value = Trim(cbo_GridCount.Text)
                    End If
                End With
            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub dgv_KuriDetails_CellEndEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_KuriDetails.CellEndEdit
        dgv_KuriDetails_CellLeave(sender, e)
    End Sub

    Private Sub dgv_KuriDetails_CellEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_KuriDetails.CellEnter
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim Dt2 As New DataTable
        Dim Dt3 As New DataTable
        Dim rect As Rectangle

        With dgv_KuriDetails

            dgv_ActiveCtrl_Name = .Name

            If e.ColumnIndex = 0 Then

                If cbo_GridCount.Visible = False Or Val(cbo_GridCount.Tag) <> e.RowIndex Then

                    cbo_GridCount.Tag = -1
                    Da = New SqlClient.SqlDataAdapter("select Count_Name from Count_Head order by Count_Name", con)
                    Dt1 = New DataTable
                    Da.Fill(Dt1)
                    cbo_GridCount.DataSource = Dt1
                    cbo_GridCount.DisplayMember = "Count_Name"

                    rect = .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False)

                    cbo_GridCount.Left = .Left + rect.Left  '  .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Left
                    cbo_GridCount.Top = .Top + rect.Top  ' .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Top

                    cbo_GridCount.Width = rect.Width  ' .CurrentCell.Size.Width
                    cbo_GridCount.Height = rect.Height  ' rect.Height
                    cbo_GridCount.Text = .CurrentCell.Value  '  Trim(.CurrentRow.Cells(e.ColumnIndex).Value)

                    cbo_GridCount.Tag = Val(e.RowIndex)
                    cbo_GridCount.Visible = True

                    cbo_GridCount.BringToFront()
                    cbo_GridCount.Focus()


                End If


            Else

                cbo_GridCount.Visible = False

            End If
        End With
    End Sub

    Private Sub dgv_KuriDetails_CellLeave(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_KuriDetails.CellLeave
        With dgv_KuriDetails
            If .CurrentCell.ColumnIndex = 1 Then
                If Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value) <> 0 Then
                    .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = Format(Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value), "#########0.00000")
                End If
            End If
        End With
    End Sub

    Private Sub dgv_KuriDetails_EditingControlShowing(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewEditingControlShowingEventArgs) Handles dgv_KuriDetails.EditingControlShowing
        dgtxt_KuriDetails = CType(dgv_KuriDetails.EditingControl, DataGridViewTextBoxEditingControl)
    End Sub

    Private Sub dgv_KuriDetails_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_KuriDetails.KeyUp
        Dim i As Integer
        Dim n As Integer

        If e.Control = True And UCase(Chr(e.KeyCode)) = "D" Then

            With dgv_KuriDetails

                n = .CurrentRow.Index

                If .CurrentCell.RowIndex = .Rows.Count - 1 Then
                    For i = 0 To .ColumnCount - 1
                        .Rows(n).Cells(i).Value = ""
                    Next

                Else
                    .Rows.RemoveAt(n)

                End If

            End With

        End If
    End Sub

    Private Sub dgv_KuriDetails_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgv_KuriDetails.LostFocus
        On Error Resume Next
        dgv_KuriDetails.CurrentCell.Selected = False
    End Sub

    Private Sub dgtxt_BobinDetails_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgtxt_BobinDetails.Enter
        dgv_ActiveCtrl_Name = dgv_BobinDetails.Name
        dgv_BobinDetails.EditingControl.BackColor = Color.Lime
        dgv_BobinDetails.EditingControl.ForeColor = Color.Blue
    End Sub

    Private Sub dgtxt_BobinDetails_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dgtxt_BobinDetails.KeyPress
        With dgv_BobinDetails
            If .Visible Then
                If .CurrentCell.ColumnIndex = 1 Then

                    If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
                        e.Handled = True
                    End If

                End If
            End If
        End With
    End Sub

    Private Sub dgtxt_KuriDetails_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgtxt_KuriDetails.Enter
        dgv_ActiveCtrl_Name = dgv_KuriDetails.Name
        dgv_KuriDetails.EditingControl.BackColor = Color.Lime
        dgv_KuriDetails.EditingControl.ForeColor = Color.Blue
    End Sub

    Private Sub dgtxt_KuriDetails_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dgtxt_KuriDetails.KeyPress
        With dgv_KuriDetails
            If .Visible Then
                If .CurrentCell.ColumnIndex = 1 Then

                    If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
                        e.Handled = True
                    End If

                End If
            End If
        End With
    End Sub

    Private Sub btn_WeftCalculation_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_WeftCalculation.Click
        Dim Wgtmtr As String = ""
        Dim wrpmtr As Single = 0
        Dim NumWftCnt As Single = 0
        Dim NumEndCnt As Single = 0
        Dim warp_count As Single = 0
        Dim WSTPERCWGT As String = ""



        '--- Consumed Yarn Formula  = "(METERS * REEDSPACE * PICK * 1.0937) / (84 * 22 * WEFT)"

        NumWftCnt = Val(Common_Procedures.get_FieldValue(con, "count_head", "Resultant_Count", "(count_name = '" & Trim(cbo_WeftCount.Text) & "')"))
        If Val(NumWftCnt) = 0 Then NumWftCnt = Val(cbo_WeftCount.Text)

        Wgtmtr = 0
        txt_Weight_Meter_Yarn.Text = ""

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1005" Then '---- Jeno Textiles (Somanur)

            '--WEFT FORMULA =  ( PICK * REEDSPACE * tapelength_meter ) / 768 

            Dim vPC_MARK As String

            If Val(txt_TapeLength.Text) <> 0 Then
                vPC_MARK = Val(txt_TapeLength.Text)
            Else
                vPC_MARK = Val(txt_MeterPcs.Text)
            End If

            Wgtmtr = ((Val(txt_Pick.Text) * Val(txt_ReedSpace.Text) * Val(vPC_MARK)) / 768) / 10

            txt_Weight_Meter_Yarn.Text = Format(Val(Wgtmtr), "#########0.0000")


        ElseIf Common_Procedures.settings.CustomerCode = "1186" Then
            If Val(NumWftCnt) <> 0 Then
                Wgtmtr = (Val(txt_ReedSpace.Text) * Val(txt_Pick.Text)) / (1693.305 * NumWftCnt)
                WSTPERCWGT = Val(Wgtmtr) * Val(txt_weft_waste_percentage.Text) / 100
                Wgtmtr = Val(Wgtmtr) + Val(WSTPERCWGT)

                txt_Weight_Meter_Yarn.Text = Format(Val(Wgtmtr), "#########0.0000")

            End If

        ElseIf Common_Procedures.settings.CustomerCode = "1438" Then

            If Val(NumWftCnt) <> 0 Then
                Wgtmtr = ((Val(txt_ReedSpace.Text) * Val(txt_Pick.Text)) / (NumWftCnt) / 1693)

                txt_Weight_Meter_Yarn.Text = Format(Val(Wgtmtr), "#########0.0000")

            End If

        ElseIf Common_Procedures.settings.CustomerCode = "1569" Then

            ' --- WEFT GRAM FORMULA = (  ( REEDSPACE + SLEVEDGEWASTE ) * PICK * 0.000591  )  /  WEFTCOUNT
            If Val(NumWftCnt) <> 0 Then

                Wgtmtr = ((Val(txt_ReedSpace.Text) + Val(txt_Slevedge_Waste.Text)) * Val(txt_Pick.Text) * 0.000591) / (NumWftCnt)
                txt_Weight_Meter_Yarn.Text = Format(Val(Wgtmtr), "#########0.0000")

            End If


        ElseIf Common_Procedures.settings.CustomerCode = "1464" Then

            If Val(NumWftCnt) <> 0 Then
                Wgtmtr = (Val(txt_ReedSpace.Text) * Val(txt_Pick.Text)) / (1692 * NumWftCnt)

                txt_Weight_Meter_Yarn.Text = Format(Val(Wgtmtr), "#########0.0000")

            End If
        ElseIf Common_Procedures.settings.CustomerCode = "1613" Then ' --- APA TEXTILE

            If Val(NumWftCnt) <> 0 Then

                Wgtmtr = (1 / (((1698 * Val(NumWftCnt)) / Val(txt_Pick.Text)) / Val(txt_ReedSpace.Text)))
                txt_Weight_Meter_Yarn.Text = Format(Val(Wgtmtr), "#########0.00000")

            End If
        Else

            If Val(NumWftCnt) <> 0 Then
                Wgtmtr = (Val(txt_ReedSpace.Text) * Val(txt_Pick.Text) * 1.0937) / (84 * 22 * NumWftCnt)
                txt_Weight_Meter_Yarn.Text = Format(Val(Wgtmtr), "#########0.0000")
            End If

        End If

        'If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1186" Then '---- United weaves
        txt_Weight_Meter_Fabric.Text = Format(Val(txt_Weight_Meter_Pavu.Text) + Val(txt_Weight_Meter_Yarn.Text), "#########0.0000")
        'End If


    End Sub

    Private Sub txt_Weight_Meter_Yarn_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txt_Weight_Meter_Yarn.TextChanged
        'Try
        '    If FrmLdSTS = True Then Exit Sub
        '    If Val(txt_Weight_Meter_Fabric.Text) > 0 Then
        '        If Me.ActiveControl.Name <> txt_Weight_Meter_Pavu.Name Then
        '            txt_Weight_Meter_Pavu.Text = Format(Val(txt_Weight_Meter_Fabric.Text) - Val(txt_Weight_Meter_Yarn.Text), "#########0.0000")
        '        End If
        '    ElseIf Val(txt_Weight_Meter_Pavu.Text) > 0 Then
        '        If Me.ActiveControl.Name <> txt_Weight_Meter_Fabric.Name Then
        '            txt_Weight_Meter_Fabric.Text = Format(Val(txt_Weight_Meter_Pavu.Text) + Val(txt_Weight_Meter_Yarn.Text), "#########0.0000")
        '        End If
        '    End If
        'Catch ex As Exception
        '    '----
        'End Try
    End Sub


    Private Sub cbo_GridEndsCount_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_GridEndsCount.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Dim f As New EndsCount_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_GridEndsCount.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()
        End If
    End Sub


    Private Sub btn_WeftCalculationActual_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_WeftCalculationActual.Click
        Dim Wgtmtr As Single = 0
        Dim NumWftCnt As Single = 0

        '--- Consumed Yarn Formula  = "(METERS * REEDSPACE * PICK * 1.0937) / (84 * 22 * WEFT)"

        NumWftCnt = Val(Common_Procedures.get_FieldValue(con, "count_head", "Resultant_Count", "(count_name = '" & Trim(cbo_WeftCount.Text) & "')"))
        If Val(NumWftCnt) = 0 Then NumWftCnt = Val(cbo_WeftCount.Text)

        Wgtmtr = 0
        txt_Weight_Meter_YarnActual.Text = ""
        If Val(NumWftCnt) <> 0 Then
            Wgtmtr = (Val(txt_ReedSpace.Text) * Val(txt_PickActual.Text) * 1.0937) / (84 * 22 * NumWftCnt)

            txt_Weight_Meter_YarnActual.Text = Format(Val(Wgtmtr), "#########0.0000")

        End If

    End Sub

    Private Sub cbo_ClothGroup_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_ClothGroup.GotFocus

        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Cloth_Head", "Cloth_Name", "", "(Cloth_IdNo = 0)")

    End Sub

    Private Sub cbo_ClothGroup_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_ClothGroup.KeyDown
        With cbo_ClothGroup
            Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_ClothGroup, Nothing, cbo_ItemGroup, "Cloth_Head", "Cloth_Name", "", "(Cloth_IdNo = 0)")
            If (e.KeyValue = 38 And .DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
                If cbo_Transfer.Visible = True Then
                    cbo_Transfer.Focus()
                Else
                    cbo_Description.Focus()
                End If
            End If
        End With

    End Sub

    Private Sub cbo_ClothGroup_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_ClothGroup.KeyPress

        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_ClothGroup, cbo_ItemGroup, "Cloth_Head", "Cloth_Name", "", "(ItemGroup_IdNo = 0)")

    End Sub

    Private Sub cbo_ItemGroup_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_ItemGroup.GotFocus

        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "ItemGroup_Head", "ItemGroup_Name", "", "(Cloth_IdNo = 0)")

    End Sub

    Private Sub cbo_ItemGroup_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_ItemGroup.KeyDown

        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_ItemGroup, Nothing, txt_sortno, "ItemGroup_Head", "ItemGroup_Name", "", "(ItemGroup_IdNo = 0)")

        If (e.KeyValue = 38 And cbo_ItemGroup.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then


            If cbo_ClothGroup.Visible = True Then
                cbo_ClothGroup.Focus()
            ElseIf cbo_Transfer.Visible = True Then
                cbo_Transfer.Focus()
            Else
                cbo_Description.Focus()
            End If


            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1413" Then
                If cbo_fabric_category.Visible = True Then
                    cbo_fabric_category.Focus()
                End If
            End If
        End If
    End Sub

    Private Sub cbo_ItemGroup_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_ItemGroup.KeyPress

        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_ItemGroup, txt_sortno, "ItemGroup_Head", "ItemGroup_Name", "", "(ItemGroup_IdNo = 0)")

    End Sub


    Private Sub txt_SoundRate_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Type1_Rate.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True

    End Sub

    Private Sub txt_secRate_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Type2_Rate.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True

    End Sub

    Private Sub txt_RejRate_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Type4_Rate.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True

    End Sub

    Private Sub txt_bitRate_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Type3_Rate.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True

    End Sub

    Private Sub Cloth_Creation_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        On Error Resume Next
        con.Close()
        con.Dispose()
        Common_Procedures.Last_Closed_FormName = Me.Name
    End Sub

    Private Sub txt_AllowShortage_Perc_Processing_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_AllowShortage_Perc_Processing.KeyDown
        If e.KeyValue = 38 Then
            If cbo_Weaver_Weft_Consumption.Visible Then
                cbo_Weaver_Weft_Consumption.Focus()
            ElseIf txt_Weave.Visible Then
                txt_Weave.Focus()
            ElseIf txt_MeterPcs.Visible Then
                txt_MeterPcs.Focus()
            Else
                txt_Type5_Rate.Focus()
            End If
        End If
        If e.KeyValue = 40 Then


            If MessageBox.Show("Do you want to save", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = DialogResult.Yes Then
                save_record()
            Else

                txt_Name.Focus()
            End If

        End If

    End Sub

    Private Sub txt_AllowShortage_Perc_Processing_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_AllowShortage_Perc_Processing.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
        If Asc(e.KeyChar) = 13 Then

            If MessageBox.Show("Do you want to save", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = DialogResult.Yes Then
                save_record()
            Else

                txt_Name.Focus()
            End If

        End If
    End Sub

    Private Sub cbo_ClothType_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_ClothType.KeyDown
        With cbo_ClothType
            Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_ClothType, Nothing, txt_Name, "", "", "", "")
            If (e.KeyValue = 38 And .DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
                If cbo_ClothSet.Visible Then
                    cbo_ClothSet.Focus()
                ElseIf Cbo_Article.Visible Then
                    Cbo_Article.Focus()
                ElseIf cbo_Weaver_Weft_Consumption.Visible Then
                    cbo_Weaver_Weft_Consumption.Focus()
                ElseIf txt_Weave.Visible Then
                    txt_Weave.Focus()
                ElseIf txt_MeterPcs.Visible Then
                    txt_MeterPcs.Focus()
                Else
                    txt_Type5_Rate.Focus()
                End If
            End If
        End With
    End Sub

    Private Sub cbo_ClothType_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_ClothType.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_ClothType, txt_Name, "", "", "", "")
    End Sub

    Private Sub Cbo_Article_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles Cbo_Article.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Article_Head", "Article_Name", "", "(Article_IdNo = 0)")
    End Sub

    Private Sub Cbo_Article_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Cbo_Article.KeyDown
        vcbo_KeyDwnVal = e.KeyCode
        With Cbo_Article
            Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, Cbo_Article, Nothing, Nothing, "Article_Head", "Article_Name", "", "(Article_IdNo = 0)")
            If (e.KeyValue = 38 And .DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
                If txt_MeterPcs.Visible = True Then
                    txt_MeterPcs.Focus()
                ElseIf cbo_StockIn.Visible Then
                    cbo_StockIn.Focus()
                Else
                    txt_Type5_Rate.Focus()
                End If

            ElseIf (e.KeyValue = 40 And .DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
                If txt_Weave.Visible = True Then
                    txt_Weave.Focus()

                ElseIf cbo_Weaver_Weft_Consumption.Visible = True Then
                    cbo_Weaver_Weft_Consumption.Focus()

                ElseIf txt_AllowShortage_Perc_Processing.Visible = True Then
                    txt_AllowShortage_Perc_Processing.Focus()

                Else
                    If MessageBox.Show("Do you want to save", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = DialogResult.Yes Then
                        save_record()
                    Else

                        txt_Name.Focus()
                    End If

                End If
            End If

        End With
    End Sub

    Private Sub Cbo_Article_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Cbo_Article.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, Cbo_Article, Nothing, "Article_Head", "Article_Name", "", "(Article_IdNo = 0)")
        If Asc(e.KeyChar) = 13 Then
            If txt_Weave.Visible = True Then
                txt_Weave.Focus()

            ElseIf cbo_Weaver_Weft_Consumption.Visible = True Then
                cbo_Weaver_Weft_Consumption.Focus()

            ElseIf txt_AllowShortage_Perc_Processing.Visible = True Then
                txt_AllowShortage_Perc_Processing.Focus()
            Else
                If MessageBox.Show("Do you want to save", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = DialogResult.Yes Then
                    save_record()
                Else

                    txt_Name.Focus()
                End If
            End If

        End If
    End Sub

    Private Sub cbo_EndscountName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_EndsCountMainName.GotFocus

        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "EndsCount_Head", "EndsCount_Name", "", "(EndsCount_IdNo = 0)")

    End Sub

    Private Sub cbo_EndsCountName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_EndsCountMainName.KeyDown

        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_EndsCountMainName, Nothing, txt_Coolie_Type1, "EndsCount_Head", "EndsCount_Name", "", "(EndsCount_IdNo = 0)")


        If (e.KeyValue = 38 And cbo_EndsCountMainName.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
            If dgv_EndsCountDetails.Rows.Count > 0 Then
                If dgv_EndsCountDetails.Rows.Count <= 0 Then dgv_EndsCountDetails.Rows.Add()
                dgv_EndsCountDetails.Focus()
                dgv_EndsCountDetails.CurrentCell = dgv_EndsCountDetails.Rows(0).Cells(1)
            Else
                If txt_weft_waste_percentage.Visible Then
                    txt_weft_waste_percentage.Focus()
                ElseIf txt_folding_wages_per_meter.Visible Then
                    txt_folding_wages_per_meter.Focus()
                ElseIf cbo_ClothSet.Visible Then
                    cbo_ClothSet.Focus()
                ElseIf cbo_Fabric_Processing_Reconsilation_Mtrs_Wgt.Visible Then
                    cbo_Fabric_Processing_Reconsilation_Mtrs_Wgt.Focus()
                Else
                    txt_Allowed_Excess_Doff_Meters_Percentage.Focus()
                End If
            End If
        End If

    End Sub

    Private Sub cbo_EndsCountName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_EndsCountMainName.KeyPress

        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_EndsCountMainName, txt_Coolie_Type1, "EndsCount_Head", "EndsCount_Name", "", "(EndsCount_IdNo = 0)")

    End Sub
    Private Sub cbo_Transfer_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Transfer.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, TrnTo_DbName & "..Cloth_Head", "Cloth_Name", "", "(Cloth_idno = 0)")
    End Sub

    Private Sub cbo_Transfer_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Transfer.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Transfer, cbo_Description, cbo_ClothGroup, TrnTo_DbName & "..Cloth_Head", "Cloth_Name", "", "(Cloth_idno = 0)")

    End Sub

    Private Sub cbo_Transfer_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Transfer.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Transfer, cbo_ClothGroup, TrnTo_DbName & "..Cloth_Head", "Cloth_Name", "", "(Cloth_idno = 0)")

    End Sub

    Private Sub cbo_Description_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Description.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Cloth_Head", "Cloth_Description", "", "(Cloth_Description <> '')")
    End Sub

    Private Sub cbo_Description_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Description.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Description, txt_Name, Nothing, "Cloth_Head", "Cloth_Description", "", "(Cloth_Description <> '')")

        If (e.KeyValue = 40 And sender.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
            If cbo_Transfer.Visible = True Then
                cbo_Transfer.Focus()
            Else
                cbo_ClothGroup.Focus()
            End If
        End If
    End Sub

    Private Sub cbo_Description_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Description.KeyPress
        If Asc(e.KeyChar) = 39 Then   '-- Single Quotes blocked
            e.Handled = True
            Exit Sub
        End If
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Description, Nothing, "Cloth_Head", "Cloth_Description", "", "(Cloth_Description <> '')", False)

        If Asc(e.KeyChar) = 13 Then

            If cbo_Transfer.Visible = True Then
                cbo_Transfer.Focus()
            Else
                cbo_ClothGroup.Focus()
            End If
        End If

    End Sub

    Private Sub cbo_quality_description_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_quality_description.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Cloth_Head", "Cloth_quality_description", "", "(Cloth_quality_description <> '')")
    End Sub

    Private Sub cbo_quality_description_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_quality_description.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_quality_description, txt_Name, Nothing, "Cloth_Head", "Cloth_quality_description", "", "(Cloth_quality_description <> '')")

        If (e.KeyValue = 38 And cbo_quality_description.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
            txt_CrimpPerc.Focus()

        End If

        If (e.KeyValue = 40 And cbo_quality_description.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
            If dgv_EndsCountDetails.Rows.Count > 0 Then
                If dgv_EndsCountDetails.Rows.Count <= 0 Then dgv_EndsCountDetails.Rows.Add()
                dgv_EndsCountDetails.Focus()
                dgv_EndsCountDetails.CurrentCell = dgv_EndsCountDetails.Rows(0).Cells(1)
            End If
        End If

    End Sub

    Private Sub cbo_quality_description_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_quality_description.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_quality_description, Nothing, "Cloth_Head", "Cloth_quality_description", "", "(Cloth_quality_description <> '')", False)

        If Asc(e.KeyChar) = 13 Then
            If dgv_EndsCountDetails.Rows.Count > 0 Then
                If dgv_EndsCountDetails.Rows.Count <= 0 Then dgv_EndsCountDetails.Rows.Add()
                dgv_EndsCountDetails.Focus()
                dgv_EndsCountDetails.CurrentCell = dgv_EndsCountDetails.Rows(0).Cells(1)
            End If
        End If

    End Sub

    'Private Sub txt_Weave_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_Weave.KeyDown
    '    If e.KeyValue = 40 Then
    '        If txt_AllowShortage.Visible = True Then
    '            txt_AllowShortage.Focus()

    '        ElseIf Cbo_Article.Visible = True Then
    '            Cbo_Article.Focus()

    '        Else
    '            txt_HSN_Code.Focus()
    '        End If

    '    End If
    '    If e.KeyValue = 38 Then SendKeys.Send("+{TAB}")


    'End Sub

    'Private Sub txt_Weave_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Weave.KeyPress
    '    If Asc(e.KeyChar) = 13 Then
    '        If Cbo_Article.Visible = True Then
    '            Cbo_Article.Focus()
    '        Else

    '            txt_HSN_Code.Focus()

    '        End If
    '    End If
    'End Sub

    Private Sub txt_SGST_Percentage_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs)
        If e.KeyValue = 40 Then
            If dgv_BobinDetails.Rows.Count <= 0 Then dgv_BobinDetails.Rows.Add()
            dgv_BobinDetails.Focus()
            dgv_BobinDetails.CurrentCell = dgv_BobinDetails.Rows(0).Cells(0)

        End If
        If e.KeyValue = 38 Then SendKeys.Send("+{TAB}")
    End Sub

    Private Sub txt_SGST_Percentage_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
        If Asc(e.KeyChar) = 13 Then

            If dgv_BobinDetails.Rows.Count <= 0 Then dgv_BobinDetails.Rows.Add()

            dgv_BobinDetails.Focus()

            dgv_BobinDetails.CurrentCell = dgv_BobinDetails.Rows(0).Cells(0)

        End If
    End Sub
    Private Sub txt_CGST_Percentage_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs)

        If e.KeyValue = 38 Then SendKeys.Send("+{TAB}")
        If e.KeyValue = 40 Then SendKeys.Send("{TAB}")
    End Sub

    Private Sub txt_CGST_Percentage_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
        If Asc(e.KeyChar) = 13 Then SendKeys.Send("{TAB}")
    End Sub

    Private Sub btn_Bobin_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Bobin.Click
        pnl_Back.Enabled = False
        pnl_Bobin.Visible = True
    End Sub

    Private Sub btn_Close_Bobin_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Close_Bobin.Click
        pnl_Back.Enabled = True
        pnl_Bobin.Visible = False
    End Sub

    Private Sub cbo_ItemGroup_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_ItemGroup.KeyUp
        If e.Control = False And e.KeyValue = 17 Then
            Dim f As New ItemGroup_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_ItemGroup.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If
    End Sub
    Private Sub cbo_Weaver_Weft_Consumption_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Weaver_Weft_Consumption.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Weaver_Weft_Consumption, txt_Weave, Nothing, "", "", "", "")
        If (e.KeyValue = 40 And cbo_Weaver_Weft_Consumption.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
            If MessageBox.Show("Do you want to save", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = DialogResult.Yes Then
                save_record()
            Else

                txt_Name.Focus()
            End If
        End If
    End Sub

    Private Sub cbo_ClothSet_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_ClothSet.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "ClothSet_Head", "ClothSet_Name", "", "(ClothSet_IdNo = 0)")
    End Sub

    Private Sub cbo_ClothSet_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_ClothSet.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_ClothSet, Nothing, Nothing, "ClothSet_Head", "ClothSet_Name", "", "(ClothSet_IdNo = 0)")

        If (e.KeyValue = 38 And cbo_ClothSet.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then

            If cbo_loomtype.Visible Then
                cbo_loomtype.Focus()
            Else
                txt_Allowed_Excess_Doff_Meters_Percentage.Focus()
            End If

        End If

        If (e.KeyValue = 40 And cbo_ClothSet.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
            If txt_checking_wages_per_meter.Visible And txt_checking_wages_per_meter.Enabled Then
                txt_checking_wages_per_meter.Focus()

            Else
                If dgv_EndsCountDetails.Rows.Count > 0 Then
                    If dgv_EndsCountDetails.Rows.Count <= 0 Then dgv_EndsCountDetails.Rows.Add()
                    dgv_EndsCountDetails.Focus()
                    dgv_EndsCountDetails.CurrentCell = dgv_EndsCountDetails.Rows(0).Cells(1)
                Else
                    cbo_EndsCountMainName.Focus()
                End If

            End If
        End If

        'If (e.KeyValue = 40 And cbo_ClothSet.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
        '    If MessageBox.Show("Do you want to save", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = DialogResult.Yes Then
        '        save_record()
        '    Else

        '        txt_Name.Focus()
        '    End If
        'End If
    End Sub

    Private Sub cbo_ClothSet_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_ClothSet.KeyPress

        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_ClothSet, Nothing, "ClothSet_Head", "ClothSet_Name", "", "(ClothSet_IdNo = 0)")
        If Asc(e.KeyChar) = 13 Then
            If txt_checking_wages_per_meter.Visible And txt_checking_wages_per_meter.Enabled Then
                txt_checking_wages_per_meter.Focus()

            Else
                If dgv_EndsCountDetails.Rows.Count > 0 Then
                    If dgv_EndsCountDetails.Rows.Count <= 0 Then dgv_EndsCountDetails.Rows.Add()
                    dgv_EndsCountDetails.Focus()
                    dgv_EndsCountDetails.CurrentCell = dgv_EndsCountDetails.Rows(0).Cells(1)
                Else
                    cbo_EndsCountMainName.Focus()
                End If

            End If
        End If
        'If Asc(e.KeyChar) = 13 Then
        '    If MessageBox.Show("Do you want to save", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = DialogResult.Yes Then
        '        save_record()
        '    Else

        '        txt_Name.Focus()
        '    End If
        'End If
    End Sub


    Private Sub cbo_Weaver_Weft_Consumption_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Weaver_Weft_Consumption.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Weaver_Weft_Consumption, Nothing, "", "", "", "")
        If Asc(e.KeyChar) = 13 Then
            If MessageBox.Show("Do you want to save", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = DialogResult.Yes Then
                save_record()
            Else

                txt_Name.Focus()
            End If
        End If
    End Sub

    Private Sub txt_Name_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_Name.KeyDown
        If e.KeyValue = 38 Then

            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1413" Then
                txt_MeterPcs.Focus()
            End If

            If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1413" Then
                If txt_AllowShortage_Perc_Processing.Visible And txt_AllowShortage_Perc_Processing.Enabled Then
                    txt_AllowShortage_Perc_Processing.Focus()

                ElseIf txt_bale_weight_to.Enabled And txt_bale_weight_to.Visible Then
                    txt_bale_weight_to.Focus()

                ElseIf cbo_Weaver_Weft_Consumption.Visible And cbo_Weaver_Weft_Consumption.Enabled Then
                    cbo_Weaver_Weft_Consumption.Focus()

                ElseIf txt_Weave.Visible And txt_Weave.Enabled Then
                    txt_Weave.Focus()
                ElseIf Cbo_Article.Visible And Cbo_Article.Enabled Then
                    Cbo_Article.Focus()

                ElseIf txt_MeterPcs.Visible And txt_MeterPcs.Enabled Then
                    txt_MeterPcs.Focus()
                ElseIf cbo_StockIn.Visible And cbo_StockIn.Enabled Then
                    cbo_StockIn.Focus()


                Else
                    txt_Type5_Rate.Focus()


                End If
            End If


        ElseIf e.KeyValue = 40 Then
            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1413" Then
                txt_EPI_PPI.Focus()
            Else
                cbo_Description.Focus()
            End If
        End If

    End Sub

    Private Sub txt_Weave_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_Weave.KeyDown
        If e.KeyValue = 38 Then
            If Cbo_Article.Visible = True Then
                Cbo_Article.Focus()
                'ElseIf txt_MeterPcs.Visible = True Then
                '    txt_MeterPcs.Focus()
            ElseIf cbo_Slevedge.Visible = True Then
                cbo_Slevedge.Focus()
                If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1413" Then '---- KAVIN GANGA (NOIDA)
                    If txt_EPI_PPI.Visible = True Then
                        txt_EPI_PPI.Focus()
                    End If
                End If
            Else
                txt_Type5_Rate.Focus()
            End If

        ElseIf e.KeyValue = 40 Then
            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1413" Then
                If cbo_fabric_category.Visible = True Then
                    cbo_fabric_category.Focus()
                End If
            Else
                If txt_RollTube_Wgt.Visible Then
                    txt_RollTube_Wgt.Focus()
                ElseIf cbo_Weaver_Weft_Consumption.Visible = True Then
                    cbo_Weaver_Weft_Consumption.Focus()
                ElseIf txt_AllowShortage_Perc_Processing.Visible = True Then
                    txt_AllowShortage_Perc_Processing.Focus()
                ElseIf cbo_WeaverWages_for.Visible = True Then
                    cbo_WeaverWages_for.Focus()
                ElseIf txt_Employee_Wages_Per_Meter.Visible = True Then
                    txt_Employee_Wages_Per_Meter.Focus()
                Else
                    If MessageBox.Show("Do you want to save", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = DialogResult.Yes Then
                        save_record()
                    Else

                        txt_Name.Focus()
                    End If
                End If
            End If
        End If



    End Sub

    Private Sub txt_Weave_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Weave.KeyPress
        If Asc(e.KeyChar) = 13 Then

            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1413" Then '---- KAVIN GANGA (NOIDA)
                If cbo_fabric_category.Visible = True Then
                    cbo_fabric_category.Focus()
                End If

            Else
                If txt_RollTube_Wgt.Visible Then
                    txt_RollTube_Wgt.Focus()
                ElseIf cbo_Weaver_Weft_Consumption.Visible = True Then
                    cbo_Weaver_Weft_Consumption.Focus()

                ElseIf txt_AllowShortage_Perc_Processing.Visible = True Then
                    txt_AllowShortage_Perc_Processing.Focus()

                ElseIf txt_Bale_Weight_from.Visible = True Then
                    txt_Bale_Weight_from.Focus()
                ElseIf cbo_WeaverWages_for.Visible = True Then
                    cbo_WeaverWages_for.Focus()
                ElseIf txt_Employee_Wages_Per_Meter.Visible = True Then
                    txt_Employee_Wages_Per_Meter.Focus()

                Else
                    If MessageBox.Show("Do you want to save", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = DialogResult.Yes Then
                        save_record()
                    Else

                        txt_Name.Focus()
                    End If
                End If

            End If
        End If
    End Sub


    Private Sub cbo_ClothSet_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_ClothSet.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Dim f As New ClothSet_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_ClothSet.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()
        End If
    End Sub


    Private Sub txt_Name_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Name.KeyPress
        If Asc(e.KeyChar) = 39 Then   '-- Single Quotes blocked
            e.Handled = True
        End If
        If Asc(e.KeyChar) = 13 Then
            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1413" Then
                txt_EPI_PPI.Focus()
            Else
                cbo_Description.Focus()
            End If
        End If
    End Sub

    Private Sub cbo_loomtype_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_loomtype.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "LoomType_Head", "LoomType_name", "", "(LoomType_IdNo=0)")
    End Sub

    Private Sub cbo_loomtype_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_loomtype.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_loomtype, Nothing, Nothing, "LoomType_Head", "LoomType_name", "", "(LoomType_IdNo=0)")
        If (e.KeyValue = 38 And sender.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then

            If txt_Allowed_Excess_Doff_Meters_Percentage.Visible Then
                txt_Allowed_Excess_Doff_Meters_Percentage.Focus()
            Else
                txt_CrimpPerc.Focus()
            End If

        End If

        If (e.KeyValue = 40 And sender.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
            If cbo_ClothSet.Visible = True Then
                cbo_ClothSet.Focus()

            ElseIf dgv_EndsCountDetails.Rows.Count > 0 Then
                If dgv_EndsCountDetails.Rows.Count <= 0 Then dgv_EndsCountDetails.Rows.Add()
                dgv_EndsCountDetails.Focus()
                dgv_EndsCountDetails.CurrentCell = dgv_EndsCountDetails.Rows(0).Cells(1)

            Else
                cbo_EndsCountMainName.Focus()

            End If
        End If

    End Sub

    Private Sub cbo_loomtype_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_loomtype.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_loomtype, Nothing, "LoomType_Head", "LoomType_name", "", "(LoomType_IdNo=0)")
        If Asc(e.KeyChar) = 13 Then
            If cbo_ClothSet.Visible Then
                cbo_ClothSet.Focus()
            ElseIf dgv_EndsCountDetails.Rows.Count > 0 Then
                If dgv_EndsCountDetails.Rows.Count <= 0 Then dgv_EndsCountDetails.Rows.Add()
                dgv_EndsCountDetails.Focus()
                dgv_EndsCountDetails.CurrentCell = dgv_EndsCountDetails.Rows(0).Cells(1)
            Else
                cbo_EndsCountMainName.Focus()
            End If
        End If
    End Sub





    Private Sub cbo_loomtype_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_loomtype.KeyUp
        If e.Control = False And e.KeyValue = 17 Then
            Dim f As New LoomType_Creation

            f.MdiParent = MDIParent1
            f.Show()
        End If
    End Sub

    Private Sub Txt_Excess_Doff_Meters_Percentage_Allowed_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_Allowed_Excess_Doff_Meters_Percentage.KeyDown
        If (e.KeyValue = 38) Or (e.Control = True And e.KeyValue = 38) Then
            txt_CrimpPerc.Focus()

        End If

        If (e.KeyValue = 40) Or (e.Control = True And e.KeyValue = 40) Then

            If cbo_loomtype.Visible Then
                cbo_loomtype.Focus()
            ElseIf cbo_ClothSet.Visible Then
                cbo_ClothSet.Focus()
            ElseIf txt_wrap_waste_percentage.Visible Then
                txt_wrap_waste_percentage.Focus()
            ElseIf txt_checking_wages_per_meter.Visible Then
                txt_checking_wages_per_meter.Focus()
            ElseIf txt_weft_waste_percentage.Visible Then
                txt_weft_waste_percentage.Focus()
            ElseIf Cbo_Pavu_Consumption_In_Meter_Weight.Visible Then
                Cbo_Pavu_Consumption_In_Meter_Weight.Focus()

            ElseIf dgv_EndsCountDetails.Rows.Count > 0 Then
                If dgv_EndsCountDetails.Rows.Count <= 0 Then dgv_EndsCountDetails.Rows.Add()
                dgv_EndsCountDetails.Focus()
                dgv_EndsCountDetails.CurrentCell = dgv_EndsCountDetails.Rows(0).Cells(1)
            Else
                cbo_EndsCountMainName.Focus()
            End If

        End If

    End Sub

    Private Sub Txt_Excess_Doff_Meters_Percentage_Allowed_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Allowed_Excess_Doff_Meters_Percentage.KeyPress
        If Asc(e.KeyChar) = 13 Then
            If cbo_loomtype.Visible Then
                cbo_loomtype.Focus()
            ElseIf cbo_ClothSet.Visible Then
                cbo_ClothSet.Focus()
            ElseIf txt_wrap_waste_percentage.Visible Then
                txt_wrap_waste_percentage.Focus()
            ElseIf txt_checking_wages_per_meter.Visible Then
                txt_checking_wages_per_meter.Focus()
            ElseIf Cbo_Pavu_Consumption_In_Meter_Weight.Visible Then
                Cbo_Pavu_Consumption_In_Meter_Weight.Focus()
            ElseIf dgv_EndsCountDetails.Rows.Count > 0 Then
                If dgv_EndsCountDetails.Rows.Count <= 0 Then dgv_EndsCountDetails.Rows.Add()
                dgv_EndsCountDetails.Focus()
                dgv_EndsCountDetails.CurrentCell = dgv_EndsCountDetails.Rows(0).Cells(1)
            Else
                cbo_EndsCountMainName.Focus()
            End If
        End If
    End Sub



    Private Sub txt_Weight_Meter_Fabric_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Weight_Meter_Fabric.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub


    Private Sub txt_MeterPcs_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_MeterPcs.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1413" Then '---- KAVIN GANGA (NOIDA)
            If Asc(e.KeyChar) = 13 Then
                If MessageBox.Show("Do you want to save", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = DialogResult.Yes Then
                    save_record()
                Else

                    txt_Name.Focus()
                End If
            End If
        End If
    End Sub


    Private Sub txt_Weight_Meter_Pavu_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txt_Weight_Meter_Pavu.TextChanged
        'Try
        '    If FrmLdSTS = True Then Exit Sub
        '    If Me.ActiveControl.Name <> txt_Weight_Meter_Fabric.Name Then
        '        txt_Weight_Meter_Fabric.Text = Format(Val(txt_Weight_Meter_Pavu.Text) + Val(txt_Weight_Meter_Yarn.Text), "#########0.0000")
        '    End If
        'Catch ex As Exception
        '    '----
        'End Try
    End Sub

    Private Sub txt_weight_min_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_weight_min.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub txt_weight_max_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_weight_max.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub txt_CrimpPerc_KeyDown(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles txt_CrimpPerc.KeyDown
        On Error Resume Next
        If e.KeyValue = 38 Then
            e.Handled = True
            e.SuppressKeyPress = True
            If txt_weight_max.Visible = True And txt_weight_max.Enabled = True Then
                txt_weight_max.Focus()
            Else
                txt_Weight_Meter_Pavu.Focus()
            End If
        End If
        If e.KeyValue = 40 Then
            e.Handled = True
            e.SuppressKeyPress = True
            If txt_Allowed_Excess_Doff_Meters_Percentage.Visible = True And txt_Allowed_Excess_Doff_Meters_Percentage.Enabled = True Then
                txt_Allowed_Excess_Doff_Meters_Percentage.Focus()

            ElseIf cbo_loomtype.Visible = True And cbo_loomtype.Enabled = True Then
                cbo_loomtype.Focus()

            ElseIf cbo_ClothSet.Visible = True And cbo_ClothSet.Enabled = True Then
                cbo_ClothSet.Focus()
            ElseIf cbo_fabric_name.Visible = True And cbo_fabric_name.Enabled = True Then
                cbo_fabric_name.Focus()
            Else
                'If dgv_EndsCountDetails.Rows.Count <= 0 Then dgv_EndsCountDetails.Rows.Add()
                'dgv_EndsCountDetails.Focus()
                'dgv_EndsCountDetails.CurrentCell = dgv_EndsCountDetails.Rows(0).Cells(1)
                cbo_quality_description.Focus()

            End If

        End If
    End Sub

    Private Sub txt_CrimpPerc_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_CrimpPerc.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
        If Asc(e.KeyChar) = 13 Then
            If txt_Allowed_Excess_Doff_Meters_Percentage.Visible = True And txt_Allowed_Excess_Doff_Meters_Percentage.Enabled = True Then
                txt_Allowed_Excess_Doff_Meters_Percentage.Focus()

            ElseIf cbo_loomtype.Visible = True And cbo_loomtype.Enabled = True Then
                cbo_loomtype.Focus()

            ElseIf cbo_ClothSet.Visible = True And cbo_ClothSet.Enabled = True Then
                cbo_ClothSet.Focus()

            ElseIf cbo_fabric_name.Visible = True And cbo_fabric_name.Enabled = True Then
                cbo_fabric_name.Focus()
            Else
                'If dgv_EndsCountDetails.Rows.Count <= 0 Then dgv_EndsCountDetails.Rows.Add()
                'dgv_EndsCountDetails.Focus()
                'dgv_EndsCountDetails.CurrentCell = dgv_EndsCountDetails.Rows(0).Cells(1)
                cbo_quality_description.Focus()


            End If
        End If
    End Sub

    Private Sub btn_RateDetails_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_RateDetails.Click
        pnl_Back.Enabled = False
        pnl_SalesRate_Details.Visible = True
        If dgv_SalesRate_Details.Enabled And dgv_SalesRate_Details.Visible Then
            dgv_SalesRate_Details.Focus()
            dgv_SalesRate_Details.CurrentCell = dgv_SalesRate_Details.Rows(0).Cells(1)
        End If
    End Sub

    Private Sub btn_Close_SalesRate_Details_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Close_SalesRate_Details.Click
        Close_SalesRate_Details()
    End Sub

    Private Sub Close_SalesRate_Details()
        pnl_SalesRate_Details.Visible = False
        pnl_Back.Enabled = True
        If txt_Name.Enabled And txt_Name.Visible Then txt_Name.Focus()
    End Sub


    Private Sub dgv_SalesRate_Details_CellEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_SalesRate_Details.CellEnter
        Dim CmpGrp_Fromdate As Date


        If FrmLdSTS = True Then Exit Sub
        With dgv_SalesRate_Details

            If Val(.CurrentRow.Cells(0).Value) = 0 Then
                .CurrentRow.Cells(0).Value = .CurrentRow.Index + 1
            End If
            dgv_ActiveCtrl_Name = dgv_SalesRate_Details.Name

            CmpGrp_Fromdate = New DateTime(Val(Microsoft.VisualBasic.Left(Common_Procedures.FnRange, 4)), 4, 1)
            .Rows(0).Cells(1).Value = Format(DateAdd(DateInterval.Year, -1, CmpGrp_Fromdate), "dd-MM-yyyy")

        End With
    End Sub

    Private Sub dgv_SalesRate_Details_CellLeave(sender As Object, e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_SalesRate_Details.CellLeave

        If FrmLdSTS = True Then Exit Sub

        With dgv_SalesRate_Details

            If e.ColumnIndex = 1 Or e.ColumnIndex = 2 Then

                If Trim(.Rows(e.RowIndex).Cells(1).Value) <> "" Then
                    If IsDate(.Rows(e.RowIndex).Cells(1).Value) = False Then
                        .Rows(e.RowIndex).Cells(1).Value = ""
                    End If
                End If

                If Trim(.Rows(e.RowIndex).Cells(2).Value) <> "" Then
                    If IsDate(.Rows(e.RowIndex).Cells(2).Value) = False Then
                        .Rows(e.RowIndex).Cells(2).Value = ""
                    End If
                End If

            End If
        End With
    End Sub

    Private Sub dgv_SalesRate_Details_CellValueChanged(sender As Object, e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_SalesRate_Details.CellValueChanged
        Dim vDat1 As Date
        If FrmLdSTS = True Then Exit Sub

        If IsNothing(dgv_SalesRate_Details.CurrentCell) Then Exit Sub
        With dgv_SalesRate_Details

            If e.ColumnIndex = 1 And e.RowIndex > 0 Then

                If Trim(.Rows(e.RowIndex).Cells(1).Value) <> "" Then
                    If IsDate(.Rows(e.RowIndex).Cells(1).Value) = True Then
                        vDat1 = CDate(.Rows(e.RowIndex).Cells(1).Value)
                        .Rows(e.RowIndex - 1).Cells(2).Value = Format(DateAdd(DateInterval.Day, -1, vDat1), "dd-MM-yyyy")
                    End If
                End If

            End If

        End With
    End Sub


    Private Sub dgv_SalesRate_Details_EditingControlShowing(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewEditingControlShowingEventArgs) Handles dgv_SalesRate_Details.EditingControlShowing
        dgtxt_SalesRate_Details = CType(dgv_SalesRate_Details.EditingControl, DataGridViewTextBoxEditingControl)
    End Sub

    Private Sub dgtxt_SalesRate_Details_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgtxt_SalesRate_Details.Enter
        Try
            dgv_ActiveCtrl_Name = dgv_SalesRate_Details.Name

            dgv_SalesRate_Details.EditingControl.BackColor = Color.Lime
            dgv_SalesRate_Details.EditingControl.ForeColor = Color.Blue
            dgv_SalesRate_Details.SelectAll()
        Catch ex As Exception
            '--
        End Try
    End Sub

    Private Sub dgtxt_SalesRate_Details_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgtxt_SalesRate_Details.KeyDown
        Try

            With dgv_SalesRate_Details

                vcbo_KeyDwnVal = e.KeyValue

                If .Visible Then
                    If e.KeyValue <> 27 Then

                        If .CurrentCell.RowIndex = 0 And .CurrentCell.ColumnIndex = 1 Then

                            e.Handled = True
                            e.SuppressKeyPress = True

                        End If

                    End If


                End If

            End With

        Catch ex As Exception
            '---

        End Try

    End Sub

    Private Sub dgtxt_SalesRate_Details_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dgtxt_SalesRate_Details.KeyPress
        Try
            With dgv_SalesRate_Details
                If .Visible Then

                    If .CurrentCell.RowIndex = 0 And .CurrentCell.ColumnIndex = 1 Then
                        e.Handled = True

                    Else

                        If .CurrentCell.ColumnIndex = 1 Then
                            If Common_Procedures.Accept_NegativeNumbers(Asc(e.KeyChar)) = 0 Then
                                e.Handled = True
                            End If

                        Else
                            If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
                                e.Handled = True
                            End If
                        End If


                    End If

                End If
            End With
        Catch ex As Exception
            '---
        End Try
    End Sub

    Private Sub dgtxt_SalesRate_Details_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgtxt_SalesRate_Details.KeyUp
        Try
            If e.Control = True And UCase(Chr(e.KeyCode)) = "D" Then
                dgv_SalesRate_Details_KeyUp(sender, e)
            End If
        Catch ex As Exception
            '---
        End Try
    End Sub

    Private Sub dgv_SalesRate_Details_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_SalesRate_Details.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
    End Sub


    Private Sub dgv_SalesRate_Details_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_SalesRate_Details.KeyUp
        Dim n As Integer

        Try
            If e.Control = True And UCase(Chr(e.KeyCode)) = "D" Then
                With dgv_SalesRate_Details

                    n = .CurrentRow.Index

                    If n = .Rows.Count - 1 Then
                        For i = 0 To .ColumnCount - 1
                            .Rows(n).Cells(i).Value = ""
                        Next

                    Else
                        .Rows.RemoveAt(n)

                    End If

                End With

            End If

        Catch ex As Exception
            '---
        End Try

    End Sub

    Private Sub dgv_SalesRate_Details_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgv_SalesRate_Details.LostFocus
        On Error Resume Next
        If Not IsNothing(dgv_SalesRate_Details.CurrentCell) Then dgv_SalesRate_Details.CurrentCell.Selected = False
    End Sub

    Private Sub dgv_SalesRate_Details_RowsAdded(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewRowsAddedEventArgs) Handles dgv_SalesRate_Details.RowsAdded
        If FrmLdSTS = True Then Exit Sub
        Dim n As Integer = 0
        Try

            If IsNothing(dgv_SalesRate_Details.CurrentCell) Then Exit Sub
            With dgv_SalesRate_Details
                n = .RowCount
                .Rows(n - 1).Cells(0).Value = Val(n)
            End With
        Catch ex As Exception
            '---
        End Try

    End Sub

    Private Sub dgtxt_SalesRate_Details_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgtxt_SalesRate_Details.TextChanged
        Try
            With dgv_SalesRate_Details

                If .Visible Then
                    If .Rows.Count > 0 Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(dgtxt_SalesRate_Details.Text)
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

    Private Sub txt_bale_weight_to_KeyDown(sender As Object, e As KeyEventArgs) Handles txt_bale_weight_to.KeyDown

        If e.KeyValue = 38 Then
            txt_Bale_Weight_from.Focus()
        End If
        If e.KeyValue = 40 Then
            If MessageBox.Show("Do you want to save", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = DialogResult.Yes Then
                save_record()
            Else

                txt_Name.Focus()
            End If
        End If

    End Sub

    Private Sub txt_bale_weight_to_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txt_bale_weight_to.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
        If Asc(e.KeyChar) = 13 Then
            If MessageBox.Show("Do you want to save", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = DialogResult.Yes Then
                save_record()
            Else
                txt_Name.Focus()
            End If
        End If
    End Sub



    Private Sub txt_Bale_Weight_from_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txt_Bale_Weight_from.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub


    Private Sub txt_wrap_waste_percentage_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txt_wrap_waste_percentage.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
        If Asc(e.KeyChar) = 13 Then

            txt_weft_waste_percentage.Focus()

        End If
    End Sub

    Private Sub txt_wrap_waste_percentage_KeyDown(sender As Object, e As KeyEventArgs) Handles txt_wrap_waste_percentage.KeyDown
        If e.KeyCode = 38 Then
            If cbo_ClothSet.Visible Then
                cbo_ClothSet.Focus()

            ElseIf cbo_loomtype.Visible Then
                cbo_loomtype.Focus()
            Else
                txt_Allowed_Excess_Doff_Meters_Percentage.Focus()
            End If
        End If

        If e.KeyCode = 40 Then
            txt_weft_waste_percentage.Focus()
        End If

    End Sub

    Private Sub txt_weft_waste_percentage_KeyDown(sender As Object, e As KeyEventArgs) Handles txt_weft_waste_percentage.KeyDown
        If e.KeyCode = 38 Then
            txt_wrap_waste_percentage.Focus()
        End If

        If e.KeyCode = 40 Then
            If dgv_EndsCountDetails.Rows.Count > 0 Then
                If dgv_EndsCountDetails.Rows.Count <= 0 Then dgv_EndsCountDetails.Rows.Add()
                dgv_EndsCountDetails.Focus()
                dgv_EndsCountDetails.CurrentCell = dgv_EndsCountDetails.Rows(0).Cells(1)
            Else
                cbo_EndsCountMainName.Focus()
            End If
        End If
    End Sub

    Private Sub txt_weft_waste_percentage_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txt_weft_waste_percentage.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
        If Asc(e.KeyChar) = 13 Then

            If dgv_EndsCountDetails.Rows.Count > 0 Then
                If dgv_EndsCountDetails.Rows.Count <= 0 Then dgv_EndsCountDetails.Rows.Add()
                dgv_EndsCountDetails.Focus()
                dgv_EndsCountDetails.CurrentCell = dgv_EndsCountDetails.Rows(0).Cells(1)
            Else
                cbo_EndsCountMainName.Focus()
            End If

        End If
    End Sub

    Private Sub txt_Weight_Meter_Pavu_GotFocus(sender As Object, e As EventArgs) Handles txt_Weight_Meter_Pavu.GotFocus
        If Val(txt_Weight_Meter_Pavu.Text) = 0 Then
            Btn_wrap_calculation_Click(sender, e)
        End If


    End Sub

    Private Sub Btn_wrap_calculation_Click(sender As Object, e As EventArgs) Handles Btn_wrap_calculation.Click
        Dim Wgtmtr As Single = 0
        Dim wrpmtr As Single = 0
        Dim NumWftCnt As Single = 0
        Dim NumEndCnt As Single = 0
        Dim warp_count As Single = 0
        Dim WSTPERCWGT As String = ""

        '--- Consumed Yarn Formula  = "(METERS * REEDSPACE * PICK * 1.0937) / (84 * 22 * WEFT)"

        NumWftCnt = Val(Common_Procedures.get_FieldValue(con, "count_head", "Resultant_Count", "(count_name = '" & Trim(cbo_WarpCount.Text) & "')"))
        If Val(NumWftCnt) = 0 Then NumWftCnt = Val(cbo_WarpCount.Text)

        Wgtmtr = 0
        txt_Weight_Meter_Pavu.Text = ""


        NumEndCnt = Val(Common_Procedures.get_FieldValue(con, "Endscount_head", "Ends_Name", "(Endscount_name = '" & Trim(cbo_EndsCountMainName.Text) & "')"))
        wrpmtr = 0
        txt_Weight_Meter_Pavu.Text = ""

        If Common_Procedures.settings.CustomerCode = "1186" Then
            If Val(NumEndCnt) <> 0 Then
                wrpmtr = (Val(NumEndCnt) / (1693.305 * Val(NumWftCnt)))
                WSTPERCWGT = Val(wrpmtr) * Val(txt_wrap_waste_percentage.Text) / 100
                wrpmtr = Val(wrpmtr) + Val(WSTPERCWGT)

                txt_Weight_Meter_Pavu.Text = Format(Val(wrpmtr), "#########0.0000")

            End If

        ElseIf Common_Procedures.settings.CustomerCode = "1438" Then

            If Val(NumWftCnt) <> 0 Then
                Wgtmtr = ((Val(txt_Reed.Text) * Val(txt_Pick.Text)) / (NumWftCnt) / 1693)

                txt_Weight_Meter_Pavu.Text = Format(Val(Wgtmtr), "#########0.0000")

            End If

        ElseIf Common_Procedures.settings.CustomerCode = "1569" Then

            '--- WARP GRAM FORMULA = (  ( WIDTH * REED ) * 0.000591  )  /  WARPCOUNT
            If Val(NumWftCnt) <> 0 Then

                Wgtmtr = ((Val(txt_Width.Text) * Val(txt_Reed.Text)) * 0.000591) / (NumWftCnt)

                txt_Weight_Meter_Pavu.Text = Format(Val(Wgtmtr), "#########0.0000")

            End If

        Else

            If Val(NumWftCnt) <> 0 Then
                Wgtmtr = (Val(txt_Reed.Text) * Val(txt_Pick.Text) * 1.0937) / (84 * 22 * NumWftCnt)
                txt_Weight_Meter_Pavu.Text = Format(Val(Wgtmtr), "#########0.0000")

            End If


        End If

        'If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1186" Then '---- United weaves
        txt_Weight_Meter_Fabric.Text = Format(Val(txt_Weight_Meter_Pavu.Text) + Val(txt_Weight_Meter_Yarn.Text), "#########0.0000")
        'End If

    End Sub

    Private Sub cbo_fabric_name_GotFocus(sender As Object, e As EventArgs) Handles cbo_fabric_name.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Fabric_Name_Head", "Fabric_Name", "", "(Fabric_Name_IdNo = 0)")
    End Sub

    Private Sub cbo_fabric_name_KeyDown(sender As Object, e As KeyEventArgs) Handles cbo_fabric_name.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_fabric_name, Nothing, Nothing, "Fabric_Name_Head", "Fabric_Name", "", "(Fabric_Name_IdNo = 0)")
        If (e.KeyValue = 38 And cbo_quality_description.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
            txt_CrimpPerc.Focus()

        End If

        If (e.KeyValue = 40 And cbo_quality_description.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
            If dgv_EndsCountDetails.Rows.Count > 0 Then
                If dgv_EndsCountDetails.Rows.Count <= 0 Then dgv_EndsCountDetails.Rows.Add()
                dgv_EndsCountDetails.Focus()
                dgv_EndsCountDetails.CurrentCell = dgv_EndsCountDetails.Rows(0).Cells(1)
            End If
        End If
    End Sub

    Private Sub cbo_fabric_name_KeyPress(sender As Object, e As KeyPressEventArgs) Handles cbo_fabric_name.KeyPress
        If Asc(e.KeyChar) = 39 Then   '-- Single Quotes blocked
            e.Handled = True
            Exit Sub
        End If
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_fabric_name, Nothing, "Fabric_Name_Head", "Fabric_Name", "", "(Fabric_Name_IdNo=0 )")

        If Asc(e.KeyChar) = 13 Then
            If dgv_EndsCountDetails.Rows.Count > 0 Then
                If dgv_EndsCountDetails.Rows.Count <= 0 Then dgv_EndsCountDetails.Rows.Add()
                dgv_EndsCountDetails.Focus()
                dgv_EndsCountDetails.CurrentCell = dgv_EndsCountDetails.Rows(0).Cells(1)
            End If
        End If
    End Sub

    Private Sub cbo_fabric_name_KeyUp(sender As Object, e As KeyEventArgs) Handles cbo_fabric_name.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then

            Dim f As New Fabric_Sales_Name_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_fabric_name.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If
    End Sub

    Private Sub cbo_fabric_category_GotFocus(sender As Object, e As EventArgs) Handles cbo_fabric_category.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Fabric_Category_Head", "Fabric_Category_Name", "", "(Fabric_Category_IdNo = 0)")
    End Sub

    Private Sub cbo_fabric_category_KeyDown(sender As Object, e As KeyEventArgs) Handles cbo_fabric_category.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_fabric_category, txt_Weave, cbo_ItemGroup, "Fabric_Category_Head", "Fabric_Category_Name", "", "(Fabric_Category_IdNo = 0)")
    End Sub

    Private Sub cbo_fabric_category_KeyPress(sender As Object, e As KeyPressEventArgs) Handles cbo_fabric_category.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_fabric_category, cbo_ItemGroup, "Fabric_Category_Head", "Fabric_Category_Name", "", "(Fabric_Category_IdNo=0 )")
    End Sub

    Private Sub cbo_fabric_category_KeyUp(sender As Object, e As KeyEventArgs) Handles cbo_fabric_category.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then

            Dim f As New Fabric_Category_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_fabric_category.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If
    End Sub

    Private Sub txt_EPI_PPI_KeyDown(sender As Object, e As KeyEventArgs) Handles txt_EPI_PPI.KeyDown
        If e.KeyCode = 38 Then

            'If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1413" Then '---- KAVIN GANGA (NOIDA)
            If txt_Name.Visible = True Then
                txt_Name.Focus()
            End If
        End If
        'End If
        If e.KeyCode = 40 Then
            If txt_Weave.Visible = True Then
                txt_Weave.Focus()
            End If
        End If


    End Sub

    Private Sub txt_EPI_PPI_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txt_EPI_PPI.KeyPress
        If Asc(e.KeyChar) = 13 Then
            txt_Weave.Focus()
        End If
    End Sub

    Private Sub txt_MeterPcs_KeyDown(sender As Object, e As KeyEventArgs) Handles txt_MeterPcs.KeyDown
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1413" Then '---- KAVIN GANGA (NOIDA)
            If e.KeyCode = 40 Then
                If MessageBox.Show("Do you want to save", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = DialogResult.Yes Then
                    save_record()
                Else

                    txt_Name.Focus()
                End If
            End If
        End If
    End Sub

    Private Sub txt_RollTube_Wgt_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txt_RollTube_Wgt.KeyPress
        If Asc(e.KeyChar) = 13 Then
            If cbo_WeaverWages_for.Visible And cbo_WeaverWages_for.Enabled Then
                cbo_WeaverWages_for.Focus()

            Else

                If MessageBox.Show("Do you want to save", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = DialogResult.Yes Then
                    save_record()
                Else
                    txt_Name.Focus()
                End If

            End If

        End If
    End Sub

    Private Sub txt_RollTube_Wgt_KeyDown(sender As Object, e As KeyEventArgs) Handles txt_RollTube_Wgt.KeyDown
        If e.KeyCode = 38 Then
            txt_Weave.Focus()
        End If

        If e.KeyCode = 40 Then
            If cbo_WeaverWages_for.Visible And cbo_WeaverWages_for.Enabled Then
                cbo_WeaverWages_for.Focus()

            Else
                If MessageBox.Show("Do you want to save", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = DialogResult.Yes Then
                    save_record()
                Else

                    txt_Name.Focus()
                End If
            End If

        End If

    End Sub

    Private Sub txt_sortno_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txt_sortno.KeyPress
        If Asc(e.KeyChar) = 39 Then   '-- Single Quotes blocked
            e.Handled = True
            Exit Sub
        End If
    End Sub

    Private Sub txt_fabric_gsm_KeyDown(sender As Object, e As KeyEventArgs) Handles txt_fabric_gsm.KeyDown
        If e.KeyCode = 38 Then
            txt_Coolie_Type5.Focus()
        End If

        If e.KeyCode = 40 Then
            txt_weight_min.Focus()
        End If
    End Sub

    Private Sub txt_fabric_gsm_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txt_fabric_gsm.KeyPress
        If Asc(e.KeyChar) = 13 Then
            txt_weight_min.Focus()
        End If
    End Sub

    Private Sub txt_Coolie_Type5_KeyDown(sender As Object, e As KeyEventArgs) Handles txt_Coolie_Type5.KeyDown
        If e.KeyCode = 38 Then
            If txt_Type4_Rate.Enabled And txt_Type4_Rate.Visible Then
                txt_Type4_Rate.Focus()
            Else
                txt_Coolie_Type4.Focus()
                'txt_Weight_Meter_Pavu.Focus()
            End If
        End If


        If e.KeyValue = 40 Then
            If txt_fabric_gsm.Enabled And txt_fabric_gsm.Visible Then
                txt_fabric_gsm.Focus()
            ElseIf txt_Type5_Rate.Visible And txt_Type5_Rate.Enabled Then
                txt_Type5_Rate.Focus()
            Else
                txt_weight_min.Focus()
            End If
            '  txt_fabric_gsm
            '  txt_Type5_Rate
        End If
    End Sub


    Private Sub cbo_Slevedge_GotFocus(sender As Object, e As EventArgs) Handles cbo_Slevedge.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Slevedge_Head", "Slevedge_Name", "", "(Slevedge_Idno=0)")
    End Sub

    Private Sub cbo_Slevedge_KeyDown(sender As Object, e As KeyEventArgs) Handles cbo_Slevedge.KeyDown

        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Slevedge, txt_MeterPcs, txt_Weave, "Slevedge_Head", "Slevedge_Name", "", "(Slevedge_Idno=0)")
    End Sub

    Private Sub cbo_Slevedge_KeyPress(sender As Object, e As KeyPressEventArgs) Handles cbo_Slevedge.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Slevedge, txt_Weave, "Slevedge_Head", "Slevedge_Name", "", "(Slevedge_Idno=0)")
    End Sub

    Private Sub cbo_Slevedge_KeyUp(sender As Object, e As KeyEventArgs) Handles cbo_Slevedge.KeyUp

        If e.Control = False And e.KeyValue = 17 Then
            Dim f As New Slevedge_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Slevedge.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If
    End Sub

    Private Sub txt_Weight_Meter_Fabric_TextChanged(sender As Object, e As EventArgs) Handles txt_Weight_Meter_Fabric.TextChanged
        'Try

        '    If FrmLdSTS = True Then Exit Sub

        '    If Me.ActiveControl.Name <> txt_Weight_Meter_Pavu.Name Then
        '        txt_Weight_Meter_Pavu.Text = Format(Val(txt_Weight_Meter_Fabric.Text) - Val(txt_Weight_Meter_Yarn.Text), "#########0.0000")
        '    End If

        'Catch ex As Exception
        '    '----
        'End Try
    End Sub

    Private Sub txt_Coolie_Type1_TextChanged(sender As Object, e As EventArgs) Handles txt_Coolie_Type1.TextChanged

    End Sub

    Private Sub Cbo_Article_KeyUp(sender As Object, e As KeyEventArgs) Handles Cbo_Article.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Dim f As New Article_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = sender.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()
        End If
    End Sub

    Private Sub btn_Show_Additional_Weft_Details_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Weft_Consumption_Details.Click
        pnl_Back.Enabled = False
        pnl_Additional_Weft_Details.Visible = True
        If dgv_Additional_Weft_Details.Rows.Count > 0 Then
            dgv_Additional_Weft_Details.Focus()
            dgv_Additional_Weft_Details.CurrentCell = dgv_Additional_Weft_Details.Rows(0).Cells(0)
        End If
    End Sub

    Private Sub btn_Close_Additional_Weft_Details_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Close_Additional_Weft_Details.Click
        pnl_Back.Enabled = True
        pnl_Additional_Weft_Details.Visible = False
    End Sub

    Private Sub dgv_Additional_Weft_Details_CellEndEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Additional_Weft_Details.CellEndEdit
        dgv_Additional_Weft_Details_CellLeave(sender, e)
    End Sub

    Private Sub dgv_Additional_Weft_Details_CellEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Additional_Weft_Details.CellEnter
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim Dt2 As New DataTable
        Dim Dt3 As New DataTable
        Dim rect As Rectangle

        With dgv_Additional_Weft_Details

            dgv_ActiveCtrl_Name = .Name

            If Trim(.Rows(e.RowIndex).Cells(1).Value) = "" Then
                .Rows(e.RowIndex).Cells(1).Value = "METER"
            End If
            If Trim(.Rows(e.RowIndex).Cells(2).Value) = "" Then
                .Rows(e.RowIndex).Cells(2).Value = "GRAM"
            End If
            If e.ColumnIndex = 0 Then

                If cbo_grid_Additional_Weft_Details.Visible = False Or Val(cbo_grid_Additional_Weft_Details.Tag) <> e.RowIndex Then

                    cbo_grid_Additional_Weft_Details.Tag = -1
                    Da = New SqlClient.SqlDataAdapter("select Count_Name from Count_Head order by Count_Name", con)
                    Dt1 = New DataTable
                    Da.Fill(Dt1)
                    cbo_grid_Additional_Weft_Details.DataSource = Dt1
                    cbo_grid_Additional_Weft_Details.DisplayMember = "Count_Name"

                    rect = .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False)

                    cbo_grid_Additional_Weft_Details.Left = .Left + rect.Left  '  .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Left
                    cbo_grid_Additional_Weft_Details.Top = .Top + rect.Top  ' .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Top

                    cbo_grid_Additional_Weft_Details.Width = rect.Width  ' .CurrentCell.Size.Width
                    cbo_grid_Additional_Weft_Details.Height = rect.Height  ' rect.Height
                    cbo_grid_Additional_Weft_Details.Text = .CurrentCell.Value  '  Trim(.CurrentRow.Cells(e.ColumnIndex).Value)

                    cbo_grid_Additional_Weft_Details.Tag = Val(e.RowIndex)
                    cbo_grid_Additional_Weft_Details.Visible = True

                    cbo_grid_Additional_Weft_Details.BringToFront()
                    cbo_grid_Additional_Weft_Details.Focus()


                End If


            Else

                cbo_grid_Additional_Weft_Details.Visible = False

            End If


            If e.ColumnIndex = 1 Then

                If Cbo_grid_Mts_Wgt.Visible = False Or Val(Cbo_grid_Mts_Wgt.Tag) <> e.RowIndex Then

                    rect = .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False)

                    Cbo_grid_Mts_Wgt.Left = .Left + rect.Left  '  .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Left
                    Cbo_grid_Mts_Wgt.Top = .Top + rect.Top  ' .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Top

                    Cbo_grid_Mts_Wgt.Width = rect.Width  ' .CurrentCell.Size.Width
                    Cbo_grid_Mts_Wgt.Height = rect.Height  ' rect.Height
                    Cbo_grid_Mts_Wgt.Text = .CurrentCell.Value  '  Trim(.CurrentRow.Cells(e.ColumnIndex).Value)

                    Cbo_grid_Mts_Wgt.Tag = Val(e.RowIndex)
                    Cbo_grid_Mts_Wgt.Visible = True

                    Cbo_grid_Mts_Wgt.BringToFront()
                    Cbo_grid_Mts_Wgt.Focus()


                End If


            Else

                Cbo_grid_Mts_Wgt.Visible = False

            End If

            If e.ColumnIndex = 2 Then

                If Cbo_Grid_Gram_Percentage.Visible = False Or Val(Cbo_Grid_Gram_Percentage.Tag) <> e.RowIndex Then

                    rect = .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False)

                    Cbo_Grid_Gram_Percentage.Left = .Left + rect.Left  '  .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Left
                    Cbo_Grid_Gram_Percentage.Top = .Top + rect.Top  ' .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Top

                    Cbo_Grid_Gram_Percentage.Width = rect.Width  ' .CurrentCell.Size.Width
                    Cbo_Grid_Gram_Percentage.Height = rect.Height  ' rect.Height
                    Cbo_Grid_Gram_Percentage.Text = .CurrentCell.Value  '  Trim(.CurrentRow.Cells(e.ColumnIndex).Value)

                    Cbo_Grid_Gram_Percentage.Tag = Val(e.RowIndex)
                    Cbo_Grid_Gram_Percentage.Visible = True

                    Cbo_Grid_Gram_Percentage.BringToFront()
                    Cbo_Grid_Gram_Percentage.Focus()

                End If

            Else

                Cbo_Grid_Gram_Percentage.Visible = False

            End If




        End With
    End Sub

    Private Sub dgv_Additional_Weft_Details_CellLeave(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Additional_Weft_Details.CellLeave
        With dgv_Additional_Weft_Details
            If .CurrentCell.ColumnIndex = 1 Then
                If Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value) <> 0 Then
                    .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = Format(Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value), "#########0.00000")
                End If
            End If
        End With
    End Sub

    Private Sub dgv_Additional_Weft_Details_EditingControlShowing(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewEditingControlShowingEventArgs) Handles dgv_Additional_Weft_Details.EditingControlShowing
        dgtxt_Additional_Weft_Details = CType(dgv_Additional_Weft_Details.EditingControl, DataGridViewTextBoxEditingControl)
    End Sub

    Private Sub dgv_Additional_Weft_Details_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_Additional_Weft_Details.KeyUp
        Dim i As Integer
        Dim n As Integer

        If e.Control = True And UCase(Chr(e.KeyCode)) = "D" Then

            With dgv_Additional_Weft_Details

                n = .CurrentRow.Index

                If .CurrentCell.RowIndex = .Rows.Count - 1 Then
                    For i = 0 To .ColumnCount - 1
                        .Rows(n).Cells(i).Value = ""
                    Next

                Else
                    .Rows.RemoveAt(n)

                End If

            End With

        End If
    End Sub

    Private Sub dgv_Additional_Weft_Details_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgv_Additional_Weft_Details.LostFocus
        On Error Resume Next
        If Not IsNothing(dgv_Additional_Weft_Details.CurrentCell) Then dgv_Additional_Weft_Details.CurrentCell.Selected = False
    End Sub

    Private Sub cbo_grid_Additional_Weft_Details_GotFocus(sender As Object, e As System.EventArgs) Handles cbo_grid_Additional_Weft_Details.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Count_Head", "Count_Name", "", "(Count_Idno = 0)")
    End Sub

    Private Sub cbo_grid_Additional_Weft_Details_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_grid_Additional_Weft_Details.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, sender, Nothing, Nothing, "Count_Head", "Count_Name", "", "(Count_Idno = 0)")

        With dgv_Additional_Weft_Details

            If (e.KeyValue = 38 And sender.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
                If .CurrentCell.RowIndex = 0 Then
                    '----

                Else
                    .Focus()
                    .CurrentCell = .Rows(.CurrentRow.Index - 1).Cells(.ColumnCount - 1)

                End If
            End If
            If (e.KeyValue = 40 And sender.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
                .Focus()
                .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 1)
            End If

        End With
    End Sub

    Private Sub cbo_grid_Additional_Weft_Details_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_grid_Additional_Weft_Details.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, sender, Nothing, "Count_Head", "Count_Name", "", "(Count_IdNo = 0)")
        If Asc(e.KeyChar) = 13 Then

            With dgv_Additional_Weft_Details
                If .CurrentCell.RowIndex = .Rows.Count - 1 And Trim(.Rows(.CurrentCell.RowIndex).Cells.Item(0).Value) = "" Then

                    If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                        save_record()
                    Else
                        txt_Name.Focus()
                    End If

                Else
                    .Focus()
                    .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)

                End If
            End With
        End If
    End Sub

    Private Sub cbo_grid_Additional_Weft_Details_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_grid_Additional_Weft_Details.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Dim f As New Count_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = sender.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()
        End If

    End Sub

    Private Sub cbo_grid_Additional_Weft_Details_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_grid_Additional_Weft_Details.TextChanged
        Try
            If cbo_grid_Additional_Weft_Details.Visible Then
                If IsNothing(dgv_Additional_Weft_Details.CurrentCell) Then Exit Sub
                With dgv_Additional_Weft_Details
                    If Val(cbo_grid_Additional_Weft_Details.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = 0 Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(0).Value = Trim(cbo_grid_Additional_Weft_Details.Text)
                    End If
                End With
            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub


    Private Sub dgtxt_Additional_Weft_Details_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgtxt_Additional_Weft_Details.Enter
        dgv_ActiveCtrl_Name = dgv_Additional_Weft_Details.Name
        dgv_Additional_Weft_Details.EditingControl.BackColor = Color.Lime
        dgv_Additional_Weft_Details.EditingControl.ForeColor = Color.Blue
    End Sub

    Private Sub dgtxt_Additional_Weft_Details_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dgtxt_Additional_Weft_Details.KeyPress
        With dgv_Additional_Weft_Details
            If .Visible Then
                If .CurrentCell.ColumnIndex = 1 Then
                    If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
                        e.Handled = True
                    End If
                End If
            End If
        End With
    End Sub

    Private Sub dgtxt_Additional_Weft_Details_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgtxt_Additional_Weft_Details.TextChanged
        Try
            With dgv_Additional_Weft_Details
                If .Visible Then
                    If .Rows.Count > 0 Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(dgtxt_Additional_Weft_Details.Text)
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

    Private Sub dgtxt_Additional_Weft_Details_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgtxt_Additional_Weft_Details.KeyUp
        Try
            With dgv_Additional_Weft_Details
                If .Visible = True Then
                    If .Rows.Count > 0 Then
                        If e.Control = True And UCase(Chr(e.KeyCode)) = "D" Then
                            dgv_Additional_Weft_Details_KeyUp(sender, e)
                        End If
                    End If
                End If
            End With

        Catch ex As Exception
            MessageBox.Show(ex.Message, "ERROR WHILE DETAILS TEXT KEYUP....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub cbo_WeaverWages_for_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_WeaverWages_for.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, sender, Nothing, Nothing, "", "", "", "")

        If (e.KeyValue = 38 And sender.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
            If txt_RollTube_Wgt.Enabled And txt_RollTube_Wgt.Visible Then
                txt_RollTube_Wgt.Focus()
            ElseIf txt_bale_weight_to.Enabled And txt_bale_weight_to.Visible Then
                txt_bale_weight_to.Focus()
            ElseIf txt_AllowShortage_Perc_Processing.Enabled And txt_AllowShortage_Perc_Processing.Visible Then
                txt_AllowShortage_Perc_Processing.Focus()
            Else
                txt_Weave.Focus()
            End If

        ElseIf (e.KeyValue = 40 And sender.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
            If MessageBox.Show("Do you want to save", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = DialogResult.Yes Then
                save_record()
            Else
                txt_Name.Focus()
            End If

        End If

    End Sub

    Private Sub cbo_WeaverWages_for_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_WeaverWages_for.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, sender, Nothing, "", "", "", "")
        If Asc(e.KeyChar) = 13 Then
            If MessageBox.Show("Do you want to save", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = DialogResult.Yes Then
                save_record()
            Else
                txt_Name.Focus()
            End If
        End If
    End Sub

    Private Sub Cbo_Pavu_Consumption_In_Meter_Weight_KeyPress(sender As Object, e As KeyPressEventArgs) Handles Cbo_Pavu_Consumption_In_Meter_Weight.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, sender, cbo_Fabric_Processing_Reconsilation_Mtrs_Wgt, "", "", "", "")
    End Sub

    Private Sub Cbo_Pavu_Consumption_In_Meter_Weight_KeyDown(sender As Object, e As KeyEventArgs) Handles Cbo_Pavu_Consumption_In_Meter_Weight.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, sender, txt_Allowed_Excess_Doff_Meters_Percentage, cbo_Fabric_Processing_Reconsilation_Mtrs_Wgt, "", "", "", "")
    End Sub

    Private Sub cbo_Fabric_Processing_Reconsilation_Mtrs_Wgt_KeyPress(sender As Object, e As KeyPressEventArgs) Handles cbo_Fabric_Processing_Reconsilation_Mtrs_Wgt.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Fabric_Processing_Reconsilation_Mtrs_Wgt, Nothing, "", "", "", "")


        If Asc(e.KeyChar) = 13 Then

            If dgv_EndsCountDetails.Rows.Count > 0 Then
                If dgv_EndsCountDetails.Rows.Count <= 0 Then dgv_EndsCountDetails.Rows.Add()
                dgv_EndsCountDetails.Focus()
                dgv_EndsCountDetails.CurrentCell = dgv_EndsCountDetails.Rows(0).Cells(1)
            Else
                cbo_EndsCountMainName.Focus()
            End If

        End If

    End Sub

    Private Sub cbo_Fabric_Processing_Reconsilation_Mtrs_Wgt_KeyDown(sender As Object, e As KeyEventArgs) Handles cbo_Fabric_Processing_Reconsilation_Mtrs_Wgt.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Fabric_Processing_Reconsilation_Mtrs_Wgt, Cbo_Pavu_Consumption_In_Meter_Weight, Nothing, "", "", "", "")


        If e.KeyCode = 40 Then
            If dgv_EndsCountDetails.Rows.Count > 0 Then
                If dgv_EndsCountDetails.Rows.Count <= 0 Then dgv_EndsCountDetails.Rows.Add()
                dgv_EndsCountDetails.Focus()
                dgv_EndsCountDetails.CurrentCell = dgv_EndsCountDetails.Rows(0).Cells(1)
            Else

                cbo_EndsCountMainName.Focus()
            End If

        End If
    End Sub

    Private Sub Chk_Multi_EndsCount_CheckedChanged(sender As Object, e As EventArgs) Handles Chk_Multi_EndsCount.CheckedChanged

        If Chk_Multi_EndsCount.Checked = True Then
            Btn_Warp_Consumption_Details.Visible = True
        Else
            Btn_Warp_Consumption_Details.Visible = False
        End If

    End Sub





    Private Sub Cbo_Grid_Mtrs_Wgt_TextChanged(sender As Object, e As EventArgs) Handles Cbo_grid_Mts_Wgt.TextChanged
        Try
            If Cbo_grid_Mts_Wgt.Visible Then
                If IsNothing(dgv_Additional_Weft_Details.CurrentCell) Then Exit Sub
                With dgv_Additional_Weft_Details
                    If Val(Cbo_grid_Mts_Wgt.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = 1 Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(1).Value = Trim(Cbo_grid_Mts_Wgt.Text)
                    End If
                End With
            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub



    Private Sub Cbo_Grid_Mtrs_Wgt_KeyPress(sender As Object, e As KeyPressEventArgs) Handles Cbo_grid_Mts_Wgt.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, Cbo_grid_Mts_Wgt, Nothing, "", "", "", "")

        With dgv_Additional_Weft_Details
            If Asc(e.KeyChar) = 13 Then
                .Focus()
                .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)
            End If
        End With


    End Sub

    Private Sub Cbo_Grid_Mtrs_Wgt_KeyDown(sender As Object, e As KeyEventArgs) Handles Cbo_grid_Mts_Wgt.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, Cbo_grid_Mts_Wgt, Nothing, Nothing, "", "", "", "")

        With dgv_Additional_Weft_Details
            If (e.KeyValue = 38 And Cbo_grid_Mts_Wgt.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
                .Focus()
                .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex - 1)
            End If

            If (e.KeyValue = 40 And Cbo_grid_Mts_Wgt.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
                .Focus()
                .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 1)
            End If
        End With




    End Sub

    Private Sub Cbo_Grid_Gram_Percentage_KeyPress(sender As Object, e As KeyPressEventArgs) Handles Cbo_Grid_Gram_Percentage.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, Cbo_Grid_Gram_Percentage, Nothing, "", "", "", "")

        With dgv_Additional_Weft_Details
            If Asc(e.KeyChar) = 13 Then
                .Focus()
                .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)
            End If
        End With
    End Sub

    Private Sub Cbo_Grid_Gram_Percentage_KeyDown(sender As Object, e As KeyEventArgs) Handles Cbo_Grid_Gram_Percentage.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, Cbo_Grid_Gram_Percentage, Nothing, Nothing, "", "", "", "")

        With dgv_Additional_Weft_Details

            If (e.KeyValue = 38 And Cbo_Grid_Gram_Percentage.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
                .Focus()
                .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex - 1)
            End If

            If (e.KeyValue = 40 And Cbo_Grid_Gram_Percentage.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
                .Focus()
                .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 1)
            End If

        End With
    End Sub

    Private Sub Cbo_Grid_Gram_Percentage_TextChanged(sender As Object, e As EventArgs) Handles Cbo_Grid_Gram_Percentage.TextChanged
        Try
            If Cbo_Grid_Gram_Percentage.Visible Then
                If IsNothing(dgv_Additional_Weft_Details.CurrentCell) Then Exit Sub
                With dgv_Additional_Weft_Details
                    If Val(Cbo_Grid_Gram_Percentage.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = 2 Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(2).Value = Trim(Cbo_Grid_Gram_Percentage.Text)
                    End If
                End With
            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub Chk_Multi_Weft_Count_CheckedChanged(sender As Object, e As EventArgs) Handles Chk_Multi_Weft_Count.CheckedChanged

        If Chk_Multi_Weft_Count.Checked = True Then
            btn_Weft_Consumption_Details.Visible = True
        Else
            btn_Weft_Consumption_Details.Visible = False
        End If

    End Sub

    Private Sub Dgv_Warp_Count_Details_CellEnter(sender As Object, e As DataGridViewCellEventArgs) Handles Dgv_Warp_Count_Details.CellEnter
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim Dt2 As New DataTable
        Dim Dt3 As New DataTable
        Dim rect As Rectangle

        With Dgv_Warp_Count_Details

            dgv_ActiveCtrl_Name = .Name

            If e.ColumnIndex = 0 Then

                If Cbo_Grid_EndsCount.Visible = False Or Val(Cbo_Grid_EndsCount.Tag) <> e.RowIndex Then

                    rect = .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False)

                    Cbo_Grid_EndsCount.Left = .Left + rect.Left  '  .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Left
                    Cbo_Grid_EndsCount.Top = .Top + rect.Top  ' .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Top

                    Cbo_Grid_EndsCount.Width = rect.Width  ' .CurrentCell.Size.Width
                    Cbo_Grid_EndsCount.Height = rect.Height  ' rect.Height
                    Cbo_Grid_EndsCount.Text = .CurrentCell.Value  '  Trim(.CurrentRow.Cells(e.ColumnIndex).Value)

                    Cbo_Grid_EndsCount.Tag = Val(e.RowIndex)
                    Cbo_Grid_EndsCount.Visible = True

                    Cbo_Grid_EndsCount.BringToFront()
                    Cbo_Grid_EndsCount.Focus()


                End If


            Else

                Cbo_Grid_EndsCount.Visible = False

            End If

            If e.ColumnIndex = 1 Then

                If Cbo_Grid_Pile_Ground.Visible = False Or Val(Cbo_Grid_Pile_Ground.Tag) <> e.RowIndex Then

                    rect = .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False)

                    Cbo_Grid_Pile_Ground.Left = .Left + rect.Left  '  .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Left
                    Cbo_Grid_Pile_Ground.Top = .Top + rect.Top  ' .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Top

                    Cbo_Grid_Pile_Ground.Width = rect.Width  ' .CurrentCell.Size.Width
                    Cbo_Grid_Pile_Ground.Height = rect.Height  ' rect.Height
                    Cbo_Grid_Pile_Ground.Text = .CurrentCell.Value  '  Trim(.CurrentRow.Cells(e.ColumnIndex).Value)

                    Cbo_Grid_Pile_Ground.Tag = Val(e.RowIndex)
                    Cbo_Grid_Pile_Ground.Visible = True

                    Cbo_Grid_Pile_Ground.BringToFront()
                    Cbo_Grid_Pile_Ground.Focus()


                End If


            Else

                Cbo_Grid_Pile_Ground.Visible = False

            End If

        End With
    End Sub

    Private Sub Dgv_Warp_Count_Details_EditingControlShowing(sender As Object, e As DataGridViewEditingControlShowingEventArgs) Handles Dgv_Warp_Count_Details.EditingControlShowing
        dgtxt_Warp_Details = CType(Dgv_Warp_Count_Details.EditingControl, DataGridViewTextBoxEditingControl)
    End Sub

    Private Sub Dgv_Warp_Count_Details_KeyUp(sender As Object, e As KeyEventArgs) Handles Dgv_Warp_Count_Details.KeyUp
        Dim i As Integer
        Dim n As Integer

        If e.Control = True And UCase(Chr(e.KeyCode)) = "D" Then

            With Dgv_Warp_Count_Details

                n = .CurrentRow.Index

                If .CurrentCell.RowIndex = .Rows.Count - 1 Then
                    For i = 0 To .ColumnCount - 1
                        .Rows(n).Cells(i).Value = ""
                    Next

                Else
                    .Rows.RemoveAt(n)

                End If

            End With

        End If
    End Sub

    Private Sub Dgv_Warp_Count_Details_LostFocus(sender As Object, e As EventArgs) Handles Dgv_Warp_Count_Details.LostFocus
        On Error Resume Next
        Dgv_Warp_Count_Details.CurrentCell.Selected = False
    End Sub

    Private Sub Btn_Warp_Consumption_Details_Click(sender As Object, e As EventArgs) Handles Btn_Warp_Consumption_Details.Click
        pnl_Back.Enabled = False
        Pnl_Warp_Consumption_Details.Visible = True
        If Dgv_Warp_Count_Details.Rows.Count > 0 Then
            Dgv_Warp_Count_Details.Focus()
            Dgv_Warp_Count_Details.CurrentCell = Dgv_Warp_Count_Details.Rows(0).Cells(0)
        End If
    End Sub

    Private Sub Btn_Close_Warp_Consumption_Click(sender As Object, e As EventArgs) Handles Btn_Close_Warp_Consumption.Click
        pnl_Back.Enabled = True
        Pnl_Warp_Consumption_Details.Visible = False
    End Sub

    Private Sub Cbo_Grid_Pile_Ground_KeyPress(sender As Object, e As KeyPressEventArgs) Handles Cbo_Grid_Pile_Ground.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, Cbo_Grid_Pile_Ground, Nothing, "", "", "", "")


        With Dgv_Warp_Count_Details

            If Asc(e.KeyChar) = 13 Then
                .Focus()
                .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)
            End If

        End With
    End Sub

    Private Sub Cbo_Grid_Pile_Ground_KeyDown(sender As Object, e As KeyEventArgs) Handles Cbo_Grid_Pile_Ground.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, Cbo_Grid_Pile_Ground, Nothing, Nothing, "", "", "", "")

        With Dgv_Warp_Count_Details


            If (e.KeyValue = 38 And Cbo_Grid_Pile_Ground.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then


                .Focus()
                .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex - 1)
            End If

            If (e.KeyValue = 40 And Cbo_Grid_Pile_Ground.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
                .Focus()
                .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 1)
            End If



            'If (e.KeyValue = 38 And sender.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then

            '    If .CurrentCell.RowIndex = 0 Then
            '        '----

            '    Else
            '        .Focus()
            '        .CurrentCell = .Rows(.CurrentRow.Index - 1).Cells(.ColumnCount - 1)

            '    End If

            'End If
            'If (e.KeyValue = 40 And sender.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
            '    .Focus()
            '    .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 1)
            'End If

        End With
    End Sub



    Private Sub Cbo_Grid_EndsCount_KeyPress(sender As Object, e As KeyPressEventArgs) Handles Cbo_Grid_EndsCount.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, Cbo_Grid_EndsCount, Nothing, "", "", "", "")

        With Dgv_Warp_Count_Details

            If Asc(e.KeyChar) = 13 Then
                .Focus()
                .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)
            End If

        End With


    End Sub

    Private Sub Cbo_Grid_EndsCount_KeyDown(sender As Object, e As KeyEventArgs) Handles Cbo_Grid_EndsCount.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, Cbo_Grid_EndsCount, Nothing, Nothing, "", "", "", "")

        With Dgv_Warp_Count_Details


            If (e.KeyValue = 38 And sender.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
                If .CurrentCell.RowIndex = 0 Then
                    '----

                Else
                    .Focus()
                    .CurrentCell = .Rows(.CurrentRow.Index - 1).Cells(.ColumnCount - 1)

                End If
            End If
            If (e.KeyValue = 40 And sender.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
                .Focus()
                .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 1)
            End If



        End With

    End Sub

    Private Sub Cbo_Grid_EndsCount_KeyUp(sender As Object, e As KeyEventArgs) Handles Cbo_Grid_EndsCount.KeyUp
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

    Private Sub Cbo_Grid_EndsCount_GotFocus(sender As Object, e As EventArgs) Handles Cbo_Grid_EndsCount.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "EndsCount_Head", "EndsCount_Name", "", "(EndsCount_Idno = 0)")
    End Sub

    Private Sub Cbo_Grid_EndsCount_TextChanged(sender As Object, e As EventArgs) Handles Cbo_Grid_EndsCount.TextChanged
        Try
            If Cbo_Grid_EndsCount.Visible Then
                If IsNothing(Dgv_Warp_Count_Details.CurrentCell) Then Exit Sub
                With Dgv_Warp_Count_Details
                    If Val(Cbo_Grid_EndsCount.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = 0 Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(0).Value = Trim(Cbo_Grid_EndsCount.Text)
                    End If
                End With
            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub Cbo_Grid_Pile_Ground_TextChanged(sender As Object, e As EventArgs) Handles Cbo_Grid_Pile_Ground.TextChanged
        Try
            If Cbo_Grid_Pile_Ground.Visible Then
                If IsNothing(Dgv_Warp_Count_Details.CurrentCell) Then Exit Sub
                With Dgv_Warp_Count_Details
                    If Val(Cbo_Grid_Pile_Ground.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = 1 Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(1).Value = Trim(Cbo_Grid_Pile_Ground.Text)
                    End If
                End With
            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub
    Private Sub txt_Employee_Wages_Per_Meter_KeyDown(sender As Object, e As KeyEventArgs) Handles txt_Employee_Wages_Per_Meter.KeyDown
        If e.KeyCode = 38 Then
            txt_Weave.Focus()
        ElseIf e.KeyCode = 40 Then
            If MessageBox.Show("Do you want to save", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = DialogResult.Yes Then
                save_record()
            Else

                txt_Name.Focus()
            End If
        End If
    End Sub

    Private Sub txt_Employee_Wages_Per_Meter_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txt_Employee_Wages_Per_Meter.KeyPress
        If Asc(e.KeyChar) = 13 Then
            If MessageBox.Show("Do you want to save", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = DialogResult.Yes Then
                save_record()
            Else

                txt_Name.Focus()
            End If
        End If
    End Sub

    Private Sub dgtxt_Warp_Details_TextChanged(sender As Object, e As EventArgs) Handles dgtxt_Warp_Details.TextChanged
        Try
            With Dgv_Warp_Count_Details
                If .Visible Then
                    If .Rows.Count > 0 Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(sender.Text)
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

    Private Sub dgtxt_Warp_Details_Enter(sender As Object, e As EventArgs) Handles dgtxt_Warp_Details.Enter
        dgtxt_Warp_Details.BackColor = Color.Lime
        dgtxt_Warp_Details.ForeColor = Color.Blue
    End Sub

    Private Sub dgtxt_Warp_Details_Leave(sender As Object, e As EventArgs) Handles dgtxt_Warp_Details.Leave
        dgtxt_Warp_Details.BackColor = Color.White
        dgtxt_Warp_Details.ForeColor = Color.Black
    End Sub

    Private Sub Cbo_Pavu_Consumption_In_Meter_Weight_Enter(sender As Object, e As EventArgs) Handles Cbo_Pavu_Consumption_In_Meter_Weight.Enter
        sender.BackColor = Color.Lime
        sender.ForeColor = Color.Blue
    End Sub

    Private Sub Cbo_Pavu_Consumption_In_Meter_Weight_Leave(sender As Object, e As EventArgs) Handles Cbo_Pavu_Consumption_In_Meter_Weight.Leave
        sender.BackColor = Color.White
        sender.ForeColor = Color.Black
    End Sub

    Private Sub btn_Mark_WagesDetails_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Mark_WagesDetails.Click
        pnl_Back.Enabled = False
        pnl_Mark_Wages.Visible = True
        If dgv_MarkWages_Details.Enabled And dgv_MarkWages_Details.Visible Then
            dgv_MarkWages_Details.Focus()
            dgv_MarkWages_Details.CurrentCell = dgv_MarkWages_Details.Rows(0).Cells(0)
        End If
    End Sub
    Private Sub btn_Close_Mark_Wages_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Close_Mark_Wages.Click
        pnl_Back.Enabled = True
        pnl_Mark_Wages.Visible = False
        txt_Name.Focus()
    End Sub
    Private Sub btn_Close_2_Mark_Wages_Click(sender As Object, e As EventArgs) Handles btn_Close_2_Mark_wages.Click
        btn_Close_Mark_Wages_Click(sender, e)
    End Sub
    Private Sub dgv_MarkWages_Details_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_MarkWages_Details.KeyUp
        Dim n As Integer

        Try
            If e.Control = True And UCase(Chr(e.KeyCode)) = "D" Then
                With dgv_MarkWages_Details

                    n = .CurrentRow.Index

                    If n = .Rows.Count - 1 Then
                        For i = 0 To .ColumnCount - 1
                            .Rows(n).Cells(i).Value = ""
                        Next

                    Else
                        .Rows.RemoveAt(n)

                    End If

                End With

            End If

        Catch ex As Exception
            '---
        End Try

    End Sub

    Private Sub dgv_MarkWages_Details_CellEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_MarkWages_Details.CellEnter
        Dim CmpGrp_Fromdate As Date


        If FrmLdSTS = True Then Exit Sub
        With dgv_MarkWages_Details

            dgv_ActiveCtrl_Name = dgv_MarkWages_Details.Name


        End With
    End Sub
    Private Sub dgv_MarkWages_Details_EditingControlShowing(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewEditingControlShowingEventArgs) Handles dgv_MarkWages_Details.EditingControlShowing
        dgtxt_FoldingWages_Details = CType(dgv_MarkWages_Details.EditingControl, DataGridViewTextBoxEditingControl)
    End Sub
    Private Sub dgtxt_FoldingWages_Details_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgtxt_FoldingWages_Details.Enter
        Try
            dgv_ActiveCtrl_Name = dgv_MarkWages_Details.Name

            dgv_MarkWages_Details.EditingControl.BackColor = Color.Lime
            dgv_MarkWages_Details.EditingControl.ForeColor = Color.Blue
            dgtxt_FoldingWages_Details.SelectAll()
        Catch ex As Exception
            '--
        End Try
    End Sub

    Private Sub dgtxt_FoldingWages_Details_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dgtxt_FoldingWages_Details.KeyPress
        Try
            With dgv_MarkWages_Details
                If .Visible Then

                    If .CurrentCell.ColumnIndex = 1 Then
                        If Common_Procedures.Accept_NegativeNumbers(Asc(e.KeyChar)) = 0 Then e.Handled = True

                    End If
                End If
            End With
        Catch ex As Exception
            '---
        End Try
    End Sub

    Private Sub txt_Name_GotFocus(sender As Object, e As EventArgs) Handles txt_Name.GotFocus
        txt_Name.BackColor = Color.Lime
        txt_Name.ForeColor = Color.Blue
        txt_Name.SelectAll()
    End Sub

    Private Sub txt_Name_LostFocus(sender As Object, e As EventArgs) Handles txt_Name.LostFocus
        txt_Name.BackColor = Color.White
        txt_Name.ForeColor = Color.Black
    End Sub

    Private Sub txt_checking_wages_per_meter_KeyDown(sender As Object, e As KeyEventArgs) Handles txt_checking_wages_per_meter.KeyDown
        If e.KeyCode = 38 Then
            e.Handled = True
            e.SuppressKeyPress = True
            cbo_ClothSet.Focus()
        End If
        If e.KeyCode = 40 Then
            e.Handled = True
            e.SuppressKeyPress = True
            txt_folding_wages_per_meter.Focus()
        End If
    End Sub

    Private Sub txt_checking_wages_per_meter_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txt_checking_wages_per_meter.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
        If Asc(e.KeyChar) = 13 Then
            e.Handled = True
            txt_folding_wages_per_meter.Focus()
        End If
    End Sub

    Private Sub txt_folding_wages_per_meter_KeyDown(sender As Object, e As KeyEventArgs) Handles txt_folding_wages_per_meter.KeyDown
        If e.KeyCode = 38 Then
            e.Handled = True
            e.SuppressKeyPress = True
            txt_checking_wages_per_meter.Focus()
        End If
        If e.KeyCode = 40 Then
            e.Handled = True
            e.SuppressKeyPress = True
            If dgv_EndsCountDetails.Rows.Count > 0 Then
                If dgv_EndsCountDetails.Rows.Count <= 0 Then dgv_EndsCountDetails.Rows.Add()
                dgv_EndsCountDetails.Focus()
                dgv_EndsCountDetails.CurrentCell = dgv_EndsCountDetails.Rows(0).Cells(1)
            Else
                cbo_EndsCountMainName.Focus()
            End If
        End If
    End Sub


    Private Sub txt_folding_wages_per_meter_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txt_folding_wages_per_meter.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
        If Asc(e.KeyChar) = 13 Then
            e.Handled = True
            If dgv_EndsCountDetails.Rows.Count > 0 Then
                If dgv_EndsCountDetails.Rows.Count <= 0 Then dgv_EndsCountDetails.Rows.Add()
                dgv_EndsCountDetails.Focus()
                dgv_EndsCountDetails.CurrentCell = dgv_EndsCountDetails.Rows(0).Cells(1)
            Else
                cbo_EndsCountMainName.Focus()
            End If
        End If
    End Sub

    Private Sub cbo_ClothSet_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbo_ClothSet.SelectedIndexChanged

    End Sub

    Private Sub cbo_EndsCount_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbo_EndsCount.SelectedIndexChanged

    End Sub

    Private Sub cbo_EndsCountMainName_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbo_EndsCountMainName.SelectedIndexChanged

    End Sub
End Class
