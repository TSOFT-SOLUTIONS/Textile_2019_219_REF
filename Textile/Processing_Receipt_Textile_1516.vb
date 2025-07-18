Public Class Processing_Receipt_Textile_1516

    Implements Interface_MDIActions

    Private con As New SqlClient.SqlConnection(Common_Procedures.Connection_String)
    Private con1 As New SqlClient.SqlConnection(Common_Procedures.ConnectionString_CompanyGroupdetails)

    Private FrmLdSTS As Boolean = False
    Private New_Entry As Boolean = False
    Private Insert_Entry As Boolean = False
    Private Filter_Status As Boolean = False

    Private Pk_Condition As String = "FPFRC-"
    Private Pk_Condition1 As String = "FPFR1-"
    Private Pk_Condition2 As String = "FPFDC-"

    Private vcbotxt_FPGRID As String

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

    Private WithEvents dgtxt_details As New DataGridViewTextBoxEditingControl
    Private WithEvents dgtxt_details_cb As New DataGridViewTextBoxEditingControl
    Private WithEvents dgtxt_details_fp As New DataGridViewTextBoxEditingControl

    Private dgv_LevColNo As Integer
    Private Act_Ctrl As String

    Dim Process_Inputs As String
    Dim Process_Outputs As String

    Private Enum dgvCol_Details As Integer
        SLNO                                '0
        DC_NO                               '1
        ITEM_FP                             '2
        COLOUR                              '3
        PROCESSING                          '4
        LOT_NO                              '5
        FOLDING                             '6
        PCS                                 '7
        QTY                                 '8
        METERS                              '9
        METERS_100PERC_FOLDING              '10
        WEIGHT                              '11
        BITSMETERS                          '12
        EXC_SHT_Mtr                         '13
        Process_Delv_Code                   '14
        Process_Delv_Slno                   '15
        Rct_Slno                            '16
        rct_Code                            '17
        Processed_Fabric_Inspection_Code    '18
        ITEM_GREY                           '19
    End Enum

    Private Enum dgvCol_Details_FP As Integer
        SLNO                        '0
        ITEM_FP                     '1
        COLOUR                      '2
        LOT_NO                      '3
        QUANTITY                    '4
        Process_Delv_Code           '5
        Process_Delv_Slno           '6
        Rct_Slno                    '7
        rct_Code                    '8
        Passed_Mtrs                 '9
        Reject_Mtrs                 '10 
        Mtrs_per_Qty                '11
        Fabric_Consumption          '12
    End Enum

    Private Enum dgvCol_Details_CB As Integer
        SLNO                        '0
        ITEM_CB                    '1
        COLOUR                      '2
        LOT_NO                      '3
        METERS                      '4
        Process_Delv_Code           '5
        Process_Delv_Slno           '6
        Rct_Slno                    '7
        rct_Code                    '8
    End Enum

    Private Displaying As Boolean = False

    Public Sub New()

        ' This call is required by the designer.

        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

    End Sub

    Private Sub clear()

        New_Entry = False

        Insert_Entry = False

        pnl_Back.Enabled = True
        pnl_Back.Visible = True
        pnl_Filter.Visible = False
        pnl_Selection.Visible = False

        lbl_RecNo.Text = ""
        lbl_RecNo.ForeColor = Color.Black

        dtp_Date.Text = ""
        txt_JobNo.Text = ""
        cbo_Ledger.Text = ""

        cbo_TransportName.Text = ""
        cbo_VehicleNo.Text = ""

        txt_Frieght.Text = ""
        cbo_DeliveryTo.Text = ""
        txt_filterpono.Text = ""
        txt_PartyDcNo.Text = ""
        lbl_UserName.Text = "USER : " & Trim(UCase(Common_Procedures.User_IdNoToName(con1, Common_Procedures.User.IdNo)))

        dgv_Fabric_Details.Rows.Clear()
        dgv_FP_Details.Rows.Clear()
        dgv_Details_CB.Rows.Clear()

        txt_DiffMeter.Text = ""
        txt_ReceiptMeter.Text = ""

        Grid_DeSelect()

        cbo_grid_Cloth_Fabric.Visible = False
        cbo_grid_Colour_Fabric.Visible = False
        cbo_grid_LotNo_Fabric.Visible = False
        cbo_grid_Processing_Fabric.Visible = False

        cbo_grid_Cloth_Fabric.Tag = -1
        cbo_grid_Colour_Fabric.Tag = -1
        cbo_grid_LotNo_Fabric.Tag = -1
        cbo_grid_Processing_Fabric.Tag = -1

        vcbotxt_FPGRID = ""
        cbo_grid_FP.Visible = False
        cbo_grid_Colour_FP.Visible = False
        cbo_grid_LotNo_FP.Visible = False

        cbo_grid_FP.Tag = -1
        cbo_grid_Colour_FP.Tag = -1
        cbo_grid_LotNo_FP.Tag = -1



        cbo_Ledger.Enabled = True
        cbo_Ledger.BackColor = Color.White

        txt_JobNo.Enabled = True
        txt_JobNo.BackColor = Color.White

        cbo_grid_Colour_Fabric.Enabled = True
        cbo_grid_Colour_Fabric.BackColor = Color.White

        cbo_grid_Cloth_Fabric.Enabled = True
        cbo_grid_Cloth_Fabric.BackColor = Color.White

        cbo_grid_LotNo_Fabric.Enabled = True
        cbo_grid_LotNo_Fabric.BackColor = Color.White

        chk_LotComplete.Checked = False

        cbo_grid_Processing_Fabric.Enabled = True
        cbo_grid_Processing_Fabric.BackColor = Color.White

        cbo_grid_Cloth_Fabric.Text = ""
        cbo_grid_Colour_Fabric.Text = ""
        cbo_grid_LotNo_Fabric.Text = ""
        cbo_grid_Processing_Fabric.Text = ""

        lbl_Total_100PercFolding_FabricMeters.Text = ""
        lbl_Total_BitsMeters.Text = ""
        lbl_Total_BedsheetMeters.Text = ""
        lbl_Total_ExcShtMeters.Text = ""
        lbl_Total_ExcShtPerc.Text = ""

        cbo_Receipt_Type.Text = "DELIVERY"
        cbo_Receipt_Type.Tag = "DELIVERY"

        dgv_Fabric_Details.AllowUserToAddRows = False
        dgv_Fabric_Details.Tag = ""
        dgv_LevColNo = -1

    End Sub

    Private Sub Grid_DeSelect()

        On Error Resume Next


        If Not IsNothing(dgv_Fabric_Details.CurrentCell) Then dgv_Fabric_Details.CurrentCell.Selected = False
        If Not IsNothing(dgv_FP_Details.CurrentCell) Then dgv_FP_Details.CurrentCell.Selected = False
        If Not IsNothing(dgv_Details_CB.CurrentCell) Then dgv_Details_CB.CurrentCell.Selected = False

        If Not IsNothing(dgv_Details_Total.CurrentCell) Then dgv_Details_Total.CurrentCell.Selected = False
        If Not IsNothing(dgv_Details_FP_Total.CurrentCell) Then dgv_Details_FP_Total.CurrentCell.Selected = False
        If Not IsNothing(dgv_Details_CB_Total.CurrentCell) Then dgv_Details_CB_Total.CurrentCell.Selected = False

        If Not IsNothing(dgv_Filter_Details.CurrentCell) Then dgv_Filter_Details.CurrentCell.Selected = False

    End Sub
    Private Sub ControlGotFocus(ByVal sender As Object, ByVal e As System.EventArgs)

        Dim txtbx As TextBox
        Dim combobx As ComboBox
        Dim chkbx As CheckBox
        On Error Resume Next

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

        If Me.ActiveControl.Name <> cbo_grid_Colour_Fabric.Name Then
            cbo_grid_Colour_Fabric.Visible = False
        End If
        If Me.ActiveControl.Name <> cbo_grid_Cloth_Fabric.Name Then
            cbo_grid_Cloth_Fabric.Visible = False
        End If
        If Me.ActiveControl.Name <> cbo_grid_Processing_Fabric.Name Then
            cbo_grid_Processing_Fabric.Visible = False
        End If
        If Me.ActiveControl.Name <> cbo_grid_LotNo_Fabric.Name Then
            cbo_grid_LotNo_Fabric.Visible = False
        End If
        If Me.ActiveControl.Name <> cbo_grid_FP.Name Then
            cbo_grid_FP.Visible = False
        End If
        If Me.ActiveControl.Name <> cbo_grid_Colour_FP.Name Then
            cbo_grid_LotNo_Fabric.Visible = False
        End If
        If Me.ActiveControl.Name <> cbo_grid_LotNo_FP.Name Then
            cbo_grid_LotNo_Fabric.Visible = False
        End If
        If Me.ActiveControl.Name <> cbo_Cloth_CB.Name Then
            cbo_grid_LotNo_Fabric.Visible = False
        End If
        If Me.ActiveControl.Name <> cbo_Colour_CB.Name Then
            cbo_grid_LotNo_Fabric.Visible = False
        End If
        If Me.ActiveControl.Name <> cbo_LotNo_CB.Name Then
            cbo_grid_LotNo_Fabric.Visible = False
        End If
        If Me.ActiveControl.Name <> dgv_Details_Total.Name Then
            Grid_DeSelect()
        End If

        If Me.ActiveControl.Name <> dgv_Fabric_Details.Name Then
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

        If Not IsNothing(dgv_Fabric_Details.CurrentCell) Then dgv_Fabric_Details.CurrentCell.Selected = False
        If Not IsNothing(dgv_Details_Total.CurrentCell) Then dgv_Details_Total.CurrentCell.Selected = False

        If Not IsNothing(dgv_FP_Details.CurrentCell) Then dgv_FP_Details.CurrentCell.Selected = False
        If Not IsNothing(dgv_Details_FP_Total.CurrentCell) Then dgv_Details_FP_Total.CurrentCell.Selected = False

        If Not IsNothing(dgv_Details_CB.CurrentCell) Then dgv_Details_CB.CurrentCell.Selected = False
        If Not IsNothing(dgv_Details_CB_Total.CurrentCell) Then dgv_Details_CB_Total.CurrentCell.Selected = False

        If Not IsNothing(dgv_Filter_Details.CurrentCell) Then dgv_Filter_Details.CurrentCell.Selected = False

    End Sub
    Private Sub move_record(ByVal no As String)
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable

        Dim da3 As New SqlClient.SqlDataAdapter
        Dim da4 As New SqlClient.SqlDataAdapter
        Dim dt3 As New DataTable
        Dim dt4 As New DataTable

        Dim NewCode As String
        Dim n As Integer
        Dim SNo As Integer
        Dim LockSTS As Boolean = False
        If Val(no) = 0 Then Exit Sub

        Displaying = True
        clear()
        Displaying = False

        NewCode = Pk_Condition + Trim(Val(lbl_Company.Tag)) & "-" & Trim(no) & "/" & Trim(Common_Procedures.FnYearCode)

        Try

            Displaying = True

            da1 = New SqlClient.SqlDataAdapter("select a.*, b.Ledger_Name , c.Ledger_Name as Transport_Name,  d.Ledger_Name as DeliveryToName from Textile_Processing_Receipt_Head a INNER JOIN Ledger_Head b ON a.Ledger_IdNo = b.Ledger_IdNo LEFT OUTER JOIN Ledger_Head c ON a.Transport_IdNo = c.Ledger_IdNo LEFT OUTER JOIN Ledger_Head d ON a.DeliveryTo_IdNo = d.Ledger_IdNo and ClothProcess_Receipt_Code Like '%" & Pk_Condition & "%' Where a.ClothProcess_Receipt_Code = '" & Trim(NewCode) & "'", con)
            da1.Fill(dt1)

            If dt1.Rows.Count > 0 Then

                lbl_RecNo.Text = dt1.Rows(0).Item("ClothProcess_Receipt_No").ToString
                dtp_Date.Text = dt1.Rows(0).Item("ClothProcess_Receipt_Date").ToString
                cbo_Ledger.Text = dt1.Rows(0).Item("Ledger_Name").ToString
                txt_PartyDcNo.Text = dt1.Rows(0).Item("Party_Dc_No").ToString
                txt_JobNo.Text = dt1.Rows(0).Item("Job_No").ToString
                cbo_TransportName.Text = dt1.Rows(0).Item("Transport_Name").ToString
                txt_Frieght.Text = Format(Val(dt1.Rows(0).Item("Freight_Charges").ToString), "########0.00")
                cbo_DeliveryTo.Text = dt1.Rows(0).Item("DeliveryToName").ToString
                cbo_ProcessingName.Text = Common_Procedures.Process_IdNoToName(con, Val(dt1.Rows(0).Item("Processing_Idno").ToString))

                If Not IsDBNull(dt1.Rows(0).Item("Process_Idno")) Then

                    cbo_Process_Completed.Text = Common_Procedures.Process_IdNoToName(con, Val(dt1.Rows(0).Item("Process_Idno").ToString))

                    Dim Process_Inputs_Tmp As String = ""
                    Dim Process_Outputs_Tmp As String = ""

                    Process_Inputs_Tmp = ""
                    Process_Outputs_Tmp = ""

                    Dim da As New SqlClient.SqlDataAdapter("select * from process_head where process_name = '" & cbo_Process_Completed.Text & "'", con)
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


                    If Process_Outputs_Tmp + Process_Inputs_Tmp <> Process_Outputs + Process_Inputs Then

                        'If MessageBox.Show("Changing the Process Will Clear All Finished Product Values in Details. Continue ?", "CHANGE PROCESS...", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.No Then
                        '    cbo_Process_Completed.Text = cbo_Process_Completed.Tag
                        '    cbo_Process_Completed.Focus()
                        'Else

                        Process_Inputs = Process_Inputs_Tmp
                        Process_Outputs = Process_Outputs_Tmp

                        'End If

                    End If

                End If

                If Val(dt1.Rows(0).Item("Lot_Status").ToString) = 1 Then
                    chk_LotComplete.Checked = True
                Else
                    chk_LotComplete.Checked = False
                End If

                cbo_VehicleNo.Text = dt1.Rows(0).Item("Vehicle_No").ToString

                If Not IsDBNull(dt1.Rows(0).Item("Receipt_Type")) Then
                    cbo_Receipt_Type.Text = dt1.Rows(0).Item("Receipt_Type").ToString
                End If

                If Trim(UCase(cbo_Receipt_Type.Text)) = "DELIVERY" Then
                    btn_Selection.Enabled = True
                    dgv_Fabric_Details.Columns(dgvCol_Details.DC_NO).ReadOnly = True
                    dgv_Fabric_Details.Columns(dgvCol_Details.ITEM_FP).ReadOnly = True
                    dgv_Fabric_Details.Columns(dgvCol_Details.COLOUR).ReadOnly = True
                    dgv_Fabric_Details.Columns(dgvCol_Details.PROCESSING).ReadOnly = True
                    dgv_Fabric_Details.Columns(dgvCol_Details.LOT_NO).ReadOnly = True
                    dgv_Fabric_Details.AllowUserToAddRows = False
                Else
                    btn_Selection.Enabled = False
                    dgv_Fabric_Details.Columns(dgvCol_Details.DC_NO).ReadOnly = False
                    dgv_Fabric_Details.Columns(dgvCol_Details.ITEM_FP).ReadOnly = False
                    dgv_Fabric_Details.Columns(dgvCol_Details.COLOUR).ReadOnly = False
                    dgv_Fabric_Details.Columns(dgvCol_Details.PROCESSING).ReadOnly = False
                    dgv_Fabric_Details.Columns(dgvCol_Details.LOT_NO).ReadOnly = False
                    dgv_Fabric_Details.AllowUserToAddRows = True
                End If

                lbl_UserName.Text = "USER : " & Trim(UCase(Common_Procedures.User_IdNoToName(con1, Val(dt1.Rows(0).Item("User_IdNo").ToString))))

                da2 = New SqlClient.SqlDataAdapter("select a.Cloth_Processing_Delivery_Slno as cpd_sno, a.*, C.Cloth_Name as Fp_Item_Name,d.Colour_Name,e.Lot_No as Lot_No,f.Process_Name,G.Cloth_Name as Item_Name from Textile_Processing_Receipt_Details a  INNER JOIN Cloth_Head C ON c.Cloth_Idno = a.Item_To_Idno LEFT OUTER JOIN Cloth_Head G ON G.Cloth_Idno = a.Item_Idno  LEFT OUTER JOIN Colour_Head d ON d.Colour_IdNo = a.Colour_IdNo LEFT OUTER JOIN Lot_Head e ON a.Lot_IdNo = e.Lot_IdNo LEFT OUTER JOIN Process_Head f ON f.Process_IdNo = a.Processing_Idno where a.Cloth_Processing_Receipt_Code = '" & Trim(NewCode) & "' and  a.Cloth_Processing_Receipt_Code Like '%" & Pk_Condition & "%' Order by Sl_No", con)
                dt2 = New DataTable
                da2.Fill(dt2)

                dgv_Fabric_Details.Rows.Clear()
                SNo = 0

                If dt2.Rows.Count > 0 Then

                    For i = 0 To dt2.Rows.Count - 1

                        n = dgv_Fabric_Details.Rows.Add()

                        SNo = SNo + 1
                        dgv_Fabric_Details.Rows(n).Cells(dgvCol_Details.SLNO).Value = Val(SNo)
                        dgv_Fabric_Details.Rows(n).Cells(dgvCol_Details.DC_NO).Value = dt2.Rows(i).Item("Dc_Rc_No").ToString
                        dgv_Fabric_Details.Rows(n).Cells(dgvCol_Details.ITEM_FP).Value = dt2.Rows(i).Item("Fp_Item_Name").ToString
                        dgv_Fabric_Details.Rows(n).Cells(dgvCol_Details.COLOUR).Value = dt2.Rows(i).Item("Colour_Name").ToString
                        dgv_Fabric_Details.Rows(n).Cells(dgvCol_Details.PROCESSING).Value = dt2.Rows(i).Item("Process_Name").ToString
                        dgv_Fabric_Details.Rows(n).Cells(dgvCol_Details.LOT_NO).Value = dt2.Rows(i).Item("Lot_No").ToString

                        dgv_Fabric_Details.Rows(n).Cells(dgvCol_Details.FOLDING).Value = Val(dt2.Rows(i).Item("Folding").ToString)
                        If Val(dgv_Fabric_Details.Rows(n).Cells(dgvCol_Details.FOLDING).Value) = 0 Then dgv_Fabric_Details.Rows(n).Cells(dgvCol_Details.FOLDING).Value = ""

                        dgv_Fabric_Details.Rows(n).Cells(dgvCol_Details.PCS).Value = Val(dt2.Rows(i).Item("Receipt_Pcs").ToString)
                        If Val(dgv_Fabric_Details.Rows(n).Cells(dgvCol_Details.PCS).Value) = 0 Then dgv_Fabric_Details.Rows(n).Cells(dgvCol_Details.PCS).Value = ""

                        dgv_Fabric_Details.Rows(n).Cells(dgvCol_Details.QTY).Value = Val(dt2.Rows(i).Item("Receipt_Qty").ToString)
                        dgv_Fabric_Details.Rows(n).Cells(dgvCol_Details.METERS).Value = Format(Val(dt2.Rows(i).Item("Receipt_Meters").ToString), "########0.00")
                        dgv_Fabric_Details.Rows(n).Cells(dgvCol_Details.METERS_100PERC_FOLDING).Value = Format(Val(dt2.Rows(i).Item("Meters_in_100Folding").ToString), "########0.00")
                        dgv_Fabric_Details.Rows(n).Cells(dgvCol_Details.BITSMETERS).Value = Format(Val(dt2.Rows(i).Item("Bits_Meters").ToString), "########0.00")
                        If Val(dgv_Fabric_Details.Rows(n).Cells(dgvCol_Details.BITSMETERS).Value) = 0 Then dgv_Fabric_Details.Rows(n).Cells(dgvCol_Details.BITSMETERS).Value = ""

                        dgv_Fabric_Details.Rows(n).Cells(dgvCol_Details.WEIGHT).Value = Format(Val(dt2.Rows(i).Item("Receipt_Weight").ToString), "########0.000")

                        dgv_Fabric_Details.Rows(n).Cells(dgvCol_Details.EXC_SHT_Mtr).Value = Format(Val(dt2.Rows(i).Item("ExcSht_Meters").ToString), "########0.00")

                        dgv_Fabric_Details.Rows(n).Cells(dgvCol_Details.Process_Delv_Code).Value = dt2.Rows(i).Item("Cloth_Processing_Delivery_Code").ToString
                        dgv_Fabric_Details.Rows(n).Cells(dgvCol_Details.Process_Delv_Slno).Value = dt2.Rows(i).Item("cpd_sno").ToString
                        dgv_Fabric_Details.Rows(n).Cells(dgvCol_Details.Rct_Slno).Value = dt2.Rows(i).Item("Cloth_Processing_Receipt_Slno").ToString
                        'dgv_Details.Rows(n).Cells(dgvCol_Details.rct_Code).Value = dt2.Rows(i).Item("Cloth_Processing_BillMaking_Code").ToString
                        dgv_Fabric_Details.Rows(n).Cells(dgvCol_Details.Processed_Fabric_Inspection_Code).Value = dt2.Rows(i).Item("Processed_Fabric_Inspection_Code").ToString
                        dgv_Fabric_Details.Rows(n).Cells(dgvCol_Details.ITEM_GREY).Value = dt2.Rows(i).Item("Item_Name").ToString

                        If Trim(dgv_Fabric_Details.Rows(n).Cells(dgvCol_Details.Processed_Fabric_Inspection_Code).Value) <> "" Then
                            For j = 0 To dgv_Fabric_Details.ColumnCount - 1
                                dgv_Fabric_Details.Rows(n).Cells(j).Style.BackColor = Color.LightGray
                            Next j
                            LockSTS = True
                        End If

                    Next i

                End If

                With dgv_Details_Total
                    If .RowCount = 0 Then .Rows.Add()
                    .Rows(0).Cells(dgvCol_Details.PCS).Value = Val(dt1.Rows(0).Item("Total_Pcs").ToString)
                    .Rows(0).Cells(dgvCol_Details.QTY).Value = Val(dt1.Rows(0).Item("Total_Qty").ToString)
                    .Rows(0).Cells(dgvCol_Details.METERS).Value = Format(Val(dt1.Rows(0).Item("Total_Meters").ToString), "########0.00")
                    .Rows(0).Cells(dgvCol_Details.METERS_100PERC_FOLDING).Value = Format(Val(dt1.Rows(0).Item("Total_100Folding_Meters").ToString), "########0.00")
                    .Rows(0).Cells(dgvCol_Details.WEIGHT).Value = Format(Val(dt1.Rows(0).Item("Total_Weight").ToString), "########0.000")
                    .Rows(0).Cells(dgvCol_Details.BITSMETERS).Value = Format(Val(dt1.Rows(0).Item("Total_BitsMeters").ToString), "########0.00")
                    .Rows(0).Cells(dgvCol_Details.EXC_SHT_Mtr).Value = Format(Val(dt1.Rows(0).Item("Total_ExcessShort").ToString), "########0.00")
                End With



                SNo = 0

                da3 = New SqlClient.SqlDataAdapter("Select a.*,i.Processed_Item_Name,c.Colour_Name , l.Lot_No from FP_Receipt_Processing_Details a left outer join Processed_Item_Head i on a.FP_IdNo = i.Processed_Item_IdNo left outer join Colour_Head c on a.Colour_IdNo = c.Colour_IdNo left outer join Lot_Head l on a.Lot_IdNo = l.Lot_IdNo Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " And FP_Receipt_Processing_Code = '" & Trim(NewCode) & "' Order by Sl_No", con)
                dt3 = New DataTable
                da3.Fill(dt3)

                If dt3.Rows.Count > 0 Then

                    For i = 0 To dt3.Rows.Count - 1

                        n = dgv_FP_Details.Rows.Add()

                        SNo = SNo + 1
                        dgv_FP_Details.Rows(n).Cells(dgvCol_Details_FP.SLNO).Value = Val(SNo)
                        dgv_FP_Details.Rows(n).Cells(dgvCol_Details_FP.COLOUR).Value = dt3.Rows(i).Item("Colour_Name")
                        dgv_FP_Details.Rows(n).Cells(dgvCol_Details_FP.ITEM_FP).Value = dt3.Rows(i).Item("Processed_Item_Name")
                        dgv_FP_Details.Rows(n).Cells(dgvCol_Details_FP.LOT_NO).Value = dt3.Rows(i).Item("Lot_No")
                        dgv_FP_Details.Rows(n).Cells(dgvCol_Details_FP.QUANTITY).Value = dt3.Rows(i).Item("Quantity")

                        dgv_FP_Details.Rows(n).Cells(dgvCol_Details_FP.Process_Delv_Code).Value = dt3.Rows(i).Item("FP_Delivery_Processing_Code")
                        dgv_FP_Details.Rows(n).Cells(dgvCol_Details_FP.Process_Delv_Slno).Value = dt3.Rows(i).Item("FP_Delivery_Processing_Details_SlNo")

                        dgv_FP_Details.Rows(n).Cells(dgvCol_Details_FP.Passed_Mtrs).Value = dt3.Rows(i).Item("Passed_Mtrs")
                        dgv_FP_Details.Rows(n).Cells(dgvCol_Details_FP.Reject_Mtrs).Value = dt3.Rows(i).Item("Reject_Mtrs")

                        dgv_FP_Details.Rows(n).Cells(dgvCol_Details_FP.Mtrs_per_Qty).Value = dt3.Rows(i).Item("Meters_per_Qty")
                        dgv_FP_Details.Rows(n).Cells(dgvCol_Details_FP.Fabric_Consumption).Value = dt3.Rows(i).Item("Fabric_Consumption_Meters")


                    Next
                End If
                dt3.Clear()

                If dgv_Details_FP_Total.RowCount = 0 Then dgv_Details_FP_Total.Rows.Add()
                If Not IsDBNull(dt1.Rows(0).Item("Total_FP")) Then
                    dgv_Details_FP_Total.Rows(0).Cells(dgvCol_Details_FP.QUANTITY).Value = Val(dt1.Rows(0).Item("Total_FP").ToString)
                End If
                If Not IsDBNull(dt1.Rows(0).Item("Total_Fabric_ConsumptionMeters")) Then
                    dgv_Details_FP_Total.Rows(0).Cells(dgvCol_Details_FP.Fabric_Consumption).Value = Val(dt1.Rows(0).Item("Total_Fabric_ConsumptionMeters").ToString)
                End If


                'SNo = 0
                'Dim NewCode_1 As String

                'NewCode_1 = Pk_Condition1 & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RecNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)
                'da4 = New SqlClient.SqlDataAdapter("select a.Cloth_Processing_Delivery_Slno as cpd_sno,a.*,C.Cloth_Name as Fp_Item_Name,d.Colour_Name,e.Lot_No as Lot_No,f.Process_Name,G.Cloth_Name as Item_Name from Textile_Processing_Receipt_Details a  INNER JOIN Cloth_Head C ON c.Cloth_Idno = a.Item_To_Idno LEFT OUTER JOIN Cloth_Head G ON G.Cloth_Idno = a.Item_Idno  LEFT OUTER JOIN Colour_Head d ON d.Colour_IdNo = a.Colour_IdNo LEFT OUTER JOIN Lot_Head e ON a.Lot_IdNo = e.Lot_IdNo LEFT OUTER JOIN Process_Head f ON f.Process_IdNo = a.Processing_Idno where a.Cloth_Processing_Receipt_Code = '" & Trim(NewCode_1) & "' and  a.Cloth_Processing_Receipt_Code Like '%" & Pk_Condition1 & "%' Order by Sl_No", con)
                'dt4 = New DataTable
                'da4.Fill(dt4)

                'For i = 0 To dt4.Rows.Count - 1

                '    n = dgv_Details_CB.Rows.Add()

                '    SNo = SNo + 1

                '    dgv_Details_CB.Rows(n).Cells(dgvCol_Details_CB.SLNO).Value = Val(SNo)
                '    dgv_Details_CB.Rows(n).Cells(dgvCol_Details_CB.ITEM_CB).Value = dt4.Rows(i).Item("Fp_Item_Name").ToString
                '    dgv_Details_CB.Rows(n).Cells(dgvCol_Details_CB.COLOUR).Value = dt4.Rows(i).Item("Colour_Name").ToString
                '    dgv_Details_CB.Rows(n).Cells(dgvCol_Details_CB.LOT_NO).Value = dt4.Rows(i).Item("Lot_No").ToString
                '    dgv_Details_CB.Rows(n).Cells(dgvCol_Details_CB.METERS).Value = Format(Val(dt4.Rows(i).Item("Receipt_Meters").ToString), "########0.00")
                '    dgv_Details_CB.Rows(n).Cells(dgvCol_Details_CB.Process_Delv_Code).Value = dt4.Rows(i).Item("Cloth_Processing_Delivery_Code").ToString
                '    dgv_Details_CB.Rows(n).Cells(dgvCol_Details_CB.Process_Delv_Slno).Value = dt4.Rows(i).Item("cpd_sno").ToString
                '    dgv_Details_CB.Rows(n).Cells(dgvCol_Details_CB.Rct_Slno).Value = dt4.Rows(i).Item("Cloth_Processing_Receipt_Slno").ToString

                'Next
                'dt4.Clear

                'If dgv_Details_CB_Total.RowCount = 0 Then
                '    dgv_Details_CB_Total.Rows.Add()
                'End If
                'If Not IsDBNull(dt1.Rows(0).Item("Total_CB_Meters")) Then
                '    dgv_Details_CB_Total.Rows(0).Cells(dgvCol_Details_CB.METERS).Value = Format(Val(dt1.Rows(0).Item("Total_CB_Meters").ToString), "######0.000")
                'End If



                If LockSTS = True Then

                    cbo_Ledger.Enabled = False
                    cbo_Ledger.BackColor = Color.LightGray

                    If Trim(dgv_Fabric_Details.Rows(n).Cells(15).Value) <> "" Then
                        txt_JobNo.Enabled = False
                        txt_JobNo.BackColor = Color.LightGray
                    End If

                    cbo_grid_Colour_Fabric.Enabled = False
                    cbo_grid_Colour_Fabric.BackColor = Color.LightGray

                    cbo_grid_Cloth_Fabric.Enabled = False
                    cbo_grid_Cloth_Fabric.BackColor = Color.LightGray

                    cbo_grid_LotNo_Fabric.Enabled = False
                    cbo_grid_LotNo_Fabric.BackColor = Color.LightGray

                    cbo_grid_Processing_Fabric.Enabled = False
                    cbo_grid_Processing_Fabric.BackColor = Color.LightGray

                    dgv_Fabric_Details.AllowUserToAddRows = False

                End If

                dt2.Clear()

                dt2.Dispose()
                da2.Dispose()

            End If

            dt1.Clear()
            dt1.Dispose()
            da1.Dispose()

            Grid_DeSelect()

        Catch ex As Exception

            Displaying = False
            MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally

            Displaying = False

        End Try

        If dtp_Date.Visible And dtp_Date.Enabled Then dtp_Date.Focus()

    End Sub

    Private Sub Processing_Receipt_Textile_1516_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated
        Try

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Ledger.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "LEDGER" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Ledger.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_TransportName.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "LEDGER" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_TransportName.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_grid_Cloth_Fabric.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "CLOTH" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_grid_Cloth_Fabric.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_ProcessingName.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "PROCESS" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_ProcessingName.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_grid_Colour_Fabric.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "COLOUR" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_grid_Colour_Fabric.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_grid_LotNo_Fabric.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "LOT" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_grid_LotNo_Fabric.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_grid_Processing_Fabric.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "PROCESS" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_grid_Processing_Fabric.Text = Trim(Common_Procedures.Master_Return.Return_Value)
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

    Private Sub Processing_Receipt_Textile_1516_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
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

        da = New SqlClient.SqlDataAdapter("select distinct(Vehicle_No) from Textile_Processing_Receipt_Head order by Vehicle_No", con)
        da.Fill(dt1)
        cbo_VehicleNo.DataSource = dt1
        cbo_VehicleNo.DisplayMember = "Vehicle_No"

        ' cbo_itemfp.Visible = False
        cbo_grid_Cloth_Fabric.Visible = False
        cbo_grid_Colour_Fabric.Visible = False
        cbo_grid_LotNo_Fabric.Visible = False
        cbo_grid_Processing_Fabric.Visible = False


        dgv_Selection.Columns(21).DisplayIndex = 7

        If Common_Procedures.settings.Hide_Qty_QtyMtr_In_Processing_Transactions Then
            dgv_Fabric_Details.Columns(dgvCol_Details.QTY).Visible = False
            dgv_Details_Total.Columns(dgvCol_Details.QTY).Visible = False
            dgv_Selection.Columns(dgvCol_Details.QTY).Visible = False
        End If

        If Common_Procedures.settings.Hide_Weight_Processing_Transactions Then
            dgv_Fabric_Details.Columns(dgvCol_Details.WEIGHT).Visible = False
            dgv_Details_Total.Columns(dgvCol_Details.FOLDING).Visible = False
            dgv_Selection.Columns(dgvCol_Details.FOLDING).Visible = False
        End If

        If Common_Procedures.settings.Show_Folding_In_Weight_Processing_Transactions Then
            dgv_Fabric_Details.Columns(dgvCol_Details.FOLDING).Visible = True
            dgv_Details_Total.Columns(dgvCol_Details.FOLDING).Visible = True
        Else
            dgv_Fabric_Details.Columns(dgvCol_Details.FOLDING).Visible = False
            dgv_Details_Total.Columns(dgvCol_Details.FOLDING).Visible = False
        End If

        If Common_Procedures.settings.CustomerCode = "1516" Then

            txt_JobNo.Text = ""
            txt_ReceiptMeter.Text = ""
            txt_DiffMeter.Text = ""

            txt_JobNo.Visible = False
            txt_ReceiptMeter.Visible = False
            txt_DiffMeter.Visible = False

            lbl_Job_No_Caption.Text = ""
            lbl_ReceiptMeter_Caption.Text = ""
            lbl_DiffMeter_Caption.Text = ""

            btn_SendSMS.Visible = False

        End If



        pnl_Back.Visible = True
        pnl_Back.BringToFront()

        pnl_Filter.Visible = False
        pnl_Filter.Left = (Me.Width - pnl_Filter.Width) \ 2
        pnl_Filter.Top = (Me.Height - pnl_Filter.Height) \ 2

        pnl_Selection.Visible = False
        pnl_Selection.Left = (Me.Width - pnl_Selection.Width) \ 2
        pnl_Selection.Top = (Me.Height - pnl_Selection.Height) \ 2
        pnl_Selection.BringToFront()

        AddHandler dtp_Date.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_grid_Colour_Fabric.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_grid_Cloth_Fabric.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Ledger.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_grid_LotNo_Fabric.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_grid_Processing_Fabric.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_ProcessingName.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_TransportName.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_filterpono.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Frieght.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_DeliveryTo.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_JobNo.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_PartyDcNo.GotFocus, AddressOf ControlGotFocus
        AddHandler chk_LotComplete.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_DiffMeter.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_ReceiptMeter.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_VehicleNo.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Filter_ProcessName.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Filter_PartyName.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Receipt_Type.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_grid_FP.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_grid_Colour_FP.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_grid_LotNo_FP.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Cloth_CB.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Colour_CB.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_LotNo_CB.GotFocus, AddressOf ControlGotFocus


        AddHandler dtp_Date.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_grid_Colour_Fabric.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_grid_Cloth_Fabric.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Ledger.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_grid_LotNo_Fabric.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_grid_Processing_Fabric.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_ProcessingName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_TransportName.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_filterpono.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Frieght.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_DeliveryTo.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_JobNo.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_PartyDcNo.LostFocus, AddressOf ControlLostFocus
        AddHandler chk_LotComplete.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_DiffMeter.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_ReceiptMeter.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_VehicleNo.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Filter_ProcessName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Filter_PartyName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Receipt_Type.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_grid_FP.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_grid_Colour_FP.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_grid_LotNo_FP.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Cloth_CB.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Colour_CB.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_LotNo_CB.LostFocus, AddressOf ControlLostFocus

        AddHandler dtp_Date.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_JobNo.KeyDown, AddressOf TextBoxControlKeyDown
        'AddHandler cbo_DeliveryTo.KeyDown, AddressOf TextBoxControlKeyDown
        'AddHandler txt_Frieght.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_PartyDcNo.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_DiffMeter.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_ReceiptMeter.KeyDown, AddressOf TextBoxControlKeyDown

        AddHandler dtp_Date.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_JobNo.KeyPress, AddressOf TextBoxControlKeyPress
        ' AddHandler cbo_DeliveryTo.KeyPress, AddressOf TextBoxControlKeyPress
        'AddHandler txt_Frieght.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_PartyDcNo.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_DiffMeter.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_ReceiptMeter.KeyPress, AddressOf TextBoxControlKeyPress

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
        new_record()

    End Sub

    Private Sub Processing_Receipt_Textile_1516_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        con.Close()
        con.Dispose()
        'Common_Procedures.Hide_CurrentStock_Display()
    End Sub

    Private Sub Processing_Receipt_Textile_1516_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress

        Try
            If Asc(e.KeyChar) = 27 Then

                If pnl_Filter.Visible = True Then
                    btn_Filter_Close_Click(sender, e)
                    Exit Sub
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

    Protected Overrides Function ProcessCmdKey(ByRef msg As System.Windows.Forms.Message, ByVal keyData As System.Windows.Forms.Keys) As Boolean
        Dim dgv1 As New DataGridView

        If ActiveControl.Name = dgv_Fabric_Details.Name Or ActiveControl.Name = dgv_FP_Details.Name Or ActiveControl.Name = dgv_Details_CB.Name Or TypeOf ActiveControl Is DataGridViewTextBoxEditingControl Then

            On Error Resume Next

            If ActiveControl.Name = dgv_Fabric_Details.Name Or Act_Ctrl = dgv_Fabric_Details.Name Then

                dgv1 = Nothing

                dgv1 = dgv_Fabric_Details

                If IsNothing(dgv1) = False Then

                    If IsNothing(dgv1.CurrentCell) Then Exit Function

                    With dgv1


                        If keyData = Keys.Enter Or keyData = Keys.Down Then
                            If dgv1.CurrentCell.ColumnIndex >= dgv1.ColumnCount - 8 Then
                                If .CurrentCell.RowIndex = .RowCount - 1 Then

                                    'If txt_ReceiptMeter.Visible And txt_ReceiptMeter.Enabled Then
                                    '    txt_ReceiptMeter.Focus()
                                    'Else
                                    '    chk_LotComplete.Focus()
                                    'End If

                                    dgv_FP_Details.Focus()
                                    dgv_FP_Details.CurrentCell = dgv_FP_Details.Rows(0).Cells(1)

                                Else

                                    .CurrentCell = .Rows(.CurrentCell.RowIndex + 1).Cells(dgvCol_Details.PCS)

                                End If

                            Else

                                'If .CurrentCell.RowIndex = .RowCount - 2 And dgvCol_Details.DC_NO > 1 And Trim(.CurrentRow.Cells(dgvCol_Details.DC_NO).Value) = "" Then

                                If .CurrentCell.ColumnIndex = dgvCol_Details.DC_NO And Trim(.CurrentRow.Cells(dgvCol_Details.DC_NO).Value) = "" Then

                                    'If txt_ReceiptMeter.Visible And txt_ReceiptMeter.Enabled Then
                                    '    txt_ReceiptMeter.Focus()
                                    'Else
                                    '    chk_LotComplete.Focus()
                                    'End If

                                    dgv_FP_Details.Focus()
                                    dgv_FP_Details.CurrentCell = dgv_FP_Details.Rows(0).Cells(1)

                                Else

                                    For K = .CurrentCell.ColumnIndex + 1 To .ColumnCount - 1
                                        If .Columns(K).Visible Then
                                            .Focus()
                                            .CurrentCell = .Rows(.CurrentRow.Index).Cells(K)
                                            Return True
                                        End If
                                    Next


                                End If
                            End If

                            Return True

                        ElseIf keyData = Keys.Up Then

                            If .CurrentCell.ColumnIndex <= dgvCol_Details.PCS Then

                                If .CurrentCell.RowIndex = 0 Then

                                    txt_Frieght.Focus()

                                Else

                                    .CurrentCell = .Rows(.CurrentCell.RowIndex - 1).Cells(dgvCol_Details.BITSMETERS)

                                End If

                            Else

                                For K = .CurrentCell.ColumnIndex - 1 To 0 Step -1
                                    If .Columns(K).Visible Then
                                        .CurrentCell = .Rows(.CurrentRow.Index).Cells(K)
                                        Return True
                                    End If
                                Next

                                '.CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex - 1)

                            End If

                            Return True

                        Else
                            Return MyBase.ProcessCmdKey(msg, keyData)

                        End If

                    End With

                Else

                    Return MyBase.ProcessCmdKey(msg, keyData)

                End If




                '*****************************************************************************************************************


            ElseIf ActiveControl.Name = dgv_FP_Details.Name Or Act_Ctrl = dgv_FP_Details.Name Then

                dgv1 = Nothing

                dgv1 = dgv_FP_Details

                If IsNothing(dgv1) = False Then

                    If IsNothing(dgv1.CurrentCell) Then Exit Function

                    With dgv1

                        If keyData = Keys.Enter Or keyData = Keys.Down Then

                            If dgv1.CurrentCell.ColumnIndex >= dgvCol_Details_FP.Reject_Mtrs Then

                                If .CurrentCell.RowIndex = .RowCount - 1 Then
                                    chk_LotComplete.Focus()

                                Else
                                    .CurrentCell = .Rows(.CurrentCell.RowIndex + 1).Cells(dgvCol_Details_FP.ITEM_FP)

                                End If

                            Else

                                If .CurrentCell.ColumnIndex = dgvCol_Details_FP.ITEM_FP And Trim(.CurrentRow.Cells(dgvCol_Details_FP.ITEM_FP).Value) = "" Then

                                    chk_LotComplete.Focus()

                                ElseIf dgv1.CurrentCell.ColumnIndex >= dgvCol_Details_FP.QUANTITY Then
                                    .CurrentCell = .Rows(.CurrentRow.Index).Cells(dgvCol_Details_FP.Reject_Mtrs)

                                Else

                                    For K = .CurrentCell.ColumnIndex + 1 To .ColumnCount - 1
                                        If .Columns(K).Visible Then
                                            .Focus()
                                            .CurrentCell = .Rows(.CurrentRow.Index).Cells(K)
                                            Return True
                                        End If
                                    Next


                                End If
                            End If

                            Return True

                        ElseIf keyData = Keys.Up Then

                            If .CurrentCell.ColumnIndex <= 1 Then

                                If .CurrentCell.RowIndex = 0 Then

                                    If dgv_Fabric_Details.Rows.Count > 0 Then
                                        dgv_Fabric_Details.Focus()
                                        dgv_Fabric_Details.CurrentCell = dgv_Fabric_Details.Rows(0).Cells(1)
                                    Else
                                        txt_Frieght.Focus()
                                    End If


                                Else

                                    .CurrentCell = .Rows(.CurrentCell.RowIndex - 1).Cells(dgvCol_Details_FP.Reject_Mtrs)

                                End If

                            ElseIf dgv1.CurrentCell.ColumnIndex >= dgvCol_Details_FP.Reject_Mtrs Then
                                .CurrentCell = .Rows(.CurrentRow.Index).Cells(dgvCol_Details_FP.QUANTITY)

                            Else

                                For K = .CurrentCell.ColumnIndex - 1 To 0 Step -1
                                    If .Columns(K).Visible Then
                                        .CurrentCell = .Rows(.CurrentRow.Index).Cells(K)
                                        Return True
                                    End If
                                Next

                                '.CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex - 1)

                            End If

                            Return True

                        Else

                            Return MyBase.ProcessCmdKey(msg, keyData)

                        End If

                    End With

                Else

                    Return MyBase.ProcessCmdKey(msg, keyData)

                End If


                'ElseIf ActiveControl.Name = dgv_Details_CB.Name Or Act_Ctrl = dgv_Details_CB.Name Then

                '    dgv1 = Nothing

                '    dgv1 = dgv_Details_CB


                '    If IsNothing(dgv1) = False Then

                '        With dgv1

                '            If keyData = Keys.Enter Or keyData = Keys.Down Then

                '                If dgv1.CurrentCell.ColumnIndex >= dgv1.ColumnCount - 5 Then

                '                    If .CurrentCell.RowIndex = .RowCount - 1 Then

                '                        If txt_ReceiptMeter.Visible And txt_ReceiptMeter.Enabled Then
                '                            txt_ReceiptMeter.Focus()
                '                        ElseIf txt_DiffMeter.Visible And txt_DiffMeter.Enabled Then
                '                            txt_DiffMeter.Focus()
                '                        Else
                '                            chk_LotComplete.Focus()
                '                        End If

                '                        'dgv_Details_FP.Focus()

                '                    Else

                '                        .CurrentCell = .Rows(.CurrentCell.RowIndex + 1).Cells(dgvCol_Details_CB.ITEM_CB)

                '                    End If

                '                Else

                '                    If .CurrentCell.ColumnIndex = dgvCol_Details_CB.ITEM_CB And Trim(.CurrentRow.Cells(dgvCol_Details_CB.ITEM_CB).Value) = "" Then

                '                        If txt_ReceiptMeter.Visible And txt_ReceiptMeter.Enabled Then
                '                            txt_ReceiptMeter.Focus()
                '                        ElseIf txt_DiffMeter.Visible And txt_DiffMeter.Enabled Then
                '                            txt_DiffMeter.Focus()
                '                        Else
                '                            chk_LotComplete.Focus()
                '                        End If

                '                    Else

                '                        For K = .CurrentCell.ColumnIndex + 1 To .ColumnCount - 1
                '                            If .Columns(K).Visible Then
                '                                .Focus()
                '                                .CurrentCell = .Rows(.CurrentRow.Index).Cells(K)
                '                                Return True
                '                            End If
                '                        Next


                '                    End If
                '                End If

                '                Return True

                '            ElseIf keyData = Keys.Up Then

                '                If .CurrentCell.ColumnIndex <= 1 Then

                '                    If .CurrentCell.RowIndex = 0 Then

                '                        dgv_FP_Details.Focus()
                '                        dgv_FP_Details.CurrentCell = dgv_FP_Details.Rows(0).Cells(1)

                '                    Else

                '                        .CurrentCell = .Rows(.CurrentCell.RowIndex - 1).Cells(.ColumnCount - 6)

                '                    End If

                '                Else

                '                    For K = .CurrentCell.ColumnIndex - 1 To 0 Step -1
                '                        If .Columns(K).Visible Then
                '                            .CurrentCell = .Rows(.CurrentRow.Index).Cells(K)
                '                            Return True
                '                        End If
                '                    Next

                '                    '.CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex - 1)

                '                End If

                '                Return True

                '            Else

                '                Return MyBase.ProcessCmdKey(msg, keyData)

                '            End If

                '        End With

                '    Else

                '        Return MyBase.ProcessCmdKey(msg, keyData)

                '    End If

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
        Dim Dt2 As New DataTable
        Dim cmd As New SqlClient.SqlCommand
        Dim trans As SqlClient.SqlTransaction
        Dim NewCode As String = ""
        Dim Qa As Windows.Forms.DialogResult

        '--If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.Entry_ProcessedFabric_ReceiptFrom_Processing, "~L~") = 0 And InStr(Common_Procedures.UR.Entry_ProcessedFabric_ReceiptFrom_Processing, "~D~") = 0 Then MessageBox.Show("You have No Rights to Delete", "DOES NOT INSERT...", MessageBoxButtons.OK, MessageBoxIcon.Error) : Exit Sub

        Qa = MessageBox.Show("Do you want to Delete?", "FOR DELETION...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2)
        If Qa = Windows.Forms.DialogResult.No Or Qa = Windows.Forms.DialogResult.Cancel Then
            Exit Sub
        End If

        Dim g As New Password
        g.ShowDialog()

        'If Trim(UCase(Common_Procedures.Password_Input)) <> "TSD123" Then
        '    MessageBox.Show("Invalid Password", "PASSWORD FAILED...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
        '    Exit Sub
        'End If

        NewCode = Pk_Condition + Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RecNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)
        If New_Entry = True Then
            MessageBox.Show("This is New Entry", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        Da = New SqlClient.SqlDataAdapter("select count(*) from Textile_Processing_Receipt_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Cloth_Processing_Receipt_Code = '" & Trim(NewCode) & "' and  Cloth_Processing_BillMaking_Code <> ''", con)
        Dt1 = New DataTable
        Da.Fill(Dt1)
        If Dt1.Rows.Count > 0 Then
            If IsDBNull(Dt1.Rows(0)(0).ToString) = False Then
                If Val(Dt1.Rows(0)(0).ToString) > 0 Then
                    MessageBox.Show("Already BillMaking Prepared", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Exit Sub
                End If
            End If
        End If
        Dt1.Clear()

        Da = New SqlClient.SqlDataAdapter("select sum(Inspection_Meters) from Textile_Processing_Receipt_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Cloth_Processing_Receipt_Code = '" & Trim(NewCode) & "'", con)
        Dt1 = New DataTable
        Da.Fill(Dt1)
        If Dt1.Rows.Count > 0 Then
            If IsDBNull(Dt1.Rows(0)(0).ToString) = False Then
                If Val(Dt1.Rows(0)(0).ToString) > 0 Then
                    MessageBox.Show("Already Inspection Prepared", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Exit Sub
                End If
            End If
        End If
        Dt1.Clear()


        trans = con.BeginTransaction

        Try

            cmd.Connection = con
            cmd.Transaction = trans


            cmd.CommandText = "Truncate Table TempTable_For_NegativeStock"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Insert into TempTable_For_NegativeStock ( Stock_Type, Reference_Code, Reference_Date, Company_Idno, Ledger_Idno         , Item_IdNo, Rack_IdNo ) " &
                                    " Select                               'PI'    , Reference_Code, Reference_Date, Company_IdNo, DeliveryTo_StockIdNo, Item_IdNo, Rack_IdNo from Stock_Item_Processing_Details where Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Stock_Cloth_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Update Textile_Processing_Delivery_Details set Receipt_Meters = a.Receipt_Meters - (b.Receipt_Meters+b.ExcSht_Meters) , Receipt_Pcs = a.Receipt_Pcs - (b.Receipt_Pcs) , Receipt_Qty = a.Receipt_Qty - (b.Receipt_Qty) , Receipt_Weight = a.Receipt_Weight - (b.Receipt_Weight) from Textile_Processing_Delivery_Details a, Textile_Processing_Receipt_Details b Where b.Cloth_Processing_Receipt_Code = '" & Trim(NewCode) & "' and a.Cloth_Processing_Delivery_code = b.Cloth_Processing_Delivery_code and a.Cloth_Processing_Delivery_SlNo = b.Cloth_Processing_Delivery_SlNo"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Update Textile_Processing_Delivery_Details set Receipt_Meters = a.Receipt_Meters - (b.Receipt_Meters+b.ExcSht_Meters) , Receipt_Pcs = a.Receipt_Pcs - (b.Receipt_Pcs) , Receipt_Qty = a.Receipt_Qty - (b.Receipt_Qty) , Receipt_Weight = a.Receipt_Weight - (b.Receipt_Weight) from Textile_Processing_Delivery_Details a, Textile_Processing_Receipt_Details b Where b.Cloth_Processing_Receipt_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' and a.Cloth_Processing_Delivery_code = b.Cloth_Processing_Delivery_code and a.Cloth_Processing_Delivery_SlNo = b.Cloth_Processing_Delivery_SlNo"
            cmd.ExecuteNonQuery()

            '----Lot Complete status

            cmd.CommandText = "Update  Textile_Processing_Delivery_Details set Lot_Complete_status = 0, Lot_Complete_Code = '' where Lot_Complete_Code  = '" & Trim(Pk_Condition) & Trim(NewCode) & "'  "
            cmd.ExecuteNonQuery()

            cmd.CommandText = "delete from Textile_Processing_Receipt_Details where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Cloth_Processing_Receipt_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "delete from Textile_Processing_Receipt_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and ClothProcess_Receipt_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()



            cmd.CommandText = "Delete from Textile_Processing_Delivery_Head Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and ClothProcess_Delivery_Code = '" & Trim(Pk_Condition2) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Textile_Processing_Delivery_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Cloth_Processing_Delivery_Code = '" & Trim(Pk_Condition2) & Trim(NewCode) & "' and Receipt_Meters = 0 And Return_Meters = 0"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Stock_Cloth_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition2) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            '--------FP

            cmd.CommandText = "Delete from FP_Receipt_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and FP_Receipt_Processing_Code = '" & Trim(NewCode) & "' "
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Stock_Item_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()


            '--------CB

            Dim NewCode_1 As String

            NewCode_1 = Pk_Condition1 & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RecNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            cmd.CommandText = "Delete from Textile_Processing_Receipt_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Cloth_Processing_receipt_Code = '" & Trim(NewCode_1) & "' and Cloth_Processing_BillMaking_Code = '' and Inspection_Meters = 0"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Stock_Cloth_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(NewCode_1) & "'"
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

            MessageBox.Show("Deleted Successfully!!!", "FOR DELETION...", MessageBoxButtons.OK, MessageBoxIcon.Information)

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

            da = New SqlClient.SqlDataAdapter("select Ledger_DisplayName from Ledger_AlaisHead order by Ledger_DisplayName", con)
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
        pnl_Filter.BringToFront()
        pnl_Back.Enabled = False
        If dtp_Filter_Fromdate.Enabled And dtp_Filter_Fromdate.Visible Then dtp_Filter_Fromdate.Focus()

    End Sub

    Public Sub movefirst_record() Implements Interface_MDIActions.movefirst_record

        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim movno As String = ""

        Try

            da = New SqlClient.SqlDataAdapter("select top 1 ClothProcess_Receipt_No from Textile_Processing_Receipt_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and ClothProcess_Receipt_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "'  and  ClothProcess_Receipt_Code Like '%" & Pk_Condition & "%' Order by for_Orderby, ClothProcess_Receipt_No", con)
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

            OrdByNo = Common_Procedures.OrderBy_CodeToValue(Trim(lbl_RecNo.Text))

            da = New SqlClient.SqlDataAdapter("select top 1 ClothProcess_Receipt_No from Textile_Processing_Receipt_Head where for_orderby > " & Str(Val(OrdByNo)) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and ClothProcess_Receipt_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' and ClothProcess_Receipt_Code Like '%" & Pk_Condition & "%'  Order by for_Orderby, ClothProcess_Receipt_No", con)
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

            OrdByNo = Common_Procedures.OrderBy_CodeToValue(Trim(lbl_RecNo.Text))

            da = New SqlClient.SqlDataAdapter("select top 1 ClothProcess_Receipt_No from Textile_Processing_Receipt_Head where for_orderby < " & Str(Val(OrdByNo)) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and ClothProcess_Receipt_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' and ClothProcess_Receipt_Code Like '%" & Pk_Condition & "%' Order by for_Orderby desc, ClothProcess_Receipt_No desc", con)
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
            da = New SqlClient.SqlDataAdapter("select top 1 ClothProcess_Receipt_No from Textile_Processing_Receipt_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and ClothProcess_Receipt_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "'  and ClothProcess_Receipt_Code Like '%" & Pk_Condition & "%' Order by for_Orderby desc, ClothProcess_Receipt_No desc", con)
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

            Displaying = True

            clear()

            New_Entry = True

            lbl_RecNo.Text = Common_Procedures.get_MaxCode(con, "Textile_Processing_Receipt_Head", "ClothProcess_Receipt_Code", "For_OrderBy", "ClothProcess_Receipt_Code LIKE '%" & Pk_Condition & "%'", Val(lbl_Company.Tag), Common_Procedures.FnYearCode)
            lbl_RecNo.ForeColor = Color.Red

            Dim Prev_Rec_Type As String = ""

            da = New SqlClient.SqlDataAdapter("select Receipt_Type from Textile_Processing_Receipt_Head where For_OrderBy = (Select max(For_OrderBy) from  Textile_Processing_Receipt_Head where ClothProcess_Receipt_Code like '%" & Common_Procedures.FnYearCode & "%' and ClothProcess_Receipt_Code Like '%" & Pk_Condition & "%' And Company_IdNo = " & Val(lbl_Company.Tag).ToString & ")", con)
            da.Fill(dt)

            If dt.Rows.Count > 0 Then
                If Not IsDBNull(dt.Rows(0).Item(0)) Then
                    cbo_Receipt_Type.Text = dt.Rows(0).Item(0)
                End If
            End If

            If cbo_Receipt_Type.Text = "DELIVERY" Then
                'On Error Resume Next
                btn_Selection.Enabled = True
                'dgv_Details.Columns(0).ReadOnly = True
                dgv_Fabric_Details.Columns(1).ReadOnly = True
                dgv_Fabric_Details.Columns(2).ReadOnly = True
                dgv_Fabric_Details.Columns(4).ReadOnly = True
                dgv_Fabric_Details.Columns(5).ReadOnly = True
                dgv_Fabric_Details.AllowUserToAddRows = False
            Else
                'On Error Resume Next
                btn_Selection.Enabled = False
                'dgv_Details.Columns(0).ReadOnly = False
                dgv_Fabric_Details.Columns(1).ReadOnly = False
                dgv_Fabric_Details.Columns(2).ReadOnly = False
                dgv_Fabric_Details.Columns(4).ReadOnly = False
                dgv_Fabric_Details.Columns(5).ReadOnly = False
                dgv_Fabric_Details.AllowUserToAddRows = True
            End If

            pnl_Back.Visible = True

            If dtp_Date.Enabled And dtp_Date.Visible Then dtp_Date.Focus()

        Catch ex As Exception

            Displaying = False
            MessageBox.Show(ex.Message, "For New RECORD...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally

            Displaying = False

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

            inpno = InputBox("Enter Rec.No.", "For FINDING...")

            RecCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

            Da = New SqlClient.SqlDataAdapter("Select ClothProcess_Receipt_No from Textile_Processing_Receipt_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " And ClothProcess_Receipt_Code = '" & Trim(RecCode) & "'", con)
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
                MessageBox.Show("Rec No. does not exists", "DOES NOT FIND...", MessageBoxButtons.OK, MessageBoxIcon.Error)

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

        '  If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.Entry_ProcessedFabric_ReceiptFrom_Processing, "~L~") = 0 And InStr(Common_Procedures.UR.Entry_ProcessedFabric_ReceiptFrom_Processing, "~I~") = 0 Then MessageBox.Show("You have No Rights to Insert", "DOES NOT INSERT...", MessageBoxButtons.OK, MessageBoxIcon.Error) : Exit Sub

        Try

            inpno = InputBox("Enter New Rec No.", "FOR NEW RECEIPT INSERTION...")

            RecCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

            Da = New SqlClient.SqlDataAdapter("select ClothProcess_Receipt_No from Textile_Processing_Receipt_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and ClothProcess_Receipt_Code = '" & Trim(RecCode) & "'", con)
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
                    MessageBox.Show("Invalid Rec No", "DOES NOT INSERT NEW RECEIPT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

                Else
                    new_record()
                    Insert_Entry = True
                    lbl_RecNo.Text = Trim(UCase(inpno))

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
        Dim Led_ID As Integer = 0
        Dim Col_ID As Integer = 0
        Dim Itfp_ID As Integer = 0
        Dim Itgry_ID As Integer = 0
        Dim Sno As Integer = 0
        Dim Scno As Integer = 0
        Dim Partcls As String = ""
        Dim EntID As String = ""
        Dim Prc_id As Integer = 0
        Dim Comp_Prc_id As Integer = 0
        Dim PBlNo As String = ""
        Dim vTotPcs As String, vTotMtrs As String, vtotqty As String, vtot100MTRS As String, vtotBITSMTRS As String
        Dim vTotFP_QTY As String, vTotFP_FABCONSMTRS As String
        Dim Proc_ID As Integer = 0
        Dim Lot_ID As Integer = 0
        Dim vTotWeight As Single, vExcSrt As Single
        Dim Tr_ID As Integer = 0
        Dim WagesCode As String = ""
        Dim PcsChkCode As String = ""
        Dim Nr As Integer = 0
        Dim Del_Id As Integer = 0
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
        Dim lotSts As Integer = 0
        Dim Stock_In As String
        Dim mtrspcs As Double
        Dim vClthRcptStk As Double = 0
        Dim ClthWarp_Idno As Integer = 0
        Dim StkOff_ID As Int16

        'Dim Lot_IdNo As Integer
        'Dim Lot_Code As String
        'Dim Lot_Code_forSelection As String
        'Dim dt2 As New DataTable

        If Common_Procedures.settings.CustomerCode = "1516" Then

            StkOff_ID = Common_Procedures.CommonLedger.OwnSort_Ac  ' Val(Common_Procedures.CommonLedger.Godown_Ac)

        End If

        ' If Common_Procedures.UserRight_Check(Common_Procedures.UR.Entry_ProcessedFabric_ReceiptFrom_Processing, New_Entry) = False Then Exit Sub

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

        Led_ID = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Ledger.Text)
        If Led_ID = 0 Then
            MessageBox.Show("Invalid Party Name", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If cbo_Ledger.Enabled And cbo_Ledger.Visible Then cbo_Ledger.Focus()
            Exit Sub
        End If

        Del_Id = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_DeliveryTo.Text)

        Dim DEL_LED_TYPE As String = Common_Procedures.get_FieldValue(con, "Ledger_Head", "Ledger_Type", "Ledger_IdNo = " & Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_DeliveryTo.Text))

        Tr_ID = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_TransportName.Text)

        If Trim(UCase(DEL_LED_TYPE)) = "GODOWN" Then
            cbo_ProcessingName.Text = ""
            Prc_id = 0
        Else
            Prc_id = Common_Procedures.Process_NameToIdNo(con, cbo_ProcessingName.Text)
        End If

        Comp_Prc_id = 0
        If Len(Trim(cbo_Process_Completed.Text)) > 0 Then
            Comp_Prc_id = Common_Procedures.Process_NameToIdNo(con, cbo_Process_Completed.Text)
        End If
        If Comp_Prc_id = 0 Then
            MessageBox.Show("Invalid Process (Completed) ", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If cbo_Process_Completed.Enabled And cbo_Process_Completed.Visible Then cbo_Process_Completed.Focus()
            Exit Sub
        End If

        lbl_UserName.Text = Common_Procedures.User.IdNo

        lotSts = 0

        If Del_Id = 0 Then
            MessageBox.Show("Invalid Delivery To Name", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If cbo_DeliveryTo.Enabled And cbo_DeliveryTo.Visible Then cbo_DeliveryTo.Focus()
            Exit Sub
        End If

        If DEL_LED_TYPE <> "GODOWN" And Prc_id = 0 Then
            MessageBox.Show("Invalid Process Name", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If cbo_ProcessingName.Enabled And cbo_ProcessingName.Visible Then cbo_ProcessingName.Focus()
            Exit Sub
        End If

        If chk_LotComplete.Checked = True Then lotSts = 1

        With dgv_Fabric_Details
            For i = 0 To dgv_Fabric_Details.RowCount - 1
                If Val(.Rows(i).Cells(dgvCol_Details.PCS).Value) <> 0 Or Val(.Rows(i).Cells(dgvCol_Details.METERS).Value) <> 0 Or Val(.Rows(i).Cells(dgvCol_Details.WEIGHT).Value) <> 0 Or Val(.Rows(i).Cells(dgvCol_Details.EXC_SHT_Mtr).Value) <> 0 Then

                    If Trim(dgv_Fabric_Details.Rows(i).Cells(dgvCol_Details.ITEM_FP).Value) = "" Then
                        MessageBox.Show("Invalid FP Item", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        If dgv_Fabric_Details.Enabled And dgv_Fabric_Details.Visible Then
                            dgv_Fabric_Details.Focus()
                            dgv_Fabric_Details.CurrentCell = dgv_Fabric_Details.Rows(i).Cells(dgvCol_Details.ITEM_FP)

                        End If
                        Exit Sub

                    End If

                    If Trim(dgv_Fabric_Details.Rows(i).Cells(dgvCol_Details.COLOUR).Value) = "" Then
                        MessageBox.Show("Invalid COLOUR Name", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        If dgv_Fabric_Details.Enabled And dgv_Fabric_Details.Visible Then
                            dgv_Fabric_Details.Focus()
                            dgv_Fabric_Details.CurrentCell = dgv_Fabric_Details.Rows(i).Cells(dgvCol_Details.COLOUR)

                        End If
                        Exit Sub
                    End If

                    If Trim(dgv_Fabric_Details.Rows(i).Cells(dgvCol_Details.PROCESSING).Value) = "" Then
                        MessageBox.Show("Invalid PROCESS Name", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        If dgv_Fabric_Details.Enabled And dgv_Fabric_Details.Visible Then
                            dgv_Fabric_Details.Focus()
                            dgv_Fabric_Details.CurrentCell = dgv_Fabric_Details.Rows(i).Cells(dgvCol_Details.PROCESSING)

                        End If
                        Exit Sub

                    End If

                    If Val(dgv_Fabric_Details.Rows(i).Cells(dgvCol_Details.METERS).Value) = 0 Then
                        MessageBox.Show("Invalid Meters..", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        If dgv_Fabric_Details.Enabled Then dgv_Fabric_Details.Focus()
                        dgv_Fabric_Details.CurrentCell = dgv_Fabric_Details.Rows(0).Cells(dgvCol_Details.METERS)
                        Exit Sub
                    End If

                    If cbo_Receipt_Type.Text = "DELIVERY" Then
                        If Trim(dgv_Fabric_Details.Rows(i).Cells(dgvCol_Details.Process_Delv_Code).Value) = "" Then
                            MessageBox.Show("Invalid Delivery Code", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                            If dgv_Fabric_Details.Enabled And dgv_Fabric_Details.Visible Then
                                dgv_Fabric_Details.Focus()
                                dgv_Fabric_Details.CurrentCell = dgv_Fabric_Details.Rows(i).Cells(dgvCol_Details.DC_NO)
                            End If
                            Exit Sub
                        End If

                        If Val(dgv_Fabric_Details.Rows(i).Cells(dgvCol_Details.Process_Delv_Slno).Value) = 0 Then
                            MessageBox.Show("Invalid Delivery Number", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                            If dgv_Fabric_Details.Enabled And dgv_Fabric_Details.Visible Then
                                dgv_Fabric_Details.Focus()
                                dgv_Fabric_Details.CurrentCell = dgv_Fabric_Details.Rows(i).Cells(dgvCol_Details.DC_NO)
                            End If
                            Exit Sub
                        End If
                    End If

                End If

            Next
        End With

        Total_Calculation()
        vTotPcs = 0 : vTotMtrs = 0 : vTotWeight = 0 : vtotqty = 0
        vtot100MTRS = 0 : vtotBITSMTRS = 0
        If dgv_Details_Total.RowCount > 0 Then
            vTotPcs = Val(dgv_Details_Total.Rows(0).Cells(dgvCol_Details.PCS).Value())
            vtotqty = Val(dgv_Details_Total.Rows(0).Cells(dgvCol_Details.QTY).Value())
            vTotMtrs = Val(dgv_Details_Total.Rows(0).Cells(dgvCol_Details.METERS).Value())
            vtot100MTRS = Val(dgv_Details_Total.Rows(0).Cells(dgvCol_Details.METERS_100PERC_FOLDING).Value)
            vTotWeight = Val(dgv_Details_Total.Rows(0).Cells(dgvCol_Details.WEIGHT).Value())
            vtotBITSMTRS = Val(dgv_Details_Total.Rows(0).Cells(dgvCol_Details.BITSMETERS).Value)
            vExcSrt = Val(dgv_Details_Total.Rows(0).Cells(dgvCol_Details.EXC_SHT_Mtr).Value())
        End If

        vTotFP_QTY = 0
        vTotFP_FABCONSMTRS = 0
        If dgv_Details_FP_Total.Rows.Count > 0 Then
            vTotFP_QTY = Val(dgv_Details_FP_Total.Rows(0).Cells(dgvCol_Details_FP.QUANTITY).Value)
            vTotFP_FABCONSMTRS = Val(dgv_Details_FP_Total.Rows(0).Cells(dgvCol_Details_FP.Fabric_Consumption).Value)
        End If


        Dt1.Clear()

        tr = con.BeginTransaction

        Try

            If Insert_Entry = True Or New_Entry = False Then

                NewCode = Pk_Condition & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RecNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            Else

                lbl_RecNo.Text = Common_Procedures.get_MaxCode(con, "Textile_Processing_Receipt_Head", "ClothProcess_Receipt_Code", "For_OrderBy", " ClothProcess_Receipt_Code Like '%" & Pk_Condition & "%' ", Val(lbl_Company.Tag), Common_Procedures.FnYearCode, tr)
                NewCode = Pk_Condition & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RecNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            End If

            cmd.Connection = con
            cmd.Transaction = tr

            cmd.Parameters.Clear()
            cmd.Parameters.AddWithValue("@RecDate", dtp_Date.Value.Date)

            cmd.CommandText = "Truncate Table TempTable_For_NegativeStock"
            cmd.ExecuteNonQuery()


            If New_Entry = True Then

                cmd.CommandText = "Insert into Textile_Processing_Receipt_Head (ClothProcess_Receipt_Code, Company_IdNo                     , ClothProcess_Receipt_No       , for_OrderBy                                                            , ClothProcess_Receipt_Date, Ledger_IdNo             , Job_No                        , Transport_IdNo         , Freight_Charges                   , DeliveryTo_IdNo     ,Total_Pcs                ,Total_Qty            , Total_Meters              , Total_100Folding_Meters      , Total_Weight                 ,          Total_BitsMeters     , Total_ExcessShort    ,Lot_Status         ,Party_Dc_No                        ,Processing_Idno    ,   User_IdNo                    , Vehicle_No                        ,Receipt_Type                   ,   Process_IdNo             ,            Total_FP          ,   Total_Fabric_ConsumptionMeters   ,            ExcShtMeters_Percentage          ) " &
                                  "Values                                      ('" & Trim(NewCode) & "'  , " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_RecNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_RecNo.Text))) & ", @RecDate                 , " & Str(Val(Led_ID)) & ", '" & Trim(txt_JobNo.Text) & "', " & Str(Val(Tr_ID)) & ", " & Str(Val(txt_Frieght.Text)) & ",  " & Val(Del_Id) & "," & Str(Val(vTotPcs)) & "," & Val(vtotqty) & " , " & Str(Val(vTotMtrs)) & ", " & Str(Val(vtot100MTRS)) & ", " & Str(Val(vTotWeight)) & " , " & Str(Val(vtotBITSMTRS)) & ", " & Val(vExcSrt) & "," & Val(lotSts) & ", '" & Trim(txt_PartyDcNo.Text) & "'," & Val(Prc_id) & ", " & Val(lbl_UserName.Text) & " , '" & Trim(cbo_VehicleNo.Text) & "','" & cbo_Receipt_Type.Text & "'," & Comp_Prc_id.ToString & ",  " & Str(Val(vTotFP_QTY)) & ", " & Str(Val(vTotFP_FABCONSMTRS)) & ", " & Str(Val(lbl_Total_ExcShtPerc.Text)) & " ) "
                cmd.ExecuteNonQuery()

            Else

                cmd.CommandText = "Update Textile_Processing_Receipt_Head set ClothProcess_Receipt_Date = @RecDate, Ledger_IdNo = " & Val(Led_ID) & ", Job_No = '" & Trim(txt_JobNo.Text) & "' , Transport_IdNo = " & Val(Tr_ID) & ", Freight_Charges = " & Val(txt_Frieght.Text) & ", DeliveryTo_IdNo = " & Val(Del_Id) & ", Total_Pcs = " & Val(vTotPcs) & " , Vehicle_No =  '" & Trim(cbo_VehicleNo.Text) & "' ,Total_Qty = " & Val(vtotqty) & " , Total_Meters = " & Val(vTotMtrs) & ", Total_100Folding_Meters = " & Str(Val(vtot100MTRS)) & ", Total_Weight = " & Val(vTotWeight) & " , Total_BitsMeters = " & Str(Val(vtotBITSMTRS)) & ", Total_ExcessShort = " & Val(vExcSrt) & " , Lot_Status = " & Val(lotSts) & " , Party_Dc_No = '" & Trim(txt_PartyDcNo.Text) & "', Processing_Idno = " & Val(Prc_id) & ",  User_IdNo  = " & Val(lbl_UserName.Text) & ", Receipt_Type = '" & cbo_Receipt_Type.Text & "', Process_IdNo = " & Comp_Prc_id.ToString & ", Total_FP = " & Str(Val(vTotFP_QTY)) & " , Total_Fabric_ConsumptionMeters = " & Str(Val(vTotFP_FABCONSMTRS)) & ", ExcShtMeters_Percentage = " & Str(Val(lbl_Total_ExcShtPerc.Text)) & " Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and ClothProcess_Receipt_Code = '" & Trim(NewCode) & "'"
                Nr = cmd.ExecuteNonQuery()

                cmd.CommandText = "Update Textile_Processing_Delivery_Details set Receipt_Meters = a.Receipt_Meters - (b.Receipt_Meters + b.ExcSht_Meters), Receipt_Pcs = a.Receipt_Pcs - (b.Receipt_Pcs) , Receipt_Qty = a.Receipt_Qty - (b.Receipt_Qty) , Receipt_Weight = a.Receipt_Weight - (b.Receipt_Weight) from Textile_Processing_Delivery_Details a, Textile_Processing_Receipt_Details b Where b.Cloth_Processing_Receipt_Code = '" & Trim(NewCode) & "' and a.Cloth_Processing_Delivery_code = b.Cloth_Processing_Delivery_code and a.Cloth_Processing_Delivery_SlNo = b.Cloth_Processing_Delivery_SlNo"
                Nr = cmd.ExecuteNonQuery()

                '----Lot Complete status

                cmd.CommandText = "Update  Textile_Processing_Delivery_Details set Lot_Complete_status = 0, Lot_Complete_Code = '' where Lot_Complete_Code  = '" & Trim(Pk_Condition) & Trim(NewCode) & "'  "
                Nr = cmd.ExecuteNonQuery()


                cmd.CommandText = "Insert into TempTable_For_NegativeStock ( Stock_Type, Reference_Code, Reference_Date, Company_Idno, Ledger_Idno         , Item_IdNo, Rack_IdNo ) " &
                                   " Select                               'PI'    , Reference_Code, Reference_Date, Company_IdNo, DeliveryTo_StockIdNo, Item_IdNo, Rack_IdNo from Stock_Item_Processing_Details where Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()

            End If

            cmd.CommandText = "Delete from Textile_Processing_Receipt_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Cloth_Processing_receipt_Code = '" & Trim(NewCode) & "' and Cloth_Processing_BillMaking_Code = '' and Inspection_Meters = 0"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Stock_Cloth_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            Partcls = "Rec : Dc.No. " & Trim(lbl_RecNo.Text)
            PBlNo = Trim(lbl_RecNo.Text)
            EntID = Trim(Pk_Condition) & Trim(lbl_RecNo.Text)

            With dgv_Fabric_Details

                Sno = 0
                Scno = 0

                For i = 0 To .RowCount - 1

                    If Val(.Rows(i).Cells(dgvCol_Details.METERS).Value) <> 0 Or Val(.Rows(i).Cells(dgvCol_Details.WEIGHT).Value) <> 0 Or Val(.Rows(i).Cells(dgvCol_Details.EXC_SHT_Mtr).Value) <> 0 Then

                        Itfp_ID = Common_Procedures.Cloth_NameToIdNo(con, .Rows(i).Cells(dgvCol_Details.ITEM_FP).Value, tr)
                        Col_ID = Common_Procedures.Colour_NameToIdNo(con, .Rows(i).Cells(dgvCol_Details.COLOUR).Value, tr)

                        'If Common_Procedures.settings.CustomerCode = "1061" or Common_Procedures.settings.CustomerCode = "1558" Then

                        Lot_ID = Common_Procedures.Lot_NoToIdNo(con, .Rows(i).Cells(dgvCol_Details.LOT_NO).Value, tr)

                        If Val(.Rows(i).Cells(dgvCol_Details.FOLDING).Value) = 0 Then
                            .Rows(i).Cells(dgvCol_Details.FOLDING).Value = "100"
                        End If

                        Proc_ID = Common_Procedures.Process_NameToIdNo(con, .Rows(i).Cells(dgvCol_Details.PROCESSING).Value, tr)

                        Sno = Sno + 1
                        Scno = Scno + 1

                        Nr = 0

                        cmd.CommandText = "Insert into Textile_Processing_Receipt_Details (Cloth_Processing_Receipt_Code,            Company_IdNo           , Cloth_Processing_Receipt_No          ,                                  for_OrderBy                              ,          Cloth_Processing_Receipt_Date   ,                            Sl_No                                   ,                    Dc_Rc_No                               ,      Ledger_Idno     ,              Item_Idno                                           ,          Item_To_Idno     , Colour_Idno           , Processing_Idno       ,       Lot_IdNo     ,                        Folding                                  ,                  Receipt_Pcs                         ,                  Receipt_Qty                         ,                      Receipt_Meters                          ,                      Meters_in_100Folding                                    ,                      Receipt_Weight                           ,                      Bits_Meters                                  ,                  ExcSht_Meters                                     ,                    Cloth_Processing_Delivery_code                       ,                        Cloth_Processing_Delivery_Slno                      ,                    Processed_Fabric_Inspection_Code                                   ) " &
                                                                                "Values   ( '" & Trim(NewCode) & "'     , " & Str(Val(lbl_Company.Tag)) & " ,      '" & Trim(lbl_RecNo.Text) & "'  , " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_RecNo.Text))) & "   ,       @RecDate                           ,        " & Str(Val(.Rows(i).Cells(dgvCol_Details.SLNO).Value)) & " , '" & Trim(.Rows(i).Cells(dgvCol_Details.DC_NO).Value) & "', " & Val(Led_ID) & "   , " & Str(Val(.Rows(i).Cells(dgvCol_Details.ITEM_GREY).Value)) & ", " & Str(Val(Itfp_ID)) & " ,    " & Val(Col_ID) & ", " & Val(Proc_ID) & "  ," & Val(Lot_ID) & " ,   " & Str(Val(.Rows(i).Cells(dgvCol_Details.FOLDING).Value)) & ", " & Val(.Rows(i).Cells(dgvCol_Details.PCS).Value) & ", " & Val(.Rows(i).Cells(dgvCol_Details.QTY).Value) & ", " & Str(Val(.Rows(i).Cells(dgvCol_Details.METERS).Value)) & ", " & Str(Val(.Rows(i).Cells(dgvCol_Details.METERS_100PERC_FOLDING).Value)) & ", " & Str(Val(.Rows(i).Cells(dgvCol_Details.WEIGHT).Value)) & " , " & Str(Val(.Rows(i).Cells(dgvCol_Details.BITSMETERS).Value)) & " , " & Str(Val(.Rows(i).Cells(dgvCol_Details.EXC_SHT_Mtr).Value)) & " , '" & Trim(.Rows(i).Cells(dgvCol_Details.Process_Delv_Code).Value) & "'  ,   " & Str(Val(.Rows(i).Cells(dgvCol_Details.Process_Delv_Slno).Value)) & " , '" & Trim(.Rows(i).Cells(dgvCol_Details.Processed_Fabric_Inspection_Code).Value) & "' ) "
                        cmd.ExecuteNonQuery()

                        ' End If

                        If Trim(UCase(cbo_Receipt_Type.Text)) = "DELIVERY" Then

                            Nr = 0
                            If Trim(.Rows(i).Cells(dgvCol_Details.Process_Delv_Code).Value) <> "" And Val(.Rows(i).Cells(dgvCol_Details.Process_Delv_Slno).Value) <> 0 Then
                                cmd.CommandText = "Update Textile_Processing_Delivery_Details set Receipt_Meters = Receipt_Meters + (" & Str(Val(.Rows(i).Cells(dgvCol_Details.METERS).Value) + Val(.Rows(i).Cells(dgvCol_Details.EXC_SHT_Mtr).Value)) & "), Receipt_Pcs = Receipt_Pcs + " & Str(Val(.Rows(i).Cells(dgvCol_Details.PCS).Value)) & " , Receipt_Qty = Receipt_Qty + " & Str(Val(.Rows(i).Cells(dgvCol_Details.QTY).Value)) & "  ,  Receipt_Weight = Receipt_Weight + " & Str(Val(.Rows(i).Cells(dgvCol_Details.WEIGHT).Value)) & "  Where Cloth_Processing_Delivery_code = '" & Trim(.Rows(i).Cells(dgvCol_Details.Process_Delv_Code).Value) & "' and Cloth_Processing_Delivery_SlNo = " & Str(Val(.Rows(i).Cells(dgvCol_Details.Process_Delv_Slno).Value)) & " and Ledger_IdNo = " & Str(Val(Led_ID))
                                Nr = cmd.ExecuteNonQuery()
                            Else
                                Throw New ApplicationException("Invalid Delivery Details")
                            End If

                            If Nr <> 1 Then
                                Throw New ApplicationException("Mismatch of Delivery and Party Details")
                            End If

                        End If

                        '----Lot Complete status

                        If Val(lotSts) = 1 Then
                            cmd.CommandText = "Update  Textile_Processing_Delivery_Details set Lot_Complete_status = " & Str(Val(lotSts)) & ", Lot_Complete_Code  = '" & Trim(Pk_Condition) & Trim(NewCode) & "' Where Lot_Complete_status = 0 and Cloth_Processing_Delivery_code = '" & Trim(.Rows(i).Cells(dgvCol_Details.Process_Delv_Code).Value) & "' and Cloth_Processing_Delivery_SlNo = " & Str(Val(.Rows(i).Cells(dgvCol_Details.Process_Delv_Slno).Value)) & " and Ledger_IdNo = " & Str(Val(Led_ID))
                            Nr = cmd.ExecuteNonQuery()
                        End If

                        'If Nr = 0 Then
                        '    Throw New ApplicationException("Mismatch of Order and Party Details")
                        'End If                        '-----------------------cheran

                        Da = New SqlClient.SqlDataAdapter("Select a.* from  Cloth_Head  a Where a.Cloth_IdNo = " & Str(Val(Itfp_ID)), con)
                        Da.SelectCommand.Transaction = tr
                        Dt1 = New DataTable
                        Da.Fill(Dt1)

                        Sno = 0

                        If Dt1.Rows.Count > 0 Then

                            Stock_In = ""
                            mtrspcs = 0

                            Stock_In = Dt1.Rows(0)("Stock_In").ToString
                            mtrspcs = Val(Dt1.Rows(0)("Meters_Pcs").ToString)

                            vClthRcptStk = Val(.Rows(i).Cells(dgvCol_Details.METERS).Value)
                            If Trim(UCase(Stock_In)) = "PCS" Then
                                If Val(mtrspcs) = 0 Then mtrspcs = 1
                                vClthRcptStk = vClthRcptStk / mtrspcs

                            Else
                                vClthRcptStk = vClthRcptStk

                            End If

                            Sno = Sno + 1

                        End If

                        Dt1.Clear()

                        '-------------------------------------- -Allow Shortage

                        da1 = New SqlClient.SqlDataAdapter("select sum(a.Receipt_Meters) as Rec_Mtr, sum(a.ExcSht_Meters) as Exc_Mtr from Textile_Processing_Receipt_Details a  Where a.Cloth_Processing_Delivery_code = '" & Trim(.Rows(i).Cells(dgvCol_Details.Process_Delv_Code).Value) & "' and a.Cloth_Processing_Delivery_SlNo = " & Str(Val(.Rows(i).Cells(dgvCol_Details.Process_Delv_Slno).Value)) & "", con)
                        da1.SelectCommand.Transaction = tr
                        Dt1 = New DataTable
                        da1.Fill(Dt1)
                        If Dt1.Rows.Count > 0 Then
                            Rec_Mtr = Val(Dt1.Rows(0).Item("Rec_Mtr").ToString)
                            '  Excs_Mtr_Rec = Dt1.Rows(0).Item("Exc_Mtr").ToString
                        End If
                        Dt1.Dispose()
                        da1.Dispose()

                        da2 = New SqlClient.SqlDataAdapter("select sum(a.Return_Meters) as Retn_Mtr, sum(a.ExcSht_Meters) as Exc_Mtr from Textile_Processing_Return_Details a  Where a.Cloth_Processing_Delivery_code = '" & Trim(.Rows(i).Cells(dgvCol_Details.Process_Delv_Code).Value) & "' and a.Cloth_Processing_Delivery_SlNo = " & Str(Val(.Rows(i).Cells(dgvCol_Details.Process_Delv_Slno).Value)) & "", con)
                        da2.SelectCommand.Transaction = tr
                        dt2 = New DataTable
                        da2.Fill(dt2)
                        If dt2.Rows.Count > 0 Then
                            If IsDBNull(dt2.Rows(0).Item("Retn_Mtr").ToString) = False Then
                                Retn_Mtr = Val(dt2.Rows(0).Item("Retn_Mtr").ToString)
                            End If

                            ' Excs_Mtr_Retn = dt2.Rows(0).Item("Exc_Mtr").ToString

                        End If
                        dt2.Dispose()
                        da2.Dispose()

                        da1 = New SqlClient.SqlDataAdapter("select a.* , A.Receipt_Meters as Rec_Mtr_Bef , b.* , b.Receipt_Meters as Rec_Mtr ,b.Return_Meters as Retn_Mtr , C.* from Textile_Processing_Receipt_Details a LEFT OUTER JOIN Textile_Processing_Delivery_Details b On a.Cloth_Processing_Delivery_Code = b.Cloth_Processing_Delivery_Code and a.Cloth_Processing_Delivery_SlNo = b.Cloth_Processing_Delivery_SlNo INNER JOIN Cloth_Head c ON c.Cloth_Idno = b.Item_to_IdNo Where a.Cloth_Processing_Receipt_Code = '" & Trim(NewCode) & "' and a.Cloth_Processing_Receipt_Slno = " & Str(Val(.Rows(i).Cells(13).Value)) & " And a.Sl_No = " & Str(Val(Sno)), con)
                        da1.SelectCommand.Transaction = tr
                        Dt1 = New DataTable
                        da1.Fill(Dt1)
                        If Dt1.Rows.Count > 0 Then
                            Delv_Mtr = Dt1.Rows(0).Item("Delivery_Meters").ToString
                            Allow_Sht_Perc = Dt1.Rows(0).Item("Allow_Shortage_Perc").ToString
                        End If
                        Dt1.Dispose()
                        da1.Dispose()

                        Allow_Sht_Mtr = Delv_Mtr * Allow_Sht_Perc / 100
                        Ent_Sht_Mtr = Delv_Mtr - Rec_Mtr - Retn_Mtr

                        If Ent_Sht_Mtr > Allow_Sht_Mtr Then
                            '  MessageBox.Show("Invalid Shortage Meter" & Chr(13) & "Allowed Shortage (%): " & " " & Str(Val(Allow_Sht_Mtr)) & " "(" " & Str(Val(Allow_Sht_Perc)) & " ")"  " & Chr(13) & "Actual Shortage (%): " & " " & Str(Val(Ent_Sht_Mtr)) & "", "DOES NOT SAVE.!!!", MessageBoxButtons.OK, MessageBoxIcon.Information)
                        End If

                        '----------------------------------------

                    End If

                Next

            End With

            '-------------------------------------------------------------------------------------

            If Val(Common_Procedures.settings.NegativeStock_Restriction) = 1 Then

                cmd.CommandText = "Insert into TempTable_For_NegativeStock ( Stock_Type, Reference_Code, Reference_Date, Company_Idno, Ledger_Idno           , Item_IdNo, Rack_IdNo ) " &
                                    " Select                               'PI'    , Reference_Code, Reference_Date, Company_IdNo, ReceivedFrom_StockIdNo, Item_IdNo,     0        from Stock_Item_Processing_Details where Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()

                If Common_Procedures.Check_is_Negative_Stock_Status(con, tr) = True Then Exit Sub

            End If

            cmd.CommandText = "Delete from Textile_Processing_Delivery_Head Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and ClothProcess_Delivery_Code = '" & Trim(Pk_Condition2) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Textile_Processing_Delivery_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Cloth_Processing_Delivery_Code = '" & Trim(Pk_Condition2) & Trim(NewCode) & "' and Receipt_Meters = 0 And Return_Meters = 0"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Stock_Cloth_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition2) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            If Len(Trim(Process_Outputs)) = 0 Then
                Process_Outputs = "00"
            End If

            If Microsoft.VisualBasic.Right(Process_Outputs, 1) = "1" Then

                cmd.CommandText = "Delete from FP_Receipt_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and FP_Receipt_Processing_Code = '" & Trim(NewCode) & "' "
                cmd.ExecuteNonQuery()

                cmd.CommandText = "Delete from Stock_Item_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()

                Partcls = "Sew : Job.No. " & Trim(lbl_RecNo.Text)
                PBlNo = Trim(lbl_RecNo.Text)
                EntID = Trim(Pk_Condition) & Trim(lbl_RecNo.Text)

                With dgv_FP_Details
                    Sno = 0
                    For i = 0 To .RowCount - 1

                        If Val(.Rows(i).Cells(4).Value) <> 0 Then

                            Sno = Sno + 1

                            Itfp_ID = Common_Procedures.Processed_Item_NameToIdNo(con, .Rows(i).Cells(dgvCol_Details_FP.ITEM_FP).Value, tr)
                            Col_ID = Common_Procedures.Colour_NameToIdNo(con, .Rows(i).Cells(dgvCol_Details_FP.COLOUR).Value, tr)
                            Lot_ID = Common_Procedures.Lot_NoToIdNo(con, .Rows(i).Cells(dgvCol_Details_FP.LOT_NO).Value, tr)

                            Sno = Sno + 1

                            cmd.CommandText = "Insert into FP_Receipt_Processing_Details ( FP_Receipt_Processing_Code,            Company_IdNo          ,    FP_Receipt_Processing_No   ,                               for_OrderBy                              , FP_Receipt_Processing_Date,            Sl_No     ,           Ledger_IdNo    ,            FP_IdNo       ,           Colour_Idno   ,                  Quantity                                    ,        Lot_IdNo         ,            Process_idNo      ,        FP_Delivery_Processing_Code                                       ,  FP_Delivery_Processing_Details_Slno                                   ,                  Passed_Mtrs                                     ,                  Reject_Mtrs                                    ,                  Meters_per_Qty                                  ,                  Fabric_Consumption_Meters                             ) " &
                                                "           Values                       ( '" & Trim(NewCode) & "'   , " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_RecNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_RecNo.Text))) & ",        @RecDate           , " & Str(Val(Sno)) & ", " & Str(Val(Led_ID)) & " , " & Str(Val(Itfp_ID)) & ", " & Str(Val(Col_ID)) & ", " & Val(.Rows(i).Cells(dgvCol_Details_FP.QUANTITY).Value) & ", " & Str(Val(Lot_ID)) & ", " & Str(Val(Comp_Prc_id)) & ", '" & Trim(.Rows(i).Cells(dgvCol_Details_FP.Process_Delv_Code).Value) & "', " & Val(.Rows(i).Cells(dgvCol_Details_FP.Process_Delv_Slno).Value) & " , " & Val(.Rows(i).Cells(dgvCol_Details_FP.Passed_Mtrs).Value) & " , " & Val(.Rows(i).Cells(dgvCol_Details_FP.Reject_Mtrs).Value) & ", " & Val(.Rows(i).Cells(dgvCol_Details_FP.Mtrs_per_Qty).Value) & ", " & Val(.Rows(i).Cells(dgvCol_Details_FP.Fabric_Consumption).Value) & ") "
                            cmd.ExecuteNonQuery()

                            cmd.CommandText = " Insert into Stock_Item_Processing_Details ( Reference_Code                ,         Company_IdNo             ,           Reference_No        ,                               for_OrderBy                              , Reference_Date , ReceivedFrom_StockIdNo  ,    DeliveryTo_StockIdNo  ,         Entry_ID     ,       Party_Bill_No  ,       Particulars        ,  Sl_No            , Lot_IdNo           , Item_IdNo               , Quantity                                                             ,  Meters                                                               ) " &
                                         " Values                            ('" & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_RecNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_RecNo.Text))) & ",  @RecDate   , " & Str(Val(Led_ID)) & "   , " & Del_Id.ToString & "  , '" & Trim(EntID) & "', '" & Trim(PBlNo) & "', '" & Trim(Partcls) & "'  , " & i.ToString & "," & Str(Lot_ID) & " , " & Itfp_ID.ToString & "," & Val(.Rows(i).Cells(dgvCol_Details_FP.QUANTITY).Value).ToString & "," & Val(.Rows(i).Cells(dgvCol_Details_FP.Fabric_Consumption).Value) & ") "
                            cmd.ExecuteNonQuery()

                        End If

                    Next

                End With

            End If

            Dim NewCode_1 As String

            NewCode_1 = Pk_Condition1 & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RecNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            cmd.CommandText = "Delete from Textile_Processing_Receipt_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " And Cloth_Processing_receipt_Code = '" & Trim(NewCode_1) & "' and Cloth_Processing_BillMaking_Code = '' and Inspection_Meters = 0"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Stock_Cloth_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(NewCode_1) & "'"
            cmd.ExecuteNonQuery()

            'If dgv_Details_CB.Rows.Count > 0 Then

            '    Partcls = "Rec : Dc.No. " & Trim(lbl_RecNo.Text)
            '    PBlNo = Trim(lbl_RecNo.Text)
            '    EntID = Trim(Pk_Condition) & Trim(lbl_RecNo.Text)

            '    With dgv_Details_CB

            '        Sno = 0
            '        Scno = 0

            '        For i = 0 To .RowCount - 1

            '            If Val(.Rows(i).Cells(dgvCol_Details_CB.METERS).Value) <> 0 Then

            '                Itfp_ID = Common_Procedures.Cloth_NameToIdNo(con, .Rows(i).Cells(dgvCol_Details_CB.ITEM_CB).Value, tr)
            '                Col_ID = Common_Procedures.Colour_NameToIdNo(con, .Rows(i).Cells(dgvCol_Details_CB.COLOUR).Value, tr)
            '                Lot_ID = Common_Procedures.Lot_NoToIdNo(con, .Rows(i).Cells(dgvCol_Details_CB.LOT_NO).Value, tr)
            '                Proc_ID = Common_Procedures.Process_NameToIdNo(con, cbo_Process_Completed.Text, tr)

            '                Sno = Sno + 1
            '                Scno = Scno + 1

            '                Nr = 0

            '                cmd.CommandText = "Insert into Textile_Processing_Receipt_Details(Cloth_Processing_Receipt_Code,            Company_IdNo             , Cloth_Processing_Receipt_No          ,                                  for_OrderBy                              ,          Cloth_Processing_Receipt_Date   ,                            Sl_No                                   ,      Ledger_Idno      ,                  Item_To_Idno        , Colour_Idno           , Processing_Idno           ,       Lot_IdNo     ,                    Receipt_Meters                               ,Folding  ) " &
            '                                                                        "Values   ( '" & Trim(NewCode_1) & "'  ,    " & Str(Val(lbl_Company.Tag)) & ",      '" & Trim(lbl_RecNo.Text) & "'  , " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_RecNo.Text))) & "   ,       @RecDate                           ,        " & Str(Val(.Rows(i).Cells(dgvCol_Details.SLNO).Value)) & " , " & Val(Led_ID) & "   , " & Str(Val(Itfp_ID)) & "            ,    " & Val(Col_ID) & ", " & Val(Comp_Prc_id) & "  ," & Val(Lot_ID) & " , " & Str(Val(.Rows(i).Cells(dgvCol_Details_CB.METERS).Value)) & ",100      )"
            '                cmd.ExecuteNonQuery()


            '                cmd.CommandText = " Insert into Stock_Cloth_Processing_Details ( Reference_Code ,             Company_IdNo         ,           Reference_No        ,                               for_OrderBy                              , Reference_Date,     DeliveryTo_Idno      ,   ReceivedFrom_Idno    ,   Entry_ID           ,  Party_Bill_No             ,  Particulars               ,           Sl_No        ,           Cloth_Idno      ,   Folding    ,   Meters_Type1                                               ,StockOff_IdNo                ,Colour_IdNo        ,Process_IdNo             ,Lot_IdNo               ) " &
            '                          " Values                                   ('" & Trim(NewCode_1) & "' , " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_RecNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_RecNo.Text))) & ",  @RecDate     ,  " & Str(Val(Del_Id)) & "," & Str(Val(Led_ID)) & ", '" & Trim(EntID) & "',       '" & Trim(PBlNo) & "',     '" & Trim(Partcls) & "', " & Str(Val(Sno)) & " , " & Str(Val(Itfp_ID)) & "  ,100           , " & Str(Val(.Rows(i).Cells(dgvCol_Details_CB.METERS).Value)) & ", " & StkOff_ID.ToString & " ," & Str(Col_ID) & " ," & Str(Comp_Prc_id) & "," & Lot_ID.ToString & ") "
            '                cmd.ExecuteNonQuery()

            '            End If

            '        Next

            '    End With

            'End If


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

    Private Sub Total_Calculation()
        Dim vTotPcs As String, vTotMtrs As String, vtotweight As String, vtotqty As String, vExcsrt As String, vTot100Mtrs As String, vTotBitsMtrs As String
        Dim i As Integer
        Dim sno As Integer

        vTotPcs = 0 : vTotMtrs = 0 : vtotqty = 0 : vtotweight = 0 : sno = 0 : vExcsrt = 0 : vTot100Mtrs = 0 : vTotBitsMtrs = 0
        With dgv_Fabric_Details
            For i = 0 To dgv_Fabric_Details.Rows.Count - 1

                sno = sno + 1

                .Rows(i).Cells(dgvCol_Details.SLNO).Value = sno

                If Val(dgv_Fabric_Details.Rows(i).Cells(dgvCol_Details.PCS).Value) <> 0 Or Val(dgv_Fabric_Details.Rows(i).Cells(dgvCol_Details.QTY).Value) <> 0 Or Val(dgv_Fabric_Details.Rows(i).Cells(dgvCol_Details.METERS).Value) <> 0 Or Val(dgv_Fabric_Details.Rows(i).Cells(dgvCol_Details.WEIGHT).Value) <> 0 Or Val(dgv_Fabric_Details.Rows(i).Cells(dgvCol_Details.EXC_SHT_Mtr).Value) <> 0 Then

                    vTotPcs = vTotPcs + Val(dgv_Fabric_Details.Rows(i).Cells(dgvCol_Details.PCS).Value)
                    vtotqty = vtotqty + Val(dgv_Fabric_Details.Rows(i).Cells(dgvCol_Details.QTY).Value)
                    vTotMtrs = vTotMtrs + Val(dgv_Fabric_Details.Rows(i).Cells(dgvCol_Details.METERS).Value)
                    vTot100Mtrs = vTot100Mtrs + Val(dgv_Fabric_Details.Rows(i).Cells(dgvCol_Details.METERS_100PERC_FOLDING).Value)
                    vtotweight = vtotweight + Val(dgv_Fabric_Details.Rows(i).Cells(dgvCol_Details.WEIGHT).Value)
                    vTotBitsMtrs = vTotBitsMtrs + Val(dgv_Fabric_Details.Rows(i).Cells(dgvCol_Details.BITSMETERS).Value)
                    vExcsrt = vExcsrt + Val(dgv_Fabric_Details.Rows(i).Cells(dgvCol_Details.EXC_SHT_Mtr).Value)

                End If
            Next
        End With

        If dgv_Details_Total.Rows.Count <= 0 Then dgv_Details_Total.Rows.Add()
        dgv_Details_Total.Rows(0).Cells(dgvCol_Details.PCS).Value = Val(vTotPcs)
        dgv_Details_Total.Rows(0).Cells(dgvCol_Details.QTY).Value = Val(vtotqty)
        dgv_Details_Total.Rows(0).Cells(dgvCol_Details.METERS).Value = Format(Val(vTotMtrs), "#########0.00")
        dgv_Details_Total.Rows(0).Cells(dgvCol_Details.METERS_100PERC_FOLDING).Value = Format(Val(vTot100Mtrs), "#########0.00")
        dgv_Details_Total.Rows(0).Cells(dgvCol_Details.BITSMETERS).Value = Format(Val(vTotBitsMtrs), "#########0.00")
        dgv_Details_Total.Rows(0).Cells(dgvCol_Details.WEIGHT).Value = Format(Val(vtotweight), "#########0.000")
        dgv_Details_Total.Rows(0).Cells(dgvCol_Details.EXC_SHT_Mtr).Value = Format(Val(vExcsrt), "#########0.00")

        lbl_Total_100PercFolding_FabricMeters.Text = dgv_Details_Total.Rows(0).Cells(dgvCol_Details.METERS_100PERC_FOLDING).Value
        lbl_Total_BitsMeters.Text = dgv_Details_Total.Rows(0).Cells(dgvCol_Details.BITSMETERS).Value

        If Val(txt_ReceiptMeter.Text) <> 0 Then
            txt_DiffMeter.Text = txt_ReceiptMeter.Text - Format(Val(vTotMtrs), "#########0.00")
        End If

        '---------------------------

        sno = 0

        Dim vTotFPQty As Integer = 0
        Dim VRejectMts As Integer = 0
        Dim VPassedMts As Integer = 0
        Dim vTotFP_FABMts As String = 0



        vTotFP_FABMts = 0
        With dgv_FP_Details
            For i = 0 To dgv_FP_Details.Rows.Count - 1

                sno = sno + 1

                .Rows(i).Cells(dgvCol_Details_FP.SLNO).Value = sno

                If Val(dgv_FP_Details.Rows(i).Cells(dgvCol_Details_FP.QUANTITY).Value) <> 0 Or Val(dgv_FP_Details.Rows(i).Cells(dgvCol_Details_FP.Reject_Mtrs).Value) <> 0 Then

                    vTotFPQty = vTotFPQty + Val(dgv_FP_Details.Rows(i).Cells(dgvCol_Details_FP.QUANTITY).Value)

                    VRejectMts = VRejectMts + Val(dgv_FP_Details.Rows(i).Cells(dgvCol_Details_FP.Reject_Mtrs).Value)

                    VPassedMts = Val(VPassedMts) + Val(dgv_FP_Details.Rows(i).Cells(dgvCol_Details_FP.Passed_Mtrs).Value)
                    vTotFP_FABMts = Format(Val(vTotFP_FABMts) + Val(dgv_FP_Details.Rows(i).Cells(dgvCol_Details_FP.Fabric_Consumption).Value), "##########0.00")
                End If

            Next

        End With

        If dgv_Details_FP_Total.Rows.Count <= 0 Then dgv_Details_FP_Total.Rows.Add()

        dgv_Details_FP_Total.Rows(0).Cells(dgvCol_Details_FP.QUANTITY).Value = Val(vTotFPQty)
        dgv_Details_FP_Total.Rows(0).Cells(dgvCol_Details_FP.Reject_Mtrs).Value = Val(VRejectMts)
        dgv_Details_FP_Total.Rows(0).Cells(dgvCol_Details_FP.Passed_Mtrs).Value = Val(VPassedMts)
        dgv_Details_FP_Total.Rows(0).Cells(dgvCol_Details_FP.Fabric_Consumption).Value = Format(Val(vTotFP_FABMts), "##########0.00")

        lbl_Total_BedsheetMeters.Text = dgv_Details_FP_Total.Rows(0).Cells(dgvCol_Details_FP.Fabric_Consumption).Value

        lbl_Total_ExcShtMeters.Text = Format(Val(lbl_Total_100PercFolding_FabricMeters.Text) - Val(lbl_Total_BitsMeters.Text) - Val(lbl_Total_BedsheetMeters.Text), "#######0.00")

        lbl_Total_ExcShtPerc.Text = ""
        If Val(lbl_Total_100PercFolding_FabricMeters.Text) > 0 Then
            lbl_Total_ExcShtPerc.Text = Format((Val(lbl_Total_ExcShtMeters.Text) * 100) / Val(lbl_Total_100PercFolding_FabricMeters.Text), "#######0.00")
        End If

        '---------------------------

        'sno = 0

        'Dim vTotCBMtrs As Double

        'With dgv_Details_CB
        '    For i = 0 To dgv_Details_CB.Rows.Count - 1

        '        sno = sno + 1

        '        .Rows(i).Cells(dgvCol_Details_CB.SLNO).Value = sno

        '        If Val(dgv_Details_CB.Rows(i).Cells(dgvCol_Details_CB.METERS).Value) <> 0 Then

        '            vTotCBMtrs = vTotCBMtrs + Val(dgv_Details_CB.Rows(i).Cells(dgvCol_Details_CB.METERS).Value)

        '        End If
        '    Next
        'End With

        'If dgv_Details_CB_Total.Rows.Count <= 0 Then dgv_Details_CB_Total.Rows.Add()
        'dgv_Details_CB_Total.Rows(0).Cells(dgvCol_Details_CB.METERS).Value = Val(vTotCBMtrs)

    End Sub
    Private Sub cbo_Ledger_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Ledger.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'GODOWN' or ( Ledger_Type = '' AND (AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14) ))", "(Ledger_idno = 0)")

    End Sub
    Private Sub cbo_Ledger_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Ledger.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Ledger, cbo_Receipt_Type, cbo_Process_Completed, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'GODOWN' or ( Ledger_Type = '' AND (AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14) ))", "(Ledger_idno = 0)")
    End Sub

    Private Sub cbo_Ledger_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Ledger.KeyPress

        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Ledger, cbo_Process_Completed, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'GODOWN' or ( Ledger_Type = '' AND (AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14) )) ", "(Ledger_idno = 0)")



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
    Private Sub dgv_Details_CellClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Fabric_Details.CellClick

        With dgv_Fabric_Details
            If e.ColumnIndex = 8 Then
                Show_Item_CurrentStock(e.RowIndex)
                .Focus()
            End If
        End With

    End Sub
    Private Sub dgv_Details_CellEndEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Fabric_Details.CellEndEdit
        With dgv_Fabric_Details

            If .CurrentCell.ColumnIndex = dgvCol_Details.METERS Or .CurrentCell.ColumnIndex = dgvCol_Details.METERS_100PERC_FOLDING Or .CurrentCell.ColumnIndex = dgvCol_Details.BITSMETERS Then
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

    Private Sub dgv_Details_CellEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Fabric_Details.CellEnter

        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim Dt2 As New DataTable
        Dim Dt3 As New DataTable
        Dim Dt4 As New DataTable
        Dim rect As Rectangle

        With dgv_Fabric_Details

            If Val(.CurrentRow.Cells(dgvCol_Details.SLNO).Value) = 0 Then
                .CurrentRow.Cells(dgvCol_Details.SLNO).Value = .CurrentRow.Index + 1
            End If

            If e.ColumnIndex = 2 Then

                If (cbo_grid_Cloth_Fabric.Visible = False Or Val(cbo_grid_Cloth_Fabric.Tag) <> e.RowIndex) And Not dgv_Fabric_Details.Columns(2).ReadOnly Then

                    cbo_grid_Cloth_Fabric.Tag = -1
                    Da = New SqlClient.SqlDataAdapter("select Processed_Item_Name from Processed_Item_Head where Processed_Item_Type = 'FP' order by Processed_Item_Name", con)
                    Dt1 = New DataTable
                    Da.Fill(Dt1)
                    cbo_grid_Cloth_Fabric.DataSource = Dt1
                    cbo_grid_Cloth_Fabric.DisplayMember = "Procesed_Item_Name"

                    rect = .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False)

                    cbo_grid_Cloth_Fabric.Left = .Left + rect.Left  '  .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Left
                    cbo_grid_Cloth_Fabric.Top = .Top + rect.Top  ' .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Top
                    cbo_grid_Cloth_Fabric.Width = rect.Width  ' .CurrentCell.Size.Width
                    cbo_grid_Cloth_Fabric.Height = rect.Height  ' rect.Height

                    cbo_grid_Cloth_Fabric.Text = .CurrentCell.Value  '  Trim(.CurrentRow.Cells(e.ColumnIndex).Value)

                    cbo_grid_Cloth_Fabric.Tag = Val(e.RowIndex)
                    cbo_grid_Cloth_Fabric.Visible = True

                    cbo_grid_Cloth_Fabric.BringToFront()
                    cbo_grid_Cloth_Fabric.Focus()



                End If

            Else

                cbo_grid_Cloth_Fabric.Visible = False

            End If

            If e.ColumnIndex = 3 Then

                If (cbo_grid_Colour_Fabric.Visible = False Or Val(cbo_grid_Colour_Fabric.Tag) <> e.RowIndex) And Not dgv_Fabric_Details.Columns(3).ReadOnly Then

                    cbo_grid_Colour_Fabric.Tag = -1
                    Da = New SqlClient.SqlDataAdapter("select Colour_Name from Colour_Head order by Colour_Name", con)
                    Dt2 = New DataTable
                    Da.Fill(Dt2)
                    cbo_grid_Colour_Fabric.DataSource = Dt2
                    cbo_grid_Colour_Fabric.DisplayMember = "Colour_Name"

                    rect = .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False)

                    cbo_grid_Colour_Fabric.Left = .Left + rect.Left
                    cbo_grid_Colour_Fabric.Top = .Top + rect.Top
                    cbo_grid_Colour_Fabric.Width = rect.Width
                    cbo_grid_Colour_Fabric.Height = rect.Height

                    cbo_grid_Colour_Fabric.Text = .CurrentCell.Value

                    cbo_grid_Colour_Fabric.Tag = Val(e.RowIndex)
                    cbo_grid_Colour_Fabric.Visible = True

                    cbo_grid_Colour_Fabric.BringToFront()
                    cbo_grid_Colour_Fabric.Focus()



                End If

            Else

                cbo_grid_Colour_Fabric.Visible = False


            End If


            If e.ColumnIndex = 4 Then

                If (cbo_grid_Processing_Fabric.Visible = False Or Val(cbo_grid_Processing_Fabric.Tag) <> e.RowIndex) And Not dgv_Fabric_Details.Columns(4).ReadOnly Then

                    cbo_grid_Processing_Fabric.Tag = -1

                    Da = New SqlClient.SqlDataAdapter("select Process_Name from Process_Head order by Process_Name", con)
                    Dt3 = New DataTable
                    Da.Fill(Dt3)

                    cbo_grid_Processing_Fabric.DataSource = Dt3
                    cbo_grid_Processing_Fabric.DisplayMember = "Process_Name"

                    rect = .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False)

                    cbo_grid_Processing_Fabric.Left = .Left + rect.Left  '  .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Left
                    cbo_grid_Processing_Fabric.Top = .Top + rect.Top  ' .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Top
                    cbo_grid_Processing_Fabric.Width = rect.Width  ' .CurrentCell.Size.Width
                    cbo_grid_Processing_Fabric.Height = rect.Height  ' rect.Height

                    cbo_grid_Processing_Fabric.Text = .CurrentCell.Value  '  Trim(.CurrentRow.Cells(e.ColumnIndex).Value)

                    cbo_grid_Processing_Fabric.Tag = Val(e.RowIndex)
                    cbo_grid_Processing_Fabric.Visible = True

                    cbo_grid_Processing_Fabric.BringToFront()
                    cbo_grid_Processing_Fabric.Focus()

                End If

            Else

                cbo_grid_Processing_Fabric.Visible = False

            End If

            If e.ColumnIndex = 5 Then

                If (cbo_grid_LotNo_Fabric.Visible = False Or Val(cbo_grid_LotNo_Fabric.Tag) <> e.RowIndex) And Not dgv_Fabric_Details.Columns(5).ReadOnly Then

                    cbo_grid_LotNo_Fabric.Tag = -1
                    Da = New SqlClient.SqlDataAdapter("select Lot_No from Lot_Head order by Lot_No", con)
                    Dt4 = New DataTable
                    Da.Fill(Dt4)
                    cbo_grid_LotNo_Fabric.DataSource = Dt4
                    cbo_grid_LotNo_Fabric.DisplayMember = "Lot_No"

                    rect = .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False)

                    cbo_grid_LotNo_Fabric.Left = .Left + rect.Left  '  .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Left
                    cbo_grid_LotNo_Fabric.Top = .Top + rect.Top  ' .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Top
                    cbo_grid_LotNo_Fabric.Width = rect.Width  ' .CurrentCell.Size.Width
                    cbo_grid_LotNo_Fabric.Height = rect.Height  ' rect.Height

                    cbo_grid_LotNo_Fabric.Text = .CurrentCell.Value  '  Trim(.CurrentRow.Cells(e.ColumnIndex).Value)

                    cbo_grid_LotNo_Fabric.Tag = Val(e.RowIndex)
                    cbo_grid_LotNo_Fabric.Visible = True

                    cbo_grid_LotNo_Fabric.BringToFront()
                    cbo_grid_LotNo_Fabric.Focus()

                    'cbo_Grid_CountName.Visible = False
                    'cbo_Grid_MillName.Visible = False

                End If

            Else

                cbo_grid_LotNo_Fabric.Visible = False
                'cbo_Grid_MillName.Tag = -1
                'cbo_Grid_MillName.Text = ""

            End If

            If e.ColumnIndex = 8 And dgv_LevColNo <> 8 Then
                Show_Item_CurrentStock(e.RowIndex)
                .Focus()
            End If

        End With
    End Sub

    Private Sub dgv_Details_CellLeave(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Fabric_Details.CellLeave

        With dgv_Fabric_Details
            dgv_LevColNo = .CurrentCell.ColumnIndex
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
        End With
    End Sub

    Private Sub dgv_Details_CellValueChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Fabric_Details.CellValueChanged
        On Error Resume Next

        If IsNothing(dgv_Fabric_Details.CurrentCell) Then Exit Sub
        With dgv_Fabric_Details
            If .Visible Then
                If .CurrentCell.ColumnIndex = dgvCol_Details.FOLDING Or .CurrentCell.ColumnIndex = dgvCol_Details.PCS Or .CurrentCell.ColumnIndex = dgvCol_Details.QTY Or .CurrentCell.ColumnIndex = dgvCol_Details.METERS Or .CurrentCell.ColumnIndex = dgvCol_Details.WEIGHT Or .CurrentCell.ColumnIndex = dgvCol_Details.BITSMETERS Or .CurrentCell.ColumnIndex = dgvCol_Details.EXC_SHT_Mtr Then

                    If .CurrentCell.ColumnIndex = dgvCol_Details.FOLDING Or .CurrentCell.ColumnIndex = dgvCol_Details.METERS Then

                        Dim vMTRS100FLDG As String = 0
                        Dim vFLDPERC As String = 0

                        vFLDPERC = Val(dgv_Details_Total.Rows(e.RowIndex).Cells(dgvCol_Details.FOLDING).Value)
                        If Val(vFLDPERC) = 0 Then vFLDPERC = 100
                        vMTRS100FLDG = Val(dgv_Details_Total.Rows(e.RowIndex).Cells(dgvCol_Details.METERS).Value) * Val(vFLDPERC) / 100
                        dgv_Fabric_Details.Rows(e.RowIndex).Cells(dgvCol_Details.METERS_100PERC_FOLDING).Value = Format(Val(vMTRS100FLDG), "#########0.00")

                    End If

                    Total_Calculation()

                End If
            End If
        End With
    End Sub


    Private Sub dgv_Details_EditingControlShowing(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewEditingControlShowingEventArgs) Handles dgv_Fabric_Details.EditingControlShowing
        dgtxt_details = CType(dgv_Fabric_Details.EditingControl, DataGridViewTextBoxEditingControl)
    End Sub

    Private Sub dgtxt_Details_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgtxt_details.Enter
        dgv_Fabric_Details.EditingControl.BackColor = Color.Lime
        Act_Ctrl = dgv_Fabric_Details.Name
    End Sub

    Private Sub dgtxt_details_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgtxt_details.KeyDown
        Try

            With dgv_Fabric_Details

                If e.KeyValue = Keys.Delete Then
                    If Trim(.Rows(.CurrentCell.RowIndex).Cells(14).Value) <> "" Then
                        e.Handled = True
                    End If
                    If Val(.Rows(.CurrentCell.RowIndex).Cells(15).Value) <> 0 Then
                        e.Handled = True
                    End If
                End If
            End With

        Catch ex As Exception

        End Try
    End Sub

    Private Sub dgtxt_Details_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dgtxt_details.KeyPress

        Try


            With dgv_Fabric_Details


                If Trim(.Rows(.CurrentCell.RowIndex).Cells(dgvCol_Details.rct_Code).Value) <> "" Then
                    e.Handled = True

                ElseIf Val(.Rows(.CurrentCell.RowIndex).Cells(dgvCol_Details.Processed_Fabric_Inspection_Code).Value) <> 0 Then
                    e.Handled = True

                Else

                    If Val(dgv_Fabric_Details.CurrentCell.ColumnIndex.ToString) = 6 Or Val(dgv_Fabric_Details.CurrentCell.ColumnIndex.ToString) = 7 Or Val(dgv_Fabric_Details.CurrentCell.ColumnIndex.ToString) = 8 Or Val(dgv_Fabric_Details.CurrentCell.ColumnIndex.ToString) = 9 Or Val(dgv_Fabric_Details.CurrentCell.ColumnIndex.ToString) = 10 Or Val(dgv_Fabric_Details.CurrentCell.ColumnIndex.ToString) = 11 Then

                        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
                            e.Handled = True
                        End If
                    End If
                End If
            End With
        Catch ex As Exception

        End Try
    End Sub
    Private Sub dgv_Details_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_Fabric_Details.KeyUp

        Dim n As Integer

        If e.Control = True And UCase(Chr(e.KeyCode)) = "D" Then

            With dgv_Fabric_Details

                n = .CurrentRow.Index

                If Trim(.Rows(n).Cells(dgvCol_Details.Processed_Fabric_Inspection_Code).Value) = "" And Val(.Rows(n).Cells(dgvCol_Details.Processed_Fabric_Inspection_Code).Value) = 0 Then
                    If .Rows.Count = 1 Then
                        For i = 0 To .Columns.Count - 1
                            .Rows(n).Cells(i).Value = ""
                        Next

                    Else
                        .Rows.RemoveAt(n)

                    End If
                End If

                For i = 0 To .Rows.Count - 1
                    .Rows(i).Cells(dgvCol_Details.SLNO).Value = i + 1
                Next

            End With

            Total_Calculation()

        End If


    End Sub


    Private Sub dgv_Details_RowsAdded(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewRowsAddedEventArgs) Handles dgv_Fabric_Details.RowsAdded
        Dim n As Integer

        With dgv_Fabric_Details
            If IsNothing(dgv_Fabric_Details.CurrentCell) Then Exit Sub
            n = .RowCount
            .Rows(n - 1).Cells(dgvCol_Details.SLNO).Value = Val(n)
        End With
    End Sub

    Private Sub cbo_LotNo_CB_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_LotNo_CB.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Lot_Head", "Lot_No", "", "(Lot_Idno=0)")

    End Sub

    Private Sub cbo_LotNo_CB_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_LotNo_CB.KeyDown

        vcbo_KeyDwnVal = e.KeyValue

        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_LotNo_CB, cbo_Colour_CB, Nothing, "Lot_Head", "Lot_No", "", "(Lot_Idno=0)")

        With dgv_Details_CB

            If (e.KeyValue = 38 And cbo_LotNo_CB.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
                .Focus()
                .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex - 1)
            End If

            If (e.KeyValue = 40 And cbo_LotNo_CB.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
                .Focus()
                If .Columns(.CurrentCell.ColumnIndex + 1).Visible And Not .Columns(.CurrentCell.ColumnIndex + 1).ReadOnly Then
                    .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 1)
                Else
                    .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 2)
                End If
            End If

        End With

    End Sub

    Private Sub cbo_LotNo_CB_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_LotNo_CB.KeyPress

        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_LotNo_CB, Nothing, "Lot_Head", "Lot_No", "", "(Lot_Idno=0)")

        If Asc(e.KeyChar) = 13 Then

            With dgv_Details_CB

                .Focus()

                If .Columns(.CurrentCell.ColumnIndex + 1).Visible And Not .Columns(.CurrentCell.ColumnIndex + 1).ReadOnly Then
                    .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 1)
                Else
                    .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 2)
                End If

            End With

        End If

    End Sub


    Private Sub cbo_LotNo_CB_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_LotNo_CB.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Dim f As New LotNo_creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_LotNo_CB.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If
    End Sub


    Private Sub cbo_LotNo_CB_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_LotNo_CB.TextChanged
        Try
            If cbo_LotNo_CB.Visible Then
                With dgv_Details_CB
                    If Val(cbo_LotNo_CB.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = 3 Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_LotNo_CB.Text)
                    End If
                End With
            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub


    Private Sub cbo_LotNo_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_grid_LotNo_Fabric.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Lot_Head", "Lot_No", "", "(Lot_Idno=0)")

    End Sub

    Private Sub cbo_Lotno_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_grid_LotNo_Fabric.KeyDown

        vcbo_KeyDwnVal = e.KeyValue

        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_grid_LotNo_Fabric, cbo_grid_Processing_Fabric, Nothing, "Lot_Head", "Lot_No", "", "(Lot_Idno=0)")

        With dgv_Fabric_Details

            If (e.KeyValue = 38 And cbo_grid_LotNo_Fabric.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
                .Focus()
                .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex - 1)
            End If

            If (e.KeyValue = 40 And cbo_grid_LotNo_Fabric.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
                .Focus()
                If .Columns(.CurrentCell.ColumnIndex + 1).Visible And Not .Columns(.CurrentCell.ColumnIndex + 1).ReadOnly Then
                    .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 1)
                Else
                    .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 2)
                End If
            End If

        End With

    End Sub

    Private Sub cbo_lotno_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_grid_LotNo_Fabric.KeyPress

        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_grid_LotNo_Fabric, Nothing, "Lot_Head", "Lot_No", "", "(Lot_Idno=0)")

        If Asc(e.KeyChar) = 13 Then

            With dgv_Fabric_Details

                .Focus()

                If .Columns(.CurrentCell.ColumnIndex + 1).Visible And Not .Columns(.CurrentCell.ColumnIndex + 1).ReadOnly Then
                    .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 1)
                Else
                    .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 2)
                End If

            End With

        End If

    End Sub


    Private Sub cbo_Lotno_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_grid_LotNo_Fabric.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Dim f As New LotNo_creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_grid_LotNo_Fabric.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If
    End Sub


    Private Sub cbo_lotno_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_grid_LotNo_Fabric.TextChanged
        Try
            If cbo_grid_LotNo_Fabric.Visible Then
                With dgv_Fabric_Details
                    If Val(cbo_grid_LotNo_Fabric.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = 5 Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_grid_LotNo_Fabric.Text)
                    End If
                End With
            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub


    Private Sub cbo_LotNo_FP_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_grid_LotNo_FP.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Lot_Head", "Lot_No", "", "(Lot_Idno=0)")

    End Sub

    Private Sub cbo_LotNo_FP_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_grid_LotNo_FP.KeyDown

        vcbo_KeyDwnVal = e.KeyValue

        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_grid_LotNo_FP, cbo_grid_Colour_FP, Nothing, "Lot_Head", "Lot_No", "", "(Lot_Idno=0)")

        With dgv_FP_Details

            If (e.KeyValue = 38 And cbo_grid_LotNo_FP.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
                .Focus()
                .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex - 1)
            End If

            If (e.KeyValue = 40 And cbo_grid_LotNo_FP.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
                .Focus()
                If .Columns(.CurrentCell.ColumnIndex + 1).Visible And Not .Columns(.CurrentCell.ColumnIndex + 1).ReadOnly Then
                    .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 1)
                Else
                    .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 2)
                End If
            End If

        End With

    End Sub

    Private Sub cbo_LotNo_FP_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_grid_LotNo_FP.KeyPress

        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_grid_LotNo_FP, Nothing, "Lot_Head", "Lot_No", "", "(Lot_Idno=0)")

        If Asc(e.KeyChar) = 13 Then

            With dgv_FP_Details

                .Focus()

                If .Columns(.CurrentCell.ColumnIndex + 1).Visible And Not .Columns(.CurrentCell.ColumnIndex + 1).ReadOnly Then
                    .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 1)
                Else
                    .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 2)
                End If

            End With

        End If

    End Sub


    Private Sub cbo_LotNo_FP_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_grid_LotNo_FP.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Dim f As New LotNo_creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_grid_LotNo_FP.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If
    End Sub


    Private Sub cbo_LotNo_FP_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_grid_LotNo_FP.TextChanged
        Try
            If cbo_grid_LotNo_FP.Visible Then
                With dgv_FP_Details
                    If Val(cbo_grid_LotNo_FP.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = 3 Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_grid_LotNo_FP.Text)
                    End If
                End With
            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub


    Private Sub cbo_Colour_FP_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_grid_Colour_FP.GotFocus

        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "colour_Head", "colour_Name", "", "(Colour_IdNo = 0)")

    End Sub
    Private Sub cbo_Colour_FP_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_grid_Colour_FP.KeyDown

        vcbo_KeyDwnVal = e.KeyValue

        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_grid_Colour_FP, cbo_grid_FP, cbo_grid_LotNo_FP, "Colour_Head", "colour_Name", "", "(Colour_IdNo = 0)")

        With dgv_FP_Details

            If (e.KeyValue = 38 And cbo_grid_Colour_FP.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
                .Focus()
                .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex - 1)
            End If

            If (e.KeyValue = 40 And cbo_grid_Colour_FP.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
                .Focus()
                .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 1)
            End If

        End With

    End Sub

    Private Sub cbo_Colour_FP_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_grid_Colour_FP.KeyPress

        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_grid_Colour_FP, cbo_grid_LotNo_FP, "colour_Head", "colour_Name", "", "(Colour_IdNo = 0)")

        If Asc(e.KeyChar) = 13 Then

            With dgv_FP_Details

                .Focus()
                .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)

            End With

        End If

    End Sub


    Private Sub cbo_Colour_FP_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_grid_Colour_FP.KeyUp

        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then

            Dim f As New Color_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_grid_Colour_FP.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If

    End Sub


    Private Sub cbo_Colour_FP_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_grid_Colour_FP.TextChanged

        Try

            If cbo_grid_Colour_FP.Visible Then

                With dgv_FP_Details
                    If Val(cbo_grid_Colour_FP.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = 2 Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_grid_Colour_FP.Text)
                    End If
                End With

            End If

        Catch ex As Exception

            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub
    Private Sub cbo_Processing_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_grid_Processing_Fabric.GotFocus

        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Process_Head", "Process_Name", "", "(Process_Idno=0)")

    End Sub
    Private Sub cbo_processing_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_grid_Processing_Fabric.KeyDown

        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_grid_Processing_Fabric, cbo_grid_Colour_Fabric, cbo_grid_LotNo_Fabric, "Process_Head", "Process_Name", "", "(Process_Idno=0)")

        With dgv_Fabric_Details

            If (e.KeyValue = 38 And cbo_grid_Processing_Fabric.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
                .Focus()
                .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex - 1)
            End If

            If (e.KeyValue = 40 And cbo_grid_Processing_Fabric.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
                .Focus()
                .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 1)
            End If

        End With
    End Sub

    Private Sub cbo_processing_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_grid_Processing_Fabric.KeyPress

        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_grid_Processing_Fabric, cbo_grid_LotNo_Fabric, "Process_Head", "Process_Name", "", "(Process_Idno=0)")
        If Asc(e.KeyChar) = 13 Then

            With dgv_Fabric_Details

                .Focus()
                .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)

            End With

        End If
    End Sub

    Private Sub cbo_processing_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_grid_Processing_Fabric.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Dim f As New Process_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_grid_Processing_Fabric.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If
    End Sub


    Private Sub cbo_Processing_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_grid_Processing_Fabric.TextChanged
        Try
            If cbo_grid_Processing_Fabric.Visible Then
                With dgv_Fabric_Details
                    If Val(cbo_grid_Processing_Fabric.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = 4 Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_grid_Processing_Fabric.Text)
                    End If
                End With
            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub cbo_itemfp_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_grid_Cloth_Fabric.GotFocus

        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Cloth_Head", "Cloth_Name", "", "(Cloth_Idno = 0)")

    End Sub

    Private Sub cbo_itemfp_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_grid_Cloth_Fabric.KeyDown

        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_grid_Cloth_Fabric, Nothing, cbo_grid_Colour_Fabric, "Cloth_Head", "Cloth_Name", "", "(Cloth_Idno = 0)")

        With dgv_Fabric_Details

            If (e.KeyValue = 38 And cbo_grid_Cloth_Fabric.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
                .Focus()
                .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex - 1)
            End If

            If (e.KeyValue = 40 And cbo_grid_Cloth_Fabric.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
                .Focus()
                .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 1)
            End If

        End With

    End Sub

    Private Sub cbo_itemfp_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_grid_Cloth_Fabric.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_grid_Cloth_Fabric, cbo_grid_Colour_Fabric, "Cloth_Head", "Cloth_Name", "", "(Cloth_Idno = 0)")
        If Asc(e.KeyChar) = 13 Then

            With dgv_Fabric_Details

                If Trim(cbo_grid_Cloth_Fabric.Text) = "" And .CurrentCell.RowIndex = .Rows.Count - 1 Then
                    dgv_Details_CB.Focus()
                    dgv_Details_CB.CurrentCell = dgv_Details_CB.Rows(0).Cells(1)
                Else
                    .Focus()
                    .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)
                End If

            End With

        End If
    End Sub

    Private Sub cbo_itemfp_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_grid_Cloth_Fabric.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Dim f As New Cloth_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_grid_Cloth_Fabric.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If
    End Sub

    Private Sub cbo_itemfp_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_grid_Cloth_Fabric.TextChanged
        Try
            If cbo_grid_Cloth_Fabric.Visible Then
                With dgv_Fabric_Details
                    If Val(cbo_grid_Cloth_Fabric.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = 2 Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_grid_Cloth_Fabric.Text)
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

    Private Sub dgv_Details_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgv_Fabric_Details.GotFocus
        Act_Ctrl = dgv_Fabric_Details.Name.ToString
        dgv_Fabric_Details.Focus()
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
                Condt = "a.ClothProcess_Receipt_Date between '" & Trim(Format(dtp_Filter_Fromdate.Value, "MM/dd/yyyy")) & "' and '" & Trim(Format(dtp_Filter_ToDate.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_Filter_Fromdate.Value) = True Then
                Condt = "a.ClothProcess_Receipt_Date = '" & Trim(Format(dtp_Filter_Fromdate.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_Filter_ToDate.Value) = True Then
                Condt = "a.ClothProcess_Receipt_Date = '" & Trim(Format(dtp_Filter_ToDate.Value, "MM/dd/yyyy")) & "' "
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
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & "c.Processing_Idno = " & Str(Val(proc_IdNo)) & ")"
            End If

            If Trim(txt_filterpono.Text) <> "" Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & "a.Job_No = '" & Trim(txt_filterpono.Text) & "'"
            End If

            da = New SqlClient.SqlDataAdapter("select a.*, b.Ledger_Name,c.*,d.Processed_Item_Name,e.Process_Name from Textile_Processing_Receipt_Head a INNER JOIN Ledger_Head b on a.Ledger_IdNo = b.Ledger_IdNo INNER JOIN Textile_Processing_Receipt_Details c ON c.Cloth_Processing_Receipt_Code = a.ClothProcess_Receipt_Code INNER JOIN Processed_Item_Head d ON d.Processed_Item_IdNo = c.Item_To_IdNo LEFT OUTER JOIN Process_Head e ON c.Processing_Idno = e.Process_IdNo where a.company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.ClothProcess_Receipt_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "' " & IIf(Trim(Condt) <> "", " and ", "") & Condt & " Order by a.for_orderby, a.ClothProcess_Receipt_No", con)
            da.Fill(dt2)

            dgv_Filter_Details.Rows.Clear()

            If dt2.Rows.Count > 0 Then

                For i = 0 To dt2.Rows.Count - 1

                    n = dgv_Filter_Details.Rows.Add()


                    dgv_Filter_Details.Rows(n).Cells(dgvCol_Details.SLNO).Value = dt2.Rows(i).Item("ClothProcess_Receipt_No").ToString
                    dgv_Filter_Details.Rows(n).Cells(dgvCol_Details.DC_NO).Value = Format(Convert.ToDateTime(dt2.Rows(i).Item("ClothProcess_Receipt_Date").ToString), "dd-MM-yyyy")
                    dgv_Filter_Details.Rows(n).Cells(dgvCol_Details.ITEM_FP).Value = dt2.Rows(i).Item("Ledger_Name").ToString
                    dgv_Filter_Details.Rows(n).Cells(dgvCol_Details.COLOUR).Value = dt2.Rows(i).Item("Processed_Item_Name").ToString
                    dgv_Filter_Details.Rows(n).Cells(dgvCol_Details.PROCESSING).Value = dt2.Rows(i).Item("Process_Name").ToString
                    dgv_Filter_Details.Rows(n).Cells(dgvCol_Details.LOT_NO).Value = Val(dt2.Rows(i).Item("Total_Pcs").ToString)
                    dgv_Filter_Details.Rows(n).Cells(dgvCol_Details.PCS).Value = Val(dt2.Rows(i).Item("total_Qty").ToString)
                    dgv_Filter_Details.Rows(n).Cells(dgvCol_Details.QTY).Value = Format(Val(dt2.Rows(i).Item("Total_Meters").ToString), "########0.00")
                    dgv_Filter_Details.Rows(n).Cells(dgvCol_Details.METERS).Value = Format(Val(dt2.Rows(i).Item("Total_Weight").ToString), "########0.000")

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

        movno = Trim(dgv_Filter_Details.CurrentRow.Cells(dgvCol_Details.SLNO).Value)

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
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim NewCode As String

        NewCode = Pk_Condition + Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RecNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Try

            da1 = New SqlClient.SqlDataAdapter("select * from Textile_Processing_Receipt_Head Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and ClothProcess_Receipt_Code = '" & Trim(NewCode) & "'", con)
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

        If Val(Common_Procedures.Print_OR_Preview_Status) = 1 Then
            Try
                PrintDialog1.PrinterSettings = PrintDocument1.PrinterSettings
                If PrintDialog1.ShowDialog() = Windows.Forms.DialogResult.OK Then
                    PrintDocument1.PrinterSettings = PrintDialog1.PrinterSettings
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
        Dim Dt1 As New DataTable
        Dim cmd As New SqlClient.SqlCommand
        Dim NewCode As String

        NewCode = Pk_Condition + Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RecNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        prn_HdDt.Clear()
        prn_DetDt.Clear()
        prn_DetIndx = 0
        prn_DetSNo = 0
        prn_PageNo = 0

        Try

            da1 = New SqlClient.SqlDataAdapter("select a.*, b.*, c.*,d.Ledger_Name as TransportName from Textile_Processing_Receipt_Head a INNER JOIN Company_Head b ON a.Company_IdNo = b.Company_IdNo INNER JOIN Ledger_Head c ON a.Ledger_IdNo = c.Ledger_IdNo LEFT OUTER JOIN Ledger_Head d ON d.Ledger_IdNo = a.Ledger_IdNo where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.ClothProcess_Receipt_Code = '" & Trim(NewCode) & "'", con)
            prn_HdDt = New DataTable
            da1.Fill(prn_HdDt)

            If prn_HdDt.Rows.Count > 0 Then
                da2 = New SqlClient.SqlDataAdapter("select a.*, b.Cloth_Name as Processed_Grey_Name, c.Colour_Name ,d.Lot_No ,e.Process_Name  from Textile_Processing_Receipt_Details a LEFT OUTER JOIN CLOTH_Head b on a.Item_To_Idno = b.CLOTH_Idno LEFT OUTER JOIN Colour_Head c on a.Colour_IdNo = c.Colour_IdNo LEFT OUTER JOIN Lot_Head d ON d.Lot_IdNo = a.Lot_IdNo LEFT OUTER JOIN Process_Head e ON e.Process_IdNo = a.Processing_Idno where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Cloth_Processing_Receipt_Code = '" & Trim(NewCode) & "' Order by a.sl_no", con)
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

    Private Sub PrintDocument1_PrintPage(ByVal sender As System.Object, ByVal e As System.Drawing.Printing.PrintPageEventArgs) Handles PrintDocument1.PrintPage
        If prn_HdDt.Rows.Count <= 0 Then Exit Sub
        'If Trim(UCase(Common_Procedures.settings.CompanyName)) = "" Then
        Printing_Format1(e)
        'End If
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
        'PrintDocument pd = new PrintDocument();
        'pd.DefaultPageSettings.PaperSize = new System.Drawing.Printing.PaperSize("PaperA4", 840, 1180);
        'pd.Print();

        For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
            ps = PrintDocument1.PrinterSettings.PaperSizes(I)
            Debug.Print(ps.PaperName)
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

        ClArr(1) = Val(35) : ClArr(2) = 160 : ClArr(3) = 100 : ClArr(4) = 100 : ClArr(5) = 120 : ClArr(6) = 70 : ClArr(7) = 80
        ClArr(8) = PageWidth - (LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7))

        TxtHgt = 18 ' 19 ' e.Graphics.MeasureString("A", pFont).Height  ' 20

        EntryCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RecNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Try

            If prn_HdDt.Rows.Count > 0 Then

                Printing_Format1_PageHeader(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, prn_PageNo, NoofItems_PerPage, CurY, LnAr, ClArr)

                ' W1 = e.Graphics.MeasureString("No.of Beams  : ", pFont).Width

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
                        Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Colour_Name").ToString, LMargin + ClArr(1) + ClArr(2) + 10, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Lot_No").ToString, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + 10, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Process_Name").ToString, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + 10, CurY, 0, 0, pFont)
                        If Val(prn_DetDt.Rows(prn_DetIndx).Item("Receipt_Pcs").ToString) <> 0 Then
                            Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(prn_DetIndx).Item("Receipt_Pcs").ToString), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) - 10, CurY, 1, 0, pFont)
                        End If
                        If Val(prn_DetDt.Rows(prn_DetIndx).Item("Receipt_Meters").ToString) <> 0 Then
                            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Receipt_Meters").ToString), "########0.00"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) - 10, CurY, 1, 0, pFont)
                        End If
                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Receipt_Weight").ToString), "########0.000"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) - 10, CurY, 1, 0, pFont)


                        NoofDets = NoofDets + 1

                        If Trim(ItmNm2) <> "" Then
                            CurY = CurY + TxtHgt - 5
                            Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm2), LMargin + ClArr(1) + 10, CurY, 0, 0, pFont)
                            NoofDets = NoofDets + 1
                        End If

                        prn_DetIndx = prn_DetIndx + 1

                    Loop

                End If


                Printing_Format1_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, NoofDets, True)


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
        Dim C1 As Single, W1 As Single, S1 As Single
        Dim Cmp_Name As String, Cmp_Add1 As String, Cmp_Add2 As String
        Dim Cmp_PhNo As String, Cmp_TinNo As String, Cmp_CstNo As String

        PageNo = PageNo + 1

        CurY = TMargin

        da2 = New SqlClient.SqlDataAdapter("select a.*, b.Processed_Item_Name as Processed_Grey_Name, c.Colour_Name ,d.Lot_No ,e.Process_Name  from Textile_Processing_Delivery_Details a INNER JOIN Processed_Item_Head b on a.Item_IdNo = b.Processed_Item_IdNo LEFT OUTER JOIN Colour_Head c on a.Colour_IdNo = c.Colour_IdNo LEFT OUTER JOIN Lot_Head d ON d.Lot_IdNo = a.Lot_IdNo LEFT OUTER JOIN Process_Head e ON e.Process_IdNo = a.Processing_Idno where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Cloth_Processing_Delivery_Code = '" & Trim(EntryCode) & "' Order by a.sl_no", con)
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

        CurY = CurY + TxtHgt - 10
        p1Font = New Font("Calibri", 16, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "PROCESSING RECEIPT", LMargin, CurY, 2, PrintWidth, p1Font)
        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height



        CurY = CurY + strHeight + 10 ' + 150
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(2) = CurY

        Try
            C1 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5)
            W1 = e.Graphics.MeasureString(" P.O.NO  : ", pFont).Width
            S1 = e.Graphics.MeasureString("FROM  :    ", pFont).Width

            CurY = CurY + TxtHgt - 10
            p1Font = New Font("Calibri", 12, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "FROM  :  " & "M/s." & prn_HdDt.Rows(0).Item("Ledger_Name").ToString, LMargin + 10, CurY, 0, 0, p1Font)
            Common_Procedures.Print_To_PrintDocument(e, "REC.NO", LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("ClothProcess_Receipt_No").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, p1Font)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address1").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
            p1Font = New Font("Calibri", 14, FontStyle.Bold)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address2").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "DATE", LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("ClothProcess_Receipt_Date").ToString), "dd-MM-yyyy").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address3").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
            If Trim(prn_HdDt.Rows(0).Item("Job_No").ToString) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, " P.O.NO", LMargin + C1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Job_No").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, pFont)
            End If

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address4").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
            If Trim(prn_HdDt.Rows(0).Item("Vehicle_No").ToString) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, "Vehicle No :", LMargin + C1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Vehicle_No").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, pFont)
            End If

            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(3) = CurY

            e.Graphics.DrawLine(Pens.Black, LMargin + C1, LnAr(3), LMargin + C1, LnAr(2))

            CurY = CurY + TxtHgt - 10
            Common_Procedures.Print_To_PrintDocument(e, "SNO", LMargin, CurY, 2, ClAr(1), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "ITEM NAME(FP)", LMargin + ClAr(1), CurY, 2, ClAr(2), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "COLOUR", LMargin + ClAr(1) + ClAr(2), CurY, 2, ClAr(3), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "LOT NO", LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, 2, ClAr(4), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "PROCESSING", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 2, ClAr(5), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "PCS", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, 2, ClAr(6), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "METERS", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, 2, ClAr(7), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "WEIGHT", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, 2, ClAr(8), pFont)

            CurY = CurY + TxtHgt + 5
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
                    Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(0).Item("Total_Pcs").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY, 1, 0, pFont)
                End If
            End If
            If Val(prn_HdDt.Rows(0).Item("Total_Meters").ToString) <> 0 Then
                If is_LastPage = True Then
                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Total_Meters").ToString), "#########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, pFont)
                End If
            End If
            If Val(prn_HdDt.Rows(0).Item("Total_Weight").ToString) <> 0 Then
                If is_LastPage = True Then
                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Total_Weight").ToString), "#########0.000"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 10, CurY, 1, 0, pFont)
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
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9), LnAr(3))

            CurY = CurY + TxtHgt - 5

            If Val(prn_HdDt.Rows(0).Item("TransportName").ToString) <> 0 Then

                Common_Procedures.Print_To_PrintDocument(e, "Transport Name : " & Trim(prn_HdDt.Rows(0).Item("TransportName").ToString), LMargin + 10, CurY, 0, 0, pFont)


                CurY = CurY + TxtHgt + 10
                e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
                LnAr(7) = CurY

            End If

            'If Val(prn_HdDt.Rows(0).Item("Empty_Beam").ToString) <> 0 Then
            '    Common_Procedures.Print_To_PrintDocument(e, " Empty Beams : " & Trim(prn_HdDt.Rows(0).Item("Empty_Beam").ToString), PageWidth - 250, CurY, 0, 0, pFont)
            'End If

            CurY = CurY + TxtHgt

            If Val(Common_Procedures.User.IdNo) <> 1 Then
                Common_Procedures.Print_To_PrintDocument(e, "(" & Trim(Common_Procedures.User.Name) & ")", LMargin + 400, CurY, 0, 0, pFont)
            End If

            CurY = CurY + TxtHgt
            Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString
            Common_Procedures.Print_To_PrintDocument(e, "Receiver's Signature", LMargin + 20, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "Prepared By ", LMargin + 300, CurY, 0, 0, pFont)
            p1Font = New Font("Calibri", 12, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "For " & Cmp_Name, PageWidth - 15, CurY, 1, 0, p1Font)

            CurY = CurY + TxtHgt + 5

            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            e.Graphics.DrawLine(Pens.Black, LMargin, LnAr(1), LMargin, CurY)
            e.Graphics.DrawLine(Pens.Black, PageWidth, LnAr(1), PageWidth, CurY)

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub txt_Frieght_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Frieght.KeyPress
        If Asc(e.KeyChar) = 13 Then
            If dgv_Fabric_Details.Rows.Count > 0 Then
                dgv_Fabric_Details.Focus()
                dgv_Fabric_Details.CurrentCell = dgv_Fabric_Details.Rows(0).Cells(1)
                dgv_Fabric_Details.CurrentCell.Selected = True
            Else
                dgv_FP_Details.Focus()
                dgv_FP_Details.CurrentCell = dgv_FP_Details.Rows(0).Cells(1)
                dgv_FP_Details.CurrentCell.Selected = True
            End If
            e.Handled = True
        Else
            If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
                e.Handled = True
            End If
        End If
    End Sub

    Private Sub cbo_TransportName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_TransportName.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'TRANSPORT')", "(Ledger_IdNo = 0)")

    End Sub
    Private Sub cbo_TransportName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_TransportName.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_TransportName, cbo_DeliveryTo, cbo_VehicleNo, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'TRANSPORT')", "(Ledger_IdNo = 0)")
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

    Private Sub btn_Selection_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Selection.Click
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim i As Integer, j As Integer, n As Integer, SNo As Integer
        Dim LedIdNo As Integer
        Dim NewCode As String
        Dim CompIDCondt As String
        Dim Ent_Qty As Single = 0
        Dim Ent_Wgt As Single = 0
        Dim Ent_Pcs As Single = 0
        Dim Ent_Mtrs As Single = 0
        Dim Ent_BitsMtrs As String = 0
        Dim nr As Single = 0

        LedIdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Ledger.Text)

        If LedIdNo = 0 Then
            MessageBox.Show("Invalid Party Name", "DOES NOT SELECT ORDER...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If cbo_Ledger.Enabled And cbo_Ledger.Visible Then cbo_Ledger.Focus()
            Exit Sub
        End If

        NewCode = Pk_Condition + Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RecNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        CompIDCondt = "(a.company_idno = " & Str(Val(lbl_Company.Tag)) & ")"
        If Common_Procedures.settings.EntrySelection_Combine_AllCompany = 1 Then
            CompIDCondt = ""
            If Trim(UCase(Common_Procedures.User.Type)) = "ACCOUNT" Then
                CompIDCondt = "(tZ.Company_Type <> 'UNACCOUNT')"
            End If
        End If


        With dgv_Selection

            .Rows.Clear()
            SNo = 0

            If Common_Procedures.settings.Continuous_Fabric_Lot_No_for_Purchase_Weaver = 1 Then
                'If Common_Procedures.settings.CustomerCode = "1516" Then
                Da = New SqlClient.SqlDataAdapter("select a.* , b.*, tz.Company_ShortName, b.Folding as Fabric_Folding, b.item_Idno, e.Ledger_Name as Transportname,h.Receipt_Pcs As Ent_Pcs, h.Receipt_Meters as Ent_Mtrs, h.Receipt_Weight As Ent_Wgt, h.Receipt_Qty As Ent_Qty, h.Bits_Meters As Ent_BitsMeters, g.Cloth_Name as Fp_Item_Name ,gf.Cloth_Name as Del_Item_Name, I.Lot_No AS Lot_No , j.* , k.Colour_Name from Textile_Processing_Delivery_Head a  INNER JOIN Company_Head tZ ON a.company_idno <> 0 and a.company_idno = tZ.company_idno INNER JOIN Textile_Processing_Delivery_Details b ON b.Lot_Complete_status = 0 and a.ClothProcess_Delivery_Code = b.Cloth_Processing_Delivery_Code left outer JOIN Cloth_Head g ON g.Cloth_Idno = b.Item_to_IdNo   left outer JOIN Cloth_Head gf ON gf.Cloth_Idno = b.Item_IdNo LEFT OUTER JOIN Lot_Head i ON b.FabricPurchase_Weaver_Lot_IdNo = i.Lot_IdNo LEFT OUTER JOIN Process_Head J ON J.Process_IdNo = b.Processing_Idno LEFT OUTER JOIN Colour_Head k ON b.Colour_IdNo = k.Colour_IdNo LEFT OUTER JOIN Ledger_Head e ON a.Transport_IdNo = e.Ledger_IdNo LEFT OUTER JOIN Textile_Processing_Receipt_Details h ON h.Cloth_Processing_Receipt_Code = '" & Trim(NewCode) & "' and b.Cloth_Processing_Delivery_Code = h.Cloth_Processing_Delivery_Code and b.Cloth_Processing_Delivery_SlNo = h.Cloth_Processing_Delivery_SlNo   Where   " & CompIDCondt & IIf(Trim(CompIDCondt) <> "", " and ", "") & " a.ledger_Idno = " & Str(Val(LedIdNo)) & " and ((b.Delivery_Meters - b.Receipt_Meters - b.Return_Meters) > 0 or h.Receipt_Meters > 0 ) and a.Processing_IdNo = " & Common_Procedures.Process_NameToIdNo(con, cbo_Process_Completed.Text) & " Order by a.ClothProcess_Delivery_Date, a.for_orderby, a.ClothProcess_Delivery_No", con)
                'Else
                'Da = New SqlClient.SqlDataAdapter("select a.* , b.* ,B.Folding as Fabric_Folding, b.item_Idno, e.Ledger_Name as Transportname,h.Receipt_Pcs As Ent_Pcs, h.Receipt_Meters as Ent_Mtrs, h.Receipt_Weight As Ent_Wgt, h.Receipt_Qty As Ent_Qty, g.Cloth_Name as Fp_Item_Name , I.Lot_No AS Lot_No , j.* , k.Colour_Name from Textile_Processing_Delivery_Head a INNER JOIN Textile_Processing_Delivery_Details b ON b.Lot_Complete_status = 0 and a.ClothProcess_Delivery_Code = b.Cloth_Processing_Delivery_Code INNER JOIN Cloth_Head g ON g.Cloth_Idno = b.Item_to_IdNo  LEFT OUTER JOIN Lot_Head i ON b.FabricPurchase_Weaver_Lot_IdNo = i.Lot_IdNo LEFT OUTER JOIN Process_Head J ON J.Process_IdNo = b.Processing_Idno LEFT OUTER JOIN Colour_Head k ON b.Colour_IdNo = k.Colour_IdNo LEFT OUTER JOIN Ledger_Head e ON a.Transport_IdNo = e.Ledger_IdNo LEFT OUTER JOIN Textile_Processing_Receipt_Details h ON h.Cloth_Processing_Receipt_Code = '" & Trim(NewCode) & "' and b.Cloth_Processing_Delivery_Code = h.Cloth_Processing_Delivery_Code and b.Cloth_Processing_Delivery_SlNo = h.Cloth_Processing_Delivery_SlNo   Where   " & CompIDCondt & IIf(Trim(CompIDCondt) <> "", " and ", "") & " a.ledger_Idno = " & Str(Val(LedIdNo)) & " and ((b.Delivery_Meters - b.Receipt_Meters - b.Return_Meters) > 0 or h.Receipt_Meters > 0 ) Order by a.ClothProcess_Delivery_Date, a.for_orderby, a.ClothProcess_Delivery_No", con)
                'End If
            Else
                Da = New SqlClient.SqlDataAdapter("select a.* , b.* , tz.Company_ShortName, b.item_Idno, e.Ledger_Name as Transportname,h.Receipt_Pcs As Ent_Pcs, h.Receipt_Meters as Ent_Mtrs, h.Receipt_Weight As Ent_Wgt, h.Receipt_Qty As Ent_Qty, h.Bits_Meters As Ent_BitsMeters, g.Cloth_Name as Fp_Item_Name ,gf.Cloth_Name as Del_Item_Name, I.Lot_No , j.Process_Name , k.Colour_Name  from Textile_Processing_Delivery_Head a  INNER JOIN Company_Head tZ ON a.company_idno <> 0 and a.company_idno = tZ.company_idno INNER JOIN Textile_Processing_Delivery_Details b ON b.Lot_Complete_status = 0 and a.ClothProcess_Delivery_Code = b.Cloth_Processing_Delivery_Code INNER JOIN Cloth_Head g ON g.Cloth_Idno = b.Item_to_IdNo  LEFT OUTER JOIN Lot_Head i ON b.Lot_IdNo = i.Lot_IdNo LEFT OUTER JOIN Process_Head J ON J.Process_IdNo = b.Processing_Idno LEFT OUTER JOIN Colour_Head k ON b.Colour_IdNo = k.Colour_IdNo LEFT OUTER JOIN Ledger_Head e ON a.Transport_IdNo = e.Ledger_IdNo LEFT OUTER JOIN Textile_Processing_Receipt_Details h ON h.Cloth_Processing_Receipt_Code = '" & Trim(NewCode) & "' and b.Cloth_Processing_Delivery_Code = h.Cloth_Processing_Delivery_Code and b.Cloth_Processing_Delivery_SlNo = h.Cloth_Processing_Delivery_SlNo   Where   " & CompIDCondt & IIf(Trim(CompIDCondt) <> "", " and ", "") & " a.ledger_Idno = " & Str(Val(LedIdNo)) & " and ((b.Delivery_Meters - b.Receipt_Meters - b.Return_Meters) > 0 or h.Receipt_Meters > 0 ) Order by a.ClothProcess_Delivery_Date, a.for_orderby, a.ClothProcess_Delivery_No", con)
            End If


            Dt1 = New DataTable
            nr = Da.Fill(Dt1)

            If Dt1.Rows.Count > 0 Then

                For i = 0 To Dt1.Rows.Count - 1

                    n = .Rows.Add()

                    Ent_Qty = 0
                    Ent_Wgt = 0
                    Ent_Pcs = 0
                    Ent_Mtrs = 0
                    Ent_BitsMtrs = 0

                    If IsDBNull(Dt1.Rows(i).Item("Ent_Pcs").ToString) = False Then
                        Ent_Pcs = Val(Dt1.Rows(i).Item("Ent_Pcs").ToString)
                    End If
                    If IsDBNull(Dt1.Rows(i).Item("Ent_Qty").ToString) = False Then
                        Ent_Qty = Val(Dt1.Rows(i).Item("Ent_Qty").ToString)
                    End If
                    If IsDBNull(Dt1.Rows(i).Item("Ent_Mtrs").ToString) = False Then
                        Ent_Mtrs = Val(Dt1.Rows(i).Item("Ent_Mtrs").ToString)
                    End If
                    If IsDBNull(Dt1.Rows(i).Item("Ent_Wgt").ToString) = False Then
                        Ent_Wgt = Val(Dt1.Rows(i).Item("Ent_Wgt").ToString)
                    End If
                    If IsDBNull(Dt1.Rows(i).Item("Ent_BitsMeters").ToString) = False Then
                        Ent_BitsMtrs = Val(Dt1.Rows(i).Item("Ent_BitsMeters").ToString)
                    End If


                    SNo = SNo + 1

                    .Rows(n).Cells(0).Value = Val(SNo)
                    .Rows(n).Cells(1).Value = Dt1.Rows(i).Item("Cloth_Processing_Delivery_No").ToString
                    .Rows(n).Cells(2).Value = Format(Convert.ToDateTime(Dt1.Rows(i).Item("Cloth_Processing_Delivery_Date").ToString), "dd-MM-yyyy")

                    .Rows(n).Cells(4).Value = Dt1.Rows(i).Item("Colour_Name").ToString
                    .Rows(n).Cells(5).Value = Dt1.Rows(i).Item("Process_Name").ToString

                    Dim Process_Inputs_Tmp As String = ""
                    Dim Process_Outputs_Tmp As String = ""

                    If Not IsDBNull(Dt1.Rows(0).Item("Cloth_Delivered")) Then
                        Process_Inputs_Tmp = IIf(Dt1.Rows(0).Item("Cloth_Delivered") = True, "1", "0")
                    Else
                        Process_Inputs_Tmp = "0"
                    End If

                    If Not IsDBNull(Dt1.Rows(0).Item("FP_Delivered")) Then
                        Process_Inputs_Tmp = Process_Inputs_Tmp + IIf(Dt1.Rows(0).Item("FP_Delivered") = True, "1", "0")
                    Else
                        Process_Inputs_Tmp = Process_Inputs_Tmp + "0"
                    End If


                    If Not IsDBNull(Dt1.Rows(0).Item("Cloth_Returned")) Then
                        Process_Outputs_Tmp = IIf(Dt1.Rows(0).Item("Cloth_Returned") = True, "1", "0")
                    Else
                        Process_Outputs_Tmp = "0"
                    End If

                    If Not IsDBNull(Dt1.Rows(0).Item("FP_Returned")) Then
                        Process_Outputs_Tmp = Process_Outputs_Tmp + IIf(Dt1.Rows(0).Item("FP_Returned") = True, "1", "0")
                    Else
                        Process_Outputs_Tmp = Process_Outputs_Tmp + "0"
                    End If

                    Dim RET_TYPE As String = "CLOTH"

                    If Len(Trim(Process_Outputs_Tmp)) > 1 Then
                        If Mid(Trim(Process_Outputs_Tmp), 2, 1) = "1" Then
                            RET_TYPE = "FP"
                        End If
                    End If

                    If RET_TYPE = "CLOTH" Then
                        .Rows(n).Cells(3).Value = Dt1.Rows(i).Item("Fp_Item_Name").ToString
                    Else
                        .Rows(n).Cells(3).Value = Dt1.Rows(i).Item("Del_Item_Name").ToString
                    End If

                    .Rows(n).Cells(6).Value = Dt1.Rows(i).Item("Lot_No").ToString
                    .Rows(n).Cells(7).Value = Format(Val(Dt1.Rows(i).Item("Delivery_Pcs").ToString) - Val(Dt1.Rows(i).Item("Receipt_Pcs").ToString) - Val(Dt1.Rows(i).Item("Return_pcs").ToString) + Val(Ent_Pcs), "#########0.00")
                    .Rows(n).Cells(8).Value = Format(Val(Dt1.Rows(i).Item("Delivery_Qty").ToString) - Val(Dt1.Rows(i).Item("Receipt_Qty").ToString) - Val(Dt1.Rows(i).Item("Return_Qty").ToString) + Val(Ent_Qty), "#########0.00")
                    .Rows(n).Cells(9).Value = Format(Val(Dt1.Rows(i).Item("Delivery_Meters").ToString) - Val(Dt1.Rows(i).Item("Receipt_Meters").ToString) - Val(Dt1.Rows(i).Item("Return_Meters").ToString) + Val(Ent_Mtrs), "#########0.00")
                    .Rows(n).Cells(10).Value = Format(Val(Dt1.Rows(i).Item("Delivery_Weight").ToString) - Val(Dt1.Rows(i).Item("Receipt_Weight").ToString) - Val(Dt1.Rows(i).Item("Return_Weight").ToString) + Val(Ent_Wgt), "#########0.000")

                    If Ent_Mtrs > 0 Then

                        .Rows(n).Cells(11).Value = "1"

                        For j = 0 To .ColumnCount - 1
                            .Rows(n).Cells(j).Style.ForeColor = Color.Red
                        Next

                    Else

                        .Rows(n).Cells(11).Value = ""

                    End If

                    .Rows(n).Cells(13).Value = Dt1.Rows(i).Item("Purchase_OrderNo").ToString
                    .Rows(n).Cells(14).Value = Dt1.Rows(i).Item("Cloth_Processing_Delivery_code").ToString
                    .Rows(n).Cells(15).Value = Dt1.Rows(i).Item("Cloth_Processing_Delivery_Slno").ToString

                    .Rows(n).Cells(16).Value = Ent_Pcs
                    .Rows(n).Cells(17).Value = Ent_Qty
                    .Rows(n).Cells(18).Value = Ent_Mtrs
                    .Rows(n).Cells(19).Value = Ent_Wgt
                    .Rows(n).Cells(20).Value = Val(Dt1.Rows(i).Item("Item_Idno").ToString)     '--CELL(20)

                    If Not IsDBNull(Dt1.Rows(i).Item("Fabric_Folding")) Then
                        .Rows(n).Cells(21).Value = Val(Dt1.Rows(i).Item("Fabric_Folding")).ToString
                    Else
                        .Rows(n).Cells(21).Value = "100"
                    End If
                    .Rows(n).Cells(22).Value = Ent_BitsMtrs

                Next

            End If
            Dt1.Clear()

        End With

        pnl_Selection.Visible = True
        pnl_Back.Enabled = False
        pnl_Back.Visible = False
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
        Cloth_Invoice_Selection()
    End Sub

    Private Sub Cloth_Invoice_Selection()
        Dim Da1 As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim n As Integer = 0
        Dim sno As Integer = 0
        Dim i As Integer = 0
        Dim j As Integer = 0

        dgv_Fabric_Details.Rows.Clear()

        For i = 0 To dgv_Selection.RowCount - 1

            If Val(dgv_Selection.Rows(i).Cells(11).Value) = 1 Then

                If txt_JobNo.Text = "" Then
                    If (dgv_Selection.Rows(i).Cells(13).Value) <> "" Then
                        txt_JobNo.Text = Trim(dgv_Selection.Rows(i).Cells(13).Value)
                    End If
                End If

                n = dgv_Fabric_Details.Rows.Add()
                sno = sno + 1
                dgv_Fabric_Details.Rows(n).Cells(dgvCol_Details.SLNO).Value = Val(sno)
                dgv_Fabric_Details.Rows(n).Cells(dgvCol_Details.DC_NO).Value = dgv_Selection.Rows(i).Cells(1).Value
                dgv_Fabric_Details.Rows(n).Cells(dgvCol_Details.ITEM_FP).Value = dgv_Selection.Rows(i).Cells(3).Value
                dgv_Fabric_Details.Rows(n).Cells(dgvCol_Details.COLOUR).Value = dgv_Selection.Rows(i).Cells(4).Value
                dgv_Fabric_Details.Rows(n).Cells(dgvCol_Details.PROCESSING).Value = dgv_Selection.Rows(i).Cells(5).Value
                dgv_Fabric_Details.Rows(n).Cells(dgvCol_Details.LOT_NO).Value = dgv_Selection.Rows(i).Cells(6).Value
                dgv_Fabric_Details.Rows(n).Cells(dgvCol_Details.FOLDING).Value = Val(dgv_Selection.Rows(i).Cells(21).Value)
                dgv_Fabric_Details.Rows(n).Cells(dgvCol_Details.Process_Delv_Code).Value = dgv_Selection.Rows(i).Cells(14).Value
                dgv_Fabric_Details.Rows(n).Cells(dgvCol_Details.Process_Delv_Slno).Value = dgv_Selection.Rows(i).Cells(15).Value

                'dgv_Details.Rows(n).Cells(13).Value = dgv_Selection.Rows(i).Cells(20).Value

                If Val(dgv_Selection.Rows(i).Cells(16).Value) <> 0 Then
                    dgv_Fabric_Details.Rows(n).Cells(dgvCol_Details.PCS).Value = dgv_Selection.Rows(i).Cells(16).Value
                Else
                    dgv_Fabric_Details.Rows(n).Cells(dgvCol_Details.PCS).Value = dgv_Selection.Rows(i).Cells(7).Value
                End If

                If Val(dgv_Selection.Rows(i).Cells(17).Value) <> 0 Then
                    dgv_Fabric_Details.Rows(n).Cells(dgvCol_Details.QTY).Value = dgv_Selection.Rows(i).Cells(17).Value
                Else
                    dgv_Fabric_Details.Rows(n).Cells(dgvCol_Details.QTY).Value = dgv_Selection.Rows(i).Cells(8).Value
                End If

                If Val(dgv_Selection.Rows(i).Cells(18).Value) <> 0 Then
                    dgv_Fabric_Details.Rows(n).Cells(dgvCol_Details.METERS).Value = dgv_Selection.Rows(i).Cells(18).Value
                Else
                    dgv_Fabric_Details.Rows(n).Cells(dgvCol_Details.METERS).Value = dgv_Selection.Rows(i).Cells(9).Value
                End If

                Dim vMTRS100FLDG As String = 0
                Dim vFLDPERC As String = 0

                vFLDPERC = Val(dgv_Fabric_Details.Rows(n).Cells(dgvCol_Details.FOLDING).Value)
                If Val(vFLDPERC) = 0 Then vFLDPERC = 100
                vMTRS100FLDG = Val(dgv_Fabric_Details.Rows(n).Cells(dgvCol_Details.METERS).Value) * Val(vFLDPERC) / 100
                dgv_Fabric_Details.Rows(n).Cells(dgvCol_Details.METERS_100PERC_FOLDING).Value = Format(Val(vMTRS100FLDG), "#########0.00")

                If Val(dgv_Selection.Rows(i).Cells(19).Value) <> 0 Then
                    dgv_Fabric_Details.Rows(n).Cells(dgvCol_Details.WEIGHT).Value = dgv_Selection.Rows(i).Cells(19).Value
                Else
                    dgv_Fabric_Details.Rows(n).Cells(dgvCol_Details.WEIGHT).Value = dgv_Selection.Rows(i).Cells(10).Value
                End If

                dgv_Fabric_Details.Rows(n).Cells(dgvCol_Details.BITSMETERS).Value = dgv_Selection.Rows(i).Cells(22).Value

                If Val(dgv_Selection.Rows(i).Cells(20).Value) <> 0 Then
                    dgv_Fabric_Details.Rows(n).Cells(dgvCol_Details.ITEM_GREY).Value = dgv_Selection.Rows(i).Cells(20).Value   '--CELL(16)
                Else
                    dgv_Fabric_Details.Rows(n).Cells(dgvCol_Details.ITEM_GREY).Value = dgv_Selection.Rows(i).Cells(20).Value
                End If



            End If

        Next

        Total_Calculation()

        pnl_Back.Enabled = True
        pnl_Back.Visible = True
        pnl_Selection.Visible = False
        If txt_PartyDcNo.Enabled And txt_PartyDcNo.Visible Then txt_PartyDcNo.Focus()

    End Sub
    Private Sub Show_Item_CurrentStock(ByVal Rw As Integer)
        Dim vItemID As Integer

        If Val(Rw) < 0 Then Exit Sub

        With dgv_Fabric_Details

            vItemID = Common_Procedures.Processed_Item_NameToIdNo(con, .Rows(Rw).Cells(dgvCol_Details.ITEM_FP).Value)

            If Val(vItemID) = 0 Then Exit Sub

            If Val(vItemID) <> Val(.Tag) Then
                Common_Procedures.Show_ProcessedItem_CurrentStock_Display(con, Val(lbl_Company.Tag), Val(Common_Procedures.CommonLedger.Godown_Ac), vItemID)
                .Tag = Val(Rw)
            End If

        End With

    End Sub

    Private Sub chk_LotComplete_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles chk_LotComplete.KeyDown

        If e.KeyCode = 40 Then

            e.Handled = True
            e.SuppressKeyPress = True

            If MessageBox.Show("Do you want to save ?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = DialogResult.Yes Then
                save_record()
            Else
                dtp_Date.Focus()
            End If
        End If

        If e.KeyValue = 38 Then
            e.Handled = True
            e.SuppressKeyPress = True
            dgv_FP_Details.Focus()
            dgv_FP_Details.CurrentCell = dgv_FP_Details.Rows(0).Cells(1)
            dgv_FP_Details.CurrentCell.Selected = True
            'SendKeys.Send("+{TAB}")
        End If

    End Sub

    Private Sub chk_LotComplete_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles chk_LotComplete.KeyPress
        If Asc(e.KeyChar) = 13 Then
            'dgv_Details.Focus()
            'dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(dgvCol_Details.DC_NO)
            'dgv_Details.CurrentCell.Selected = True

            e.Handled = True
            If MessageBox.Show("Do you want to save ?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = DialogResult.Yes Then
                save_record()
            Else
                dtp_Date.Focus()
            End If
        End If
    End Sub

    Private Sub cbo_DeliveryTo_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_DeliveryTo.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", " (Ledger_Type = 'GODOWN' or  Ledger_Type ='')", "(Ledger_idno = 0)")
        ' AND (AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14) )
        cbo_DeliveryTo.Tag = cbo_DeliveryTo.Text
    End Sub

    Private Sub cbo_DeliveryTo_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_DeliveryTo.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_DeliveryTo, cbo_Ledger, IIf(cbo_ProcessingName.Enabled, cbo_ProcessingName, cbo_TransportName), "Ledger_AlaisHead", "Ledger_DisplayName", " (Ledger_Type = 'GODOWN' )", "(Ledger_idno = 0)")
        'or ( Ledger_Type = '' AND (AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14) )
    End Sub

    Private Sub cbo_DeliveryTo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_DeliveryTo.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_DeliveryTo, IIf(cbo_ProcessingName.Enabled, cbo_ProcessingName, cbo_TransportName), "Ledger_AlaisHead", "Ledger_DisplayName", " (Ledger_Type = 'GODOWN' ) ", "(Ledger_idno = 0)")
        'or ( Ledger_Type = '' AND (AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14) )
    End Sub
    Private Sub cbo_ProcessingName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_ProcessingName.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Process_Head", "Process_Name", "", "(Process_Idno=0)")
    End Sub

    Private Sub cbo_ProcessingName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_ProcessingName.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_ProcessingName, cbo_DeliveryTo, cbo_TransportName, "Process_Head", "Process_Name", "", "(Process_Idno=0)")
    End Sub

    Private Sub cbo_ProcessingName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_ProcessingName.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_ProcessingName, cbo_TransportName, "Process_Head", "Process_Name", "", "(Process_Idno=0)")
    End Sub

    Private Sub cbo_ProcessingName_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_ProcessingName.KeyUp
        If e.Control = False And e.KeyValue = 17 Then
            Dim f As New Process_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_ProcessingName.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()
        End If
    End Sub

    Private Sub cbo_DeliveryTo_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_DeliveryTo.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Common_Procedures.MDI_LedType = "GODOWN"
            Dim f As New Ledger_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Ledger.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()
        End If
    End Sub

    Private Sub txt_ReceiptMeter_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_ReceiptMeter.TextChanged
        Total_Calculation()
    End Sub

    Private Sub btn_SendSMS_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_SendSMS.Click
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
            smstxt = smstxt & " Rec.No : " & Trim(lbl_RecNo.Text) & Chr(13)
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
                'smstxt = smstxt & " No.Of Bales : " & Val((dgv_Details_Total.Rows(0).Cells(dgvCol_Details.PCS ).Value())) & Chr(13)
                'BlNos = ""
                'For i = 0 To dgv_Details.Rows.Count - 1
                '    If Val(dgv_Details_Total.Rows(0).Cells(dgvCol_Details.WEIGHT).Value()) <> 0 Then
                '        BlNos = BlNos & IIf(Trim(BlNos) <> "", ", ", "") & Trim(dgv_Details.Rows(0).Cells(dgvCol_Details.QTY ).Value)
                '    End If
                'Next
                ' smstxt = smstxt & " Bales No.s : " & Trim(BlNos) & Chr(13)
                smstxt = smstxt & " Pcs : " & Val(dgv_Details_Total.Rows(0).Cells(dgvCol_Details.PCS).Value()) & Chr(13)
                smstxt = smstxt & " Meters : " & Val(dgv_Details_Total.Rows(0).Cells(dgvCol_Details.METERS).Value()) & Chr(13)
            End If
            'If dgv_Details.RowCount > 0 Then
            '    smstxt = smstxt & " No.Of Bales : " & Val((dgv_Details.Rows(0).Cells(dgvCol_Details.PROCESSING ).Value())) & Chr(13)
            '    smstxt = smstxt & " Meters : " & Val((dgv_Details.Rows(0).Cells(dgvCol_Details.QTY ).Value())) & Chr(13)
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
            MessageBox.Show(ex.Message, "DOES NOT SEND SMS...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub
    Private Sub cbo_Vechile_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_VehicleNo.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Textile_Processing_Receipt_Head", "Vehicle_No", "", "")
    End Sub

    Private Sub cbo_VehicleNo_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_VehicleNo.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_VehicleNo, cbo_TransportName, txt_Frieght, "Textile_Processing_Receipt_Head", "Vehicle_No", "", "")

    End Sub

    Private Sub cbo_VehicleNo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_VehicleNo.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_VehicleNo, txt_Frieght, "Textile_Processing_Receipt_Head", "Vehicle_No", "", "", False)

    End Sub

    Private Sub cbo_itemfp_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbo_grid_Cloth_Fabric.SelectedIndexChanged

    End Sub

    Private Sub cbo_itemfp_Enter(sender As Object, e As EventArgs) Handles cbo_grid_Cloth_Fabric.Enter

    End Sub

    Private Sub dgv_Details_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgv_Fabric_Details.CellContentClick

    End Sub

    Private Sub txt_Frieght_TextChanged(sender As Object, e As EventArgs) Handles txt_Frieght.TextChanged

    End Sub

    Private Sub chk_LotComplete_CheckedChanged(sender As Object, e As EventArgs) Handles chk_LotComplete.CheckedChanged

    End Sub

    Private Sub cbo_Ledger_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbo_Ledger.SelectedIndexChanged

    End Sub

    Private Sub cbo_Receipt_Type_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbo_Receipt_Type.SelectedIndexChanged

        'If Not Displaying Then
        '    If cbo_Receipt_Type.Tag <> cbo_Receipt_Type.Text Then
        '        If MessageBox.Show("Changing Receipt Type Will Remove All Rows in Details. Continue ?", "Receipt Type Change", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.No Then
        '            dgv_Details.Rows.Clear()
        '            cbo_Receipt_Type.Tag = cbo_Receipt_Type.Text
        '            Exit Sub
        '        End If
        '    End If
        'End If


        'If cbo_Receipt_Type.Text = "DELIVERY" Then
        '    btn_Selection.Enabled = True
        '    dgv_Details.Columns(0).ReadOnly = True
        '    dgv_Details.Columns(1).ReadOnly = True
        '    dgv_Details.Columns(2).ReadOnly = True
        '    dgv_Details.Columns(4).ReadOnly = True
        '    dgv_Details.Columns(5).ReadOnly = True
        '    dgv_Details.AllowUserToAddRows = False
        'Else
        '    btn_Selection.Enabled = False
        '    dgv_Details.Columns(0).ReadOnly = False
        '    dgv_Details.Columns(1).ReadOnly = False
        '    dgv_Details.Columns(2).ReadOnly = False
        '    dgv_Details.Columns(4).ReadOnly = False
        '    dgv_Details.Columns(5).ReadOnly = False
        '    dgv_Details.AllowUserToAddRows = True
        'End If

        'cbo_Receipt_Type.Tag = cbo_Receipt_Type.Text

    End Sub

    Private Sub cbo_Receipt_Type_Leave(sender As Object, e As EventArgs) Handles cbo_Receipt_Type.Leave

        If Not Displaying Then
            If cbo_Receipt_Type.Tag <> cbo_Receipt_Type.Text And Len(Trim(cbo_Receipt_Type.Tag)) <> 0 Then
                If MessageBox.Show("Changing Receipt Type Will Remove All Rows in Details. Continue ?", "Receipt Type Change", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = DialogResult.No Then
                    cbo_Receipt_Type.Text = cbo_Receipt_Type.Tag
                    Exit Sub
                Else
                    dgv_Fabric_Details.Rows.Clear()
                    'Exit Sub
                End If
            End If
        End If

        If cbo_Receipt_Type.Text = "DELIVERY" Then
            On Error Resume Next
            btn_Selection.Enabled = True
            'dgv_Details.Columns(0).ReadOnly = True
            dgv_Fabric_Details.Columns(1).ReadOnly = True
            dgv_Fabric_Details.Columns(2).ReadOnly = True
            dgv_Fabric_Details.Columns(3).ReadOnly = True
            dgv_Fabric_Details.Columns(4).ReadOnly = True
            dgv_Fabric_Details.Columns(5).ReadOnly = True
            dgv_Fabric_Details.AllowUserToAddRows = False
        Else
            On Error Resume Next
            btn_Selection.Enabled = False
            'dgv_Details.Columns(0).ReadOnly = False
            dgv_Fabric_Details.Columns(1).ReadOnly = False
            dgv_Fabric_Details.Columns(2).ReadOnly = False
            dgv_Fabric_Details.Columns(3).ReadOnly = False
            dgv_Fabric_Details.Columns(4).ReadOnly = False
            dgv_Fabric_Details.Columns(5).ReadOnly = False
            dgv_Fabric_Details.AllowUserToAddRows = True
        End If

        cbo_Receipt_Type.Tag = cbo_Receipt_Type.Text

    End Sub

    Private Sub cbo_Receipt_Type_Enter(sender As Object, e As EventArgs) Handles cbo_Receipt_Type.Enter

        cbo_Receipt_Type.Tag = cbo_Receipt_Type.Text

    End Sub

    Private Sub cbo_TransportName_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbo_TransportName.SelectedIndexChanged

    End Sub

    Private Sub cbo_LotNo_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbo_grid_LotNo_Fabric.SelectedIndexChanged

    End Sub

    Private Sub dgv_Selection_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgv_Selection.CellContentClick

    End Sub

    Private Sub cbo_DeliveryTo_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbo_DeliveryTo.SelectedIndexChanged

        If cbo_DeliveryTo.Tag <> cbo_DeliveryTo.Text Then
            Dim DEL_LED_TYPE As String = Common_Procedures.get_FieldValue(con, "Ledger_Head", "Ledger_Type", "Ledger_IdNo = " & Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_DeliveryTo.Text))
            If DEL_LED_TYPE = "GODOWN" Then
                cbo_ProcessingName.Text = ""
                cbo_ProcessingName.Enabled = False
            Else
                cbo_ProcessingName.Enabled = True
            End If
        End If

    End Sub

    Private Sub cbo_DeliveryTo_Leave(sender As Object, e As EventArgs) Handles cbo_DeliveryTo.Leave

        If cbo_DeliveryTo.Tag <> cbo_DeliveryTo.Text Then
            Dim DEL_LED_TYPE As String = Common_Procedures.get_FieldValue(con, "Ledger_Head", "Ledger_Type", "Ledger_IdNo = " & Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_DeliveryTo.Text))
            If DEL_LED_TYPE = "GODOWN" Then
                cbo_ProcessingName.Text = ""
                cbo_ProcessingName.Enabled = False
            Else
                cbo_ProcessingName.Enabled = True
            End If
        End If

    End Sub

    Private Sub cbo_Receipt_Type_KeyDown(sender As Object, e As KeyEventArgs) Handles cbo_Receipt_Type.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Receipt_Type, dtp_Date, cbo_Ledger, "", "", "", "")
    End Sub

    Private Sub cbo_Receipt_Type_KeyPress(sender As Object, e As KeyPressEventArgs) Handles cbo_Receipt_Type.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Receipt_Type, cbo_Ledger, "", "", "", "")
    End Sub

    Private Sub dgtxt_details_KeyUp(sender As Object, e As KeyEventArgs) Handles dgtxt_details.KeyUp
        dgv_Fabric_Details.CurrentCell.Value = dgtxt_details.Text
    End Sub



    Private Sub txt_ReceiptMeter_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txt_ReceiptMeter.KeyPress

        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
            e.Handled = True
        End If

    End Sub


    Private Sub txt_DiffMeter_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txt_DiffMeter.KeyPress

        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
            e.Handled = True
        End If

    End Sub



    Private Sub cbo_FP_Enter(sender As Object, e As EventArgs) Handles cbo_grid_FP.Enter
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Processed_Item_Head", "Processed_Item_Name", "(Processed_Item_Type = 'FP')", "(Processed_Item_Idno = 0)")
    End Sub

    Private Sub cbo_FP_KeyDown(sender As Object, e As KeyEventArgs) Handles cbo_grid_FP.KeyDown

        vcbo_KeyDwnVal = e.KeyValue

        Dim RET_TYPE As String = "CLOTH"

        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_grid_FP, Nothing, cbo_grid_Colour_Fabric, "Processed_Item_Head", "Processed_Item_Name", "(Processed_Item_Type = 'FP')", "(Processed_Item_Idno = 0)")

        With dgv_FP_Details

            If (e.KeyValue = 38 And cbo_grid_FP.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
                If .CurrentCell.RowIndex <= 0 Then
                    If dgv_Fabric_Details.Rows.Count > 0 Then
                        dgv_Fabric_Details.Focus()
                        dgv_Fabric_Details.CurrentCell = dgv_Fabric_Details.Rows(0).Cells(1)
                    Else
                        txt_Frieght.Focus()
                    End If

                Else
                    .Focus()
                    .CurrentCell = .Rows(.CurrentRow.Index).Cells(dgvCol_Details_FP.Reject_Mtrs)

                End If

            End If

            If (e.KeyValue = 40 And cbo_grid_FP.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
                .Focus()
                .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 1)
            End If

        End With

    End Sub

    Private Sub cbo_FP_KeyPress(sender As Object, e As KeyPressEventArgs) Handles cbo_grid_FP.KeyPress
        Dim Itfp_ID As Integer = 0
        Dim vMTRSQTY As String = ""

        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_grid_FP, cbo_grid_Colour_Fabric, "Processed_Item_Head", "Processed_Item_Name", "(Processed_Item_Type = 'FP')", "(Processed_Item_Idno = 0)")

        If Asc(e.KeyChar) = 13 Then

            With dgv_FP_Details

                If Trim(UCase(vcbotxt_FPGRID)) <> Trim(UCase(cbo_grid_FP.Text)) Then

                    Itfp_ID = Common_Procedures.Processed_Item_NameToIdNo(con, .Rows(.CurrentCell.RowIndex).Cells(dgvCol_Details_FP.ITEM_FP).Value)
                    vMTRSQTY = Common_Procedures.get_FieldValue(con, "Processed_Item_Head", "Meter_Qty", "(Processed_Item_IdNo = " & Str(Val(Itfp_ID)) & ")")
                    .Rows(.CurrentCell.RowIndex).Cells(dgvCol_Details_FP.Mtrs_per_Qty).Value = Format(Val(vMTRSQTY), "##########0.00")

                End If

                If Trim(cbo_grid_FP.Text) = "" And .CurrentCell.RowIndex = .Rows.Count - 1 Then
                    dgv_Details_CB.Focus()
                    dgv_Details_CB.CurrentCell = dgv_Details_CB.Rows(0).Cells(1)
                Else
                    .Focus()
                    .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)
                End If

            End With

        End If

    End Sub

    Private Sub cbo_grid_FP_KeyUp(sender As Object, e As KeyEventArgs) Handles cbo_grid_FP.KeyUp

        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then

            Dim f As New FinishedProduct_Creation_Simple

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_grid_FP.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If

    End Sub

    'Private Sub cbo_Colour_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Colour.GotFocus
    '    Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "colour_Head", "colour_Name", "", "(Colour_IdNo = 0)")

    'End Sub
    'Private Sub cbo_Colour_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Colour.KeyDown

    '    vcbo_KeyDwnVal = e.KeyValue

    '    Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Colour, cbo_Cloth, cbo_LotNo, "colour_Head", "colour_Name", "", "(Colour_IdNo = 0)")

    '    With dgv_Details

    '        If (e.KeyValue = 38 And cbo_Colour.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
    '            .Focus()
    '            .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex - 1)
    '        End If

    '        If (e.KeyValue = 40 And cbo_Colour.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
    '            .Focus()
    '            .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 1)
    '        End If

    '    End With

    'End Sub

    'Private Sub cbo_Colour_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Colour.KeyPress

    '    Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Colour, cbo_LotNo, "colour_Head", "colour_Name", "", "(Colour_IdNo = 0)")

    '    If Asc(e.KeyChar) = 13 Then

    '        With dgv_Details

    '            .Focus()
    '            .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)

    '        End With

    '    End If

    'End Sub


    'Private Sub cbo_Colour_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Colour.KeyUp

    '    If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
    '        Dim f As New Color_Creation

    '        Common_Procedures.Master_Return.Form_Name = Me.Name
    '        Common_Procedures.Master_Return.Control_Name = cbo_Colour.Name
    '        Common_Procedures.Master_Return.Return_Value = ""
    '        Common_Procedures.Master_Return.Master_Type = ""

    '        f.MdiParent = MDIParent1
    '        f.Show()

    '    End If

    'End Sub


    'Private Sub cbo_Colour_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Colour.TextChanged

    '    Try

    '        If cbo_Colour.Visible Then
    '            With dgv_Details
    '                If Val(cbo_Colour.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = 3 Then
    '                    .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_Colour.Text)
    '                End If
    '            End With
    '        End If

    '    Catch ex As Exception

    '        'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

    '    End Try

    'End Sub


    Private Sub cbo_Colour_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_grid_Colour_Fabric.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "colour_Head", "colour_Name", "", "(Colour_IdNo = 0)")

    End Sub
    Private Sub cbo_Colour_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_grid_Colour_Fabric.KeyDown

        vcbo_KeyDwnVal = e.KeyValue

        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_grid_Colour_Fabric, cbo_grid_Cloth_Fabric, cbo_grid_Processing_Fabric, "colour_Head", "colour_Name", "", "(Colour_IdNo = 0)")

        With dgv_Fabric_Details

            If (e.KeyValue = 38 And cbo_grid_Colour_Fabric.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
                .Focus()
                .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex - 1)
            End If

            If (e.KeyValue = 40 And cbo_grid_Colour_Fabric.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
                .Focus()
                .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 1)
            End If

        End With

    End Sub

    Private Sub cbo_Colour_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_grid_Colour_Fabric.KeyPress

        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_grid_Colour_Fabric, cbo_grid_Processing_Fabric, "colour_Head", "colour_Name", "", "(Colour_IdNo = 0)")

        If Asc(e.KeyChar) = 13 Then

            With dgv_Fabric_Details

                .Focus()
                .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)

            End With

        End If

    End Sub


    Private Sub cbo_Colour_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_grid_Colour_Fabric.KeyUp

        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Dim f As New Color_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_grid_Colour_Fabric.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If

    End Sub


    Private Sub cbo_Colour_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_grid_Colour_Fabric.TextChanged

        Try

            If cbo_grid_Colour_Fabric.Visible Then
                With dgv_Fabric_Details
                    If Val(cbo_grid_Colour_Fabric.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = 3 Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_grid_Colour_Fabric.Text)
                    End If
                End With
            End If

        Catch ex As Exception

            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub cbo_Colour_CB_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Colour_CB.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "colour_Head", "colour_Name", "", "(Colour_IdNo = 0)")

    End Sub
    Private Sub cbo_Colour_CB_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Colour_CB.KeyDown

        vcbo_KeyDwnVal = e.KeyValue

        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Colour_CB, cbo_Cloth_CB, cbo_LotNo_CB, "colour_Head", "colour_Name", "", "(Colour_IdNo = 0)")

        With dgv_Details_CB

            If (e.KeyValue = 38 And cbo_Colour_CB.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
                .Focus()
                .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex - 1)
            End If

            If (e.KeyValue = 40 And cbo_Colour_CB.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
                .Focus()
                .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 1)
            End If

        End With

    End Sub

    Private Sub cbo_Colour_CB_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Colour_CB.KeyPress

        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Colour_CB, cbo_LotNo_CB, "Colour_Head", "Colour_Name", "", "(Colour_IdNo = 0)")

        If Asc(e.KeyChar) = 13 Then

            With dgv_Details_CB

                'If Trim(cbo_Colour_CB.Text) = "" And .CurrentCell.RowIndex = .Rows.Count - 1 Then
                '    chk_LotComplete.Focus()
                'Else
                .Focus()
                .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)
                'End If

            End With

        End If

    End Sub


    Private Sub cbo_Colour_CB_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Colour_CB.KeyUp

        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Dim f As New Color_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Colour_CB.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If

    End Sub


    Private Sub cbo_Colour_CB_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Colour_CB.TextChanged

        Try

            If cbo_Colour_CB.Visible Then
                With dgv_Details_CB
                    If Val(cbo_Colour_CB.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = 2 Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_Colour_CB.Text)
                    End If
                End With
            End If

        Catch ex As Exception

            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub cbo_FP_TextChanged(sender As Object, e As EventArgs) Handles cbo_grid_FP.TextChanged

        Try
            If cbo_grid_FP.Visible Then
                With dgv_FP_Details
                    If Val(cbo_grid_FP.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = 1 Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_grid_FP.Text)
                    End If
                End With
            End If

        Catch ex As Exception

            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub cbo_Cloth_CB_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Cloth_CB.GotFocus

        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Cloth_Head", "Cloth_Name", "", "(Cloth_Idno = 0)")

    End Sub

    Private Sub cbo_Cloth_CB_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Cloth_CB.KeyDown

        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Cloth_CB, Nothing, cbo_grid_Colour_Fabric, "Cloth_Head", "Cloth_Name", "", "(Cloth_Idno = 0)")

        With dgv_Details_CB

            If (e.KeyValue = 38 And cbo_Cloth_CB.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
                .Focus()
                .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex - 1)
            End If

            If (e.KeyValue = 40 And cbo_Cloth_CB.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
                .Focus()
                .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 1)
            End If

        End With

    End Sub

    Private Sub cbo_Cloth_CB_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Cloth_CB.KeyPress

        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Cloth_CB, cbo_grid_Colour_Fabric, "Cloth_Head", "Cloth_Name", "", "(Cloth_Idno = 0)")

        If Asc(e.KeyChar) = 13 Then

            With dgv_Details_CB

                If Trim(cbo_Cloth_CB.Text) = "" And .CurrentCell.RowIndex = .Rows.Count - 1 Then
                    chk_LotComplete.Focus()
                Else
                    .Focus()
                    .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)
                End If


            End With

        End If

    End Sub

    Private Sub cbo_Cloth_CB_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Cloth_CB.KeyUp

        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Dim f As New Cloth_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Cloth_CB.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If

    End Sub

    Private Sub cbo_Cloth_CB_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Cloth_CB.TextChanged

        Try
            If cbo_Cloth_CB.Visible Then
                With dgv_Details_CB
                    If Val(cbo_Cloth_CB.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = 1 Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_Cloth_CB.Text)
                    End If
                End With
            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub dgv_Details_FP_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgv_FP_Details.CellContentClick

    End Sub

    Private Sub dgv_Details_FP_CellEnter(sender As Object, e As DataGridViewCellEventArgs) Handles dgv_FP_Details.CellEnter

        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim Dt2 As New DataTable
        Dim Dt3 As New DataTable
        Dim Dt4 As New DataTable
        Dim rect As Rectangle

        With dgv_FP_Details

            dgv_FP_Details.BringToFront()
            dgv_Details_FP_Total.BringToFront()

            If Val(.CurrentRow.Cells(dgvCol_Details_FP.SLNO).Value) = 0 Then
                .CurrentRow.Cells(dgvCol_Details_FP.SLNO).Value = .CurrentRow.Index + 1
            End If

            If e.ColumnIndex = dgvCol_Details_FP.ITEM_FP Then

                If (cbo_grid_FP.Visible = False Or Val(cbo_grid_FP.Tag) <> e.RowIndex) Then

                    cbo_grid_FP.Tag = -1
                    Da = New SqlClient.SqlDataAdapter("select Processed_Item_Name from Processed_Item_Head where Processed_Item_Type = 'FP' order by Processed_Item_Name", con)
                    Dt1 = New DataTable
                    Da.Fill(Dt1)
                    cbo_grid_FP.DataSource = Dt1
                    cbo_grid_FP.DisplayMember = "Processed_Item_Name"

                    rect = .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False)

                    cbo_grid_FP.Left = .Left + rect.Left  '  .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Left
                    cbo_grid_FP.Top = .Top + rect.Top  ' .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Top
                    cbo_grid_FP.Width = rect.Width  ' .CurrentCell.Size.Width
                    cbo_grid_FP.Height = rect.Height  ' rect.Height

                    cbo_grid_FP.Text = .CurrentCell.Value  '  Trim(.CurrentRow.Cells(e.ColumnIndex).Value)

                    cbo_grid_FP.Tag = Val(e.RowIndex)
                    cbo_grid_FP.Visible = True

                    cbo_grid_FP.BringToFront()
                    cbo_grid_FP.Focus()



                End If

            Else

                cbo_grid_FP.Visible = False

            End If

            If e.ColumnIndex = dgvCol_Details_FP.COLOUR Then

                If (cbo_grid_Colour_FP.Visible = False Or Val(cbo_grid_Colour_FP.Tag) <> e.RowIndex) Then

                    cbo_grid_Colour_FP.Tag = -1
                    Da = New SqlClient.SqlDataAdapter("select Colour_Name from Colour_Head order by Colour_Name", con)
                    Dt2 = New DataTable
                    Da.Fill(Dt2)
                    cbo_grid_Colour_FP.DataSource = Dt2
                    cbo_grid_Colour_FP.DisplayMember = "Colour_Name"

                    rect = .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False)

                    cbo_grid_Colour_FP.Left = .Left + rect.Left
                    cbo_grid_Colour_FP.Top = .Top + rect.Top
                    cbo_grid_Colour_FP.Width = rect.Width
                    cbo_grid_Colour_FP.Height = rect.Height

                    cbo_grid_Colour_FP.Text = .CurrentCell.Value

                    cbo_grid_Colour_FP.Tag = Val(e.RowIndex)
                    cbo_grid_Colour_FP.Visible = True

                    cbo_grid_Colour_FP.BringToFront()
                    cbo_grid_Colour_FP.Focus()



                End If

            Else

                cbo_grid_Colour_FP.Visible = False


            End If



            If e.ColumnIndex = dgvCol_Details_FP.LOT_NO Then

                If (cbo_grid_LotNo_FP.Visible = False Or Val(cbo_grid_LotNo_FP.Tag) <> e.RowIndex) Then

                    cbo_grid_LotNo_FP.Tag = -1
                    Da = New SqlClient.SqlDataAdapter("select Lot_No from Lot_Head order by Lot_No", con)
                    Dt4 = New DataTable
                    Da.Fill(Dt4)
                    cbo_grid_LotNo_FP.DataSource = Dt4
                    cbo_grid_LotNo_FP.DisplayMember = "Lot_No"

                    rect = .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False)

                    cbo_grid_LotNo_FP.Left = .Left + rect.Left  '  .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Left
                    cbo_grid_LotNo_FP.Top = .Top + rect.Top  ' .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Top
                    cbo_grid_LotNo_FP.Width = rect.Width  ' .CurrentCell.Size.Width
                    cbo_grid_LotNo_FP.Height = rect.Height  ' rect.Height

                    cbo_grid_LotNo_FP.Text = .CurrentCell.Value  '  Trim(.CurrentRow.Cells(e.ColumnIndex).Value)

                    cbo_grid_LotNo_FP.Tag = Val(e.RowIndex)
                    cbo_grid_LotNo_FP.Visible = True

                    cbo_grid_LotNo_FP.BringToFront()
                    cbo_grid_LotNo_FP.Focus()

                    'cbo_Grid_CountName.Visible = False
                    'cbo_Grid_MillName.Visible = False

                End If

            Else

                cbo_grid_LotNo_FP.Visible = False
                'cbo_Grid_MillName.Tag = -1
                'cbo_Grid_MillName.Text = ""

            End If

            'If e.ColumnIndex = 8 And dgv_LevColNo <> 8 Then
            '    Show_Item_CurrentStock(e.RowIndex)
            '    .Focus()
            'End If

            If e.ColumnIndex = dgvCol_Details_FP.QUANTITY Then
                If Val(.CurrentCell.Value) = 0 Then

                    If Val(.Rows(e.RowIndex).Cells(dgvCol_Details_FP.Mtrs_per_Qty).Value) > 0 Then

                        Dim vQTY As Long = 0
                        Dim vFABMTRS As String = 0
                        Dim vBALFABMTRS As String = 0

                        vFABMTRS = 0
                        For I = 0 To dgv_Fabric_Details.Rows.Count
                            If I <> e.RowIndex Then
                                If Val(.Rows(e.RowIndex).Cells(dgvCol_Details_FP.QUANTITY).Value) > 0 And Val(.Rows(e.RowIndex).Cells(dgvCol_Details_FP.Fabric_Consumption).Value) > 0 Then
                                    vFABMTRS = Format(Val(vFABMTRS) + Val(.Rows(e.RowIndex).Cells(dgvCol_Details_FP.Fabric_Consumption).Value), "#########0.00")
                                End If
                            End If
                        Next

                        vBALFABMTRS = Format(Val(lbl_Total_100PercFolding_FabricMeters.Text) - Val(lbl_Total_BitsMeters.Text) - Val(vFABMTRS), "#########0.00")

                        vQTY = Math.Floor(vBALFABMTRS / Val(.Rows(e.RowIndex).Cells(dgvCol_Details_FP.Mtrs_per_Qty).Value))

                        .Rows(e.RowIndex).Cells(dgvCol_Details_FP.QUANTITY).Value = vQTY

                    End If

                End If
            End If


        End With

    End Sub

    Private Sub dgv_Details_CB_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgv_Details_CB.CellContentClick

    End Sub

    Private Sub dgv_Details_CB_CellEnter(sender As Object, e As DataGridViewCellEventArgs) Handles dgv_Details_CB.CellEnter

        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim Dt2 As New DataTable
        Dim Dt3 As New DataTable
        Dim Dt4 As New DataTable
        Dim rect As Rectangle

        With dgv_Details_CB

            dgv_Details_CB.BringToFront()
            dgv_Details_CB_Total.BringToFront()

            If Val(.CurrentRow.Cells(dgvCol_Details_CB.SLNO).Value) = 0 Then
                .CurrentRow.Cells(dgvCol_Details_CB.SLNO).Value = .CurrentRow.Index + 1
            End If

            If e.ColumnIndex = 1 Then

                If (cbo_Cloth_CB.Visible = False Or Val(cbo_Cloth_CB.Tag) <> e.RowIndex) Then

                    cbo_Cloth_CB.Tag = -1
                    Da = New SqlClient.SqlDataAdapter("select Cloth_Name from Cloth_Head order by Cloth_Name", con)
                    Dt1 = New DataTable
                    Da.Fill(Dt1)
                    cbo_Cloth_CB.DataSource = Dt1
                    cbo_Cloth_CB.DisplayMember = "Cloth_Name"

                    rect = .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False)

                    cbo_Cloth_CB.Left = .Left + rect.Left  '  .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Left
                    cbo_Cloth_CB.Top = .Top + rect.Top  ' .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Top
                    cbo_Cloth_CB.Width = rect.Width  ' .CurrentCell.Size.Width
                    cbo_Cloth_CB.Height = rect.Height  ' rect.Height

                    cbo_Cloth_CB.Text = .CurrentCell.Value  '  Trim(.CurrentRow.Cells(e.ColumnIndex).Value)

                    cbo_Cloth_CB.Tag = Val(e.RowIndex)
                    cbo_Cloth_CB.Visible = True

                    cbo_Cloth_CB.BringToFront()
                    cbo_Cloth_CB.Focus()

                End If

            Else

                cbo_Cloth_CB.Visible = False

            End If

            If e.ColumnIndex = 2 Then

                If (cbo_Colour_CB.Visible = False Or Val(cbo_Colour_CB.Tag) <> e.RowIndex) Then

                    cbo_Colour_CB.Tag = -1
                    Da = New SqlClient.SqlDataAdapter("select Colour_Name from Colour_Head order by Colour_Name", con)
                    Dt2 = New DataTable
                    Da.Fill(Dt2)
                    cbo_Colour_CB.DataSource = Dt2
                    cbo_Colour_CB.DisplayMember = "Colour_Name"

                    rect = .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False)

                    cbo_Colour_CB.Left = .Left + rect.Left
                    cbo_Colour_CB.Top = .Top + rect.Top
                    cbo_Colour_CB.Width = rect.Width
                    cbo_Colour_CB.Height = rect.Height

                    cbo_Colour_CB.Text = .CurrentCell.Value

                    cbo_Colour_CB.Tag = Val(e.RowIndex)
                    cbo_Colour_CB.Visible = True

                    cbo_Colour_CB.BringToFront()
                    cbo_Colour_CB.Focus()

                End If

            Else

                cbo_Colour_CB.Visible = False

            End If



            If e.ColumnIndex = 3 Then

                If (cbo_LotNo_CB.Visible = False Or Val(cbo_LotNo_CB.Tag) <> e.RowIndex) And Not dgv_FP_Details.Columns(3).ReadOnly Then

                    cbo_LotNo_CB.Tag = -1
                    Da = New SqlClient.SqlDataAdapter("select Lot_No from Lot_Head order by Lot_No", con)
                    Dt4 = New DataTable
                    Da.Fill(Dt4)
                    cbo_LotNo_CB.DataSource = Dt4
                    cbo_LotNo_CB.DisplayMember = "Lot_No"

                    rect = .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False)

                    cbo_LotNo_CB.Left = .Left + rect.Left  '  .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Left
                    cbo_LotNo_CB.Top = .Top + rect.Top  ' .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Top
                    cbo_LotNo_CB.Width = rect.Width  ' .CurrentCell.Size.Width
                    cbo_LotNo_CB.Height = rect.Height  ' rect.Height

                    cbo_LotNo_CB.Text = .CurrentCell.Value  '  Trim(.CurrentRow.Cells(e.ColumnIndex).Value)

                    cbo_LotNo_CB.Tag = Val(e.RowIndex)
                    cbo_LotNo_CB.Visible = True

                    cbo_LotNo_CB.BringToFront()
                    cbo_LotNo_CB.Focus()

                    'cbo_Grid_CountName.Visible = False
                    'cbo_Grid_MillName.Visible = False

                End If

            Else

                cbo_LotNo_CB.Visible = False
                'cbo_Grid_MillName.Tag = -1
                'cbo_Grid_MillName.Text = ""

            End If

            If e.ColumnIndex = 8 And dgv_LevColNo <> 8 Then
                Show_Item_CurrentStock(e.RowIndex)
                .Focus()
            End If

        End With

    End Sub

    Private Sub dgv_Details_FP_GotFocus(sender As Object, e As EventArgs) Handles dgv_FP_Details.GotFocus
        Act_Ctrl = dgv_FP_Details.Name.ToString
        dgv_FP_Details.Focus()
    End Sub

    Private Sub dgv_Details_CB_GotFocus(sender As Object, e As EventArgs) Handles dgv_Details_CB.GotFocus
        Act_Ctrl = dgv_Details_CB.Name.ToString
        dgv_Details_CB.Focus()
    End Sub

    Private Sub dgv_Details_FP_EditingControlShowing(sender As Object, e As DataGridViewEditingControlShowingEventArgs) Handles dgv_FP_Details.EditingControlShowing
        dgtxt_details_fp = CType(dgv_FP_Details.EditingControl, DataGridViewTextBoxEditingControl)
    End Sub

    Private Sub dgv_Details_CB_EditingControlShowing(sender As Object, e As DataGridViewEditingControlShowingEventArgs) Handles dgv_Details_CB.EditingControlShowing
        dgtxt_details_cb = CType(dgv_Details_CB.EditingControl, DataGridViewTextBoxEditingControl)
    End Sub

    Private Sub dgtxt_details_cb_Enter(sender As Object, e As EventArgs) Handles dgtxt_details_cb.Enter
        dgv_Details_CB.EditingControl.BackColor = Color.Lime
        Act_Ctrl = dgv_Details_CB.Name
    End Sub

    Private Sub dgtxt_details_fp_Enter(sender As Object, e As EventArgs) Handles dgtxt_details_fp.Enter
        dgv_FP_Details.EditingControl.BackColor = Color.Lime
        Act_Ctrl = dgv_FP_Details.Name
    End Sub

    Private Sub cbo_Process_Completed_KeyPress(sender As Object, e As KeyPressEventArgs) Handles cbo_Process_Completed.KeyPress

        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Process_Completed, txt_PartyDcNo, "Process_Head", "Process_Name", "", "(Ledger_idno = 0)")

        If Asc(e.KeyChar) = 13 Then
            If btn_Selection.Enabled = False Then
                txt_PartyDcNo.Focus()
                Exit Sub
            End If
            If MessageBox.Show("Do you want to select Fabric Delivery:", "FOR FABRIC DELIVERY SELECTION...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = DialogResult.Yes Then
                btn_Selection_Click(sender, e)
            Else
                txt_PartyDcNo.Focus()
            End If
        End If
    End Sub

    Private Sub cbo_Process_Completed_Enter(sender As Object, e As EventArgs) Handles cbo_Process_Completed.Enter
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Process_Head", "Process_Name", "", "(Ledger_idno = 0)")
    End Sub

    Private Sub cbo_Process_Completed_KeyDown(sender As Object, e As KeyEventArgs) Handles cbo_Process_Completed.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Process_Completed, cbo_Ledger, txt_PartyDcNo, "Process_Head", "Process_Name", "", "(Ledger_idno = 0)")
    End Sub

    Private Sub cbo_Process_Completed_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbo_Process_Completed.SelectedIndexChanged

    End Sub

    Private Sub cbo_FP_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbo_grid_FP.SelectedIndexChanged

    End Sub

    Private Sub dgv_Details_LostFocus(sender As Object, e As EventArgs) Handles dgv_Fabric_Details.LostFocus
        Act_Ctrl = ""
        On Error Resume Next
        If IsNothing(dgv_Fabric_Details.CurrentCell) Then Exit Sub
        dgv_Fabric_Details.CurrentCell.Selected = False
    End Sub

    Private Sub dgv_Details_FP_LostFocus(sender As Object, e As EventArgs) Handles dgv_FP_Details.LostFocus
        Act_Ctrl = ""
        On Error Resume Next
        If IsNothing(dgv_FP_Details.CurrentCell) Then Exit Sub
        dgv_FP_Details.CurrentCell.Selected = False
    End Sub

    Private Sub dgv_Details_CB_LostFocus(sender As Object, e As EventArgs) Handles dgv_Details_CB.LostFocus
        Act_Ctrl = ""
        On Error Resume Next
        If IsNothing(dgv_Details_CB.CurrentCell) Then Exit Sub
        dgv_Details_CB.CurrentCell.Selected = False
    End Sub

    Private Sub cbo_Cloth_CB_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbo_Cloth_CB.SelectedIndexChanged

    End Sub

    Private Sub cbo_Colour_CB_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbo_Colour_CB.SelectedIndexChanged

    End Sub

    Private Sub cbo_LotNo_CB_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbo_LotNo_CB.SelectedIndexChanged

    End Sub

    Private Sub cbo_Process_Completed_LostFocus(sender As Object, e As EventArgs) Handles cbo_Process_Completed.LostFocus

        Dim Process_Inputs_Tmp As String = ""
        Dim Process_Outputs_Tmp As String = ""

        If cbo_Process_Completed.Tag <> cbo_Process_Completed.Text Then

            If Len(Trim(cbo_Process_Completed.Text)) = 0 Then

                Process_Inputs_Tmp = ""
                Process_Outputs_Tmp = ""

            Else

                Dim da As New SqlClient.SqlDataAdapter("select * from process_head where process_name = '" & cbo_Process_Completed.Text & "'", con)
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

        End If

        If Process_Outputs_Tmp + Process_Inputs_Tmp <> Process_Outputs + Process_Inputs Then
            If Not Displaying Then
                If MessageBox.Show("Changing the Process Will Clear All Finished Product Values in Details. Continue ?", "CHANGE PROCESS...", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.No Then
                    cbo_Process_Completed.Text = cbo_Process_Completed.Tag
                    cbo_Process_Completed.Focus()
                Else
                    Process_Inputs = Process_Inputs_Tmp
                    Process_Outputs = Process_Outputs_Tmp
                    For I = 0 To dgv_Fabric_Details.Rows.Count - 1
                        dgv_Fabric_Details.Rows(I).Cells(dgvCol_Details.ITEM_FP).Value = ""
                    Next
                End If
            End If
        End If

    End Sub

    Private Sub cbo_Colour_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbo_grid_Colour_Fabric.SelectedIndexChanged

    End Sub

    Private Sub dgv_Details_TextChanged(sender As Object, e As EventArgs) Handles dgv_Fabric_Details.TextChanged

    End Sub

    Private Sub dgv_Details_CellMouseDoubleClick(sender As Object, e As DataGridViewCellMouseEventArgs) Handles dgv_Fabric_Details.CellMouseDoubleClick

    End Sub

    Private Sub dgv_Details_FP_CellValueChanged(sender As Object, e As DataGridViewCellEventArgs) Handles dgv_FP_Details.CellValueChanged

        On Error Resume Next

        If IsNothing(dgv_FP_Details.CurrentCell) Then Exit Sub

        With dgv_FP_Details
            If .Visible Then
                If .CurrentCell.ColumnIndex = dgvCol_Details_FP.QUANTITY Or .CurrentCell.ColumnIndex = dgvCol_Details_FP.Reject_Mtrs Or .CurrentCell.ColumnIndex = dgvCol_Details_FP.Mtrs_per_Qty Then

                    If .CurrentCell.ColumnIndex = dgvCol_Details_FP.QUANTITY Or .CurrentCell.ColumnIndex = dgvCol_Details_FP.Reject_Mtrs Then
                        dgv_FP_Details.Rows(.CurrentCell.RowIndex).Cells(dgvCol_Details_FP.Passed_Mtrs).Value = Val(dgv_FP_Details.Rows(.CurrentCell.RowIndex).Cells(dgvCol_Details_FP.QUANTITY).Value) - Val(dgv_FP_Details.Rows(.CurrentCell.RowIndex).Cells(dgvCol_Details_FP.Reject_Mtrs).Value)
                    End If
                    If .CurrentCell.ColumnIndex = dgvCol_Details_FP.QUANTITY Or .CurrentCell.ColumnIndex = dgvCol_Details_FP.Mtrs_per_Qty Then
                        dgv_FP_Details.Rows(.CurrentCell.RowIndex).Cells(dgvCol_Details_FP.Fabric_Consumption).Value = Format(Val(dgv_FP_Details.Rows(.CurrentCell.RowIndex).Cells(dgvCol_Details_FP.QUANTITY).Value) * Val(dgv_FP_Details.Rows(.CurrentCell.RowIndex).Cells(dgvCol_Details_FP.Mtrs_per_Qty).Value), "#########0.00")
                    End If

                    Total_Calculation()

                End If
            End If
        End With

    End Sub

    Private Sub dgv_Details_FP_KeyUp(sender As Object, e As KeyEventArgs) Handles dgv_FP_Details.KeyUp

        On Error Resume Next

        If IsNothing(dgv_FP_Details.CurrentCell) Then Exit Sub

        With dgv_FP_Details

            Dim n As Integer

            If e.Control = True And UCase(Chr(e.KeyCode)) = "D" Then

                With dgv_FP_Details

                    n = .CurrentRow.Index

                    'If Trim(.Rows(n).Cells(dgvCol_Details.Processed_Fabric_Inspection_Code).Value) = "" And Val(.Rows(n).Cells(dgvCol_Details.Processed_Fabric_Inspection_Code).Value) = 0 Then
                    If .Rows.Count = 1 Then
                        For i = 0 To .Columns.Count - 1
                            .Rows(n).Cells(i).Value = ""
                        Next

                    Else
                        .Rows.RemoveAt(n)

                    End If
                    'End If

                    For i = 0 To .Rows.Count - 1
                        .Rows(i).Cells(dgvCol_Details.SLNO).Value = i + 1
                    Next

                End With

                Total_Calculation()

            End If

            If .Visible Then
                If .CurrentCell.ColumnIndex = dgvCol_Details_FP.QUANTITY Then
                    Total_Calculation()
                End If
            End If

        End With

    End Sub

    Private Sub dgv_Details_FP_CellEndEdit(sender As Object, e As DataGridViewCellEventArgs) Handles dgv_FP_Details.CellEndEdit
        On Error Resume Next
        If IsNothing(dgv_FP_Details.CurrentCell) Then Exit Sub

        With dgv_FP_Details
            If .Visible Then
                If .CurrentCell.ColumnIndex = dgvCol_Details_FP.QUANTITY Then
                    Total_Calculation()
                End If
            End If
        End With
    End Sub

    Private Sub dgv_Details_CB_CellValueChanged(sender As Object, e As DataGridViewCellEventArgs) Handles dgv_Details_CB.CellValueChanged

        On Error Resume Next
        If IsNothing(dgv_Details_CB.CurrentCell) Then Exit Sub

        With dgv_Details_CB
            If .Visible Then
                If .CurrentCell.ColumnIndex = dgvCol_Details_CB.METERS Then
                    Total_Calculation()
                End If
            End If
        End With

    End Sub

    Private Sub dgv_Details_CB_CellEndEdit(sender As Object, e As DataGridViewCellEventArgs) Handles dgv_Details_CB.CellEndEdit

        On Error Resume Next
        If IsNothing(dgv_Details_CB.CurrentCell) Then Exit Sub

        With dgv_Details_CB
            If .Visible Then
                If .CurrentCell.ColumnIndex = dgvCol_Details_CB.METERS Then
                    Total_Calculation()
                End If
            End If
        End With

    End Sub

    Private Sub dgv_Details_CB_KeyUp(sender As Object, e As KeyEventArgs) Handles dgv_Details_CB.KeyUp

        On Error Resume Next
        If IsNothing(dgv_Details_CB.CurrentCell) Then Exit Sub

        With dgv_Details_CB


            If IsNothing(dgv_Details_CB.CurrentCell) Then Exit Sub

            Dim n As Integer

            If e.Control = True And UCase(Chr(e.KeyCode)) = "D" Then

                'With dgv_Details_CB

                n = .CurrentRow.Index

                'If Trim(.Rows(n).Cells(dgvCol_Details.Processed_Fabric_Inspection_Code).Value) = "" And Val(.Rows(n).Cells(dgvCol_Details.Processed_Fabric_Inspection_Code).Value) = 0 Then
                If .Rows.Count = 1 Then
                    For i = 0 To .Columns.Count - 1
                        .Rows(n).Cells(i).Value = ""
                    Next

                Else
                    .Rows.RemoveAt(n)

                End If
                'End If

                For i = 0 To .Rows.Count - 1
                    .Rows(i).Cells(dgvCol_Details.SLNO).Value = i + 1
                Next

                'End With

                Total_Calculation()

            End If

            If .Visible Then
                If .CurrentCell.ColumnIndex = dgvCol_Details_CB.METERS Then
                    Total_Calculation()
                End If
            End If


            If .Visible Then
                If .CurrentCell.ColumnIndex = dgvCol_Details_CB.METERS Then
                    Total_Calculation()
                End If
            End If
        End With

    End Sub

    Private Sub dgv_Details_CB_Enter(sender As Object, e As EventArgs) Handles dgv_Details_CB.Enter
        dgv_Details_CB.BringToFront()
        dgv_Details_CB_Total.BringToFront()
    End Sub

    Private Sub dgv_Details_FP_Enter(sender As Object, e As EventArgs) Handles dgv_FP_Details.Enter
        dgv_FP_Details.BringToFront()
        dgv_Details_FP_Total.BringToFront()
    End Sub



    Private Sub dgtxt_details_fp_TextChanged(sender As Object, e As EventArgs) Handles dgtxt_details_fp.TextChanged
        Try
            With dgv_FP_Details
                If .Rows.Count <> 0 Then
                    .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(dgtxt_details_fp.Text)
                End If
            End With

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub cbo_grid_FP_GotFocus(sender As Object, e As EventArgs) Handles cbo_grid_FP.GotFocus
        vcbotxt_FPGRID = cbo_grid_FP.Text
    End Sub

    Private Sub txt_JobNo_TextChanged(sender As Object, e As EventArgs) Handles txt_JobNo.TextChanged

    End Sub

    Private Sub txt_Frieght_KeyDown(sender As Object, e As KeyEventArgs) Handles txt_Frieght.KeyDown
        If e.KeyValue = 38 Then
            e.Handled = True
            e.SuppressKeyPress = True
            SendKeys.Send("+{TAB}")
        End If
        If e.KeyValue = 40 Then
            e.Handled = True
            e.SuppressKeyPress = True

            If dgv_Fabric_Details.Rows.Count > 0 Then
                dgv_Fabric_Details.Focus()
                dgv_Fabric_Details.CurrentCell = dgv_Fabric_Details.Rows(0).Cells(1)
                dgv_Fabric_Details.CurrentCell.Selected = True
            Else
                dgv_FP_Details.Focus()
                dgv_FP_Details.CurrentCell = dgv_FP_Details.Rows(0).Cells(1)
                dgv_FP_Details.CurrentCell.Selected = True
            End If

        End If
    End Sub
End Class