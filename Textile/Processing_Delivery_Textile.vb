Imports System.IO
Imports System.Runtime.Remoting
Public Class Processing_Delivery_Textile
    Implements Interface_MDIActions

    Private con As New SqlClient.SqlConnection(Common_Procedures.Connection_String)
    Private con1 As New SqlClient.SqlConnection(Common_Procedures.ConnectionString_CompanyGroupdetails)
    Private FrmLdSTS As Boolean = False
    Private New_Entry As Boolean = False
    Private Insert_Entry As Boolean = False
    Private Filter_Status As Boolean = False
    Private Pk_Condition As String = "TPDEL-"
    Private Pk_Condition2 As String = "FPRDC-"
    Private Pk_Condition3 As String = ""
    Private Prec_ActCtrl As New Control
    Private dgv_ActCtrlName As String = ""

    Private SaveAll_STS As Boolean = False
    Private LastNo As String = ""

    Private vcbo_KeyDwnVal As Double
    Private prn_HdDt As New DataTable
    Private prn_DetDt As New DataTable
    Private prn_PageNo As Integer

    Private prn_DetailsIndex As Integer = 0
    Private prn_DetailsCount As Integer = 0
    Private ChkPrintRow As Integer = 0

    Private prn_DetIndx As Integer
    Private prn_DetSNo As Integer
    Private prn_Status As Integer = 0
    Private prn_DetAr(1000, 50, 10) As String
    Private prn_InpOpts As String = ""
    Private prn_OriDupTri As String = ""
    Private Total_mtrs As Single = 0
    Private Total_wEIGHT As Double = 0
    Private Total_PCS As Double = 0

    Private Total_Mtrs_Abv80 As Single = 0
    Private Total_Mtrs_40To79 As Double = 0
    Private Total_Mtrs_20To40 As Double = 0

    Private prn_DetMxIndx As Integer
    Private prn_NoofBmDets As Integer
    Private NoFo_STS As Integer = 0
    Private prn_HdIndx As Integer
    Private prn_HdMxIndx As Integer
    Private prn_Count As Integer
    Private prn_123Count As Integer
    Private prn_HdAr(1000, 10) As String
    Private vPrn_PvuEdsCnt As String
    Private vPrn_PvuTotBms As Integer
    Private vPrn_PvuTotMtrs As Single
    Private vPrn_PvuSetNo As String
    Private vPrn_PvuBmNos1 As String
    Private vPrn_PvuBmNos2 As String
    Private vPrn_PvuBmNos3 As String
    Private vPrn_PvuBmNos4 As String
    Private WithEvents dgtxt_details As New DataGridViewTextBoxEditingControl
    Private WithEvents dgtxt_Pieces_BaleNo_EntryDetails As New DataGridViewTextBoxEditingControl
    Private vPcsSelc_STatus As Boolean = False



    Private dgv_LevColNo As Integer = 0

    Public Shared EntFnYrCode As String = ""

    Dim Last_Effective_Grid_Col As Int16 = 14

    Dim Process_Inputs As String
    Dim Process_Outputs As String

    Private vCLO_CONDT As String = ""
    Private FnYearCode1 As String = ""
    Private FnYearCode2 As String = ""

    Private Enum dgvCol_Details As Integer
        SLNO                        '0
        ITEM_GREY                   '1
        COLOUR_DELY                 '2
        ITEM_FP                     '3
        COLOUR                      '4
        LOT_NO                      '5
        PROCESSING                  '6
        BALES                       '7
        BALES_NOS                   '8
        PCS                         '9
        QTY                         '10
        MTR_QTY                    '11
        METERS                     '12
        WEIGHT                     '13
        RATE                       '14
        AMOUNT                     '15
        CLOTH_PROCESSING_DELIVERY_SLNO                  '16   '---- NOT Auto_Slno , USED IN PROCESSING RECEIPT ENTRY FOR DELIVERY SELECTION
        RECEIPT_METERS                                  '17
        RETURN_MTRS                                     '18
        PACKINGSLIP_CODE                                '19
        CLOTH_PROCESSING_DELIVERY_PACKINGSLIP_SLNO      '20   '----NOT Auto_Slno , USED IN PACKINGSLIP SELECTION
        PIECE_SELECTION_LOTCODE_PCSNOS           '21
    End Enum
    Private Enum dgvCol_BaleSelection As Integer
        SLNO                         '0
        BALE_NO                      '1
        PCS                          '2
        METERS                       '3
        WEIGHT                       '4
        STS                          '5
        PACKING_SLIP_CODE            '6
        BALE_BUNDLE                  '7
    End Enum

    Private Enum dgvCol_BaleSeledetails As Integer
        CLOTH_PROCESSING_DELIVERY_PACKINGSLIP_SLNO  '0
        BALE_NO                                     '1
        PCS                                         '2
        METERS                                      '3
        WEIGHT                                      '4
        PACKING_SLIP_CODE                           '5
        BALE_BUNDLE                                 '6
    End Enum

    Private Enum dgvCol_Filterdetails As Integer
        REF_NO           '0
        FILTER_DATE      '1
        PARTY_NAME       '2
        ITEM_NAME_GREY   '3
        PROCESSING       '4
        PCS              '5
        QTY              '6
        METERS           '7
        WEIGHT           '8
        DC_NO            '9              
    End Enum
    Private Enum dgvCol_PieceDetails As Integer

        CLOTH_PROCESSING_DELIVERY_PACKINGSLIP_SLNO  '0
        LOT_NO                                      '1
        PCS_NO                                      '2
        CLOTH_TYPE                                  '3
        METERS                                      '4
        WEIGHT                                      '5
        WEIGHT_METER                                '6
        PCS_PARTY_NAME                              '7
        LOT_CODE                                    '8
        CLOTH_NAME                                  '9
        LOOM_NO                                     '10
        BALE_NO                                     '11

    End Enum
    Private Enum dgvCol_PieceSelection As Integer

        SLNO                                    '0
        LOT_NO                                  '1
        PCS_NO                                  '2
        CLOTH_TYPE                              '3
        METERS                                  '4
        WEIGHT                                  '5
        WEIGHT_METER                            '6
        PCS_PARTY_NAME                          '7
        STS                                     '8
        LOT_CODE                                '9
        CLOTH_IDNO                              '10
        LOOM_NO                                 '11
        BAR_CODE                                '12
        ENTRY_BALE_NO                           '14
    End Enum
    Private Sub clear()

        New_Entry = False
        Insert_Entry = False

        pnl_Back.Enabled = True
        pnl_Filter.Visible = False
        pnl_BaleSelection.Visible = False

        lbl_DcNo.Text = ""
        lbl_DcNo.ForeColor = Color.Black

        dtp_Date.Text = ""
        txt_PoNo.Text = ""
        cbo_Ledger.Text = ""

        cbo_TransportName.Text = ""
        Cbo_ProcessingHEAD.Text = ""
        CBO_JobNO.Text = ""
        cbo_VehicleNo.Text = ""

        txt_Frieght.Text = ""
        txt_Note.Text = ""
        txt_filterpono.Text = ""
        lbl_UserName.Text = "USER : " & Trim(UCase(Common_Procedures.User_IdNoToName(con1, Common_Procedures.User.IdNo)))
        cbo_Ledger.Enabled = True
        cbo_Ledger.BackColor = Color.White

        txt_PoNo.Enabled = True
        txt_PoNo.BackColor = Color.White

        cbo_Colour.Enabled = True
        cbo_Colour.BackColor = Color.White

        cbo_itemfp.Enabled = True
        cbo_itemfp.BackColor = Color.White

        cbo_itemgrey.Enabled = True
        cbo_itemgrey.BackColor = Color.White

        cbo_Processing.Enabled = True
        cbo_Processing.BackColor = Color.White

        cbo_LotNo.Enabled = True
        cbo_LotNo.BackColor = Color.White

        Process_Outputs = ""
        Process_Inputs = ""
        lbl_Lot_No_Discribtion.Text = ""

        dgv_Details.Rows.Clear()
        dgv_Details_Total.Rows.Clear()
        dgv_Details.Rows(0).Cells(dgvCol_Details.CLOTH_PROCESSING_DELIVERY_PACKINGSLIP_SLNO).Value = 0
        dgv_BaleSelectionDetails.Rows.Clear()

        Grid_DeSelect()

        cbo_itemgrey.Visible = False
        cbo_itemfp.Visible = False
        cbo_Colour.Visible = False
        cbo_LotNo.Visible = False
        cbo_Processing.Visible = False

        cbo_itemgrey.Tag = -1
        cbo_itemfp.Tag = -1
        cbo_Colour.Tag = -1
        cbo_LotNo.Tag = -1
        cbo_Processing.Tag = -1

        cbo_itemgrey.Text = ""
        cbo_itemfp.Text = ""
        cbo_Colour.Text = ""
        cbo_LotNo.Text = ""
        cbo_Processing.Text = ""

        dgv_Details.Tag = ""
        dgv_LevColNo = -1

        txt_Folding.Text = 100


        txt_EWBNo.Text = ""
        Grp_EWB.Visible = False
        chk_GSTTax_Invocie.Checked = True
        chk_Ewb_No_Sts.Checked = False

        cbo_ClothSales_OrderCode_forSelection.Text = ""

        txt_DcPrefixNo.Text = ""
        cbo_DcSufixNo.Text = "" ' "/" & Common_Procedures.FnYearCode

        chk_LotClose.Checked = False

        pnl_PieceSelection.Visible = False
        pnl_PieceSelection_ToolTip.Visible = False
        dgv_PieceSelection.Rows.Clear()
        dgv_PieceDetails.Rows.Clear()



    End Sub

    Private Sub Grid_DeSelect()
        On Error Resume Next
        If Not IsNothing(dgv_Details.CurrentCell) Then dgv_Details.CurrentCell.Selected = False
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

        If Me.ActiveControl.Name <> cbo_Colour.Name Then
            cbo_Colour.Visible = False
        End If
        If Me.ActiveControl.Name <> cbo_itemfp.Name Then
            cbo_itemfp.Visible = False
        End If
        If Me.ActiveControl.Name <> cbo_itemgrey.Name Then
            cbo_itemgrey.Visible = False
        End If
        If Me.ActiveControl.Name <> cbo_Processing.Name Then
            cbo_Processing.Visible = False
        End If
        If Me.ActiveControl.Name <> cbo_LotNo.Name Then
            cbo_LotNo.Visible = False
        End If
        If Me.ActiveControl.Name <> dgv_Details_Total.Name Then
            Grid_DeSelect()
        End If

        If Me.ActiveControl.Name <> dgv_Details.Name Then
            Common_Procedures.Hide_CurrentStock_Display()
        End If
        If Me.ActiveControl.Name <> dgv_Details.Name And Not (TypeOf ActiveControl Is DataGridViewTextBoxEditingControl) Then
            pnl_BaleSelection_ToolTip.Visible = False
            pnl_PieceSelection_ToolTip.Visible = False
        End If

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
        dgv_Details.CurrentCell.Selected = False
        dgv_Details_Total.CurrentCell.Selected = False
        dgv_Filter_Details.CurrentCell.Selected = False
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

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(no) & "/" & Trim(EntFnYrCode)

        Try

            da1 = New SqlClient.SqlDataAdapter("select a.*, b.Ledger_Name , c.Ledger_Name as Transport_Name , f.* ,G.Ledger_Name as Godown_Name from Textile_Processing_Delivery_Head a INNER JOIN Ledger_Head b ON a.Ledger_IdNo = b.Ledger_IdNo LEFT OUTER JOIN Ledger_Head c ON a.Transport_IdNo = c.Ledger_IdNo LEFT OUTER JOIN Process_Head f ON f.Process_IdNo = a.Processing_Idno left outer join Ledger_Head g on a.Delivery_From_Godown_IdNo = g.Ledger_IdNo Where a.ClothProcess_Delivery_Code = '" & Trim(NewCode) & "' and ClothProcess_Delivery_Code not like '" & Trim(Pk_Condition2) & Trim(NewCode) & "%' ", con)
            dt1 = New DataTable
            da1.Fill(dt1)

            If dt1.Rows.Count > 0 Then
                txt_DcPrefixNo.Text = dt1.Rows(0).Item("ClothProcess_Delivery_PrefixNo").ToString
                lbl_DcNo.Text = dt1.Rows(0).Item("ClothProcess_Delivery_RefNo").ToString
                cbo_DcSufixNo.Text = dt1.Rows(0).Item("ClothProcess_Delivery_SuffixNo").ToString


                'lbl_DcNo.Text = dt1.Rows(0).Item("ClothProcess_Delivery_No").ToString

                dtp_Date.Text = dt1.Rows(0).Item("ClothProcess_Delivery_Date").ToString
                cbo_Ledger.Text = dt1.Rows(0).Item("Ledger_Name").ToString
                txt_PoNo.Text = dt1.Rows(0).Item("Purchase_OrderNo").ToString
                cbo_TransportName.Text = dt1.Rows(0).Item("Transport_Name").ToString
                txt_Frieght.Text = Format(Val(dt1.Rows(0).Item("Freight_Charges").ToString), "########0.00")
                txt_Note.Text = dt1.Rows(0).Item("Note").ToString
                Cbo_ProcessingHEAD.Text = dt1.Rows(0).Item("Process_Name").ToString

                cbo_ClothSales_OrderCode_forSelection.Text = dt1.Rows(0).Item("ClothSales_OrderCode_forSelection").ToString

                Dim Process_Inputs_Tmp As String = ""
                Dim Process_Outputs_Tmp As String = ""

                If dt1.Rows.Count > 0 Then

                    If Not IsDBNull(dt1.Rows(0).Item("Cloth_Delivered")) Then
                        Process_Inputs_Tmp = IIf(dt1.Rows(0).Item("Cloth_Delivered") = True, "1", "0")
                    Else
                        Process_Inputs_Tmp = "0"
                    End If

                    If Not IsDBNull(dt1.Rows(0).Item("FP_Delivered")) Then
                        Process_Inputs_Tmp = Process_Inputs_Tmp + IIf(dt1.Rows(0).Item("FP_Delivered") = True, "1", "0")
                    Else
                        Process_Inputs_Tmp = Process_Inputs_Tmp + "0"
                    End If


                    If Not IsDBNull(dt1.Rows(0).Item("Cloth_Returned")) Then
                        Process_Outputs_Tmp = IIf(dt1.Rows(0).Item("Cloth_Returned") = True, "1", "0")
                    Else
                        Process_Outputs_Tmp = "0"
                    End If

                    If Not IsDBNull(dt1.Rows(0).Item("FP_Returned")) Then
                        Process_Outputs_Tmp = Process_Outputs_Tmp + IIf(dt1.Rows(0).Item("FP_Returned") = True, "1", "0")
                    Else
                        Process_Outputs_Tmp = Process_Outputs_Tmp + "0"
                    End If

                End If

                Process_Inputs = Process_Inputs_Tmp
                Process_Outputs = Process_Outputs_Tmp

                Dim RET_TYPE As String = "CLOTH"

                If Len(Trim(Process_Outputs)) > 1 Then
                    If Mid(Trim(Process_Outputs), 2, 1) = "1" Then
                        RET_TYPE = "FP"
                    End If
                End If

                lbl_UserName.Text = "USER : " & Trim(UCase(Common_Procedures.User_IdNoToName(con1, Val(dt1.Rows(0).Item("User_IdNo").ToString))))
                cbo_VehicleNo.Text = dt1.Rows(0).Item("Vehicle_No").ToString

                If Not IsDBNull(dt1.Rows(0).Item("Folding")) Then
                    txt_Folding.Text = Format(dt1.Rows(0).Item("Folding"), "####0.0")
                End If

                If Not IsDBNull(dt1.Rows(0).Item("Godown_Name")) Then
                    cbo_Delivery_From.Text = dt1.Rows(0).Item("Godown_Name")
                End If

                If Not IsDBNull(dt1.Rows(0).Item("Lot_IdNo")) Then
                    CBO_JobNO.Text = Common_Procedures.Lot_IdNoToNo(con, dt1.Rows(0).Item("Lot_IdNo"))
                End If

                If Len(Trim(CBO_JobNO.Text)) = 0 Then
                    CBO_JobNO.Text = dt1.Rows(0).Item("JobOrder_No").ToString
                End If

                txt_EWBNo.Text = dt1.Rows(0).Item("EwayBill_No").ToString
                If Trim(txt_EWBNo.Text) <> "" Then
                    chk_Ewb_No_Sts.Checked = True
                Else
                    chk_Ewb_No_Sts.Checked = False
                End If


                If Val(dt1.Rows(0).Item("Lot_Close_Status").ToString) = 1 Then
                    chk_LotClose.Checked = True
                Else
                    chk_LotClose.Checked = False
                End If

                da2 = New SqlClient.SqlDataAdapter("select a.*, b.Cloth_Name as Grey_Item_name , C.Cloth_Name as Fp_Item_Name,d.Colour_Name,e.Lot_No,f.Process_Name,PI.Processed_Item_Name as Fin_Pro,dc.Colour_Name as Del_Colour_Name from Textile_Processing_Delivery_Details a left outer join Cloth_Head b ON  b.Cloth_Idno = a.Item_Idno LEFT OUTER JOIN Cloth_Head C ON c.Cloth_Idno = a.Item_To_Idno LEFT OUTER JOIN Colour_Head d ON d.Colour_IdNo = a.Colour_IdNo LEFT OUTER JOIN Lot_Head e ON e.Lot_IdNo = a.Lot_IdNo LEFT OUTER JOIN Process_Head f ON f.Process_IdNo = a.Processing_Idno left outer join  Processed_Item_Head PI on a.Item_To_Idno = pi.Processed_Item_IdNo left outer join Colour_Head dc on a.Del_Colour_IdNo = dc.Colour_IdNo where a.Cloth_Processing_Delivery_Code = '" & Trim(NewCode) & "' and a.Cloth_Processing_Delivery_Code not like '" & Trim(Pk_Condition2) & "%' Order by Sl_No", con)
                dt2 = New DataTable
                da2.Fill(dt2)

                'dgv_Details.Rows.Clear()
                SNo = 0

                If dt2.Rows.Count > 0 Then

                    For i = 0 To dt2.Rows.Count - 1

                        n = dgv_Details.Rows.Add()

                        SNo = SNo + 1

                        dgv_Details.Rows(n).Cells(dgvCol_Details.SLNO).Value = Val(SNo)
                        dgv_Details.Rows(n).Cells(dgvCol_Details.ITEM_GREY).Value = dt2.Rows(i).Item("Grey_Item_Name").ToString

                        If RET_TYPE = "CLOTH" Then
                            dgv_Details.Rows(n).Cells(dgvCol_Details.ITEM_FP).Value = dt2.Rows(i).Item("Fp_Item_Name").ToString
                        Else
                            If Not IsDBNull(dt2.Rows(0).Item("Fin_Pro")) Then
                                dgv_Details.Rows(n).Cells(dgvCol_Details.ITEM_FP).Value = dt2.Rows(i).Item("Fin_Pro").ToString
                            End If
                        End If

                        dgv_Details.Rows(n).Cells(dgvCol_Details.COLOUR).Value = dt2.Rows(i).Item("Colour_Name").ToString
                        dgv_Details.Rows(n).Cells(dgvCol_Details.LOT_NO).Value = dt2.Rows(i).Item("Lot_No").ToString
                        dgv_Details.Rows(n).Cells(dgvCol_Details.PROCESSING).Value = dt2.Rows(i).Item("Process_Name").ToString

                        dgv_Details.Rows(n).Cells(dgvCol_Details.BALES).Value = Val(dt2.Rows(i).Item("Bales").ToString)
                        dgv_Details.Rows(n).Cells(dgvCol_Details.BALES_NOS).Value = dt2.Rows(i).Item("Bales_Nos").ToString

                        dgv_Details.Rows(n).Cells(dgvCol_Details.PCS).Value = Format(Val(dt2.Rows(i).Item("Delivery_Pcs").ToString), "########0.00")
                        dgv_Details.Rows(n).Cells(dgvCol_Details.QTY).Value = Val(dt2.Rows(i).Item("Delivery_Qty").ToString)
                        dgv_Details.Rows(n).Cells(dgvCol_Details.MTR_QTY).Value = Format(Val(dt2.Rows(i).Item("Meter_Qty").ToString), "########0.00")
                        dgv_Details.Rows(n).Cells(dgvCol_Details.METERS).Value = Format(Val(dt2.Rows(i).Item("Delivery_Meters").ToString), "########0.00")

                        dgv_Details.Rows(n).Cells(dgvCol_Details.WEIGHT).Value = Format(Val(dt2.Rows(i).Item("Delivery_Weight").ToString), "########0.000")

                        dgv_Details.Rows(n).Cells(dgvCol_Details.CLOTH_PROCESSING_DELIVERY_SLNO).Value = dt2.Rows(i).Item("Cloth_Processing_Delivery_Slno").ToString
                        dgv_Details.Rows(n).Cells(dgvCol_Details.RECEIPT_METERS).Value = dt2.Rows(i).Item("Receipt_Meters").ToString
                        dgv_Details.Rows(n).Cells(dgvCol_Details.RETURN_MTRS).Value = dt2.Rows(i).Item("Return_Meters").ToString
                        dgv_Details.Rows(n).Cells(dgvCol_Details.PACKINGSLIP_CODE).Value = dt2.Rows(i).Item("PackingSlip_Codes").ToString
                        dgv_Details.Rows(n).Cells(dgvCol_Details.CLOTH_PROCESSING_DELIVERY_PACKINGSLIP_SLNO).Value = dt2.Rows(i).Item("ClothProcessing_Delivery_PackingSlno").ToString

                        If Not IsDBNull(dt2.Rows(i).Item("Del_Colour_Name")) Then
                            dgv_Details.Rows(n).Cells(dgvCol_Details.COLOUR_DELY).Value = dt2.Rows(i).Item("Del_Colour_Name").ToString
                        End If

                        If Val(dgv_Details.Rows(n).Cells(dgvCol_Details.RECEIPT_METERS).Value) <> 0 Or Val(dgv_Details.Rows(n).Cells(dgvCol_Details.RETURN_MTRS).Value) <> 0 Then
                            For j = 0 To dgv_Details.ColumnCount - 1
                                dgv_Details.Rows(n).Cells(j).Style.BackColor = Color.LightGray
                            Next j
                            LockSTS = True
                        End If


                        dgv_Details.Rows(n).Cells(dgvCol_Details.RATE).Value = Val(dt2.Rows(i).Item("Rate").ToString)
                        dgv_Details.Rows(n).Cells(dgvCol_Details.AMOUNT).Value = Val(dt2.Rows(i).Item("Amount").ToString)
                        If Val(dt1.Rows(0).Item("GST_Tax_Invoice_Status").ToString) = 1 Then chk_GSTTax_Invocie.Checked = True Else chk_GSTTax_Invocie.Checked = False

                    Next i

                End If

                If dgv_Details.Rows.Count = 0 Then
                    dgv_Details.Rows.Add()
                Else

                    n = dgv_Details.Rows.Count - 1
                    If Trim(dgv_Details.Rows(n).Cells(dgvCol_Details.ITEM_GREY).Value) = "" Then
                        dgv_Details.Rows(n).Cells(dgvCol_Details.CLOTH_PROCESSING_DELIVERY_SLNO).Value = ""
                        If Val(dgv_Details.Rows(n).Cells(dgvCol_Details.CLOTH_PROCESSING_DELIVERY_SLNO).Value) = 0 Then
                            If n = 0 Then
                                dgv_Details.Rows(n).Cells(dgvCol_Details.CLOTH_PROCESSING_DELIVERY_SLNO).Value = 1
                            Else
                                dgv_Details.Rows(n).Cells(dgvCol_Details.CLOTH_PROCESSING_DELIVERY_SLNO).Value = Val(dgv_Details.Rows(n - 1).Cells(dgvCol_Details.CLOTH_PROCESSING_DELIVERY_SLNO).Value) + 1
                            End If
                        End If
                    End If

                End If


                With dgv_Details_Total
                    If .RowCount = 0 Then .Rows.Add()
                    .Rows(0).Cells(dgvCol_Details.BALES).Value = Val(dt1.Rows(0).Item("Total_Pcs").ToString)
                    .Rows(0).Cells(dgvCol_Details.BALES_NOS).Value = Val(dt1.Rows(0).Item("Total_Qty").ToString)
                    .Rows(0).Cells(dgvCol_Details.QTY).Value = Format(Val(dt1.Rows(0).Item("Total_Meters").ToString), "########0.00")
                    .Rows(0).Cells(dgvCol_Details.WEIGHT).Value = Format(Val(dt1.Rows(0).Item("Total_Weight").ToString), "########0.000")
                    .Rows(0).Cells(dgvCol_Details.AMOUNT).Value = Format(Val(dt1.Rows(0).Item("Total_Amount").ToString), "########0.000")
                End With

                da2 = New SqlClient.SqlDataAdapter("Select a.* from Packing_Slip_Head a Where a.Delivery_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' Order by a.Delivery_DetailsSlNo, a.Delivery_No, a.Packing_Slip_Date, a.for_OrderBy, a.Packing_Slip_No, a.Packing_Slip_Code", con)
                dt2 = New DataTable
                da2.Fill(dt2)

                With dgv_BaleSelectionDetails

                    .Rows.Clear()
                    SNo = 0

                    If dt2.Rows.Count > 0 Then

                        For i = 0 To dt2.Rows.Count - 1

                            n = .Rows.Add()

                            SNo = SNo + 1

                            .Rows(n).Cells(dgvCol_BaleSeledetails.CLOTH_PROCESSING_DELIVERY_PACKINGSLIP_SLNO).Value = Val(dt2.Rows(i).Item("Delivery_DetailsSlNo").ToString)
                            .Rows(n).Cells(dgvCol_BaleSeledetails.BALE_NO).Value = dt2.Rows(i).Item("Packing_Slip_No").ToString
                            .Rows(n).Cells(dgvCol_BaleSeledetails.PCS).Value = Val(dt2.Rows(i).Item("Total_Pcs").ToString)
                            .Rows(n).Cells(dgvCol_BaleSeledetails.METERS).Value = Val(dt2.Rows(i).Item("Total_Meters").ToString)
                            .Rows(n).Cells(dgvCol_BaleSeledetails.WEIGHT).Value = Val(dt2.Rows(i).Item("Total_Weight").ToString)
                            .Rows(n).Cells(dgvCol_BaleSeledetails.PACKING_SLIP_CODE).Value = dt2.Rows(i).Item("Packing_Slip_Code").ToString
                            .Rows(n).Cells(dgvCol_BaleSeledetails.BALE_BUNDLE).Value = dt2.Rows(i).Item("Bale_Bundle").ToString

                        Next i

                    End If

                End With


                ' ------

                dt2.Clear()

                dt2.Dispose()
                da2.Dispose()


                da2 = New SqlClient.SqlDataAdapter("Select a.*, b.Cloth_Name as  PieceCloth_Name, c.ClothType_Name as PieceType_Name, d.ledger_name as PieceParty_Name from Textile_Processing_Delivery_Piece_Details a LEFT OUTER JOIN Cloth_Head b ON a.PieceCloth_IdNo = b.Cloth_IdNo LEFT OUTER JOIN ClothType_Head c ON a.PieceType_IdNo = c.ClothType_IdNo LEFT OUTER JOIN Ledger_Head d ON a.PieceParty_IdNo <> 0 and a.PieceParty_IdNo = d.Ledger_Idno Where a.ClothProcess_Delivery_Code = '" & Trim(NewCode) & "' Order by a.ClothProcessing_Delivery_PackingSlno, a.sl_no", con)
                dt2 = New DataTable
                da2.Fill(dt2)

                With dgv_PieceDetails

                    .Rows.Clear()

                    If dt2.Rows.Count > 0 Then

                        For i = 0 To dt2.Rows.Count - 1

                            n = .Rows.Add()

                            .Rows(n).Cells(0).Value = dt2.Rows(i).Item("ClothProcessing_Delivery_PackingSlno").ToString
                            .Rows(n).Cells(1).Value = dt2.Rows(i).Item("Lot_No").ToString
                            .Rows(n).Cells(2).Value = dt2.Rows(i).Item("Piece_No").ToString
                            .Rows(n).Cells(3).Value = dt2.Rows(i).Item("PieceType_Name").ToString
                            .Rows(n).Cells(4).Value = Format(Val(dt2.Rows(i).Item("Meters").ToString), "########0.00")
                            .Rows(n).Cells(5).Value = Format(Val(dt2.Rows(i).Item("Weight").ToString), "########0.000")
                            .Rows(n).Cells(6).Value = Format(Val(dt2.Rows(i).Item("Weight_Meter").ToString), "########0.000")

                            .Rows(n).Cells(7).Value = dt2.Rows(i).Item("PieceParty_Name").ToString

                            .Rows(n).Cells(8).Value = dt2.Rows(i).Item("lot_code").ToString
                            .Rows(n).Cells(9).Value = dt2.Rows(i).Item("PieceCloth_Name").ToString

                            .Rows(n).Cells(10).Value = dt2.Rows(i).Item("Loom_No").ToString

                            .Rows(n).Cells(11).Value = dt2.Rows(i).Item("Bale_No").ToString


                        Next i

                    End If

                End With



                Grid_DeSelect()

                dt2.Clear()

                dt2.Dispose()
                da2.Dispose()

            End If

            dt1.Clear()
            dt1.Dispose()
            da1.Dispose()

            If LockSTS = True Then

                cbo_Ledger.Enabled = False
                cbo_Ledger.BackColor = Color.LightGray

                txt_PoNo.Enabled = False
                txt_PoNo.BackColor = Color.LightGray

                cbo_Colour.Enabled = False
                cbo_Colour.BackColor = Color.LightGray

                cbo_itemfp.Enabled = False
                cbo_itemfp.BackColor = Color.LightGray

                cbo_itemgrey.Enabled = False
                cbo_itemgrey.BackColor = Color.LightGray

                cbo_Processing.Enabled = False
                cbo_Processing.BackColor = Color.LightGray

                cbo_LotNo.Enabled = False
                cbo_LotNo.BackColor = Color.LightGray

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

        If dtp_Date.Visible And dtp_Date.Enabled Then dtp_Date.Focus()

    End Sub

    Private Sub Processing_Delivery_Textile_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated

        Try

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Ledger.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "LEDGER" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Ledger.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_TransportName.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "LEDGER" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_TransportName.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_itemfp.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "CLOTH" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_itemfp.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_itemgrey.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "CLOTH" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_itemgrey.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Colour.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "COLOUR" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Colour.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_LotNo.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "LOT" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                CBO_JobNO.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Processing.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "PROCESS" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Processing.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Delivery_From.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "LEDGER" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Delivery_From.Text = Trim(Common_Procedures.Master_Return.Return_Value)
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

    Private Sub Processing_Delivery_Textile_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim OpYrCode As String = ""

        Me.Text = ""

        If Trim(UCase(Common_Procedures.Proc_Opening_OR_Entry)) = "OPENING" Then
            OpYrCode = Microsoft.VisualBasic.Left(Trim(Common_Procedures.FnRange), 4)
            OpYrCode = Trim(Mid(Val(OpYrCode) - 1, 3, 2)) & "-" & Trim(Microsoft.VisualBasic.Right(OpYrCode, 2))

            EntFnYrCode = OpYrCode

        Else
            EntFnYrCode = Common_Procedures.FnYearCode

        End If

        con.Open()

        If Common_Procedures.settings.Hide_COLOUR_DELIVERY_In_Processing_Transactions = True Then '---- P S ENTERPRISES (KANPUR)
            dgv_Details.Columns(dgvCol_Details.COLOUR_DELY).Visible = False
            dgv_Details_Total.Columns(dgvCol_Details.COLOUR_DELY).Visible = False
        Else
            dgv_Details.Columns(dgvCol_Details.COLOUR_DELY).Visible = True
            dgv_Details_Total.Columns(dgvCol_Details.COLOUR_DELY).Visible = True
        End If





        cbo_itemfp.Visible = False
        cbo_itemfp.Visible = False
        cbo_Colour.Visible = False
        cbo_LotNo.Visible = False
        cbo_Processing.Visible = False


        dgv_PieceDetails.Visible = False
        pnl_PieceSelection_ToolTip.Visible = False

        pnl_Filter.Visible = False
        pnl_Filter.Left = (Me.Width - pnl_Filter.Width) \ 2
        pnl_Filter.Top = (Me.Height - pnl_Filter.Height) \ 2

        pnl_BaleSelection.Visible = False
        pnl_BaleSelection.Left = (Me.Width - pnl_BaleSelection.Width) \ 2
        pnl_BaleSelection.Top = (Me.Height - pnl_BaleSelection.Height) \ 2
        pnl_BaleSelection.BringToFront()

        pnl_PieceSelection.Visible = False
        pnl_PieceSelection.Left = (Me.Width - pnl_PieceSelection.Width) \ 2
        pnl_PieceSelection.Top = (Me.Height - pnl_PieceSelection.Height) \ 2
        pnl_PieceSelection.BringToFront()

        pnl_Print.Visible = False
        pnl_Print.Left = (Me.Width - pnl_Print.Width) \ 2
        pnl_Print.Top = (Me.Height - pnl_Print.Height) \ 2
        pnl_Print.BringToFront()

        cbo_DcSufixNo.Items.Clear()
        cbo_DcSufixNo.Items.Add("")
        cbo_DcSufixNo.Items.Add("/" & Common_Procedures.FnYearCode)
        cbo_DcSufixNo.Items.Add("/" & Year(Common_Procedures.Company_FromDate) & "-" & Year(Common_Procedures.Company_ToDate))
        cbo_DcSufixNo.Items.Add("/" & Year(Common_Procedures.Company_FromDate))
        cbo_DcSufixNo.Items.Add("/" & Trim(Year(Common_Procedures.Company_FromDate)) & "-" & Trim(Microsoft.VisualBasic.Right(Year(Common_Procedures.Company_ToDate), 2)))

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

        AddHandler dtp_Date.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Colour.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_itemfp.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_itemgrey.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Ledger.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_LotNo.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Processing.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_TransportName.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_filterpono.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Frieght.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Note.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_PoNo.GotFocus, AddressOf ControlGotFocus
        AddHandler Cbo_ProcessingHEAD.GotFocus, AddressOf ControlGotFocus
        AddHandler CBO_JobNO.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_VehicleNo.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Filter_ProcessName.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Filter_PartyName.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_Print_delivery.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_Print_packinglist.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_Print_Cancel.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Delivery_From.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Folding.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Colour_Dely.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_DcPrefixNo.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_DcSufixNo.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_ClothSales_OrderCode_forSelection.GotFocus, AddressOf ControlGotFocus

        AddHandler txt_PieceSelection_LotNo.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_PieceSelection_Meters.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_PieceSelection_PcsNo.GotFocus, AddressOf ControlGotFocus


        AddHandler btn_Print_Cancel.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_Print_packinglist.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_Print_delivery.LostFocus, AddressOf ControlLostFocus
        AddHandler dtp_Date.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Colour.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_itemfp.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_itemgrey.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Ledger.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_LotNo.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Processing.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_TransportName.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_filterpono.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Frieght.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Note.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_PoNo.LostFocus, AddressOf ControlLostFocus
        AddHandler Cbo_ProcessingHEAD.LostFocus, AddressOf ControlLostFocus
        AddHandler CBO_JobNO.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_VehicleNo.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Filter_ProcessName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Filter_PartyName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Delivery_From.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Folding.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Colour_Dely.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_DcPrefixNo.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_DcSufixNo.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_ClothSales_OrderCode_forSelection.LostFocus, AddressOf ControlLostFocus

        AddHandler txt_PieceSelection_LotNo.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_PieceSelection_Meters.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_PieceSelection_PcsNo.LostFocus, AddressOf ControlLostFocus

        'AddHandler dtp_Date.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Frieght.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_PoNo.KeyDown, AddressOf TextBoxControlKeyDown

        AddHandler dtp_Date.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Frieght.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_PoNo.KeyPress, AddressOf TextBoxControlKeyPress

        AddHandler dtp_Filter_Fromdate.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler dtp_Filter_ToDate.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_filterpono.KeyDown, AddressOf TextBoxControlKeyDown

        AddHandler dtp_Filter_Fromdate.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler dtp_Filter_ToDate.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_filterpono.KeyPress, AddressOf TextBoxControlKeyPress


        lbl_Company.Text = ""
        lbl_Company.Tag = 0
        lbl_Company.Visible = False
        Common_Procedures.CompIdNo = 0

        Filter_Status = False
        FrmLdSTS = True


        If Common_Procedures.settings.Hide_Qty_QtyMtr_In_Processing_Transactions = True Then
            dgv_Details.Columns(dgvCol_Details.QTY).Visible = False
            dgv_Details.Columns(dgvCol_Details.MTR_QTY).Visible = False
            dgv_Details_Total.Columns(dgvCol_Details.QTY).Visible = False
            dgv_Details_Total.Columns(dgvCol_Details.MTR_QTY).Visible = False
            dgv_Details.Columns(dgvCol_Details.ITEM_GREY).Width = dgv_Details.Columns(dgvCol_Details.ITEM_GREY).Width + dgv_Details.Columns(dgvCol_Details.QTY).Width - 5
            dgv_Details.Columns(dgvCol_Details.ITEM_FP).Width = dgv_Details.Columns(dgvCol_Details.ITEM_FP).Width + dgv_Details.Columns(dgvCol_Details.MTR_QTY).Width - 5

            dgv_Details_Total.Columns(dgvCol_Details.ITEM_GREY).Width = dgv_Details.Columns(dgvCol_Details.ITEM_GREY).Width
            dgv_Details_Total.Columns(dgvCol_Details.ITEM_FP).Width = dgv_Details.Columns(dgvCol_Details.ITEM_FP).Width
        End If

        If Common_Procedures.settings.Hide_Weight_Processing_Transactions = True Then
            dgv_Details.Columns(dgvCol_Details.WEIGHT).Visible = False
            dgv_Details_Total.Columns(dgvCol_Details.WEIGHT).Visible = False
            Last_Effective_Grid_Col = 14

            dgv_Details.Columns(dgvCol_Details.ITEM_GREY).Width = dgv_Details.Columns(dgvCol_Details.ITEM_GREY).Width + (dgv_Details.Columns(dgvCol_Details.WEIGHT).Width / 2)
            dgv_Details.Columns(dgvCol_Details.ITEM_FP).Width = dgv_Details.Columns(dgvCol_Details.ITEM_FP).Width + (dgv_Details.Columns(dgvCol_Details.WEIGHT).Width / 2)

            dgv_Details_Total.Columns(dgvCol_Details.ITEM_GREY).Width = dgv_Details.Columns(dgvCol_Details.ITEM_GREY).Width
            dgv_Details_Total.Columns(dgvCol_Details.ITEM_FP).Width = dgv_Details.Columns(dgvCol_Details.ITEM_FP).Width
        End If

        If Common_Procedures.settings.Show_Folding_In_Weight_Processing_Transactions = True Or Trim(Common_Procedures.settings.CustomerCode) = "1490" Then
            txt_Folding.Visible = True
            lbl_Foldig_Caption.Visible = True
        Else
            txt_Folding.Visible = False
            lbl_Foldig_Caption.Visible = False
        End If

        If Trim(Common_Procedures.settings.CustomerCode) = "1490" Then
            lbl_Lot_No_Discribtion.Visible = True
        End If

        If Trim(Common_Procedures.settings.CustomerCode) = "1558" Then
            btn_PieceSelection.Visible = True
        End If

        btn_SaveAll.Visible = False
        If Trim(Common_Procedures.settings.CustomerCode) = "-1558-" Then
            btn_SaveAll.Visible = True
        End If

        Dim rect As Rectangle
        rect = dgv_Details.GetCellDisplayRectangle(dgvCol_Details.BALES_NOS, 0, False)
        btn_BaleSelection.Left = dgv_Details.Left + rect.Left + rect.Width - btn_BaleSelection.Width - 2
        btn_BaleSelection.BringToFront()

        rect = dgv_Details.GetCellDisplayRectangle(dgvCol_Details.PCS, 0, False)
        btn_PieceSelection.Left = dgv_Details.Left + rect.Left + rect.Width - btn_PieceSelection.Width - 2
        btn_PieceSelection.BringToFront()


        For i = 0 To dgv_Details_Total.Columns.Count - 1
            dgv_Details_Total.Columns(i).Visible = dgv_Details.Columns(i).Visible
            dgv_Details_Total.Columns(i).Width = dgv_Details.Columns(i).Width
        Next i


        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1061" Then  ' --- prakash cottex
            Label3.Text = "JobOrder No"
        End If


        new_record()

    End Sub

    Private Sub processing_Delivery_Textile_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        con.Close()
        con.Dispose()
        Common_Procedures.Hide_CurrentStock_Display()
    End Sub

    Private Sub Processing_Delivery_Textile_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress

        Try
            If Asc(e.KeyChar) = 27 Then

                If pnl_Filter.Visible = True Then
                    btn_Filter_Close_Click(sender, e)
                    Exit Sub

                ElseIf pnl_BaleSelection.Visible = True Then
                    btn_Close_BaleSelection_Click(sender, e)
                    Exit Sub

                ElseIf pnl_Print.Visible = True Then
                    btn_print_Close_Click(sender, e)
                    Exit Sub
                ElseIf pnl_PieceSelection.Visible = True Then
                    btn_Close_PieceSelection_Click(sender, e)
                    Exit Sub
                Else
                    Close_Form()

                End If

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT CLOSE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

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
            ElseIf pnl_Back.Enabled = True Then
                dgv1 = dgv_Details

            End If

            If IsNothing(dgv1) = False Then

                With dgv1

                    If keyData = Keys.Enter Or keyData = Keys.Down Then
                        'If .CurrentCell.ColumnIndex >= .ColumnCount - 6 Then
                        If .CurrentCell.ColumnIndex >= Last_Effective_Grid_Col Then
                            If .CurrentCell.RowIndex = .RowCount - 1 Then
                                cbo_TransportName.Focus()

                            Else
                                .CurrentCell = .Rows(.CurrentCell.RowIndex + 1).Cells(dgvCol_Details.ITEM_GREY)

                            End If

                        Else

                            For K = .CurrentCell.ColumnIndex + 1 To .ColumnCount - 1
                                If .Columns(K).Visible = True And .Columns(K).ReadOnly = False Then
                                    .CurrentCell = .Rows(.CurrentRow.Index).Cells(K)
                                    Return True
                                End If
                            Next

                        End If

                        Return True

                    ElseIf keyData = Keys.Up Then

                        If .CurrentCell.ColumnIndex <= 1 Then
                            If .CurrentCell.RowIndex = 0 Then
                                cbo_Ledger.Focus()

                            Else
                                .CurrentCell = .Rows(.CurrentCell.RowIndex - 1).Cells(Last_Effective_Grid_Col)

                            End If

                        Else

                            For K = .CurrentCell.ColumnIndex - 1 To 0 Step -1
                                If .Columns(K).Visible Then
                                    .CurrentCell = .Rows(.CurrentRow.Index).Cells(K)
                                    Return True
                                End If
                            Next


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
            MessageBox.Show(ex.Message, "DOES NOT CLOSE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Public Sub delete_record() Implements Interface_MDIActions.delete_record
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim cmd As New SqlClient.SqlCommand
        Dim trans As SqlClient.SqlTransaction
        Dim NewCode As String = ""
        Dim Qa As Windows.Forms.DialogResult
        Dim k As Integer = 0
        Dim vPackSlp_Code_FldNm As String = ""
        Dim vPackSlp_Inc_FldNm As String = ""


        'If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.Entry_Fabric_DeliveryTo_Processing, "~L~") = 0 And InStr(Common_Procedures.UR.Entry_Fabric_DeliveryTo_Processing, "~D~") = 0 Then MessageBox.Show("You have No Rights to Delete", "DOES NOT INSERT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error) : Exit Sub

        Qa = MessageBox.Show("Do you want to Delete?", "FOR DELETION...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2)
        If Qa = Windows.Forms.DialogResult.No Or Qa = Windows.Forms.DialogResult.Cancel Then
            Exit Sub
        End If

        If New_Entry = True Then
            MessageBox.Show("This is New Entry", "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
        End If

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(EntFnYrCode)

        Da = New SqlClient.SqlDataAdapter("select sum(Receipt_Meters) from Textile_Processing_Delivery_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Cloth_Processing_Delivery_code = '" & Trim(NewCode) & "' ", con)
        Dt1 = New DataTable
        Da.Fill(Dt1)
        If Dt1.Rows.Count > 0 Then
            If IsDBNull(Dt1.Rows(0)(0).ToString) = False Then
                If Val(Dt1.Rows(0)(0).ToString) > 0 Then
                    MessageBox.Show("Already Some Cloths Receipted for this order", "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                    Exit Sub
                End If
            End If
        End If
        Dt1.Clear()


        Da = New SqlClient.SqlDataAdapter("select sum(Return_Meters) from Textile_Processing_Delivery_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Cloth_Processing_Delivery_code = '" & Trim(NewCode) & "'", con)
        Dt1 = New DataTable
        Da.Fill(Dt1)
        If Dt1.Rows.Count > 0 Then
            If IsDBNull(Dt1.Rows(0)(0).ToString) = False Then
                If Val(Dt1.Rows(0)(0).ToString) > 0 Then
                    MessageBox.Show("Already Some Cloths Returned for this order", "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                    Exit Sub
                End If
            End If
        End If
        Dt1.Clear()

        trans = con.BeginTransaction

        Try

            NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(EntFnYrCode)

            cmd.Connection = con
            cmd.Transaction = trans

            cmd.CommandText = "Truncate Table TempTable_For_NegativeStock"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Insert into TempTable_For_NegativeStock ( Stock_Type, Reference_Code, Reference_Date, Company_Idno, Ledger_Idno         , Item_IdNo, Rack_IdNo ) " &
                                    " Select                               'PI'    , Reference_Code, Reference_Date, Company_IdNo, DeliveryTo_StockIdNo, Item_IdNo, Rack_IdNo from Stock_Item_Processing_Details where Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Update Packing_Slip_Head set Delivery_Code = '', Delivery_No = '', Delivery_DetailsSlNo = 0, Delivery_Increment = Delivery_Increment - 1, Delivery_Date = Null Where Delivery_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            'For k = 1 To 5
            k = 1
            vPackSlp_Code_FldNm = "PackingSlip_Code_Type" & Trim(Val(k))
            vPackSlp_Inc_FldNm = "PackingSlip_Inc_Type" & Trim(Val(k))

            cmd.CommandText = "Update Weaver_ClothReceipt_Piece_Details set " & vPackSlp_Code_FldNm & " = '',  " & vPackSlp_Inc_FldNm & "  = " & vPackSlp_Inc_FldNm & " - 1 Where  " & vPackSlp_Code_FldNm & " = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            'Next k
            cmd.CommandText = "Delete from Stock_Cloth_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Textile_Processing_Delivery_Piece_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and ClothProcess_Delivery_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "delete from Textile_Processing_Delivery_Details where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Cloth_Processing_Delivery_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "delete from Textile_Processing_Delivery_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and ClothProcess_Delivery_Code = '" & Trim(NewCode) & "' and ClothProcess_Delivery_Code not like '" & Trim(Pk_Condition2) & "%'"
            cmd.ExecuteNonQuery()

            If Val(Common_Procedures.settings.NegativeStock_Restriction) = 1 Then

                If Common_Procedures.Check_is_Negative_Stock_Status(con, trans) = True Then Exit Sub

            End If

            trans.Commit()

            Dt1.Dispose()
            Da.Dispose()
            trans.Dispose()
            cmd.Dispose()

            new_record()

            MessageBox.Show("Deleted Successfully!!!", "FOR DELETION...", MessageBoxButtons.OKCancel, MessageBoxIcon.Information)

        Catch ex As Exception

            trans.Rollback()
            MessageBox.Show(ex.Message, "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

        If dtp_Date.Enabled = True And dtp_Date.Visible = True Then dtp_Date.Focus()

    End Sub

    Public Sub filter_record() Implements Interface_MDIActions.filter_record

        If Filter_Status = False Then

            Dim da As New SqlClient.SqlDataAdapter
            Dim dt1 As New DataTable
            Dim dt2 As New DataTable
            Dim dt3 As New DataTable

            da = New SqlClient.SqlDataAdapter("select Ledger_DisplayName from Ledger_AlaisHead where (Ledger_IdNo = 0 or AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14 ) order by Ledger_DisplayName", con)
            da.Fill(dt1)
            cbo_Filter_PartyName.DataSource = dt1
            cbo_Filter_PartyName.DisplayMember = "Ledger_DisplayName"

            da = New SqlClient.SqlDataAdapter("select process_name from process_head order by process_name", con)
            da.Fill(dt2)
            cbo_Filter_ProcessName.DataSource = dt2
            cbo_Filter_ProcessName.DisplayMember = "process_name"


            dtp_Filter_Fromdate.Text = ""
            dtp_Filter_ToDate.Text = ""
            cbo_Filter_PartyName.Text = ""
            cbo_Filter_ProcessName.Text = ""

            cbo_Filter_PartyName.SelectedIndex = -1
            cbo_Filter_ProcessName.SelectedIndex = -1
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
        Dim movno As String = ""
        Dim NewCode As String = ""
        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(EntFnYrCode)
        Try

            da = New SqlClient.SqlDataAdapter("select top 1 ClothProcess_Delivery_RefNo from Textile_Processing_Delivery_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & "  and ClothProcess_Delivery_Code like '%/" & Trim(EntFnYrCode) & "'  and (ClothProcess_Delivery_Code not like '" & Trim(Pk_Condition2) & "%' and ClothProcess_Delivery_Code not like 'CPREC%' and ClothProcess_Delivery_Code not like 'WCLRC%' ) Order by for_Orderby , ClothProcess_Delivery_RefNo", con)
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
        Dim NewCode As String

        Try
            NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(EntFnYrCode)
            OrdByNo = Common_Procedures.OrderBy_CodeToValue(Trim(lbl_DcNo.Text))

            da = New SqlClient.SqlDataAdapter("select top 1 ClothProcess_Delivery_RefNo from Textile_Processing_Delivery_Head where for_orderby > " & Str(Val(OrdByNo)) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and ClothProcess_Delivery_Code like '%/" & Trim(EntFnYrCode) & "' and (ClothProcess_Delivery_Code not like '" & Trim(Pk_Condition2) & "%' and ClothProcess_Delivery_Code not like 'CPREC%' and ClothProcess_Delivery_Code not like 'WCLRC%' ) Order by for_Orderby, ClothProcess_Delivery_RefNo", con)
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

            OrdByNo = Common_Procedures.OrderBy_CodeToValue(Trim(lbl_DcNo.Text))

            da = New SqlClient.SqlDataAdapter("select top 1 ClothProcess_Delivery_RefNo from Textile_Processing_Delivery_Head where for_orderby < " & Str(Val(OrdByNo)) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and ClothProcess_Delivery_Code like '%/" & Trim(EntFnYrCode) & "' and (ClothProcess_Delivery_Code not like '" & Trim(Pk_Condition2) & "%' and ClothProcess_Delivery_Code not like 'CPREC%' and ClothProcess_Delivery_Code not like 'WCLRC%' )  Order by for_Orderby desc, ClothProcess_Delivery_RefNo desc", con)
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
            da = New SqlClient.SqlDataAdapter("select top 1 ClothProcess_Delivery_RefNo from Textile_Processing_Delivery_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and ClothProcess_Delivery_Code like '%/" & Trim(EntFnYrCode) & "' and (ClothProcess_Delivery_Code not like '" & Trim(Pk_Condition2) & "%' and ClothProcess_Delivery_Code not like 'CPREC%' and ClothProcess_Delivery_Code not like 'WCLRC%' ) Order by for_Orderby desc, ClothProcess_Delivery_RefNo desc", con)
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
        Dim dt As New DataTable
        Dim dt1 As New DataTable
        Dim NewID As Integer = 0

        Try
            clear()

            New_Entry = True


            lbl_DcNo.Text = Common_Procedures.get_MaxCode(con, "Textile_Processing_Delivery_Head", "ClothProcess_Delivery_Code", "For_OrderBy", "ClothProcess_Delivery_Code not like '" & Trim(Pk_Condition2) & "%'", Val(lbl_Company.Tag), Trim(EntFnYrCode))

            lbl_DcNo.ForeColor = Color.Red

            da = New SqlClient.SqlDataAdapter("select top 1 * from Textile_Processing_Delivery_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and ClothProcess_Delivery_Code like '%/" & Trim(EntFnYrCode) & "' and (ClothProcess_Delivery_Code not like '" & Trim(Pk_Condition2) & "%' and ClothProcess_Delivery_Code not like 'CPREC%' and ClothProcess_Delivery_Code not like 'WCLRC%' )  Order by for_Orderby desc, ClothProcess_Delivery_RefNo desc", con)
            dt1 = New DataTable
            da.Fill(dt1)
            If dt1.Rows.Count > 0 Then

                If Val(dt1.Rows(0).Item("GST_Tax_Invoice_Status").ToString) = 1 Then chk_GSTTax_Invocie.Checked = True Else chk_GSTTax_Invocie.Checked = False

                If IsDBNull(dt1.Rows(0).Item("ClothProcess_Delivery_PrefixNo").ToString) = False Then
                    If dt1.Rows(0).Item("ClothProcess_Delivery_PrefixNo").ToString <> "" Then txt_DcPrefixNo.Text = dt1.Rows(0).Item("ClothProcess_Delivery_PrefixNo").ToString
                End If
                If IsDBNull(dt1.Rows(0).Item("ClothProcess_Delivery_SuffixNo").ToString) = False Then
                    If dt1.Rows(0).Item("ClothProcess_Delivery_SuffixNo").ToString <> "" Then cbo_DcSufixNo.Text = dt1.Rows(0).Item("ClothProcess_Delivery_SuffixNo").ToString
                End If

            End If
            dt1.Clear()

            If dtp_Date.Enabled And dtp_Date.Visible Then dtp_Date.Focus()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR NEW RECORD...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

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

            inpno = InputBox("Enter Dc.No.", "FOR FINDING...")

            RecCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(EntFnYrCode)

            Da = New SqlClient.SqlDataAdapter("select ClothProcess_Delivery_RefNo from Textile_Processing_Delivery_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and ClothProcess_Delivery_Code = '" & Trim(RecCode) & "'", con)
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
                MessageBox.Show("Dc No. does not exists", "DOES NOT FIND...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

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

        ' If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.Entry_Fabric_DeliveryTo_Processing, "~L~") = 0 And InStr(Common_Procedures.UR.Entry_Fabric_DeliveryTo_Processing, "~I~") = 0 Then MessageBox.Show("You have No Rights to Insert", "DOES NOT INSERT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error) : Exit Sub

        Try

            inpno = InputBox("Enter New Dc No.", "FOR NEW DELIVERY INSERTION...")

            RecCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(EntFnYrCode)

            Da = New SqlClient.SqlDataAdapter("select ClothProcess_Delivery_RefNo from Textile_Processing_Delivery_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and ClothProcess_Delivery_Code = '" & Trim(RecCode) & "'", con)
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
                    MessageBox.Show("Invalid Dc No", "DOES NOT INSERT NEW DELIVERY...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

                Else
                    new_record()
                    Insert_Entry = True
                    lbl_DcNo.Text = Trim(UCase(inpno))

                End If

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT INSERT NEW DELIVERY...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Public Sub save_record() Implements Interface_MDIActions.save_record
        Dim cmd As New SqlClient.SqlCommand
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim Da4 As New SqlClient.SqlDataAdapter
        Dim Dt4 As New DataTable
        Dim tr As SqlClient.SqlTransaction
        Dim NewCode As String = ""
        Dim Led_ID As Integer = 0
        Dim Del_Godown_Led_ID As Integer = 0
        Dim Col_ID As Integer = 0
        Dim Col_ID_Del As Integer = 0
        Dim Itfp_ID As Integer = 0
        Dim Sno As Integer = 0
        Dim Partcls As String = ""
        Dim PBlNo As String = ""
        Dim EntID As String = ""
        Dim vTotPcs As Single, vTotMtrs As Single, vtotqty As Single, vTotAmt As Single, vPcsSno As Integer = 0
        Dim Proc_ID As Integer = 0
        Dim Lot_ID As Integer = 0
        Dim vTotWeight As Single
        Dim Tr_ID As Integer = 0
        Dim itgry_id As Integer = 0
        Dim Nr As Integer = 0
        Dim OpYrCode As String = ""
        Dim vFoldPerc As String = ""
        Dim vStkOf_Pos_IdNo As Integer = 0
        Dim vDcNo = ""
        Dim vCLOID As Integer
        Dim vRECON_IN As String
        Dim vCLOSTK_IN As String
        Dim vPackSlp_Code_FldNm As String = ""
        Dim vPackSlp_Inc_FldNm As String = ""
        Dim k As Integer = 0
        Dim vERR_BALECODE As String = ""

        Dim vPcsTyp_ID As Integer = 0
        Dim vPcsparty_ID As Integer = 0
        Dim vPcsClo_ID As Integer = 0
        Dim vPcsMtr_FldNm As String = ""

        Dim vOrdByNo As String = ""
        Dim clthtyp_ID = 0
        Dim Lot_Close_Status = 0


        vOrdByNo = Common_Procedures.OrderBy_CodeToValue(lbl_DcNo.Text)


        If Val(lbl_Company.Tag) = 0 Then
            MessageBox.Show("Invalid Company Selection", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
        End If

        ' If Common_Procedures.UserRight_Check(Common_Procedures.UR.Entry_Fabric_DeliveryTo_Processing, New_Entry) = False Then Exit Sub

        If pnl_Back.Enabled = False Then
            MessageBox.Show("Close Other Windows", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
        End If

        If IsDate(dtp_Date.Text) = False Then
            MessageBox.Show("Invalid Date", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If dtp_Date.Enabled And dtp_Date.Visible Then dtp_Date.Focus()
            Exit Sub
        End If
        If EntFnYrCode = Common_Procedures.FnYearCode Then
            If Not (dtp_Date.Value.Date >= Common_Procedures.Company_FromDate And dtp_Date.Value.Date <= Common_Procedures.Company_ToDate) Then
                MessageBox.Show("Date is out of financial range", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                If dtp_Date.Enabled And dtp_Date.Visible Then dtp_Date.Focus()
                Exit Sub
            End If
        End If

        Led_ID = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Ledger.Text)
        If Led_ID = 0 Then
            MessageBox.Show("Invalid Party Name", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If cbo_Ledger.Enabled And cbo_Ledger.Visible Then cbo_Ledger.Focus()
            Exit Sub
        End If

        Del_Godown_Led_ID = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Delivery_From.Text)
        If Del_Godown_Led_ID = 0 Then
            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1516" Then '---- P S ENTERPRISES (KANPUR)
                MessageBox.Show("Invalid Delivery from Godown Name", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                If cbo_Delivery_From.Enabled And cbo_Delivery_From.Visible Then cbo_Delivery_From.Focus()
                Exit Sub

            Else
                Del_Godown_Led_ID = 4

            End If

        End If

        If txt_Folding.Visible Then
            If Val(txt_Folding.Text) = 0 Then
                If MessageBox.Show("Invalid 'Folding' Value Provided. Do you want to apply the default folding Value of '100' ? ", "FOLDING...", MessageBoxButtons.YesNo, MessageBoxIcon.Question) Then
                    txt_Folding.Text = "100"
                Else
                    txt_Folding.Focus()
                    Exit Sub
                End If
            End If
        Else
            txt_Folding.Text = "100"
        End If

        Lot_ID = Common_Procedures.Lot_NoToIdNo(con, CBO_JobNO.Text)

        If Lot_ID = 0 And Common_Procedures.settings.CustomerCode <> "1061" And Common_Procedures.settings.CustomerCode <> "1558" Then
            MessageBox.Show("Invalid Lot Number", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If CBO_JobNO.Enabled And CBO_JobNO.Visible Then cbo_LotNo.Focus()
            Exit Sub
        End If

        Tr_ID = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_TransportName.Text)

        Proc_ID = Common_Procedures.Process_NameToIdNo(con, Cbo_ProcessingHEAD.Text)

        lbl_UserName.Text = Common_Procedures.User.IdNo

        If Proc_ID = 0 Then
            MessageBox.Show("Invalid PROCESSING Name ", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If Cbo_ProcessingHEAD.Enabled And Cbo_ProcessingHEAD.Visible Then
                Cbo_ProcessingHEAD.Focus()
            End If
            Exit Sub
        End If

        With dgv_Details
            For i = 0 To dgv_Details.RowCount - 1
                If Val(.Rows(i).Cells(dgvCol_Details.PCS).Value) <> 0 Or Val(.Rows(i).Cells(dgvCol_Details.MTR_QTY).Value) <> 0 Or Val(.Rows(i).Cells(dgvCol_Details.WEIGHT).Value) <> 0 Then

                    If Trim(dgv_Details.Rows(i).Cells(dgvCol_Details.ITEM_GREY).Value) = "" Then
                        MessageBox.Show("Invalid GREY Item", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                        If dgv_Details.Enabled And dgv_Details.Visible Then
                            dgv_Details.Focus()
                            dgv_Details.CurrentCell = dgv_Details.Rows(i).Cells(dgvCol_Details.ITEM_GREY)

                        End If
                        Exit Sub
                    End If


                    vCLOID = Common_Procedures.Cloth_NameToIdNo(con, .Rows(i).Cells(dgvCol_Details.ITEM_GREY).Value)
                        If vCLOID = 0 Then
                            MessageBox.Show("Invalid GREY Item", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                            If dgv_Details.Enabled And dgv_Details.Visible Then
                                dgv_Details.Focus()
                                dgv_Details.CurrentCell = dgv_Details.Rows(i).Cells(dgvCol_Details.ITEM_GREY)
                            End If
                            Exit Sub
                        End If

                    If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1464" Then  '-- MOF

                        If Trim(dgv_Details.Rows(i).Cells(dgvCol_Details.ITEM_FP).Value) = "" Then
                            MessageBox.Show("Invalid FP Item", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                            If dgv_Details.Enabled And dgv_Details.Visible Then
                                dgv_Details.Focus()
                                dgv_Details.CurrentCell = dgv_Details.Rows(i).Cells(dgvCol_Details.ITEM_FP)

                            End If
                            Exit Sub

                        End If

                        If Trim(dgv_Details.Rows(i).Cells(dgvCol_Details.COLOUR).Value) = "" Then
                            MessageBox.Show("Invalid COLOUR Name", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                            If dgv_Details.Enabled And dgv_Details.Visible Then
                                dgv_Details.Focus()
                                dgv_Details.CurrentCell = dgv_Details.Rows(i).Cells(dgvCol_Details.COLOUR)

                            End If
                            Exit Sub

                        End If
                    End If

                    'If Trim(dgv_Details.Rows(i).Cells(5).Value) = "" Then
                    '    MessageBox.Show("Invalid PROCESSING Name ", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    '    If dgv_Details.Enabled And dgv_Details.Visible Then
                    '        dgv_Details.Focus()
                    '        dgv_Details.CurrentCell = dgv_Details.Rows(i).Cells(5)

                    '    End If
                    '    Exit Sub
                    'End If

                    vRECON_IN = "METER"
                        vCLOSTK_IN = "METER"
                        Da4 = New SqlClient.SqlDataAdapter("Select Stock_In, Fabric_Processing_Reconsilation_In_Meter_Weight from Cloth_Head Where Cloth_Idno = " & Str(Val(vCLOID)), con)
                        Dt4 = New DataTable
                        Da4.Fill(Dt4)
                        If Dt4.Rows.Count > 0 Then
                            vCLOSTK_IN = Dt4.Rows(0).Item("Stock_In").ToString
                            vRECON_IN = Dt4.Rows(0).Item("Fabric_Processing_Reconsilation_In_Meter_Weight").ToString
                        End If
                        Dt4.Clear()
                        Da4.Dispose()

                        If Trim(UCase(vCLOSTK_IN)) = "PCS" Then
                            If Val(dgv_Details.Rows(i).Cells(dgvCol_Details.PCS).Value) = 0 Then
                                MessageBox.Show("Invalid PCS..", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                                If dgv_Details.Enabled Then dgv_Details.Focus()
                                dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(dgvCol_Details.PCS)
                                Exit Sub
                            End If

                        Else

                            If Val(dgv_Details.Rows(i).Cells(dgvCol_Details.METERS).Value) = 0 Then
                                MessageBox.Show("Invalid Meters..", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                                If dgv_Details.Enabled Then dgv_Details.Focus()
                                dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(dgvCol_Details.METERS)
                                Exit Sub
                            End If

                        End If

                        If Trim(UCase(vRECON_IN)) = "WEIGHT" Then
                            If Val(dgv_Details.Rows(i).Cells(dgvCol_Details.WEIGHT).Value) = 0 Then
                                MessageBox.Show("Invalid Weight..", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                                If dgv_Details.Enabled Then dgv_Details.Focus()
                                dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(dgvCol_Details.WEIGHT)
                                Exit Sub
                            End If

                        Else

                            If Val(dgv_Details.Rows(i).Cells(dgvCol_Details.METERS).Value) = 0 Then
                                MessageBox.Show("Invalid Meters..", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                                If dgv_Details.Enabled Then dgv_Details.Focus()
                                dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(dgvCol_Details.METERS)
                                Exit Sub
                            End If

                        End If

                    End If

            Next
        End With

        Total_Calculation()

        vTotMtrs = 0 : vTotWeight = 0 : vTotPcs = 0 : vtotqty = 0 : vTotAmt = 0

        If dgv_Details_Total.RowCount > 0 Then

            vTotPcs = Val(dgv_Details_Total.Rows(0).Cells(dgvCol_Details.PCS).Value())
            vtotqty = Val(dgv_Details_Total.Rows(0).Cells(dgvCol_Details.QTY).Value())
            vTotMtrs = Val(dgv_Details_Total.Rows(0).Cells(dgvCol_Details.METERS).Value())
            vTotWeight = Val(dgv_Details_Total.Rows(0).Cells(dgvCol_Details.WEIGHT).Value())
            vTotAmt = Val(dgv_Details_Total.Rows(0).Cells(dgvCol_Details.AMOUNT).Value())

        End If

        vStkOf_Pos_IdNo = Common_Procedures.CommonLedger.OwnSort_Ac  ' Val(Common_Procedures.CommonLedger.Godown_Ac)

        Dim RET_TYPE As String = "CLOTH"

        If Len(Trim(Process_Outputs)) > 1 Then
            If Mid(Trim(Process_Outputs), 2, 1) = "1" Then
                RET_TYPE = "FP"
            End If
        End If

        Dim vGST_Tax_Inv_Sts = 0
        If chk_GSTTax_Invocie.Checked = True Then vGST_Tax_Inv_Sts = 1

        Lot_Close_Status = 0
        If chk_LotClose.Checked = True Then Lot_Close_Status = 1

        If Common_Procedures.settings.Sales_OrderNumber_compulsory_in_ALLEntry_Status = 1 Then
            If Trim(cbo_ClothSales_OrderCode_forSelection.Text) = "" Then
                MessageBox.Show("Invalid Sales Order No", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                If cbo_ClothSales_OrderCode_forSelection.Enabled And cbo_ClothSales_OrderCode_forSelection.Visible Then cbo_ClothSales_OrderCode_forSelection.Focus()
                Exit Sub
            End If
        End If

        tr = con.BeginTransaction

        Try

            If Insert_Entry = True Or New_Entry = False Then

                NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(EntFnYrCode)

            Else

                lbl_DcNo.Text = Common_Procedures.get_MaxCode(con, "Textile_Processing_Delivery_Head", "ClothProcess_Delivery_Code", "For_OrderBy", "ClothProcess_Delivery_Code not like '" & Trim(Pk_Condition2) & "%'", Val(lbl_Company.Tag), EntFnYrCode, tr)

                NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(EntFnYrCode)

            End If

            vDcNo = Trim(txt_DcPrefixNo.Text) & Trim(lbl_DcNo.Text) & Trim(cbo_DcSufixNo.Text)

            cmd.Connection = con
            cmd.Transaction = tr

            cmd.Parameters.Clear()
            cmd.Parameters.AddWithValue("@DeliveryDate", dtp_Date.Value.Date)

            cmd.CommandText = "Truncate Table TempTable_For_NegativeStock"
            cmd.ExecuteNonQuery()

            If New_Entry = True Then

                cmd.CommandText = "Insert into Textile_Processing_Delivery_Head(ClothProcess_Delivery_Code,              Company_IdNo,            ClothProcess_Delivery_RefNo , ClothProcess_Delivery_No ,                   ClothProcess_Delivery_PrefixNo             ,              ClothProcess_Delivery_SuffixNo    ,                 for_OrderBy                                          , ClothProcess_Delivery_Date,   Ledger_IdNo,            Purchase_OrderNo,           Transport_IdNo,         Freight_Charges,                                Note,                   Total_Pcs,               Total_Qty,              Total_Meters        ,               Total_Weight ,         Processing_Idno ,           JobOrder_No  ,  User_idNo  , Vehicle_No ,Delivery_From_Godown_IdNo,Folding    ,Lot_IdNo,FabricPurchase_Weaver_Lot_Idno , Return_Product_Type , GST_Tax_Invoice_Status ,EwayBill_No  ,Total_Amount  ,Net_Amount , ClothSales_OrderCode_forSelection                                                                                                                                  ,        Lot_Close_Status        )  " &
                                                                "Values (   '" & Trim(NewCode) & "',     " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_DcNo.Text) & "' ,'" & Trim(vDcNo) & "'   ,'" & Trim(UCase(txt_DcPrefixNo.Text)) & "' ,'" & Trim(cbo_DcSufixNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_DcNo.Text))) & ", @DeliveryDate, " & Str(Val(Led_ID)) & ", '" & Trim(txt_PoNo.Text) & "', " & Str(Val(Tr_ID)) & ", " & Str(Val(txt_Frieght.Text)) & ",  '" & Trim(txt_Note.Text) & "'," & Str(Val(vTotPcs)) & "," & Str(Val(vtotqty)) & " , " & Str(Val(vTotMtrs)) & ", " & Str(Val(vTotWeight)) & " ,  " & Str(Val(Proc_ID)) & ",'" & Trim(CBO_JobNO.Text) & "'," & Val(lbl_UserName.Text) & " ,'" & Trim(cbo_VehicleNo.Text) & "'," & Del_Godown_Led_ID.ToString & "," & Val(txt_Folding.Text).ToString & " ," & Lot_ID.ToString & "," & Lot_ID.ToString & ",'" & RET_TYPE & "' ," & Str(Val(vGST_Tax_Inv_Sts)) & " ,'" & Trim(txt_EWBNo.Text) & "'," & Str(Val(vTotAmt)) & "," & Str(Val(vTotAmt)) & " , '" & Trim(cbo_ClothSales_OrderCode_forSelection.Text) & "' , " & Val(Lot_Close_Status) & " )"
                cmd.ExecuteNonQuery()

            Else

                cmd.CommandText = "Update Textile_Processing_Delivery_Head set ClothProcess_Delivery_Date = @DeliveryDate , ClothProcess_Delivery_RefNo = '" & Trim(lbl_DcNo.Text) & "'  ,ClothProcess_Delivery_PrefixNo = '" & Trim(UCase(txt_DcPrefixNo.Text)) & "' ,  ClothProcess_Delivery_No =   '" & Trim(vDcNo) & "', ClothProcess_Delivery_SuffixNo = '" & Trim(cbo_DcSufixNo.Text) & "', Processing_Idno = " & Val(Proc_ID) & ",Ledger_IdNo = " & Val(Led_ID) & ", Purchase_OrderNo = '" & Trim(txt_PoNo.Text) & "' , Transport_IdNo = " & Val(Tr_ID) & ", Freight_Charges = " & Val(txt_Frieght.Text) & ", Vehicle_No =  '" & Trim(cbo_VehicleNo.Text) & "' , JobOrder_No = '" & Trim(CBO_JobNO.Text) & "' , Note = '" & Trim(txt_Note.Text) & "', Total_Pcs = " & Val(vTotPcs) & ",Total_Qty = " & Val(vtotqty) & ", Total_Meters = " & Val(vTotMtrs) & ",Total_Weight = " & Val(vTotWeight) & " , User_idNo = " & Val(lbl_UserName.Text) & ",  Delivery_From_Godown_IdNo = " & Del_Godown_Led_ID.ToString & ",Folding = " & Val(txt_Folding.Text).ToString & ",Lot_IdNo = " & Lot_ID.ToString & ",FabricPurchase_Weaver_Lot_Idno = " & Lot_ID.ToString & ",Return_Product_Type = '" & RET_TYPE & "', GST_Tax_Invoice_Status = " & Str(Val(vGST_Tax_Inv_Sts)) & " ,EwayBill_No ='" & Trim(txt_EWBNo.Text) & "' , Total_Amount = " & Str(Val(vTotAmt)) & " , Net_Amount = " & Str(Val(vTotAmt)) & " , ClothSales_OrderCode_forSelection = '" & Trim(cbo_ClothSales_OrderCode_forSelection.Text) & "',Lot_Close_Status =" & Val(Lot_Close_Status) & "  Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " And ClothProcess_Delivery_Code = '" & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()

                cmd.CommandText = "Update Packing_Slip_Head set Delivery_Code = '', Delivery_No = '', Delivery_DetailsSlNo = 0, Delivery_Increment = Delivery_Increment - 1, Delivery_Date = Null Where Delivery_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()

                cmd.CommandText = "Insert into TempTable_For_NegativeStock ( Stock_Type, Reference_Code, Reference_Date, Company_Idno, Ledger_Idno         , Item_IdNo, Rack_IdNo ) " &
                                  " Select                               'PI'    , Reference_Code, Reference_Date, Company_IdNo, DeliveryTo_StockIdNo, Item_IdNo, Rack_IdNo from Stock_Item_Processing_Details where Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()


                'For k = 1 To 5
                k = 1

                vPackSlp_Code_FldNm = "PackingSlip_Code_Type" & Trim(Val(k))
                vPackSlp_Inc_FldNm = "PackingSlip_Inc_Type" & Trim(Val(k))

                cmd.CommandText = "Update Weaver_ClothReceipt_Piece_Details set " & vPackSlp_Code_FldNm & " = '',  " & vPackSlp_Inc_FldNm & "  = " & vPackSlp_Inc_FldNm & " - 1 Where  " & vPackSlp_Code_FldNm & " = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()

                'Next k

            End If

            cmd.CommandText = "Delete from Textile_Processing_Delivery_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Cloth_Processing_Delivery_Code = '" & Trim(NewCode) & "' and Receipt_Meters = 0 And Return_Meters = 0 "
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Textile_Processing_Delivery_Piece_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and ClothProcess_Delivery_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Stock_Cloth_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Pk_Condition & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            Partcls = "Delv : Dc.No. " & Trim(lbl_DcNo.Text)

            PBlNo = Trim(lbl_DcNo.Text)
            EntID = Trim(Pk_Condition) & Trim(lbl_DcNo.Text)


            With dgv_Details

                Sno = 0
                vPcsSno = 0

                For i = 0 To .RowCount - 1

                    If Val(.Rows(i).Cells(dgvCol_Details.PCS).Value) <> 0 Or Val(.Rows(i).Cells(dgvCol_Details.METERS).Value) <> 0 Or Val(.Rows(i).Cells(dgvCol_Details.WEIGHT).Value) <> 0 Then

                        Sno = Sno + 1

                        itgry_id = Common_Procedures.Cloth_NameToIdNo(con, .Rows(i).Cells(dgvCol_Details.ITEM_GREY).Value, tr)

                        If RET_TYPE = "FP" Then
                            Itfp_ID = Common_Procedures.Processed_Item_NameToIdNo(con, .Rows(i).Cells(dgvCol_Details.ITEM_FP).Value, tr)
                        Else
                            Itfp_ID = Common_Procedures.Cloth_NameToIdNo(con, .Rows(i).Cells(dgvCol_Details.ITEM_FP).Value, tr)
                        End If

                        Col_ID = Common_Procedures.Colour_NameToIdNo(con, .Rows(i).Cells(dgvCol_Details.COLOUR).Value, tr)

                        Col_ID_Del = Common_Procedures.Colour_NameToIdNo(con, .Rows(i).Cells(dgvCol_Details.COLOUR_DELY).Value, tr)

                        'Pro_ID = Common_Procedures.Process_NameToIdNo(con, .Rows(i).Cells(5).Value, tr)
                        'Sno = Sno + 1

                        Nr = 0
                        cmd.CommandText = "Update  Textile_Processing_Delivery_Details set Cloth_Processing_Delivery_Date = @DeliveryDate , ClothProcess_Delivery_RefNo = '" & Trim(lbl_DcNo.Text) & "' ,  Cloth_Processing_Delivery_No =   '" & Trim(vDcNo) & "' , Ledger_IdNo = " & Str(Val(Led_ID)) & ", Sl_No  = " & Str(Val(Sno)) & " , Item_Idno = " & Str(Val(itgry_id)) & " , Item_To_Idno = " & Str(Val(Itfp_ID)) & " , Colour_Idno = " & Val(Col_ID) & " , Lot_IdNo = " & Val(Lot_ID) & " ,Processing_Idno = " & Val(Proc_ID) & " ,  Bales = " & Str(Val(.Rows(i).Cells(dgvCol_Details.BALES).Value)) & "  ,  Bales_Nos = '" & Trim(.Rows(i).Cells(dgvCol_Details.BALES_NOS).Value) & "' , Delivery_Pcs =  " & Val(.Rows(i).Cells(dgvCol_Details.PCS).Value) & ", Delivery_Qty = " & Val(.Rows(i).Cells(dgvCol_Details.QTY).Value) & " ,  Meter_Qty = " & Str(Val(.Rows(i).Cells(dgvCol_Details.MTR_QTY).Value)) & " ,    Delivery_Meters = " & Str(Val(.Rows(i).Cells(dgvCol_Details.METERS).Value)) & " ,    Delivery_Weight = " & Str(Val(.Rows(i).Cells(dgvCol_Details.WEIGHT).Value)) & ", PackingSlip_Codes = '" & Trim(.Rows(i).Cells(dgvCol_Details.PACKINGSLIP_CODE).Value) & "', ClothProcessing_Delivery_PackingSlno = " & Str(Val(.Rows(i).Cells(dgvCol_Details.CLOTH_PROCESSING_DELIVERY_PACKINGSLIP_SLNO).Value)) & ",Folding = " & Val(txt_Folding.Text).ToString & ",FabricPurchase_Weaver_Lot_Idno = " & Lot_ID.ToString & ",Del_Colour_IdNo = " & Col_ID_Del.ToString & " , Rate = " & Str(Val(.Rows(i).Cells(dgvCol_Details.RATE).Value)) & " ,Amount =" & Str(Val(.Rows(i).Cells(dgvCol_Details.AMOUNT).Value)) & "  where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " And Cloth_Processing_Delivery_code = '" & Trim(NewCode) & "'  and Cloth_Processing_Delivery_Slno = " & Val(.Rows(i).Cells(dgvCol_Details.CLOTH_PROCESSING_DELIVERY_SLNO).Value)
                        Nr = cmd.ExecuteNonQuery()

                        If Nr = 0 Then
                            cmd.CommandText = "Insert into Textile_Processing_Delivery_Details(Cloth_Processing_Delivery_Code, Company_IdNo, ClothProcess_Delivery_RefNo , Cloth_Processing_Delivery_No, for_OrderBy, Cloth_Processing_Delivery_Date,Sl_No, Ledger_IdNo,  Item_Idno,Item_To_Idno, Colour_Idno ,Lot_IdNo,Processing_Idno,   Bales   ,  Bales_Nos  ,  Delivery_Pcs,Delivery_Qty,Meter_Qty,Delivery_Meters,Delivery_Weight , PackingSlip_Codes , ClothProcessing_Delivery_PackingSlno ,Folding,FabricPurchase_Weaver_Lot_Idno,Del_Colour_IdNo ,Rate ,Amount ,Cloth_Processing_Delivery_Slno) Values ('" & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_DcNo.Text) & "', '" & Trim(vDcNo) & "' ," & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_DcNo.Text))) & ", @DeliveryDate, " & Str(Val(Sno)) & ", " & Str(Val(Led_ID)) & " ," & Str(Val(itgry_id)) & ", " & Str(Val(Itfp_ID)) & ", " & Val(Col_ID) & ", " & Val(Lot_ID).ToString & " , " & Val(Proc_ID) & " ," & Str(Val(.Rows(i).Cells(dgvCol_Details.BALES).Value)) & ",'" & Trim(.Rows(i).Cells(dgvCol_Details.BALES_NOS).Value) & "', " & Val(.Rows(i).Cells(dgvCol_Details.PCS).Value) & ", " & Val(.Rows(i).Cells(dgvCol_Details.QTY).Value) & ", " & Str(Val(.Rows(i).Cells(dgvCol_Details.MTR_QTY).Value)) & ", " & Str(Val(.Rows(i).Cells(dgvCol_Details.METERS).Value)) & " ," & Str(Val(.Rows(i).Cells(dgvCol_Details.WEIGHT).Value)) & " ,'" & Trim(.Rows(i).Cells(dgvCol_Details.PACKINGSLIP_CODE).Value) & "' ," & Str(Val(.Rows(i).Cells(dgvCol_Details.CLOTH_PROCESSING_DELIVERY_PACKINGSLIP_SLNO).Value)) & "," & Val(txt_Folding.Text).ToString & "," & Lot_ID.ToString & "," & Col_ID_Del.ToString & " , " & Str(Val(.Rows(i).Cells(dgvCol_Details.RATE).Value)) & " , " & Str(Val(.Rows(i).Cells(dgvCol_Details.AMOUNT).Value)) & " , " & Val(.Rows(i).Cells(dgvCol_Details.CLOTH_PROCESSING_DELIVERY_SLNO).Value) & " )"
                            cmd.ExecuteNonQuery()
                        End If

                        '        End If

                        '    Next

                        'End With

                        With dgv_BaleSelectionDetails

                            For J = 0 To .RowCount - 1

                                If (Val(.Rows(J).Cells(dgvCol_BaleSeledetails.METERS).Value) <> 0 Or Val(.Rows(J).Cells(dgvCol_BaleSeledetails.PCS).Value) <> 0) And Trim(.Rows(J).Cells(dgvCol_BaleSeledetails.PACKING_SLIP_CODE).Value) <> "" Then

                                    If Val(dgv_BaleSelectionDetails.Rows(J).Cells(dgvCol_BaleSeledetails.CLOTH_PROCESSING_DELIVERY_PACKINGSLIP_SLNO).Value) = Val(dgv_Details.Rows(i).Cells(dgvCol_Details.CLOTH_PROCESSING_DELIVERY_PACKINGSLIP_SLNO).Value) Then

                                        vERR_BALECODE = Trim(.Rows(J).Cells(dgvCol_BaleSeledetails.PACKING_SLIP_CODE).Value)

                                        Nr = 0
                                        cmd.CommandText = "Update Packing_Slip_Head Set Delivery_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "', Delivery_No = '" & Trim(lbl_DcNo.Text) & "', Delivery_DetailsSlNo = " & Str(Val(.Rows(J).Cells(dgvCol_BaleSeledetails.CLOTH_PROCESSING_DELIVERY_PACKINGSLIP_SLNO).Value)) & ", Delivery_Increment = Delivery_Increment + 1, Delivery_Date = @DeliveryDate Where Packing_Slip_Code = '" & Trim(.Rows(J).Cells(dgvCol_BaleSeledetails.PACKING_SLIP_CODE).Value) & "'"
                                        Nr = cmd.ExecuteNonQuery()

                                        If Nr <> 1 Then
                                            Throw New ApplicationException("Invalid PackingSlip Updation for BaleNo : " & Trim(.Rows(J).Cells(dgvCol_BaleSeledetails.PACKING_SLIP_CODE).Value) & " in Sl.No : " & Trim(Val(dgv_Details.Rows(i).Cells(dgvCol_BaleSeledetails.CLOTH_PROCESSING_DELIVERY_PACKINGSLIP_SLNO).Value)))
                                        End If

                                        'If Nr = 0 Then
                                        '    Throw New ApplicationException("Error updating packing slips")
                                        'End If

                                    End If
                                End If

                            Next J

                        End With


                        With dgv_PieceDetails

                            For k = 0 To dgv_PieceDetails.RowCount - 1

                                If Val(dgv_PieceDetails.Rows(k).Cells(dgvCol_PieceDetails.CLOTH_PROCESSING_DELIVERY_PACKINGSLIP_SLNO).Value) <> 0 And Trim(dgv_PieceDetails.Rows(k).Cells(dgvCol_PieceDetails.PCS_NO).Value) <> "" And Val(dgv_PieceDetails.Rows(k).Cells(dgvCol_PieceDetails.METERS).Value) <> 0 And Trim(dgv_PieceDetails.Rows(k).Cells(dgvCol_PieceDetails.LOT_CODE).Value) <> "" Then

                                    If Val(dgv_PieceDetails.Rows(k).Cells(dgvCol_PieceDetails.CLOTH_PROCESSING_DELIVERY_PACKINGSLIP_SLNO).Value) = Val(dgv_Details.Rows(i).Cells(dgvCol_Details.CLOTH_PROCESSING_DELIVERY_PACKINGSLIP_SLNO).Value) Then

                                        vPcsSno = vPcsSno + 1

                                        vPcsTyp_ID = Common_Procedures.ClothType_NameToIdNo(con, dgv_PieceDetails.Rows(k).Cells(dgvCol_PieceDetails.CLOTH_TYPE).Value, tr)
                                        vPcsparty_ID = Common_Procedures.Ledger_AlaisNameToIdNo(con, dgv_PieceDetails.Rows(k).Cells(dgvCol_PieceDetails.PCS_PARTY_NAME).Value, tr)
                                        vPcsClo_ID = Common_Procedures.Cloth_NameToIdNo(con, dgv_PieceDetails.Rows(k).Cells(dgvCol_PieceDetails.CLOTH_NAME).Value, tr)

                                        If clthtyp_ID = 0 Then clthtyp_ID = vPcsTyp_ID
                                        If txt_Folding.Text = 0 Then txt_Folding.Text = "100"

                                        cmd.CommandText = "Insert into Textile_Processing_Delivery_Piece_Details ( ClothProcess_Delivery_Code    ,               Company_IdNo       ,   Cloth_Processing_Delivery_No    ,           for_OrderBy      , ClothProcess_Delivery_Date       ,            Ledger_IdNo        ,         Cloth_IdNo          ,      ClothType_IdNo          ,             Fold_Perc             ,                                                                 ClothProcessing_Delivery_PackingSlno                                    ,                Sl_No      ,                                    Lot_No                                            ,                             Piece_No                                                  ,            PieceType_IdNo     ,                                   Meters                                   ,                                                                Weight              ,                                                      Weight_Meter                                            ,            PieceParty_IdNo    ,                                    Lot_Code                                      ,         PieceCloth_IdNo      ,                                    Loom_No                                              ,                                    Bale_No               ) " &
                                                                        "     Values                     (   '" & Trim(NewCode) & "'   , " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_DcNo.Text) & "', " & Str(Val(vOrdByNo)) & ",       @DeliveryDate            ,      " & Str(Val(Led_ID)) & " , " & Str(Val(itgry_id)) & "   , " & Str(Val(clthtyp_ID)) & ", " & Str(Val(txt_Folding.Text)) & ", " & Str(Val(dgv_PieceDetails.Rows(k).Cells(dgvCol_PieceDetails.CLOTH_PROCESSING_DELIVERY_PACKINGSLIP_SLNO).Value)) & ",  " & Str(Val(vPcsSno)) & ", '" & Trim(dgv_PieceDetails.Rows(k).Cells(dgvCol_PieceDetails.LOT_NO).Value) & "',  '" & Trim(dgv_PieceDetails.Rows(k).Cells(dgvCol_PieceDetails.PCS_NO).Value) & "' ,  " & Str(Val(vPcsTyp_ID)) & ", " & Str(Val(dgv_PieceDetails.Rows(k).Cells(dgvCol_PieceDetails.METERS).Value)) & ", " & Str(Val(dgv_PieceDetails.Rows(k).Cells(dgvCol_PieceDetails.WEIGHT).Value)) & ", " & Str(Val(dgv_PieceDetails.Rows(k).Cells(dgvCol_PieceDetails.WEIGHT_METER).Value)) & ", " & Str(Val(vPcsparty_ID)) & ", '" & Trim(dgv_PieceDetails.Rows(k).Cells(dgvCol_PieceDetails.LOT_CODE).Value) & "', " & Str(Val(vPcsClo_ID)) & " , '" & Trim(dgv_PieceDetails.Rows(k).Cells(dgvCol_PieceDetails.LOOM_NO).Value) & "', '" & Trim(dgv_PieceDetails.Rows(k).Cells(dgvCol_PieceDetails.BALE_NO).Value) & "' ) "
                                        cmd.ExecuteNonQuery()


                                        vPackSlp_Code_FldNm = "PackingSlip_Code_Type" & Trim(Val(vPcsTyp_ID))
                                        vPackSlp_Inc_FldNm = "PackingSlip_Inc_Type" & Trim(Val(vPcsTyp_ID))
                                        vPcsMtr_FldNm = "Type" & Trim(Val(vPcsTyp_ID)) & "_Meters"

                                        Nr = 0
                                        cmd.CommandText = "update Weaver_ClothReceipt_Piece_Details set " & vPackSlp_Code_FldNm & " = '" & Trim(Pk_Condition) & Trim(NewCode) & "',  " & vPackSlp_Inc_FldNm & "  = " & vPackSlp_Inc_FldNm & " + 1 Where lot_code = '" & Trim(dgv_PieceDetails.Rows(k).Cells(dgvCol_PieceDetails.LOT_CODE).Value) & "' and Piece_No = '" & Trim(dgv_PieceDetails.Rows(k).Cells(dgvCol_PieceDetails.PCS_NO).Value) & "' and " & vPcsMtr_FldNm & " <> 0 "
                                        Nr = cmd.ExecuteNonQuery()

                                        If Nr <> 1 Then
                                            Throw New ApplicationException("Invalid Pcs Details  -  LotNo. " & Trim(dgv_PieceDetails.Rows(k).Cells(dgvCol_PieceDetails.LOT_CODE).Value) & " , Piece No = " & Trim(dgv_PieceDetails.Rows(k).Cells(dgvCol_PieceDetails.PCS_NO).Value))
                                            Exit Sub
                                        End If

                                    End If

                                End If

                            Next k

                        End With

                    End If

                Next

            End With

            OpYrCode = Microsoft.VisualBasic.Left(Trim(Common_Procedures.FnRange), 4)
            OpYrCode = Trim(Mid(Val(OpYrCode) - 1, 3, 2)) & "-" & Trim(Microsoft.VisualBasic.Right(OpYrCode, 2))

            cmd.CommandText = "Truncate Table " & Trim(Common_Procedures.EntryTempSubTable) & ""
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Insert into " & Trim(Common_Procedures.EntryTempSubTable) & "(Name1, Int1, Int2, Weight10, Meters1, Weight1  , Weight2) select a.Cloth_Processing_Delivery_Code, a.Cloth_Processing_Delivery_SlNo, a.Item_Idno, a.Folding, a.Delivery_Meters, a.Delivery_Weight, a.Delivery_Pcs from Textile_Processing_Delivery_Details a where a.Cloth_Processing_Delivery_Code = '" & Trim(NewCode) & "'"
            'cmd.CommandText = "Insert into " & Trim(Common_Procedures.EntryTempSubTable) & "(Name1, Int1, Int2, Weight1, Meters1) select a.Cloth_Processing_Delivery_Code, a.Cloth_Processing_Delivery_SlNo, a.Item_Idno, 100, a.Delivery_Meters from Textile_Processing_Delivery_Details a where a.Cloth_Processing_Delivery_Code = '" & Trim(NewCode) & "'"
            Nr = cmd.ExecuteNonQuery()

            cmd.CommandText = "Insert into " & Trim(Common_Procedures.EntryTempSubTable) & "(Name1, Int1, Int2, Weight10, Meters1 , Weight1 , Weight2 ) select tC.Cloth_Processing_Delivery_Code, tC.Cloth_Processing_Delivery_SlNo, tC.Item_Idno, tC.Folding , -1*a.Total_Meters as Mtr, -1*a.Total_Weight as Weight , -1*a.Total_Meters as Pcs from Packing_Slip_Head a, Textile_Processing_Delivery_Details tC where tC.Cloth_Processing_Delivery_Code = '" & Trim(NewCode) & "' and a.Delivery_Code = '" & Trim(Pk_Condition) & "' + tC.Cloth_Processing_Delivery_Code and a.Delivery_DetailsSlNo = tC.ClothProcessing_Delivery_PackingSlno"
            Nr = cmd.ExecuteNonQuery()

            '--------------
            cmd.CommandText = "Truncate Table " & Trim(Common_Procedures.EntryTempTable) & ""
            Nr = cmd.ExecuteNonQuery()

            cmd.CommandText = "Insert into " & Trim(Common_Procedures.EntryTempTable) & "(Int1, Weight10, Meters1, Meters2, Meters3, Meters4, Meters5  , Weight1 , Weight6) select Int2, Weight10, sum(Meters1), sum(Meters2), sum(Meters3), sum(Meters4), sum(Meters5) ,sum(Weight1),sum(Weight2) from " & Trim(Common_Procedures.EntryTempSubTable) & " group by Int2, Weight10 "
            Nr = cmd.ExecuteNonQuery()

            '-------------

            cmd.CommandText = "Insert into " & Trim(Common_Procedures.EntryTempTable) & "(  Int1    ,    Weight10    ,                                                Meters1        ,                                               Meters2         ,                                                       Meters3     ,                                                       Meters4     ,                               Meters5                         ,                                                  Weight1          ,                                 Weight2                          ,                   Weight3                                     ,                                       Weight4                  ,                                   Weight5                    ,                            Weight6 ) " &
                                                                " select                a.Cloth_IdNo,   a.Folding   , (CASE WHEN a.ClothType_IdNo = 1 THEN a.Total_Meters ELSE 0 END), (CASE WHEN a.ClothType_IdNo = 2 THEN a.Total_Meters ELSE 0 END), (CASE WHEN a.ClothType_IdNo = 3 THEN a.Total_Meters ELSE 0 END), (CASE WHEN a.ClothType_IdNo = 4 THEN a.Total_Meters ELSE 0 END), (CASE WHEN a.ClothType_IdNo = 5 THEN a.Total_Meters ELSE 0 END)   , (CASE WHEN a.ClothType_IdNo = 1 THEN a.Total_Weight ELSE 0 END)    ,  (CASE WHEN a.ClothType_IdNo = 2 THEN a.Total_Weight ELSE 0 END), (CASE WHEN a.ClothType_IdNo = 3 THEN a.Total_Weight ELSE 0 END), (CASE WHEN a.ClothType_IdNo = 4 THEN a.Total_Weight ELSE 0 END), (CASE WHEN a.ClothType_IdNo = 5 THEN a.Total_Weight ELSE 0 END)  ,(CASE WHEN a.ClothType_IdNo = 1 THEN a.Total_Meters ELSE 0 END) from Packing_Slip_Head a where a.Delivery_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' and a.Packing_Slip_Code LIKE '%/" & Trim(OpYrCode) & "'"
            Nr = cmd.ExecuteNonQuery()
            cmd.CommandText = "Insert into " & Trim(Common_Procedures.EntryTempTable) & "(      Int1    ,   Weight10    ,                                               Meters1     ,                                   Meters2             ,                                        Meters3             ,                                 Meters4                   ,                    Meters5                               ,              Weight1                                      ,                                 Weight2                          ,                   Weight3                      ,                          Weight4                  ,                                   Weight5                       ,                                                  Weight6  ) " &
                                                                " select                 b.Cloth_IdNo   ,   b.Folding   , (CASE WHEN b.ClothType_IdNo = 1 THEN b.Meters ELSE 0 END), (CASE WHEN b.ClothType_IdNo = 2 THEN b.Meters ELSE 0 END), (CASE WHEN b.ClothType_IdNo = 3 THEN b.Meters ELSE 0 END), (CASE WHEN b.ClothType_IdNo = 4 THEN b.Meters ELSE 0 END), (CASE WHEN b.ClothType_IdNo = 5 THEN b.Meters ELSE 0 END) ,(CASE WHEN b.ClothType_IdNo = 1 THEN b.Weight ELSE 0 END), (CASE WHEN b.ClothType_IdNo = 2 THEN b.Weight ELSE 0 END), (CASE WHEN b.ClothType_IdNo = 3 THEN b.Weight ELSE 0 END), (CASE WHEN b.ClothType_IdNo = 4 THEN b.Weight ELSE 0 END), (CASE WHEN b.ClothType_IdNo = 5 THEN b.Weight ELSE 0 END) , (CASE WHEN b.ClothType_IdNo = 1 THEN b.Meters ELSE 0 END)     from Packing_Slip_Head a, Packing_Slip_Details b where a.Delivery_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' and a.Packing_Slip_Code NOT LIKE '%/" & Trim(OpYrCode) & "' and a.Company_IdNo = b.Company_IdNo and a.Packing_Slip_Code = b.Packing_Slip_Code"
            Nr = cmd.ExecuteNonQuery()

            'cmd.CommandText = "Insert into " & Trim(Common_Procedures.EntryTempTable) & "(Int1, Weight1, Meters1, Meters2, Meters3, Meters4, Meters5 , Weight2, Weight3, Weight4, Weight5, Weight6) select a.Cloth_IdNo, a.Folding, (CASE WHEN a.ClothType_IdNo = 1 THEN a.Total_Meters ELSE 0 END), (CASE WHEN a.ClothType_IdNo = 2 THEN a.Total_Meters ELSE 0 END), (CASE WHEN a.ClothType_IdNo = 3 THEN a.Total_Meters ELSE 0 END), (CASE WHEN a.ClothType_IdNo = 4 THEN a.Total_Meters ELSE 0 END), (CASE WHEN a.ClothType_IdNo = 5 THEN a.Total_Meters ELSE 0 END)  from Packing_Slip_Head a where a.Delivery_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' and a.Packing_Slip_Code LIKE '%/" & Trim(OpYrCode) & "'"
            'Nr = cmd.ExecuteNonQuery()

            'cmd.CommandText = "Insert into " & Trim(Common_Procedures.EntryTempTable) & "(Int1, Weight1, Meters1, Meters2, Meters3, Meters4, Meters5 , Weight2, Weight3, Weight4, Weight5, Weight6) select b.Cloth_IdNo, b.Folding, (CASE WHEN b.ClothType_IdNo = 1 THEN b.Meters ELSE 0 END), (CASE WHEN b.ClothType_IdNo = 2 THEN b.Meters ELSE 0 END), (CASE WHEN b.ClothType_IdNo = 3 THEN b.Meters ELSE 0 END), (CASE WHEN b.ClothType_IdNo = 4 THEN b.Meters ELSE 0 END), (CASE WHEN b.ClothType_IdNo = 5 THEN b.Meters ELSE 0 END) from Packing_Slip_Head a, Packing_Slip_Details b where a.Delivery_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' and a.Packing_Slip_Code NOT LIKE '%/" & Trim(OpYrCode) & "' and a.Company_IdNo = b.Company_IdNo and a.Packing_Slip_Code = b.Packing_Slip_Code"
            'Nr = cmd.ExecuteNonQuery()


            Da = New SqlClient.SqlDataAdapter("select Int1 as Clo_IdNo, Weight10 as FoldPerc, sum(Meters1 ) as Type1_Mtrs, sum(Meters2) as Type2_Mtrs, sum(Meters3) as Type3_Mtrs, sum(Meters4) as Type4_Mtrs, sum(Meters5) as Type5_Mtrs  , sum(Weight1 ) as Type1_Wgt, sum(Weight2) as Type2_Wgt, sum(Weight3) as Type3_Wgt, sum(Weight4) as Type4_Wgt, sum(Weight5) as Type5_Wgt , sum(Weight6) as Noof_Pcs from " & Trim(Common_Procedures.EntryTempTable) & " group by Int1, Weight10  having sum(Meters1) <> 0 or sum(Meters2) <> 0 or sum(Meters3) <> 0 or sum(Meters4) <> 0 or sum(Meters5) <> 0 or sum(Weight1) <> 0 or sum(Weight2) <> 0 or sum(Weight3) <> 0 or sum(Weight4) <> 0 or sum(Weight5) <> 0 or sum(Weight6) <> 0", con)
            'Da = New SqlClient.SqlDataAdapter("select Int1 as Clo_IdNo, Weight10 as FoldPerc, sum(Meters1 ) as Type1_Mtrs, sum(Meters2) as Type2_Mtrs, sum(Meters3) as Type3_Mtrs, sum(Meters4) as Type4_Mtrs, sum(Meters5) as Type5_Mtrs  from " & Trim(Common_Procedures.EntryTempTable) & " group by Int1, Weight10  having sum(Meters1) <> 0 or sum(Meters2) <> 0 or sum(Meters3) <> 0 or sum(Meters4) <> 0 or sum(Meters5) <> 0 ", con)
            Da.SelectCommand.Transaction = tr
            Dt1 = New DataTable
            Da.Fill(Dt1)

            Dim vSTOCK_POSTING_QTY = ""
            Sno = 1000
            vSTOCK_POSTING_QTY = 0
            If Dt1.Rows.Count > 0 Then
                For i = 0 To Dt1.Rows.Count - 1
                    Sno = Sno + 1
                    If Trim(UCase(EntFnYrCode)) <> Trim(UCase(OpYrCode)) Then

                        vFoldPerc = Val(Dt1.Rows(i).Item("FoldPerc").ToString)

                        If Val(vFoldPerc) = 0 Then vFoldPerc = 100

                        vCLOSTK_IN = ""

                        Da4 = New SqlClient.SqlDataAdapter("Select Stock_In from Cloth_Head Where Cloth_Idno = " & Str(Val(Dt1.Rows(i).Item("Clo_IdNo").ToString)), con)
                        If IsNothing(tr) = False Then
                            Da4.SelectCommand.Transaction = tr
                        End If
                        Dt4 = New DataTable
                        Da4.Fill(Dt4)

                        If Dt4.Rows.Count > 0 Then
                            vCLOSTK_IN = Dt4.Rows(0).Item("Stock_In").ToString
                        End If


                        vSTOCK_POSTING_QTY = 0

                        If Trim(UCase(vCLOSTK_IN)) = "PCS" Then

                            vSTOCK_POSTING_QTY = Val(Dt1.Rows(i).Item("Noof_Pcs").ToString)

                        Else

                            vSTOCK_POSTING_QTY = Val(Dt1.Rows(i).Item("Type1_Mtrs").ToString)

                        End If

                        '--- CODE BY GOPI -- 2024-12-17

                        cmd.CommandText = "Insert into Stock_Cloth_Processing_Details ( Reference_Code                             ,             Company_IdNo         ,           Reference_No       ,                               for_OrderBy                             , Reference_Date,        StockOff_IdNo              ,   DeliveryTo_Idno       ,   ReceivedFrom_Idno                ,         Entry_ID     ,       Party_Bill_No  ,       Particulars      ,           Sl_No        ,                         Cloth_Idno                      ,                Folding        ,                         Meters_Type1                     ,                         Meters_Type2                     ,                         Meters_Type3                    ,                         Meters_Type4                     ,                         Meters_Type5                     ,Lot_IdNo               ,Process_IdNo               ,                           Weight_Type1                       ,                          Weight_Type2                         ,                             Weight_Type3                       ,                      Weight_Type4                             ,                                 Weight_Type5    ,  ClothSales_OrderCode_forSelection   ) " &
                                                    " Values                              ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_DcNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_DcNo.Text))) & ",  @DeliveryDate,  " & Str(Val(vStkOf_Pos_IdNo)) & "," & Led_ID.ToString & " , " & Str(Val(Del_Godown_Led_ID)) & ", '" & Trim(EntID) & "', '" & Trim(PBlNo) & "', '" & Trim(Partcls) & "', " & Str(Val(Sno)) & "  , " & Str(Val(Dt1.Rows(i).Item("Clo_IdNo").ToString)) & " ,   " & Str(Val(vFoldPerc)) & " , " & Str(Val(vSTOCK_POSTING_QTY)) & ", " & Str(Val(Dt1.Rows(i).Item("Type2_Mtrs").ToString)) & ", " & Str(Val(Dt1.Rows(i).Item("Type3_Mtrs").ToString)) & ", " & Str(Val(Dt1.Rows(i).Item("Type4_Mtrs").ToString)) & ", " & Str(Val(Dt1.Rows(i).Item("Type5_Mtrs").ToString)) & "," & Lot_ID.ToString & "," & Proc_ID.ToString & "   , " & Str(Val(Dt1.Rows(i).Item("Type1_Wgt").ToString)) & ", " & Str(Val(Dt1.Rows(i).Item("Type2_Wgt").ToString)) & " , " & Str(Val(Dt1.Rows(i).Item("Type3_Wgt").ToString)) & "  , " & Str(Val(Dt1.Rows(i).Item("Type4_Wgt").ToString)) & " ,  " & Str(Val(Dt1.Rows(i).Item("Type5_Wgt").ToString)) & "  ,  '" & Trim(cbo_ClothSales_OrderCode_forSelection.Text) & "'  ) "
                        cmd.ExecuteNonQuery()


                        '--- CMD DATE 2024-12-17 CMD BY GOPI 

                        'cmd.CommandText = "Insert into Stock_Cloth_Processing_Details ( Reference_Code                             ,             Company_IdNo         ,           Reference_No       ,                               for_OrderBy                             , Reference_Date,        StockOff_IdNo              ,   DeliveryTo_Idno       ,   ReceivedFrom_Idno                ,         Entry_ID     ,       Party_Bill_No  ,       Particulars      ,           Sl_No        ,                         Cloth_Idno                      ,                Folding        ,                         Meters_Type1                     ,                         Meters_Type2                     ,                         Meters_Type3                    ,                         Meters_Type4                     ,                         Meters_Type5                     ,Lot_IdNo               ,Process_IdNo               ,                           Weight_Type1                       ,                          Weight_Type2                         ,                             Weight_Type3                       ,                      Weight_Type4                             ,                                 Weight_Type5    ,  ClothSales_OrderCode_forSelection   ) " &
                        '                            " Values                              ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_DcNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_DcNo.Text))) & ",  @DeliveryDate,  " & Str(Val(vStkOf_Pos_IdNo)) & "," & Led_ID.ToString & " , " & Str(Val(Del_Godown_Led_ID)) & ", '" & Trim(EntID) & "', '" & Trim(PBlNo) & "', '" & Trim(Partcls) & "', " & Str(Val(Sno)) & "  , " & Str(Val(Dt1.Rows(i).Item("Clo_IdNo").ToString)) & " ,   " & Str(Val(vFoldPerc)) & " , " & Str(Val(Dt1.Rows(i).Item("Type1_Mtrs").ToString)) & ", " & Str(Val(Dt1.Rows(i).Item("Type2_Mtrs").ToString)) & ", " & Str(Val(Dt1.Rows(i).Item("Type3_Mtrs").ToString)) & ", " & Str(Val(Dt1.Rows(i).Item("Type4_Mtrs").ToString)) & ", " & Str(Val(Dt1.Rows(i).Item("Type5_Mtrs").ToString)) & "," & Lot_ID.ToString & "," & Proc_ID.ToString & "   , " & Str(Val(Dt1.Rows(i).Item("Type1_Wgt").ToString)) & ", " & Str(Val(Dt1.Rows(i).Item("Type2_Wgt").ToString)) & " , " & Str(Val(Dt1.Rows(i).Item("Type3_Wgt").ToString)) & "  , " & Str(Val(Dt1.Rows(i).Item("Type4_Wgt").ToString)) & " ,  " & Str(Val(Dt1.Rows(i).Item("Type5_Wgt").ToString)) & "  ,  '" & Trim(cbo_ClothSales_OrderCode_forSelection.Text) & "'  ) "
                        'cmd.ExecuteNonQuery()


                        ' --- OLD
                        'cmd.CommandText = "Insert into Stock_Cloth_Processing_Details ( Reference_Code                             ,             Company_IdNo         ,           Reference_No       ,                               for_OrderBy                             , Reference_Date,        StockOff_IdNo              ,   DeliveryTo_Idno       ,   ReceivedFrom_Idno                ,         Entry_ID     ,       Party_Bill_No  ,       Particulars      ,           Sl_No        ,                         Cloth_Idno                      ,                Folding        ,                         Meters_Type1                     ,                         Meters_Type2                     ,                         Meters_Type3                    ,                         Meters_Type4                     ,                         Meters_Type5                     ,Lot_IdNo               ,Process_IdNo              ,  ClothSales_OrderCode_forSelection   ) " &
                        '                        " Values                              ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_DcNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_DcNo.Text))) & ",  @DeliveryDate,  " & Str(Val(vStkOf_Pos_IdNo)) & "," & Led_ID.ToString & " , " & Str(Val(Del_Godown_Led_ID)) & ", '" & Trim(EntID) & "', '" & Trim(PBlNo) & "', '" & Trim(Partcls) & "', " & Str(Val(Sno)) & "  , " & Str(Val(Dt1.Rows(i).Item("Clo_IdNo").ToString)) & " ,   " & Str(Val(vFoldPerc)) & " , " & Str(Val(Dt1.Rows(i).Item("Type1_Mtrs").ToString)) & ", " & Str(Val(Dt1.Rows(i).Item("Type2_Mtrs").ToString)) & ", " & Str(Val(Dt1.Rows(i).Item("Type3_Mtrs").ToString)) & ", " & Str(Val(Dt1.Rows(i).Item("Type4_Mtrs").ToString)) & ", " & Str(Val(Dt1.Rows(i).Item("Type5_Mtrs").ToString)) & "," & Lot_ID.ToString & "," & Proc_ID.ToString & "  ,  '" & Trim(cbo_ClothSales_OrderCode_forSelection.Text) & "'   ) "
                        'cmd.ExecuteNonQuery()

                    End If

                Next
            End If

            Dt4.Clear()
            Da4.Dispose()

            If Val(Common_Procedures.settings.NegativeStock_Restriction) = 1 Then
                cmd.CommandText = "Insert into TempTable_For_NegativeStock ( Stock_Type, Reference_Code, Reference_Date, Company_Idno, Ledger_Idno           , Item_IdNo, Rack_IdNo ) " &
                                        " Select                               'PI'    , Reference_Code, Reference_Date, Company_IdNo, ReceivedFrom_StockIdNo, Item_IdNo,     0        from Stock_Item_Processing_Details where Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()

                If Common_Procedures.Check_is_Negative_Stock_Status(con, tr) = True Then Exit Sub

            End If


            tr.Commit()

            Dt1.Dispose()
            Da.Dispose()

            If SaveAll_STS <> True Then
                MessageBox.Show("Saved Successfully!!!", "FOR SAVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Information)
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
            If InStr(1, Trim(LCase(ex.Message)), Trim(LCase("CK_PackingSlip_Delivery_Increment"))) > 0 Then
                MessageBox.Show("Invalid : Duplicate Bale Selection - " & vERR_BALECODE, "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            ElseIf InStr(1, Trim(LCase(ex.Message)), Trim(LCase("CK_Textile_Processing_Delivery_Piece_Details_1"))) > 0 Then
                MessageBox.Show("Invalid Delivery Meters : Lesser than Invoice/Return Meters ", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            ElseIf InStr(1, Trim(LCase(ex.Message)), Trim(LCase("CK_Textile_Processing_Delivery_Details"))) > 0 Then
                MessageBox.Show("Invaild Cloth_Processing_Delivery_Slno : " & Chr(13) & "The Delivery cannot accept a lesser than zero ", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Else
                MessageBox.Show(ex.Message, "FOR SAVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If
        End Try

        If dtp_Date.Enabled And dtp_Date.Visible Then dtp_Date.Focus()

    End Sub


    Private Sub Get_GreyItemMtr_Qty()
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim GITID As Integer



        GITID = Common_Procedures.Processed_Item_NameToIdNo(con, cbo_itemgrey.Text)



        If GITID <> 0 Then

            Da = New SqlClient.SqlDataAdapter("select * from Processed_Item_Head where Processed_Item_IdNo = " & Str(Val(GITID)) & " and Processed_Item_Type= 'GREY' ", con)
            Da.Fill(Dt)


            If Dt.Rows.Count > 0 Then

                dgv_Details.CurrentRow.Cells(dgvCol_Details.MTR_QTY).Value = Dt.Rows(0).Item("Meter_Qty").ToString

            End If


            Dt.Clear()
            Dt.Dispose()
            Da.Dispose()

        End If

    End Sub

    Private Sub Total_Calculation()

        Dim vTotPcs As Single, vTotMtrs As Single, vtotweight As Single, vtotqty As Single
        Dim vTotBales As Integer
        Dim vTotAmt = ""

        Dim i As Integer
        Dim sno As Integer

        vTotPcs = 0 : vTotMtrs = 0 : vtotweight = 0 : sno = 0 : vTotAmt = 0
        With dgv_Details
            For i = 0 To dgv_Details.Rows.Count - 1

                sno = sno + 1

                .Rows(i).Cells(dgvCol_Details.SLNO).Value = sno

                If Val(dgv_Details.Rows(i).Cells(dgvCol_Details.PCS).Value) <> 0 Or Val(dgv_Details.Rows(i).Cells(dgvCol_Details.METERS).Value) <> 0 Or Val(dgv_Details.Rows(i).Cells(dgvCol_Details.WEIGHT).Value) <> 0 Then
                    vTotPcs = vTotPcs + Val(dgv_Details.Rows(i).Cells(dgvCol_Details.PCS).Value)
                    vtotqty = vtotqty + Val(dgv_Details.Rows(i).Cells(dgvCol_Details.QTY).Value)
                    vTotMtrs = vTotMtrs + Val(dgv_Details.Rows(i).Cells(dgvCol_Details.METERS).Value)
                    vtotweight = vtotweight + Val(dgv_Details.Rows(i).Cells(dgvCol_Details.WEIGHT).Value)
                    vTotBales = vTotBales + Val(dgv_Details.Rows(i).Cells(dgvCol_Details.BALES).Value)

                    vTotAmt = vTotAmt + Val(dgv_Details.Rows(i).Cells(dgvCol_Details.AMOUNT).Value)

                End If
            Next
        End With

        If dgv_Details_Total.Rows.Count <= 0 Then dgv_Details_Total.Rows.Add()

        dgv_Details_Total.Rows(0).Cells(dgvCol_Details.PCS).Value = Val(vTotPcs)
        dgv_Details_Total.Rows(0).Cells(dgvCol_Details.QTY).Value = Val(vtotqty)
        dgv_Details_Total.Rows(0).Cells(dgvCol_Details.BALES).Value = Val(vTotBales)
        dgv_Details_Total.Rows(0).Cells(dgvCol_Details.METERS).Value = Format(Val(vTotMtrs), "#########0.00")
        dgv_Details_Total.Rows(0).Cells(dgvCol_Details.WEIGHT).Value = Format(Val(vtotweight), "#########0.000")

        dgv_Details_Total.Rows(0).Cells(dgvCol_Details.AMOUNT).Value = Format(Val(vTotAmt), "#########0.00")

    End Sub

    Private Sub cbo_Ledger_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Ledger.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", " (Ledger_Type = '' and ( AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14) )", "(Ledger_idno = 0)")

    End Sub

    Private Sub cbo_Ledger_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Ledger.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Ledger, CBO_JobNO, Cbo_ProcessingHEAD, "Ledger_AlaisHead", "Ledger_DisplayName", " (Ledger_Type = '' and ( AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14) )", "(Ledger_idno = 0)")
    End Sub

    Private Sub cbo_Ledger_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Ledger.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Ledger, Cbo_ProcessingHEAD, "Ledger_AlaisHead", "Ledger_DisplayName", " (Ledger_Type = '' and ( AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14) ) ", "(Ledger_idno = 0)")

    End Sub

    Private Sub cbo_Ledger_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Ledger.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Common_Procedures.MDI_LedType = ""
            Dim f As New Ledger_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Ledger.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()
        End If
    End Sub

    Private Sub dgv_Details_CellClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellClick
        With dgv_Details
            If dgvCol_Details.METERS Then
                Show_Item_CurrentStock(e.RowIndex)
                .Focus()
            End If
        End With
    End Sub

    Private Sub dgv_Details_CellEndEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellEndEdit
        With dgv_Details

            If .CurrentCell.ColumnIndex = dgvCol_Details.METERS Or .CurrentCell.ColumnIndex = dgvCol_Details.MTR_QTY Then
                If Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value) <> 0 Then
                    .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = Format(Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value), "#########0.00")
                End If
            End If
            If .CurrentCell.ColumnIndex = dgvCol_Details.WEIGHT Then
                If Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value) <> 0 Then
                    .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = Format(Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value), "#########0.000")
                End If
            End If
            Total_Calculation()

        End With
    End Sub
    Private Sub Set_Max_DetailsSlNo(ByVal RowNo As Integer, ByVal DetSlNo_ColNo As Integer)
        Dim MaxSlNo As Integer = 0
        Dim i As Integer

        With dgv_Details
            For i = 0 To .Rows.Count - 1
                If Val(dgv_Details.Rows(i).Cells(DetSlNo_ColNo).Value) > Val(MaxSlNo) Then
                    MaxSlNo = Val(.Rows(i).Cells(DetSlNo_ColNo).Value)
                End If
            Next
            dgv_Details.Rows(RowNo).Cells(DetSlNo_ColNo).Value = Val(MaxSlNo) + 1
        End With

    End Sub
    Private Sub dgv_Details_CellEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellEnter
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim Dt2 As New DataTable
        Dim Dt3 As New DataTable
        Dim rect As Rectangle

        If FrmLdSTS = True Then Exit Sub

        With dgv_Details

            If Val(.Rows(e.RowIndex).Cells(dgvCol_Details.CLOTH_PROCESSING_DELIVERY_SLNO).Value) = 0 Then
                Set_Max_DetailsSlNo(e.RowIndex, dgvCol_Details.CLOTH_PROCESSING_DELIVERY_SLNO)
            End If

            If Val(.Rows(e.RowIndex).Cells(dgvCol_Details.CLOTH_PROCESSING_DELIVERY_PACKINGSLIP_SLNO).Value) = 0 Then

                Set_Max_DetailsSlNo(e.RowIndex, dgvCol_Details.CLOTH_PROCESSING_DELIVERY_PACKINGSLIP_SLNO)

                'Set_Max_DetailsSlNo(e.RowIndex, 17)
                'If e.RowIndex = 0 Then
                '    .Rows(e.RowIndex).Cells(15).Value = 1
                'Else
                '    .Rows(e.RowIndex).Cells(15).Value = Val(.Rows(e.RowIndex - 1).Cells(15).Value) + 1
                'End If
            End If
                If Val(.CurrentRow.Cells(dgvCol_Details.SLNO).Value) = 0 Then
                .CurrentRow.Cells(dgvCol_Details.SLNO).Value = .CurrentRow.Index + 1
            End If

            If e.ColumnIndex = dgvCol_Details.ITEM_GREY And (Val(dgv_Details.Rows(dgv_Details.CurrentCell.RowIndex).Cells(dgvCol_Details.RECEIPT_METERS).Value)) = 0 Then

                If cbo_itemgrey.Visible = False Or Val(cbo_itemgrey.Tag) <> e.RowIndex Then

                    cbo_itemfp.Tag = -1


                    Dim RET_TYPE As String = "CLOTH"

                    If Len(Trim(Process_Outputs)) > 1 Then
                        If Mid(Trim(Process_Outputs), 2, 1) = "1" Then
                            RET_TYPE = "FP"
                        End If
                    End If


                    'Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Processed_Item_Head", "Processed_Item_Name", "(Processed_Item_Type = 'FP')", "(Processed_Item_Idno = 0)")
                    'Else
                    'Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Cloth_Head", "Cloth_Name", "(Cloth_Type = 'PROCESSED FABRIC')", "(Cloth_Idno = 0)")
                    'End If

                    'where Processed_Item_Type = 'GREY'

                    If RET_TYPE = "FP" Then
                        Da = New SqlClient.SqlDataAdapter("select Processed_Item_Name from Processed_Item_Head  order by Processed_item_Name", con)
                        Dt1 = New DataTable
                        Da.Fill(Dt1)
                        cbo_itemgrey.DataSource = Dt1
                        cbo_itemgrey.DisplayMember = "Processed_Item_Name"
                    Else
                        Da = New SqlClient.SqlDataAdapter("select Cloth_Name from Cloth_Head  order by Cloth_Name", con)
                        Dt1 = New DataTable
                        Da.Fill(Dt1)
                        cbo_itemgrey.DataSource = Dt1
                        cbo_itemgrey.DisplayMember = "Cloth_Name"
                    End If

                    rect = .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False)

                    cbo_itemgrey.Left = .Left + rect.Left  '  .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Left
                    cbo_itemgrey.Top = .Top + rect.Top  ' .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Top

                    cbo_itemgrey.Width = rect.Width  ' .CurrentCell.Size.Width
                    cbo_itemgrey.Height = rect.Height  ' rect.Height
                    cbo_itemgrey.Text = .CurrentCell.Value  '  Trim(.CurrentRow.Cells(e.ColumnIndex).Value)

                    cbo_itemgrey.Tag = Val(e.RowIndex)
                    cbo_itemgrey.Visible = True

                    cbo_itemgrey.BringToFront()
                    cbo_itemgrey.Focus()

                End If


            Else

                cbo_itemgrey.Visible = False

            End If

            If e.ColumnIndex = dgvCol_Details.COLOUR_DELY And (Val(.Rows(.CurrentCell.RowIndex).Cells(dgvCol_Details.RECEIPT_METERS).Value)) = 0 Then

                If cbo_Colour_Dely.Visible = False Or Val(cbo_Colour_Dely.Tag) <> e.RowIndex Then

                    cbo_Colour_Dely.Tag = -1
                    Da = New SqlClient.SqlDataAdapter("select Colour_Name from Colour_Head order by Colour_Name", con)
                    Dt3 = New DataTable
                    Da.Fill(Dt3)
                    cbo_Colour_Dely.DataSource = Dt3
                    cbo_Colour_Dely.DisplayMember = "Colour_Name"

                    rect = .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False)

                    cbo_Colour_Dely.Left = .Left + rect.Left  '  .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Left
                    cbo_Colour_Dely.Top = .Top + rect.Top  ' .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Top
                    cbo_Colour_Dely.Width = rect.Width  ' .CurrentCell.Size.Width
                    cbo_Colour_Dely.Height = rect.Height  ' rect.Height

                    cbo_Colour_Dely.Text = .CurrentCell.Value  '  Trim(.CurrentRow.Cells(e.ColumnIndex).Value)

                    cbo_Colour_Dely.Tag = Val(e.RowIndex)
                    cbo_Colour_Dely.Visible = True

                    cbo_Colour_Dely.BringToFront()
                    cbo_Colour_Dely.Focus()

                End If

            Else

                cbo_Colour_Dely.Visible = False

            End If


            If e.ColumnIndex = dgvCol_Details.ITEM_FP And (Val(.Rows(.CurrentCell.RowIndex).Cells(dgvCol_Details.RECEIPT_METERS).Value)) = 0 Then

                If cbo_itemfp.Visible = False Or Val(cbo_itemfp.Tag) <> e.RowIndex Then

                    cbo_itemfp.Tag = -1

                    Da = New SqlClient.SqlDataAdapter("select Processed_Item_Name from Processed_Item_Head where Processed_Item_Type = 'FP' order by Processed_Item_Name", con)
                    Dt2 = New DataTable
                    Da.Fill(Dt2)
                    cbo_itemfp.DataSource = Dt2
                    cbo_itemfp.DisplayMember = "Processed_Item_Name"

                    rect = .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False)

                    cbo_itemfp.Left = .Left + rect.Left  '  .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Left
                    cbo_itemfp.Top = .Top + rect.Top  ' .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Top
                    cbo_itemfp.Width = rect.Width  ' .CurrentCell.Size.Width
                    cbo_itemfp.Height = rect.Height  ' rect.Height

                    cbo_itemfp.Text = .CurrentCell.Value  '  Trim(.CurrentRow.Cells(e.ColumnIndex).Value)

                    cbo_itemfp.Tag = Val(e.RowIndex)
                    cbo_itemfp.Visible = True

                    cbo_itemfp.BringToFront()
                    cbo_itemfp.Focus()

                    'cbo_Grid_CountName.Visible = False
                    'cbo_Grid_YarnType.Visible = False

                End If

            Else

                cbo_itemfp.Visible = False
                'cbo_Grid_YarnType.Tag = -1
                'cbo_Grid_YarnType.Text = ""

            End If

            If e.ColumnIndex = dgvCol_Details.COLOUR And (Val(.Rows(.CurrentCell.RowIndex).Cells(dgvCol_Details.RECEIPT_METERS).Value)) = 0 Then

                If cbo_Colour.Visible = False Or Val(cbo_Colour.Tag) <> e.RowIndex Then

                    cbo_Colour.Tag = -1
                    Da = New SqlClient.SqlDataAdapter("select Colour_Name from Colour_Head order by Colour_Name", con)
                    Dt3 = New DataTable
                    Da.Fill(Dt3)
                    cbo_Colour.DataSource = Dt3
                    cbo_Colour.DisplayMember = "Colour_Name"

                    rect = .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False)

                    cbo_Colour.Left = .Left + rect.Left  '  .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Left
                    cbo_Colour.Top = .Top + rect.Top  ' .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Top
                    cbo_Colour.Width = rect.Width  ' .CurrentCell.Size.Width
                    cbo_Colour.Height = rect.Height  ' rect.Height

                    cbo_Colour.Text = .CurrentCell.Value  '  Trim(.CurrentRow.Cells(e.ColumnIndex).Value)

                    cbo_Colour.Tag = Val(e.RowIndex)
                    cbo_Colour.Visible = True

                    cbo_Colour.BringToFront()
                    cbo_Colour.Focus()

                    'cbo_Grid_CountName.Visible = False
                    'cbo_Grid_MillName.Visible = False

                End If

            Else

                cbo_Colour.Visible = False
                'cbo_Grid_MillName.Tag = -1
                'cbo_Grid_MillName.Text = ""

            End If


            If (e.ColumnIndex = dgvCol_Details.BALES Or e.ColumnIndex = dgvCol_Details.BALES_NOS) Then

                rect = .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False)

                pnl_BaleSelection_ToolTip.Left = .Left + rect.Left
                pnl_BaleSelection_ToolTip.Top = .Top + rect.Top + rect.Height + 50

                pnl_BaleSelection_ToolTip.Visible = True

            Else
                pnl_BaleSelection_ToolTip.Visible = False

            End If


            If btn_PieceSelection.Visible = True Then

                If e.ColumnIndex = dgvCol_Details.PCS Then

                    rect = .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False)

                    pnl_PieceSelection_ToolTip.Left = .Left + rect.Left
                    pnl_PieceSelection_ToolTip.Top = .Top + rect.Top + rect.Height + 3

                    pnl_PieceSelection_ToolTip.Visible = True

                Else
                    pnl_PieceSelection_ToolTip.Visible = False

                End If

            End If


            'If e.ColumnIndex = dgvCol_Details.METERS And dgv_LevColNo <> 12 Then
            '    Show_Item_CurrentStock(e.RowIndex)
            '    .Focus()
            'End If

        End With
    End Sub

    Private Sub dgv_Details_CellLeave(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellLeave
        With dgv_Details
            dgv_LevColNo = .CurrentCell.ColumnIndex
            If .CurrentCell.ColumnIndex = dgvCol_Details.METERS Then
                If Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value) <> 0 Then
                    .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = Format(Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value), "#########0.00")
                End If
            End If
            If .CurrentCell.ColumnIndex = dgvCol_Details.WEIGHT Then
                If Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value) <> 0 Then
                    .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = Format(Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value), "#########0.000")
                End If
            End If
        End With
    End Sub

    Private Sub dgv_Details_CellValueChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellValueChanged
        Dim i As Integer
        Dim vTotMtrs As Single
        On Error Resume Next

        If FrmLdSTS = True Then Exit Sub
        With dgv_Details
            If .Visible Then
                If IsNothing(.CurrentCell) Then Exit Sub
                If .CurrentCell.ColumnIndex = dgvCol_Details.PCS Or .CurrentCell.ColumnIndex = dgvCol_Details.QTY Or .CurrentCell.ColumnIndex = dgvCol_Details.MTR_QTY Or .CurrentCell.ColumnIndex = dgvCol_Details.METERS Or .CurrentCell.ColumnIndex = dgvCol_Details.WEIGHT Or .CurrentCell.ColumnIndex = dgvCol_Details.RATE Then
                    If .CurrentCell.ColumnIndex = dgvCol_Details.QTY Or .CurrentCell.ColumnIndex = dgvCol_Details.MTR_QTY Then
                        If .Columns(dgvCol_Details.QTY).Visible = True And .Columns(dgvCol_Details.MTR_QTY).Visible = True Then
                            .Rows(.CurrentCell.RowIndex).Cells(dgvCol_Details.METERS).Value = Val(dgv_Details.Rows(.CurrentCell.RowIndex).Cells(dgvCol_Details.QTY).Value) * Val(dgv_Details.Rows(.CurrentCell.RowIndex).Cells(dgvCol_Details.MTR_QTY).Value)
                        End If
                    End If

                    If Val(.CurrentCell.ColumnIndex) = dgvCol_Details.METERS Or Val(.CurrentCell.ColumnIndex) = dgvCol_Details.RATE Then
                        .Rows(.CurrentCell.RowIndex).Cells(dgvCol_Details.AMOUNT).Value = Val(dgv_Details.Rows(.CurrentCell.RowIndex).Cells(dgvCol_Details.METERS).Value) * Val(dgv_Details.Rows(.CurrentCell.RowIndex).Cells(dgvCol_Details.RATE).Value)
                    End If


                    Total_Calculation()
                End If
            End If
        End With
    End Sub

    Private Sub dgv_Details_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_Details.KeyDown

        'On Error Resume Next
        On Error Resume Next
        vcbo_KeyDwnVal = e.KeyValue
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
                    cbo_TransportName.Focus()
                End If
            End If
        End With
    End Sub
    Private Sub dgv_Details_EditingControlShowing(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewEditingControlShowingEventArgs) Handles dgv_Details.EditingControlShowing
        dgtxt_details = CType(dgv_Details.EditingControl, DataGridViewTextBoxEditingControl)
    End Sub
    Private Sub dgtxt_Details_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgtxt_details.Enter
        dgv_Details.EditingControl.BackColor = Color.Lime
        dgv_Details.EditingControl.ForeColor = Color.Blue
        dgtxt_details.SelectAll()
    End Sub

    Private Sub dgtxt_details_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgtxt_details.KeyDown
        Try
            With dgv_Details
                vcbo_KeyDwnVal = e.KeyValue
                If e.KeyValue = Keys.Delete Then
                    If Val(.Rows(.CurrentCell.RowIndex).Cells(dgvCol_Details.RECEIPT_METERS).Value) <> 0 Then
                        e.Handled = True
                    End If
                End If
            End With

        Catch ex As Exception
            '----

        End Try
    End Sub

    Private Sub dgtxt_Details_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dgtxt_details.KeyPress

        Try

            If Not IsNothing(dgv_Details.CurrentCell) Then

                If Val(dgv_Details.Rows(dgv_Details.CurrentCell.RowIndex).Cells(dgvCol_Details.RECEIPT_METERS).Value) <> 0 Then
                    e.Handled = True
                End If


                If dgv_Details.CurrentCell.ColumnIndex <> dgvCol_Details.BALES_NOS Then
                    If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
                        e.Handled = True
                    End If
                End If

            End If

        Catch ex As Exception
            '---
        End Try

    End Sub

    Private Sub dgv_Details_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_Details.KeyUp
        Dim n As Integer

        Try
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
                        .Rows(i).Cells(dgvCol_Details.SLNO).Value = i + 1
                    Next

                End With

                Total_Calculation()

            End If

        Catch ex As Exception
            '-----
        End Try

    End Sub

    Private Sub dgv_Details_RowsAdded(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewRowsAddedEventArgs) Handles dgv_Details.RowsAdded
        Dim n As Integer
        If FrmLdSTS = True Then Exit Sub
        If IsNothing(dgv_Details.CurrentCell) Then Exit Sub
        With dgv_Details
            n = .RowCount
            .Rows(n - 1).Cells(dgvCol_Details.SLNO).Value = Val(n)

            If Val(.Rows(e.RowIndex).Cells(dgvCol_Details.CLOTH_PROCESSING_DELIVERY_PACKINGSLIP_SLNO).Value) = 0 Then
                Set_Max_DetailsSlNo(e.RowIndex, dgvCol_Details.CLOTH_PROCESSING_DELIVERY_PACKINGSLIP_SLNO)

                'Set_Max_DetailsSlNo(e.RowIndex, 17)
                'If e.RowIndex = 0 Then
                '    .Rows(e.RowIndex).Cells(15).Value = 1
                'Else
                '    .Rows(e.RowIndex).Cells(15).Value = Val(.Rows(e.RowIndex - 1).Cells(15).Value) + 1
                'End If
            End If
            If Val(dgv_Details.Rows(e.RowIndex).Cells(dgvCol_Details.CLOTH_PROCESSING_DELIVERY_SLNO).Value) = 0 Then
                Set_Max_DetailsSlNo(e.RowIndex, dgvCol_Details.CLOTH_PROCESSING_DELIVERY_SLNO)
            End If
        End With

    End Sub

    Private Sub cbo_LotNo_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_LotNo.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Lot_Head", "Lot_No", "", "(Lot_Idno=0)")

    End Sub

    Private Sub cbo_Lotno_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_LotNo.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_LotNo, cbo_Colour, cbo_Processing, "Lot_Head", "Lot_No", "", "(Lot_Idno=0)")
        With dgv_Details

            If (e.KeyValue = 38 And cbo_LotNo.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
                .Focus()
                .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex - 1)
            End If

            If (e.KeyValue = 40 And cbo_LotNo.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
                .Focus()
                .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 1)
            End If

        End With
    End Sub

    Private Sub cbo_lotno_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_LotNo.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_LotNo, cbo_Processing, "Lot_Head", "Lot_No", "", "(Lot_Idno=0)")
        If Asc(e.KeyChar) = 13 Then

            With dgv_Details

                .Focus()
                .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)

            End With

        End If
    End Sub

    Private Sub cbo_Lotno_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_LotNo.KeyUp
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

    Private Sub cbo_lotno_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_LotNo.TextChanged
        Try
            If cbo_LotNo.Visible Then
                With dgv_Details
                    If Val(cbo_LotNo.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = dgvCol_Details.LOT_NO Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_LotNo.Text)
                    End If
                End With
            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub cbo_Colour_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Colour.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "colour_Head", "colour_Name", "", "(Colour_IdNo = 0)")

    End Sub
    Private Sub cbo_colour_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Colour.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Colour, cbo_itemfp, cbo_LotNo, "colour_Head", "colour_Name", "", "(Colour_IdNo = 0)")
        With dgv_Details

            If (e.KeyValue = 38 And cbo_Colour.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
                .Focus()
                .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex - 1)
            End If

            If (e.KeyValue = 40 And cbo_Colour.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
                .Focus()
                .CurrentCell = .Rows(.CurrentRow.Index).Cells(dgvCol_Details.BALES)
            End If

        End With
    End Sub

    Private Sub cbo_colour_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Colour.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Colour, cbo_LotNo, "colour_Head", "colour_Name", "", "(Colour_IdNo = 0)")
        If Asc(e.KeyChar) = 13 Then

            With dgv_Details

                .Focus()
                .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(dgvCol_Details.BALES)

            End With

        End If
    End Sub

    Private Sub cbo_colour_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Colour.KeyUp
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
                With dgv_Details
                    If Val(cbo_Colour.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = dgvCol_Details.COLOUR Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_Colour.Text)
                    End If
                End With
            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub cbo_Colour_Dely_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Colour_Dely.GotFocus

        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "colour_Head", "colour_Name", "", "(Colour_IdNo = 0)")

    End Sub
    Private Sub cbo_Colour_Dely_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Colour_Dely.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Colour_Dely, cbo_itemgrey, cbo_itemfp, "colour_Head", "colour_Name", "", "(Colour_IdNo = 0)")
        With dgv_Details

            If (e.KeyValue = 38 And cbo_Colour_Dely.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
                .Focus()
                .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex - 1)
            End If

            If (e.KeyValue = 40 And cbo_Colour_Dely.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
                .Focus()
                .CurrentCell = .Rows(.CurrentRow.Index).Cells(dgvCol_Details.BALES)
            End If

        End With
    End Sub

    Private Sub cbo_Colour_Dely_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Colour_Dely.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Colour_Dely, cbo_LotNo, "colour_Head", "colour_Name", "", "(Colour_IdNo = 0)")
        If Asc(e.KeyChar) = 13 Then

            With dgv_Details

                .Focus()
                .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(dgvCol_Details.ITEM_FP)

            End With

        End If
    End Sub

    Private Sub cbo_Colour_Dely_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Colour_Dely.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Dim f As New Color_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Colour_Dely.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If
    End Sub

    Private Sub cbo_Colour_Dely_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Colour_Dely.TextChanged
        Try
            If cbo_Colour_Dely.Visible Then
                With dgv_Details
                    If Val(cbo_Colour_Dely.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = dgvCol_Details.COLOUR_DELY Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_Colour_Dely.Text)
                    End If
                End With
            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub cbo_Processing_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Processing.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Process_Head", "Process_Name", "", "(Process_Idno=0)")

    End Sub

    Private Sub cbo_processing_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Processing.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Processing, cbo_LotNo, Nothing, "Process_Head", "Process_Name", "", "(Process_Idno=0)")
        With dgv_Details

            If (e.KeyValue = 38 And cbo_Processing.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
                .Focus()
                .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex - 1)
            End If

            If (e.KeyValue = 40 And cbo_Processing.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
                .Focus()
                .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 1)
            End If

        End With
    End Sub

    Private Sub cbo_processing_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Processing.KeyPress

        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Processing, Nothing, "Process_Head", "Process_Name", "", "(Process_Idno=0)")
        If Asc(e.KeyChar) = 13 Then

            With dgv_Details

                .Focus()
                .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)

            End With

        End If
    End Sub

    Private Sub cbo_processing_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Processing.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Dim f As New Process_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Processing.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If
    End Sub


    Private Sub cbo_Processing_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Processing.TextChanged
        Try
            If cbo_Processing.Visible Then
                With dgv_Details
                    If Val(cbo_Processing.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = dgvCol_Details.PROCESSING Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_Processing.Text)
                    End If
                End With
            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub cbo_itemgrey_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_itemgrey.GotFocus
        '(Cloth_Type = 'GREY')
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Cloth_Head", "Cloth_Name", "", "(Cloth_Idno = 0)")
    End Sub

    Private Sub cbo_itemgrey_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_itemgrey.KeyDown

        vcbo_KeyDwnVal = e.KeyValue

        '(Cloth_Type = 'GREY')
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_itemgrey, Nothing, Nothing, "Cloth_Head", "Cloth_Name", "", "(Cloth_Idno = 0)")

        With dgv_Details

            If (e.KeyValue = 38 And cbo_itemgrey.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
                If .CurrentCell.RowIndex = .Rows.Count - 1 Then

                    If cbo_ClothSales_OrderCode_forSelection.Visible = True Then
                        cbo_ClothSales_OrderCode_forSelection.Focus()
                    ElseIf txt_Folding.Visible Then
                        txt_Folding.Focus()
                    Else
                        cbo_Delivery_From.Focus()
                    End If
                Else
                    .Focus()
                    .CurrentCell = .Rows(.CurrentRow.Index).Cells(dgvCol_Details.RATE)
                    .CurrentCell.Selected = True
                End If
            End If

            If (e.KeyValue = 40 And cbo_itemgrey.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then

                If .CurrentCell.RowIndex = .Rows.Count - 1 And Trim(.Rows(.CurrentCell.RowIndex).Cells.Item(1).Value) = "" Then
                    cbo_TransportName.Focus()

                Else
                    .Focus()
                    If dgv_Details.Columns(dgvCol_Details.COLOUR_DELY).Visible Then
                        .CurrentCell = .Rows(.CurrentRow.Index).Cells(dgvCol_Details.COLOUR_DELY)
                    Else
                        .CurrentCell = .Rows(.CurrentRow.Index).Cells(dgvCol_Details.ITEM_FP)
                    End If
                    .CurrentCell.Selected = True

                End If
            End If



        End With
    End Sub

    Private Sub cbo_itemgrey_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_itemgrey.KeyPress

        '(Cloth_Type = 'GREY')
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_itemgrey, cbo_itemfp, "Cloth_Head", "Cloth_Name", "", "(Cloth_Idno = 0)")
        If Asc(e.KeyChar) = 13 Then

            e.Handled = True

            With dgv_Details
                If Trim(.Rows(.CurrentRow.Index).Cells(dgvCol_Details.ITEM_GREY).Value) = "" And .CurrentRow.Index = .Rows.Count - 1 Then
                    cbo_TransportName.Focus()
                Else
                    .Focus()
                    If dgv_Details.Columns(dgvCol_Details.COLOUR_DELY).Visible Then
                        .CurrentCell = .Rows(.CurrentRow.Index).Cells(dgvCol_Details.COLOUR_DELY)
                    Else
                        .CurrentCell = .Rows(.CurrentRow.Index).Cells(dgvCol_Details.ITEM_FP)
                    End If
                    .CurrentCell.Selected = True
                End If
            End With

        End If
    End Sub

    Private Sub cbo_itemgrey_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_itemgrey.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Dim f As New Cloth_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_itemgrey.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If
    End Sub


    Private Sub Cbo_itemgrey_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_itemgrey.TextChanged
        Try
            If cbo_itemgrey.Visible Then
                With dgv_Details
                    If Val(cbo_itemgrey.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = dgvCol_Details.ITEM_GREY Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_itemgrey.Text)
                    End If
                End With
            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub cbo_itemfp_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_itemfp.GotFocus

        Dim RET_TYPE As String = "CLOTH"

        If Len(Trim(Process_Outputs)) > 1 Then
            If Mid(Trim(Process_Outputs), 2, 1) = "1" Then
                RET_TYPE = "FP"
            End If
        End If

        If RET_TYPE = "FP" Then
            Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Processed_Item_Head", "Processed_Item_Name", "(Processed_Item_Type = 'FP')", "(Processed_Item_Idno = 0)")
        Else
            Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Cloth_Head", "Cloth_Name", "(Cloth_Type = 'PROCESSED FABRIC')", "(Cloth_Idno = 0)")
        End If

    End Sub

    Private Sub cbo_itemfp_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_itemfp.KeyDown

        vcbo_KeyDwnVal = e.KeyValue

        Dim RET_TYPE As String = "CLOTH"

        If Len(Trim(Process_Outputs)) > 1 Then
            If Mid(Trim(Process_Outputs), 2, 1) = "1" Then
                RET_TYPE = "FP"
            End If
        End If

        If RET_TYPE = "FP" Then
            Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_itemfp, cbo_Colour_Dely, cbo_Colour, "Processed_Item_Head", "Processed_Item_Name", "(Processed_Item_Type = 'FP')", "(Processed_Item_Idno = 0)")
        Else
            Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_itemfp, cbo_Colour_Dely, cbo_Colour, "Cloth_Head", "Cloth_Name", "(Cloth_Type = 'PROCESSED FABRIC')", "(Cloth_Idno = 0)")
        End If


        With dgv_Details

            If (e.KeyValue = 38 And cbo_itemfp.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
                .Focus()
                If dgv_Details.Columns(dgvCol_Details.COLOUR_DELY).Visible Then
                    .CurrentCell = .Rows(.CurrentRow.Index).Cells(dgvCol_Details.COLOUR_DELY)
                Else
                    .CurrentCell = .Rows(.CurrentRow.Index).Cells(dgvCol_Details.ITEM_GREY)
                End If
                .CurrentCell.Selected = True
            End If

            If (e.KeyValue = 40 And cbo_itemfp.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
                .Focus()
                .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 1)
            End If

        End With

    End Sub

    Private Sub cbo_itemfp_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_itemfp.KeyPress

        Dim RET_TYPE As String = "CLOTH"

        If Len(Trim(Process_Outputs)) > 1 Then
            If Mid(Trim(Process_Outputs), 2, 1) = "1" Then
                RET_TYPE = "FP"
            End If
        End If

        If RET_TYPE = "FP" Then
            Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_itemfp, cbo_Colour, "Processed_Item_Head", "Processed_Item_Name", "(Processed_Item_Type = 'FP')", "(Processed_Item_Idno = 0)")
        Else
            Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_itemfp, cbo_Colour, "Cloth_Head", "Cloth_Name", "(Cloth_Type = 'PROCESSED FABRIC')", "(Cloth_Idno = 0)")
        End If

        If Asc(e.KeyChar) = 13 Then

            With dgv_Details

                .Focus()
                .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)

            End With

        End If

    End Sub

    Private Sub cbo_itemfp_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_itemfp.KeyUp

        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then

            Dim RET_TYPE As String = "CLOTH"

            If Len(Trim(Process_Outputs)) > 1 Then
                If Mid(Trim(Process_Outputs), 2, 1) = "1" Then
                    RET_TYPE = "FP"
                End If
            End If

            If RET_TYPE = "FP" Then

                Dim f As New FinishedProduct_Creation_Simple

                Common_Procedures.Master_Return.Form_Name = Me.Name
                Common_Procedures.Master_Return.Control_Name = cbo_itemfp.Name
                Common_Procedures.Master_Return.Return_Value = ""
                Common_Procedures.Master_Return.Master_Type = ""

                f.MdiParent = MDIParent1
                f.Show()

            Else

                Dim f As New Cloth_Creation

                Common_Procedures.Master_Return.Form_Name = Me.Name
                Common_Procedures.Master_Return.Control_Name = cbo_itemfp.Name
                Common_Procedures.Master_Return.Return_Value = ""
                Common_Procedures.Master_Return.Master_Type = ""

                f.MdiParent = MDIParent1
                f.Show()

            End If

        End If

    End Sub

    Private Sub cbo_itemfp_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_itemfp.TextChanged

        Try

            If cbo_itemfp.Visible Then
                With dgv_Details
                    If Val(cbo_itemfp.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = dgvCol_Details.ITEM_FP Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_itemfp.Text)
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

    Private Sub dgv_Details_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgv_Details.GotFocus
        dgv_Details.Focus()
        'dgv_Details.CurrentCell.Selected = True
    End Sub

    Private Sub btn_Filter_Show_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Filter_Show.Click
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim n As Integer, i As Integer
        Dim Led_IdNo As Integer, proc_IdNo As Integer
        Dim Condt As String = ""


        Try

            Condt = ""
            Led_IdNo = 0
            proc_IdNo = 0

            If IsDate(dtp_Filter_Fromdate.Value) = True And IsDate(dtp_Filter_ToDate.Value) = True Then
                Condt = "a.ClothProcess_Delivery_Date between '" & Trim(Format(dtp_Filter_Fromdate.Value, "MM/dd/yyyy")) & "' and '" & Trim(Format(dtp_Filter_ToDate.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_Filter_Fromdate.Value) = True Then
                Condt = "a.ClothProcess_Delivery_Date = '" & Trim(Format(dtp_Filter_Fromdate.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_Filter_ToDate.Value) = True Then
                Condt = "a.ClothProcess_Delivery_Date = '" & Trim(Format(dtp_Filter_ToDate.Value, "MM/dd/yyyy")) & "' "
            End If

            If Trim(cbo_Filter_PartyName.Text) <> "" Then
                Led_IdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Filter_PartyName.Text)
            End If

            If Trim(cbo_Filter_ProcessName.Text) <> "" Then
                proc_IdNo = Common_Procedures.Process_NameToIdNo(con, cbo_Filter_ProcessName.Text)
            End If

            If Val(Led_IdNo) <> 0 Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & "a.Ledger_IdNo = " & Str(Val(Led_IdNo))
            End If


            If Val(proc_IdNo) <> 0 Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " c.Processing_Idno = " & Str(Val(proc_IdNo))
            End If

            If Trim(txt_filterpono.Text) <> "" Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & "a.Purchase_OrderNo = '" & Trim(txt_filterpono.Text) & "'"
            End If

            da = New SqlClient.SqlDataAdapter("select a.*, b.Ledger_Name,c.*,d.Cloth_Name,e.Process_Name from Textile_Processing_Delivery_Head a INNER JOIN Ledger_Head b on a.Ledger_IdNo = b.Ledger_IdNo INNER JOIN Textile_Processing_Delivery_Details c ON c.Cloth_Processing_Delivery_Code = a.ClothProcess_Delivery_Code INNER JOIN Cloth_Head d ON d.Cloth_Idno= c.Item_IdNo  LEFT OUTER JOIN Process_Head e ON c.Processing_Idno = e.Process_IdNo where a.company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.ClothProcess_Delivery_Code LIKE '%/" & Trim(EntFnYrCode) & "' and (ClothProcess_Delivery_Code not like '" & Trim(Pk_Condition2) & "%' and ClothProcess_Delivery_Code not like 'CPREC%' and ClothProcess_Delivery_Code not like 'WCLRC%' ) " & IIf(Trim(Condt) <> "", " and ", "") & Condt & " Order by a.for_orderby, a.ClothProcess_Delivery_RefNo", con)
            'da = New SqlClient.SqlDataAdapter("select a.*, b.Ledger_Name,c.*,d.Processed_Item_Name,e.Process_Name from Textile_Processing_Delivery_Head a INNER JOIN Ledger_Head b on a.Ledger_IdNo = b.Ledger_IdNo INNER JOIN Textile_Processing_Delivery_Details c ON c.Cloth_Processing_Delivery_Code = a.ClothProcess_Delivery_Code INNER JOIN Processed_Item_Head d ON d.Processed_Item_IdNo = c.Item_IdNo LEFT OUTER JOIN Process_Head e ON c.Processing_Idno = e.Process_IdNo where a.company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.ClothProcess_Delivery_Code LIKE '%/" & Trim(EntFnYrCode) & "' " & IIf(Trim(Condt) <> "", " and ", "") & Condt & " Order by a.for_orderby, a.ClothProcess_Delivery_RefNo", con)
            da.Fill(dt2)

            dgv_Filter_Details.Rows.Clear()

            If dt2.Rows.Count > 0 Then

                For i = 0 To dt2.Rows.Count - 1

                    n = dgv_Filter_Details.Rows.Add()


                    dgv_Filter_Details.Rows(n).Cells(dgvCol_Filterdetails.REF_NO).Value = dt2.Rows(i).Item("ClothProcess_Delivery_No").ToString
                    dgv_Filter_Details.Rows(n).Cells(dgvCol_Filterdetails.DC_NO).Value = dt2.Rows(i).Item("ClothProcess_Delivery_RefNo").ToString
                    dgv_Filter_Details.Rows(n).Cells(dgvCol_Filterdetails.FILTER_DATE).Value = Format(Convert.ToDateTime(dt2.Rows(i).Item("ClothProcess_Delivery_Date").ToString), "dd-MM-yyyy")
                    dgv_Filter_Details.Rows(n).Cells(dgvCol_Filterdetails.PARTY_NAME).Value = dt2.Rows(i).Item("Ledger_Name").ToString
                    dgv_Filter_Details.Rows(n).Cells(dgvCol_Filterdetails.ITEM_NAME_GREY).Value = dt2.Rows(i).Item("Cloth_Name").ToString
                    dgv_Filter_Details.Rows(n).Cells(dgvCol_Filterdetails.PROCESSING).Value = dt2.Rows(i).Item("Process_Name").ToString
                    dgv_Filter_Details.Rows(n).Cells(dgvCol_Filterdetails.PCS).Value = Val(dt2.Rows(i).Item("Delivery_Pcs").ToString)
                    dgv_Filter_Details.Rows(n).Cells(dgvCol_Filterdetails.QTY).Value = Val(dt2.Rows(i).Item("Delivery_Qty").ToString)
                    dgv_Filter_Details.Rows(n).Cells(dgvCol_Filterdetails.METERS).Value = Format(Val(dt2.Rows(i).Item("Total_Meters").ToString), "########0.00")
                    dgv_Filter_Details.Rows(n).Cells(dgvCol_Filterdetails.WEIGHT).Value = Format(Val(dt2.Rows(i).Item("Total_Weight").ToString), "########0.000")

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
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", " (Ledger_Type = '' and ( AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14) )", "(Ledger_idno = 0)")

    End Sub

    Private Sub cbo_Filter_PartyName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Filter_PartyName.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_PartyName, txt_filterpono, cbo_Filter_ProcessName, "Ledger_AlaisHead", "Ledger_DisplayName", " (Ledger_Type = '' and ( AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14) )", "(Ledger_idno = 0)")

    End Sub

    Private Sub cbo_Filter_PartyName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Filter_PartyName.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Filter_PartyName, cbo_Filter_ProcessName, "Ledger_AlaisHead", "Ledger_DisplayName", " (Ledger_Type = '' and ( AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14) ) ", "(Ledger_idno = 0)")
    End Sub

    Private Sub cbo_Filter_ProcessName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Filter_ProcessName.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Textile_Process_Head", "Process_name", "", "(Process_iDNO = 0)")

    End Sub

    Private Sub cbo_Filter_ProcessName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Filter_ProcessName.KeyDown

        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_ProcessName, cbo_Filter_PartyName, btn_Filter_Show, "Textile_Process_Head", "Process_name", "", "(Process_iDNO = 0)")

    End Sub

    Private Sub cbo_Filter_ProcessName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Filter_ProcessName.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Filter_ProcessName, btn_Filter_Show, "Textile_Process_Head", "Process_name", "", "(Process_iDNO = 0)")
    End Sub

    Private Sub Open_FilterEntry()
        Dim movno As String

        On Error Resume Next

        movno = Trim(dgv_Filter_Details.CurrentRow.Cells(dgvCol_Filterdetails.DC_NO).Value)

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
    Public Sub print_record() Implements Interface_MDIActions.print_record

        pnl_Print.Visible = True
        pnl_Back.Enabled = False
        If btn_Print_delivery.Enabled And btn_Print_delivery.Visible Then
            btn_Print_delivery.Focus()
        End If

    End Sub

    Private Sub print_Selection()
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim NewCode As String

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & EntFnYrCode

        Try

            da1 = New SqlClient.SqlDataAdapter("select * from Textile_Processing_Delivery_Head Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and ClothProcess_Delivery_Code = '" & Trim(NewCode) & "'", con)
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

        prn_InpOpts = ""
        prn_InpOpts = InputBox("Select    -   0. None" & Chr(13) & "                  1. Original" & Chr(13) & "                  2. Duplicate" & Chr(13) & "                  3. Triplicate" & Chr(13) & "                  4. All", "FOR INVOICE PRINTING...", "123")
        prn_InpOpts = Replace(Trim(prn_InpOpts), "4", "123")


        If Val(Common_Procedures.Print_OR_Preview_Status) = 1 Then
            Try
                PrintDialog1.PrinterSettings = PrintDocument1.PrinterSettings
                If PrintDialog1.ShowDialog() = Windows.Forms.DialogResult.OK Then
                    PrintDocument1.PrinterSettings = PrintDialog1.PrinterSettings
                    PrintDocument1.Print()
                End If

            Catch ex As Exception
                MessageBox.Show("The printing operation failed" & vbCrLf & ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

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
        Dim Dt1 As New DataTable
        Dim cmd As New SqlClient.SqlCommand
        Dim NewCode As String


        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & EntFnYrCode

        prn_HdDt.Clear()
        prn_DetDt.Clear()
        prn_DetIndx = 0
        prn_DetSNo = 0
        prn_PageNo = 0
        prn_Count = 0

        Try

            da1 = New SqlClient.SqlDataAdapter("select a.*, b.*, c.*, d.Ledger_Name as TransportName , f.Process_Name from Textile_Processing_Delivery_Head a INNER JOIN Company_Head b ON a.Company_IdNo = b.Company_IdNo INNER JOIN Ledger_Head c ON a.Ledger_IdNo = c.Ledger_IdNo LEFT OUTER JOIN Ledger_Head d ON d.Ledger_IdNo = a.Transport_Idno  LEFT OUTER JOIN Process_Head f ON f.Process_IdNo = a.Processing_Idno  where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.ClothProcess_Delivery_Code = '" & Trim(NewCode) & "'", con)
            prn_HdDt = New DataTable
            da1.Fill(prn_HdDt)

            If prn_HdDt.Rows.Count > 0 Then

                If Len(Trim(Process_Outputs)) >= 2 And Microsoft.VisualBasic.Right(Trim(Process_Outputs), 1) = "1" Then
                    da2 = New SqlClient.SqlDataAdapter("select a.*, b.CLOTH_Name as Processed_Grey_Name, c.Colour_Name ,d.Lot_No ,PR_CL.PROCESSED_ITEM_NAME AS PROCESSED_CLOTH_NAME , D_COL.COLOUR_NAME AS DEL_COLOUR_NAME , Ig.Item_GST_Percentage  FROM Textile_Processing_Delivery_Details a LEFT OUTER JOIN CLOTH_Head b on a.Item_IdNo = b.CLOTH_IdNo LEFT OUTER JOIN Colour_Head c on a.Colour_IdNo = c.Colour_IdNo LEFT OUTER JOIN Lot_Head d ON d.Lot_IdNo = a.Lot_IdNo LEFT OUTER JOIN PROCESSED_ITEM_HEAD PR_CL ON A.ITEM_TO_IDNO = PR_CL.PROCESSED_ITEM_IDNO LEFT OUTER JOIN COLOUR_HEAD D_COL ON A.Del_Colour_IdNo = D_COL.Colour_IdNo Left Outer Join ItemGroup_Head Ig On b.ItemGroup_IdNo = Ig.ItemGroup_IdNo where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Cloth_Processing_Delivery_Code = '" & Trim(NewCode) & "' Order by a.sl_no", con)
                    'da2 = New SqlClient.SqlDataAdapter("select a.*, b.CLOTH_Name as Processed_Grey_Name, c.Colour_Name ,d.Lot_No ,PR_CL.PROCESSED_ITEM_NAME AS PROCESSED_CLOTH_NAME , D_COL.COLOUR_NAME AS DEL_COLOUR_NAME FROM Textile_Processing_Delivery_Details a LEFT OUTER JOIN CLOTH_Head b on a.Item_IdNo = b.CLOTH_IdNo LEFT OUTER JOIN Colour_Head c on a.Colour_IdNo = c.Colour_IdNo LEFT OUTER JOIN Lot_Head d ON d.Lot_IdNo = a.Lot_IdNo LEFT OUTER JOIN PROCESSED_ITEM_HEAD PR_CL ON A.ITEM_TO_IDNO = PR_CL.PROCESSED_ITEM_IDNO LEFT OUTER JOIN COLOUR_HEAD D_COL ON A.Del_Colour_IdNo = D_COL.Colour_IdNo where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Cloth_Processing_Delivery_Code = '" & Trim(NewCode) & "' Order by a.sl_no", con)
                Else
                    da2 = New SqlClient.SqlDataAdapter("select a.*, b.CLOTH_Name as Processed_Grey_Name, c.Colour_Name ,d.Lot_No ,PR_CL.CLOTH_NAME AS PROCESSED_CLOTH_NAME , D_COL.COLOUR_NAME AS DEL_COLOUR_NAME , Ig.Item_GST_Percentage FROM Textile_Processing_Delivery_Details a LEFT OUTER JOIN CLOTH_Head b on a.Item_IdNo = b.CLOTH_IdNo LEFT OUTER JOIN Colour_Head c on a.Colour_IdNo = c.Colour_IdNo LEFT OUTER JOIN Lot_Head d ON d.Lot_IdNo = a.Lot_IdNo LEFT OUTER JOIN CLOTH_HEAD PR_CL ON A.ITEM_TO_IDNO = PR_CL.CLOTH_IDNO LEFT OUTER JOIN COLOUR_HEAD D_COL ON A.Del_Colour_IdNo = D_COL.Colour_IdNo Left Outer Join ItemGroup_Head Ig On b.ItemGroup_IdNo = Ig.ItemGroup_IdNo where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Cloth_Processing_Delivery_Code = '" & Trim(NewCode) & "' Order by a.sl_no", con)
                    'da2 = New SqlClient.SqlDataAdapter("select a.*, b.CLOTH_Name as Processed_Grey_Name, c.Colour_Name ,d.Lot_No ,PR_CL.CLOTH_NAME AS PROCESSED_CLOTH_NAME , D_COL.COLOUR_NAME AS DEL_COLOUR_NAME FROM Textile_Processing_Delivery_Details a LEFT OUTER JOIN CLOTH_Head b on a.Item_IdNo = b.CLOTH_IdNo LEFT OUTER JOIN Colour_Head c on a.Colour_IdNo = c.Colour_IdNo LEFT OUTER JOIN Lot_Head d ON d.Lot_IdNo = a.Lot_IdNo LEFT OUTER JOIN CLOTH_HEAD PR_CL ON A.ITEM_TO_IDNO = PR_CL.CLOTH_IDNO LEFT OUTER JOIN COLOUR_HEAD D_COL ON A.Del_Colour_IdNo = D_COL.Colour_IdNo where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Cloth_Processing_Delivery_Code = '" & Trim(NewCode) & "' Order by a.sl_no", con)
                End If

                prn_DetDt = New DataTable
                da2.Fill(prn_DetDt)

            Else

                MessageBox.Show("This is New Entry", "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

            End If

            da1.Dispose()

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub PrintDocument1_PrintPage(ByVal sender As System.Object, ByVal e As System.Drawing.Printing.PrintPageEventArgs) Handles PrintDocument1.PrintPage

        If prn_HdDt.Rows.Count <= 0 Then Exit Sub

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1558" Then '-------Sotexpa Qualidis
            Printing_Format1558(e)
        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1061" Then '-------Prakash Cottex
            Printing_Format1061(e)
        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1464" Then  '-- MOF
            Printing_Format1464(e)
        Else
            Printing_Format1(e)
        End If

    End Sub

    Public Sub Printing_Bale()

        Dim prtFrm As Single = 0
        Dim prtTo As Single = 0
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim Condt As String = ""
        Dim PpSzSTS As Boolean = False
        Dim ps As Printing.PaperSize
        Dim NewCode As String

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Try

            da1 = New SqlClient.SqlDataAdapter("select a.*, b.Cloth_Name from Packing_Slip_Head a INNER JOIN Cloth_Head b ON a.Cloth_IdNo = b.Cloth_IdNo Where a.Delivery_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'", con)
            dt1 = New DataTable
            da1.Fill(dt1)

            If dt1.Rows.Count <= 0 Then

                MessageBox.Show("No Entry Found", "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                Exit Sub

            End If

            dt1.Dispose()
            da1.Dispose()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try


        prn_InpOpts = ""
        prn_InpOpts = InputBox("Select    -   0. None" & Chr(13) & "                  1. Original" & Chr(13) & "                  2. Duplicate" & Chr(13) & "                  3. All", "FOR INVOICE PRINTING...", "12")
        prn_InpOpts = Replace(Trim(prn_InpOpts), "3", "12")


        If Val(Common_Procedures.Print_OR_Preview_Status) = 1 Then
            Try

                'Dim pkCustomSize1 As New System.Drawing.Printing.PaperSize("PAPER 8.25X12", 850, 1200)
                'PrintDocument2.PrinterSettings.DefaultPageSettings.PaperSize = pkCustomSize1
                'PrintDocument2.DefaultPageSettings.PaperSize = pkCustomSize1

                For I = 0 To PrintDocument2.PrinterSettings.PaperSizes.Count - 1
                    If PrintDocument2.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
                        ps = PrintDocument2.PrinterSettings.PaperSizes(I)
                        PrintDocument2.DefaultPageSettings.PaperSize = ps
                        Exit For
                    End If
                Next

                PrintDialog1.PrinterSettings = PrintDocument2.PrinterSettings
                If PrintDialog1.ShowDialog() = Windows.Forms.DialogResult.OK Then
                    PrintDocument2.PrinterSettings = PrintDialog1.PrinterSettings
                    PrintDocument2.Print()
                End If

            Catch ex As Exception
                MessageBox.Show("The printing operation failed" & vbCrLf & ex.Message, "DOES NOT SHOW PRINT PREVIEW...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

            End Try

        Else

            Try

                Dim ppd As New PrintPreviewDialog

                For I = 0 To PrintDocument2.PrinterSettings.PaperSizes.Count - 1
                    If PrintDocument2.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
                        ps = PrintDocument2.PrinterSettings.PaperSizes(I)
                        PrintDocument2.DefaultPageSettings.PaperSize = ps
                        Exit For
                    End If
                Next

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

        pnl_Back.Enabled = True
        pnl_Print.Visible = False

    End Sub

    Private Sub PrintDocument2_BeginPrint(ByVal sender As Object, ByVal e As System.Drawing.Printing.PrintEventArgs) Handles PrintDocument2.BeginPrint
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim NewCode As String = ""
        Dim sno As Integer

        prn_HdDt.Clear()
        prn_DetDt.Clear()

        prn_PageNo = 0
        prn_HdIndx = 0
        prn_DetIndx = 0
        prn_HdMxIndx = 0
        prn_DetMxIndx = 0
        prn_Count = 0
        prn_123Count = 0
        Erase prn_DetAr
        Erase prn_HdAr

        prn_Count = 0


        prn_HdAr = New String(1000, 10) {}
        prn_DetAr = New String(1000, 50, 10) {}

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Try

            Total_mtrs = 0
            sno = 0
            Total_wEIGHT = 0
            Total_PCS = 0
            Total_Mtrs_20To40 = 0
            Total_Mtrs_40To79 = 0
            Total_Mtrs_Abv80 = 0

            da1 = New SqlClient.SqlDataAdapter("select a.*, b.*, tZ.*, e.* from Textile_Processing_Delivery_Head a INNER JOIN Textile_Processing_Delivery_Details b ON a.ClothProcess_Delivery_Code = b.Cloth_Processing_Delivery_code INNER JOIN Company_head tZ ON a.company_idno = tZ.Company_Idno INNER JOIN LEDGER_Head E ON a.Ledger_IdNo = e.Ledger_IdNo  Where a.ClothProcess_Delivery_Code = '" & Trim(NewCode) & "' Order by a.ClothProcess_Delivery_date, a.for_OrderBy, a.ClothProcess_Delivery_RefNo, a.ClothProcess_Delivery_Code, b.Sl_No", con)
            prn_HdDt = New DataTable
            da1.Fill(prn_HdDt)

            ChkPrintRow = 0
            prn_DetailsCount = 0
            If prn_HdDt.Rows.Count > 0 Then

                prn_DetailsCount = Val(prn_HdDt.Rows.Count)

            Else
                MessageBox.Show("This is New Entry", "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

            End If

            da1.Dispose()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub PrintDocument2_PrintPage(ByVal sender As System.Object, ByVal e As System.Drawing.Printing.PrintPageEventArgs) Handles PrintDocument2.PrintPage
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim sno As Integer
        ' Dim ChkPrintRow As Integer = 0

        If prn_HdDt.Rows.Count <= 0 Then Exit Sub

        Erase prn_HdAr
        Erase prn_DetAr

        prn_HdAr = New String(1000, 10) {}

        prn_DetAr = New String(1000, 50, 10) {}

        If prn_Count <= 0 Then
            prn_HdIndx = 0
        End If

        If Val(ChkPrintRow) < Val(prn_DetailsCount) Then
            prn_DetailsIndex = Val(ChkPrintRow)
        Else
            Exit Sub
        End If

        'If Trim(prn_InpOpts) <> "" Then
        '    If prn_Count < Len(Trim(prn_InpOpts)) Then
        '        If Val(prn_InpOpts) <> "0" Then
        '            '  prn_DetailsIndex = 0
        '        End If
        '    End If
        'End If

        ' prn_DetailsIndex = 0


        Total_mtrs = 0
        sno = 0
        Total_wEIGHT = 0
        Total_PCS = 0
        Total_Mtrs_20To40 = 0
        Total_Mtrs_40To79 = 0
        Total_Mtrs_Abv80 = 0

        prn_HdMxIndx = 0

        da1 = New SqlClient.SqlDataAdapter("select a.*, c.Cloth_Name from Packing_Slip_Head a INNER JOIN Cloth_Head c ON a.Cloth_IdNo = c.Cloth_IdNo Where a.Delivery_Code = '" & Trim(Pk_Condition) & Trim(prn_HdDt.Rows(prn_DetailsIndex).Item("Cloth_Processing_Delivery_code").ToString) & "' and  a.Delivery_DetailsSlNo = " & Str(Val(prn_HdDt.Rows(prn_DetailsIndex).Item("ClothProcessing_Delivery_PackingSlno").ToString)) & " Order by a.Packing_Slip_Date, a.for_OrderBy, a.Packing_Slip_No, a.Packing_Slip_Code", con)
        Dt1 = New DataTable
        da1.Fill(Dt1)

        If Dt1.Rows.Count > 0 Then
            For i = 0 To Dt1.Rows.Count - 1

                prn_HdMxIndx = prn_HdMxIndx + 1
                sno = sno + 1

                prn_HdAr(prn_HdMxIndx, 1) = Trim(Dt1.Rows(i).Item("Packing_Slip_No").ToString)
                prn_HdAr(prn_HdMxIndx, 2) = Val(Dt1.Rows(i).Item("Total_Pcs").ToString)
                prn_HdAr(prn_HdMxIndx, 3) = Format(Val(Dt1.Rows(i).Item("Total_Meters").ToString), "#########0.00")
                prn_HdAr(prn_HdMxIndx, 4) = Format(Val(Dt1.Rows(i).Item("Total_Weight").ToString), "#########0.000")
                prn_HdAr(prn_HdMxIndx, 5) = Val(sno)
                prn_HdAr(prn_HdMxIndx, 6) = Trim(Dt1.Rows(i).Item("Cloth_Name").ToString)

                prn_DetMxIndx = 0

                Total_PCS = Total_PCS + Val(Dt1.Rows(i).Item("Total_Pcs").ToString)

                da2 = New SqlClient.SqlDataAdapter("select a.* from Packing_Slip_Details a where a.Packing_Slip_Code = '" & Trim(Dt1.Rows(i).Item("Packing_Slip_Code").ToString) & "' order by a.Sl_No", con)
                prn_DetDt = New DataTable
                da2.Fill(prn_DetDt)
                If prn_DetDt.Rows.Count > 0 Then
                    For j = 0 To prn_DetDt.Rows.Count - 1
                        If Val(prn_DetDt.Rows(j).Item("Meters").ToString) <> 0 Then
                            prn_DetMxIndx = prn_DetMxIndx + 1

                            prn_DetAr(prn_HdMxIndx, prn_DetMxIndx, 1) = Trim(prn_DetDt.Rows(j).Item("Lot_No").ToString)
                            prn_DetAr(prn_HdMxIndx, prn_DetMxIndx, 2) = Trim(prn_DetDt.Rows(j).Item("Pcs_No").ToString)
                            prn_DetAr(prn_HdMxIndx, prn_DetMxIndx, 3) = Format(Val(prn_DetDt.Rows(j).Item("Meters").ToString), "#########0.00")

                            Total_mtrs = Total_mtrs + Format(Val(prn_DetDt.Rows(j).Item("Meters").ToString), "#########0.00")
                            Total_wEIGHT = Total_wEIGHT + Format(Val(prn_DetDt.Rows(j).Item("Weight").ToString), "#########0.000")

                            If Val(prn_DetDt.Rows(j).Item("Meters").ToString) >= 80 Then
                                Total_Mtrs_Abv80 = Total_Mtrs_Abv80 + Format(Val(prn_DetDt.Rows(j).Item("Meters").ToString), "#########0.00")

                            ElseIf Val(prn_DetDt.Rows(j).Item("Meters").ToString) > 40 And Val(prn_DetDt.Rows(j).Item("Meters").ToString) < 80 Then
                                Total_Mtrs_40To79 = Total_Mtrs_40To79 + Format(Val(prn_DetDt.Rows(j).Item("Meters").ToString), "#########0.00")

                            ElseIf Val(prn_DetDt.Rows(j).Item("Meters").ToString) > 20 And Val(prn_DetDt.Rows(j).Item("Meters").ToString) <= 40 Then
                                Total_Mtrs_20To40 = Total_Mtrs_20To40 + Format(Val(prn_DetDt.Rows(j).Item("Meters").ToString), "#########0.00")

                            End If

                        End If
                    Next j
                End If

            Next i

        End If

        Printing_PackingSlip_Format2(PrintDocument2, e, prn_HdDt, prn_HdMxIndx, prn_DetMxIndx, prn_HdAr, prn_DetAr, prn_PageNo, prn_Count, prn_HdIndx, prn_DetIndx)

    End Sub


    'Private Sub PrintDocument2_BeginPrint(ByVal sender As Object, ByVal e As System.Drawing.Printing.PrintEventArgs) Handles PrintDocument2.BeginPrint
    '    Dim da1 As New SqlClient.SqlDataAdapter
    '    Dim da2 As New SqlClient.SqlDataAdapter
    '    Dim NewCode As String = ""
    '    Dim sno As Integer

    '    prn_HdDt.Clear()
    '    prn_DetDt.Clear()

    '    prn_PageNo = 0
    '    prn_HdIndx = 0
    '    prn_DetIndx = 0
    '    prn_HdMxIndx = 0
    '    prn_DetMxIndx = 0
    '    prn_Count = 0
    '    Erase prn_DetAr
    '    Erase prn_HdAr

    '    prn_HdAr = New String(100, 10) {}

    '    prn_DetAr = New String(100, 50, 10) {}

    '    NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

    '    Try
    '        Total_mtrs = 0
    '        sno = 0
    '        Total_wEIGHT = 0
    '        Total_PCS = 0
    '        Total_Mtrs_20To40 = 0
    '        Total_Mtrs_40To79 = 0
    '        Total_Mtrs_Abv80 = 0

    '        da1 = New SqlClient.SqlDataAdapter("select a.*, tZ.*, c.Cloth_Name , d.* , E.* from Packing_Slip_Head a  INNER JOIN Textile_Processing_Delivery_Head d ON d.ClothProcess_Delivery_Code =  '" & Trim(NewCode) & "' INNER JOIN Company_head tZ ON a.company_idno = tZ.Company_Idno INNER JOIN Cloth_Head c ON a.Cloth_IdNo = c.Cloth_IdNo LEFT OUTER JOIN LEDGER_Head E ON D.Ledger_IdNo = E.Ledger_IdNo  Where a.Delivery_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' Order by a.Packing_Slip_Date, a.for_OrderBy, a.Packing_Slip_No, a.Packing_Slip_Code", con)
    '        prn_HdDt = New DataTable
    '        da1.Fill(prn_HdDt)

    '        If prn_HdDt.Rows.Count > 0 Then
    '            For i = 0 To prn_HdDt.Rows.Count - 1

    '                prn_HdMxIndx = prn_HdMxIndx + 1
    '                sno = sno + 1

    '                prn_HdAr(prn_HdMxIndx, 1) = Trim(prn_HdDt.Rows(i).Item("Packing_Slip_No").ToString)
    '                '  prn_HdAr(prn_HdMxIndx, 2) = Trim(prn_HdDt.Rows(i).Item("Cloth_Name").ToString)
    '                prn_HdAr(prn_HdMxIndx, 2) = Val(prn_HdDt.Rows(i).Item("Total_Pcs").ToString)
    '                prn_HdAr(prn_HdMxIndx, 3) = Format(Val(prn_HdDt.Rows(i).Item("Total_Meters").ToString), "#########0.00")
    '                prn_HdAr(prn_HdMxIndx, 4) = Format(Val(prn_HdDt.Rows(i).Item("Total_Weight").ToString), "#########0.000")
    '                prn_HdAr(prn_HdMxIndx, 5) = Val(sno)
    '                prn_HdAr(prn_HdMxIndx, 6) = Trim(prn_HdDt.Rows(i).Item("Cloth_Name").ToString)

    '                prn_DetMxIndx = 0

    '                Total_PCS = Total_PCS + Val(prn_HdDt.Rows(i).Item("Total_Pcs").ToString)

    '                da2 = New SqlClient.SqlDataAdapter("select a.* from Packing_Slip_Details a where a.Packing_Slip_Code = '" & Trim(prn_HdDt.Rows(i).Item("Packing_Slip_Code").ToString) & "' order by a.Sl_No", con)
    '                prn_DetDt = New DataTable
    '                da2.Fill(prn_DetDt)
    '                If prn_DetDt.Rows.Count > 0 Then
    '                    For j = 0 To prn_DetDt.Rows.Count - 1
    '                        If Val(prn_DetDt.Rows(j).Item("Meters").ToString) <> 0 Then
    '                            prn_DetMxIndx = prn_DetMxIndx + 1

    '                            prn_DetAr(prn_HdMxIndx, prn_DetMxIndx, 1) = Trim(prn_DetDt.Rows(j).Item("Lot_No").ToString)
    '                            prn_DetAr(prn_HdMxIndx, prn_DetMxIndx, 2) = Trim(prn_DetDt.Rows(j).Item("Pcs_No").ToString)
    '                            prn_DetAr(prn_HdMxIndx, prn_DetMxIndx, 3) = Format(Val(prn_DetDt.Rows(j).Item("Meters").ToString), "#########0.00")

    '                            Total_mtrs = Total_mtrs + Format(Val(prn_DetDt.Rows(j).Item("Meters").ToString), "#########0.00")
    '                            Total_wEIGHT = Total_wEIGHT + Format(Val(prn_DetDt.Rows(j).Item("Weight").ToString), "#########0.000")

    '                            If Val(prn_DetDt.Rows(j).Item("Meters").ToString) >= 80 Then
    '                                Total_Mtrs_Abv80 = Total_Mtrs_Abv80 + Format(Val(prn_DetDt.Rows(j).Item("Meters").ToString), "#########0.00")

    '                            ElseIf Val(prn_DetDt.Rows(j).Item("Meters").ToString) > 40 And Val(prn_DetDt.Rows(j).Item("Meters").ToString) < 80 Then
    '                                Total_Mtrs_40To79 = Total_Mtrs_40To79 + Format(Val(prn_DetDt.Rows(j).Item("Meters").ToString), "#########0.00")

    '                            ElseIf Val(prn_DetDt.Rows(j).Item("Meters").ToString) > 20 And Val(prn_DetDt.Rows(j).Item("Meters").ToString) <= 40 Then
    '                                Total_Mtrs_20To40 = Total_Mtrs_20To40 + Format(Val(prn_DetDt.Rows(j).Item("Meters").ToString), "#########0.00")

    '                            End If

    '                        End If
    '                    Next j
    '                End If

    '            Next i

    '        Else
    '            MessageBox.Show("This is New Entry", "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

    '        End If

    '        da1.Dispose()

    '    Catch ex As Exception
    '        MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

    '    End Try

    'End Sub

    'Private Sub PrintDocument2_PrintPage(ByVal sender As System.Object, ByVal e As System.Drawing.Printing.PrintPageEventArgs) Handles PrintDocument2.PrintPage
    '    If prn_HdDt.Rows.Count <= 0 Then Exit Sub
    '    '  If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1018" Then '---- M.K Textiles (Palladam)
    '    Printing_PackingSlip_Format2(PrintDocument2, e, prn_HdDt, prn_HdMxIndx, prn_DetMxIndx, prn_HdAr, prn_DetAr, prn_PageNo, prn_Count, prn_HdIndx, prn_DetIndx)
    '    'Else
    '    '    Common_Procedures.Printing_PackingSlip_Format1(PrintDocument2, e, prn_HdDt, prn_HdMxIndx, prn_DetMxIndx, prn_HdAr, prn_DetAr, prn_PageNo, prn_Count, prn_HdIndx, prn_DetIndx)
    '    'End If

    'End Sub

    Private Sub Printing_Format1(ByRef e As System.Drawing.Printing.PrintPageEventArgs)

        Dim cmd As New SqlClient.SqlCommand
        Dim Da1 As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim EntryCode As String
        Dim I As Integer, NoofDets As Integer, NoofItems_PerPage As Integer
        Dim pFont As Font
        Dim p1Font As Font
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
        Dim clrName As String = ""
        Dim Clrln As Integer = 0
        'PrintDocument pd = new PrintDocument();
        'pd.DefaultPageSettings.PaperSize = new System.Drawing.Printing.PaperSize("PaperA4", 840, 1180);
        'pd.Print();

        'For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
        '    ps = PrintDocument1.PrinterSettings.PaperSizes(I)
        '    Debug.Print(ps.PaperName)
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

        '    If PpSzSTS = False Then
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
            .Left = 20
            .Right = 55
            .Top = 35
            .Bottom = 35
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

        NoofItems_PerPage = 4 ' 6

        Erase LnAr
        Erase ClArr

        LnAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        ClArr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}

        ClArr(1) = Val(30) : ClArr(2) = 190 : ClArr(3) = 80 : ClArr(4) = 190 : ClArr(5) = 80 : ClArr(6) = 40 : ClArr(7) = 40
        ClArr(8) = PageWidth - (LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7))

        TxtHgt = 16 '18 ' 19 ' e.Graphics.MeasureString("A", pFont).Height  ' 20

        EntryCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(EntFnYrCode)

        Try

            If prn_HdDt.Rows.Count > 0 Then

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

                        ItmNm1 = Trim(prn_DetDt.Rows(prn_DetIndx).Item("Processed_Grey_Name").ToString)
                        ItmNm2 = ""
                        If Len(ItmNm1) > 25 Then
                            For I = 25 To 1 Step -1
                                If Mid$(Trim(ItmNm1), I, 1) = " " Or Mid$(Trim(ItmNm1), I, 1) = "," Or Mid$(Trim(ItmNm1), I, 1) = "." Or Mid$(Trim(ItmNm1), I, 1) = "-" Or Mid$(Trim(ItmNm1), I, 1) = "/" Or Mid$(Trim(ItmNm1), I, 1) = "_" Or Mid$(Trim(ItmNm1), I, 1) = "(" Or Mid$(Trim(ItmNm1), I, 1) = ")" Or Mid$(Trim(ItmNm1), I, 1) = "\" Or Mid$(Trim(ItmNm1), I, 1) = "[" Or Mid$(Trim(ItmNm1), I, 1) = "]" Or Mid$(Trim(ItmNm1), I, 1) = "{" Or Mid$(Trim(ItmNm1), I, 1) = "}" Then Exit For
                            Next I
                            If I = 0 Then I = 25
                            ItmNm2 = Microsoft.VisualBasic.Right(Trim(ItmNm1), Len(ItmNm1) - I)
                            ItmNm1 = Microsoft.VisualBasic.Left(Trim(ItmNm1), I - 1)
                        End If

                        Dim ClrNm1 As String, ClrNm2 As String

                        ClrNm1 = Trim(prn_DetDt.Rows(prn_DetIndx).Item("DEL_COLOUR_NAME").ToString)
                        ClrNm2 = ""
                        If Len(ClrNm1) > 12 Then
                            For I = 12 To 1 Step -1
                                If Mid$(Trim(ClrNm1), I, 1) = "@" Or Mid$(Trim(ClrNm1), I, 1) = " " Or Mid$(Trim(ClrNm1), I, 1) = "," Or Mid$(Trim(ClrNm1), I, 1) = "." Or Mid$(Trim(ClrNm1), I, 1) = "-" Or Mid$(Trim(ClrNm1), I, 1) = "/" Or Mid$(Trim(ClrNm1), I, 1) = "_" Or Mid$(Trim(ClrNm1), I, 1) = "(" Or Mid$(Trim(ClrNm1), I, 1) = ")" Or Mid$(Trim(ClrNm1), I, 1) = "\" Or Mid$(Trim(ClrNm1), I, 1) = "[" Or Mid$(Trim(ClrNm1), I, 1) = "]" Or Mid$(Trim(ClrNm1), I, 1) = "{" Or Mid$(Trim(ClrNm1), I, 1) = "}" Or Mid$(Trim(ClrNm1), I, 1) = "@" Then Exit For
                            Next I
                            If I = 0 Then I = 12
                            ClrNm2 = Microsoft.VisualBasic.Right(Trim(ClrNm1), Len(ClrNm1) - I)
                            ClrNm1 = Microsoft.VisualBasic.Left(Trim(ClrNm1), I)
                        End If

                        '----------------------------

                        Dim ItmNmP1 As String, ItmNmP2 As String
                        Dim ClrNmP1 As String, ClrNmP2 As String

                        ItmNmP1 = Trim(prn_DetDt.Rows(prn_DetIndx).Item("Processed_Cloth_Name").ToString)
                        ItmNmP2 = ""
                        If Len(ItmNmP1) > 25 Then
                            For I = 25 To 1 Step -1
                                If Mid$(Trim(ItmNmP1), I, 1) = " " Or Mid$(Trim(ItmNmP1), I, 1) = "," Or Mid$(Trim(ItmNmP1), I, 1) = "." Or Mid$(Trim(ItmNmP1), I, 1) = "-" Or Mid$(Trim(ItmNmP1), I, 1) = "/" Or Mid$(Trim(ItmNmP1), I, 1) = "_" Or Mid$(Trim(ItmNmP1), I, 1) = "(" Or Mid$(Trim(ItmNmP1), I, 1) = ")" Or Mid$(Trim(ItmNmP1), I, 1) = "\" Or Mid$(Trim(ItmNmP1), I, 1) = "[" Or Mid$(Trim(ItmNmP1), I, 1) = "]" Or Mid$(Trim(ItmNmP1), I, 1) = "{" Or Mid$(Trim(ItmNmP1), I, 1) = "}" Then Exit For
                            Next I
                            If I = 0 Then I = 25
                            ItmNmP2 = Microsoft.VisualBasic.Right(Trim(ItmNmP1), Len(ItmNmP1) - I)
                            ItmNmP1 = Microsoft.VisualBasic.Left(Trim(ItmNmP1), I - 1)
                        End If

                        'Dim ClrNmP1 As String, ClrNmP2 As String

                        ClrNmP1 = Trim(prn_DetDt.Rows(prn_DetIndx).Item("DEL_COLOUR_NAME").ToString)
                        ClrNmP2 = ""
                        If Len(ClrNmP1) > 12 Then
                            For I = 12 To 1 Step -1
                                If Mid$(Trim(ClrNmP1), I, 1) = "@" Or Mid$(Trim(ClrNmP1), I, 1) = " " Or Mid$(Trim(ClrNmP1), I, 1) = "," Or Mid$(Trim(ClrNmP1), I, 1) = "." Or Mid$(Trim(ClrNmP1), I, 1) = "-" Or Mid$(Trim(ClrNmP1), I, 1) = "/" Or Mid$(Trim(ClrNmP1), I, 1) = "_" Or Mid$(Trim(ClrNmP1), I, 1) = "(" Or Mid$(Trim(ClrNmP1), I, 1) = ")" Or Mid$(Trim(ClrNmP1), I, 1) = "\" Or Mid$(Trim(ClrNmP1), I, 1) = "[" Or Mid$(Trim(ClrNmP1), I, 1) = "]" Or Mid$(Trim(ClrNmP1), I, 1) = "{" Or Mid$(Trim(ClrNmP1), I, 1) = "}" Or Mid$(Trim(ClrNmP1), I, 1) = "@" Then Exit For
                            Next I
                            If I = 0 Then I = 12
                            ClrNmP2 = Microsoft.VisualBasic.Right(Trim(ClrNmP1), Len(ClrNmP1) - I)
                            ClrNmP1 = Microsoft.VisualBasic.Left(Trim(ClrNmP1), I)
                        End If


                        CurY = CurY + TxtHgt
                        SNo = SNo + 1
                        Common_Procedures.Print_To_PrintDocument(e, Trim(Val(SNo)), LMargin + 10, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm1), LMargin + ClArr(1) + 10, CurY, 0, 0, pFont)

                        p1Font = New Font("Calibri", 8, FontStyle.Regular)
                        Common_Procedures.Print_To_PrintDocument(e, ClrNm1, LMargin + ClArr(1) + ClArr(2) + 10, CurY, 0, 0, p1Font)

                        Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNmP1), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + 10, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, ClrNmP1, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + 10, CurY, 0, 0, p1Font)

                        Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Bales").ToString, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) - 10, CurY, 1, 0, pFont)

                        If Val(prn_DetDt.Rows(prn_DetIndx).Item("Delivery_Pcs").ToString) <> 0 Then
                            Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(prn_DetIndx).Item("Delivery_Pcs").ToString), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) - 10, CurY, 1, 0, pFont)
                        End If
                        If Val(prn_DetDt.Rows(prn_DetIndx).Item("Delivery_Meters").ToString) <> 0 Then
                            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Delivery_Meters").ToString), "#########0.00"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) - 10, CurY, 1, 0, pFont)
                        End If

                        NoofDets = NoofDets + 1

                        If Trim(ItmNm2) <> "" Or Trim(ClrNm2) <> "" Or Trim(ItmNmP2) <> "" Or Trim(ClrNmP2) <> "" Then
                            CurY = CurY + TxtHgt - 5
                            Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm2), LMargin + ClArr(1) + 10, CurY, 0, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, ClrNm2, LMargin + ClArr(1) + ClArr(2) + 10, CurY, 0, 0, p1Font)
                            Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNmP2), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + 10, CurY, 0, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, ClrNmP2, LMargin + ClArr(1) + ClArr(2) + 10, CurY, 0, 0, p1Font)
                            NoofDets = NoofDets + 1
                        End If

                        prn_DetIndx = prn_DetIndx + 1

                    Loop

                End If


                Printing_Format1_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, NoofDets, True)

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

            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

        e.HasMorePages = False

    End Sub

    Private Sub Printing_Format1_PageHeader(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByRef PageNo As Integer, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single)
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt2 As New DataTable
        Dim p1Font As Font
        Dim strHeight As Single
        Dim C1 As Single, W1 As Single, S1 As Single
        Dim Cmp_Name As String, Cmp_Add1 As String, Cmp_Add2 As String
        Dim Cmp_PhNo As String, Cmp_TinNo As String, Cmp_CstNo As String
        Dim Cmp_GSTIN_Cap As String, Cmp_GSTIN_No As String
        Dim S As String

        PageNo = PageNo + 1

        CurY = TMargin

        da2 = New SqlClient.SqlDataAdapter("select a.*, b.Processed_Item_Name as Processed_Grey_Name, c.Colour_Name ,d.Lot_No ,e.Process_Name  from Textile_Processing_Delivery_Details a INNER JOIN Processed_Item_Head b on a.Item_IdNo = b.Processed_Item_IdNo LEFT OUTER JOIN Colour_Head c on a.Colour_IdNo = c.Colour_IdNo LEFT OUTER JOIN Lot_Head d ON d.Lot_IdNo = a.Lot_IdNo LEFT OUTER JOIN Process_Head e ON e.Process_IdNo = a.Processing_Idno where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Cloth_Processing_Delivery_Code = '" & Trim(EntryCode) & "' Order by a.sl_no", con)
        da2.Fill(dt2)

        If dt2.Rows.Count > NoofItems_PerPage Then
            Common_Procedures.Print_To_PrintDocument(e, "Page : " & Trim(Val(PageNo)), PageWidth - 10, CurY - TxtHgt, 1, 0, pFont)
        End If
        dt2.Clear()

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
                    If Trim(prn_InpOpts) <> "0" And Val(prn_InpOpts) = "0" And Len(Trim(prn_InpOpts)) > 4 Then
                        prn_OriDupTri = Trim(prn_InpOpts)
                    End If
                End If

            End If
        End If
        If Trim(prn_OriDupTri) <> "" Then
            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_OriDupTri), PageWidth - 10, CurY - TxtHgt, 1, 0, pFont)
        End If

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
        If Trim(prn_HdDt.Rows(0).Item("Company_GSTinNo").ToString) <> "" Then
            Cmp_GSTIN_No = "GST NO.: " & prn_HdDt.Rows(0).Item("Company_GSTinNo").ToString
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
        Common_Procedures.Print_To_PrintDocument(e, Cmp_PhNo, LMargin - 70, CurY, 2, PrintWidth, pFont)
        'CurY = CurY + TxtHgt - 1
        'Common_Procedures.Print_To_PrintDocument(e, Cmp_TinNo, LMargin + 10, CurY, 0, 0, pFont)
        'Common_Procedures.Print_To_PrintDocument(e, Cmp_CstNo, PageWidth - 10, CurY, 1, 0, pFont)
        'CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, Cmp_GSTIN_No, PageWidth - 160, CurY, 1, 0, pFont)

        CurY = CurY + TxtHgt - 5
        p1Font = New Font("Calibri", 16, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "PROCESSING DELIVERY", LMargin, CurY, 2, PrintWidth, p1Font)
        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height



        CurY = CurY + strHeight  ' + 150
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(2) = CurY

        Try
            C1 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4)
            W1 = e.Graphics.MeasureString("PROCESSING  : ", pFont).Width
            S1 = e.Graphics.MeasureString("TO :    ", pFont).Width

            CurY = CurY + TxtHgt - 10
            p1Font = New Font("Calibri", 12, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "TO  :  " & "M/s." & prn_HdDt.Rows(0).Item("Ledger_Name").ToString, LMargin + 10, CurY, 0, 0, p1Font)
            Common_Procedures.Print_To_PrintDocument(e, "DC.NO", LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("ClothProcess_Delivery_No").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, p1Font)

            CurY = CurY + TxtHgt + 5
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address1").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
            p1Font = New Font("Calibri", 14, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "DATE", LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("ClothProcess_Delivery_Date").ToString), "dd-MM-yyyy").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt + 5
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address2").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)

            Common_Procedures.Print_To_PrintDocument(e, "LOT NO", LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("JobOrder_No").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt + 5
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address3").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)

            Dim vProcess_Nm1 = ""
            Dim vProcess_Nm2 = ""
            Dim i = 0

            vProcess_Nm1 = Trim(prn_HdDt.Rows(0).Item("Process_Name").ToString)
            If Trim(vProcess_Nm1) <> "" Then

                If Len(vProcess_Nm1) > 15 Then

                    For i = 20 To 1 Step -1

                        If Mid$(Trim(vProcess_Nm1), i, 1) = "@" Or Mid$(Trim(vProcess_Nm1), i, 1) = " " Or Mid$(Trim(vProcess_Nm1), i, 1) = "," Or Mid$(Trim(vProcess_Nm1), i, 1) = "." Or Mid$(Trim(vProcess_Nm1), i, 1) = "-" Or Mid$(Trim(vProcess_Nm1), i, 1) = "/" Or Mid$(Trim(vProcess_Nm1), i, 1) = "_" Or Mid$(Trim(vProcess_Nm1), i, 1) = "(" Or Mid$(Trim(vProcess_Nm1), i, 1) = ")" Or Mid$(Trim(vProcess_Nm1), i, 1) = "\" Or Mid$(Trim(vProcess_Nm1), i, 1) = "[" Or Mid$(Trim(vProcess_Nm1), i, 1) = "]" Or Mid$(Trim(vProcess_Nm1), i, 1) = "{" Or Mid$(Trim(vProcess_Nm1), i, 1) = "}" Or Mid$(Trim(vProcess_Nm1), i, 1) = "@" Then Exit For
                    Next i
                    If i = 0 Then i = 15

                    vProcess_Nm2 = Microsoft.VisualBasic.Right(Trim(vProcess_Nm1), Len(vProcess_Nm1) - i)
                    vProcess_Nm1 = Microsoft.VisualBasic.Left(Trim(vProcess_Nm1), i)



                End If
            End If

            Common_Procedures.Print_To_PrintDocument(e, "PROCESSING", LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Trim(vProcess_Nm1), LMargin + C1 + W1 + 30, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt + 5
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address4").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
            If Trim(vProcess_Nm2) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, Trim(vProcess_Nm2), LMargin + C1 + W1 + 30, CurY, 0, 0, pFont)
            Else
                Common_Procedures.Print_To_PrintDocument(e, "E-Way Bill No", LMargin + C1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("EwayBill_No").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, pFont)
            End If
            CurY = CurY + TxtHgt + 5
            p1Font = New Font("Calibri", 11, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "GST No: " & prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString, LMargin + S1 + 10, CurY, 0, 0, p1Font)

            If Trim(vProcess_Nm2) <> "" And prn_HdDt.Rows(0).Item("EwayBill_No").ToString <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, "E-Way Bill No", LMargin + C1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("EwayBill_No").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, pFont)
            End If

            If Trim(prn_HdDt.Rows(0).Item("Purchase_OrderNo").ToString) <> "" Then
                CurY = CurY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, "P.O.NO", LMargin + C1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Purchase_OrderNo").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, pFont)
            End If

            CurY = CurY + TxtHgt + 5
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(3) = CurY

            e.Graphics.DrawLine(Pens.Black, LMargin + C1, LnAr(3), LMargin + C1, LnAr(2))

            CurY = CurY + TxtHgt - 10
            Common_Procedures.Print_To_PrintDocument(e, "SNO", LMargin, CurY, 2, ClAr(1), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "ITEM NAME", LMargin + ClAr(1), CurY, 2, ClAr(2), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "COLOUR", LMargin + ClAr(1) + ClAr(2), CurY, 2, ClAr(3), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "PRODUCT", LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, 2, ClAr(4), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "COLOUR", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 2, ClAr(5), pFont)

            Common_Procedures.Print_To_PrintDocument(e, "BALE", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, 2, ClAr(6), pFont)
            'Common_Procedures.Print_To_PrintDocument(e, "BALES NOS", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 2, ClAr(5), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "PCS", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, 2, ClAr(7), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "METERS", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, 2, ClAr(8), pFont)


            CurY = CurY + TxtHgt + 5
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(4) = CurY

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub Printing_Format1_PageFooter(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single, ByVal NoofDets As Integer, ByVal is_LastPage As Boolean)
        Dim p1Font As Font
        Dim I As Integer
        Dim Cmp_Name As String
        Dim W1 As Single = 0
        Dim vprn_BlNos As String = ""
        Dim BLNo1 As String
        Dim BLNo2 As String
        Dim NoteStr1 As String = ""
        Dim NoteStr2 As String = ""
        Dim vTxamt As String = 0
        Dim vNtAMt As String = 0
        Dim vTxPerc As String
        Dim vSgst_amt As String = 0
        Dim vCgst_amt As String = 0
        Dim vIgst_amt As String = 0
        Dim vChk_GST_Bill As Integer = 0
        Dim C1 As Single
        Dim W2 As Single
        Dim W3 As Single

        W1 = e.Graphics.MeasureString("No.of Beams  : ", pFont).Width

        Try



            For I = NoofDets + 1 To NoofItems_PerPage

                CurY = CurY + TxtHgt



                prn_DetIndx = prn_DetIndx + 1

            Next

            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(5) = CurY

            CurY = CurY + TxtHgt - 10
            If is_LastPage = True Then
                Common_Procedures.Print_To_PrintDocument(e, " TOTAL", LMargin + ClAr(1) + ClAr(2) + 10, CurY, 2, ClAr(4), pFont)
            End If

            If Val(prn_HdDt.Rows(0).Item("Total_Pcs").ToString) <> 0 Then
                If is_LastPage = True Then
                    Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(0).Item("Total_Pcs").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 5, CurY, 1, 0, pFont)
                End If
            End If

            'If Val(prn_HdDt.Rows(0).Item("Total_Weight").ToString) <> 0 Then
            '    If is_LastPage = True Then
            '        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Total_Weight").ToString), "#########0.000"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, pFont)
            '    End If
            'End If

            If Val(prn_HdDt.Rows(0).Item("Total_Meters").ToString) <> 0 Then
                If is_LastPage = True Then
                    ' Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Total_Meters").ToString), "#########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY, 1, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Total_Meters").ToString), "#########0.00"), PageWidth - 10, CurY, 1, 0, pFont)
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
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), LnAr(3))

            CurY = CurY + TxtHgt - 5

            vTxPerc = 0
            vCgst_amt = 0
            vSgst_amt = 0
            vIgst_amt = 0
            ' 

            vChk_GST_Bill = Val(prn_HdDt.Rows(0).Item("GST_Tax_Invoice_Status").ToString)

            If Val(vChk_GST_Bill) = 1 Then



                If Val(prn_HdDt.Rows(0).Item("Company_State_IdNo").ToString) = Val(prn_HdDt.Rows(0).Item("Ledger_State_IdNo").ToString) Then

                    vTxPerc = Format(Val(prn_DetDt.Rows(0).Item("item_gst_percentage").ToString) / 2, "############0.00")

                    vCgst_amt = Format(Val(prn_HdDt.Rows(0).Item("Net_Amount").ToString) * Val(vTxPerc) / 100, "############0.00")
                    vSgst_amt = Format(Val(prn_HdDt.Rows(0).Item("Net_Amount").ToString) * Val(vTxPerc) / 100, "############0.00")

                Else

                    vTxPerc = prn_DetDt.Rows(0).Item("item_gst_percentage").ToString
                    vIgst_amt = Format(Val(prn_HdDt.Rows(0).Item("Net_Amount").ToString) * Val(vTxPerc) / 100, "############0.00")

                End If
            End If

            W1 = e.Graphics.MeasureString("Transport Name:", pFont).Width
            W2 = e.Graphics.MeasureString("CGST @ 2.5%:", pFont).Width
            W3 = e.Graphics.MeasureString("Value Of Goods :", pFont).Width

            C1 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(3)


            vprn_BlNos = ""
            For I = 0 To prn_DetDt.Rows.Count - 1
                If Trim(prn_DetDt.Rows(I).Item("Bales_Nos").ToString) <> "" Then
                    vprn_BlNos = Trim(vprn_BlNos) & IIf(Trim(vprn_BlNos) <> "", ", ", "") & prn_DetDt.Rows(I).Item("Bales_Nos").ToString
                End If
            Next

            ' CurY = CurY + TxtHgt
            BLNo1 = Trim(vprn_BlNos)
            BLNo2 = ""
            If Len(BLNo1) > 90 Then
                For I = 90 To 1 Step -1
                    If Mid$(Trim(BLNo1), I, 1) = " " Or Mid$(Trim(BLNo1), I, 1) = "," Or Mid$(Trim(BLNo1), I, 1) = "." Or Mid$(Trim(BLNo1), I, 1) = "-" Or Mid$(Trim(BLNo1), I, 1) = "/" Or Mid$(Trim(BLNo1), I, 1) = "_" Or Mid$(Trim(BLNo1), I, 1) = "(" Or Mid$(Trim(BLNo1), I, 1) = ")" Or Mid$(Trim(BLNo1), I, 1) = "\" Or Mid$(Trim(BLNo1), I, 1) = "[" Or Mid$(Trim(BLNo1), I, 1) = "]" Or Mid$(Trim(BLNo1), I, 1) = "{" Or Mid$(Trim(BLNo1), I, 1) = "}" Then Exit For
                Next I
                If I = 0 Then I = 90
                BLNo2 = Microsoft.VisualBasic.Right(Trim(BLNo1), Len(BLNo1) - I)
                BLNo1 = Microsoft.VisualBasic.Left(Trim(BLNo1), I)
            End If

            If Trim(BLNo1) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, "Bale/Bundle No : " & BLNo1, LMargin + 10, CurY, 0, 0, pFont)
            End If

            If Val(prn_HdDt.Rows(0).Item("Net_Amount").ToString) <> 0 And Val(vChk_GST_Bill) = 1 Then
                Common_Procedures.Print_To_PrintDocument(e, "Taxable Amount", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + W3 - 15, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Net_Amount").ToString), "##########0.00"), PageWidth - 10, CurY, 1, 0, pFont)
            Else
                If Val(prn_HdDt.Rows(0).Item("Net_Amount").ToString) <> 0 Then
                    Common_Procedures.Print_To_PrintDocument(e, "Value Of Goods", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + W3 - 15, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Net_Amount").ToString), "##########0.00"), PageWidth - 10, CurY, 1, 0, pFont)
                End If
            End If


            CurY = CurY + TxtHgt

            If Trim(BLNo2) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, Space(Len("Bale/Bundle No : ")) & BLNo2, LMargin + 10, CurY, 0, 0, pFont)
            End If
            If Val(prn_HdDt.Rows(0).Item("Net_Amount").ToString) <> 0 And Val(vChk_GST_Bill) = 1 Then

                If Val(vIgst_amt) <> 0 Then

                    Common_Procedures.Print_To_PrintDocument(e, "IGST @ " & Val(vTxPerc) & " %", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(6) + ClAr(7) + 10 + W3, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(vIgst_amt), "##########0.00"), PageWidth - 10, CurY, 1, 0, pFont)

                Else

                    Common_Procedures.Print_To_PrintDocument(e, "CGST @ " & Val(vTxPerc) & " %", LMargin + C1, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W2, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(vCgst_amt), "##########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + 50, CurY, 1, 0, pFont)

                    Common_Procedures.Print_To_PrintDocument(e, "SGST @ " & Val(vTxPerc) & " %", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + W3 - 15, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(vSgst_amt), "##########0.00"), PageWidth - 10, CurY, 1, 0, pFont)

                End If

            End If

            CurY = CurY + TxtHgt
            If Trim(prn_HdDt.Rows(0).Item("Vehicle_No").ToString) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, "Vehicle No : " & Trim(prn_HdDt.Rows(0).Item("Vehicle_No").ToString), LMargin + 10, CurY, 0, 0, pFont)
            End If

            If Val(prn_HdDt.Rows(0).Item("Net_Amount").ToString) <> 0 And Val(vChk_GST_Bill) = 1 Then
                vNtAMt = Format(Val(prn_HdDt.Rows(0).Item("Net_Amount").ToString) + Val(vCgst_amt) + Val(vSgst_amt) + Val(vIgst_amt), "###########0")

                Common_Procedures.Print_To_PrintDocument(e, "Value Of Goods", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + W3 - 15, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, Format(Val(vNtAMt), "###########0.00"), PageWidth - 10, CurY, 1, 0, pFont)

            End If

            CurY = CurY + TxtHgt + 10
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(7) = CurY

            If Trim(prn_HdDt.Rows(0).Item("Note").ToString) <> "" Then

                CurY = CurY + TxtHgt
                p1Font = New Font("Calibri", 9, FontStyle.Regular)
                NoteStr1 = "( Note: " & Trim(prn_HdDt.Rows(0).Item("Note").ToString) & " )"
                If Len(NoteStr1) > 90 Then
                    For I = 90 To 1 Step -1
                        If Mid$(Trim(NoteStr1), I, 1) = " " Or Mid$(Trim(NoteStr1), I, 1) = "," Or Mid$(Trim(NoteStr1), I, 1) = "." Or Mid$(Trim(NoteStr1), I, 1) = "-" Or Mid$(Trim(NoteStr1), I, 1) = "/" Or Mid$(Trim(NoteStr1), I, 1) = "_" Or Mid$(Trim(NoteStr1), I, 1) = "(" Or Mid$(Trim(NoteStr1), I, 1) = ")" Or Mid$(Trim(NoteStr1), I, 1) = "\" Or Mid$(Trim(NoteStr1), I, 1) = "[" Or Mid$(Trim(NoteStr1), I, 1) = "]" Or Mid$(Trim(NoteStr1), I, 1) = "{" Or Mid$(Trim(NoteStr1), I, 1) = "}" Then Exit For
                    Next I
                    If I = 0 Then I = 90
                    NoteStr2 = Microsoft.VisualBasic.Right(Trim(NoteStr1), Len(NoteStr1) - I)
                    NoteStr1 = Microsoft.VisualBasic.Left(Trim(NoteStr1), I)
                End If
                Common_Procedures.Print_To_PrintDocument(e, NoteStr1, LMargin + 10, CurY, 0, 0, p1Font)
                If NoteStr2 <> "" Then
                    CurY = CurY + TxtHgt
                    Common_Procedures.Print_To_PrintDocument(e, NoteStr2, LMargin + 10, CurY, 0, 0, p1Font)
                End If

            End If

            If Val(Common_Procedures.User.IdNo) <> 1 Then
                Common_Procedures.Print_To_PrintDocument(e, "(" & Trim(Common_Procedures.User.Name) & ")", LMargin + 400, CurY, 0, 0, pFont)
            End If

            CurY = CurY + TxtHgt
            CurY = CurY + TxtHgt
            Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString
            Common_Procedures.Print_To_PrintDocument(e, "Receiver's Signature", LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "Prepared By ", LMargin + 300, CurY, 0, 0, pFont)
            p1Font = New Font("Calibri", 12, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "For " & Cmp_Name, PageWidth - 15, CurY, 1, 0, p1Font)

            CurY = CurY + TxtHgt + 5

            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            e.Graphics.DrawLine(Pens.Black, LMargin, LnAr(1), LMargin, CurY)
            e.Graphics.DrawLine(Pens.Black, PageWidth, LnAr(1), PageWidth, CurY)

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub Printing_PackingSlip_Format2(ByRef PrintDocument1 As Printing.PrintDocument, ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByRef prn_HdDt As DataTable, ByVal prn_HdMxIndx As Integer, ByVal prn_DetMxIndx As Integer, ByRef prn_HdAr(,) As String, ByRef prn_DetAr(,,) As String, ByRef prn_PageNo As Integer, ByRef prn_Count As Integer, ByRef prn_HdIndx As Integer, ByRef prn_DetIndx As Integer)
        Dim NoofDets As Integer, NoofItems_PerPage As Integer
        Dim pFont As Font, P1fONT As Font
        Dim LMargin As Single, RMargin As Single, TMargin As Single, BMargin As Single
        Dim PrintWidth As Single, PrintHeight As Single
        Dim PageWidth As Single, PageHeight As Single
        Dim CurY As Single, TxtHgt As Single
        Dim LnAr(15) As Single, ClArr(15) As Single
        Dim ps As Printing.PaperSize
        Dim strHeight As Single = 0
        Dim PpSzSTS As Boolean = False
        Dim LM As Single = 0, TM As Single = 0
        Dim PgWt As Single = 0, PrWt As Single = 0
        Dim PgHt As Single = 0, PrHt As Single = 0
        Dim prn_totpcs As Integer = 0
        Dim prn_PcsDetails As String = ""
        Dim I As Integer
        Dim ItmName(10) As String

        For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
            If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
                ps = PrintDocument1.PrinterSettings.PaperSizes(I)
                PrintDocument1.DefaultPageSettings.PaperSize = ps
                e.PageSettings.PaperSize = ps
                Exit For
            End If
        Next

        With PrintDocument1.DefaultPageSettings.Margins
            .Left = 20
            .Right = 55
            .Top = 35
            .Bottom = 35
            LMargin = .Left
            RMargin = .Right
            TMargin = .Top
            BMargin = .Bottom
        End With

        With PrintDocument1.DefaultPageSettings.PaperSize
            PrintWidth = .Width - RMargin - LMargin
            PrintHeight = .Height - TMargin - BMargin
            PageWidth = .Width - RMargin
            PageHeight = .Height - BMargin
        End With
        'With PrintDocument1.DefaultPageSettings.PaperSize
        '    PrintWidth = (.Width / 2) - RMargin - LMargin
        '    PrintHeight = (.Height / 2) - TMargin - BMargin
        '    PageWidth = (.Width / 2) - RMargin
        '    PageHeight = (.Height / 2) - BMargin
        'End With
        If PrintDocument1.DefaultPageSettings.Landscape = True Then
            With PrintDocument1.DefaultPageSettings.PaperSize
                PrintWidth = .Height - TMargin - BMargin
                PrintHeight = .Width - RMargin - LMargin
                PageWidth = .Height - TMargin
                PageHeight = .Width - RMargin
            End With
        End If

        e.Graphics.TextRenderingHint = Drawing.Text.TextRenderingHint.AntiAlias

        pFont = New Font("Calibri", 10, FontStyle.Regular)

        NoofItems_PerPage = 25 ' 15 ' 29 ' 17 ' 20 

        Erase ClArr
        Erase LnAr
        ClArr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        LnAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}

        ClArr(1) = 55 : ClArr(2) = 95 : ClArr(3) = 80 : ClArr(4) = 95 : ClArr(5) = 95
        ClArr(6) = PageWidth - (LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5))

        'ClArr(1) = 100 : ClArr(2) = 80 : ClArr(3) = 80 : ClArr(4) = 80 : ClArr(5) = 80 : ClArr(6) = 80 : ClArr(7) = 80 : ClArr(8) = 80
        'ClArr(9) = PageWidth - (LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8))

        TxtHgt = 19 ' e.Graphics.MeasureString("A", pFont).Height  ' 20

        Try

            If prn_HdDt.Rows.Count > 0 Then

                If prn_HdMxIndx > 0 Then

                    Erase LnAr
                    LnAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}

                    Printing_PackingSlip_Format2_PageHeader(PrintDocument1, e, prn_HdDt, prn_HdAr, TxtHgt, pFont, LMargin, RMargin, TM, BMargin, PageWidth, PrintWidth, prn_PageNo, NoofItems_PerPage, CurY, LnAr, ClArr, prn_HdIndx)
                    CurY = CurY - 10

                    NoofDets = 0
                    Do While prn_HdIndx < prn_HdMxIndx

                        If NoofDets >= NoofItems_PerPage Then

                            CurY = CurY + TxtHgt
                            Common_Procedures.Print_To_PrintDocument(e, "Continued....", LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + ClArr(9) - 10, CurY, 1, 0, pFont)
                            NoofDets = NoofDets + 1

                            Printing_PackingSlip_Format2_PageFooter(e, prn_HdAr, TxtHgt, pFont, LMargin, RMargin, TM, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, NoofDets, prn_HdIndx, False)

                            'prn_DetIndx = prn_DetIndx + NoofItems_PerPage

                            e.HasMorePages = True

                            NoofDets = 0
                            prn_123Count = prn_123Count - 1
                            ' prn_Count = prn_Count + 1

                            Return

                        End If



                        prn_HdIndx = prn_HdIndx + 1

                        If Val(prn_HdAr(prn_HdIndx, 5)) <> 0 Then

                            CurY = CurY + TxtHgt

                            P1fONT = New Font("Calibri", 8, FontStyle.Regular)

                            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdAr(prn_HdIndx, 5)), LMargin + 15, CurY, 0, 0, P1fONT)
                            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdAr(prn_HdIndx, 1)), LMargin + ClArr(1) + 15, CurY, 0, 0, P1fONT)
                            Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdAr(prn_HdIndx, 2)), LMargin + ClArr(1) + ClArr(2) + ClArr(3) - 10, CurY, 1, 0, P1fONT)
                            Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdAr(prn_HdIndx, 3)), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) - 2, CurY, 1, 0, P1fONT)
                            Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdAr(prn_HdIndx, 4)), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) - 2, CurY, 1, 0, P1fONT)

                            ' Common_Procedures.Print_To_PrintDocument(e, Trim(ItmName(1)), LMargin + ClArr(1) + 10, CurY, 0, 0, P1fONT)
                            ' 
                            NoofDets = NoofDets + 1


                            prn_totpcs = 0
                            prn_PcsDetails = ""
                            Do While prn_totpcs < Val(prn_HdAr(prn_HdIndx, 2))

                                prn_totpcs = prn_totpcs + 1
                                ' prn_PcsDetails = prn_PcsDetails
                                prn_PcsDetails = Trim(prn_PcsDetails) & IIf(Trim(prn_PcsDetails) <> "", ", ", "") & Trim(prn_DetAr(prn_HdIndx, prn_totpcs, 3)) & "(" & Trim(prn_DetAr(prn_HdIndx, prn_totpcs, 1)) & "-" & Trim(prn_DetAr(prn_HdIndx, prn_totpcs, 2)) & ")"

                            Loop


                            Dim J As Integer
                            prn_DetSNo = prn_DetSNo + 1
                            J = 0
                            For k = 0 To 10
                                ItmName(k) = ""
                            Next


                            ItmName(0) = Trim(prn_PcsDetails)

                            If Len(ItmName(0)) > 70 Then
Lp:
                                For I = 65 To 1 Step -1
                                    If Mid$(Trim(ItmName(0)), I, 1) = " " Or Mid$(Trim(ItmName(0)), I, 1) = "," Or Mid$(Trim(ItmName(0)), I, 1) = "." Or Mid$(Trim(ItmName(0)), I, 1) = "-" Or Mid$(Trim(ItmName(0)), I, 1) = "/" Or Mid$(Trim(ItmName(0)), I, 1) = "_" Or Mid$(Trim(ItmName(0)), I, 1) = "(" Or Mid$(Trim(ItmName(0)), I, 1) = ")" Or Mid$(Trim(ItmName(0)), I, 1) = "\" Or Mid$(Trim(ItmName(0)), I, 1) = "[" Or Mid$(Trim(ItmName(0)), I, 1) = "]" Or Mid$(Trim(ItmName(0)), I, 1) = "{" Or Mid$(Trim(ItmName(0)), I, 1) = "}" Then Exit For
                                Next I
                                J = J + 1
                                If I = 0 Then I = 70
                                ItmName(J) = Microsoft.VisualBasic.Left(Trim(ItmName(0)), I - 1)
                                ItmName(0) = Microsoft.VisualBasic.Right(Trim(ItmName(0)), Len(ItmName(0)) - I)

                                If Len(ItmName(0)) > 70 Then
                                    GoTo Lp
                                End If
                            Else
                                ItmName(1) = ItmName(0)
                                ItmName(0) = ""
                            End If

                            Common_Procedures.Print_To_PrintDocument(e, Trim(ItmName(1)), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + 5, CurY, 0, 0, P1fONT)

                            If Trim(ItmName(2)) <> "" Then
                                CurY = CurY + TxtHgt - 5
                                Common_Procedures.Print_To_PrintDocument(e, Trim(ItmName(2)), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + 5, CurY, 0, 0, P1fONT)
                                NoofDets = NoofDets + 1
                            End If
                            If Trim(ItmName(3)) <> "" Then
                                CurY = CurY + TxtHgt - 5
                                Common_Procedures.Print_To_PrintDocument(e, Trim(ItmName(3)), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + 5, CurY, 0, 0, P1fONT)
                                NoofDets = NoofDets + 1
                            End If

                            If Trim(ItmName(4)) <> "" Then
                                CurY = CurY + TxtHgt - 5
                                Common_Procedures.Print_To_PrintDocument(e, Trim(ItmName(4)), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + 5, CurY, 0, 0, P1fONT)
                                NoofDets = NoofDets + 1
                            End If

                            If Trim(ItmName(5)) <> "" Then
                                CurY = CurY + TxtHgt - 5
                                Common_Procedures.Print_To_PrintDocument(e, Trim(ItmName(5)), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + 5, CurY, 0, 0, P1fONT)
                                NoofDets = NoofDets + 1
                            End If
                            If Trim(ItmName(6)) <> "" Then
                                CurY = CurY + TxtHgt - 5
                                Common_Procedures.Print_To_PrintDocument(e, Trim(ItmName(6)), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + 5, CurY, 0, 0, P1fONT)
                                NoofDets = NoofDets + 1
                            End If

                            If Trim(ItmName(7)) <> "" Then
                                CurY = CurY + TxtHgt - 5
                                Common_Procedures.Print_To_PrintDocument(e, Trim(ItmName(7)), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + 5, CurY, 0, 0, P1fONT)
                                NoofDets = NoofDets + 1
                            End If
                            If Trim(ItmName(0)) <> "" Then
                                CurY = CurY + TxtHgt - 5
                                Common_Procedures.Print_To_PrintDocument(e, Trim(ItmName(0)), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + 5, CurY, 0, 0, P1fONT)
                                NoofDets = NoofDets + 1
                            End If

                            'ItmNm1 = Trim(prn_PcsDetails)

                            'ItmNm2 = ""
                            'If Len(ItmNm1) > 70 Then
                            '    For I = 70 To 1 Step -1
                            '        If Mid$(Trim(ItmNm1), I, 1) = " " Or Mid$(Trim(ItmNm1), I, 1) = "," Or Mid$(Trim(ItmNm1), I, 1) = "." Or Mid$(Trim(ItmNm1), I, 1) = "-" Or Mid$(Trim(ItmNm1), I, 1) = "/" Or Mid$(Trim(ItmNm1), I, 1) = "_" Or Mid$(Trim(ItmNm1), I, 1) = "(" Or Mid$(Trim(ItmNm1), I, 1) = ")" Or Mid$(Trim(ItmNm1), I, 1) = "\" Or Mid$(Trim(ItmNm1), I, 1) = "[" Or Mid$(Trim(ItmNm1), I, 1) = "]" Or Mid$(Trim(ItmNm1), I, 1) = "{" Or Mid$(Trim(ItmNm1), I, 1) = "}" Then Exit For
                            '    Next I
                            '    If I = 0 Then I = 50
                            '    ItmNm2 = Microsoft.VisualBasic.Right(Trim(ItmNm1), Len(ItmNm1) - I)
                            '    ItmNm1 = Microsoft.VisualBasic.Left(Trim(ItmNm1), I - 1)
                            'End If

                            'Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm1), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + 5, CurY, 0, 0, P1fONT)

                            'If Trim(ItmNm2) <> "" Then
                            '    CurY = CurY + TxtHgt - 5
                            '    Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm2), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + 5, CurY, 0, 0, P1fONT)
                            'End If



                        End If

                    Loop

                    Printing_PackingSlip_Format2_PageFooter(e, prn_HdAr, TxtHgt, pFont, LMargin, RMargin, TM, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, NoofDets, prn_HdIndx, True)

                    If Trim(prn_InpOpts) <> "" Then
                        If prn_123Count < Len(Trim(prn_InpOpts)) Then


                            If Val(prn_InpOpts) <> "0" Then
                                prn_DetIndx = 0
                                prn_DetSNo = 0
                                prn_PageNo = 0
                                prn_HdIndx = 0
                                e.HasMorePages = True
                                Return
                            End If
                        Else
                            If Val(prn_DetailsCount) > Val(ChkPrintRow + 1) Then
                                ChkPrintRow = ChkPrintRow + 1
                                prn_Count = 0
                                prn_123Count = 0
                                e.HasMorePages = True
                                Return
                            Else
                                Exit Sub
                            End If
                        End If
                    End If
                End If

            End If

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

        e.HasMorePages = False

    End Sub

    Private Sub Printing_PackingSlip_Format2_PageHeader(ByRef PrintDocument1 As Printing.PrintDocument, ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByRef prn_HdDt As DataTable, ByRef prn_HdAr(,) As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByRef PageNo As Integer, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single, ByVal prn_HdIndx As Integer)
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt2 As New DataTable
        Dim dt3 As New DataTable
        Dim dt4 As New DataTable
        Dim p1Font As Font
        Dim strHeight As Single
        Dim Cmp_Add As String = ""
        Dim C1 As Single, W1, W2 As Single, S1, S2 As Single
        Dim Cmp_Name, Desc As String, Cmp_Add1 As String, Cmp_Add2 As String
        Dim Cmp_PhNo As String, Cmp_TinNo As String, Cmp_CstNo As String, Cmp_EMail As String, Cmp_GstNo As String
        Dim S As String

        PageNo = PageNo + 1

        CurY = TMargin + 30

        'da2 = New SqlClient.SqlDataAdapter("select a.*, b.Ledger_Name, c.Ledger_Name, d.Company_Description as Transport_Name from ClothSales_Invoice_Head a  INNER JOIN Ledger_Head b ON b.Ledger_IdNo = a.Ledger_Idno LEFT OUTER JOIN Ledger_Head c ON c.Ledger_IdNo = a.Transport_IdNo LEFT OUTER JOIN Company_Head d ON d.Company_IdNo = a.Company_IdNo where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.ClothSales_Invoice_Code = '" & Trim(EntryCode) & "' Order by a.For_OrderBy", con)
        'da2.Fill(dt2)
        'If dt2.Rows.Count > NoofItems_PerPage Then
        '    Common_Procedures.Print_To_PrintDocument(e, "Page : " & Trim(Val(PageNo)), PageWidth - 10, CurY - TxtHgt, 1, 0, pFont)
        'End If
        'dt2.Clear()

        prn_Count = prn_Count + 1
        prn_123Count = prn_123Count + 1
        prn_OriDupTri = ""
        If Trim(prn_InpOpts) <> "" Then
            If prn_123Count <= Len(Trim(prn_InpOpts)) Then

                S = Mid$(Trim(prn_InpOpts), prn_123Count, 1)

                If Val(S) = 1 Then
                    prn_OriDupTri = "ORIGINAL"
                ElseIf Val(S) = 2 Then
                    prn_OriDupTri = "DUPLICATE"
                Else
                    If Trim(prn_InpOpts) <> "0" And Val(prn_InpOpts) = "0" And Len(Trim(prn_InpOpts)) > 3 Then
                        prn_OriDupTri = Trim(prn_InpOpts)
                    End If
                End If

            End If
        End If

        p1Font = New Font("Calibri", 15, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "PACKING LIST", LMargin, CurY - TxtHgt - 5, 2, PrintWidth, p1Font)
        If Trim(prn_OriDupTri) <> "" Then
            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_OriDupTri), PageWidth - 10, CurY - TxtHgt, 1, 0, pFont)
        End If

        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(1) = CurY
        Desc = ""
        Cmp_Name = "" : Cmp_Add1 = "" : Cmp_Add2 = ""
        Cmp_PhNo = "" : Cmp_TinNo = "" : Cmp_CstNo = "" : Cmp_EMail = "" : Cmp_GstNo = ""

        Desc = prn_HdDt.Rows(0).Item("Company_Description").ToString
        Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString
        Cmp_Add1 = prn_HdDt.Rows(0).Item("Company_Address1").ToString & " " & prn_HdDt.Rows(0).Item("Company_Address2").ToString
        Cmp_Add2 = prn_HdDt.Rows(0).Item("Company_Address3").ToString & " " & prn_HdDt.Rows(0).Item("Company_Address4").ToString

        If Trim(prn_HdDt.Rows(0).Item("Company_PhoneNo").ToString) <> "" Then
            Cmp_PhNo = "PHONE : " & prn_HdDt.Rows(0).Item("Company_PhoneNo").ToString
        End If
        If Trim(prn_HdDt.Rows(0).Item("Company_TinNo").ToString) <> "" Then
            Cmp_TinNo = "TIN NO.: " & prn_HdDt.Rows(0).Item("Company_TinNo").ToString
        End If
        If Trim(prn_HdDt.Rows(0).Item("Company_CstNo").ToString) <> "" Then
            Cmp_CstNo = "CST NO.: " & prn_HdDt.Rows(0).Item("Company_CstNo").ToString
        End If
        If Trim(prn_HdDt.Rows(0).Item("Company_GSTinNo").ToString) <> "" Then
            Cmp_GstNo = "GST NO.: " & prn_HdDt.Rows(0).Item("Company_GSTinNo").ToString
        End If
        If Trim(prn_HdDt.Rows(0).Item("Company_EMail").ToString) <> "" Then
            Cmp_EMail = "MAIL ID : " & prn_HdDt.Rows(0).Item("Company_EMail").ToString
        End If

        p1Font = New Font("Calibri", 15, FontStyle.Bold)
        'If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1018" Then '---- M.K Textiles (Palladam)
        '    p1Font = New Font("Calibri", 15, FontStyle.Bold)
        '    Common_Procedures.Print_To_PrintDocument(e, "PACKING LIST", LMargin, CurY, 2, PrintWidth, p1Font)
        'End If
        CurY = CurY + TxtHgt - 10
        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height

        'If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1018" Then '---- M.K Textiles (Palladam)
        '    e.Graphics.DrawImage(DirectCast(Global.Textile.My.Resources.Resources.Company_Logo_MK, Drawing.Image), LMargin + 20, CurY, 112, 80)
        '    'e.Graphics.DrawImage(DirectCast(Global.Textile.My.Resources.Resources.Company_Logo_MK_2, Drawing.Image), LMargin + 20, CurY, 115, 80)
        '    'e.Graphics.DrawImage(DirectCast(Global.Textile.My.Resources.Resources.Company_Logo_MK, Drawing.Image), LMargin + 20, CurY, 75, 75)
        'End If

        p1Font = New Font("Calibri", 18, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Name, LMargin, CurY, 2, PrintWidth, p1Font)
        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height

        CurY = CurY + strHeight - 1
        'Common_Procedures.Print_To_PrintDocument(e, Desc, LMargin, CurY, 2, PrintWidth, pFont)

        'CurY = CurY + TxtHgt - 1
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Add1, LMargin, CurY, 2, PrintWidth, pFont)

        CurY = CurY + TxtHgt - 1
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Add2, LMargin, CurY, 2, PrintWidth, pFont)
        CurY = CurY + TxtHgt - 1
        Common_Procedures.Print_To_PrintDocument(e, Cmp_EMail, LMargin, CurY, 2, PrintWidth, pFont)
        'CurY = CurY + TxtHgt - 1
        'Common_Procedures.Print_To_PrintDocument(e, Cmp_PhNo, LMargin, CurY, 2, PrintWidth, pFont)
        p1Font = New Font("CALIBRI", 11, FontStyle.Bold)
        CurY = CurY + TxtHgt - 1
        'Common_Procedures.Print_To_PrintDocument(e, Cmp_TinNo, LMargin + 10, CurY, 0, 0, pFont)
        'Common_Procedures.Print_To_PrintDocument(e, Cmp_EMail, LMargin, CurY, 2, PrintWidth, pFont)
        'Common_Procedures.Print_To_PrintDocument(e, Cmp_CstNo, PageWidth - 10, CurY, 1, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, Cmp_GstNo, PageWidth - 10, CurY, 1, 0, p1Font)

        CurY = CurY + TxtHgt + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(2) = CurY

        C1 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5)
        W1 = e.Graphics.MeasureString("INVOICE DATE  : ", pFont).Width
        S1 = e.Graphics.MeasureString("TO     :    ", pFont).Width
        W2 = e.Graphics.MeasureString("Despatch To   : ", pFont).Width
        S2 = e.Graphics.MeasureString("Sent Through  : ", pFont).Width


        CurY = CurY + 10
        p1Font = New Font("Calibri", 12, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "TO  :  " & "M/s." & prn_HdDt.Rows(0).Item("Ledger_Name").ToString, LMargin + 10, CurY, 0, 0, p1Font)
        Common_Procedures.Print_To_PrintDocument(e, "DELIVERY NO", LMargin + C1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("ClothProcess_Delivery_No").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, p1Font)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address1").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
        p1Font = New Font("Calibri", 14, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "DELIVERY DATE", LMargin + C1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("ClothProcess_Delivery_Date").ToString), "dd-MM-yyyy").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, pFont)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address2").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
        'Common_Procedures.Print_To_PrintDocument(e, "ITEM", LMargin + C1 + 10, CurY, 0, 0, pFont)
        'Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
        'Common_Procedures.Print_To_PrintDocument(e, prn_HdAr(prn_HdMxIndx, 2), LMargin + C1 + W1 + 30, CurY, 0, 0, p1Font)
        Common_Procedures.Print_To_PrintDocument(e, "LOT NO.", LMargin + C1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("JobOrder_no").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, pFont)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address3").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address4").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
        pFont = New Font("CALIBRI", 11, FontStyle.Bold)
        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, "GST NO.: " & prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
        'If Trim(prn_HdDt.Rows(0).Item("Dc_No").ToString) <> "" Then
        '    Common_Procedures.Print_To_PrintDocument(e, "DC NO : " & prn_HdDt.Rows(0).Item("Dc_No").ToString, LMargin + C1 + 10, CurY, 0, 0, pFont)
        'End If
        'If Trim(prn_HdDt.Rows(0).Item("Dc_Date").ToString) <> "" Then
        '    Common_Procedures.Print_To_PrintDocument(e, "DC DATE : " & prn_HdDt.Rows(0).Item("Dc_Date").ToString, LMargin + C1 + 100, CurY, 0, 0, pFont)
        'End If

        'CurY = CurY + TxtHgt
        'If Trim(prn_HdDt.Rows(0).Item("Ledger_TinNo").ToString) <> "" Then
        '    Common_Procedures.Print_To_PrintDocument(e, " TIN : " & prn_HdDt.Rows(0).Item("Ledger_TinNo").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
        'End If

        CurY = CurY + TxtHgt
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        e.Graphics.DrawLine(Pens.Black, LMargin + C1, CurY, LMargin + C1, LnAr(2))

        Try

            CurY = CurY + TxtHgt - 5
            Common_Procedures.Print_To_PrintDocument(e, "ITEM", LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdAr(prn_HdMxIndx, 6), LMargin + W1 + 25, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt + 10
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(2) = CurY

            CurY = CurY + TxtHgt - 10
            Common_Procedures.Print_To_PrintDocument(e, "S.No", LMargin, CurY, 2, ClAr(1), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "Bale No.", LMargin + ClAr(1), CurY, 2, ClAr(2), pFont)

            If Trim(Common_Procedures.settings.CustomerCode) = "1558" Then ' --- sotexpa 

                Common_Procedures.Print_To_PrintDocument(e, "No Of", LMargin + ClAr(1) + ClAr(2), CurY, 2, ClAr(3), pFont)
                Common_Procedures.Print_To_PrintDocument(e, "Bag/Roll", LMargin + ClAr(1) + ClAr(2), CurY + TxtHgt, 2, ClAr(3), pFont)
                Common_Procedures.Print_To_PrintDocument(e, "Pcs/Mtrs", LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, 2, ClAr(4), pFont)

            Else
                Common_Procedures.Print_To_PrintDocument(e, "No Of", LMargin + ClAr(1) + ClAr(2), CurY, 2, ClAr(3), pFont)
                Common_Procedures.Print_To_PrintDocument(e, "Pieces", LMargin + ClAr(1) + ClAr(2), CurY + TxtHgt, 2, ClAr(3), pFont)
                Common_Procedures.Print_To_PrintDocument(e, "Bale Mtrs", LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, 2, ClAr(4), pFont)

            End If
            Common_Procedures.Print_To_PrintDocument(e, "Net Weight", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 2, ClAr(5), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "Individual Piece(s) Mtrs(Lot No,Pieces)", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, 2, ClAr(6), pFont)



            'Common_Procedures.Print_To_PrintDocument(e, "PCS-6", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, 2, ClAr(7), pFont)
            'Common_Procedures.Print_To_PrintDocument(e, "PCS-7", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, 2, ClAr(8), pFont)
            'Common_Procedures.Print_To_PrintDocument(e, "METERS", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, 2, ClAr(9), pFont)

            CurY = CurY + TxtHgt + 20
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(3) = CurY
            e.Graphics.DrawLine(Pens.Black, LMargin, LnAr(1), LMargin, CurY)
            e.Graphics.DrawLine(Pens.Black, PageWidth, LnAr(1), PageWidth, CurY)


        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub Printing_PackingSlip_Format2_PageFooter(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByRef prn_HdAr(,) As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single, ByVal NoofDets As Integer, ByVal prn_HdIndx As Integer, ByVal is_LastPage As Boolean)
        ' Dim I As Integer
        Dim p1Font As Font
        Dim W1 As Single = 0

        Try

            'For I = NoofDets + 1 To NoofItems_PerPage
            '    CurY = CurY + TxtHgt
            'Next

            ' W1 = e.Graphics.MeasureString("INVOICE DATE  : ", pFont).Width
            CurY = CurY + TxtHgt

            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(4) = CurY


            ' Common_Procedures.Print_To_PrintDocument(e, Trim(Val(prn_HdAr(prn_HdIndx, 3))), LMargin + ClAr(1) + 15, CurY, 0, 0, pFont)
            ' Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdAr(prn_HdIndx, 4)), "#########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) - 15, CurY, 1, 0, pFont)

            CurY = CurY + TxtHgt - 10
            If is_LastPage = True Then
                Common_Procedures.Print_To_PrintDocument(e, "TOTAL", LMargin + ClAr(1), CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, Format(Val(Total_PCS), "#########0"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) - 10, CurY, 1, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, Format(Val(Total_mtrs), "#########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) - 2, CurY, 1, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, Format(Val(Total_wEIGHT), "#########0.000"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 2, CurY, 1, 0, pFont)
            End If

            CurY = CurY + TxtHgt + 5
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(5) = CurY

            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, LMargin, LnAr(2))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1), CurY, LMargin + ClAr(1), LnAr(2))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2), CurY, LMargin + ClAr(1) + ClAr(2), LnAr(2))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3), LnAr(2))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), LnAr(2))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(2))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), LnAr(2))


            If is_LastPage = True Then
                CurY = CurY + TxtHgt - 10
                Common_Procedures.Print_To_PrintDocument(e, "Above 80 Mtrs", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + 150, CurY, 1, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, " :", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + 150, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, Format(Val(Total_Mtrs_Abv80), "#########0.00"), PageWidth - 5, CurY, 1, 0, pFont)

                CurY = CurY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, "40 To 79 Mtrs", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + 150, CurY, 1, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, " :", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + 150, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, Format(Val(Total_Mtrs_40To79), "#########0.00"), PageWidth - 5, CurY, 1, 0, pFont)

                CurY = CurY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, "20 To 40 Mtrs", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + 150, CurY, 1, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, " :", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + 150, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, Format(Val(Total_Mtrs_20To40), "#########0.00"), PageWidth - 5, CurY, 1, 0, pFont)

                CurY = CurY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, "Total Mtrs", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + 150, CurY, 1, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, " :", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + 150, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, Format(Val(Total_mtrs), "#########0.00"), PageWidth - 5, CurY, 1, 0, pFont)

                CurY = CurY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, "No Of Bales", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + 150, CurY, 1, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, " :", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + 150, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdMxIndx), "#########0.00"), PageWidth - 5, CurY, 1, 0, pFont)

                CurY = CurY + TxtHgt
                CurY = CurY + TxtHgt

                p1Font = New Font("Calibri", 12, FontStyle.Bold)
                Common_Procedures.Print_To_PrintDocument(e, "For " & Trim(prn_HdDt.Rows(0).Item("Company_Name").ToString), PageWidth - 5, CurY, 1, 0, p1Font)
                CurY = CurY + TxtHgt
                CurY = CurY + TxtHgt
                CurY = CurY + TxtHgt

                p1Font = New Font("Calibri", 12, FontStyle.Bold)

                Common_Procedures.Print_To_PrintDocument(e, "AUTHORISED SIGNATORY ", PageWidth - 5, CurY, 1, 0, pFont)
                CurY = CurY + TxtHgt + 10

            Else

                CurY = CurY + TxtHgt
                CurY = CurY + TxtHgt
                CurY = CurY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, "Continued....", PageWidth - 5, CurY, 1, 0, pFont)
                CurY = CurY + TxtHgt + 10

            End If

            'e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            'e.Graphics.DrawLine(Pens.Black, LMargin, LnAr(1), LMargin, CurY)
            'e.Graphics.DrawLine(Pens.Black, PageWidth, LnAr(1), PageWidth, CurY)


        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub Printing_PackingSlip_Format3(ByRef PrintDocument1 As Printing.PrintDocument, ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByRef prn_HdDt As DataTable, ByVal prn_HdMxIndx As Integer, ByVal prn_DetMxIndx As Integer, ByRef prn_HdAr(,) As String, ByRef prn_DetAr(,,) As String, ByRef prn_PageNo As Integer, ByRef prn_Count As Integer, ByRef prn_HdIndx As Integer, ByRef prn_DetIndx As Integer)
        Dim NoofDets As Integer, NoofItems_PerPage As Integer
        Dim pFont As Font, P1fONT As Font
        Dim LMargin As Single, RMargin As Single, TMargin As Single, BMargin As Single
        Dim PrintWidth As Single, PrintHeight As Single
        Dim PageWidth As Single, PageHeight As Single
        Dim CurY As Single, TxtHgt As Single
        Dim LnAr(15) As Single, ClArr(15) As Single
        Dim ps As Printing.PaperSize
        Dim strHeight As Single = 0
        Dim PpSzSTS As Boolean = False
        Dim LM As Single = 0, TM As Single = 0
        Dim PgWt As Single = 0, PrWt As Single = 0
        Dim PgHt As Single = 0, PrHt As Single = 0
        Dim prn_totpcs As Integer = 0
        Dim prn_PcsDetails As String = ""
        Dim ItmNm1 As String, ItmNm2 As String
        Dim I As Integer

        For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
            If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
                ps = PrintDocument1.PrinterSettings.PaperSizes(I)
                PrintDocument1.DefaultPageSettings.PaperSize = ps
                e.PageSettings.PaperSize = ps
                Exit For
            End If
        Next

        With PrintDocument1.DefaultPageSettings.Margins
            .Left = 20
            .Right = 55
            .Top = 35
            .Bottom = 35
            LMargin = .Left
            RMargin = .Right
            TMargin = .Top
            BMargin = .Bottom
        End With

        With PrintDocument1.DefaultPageSettings.PaperSize
            PrintWidth = .Width - RMargin - LMargin
            PrintHeight = .Height - TMargin - BMargin
            PageWidth = .Width - RMargin
            PageHeight = .Height - BMargin
        End With
        'With PrintDocument1.DefaultPageSettings.PaperSize
        '    PrintWidth = (.Width / 2) - RMargin - LMargin
        '    PrintHeight = (.Height / 2) - TMargin - BMargin
        '    PageWidth = (.Width / 2) - RMargin
        '    PageHeight = (.Height / 2) - BMargin
        'End With
        If PrintDocument1.DefaultPageSettings.Landscape = True Then
            With PrintDocument1.DefaultPageSettings.PaperSize
                PrintWidth = .Height - TMargin - BMargin
                PrintHeight = .Width - RMargin - LMargin
                PageWidth = .Height - TMargin
                PageHeight = .Width - RMargin
            End With
        End If

        e.Graphics.TextRenderingHint = Drawing.Text.TextRenderingHint.AntiAlias

        pFont = New Font("Calibri", 10, FontStyle.Regular)

        NoofItems_PerPage = 15 ' 29 ' 17 ' 20 

        Erase ClArr
        Erase LnAr
        ClArr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        LnAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}

        ClArr(1) = 55 : ClArr(2) = 95 : ClArr(3) = 80 : ClArr(4) = 95 : ClArr(5) = 95
        ClArr(6) = PageWidth - (LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5))

        'ClArr(1) = 100 : ClArr(2) = 80 : ClArr(3) = 80 : ClArr(4) = 80 : ClArr(5) = 80 : ClArr(6) = 80 : ClArr(7) = 80 : ClArr(8) = 80
        'ClArr(9) = PageWidth - (LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8))

        TxtHgt = 19 ' e.Graphics.MeasureString("A", pFont).Height  ' 20

        Try
            prn_HdIndx = 0
            If prn_HdDt.Rows.Count > 0 Then

                If prn_HdMxIndx > 0 Then

                    Erase LnAr
                    LnAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}

                    Printing_PackingSlip_Format3_PageHeader(PrintDocument1, e, prn_HdDt, prn_HdAr, TxtHgt, pFont, LMargin, RMargin, TM, BMargin, PageWidth, PrintWidth, prn_PageNo, NoofItems_PerPage, CurY, LnAr, ClArr, prn_HdIndx)
                    CurY = CurY - 10

                    NoofDets = 0
                    Do While prn_HdIndx < prn_HdMxIndx

                        If NoofDets >= NoofItems_PerPage Then

                            CurY = CurY + TxtHgt
                            Common_Procedures.Print_To_PrintDocument(e, "Continued....", LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + ClArr(9) - 10, CurY, 1, 0, pFont)
                            NoofDets = NoofDets + 1

                            Printing_PackingSlip_Format3_PageFooter(e, prn_HdAr, TxtHgt, pFont, LMargin, RMargin, TM, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, NoofDets, prn_HdIndx, False)

                            'prn_DetIndx = prn_DetIndx + NoofItems_PerPage

                            e.HasMorePages = True

                            NoofDets = 0
                            prn_Count = prn_Count + 1

                            Return

                        End If

                        prn_HdIndx = prn_HdIndx + 1

                        If Val(prn_HdAr(prn_HdIndx, 5)) <> 0 Then

                            CurY = CurY + TxtHgt

                            P1fONT = New Font("Calibri", 8, FontStyle.Regular)

                            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdAr(prn_HdIndx, 5)), LMargin + 15, CurY, 0, 0, P1fONT)
                            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdAr(prn_HdIndx, 1)), LMargin + ClArr(1) + 15, CurY, 0, 0, P1fONT)
                            Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdAr(prn_HdIndx, 2)), LMargin + ClArr(1) + ClArr(2) + ClArr(3) - 10, CurY, 1, 0, P1fONT)
                            Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdAr(prn_HdIndx, 3)), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) - 2, CurY, 1, 0, P1fONT)
                            Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdAr(prn_HdIndx, 4)), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) - 2, CurY, 1, 0, P1fONT)

                            prn_totpcs = 0
                            prn_PcsDetails = ""
                            Do While prn_totpcs < Val(prn_HdAr(prn_HdIndx, 2))

                                prn_totpcs = prn_totpcs + 1
                                ' prn_PcsDetails = prn_PcsDetails
                                prn_PcsDetails = Trim(prn_PcsDetails) & IIf(Trim(prn_PcsDetails) <> "", ", ", "") & Trim(prn_DetAr(prn_HdIndx, prn_totpcs, 3)) & "(" & Trim(prn_DetAr(prn_HdIndx, prn_totpcs, 1)) & "-" & Trim(prn_DetAr(prn_HdIndx, prn_totpcs, 2)) & ")"

                            Loop


                            ItmNm1 = Trim(prn_PcsDetails)

                            ItmNm2 = ""
                            If Len(ItmNm1) > 70 Then
                                For I = 70 To 1 Step -1
                                    If Mid$(Trim(ItmNm1), I, 1) = " " Or Mid$(Trim(ItmNm1), I, 1) = "," Or Mid$(Trim(ItmNm1), I, 1) = "." Or Mid$(Trim(ItmNm1), I, 1) = "-" Or Mid$(Trim(ItmNm1), I, 1) = "/" Or Mid$(Trim(ItmNm1), I, 1) = "_" Or Mid$(Trim(ItmNm1), I, 1) = "(" Or Mid$(Trim(ItmNm1), I, 1) = ")" Or Mid$(Trim(ItmNm1), I, 1) = "\" Or Mid$(Trim(ItmNm1), I, 1) = "[" Or Mid$(Trim(ItmNm1), I, 1) = "]" Or Mid$(Trim(ItmNm1), I, 1) = "{" Or Mid$(Trim(ItmNm1), I, 1) = "}" Then Exit For
                                Next I
                                If I = 0 Then I = 50
                                ItmNm2 = Microsoft.VisualBasic.Right(Trim(ItmNm1), Len(ItmNm1) - I)
                                ItmNm1 = Microsoft.VisualBasic.Left(Trim(ItmNm1), I - 1)
                            End If

                            Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm1), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + 5, CurY, 0, 0, P1fONT)

                            If Trim(ItmNm2) <> "" Then
                                CurY = CurY + TxtHgt - 5
                                Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm2), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + 5, CurY, 0, 0, P1fONT)
                            End If


                            'End If
                            'If Val(prn_DetAr(prn_HdIndx, 2, 3)) <> 0 Then
                            '    Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr(prn_HdIndx, 2, 1)) & "/" & Trim(prn_DetAr(prn_HdIndx, 2, 2)), LMargin + ClArr(1) + ClArr(2) + 5, CurY, 0, 0, P1fONT)
                            '    Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr(prn_HdIndx, 2, 3)), LMargin + ClArr(1) + ClArr(2) + ClArr(3) - 2, CurY, 1, 0, P1fONT)

                            'End If
                            'If Val(prn_DetAr(prn_HdIndx, 3, 3)) <> 0 Then
                            '    Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr(prn_HdIndx, 3, 1)) & "/" & Trim(prn_DetAr(prn_HdIndx, 3, 2)), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + 5, CurY, 0, 0, P1fONT)
                            '    Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr(prn_HdIndx, 3, 3)), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) - 2, CurY, 1, 0, P1fONT)

                            'End If

                            'If Val(prn_DetAr(prn_HdIndx, 4, 3)) <> 0 Then
                            '    Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr(prn_HdIndx, 4, 1)) & "/" & Trim(prn_DetAr(prn_HdIndx, 4, 2)), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + 5, CurY, 0, 0, P1fONT)
                            '    Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr(prn_HdIndx, 4, 3)), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) - 2, CurY, 1, 0, P1fONT)

                            'End If
                            'If Val(prn_DetAr(prn_HdIndx, 5, 3)) <> 0 Then
                            '    Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr(prn_HdIndx, 5, 1)) & "/" & Trim(prn_DetAr(prn_HdIndx, 5, 2)), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + 5, CurY, 0, 0, P1fONT)
                            '    Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr(prn_HdIndx, 5, 3)), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) - 2, CurY, 1, 0, P1fONT)

                            'End If
                            'If Val(prn_DetAr(prn_HdIndx, 6, 3)) <> 0 Then
                            '    Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr(prn_HdIndx, 6, 1)) & "/" & Trim(prn_DetAr(prn_HdIndx, 6, 2)), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + 5, CurY, 0, 0, P1fONT)
                            '    Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr(prn_HdIndx, 6, 3)), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) - 2, CurY, 1, 0, P1fONT)

                            'End If
                            'If Val(prn_DetAr(prn_HdIndx, 7, 3)) <> 0 Then
                            '    Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr(prn_HdIndx, 7, 1)) & "/" & Trim(prn_DetAr(prn_HdIndx, 7, 2)), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + 5, CurY, 0, 0, P1fONT)
                            '    Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr(prn_HdIndx, 7, 3)), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) - 2, CurY, 1, 0, P1fONT)

                            'End If

                            ' Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdAr(prn_HdIndx, 4)), "#########0.00"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + ClArr(9) - 2, CurY, 1, 0, pFont)

                            NoofDets = NoofDets + 1

                        End If

                    Loop

                    Printing_PackingSlip_Format3_PageFooter(e, prn_HdAr, TxtHgt, pFont, LMargin, RMargin, TM, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, NoofDets, prn_HdIndx, True)

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

            End If

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

        e.HasMorePages = False

    End Sub

    Private Sub Printing_PackingSlip_Format3_PageHeader(ByRef PrintDocument1 As Printing.PrintDocument, ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByRef prn_HdDt As DataTable, ByRef prn_HdAr(,) As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByRef PageNo As Integer, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single, ByVal prn_HdIndx As Integer)
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt2 As New DataTable
        Dim dt3 As New DataTable
        Dim dt4 As New DataTable
        Dim p1Font As Font
        Dim strHeight As Single
        Dim Cmp_Add As String = ""
        Dim C1 As Single, W1, W2 As Single, S1, S2 As Single
        Dim Cmp_Name, Desc As String, Cmp_Add1 As String, Cmp_Add2 As String
        Dim Cmp_PhNo As String, Cmp_TinNo As String, Cmp_CstNo As String, Cmp_EMail As String
        Dim S As String

        PageNo = PageNo + 1

        CurY = TMargin + 30

        'da2 = New SqlClient.SqlDataAdapter("select a.*, b.Ledger_Name, c.Ledger_Name, d.Company_Description as Transport_Name from ClothSales_Invoice_Head a  INNER JOIN Ledger_Head b ON b.Ledger_IdNo = a.Ledger_Idno LEFT OUTER JOIN Ledger_Head c ON c.Ledger_IdNo = a.Transport_IdNo LEFT OUTER JOIN Company_Head d ON d.Company_IdNo = a.Company_IdNo where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.ClothSales_Invoice_Code = '" & Trim(EntryCode) & "' Order by a.For_OrderBy", con)
        'da2.Fill(dt2)
        'If dt2.Rows.Count > NoofItems_PerPage Then
        '    Common_Procedures.Print_To_PrintDocument(e, "Page : " & Trim(Val(PageNo)), PageWidth - 10, CurY - TxtHgt, 1, 0, pFont)
        'End If
        'dt2.Clear()

        prn_Count = prn_Count + 1
        prn_OriDupTri = ""
        If Trim(prn_InpOpts) <> "" Then
            If prn_Count <= Len(Trim(prn_InpOpts)) Then

                S = Mid$(Trim(prn_InpOpts), prn_Count, 1)

                If Val(S) = 1 Then
                    prn_OriDupTri = "ORIGINAL"
                ElseIf Val(S) = 2 Then
                    prn_OriDupTri = "DUPLICATE"
                Else
                    If Trim(prn_InpOpts) <> "0" And Val(prn_InpOpts) = "0" And Len(Trim(prn_InpOpts)) > 3 Then
                        prn_OriDupTri = Trim(prn_InpOpts)
                    End If
                End If

            End If
        End If

        p1Font = New Font("Calibri", 15, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "PACKING LIST", LMargin, CurY - TxtHgt - 5, 2, PrintWidth, p1Font)
        If Trim(prn_OriDupTri) <> "" Then
            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_OriDupTri), PageWidth - 10, CurY - TxtHgt, 1, 0, pFont)
        End If

        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(1) = CurY
        Desc = ""
        Cmp_Name = "" : Cmp_Add1 = "" : Cmp_Add2 = ""
        Cmp_PhNo = "" : Cmp_TinNo = "" : Cmp_CstNo = "" : Cmp_EMail = ""

        Desc = prn_HdDt.Rows(0).Item("Company_Description").ToString
        Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString
        Cmp_Add1 = prn_HdDt.Rows(0).Item("Company_Address1").ToString & " " & prn_HdDt.Rows(0).Item("Company_Address2").ToString
        Cmp_Add2 = prn_HdDt.Rows(0).Item("Company_Address3").ToString & " " & prn_HdDt.Rows(0).Item("Company_Address4").ToString

        If Trim(prn_HdDt.Rows(0).Item("Company_PhoneNo").ToString) <> "" Then
            Cmp_PhNo = "PHONE : " & prn_HdDt.Rows(0).Item("Company_PhoneNo").ToString
        End If
        If Trim(prn_HdDt.Rows(0).Item("Company_TinNo").ToString) <> "" Then
            Cmp_TinNo = "TIN NO.: " & prn_HdDt.Rows(0).Item("Company_TinNo").ToString
        End If
        If Trim(prn_HdDt.Rows(0).Item("Company_CstNo").ToString) <> "" Then
            Cmp_CstNo = "CST NO.: " & prn_HdDt.Rows(0).Item("Company_CstNo").ToString
        End If
        If Trim(prn_HdDt.Rows(0).Item("Company_EMail").ToString) <> "" Then
            Cmp_EMail = "MAIL ID : " & prn_HdDt.Rows(0).Item("Company_EMail").ToString
        End If

        p1Font = New Font("Calibri", 15, FontStyle.Bold)
        'If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1018" Then '---- M.K Textiles (Palladam)
        '    p1Font = New Font("Calibri", 15, FontStyle.Bold)
        '    Common_Procedures.Print_To_PrintDocument(e, "PACKING LIST", LMargin, CurY, 2, PrintWidth, p1Font)
        'End If
        CurY = CurY + TxtHgt - 10
        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height

        'If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1018" Then '---- M.K Textiles (Palladam)
        '    e.Graphics.DrawImage(DirectCast(Global.Textile.My.Resources.Resources.Company_Logo_MK, Drawing.Image), LMargin + 20, CurY, 112, 80)
        '    'e.Graphics.DrawImage(DirectCast(Global.Textile.My.Resources.Resources.Company_Logo_MK_2, Drawing.Image), LMargin + 20, CurY, 115, 80)
        '    'e.Graphics.DrawImage(DirectCast(Global.Textile.My.Resources.Resources.Company_Logo_MK, Drawing.Image), LMargin + 20, CurY, 75, 75)
        'End If

        p1Font = New Font("Calibri", 18, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Name, LMargin, CurY, 2, PrintWidth, p1Font)
        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height

        CurY = CurY + strHeight - 1
        'Common_Procedures.Print_To_PrintDocument(e, Desc, LMargin, CurY, 2, PrintWidth, pFont)

        'CurY = CurY + TxtHgt - 1
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Add1, LMargin, CurY, 2, PrintWidth, pFont)

        CurY = CurY + TxtHgt - 1
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Add2, LMargin, CurY, 2, PrintWidth, pFont)
        'CurY = CurY + TxtHgt - 1
        'Common_Procedures.Print_To_PrintDocument(e, Cmp_PhNo, LMargin, CurY, 2, PrintWidth, pFont)
        'CurY = CurY + TxtHgt - 1
        'Common_Procedures.Print_To_PrintDocument(e, Cmp_TinNo, LMargin + 10, CurY, 0, 0, pFont)
        'Common_Procedures.Print_To_PrintDocument(e, Cmp_EMail, LMargin, CurY, 2, PrintWidth, pFont)
        'Common_Procedures.Print_To_PrintDocument(e, Cmp_CstNo, PageWidth - 10, CurY, 1, 0, pFont)

        CurY = CurY + TxtHgt + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(2) = CurY

        C1 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5)
        W1 = e.Graphics.MeasureString("INVOICE DATE  : ", pFont).Width
        S1 = e.Graphics.MeasureString("TO     :    ", pFont).Width
        W2 = e.Graphics.MeasureString("Despatch To   : ", pFont).Width
        S2 = e.Graphics.MeasureString("Sent Through  : ", pFont).Width


        CurY = CurY + 10
        p1Font = New Font("Calibri", 12, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "TO  :  " & "M/s." & prn_HdDt.Rows(0).Item("Ledger_Name").ToString, LMargin + 10, CurY, 0, 0, p1Font)
        Common_Procedures.Print_To_PrintDocument(e, "DELIVERY NO", LMargin + C1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("ClothProcess_Delivery_No").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, p1Font)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address1").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
        p1Font = New Font("Calibri", 14, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "DELIVERY DATE", LMargin + C1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("ClothProcess_Delivery_Date").ToString), "dd-MM-yyyy").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, pFont)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address2").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
        'Common_Procedures.Print_To_PrintDocument(e, "ITEM", LMargin + C1 + 10, CurY, 0, 0, pFont)
        'Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
        'Common_Procedures.Print_To_PrintDocument(e, prn_HdAr(prn_HdMxIndx, 2), LMargin + C1 + W1 + 30, CurY, 0, 0, p1Font)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address3").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)


        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address4").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
        'If Trim(prn_HdDt.Rows(0).Item("Dc_No").ToString) <> "" Then
        '    Common_Procedures.Print_To_PrintDocument(e, "DC NO : " & prn_HdDt.Rows(0).Item("Dc_No").ToString, LMargin + C1 + 10, CurY, 0, 0, pFont)
        'End If
        'If Trim(prn_HdDt.Rows(0).Item("Dc_Date").ToString) <> "" Then
        '    Common_Procedures.Print_To_PrintDocument(e, "DC DATE : " & prn_HdDt.Rows(0).Item("Dc_Date").ToString, LMargin + C1 + 100, CurY, 0, 0, pFont)
        'End If

        'CurY = CurY + TxtHgt
        'If Trim(prn_HdDt.Rows(0).Item("Ledger_TinNo").ToString) <> "" Then
        '    Common_Procedures.Print_To_PrintDocument(e, " TIN : " & prn_HdDt.Rows(0).Item("Ledger_TinNo").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
        'End If

        CurY = CurY + TxtHgt
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        e.Graphics.DrawLine(Pens.Black, LMargin + C1, CurY, LMargin + C1, LnAr(2))

        Try

            CurY = CurY + TxtHgt - 5
            Common_Procedures.Print_To_PrintDocument(e, "ITEM", LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdAr(prn_HdMxIndx, 6), LMargin + W1 + 25, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt + 10
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(2) = CurY

            CurY = CurY + TxtHgt - 10
            Common_Procedures.Print_To_PrintDocument(e, "S.No", LMargin, CurY, 2, ClAr(1), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "Bale No.", LMargin + ClAr(1), CurY, 2, ClAr(2), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "No Of", LMargin + ClAr(1) + ClAr(2), CurY, 2, ClAr(3), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "Pieces", LMargin + ClAr(1) + ClAr(2), CurY + TxtHgt, 2, ClAr(3), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "Bale Mtrs", LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, 2, ClAr(4), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "Nett Weight", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 2, ClAr(5), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "Individual Piece(s) Mtrs(Lot No,Pieces)", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, 2, ClAr(6), pFont)
            'Common_Procedures.Print_To_PrintDocument(e, "PCS-6", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, 2, ClAr(7), pFont)
            'Common_Procedures.Print_To_PrintDocument(e, "PCS-7", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, 2, ClAr(8), pFont)
            'Common_Procedures.Print_To_PrintDocument(e, "METERS", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, 2, ClAr(9), pFont)

            CurY = CurY + TxtHgt + 20
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(3) = CurY
            e.Graphics.DrawLine(Pens.Black, LMargin, LnAr(1), LMargin, CurY)
            e.Graphics.DrawLine(Pens.Black, PageWidth, LnAr(1), PageWidth, CurY)


        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub Printing_PackingSlip_Format3_PageFooter(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByRef prn_HdAr(,) As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single, ByVal NoofDets As Integer, ByVal prn_HdIndx As Integer, ByVal is_LastPage As Boolean)
        ' Dim I As Integer
        Dim p1Font As Font
        Dim W1 As Single = 0

        Try

            'For I = NoofDets + 1 To NoofItems_PerPage
            '    CurY = CurY + TxtHgt
            'Next

            ' W1 = e.Graphics.MeasureString("INVOICE DATE  : ", pFont).Width
            CurY = CurY + TxtHgt

            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(4) = CurY


            ' Common_Procedures.Print_To_PrintDocument(e, Trim(Val(prn_HdAr(prn_HdIndx, 3))), LMargin + ClAr(1) + 15, CurY, 0, 0, pFont)
            ' Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdAr(prn_HdIndx, 4)), "#########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) - 15, CurY, 1, 0, pFont)

            CurY = CurY + TxtHgt - 10
            Common_Procedures.Print_To_PrintDocument(e, "TOTAL", LMargin + ClAr(1), CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Format(Val(Total_PCS), "#########0"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) - 10, CurY, 1, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Format(Val(Total_mtrs), "#########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) - 2, CurY, 1, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Format(Val(Total_wEIGHT), "#########0.000"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 2, CurY, 1, 0, pFont)

            CurY = CurY + TxtHgt + 5
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(5) = CurY

            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, LMargin, LnAr(2))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1), CurY, LMargin + ClAr(1), LnAr(2))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2), CurY, LMargin + ClAr(1) + ClAr(2), LnAr(2))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3), LnAr(2))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), LnAr(2))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(2))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), LnAr(2))


            CurY = CurY + TxtHgt - 10
            Common_Procedures.Print_To_PrintDocument(e, "Above 80 Mtrs", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + 150, CurY, 1, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, " :", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + 150, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Format(Val(Total_Mtrs_Abv80), "#########0.00"), PageWidth - 5, CurY, 1, 0, pFont)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "40 To 79 Mtrs", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + 150, CurY, 1, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, " :", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + 150, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Format(Val(Total_Mtrs_40To79), "#########0.00"), PageWidth - 5, CurY, 1, 0, pFont)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "20 To 40 Mtrs", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + 150, CurY, 1, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, " :", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + 150, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Format(Val(Total_Mtrs_20To40), "#########0.00"), PageWidth - 5, CurY, 1, 0, pFont)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "Total Mtrs", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + 150, CurY, 1, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, " :", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + 150, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Format(Val(Total_mtrs), "#########0.00"), PageWidth - 5, CurY, 1, 0, pFont)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "No Of Bales", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + 150, CurY, 1, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, " :", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + 150, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdMxIndx), "#########0.00"), PageWidth - 5, CurY, 1, 0, pFont)

            CurY = CurY + TxtHgt
            CurY = CurY + TxtHgt

            p1Font = New Font("Calibri", 12, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "For " & Trim(prn_HdDt.Rows(0).Item("Company_Name").ToString), PageWidth - 5, CurY, 1, 0, p1Font)
            CurY = CurY + TxtHgt
            CurY = CurY + TxtHgt
            CurY = CurY + TxtHgt

            p1Font = New Font("Calibri", 12, FontStyle.Bold)

            Common_Procedures.Print_To_PrintDocument(e, "AUTHORISED SIGNATORY ", PageWidth - 5, CurY, 1, 0, pFont)
            CurY = CurY + TxtHgt + 10

            'e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            'e.Graphics.DrawLine(Pens.Black, LMargin, LnAr(1), LMargin, CurY)
            'e.Graphics.DrawLine(Pens.Black, PageWidth, LnAr(1), PageWidth, CurY)


        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub
    Private Sub txt_Note_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_Note.KeyDown
        If e.KeyCode = 40 Then
            If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                save_record()
            Else
                dtp_Date.Focus()
            End If
        End If
        If e.KeyCode = 38 Then SendKeys.Send("+{TAB}")
    End Sub

    Private Sub txt_Note_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Note.KeyPress
        If Asc(e.KeyChar) = 13 Then
            If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                save_record()
            Else
                dtp_Date.Focus()
            End If
        End If
    End Sub

    Private Sub cbo_TransportName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_TransportName.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'TRANSPORT')", "(Ledger_IdNo = 0)")

    End Sub


    Private Sub cbo_TransportName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_TransportName.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_TransportName, Nothing, cbo_VehicleNo, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'TRANSPORT')", "(Ledger_IdNo = 0)")
        If e.KeyValue = 38 Then
            If dgv_Details.Rows.Count > 0 Then
                dgv_Details.Focus()
                dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(dgvCol_Details.ITEM_GREY)
                dgv_Details.CurrentCell.Selected = True

            Else
                Cbo_ProcessingHEAD.Focus()

            End If
        End If

    End Sub

    Private Sub cbo_Transportname_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_TransportName.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_TransportName, cbo_VehicleNo, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'TRANSPORT')", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_TransportName_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_TransportName.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Common_Procedures.MDI_LedType = "TRANSPORT"
            Dim f As New Ledger_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_TransportName.Name
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

    Private Sub btn_Print_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Print.Click
        print_record()
    End Sub

    Private Sub Show_Item_CurrentStock(ByVal Rw As Integer)

        Dim vItemID As Integer

        Dim Del_Godown_Led_ID As Integer = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Delivery_From.Text)

        If Val(Rw) < 0 Then Exit Sub

        With dgv_Details

            vItemID = Common_Procedures.Processed_Item_NameToIdNo(con, .Rows(Rw).Cells(dgvCol_Details.ITEM_GREY).Value)

            If Val(vItemID) = 0 Then Exit Sub

            If Val(vItemID) <> Val(.Tag) Then
                Common_Procedures.Show_ProcessedItem_CurrentStock_Display(con, Val(lbl_Company.Tag), Val(Del_Godown_Led_ID), vItemID)
                .Tag = Val(Rw)
            End If

        End With


    End Sub

    Private Sub txt_Frieght_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Frieght.KeyPress

        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
            e.Handled = True
        End If

    End Sub

    Private Sub Cbo_ProcessingHEAD_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles Cbo_ProcessingHEAD.GotFocus

        Cbo_ProcessingHEAD.Tag = Cbo_ProcessingHEAD.Text

        If Common_Procedures.settings.CustomerCode = "1516" Then
            Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Process_Head", "Process_Name", "Cloth_Delivered=1", "(Process_Idno=0)")
        Else
            Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Process_Head", "Process_Name", "", "(Process_Idno=0)")
        End If

    End Sub

    Private Sub Cbo_ProcessingHEAD_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Cbo_ProcessingHEAD.KeyDown

        If Common_Procedures.settings.CustomerCode = "1516" Then
            Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, Cbo_ProcessingHEAD, cbo_Ledger, cbo_Delivery_From, "Process_Head", "Process_Name", "Cloth_Delivered=1", "(Process_Idno=0)")
        Else
            Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, Cbo_ProcessingHEAD, cbo_Ledger, cbo_Delivery_From, "Process_Head", "Process_Name", "", "(Process_Idno=0)")
        End If

    End Sub

    Private Sub Cbo_ProcessingHEAD_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Cbo_ProcessingHEAD.KeyPress

        If Common_Procedures.settings.CustomerCode = "1516" Then
            Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, Cbo_ProcessingHEAD, cbo_Delivery_From, "Process_Head", "Process_Name", "Cloth_Delivered=1", "(Process_Idno=0)")
        Else
            Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, Cbo_ProcessingHEAD, cbo_Delivery_From, "Process_Head", "Process_Name", "", "(Process_Idno=0)")
        End If

    End Sub

    Private Sub Cbo_ProcessingHEAD_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Cbo_ProcessingHEAD.KeyUp

        If e.Control = False And e.KeyValue = 17 Then

            Dim f As New Process_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = Cbo_ProcessingHEAD.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If

    End Sub
    Private Sub btn_BaleSelection_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_BaleSelection.Click
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim i As Integer, j As Integer, n As Integer, SNo As Integer
        Dim Clo_ID As Integer
        Dim NewCode As String
        '  Dim Fd_Perc As Integer
        Dim CompIDCondt As String
        Dim dgvDet_CurRow As Integer
        Dim dgv_DetSlNo As Long
        Dim vCLOTH_STOCKMAINTENANCE_IN As String = ""

        Try

            'If Trim(UCase(cbo_Type.Text)) = "DELIVERY" Then
            '    Exit Sub
            'End If

            If dgv_Details.CurrentCell.RowIndex < 0 Then
                MessageBox.Show("Invalid Cloth Name & Type Selection", "DOES NOT SELECT BALE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                If dgv_Details.Enabled And dgv_Details.Visible Then
                    If dgv_Details.Rows.Count > 0 Then
                        dgv_Details.Focus()
                        dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(dgvCol_Details.LOT_NO)
                        dgv_Details.CurrentCell.Selected = True
                    End If
                End If
                Exit Sub
            End If

            Clo_ID = Common_Procedures.Cloth_NameToIdNo(con, dgv_Details.Rows(dgv_Details.CurrentCell.RowIndex).Cells(dgvCol_Details.ITEM_GREY).Value)
            If Clo_ID = 0 Then
                MessageBox.Show("Invalid Cloth Name", "DOES NOT SELECT BALE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                If dgv_Details.Enabled And dgv_Details.Visible Then
                    If dgv_Details.Rows.Count > 0 Then
                        dgv_Details.Focus()
                        dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(dgvCol_Details.ITEM_GREY)
                        If cbo_itemgrey.Visible And cbo_itemgrey.Enabled Then cbo_itemgrey.Focus()
                        'dgv_Details.CurrentCell.Selected = True
                        Exit Sub
                    End If
                End If
                Exit Sub
            End If

            'CloType_ID = Common_Procedures.ClothType_NameToIdNo(con, dgv_Details.Rows(dgv_Details.CurrentCell.RowIndex).Cells(2).Value)
            'If CloType_ID = 0 Then
            '    MessageBox.Show("Invalid Cloth Type ", "DOES NOT SELECT BALE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            '    If dgv_Details.Enabled And dgv_Details.Visible Then
            '        If dgv_Details.Rows.Count > 0 Then
            '            dgv_Details.Focus()
            '            dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(2)
            '            If cbo_Grid_Clothtype.Visible And cbo_Grid_Clothtype.Enabled Then cbo_Grid_Clothtype.Focus()
            '            Exit Sub
            '        End If
            '    End If
            '    Exit Sub
            'End If

            'Fd_Perc = Val(dgv_Details.Rows(dgv_Details.CurrentCell.RowIndex).Cells(3).Value)
            'If Val(Fd_Perc) = 0 Then
            '    MessageBox.Show("Invalid Folding", "DOES NOT SELECT BALE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            '    If dgv_Details.Enabled And dgv_Details.Visible Then
            '        If dgv_Details.Rows.Count > 0 Then
            '            dgv_Details.Focus()
            '            dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(3)
            '            dgv_Details.CurrentCell.Selected = True
            '        End If
            '    End If
            '    Exit Sub
            'End If

            CompIDCondt = "(a.company_idno = " & Str(Val(lbl_Company.Tag)) & ")"
            If Trim(UCase(Common_Procedures.settings.CompanyName)) = "-----~~~" Then
                CompIDCondt = ""
            End If

            NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(EntFnYrCode)

            dgvDet_CurRow = dgv_Details.CurrentCell.RowIndex
            dgv_DetSlNo = Val(dgv_Details.Rows(dgvDet_CurRow).Cells(dgvCol_Details.CLOTH_PROCESSING_DELIVERY_PACKINGSLIP_SLNO).Value)

            With dgv_BaleSelection
                chk_SelectAll.Checked = False
                .Rows.Clear()
                SNo = 0

                Da = New SqlClient.SqlDataAdapter("Select a.* from Packing_Slip_Head a where " & CompIDCondt & IIf(Trim(CompIDCondt) <> "", " and ", "") & " a.Delivery_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' and a.Delivery_DetailsSlNo = " & Str(Val(dgv_DetSlNo)) & " and a.Cloth_IdNo = " & Str(Val(Clo_ID)) & " order by a.Packing_Slip_Date, a.for_orderby, a.Packing_Slip_No, a.Packing_Slip_Code", con)
                Dt1 = New DataTable
                Da.Fill(Dt1)

                If Dt1.Rows.Count > 0 Then

                    For i = 0 To Dt1.Rows.Count - 1

                        n = .Rows.Add()

                        SNo = SNo + 1
                        .Rows(n).Cells(dgvCol_BaleSelection.SLNO).Value = Val(SNo)
                        .Rows(n).Cells(dgvCol_BaleSelection.BALE_NO).Value = Dt1.Rows(i).Item("Packing_Slip_No").ToString



                        Check_Cloth_Stock_Maintanance_In_Pcs_Mtr(vCLOTH_STOCKMAINTENANCE_IN)

                        If Trim(UCase(vCLOTH_STOCKMAINTENANCE_IN)) = "PCS" Then

                            .Rows(n).Cells(dgvCol_BaleSelection.PCS).Value = Format(Val(Dt1.Rows(i).Item("Total_Meters").ToString), "#########0.00")

                            .Rows(n).Cells(dgvCol_BaleSelection.METERS).Value = 0


                        Else

                            If Val(Dt1.Rows(i).Item("Total_Pcs").ToString) <> 0 Then
                                .Rows(n).Cells(dgvCol_BaleSelection.PCS).Value = Val(Dt1.Rows(i).Item("Total_Pcs").ToString)
                            End If

                            If Val(Dt1.Rows(i).Item("Total_Meters").ToString) <> 0 Then
                                .Rows(n).Cells(dgvCol_BaleSelection.METERS).Value = Format(Val(Dt1.Rows(i).Item("Total_Meters").ToString), "#########0.00")
                            End If

                        End If

                        If Val(Dt1.Rows(i).Item("Total_Weight").ToString) <> 0 Then
                            .Rows(n).Cells(dgvCol_BaleSelection.WEIGHT).Value = Format(Val(Dt1.Rows(i).Item("Total_Weight").ToString), "#########0.000")
                        End If
                        .Rows(n).Cells(dgvCol_BaleSelection.STS).Value = "1"
                        .Rows(n).Cells(dgvCol_BaleSelection.PACKING_SLIP_CODE).Value = Dt1.Rows(i).Item("Packing_Slip_Code").ToString
                        .Rows(n).Cells(dgvCol_BaleSelection.BALE_BUNDLE).Value = Dt1.Rows(i).Item("Bale_Bundle").ToString

                        For j = 0 To .ColumnCount - 1
                            .Rows(i).Cells(j).Style.ForeColor = Color.Red
                        Next

                    Next

                End If
                Dt1.Clear()

                Da = New SqlClient.SqlDataAdapter("select a.* from Packing_Slip_Head a where " & CompIDCondt & IIf(Trim(CompIDCondt) <> "", " and ", "") & " a.Delivery_Code = '' and a.Cloth_IdNo = " & Str(Val(Clo_ID)) & " order by a.Packing_Slip_Date, a.for_orderby, a.Packing_Slip_No, a.Packing_Slip_Code", con)
                Dt1 = New DataTable
                Da.Fill(Dt1)

                If Dt1.Rows.Count > 0 Then

                    For i = 0 To Dt1.Rows.Count - 1

                        n = .Rows.Add()

                        SNo = SNo + 1
                        .Rows(n).Cells(dgvCol_BaleSelection.SLNO).Value = Val(SNo)
                        .Rows(n).Cells(dgvCol_BaleSelection.BALE_NO).Value = Dt1.Rows(i).Item("Packing_Slip_No").ToString

                        ' If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1558" Then ' --- sotexpa 

                        Check_Cloth_Stock_Maintanance_In_Pcs_Mtr(vCLOTH_STOCKMAINTENANCE_IN)

                            If Trim(UCase(vCLOTH_STOCKMAINTENANCE_IN)) = "PCS" Then

                            If Val(Dt1.Rows(i).Item("Total_Meters").ToString) <> 0 Then
                                .Rows(n).Cells(dgvCol_BaleSelection.PCS).Value = Format(Val(Dt1.Rows(i).Item("Total_Meters").ToString), "#########0.00")
                            End If

                            .Rows(n).Cells(dgvCol_BaleSelection.METERS).Value = 0
                                'Else
                                '    GoTo LOOP2
                                'End If

                            Else
                                ''LOOP2:
                                If Val(Dt1.Rows(i).Item("Total_Pcs").ToString) <> 0 Then
                                .Rows(n).Cells(dgvCol_BaleSelection.PCS).Value = Val(Dt1.Rows(i).Item("Total_Pcs").ToString)
                            End If

                            If Val(Dt1.Rows(i).Item("Total_Meters").ToString) <> 0 Then
                                .Rows(n).Cells(dgvCol_BaleSelection.METERS).Value = Format(Val(Dt1.Rows(i).Item("Total_Meters").ToString), "#########0.00")
                            End If

                        End If
                        If Val(Dt1.Rows(i).Item("Total_Weight").ToString) <> 0 Then
                            .Rows(n).Cells(dgvCol_BaleSelection.WEIGHT).Value = Format(Val(Dt1.Rows(i).Item("Total_Weight").ToString), "#########0.000")
                        End If
                        .Rows(n).Cells(dgvCol_BaleSelection.STS).Value = ""
                        .Rows(n).Cells(dgvCol_BaleSelection.PACKING_SLIP_CODE).Value = Dt1.Rows(i).Item("Packing_Slip_Code").ToString
                        .Rows(n).Cells(dgvCol_BaleSelection.BALE_BUNDLE).Value = Dt1.Rows(i).Item("Bale_Bundle").ToString

                    Next

                End If
                Dt1.Clear()


            End With

            pnl_BaleSelection.Visible = True
            pnl_Back.Enabled = False
            dgv_BaleSelection.Focus()
            If dgv_BaleSelection.Rows.Count > 0 Then
                dgv_BaleSelection.CurrentCell = dgv_BaleSelection.Rows(0).Cells(dgvCol_BaleSelection.SLNO)
                dgv_BaleSelection.CurrentCell.Selected = True
            End If

        Catch ex As NullReferenceException
            MessageBox.Show("Select the ClothName for Bale Selection", "DOES NOT SELECT BALE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT SELECT BALE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try



    End Sub

    Private Sub dgv_BaleSelection_CellClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_BaleSelection.CellClick
        Select_Bale(e.RowIndex)
    End Sub

    Private Sub Select_Bale(ByVal RwIndx As Integer)
        Dim i As Integer

        With dgv_BaleSelection

            If .RowCount > 0 And RwIndx >= 0 Then

                .Rows(RwIndx).Cells(dgvCol_BaleSelection.STS).Value = (Val(.Rows(RwIndx).Cells(dgvCol_BaleSelection.STS).Value) + 1) Mod 2

                If Val(.Rows(RwIndx).Cells(dgvCol_BaleSelection.STS).Value) = 0 Then .Rows(RwIndx).Cells(dgvCol_BaleSelection.STS).Value = ""

                For i = 0 To .ColumnCount - 1
                    .Rows(RwIndx).Cells(i).Style.ForeColor = Color.Red
                Next

            End If

        End With

    End Sub

    Private Sub dgv_BaleSelection_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_BaleSelection.KeyDown
        On Error Resume Next

        If e.KeyCode = Keys.Enter Or e.KeyCode = Keys.Space Then
            If dgv_BaleSelection.CurrentCell.RowIndex >= 0 Then
                Select_Bale(dgv_BaleSelection.CurrentCell.RowIndex)
                e.Handled = True
            End If
        End If

        If e.KeyCode = Keys.Delete Or e.KeyCode = Keys.Back Then
            If dgv_BaleSelection.CurrentCell.RowIndex >= 0 Then
                If Val(dgv_BaleSelection.Rows(dgv_BaleSelection.CurrentCell.RowIndex).Cells(dgvCol_BaleSelection.STS).Value) = 1 Then
                    e.Handled = True
                    Select_Bale(dgv_BaleSelection.CurrentCell.RowIndex)
                End If
            End If
        End If

    End Sub

    Private Sub btn_Close_BaleSelection_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Close_BaleSelection.Click
        Dim Cmd As New SqlClient.SqlCommand
        Dim Da1 As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim Dt2 As New DataTable

        Dim I As Integer, J As Integer
        Dim n As Integer
        Dim sno As Integer
        Dim dgvDet_CurRow As Integer = 0
        Dim dgv_DetSlNo As Integer = 0
        Dim NoofBls As Integer
        Dim FsNo As Single, LsNo As Single
        Dim FsBaleNo As String, LsBaleNo As String
        Dim BlNo As String, PackSlpCodes As String
        Dim Tot_Pcs As String, Tot_Mtrs As Single, Tot_WGT As Single
        Dim vCLOTH_STOCKMAINTENANCE_IN As String
        Dim Cloth_Nm As String
        Cmd.Connection = con

        dgvDet_CurRow = dgv_Details.CurrentCell.RowIndex
        dgv_DetSlNo = Val(dgv_Details.Rows(dgvDet_CurRow).Cells(dgvCol_Details.CLOTH_PROCESSING_DELIVERY_PACKINGSLIP_SLNO).Value)

        With dgv_BaleSelectionDetails

LOOP1:
            For I = 0 To .RowCount - 1

                If Val(.Rows(I).Cells(dgvCol_BaleSeledetails.CLOTH_PROCESSING_DELIVERY_PACKINGSLIP_SLNO).Value) = Val(dgv_DetSlNo) And Val(.Rows(I).Cells(dgvCol_BaleSeledetails.METERS).Value) <> 0 And Trim(.Rows(I).Cells(dgvCol_BaleSeledetails.PACKING_SLIP_CODE).Value) <> "" Then

                    If I = .Rows.Count - 1 Then
                        For J = 0 To .ColumnCount - 1
                            .Rows(I).Cells(J).Value = ""
                        Next

                    Else
                        .Rows.RemoveAt(I)

                    End If

                    GoTo LOOP1

                End If

            Next I

            Cmd.CommandText = "truncate table " & Trim(Common_Procedures.EntryTempTable) & ""
            Cmd.ExecuteNonQuery()

            NoofBls = 0 : Tot_Pcs = 0 : Tot_Mtrs = 0 : BlNo = "" : PackSlpCodes = "" : Tot_WGT = 0

            For I = 0 To dgv_BaleSelection.RowCount - 1

                If Val(dgv_BaleSelection.Rows(I).Cells(dgvCol_BaleSelection.STS).Value) = 1 Then

                    n = .Rows.Add()

                    sno = sno + 1
                    .Rows(n).Cells(dgvCol_BaleSeledetails.CLOTH_PROCESSING_DELIVERY_PACKINGSLIP_SLNO).Value = Val(dgv_DetSlNo)
                    .Rows(n).Cells(dgvCol_BaleSeledetails.BALE_NO).Value = dgv_BaleSelection.Rows(I).Cells(dgvCol_BaleSelection.BALE_NO).Value

                    .Rows(n).Cells(dgvCol_BaleSeledetails.PCS).Value = Val(dgv_BaleSelection.Rows(I).Cells(dgvCol_BaleSelection.PCS).Value)
                    .Rows(n).Cells(dgvCol_BaleSeledetails.METERS).Value = Format(Val(dgv_BaleSelection.Rows(I).Cells(dgvCol_BaleSelection.METERS).Value), "#########0.00")

                    .Rows(n).Cells(dgvCol_BaleSeledetails.WEIGHT).Value = Format(Val(dgv_BaleSelection.Rows(I).Cells(dgvCol_BaleSelection.WEIGHT).Value), "#########0.000")
                    .Rows(n).Cells(dgvCol_BaleSeledetails.PACKING_SLIP_CODE).Value = dgv_BaleSelection.Rows(I).Cells(dgvCol_BaleSelection.PACKING_SLIP_CODE).Value
                    .Rows(n).Cells(dgvCol_BaleSeledetails.BALE_BUNDLE).Value = dgv_BaleSelection.Rows(I).Cells(dgvCol_BaleSelection.BALE_BUNDLE).Value



                    Cmd.CommandText = "Insert into " & Trim(Common_Procedures.EntryTempTable) & "(Name1, Name2, Meters1) values ('" & Trim(dgv_BaleSelection.Rows(I).Cells(dgvCol_BaleSelection.PACKING_SLIP_CODE).Value) & "', '" & Trim(dgv_BaleSelection.Rows(I).Cells(dgvCol_BaleSelection.BALE_NO).Value) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(dgv_BaleSelection.Rows(I).Cells(dgvCol_BaleSelection.BALE_NO).Value))) & " ) "
                    Cmd.ExecuteNonQuery()

                    NoofBls = NoofBls + 1
                    Tot_Pcs = Val(Tot_Pcs) + Val(dgv_BaleSelection.Rows(I).Cells(dgvCol_BaleSelection.PCS).Value)
                    Tot_Mtrs = Val(Tot_Mtrs) + Val(dgv_BaleSelection.Rows(I).Cells(dgvCol_BaleSelection.METERS).Value)
                    Tot_WGT = Val(Tot_WGT) + Val(dgv_BaleSelection.Rows(I).Cells(dgvCol_BaleSelection.WEIGHT).Value)
                    PackSlpCodes = Trim(PackSlpCodes) & IIf(Trim(PackSlpCodes) = "", "~", "") & Trim(dgv_BaleSelection.Rows(I).Cells(dgvCol_BaleSelection.PACKING_SLIP_CODE).Value) & "~"

                End If

            Next



            BlNo = ""
            FsNo = 0 : LsNo = 0
            FsBaleNo = "" : LsBaleNo = ""

            Da1 = New SqlClient.SqlDataAdapter("Select Name1 as Bale_Code, Name2 as Bale_No, Meters1 as fororderby_baleno from " & Trim(Common_Procedures.EntryTempTable) & " where Name1 <> '' order by Meters1, Name2, Name1", con)
            Dt1 = New DataTable
            Da1.Fill(Dt1)

            If Dt1.Rows.Count > 0 Then

                FsNo = Val(Dt1.Rows(0).Item("fororderby_baleno").ToString)
                LsNo = Val(Dt1.Rows(0).Item("fororderby_baleno").ToString)

                FsBaleNo = Trim(UCase(Dt1.Rows(0).Item("Bale_No").ToString))
                LsBaleNo = Trim(UCase(Dt1.Rows(0).Item("Bale_No").ToString))

                For I = 1 To Dt1.Rows.Count - 1
                    If LsNo + 1 = Val(Dt1.Rows(I).Item("fororderby_baleno").ToString) Then
                        LsNo = Val(Dt1.Rows(I).Item("fororderby_baleno").ToString)
                        LsBaleNo = Trim(UCase(Dt1.Rows(I).Item("Bale_No").ToString))

                    Else
                        If FsNo = LsNo Then
                            BlNo = BlNo & Trim(FsBaleNo) & ","
                        Else
                            BlNo = BlNo & Trim(FsBaleNo) & "-" & Trim(LsBaleNo) & ","
                        End If
                        FsNo = Dt1.Rows(I).Item("fororderby_baleno").ToString
                        LsNo = Dt1.Rows(I).Item("fororderby_baleno").ToString

                        FsBaleNo = Trim(UCase(Dt1.Rows(I).Item("Bale_No").ToString))
                        LsBaleNo = Trim(UCase(Dt1.Rows(I).Item("Bale_No").ToString))

                    End If

                Next

                If FsNo = LsNo Then BlNo = BlNo & Trim(FsBaleNo) Else BlNo = BlNo & Trim(FsBaleNo) & "-" & Trim(LsBaleNo)

            End If
            Dt1.Clear()

            If Trim(dgv_Details.Rows(dgvDet_CurRow).Cells(dgvCol_Details.PACKINGSLIP_CODE).Value) <> "" Then
                dgv_Details.Rows(dgvDet_CurRow).Cells(dgvCol_Details.BALES).Value = ""
                dgv_Details.Rows(dgvDet_CurRow).Cells(dgvCol_Details.BALES_NOS).Value = ""
                dgv_Details.Rows(dgvDet_CurRow).Cells(dgvCol_Details.PCS).Value = ""
                dgv_Details.Rows(dgvDet_CurRow).Cells(dgvCol_Details.QTY).Value = ""
                dgv_Details.Rows(dgvDet_CurRow).Cells(dgvCol_Details.PACKINGSLIP_CODE).Value = ""
            End If
            If Val(NoofBls) <> 0 And (Val(Tot_Mtrs) <> 0 Or Val(Tot_Pcs) <> 0) Then
                'If Val(NoofBls) <> 0 And Val(Tot_Mtrs) <> 0 Then
                dgv_Details.Rows(dgvDet_CurRow).Cells(dgvCol_Details.BALES).Value = NoofBls
                    dgv_Details.Rows(dgvDet_CurRow).Cells(dgvCol_Details.BALES_NOS).Value = BlNo

                    If Val(Tot_Pcs) <> 0 Then
                    dgv_Details.Rows(dgvDet_CurRow).Cells(dgvCol_Details.PCS).Value = Format(Val(Tot_Pcs), "#########0.00")
                End If

                    dgv_Details.Rows(dgvDet_CurRow).Cells(dgvCol_Details.METERS).Value = Format(Val(Tot_Mtrs), "#########0.00")
                End If

                dgv_Details.Rows(dgvDet_CurRow).Cells(dgvCol_Details.WEIGHT).Value = Format(Val(Tot_WGT), "#########0.000")
                dgv_Details.Rows(dgvDet_CurRow).Cells(dgvCol_Details.PACKINGSLIP_CODE).Value = PackSlpCodes


            ' Amount_Calculation(dgvDet_CurRow, 7)

            'Add_NewRow_ToGrid()

            Total_Calculation()

        End With

        pnl_Back.Enabled = True
        pnl_BaleSelection.Visible = False
        If dgv_Details.Enabled And dgv_Details.Visible Then
            If dgv_Details.Rows.Count > 0 Then
                dgv_Details.Focus()
                If dgv_Details.CurrentCell.RowIndex >= 0 Then
                    dgv_Details.CurrentCell = dgv_Details.Rows(dgv_Details.CurrentCell.RowIndex).Cells(dgvCol_Details.PCS)
                    dgv_Details.CurrentCell.Selected = True
                End If
            End If
        End If

    End Sub

    Private Sub Add_NewRow_ToGrid()
        On Error Resume Next

        Dim i As Integer
        Dim n As Integer = -1

        With dgv_Details
            If .Visible Then

                If .CurrentCell.RowIndex = .Rows.Count - 1 Then

                    n = .Rows.Add()

                    For i = 0 To .Columns.Count - 1
                        .Rows(n).Cells(i).Value = .Rows(.CurrentCell.RowIndex).Cells(i).Value
                        .Rows(.CurrentCell.RowIndex).Cells(i).Value = ""
                    Next

                    For i = 0 To .Rows.Count - 1
                        .Rows(i).Cells(dgvCol_Details.SLNO).Value = i + 1
                    Next

                    .CurrentCell = .Rows(n).Cells(.CurrentCell.ColumnIndex)
                    .CurrentCell.Selected = True


                End If

            End If

        End With

    End Sub
    Private Sub txt_BaleSelction_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_BaleSelction.KeyDown
        If e.KeyValue = 40 Then
            If dgv_BaleSelection.Rows.Count > 0 Then
                dgv_BaleSelection.Focus()
                dgv_BaleSelection.CurrentCell = dgv_BaleSelection.Rows(0).Cells(dgvCol_BaleSelection.SLNO)
                dgv_BaleSelection.CurrentCell.Selected = True
            Else
                btn_lot_Pcs_selection.Focus()
            End If
        End If
    End Sub

    Private Sub txt_BaleSelction_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_BaleSelction.KeyPress
        If Asc(e.KeyChar) = 13 Then

            If Trim(txt_BaleSelction.Text) <> "" Then
                btn_lot_Pcs_selection_Click(sender, e)

            Else
                If dgv_BaleSelection.Rows.Count > 0 Then
                    dgv_BaleSelection.Focus()
                    dgv_BaleSelection.CurrentCell = dgv_BaleSelection.Rows(0).Cells(dgvCol_BaleSelection.SLNO)
                    dgv_BaleSelection.CurrentCell.Selected = True
                End If

            End If

        End If
    End Sub

    Private Sub btn_lot_Pcs_selection_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_lot_Pcs_selection.Click
        Dim LtNo As String
        Dim i As Integer

        If Trim(txt_BaleSelction.Text) <> "" Then

            LtNo = Trim(txt_BaleSelction.Text)

            For i = 0 To dgv_BaleSelection.Rows.Count - 1
                If Trim(UCase(LtNo)) = Trim(UCase(dgv_BaleSelection.Rows(i).Cells(dgvCol_BaleSelection.BALE_NO).Value)) Then
                    Call Select_Bale(i)
                    dgv_BaleSelection.CurrentCell = dgv_BaleSelection.Rows(i).Cells(dgvCol_BaleSelection.SLNO)
                    If i >= 9 Then dgv_BaleSelection.FirstDisplayedScrollingRowIndex = i - 8
                    Exit For
                End If
            Next

            txt_BaleSelction.Text = ""
            If txt_BaleSelction.Enabled = True Then txt_BaleSelction.Focus()

        End If

    End Sub

    Private Sub chk_SelectAll_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chk_SelectAll.CheckedChanged
        Dim i As Integer
        Dim J As Integer

        With dgv_BaleSelection

            For i = 0 To .Rows.Count - 1
                .Rows(i).Cells(dgvCol_BaleSelection.STS).Value = ""
                For J = 0 To .ColumnCount - 1
                    .Rows(i).Cells(J).Style.ForeColor = Color.Black
                Next J
            Next i

            If chk_SelectAll.Checked = True Then
                For i = 0 To .Rows.Count - 1
                    Select_Bale(i)
                Next i
            End If

            If .Rows.Count > 0 Then
                .Focus()
                .CurrentCell = .Rows(0).Cells(dgvCol_BaleSelection.SLNO)
                .CurrentCell.Selected = True
            End If

        End With

    End Sub

    Private Sub dgtxt_details_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgtxt_details.KeyUp

        Try
            With dgv_Details
                If .Rows.Count > 0 Then
                    If e.Control = True And UCase(Chr(e.KeyCode)) = "D" Then
                        dgv_Details_KeyUp(sender, e)
                    End If

                    If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
                        If .CurrentCell.ColumnIndex = dgvCol_Details.PCS Then
                            btn_PieceSelection_Click(sender, e)
                        ElseIf (.CurrentCell.ColumnIndex = dgvCol_Details.BALES Or .CurrentCell.ColumnIndex = dgvCol_Details.BALES_NOS) Then
                            btn_BaleSelection_Click(sender, e)
                        End If
                    End If
                End If
            End With

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR DGTXT KEYUP...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub CBO_JobNO_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles CBO_JobNO.GotFocus

        If Common_Procedures.settings.Continuous_Fabric_Lot_No_for_Purchase_Weaver = 1 Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1490" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1464" Then
            'Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "FabricPurchase_Weaver_Lot_Head", "FabricPurchase_Weaver_Lot_Code_forSelection", "", "")
            Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Lot_Head", "Lot_No", "", "")
        Else
            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1061" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1558" Then
                Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Textile_Processing_Delivery_Head", "JobOrder_no", "", "")
            Else
                Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Textile_Processing_JobOrder_Head", "Job_Order_SelectionCode", "", "")
            End If
        End If

    End Sub

    Private Sub CBO_JobNO_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles CBO_JobNO.KeyDown

        vcbo_KeyDwnVal = e.KeyValue

        'If Common_Procedures.settings.Continuous_Fabric_Lot_No_for_Purchase_Weaver = 1 Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1490" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1464" Then
        '    Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, CBO_JobNO, dtp_Date, cbo_Ledger, "Lot_Head", "Lot_No", "", "")
        'Else
        '    If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1061" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1558" Then
        '        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, CBO_JobNO, dtp_Date, cbo_Ledger, "Textile_Processing_Delivery_Head", "JobOrder_no", "", "")
        '    Else
        '        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, CBO_JobNO, dtp_Date, cbo_Ledger, "Textile_Processing_JobOrder_Head", "Job_Order_SelectionCode", "", "")
        '    End If
        'End If

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1061" Then  ' --- prakash cottex
            Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, CBO_JobNO, dtp_Date, cbo_Ledger, "Textile_Processing_Delivery_Head", "JobOrder_no", "", "")
        Else
            Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, CBO_JobNO, dtp_Date, cbo_Ledger, "Lot_Head", "Lot_No", "", "")
        End If

    End Sub

    Private Sub CBO_JobNO_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles CBO_JobNO.KeyPress

        'If Common_Procedures.settings.Continuous_Fabric_Lot_No_for_Purchase_Weaver = 1 Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1490" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1464" Then
        '    Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, CBO_JobNO, cbo_Ledger, "Lot_Head", "Lot_No", "", "")
        'Else
        '    If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1061" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1558" Then
        '        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, CBO_JobNO, cbo_Ledger, "Textile_Processing_Delivery_Head", "JobOrder_no", "", "", False)
        '    Else
        '        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, CBO_JobNO, cbo_Ledger, "Textile_Processing_JobOrder_Head", "Job_Order_SelectionCode", "", "", False)
        '    End If
        'End If

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1061" Then  ' --- prakash cottex

            Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, CBO_JobNO, cbo_Ledger, "Textile_Processing_Delivery_Head", "JobOrder_no", "", "", False)

        Else

            Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, CBO_JobNO, cbo_Ledger, "Lot_Head", "Lot_No", "", "")

        End If

    End Sub

    Private Sub CBO_JobNO_KeyUp(sender As Object, e As KeyEventArgs) Handles CBO_JobNO.KeyUp

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1061" Then  ' --- prakash cottex


            If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
                Dim f As New LotNo_creation

                Common_Procedures.Master_Return.Form_Name = Me.Name
                Common_Procedures.Master_Return.Control_Name = cbo_LotNo.Name
                Common_Procedures.Master_Return.Return_Value = ""
                Common_Procedures.Master_Return.Master_Type = ""

                f.MdiParent = MDIParent1
                f.Show()

            End If

        End If

    End Sub

    Private Sub btn_Print_packinglist_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Print_packinglist.Click
        Printing_Bale()
        btn_print_Close_Click(sender, e)
    End Sub

    Private Sub btn_Print_delivery_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Print_delivery.Click
        print_Selection()
        btn_print_Close_Click(sender, e)
    End Sub

    Private Sub btn_Print_Cancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Print_Cancel.Click
        btn_print_Close_Click(sender, e)
    End Sub

    Private Sub btn_print_Close_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Close_Print.Click
        pnl_Back.Enabled = True
        pnl_Print.Visible = False
    End Sub

    Private Sub btn_SMS_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_SendSMS.Click
        Dim i As Integer = 0
        Dim smstxt As String = ""
        Dim PhNo As String = "", AgPNo As String = ""
        Dim Led_IdNo As Integer = 0, Agnt_IdNo As Integer = 0
        Dim SMS_SenderID As String = ""
        Dim SMS_Key As String = ""
        Dim SMS_RouteID As String = ""
        Dim SMS_Type As String = ""
        Dim BlNos As String = ""

        Try

            Led_IdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Ledger.Text)

            PhNo = Common_Procedures.get_FieldValue(con, "Ledger_Head", "MobileNo_Frsms", "(Ledger_IdNo = " & Str(Val(Led_IdNo)) & ")")

            'Agnt_IdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Agent.Text)
            'AgPNo = ""
            'If Val(Agnt_IdNo) <> 0 Then
            '    AgPNo = Common_Procedures.get_FieldValue(con, "Ledger_Head", "Ledger_PhoneNo", "(Ledger_IdNo = " & Str(Val(Agnt_IdNo)) & ")")
            'End If

            If Trim(AgPNo) <> "" Then
                PhNo = Trim(PhNo) & IIf(Trim(PhNo) <> "", ",", "") & Trim(AgPNo)
            End If

            smstxt = Trim(cbo_Ledger.Text) & Chr(13)
            smstxt = smstxt & " Dc.No : " & Trim(lbl_DcNo.Text) & Chr(13)
            smstxt = smstxt & " Date : " & Trim(dtp_Date.Text) & Chr(13)
            If Trim(cbo_TransportName.Text) <> "" Then
                smstxt = smstxt & " Transport : " & Trim(cbo_TransportName.Text) & Chr(13)
            End If
            'If Trim(txt_LNo.Text) <> "" Then
            '    smstxt = smstxt & " Lr No : " & Trim(txt_LrNo.Text) & Chr(13)
            '    If Trim(msk_Lr_Date.Text) <> "" Then
            '        smstxt = smstxt & " Dt : " & Trim(msk_Lr_Date.Text) & Chr(13)
            '    End If
            'End If
            'If Trim(cbo_DespTo.Text) <> "" Then
            '    smstxt = smstxt & " Despatch To : " & Trim(cbo_DespTo.Text) & Chr(13)
            'End If
            If dgv_Details_Total.RowCount > 0 Then
                smstxt = smstxt & " No.Of Bales : " & Val((dgv_Details_Total.Rows(0).Cells(dgvCol_Details.BALES).Value())) & Chr(13)
                BlNos = ""
                For i = 0 To dgv_Details.Rows.Count - 1
                    If Val(dgv_Details_Total.Rows(0).Cells(dgvCol_Details.QTY).Value()) <> 0 Then
                        BlNos = BlNos & IIf(Trim(BlNos) <> "", ", ", "") & Trim(dgv_Details.Rows(0).Cells(dgvCol_Details.BALES_NOS).Value)
                    End If
                Next
                smstxt = smstxt & " Bales No.s : " & Trim(BlNos) & Chr(13)
                smstxt = smstxt & " Pcs : " & Val(dgv_Details_Total.Rows(0).Cells(dgvCol_Details.BALES).Value()) & Chr(13)
                smstxt = smstxt & " Meters : " & Val(dgv_Details_Total.Rows(0).Cells(dgvCol_Details.QTY).Value()) & Chr(13)
            End If
            'If dgv_Details.RowCount > 0 Then
            '    smstxt = smstxt & " No.Of Bales : " & Val((dgv_Details.Rows(0).Cells(4).Value())) & Chr(13)
            '    smstxt = smstxt & " Meters : " & Val((dgv_Details.Rows(0).Cells(7).Value())) & Chr(13)
            'End If
            'smstxt = smstxt & " Bill Amount : " & Trim(lbl_NetAmt.Text) & Chr(13)
            'smstxt = smstxt & " " & Chr(13)
            smstxt = smstxt & " Thanks! " & Chr(13)
            smstxt = smstxt & Common_Procedures.Company_IdNoToName(con, Val(lbl_Company.Tag))

            SMS_SenderID = ""
            SMS_Key = ""
            SMS_RouteID = ""
            SMS_Type = ""

            Common_Procedures.get_SMS_Provider_Details(con, Val(lbl_Company.Tag), SMS_SenderID, SMS_Key, SMS_RouteID, SMS_Type)


            Sms_Entry.vSmsPhoneNo = Trim(PhNo)
            Sms_Entry.vSmsMessage = Trim(smstxt)

            Sms_Entry.SMSProvider_SenderID = SMS_SenderID
            Sms_Entry.SMSProvider_Key = SMS_Key
            Sms_Entry.SMSProvider_RouteID = SMS_RouteID
            Sms_Entry.SMSProvider_Type = SMS_Type

            Dim f1 As New Sms_Entry
            f1.MdiParent = MDIParent1
            f1.Show()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT SEND SMS...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try
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

        LastNo = lbl_DcNo.Text

        movefirst_record()
        Timer1.Enabled = True

    End Sub

    Private Sub Timer1_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer1.Tick
        save_record()
        If Trim(UCase(LastNo)) = Trim(UCase(lbl_DcNo.Text)) Then
            Timer1.Enabled = False
            SaveAll_STS = False
            MessageBox.Show("All entries saved Successfully", "FOR SAVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Information)

        Else
            movenext_record()

        End If
    End Sub
    Private Sub cbo_Vechile_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_VehicleNo.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Textile_Processing_Delivery_Head", "Vehicle_No", "", "")
    End Sub

    Private Sub cbo_VehicleNo_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_VehicleNo.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_VehicleNo, cbo_TransportName, txt_Frieght, "Textile_Processing_Delivery_Head", "Vehicle_No", "", "")

    End Sub

    Private Sub cbo_VehicleNo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_VehicleNo.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_VehicleNo, txt_Frieght, "Textile_Processing_Delivery_Head", "Vehicle_No", "", "", False)

    End Sub


    Private Sub txt_Folding_KeyDown(sender As Object, e As KeyEventArgs) Handles txt_Folding.KeyDown

        If (e.KeyValue = 40) Or (e.Control = True And e.KeyValue = 40) Then

            If cbo_ClothSales_OrderCode_forSelection.Visible = True Then
                cbo_ClothSales_OrderCode_forSelection.Focus()
            Else

                If dgv_Details.Rows.Count > 0 Then
                    dgv_Details.Focus()
                    dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(dgvCol_Details.ITEM_GREY)
                Else
                    cbo_TransportName.Focus()
                End If
            End If
        End If

        If (e.KeyValue = 38) Then
            cbo_Delivery_From.Focus()
        End If

    End Sub

    Private Sub txt_Folding_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txt_Folding.KeyPress

        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
            e.Handled = True
        End If

        If Asc(e.KeyChar) = 13 Then


            If cbo_ClothSales_OrderCode_forSelection.Visible = True Then
                cbo_ClothSales_OrderCode_forSelection.Focus()
            Else

                If dgv_Details.Rows.Count > 0 Then
                    dgv_Details.Focus()
                    dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(dgvCol_Details.ITEM_GREY)

                Else
                    cbo_TransportName.Focus()

                End If
            End If
        End If

    End Sub

    Private Sub txt_Frieght_TextChanged(sender As Object, e As EventArgs) Handles txt_Frieght.TextChanged

    End Sub

    Private Sub cbo_Delivery_From_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbo_Delivery_From.SelectedIndexChanged

    End Sub

    Private Sub cbo_Delivery_From_KeyDown(sender As Object, e As KeyEventArgs) Handles cbo_Delivery_From.KeyDown

        vcbo_KeyDwnVal = e.KeyValue

        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Delivery_From, Cbo_ProcessingHEAD, Nothing, "Ledger_AlaisHead", "Ledger_DisplayName", " (Ledger_Type = 'GODOWN' OR Show_In_All_Entry =1 )", "(Ledger_idno = 0)")


        If ((e.KeyValue = 40) Or (e.Control = True And e.KeyValue = 40)) And Not cbo_Delivery_From.DroppedDown Then

            If txt_Folding.Visible And txt_Folding.Enabled Then
                txt_Folding.Focus()
            ElseIf cbo_ClothSales_OrderCode_forSelection.Visible And cbo_ClothSales_OrderCode_forSelection.Enabled Then
                cbo_ClothSales_OrderCode_forSelection.Focus()
            Else
                If dgv_Details.Rows.Count > 0 Then
                    dgv_Details.Focus()
                    dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(dgvCol_Details.ITEM_GREY)

                Else
                    cbo_TransportName.Focus()
                End If
            End If
        End If




    End Sub

    Private Sub cbo_Delivery_From_KeyPress(sender As Object, e As KeyPressEventArgs) Handles cbo_Delivery_From.KeyPress

        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Delivery_From, Nothing, "Ledger_AlaisHead", "Ledger_DisplayName", " (Ledger_Type = 'GODOWN' OR Show_In_All_Entry =1 )", "(Ledger_idno = 0)")

        If Asc(e.KeyChar) = 13 Then

            If cbo_ClothSales_OrderCode_forSelection.Visible = True Then
                cbo_ClothSales_OrderCode_forSelection.Focus()
            ElseIf txt_Folding.Visible Then
                txt_Folding.Focus()
            Else
                If dgv_Details.Rows.Count > 0 Then
                    dgv_Details.Focus()
                    dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(dgvCol_Details.ITEM_GREY)

                Else
                    cbo_TransportName.Focus()

                End If
            End If


        End If

    End Sub

    Private Sub cbo_Delivery_From_KeyUp(sender As Object, e As KeyEventArgs) Handles cbo_Delivery_From.KeyUp

        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then

            Common_Procedures.MDI_LedType = "GODOWN"
            Dim f As New Ledger_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Delivery_From.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = "GODOWN LEDGER"

            f.MdiParent = MDIParent1
            f.Show()

        End If

    End Sub

    Private Sub cbo_Delivery_From_Enter(sender As Object, e As EventArgs) Handles cbo_Delivery_From.Enter

        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", " (Ledger_Type = 'GODOWN' OR Show_In_All_Entry =1 )", "(Ledger_idno = 0)")

    End Sub

    Private Sub dgv_Details_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgv_Details.CellContentClick

    End Sub

    Private Sub cbo_Ledger_Leave(sender As Object, e As EventArgs) Handles cbo_Ledger.Leave

    End Sub

    Private Sub cbo_itemfp_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbo_itemfp.SelectedIndexChanged

    End Sub

    Private Sub CBO_JobNO_TextChanged(sender As Object, e As EventArgs) Handles CBO_JobNO.TextChanged
        Dim Lot_ID = 0

        If Trim(CBO_JobNO.Text) <> "" And lbl_Lot_No_Discribtion.Visible Then

            Lot_ID = Common_Procedures.Lot_NoToIdNo(con, CBO_JobNO.Text)

            lbl_Lot_No_Discribtion.Text = " Lot Quality : " & Common_Procedures.get_FieldValue(con, "Lot_Head", "Lot_Description", " Lot_IdNo = " & Val(Lot_ID) & " ")

        End If

    End Sub

    Private Sub cbo_Ledger_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbo_Ledger.SelectedIndexChanged

    End Sub

    Private Sub cbo_itemgrey_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbo_itemgrey.SelectedIndexChanged

    End Sub

    Private Sub Cbo_ProcessingHEAD_SelectedIndexChanged(sender As Object, e As EventArgs) Handles Cbo_ProcessingHEAD.SelectedIndexChanged

    End Sub

    Private Sub Cbo_ProcessingHEAD_Leave(sender As Object, e As EventArgs) Handles Cbo_ProcessingHEAD.Leave

        Dim Process_Inputs_Tmp As String = ""
        Dim Process_Outputs_Tmp As String = ""

        If Cbo_ProcessingHEAD.Tag <> Cbo_ProcessingHEAD.Text Then

            If Len(Trim(Cbo_ProcessingHEAD.Text)) = 0 Then

                Process_Inputs_Tmp = ""
                Process_Outputs_Tmp = ""

            Else

                Dim da As New SqlClient.SqlDataAdapter("select * from process_head where process_name = '" & Cbo_ProcessingHEAD.Text & "'", con)
                Dim dt As New DataTable

                da.Fill(dt)

                If dt.Rows.Count > 0 Then

                    If Not IsDBNull(dt.Rows(0).Item("Cloth_Delivered")) Then
                        Process_Inputs_Tmp = IIf(dt.Rows(0).Item("Cloth_Delivered") = True, "1", "0")
                    Else
                        Process_Inputs_Tmp = "0"
                    End If

                    If Not IsDBNull(dt.Rows(0).Item("FP_Delivered")) Then
                        Process_Inputs_Tmp = Process_Inputs_Tmp + IIf(dt.Rows(0).Item("FP_Delivered") = True, "1", "0")
                    Else
                        Process_Inputs_Tmp = Process_Inputs_Tmp + "0"
                    End If


                    If Not IsDBNull(dt.Rows(0).Item("Cloth_Returned")) Then
                        Process_Outputs_Tmp = IIf(dt.Rows(0).Item("Cloth_Returned") = True, "1", "0")
                    Else
                        Process_Outputs_Tmp = "0"
                    End If

                    If Not IsDBNull(dt.Rows(0).Item("FP_Returned")) Then
                        Process_Outputs_Tmp = Process_Outputs_Tmp + IIf(dt.Rows(0).Item("FP_Returned") = True, "1", "0")
                    Else
                        Process_Outputs_Tmp = Process_Outputs_Tmp + "0"
                    End If

                End If

            End If

            If Process_Outputs_Tmp + Process_Inputs_Tmp <> Process_Outputs + Process_Inputs Then

                If Len(Trim(Process_Outputs)) + Len(Trim(Process_Inputs)) = 0 Then
                    Process_Inputs = Process_Inputs_Tmp
                    Process_Outputs = Process_Outputs_Tmp
                    For I = 0 To dgv_Details.Rows.Count - 1
                        dgv_Details.Rows(I).Cells(dgvCol_Details.ITEM_FP).Value = ""
                    Next
                Else

                    If MessageBox.Show("Changing the Process Will Clear All Finished Product Values in Details. Continue ?", "CHANGE PROCESS...", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.No Then
                        Cbo_ProcessingHEAD.Text = Cbo_ProcessingHEAD.Tag
                        Cbo_ProcessingHEAD.Focus()
                    Else
                        Process_Inputs = Process_Inputs_Tmp
                        Process_Outputs = Process_Outputs_Tmp
                        For I = 0 To dgv_Details.Rows.Count - 1
                            dgv_Details.Rows(I).Cells(dgvCol_Details.ITEM_FP).Value = ""
                        Next
                    End If
                End If

            End If

        End If

    End Sub



    Private Sub btn_EWayBIll_Generation_Click(sender As Object, e As EventArgs) Handles btn_EWayBIll_Generation.Click
        btn_GENERATEEWB.Enabled = True
        Grp_EWB.Visible = True
        Grp_EWB.BringToFront()
        Grp_EWB.Left = (Me.Width - pnl_Back.Width) / 2 + 160
        Grp_EWB.Top = (Me.Height - pnl_Back.Height) / 2 + 150
    End Sub

    Private Sub btn_GENERATEEWB_Click(sender As Object, e As EventArgs) Handles btn_GENERATEEWB.Click

        con = New SqlClient.SqlConnection(Common_Procedures.Connection_String)
        con.Open()

        '   Dim NewCode As String = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)
        Dim NewCode As String = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)



        For i = 0 To dgv_Details.RowCount - 1
            If Val(dgv_Details.Rows(i).Cells(dgvCol_Details.METERS).Value) <> 0 And Val(dgv_Details.Rows(i).Cells(dgvCol_Details.RATE).Value) = 0 Then
                MessageBox.Show("Invalid Meter Rate", "DOES NOT GENERATE EWB...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                dgv_Details.Focus()
                dgv_Details.CurrentCell = dgv_Details.Rows(i).Cells(dgvCol_Details.RATE)
                dgv_Details.CurrentCell.Selected = True
                Exit Sub
            End If


        Next


        Dim da As New SqlClient.SqlDataAdapter("Select EwayBill_No from Textile_Processing_Delivery_Head where ClothProcess_Delivery_Code = '" & NewCode & "'", con)
        Dim dt As New DataTable

        da.Fill(dt)

        If dt.Rows.Count = 0 Then
            MessageBox.Show("Please Save the Delivery Challan before proceeding to generate EWB", "Please SAVE", MessageBoxButtons.OKCancel)
            dt.Clear()
            Exit Sub
        End If

        If Not IsDBNull(dt.Rows(0).Item(0)) Then
            If Len(Trim(dt.Rows(0).Item(0))) > 0 Then
                MessageBox.Show("EWB has been generated for this Delivery already", "Redundant Request", MessageBoxButtons.OKCancel)
                dt.Clear()
                Exit Sub
            End If
        End If

        dt.Clear()

        Dim CMD As New SqlClient.SqlCommand
        CMD.Connection = con


        CMD.CommandText = "Delete from EWB_Head Where InvCode = '" & NewCode & "'"
        CMD.ExecuteNonQuery()


        CMD.CommandText = "Insert into EWB_Head ([SupplyType]  ,[SubSupplyType]  , [SubSupplyDesc]  ,[DocType]  ,	[EWBGenDocNo]                           ,[EWBDocDate]        ,[FromGSTIN]       ,[FromTradeName]  ,[FromAddress1]   ,[FromAddress2]     ,[FromPlace] ," &
                         "[FromPINCode]     ,	[FromStateCode] ,[ActualFromStateCode] ,[ToGSTIN]       ,[ToTradeName],[ToAddress1]      ,[ToAddress2]    ,[ToPlace]       ,[ToPINCode]       ,[ToStateCode] , [ActualToStateCode] ," &
                         "[TransactionType],[OtherValue]                       ,	[Total_value]       ,	[CGST_Value],[SGST_Value],[IGST_Value]     ,	[CessValue],[CessNonAdvolValue],[TransporterID]    ,[TransporterName]," &
                         "[TransportDOCNo] ,[TransportDOCDate]    ,[TotalInvValue]    ,[TransMode]             ," &
                         "[VehicleNo]      ,[VehicleType]   , [InvCode], [ShippedToGSTIN], [ShippedToTradeName] ) " &
                         " " &
                         " " &
                         "  SELECT               'O'              , '4'             ,   'JOB WORK'              ,    'CHL'    , a.ClothProcess_Delivery_No ,a.ClothProcess_Delivery_Date          , C.Company_GSTINNo, C.Company_Name   ,C.Company_Address1+C.Company_Address2,c.Company_Address3+C.Company_Address4,C.Company_City ," &
                         " C.Company_PinCode    , FS.State_Code  ,FS.State_Code    ,L.Ledger_GSTINNo, L.Ledger_MainName, (case when a.DeliveryTo_IdNo <> 0 then tDELV.Ledger_Address1+tDELV.Ledger_Address2 else  L.Ledger_Address1+L.Ledger_Address2 end) as deliveryaddress1,  (case when a.DeliveryTo_IdNo <> 0 then tDELV.Ledger_Address3+tDELV.Ledger_Address4 else  L.Ledger_Address3+L.Ledger_Address4 end) as deliveryaddress2, (case when a.DeliveryTo_IdNo <> 0 then tDELV.City_Town else  L.City_Town end) as city_town_name, (case when a.DeliveryTo_IdNo <> 0 then tDELV.Pincode else  L.Pincode end) as pincodee, TS.State_Code, (case when a.DeliveryTo_IdNo <> 0 then TDCS.State_Code else TS.State_Code end) as actual_StateCode," &
                         " 1                     , 0 , a.Net_Amount     , 0 ,  0 , 0   ,   0         ,0                   , t.Ledger_GSTINNo  , t.Ledger_Name ," &
                         " ''        ,''            ,  0        ,     '1'  AS TrMode ," &
                         " a.Vehicle_No, 'R', '" & NewCode & "', tDELV.Ledger_GSTINNo as ShippedTo_GSTIN, tDELV.Ledger_MainName as ShippedTo_LedgerName from Textile_Processing_Delivery_Head a inner Join Company_Head C on a.Company_IdNo = C.Company_IdNo " &
                         " Inner Join Ledger_Head L ON a.Ledger_IdNo = L.Ledger_IdNo Left Outer Join Ledger_Head tDELV on ( case When a.DeliveryTo_IdNo <> 0  then a.DeliveryTo_IdNo else a.Ledger_IdNo end ) = tDELV.Ledger_IdNo left Outer Join Ledger_Head T on a.Transport_IdNo = T.Ledger_IdNo " &
                         " Left Outer Join State_Head FS On C.Company_State_IdNo = fs.State_IdNo left Outer Join State_Head TS on L.Ledger_State_IdNo = TS.State_IdNo  left Outer Join State_Head TDCS on tDELV.Ledger_State_IdNo = TDCS.State_IdNo " &
                          " where a.ClothProcess_Delivery_Code = '" & Trim(NewCode) & "'"
        CMD.ExecuteNonQuery()


        Dim vCgst_Amt As String = 0
        Dim vSgst_Amt As String = 0
        Dim vIgst_AMt As String = 0
        Dim vTax_Perc As String = 0

        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim da1 As New SqlClient.SqlDataAdapter


        CMD.CommandText = "Delete from EWB_Details Where InvCode = '" & NewCode & "'"
        CMD.ExecuteNonQuery()

        da = New SqlClient.SqlDataAdapter(" Select  I.Cloth_Name,IG.ItemGroup_Name,IG.Item_HSN_Code,IG.Item_GST_Percentage,sum(SD.Delivery_Meters * SD.Rate) As TaxableAmt,sum(SD.Delivery_Meters) as Qty, Min(Sl_No), 'MTR' AS Units , tz.Company_State_IdNo , Lh.Ledger_State_Idno  ,a.GST_Tax_Invoice_Status  " &
                                          " from Textile_Processing_Delivery_Details SD Inner join Textile_Processing_Delivery_Head a on a.ClothProcess_Delivery_Code = SD.Cloth_Processing_Delivery_Code  Inner Join Cloth_Head I On SD.Item_Idno = I.Cloth_IdNo Inner Join ItemGroup_Head IG on I.ItemGroup_IdNo = IG.ItemGroup_IdNo " &
                                          " INNER Join Ledger_Head Lh ON Lh.Ledger_Idno =  a.Ledger_Idno  INNER JOIN Company_Head tz On tz.Company_Idno = a.Company_Idno" &
                                          " Where SD.Cloth_Processing_Delivery_Code = '" & Trim(NewCode) & "' Group By " &
                                          " I.Cloth_Name,IG.ItemGroup_Name,IG.Item_HSN_Code,IG.ItemGroup_Name ,IG.Item_HSN_Code,IG.Item_GST_Percentage, tz.Company_State_IdNo , Lh.Ledger_State_Idno  ,a.GST_Tax_Invoice_Status ", con)
        dt1 = New DataTable
        da.Fill(dt1)


        For I = 0 To dt1.Rows.Count - 1

            If Val(dt1.Rows(I).Item("GST_Tax_Invoice_Status")) = 1 Then
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

                vTax_Perc = dt1.Rows(I).Item(3).ToString

            Else

                vIgst_AMt = 0
                vCgst_Amt = 0
                vSgst_Amt = 0
                vTax_Perc = 0

            End If

            CMD.CommandText = "Insert into EWB_Details ([SlNo]      ,       [Product_Name]          ,	[Product_Description]      ,	        [HSNCode]                      ,	    [Quantity]  ,               [QuantityUnit]         ,           Tax_Perc          ,	[CessRate]    ,	  [CessNonAdvol]  ,  	[TaxableAmount]          ,      InvCode       ,             Cgst_Value          ,             Sgst_Value          ,           Igst_Value)  " &
                  " values                 (        " & I & "       , '" & dt1.Rows(I).Item(0) & "' , '" & dt1.Rows(I).Item(1) & "', '" & dt1.Rows(I).Item(2) & "', " & dt1.Rows(I).Item(5).ToString & ",                'MTR'             ,  " & Val(vTax_Perc) & "     ,      0         ,        0          , " & dt1.Rows(I).Item(4) & "  , '" & NewCode & "' ,   '" & Str(Val(vCgst_Amt)) & "' ,   '" & Str(Val(vSgst_Amt)) & "' , '" & Str(Val(vIgst_AMt)) & "')"
            CMD.ExecuteNonQuery()


        Next I

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


        btn_GENERATEEWB.Enabled = False

        ' -------------

        Dim ewb As New EWB(Val(lbl_Company.Tag))
        EWB.GenerateEWB(NewCode, con, rtbEWBResponse, txt_EWBNo, "Textile_Processing_Delivery_Head", "EwayBill_No", "ClothProcess_Delivery_Code", Pk_Condition)


    End Sub

    Private Sub btn_PRINTEWB_Click(sender As Object, e As EventArgs) Handles btn_PRINTEWB.Click
        Dim ewb As New EWB(Val(lbl_Company.Tag))
        EWB.PrintEWB(txt_EWBNo.Text, rtbEWBResponse, 0)
    End Sub

    Private Sub btn_CancelEWB_1_Click(sender As Object, e As EventArgs) Handles btn_CancelEWB_1.Click
        'Dim NewCode As String = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Dim NewCode As String = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)
        Dim c As Integer = MsgBox("Are You Sure To Cancel This EWB ? ", vbYesNo)

        If c = vbNo Then Exit Sub

        con = New SqlClient.SqlConnection(Common_Procedures.Connection_String)
        con.Open()

        Dim ewb As New EWB(Val(lbl_Company.Tag))

        EWB.CancelEWB(txt_EWBNo.Text, NewCode, con, rtbEWBResponse, txt_EWBNo, "Textile_Processing_Delivery_Head", "EwayBill_No", "ClothProcess_Delivery_Code")

    End Sub

    Private Sub btn_Close_EWB_Click(sender As Object, e As EventArgs) Handles btn_Close_EWB.Click
        Grp_EWB.Visible = False
    End Sub
    Private Sub btn_Detail_PRINTEWB_Click(sender As Object, e As EventArgs) Handles btn_Detail_PRINTEWB.Click
        Dim ewb As New EWB(Val(lbl_Company.Tag))
        EWB.PrintEWB(txt_EWBNo.Text, rtbEWBResponse, 1)
    End Sub

    Private Sub btn_CheckConnectivity_Click(sender As Object, e As EventArgs) Handles btn_CheckConnectivity.Click
        con = New SqlClient.SqlConnection(Common_Procedures.Connection_String)
        con.Open()

        'Dim einv As New eInvoice(Val(lbl_Company.Tag))
        'einv.GetAuthToken(rtbEWBResponse)

        Dim ewb As New EWB(Val(lbl_Company.Tag))
        EWB.GetAuthToken(rtbEWBResponse)
    End Sub
    Private Sub dgtxt_Details_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgtxt_details.TextChanged
        Try
            With dgv_Details
                If .Rows.Count <> 0 Then
                    .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(dgtxt_details.Text)
                End If
            End With

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try
    End Sub
    Private Sub txt_EWBNo_TextChanged(sender As Object, e As EventArgs) Handles txt_EWBNo.TextChanged
        With chk_Ewb_No_Sts
            If Trim(txt_EWBNo.Text) <> "" Then
                .Checked = True
            Else
                .Checked = False
            End If
        End With

    End Sub
    Private Sub cbo_InvoiceSufixNo_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_DcSufixNo.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_DcSufixNo, txt_DcPrefixNo, dtp_Date, "", "", "", "")
    End Sub

    Private Sub cbo_InvoiceSufixNo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_DcSufixNo.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_DcSufixNo, dtp_Date, "", "", "", "", False)
    End Sub

    Private Sub txt_InvoicePrefixNo_KeyDown(sender As Object, e As KeyEventArgs) Handles txt_DcPrefixNo.KeyDown
        If e.KeyCode = 38 Then
            txt_Note.Focus()
        ElseIf e.KeyCode = 40 Then
            cbo_DcSufixNo.Focus()
        End If
    End Sub

    Private Sub txt_InvoicePrefixNo_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txt_DcPrefixNo.KeyPress
        If Asc(e.KeyChar) = 13 Then
            cbo_DcSufixNo.Focus()
        End If
    End Sub

    Private Sub dtp_Date_KeyDown(sender As Object, e As KeyEventArgs) Handles dtp_Date.KeyDown
        If e.KeyCode = 38 Then
            cbo_DcSufixNo.Focus()
        ElseIf e.KeyCode = 40 Then
            CBO_JobNO.Focus()
        End If
    End Sub
    Private Sub dgv_PieceSelection_CellClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_PieceSelection.CellClick
        Select_PieceSelection_Grid(e.RowIndex)
    End Sub

    Private Sub dgv_PieceSelection_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_PieceSelection.KeyDown
        Dim n As Integer

        On Error Resume Next

        If e.KeyCode = Keys.Enter Or e.KeyCode = Keys.Space Then

            If dgv_PieceSelection.CurrentCell.RowIndex >= 0 Then

                n = dgv_PieceSelection.CurrentCell.RowIndex

                Select_PieceSelection_Grid(n)

                e.Handled = True

            End If

        End If
    End Sub

    Private Sub Select_PieceSelection_Grid(ByVal RwIndx As Integer)
        Dim i As Integer

        With dgv_PieceSelection

            If .RowCount > 0 And RwIndx >= 0 Then

                .Rows(RwIndx).Cells(dgvCol_PieceSelection.STS).Value = (Val(.Rows(RwIndx).Cells(dgvCol_PieceSelection.STS).Value) + 1) Mod 2

                If Val(.Rows(RwIndx).Cells(dgvCol_PieceSelection.STS).Value) = 1 Then

                    For i = 0 To .ColumnCount - 1
                        .Rows(RwIndx).Cells(i).Style.ForeColor = Color.Red
                    Next

                Else

                    .Rows(RwIndx).Cells(8).Value = ""

                    For i = 0 To .ColumnCount - 1
                        .Rows(RwIndx).Cells(i).Style.ForeColor = Color.Blue
                    Next

                End If

            End If

        End With

    End Sub

    Private Sub chk_PieceSelection_SelectAll_Pieces_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chk_PieceSelection_SelectAll_Pieces.CheckedChanged
        Dim i As Integer
        Dim J As Integer
        Dim v1stVisiRow As Integer = 0

        With dgv_PieceSelection

            For i = 0 To .Rows.Count - 1
                If .Rows(i).Visible = True Then
                    .Rows(i).Cells(dgvCol_PieceSelection.STS).Value = ""
                    For J = 0 To .ColumnCount - 1
                        .Rows(i).Cells(J).Style.ForeColor = Color.Black
                    Next J
                End If
            Next i

            v1stVisiRow = -1
            If chk_PieceSelection_SelectAll_Pieces.Checked = True Then
                For i = 0 To .Rows.Count - 1
                    If .Rows(i).Visible = True Then
                        Select_PieceSelection_Grid(i)
                        If v1stVisiRow = -1 Then v1stVisiRow = i
                    End If
                Next i
            End If

            If .Rows.Count > 0 Then

                If v1stVisiRow >= 0 Then
                    .Focus()
                    .CurrentCell = .Rows(v1stVisiRow).Cells(dgvCol_PieceSelection.SLNO)
                    .CurrentCell.Selected = True
                Else
                    txt_PieceSelection_LotNo.Focus()
                End If

            End If

        End With

    End Sub



    Private Sub txt_PieceSelection_PcsNo_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_PieceSelection_PcsNo.KeyDown
        If e.KeyValue = 40 Then
            txt_PieceSelection_Meters.Focus()

            'If dgv_PieceSelection.Rows.Count > 0 Then
            '    dgv_PieceSelection.Focus()
            '    dgv_PieceSelection.CurrentCell = dgv_PieceSelection.Rows(0).Cells(0)
            '    dgv_PieceSelection.CurrentCell.Selected = True

            'End If

        End If
        If (e.KeyValue = 38) Then txt_PieceSelection_LotNo.Focus()

    End Sub

    Private Sub txt_PieceSelection_PcsNo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_PieceSelection_PcsNo.KeyPress

        If Asc(e.KeyChar) = 13 Then

            If Trim(txt_PieceSelection_PcsNo.Text) <> "" Or Trim(txt_PieceSelection_PcsNo.Text) <> "" Then
                btn_PieceSelection_SelectPiece_Click(sender, e)


            Else

                txt_PieceSelection_Meters.Focus()
                'If dgv_PieceSelection.Rows.Count > 0 Then
                '    dgv_PieceSelection.Focus()
                '    dgv_PieceSelection.CurrentCell = dgv_PieceSelection.Rows(0).Cells(0)
                '    dgv_PieceSelection.CurrentCell.Selected = True
                'End If

            End If

        End If

    End Sub

    Private Sub txt_PieceSelection_LotNo_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_PieceSelection_LotNo.KeyDown
        If (e.KeyValue = 40) Then
            txt_PieceSelection_PcsNo.Focus()
        End If
    End Sub

    Private Sub txt_PieceSelection_LotNo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_PieceSelection_LotNo.KeyPress
        If Asc(e.KeyChar) = 13 Then
            txt_PieceSelection_PcsNo.Focus()
        End If
    End Sub

    Private Sub btn_PieceSelection_SelectPiece_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_PieceSelection_SelectPiece.Click
        Dim Da1 As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim LtNo As String
        Dim PcsNo As String
        Dim i As Integer



        If Trim(txt_PieceSelection_LotNo.Text) <> "" Or Trim(txt_PieceSelection_PcsNo.Text) <> "" Then

            LtNo = Trim(txt_PieceSelection_LotNo.Text)
            PcsNo = Trim(txt_PieceSelection_PcsNo.Text)

            For i = 0 To dgv_PieceSelection.Rows.Count - 1

                If dgv_PieceSelection.Rows(i).Visible = True Then

                    If Trim(txt_PieceSelection_LotNo.Text) <> "" And Trim(txt_PieceSelection_PcsNo.Text) <> "" Then

                        If Trim(UCase(LtNo)) = Trim(UCase(dgv_PieceSelection.Rows(i).Cells(dgvCol_PieceSelection.LOT_NO).Value)) And Trim(UCase(PcsNo)) = Trim(UCase(dgv_PieceSelection.Rows(i).Cells(dgvCol_PieceSelection.PCS_NO).Value)) Then
                            Call Select_PieceSelection_Grid(i)
                            dgv_PieceSelection.CurrentCell = dgv_PieceSelection.Rows(i).Cells(dgvCol_PieceSelection.SLNO)
                            If i >= 9 Then dgv_PieceSelection.FirstDisplayedScrollingRowIndex = i - 8
                            Exit For
                        End If

                    ElseIf Trim(txt_PieceSelection_LotNo.Text) <> "" Then

                        If Trim(UCase(LtNo)) = Trim(UCase(dgv_PieceSelection.Rows(i).Cells(dgvCol_PieceSelection.LOT_NO).Value)) Then
                            Call Select_PieceSelection_Grid(i)
                            dgv_PieceSelection.CurrentCell = dgv_PieceSelection.Rows(i).Cells(dgvCol_PieceSelection.SLNO)
                            If i >= 9 Then dgv_PieceSelection.FirstDisplayedScrollingRowIndex = i - 8
                            Exit For
                        End If

                    ElseIf Trim(txt_PieceSelection_PcsNo.Text) <> "" Then

                        If Trim(UCase(PcsNo)) = Trim(UCase(dgv_PieceSelection.Rows(i).Cells(dgvCol_PieceSelection.PCS_NO).Value)) Then
                            Call Select_PieceSelection_Grid(i)
                            dgv_PieceSelection.CurrentCell = dgv_PieceSelection.Rows(i).Cells(dgvCol_PieceSelection.SLNO)
                            If i >= 9 Then dgv_PieceSelection.FirstDisplayedScrollingRowIndex = i - 8
                            Exit For
                        End If

                    End If

                End If

            Next i

            txt_PieceSelection_LotNo.Text = ""
            txt_PieceSelection_PcsNo.Text = ""

            If txt_PieceSelection_LotNo.Enabled = True Then txt_PieceSelection_LotNo.Focus()

        End If
    End Sub

    Private Sub btn_PieceSelection_Show_Piece_by_Meters_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_PieceSelection_Show_Piece_by_Meters.Click
        Dim vFirstRowNo As Integer = -1
        Dim i As Integer

        If Val(txt_PieceSelection_Meters.Text) <> 0 Then

            For i = 0 To dgv_PieceSelection.Rows.Count - 1
                dgv_PieceSelection.Rows(i).Visible = False
            Next

            vFirstRowNo = -1
            For i = 0 To dgv_PieceSelection.Rows.Count - 1
                If Val(dgv_PieceSelection.Rows(i).Cells(dgvCol_PieceSelection.METERS).Value) = Val(txt_PieceSelection_Meters.Text) Then
                    dgv_PieceSelection.Rows(i).Visible = True
                    If vFirstRowNo = -1 Then vFirstRowNo = i
                End If
            Next

            If vFirstRowNo >= 0 Then
                dgv_PieceSelection.Focus()
                dgv_PieceSelection.CurrentCell = dgv_PieceSelection.Rows(vFirstRowNo).Cells(dgvCol_PieceSelection.SLNO)
                dgv_PieceSelection.CurrentCell.Selected = True
            Else
                txt_PieceSelection_Meters.SelectAll()
                If txt_PieceSelection_Meters.Enabled = True Then txt_PieceSelection_Meters.Focus()
            End If

        Else

            btn_PieceSelection_Show_AllPiece_Click(sender, e)

        End If

    End Sub

    Private Sub btn_PieceSelection_Show_AllPiece_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_PieceSelection_Show_AllPiece.Click
        Dim i As Integer = 0
        Dim CurRow As Integer = 0

        Try

            For i = 0 To dgv_PieceSelection.Rows.Count - 1
                dgv_PieceSelection.Rows(i).Visible = True
            Next
            txt_PieceSelection_Meters.Text = ""

        Catch ex As Exception
            '---
        End Try
    End Sub

    Private Sub txt_PieceSelection_Meters_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_PieceSelection_Meters.KeyDown
        If e.KeyValue = 40 Then
            If dgv_PieceSelection.Rows.Count > 0 Then
                dgv_PieceSelection.Focus()
                dgv_PieceSelection.CurrentCell = dgv_PieceSelection.Rows(0).Cells(dgvCol_PieceSelection.SLNO)
                dgv_PieceSelection.CurrentCell.Selected = True
            Else
                txt_PieceSelection_LotNo.Focus()
            End If
        End If
        If (e.KeyValue = 38) Then txt_PieceSelection_PcsNo.Focus()
    End Sub

    Private Sub txt_PieceSelection_Meters_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_PieceSelection_Meters.KeyPress
        If Asc(e.KeyChar) = 13 Then

            btn_PieceSelection_Show_Piece_by_Meters_Click(sender, e)

        End If
    End Sub
    Private Sub btn_PieceSelection_Click(sender As Object, e As EventArgs) Handles btn_PieceSelection.Click
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim i As Integer, j As Integer, n As Integer, SNo As Integer
        Dim CloIdNo As Integer, CloTypIdNo As Integer
        Dim NewCode As String
        Dim CompIDCondt As String
        Dim vStkOf_IdNo As Integer = 0
        Dim Clo_GrpIdNos As String
        Dim Clo_UndIdNo As Integer
        Dim CloID_Cond As String = ""
        Dim PcsMtrs As Double = 0
        Dim vLmIdNo As Long = 0
        Dim vLmNo As String = ""
        Dim vGod_ID As Integer = 0
        Dim CloTyp_Selc_STS As Boolean = False
        Dim Fld_Perc As String = 0
        Dim vEntBaleNo As String = ""
        Dim dgvDet_CurRow As Integer
        Dim dgvDet_DetSlNo As Integer
        Dim vCTCnt As Integer
        Dim K As Integer
        Dim vSelcCodeCondt As String = ""
        Dim vMtrsColCondt As String = ""
        Dim vMtrsColNm As String = ""
        Dim vByrOfrMtrsColNm As String = ""
        Dim vByrOfrCodeColNm As String = ""
        Dim vBarCodeColNm As String = ""
        Dim vCLOTH_STOCKMAINTENANCE_IN As String = ""

        If IsNothing(dgv_Details.CurrentCell) Then Exit Sub

        dgvDet_CurRow = dgv_Details.CurrentCell.RowIndex
        dgvDet_DetSlNo = Val(dgv_Details.Rows(dgvDet_CurRow).Cells(dgvCol_Details.CLOTH_PROCESSING_DELIVERY_PACKINGSLIP_SLNO).Value)


        dgvDet_CurRow = dgv_Details.CurrentCell.RowIndex
        dgvDet_DetSlNo = Val(dgv_Details.Rows(dgvDet_CurRow).Cells(dgvCol_Details.CLOTH_PROCESSING_DELIVERY_PACKINGSLIP_SLNO).Value)

        If Val(dgvDet_DetSlNo) = 0 Then
            Set_Max_DetailsSlNo(dgvDet_CurRow, dgvCol_Details.CLOTH_PROCESSING_DELIVERY_PACKINGSLIP_SLNO)
        End If

        dgvDet_DetSlNo = Val(dgv_Details.Rows(dgvDet_CurRow).Cells(dgvCol_Details.CLOTH_PROCESSING_DELIVERY_PACKINGSLIP_SLNO).Value)

        If Val(dgvDet_DetSlNo) = 0 Then
            MessageBox.Show("Invalid Cloth Delivery Details.SlNo", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If dgv_Details.Enabled And dgv_Details.Visible Then
                If dgv_Details.Rows.Count > 0 Then
                    dgv_Details.Focus()
                    dgv_Details.CurrentCell = dgv_Details.Rows(dgvDet_DetSlNo).Cells(dgvCol_Details.ITEM_GREY)
                    dgv_Details.CurrentCell.Selected = True
                End If
            End If
            Exit Sub
        End If


        If dgv_Details.CurrentCell.RowIndex < 0 Then
            MessageBox.Show("Invalid Cloth Name ", "DOES NOT SELECT PIECE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If dgv_Details.Enabled And dgv_Details.Visible Then
                If dgv_Details.Rows.Count > 0 Then
                    dgv_Details.Focus()
                    dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(dgvCol_Details.PCS)
                    dgv_Details.CurrentCell.Selected = True
                End If
            End If
            Exit Sub
        End If

        CloIdNo = Common_Procedures.Cloth_NameToIdNo(con, dgv_Details.Rows(dgv_Details.CurrentCell.RowIndex).Cells(dgvCol_Details.ITEM_GREY).Value)
        If CloIdNo = 0 Then
            MessageBox.Show("Invalid Cloth Name", "DOES NOT SELECT PIECE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If dgv_Details.Enabled And dgv_Details.Visible Then
                If dgv_Details.Rows.Count > 0 Then
                    dgv_Details.Focus()
                    dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(dgvCol_Details.ITEM_GREY)
                    If cbo_itemgrey.Visible And cbo_itemgrey.Enabled Then cbo_itemgrey.Focus()
                    'dgv_Details.CurrentCell.Selected = True
                    Exit Sub
                End If
            End If
            Exit Sub
        End If



        If vStkOf_IdNo = 0 Then vStkOf_IdNo = Common_Procedures.CommonLedger.OwnSort_Ac

        vGod_ID = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Delivery_From.Text)
        If cbo_Delivery_From.Visible = True Then
            If vGod_ID = 0 Then
                MessageBox.Show("Invalid Fabric Godown Name", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                If cbo_Delivery_From.Enabled And cbo_Delivery_From.Visible Then cbo_Delivery_From.Focus()
                Exit Sub
            End If
        End If
        If vGod_ID = 0 Then vGod_ID = Common_Procedures.CommonLedger.Godown_Ac


        'CloTypIdNo = Common_Procedures.ClothType_NameToIdNo(con, dgv_Details.Rows(dgv_Details.CurrentCell.RowIndex).Cells(2).Value)
        'If CloTypIdNo = 0 Then
        '    MessageBox.Show("Invalid Cloth Type ", "DOES NOT SELECT BALE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
        '    If dgv_Details.Enabled And dgv_Details.Visible Then
        '        If dgv_Details.Rows.Count > 0 Then
        '            dgv_Details.Focus()
        '            dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(2)
        '            If cbo_Grid_Clothtype.Visible And cbo_Grid_Clothtype.Enabled Then cbo_Grid_Clothtype.Focus()
        '            Exit Sub
        '        End If
        '    End If
        '    Exit Sub
        'End If

        'Fld_Perc = dgv_Details.Rows(dgv_Details.CurrentCell.RowIndex).Cells(3).Value
        'If Val(Fld_Perc) = 0 Then Fld_Perc = 100
        'If Val(Fld_Perc) = 0 Then
        '    MessageBox.Show("Invalid Folding", "DOES NOT SELECT BALE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
        '    If dgv_Details.Enabled And dgv_Details.Visible Then
        '        If dgv_Details.Rows.Count > 0 Then
        '            dgv_Details.Focus()
        '            dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(3)
        '            dgv_Details.CurrentCell.Selected = True
        '        End If
        '    End If
        '    Exit Sub
        'End If



        If Val(Fld_Perc) = 0 Then Fld_Perc = 100

        CompIDCondt = "(a.company_idno = " & Str(Val(lbl_Company.Tag)) & ")"
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1155" And Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1204" Then '----KRG TEXTILE MILLS (PALLADAM)
            If Common_Procedures.settings.EntrySelection_Combine_AllCompany = 1 Then
                CompIDCondt = ""
                If Trim(UCase(Common_Procedures.User.Type)) = "ACCOUNT" Then
                    CompIDCondt = "(Company_Type <> 'UNACCOUNT')"
                End If
            End If
        End If

        NewCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Clo_UndIdNo = CloIdNo

        Da = New SqlClient.SqlDataAdapter("select * from Cloth_head where Cloth_idno = " & Str(Val(Clo_UndIdNo)), con)
        Dt1 = New DataTable
        Da.Fill(Dt1)
        If Dt1.Rows.Count > 0 Then
            If IsDBNull(Dt1.Rows(0).Item("Cloth_StockUnder_IdNo").ToString) = False Then
                If Val(Dt1.Rows(0).Item("Cloth_StockUnder_IdNo").ToString) <> 0 Then Clo_UndIdNo = Val(Dt1.Rows(0).Item("Cloth_StockUnder_IdNo").ToString)
            End If
        End If
        Dt1.Clear()

        Da = New SqlClient.SqlDataAdapter("select * from Cloth_head where Cloth_StockUnder_IdNo = " & Str(Val(Clo_UndIdNo)), con)
        Dt1 = New DataTable
        Da.Fill(Dt1)

        Clo_GrpIdNos = ""
        If Dt1.Rows.Count > 0 Then
            For i = 0 To Dt1.Rows.Count - 1
                Clo_GrpIdNos = Trim(Clo_GrpIdNos) & IIf(Trim(Clo_GrpIdNos) <> "", ", ", "") & Trim(Val(Dt1.Rows(i).Item("Cloth_IdNo")))
            Next
        End If
        If Trim(Clo_GrpIdNos) <> "" Then
            Clo_GrpIdNos = "(" & Clo_GrpIdNos & ")"
        Else
            Clo_GrpIdNos = "(" & Trim(Val(CloIdNo)) & ")"
        End If

        CloID_Cond = "(a.Cloth_idno = " & Str(CloIdNo) & " or a.Cloth_idno IN " & Trim(Clo_GrpIdNos) & ")"

        If cbo_Delivery_From.Visible = True Then
            CloID_Cond = CloID_Cond & IIf(CloID_Cond <> "", " and ", " ") & "(a.WareHouse_idno = " & Str(vGod_ID) & ")"
        End If

        If vStkOf_IdNo = 4 Or vStkOf_IdNo = 5 Then
            CloID_Cond = CloID_Cond & IIf(CloID_Cond <> "", " and ", " ") & "(a.StockOff_IdNo = 4 or a.StockOff_IdNo = 5)"
        Else
            CloID_Cond = CloID_Cond & IIf(CloID_Cond <> "", " and ", " ") & "(a.StockOff_IdNo = " & Str(vStkOf_IdNo) & ")"
        End If

        'If pnl_Pieces_BaleNo_Entry_Details.Visible = False And Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1186" And Trim(UCase(cbo_RollBundle.Text)) = "BALE" And Trim(dgv_Details.Rows(dgv_Details.CurrentCell.RowIndex).Cells(14).Value) <> "" Then

        '    With dgv_PieceDetails

        '        dgv_Pieces_BaleNo_Entry_Details.Rows.Clear()

        '        SNo = 0
        '        For i = 0 To .Rows.Count - 1

        '            If Val(UCase(.Rows(i).Cells(0).Value)) = Val(dgvDet_DetSlNo) Then

        '                If Val(.Rows(i).Cells(4).Value) <> 0 And Trim(.Rows(i).Cells(8).Value) <> "" And Trim(.Rows(i).Cells(2).Value) <> "" And Trim(.Rows(i).Cells(3).Value) <> "" Then

        '                    SNo = SNo + 1
        '                    n = dgv_Pieces_BaleNo_Entry_Details.Rows.Add()
        '                    dgv_Pieces_BaleNo_Entry_Details.Rows(n).Cells(0).Value = SNo
        '                    dgv_Pieces_BaleNo_Entry_Details.Rows(n).Cells(1).Value = dgv_PieceDetails.Rows(i).Cells(1).Value
        '                    dgv_Pieces_BaleNo_Entry_Details.Rows(n).Cells(2).Value = dgv_PieceDetails.Rows(i).Cells(2).Value
        '                    dgv_Pieces_BaleNo_Entry_Details.Rows(n).Cells(3).Value = dgv_PieceDetails.Rows(i).Cells(3).Value
        '                    dgv_Pieces_BaleNo_Entry_Details.Rows(n).Cells(4).Value = dgv_PieceDetails.Rows(i).Cells(4).Value
        '                    If Val(dgv_PieceDetails.Rows(i).Cells(5).Value) <> 0 Then
        '                        dgv_Pieces_BaleNo_Entry_Details.Rows(n).Cells(5).Value = dgv_PieceDetails.Rows(i).Cells(5).Value
        '                    End If
        '                    If Val(dgv_PieceDetails.Rows(i).Cells(6).Value) <> 0 Then
        '                        dgv_Pieces_BaleNo_Entry_Details.Rows(n).Cells(6).Value = dgv_PieceDetails.Rows(i).Cells(6).Value
        '                    End If
        '                    dgv_Pieces_BaleNo_Entry_Details.Rows(n).Cells(7).Value = dgv_PieceDetails.Rows(i).Cells(7).Value
        '                    dgv_Pieces_BaleNo_Entry_Details.Rows(n).Cells(8).Value = dgv_PieceDetails.Rows(i).Cells(8).Value
        '                    dgv_Pieces_BaleNo_Entry_Details.Rows(n).Cells(9).Value = dgv_PieceDetails.Rows(i).Cells(9).Value
        '                    dgv_Pieces_BaleNo_Entry_Details.Rows(n).Cells(10).Value = dgv_PieceDetails.Rows(i).Cells(10).Value
        '                    dgv_Pieces_BaleNo_Entry_Details.Rows(n).Cells(11).Value = dgv_PieceDetails.Rows(i).Cells(11).Value


        '                End If

        '            End If

        '        Next i

        '    End With


        '    pnl_Pieces_BaleNo_Entry_Details.Visible = True
        '    pnl_Back.Enabled = False
        '    pnl_PieceSelection.Visible = False
        '    If dgv_Pieces_BaleNo_Entry_Details.Enabled And dgv_Pieces_BaleNo_Entry_Details.Visible And dgv_Pieces_BaleNo_Entry_Details.Rows.Count > 0 Then

        '        dgv_Pieces_BaleNo_Entry_Details.Focus()
        '        If dgv_Pieces_BaleNo_Entry_Details.CurrentCell.RowIndex >= 0 Then
        '            dgv_Pieces_BaleNo_Entry_Details.CurrentCell = dgv_Pieces_BaleNo_Entry_Details.Rows(0).Cells(11)
        '            dgv_Pieces_BaleNo_Entry_Details.CurrentCell.Selected = True
        '        End If



        '    Else

        '        btn_Pieces_BaleNo_Entry_Details_PieceSelection.Focus()

        '    End If


        'Else

        With dgv_PieceSelection

            chk_PieceSelection_SelectAll_Pieces.Checked = False

            .Rows.Clear()
            SNo = 0

            For K = 1 To 2

                ' For vCTCnt = 1 To 5

                'CloTyp_Selc_STS = False

                '    If vCTCnt = 1 Then
                '        If CloTypIdNo = vCTCnt Then
                '            CloTyp_Selc_STS = True
                '        End If

                '    ElseIf vCTCnt = 2 Then
                '        If CloTypIdNo = 1 Or CloTypIdNo = vCTCnt Then
                '            CloTyp_Selc_STS = True
                '        End If

                '    ElseIf vCTCnt = 3 Then
                '        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1267" Then '---- BRT TEXTTILES (SOMANUR)
                '            If CloTypIdNo = 1 Or CloTypIdNo = vCTCnt Then
                '                CloTyp_Selc_STS = True
                '            End If
                '        Else
                '            If CloTypIdNo = vCTCnt Then
                '                CloTyp_Selc_STS = True
                '            End If
                '        End If

                '    ElseIf vCTCnt = 4 Then
                '        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1267" Then '---- BRT TEXTTILES (SOMANUR)
                '            If CloTypIdNo = 1 Or CloTypIdNo = vCTCnt Then
                '                CloTyp_Selc_STS = True
                '            End If
                '        Else
                '            If CloTypIdNo = vCTCnt Then
                '                CloTyp_Selc_STS = True
                '            End If
                '        End If

                '    ElseIf vCTCnt = 5 Then
                '        If CloTypIdNo = vCTCnt Then
                '            CloTyp_Selc_STS = True
                '        End If

                '    End If


                '  If CloTyp_Selc_STS = True Then
                vCTCnt = 1

                If K = 1 Then
                    vSelcCodeCondt = " ( a.PackingSlip_Code_Type" & Trim(Val(vCTCnt)) & " = '" & Trim(NewCode) & "' ) "
                Else
                    vSelcCodeCondt = " ( a.PackingSlip_Code_Type" & Trim(Val(vCTCnt)) & " = '' ) "
                End If

                vMtrsColCondt = " (a.Type" & Trim(Val(vCTCnt)) & "_Meters <> 0) "
                vMtrsColNm = "Type" & Trim(Val(vCTCnt)) & "_Meters"

                vByrOfrMtrsColNm = "BuyerOffer_Passed_Meters_Type" & Trim(Val(vCTCnt))
                vByrOfrCodeColNm = "BuyerOffer_Code_Type" & Trim(Val(vCTCnt))

                vBarCodeColNm = "Checked_Pcs_Barcode_Type" & Trim(Val(vCTCnt))

                Da = New SqlClient.SqlDataAdapter("select a.*, c.Ledger_Name, d.cloth_name from Weaver_ClothReceipt_Piece_Details a " &
                                                            " INNER JOIN Company_Head tZ ON a.company_idno = tZ.company_idno " &
                                                            " LEFT OUTER JOIN Ledger_Head c ON a.Ledger_IdNo = c.Ledger_IdNo " &
                                                            " LEFT OUTER JOIN Cloth_Head d ON a.Cloth_IdNo <> 0 and a.Cloth_IdNo = d.Cloth_IdNo " &
                                                            " Where " & CompIDCondt & IIf(Trim(CompIDCondt) <> "", " and ", "") & vMtrsColCondt & " and " & vSelcCodeCondt & " and " & CloID_Cond & IIf(CloID_Cond <> "", " and ", " ") & "  a.Folding = " & Str(Val(Fld_Perc)) &
                                                            "Order by sl_no ASC,a.Weaver_ClothReceipt_Date, a.for_orderby, a.Weaver_ClothReceipt_No, a.PieceNo_OrderBy ", con)


                Dt1 = New DataTable
                Da.Fill(Dt1)

                If Dt1.Rows.Count > 0 Then

                    For i = 0 To Dt1.Rows.Count - 1

                        n = .Rows.Add()

                        SNo = SNo + 1

                        .Rows(n).Cells(0).Value = Val(SNo)

                        .Rows(n).Cells(1).Value = Dt1.Rows(i).Item("Lot_No").ToString
                        .Rows(n).Cells(2).Value = Dt1.Rows(i).Item("Piece_No").ToString
                        .Rows(n).Cells(3).Value = Common_Procedures.ClothType.Type1

                        If Trim(Dt1.Rows(i).Item(vByrOfrCodeColNm).ToString) <> "" And Val(Dt1.Rows(i).Item(vByrOfrMtrsColNm).ToString) <> 0 Then
                            PcsMtrs = Val(Dt1.Rows(i).Item(vByrOfrMtrsColNm).ToString)
                        Else
                            PcsMtrs = Val(Dt1.Rows(i).Item(vMtrsColNm).ToString)
                        End If

                        .Rows(n).Cells(4).Value = Format(Val(PcsMtrs), "#########0.00")

                        If Val(Dt1.Rows(i).Item("Weight_Meter").ToString) <> 0 Then
                            .Rows(n).Cells(5).Value = Format(Val(PcsMtrs) * Val(Dt1.Rows(i).Item("Weight_Meter").ToString), "#########0.000")
                            .Rows(n).Cells(6).Value = Format(Val(Dt1.Rows(i).Item("Weight_Meter").ToString), "#########0.000")
                        End If
                        .Rows(n).Cells(7).Value = Dt1.Rows(i).Item("Ledger_Name").ToString

                        If K = 1 Then
                            .Rows(n).Cells(8).Value = "1"
                        Else
                            .Rows(n).Cells(8).Value = ""
                        End If

                        .Rows(n).Cells(9).Value = Dt1.Rows(i).Item("Lot_Code").ToString
                        .Rows(n).Cells(10).Value = Dt1.Rows(i).Item("Cloth_Name").ToString

                        vLmIdNo = 0
                        If IsDBNull(Dt1.Rows(i).Item("Loom_IdNo").ToString) = False Then
                            vLmIdNo = Val(Dt1.Rows(i).Item("Loom_IdNo").ToString)
                        End If

                        vLmNo = ""
                        If vLmIdNo <> 0 Then
                            vLmNo = Common_Procedures.Loom_IdNoToName(con, vLmIdNo)

                        Else
                            If IsDBNull(Dt1.Rows(i).Item("Loom_No").ToString) = False Then
                                vLmNo = Dt1.Rows(i).Item("Loom_No").ToString
                            End If

                        End If

                        .Rows(n).Cells(11).Value = vLmNo
                        .Rows(n).Cells(12).Value = Dt1.Rows(i).Item(vBarCodeColNm).ToString

                        vEntBaleNo = get_Piece_BaleNo_from_PieceSelectionDetails_Grid(dgvDet_DetSlNo, .Rows(n).Cells(9).Value, .Rows(n).Cells(2).Value, .Rows(n).Cells(3).Value)

                        .Rows(n).Cells(13).Value = vEntBaleNo

                        If K = 1 Then

                            For j = 0 To .ColumnCount - 1
                                .Rows(i).Cells(j).Style.ForeColor = Color.Red
                            Next j

                        End If

                    Next i

                End If
                Dt1.Clear()

                '   End If

                '     Next vCTCnt

            Next K


        End With

        pnl_PieceSelection.Visible = True
        'pnl_Pieces_BaleNo_Entry_Details.Visible = False
        pnl_Back.Enabled = False
        If dgv_PieceSelection.Rows.Count > 0 Then
            dgv_PieceSelection.Focus()
            dgv_PieceSelection.CurrentCell = dgv_PieceSelection.Rows(0).Cells(dgvCol_PieceSelection.SLNO)
            dgv_PieceSelection.CurrentCell.Selected = True
        Else
            txt_PieceSelection_LotNo.Focus()
        End If



        '  End If




    End Sub
    Private Function get_Piece_BaleNo_from_PieceSelectionDetails_Grid(ByVal dgvDet_DetSlNo As Integer, ByVal vLotCd As String, ByVal vPcsNo As String, ByVal vPcsTypName As String) As String
        Dim vEntBaleNo As String = ""
        Dim vPcsTyp_ID As Integer = 0

        vEntBaleNo = ""

        With dgv_PieceDetails

            For i = 0 To .Rows.Count - 1

                If Val(.Rows(i).Cells(dgvCol_PieceDetails.METERS).Value) <> 0 Then


                    If Val(UCase(.Rows(i).Cells(dgvCol_PieceDetails.CLOTH_PROCESSING_DELIVERY_PACKINGSLIP_SLNO).Value)) = Val(dgvDet_DetSlNo) And Trim(UCase(.Rows(i).Cells(dgvCol_PieceDetails.LOT_CODE).Value)) = Trim(UCase(vLotCd)) And Trim(UCase(.Rows(i).Cells(dgvCol_PieceDetails.PCS_NO).Value)) = Trim(UCase(vPcsNo)) And Trim(UCase(.Rows(i).Cells(dgvCol_PieceDetails.CLOTH_TYPE).Value)) = Trim(UCase(vPcsTypName)) Then

                        vEntBaleNo = Trim(.Rows(i).Cells(dgvCol_PieceDetails.BALE_NO).Value)

                        Exit For

                    End If

                End If

            Next i

        End With

        If Trim(vEntBaleNo) = "" Then

            Dim da2 As New SqlClient.SqlDataAdapter
            Dim dt2 As New DataTable
            Dim NewCode As String

            NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(EntFnYrCode)

            vPcsTyp_ID = Common_Procedures.ClothType_NameToIdNo(con, vPcsTypName)

            da2 = New SqlClient.SqlDataAdapter("Select a.* from Textile_Processing_Delivery_Piece_Details a Where a.ClothProcess_Delivery_Code = '" & Trim(NewCode) & "' and a.ClothProcessing_Delivery_PackingSlno = " & Str(Val(dgvDet_DetSlNo)) & "  and a.Lot_Code = '" & Trim(vLotCd) & "' and a.Piece_No = '" & Trim(vPcsNo) & "' and a.PieceType_IdNo = " & Str(Val(vPcsTyp_ID)) & " and a.Bale_No <> '' Order by a.ClothProcessing_Delivery_PackingSlno, a.sl_no", con)
            dt2 = New DataTable
            da2.Fill(dt2)

            If dt2.Rows.Count > 0 Then
                If IsDBNull(dt2.Rows(0).Item("Bale_No").ToString) = False Then
                    vEntBaleNo = dt2.Rows(0).Item("Bale_No").ToString
                End If
            End If

            dt2.Clear()

        End If


        Return vEntBaleNo


    End Function
    Private Sub btn_Close_PieceSelection_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Close_PieceSelection.Click
        Close_Piece_Selection()
    End Sub

    Private Sub Close_Piece_Selection()
        Dim n As Integer = 0
        Dim sno As Integer = 0
        Dim i As Integer = 0
        Dim dgvDet_CurRow As Integer = 0
        Dim dgvDet_DetSlNo As Integer = 0
        Dim Tot_Pcs As Integer = 0
        Dim Tot_Mtrs As String = 0
        Dim Tot_Wgt As String = 0
        Dim vPcs_Selc_LtCds_PcsNos As String = ""
        Dim NoofBls As Integer
        Dim FsNo As Single, LsNo As Single
        Dim FsBaleNo As String, LsBaleNo As String
        Dim BlNo As String, PackSlpCodes As String
        Dim Da1 As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim Cmd As New SqlClient.SqlCommand
        Dim vCLOTH_STOCKMAINTENANCE_IN As String

        Cmd.Connection = con

        dgvDet_CurRow = dgv_Details.CurrentCell.RowIndex
        dgvDet_DetSlNo = Val(dgv_Details.Rows(dgvDet_CurRow).Cells(dgvCol_Details.CLOTH_PROCESSING_DELIVERY_PACKINGSLIP_SLNO).Value)

        'dgv_Pieces_BaleNo_Entry_Details.Rows.Clear()

LOOP1:
        For i = 0 To dgv_PieceDetails.RowCount - 1

            If Val(dgv_PieceDetails.Rows(i).Cells(dgvCol_PieceDetails.CLOTH_PROCESSING_DELIVERY_PACKINGSLIP_SLNO).Value) = Val(dgvDet_DetSlNo) Then

                If i = dgv_PieceDetails.Rows.Count - 1 Then
                    For J = 0 To dgv_PieceDetails.ColumnCount - 1
                        dgv_PieceDetails.Rows(i).Cells(J).Value = ""
                    Next

                Else
                    dgv_PieceDetails.Rows.RemoveAt(i)

                End If

                GoTo LOOP1

            End If

        Next i


        sno = 0
        Tot_Pcs = 0
        Tot_Mtrs = 0
        vPcs_Selc_LtCds_PcsNos = ""

        Cmd.CommandText = "truncate table " & Trim(Common_Procedures.EntryTempTable) & ""
        Cmd.ExecuteNonQuery()

        For i = 0 To dgv_PieceSelection.RowCount - 1

            If Val(dgv_PieceSelection.Rows(i).Cells(dgvCol_PieceSelection.STS).Value) = 1 Then

                n = dgv_PieceDetails.Rows.Add()

                dgv_PieceDetails.Rows(n).Cells(dgvCol_PieceDetails.CLOTH_PROCESSING_DELIVERY_PACKINGSLIP_SLNO).Value = Val(dgvDet_DetSlNo)
                dgv_PieceDetails.Rows(n).Cells(dgvCol_PieceDetails.LOT_NO).Value = dgv_PieceSelection.Rows(i).Cells(dgvCol_PieceSelection.LOT_NO).Value
                dgv_PieceDetails.Rows(n).Cells(dgvCol_PieceDetails.PCS_NO).Value = dgv_PieceSelection.Rows(i).Cells(dgvCol_PieceSelection.PCS_NO).Value
                dgv_PieceDetails.Rows(n).Cells(dgvCol_PieceDetails.CLOTH_TYPE).Value = dgv_PieceSelection.Rows(i).Cells(dgvCol_PieceSelection.CLOTH_TYPE).Value
                dgv_PieceDetails.Rows(n).Cells(dgvCol_PieceDetails.METERS).Value = dgv_PieceSelection.Rows(i).Cells(dgvCol_PieceSelection.METERS).Value



                If Val(dgv_PieceSelection.Rows(i).Cells(dgvCol_PieceSelection.WEIGHT).Value) <> 0 Then
                    dgv_PieceDetails.Rows(n).Cells(dgvCol_PieceDetails.WEIGHT).Value = dgv_PieceSelection.Rows(i).Cells(dgvCol_PieceSelection.WEIGHT).Value
                End If
                If Val(dgv_PieceSelection.Rows(i).Cells(dgvCol_PieceSelection.WEIGHT_METER).Value) <> 0 Then
                    dgv_PieceDetails.Rows(n).Cells(dgvCol_PieceDetails.WEIGHT_METER).Value = dgv_PieceSelection.Rows(i).Cells(dgvCol_PieceSelection.WEIGHT_METER).Value
                End If
                dgv_PieceDetails.Rows(n).Cells(dgvCol_PieceDetails.PCS_PARTY_NAME).Value = dgv_PieceSelection.Rows(i).Cells(dgvCol_PieceSelection.PCS_PARTY_NAME).Value
                dgv_PieceDetails.Rows(n).Cells(dgvCol_PieceDetails.LOT_CODE).Value = dgv_PieceSelection.Rows(i).Cells(dgvCol_PieceSelection.LOT_CODE).Value
                dgv_PieceDetails.Rows(n).Cells(dgvCol_PieceDetails.CLOTH_NAME).Value = dgv_PieceSelection.Rows(i).Cells(dgvCol_PieceSelection.CLOTH_IDNO).Value
                dgv_PieceDetails.Rows(n).Cells(dgvCol_PieceDetails.LOOM_NO).Value = dgv_PieceSelection.Rows(i).Cells(dgvCol_PieceSelection.LOOM_NO).Value
                dgv_PieceDetails.Rows(n).Cells(dgvCol_PieceDetails.BALE_NO).Value = dgv_PieceSelection.Rows(i).Cells(dgvCol_PieceSelection.ENTRY_BALE_NO).Value


                Cmd.CommandText = "Insert into " & Trim(Common_Procedures.EntryTempTable) & "(Name1, Name2, Meters1) values ('" & Trim(dgv_PieceSelection.Rows(i).Cells(dgvCol_PieceSelection.LOT_CODE).Value) & "', '" & Trim(dgv_PieceSelection.Rows(i).Cells(dgvCol_PieceSelection.PCS_NO).Value) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(dgv_PieceSelection.Rows(i).Cells(dgvCol_PieceSelection.PCS_NO).Value))) & " ) "
                Cmd.ExecuteNonQuery()

                Tot_Pcs = Tot_Pcs + 1
                Tot_Mtrs = Val(Tot_Mtrs) + Val(dgv_PieceSelection.Rows(i).Cells(dgvCol_PieceSelection.METERS).Value)
                Tot_Wgt = Val(Tot_Wgt) + Val(dgv_PieceSelection.Rows(i).Cells(dgvCol_PieceSelection.WEIGHT).Value)
                vPcs_Selc_LtCds_PcsNos = Trim(vPcs_Selc_LtCds_PcsNos) & IIf(Trim(vPcs_Selc_LtCds_PcsNos) = "", "~", "") & Trim(dgv_PieceSelection.Rows(i).Cells(dgvCol_PieceSelection.LOT_CODE).Value) & "|" & Trim(dgv_PieceSelection.Rows(i).Cells(dgvCol_PieceSelection.PCS_NO).Value) & "~"

                'If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1186" And Trim(UCase(cbo_RollBundle.Text)) = "BALE" Then

                '    sno = sno + 1
                '    n = dgv_Pieces_BaleNo_Entry_Details.Rows.Add()
                '    dgv_Pieces_BaleNo_Entry_Details.Rows(n).Cells(0).Value = sno
                '    dgv_Pieces_BaleNo_Entry_Details.Rows(n).Cells(1).Value = dgv_PieceSelection.Rows(i).Cells(1).Value
                '    dgv_Pieces_BaleNo_Entry_Details.Rows(n).Cells(2).Value = dgv_PieceSelection.Rows(i).Cells(2).Value
                '    dgv_Pieces_BaleNo_Entry_Details.Rows(n).Cells(3).Value = dgv_PieceSelection.Rows(i).Cells(3).Value
                '    dgv_Pieces_BaleNo_Entry_Details.Rows(n).Cells(4).Value = dgv_PieceSelection.Rows(i).Cells(4).Value
                '    If Val(dgv_PieceSelection.Rows(i).Cells(5).Value) <> 0 Then
                '        dgv_Pieces_BaleNo_Entry_Details.Rows(n).Cells(5).Value = dgv_PieceSelection.Rows(i).Cells(5).Value
                '    End If
                '    If Val(dgv_PieceSelection.Rows(i).Cells(6).Value) <> 0 Then
                '        dgv_Pieces_BaleNo_Entry_Details.Rows(n).Cells(6).Value = dgv_PieceSelection.Rows(i).Cells(6).Value
                '    End If
                '    dgv_Pieces_BaleNo_Entry_Details.Rows(n).Cells(7).Value = dgv_PieceSelection.Rows(i).Cells(7).Value
                '    dgv_Pieces_BaleNo_Entry_Details.Rows(n).Cells(8).Value = dgv_PieceSelection.Rows(i).Cells(9).Value
                '    dgv_Pieces_BaleNo_Entry_Details.Rows(n).Cells(9).Value = dgv_PieceSelection.Rows(i).Cells(10).Value
                '    dgv_Pieces_BaleNo_Entry_Details.Rows(n).Cells(10).Value = dgv_PieceSelection.Rows(i).Cells(11).Value
                '    dgv_Pieces_BaleNo_Entry_Details.Rows(n).Cells(11).Value = dgv_PieceSelection.Rows(i).Cells(13).Value

                'End If


            End If

        Next i


        BlNo = ""
        FsNo = 0 : LsNo = 0
        FsBaleNo = "" : LsBaleNo = ""

        Da1 = New SqlClient.SqlDataAdapter("Select Name1 as Lot_Code, Name2 as Pcs_No, Meters1 as fororderby_PcsNo from " & Trim(Common_Procedures.EntryTempTable) & " where Name1 <> '' order by Meters1, Name2, Name1", con)
        Dt1 = New DataTable
        Da1.Fill(Dt1)

        If Dt1.Rows.Count > 0 Then

            FsNo = Val(Dt1.Rows(0).Item("fororderby_PcsNo").ToString)
            LsNo = Val(Dt1.Rows(0).Item("fororderby_PcsNo").ToString)

            FsBaleNo = Trim(UCase(Dt1.Rows(0).Item("Pcs_No").ToString))
            LsBaleNo = Trim(UCase(Dt1.Rows(0).Item("Pcs_No").ToString))

            For i = 1 To Dt1.Rows.Count - 1
                If LsNo + 1 = Val(Dt1.Rows(i).Item("fororderby_PcsNo").ToString) Then
                    LsNo = Val(Dt1.Rows(i).Item("fororderby_PcsNo").ToString)
                    LsBaleNo = Trim(UCase(Dt1.Rows(i).Item("Pcs_No").ToString))

                Else
                    If FsNo = LsNo Then
                        BlNo = BlNo & Trim(FsBaleNo) & ","
                    Else
                        BlNo = BlNo & Trim(FsBaleNo) & "-" & Trim(LsBaleNo) & ","
                    End If
                    FsNo = Dt1.Rows(i).Item("fororderby_PcsNo").ToString
                    LsNo = Dt1.Rows(i).Item("fororderby_PcsNo").ToString

                    FsBaleNo = Trim(UCase(Dt1.Rows(i).Item("Pcs_No").ToString))
                    LsBaleNo = Trim(UCase(Dt1.Rows(i).Item("Pcs_No").ToString))

                End If

            Next

            If FsNo = LsNo Then BlNo = BlNo & Trim(FsBaleNo) Else BlNo = BlNo & Trim(FsBaleNo) & "-" & Trim(LsBaleNo)

        End If
        Dt1.Clear()


        If Trim(dgv_Details.Rows(dgvDet_CurRow).Cells(dgvCol_Details.PIECE_SELECTION_LOTCODE_PCSNOS).Value) <> "" Then
            dgv_Details.Rows(dgvDet_CurRow).Cells(dgvCol_Details.BALES_NOS).Value = ""
            dgv_Details.Rows(dgvDet_CurRow).Cells(dgvCol_Details.PCS).Value = ""
            dgv_Details.Rows(dgvDet_CurRow).Cells(dgvCol_Details.METERS).Value = ""
            dgv_Details.Rows(dgvDet_CurRow).Cells(dgvCol_Details.WEIGHT).Value = ""
            dgv_Details.Rows(dgvDet_CurRow).Cells(dgvCol_Details.PIECE_SELECTION_LOTCODE_PCSNOS).Value = ""
        End If


        If Val(Tot_Pcs) <> 0 And Val(Tot_Mtrs) <> 0 Then

            dgv_Details.Rows(dgvDet_CurRow).Cells(dgvCol_Details.BALES_NOS).Value = BlNo


            Check_Cloth_Stock_Maintanance_In_Pcs_Mtr(vCLOTH_STOCKMAINTENANCE_IN)

            If Trim(UCase(vCLOTH_STOCKMAINTENANCE_IN)) = "PCS" Then

                If Val(Tot_Mtrs) <> 0 Then

                    dgv_Details.Rows(dgvDet_CurRow).Cells(dgvCol_Details.PCS).Value = Format(Val(Tot_Mtrs), "#########0.00")

                End If

                dgv_Details.Rows(dgvDet_CurRow).Cells(dgvCol_Details.METERS).Value = 0

                Else


                If Val(Tot_Pcs) <> 0 Then
                    dgv_Details.Rows(dgvDet_CurRow).Cells(dgvCol_Details.PCS).Value = Val(Tot_Pcs)
                End If

                If Val(Tot_Mtrs) <> 0 Then
                    dgv_Details.Rows(dgvDet_CurRow).Cells(dgvCol_Details.METERS).Value = Format(Val(Tot_Mtrs), "#########0.00")
                End If

            End If

            If Val(Tot_Wgt) <> 0 Then
                    dgv_Details.Rows(dgvDet_CurRow).Cells(dgvCol_Details.WEIGHT).Value = Format(Val(Tot_Wgt), "#########0.000")
                End If

                dgv_Details.Rows(dgvDet_CurRow).Cells(dgvCol_Details.PIECE_SELECTION_LOTCODE_PCSNOS).Value = vPcs_Selc_LtCds_PcsNos

            End If

            Total_Calculation()

        'If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1186" And Trim(UCase(cbo_RollBundle.Text)) = "BALE" And Val(Tot_Mtrs) <> 0 Then

        '    pnl_Back.Enabled = False
        '    pnl_Pieces_BaleNo_Entry_Details.Visible = True
        '    pnl_PieceSelection.Visible = False
        '    If dgv_Pieces_BaleNo_Entry_Details.Enabled And dgv_Pieces_BaleNo_Entry_Details.Visible Then
        '        If dgv_Pieces_BaleNo_Entry_Details.Rows.Count > 0 Then
        '            dgv_Pieces_BaleNo_Entry_Details.Focus()
        '            If dgv_Pieces_BaleNo_Entry_Details.CurrentCell.RowIndex >= 0 Then
        '                dgv_Pieces_BaleNo_Entry_Details.CurrentCell = dgv_Pieces_BaleNo_Entry_Details.Rows(0).Cells(11)
        '                dgv_Pieces_BaleNo_Entry_Details.CurrentCell.Selected = True
        '            End If
        '        End If

        '    Else

        '        btn_Pieces_BaleNo_Entry_Details_PieceSelection.Focus()

        '    End If

        'Else

        pnl_Back.Enabled = True
        pnl_PieceSelection.Visible = False
        If dgv_Details.Enabled And dgv_Details.Visible Then
            If dgv_Details.Rows.Count > 0 Then
                dgv_Details.Focus()
                If dgv_Details.CurrentCell.RowIndex >= 0 Then
                    dgv_Details.CurrentCell = dgv_Details.Rows(dgv_Details.CurrentCell.RowIndex).Cells(dgvCol_Details.PCS)
                    dgv_Details.CurrentCell.Selected = True
                End If
            End If


        Else
            cbo_TransportName.Focus()

        End If

        '  End If

    End Sub


    Private Sub Check_Cloth_Stock_Maintanance_In_Pcs_Mtr(ByRef vCLOTH_STOCKMAINTENANCE_IN As String)

        Dim Cmd As New SqlClient.SqlCommand
        Dim Da1 As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable

        vCLOTH_STOCKMAINTENANCE_IN = ""

        Dim vCLOID As Integer

        vCLOID = Common_Procedures.Cloth_NameToIdNo(con, dgv_Details.Rows(dgv_Details.CurrentCell.RowIndex).Cells(dgvCol_Details.ITEM_GREY).Value)


        Da1 = New SqlClient.SqlDataAdapter("Select * from Cloth_Head Where Cloth_Idno = " & Str(Val(vCLOID)) & " ", con)
        Dt1 = New DataTable
        Da1.Fill(Dt1)

        If Dt1.Rows.Count > 0 Then
            vCLOTH_STOCKMAINTENANCE_IN = Dt1.Rows(0).Item("Stock_in").ToString
        End If

    End Sub

    Private Sub cbo_ClothSales_OrderCode_forSelection_KeyPress(sender As Object, e As KeyPressEventArgs) Handles cbo_ClothSales_OrderCode_forSelection.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, sender, Nothing, "ClothSales_Order_Head", "ClothSales_OrderCode_forSelection", vCLO_CONDT, "(ClothSales_Order_Code = '999999/00-00')")

        If Asc(e.KeyChar) = 13 Then

            If dgv_Details.Rows.Count > 0 Then
                dgv_Details.Focus()
                dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(dgvCol_Details.ITEM_GREY)
            End If
        End If

    End Sub

    Private Sub cbo_ClothSales_OrderCode_forSelection_KeyDown(sender As Object, e As KeyEventArgs) Handles cbo_ClothSales_OrderCode_forSelection.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, sender, txt_Folding, cbo_itemgrey, "ClothSales_Order_Head", "ClothSales_OrderCode_forSelection", vCLO_CONDT, "(ClothSales_Order_Code = '999999/00-00')")

        If (e.KeyValue = 40 And sender.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
            If dgv_Details.Rows.Count > 0 Then
                dgv_Details.Focus()
                dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(dgvCol_Details.ITEM_GREY)
            End If
        End If


        If (e.KeyValue = 38 And sender.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then

            If txt_Frieght.Visible = True Then
                txt_Frieght.Focus()
            Else
                cbo_Delivery_From.Focus()
            End If

        End If


    End Sub

    Private Sub cbo_ClothSales_OrderCode_forSelection_Enter(sender As Object, e As EventArgs) Handles cbo_ClothSales_OrderCode_forSelection.Enter
        vCLO_CONDT = "(ClothSales_Order_Code LIKE '%/" & Trim(FnYearCode1) & "' or ClothSales_Order_Code LIKE '%/" & Trim(FnYearCode2) & "' and Order_Close_Status = 0 )"
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "ClothSales_Order_Head", "ClothSales_OrderCode_forSelection", vCLO_CONDT, "(ClothSales_Order_Code = '999999/00-00')")
    End Sub

    Private Sub Printing_Format1558(ByRef e As System.Drawing.Printing.PrintPageEventArgs)

        Dim cmd As New SqlClient.SqlCommand
        Dim Da1 As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim EntryCode As String
        Dim I As Integer, NoofDets As Integer, NoofItems_PerPage As Integer
        Dim pFont As Font
        Dim p1Font As Font
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
        Dim clrName As String = ""
        Dim Clrln As Integer = 0
        'PrintDocument pd = new PrintDocument();
        'pd.DefaultPageSettings.PaperSize = new System.Drawing.Printing.PaperSize("PaperA4", 840, 1180);
        'pd.Print();

        'For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
        '    ps = PrintDocument1.PrinterSettings.PaperSizes(I)
        '    Debug.Print(ps.PaperName)
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

        '    If PpSzSTS = False Then
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
            .Left = 20
            .Right = 55
            .Top = 35
            .Bottom = 35
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

        NoofItems_PerPage = 4 ' 6

        Erase LnAr
        Erase ClArr

        LnAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        ClArr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}

        ClArr(1) = Val(30) : ClArr(2) = 160 : ClArr(3) = 80 : ClArr(4) = 160 : ClArr(5) = 70 : ClArr(6) = 70 : ClArr(7) = 80
        ClArr(8) = PageWidth - (LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7))

        TxtHgt = 16 '18 ' 19 ' e.Graphics.MeasureString("A", pFont).Height  ' 20

        EntryCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(EntFnYrCode)

        Try

            If prn_HdDt.Rows.Count > 0 Then

                Printing_Format1558_PageHeader(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, prn_PageNo, NoofItems_PerPage, CurY, LnAr, ClArr)


                W1 = e.Graphics.MeasureString("No.of Beams  : ", pFont).Width

                NoofDets = 0

                CurY = CurY - 10

                If prn_DetDt.Rows.Count > 0 Then

                    Do While prn_DetIndx <= prn_DetDt.Rows.Count - 1

                        If NoofDets >= NoofItems_PerPage Then
                            CurY = CurY + TxtHgt

                            Common_Procedures.Print_To_PrintDocument(e, "Continued....", PageWidth - 10, CurY, 1, 0, pFont)

                            NoofDets = NoofDets + 1

                            Printing_Format1558_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, NoofDets, False)

                            e.HasMorePages = True
                            Return

                        End If

                        prn_DetSNo = prn_DetSNo + 1

                        ItmNm1 = Trim(prn_DetDt.Rows(prn_DetIndx).Item("Processed_Grey_Name").ToString)
                        ItmNm2 = ""
                        If Len(ItmNm1) > 25 Then
                            For I = 25 To 1 Step -1
                                If Mid$(Trim(ItmNm1), I, 1) = " " Or Mid$(Trim(ItmNm1), I, 1) = "," Or Mid$(Trim(ItmNm1), I, 1) = "." Or Mid$(Trim(ItmNm1), I, 1) = "-" Or Mid$(Trim(ItmNm1), I, 1) = "/" Or Mid$(Trim(ItmNm1), I, 1) = "_" Or Mid$(Trim(ItmNm1), I, 1) = "(" Or Mid$(Trim(ItmNm1), I, 1) = ")" Or Mid$(Trim(ItmNm1), I, 1) = "\" Or Mid$(Trim(ItmNm1), I, 1) = "[" Or Mid$(Trim(ItmNm1), I, 1) = "]" Or Mid$(Trim(ItmNm1), I, 1) = "{" Or Mid$(Trim(ItmNm1), I, 1) = "}" Then Exit For
                            Next I
                            If I = 0 Then I = 25
                            ItmNm2 = Microsoft.VisualBasic.Right(Trim(ItmNm1), Len(ItmNm1) - I)
                            ItmNm1 = Microsoft.VisualBasic.Left(Trim(ItmNm1), I - 1)
                        End If

                        Dim ClrNm1 As String, ClrNm2 As String

                        ClrNm1 = Common_Procedures.Colour_IdNoToName(con, Val(prn_DetDt.Rows(prn_DetIndx).Item("Colour_Idno").ToString))
                        ClrNm2 = ""
                        If Len(ClrNm1) > 12 Then
                            For I = 12 To 1 Step -1
                                If Mid$(Trim(ClrNm1), I, 1) = "@" Or Mid$(Trim(ClrNm1), I, 1) = " " Or Mid$(Trim(ClrNm1), I, 1) = "," Or Mid$(Trim(ClrNm1), I, 1) = "." Or Mid$(Trim(ClrNm1), I, 1) = "-" Or Mid$(Trim(ClrNm1), I, 1) = "/" Or Mid$(Trim(ClrNm1), I, 1) = "_" Or Mid$(Trim(ClrNm1), I, 1) = "(" Or Mid$(Trim(ClrNm1), I, 1) = ")" Or Mid$(Trim(ClrNm1), I, 1) = "\" Or Mid$(Trim(ClrNm1), I, 1) = "[" Or Mid$(Trim(ClrNm1), I, 1) = "]" Or Mid$(Trim(ClrNm1), I, 1) = "{" Or Mid$(Trim(ClrNm1), I, 1) = "}" Or Mid$(Trim(ClrNm1), I, 1) = "@" Then Exit For
                            Next I
                            If I = 0 Then I = 12
                            ClrNm2 = Microsoft.VisualBasic.Right(Trim(ClrNm1), Len(ClrNm1) - I)
                            ClrNm1 = Microsoft.VisualBasic.Left(Trim(ClrNm1), I)
                        End If

                        '----------------------------

                        Dim ItmNmP1 As String, ItmNmP2 As String
                        Dim ClrNmP1 As String, ClrNmP2 As String

                        ItmNmP1 = Trim(prn_DetDt.Rows(prn_DetIndx).Item("Processed_Cloth_Name").ToString)
                        ItmNmP2 = ""
                        If Len(ItmNmP1) > 25 Then
                            For I = 25 To 1 Step -1
                                If Mid$(Trim(ItmNmP1), I, 1) = " " Or Mid$(Trim(ItmNmP1), I, 1) = "," Or Mid$(Trim(ItmNmP1), I, 1) = "." Or Mid$(Trim(ItmNmP1), I, 1) = "-" Or Mid$(Trim(ItmNmP1), I, 1) = "/" Or Mid$(Trim(ItmNmP1), I, 1) = "_" Or Mid$(Trim(ItmNmP1), I, 1) = "(" Or Mid$(Trim(ItmNmP1), I, 1) = ")" Or Mid$(Trim(ItmNmP1), I, 1) = "\" Or Mid$(Trim(ItmNmP1), I, 1) = "[" Or Mid$(Trim(ItmNmP1), I, 1) = "]" Or Mid$(Trim(ItmNmP1), I, 1) = "{" Or Mid$(Trim(ItmNmP1), I, 1) = "}" Then Exit For
                            Next I
                            If I = 0 Then I = 25
                            ItmNmP2 = Microsoft.VisualBasic.Right(Trim(ItmNmP1), Len(ItmNmP1) - I)
                            ItmNmP1 = Microsoft.VisualBasic.Left(Trim(ItmNmP1), I - 1)
                        End If

                        'Dim ClrNmP1 As String, ClrNmP2 As String

                        ClrNmP1 = Trim(prn_DetDt.Rows(prn_DetIndx).Item("DEL_COLOUR_NAME").ToString)
                        ClrNmP2 = ""
                        If Len(ClrNmP1) > 12 Then
                            For I = 12 To 1 Step -1
                                If Mid$(Trim(ClrNmP1), I, 1) = "@" Or Mid$(Trim(ClrNmP1), I, 1) = " " Or Mid$(Trim(ClrNmP1), I, 1) = "," Or Mid$(Trim(ClrNmP1), I, 1) = "." Or Mid$(Trim(ClrNmP1), I, 1) = "-" Or Mid$(Trim(ClrNmP1), I, 1) = "/" Or Mid$(Trim(ClrNmP1), I, 1) = "_" Or Mid$(Trim(ClrNmP1), I, 1) = "(" Or Mid$(Trim(ClrNmP1), I, 1) = ")" Or Mid$(Trim(ClrNmP1), I, 1) = "\" Or Mid$(Trim(ClrNmP1), I, 1) = "[" Or Mid$(Trim(ClrNmP1), I, 1) = "]" Or Mid$(Trim(ClrNmP1), I, 1) = "{" Or Mid$(Trim(ClrNmP1), I, 1) = "}" Or Mid$(Trim(ClrNmP1), I, 1) = "@" Then Exit For
                            Next I
                            If I = 0 Then I = 12
                            ClrNmP2 = Microsoft.VisualBasic.Right(Trim(ClrNmP1), Len(ClrNmP1) - I)
                            ClrNmP1 = Microsoft.VisualBasic.Left(Trim(ClrNmP1), I)
                        End If


                        CurY = CurY + TxtHgt
                        SNo = SNo + 1
                        Common_Procedures.Print_To_PrintDocument(e, Trim(Val(SNo)), LMargin + 10, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm1), LMargin + ClArr(1) + 10, CurY, 0, 0, pFont)

                        p1Font = New Font("Calibri", 8, FontStyle.Regular)
                        Common_Procedures.Print_To_PrintDocument(e, ClrNm1, LMargin + ClArr(1) + ClArr(2) + 10, CurY, 0, 0, p1Font)

                        Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNmP1), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + 10, CurY, 0, 0, pFont)
                        'Common_Procedures.Print_To_PrintDocument(e, ClrNmP1, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + 10, CurY, 0, 0, p1Font)

                        Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Bales").ToString, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) - 10, CurY, 1, 0, pFont)

                        If Val(prn_DetDt.Rows(prn_DetIndx).Item("Delivery_Pcs").ToString) <> 0 Then
                            Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(prn_DetIndx).Item("Delivery_Pcs").ToString), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) - 10, CurY, 1, 0, pFont)
                        End If
                        If Val(prn_DetDt.Rows(prn_DetIndx).Item("Delivery_Meters").ToString) <> 0 Then
                            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Delivery_Meters").ToString), "#########0.00"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) - 10, CurY, 1, 0, pFont)
                        End If

                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Delivery_Weight").ToString), "#########0.000"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) - 10, CurY, 1, 0, pFont)

                        NoofDets = NoofDets + 1

                        If Trim(ItmNm2) <> "" Or Trim(ClrNm2) <> "" Or Trim(ItmNmP2) <> "" Or Trim(ClrNmP2) <> "" Then
                            CurY = CurY + TxtHgt - 5
                            Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm2), LMargin + ClArr(1) + 10, CurY, 0, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, ClrNm2, LMargin + ClArr(1) + ClArr(2) + 10, CurY, 0, 0, p1Font)
                            Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNmP2), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + 10, CurY, 0, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, ClrNmP2, LMargin + ClArr(1) + ClArr(2) + 10, CurY, 0, 0, p1Font)
                            NoofDets = NoofDets + 1
                        End If

                        prn_DetIndx = prn_DetIndx + 1

                    Loop

                End If


                Printing_Format1558_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, NoofDets, True)

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

            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

        e.HasMorePages = False

    End Sub

    Private Sub Printing_Format1558_PageHeader(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByRef PageNo As Integer, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single)
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt2 As New DataTable
        Dim p1Font As Font
        Dim strHeight As Single
        Dim C1 As Single, W1 As Single, S1 As Single
        Dim Cmp_Name As String, Cmp_Add1 As String, Cmp_Add2 As String
        Dim Cmp_PhNo As String, Cmp_TinNo As String, Cmp_CstNo As String
        Dim Cmp_GSTIN_Cap As String, Cmp_GSTIN_No As String
        Dim S As String

        PageNo = PageNo + 1

        CurY = TMargin

        da2 = New SqlClient.SqlDataAdapter("select a.*, b.Processed_Item_Name as Processed_Grey_Name, c.Colour_Name ,d.Lot_No ,e.Process_Name  from Textile_Processing_Delivery_Details a INNER JOIN Processed_Item_Head b on a.Item_IdNo = b.Processed_Item_IdNo LEFT OUTER JOIN Colour_Head c on a.Colour_IdNo = c.Colour_IdNo LEFT OUTER JOIN Lot_Head d ON d.Lot_IdNo = a.Lot_IdNo LEFT OUTER JOIN Process_Head e ON e.Process_IdNo = a.Processing_Idno where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Cloth_Processing_Delivery_Code = '" & Trim(EntryCode) & "' Order by a.sl_no", con)
        da2.Fill(dt2)

        If dt2.Rows.Count > NoofItems_PerPage Then
            Common_Procedures.Print_To_PrintDocument(e, "Page : " & Trim(Val(PageNo)), PageWidth - 10, CurY - TxtHgt, 1, 0, pFont)
        End If
        dt2.Clear()

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
                    If Trim(prn_InpOpts) <> "0" And Val(prn_InpOpts) = "0" And Len(Trim(prn_InpOpts)) > 4 Then
                        prn_OriDupTri = Trim(prn_InpOpts)
                    End If
                End If

            End If
        End If
        If Trim(prn_OriDupTri) <> "" Then
            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_OriDupTri), PageWidth - 10, CurY - TxtHgt, 1, 0, pFont)
        End If

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
        If Trim(prn_HdDt.Rows(0).Item("Company_GSTinNo").ToString) <> "" Then
            Cmp_GSTIN_No = "GSTIN : " & prn_HdDt.Rows(0).Item("Company_GSTinNo").ToString
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
        Common_Procedures.Print_To_PrintDocument(e, Cmp_PhNo, LMargin - 70, CurY, 2, PrintWidth, pFont)
        'CurY = CurY + TxtHgt - 1
        'Common_Procedures.Print_To_PrintDocument(e, Cmp_TinNo, LMargin + 10, CurY, 0, 0, pFont)
        'Common_Procedures.Print_To_PrintDocument(e, Cmp_CstNo, PageWidth - 10, CurY, 1, 0, pFont)
        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, Cmp_GSTIN_No, LMargin, CurY, 2, PrintWidth, pFont)

        CurY = CurY + TxtHgt - 5
        p1Font = New Font("Calibri", 16, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "PROCESSING DELIVERY", LMargin, CurY, 2, PrintWidth, p1Font)
        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height


        Common_Procedures.Print_To_PrintDocument(e, "(Not For Sale)", LMargin + 10, CurY, 0, 0, pFont)



        CurY = CurY + strHeight  ' + 150
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(2) = CurY

        Try
            C1 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4)
            W1 = e.Graphics.MeasureString("PROCESSING  : ", pFont).Width
            S1 = e.Graphics.MeasureString("TO :    ", pFont).Width

            CurY = CurY + TxtHgt - 10
            p1Font = New Font("Calibri", 12, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "TO  :  " & "M/s." & prn_HdDt.Rows(0).Item("Ledger_Name").ToString, LMargin + 10, CurY, 0, 0, p1Font)
            Common_Procedures.Print_To_PrintDocument(e, "DC.NO", LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("ClothProcess_Delivery_No").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, p1Font)

            CurY = CurY + TxtHgt + 5
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address1").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
            p1Font = New Font("Calibri", 14, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "DATE", LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("ClothProcess_Delivery_Date").ToString), "dd-MM-yyyy").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt + 5
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address2").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)

            Common_Procedures.Print_To_PrintDocument(e, "LOT NO", LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("JobOrder_No").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt + 5
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address3").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)

            Dim vProcess_Nm1 = ""
            Dim vProcess_Nm2 = ""
            Dim i = 0

            vProcess_Nm1 = Trim(prn_HdDt.Rows(0).Item("Process_Name").ToString)
            If Trim(vProcess_Nm1) <> "" Then

                If Len(vProcess_Nm1) > 15 Then

                    For i = 20 To 1 Step -1

                        If Mid$(Trim(vProcess_Nm1), i, 1) = "@" Or Mid$(Trim(vProcess_Nm1), i, 1) = " " Or Mid$(Trim(vProcess_Nm1), i, 1) = "," Or Mid$(Trim(vProcess_Nm1), i, 1) = "." Or Mid$(Trim(vProcess_Nm1), i, 1) = "-" Or Mid$(Trim(vProcess_Nm1), i, 1) = "/" Or Mid$(Trim(vProcess_Nm1), i, 1) = "_" Or Mid$(Trim(vProcess_Nm1), i, 1) = "(" Or Mid$(Trim(vProcess_Nm1), i, 1) = ")" Or Mid$(Trim(vProcess_Nm1), i, 1) = "\" Or Mid$(Trim(vProcess_Nm1), i, 1) = "[" Or Mid$(Trim(vProcess_Nm1), i, 1) = "]" Or Mid$(Trim(vProcess_Nm1), i, 1) = "{" Or Mid$(Trim(vProcess_Nm1), i, 1) = "}" Or Mid$(Trim(vProcess_Nm1), i, 1) = "@" Then Exit For
                    Next i
                    If i = 0 Then i = 15

                    vProcess_Nm2 = Microsoft.VisualBasic.Right(Trim(vProcess_Nm1), Len(vProcess_Nm1) - i)
                    vProcess_Nm1 = Microsoft.VisualBasic.Left(Trim(vProcess_Nm1), i)



                End If
            End If

            Common_Procedures.Print_To_PrintDocument(e, "PROCESSING", LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Trim(vProcess_Nm1), LMargin + C1 + W1 + 30, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt + 5
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address4").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
            If Trim(vProcess_Nm2) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, Trim(vProcess_Nm2), LMargin + C1 + W1 + 30, CurY, 0, 0, pFont)
            Else
                Common_Procedures.Print_To_PrintDocument(e, "E-Way Bill No", LMargin + C1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("EwayBill_No").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, pFont)
            End If
            CurY = CurY + TxtHgt + 5
            p1Font = New Font("Calibri", 11, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "GSTIN : " & prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString, LMargin + S1 + 10, CurY, 0, 0, p1Font)

            If Trim(vProcess_Nm2) <> "" And prn_HdDt.Rows(0).Item("EwayBill_No").ToString <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, "E-Way Bill No", LMargin + C1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("EwayBill_No").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, pFont)
            End If

            If Trim(prn_HdDt.Rows(0).Item("Purchase_OrderNo").ToString) <> "" Then
                CurY = CurY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, "P.O.NO", LMargin + C1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Purchase_OrderNo").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, pFont)
            End If

            CurY = CurY + TxtHgt + 5
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(3) = CurY

            e.Graphics.DrawLine(Pens.Black, LMargin + C1, LnAr(3), LMargin + C1, LnAr(2))

            CurY = CurY + TxtHgt - 10
            Common_Procedures.Print_To_PrintDocument(e, "SNO", LMargin, CurY, 2, ClAr(1), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "ITEM NAME", LMargin + ClAr(1), CurY, 2, ClAr(2), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "COLOUR", LMargin + ClAr(1) + ClAr(2), CurY, 2, ClAr(3), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "PRODUCT", LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, 2, ClAr(4), pFont)
            'Common_Procedures.Print_To_PrintDocument(e, "COLOUR", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 2, ClAr(5), pFont)

            Common_Procedures.Print_To_PrintDocument(e, "BALE", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 2, ClAr(5), pFont)
            'Common_Procedures.Print_To_PrintDocument(e, "BALES NOS", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 2, ClAr(5), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "PCS", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, 2, ClAr(6), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "METERS", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, 2, ClAr(7), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "WEIGHT", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, 2, ClAr(8), pFont)


            CurY = CurY + TxtHgt + 5
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(4) = CurY

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub Printing_Format1558_PageFooter(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single, ByVal NoofDets As Integer, ByVal is_LastPage As Boolean)
        Dim p1Font As Font
        Dim I As Integer
        Dim Cmp_Name As String
        Dim W1 As Single = 0
        Dim vprn_BlNos As String = ""
        Dim BLNo1 As String
        Dim BLNo2 As String
        Dim NoteStr1 As String = ""
        Dim NoteStr2 As String = ""
        Dim vTxamt As String = 0
        Dim vNtAMt As String = 0
        Dim vTxPerc As String
        Dim vSgst_amt As String = 0
        Dim vCgst_amt As String = 0
        Dim vIgst_amt As String = 0
        Dim vChk_GST_Bill As Integer = 0
        Dim C1 As Single
        Dim W2 As Single
        Dim W3 As Single

        W1 = e.Graphics.MeasureString("No.of Beams  : ", pFont).Width

        Try



            For I = NoofDets + 1 To NoofItems_PerPage

                CurY = CurY + TxtHgt



                prn_DetIndx = prn_DetIndx + 1

            Next

            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(5) = CurY

            CurY = CurY + TxtHgt - 10
            If is_LastPage = True Then
                Common_Procedures.Print_To_PrintDocument(e, " TOTAL", LMargin + ClAr(1) + 10, CurY, 2, ClAr(2), pFont)
            End If

            If Val(prn_HdDt.Rows(0).Item("Total_Pcs").ToString) <> 0 Then
                If is_LastPage = True Then
                    Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(0).Item("Total_Pcs").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 5, CurY, 1, 0, pFont)
                End If
            End If

            If Val(prn_HdDt.Rows(0).Item("Total_Weight").ToString) <> 0 Then
                If is_LastPage = True Then
                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Total_Weight").ToString), "#########0.000"), PageWidth - 10, CurY, 1, 0, pFont)
                End If
            End If

            If Val(prn_HdDt.Rows(0).Item("Total_Meters").ToString) <> 0 Then
                If is_LastPage = True Then
                    ' Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Total_Meters").ToString), "#########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY, 1, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Total_Meters").ToString), "#########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, pFont)
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
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), LnAr(3))

            CurY = CurY + TxtHgt - 5

            vTxPerc = 0
            vCgst_amt = 0
            vSgst_amt = 0
            vIgst_amt = 0
            ' 

            vChk_GST_Bill = Val(prn_HdDt.Rows(0).Item("GST_Tax_Invoice_Status").ToString)

            If Val(vChk_GST_Bill) = 1 Then



                If Val(prn_HdDt.Rows(0).Item("Company_State_IdNo").ToString) = Val(prn_HdDt.Rows(0).Item("Ledger_State_IdNo").ToString) Then

                    vTxPerc = Format(Val(prn_DetDt.Rows(0).Item("item_gst_percentage").ToString) / 2, "############0.00")

                    vCgst_amt = Format(Val(prn_HdDt.Rows(0).Item("Net_Amount").ToString) * Val(vTxPerc) / 100, "############0.00")
                    vSgst_amt = Format(Val(prn_HdDt.Rows(0).Item("Net_Amount").ToString) * Val(vTxPerc) / 100, "############0.00")

                Else

                    vTxPerc = prn_DetDt.Rows(0).Item("item_gst_percentage").ToString
                    vIgst_amt = Format(Val(prn_HdDt.Rows(0).Item("Net_Amount").ToString) * Val(vTxPerc) / 100, "############0.00")

                End If
            End If

            W1 = e.Graphics.MeasureString("Transport Name:", pFont).Width
            W2 = e.Graphics.MeasureString("CGST @ 2.5%:", pFont).Width
            W3 = e.Graphics.MeasureString("Value Of Goods :", pFont).Width

            C1 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(3)


            vprn_BlNos = ""
            For I = 0 To prn_DetDt.Rows.Count - 1
                If Trim(prn_DetDt.Rows(I).Item("Bales_Nos").ToString) <> "" Then
                    vprn_BlNos = Trim(vprn_BlNos) & IIf(Trim(vprn_BlNos) <> "", ", ", "") & prn_DetDt.Rows(I).Item("Bales_Nos").ToString
                End If
            Next

            ' CurY = CurY + TxtHgt
            BLNo1 = Trim(vprn_BlNos)
            BLNo2 = ""
            If Len(BLNo1) > 90 Then
                For I = 90 To 1 Step -1
                    If Mid$(Trim(BLNo1), I, 1) = " " Or Mid$(Trim(BLNo1), I, 1) = "," Or Mid$(Trim(BLNo1), I, 1) = "." Or Mid$(Trim(BLNo1), I, 1) = "-" Or Mid$(Trim(BLNo1), I, 1) = "/" Or Mid$(Trim(BLNo1), I, 1) = "_" Or Mid$(Trim(BLNo1), I, 1) = "(" Or Mid$(Trim(BLNo1), I, 1) = ")" Or Mid$(Trim(BLNo1), I, 1) = "\" Or Mid$(Trim(BLNo1), I, 1) = "[" Or Mid$(Trim(BLNo1), I, 1) = "]" Or Mid$(Trim(BLNo1), I, 1) = "{" Or Mid$(Trim(BLNo1), I, 1) = "}" Then Exit For
                Next I
                If I = 0 Then I = 90
                BLNo2 = Microsoft.VisualBasic.Right(Trim(BLNo1), Len(BLNo1) - I)
                BLNo1 = Microsoft.VisualBasic.Left(Trim(BLNo1), I)
            End If

            If Trim(BLNo1) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, "Bale/Bundle No : " & BLNo1, LMargin + 10, CurY, 0, 0, pFont)
            End If

            If Val(prn_HdDt.Rows(0).Item("Net_Amount").ToString) <> 0 And Val(vChk_GST_Bill) = 1 Then
                Common_Procedures.Print_To_PrintDocument(e, "Taxable Amount", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + W3 - 15, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Net_Amount").ToString), "##########0.00"), PageWidth - 10, CurY, 1, 0, pFont)
            Else
                If Val(prn_HdDt.Rows(0).Item("Net_Amount").ToString) <> 0 Then
                    Common_Procedures.Print_To_PrintDocument(e, "Value Of Goods", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + W3 - 15, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Net_Amount").ToString), "##########0.00"), PageWidth - 10, CurY, 1, 0, pFont)
                End If
            End If


            CurY = CurY + TxtHgt

            If Trim(BLNo2) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, Space(Len("Bale/Bundle No : ")) & BLNo2, LMargin + 10, CurY, 0, 0, pFont)
            End If
            If Val(prn_HdDt.Rows(0).Item("Net_Amount").ToString) <> 0 And Val(vChk_GST_Bill) = 1 Then

                If Val(vIgst_amt) <> 0 Then

                    Common_Procedures.Print_To_PrintDocument(e, "IGST @ " & Val(vTxPerc) & " %", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(6) + ClAr(7) + 10 + W3, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(vIgst_amt), "##########0.00"), PageWidth - 10, CurY, 1, 0, pFont)

                Else

                    Common_Procedures.Print_To_PrintDocument(e, "CGST @ " & Val(vTxPerc) & " %", LMargin + C1, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W2, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(vCgst_amt), "##########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + 50, CurY, 1, 0, pFont)

                    Common_Procedures.Print_To_PrintDocument(e, "SGST @ " & Val(vTxPerc) & " %", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + W3 - 15, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(vSgst_amt), "##########0.00"), PageWidth - 10, CurY, 1, 0, pFont)

                End If

            End If

            CurY = CurY + TxtHgt
            If Trim(prn_HdDt.Rows(0).Item("Vehicle_No").ToString) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, "Vehicle No : " & Trim(prn_HdDt.Rows(0).Item("Vehicle_No").ToString), LMargin + 10, CurY, 0, 0, pFont)
            End If

            If Val(prn_HdDt.Rows(0).Item("Net_Amount").ToString) <> 0 And Val(vChk_GST_Bill) = 1 Then
                vNtAMt = Format(Val(prn_HdDt.Rows(0).Item("Net_Amount").ToString) + Val(vCgst_amt) + Val(vSgst_amt) + Val(vIgst_amt), "###########0")

                Common_Procedures.Print_To_PrintDocument(e, "Value Of Goods", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + W3 - 15, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, Format(Val(vNtAMt), "###########0.00"), PageWidth - 10, CurY, 1, 0, pFont)

            End If

            CurY = CurY + TxtHgt + 10
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(7) = CurY

            If Trim(prn_HdDt.Rows(0).Item("Note").ToString) <> "" Then

                CurY = CurY + TxtHgt
                p1Font = New Font("Calibri", 9, FontStyle.Regular)
                NoteStr1 = "( Note: " & Trim(prn_HdDt.Rows(0).Item("Note").ToString) & " )"
                If Len(NoteStr1) > 90 Then
                    For I = 90 To 1 Step -1
                        If Mid$(Trim(NoteStr1), I, 1) = " " Or Mid$(Trim(NoteStr1), I, 1) = "," Or Mid$(Trim(NoteStr1), I, 1) = "." Or Mid$(Trim(NoteStr1), I, 1) = "-" Or Mid$(Trim(NoteStr1), I, 1) = "/" Or Mid$(Trim(NoteStr1), I, 1) = "_" Or Mid$(Trim(NoteStr1), I, 1) = "(" Or Mid$(Trim(NoteStr1), I, 1) = ")" Or Mid$(Trim(NoteStr1), I, 1) = "\" Or Mid$(Trim(NoteStr1), I, 1) = "[" Or Mid$(Trim(NoteStr1), I, 1) = "]" Or Mid$(Trim(NoteStr1), I, 1) = "{" Or Mid$(Trim(NoteStr1), I, 1) = "}" Then Exit For
                    Next I
                    If I = 0 Then I = 90
                    NoteStr2 = Microsoft.VisualBasic.Right(Trim(NoteStr1), Len(NoteStr1) - I)
                    NoteStr1 = Microsoft.VisualBasic.Left(Trim(NoteStr1), I)
                End If
                Common_Procedures.Print_To_PrintDocument(e, NoteStr1, LMargin + 10, CurY, 0, 0, p1Font)
                If NoteStr2 <> "" Then
                    CurY = CurY + TxtHgt
                    Common_Procedures.Print_To_PrintDocument(e, NoteStr2, LMargin + 10, CurY, 0, 0, p1Font)
                End If

            End If

            If Val(Common_Procedures.User.IdNo) <> 1 Then
                Common_Procedures.Print_To_PrintDocument(e, "(" & Trim(Common_Procedures.User.Name) & ")", LMargin + 400, CurY, 0, 0, pFont)
            End If

            CurY = CurY + TxtHgt
            CurY = CurY + TxtHgt
            Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString
            Common_Procedures.Print_To_PrintDocument(e, "Receiver's Signature", LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "Prepared By ", LMargin + 300, CurY, 0, 0, pFont)
            p1Font = New Font("Calibri", 12, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "For " & Cmp_Name, PageWidth - 15, CurY, 1, 0, p1Font)

            CurY = CurY + TxtHgt + 5

            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            e.Graphics.DrawLine(Pens.Black, LMargin, LnAr(1), LMargin, CurY)
            e.Graphics.DrawLine(Pens.Black, PageWidth, LnAr(1), PageWidth, CurY)

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub


    Private Sub Printing_Format1061(ByRef e As System.Drawing.Printing.PrintPageEventArgs)

        Dim cmd As New SqlClient.SqlCommand
        Dim Da1 As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim EntryCode As String
        Dim I As Integer, NoofDets As Integer, NoofItems_PerPage As Integer
        Dim pFont As Font
        Dim p1Font As Font
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
        Dim clrName As String = ""
        Dim Clrln As Integer = 0
        'PrintDocument pd = new PrintDocument();
        'pd.DefaultPageSettings.PaperSize = new System.Drawing.Printing.PaperSize("PaperA4", 840, 1180);
        'pd.Print();

        'For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
        '    ps = PrintDocument1.PrinterSettings.PaperSizes(I)
        '    Debug.Print(ps.PaperName)
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

        '    If PpSzSTS = False Then
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
            .Left = 20
            .Right = 55
            .Top = 35
            .Bottom = 35
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

        NoofItems_PerPage = 4 ' 6

        Erase LnAr
        Erase ClArr

        LnAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        ClArr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}

        ClArr(1) = Val(30) : ClArr(2) = 160 : ClArr(3) = 80 : ClArr(4) = 160 : ClArr(5) = 70 : ClArr(6) = 70 : ClArr(7) = 80
        ClArr(8) = PageWidth - (LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7))

        TxtHgt = 16 '18 ' 19 ' e.Graphics.MeasureString("A", pFont).Height  ' 20

        EntryCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(EntFnYrCode)

        Try

            If prn_HdDt.Rows.Count > 0 Then

                Printing_Format1061_PageHeader(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, prn_PageNo, NoofItems_PerPage, CurY, LnAr, ClArr)


                W1 = e.Graphics.MeasureString("No.of Beams  : ", pFont).Width

                NoofDets = 0

                CurY = CurY - 10

                If prn_DetDt.Rows.Count > 0 Then

                    Do While prn_DetIndx <= prn_DetDt.Rows.Count - 1

                        If NoofDets >= NoofItems_PerPage Then
                            CurY = CurY + TxtHgt

                            Common_Procedures.Print_To_PrintDocument(e, "Continued....", PageWidth - 10, CurY, 1, 0, pFont)

                            NoofDets = NoofDets + 1

                            Printing_Format1061_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, NoofDets, False)

                            e.HasMorePages = True
                            Return

                        End If

                        prn_DetSNo = prn_DetSNo + 1

                        ItmNm1 = Trim(prn_DetDt.Rows(prn_DetIndx).Item("Processed_Grey_Name").ToString)
                        ItmNm2 = ""
                        If Len(ItmNm1) > 20 Then
                            For I = 20 To 1 Step -1
                                If Mid$(Trim(ItmNm1), I, 1) = " " Or Mid$(Trim(ItmNm1), I, 1) = "," Or Mid$(Trim(ItmNm1), I, 1) = "." Or Mid$(Trim(ItmNm1), I, 1) = "-" Or Mid$(Trim(ItmNm1), I, 1) = "/" Or Mid$(Trim(ItmNm1), I, 1) = "_" Or Mid$(Trim(ItmNm1), I, 1) = "(" Or Mid$(Trim(ItmNm1), I, 1) = ")" Or Mid$(Trim(ItmNm1), I, 1) = "\" Or Mid$(Trim(ItmNm1), I, 1) = "[" Or Mid$(Trim(ItmNm1), I, 1) = "]" Or Mid$(Trim(ItmNm1), I, 1) = "{" Or Mid$(Trim(ItmNm1), I, 1) = "}" Then Exit For
                            Next I
                            If I = 0 Then I = 20
                            ItmNm2 = Microsoft.VisualBasic.Right(Trim(ItmNm1), Len(ItmNm1) - I)
                            ItmNm1 = Microsoft.VisualBasic.Left(Trim(ItmNm1), I - 1)
                        End If

                        Dim ClrNm1 As String, ClrNm2 As String

                        ClrNm1 = Common_Procedures.Colour_IdNoToName(con, Val(prn_DetDt.Rows(prn_DetIndx).Item("Colour_Idno").ToString))
                        ClrNm2 = ""
                        If Len(ClrNm1) > 12 Then
                            For I = 12 To 1 Step -1
                                If Mid$(Trim(ClrNm1), I, 1) = "@" Or Mid$(Trim(ClrNm1), I, 1) = " " Or Mid$(Trim(ClrNm1), I, 1) = "," Or Mid$(Trim(ClrNm1), I, 1) = "." Or Mid$(Trim(ClrNm1), I, 1) = "-" Or Mid$(Trim(ClrNm1), I, 1) = "/" Or Mid$(Trim(ClrNm1), I, 1) = "_" Or Mid$(Trim(ClrNm1), I, 1) = "(" Or Mid$(Trim(ClrNm1), I, 1) = ")" Or Mid$(Trim(ClrNm1), I, 1) = "\" Or Mid$(Trim(ClrNm1), I, 1) = "[" Or Mid$(Trim(ClrNm1), I, 1) = "]" Or Mid$(Trim(ClrNm1), I, 1) = "{" Or Mid$(Trim(ClrNm1), I, 1) = "}" Or Mid$(Trim(ClrNm1), I, 1) = "@" Then Exit For
                            Next I
                            If I = 0 Then I = 12
                            ClrNm2 = Microsoft.VisualBasic.Right(Trim(ClrNm1), Len(ClrNm1) - I)
                            ClrNm1 = Microsoft.VisualBasic.Left(Trim(ClrNm1), I)
                        End If

                        '----------------------------

                        Dim ItmNmP1 As String, ItmNmP2 As String
                        Dim ClrNmP1 As String, ClrNmP2 As String

                        ItmNmP1 = Trim(prn_DetDt.Rows(prn_DetIndx).Item("Processed_Cloth_Name").ToString)
                        ItmNmP2 = ""
                        If Len(ItmNmP1) > 20 Then
                            For I = 20 To 1 Step -1
                                If Mid$(Trim(ItmNmP1), I, 1) = " " Or Mid$(Trim(ItmNmP1), I, 1) = "," Or Mid$(Trim(ItmNmP1), I, 1) = "." Or Mid$(Trim(ItmNmP1), I, 1) = "-" Or Mid$(Trim(ItmNmP1), I, 1) = "/" Or Mid$(Trim(ItmNmP1), I, 1) = "_" Or Mid$(Trim(ItmNmP1), I, 1) = "(" Or Mid$(Trim(ItmNmP1), I, 1) = ")" Or Mid$(Trim(ItmNmP1), I, 1) = "\" Or Mid$(Trim(ItmNmP1), I, 1) = "[" Or Mid$(Trim(ItmNmP1), I, 1) = "]" Or Mid$(Trim(ItmNmP1), I, 1) = "{" Or Mid$(Trim(ItmNmP1), I, 1) = "}" Then Exit For
                            Next I
                            If I = 0 Then I = 20
                            ItmNmP2 = Microsoft.VisualBasic.Right(Trim(ItmNmP1), Len(ItmNmP1) - I)
                            ItmNmP1 = Microsoft.VisualBasic.Left(Trim(ItmNmP1), I - 1)
                        End If

                        'Dim ClrNmP1 As String, ClrNmP2 As String

                        ClrNmP1 = Trim(prn_DetDt.Rows(prn_DetIndx).Item("DEL_COLOUR_NAME").ToString)
                        ClrNmP2 = ""
                        If Len(ClrNmP1) > 12 Then
                            For I = 12 To 1 Step -1
                                If Mid$(Trim(ClrNmP1), I, 1) = "@" Or Mid$(Trim(ClrNmP1), I, 1) = " " Or Mid$(Trim(ClrNmP1), I, 1) = "," Or Mid$(Trim(ClrNmP1), I, 1) = "." Or Mid$(Trim(ClrNmP1), I, 1) = "-" Or Mid$(Trim(ClrNmP1), I, 1) = "/" Or Mid$(Trim(ClrNmP1), I, 1) = "_" Or Mid$(Trim(ClrNmP1), I, 1) = "(" Or Mid$(Trim(ClrNmP1), I, 1) = ")" Or Mid$(Trim(ClrNmP1), I, 1) = "\" Or Mid$(Trim(ClrNmP1), I, 1) = "[" Or Mid$(Trim(ClrNmP1), I, 1) = "]" Or Mid$(Trim(ClrNmP1), I, 1) = "{" Or Mid$(Trim(ClrNmP1), I, 1) = "}" Or Mid$(Trim(ClrNmP1), I, 1) = "@" Then Exit For
                            Next I
                            If I = 0 Then I = 12
                            ClrNmP2 = Microsoft.VisualBasic.Right(Trim(ClrNmP1), Len(ClrNmP1) - I)
                            ClrNmP1 = Microsoft.VisualBasic.Left(Trim(ClrNmP1), I)
                        End If


                        CurY = CurY + TxtHgt
                        SNo = SNo + 1
                        Common_Procedures.Print_To_PrintDocument(e, Trim(Val(SNo)), LMargin + 10, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm1), LMargin + ClArr(1) + 10, CurY, 0, 0, pFont)

                        p1Font = New Font("Calibri", 8, FontStyle.Regular)
                        Common_Procedures.Print_To_PrintDocument(e, ClrNm1, LMargin + ClArr(1) + ClArr(2) + 10, CurY, 0, 0, p1Font)

                        Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNmP1), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + 10, CurY, 0, 0, pFont)
                        'Common_Procedures.Print_To_PrintDocument(e, ClrNmP1, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + 10, CurY, 0, 0, p1Font)

                        Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Bales").ToString, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) - 10, CurY, 1, 0, pFont)

                        If Val(prn_DetDt.Rows(prn_DetIndx).Item("Delivery_Pcs").ToString) <> 0 Then
                            Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(prn_DetIndx).Item("Delivery_Pcs").ToString), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) - 10, CurY, 1, 0, pFont)
                        End If
                        If Val(prn_DetDt.Rows(prn_DetIndx).Item("Delivery_Meters").ToString) <> 0 Then
                            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Delivery_Meters").ToString), "#########0.00"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) - 10, CurY, 1, 0, pFont)
                        End If

                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Delivery_Weight").ToString), "#########0.000"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) - 10, CurY, 1, 0, pFont)

                        NoofDets = NoofDets + 1

                        If Trim(ItmNm2) <> "" Or Trim(ClrNm2) <> "" Or Trim(ItmNmP2) <> "" Or Trim(ClrNmP2) <> "" Then
                            CurY = CurY + TxtHgt - 5
                            Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm2), LMargin + ClArr(1) + 10, CurY, 0, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, ClrNm2, LMargin + ClArr(1) + ClArr(2) + 10, CurY, 0, 0, p1Font)
                            Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNmP2), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + 10, CurY, 0, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, ClrNmP2, LMargin + ClArr(1) + ClArr(2) + 10, CurY, 0, 0, p1Font)
                            NoofDets = NoofDets + 1
                        End If

                        prn_DetIndx = prn_DetIndx + 1

                    Loop

                End If


                Printing_Format1061_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, NoofDets, True)

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

            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

        e.HasMorePages = False

    End Sub

    Private Sub Printing_Format1061_PageHeader(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByRef PageNo As Integer, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single)
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt2 As New DataTable
        Dim p1Font As Font
        Dim strHeight As Single
        Dim C1 As Single, W1 As Single, S1 As Single
        Dim Cmp_Name As String, Cmp_Add1 As String, Cmp_Add2 As String
        Dim Cmp_PhNo As String, Cmp_TinNo As String, Cmp_CstNo As String
        Dim Cmp_GSTIN_Cap As String, Cmp_GSTIN_No As String
        Dim S As String

        PageNo = PageNo + 1

        CurY = TMargin

        da2 = New SqlClient.SqlDataAdapter("select a.*, b.Processed_Item_Name as Processed_Grey_Name, c.Colour_Name ,d.Lot_No ,e.Process_Name  from Textile_Processing_Delivery_Details a INNER JOIN Processed_Item_Head b on a.Item_IdNo = b.Processed_Item_IdNo LEFT OUTER JOIN Colour_Head c on a.Colour_IdNo = c.Colour_IdNo LEFT OUTER JOIN Lot_Head d ON d.Lot_IdNo = a.Lot_IdNo LEFT OUTER JOIN Process_Head e ON e.Process_IdNo = a.Processing_Idno where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Cloth_Processing_Delivery_Code = '" & Trim(EntryCode) & "' Order by a.sl_no", con)
        da2.Fill(dt2)

        If dt2.Rows.Count > NoofItems_PerPage Then
            Common_Procedures.Print_To_PrintDocument(e, "Page : " & Trim(Val(PageNo)), PageWidth - 10, CurY - TxtHgt, 1, 0, pFont)
        End If
        dt2.Clear()

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
                    If Trim(prn_InpOpts) <> "0" And Val(prn_InpOpts) = "0" And Len(Trim(prn_InpOpts)) > 4 Then
                        prn_OriDupTri = Trim(prn_InpOpts)
                    End If
                End If

            End If
        End If
        If Trim(prn_OriDupTri) <> "" Then
            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_OriDupTri), PageWidth - 10, CurY - TxtHgt, 1, 0, pFont)
        End If

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
        If Trim(prn_HdDt.Rows(0).Item("Company_GSTinNo").ToString) <> "" Then
            Cmp_GSTIN_No = "GST NO.: " & prn_HdDt.Rows(0).Item("Company_GSTinNo").ToString
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
        Common_Procedures.Print_To_PrintDocument(e, Cmp_PhNo, LMargin - 70, CurY, 2, PrintWidth, pFont)
        'CurY = CurY + TxtHgt - 1
        'Common_Procedures.Print_To_PrintDocument(e, Cmp_TinNo, LMargin + 10, CurY, 0, 0, pFont)
        'Common_Procedures.Print_To_PrintDocument(e, Cmp_CstNo, PageWidth - 10, CurY, 1, 0, pFont)
        'CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, Cmp_GSTIN_No, PageWidth - 160, CurY, 1, 0, pFont)

        CurY = CurY + TxtHgt - 5
        p1Font = New Font("Calibri", 16, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "PROCESSING DELIVERY", LMargin, CurY, 2, PrintWidth, p1Font)
        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height



        CurY = CurY + strHeight  ' + 150
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(2) = CurY

        Try
            C1 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5)
            W1 = e.Graphics.MeasureString("PROCESSING  : ", pFont).Width
            S1 = e.Graphics.MeasureString("TO :    ", pFont).Width

            CurY = CurY + TxtHgt - 10
            p1Font = New Font("Calibri", 12, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "TO  :  " & "M/s." & prn_HdDt.Rows(0).Item("Ledger_Name").ToString, LMargin + 10, CurY, 0, 0, p1Font)
            Common_Procedures.Print_To_PrintDocument(e, "DC.NO", LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("ClothProcess_Delivery_No").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, p1Font)

            CurY = CurY + TxtHgt + 5
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address1").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
            p1Font = New Font("Calibri", 14, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "DATE", LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("ClothProcess_Delivery_Date").ToString), "dd-MM-yyyy").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt + 5
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address2").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)

            Common_Procedures.Print_To_PrintDocument(e, "LOT NO", LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("JobOrder_No").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt + 5
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address3").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)

            Dim vProcess_Nm1 = ""
            Dim vProcess_Nm2 = ""
            Dim i = 0

            vProcess_Nm1 = Trim(prn_HdDt.Rows(0).Item("Process_Name").ToString)
            If Trim(vProcess_Nm1) <> "" Then

                If Len(vProcess_Nm1) > 15 Then

                    For i = 20 To 1 Step -1

                        If Mid$(Trim(vProcess_Nm1), i, 1) = "@" Or Mid$(Trim(vProcess_Nm1), i, 1) = " " Or Mid$(Trim(vProcess_Nm1), i, 1) = "," Or Mid$(Trim(vProcess_Nm1), i, 1) = "." Or Mid$(Trim(vProcess_Nm1), i, 1) = "-" Or Mid$(Trim(vProcess_Nm1), i, 1) = "/" Or Mid$(Trim(vProcess_Nm1), i, 1) = "_" Or Mid$(Trim(vProcess_Nm1), i, 1) = "(" Or Mid$(Trim(vProcess_Nm1), i, 1) = ")" Or Mid$(Trim(vProcess_Nm1), i, 1) = "\" Or Mid$(Trim(vProcess_Nm1), i, 1) = "[" Or Mid$(Trim(vProcess_Nm1), i, 1) = "]" Or Mid$(Trim(vProcess_Nm1), i, 1) = "{" Or Mid$(Trim(vProcess_Nm1), i, 1) = "}" Or Mid$(Trim(vProcess_Nm1), i, 1) = "@" Then Exit For
                    Next i
                    If i = 0 Then i = 15

                    vProcess_Nm2 = Microsoft.VisualBasic.Right(Trim(vProcess_Nm1), Len(vProcess_Nm1) - i)
                    vProcess_Nm1 = Microsoft.VisualBasic.Left(Trim(vProcess_Nm1), i)



                End If
            End If

            Common_Procedures.Print_To_PrintDocument(e, "PROCESSING", LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Trim(vProcess_Nm1), LMargin + C1 + W1 + 30, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt + 5
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address4").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
            If Trim(vProcess_Nm2) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, Trim(vProcess_Nm2), LMargin + C1 + W1 + 30, CurY, 0, 0, pFont)
            Else
                Common_Procedures.Print_To_PrintDocument(e, "E-Way Bill No", LMargin + C1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("EwayBill_No").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, pFont)
            End If
            CurY = CurY + TxtHgt + 5
            p1Font = New Font("Calibri", 11, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "GST No: " & prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString, LMargin + S1 + 10, CurY, 0, 0, p1Font)

            If Trim(vProcess_Nm2) <> "" And prn_HdDt.Rows(0).Item("EwayBill_No").ToString <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, "E-Way Bill No", LMargin + C1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("EwayBill_No").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, pFont)
            End If

            If Trim(prn_HdDt.Rows(0).Item("Purchase_OrderNo").ToString) <> "" Then
                CurY = CurY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, "P.O.NO", LMargin + C1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Purchase_OrderNo").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, pFont)
            End If

            CurY = CurY + TxtHgt + 5
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(3) = CurY

            e.Graphics.DrawLine(Pens.Black, LMargin + C1, LnAr(3), LMargin + C1, LnAr(2))

            CurY = CurY + TxtHgt - 10
            Common_Procedures.Print_To_PrintDocument(e, "SNO", LMargin, CurY, 2, ClAr(1), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "ITEM NAME", LMargin + ClAr(1), CurY, 2, ClAr(2), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "COLOUR", LMargin + ClAr(1) + ClAr(2), CurY, 2, ClAr(3), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "PRODUCT", LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, 2, ClAr(4), pFont)
            'Common_Procedures.Print_To_PrintDocument(e, "COLOUR", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 2, ClAr(5), pFont)

            Common_Procedures.Print_To_PrintDocument(e, "BALE", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 2, ClAr(5), pFont)
            'Common_Procedures.Print_To_PrintDocument(e, "BALES NOS", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 2, ClAr(5), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "PCS", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, 2, ClAr(6), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "METERS", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, 2, ClAr(7), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "WEIGHT", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, 2, ClAr(8), pFont)


            CurY = CurY + TxtHgt + 5
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(4) = CurY

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub Printing_Format1061_PageFooter(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single, ByVal NoofDets As Integer, ByVal is_LastPage As Boolean)
        Dim p1Font As Font
        Dim I As Integer
        Dim Cmp_Name As String
        Dim W1 As Single = 0
        Dim vprn_BlNos As String = ""
        Dim BLNo1 As String
        Dim BLNo2 As String
        Dim NoteStr1 As String = ""
        Dim NoteStr2 As String = ""
        Dim vTxamt As String = 0
        Dim vNtAMt As String = 0
        Dim vTxPerc As String
        Dim vSgst_amt As String = 0
        Dim vCgst_amt As String = 0
        Dim vIgst_amt As String = 0
        Dim vChk_GST_Bill As Integer = 0
        Dim C1 As Single
        Dim W2 As Single
        Dim W3 As Single

        W1 = e.Graphics.MeasureString("No.of Beams  : ", pFont).Width

        Try



            For I = NoofDets + 1 To NoofItems_PerPage

                CurY = CurY + TxtHgt



                prn_DetIndx = prn_DetIndx + 1

            Next

            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(5) = CurY

            CurY = CurY + TxtHgt - 10
            If is_LastPage = True Then
                Common_Procedures.Print_To_PrintDocument(e, " TOTAL", LMargin + ClAr(1) + 10, CurY, 2, ClAr(2), pFont)
            End If

            If Val(prn_HdDt.Rows(0).Item("Total_Pcs").ToString) <> 0 Then
                If is_LastPage = True Then
                    Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(0).Item("Total_Pcs").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 5, CurY, 1, 0, pFont)
                End If
            End If

            If Val(prn_HdDt.Rows(0).Item("Total_Weight").ToString) <> 0 Then
                If is_LastPage = True Then
                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Total_Weight").ToString), "#########0.000"), PageWidth - 10, CurY, 1, 0, pFont)
                End If
            End If

            If Val(prn_HdDt.Rows(0).Item("Total_Meters").ToString) <> 0 Then
                If is_LastPage = True Then
                    ' Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Total_Meters").ToString), "#########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY, 1, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Total_Meters").ToString), "#########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, pFont)
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
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), LnAr(3))

            CurY = CurY + TxtHgt - 5

            vTxPerc = 0
            vCgst_amt = 0
            vSgst_amt = 0
            vIgst_amt = 0
            ' 

            vChk_GST_Bill = Val(prn_HdDt.Rows(0).Item("GST_Tax_Invoice_Status").ToString)

            If Val(vChk_GST_Bill) = 1 Then



                If Val(prn_HdDt.Rows(0).Item("Company_State_IdNo").ToString) = Val(prn_HdDt.Rows(0).Item("Ledger_State_IdNo").ToString) Then

                    vTxPerc = Format(Val(prn_DetDt.Rows(0).Item("item_gst_percentage").ToString) / 2, "############0.00")

                    vCgst_amt = Format(Val(prn_HdDt.Rows(0).Item("Net_Amount").ToString) * Val(vTxPerc) / 100, "############0.00")
                    vSgst_amt = Format(Val(prn_HdDt.Rows(0).Item("Net_Amount").ToString) * Val(vTxPerc) / 100, "############0.00")

                Else

                    vTxPerc = prn_DetDt.Rows(0).Item("item_gst_percentage").ToString
                    vIgst_amt = Format(Val(prn_HdDt.Rows(0).Item("Net_Amount").ToString) * Val(vTxPerc) / 100, "############0.00")

                End If
            End If

            W1 = e.Graphics.MeasureString("Transport Name:", pFont).Width
            W2 = e.Graphics.MeasureString("CGST @ 2.5%:", pFont).Width
            W3 = e.Graphics.MeasureString("Value Of Goods :", pFont).Width

            C1 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(3)


            vprn_BlNos = ""
            For I = 0 To prn_DetDt.Rows.Count - 1
                If Trim(prn_DetDt.Rows(I).Item("Bales_Nos").ToString) <> "" Then
                    vprn_BlNos = Trim(vprn_BlNos) & IIf(Trim(vprn_BlNos) <> "", ", ", "") & prn_DetDt.Rows(I).Item("Bales_Nos").ToString
                End If
            Next

            ' CurY = CurY + TxtHgt
            BLNo1 = Trim(vprn_BlNos)
            BLNo2 = ""
            If Len(BLNo1) > 90 Then
                For I = 90 To 1 Step -1
                    If Mid$(Trim(BLNo1), I, 1) = " " Or Mid$(Trim(BLNo1), I, 1) = "," Or Mid$(Trim(BLNo1), I, 1) = "." Or Mid$(Trim(BLNo1), I, 1) = "-" Or Mid$(Trim(BLNo1), I, 1) = "/" Or Mid$(Trim(BLNo1), I, 1) = "_" Or Mid$(Trim(BLNo1), I, 1) = "(" Or Mid$(Trim(BLNo1), I, 1) = ")" Or Mid$(Trim(BLNo1), I, 1) = "\" Or Mid$(Trim(BLNo1), I, 1) = "[" Or Mid$(Trim(BLNo1), I, 1) = "]" Or Mid$(Trim(BLNo1), I, 1) = "{" Or Mid$(Trim(BLNo1), I, 1) = "}" Then Exit For
                Next I
                If I = 0 Then I = 90
                BLNo2 = Microsoft.VisualBasic.Right(Trim(BLNo1), Len(BLNo1) - I)
                BLNo1 = Microsoft.VisualBasic.Left(Trim(BLNo1), I)
            End If

            If Trim(BLNo1) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, "Bale/Bundle No : " & BLNo1, LMargin + 10, CurY, 0, 0, pFont)
            End If

            If Val(prn_HdDt.Rows(0).Item("Net_Amount").ToString) <> 0 And Val(vChk_GST_Bill) = 1 Then
                Common_Procedures.Print_To_PrintDocument(e, "Taxable Amount", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + W3 - 15, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Net_Amount").ToString), "##########0.00"), PageWidth - 10, CurY, 1, 0, pFont)
            Else
                If Val(prn_HdDt.Rows(0).Item("Net_Amount").ToString) <> 0 Then
                    Common_Procedures.Print_To_PrintDocument(e, "Value Of Goods", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + W3 - 15, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Net_Amount").ToString), "##########0.00"), PageWidth - 10, CurY, 1, 0, pFont)
                End If
            End If


            CurY = CurY + TxtHgt

            If Trim(BLNo2) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, Space(Len("Bale/Bundle No : ")) & BLNo2, LMargin + 10, CurY, 0, 0, pFont)
            End If
            If Val(prn_HdDt.Rows(0).Item("Net_Amount").ToString) <> 0 And Val(vChk_GST_Bill) = 1 Then

                If Val(vIgst_amt) <> 0 Then

                    Common_Procedures.Print_To_PrintDocument(e, "IGST @ " & Val(vTxPerc) & " %", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(6) + ClAr(7) + 10 + W3, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(vIgst_amt), "##########0.00"), PageWidth - 10, CurY, 1, 0, pFont)

                Else

                    Common_Procedures.Print_To_PrintDocument(e, "CGST @ " & Val(vTxPerc) & " %", LMargin + C1, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W2, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(vCgst_amt), "##########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + 50, CurY, 1, 0, pFont)

                    Common_Procedures.Print_To_PrintDocument(e, "SGST @ " & Val(vTxPerc) & " %", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + W3 - 15, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(vSgst_amt), "##########0.00"), PageWidth - 10, CurY, 1, 0, pFont)

                End If

            End If

            CurY = CurY + TxtHgt
            If Trim(prn_HdDt.Rows(0).Item("Vehicle_No").ToString) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, "Vehicle No : " & Trim(prn_HdDt.Rows(0).Item("Vehicle_No").ToString), LMargin + 10, CurY, 0, 0, pFont)
            End If

            If Val(prn_HdDt.Rows(0).Item("Net_Amount").ToString) <> 0 And Val(vChk_GST_Bill) = 1 Then
                vNtAMt = Format(Val(prn_HdDt.Rows(0).Item("Net_Amount").ToString) + Val(vCgst_amt) + Val(vSgst_amt) + Val(vIgst_amt), "###########0")

                Common_Procedures.Print_To_PrintDocument(e, "Value Of Goods", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + W3 - 15, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, Format(Val(vNtAMt), "###########0.00"), PageWidth - 10, CurY, 1, 0, pFont)

            End If

            CurY = CurY + TxtHgt + 10
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(7) = CurY

            If Trim(prn_HdDt.Rows(0).Item("Note").ToString) <> "" Then

                CurY = CurY + TxtHgt
                p1Font = New Font("Calibri", 9, FontStyle.Regular)
                NoteStr1 = "( Note: " & Trim(prn_HdDt.Rows(0).Item("Note").ToString) & " )"
                If Len(NoteStr1) > 90 Then
                    For I = 90 To 1 Step -1
                        If Mid$(Trim(NoteStr1), I, 1) = " " Or Mid$(Trim(NoteStr1), I, 1) = "," Or Mid$(Trim(NoteStr1), I, 1) = "." Or Mid$(Trim(NoteStr1), I, 1) = "-" Or Mid$(Trim(NoteStr1), I, 1) = "/" Or Mid$(Trim(NoteStr1), I, 1) = "_" Or Mid$(Trim(NoteStr1), I, 1) = "(" Or Mid$(Trim(NoteStr1), I, 1) = ")" Or Mid$(Trim(NoteStr1), I, 1) = "\" Or Mid$(Trim(NoteStr1), I, 1) = "[" Or Mid$(Trim(NoteStr1), I, 1) = "]" Or Mid$(Trim(NoteStr1), I, 1) = "{" Or Mid$(Trim(NoteStr1), I, 1) = "}" Then Exit For
                    Next I
                    If I = 0 Then I = 90
                    NoteStr2 = Microsoft.VisualBasic.Right(Trim(NoteStr1), Len(NoteStr1) - I)
                    NoteStr1 = Microsoft.VisualBasic.Left(Trim(NoteStr1), I)
                End If
                Common_Procedures.Print_To_PrintDocument(e, NoteStr1, LMargin + 10, CurY, 0, 0, p1Font)
                If NoteStr2 <> "" Then
                    CurY = CurY + TxtHgt
                    Common_Procedures.Print_To_PrintDocument(e, NoteStr2, LMargin + 10, CurY, 0, 0, p1Font)
                End If

            End If

            If Val(Common_Procedures.User.IdNo) <> 1 Then
                Common_Procedures.Print_To_PrintDocument(e, "(" & Trim(Common_Procedures.User.Name) & ")", LMargin + 400, CurY, 0, 0, pFont)
            End If

            CurY = CurY + TxtHgt
            CurY = CurY + TxtHgt
            Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString
            Common_Procedures.Print_To_PrintDocument(e, "Receiver's Signature", LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "Prepared By ", LMargin + 300, CurY, 0, 0, pFont)
            p1Font = New Font("Calibri", 12, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "For " & Cmp_Name, PageWidth - 15, CurY, 1, 0, p1Font)

            CurY = CurY + TxtHgt + 5

            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            e.Graphics.DrawLine(Pens.Black, LMargin, LnAr(1), LMargin, CurY)
            e.Graphics.DrawLine(Pens.Black, PageWidth, LnAr(1), PageWidth, CurY)

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub Printing_Format1464(ByRef e As System.Drawing.Printing.PrintPageEventArgs)

        Dim cmd As New SqlClient.SqlCommand
        Dim Da1 As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim EntryCode As String
        Dim I As Integer, NoofDets As Integer, NoofItems_PerPage As Integer
        Dim pFont As Font
        Dim p1Font As Font
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
        Dim clrName As String = ""
        Dim Clrln As Integer = 0
        'PrintDocument pd = new PrintDocument();
        'pd.DefaultPageSettings.PaperSize = new System.Drawing.Printing.PaperSize("PaperA4", 840, 1180);
        'pd.Print();

        'For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
        '    ps = PrintDocument1.PrinterSettings.PaperSizes(I)
        '    Debug.Print(ps.PaperName)
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

        '    If PpSzSTS = False Then
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
            .Left = 20
            .Right = 55
            .Top = 35
            .Bottom = 35
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

        NoofItems_PerPage = 4 ' 6

        Erase LnAr
        Erase ClArr

        LnAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        ClArr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}

        ClArr(1) = Val(30) : ClArr(2) = 160 : ClArr(3) = 80 : ClArr(4) = 160 : ClArr(5) = 70 : ClArr(6) = 70 : ClArr(7) = 80
        ClArr(8) = PageWidth - (LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7))



        ClArr(2) = ClArr(2) + ClArr(3) + ClArr(4)
        ClArr(3) = 0
        ClArr(4) = 0


        TxtHgt = 16 '18 ' 19 ' e.Graphics.MeasureString("A", pFont).Height  ' 20

        EntryCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(EntFnYrCode)

        Try

            If prn_HdDt.Rows.Count > 0 Then

                Printing_Format1464_PageHeader(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, prn_PageNo, NoofItems_PerPage, CurY, LnAr, ClArr)


                W1 = e.Graphics.MeasureString("No.of Beams  : ", pFont).Width

                NoofDets = 0

                CurY = CurY - 10

                If prn_DetDt.Rows.Count > 0 Then

                    Do While prn_DetIndx <= prn_DetDt.Rows.Count - 1

                        If NoofDets >= NoofItems_PerPage Then
                            CurY = CurY + TxtHgt

                            Common_Procedures.Print_To_PrintDocument(e, "Continued....", PageWidth - 10, CurY, 1, 0, pFont)

                            NoofDets = NoofDets + 1

                            Printing_Format1464_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, NoofDets, False)

                            e.HasMorePages = True
                            Return

                        End If

                        prn_DetSNo = prn_DetSNo + 1

                        ItmNm1 = Trim(prn_DetDt.Rows(prn_DetIndx).Item("Processed_Grey_Name").ToString)
                        ItmNm2 = ""
                        If Len(ItmNm1) > 45 Then
                            For I = 45 To 1 Step -1
                                If Mid$(Trim(ItmNm1), I, 1) = " " Or Mid$(Trim(ItmNm1), I, 1) = "," Or Mid$(Trim(ItmNm1), I, 1) = "." Or Mid$(Trim(ItmNm1), I, 1) = "-" Or Mid$(Trim(ItmNm1), I, 1) = "/" Or Mid$(Trim(ItmNm1), I, 1) = "_" Or Mid$(Trim(ItmNm1), I, 1) = "(" Or Mid$(Trim(ItmNm1), I, 1) = ")" Or Mid$(Trim(ItmNm1), I, 1) = "\" Or Mid$(Trim(ItmNm1), I, 1) = "[" Or Mid$(Trim(ItmNm1), I, 1) = "]" Or Mid$(Trim(ItmNm1), I, 1) = "{" Or Mid$(Trim(ItmNm1), I, 1) = "}" Then Exit For
                            Next I
                            If I = 0 Then I = 45
                            ItmNm2 = Microsoft.VisualBasic.Right(Trim(ItmNm1), Len(ItmNm1) - I)
                            ItmNm1 = Microsoft.VisualBasic.Left(Trim(ItmNm1), I - 1)
                        End If

                        Dim ClrNm1 As String, ClrNm2 As String

                        ClrNm1 = Common_Procedures.Colour_IdNoToName(con, Val(prn_DetDt.Rows(prn_DetIndx).Item("Colour_Idno").ToString))
                        ClrNm2 = ""
                        If Len(ClrNm1) > 12 Then
                            For I = 12 To 1 Step -1
                                If Mid$(Trim(ClrNm1), I, 1) = "@" Or Mid$(Trim(ClrNm1), I, 1) = " " Or Mid$(Trim(ClrNm1), I, 1) = "," Or Mid$(Trim(ClrNm1), I, 1) = "." Or Mid$(Trim(ClrNm1), I, 1) = "-" Or Mid$(Trim(ClrNm1), I, 1) = "/" Or Mid$(Trim(ClrNm1), I, 1) = "_" Or Mid$(Trim(ClrNm1), I, 1) = "(" Or Mid$(Trim(ClrNm1), I, 1) = ")" Or Mid$(Trim(ClrNm1), I, 1) = "\" Or Mid$(Trim(ClrNm1), I, 1) = "[" Or Mid$(Trim(ClrNm1), I, 1) = "]" Or Mid$(Trim(ClrNm1), I, 1) = "{" Or Mid$(Trim(ClrNm1), I, 1) = "}" Or Mid$(Trim(ClrNm1), I, 1) = "@" Then Exit For
                            Next I
                            If I = 0 Then I = 12
                            ClrNm2 = Microsoft.VisualBasic.Right(Trim(ClrNm1), Len(ClrNm1) - I)
                            ClrNm1 = Microsoft.VisualBasic.Left(Trim(ClrNm1), I)
                        End If

                        '----------------------------

                        Dim ItmNmP1 As String, ItmNmP2 As String
                        Dim ClrNmP1 As String, ClrNmP2 As String

                        ItmNmP1 = Trim(prn_DetDt.Rows(prn_DetIndx).Item("Processed_Cloth_Name").ToString)
                        ItmNmP2 = ""
                        If Len(ItmNmP1) > 25 Then
                            For I = 25 To 1 Step -1
                                If Mid$(Trim(ItmNmP1), I, 1) = " " Or Mid$(Trim(ItmNmP1), I, 1) = "," Or Mid$(Trim(ItmNmP1), I, 1) = "." Or Mid$(Trim(ItmNmP1), I, 1) = "-" Or Mid$(Trim(ItmNmP1), I, 1) = "/" Or Mid$(Trim(ItmNmP1), I, 1) = "_" Or Mid$(Trim(ItmNmP1), I, 1) = "(" Or Mid$(Trim(ItmNmP1), I, 1) = ")" Or Mid$(Trim(ItmNmP1), I, 1) = "\" Or Mid$(Trim(ItmNmP1), I, 1) = "[" Or Mid$(Trim(ItmNmP1), I, 1) = "]" Or Mid$(Trim(ItmNmP1), I, 1) = "{" Or Mid$(Trim(ItmNmP1), I, 1) = "}" Then Exit For
                            Next I
                            If I = 0 Then I = 25
                            ItmNmP2 = Microsoft.VisualBasic.Right(Trim(ItmNmP1), Len(ItmNmP1) - I)
                            ItmNmP1 = Microsoft.VisualBasic.Left(Trim(ItmNmP1), I - 1)
                        End If

                        'Dim ClrNmP1 As String, ClrNmP2 As String

                        ClrNmP1 = Trim(prn_DetDt.Rows(prn_DetIndx).Item("DEL_COLOUR_NAME").ToString)
                        ClrNmP2 = ""
                        If Len(ClrNmP1) > 12 Then
                            For I = 12 To 1 Step -1
                                If Mid$(Trim(ClrNmP1), I, 1) = "@" Or Mid$(Trim(ClrNmP1), I, 1) = " " Or Mid$(Trim(ClrNmP1), I, 1) = "," Or Mid$(Trim(ClrNmP1), I, 1) = "." Or Mid$(Trim(ClrNmP1), I, 1) = "-" Or Mid$(Trim(ClrNmP1), I, 1) = "/" Or Mid$(Trim(ClrNmP1), I, 1) = "_" Or Mid$(Trim(ClrNmP1), I, 1) = "(" Or Mid$(Trim(ClrNmP1), I, 1) = ")" Or Mid$(Trim(ClrNmP1), I, 1) = "\" Or Mid$(Trim(ClrNmP1), I, 1) = "[" Or Mid$(Trim(ClrNmP1), I, 1) = "]" Or Mid$(Trim(ClrNmP1), I, 1) = "{" Or Mid$(Trim(ClrNmP1), I, 1) = "}" Or Mid$(Trim(ClrNmP1), I, 1) = "@" Then Exit For
                            Next I
                            If I = 0 Then I = 12
                            ClrNmP2 = Microsoft.VisualBasic.Right(Trim(ClrNmP1), Len(ClrNmP1) - I)
                            ClrNmP1 = Microsoft.VisualBasic.Left(Trim(ClrNmP1), I)
                        End If


                        CurY = CurY + TxtHgt
                        SNo = SNo + 1
                        Common_Procedures.Print_To_PrintDocument(e, Trim(Val(SNo)), LMargin + 10, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm1), LMargin + ClArr(1) + 10, CurY, 0, 0, pFont)


                        If ClArr(3) > 0 Then
                            p1Font = New Font("Calibri", 8, FontStyle.Regular)
                            Common_Procedures.Print_To_PrintDocument(e, ClrNm1, LMargin + ClArr(1) + ClArr(2) + 10, CurY, 0, 0, p1Font)
                        End If

                        If ClArr(4) > 0 Then
                            Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNmP1), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + 10, CurY, 0, 0, pFont)
                            'Common_Procedures.Print_To_PrintDocument(e, ClrNmP1, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + 10, CurY, 0, 0, p1Font)
                        End If


                        Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Bales").ToString, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) - 10, CurY, 1, 0, pFont)

                        If Val(prn_DetDt.Rows(prn_DetIndx).Item("Delivery_Pcs").ToString) <> 0 Then
                            Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(prn_DetIndx).Item("Delivery_Pcs").ToString), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) - 10, CurY, 1, 0, pFont)
                        End If
                        If Val(prn_DetDt.Rows(prn_DetIndx).Item("Delivery_Meters").ToString) <> 0 Then
                            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Delivery_Meters").ToString), "#########0.00"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) - 10, CurY, 1, 0, pFont)
                        End If

                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Delivery_Weight").ToString), "#########0.000"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) - 10, CurY, 1, 0, pFont)

                        NoofDets = NoofDets + 1

                        If Trim(ItmNm2) <> "" Or Trim(ClrNm2) <> "" Or Trim(ItmNmP2) <> "" Or Trim(ClrNmP2) <> "" Then
                            CurY = CurY + TxtHgt
                            Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm2), LMargin + ClArr(1) + 10, CurY, 0, 0, pFont)
                            If ClArr(3) > 0 Then
                                Common_Procedures.Print_To_PrintDocument(e, ClrNm2, LMargin + ClArr(1) + ClArr(2) + 10, CurY, 0, 0, p1Font)
                            End If
                            If ClArr(4) > 0 Then
                                Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNmP2), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + 10, CurY, 0, 0, pFont)
                            End If
                            'Common_Procedures.Print_To_PrintDocument(e, ClrNmP2, LMargin + ClArr(1) + ClArr(2) + 10, CurY, 0, 0, p1Font)
                            NoofDets = NoofDets + 1
                            End If

                            prn_DetIndx = prn_DetIndx + 1

                    Loop

                End If


                Printing_Format1464_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, NoofDets, True)

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

            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

        e.HasMorePages = False

    End Sub

    Private Sub Printing_Format1464_PageHeader(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByRef PageNo As Integer, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single)
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt2 As New DataTable
        Dim p1Font As Font
        Dim strHeight As Single
        Dim C1 As Single, W1 As Single, S1 As Single
        Dim Cmp_Name As String, Cmp_Add1 As String, Cmp_Add2 As String
        Dim Cmp_PhNo As String, Cmp_TinNo As String, Cmp_CstNo As String
        Dim Cmp_GSTIN_Cap As String, Cmp_GSTIN_No As String
        Dim S As String

        PageNo = PageNo + 1

        CurY = TMargin

        da2 = New SqlClient.SqlDataAdapter("select a.*, b.Processed_Item_Name as Processed_Grey_Name, c.Colour_Name ,d.Lot_No ,e.Process_Name  from Textile_Processing_Delivery_Details a INNER JOIN Processed_Item_Head b on a.Item_IdNo = b.Processed_Item_IdNo LEFT OUTER JOIN Colour_Head c on a.Colour_IdNo = c.Colour_IdNo LEFT OUTER JOIN Lot_Head d ON d.Lot_IdNo = a.Lot_IdNo LEFT OUTER JOIN Process_Head e ON e.Process_IdNo = a.Processing_Idno where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Cloth_Processing_Delivery_Code = '" & Trim(EntryCode) & "' Order by a.sl_no", con)
        da2.Fill(dt2)

        If dt2.Rows.Count > NoofItems_PerPage Then
            Common_Procedures.Print_To_PrintDocument(e, "Page : " & Trim(Val(PageNo)), PageWidth - 10, CurY - TxtHgt, 1, 0, pFont)
        End If
        dt2.Clear()

        If PageNo <= 1 Then
            prn_Count = prn_Count + 1
        End If

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
                    If Trim(prn_InpOpts) <> "0" And Val(prn_InpOpts) = "0" And Len(Trim(prn_InpOpts)) > 4 Then
                        prn_OriDupTri = Trim(prn_InpOpts)
                    End If
                End If

            End If
        End If
        If Trim(prn_OriDupTri) <> "" Then
            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_OriDupTri), PageWidth - 10, CurY - TxtHgt, 1, 0, pFont)
        End If

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
        If Trim(prn_HdDt.Rows(0).Item("Company_GSTinNo").ToString) <> "" Then
            Cmp_GSTIN_No = "GSTIN : " & prn_HdDt.Rows(0).Item("Company_GSTinNo").ToString
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
        Common_Procedures.Print_To_PrintDocument(e, Cmp_PhNo & "    " & Cmp_GSTIN_No, LMargin, CurY, 2, PrintWidth, pFont)
        'CurY = CurY + TxtHgt - 1
        'Common_Procedures.Print_To_PrintDocument(e, Cmp_TinNo, LMargin + 10, CurY, 0, 0, pFont)
        'Common_Procedures.Print_To_PrintDocument(e, Cmp_CstNo, PageWidth - 10, CurY, 1, 0, pFont)
        'CurY = CurY + TxtHgt
        'Common_Procedures.Print_To_PrintDocument(e, Cmp_GSTIN_No, LMargin, CurY, 2, PrintWidth, pFont)

        CurY = CurY + TxtHgt - 5
        p1Font = New Font("Calibri", 16, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "PROCESSING DELIVERY", LMargin, CurY, 2, PrintWidth, p1Font)
        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height


        Common_Procedures.Print_To_PrintDocument(e, "(Not For Sale)", LMargin + 10, CurY, 0, 0, pFont)



        CurY = CurY + strHeight  ' + 150
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(2) = CurY

        Try
            C1 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4)
            W1 = e.Graphics.MeasureString("PROCESSING  : ", pFont).Width
            S1 = e.Graphics.MeasureString("TO :    ", pFont).Width

            CurY = CurY + TxtHgt - 10
            p1Font = New Font("Calibri", 12, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "TO  :  " & "M/s." & prn_HdDt.Rows(0).Item("Ledger_Name").ToString, LMargin + 10, CurY, 0, 0, p1Font)
            Common_Procedures.Print_To_PrintDocument(e, "DC.NO", LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("ClothProcess_Delivery_No").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, p1Font)

            CurY = CurY + TxtHgt + 5
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address1").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
            p1Font = New Font("Calibri", 14, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "DATE", LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("ClothProcess_Delivery_Date").ToString), "dd-MM-yyyy").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt + 5
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address2").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)

            Common_Procedures.Print_To_PrintDocument(e, "LOT NO", LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("JobOrder_No").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt + 5
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address3").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)

            Dim vProcess_Nm1 = ""
            Dim vProcess_Nm2 = ""
            Dim i = 0

            vProcess_Nm1 = Trim(prn_HdDt.Rows(0).Item("Process_Name").ToString)
            If Trim(vProcess_Nm1) <> "" Then

                If Len(vProcess_Nm1) > 15 Then

                    For i = 20 To 1 Step -1

                        If Mid$(Trim(vProcess_Nm1), i, 1) = "@" Or Mid$(Trim(vProcess_Nm1), i, 1) = " " Or Mid$(Trim(vProcess_Nm1), i, 1) = "," Or Mid$(Trim(vProcess_Nm1), i, 1) = "." Or Mid$(Trim(vProcess_Nm1), i, 1) = "-" Or Mid$(Trim(vProcess_Nm1), i, 1) = "/" Or Mid$(Trim(vProcess_Nm1), i, 1) = "_" Or Mid$(Trim(vProcess_Nm1), i, 1) = "(" Or Mid$(Trim(vProcess_Nm1), i, 1) = ")" Or Mid$(Trim(vProcess_Nm1), i, 1) = "\" Or Mid$(Trim(vProcess_Nm1), i, 1) = "[" Or Mid$(Trim(vProcess_Nm1), i, 1) = "]" Or Mid$(Trim(vProcess_Nm1), i, 1) = "{" Or Mid$(Trim(vProcess_Nm1), i, 1) = "}" Or Mid$(Trim(vProcess_Nm1), i, 1) = "@" Then Exit For
                    Next i
                    If i = 0 Then i = 15

                    vProcess_Nm2 = Microsoft.VisualBasic.Right(Trim(vProcess_Nm1), Len(vProcess_Nm1) - i)
                    vProcess_Nm1 = Microsoft.VisualBasic.Left(Trim(vProcess_Nm1), i)



                End If
            End If

            Common_Procedures.Print_To_PrintDocument(e, "PROCESSING", LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Trim(vProcess_Nm1), LMargin + C1 + W1 + 30, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt + 5
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address4").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
            If Trim(vProcess_Nm2) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, Trim(vProcess_Nm2), LMargin + C1 + W1 + 30, CurY, 0, 0, pFont)
            Else
                Common_Procedures.Print_To_PrintDocument(e, "E-Way Bill No", LMargin + C1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("EwayBill_No").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, pFont)
            End If
            CurY = CurY + TxtHgt + 5
            p1Font = New Font("Calibri", 11, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "GSTIN : " & prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString, LMargin + S1 + 10, CurY, 0, 0, p1Font)

            If Trim(vProcess_Nm2) <> "" And prn_HdDt.Rows(0).Item("EwayBill_No").ToString <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, "E-Way Bill No", LMargin + C1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("EwayBill_No").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, pFont)
            End If

            If Trim(prn_HdDt.Rows(0).Item("Purchase_OrderNo").ToString) <> "" Then
                CurY = CurY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, "P.O.NO", LMargin + C1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Purchase_OrderNo").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, pFont)
            End If

            CurY = CurY + TxtHgt + 5
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(3) = CurY

            e.Graphics.DrawLine(Pens.Black, LMargin + C1, LnAr(3), LMargin + C1, LnAr(2))

            CurY = CurY + TxtHgt - 10
            Common_Procedures.Print_To_PrintDocument(e, "SNO", LMargin, CurY, 2, ClAr(1), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "ITEM NAME", LMargin + ClAr(1), CurY, 2, ClAr(2), pFont)
            If ClAr(3) > 0 Then
                Common_Procedures.Print_To_PrintDocument(e, "COLOUR", LMargin + ClAr(1) + ClAr(2), CurY, 2, ClAr(3), pFont)
            End If

            If ClAr(4) > 0 Then
                Common_Procedures.Print_To_PrintDocument(e, "PRODUCT", LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, 2, ClAr(4), pFont)
                'Common_Procedures.Print_To_PrintDocument(e, "COLOUR", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 2, ClAr(5), pFont)
            End If

            Common_Procedures.Print_To_PrintDocument(e, "BALE", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 2, ClAr(5), pFont)
            'Common_Procedures.Print_To_PrintDocument(e, "BALES NOS", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 2, ClAr(5), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "PCS", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, 2, ClAr(6), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "METERS", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, 2, ClAr(7), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "WEIGHT", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, 2, ClAr(8), pFont)


            CurY = CurY + TxtHgt + 5
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(4) = CurY

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub Printing_Format1464_PageFooter(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single, ByVal NoofDets As Integer, ByVal is_LastPage As Boolean)
        Dim p1Font As Font
        Dim I As Integer
        Dim Cmp_Name As String
        Dim W1 As Single = 0
        Dim vprn_BlNos As String = ""
        Dim BLNo1 As String
        Dim BLNo2 As String
        Dim NoteStr1 As String = ""
        Dim NoteStr2 As String = ""
        Dim vTxamt As String = 0
        Dim vNtAMt As String = 0
        Dim vTxPerc As String
        Dim vSgst_amt As String = 0
        Dim vCgst_amt As String = 0
        Dim vIgst_amt As String = 0
        Dim vChk_GST_Bill As Integer = 0
        Dim C1 As Single
        Dim W2 As Single
        Dim W3 As Single

        W1 = e.Graphics.MeasureString("No.of Beams  : ", pFont).Width

        Try



            For I = NoofDets + 1 To NoofItems_PerPage

                CurY = CurY + TxtHgt



                prn_DetIndx = prn_DetIndx + 1

            Next

            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(5) = CurY

            CurY = CurY + TxtHgt - 10
            If is_LastPage = True Then
                Common_Procedures.Print_To_PrintDocument(e, " TOTAL", LMargin + ClAr(1) + 10, CurY, 2, ClAr(2), pFont)
            End If

            If Val(prn_HdDt.Rows(0).Item("Total_Pcs").ToString) <> 0 Then
                If is_LastPage = True Then
                    Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(0).Item("Total_Pcs").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 5, CurY, 1, 0, pFont)
                End If
            End If

            If Val(prn_HdDt.Rows(0).Item("Total_Weight").ToString) <> 0 Then
                If is_LastPage = True Then
                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Total_Weight").ToString), "#########0.000"), PageWidth - 10, CurY, 1, 0, pFont)
                End If
            End If

            If Val(prn_HdDt.Rows(0).Item("Total_Meters").ToString) <> 0 Then
                If is_LastPage = True Then
                    ' Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Total_Meters").ToString), "#########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY, 1, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Total_Meters").ToString), "#########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, pFont)
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
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), LnAr(3))

            CurY = CurY + TxtHgt - 5

            vTxPerc = 0
            vCgst_amt = 0
            vSgst_amt = 0
            vIgst_amt = 0
            ' 

            vChk_GST_Bill = Val(prn_HdDt.Rows(0).Item("GST_Tax_Invoice_Status").ToString)

            If Val(vChk_GST_Bill) = 1 Then



                If Val(prn_HdDt.Rows(0).Item("Company_State_IdNo").ToString) = Val(prn_HdDt.Rows(0).Item("Ledger_State_IdNo").ToString) Then

                    vTxPerc = Format(Val(prn_DetDt.Rows(0).Item("item_gst_percentage").ToString) / 2, "############0.00")

                    vCgst_amt = Format(Val(prn_HdDt.Rows(0).Item("Net_Amount").ToString) * Val(vTxPerc) / 100, "############0.00")
                    vSgst_amt = Format(Val(prn_HdDt.Rows(0).Item("Net_Amount").ToString) * Val(vTxPerc) / 100, "############0.00")

                Else

                    vTxPerc = prn_DetDt.Rows(0).Item("item_gst_percentage").ToString
                    vIgst_amt = Format(Val(prn_HdDt.Rows(0).Item("Net_Amount").ToString) * Val(vTxPerc) / 100, "############0.00")

                End If
            End If

            W1 = e.Graphics.MeasureString("Transport Name:", pFont).Width
            W2 = e.Graphics.MeasureString("CGST @ 2.5%:", pFont).Width
            W3 = e.Graphics.MeasureString("Value Of Goods :", pFont).Width

            C1 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(3)


            vprn_BlNos = ""
            For I = 0 To prn_DetDt.Rows.Count - 1
                If Trim(prn_DetDt.Rows(I).Item("Bales_Nos").ToString) <> "" Then
                    vprn_BlNos = Trim(vprn_BlNos) & IIf(Trim(vprn_BlNos) <> "", ", ", "") & prn_DetDt.Rows(I).Item("Bales_Nos").ToString
                End If
            Next

            ' CurY = CurY + TxtHgt
            BLNo1 = Trim(vprn_BlNos)
            BLNo2 = ""
            If Len(BLNo1) > 90 Then
                For I = 90 To 1 Step -1
                    If Mid$(Trim(BLNo1), I, 1) = " " Or Mid$(Trim(BLNo1), I, 1) = "," Or Mid$(Trim(BLNo1), I, 1) = "." Or Mid$(Trim(BLNo1), I, 1) = "-" Or Mid$(Trim(BLNo1), I, 1) = "/" Or Mid$(Trim(BLNo1), I, 1) = "_" Or Mid$(Trim(BLNo1), I, 1) = "(" Or Mid$(Trim(BLNo1), I, 1) = ")" Or Mid$(Trim(BLNo1), I, 1) = "\" Or Mid$(Trim(BLNo1), I, 1) = "[" Or Mid$(Trim(BLNo1), I, 1) = "]" Or Mid$(Trim(BLNo1), I, 1) = "{" Or Mid$(Trim(BLNo1), I, 1) = "}" Then Exit For
                Next I
                If I = 0 Then I = 90
                BLNo2 = Microsoft.VisualBasic.Right(Trim(BLNo1), Len(BLNo1) - I)
                BLNo1 = Microsoft.VisualBasic.Left(Trim(BLNo1), I)
            End If

            If Trim(BLNo1) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, "Bale/Bundle No : " & BLNo1, LMargin + 10, CurY, 0, 0, pFont)
            End If

            If Val(prn_HdDt.Rows(0).Item("Net_Amount").ToString) <> 0 And Val(vChk_GST_Bill) = 1 Then
                Common_Procedures.Print_To_PrintDocument(e, "Taxable Amount", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + W3 - 15, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Net_Amount").ToString), "##########0.00"), PageWidth - 10, CurY, 1, 0, pFont)
            Else
                If Val(prn_HdDt.Rows(0).Item("Net_Amount").ToString) <> 0 Then
                    Common_Procedures.Print_To_PrintDocument(e, "Value Of Goods", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + W3 - 15, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Net_Amount").ToString), "##########0.00"), PageWidth - 10, CurY, 1, 0, pFont)
                End If
            End If


            CurY = CurY + TxtHgt

            If Trim(BLNo2) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, Space(Len("Bale/Bundle No : ")) & BLNo2, LMargin + 10, CurY, 0, 0, pFont)
            End If
            If Val(prn_HdDt.Rows(0).Item("Net_Amount").ToString) <> 0 And Val(vChk_GST_Bill) = 1 Then

                If Val(vIgst_amt) <> 0 Then

                    Common_Procedures.Print_To_PrintDocument(e, "IGST @ " & Val(vTxPerc) & " %", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(6) + ClAr(7) + 10 + W3, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(vIgst_amt), "##########0.00"), PageWidth - 10, CurY, 1, 0, pFont)

                Else

                    Common_Procedures.Print_To_PrintDocument(e, "CGST @ " & Val(vTxPerc) & " %  : " & Format(Val(vCgst_amt), "##########0.00"), LMargin + ClAr(1) + ClAr(2) + 5, CurY, 1, 0, pFont)

                    ''                    Common_Procedures.Print_To_PrintDocument(e, "CGST @ " & Val(vTxPerc) & " %", LMargin + C1, CurY, 0, 0, pFont)
                    'Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W2, CurY, 0, 0, pFont)
                    'Common_Procedures.Print_To_PrintDocument(e, Format(Val(vCgst_amt), "##########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + 50, CurY, 1, 0, pFont)

                    Common_Procedures.Print_To_PrintDocument(e, "SGST @ " & Val(vTxPerc) & " %", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + W3 - 15, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(vSgst_amt), "##########0.00"), PageWidth - 10, CurY, 1, 0, pFont)

                End If

            End If

            CurY = CurY + TxtHgt
            If Trim(prn_HdDt.Rows(0).Item("Vehicle_No").ToString) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, "Vehicle No : " & Trim(prn_HdDt.Rows(0).Item("Vehicle_No").ToString), LMargin + 10, CurY, 0, 0, pFont)
            End If

            If Val(prn_HdDt.Rows(0).Item("Net_Amount").ToString) <> 0 And Val(vChk_GST_Bill) = 1 Then
                vNtAMt = Format(Val(prn_HdDt.Rows(0).Item("Net_Amount").ToString) + Val(vCgst_amt) + Val(vSgst_amt) + Val(vIgst_amt), "###########0")

                Common_Procedures.Print_To_PrintDocument(e, "Value Of Goods", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + W3 - 15, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, Format(Val(vNtAMt), "###########0.00"), PageWidth - 10, CurY, 1, 0, pFont)

            End If

            CurY = CurY + TxtHgt + 10
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(7) = CurY

            If Trim(prn_HdDt.Rows(0).Item("Note").ToString) <> "" Then

                CurY = CurY + TxtHgt
                p1Font = New Font("Calibri", 9, FontStyle.Regular)
                NoteStr1 = "( Note: " & Trim(prn_HdDt.Rows(0).Item("Note").ToString) & " )"
                If Len(NoteStr1) > 90 Then
                    For I = 90 To 1 Step -1
                        If Mid$(Trim(NoteStr1), I, 1) = " " Or Mid$(Trim(NoteStr1), I, 1) = "," Or Mid$(Trim(NoteStr1), I, 1) = "." Or Mid$(Trim(NoteStr1), I, 1) = "-" Or Mid$(Trim(NoteStr1), I, 1) = "/" Or Mid$(Trim(NoteStr1), I, 1) = "_" Or Mid$(Trim(NoteStr1), I, 1) = "(" Or Mid$(Trim(NoteStr1), I, 1) = ")" Or Mid$(Trim(NoteStr1), I, 1) = "\" Or Mid$(Trim(NoteStr1), I, 1) = "[" Or Mid$(Trim(NoteStr1), I, 1) = "]" Or Mid$(Trim(NoteStr1), I, 1) = "{" Or Mid$(Trim(NoteStr1), I, 1) = "}" Then Exit For
                    Next I
                    If I = 0 Then I = 90
                    NoteStr2 = Microsoft.VisualBasic.Right(Trim(NoteStr1), Len(NoteStr1) - I)
                    NoteStr1 = Microsoft.VisualBasic.Left(Trim(NoteStr1), I)
                End If
                Common_Procedures.Print_To_PrintDocument(e, NoteStr1, LMargin + 10, CurY, 0, 0, p1Font)
                If NoteStr2 <> "" Then
                    CurY = CurY + TxtHgt
                    Common_Procedures.Print_To_PrintDocument(e, NoteStr2, LMargin + 10, CurY, 0, 0, p1Font)
                End If

            End If

            If Val(Common_Procedures.User.IdNo) <> 1 Then
                Common_Procedures.Print_To_PrintDocument(e, "(" & Trim(Common_Procedures.User.Name) & ")", LMargin + 400, CurY, 0, 0, pFont)
            End If

            CurY = CurY + TxtHgt
            CurY = CurY + TxtHgt
            Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString
            Common_Procedures.Print_To_PrintDocument(e, "Receiver's Signature", LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "Prepared By ", LMargin + 300, CurY, 0, 0, pFont)
            p1Font = New Font("Calibri", 12, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "For " & Cmp_Name, PageWidth - 15, CurY, 1, 0, p1Font)

            CurY = CurY + TxtHgt + 5

            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            e.Graphics.DrawLine(Pens.Black, LMargin, LnAr(1), LMargin, CurY)
            e.Graphics.DrawLine(Pens.Black, PageWidth, LnAr(1), PageWidth, CurY)

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

End Class