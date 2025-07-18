Public Class Doffing_Piece_Checking_Entry
    Implements Interface_MDIActions

    Private con As New SqlClient.SqlConnection(Common_Procedures.Connection_String)
    Private con1 As New SqlClient.SqlConnection(Common_Procedures.ConnectionString_CompanyGroupdetails)
    Private FrmLdSTS As Boolean = False
    Private New_Entry As Boolean = False
    Private Insert_Entry As Boolean = False
    Private Filter_Status As Boolean = False
    Private Pk_Condition As String = "INCHK-"
    Private Prec_ActCtrl As New Control
    Private vcbo_KeyDwnVal As Double
    Public vmskOldText As String = ""
    Public vmskSelStrt As Integer = -1
    Private SaveAll_Sts As Boolean = False

    Private prn_HdDt As New DataTable
    Private prn_DetDt As New DataTable
    Private prn_PageNo As Integer
    Private prn_DetIndx As Integer
    Private prn_DetAr(50, 10) As String
    Private prn_DetMxIndx As Integer
    Private prn_NoofBmDets As Integer
    Private prn_DetSNo As Integer
    Private prn_DetBarCdStkr As Integer
    Private LastNo As String = ""
    Private WithEvents dgtxt_Details As New DataGridViewTextBoxEditingControl
    Private dgv_ActCtrlName As String = ""
    Private dgvDet_CboBx_ColNos_Arr As Integer() = {1}

    Private prn_HeadIndx As Integer

    Private vCLO_CONDT As String = ""
    Private FnYearCode1 As String = ""
    Private FnYearCode2 As String = ""

    Private Enum dgvCol_Details As Integer
        Pcsno   '0
        ClothType '1
        Meters '2
        Weight '3
        Wgt_Mtr '4
        Sts  '5
        packslipcode '6
        Lot_No '7
    End Enum

    Public Sub New()
        FrmLdSTS = True
        ' This call is required by the designer.
        InitializeComponent()
        ' Add any initialization after the InitializeComponent() call.
    End Sub

    Private Sub clear()
        New_Entry = False
        Insert_Entry = False

        lbl_NewSTS.Visible = False

        pnl_back.Enabled = True
        pnl_filter.Visible = False
        pnl_Selection.Visible = False
        pnl_Weft_Consumption_Details.Visible = False
        btn_Show_WeftConsumption_Details.Visible = False

        vmskOldText = ""
        vmskSelStrt = -1

        txt_Roll_SuffixNo.Text = ""
        lbl_RollNo.Text = ""
        lbl_RollNo.ForeColor = Color.Black
        msk_Date.Text = ""
        dtp_Date.Text = ""
        lbl_WeftCount.Text = ""
        lbl_PartyName.Text = ""
        lbl_PartyName.Tag = ""
        cbo_ClothName.Text = ""
        cbo_ClothName.Tag = ""
        lbl_WidthType.Text = ""
        lbl_EndsCount.Text = ""
        lbl_EndsCount_Beam1.Text = ""
        lbl_EndsCount_Beam2.Text = ""
        txt_BarCode.Text = ""
        txt_BarCode.Enabled = False
        lbl_poNo.Text = ""

        lbl_KnotNo.Text = ""
        lbl_KnotCode.Text = ""

        lbl_ExcSht.Text = ""
        txt_Folding.Text = "100"

        cbo_Pcs_LastPiece_Status.Text = "NO"
        cbo_Pcs_LastPiece_Status.Tag = ""

        lbl_SetCode1.Text = ""
        lbl_SetNo1.Text = ""
        lbl_TotMtrs1.Text = ""
        lbl_TotMtrs2.Text = ""
        lbl_SetCode2.Text = ""
        lbl_SetNo2.Text = ""
        cbo_LoomNo.Text = ""
        cbo_LoomNo.Tag = ""
        lbl_BeamNo1.Text = ""
        lbl_BeamNo2.Text = ""
        txt_DoffMtrs.Text = ""
        txt_CrimpPerc.Text = ""
        lbl_ConsPavu.Text = ""
        lbl_ConsPavu_Beam1.Text = ""
        lbl_ConsPavu_Beam2.Text = ""
        lbl_BeamConsPavu.Text = ""
        lbl_ConsWeftYarn.Text = ""
        lbl_BalMtrs1.Text = ""
        lbl_BalMtrs2.Text = ""
        cbo_SelectionLoomNo.Text = ""
        lbl_Weaver_Job_No.Text = ""

        lbl_ClothSales_OrderCode_forSelection.Text = ""

        lbl_UserName.Text = "USER : " & Trim(UCase(Common_Procedures.User_IdNoToName(con1, Common_Procedures.User.IdNo)))
        cbo_LoomNo.Enabled = True
        cbo_LoomNo.BackColor = Color.White

        cbo_ClothName.Enabled = True
        cbo_ClothName.BackColor = Color.White

        txt_DoffMtrs.Enabled = True
        txt_DoffMtrs.BackColor = Color.White

        txt_CrimpPerc.Enabled = True
        txt_CrimpPerc.BackColor = Color.White

        txt_Folding.Enabled = True
        txt_Folding.BackColor = Color.White


        dgv_Details.Rows.Clear()
        dgv_Details_Total.Rows.Clear()
        dgv_Details_Total.Rows.Add()


        cbo_Grid_ClothType.Text = ""
        cbo_Grid_ClothType.Visible = False

        pnl_Weft_Consumption_Details.Visible = False
        dgv_Weft_Consumption_Details.Rows.Clear()

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1558" Then '---- SOTEXPA QUALIDIS TEXTILE (SULUR)
            set_grid_Meters_ColumnHeading("CLEAR")
        End If

    End Sub

    Private Sub ControlGotFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim txtbx As TextBox
        Dim combobx As ComboBox
        Dim msktxbx As MaskedTextBox
        On Error Resume Next

        If TypeOf Me.ActiveControl Is TextBox Or TypeOf Me.ActiveControl Is ComboBox Or TypeOf Me.ActiveControl Is Button Or TypeOf Me.ActiveControl Is MaskedTextBox Then
            Me.ActiveControl.BackColor = Color.Lime
            Me.ActiveControl.ForeColor = Color.Blue
        End If

        If TypeOf Me.ActiveControl Is TextBox Then
            txtbx = Me.ActiveControl
            txtbx.SelectAll()
        ElseIf TypeOf Me.ActiveControl Is MaskedTextBox Then
            msktxbx = Me.ActiveControl
            msktxbx.SelectionStart = 0
        ElseIf TypeOf Me.ActiveControl Is ComboBox Then
            combobx = Me.ActiveControl
            combobx.SelectAll()
        End If

        If Me.ActiveControl.Name <> cbo_Grid_ClothType.Name Then
            cbo_Grid_ClothType.Visible = False
        End If

        If Me.ActiveControl.Name <> dgv_Details.Name Then
            Grid_DeSelect()
        End If

        If Me.ActiveControl.Name <> dgv_Details_Total.Name Then
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
            ElseIf TypeOf Prec_ActCtrl Is Button Then
                Prec_ActCtrl.BackColor = Color.FromArgb(2, 57, 111)
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

    Private Sub Grid_DeSelect()
        On Error Resume Next
        If FrmLdSTS = True Or Filter_Status = True Then Exit Sub
        If Not IsNothing(dgv_Details.CurrentCell) Then dgv_Details.CurrentCell.Selected = False
        If Not IsNothing(dgv_Details_Total.CurrentCell) Then dgv_Details_Total.CurrentCell.Selected = False
    End Sub
    Private Sub Grid_Cell_DeSelect()
        On Error Resume Next
        If Not IsNothing(dgv_Details.CurrentCell) Then dgv_Details.CurrentCell.Selected = False
        If Not IsNothing(dgv_Details_Total.CurrentCell) Then dgv_Details_Total.CurrentCell.Selected = False
        If Not IsNothing(dgv_filter.CurrentCell) Then dgv_filter.CurrentCell.Selected = False
    End Sub

    Private Sub move_record(ByVal no As String)
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim NewCode As String
        Dim LockSTS As Boolean = False
        Dim BmRunOutCd As String = ""

        Dim da3 As New SqlClient.SqlDataAdapter
        Dim dt3 As New DataTable
        Dim dt4 As New DataTable
        Dim n As Integer, i As Integer, j As Integer
        Dim SNo As Integer = 0

        If Val(no) = 0 Then Exit Sub

        clear()

        Try

            NewCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(no) & "/" & Trim(Common_Procedures.FnYearCode)

            da1 = New SqlClient.SqlDataAdapter("Select a.*, b.Ledger_Name, c.Cloth_Name, c.Stock_In, c.Multiple_WeftCount_Status, d.EndsCount_Name, e.Count_Name, f.Loom_Name , tEC1.EndsCount_Name as EndsCountName_Beam1, tEC2.EndsCount_Name as EndsCountName_Beam2 from Weaver_Cloth_Receipt_Head a INNER JOIN Ledger_Head b ON a.Ledger_IdNo = b.Ledger_IdNo INNER JOIN Cloth_Head c ON a.Cloth_IdNo = c.Cloth_IdNo LEFT OUTER JOIN EndsCount_Head d ON a.EndsCount_IdNo = d.EndsCount_IdNo LEFT OUTER JOIN Count_Head e ON a.Count_IdNo = e.Count_IdNo LEFT OUTER JOIN Loom_Head f ON a.Loom_IdNo = f.Loom_IdNo LEFT OUTER JOIN EndsCount_Head tEC1 ON a.EndsCount1_IdNo = tEC1.EndsCount_IdNo  LEFT OUTER JOIN EndsCount_Head tEC2 ON a.EndsCount2_IdNo = tEC2.EndsCount_IdNo Where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Weaver_ClothReceipt_Code = '" & Trim(NewCode) & "' and a.Receipt_Type = 'L'", con)
            dt1 = New DataTable
            da1.Fill(dt1)

            If dt1.Rows.Count > 0 Then
                lbl_RollNo.Text = dt1.Rows(0).Item("Weaver_ClothReceipt_RefNo").ToString
                'lbl_RollNo.Text = dt1.Rows(0).Item("Weaver_ClothReceipt_No").ToString
                txt_Roll_SuffixNo.Text = dt1.Rows(0).Item("Weaver_ClothReceipt_SuffixNo").ToString

                dtp_Date.Text = dt1.Rows(0).Item("Weaver_ClothReceipt_Date").ToString
                msk_Date.Text = dtp_Date.Text
                lbl_PartyName.Text = dt1.Rows(0).Item("Ledger_Name").ToString
                cbo_LoomNo.Text = dt1.Rows(0).Item("Loom_Name").ToString
                txt_BarCode.Text = dt1.Rows(0).Item("Bar_Code").ToString

                lbl_KnotCode.Text = dt1.Rows(0).Item("Beam_Knotting_Code").ToString
                lbl_KnotNo.Text = dt1.Rows(0).Item("Beam_Knotting_No").ToString

                cbo_Pcs_LastPiece_Status.Text = dt1.Rows(0).Item("Is_LastPiece").ToString
                cbo_Pcs_LastPiece_Status.Tag = cbo_Pcs_LastPiece_Status.Text

                cbo_ClothName.Text = dt1.Rows(0).Item("Cloth_Name").ToString
                set_grid_Meters_ColumnHeading(dt1.Rows(0).Item("Stock_In").ToString)
                lbl_WidthType.Text = dt1.Rows(0).Item("Width_Type").ToString
                lbl_EndsCount.Text = dt1.Rows(0).Item("EndsCount_Name").ToString
                lbl_EndsCount_Beam1.Text = dt1.Rows(0).Item("EndsCountName_Beam1").ToString
                lbl_EndsCount_Beam2.Text = dt1.Rows(0).Item("EndsCountName_Beam2").ToString

                If Val(dt1.Rows(0).Item("Multiple_WeftCount_Status").ToString) = 1 Then
                    btn_Show_WeftConsumption_Details.Visible = True
                    btn_Show_WeftConsumption_Details.BringToFront()
                    lbl_WeftCount.Text = ""
                Else
                    lbl_WeftCount.Text = dt1.Rows(0).Item("Count_Name").ToString
                    btn_Show_WeftConsumption_Details.Visible = False
                End If

                lbl_SetCode1.Text = dt1.Rows(0).Item("Set_Code1").ToString
                lbl_SetNo1.Text = dt1.Rows(0).Item("Set_No1").ToString
                lbl_BeamNo1.Text = dt1.Rows(0).Item("Beam_No1").ToString
                lbl_BalMtrs1.Text = dt1.Rows(0).Item("Balance_Meters1").ToString
                txt_Folding.Text = dt1.Rows(0).Item("Folding").ToString
                lbl_Weaver_Job_No.Text = dt1.Rows(0).Item("Weaving_JobCode_forSelection").ToString

                lbl_ClothSales_OrderCode_forSelection.Text = dt1.Rows(0).Item("ClothSales_OrderCode_forSelection").ToString

                lbl_UserName.Text = "USER : " & Trim(UCase(Common_Procedures.User_IdNoToName(con1, Val(dt1.Rows(0).Item("User_IdNo").ToString))))
                lbl_TotMtrs1.Text = ""
                lbl_poNo.Text = dt1.Rows(0).Item("Po_No").ToString
                da2 = New SqlClient.SqlDataAdapter("Select * from Stock_SizedPavu_Processing_Details Where Set_Code = '" & Trim(lbl_SetCode1.Text) & "' and Beam_No = '" & Trim(lbl_BeamNo1.Text) & "'", con)
                dt2 = New DataTable
                da2.Fill(dt2)
                If dt2.Rows.Count > 0 Then
                    lbl_TotMtrs1.Text = dt2.Rows(0).Item("Meters").ToString
                End If
                dt2.Clear()

                lbl_SetCode2.Text = dt1.Rows(0).Item("Set_Code2").ToString
                lbl_SetNo2.Text = dt1.Rows(0).Item("Set_No2").ToString
                lbl_BeamNo2.Text = dt1.Rows(0).Item("Beam_No2").ToString
                lbl_BalMtrs2.Text = dt1.Rows(0).Item("Balance_Meters2").ToString
                lbl_TotMtrs2.Text = ""
                da2 = New SqlClient.SqlDataAdapter("Select * from Stock_SizedPavu_Processing_Details Where Set_Code = '" & Trim(lbl_SetCode2.Text) & "' and Beam_No = '" & Trim(lbl_BeamNo2.Text) & "'", con)
                dt2 = New DataTable
                da2.Fill(dt2)
                If dt2.Rows.Count > 0 Then
                    lbl_TotMtrs2.Text = dt2.Rows(0).Item("Meters").ToString
                End If
                dt2.Clear()

                txt_DoffMtrs.Text = dt1.Rows(0).Item("ReceiptMeters_Receipt").ToString
                txt_CrimpPerc.Text = dt1.Rows(0).Item("Crimp_Percentage").ToString
                lbl_ConsPavu.Text = dt1.Rows(0).Item("ConsumedPavu_Receipt").ToString
                lbl_ConsPavu_Beam1.Text = dt1.Rows(0).Item("ConsumedPavu_Checking_Beam1").ToString
                lbl_ConsPavu_Beam2.Text = dt1.Rows(0).Item("ConsumedPavu_Checking_Beam2").ToString
                lbl_ConsWeftYarn.Text = dt1.Rows(0).Item("ConsumedYarn_Receipt").ToString
                lbl_BeamConsPavu.Text = dt1.Rows(0).Item("BeamConsumption_Receipt").ToString

                LockSTS = False

                da3 = New SqlClient.SqlDataAdapter("Select a.*, b.Loom_Name from Weaver_ClothReceipt_Piece_Details a LEFT OUTER JOIN Loom_Head b ON a.Loom_IdNo = b.Loom_IdNo where a.Weaver_Piece_Checking_Code = '" & Trim(NewCode) & "' Order by a.PieceNo_OrderBy, a.Piece_No", con)
                dt3 = New DataTable
                da3.Fill(dt3)

                With dgv_Details

                    .Rows.Clear()
                    SNo = 0

                    If dt3.Rows.Count > 0 Then

                        For i = 0 To dt3.Rows.Count - 1

                            n = .Rows.Add()

                            .Rows(n).Cells(dgvCol_Details.Pcsno).Value = dt3.Rows(i).Item("Piece_No").ToString
                            .Rows(n).Cells(dgvCol_Details.Sts).Value = ""
                            .Rows(n).Cells(dgvCol_Details.packslipcode).Value = ""
                            .Rows(n).Cells(dgvCol_Details.Lot_No).Value = dt3.Rows(i).Item("Lot_NUmber").ToString
                            If Val(dt3.Rows(i).Item("Type1_Meters").ToString) <> 0 Then
                                .Rows(n).Cells(dgvCol_Details.ClothType).Value = Common_Procedures.ClothType.Type1
                                .Rows(n).Cells(dgvCol_Details.Meters).Value = Format(Val(dt3.Rows(i).Item("Type1_Meters").ToString), "########0.00")
                                .Rows(n).Cells(dgvCol_Details.packslipcode).Value = dt3.Rows(i).Item("PackingSlip_Code_Type1").ToString
                                If Trim(.Rows(n).Cells(dgvCol_Details.packslipcode).Value) = "" Then .Rows(n).Cells(dgvCol_Details.packslipcode).Value = dt3.Rows(i).Item("BuyerOffer_Code_Type1").ToString
                                If Trim(.Rows(n).Cells(dgvCol_Details.packslipcode).Value) <> "" Then
                                    .Rows(n).Cells(dgvCol_Details.Sts).Value = "1"

                                    LockSTS = True
                                    For j = 0 To .ColumnCount - 1
                                        .Rows(n).Cells(j).Style.BackColor = Color.Gainsboro
                                        .Rows(n).Cells(j).Style.ForeColor = Color.Red
                                        .Rows(n).ReadOnly = True
                                    Next
                                End If

                            ElseIf Val(dt3.Rows(i).Item("Type2_Meters").ToString) <> 0 Then
                                .Rows(n).Cells(dgvCol_Details.ClothType).Value = Common_Procedures.ClothType.Type2
                                .Rows(n).Cells(dgvCol_Details.Meters).Value = Format(Val(dt3.Rows(i).Item("Type2_Meters").ToString), "########0.00")
                                .Rows(n).Cells(dgvCol_Details.packslipcode).Value = dt3.Rows(i).Item("PackingSlip_Code_Type2").ToString
                                If Trim(.Rows(n).Cells(dgvCol_Details.packslipcode).Value) = "" Then .Rows(n).Cells(dgvCol_Details.packslipcode).Value = dt3.Rows(i).Item("BuyerOffer_Code_Type2").ToString
                                If Trim(.Rows(n).Cells(dgvCol_Details.packslipcode).Value) <> "" Then
                                    .Rows(n).Cells(dgvCol_Details.Sts).Value = "1"
                                    .Rows(n).Cells(dgvCol_Details.ClothType).Style.ForeColor = Color.Red
                                    LockSTS = True
                                    For j = 0 To .ColumnCount - 1
                                        .Rows(n).Cells(j).Style.BackColor = Color.Gainsboro
                                        .Rows(n).Cells(j).Style.ForeColor = Color.Red
                                        .Rows(n).ReadOnly = True
                                    Next
                                End If

                            ElseIf Val(dt3.Rows(i).Item("Type3_Meters").ToString) <> 0 Then
                                .Rows(n).Cells(dgvCol_Details.ClothType).Value = Common_Procedures.ClothType.Type3
                                .Rows(n).Cells(dgvCol_Details.Meters).Value = Format(Val(dt3.Rows(i).Item("Type3_Meters").ToString), "########0.00")
                                .Rows(n).Cells(dgvCol_Details.packslipcode).Value = dt3.Rows(i).Item("PackingSlip_Code_Type3").ToString
                                If Trim(.Rows(n).Cells(dgvCol_Details.packslipcode).Value) = "" Then .Rows(n).Cells(dgvCol_Details.packslipcode).Value = dt3.Rows(i).Item("BuyerOffer_Code_Type3").ToString
                                If Trim(.Rows(n).Cells(dgvCol_Details.packslipcode).Value) <> "" Then
                                    .Rows(n).Cells(dgvCol_Details.Sts).Value = "1"
                                    .Rows(n).Cells(dgvCol_Details.ClothType).Style.ForeColor = Color.Red
                                    LockSTS = True
                                    For j = 0 To .ColumnCount - 1
                                        .Rows(n).Cells(j).Style.BackColor = Color.Gainsboro
                                        .Rows(n).Cells(j).Style.ForeColor = Color.Red
                                        .Rows(n).ReadOnly = True
                                    Next
                                End If

                            ElseIf Val(dt3.Rows(i).Item("Type4_Meters").ToString) <> 0 Then
                                .Rows(n).Cells(dgvCol_Details.ClothType).Value = Common_Procedures.ClothType.Type4
                                .Rows(n).Cells(dgvCol_Details.Meters).Value = Format(Val(dt3.Rows(i).Item("Type4_Meters").ToString), "########0.00")
                                .Rows(n).Cells(dgvCol_Details.packslipcode).Value = dt3.Rows(i).Item("PackingSlip_Code_Type4").ToString
                                If Trim(.Rows(n).Cells(dgvCol_Details.packslipcode).Value) = "" Then .Rows(n).Cells(dgvCol_Details.packslipcode).Value = dt3.Rows(i).Item("BuyerOffer_Code_Type4").ToString
                                If Trim(.Rows(n).Cells(dgvCol_Details.packslipcode).Value) <> "" Then
                                    .Rows(n).Cells(dgvCol_Details.Sts).Value = "1"
                                    .Rows(n).Cells(dgvCol_Details.ClothType).Style.ForeColor = Color.Red
                                    LockSTS = True
                                    For j = 0 To .ColumnCount - 1
                                        .Rows(n).Cells(j).Style.BackColor = Color.Gainsboro
                                        .Rows(n).Cells(j).Style.ForeColor = Color.Red
                                        .Rows(n).ReadOnly = True
                                    Next
                                End If

                            ElseIf Val(dt3.Rows(i).Item("Type5_Meters").ToString) <> 0 Then
                                .Rows(n).Cells(dgvCol_Details.ClothType).Value = Common_Procedures.ClothType.Type5
                                .Rows(n).Cells(dgvCol_Details.Meters).Value = Format(Val(dt3.Rows(i).Item("Type5_Meters").ToString), "########0.00")
                                .Rows(n).Cells(dgvCol_Details.packslipcode).Value = dt3.Rows(i).Item("PackingSlip_Code_Type5").ToString
                                If Trim(.Rows(n).Cells(dgvCol_Details.packslipcode).Value) = "" Then .Rows(n).Cells(dgvCol_Details.packslipcode).Value = dt3.Rows(i).Item("BuyerOffer_Code_Type5").ToString
                                If Trim(.Rows(n).Cells(dgvCol_Details.packslipcode).Value) <> "" Then
                                    .Rows(n).Cells(dgvCol_Details.Sts).Value = "1"
                                    .Rows(n).Cells(dgvCol_Details.ClothType).Style.ForeColor = Color.Red
                                    LockSTS = True
                                    For j = 0 To .ColumnCount - 1
                                        .Rows(n).Cells(j).Style.BackColor = Color.Gainsboro
                                        .Rows(n).Cells(j).Style.ForeColor = Color.Red
                                        .Rows(n).ReadOnly = True
                                    Next
                                End If

                            End If

                            .Rows(n).Cells(dgvCol_Details.Weight).Value = Val(dt3.Rows(i).Item("Weight").ToString)
                            .Rows(n).Cells(dgvCol_Details.Wgt_Mtr).Value = Format(Val(dt3.Rows(i).Item("Weight_Meter").ToString), "########0.000")

                        Next i

                    End If
                    dt3.Clear()
                    n = .Rows.Count - 1
                    If (Trim(.Rows(n).Cells(dgvCol_Details.ClothType).Value) = "" And Val(.Rows(n).Cells(dgvCol_Details.Meters).Value) <> 0) Or (.Rows(n).Cells(dgvCol_Details.ClothType).Value = Nothing And .Rows(n).Cells(dgvCol_Details.Meters).Value = Nothing) Then
                        .Rows(n).Cells(dgvCol_Details.Pcsno).Value = ""
                    End If

                End With

                With dgv_Details_Total
                    If .RowCount = 0 Then .Rows.Add()
                    .Rows(0).Cells(dgvCol_Details.Meters).Value = Format(Val(dt1.Rows(0).Item("Total_Checking_Meters").ToString), "########0.00")
                    .Rows(0).Cells(dgvCol_Details.Weight).Value = Format(Val(dt1.Rows(0).Item("Total_Weight").ToString), "########0.000")
                End With


                dgv_Weft_Consumption_Details.Rows.Clear()
                da1 = New SqlClient.SqlDataAdapter("Select a.*, b.count_name from Weaver_ClothReceipt_Consumed_Yarn_Details a INNER JOIN Count_Head b ON a.Count_IdNo = b.Count_IdNo Where a.Weaver_ClothReceipt_Code = '" & Trim(NewCode) & "'", con)
                dt4 = New DataTable
                da1.Fill(dt4)
                If dt4.Rows.Count > 0 Then
                    For i = 0 To dt4.Rows.Count - 1

                        n = dgv_Weft_Consumption_Details.Rows.Add()
                        dgv_Weft_Consumption_Details.Rows(n).Cells(0).Value = dt4.Rows(i).Item("count_name").ToString
                        dgv_Weft_Consumption_Details.Rows(n).Cells(1).Value = dt4.Rows(i).Item("Gram_Perc_Type").ToString
                        dgv_Weft_Consumption_Details.Rows(n).Cells(2).Value = Val(dt4.Rows(i).Item("Consumption_Gram_Perc").ToString)
                        dgv_Weft_Consumption_Details.Rows(n).Cells(3).Value = Val(dt4.Rows(i).Item("Consumed_Yarn_Weight").ToString)

                    Next

                End If
                dt4.Clear()


                If LockSTS = True Then

                    cbo_LoomNo.Enabled = False
                    cbo_LoomNo.BackColor = Color.LightGray

                    cbo_ClothName.Enabled = False
                    cbo_ClothName.BackColor = Color.LightGray

                    txt_DoffMtrs.Enabled = False
                    txt_DoffMtrs.BackColor = Color.LightGray

                    txt_Folding.Enabled = False
                    txt_Folding.BackColor = Color.LightGray

                    'txt_CrimpPerc.Enabled = False
                    'txt_CrimpPerc.BackColor = Color.LightGray

                End If



            Else

                new_record()

            End If

            dt1.Dispose()
            da1.Dispose()

            Grid_DeSelect()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

        If msk_Date.Visible And msk_Date.Enabled Then msk_Date.Focus()

    End Sub

    Private Sub Doffing_Entry_Format1_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated

        Try

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(lbl_PartyName.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "LEDGER" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                lbl_PartyName.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If
            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_ClothName.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "CLOTH" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_ClothName.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If
            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_LoomNo.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "LOOMNO" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_LoomNo.Text = Trim(Common_Procedures.Master_Return.Return_Value)
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

    Private Sub Doffing_Entry_Format1_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim Dt2 As New DataTable
        Dim dt3 As New DataTable
        Dim dt4 As New DataTable
        Dim dt5 As New DataTable
        Dim dt6 As New DataTable

        FrmLdSTS = True

        Me.Text = ""

        con = New SqlClient.SqlConnection(Common_Procedures.Connection_String)
        con.Open()

        lbl_Weaver_Job_No.Visible = False
        lbl_Weaver_Job_No_Caption.Visible = False

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1428" Then '---- SAKTHI VINAYAGA TEXTILES  (ERODE-PALLIPALAYAM)
            lbl_RollNo_Caption.Text = "Ref No."
        Else
            lbl_RollNo_Caption.Text = StrConv(Common_Procedures.settings.ClothReceipt_LotNo_OR_RollNo_Text, vbProperCase)
        End If

        txt_Roll_SuffixNo.Visible = False
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1520" Then '----- SRI RAINBOW COTTON FABRIC (KARUR)
            txt_Roll_SuffixNo.Visible = True
            txt_Roll_SuffixNo.Left = lbl_RollNo.Left + lbl_RollNo.Width - txt_Roll_SuffixNo.Width + 2

            lbl_RollNo.Left = lbl_RollNo.Left - 25
            lbl_RollNo.Width = lbl_RollNo.Width - txt_Roll_SuffixNo.Width + 25
            lbl_NewSTS.Left = lbl_RollNo.Left + lbl_RollNo.Width - lbl_NewSTS.Width - 3
        End If

        If Trim(Common_Procedures.settings.CustomerCode) = "1234" Then '---ARULJOTHI EXPORTS PVT LTD (SOMANUR)
            lbl_PartyName.Width = 327
            txt_BarCode.Visible = True
            lbl_BarCode.Visible = True
        End If

        If Common_Procedures.settings.ClothReceipt_PieceNo_Concept = "CONTINUOUS NO" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1037" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1343" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1410" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1155" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1428" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1558" Then
            dgv_Details.Columns(0).ReadOnly = False
            dgv_Details.Columns(0).DefaultCellStyle.Alignment = 0

        Else
            dgv_Details.Columns(0).ReadOnly = True
            dgv_Details.Columns(0).DefaultCellStyle.Alignment = 2

        End If

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1371" Then '---- RAAY SAN TEXTILES (ANNUR)

            lbl_DoffMtrs_Caption.Visible = False
            txt_DoffMtrs.Visible = False

            lbl_ExcSht_Caption.Visible = False
            lbl_ExcSht.Visible = False

            lbl_Folding_Caption.Left = 15

            txt_Folding.Left = 111
            txt_Folding.Width = 117

            lbl_CrimpPerc_Caption.Left = 236

            txt_CrimpPerc.Left = 298
            txt_CrimpPerc.Width = 124

            lbl_ConsPavu_Caption.Left = 435

            lbl_ConsPavu.Left = 519
            lbl_ConsPavu.Width = 164

            lbl_ConsWeftYarn_Caption.Left = 691

            lbl_ConsWeftYarn.Left = 781
            lbl_ConsWeftYarn.Width = 155

        End If

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1438" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1474" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1487" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1489" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1558" Then '-----SATHY TEXTILES (SATHYAMANGALAM)
            dgv_Details.Columns(0).HeaderText = "ROLL NO"
        End If

        If Common_Procedures.settings.Cloth_WarpConsumption_Multiple_EndsCount_Status = 1 Then '---- SOTEXPA QUALIDIS TEXTILE (SULUR)
            lbl_EndsCount_Caption.Visible = False
            lbl_EndsCount.Visible = False
            lbl_weft_Count_Caption.Left = lbl_EndsCount_Caption.Left
            lbl_WeftCount.Left = lbl_EndsCount.Left
            lbl_WeftCount.Width = cbo_ClothName.Width

        Else

            lbl_EndsCount_Beam1_Caption.Visible = False
            lbl_EndsCount_Beam1.Visible = False
            lbl_EndsCount_Beam2_Caption.Visible = False
            lbl_EndsCount_Beam2.Visible = False

            lbl_ConsPavu_Beam1_Caption.Visible = False
            lbl_ConsPavu_Beam1.Visible = False
            lbl_ConsPavu_Beam2_Caption.Visible = False
            lbl_ConsPavu_Beam2.Visible = False

            lbl_BeamNo1_Caption.AutoSize = True
            lbl_BeamNo1_Caption.Left = lbl_WidthType_Caption.Left
            lbl_BeamNo1_Caption.Top = lbl_BeamNo1.Top + 4
            lbl_BeamNo1_Caption.Font = lbl_WidthType_Caption.Font
            lbl_BeamNo1.Size = lbl_WidthType.Size
            lbl_BeamNo1.Left = lbl_WidthType.Left

            lbl_BeamNo2_Caption.AutoSize = True
            lbl_BeamNo2_Caption.Left = lbl_WidthType_Caption.Left
            lbl_BeamNo2_Caption.Top = lbl_BeamNo2.Top + 4
            lbl_BeamNo2_Caption.Font = lbl_WidthType_Caption.Font
            lbl_BeamNo2.Size = lbl_WidthType.Size
            lbl_BeamNo2.Left = lbl_WidthType.Left

            lbl_SetNo1_Caption.AutoSize = True
            lbl_SetNo1_Caption.Left = lbl_poNo_Caption.Left
            lbl_SetNo1_Caption.Top = lbl_SetNo1.Top + 4
            lbl_SetNo1_Caption.Font = lbl_poNo_Caption.Font
            lbl_SetNo1.Size = lbl_poNo.Size
            lbl_SetNo1.Left = lbl_poNo.Left

            lbl_SetNo2_Caption.AutoSize = True
            lbl_SetNo2_Caption.Left = lbl_poNo_Caption.Left
            lbl_SetNo2_Caption.Top = lbl_SetNo2.Top + 4
            lbl_SetNo2_Caption.Font = lbl_poNo_Caption.Font
            lbl_SetNo2.Size = lbl_poNo.Size
            lbl_SetNo2.Left = lbl_poNo.Left

            lbl_TotMtrs1_Caption.AutoSize = True
            lbl_TotMtrs1_Caption.Left = lbl_EndsCount_Caption.Left
            lbl_TotMtrs1_Caption.Top = lbl_TotMtrs1.Top + 4
            lbl_TotMtrs1_Caption.Font = lbl_EndsCount_Caption.Font
            lbl_TotMtrs1.Size = lbl_EndsCount.Size
            lbl_TotMtrs1.Left = lbl_EndsCount.Left

            lbl_TotMtrs2_Caption.AutoSize = True
            lbl_TotMtrs2_Caption.Left = lbl_EndsCount_Caption.Left
            lbl_TotMtrs2_Caption.Top = lbl_TotMtrs2.Top + 4
            lbl_TotMtrs2_Caption.Font = lbl_EndsCount_Caption.Font
            lbl_TotMtrs2.Size = lbl_EndsCount.Size
            lbl_TotMtrs2.Left = lbl_EndsCount.Left

            lbl_BalMtrs1_Caption.AutoSize = True
            lbl_BalMtrs1_Caption.Left = lbl_weft_Count_Caption.Left
            lbl_BalMtrs1_Caption.Top = lbl_BalMtrs1.Top + 4
            lbl_BalMtrs1_Caption.Font = lbl_weft_Count_Caption.Font
            lbl_BalMtrs1.Size = lbl_WeftCount.Size
            lbl_BalMtrs1.Left = lbl_WeftCount.Left

            lbl_BalMtrs2_Caption.AutoSize = True
            lbl_BalMtrs2_Caption.Left = lbl_weft_Count_Caption.Left
            lbl_BalMtrs2_Caption.Top = lbl_BalMtrs2.Top + 4
            lbl_BalMtrs2_Caption.Font = lbl_weft_Count_Caption.Font
            lbl_BalMtrs2.Size = lbl_WeftCount.Size
            lbl_BalMtrs2.Left = lbl_WeftCount.Left

        End If


        btn_SaveAll.Visible = False
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "--1558--" Then '---- JR TEX ( STANLEY ) ( MS FABRICS ) (SULUR)   (or)   J.R TEX ( STANLEY ) ( M.S FABRICS ) (SULUR)
            btn_SaveAll.Visible = True
        End If

        pnl_filter.Visible = False
        pnl_filter.Left = (Me.Width - pnl_filter.Width) \ 2
        pnl_filter.Top = ((Me.Height - pnl_filter.Height) \ 2) + 20

        pnl_Print.Visible = False
        pnl_Print.Left = (Me.Width - pnl_Print.Width) \ 2
        pnl_Print.Top = (Me.Height - pnl_Print.Height) \ 2
        pnl_Print.BringToFront()

        pnl_Weft_Consumption_Details.Visible = False
        pnl_Weft_Consumption_Details.Left = (Me.Width - pnl_Weft_Consumption_Details.Width) \ 2
        pnl_Weft_Consumption_Details.Top = (Me.Height - pnl_Weft_Consumption_Details.Height) \ 2

        btn_UserModification.Visible = False
        If Common_Procedures.settings.User_Modifications_Show_Status = 1 Then
            If Val(Common_Procedures.User.IdNo) = 1 Or Common_Procedures.User.Show_UserModification_Status = 1 Then
                btn_UserModification.Visible = True
            End If
        End If

        If Common_Procedures.settings.CustomerCode = "1186" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1544" Then    '----UNITED WEAVES(PALLADAM)
            lbl_poNo_Caption.Visible = True
            lbl_poNo.Visible = True
            dgv_Details.Columns(dgvCol_Details.Lot_No).Visible = True
            dgv_Details.Columns(dgvCol_Details.packslipcode).Width = 130

        Else

            lbl_WidthType.Size = New Size(313, 23)
            lbl_poNo_Caption.Visible = False
            lbl_poNo.Visible = False

        End If

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1428" Then '---- SAKTHI VINAYAGA TEXTILES  (ERODE-PALLIPALAYAM)
            dgv_Details.Columns(dgvCol_Details.Pcsno).HeaderText = "ROLL NO"
        End If

        If Common_Procedures.settings.Show_Weaver_JobCard_Entry_STATUS = 1 Then

            lbl_Weaver_Job_No.Visible = True
            lbl_Weaver_Job_No.BackColor = Color.PaleGoldenrod
            lbl_Weaver_Job_No_Caption.Visible = True

            If lbl_Weaver_Job_No.Visible And lbl_Weaver_Job_No_Caption.Visible Then


                lbl_EndsCount.Width = lbl_ExcSht.Width

                lbl_weft_Count_Caption.Left = lbl_ConsPavu_Caption.Left
                lbl_weft_Count_Caption.Width = lbl_ConsPavu_Caption.Width
                lbl_WeftCount.Left = lbl_ConsPavu.Left
                lbl_WeftCount.Width = lbl_ConsPavu.Width

                lbl_Weaver_Job_No_Caption.Left = lbl_ConsWeftYarn_Caption.Left

            End If

        End If

        If Common_Procedures.settings.Show_Sales_OrderNumber_in_ALLEntry_Status = 1 Then

            lbl_ClothSales_OrderCode_forSelection.Visible = True
            lbl_ClothSales_OrderCode_forSelection_Caption.Visible = True

            FnYearCode1 = ""
            FnYearCode2 = ""
            Common_Procedures.get_FnYearCode_of_Last_2_Years(FnYearCode1, FnYearCode2)

        Else

            lbl_ClothSales_OrderCode_forSelection.Visible = False
            lbl_ClothSales_OrderCode_forSelection_Caption.Visible = False

        End If


        cbo_Pcs_LastPiece_Status.Items.Clear()
        cbo_Pcs_LastPiece_Status.Items.Add("")
        cbo_Pcs_LastPiece_Status.Items.Add("YES")
        cbo_Pcs_LastPiece_Status.Items.Add("NO")


        lbl_Filter_PieceNo_Caption.Text = dgv_Details.Columns(0).HeaderText
        dgv_filter.Columns(0).HeaderText = lbl_RollNo_Caption.Text

        AddHandler txt_Roll_SuffixNo.GotFocus, AddressOf ControlGotFocus
        AddHandler msk_Date.GotFocus, AddressOf ControlGotFocus
        AddHandler dtp_Date.GotFocus, AddressOf ControlGotFocus
        AddHandler lbl_PartyName.GotFocus, AddressOf ControlGotFocus
        AddHandler lbl_WeftCount.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_ClothName.GotFocus, AddressOf ControlGotFocus

        AddHandler cbo_LoomNo.GotFocus, AddressOf ControlGotFocus
        AddHandler lbl_KnotNo.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Filter_ClothName.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Filter_PartyName.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Filter_LoomNo.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_DoffMtrs.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_CrimpPerc.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_BarCode.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_PrintFrom.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_PrintTo.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Folding.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Grid_ClothType.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Pcs_LastPiece_Status.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_save.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_close.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_ShowKnottingDetails.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_CloseKnottingDetails.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Filter_PieceNo.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Filter_BeamNo.GotFocus, AddressOf ControlGotFocus

        AddHandler txt_Roll_SuffixNo.LostFocus, AddressOf ControlLostFocus
        AddHandler msk_Date.LostFocus, AddressOf ControlLostFocus
        AddHandler dtp_Date.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_ClothName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_LoomNo.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_DoffMtrs.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Filter_ClothName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Filter_PartyName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Filter_LoomNo.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_CrimpPerc.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_BarCode.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_PrintFrom.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_PrintTo.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_save.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_close.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_ShowKnottingDetails.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_CloseKnottingDetails.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Folding.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Grid_ClothType.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Pcs_LastPiece_Status.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Filter_PieceNo.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Filter_BeamNo.LostFocus, AddressOf ControlLostFocus


        AddHandler msk_Date.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler dtp_Date.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler dtp_FilterFrom_date.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler dtp_FilterTo_date.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_DoffMtrs.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_BarCode.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_PrintFrom.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Filter_PieceNo.KeyDown, AddressOf TextBoxControlKeyDown

        AddHandler msk_Date.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler dtp_Date.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler dtp_FilterFrom_date.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler dtp_FilterTo_date.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_BarCode.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_PrintFrom.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Filter_PieceNo.KeyPress, AddressOf TextBoxControlKeyPress


        pnl_Selection.Visible = False
        pnl_Selection.Left = (Me.Width - pnl_Selection.Width) \ 2
        pnl_Selection.Top = (Me.Height - pnl_Selection.Height) \ 2

        lbl_Company.Text = ""
        lbl_Company.Tag = 0
        lbl_Company.Visible = False
        Common_Procedures.CompIdNo = 0

        dgv_filter.RowTemplate.Height = 27

        Filter_Status = False
        FrmLdSTS = True
        new_record()

    End Sub


    Private Sub Doffing_Entry_Format1_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        On Error Resume Next
        con.Close()
        con.Dispose()
        Common_Procedures.Last_Closed_FormName = Me.Name
    End Sub

    Private Sub Doffing_Entry_Format1_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress

        Try

            If Asc(e.KeyChar) = 27 Then

                If pnl_filter.Visible = True Then
                    btn_closefilter_Click(sender, e)
                    Exit Sub

                ElseIf pnl_Selection.Visible = True Then
                    btn_Close_Selection_Click(sender, e)
                    Exit Sub

                ElseIf pnl_Print.Visible = True Then
                    btn_Print_Cancel_Click(sender, e)
                    Exit Sub

                ElseIf pnl_Weft_Consumption_Details.Visible Then
                    Call btn_Close_Weft_Consumption_Details_Click(sender, e)
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

        If ActiveControl.Name = dgv_Details.Name Or TypeOf ActiveControl Is DataGridViewTextBoxEditingControl Then

            On Error Resume Next

            dgv1 = Nothing

            If ActiveControl.Name = dgv_Details.Name Then
                dgv1 = dgv_Details

            ElseIf dgv_Details.IsCurrentRowDirty = True Then
                dgv1 = dgv_Details

            Else
                dgv1 = dgv_Details

            End If

            If IsNothing(dgv1) = False Then

                With dgv1

                    If dgv1.Name = dgv_Details.Name Then

                        If keyData = Keys.Enter Or keyData = Keys.Down Then

                            If .CurrentCell.ColumnIndex >= 3 Then

                                If .CurrentCell.RowIndex = .RowCount - 1 Then

                                    If MessageBox.Show("Do you want to save ?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = DialogResult.Yes Then
                                        save_record()
                                    Else
                                        msk_Date.Focus()
                                    End If

                                Else

                                    If dgv_Details.Columns(0).ReadOnly = False Then
                                        .CurrentCell = .Rows(.CurrentCell.RowIndex + 1).Cells(0)
                                    Else
                                        .CurrentCell = .Rows(.CurrentCell.RowIndex + 1).Cells(1)
                                    End If

                                End If

                            Else

                                If .CurrentCell.RowIndex = .RowCount - 1 And .CurrentCell.ColumnIndex >= 0 And ((Trim(.CurrentRow.Cells(0).Value) = "" Or Trim(.CurrentRow.Cells(0).Value) = "0") And Val(.CurrentRow.Cells(2).Value) = 0) Then
                                    If MessageBox.Show("Do you want to save ?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = DialogResult.Yes Then
                                        save_record()
                                    Else
                                        msk_Date.Focus()
                                    End If

                                ElseIf .CurrentCell.RowIndex = .RowCount - 2 And .CurrentCell.ColumnIndex >= 0 And ((Trim(.CurrentRow.Cells(0).Value) = "" Or Trim(.CurrentRow.Cells(0).Value) = "0") And Val(.CurrentRow.Cells(2).Value) = 0 And (Trim(.Rows(.RowCount - 1).Cells(0).Value) = "" Or Trim(.Rows(.RowCount - 1).Cells(0).Value) = "0")) And Val(.Rows(.RowCount - 1).Cells(2).Value) = 0 Then
                                    If MessageBox.Show("Do you want to save ?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = DialogResult.Yes Then
                                        save_record()
                                    Else
                                        msk_Date.Focus()
                                    End If

                                Else
                                    .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 1)

                                End If


                            End If

                            Return True

                        ElseIf keyData = Keys.Up Then

                            If .CurrentCell.ColumnIndex <= 1 Then
                                If .CurrentCell.RowIndex = 0 Then
                                    If txt_DoffMtrs.Enabled And txt_DoffMtrs.Visible Then
                                        txt_DoffMtrs.Focus()
                                    ElseIf cbo_ClothName.Enabled And cbo_ClothName.Visible Then
                                        cbo_ClothName.Focus()
                                    ElseIf cbo_LoomNo.Enabled And cbo_LoomNo.Visible Then
                                        cbo_LoomNo.Focus()
                                    Else
                                        msk_Date.Focus()
                                    End If

                                Else
                                    .CurrentCell = .Rows(.CurrentCell.RowIndex - 1).Cells(3)

                                End If

                            Else
                                .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex - 1)

                            End If

                            Return True

                        Else
                            Return MyBase.ProcessCmdKey(msg, keyData)

                        End If



                    Else
                        Return MyBase.ProcessCmdKey(msg, keyData)

                    End If

                    'If keyData = Keys.Enter Or keyData = Keys.Down Or keyData = Keys.Up Then

                    '    Dim vPREVCtrlName As Object

                    '    vPREVCtrlName = IIf(txt_DoffMtrs.Enabled And txt_DoffMtrs.Visible, txt_DoffMtrs, IIf(cbo_ClothName.Enabled And cbo_ClothName.Visible, cbo_ClothName, IIf(cbo_LoomNo.Enabled And cbo_LoomNo.Visible, cbo_LoomNo, msk_Date)))

                    '    Common_Procedures.DGVGrid_Cursor_Focus(dgv1, keyData, vPREVCtrlName, Nothing, dgvDet_CboBx_ColNos_Arr, dgtxt_Details, msk_Date, 0)

                    '    Return True

                    'Else
                    '    Return MyBase.ProcessCmdKey(msg, keyData)

                    'End If

                End With

            Else

                Return MyBase.ProcessCmdKey(msg, keyData)

            End If

        Else

            Return MyBase.ProcessCmdKey(msg, keyData)

        End If

    End Function

    Public Sub delete_record() Implements Interface_MDIActions.delete_record
        Dim cmd As New SqlClient.SqlCommand
        Dim tr As SqlClient.SqlTransaction
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim Dt2 As New DataTable
        Dim NewCode As String = ""
        Dim DelvSts As Integer = 0
        Dim BmRunOutCd As String = ""
        Dim vOrdByNo As String = ""


        con = New SqlClient.SqlConnection(Common_Procedures.Connection_String)
        con.Open()


        vOrdByNo = Common_Procedures.OrderBy_CodeToValue(lbl_RollNo.Text)

        If MessageBox.Show("Do you want to Delete?", "FOR DELETION...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) <> Windows.Forms.DialogResult.Yes Then
            Exit Sub
        End If

        NewCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RollNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.DeletingEntry, Common_Procedures.UR.Inhouse_Doffing_and_PieceChecking_Entry, New_Entry, Me, con, "Weaver_Cloth_Receipt_Head", "Weaver_Cloth_Receipt_Code", NewCode, "Weaver_Cloth_Receipt_Date", "(Weaver_Cloth_Receipt_Code = '" & Trim(NewCode) & "')") = False Then Exit Sub

        If New_Entry = True Then
            MessageBox.Show("This is New Entry", "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
        End If

        NewCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RollNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        '-------------------------

        Da = New SqlClient.SqlDataAdapter("select COUNT(*) from Weaver_ClothReceipt_Piece_Details Where Weaver_Piece_Checking_Code = '" & Trim(NewCode) & "' and (PackingSlip_Code_Type1 <> '' or PackingSlip_Code_Type2 <> '' or PackingSlip_Code_Type3 <> '' or PackingSlip_Code_Type4 <> '' or PackingSlip_Code_Type5 <> '' or BuyerOffer_Code_Type1 <> '' or BuyerOffer_Code_Type2 <> '' or BuyerOffer_Code_Type3 <> '' or BuyerOffer_Code_Type4 <> '' or BuyerOffer_Code_Type5 <> '')", con)
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

        '--------------------

        'Da = New SqlClient.SqlDataAdapter("select * from Weaver_Cloth_Receipt_Head Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Weaver_ClothReceipt_Code = '" & Trim(NewCode) & "'", con)
        'Dt1 = New DataTable
        'Da.Fill(Dt1)

        'If Dt1.Rows.Count > 0 Then

        '    If IsDBNull(Dt1.Rows(0).Item("Weaver_Piece_Checking_Code").ToString) = False Then
        '        If Trim(Dt1.Rows(0).Item("Weaver_Piece_Checking_Code").ToString) <> "" Then
        '            MessageBox.Show("Already Piece Checking Prepared", "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
        '            Exit Sub
        '        End If
        '    End If

        'End If
        'Dt1.Clear()

        tr = con.BeginTransaction
        'Call Common_Procedures.User_Modification_Updation(con, "HEAD", Me.Name, "DELETE", "Weaver_Cloth_Receipt_head", "Weaver_ClothReceipt_Code", Val(lbl_Company.Tag), NewCode, lbl_RollNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, True, "", "", "Weaver_ClothReceipt_Code, Company_IdNo, for_OrderBy", tr)

        Try

            cmd.Connection = con
            cmd.Transaction = tr

            cmd.CommandText = "delete from Weaver_ClothReceipt_Consumed_Yarn_Details where Weaver_ClothReceipt_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Update Beam_Knotting_Head set Production_Meters = a.Production_Meters - (CASE WHEN b.Total_Checking_Meters <> 0 THEN b.Total_Checking_Meters ELSE b.ReceiptMeters_Receipt END) from Beam_Knotting_Head a, Weaver_Cloth_Receipt_Head b where b.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and b.Weaver_ClothReceipt_Code = '" & Trim(NewCode) & "' and a.Beam_Knotting_Code = b.Beam_Knotting_Code"
            cmd.ExecuteNonQuery()

            If lbl_ConsPavu_Beam1.Visible = True And lbl_ConsPavu_Beam2.Visible = True Then

                cmd.CommandText = "Update Stock_SizedPavu_Processing_Details set Production_Meters = a.Production_Meters - b.ConsumedPavu_Checking_Beam1 from Stock_SizedPavu_Processing_Details a, Weaver_Cloth_Receipt_Head b where b.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and b.Weaver_ClothReceipt_Code = '" & Trim(NewCode) & "' and b.Set_code1 <> '' AND b.Beam_No1 <> '' and a.set_code = b.Set_code1 and a.Beam_No = b.Beam_No1"
                cmd.ExecuteNonQuery()

                cmd.CommandText = "Update Stock_SizedPavu_Processing_Details set Production_Meters = a.Production_Meters - b.ConsumedPavu_Checking_Beam2 from Stock_SizedPavu_Processing_Details a, Weaver_Cloth_Receipt_Head b where b.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and b.Weaver_ClothReceipt_Code = '" & Trim(NewCode) & "' AND b.Set_code2 <> '' AND b.Beam_No2 <> '' and a.set_code = b.Set_code2 and a.Beam_No = b.Beam_No2"
                cmd.ExecuteNonQuery()

            Else

                cmd.CommandText = "Update Stock_SizedPavu_Processing_Details set Production_Meters = a.Production_Meters - b.BeamConsumption_Receipt from Stock_SizedPavu_Processing_Details a, Weaver_Cloth_Receipt_Head b where b.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and b.Weaver_ClothReceipt_Code = '" & Trim(NewCode) & "' and b.Set_code1 <> '' AND b.Beam_No1 <> '' and a.set_code = b.Set_code1 and a.Beam_No = b.Beam_No1"
                cmd.ExecuteNonQuery()

                cmd.CommandText = "Update Stock_SizedPavu_Processing_Details set Production_Meters = a.Production_Meters - b.BeamConsumption_Receipt from Stock_SizedPavu_Processing_Details a, Weaver_Cloth_Receipt_Head b where b.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and b.Weaver_ClothReceipt_Code = '" & Trim(NewCode) & "' AND b.Set_code2 <> '' AND b.Beam_No2 <> '' and a.set_code = b.Set_code2 and a.Beam_No = b.Beam_No2"
                cmd.ExecuteNonQuery()

            End If


            cmd.CommandText = "Delete from Stock_Yarn_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Stock_Pavu_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Stock_Cloth_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "delete from Weaver_ClothReceipt_Piece_Details where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Weaver_ClothReceipt_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Weaver_Piece_Checking_Head where Weaver_Piece_Checking_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "delete from Weaver_Cloth_Receipt_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Weaver_ClothReceipt_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            tr.Commit()

            new_record()

            MessageBox.Show("Deleted Sucessfully!!!", "FOR DELETION...", MessageBoxButtons.OKCancel, MessageBoxIcon.Information)

        Catch ex As Exception

            tr.Rollback()
            MessageBox.Show(ex.Message, "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

        If msk_Date.Visible And msk_Date.Enabled Then msk_Date.Focus()

    End Sub

    Public Sub filter_record() Implements Interface_MDIActions.filter_record
        Dim Cmd As New SqlClient.SqlCommand

        If Filter_Status = False Then

            con = New SqlClient.SqlConnection(Common_Procedures.Connection_String)
            con.Open()

            Common_Procedures.Update_SizedPavu_BeamNo_for_Selection(con)

            'Cmd.Connection = con
            'Cmd.CommandText = "Update Stock_SizedPavu_Processing_Details set BeamNo_SetCode_forSelection = Beam_No + ' | ' + setcode_forSelection Where Beam_No <> ''"
            'Cmd.ExecuteNonQuery()

            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1428" Then '---- SAKTHI VINAYAGA TEXTILES  (ERODE-PALLIPALAYAM)
                dtp_FilterFrom_date.Text = Now.Date
                dtp_FilterTo_date.Text = Now.Date
            Else
                dtp_FilterFrom_date.Text = Common_Procedures.Company_FromDate
                dtp_FilterTo_date.Text = Common_Procedures.Company_ToDate
            End If

            pnl_filter.Text = ""
            cbo_Filter_PartyName.SelectedIndex = -1
            cbo_Filter_ClothName.SelectedIndex = -1
            cbo_Filter_LoomNo.SelectedIndex = -1
            cbo_Filter_BeamNo.SelectedIndex = -1
            txt_Filter_PieceNo.Text = ""
            dgv_filter.Rows.Clear()

            Cmd.Dispose()

        End If

        pnl_filter.Visible = True
        pnl_filter.Enabled = True
        pnl_filter.BringToFront()
        pnl_back.Enabled = False
        If dtp_FilterFrom_date.Enabled And dtp_FilterFrom_date.Visible Then dtp_FilterFrom_date.Focus()

    End Sub

    Public Sub insert_record() Implements Interface_MDIActions.insert_record
        Dim cmd As New SqlClient.SqlCommand
        Dim dr As SqlClient.SqlDataReader
        Dim movno As String, inpno As String
        Dim NewCode As String

        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.InsertingEntry, Common_Procedures.UR.Inhouse_Doffing_and_PieceChecking_Entry, New_Entry, Me) = False Then Exit Sub

        Try

            inpno = InputBox("Enter New Roll.No.", "FOR INSERTION...")

            NewCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

            cmd.Connection = con
            cmd.CommandText = "select Weaver_ClothReceipt_RefNo from Weaver_Cloth_Receipt_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Weaver_ClothReceipt_Code = '" & Trim(NewCode) & "'"
            dr = cmd.ExecuteReader

            movno = ""
            If dr.HasRows Then
                If dr.Read Then
                    If IsDBNull(dr(0).ToString) = False Then
                        movno = dr(0).ToString
                    End If
                End If
            End If

            dr.Close()
            cmd.Dispose()

            If Val(movno) <> 0 Then
                move_record(movno)

            Else
                If Val(inpno) = 0 Then
                    MessageBox.Show("Invalid Roll.No", "DOES NOT INSERT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

                Else
                    new_record()
                    Insert_Entry = True
                    lbl_RollNo.Text = Trim(UCase(inpno))

                End If

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT FIND...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Public Sub movefirst_record() Implements Interface_MDIActions.movefirst_record
        Dim cmd As New SqlClient.SqlCommand
        Dim dr As SqlClient.SqlDataReader
        Dim movno As String

        Try

            cmd.Connection = con
            cmd.CommandText = "select top 1 Weaver_ClothReceipt_RefNo from Weaver_Cloth_Receipt_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Weaver_ClothReceipt_Code like '" & Trim(Pk_Condition) & "%/" & Trim(Common_Procedures.FnYearCode) & "' and Receipt_Type = 'L' Order by for_Orderby, Weaver_ClothReceipt_RefNo"
            dr = cmd.ExecuteReader

            movno = ""
            If dr.HasRows Then
                If dr.Read Then
                    If IsDBNull(dr(0).ToString) = False Then
                        movno = dr(0).ToString
                    End If
                End If
            End If

            dr.Close()

            If Val(movno) <> 0 Then move_record(movno)

        Catch ex As Exception

            MessageBox.Show(ex.Message, "FOR  MOVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Public Sub movelast_record() Implements Interface_MDIActions.movelast_record
        Dim cmd As New SqlClient.SqlCommand
        Dim dr As SqlClient.SqlDataReader
        Dim movno As String

        Try
            cmd.Connection = con
            cmd.CommandText = "select top 1 Weaver_ClothReceipt_RefNo from Weaver_Cloth_Receipt_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Weaver_ClothReceipt_Code like '" & Trim(Pk_Condition) & "%/" & Trim(Common_Procedures.FnYearCode) & "' and Receipt_Type = 'L' Order by for_Orderby desc, Weaver_ClothReceipt_RefNo desc"
            dr = cmd.ExecuteReader

            movno = ""
            If dr.HasRows Then
                If dr.Read Then
                    If IsDBNull(dr(0).ToString) = False Then
                        movno = dr(0).ToString
                    End If
                End If
            End If

            dr.Close()

            If Val(movno) <> 0 Then move_record(movno)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR  MOVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Public Sub movenext_record() Implements Interface_MDIActions.movenext_record
        Dim cmd As New SqlClient.SqlCommand
        Dim dr As SqlClient.SqlDataReader
        Dim movno As String = ""
        Dim OrdByNo As Single = 0

        Try

            OrdByNo = Common_Procedures.OrderBy_CodeToValue(Trim(lbl_RollNo.Text))

            cmd.Connection = con
            cmd.CommandText = "select top 1 Weaver_ClothReceipt_RefNo from Weaver_Cloth_Receipt_Head where for_orderby > " & Str(OrdByNo) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and Weaver_ClothReceipt_Code like '" & Trim(Pk_Condition) & "%/" & Trim(Common_Procedures.FnYearCode) & "' and Receipt_Type = 'L' Order by for_Orderby, Weaver_ClothReceipt_RefNo"
            dr = cmd.ExecuteReader

            movno = ""
            If dr.HasRows Then
                If dr.Read Then
                    If IsDBNull(dr(0).ToString) = False Then
                        movno = dr(0).ToString
                    End If
                End If
            End If

            dr.Close()

            If Val(movno) <> 0 Then move_record(movno)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR  MOVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try
    End Sub

    Public Sub moveprevious_record() Implements Interface_MDIActions.moveprevious_record
        Dim cmd As New SqlClient.SqlCommand
        Dim dr As SqlClient.SqlDataReader
        Dim movno As String = ""
        Dim OrdByNo As Single = 0

        Try

            OrdByNo = Common_Procedures.OrderBy_CodeToValue(Trim(lbl_RollNo.Text))

            cmd.Connection = con
            cmd.CommandText = "select top 1 Weaver_ClothReceipt_RefNo from Weaver_Cloth_Receipt_Head where for_orderby < " & Str(OrdByNo) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and  Weaver_ClothReceipt_Code like '" & Trim(Pk_Condition) & "%/" & Trim(Common_Procedures.FnYearCode) & "' and Receipt_Type = 'L' Order by for_Orderby desc, Weaver_ClothReceipt_RefNo desc"
            dr = cmd.ExecuteReader

            movno = ""
            If dr.HasRows Then
                If dr.Read Then
                    If IsDBNull(dr(0).ToString) = False Then
                        movno = dr(0).ToString
                    End If
                End If
            End If

            dr.Close()

            If Val(movno) <> 0 Then move_record(movno)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR  MOVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try
    End Sub

    Public Sub new_record() Implements Interface_MDIActions.new_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim dt1 As New DataTable
        Dim NewID As Integer = 0
        Dim Barcode_Generate As String = ""
        Dim CMPNAME As String = ""
        Dim vPREV_DATSTS As Boolean = False

        Try

            con = New SqlClient.SqlConnection(Common_Procedures.Connection_String)
            con.Open()

            clear()

            New_Entry = True
            lbl_NewSTS.Visible = True

            lbl_RollNo.ForeColor = Color.Red

            lbl_RollNo.Text = Common_Procedures.get_MaxCode(con, "Weaver_Cloth_Receipt_Head", "Weaver_ClothReceipt_Code", "for_OrderBy", "(Weaver_ClothReceipt_Code LIKE '" & Trim(Pk_Condition) & "%')", Val(lbl_Company.Tag), Common_Procedures.FnYearCode)

            msk_Date.Text = Date.Today.ToShortDateString
            da = New SqlClient.SqlDataAdapter("select top 1 a.*, tZ.Company_name from Weaver_Cloth_Receipt_Head a INNER JOIN company_head tZ ON a.company_idno = tZ.company_idno where a.company_idno = " & Str(Val(lbl_Company.Tag)) & " and a.Weaver_ClothReceipt_Code LIKE '" & Trim(Pk_Condition) & "%/" & Trim(Common_Procedures.FnYearCode) & "' Order by a.for_Orderby desc, a.Weaver_ClothReceipt_No desc", con)
            dt1 = New DataTable
            da.Fill(dt1)
            If dt1.Rows.Count > 0 Then

                If Val(Common_Procedures.settings.PreviousEntryDate_ByDefault) = 1 Then
                    If dt1.Rows(0).Item("Weaver_ClothReceipt_Date").ToString <> "" Then msk_Date.Text = dt1.Rows(0).Item("Weaver_ClothReceipt_Date").ToString
                End If
                If dt1.Rows(0).Item("Folding").ToString <> "" Then txt_Folding.Text = dt1.Rows(0).Item("Folding").ToString
                If txt_Roll_SuffixNo.Visible Then
                    If dt1.Rows(0).Item("Weaver_ClothReceipt_SuffixNo").ToString <> "" Then txt_Roll_SuffixNo.Text = dt1.Rows(0).Item("Weaver_ClothReceipt_SuffixNo").ToString
                End If
            End If
            dt1.Clear()

            Generate_Barcode()

            If msk_Date.Enabled And msk_Date.Visible Then
                msk_Date.Focus()
                msk_Date.SelectionStart = 0
            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR NEW RECORD...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Public Sub open_record() Implements Interface_MDIActions.open_record

        Dim cmd As New SqlClient.SqlCommand
        Dim dr As SqlClient.SqlDataReader
        Dim movno As String, inpno As String
        Dim NewCode As String

        Try

            con = New SqlClient.SqlConnection(Common_Procedures.Connection_String)
            con.Open()


            inpno = InputBox("Enter Roll.No", "FOR FINDING...")

            NewCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

            cmd.Connection = con
            cmd.CommandText = "select Weaver_ClothReceipt_RefNo from Weaver_Cloth_Receipt_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Weaver_ClothReceipt_Code = '" & Trim(NewCode) & "'"
            dr = cmd.ExecuteReader

            movno = ""
            If dr.HasRows Then
                If dr.Read Then
                    If IsDBNull(dr(0).ToString) = False Then
                        movno = dr(0).ToString
                    End If
                End If
            End If

            dr.Close()
            cmd.Dispose()

            If Val(movno) <> 0 Then
                move_record(movno)

            Else
                MessageBox.Show("Roll.No. Does not exists", "DOES NOT FIND...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT FIND...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Public Sub save_record() Implements Interface_MDIActions.save_record
        Dim cmd As New SqlClient.SqlCommand
        Dim tr As SqlClient.SqlTransaction
        Dim da As SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim dt3 As New DataTable
        Dim dt4 As New DataTable
        Dim dt5 As New DataTable
        Dim NewCode As String = ""
        Dim NewNo As Long = 0
        Dim nr As Long = 0
        Dim led_id As Integer = 0, Delv_ID As Integer = 0, Rec_ID As Integer = 0
        Dim Clo_ID As Integer = 0
        Dim EdsCnt_ID As Integer = 0, vEdsCnt_ID_BM1 As Integer = 0, vEdsCnt_ID_BM2 As Integer = 0
        Dim WftCnt_ID As Integer = 0, vWFTCNTIDno As Integer
        Dim Lm_ID As Integer = 0
        Dim Partcls As String, PBlNo As String, EntID As String
        Dim PcsChkCode As String = 0
        Dim PavuConsMtrs As Single = 0
        Dim NoofInpBmsInLom As Integer
        Dim Old_Loom_Idno As Integer
        Dim Old_SetCd1 As String, Old_Beam1 As String
        Dim Old_SetCd2 As String, Old_Beam2 As String
        Dim OrdByNo As Single = 0
        Dim stkof_idno As Integer = 0
        Dim vGod_ID As Integer = 0
        Dim Led_type As String = 0
        Dim YrnPartcls As String = ""
        Dim MasWftCnt_IDNo As Integer = 0
        Dim vErrMsg As String = ""
        Dim vUC_Mtrs As String = 0
        Dim Sno As Integer = 0
        Dim vBrCode_Typ1 As String = "", vBrCode_Typ2 As String = "", vBrCode_Typ3 As String = "", vBrCode_Typ4 As String = "", vBrCode_Typ5 As String = ""
        Dim CloTyp_ID As Integer = 0
        Dim vTot_Typ1Mtrs As Single
        Dim vTot_Typ2Mtrs As Single
        Dim vTot_Typ3Mtrs As Single
        Dim vTot_Typ5Mtrs As Single
        Dim vTot_Typ4Mtrs As Single
        Dim vTot_ChkMtrs As Single
        Dim vTot_Wgt As Single
        Dim vTot_Wgt1 As String
        Dim vTot_Wgt2 As String
        Dim vTot_Wgt3 As String
        Dim vTot_Wgt4 As String
        Dim vTot_Wgt5 As String
        Dim vYrCd As String = ""
        Dim vProd_Mtrs As String = 0
        Dim vOrdByNo As String = 0
        Dim vTot_100Fld_Typ1Mtrs As Single
        Dim vTot_100Fld_Typ2Mtrs As Single
        Dim vTot_100Fld_Typ3Mtrs As Single
        Dim vTot_100Fld_Typ4Mtrs As Single
        Dim vTot_100Fld_Typ5Mtrs As Single
        Dim vTot_100Fld_ChkMtr As Single
        Dim vPcNo As String = ""
        Dim vPcSubNo As String = ""
        Dim vOrdByPcNo As String = ""
        Dim vSELC_LOTCODE As String = ""
        Dim vPCSCODE_FORSELECTION As String
        Dim vWEAVER_JOBCARD_FORSELECTION As String = ""
        Dim vROLLNO As String = ""
        Dim vSTKPOSTING_STS As Boolean = False


        con = New SqlClient.SqlConnection(Common_Procedures.Connection_String)
        con.Open()

        vOrdByNo = Common_Procedures.OrderBy_CodeToValue(lbl_RollNo.Text)


        If pnl_back.Enabled = False Then
            MessageBox.Show("Close all other windows", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
        End If

        NewCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RollNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.SavingEntry, Common_Procedures.UR.Inhouse_Doffing_and_PieceChecking_Entry, New_Entry, Me, con, "Weaver_Cloth_Receipt_Head", "Weaver_Cloth_Receipt_Code", NewCode, "Weaver_Cloth_Receipt_Date", "(Weaver_Cloth_Receipt_Code = '" & Trim(NewCode) & "')", "(Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Weaver_Cloth_Receipt_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "')", "for_Orderby desc, Weaver_Cloth_Receipt_No desc", dtp_Date.Value.Date) = False Then Exit Sub

        If Val(lbl_Company.Tag) = 0 Then
            MessageBox.Show("Invalid Company Selection", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
        End If

        If IsDate(msk_Date.Text) = False Then
            MessageBox.Show("Invalid Date", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If msk_Date.Enabled Then msk_Date.Focus()
            Exit Sub
        End If

        If Not (Convert.ToDateTime(msk_Date.Text) >= Common_Procedures.Company_FromDate And Convert.ToDateTime(msk_Date.Text) <= Common_Procedures.Company_ToDate) Then
            MessageBox.Show("Date is out of financial range", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If msk_Date.Enabled Then msk_Date.Focus()
            Exit Sub
        End If

        If Val(txt_Folding.Text) = 0 Then txt_Folding.Text = 100

        Lm_ID = Common_Procedures.Loom_NameToIdNo(con, cbo_LoomNo.Text)
        If Lm_ID = 0 Then
            MessageBox.Show("Invalid Loom No", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If cbo_LoomNo.Enabled Then cbo_LoomNo.Focus()
            Exit Sub
        End If

        If Trim(cbo_Pcs_LastPiece_Status.Text) = "" Then
            cbo_Pcs_LastPiece_Status.Text = "NO"
        ElseIf Trim(cbo_Pcs_LastPiece_Status.Text) <> "YES" And Trim(cbo_Pcs_LastPiece_Status.Text) <> "NO" Then
            cbo_Pcs_LastPiece_Status.Text = "NO"
        End If

        lbl_UserName.Text = Common_Procedures.User.IdNo
        led_id = Common_Procedures.Ledger_AlaisNameToIdNo(con, lbl_PartyName.Text)
        If led_id = 0 Then
            MessageBox.Show("Invalid Ledger Name", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If cbo_LoomNo.Enabled Then cbo_LoomNo.Focus()
            Exit Sub
        End If

        Led_type = Common_Procedures.get_FieldValue(con, "Ledger_Head", "Ledger_Type", "(Ledger_IdNo = " & Str(Val(led_id)) & ")")

        stkof_idno = 0
        If Trim(UCase(Led_type)) = "JOBWORKER" Then
            stkof_idno = led_id
        Else
            stkof_idno = Val(Common_Procedures.CommonLedger.OwnSort_Ac)
        End If

        vGod_ID = Common_Procedures.CommonLedger.Godown_Ac

        Clo_ID = Common_Procedures.Cloth_NameToIdNo(con, cbo_ClothName.Text)
        If Clo_ID = 0 Then
            MessageBox.Show("Invalid Cloth Name", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If cbo_ClothName.Enabled Then cbo_ClothName.Focus()
            Exit Sub
        End If

        If Trim(lbl_WidthType.Text) = "" Then
            MessageBox.Show("Invalid Width Type", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If cbo_ClothName.Enabled Then cbo_ClothName.Focus()
            Exit Sub
        End If

        EdsCnt_ID = 0
        If lbl_EndsCount.Visible = True Then

            EdsCnt_ID = Common_Procedures.EndsCount_NameToIdNo(con, lbl_EndsCount.Text)
            If Val(EdsCnt_ID) = 0 Then
                MessageBox.Show("Invalid Ends/Count", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                If cbo_ClothName.Enabled Then cbo_ClothName.Focus()
                Exit Sub
            End If

            da = New SqlClient.SqlDataAdapter("select * from Cloth_EndsCount_Details Where Cloth_Idno = " & Str(Val(Clo_ID)) & " and EndsCount_IdNo = " & Str(Val(EdsCnt_ID)), con)
            dt1 = New DataTable
            da.Fill(dt1)
            If dt1.Rows.Count = 0 Then
                MessageBox.Show("Mismatch of EndsCount with Cloth Master", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                If cbo_ClothName.Enabled Then cbo_ClothName.Focus()
                Exit Sub
            End If
            dt1.Clear()

        End If

        vEdsCnt_ID_BM1 = 0
        vEdsCnt_ID_BM2 = 0

        If lbl_EndsCount_Beam1.Visible = True Then

            vEdsCnt_ID_BM1 = Common_Procedures.EndsCount_NameToIdNo(con, lbl_EndsCount_Beam1.Text)
            If Val(vEdsCnt_ID_BM1) = 0 And Trim(lbl_SetCode1.Text) <> "" And Trim(lbl_BeamNo1.Text) <> "" Then
                MessageBox.Show("Invalid Ends/Count for Beam1", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                If cbo_ClothName.Enabled Then cbo_ClothName.Focus()
                Exit Sub
            End If

            If Val(vEdsCnt_ID_BM1) <> 0 Then
                da = New SqlClient.SqlDataAdapter("select * from Cloth_EndsCount_Details Where Cloth_Idno = " & Str(Val(Clo_ID)) & " and EndsCount_IdNo = " & Str(Val(vEdsCnt_ID_BM1)), con)
                dt1 = New DataTable
                da.Fill(dt1)
                If dt1.Rows.Count = 0 Then
                    MessageBox.Show("Mismatch of EndsCount-1 with Cloth Master", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                    If cbo_ClothName.Enabled Then cbo_ClothName.Focus()
                    Exit Sub
                End If
                dt1.Clear()
            End If

            EdsCnt_ID = vEdsCnt_ID_BM1

        Else

            If Trim(lbl_SetCode1.Text) <> "" And Trim(lbl_BeamNo1.Text) <> "" Then
                vEdsCnt_ID_BM1 = EdsCnt_ID
            End If

        End If

        If lbl_EndsCount_Beam2.Visible = True Then

            vEdsCnt_ID_BM2 = Common_Procedures.EndsCount_NameToIdNo(con, lbl_EndsCount_Beam2.Text)
            If Val(vEdsCnt_ID_BM2) = 0 And Trim(lbl_SetCode2.Text) <> "" And Trim(lbl_BeamNo2.Text) <> "" Then
                MessageBox.Show("Invalid Ends/Count for Beam2", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                If cbo_ClothName.Enabled Then cbo_ClothName.Focus()
                Exit Sub
            End If

            If Val(vEdsCnt_ID_BM2) <> 0 Then
                da = New SqlClient.SqlDataAdapter("select * from Cloth_EndsCount_Details Where Cloth_Idno = " & Str(Val(Clo_ID)) & " and EndsCount_IdNo = " & Str(Val(vEdsCnt_ID_BM2)), con)
                dt1 = New DataTable
                da.Fill(dt1)
                If dt1.Rows.Count = 0 Then
                    MessageBox.Show("Mismatch of EndsCount-2 with Cloth Master", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                    If cbo_ClothName.Enabled Then cbo_ClothName.Focus()
                    Exit Sub
                End If
                dt1.Clear()
            End If

        Else

            If Trim(lbl_SetCode2.Text) <> "" And Trim(lbl_BeamNo2.Text) <> "" Then
                vEdsCnt_ID_BM2 = EdsCnt_ID
            End If

        End If

        WftCnt_ID = 0
        If btn_Show_WeftConsumption_Details.Visible = False Then
            WftCnt_ID = Common_Procedures.Count_NameToIdNo(con, lbl_WeftCount.Text)
            If Val(WftCnt_ID) = 0 Then

                MessageBox.Show("Invalid Weft Count", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                If cbo_ClothName.Enabled Then cbo_ClothName.Focus()
                Exit Sub
            End If

            MasWftCnt_IDNo = Common_Procedures.get_FieldValue(con, "Cloth_Head", "Cloth_WeftCount_IdNo", "(Cloth_IdNo = " & Str(Val(Clo_ID)) & ")")
            If Val(WftCnt_ID) <> Val(MasWftCnt_IDNo) Then
                MessageBox.Show("Mismatch of Weft Count with Cloth Master", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                If cbo_ClothName.Enabled Then cbo_ClothName.Focus()
                Exit Sub
            End If
        End If

        NoofInpBmsInLom = Common_Procedures.get_FieldValue(con, "Loom_Head", "Noof_Input_Beams", "(Loom_IdNo = " & Str(Val(Lm_ID)) & ")")
        If Val(NoofInpBmsInLom) = 0 Then NoofInpBmsInLom = 1

        If NoofInpBmsInLom = 1 Then
            If Trim(lbl_BeamNo1.Text) = "" And Trim(lbl_BeamNo2.Text) = "" Then
                MessageBox.Show("Invalid Beams", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                If cbo_LoomNo.Enabled Then cbo_LoomNo.Focus()
                Exit Sub
            End If

            If Val(lbl_BalMtrs1.Text) = 0 And Val(lbl_BalMtrs2.Text) = 0 Then
                MessageBox.Show("Invalid Beam Meters", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                If cbo_LoomNo.Enabled Then cbo_LoomNo.Focus()
                Exit Sub
            End If

        Else
            If Trim(lbl_BeamNo1.Text) = "" Or Trim(lbl_BeamNo2.Text) = "" Then
                MessageBox.Show("Invalid Beams", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                If cbo_LoomNo.Enabled Then cbo_LoomNo.Focus()
                Exit Sub
            End If

            If Val(lbl_BalMtrs1.Text) = 0 Or Val(lbl_BalMtrs2.Text) = 0 Then
                MessageBox.Show("Invalid Beam Meters", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                If cbo_LoomNo.Enabled Then cbo_LoomNo.Focus()
                Exit Sub
            End If

        End If

        Dim vDUP_PCSNO As String = ""
        Dim vCHECK_DUP_PcsNo As Boolean = False
        Dim vAUTO_CLOWISE_PcsNo As Boolean = False
        Dim vCOMP_NAME As String
        Dim vCLONAME_CONDT As String

        vAUTO_CLOWISE_PcsNo = False
        vCHECK_DUP_PcsNo = False
        vCLONAME_CONDT = ""

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1186" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1438" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1474" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1487" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1489" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1544" Then

            vCHECK_DUP_PcsNo = True

            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1474" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1489" Then
                vAUTO_CLOWISE_PcsNo = True
            ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1438" Then
                vCOMP_NAME = Common_Procedures.Company_IdNoToName(con, Val(lbl_Company.Tag))
                vCOMP_NAME = vCOMP_NAME.ToString.ToUpper
                If InStr(1, vCOMP_NAME, "KVP") > 0 And InStr(1, vCOMP_NAME, "WEAVES") > 0 Then
                    vAUTO_CLOWISE_PcsNo = True
                End If
            End If

            If vAUTO_CLOWISE_PcsNo = True Then
                vCLONAME_CONDT = " and Cloth_IdNo = " & Str(Val(Clo_ID))
            End If

        End If

        With dgv_Details

            vDUP_PCSNO = ""
            For i = 0 To .RowCount - 1

                If Val(.Rows(i).Cells(dgvCol_Details.Meters).Value) <> 0 Or Val(.Rows(i).Cells(dgvCol_Details.Weight).Value) <> 0 Then

                    If Trim(.Rows(i).Cells(dgvCol_Details.Pcsno).Value) = "" Or Trim(.Rows(i).Cells(dgvCol_Details.Pcsno).Value) = "0" Then
                        MessageBox.Show("Invalid " & Trim(.Columns(dgvCol_Details.Pcsno).HeaderText), "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                        .Focus()
                        .CurrentCell = .Rows(i).Cells(dgvCol_Details.Pcsno)
                        Exit Sub
                    End If

                    If Trim(.Rows(i).Cells(dgvCol_Details.ClothType).Value) = "" Then
                        MessageBox.Show("Invalid Cloth Type", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                        .Focus()
                        .CurrentCell = .Rows(i).Cells(dgvCol_Details.ClothType)
                        Exit Sub
                    End If

                    CloTyp_ID = Common_Procedures.ClothType_NameToIdNo(con, .Rows(i).Cells(dgvCol_Details.ClothType).Value)
                    If CloTyp_ID = 0 Then
                        MessageBox.Show("Invalid Cloth Type", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                        .Focus()
                        .CurrentCell = .Rows(i).Cells(dgvCol_Details.ClothType)
                        Exit Sub
                    End If

                    If Val(.Rows(i).Cells(dgvCol_Details.Meters).Value) = 0 Then
                        MessageBox.Show("Invalid " & .Columns(dgvCol_Details.Meters).HeaderText, "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                        .Focus()
                        .CurrentCell = .Rows(i).Cells(dgvCol_Details.Meters)
                        Exit Sub
                    End If

                    If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1558" Then '---- SOTEXPA QUALIDIS TEXTILE (SULUR)
                        If Val(.Rows(i).Cells(dgvCol_Details.Weight).Value) = 0 Then
                            MessageBox.Show("Invalid Weight", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                            .Focus()
                            .CurrentCell = .Rows(i).Cells(dgvCol_Details.Weight)
                            Exit Sub
                        End If
                    End If

                    If InStr(1, Trim(UCase(vDUP_PCSNO)), "~" & Trim(UCase(.Rows(i).Cells(dgvCol_Details.Pcsno).Value)) & "~") > 0 Then
                        MessageBox.Show("Duplicate " & Trim(.Columns(dgvCol_Details.Pcsno).HeaderText), "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                        .Focus()
                        .CurrentCell = .Rows(i).Cells(dgvCol_Details.Pcsno)
                        Exit Sub
                    End If
                    vDUP_PCSNO = Trim(vDUP_PCSNO) & "~" & Trim(.Rows(i).Cells(dgvCol_Details.Pcsno).Value) & "~"

                    If Trim(.Rows(i).Cells(dgvCol_Details.Pcsno).Value) <> "" And Trim(.Rows(i).Cells(dgvCol_Details.Pcsno).Value) <> "0" Then

                        If vCHECK_DUP_PcsNo = True Then

                            NewCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RollNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

                            da = New SqlClient.SqlDataAdapter("select Lot_Code from Weaver_ClothReceipt_Piece_Details where company_idno = " & Str(Val(lbl_Company.Tag)) & "  " & Trim(vCLONAME_CONDT) & " and Piece_No = '" & Trim(.Rows(i).Cells(dgvCol_Details.Pcsno).Value) & "' and Weaver_ClothReceipt_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "' and Weaver_ClothReceipt_Code <> '" & Trim(NewCode) & "'", con)
                            dt1 = New DataTable
                            da.Fill(dt1)
                            If dt1.Rows.Count > 0 Then
                                MessageBox.Show("Duplicate " & Trim(.Columns(dgvCol_Details.Pcsno).HeaderText) & Chr(13) & "Already entered in Lot No : " & Trim(dt1.Rows(0)(0).ToString), "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                                .Focus()
                                .CurrentCell = .Rows(i).Cells(dgvCol_Details.Pcsno)
                                Exit Sub
                            End If
                            dt1.Clear()

                        End If

                    End If

                End If

            Next

        End With

        Total_Calculation()

        vTot_Typ1Mtrs = 0 : vTot_Typ2Mtrs = 0 : vTot_Typ3Mtrs = 0 : vTot_Typ4Mtrs = 0 : vTot_Typ5Mtrs = 0
        With dgv_Details
            For i = 0 To .RowCount - 1

                If Val(.Rows(i).Cells(dgvCol_Details.Meters).Value) <> 0 Or Val(.Rows(i).Cells(dgvCol_Details.Weight).Value) <> 0 Then

                    CloTyp_ID = Common_Procedures.ClothType_NameToIdNo(con, .Rows(i).Cells(dgvCol_Details.ClothType).Value)
                    If CloTyp_ID <> 0 Then

                        If CloTyp_ID = 1 Then
                            vTot_Typ1Mtrs = vTot_Typ1Mtrs + Val(.Rows(i).Cells(dgvCol_Details.Meters).Value)

                        ElseIf CloTyp_ID = 2 Then
                            vTot_Typ2Mtrs = vTot_Typ2Mtrs + Val(.Rows(i).Cells(dgvCol_Details.Meters).Value)

                        ElseIf CloTyp_ID = 3 Then
                            vTot_Typ3Mtrs = vTot_Typ3Mtrs + Val(.Rows(i).Cells(dgvCol_Details.Meters).Value)

                        ElseIf CloTyp_ID = 4 Then
                            vTot_Typ4Mtrs = vTot_Typ4Mtrs + Val(.Rows(i).Cells(dgvCol_Details.Meters).Value)

                        ElseIf CloTyp_ID = 5 Then
                            vTot_Typ5Mtrs = vTot_Typ5Mtrs + Val(.Rows(i).Cells(dgvCol_Details.Meters).Value)

                        End If

                    End If



                    If CloTyp_ID = 1 And Val(.Rows(i).Cells(dgvCol_Details.Weight).Value()) <> 0 Then
                        vTot_Wgt1 = Format(Val(vTot_Wgt1) + Val(.Rows(i).Cells(dgvCol_Details.Weight).Value()), "##########0.000")
                    End If

                    If CloTyp_ID = 2 And Val(.Rows(i).Cells(dgvCol_Details.Weight).Value()) <> 0 Then
                        vTot_Wgt2 = Format(Val(vTot_Wgt2) + Val(.Rows(i).Cells(dgvCol_Details.Weight).Value()), "##########0.000")
                    End If

                    If CloTyp_ID = 3 And Val(.Rows(i).Cells(dgvCol_Details.Weight).Value()) <> 0 Then
                        vTot_Wgt3 = Format(Val(vTot_Wgt3) + Val(.Rows(i).Cells(dgvCol_Details.Weight).Value()), "##########0.000")
                    End If

                    If CloTyp_ID = 4 And Val(.Rows(i).Cells(dgvCol_Details.Weight).Value()) <> 0 Then
                        vTot_Wgt4 = Format(Val(vTot_Wgt4) + Val(.Rows(i).Cells(dgvCol_Details.Weight).Value()), "##########0.000")
                    End If

                    If CloTyp_ID = 5 And Val(.Rows(i).Cells(dgvCol_Details.Weight).Value()) <> 0 Then
                        vTot_Wgt5 = Format(Val(vTot_Wgt5) + Val(.Rows(i).Cells(dgvCol_Details.Weight).Value()), "##########0.000")
                    End If

                End If

            Next

        End With

        vTot_ChkMtrs = 0 : vTot_Wgt = 0
        If dgv_Details_Total.RowCount > 0 Then
            vTot_ChkMtrs = Val(dgv_Details_Total.Rows(0).Cells(dgvCol_Details.Meters).Value())
            vTot_Wgt = Val(dgv_Details_Total.Rows(0).Cells(dgvCol_Details.Weight).Value())
        End If

        If Val(vTot_ChkMtrs) = 0 Then

            MessageBox.Show("Invalid Checking Meters", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If dgv_Details.Rows.Count > 0 Then
                dgv_Details.Focus()
                dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(dgvCol_Details.ClothType)
            Else
                txt_Folding.Focus()
            End If

            Exit Sub

        End If

        If txt_DoffMtrs.Visible = False Or txt_DoffMtrs.Enabled = False Then
            txt_DoffMtrs.Text = Val(vTot_ChkMtrs)
        End If

        vProd_Mtrs = Val(vTot_ChkMtrs)
        If Val(vProd_Mtrs) = 0 Then
            vProd_Mtrs = Val(txt_DoffMtrs.Text)
        End If

        Call ConsumedPavu_Calculation()
        Call ConsumedYarn_Calculation()
        Call Generate_Barcode()


        vTot_100Fld_Typ1Mtrs = Format(Val(vTot_Typ1Mtrs) * Val(txt_Folding.Text) / 100, "########0.00")
        vTot_100Fld_Typ2Mtrs = Format(Val(vTot_Typ2Mtrs) * Val(txt_Folding.Text) / 100, "########0.00")
        vTot_100Fld_Typ3Mtrs = Format(Val(vTot_Typ3Mtrs) * Val(txt_Folding.Text) / 100, "########0.00")
        vTot_100Fld_Typ4Mtrs = Format(Val(vTot_Typ4Mtrs) * Val(txt_Folding.Text) / 100, "########0.00")
        vTot_100Fld_Typ5Mtrs = Format(Val(vTot_Typ5Mtrs) * Val(txt_Folding.Text) / 100, "########0.00")
        vTot_100Fld_ChkMtr = Format(Val(vTot_ChkMtrs) * Val(txt_Folding.Text) / 100, "########0.00")

        PcsChkCode = ""
        Old_Loom_Idno = 0
        Old_SetCd1 = ""
        Old_Beam1 = ""
        Old_SetCd2 = ""
        Old_Beam2 = ""



        vWEAVER_JOBCARD_FORSELECTION = ""
        If Trim(lbl_Weaver_Job_No.Text) <> "" Then
            vWEAVER_JOBCARD_FORSELECTION = Trim(lbl_Weaver_Job_No.Text)
        End If

        tr = con.BeginTransaction

        Try

            If Insert_Entry = True Or New_Entry = False Then
                NewCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RollNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            Else

                lbl_RollNo.Text = Common_Procedures.get_MaxCode(con, "Weaver_Cloth_Receipt_Head", "Weaver_ClothReceipt_Code", "for_OrderBy", "(Weaver_ClothReceipt_Code LIKE '" & Trim(Pk_Condition) & "%')", Val(lbl_Company.Tag), Common_Procedures.FnYearCode, tr)

                NewCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RollNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            End If


            OrdByNo = Val(Common_Procedures.OrderBy_CodeToValue(lbl_RollNo.Text))
            vSELC_LOTCODE = Trim(lbl_RollNo.Text) & "/" & Trim(Common_Procedures.FnYearCode) & "/" & Trim(Val(lbl_Company.Tag)) & "/" & Trim(Pk_Condition)

            vROLLNO = Trim(lbl_RollNo.Text) & Trim(txt_Roll_SuffixNo.Text)

            cmd.Connection = con
            cmd.Transaction = tr

            cmd.Parameters.Clear()
            cmd.Parameters.AddWithValue("@EntryDate", dtp_Date.Value.Date)


            If New_Entry = True Then

                cmd.CommandText = "Insert into Weaver_Cloth_Receipt_Head ( Receipt_Type, Weaver_ClothReceipt_Code,           Company_IdNo      ,        Weaver_ClothReceipt_RefNo,       Weaver_ClothReceipt_SuffixNo    ,  Weaver_ClothReceipt_No ,     for_OrderBy     , Weaver_ClothReceipt_Date,    Weaver_Piece_Checking_Code , Weaver_Piece_Checking_Increment , Weaver_Piece_Checking_Date ,    Ledger_IdNo     ,       Loom_IdNo   ,             Width_Type            ,           Beam_Knotting_Code     ,       Beam_Knotting_No         ,     Cloth_Idno     ,       EndsCount_Idno  ,     Count_IdNo        ,              Beam_No1           ,              Set_Code1           ,              Set_No1           ,          Balance_Meters1      ,               Beam_No2          ,               Set_Code2          ,             Set_No2            ,            Balance_Meters2    , Folding_Receipt,              Folding_Checking      ,                 Folding            ,  Total_Receipt_Pcs, noof_pcs,       ReceiptMeters_Receipt   ,                ReceiptMeters_Checking      ,             Receipt_Meters    ,       Total_Receipt_Meters    ,       ConsumedYarn_Receipt        ,              ConsumedYarn_Checking     ,              Consumed_Yarn        ,         ConsumedPavu_Receipt       ,        ConsumedPavu_Checking        ,              Consumed_Pavu         ,     BeamConsumption_Receipt            ,            BeamConsumption_Checking    ,         BeamConsumption_Meters         ,             Crimp_Percentage        ,                user_idNo           ,               Bar_Code         ,               Excess_Short_Meter  ,        Type1_Checking_Meters   ,       Type2_Checking_Meters    ,        Type3_Checking_Meters   ,         Type4_Checking_Meters  ,        Type5_Checking_Meters   ,     Total_Checking_Meters     ,         Total_Weight     ,Po_No ,                             lotcode_forSelection        ,                 Is_LastPiece                     ,            Weaving_JobCode_forSelection        ,            EndsCount1_IdNo       ,          EndsCount2_IdNo         ,    ConsumedPavu_Checking_Beam1            ,       ConsumedPavu_Checking_Beam2         , ClothSales_OrderCode_forSelection  ) " &
                                  "            Values                    (      'L'    ,  '" & Trim(NewCode) & "', " & Val(lbl_Company.Tag) & ", '" & Trim(lbl_RollNo.Text) & "' , '" & Trim(txt_Roll_SuffixNo.Text) & "', '" & Trim(vROLLNO) & "' , " & Val(OrdByNo) & ",         @EntryDate      ,       '" & Trim(NewCode) & "' ,                1                ,      @EntryDate            , " & Val(led_id) & ", " & Val(Lm_ID) & ", '" & Trim(lbl_WidthType.Text) & "', '" & Trim(lbl_KnotCode.Text) & "', '" & Trim(lbl_KnotNo.Text) & "', " & Val(Clo_ID) & ", " & Val(EdsCnt_ID) & ", " & Val(WftCnt_ID) & ", '" & Trim(lbl_BeamNo1.Text) & "', '" & Trim(lbl_SetCode1.Text) & "', '" & Trim(lbl_SetNo1.Text) & "', " & Val(lbl_BalMtrs1.Text) & ", '" & Trim(lbl_BeamNo2.Text) & "', '" & Trim(lbl_SetCode2.Text) & "', '" & Trim(lbl_SetNo2.Text) & "', " & Val(lbl_BalMtrs2.Text) & ",      100       ,  " & Str(Val(txt_Folding.Text)) & ", " & Str(Val(txt_Folding.Text)) & " ,       1           ,    1    , " & Val(txt_DoffMtrs.Text) & ", " & Str(Val(lbl_DoffMtrs_Caption.Text)) & ", " & Val(txt_DoffMtrs.Text) & ", " & Val(txt_DoffMtrs.Text) & ", " & Val(lbl_ConsWeftYarn.Text) & ", " & Str(Val(lbl_ConsWeftYarn.Text)) & ", " & Val(lbl_ConsWeftYarn.Text) & ", " & Str(Val(lbl_ConsPavu.Text)) & ",  " & Str(Val(lbl_ConsPavu.Text)) & ", " & Str(Val(lbl_ConsPavu.Text)) & ", " & Str(Val(lbl_BeamConsPavu.Text)) & ", " & Str(Val(lbl_BeamConsPavu.Text)) & ", " & Str(Val(lbl_BeamConsPavu.Text)) & ", " & Str(Val(txt_CrimpPerc.Text)) & ",    " & Val(lbl_UserName.Text) & "  ,'" & Trim(txt_BarCode.Text) & "' , " & Str(Val(lbl_ExcSht.Text)) & ", " & Str(Val(vTot_Typ1Mtrs)) & ", " & Str(Val(vTot_Typ2Mtrs)) & ", " & Str(Val(vTot_Typ3Mtrs)) & ", " & Str(Val(vTot_Typ4Mtrs)) & ", " & Str(Val(vTot_Typ5Mtrs)) & ", " & Str(Val(vTot_ChkMtrs)) & ", " & Str(Val(vTot_Wgt)) & ", '" & Trim(lbl_poNo.Text) & "','" & Trim(vSELC_LOTCODE) & "' , '" & Trim(UCase(cbo_Pcs_LastPiece_Status.Text)) & "',  '" & Trim(vWEAVER_JOBCARD_FORSELECTION) & "' , " & Str(Val(vEdsCnt_ID_BM1)) & " , " & Str(Val(vEdsCnt_ID_BM2)) & " , " & Str(Val(lbl_ConsPavu_Beam1.Text)) & " , " & Str(Val(lbl_ConsPavu_Beam2.Text)) & " , '" & Trim(lbl_ClothSales_OrderCode_forSelection.Text) & "') "
                cmd.ExecuteNonQuery()

            Else

                'Call Common_Procedures.User_Modification_Updation(con, "HEAD", Me.Name, "OLD", "Weaver_Cloth_Receipt_head", "Weaver_ClothReceipt_Code", Val(lbl_Company.Tag), NewCode, lbl_RollNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, False, "", "", "Weaver_ClothReceipt_Code, Company_IdNo, for_OrderBy", tr)

                da = New SqlClient.SqlDataAdapter("Select * from Weaver_Cloth_Receipt_Head Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Weaver_ClothReceipt_Code = '" & Trim(NewCode) & "'", con)
                da.SelectCommand.Transaction = tr
                dt1 = New DataTable
                da.Fill(dt1)
                If dt1.Rows.Count > 0 Then

                    If IsDBNull(dt1.Rows(0).Item("Weaver_Piece_Checking_Code").ToString) = False Then
                        If Trim(dt1.Rows(0).Item("Weaver_Piece_Checking_Code").ToString) <> "" And SaveAll_Sts = False Then
                            PcsChkCode = Trim(dt1.Rows(0).Item("Weaver_Piece_Checking_Code").ToString)
                            'Throw New ApplicationException("Already Piece Checking Prepared")
                            'Exit Sub
                        End If
                    End If

                    Old_Loom_Idno = Val(dt1.Rows(0).Item("Loom_IdNo").ToString)
                    Old_SetCd1 = dt1.Rows(0).Item("set_code1").ToString
                    Old_Beam1 = dt1.Rows(0).Item("beam_no1").ToString
                    Old_SetCd2 = dt1.Rows(0).Item("set_code2").ToString
                    Old_Beam2 = dt1.Rows(0).Item("beam_no2").ToString

                End If
                dt1.Clear()


                cmd.CommandText = "Update Beam_Knotting_Head set Production_Meters = a.Production_Meters - (CASE WHEN b.Total_Checking_Meters <> 0 THEN b.Total_Checking_Meters ELSE b.ReceiptMeters_Receipt END)  from Beam_Knotting_Head a, Weaver_Cloth_Receipt_Head b where b.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and b.Weaver_ClothReceipt_Code = '" & Trim(NewCode) & "' and a.Beam_Knotting_Code = b.Beam_Knotting_Code"
                cmd.ExecuteNonQuery()

                If lbl_ConsPavu_Beam1.Visible = True And lbl_ConsPavu_Beam2.Visible = True Then

                    cmd.CommandText = "Update Stock_SizedPavu_Processing_Details set Production_Meters = a.Production_Meters - b.ConsumedPavu_Checking_Beam1 from Stock_SizedPavu_Processing_Details a, Weaver_Cloth_Receipt_Head b where b.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and b.Weaver_ClothReceipt_Code = '" & Trim(NewCode) & "' and b.Set_code1 <> '' and b.Beam_No1 <> '' and a.set_code = b.Set_code1 and a.Beam_No = b.Beam_No1"
                    cmd.ExecuteNonQuery()

                    cmd.CommandText = "Update Stock_SizedPavu_Processing_Details set Production_Meters = a.Production_Meters - b.ConsumedPavu_Checking_Beam2 from Stock_SizedPavu_Processing_Details a, Weaver_Cloth_Receipt_Head b where b.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and b.Weaver_ClothReceipt_Code = '" & Trim(NewCode) & "' and b.Set_code2 <> '' and b.Beam_No2 <> '' and a.set_code = b.Set_code2 and a.Beam_No = b.Beam_No2"
                    cmd.ExecuteNonQuery()

                Else

                    cmd.CommandText = "Update Stock_SizedPavu_Processing_Details set Production_Meters = a.Production_Meters - b.BeamConsumption_Meters from Stock_SizedPavu_Processing_Details a, Weaver_Cloth_Receipt_Head b where b.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and b.Weaver_ClothReceipt_Code = '" & Trim(NewCode) & "' and b.Set_code1 <> '' and b.Beam_No1 <> '' and a.set_code = b.Set_code1 and a.Beam_No = b.Beam_No1"
                    cmd.ExecuteNonQuery()

                    cmd.CommandText = "Update Stock_SizedPavu_Processing_Details set Production_Meters = a.Production_Meters - b.BeamConsumption_Meters from Stock_SizedPavu_Processing_Details a, Weaver_Cloth_Receipt_Head b where b.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and b.Weaver_ClothReceipt_Code = '" & Trim(NewCode) & "' and b.Set_code2 <> '' and b.Beam_No2 <> '' and a.set_code = b.Set_code2 and a.Beam_No = b.Beam_No2"
                    cmd.ExecuteNonQuery()

                End If


                '------ HEAD Updation
                cmd.CommandText = "Update Weaver_Cloth_Receipt_Head set Weaver_ClothReceipt_RefNo = '" & Trim(lbl_RollNo.Text) & "' , Weaver_ClothReceipt_SuffixNo = '" & Trim(txt_Roll_SuffixNo.Text) & "', Weaver_ClothReceipt_No = '" & Trim(vROLLNO) & "' , Weaver_ClothReceipt_Date = @EntryDate, Weaver_Piece_Checking_Code = '" & Trim(NewCode) & "', Weaver_Piece_Checking_Increment = 1, Weaver_Piece_Checking_Date = @EntryDate, Ledger_IdNo = " & Str(Val(led_id)) & ", Loom_IdNo = " & Str(Val(Lm_ID)) & ", Width_Type = '" & Trim(lbl_WidthType.Text) & "', Beam_Knotting_Code = '" & Trim(lbl_KnotCode.Text) & "', Beam_Knotting_No = '" & Trim(lbl_KnotNo.Text) & "', Cloth_Idno = " & Str(Val(Clo_ID)) & ", EndsCount_IdNo = " & Val(EdsCnt_ID) & ", Count_IdNo = " & Val(WftCnt_ID) & ", set_Code1 = '" & Trim(lbl_SetCode1.Text) & "', set_No1 = '" & Trim(lbl_SetNo1.Text) & "', Beam_No1 = '" & Trim(lbl_BeamNo1.Text) & "', Balance_Meters1 = " & Str(Val(lbl_BalMtrs1.Text)) & ", set_Code2 = '" & Trim(lbl_SetCode2.Text) & "', set_no2 = '" & Trim(lbl_SetNo2.Text) & "', Beam_No2 = '" & Trim(lbl_BeamNo2.Text) & "', Balance_Meters2 = " & Str(Val(lbl_BalMtrs2.Text)) & ", ReceiptMeters_Receipt = " & Str(Val(txt_DoffMtrs.Text)) & ", ReceiptMeters_Checking = " & Str(Val(lbl_DoffMtrs_Caption.Text)) & ", Receipt_Meters = " & Str(Val(txt_DoffMtrs.Text)) & ", Total_Receipt_Meters = " & Str(Val(txt_DoffMtrs.Text)) & ", ConsumedYarn_Receipt = " & Str(Val(lbl_ConsWeftYarn.Text)) & ", ConsumedYarn_Checking = " & Str(Val(lbl_ConsWeftYarn.Text)) & ", Consumed_Yarn = " & Str(Val(lbl_ConsWeftYarn.Text)) & ",  ConsumedPavu_Receipt = " & Str(Val(lbl_ConsPavu.Text)) & ", ConsumedPavu_Checking = " & Str(Val(lbl_ConsPavu.Text)) & ", Consumed_Pavu = " & Str(Val(lbl_ConsPavu.Text)) & ", BeamConsumption_Receipt = " & Str(Val(lbl_BeamConsPavu.Text)) & ", BeamConsumption_Checking = " & Str(Val(lbl_BeamConsPavu.Text)) & ", BeamConsumption_Meters = " & Str(Val(lbl_BeamConsPavu.Text)) & " , Crimp_Percentage = " & Str(Val(txt_CrimpPerc.Text)) & " , Bar_Code = '" & Trim(txt_BarCode.Text) & "' ,   Folding_Checking = " & Str(Val(txt_Folding.Text)) & ", Folding = " & Str(Val(txt_Folding.Text)) & ", Excess_Short_Meter = " & Str(Val(lbl_ExcSht.Text)) & " , Type1_Checking_Meters = " & Str(Val(vTot_Typ1Mtrs)) & ", Type2_Checking_Meters = " & Str(Val(vTot_Typ2Mtrs)) & ", Type3_Checking_Meters = " & Str(Val(vTot_Typ3Mtrs)) & ", Type4_Checking_Meters = " & Str(Val(vTot_Typ4Mtrs)) & ", Type5_Checking_Meters = " & Str(Val(vTot_Typ5Mtrs)) & ", Total_Checking_Meters = " & Str(Val(vTot_ChkMtrs)) & " , Total_weight = " & Str(Val(vTot_Wgt)) & " , po_No='" & Trim(lbl_poNo.Text) & "' ,lotcode_forSelection='" & Trim(vSELC_LOTCODE) & "' , Is_LastPiece = '" & Trim(UCase(cbo_Pcs_LastPiece_Status.Text)) & "',Weaving_JobCode_forSelection = '" & Trim(vWEAVER_JOBCARD_FORSELECTION) & "' , EndsCount1_IdNo = " & Str(Val(vEdsCnt_ID_BM1)) & " , EndsCount2_IdNo = " & Str(Val(vEdsCnt_ID_BM2)) & " , ConsumedPavu_Checking_Beam1 = " & Str(Val(lbl_ConsPavu_Beam1.Text)) & " , ConsumedPavu_Checking_Beam2 = " & Str(Val(lbl_ConsPavu_Beam2.Text)) & " , ClothSales_OrderCode_forSelection = '" & Trim(lbl_ClothSales_OrderCode_forSelection.Text) & "' Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Weaver_ClothReceipt_Code = '" & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()


            End If
            'Call Common_Procedures.User_Modification_Updation(con, "HEAD", Me.Name, "NEW", "Weaver_Cloth_Receipt_head", "Weaver_ClothReceipt_Code", Val(lbl_Company.Tag), NewCode, lbl_RollNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, False, "", "", "Weaver_ClothReceipt_Code, Company_IdNo, for_OrderBy", tr)

            '----------------------

            cmd.CommandText = "Delete from Weaver_Piece_Checking_Head where Weaver_Piece_Checking_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()


            cmd.CommandText = "Insert into Weaver_Piece_Checking_Head ( Receipt_Type, Weaver_Piece_Checking_Code,             Company_IdNo         ,      Weaver_Piece_Checking_No  ,             for_OrderBy  , Weaver_Piece_Checking_Date,      Receipt_PkCondition    ,      Piece_Receipt_Code,  Loom_IdNo        ,     Piece_Receipt_No  , Piece_Receipt_Date,         Ledger_IdNo     ,         Cloth_IdNo ,             ReceiptMeters_Receipt  ,         Crimp_Percentage       ,         Folding              ,  Total_Checking_Receipt_Meters      ,          Total_Type1_Meters    ,      Total_Type2_Meters         ,   Total_Type3_Meters           ,     Total_Type4_Meters          ,     Total_Type5_Meters         ,     Total_Checking_Meters     ,     Total_Weight          ,     Total_Type1Meters_100Folding      ,     Total_Type2Meters_100Folding      ,     Total_Type3Meters_100Folding      ,      Total_Type4Meters_100Folding      ,     Total_Type5Meters_100Folding      ,      Total_Meters_100Folding         ,         Excess_Short_Meter        ,               Bar_Code         ,           Weaving_JobCode_forSelection ) " &
                                "          Values                     (     'L'     ,    '" & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(vROLLNO) & "'        , " & Str(Val(OrdByNo)) & ",      @EntryDate           , '" & Trim(Pk_Condition) & "', '" & Trim(NewCode) & "', " & Val(Lm_ID) & ",'" & Trim(vROLLNO) & "',      @EntryDate   , " & Str(Val(led_id)) & ", " & Val(Clo_ID) & ", " & Str(Val(txt_DoffMtrs.Text)) & ", " & Val(txt_CrimpPerc.Text) & ", " & Val(txt_Folding.Text) & ", " & Str(Val(txt_DoffMtrs.Text)) & " , " & Str(Val(vTot_Typ1Mtrs)) & ",  " & Str(Val(vTot_Typ2Mtrs)) & ", " & Str(Val(vTot_Typ3Mtrs)) & ",  " & Str(Val(vTot_Typ4Mtrs)) & ", " & Str(Val(vTot_Typ5Mtrs)) & ", " & Str(Val(vTot_ChkMtrs)) & ", " & Str(Val(vTot_Wgt)) & ", " & Str(Val(vTot_100Fld_Typ1Mtrs)) & ", " & Str(Val(vTot_100Fld_Typ2Mtrs)) & ", " & Str(Val(vTot_100Fld_Typ3Mtrs)) & ",  " & Str(Val(vTot_100Fld_Typ4Mtrs)) & ", " & Str(Val(vTot_100Fld_Typ5Mtrs)) & ",  " & Str(Val(vTot_100Fld_ChkMtr)) & ", " & Str(Val(lbl_ExcSht.Text)) & " , '" & Trim(txt_BarCode.Text) & "','" & Trim(vWEAVER_JOBCARD_FORSELECTION) & "') "
            cmd.ExecuteNonQuery()

            cmd.CommandText = "delete from Weaver_ClothReceipt_Piece_Details where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Weaver_ClothReceipt_Code = '" & Trim(NewCode) & "' and (PackingSlip_Code_Type1 = '' and PackingSlip_Code_Type2 = '' and PackingSlip_Code_Type3 = '' and PackingSlip_Code_Type4 = '' and PackingSlip_Code_Type5 = '' and BuyerOffer_Code_Type1 = '' and BuyerOffer_Code_Type2 = '' and BuyerOffer_Code_Type3 = '' and BuyerOffer_Code_Type4 = '' and BuyerOffer_Code_Type5 = '')"
            cmd.ExecuteNonQuery()

            With dgv_Details
                Sno = 0

                For i = 0 To .RowCount - 1

                    If Val(.Rows(i).Cells(dgvCol_Details.Meters).Value) <> 0 Then

                        Sno = Sno + 1

                        CloTyp_ID = Common_Procedures.ClothType_NameToIdNo(con, .Rows(i).Cells(dgvCol_Details.ClothType).Value, tr)

                        vBrCode_Typ1 = ""
                        vBrCode_Typ2 = ""
                        vBrCode_Typ3 = ""
                        vBrCode_Typ4 = ""
                        vBrCode_Typ5 = ""



                        vYrCd = Microsoft.VisualBasic.Right(Trim(NewCode), 5)

                        If CloTyp_ID = 1 Then
                            vBrCode_Typ1 = Microsoft.VisualBasic.Left(vYrCd, 2) & Trim(Val(lbl_Company.Tag)) & Trim(UCase(vROLLNO)) & Trim(UCase((.Rows(i).Cells(dgvCol_Details.Pcsno).Value))) & "1"
                        ElseIf CloTyp_ID = 2 Then
                            vBrCode_Typ2 = Microsoft.VisualBasic.Left(vYrCd, 2) & Trim(Val(lbl_Company.Tag)) & Trim(UCase(vROLLNO)) & Trim(UCase((.Rows(i).Cells(dgvCol_Details.Pcsno).Value))) & "2"
                        ElseIf CloTyp_ID = 3 Then
                            vBrCode_Typ3 = Microsoft.VisualBasic.Left(vYrCd, 2) & Trim(Val(lbl_Company.Tag)) & Trim(UCase(vROLLNO)) & Trim(UCase((.Rows(i).Cells(dgvCol_Details.Pcsno).Value))) & "3"
                        ElseIf CloTyp_ID = 4 Then
                            vBrCode_Typ4 = Microsoft.VisualBasic.Left(vYrCd, 2) & Trim(Val(lbl_Company.Tag)) & Trim(UCase(vROLLNO)) & Trim(UCase((.Rows(i).Cells(dgvCol_Details.Pcsno).Value))) & "4"
                        ElseIf CloTyp_ID = 5 Then
                            vBrCode_Typ5 = Microsoft.VisualBasic.Left(vYrCd, 2) & Trim(Val(lbl_Company.Tag)) & Trim(UCase(vROLLNO)) & Trim(UCase((.Rows(i).Cells(dgvCol_Details.Pcsno).Value))) & "5"
                        End If

                        vPcNo = Trim(.Rows(i).Cells(dgvCol_Details.Pcsno).Value)

                        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1186" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1544" Then '---- UNITED WEAVES (PALLADAM)
                            vPcSubNo = get_RollNo_from_PieceNo_for_CC1186(vPcNo)
                            vOrdByPcNo = Common_Procedures.OrderBy_CodeToValue(vPcSubNo)

                        Else
                            vOrdByPcNo = Common_Procedures.OrderBy_CodeToValue(vPcNo)

                        End If

                        vPCSCODE_FORSELECTION = Trim(UCase(.Rows(i).Cells(dgvCol_Details.Pcsno).Value)) & "/" & Trim(Common_Procedures.FnYearCode) & "/" & Trim(Val(lbl_Company.Tag))

                        nr = 0
                        cmd.CommandText = "Update Weaver_ClothReceipt_Piece_Details set Weaver_Piece_Checking_Code = '" & Trim(NewCode) & "',  Weaver_Piece_Checking_No = '" & Trim(vROLLNO) & "', Weaver_Piece_Checking_Date = @EntryDate, StockOff_IdNo = " & Str(Val(stkof_idno)) & ", WareHouse_IdNo = " & Str(Val(vGod_ID)) & ", Ledger_IdNo = " & Str(Val(led_id)) & ", Cloth_IdNo = " & Str(Val(Clo_ID)) & ", Loom_IdNo = " & Str(Val(Lm_ID)) & ", Folding_Receipt = " & Str(Val(txt_Folding.Text)) & ", Folding_Checking = " & Str(Val(txt_Folding.Text)) & ", Folding = " & Str(Val(txt_Folding.Text)) & ", Sl_No = " & Str(Val(Sno)) & ", PieceNo_OrderBy = " & Str(Val(vOrdByPcNo)) & ", PieceCode_for_Selection = '" & Trim(vPCSCODE_FORSELECTION) & "', ReceiptMeters_Checking = " & Str(Val(txt_DoffMtrs.Text)) & ", Receipt_Meters = " & Str(Val(txt_DoffMtrs.Text)) & ", Type" & Trim(Val(CloTyp_ID)) & "_Meters = " & Str(Val(.Rows(i).Cells(dgvCol_Details.Meters).Value)) & ", Total_Checking_Meters = " & Str(Val(.Rows(i).Cells(dgvCol_Details.Meters).Value)) & ", Weight = " & Str(Val(.Rows(i).Cells(dgvCol_Details.Weight).Value)) & ", Weight_Meter = " & Str(Val(.Rows(i).Cells(dgvCol_Details.Wgt_Mtr).Value)) & " , Checked_Pcs_Barcode_Type1 = '" & Trim(vBrCode_Typ1) & "', Checked_Pcs_Barcode_Type2 = '" & Trim(vBrCode_Typ2) & "', Checked_Pcs_Barcode_Type3 = '" & Trim(vBrCode_Typ3) & "', Checked_Pcs_Barcode_Type4 = '" & Trim(vBrCode_Typ4) & "', Checked_Pcs_Barcode_Type5 = '" & Trim(vBrCode_Typ5) & "', Lot_NUmber = '" & Trim(.Rows(i).Cells(dgvCol_Details.Lot_No).Value) & "' , Weaving_JobCode_forSelection = '" & Trim(lbl_Weaver_Job_No.Text) & "'  Where Weaver_ClothReceipt_Code = '" & Trim(NewCode) & "' and Lot_Code = '" & Trim(NewCode) & "' and Piece_No = '" & Trim(.Rows(i).Cells(dgvCol_Details.Pcsno).Value) & "'"
                        nr = cmd.ExecuteNonQuery()

                        If nr = 0 Then
                            cmd.CommandText = "Insert into Weaver_ClothReceipt_Piece_Details ( Weaver_Piece_Checking_Code ,             Company_IdNo         ,     Weaver_Piece_Checking_No,  Weaver_Piece_Checking_Date,   Weaver_ClothReceipt_Code,    Weaver_ClothReceipt_No ,         for_orderby      , Weaver_ClothReceipt_Date,           Lot_Code       ,               Lot_No   ,           StockOff_IdNo     ,         WareHouse_IdNo   ,         Ledger_IdNo     ,           Cloth_IdNo    ,            Loom_IdNo   ,            Folding_Receipt        ,             Folding_Checking       ,             Folding               ,           Sl_No      ,                                       Piece_No           ,                         Main_PieceNo                            ,           PieceNo_OrderBy    ,            PieceCode_for_Selection    ,            ReceiptMeters_Checking  ,                Receipt_Meters        ,   Type" & Trim(Val(CloTyp_ID)) & "_Meters                    ,                                 Total_Checking_Meters        ,                                           Weight                ,                                        Weight_Meter          ,   Checked_Pcs_Barcode_Type1 ,   Checked_Pcs_Barcode_Type2 ,   Checked_Pcs_Barcode_Type3 ,   Checked_Pcs_Barcode_Type4 ,   Checked_Pcs_Barcode_Type5,                    Lot_NUmber                              ,     Weaving_JobCode_forSelection       ) " &
                                                "     Values                                 (    '" & Trim(NewCode) & "' , " & Str(Val(lbl_Company.Tag)) & ",  '" & Trim(vROLLNO) & "'    ,            @EntryDate      ,    '" & Trim(NewCode) & "', '" & Trim(vROLLNO) & "'   , " & Str(Val(OrdByNo)) & ",      @EntryDate           , '" & Trim(NewCode) & "', '" & Trim(vROLLNO) & "', " & Str(Val(stkof_idno)) & ", " & Str(Val(vGod_ID)) & ", " & Str(Val(led_id)) & ", " & Str(Val(Clo_ID)) & ", " & Str(Val(Lm_ID)) & ", " & Str(Val(txt_Folding.Text)) & ", " & Str(Val(txt_Folding.Text)) & ", " & Str(Val(txt_Folding.Text)) & ", " & Str(Val(Sno)) & ", '" & Trim(.Rows(i).Cells(dgvCol_Details.Pcsno).Value) & "',  '" & Trim(Val(.Rows(i).Cells(dgvCol_Details.Pcsno).Value)) & "',  " & Str(Val(vOrdByPcNo)) & ",  '" & Trim(vPCSCODE_FORSELECTION) & "', " & Str(Val(txt_DoffMtrs.Text)) & ",  " & Str(Val(txt_DoffMtrs.Text)) & ", " & Str(Val(.Rows(i).Cells(dgvCol_Details.Meters).Value)) & ", " & Str(Val(.Rows(i).Cells(dgvCol_Details.Meters).Value)) & ", " & Str(Val(.Rows(i).Cells(dgvCol_Details.Weight).Value)) & " , " & Str(Val(.Rows(i).Cells(dgvCol_Details.Wgt_Mtr).Value)) & " , '" & Trim(vBrCode_Typ1) & "', '" & Trim(vBrCode_Typ2) & "', '" & Trim(vBrCode_Typ3) & "', '" & Trim(vBrCode_Typ4) & "', '" & Trim(vBrCode_Typ5) & "', '" & Trim(.Rows(i).Cells(dgvCol_Details.Lot_No).Value) & "', '" & Trim(lbl_Weaver_Job_No.Text) & "' ) "
                            nr = cmd.ExecuteNonQuery()
                        End If


                        cmd.CommandText = "Update JobWork_Piece_Delivery_Details set Po_No = '" & Trim(dgv_Details.Rows(i).Cells(dgvCol_Details.Lot_No).Value) & "'  where lot_code = '" & Trim(NewCode) & "'"
                        cmd.ExecuteNonQuery()


                    End If

                Next
                'Call Common_Procedures.User_Modification_Updation(con, "DETAILS", Me.Name, "NEW", "Weaver_ClothReceipt_Piece_Details", "Weaver_Piece_Checking_Code", Val(lbl_Company.Tag), NewCode, lbl_ChkNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, False, "Piece_No,PieceNo_OrderBy,ReceiptMeters_Checking", "Sl_No", "Weaver_Piece_Checking_Code, For_OrderBy, Company_IdNo, Weaver_Piece_Checking_No, Weaver_Piece_Checking_Date, Ledger_Idno", tr)

            End With

            '-----------------------


            EntID = Trim(Pk_Condition) & Trim(lbl_RollNo.Text)
            Partcls = "Doff : Roll.No. " & Trim(vROLLNO)
            PBlNo = Trim(vROLLNO)

            nr = 0
            cmd.CommandText = "Update Beam_Knotting_Head set Production_Meters = Production_Meters + " & Str(Val(vProd_Mtrs)) & " where Loom_IdNo = " & Str(Val(Lm_ID)) & " and Beam_Knotting_Code = '" & Trim(lbl_KnotCode.Text) & "' and Ledger_IdNo = " & Str(Val(led_id))
            nr = cmd.ExecuteNonQuery
            If nr = 0 Then
                Throw New ApplicationException("Mismatch of Loom Knotting && Party")
                Exit Sub
            End If

            YrnPartcls = Partcls & ", Cloth : " & Trim(cbo_ClothName.Text) & ", Meters :" & Str(Val(vProd_Mtrs))


            cmd.CommandText = "Delete from Stock_Pavu_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Stock_Yarn_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Stock_Cloth_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()



            Delv_ID = 0 : Rec_ID = 0
            If Trim(UCase(Led_type)) = "JOBWORKER" Then
                Delv_ID = led_id
                Rec_ID = 0
            Else
                Delv_ID = 0
                Rec_ID = led_id
            End If

            If Trim(lbl_SetCode1.Text) <> "" And Trim(lbl_BeamNo1.Text) <> "" Then
                Dim vPRODMTRS_BM1 As String = 0
                If lbl_ConsPavu_Beam1.Visible = True Then
                    vPRODMTRS_BM1 = lbl_ConsPavu_Beam1.Text
                Else
                    vPRODMTRS_BM1 = lbl_BeamConsPavu.Text
                End If
                cmd.CommandText = "Update Stock_SizedPavu_Processing_Details set Production_Meters = Production_Meters + " & Str(Val(vPRODMTRS_BM1)) & " where set_code = '" & Trim(lbl_SetCode1.Text) & "' and beam_no = '" & Trim(lbl_BeamNo1.Text) & "'"
                cmd.ExecuteNonQuery()
            End If

            If Trim(lbl_SetCode2.Text) <> "" And Trim(lbl_BeamNo2.Text) <> "" Then
                Dim vPRODMTRS_BM2 As String = 0
                If lbl_ConsPavu_Beam2.Visible = True Then
                    vPRODMTRS_BM2 = lbl_ConsPavu_Beam2.Text
                Else
                    vPRODMTRS_BM2 = lbl_BeamConsPavu.Text
                End If
                cmd.CommandText = "Update Stock_SizedPavu_Processing_Details set Production_Meters = Production_Meters + " & Str(Val(vPRODMTRS_BM2)) & " where set_code = '" & Trim(lbl_SetCode2.Text) & "' and beam_no = '" & Trim(lbl_BeamNo2.Text) & "'"
                cmd.ExecuteNonQuery()
            End If



            vSTKPOSTING_STS = False
            If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1204" Then '-----KOHINOOR TEXTILE MILLS(PALLADAM)    or   RAJAMURUGAN MILLS (PALLADAM)

                If Trim(UCase(Led_type)) <> "JOBWORKER" Or (Common_Procedures.settings.JobWorker_Pavu_Yarn_Stock_Posting_IN_Production = 1 And Trim(UCase(Led_type)) = "JOBWORKER") Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1192" Then

                    vSTKPOSTING_STS = True

                    If lbl_EndsCount_Beam1.Visible = True And lbl_EndsCount_Beam2.Visible = True Then

                        If Val(vEdsCnt_ID_BM1) = Val(vEdsCnt_ID_BM2) Then
                            GoTo GOTOLOOP_10

                        Else

                            If Val(lbl_ConsPavu_Beam1.Text) <> 0 Then
                                cmd.CommandText = "Insert into Stock_Pavu_Processing_Details ( Reference_Code, Company_IdNo, Reference_No, for_OrderBy, Reference_Date, DeliveryTo_Idno, ReceivedFrom_Idno, Cloth_Idno, Entry_ID, Party_Bill_No, Particulars, Sl_No, EndsCount_IdNo, Sized_Beam, Meters ,Weaving_JobCode_forSelection , ClothSales_OrderCode_forSelection) Values ('" & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(vROLLNO) & "', " & Str(Val(OrdByNo)) & ", @EntryDate, " & Str(Val(Delv_ID)) & ", " & Str(Val(Rec_ID)) & ", " & Str(Val(Clo_ID)) & ", '" & Trim(EntID) & "', '" & Trim(PBlNo) & "', '" & Trim(Partcls) & "', 1, " & Str(Val(vEdsCnt_ID_BM1)) & ", 0, " & Str(Val(lbl_ConsPavu_Beam1.Text)) & "     ,'" & Trim(vWEAVER_JOBCARD_FORSELECTION) & "' , '" & Trim(lbl_ClothSales_OrderCode_forSelection.Text) & "')"
                                cmd.ExecuteNonQuery()
                            End If
                            If Val(lbl_ConsPavu_Beam2.Text) <> 0 Then
                                cmd.CommandText = "Insert into Stock_Pavu_Processing_Details ( Reference_Code, Company_IdNo, Reference_No, for_OrderBy, Reference_Date, DeliveryTo_Idno, ReceivedFrom_Idno, Cloth_Idno, Entry_ID, Party_Bill_No, Particulars, Sl_No, EndsCount_IdNo, Sized_Beam, Meters ,Weaving_JobCode_forSelection , ClothSales_OrderCode_forSelection ) Values ('" & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(vROLLNO) & "', " & Str(Val(OrdByNo)) & ", @EntryDate, " & Str(Val(Delv_ID)) & ", " & Str(Val(Rec_ID)) & ", " & Str(Val(Clo_ID)) & ", '" & Trim(EntID) & "', '" & Trim(PBlNo) & "', '" & Trim(Partcls) & "', 2, " & Str(Val(vEdsCnt_ID_BM2)) & ", 0, " & Str(Val(lbl_ConsPavu_Beam2.Text)) & "     ,'" & Trim(vWEAVER_JOBCARD_FORSELECTION) & "' , '" & Trim(lbl_ClothSales_OrderCode_forSelection.Text) & "' )"
                                cmd.ExecuteNonQuery()
                            End If

                        End If

                    Else

GOTOLOOP_10:
                        cmd.CommandText = "Insert into Stock_Pavu_Processing_Details ( Reference_Code, Company_IdNo, Reference_No, for_OrderBy, Reference_Date, DeliveryTo_Idno, ReceivedFrom_Idno, Cloth_Idno, Entry_ID, Party_Bill_No, Particulars, Sl_No, EndsCount_IdNo, Sized_Beam, Meters ,Weaving_JobCode_forSelection) Values ('" & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(vROLLNO) & "', " & Str(Val(OrdByNo)) & ", @EntryDate, " & Str(Val(Delv_ID)) & ", " & Str(Val(Rec_ID)) & ", " & Str(Val(Clo_ID)) & ", '" & Trim(EntID) & "', '" & Trim(PBlNo) & "', '" & Trim(Partcls) & "', 1, " & Str(Val(EdsCnt_ID)) & ", 0, " & Str(Val(lbl_ConsPavu.Text)) & "     ,'" & Trim(vWEAVER_JOBCARD_FORSELECTION) & "' )"
                        cmd.ExecuteNonQuery()

                    End If

                    If btn_Show_WeftConsumption_Details.Visible = False Then
                        cmd.CommandText = "Insert into Stock_Yarn_Processing_Details ( Reference_Code, Company_IdNo, Reference_No, for_OrderBy, Reference_Date, DeliveryTo_Idno, ReceivedFrom_Idno, Entry_ID, Particulars, Party_Bill_No, Sl_No, Count_IdNo, Yarn_Type, Mill_IdNo, Bags, Cones, Weight,Weaving_JobCode_forSelection , ClothSales_OrderCode_forSelection ) Values ('" & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(vROLLNO) & "', " & Str(Val(OrdByNo)) & ", @EntryDate, " & Str(Val(Delv_ID)) & ", " & Str(Val(Rec_ID)) & ", '" & Trim(EntID) & "', '" & Trim(YrnPartcls) & "', '" & Trim(PBlNo) & "', 1, " & Str(Val(WftCnt_ID)) & ", 'MILL', 0, 0, 0, " & Str(Val(lbl_ConsWeftYarn.Text)) & " ,'" & Trim(vWEAVER_JOBCARD_FORSELECTION) & "' , '" & Trim(lbl_ClothSales_OrderCode_forSelection.Text) & "' )"
                        cmd.ExecuteNonQuery()
                    End If


                End If

            End If


            '----Multi WeftCount Yarn consumption posting

            cmd.CommandText = "delete from Weaver_ClothReceipt_Consumed_Yarn_Details where Weaver_ClothReceipt_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            If btn_Show_WeftConsumption_Details.Visible = True Then

                With dgv_Weft_Consumption_Details
                    Sno = 0
                    For i = 0 To .RowCount - 1

                        If Trim(.Rows(i).Cells(0).Value) <> "" Then

                            vWFTCNTIDno = Common_Procedures.Count_NameToIdNo(con, Trim(.Rows(i).Cells(0).Value), tr)

                            If Val(vWFTCNTIDno) <> 0 Then

                                Sno = Sno + 1

                                cmd.CommandText = "Insert into Weaver_ClothReceipt_Consumed_Yarn_Details (  Weaver_ClothReceipt_Code ,           Company_IdNo           ,           Sl_No      ,             Count_IdNo       ,                    Gram_Perc_Type       ,                    Consumption_Gram_Perc  ,                Consumed_Yarn_Weight        )  " &
                                                    " Values                                             (    '" & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", " & Str(Val(Sno)) & ", " & Str(Val(vWFTCNTIDno)) & ", '" & Trim(.Rows(i).Cells(1).Value) & "' , " & Str(Val(.Rows(i).Cells(2).Value)) & " ,  " & Str(Val(.Rows(i).Cells(3).Value)) & " ) "
                                cmd.ExecuteNonQuery()

                                If vSTKPOSTING_STS = True And Val(.Rows(i).Cells(3).Value) <> 0 Then

                                    cmd.CommandText = "Insert into Stock_Yarn_Processing_Details (     Reference_Code    ,                Company_IdNo      ,          Reference_No  ,        for_OrderBy       , Reference_Date,      DeliveryTo_Idno     ,       ReceivedFrom_Idno ,         Entry_ID     ,         Particulars       ,      Party_Bill_No   ,              Sl_No           ,           Count_IdNo         , Yarn_Type, Mill_IdNo, Bags, Cones,                 Weight                    ,            Weaving_JobCode_forSelection       , ClothSales_OrderCode_forSelection) " &
                                                        "           Values                       ('" & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(vROLLNO) & "', " & Str(Val(OrdByNo)) & ",   @EntryDate  , " & Str(Val(Delv_ID)) & ", " & Str(Val(Rec_ID)) & ", '" & Trim(EntID) & "', '" & Trim(YrnPartcls) & "', '" & Trim(PBlNo) & "',  " & Str(Val(1000 + Sno)) & " , " & Str(Val(vWFTCNTIDno)) & ",   'MILL' ,     0    ,   0 ,    0 , " & Str(Val(.Rows(i).Cells(3).Value)) & " , '" & Trim(vWEAVER_JOBCARD_FORSELECTION) & "' , '" & Trim(lbl_ClothSales_OrderCode_forSelection.Text) & "' ) "
                                    cmd.ExecuteNonQuery()

                                End If

                            End If

                        End If

                    Next

                End With

            End If


            If Val(txt_DoffMtrs.Text) <> 0 Or Val(vTot_ChkMtrs) <> 0 Then

                Delv_ID = 0 : Rec_ID = 0
                If Val(led_id) = Val(Common_Procedures.CommonLedger.Godown_Ac) Then
                    Delv_ID = Val(Common_Procedures.CommonLedger.Godown_Ac)
                    Rec_ID = 0

                Else
                    Delv_ID = Val(Common_Procedures.CommonLedger.Godown_Ac)
                    Rec_ID = Val(led_id)

                End If


                vUC_Mtrs = 0
                If Val(vTot_ChkMtrs) = 0 Then
                    vUC_Mtrs = Val(txt_DoffMtrs.Text)
                End If

                cmd.CommandText = "Insert into Stock_Cloth_Processing_Details (    Reference_Code     ,             Company_IdNo         ,      Reference_No      ,     for_OrderBy          , Reference_Date,         StockOff_IdNo       ,  DeliveryTo_Idno      ,       ReceivedFrom_Idno ,         Entry_ID     ,      Party_Bill_No   ,      Particulars       , Sl_No,          Cloth_Idno        ,                 Folding            ,          UnChecked_Meters ,               Meters_Type1     ,              Meters_Type2      ,              Meters_Type3      ,              Meters_Type4       ,              Meters_Type5       ,            Weight_Type1       ,            Weight_Type2      ,            Weight_Type3       ,             Weight_Type4       ,              Weight_Type5       ,                      ClothSales_OrderCode_forSelection       ) " &
                                    "          Values                         ('" & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(vROLLNO) & "', " & Str(Val(OrdByNo)) & ",    @EntryDate , " & Str(Val(stkof_idno)) & ", " & Str(Val(Delv_ID)) & ", " & Str(Val(Rec_ID)) & ", '" & Trim(EntID) & "', '" & Trim(PBlNo) & "', '" & Trim(Partcls) & "',   1  , " & Str(Val(Clo_ID)) & ", " & Str(Val(txt_Folding.Text)) & " , " & Str(Val(vUC_Mtrs)) & ", " & Str(Val(vTot_Typ1Mtrs)) & ", " & Str(Val(vTot_Typ2Mtrs)) & ", " & Str(Val(vTot_Typ3Mtrs)) & ", " & Str(Val(vTot_Typ4Mtrs)) & " , " & Str(Val(vTot_Typ5Mtrs)) & " ,  " & Str(Val(vTot_Wgt1)) & "  ,  " & Str(Val(vTot_Wgt2)) & " ,   " & Str(Val(vTot_Wgt3)) & " ,   " & Str(Val(vTot_Wgt4)) & "  ,     " & Str(Val(vTot_Wgt5)) & " , '" & Trim(lbl_ClothSales_OrderCode_forSelection.Text) & "'  ) "
                cmd.ExecuteNonQuery()


                '---******************************************************** COMMEMTED BY GOPI 2025-01-13 ---- FOR WEIGHT TYPE INSERT

                'cmd.CommandText = "Insert into Stock_Cloth_Processing_Details (    Reference_Code     ,             Company_IdNo         ,      Reference_No      ,     for_OrderBy          , Reference_Date,         StockOff_IdNo       ,  DeliveryTo_Idno      ,       ReceivedFrom_Idno ,         Entry_ID     ,      Party_Bill_No   ,      Particulars       , Sl_No,          Cloth_Idno        ,                 Folding            ,          UnChecked_Meters ,               Meters_Type1     ,              Meters_Type2      ,              Meters_Type3      ,              Meters_Type4       ,              Meters_Type5       , ClothSales_OrderCode_forSelection) " &
                '                    "          Values                         ('" & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(vROLLNO) & "', " & Str(Val(OrdByNo)) & ",    @EntryDate , " & Str(Val(stkof_idno)) & ", " & Str(Val(Delv_ID)) & ", " & Str(Val(Rec_ID)) & ", '" & Trim(EntID) & "', '" & Trim(PBlNo) & "', '" & Trim(Partcls) & "',   1  , " & Str(Val(Clo_ID)) & ", " & Str(Val(txt_Folding.Text)) & " , " & Str(Val(vUC_Mtrs)) & ", " & Str(Val(vTot_Typ1Mtrs)) & ", " & Str(Val(vTot_Typ2Mtrs)) & ", " & Str(Val(vTot_Typ3Mtrs)) & ", " & Str(Val(vTot_Typ4Mtrs)) & " , " & Str(Val(vTot_Typ5Mtrs)) & " , '" & Trim(lbl_ClothSales_OrderCode_forSelection.Text) & "') "
                'cmd.ExecuteNonQuery()

            End If


            '---***********************************************************COMMEMTED BY THANGES FOR -  KVP WEAVES - TODAY-ONLY(06-10-2023)
            'cmd.CommandText = "Truncate Table " & Trim(Common_Procedures.EntryTempTable) & ""
            'cmd.ExecuteNonQuery()

            'If New_Entry = False Then
            '    '----- Editing
            '    If Trim(Old_SetCd1) <> "" And Trim(Old_Beam1) <> "" Then
            '        cmd.CommandText = "Insert Into " & Trim(Common_Procedures.EntryTempTable) & "(Name1, Name2, Meters1) select Set_Code, Beam_No, Production_Meters from Stock_SizedPavu_Processing_Details where Set_Code = '" & Trim(Old_SetCd1) & "' and Beam_No = '" & Trim(Old_Beam1) & "'"
            '        cmd.ExecuteNonQuery()
            '        cmd.CommandText = "Insert Into " & Trim(Common_Procedures.EntryTempTable) & "(Name1, Name2, Meters1) select '" & Trim(Old_SetCd1) & "', '" & Trim(Old_Beam1) & "', -1*BeamConsumption_Meters from Weaver_Cloth_Receipt_Head where (Set_Code1 = '" & Trim(Old_SetCd1) & "' and Beam_No1 = '" & Trim(Old_Beam1) & "') OR (Set_Code2 = '" & Trim(Old_SetCd1) & "' and Beam_No2 = '" & Trim(Old_Beam1) & "')"
            '        cmd.ExecuteNonQuery()
            '    End If
            '    If Trim(Old_SetCd2) <> "" And Trim(Old_Beam2) <> "" Then
            '        cmd.CommandText = "Insert Into " & Trim(Common_Procedures.EntryTempTable) & "(Name1, Name2, Meters1) select Set_Code, Beam_No, Production_Meters from Stock_SizedPavu_Processing_Details where Set_Code = '" & Trim(Old_SetCd2) & "' and Beam_No = '" & Trim(Old_Beam2) & "'"
            '        cmd.ExecuteNonQuery()

            '        cmd.CommandText = "Insert Into " & Trim(Common_Procedures.EntryTempTable) & "(Name1, Name2, Meters1) select '" & Trim(Old_SetCd2) & "', '" & Trim(Old_Beam2) & "', -1*BeamConsumption_Meters from Weaver_Cloth_Receipt_Head where (Set_Code1 = '" & Trim(Old_SetCd2) & "' and Beam_No1 = '" & Trim(Old_Beam2) & "') OR (Set_Code2 = '" & Trim(Old_SetCd2) & "' and Beam_No2 = '" & Trim(Old_Beam2) & "')"
            '        cmd.ExecuteNonQuery()

            '    End If

            'End If

            '''''----- Saving

            'If Trim(lbl_SetCode1.Text) <> "" And Trim(lbl_BeamNo1.Text) <> "" Then
            '    cmd.CommandText = "Insert Into " & Trim(Common_Procedures.EntryTempTable) & "(Name1, Name2, Meters1) select Set_Code, Beam_No, Production_Meters from Stock_SizedPavu_Processing_Details where Set_Code = '" & Trim(lbl_SetCode1.Text) & "' and Beam_No = '" & Trim(lbl_BeamNo1.Text) & "'"
            '    cmd.ExecuteNonQuery()

            '    cmd.CommandText = "Insert Into " & Trim(Common_Procedures.EntryTempTable) & "(Name1, Name2, Meters1) select '" & Trim(lbl_SetCode1.Text) & "', '" & Trim(lbl_BeamNo1.Text) & "', -1*BeamConsumption_Meters from Weaver_Cloth_Receipt_Head where (Set_Code1 = '" & Trim(lbl_SetCode1.Text) & "' and Beam_No1 = '" & Trim(lbl_BeamNo1.Text) & "') OR (Set_Code2 = '" & Trim(lbl_SetCode1.Text) & "' and Beam_No2 = '" & Trim(lbl_BeamNo1.Text) & "')"
            '    cmd.ExecuteNonQuery()
            'End If
            'If Trim(lbl_SetCode2.Text) <> "" And Trim(lbl_BeamNo2.Text) <> "" Then
            '    cmd.CommandText = "Insert Into " & Trim(Common_Procedures.EntryTempTable) & "(Name1, Name2, Meters1) select Set_Code, Beam_No, Production_Meters from Stock_SizedPavu_Processing_Details where Set_Code = '" & Trim(lbl_SetCode2.Text) & "' and Beam_No = '" & Trim(lbl_BeamNo2.Text) & "'"
            '    cmd.ExecuteNonQuery()
            '    cmd.CommandText = "Insert Into " & Trim(Common_Procedures.EntryTempTable) & "(Name1, Name2, Meters1) select '" & Trim(lbl_SetCode2.Text) & "', '" & Trim(lbl_BeamNo2.Text) & "', -1*BeamConsumption_Meters from Weaver_Cloth_Receipt_Head where (Set_Code1 = '" & Trim(lbl_SetCode2.Text) & "' and Beam_No1 = '" & Trim(lbl_BeamNo2.Text) & "') OR (Set_Code2 = '" & Trim(lbl_SetCode2.Text) & "' and Beam_No2 = '" & Trim(lbl_BeamNo2.Text) & "')"
            '    cmd.ExecuteNonQuery()
            'End If

            'da = New SqlClient.SqlDataAdapter("select Name1, Name2, sum(Meters1) as ProdMtrs from " & Trim(Common_Procedures.EntryTempTable) & " Group by Name1, Name2 having sum(Meters1) <> 0 Order by Name1, Name2", con)
            'da.SelectCommand.Transaction = tr
            'dt2 = New DataTable
            'da.Fill(dt2)
            'If dt2.Rows.Count > 0 Then
            '    If IsDBNull(dt2.Rows(0).Item("ProdMtrs").ToString) = False Then
            '        If Val(dt2.Rows(0).Item("ProdMtrs").ToString) <> 0 Then
            '            Throw (New ApplicationException("Invalid Editing : Mismatch of Production Meters"))
            '            Exit Sub
            '        End If
            '    End If
            'End If
            'dt2.Clear()


            ''----- Saving
            'cmd.CommandText = "Truncate Table " & Trim(Common_Procedures.EntryTempTable) & ""
            'cmd.ExecuteNonQuery()

            'cmd.CommandText = "Insert Into " & Trim(Common_Procedures.EntryTempTable) & "(Meters1) select Production_Meters from Beam_Knotting_Head where Beam_Knotting_Code = '" & Trim(lbl_KnotCode.Text) & "'"
            'cmd.ExecuteNonQuery()
            'cmd.CommandText = "Insert Into " & Trim(Common_Procedures.EntryTempTable) & "(Meters1) select -1*(CASE WHEN Total_Checking_Meters <> 0 THEN Total_Checking_Meters ELSE ReceiptMeters_Receipt END) from Weaver_Cloth_Receipt_Head where Beam_Knotting_Code = '" & Trim(lbl_KnotCode.Text) & "' and Weaver_ClothReceipt_Code LIKE '" & Trim(Pk_Condition) & "%'"
            'cmd.ExecuteNonQuery()
            'cmd.CommandText = "Insert Into " & Trim(Common_Procedures.EntryTempTable) & "(Meters1) select -1*ReceiptMeters_Receipt from Weaver_Cloth_Receipt_Head where Beam_Knotting_Code = '" & Trim(lbl_KnotCode.Text) & "' and Weaver_ClothReceipt_Code NOT LIKE '" & Trim(Pk_Condition) & "%'"
            'cmd.ExecuteNonQuery()

            'da = New SqlClient.SqlDataAdapter("select sum(Meters1) as ProdMtrs from " & Trim(Common_Procedures.EntryTempTable) & " having sum(Meters1) <> 0", con)
            'da.SelectCommand.Transaction = tr
            'dt2 = New DataTable
            'da.Fill(dt2)
            'If dt2.Rows.Count > 0 Then
            '    If IsDBNull(dt2.Rows(0)(0).ToString) = False Then
            '        If Val(dt2.Rows(0)(0).ToString) <> 0 Then
            '            Throw New ApplicationException("Invalid Editing : Mismatch of Production Meters")
            '            Exit Sub
            '        End If
            '    End If
            'End If
            'dt2.Clear()

            'If New_Entry = True Then

            '    If Common_Procedures.Check_SizedBeam_Doffing_Meters(con, Clo_ID, lbl_SetCode1.Text, lbl_BeamNo1.Text, vErrMsg, tr) = True Then
            '        Throw New ApplicationException(vErrMsg)
            '        Exit Sub
            '    End If

            '    If Common_Procedures.Check_SizedBeam_Doffing_Meters(con, Clo_ID, lbl_SetCode2.Text, lbl_BeamNo2.Text, vErrMsg, tr) = True Then
            '        Throw New ApplicationException(vErrMsg)
            '        Exit Sub
            '    End If

            'End If

            ''----- Saving Cross Checking
            'vErrMsg = ""
            'Dim vFAB_LOTCODE As String
            'vFAB_LOTCODE = "~" & Trim(NewCode) & "~"
            'If Common_Procedures.Cross_Checking_PieceChecking_PackingSlip_Meters(con, vFAB_LOTCODE, vErrMsg, tr) = False Then
            '    Throw New ApplicationException(vErrMsg)
            '    Exit Sub
            'End If


            tr.Commit()


            If SaveAll_Sts <> True Then
                MessageBox.Show("Saved Sucessfully!!!", "FOR SAVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Information)
                'new_record()
                If New_Entry = True Then
                    new_record()
                Else
                    move_record(lbl_RollNo.Text)
                End If

            End If

        Catch ex As Exception
            tr.Rollback()
            MessageBox.Show(ex.Message, "FOR SAVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

        If msk_Date.Visible And msk_Date.Enabled Then msk_Date.Focus()

    End Sub


    Private Sub cbo_ClothName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_ClothName.GotFocus
        Dim Da4 As New SqlClient.SqlDataAdapter
        Dim Dt4 As New DataTable
        Dim Lm_ID As Integer = 0
        Dim Clo_ID1 As Integer = 0
        Dim Clo_ID2 As Integer = 0
        Dim Clo_ID3 As Integer = 0
        Dim Clo_ID4 As Integer = 0
        Dim Clo_Wrp_ID1 As Integer = 0, Clo_Wrp_ID2 As Integer = 0, Clo_Wrp_ID3 As Integer = 0, Clo_Wrp_ID4 As Integer = 0
        Dim Clo_Wft_ID1 As Integer = 0, Clo_Wft_ID2 As Integer = 0, Clo_Wft_ID3 As Integer = 0, Clo_Wft_ID4 As Integer = 0
        Dim Clo_Reed1 As Integer = 0, Clo_Reed2 As Integer = 0, Clo_Reed3 As Integer = 0, Clo_Reed4 As Integer = 0
        Dim Clo_Width1 As Integer = 0, Clo_Width2 As Integer = 0, Clo_Width3 As Integer = 0, Clo_Width4 As Integer = 0

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1037" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1474" Then '---- Prakash Textiles (Somanur)

            Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Cloth_Head", "Cloth_Name", "", "(Cloth_IdNo = 0)")

        Else

            Lm_ID = Common_Procedures.Loom_NameToIdNo(con, cbo_LoomNo.Text)

            Clo_ID1 = 0 : Clo_ID2 = 0 : Clo_ID3 = 0 : Clo_ID4 = 0
            Clo_Wrp_ID1 = 0 : Clo_Wrp_ID2 = 0 : Clo_Wrp_ID3 = 0 : Clo_Wrp_ID4 = 0
            Clo_Wft_ID1 = 0 : Clo_Wft_ID2 = 0 : Clo_Wft_ID3 = 0 : Clo_Wft_ID4 = 0
            Clo_Reed1 = 0 : Clo_Reed2 = 0 : Clo_Reed3 = 0 : Clo_Reed4 = 0
            Clo_Width1 = 0 : Clo_Width2 = 0 : Clo_Width3 = 0 : Clo_Width4 = 0

            Da4 = New SqlClient.SqlDataAdapter("select * from Beam_Knotting_Head a Where a.Loom_IdNo = " & Str(Val(Lm_ID)) & " and a.Beam_Knotting_Code = '" & Trim(lbl_KnotCode.Text) & "'", con)
            Dt4 = New DataTable
            Da4.Fill(Dt4)
            If Dt4.Rows.Count > 0 Then
                Clo_ID1 = Val(Dt4.Rows(0).Item("Cloth_Idno1").ToString)
                Clo_ID2 = Val(Dt4.Rows(0).Item("Cloth_Idno2").ToString)
                Clo_ID3 = Val(Dt4.Rows(0).Item("Cloth_Idno3").ToString)
                Clo_ID4 = Val(Dt4.Rows(0).Item("Cloth_Idno4").ToString)
            End If
            Dt4.Clear()
            Dt4.Dispose()
            Da4.Dispose()

            Clo_Wrp_ID1 = Val(Common_Procedures.get_FieldValue(con, "cloth_head", "Cloth_WarpCount_IdNo", "(cloth_idno = " & Str(Val(Clo_ID1)) & ")"))
            Clo_Wrp_ID2 = Val(Common_Procedures.get_FieldValue(con, "cloth_head", "Cloth_WarpCount_IdNo", "(cloth_idno = " & Str(Val(Clo_ID2)) & ")"))
            Clo_Wrp_ID3 = Val(Common_Procedures.get_FieldValue(con, "cloth_head", "Cloth_WarpCount_IdNo", "(cloth_idno = " & Str(Val(Clo_ID3)) & ")"))
            Clo_Wrp_ID4 = Val(Common_Procedures.get_FieldValue(con, "cloth_head", "Cloth_WarpCount_IdNo", "(cloth_idno = " & Str(Val(Clo_ID4)) & ")"))

            Clo_Wft_ID1 = Val(Common_Procedures.get_FieldValue(con, "cloth_head", "Cloth_WeftCount_IdNo", "(cloth_idno = " & Str(Val(Clo_ID1)) & ")"))
            Clo_Wft_ID2 = Val(Common_Procedures.get_FieldValue(con, "cloth_head", "Cloth_WeftCount_IdNo", "(cloth_idno = " & Str(Val(Clo_ID2)) & ")"))
            Clo_Wft_ID3 = Val(Common_Procedures.get_FieldValue(con, "cloth_head", "Cloth_WeftCount_IdNo", "(cloth_idno = " & Str(Val(Clo_ID3)) & ")"))
            Clo_Wft_ID4 = Val(Common_Procedures.get_FieldValue(con, "cloth_head", "Cloth_WeftCount_IdNo", "(cloth_idno = " & Str(Val(Clo_ID4)) & ")"))

            Clo_Reed1 = Val(Common_Procedures.get_FieldValue(con, "cloth_head", "Cloth_Reed", "(cloth_idno = " & Str(Val(Clo_ID1)) & ")"))
            Clo_Reed2 = Val(Common_Procedures.get_FieldValue(con, "cloth_head", "Cloth_Reed", "(cloth_idno = " & Str(Val(Clo_ID2)) & ")"))
            Clo_Reed3 = Val(Common_Procedures.get_FieldValue(con, "cloth_head", "Cloth_Reed", "(cloth_idno = " & Str(Val(Clo_ID3)) & ")"))
            Clo_Reed4 = Val(Common_Procedures.get_FieldValue(con, "cloth_head", "Cloth_Reed", "(cloth_idno = " & Str(Val(Clo_ID4)) & ")"))

            Clo_Width1 = Val(Common_Procedures.get_FieldValue(con, "cloth_head", "Cloth_Width", "(cloth_idno = " & Str(Val(Clo_ID1)) & ")"))
            Clo_Width2 = Val(Common_Procedures.get_FieldValue(con, "cloth_head", "Cloth_Width", "(cloth_idno = " & Str(Val(Clo_ID2)) & ")"))
            Clo_Width3 = Val(Common_Procedures.get_FieldValue(con, "cloth_head", "Cloth_Width", "(cloth_idno = " & Str(Val(Clo_ID3)) & ")"))
            Clo_Width4 = Val(Common_Procedures.get_FieldValue(con, "cloth_head", "Cloth_Width", "(cloth_idno = " & Str(Val(Clo_ID4)) & ")"))

            Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Cloth_Head", "Cloth_Name", "(Cloth_idno = 0 or Cloth_idno = " & Str(Val(Clo_ID1)) & " or Cloth_idno = " & Str(Val(Clo_ID2)) & " or Cloth_idno = " & Str(Val(Clo_ID3)) & "  or ( Cloth_WarpCount_IdNo = " & Str(Val(Clo_Wrp_ID1)) & " and Cloth_WeftCount_IdNo = " & Str(Val(Clo_Wft_ID1)) & " and Cloth_Reed = " & Str(Val(Clo_Reed1)) & " and Cloth_Width = " & Str(Val(Clo_Width1)) & " )  or ( Cloth_WarpCount_IdNo = " & Str(Val(Clo_Wrp_ID2)) & " and Cloth_WeftCount_IdNo = " & Str(Val(Clo_Wft_ID2)) & " and Cloth_Reed = " & Str(Val(Clo_Reed2)) & " and Cloth_Width = " & Str(Val(Clo_Width2)) & " )  or ( Cloth_WarpCount_IdNo = " & Str(Val(Clo_Wrp_ID3)) & " and Cloth_WeftCount_IdNo = " & Str(Val(Clo_Wft_ID3)) & " and Cloth_Reed = " & Str(Val(Clo_Reed3)) & " and Cloth_Width = " & Str(Val(Clo_Width3)) & " ) or ( Cloth_WarpCount_IdNo = " & Str(Val(Clo_Wrp_ID4)) & " and Cloth_WeftCount_IdNo = " & Str(Val(Clo_Wft_ID4)) & " and Cloth_Reed = " & Str(Val(Clo_Reed4)) & " and Cloth_Width = " & Str(Val(Clo_Width4)) & " ) )", "(Cloth_idno = 0)")

            'Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Cloth_Head", "Cloth_Name", "(Cloth_idno = 0 or Cloth_idno = " & Str(Val(Clo_ID1)) & " or Cloth_idno = " & Str(Val(Clo_ID2)) & " or Cloth_idno = " & Str(Val(Clo_ID3)) & ")", "(Cloth_idno = 0)")
            cbo_ClothName.Tag = cbo_ClothName.Text

        End If


    End Sub

    Private Sub cbo_ClothName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_ClothName.KeyDown
        Dim Da4 As New SqlClient.SqlDataAdapter
        Dim Dt4 As New DataTable
        Dim Lm_ID As Integer = 0
        Dim Clo_ID1 As Integer = 0
        Dim Clo_ID2 As Integer = 0
        Dim Clo_ID3 As Integer = 0
        Dim Clo_ID4 As Integer = 0
        Dim Clo_Wrp_ID1 As Integer = 0, Clo_Wrp_ID2 As Integer = 0, Clo_Wrp_ID3 As Integer = 0, Clo_Wrp_ID4 As Integer = 0
        Dim Clo_Wft_ID1 As Integer = 0, Clo_Wft_ID2 As Integer = 0, Clo_Wft_ID3 As Integer = 0, Clo_Wft_ID4 As Integer = 0
        Dim Clo_Reed1 As Integer = 0, Clo_Reed2 As Integer = 0, Clo_Reed3 As Integer = 0, Clo_Reed4 As Integer = 0
        Dim Clo_Width1 As Integer = 0, Clo_Width2 As Integer = 0, Clo_Width3 As Integer = 0, Clo_Width4 As Integer = 0

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1037" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1474" Then '---- Prakash Textiles (Somanur)
            Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_ClothName, Nothing, Nothing, "Cloth_Head", "Cloth_Name", "", "(Cloth_IdNo = 0)")

            If (e.KeyCode = 38 And cbo_ClothName.DroppedDown = False) Or (e.Control = True And e.KeyCode = 38) Then
                If txt_BarCode.Visible = True And txt_BarCode.Enabled = True Then
                    txt_BarCode.Focus()
                Else
                    cbo_LoomNo.Focus()
                End If

            ElseIf (e.KeyValue = 40 And cbo_ClothName.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
                If txt_DoffMtrs.Enabled And txt_DoffMtrs.Visible Then
                    txt_DoffMtrs.Focus()

                ElseIf dgv_Details.Rows.Count > 0 Then
                    dgv_Details.Focus()
                    If dgv_Details.Columns(dgvCol_Details.Pcsno).ReadOnly = False Then
                        dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(dgvCol_Details.Pcsno)
                    Else
                        dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(dgvCol_Details.ClothType)
                    End If


                Else
                    txt_Folding.Focus()

                End If

            End If

        Else
            Lm_ID = Common_Procedures.Loom_NameToIdNo(con, cbo_LoomNo.Text)

            Clo_ID1 = 0 : Clo_ID2 = 0 : Clo_ID3 = 0 : Clo_ID4 = 0
            Clo_Wrp_ID1 = 0 : Clo_Wrp_ID2 = 0 : Clo_Wrp_ID3 = 0 : Clo_Wrp_ID4 = 0
            Clo_Wft_ID1 = 0 : Clo_Wft_ID2 = 0 : Clo_Wft_ID3 = 0 : Clo_Wft_ID4 = 0
            Clo_Reed1 = 0 : Clo_Reed2 = 0 : Clo_Reed3 = 0 : Clo_Reed4 = 0
            Clo_Width1 = 0 : Clo_Width2 = 0 : Clo_Width3 = 0 : Clo_Width4 = 0

            Da4 = New SqlClient.SqlDataAdapter("select * from Beam_Knotting_Head a Where a.Loom_IdNo = " & Str(Val(Lm_ID)) & " and a.Beam_Knotting_Code = '" & Trim(lbl_KnotCode.Text) & "'", con)
            Dt4 = New DataTable
            Da4.Fill(Dt4)
            If Dt4.Rows.Count > 0 Then
                Clo_ID1 = Val(Dt4.Rows(0).Item("Cloth_Idno1").ToString)
                Clo_ID2 = Val(Dt4.Rows(0).Item("Cloth_Idno2").ToString)
                Clo_ID3 = Val(Dt4.Rows(0).Item("Cloth_Idno3").ToString)
                Clo_ID4 = Val(Dt4.Rows(0).Item("Cloth_Idno4").ToString)
            End If
            Dt4.Clear()
            Dt4.Dispose()
            Da4.Dispose()

            Clo_Wrp_ID1 = Val(Common_Procedures.get_FieldValue(con, "cloth_head", "Cloth_WarpCount_IdNo", "(cloth_idno = " & Str(Val(Clo_ID1)) & ")"))
            Clo_Wrp_ID2 = Val(Common_Procedures.get_FieldValue(con, "cloth_head", "Cloth_WarpCount_IdNo", "(cloth_idno = " & Str(Val(Clo_ID2)) & ")"))
            Clo_Wrp_ID3 = Val(Common_Procedures.get_FieldValue(con, "cloth_head", "Cloth_WarpCount_IdNo", "(cloth_idno = " & Str(Val(Clo_ID3)) & ")"))
            Clo_Wrp_ID4 = Val(Common_Procedures.get_FieldValue(con, "cloth_head", "Cloth_WarpCount_IdNo", "(cloth_idno = " & Str(Val(Clo_ID4)) & ")"))

            Clo_Wft_ID1 = Val(Common_Procedures.get_FieldValue(con, "cloth_head", "Cloth_WeftCount_IdNo", "(cloth_idno = " & Str(Val(Clo_ID1)) & ")"))
            Clo_Wft_ID2 = Val(Common_Procedures.get_FieldValue(con, "cloth_head", "Cloth_WeftCount_IdNo", "(cloth_idno = " & Str(Val(Clo_ID2)) & ")"))
            Clo_Wft_ID3 = Val(Common_Procedures.get_FieldValue(con, "cloth_head", "Cloth_WeftCount_IdNo", "(cloth_idno = " & Str(Val(Clo_ID3)) & ")"))
            Clo_Wft_ID4 = Val(Common_Procedures.get_FieldValue(con, "cloth_head", "Cloth_WeftCount_IdNo", "(cloth_idno = " & Str(Val(Clo_ID4)) & ")"))

            Clo_Reed1 = Val(Common_Procedures.get_FieldValue(con, "cloth_head", "Cloth_Reed", "(cloth_idno = " & Str(Val(Clo_ID1)) & ")"))
            Clo_Reed2 = Val(Common_Procedures.get_FieldValue(con, "cloth_head", "Cloth_Reed", "(cloth_idno = " & Str(Val(Clo_ID2)) & ")"))
            Clo_Reed3 = Val(Common_Procedures.get_FieldValue(con, "cloth_head", "Cloth_Reed", "(cloth_idno = " & Str(Val(Clo_ID3)) & ")"))
            Clo_Reed4 = Val(Common_Procedures.get_FieldValue(con, "cloth_head", "Cloth_Reed", "(cloth_idno = " & Str(Val(Clo_ID4)) & ")"))

            Clo_Width1 = Val(Common_Procedures.get_FieldValue(con, "cloth_head", "Cloth_Width", "(cloth_idno = " & Str(Val(Clo_ID1)) & ")"))
            Clo_Width2 = Val(Common_Procedures.get_FieldValue(con, "cloth_head", "Cloth_Width", "(cloth_idno = " & Str(Val(Clo_ID2)) & ")"))
            Clo_Width3 = Val(Common_Procedures.get_FieldValue(con, "cloth_head", "Cloth_Width", "(cloth_idno = " & Str(Val(Clo_ID3)) & ")"))
            Clo_Width4 = Val(Common_Procedures.get_FieldValue(con, "cloth_head", "Cloth_Width", "(cloth_idno = " & Str(Val(Clo_ID4)) & ")"))

            vcbo_KeyDwnVal = e.KeyValue

            Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_ClothName, Nothing, Nothing, "Cloth_Head", "Cloth_Name", "(Cloth_idno = 0 or Cloth_idno = " & Str(Val(Clo_ID1)) & " or Cloth_idno = " & Str(Val(Clo_ID2)) & " or Cloth_idno = " & Str(Val(Clo_ID3)) & "  or ( Cloth_WarpCount_IdNo = " & Str(Val(Clo_Wrp_ID1)) & " and Cloth_WeftCount_IdNo = " & Str(Val(Clo_Wft_ID1)) & " and Cloth_Reed = " & Str(Val(Clo_Reed1)) & " and Cloth_Width = " & Str(Val(Clo_Width1)) & " )  or ( Cloth_WarpCount_IdNo = " & Str(Val(Clo_Wrp_ID2)) & " and Cloth_WeftCount_IdNo = " & Str(Val(Clo_Wft_ID2)) & " and Cloth_Reed = " & Str(Val(Clo_Reed2)) & " and Cloth_Width = " & Str(Val(Clo_Width2)) & " )  or ( Cloth_WarpCount_IdNo = " & Str(Val(Clo_Wrp_ID3)) & " and Cloth_WeftCount_IdNo = " & Str(Val(Clo_Wft_ID3)) & " and Cloth_Reed = " & Str(Val(Clo_Reed3)) & " and Cloth_Width = " & Str(Val(Clo_Width3)) & " ) or ( Cloth_WarpCount_IdNo = " & Str(Val(Clo_Wrp_ID4)) & " and Cloth_WeftCount_IdNo = " & Str(Val(Clo_Wft_ID4)) & " and Cloth_Reed = " & Str(Val(Clo_Reed4)) & " and Cloth_Width = " & Str(Val(Clo_Width4)) & " ) )", "(Cloth_idno = 0)")
            If (e.KeyCode = 38 And cbo_ClothName.DroppedDown = False) Or (e.Control = True And e.KeyCode = 38) Then
                If txt_BarCode.Visible = True And txt_BarCode.Enabled = True Then
                    txt_BarCode.Focus()
                Else
                    cbo_LoomNo.Focus()
                End If

            ElseIf (e.KeyValue = 40 And cbo_ClothName.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
                If dgv_Details.Rows.Count > 0 Then
                    dgv_Details.Focus()
                    If dgv_Details.Columns(dgvCol_Details.Pcsno).ReadOnly = False Then
                        dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(dgvCol_Details.Pcsno)
                    Else
                        dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(dgvCol_Details.ClothType)
                    End If


                Else
                    If txt_DoffMtrs.Enabled And txt_DoffMtrs.Visible Then
                        txt_DoffMtrs.Focus()
                    Else
                        txt_Folding.Focus()
                    End If

                End If

            End If

        End If

        'Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_ClothName, cbo_LoomNo, txt_DoffMtrs, "Cloth_Head", "Cloth_Name", "(Cloth_idno = 0 or Cloth_idno = " & Str(Val(Clo_ID1)) & " or Cloth_idno = " & Str(Val(Clo_ID2)) & " or Cloth_idno = " & Str(Val(Clo_ID3)) & ")", "(Cloth_idno = 0)")
    End Sub

    Private Sub cbo_ClothName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_ClothName.KeyPress
        Dim Da4 As New SqlClient.SqlDataAdapter
        Dim Dt4 As New DataTable
        Dim Lm_ID As Integer = 0
        Dim Clo_ID1 As Integer = 0
        Dim Clo_ID2 As Integer = 0
        Dim Clo_ID3 As Integer = 0
        Dim Clo_ID4 As Integer = 0
        Dim Clo_Wrp_ID1 As Integer = 0, Clo_Wrp_ID2 As Integer = 0, Clo_Wrp_ID3 As Integer = 0, Clo_Wrp_ID4 As Integer = 0
        Dim Clo_Wft_ID1 As Integer = 0, Clo_Wft_ID2 As Integer = 0, Clo_Wft_ID3 As Integer = 0, Clo_Wft_ID4 As Integer = 0
        Dim Clo_Reed1 As Integer = 0, Clo_Reed2 As Integer = 0, Clo_Reed3 As Integer = 0, Clo_Reed4 As Integer = 0
        Dim Clo_Width1 As Integer = 0, Clo_Width2 As Integer = 0, Clo_Width3 As Integer = 0, Clo_Width4 As Integer = 0

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1037" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1474" Then '---- Prakash Textiles (Somanur)

            Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_ClothName, Nothing, "Cloth_Head", "Cloth_Name", "", "(Cloth_IdNo = 0)")

            If Asc(e.KeyChar) = 13 Then

                If txt_DoffMtrs.Enabled And txt_DoffMtrs.Visible Then
                    txt_DoffMtrs.Focus()

                ElseIf dgv_Details.Rows.Count > 0 Then

                    dgv_Details.Focus()
                    If dgv_Details.Columns(dgvCol_Details.Pcsno).ReadOnly = False Then
                        dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(dgvCol_Details.Pcsno)
                    Else
                        dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(dgvCol_Details.ClothType)
                    End If


                Else
                    txt_Folding.Focus()

                End If

            End If


        Else


            Lm_ID = Common_Procedures.Loom_NameToIdNo(con, cbo_LoomNo.Text)

            Clo_ID1 = 0 : Clo_ID2 = 0 : Clo_ID3 = 0 : Clo_ID4 = 0
            Clo_Wrp_ID1 = 0 : Clo_Wrp_ID2 = 0 : Clo_Wrp_ID3 = 0 : Clo_Wrp_ID4 = 0
            Clo_Wft_ID1 = 0 : Clo_Wft_ID2 = 0 : Clo_Wft_ID3 = 0 : Clo_Wft_ID4 = 0
            Clo_Reed1 = 0 : Clo_Reed2 = 0 : Clo_Reed3 = 0 : Clo_Reed4 = 0
            Clo_Width1 = 0 : Clo_Width2 = 0 : Clo_Width3 = 0 : Clo_Width4 = 0

            Da4 = New SqlClient.SqlDataAdapter("select * from Beam_Knotting_Head a Where a.Loom_IdNo = " & Str(Val(Lm_ID)) & " and a.Beam_Knotting_Code = '" & Trim(lbl_KnotCode.Text) & "'", con)
            Dt4 = New DataTable
            Da4.Fill(Dt4)
            If Dt4.Rows.Count > 0 Then
                Clo_ID1 = Val(Dt4.Rows(0).Item("Cloth_Idno1").ToString)
                Clo_ID2 = Val(Dt4.Rows(0).Item("Cloth_Idno2").ToString)
                Clo_ID3 = Val(Dt4.Rows(0).Item("Cloth_Idno3").ToString)
                Clo_ID4 = Val(Dt4.Rows(0).Item("Cloth_Idno4").ToString)
            End If
            Dt4.Clear()
            Dt4.Dispose()
            Da4.Dispose()

            Clo_Wrp_ID1 = Val(Common_Procedures.get_FieldValue(con, "cloth_head", "Cloth_WarpCount_IdNo", "(cloth_idno = " & Str(Val(Clo_ID1)) & ")"))
            Clo_Wrp_ID2 = Val(Common_Procedures.get_FieldValue(con, "cloth_head", "Cloth_WarpCount_IdNo", "(cloth_idno = " & Str(Val(Clo_ID2)) & ")"))
            Clo_Wrp_ID3 = Val(Common_Procedures.get_FieldValue(con, "cloth_head", "Cloth_WarpCount_IdNo", "(cloth_idno = " & Str(Val(Clo_ID3)) & ")"))
            Clo_Wrp_ID4 = Val(Common_Procedures.get_FieldValue(con, "cloth_head", "Cloth_WarpCount_IdNo", "(cloth_idno = " & Str(Val(Clo_ID4)) & ")"))

            Clo_Wft_ID1 = Val(Common_Procedures.get_FieldValue(con, "cloth_head", "Cloth_WeftCount_IdNo", "(cloth_idno = " & Str(Val(Clo_ID1)) & ")"))
            Clo_Wft_ID2 = Val(Common_Procedures.get_FieldValue(con, "cloth_head", "Cloth_WeftCount_IdNo", "(cloth_idno = " & Str(Val(Clo_ID2)) & ")"))
            Clo_Wft_ID3 = Val(Common_Procedures.get_FieldValue(con, "cloth_head", "Cloth_WeftCount_IdNo", "(cloth_idno = " & Str(Val(Clo_ID3)) & ")"))
            Clo_Wft_ID4 = Val(Common_Procedures.get_FieldValue(con, "cloth_head", "Cloth_WeftCount_IdNo", "(cloth_idno = " & Str(Val(Clo_ID4)) & ")"))

            Clo_Reed1 = Val(Common_Procedures.get_FieldValue(con, "cloth_head", "Cloth_Reed", "(cloth_idno = " & Str(Val(Clo_ID1)) & ")"))
            Clo_Reed2 = Val(Common_Procedures.get_FieldValue(con, "cloth_head", "Cloth_Reed", "(cloth_idno = " & Str(Val(Clo_ID2)) & ")"))
            Clo_Reed3 = Val(Common_Procedures.get_FieldValue(con, "cloth_head", "Cloth_Reed", "(cloth_idno = " & Str(Val(Clo_ID3)) & ")"))
            Clo_Reed4 = Val(Common_Procedures.get_FieldValue(con, "cloth_head", "Cloth_Reed", "(cloth_idno = " & Str(Val(Clo_ID4)) & ")"))

            Clo_Width1 = Val(Common_Procedures.get_FieldValue(con, "cloth_head", "Cloth_Width", "(cloth_idno = " & Str(Val(Clo_ID1)) & ")"))
            Clo_Width2 = Val(Common_Procedures.get_FieldValue(con, "cloth_head", "Cloth_Width", "(cloth_idno = " & Str(Val(Clo_ID2)) & ")"))
            Clo_Width3 = Val(Common_Procedures.get_FieldValue(con, "cloth_head", "Cloth_Width", "(cloth_idno = " & Str(Val(Clo_ID3)) & ")"))
            Clo_Width4 = Val(Common_Procedures.get_FieldValue(con, "cloth_head", "Cloth_Width", "(cloth_idno = " & Str(Val(Clo_ID4)) & ")"))

            If Asc(e.KeyChar) = 13 Then
                cbo_ClothName.Tag = "----------------------"
            End If

            Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_ClothName, Nothing, "Cloth_Head", "Cloth_Name", "(Cloth_idno = 0 or Cloth_idno = " & Str(Val(Clo_ID1)) & " or Cloth_idno = " & Str(Val(Clo_ID2)) & " or Cloth_idno = " & Str(Val(Clo_ID3)) & "  or ( Cloth_WarpCount_IdNo = " & Str(Val(Clo_Wrp_ID1)) & " and Cloth_WeftCount_IdNo = " & Str(Val(Clo_Wft_ID1)) & " and Cloth_Reed = " & Str(Val(Clo_Reed1)) & " and Cloth_Width = " & Str(Val(Clo_Width1)) & " )  or ( Cloth_WarpCount_IdNo = " & Str(Val(Clo_Wrp_ID2)) & " and Cloth_WeftCount_IdNo = " & Str(Val(Clo_Wft_ID2)) & " and Cloth_Reed = " & Str(Val(Clo_Reed2)) & " and Cloth_Width = " & Str(Val(Clo_Width2)) & " )  or ( Cloth_WarpCount_IdNo = " & Str(Val(Clo_Wrp_ID3)) & " and Cloth_WeftCount_IdNo = " & Str(Val(Clo_Wft_ID3)) & " and Cloth_Reed = " & Str(Val(Clo_Reed3)) & " and Cloth_Width = " & Str(Val(Clo_Width3)) & " )   or ( Cloth_WarpCount_IdNo = " & Str(Val(Clo_Wrp_ID4)) & " and Cloth_WeftCount_IdNo = " & Str(Val(Clo_Wft_ID4)) & " and Cloth_Reed = " & Str(Val(Clo_Reed4)) & " and Cloth_Width = " & Str(Val(Clo_Width4)) & " ) )", "(Cloth_idno = 0)")

            If Asc(e.KeyChar) = 13 Then
                If txt_DoffMtrs.Enabled And txt_DoffMtrs.Visible Then
                    txt_DoffMtrs.Focus()

                ElseIf dgv_Details.Rows.Count > 0 Then
                    dgv_Details.Focus()
                    If dgv_Details.Columns(dgvCol_Details.Pcsno).ReadOnly = False Then
                        dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(dgvCol_Details.Pcsno)
                    Else
                        dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(dgvCol_Details.ClothType)
                    End If

                Else

                    txt_Folding.Focus()

                End If

            End If

        End If


        'Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_ClothName, txt_DoffMtrs, "Cloth_Head", "Cloth_Name", "(Cloth_idno = 0 or Cloth_idno = " & Str(Val(Clo_ID1)) & " or Cloth_idno = " & Str(Val(Clo_ID2)) & " or Cloth_idno = " & Str(Val(Clo_ID3)) & ")", "(Cloth_idno = 0)")

    End Sub

    Private Sub cbo_ClothName_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_ClothName.LostFocus
        Dim Da4 As New SqlClient.SqlDataAdapter
        Dim Dt4 As New DataTable
        Dim Clo_IdNo As Integer = 0
        Dim wftcnt_idno As Integer = 0
        Dim vMULTIends_STS As Integer = 0

        If Trim(UCase(cbo_ClothName.Tag)) <> Trim(UCase(cbo_ClothName.Text)) Then

            Clo_IdNo = Common_Procedures.Cloth_NameToIdNo(con, cbo_ClothName.Text)

            wftcnt_idno = 0
            vMULTIends_STS = 0
            Da4 = New SqlClient.SqlDataAdapter("Select Cloth_WeftCount_IdNo, Multiple_WeftCount_Status from Cloth_Head Where Cloth_Idno = " & Str(Val(Clo_IdNo)), con)
            Dt4 = New DataTable
            Da4.Fill(Dt4)
            If Dt4.Rows.Count > 0 Then
                vMULTIends_STS = Val(Dt4.Rows(0).Item("Multiple_WeftCount_Status").ToString)
                wftcnt_idno = Val(Dt4.Rows(0).Item("Cloth_WeftCount_IdNo").ToString)
            End If
            Dt4.Clear()

            If vMULTIends_STS = 0 Then
                'wftcnt_idno = Val(Common_Procedures.get_FieldValue(con, "cloth_head", "Cloth_WeftCount_IdNo", "(cloth_idno = " & Str(Val(Clo_IdNo)) & ")"))
                lbl_WeftCount.Text = Common_Procedures.Count_IdNoToName(con, wftcnt_idno)
                btn_Show_WeftConsumption_Details.Visible = False

            Else
                lbl_WeftCount.Text = ""
                btn_Show_WeftConsumption_Details.Visible = True
                btn_Show_WeftConsumption_Details.BringToFront()

            End If

            ConsumedYarn_Calculation()

        End If

    End Sub

    Private Sub cbo_Filter_PartyName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Filter_PartyName.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", " (  Ledger_Type = 'GODOWN' or Ledger_Type = 'WEAVER' OR Ledger_Type = 'JOBWORKER')", "(Ledger_idno = 0)")
    End Sub

    Private Sub cbo_Filter_PartyName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Filter_PartyName.KeyDown

        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_PartyName, cbo_Filter_ClothName, txt_Filter_PieceNo, "Ledger_AlaisHead", "Ledger_DisplayName", " (  Ledger_Type = 'GODOWN' or Ledger_Type = 'WEAVER' OR Ledger_Type = 'JOBWORKER')", "(Ledger_idno = 0)")
    End Sub

    Private Sub cbo_Filter_PartyName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Filter_PartyName.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Filter_PartyName, txt_Filter_PieceNo, "Ledger_AlaisHead", "Ledger_DisplayName", " (  Ledger_Type = 'GODOWN' or Ledger_Type = 'WEAVER' OR Ledger_Type = 'JOBWORKER')", "(Ledger_idno = 0)")
    End Sub

    Private Sub btn_filtershow_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_filtershow.Click
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim n As Integer
        Dim Led_IdNo As Integer, Clt_IdNo As Integer, Lom_IdNo As Integer
        Dim Condt As String = ""
        Dim vNOOFPcs As Integer = 0
        Dim Doff_Mtr As Double = 0
        Dim Chk_Mtr As Double = 0
        Dim StCode As String = "", BmNo As String = ""

        Try

            Condt = ""
            Led_IdNo = 0
            Clt_IdNo = 0
            Lom_IdNo = 0

            If IsDate(dtp_FilterFrom_date.Value) = True And IsDate(dtp_FilterTo_date.Value) = True Then
                Condt = "a.Weaver_ClothReceipt_Date between '" & Trim(Format(dtp_FilterFrom_date.Value, "MM/dd/yyyy")) & "' and '" & Trim(Format(dtp_FilterTo_date.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_FilterFrom_date.Value) = True Then
                Condt = "a.Weaver_ClothReceipt_Date = '" & Trim(Format(dtp_FilterFrom_date.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_FilterTo_date.Value) = True Then
                Condt = "a. Weaver_ClothReceipt_Date= '" & Trim(Format(dtp_FilterTo_date.Value, "MM/dd/yyyy")) & "' "
            End If

            If Trim(cbo_Filter_PartyName.Text) <> "" Then
                Led_IdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Filter_PartyName.Text)
            End If

            If Val(Led_IdNo) <> 0 Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " (a.Ledger_Idno = " & Str(Val(Led_IdNo)) & ")"
            End If

            If Trim(cbo_Filter_ClothName.Text) <> "" Then
                Clt_IdNo = Common_Procedures.Cloth_NameToIdNo(con, cbo_Filter_ClothName.Text)
            End If
            If Val(Clt_IdNo) <> 0 Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " (a.Cloth_Idno = " & Str(Val(Clt_IdNo)) & ")"
            End If

            Lom_IdNo = 0
            If Trim(cbo_Filter_LoomNo.Text) <> "" Then
                Lom_IdNo = Common_Procedures.Loom_NameToIdNo(con, cbo_Filter_LoomNo.Text)
            End If
            If Val(Lom_IdNo) <> 0 Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " (a.Loom_Idno = " & Str(Val(Lom_IdNo)) & ")"
            End If

            StCode = "" : BmNo = ""
            If Trim(cbo_Filter_BeamNo.Text) <> "" Then
                da = New SqlClient.SqlDataAdapter("select * from Stock_SizedPavu_Processing_Details where BeamNo_SetCode_forSelection = '" & Trim(cbo_Filter_BeamNo.Text) & "'", con)
                dt2 = New DataTable
                da.Fill(dt2)
                If dt2.Rows.Count > 0 Then
                    StCode = dt2.Rows(0).Item("set_code").ToString
                    BmNo = dt2.Rows(0).Item("beam_no").ToString
                End If

                If Trim(StCode) <> "" And Trim(BmNo) <> "" Then
                    Condt = Trim(Condt) & IIf(Trim(Condt) <> "", " and ", "") & "  ( (a.Set_Code1 = '" & Trim(StCode) & "' and a.Beam_No1 = '" & Trim(BmNo) & "') or (a.Set_Code2 = '" & Trim(StCode) & "' and a.Beam_No2 = '" & Trim(BmNo) & "') ) "
                End If

                'Condt = Trim(Condt) & IIf(Trim(Condt) <> "", " and ", "") & " tSPP.BeamNo_SetCode_forSelection = '" & Trim(cbo_Filter_BeamNo.Text) & "'"
                'Join1 = " LEFT OUTER JOIN Stock_SizedPavu_Processing_Details tSPP ON tSPP.BeamNo_SetCode_forSelection = '" & Trim(cbo_Filter_BeamNo.Text) & "' and ( (tSPP.Set_Code = a.Set_Code1 and tSPP.Beam_No = a.Beam_No1) or (tSPP.Set_Code = a.Set_Code2 and tSPP.Beam_No = a.Beam_No2) ) "

            End If

            If Trim(txt_Filter_PieceNo.Text) <> "" Then
                Condt = Trim(Condt) & IIf(Trim(Condt) <> "", " and ", "") & " ( a.Weaver_ClothReceipt_Code IN (select sq1.lot_code from Weaver_ClothReceipt_Piece_Details sq1 where sq1.piece_no = '" & Trim(txt_Filter_PieceNo.Text) & "' ) ) "
            End If


            da = New SqlClient.SqlDataAdapter("select a.*, b.Ledger_Name , c.Cloth_Name,  d.Loom_Name from Weaver_Cloth_Receipt_Head a INNER JOIN Ledger_Head b ON a.Ledger_IdNo = b.Ledger_IdNo INNER JOIN Cloth_Head c ON a.Cloth_IdNo = c.Cloth_IdNo  LEFT OUTER JOIN Loom_Head d ON a.Loom_IdNo = d.Loom_IdNo  Where a.company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Weaver_ClothReceipt_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "' " & IIf(Trim(Condt) <> "", " and ", "") & Condt & " Order by a.for_orderby, a.Weaver_ClothReceipt_No", con)
            dt2 = New DataTable
            da.Fill(dt2)

            dgv_filter.Rows.Clear()

            vNOOFPcs = 0
            Chk_Mtr = 0
            Doff_Mtr = 0

            If dt2.Rows.Count > 0 Then

                For i = 0 To dt2.Rows.Count - 1

                    n = dgv_filter.Rows.Add()

                    dgv_filter.Rows(n).Cells(0).Value = dt2.Rows(i).Item("Weaver_ClothReceipt_No").ToString
                    dgv_filter.Rows(n).Cells(1).Value = Format(Convert.ToDateTime(dt2.Rows(i).Item("Weaver_ClothReceipt_Date").ToString), "dd-MM-yyyy")
                    dgv_filter.Rows(n).Cells(2).Value = dt2.Rows(i).Item("Ledger_Name").ToString
                    dgv_filter.Rows(n).Cells(3).Value = dt2.Rows(i).Item("Cloth_Name").ToString
                    dgv_filter.Rows(n).Cells(4).Value = dt2.Rows(i).Item("Loom_Name").ToString
                    dgv_filter.Rows(n).Cells(5).Value = dt2.Rows(i).Item("Beam_No1").ToString
                    dgv_filter.Rows(n).Cells(6).Value = dt2.Rows(i).Item("Beam_No2").ToString
                    dgv_filter.Rows(n).Cells(7).Value = Format(Val(dt2.Rows(i).Item("ReceiptMeters_Receipt").ToString), "########0.00")
                    dgv_filter.Rows(n).Cells(8).Value = Format(Val(dt2.Rows(i).Item("Type1_Checking_Meters").ToString) + Val(dt2.Rows(i).Item("Type2_Checking_Meters").ToString) + Val(dt2.Rows(i).Item("Type3_Checking_Meters").ToString) + Val(dt2.Rows(i).Item("Type4_Checking_Meters").ToString) + Val(dt2.Rows(i).Item("Type5_Checking_Meters").ToString), "########0.00")
                    If Val(dgv_filter.Rows(n).Cells(8).Value) = 0 Then
                        dgv_filter.Rows(n).Cells(8).Value = ""
                    End If

                    vNOOFPcs = vNOOFPcs + 1
                    Chk_Mtr = Chk_Mtr + (Val(dt2.Rows(i).Item("Type1_Checking_Meters").ToString) + Val(dt2.Rows(i).Item("Type2_Checking_Meters").ToString) + Val(dt2.Rows(i).Item("Type3_Checking_Meters").ToString) + Val(dt2.Rows(i).Item("Type4_Checking_Meters").ToString) + Val(dt2.Rows(i).Item("Type5_Checking_Meters").ToString))
                    Doff_Mtr = Doff_Mtr + Format(Val(dt2.Rows(i).Item("ReceiptMeters_Receipt").ToString), "########0.00")

                Next i

            End If

            dgv_fILTER_Total.Rows.Add()

            dgv_fILTER_Total.Rows(0).Cells(2).Value = "TOTAL"
            dgv_fILTER_Total.Rows(0).Cells(0).Value = vNOOFPcs
            dgv_fILTER_Total.Rows(0).Cells(7).Value = Format(Val(Doff_Mtr), "########0.00")
            dgv_fILTER_Total.Rows(0).Cells(8).Value = Format(Val(Chk_Mtr), "########0.00")

            dt2.Clear()
            dt2.Dispose()
            da.Dispose()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT FILTER...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

        If dgv_filter.Visible And dgv_filter.Enabled Then dgv_filter.Focus()

    End Sub

    Private Sub cbo_Filter_ClothName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Filter_ClothName.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Cloth_Head", "Cloth_Name", "", "(Cloth_idno = 0)")
    End Sub

    Private Sub cbo_Filter_ClothName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Filter_ClothName.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_ClothName, dtp_FilterTo_date, cbo_Filter_PartyName, "Cloth_Head", "Cloth_Name", "", "(Cloth_idno = 0)")
    End Sub

    Private Sub cbo_Filter_ClothName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Filter_ClothName.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Filter_ClothName, cbo_Filter_PartyName, "Cloth_Head", "Cloth_Name", "", "(Cloth_idno = 0)")
    End Sub

    Private Sub cbo_Filter_LoomNo_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Filter_LoomNo.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Loom_Head", "Loom_Name", "", "(Loom_IdNo = 0 )")
    End Sub
    Private Sub cbo_Filter_LoomNo_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Filter_LoomNo.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_LoomNo, txt_Filter_PieceNo, cbo_Filter_BeamNo, "Loom_Head", "Loom_Name", "", "(Loom_IdNo = 0)")
    End Sub

    Private Sub cbo_Filter_LoomNo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Filter_LoomNo.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Filter_LoomNo, cbo_Filter_BeamNo, "Loom_Head", "Loom_Name", "", "(Loom_IdNo = 0 )")
        'If Asc(e.KeyChar) = 13 Then
        '    If Trim(UCase(cbo_LoomNo.Text)) = "" Then
        '        If MessageBox.Show("Do you want to select  :", "FOR  SELECTION...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = DialogResult.Yes Then
        '            btn_Selection_Click(sender, e)
        '        Else
        '            cbo_WidthType.Focus()
        '        End If

        '    Else
        '        cbo_WidthType.Focus()

        '    End If

        'End If

    End Sub


    Private Sub dgv_filter_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgv_filter.DoubleClick
        Open_FilterEntry()
    End Sub


    Private Sub btn_filtershow_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles btn_filtershow.KeyPress
        If Asc(e.KeyChar) = 13 Then
            cbo_Filter_PartyName.Focus()
        End If
    End Sub


    Private Sub btn_closefilter_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        pnl_back.Enabled = True
        pnl_filter.Visible = False
        Filter_Status = False
    End Sub

    Private Sub dgv_filter_CellDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_filter.CellDoubleClick
        Open_FilterEntry()
    End Sub

    Private Sub dgv_filter_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_filter.KeyDown
        If e.KeyCode = 13 Then
            Open_FilterEntry()
        End If
    End Sub

    Private Sub Open_FilterEntry()
        Try
            Dim movno As String = ""

            movno = Trim(dgv_filter.CurrentRow.Cells(0).Value)

            If Val(movno) <> 0 Then
                Filter_Status = True
                move_record(movno)
                pnl_back.Enabled = True
                pnl_filter.Visible = False
            End If

        Catch ex As NullReferenceException
            '---MessageBox.Show(ex.Message, "ERROR WHILE DETAILS CELL CHANGE....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Catch ex As ObjectDisposedException
            '---MessageBox.Show(ex.Message, "ERROR WHILE DETAILS CELL CHANGE....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "ERROR WHILE OPEN FILTER....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try


    End Sub



    Private Sub cbo_ClothName_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_ClothName.KeyUp
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

    Private Sub cbo_LoomNo_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_LoomNo.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Loom_Head", "Loom_Name", "", "(Loom_IdNo = 0 )")
    End Sub

    Private Sub cbo_LoomNo_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_LoomNo.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_LoomNo, msk_Date, Nothing, "Loom_Head", "Loom_Name", "", "(Loom_IdNo = 0)")
        If e.KeyCode = 40 And cbo_LoomNo.DroppedDown = False Or (e.Control = True And e.KeyCode = 40) Then
            If txt_BarCode.Visible = True And txt_BarCode.Enabled = True Then
                txt_BarCode.Focus()
            Else
                'cbo_ClothName.Focus()
                cbo_Pcs_LastPiece_Status.Focus()
            End If
        End If
    End Sub

    Private Sub cbo_LoomNo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_LoomNo.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_LoomNo, Nothing, "Loom_Head", "Loom_Name", "", "(Loom_IdNo = 0 )")
        If Asc(e.KeyChar) = 13 Then
            If Trim(cbo_LoomNo.Text) <> "" And (Trim(UCase(cbo_LoomNo.Text)) <> Trim(UCase(cbo_LoomNo.Tag)) Or Trim(lbl_KnotCode.Text) = "") Then
                btn_Selection_Click(sender, e)
            End If
            If txt_BarCode.Visible = True And txt_BarCode.Enabled = True Then
                txt_BarCode.Focus()
            Else
                '    cbo_ClothName.Focus()
                cbo_Pcs_LastPiece_Status.Focus()
            End If
        End If
    End Sub

    Private Sub cbo_LoomNo_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_LoomNo.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Dim f As New LoomNo_Creation
            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_LoomNo.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""
            f.MdiParent = MDIParent1
            f.Show()
        End If
    End Sub

    Private Sub btn_save_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_save.Click
        save_record()
    End Sub

    Private Sub btn_close_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_close.Click
        Me.Close()
    End Sub

    Private Sub cbo_Count_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs)
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Dim f As New Count_Creation
            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = lbl_WeftCount.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""
            f.MdiParent = MDIParent1
            f.Show()
        End If
    End Sub

    Private Sub txt_DoffMtrs_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_DoffMtrs.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
        If Asc(e.KeyChar) = 13 Then
            'If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
            '    save_record()
            'Else
            '    dtp_Date.Focus()
            'End If
            dgv_Details.Focus()
            If dgv_Details.Columns(dgvCol_Details.Pcsno).ReadOnly = False Then
                dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(dgvCol_Details.Pcsno)
            Else
                dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(dgvCol_Details.ClothType)
            End If

        End If
    End Sub

    Private Sub txt_CrimpPerc_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_CrimpPerc.KeyDown
        If e.KeyValue = 38 Then SendKeys.Send("+{TAB}")
        If e.KeyValue = 40 Then SendKeys.Send("{TAB}")
    End Sub

    Private Sub txt_CrimpPerc_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_CrimpPerc.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
        If Asc(e.KeyChar) = 13 Then
            'If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
            '    save_record()
            'Else
            '    msk_Date.Focus()
            'End If
            dgv_Details.Focus()
            dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(dgvCol_Details.ClothType)
        End If
    End Sub

    Private Sub ConsumedPavu_Calculation()
        Dim CloID As Integer
        Dim ConsPavu As Single
        Dim LmID As Integer
        Dim NoofBeams As Integer = 0
        Dim vTot_ChkMtrs As String = 0, vTot_ChkWGT As String = 0
        Dim vPVUSTK_IN As String = ""
        Dim Da4 As New SqlClient.SqlDataAdapter
        Dim Dt4 As New DataTable
        Dim vEDSCNTID1 As Integer = 0
        Dim vEDSCNTID2 As Integer = 0
        Dim vEDSCNT1_CONSPERC As String = 0
        Dim vEDSCNT2_CONSPERC As String = 0
        Dim vMULTIends_STS As String = 0

        CloID = Common_Procedures.Cloth_NameToIdNo(con, cbo_ClothName.Text)
        LmID = Common_Procedures.Loom_NameToIdNo(con, cbo_LoomNo.Text)

        vTot_ChkMtrs = 0
        vTot_ChkWGT = 0
        If dgv_Details_Total.RowCount > 0 Then
            vTot_ChkMtrs = Val(dgv_Details_Total.Rows(0).Cells(dgvCol_Details.Meters).Value)
            vTot_ChkWGT = Val(dgv_Details_Total.Rows(0).Cells(dgvCol_Details.Weight).Value)
        End If

        vEDSCNTID1 = 0 : vEDSCNTID2 = 0
        vEDSCNT1_CONSPERC = 0 : vEDSCNT2_CONSPERC = 0
        vPVUSTK_IN = "METER"
        vMULTIends_STS = 0

        Da4 = New SqlClient.SqlDataAdapter("Select Multiple_EndsCount_Status, Pavu_Consumption_In_Meter_Weight from Cloth_Head Where Cloth_Idno = " & Str(Val(CloID)), con)
        Dt4 = New DataTable
        Da4.Fill(Dt4)
        If Dt4.Rows.Count > 0 Then
            vMULTIends_STS = Val(Dt4.Rows(0).Item("Multiple_EndsCount_Status").ToString)
            vPVUSTK_IN = Dt4.Rows(0).Item("Pavu_Consumption_In_Meter_Weight").ToString
        End If
        Dt4.Clear()

        If Val(vMULTIends_STS) = 1 And lbl_EndsCount_Beam1.Visible = True And lbl_EndsCount_Beam2.Visible = True Then

            If Trim(lbl_EndsCount_Beam1.Text) <> "" Then

                vEDSCNTID1 = Common_Procedures.EndsCount_NameToIdNo(con, lbl_EndsCount_Beam1.Text)

                vEDSCNT1_CONSPERC = 0
                Da4 = New SqlClient.SqlDataAdapter("Select * from Cloth_EndsCount_Consumption_Details Where Cloth_Idno = " & Str(Val(CloID)) & " and EndsCount_IdNo = " & Str(Val(vEDSCNTID1)), con)
                Dt4 = New DataTable
                Da4.Fill(Dt4)
                If Dt4.Rows.Count > 0 Then
                    vEDSCNT1_CONSPERC = Dt4.Rows(0).Item("Consumption_Perc").ToString
                End If
                Dt4.Clear()

            End If

            If Trim(lbl_EndsCount_Beam2.Text) <> "" Then

                vEDSCNTID2 = Common_Procedures.EndsCount_NameToIdNo(con, lbl_EndsCount_Beam2.Text)

                vEDSCNT2_CONSPERC = 0
                Da4 = New SqlClient.SqlDataAdapter("Select * from Cloth_EndsCount_Consumption_Details Where Cloth_Idno = " & Str(Val(CloID)) & " and EndsCount_IdNo = " & Str(Val(vEDSCNTID2)), con)
                Dt4 = New DataTable
                Da4.Fill(Dt4)
                If Dt4.Rows.Count > 0 Then
                    vEDSCNT2_CONSPERC = Dt4.Rows(0).Item("Consumption_Perc").ToString
                End If
                Dt4.Clear()

            End If

            If Val(vEDSCNT1_CONSPERC) = 0 And Val(vEDSCNT2_CONSPERC) = 0 Then

                If Trim(lbl_EndsCount_Beam1.Text) <> "" And Trim(lbl_EndsCount_Beam2.Text) <> "" Then
                    vEDSCNT1_CONSPERC = 50
                    vEDSCNT2_CONSPERC = 50
                ElseIf Trim(lbl_EndsCount_Beam1.Text) <> "" Then
                    vEDSCNT1_CONSPERC = 100
                    vEDSCNT2_CONSPERC = 0
                ElseIf Trim(lbl_EndsCount_Beam2.Text) <> "" Then
                    vEDSCNT1_CONSPERC = 0
                    vEDSCNT2_CONSPERC = 100
                End If

            End If

        End If

        NoofBeams = 0
        If Trim(lbl_BeamNo1.Text) <> "" And Trim(lbl_BeamNo2.Text) <> "" Then
            NoofBeams = 2
        Else
            NoofBeams = 1
        End If
        If Val(NoofBeams) = 0 Then NoofBeams = 1


        If lbl_ConsPavu_Beam1.Visible = True And lbl_ConsPavu_Beam2.Visible = True And (Val(vEDSCNT1_CONSPERC) <> 0 Or Val(vEDSCNT2_CONSPERC) <> 0) Then

            If Trim(UCase(vPVUSTK_IN)) = "WEIGHT" Then

                lbl_ConsPavu_Beam1.Text = Format(Val(vTot_ChkWGT) * Val(vEDSCNT1_CONSPERC) / 100, "##########0.000")
                lbl_ConsPavu_Beam2.Text = Format(Val(vTot_ChkWGT) * Val(vEDSCNT2_CONSPERC) / 100, "##########0.000")
                lbl_ConsPavu.Text = Format(Val(lbl_ConsPavu_Beam1.Text) + Val(lbl_ConsPavu_Beam2.Text), "##########0.000")
                lbl_BeamConsPavu.Text = Format(Val(lbl_ConsPavu.Text) / NoofBeams, "#########0.00")

            Else

                GoTo GOTOLOOP1

            End If

        Else

GOTOLOOP1:
            ConsPavu = Common_Procedures.get_Pavu_Consumption(con, CloID, LmID, Val(vTot_ChkMtrs), Trim(lbl_WidthType.Text), , Val(txt_CrimpPerc.Text))

            lbl_ConsPavu.Text = Format(ConsPavu, "#########0.00")

            lbl_BeamConsPavu.Text = Format(Val(lbl_ConsPavu.Text) / NoofBeams, "#########0.00")

            If lbl_ConsPavu_Beam1.Visible = True And lbl_ConsPavu_Beam1.Visible = True And (Val(vEDSCNT1_CONSPERC) <> 0 Or Val(vEDSCNT2_CONSPERC) <> 0) Then

                lbl_ConsPavu_Beam1.Text = Format(Val(lbl_ConsPavu.Text) * Val(vEDSCNT1_CONSPERC) / 100, "##########0.00")
                lbl_ConsPavu_Beam2.Text = Format(Val(lbl_ConsPavu.Text) * Val(vEDSCNT2_CONSPERC) / 100, "##########0.00")

            Else

                lbl_ConsPavu_Beam1.Text = lbl_BeamConsPavu.Text
                If Trim(lbl_BeamNo1.Text) <> "" And Trim(lbl_BeamNo2.Text) <> "" Then
                    lbl_ConsPavu_Beam2.Text = lbl_BeamConsPavu.Text
                Else
                    lbl_ConsPavu_Beam2.Text = ""
                End If

            End If

        End If

    End Sub

    Private Sub ConsumedYarn_Calculation()
        Dim Da4 As New SqlClient.SqlDataAdapter
        Dim Dt4 As New DataTable
        Dim CloID As Integer
        Dim ConsYarn As String = 0, vTOTConsYarn As String = 0
        Dim vTot_ChkMtrs As String = 0
        Dim vTot_ChkWGT As String = 0
        Dim n, I As Integer
        Dim vWEFT_CONSFOR_MTRS_OR_WGT As String = 0

        CloID = Common_Procedures.Cloth_NameToIdNo(con, cbo_ClothName.Text)

        vTot_ChkMtrs = 0
        vTot_ChkWGT = 0
        If dgv_Details_Total.RowCount > 0 Then
            vTot_ChkMtrs = Val(dgv_Details_Total.Rows(0).Cells(dgvCol_Details.Meters).Value)
            vTot_ChkWGT = Val(dgv_Details_Total.Rows(0).Cells(dgvCol_Details.Weight).Value)
        End If

        dgv_Weft_Consumption_Details.Rows.Clear()

        If Common_Procedures.settings.Cloth_WeftConsumption_Multiple_WeftCount_Status = 1 Then '---- SOTEXPA QUALIDIS TEXTILE (SULUR)

            Dim vMULTIWEFTSTS As String = 0

            vMULTIWEFTSTS = Common_Procedures.get_FieldValue(con, "cloth_Head", "Multiple_WeftCount_Status", "(cloth_idno = " & Str(Val(CloID)) & ")")
            If Val(vMULTIWEFTSTS) = 1 Then

                vTOTConsYarn = 0
                Da4 = New SqlClient.SqlDataAdapter("Select a.*, b.count_name from Cloth_Additional_Weft_Details a INNER JOIN Count_Head b ON a.Count_IdNo = b.Count_IdNo Where Cloth_Idno = " & Str(Val(CloID)), con)
                Dt4 = New DataTable
                Da4.Fill(Dt4)
                If Dt4.Rows.Count > 0 Then
                    For I = 0 To Dt4.Rows.Count - 1

                        If Trim(UCase(Dt4.Rows(I).Item("ConsumptionFor_Meters_Weight").ToString)) = "WEIGHT" Then
                            vWEFT_CONSFOR_MTRS_OR_WGT = Val(vTot_ChkWGT)
                        Else
                            vWEFT_CONSFOR_MTRS_OR_WGT = Val(vTot_ChkMtrs)
                        End If

                        If Trim(UCase(Dt4.Rows(I).Item("Gram_Perc_Type").ToString)) = "%" Then
                            ConsYarn = Format(Val(vWEFT_CONSFOR_MTRS_OR_WGT) * Val(Dt4.Rows(I).Item("Consumption_Gram_Perc").ToString) / 100, "##########0.000")
                        Else
                            ConsYarn = Format(Val(vWEFT_CONSFOR_MTRS_OR_WGT) * Val(Dt4.Rows(I).Item("Consumption_Gram_Perc").ToString), "##########0.000")
                        End If

                        n = dgv_Weft_Consumption_Details.Rows.Add()
                        dgv_Weft_Consumption_Details.Rows(n).Cells(0).Value = Dt4.Rows(I).Item("count_name").ToString
                        dgv_Weft_Consumption_Details.Rows(n).Cells(1).Value = Dt4.Rows(I).Item("Gram_Perc_Type").ToString
                        dgv_Weft_Consumption_Details.Rows(n).Cells(2).Value = Val(Dt4.Rows(I).Item("Consumption_Gram_Perc").ToString)
                        dgv_Weft_Consumption_Details.Rows(n).Cells(3).Value = Format(Val(ConsYarn), "#########0.000")

                        vTOTConsYarn = Val(vTOTConsYarn) + Val(ConsYarn)

                    Next

                End If
                Dt4.Clear()

                lbl_ConsWeftYarn.Text = Format(Val(vTOTConsYarn), "#########0.000")

            Else
                GoTo GOTOLOOP1

            End If

        Else

GOTOLOOP1:
            ConsYarn = Common_Procedures.get_Weft_ConsumedYarn(con, CloID, Val(vTot_ChkMtrs))
            lbl_ConsWeftYarn.Text = Format(Val(ConsYarn), "#########0.000")

            dgv_Weft_Consumption_Details.Rows.Clear()

            If btn_Show_WeftConsumption_Details.Visible = True Then

                If Val(lbl_ConsWeftYarn.Text) <> 0 Then

                    Dim vCLO_WGTPERMTR As String = 0
                    vCLO_WGTPERMTR = Common_Procedures.get_FieldValue(con, "cloth_Head", "Weight_Meter_Weft", "(cloth_idno = " & Str(Val(CloID)) & ")")

                    n = dgv_Weft_Consumption_Details.Rows.Add()

                    dgv_Weft_Consumption_Details.Rows(n).Cells(0).Value = lbl_WeftCount.Text
                    dgv_Weft_Consumption_Details.Rows(n).Cells(1).Value = "GRAM"
                    dgv_Weft_Consumption_Details.Rows(n).Cells(2).Value = vCLO_WGTPERMTR
                    dgv_Weft_Consumption_Details.Rows(n).Cells(3).Value = lbl_ConsWeftYarn.Text

                End If

            End If

        End If

    End Sub

    Private Sub txt_DoffMtrs_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_DoffMtrs.TextChanged
        ConsumedPavu_Calculation()
        ConsumedYarn_Calculation()
    End Sub

    Private Sub txt_CrimpPerc_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_CrimpPerc.TextChanged
        ConsumedPavu_Calculation()
    End Sub

    Private Sub btn_Selection_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Selection.Click
        Dim Da1 As New SqlClient.SqlDataAdapter
        Dim Da2 As New SqlClient.SqlDataAdapter
        Dim Da3 As New SqlClient.SqlDataAdapter
        Dim Da4 As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim Dt2 As New DataTable
        Dim Dt3 As New DataTable
        Dim Dt4 As New DataTable
        Dim Lm_ID As Integer
        Dim NewCode As String = ""
        Dim vWFTCNT_NM As String = ""

        Lm_ID = Common_Procedures.Loom_NameToIdNo(con, cbo_LoomNo.Text)
        If Val(Lm_ID) = 0 Then
            MessageBox.Show("Invalid Loom NO", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If cbo_LoomNo.Enabled Then cbo_LoomNo.Focus()
            Exit Sub
        End If

        NewCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RollNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)


        If Trim(UCase(cbo_Pcs_LastPiece_Status.Text)) = "YES" Then
            btn_KnottingSelection_Click(sender, e)

        Else

            Da1 = New SqlClient.SqlDataAdapter("Select a.*, b.Ledger_Name, c.Cloth_Name, c.Stock_In, c.Multiple_WeftCount_Status, d.EndsCount_Name,j.Po_No, e.Count_Name, f.Loom_Name , tEC1.EndsCount_Name as EndsCountName_Beam1, tEC2.EndsCount_Name as EndsCountName_Beam2 from Weaver_Cloth_Receipt_Head a INNER JOIN Ledger_Head b ON a.Ledger_IdNo = b.Ledger_IdNo INNER JOIN Cloth_Head c ON a.Cloth_IdNo = c.Cloth_IdNo LEFT OUTER JOIN EndsCount_Head d ON a.EndsCount_IdNo = d.EndsCount_IdNo LEFT OUTER JOIN Count_Head e ON a.Count_IdNo = e.Count_IdNo LEFT OUTER JOIN Loom_Head f ON a.Loom_IdNo = f.Loom_IdNo LEFT OUTER JOIN JobWork_Pavu_Receipt_Details j ON a.Set_Code1  = j.Set_Code LEFT OUTER JOIN EndsCount_Head tEC1 ON a.EndsCount1_IdNo = tEC1.EndsCount_IdNo  LEFT OUTER JOIN EndsCount_Head tEC2 ON a.EndsCount2_IdNo = tEC2.EndsCount_IdNo Where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Weaver_ClothReceipt_Code = '" & Trim(NewCode) & "' and a.Loom_IdNo = " & Str(Val(Lm_ID)) & " and a.Receipt_Type = 'L'", con)
            Dt1 = New DataTable
            Da1.Fill(Dt1)
            If Dt1.Rows.Count > 0 Then

                lbl_PartyName.Text = Dt1.Rows(0).Item("Ledger_Name").ToString

                lbl_KnotCode.Text = Dt1.Rows(0).Item("Beam_Knotting_Code").ToString
                lbl_KnotNo.Text = Dt1.Rows(0).Item("Beam_Knotting_No").ToString

                cbo_ClothName.Text = Dt1.Rows(0).Item("Cloth_Name").ToString
                set_grid_Meters_ColumnHeading(Dt1.Rows(0).Item("Stock_In").ToString)
                lbl_WidthType.Text = Dt1.Rows(0).Item("Width_Type").ToString
                lbl_EndsCount.Text = Dt1.Rows(0).Item("EndsCount_Name").ToString
                lbl_EndsCount_Beam1.Text = Dt1.Rows(0).Item("EndsCountName_Beam1").ToString
                lbl_EndsCount_Beam2.Text = Dt1.Rows(0).Item("EndsCountName_Beam2").ToString

                lbl_ClothSales_OrderCode_forSelection.Text = Dt1.Rows(0).Item("ClothSales_OrderCode_forSelection").ToString

                If Val(Dt1.Rows(0).Item("Multiple_WeftCount_Status").ToString) = 1 Then
                    btn_Show_WeftConsumption_Details.Visible = True
                    btn_Show_WeftConsumption_Details.BringToFront()
                    lbl_WeftCount.Text = ""
                    get_Multiple_WeftYarn_Consumption_Count_Details(Val(Dt1.Rows(0).Item("Cloth_IdNo").ToString))

                Else
                    btn_Show_WeftConsumption_Details.Visible = False
                    lbl_WeftCount.Text = Dt1.Rows(0).Item("Count_Name").ToString
                    dgv_Weft_Consumption_Details.Rows.Clear()

                End If

                lbl_SetCode1.Text = Dt1.Rows(0).Item("Set_Code1").ToString
                lbl_SetNo1.Text = Dt1.Rows(0).Item("Set_No1").ToString
                lbl_BeamNo1.Text = Dt1.Rows(0).Item("Beam_No1").ToString
                lbl_BalMtrs1.Text = Dt1.Rows(0).Item("Balance_Meters1").ToString
                lbl_poNo.Text = Dt1.Rows(0).Item("po_no").ToString
                lbl_TotMtrs1.Text = ""
                Da2 = New SqlClient.SqlDataAdapter("Select * from Stock_SizedPavu_Processing_Details Where Set_Code = '" & Trim(lbl_SetCode1.Text) & "' and Beam_No = '" & Trim(lbl_BeamNo1.Text) & "'", con)
                Dt2 = New DataTable
                Da2.Fill(Dt2)
                If Dt2.Rows.Count > 0 Then
                    lbl_TotMtrs1.Text = Dt2.Rows(0).Item("Meters").ToString
                End If
                Dt2.Clear()

                lbl_SetCode2.Text = Dt1.Rows(0).Item("Set_Code2").ToString
                lbl_SetNo2.Text = Dt1.Rows(0).Item("Set_No2").ToString
                lbl_BeamNo2.Text = Dt1.Rows(0).Item("Beam_No2").ToString
                lbl_BalMtrs2.Text = Dt1.Rows(0).Item("Balance_Meters2").ToString
                lbl_TotMtrs2.Text = ""



                Da2 = New SqlClient.SqlDataAdapter("Select * from Stock_SizedPavu_Processing_Details Where Set_Code = '" & Trim(lbl_SetCode2.Text) & "' and Beam_No = '" & Trim(lbl_BeamNo2.Text) & "'", con)
                Dt2 = New DataTable
                Da2.Fill(Dt2)
                If Dt2.Rows.Count > 0 Then
                    lbl_TotMtrs2.Text = Dt2.Rows(0).Item("Meters").ToString
                End If
                Dt2.Clear()

                'txt_DoffMtrs.Text = Dt1.Rows(0).Item("Doff_Meters").ToString
                'txt_CrimpPerc.Text = Dt1.Rows(0).Item("Crimp_Percentage").ToString
                'lbl_ConsPavu.Text = Dt1.Rows(0).Item("ConsumedPavu_Receipt").ToString  
                'lbl_ConsWeftYarn.Text = Dt1.Rows(0).Item("ConsumedYarn_Receipt").ToString
                lbl_Weaver_Job_No.Text = Dt1.Rows(0).Item("Weaving_JobCode_forSelection").ToString

                set_grid_Meters_ColumnHeading(Dt1.Rows(0).Item("Stock_In").ToString)

            Else

                Da3 = New SqlClient.SqlDataAdapter("select a.*, b.Ledger_Name, c.Cloth_IdNo, c.Cloth_Name, c.Stock_In, c.Multiple_WeftCount_Status, c.Crimp_Percentage, d.EndsCount_Name, e.Count_Name, j.Po_No , tEC1.EndsCount_Name as EndsCountName_Beam1, tEC2.EndsCount_Name as EndsCountName_Beam2 from Beam_Knotting_Head a INNER JOIN Ledger_Head b ON a.Ledger_IdNo = b.Ledger_IdNo INNER JOIN Cloth_Head c ON a.Cloth_IdNo1 = c.Cloth_IdNo INNER JOIN EndsCount_Head d ON a.EndsCount_IdNo = d.EndsCount_IdNo INNER JOIN Count_Head e ON c.Cloth_WeftCount_IdNo = e.Count_IdNo LEFT OUTER JOIN JobWork_Pavu_Receipt_Details j ON a.Set_Code1  = j.Set_Code and  a.Beam_no1  = j.Beam_No LEFT OUTER JOIN EndsCount_Head tEC1 ON a.EndsCount1_IdNo = tEC1.EndsCount_IdNo  LEFT OUTER JOIN EndsCount_Head tEC2 ON a.EndsCount2_IdNo = tEC2.EndsCount_IdNo Where a.Loom_IdNo = " & Str(Val(Lm_ID)) & " and a.Beam_RunOut_Code = '' Order by a.Beam_Knotting_Date, a.for_OrderBy, a.Beam_Knotting_Code", con)
                Dt3 = New DataTable
                Da3.Fill(Dt3)
                If Dt3.Rows.Count > 0 Then
                    lbl_PartyName.Text = Dt3.Rows(0).Item("Ledger_Name").ToString

                    lbl_KnotCode.Text = Dt3.Rows(0).Item("Beam_Knotting_Code").ToString
                    lbl_KnotNo.Text = Dt3.Rows(0).Item("Beam_Knotting_No").ToString
                    lbl_EndsCount.Text = Dt3.Rows(0).Item("EndsCount_Name").ToString
                    lbl_EndsCount_Beam1.Text = Dt3.Rows(0).Item("EndsCountName_Beam1").ToString
                    lbl_EndsCount_Beam2.Text = Dt3.Rows(0).Item("EndsCountName_Beam2").ToString
                    lbl_WidthType.Text = Dt3.Rows(0).Item("Width_Type").ToString

                    lbl_ClothSales_OrderCode_forSelection.Text = Dt3.Rows(0).Item("ClothSales_OrderCode_forSelection").ToString

                    cbo_ClothName.Text = ""
                    lbl_WeftCount.Text = ""
                    vWFTCNT_NM = ""
                    If Val(Dt3.Rows(0).Item("Cloth_Idno2").ToString) = 0 And Val(Dt3.Rows(0).Item("Cloth_Idno3").ToString) = 0 And Val(Dt3.Rows(0).Item("Cloth_Idno4").ToString) = 0 Then
                        cbo_ClothName.Text = Dt3.Rows(0).Item("Cloth_Name").ToString
                        vWFTCNT_NM = Dt3.Rows(0).Item("Count_Name").ToString
                    ElseIf Val(Dt3.Rows(0).Item("Cloth_Idno1").ToString) = Val(Dt3.Rows(0).Item("Cloth_Idno2").ToString) And Val(Dt3.Rows(0).Item("Cloth_Idno3").ToString) = 0 And Val(Dt3.Rows(0).Item("Cloth_Idno4").ToString) = 0 Then
                        cbo_ClothName.Text = Dt3.Rows(0).Item("Cloth_Name").ToString
                        vWFTCNT_NM = Dt3.Rows(0).Item("Count_Name").ToString
                    ElseIf Val(Dt3.Rows(0).Item("Cloth_Idno1").ToString) = Val(Dt3.Rows(0).Item("Cloth_Idno2").ToString) And Val(Dt3.Rows(0).Item("Cloth_Idno1").ToString) = Val(Dt3.Rows(0).Item("Cloth_Idno3").ToString) And Val(Dt3.Rows(0).Item("Cloth_Idno4").ToString) = 0 Then
                        cbo_ClothName.Text = Dt3.Rows(0).Item("Cloth_Name").ToString
                        vWFTCNT_NM = Dt3.Rows(0).Item("Count_Name").ToString
                    ElseIf Val(Dt3.Rows(0).Item("Cloth_Idno1").ToString) = Val(Dt3.Rows(0).Item("Cloth_Idno2").ToString) And Val(Dt3.Rows(0).Item("Cloth_Idno1").ToString) = Val(Dt3.Rows(0).Item("Cloth_Idno3").ToString) And Val(Dt3.Rows(0).Item("Cloth_Idno1").ToString) = Val(Dt3.Rows(0).Item("Cloth_Idno4").ToString) Then
                        cbo_ClothName.Text = Dt3.Rows(0).Item("Cloth_Name").ToString
                        vWFTCNT_NM = Dt3.Rows(0).Item("Count_Name").ToString
                    End If
                    If Val(Dt3.Rows(0).Item("Multiple_WeftCount_Status").ToString) = 1 Then
                        btn_Show_WeftConsumption_Details.Visible = True
                        btn_Show_WeftConsumption_Details.BringToFront()
                        lbl_WeftCount.Text = ""
                        get_Multiple_WeftYarn_Consumption_Count_Details(Val(Dt3.Rows(0).Item("Cloth_IdNo").ToString))
                    Else
                        lbl_WeftCount.Text = vWFTCNT_NM
                        btn_Show_WeftConsumption_Details.Visible = False
                        dgv_Weft_Consumption_Details.Rows.Clear()
                    End If
                    set_grid_Meters_ColumnHeading(Dt3.Rows(0).Item("Stock_In").ToString)

                    'lbl_TotMtrs1.Text = ""
                    'lbl_BalMtrs1.Text = ""
                    'Da4 = New SqlClient.SqlDataAdapter("Select * from Stock_SizedPavu_Processing_Details Where Set_Code = '" & Trim(lbl_SetCode1.Text) & "' and Beam_No = '" & Trim(lbl_BeamNo1.Text) & "'", con)
                    'Dt4 = New DataTable
                    'Da4.Fill(Dt4)
                    'If Dt4.Rows.Count > 0 Then
                    '    lbl_TotMtrs1.Text = Dt4.Rows(0).Item("Meters").ToString
                    '    lbl_BalMtrs1.Text = Format(Val(Dt4.Rows(0).Item("Meters").ToString) - Val(Dt3.Rows(0).Item("Production_Meters").ToString), "#########0.00")
                    'End If
                    'Dt4.Clear()

                    lbl_SetCode1.Text = Dt3.Rows(0).Item("Set_Code1").ToString
                    lbl_SetNo1.Text = Dt3.Rows(0).Item("Set_No1").ToString
                    lbl_BeamNo1.Text = Dt3.Rows(0).Item("Beam_No1").ToString
                    lbl_poNo.Text = Dt3.Rows(0).Item("po_No").ToString
                    lbl_TotMtrs1.Text = ""
                    lbl_BalMtrs1.Text = ""
                    Da4 = New SqlClient.SqlDataAdapter("Select * from Stock_SizedPavu_Processing_Details Where Set_Code = '" & Trim(lbl_SetCode1.Text) & "' and Beam_No = '" & Trim(lbl_BeamNo1.Text) & "'", con)
                    Dt4 = New DataTable
                    Da4.Fill(Dt4)
                    If Dt4.Rows.Count > 0 Then
                        lbl_TotMtrs1.Text = Dt4.Rows(0).Item("Meters").ToString
                        lbl_BalMtrs1.Text = Format(Val(Dt4.Rows(0).Item("Meters").ToString) - Val(Dt4.Rows(0).Item("Production_Meters").ToString), "#########0.00")
                    End If
                    Dt4.Clear()

                    lbl_SetCode2.Text = Dt3.Rows(0).Item("Set_Code2").ToString
                    lbl_SetNo2.Text = Dt3.Rows(0).Item("Set_No2").ToString
                    lbl_BeamNo2.Text = Dt3.Rows(0).Item("Beam_No2").ToString
                    lbl_BalMtrs2.Text = ""
                    lbl_TotMtrs2.Text = ""
                    If Trim(lbl_SetCode2.Text) <> "" And Trim(lbl_BeamNo2.Text) <> "" Then
                        Da4 = New SqlClient.SqlDataAdapter("Select * from Stock_SizedPavu_Processing_Details Where Set_Code = '" & Trim(lbl_SetCode2.Text) & "' and Beam_No = '" & Trim(lbl_BeamNo2.Text) & "'", con)
                        Dt4 = New DataTable
                        Da4.Fill(Dt4)
                        If Dt4.Rows.Count > 0 Then
                            lbl_TotMtrs2.Text = Dt4.Rows(0).Item("Meters").ToString
                            lbl_BalMtrs2.Text = Format(Val(Dt4.Rows(0).Item("Meters").ToString) - Val(Dt4.Rows(0).Item("Production_Meters").ToString), "#########0.00")
                        End If
                        Dt4.Clear()
                    End If

                    txt_CrimpPerc.Text = Dt3.Rows(0).Item("Crimp_Percentage").ToString
                    'lbl_ConsPavu.Text = dt3.Rows(0).Item("ConsumedPavu_Receipt").ToString
                    'lbl_ConsWeftYarn.Text = dt3.Rows(0).Item("ConsumedYarn_Receipt").ToString
                    lbl_Weaver_Job_No.Text = Dt3.Rows(0).Item("Weaving_JobCode_forSelection").ToString

                End If
                Dt3.Clear()


            End If

        End If




        cbo_LoomNo.Tag = cbo_LoomNo.Text
        cbo_Pcs_LastPiece_Status.Tag = cbo_Pcs_LastPiece_Status.Text

        Dt1.Dispose()
        Da1.Dispose()

        Dt2.Dispose()
        Da2.Dispose()

        Dt3.Dispose()
        Da3.Dispose()

        Dt4.Dispose()
        Da4.Dispose()


        If txt_DoffMtrs.Enabled And txt_DoffMtrs.Visible Then txt_DoffMtrs.Focus()

    End Sub

    Private Sub btn_Filter_Close_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Filter_Close.Click
        pnl_back.Enabled = True
        pnl_filter.Visible = False
        Filter_Status = False
    End Sub

    Private Sub btn_KnottingSelection_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_KnottingSelection.Click
        pnl_Selection.Visible = True
        pnl_back.Enabled = False

        cbo_SelectionLoomNo.Text = ""
        dgv_Selection.Rows.Clear()
        If Trim(cbo_LoomNo.Text) <> "" Then
            cbo_SelectionLoomNo.Text = cbo_LoomNo.Text
            btn_ShowKnottingDetails_Click(sender, e)
        End If

        If cbo_SelectionLoomNo.Enabled And cbo_SelectionLoomNo.Visible Then cbo_SelectionLoomNo.Focus()

    End Sub

    Private Sub btn_ShowKnottingDetails_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_ShowKnottingDetails.Click
        Dim Da1 As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim Dt2 As New DataTable
        Dim Lm_ID As Integer
        Dim NewCode As String = ""
        Dim EntKnotCode As String = ""
        Dim n As Integer = 0
        Dim i As Integer = 0
        Dim j As Integer = 0
        Dim SNo As Integer = 0

        Lm_ID = Common_Procedures.Loom_NameToIdNo(con, cbo_SelectionLoomNo.Text)
        If Val(Lm_ID) = 0 Then
            MessageBox.Show("Invalid Loom No", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If cbo_SelectionLoomNo.Enabled Then cbo_SelectionLoomNo.Focus()
            Exit Sub
        End If

        NewCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RollNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        EntKnotCode = ""

        SNo = 0
        dgv_Selection.Rows.Clear()

        Da1 = New SqlClient.SqlDataAdapter("select a.*, tP.Ledger_Name, b.Loom_Name, c.Cloth_Name, c.Crimp_Percentage, d.EndsCount_Name, e.Count_Name from Weaver_Cloth_Receipt_Head tW INNER JOIN Beam_Knotting_Head a ON Tw.Beam_Knotting_Code = a.Beam_Knotting_Code INNER JOIN Ledger_Head tP ON a.Ledger_IdNo <> 0 and a.Ledger_IdNo = tP.Ledger_IdNo INNER JOIN Loom_Head b ON a.Loom_IdNo <> 0 and a.Loom_IdNo = b.Loom_IdNo INNER JOIN Cloth_Head c ON a.Cloth_IdNo1 <> 0 and a.Cloth_IdNo1 = c.Cloth_IdNo INNER JOIN EndsCount_Head d ON a.EndsCount_IdNo <> 0 and a.EndsCount_IdNo = d.EndsCount_IdNo INNER JOIN Count_Head e ON c.Cloth_WeftCount_IdNo <> 0 and c.Cloth_WeftCount_IdNo = e.Count_IdNo Where tW.Loom_IdNo = " & Str(Val(Lm_ID)) & " and tW.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and tW.Weaver_ClothReceipt_Code = '" & Trim(NewCode) & "' and tW.Receipt_Type = 'L' Order by a.Beam_Knotting_Date Desc, a.for_OrderBy Desc, a.Beam_Knotting_Code Desc", con)
        Dt1 = New DataTable
        Da1.Fill(Dt1)
        If Dt1.Rows.Count > 0 Then

            n = dgv_Selection.Rows.Add()

            SNo = SNo + 1
            dgv_Selection.Rows(n).Cells(0).Value = Val(SNo)
            dgv_Selection.Rows(n).Cells(1).Value = Format(Convert.ToDateTime(Dt1.Rows(i).Item("Beam_Knotting_Date").ToString), "dd-MM-yyyy")
            dgv_Selection.Rows(n).Cells(2).Value = Dt1.Rows(i).Item("Beam_Knotting_No").ToString
            dgv_Selection.Rows(n).Cells(3).Value = Dt1.Rows(i).Item("Loom_Name").ToString
            dgv_Selection.Rows(n).Cells(4).Value = Dt1.Rows(i).Item("Ledger_Name").ToString
            dgv_Selection.Rows(n).Cells(5).Value = Dt1.Rows(i).Item("Cloth_Name").ToString
            dgv_Selection.Rows(n).Cells(6).Value = Dt1.Rows(i).Item("Set_No1").ToString
            dgv_Selection.Rows(n).Cells(7).Value = Dt1.Rows(i).Item("Beam_No1").ToString
            dgv_Selection.Rows(n).Cells(8).Value = Dt1.Rows(i).Item("Beam_No2").ToString
            dgv_Selection.Rows(n).Cells(9).Value = ""
            dgv_Selection.Rows(n).Cells(10).Value = ""

            If Trim(Dt1.Rows(i).Item("Set_Code1").ToString) <> "" And Trim(Dt1.Rows(i).Item("Beam_No1").ToString) <> "" Then
                Da1 = New SqlClient.SqlDataAdapter("Select * from Stock_SizedPavu_Processing_Details Where Set_Code = '" & Trim(Dt1.Rows(i).Item("Set_Code1").ToString) & "' and Beam_No = '" & Trim(Dt1.Rows(i).Item("Beam_No1").ToString) & "'", con)
                Dt2 = New DataTable
                Da1.Fill(Dt2)
                If Dt2.Rows.Count > 0 Then
                    dgv_Selection.Rows(n).Cells(9).Value = Dt2.Rows(0).Item("Meters").ToString
                    dgv_Selection.Rows(n).Cells(10).Value = Format(Val(Dt2.Rows(0).Item("Meters").ToString) - Val(Dt2.Rows(0).Item("Production_Meters").ToString), "#########0.00")
                End If
                Dt2.Clear()
            End If

            dgv_Selection.Rows(n).Cells(11).Value = "1"
            dgv_Selection.Rows(n).Cells(12).Value = Dt1.Rows(i).Item("Beam_Knotting_Code").ToString

            For j = 0 To dgv_Selection.ColumnCount - 1
                dgv_Selection.Rows(n).Cells(j).Style.ForeColor = Color.Red
            Next

            EntKnotCode = Dt1.Rows(i).Item("Beam_Knotting_Code").ToString

        End If
        Dt1.Clear()

        Da1 = New SqlClient.SqlDataAdapter("select a.*, tP.Ledger_Name, b.Loom_Name, c.Cloth_Name, c.Crimp_Percentage, d.EndsCount_Name, e.Count_Name from Beam_Knotting_Head a INNER JOIN Ledger_Head tP ON a.Ledger_IdNo <> 0 and a.Ledger_IdNo = tP.Ledger_IdNo INNER JOIN Loom_Head b ON a.Loom_IdNo <> 0 and a.Loom_IdNo = b.Loom_IdNo INNER JOIN Cloth_Head c ON a.Cloth_IdNo1 <> 0 and a.Cloth_IdNo1 = c.Cloth_IdNo INNER JOIN EndsCount_Head d ON a.EndsCount_IdNo <> 0 and a.EndsCount_IdNo = d.EndsCount_IdNo INNER JOIN Count_Head e ON c.Cloth_WeftCount_IdNo <> 0 and c.Cloth_WeftCount_IdNo = e.Count_IdNo Where a.Loom_IdNo = " & Str(Val(Lm_ID)) & " and a.Beam_Knotting_Code <> '" & Trim(EntKnotCode) & "' Order by a.Beam_Knotting_Date Desc, a.for_OrderBy Desc, a.Beam_Knotting_Code Desc", con)
        Dt1 = New DataTable
        Da1.Fill(Dt1)
        If Dt1.Rows.Count > 0 Then

            For i = 0 To Dt1.Rows.Count - 1

                n = dgv_Selection.Rows.Add()

                SNo = SNo + 1
                dgv_Selection.Rows(n).Cells(0).Value = Val(SNo)
                dgv_Selection.Rows(n).Cells(1).Value = Format(Convert.ToDateTime(Dt1.Rows(i).Item("Beam_Knotting_Date").ToString), "dd-MM-yyyy")
                dgv_Selection.Rows(n).Cells(2).Value = Dt1.Rows(i).Item("Beam_Knotting_No").ToString
                dgv_Selection.Rows(n).Cells(3).Value = Dt1.Rows(i).Item("Loom_Name").ToString
                dgv_Selection.Rows(n).Cells(4).Value = Dt1.Rows(i).Item("Ledger_Name").ToString
                dgv_Selection.Rows(n).Cells(5).Value = Dt1.Rows(i).Item("Cloth_Name").ToString
                dgv_Selection.Rows(n).Cells(6).Value = Dt1.Rows(i).Item("Set_No1").ToString
                dgv_Selection.Rows(n).Cells(7).Value = Dt1.Rows(i).Item("Beam_No1").ToString
                dgv_Selection.Rows(n).Cells(8).Value = Dt1.Rows(i).Item("Beam_No2").ToString
                dgv_Selection.Rows(n).Cells(9).Value = ""
                dgv_Selection.Rows(n).Cells(10).Value = ""

                If Trim(Dt1.Rows(i).Item("Set_Code1").ToString) <> "" And Trim(Dt1.Rows(i).Item("Beam_No1").ToString) <> "" Then
                    Da1 = New SqlClient.SqlDataAdapter("Select * from Stock_SizedPavu_Processing_Details Where Set_Code = '" & Trim(Dt1.Rows(i).Item("Set_Code1").ToString) & "' and Beam_No = '" & Trim(Dt1.Rows(i).Item("Beam_No1").ToString) & "'", con)
                    Dt2 = New DataTable
                    Da1.Fill(Dt2)
                    If Dt2.Rows.Count > 0 Then
                        dgv_Selection.Rows(n).Cells(9).Value = Dt2.Rows(0).Item("Meters").ToString
                        dgv_Selection.Rows(n).Cells(10).Value = Format(Val(Dt2.Rows(0).Item("Meters").ToString) - Val(Dt2.Rows(0).Item("Production_Meters").ToString), "#########0.00")
                    End If
                    Dt2.Clear()
                End If

                dgv_Selection.Rows(n).Cells(11).Value = ""
                dgv_Selection.Rows(n).Cells(12).Value = Dt1.Rows(i).Item("Beam_Knotting_Code").ToString

                For j = 0 To dgv_Selection.ColumnCount - 1
                    dgv_Selection.Rows(n).Cells(j).Style.ForeColor = Color.Black
                Next

            Next

        End If
        Dt1.Clear()

        Dt1.Dispose()
        Da1.Dispose()

        Dt2.Dispose()

        If dgv_Selection.Rows.Count > 0 Then
            If dgv_Selection.Enabled And dgv_Selection.Visible Then
                dgv_Selection.Focus()
                dgv_Selection.CurrentCell = dgv_Selection.Rows(0).Cells(0)
            End If

        Else
            If cbo_SelectionLoomNo.Enabled And cbo_SelectionLoomNo.Visible Then cbo_SelectionLoomNo.Focus()

        End If

    End Sub

    Private Sub Select_Knotting(ByVal RwIndx As Integer)
        Dim i As Integer
        Dim j As Integer

        With dgv_Selection

            If .RowCount > 0 And RwIndx >= 0 Then

                For i = 0 To .Rows.Count - 1
                    .Rows(i).Cells(11).Value = ""
                    For j = 0 To .ColumnCount - 1
                        .Rows(i).Cells(j).Style.ForeColor = Color.Black
                    Next
                Next

                .Rows(RwIndx).Cells(11).Value = 1
                For i = 0 To .ColumnCount - 1
                    .Rows(RwIndx).Cells(i).Style.ForeColor = Color.Red
                Next

            End If

        End With

    End Sub

    Private Sub dgv_Selection_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_Selection.KeyDown
        Dim n As Integer

        Try

            If e.KeyCode = Keys.Enter Or e.KeyCode = Keys.Space Then

                If dgv_Selection.Rows.Count > 0 Then

                    If dgv_Selection.CurrentCell.RowIndex >= 0 Then

                        n = dgv_Selection.CurrentCell.RowIndex

                        Select_Knotting(n)

                        e.Handled = True

                    End If

                End If

            End If

        Catch ex As Exception
            '---

        End Try

    End Sub

    Private Sub dgv_Selection_CellClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Selection.CellClick
        Select_Knotting(e.RowIndex)
    End Sub

    Private Sub dgv_Selection_CellDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Selection.CellDoubleClick
        Select_Knotting(e.RowIndex)
        Close_Knotting_Selection()
    End Sub

    Private Sub btn_Close_Selection_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Close_Selection.Click
        Close_Knotting_Selection()
    End Sub

    Private Sub Close_Knotting_Selection()
        Dim Da1 As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim Da4 As New SqlClient.SqlDataAdapter
        Dim Dt4 As New DataTable
        Dim KnotCode As String = ""
        Dim vWFTCNT_NM As String = ""

        KnotCode = ""
        For i = 0 To dgv_Selection.RowCount - 1

            If Val(dgv_Selection.Rows(i).Cells(11).Value) = 1 Then
                KnotCode = dgv_Selection.Rows(i).Cells(12).Value
                Exit For

            End If

        Next

        Da1 = New SqlClient.SqlDataAdapter("select a.*, b.Ledger_Name, c.Cloth_IdNo, c.Cloth_Name, c.Stock_In, c.Multiple_WeftCount_Status, c.Crimp_Percentage, d.EndsCount_Name, e.Count_Name, f.Loom_name, tEC1.EndsCount_Name as EndsCountName_Beam1, tEC2.EndsCount_Name as EndsCountName_Beam2  from Beam_Knotting_Head a INNER JOIN Ledger_Head b ON a.Ledger_IdNo <> 0 and a.Ledger_IdNo = b.Ledger_IdNo INNER JOIN Cloth_Head c ON a.Cloth_IdNo1 <> 0 and a.Cloth_IdNo1 = c.Cloth_IdNo INNER JOIN EndsCount_Head d ON a.EndsCount_IdNo <> 0 and a.EndsCount_IdNo = d.EndsCount_IdNo INNER JOIN Count_Head e ON c.Cloth_WeftCount_IdNo <> 0 and c.Cloth_WeftCount_IdNo = e.Count_IdNo INNER JOIN Loom_Head f ON a.Loom_IdNo <> 0 and a.Loom_IdNo = f.Loom_IdNo LEFT OUTER JOIN EndsCount_Head tEC1 ON a.EndsCount1_IdNo = tEC1.EndsCount_IdNo LEFT OUTER JOIN EndsCount_Head tEC2 ON a.EndsCount2_IdNo = tEC2.EndsCount_IdNo Where a.Beam_Knotting_Code = '" & Trim(KnotCode) & "'", con)
        Dt1 = New DataTable
        Da1.Fill(Dt1)
        If Dt1.Rows.Count > 0 Then

            lbl_PartyName.Text = Dt1.Rows(0).Item("Ledger_Name").ToString

            cbo_LoomNo.Text = Dt1.Rows(0).Item("Loom_Name").ToString

            lbl_KnotCode.Text = Dt1.Rows(0).Item("Beam_Knotting_Code").ToString
            lbl_KnotNo.Text = Dt1.Rows(0).Item("Beam_Knotting_No").ToString
            lbl_EndsCount.Text = Dt1.Rows(0).Item("EndsCount_Name").ToString
            lbl_EndsCount_Beam1.Text = Dt1.Rows(0).Item("EndsCountName_Beam1").ToString
            lbl_EndsCount_Beam2.Text = Dt1.Rows(0).Item("EndsCountName_Beam2").ToString
            lbl_WidthType.Text = Dt1.Rows(0).Item("Width_Type").ToString

            cbo_ClothName.Text = ""
            lbl_WeftCount.Text = ""
            vWFTCNT_NM = ""
            If Val(Dt1.Rows(0).Item("Cloth_Idno2").ToString) = 0 And Val(Dt1.Rows(0).Item("Cloth_Idno3").ToString) = 0 And Val(Dt1.Rows(0).Item("Cloth_Idno4").ToString) = 0 Then
                cbo_ClothName.Text = Dt1.Rows(0).Item("Cloth_Name").ToString
                vWFTCNT_NM = Dt1.Rows(0).Item("Count_Name").ToString
            ElseIf Val(Dt1.Rows(0).Item("Cloth_Idno1").ToString) = Val(Dt1.Rows(0).Item("Cloth_Idno2").ToString) And Val(Dt1.Rows(0).Item("Cloth_Idno3").ToString) = 0 And Val(Dt1.Rows(0).Item("Cloth_Idno4").ToString) = 0 Then
                cbo_ClothName.Text = Dt1.Rows(0).Item("Cloth_Name").ToString
                vWFTCNT_NM = Dt1.Rows(0).Item("Count_Name").ToString
            ElseIf Val(Dt1.Rows(0).Item("Cloth_Idno1").ToString) = Val(Dt1.Rows(0).Item("Cloth_Idno2").ToString) And Val(Dt1.Rows(0).Item("Cloth_Idno1").ToString) = Val(Dt1.Rows(0).Item("Cloth_Idno3").ToString) And Val(Dt1.Rows(0).Item("Cloth_Idno4").ToString) = 0 Then
                cbo_ClothName.Text = Dt1.Rows(0).Item("Cloth_Name").ToString
                vWFTCNT_NM = Dt1.Rows(0).Item("Count_Name").ToString
            ElseIf Val(Dt1.Rows(0).Item("Cloth_Idno1").ToString) = Val(Dt1.Rows(0).Item("Cloth_Idno2").ToString) And Val(Dt1.Rows(0).Item("Cloth_Idno1").ToString) = Val(Dt1.Rows(0).Item("Cloth_Idno3").ToString) And Val(Dt1.Rows(0).Item("Cloth_Idno1").ToString) = Val(Dt1.Rows(0).Item("Cloth_Idno4").ToString) Then
                cbo_ClothName.Text = Dt1.Rows(0).Item("Cloth_Name").ToString
                vWFTCNT_NM = Dt1.Rows(0).Item("Count_Name").ToString
            End If

            If Val(Dt1.Rows(0).Item("Multiple_WeftCount_Status").ToString) = 1 Then
                btn_Show_WeftConsumption_Details.Visible = True
                btn_Show_WeftConsumption_Details.BringToFront()
                lbl_WeftCount.Text = ""
                get_Multiple_WeftYarn_Consumption_Count_Details(Val(Dt1.Rows(0).Item("Cloth_IdNo").ToString))
            Else
                lbl_WeftCount.Text = vWFTCNT_NM
                btn_Show_WeftConsumption_Details.Visible = False
                dgv_Weft_Consumption_Details.Rows.Clear()
            End If
            set_grid_Meters_ColumnHeading(Dt1.Rows(0).Item("Stock_In").ToString)

            lbl_SetCode1.Text = Dt1.Rows(0).Item("Set_Code1").ToString
            lbl_SetNo1.Text = Dt1.Rows(0).Item("Set_No1").ToString
            lbl_BeamNo1.Text = Dt1.Rows(0).Item("Beam_No1").ToString

            lbl_TotMtrs1.Text = ""
            lbl_BalMtrs1.Text = ""
            Da4 = New SqlClient.SqlDataAdapter("Select * from Stock_SizedPavu_Processing_Details Where Set_Code = '" & Trim(lbl_SetCode1.Text) & "' and Beam_No = '" & Trim(lbl_BeamNo1.Text) & "'", con)
            Dt4 = New DataTable
            Da4.Fill(Dt4)
            If Dt4.Rows.Count > 0 Then
                lbl_TotMtrs1.Text = Dt4.Rows(0).Item("Meters").ToString
                lbl_BalMtrs1.Text = Format(Val(Dt4.Rows(0).Item("Meters").ToString) - Val(Dt4.Rows(0).Item("Production_Meters").ToString), "#########0.00")
            End If
            Dt4.Clear()

            lbl_SetCode2.Text = Dt1.Rows(0).Item("Set_Code2").ToString
            lbl_SetNo2.Text = Dt1.Rows(0).Item("Set_No2").ToString
            lbl_BeamNo2.Text = Dt1.Rows(0).Item("Beam_No2").ToString
            lbl_BalMtrs2.Text = ""
            lbl_TotMtrs2.Text = ""
            If Trim(lbl_SetCode2.Text) <> "" And Trim(lbl_BeamNo2.Text) <> "" Then
                Da4 = New SqlClient.SqlDataAdapter("Select * from Stock_SizedPavu_Processing_Details Where Set_Code = '" & Trim(lbl_SetCode2.Text) & "' and Beam_No = '" & Trim(lbl_BeamNo2.Text) & "'", con)
                Dt4 = New DataTable
                Da4.Fill(Dt4)
                If Dt4.Rows.Count > 0 Then
                    lbl_TotMtrs2.Text = Dt4.Rows(0).Item("Meters").ToString
                    lbl_BalMtrs2.Text = Format(Val(Dt4.Rows(0).Item("Meters").ToString) - Val(Dt4.Rows(0).Item("Production_Meters").ToString), "#########0.00")
                End If
                Dt4.Clear()
            End If

            'txt_DoffMtrs.Text = dt1.Rows(0).Item("Doff_Meters").ToString
            txt_CrimpPerc.Text = Dt1.Rows(0).Item("Crimp_Percentage").ToString
            'lbl_ConsPavu.Text = dt1.Rows(0).Item("ConsumedPavu_Receipt").ToString
            'lbl_ConsWeftYarn.Text = dt1.Rows(0).Item("ConsumedYarn_Receipt").ToString

        End If
        Dt1.Clear()

        Dt1.Dispose()
        Da1.Dispose()

        cbo_Pcs_LastPiece_Status.Tag = cbo_Pcs_LastPiece_Status.Text

        ConsumedPavu_Calculation()
        ConsumedYarn_Calculation()

        pnl_back.Enabled = True
        pnl_Selection.Visible = False
        If txt_DoffMtrs.Enabled And txt_DoffMtrs.Visible Then txt_DoffMtrs.Focus()

    End Sub

    Private Sub btn_CloseKnottingDetails_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_CloseKnottingDetails.Click
        Close_Knotting_Selection()
    End Sub

    Private Sub cbo_SelectionLoomNo_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_SelectionLoomNo.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Loom_Head", "Loom_Name", "", "(Loom_IdNo = 0)")
    End Sub

    Private Sub cbo_SelectionLoomNo_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_SelectionLoomNo.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_SelectionLoomNo, Nothing, Nothing, "Loom_Head", "Loom_Name", "", "(Loom_IdNo = 0)")

        If (e.KeyValue = 40 And cbo_SelectionLoomNo.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
            If dgv_Selection.Rows.Count > 0 Then
                If dgv_Selection.Enabled And dgv_Selection.Visible Then
                    dgv_Selection.Focus()
                    dgv_Selection.CurrentCell = dgv_Selection.Rows(0).Cells(0)
                End If

            Else
                If btn_ShowKnottingDetails.Enabled And btn_ShowKnottingDetails.Visible Then btn_ShowKnottingDetails.Focus()

            End If

        End If


    End Sub

    Private Sub cbo_SelectionLoomNo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_SelectionLoomNo.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_SelectionLoomNo, Nothing, "Loom_Head", "Loom_Name", "", "(Loom_IdNo = 0 )")
        If Asc(e.KeyChar) = 13 Then
            btn_ShowKnottingDetails_Click(sender, e)
            'If MessageBox.Show("Do you want to select Pavu :", "FOR PAVU SELECTION...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = DialogResult.Yes Then
            '    btn_Selection_Click(sender, e)
            'Else
            '    cbo_WidthType.Focus()
            'End If
        End If
    End Sub

    Private Sub cbo_Filter_BeamNo_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Filter_BeamNo.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Stock_SizedPavu_Processing_Details", "BeamNo_SetCode_forSelection", "", "(BeamNo_SetCode_forSelection = '')")
    End Sub

    Private Sub cbo_Filter_BeamNo_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Filter_BeamNo.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_BeamNo, cbo_Filter_LoomNo, btn_filtershow, "Stock_SizedPavu_Processing_Details", "BeamNo_SetCode_forSelection", "", "(BeamNo_SetCode_forSelection = '')")
    End Sub

    Private Sub cbo_Filter_BeamNo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Filter_BeamNo.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Filter_BeamNo, btn_filtershow, "Stock_SizedPavu_Processing_Details", "BeamNo_SetCode_forSelection", "", "(BeamNo_SetCode_forSelection = '')")
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

    Public Sub Generate_Barcode()

        txt_BarCode.Text = Microsoft.VisualBasic.Left(Common_Procedures.FnYearCode, 2) & Trim(Val(lbl_Company.Tag)) & Trim(UCase(lbl_RollNo.Text)) & "U"

    End Sub

    Private Sub btn_BarCodePrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_BarCodePrint.Click
        Common_Procedures.Print_OR_Preview_Status = 0
        'Printing_BarCode_Sticker()
        print_record()
    End Sub

    'Public Sub print_record() Implements Interface_MDIActions.print_record
    '    Common_Procedures.Print_OR_Preview_Status = 0
    '    Printing_BarCode_Sticker()
    'End Sub

    Public Sub print_record() Implements Interface_MDIActions.print_record
        pnl_Print.Visible = True
        pnl_back.Enabled = False
        txt_PrintFrom.Text = lbl_RollNo.Text
        txt_PrintTo.Text = lbl_RollNo.Text
        If txt_PrintFrom.Enabled And txt_PrintFrom.Visible Then
            txt_PrintFrom.Focus()
            txt_PrintFrom.SelectAll()
        End If
    End Sub

    Private Sub Printing_BarCode_Sticker()
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim NewCode As String
        Dim sql As String = ""


        NewCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RollNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)


        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.PrintEntry, Common_Procedures.UR.Weaver_Pavu_Rceipt_Entry, New_Entry) = False Then Exit Sub

        Try

            da1 = New SqlClient.SqlDataAdapter("Select a.*, c.Cloth_Name from Weaver_Cloth_Receipt_Head a INNER JOIN Cloth_Head c ON a.Cloth_IdNo = c.Cloth_IdNo Where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Weaver_ClothReceipt_Code = '" & Trim(NewCode) & "' and a.Receipt_Type = 'L'", con)
            dt1 = New DataTable
            da1.Fill(dt1)

            If dt1.Rows.Count <= 0 Then

                MessageBox.Show("This is New Entry", "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                Exit Sub

            End If

            dt1.Dispose()
            da1.Dispose()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

        Dim pkCustomSize1 As New System.Drawing.Printing.PaperSize("PAPER 3.25X1.18", 325, 118)
        'Dim pkCustomSize1 As New System.Drawing.Printing.PaperSize("PAPER 3.25X1.5", 325, 150)
        PrintDocument2.PrinterSettings.DefaultPageSettings.PaperSize = pkCustomSize1
        PrintDocument2.DefaultPageSettings.PaperSize = pkCustomSize1

        If Val(Common_Procedures.Print_OR_Preview_Status) = 1 Then

            Try


                'Else
                If Val(Common_Procedures.settings.Printing_Show_PrintDialogue) = 1 Then
                    PrintDialog1.PrinterSettings = PrintDocument2.PrinterSettings
                    If PrintDialog1.ShowDialog() = Windows.Forms.DialogResult.OK Then
                        PrintDocument2.PrinterSettings = PrintDialog1.PrinterSettings
                        PrintDocument2.Print()
                    End If

                Else
                    PrintDocument2.Print()

                End If

                'End If

            Catch ex As Exception
                MessageBox.Show("The printing operation failed" & vbCrLf & ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

            End Try


        Else

            Try

                Dim ppd As New PrintPreviewDialog

                ppd.Document = PrintDocument2

                ppd.WindowState = FormWindowState.Maximized
                ppd.StartPosition = FormStartPosition.CenterScreen
                'ppd.ClientSize = New Size(600, 600)
                ppd.PrintPreviewControl.AutoZoom = True
                ppd.PrintPreviewControl.Zoom = 1.0

                ppd.ShowDialog()

            Catch ex As Exception
                MsgBox("The printing operation failed" & vbCrLf & ex.Message, MsgBoxStyle.Critical, "DOES NOT SHOW PRINT PREVIEW...")

            End Try

        End If

        'Print_PDF_Status = False

    End Sub

    Private Sub PrintDocument2_BeginPrint(ByVal sender As Object, ByVal e As System.Drawing.Printing.PrintEventArgs) Handles PrintDocument2.BeginPrint
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim NewCode As String
        Dim SQL As String
        Dim prtFrm As Single = 0
        Dim prtTo As Single = 0
        Dim Condt As String = ""

        NewCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RollNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)


        prn_HeadIndx = 0


        If Val(txt_PrintFrom.Text) = 0 Then Exit Sub
        If Val(txt_PrintTo.Text) = 0 Then Exit Sub



        prtFrm = Val(Common_Procedures.OrderBy_CodeToValue(txt_PrintFrom.Text))
        prtTo = Val(Common_Procedures.OrderBy_CodeToValue(txt_PrintTo.Text))

        Condt = ""
        If Val(txt_PrintFrom.Text) <> 0 And Val(txt_PrintTo.Text) <> 0 Then
            Condt = " a.for_OrderBy between " & Str(Val(prtFrm)) & " and " & Trim(prtTo)

        ElseIf Val(txt_PrintFrom.Text) <> 0 Then
            Condt = " a.for_OrderBy = " & Str(Val(prtFrm))

        Else
            Exit Sub

        End If

        prn_HdDt.Clear()
        prn_DetIndx = 0
        prn_DetSNo = 0
        prn_DetDt.Clear()
        prn_DetIndx = 0
        prn_DetSNo = 0
        prn_PageNo = 0
        prn_DetBarCdStkr = 1

        Try



            SQL = "Select a.*, c.Cloth_Name from Weaver_Cloth_Receipt_Head a " &
                        " INNER JOIN Cloth_Head c ON a.Cloth_IdNo = c.Cloth_IdNo " &
                        " Where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) &
                          " and a.Weaver_ClothReceipt_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "'" &
                            IIf(Trim(Condt) <> "", " and ", "") & Condt &
                          " and a.Receipt_Type = 'L'"

            da1 = New SqlClient.SqlDataAdapter(SQL, con)
            prn_HdDt = New DataTable
            da1.Fill(prn_HdDt)

            If prn_HdDt.Rows.Count > 0 Then

            Else
                MessageBox.Show("This is New Entry", "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

            End If

            da1.Dispose()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub PrintDocument2_PrintPage(ByVal sender As Object, ByVal e As System.Drawing.Printing.PrintPageEventArgs) Handles PrintDocument2.PrintPage
        If prn_HdDt.Rows.Count <= 0 Then Exit Sub

        Printing_BarCode_Sticker_Format1(e)

    End Sub

    Private Sub Printing_BarCode_Sticker_Format1(ByRef e As System.Drawing.Printing.PrintPageEventArgs)
        Dim pFont As Font, BarFont As Font
        Dim LMargin As Single, RMargin As Single, TMargin As Single, BMargin As Single
        Dim PrintWidth As Single, PrintHeight As Single
        Dim PageWidth As Single, PageHeight As Single
        Dim I As Integer
        Dim NoofItems_PerPage As Integer, NoofDets As Integer
        Dim TxtHgt As Single
        Dim PpSzSTS As Boolean = False
        Dim LnAr(15) As Single, ClAr(15) As Single
        Dim CurY As Single
        Dim CurX As Single
        Dim BrCdX As Single = 20
        Dim BrCdY As Single = 100
        Dim vBarCode As String = ""
        Dim vFldMtrs As String = ""
        Dim ItmNm1 As String, ItmNm2 As String


        Dim pkCustomSize1 As New System.Drawing.Printing.PaperSize("PAPER 3.25X1.18", 325, 118)
        PrintDocument2.PrinterSettings.DefaultPageSettings.PaperSize = pkCustomSize1
        PrintDocument2.DefaultPageSettings.PaperSize = pkCustomSize1
        e.PageSettings.PaperSize = pkCustomSize1

        With PrintDocument2.DefaultPageSettings.Margins
            .Left = 5
            .Right = 2
            .Top = 5 ' 40
            .Bottom = 2
            LMargin = .Left
            RMargin = .Right
            TMargin = .Top
            BMargin = .Bottom
        End With

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

        NoofItems_PerPage = 2

        TxtHgt = 13.5

        Try

            If prn_HdDt.Rows.Count > 0 Then




                For noofitems = 1 To NoofItems_PerPage
                    NoofDets = 0

                    vFldMtrs = Format(Val(prn_HdDt.Rows(prn_HeadIndx).Item("ReceiptMeters_Receipt").ToString), "##########0.00")
                    vBarCode = Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Bar_Code").ToString)

                    If Val(vFldMtrs) <> 0 Then

                        If NoofDets >= NoofItems_PerPage Then
                            e.HasMorePages = True
                            Return
                        End If

                        CurY = TMargin

                        CurX = LMargin - 1
                        If noofitems Mod 2 = 0 Then
                            CurX = CurX + ((PageWidth + RMargin) \ 2)
                        End If

                        'If Trim(prn_DetDt.Rows(prn_DetIndx).Item("Cloth_Description").ToString) <> "" Then
                        '    ItmNm1 = Trim(prn_DetDt.Rows(prn_DetIndx).Item("Cloth_Description").ToString)
                        'Else
                        ItmNm1 = Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Cloth_Name").ToString)
                        'End If

                        ItmNm2 = ""
                        If Len(ItmNm1) > 21 Then
                            For I = 21 To 1 Step -1
                                If Mid$(Trim(ItmNm1), I, 1) = " " Or Mid$(Trim(ItmNm1), I, 1) = "," Or Mid$(Trim(ItmNm1), I, 1) = "." Or Mid$(Trim(ItmNm1), I, 1) = "-" Or Mid$(Trim(ItmNm1), I, 1) = "/" Or Mid$(Trim(ItmNm1), I, 1) = "_" Or Mid$(Trim(ItmNm1), I, 1) = "(" Or Mid$(Trim(ItmNm1), I, 1) = ")" Or Mid$(Trim(ItmNm1), I, 1) = "\" Or Mid$(Trim(ItmNm1), I, 1) = "[" Or Mid$(Trim(ItmNm1), I, 1) = "]" Or Mid$(Trim(ItmNm1), I, 1) = "{" Or Mid$(Trim(ItmNm1), I, 1) = "}" Then Exit For
                            Next I
                            If I = 0 Then I = 21

                            ItmNm2 = Microsoft.VisualBasic.Right(Trim(ItmNm1), Len(ItmNm1) - I + 1)
                            ItmNm1 = Microsoft.VisualBasic.Left(Trim(ItmNm1), I - 1)
                        End If

                        pFont = New Font("Calibri", 9, FontStyle.Bold)
                        Common_Procedures.Print_To_PrintDocument(e, ItmNm1, CurX, CurY, 0, PrintWidth, pFont, , True)
                        If Trim(ItmNm2) <> "" Then
                            CurY = CurY + TxtHgt - 2
                            Common_Procedures.Print_To_PrintDocument(e, ItmNm2, CurX + 120, CurY, 1, PrintWidth, pFont, , True)
                        End If

                        pFont = New Font("Calibri", 9, FontStyle.Bold)

                        CurY = CurY + TxtHgt
                        Common_Procedures.Print_To_PrintDocument(e, "Lot NO: " & prn_HdDt.Rows(prn_HeadIndx).Item("Weaver_ClothReceipt_No").ToString, CurX, CurY, 0, PrintWidth, pFont, , True)

                        CurY = CurY + TxtHgt
                        Common_Procedures.Print_To_PrintDocument(e, "METERS : " & vFldMtrs, CurX, CurY, 0, PrintWidth, pFont, , True)

                        If Trim(Common_Procedures.settings.CustomerCode) = "1234" Then
                            CurY = CurY + TxtHgt
                            Common_Procedures.Print_To_PrintDocument(e, Common_Procedures.Loom_IdNoToName(con, prn_HdDt.Rows(prn_HeadIndx).Item("Loom_IdNo").ToString), CurX, CurY, 0, PrintWidth, pFont, , True)
                        End If


                        'vBarCode = Microsoft.VisualBasic.Left(Common_Procedures.FnYearCode, 2) & Val(lbl_Company.Tag) & Trim(prn_HdDt.Rows(0).Item("Piece_Receipt_No").ToString) & Trim(prn_DetDt.Rows(prn_DetIndx).Item("Piece_No").ToString) & Trim(Val(prn_DetBarCdStkr))

                        'vBarCode = Chr(204) & Trim(UCase(vBarCode)) & "g" & Chr(206)
                        'BarFont = New Font("Code 128", 36, FontStyle.Regular)
                        'BarFont = New Font("Code 128", 24, FontStyle.Regular)

                        vBarCode = "*" & Trim(UCase(vBarCode)) & "*"
                        'BarFont = New Font("Free 3 of 9", 24, FontStyle.Regular)
                        BarFont = New Font("Free 3 of 9", 18, FontStyle.Regular)

                        CurY = CurY + TxtHgt + 5
                        'CurY = CurY + TxtHgt + 2
                        'CurY = CurY + TxtHgt - 2
                        e.Graphics.DrawString(Trim(vBarCode), BarFont, Brushes.Black, CurX, CurY)

                        pFont = New Font("Calibri", 12, FontStyle.Bold)
                        'CurY = CurY + TxtHgt + TxtHgt + 5
                        CurY = CurY + TxtHgt + TxtHgt - 6
                        Common_Procedures.Print_To_PrintDocument(e, Trim(vBarCode), CurX + 5, CurY, 0, PrintWidth, pFont, , True)

                        NoofDets = NoofDets + 1

                    End If

                    prn_DetBarCdStkr = prn_DetBarCdStkr + 1

                    'Loop

                    prn_DetBarCdStkr = 1
                    prn_DetIndx = prn_DetIndx + 1



                    prn_HeadIndx = prn_HeadIndx + 1

                    If prn_HeadIndx > prn_HdDt.Rows.Count - 1 Then
                        Exit For
                    End If
                Next

            End If '' end of  If prn_HdDt.Rows.Count > 0 Then


            'prn_HeadIndx = prn_HeadIndx + 1

            If prn_HeadIndx <= prn_HdDt.Rows.Count - 1 Then
                e.HasMorePages = True
            Else
                'e.HasMorePages = False
                e.HasMorePages = False

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT PRINT BARCODE STICKER...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

        'e.HasMorePages = False

    End Sub

    Private Sub btn_SaveAll_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_SaveAll.Click
        Dim Pwd As String = ""
        Dim g As New Password

        g.ShowDialog()

        Pwd = Common_Procedures.Password_Input

        If Trim(Pwd) <> "TSSA7417" Then
            MessageBox.Show("Incorrect Password!...", "DOESNOT SAVE", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
        End If

        SaveAll_Sts = True

        LastNo = ""
        movelast_record()

        LastNo = lbl_RollNo.Text

        movefirst_record()
        Timer1.Enabled = True

    End Sub

    Private Sub Timer1_Tick(ByVal sender As Object, ByVal e As System.EventArgs) Handles Timer1.Tick
        save_record()
        If Trim(UCase(LastNo)) = Trim(lbl_RollNo.Text) Then
            Timer1.Enabled = False
            SaveAll_Sts = False
            MessageBox.Show("All Entries Saved Successfully", "FOR SAVING", MessageBoxButtons.OKCancel, MessageBoxIcon.Information)
            Exit Sub
        Else
            movenext_record()
        End If
    End Sub

    Private Sub btn_BarcodePrint_prnpnl_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_BarcodePrint_prnpnl.Click
        Common_Procedures.Print_OR_Preview_Status = 0
        Printing_BarCode_Sticker()

    End Sub

    Private Sub btn_Print_Cancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Print_Cancel.Click
        pnl_back.Enabled = True
        pnl_Print.Visible = False
    End Sub

    Private Sub btn_Close_Print_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Close_Print.Click
        btn_Print_Cancel_Click(sender, e)
    End Sub

    Private Sub txt_PrintTo_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_PrintTo.KeyDown
        If e.KeyCode = Keys.Down Then
            btn_Print_Ok.Focus()
        End If
        If e.KeyCode = Keys.Up Then
            txt_PrintFrom.Focus()
        End If
    End Sub

    Private Sub txt_PrintTo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_PrintTo.KeyPress
        If Asc(e.KeyChar) = 13 Then
            btn_BarcodePrint_prnpnl_Click(sender, e)
        End If
    End Sub

    Private Sub btn_UserModification_Click(sender As Object, e As System.EventArgs) Handles btn_UserModification.Click
        If Val(Common_Procedures.User.IdNo) = 1 Then
            Dim f1 As New User_Modifications
            f1.Entry_Name = Me.Name
            f1.Entry_PkValue = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RollNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)
            f1.ShowDialog()
        End If
    End Sub

    Private Sub Total_Calculation()
        Dim Sno As Integer
        Dim TotMtrs As String = 0, TotWgt As String = 0, TtMtrs_100Fld As String = 0
        Dim FldPerc As String = 0

        Sno = -1
        TotMtrs = 0
        TotWgt = 0

        With dgv_Details
            For i = 0 To .RowCount - 1
                Sno = Sno + 1

                '.Rows(i).Cells(0).Value = Chr(65 + Sno)
                '.Rows(i).Cells(0).Value = Sno

                If Trim(.Rows(i).Cells(dgvCol_Details.ClothType).Value) <> "" Or Val(.Rows(i).Cells(dgvCol_Details.Meters).Value) <> 0 Then
                    TotMtrs = Val(TotMtrs) + Val(.Rows(i).Cells(dgvCol_Details.Meters).Value)
                    TotWgt = Val(TotWgt) + Val(.Rows(i).Cells(dgvCol_Details.Weight).Value)
                End If
            Next
        End With

        With dgv_Details_Total
            If .RowCount = 0 Then .Rows.Add()
            .Rows(0).Cells(dgvCol_Details.Meters).Value = Format(Val(TotMtrs), "#########0.00")
            .Rows(0).Cells(dgvCol_Details.Weight).Value = Format(Val(TotWgt), "#########0.000")
        End With

        If txt_DoffMtrs.Visible = False Or txt_DoffMtrs.Enabled = False Then
            txt_DoffMtrs.Text = Format(Val(TotMtrs), "#########0.00")
        End If

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1370" Then '---- AKIL IMPEX (ANNUR)
            FldPerc = Val(txt_Folding.Text)
            If Val(FldPerc) = 0 Then FldPerc = 100

            TtMtrs_100Fld = Format(Val(TotMtrs) * Val(FldPerc) / 100, "#########0.00")

            lbl_ExcSht.Text = Format(Val(TotMtrs) - Val(TtMtrs_100Fld), "#########0.00")

        Else

            lbl_ExcSht.Text = Format(Val(TotMtrs) - Val(txt_DoffMtrs.Text), "#########0.00")

        End If


        '     wages_calculation()

    End Sub

    Private Sub dgtxt_Details_Enter(sender As Object, e As System.EventArgs) Handles dgtxt_Details.Enter
        dgv_Details.EditingControl.BackColor = Color.Lime
        dgv_Details.EditingControl.ForeColor = Color.Blue
        dgtxt_Details.SelectAll()

        'If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1428" Then '---- SAKTHI VINAYAGA TEXTILES  (ERODE-PALLIPALAYAM)
        '    'SendKeys.Send("{RIGHT}")
        '    'dgtxt_Details.DeselectAll()
        '    'If Trim(dgtxt_Details.Text) <> "" Then
        '    '    dgtxt_Details.SelectionStart = dgtxt_Details.Text.Length
        '    'End If
        'End If
        dgv_ActCtrlName = dgv_Details.Name
    End Sub

    Private Sub dgtxt_Details_KeyDown(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles dgtxt_Details.KeyDown
        With dgv_Details
            If .Visible Then
                If .Rows.Count > 0 Then
                    If Trim(.CurrentRow.Cells(dgvCol_Details.packslipcode).Value) <> "" Then
                        'e.Handled = True
                        'e.SuppressKeyPress = True

                    End If
                End If
            End If
        End With
    End Sub

    Private Sub dgtxt_Details_KeyPress(sender As Object, e As System.Windows.Forms.KeyPressEventArgs) Handles dgtxt_Details.KeyPress
        With dgv_Details
            If .Visible Then
                If .Rows.Count > 0 Then
                    If Trim(.CurrentRow.Cells(dgvCol_Details.packslipcode).Value) <> "" Then
                        ' e.Handled = True

                    Else
                        If .CurrentCell.ColumnIndex = dgvCol_Details.Meters Or .CurrentCell.ColumnIndex = dgvCol_Details.Weight Or .CurrentCell.ColumnIndex = dgvCol_Details.Wgt_Mtr Then
                            If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
                                e.Handled = True
                            End If
                        End If

                    End If
                End If
            End If
        End With
    End Sub

    Private Sub dgtxt_Details_KeyUp(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles dgtxt_Details.KeyUp
        dgv_Details_KeyUp(sender, e)
    End Sub

    Private Sub dgtxt_Details_TextChanged(sender As Object, e As System.EventArgs) Handles dgtxt_Details.TextChanged
        Try
            With dgv_Details
                If .Rows.Count <> 0 Then
                    .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(dgtxt_Details.Text)
                End If
            End With

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub dgv_Details_CellEndEdit(sender As Object, e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellEndEdit
        dgv_Details_CellLeave(sender, e)
    End Sub

    Private Sub dgv_Details_CellEnter(sender As Object, e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellEnter
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim Dt2 As New DataTable
        Dim Dt3 As New DataTable
        Dim rect As Rectangle
        Dim n As Integer = 0
        Dim vCloID As Integer = 0
        Dim vSrtNo As String = ""
        Dim vRolNo As String = ""
        Dim vPcNo As String = "", vPCSUBNO As String = ""


        With dgv_Details

            If Trim(.CurrentRow.Cells(dgvCol_Details.Pcsno).Value) = "" Then

                Dim vAUTO_CLOWISE_PcsNo As Boolean = False
                Dim vCOMP_NAME As String

                Dim vDUP_PCSNO As String = ""
                Dim vGET_AUTO_PcsNo As Boolean = False
                Dim vCLONAME_CONDT As String

                vAUTO_CLOWISE_PcsNo = False
                vGET_AUTO_PcsNo = False
                vCLONAME_CONDT = ""

                If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1438" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1474" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1487" Then

                    vGET_AUTO_PcsNo = True

                    If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1474" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1489" Then
                        vAUTO_CLOWISE_PcsNo = True

                    ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1438" Then
                        vCOMP_NAME = Common_Procedures.Company_IdNoToName(con, Val(lbl_Company.Tag))
                        vCOMP_NAME = vCOMP_NAME.ToString.ToUpper
                        If InStr(1, vCOMP_NAME, "KVP") > 0 And InStr(1, vCOMP_NAME, "WEAVES") > 0 Then
                            vAUTO_CLOWISE_PcsNo = True
                        End If

                    End If

                End If


                If vGET_AUTO_PcsNo = True Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1186" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1544" Then '---- UNITED WEAVES (PALLADAM)

                    vCLONAME_CONDT = " "
                    vCloID = 0
                    If vAUTO_CLOWISE_PcsNo = True Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1186" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1544" Then
                        vCloID = Common_Procedures.Cloth_NameToIdNo(con, cbo_ClothName.Text)
                        vCLONAME_CONDT = " and Cloth_IdNo = " & Str(Val(vCloID))
                    End If

                    vSrtNo = ""
                    If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1186" Then '---- UNITED WEAVES (PALLADAM)
                        vSrtNo = Common_Procedures.get_FieldValue(con, "Cloth_Head", "Sort_No", "(Cloth_IdNo = " & Str(Val(vCloID)) & ")")
                    End If

                    If e.RowIndex = 0 Then

                        vRolNo = ""
                        Da = New SqlClient.SqlDataAdapter("select Piece_No from Weaver_ClothReceipt_Piece_Details Where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Weaver_ClothReceipt_Code LIKE '" & Trim(Pk_Condition) & "%/" & Trim(Common_Procedures.FnYearCode) & "'  " & Trim(vCLONAME_CONDT) & " Order by Weaver_Piece_Checking_Date DESC, PieceNo_OrderBy DESC, Piece_No DESC", con)
                        Dt1 = New DataTable
                        Da.Fill(Dt1)
                        If Dt1.Rows.Count > 0 Then
                            vPcNo = Dt1.Rows(0).Item("Piece_No").ToString
                            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1186" Then '---- UNITED WEAVES (PALLADAM)
                                vRolNo = get_RollNo_from_PieceNo_for_CC1186(vPcNo)
                            Else
                                vRolNo = Val(vPcNo)
                            End If

                        End If
                        Dt1.Clear()

                        If Val(vRolNo) <> 0 Then

                            vRolNo = Trim(Val(vRolNo)) + 1

                            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1186" Then '---- UNITED WEAVES (PALLADAM)
                                .Rows(e.RowIndex).Cells(dgvCol_Details.Pcsno).Value = Trim(Common_Procedures.FnYearCode) & ":" & Trim(vSrtNo) & ":" & Trim(vRolNo)
                            Else
                                .Rows(e.RowIndex).Cells(dgvCol_Details.Pcsno).Value = Trim(vRolNo)
                            End If

                        End If

                    Else

                        vPcNo = .Rows(e.RowIndex - 1).Cells(dgvCol_Details.Pcsno).Value
                        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1186" Then '---- UNITED WEAVES (PALLADAM)
                            vRolNo = get_RollNo_from_PieceNo_for_CC1186(vPcNo)
                        Else
                            vRolNo = Val(vPcNo)
                        End If

                        If Val(vRolNo) <> 0 Then

                            vRolNo = Trim(Val(vRolNo)) + 1

                            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1186" Then '---- UNITED WEAVES (PALLADAM)
                                .Rows(e.RowIndex).Cells(dgvCol_Details.Pcsno).Value = Trim(Common_Procedures.FnYearCode) & ":" & Trim(vSrtNo) & ":" & Trim(vRolNo)
                            Else
                                .Rows(e.RowIndex).Cells(dgvCol_Details.Pcsno).Value = Trim(vRolNo)
                            End If

                        End If


                    End If


                ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1428" Then '---- SAKTHI VINAYAGA TEXTILES  (ERODE-PALLIPALAYAM)

                    'vCloID = Common_Procedures.Cloth_NameToIdNo(con, cbo_ClothName.Text)
                    ''vSrtNo = Common_Procedures.get_FieldValue(con, "Cloth_Head", "Sort_No", "(Cloth_IdNo = " & Str(Val(vCloID)) & ")")

                    'If e.RowIndex = 0 Then

                    '    vRolNo = ""
                    '    Da = New SqlClient.SqlDataAdapter("select Piece_No from Weaver_ClothReceipt_Piece_Details Where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Weaver_ClothReceipt_Code LIKE '" & Trim(Pk_Condition) & "%/" & Trim(Common_Procedures.FnYearCode) & "' Order by Weaver_Piece_Checking_Date DESC, PieceNo_OrderBy DESC, Piece_No DESC", con)
                    '    Dt1 = New DataTable
                    '    Da.Fill(Dt1)
                    '    If Dt1.Rows.Count > 0 Then
                    '        vPcNo = Dt1.Rows(0).Item("Piece_No").ToString
                    '        vRolNo = Val(vPcNo)
                    '    End If
                    '    Dt1.Clear()


                    '    vRolNo = Trim(Val(vRolNo) + 1) & "A"

                    '    .Rows(e.RowIndex).Cells(dgvCol_Details.Pcsno).Value = Trim(vRolNo)

                    'Else

                    '    vPcNo = Trim(.Rows(e.RowIndex - 1).Cells(dgvCol_Details.Pcsno).Value)
                    '    vRolNo = Val(vPcNo)

                    '    vPCSUBNO = ""
                    '    For k = Len(vPcNo) To 1 Step -1

                    '        If IsNumeric(Mid(vPcNo, k, 1)) = False Then
                    '            vPCSUBNO = Chr(Asc(Mid(vPcNo, k, 1)) + 1)
                    '            Exit For
                    '        End If

                    '    Next k

                    '    .Rows(e.RowIndex).Cells(dgvCol_Details.Pcsno).Value = Trim(vRolNo) & Trim(vPCSUBNO)

                    'End If

                ElseIf Common_Procedures.settings.ClothReceipt_PieceNo_Concept = "1,2,3" Or Trim(UCase(Common_Procedures.settings.ClothReceipt_PieceNo_Concept)) = "CONTINUOUS NO" Then
                    If e.RowIndex = 0 Then
                        .Rows(e.RowIndex).Cells(dgvCol_Details.Pcsno).Value = 1
                    Else
                        .Rows(e.RowIndex).Cells(dgvCol_Details.Pcsno).Value = Val(.Rows(e.RowIndex - 1).Cells(dgvCol_Details.Pcsno).Value) + 1
                    End If

                ElseIf Trim(UCase(Common_Procedures.settings.ClothReceipt_PieceNo_Concept)) = "A,B,C" Then
                    If e.RowIndex = 0 Then
                        .Rows(e.RowIndex).Cells(dgvCol_Details.Pcsno).Value = "A"
                    Else
                        .Rows(e.RowIndex).Cells(dgvCol_Details.Pcsno).Value = Chr(Asc(.Rows(e.RowIndex - 1).Cells(dgvCol_Details.Pcsno).Value) + 1)
                    End If

                End If

            End If

            If Trim(.CurrentRow.Cells(dgvCol_Details.ClothType).Value) = "" Then
                .CurrentRow.Cells(dgvCol_Details.ClothType).Value = Common_Procedures.ClothType_IdNoToName(con, 1)
            End If

            If e.ColumnIndex = 1 And Trim(.CurrentRow.Cells(dgvCol_Details.packslipcode).Value) = "" Then

                If cbo_Grid_ClothType.Visible = False Or Val(cbo_Grid_ClothType.Tag) <> e.RowIndex Then

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

    Private Sub dgv_Details_CellLeave(sender As Object, e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellLeave
        With dgv_Details

            If .CurrentCell.ColumnIndex = dgvCol_Details.Meters Then
                If Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value) <> 0 Then
                    .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = Format(Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value), "#########0.00")
                End If
            End If

            If .CurrentCell.ColumnIndex = dgvCol_Details.Weight Or .CurrentCell.ColumnIndex = dgvCol_Details.Wgt_Mtr Then
                If Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value) <> 0 Then
                    .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = Format(Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value), "#########0.000")
                Else
                    .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = ""
                End If
            End If

        End With
    End Sub

    Private Sub dgv_Details_CellValueChanged(sender As Object, e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellValueChanged
        Dim TtMtrs_100Fld As String = 0
        Dim FldPerc As String = 0

        Try

            If FrmLdSTS = True Then Exit Sub
            If IsNothing(dgv_Details.CurrentCell) Then Exit Sub

            With dgv_Details

                If .Visible Then

                    If .Rows.Count > 0 Then

                        If e.ColumnIndex = dgvCol_Details.Meters Or e.ColumnIndex = dgvCol_Details.Weight Then

                            TtMtrs_100Fld = Format(Val(.CurrentRow.Cells(dgvCol_Details.Meters).Value), "#########0.00")

                            If Common_Procedures.settings.CustomerCode = "1370" Then   'akill

                                FldPerc = Val(txt_Folding.Text)
                                If Val(FldPerc) = 0 Then FldPerc = 100

                                TtMtrs_100Fld = Format(Val(.CurrentRow.Cells(dgvCol_Details.Meters).Value) * Val(FldPerc) / 100, "#########0.00")

                            End If


                            If Val(.CurrentRow.Cells(dgvCol_Details.Meters).Value) <> 0 Then
                                .CurrentRow.Cells(dgvCol_Details.Wgt_Mtr).Value = Format(Val(.CurrentRow.Cells(dgvCol_Details.Weight).Value) / Val(TtMtrs_100Fld), "#########0.000")
                            Else
                                .CurrentRow.Cells(dgvCol_Details.Wgt_Mtr).Value = 0
                            End If

                            Total_Calculation()

                        End If
                    End If
                End If

            End With

        Catch ex As Exception
            '------

        End Try
    End Sub

    Private Sub dgv_Details_EditingControlShowing(sender As Object, e As System.Windows.Forms.DataGridViewEditingControlShowingEventArgs) Handles dgv_Details.EditingControlShowing
        dgtxt_Details = CType(dgv_Details.EditingControl, DataGridViewTextBoxEditingControl)

    End Sub

    Private Sub dgv_Details_KeyUp(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles dgv_Details.KeyUp
        Dim i As Integer = 0
        Dim n As Integer = 0
        Dim nrw As Integer
        Dim PNO As String
        Dim S As String

        With dgv_Details

            '-- Insert a row  with next no  (1, 2, 3  or   A, B, C  )
            If e.Control = True And (UCase(Chr(e.KeyCode)) = "I" Or e.KeyCode = Keys.Insert) Then

                n = .CurrentRow.Index

                PNO = Trim(UCase(.Rows(n).Cells(0).Value))

                If Common_Procedures.settings.ClothReceipt_PieceNo_Concept = "1,2,3" Or Trim(UCase(Common_Procedures.settings.ClothReceipt_PieceNo_Concept)) = "CONTINUOUS NO" Then

                    S = Val(PNO) + 1

                ElseIf Trim(UCase(Common_Procedures.settings.ClothReceipt_PieceNo_Concept)) = "A,B,C" Then

                    S = Chr(Asc(PNO) + 1)

                End If

                If n <> .Rows.Count - 1 Then
                    If Trim(UCase(S)) = Trim(UCase(.Rows(n + 1).Cells(0).Value)) Then
                        MessageBox.Show("Already Piece Inserted", "DOES NOT INSERT NEW PIECE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                        Exit Sub
                    End If
                End If

                nrw = n + 1

                dgv_Details.Rows.Insert(nrw, 1)

                dgv_Details.Rows(nrw).Cells(0).Value = S


            End If

            '-- add a new row  (1, 1A, 1B  or   A, A1, A2, A3  )
            If e.Control = True And UCase(Chr(e.KeyCode)) = "A" Then


                n = .CurrentRow.Index

                PNO = Trim(UCase(.Rows(n).Cells(0).Value))

                If Common_Procedures.settings.ClothReceipt_PieceNo_Concept = "1,2,3" Or Trim(UCase(Common_Procedures.settings.ClothReceipt_PieceNo_Concept)) = "CONTINUOUS NO" Then

                    S = Replace(Trim(PNO), Val(PNO), "")
                    PNO = Val(PNO)

                    If Trim(UCase(S)) <> "Z" Then
                        S = Trim(UCase(S))
                        If Trim(S) = "" Then S = "A" Else S = Trim(Chr(Asc(S) + 1))
                    End If

                ElseIf Trim(UCase(Common_Procedures.settings.ClothReceipt_PieceNo_Concept)) = "A,B,C" Then


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
                        MessageBox.Show("Already Piece Added", "DOES NOT ADD NEW PIECE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                        Exit Sub
                    End If
                End If

                nrw = n + 1

                dgv_Details.Rows.Insert(nrw, 1)

                dgv_Details.Rows(nrw).Cells(0).Value = Trim(UCase(PNO)) & S


            End If

            If e.Control And Trim(UCase(e.KeyCode)) = "D" Then



                n = .CurrentRow.Index

                If .CurrentCell.RowIndex = .Rows.Count - 1 Then
                    For i = 0 To .Columns.Count - 1
                        .Rows(n).Cells(i).Value = ""
                    Next
                Else
                    .Rows.RemoveAt(n)
                End If



            End If

        End With

    End Sub

    Private Sub dgv_Details_LostFocus(sender As Object, e As System.EventArgs) Handles dgv_Details.LostFocus
        On Error Resume Next
        If Not IsNothing(dgv_Details.CurrentCell) Then dgv_Details.CurrentCell.Selected = False
    End Sub

    Private Sub dgv_Details_RowsAdded(sender As Object, e As System.Windows.Forms.DataGridViewRowsAddedEventArgs) Handles dgv_Details.RowsAdded
        Dim vCloID As Integer = 0
        Dim vSrtNo As String = ""
        Dim vRolNo As String = ""
        Dim vPcNo As String = ""
        Dim vPCSUBNO As String = ""


        If IsNothing(dgv_Details.CurrentCell) Then Exit Sub

        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable

        With dgv_Details

            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1186" Then '---- UNITED WEAVES (PALLADAM)

                vCloID = Common_Procedures.Cloth_NameToIdNo(con, cbo_ClothName.Text)
                vSrtNo = Common_Procedures.get_FieldValue(con, "Cloth_Head", "Sort_No", "(Cloth_IdNo = " & Str(Val(vCloID)) & ")")

                If e.RowIndex = 0 Then

                    vRolNo = ""
                    Da = New SqlClient.SqlDataAdapter("select * from Weaver_ClothReceipt_Piece_Details Where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Weaver_ClothReceipt_Code LIKE '" & Trim(Pk_Condition) & "%/" & Trim(Common_Procedures.FnYearCode) & "' and Cloth_Idno = " & Str(Val(vCloID)) & "  Order by Weaver_ClothReceipt_Date, PieceNo_OrderBy, Piece_No", con)
                    Dt1 = New DataTable
                    Da.Fill(Dt1)
                    If Dt1.Rows.Count > 0 Then
                        vPcNo = Dt1.Rows(0).Item("Piece_No").ToString
                        vRolNo = get_RollNo_from_PieceNo_for_CC1186(vPcNo)
                    End If
                    Dt1.Clear()

                    vRolNo = Trim(Val(vRolNo)) + 1

                    .Rows(e.RowIndex).Cells(dgvCol_Details.Pcsno).Value = Trim(Common_Procedures.FnYearCode) & ":" & Trim(vSrtNo) & ":" & Trim(vRolNo)

                Else

                    vPcNo = .Rows(e.RowIndex - 1).Cells(dgvCol_Details.Pcsno).Value
                    vRolNo = get_RollNo_from_PieceNo_for_CC1186(vPcNo)

                    vRolNo = Trim(Val(vRolNo)) + 1

                    .Rows(e.RowIndex).Cells(dgvCol_Details.Pcsno).Value = Trim(Common_Procedures.FnYearCode) & ":" & Trim(vSrtNo) & ":" & Trim(vRolNo)

                End If


            ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1428" Then '---- SAKTHI VINAYAGA TEXTILES  (ERODE-PALLIPALAYAM)

                'vCloID = Common_Procedures.Cloth_NameToIdNo(con, cbo_ClothName.Text)
                ''vSrtNo = Common_Procedures.get_FieldValue(con, "Cloth_Head", "Sort_No", "(Cloth_IdNo = " & Str(Val(vCloID)) & ")")

                'If e.RowIndex = 0 Then

                '    vRolNo = ""
                '    Da = New SqlClient.SqlDataAdapter("select Piece_No from Weaver_ClothReceipt_Piece_Details Where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Weaver_ClothReceipt_Code LIKE '" & Trim(Pk_Condition) & "%/" & Trim(Common_Procedures.FnYearCode) & "' Order by Weaver_Piece_Checking_Date DESC, PieceNo_OrderBy DESC, Piece_No DESC", con)
                '    Dt1 = New DataTable
                '    Da.Fill(Dt1)
                '    If Dt1.Rows.Count > 0 Then
                '        vPcNo = Dt1.Rows(0).Item("Piece_No").ToString
                '        vRolNo = Val(vPcNo)
                '    End If
                '    Dt1.Clear()


                '    vRolNo = Trim(Val(vRolNo) + 1) & "A"

                '    .Rows(e.RowIndex).Cells(dgvCol_Details.Pcsno).Value = Trim(vRolNo)

                'Else

                '    vPcNo = Trim(.Rows(e.RowIndex - 1).Cells(dgvCol_Details.Pcsno).Value)
                '    vRolNo = Val(vPcNo)

                '    vPCSUBNO = ""
                '    For k = Len(vPcNo) To 1 Step -1

                '        If IsNumeric(Mid(vPcNo, k, 1)) = False Then
                '            vPCSUBNO = Chr(Asc(Mid(vPcNo, k, 1)) + 1)
                '            Exit For
                '        End If

                '    Next k

                '    .Rows(e.RowIndex).Cells(dgvCol_Details.Pcsno).Value = Trim(vRolNo) & Trim(vPCSUBNO)

                'End If


            ElseIf Common_Procedures.settings.ClothReceipt_PieceNo_Concept = "1,2,3" Then

                If e.RowIndex = 0 Then
                    .Rows(e.RowIndex).Cells(dgvCol_Details.Pcsno).Value = 1
                Else
                    .Rows(e.RowIndex).Cells(dgvCol_Details.Pcsno).Value = Val(.Rows(e.RowIndex - 1).Cells(dgvCol_Details.Pcsno).Value) + 1
                End If


            ElseIf Trim(UCase(Common_Procedures.settings.ClothReceipt_PieceNo_Concept)) = "A,B,C" Then

                If e.RowIndex = 0 Then
                    .Rows(e.RowIndex).Cells(dgvCol_Details.Pcsno).Value = "A"
                Else
                    .Rows(e.RowIndex).Cells(dgvCol_Details.Pcsno).Value = Chr(Asc(.Rows(e.RowIndex - 1).Cells(0).Value) + 1)
                End If


            End If

        End With
    End Sub

    Private Sub cbo_Grid_ClothType_GotFocus(sender As Object, e As System.EventArgs) Handles cbo_Grid_ClothType.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "ClothType_Head", "ClothType_Name", "(ClothType_IdNo BetWeen 1 and 5)", "(ClothType_IdNo = 0)")
    End Sub

    Private Sub cbo_Grid_ClothType_KeyDown(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles cbo_Grid_ClothType.KeyDown
        With dgv_Details

            If .Rows.Count > 0 Then

                Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Grid_ClothType, Nothing, Nothing, "ClothType_Head", "ClothType_Name", "(ClothType_IdNo BetWeen 1 and 5)", "(ClothType_IdNo = 0)")

                If (e.KeyValue = 38 And cbo_Grid_ClothType.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
                    If dgv_Details.Columns(0).ReadOnly = True Then
                        If .CurrentRow.Index <= 0 Then
                            If txt_DoffMtrs.Enabled And txt_DoffMtrs.Visible Then
                                txt_DoffMtrs.Focus()
                            Else
                                If txt_Folding.Enabled Then txt_Folding.Focus() Else dtp_Date.Focus()
                            End If


                        Else
                            .Focus()
                            .CurrentCell = .Rows(.CurrentCell.RowIndex - 1).Cells(dgvCol_Details.Wgt_Mtr)
                            .CurrentCell.Selected = True

                        End If

                    Else
                        .Focus()
                        .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(dgvCol_Details.Pcsno)
                        .CurrentCell.Selected = True

                    End If

                End If

                If (e.KeyValue = 40 And cbo_Grid_ClothType.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
                    .Focus()
                    .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 1)
                End If

            End If

        End With

    End Sub

    Private Sub cbo_Grid_ClothType_KeyPress(sender As Object, e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Grid_ClothType.KeyPress
        With dgv_Details

            If .Rows.Count > 0 Then

                Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Grid_ClothType, Nothing, "ClothType_Head", "ClothType_Name", "(ClothType_IdNo BetWeen 1 and 5)", "(ClothType_IdNo = 0)")

                If Asc(e.KeyChar) = 13 Then


                    If .CurrentCell.RowIndex = .Rows.Count - 1 And Trim(.Rows(.CurrentCell.RowIndex).Cells.Item(dgvCol_Details.ClothType).Value) = "" Then

                        If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                            save_record()
                        Else
                            dtp_Date.Focus()
                        End If

                    Else
                        .Focus()
                        .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)

                    End If

                End If

            End If

        End With
    End Sub

    Private Sub cbo_Grid_ClothType_TextChanged(sender As Object, e As System.EventArgs) Handles cbo_Grid_ClothType.TextChanged
        Try
            If cbo_Grid_ClothType.Visible Then
                If IsNothing(dgv_Details.CurrentCell) Then Exit Sub
                With dgv_Details
                    If .Rows.Count > 0 Then
                        If Val(cbo_Grid_ClothType.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = dgvCol_Details.ClothType Then
                            .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_Grid_ClothType.Text)
                        End If
                    End If
                End With
            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub txt_Folding_KeyDown(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles txt_Folding.KeyDown
        If e.KeyValue = 38 Then SendKeys.Send("+{TAB}")
        If e.KeyValue = 40 Then
            dgv_Details.Focus()
            If dgv_Details.Columns(dgvCol_Details.Pcsno).ReadOnly = False Then
                dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(dgvCol_Details.Pcsno)
            Else
                dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(dgvCol_Details.ClothType)
            End If

        End If
        'SendKeys.Send("{TAB}")
    End Sub

    Private Sub txt_Folding_KeyPress(sender As Object, e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Folding.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
        If Asc(e.KeyChar) = 13 Then
            'If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
            '    save_record()
            'Else
            '    msk_Date.Focus()
            'End If
            dgv_Details.Focus()
            If dgv_Details.Columns(dgvCol_Details.Pcsno).ReadOnly = False Then
                dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(dgvCol_Details.Pcsno)
            Else
                dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(dgvCol_Details.ClothType)
            End If
        End If
    End Sub

    Private Function get_RollNo_from_PieceNo_for_CC1186(PcsNo As String) As String
        Dim i, k As Integer
        Dim vRolNo As String = ""

        vRolNo = ""
        If Trim(PcsNo) <> "" Then

            If InStr(1, PcsNo, ":") > 0 Then

                k = 0
                For i = Len(PcsNo) To 1 Step -1
                    If Trim(Mid(PcsNo, i, 1)) = ":" Then
                        k = i
                        Exit For
                    End If
                Next i

                vRolNo = Trim(Mid(PcsNo, k + 1, Len(PcsNo)))

            End If

        End If

        Return vRolNo

    End Function

    Private Sub dtp_FilterFrom_date_KeyPress(sender As Object, e As KeyPressEventArgs) Handles dtp_FilterFrom_date.KeyPress
        If Trim(UCase(e.KeyChar)) = "D" Then
            dtp_FilterFrom_date.Text = Date.Today
        End If
    End Sub

    Private Sub dtp_FilterTo_date_KeyPress(sender As Object, e As KeyPressEventArgs) Handles dtp_FilterTo_date.KeyPress
        If Trim(UCase(e.KeyChar)) = "D" Then
            dtp_FilterTo_date.Text = Date.Today
        End If
    End Sub

    Private Sub cbo_Pcs_LastPiece_Status_GotFocus(sender As Object, e As EventArgs) Handles cbo_Pcs_LastPiece_Status.GotFocus
        cbo_Pcs_LastPiece_Status.Tag = cbo_Pcs_LastPiece_Status.Text
    End Sub

    Private Sub cbo_Pcs_LastPiece_Status_KeyDown(sender As Object, e As KeyEventArgs) Handles cbo_Pcs_LastPiece_Status.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, sender, cbo_LoomNo, Nothing, "", "", "", "")

        If (e.KeyValue = 38 And cbo_Pcs_LastPiece_Status.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
            cbo_LoomNo.Focus()

        ElseIf (e.KeyValue = 40 And cbo_Pcs_LastPiece_Status.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then

            cbo_ClothName.Focus()

        End If

    End Sub

    Private Sub cbo_Pcs_LastPiece_Status_KeyPress(sender As Object, e As KeyPressEventArgs) Handles cbo_Pcs_LastPiece_Status.KeyPress
        Dim clo_ID As Integer = 0
        Dim Lm_ID As Integer = 0

        clo_ID = Common_Procedures.Cloth_NameToIdNo(con, cbo_ClothName.Text)

        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, sender, Nothing, "", "", "", "")

        If Asc(e.KeyChar) = 13 Then



            If Trim(UCase(cbo_Pcs_LastPiece_Status.Text)) = "YES" Then

                If Trim(cbo_LoomNo.Text) <> "" And (Trim(UCase(cbo_LoomNo.Text)) <> Trim(UCase(cbo_LoomNo.Tag)) Or Trim(UCase(cbo_Pcs_LastPiece_Status.Text)) <> Trim(UCase(cbo_Pcs_LastPiece_Status.Tag)) Or Trim(lbl_KnotCode.Text) = "") Then

                    Lm_ID = Common_Procedures.Loom_NameToIdNo(con, cbo_LoomNo.Text)
                    If Val(Lm_ID) <> 0 Then
                        btn_KnottingSelection_Click(sender, e)

                    Else

                        lbl_KnotCode.Text = ""
                        lbl_KnotNo.Text = ""

                        lbl_WidthType.Text = ""
                        txt_CrimpPerc.Text = ""

                        lbl_SetCode1.Text = ""
                        lbl_SetNo1.Text = ""
                        lbl_BeamNo1.Text = ""

                        lbl_SetCode2.Text = ""
                        lbl_SetNo2.Text = ""
                        lbl_BeamNo2.Text = ""

                        lbl_TotMtrs1.Text = ""
                        lbl_TotMtrs1.Text = ""

                        lbl_BalMtrs1.Text = ""
                        lbl_BalMtrs2.Text = ""

                        lbl_BeamConsPavu.Text = ""

                        'lbl_WarpMillName.Text = ""
                        'lbl_WarpLotNo.Text = ""
                        'cbo_Weft_MillName.Text = ""
                        'txt_WeftLotNo.Text = ""
                        'lbl_FabricLotNo.Text = ""


                    End If

                End If

            Else


                btn_Selection_Click(sender, e)


            End If


            cbo_ClothName.Focus()

        End If
    End Sub

    Private Sub cbo_Pcs_LastPiece_Status_LostFocus(sender As Object, e As EventArgs) Handles cbo_Pcs_LastPiece_Status.LostFocus
        If Trim(cbo_Pcs_LastPiece_Status.Text) = "" Then
            cbo_Pcs_LastPiece_Status.Text = "NO"
        ElseIf Trim(cbo_Pcs_LastPiece_Status.Text) <> "YES" And Trim(cbo_Pcs_LastPiece_Status.Text) <> "NO" Then
            cbo_Pcs_LastPiece_Status.Text = "NO"
        End If
    End Sub

    Private Sub txt_Roll_SuffixNo_KeyDown(sender As Object, e As KeyEventArgs) Handles txt_Roll_SuffixNo.KeyDown
        On Error Resume Next
        If e.KeyValue = 38 Or e.KeyValue = 40 Then
            e.Handled = True
            msk_Date.Focus()
        End If
    End Sub

    Private Sub txt_Roll_SuffixNo_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txt_Roll_SuffixNo.KeyPress
        On Error Resume Next
        If Asc(e.KeyChar) = 13 Then
            e.Handled = True
            msk_Date.Focus()
        End If
    End Sub

    Private Sub set_grid_Meters_ColumnHeading(ByVal vCLOTH_STOCKMAINTENANCE_IN As String)

        If Trim(UCase(vCLOTH_STOCKMAINTENANCE_IN)) = "CLEAR" Then
            dgv_Details.Columns(2).HeaderText = "PCS/METERS"
            dgv_Details.Columns(4).HeaderText = "WEIGHT / " & Chr(13) & "(PC/METER)"   '"GRAMS"
        ElseIf Trim(UCase(vCLOTH_STOCKMAINTENANCE_IN)) = "PCS" Then
            dgv_Details.Columns(2).HeaderText = "No. Of PCS"
            dgv_Details.Columns(4).HeaderText = "WEIGHT / PC"   '"GRAMS"
        Else
            dgv_Details.Columns(2).HeaderText = "METERS"
            dgv_Details.Columns(4).HeaderText = "WEIGHT / METER"   '"GRAMS"
        End If
    End Sub

    Private Sub get_Multiple_WeftYarn_Consumption_Count_Details(ByVal CloID As Integer)
        Dim Da4 As New SqlClient.SqlDataAdapter
        Dim Dt4 As New DataTable
        Dim n, I As Integer
        Dim vMULTIWEFTSTS As String = 0

        dgv_Weft_Consumption_Details.Rows.Clear()

        If Common_Procedures.settings.Cloth_WeftConsumption_Multiple_WeftCount_Status = 1 Then '---- SOTEXPA QUALIDIS TEXTILE (SULUR)

            If CloID = 0 Then
                If Trim(cbo_ClothName.Text) <> "" Then
                    CloID = Common_Procedures.Cloth_NameToIdNo(con, cbo_ClothName.Text)
                End If
            End If

            vMULTIWEFTSTS = Common_Procedures.get_FieldValue(con, "cloth_Head", "Multiple_WeftCount_Status", "(cloth_idno = " & Str(Val(CloID)) & ")")
            If Val(vMULTIWEFTSTS) = 1 Then


                Da4 = New SqlClient.SqlDataAdapter("Select a.*, b.count_name from Cloth_Additional_Weft_Details a INNER JOIN Count_Head b ON a.Count_IdNo = b.Count_IdNo Where Cloth_Idno = " & Str(Val(CloID)), con)
                Dt4 = New DataTable
                Da4.Fill(Dt4)
                If Dt4.Rows.Count > 0 Then
                    For I = 0 To Dt4.Rows.Count - 1

                        n = dgv_Weft_Consumption_Details.Rows.Add()
                        dgv_Weft_Consumption_Details.Rows(n).Cells(0).Value = Dt4.Rows(I).Item("count_name").ToString
                        dgv_Weft_Consumption_Details.Rows(n).Cells(1).Value = Dt4.Rows(I).Item("Gram_Perc_Type").ToString
                        dgv_Weft_Consumption_Details.Rows(n).Cells(2).Value = Val(Dt4.Rows(I).Item("Consumption_Gram_Perc").ToString)
                        dgv_Weft_Consumption_Details.Rows(n).Cells(3).Value = ""

                    Next

                End If
                Dt4.Clear()

            End If

        End If

    End Sub

    Private Sub btn_Show_WeftConsumption_Details_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Show_WeftConsumption_Details.Click
        pnl_back.Enabled = False
        pnl_Weft_Consumption_Details.Visible = True
        If dgv_Weft_Consumption_Details.Rows.Count > 0 Then
            dgv_Weft_Consumption_Details.Focus()
            dgv_Weft_Consumption_Details.CurrentCell = dgv_Weft_Consumption_Details.Rows(0).Cells(0)
        End If
    End Sub

    Private Sub btn_Close_Weft_Consumption_Details_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Close_Weft_Consumption_Details.Click
        pnl_back.Enabled = True
        pnl_Weft_Consumption_Details.Visible = False

    End Sub

End Class