Public Class Processing_Receipt_Textile
    Implements Interface_MDIActions

    Private con As New SqlClient.SqlConnection(Common_Procedures.Connection_String)
    Private con1 As New SqlClient.SqlConnection(Common_Procedures.ConnectionString_CompanyGroupdetails)
    Private FrmLdSTS As Boolean = False
    Private New_Entry As Boolean = False
    Private Insert_Entry As Boolean = False
    Private Filter_Status As Boolean = False
    Private Pk_Condition As String = "FPRRC-"
    Private Pk_Condition2 As String = "FPRDC-"
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
    Private dgv_LevColNo As Integer
    Private Displaying As Boolean = False

    Private vCLO_CONDT As String = ""
    Private FnYearCode1 As String = ""
    Private FnYearCode2 As String = ""
    Private Enum dgvCol_Details As Integer
        SLNO                                    '0
        DC_NO                                   '1
        ITEM_RECEIVED                           '2
        COLOUR                                  '3
        PROCESSING                              '4
        LOT_NO                                  '5
        FOLDING                                 '6
        PCS                                     '7
        QTY                                     '8
        Delivery_Meter                          '9
        METERS                                  '10
        WEIGHT                                  '11
        EXC_SHT_Mtr                             '12
        EXC_SHT_Wgt                             '13
        EXC_SHT_Percentage                      '14
        Process_Delv_Code                       '15
        Process_Delv_Slno                       '16
        Cloth_Processing_Receipt_Slno           '17
        rct_Code                                '18
        Processed_Fabric_Inspection_Code        '19
        ITEM_DELIVERED                          '20
        ITEM_after_NextProcess                  '21
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
        dgv_Details.Rows.Clear()
        dgv_Details_Total.Rows.Clear()

        txt_DiffMeter.Text = ""
        txt_ReceiptMeter.Text = ""

        Grid_DeSelect()

        cbo_GRID_item_received.Visible = False
        cbo_GRID_Colour.Visible = False
        cbo_GRID_LotNo.Visible = False
        cbo_GRID_Processing.Visible = False

        cbo_GRID_item_received.Tag = -1
        cbo_GRID_item_after_NextProcess.Tag = -1
        cbo_GRID_Colour.Tag = -1
        cbo_GRID_LotNo.Tag = -1
        cbo_GRID_Processing.Tag = -1

        cbo_Ledger.Enabled = True
        cbo_Ledger.BackColor = Color.White

        cbo_DeliveryTo.Enabled = True
        cbo_DeliveryTo.BackColor = Color.White

        'txt_JobNo.Enabled = True


        cbo_GRID_Colour.Enabled = True
        cbo_GRID_Colour.BackColor = Color.White

        cbo_GRID_item_received.Enabled = True
        cbo_GRID_item_received.BackColor = Color.White

        cbo_GRID_LotNo.Enabled = True
        cbo_GRID_LotNo.BackColor = Color.White

        chk_LotComplete.Checked = False

        cbo_GRID_Processing.Enabled = True
        cbo_GRID_Processing.BackColor = Color.White

        cbo_GRID_item_received.Text = ""
        cbo_GRID_Colour.Text = ""
        cbo_GRID_LotNo.Text = ""
        cbo_GRID_Processing.Text = ""

        cbo_Receipt_Type.Text = "DELIVERY"
        cbo_Receipt_Type.Tag = cbo_Receipt_Type.Text
        txt_JobNo.BackColor = Color.White

        If cbo_Receipt_Type.Text = "DELIVERY" Then
            txt_JobNo.Enabled = False
        Else
            txt_JobNo.Enabled = True
        End If

        dgv_Details.AllowUserToAddRows = False
        dgv_Details.Tag = ""
        dgv_LevColNo = -1

        cbo_ClothSales_OrderCode_forSelection.Text = ""

    End Sub

    Private Sub Grid_DeSelect()

        On Error Resume Next

        If Not IsNothing(dgv_Details.CurrentCell) Then dgv_Details.CurrentCell.Selected = False
        If Not IsNothing(dgv_Details_Total.CurrentCell) Then dgv_Details_Total.CurrentCell.Selected = False
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

        If Me.ActiveControl.Name <> cbo_GRID_Colour.Name Then
            cbo_GRID_Colour.Visible = False
        End If
        If Me.ActiveControl.Name <> cbo_GRID_item_received.Name Then
            cbo_GRID_item_received.Visible = False
        End If
        If Me.ActiveControl.Name <> cbo_GRID_Processing.Name Then
            cbo_GRID_Processing.Visible = False
        End If
        If Me.ActiveControl.Name <> cbo_GRID_LotNo.Name Then
            cbo_GRID_LotNo.Visible = False
        End If
        If Me.ActiveControl.Name <> cbo_GRID_item_after_NextProcess.Name Then
            cbo_GRID_item_after_NextProcess.Visible = False
        End If
        If Me.ActiveControl.Name <> dgv_Details_Total.Name Then
            Grid_DeSelect()
        End If

        If Me.ActiveControl.Name <> dgv_Details.Name Then
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

        Displaying = True
        clear()
        Displaying = False

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(no) & "/" & Trim(Common_Procedures.FnYearCode)

        Try

            Displaying = True

            da1 = New SqlClient.SqlDataAdapter("select a.*, b.Ledger_Name , c.Ledger_Name as Transport_Name,  d.Ledger_Name as DeliveryToName from Textile_Processing_Receipt_Head a INNER JOIN Ledger_Head b ON a.Ledger_IdNo = b.Ledger_IdNo LEFT OUTER JOIN Ledger_Head c ON a.Transport_IdNo = c.Ledger_IdNo LEFT OUTER JOIN Ledger_Head d ON a.DeliveryTo_IdNo = d.Ledger_IdNo Where a.ClothProcess_Receipt_Code = '" & Trim(NewCode) & "' AND ClothProcess_Receipt_Code not like '%FPFRC%' AND ClothProcess_Receipt_Code not like '%FPFR1%'", con)
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
                cbo_ProcessingName_of_NextProcess.Text = Common_Procedures.Process_IdNoToName(con, Val(dt1.Rows(0).Item("Processing_Idno").ToString))

                If Val(dt1.Rows(0).Item("Lot_Status").ToString) = 1 Then
                    chk_LotComplete.Checked = True
                Else
                    chk_LotComplete.Checked = False
                End If

                cbo_ClothSales_OrderCode_forSelection.Text = dt1.Rows(0).Item("ClothSales_OrderCode_forSelection").ToString

                cbo_VehicleNo.Text = dt1.Rows(0).Item("Vehicle_No").ToString

                If Not IsDBNull(dt1.Rows(0).Item("Receipt_Type")) Then
                    cbo_Receipt_Type.Text = dt1.Rows(0).Item("Receipt_Type").ToString
                End If
                cbo_Receipt_Type.Tag = cbo_Receipt_Type.Text

                If IsDBNull(dt1.Rows(0).Item("Weaver_Piece_Checking_Code").ToString) = False Then
                    If Trim(dt1.Rows(0).Item("Weaver_Piece_Checking_Code").ToString) <> "" Then
                        LockSTS = True
                    End If
                End If

                If Trim(UCase(cbo_Receipt_Type.Text)) = "DELIVERY" Then
                    btn_Selection.Enabled = True
                    'dgv_Details.Columns(0).ReadOnly = True
                    dgv_Details.Columns(1).ReadOnly = True
                    dgv_Details.Columns(2).ReadOnly = True
                    dgv_Details.Columns(dgvCol_Details.PROCESSING).ReadOnly = True
                    dgv_Details.Columns(dgvCol_Details.LOT_NO).ReadOnly = True
                    dgv_Details.AllowUserToAddRows = False
                    txt_JobNo.Enabled = False
                Else

                        btn_Selection.Enabled = False
                    'dgv_Details.Columns(0).ReadOnly = False
                    dgv_Details.Columns(1).ReadOnly = False
                    dgv_Details.Columns(2).ReadOnly = False
                    dgv_Details.Columns(dgvCol_Details.PROCESSING).ReadOnly = False
                    dgv_Details.Columns(dgvCol_Details.LOT_NO).ReadOnly = False
                    dgv_Details.AllowUserToAddRows = True
                    txt_JobNo.Enabled = True

                End If

                lbl_UserName.Text = "USER : " & Trim(UCase(Common_Procedures.User_IdNoToName(con1, Val(dt1.Rows(0).Item("User_IdNo").ToString))))

                da2 = New SqlClient.SqlDataAdapter("select a.Cloth_Processing_Delivery_Slno as cpd_sno,a.*,C.Cloth_Name as Fp_Item_Name,d.Colour_Name,e.Lot_No as Lot_No,f.Process_Name,G.Cloth_Name as Item_Name_Delivered , H.Cloth_Name as ItemName_after_NextProcess from Textile_Processing_Receipt_Details a  INNER JOIN Cloth_Head C ON c.Cloth_Idno = a.Item_To_Idno LEFT OUTER JOIN Cloth_Head G ON G.Cloth_Idno = a.Item_Idno  LEFT OUTER JOIN Colour_Head d ON d.Colour_IdNo = a.Colour_IdNo LEFT OUTER JOIN Lot_Head e ON a.Lot_IdNo = e.Lot_IdNo LEFT OUTER JOIN Process_Head f ON f.Process_IdNo = a.Processing_Idno   LEFT OUTER JOIN Cloth_Head H ON H.Cloth_Idno = a.ItemIdno_after_NextProcess Where a.Cloth_Processing_Receipt_Code = '" & Trim(NewCode) & "' Order by Sl_No", con)
                dt2 = New DataTable
                da2.Fill(dt2)

                dgv_Details.Rows.Clear()
                SNo = 0

                If dt2.Rows.Count > 0 Then

                    For i = 0 To dt2.Rows.Count - 1

                        n = dgv_Details.Rows.Add()

                        SNo = SNo + 1
                        dgv_Details.Rows(n).Cells(dgvCol_Details.SLNO).Value = Val(SNo)
                        dgv_Details.Rows(n).Cells(dgvCol_Details.DC_NO).Value = dt2.Rows(i).Item("Dc_Rc_No").ToString
                        dgv_Details.Rows(n).Cells(dgvCol_Details.ITEM_RECEIVED).Value = dt2.Rows(i).Item("Fp_Item_Name").ToString
                        dgv_Details.Rows(n).Cells(dgvCol_Details.COLOUR).Value = dt2.Rows(i).Item("Colour_Name").ToString
                        dgv_Details.Rows(n).Cells(dgvCol_Details.PROCESSING).Value = dt2.Rows(i).Item("Process_Name").ToString
                        dgv_Details.Rows(n).Cells(dgvCol_Details.LOT_NO).Value = dt2.Rows(i).Item("Lot_No").ToString

                        If Not IsDBNull(dt2.Rows(i).Item("Folding")) Then
                            dgv_Details.Rows(n).Cells(dgvCol_Details.FOLDING).Value = Val(dt2.Rows(i).Item("Folding").ToString)
                        End If

                        dgv_Details.Rows(n).Cells(dgvCol_Details.PCS).Value = Val(dt2.Rows(i).Item("Receipt_Pcs").ToString)
                        dgv_Details.Rows(n).Cells(dgvCol_Details.QTY).Value = Val(dt2.Rows(i).Item("Receipt_Qty").ToString)
                        dgv_Details.Rows(n).Cells(dgvCol_Details.METERS).Value = Format(Val(dt2.Rows(i).Item("Receipt_Meters").ToString), "########0.00")
                        dgv_Details.Rows(n).Cells(dgvCol_Details.WEIGHT).Value = Format(Val(dt2.Rows(i).Item("Receipt_Weight").ToString), "########0.000")
                        dgv_Details.Rows(n).Cells(dgvCol_Details.EXC_SHT_Mtr).Value = Format(Val(dt2.Rows(i).Item("ExcSht_Meters").ToString), "########0.00")
                        dgv_Details.Rows(n).Cells(dgvCol_Details.EXC_SHT_Wgt).Value = Format(Val(dt2.Rows(i).Item("ExcSht_Weight").ToString), "########0.000")

                        dgv_Details.Rows(n).Cells(dgvCol_Details.Process_Delv_Code).Value = dt2.Rows(i).Item("Cloth_Processing_Delivery_Code").ToString
                        dgv_Details.Rows(n).Cells(dgvCol_Details.Process_Delv_Slno).Value = dt2.Rows(i).Item("cpd_sno").ToString
                        dgv_Details.Rows(n).Cells(dgvCol_Details.Cloth_Processing_Receipt_Slno).Value = dt2.Rows(i).Item("Cloth_Processing_Receipt_Slno").ToString
                        'dgv_Details.Rows(n).Cells(dgvCol_Details.rct_Code).Value = dt2.Rows(i).Item("Cloth_Processing_BillMaking_Code").ToString
                        dgv_Details.Rows(n).Cells(dgvCol_Details.Processed_Fabric_Inspection_Code).Value = dt2.Rows(i).Item("Processed_Fabric_Inspection_Code").ToString
                        dgv_Details.Rows(n).Cells(dgvCol_Details.ITEM_DELIVERED).Value = dt2.Rows(i).Item("Item_Name_Delivered").ToString

                        dgv_Details.Rows(n).Cells(dgvCol_Details.Delivery_Meter).Value = Format(Val(dt2.Rows(i).Item("Processing_Delivery_Meters").ToString), "########0.00")
                        dgv_Details.Rows(n).Cells(dgvCol_Details.EXC_SHT_Percentage).Value = Format(Val(dt2.Rows(i).Item("Excess_Short_Percentage").ToString), "########0.00")

                        dgv_Details.Rows(n).Cells(dgvCol_Details.ITEM_after_NextProcess).Value = dt2.Rows(i).Item("ItemName_after_NextProcess").ToString


                        If Trim(dgv_Details.Rows(n).Cells(dgvCol_Details.Processed_Fabric_Inspection_Code).Value) <> "" Then
                            For j = 0 To dgv_Details.ColumnCount - 1
                                dgv_Details.Rows(n).Cells(j).Style.BackColor = Color.LightGray
                            Next j
                            LockSTS = True
                        End If

                        'If Val(dgv_Details.Rows(n).Cells(15).Value) <> 0 Then
                        '    For j = 0 To dgv_Details.ColumnCount - 1
                        '        dgv_Details.Rows(n).Cells(j).Style.BackColor = Color.LightGray
                        '    Next j
                        '    LockSTS = True
                        'End If

                    Next i

                End If

                If dgv_Details.Rows.Count = 0 Then
                    dgv_Details.Rows.Add()
                Else

                    n = dgv_Details.Rows.Count - 1
                    If Trim(dgv_Details.Rows(n).Cells(dgvCol_Details.ITEM_RECEIVED).Value) = "" Then
                        dgv_Details.Rows(n).Cells(dgvCol_Details.Cloth_Processing_Receipt_Slno).Value = ""
                        If Val(dgv_Details.Rows(n).Cells(dgvCol_Details.Cloth_Processing_Receipt_Slno).Value) = 0 Then
                            If n = 0 Then
                                dgv_Details.Rows(n).Cells(dgvCol_Details.Cloth_Processing_Receipt_Slno).Value = 1
                            Else
                                dgv_Details.Rows(n).Cells(dgvCol_Details.Cloth_Processing_Receipt_Slno).Value = Val(dgv_Details.Rows(n - 1).Cells(dgvCol_Details.Cloth_Processing_Receipt_Slno).Value) + 1
                            End If
                        End If
                    End If

                End If


                With dgv_Details_Total
                    If .RowCount = 0 Then .Rows.Add()
                    .Rows(0).Cells(dgvCol_Details.PCS).Value = Val(dt1.Rows(0).Item("Total_Pcs").ToString)
                    .Rows(0).Cells(dgvCol_Details.QTY).Value = Val(dt1.Rows(0).Item("Total_Qty").ToString)
                    .Rows(0).Cells(dgvCol_Details.METERS).Value = Format(Val(dt1.Rows(0).Item("Total_Meters").ToString), "########0.00")
                    .Rows(0).Cells(dgvCol_Details.Delivery_Meter).Value = Format(Val(dt1.Rows(0).Item("Total_Processing_Delivery_Meters").ToString), "########0.00")
                    .Rows(0).Cells(dgvCol_Details.WEIGHT).Value = Format(Val(dt1.Rows(0).Item("Total_Weight").ToString), "########0.000")
                    .Rows(0).Cells(dgvCol_Details.EXC_SHT_Mtr).Value = Format(Val(dt1.Rows(0).Item("Total_ExcessShort").ToString), "########0.00")
                    .Rows(0).Cells(dgvCol_Details.EXC_SHT_Wgt).Value = Format(Val(dt1.Rows(0).Item("Total_ExcSht_Weight").ToString), "########0.00")

                End With


                Grid_DeSelect()

                If LockSTS = True Then

                    cbo_Ledger.Enabled = False
                    cbo_Ledger.BackColor = Color.LightGray

                    cbo_DeliveryTo.Enabled = False
                    cbo_DeliveryTo.BackColor = Color.LightGray

                    If Trim(dgv_Details.Rows(n).Cells(15).Value) <> "" Then
                        txt_JobNo.Enabled = False
                        txt_JobNo.BackColor = Color.LightGray
                    End If

                    cbo_GRID_Colour.Enabled = False
                    cbo_GRID_Colour.BackColor = Color.LightGray

                    cbo_GRID_item_received.Enabled = False
                    cbo_GRID_item_received.BackColor = Color.LightGray

                    cbo_GRID_LotNo.Enabled = False
                    cbo_GRID_LotNo.BackColor = Color.LightGray

                    cbo_GRID_Processing.Enabled = False
                    cbo_GRID_Processing.BackColor = Color.LightGray

                    dgv_Details.AllowUserToAddRows = False

                End If

                dt2.Clear()

                dt2.Dispose()
                da2.Dispose()

            End If

            dt1.Clear()
            dt1.Dispose()
            da1.Dispose()

        Catch ex As Exception

            Displaying = False
            MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Finally

            Displaying = False

        End Try

        If dtp_Date.Visible And dtp_Date.Enabled Then dtp_Date.Focus()

    End Sub

    Private Sub Processing_Receipt_Textile_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated
        Try

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Ledger.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "LEDGER" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Ledger.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If
            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_TransportName.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "LEDGER" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_TransportName.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If
            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_GRID_item_received.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "CLOTH" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_GRID_item_received.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If
            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_GRID_item_after_NextProcess.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "CLOTH" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_GRID_item_after_NextProcess.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If
            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_ProcessingName_of_NextProcess.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "PROCESS" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_ProcessingName_of_NextProcess.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If
            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_GRID_Colour.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "COLOUR" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_GRID_Colour.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If
            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_GRID_LotNo.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "LOT" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_GRID_LotNo.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_GRID_Processing.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "PROCESS" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_GRID_Processing.Text = Trim(Common_Procedures.Master_Return.Return_Value)
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
            'MessageBox.Show(ex.Message, "DOES NOT SHOW...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

        FrmLdSTS = False

    End Sub

    Private Sub Processing_Receipt_Textile_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim dt3 As New DataTable
        Dim dt4 As New DataTable
        Dim dt5 As New DataTable
        Dim dt6 As New DataTable
        Dim dt7 As New DataTable

        Me.Text = ""

        If Common_Procedures.settings.CustomerCode = "1516" Or Common_Procedures.settings.CustomerCode = "1490" Then
            lbl_Heading.Text = "PROCESSEDFABRIC DELIVERY/RECEIPT"
        End If

        con.Open()

        da = New SqlClient.SqlDataAdapter("select distinct(Vehicle_No) from Textile_Processing_Receipt_Head order by Vehicle_No", con)
        da.Fill(dt1)
        cbo_VehicleNo.DataSource = dt1
        cbo_VehicleNo.DisplayMember = "Vehicle_No"

        ' cbo_itemfp.Visible = False
        cbo_GRID_item_received.Visible = False
        cbo_GRID_Colour.Visible = False
        cbo_GRID_LotNo.Visible = False
        cbo_GRID_Processing.Visible = False

        pnl_Back.Visible = True
        pnl_Back.BringToFront()

        pnl_Filter.Visible = False
        pnl_Filter.Left = (Me.Width - pnl_Filter.Width) \ 2
        pnl_Filter.Top = (Me.Height - pnl_Filter.Height) \ 2

        pnl_Selection.Visible = False
        pnl_Selection.Left = (Me.Width - pnl_Selection.Width) \ 2
        pnl_Selection.Top = (Me.Height - pnl_Selection.Height) \ 2
        pnl_Selection.BringToFront()

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
        AddHandler cbo_GRID_Colour.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_GRID_item_received.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Ledger.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_GRID_LotNo.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_GRID_Processing.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_ProcessingName_of_NextProcess.GotFocus, AddressOf ControlGotFocus
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
        AddHandler cbo_ClothSales_OrderCode_forSelection.GotFocus, AddressOf ControlGotFocus

        AddHandler dtp_Date.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_GRID_Colour.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_GRID_item_received.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Ledger.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_GRID_LotNo.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_GRID_Processing.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_ProcessingName_of_NextProcess.LostFocus, AddressOf ControlLostFocus
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
        AddHandler cbo_ClothSales_OrderCode_forSelection.LostFocus, AddressOf ControlLostFocus

        AddHandler dtp_Date.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_JobNo.KeyDown, AddressOf TextBoxControlKeyDown
        'AddHandler cbo_DeliveryTo.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Frieght.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_PartyDcNo.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_DiffMeter.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_ReceiptMeter.KeyDown, AddressOf TextBoxControlKeyDown

        AddHandler dtp_Date.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_JobNo.KeyPress, AddressOf TextBoxControlKeyPress
        ' AddHandler cbo_DeliveryTo.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Frieght.KeyPress, AddressOf TextBoxControlKeyPress
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

        dgv_Details.Columns(dgvCol_Details.EXC_SHT_Wgt).Visible = False
        dgv_Details_Total.Columns(dgvCol_Details.EXC_SHT_Wgt).Visible = False

        dgv_Selection.Columns(21).DisplayIndex = 7

        'dgv_Details.Columns(dgvCol_Details.Delivery_Meter).Visible = False
        'dgv_Details.Columns(dgvCol_Details.EXC_SHT_Percentage).Visible = False

        'dgv_Details_Total.Columns(dgvCol_Details.Delivery_Meter).Visible = False
        'dgv_Details_Total.Columns(dgvCol_Details.EXC_SHT_Percentage).Visible = False

        If Common_Procedures.settings.Hide_Qty_QtyMtr_In_Processing_Transactions Then
            dgv_Details.Columns(dgvCol_Details.QTY).Visible = False
            dgv_Details_Total.Columns(dgvCol_Details.QTY).Visible = False
            dgv_Selection.Columns(8).Visible = False
        End If

        If Common_Procedures.settings.Hide_Weight_Processing_Transactions Then
            dgv_Details.Columns(dgvCol_Details.WEIGHT).Visible = False
            dgv_Details_Total.Columns(dgvCol_Details.WEIGHT).Visible = False
            dgv_Selection.Columns(10).Visible = False
        End If

        If Common_Procedures.settings.Show_Folding_In_Weight_Processing_Transactions Then
            dgv_Details.Columns(dgvCol_Details.FOLDING).Visible = True
            dgv_Details_Total.Columns(dgvCol_Details.FOLDING).Visible = True
        Else
            dgv_Details.Columns(dgvCol_Details.FOLDING).Visible = False
            dgv_Details_Total.Columns(dgvCol_Details.FOLDING).Visible = False
        End If

        If Common_Procedures.settings.CustomerCode = "1516" Or Common_Procedures.settings.CustomerCode = "1490" Or Common_Procedures.settings.CustomerCode = "1464" Then

            dgv_Details.Columns(dgvCol_Details.ITEM_RECEIVED).Width = dgv_Details.Columns(dgvCol_Details.ITEM_RECEIVED).Width + 75
            dgv_Details_Total.Columns(dgvCol_Details.ITEM_RECEIVED).Width = dgv_Details.Columns(dgvCol_Details.ITEM_RECEIVED).Width

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

            'If Common_Procedures.settings.CustomerCode = "1490" Then '  --- LS EXPORTS

            'dgv_Details.Columns(dgvCol_Details.Delivery_Meter).Visible = True
            'dgv_Details.Columns(dgvCol_Details.EXC_SHT_Percentage).Visible = True

            'dgv_Details_Total.Columns(dgvCol_Details.Delivery_Meter).Visible = True
            'dgv_Details_Total.Columns(dgvCol_Details.EXC_SHT_Percentage).Visible = True

            dgv_Details.Columns(dgvCol_Details.Delivery_Meter).Width = dgv_Details.Columns(dgvCol_Details.Delivery_Meter).Width + 25
            dgv_Details.Columns(dgvCol_Details.METERS).Width = dgv_Details.Columns(dgvCol_Details.METERS).Width + 15

            If Common_Procedures.settings.CustomerCode = "1490" Then
                dgv_Details.Columns(dgvCol_Details.EXC_SHT_Percentage).Width = dgv_Details.Columns(dgvCol_Details.EXC_SHT_Percentage).Width + 15
                dgv_Details.Columns(dgvCol_Details.EXC_SHT_Mtr).Width = dgv_Details.Columns(dgvCol_Details.EXC_SHT_Mtr).Width + 15
                dgv_Details_Total.Columns(dgvCol_Details.EXC_SHT_Mtr).Width = dgv_Details_Total.Columns(dgvCol_Details.EXC_SHT_Mtr).Width + 15
                dgv_Details_Total.Columns(dgvCol_Details.EXC_SHT_Percentage).Width = dgv_Details_Total.Columns(dgvCol_Details.EXC_SHT_Percentage).Width + 15
            End If

            dgv_Details_Total.Columns(dgvCol_Details.Delivery_Meter).Width = dgv_Details_Total.Columns(dgvCol_Details.Delivery_Meter).Width + 25
            dgv_Details_Total.Columns(dgvCol_Details.METERS).Width = dgv_Details_Total.Columns(dgvCol_Details.METERS).Width + 15


            'End If
        End If

        If Common_Procedures.settings.CustomerCode = "1266" Or Common_Procedures.settings.CustomerCode = "1530" Then
            dgv_Details.Columns(dgvCol_Details.ITEM_RECEIVED).Width = dgv_Details.Columns(dgvCol_Details.ITEM_RECEIVED).Width + 75
            dgv_Details_Total.Columns(dgvCol_Details.ITEM_RECEIVED).Width = dgv_Details.Columns(dgvCol_Details.ITEM_RECEIVED).Width
        End If

        If Common_Procedures.settings.CustomerCode = "1061" Then ' --- PRAKASH COTTEX
            lbl_ReceiptMeter_Caption.Visible = True
            txt_ReceiptMeter.Visible = True
            lbl_DiffMeter_Caption.Visible = True
            txt_DiffMeter.Visible = True
        End If


        If Common_Procedures.settings.CustomerCode = "1558" Then ' --- SOTEXPA

            dgv_Details.Columns(dgvCol_Details.EXC_SHT_Wgt).Visible = True
            dgv_Details_Total.Columns(dgvCol_Details.EXC_SHT_Wgt).Visible = dgv_Details.Columns(dgvCol_Details.EXC_SHT_Wgt).Visible

            dgv_Details.Columns(dgvCol_Details.ITEM_RECEIVED).Width = dgv_Details.Columns(dgvCol_Details.ITEM_RECEIVED).Width + 40
            dgv_Details_Total.Columns(dgvCol_Details.ITEM_RECEIVED).Width = dgv_Details.Columns(dgvCol_Details.ITEM_RECEIVED).Width

        End If

        new_record()

    End Sub

    Private Sub Processing_Receipt_Textile_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        con.Close()
        con.Dispose()
    End Sub

    Private Sub Processing_Receipt_Textile_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress

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
                        If dgv1.CurrentCell.ColumnIndex >= dgv1.ColumnCount - 7 Then
                            If .CurrentCell.RowIndex = .RowCount - 1 Then

                                If txt_ReceiptMeter.Visible And txt_ReceiptMeter.Enabled Then
                                    txt_ReceiptMeter.Focus()
                                Else
                                    chk_LotComplete.Focus()
                                End If

                            Else

                                .CurrentCell = .Rows(.CurrentCell.RowIndex + 1).Cells(dgvCol_Details.DC_NO)

                            End If
                        Else

                            'If .CurrentCell.RowIndex = .RowCount - 2 And dgvCol_Details.DC_NO > 1 And Trim(.CurrentRow.Cells(dgvCol_Details.DC_NO).Value) = "" Then

                            If .CurrentCell.ColumnIndex = dgvCol_Details.DC_NO And Trim(.CurrentRow.Cells(dgvCol_Details.DC_NO).Value) = "" Then

                                If txt_ReceiptMeter.Visible And txt_ReceiptMeter.Enabled Then
                                    txt_ReceiptMeter.Focus()
                                Else
                                    chk_LotComplete.Focus()
                                End If

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

                                If cbo_ClothSales_OrderCode_forSelection.Visible = True Then
                                    cbo_ClothSales_OrderCode_forSelection.Focus()
                                Else
                                    txt_Frieght.Focus()
                                End If


                            Else

                                .CurrentCell = .Rows(.CurrentCell.RowIndex - 1).Cells(.ColumnCount - 6)

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
            MessageBox.Show(ex.Message, "DOES NOT CLOSE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

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

        '   If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.Entry_ProcessedFabric_ReceiptFrom_Processing, "~L~") = 0 And InStr(Common_Procedures.UR.Entry_ProcessedFabric_ReceiptFrom_Processing, "~D~") = 0 Then MessageBox.Show("You have No Rights to Delete", "DOES NOT INSERT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error) : Exit Sub

        Qa = MessageBox.Show("Do you want to Delete?", "FOR DELETION...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2)
        If Qa = Windows.Forms.DialogResult.No Or Qa = Windows.Forms.DialogResult.Cancel Then
            Exit Sub
        End If

        'Dim g As New Password
        'g.ShowDialog()
        'If Trim(UCase(Common_Procedures.Password_Input)) <> "TSD123" Then
        '    MessageBox.Show("Invalid Password", "PASSWORD FAILED...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
        '    Exit Sub
        'End If


        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RecNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)
        If New_Entry = True Then
            MessageBox.Show("This is New Entry", "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
        End If

        Da = New SqlClient.SqlDataAdapter("select count(*) from Textile_Processing_Delivery_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Cloth_Processing_Delivery_Code = '" & Trim(Pk_Condition2) & Trim(NewCode) & "' and (Receipt_Meters <> 0 OR Return_Meters <> 0)", con)
        Dt1 = New DataTable
        Da.Fill(Dt1)
        If Dt1.Rows.Count > 0 Then
            If IsDBNull(Dt1.Rows(0)(0).ToString) = False Then
                If Val(Dt1.Rows(0)(0).ToString) > 0 Then
                    MessageBox.Show("Already Receipt Entry Prepared", "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                    Exit Sub
                End If
            End If
        End If
        Dt1.Clear()


        Da = New SqlClient.SqlDataAdapter("select count(*) from Textile_Processing_Receipt_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Cloth_Processing_Receipt_Code = '" & Trim(NewCode) & "' and  Cloth_Processing_BillMaking_Code <> ''", con)
        Dt1 = New DataTable
        Da.Fill(Dt1)
        If Dt1.Rows.Count > 0 Then
            If IsDBNull(Dt1.Rows(0)(0).ToString) = False Then
                If Val(Dt1.Rows(0)(0).ToString) > 0 Then
                    MessageBox.Show("Already BillMaking Prepared", "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                    Exit Sub
                End If
            End If
        End If
        Dt1.Clear()

        Da = New SqlClient.SqlDataAdapter("select count(*) from Textile_Processing_Receipt_Head Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and ClothProcess_Receipt_Code = '" & Trim(NewCode) & "' and  Weaver_Piece_Checking_Code <> ''", con)
        Dt1 = New DataTable
        Da.Fill(Dt1)
        If Dt1.Rows.Count > 0 Then
            If IsDBNull(Dt1.Rows(0)(0).ToString) = False Then
                If Val(Dt1.Rows(0)(0).ToString) > 0 Then
                    MessageBox.Show("Already PieceChecking Prepared", "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
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
                    MessageBox.Show("Already Inspection Prepared", "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                    Exit Sub
                End If
            End If
        End If
        Dt1.Clear()

        'Da = New SqlClient.SqlDataAdapter("select  count(*) from Textile_Processing_Receipt_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Cloth_Processing_Receipt_Code = '" & Trim(NewCode) & "' and inspection_Meter <> 0", con)
        'Dt2 = New DataTable
        'Da.Fill(Dt2)
        'If Dt2.Rows.Count > 0 Then
        '    If IsDBNull(Dt2.Rows(0)(0).ToString) = False Then
        '        If Val(Dt2.Rows(0)(0).ToString) > 0 Then
        '            MessageBox.Show("Already Inspection Prepared", "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
        '            Exit Sub
        '        End If
        '    End If
        'End If
        'Dt2.Clear()




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

            cmd.CommandText = "Update Textile_Processing_Delivery_Details set Receipt_Meters = a.Receipt_Meters - (b.Receipt_Meters-b.ExcSht_Meters) , Receipt_Pcs = a.Receipt_Pcs - (b.Receipt_Pcs) , Receipt_Qty = a.Receipt_Qty - (b.Receipt_Qty) , Receipt_Weight = a.Receipt_Weight - (b.Receipt_Weight-b.ExcSht_Weight) from Textile_Processing_Delivery_Details a, Textile_Processing_Receipt_Details b Where b.Cloth_Processing_Receipt_Code = '" & Trim(NewCode) & "' and a.Cloth_Processing_Delivery_code = b.Cloth_Processing_Delivery_code and a.Cloth_Processing_Delivery_SlNo = b.Cloth_Processing_Delivery_SlNo"
            cmd.ExecuteNonQuery()

            'cmd.CommandText = "Update Textile_Processing_Delivery_Details set Receipt_Meters = a.Receipt_Meters - (b.Receipt_Meters-b.ExcSht_Meters) , Receipt_Pcs = a.Receipt_Pcs - (b.Receipt_Pcs) , Receipt_Qty = a.Receipt_Qty - (b.Receipt_Qty) , Receipt_Weight = a.Receipt_Weight - (b.Receipt_Weight) from Textile_Processing_Delivery_Details a, Textile_Processing_Receipt_Details b Where b.Cloth_Processing_Receipt_Code = '" & Trim(NewCode) & "' and a.Cloth_Processing_Delivery_code = b.Cloth_Processing_Delivery_code and a.Cloth_Processing_Delivery_SlNo = b.Cloth_Processing_Delivery_SlNo"
            'cmd.CommandText = "Update Textile_Processing_Delivery_Details set Receipt_Meters = a.Receipt_Meters - (b.Receipt_Meters+b.ExcSht_Meters) , Receipt_Pcs = a.Receipt_Pcs - (b.Receipt_Pcs) , Receipt_Qty = a.Receipt_Qty - (b.Receipt_Qty) , Receipt_Weight = a.Receipt_Weight - (b.Receipt_Weight) from Textile_Processing_Delivery_Details a, Textile_Processing_Receipt_Details b Where b.Cloth_Processing_Receipt_Code = '" & Trim(NewCode) & "' and a.Cloth_Processing_Delivery_code = b.Cloth_Processing_Delivery_code and a.Cloth_Processing_Delivery_SlNo = b.Cloth_Processing_Delivery_SlNo"
            'cmd.ExecuteNonQuery()

            'cmd.CommandText = "Update Textile_Processing_Delivery_Details set Receipt_Meters = a.Receipt_Meters - (b.Receipt_Meters-b.ExcSht_Meters) , Receipt_Pcs = a.Receipt_Pcs - (b.Receipt_Pcs) , Receipt_Qty = a.Receipt_Qty - (b.Receipt_Qty) , Receipt_Weight = a.Receipt_Weight - (b.Receipt_Weight) from Textile_Processing_Delivery_Details a, Textile_Processing_Receipt_Details b Where b.Cloth_Processing_Receipt_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' and a.Cloth_Processing_Delivery_code = b.Cloth_Processing_Delivery_code and a.Cloth_Processing_Delivery_SlNo = b.Cloth_Processing_Delivery_SlNo"
            ''cmd.CommandText = "Update Textile_Processing_Delivery_Details set Receipt_Meters = a.Receipt_Meters - (b.Receipt_Meters+b.ExcSht_Meters) , Receipt_Pcs = a.Receipt_Pcs - (b.Receipt_Pcs) , Receipt_Qty = a.Receipt_Qty - (b.Receipt_Qty) , Receipt_Weight = a.Receipt_Weight - (b.Receipt_Weight) from Textile_Processing_Delivery_Details a, Textile_Processing_Receipt_Details b Where b.Cloth_Processing_Receipt_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' and a.Cloth_Processing_Delivery_code = b.Cloth_Processing_Delivery_code and a.Cloth_Processing_Delivery_SlNo = b.Cloth_Processing_Delivery_SlNo"
            'cmd.ExecuteNonQuery()

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

            da = New SqlClient.SqlDataAdapter("select top 1 ClothProcess_Receipt_No from Textile_Processing_Receipt_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and ClothProcess_Receipt_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' AND ClothProcess_Receipt_Code not like '%FPFRC%'  AND ClothProcess_Receipt_Code not like '%FPFR1%' Order by for_Orderby, ClothProcess_Receipt_No", con)
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

            OrdByNo = Common_Procedures.OrderBy_CodeToValue(Trim(lbl_RecNo.Text))

            da = New SqlClient.SqlDataAdapter("select top 1 ClothProcess_Receipt_No from Textile_Processing_Receipt_Head where for_orderby > " & Str(Val(OrdByNo)) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and ClothProcess_Receipt_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' AND ClothProcess_Receipt_Code not like '%FPFRC%' AND ClothProcess_Receipt_Code not like '%FPFR1%' Order by for_Orderby, ClothProcess_Receipt_No", con)
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

            OrdByNo = Common_Procedures.OrderBy_CodeToValue(Trim(lbl_RecNo.Text))

            da = New SqlClient.SqlDataAdapter("select top 1 ClothProcess_Receipt_No from Textile_Processing_Receipt_Head where for_orderby < " & Str(Val(OrdByNo)) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and ClothProcess_Receipt_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' AND ClothProcess_Receipt_Code not like '%FPFRC%'  AND ClothProcess_Receipt_Code not like '%FPFR1%' Order by for_Orderby desc, ClothProcess_Receipt_No desc", con)
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
            da = New SqlClient.SqlDataAdapter("select top 1 ClothProcess_Receipt_No from Textile_Processing_Receipt_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and ClothProcess_Receipt_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' AND ClothProcess_Receipt_Code not like '%FPFRC%' AND ClothProcess_Receipt_Code not like '%FPFR1%' Order by for_Orderby desc, ClothProcess_Receipt_No desc", con)
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
        Dim NewID As Integer = 0

        Try

            Displaying = True

            clear()

            New_Entry = True

            lbl_RecNo.Text = Common_Procedures.get_MaxCode(con, "Textile_Processing_Receipt_Head", "ClothProcess_Receipt_Code", "For_OrderBy", "ClothProcess_Receipt_Code not like '%FPFRC%'  AND ClothProcess_Receipt_Code not like '%FPFR1%'", Val(lbl_Company.Tag), Common_Procedures.FnYearCode)
            lbl_RecNo.ForeColor = Color.Red

            Dim Prev_Rec_Type As String = ""

            da = New SqlClient.SqlDataAdapter("select Receipt_Type from Textile_Processing_Receipt_Head where For_OrderBy = (Select max(For_OrderBy) from  Textile_Processing_Receipt_Head where ClothProcess_Receipt_Code like '%" & Common_Procedures.FnYearCode & "%' and Company_IdNo = " & Val(lbl_Company.Tag).ToString & ")", con)
            da.Fill(dt)

            If dt.Rows.Count > 0 Then
                If Not IsDBNull(dt.Rows(0).Item(0)) Then
                    cbo_Receipt_Type.Text = dt.Rows(0).Item(0)
                    cbo_Receipt_Type.Tag = cbo_Receipt_Type.Text
                End If
            End If

            If Trim(UCase(cbo_Receipt_Type.Text)) = "DELIVERY" Then
                'On Error Resume Next
                btn_Selection.Enabled = True
                'dgv_Details.Columns(0).ReadOnly = True
                dgv_Details.Columns(1).ReadOnly = True
                dgv_Details.Columns(dgvCol_Details.ITEM_RECEIVED).ReadOnly = True
                dgv_Details.Columns(dgvCol_Details.PROCESSING).ReadOnly = True
                dgv_Details.Columns(dgvCol_Details.LOT_NO).ReadOnly = True
                dgv_Details.AllowUserToAddRows = False
            Else
                'On Error Resume Next
                btn_Selection.Enabled = False
                'dgv_Details.Columns(0).ReadOnly = False
                dgv_Details.Columns(1).ReadOnly = False
                dgv_Details.Columns(dgvCol_Details.ITEM_RECEIVED).ReadOnly = False
                dgv_Details.Columns(dgvCol_Details.PROCESSING).ReadOnly = False
                dgv_Details.Columns(dgvCol_Details.LOT_NO).ReadOnly = False
                dgv_Details.AllowUserToAddRows = True
            End If

            pnl_Back.Visible = True
            If dtp_Date.Enabled And dtp_Date.Visible Then dtp_Date.Focus()

        Catch ex As Exception

            Displaying = False
            MessageBox.Show(ex.Message, "FOR NEW RECORD...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

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

            inpno = InputBox("Enter Rec.No.", "FOR FINDING...")

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
                MessageBox.Show("Rec No. does not exists", "DOES NOT FIND...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

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

        '  If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.Entry_ProcessedFabric_ReceiptFrom_Processing, "~L~") = 0 And InStr(Common_Procedures.UR.Entry_ProcessedFabric_ReceiptFrom_Processing, "~I~") = 0 Then MessageBox.Show("You have No Rights to Insert", "DOES NOT INSERT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error) : Exit Sub

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
                    MessageBox.Show("Invalid Rec No", "DOES NOT INSERT NEW RECEIPT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

                Else
                    new_record()
                    Insert_Entry = True
                    lbl_RecNo.Text = Trim(UCase(inpno))

                End If

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT INSERT NEW RECEIPT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

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
        Dim Col_ID As Integer = 0
        Dim Itfp_ID As Integer = 0
        Dim Itgry_ID As Integer = 0
        Dim Sno As Integer = 0
        Dim Scno As Integer = 0
        Dim Partcls As String = ""
        Dim EntID As String = ""
        Dim vNXT_Proc_idno As Integer = 0
        Dim PBlNo As String = ""
        Dim vTotPcs As Single, vTotMtrs As Single, vtotqty As Single, vtotDelMtr As Single
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
        Dim StkOff_ID As Int16 = Common_Procedures.CommonLedger.OwnSort_Ac
        Dim vFIELDNAME As String = ""
        Dim ItmfpID_aftr_nxtprocs As Integer = 0
        Dim vCLOID As Integer
        Dim vRECON_IN As String
        Dim vCLOSTK_IN As String
        Dim vExcSrt_Wgt = ""

        If Common_Procedures.settings.CustomerCode = "1516" Then
            StkOff_ID = Common_Procedures.CommonLedger.OwnSort_Ac  ' Val(Common_Procedures.CommonLedger.Godown_Ac)
        End If

        ' If Common_Procedures.UserRight_Check(Common_Procedures.UR.Entry_ProcessedFabric_ReceiptFrom_Processing, New_Entry) = False Then Exit Sub

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

        Led_ID = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Ledger.Text)
        If Led_ID = 0 Then
            MessageBox.Show("Invalid Party Name", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If cbo_Ledger.Enabled And cbo_Ledger.Visible Then cbo_Ledger.Focus()
            Exit Sub
        End If

        If Trim(txt_PartyDcNo.Text) = "" Then
            MessageBox.Show("Invalid Party Dc No", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If txt_PartyDcNo.Enabled And txt_PartyDcNo.Visible Then txt_PartyDcNo.Focus()
            Exit Sub
        End If

        Del_Id = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_DeliveryTo.Text)

        Dim DEL_LED_TYPE As String = Common_Procedures.get_FieldValue(con, "Ledger_Head", "Ledger_Type", "Ledger_IdNo = " & Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_DeliveryTo.Text))

        Tr_ID = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_TransportName.Text)

        If Trim(UCase(DEL_LED_TYPE)) = "GODOWN" Then
            cbo_ProcessingName_of_NextProcess.Text = ""
            vNXT_Proc_idno = 0
        Else
            vNXT_Proc_idno = Common_Procedures.Process_NameToIdNo(con, cbo_ProcessingName_of_NextProcess.Text)
        End If

        lbl_UserName.Text = Common_Procedures.User.IdNo

        lotSts = 0

        If Del_Id = 0 Then
            MessageBox.Show("Invalid Delivery To Name", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If cbo_DeliveryTo.Enabled And cbo_DeliveryTo.Visible Then cbo_DeliveryTo.Focus()
            Exit Sub
        End If

        If Trim(UCase(DEL_LED_TYPE)) <> "GODOWN" And vNXT_Proc_idno = 0 Then
            MessageBox.Show("Invalid Next Process Name", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If cbo_ProcessingName_of_NextProcess.Enabled And cbo_ProcessingName_of_NextProcess.Visible Then cbo_ProcessingName_of_NextProcess.Focus()
            Exit Sub
        End If

        If chk_LotComplete.Checked = True Then lotSts = 1

        With dgv_Details
            For i = 0 To dgv_Details.RowCount - 1
                If Val(.Rows(i).Cells(dgvCol_Details.PCS).Value) <> 0 Or Val(.Rows(i).Cells(dgvCol_Details.METERS).Value) <> 0 Or Val(.Rows(i).Cells(dgvCol_Details.WEIGHT).Value) <> 0 Or Val(.Rows(i).Cells(dgvCol_Details.EXC_SHT_Mtr).Value) <> 0 Then

                    If Trim(dgv_Details.Rows(i).Cells(dgvCol_Details.ITEM_RECEIVED).Value) = "" Then
                        MessageBox.Show("Invalid FP Item", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                        If dgv_Details.Enabled And dgv_Details.Visible Then
                            dgv_Details.Focus()
                            dgv_Details.CurrentCell = dgv_Details.Rows(i).Cells(dgvCol_Details.ITEM_RECEIVED)

                        End If
                        Exit Sub
                    End If

                    vCLOID = Common_Procedures.Cloth_NameToIdNo(con, .Rows(i).Cells(dgvCol_Details.ITEM_RECEIVED).Value)
                    If vCLOID = 0 Then
                        MessageBox.Show("Invalid GREY Item", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                        If dgv_Details.Enabled And dgv_Details.Visible Then
                            dgv_Details.Focus()
                            dgv_Details.CurrentCell = dgv_Details.Rows(i).Cells(dgvCol_Details.ITEM_RECEIVED)
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

                    If Trim(dgv_Details.Rows(i).Cells(dgvCol_Details.PROCESSING).Value) = "" Then
                        MessageBox.Show("Invalid PROCESS Name", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                        If dgv_Details.Enabled And dgv_Details.Visible Then
                            dgv_Details.Focus()
                            dgv_Details.CurrentCell = dgv_Details.Rows(i).Cells(dgvCol_Details.PROCESSING)

                        End If
                        Exit Sub
                    End If

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


                    If Trim(UCase(cbo_Receipt_Type.Text)) = "DELIVERY" Then
                        If Trim(dgv_Details.Rows(i).Cells(dgvCol_Details.Process_Delv_Code).Value) = "" Then
                            MessageBox.Show("Invalid Delivery Code", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                            If dgv_Details.Enabled And dgv_Details.Visible Then
                                dgv_Details.Focus()
                                dgv_Details.CurrentCell = dgv_Details.Rows(i).Cells(dgvCol_Details.DC_NO)
                            End If
                            Exit Sub
                        End If

                        If Val(dgv_Details.Rows(i).Cells(dgvCol_Details.Process_Delv_Slno).Value) = 0 Then
                            MessageBox.Show("Invalid Delivery Number", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                            If dgv_Details.Enabled And dgv_Details.Visible Then
                                dgv_Details.Focus()
                                dgv_Details.CurrentCell = dgv_Details.Rows(i).Cells(dgvCol_Details.DC_NO)
                            End If
                            Exit Sub
                        End If
                    End If

                End If

            Next
        End With

        Total_Calculation()
        vTotPcs = 0 : vTotMtrs = 0 : vTotWeight = 0 : vtotqty = 0 : vExcSrt_Wgt = 0
        If dgv_Details_Total.RowCount > 0 Then
            vTotPcs = Val(dgv_Details_Total.Rows(0).Cells(dgvCol_Details.PCS).Value())
            vtotqty = Val(dgv_Details_Total.Rows(0).Cells(dgvCol_Details.QTY).Value())
            vTotMtrs = Val(dgv_Details_Total.Rows(0).Cells(dgvCol_Details.METERS).Value())
            vtotDelMtr = Val(dgv_Details_Total.Rows(0).Cells(dgvCol_Details.Delivery_Meter).Value())
            vTotWeight = Val(dgv_Details_Total.Rows(0).Cells(dgvCol_Details.WEIGHT).Value())
            vExcSrt = Val(dgv_Details_Total.Rows(0).Cells(dgvCol_Details.EXC_SHT_Mtr).Value())
            vExcSrt_Wgt = Val(dgv_Details_Total.Rows(0).Cells(dgvCol_Details.EXC_SHT_Wgt).Value())
        End If
        Dt1.Clear()


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
                NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RecNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            Else

                lbl_RecNo.Text = Common_Procedures.get_MaxCode(con, "Textile_Processing_Receipt_Head", "ClothProcess_Receipt_Code", "For_OrderBy", "ClothProcess_Receipt_Code not like '%FPFRC%'  AND ClothProcess_Receipt_Code not like '%FPFR1%'", Val(lbl_Company.Tag), Common_Procedures.FnYearCode, tr)
                NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RecNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            End If

            cmd.Connection = con
            cmd.Transaction = tr

            cmd.Parameters.Clear()
            cmd.Parameters.AddWithValue("@RecDate", dtp_Date.Value.Date)

            cmd.CommandText = "Truncate Table TempTable_For_NegativeStock"
            cmd.ExecuteNonQuery()

            If New_Entry = True Then

                cmd.CommandText = "Insert into Textile_Processing_Receipt_Head (ClothProcess_Receipt_Code, Company_IdNo, ClothProcess_Receipt_No, for_OrderBy, ClothProcess_Receipt_Date, Ledger_IdNo, Job_No, Transport_IdNo, Freight_Charges, DeliveryTo_IdNo,Total_Pcs,Total_Qty, Total_Meters, Total_Weight ,Total_ExcessShort,Lot_Status,Party_Dc_No,Processing_Idno     ,   User_IdNo , Vehicle_No  ,Receipt_Type , Total_Processing_Delivery_Meters , ClothSales_OrderCode_forSelection ,Total_ExcSht_Weight  ) Values ('" & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_RecNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_RecNo.Text))) & ", @RecDate, " & Str(Val(Led_ID)) & ", '" & Trim(txt_JobNo.Text) & "', " & Str(Val(Tr_ID)) & ", " & Str(Val(txt_Frieght.Text)) & ",  " & Val(Del_Id) & "," & Str(Val(vTotPcs)) & "," & Val(vtotqty) & " , " & Str(Val(vTotMtrs)) & ", " & Str(Val(vTotWeight)) & " , " & Val(vExcSrt) & "," & Val(lotSts) & ", '" & Trim(txt_PartyDcNo.Text) & "'," & Val(vNXT_Proc_idno) & ", " & Val(lbl_UserName.Text) & " , '" & Trim(cbo_VehicleNo.Text) & "','" & cbo_Receipt_Type.Text & "' ," & Str(Val(vtotDelMtr)) & " , '" & Trim(cbo_ClothSales_OrderCode_forSelection.Text) & "' , " & Val(vExcSrt_Wgt) & "  )"
                cmd.ExecuteNonQuery()

            Else

                Da = New SqlClient.SqlDataAdapter("select Weaver_Piece_Checking_Code from Textile_Processing_Receipt_Head Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and ClothProcess_Receipt_Code = '" & Trim(NewCode) & "'", con)
                Da.SelectCommand.Transaction = tr
                Dt1 = New DataTable
                Da.Fill(Dt1)
                If Dt1.Rows.Count > 0 Then
                    If IsDBNull(Dt1.Rows(0).Item("Weaver_Piece_Checking_Code").ToString) = False Then
                        If Trim(Dt1.Rows(0).Item("Weaver_Piece_Checking_Code").ToString) <> "" Then
                            Throw New ApplicationException("Piece Inspection prepared - " & Trim(Dt1.Rows(0).Item("Weaver_Piece_Checking_Code").ToString))
                        End If
                    End If
                End If
                Dt1.Clear()

                cmd.CommandText = "Update Textile_Processing_Receipt_Head set ClothProcess_Receipt_Date = @RecDate, Ledger_IdNo = " & Val(Led_ID) & ", Job_No = '" & Trim(txt_JobNo.Text) & "' , Transport_IdNo = " & Val(Tr_ID) & ", Freight_Charges = " & Val(txt_Frieght.Text) & ", DeliveryTo_IdNo = " & Val(Del_Id) & ", Total_Pcs = " & Val(vTotPcs) & " , Vehicle_No =  '" & Trim(cbo_VehicleNo.Text) & "' ,Total_Qty = " & Val(vtotqty) & " , Total_Meters = " & Val(vTotMtrs) & ",Total_Weight = " & Val(vTotWeight) & " ,Total_ExcessShort = " & Val(vExcSrt) & " ,Lot_Status = " & Val(lotSts) & " ,Party_Dc_No = '" & Trim(txt_PartyDcNo.Text) & "',Processing_Idno = " & Val(vNXT_Proc_idno) & ",  User_IdNo  = " & Val(lbl_UserName.Text) & ",Receipt_Type = '" & cbo_Receipt_Type.Text & "'  , Total_Processing_Delivery_Meters = " & Str(Val(vtotDelMtr)) & " , ClothSales_OrderCode_forSelection = '" & Trim(cbo_ClothSales_OrderCode_forSelection.Text) & "' ,Total_ExcSht_Weight = " & Val(vExcSrt_Wgt) & "  Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and ClothProcess_Receipt_Code = '" & Trim(NewCode) & "'"
                Nr = cmd.ExecuteNonQuery()

                cmd.CommandText = "Update Textile_Processing_Delivery_Details set Receipt_Meters = a.Receipt_Meters - (b.Receipt_Meters - b.ExcSht_Meters), Receipt_Pcs = a.Receipt_Pcs - (b.Receipt_Pcs) , Receipt_Qty = a.Receipt_Qty - (b.Receipt_Qty) , Receipt_Weight = a.Receipt_Weight - (b.Receipt_Weight- b.ExcSht_Weight) from Textile_Processing_Delivery_Details a, Textile_Processing_Receipt_Details b Where b.Cloth_Processing_Receipt_Code = '" & Trim(NewCode) & "' and a.Cloth_Processing_Delivery_code = b.Cloth_Processing_Delivery_code and a.Cloth_Processing_Delivery_SlNo = b.Cloth_Processing_Delivery_SlNo"
                'cmd.CommandText = "Update Textile_Processing_Delivery_Details set Receipt_Meters = a.Receipt_Meters - (b.Receipt_Meters - b.ExcSht_Meters), Receipt_Pcs = a.Receipt_Pcs - (b.Receipt_Pcs) , Receipt_Qty = a.Receipt_Qty - (b.Receipt_Qty) , Receipt_Weight = a.Receipt_Weight - (b.Receipt_Weight) from Textile_Processing_Delivery_Details a, Textile_Processing_Receipt_Details b Where b.Cloth_Processing_Receipt_Code = '" & Trim(NewCode) & "' and a.Cloth_Processing_Delivery_code = b.Cloth_Processing_Delivery_code and a.Cloth_Processing_Delivery_SlNo = b.Cloth_Processing_Delivery_SlNo"

                'cmd.CommandText = "Update Textile_Processing_Delivery_Details set Receipt_Meters = a.Receipt_Meters - (b.Receipt_Meters + b.ExcSht_Meters), Receipt_Pcs = a.Receipt_Pcs - (b.Receipt_Pcs) , Receipt_Qty = a.Receipt_Qty - (b.Receipt_Qty) , Receipt_Weight = a.Receipt_Weight - (b.Receipt_Weight) from Textile_Processing_Delivery_Details a, Textile_Processing_Receipt_Details b Where b.Cloth_Processing_Receipt_Code = '" & Trim(NewCode) & "' and a.Cloth_Processing_Delivery_code = b.Cloth_Processing_Delivery_code and a.Cloth_Processing_Delivery_SlNo = b.Cloth_Processing_Delivery_SlNo"
                Nr = cmd.ExecuteNonQuery()


                '----Lot Complete status

                cmd.CommandText = "Update  Textile_Processing_Delivery_Details set Lot_Complete_status = 0, Lot_Complete_Code = '' where Lot_Complete_Code  = '" & Trim(Pk_Condition) & Trim(NewCode) & "'  "
                Nr = cmd.ExecuteNonQuery()


                cmd.CommandText = "Insert into TempTable_For_NegativeStock ( Stock_Type, Reference_Code, Reference_Date, Company_Idno, Ledger_Idno         , Item_IdNo, Rack_IdNo ) " &
                                   " Select                               'PI'    , Reference_Code, Reference_Date, Company_IdNo, DeliveryTo_StockIdNo, Item_IdNo, Rack_IdNo from Stock_Item_Processing_Details where Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()

            End If

            cmd.CommandText = "Delete from Textile_Processing_Receipt_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Cloth_Processing_receipt_Code = '" & Trim(NewCode) & "' and Cloth_Processing_BillMaking_Code = '' and Inspection_Meters = 0"
            Nr = cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Stock_Cloth_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            Partcls = "Rec : Dc.No. " & Trim(lbl_RecNo.Text)
            PBlNo = Trim(lbl_RecNo.Text)
            EntID = Trim(Pk_Condition) & Trim(lbl_RecNo.Text)

            With dgv_Details

                Sno = 0
                Scno = 0

                For i = 0 To .RowCount - 1

                    If Val(.Rows(i).Cells(dgvCol_Details.PCS).Value) <> 0 Or Val(.Rows(i).Cells(dgvCol_Details.METERS).Value) <> 0 Or Val(.Rows(i).Cells(dgvCol_Details.WEIGHT).Value) <> 0 Or Val(.Rows(i).Cells(dgvCol_Details.EXC_SHT_Mtr).Value) <> 0 Then

                        Itfp_ID = Common_Procedures.Cloth_NameToIdNo(con, .Rows(i).Cells(dgvCol_Details.ITEM_RECEIVED).Value, tr)
                        Col_ID = Common_Procedures.Colour_NameToIdNo(con, .Rows(i).Cells(dgvCol_Details.COLOUR).Value, tr)

                        ItmfpID_aftr_nxtprocs = 0
                        If vNXT_Proc_idno <> 0 Then
                            ItmfpID_aftr_nxtprocs = Common_Procedures.Cloth_NameToIdNo(con, .Rows(i).Cells(dgvCol_Details.ITEM_after_NextProcess).Value, tr)
                            If ItmfpID_aftr_nxtprocs = 0 Then
                                ItmfpID_aftr_nxtprocs = Itfp_ID
                            End If
                        End If

                        'If Common_Procedures.settings.CustomerCode = "1061" or Common_Procedures.settings.CustomerCode = "1558" Then

                        Lot_ID = Common_Procedures.Lot_NoToIdNo(con, .Rows(i).Cells(dgvCol_Details.LOT_NO).Value, tr)

                        If Val(.Rows(i).Cells(dgvCol_Details.FOLDING).Value) = 0 Then
                            .Rows(i).Cells(dgvCol_Details.FOLDING).Value = "100"
                        End If

                        Proc_ID = Common_Procedures.Process_NameToIdNo(con, .Rows(i).Cells(dgvCol_Details.PROCESSING).Value, tr)

                        Sno = Sno + 1
                        Scno = Scno + 1

                        Nr = 0

                        cmd.CommandText = "Update Textile_Processing_Receipt_Details set Cloth_Processing_Receipt_Date = @RecDate , Sl_No = " & Str(Val(Sno)) & " , Dc_Rc_No = '" & Trim(.Rows(i).Cells(dgvCol_Details.DC_NO).Value) & "' , Ledger_Idno = " & Val(Led_ID) & ", Item_Idno = " & Str(Val(.Rows(i).Cells(dgvCol_Details.ITEM_DELIVERED).Value)) & " ,Item_To_Idno = " & Str(Val(Itfp_ID)) & ", Colour_Idno = " & Val(Col_ID) & ", Processing_Idno =  " & Val(Proc_ID) & ",Lot_IdNo = " & Val(Lot_ID) & " ,Receipt_Pcs =  " & Val(.Rows(i).Cells(dgvCol_Details.PCS).Value) & ",Receipt_Qty = " & Val(.Rows(i).Cells(dgvCol_Details.QTY).Value) & " ,Receipt_Meters = " & Str(Val(.Rows(i).Cells(dgvCol_Details.METERS).Value)) & ",Receipt_Weight =" & Str(Val(.Rows(i).Cells(dgvCol_Details.WEIGHT).Value)) & "  ,ExcSht_Meters =  " & Str(Val(.Rows(i).Cells(dgvCol_Details.EXC_SHT_Mtr).Value)) & " ,  Cloth_Processing_Delivery_code = '" & Trim(.Rows(i).Cells(dgvCol_Details.Process_Delv_Code).Value) & "', Cloth_Processing_Delivery_Slno = " & Str(Val(.Rows(i).Cells(dgvCol_Details.Process_Delv_Slno).Value)) & ", Folding = " & Str(Val(.Rows(i).Cells(dgvCol_Details.FOLDING).Value)) & " ,Processing_Delivery_Meters = " & Str(Val(.Rows(i).Cells(dgvCol_Details.Delivery_Meter).Value)) & ",Excess_Short_Percentage =" & Str(Val(.Rows(i).Cells(dgvCol_Details.EXC_SHT_Percentage).Value)) & " , ItemIdno_after_NextProcess  = " & Str(Val(ItmfpID_aftr_nxtprocs)) & " ,  ExcSht_Weight   =" & Str(Val(.Rows(i).Cells(dgvCol_Details.EXC_SHT_Wgt).Value)) & " ,Cloth_Processing_Receipt_Slno =" & Str(Val(.Rows(i).Cells(dgvCol_Details.Cloth_Processing_Receipt_Slno).Value)) & "  where Company_IdNo =  " & Str(Val(lbl_Company.Tag)) & " and Cloth_Processing_Receipt_Code = '" & Trim(NewCode) & "' and Cloth_Processing_Receipt_Slno = " & Str(Val(.Rows(i).Cells(dgvCol_Details.Cloth_Processing_Receipt_Slno).Value)) & " "
                        Nr = cmd.ExecuteNonQuery()


                        If Nr = 0 Then
                            '--new
                            cmd.CommandText = "Insert into Textile_Processing_Receipt_Details ( Cloth_Processing_Receipt_Code,            Company_IdNo             , Cloth_Processing_Receipt_No          ,                                  for_OrderBy                              ,          Cloth_Processing_Receipt_Date   ,                            Sl_No                                   ,                    Dc_Rc_No                               ,      Ledger_Idno     ,              Item_Idno                                            ,                  Item_To_Idno        , Colour_Idno           , Processing_Idno       ,       Lot_IdNo     ,Receipt_Pcs                                           ,               Receipt_Qty                            ,                    Receipt_Meters                            ,               Receipt_Weight                                  ,              ExcSht_Meters                                        ,      Cloth_Processing_Delivery_code                                     ,       Cloth_Processing_Delivery_Slno                                   ,                                 Folding                                         ,               Processing_Delivery_Meters                       ,                                      Excess_Short_Percentage                             ,        ItemIdno_after_NextProcess       ,                                 ExcSht_Weight                       ,                                    Cloth_Processing_Receipt_Slno                      ) " &
                                                                            " Values  (   '" & Trim(NewCode) & "'     ,  " & Str(Val(lbl_Company.Tag)) & " ,      '" & Trim(lbl_RecNo.Text) & "'  , " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_RecNo.Text))) & "   ,       @RecDate                           ,        " & Str(Val(.Rows(i).Cells(dgvCol_Details.SLNO).Value)) & " , '" & Trim(.Rows(i).Cells(dgvCol_Details.DC_NO).Value) & "', " & Val(Led_ID) & "   , " & Str(Val(.Rows(i).Cells(dgvCol_Details.ITEM_DELIVERED).Value)) & ", " & Str(Val(Itfp_ID)) & "            ,    " & Val(Col_ID) & ", " & Val(Proc_ID) & "  ," & Val(Lot_ID) & " , " & Val(.Rows(i).Cells(dgvCol_Details.PCS).Value) & ", " & Val(.Rows(i).Cells(dgvCol_Details.QTY).Value) & ", " & Str(Val(.Rows(i).Cells(dgvCol_Details.METERS).Value)) & ", " & Str(Val(.Rows(i).Cells(dgvCol_Details.WEIGHT).Value)) & " ," & Str(Val(.Rows(i).Cells(dgvCol_Details.EXC_SHT_Mtr).Value)) & " , '" & Trim(.Rows(i).Cells(dgvCol_Details.Process_Delv_Code).Value) & "'  ,   " & Str(Val(.Rows(i).Cells(dgvCol_Details.Process_Delv_Slno).Value)) & " ,   " & Str(Val(.Rows(i).Cells(dgvCol_Details.FOLDING).Value)) & " , " & Str(Val(.Rows(i).Cells(dgvCol_Details.Delivery_Meter).Value)) & "  , " & Str(Val(.Rows(i).Cells(dgvCol_Details.EXC_SHT_Percentage).Value)) & ", " & Str(Val(ItmfpID_aftr_nxtprocs)) & " ,  " & Str(Val(.Rows(i).Cells(dgvCol_Details.EXC_SHT_Wgt).Value)) & " , " & Str(Val(.Rows(i).Cells(dgvCol_Details.Cloth_Processing_Receipt_Slno).Value)) & "  ) "
                            cmd.ExecuteNonQuery()

                            ' ---old
                            'cmd.CommandText = "Insert into Textile_Processing_Receipt_Details(Cloth_Processing_Receipt_Code,            Company_IdNo                , Cloth_Processing_Receipt_No       ,                                  for_OrderBy                   ,          Cloth_Processing_Receipt_Date   ,                            Sl_No                  ,                    Dc_Rc_No            ,      Ledger_Idno     ,              Item_Idno              ,                  Item_To_Idno        , Colour_Idno       , Processing_Idno       ,       Lot_IdNo                    ,Receipt_Pcs            ,               Receipt_Qty    ,                    Receipt_Meters              ,               Receipt_Weight              ,              ExcSht_Meters                ,      Cloth_Processing_Delivery_code        ,       Cloth_Processing_Delivery_Slno         ,     Processed_Fabric_Inspection_Code) " &
                            '                                                        "Values   (     '" & Trim(NewCode) & "',    " & Str(Val(lbl_Company.Tag)) & ",      '" & Trim(lbl_RecNo.Text) & "'  , " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_RecNo.Text))) & "   ,       @RecDate                 ,        " & Str(Val(.Rows(i).Cells(dgvCol_Details.SLNO).Value)) & " , '" & Trim(.Rows(i).Cells(dgvCol_Details.DC_NO).Value) & "', " & Val(Led_ID) & "   , " & Str(Val(.Rows(i).Cells(13).Value)) & ", " & Str(Val(Itfp_ID)) & ",    " & Val(Col_ID) & ", " & Val(Proc_ID) & "  ," & Val(Lot_ID) & " , " & Val(.Rows(i).Cells(dgvCol_Details.PCS ).Value) & ", " & Val(.Rows(i).Cells(dgvCol_Details.QTY ).Value) & ", " & Str(Val(.Rows(i).Cells(dgvCol_Details.METERS ).Value)) & ", " & Str(Val(.Rows(i).Cells(dgvCol_Details.WEIGHT).Value)) & " ," & Str(Val(.Rows(i).Cells(dgvCol_Details.EXC_SHT_Mtr).Value)) & " , '" & Trim(.Rows(i).Cells(dgvCol_Details.Process_Delv_Code).Value) & "'  ,   " & Str(Val(.Rows(i).Cells(dgvCol_Details.Process_Delv_SLNO).Value)) & "   ,'" & Trim(.Rows(i).Cells(dgvCol_Details.).Value) & "')"
                            'cmd.ExecuteNonQuery()

                        End If

                        If Trim(UCase(cbo_Receipt_Type.Text)) = "DELIVERY" Then

                            Nr = 0
                            If Trim(.Rows(i).Cells(dgvCol_Details.Process_Delv_Code).Value) <> "" And Val(.Rows(i).Cells(dgvCol_Details.Process_Delv_Slno).Value) <> 0 Then
                                cmd.CommandText = "Update Textile_Processing_Delivery_Details set Receipt_Meters = Receipt_Meters + (" & Str(Val(.Rows(i).Cells(dgvCol_Details.METERS).Value) - Val(.Rows(i).Cells(dgvCol_Details.EXC_SHT_Mtr).Value)) & "), Receipt_Pcs = Receipt_Pcs + " & Str(Val(.Rows(i).Cells(dgvCol_Details.PCS).Value)) & " , Receipt_Qty = Receipt_Qty + " & Str(Val(.Rows(i).Cells(dgvCol_Details.QTY).Value)) & "  ,  Receipt_Weight = Receipt_Weight + " & Str(Val(.Rows(i).Cells(dgvCol_Details.WEIGHT).Value) - Val(.Rows(i).Cells(dgvCol_Details.EXC_SHT_Wgt).Value)) & "  Where Cloth_Processing_Delivery_code = '" & Trim(.Rows(i).Cells(dgvCol_Details.Process_Delv_Code).Value) & "' and Cloth_Processing_Delivery_SlNo = " & Str(Val(.Rows(i).Cells(dgvCol_Details.Process_Delv_Slno).Value)) & " and Ledger_IdNo = " & Str(Val(Led_ID))
                                'cmd.CommandText = "Update Textile_Processing_Delivery_Details set Receipt_Meters = Receipt_Meters + (" & Str(Val(.Rows(i).Cells(dgvCol_Details.METERS).Value) - Val(.Rows(i).Cells(dgvCol_Details.EXC_SHT_Mtr).Value)) & "), Receipt_Pcs = Receipt_Pcs + " & Str(Val(.Rows(i).Cells(dgvCol_Details.PCS).Value)) & " , Receipt_Qty = Receipt_Qty + " & Str(Val(.Rows(i).Cells(dgvCol_Details.QTY).Value)) & "  ,  Receipt_Weight = Receipt_Weight + " & Str(Val(.Rows(i).Cells(dgvCol_Details.WEIGHT).Value)) & "  Where Cloth_Processing_Delivery_code = '" & Trim(.Rows(i).Cells(dgvCol_Details.Process_Delv_Code).Value) & "' and Cloth_Processing_Delivery_SlNo = " & Str(Val(.Rows(i).Cells(dgvCol_Details.Process_Delv_Slno).Value)) & " and Ledger_IdNo = " & Str(Val(Led_ID))


                                'cmd.CommandText = "Update Textile_Processing_Delivery_Details set Receipt_Meters = Receipt_Meters + (" & Str(Val(.Rows(i).Cells(dgvCol_Details.METERS).Value) + Val(.Rows(i).Cells(dgvCol_Details.EXC_SHT_Mtr).Value)) & "), Receipt_Pcs = Receipt_Pcs + " & Str(Val(.Rows(i).Cells(dgvCol_Details.PCS).Value)) & " , Receipt_Qty = Receipt_Qty + " & Str(Val(.Rows(i).Cells(dgvCol_Details.QTY).Value)) & "  ,  Receipt_Weight = Receipt_Weight + " & Str(Val(.Rows(i).Cells(dgvCol_Details.WEIGHT).Value)) & "  Where Cloth_Processing_Delivery_code = '" & Trim(.Rows(i).Cells(dgvCol_Details.Process_Delv_Code).Value) & "' and Cloth_Processing_Delivery_SlNo = " & Str(Val(.Rows(i).Cells(dgvCol_Details.Process_Delv_Slno).Value)) & " and Ledger_IdNo = " & Str(Val(Led_ID))
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
                            cmd.CommandText = "Update  Textile_Processing_Delivery_Details set Lot_Complete_status   = " & Str(Val(lotSts)) & ", Lot_Complete_Code  = '" & Trim(Pk_Condition) & Trim(NewCode) & "' Where Lot_Complete_status = 0 and Cloth_Processing_Delivery_code = '" & Trim(.Rows(i).Cells(dgvCol_Details.Process_Delv_Code).Value) & "' and Cloth_Processing_Delivery_SlNo = " & Str(Val(.Rows(i).Cells(dgvCol_Details.Process_Delv_Slno).Value)) & " and Ledger_IdNo = " & Str(Val(Led_ID))
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

                            'ClthWarp_Idno = 0
                            'ClthWarp_Idno = Str(Val(Dt1.Rows(i).Item("Cloth_WarpCount_IdNo").ToString))

                            'Da = New SqlClient.SqlDataAdapter("Select Stock_In , Meters_Pcs from  EndsCount_Head Where EndsCount_IdNo = " & Str(Val(ClthWarp_Idno)), con)
                            'Da.SelectCommand.Transaction = tr
                            'dt2 = New DataTable
                            'Da.Fill(dt2)
                            'If dt2.Rows.Count > 0 Then
                            '    Stock_In = dt2.Rows(0)("Stock_In").ToString
                            '    mtrspcs = Val(dt2.Rows(0)("Meters_Pcs").ToString)
                            'End If
                            'dt2.Clear()


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

                        da1 = New SqlClient.SqlDataAdapter("select a.* , A.Receipt_Meters as Rec_Mtr_Bef , b.* , b.Receipt_Meters as Rec_Mtr ,b.Return_Meters as Retn_Mtr , C.* from Textile_Processing_Receipt_Details a LEFT OUTER JOIN Textile_Processing_Delivery_Details b On a.Cloth_Processing_Delivery_Code = b.Cloth_Processing_Delivery_Code and a.Cloth_Processing_Delivery_SlNo = b.Cloth_Processing_Delivery_SlNo INNER JOIN Cloth_Head c ON c.Cloth_Idno = b.Item_to_IdNo Where a.Cloth_Processing_Receipt_Code = '" & Trim(NewCode) & "' and a.Cloth_Processing_Receipt_Slno = " & Str(Val(.Rows(i).Cells(dgvCol_Details.Cloth_Processing_Receipt_Slno).Value)) & " And a.Sl_No = " & Str(Val(Sno)), con)
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
                            '  MessageBox.Show("Invalid Shortage Meter" & Chr(13) & "Allowed Shortage (%): " & " " & Str(Val(Allow_Sht_Mtr)) & " "(" " & Str(Val(Allow_Sht_Perc)) & " ")"  " & Chr(13) & "Actual Shortage (%): " & " " & Str(Val(Ent_Sht_Mtr)) & "", "DOES NOT SAVE.!!!", MessageBoxButtons.OKCancel, MessageBoxIcon.Information)
                        End If

                        '----------------------------------------

                    End If

                Next

            End With


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

            If Del_Id <> 0 And Trim(UCase(DEL_LED_TYPE)) <> "GODOWN" Then

                Partcls = "Delv : Dc.No. " & Trim(lbl_RecNo.Text)
                PBlNo = Trim(lbl_RecNo.Text)
                EntID = Trim(Pk_Condition2) & Trim(lbl_RecNo.Text)

                cmd.CommandText = "Insert into Textile_Processing_Delivery_Head ( ClothProcess_Delivery_Code, Company_IdNo, ClothProcess_Delivery_No          ,             for_OrderBy           ,                      ClothProcess_Delivery_Date                 ,   Ledger_IdNo     ,  Purchase_OrderNo  , Transport_IdNo  , Freight_Charges         ,Total_Pcs            ,Total_Qty            , Total_Meters            , Total_Weight , Processing_Idno                                                                                                                   ,    Party_Dc_No       ,           JobOrder_No         )" &
                                                                       " Values ('" & Trim(Pk_Condition2) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_RecNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_RecNo.Text))) & ", @RecDate, " & Str(Val(Del_Id)) & ", '" & Trim(txt_PartyDcNo.Text) & "', " & Str(Val(Tr_ID)) & ", " & Str(Val(txt_Frieght.Text)) & ",  " & Str(Val(vTotPcs)) & "," & Str(Val(vtotqty)) & " , " & Str(Val(vTotMtrs)) & ", " & Str(Val(vTotWeight)) & " ,  " & Str(Val(vNXT_Proc_idno)) & " ,'" & Trim(txt_PartyDcNo.Text) & "','" & Trim(txt_JobNo.Text) & "'  )"
                cmd.ExecuteNonQuery()

                With dgv_Details
                    Sno = 0
                    For i = 0 To .RowCount - 1

                        If Val(.Rows(i).Cells(dgvCol_Details.METERS).Value) <> 0 Or Len(.Rows(i).Cells(dgvCol_Details.Process_Delv_Code).Value) <> 0 Or Val(.Rows(i).Cells(dgvCol_Details.Process_Delv_Slno).Value) <> 0 Then

                            Sno = Sno + 1
                            'Itgry_ID = Common_Procedures.Cloth_NameToIdNo(con, .Rows(i).Cells(dgvCol_Details.DC_NO).Value, tr)
                            Itfp_ID = Common_Procedures.Cloth_NameToIdNo(con, .Rows(i).Cells(dgvCol_Details.ITEM_RECEIVED).Value, tr)
                            Col_ID = Common_Procedures.Colour_NameToIdNo(con, .Rows(i).Cells(dgvCol_Details.COLOUR).Value, tr)

                            'Lot_ID = Common_Procedures.get_FieldValue(con, "FabricPurchase_Weaver_Lot_Head", "FabricPurchase_Weaver_Lot_IdNo", "FabricPurchase_Weaver_Lot_Code_forSelection = '" & .Rows(i).Cells(dgvCol_Details.LOT_NO).Value & "'",, tr)  'Common_Procedures.Lot_NoToIdNo(con, .Rows(i).Cells(dgvCol_Details.PROCESSING ).Value, tr)

                            Lot_ID = Common_Procedures.Lot_NoToIdNo(con, .Rows(i).Cells(dgvCol_Details.LOT_NO).Value, tr)  'Common_Procedures.Lot_NoToIdNo(con, .Rows(i).Cells(dgvCol_Details.PROCESSING ).Value, tr)

                            Proc_ID = Common_Procedures.Process_NameToIdNo(con, .Rows(i).Cells(dgvCol_Details.PROCESSING).Value, tr)

                            ItmfpID_aftr_nxtprocs = 0
                            If vNXT_Proc_idno <> 0 Then
                                ItmfpID_aftr_nxtprocs = Common_Procedures.Cloth_NameToIdNo(con, .Rows(i).Cells(dgvCol_Details.ITEM_after_NextProcess).Value, tr)
                                If ItmfpID_aftr_nxtprocs = 0 Then
                                    ItmfpID_aftr_nxtprocs = Itfp_ID
                                End If
                            End If

                            Sno = Sno + 1

                            Nr = 0
                            cmd.CommandText = "Update  Textile_Processing_Delivery_Details set Cloth_Processing_Delivery_Date = @RecDate , Ledger_IdNo = " & Str(Val(Led_ID)) & ", Sl_No  = " & Str(Val(Sno)) & " , Item_Idno = " & Str(Val(Itfp_ID)) & "  , Item_To_Idno = " & Str(Val(ItmfpID_aftr_nxtprocs)) & " , Colour_Idno = " & Val(Col_ID) & " , Lot_IdNo = " & Val(Lot_ID) & " ,   Folding =  " & Val(.Rows(i).Cells(dgvCol_Details.FOLDING).Value).ToString & ",FabricPurchase_Weaver_Lot_IdNo = " & Val(Lot_ID) & " ,Processing_Idno = " & Val(vNXT_Proc_idno) & " ,   Delivery_Pcs =  " & Val(.Rows(i).Cells(dgvCol_Details.PCS).Value) & ", Delivery_Qty = " & Val(.Rows(i).Cells(dgvCol_Details.QTY).Value) & " ,  Meter_Qty = 0 ,    Delivery_Meters = " & Str(Val(.Rows(i).Cells(dgvCol_Details.METERS).Value)) & " ,    Delivery_Weight = " & Str(Val(.Rows(i).Cells(dgvCol_Details.WEIGHT).Value)) & "  where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Cloth_Processing_Delivery_code = '" & Trim(Pk_Condition2) & Trim(NewCode) & "'  and Cloth_Processing_Delivery_Slno = " & Val(.Rows(i).Cells(dgvCol_Details.Process_Delv_Slno).Value)
                            Nr = cmd.ExecuteNonQuery()
                            If Nr = 0 Then
                                cmd.CommandText = "Insert into Textile_Processing_Delivery_Details (          Cloth_Processing_Delivery_Code      ,                  Company_IdNo    ,  Cloth_Processing_Delivery_No ,                               for_OrderBy                              , Cloth_Processing_Delivery_Date,            Sl_No     ,            Ledger_IdNo   ,           Item_Idno       ,           Item_To_Idno                 ,       Colour_Idno  ,        Lot_IdNo     , FabricPurchase_Weaver_Lot_IdNo,         Processing_Idno     ,                      Delivery_Pcs                         ,                       Delivery_Qty                                  , Meter_Qty,                  Delivery_Meters                        ,                      Delivery_Weight                         ,                   Folding                                           ,                Cloth_Processing_Delivery_Slno                      ) " &
                                                    "           Values                             ( '" & Trim(Pk_Condition2) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_RecNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_RecNo.Text))) & ",        @RecDate               , " & Str(Val(Sno)) & ", " & Str(Val(Del_Id)) & " , " & Str(Val(Itfp_ID)) & " , " & Str(Val(ItmfpID_aftr_nxtprocs)) & ", " & Val(Col_ID) & ", " & Val(Lot_ID) & " ,         " & Val(Lot_ID) & "   , " & Val(vNXT_Proc_idno) & " , " & Str(Val(.Rows(i).Cells(dgvCol_Details.PCS).Value)) & ", " & Val(Trim(.Rows(i).Cells(dgvCol_Details.QTY).Value)).ToString & ",      0   , " & Val(.Rows(i).Cells(dgvCol_Details.METERS).Value) & ", " & Str(Val(.Rows(i).Cells(dgvCol_Details.WEIGHT).Value)) & ",  " & Val(.Rows(i).Cells(dgvCol_Details.FOLDING).Value).ToString & " ," & Val(.Rows(i).Cells(dgvCol_Details.Process_Delv_Slno).Value) & " ) "
                                cmd.ExecuteNonQuery()
                            End If


                        End If



                    Next

                End With

            Else


                Partcls = "Delv : Dc.No. " & Trim(lbl_RecNo.Text)
                PBlNo = Trim(lbl_RecNo.Text)
                EntID = Trim(Pk_Condition2) & Trim(lbl_RecNo.Text)


                Dim vSTOCK_POSTING_QTY = ""
                vSTOCK_POSTING_QTY = 0

                With dgv_Details

                    Sno = 0
                    For i = 0 To .RowCount - 1

                        If Val(.Rows(i).Cells(dgvCol_Details.METERS).Value) <> 0 Or Len(.Rows(i).Cells(dgvCol_Details.Process_Delv_Code).Value) <> 0 Or Val(.Rows(i).Cells(dgvCol_Details.Process_Delv_Slno).Value) <> 0 Then

                            Sno = Sno + 1
                            'Itgry_ID = Common_Procedures.Cloth_NameToIdNo(con, .Rows(i).Cells(dgvCol_Details.DC_NO).Value, tr)
                            Itfp_ID = Common_Procedures.Cloth_NameToIdNo(con, .Rows(i).Cells(dgvCol_Details.ITEM_RECEIVED).Value, tr)
                            Col_ID = Common_Procedures.Colour_NameToIdNo(con, .Rows(i).Cells(dgvCol_Details.COLOUR).Value, tr)

                            Lot_ID = Common_Procedures.Lot_NoToIdNo(con, .Rows(i).Cells(dgvCol_Details.LOT_NO).Value, tr)  'Common_Procedures.Lot_NoToIdNo(con, .Rows(i).Cells(dgvCol_Details.PROCESSING ).Value, tr)

                            Proc_ID = Common_Procedures.Process_NameToIdNo(con, .Rows(i).Cells(dgvCol_Details.PROCESSING).Value, tr)

                            Sno = Sno + 1

                            If Common_Procedures.settings.CustomerCode = "1490" Then '---- LAKSHMI SARASWATHI EXPORTS (THIRUCHENCODE)
                                vFIELDNAME = "UnChecked_Meters"
                            Else
                                vFIELDNAME = "Meters_Type1"
                            End If

                            ' ----------- CODE BY GOPI 2025-02-03 ' --- FOR SOTEXPA

                            vCLOSTK_IN = ""

                            Da4 = New SqlClient.SqlDataAdapter("Select Stock_In from Cloth_Head Where Cloth_Idno = " & Val(Itfp_ID) & "", con)
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
                                vSTOCK_POSTING_QTY = Str(Val(.Rows(i).Cells(dgvCol_Details.PCS).Value))
                            Else
                                vSTOCK_POSTING_QTY = Str(Val(.Rows(i).Cells(dgvCol_Details.METERS).Value))
                            End If


                            Nr = 0
                            cmd.CommandText = "Insert into Stock_Cloth_Processing_Details ( Reference_Code                     ,             Company_IdNo         ,           Reference_No       ,                               for_OrderBy                               , Reference_Date,     DeliveryTo_Idno      ,   ReceivedFrom_Idno    ,   Entry_ID           ,  Party_Bill_No             ,  Particulars               ,           Sl_No        ,           Cloth_Idno      ,   Folding                                                      ,                 " & vFIELDNAME & "   ,   StockOff_IdNo                 ,Weight                                                       , Pcs                                                       ,Colour_IdNo        ,Process_IdNo        ,Lot_IdNo                ,Direct_Process_Delivery_Purpose_IdNo , ClothSales_OrderCode_forSelection ,   Weight_Type1         ) " &
                                           " Values                                   ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_RecNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_RecNo.Text))) & ",  @RecDate     ,  " & Str(Val(Del_Id)) & "," & Str(Val(Led_ID)) & ", '" & Trim(EntID) & "',       '" & Trim(PBlNo) & "',     '" & Trim(Partcls) & "', " & Str(Val(Sno)) & " , " & Str(Val(Itfp_ID)) & " ," & Str(Val(.Rows(i).Cells(dgvCol_Details.FOLDING).Value)) & ", " & Str(Val(vSTOCK_POSTING_QTY)) & " , " & StkOff_ID.ToString & "     ," & Str(Val(.Rows(i).Cells(dgvCol_Details.WEIGHT).Value)) & "," & Str(Val(.Rows(i).Cells(dgvCol_Details.PCS).Value)) & "," & Str(Col_ID) & "," & Str(Proc_ID) & "," & Lot_ID.ToString & ", " & vNXT_Proc_idno.ToString & " , '" & Trim(cbo_ClothSales_OrderCode_forSelection.Text) & "' ," & Str(Val(.Rows(i).Cells(dgvCol_Details.WEIGHT).Value)) & ") "
                            cmd.ExecuteNonQuery()

                            ' ----------- COMMAND BY GOPI 2025-02-03 ' --- FOR SOTEXPA

                            'cmd.CommandText = "Insert into Stock_Cloth_Processing_Details ( Reference_Code                     ,             Company_IdNo         ,           Reference_No       ,                               for_OrderBy                               , Reference_Date,     DeliveryTo_Idno      ,   ReceivedFrom_Idno    ,   Entry_ID           ,  Party_Bill_No             ,  Particulars               ,           Sl_No        ,           Cloth_Idno      ,   Folding                                                      ,   " & vFIELDNAME & "                                          ,StockOff_IdNo                 ,Weight                                                       ,Pcs                                                       ,Colour_IdNo        ,Process_IdNo        ,Lot_IdNo                ,Direct_Process_Delivery_Purpose_IdNo , ClothSales_OrderCode_forSelection ,   Weight_Type1         ) " &
                            '               " Values                                   ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_RecNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_RecNo.Text))) & ",  @RecDate     ,  " & Str(Val(Del_Id)) & "," & Str(Val(Led_ID)) & ", '" & Trim(EntID) & "',       '" & Trim(PBlNo) & "',     '" & Trim(Partcls) & "', " & Str(Val(Sno)) & " , " & Str(Val(Itfp_ID)) & " ," & Str(Val(.Rows(i).Cells(dgvCol_Details.FOLDING).Value)) & ", " & Str(Val(.Rows(i).Cells(dgvCol_Details.METERS).Value)) & ", " & StkOff_ID.ToString & "     ," & Str(Val(.Rows(i).Cells(dgvCol_Details.WEIGHT).Value)) & "," & Str(Val(.Rows(i).Cells(dgvCol_Details.PCS).Value)) & "," & Str(Col_ID) & "," & Str(Proc_ID) & "," & Lot_ID.ToString & ", " & vNXT_Proc_idno.ToString & " , '" & Trim(cbo_ClothSales_OrderCode_forSelection.Text) & "' ," & Str(Val(.Rows(i).Cells(dgvCol_Details.WEIGHT).Value)) & ") "
                            'cmd.ExecuteNonQuery()

                            ''cmd.CommandText = "Insert into Stock_Cloth_Processing_Details ( Reference_Code                     ,             Company_IdNo         ,           Reference_No       ,                               for_OrderBy                               , Reference_Date,     DeliveryTo_Idno      ,   ReceivedFrom_Idno    ,   Entry_ID           ,  Party_Bill_No             ,  Particulars               ,           Sl_No        ,           Cloth_Idno      ,   Folding                                                      ,   " & vFIELDNAME & "                                          ,StockOff_IdNo                 ,Weight                                                       ,Pcs                                                       ,Colour_IdNo        ,Process_IdNo        ,Lot_IdNo                ,Direct_Process_Delivery_Purpose_IdNo , ClothSales_OrderCode_forSelection) " &
                            ''               " Values                                   ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_RecNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_RecNo.Text))) & ",  @RecDate     ,  " & Str(Val(Del_Id)) & "," & Str(Val(Led_ID)) & ", '" & Trim(EntID) & "',       '" & Trim(PBlNo) & "',     '" & Trim(Partcls) & "', " & Str(Val(Sno)) & " , " & Str(Val(Itfp_ID)) & " ," & Str(Val(.Rows(i).Cells(dgvCol_Details.FOLDING).Value)) & ", " & Str(Val(.Rows(i).Cells(dgvCol_Details.METERS).Value)) & ", " & StkOff_ID.ToString & "     ," & Str(Val(.Rows(i).Cells(dgvCol_Details.WEIGHT).Value)) & "," & Str(Val(.Rows(i).Cells(dgvCol_Details.PCS).Value)) & "," & Str(Col_ID) & "," & Str(Proc_ID) & "," & Lot_ID.ToString & ", " & vNXT_Proc_idno.ToString & " , '" & Trim(cbo_ClothSales_OrderCode_forSelection.Text) & "') "
                            ''cmd.ExecuteNonQuery()

                        End If

                    Next

                End With

            End If

            tr.Commit()

            Dt1.Dispose()
            Da.Dispose()

            If New_Entry = True Then
                new_record()
            Else
                move_record(lbl_RecNo.Text)
            End If

            MessageBox.Show("Saved Successfully!!!", "FOR SAVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Information)

        Catch ex As Exception

            tr.Rollback()
            MessageBox.Show(ex.Message, "FOR SAVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

        If dtp_Date.Enabled And dtp_Date.Visible Then dtp_Date.Focus()

    End Sub


    Private Sub Total_Calculation()
        Dim vTotPcs As Single, vTotMtrs As Single, vtotweight As Single, vtotqty As Single, vExcsrt As Single, vTotDelvMtr As Single
        Dim i As Integer
        Dim sno As Integer
        Dim vTot_Exsrt_Wgt = ""

        If FrmLdSTS = True Then Exit Sub

        vTotPcs = 0 : vTotMtrs = 0 : vtotweight = 0 : sno = 0 : vExcsrt = 0 : vTotDelvMtr = 0 : vTot_Exsrt_Wgt = 0
        With dgv_Details
            For i = 0 To dgv_Details.Rows.Count - 1

                sno = sno + 1

                .Rows(i).Cells(dgvCol_Details.SLNO).Value = sno

                If Val(dgv_Details.Rows(i).Cells(dgvCol_Details.PCS).Value) <> 0 Or Val(dgv_Details.Rows(i).Cells(dgvCol_Details.QTY).Value) <> 0 Or Val(dgv_Details.Rows(i).Cells(dgvCol_Details.METERS).Value) <> 0 Or Val(dgv_Details.Rows(i).Cells(dgvCol_Details.WEIGHT).Value) <> 0 Or Val(dgv_Details.Rows(i).Cells(dgvCol_Details.EXC_SHT_Mtr).Value) <> 0 Then
                    '.Rows(i).Cells(dgvCol_Details.WEIGHT).Value = Val(dgv_Details.Rows(i).Cells(dgvCol_Details.QTY ).Value) * Val(dgv_Details.Rows(i).Cells(dgvCol_Details.METERS ).Value)

                    vTotPcs = vTotPcs + Val(dgv_Details.Rows(i).Cells(dgvCol_Details.PCS).Value)
                    vtotqty = vtotqty + Val(dgv_Details.Rows(i).Cells(dgvCol_Details.QTY).Value)
                    vTotMtrs = vTotMtrs + Val(dgv_Details.Rows(i).Cells(dgvCol_Details.METERS).Value)
                    vtotweight = vtotweight + Val(dgv_Details.Rows(i).Cells(dgvCol_Details.WEIGHT).Value)
                    vExcsrt = vExcsrt + Val(dgv_Details.Rows(i).Cells(dgvCol_Details.EXC_SHT_Mtr).Value)
                    vTotDelvMtr = vTotDelvMtr + Val(dgv_Details.Rows(i).Cells(dgvCol_Details.Delivery_Meter).Value)

                    vTot_Exsrt_Wgt = vTot_Exsrt_Wgt + Val(dgv_Details.Rows(i).Cells(dgvCol_Details.EXC_SHT_Wgt).Value)

                End If
            Next
        End With
        If dgv_Details_Total.Rows.Count <= 0 Then dgv_Details_Total.Rows.Add()
        dgv_Details_Total.Rows(0).Cells(dgvCol_Details.PCS).Value = Val(vTotPcs)
        dgv_Details_Total.Rows(0).Cells(dgvCol_Details.QTY).Value = Val(vtotqty)
        dgv_Details_Total.Rows(0).Cells(dgvCol_Details.METERS).Value = Format(Val(vTotMtrs), "#########0.00")
        dgv_Details_Total.Rows(0).Cells(dgvCol_Details.WEIGHT).Value = Format(Val(vtotweight), "#########0.000")
        dgv_Details_Total.Rows(0).Cells(dgvCol_Details.EXC_SHT_Mtr).Value = Format(Val(vExcsrt), "#########0.00")
        dgv_Details_Total.Rows(0).Cells(dgvCol_Details.Delivery_Meter).Value = Format(Val(vTotDelvMtr), "#########0.00")

        dgv_Details_Total.Rows(0).Cells(dgvCol_Details.EXC_SHT_Wgt).Value = Format(Val(vTot_Exsrt_Wgt), "#########0.00")


        If Val(txt_ReceiptMeter.Text) <> 0 Then
            txt_DiffMeter.Text = txt_ReceiptMeter.Text - Format(Val(vTotMtrs), "#########0.00")
        End If

    End Sub
    Private Sub cbo_Ledger_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Ledger.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'GODOWN' or ( Ledger_Type = '' AND (AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14) ))", "(Ledger_idno = 0)")
    End Sub
    Private Sub cbo_Ledger_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Ledger.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Ledger, cbo_Receipt_Type, txt_PartyDcNo, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'GODOWN' or ( Ledger_Type = '' AND (AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14) ))", "(Ledger_idno = 0)")
    End Sub

    Private Sub cbo_Ledger_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Ledger.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Ledger, Nothing, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'GODOWN' or ( Ledger_Type = '' AND (AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14) )) ", "(Ledger_idno = 0)")
        If Asc(e.KeyChar) = 13 Then
            If btn_Selection.Enabled = False Then
                txt_PartyDcNo.Focus()
                Exit Sub

            Else

                If MessageBox.Show("Do you want to select from Delivery :", "FOR DELIVERY SELECTION...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = DialogResult.Yes Then
                    btn_Selection_Click(sender, e)

                Else
                    txt_PartyDcNo.Focus()

                End If

            End If

        End If
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
            If e.ColumnIndex = dgvCol_Details.QTY Then
                Show_Item_CurrentStock(e.RowIndex)
                .Focus()
            End If
        End With

    End Sub
    Private Sub dgv_Details_CellEndEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellEndEdit
        With dgv_Details

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
            Total_Calculation()

        End With
    End Sub

    Private Sub dgv_Details_CellEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellEnter
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim Dt2 As New DataTable
        Dim Dt3 As New DataTable
        Dim Dt4 As New DataTable
        Dim Dt5 As New DataTable
        Dim rect As Rectangle

        If FrmLdSTS = True Then Exit Sub

        With dgv_Details


            If Val(.Rows(e.RowIndex).Cells(dgvCol_Details.Cloth_Processing_Receipt_Slno).Value) = 0 Then
                Set_Max_DetailsSlNo(e.RowIndex, dgvCol_Details.Cloth_Processing_Receipt_Slno)
            End If

            If Val(.CurrentRow.Cells(dgvCol_Details.SLNO).Value) = 0 Then
                .CurrentRow.Cells(dgvCol_Details.SLNO).Value = .CurrentRow.Index + 1
            End If

            If e.ColumnIndex = dgvCol_Details.ITEM_RECEIVED Then

                If (cbo_GRID_item_received.Visible = False Or Val(cbo_GRID_item_received.Tag) <> e.RowIndex) Then

                    cbo_GRID_item_received.Tag = -1
                    Da = New SqlClient.SqlDataAdapter("select Cloth_Name from Cloth_Head where (Cloth_Type = 'PROCESSED FABRIC') or Cloth_Name = '' order by Cloth_Name", con)
                    Dt1 = New DataTable
                    Da.Fill(Dt1)
                    cbo_GRID_item_received.DataSource = Dt1
                    cbo_GRID_item_received.DisplayMember = "Cloth_Name"

                    rect = .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False)

                    cbo_GRID_item_received.Left = .Left + rect.Left  '  .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Left
                    cbo_GRID_item_received.Top = .Top + rect.Top  ' .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Top
                    cbo_GRID_item_received.Width = rect.Width  ' .CurrentCell.Size.Width
                    cbo_GRID_item_received.Height = rect.Height  ' rect.Height

                    cbo_GRID_item_received.Text = .CurrentCell.Value  '  Trim(.CurrentRow.Cells(e.ColumnIndex).Value)

                    cbo_GRID_item_received.Tag = Val(e.RowIndex)
                    cbo_GRID_item_received.Visible = True

                    cbo_GRID_item_received.BringToFront()
                    cbo_GRID_item_received.Focus()


                End If

            Else
                cbo_GRID_item_received.Visible = False

            End If

            If e.ColumnIndex = dgvCol_Details.COLOUR Then

                If (cbo_GRID_Colour.Visible = False Or Val(cbo_GRID_Colour.Tag) <> e.RowIndex) Then

                    cbo_GRID_Colour.Tag = -1
                    Da = New SqlClient.SqlDataAdapter("select Colour_Name from Colour_Head order by Colour_Name", con)
                    Dt2 = New DataTable
                    Da.Fill(Dt2)
                    cbo_GRID_Colour.DataSource = Dt2
                    cbo_GRID_Colour.DisplayMember = "Colour_Name"

                    rect = .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False)

                    cbo_GRID_Colour.Left = .Left + rect.Left
                    cbo_GRID_Colour.Top = .Top + rect.Top
                    cbo_GRID_Colour.Width = rect.Width
                    cbo_GRID_Colour.Height = rect.Height

                    cbo_GRID_Colour.Text = .CurrentCell.Value

                    cbo_GRID_Colour.Tag = Val(e.RowIndex)
                    cbo_GRID_Colour.Visible = True

                    cbo_GRID_Colour.BringToFront()
                    cbo_GRID_Colour.Focus()

                Else
                    cbo_GRID_Colour.Visible = False

                End If

            Else
                cbo_GRID_Colour.Visible = False

            End If

            'If e.ColumnIndex = dgvCol_Details.PROCESSING Then

            '    If (cbo_GRID_Processing.Visible = False Or Val(cbo_GRID_Processing.Tag) <> e.RowIndex) Then

            '        cbo_GRID_Processing.Tag = -1

            '        Da = New SqlClient.SqlDataAdapter("select Process_Name from Process_Head order by Process_Name", con)
            '        Dt3 = New DataTable
            '        Da.Fill(Dt3)

            '        cbo_GRID_Processing.DataSource = Dt3
            '        cbo_GRID_Processing.DisplayMember = "Process_Name"

            '        rect = .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False)

            '        cbo_GRID_Processing.Left = .Left + rect.Left  '  .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Left
            '        cbo_GRID_Processing.Top = .Top + rect.Top  ' .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Top
            '        cbo_GRID_Processing.Width = rect.Width  ' .CurrentCell.Size.Width
            '        cbo_GRID_Processing.Height = rect.Height  ' rect.Height

            '        cbo_GRID_Processing.Text = .CurrentCell.Value  '  Trim(.CurrentRow.Cells(e.ColumnIndex).Value)

            '        cbo_GRID_Processing.Tag = Val(e.RowIndex)
            '        cbo_GRID_Processing.Visible = True

            '        cbo_GRID_Processing.BringToFront()
            '        cbo_GRID_Processing.Focus()

            '    End If

            'Else

            '    cbo_GRID_Processing.Visible = False

            'End If

            'If e.ColumnIndex = dgvCol_Details.LOT_NO Then

            '    If (cbo_GRID_LotNo.Visible = False Or Val(cbo_GRID_LotNo.Tag) <> e.RowIndex) Then

            '        cbo_GRID_LotNo.Tag = -1
            '        Da = New SqlClient.SqlDataAdapter("select Lot_No from Lot_Head order by Lot_No", con)
            '        Dt4 = New DataTable
            '        Da.Fill(Dt4)
            '        cbo_GRID_LotNo.DataSource = Dt4
            '        cbo_GRID_LotNo.DisplayMember = "Lot_No"

            '        rect = .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False)

            '        cbo_GRID_LotNo.Left = .Left + rect.Left  '  .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Left
            '        cbo_GRID_LotNo.Top = .Top + rect.Top  ' .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Top
            '        cbo_GRID_LotNo.Width = rect.Width  ' .CurrentCell.Size.Width
            '        cbo_GRID_LotNo.Height = rect.Height  ' rect.Height

            '        cbo_GRID_LotNo.Text = .CurrentCell.Value  '  Trim(.CurrentRow.Cells(e.ColumnIndex).Value)

            '        cbo_GRID_LotNo.Tag = Val(e.RowIndex)
            '        cbo_GRID_LotNo.Visible = True

            '        cbo_GRID_LotNo.BringToFront()
            '        cbo_GRID_LotNo.Focus()

            '    End If

            'Else

            '    cbo_GRID_LotNo.Visible = False

            'End If

            If e.ColumnIndex = dgvCol_Details.ITEM_after_NextProcess Then

                If (cbo_GRID_item_after_NextProcess.Visible = False Or Val(cbo_GRID_item_after_NextProcess.Tag) <> e.RowIndex) And Trim(cbo_ProcessingName_of_NextProcess.Text) <> "" Then

                    cbo_GRID_item_after_NextProcess.Tag = -1
                    Da = New SqlClient.SqlDataAdapter("select Cloth_Name from Cloth_Head where (Cloth_Type = 'PROCESSED FABRIC') or Cloth_Idno = 0 order by Cloth_Name", con)
                    Dt5 = New DataTable
                    Da.Fill(Dt5)
                    cbo_GRID_item_after_NextProcess.DataSource = Dt5
                    cbo_GRID_item_after_NextProcess.DisplayMember = "Cloth_Name"

                    rect = .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False)

                    cbo_GRID_item_after_NextProcess.Left = .Left + rect.Left  '  .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Left
                    cbo_GRID_item_after_NextProcess.Top = .Top + rect.Top  ' .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Top
                    cbo_GRID_item_after_NextProcess.Width = rect.Width  ' .CurrentCell.Size.Width
                    cbo_GRID_item_after_NextProcess.Height = rect.Height  ' rect.Height

                    cbo_GRID_item_after_NextProcess.Text = .CurrentCell.Value  '  Trim(.CurrentRow.Cells(e.ColumnIndex).Value)

                    cbo_GRID_item_after_NextProcess.Tag = Val(e.RowIndex)

                    cbo_GRID_item_after_NextProcess.Visible = True

                    cbo_GRID_item_after_NextProcess.BringToFront()

                    cbo_GRID_item_after_NextProcess.Focus()

                End If

            Else
                cbo_GRID_item_after_NextProcess.Visible = False

            End If

            If e.ColumnIndex = dgvCol_Details.QTY And dgv_LevColNo <> dgvCol_Details.QTY Then
                Show_Item_CurrentStock(e.RowIndex)
                .Focus()
            End If

        End With
    End Sub

    Private Sub dgv_Details_CellLeave(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellLeave

        If FrmLdSTS = True Then Exit Sub

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
        Dim Fabric_Processing_Recons = ""
        Dim EXCESS_SHRT = ""

        On Error Resume Next

        If FrmLdSTS = True Then Exit Sub
        If IsNothing(dgv_Details.CurrentCell) Then Exit Sub

        With dgv_Details
            If .Visible Then
                If .CurrentCell.ColumnIndex = dgvCol_Details.FOLDING Or .CurrentCell.ColumnIndex = dgvCol_Details.PCS Or .CurrentCell.ColumnIndex = dgvCol_Details.METERS Or .CurrentCell.ColumnIndex = dgvCol_Details.WEIGHT Or .CurrentCell.ColumnIndex = dgvCol_Details.EXC_SHT_Mtr Or .CurrentCell.ColumnIndex = dgvCol_Details.EXC_SHT_Wgt Then

                    EXCESS_SHRT = 0

                    If dgv_Details.Columns(dgvCol_Details.EXC_SHT_Wgt).Visible Then

                        Fabric_Processing_Recons = String.Empty

                        Fabric_Processing_Reconsilation_Mtrs_Wgt(Fabric_Processing_Recons)

                        If Trim(UCase(Fabric_Processing_Recons)) = "WEIGHT" Then

                            If .Rows(e.RowIndex).Cells(dgvCol_Details.EXC_SHT_Wgt).Value <> 0 Then
                                EXCESS_SHRT = Format((Val(.Rows(e.RowIndex).Cells(dgvCol_Details.EXC_SHT_Wgt).Value) / 100), "##########0.00")
                            End If

                            .Rows(e.RowIndex).Cells(dgvCol_Details.EXC_SHT_Percentage).Value = Format(Val(EXCESS_SHRT), "##########0.00")
                        Else
                            GoTo LOOP1
                        End If

                    Else
LOOP1:
                        If .Rows(e.RowIndex).Cells(dgvCol_Details.EXC_SHT_Mtr).Value <> 0 Then
                            EXCESS_SHRT = Format((Val(.Rows(e.RowIndex).Cells(dgvCol_Details.EXC_SHT_Mtr).Value) / 100), "##########0.00")
                        End If

                        .Rows(e.RowIndex).Cells(dgvCol_Details.EXC_SHT_Percentage).Value = Format(Val(EXCESS_SHRT), "##########0.00")

                        '.Rows(e.RowIndex).Cells(dgvCol_Details.EXC_SHT_Percentage).Value = Format((Val(.Rows(e.RowIndex).Cells(dgvCol_Details.EXC_SHT_Mtr).Value) / 100), "##########0.00")

                    End If

                    Total_Calculation()
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
                    If Trim(.Rows(.CurrentCell.RowIndex).Cells(dgvCol_Details.Process_Delv_Code).Value) <> "" Then
                        e.Handled = True
                    End If
                    If Val(.Rows(.CurrentCell.RowIndex).Cells(dgvCol_Details.Process_Delv_Slno).Value) <> 0 Then
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


                If Trim(.Rows(.CurrentCell.RowIndex).Cells(dgvCol_Details.rct_Code).Value) <> "" Then
                    e.Handled = True

                ElseIf Val(.Rows(.CurrentCell.RowIndex).Cells(dgvCol_Details.Processed_Fabric_Inspection_Code).Value) <> 0 Then
                    e.Handled = True

                Else

                    If Val(dgv_Details.CurrentCell.ColumnIndex.ToString) = 6 Or Val(dgv_Details.CurrentCell.ColumnIndex.ToString) = 7 Or Val(dgv_Details.CurrentCell.ColumnIndex.ToString) = 8 Or Val(dgv_Details.CurrentCell.ColumnIndex.ToString) = 9 Or Val(dgv_Details.CurrentCell.ColumnIndex.ToString) = 10 Or Val(dgv_Details.CurrentCell.ColumnIndex.ToString) = 11 Then

                        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
                            e.Handled = True
                        End If
                    End If
                End If
            End With
        Catch ex As Exception

        End Try
    End Sub
    Private Sub dgv_Details_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_Details.KeyUp
        Dim n As Integer

        If e.Control = True And UCase(Chr(e.KeyCode)) = "D" Then

            With dgv_Details

                n = .CurrentRow.Index

                If Trim(.Rows(n).Cells(dgvCol_Details.Process_Delv_Code).Value) = "" And Val(.Rows(n).Cells(dgvCol_Details.Process_Delv_Code).Value) = 0 Then
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


    Private Sub dgv_Details_RowsAdded(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewRowsAddedEventArgs) Handles dgv_Details.RowsAdded
        Dim n As Integer
        If FrmLdSTS = True Then Exit Sub
        With dgv_Details
            If IsNothing(dgv_Details.CurrentCell) Then Exit Sub
            n = .RowCount
            .Rows(n - 1).Cells(dgvCol_Details.SLNO).Value = Val(n)

            If Val(.Rows(e.RowIndex).Cells(dgvCol_Details.Cloth_Processing_Receipt_Slno).Value) = 0 Then
                Set_Max_DetailsSlNo(e.RowIndex, dgvCol_Details.Cloth_Processing_Receipt_Slno)
            End If

        End With
    End Sub
    Private Sub cbo_GRID_LotNo_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_GRID_LotNo.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Lot_Head", "Lot_No", "", "(Lot_Idno=0)")
    End Sub

    Private Sub cbo_GRID_LotNo_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_GRID_LotNo.KeyDown

        vcbo_KeyDwnVal = e.KeyValue

        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_GRID_LotNo, cbo_GRID_Processing, Nothing, "Lot_Head", "Lot_No", "", "(Lot_Idno=0)")

        With dgv_Details

            If (e.KeyValue = 38 And cbo_GRID_LotNo.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
                .Focus()
                .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex - 1)
            End If

            If (e.KeyValue = 40 And cbo_GRID_LotNo.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
                .Focus()
                If .Columns(.CurrentCell.ColumnIndex + 1).Visible And Not .Columns(.CurrentCell.ColumnIndex + 1).ReadOnly Then
                    .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 1)
                Else
                    .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 2)
                End If
            End If

        End With

    End Sub

    Private Sub cbo_GRID_LotNo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_GRID_LotNo.KeyPress

        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_GRID_LotNo, Nothing, "Lot_Head", "Lot_No", "", "(Lot_Idno=0)")

        If Asc(e.KeyChar) = 13 Then

            With dgv_Details

                .Focus()

                If .Columns(.CurrentCell.ColumnIndex + 1).Visible And Not .Columns(.CurrentCell.ColumnIndex + 1).ReadOnly Then
                    .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 1)
                Else
                    .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 2)
                End If

            End With

        End If

    End Sub


    Private Sub cbo_GRID_LotNo_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_GRID_LotNo.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Dim f As New LotNo_creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_GRID_LotNo.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If
    End Sub


    Private Sub cbo_GRID_LotNo_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_GRID_LotNo.TextChanged
        Try
            If cbo_GRID_LotNo.Visible Then
                With dgv_Details
                    If Val(cbo_GRID_LotNo.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = dgvCol_Details.LOT_NO Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_GRID_LotNo.Text)
                    End If
                End With
            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub cbo_GRID_Colour_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_GRID_Colour.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "colour_Head", "colour_Name", "", "(Colour_IdNo = 0)")
    End Sub
    Private Sub cbo_GRID_Colour_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_GRID_Colour.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_GRID_Colour, cbo_GRID_item_received, cbo_GRID_Processing, "colour_Head", "colour_Name", "", "(Colour_IdNo = 0)")
        With dgv_Details

            If (e.KeyValue = 38 And cbo_GRID_Colour.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
                .Focus()
                .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex - 1)
            End If

            If (e.KeyValue = 40 And cbo_GRID_Colour.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
                .Focus()
                .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 1)
            End If

        End With
    End Sub

    Private Sub cbo_GRID_Colour_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_GRID_Colour.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_GRID_Colour, cbo_GRID_Processing, "colour_Head", "colour_Name", "", "(Colour_IdNo = 0)")
        If Asc(e.KeyChar) = 13 Then
            With dgv_Details
                .Focus()
                .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)
            End With
        End If
    End Sub


    Private Sub cbo_GRID_Colour_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_GRID_Colour.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Dim f As New Color_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_GRID_Colour.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If
    End Sub


    Private Sub cbo_GRID_Colour_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_GRID_Colour.TextChanged
        Try
            If cbo_GRID_Colour.Visible Then
                With dgv_Details
                    If Val(cbo_GRID_Colour.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = dgvCol_Details.COLOUR Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_GRID_Colour.Text)
                    End If
                End With
            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try
    End Sub
    Private Sub cbo_GRID_Processing_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_GRID_Processing.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Process_Head", "Process_Name", "", "(Process_Idno=0)")

    End Sub
    Private Sub cbo_GRID_Processing_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_GRID_Processing.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_GRID_Processing, cbo_GRID_Colour, cbo_GRID_LotNo, "Process_Head", "Process_Name", "", "(Process_Idno=0)")

        With dgv_Details

            If (e.KeyValue = 38 And cbo_GRID_Processing.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
                .Focus()
                .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex - 1)
            End If

            If (e.KeyValue = 40 And cbo_GRID_Processing.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
                .Focus()
                .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 1)
            End If

        End With
    End Sub

    Private Sub cbo_GRID_Processing_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_GRID_Processing.KeyPress

        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_GRID_Processing, cbo_GRID_LotNo, "Process_Head", "Process_Name", "", "(Process_Idno=0)")
        If Asc(e.KeyChar) = 13 Then

            With dgv_Details

                .Focus()
                .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)

            End With

        End If
    End Sub

    Private Sub cbo_GRID_Processing_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_GRID_Processing.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Dim f As New Process_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_GRID_Processing.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If
    End Sub


    Private Sub cbo_GRID_Processing_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_GRID_Processing.TextChanged
        Try
            If cbo_GRID_Processing.Visible Then
                With dgv_Details
                    If Val(cbo_GRID_Processing.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = dgvCol_Details.PROCESSING Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_GRID_Processing.Text)
                    End If
                End With
            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub cbo_GRID_item_received_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_GRID_item_received.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Cloth_Head", "Cloth_Name", "(Cloth_Type = 'PROCESSED FABRIC')", "(Cloth_Idno = 0)")
    End Sub

    Private Sub cbo_GRID_item_received_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_GRID_item_received.KeyDown

        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_GRID_item_received, Nothing, cbo_GRID_Colour, "Cloth_Head", "Cloth_Name", "(Cloth_Type = 'PROCESSED FABRIC')", "(Cloth_Idno = 0)")

        With dgv_Details

            If (e.KeyValue = 38 And cbo_GRID_item_received.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
                .Focus()
                .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex - 1)
            End If

            If (e.KeyValue = 40 And cbo_GRID_item_received.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
                .Focus()
                .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 1)
            End If

        End With

    End Sub

    Private Sub cbo_GRID_item_received_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_GRID_item_received.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_GRID_item_received, cbo_GRID_Colour, "Cloth_Head", "Cloth_Name", "(Cloth_Type = 'PROCESSED FABRIC')", "(Cloth_Idno = 0)")
        If Asc(e.KeyChar) = 13 Then

            With dgv_Details

                .Focus()
                .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)

            End With

        End If
    End Sub

    Private Sub cbo_GRID_item_received_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_GRID_item_received.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Dim f As New Cloth_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_GRID_item_received.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If
    End Sub

    Private Sub cbo_GRID_item_received_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_GRID_item_received.TextChanged
        Try
            If cbo_GRID_item_received.Visible Then
                With dgv_Details
                    If Val(cbo_GRID_item_received.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = dgvCol_Details.ITEM_RECEIVED Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_GRID_item_received.Text)
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
                    dgv_Filter_Details.Rows(n).Cells(dgvCol_Details.ITEM_RECEIVED).Value = dt2.Rows(i).Item("Ledger_Name").ToString
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

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RecNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Try

            da1 = New SqlClient.SqlDataAdapter("select * from Textile_Processing_Receipt_Head Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and ClothProcess_Receipt_Code = '" & Trim(NewCode) & "'", con)
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


        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RecNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

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
                MessageBox.Show("This is New Entry", "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

            End If

            da1.Dispose()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

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

            MessageBox.Show(ex.Message, "DOES NOT PRINT", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

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

            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub txt_Frieght_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Frieght.KeyPress

        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
            e.Handled = True
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
        Dim nr As Single = 0


        ' START *********** METER OR WEIGHT TEMP FIELDS UPDATION  *****************'


        Dim Cmd As New SqlClient.SqlCommand

        Cmd.Connection = con

        '--Textile_Processing_Delivery_Details
        Cmd.CommandText = "update  a set Selection_Receipt_Meters_or_Weight_Temp = ( CASE WHEN b.Fabric_Processing_Reconsilation_In_Meter_Weight = 'WEIGHT' THEN a.Receipt_Weight else a.Receipt_Meters end) " &
                " , Selection_Return_Meters_or_Weight_Temp = ( CASE WHEN b.Fabric_Processing_Reconsilation_In_Meter_Weight = 'WEIGHT' THEN a.Return_Weight else a.Return_Meters end)  " &
                " , Selection_Delivery_Meters_or_Weight_Temp = ( CASE WHEN b.Fabric_Processing_Reconsilation_In_Meter_Weight = 'WEIGHT' THEN a.Delivery_Weight else a.Delivery_Meters end) " &
                " From Textile_Processing_Delivery_Details a INNER JOIN Cloth_Head b on a.item_Idno = b.Cloth_Idno"
        Cmd.ExecuteNonQuery()

        '--Textile_Processing_Receipt_Details
        Cmd.CommandText = "update  a set Selection_Receipt_Meters_or_Weight_Temp = ( CASE WHEN b.Fabric_Processing_Reconsilation_In_Meter_Weight = 'WEIGHT' THEN a.Receipt_Weight else a.Receipt_Meters end) " &
                " From Textile_Processing_Receipt_Details a INNER JOIN Cloth_Head b on a.item_Idno = b.Cloth_Idno "
        Cmd.ExecuteNonQuery()


        ' END *********** METER OR WEIGHT TEMP FIELDS UPDATION  *****************'

        LedIdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Ledger.Text)

        If LedIdNo = 0 Then
            MessageBox.Show("Invalid Party Name", "DOES NOT SELECT ORDER...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If cbo_Ledger.Enabled And cbo_Ledger.Visible Then cbo_Ledger.Focus()
            Exit Sub
        End If

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RecNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        CompIDCondt = "(a.company_idno = " & Str(Val(lbl_Company.Tag)) & ")"
        If Trim(UCase(Common_Procedures.settings.CompanyName)) = "-----~~~" Then
            CompIDCondt = ""
        End If


        With dgv_Selection

            .Rows.Clear()
            SNo = 0


            If Common_Procedures.settings.Continuous_Fabric_Lot_No_for_Purchase_Weaver = 1 Then
                'If Common_Procedures.settings.CustomerCode = "1516" Then
                Da = New SqlClient.SqlDataAdapter("select a.* , b.* ,B.Folding as Fabric_Folding, b.item_Idno, e.Ledger_Name as Transportname,h.Receipt_Pcs As Ent_Pcs, h.Receipt_Meters as Ent_Mtrs, h.Receipt_Weight As Ent_Wgt, h.Receipt_Qty As Ent_Qty, g.Cloth_Name as Fp_Item_Name ,gf.Cloth_Name as Del_Item_Name, I.Lot_No AS Lot_No , j.* , k.Colour_Name from Textile_Processing_Delivery_Head a INNER JOIN Textile_Processing_Delivery_Details b ON b.Lot_Complete_status = 0 and a.ClothProcess_Delivery_Code = b.Cloth_Processing_Delivery_Code left outer JOIN Cloth_Head g ON g.Cloth_Idno = b.Item_to_IdNo   left outer JOIN Cloth_Head gf ON gf.Cloth_Idno = b.Item_IdNo LEFT OUTER JOIN Lot_Head i ON b.FabricPurchase_Weaver_Lot_IdNo = i.Lot_IdNo LEFT OUTER JOIN Process_Head J ON J.Process_IdNo = b.Processing_Idno LEFT OUTER JOIN Colour_Head k ON b.Colour_IdNo = k.Colour_IdNo LEFT OUTER JOIN Ledger_Head e ON a.Transport_IdNo = e.Ledger_IdNo LEFT OUTER JOIN Textile_Processing_Receipt_Details h ON h.Cloth_Processing_Receipt_Code = '" & Trim(NewCode) & "' and b.Cloth_Processing_Delivery_Code = h.Cloth_Processing_Delivery_Code and b.Cloth_Processing_Delivery_SlNo = h.Cloth_Processing_Delivery_SlNo   Where   " & CompIDCondt & IIf(Trim(CompIDCondt) <> "", " and ", "") & " a.ledger_Idno = " & Str(Val(LedIdNo)) & " and ((b.Delivery_Meters - b.Receipt_Meters - b.Return_Meters) > 0 or h.Receipt_Meters > 0 ) Order by a.ClothProcess_Delivery_Date, a.for_orderby, a.ClothProcess_Delivery_No", con)
                'Else
                Da = New SqlClient.SqlDataAdapter("select a.* , b.* ,B.Folding as Fabric_Folding, b.item_Idno, e.Ledger_Name as Transportname,h.Receipt_Pcs As Ent_Pcs, h.Receipt_Meters as Ent_Mtrs, h.Receipt_Weight As Ent_Wgt, h.Receipt_Qty As Ent_Qty, g.Cloth_Name as Fp_Item_Name , I.Lot_No AS Lot_No , j.* , k.Colour_Name from Textile_Processing_Delivery_Head a INNER JOIN Textile_Processing_Delivery_Details b ON b.Lot_Complete_status = 0 and a.ClothProcess_Delivery_Code = b.Cloth_Processing_Delivery_Code INNER JOIN Cloth_Head g ON g.Cloth_Idno = b.Item_to_IdNo  LEFT OUTER JOIN Lot_Head i ON b.FabricPurchase_Weaver_Lot_IdNo = i.Lot_IdNo LEFT OUTER JOIN Process_Head J ON J.Process_IdNo = b.Processing_Idno LEFT OUTER JOIN Colour_Head k ON b.Colour_IdNo = k.Colour_IdNo LEFT OUTER JOIN Ledger_Head e ON a.Transport_IdNo = e.Ledger_IdNo LEFT OUTER JOIN Textile_Processing_Receipt_Details h ON h.Cloth_Processing_Receipt_Code = '" & Trim(NewCode) & "' and b.Cloth_Processing_Delivery_Code = h.Cloth_Processing_Delivery_Code and b.Cloth_Processing_Delivery_SlNo = h.Cloth_Processing_Delivery_SlNo   Where   " & CompIDCondt & IIf(Trim(CompIDCondt) <> "", " and ", "") & " a.ledger_Idno = " & Str(Val(LedIdNo)) & " and ((b.Delivery_Meters - b.Receipt_Meters - b.Return_Meters) > 0 or h.Receipt_Meters > 0 ) Order by a.ClothProcess_Delivery_Date, a.for_orderby, a.ClothProcess_Delivery_No", con)
                'End If
            Else

                ' --- CODE BY GOPI 2024-12-21 -- NEW

                Da = New SqlClient.SqlDataAdapter("select a.* , b.* , b.item_Idno, e.Ledger_Name as Transportname,h.Receipt_Pcs As Ent_Pcs, h.Receipt_Meters as Ent_Mtrs, h.Receipt_Weight As Ent_Wgt, h.Receipt_Qty As Ent_Qty, g.Cloth_Name as Fp_Item_Name ,g.Cloth_Name as Del_Item_Name, I.Lot_No , j.Process_Name , k.Colour_Name  from Textile_Processing_Delivery_Head a INNER JOIN Textile_Processing_Delivery_Details b ON b.Lot_Complete_status = 0 and a.ClothProcess_Delivery_Code = b.Cloth_Processing_Delivery_Code INNER JOIN Cloth_Head g ON g.Cloth_Idno = b.Item_to_IdNo  LEFT OUTER JOIN Lot_Head i ON b.Lot_IdNo = i.Lot_IdNo LEFT OUTER JOIN Process_Head J ON J.Process_IdNo = b.Processing_Idno LEFT OUTER JOIN Colour_Head k ON b.Colour_IdNo = k.Colour_IdNo LEFT OUTER JOIN Ledger_Head e ON a.Transport_IdNo = e.Ledger_IdNo LEFT OUTER JOIN Textile_Processing_Receipt_Details h ON h.Cloth_Processing_Receipt_Code = '" & Trim(NewCode) & "' and b.Cloth_Processing_Delivery_Code = h.Cloth_Processing_Delivery_Code and b.Cloth_Processing_Delivery_SlNo = h.Cloth_Processing_Delivery_SlNo   Where   " & CompIDCondt & IIf(Trim(CompIDCondt) <> "", " and ", "") & " a.ledger_Idno = " & Str(Val(LedIdNo)) & " and (((b.Selection_Delivery_Meters_or_Weight_Temp - b.Selection_Receipt_Meters_or_Weight_Temp - b.Selection_Return_Meters_or_Weight_Temp) > 0.02 and a.Lot_Close_Status = 0) or h.Selection_Receipt_Meters_or_Weight_Temp > 0 ) Order by a.ClothProcess_Delivery_Date, a.for_orderby, a.ClothProcess_Delivery_No", con)

                ' --- COMMAND BY GOPI 2024-12-21 -- OLD
                'Da = New SqlClient.SqlDataAdapter("select a.* , b.* , b.item_Idno, e.Ledger_Name as Transportname,h.Receipt_Pcs As Ent_Pcs, h.Receipt_Meters as Ent_Mtrs, h.Receipt_Weight As Ent_Wgt, h.Receipt_Qty As Ent_Qty, g.Cloth_Name as Fp_Item_Name ,g.Cloth_Name as Del_Item_Name, I.Lot_No , j.Process_Name , k.Colour_Name  from Textile_Processing_Delivery_Head a INNER JOIN Textile_Processing_Delivery_Details b ON b.Lot_Complete_status = 0 and a.ClothProcess_Delivery_Code = b.Cloth_Processing_Delivery_Code INNER JOIN Cloth_Head g ON g.Cloth_Idno = b.Item_to_IdNo  LEFT OUTER JOIN Lot_Head i ON b.Lot_IdNo = i.Lot_IdNo LEFT OUTER JOIN Process_Head J ON J.Process_IdNo = b.Processing_Idno LEFT OUTER JOIN Colour_Head k ON b.Colour_IdNo = k.Colour_IdNo LEFT OUTER JOIN Ledger_Head e ON a.Transport_IdNo = e.Ledger_IdNo LEFT OUTER JOIN Textile_Processing_Receipt_Details h ON h.Cloth_Processing_Receipt_Code = '" & Trim(NewCode) & "' and b.Cloth_Processing_Delivery_Code = h.Cloth_Processing_Delivery_Code and b.Cloth_Processing_Delivery_SlNo = h.Cloth_Processing_Delivery_SlNo   Where   " & CompIDCondt & IIf(Trim(CompIDCondt) <> "", " and ", "") & " a.ledger_Idno = " & Str(Val(LedIdNo)) & " and ((b.Delivery_Meters - b.Receipt_Meters - b.Return_Meters) > 0 or h.Receipt_Meters > 0 ) Order by a.ClothProcess_Delivery_Date, a.for_orderby, a.ClothProcess_Delivery_No", con)

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




                    SNo = SNo + 1

                    .Rows(n).Cells(0).Value = Val(SNo)
                    .Rows(n).Cells(1).Value = Dt1.Rows(i).Item("Cloth_Processing_Delivery_No").ToString
                    .Rows(n).Cells(2).Value = Format(Convert.ToDateTime(Dt1.Rows(i).Item("Cloth_Processing_Delivery_Date").ToString), "dd-MM-yyyy")

                    .Rows(n).Cells(4).Value = Dt1.Rows(i).Item("Colour_Name").ToString
                    .Rows(n).Cells(5).Value = Dt1.Rows(i).Item("Process_Name").ToString

                    Dim Process_Inputs_Tmp As String = ""
                    Dim Process_Outputs_Tmp As String = ""

                    'If Not IsDBNull(Dt1.Rows(0).Item("Cloth_Delivered")) Then
                    '    Process_Inputs_Tmp = IIf(Dt1.Rows(0).Item("Cloth_Delivered") = True, "1", "0")
                    'Else
                    '    Process_Inputs_Tmp = "0"
                    'End If

                    'If Not IsDBNull(Dt1.Rows(0).Item("FP_Delivered")) Then
                    '    Process_Inputs_Tmp = Process_Inputs_Tmp + IIf(Dt1.Rows(0).Item("FP_Delivered") = True, "1", "0")
                    'Else
                    '    Process_Inputs_Tmp = Process_Inputs_Tmp + "0"
                    'End If


                    'If Not IsDBNull(Dt1.Rows(0).Item("Cloth_Returned")) Then
                    '    Process_Outputs_Tmp = IIf(Dt1.Rows(0).Item("Cloth_Returned") = True, "1", "0")
                    'Else
                    '    Process_Outputs_Tmp = "0"
                    'End If

                    'If Not IsDBNull(Dt1.Rows(0).Item("FP_Returned")) Then
                    '    Process_Outputs_Tmp = Process_Outputs_Tmp + IIf(Dt1.Rows(0).Item("FP_Returned") = True, "1", "0")
                    'Else
                    '    Process_Outputs_Tmp = Process_Outputs_Tmp + "0"
                    'End If

                    'Dim RET_TYPE As String = "CLOTH"

                    'If Len(Trim(Process_Outputs_Tmp)) > 1 Then
                    '    If Mid(Trim(Process_Outputs_Tmp), 2, 1) = "1" Then
                    '        RET_TYPE = "FP"
                    '    End If
                    'End If

                    'If RET_TYPE = "CLOTH" Then
                    .Rows(n).Cells(3).Value = Dt1.Rows(i).Item("Fp_Item_Name").ToString
                    'Else
                    '.Rows(n).Cells(3).Value = Dt1.Rows(i).Item("Del_Item_Name").ToString
                    'End If

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

                    If Not IsDBNull(Dt1.Rows(i).Item("Lot_IdNo")) Then
                        .Rows(n).Cells(13).Value = Common_Procedures.Lot_IdNoToNo(con, Dt1.Rows(i).Item("Lot_IdNo"))
                    End If
                    If Len(Trim(.Rows(n).Cells(13).Value)) = 0 Then
                        .Rows(n).Cells(13).Value = Dt1.Rows(i).Item("JobOrder_No").ToString
                    End If
                    '.Rows(n).Cells(13).Value = Dt1.Rows(i).Item("Purchase_OrderNo").ToString
                    .Rows(n).Cells(14).Value = Dt1.Rows(i).Item("Cloth_Processing_Delivery_code").ToString
                    .Rows(n).Cells(15).Value = Dt1.Rows(i).Item("Cloth_Processing_Delivery_Slno").ToString

                    .Rows(n).Cells(16).Value = Ent_Pcs
                    .Rows(n).Cells(17).Value = Ent_Qty
                    .Rows(n).Cells(18).Value = Ent_Mtrs
                    .Rows(n).Cells(19).Value = Ent_Wgt
                    .Rows(n).Cells(20).Value = Val(Dt1.Rows(i).Item("Item_Idno").ToString)     '--CELL(20)

                    .Rows(n).Cells(22).Value = Format(Val(Dt1.Rows(i).Item("Delivery_Meters").ToString), "#########0.00")
                    .Rows(n).Cells(23).Value = Trim(Dt1.Rows(i).Item("ClothSales_OrderCode_forSelection").ToString)

                    'If Not IsDBNull(Dt1.Rows(i).Item("Fabric_Folding")) Then
                    '    .Rows(n).Cells(21).Value = Val(Dt1.Rows(i).Item("Fabric_Folding")).ToString
                    'Else
                    '    .Rows(n).Cells(21).Value = "100"
                    'End If

                    ' .Rows(n).Cells(21).Value = IIf(Not IsDBNull(Dt1.Rows(i).Item("Folding")), Val(Dt1.Rows(i).Item("Folding")), "100")

                Next

            End If
            Dt1.Clear()

        End With

        pnl_Selection.Visible = True
        pnl_Back.Enabled = False
        'pnl_Back.Visible = False
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

        dgv_Details.Rows.Clear()

        For i = 0 To dgv_Selection.RowCount - 1

            If Val(dgv_Selection.Rows(i).Cells(11).Value) = 1 Then

                If txt_JobNo.Text = "" Then
                    If (dgv_Selection.Rows(i).Cells(13).Value) <> "" Then
                        txt_JobNo.Text = Trim(dgv_Selection.Rows(i).Cells(13).Value)
                    End If
                End If

                If cbo_ClothSales_OrderCode_forSelection.Text = "" Then
                    If Trim(dgv_Selection.Rows(i).Cells(23).Value) <> "" Then
                        cbo_ClothSales_OrderCode_forSelection.Text = Trim(dgv_Selection.Rows(i).Cells(23).Value)
                    End If
                End If

                n = dgv_Details.Rows.Add()
                sno = sno + 1
                dgv_Details.Rows(n).Cells(dgvCol_Details.SLNO).Value = Val(sno)
                dgv_Details.Rows(n).Cells(dgvCol_Details.DC_NO).Value = dgv_Selection.Rows(i).Cells(1).Value
                dgv_Details.Rows(n).Cells(dgvCol_Details.ITEM_RECEIVED).Value = dgv_Selection.Rows(i).Cells(3).Value
                dgv_Details.Rows(n).Cells(dgvCol_Details.COLOUR).Value = dgv_Selection.Rows(i).Cells(4).Value
                dgv_Details.Rows(n).Cells(dgvCol_Details.PROCESSING).Value = dgv_Selection.Rows(i).Cells(5).Value
                dgv_Details.Rows(n).Cells(dgvCol_Details.LOT_NO).Value = dgv_Selection.Rows(i).Cells(6).Value
                dgv_Details.Rows(n).Cells(dgvCol_Details.FOLDING).Value = Val(dgv_Selection.Rows(i).Cells(21).Value)
                dgv_Details.Rows(n).Cells(dgvCol_Details.Process_Delv_Code).Value = dgv_Selection.Rows(i).Cells(14).Value
                dgv_Details.Rows(n).Cells(dgvCol_Details.Process_Delv_Slno).Value = dgv_Selection.Rows(i).Cells(15).Value


                If Val(dgv_Selection.Rows(i).Cells(16).Value) <> 0 Then
                    dgv_Details.Rows(n).Cells(dgvCol_Details.PCS).Value = dgv_Selection.Rows(i).Cells(16).Value
                Else
                    dgv_Details.Rows(n).Cells(dgvCol_Details.PCS).Value = dgv_Selection.Rows(i).Cells(7).Value
                End If

                If Val(dgv_Selection.Rows(i).Cells(17).Value) <> 0 Then
                    dgv_Details.Rows(n).Cells(dgvCol_Details.QTY).Value = dgv_Selection.Rows(i).Cells(17).Value
                Else
                    dgv_Details.Rows(n).Cells(dgvCol_Details.QTY).Value = dgv_Selection.Rows(i).Cells(8).Value
                End If

                If Val(dgv_Selection.Rows(i).Cells(18).Value) <> 0 Then
                    dgv_Details.Rows(n).Cells(dgvCol_Details.METERS).Value = dgv_Selection.Rows(i).Cells(18).Value
                Else
                    dgv_Details.Rows(n).Cells(dgvCol_Details.METERS).Value = dgv_Selection.Rows(i).Cells(9).Value
                End If

                If Val(dgv_Selection.Rows(i).Cells(19).Value) <> 0 Then
                    dgv_Details.Rows(n).Cells(dgvCol_Details.WEIGHT).Value = dgv_Selection.Rows(i).Cells(19).Value
                Else
                    dgv_Details.Rows(n).Cells(dgvCol_Details.WEIGHT).Value = dgv_Selection.Rows(i).Cells(10).Value
                End If

                If Val(dgv_Selection.Rows(i).Cells(20).Value) <> 0 Then
                    dgv_Details.Rows(n).Cells(dgvCol_Details.ITEM_DELIVERED).Value = dgv_Selection.Rows(i).Cells(20).Value   '--CELL(16)
                Else
                    dgv_Details.Rows(n).Cells(dgvCol_Details.ITEM_DELIVERED).Value = dgv_Selection.Rows(i).Cells(20).Value
                End If

                dgv_Details.Rows(n).Cells(dgvCol_Details.Delivery_Meter).Value = dgv_Selection.Rows(i).Cells(22).Value


            End If

        Next

        Total_Calculation()

        pnl_Back.Enabled = True
        'pnl_Back.Visible = True
        pnl_Selection.Visible = False
        If txt_PartyDcNo.Enabled And txt_PartyDcNo.Visible Then txt_PartyDcNo.Focus()

    End Sub
    Private Sub Show_Item_CurrentStock(ByVal Rw As Integer)
        Dim vItemID As Integer

        If Val(Rw) < 0 Then Exit Sub

        With dgv_Details

            vItemID = Common_Procedures.Processed_Item_NameToIdNo(con, .Rows(Rw).Cells(dgvCol_Details.ITEM_RECEIVED).Value)

            If Val(vItemID) = 0 Then Exit Sub

            If Val(vItemID) <> Val(.Tag) Then
                Common_Procedures.Show_ProcessedItem_CurrentStock_Display(con, Val(lbl_Company.Tag), Val(Common_Procedures.CommonLedger.Godown_Ac), vItemID)
                .Tag = Val(Rw)
            End If

        End With

    End Sub

    Private Sub chk_LotComplete_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles chk_LotComplete.KeyDown

        If e.KeyCode = 40 Then
            'dgv_Details.Focus()
            'dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(dgvCol_Details.DC_NO)
            'dgv_Details.CurrentCell.Selected = True
            If MessageBox.Show("Do you want to save ?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = DialogResult.Yes Then
                save_record()
            Else
                dtp_Date.Focus()
            End If
        End If

        If e.KeyValue = 38 Then SendKeys.Send("+{TAB}")

    End Sub

    Private Sub chk_LotComplete_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles chk_LotComplete.KeyPress
        If Asc(e.KeyChar) = 13 Then
            'dgv_Details.Focus()
            'dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(dgvCol_Details.DC_NO)
            'dgv_Details.CurrentCell.Selected = True
            If MessageBox.Show("Do you want to save ?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = DialogResult.Yes Then
                save_record()
            Else
                dtp_Date.Focus()
            End If
        End If
    End Sub

    Private Sub cbo_DeliveryTo_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_DeliveryTo.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", " (Ledger_Type = 'GODOWN' or ( Ledger_Type = '' AND (AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14) ))", "(Ledger_idno = 0)")
        cbo_DeliveryTo.Tag = cbo_DeliveryTo.Text
    End Sub

    Private Sub cbo_DeliveryTo_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_DeliveryTo.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_DeliveryTo, cbo_Ledger, IIf(cbo_ProcessingName_of_NextProcess.Enabled, cbo_ProcessingName_of_NextProcess, cbo_TransportName), "Ledger_AlaisHead", "Ledger_DisplayName", " (Ledger_Type = 'GODOWN' or ( Ledger_Type = '' AND (AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14) ))", "(Ledger_idno = 0)")
    End Sub

    Private Sub cbo_DeliveryTo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_DeliveryTo.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_DeliveryTo, IIf(cbo_ProcessingName_of_NextProcess.Enabled, cbo_ProcessingName_of_NextProcess, cbo_TransportName), "Ledger_AlaisHead", "Ledger_DisplayName", " (Ledger_Type = 'GODOWN' or ( Ledger_Type = '' AND (AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14) )) ", "(Ledger_idno = 0)")
    End Sub
    Private Sub cbo_ProcessingName_of_NextProcess_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_ProcessingName_of_NextProcess.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Process_Head", "Process_Name", "", "(Process_Idno=0)")
    End Sub

    Private Sub cbo_ProcessingName_of_NextProcess_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_ProcessingName_of_NextProcess.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_ProcessingName_of_NextProcess, cbo_DeliveryTo, cbo_TransportName, "Process_Head", "Process_Name", "", "(Process_Idno=0)")
    End Sub

    Private Sub cbo_ProcessingName_of_NextProcess_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_ProcessingName_of_NextProcess.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_ProcessingName_of_NextProcess, cbo_TransportName, "Process_Head", "Process_Name", "", "(Process_Idno=0)")
    End Sub

    Private Sub cbo_ProcessingName_of_NextProcess_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_ProcessingName_of_NextProcess.KeyUp
        If e.Control = False And e.KeyValue = 17 Then
            Dim f As New Process_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_ProcessingName_of_NextProcess.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()
        End If
    End Sub

    Private Sub cbo_DeliveryTo_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_DeliveryTo.KeyUp
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
            MessageBox.Show(ex.Message, "DOES NOT SEND SMS...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

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

        If cbo_Receipt_Type.Text = "DELIVERY" Then
            txt_JobNo.Enabled = False
        Else
            txt_JobNo.Enabled = True
        End If

    End Sub

    Private Sub cbo_Receipt_Type_Leave(sender As Object, e As EventArgs) Handles cbo_Receipt_Type.Leave

        If Not Displaying Then
            If cbo_Receipt_Type.Tag <> cbo_Receipt_Type.Text And Len(Trim(cbo_Receipt_Type.Tag)) <> 0 Then
                If MessageBox.Show("Changing Receipt Type Will Remove All Rows in Details. Continue ?", "Receipt Type Change", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = DialogResult.No Then
                    cbo_Receipt_Type.Text = cbo_Receipt_Type.Tag
                    Exit Sub
                Else
                    dgv_Details.Rows.Clear()
                    'Exit Sub
                End If
            End If
        End If

        If Trim(UCase(cbo_Receipt_Type.Text)) = "DELIVERY" Then
            On Error Resume Next
            btn_Selection.Enabled = True
            'dgv_Details.Columns(0).ReadOnly = True
            dgv_Details.Columns(1).ReadOnly = True
            dgv_Details.Columns(2).ReadOnly = True
            dgv_Details.Columns(dgvCol_Details.PROCESSING).ReadOnly = True
            dgv_Details.Columns(dgvCol_Details.LOT_NO).ReadOnly = True
            dgv_Details.AllowUserToAddRows = False
        Else
            On Error Resume Next
            btn_Selection.Enabled = False
            'dgv_Details.Columns(0).ReadOnly = False
            dgv_Details.Columns(1).ReadOnly = False
            dgv_Details.Columns(2).ReadOnly = False
            dgv_Details.Columns(dgvCol_Details.PROCESSING).ReadOnly = False
            dgv_Details.Columns(dgvCol_Details.LOT_NO).ReadOnly = False
            dgv_Details.AllowUserToAddRows = True
        End If

        cbo_Receipt_Type.Tag = cbo_Receipt_Type.Text

    End Sub

    Private Sub cbo_Receipt_Type_Enter(sender As Object, e As EventArgs) Handles cbo_Receipt_Type.Enter

        cbo_Receipt_Type.Tag = cbo_Receipt_Type.Text

    End Sub

    Private Sub cbo_TransportName_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbo_TransportName.SelectedIndexChanged

    End Sub

    Private Sub cbo_LotNo_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbo_GRID_LotNo.SelectedIndexChanged

    End Sub

    Private Sub dgv_Selection_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgv_Selection.CellContentClick

    End Sub

    Private Sub cbo_DeliveryTo_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbo_DeliveryTo.SelectedIndexChanged

        If cbo_DeliveryTo.Tag <> cbo_DeliveryTo.Text Then
            Dim DEL_LED_TYPE As String = Common_Procedures.get_FieldValue(con, "Ledger_Head", "Ledger_Type", "Ledger_IdNo = " & Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_DeliveryTo.Text))
            If DEL_LED_TYPE = "GODOWN" Then
                cbo_ProcessingName_of_NextProcess.Text = ""
                cbo_ProcessingName_of_NextProcess.Enabled = False
            Else
                cbo_ProcessingName_of_NextProcess.Enabled = True
            End If
        End If

    End Sub

    Private Sub cbo_DeliveryTo_Leave(sender As Object, e As EventArgs) Handles cbo_DeliveryTo.Leave

        If cbo_DeliveryTo.Tag <> cbo_DeliveryTo.Text Then
            Dim DEL_LED_TYPE As String = Common_Procedures.get_FieldValue(con, "Ledger_Head", "Ledger_Type", "Ledger_IdNo = " & Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_DeliveryTo.Text))
            If DEL_LED_TYPE = "GODOWN" Then
                cbo_ProcessingName_of_NextProcess.Text = ""
                cbo_ProcessingName_of_NextProcess.Enabled = False
            Else
                cbo_ProcessingName_of_NextProcess.Enabled = True
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
        dgv_Details.CurrentCell.Value = dgtxt_details.Text
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

    Private Sub btn_Close_Selection2_Click(sender As Object, e As EventArgs) Handles btn_Close_Selection2.Click
        Cloth_Invoice_Selection()
    End Sub

    Private Sub cbo_GRID_item_after_NextProcess_Enter(sender As Object, e As EventArgs) Handles cbo_GRID_item_after_NextProcess.Enter
        If FrmLdSTS = True Then Exit Sub
        Debug.Print(cbo_GRID_item_after_NextProcess.Text)
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Cloth_Head", "Cloth_Name", "(Cloth_Type = 'PROCESSED FABRIC')", "(Cloth_Idno = 0)")
        Debug.Print(cbo_GRID_item_after_NextProcess.Text)
        cbo_GRID_item_after_NextProcess.BackColor = Color.Lime
    End Sub

    Private Sub cbo_GRID_item_after_NextProcess_LostFocus(sender As Object, e As EventArgs) Handles cbo_GRID_item_after_NextProcess.LostFocus
        cbo_GRID_item_after_NextProcess.BackColor = Color.White
    End Sub

    Private Sub cbo_GRID_item_after_NextProcess_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_GRID_item_after_NextProcess.KeyDown

        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, sender, Nothing, Nothing, "Cloth_Head", "Cloth_Name", "(Cloth_Type = 'PROCESSED FABRIC')", "(Cloth_Idno = 0)")

        With dgv_Details

            If (e.KeyValue = 38 And sender.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
                .Focus()
                .CurrentCell = .Rows(.CurrentRow.Index).Cells(dgvCol_Details.EXC_SHT_Mtr)
            End If

            If (e.KeyValue = 40 And sender.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
                If .CurrentCell.RowIndex >= .Rows.Count - 1 Then
                    chk_LotComplete.Focus()
                Else
                    .Focus()
                    .CurrentCell = .Rows(.CurrentCell.RowIndex + 1).Cells(dgvCol_Details.PCS)
                End If
            End If

        End With

    End Sub

    Private Sub cbo_GRID_item_after_NextProcess_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_GRID_item_after_NextProcess.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, sender, Nothing, "Cloth_Head", "Cloth_Name", "(Cloth_Type = 'PROCESSED FABRIC')", "(Cloth_Idno = 0)")
        If Asc(e.KeyChar) = 13 Then
            With dgv_Details
                If .CurrentCell.RowIndex >= .Rows.Count - 1 Then
                    chk_LotComplete.Focus()
                Else
                    .Focus()
                    .CurrentCell = .Rows(.CurrentCell.RowIndex + 1).Cells(dgvCol_Details.PCS)
                End If
            End With
        End If
    End Sub

    Private Sub cbo_GRID_item_after_NextProcess_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_GRID_item_after_NextProcess.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Dim f As New Cloth_Creation
            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = sender.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""
            f.MdiParent = MDIParent1
            f.Show()
        End If
    End Sub

    Private Sub cbo_GRID_item_after_NextProcess_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_GRID_item_after_NextProcess.TextChanged
        Try

            If sender.Visible Then
                With dgv_Details
                    If Val(sender.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = dgvCol_Details.ITEM_after_NextProcess Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(sender.Text)
                    End If
                End With
            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub cbo_ClothSales_OrderCode_forSelection_KeyPress(sender As Object, e As KeyPressEventArgs) Handles cbo_ClothSales_OrderCode_forSelection.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, sender, Nothing, "ClothSales_Order_Head", "ClothSales_OrderCode_forSelection", vCLO_CONDT, "(ClothSales_Order_Code = '999999/00-00')")
    End Sub

    Private Sub cbo_ClothSales_OrderCode_forSelection_KeyDown(sender As Object, e As KeyEventArgs) Handles cbo_ClothSales_OrderCode_forSelection.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, sender, txt_JobNo, Nothing, "ClothSales_Order_Head", "ClothSales_OrderCode_forSelection", vCLO_CONDT, "(ClothSales_Order_Code = '999999/00-00')")
    End Sub

    Private Sub cbo_ClothSales_OrderCode_forSelection_Enter(sender As Object, e As EventArgs) Handles cbo_ClothSales_OrderCode_forSelection.Enter
        vCLO_CONDT = "(ClothSales_Order_Code LIKE '%/" & Trim(FnYearCode1) & "' or ClothSales_Order_Code LIKE '%/" & Trim(FnYearCode2) & "' and Order_Close_Status = 0 )"
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "ClothSales_Order_Head", "ClothSales_OrderCode_forSelection", vCLO_CONDT, "(ClothSales_Order_Code = '999999/00-00')")
    End Sub


    Private Sub Fabric_Processing_Reconsilation_Mtrs_Wgt(ByRef vFabric_Processing_Recons_Type As String)

        Dim Cmd As New SqlClient.SqlCommand
        Dim Da1 As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable

        vFabric_Processing_Recons_Type = ""

        Dim vCLOID As Integer
        vCLOID = Common_Procedures.Cloth_NameToIdNo(con, dgv_Details.Rows(dgv_Details.CurrentCell.RowIndex).Cells(dgvCol_Details.ITEM_RECEIVED).Value)

        Da1 = New SqlClient.SqlDataAdapter("Select * from Cloth_Head Where Cloth_Idno = " & Str(Val(vCLOID)) & " ", con)
        Dt1 = New DataTable
        Da1.Fill(Dt1)

        If Dt1.Rows.Count > 0 Then
            vFabric_Processing_Recons_Type = Dt1.Rows(0).Item("Fabric_Processing_Reconsilation_In_Meter_Weight").ToString
        End If

    End Sub
    Private Sub Set_Max_DetailsSlNo(ByVal RowNo As Integer, ByVal DetSlNo_ColNo As Integer)
        Dim MaxSlNo As Integer = 0
        Dim i As Integer

        With dgv_Details
            For i = 0 To .Rows.Count - 1
                If Val(.Rows(i).Cells(DetSlNo_ColNo).Value) > Val(MaxSlNo) Then
                    MaxSlNo = Val(.Rows(i).Cells(DetSlNo_ColNo).Value)
                End If
            Next
            .Rows(RowNo).Cells(DetSlNo_ColNo).Value = Val(MaxSlNo) + 1
        End With

    End Sub
    Private Sub btn_Save_ShiftMeters_Click(sender As Object, e As EventArgs) Handles btn_Save_ShiftMeters.Click
        Dim cmd As New SqlClient.SqlCommand
        Dim NewCode As String
        Dim vSHFTIDNO As Integer
        Dim Del_Id = 0

        Del_Id = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_DeliveryTo.Text)

        Dim DEL_LED_TYPE As String = Common_Procedures.get_FieldValue(con, "Ledger_Head", "Ledger_Type", "Ledger_IdNo = " & Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_DeliveryTo.Text))

        If Del_Id = 0 Then
            MessageBox.Show("Invalid Delivery To Name", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If cbo_DeliveryTo.Enabled And cbo_DeliveryTo.Visible Then cbo_DeliveryTo.Focus()
            Exit Sub
        End If

        If Trim(txt_PartyDcNo.Text) = "" Then
            MessageBox.Show("Invalid Party DcNo", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If txt_PartyDcNo.Enabled And txt_PartyDcNo.Visible Then txt_PartyDcNo.Focus()
            Exit Sub
        End If

        If Del_Id <> 0 And Trim(UCase(DEL_LED_TYPE)) <> "GODOWN" Then

            NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RecNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        cmd.Connection = con

            cmd.CommandText = "Update Textile_Processing_Delivery_head set Party_Dc_No = '" & Trim(txt_PartyDcNo.Text) & "' , JobOrder_No = '" & Trim(txt_JobNo.Text) & "' Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and ClothProcess_Delivery_Code = '" & Trim(Pk_Condition2) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

        MessageBox.Show("Party DcNo & JobNo Updated Sucessfully!!!", "FOR SAVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Information)

        move_record(lbl_RecNo.Text)

            If dtp_Date.Enabled And dtp_Date.Visible Then dtp_Date.Focus()


        End If

    End Sub

End Class